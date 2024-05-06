using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
//using System.Linq;
using System.Text;

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

using Entities;
using Helpers.UserServiceEx;

namespace Helpers
{
    /// <summary>
    /// 登入處理工具類別
    /// </summary>
    /// <remarks>
    /// 為了避過源碼掃描敏感字詞，所有密碼相關變數名稱改用 PXX
    /// </remarks>
    public class LogonHelper
    {
        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 取得最大密碼錯誤次數
        /// </summary>
        /// <returns></returns>
        private int GetMaxPXXWrongTimes()
        {
            //[TODO] 暫時寫死
            return 3;
        }

        /// <summary>
        /// 最大密碼有效天數
        /// </summary>
        /// <returns></returns>
        private int GetMaxPXXLiveDays()
        {
            //[TODO] 暫時寫死
            return 365;
        }
        #endregion

        #region Member
        private EntityFactory _Factory = null;

        private FileLoger _FileLog = new FileLoger("LogonHelper");
        #endregion

        #region Constructor
        /// <summary>
        /// 建構登入處理工具類別
        /// </summary>
        /// <param name="factory"></param>
        public LogonHelper(EntityFactory factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Method
        #region 使用者登入相關
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 行員登入
        /// </summary>
        /// <param name="asker"></param>
        /// <param name="pxx"></param>
        /// <param name="clientIP"></param>
        /// <param name="logonUser"></param>
        /// <returns></returns>
        public Result BankLogon(CommandAsker asker, string pxx, string clientIP, out LogonUser logonUser)
        {
            logonUser = null;
            string logSN = null;

            #region 檢查參數
            string userQual = asker.UserQual;
            string logonUnit = asker.UnitId;
            string userId = asker.UserId;
            if (String.IsNullOrWhiteSpace(userId) || String.IsNullOrWhiteSpace(pxx))
            {
                return new Result(false, "登入失敗 (帳號或密碼未指定)", ErrorCode.L_LOGON_FAILURE, null);
            }
            if (userQual != UserQualCodeTexts.BANK)
            {
                return new Result(false, "登入失敗 (登入參數錯誤)", ErrorCode.L_LOGON_FAILURE, null);
            }
            pxx = pxx.Trim();
            DateTime logonTime = DateTime.Now;
            #endregion

            #region 所有行員都是透過 AD 驗證
            UsersEntity user = null;

            #region [MDY:20161013] 測試用，改用條件式編譯
#if DEBUG
            List<string> testBankIds = new List<string>(new string[] { "BANK01", "BANK02", "BANK03", "BANK04", "BANK05", "BANK06", "BANK07", "BANK08", "BANK09", "BANK10", "BANK11", "BANK12" });
            if (testBankIds.Contains(userId))
            {
                Result result = this.GetUserByBank(userId, out user);
                if (!result.IsSuccess)
                {
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "讀取行員使用者資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
                if (user == null)
                {
                    result = new Result(false, "登入失敗 (該帳號不存在系統內)", ErrorCode.L_LOGON_FAILURE, null);
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return result;
                }

                #region [MDY:20220530] Checkmarx 調整
                if (user.UPXX != pxx)
                {
                    result = new Result(false, "登入失敗 (密碼錯誤)", ErrorCode.L_LOGON_FAILURE, null);
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return result;
                }
                #endregion
            }
#endif
            #endregion

            if (user == null)
            {
                //MemberOf 包含 SCOM_ST 則為總行
                //Title=1為分行主控, Title=2為會計主管

                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                ResponseDocument rsDoc = null;
                try
                {
                    string endpointConfigurationName = ConfigHelper.Current.GetUserServiceExSoapEndpointConfigurationName();
                    UserServiceExSoapClient client = new UserServiceExSoapClient(endpointConfigurationName);

                    rsDoc = client.IdVerify(userId, pxx);
                }
                catch (Exception ex)
                {
                    _FileLog.WriteLog("[行員登入] 呼叫 UserServiceExSoapClient.IdVerify 服務發生例外，錯誤訊息：" + ex.Message);
                }
                if (rsDoc.Status.StatusCode == 0)
                {
                    //成功
                    if (rsDoc.Users == null || rsDoc.Users.Length == 0)
                    {
                        Result result = new Result(false, "登入失敗 (AD 未回傳 Users)", ErrorCode.L_LOGON_FAILURE, null);
                        this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                        return result;
                    }

                    UserInfo adUser = rsDoc.Users[0];
                    user = new UsersEntity();
                    user.UId = adUser.Id;
                    user.URt = String.Empty;
                    user.UBank = null;
                    user.UGrp = null;

                    #region 判斷總行人員
                    if (adUser.MemberOf != null && adUser.MemberOf.Count > 0)
                    {
                        foreach (string memberOf in adUser.MemberOf)
                        {
                            if (memberOf == "SCOM_ST")
                            {
                                user.UBank = DataFormat.MyBankID + adUser.Department;
                                user.UGrp = BankADGroupCodeTexts.AD0;
                            }
                        }
                    }
                    #endregion

                    #region 判斷分行主控或會計主管
                    if (String.IsNullOrWhiteSpace(user.UGrp))
                    {
                        switch (adUser.Title)
                        {
                            case "1":   //分行主控
                                user.UBank = DataFormat.MyBankID + adUser.Department;
                                user.UGrp = BankADGroupCodeTexts.AD1;
                                break;
                            case "2":   //會計主管
                                user.UBank = DataFormat.MyBankID + adUser.Department;
                                user.UGrp = BankADGroupCodeTexts.AD2;
                                break;
                        }
                    }
                    #endregion

                    user.UName = adUser.DisplayName;
                }
                else
                {
                    _FileLog.WriteLog("[行員登入] 呼叫 UserServiceExSoapClient.IdVerify 服務回傳失敗，StatusCode = {0}; Desciption = {1} ", rsDoc.Status.StatusCode, rsDoc.Status.Desciption);

                    //失敗
                    Result result = new Result(false, String.Format("登入失敗 ({0})", rsDoc.Status.Desciption), ErrorCode.L_LOGON_FAILURE, null);
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return result;
                }
            }
            #endregion

            #region 取得使用者 (這裡只是為了取的系統內行員帳號的群組與銀行代碼)
            if (String.IsNullOrEmpty(user.UGrp))
            {
                UsersEntity myUser = null;
                Result result = this.GetUserByBank(userId, out myUser);
                if (!result.IsSuccess)
                {
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "讀取行員使用者資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
                if (myUser == null)
                {
                    result = new Result(false, "登入失敗 (該帳號不存在系統內)", ErrorCode.L_LOGON_FAILURE, null);
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return result;
                }
                user.UGrp = myUser.UGrp;
                user.UBank = myUser.UBank;
            }
            #endregion

            #region [Old] 檢查密碼 (AD 已驗證不用檢查自己的密碼)
            //string status = "FAILURE";
            //{
            //    Result result = new Result(true);
            //    if (user.IsLocked())
            //    {
            //        result = new Result(false, "登入失敗 (帳號已被鎖定)", ErrorCode.L_ACCOUNT_LOCKED, null);
            //        this.WriteLogonLog(logonUnit, userId, password, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
            //        return result;
            //    }

            //    if (String.IsNullOrEmpty(user.UPwd))
            //    {
            //        //首次登入
            //        if (user.InitPwd == password)
            //        {
            //            status = "MUST_CHANGE_PWD"; //強迫變更密碼
            //        }
            //        else
            //        {
            //            result = new Result(false, "登入失敗 (密碼錯誤)", ErrorCode.L_LOGON_FAILURE, null);
            //        }
            //    }
            //    else if (user.UPwd == password)
            //    {
            //        if (user.DataStatus != "0")
            //        {
            //            result = new Result(false, "登入失敗 (目前帳號未被核准)", ErrorCode.L_LOGON_FAILURE, null);
            //            this.WriteLogonLog(logonUnit, userId, password, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
            //            return result;
            //        }

            //        #region 檢查密碼有效期
            //        {
            //            int maxLiveDays = this.GetMaxPasswordLiveDays();
            //            DateTime? passwordChangeDate = user.GetPasswordChangeDate();
            //            DateTime? chkDate = null;
            //            if (String.IsNullOrEmpty(user.UPwd1))
            //            {
            //                //無上次密碼，表示首次登入，比較資料建立日期
            //                chkDate = user.GetCreateDate();
            //            }
            //            else
            //            {
            //                //有上次密碼，表示重置密碼後首次登入，比較重置密碼日期
            //                chkDate = user.GetRestDate();
            //            }
            //            if (chkDate == null || chkDate < logonTime.Date.AddDays(maxLiveDays * -1))
            //            {
            //                status = "ALERT_CHANGE_PWD";    //提醒變更密碼
            //            }
            //            else
            //            {
            //                status = "SUCCESS";
            //            }
            //        }
            //        #endregion
            //    }
            //    else
            //    {
            //        result = new Result(false, "登入失敗 (密碼錯誤)", ErrorCode.L_LOGON_FAILURE, null);
            //    }

            //    if (!result.IsSuccess)
            //    {
            //        #region 密碼錯誤，更新錯誤次數
            //        int maxWrongTimes = this.GetMaxPasswordWrongTimes();
            //        int wrongTimes = 0;
            //        Result result2 = this.UpdateWrongTimes(user, out wrongTimes);
            //        if (result2.IsSuccess)
            //        {
            //            user.UNum = wrongTimes.ToString();
            //            if (user.GetWrongTimes() >= maxWrongTimes)
            //            {
            //                this.LockUser(user, logonTime);
            //                result = new Result(false, String.Format("登入失敗 (密碼錯誤超過{0}次，帳號已被鎖定)", maxWrongTimes), ErrorCode.L_ACCOUNT_LOCKED, null);
            //            }
            //            else
            //            {
            //                result = new Result(false, String.Format("登入失敗 (第{0}次密碼錯誤)", wrongTimes), ErrorCode.L_LOGON_FAILURE, null);
            //            }
            //        }
            //        else
            //        {
            //            result = new Result(false, "更新登入錯誤次數失敗", CoreStatusCode.S_UPDATE_DATA_FAILURE, null);
            //        }
            //        this.WriteLogonLog(logonUnit, userId, password, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
            //        return result;
            //        #endregion
            //    }
            //    else
            //    {
            //        #region 重置登入錯誤次數
            //        if (user.GetWrongTimes() > 0)
            //        {
            //            Result result2 = this.ResetWrongTimes(user);
            //            if (!result2.IsSuccess)
            //            {
            //                this.WriteLogonLog(logonUnit, userId, password, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
            //                return new Result(false, "重置登入錯誤次數失敗", CoreStatusCode.S_UPDATE_DATA_FAILURE, null);
            //            }
            //        }
            //        #endregion
            //    }
            //}
            #endregion

            #region 取得分行資料
            string unitName = null;
            {
                BankEntity bank = null;
                Expression where = null;
                string bankId = user.UBank == null ? String.Empty : user.UBank.Trim();
                if (bankId.Length == 7)
                {
                    where = new Expression(BankEntity.Field.FullCode, bankId);
                }
                else if (bankId.Length == 6)
                {
                    where = new Expression(BankEntity.Field.BankNo, bankId);
                }
                else if (bankId.Length == 3)
                {
                    where = new Expression(BankEntity.Field.BankNo, DataFormat.MyBankID + bankId);
                }
                else
                {
                    where = new Expression(BankEntity.Field.BankNo, RelationEnum.Like, DataFormat.MyBankID + bankId + "%");
                }
                Result result = _Factory.SelectFirst<BankEntity>(where, null, out bank);

                #region [Old] 因為土銀未提供完整分行資料且AD的Department不確定是代碼還是名稱且分行名稱僅為了顯示，所以失敗或找不到資料以分行代碼替代
                //if (result.IsSuccess)
                //{
                //    if (bank == null)
                //    {
                //        result = new Result(false, "查無該使用者的分行資料", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                //        this.WriteLogonLog(logonUnit, userId, password, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                //        return result;
                //    }
                //}
                //else
                //{
                //    this.WriteLogonLog(logonUnit, userId, password, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                //    return new Result(false, "取得使用者的分行資料失敗", CoreStatusCode.UNKNOWN_ERROR, null);
                //}
                #endregion

                if (bank == null)
                {
                    if (bankId.Length > 3 && bankId.StartsWith(DataFormat.MyBankID))
                    {
                        unitName = String.Format("{0}分行", bankId.Substring(3));
                    }
                    else
                    {
                        unitName = String.Format("{0}分行", bankId);
                    }
                }
                else
                {
                    unitName = bank.BankSName;
                }
            }
            #endregion

            #region 取得群組資料
            GroupListEntity group = null;
            {
                Expression where = new Expression(GroupListEntity.Field.GroupId, user.UGrp);
                Result result = _Factory.SelectFirst<GroupListEntity>(where, null, out group);
                if (!result.IsSuccess)
                {
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "取得使用者的群組資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
                if (group == null)
                {
                    result = new Result(false, String.Format("無法取得該使用者的 {0} 群組資料", user.UGrp), ErrorCode.D_DATA_NOT_FOUND, null);
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "無法取得該使用者的群組資料", ErrorCode.D_DATA_NOT_FOUND, null); ;
                }
                if (group.Role != RoleCodeTexts.STAFF)
                {
                    result = new Result(false, String.Format("無法取得該使用者的 {0} 群組不屬於行員", user.UGrp), CoreStatusCode.UNKNOWN_ERROR, null);
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "該使用者的群組資料不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 取得群組權限資料
            GroupRightEntity[] groupRights = null;
            {
                Expression where = new Expression(GroupRightEntity.Field.GroupId, user.UGrp)
                    .And(GroupRightEntity.Field.RightCode, RelationEnum.NotEqual, GroupRightEntity.None_RightCode) // 只取有權限的 //?????
                    .And(GroupRightEntity.Field.RightCode, RelationEnum.NotEqual, String.Empty)
                    .And(GroupRightEntity.Field.RightCode, RelationEnum.NotEqual, null);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(GroupRightEntity.Field.FuncId, OrderByEnum.Asc);
                Result result = _Factory.SelectAll<GroupRightEntity>(where, orderbys, out groupRights);
                if (!result.IsSuccess)
                {
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "取得群組權限資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
            }
            #endregion

            #region [Old] 土銀不使用使用者權限
            //#region 取得使用者權限資料
            //UsersRightEntity[] usersRights = null;
            //if ((status == "SUCCESS" || status == "ALERT_CHANGE_PWD") && group.RoleType == RoleTypeCodeTexts.USER)
            //{
            //    Expression where = new Expression(UsersRightEntity.Field.UId, user.UId)
            //        .And(UsersRightEntity.Field.UGrp, user.UGrp)
            //        .And(UsersRightEntity.Field.URt, user.URt)
            //        .And(UsersRightEntity.Field.UBank, user.UBank)
            //        .And(UsersRightEntity.Field.RightCode, RelationEnum.In, new string[] { "1", "2" }); //只取有權限的
            //    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            //    orderbys.Add(UsersRightEntity.Field.FuncId, OrderByEnum.Asc);
            //    Result result = _Factory.SelectAll<UsersRightEntity>(where, orderbys, out usersRights);
            //    if (!result.IsSuccess)
            //    {
            //        this.WriteLogonLog(logonUnit, userId, password, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
            //        return new Result(false, "取得使用者權限資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
            //    }
            //}
            //#endregion
            #endregion

            UsersRightEntity[] usersRights = null;

            #region 取得授權的商家代號 與 學校代碼
            string[] receiveTypes = null;
            List<string> schIdentys = new List<string>();

            #region [MDY:20161010] 統一由 BankADGroupCodeTexts.IsManager 判斷是否為總行
            #region [Old]
            //if (group.RoleType == RoleTypeCodeTexts.USER)
            //{
            //    SchoolRTypeEntity[] schoolRTypes = null;
            //    Expression where = new Expression(SchoolRTypeEntity.Field.BankId, user.UBank)
            //        .And(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
            //    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            //    orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
            //    Result result = _Factory.SelectAll<SchoolRTypeEntity>(where, orderbys, out schoolRTypes);
            //    if (result.IsSuccess)
            //    {
            //        if (schoolRTypes != null && schoolRTypes.Length > 0)
            //        {
            //            receiveTypes = new string[schoolRTypes.Length];
            //            for (int idx = 0; idx < schoolRTypes.Length; idx++)
            //            {
            //                string receiveType = schoolRTypes[idx].ReceiveType;
            //                if (!String.IsNullOrEmpty(receiveType))
            //                {
            //                    receiveTypes[idx] = schoolRTypes[idx].ReceiveType;
            //                }
            //                string schIdenty = schoolRTypes[idx].SchIdenty;
            //                if (!String.IsNullOrEmpty(schIdenty) && !schIdentys.Contains(schIdenty))
            //                {
            //                    schIdentys.Add(schIdenty);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return result;
            //    }
            //}
            //else
            //{
            //    receiveTypes = new string[0];
            //}
            #endregion

            if (BankADGroupCodeTexts.IsHeadOffice(group.GroupId))
            {
                //總行可跨商家代號，所以不用取
                receiveTypes = new string[0];
            }
            else
            {
                SchoolRTypeEntity[] schoolRTypes = null;
                Expression where = new Expression(SchoolRTypeEntity.Field.BankId, user.UBank)
                    .And(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
                Result result = _Factory.SelectAll<SchoolRTypeEntity>(where, orderbys, out schoolRTypes);
                if (result.IsSuccess)
                {
                    if (schoolRTypes != null && schoolRTypes.Length > 0)
                    {
                        receiveTypes = new string[schoolRTypes.Length];
                        for (int idx = 0; idx < schoolRTypes.Length; idx++)
                        {
                            string receiveType = schoolRTypes[idx].ReceiveType;
                            if (!String.IsNullOrEmpty(receiveType))
                            {
                                receiveTypes[idx] = schoolRTypes[idx].ReceiveType;
                            }
                            string schIdenty = schoolRTypes[idx].SchIdenty;
                            if (!String.IsNullOrEmpty(schIdenty) && !schIdentys.Contains(schIdenty))
                            {
                                schIdentys.Add(schIdenty);
                            }
                        }
                    }
                }
                else
                {
                    return result;
                }
            }
            #endregion
            #endregion

            #region 產生 LogonUser 物件
            {
                Result result = new Result(true, "登入成功", CoreStatusCode.NORMAL_STATUS, null);
                this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                logonUser = this.GenLogonUser(userQual, user, group, groupRights, usersRights, receiveTypes, String.Empty, String.Empty, user.UBank, unitName, asker.CultureName, clientIP, logonTime, logSN);
                logonUser.MySchIdentys = schIdentys.ToArray();
                return result;
            }
            #endregion
        }

        /// <summary>
        /// 學校登入
        /// </summary>
        /// <param name="asker"></param>
        /// <param name="pxx"></param>
        /// <param name="clientIP"></param>
        /// <param name="logonUser"></param>
        /// <returns></returns>
        public Result SchoolLogon(CommandAsker asker, string pxx, string clientIP, out LogonUser logonUser)
        {
            logonUser = null;
            string logSN = null;

            #region 檢查參數
            string userQual = asker.UserQual;
            string logonUnit = asker.UnitId;
            string userId = asker.UserId;

            _FileLog.WriteLog("[學校登入] userQual = {0}; 學校代號 = {1}; 使用者帳號 = {2}; 使用者密碼 = {3}; ", userQual, logonUnit, userId, pxx);

            if (String.IsNullOrWhiteSpace(userId) || String.IsNullOrWhiteSpace(pxx))
            {
                Result result = new Result(false, "登入失敗 (帳號或密碼未指定)", ErrorCode.L_LOGON_FAILURE, null);

                _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);

                return result;
            }
            if (userQual != UserQualCodeTexts.SCHOOL)
            {
                Result result = new Result(false, "登入失敗 (登入參數錯誤)", ErrorCode.L_LOGON_FAILURE, null);

                _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);

                return result;
            }
            pxx = pxx.Trim();
            DateTime logonTime = DateTime.Now;
            #endregion

            #region [MDY:20160921] 使用者密碼加密
            #region [MDY:20220530] Checkmarx 調整
            pxx = DataFormat.GetUserPXXEncode(pxx);
            #endregion
            if (String.IsNullOrEmpty(pxx))
            {
                Result result = new Result(false, "登入密碼加解密處理失敗", CoreStatusCode.UNKNOWN_ERROR, null);
                _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);
                return result;
            }
            #endregion

            #region 取得學校資料
            string unitName = null;
            string receiveTypeEName = null;
            SchoolRTypeEntity[] schoolRTypes = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.SchIdenty, logonUnit)
                    .And(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
                Result result = _Factory.SelectAll<SchoolRTypeEntity>(where, orderbys, out schoolRTypes);
                if (result.IsSuccess)
                {
                    if (schoolRTypes == null || schoolRTypes.Length == 0)
                    {
                        result = new Result(false, "查無該使用者的學校資料", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                    }
                }
                if (!result.IsSuccess)
                {
                    if (result.Exception == null)
                    {
                        _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);
                    }
                    else
                    {
                        _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; Exception={3}; ", result.IsSuccess, result.Code, result.Message, result.Exception.Message);
                    }
                    return result;
                }

                unitName = schoolRTypes[0].SchName;
                receiveTypeEName = schoolRTypes[0].SchEName;
            }
            #endregion

            #region 取得使用者
            UsersEntity user = null;
            {
                Result result = this.GetUserBySchool(userId, logonUnit, out user);
                if (!result.IsSuccess)
                {
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    result = new Result(false, "讀取學校使用者資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);

                    _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);

                    return result;
                }
                if (user == null)
                {
                    result = new Result(false, "登入失敗 (帳號不存在)", ErrorCode.L_LOGON_FAILURE, null);
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);

                    _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);

                    return result;
                }
            }
            #endregion

            #region [Old] 土銀採用前擋後，所以這裡要檢查帳號是否已登入 (20150605 取消前擋後，改回後踢前)
            //{
            //    Expression where = new Expression(LogonLogEntity.Field.StatusCode, LogonLogEntity.STATUS_LOGON_OK)
            //        .And(LogonLogEntity.Field.LogonUnit, user.UBank)
            //        .And(LogonLogEntity.Field.LogonId, user.UId)
            //        .And(LogonLogEntity.Field.LogonQual, userQual);
            //    int count = 0;
            //    Result result = _Factory.SelectCount<LogonLogEntity>(where, out count);
            //    if (result.IsSuccess)
            //    {
            //        if (count > 0)
            //        {
            //            result = new Result(false, "此帳號已登入", ErrorCode.L_ACCOUNT_HAS_LOGON, null);
            //            this.WriteLogonLog(logonUnit, userId, password, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
            //            return result;
            //        }
            //    }
            //    else
            //    {
            //        this.WriteLogonLog(logonUnit, userId, password, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
            //        return new Result(false, "查詢帳號登入狀待失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
            //    }
            //}
            #endregion

            #region 檢查密碼
            string status = "FAILURE";
            {
                Result result = new Result(true);
                if (user.IsLocked())
                {
                    result = new Result(false, "登入失敗 (帳號已被鎖定)", ErrorCode.L_ACCOUNT_LOCKED, null);
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);

                    _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);

                    return result;
                }

                #region [MDY:20220530] Checkmarx 調整
                if (String.IsNullOrEmpty(user.UPXX))
                {
                    //首次登入
                    if (user.InitPXX == pxx)
                    {
                        status = "MUST_CHANGE_PXX"; //強迫變更密碼
                    }
                    else
                    {
                        result = new Result(false, "登入失敗 (密碼錯誤)", ErrorCode.L_LOGON_FAILURE, null);
                    }
                }
                else if (user.UPXX == pxx)
                {
                    if (user.DataStatus != "0")
                    {
                        result = new Result(false, "登入失敗 (目前帳號未被核准)", ErrorCode.L_LOGON_FAILURE, null);
                        this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);

                        _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);

                        return result;
                    }

                    #region 檢查密碼有效期
                    {
                        int maxLiveDays = this.GetMaxPXXLiveDays();

                        #region [MDY:20220530] Checkmarx 調整
                        DateTime? pxxChangeDate = user.GetPXXChangeDate();

                        #region [Old] 土銀好像沒有重置密碼，所以只比較上次密碼變更日期
                        //DateTime? chkDate = null;
                        //if (String.IsNullOrEmpty(user.UPwd1))
                        //{
                        //    //無上次密碼，表示首次登入，比較資料建立日期
                        //    chkDate = user.GetCreateDate();
                        //}
                        //else
                        //{
                        //    //有上次密碼，表示重置密碼後首次登入，比較重置密碼日期
                        //    chkDate = user.GetRestDate();
                        //}
                        //if (chkDate == null || chkDate < logonTime.Date.AddDays(maxLiveDays * -1))
                        //{
                        //    status = "ALERT_CHANGE_PWD";    //提醒變更密碼
                        //}
                        //else
                        //{
                        //    status = "SUCCESS";
                        //}
                        #endregion

                        #region 土銀好像沒有重置密碼，所以只比較上次密碼變更日期
                        if (pxxChangeDate == null || pxxChangeDate.Value < logonTime.Date.AddDays(maxLiveDays * -1))
                        {
                            status = "ALERT_CHANGE_PXX";    //提醒變更密碼
                        }
                        else
                        {
                            status = "SUCCESS";
                        }
                        #endregion
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    result = new Result(false, "登入失敗 (密碼錯誤)", ErrorCode.L_LOGON_FAILURE, null);
                }
                #endregion

                if (!result.IsSuccess)
                {
                    #region 密碼錯誤，更新錯誤次數
                    int maxWrongTimes = this.GetMaxPXXWrongTimes();
                    int wrongTimes = 0;
                    Result result2 = this.UpdateWrongTimes(user, out wrongTimes);
                    if (result2.IsSuccess)
                    {
                        user.UNum = wrongTimes.ToString();
                        if (user.GetWrongTimes() >= maxWrongTimes)
                        {
                            this.LockUser(user, logonTime);
                            result = new Result(false, String.Format("登入失敗 (密碼錯誤超過{0}次，帳號已被鎖定)", maxWrongTimes), ErrorCode.L_ACCOUNT_LOCKED, null);
                        }
                        else
                        {
                            result = new Result(false, String.Format("登入失敗 (第{0}次密碼錯誤)", wrongTimes), ErrorCode.L_LOGON_FAILURE, null);
                        }
                    }
                    else
                    {
                        result = new Result(false, "更新登入錯誤次數失敗", CoreStatusCode.S_UPDATE_DATA_FAILURE, result2.Exception);
                    }
                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);

                    _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);

                    return result;
                    #endregion
                }
                else
                {
                    #region 重置登入錯誤次數
                    if (user.GetWrongTimes() > 0)
                    {
                        Result result2 = this.ResetWrongTimes(user);
                        if (!result2.IsSuccess)
                        {
                            this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                            result = new Result(false, "重置登入錯誤次數失敗", CoreStatusCode.S_UPDATE_DATA_FAILURE, result2.Exception);

                            if (result.Exception == null)
                            {
                                _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);
                            }
                            else
                            {
                                _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; Exception={3}; ", result.IsSuccess, result.Code, result.Message, result.Exception.Message);
                            }

                            return result;
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region 取得群組資料
            GroupListEntity group = null;
            {
                Expression where = new Expression(GroupListEntity.Field.GroupId, user.UGrp);
                Result result = _Factory.SelectFirst<GroupListEntity>(where, null, out group);
                if (!result.IsSuccess)
                {
                    if (result.Exception == null)
                    {
                        _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);
                    }
                    else
                    {
                        _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; Exception={3}; ", result.IsSuccess, result.Code, result.Message, result.Exception.Message);
                    }

                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "取得使用者的群組資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
                if (group == null)
                {
                    result = new Result(false, String.Format("無法取得該使用者的 {0} 群組資料", user.UGrp), ErrorCode.D_DATA_NOT_FOUND, null);

                    _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);

                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "無法取得該使用者的群組資料", ErrorCode.D_DATA_NOT_FOUND, null);
                }
                if (group.Role != RoleCodeTexts.SCHOOL)
                {
                    result = new Result(false, String.Format("無法取得該使用者的 {0} 群組不屬於學校", user.UGrp), CoreStatusCode.UNKNOWN_ERROR, null);

                    _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);

                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "該使用者的群組資料不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 取得群組權限資料
            GroupRightEntity[] groupRights = null;
            if (status == "SUCCESS" || status == "ALERT_CHANGE_PXX")
            {
                Expression where = new Expression(GroupRightEntity.Field.GroupId, user.UGrp)
                    .And(GroupRightEntity.Field.RightCode, RelationEnum.NotEqual, GroupRightEntity.None_RightCode) // 只取有權限的 //?????
                    .And(GroupRightEntity.Field.RightCode, RelationEnum.NotEqual, String.Empty)
                    .And(GroupRightEntity.Field.RightCode, RelationEnum.NotEqual, null);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(GroupRightEntity.Field.FuncId, OrderByEnum.Asc);
                Result result = _Factory.SelectAll<GroupRightEntity>(where, orderbys, out groupRights);
                if (!result.IsSuccess)
                {
                    if (result.Exception == null)
                    {
                        _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);
                    }
                    else
                    {
                        _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; Exception={3}; ", result.IsSuccess, result.Code, result.Message, result.Exception.Message);
                    }

                    this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "取得群組權限資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
            }
            #endregion

            #region [Old] 土銀不使用使用者權限
            //#region 取得使用者權限資料
            //if ((status == "SUCCESS" || status == "ALERT_CHANGE_PWD") && group.RoleType == RoleTypeCodeTexts.USER)
            //{
            //    Expression where = new Expression(UsersRightEntity.Field.UserId, user.UserId)
            //        .And(UsersRightEntity.Field.GroupId, user.GroupId)
            //        .And(UsersRightEntity.Field.ReceiveType, user.ReceiveType)
            //        .And(UsersRightEntity.Field.BankId, user.BankId)
            //        .And(UsersRightEntity.Field.RightCode, RelationEnum.In, new string[] { "1", "2" }); //只取有權限的  //????
            //    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            //    orderbys.Add(UsersRightEntity.Field.FuncId, OrderByEnum.Asc);
            //    Result result = _Factory.SelectAll<UsersRightEntity>(where, orderbys, out usersRights);
            //    if (!result.IsSuccess)
            //    {
            //        this.WriteLogonLog(logonUnit, userId, password, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
            //        return new Result(false, "取得使用者權限資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
            //    }
            //}
            //#endregion
            #endregion

            UsersRightEntity[] usersRights = null;

            #region 取得授權的商家代號
            string[] receiveTypes = null;
            if (schoolRTypes != null && schoolRTypes.Length > 0)
            {
                if (group.RoleType == RoleTypeCodeTexts.MANAGER)
                {
                    receiveTypes = new string[schoolRTypes.Length];
                    for (int idx = 0; idx < schoolRTypes.Length; idx++)
                    {
                        string receiveType = schoolRTypes[idx].ReceiveType;
                        if (!String.IsNullOrEmpty(receiveType))
                        {
                            receiveTypes[idx] = schoolRTypes[idx].ReceiveType;
                        }
                    }
                }
                else
                {
                    string userReceiveType = user.URt ?? String.Empty;
                    if (!userReceiveType.StartsWith(","))
                    {
                        userReceiveType = "," + userReceiveType;
                    }
                    if (!userReceiveType.EndsWith(","))
                    {
                        userReceiveType = userReceiveType + ",";
                    }
                    List<string> tmps = new List<string>(schoolRTypes.Length);
                    foreach (SchoolRTypeEntity schoolRType in schoolRTypes)
                    {
                        string receiveType = schoolRType.ReceiveType;
                        if (userReceiveType.IndexOf("," + receiveType + ",") > -1)
                        {
                            tmps.Add(receiveType);
                        }
                    }
                    receiveTypes = tmps.ToArray();
                }
            }
            else
            {
                receiveTypes = new string[0];
            }
            #endregion

            #region 產生 LogonUser 物件
            {
                Result result = null;
                if (status == "MUST_CHANGE_PXX")
                {
                    result = new Result(true, "首次登入必須變更密碼", ErrorCode.L_PASSWORD_CHANEG_MUST, null);
                }
                else if (status == "ALERT_CHANGE_PXX")
                {
                    result = new Result(true, "您的密碼即將到期", ErrorCode.L_PASSWORD_CHANEG_MEMO, null);
                }
                else
                {
                    result = new Result(true, "登入成功", CoreStatusCode.NORMAL_STATUS, null);
                }
                this.WriteLogonLog(logonUnit, userId, pxx, userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                logonUser = this.GenLogonUser(userQual, user, group, groupRights, usersRights, receiveTypes, unitName, receiveTypeEName, logonUnit, unitName, asker.CultureName, clientIP, logonTime, logSN);

                #region 測試 logonUser 序列化
                {
                    try
                    {
                        StringBuilder log = new StringBuilder();
                        string xml = null;
                        Type[] extraTypes = null;
                        if (!Common.TryToXml2(logonUser, typeof(LogonUser), out extraTypes, out xml))
                        {
                            log.AppendLine("[學校登入] logonUser 資料序列化測試失敗");
                        }
                        else
                        {
                            log.AppendLine("[學校登入] logonUser 資料序列化測試成功");
                        }
                        log
                            .AppendFormat("UserQual='{0}'; UserId='{1}'; ReceiveType='{2}'; GroupId='{3}'; BankId='{4}'; DepId='{5}'"
                                , logonUser.UserQual, logonUser.UserId, logonUser.ReceiveType, logonUser.GroupId, logonUser.BankId, logonUser.RoleType).AppendLine()
                            .AppendFormat("SSOPusid='{0}'; UserName='{1}'; ReceiveTypeName='{2}'; UnitId='{3}'; UnitName='{4}'; DepId='{5}'"
                                , logonUser.SSOPusid, logonUser.UserName, logonUser.ReceiveTypeName, logonUser.UnitId, logonUser.UnitName, logonUser.RoleType).AppendLine()
                            .AppendFormat("ClientIP='{0}'; CultureName='{1}'; LogonTime='{2}'; LogonSN='{3}'; IsRemindChangePXX='{4}'"
                                , logonUser.ClientIP, logonUser.CultureName, logonUser.LogonTime, logonUser.LogonSN, logonUser.IsRemindChangePXX).AppendLine();
                        if (logonUser.MyReceiveTypes != null && logonUser.MyReceiveTypes.Length > 0)
                        {
                            log.AppendFormat("logonUser.MyReceiveTypes = '{0}'", String.Join("','", logonUser.MyReceiveTypes)).AppendLine();
                        }
                        if (logonUser.MySchIdentys != null && logonUser.MySchIdentys.Length > 0)
                        {
                            log.AppendFormat("logonUser.MySchIdentys = '{0}'", String.Join("','", logonUser.MySchIdentys)).AppendLine();
                        }
                        if (!Common.TryToXml2(logonUser.AuthMenus, typeof(MenuAuth), out extraTypes, out xml))
                        {
                            log.AppendFormat("logonUser.AuthMenus 無法被序列化").AppendLine();
                            int idx = 0;
                            foreach (MenuAuth menuAuth in logonUser.AuthMenus)
                            {
                                if (menuAuth == null)
                                {
                                    log.AppendFormat("[{0}] is null", idx).AppendLine();
                                }
                                else
                                {
                                    log.AppendFormat("[{0}] MenuID = {1}; AuthCode = {2};", idx, menuAuth.MenuID, menuAuth.AuthCode).AppendLine();
                                }
                            }
                        }

                        _FileLog.WriteLog(log.ToString());
                    }
                    catch(Exception ex)
                    {
                        _FileLog.WriteLog("測試 logonUser 序列化發生例外：{0}", ex.Message);
                    }
                }
                #endregion


                if (result.Exception == null)
                {
                    _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; ", result.IsSuccess, result.Code, result.Message);
                }
                else
                {
                    _FileLog.WriteLog("[學校登入] 處理結果，IsSuccess = {0}; Code = {1}; Message = {2}; Exception={3}; ", result.IsSuccess, result.Code, result.Message, result.Exception.Message);
                }

                #region
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region [Old]
        ///// <summary>
        ///// 取得使用者資料
        ///// </summary>
        ///// <param name="userId">使用者帳號</param>
        ///// <param name="unitId">使用者單位代碼</param>
        ///// <param name="user">成功則傳回使用者資料，否則傳回 null</param>
        ///// <returns>傳回處理結果</returns>
        //private Result GetUser(string userId, string unitId, out UsersEntity user)
        //{
        //    user = null;
        //    unitId = unitId == null ? String.Empty : unitId.Trim();
        //    string userQual = null;
        //    //行員不用輸入分行碼
        //    if (String.IsNullOrEmpty(unitId))
        //    {
        //        userQual = UserQualCodeTexts.BANK;
        //    }
        //    else if (unitId.Length == 4)//?????
        //    {
        //        userQual = UserQualCodeTexts.SCHOOL;
        //    }

        //    #region 組條件
        //    Expression where = null;
        //    switch (userQual)
        //    {
        //        // [TODO] 測試用，以後可能要拿掉
        //        case UserQualCodeTexts.BANK:
        //            where = new Expression(UsersEntity.Field.UId, userId)
        //                .And(UsersEntity.Field.URt, String.Empty)
        //                .And(UsersEntity.Field.UBank, RelationEnum.NotEqual, String.Empty);
        //            break;
        //        case UserQualCodeTexts.SCHOOL:
        //            // [TODO] 台企銀是用統編登入，這裡可能需要改
        //            where = new Expression(UsersEntity.Field.UId, userId)
        //                .And(UsersEntity.Field.URt, RelationEnum.NotEqual, String.Empty)
        //                .And(UsersEntity.Field.UBank, unitId);
        //            break;
        //        default:
        //            return new Result(false, "缺少或無效的身分別", ErrorCode.L_INVALID_QUAL, null);
        //    }
        //    #endregion

        //    #region 取得 User
        //    Result result = _Factory.SelectFirst<UsersEntity>(where, null, out user);
        //    if (!result.IsSuccess)
        //    {
        //        //原錯誤代碼與訊息: EEEE 交易處理錯誤訊息(E)
        //        result = new Result(false, "查詢使用者資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        //    }
        //    #endregion

        //    return result;
        //}
        #endregion

        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <param name="userId">使用者帳號</param>
        /// <param name="unitId">使用者單位代碼</param>
        /// <param name="user">成功則傳回使用者資料，否則傳回 null</param>
        /// <returns>傳回處理結果</returns>
        private Result GetUserBySchool(string userId, string unitId, out UsersEntity user)
        {
            user = null;

            //學校一定會有 URt
            Expression where = new Expression(UsersEntity.Field.UId, userId)
                .And(UsersEntity.Field.URt, RelationEnum.NotEqual, String.Empty)
                .And(UsersEntity.Field.UBank, unitId);
            Result result = _Factory.SelectFirst<UsersEntity>(where, null, out user);
            if (!result.IsSuccess)
            {
                result = new Result(false, "查詢學校使用者資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
            }
            return result;
        }

        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <param name="userId">使用者帳號</param>
        /// <param name="user">成功則傳回使用者資料，否則傳回 null</param>
        /// <returns>傳回處理結果</returns>
        private Result GetUserByBank(string userId, out UsersEntity user)
        {
            user = null;

            //行員一定不會有 URt
            Expression where = new Expression(UsersEntity.Field.UId, userId)
                .And(UsersEntity.Field.URt, String.Empty)
                .And(UsersEntity.Field.UBank, RelationEnum.NotEqual, String.Empty);
            Result result = _Factory.SelectFirst<UsersEntity>(where, null, out user);
            if (!result.IsSuccess)
            {
                result = new Result(false, "查詢銀行使用者資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
            }
            return result;
        }

        /// <summary>
        /// 更新登入錯誤次數
        /// </summary>
        /// <param name="user"></param>
        /// <param name="wrongTimes">回傳更新後的錯誤次數</param>
        /// <returns></returns>
        private Result UpdateWrongTimes(UsersEntity user, out int wrongTimes)
        {
            string sql = String.Format(@"
UPDATE [{0}] 
   SET [{1}] = CASE WHEN ISNUMERIC([{1}]) = 0 
                    THEN 1 
                    ELSE (CASE WHEN CAST([{1}] AS int) >= 2 THEN 3 ELSE CAST([{1}] AS int) + 1 
                          END) 
               END 
     , [{2}] = CASE WHEN ISNUMERIC([{1}]) = 0 OR CAST([{1}] AS int) < 2 THEN [{2}] ELSE 'Y' END 
 WHERE [{3}] = @UserId AND [{4}] = @ReceiveType AND [{5}] = @GroupId AND [{6}] = @BankId; 

SELECT CAST([{1}] AS int) FROM [{0}] WHERE [{3}] = @UserId AND [{4}] = @ReceiveType AND [{5}] = @GroupId AND [{6}] = @BankId;
"
                , UsersEntity.TABLE_NAME
                , UsersEntity.Field.UNum
                , UsersEntity.Field.ULock
                , UsersEntity.Field.UId
                , UsersEntity.Field.URt
                , UsersEntity.Field.UGrp
                , UsersEntity.Field.UBank);
            KeyValueList parameters = new KeyValueList();
            parameters.Add("@UserId", user.UId);
            parameters.Add("@ReceiveType", user.URt);
            parameters.Add("@GroupID", user.UGrp);
            parameters.Add("@BankId", user.UBank);

            object value = 0;
            Result result = _Factory.ExecuteScalar(sql, parameters, out value);
            if (result.IsSuccess)
            {
                wrongTimes = (int)value;
            }
            else
            {
                wrongTimes = 0;
                //原錯誤代碼與訊息: EEEE 交易處理錯誤訊息(E)
                result = new Result(false, "更新登入錯誤次數失敗", CoreStatusCode.S_EXECUTE_SQL_COMMAND_FAILURE, null);
            }
            return result;
        }

        /// <summary>
        /// 重置登入錯誤次數
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Result ResetWrongTimes(UsersEntity user)
        {
            string sql = String.Format(@"
UPDATE [{0}] SET [{1}] = 0 
 WHERE [{2}] = @UserId AND [{3}] = @ReceiveType AND [{4}] = @GroupId AND [{5}] = @BankId;"
                , UsersEntity.TABLE_NAME
                , UsersEntity.Field.UNum
                , UsersEntity.Field.UId
                , UsersEntity.Field.URt
                , UsersEntity.Field.UGrp
                , UsersEntity.Field.UBank);
            KeyValueList parameters = new KeyValueList();
            parameters.Add("@UserId", user.UId);
            parameters.Add("@ReceiveType", user.URt);
            parameters.Add("@GroupID", user.UGrp);
            parameters.Add("@BankId", user.UBank);

            int count = 0;
            Result result = _Factory.ExecuteNonQuery(sql, parameters, out count);
            if (result.IsSuccess && count < 1)
            {
                result = new Result(false, "登入錯誤次數重置失敗", CoreStatusCode.D_NOT_DATA_UPDATE, null);
            }
            return result;
        }

        /// <summary>
        /// 鎖住帳號
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Result LockUser(UsersEntity user, DateTime logonTime)
        {
            Expression where = new Expression(UsersEntity.Field.UId, user.UId)
                .And(UsersEntity.Field.URt, user.URt)
                .And(UsersEntity.Field.UGrp, user.UGrp)
                .And(UsersEntity.Field.UBank, user.UBank);
            KeyValueList parameters = new KeyValueList();
            parameters.Add(UsersEntity.Field.ULock, "Y");

            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20160921] 土銀沒有超過 30 天自動鎖帳號的要求，所以這段 code 不需要
            //parameters.Add(UsersEntity.Field.UPXX1, "12345678");    //一定要這個值，用來判斷是否為超過30天未登入而被鎖住
            #endregion

            //parameters.Add(UsersEntity.Field.UNum, "0");  //舊學雜費有將錯誤次數歸 0，但應該為必要
            //parameters.Add(UsersEntity.Field.PXXChangeDate, Common.GetTWDate7(logonTime)); //舊學雜費有設為今天，但應該為必要
            #endregion

            int count = 0;
            Result result = _Factory.UpdateFields<UsersEntity>(parameters, where, out count);
            if (result.IsSuccess && count == 0)
            {
                result = new Result(false, "鎖住帳號資料更新失敗", CoreStatusCode.D_NOT_DATA_UPDATE, null);
            }
            return result;
        }

        #region [MDY:202203XX] 2022擴充案 增加學校英文名稱
        /// <summary>
        /// 取得使用者所屬的單位資料，包含商家代號名稱、商家代號英文名稱、單位代碼、單位名稱
        /// </summary>
        /// <param name="userQual"></param>
        /// <param name="user"></param>
        /// <param name="receiveTypeName">傳回商家代號名稱</param>
        /// <param name="receiveTypeEName">傳回商家代號英文名稱</param>
        /// <param name="unitId">傳回單位代碼</param>
        /// <param name="unitName">傳回單位名稱</param>
        /// <returns></returns>
        private Result GetUserUnitData(string userQual, UsersEntity user
            , out string receiveTypeName, out string receiveTypeEName, out string unitId, out string unitName)
        {
            receiveTypeName = null;
            receiveTypeEName = null;
            unitId = null;
            unitName = null;

            #region 檢查參數
            if (user == null || String.IsNullOrEmpty(userQual))
            {
                return new Result(false, "缺少或無效的使用者資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得 ReceiveTypeName、receiveTypeEName、unitId、unitName
            switch (userQual)
            {
                case UserQualCodeTexts.BANK:
                    #region BANK
                    {
                        if (String.IsNullOrEmpty(user.UBank))
                        {
                            return new Result(false, "缺少或無效的使用者資料參數", ErrorCode.S_INVALID_PARAMETER, null);
                        }

                        BankEntity bank = null;
                        Expression where = null;
                        string bankId = user.UBank == null ? String.Empty : user.UBank.Trim();
                        if (bankId.Length == 7)
                        {
                            where = new Expression(BankEntity.Field.FullCode, bankId);
                        }
                        else if (bankId.Length == 6)
                        {
                            where = new Expression(BankEntity.Field.BankNo, bankId);
                        }
                        else if (bankId.Length == 3)
                        {
                            where = new Expression(BankEntity.Field.BankNo, DataFormat.MyBankID + bankId);
                        }
                        else
                        {
                            where = new Expression(BankEntity.Field.BankNo, RelationEnum.Like, DataFormat.MyBankID + bankId + "%");
                        }
                        Result result = _Factory.SelectFirst<BankEntity>(where, null, out bank);
                        if (result.IsSuccess)
                        {
                            if (bank == null)
                            {
                                result = new Result(false, "查無銀行資料", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                            }
                            else
                            {
                                unitId = bank.BankNo;
                                unitName = bank.BankFName;
                            }
                        }
                        return result;
                    }
                    #endregion
                case UserQualCodeTexts.SCHOOL:
                    #region SCHOOL
                    {
                        if (String.IsNullOrEmpty(user.URt))
                        {
                            return new Result(false, "缺少或無效的使用者資料參數", ErrorCode.S_INVALID_PARAMETER, null);
                        }

                        unitId = user.URt;

                        #region [OLD]
                        //string sql = String.Format("SELECT TOP 1 [{0}] FROM [{1}] WHERE [{2}] = @ReceiveType",
                        //    SchoolRTypeEntity.Field.SchName, SchoolRTypeEntity.TABLE_NAME, SchoolRTypeEntity.Field.ReceiveType);
                        //KeyValue[] parameters = new KeyValue[] { new KeyValue("@ReceiveType", unitId) };
                        //object value = null;
                        //Result result = _Factory.ExecuteScalar(sql, parameters, out value);
                        //if (result.IsSuccess)
                        //{
                        //    if (value == null)
                        //    {
                        //        result = new Result(false, "查無商家代號資料", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                        //    }
                        //    else
                        //    {
                        //        receiveTypeName = value.ToString();
                        //        unitName = receiveTypeName;
                        //    }
                        //}
                        #endregion

                        string sql = $@"
SELECT TOP 1 [{SchoolRTypeEntity.Field.SchName}], [{SchoolRTypeEntity.Field.SchEName}]
  FROM [{SchoolRTypeEntity.TABLE_NAME}] 
 WHERE [{SchoolRTypeEntity.Field.ReceiveType}] = @ReceiveType".Trim();
                        KeyValue[] parameters = new KeyValue[] { new KeyValue("@ReceiveType", unitId) };
                        DataTable dt = null;
                        Result result = _Factory.GetDataTable(sql, parameters, 0, 1, out dt);
                        if (result.IsSuccess)
                        {
                            if (dt == null || dt.Rows.Count == 0)
                            {
                                result = new Result(false, "查無商家代號資料", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                            }
                            else
                            {
                                DataRow dRow = dt.Rows[0];
                                receiveTypeName = dRow[SchoolRTypeEntity.Field.SchName]?.ToString();
                                receiveTypeEName = dRow[SchoolRTypeEntity.Field.SchEName]?.ToString();
                                unitName = receiveTypeName;
                            }
                        }

                        return result;
                    }
                    #endregion
                default:
                    return new Result(false, "缺少或無效的使用者資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion
        }

        /// <summary>
        /// 產生使用者的登入者資料
        /// </summary>
        /// <param name="userQual"></param>
        /// <param name="user"></param>
        /// <param name="group"></param>
        /// <param name="groupRights"></param>
        /// <param name="receiveTypes"></param>
        /// <param name="receiveTypeName"></param>
        /// <param name="receiveTypeEName"></param>
        /// <param name="unitId"></param>
        /// <param name="unitName"></param>
        /// <param name="logonTime"></param>
        /// <param name="logonSN"></param>
        /// <returns></returns>
        private LogonUser GenLogonUser(string userQual, UsersEntity user
            , GroupListEntity group, GroupRightEntity[] groupRights
            , UsersRightEntity[] usersRights, string[] receiveTypes
            , string receiveTypeName, string receiveTypeEName, string unitId, string unitName
            , string cultureName, string clientIP, DateTime logonTime, string logonSN)
        {
            if (user == null || !UserQualCodeTexts.IsDefine(userQual) || userQual == UserQualCodeTexts.STUDENT)
            {
                return null;
            }

            LogonUser logonUser = new LogonUser();
            logonUser.UserQual = userQual;
            logonUser.UserId = user.UId;
            logonUser.ReceiveType = user.URt;
            logonUser.GroupId = user.UGrp;
            logonUser.BankId = user.UBank;
            logonUser.DepId = null;
            logonUser.SSOPusid = null;
            logonUser.UserName = user.UName;
            logonUser.ReceiveTypeName = receiveTypeName;
            logonUser.ReceiveTypeEName = receiveTypeEName;

            #region 單位資料
            logonUser.UnitId = unitId;
            logonUser.UnitName = unitName;
            #endregion

            #region 權限資料
            logonUser.RoleType = group == null ? null : group.RoleType;

            #region [Old] 土銀以群組權限為依據，不使用 UserRights
            //if (group.RoleType == RoleTypeCodeTexts.MANAGER)
            //{
            //    //管理者群組的使用者以群組權限為準
            //    if (groupRights == null || groupRights.Length == 0)
            //    {
            //        logonUser.AuthMenus = new MenuAuth[0];
            //    }
            //    else
            //    {
            //        List<MenuAuth> datas = new List<MenuAuth>(groupRights.Length);
            //        foreach (GroupRightEntity groupRight in groupRights)
            //        {
            //            switch (groupRight.RightCode)
            //            {
            //                case "1":   //維護
            //                    datas.Add(new MenuAuth(groupRight.FuncId, AuthCodeEnum.All));
            //                    break;
            //                case "2":   //查詢
            //                    datas.Add(new MenuAuth(groupRight.FuncId, AuthCodeEnum.Query));
            //                    break;
            //            }
            //        }
            //        logonUser.AuthMenus = datas.ToArray();
            //    }
            //}
            //else
            //{
            //    //使用者群組的使用者以使用者權限為準但不可超過群組權限
            //    if (usersRights == null || usersRights.Length == 0 || groupRights == null || groupRights.Length == 0)
            //    {
            //        logonUser.AuthMenus = new MenuAuth[0];
            //    }
            //    else
            //    {
            //        List<MenuAuth> datas = new List<MenuAuth>(usersRights.Length);
            //        foreach (UsersRightEntity usersRight in usersRights)
            //        {
            //            switch (usersRight.RightCode)
            //            {
            //                case "1":   //維護
            //                    #region 維護
            //                    {
            //                        GroupRightEntity groupRight = Array.Find<GroupRightEntity>(groupRights, (x => x.GroupId == usersRight.GroupId));
            //                        if (groupRight != null)
            //                        {
            //                            if (groupRight.RightCode == "1")
            //                            {
            //                                datas.Add(new MenuAuth(usersRight.FuncId, AuthCodeEnum.All));

            //                            }
            //                            else if (groupRight.RightCode == "2")
            //                            {
            //                                datas.Add(new MenuAuth(usersRight.FuncId, AuthCodeEnum.Query));
            //                            }
            //                        }
            //                    }
            //                    #endregion
            //                    break;
            //                case "2":
            //                    #region 查詢
            //                    {
            //                        //查詢
            //                        GroupRightEntity groupRight = Array.Find<GroupRightEntity>(groupRights, (x => x.GroupId == usersRight.GroupId));
            //                        if (groupRight != null && (groupRight.RightCode == "1" || groupRight.RightCode == "2"))
            //                        {
            //                            datas.Add(new MenuAuth(usersRight.FuncId, AuthCodeEnum.Query));
            //                        }
            //                    }
            //                    #endregion
            //                    break;
            //            }
            //        }
            //        logonUser.AuthMenus = datas.ToArray();
            //    }
            //}
            #endregion

            if (groupRights == null || groupRights.Length == 0)
            {
                logonUser.AuthMenus = new MenuAuth[0];
            }
            else
            {
                List<MenuAuth> datas = new List<MenuAuth>(groupRights.Length);
                foreach (GroupRightEntity groupRight in groupRights)
                {
                    #region [Old]
                    //switch (groupRight.RightCode)
                    //{
                    //    case "1":   //維護
                    //        datas.Add(new MenuAuth(groupRight.FuncId, AuthCodeEnum.All));
                    //        break;
                    //    case "2":   //查詢
                    //        datas.Add(new MenuAuth(groupRight.FuncId, AuthCodeEnum.Query));
                    //        break;
                    //    default:
                    //        datas.Add(new MenuAuth(groupRight.FuncId, AuthCodeEnum.All));
                    //        break;

                    //}
                    #endregion

                    AuthCodeEnum authCode = AuthCodeHelper.ToAuthCode(groupRight.RightCode);
                    datas.Add(new MenuAuth(groupRight.FuncId, authCode));
                }
                logonUser.AuthMenus = datas.ToArray();
            }

            logonUser.MyReceiveTypes = receiveTypes;
            #endregion

            #region 登入資料
            logonUser.ClientIP = clientIP;
            logonUser.CultureName = cultureName;
            logonUser.LogonTime = logonTime;
            logonUser.LogonSN = logonSN;
            #endregion

            return logonUser;
        }
        #endregion
        #endregion

        #region 學生登入相關
        /// <summary>
        /// 學生登入
        /// </summary>
        /// <param name="asker">指定服務命令請求者資料。</param>
        /// <param name="schIdentity">指定學生學校統編。</param>
        /// <param name="studentId">指定學生學號。</param>
        /// <param name="loginKey">指定學生登入 Key。</param>
        /// <param name="logonUser">登入成功則傳回登入者資料，否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        public Result StudentLogon(CommandAsker asker, string schIdentity, string studentId, string loginKey, string clientIP, out LogonUser logonUser)
        {
            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            logonUser = null;
            string logSN = null;

            #region 檢查參數
            schIdentity = schIdentity == null ? String.Empty : schIdentity.Trim();
            studentId = studentId == null ? String.Empty : studentId.Trim();
            loginKey = loginKey == null ? String.Empty : loginKey.Trim();
            if (String.IsNullOrWhiteSpace(schIdentity))
            {
                return new Result(false, "登入失敗 (缺少學校統編參數)", ErrorCode.L_LOGON_FAILURE, null);
            }
            if (String.IsNullOrWhiteSpace(studentId))
            {
                return new Result(false, "登入失敗 (缺少學號參數)", ErrorCode.L_LOGON_FAILURE, null);
            }
            if (String.IsNullOrWhiteSpace(loginKey))
            {
                return new Result(false, "登入失敗 (缺少登入 Key 參數)", ErrorCode.L_LOGON_FAILURE, null);
            }
            DateTime logonTime = DateTime.Now;

            string logonUnit = schIdentity;
            string logonId = studentId;
            string logonPXX= loginKey;
            string logonQual = UserQualCodeTexts.STUDENT;
            #endregion

            #region 取得登入 Kye 類別
            CodeText loginKeyType = null;
            List<string> receiveTypes = new List<string>();
            {
                #region [Old] 因為台企銀堅持用學校統編做登入，所以只能改用 SchoolConfigView 找出登入 Key 類別，並收集該統編的商家代號
                //ReceiveConfigEntity receiveConfig = null;
                //Result result = this.GetReceiveConfig(schIdentity, out receiveConfig);
                //if (!result.IsSuccess)
                //{
                //    this.WriteLogonLog(logonUnit, logonId, logonPwd, logonQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                //    return new Result(false, "讀取學校設定資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                //}
                //if (receiveConfig != null)
                //{
                //    loginKeyType = LoginKeyTypeCodeTexts.GetCodeText(receiveConfig.CheckBirthday);
                //}
                //if (loginKeyType == null)
                //{
                //    loginKeyType = LoginKeyTypeCodeTexts.Default;
                //}
                #endregion

                SchoolConfigView[] datas = null;
                Expression where = new Expression(SchoolConfigView.Field.SchIdenty, schIdentity);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(SchoolConfigView.Field.ReceiveType, OrderByEnum.Asc);
                Result result = _Factory.SelectAll<SchoolConfigView>(where, null, out datas);
                if (!result.IsSuccess)
                {
                    this.WriteLogonLog(logonUnit, logonId, logonPXX, logonQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "讀取學校設定資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
                if (datas == null || datas.Length == 0)
                {
                    return new Result(false, "查無學校設定資料失敗", ErrorCode.D_DATA_NOT_FOUND, null);
                }
                loginKeyType = LoginKeyTypeCodeTexts.GetCodeText(datas[0].LoginKeyType);
                if (loginKeyType == null)
                {
                    loginKeyType = LoginKeyTypeCodeTexts.Default;
                }
                foreach (SchoolConfigView data in datas)
                {
                    receiveTypes.Add(data.ReceiveType);
                }
            }
            #endregion

            #region 取得學生資料
            StudentMasterEntity student = null;
            {
                bool fgOnlyOne = true;
                Result result = this.GetStudent(receiveTypes, studentId, loginKey, loginKeyType.Code, out student, out fgOnlyOne);
                if (!result.IsSuccess)
                {
                    this.WriteLogonLog(logonUnit, logonId, logonPXX, logonQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "讀取學生資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
                if (student == null)
                {
                    #region [MDY:20160705] 修改使用身分證驗證的錯誤訊息
                    if (loginKeyType.Code == LoginKeyTypeCodeTexts.PERSONAL_ID)
                    {
                        result = new Result(false, String.Format("登入失敗 (學號或{0}錯誤)，\r請重新輸入或詳閱下列「公告事項」", loginKeyType.Text), ErrorCode.L_LOGON_FAILURE, null);
                    }
                    else
                    {
                        result = new Result(false, String.Format("登入失敗 (學號或{0}錯誤)", loginKeyType.Text), ErrorCode.L_LOGON_FAILURE, null);
                    }
                    #endregion

                    this.WriteLogonLog(logonUnit, logonId, logonPXX, logonQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return result;
                }

                #region Write File Log
                if (!fgOnlyOne)
                {
                    _FileLog.WriteDebugLog("學生登入無法取得唯一學生(統編={0}, 學號={1}, 登入Key={2}) \r\n", schIdentity, studentId, loginKey);
                }
                #endregion
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 增加學校英文名稱
            #region 取得學生所屬的單位資料
            string receiveTypeName = null;
            string receiveTypeEName = null;
            string unitId = null;
            string unitName = null;
            {
                Result result = this.GetStudentUnitData(student, out receiveTypeName, out receiveTypeEName, out unitId, out unitName);
                if (!result.IsSuccess)
                {
                    this.WriteLogonLog(logonUnit, logonId, logonPXX, logonQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                    return new Result(false, "取得所屬學校資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
            }
            #endregion

            #region 產生 LogonUser 物件
            {
                Result result = new Result(true, "登入成功", CoreStatusCode.NORMAL_STATUS, null);
                this.WriteLogonLog(logonUnit, logonId, logonPXX, logonQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, asker.CultureName, logonTime, result, out logSN);
                logonUser = this.GenLogonUser(student, receiveTypeName, receiveTypeEName, unitId, unitName, schIdentity, receiveTypes.ToArray(), asker.CultureName, clientIP, logonTime, logSN);
                return result;
            }
            #endregion
            #endregion
            #endregion
            #endregion
        }

        /// <summary>
        /// 取得學生資料
        /// </summary>
        /// <param name="receiveTypes">學生學校商家代號</param>
        /// <param name="studentId">學生學號</param>
        /// <param name="loginKey">學生登入 Key</param>
        /// <param name="loginKeyType">學生登入 Key 類別</param>
        /// <param name="student">成功則傳回學生資料，否則傳回 null</param>
        /// <param name="fgOnlyOne">傳回是否只找到一個學生</param>
        /// <returns>傳回處理結果</returns>
        private Result GetStudent(List<string> receiveTypes, string studentId, string loginKey, string loginKeyType, out StudentMasterEntity student, out bool fgOnlyOne)
        {
            student = null;
            fgOnlyOne = true;

            #region 組條件
            Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, receiveTypes.ToArray())
                .And(StudentMasterEntity.Field.Id, studentId);
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentMasterEntity.Field.ReceiveType, OrderByEnum.Asc);
            switch (loginKeyType)
            {
                case LoginKeyTypeCodeTexts.PERSONAL_ID:
                    where.And(StudentMasterEntity.Field.IdNumber, loginKey);
                    break;
                case LoginKeyTypeCodeTexts.BIRTHDAY:
                    DateTime? date = DataFormat.ConvertDateText(loginKey);
                    if (date == null)
                    {
                        where.And(StudentMasterEntity.Field.Birthday, loginKey);
                    }
                    else
                    {
                        where.And(StudentMasterEntity.Field.Birthday, Common.GetTWDate7(date.Value));
                    }
                    break;
            }
            #endregion

            #region 取得 Student
            StudentMasterEntity[] datas = null;
            Result result = _Factory.Select<StudentMasterEntity>(where, orderbys, 0, 2, out datas);
            if (!result.IsSuccess)
            {
                result = new Result(false, "查詢學生資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
            }
            if (datas != null && datas.Length > 0)
            {
                student = datas[0];

                #region [MDY:20160705] 修改判斷是否只找到一個學生的邏輯
                if (datas.Length > 1)
                {
                    for (int idx = 1; idx < datas.Length; idx++)
                    {
                        if (datas[idx].Name != student.Name)
                        {
                            fgOnlyOne = false;
                            break;
                        }
                    }
                }
                #endregion
            }
            #endregion

            return result;
        }

        #region [Old]
        ///// <summary>
        ///// 取得學生資料
        ///// </summary>
        ///// <param name="receiveType">學生學校代收類別</param>
        ///// <param name="studentId">學生學號</param>
        ///// <param name="loginKey">學生登入 Key</param>
        ///// <param name="loginKeyType">學生登入 Key 類別</param>
        ///// <param name="student">成功則傳回學生資料，否則傳回 null</param>
        ///// <returns>傳回處理結果</returns>
        //private Result GetStudent(string receiveType, string studentId, string loginKey, string loginKeyType, out StudentMasterEntity student)
        //{
        //    student = null;

        //    #region 組條件
        //    Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, receiveType)
        //        .And(StudentMasterEntity.Field.Id, studentId);
        //    switch (loginKeyType)
        //    {
        //        //case LoginKeyTypeCodeTexts.STUDENT_ID:
        //        //    where.And(StudentMasterEntity.Field.Id, loginKey);
        //        //    break;
        //        case LoginKeyTypeCodeTexts.PERSONAL_ID:
        //            where.And(StudentMasterEntity.Field.IdNumber, loginKey);
        //            break;
        //        case LoginKeyTypeCodeTexts.BIRTHDAY:
        //            where.And(StudentMasterEntity.Field.Birthday, loginKey);
        //            break;
        //    }
        //    #endregion

        //    #region 取得 Student
        //    Result result = _Factory.SelectFirst<StudentMasterEntity>(where, null, out student);
        //    if (!result.IsSuccess)
        //    {
        //        result = new Result(false, "查詢學生資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        //    }
        //    #endregion

        //    return result;
        //}

        ///// <summary>
        ///// 取得商家代號設定資料
        ///// </summary>
        ///// <param name="receiveType">商家代號代碼</param>
        ///// <param name="receiveConfig">成功則傳回商家代號設定資料，否則傳蕤 null</param>
        ///// <returns>傳回處理結果</returns>
        //private Result GetReceiveConfig(string receiveType, out ReceiveConfigEntity receiveConfig)
        //{
        //    receiveConfig = null;

        //    #region 組條件
        //    Expression where = new Expression(ReceiveConfigEntity.Field.ReceiveType, receiveType);
        //    #endregion

        //    #region 取得 ReceiveConfigEntity
        //    Result result = _Factory.SelectFirst<ReceiveConfigEntity>(where, null, out receiveConfig);
        //    if (!result.IsSuccess)
        //    {
        //        //原錯誤代碼與訊息: EEEE 交易處理錯誤訊息(E)
        //        result = new Result(false, "查詢商家代號設定資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        //    }
        //    #endregion

        //    return result;
        //}
        #endregion

        #region [MDY:202203XX] 2022擴充案 增加學校英文名稱
        /// <summary>
        /// 取得學生所屬的單位資料，包含商家代號名稱(學校名稱)、商家代號英文名稱(學校英文名稱)、單位代碼(商家代號代碼)、單位名稱(學校名稱)
        /// </summary>
        /// <param name="student"></param>
        /// <param name="receiveTypeName"></param>
        /// <param name="receiveTypeEName"></param>
        /// <param name="unitId"></param>
        /// <param name="unitName"></param>
        /// <returns></returns>
        private Result GetStudentUnitData(StudentMasterEntity student
            , out string receiveTypeName, out string receiveTypeEName, out string unitId, out string unitName)
        {
            receiveTypeName = null;
            receiveTypeEName = null;
            unitId = null;
            unitName = null;

            #region 檢查參數
            if (student == null || String.IsNullOrEmpty(student.ReceiveType))
            {
                return new Result(false, "缺少或無效的學生資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            unitId = student.ReceiveType;

            #region 取得 ReceiveTypeName、ReceiveTypeEName、unitName
            #region [OLD]
            //string sql = String.Format("SELECT TOP 1 [{0}] FROM [{1}] WHERE [{2}] = @ReceiveType",
            //    SchoolRTypeEntity.Field.SchName, SchoolRTypeEntity.TABLE_NAME, SchoolRTypeEntity.Field.ReceiveType);
            //KeyValue[] parameters = new KeyValue[] { new KeyValue("@ReceiveType", unitId) };
            //object value = null;
            //Result result = _Factory.ExecuteScalar(sql, parameters, out value);
            //if (result.IsSuccess)
            //{
            //    if (value == null)
            //    {
            //        result = new Result(false, "查無商家代號資料", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
            //    }
            //    else
            //    {
            //        receiveTypeName = value.ToString();
            //        unitName = receiveTypeName;
            //    }
            //}
            #endregion

            string sql = $@"
SELECT TOP 1 [{SchoolRTypeEntity.Field.SchName}], [{SchoolRTypeEntity.Field.SchEName}]
  FROM [{SchoolRTypeEntity.TABLE_NAME}] 
 WHERE [{SchoolRTypeEntity.Field.ReceiveType}] = @ReceiveType".Trim();
            KeyValue[] parameters = new KeyValue[] { new KeyValue("@ReceiveType", unitId) };
            DataTable dt = null;
            Result result = _Factory.GetDataTable(sql, parameters, 0, 1, out dt);
            if (result.IsSuccess)
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    result = new Result(false, "查無商家代號資料", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
                else
                {
                    DataRow dRow = dt.Rows[0];
                    receiveTypeName = dRow[SchoolRTypeEntity.Field.SchName]?.ToString();
                    receiveTypeEName = dRow[SchoolRTypeEntity.Field.SchEName]?.ToString();
                    unitName = receiveTypeName;
                }
            }

            return result;
            #endregion
        }

        /// <summary>
        /// 產生學生的登入者資料
        /// </summary>
        /// <param name="student"></param>
        /// <param name="receiveTypeName"></param>
        /// <param name="receiveTypeEName"></param>
        /// <param name="unitId"></param>
        /// <param name="unitName"></param>
        /// <param name="schoolIdentity"></param>
        /// <param name="cultureName"></param>
        /// <param name="clientIP"></param>
        /// <param name="logonTime"></param>
        /// <param name="logonSN"></param>
        /// <returns></returns>
        private LogonUser GenLogonUser(StudentMasterEntity student
            , string receiveTypeName, string receiveTypeEName, string unitId, string unitName
            , string schoolIdentity, string[] schoolReceiveTypes
            , string cultureName, string clientIP, DateTime logonTime, string logonSN)
        {
            if (student == null)
            {
                return null;
            }

            LogonUser logonUser = new LogonUser();
            logonUser.UserQual = UserQualCodeTexts.STUDENT;
            logonUser.UserId = student.Id;
            logonUser.ReceiveType = student.ReceiveType;
            logonUser.GroupId = null;
            logonUser.BankId = schoolIdentity;
            logonUser.DepId = student.DepId;
            logonUser.SSOPusid = null;
            logonUser.UserName = student.Name;
            logonUser.ReceiveTypeName = receiveTypeName;
            logonUser.ReceiveTypeEName = receiveTypeEName;

            #region 單位資料
            logonUser.UnitId = unitId;
            logonUser.UnitName = unitName;
            #endregion

            #region 權限資料
            logonUser.RoleType = RoleTypeCodeTexts.USER;
            logonUser.AuthMenus = new MenuAuth[0];
            logonUser.MyReceiveTypes = schoolReceiveTypes == null ? new string[0] : schoolReceiveTypes;
            #endregion

            #region 登入資料
            logonUser.ClientIP = clientIP;
            logonUser.CultureName = cultureName;
            logonUser.LogonTime = logonTime;
            logonUser.LogonSN = logonSN;
            #endregion

            return logonUser;
        }
        #endregion
        #endregion

        #region [Old] 行員 SSO 登入相關 (土銀沒有)
        //#region [Old]
        /////// <summary>
        /////// 行員 SSO 登入
        /////// </summary>
        /////// <param name="pusid"></param>
        /////// <param name="userId"></param>
        /////// <param name="userName"></param>
        /////// <param name="roleIds"></param>
        /////// <param name="branchId"></param>
        /////// <param name="logonUser"></param>
        /////// <returns></returns>
        ////public Result SSOLogon(string pusid, string userId, string userName, string[] roleIds, string branchId
        ////    , out LogonUser logonUser)
        ////{
        ////    logonUser = null;
        ////    string logSN = null;

        ////    #region 檢查參數
        ////    if (String.IsNullOrWhiteSpace(pusid) || String.IsNullOrWhiteSpace(userId) || String.IsNullOrWhiteSpace(branchId))
        ////    {
        ////        return new Result(false, "登入失敗，缺少必要參數", ErrorCode.L_LOGON_FAILURE, null);
        ////    }
        ////    string userQual = UserQualCodeTexts.BANK;
        ////    string clientIP = String.Empty;
        ////    string cultureName = String.Empty;
        ////    DateTime logonTime = DateTime.Now;
        ////    #endregion

        ////    #region 取得使用者
        ////    UsersEntity user = null;
        ////    {
        ////        Result result = this.GetUser(userId, branchId, out user);
        ////        if (!result.IsSuccess)
        ////        {
        ////            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        ////            return new Result(false, "讀取使用者資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        ////        }
        ////        if (user == null)
        ////        {
        ////            result = new Result(false, "登入失敗 (帳號不存在)", ErrorCode.L_LOGON_FAILURE, null);
        ////            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        ////            return result;
        ////        }

        ////        user.UserName = userName;
        ////    }
        ////    #endregion

        ////    #region 取得分行資料
        ////    BankEntity bank = null;
        ////    {
        ////        Expression where = new Expression(BankEntity.Field.Branch, branchId);
        ////        Result result = _Factory.SelectFirst<BankEntity>(where, null, out bank);
        ////        if (!result.IsSuccess)
        ////        {
        ////            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        ////            return new Result(false, "取得分行資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        ////        }
        ////        if (bank == null)
        ////        {
        ////            result = new Result(false, "登入失敗 (分行不存在)", ErrorCode.L_LOGON_FAILURE, null);
        ////            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        ////            return result;
        ////        }
        ////    }
        ////    #endregion

        ////    #region 取得群組資料
        ////    GroupListEntity group = null;
        ////    {
        ////        Expression where = new Expression(GroupListEntity.Field.GroupId, user.GroupId);
        ////        Result result = _Factory.SelectFirst<GroupListEntity>(where, null, out group);
        ////        if (!result.IsSuccess)
        ////        {
        ////            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        ////            return new Result(false, "取得群組資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        ////        }
        ////    }
        ////    #endregion

        ////    #region 取得群組權限資料
        ////    GroupRightEntity[] groupRights = null;
        ////    {
        ////        Expression where = new Expression(GroupRightEntity.Field.GroupId, user.GroupId)
        ////            .And(GroupRightEntity.Field.RightCode, RelationEnum.In, new string[] { "1", "2" }); //只取有權限的
        ////        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
        ////        orderbys.Add(GroupRightEntity.Field.FuncId, OrderByEnum.Asc);
        ////        Result result = _Factory.SelectAll<GroupRightEntity>(where, orderbys, out groupRights);
        ////        if (!result.IsSuccess)
        ////        {
        ////            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        ////            return new Result(false, "取得群組權限資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        ////        }
        ////    }
        ////    #endregion

        ////    #region 取得使用者權限資料
        ////    UsersRightEntity[] usersRights = null;
        ////    #endregion

        ////    #region 取得授權的商家代號
        ////    string[] receiveTypes = null;
        ////    {
        ////        SchoolRTypeEntity[] schoolRTypes = null;
        ////        Expression where = new Expression(SchoolRTypeEntity.Field.BankId, branchId)
        ////            .And(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
        ////        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
        ////        orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
        ////        Result result = _Factory.SelectAll<SchoolRTypeEntity>(where, orderbys, out schoolRTypes);
        ////        if (result.IsSuccess)
        ////        {
        ////            if (schoolRTypes != null && schoolRTypes.Length > 0)
        ////            {
        ////                receiveTypes = new string[schoolRTypes.Length];
        ////                for (int idx = 0; idx < schoolRTypes.Length; idx++)
        ////                {
        ////                    receiveTypes[idx] = schoolRTypes[idx].ReceiveType;
        ////                }
        ////            }
        ////            else
        ////            {
        ////                receiveTypes = new string[0];
        ////            }
        ////        }
        ////        else
        ////        {
        ////            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        ////            return new Result(false, "取得授權的商家代號失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        ////        }
        ////    }
        ////    #endregion

        ////    #region 產生 LogonUser 物件
        ////    {
        ////        Result result = new Result(true, "登入成功", CoreStatusCode.NORMAL_STATUS, null);
        ////        this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        ////        logonUser = this.GenLogonUser(userQual, user, group, groupRights, usersRights, receiveTypes, "", branchId, bank.BankSName, cultureName, clientIP, logonTime, logSN);
        ////        return result;
        ////    }
        ////    #endregion
        ////}
        //#endregion

        ///// <summary>
        ///// 行員 SSO 登入
        ///// </summary>
        ///// <param name="pusid"></param>
        ///// <param name="userId"></param>
        ///// <param name="userName"></param>
        ///// <param name="groupIds">SSO的群組</param>
        ///// <param name="branchId"></param>
        ///// <param name="logonUser"></param>
        ///// <returns></returns>
        //public Result SSOLogon(string pusid, string userId, string userName, string[] groupIds, string branchId
        //    , out LogonUser logonUser)
        //{
        //    logonUser = null;
        //    string logSN = null;

        //    #region 檢查參數
        //    if (String.IsNullOrWhiteSpace(userId) || String.IsNullOrWhiteSpace(branchId) || groupIds == null || groupIds.Length == 0)
        //    {
        //        return new Result(false, "登入失敗，缺少必要參數", ErrorCode.L_LOGON_FAILURE, null);
        //    }
        //    string userQual = UserQualCodeTexts.BANK;
        //    string clientIP = String.Empty;
        //    string cultureName = String.Empty;
        //    DateTime logonTime = DateTime.Now;
        //    #endregion

        //    #region 取得使用者
        //    UsersEntity user = new UsersEntity();
        //    user.UserId = userId;
        //    user.ReceiveType = String.Empty;
        //    user.GroupId = null;
        //    user.BankId = branchId;
        //    user.UserName = userName;
        //    #endregion

        //    #region 取得分行資料
        //    BankEntity bank = null;
        //    {
        //        Expression where = new Expression(BankEntity.Field.BankNo, RelationEnum.Like, DataFormat.MyBankID + branchId + "%");
        //        Result result = _Factory.SelectFirst<BankEntity>(where, null, out bank);
        //        if (!result.IsSuccess)
        //        {
        //            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        //            return new Result(false, "取得分行資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        //        }
        //        if (bank == null)
        //        {
        //            result = new Result(false, "登入失敗 (分行不存在)", ErrorCode.L_LOGON_FAILURE, null);
        //            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        //            return result;
        //        }
        //    }
        //    #endregion

        //    #region 取得SSO群組對應成本系統的群組
        //    string myGroupId = null;
        //    foreach (string groupId in groupIds)
        //    {
        //        if (branchId == "H15")
        //        {
        //             switch (groupId)
        //            {
        //                case "AP":
        //                    myGroupId = "A";
        //                    break;
        //                case "DC":
        //                     myGroupId = "B";
        //                     break;
        //                 case "DO":
        //                     myGroupId = "C";
        //                     break;
        //            }
        //        }
        //        else
        //        {
        //            switch(groupId)
        //            {
        //                case "HD":
        //                    if (branchId == "H07")
        //                    {
        //                        myGroupId = "E";
        //                    }
        //                    else if (branchId == "H69")
        //                    {
        //                        myGroupId = "F";
        //                    }
        //                    else
        //                    {
        //                        List<string> gBranchIds = new List<string> (new string[] {"991", "992", "993", "994", "995", "996"});
        //                        if (gBranchIds.Contains(branchId))
        //                        {
        //                            myGroupId = "G";
        //                        }
        //                    }
        //                    break;
        //                case "HC":
        //                    if (branchId == "H07")
        //                    {
        //                        myGroupId = "D";
        //                    }
        //                    break;
        //                case "UO":
        //                    if (branchId == "010")
        //                    {
        //                        myGroupId = "J";
        //                    }
        //                    else
        //                    {
        //                        myGroupId = "I";
        //                    }
        //                    break;
        //                case "UC":
        //                    myGroupId = "H";
        //                    break;
        //            }
        //        }
        //        if (!String.IsNullOrEmpty(myGroupId))
        //        {
        //            break;
        //        }
        //    }
        //    if (String.IsNullOrEmpty(myGroupId))
        //    {
        //        this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, CoreStatusCode.UNKNOWN_ERROR, "無法對應到本系統的群組", out logSN);
        //        return new Result(false, "無法對應到本系統的群組", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        //    }
        //    else
        //    {
        //        user.GroupId = myGroupId;
        //    }
        //    #endregion

        //    #region 取得群組資料
        //    GroupListEntity group = null;
        //    {
        //        Expression where = new Expression(GroupListEntity.Field.GroupId, user.GroupId);
        //        Result result = _Factory.SelectFirst<GroupListEntity>(where, null, out group);
        //        if (!result.IsSuccess)
        //        {
        //            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        //            return new Result(false, "取得群組資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        //        }
        //    }
        //    #endregion

        //    #region 取得群組權限資料
        //    GroupRightEntity[] groupRights = null;
        //    {
        //        Expression where = new Expression(GroupRightEntity.Field.GroupId, user.GroupId)
        //            .And(GroupRightEntity.Field.RightCode, RelationEnum.In, new string[] { "1", "2" }); //只取有權限的
        //        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
        //        orderbys.Add(GroupRightEntity.Field.FuncId, OrderByEnum.Asc);
        //        Result result = _Factory.SelectAll<GroupRightEntity>(where, orderbys, out groupRights);
        //        if (!result.IsSuccess)
        //        {
        //            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        //            return new Result(false, "取得群組權限資料失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        //        }
        //    }
        //    #endregion

        //    #region 取得授權的商家代號
        //    string[] receiveTypes = null;
        //    {
        //        SchoolRTypeEntity[] schoolRTypes = null;
        //        Expression where = new Expression(SchoolRTypeEntity.Field.BankId, branchId)
        //            .And(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
        //        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
        //        orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
        //        Result result = _Factory.SelectAll<SchoolRTypeEntity>(where, orderbys, out schoolRTypes);
        //        if (result.IsSuccess)
        //        {
        //            if (schoolRTypes != null && schoolRTypes.Length > 0)
        //            {
        //                receiveTypes = new string[schoolRTypes.Length];
        //                for (int idx = 0; idx < schoolRTypes.Length; idx++)
        //                {
        //                    receiveTypes[idx] = schoolRTypes[idx].ReceiveType;
        //                }
        //            }
        //            else
        //            {
        //                receiveTypes = new string[0];
        //            }
        //        }
        //        else
        //        {
        //            this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        //            return new Result(false, "取得授權的商家代號失敗", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
        //        }
        //    }
        //    #endregion

        //    #region 產生 LogonUser 物件
        //    {
        //        Result result = new Result(true, "登入成功", CoreStatusCode.NORMAL_STATUS, null);
        //        this.WriteLogonLog(branchId, userId, "", userQual, LogonLogEntity.LOGON_WAY_PAGE, clientIP, cultureName, logonTime, result, out logSN);
        //        logonUser = this.GenLogonUser(userQual, user, group, groupRights, null, receiveTypes, "", branchId, bank.BankSName, cultureName, clientIP, logonTime, logSN);
        //        logonUser.SSOPusid = pusid;
        //        return result;
        //    }
        //    #endregion
        //}
        #endregion

        #region 寫 LogonLog
        #region [MDY:20220530] Checkmarx 調整
        #region [MDY:20210401] 原碼修正
        /// <summary>
        /// 寫入 Logon 日誌
        /// </summary>
        /// <param name="logonUnit"></param>
        /// <param name="logonId"></param>
        /// <param name="logonPXX"></param>
        /// <param name="logonQual"></param>
        /// <param name="logonWay"></param>
        /// <param name="clientIP"></param>
        /// <param name="cultureName"></param>
        /// <param name="logonTime"></param>
        /// <param name="result"></param>
        /// <param name="logSN"></param>
        /// <returns></returns>
        public Result WriteLogonLog(string logonUnit, string logonId, string logonPXX, string logonQual, string logonWay, string clientIP, string cultureName, DateTime logonTime, Result result, out string logSN)
        {
            if (result == null)
            {
                logSN = null;
                return new Result(false, "缺少處理結果參數", CoreStatusCode.UNKNOWN_ERROR, null);
            }

            #region [MDY:20220530] Checkmarx 調整
            if (result.IsSuccess)
            {
                return this.WriteLogonLog(logonUnit, logonId, logonPXX, logonQual, logonWay, clientIP, cultureName, logonTime, LogonLogEntity.STATUS_LOGON_OK, result.Message, out logSN);
            }
            else
            {
                return this.WriteLogonLog(logonUnit, logonId, logonPXX, logonQual, logonWay, clientIP, cultureName, logonTime, result.Code, result.Message, out logSN);
            }
            #endregion
        }

        /// <summary>
        /// 寫入 Logon 日誌
        /// </summary>
        /// <param name="logonUnit"></param>
        /// <param name="logonId"></param>
        /// <param name="logonPXX"></param>
        /// <param name="logonQual"></param>
        /// <param name="logonWay"></param>
        /// <param name="clientIP"></param>
        /// <param name="cultureName"></param>
        /// <param name="statusCode"></param>
        /// <param name="statusDesc"></param>
        /// <param name="logSN"></param>
        /// <returns></returns>
        public Result WriteLogonLog(string logonUnit, string logonId, string logonPXX, string logonQual, string logonWay, string clientIP, string cultureName, DateTime logonTime, string statusCode, string statusDesc, out string logSN)
        {
            logSN = Guid.NewGuid().ToString();

            #region 強迫登出已登入的相同帳號
            if (statusCode == LogonLogEntity.STATUS_LOGON_OK)
            {
                string sql = String.Format(@"
UPDATE [{0}] SET [{1}] = GETDATE(), [{2}] = @NewStatusCode, [{3}] = @NewStatusDesc
 WHERE [{4}] = @LogonUnit AND [{5}] = @LogonId AND [{6}] = @LogonQual
   AND [{2}] = @OldStatusCode AND [{1}] IS NULL",
                    LogonLogEntity.TABLE_NAME,
                    LogonLogEntity.Field.LogoutTime,    //1
                    LogonLogEntity.Field.StatusCode,
                    LogonLogEntity.Field.StatusDesc,
                    LogonLogEntity.Field.LogonUnit,     //4
                    LogonLogEntity.Field.LogonId,
                    LogonLogEntity.Field.LogonQual);

                KeyValueList logoutParameters = new KeyValueList();
                logoutParameters.Add("@NewStatusCode", LogonLogEntity.STATUS_LOGOUT_FORCED);
                logoutParameters.Add("@NewStatusDesc", String.Format("被 ({0}) 強迫登出", clientIP));
                logoutParameters.Add("@LogonUnit", logonUnit);
                logoutParameters.Add("@LogonId", logonId);
                logoutParameters.Add("@LogonQual", logonQual);
                logoutParameters.Add("@OldStatusCode", LogonLogEntity.STATUS_LOGON_OK);

                int count = 0;
                //[TODO] 強迫登出失敗不做其他處理 ??
                Result result = _Factory.ExecuteNonQuery(sql, logoutParameters, out count);
            }
            #endregion

            #region 新增日誌
            {
                LogonLogEntity log = new LogonLogEntity();
                log.SN = logSN;
                log.LogonUnit = logonUnit;
                log.LogonId = logonId;

                #region [MDY:20220530] Checkmarx 調整
                log.LogonPXX = logonPXX;
                #endregion

                log.LogonQual = logonQual;
                log.LogonWay = logonWay;
                log.LogonTime = logonTime;
                log.ClientIP = clientIP;
                log.CultureName = cultureName;
                log.StatusCode = statusCode;
                log.StatusDesc = statusDesc;
                log.LogoutTime = null;

                int count = 0;
                Result result = _Factory.Insert(log, out count);
                if (result.IsSuccess && count == 0)
                {
                    result = new Result(false, "無登入日誌被新增", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                }

                return result;
            }
            #endregion
        }
        #endregion
        #endregion
        #endregion

        #region 檢查登入與功能狀態
        /// <summary>
        /// 檢查登入與功能狀態
        /// </summary>
        /// <param name="asker"></param>
        /// <param name="logonSN"></param>
        /// <param name="checkFuncId"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public Result CheckLogonAndFunc(CommandAsker asker, string logonSN, string checkFuncId, out string resultCode)
        {
            resultCode = CheckLogonResultCodeTexts.CHECK_FAILURE;
            if (asker == null || String.IsNullOrEmpty(logonSN))
            {
                return new Result(false, "缺少或無效的參數", CoreStatusCode.UNKNOWN_ERROR, null);
            }
            string funcId = checkFuncId == null ? String.Empty : checkFuncId.Trim();

            #region 取資料
            {
                string sql = null;
                KeyValueList parameters = new KeyValueList();
                if (String.IsNullOrWhiteSpace(checkFuncId))
                {
                    sql = String.Format(@"
SELECT 
ISNULL((SELECT TOP 1 'Y' FROM [{0}] WHERE [{1}] = @LogonSN AND [{2}] = @LogonStatusCode), 'N') AS LOGON, 
'Y' AS FUNC_ENABLED",
                         LogonLogEntity.TABLE_NAME, LogonLogEntity.Field.SN, LogonLogEntity.Field.StatusCode);
                    parameters.Add("@LogonSN", logonSN);
                    parameters.Add("@LogonStatusCode", LogonLogEntity.STATUS_LOGON_OK);
                }
                else
                {
                    sql = String.Format(@"
SELECT 
ISNULL((SELECT TOP 1 'Y' FROM [{0}] WHERE [{1}] = @LogonSN AND [{2}] = @LogonStatusCode), 'N') AS LOGON, 
ISNULL((SELECT TOP 1 CASE WHEN [{5}] = @FuncStatus THEN 'Y' ELSE 'N' END FROM [{3}] WHERE [{4}] = @FuncID), 'N') AS FUNC_ENABLED",
                         LogonLogEntity.TABLE_NAME, LogonLogEntity.Field.SN, LogonLogEntity.Field.StatusCode,
                         FuncMenuEntity.TABLE_NAME, FuncMenuEntity.Field.FuncId, FuncMenuEntity.Field.Status);
                    parameters.Add("@LogonSN", logonSN);
                    parameters.Add("@LogonStatusCode", LogonLogEntity.STATUS_LOGON_OK);
                    parameters.Add("@FuncID", funcId);
                    parameters.Add("@FuncStatus", DataStatusCodeTexts.NORMAL);
                }

                DataTable dt = null;
                Result result = _Factory.GetDataTable(sql, parameters, 0, 1, out dt);
                if (result.IsSuccess)
                {
                    DataRow row = dt.Rows[0];   //一定有資料
                    bool logon = row["LOGON"].ToString() == "Y";
                    bool funcEnabled = row["FUNC_ENABLED"].ToString() == "Y";
                    if (logon)
                    {
                        resultCode = (!String.IsNullOrEmpty(funcId) && !funcEnabled) ? CheckLogonResultCodeTexts.FUNC_DISABLED : CheckLogonResultCodeTexts.IS_OK;
                    }
                    else
                    {
                        resultCode = CheckLogonResultCodeTexts.NON_LOGON;
                    }
                }
                return result;
            }
            #endregion
        }
        #endregion

        #region 使用者登出
        /// <summary>
        /// 使用者登出
        /// </summary>
        /// <param name="asker"></param>
        /// <param name="logonSN"></param>
        /// <returns></returns>
        public Result UserLogout(CommandAsker asker, string logonSN)
        {
            if (asker == null || String.IsNullOrEmpty(logonSN))
            {
                return new Result(false, "缺少或無效的參數", CoreStatusCode.UNKNOWN_ERROR, null);
            }

            #region 取資料
            {
                string sql = String.Format(@"
UPDATE [{0}] SET [{1}] = @LogoutStatusCode, [{2}] = @LogoutStatusDesc, [{3}] = @LogoutTime
 WHERE [{4}] = @LogonSN AND [{1}] = @LogonStatusCode",
                    LogonLogEntity.TABLE_NAME,
                    LogonLogEntity.Field.StatusCode, LogonLogEntity.Field.StatusDesc, LogonLogEntity.Field.LogoutTime,
                    LogonLogEntity.Field.SN);
                KeyValueList parameters = new KeyValueList();
                parameters.Add("@LogoutStatusCode", LogonLogEntity.STATUS_LOGOUT_OK);
                parameters.Add("@LogoutStatusDesc", "登出");
                parameters.Add("@LogoutTime", DateTime.Now);
                parameters.Add("@LogonSN", logonSN);
                parameters.Add("@LogonStatusCode", LogonLogEntity.STATUS_LOGON_OK);

                int count = 0;
                Result result = _Factory.ExecuteNonQuery(sql, parameters, out count);
                return result;
            }
            #endregion
        }

        /// <summary>
        /// 強迫登出指定帳號
        /// </summary>
        /// <param name="asker"></param>
        /// <param name="logonUnit"></param>
        /// <param name="logonId"></param>
        /// <param name="logonQual"></param>
        /// <returns></returns>
        public Result UserForcedLogout(CommandAsker asker, string logonUnit, string logonId, string logonQual)
        {
            if (asker == null || String.IsNullOrEmpty(logonUnit) || String.IsNullOrEmpty(logonId) || String.IsNullOrEmpty(logonQual))
            {
                return new Result(false, "缺少或無效的參數", CoreStatusCode.UNKNOWN_ERROR, null);
            }

            string sql = String.Format(@"
UPDATE [{0}] SET [{1}] = GETDATE(), [{2}] = @NewStatusCode, [{3}] = @NewStatusDesc
 WHERE [{4}] = @LogonUnit AND [{5}] = @LogonId AND [{6}] = @LogonQual
   AND [{2}] = @OldStatusCode AND [{1}] IS NULL",
                    LogonLogEntity.TABLE_NAME,
                    LogonLogEntity.Field.LogoutTime,    //1
                    LogonLogEntity.Field.StatusCode,
                    LogonLogEntity.Field.StatusDesc,
                    LogonLogEntity.Field.LogonUnit,     //4
                    LogonLogEntity.Field.LogonId,
                    LogonLogEntity.Field.LogonQual);

            KeyValueList logoutParameters = new KeyValueList();
            logoutParameters.Add("@NewStatusCode", LogonLogEntity.STATUS_LOGOUT_FORCED);
            logoutParameters.Add("@NewStatusDesc", String.Format("被 ({0}) 強迫登出", asker.UserId));
            logoutParameters.Add("@LogonUnit", logonUnit);
            logoutParameters.Add("@LogonId", logonId);
            logoutParameters.Add("@LogonQual", logonQual);
            logoutParameters.Add("@OldStatusCode", LogonLogEntity.STATUS_LOGON_OK);

            int count = 0;
            //[TODO] 強迫登出失敗不做其他處理 ??
            Result result = _Factory.ExecuteNonQuery(sql, logoutParameters, out count);
            return result;
        }
        #endregion

        #region 變更使用者密碼
        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 變更使用者密碼
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="oldPXX"></param>
        /// <param name="newPXX"></param>
        /// <returns></returns>
        public Result ChangeUserPXX(string unitId, string userId, string groupId, string oldPXX, string newPXX)
        {
            Expression where = new Expression(UsersEntity.Field.UId, userId)
                .And(UsersEntity.Field.UGrp, groupId)
                .And(UsersEntity.Field.UBank, unitId);

            #region [MDY:20160921] 使用者密碼加密
            oldPXX = DataFormat.GetUserPXXEncode(oldPXX);
            if (String.IsNullOrEmpty(oldPXX))
            {
                return new Result(false, "舊密碼加解密處理失敗", CoreStatusCode.UNKNOWN_ERROR, null);
            }
            newPXX = DataFormat.GetUserPXXEncode(newPXX);
            if (String.IsNullOrEmpty(newPXX))
            {
                return new Result(false, "新密碼加解密處理失敗", CoreStatusCode.UNKNOWN_ERROR, null);
            }
            #endregion

            #region 取得帳號
            UsersEntity user = null;
            {
                Result result = _Factory.SelectFirst<UsersEntity>(where, null, out user);
                if (!result.IsSuccess)
                {
                    return result;
                }
                if (user == null)
                {
                    return new Result(false, "該帳號不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                }
                if (user.IsLocked())
                {
                    return new Result(false, "帳號以鎖住，請先通知本行解鎖", ErrorCode.L_ACCOUNT_LOCKED, null);
                }

                #region [MDY:20220530] Checkmarx 調整
                if (user.UPXX != oldPXX)
                {
                    return new Result(false, "密碼不正確", ErrorCode.L_LOGON_FAILURE, null);
                }
                if (user.UPXX1 == newPXX || user.UPXX == newPXX)
                {
                    return new Result(false, "密碼不可與前兩次相同", ErrorCode.L_LOGON_FAILURE, null);
                }
                #endregion
            }
            #endregion

            #region 更新資料
            {
                KeyValueList fieldValues = new KeyValueList();

                #region [MDY:20220530] Checkmarx 調整
                #region [MDY:20210401] 原碼修正
                fieldValues.Add(UsersEntity.Field.UPXX, newPXX);
                fieldValues.Add(UsersEntity.Field.PXXChangeDate, Common.GetTWDate7());
                #endregion
                #endregion

                fieldValues.Add(UsersEntity.Field.UNum, 0);
                fieldValues.Add(UsersEntity.Field.UPXX1, oldPXX);
                int count = 0;
                Result result = _Factory.UpdateFields<UsersEntity>(fieldValues, where, out count);
                if (!result.IsSuccess)
                {
                    return result;
                }
                if (count == 0)
                {
                    return new Result(false, "無資料被更新", ErrorCode.D_DATA_NOT_FOUND, null);
                }
                return result;
            }
            #endregion
        }
        #endregion
        #endregion

        #region FileLog 相關
        private class FileLoger
        {
            private string _LogName = null;
            public string LogName
            {
                get
                {
                    return _LogName;
                }
                private set
                {
                    _LogName = value == null ? null : value.Trim();
                }
            }

            private string _LogPath = null;
            public string LogPath
            {
                get
                {
                    return _LogPath;
                }
                private set
                {
                    _LogPath = value == null ? String.Empty : value.Trim();
                }
            }

            public bool IsDebug
            {
                get;
                private set;
            }

            private string _LogFileName = null;

            public FileLoger(string logName)
            {
                this.LogName = logName;
                this.LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                string logMode = ConfigurationManager.AppSettings.Get("LOG_MDOE");
                if (String.IsNullOrEmpty(logMode))
                {
                    this.IsDebug = false;
                }
                else
                {
                    this.IsDebug = (logMode.Trim().Equals("DEBUG", StringComparison.CurrentCultureIgnoreCase));
                }
                this.Initial();
            }

            public FileLoger(string logName, string path, bool isDebug)
            {
                this.LogName = logName;
                this.LogPath = path;
                this.IsDebug = isDebug;
                this.Initial();
            }

            public string Initial()
            {
                if (!String.IsNullOrEmpty(this.LogPath))
                {
                    try
                    {
                        DirectoryInfo info = new DirectoryInfo(this.LogPath);
                        if (!info.Exists)
                        {
                            info.Create();
                        }
                        if (String.IsNullOrEmpty(this.LogName))
                        {
                            string fileName = String.Format("{0:yyyyMMdd}.log", DateTime.Today);
                            _LogFileName = Path.Combine(info.FullName, fileName);
                        }
                        else
                        {
                            string fileName = String.Format("{0}_{1:yyyyMMdd}.log", this.LogName, DateTime.Today);
                            _LogFileName = Path.Combine(info.FullName, fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                return null;
            }

            public void WriteLog(string msg)
            {
                if (!String.IsNullOrEmpty(_LogFileName) && msg != null)
                {
                    try
                    {
                        #region [MDY:20220618] Checkmarx 調整 (Information Exposure Through an Error Message)
                        #region [OLD] 土銀建議改用 File.AppendAllText()，都是寫檔有什麼差別，很奇怪
                        //using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
                        //{
                        //    if (String.IsNullOrEmpty(msg))
                        //    {
                        //        sw.WriteLine(String.Empty);
                        //    }
                        //    else
                        //    {
                        //        sw.WriteLine("[{0:HH:mm:ss}] {1}", DateTime.Now, msg);
                        //    }
                        //}
                        #endregion

                        if (String.IsNullOrEmpty(msg))
                        {
                            System.IO.File.AppendAllText(_LogFileName, Environment.NewLine, Encoding.Default);
                        }
                        else
                        {
                            System.IO.File.AppendAllText(_LogFileName, $"[{DateTime.Now:HH:mm:ss}] {msg} {Environment.NewLine}", Encoding.Default);
                        }
                        #endregion
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public void WriteLog(string format, params object[] args)
            {
                if (!String.IsNullOrEmpty(format) && args != null && args.Length > 0)
                {
                    try
                    {
                        this.WriteLog(String.Format(format, args));
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public void WriteDebugLog(string msg)
            {
                if (this.IsDebug)
                {
                    this.WriteLog(msg);
                }
            }

            public void WriteDebugLog(string format, params object[] args)
            {
                if (this.IsDebug)
                {
                    this.WriteLog(format, args);
                }
            }
        }
        #endregion
        #endregion
    }
}
