using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Xml;

using msxml3managed;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Configuration;

namespace Entities
{
    /// <summary>
    /// EAI 處理類別
    /// </summary>
    public sealed class EAIHelper
    {
        #region Member
        /// <summary>
        /// 儲存 EAI 的服務網址
        /// </summary>
        private string _EAIUrl = null;
        /// <summary>
        /// 儲存 EAI 的 SPName 參數值
        /// </summary>
        private string _EAISPName = null;
        /// <summary>
        /// 儲存 EAI 的 CustLoginId 參數值
        /// </summary>
        private string _EAICustLoginId = null;

        /// <summary>
        /// 儲存 EAI 的 Daily Key 的日期
        /// </summary>
        private DateTime? _EAIDailyKeyDate = null;
        /// <summary>
        /// 儲存 EAI 的 Daily Key 的值 (Pswd 參數值)
        /// </summary>
        private string _EAIDailyKey = null;
        #endregion

        #region Property
        private string _ErrMsg = null;
        /// <summary>
        /// 取得最後一次執行方法的錯誤訊息
        /// </summary>
        public string ErrMsg
        {
            get
            {
                return _ErrMsg ?? String.Empty;
            }
        }

        private StringBuilder _Log = null;
        /// <summary>
        /// 取得最後一次執行方法的日誌
        /// </summary>
        public string Log
        {
            get
            {
                return _Log == null ? String.Empty : _Log.ToString();
            }
        }

        /// <summary>
        /// 取得 EAI 的 Daily Key 的日期
        /// </summary>
        public DateTime? EAIDailyKeyDate
        {
            get
            {
                return _EAIDailyKeyDate;
            }
        }
        #endregion

        #region Constructor
        #region [MDY:20181007] 一律取自專案組態的參數
        #region [OLD]
        /////// <summary>
        /////// 建構 EAI 處理類別
        /////// </summary>
        ////public EAIHelper()
        ////{
        ////    _EAIUrl = this.GetEAIUrl();
        ////    _EAISPName = this.GetSPName();
        ////    _EAICustLoginId = this.GetEAICustLoginId();

        ////    _ErrMsg = this.GetEAIDailyKey(out _EAIDailyKeyDate, out _EAIDailyKey);
        ////}

        ///// <summary>
        ///// 建構 EAI 處理類別
        ///// </summary>
        ///// <param name="byProject">指定是否取自專案組態的設定</param>
        //public EAIHelper(bool byProject)
        //{
        //    if (byProject)
        //    {
        //        _EAIUrl = this.GetEAIUrlByProject();
        //        _EAISPName = this.GetSPNameByProject();
        //        _EAICustLoginId = this.GetEAICustLoginIdByProject();

        //        //_ErrMsg = this.GetEAIDailyKey(out _EAIDailyKeyDate, out _EAIDailyKey);
        //    }
        //    else
        //    {
        //        _EAIUrl = this.GetEAIUrl();
        //        _EAISPName = this.GetSPName();
        //        _EAICustLoginId = this.GetEAICustLoginId();

        //        //_ErrMsg = this.GetEAIDailyKey(out _EAIDailyKeyDate, out _EAIDailyKey);
        //    }
        //}

        ////public EAIHelper(string url = null, string spName = null, string loginId = null, string dailyKey = null)
        ////{
        ////    _EAIUrl = url == null ? this.GetEAIUrl() : spName.Trim();
        ////    _EAISPName = spName == null ? this.GetSPName() : spName.Trim();
        ////    _EAICustLoginId = loginId == null ? this.GetEAICustLoginId() : loginId.Trim();
        ////    if (dailyKey == null)
        ////    {
        ////        _ErrMsg = this.GetEAIDailyKey(out _EAIDailyKeyDate, out _EAIDailyKey);
        ////    }
        ////    else
        ////    {
        ////        _EAIDailyKey = dailyKey.Trim();
        ////        _EAIDailyKeyDate = DateTime.Today;
        ////    }
        ////}
        #endregion

        /// <summary>
        /// 建構 EAI 處理類別
        /// </summary>
        public EAIHelper()
        {
            _EAIUrl = this.GetEAIUrl();
            _EAISPName = this.GetSPName();
            _EAICustLoginId = this.GetEAICustLoginId();
        }
        #endregion

        #region [MDY:20181007] 指定伺服器名稱，不同伺服器使用自己的 CustLoginId
        /// <summary>
        /// 建構 EAI 處理類別
        /// </summary>
        /// <param name="machineName"></param>
        public EAIHelper(string machineName)
        {
            _EAIUrl = this.GetEAIUrl();
            _EAISPName = this.GetSPName();
            _EAICustLoginId = this.GetEAICustLoginId(machineName);
        }
        #endregion
        #endregion

        #region Private Method
        private string EncodeKey(string key)
        {
            if (!String.IsNullOrEmpty(key) && key.Length == 16)
            {
                string value = String.Format("00{0}55{1}99", key.Substring(0, 4), key.Substring(4));
                return Common.GetBase64Encode(value);
            }
            return key;
        }

        private string DecodeKey(string value)
        {
            if (!String.IsNullOrEmpty(value) && value.Length == 32)
            {
                value = Common.GetBase64Decode(value);
                if (value.Length == 22)
                {
                    return String.Format("{0}{1}", value.Substring(2,4), value.Substring(8, 12));
                }
            }
            return value;
        }

        #region [MDY:20181007] 一律取自專案組態的參數
        #region [OLD]
        ///// <summary>
        ///// 取得 EAI 的服務網址
        ///// </summary>
        ///// <returns></returns>
        //private string GetEAIUrl()
        //{
        //    string url = ConfigManager.Current.GetSystemConfigValue("EAI", "URL", StringComparison.CurrentCultureIgnoreCase);
        //    //if (String.IsNullOrEmpty(url))
        //    //{
        //    //    //[TODO] 暫時寫死
        //    //    return "http://10.253.23.176/EAI/httppost.ashx";
        //    //}
        //    //else
        //    //{
        //    //    return url;
        //    //}
        //    return url == null ? null : url.Trim();
        //}

        ///// <summary>
        ///// 取得專案組態中 EAI 的服務網址
        ///// </summary>
        ///// <returns></returns>
        //private string GetEAIUrlByProject()
        //{
        //    string url = ConfigHelper.Current.GetMyProjectConfigValue("EAI", "URL", StringComparison.CurrentCultureIgnoreCase);

        //    return url == null ? null : url.Trim();
        //}

        ///// <summary>
        ///// 取得 EAI 的 SPName 參數值
        ///// </summary>
        ///// <returns></returns>
        //private string GetSPName()
        //{
        //    string spname = ConfigManager.Current.GetSystemConfigValue("EAI", "SPName", StringComparison.CurrentCultureIgnoreCase);
        //    //if (String.IsNullOrEmpty(spname))
        //    //{
        //    //    //[TODO] 暫時寫死
        //    //    return "ST";
        //    //}
        //    //else
        //    //{
        //    //    return spname;
        //    //}
        //    return spname == null ? null : spname.Trim();
        //}

        ///// <summary>
        ///// 取得專案組態中 EAI 的 SPName 參數值
        ///// </summary>
        ///// <returns></returns>
        //private string GetSPNameByProject()
        //{
        //    string spname = ConfigHelper.Current.GetMyProjectConfigValue("EAI", "SPName", StringComparison.CurrentCultureIgnoreCase);

        //    return spname == null ? null : spname.Trim();
        //}

        ///// <summary>
        ///// 取得 EAI 的 CustLoginId 參數值
        ///// </summary>
        ///// <returns></returns>
        //private string GetEAICustLoginId()
        //{
        //    string custLoginId = ConfigManager.Current.GetSystemConfigValue("EAI", "CustLoginId", StringComparison.CurrentCultureIgnoreCase);
        //    //if (String.IsNullOrEmpty(custLoginId))
        //    //{
        //    //    //[TODO] 暫時寫死
        //    //    return "SST00ETA11";
        //    //}
        //    //else
        //    //{
        //    //    return custLoginId;
        //    //}
        //    return custLoginId == null ? null : custLoginId.Trim();
        //}

        ///// <summary>
        ///// 取得專案組態中 EAI 的 CustLoginId 參數值
        ///// </summary>
        ///// <returns></returns>
        //private string GetEAICustLoginIdByProject()
        //{
        //    //string custLoginId = ConfigHelper.Current.GetMyProjectConfigValue("EAI", "CustLoginId", StringComparison.CurrentCultureIgnoreCase);

        //    //return custLoginId == null ? null : custLoginId.Trim();

        //    #region [MDY:20160406] 因為EAI每台電腦的 CustLoginId 都不一樣，為了避免 MachineGroup 太多 ConfigName 串上 MachineName
        //    ConfigManager manager = ConfigManager.Current;
        //    string myConfigGroup = manager.GetMyMachineConfigGroup("EAI");
        //    if (myConfigGroup == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        string myConfigName = String.Format("CustLoginId_{0}", Environment.MachineName);
        //        string custLoginId = manager.GetProjectConfigValue(myConfigGroup, myConfigName, StringComparison.CurrentCultureIgnoreCase);
        //        if (custLoginId == null)
        //        {
        //            //沒有 ConfigName 串上 MachineName 的設定就取預設的 ConfigName 設定
        //            custLoginId = manager.GetProjectConfigValue(myConfigGroup, "CustLoginId", StringComparison.CurrentCultureIgnoreCase);
        //        }
        //        return custLoginId;
        //    }
        //    #endregion
        //}
        #endregion

        /// <summary>
        /// 取得專案組態中 EAI 的服務網址
        /// </summary>
        /// <returns></returns>
        private string GetEAIUrl()
        {
            string url = ConfigHelper.Current.GetMyProjectConfigValue("EAI", "URL", StringComparison.CurrentCultureIgnoreCase);

            return url == null ? null : url.Trim();
        }

        /// <summary>
        /// 取得專案組態中 EAI 的 SPName 參數值
        /// </summary>
        /// <returns></returns>
        private string GetSPName()
        {
            string spname = ConfigHelper.Current.GetMyProjectConfigValue("EAI", "SPName", StringComparison.CurrentCultureIgnoreCase);

            return spname == null ? null : spname.Trim();
        }

        #region [MDY:20181007] 可指定伺服器名稱，因為換 KEY 排程要換所有 CustLoginId 的 KEY
        #region [OLD]
        ///// <summary>
        ///// 取得專案組態中 EAI 的 CustLoginId 參數值
        ///// </summary>
        ///// <returns></returns>
        //private string GetEAICustLoginId()
        //{
        //    #region [MDY:20160406] 因為EAI每台電腦的 CustLoginId 都不一樣，為了避免 MachineGroup 太多 ConfigName 串上 MachineName
        //    ConfigManager manager = ConfigManager.Current;
        //    string myConfigGroup = manager.GetMyMachineConfigGroup("EAI");
        //    if (myConfigGroup == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        string myConfigName = String.Format("CustLoginId_{0}", Environment.MachineName);
        //        string custLoginId = manager.GetProjectConfigValue(myConfigGroup, myConfigName, StringComparison.CurrentCultureIgnoreCase);
        //        if (custLoginId == null)
        //        {
        //            //沒有 ConfigName 串上 MachineName 的設定就取預設的 ConfigName 設定
        //            custLoginId = manager.GetProjectConfigValue(myConfigGroup, "CustLoginId", StringComparison.CurrentCultureIgnoreCase);
        //        }
        //        return custLoginId;
        //    }
        //    #endregion
        //}
        #endregion

        /// <summary>
        /// 取得專案組態中指定伺服器名稱的 EAI 的 CustLoginId 參數值
        /// </summary>
        /// <returns></returns>
        private string GetEAICustLoginId(string machineName = null)
        {
            #region [MDY:20160406] 因為EAI每台電腦的 CustLoginId 都不一樣，為了避免 MachineGroup 太多 ConfigName 串上 MachineName
            ConfigManager manager = ConfigManager.Current;
            string myConfigGroup = manager.GetMyMachineConfigGroup("EAI");
            if (myConfigGroup == null)
            {
                return null;
            }
            else
            {
                //沒指定就是本機的 MachineName
                if (String.IsNullOrWhiteSpace(machineName))
                {
                    machineName = Environment.MachineName;
                }
                string myConfigName = String.Format("CustLoginId_{0}",machineName);
                string custLoginId = manager.GetProjectConfigValue(myConfigGroup, myConfigName, StringComparison.CurrentCultureIgnoreCase);
                if (custLoginId == null)
                {
                    //沒有 ConfigName 串上 MachineName 的設定就取預設的 ConfigName 設定
                    custLoginId = manager.GetProjectConfigValue(myConfigGroup, "CustLoginId", StringComparison.CurrentCultureIgnoreCase);
                }
                return custLoginId;
            }
            #endregion
        }
        #endregion
        #endregion

        /// <summary>
        /// 取得 EAI 的 Init Key 參數值
        /// </summary>
        /// <param name="initKey">成功則傳回 EAI 的 Init Key，否則傳回 null</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        private string GetEAIInitKey(out string initKey)
        {
            #region [Old]
            //string initKey = ConfigManager.Current.GetSystemConfigValue("EAI", "InitKey", StringComparison.CurrentCultureIgnoreCase);
            //if (String.IsNullOrEmpty(initKey))
            //{
            //    //[TODO] 暫時寫死
            //    return "000A27D229DCFFF6";
            //}
            //else
            //{
            //    return initKey;
            //}
            #endregion

            initKey = null;
            try
            {
                using (EntityFactory factory = new EntityFactory())
                {
                    ConfigEntity data = null;
                    Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.EAI_INIT_KEY);
                    Result result = factory.SelectFirst<ConfigEntity>(where, null, out data);
                    if (result.IsSuccess)
                    {
                        if (data == null || String.IsNullOrWhiteSpace(data.ConfigValue))
                        {
                            return "系統未設定 EAI 的 Init Key 值";
                        }
                        else
                        {
                            initKey = this.DecodeKey(data.ConfigValue.Trim());
                            return null;
                        }
                    }
                    else
                    {
                        return result.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region [MDY:20181007] 不同 CustLoginId 使用自己的每日的交易 KEY
        private string _EAIDailyKeyConfigKey = null;
        /// <summary>
        /// 取得 EAI 的 Daily Key 的 ConfigKey
        /// </summary>
        /// <returns></returns>
        private string GetEAIDailyKeyConfigKey()
        {
            if (_EAIDailyKeyConfigKey == null)
            {
                _EAIDailyKeyConfigKey = String.Format("{0}_{1}", ConfigKeyCodeTexts.EAI_DAILY_KEY, _EAICustLoginId);
            }
            return _EAIDailyKeyConfigKey;
        }
        #endregion

        /// <summary>
        /// 取得 EAI 的 Daily Key 參數值
        /// </summary>
        /// <param name="keyDate">成功則傳回 EAI 的 Daily Key 的日期，否則傳回 null</param>
        /// <param name="keyValue">成功則傳回 EAI 的 Daily Key 的值，否則傳回 null</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        private  string GetEAIDailyKey(out DateTime? keyDate, out string keyValue)
        {
            keyDate = null;
            keyValue = null;
            try
            {
                using (EntityFactory factory = new EntityFactory())
                {
                    ConfigEntity data = null;

                    #region [MDY:20181007] 不同 CustLoginId 使用自己的每日的交易 KEY
                    #region [OLD]
                    //Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.EDI_DAILY_KEY);
                    #endregion

                    Expression where = new Expression(ConfigEntity.Field.ConfigKey, this.GetEAIDailyKeyConfigKey());
                    #endregion

                    Result result = factory.SelectFirst<ConfigEntity>(where, null, out data);
                    if (result.IsSuccess)
                    {
                        if (data == null || String.IsNullOrWhiteSpace(data.ConfigValue))
                        {
                            return "本日未換 Key";
                        }
                        else
                        {
                            string[] args = data.ConfigValue.Split(new string[] { ":::" }, StringSplitOptions.None);
                            if (args.Length == 2)
                            {
                                DateTime date;
                                if (Common.TryConvertDate8(args[0], out date))
                                {
                                    keyDate = date;
                                    keyValue = this.DecodeKey(args[1].Trim());
                                }
                            }
                            if (keyDate == null || String.IsNullOrEmpty(keyValue))
                            {
                                return "本日未換 Key";
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        return result.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 儲存 EAI 的 Daily Key 參數值
        /// </summary>
        /// <param name="keyDate"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        private string SetEAIDailyKey(DateTime keyDate, string keyValue)
        {
            keyValue = this.EncodeKey(keyValue);
            try
            {
                using (EntityFactory factory = new EntityFactory())
                {
                    ConfigEntity data = null;

                    #region [MDY:20181007] 不同 CustLoginId 使用自己的每日的交易 KEY
                    #region [OLD]
                    //Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.EDI_DAILY_KEY);
                    #endregion

                    Expression where = new Expression(ConfigEntity.Field.ConfigKey, this.GetEAIDailyKeyConfigKey());
                    #endregion

                    Result result = factory.SelectFirst<ConfigEntity>(where, null, out data);
                    if (result.IsSuccess)
                    {
                        string configValue = String.Format("{0}:::{1}", Common.GetDate8(keyDate), keyValue);
                        int count = 0;
                        if (data == null)
                        {
                            data = new ConfigEntity();

                            #region [MDY:20181007] 不同 CustLoginId 使用自己的每日的交易 KEY
                            #region [OLD]
                            //data.ConfigKey = ConfigKeyCodeTexts.EDI_DAILY_KEY;
                            #endregion

                            data.ConfigKey = this.GetEAIDailyKeyConfigKey();
                            #endregion

                            data.ConfigValue = configValue;
                            result = factory.Insert(data, out count);
                            if (result.IsSuccess && count == 0)
                            {
                                result = new Result(false, "無資料被新增", CoreStatusCode.D_NOT_DATA_INSERT, null);
                            }
                        }
                        else
                        {
                            data.ConfigValue = configValue; // String.Format("{0}={1}", Common.GetDate8(keyDate), keyValue);
                            result = factory.Update(data, out count);
                            if (result.IsSuccess && count == 0)
                            {
                                result = new Result(false, "無資料被更新", CoreStatusCode.D_NOT_DATA_INSERT, null);
                            }
                        }
                    }

                    if (result.IsSuccess)
                    {
                        return null;
                    }
                    else
                    {
                        return result.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region [Old]
        ///// <summary>
        ///// 更換 EAI 的 Pswd
        ///// </summary>
        ///// <param name="pswd"></param>
        //public void SetPswd(string pswd)
        //{
        //    _EAIDailtKey = pswd == null ? String.Empty : pswd.Trim();
        //}
        #endregion

        private string CallEAI(string rqXml, out string rsXml)
        {
            rsXml = null;
            string errmsg = null;
            try
            {
                ServerXMLHTTP30 xmlHttp = new ServerXMLHTTP30();
                xmlHttp.setTimeouts(0, 0, 0, 0);

                FreeThreadedDOMDocument30 domDoc = new FreeThreadedDOMDocument30();
                domDoc.loadXML(rqXml);
                xmlHttp.open("POST", _EAIUrl, false, "", "");
                xmlHttp.send(domDoc.xml);
                rsXml = xmlHttp.responseText;
            }
            catch (Exception ex)
            {
                errmsg = String.Format("呼叫 EAI 服務 ({0}) 發生例外，錯誤訊息：{1}", _EAIUrl, ex.Message);
            }
            finally
            {
            }

            return errmsg;
        }

        private string GetEAIHeader(DateTime now)
        {
            string cryptType = "DES1";
            string clientDt = now.ToString("yyyy/MM/dd HH:mm:ss");
            string nowText = now.ToString("yyyyMMddHHmmss");

            #region [MDY:20220910] Checkmarx - Use Of Broken Or Risky Cryptographic Algorithm 誤判調整
            #region [OLD] Checkmarx 不允許用 DESCryptoServiceProvider
            //Encryption myEncryption = new Encryption();
            //string encryPwsd = myEncryption.GetDESEncrypt("F" + nowText + "F", _EAIDailyKey);
            #endregion

            Fuju.Help.LBHelper lbHelper = new Fuju.Help.LBHelper();
            string encryPwsd = lbHelper.GetDESEncrypt("F" + nowText + "F", _EAIDailyKey);
            #endregion

            StringBuilder xml = new StringBuilder();
            xml
                .Append("	<SignonRq>").AppendLine()
                .Append("		<SignonPswd>").AppendLine()
                .Append("			<CustId>").AppendLine()
                .AppendFormat("				<SPName>{0}</SPName>", _EAISPName).AppendLine()
                .AppendFormat("				<CustLoginId>{0}</CustLoginId>", _EAICustLoginId).AppendLine()
                .Append("			</CustId>").AppendLine()
                .Append("			<CustPswd>").AppendLine()
                .AppendFormat("				<CryptType>{0}</CryptType>", cryptType).AppendLine()
                .AppendFormat("				<Pswd>{0}</Pswd>", encryPwsd).AppendLine()
                .AppendFormat("			</CustPswd>").AppendLine()
                .AppendFormat("		</SignonPswd>").AppendLine()
                .AppendFormat("	<ClientDt>{0}</ClientDt>", clientDt).AppendLine()
                .Append("	</SignonRq>").AppendLine();
            return xml.ToString();
        }
        #endregion

        #region [MDY:20160308] EAI 日誌處理
        private string WriteEAILog(EAILogEntity log)
        {
            string errmsg = null;
            try
            {
                #region [MDY:20160705] RsStatusDesc、SendResult 超過欄位長度就剪掉，避免新增失敗
                if (log.RsStatusDesc != null && log.RsStatusDesc.Length > 100)
                {
                    log.RsStatusDesc = log.RsStatusDesc.Substring(0, 100);
                }
                if (log.SendResult != null && log.SendResult.Length > 200)
                {
                    log.SendResult = log.SendResult.Substring(0, 200);
                }
                #endregion

                int count = 0;
                Result result = null;
                using (EntityFactory factory = new EntityFactory())
                {
                    result = factory.Insert(log, out count);
                }
                if (result.IsSuccess)
                {
                    if (count == 0)
                    {
                        errmsg = "儲存 EAI 日誌失敗，錯誤訊息：無資料被新增";
                    }
                }
                else
                {
                    errmsg = String.Concat("儲存 EAI 日誌失敗，錯誤訊息：", result.Message);
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Concat("儲存 EAI 日誌發生例外，錯誤訊息：", ex.Message);
            }
            return errmsg;
        }
        #endregion

        #region IsReady 相關 Method
        /// <summary>
        /// 取得此物件是否準備好
        /// </summary>
        /// <returns></returns>
        public bool IsReady()
        {
            return IsEAIArgsReady() && this.IsEAIDailyKeyReady();
        }

        /// <summary>
        /// 取得 EAI 參數是否準備好
        /// </summary>
        /// <returns></returns>
        public bool IsEAIArgsReady()
        {
            return !String.IsNullOrEmpty(_EAIUrl) && !String.IsNullOrEmpty(_EAISPName) && !String.IsNullOrEmpty(_EAICustLoginId);
        }

        /// <summary>
        /// 取得 EAI Daily Key 是否準備好
        /// </summary>
        /// <returns></returns>
        public bool IsEAIDailyKeyReady()
        {
            if (_EAIDailyKeyDate == null || _EAIDailyKeyDate.Value.Date != DateTime.Today || String.IsNullOrEmpty(_EAIDailyKey))
            {
                _ErrMsg = this.GetEAIDailyKey(out _EAIDailyKeyDate, out _EAIDailyKey);
                if (!String.IsNullOrEmpty(_ErrMsg))
                {
                    return false;
                }
            }
            return _EAIDailyKeyDate != null && _EAIDailyKeyDate.Value.Date == DateTime.Today && !String.IsNullOrEmpty(_EAIDailyKey);
        }
        #endregion

        #region 換 Key
        private string GetChgPswdRq(string initKey, out EAILogEntity eaiLog)
        {
            string cryptType = "DES1";
            DateTime now = DateTime.Now;
            string nowText = now.ToString("yyyyMMddHHmmss");
            string newPW = "";

            #region [MDY:20220910] Checkmarx - Use Of Broken Or Risky Cryptographic Algorithm 誤判調整
            #region [OLD] Checkmarx 不允許用 DESCryptoServiceProvider
            //Encryption myEncryption = new Encryption();
            //string encryPwsd = myEncryption.GetDESEncrypt("F" + nowText + "F", initKey);
            #endregion

            Fuju.Help.LBHelper lbHelper = new Fuju.Help.LBHelper();
            string encryPwsd = lbHelper.GetDESEncrypt("F" + nowText + "F", initKey);
            #endregion

            StringBuilder xml = new StringBuilder();
            xml
                .AppendFormat("<?xml version=\"1.0\" encoding=\"BIG5\" ?>").AppendLine()
                .AppendFormat("<LandBankML xmlns=\"urn:schema-bluestar-com:multichannel\" version=\"1.0\">").AppendLine()
                .AppendFormat("	<SignonRq>").AppendLine()
                .AppendFormat("		<SignonChgPswd>").AppendLine()
                .AppendFormat("			<CustId>").AppendLine()
                .AppendFormat("				<SPName>{0}</SPName>", _EAISPName).AppendLine()
                .AppendFormat("				<CustLoginId>{0}</CustLoginId>", _EAICustLoginId).AppendLine()
                .AppendFormat("			</CustId>").AppendLine()
                .AppendFormat("			<CustChgPswd>").AppendLine()
                .AppendFormat("				<CryptType>{0}</CryptType>", cryptType).AppendLine()
                .AppendFormat("				<OldPswd>{0}</OldPswd>", encryPwsd).AppendLine()
                .AppendFormat("				<NewPswd>{0}</NewPswd>", newPW).AppendLine()
                .AppendFormat("			</CustChgPswd>").AppendLine()
                .AppendFormat("		</SignonChgPswd>").AppendLine()
                .AppendFormat("		<ClientDt>{0:yyyy/MM/dd HH:mm:ss}</ClientDt>", now).AppendLine()
                .AppendFormat("	</SignonRq>").AppendLine()
                .AppendFormat("</LandBankML>");

            #region [MDY:20160308] EAI 日誌處理
            eaiLog = new EAILogEntity(EAILogEntity.KIND_CHANGEKEY, now);
            eaiLog.MsgId = null;
            eaiLog.RqUID = null;
            eaiLog.RqXml = xml.ToString();
            #endregion

            return eaiLog.RqXml;
        }

        private string ParseChgPswdRs(string rsXml, string initKey, out string newPswdData, out string statusCode, out string statusDesc)
        {
            StringBuilder log = new StringBuilder();

            string errmsg = null;
            newPswdData = String.Empty;
            statusCode = String.Empty;
            statusDesc = String.Empty;

            try
            {
                XmlNode node = null;

                rsXml = rsXml.Replace("xmlns=\"urn:schema-bluestar-com:multichannel\"", "");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rsXml);

                #region StatusCode
                {
                    node = xmlDoc.SelectSingleNode("/LandBankML/SignonRs/Status/StatusCode");
                    statusCode = node.InnerText.Trim();
                    log.AppendFormat("Get StatusCode : {0}", statusCode).AppendLine();
                }
                #endregion

                #region StatusDesc
                {
                    node = xmlDoc.SelectSingleNode("/LandBankML/SignonRs/Status/StatusDesc");
                    statusDesc = node.InnerText.Trim();
                    log.AppendFormat("Get StatusDesc : {0}", statusDesc).AppendLine();
                }
                #endregion

                #region NewPswd
                if (statusCode == "0")
                {
                    node = xmlDoc.SelectSingleNode("/LandBankML/SignonRs/NewPswd");
                    string newPswd = node.InnerText.Trim();

                    #region [MDY:20220910] Checkmarx - Use Of Broken Or Risky Cryptographic Algorithm 誤判調整
                    #region [OLD] Checkmarx 不允許用 DESCryptoServiceProvider
                    //Encryption myEncryption = new Encryption();
                    //newPswdData = myEncryption.GedDESDecrypt(newPswd, initKey);
                    #endregion

                    Fuju.Help.LBHelper lbHelper = new Fuju.Help.LBHelper();
                    newPswdData = lbHelper.GetDESDecrypt(newPswd, initKey);
                    #endregion

                    log.AppendFormat("Get NewPswd : {0} ({1})", newPswd, newPswdData).AppendLine();
                }
                else
                {
                    errmsg = String.Format("換 Key 失敗，回傳訊息：StatusCode = {0}; StatusDesc = {1}", statusCode, statusDesc);
                }
                #endregion
            }
            catch (Exception exp)
            {
                errmsg = String.Format("解析換 Key 回傳電文發生例外，錯誤訊息：{0}", exp.Message);
            }

            return errmsg;
        }

        /// <summary>
        /// 發送 EAI 換 Key 電文
        /// </summary>
        /// <param name="newKey">成功則傳回新的 Daily Key，否則傳回 null</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool SendChangeKey(out string newKey)
        {
            newKey = null;
            _ErrMsg = String.Empty;
            _Log = new StringBuilder();

            if (!this.IsEAIArgsReady())
            {
                _ErrMsg = "此物件的 EAI 參數未準備好";
                return false;
            }

            string errmsg = null;

            #region [MDY:20160308] EAI 日誌處理
            EAILogEntity eaiLog = null;
            #endregion

            try
            {
                string initKey = null;
                errmsg = this.GetEAIInitKey(out initKey);
                if (String.IsNullOrEmpty(errmsg))
                {
                    #region 發送換 Key
                    string rsXml = null;
                    {
                        string rqXml = this.GetChgPswdRq(initKey, out eaiLog);
                        _Log
                            .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] Call ChangeKey(initKey:{1}), "
                                , DateTime.Now, initKey)
                            .AppendLine(" Rq: ")
                            .AppendLine(rqXml);
                        errmsg = this.CallEAI(rqXml, out rsXml);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            _Log
                                .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] CallEAI 成功, ", DateTime.Now)
                                .AppendLine(" Rs: ").AppendLine(rsXml);
                        }
                        else
                        {
                            _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] CallEAI 失敗, {1}", DateTime.Now, errmsg).AppendLine();
                        }
                    }
                    #endregion

                    #region [MDY:20160308] EAI 日誌處理
                    eaiLog.RsXml = rsXml;
                    eaiLog.SendResult = errmsg;
                    #endregion

                    #region 處理回傳資料
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        string newPswdData = null;
                        string statusCode = null;
                        string statusDesc = null;
                        errmsg = this.ParseChgPswdRs(rsXml, initKey, out newPswdData, out statusCode, out statusDesc);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 換 Key 成功, newOrgKey = {1}", DateTime.Now, newPswdData).AppendLine();
                            newKey = newPswdData.Substring(0, 16);
                        }
                        else
                        {
                            _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 換 Key 失敗, {1}", DateTime.Now, errmsg).AppendLine();
                        }

                        #region [MDY:20160308] EAI 日誌處理
                        eaiLog.RsStatusCode = statusCode;
                        eaiLog.RsStatusDesc = statusDesc;
                        eaiLog.SendResult = errmsg;
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] CallEAI 失敗, {1}", DateTime.Now, errmsg).AppendLine();
                }
            }
            catch (Exception exp)
            {
                errmsg = String.Format("Call ChangeKey 發生例外, {0}", exp.Message);
                _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] Get Exception : {1}", DateTime.Now, exp.Message).AppendLine();

                #region [MDY:20160308] EAI 日誌處理
                if (eaiLog != null)
                {
                    eaiLog.SendResult = errmsg;
                }
                #endregion
            }
            finally
            {
                #region [MDY:20160308] EAI 日誌處理
                if (eaiLog != null)
                {
                    string errmsg2 = this.WriteEAILog(eaiLog);
                    if (!String.IsNullOrEmpty(errmsg2))
                    {
                        _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg2).AppendLine();
                    }
                }
                #endregion
            }

            _ErrMsg = errmsg;
            return String.IsNullOrEmpty(_ErrMsg);
        }

        /// <summary>
        /// 換 Key 並存入資料庫
        /// </summary>
        /// <returns></returns>
        public bool ChangeDailyKey()
        {
            DateTime keyDate = DateTime.Now;
            string keyValue = null;
            bool isOK = this.SendChangeKey(out keyValue);
            if (isOK)
            {
                _EAIDailyKeyDate = keyDate.Date;
                _EAIDailyKey = keyValue;
                string errmsg = this.SetEAIDailyKey(keyDate, keyValue);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    _ErrMsg = errmsg;
                    _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 儲存 Daily Key 失敗, {1}", DateTime.Now, errmsg).AppendLine();
                }
            }
            return isOK;
        }

        public string EncodEAIInitKey()
        {
            string errmsg = null;
            using (EntityFactory factory = new EntityFactory())
            {
                ConfigEntity data = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.EAI_INIT_KEY);
                Result result = factory.SelectFirst<ConfigEntity>(where, null, out data);
                if (data != null)
                {
                    string encodKey = this.EncodeKey(data.ConfigValue);
                    if (!String.IsNullOrEmpty(encodKey) && data.ConfigValue != encodKey)
                    {
                        data.ConfigValue = encodKey;
                        int count = 0;
                        result = factory.Update(data, out count);
                        if (result.IsSuccess && count == 0)
                        {
                            errmsg = "無法更新回資料庫";
                        }
                    }
                    else
                    {
                        errmsg = "無法加密原 Key";
                    }
                }
                if (!result.IsSuccess)
                {
                    errmsg = result.Message;
                }
            }
            return errmsg;
        }
        #endregion

        #region D00I70
        //public string test_data()
        //{
        //    string dooi70_msg = "";
        //    dooi70_msg = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        //    dooi70_msg += "<LandBankML xmlns=\"urn:schema-bluestar-com:multichannel\">";
        //    dooi70_msg += "<SignonRs>";
        //    dooi70_msg += "  <Status>";
        //    dooi70_msg += "    <StatusCode>0</StatusCode>";
        //    dooi70_msg += "    <Severity>Info</Severity>";
        //    dooi70_msg += "    <StatusDesc>交易完成</StatusDesc>";
        //    dooi70_msg += "    <StatusDetail></StatusDetail>";
        //    dooi70_msg += "  </Status>";
        //    dooi70_msg += "  <CustId>";
        //    dooi70_msg += "    <SPName>EC</SPName>";
        //    dooi70_msg += "    <CustLoginId>SEC00ETA03</CustLoginId>";
        //    dooi70_msg += "  </CustId>";
        //    dooi70_msg += "  <ClientDt>2010/07/28 17:45:23</ClientDt>";
        //    dooi70_msg += "  <ServerDt>2010/07/28 17:45:23</ServerDt>";
        //    dooi70_msg += "</SignonRs>";
        //    dooi70_msg += "<CommMsg>";
        //    dooi70_msg += "  <Status>";
        //    dooi70_msg += "    <StatusCode>0</StatusCode>";
        //    dooi70_msg += "    <Severity>Info</Severity>";
        //    dooi70_msg += "    <StatusDesc>交易完成</StatusDesc>";
        //    dooi70_msg += "    <StatusDetail></StatusDetail>";
        //    dooi70_msg += "  </Status>";
        //    dooi70_msg += "  <TXREC>";
        //    dooi70_msg += "    <BRNO>908</BRNO>";
        //    dooi70_msg += "    <TRMSEQ>33</TRMSEQ>";
        //    dooi70_msg += "    <TXTNO>346</TXTNO>";
        //    dooi70_msg += "    <TTSKID>EC</TTSKID>";
        //    dooi70_msg += "    <TRMTYP>8</TRMTYP>";
        //    dooi70_msg += "    <TXTSK>1</TXTSK>";
        //    dooi70_msg += "    <MSGEND>1</MSGEND>";
        //    dooi70_msg += "    <MTYPE>D</MTYPE>";
        //    dooi70_msg += "    <MSGNO>170</MSGNO>";
        //    dooi70_msg += "    <MSGLNG>193</MSGLNG>";
        //    dooi70_msg += "    <SUBERR>000000</SUBERR>";
        //    dooi70_msg += "    <TXDAY>00990728</TXDAY>";
        //    dooi70_msg += "    <ENDCD>1</ENDCD>";
        //    dooi70_msg += "    <IPADDR>0              </IPADDR>";
        //    dooi70_msg += "    <INQCD>1</INQCD>";
        //    dooi70_msg += "    <APPNO>6016</APPNO>";
        //    dooi70_msg += "    <TXDAY1>00990722</TXDAY1>";
        //    dooi70_msg += "    <STXSEQ>000001</STXSEQ>";
        //    dooi70_msg += "    <ETXSEQ>999999</ETXSEQ>";
        //    dooi70_msg += "    <ENDDT>00990728</ENDDT>";
        //    dooi70_msg += "    <TXDT>0</TXDT>";
        //    dooi70_msg += "    <TXSOURCE>00</TXSOURCE>";
        //    dooi70_msg += "    <KINBR>999</KINBR>";
        //    dooi70_msg += "    <D00I70Rec>";
        //    dooi70_msg += "      <TXSEQ>000001</TXSEQ>";
        //    dooi70_msg += "      <USRNO>0000000003  </USRNO>";
        //    dooi70_msg += "      <PTXDAY>00990722</PTXDAY>";
        //    dooi70_msg += "      <TXTIME>094708</TXTIME>";
        //    dooi70_msg += "      <TXAMT>1.00</TXAMT>";
        //    dooi70_msg += "      <HCODE>0</HCODE>";
        //    dooi70_msg += "      <SOURCE>01</SOURCE>";
        //    dooi70_msg += "      <MAINDT>00990722</MAINDT>";
        //    dooi70_msg += "      <PMTBRNO>041</PMTBRNO>";
        //    dooi70_msg += "      <CSTORE>        </CSTORE>";
        //    dooi70_msg += "      <MEMO>016001-041-0045-D7000-C       </MEMO>";
        //    dooi70_msg += "   </D00I70Rec>";
        //    dooi70_msg += "   <D00I70Rec>";
        //    dooi70_msg += "      <TXSEQ>000002</TXSEQ>";
        //    dooi70_msg += "      <USRNO>0000000004  </USRNO>";
        //    dooi70_msg += "      <PTXDAY>00990722</PTXDAY>";
        //    dooi70_msg += "      <TXTIME>104708</TXTIME>";
        //    dooi70_msg += "      <TXAMT>2.00</TXAMT>";
        //    dooi70_msg += "      <HCODE>0</HCODE>";
        //    dooi70_msg += "      <SOURCE>01</SOURCE>";
        //    dooi70_msg += "      <MAINDT>00990722</MAINDT>";
        //    dooi70_msg += "      <PMTBRNO>041</PMTBRNO>";
        //    dooi70_msg += "      <CSTORE>        </CSTORE>";
        //    dooi70_msg += "      <MEMO>016001-041-0045-D7000-C       </MEMO>";
        //    dooi70_msg += "   </D00I70Rec>";
        //    dooi70_msg += "  </TXREC>";
        //    dooi70_msg += "</CommMsg>";
        //    dooi70_msg += "</LandBankML>";
        //    return dooi70_msg;
        //}

        private string GetD00I70RqMsg(string inqcd, string appno, string txday, string eday, string stxseq, string etxseq, out EAILogEntity eaiLog)
        {
            DateTime now = DateTime.Now;
            string strEAIHeader = this.GetEAIHeader(now);

            #region [MDY:20161124] 改用新電文格式 (多 SUBAPPNO)
            string msgId = "D0070";
            string rqUID = Guid.NewGuid().ToString(); //"52304abb-7137-4cf5-a04c-45f9db6db71a";
            string allRecCtrlIn = "Y";  //"N"
            string subAppNo = "0";      //次應用代碼，學雜費不使用 7300 ~ 7599 的商家代號，所以固定填 0
            string txday1 = "";
            string txsource = "";
            string brno = "";

            StringBuilder xml = new StringBuilder();
            xml
                .Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>").AppendLine()
                .Append("<LandBankML xmlns=\"urn:schema-bluestar-com:multichannel\">").AppendLine()
                .Append(strEAIHeader)
                .Append("	<CommMsg>").AppendLine()
                .AppendFormat("		<SPName>{0}</SPName>", _EAISPName).AppendLine()
                .AppendFormat("		<MsgId>{0}</MsgId>", msgId).AppendLine()
                .AppendFormat("		<RqUID>{0}</RqUID>", rqUID).AppendLine()
                .AppendFormat("		<AllRecCtrlIn>{0}</AllRecCtrlIn>", allRecCtrlIn).AppendLine()
                .AppendFormat("		<INQCD>{0}</INQCD>", inqcd).AppendLine()            //查詢碼
                .AppendFormat("		<APPNO>{0}</APPNO>", appno).AppendLine()            //應用代碼 (商家代號)
                .AppendFormat("		<SUBAPPNO>{0}</SUBAPPNO>", subAppNo).AppendLine()   //次應用代碼，應用代碼為7300~7599時才需輸入，否則置0
                .AppendFormat("		<TXDAY>{0}</TXDAY>", txday).AppendLine()            //查詢起日
                .AppendFormat("		<STXSEQ>{0}</STXSEQ>", stxseq).AppendLine()         //查詢起號
                .AppendFormat("		<ETXSEQ>{0}</ETXSEQ>", etxseq).AppendLine()         //查詢止號
                .AppendFormat("		<EDAY>{0}</EDAY>", eday).AppendLine()               //查詢止日
                .AppendFormat("		<TXDAY1>{0}</TXDAY1>", txday1).AppendLine()         //交易日
                .AppendFormat("		<TXSOURCE>{0}</TXSOURCE>", txsource).AppendLine()   //交易來源
                .AppendFormat("		<BRNO>{0}</BRNO>", brno).AppendLine()               //代收行，預設 999，999為全部
                .Append("	</CommMsg>").AppendLine()
                .Append("</LandBankML>").AppendLine();
            #endregion

            #region [MDY:20160308] EAI 日誌處理
            eaiLog = new EAILogEntity(EAILogEntity.KIND_D0070, now);
            eaiLog.MsgId = msgId;
            eaiLog.RqUID = rqUID;
            eaiLog.RqXml = xml.ToString();
            #endregion

            return eaiLog.RqXml;
        }

        /// <summary>
        /// 發送 EAI 的 D00I70 電文
        /// </summary>
        /// <param name="inqcd"></param>
        /// <param name="appno"></param>
        /// <param name="txday"></param>
        /// <param name="stxseq"></param>
        /// <param name="etxseq"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool SendD00I70(string inqcd, string appno, string txday, string stxseq, string etxseq, out CancelDebtsEntity[] datas)
        {
            datas = null;
            _ErrMsg = String.Empty;
            _Log = new StringBuilder();

            if (!this.IsEAIArgsReady())
            {
                _ErrMsg = "此物件的 EAI 參數未準備好";
                return false;
            }
            if (!this.IsEAIDailyKeyReady())
            {
                if (!this.ChangeDailyKey())
                {
                    _ErrMsg = "自動換 Key 失敗，" + _ErrMsg;
                    return false;
                }
            }

            string errmsg = null;

            #region [MDY:20161124] 檢查是否需輸入次應用代碼，預設學雜費不使用需要次應用代碼的商家代號
            if (DataFormat.CheckHasSubAppNo(appno))
            {
                _ErrMsg = String.Format("appno ({0}) 需要次應用代碼，學雜費不支援", appno);
                return false;
            }
            #endregion

            #region [MDY:20160607] 更正資料改由銷帳時處理
            //const string HCODE = "1";   //更正記號
            #endregion

            #region [MDY:20160308] EAI 日誌處理
            EAILogEntity eaiLog = null;
            #endregion

            try
            {
                #region 發送 D00I70
                string rsXml = null;
                {
                    string rqXml = this.GetD00I70RqMsg(inqcd, appno, txday, txday, stxseq, etxseq, out eaiLog);

                    _Log
                        .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] Call SendD00I70(inqcd:{1}, appno:{2}, txday:{3}, stxseq:{4}, etxseq:{5}) (Key={6})； "
                            , DateTime.Now, inqcd, appno, txday, stxseq, etxseq, _EAIDailyKey)
                        .AppendLine(" Rq: ")
                        .AppendLine(rqXml);
                    errmsg = this.CallEAI(rqXml, out rsXml);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        _Log
                            .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] CallEAI 成功； ", DateTime.Now)
                            .AppendLine(" Rs: ").AppendLine(rsXml);
                    }
                    else
                    {
                        _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] CallEAI 失敗，{1}", DateTime.Now, errmsg).AppendLine();
                    }
                }
                #endregion

                #region [MDY:20160308] EAI 日誌處理
                eaiLog.RsXml = rsXml;
                eaiLog.SendResult = errmsg;
                #endregion

                #region 處理回傳資料
                if (String.IsNullOrEmpty(errmsg))
                {
                    rsXml = rsXml.Replace("xmlns=\"urn:schema-bluestar-com:multichannel\"", "");

                    XmlDocument doc = new XmlDocument();
                    XmlNode node = null;
                    doc.LoadXml(rsXml);

                    #region StatusCode
                    //string StatusCodePath = "/LandBankML/CommMsg/Status/StatusCode";
                    string statusCodePath = "/LandBankML/SignonRs/Status/StatusCode";
                    node = doc.SelectSingleNode(statusCodePath);
                    string statusCode = node.InnerText.Trim();
                    #endregion

                    #region StatusDesc
                    //string StstusDescPath = "/LandBankML/CommMsg/Status/StatusDesc";
                    string statusDescPath = "/LandBankML/SignonRs/Status/StatusDesc";
                    node = doc.SelectSingleNode(statusDescPath);
                    string statusDesc = node.InnerText.Trim();
                    #endregion

                    List<CancelDebtsEntity> list = null;
                    if (statusCode == "0")
                    {
                        #region 欄位說明
                        //TXSEQ   [0]  : 交易序號
                        //USRNO   [1]  : 用戶號碼 (銷編裡的客戶編號)
                        //PTXDAY  [2]  : 交易日期 (代收日期, 異業代收則為行員確認日)
                        //TXTIME  [3]  : 交易時間
                        //TXAMT   [4]  : 交易金額
                        //HCODE   [5]  : 更正記號
                        //SOURCE  [6]  : 交易來源 (代收管道)
                        //MAINDT  [7]  : 作帳日
                        //PMTBRNO [8]  : 繳款行 (3碼)
                        //CSTORE  [9]  : 超商代收處
                        //MEMO    [10] : 備註
                        #endregion

                        string[] dataTags = { "TXSEQ", "USRNO", "PTXDAY", "TXTIME", "TXAMT", "HCODE", "SOURCE", "MAINDT", "PMTBRNO", "CSTORE", "MEMO" };
                        string[] dataVals = new string[dataTags.Length];

                        #region 取得 Details
                        XmlNodeList Details = null;
                        {
                            Details = doc.SelectNodes("/LandBankML/CommMsg/TXREC/Detail");
                        }
                        #endregion

                        #region 逐筆處理
                        if (Details != null && Details.Count > 0)
                        {
                            _Log.AppendFormat("SendD00I70 回傳 {0} 筆 D00I70Rec 資料", Details.Count);

                            list = new List<CancelDebtsEntity>(Details.Count);
                            foreach (XmlNode rec in Details)
                            {
                                for (int idx = 0; idx < dataTags.Length; idx++)
                                {
                                    node = rec.SelectSingleNode(dataTags[idx]);
                                    if (node != null)
                                    {
                                        dataVals[idx] = node.InnerText.Trim();
                                    }
                                    else
                                    {
                                        dataVals[idx] = String.Empty;
                                    }
                                }
                                if (dataVals[0] == "0" || String.IsNullOrEmpty(dataVals[0]) || String.IsNullOrEmpty(dataVals[1]))
                                {
                                    continue;
                                }

                                DateTime date;

                                #region 交易序號
                                int txSeq = 0;
                                int.TryParse(dataVals[0], out txSeq);
                                #endregion

                                #region 用戶編號 (銷編裡的客戶編號)
                                string customNo = dataVals[1].Trim();
                                #endregion

                                #region 交易日期 (代收日期, 異業代收則為行員確認日)
                                string receiveDate = null;
                                if (Fuju.Common.TryConvertTWDate8(dataVals[2], out date))
                                {
                                    receiveDate = Fuju.Common.GetTWDate7(date); //資料庫存的是民國年7碼
                                }
                                else
                                {
                                    receiveDate = dataVals[2].Trim();
                                }
                                #endregion

                                #region 交易時間
                                string receiveTime = dataVals[3].Trim();
                                #endregion

                                #region 交易金額
                                decimal receiveAmount = 0;
                                decimal.TryParse(dataVals[4], out receiveAmount);
                                #endregion

                                #region [MDY:20160607] 紀錄更正記號
                                string hCode = dataVals[5].Trim();
                                #endregion

                                #region 交易來源 (代收管道)
                                string receiveWay = dataVals[6].Trim();
                                #endregion

                                #region 作帳日
                                string accountDate = null;
                                if (Fuju.Common.TryConvertTWDate8(dataVals[7], out date))
                                {
                                    accountDate = Fuju.Common.GetTWDate7(date); //資料庫存的是民國年7碼
                                }
                                else
                                {
                                    accountDate = dataVals[7].Trim();
                                }
                                #endregion

                                #region 繳款行
                                string receiveBank = "005" + dataVals[8].Trim();   //EAI只給分行3碼，所以補上 005
                                #endregion

                                if (receiveWay == ChannelHelper.PO)
                                {
                                    //郵局的 PTXDAY (行員確認日) 才是實際入帳日，所以 accountDate 與 receiveDate 互換 ??
                                    string tmp = accountDate;
                                    accountDate = receiveDate;
                                    receiveDate = tmp;
                                }

                                #region [MDY:20160607] 更正資料改由銷帳時處理
                                //#region 沖帳處理
                                ////1. 如果遇到沖帳資料，則由該資料往前找到相同的資料，取消該資料的處理
                                ////2. 將取消處理的資料與該沖帳資料記錄到 Log
                                //bool rollbackFlag = (dataVals[5].Trim() == HCODE);	//沖帳記號
                                //if (rollbackFlag)
                                //{
                                //    _Log.AppendFormat("發現沖帳資料 {0} : {1} ", dataVals[0], rec.OuterXml).AppendLine();
                                //    for (int idx = list.Count - 1; idx >= 0; idx--)
                                //    {
                                //        CancelDebtsEntity one = list[idx];
                                //        if (one.ReceiveType == appno && one.FileName == dataVals[1] && one.AccountDate == accountDate
                                //            && one.ReceiveWay == dataVals[6] && one.ReceiveBank == dataVals[8]
                                //            && one.ReceiveDate == receiveDate && one.ReceiveAmount == receiveAmount)
                                //        {
                                //            list.RemoveAt(idx);
                                //            _Log.AppendFormat("找到被沖資料 {0} : {1} ", one.FileName, one.Remark).AppendLine();
                                //            break;
                                //        }
                                //    }
                                //    continue;
                                //}
                                //#endregion
                                #endregion

                                CancelDebtsEntity data = new CancelDebtsEntity();

                                #region 繳款資料
                                data.ReceiveType = appno;
                                data.CancelNo = appno + customNo;
                                data.ReceiveDate = receiveDate;
                                data.ReceiveTime = receiveTime;
                                data.AccountDate = accountDate;
                                data.ReceiveBank = receiveBank;
                                data.ReceiveWay = receiveWay;
                                data.ReceiveAmount = receiveAmount;
                                #endregion

                                #region 資料來源
                                data.FileName = String.Format("{0}_{1}_{2}", "D00I70", appno, txday);
                                data.SourceData = rec.OuterXml;
                                data.SourceSeq = txSeq;
                                #endregion

                                data.PayDueDate = String.Empty;
                                data.Reserve1 = String.Empty;

                                #region [MDY:20160607] 紀錄更正記號
                                data.Reserve2 = hCode;
                                #endregion

                                data.Remark = String.Empty;

                                data.ModifyDate = DateTime.Now;
                                data.RollbackDate = null;
                                data.CancelDate = null;
                                data.Status = CancelDebtsStatusCodeTexts.IS_WAITING_CODE;  //待處理

                                list.Add(data);
                            }
                        }
                        else
                        {
                            _Log.AppendLine("SendD00I70 未回傳任何 D00I70Rec 資料");
                        }
                        #endregion
                    }
                    else
                    {
                        errmsg = String.Format("Call SendD00I70 失敗, StatusCode:{0} statusDesc:{1}", statusCode, statusDesc);
                        _Log.AppendLine(errmsg);
                    }
                    datas = list == null ? null : list.ToArray();

                    #region [MDY:20160308] EAI 日誌處理
                    eaiLog.RsStatusCode = statusCode;
                    eaiLog.RsStatusDesc = statusDesc;
                    eaiLog.SendResult = errmsg;
                    #endregion
                }
                #endregion
            }
            catch (Exception exp)
            {
                errmsg = String.Format("Call SendD00I70 發生例外, {0}", exp.Message);
                _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] Get Exception : {1}", DateTime.Now, exp.Message).AppendLine();

                #region [MDY:20160308] EAI 日誌處理
                eaiLog.SendResult = errmsg;
                #endregion
            }
            finally
            {
                #region [MDY:20160308] EAI 日誌處理
                {
                    string errmsg2 = this.WriteEAILog(eaiLog);
                    if (!String.IsNullOrEmpty(errmsg2))
                    {
                        _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg2).AppendLine();
                    }
                }
                #endregion
            }

            _ErrMsg = errmsg;
            return String.IsNullOrEmpty(_ErrMsg);
        }

        /// <summary>
        /// 發送 EAI 的 D00I70 電文
        /// </summary>
        /// <param name="v_id"></param>
        /// <param name="txDay"></param>
        /// <param name="sTxSeq"></param>
        /// <param name="eTxSeq"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool SendD00I70(string v_id, string txDay, int sTxSeq, int eTxSeq, out CancelDebtsEntity[] datas)
        {
            string stxseq = sTxSeq < 1 || sTxSeq > 999999 ? "0" : sTxSeq.ToString("0");
            string etxseq = eTxSeq < 1 || eTxSeq > 999999 ? "999999" : eTxSeq.ToString("0");
            return SendD00I70("1", v_id, txDay, stxseq, etxseq, out datas);
        }
        #endregion

        #region D00I71
        private string GetD00I71RqMsg(string appno, out EAILogEntity eaiLog)
        {
            DateTime now = DateTime.Now;
            string strEAIHeader = this.GetEAIHeader(now);

            #region [MDY:20161124] 改用新電文格式 (多 SUBAPPNO)
            string msgId = "D0071";
            string rqUID = Guid.NewGuid().ToString(); //"52304abb-7137-4cf5-a04c-45f9db6db71a";
            string allRecCtrlIn = "Y";  //"N"
            string subAppNo = "0";      //次應用代碼，學雜費不使用 7300 ~ 7599 的商家代號，所以固定填 0

            StringBuilder xml = new StringBuilder();
            xml
                .Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>").AppendLine()
                .Append("<LandBankML xmlns=\"urn:schema-bluestar-com:multichannel\">").AppendLine()
                .Append(strEAIHeader)
                .Append("	<CommMsg>").AppendLine()
                .AppendFormat("		<SPName>{0}</SPName>", _EAISPName).AppendLine()
                .AppendFormat("		<MsgId>{0}</MsgId>", msgId).AppendLine()
                .AppendFormat("		<RqUID>{0}</RqUID>", rqUID).AppendLine()
                .AppendFormat("		<AllRecCtrlIn>{0}</AllRecCtrlIn>", allRecCtrlIn).AppendLine()
                .AppendFormat("		<APPNO>{0}</APPNO>", appno).AppendLine()
                .AppendFormat("		<SUBAPPNO>{0}</SUBAPPNO>", subAppNo).AppendLine()
                .Append("	</CommMsg>").AppendLine()
                .Append("</LandBankML>").AppendLine();
            #endregion

            #region [MDY:20160308] EAI 日誌處理
            eaiLog = new EAILogEntity(EAILogEntity.KIND_D0071, now);
            eaiLog.MsgId = msgId;
            eaiLog.RqUID = rqUID;
            eaiLog.RqXml = xml.ToString();
            #endregion

            return eaiLog.RqXml;
        }

        /// <summary>
        /// 發送 EAI 的 D00I71 電文
        /// </summary>
        /// <param name="appno">要查詢的商店代號</param>
        /// <param name="datas">傳回 TXREC 節點下的資料 (Key=節點 Name, Value=節點 InnerText)</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool SendD00I71(string appno, out KeyValueList<string> datas)
        {
            datas = null;
            _ErrMsg = String.Empty;
            _Log = new StringBuilder();

            if (!this.IsEAIArgsReady())
            {
                _ErrMsg = "此物件的 EAI 參數未準備好";
                return false;
            }
            if (!this.IsEAIDailyKeyReady())
            {
                if (!this.ChangeDailyKey())
                {
                    _ErrMsg = "自動換 Key 失敗，" + _ErrMsg;
                    return false;
                }
            }

            string errmsg = null;

            #region [MDY:20161124] 檢查是否需輸入次應用代碼，預設學雜費不使用需要次應用代碼的商家代號
            if (DataFormat.CheckHasSubAppNo(appno))
            {
                _ErrMsg = String.Format("appno ({0}) 需要次應用代碼，學雜費不支援", appno);
                return false;
            }
            #endregion

            #region [MDY:20160308] EAI 日誌處理
            EAILogEntity eaiLog = null;
            #endregion

            try
            {
                #region 發送 D00I71
                string rsXml = null;
                {
                    string rqXml = this.GetD00I71RqMsg(appno, out eaiLog);

                    _Log
                        .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] Call SendD00I71(appno:{1}) (Key={2}), "
                            , DateTime.Now, appno, _EAIDailyKey)
                        .AppendLine(" Rq: ")
                        .AppendLine(rqXml);
                    errmsg = this.CallEAI(rqXml, out rsXml);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        _Log
                            .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] CallEAI 成功, ", DateTime.Now)
                            .AppendLine(" Rs: ").AppendLine(rsXml);
                    }
                    else
                    {
                        _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] CallEAI 失敗{1}, ", DateTime.Now, errmsg).AppendLine();
                    }
                }
                #endregion

                #region [MDY:20160308] EAI 日誌處理
                eaiLog.RsXml = rsXml;
                eaiLog.SendResult = errmsg;
                #endregion

                #region 處理回傳資料
                if (String.IsNullOrEmpty(errmsg))
                {
                    rsXml = rsXml.Replace("xmlns=\"urn:schema-bluestar-com:multichannel\"", "");

                    XmlDocument doc = new XmlDocument();
                    XmlNode node = null;
                    doc.LoadXml(rsXml);

                    #region SignonRs/StatusCode
                    string statusCode = null;
                    {
                        string statusCodePath = "/LandBankML/SignonRs/Status/StatusCode";
                        node = doc.SelectSingleNode(statusCodePath);
                        statusCode = node.InnerText.Trim();
                    }
                    #endregion

                    #region SignonRs/StatusDesc
                    string statusDesc = null;
                    {
                        string statusDescPath = "/LandBankML/SignonRs/Status/StatusDesc";
                        node = doc.SelectSingleNode(statusDescPath);
                        statusDesc = node.InnerText.Trim();
                    }
                    #endregion

                    if (statusCode == "0")
                    {
                        #region Status/StatusCode
                        {
                            string statusCodePath = "/LandBankML/CommMsg/Status/StatusCode";
                            node = doc.SelectSingleNode(statusCodePath);
                            statusCode = node.InnerText.Trim();
                        }
                        #endregion

                        #region Status/StatusDesc
                        {
                            string statusDescPath = "/LandBankML/CommMsg/Status/StatusDesc";
                            node = doc.SelectSingleNode(statusDescPath);
                            statusDesc = node.InnerText.Trim();
                        }
                        #endregion

                        if (statusCode == "0")
                        {
                            #region 取得 TXREC 並將此節點下的子節點轉成 KeyValueList
                            XmlNode txrec = doc.SelectSingleNode("/LandBankML/CommMsg/TXREC");
                            if (txrec != null && txrec.HasChildNodes)
                            {
                                XmlNodeList xNodes = txrec.ChildNodes;
                                datas = new KeyValueList<string>(xNodes.Count);
                                foreach (XmlNode xNode in xNodes)
                                {
                                    if (xNode.NodeType == XmlNodeType.Element)
                                    {
                                        datas.Add(xNode.Name, xNode.InnerText.Trim());
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            errmsg = String.Format("Call SendD00I71 失敗, StatusCode:{0} statusDesc:{1}", statusCode, statusDesc);
                            _Log.AppendLine(errmsg);
                        }
                    }
                    else
                    {
                        errmsg = String.Format("Call SendD00I71 失敗, StatusCode:{0} statusDesc:{1}", statusCode, statusDesc);
                        _Log.AppendLine(errmsg);
                    }

                    #region [MDY:20160308] EAI 日誌處理
                    eaiLog.RsStatusCode = statusCode;
                    eaiLog.RsStatusDesc = statusDesc;
                    eaiLog.SendResult = errmsg;
                    #endregion
                }
                #endregion
            }
            catch (Exception exp)
            {
                errmsg = String.Format("Call SendD00I71 發生例外, {0}", exp.Message);
                _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] Get Exception : {1}", DateTime.Now, exp.Message).AppendLine();

                #region [MDY:20160308] EAI 日誌處理
                if (eaiLog != null)
                {
                    eaiLog.SendResult = errmsg;
                }
                #endregion
            }
            finally
            {
                #region [MDY:20160308] EAI 日誌處理
                if (eaiLog != null)
                {
                    string errmsg2 = this.WriteEAILog(eaiLog);
                    if (!String.IsNullOrEmpty(errmsg2))
                    {
                        _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg2).AppendLine();
                    }
                }
                #endregion
            }

            _ErrMsg = errmsg;
            return String.IsNullOrEmpty(_ErrMsg);
        }
        #endregion

        #region D38
        /// <summary>
        /// 取得 D38 發送電文
        /// </summary>
        /// <param name="upd">異動記號</param>
        /// <param name="schno">學校代號</param>
        /// <param name="actno">學校存款帳號</param>
        /// <param name="payno">存繳代號</param>
        /// <param name="stuno">學號</param>
        /// <param name="name">姓名</param>
        /// <param name="vidno">學生虛擬帳號</param>
        /// <param name="dueamt">應繳學費</param>
        /// <param name="clstype">銷帳方式</param>
        /// <param name="mtype">新舊媒體記號</param>
        /// <returns></returns>
        private string GetD38RqMsg(string upd, string schno, string actno, string payno, string stuno, string name, string vidno, decimal dueamt
            , string clstype, string mtype, out EAILogEntity eaiLog)
        {
            DateTime now = DateTime.Now;
            string strEAIHeader = this.GetEAIHeader(now);

            string msgId = "D3800_1";
            string rqUID = Guid.NewGuid().ToString(); //"52304abb-7137-4cf5-a04c-45f9db6db71a";
            string allRecCtrlIn = "Y";  //"N"
            int duecnt = 1;

            StringBuilder xml = new StringBuilder();
            xml
                .Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>").AppendLine()
                .Append("<LandBankML xmlns=\"urn:schema-bluestar-com:multichannel\">").AppendLine()
                .Append(strEAIHeader)
                .Append("	<CommMsg>").AppendLine()
                .AppendFormat("		<SPName>{0}</SPName>", _EAISPName).AppendLine()
                .AppendFormat("		<MsgId>{0}</MsgId>", msgId).AppendLine()
                .AppendFormat("		<RqUID>{0}</RqUID>", rqUID).AppendLine()
                .AppendFormat("		<AllRecCtrlIn>{0}</AllRecCtrlIn>", allRecCtrlIn).AppendLine()
                .AppendFormat("		<UPD>{0}</UPD>", upd).AppendLine()
                .AppendFormat("		<SCHNO>{0}</SCHNO>", schno == null ? String.Empty : schno.Trim()).AppendLine()
                .AppendFormat("		<STUNO>{0}</STUNO>", stuno == null ? String.Empty : stuno.Trim()).AppendLine()
                .AppendFormat("		<PAYNO>{0}</PAYNO>", payno == null ? String.Empty : payno.Trim()).AppendLine()
                .AppendFormat("		<ACTNO>{0}</ACTNO>", actno == null ? String.Empty : actno.Trim()).AppendLine()
                .AppendFormat("		<VIDNO>{0}</VIDNO>", vidno == null ? String.Empty : vidno.Trim()).AppendLine()
                .AppendFormat("		<NAME>{0}</NAME>", name == null ? String.Empty : name.Trim()).AppendLine()
                .AppendFormat("		<CLSTYPE>{0}</CLSTYPE>", clstype).AppendLine()
                .AppendFormat("		<DUECNT>{0}</DUECNT>", duecnt).AppendLine()
                .AppendFormat("		<DUEAMT_R1>{0:0}</DUEAMT_R1>", dueamt).AppendLine()
                .AppendFormat("		<DUEAMT_R2>{0}</DUEAMT_R2>", String.Empty).AppendLine()
                .AppendFormat("		<DUEAMT_R3>{0}</DUEAMT_R3>", String.Empty).AppendLine()
                .AppendFormat("		<DUEAMT_R4>{0}</DUEAMT_R4>", String.Empty).AppendLine()
                .AppendFormat("		<DUEAMT_R5>{0}</DUEAMT_R5>", String.Empty).AppendLine()
                .AppendFormat("		<DUEAMT_R6>{0}</DUEAMT_R6>", String.Empty).AppendLine()
                .AppendFormat("		<DUEAMT_R7>{0}</DUEAMT_R7>", String.Empty).AppendLine()
                .AppendFormat("		<DUEAMT_R8>{0}</DUEAMT_R8>", String.Empty).AppendLine()
                .AppendFormat("		<DUEAMT_R9>{0}</DUEAMT_R9>", String.Empty).AppendLine()
                .AppendFormat("		<DUEAMT_R10>{0}</DUEAMT_R10>", String.Empty).AppendLine()
                .AppendFormat("		<FILLER>{0}</FILLER>", String.Empty).AppendLine()
                .AppendFormat("		<MTYPE>{0}</MTYPE>", mtype).AppendLine()
                .Append("	</CommMsg>").AppendLine()
                .Append("</LandBankML>").AppendLine();

            #region [MDY:20160308] EAI 日誌處理
            eaiLog = new EAILogEntity(EAILogEntity.KIND_D3800, now);
            eaiLog.MsgId = msgId;
            eaiLog.RqUID = rqUID;
            eaiLog.RqXml = xml.ToString();
            #endregion

            return eaiLog.RqXml;
        }

        /// <summary>
        /// 發送 EAI 的 D38 電文
        /// </summary>
        /// <param name="upd"></param>
        /// <param name="schno"></param>
        /// <param name="actno"></param>
        /// <param name="payno"></param>
        /// <param name="stuno"></param>
        /// <param name="name"></param>
        /// <param name="vidno"></param>
        /// <param name="dueamt"></param>
        /// <param name="rqXml"></param>
        /// <param name="rsXml"></param>
        /// <param name="replycode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool SendD38(string upd, string schno, string actno, string payno, string stuno, string name, string vidno, decimal dueamt
            , out string rqXml, out string rsXml, out string replycode, out string result)
        {
            rqXml = null;
            rsXml = null;
            replycode = null;
            result = null;
            _ErrMsg = String.Empty;
            _Log = new StringBuilder();

            if (!this.IsEAIArgsReady())
            {
                _ErrMsg = "此物件的 EAI 參數未準備好";
                return false;
            }
            if (!this.IsEAIDailyKeyReady())
            {
                if (!this.ChangeDailyKey())
                {
                    _ErrMsg = "自動換 Key 失敗，" + _ErrMsg;
                    return false;
                }
            }

            string errmsg = null;

            #region [MDY:20160308] EAI 日誌處理
            EAILogEntity eaiLog = null;
            #endregion

            try
            {
                #region 發送 D3800_1
                rsXml = null;
                {
                    string clstype = "1";
                    string mtype = "1";
                    rqXml = this.GetD38RqMsg(upd, schno, actno, payno, stuno, name, vidno, dueamt, clstype, mtype, out eaiLog);

                    _Log
                        .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] Call SendD38() (Key={1}), "
                            , DateTime.Now, _EAIDailyKey)
                        .AppendLine(" Rq: ")
                        .AppendLine(rqXml);
                    errmsg = this.CallEAI(rqXml, out rsXml);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        _Log
                            .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] CallEAI 成功, ", DateTime.Now)
                            .AppendLine(" Rs: ").AppendLine(rsXml);
                    }
                    else
                    {
                        _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] CallEAI 失敗{1}, ", DateTime.Now, errmsg).AppendLine();
                    }
                }
                #endregion

                #region [MDY:20160308] EAI 日誌處理
                eaiLog.RsXml = rsXml;
                eaiLog.SendResult = errmsg;
                #endregion

                #region 處理回傳資料
                if (String.IsNullOrEmpty(errmsg))
                {
                    rsXml = rsXml.Replace("xmlns=\"urn:schema-bluestar-com:multichannel\"", "");

                    XmlDocument doc = new XmlDocument();
                    XmlNode node = null;
                    doc.LoadXml(rsXml);

                    #region SignonRs/StatusCode
                    string statusCode = null;
                    {
                        string statusCodePath = "/LandBankML/SignonRs/Status/StatusCode";
                        node = doc.SelectSingleNode(statusCodePath);
                        statusCode = node.InnerText.Trim();
                    }
                    #endregion

                    #region SignonRs/StatusDesc
                    string statusDesc = null;
                    {
                        string statusDescPath = "/LandBankML/SignonRs/Status/StatusDesc";
                        node = doc.SelectSingleNode(statusDescPath);
                        statusDesc = node.InnerText.Trim();
                    }
                    #endregion

                    if (statusCode == "0")
                    {
                        #region Status/StatusCode
                        {
                            string statusCodePath = "/LandBankML/CommMsg/Status/StatusCode";
                            node = doc.SelectSingleNode(statusCodePath);
                            statusCode = node.InnerText.Trim();
                        }
                        #endregion

                        #region Status/StatusDesc
                        {
                            string statusDescPath = "/LandBankML/CommMsg/Status/StatusDesc";
                            node = doc.SelectSingleNode(statusDescPath);
                            statusDesc = node.InnerText.Trim();
                        }
                        #endregion

                        if (statusCode == "0")
                        {
                            #region 取得 TXREC 並將此節點下的子節點轉成 KeyValueList
                            XmlNode xNode = doc.SelectSingleNode("/LandBankML/CommMsg/TXREC/REPLYCODE");
                            if (xNode != null)
                            {
                                replycode = xNode.InnerText.Trim();
                            }
                            //else
                            //{
                            //    replycode = statusCode;
                            //}
                            #endregion

                            #region [MDY:20160705] 回傳處理結果
                            result = replycode;
                            #endregion
                        }
                        else
                        {
                            replycode = statusCode;
                            errmsg = String.Format("Call SendD38 失敗, StatusCode:{0} statusDesc:{1}", statusCode, statusDesc);
                            _Log.AppendLine(errmsg);
                        }
                    }
                    else
                    {
                        replycode = statusCode;
                        errmsg = String.Format("Call SendD38 失敗, StatusCode:{0} statusDesc:{1}", statusCode, statusDesc);
                        _Log.AppendLine(errmsg);
                    }

                    #region [MDY:20160308] EAI 日誌處理
                    eaiLog.RsStatusCode = statusCode;
                    eaiLog.RsStatusDesc = statusDesc;
                    eaiLog.SendResult = errmsg;
                    #endregion
                }
                else
                {
                    replycode = "ERR";
                }
                #endregion
            }
            catch (Exception exp)
            {
                replycode = "ERR";
                errmsg = String.Format("Call SendD38 發生例外, {0}", exp.Message);
                _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] Get Exception : {1}", DateTime.Now, exp.Message).AppendLine();

                #region [MDY:20160308] EAI 日誌處理
                if (eaiLog != null)
                {
                    eaiLog.SendResult = errmsg;
                }
                #endregion
            }
            finally
            {
                #region [MDY:20160308] EAI 日誌處理
                if (eaiLog != null)
                {
                    string errmsg2 = this.WriteEAILog(eaiLog);
                    if (!String.IsNullOrEmpty(errmsg2))
                    {
                        _Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg2).AppendLine();
                    }
                }
                #endregion

                #region [MDY:20160705] 回傳處理結果
                if (String.IsNullOrEmpty(result))   //還未指定 result 值就用 RsStatusDesc
                {
                    result = eaiLog.RsStatusDesc;
                }
                #endregion
            }

            _ErrMsg = errmsg;
            return String.IsNullOrEmpty(_ErrMsg);
        }
        #endregion
    }
}
