using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    public class CancelHelper
    {
        #region Property
        private string _err_msg = "";
        /// <summary>
        /// 取得錯誤訊息
        /// </summary>
        public string err_msg
        {
            get
            {
                return _err_msg;
            }
            private set
            {
                _err_msg = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        public CancelHelper()
        {

        }

        #region CancelDebts 相關
        /// <summary>
        /// 讀取未處理的cancel_debts，並更新為處理中
        /// </summary>
        /// <param name="tw7Date">指定要處理的資料日期 (民國年 yyymmdd，非ATM、網路銀行取入帳日，ATM、網路銀行取代收日)</param>
        /// <param name="cancel_debtss"></param>
        /// <returns></returns>
        private bool getCancelDebtsByAccountDate(string tw7Date, out CancelDebtsEntity[] cancel_debtss)
        {
            bool rc = false;
            _err_msg = "";
            cancel_debtss = null;
            Int32 count=0;
            //Int32[] failindexs = null;

            EntityFactory factory = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderby = null;
            Result result = null;

            using (factory = new EntityFactory())
            {
                #region 取資料邏輯修改
                //土銀說ATM (03) 與網路銀行 (05) 代收日視為入帳，但仍保留 AccountDate 的值
                #endregion

                #region [Old] 取資料邏輯修改
                //where = new Expression(CancelDebtsEntity.Field.AccountDate, account_date);
                //where.And(CancelDebtsEntity.Field.Status, JobCubeStatusCodeTexts.WAIT);
                #endregion

                #region [New] 取資料邏輯修改
                string[] channelWays = new string[] {ChannelHelper.ATM, ChannelHelper.NB};
                //非ATM、網路銀行取入帳日
                Expression w1 = new Expression(CancelDebtsEntity.Field.AccountDate, tw7Date)
                    .And(CancelDebtsEntity.Field.ReceiveWay, RelationEnum.NotIn, channelWays );
                //ATM、網路銀行取代收日
                Expression w2 = new Expression(CancelDebtsEntity.Field.ReceiveDate, tw7Date)
                    .And(CancelDebtsEntity.Field.ReceiveWay, RelationEnum.In, channelWays );

                #region [MDY:20160607] 改用 CancelDebtsStatusCodeTexts 定義常數
                where = new Expression(CancelDebtsEntity.Field.Status, CancelDebtsStatusCodeTexts.IS_WAITING_CODE)
                    .And(w1.Or(w2));
                #endregion
                #endregion

                orderby = new KeyValueList<OrderByEnum>(1);
                orderby.Add(CancelDebtsEntity.Field.ReceiveType, OrderByEnum.Asc);
                result = factory.Select<CancelDebtsEntity>(where, orderby, 0, 100, out cancel_debtss);  //一次最多處理100筆，以避免所有資料都被這個程序佔住
                if (!result.IsSuccess)
                {
                    _err_msg = string.Format("[getCancelDebtsByAccountDate] 讀取cancel_debts發生錯誤，處理的資料日期={0}，錯誤訊息={1}", tw7Date, result.Message);
                }
                else
                {
                    if (cancel_debtss == null || cancel_debtss.Length <= 0)
                    {
                        //_err_msg = string.Format("[getCancelDebtsByAccountDate] 讀取cancel_debts發生錯誤，入帳日={0}，錯誤訊息={1}", account_date, "查無cancel_debts資料");
                        cancel_debtss = new CancelDebtsEntity[0];
                        rc = true;
                    }
                    else
                    {
                        #region [Old]
                        //DateTime ndt = DateTime.Now;
                        //result = factory.Update((CancelDebtsEntity[])cancel_debtss, true, out count, out failindexs);
                        //if (!result.IsSuccess)
                        //{
                        //    _err_msg = string.Format("[getCancelDebts] 更新cancel_debts狀態發生錯誤，入帳日={0}，錯誤訊息={1}", account_date, result.Message);
                        //}
                        //else
                        //{
                        //    rc = true;
                        //}
                        #endregion

                        DateTime modifyDate = DateTime.Now;
                        //string errmsg2 = null;
                        List<CancelDebtsEntity> datas = new List<CancelDebtsEntity>(cancel_debtss.Length);
                        foreach (CancelDebtsEntity data in cancel_debtss)
                        {
                            #region [MDY:20160607] 改用 CancelDebtsStatusCodeTexts 定義常數
                            Expression where2 = new Expression(CancelDebtsEntity.Field.SNo, data.SNo)
                                .And(CancelDebtsEntity.Field.Status, CancelDebtsStatusCodeTexts.IS_WAITING_CODE);  //確保沒被其他程序搶走

                            #region [Old]
                            //KeyValue[] fieldValues = new KeyValue[] {
                            //    new KeyValue(CancelDebtsEntity.Field.Status, JobCubeStatusCodeTexts.PROCESS),
                            //    new KeyValue(CancelDebtsEntity.Field.ModifyDate, modifyDate)
                            //};
                            #endregion

                            //不要更新 ModifyDate ，不然會分不出來資料是什麽時候匯入的
                            KeyValue[] fieldValues = new KeyValue[] {
                                new KeyValue(CancelDebtsEntity.Field.Status, CancelDebtsStatusCodeTexts.IS_PROCESSING_CODE)
                            };
                            result = factory.UpdateFields<CancelDebtsEntity>(fieldValues, where2, out count);
                            if (result.IsSuccess)
                            {
                                if (count > 0)
                                {
                                    data.Status = CancelDebtsStatusCodeTexts.IS_PROCESSING_CODE;
                                    datas.Add(data);
                                }

                                #region [Old]
                                //else
                                //{
                                //    errmsg2 = "找不到該資料";
                                //}
                                #endregion
                            }

                            #region [Old]
                            //else
                            //{
                            //    errmsg2 = result.Message;
                            //}
                            #endregion
                            #endregion
                        }
                        if (datas.Count == 0)
                        {
                            _err_msg = string.Format("[getCancelDebtsByAccountDate] 更新cancel_debts狀態發生錯誤，處理的資料日期={0}，錯誤訊息=已找到{1}筆資料但無任何一筆資料的狀態被更新成功", tw7Date, cancel_debtss.Length);
                        }
                        else
                        {
                            cancel_debtss = datas.ToArray();
                            rc = true;
                        }
                    }
                }
            }
            return rc;
        }

        #region [MDY:20160607] 沒用到先 Mark
        //public bool updateCancelDebtsStatus(CancelDebtsEntity[] cancel_debtss)
        //{
        //    bool rc = false;
        //    string log = "";
        //    StringBuilder logs = new StringBuilder();

        //    int[] fail_index_array=null;
        //    int count=0;
        //    EntityFactory factory = new EntityFactory();
        //    Result result = factory.Update(cancel_debtss, false, out count, out fail_index_array);
        //    if (fail_index_array != null && fail_index_array.Length > 0)
        //    {
        //        string key = string.Format("");
        //        log = string.Format("");
        //        logs.AppendLine(log);
        //    }
        //    rc = true;
        //    return rc;
        //}

        //public bool insertCancelDetbs(CancelDebtsEntity cancel_debts)
        //{
        //    bool rc = false;

        //    return rc;
        //}

        //public bool getCancelDebtsByReceiveTypeResver1(string receive_type,string account_date7,out CancelDebtsEntity[] cancel_debtss,out string msg)
        //{
        //    bool rc = false;
        //    cancel_debtss = null;
        //    msg = "";

        //    using (EntityFactory factory = new EntityFactory())
        //    {
        //        Expression where = new Expression(CancelDebtsEntity.Field.ReceiveType, receive_type);
        //        where.And(CancelDebtsEntity.Field.AccountDate, account_date7);
        //        where.And(CancelDebtsEntity.Field.Reserve1, "");

        //        KeyValueList<OrderByEnum> orderby = new KeyValueList<OrderByEnum>(1);
        //        orderby.Add(CancelDebtsEntity.Field.CancelNo, OrderByEnum.Asc);

        //        Result result = factory.SelectAll<CancelDebtsEntity>(where, orderby, out cancel_debtss);
        //        if (result.IsSuccess)
        //        {
        //            if (cancel_debtss != null && cancel_debtss.Length > 0)
        //            {
        //                rc = true;
        //            }
        //            else
        //            {

        //            }
        //        }
        //        else
        //        {

        //        }
        //    }
        //    return rc;
        //}
        #endregion

        /// <summary>
        /// 使用交易批次更新 CancelDebtsEntity 的 Reserve1 (被匯出成銷帳資料的日期) 欄位值為指定的日期 (錯誤訊息由 err_msg 屬性取得)
        /// </summary>
        /// <param name="date">指定的Resver1日期</param>
        /// <param name="datas">要更新的 CancelDebtsEntity 陣列</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool BatchUpdateCancelDebtsResver1(DateTime date, CancelDebtsEntity[] datas)
        {
            _err_msg = String.Empty;
            if (datas != null && datas.Length > 0)
            {
                int count = 0;
                KeyValue[] fieldValues = new KeyValue[] { new KeyValue(CancelDebtsEntity.Field.Reserve1, Common.GetDate8(date)) };
                Result result = null;
                using (EntityFactory factory = new EntityFactory(true))
                {
                    foreach (CancelDebtsEntity data in datas)
                    {
                        Expression where = new Expression(CancelDebtsEntity.Field.SNo, data.SNo);
                        result = factory.UpdateFields<CancelDebtsEntity>(fieldValues, where, out count);
                        if (!result.IsSuccess)
                        {
                            break;
                        }
                    }
                    if (result.IsSuccess)
                    {
                        factory.Commit();
                    }
                    else
                    {
                        factory.Rollback();
                    }
                }
                _err_msg = result.Message;
                return result.IsSuccess;
            }
            return false;
        }


        /// <summary>
        /// 取產生學校銷帳檔用的 CancelDebts 資料，以交易序號遞增排序 (此方法的錯誤訊息由 msg 傳回，不會更新 err_msg 屬性)
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="date7">匯出資料日期，格式為民國年7碼 (ReceiveDate 交易日)</param>
        /// <param name="datas"></param>
        /// <param name="msg">傳回錯誤訊息或空字串</param>
        /// <returns></returns>
        public bool GetCancelDebtsForGenCancelData(string receiveType, string date7, out CancelDebtsEntity[] datas, out string msg)
        {
            bool rc = false;
            datas = null;
            msg = "";

            using (EntityFactory factory = new EntityFactory())
            {
                Expression where = new Expression(CancelDebtsEntity.Field.ReceiveType, receiveType)
                    .And(CancelDebtsEntity.Field.ReceiveDate, date7);

                KeyValueList<OrderByEnum> orderby = new KeyValueList<OrderByEnum>(1);
                orderby.Add(CancelDebtsEntity.Field.SourceSeq, OrderByEnum.Asc);

                Result result = factory.SelectAll<CancelDebtsEntity>(where, orderby, out datas);
                if (result.IsSuccess)
                {
                    if (datas != null && datas.Length > 0)
                    {
                        rc = true;
                    }
                    else
                    {
                        msg = "查無資料";
                    }
                }
                else
                {
                    msg = result.Message;
                }
            }
            return rc;
        }


        /// <summary>
        /// 取得指定商家代號與交易日的 CancelDebts 資料 (產生學校銷帳檔用，以 SourceSeq 遞增排序，注意：此方法不會更新 err_msg 屬性)
        /// </summary>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="receiveDate">指定交易日 (民國年7碼))</param>
        /// <param name="datas">傳回 CancelDebts 資料</param>
        /// <returns>傳回錯誤訊息</returns>
        public string GetCancelDebtsForGenCancelData(string receiveType, string receiveDate, out CancelDebtsEntity[] datas)
        {
            datas = null;
            string errmsg = null;
            using (EntityFactory factory = new EntityFactory())
            {
                Expression where = new Expression(CancelDebtsEntity.Field.ReceiveType, receiveType)
                    .And(CancelDebtsEntity.Field.ReceiveDate, receiveDate);
                KeyValueList<OrderByEnum> orderby = new KeyValueList<OrderByEnum>(1);
                orderby.Add(CancelDebtsEntity.Field.SourceSeq, OrderByEnum.Asc);

                Result result = factory.SelectAll<CancelDebtsEntity>(where, orderby, out datas);
                if (!result.IsSuccess)
                {
                    errmsg = result.Message;
                }
            }
            return errmsg;
        }


        #region [MDY:20170606] 檢查指定的銷帳(入帳)資料是否存在，不存在則新增該資料 (M201705_02)
        /// <summary>
        /// 檢查指定的銷帳(入帳)資料是否存在，不存在則新增該資料 (注意：此方法不會更新 err_msg 屬性)
        /// </summary>
        /// <param name="factory">指定資料存取物件</param>
        /// <param name="data">指定要新增的銷帳(入帳)資料</param>
        /// <returns>傳回處理結果，如果資料已存在則傳回 new Result(false, ErrorCode.D_DATA_EXISTS)</returns>
        public Result CheckThenInsertCancelDebts(EntityFactory factory, CancelDebtsEntity data)
        {
            Result result = new Result(true);
            int count = 0;

            #region 檢查資料來源，避免重複新增
            if (result.IsSuccess)
            {
                Expression where = new Expression(CancelDebtsEntity.Field.FileName, data.FileName)
                    .And(CancelDebtsEntity.Field.SourceSeq, data.SourceSeq);
                result = factory.SelectCount<CancelDebtsEntity>(where, out count);
                if (result.IsSuccess)
                {
                    if (count > 0)
                    {
                        string errmsg = String.Format("資料已存在 (FileName={0}; SourceSeq={1})，忽略不處理", data.FileName, data.SourceSeq);
                        result = new Result(false, errmsg, ErrorCode.D_DATA_EXISTS, null);
                    }
                }
                else
                {
                    string errmsg = String.Format("查詢資料 (FileName={0}; SourceSeq={1}) 是否重複失敗，{2}", data.FileName, data.SourceSeq, result.Message);
                    result = new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region 檢查是否資料已存在 (可能是 D338 或分行上傳 D00I70 新增的)
            if (result.IsSuccess)
            {
                Expression where = new Expression(CancelDebtsEntity.Field.ReceiveType, data.ReceiveType)
                    .And(CancelDebtsEntity.Field.CancelNo, data.CancelNo)
                    .And(CancelDebtsEntity.Field.ReceiveAmount, data.ReceiveAmount)
                    .And(CancelDebtsEntity.Field.ReceiveDate, data.ReceiveDate)
                    .And(CancelDebtsEntity.Field.ReceiveTime, data.ReceiveTime)
                    .And(CancelDebtsEntity.Field.ReceiveWay, data.ReceiveWay)
                    .And(CancelDebtsEntity.Field.AccountDate, data.AccountDate)
                    .And(CancelDebtsEntity.Field.ReceiveBank, data.ReceiveBank);
                result = factory.SelectCount<CancelDebtsEntity>(where, out count);
                if (result.IsSuccess)
                {
                    if (count > 0)
                    {
                        string errmsg = String.Format("資料已存在 (ReceiveType={0}; CancelNo={1}; ReceiveAmount={2}; ReceiveDate={3}; ReceiveTime={4}; ReceiveWay={5}; AccountDate={6}; ReceiveBank={7})，忽略不處理"
                            , data.ReceiveType, data.CancelNo, data.ReceiveAmount, data.ReceiveDate, data.ReceiveTime, data.ReceiveWay, data.AccountDate, data.ReceiveBank);
                        result = new Result(false, errmsg, ErrorCode.D_DATA_EXISTS, null);
                    }
                }
                else
                {
                    string errmsg = String.Format("查詢資料 (ReceiveType={0}; CancelNo={1}; ReceiveAmount={2}; ReceiveDate={3}; ReceiveTime={4}; ReceiveWay={5}; AccountDate={6}; ReceiveBank={7}) 是否重複失敗，{8}"
                        , data.ReceiveType, data.CancelNo, data.ReceiveAmount, data.ReceiveDate, data.ReceiveTime, data.ReceiveWay, data.AccountDate, data.ReceiveBank, result.Message);
                    result = new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region 新增資料
            if (result.IsSuccess)
            {
                result = factory.Insert(data, out count);
                if (!result.IsSuccess)
                {
                    string errmsg = String.Format("新增資料 (ReceiveType={0}; CancelNo={1}; ReceiveAmount={2}; ReceiveDate={3}; ReceiveTime={4}; ReceiveWay={5}; AccountDate={6}; ReceiveBank={7}; SourceSeq={8}) 失敗，{9}"
                        , data.ReceiveType, data.CancelNo, data.ReceiveAmount, data.ReceiveDate, data.ReceiveTime, data.ReceiveWay, data.AccountDate, data.ReceiveBank, data.SourceSeq, result.Message);
                    result = new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            return result;
        }
        #endregion

        #region [MDY:20170828] 產生學校銷帳檔 (M201705_02)
        /// <summary>
        /// 產生學校銷帳檔相關檔案
        /// </summary>
        /// <param name="school">商家代號</param>
        /// <param name="qReceiveDate">匯出資料的代收日期</param>
        /// <param name="workPath">產檔工作路徑</param>
        /// <param name="srcPath">原始資料檔路徑</param>
        /// <param name="dataCount">傳回資料筆數</param>
        /// <param name="keyFileFullName">傳回產生的 key 檔完整路徑名稱</param>
        /// <param name="txtFileFullName">傳回產生的發送資料檔完整路徑名稱</param>
        /// <param name="configFileContent">傳回指示檔內容</param>
        /// <param name="warning">傳回非錯誤的警告訊息</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        public string GenSchoolCancelData(SchoolRTypeEntity school, DateTime qReceiveDate, string workPath, string srcPath
            , out int dataCount, out string keyFileFullName, out string txtFileFullName, out string configFileContent, out string warning)
        {
            string errmsg = null;
            dataCount = 0;
            keyFileFullName = null;
            txtFileFullName = null;
            configFileContent = null;
            warning = null;
            StringBuilder content = new StringBuilder();

            DateTime now = DateTime.Now;

            #region 取得資料並組資料檔內容
            CancelDebtsEntity[] datas = null;
            try
            {
                errmsg = this.GetCancelDebtsForGenCancelData(school.ReceiveType, Common.GetTWDate7(qReceiveDate), out datas);
                if (String.IsNullOrEmpty(errmsg))
                {
                    if (datas != null && datas.Length > 0)
                    {
                        dataCount = datas.Length;

                        #region 資料檔內容格式
                        //位置   欄位名稱   型別(長度)  備註
                        //01~06：交易序號   9(06)
                        //07~10：商家代號   9(04)
                        //11~20：用戶號碼   9(10)       商家代號+用戶號碼=虛擬帳號(繳款帳號)
                        //21~28：交易日期   9(08)       yyyymmdd民國年，右靠左補0
                        //29~34：交易時間   (06)
                        //35~47：交易金額   9(11)v99    含小數後2位
                        //48~48：更正記號   9(01)       0:正常交易 1:更正
                        //49~50：交易來源   (02)
                        //51~58：作帳日期   (08)        yyyymmdd民國年，右靠左補0
                        //59~61：繳款行     9(03)
                        //62~80：FILLER     X(19)
                        #endregion

                        #region 組資料檔內容
                        #region FILLER     X(19)
                        string filler = "".PadRight(19, ' ');
                        #endregion

                        foreach (CancelDebtsEntity data in datas)
                        {
                            #region 交易序號   9(06)
                            string seq = (data.SourceSeq == null ? "" : data.SourceSeq.Value.ToString("000000"));
                            #endregion

                            #region 商家代號   9(04) + 用戶號碼   9(10) = 虛擬帳號
                            //MEMO：土銀有預留16碼的虛擬帳號機制，只是沒商家代號使用，所以不一定只有14碼，這可能會有問題
                            string cancelNo = data.CancelNo.Trim().PadRight(14, ' ');
                            #endregion

                            #region 交易日期   9(08)       yyyymmdd民國年，右靠左補0
                            string receiveDate = data.ReceiveDate.PadLeft(8, '0');
                            #endregion

                            #region 交易時間   9(06)
                            string receiveTime = data.ReceiveTime.PadLeft(6, ' ');
                            #endregion

                            #region 交易金額   9(11)v99    含小數後2位
                            string receiveAmount = String.Format("{0:00000000000}00", data.ReceiveAmount);
                            #endregion

                            #region 更正記號   9(01)       0:正常交易 1:更正
                            string flag = data.Reserve2.Trim().PadRight(1, ' ');
                            #endregion

                            #region 交易來源   (02)
                            string receiveWay = data.ReceiveWay.Trim().PadRight(2, ' ');
                            #endregion

                            #region 作帳日期   (08)        yyyymmdd民國年，右靠左補0
                            string accountDate = data.AccountDate.PadLeft(8, '0');
                            #endregion

                            #region 繳款行     9(03)
                            string bankId = data.ReceiveBank.Substring(3, 3);
                            #endregion

                            content.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", seq, cancelNo, receiveDate, receiveTime, receiveAmount, flag, receiveWay, accountDate, bankId, filler).AppendLine();
                        }
                        #endregion
                    }
                    else
                    {
                        content.AppendLine(""); //無資料組空檔
                    }
                }
                else
                {
                    errmsg = String.Format("讀取 {0} 銷帳資料失敗，錯誤訊息：{1}", school.ReceiveType, errmsg);
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Format("產生 {0} 學校銷帳資料檔內容發生例外，錯誤訊息：{1}", school.ReceiveType, ex.Message);
            }
            #endregion

            #region 寫檔 (每一個商家代號一個加密資料檔，對應一個 key 檔)
            if (String.IsNullOrEmpty(errmsg))
            {
                string actionName = null;
                try
                {
                    string filePrefix = school.ReceiveType + "_";   //本地端檔名以商家代號做字首，方便找檔案

                    #region 產生原始資料檔
                    actionName = "產生原始資料檔";
                    string srcFileName = String.Format("{0}{1}{2:HHmmss}.src", filePrefix, Common.GetTWDate7(now), now);
                    string srcFileFullName = Path.Combine(workPath, srcFileName);
                    File.WriteAllText(srcFileFullName, content.ToString(), Encoding.Default);
                    #endregion

                    #region 產生 key 檔
                    actionName = "產生 key 檔";
                    string keyFileName = srcFileName.Replace(".src", ".key");
                    keyFileFullName = Path.Combine(workPath, keyFileName);
                    //key值：receive_type + ccMMddHHmmss。共16碼
                    string key = String.Format("{0}{1}{2:HHmmss}", school.ReceiveType, Common.GetTWDate6(now), now);
                    File.WriteAllText(keyFileFullName, key, Encoding.Default);
                    #endregion

                    #region 產生發送資料檔 (加密原始資料檔成為發送資料檔)
                    actionName = "產生發送資料檔";
                    string txtFileName = srcFileName.Replace(".src", ".txt");
                    txtFileFullName = Path.Combine(workPath, txtFileName);

                    #region 加密原始資料檔成為發送資料檔
                    {
                        string errmsg2 = null;
                        Encryption encrypt = new Encryption();
                        if (!encrypt.DESEncryptFile(srcFileFullName, txtFileFullName, key, out errmsg2))
                        {
                            errmsg = String.Format("加密 {0} 原始資料檔成 {1} 發送資料檔失敗，{2}", srcFileName, txtFileName, errmsg2);
                        }
                    }
                    #endregion
                    #endregion

                    #region 產生指示檔內容
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        actionName = "產生指示檔內容";
                        if (String.IsNullOrWhiteSpace(school.FtpLocation) || String.IsNullOrWhiteSpace(school.FtpAccount))
                        {
                            errmsg = "缺少 FTP 伺服器或帳號的設定";
                        }
                        else
                        {
                            #region [MDY:20211001] M202110_01 支援 FTP/FTPS/SFTP (2021擴充案先做)
                            #region [MDY:20210401] 原碼修正
                            #region [MDY:20220530] Checkmarx 調整
                            string ftpKind = String.IsNullOrEmpty(school.FtpKind) ? "FTP" : school.FtpKind.Trim();
                            string ftpHost = school.FtpLocation.Trim();
                            string ftpPort = school.FtpPort == null ? String.Empty : school.FtpPort.Trim();
                            string ftpUid = school.FtpAccount.Trim();
                            string ftpPXX = school.FtpPXX == null ? String.Empty : school.FtpPXX;
                            string remotePath = "/";
                            StringBuilder cmd = new StringBuilder();
                            string remoteTxtFile = txtFileName.Replace(filePrefix, ""); //遠端檔名移除商家代號字首
                            string remoteKeyFile = keyFileName.Replace(filePrefix, ""); //遠端檔名移除商家代號字首
                            cmd.AppendFormat("protocol={0} host={1} port={2} uid={3} pwd={4} remote_path={5} remote_file={6} local_file={7}", ftpKind, ftpHost, ftpPort, ftpUid, ftpPXX, remotePath, remoteTxtFile, txtFileName).AppendLine();
                            cmd.AppendFormat("protocol={0} host={1} port={2} uid={3} pwd={4} remote_path={5} remote_file={6} local_file={7}", ftpKind, ftpHost, ftpPort, ftpUid, ftpPXX, remotePath, remoteKeyFile, keyFileName).AppendLine();
                            configFileContent = cmd.ToString();
                            #endregion
                            #endregion
                            #endregion
                        }
                    }
                    #endregion

                    #region 移動原始資料檔到備份資料夾
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        string bakFileFullName = null;
                        try
                        {
                            bakFileFullName = Path.Combine(srcPath, srcFileName);
                            File.Move(srcFileFullName, bakFileFullName);
                        }
                        catch (Exception ex)
                        {
                            //失敗不要傳會錯誤，避免後面程式不處理產生的檔案
                            warning += String.Format("移動原始資料檔 ({0}) 到備份資料夾 ({1}) 失敗，{2}。", srcFileFullName, bakFileFullName, ex.Message);
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    errmsg = String.Format("產生 {0} 學校銷帳檔相關檔案發生例外 ({1})，錯誤訊息：{2}", school.ReceiveType, actionName, ex.Message);
                }
            }
            #endregion

            #region 更新 CancelDebts 的　Resver1
            if (String.IsNullOrEmpty(errmsg) && datas != null && datas.Length > 0)
            {
                //失敗不要傳會錯誤，避免後面程式不處理產生的檔案
                bool isUpdateOK = this.BatchUpdateCancelDebtsResver1(now, datas);
                if (!isUpdateOK)
                {
                    warning += String.Format("更新匯出資料的匯出日期失敗，{0}。", this.err_msg);
                }
            }
            #endregion

            return errmsg;
        }
        #endregion
        #endregion

        #region StudentReceive 相關
        /// <summary>
        /// 取得學生繳費單資料
        /// </summary>
        /// <param name="receive_type">商家代號碼</param>
        /// <param name="cancel_no">虛擬帳號</param>
        /// <param name="student_receives">學生繳費單資料</param>
        /// <returns>-1:失敗 0:查無資料 1以上:查到幾筆</returns>
        public int getStudentReceive(string receive_type,string cancel_no,out StudentReceiveEntity[] student_receives)
        {
            _err_msg = "";
            int rc = -1;
            student_receives = null;
            string key = string.Format("receive_type={0},cancel_no={1}", receive_type, cancel_no);

            EntityFactory factory = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderby = null;

            try
            {
                factory = new EntityFactory();
                where = new Expression(StudentReceiveEntity.Field.ReceiveType, receive_type);
                where.And(StudentReceiveEntity.Field.CancelNo, cancel_no);
                Result result = factory.SelectAll<StudentReceiveEntity>(where, orderby,out student_receives);
                if(!result.IsSuccess )
                {
                    _err_msg = string.Format("[getStudentReceive] 讀取學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, result.Message);
                }
                else
                {
                    if(student_receives==null || student_receives.Length==0)
                    {
                        rc = 0;
                        _err_msg = string.Format("[getStudentReceive] 讀取學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, "查無學生繳費單資料");
                    }
                    else
                    {
                        rc = student_receives.Length;
                    }
                }
            }
            catch(Exception ex)
            {
                _err_msg = string.Format("[getStudentReceive] 讀取學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, ex.Message);
            }
            return rc;
        }

        /// <summary>
        /// 更新指定資料的代收日、代收時間、代收管道 (預銷即已繳待銷)
        /// </summary>
        /// <param name="receive_type"></param>
        /// <param name="year_id"></param>
        /// <param name="term_id"></param>
        /// <param name="dep_id"></param>
        /// <param name="receive_id"></param>
        /// <param name="stu_id"></param>
        /// <param name="old_Seq"></param>
        /// <param name="cancel_no"></param>
        /// <param name="receive_way"></param>
        /// <param name="receive_date"></param>
        /// <param name="receive_time"></param>
        /// <returns></returns>
        public int updateStudentReceive(string receive_type,string year_id,string term_id,string dep_id,string receive_id,string stu_id, int old_Seq,string cancel_no,string receive_way,string receive_date,string receive_time)
        {
            int rc = -1;
            _err_msg = "";
            EntityFactory factory = new EntityFactory();

            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receive_type)
                .And(StudentReceiveEntity.Field.YearId, year_id)
                .And(StudentReceiveEntity.Field.TermId, term_id)
                .And(StudentReceiveEntity.Field.DepId, dep_id)
                .And(StudentReceiveEntity.Field.ReceiveId, receive_id)
                .And(StudentReceiveEntity.Field.StuId, stu_id)
                .And(StudentReceiveEntity.Field.OldSeq, old_Seq)
                .And(StudentReceiveEntity.Field.CancelNo, cancel_no);
            KeyValue[] fieldValues = new KeyValue[] {
                        new KeyValue(StudentReceiveEntity.Field.ReceiveDate, receive_date),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveTime, receive_time),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveWay, receive_way)
            };

            #region [20150915] 加強更新條件，避免更新到已繳或已銷的資料
            {
                where.And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty));
                where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                where.And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));
            }
            #endregion

            int count = 0;
            Result result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
            if (result.IsSuccess)
            {
                if (count == 0)
                {
                    rc = -2;
                    _err_msg = string.Format("更新失敗");
                }
                else
                {
                    rc = 0;
                }
            }
            else
            {
                rc = -9;
                _err_msg = result.Message;
            }
            return rc;
        }

        /// <summary>
        /// 取得學生繳費單資料 (依 YearId Desc, TermId Desc, ReceiveType Asc, CreateDate Desc, OldSeq Desc 排序)
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="receiveType"></param>
        /// <param name="cancelNo"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public int GetStudentReceive2(EntityFactory factory, string receiveType, string cancelNo, out StudentReceiveEntity[] datas)
        {
            datas = null;
            string key = string.Format("receive_type={0},cancel_no={1}", receiveType, cancelNo);

            if (factory == null || !factory.IsReady())
            {
                _err_msg = string.Format("[GetStudentReceive2] 讀取學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, "缺少資料庫存取物件");
                return -1;
            }

            _err_msg = "";
            int rc = -1;

            try
            {
                #region [MDY:202204XX] 2022擴充案 只取啟用學年的資料
                #region [OLD]
                //Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                //    .And(StudentReceiveEntity.Field.CancelNo, cancelNo);
                //KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
                //orderbys.Add(StudentReceiveEntity.Field.YearId, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveEntity.Field.TermId, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveEntity.Field.ReceiveType, OrderByEnum.Asc);
                //orderbys.Add(StudentReceiveEntity.Field.CreateDate, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveEntity.Field.OldSeq, OrderByEnum.Desc);
                //Result result = factory.SelectAll<StudentReceiveEntity>(where, orderbys, out datas);

                //#region [MDY:20180414] 間隔30秒後重取資料，最多兩次
                //#region 第一次重取
                //if (!result.IsSuccess)
                //{
                //    System.Threading.Thread.Sleep(30 * 1000);  //先睡30秒
                //    result = factory.SelectAll<StudentReceiveEntity>(where, orderbys, out datas);
                //}
                //#endregion

                //#region 第二次重取
                //if (!result.IsSuccess)
                //{
                //    System.Threading.Thread.Sleep(30 * 1000);  //先睡30秒
                //    result = factory.SelectAll<StudentReceiveEntity>(where, orderbys, out datas);
                //}
                //#endregion
                //#endregion
                #endregion

                Result result = null;
                {
                    string sql = $@"
SELECT S.* 
  FROM {StudentReceiveEntity.TABLE_NAME} S
 WHERE S.{StudentReceiveEntity.Field.ReceiveType} = @ReceiveType
   AND S.{StudentReceiveEntity.Field.CancelNo} = @CancelNo
   AND EXISTS (SELECT 1 FROM Year_List Y WHERE Y.Year_Id = S.Year_Id AND (Y.Enabled = 'Y' OR Y.Enabled IS NULL))
 ORDER BY {StudentReceiveEntity.Field.YearId} DESC, {StudentReceiveEntity.Field.TermId} DESC, {StudentReceiveEntity.Field.ReceiveType} ASC
     , {StudentReceiveEntity.Field.CreateDate} DESC, {StudentReceiveEntity.Field.OldSeq} DESC
".Trim();
                    KeyValue[] parameters = new KeyValue[]
                    {
                        new KeyValue("@ReceiveType", receiveType),
                        new KeyValue("@CancelNo", cancelNo)
                    };

                    result = factory.SelectSql<StudentReceiveEntity>(sql, parameters, 0, 0, out datas);

                    #region [MDY:20180414] 間隔30秒後重取資料，最多兩次
                    #region 第一次重取
                    if (!result.IsSuccess)
                    {
                        System.Threading.Thread.Sleep(30 * 1000);  //先睡30秒
                        result = factory.SelectSql<StudentReceiveEntity>(sql, parameters, 0, 0, out datas);
                    }
                    #endregion

                    #region 第二次重取
                    if (!result.IsSuccess)
                    {
                        System.Threading.Thread.Sleep(30 * 1000);  //先睡30秒
                        result = factory.SelectSql<StudentReceiveEntity>(sql, parameters, 0, 0, out datas);
                    }
                    #endregion
                    #endregion
                }
                #endregion

                if (!result.IsSuccess)
                {
                    #region [MDY:20180414] 將例外訊息寫到日誌
                    #region [Old]
                    //_err_msg = string.Format("[GetStudentReceive2] 讀取學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, result.Message);
                    #endregion

                    if (result.Exception != null)
                    {
                        _err_msg = string.Format("[GetStudentReceive2] 讀取學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, result.Exception.Message);
                    }
                    else
                    {
                        _err_msg = string.Format("[GetStudentReceive2] 讀取學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, result.Message);
                    }
                    #endregion
                }
                else
                {
                    if (datas == null || datas.Length == 0)
                    {
                        rc = 0;
                        _err_msg = string.Format("[GetStudentReceive2] 讀取學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, "查無學生繳費單資料");
                    }
                    else
                    {
                        rc = datas.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                _err_msg = string.Format("[GetStudentReceive2] 讀取學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, ex.Message);
            }
            return rc;
        }
        #endregion

        #region [MDY:20190226] 新版異業代收預銷（已繳待銷）處理相關
        /// <summary>
        /// 取得 (銷帳用) 學生繳費資料 (銷帳相關欄位) + 學生資料 (學號、名稱、身分證號) View (依 YearId Desc, TermId Desc, ReceiveType Asc, CreateDate Desc, OldSeq Desc 排序)
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="receiveType"></param>
        /// <param name="cancelNo"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public Result GetStudentReceiveView7(EntityFactory factory, string receiveType, string cancelNo, out StudentReceiveView7[] datas)
        {
            datas = null;
            _err_msg = "";

            #region 檢查參數
            if (factory == null)
            {
                factory = new EntityFactory();
            }
            else if (!factory.IsReady())
            {
                return new Result(false, "資料庫存取物件不正確", CoreStatusCode.S_INVALID_FACTORY, null);
            }
            if (String.IsNullOrWhiteSpace(receiveType) || String.IsNullOrWhiteSpace(cancelNo))
            {
                return new Result(false, "缺少查詢條件參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            try
            {
                #region [MDY:202204XX] 2022擴充案 只取啟用學年的資料
                #region [OLD]
                //Expression where = new Expression(StudentReceiveView7.Field.ReceiveType, receiveType)
                //    .And(StudentReceiveView7.Field.CancelNo, cancelNo)
                //    .And(StudentReceiveView7.Field.ReceiveAmount, RelationEnum.Greater, 0);

                //KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
                //orderbys.Add(StudentReceiveView7.Field.YearId, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveView7.Field.TermId, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveView7.Field.ReceiveType, OrderByEnum.Asc);
                //orderbys.Add(StudentReceiveView7.Field.CreateDate, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveView7.Field.OldSeq, OrderByEnum.Desc);

                //Result result = factory.SelectAll<StudentReceiveView7>(where, orderbys, out datas);

                //#region [MDY:20180414] 間隔30秒後重取資料，最多兩次
                //#region 第一次重取
                //if (!result.IsSuccess)
                //{
                //    System.Threading.Thread.Sleep(30 * 1000);  //先睡30秒
                //    result = factory.SelectAll<StudentReceiveView7>(where, orderbys, out datas);
                //}
                //#endregion

                //#region 第二次重取
                //if (!result.IsSuccess)
                //{
                //    System.Threading.Thread.Sleep(30 * 1000);  //先睡30秒
                //    result = factory.SelectAll<StudentReceiveView7>(where, orderbys, out datas);
                //}
                //#endregion
                //#endregion
                #endregion

                Result result = null;
                {
                    string sql = $@"
SELECT SR.{StudentReceiveEntity.Field.ReceiveType}, SR.{StudentReceiveEntity.Field.YearId}, SR.{StudentReceiveEntity.Field.TermId}
     , SR.{StudentReceiveEntity.Field.DepId}, SR.{StudentReceiveEntity.Field.ReceiveId}
     , SR.{StudentReceiveEntity.Field.StuId}, SR.{StudentReceiveEntity.Field.OldSeq}
     , SR.{StudentReceiveEntity.Field.CancelNo}, SR.{StudentReceiveEntity.Field.CancelAtmno}, SR.{StudentReceiveEntity.Field.CancelSmno}
     , SR.{StudentReceiveEntity.Field.ReceiveAmount}, SR.{StudentReceiveEntity.Field.ReceiveAtmamount}, SR.{StudentReceiveEntity.Field.ReceiveSmamount}
     , SR.{StudentReceiveEntity.Field.ReceiveDate}, SR.{StudentReceiveEntity.Field.ReceiveTime}, SR.{StudentReceiveEntity.Field.ReceiveWay}
     , SR.{StudentReceiveEntity.Field.AccountDate}, SR.{StudentReceiveEntity.Field.ReceivebankId}, SR.{StudentReceiveEntity.Field.CancelFlag}
     , SM.{StudentMasterEntity.Field.Name}, SM.{StudentMasterEntity.Field.IdNumber}
     , SR.{StudentReceiveEntity.Field.CreateDate}, SR.{StudentReceiveEntity.Field.NCCardFlag}
  FROM [{StudentReceiveEntity.TABLE_NAME}] AS SR
  LEFT JOIN [{StudentMasterEntity.TABLE_NAME}] AS SM 
             ON SR.{StudentReceiveEntity.Field.ReceiveType} = SM.{StudentMasterEntity.Field.ReceiveType}
            AND SR.{StudentReceiveEntity.Field.DepId} = SM.{StudentMasterEntity.Field.DepId}
            AND SR.{StudentReceiveEntity.Field.StuId} = SM.{StudentMasterEntity.Field.Id}
 WHERE SR.[{StudentReceiveEntity.Field.ReceiveType}] = @ReceiveType
   AND SR.[{StudentReceiveEntity.Field.CancelNo}] = @CancelNo
   AND SR.{StudentReceiveEntity.Field.ReceiveAmount} > 0
 ORDER BY SR.[{StudentReceiveEntity.Field.YearId}] DESC, SR.[{StudentReceiveEntity.Field.TermId}] DESC, SR.[{StudentReceiveEntity.Field.ReceiveType}] ASC
     , SR.[{StudentReceiveEntity.Field.CreateDate}] DESC, SR.[{StudentReceiveEntity.Field.OldSeq}] DESC
".Trim();

                    KeyValue[] parameters = new KeyValue[]
                    {
                        new KeyValue("@ReceiveType", receiveType),
                        new KeyValue("@CancelNo", cancelNo)
                    };

                    result = factory.SelectSql<StudentReceiveView7>(sql, parameters, 0, 0, out datas);

                    #region [MDY:20180414] 間隔30秒後重取資料，最多兩次
                    #region 第一次重取
                    if (!result.IsSuccess)
                    {
                        System.Threading.Thread.Sleep(30 * 1000);  //先睡30秒
                        result = factory.SelectSql<StudentReceiveView7>(sql, parameters, 0, 0, out datas);
                    }
                    #endregion

                    #region 第二次重取
                    if (!result.IsSuccess)
                    {
                        System.Threading.Thread.Sleep(30 * 1000);  //先睡30秒
                        result = factory.SelectSql<StudentReceiveView7>(sql, parameters, 0, 0, out datas);
                    }
                    #endregion
                    #endregion
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                return new Result(false, string.Format("讀取資料發生例外 (receive_type={0},cancel_no={1})", receiveType, cancelNo), CoreStatusCode.E_SELECT_DATA_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 更新預銷資料相關欄位 (即已繳待銷相關欄位：代收日、代收時間、代收管道)
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="stuId"></param>
        /// <param name="oldSeq"></param>
        /// <param name="cancelNo"></param>
        /// <param name="receiveWay"></param>
        /// <param name="receiveDate"></param>
        /// <param name="receiveTime"></param>
        /// <returns></returns>
        public Result UpdateStudentReceive(EntityFactory factory, string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int oldSeq, string cancelNo, string receiveDate, string receiveTime, string receiveWay)
        {
            _err_msg = "";

            #region 檢查參數
            if (factory == null)
            {
                factory = new EntityFactory();
            }
            else if (!factory.IsReady())
            {
                return new Result(false, "資料庫存取物件不正確", CoreStatusCode.S_INVALID_FACTORY, null);
            }
            if (String.IsNullOrWhiteSpace(receiveType) || String.IsNullOrWhiteSpace(cancelNo))
            {
                return new Result(false, "缺少查詢條件參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            try
            {
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                    .And(StudentReceiveEntity.Field.YearId, yearId)
                    .And(StudentReceiveEntity.Field.TermId, termId)
                    .And(StudentReceiveEntity.Field.DepId, depId)
                    .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                    .And(StudentReceiveEntity.Field.StuId, stuId)
                    .And(StudentReceiveEntity.Field.OldSeq, oldSeq)
                    .And(StudentReceiveEntity.Field.CancelNo, cancelNo)
                    .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty))
                    .And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty))
                    .And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));

                KeyValue[] fieldValues = new KeyValue[] {
                    new KeyValue(StudentReceiveEntity.Field.ReceiveDate, receiveDate),
                    new KeyValue(StudentReceiveEntity.Field.ReceiveTime, receiveTime),
                    new KeyValue(StudentReceiveEntity.Field.ReceiveWay, receiveWay)
                };

                int count = 0;
                Result result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                if (result.IsSuccess && count == 0)
                {
                    result = new Result(false, "無資料被更新", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new Result(false, "更新欄位資料發生例外", CoreStatusCode.E_SELECT_DATA_EXCEPTION, ex);
            }
        }
        #endregion


        #region [MDY:20170925] 取得指定商家代號與銷帳編號的有做支付寶交易且轉檔的學生繳費資料 View 陣列
        /// <summary>
        /// 取得使用支付寶繳費的學生繳費資料
        /// </summary>
        /// <param name="factory">指定資料庫存取物件</param>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="cancelNo">指定銷帳編號</param>
        /// <param name="datas">傳回 使用支付寶交易且已清算的學生繳費資料 物件陣列</param>
        /// <returns>成功傳回 true，否則傳回 flase。錯誤訊息由 err_msg 屬性取得</returns>
        public bool GetStudentReceiveView6(EntityFactory factory, string receiveType, string cancelNo, out StudentReceiveView6[] datas)
        {
            _err_msg = "";
            datas = null;
            bool isOK = false;
            string key = string.Format("receive_type={0},cancel_no={1}", receiveType, cancelNo);
            try
            {
                if (factory == null || !factory.IsReady())
                {
                    _err_msg = string.Format("[GetStudentReceiveView6] 讀取支付寶繳費的學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, "缺少資料庫存取物件");
                    return false;
                }
                if (String.IsNullOrWhiteSpace(receiveType) || String.IsNullOrWhiteSpace(cancelNo))
                {
                    _err_msg = string.Format("[GetStudentReceiveView6] 讀取支付寶繳費的學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, "缺少查詢參數");
                    return false;
                }

                #region [MDY:202204XX] 2022擴充案 只取啟用學年的資料
                #region [OLD]
                //Expression where = new Expression(StudentReceiveView6.Field.ReceiveType, receiveType)
                //    .And(StudentReceiveView6.Field.CancelNo, cancelNo);
                //KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
                //orderbys.Add(StudentReceiveView6.Field.YearId, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveView6.Field.TermId, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveView6.Field.ReceiveType, OrderByEnum.Asc);
                //orderbys.Add(StudentReceiveView6.Field.CreateDate, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveView6.Field.OldSeq, OrderByEnum.Desc);
                //Result result = factory.SelectAll<StudentReceiveView6>(where, orderbys, out datas);
                #endregion

                Result result = null;
                {
                    string sql = $@"
SELECT *
     , ISNULL((SELECT TOP 1 [{InboundTxnDtlEntity.Field.Sn}] FROM [{InboundTxnDtlEntity.TABLE_NAME}] AS B
                WHERE B.[{InboundTxnDtlEntity.Field.ReceiveType}] = A.[{StudentReceiveEntity.Field.ReceiveType}]
                  AND B.[{InboundTxnDtlEntity.Field.YearId}] = A.[{StudentReceiveEntity.Field.YearId}]
                  AND B.[{InboundTxnDtlEntity.Field.TermId}] = A.[{StudentReceiveEntity.Field.TermId}]
                  AND B.[{InboundTxnDtlEntity.Field.DepId}] = A.[{StudentReceiveEntity.Field.DepId}]
                  AND B.[{InboundTxnDtlEntity.Field.ReceiveId}] = A.[{StudentReceiveEntity.Field.ReceiveId}]
                  AND B.[{InboundTxnDtlEntity.Field.StuId}] = A.[{StudentReceiveEntity.Field.StuId}]
                  AND B.[{InboundTxnDtlEntity.Field.Seq}] = A.[{StudentReceiveEntity.Field.OldSeq}]
                  AND B.[{InboundTxnDtlEntity.Field.InboundFile}] IS NOT NULL AND B.[{InboundTxnDtlEntity.Field.InboundFile}] <> '' 
                  AND B.[{InboundTxnDtlEntity.Field.InboundData}] IS NOT NULL AND B.[{InboundTxnDtlEntity.Field.InboundData}] <> ''
                  AND B.[{InboundTxnDtlEntity.Field.TxnTime}] < GETDATE()
                ORDER BY B.[{InboundTxnDtlEntity.Field.TxnTime}] DESC), 0) AS [TXN_SN]
  FROM [{StudentReceiveEntity.TABLE_NAME}] AS A
 WHERE A.[{StudentReceiveEntity.Field.ReceiveType}] = @ReceiveType
   AND A.[{StudentReceiveEntity.Field.CancelNo}] = @CancelNo
   AND EXISTS (SELECT 1 FROM [{InboundTxnDtlEntity.TABLE_NAME}] AS B
                WHERE B.[{InboundTxnDtlEntity.Field.ReceiveType}] = A.[{StudentReceiveEntity.Field.ReceiveType}]
                  AND B.[{InboundTxnDtlEntity.Field.YearId}] = A.[{StudentReceiveEntity.Field.YearId}]
                  AND B.[{InboundTxnDtlEntity.Field.TermId}] = A.[{StudentReceiveEntity.Field.TermId}]
                  AND B.[{InboundTxnDtlEntity.Field.DepId}] = A.[{StudentReceiveEntity.Field.DepId}]
                  AND B.[{InboundTxnDtlEntity.Field.ReceiveId}] = A.[{StudentReceiveEntity.Field.ReceiveId}]
                  AND B.[{InboundTxnDtlEntity.Field.StuId}] = A.[{StudentReceiveEntity.Field.StuId}]
                  AND B.[{InboundTxnDtlEntity.Field.Seq}] = A.[{StudentReceiveEntity.Field.OldSeq}]
                  AND B.[{InboundTxnDtlEntity.Field.InboundFile}] IS NOT NULL AND B.[{InboundTxnDtlEntity.Field.InboundFile}] <> ''
                  AND B.[{InboundTxnDtlEntity.Field.InboundData}] IS NOT NULL AND B.[{InboundTxnDtlEntity.Field.InboundData}] <> ''
                  AND B.[{InboundTxnDtlEntity.Field.TxnTime}] < GETDATE())
".Trim();
                    KeyValue[] parameters = new KeyValue[]
                    {
                        new KeyValue("@ReceiveType", receiveType),
                        new KeyValue("@CancelNo", cancelNo)
                    };

                    result = factory.SelectSql<StudentReceiveView6>(sql, parameters, 0, 0, out datas);
                }
                #endregion

                if (result.IsSuccess)
                {
                    isOK = true;
                }
                else
                {
                    isOK = false;
                    _err_msg = string.Format("[GetStudentReceiveView6] 讀取支付寶繳費的學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, result.Message);
                }
            }
            catch (Exception ex)
            {
                isOK = false;
                _err_msg = string.Format("[GetStudentReceiveView6] 讀取支付寶繳費的學生繳費單資料發生例外，key={0}，錯誤訊息={1}", key, ex.Message);
            }
            return isOK;
        }
        #endregion

        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 取得指定商家代號與銷帳編號的有做財金信用卡交易的學生繳費資料 View 陣列
        /// <summary>
        /// 取得使用財金信用卡交易的學生繳費資料
        /// </summary>
        /// <param name="factory">指定資料庫存取物件</param>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="cancelNo">指定銷帳編號</param>
        /// <param name="datas">傳回 使用財金信用卡交易的學生繳費資料 物件陣列</param>
        /// <returns></returns>
        public bool GetStudentReceiveView8(EntityFactory factory, string receiveType, string cancelNo, out StudentReceiveView8[] datas)
        {
            _err_msg = "";
            datas = null;
            bool isOK = false;
            string key = string.Format("receive_type={0},cancel_no={1}", receiveType, cancelNo);
            try
            {
                if (factory == null || !factory.IsReady())
                {
                    _err_msg = string.Format("[GetStudentReceiveView8] 讀取財金信用卡繳費的學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, "缺少資料庫存取物件");
                    return false;
                }
                if (String.IsNullOrWhiteSpace(receiveType) || String.IsNullOrWhiteSpace(cancelNo))
                {
                    _err_msg = string.Format("[GetStudentReceiveView8] 讀取財金信用卡繳費的學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, "缺少查詢參數");
                    return false;
                }

                #region [MDY:202204XX] 2022擴充案 只取啟用學年的資料
                #region [OLD]
                //Expression where = new Expression(StudentReceiveView8.Field.ReceiveType, receiveType)
                //    .And(StudentReceiveView8.Field.CancelNo, cancelNo);
                //KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
                //orderbys.Add(StudentReceiveView8.Field.YearId, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveView8.Field.TermId, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveView8.Field.ReceiveType, OrderByEnum.Asc);
                //orderbys.Add(StudentReceiveView8.Field.CreateDate, OrderByEnum.Desc);
                //orderbys.Add(StudentReceiveView8.Field.OldSeq, OrderByEnum.Desc);
                //Result result = factory.SelectAll<StudentReceiveView8>(where, orderbys, out datas);
                #endregion

                Result result = null;
                {
                    string sql = $@"
SELECT *
     , ISNULL((SELECT TOP 1 [{CCardTxnDtlEntity.Field.TxnId}] FROM [{CCardTxnDtlEntity.TABLE_NAME}] AS B
                WHERE B.[{CCardTxnDtlEntity.Field.ReceiveType}] = A.[{StudentReceiveEntity.Field.ReceiveType}]
                  AND B.[{CCardTxnDtlEntity.Field.YearId}] = A.[{StudentReceiveEntity.Field.YearId}]
                  AND B.[{CCardTxnDtlEntity.Field.TermId}] = A.[{StudentReceiveEntity.Field.TermId}]
                  AND B.[{CCardTxnDtlEntity.Field.DepId}] = A.[{StudentReceiveEntity.Field.DepId}]
                  AND B.[{CCardTxnDtlEntity.Field.ReceiveId}] = A.[{StudentReceiveEntity.Field.ReceiveId}]
                  AND B.[{CCardTxnDtlEntity.Field.StudentNo}] = A.[{StudentReceiveEntity.Field.StuId}]
                  AND B.[{CCardTxnDtlEntity.Field.OldSeq}] = A.[{StudentReceiveEntity.Field.OldSeq}]
                  AND B.[{CCardTxnDtlEntity.Field.Rid}] = A.[{StudentReceiveEntity.Field.CancelNo}]
                  AND B.[{CCardTxnDtlEntity.Field.Status}] IN ('1', '2') AND B.[{CCardTxnDtlEntity.Field.CreateDate}] < GETDATE()
                  AND B.[{CCardTxnDtlEntity.Field.ApNo}] = (CASE A.[{StudentReceiveEntity.Field.NCCardFlag}] WHEN 'Y' THEN '4' ELSE '1'END)
                ORDER BY B.[{CCardTxnDtlEntity.Field.Status}] DESC, B.[{CCardTxnDtlEntity.Field.CreateDate}] DESC), 0) AS [{CCardTxnDtlEntity.Field.TxnId}]
  FROM [{StudentReceiveEntity.TABLE_NAME}] AS A
 WHERE A.[{StudentReceiveEntity.Field.ReceiveType}] = @ReceiveType
   AND A.[{StudentReceiveEntity.Field.CancelNo}] = @CancelNo
   AND EXISTS (SELECT 1 FROM [{CCardTxnDtlEntity.TABLE_NAME}] AS B
                WHERE B.[{CCardTxnDtlEntity.Field.ReceiveType}] = A.[{StudentReceiveEntity.Field.ReceiveType}]
                  AND B.[{CCardTxnDtlEntity.Field.YearId}] = A.[{StudentReceiveEntity.Field.YearId}]
                  AND B.[{CCardTxnDtlEntity.Field.TermId}] = A.[{StudentReceiveEntity.Field.TermId}]
                  AND B.[{CCardTxnDtlEntity.Field.DepId}] = A.[{StudentReceiveEntity.Field.DepId}]
                  AND B.[{CCardTxnDtlEntity.Field.ReceiveId}] = A.[{StudentReceiveEntity.Field.ReceiveId}]
                  AND B.[{CCardTxnDtlEntity.Field.StudentNo}] = A.[{StudentReceiveEntity.Field.StuId}]
                  AND B.[{CCardTxnDtlEntity.Field.OldSeq}] = A.[{StudentReceiveEntity.Field.OldSeq}]
                  AND B.[{CCardTxnDtlEntity.Field.Rid}] = A.[{StudentReceiveEntity.Field.CancelNo}]
                  AND B.[{CCardTxnDtlEntity.Field.Status}] IN ('1', '2') AND B.[{CCardTxnDtlEntity.Field.CreateDate}] < GETDATE()
                  AND B.[{CCardTxnDtlEntity.Field.ApNo}] = (CASE A.[{StudentReceiveEntity.Field.NCCardFlag}] WHEN 'Y' THEN '4' ELSE '1'END))
 ORDER BY A.[{StudentReceiveEntity.Field.YearId}] DESC, A.[{StudentReceiveEntity.Field.TermId}] DESC, A.[{StudentReceiveEntity.Field.ReceiveType}] ASC
     , A.[{StudentReceiveEntity.Field.CreateDate}] DESC, A.[{StudentReceiveEntity.Field.OldSeq}] DESC
".Trim();
                    KeyValue[] parameters = new KeyValue[]
                    {
                        new KeyValue("@ReceiveType", receiveType),
                        new KeyValue("@CancelNo", cancelNo)
                    };

                    result = factory.SelectSql<StudentReceiveView8>(sql, parameters, 0, 0, out datas);
                }
                #endregion

                if (result.IsSuccess)
                {
                    isOK = true;
                }
                else
                {
                    isOK = false;
                    _err_msg = string.Format("[GetStudentReceiveView8] 讀取財金信用卡繳費的學生繳費單資料發生錯誤，key={0}，錯誤訊息={1}", key, result.Message);
                }
            }
            catch (Exception ex)
            {
                isOK = false;
                _err_msg = string.Format("[GetStudentReceiveView8] 讀取財金信用卡繳費的學生繳費單資料發生例外，key={0}，錯誤訊息={1}", key, ex.Message);
            }
            return isOK;
        }
        #endregion

        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 取得指定商家代號的國際信用卡手續費
        private KeyValueList<decimal> _NCHandlingFeeRates = null;
        /// <summary>
        /// 取得指定商家代號、指定金額的國際信用卡手續費
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="receiveType"></param>
        /// <param name="amount"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public decimal? GetNCHandlingFee(EntityFactory factory, string receiveType, decimal amount, out string errmsg)
        {
            errmsg = null;
            KeyValue<decimal> handlingFeeRate = null;

            #region 取得手續費率
            try
            {
                if (_NCHandlingFeeRates != null)
                {
                    handlingFeeRate = _NCHandlingFeeRates.Find(x => x.Key == receiveType);
                }
                else
                {
                    _NCHandlingFeeRates = new KeyValueList<decimal>();
                }
                if (handlingFeeRate == null)
                {
                    string sql = @"SELECT [Handling_Fee_Rate] FROM [School_Rtype] WHERE [Receive_Type] = @RECEIVE_TYPE AND ([TerminalId2] IS NOT NULL AND [TerminalId2] != '') AND [Handling_Fee_Rate] IS NOT NULL";
                    KeyValue[] parameters = new KeyValue[] { new KeyValue("@RECEIVE_TYPE", receiveType) };
                    object value = null;
                    Result result = factory.ExecuteScalar(sql, parameters, out value);
                    if (result.IsSuccess)
                    {
                        if (value != null)
                        {
                            handlingFeeRate = new KeyValue<decimal>(receiveType, Convert.ToDecimal(value));
                            _NCHandlingFeeRates.Add(handlingFeeRate);
                        }
                        else
                        {
                            errmsg = "查無該商家代號的國際信用卡手續費率";
                        }
                    }
                    else
                    {
                        errmsg = result.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = "讀取國際信用卡手續費率發生例外，" + ex.Message;
            }
            #endregion

            if (handlingFeeRate != null)
            {
                return Math.Round(handlingFeeRate.Value * amount, MidpointRounding.AwayFromZero);
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region [Old]
        //public bool N160toCancelDebts()
        //{
        //    bool rc = false;
        //    return rc;
        //}
        #endregion

        #region [MDY:20170925] 銷帳處理作業 重新整理 Code 並增加支付寶代收日期、代收時間處理
        #region [Old]
//        /// <summary>
//        /// 銷帳作業
//        /// </summary>
//        /// <param name="account_date">入帳日(民國年 yyymmdd)</param>
//        /// <returns>成功或失敗，失敗可查err_msg</returns>
//        public bool CancelByAccountDate(string account_date)
//        {
//            bool rc = false;
//            _err_msg = "";
//            string log = "";
//            StringBuilder logs = new StringBuilder();
//            Int32 count = 0;
//            Int32 success = 0;
//            Int32 fail = 0;

//            Result result = null;
//            CancelDebtsEntity[] cancel_debtss;
//            StudentReceiveEntity[] student_receives;
//            StudentReceiveEntity student_receive = null;
//            try
//            {
//                List<string> upctcbReceiveTypes = null;
//                if (!this.GetUPCTCBReceiveTypes(out upctcbReceiveTypes))
//                {
//                    //_err_msg = string.Format("[CancelByAccountDate] 錯誤訊息={1}", account_date, _err_msg);
//                    return rc;
//                }

//                if (!getCancelDebtsByAccountDate(account_date, out cancel_debtss))
//                {
//                    //_err_msg = string.Format("[CancelByAccountDate] 錯誤訊息={1}", account_date, _err_msg);
//                    return rc;
//                }
//                else if (cancel_debtss == null || cancel_debtss.Length == 0)
//                {
//                    _err_msg = String.Format("查無待銷帳的 cancel_debts 資料 (入帳日={0})", account_date);
//                    return true;
//                }

//                using (EntityFactory factory = new EntityFactory())
//                {
//                    #region [MDY:20160301] 取得有 6R6 的商家代號
//                    decimal z6R6MinAmount = 0;
//                    decimal z6R6MaxAmount = 20000;
//                    List<string> z6R6ReceiveTypes = null;
//                    {
//                        ReceiveChannelEntity[] datas = null;
//                        Expression where = new Expression(ReceiveChannelEntity.Field.BarcodeId, "6R6");
//                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
//                        orderbys.Add(ReceiveChannelEntity.Field.ReceiveType, OrderByEnum.Asc);
//                        result = factory.SelectAll<ReceiveChannelEntity>(where, orderbys, out datas);
//                        if (result.IsSuccess)
//                        {
//                            if (datas != null && datas.Length > 0)
//                            {
//                                z6R6ReceiveTypes = new List<string>(datas.Length);
//                                foreach (ReceiveChannelEntity data in datas)
//                                {
//                                    z6R6ReceiveTypes.Add(data.ReceiveType);
//                                }
//                            }
//                            else
//                            {
//                                z6R6ReceiveTypes = new List<string>(0);
//                            }
//                        }
//                        else
//                        {
//                            _err_msg = string.Format("[CancelByAccountDate] 讀取有 6R6 代收管道的商家代號資料失敗，錯誤訊息={1}", account_date, _err_msg);
//                            return rc;
//                        }
//                    }
//                    #endregion

//                    count = cancel_debtss.Length;
//                    DateTime ndt = DateTime.Now;
//                    Int32 rcds = 0;
//                    bool procStatus = false;
//                    string problem_flag = "";
//                    string problem_desc = "";

//                    foreach (CancelDebtsEntity cancel_debts in cancel_debtss)
//                    {
//                        problem_flag = "";
//                        problem_desc = "";
//                        procStatus = false;

//                        #region {MDY:20160921] 紀錄是否發生資料庫 Timeout
//                        bool isDbTimeout = false;
//                        #endregion

//                        #region 處理
//                        string receive_type = cancel_debts.ReceiveType;
//                        string cancel_no = cancel_debts.CancelNo;
//                        decimal amount = cancel_debts.ReceiveAmount;
//                        bool isUPCTCBReceiveType = upctcbReceiveTypes.Contains(receive_type);

//                        #region [MDY:20160607] 處理更正交易資料
//                        #region 處理邏輯
//                        //1. 僅處理臨櫃管道的更正資料
//                        //2. 被更正的資料限制以更正資料中的交易日期與時間往前找第一筆相同交易日期、商家代號、虛擬帳號、金額、分行代碼的臨櫃資料
//                        //3. 已被更正的資料不能重覆更正
//                        //4. 找不被更正的資料僅在日誌檔中記錄，不另外處理
//                        //5. 不管更正資料或被更正的資料，都會保留在中心入帳資料表裡 (Cancel_Debts)
//                        //5. 被更正的資料如果為問題檔，則將該資料狀態欄位 (Status) 改為被更正，Reserve2 欄位改為被更正，並刪除對應的問題檔
//                        //6. 被更正的資料如果為已銷帳，則將該資料狀態欄位 (Status) 改為被更正，Reserve2 欄位改為被更正，並清除學生繳費資料表 (Student_Receive) 銷帳資料相關欄位
//                        //7. 更正資料的狀態欄位 (Status) 改為已更正，RollbackDate 設為當下的處理日期時間
//                        #endregion

//                        if (cancel_debts.Reserve2 == D00I70ECMarkCodeTexts.RECTIFY_CODE)
//                        {
//                            if (ChannelHelper.IsCashChannel(cancel_debts.ReceiveWay))
//                            {
//                                CancelDebtsEntity oData = null;
//                                string errmsg = this.GetCancelDebtsForRectify(factory, cancel_debts, out oData);
//                                if (String.IsNullOrEmpty(errmsg))
//                                {
//                                    if (oData == null)
//                                    {
//                                        fail++;
//                                        logs.AppendFormat("查無 (商家代號：{0}; 虛擬帳號：{1}; 代收金額：{2}; 代收時間：{3}; 資料來源序號：{4}) 可被更正資料", cancel_debts.ReceiveType, cancel_debts.CancelNo, cancel_debts.ReceiveAmount, cancel_debts.ReceiveTime, cancel_debts.SourceSeq).AppendLine();
//                                    }
//                                    else if (oData.Reserve2 != D00I70ECMarkCodeTexts.NORMAL_CODE)
//                                    {
//                                        fail++;
//                                        logs.AppendFormat("查無 (商家代號：{0}; 虛擬帳號：{1}; 代收金額：{2}; 代收時間：{3}; 資料來源序號：{4}) 可被更正資料，錯誤訊息：找到的資料更正記號不正確", cancel_debts.ReceiveType, cancel_debts.CancelNo, cancel_debts.ReceiveAmount, cancel_debts.ReceiveTime, cancel_debts.SourceSeq).AppendLine();
//                                    }
//                                    else
//                                    {
//                                        string extraLog = null;
//                                        errmsg = this.RectifyCancelDebtsData(factory, cancel_debts, oData, ndt, out extraLog);
//                                        if (String.IsNullOrEmpty(errmsg))
//                                        {
//                                            success++;
//                                            logs.AppendFormat("更正交易 (商家代號：{0}; 虛擬帳號：{1}; 代收金額：{2}; 代收時間：{3}; 資料來源序號：{4}) 處理成功，被更正資料 SNo = {5}", cancel_debts.ReceiveType, cancel_debts.CancelNo, cancel_debts.ReceiveAmount, cancel_debts.ReceiveTime, cancel_debts.SourceSeq, oData.SNo).AppendLine();
//                                        }
//                                        else
//                                        {
//                                            fail++;
//                                            logs.AppendFormat("處理更正交易資料 (商家代號：{0}; 虛擬帳號：{1}; 代收金額：{2}; 代收時間：{3}; 資料來源序號：{4}; 更正資料 SNo = {5}) 失敗，錯誤訊息={6}", cancel_debts.ReceiveType, cancel_debts.CancelNo, cancel_debts.ReceiveAmount, cancel_debts.ReceiveTime, cancel_debts.SourceSeq, oData.SNo, errmsg).AppendLine();
//                                        }
//                                        if (!String.IsNullOrEmpty(extraLog))
//                                        {
//                                            logs.AppendLine(extraLog);
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    fail++;
//                                    logs.AppendFormat("查詢可被更正資料失敗，錯誤訊息={0}", errmsg).AppendLine();
//                                }
//                            }
//                            else
//                            {
//                                fail++;
//                                logs.AppendFormat("更正交易 (商家代號：{0}; 虛擬帳號：{1}; 代收金額：{2}; 代收管道：{3}; 資料來源序號：{4}) 處理失敗，錯誤訊息：非臨櫃的的更正交易資料", cancel_debts.ReceiveType, cancel_debts.CancelNo, cancel_debts.ReceiveAmount, cancel_debts.ReceiveWay, cancel_debts.SourceSeq).AppendLine();
//                            }
//                            continue;
//                        }
//                        #endregion

//                        #region 處理正常交易資料
//                        student_receive = null;
//                        if (this.GetStudentReceive2(factory, receive_type, cancel_no, out student_receives) > 0)
//                        {
//                            #region 可以對應到學生繳費單資料
//                            bool found = false;

//                            #region [Old] 因為土銀的異業代收會先做已繳待銷，所以銷帳的時候如為異業代收要先銷
//                            //#region 找學生繳費單未銷帳的
//                            //for (Int32 i = 0; i < student_receives.Length; i++)
//                            //{
//                            //    student_receive = student_receives[i];
//                            //    if (student_receive.ReceiveWay == null && student_receive.ReceiveWay != "")
//                            //    {
//                            //        found = true;
//                            //        break;
//                            //    }
//                            //}
//                            //#endregion
//                            #endregion

//                            bool isPreCancelChannel = ChannelHelper.IsPreCancelChannel(cancel_debts.ReceiveWay);
//                            if (isPreCancelChannel)
//                            {
//                                #region 如果是有做已繳代銷的管道則先銷該管道的已繳待銷資料 (管道金額都要相同且無入帳日)
//                                foreach (StudentReceiveEntity one in student_receives)
//                                {
//                                    if (one.ReceiveWay == cancel_debts.ReceiveWay && one.ReceiveAmount == cancel_debts.ReceiveAmount && String.IsNullOrEmpty(one.AccountDate))
//                                    {
//                                        student_receive = one;
//                                        found = true;
//                                        break;
//                                    }
//                                }
//                                #endregion
//                            }

//                            if (!found)
//                            {
//                                #region 找學生繳費單未銷帳的 (已繳待銷的也不要，因為已繳待銷應該在上面被處理)
//                                foreach (StudentReceiveEntity one in student_receives)
//                                {
//                                    if (String.IsNullOrEmpty(one.ReceiveWay) && String.IsNullOrEmpty(one.ReceiveDate) && String.IsNullOrEmpty(one.AccountDate))
//                                    {
//                                        student_receive = one;
//                                        found = true;
//                                        break;
//                                    }
//                                }
//                                #endregion
//                            }

//                            if (found)
//                            {
//                                if (amount == student_receive.ReceiveAmount) //可以銷帳
//                                {
//                                    #region [Old] 改用 UpdateFields 已避免新增欄位後，此程式卻沒換版造成的更新失敗問題
//                                    //student_receive.ReceiveWay = cancel_debts.ReceiveWay;
//                                    //student_receive.ReceiveDate = cancel_debts.ReceiveDate;
//                                    //student_receive.ReceiveTime = cancel_debts.ReceiveTime;
//                                    //student_receive.ReceivebankId = cancel_debts.ReceiveBank;
//                                    //student_receive.AccountDate = cancel_debts.AccountDate;
//                                    //student_receive.CancelFlag = "9";
//                                    //student_receive.CancelDate = DateTime.Today.ToString("yyyyMMdd");
//                                    //int rcd = 0;
//                                    //result = factory.Update((StudentReceiveEntity)student_receive, out rcd);
//                                    #endregion

//                                    #region [Old] 因為土銀的異業代收會先做已繳待銷，且中心沒有代收日期與時間，所以異業代收不更新代收管道、代收日期與時間
//                                    //KeyValue[] fieldValues = new KeyValue[] {
//                                    //    new KeyValue(StudentReceiveEntity.Field.ReceiveWay, cancel_debts.ReceiveWay),
//                                    //    new KeyValue(StudentReceiveEntity.Field.ReceiveDate, cancel_debts.ReceiveDate),
//                                    //    new KeyValue(StudentReceiveEntity.Field.ReceiveTime, cancel_debts.ReceiveTime),
//                                    //    new KeyValue(StudentReceiveEntity.Field.ReceivebankId, cancel_debts.ReceiveBank),
//                                    //    new KeyValue(StudentReceiveEntity.Field.AccountDate, cancel_debts.AccountDate),
//                                    //    new KeyValue(StudentReceiveEntity.Field.CancelFlag, CancelFlagCodeTexts.RECEIVE_SELF),
//                                    //    new KeyValue(StudentReceiveEntity.Field.CancelDate, DateTime.Today.ToString("yyyyMMdd")),
//                                    //};
//                                    #endregion

//                                    KeyValueList fieldValues = new KeyValueList();
//                                    fieldValues
//                                        .Add(StudentReceiveEntity.Field.AccountDate, cancel_debts.AccountDate)
//                                        .Add(StudentReceiveEntity.Field.ReceivebankId, cancel_debts.ReceiveBank)
//                                        .Add(StudentReceiveEntity.Field.CancelFlag, CancelFlagCodeTexts.RECEIVE_SELF)
//                                        .Add(StudentReceiveEntity.Field.CancelDate, ndt.ToString("yyyyMMdd"));

//                                    if (!isPreCancelChannel)
//                                    {
//                                        //非做已繳代銷的管道：一律要更新代收管道、代收日期與時間
//                                        fieldValues
//                                            .Add(StudentReceiveEntity.Field.ReceiveWay, cancel_debts.ReceiveWay)
//                                            .Add(StudentReceiveEntity.Field.ReceiveDate, cancel_debts.ReceiveDate)
//                                            .Add(StudentReceiveEntity.Field.ReceiveTime, cancel_debts.ReceiveTime);
//                                    }
//                                    else
//                                    {
//                                        #region [MDY:20160301] 有做已繳代銷的管道：無代收管道時要更新代收管道、無代收日期且為超商 6R6 管道要與更新代收日期時間
//                                        if (String.IsNullOrWhiteSpace(student_receive.ReceiveWay))
//                                        {
//                                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveWay, cancel_debts.ReceiveWay);
//                                        }

//                                        #region [MDY:20160607] 修正有6R6的商家代號才處理
//                                        if (String.IsNullOrWhiteSpace(student_receive.ReceiveDate)
//                                            && amount >= z6R6MinAmount && amount <= z6R6MaxAmount
//                                            && ChannelHelper.IsSMChannel(cancel_debts.ReceiveWay)
//                                            && z6R6ReceiveTypes != null && z6R6ReceiveTypes.Count > 0 && z6R6ReceiveTypes.Contains(student_receive.ReceiveType))
//                                        {
//                                            fieldValues
//                                                .Add(StudentReceiveEntity.Field.ReceiveDate, cancel_debts.ReceiveDate)
//                                                .Add(StudentReceiveEntity.Field.ReceiveTime, cancel_debts.ReceiveTime);
//                                        }
//                                        #endregion

//                                        #endregion
//                                    }

//                                    #region [MDY:20170506] 增加支付寶管道手續費的處理
//                                    if (cancel_debts.ReceiveWay == ChannelHelper.ALIPAY)
//                                    {
//                                        DateTime maxTxnDate;
//                                        if (Common.TryConvertTWDate7(student_receive.ReceiveDate, out maxTxnDate))
//                                        {
//                                            maxTxnDate = maxTxnDate.AddDays(1); //因為 TxnTime 包含時間，所以取隔天作為最大交易日期
//                                        }
//                                        else if (Common.TryConvertTWDate7(cancel_debts.ReceiveDate, out maxTxnDate))
//                                        {
//                                            maxTxnDate = maxTxnDate.AddDays(1); //因為 TxnTime 包含時間，所以取隔天作為最大交易日期
//                                        }
//                                        else
//                                        {
//                                            maxTxnDate = DateTime.Today;    //基本上不會發生轉換失敗，萬一真的發生就用當天作為交易日期
//                                        }
//                                        decimal fee = 0;
//                                        result = this.GetAlipayFee(factory, student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId,
//                                            student_receive.StuId, student_receive.OldSeq, student_receive.CancelNo, student_receive.ReceiveAmount.Value, maxTxnDate, out fee);
//                                        if (result.IsSuccess)
//                                        {
//                                            fieldValues.Add(StudentReceiveEntity.Field.ProcessFee, fee);
//                                        }
//                                    }
//                                    else
//                                    {
//                                        result = new Result(true);
//                                    }
//                                    #endregion

//                                    if (result.IsSuccess)
//                                    {
//                                        Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, student_receive.ReceiveType)
//                                            .And(StudentReceiveEntity.Field.YearId, student_receive.YearId)
//                                            .And(StudentReceiveEntity.Field.TermId, student_receive.TermId)
//                                            .And(StudentReceiveEntity.Field.DepId, student_receive.DepId)
//                                            .And(StudentReceiveEntity.Field.ReceiveId, student_receive.ReceiveId)
//                                            .And(StudentReceiveEntity.Field.StuId, student_receive.StuId)
//                                            .And(StudentReceiveEntity.Field.OldSeq, student_receive.OldSeq);

//                                        #region [20150915] 加強更新條件，避免更新到已銷的資料
//                                        {
//                                            where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
//                                        }
//                                        #endregion

//                                        int rcd = 0;
//                                        result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out rcd);

//                                        #region [MDY:20160921] 判斷是否為資料庫 Timeout，如果是則 isDbTimeout 設為 true
//                                        if (!result.IsSuccess)
//                                        {
//                                            System.Data.Common.DbException dbException = result.Exception as System.Data.Common.DbException;
//                                            isDbTimeout = (dbException != null && dbException.Message.IndexOf("Timeout", StringComparison.CurrentCultureIgnoreCase) > -1);
//                                        }
//                                        #endregion
//                                    }

//                                    if (!result.IsSuccess) //更新失敗 problem_flag="1"
//                                    {
//                                        problem_flag = "1";
//                                        problem_desc = "更新學生繳費資料失敗，錯誤訊息=" + result.Message;
//                                        if (result.Exception != null)
//                                        {
//                                            problem_desc += "(" + result.Exception.Message + ")";
//                                        }
//                                        fail++;
//                                        log = string.Format("虛擬帳號{0}更新失敗，錯誤訊息={1}", cancel_debts.CancelNo, result.Message);
//                                        logs.AppendLine(log);
//                                    }
//                                    else
//                                    {
//                                        procStatus = true;
//                                        success++;
//                                    }
//                                }
//                                else //金額不符 problem_flag="2"
//                                {
//                                    problem_flag = "2";
//                                    problem_desc = "金額不符";
//                                    fail++;
//                                    log = string.Format("虛擬帳號{0}銷帳失敗，錯誤訊息={1}", cancel_debts.CancelNo, "金額不符");
//                                    logs.AppendLine(log);
//                                }
//                            }
//                            else //重複繳費 problem_flag="5"
//                            {
//                                problem_flag = "5";
//                                problem_desc = "重複繳費";
//                                log = string.Format("虛擬帳號{0}銷帳失敗，錯誤訊息={1}", cancel_debts.CancelNo, "重複繳費");
//                                logs.AppendLine(log);
//                                fail++;
//                            }
//                            #endregion
//                        }
//                        else //查無此銷帳編號 problem_flag="4"
//                        {
//                            problem_flag = "4";
//                            problem_desc = "查無此虛擬帳號";
//                            if (isUPCTCBReceiveType)
//                            {
//                                #region [MDY:20160607] FIX BUG
//                                #region [Old]
//                                //log = string.Format("虛擬帳號{0}銷帳失敗，錯誤訊息={1}", cancel_debts.CancelNo, "查無此虛擬帳號，且此商家代號屬於只上傳中信繳費單資料的種類");
//                                #endregion

//                                logs.AppendFormat("虛擬帳號{0}銷帳失敗，錯誤訊息={1}", cancel_debts.CancelNo, "查無此虛擬帳號，且此商家代號屬於只上傳中信繳費單資料的種類").AppendLine();
//                                #endregion
//                            }
//                            else
//                            {
//                                logs.AppendFormat("虛擬帳號{0}銷帳失敗，錯誤訊息={1}", cancel_debts.CancelNo, "查無此虛擬帳號").AppendLine();
//                            }
//                            fail++;
//                        }
//                        #endregion

//                        #region [MDY:20160921] 如果發生資料庫 Timeout，只寫 log 檔不新增問題檔並將狀態改為待處理
//                        if (isDbTimeout)
//                        {
//                            // log 檔前面已寫了，所以這裡不用再處理

//                            #region 狀態改為待處理
//                            cancel_debts.Status = "1";  //待處理
//                            cancel_debts.ModifyDate = ndt;
//                            result = factory.Update((CancelDebtsEntity)cancel_debts, out rcds);
//                            if (!result.IsSuccess)
//                            {
//                                log = string.Format("還原 cancel_debts 為待處理失敗，虛擬帳號={0}，錯誤訊息={1}", cancel_debts.CancelNo, result.Message);
//                                logs.AppendLine(log);
//                            }
//                            #endregion

//                            continue;
//                        }
//                        #endregion

//                        #region 銷帳失敗寫問題檔
//                        if (procStatus == false)
//                        {
//                            if (isUPCTCBReceiveType && problem_flag == "4")
//                            {
//                                //只上傳中信繳費單資料的商家代號，且查無此虛擬帳號，不寫問題檔
//                            }
//                            else
//                            {
//                                ProblemListEntity problem = new ProblemListEntity();
//                                problem.ReceiveType = cancel_debts.ReceiveType;
//                                problem.CancelNo = cancel_debts.CancelNo;
//                                problem.ProblemFlag = problem_flag;
//                                problem.BankId = cancel_debts.ReceiveBank;
//                                DateTime receiveDate;
//                                if (Common.TryConvertTWDate7(cancel_debts.ReceiveDate, out receiveDate))
//                                {
//                                    problem.ReceiveDate = receiveDate;
//                                }
//                                problem.ReceiveTime = cancel_debts.ReceiveTime;
//                                DateTime accountDate;
//                                if (Common.TryConvertTWDate7(cancel_debts.AccountDate, out accountDate))
//                                {
//                                    problem.AccountDate = accountDate;
//                                }
//                                problem.ProcessFee = 0;
//                                problem.PayAmount = cancel_debts.ReceiveAmount;
//                                problem.ProblemRemark = problem_desc;
//                                problem.ReceiveWay = cancel_debts.ReceiveWay;
//                                problem.CanceleEBNno = null;
//                                problem.UpFlag = null;
//                                problem.ReceiveAmount = student_receive == null ? null : student_receive.ReceiveAmount;
//                                problem.CancelReceive = 0;
//                                problem.FeeReceivable = 0;
//                                problem.FeePayable = 0;
//                                problem.PFlag = null;
//                                problem.CreateDate = DateTime.Now;
//                                problem.CreateMan = "SYSTEM";
//                                problem.UpdateDate = null;
//                                problem.UpdateMan = null;
//                                result = factory.Insert(problem, out count);
//                            }
//                        }
//                        #endregion

//                        #region 更新狀態
//                        if (procStatus == true)
//                        {
//                            cancel_debts.Status = CancelDebtsStatusCodeTexts.HAS_CANCELED_CODE;     //已銷帳
//                        }
//                        else
//                        {
//                            if (isUPCTCBReceiveType && problem_flag == "4")
//                            {
//                                cancel_debts.Status = CancelDebtsStatusCodeTexts.IS_EXCLUDED_CODE;  //免銷帳
//                            }
//                            else
//                            {
//                                cancel_debts.Status = CancelDebtsStatusCodeTexts.IS_FAILURE_CODE;   //銷帳失敗
//                            }
//                        }
//                        cancel_debts.ModifyDate = ndt;
//                        cancel_debts.CancelDate = ndt;
//                        result = factory.Update((CancelDebtsEntity)cancel_debts, out rcds);
//                        if (!result.IsSuccess)
//                        {
//                            log = string.Format("更新cancel_debts失敗，虛擬帳號={0}，錯誤訊息={1}", cancel_debts.CancelNo, result.Message);
//                            logs.AppendLine(log);
//                        }
//                        #endregion
//                        #endregion
//                    }
//                    log = string.Format("銷帳處理結果{0}共{1}筆，成功{2}筆，失敗{3}筆", System.Environment.NewLine, count, success, fail);
//                    logs.AppendLine(log);
//                    rc = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                _err_msg = string.Format("[CancelByAccountDate] 取入帳資料發生錯誤，account_date={0}，錯誤訊息={1}", account_date, ex.Message);
//                return rc;
//            }

//            return rc;
//        }

//        #region [MDY:20170506] 取得符合條件的支付寶交易資料的手續費
//        /// <summary>
//        /// 取得符合條件的支付寶交易資料的手續費 的 SQL
//        /// </summary>
//        private static string sqlGetAlipayFee = String.Format(@"
//SELECT TOP 1 [{0}] FROM [{1}] 
// WHERE [{2}] = @ReceiveType AND [{3}] = @YearId AND [{4}] = @TermId 
//   AND [{5}] = @DepId AND [{6}] = @ReceiveId AND [{7}] = @StuId AND [{8}] = @OldSeq
//   AND [{9}] = @CancelNo AND [{10}] = @ReceiveAmount
//   AND [{11}] < @maxTxnDate
//   AND ([{12}] IS NOT NULL AND [{12}] <> '')
// ORDER BY [{11}] DESC"
//                , InboundTxnDtlEntity.Field.Fee, InboundTxnDtlEntity.TABLE_NAME
//                , InboundTxnDtlEntity.Field.ReceiveType, InboundTxnDtlEntity.Field.YearId, InboundTxnDtlEntity.Field.TermId
//                , InboundTxnDtlEntity.Field.DepId, InboundTxnDtlEntity.Field.ReceiveId, InboundTxnDtlEntity.Field.StuId, InboundTxnDtlEntity.Field.Seq
//                , InboundTxnDtlEntity.Field.CancelNo, InboundTxnDtlEntity.Field.ReceiveAmount
//                , InboundTxnDtlEntity.Field.TxnTime, InboundTxnDtlEntity.Field.InboundFile);

//        /// <summary>
//        /// 取得符合條件的支付寶交易資料的手續費
//        /// </summary>
//        /// <param name="factory"></param>
//        /// <param name="receiveType"></param>
//        /// <param name="yearId"></param>
//        /// <param name="termId"></param>
//        /// <param name="depId"></param>
//        /// <param name="receiveId"></param>
//        /// <param name="stuId"></param>
//        /// <param name="oldSeq"></param>
//        /// <param name="cancelNo"></param>
//        /// <param name="receiveAmount"></param>
//        /// <param name="maxTxnDate"></param>
//        /// <param name="fee"></param>
//        /// <returns></returns>
//        private Result GetAlipayFee(EntityFactory factory, string receiveType, string yearId, string termId, string depId, string receiveId
//            , string stuId, int oldSeq, string cancelNo, decimal receiveAmount, DateTime maxTxnDate, out decimal fee)
//        {
//            fee = 0;

//            KeyValue[] parameters = new KeyValue[] {
//                new KeyValue("@ReceiveType", receiveType), new KeyValue("@YearId", yearId), new KeyValue("@TermId", termId), 
//                new KeyValue("@DepId", depId), new KeyValue("@ReceiveId", receiveId), new KeyValue("@StuId", stuId), new KeyValue("@OldSeq", oldSeq),
//                new KeyValue("@CancelNo", cancelNo), new KeyValue("@ReceiveAmount", receiveAmount),
//                new KeyValue("@maxTxnDate", maxTxnDate)
//            };
//            object value = null;
//            Result result = factory.ExecuteScalar(sqlGetAlipayFee, parameters, out value);
//            if (result.IsSuccess)
//            {
//                if (value == null)
//                {
//                    result = new Result(false, "查無符合的支付寶交易資料", ErrorCode.D_DATA_NOT_FOUND, null);
//                }
//                else
//                {
//                    fee = Convert.ToDecimal(value);
//                }
//            }
//            else
//            {
//                result = new Result(false, "查詢支付寶交易資料失敗，" + result.Message, result.Code, result.Exception);
//            }
//            return result;
//        }
//        #endregion
        #endregion

        /// <summary>
        /// 銷帳處理作業
        /// </summary>
        /// <param name="tw7Date">指定要處理的資料日期 (民國年 yyymmdd，非ATM、網路銀行取入帳日，ATM、網路銀行取代收日)</param>
        /// <param name="log">日誌訊息</param>
        /// <returns>成功傳回 true，否則傳回 false。錯誤訊息由 err_msg 傳回</returns>
        public bool CancelByAccountDate(string tw7Date, ref StringBuilder log)
        {
            _err_msg = "";
            if (log == null)
            {
                log = new StringBuilder();
            }
            bool isOK = false;
            Int32 recordCount = 0;
            Int32 successCount = 0;
            Int32 failureCount = 0;
            Int32 rectifyCount = 0;

            try
            {
                List<string> upctcbReceiveTypes = null;
                if (!this.GetUPCTCBReceiveTypes(out upctcbReceiveTypes))
                {
                    return false;
                }

                CancelDebtsEntity[] cancelDebtss = null;
                if (!this.getCancelDebtsByAccountDate(tw7Date, out cancelDebtss))
                {
                    return false;
                }

                if (cancelDebtss == null || cancelDebtss.Length == 0)
                {
                    _err_msg = String.Format("查無待銷帳的 cancel_debts 資料 (處理的資料日期={0})", tw7Date);
                    return true;
                }
                recordCount = cancelDebtss.Length;

                Result result = null;
                DateTime processTime = DateTime.Now;
                using (EntityFactory factory = new EntityFactory())
                {
                    #region [MDY:20160301] 取得有 6R6 的商家代號
                    decimal z6R6MinAmount = 0;
                    decimal z6R6MaxAmount = 20000;
                    List<string> z6R6ReceiveTypes = null;
                    {
                        ReceiveChannelEntity[] datas = null;
                        Expression where = new Expression(ReceiveChannelEntity.Field.BarcodeId, "6R6");
                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                        orderbys.Add(ReceiveChannelEntity.Field.ReceiveType, OrderByEnum.Asc);
                        result = factory.SelectAll<ReceiveChannelEntity>(where, orderbys, out datas);
                        if (result.IsSuccess)
                        {
                            if (datas != null && datas.Length > 0)
                            {
                                z6R6ReceiveTypes = new List<string>(datas.Length);
                                foreach (ReceiveChannelEntity data in datas)
                                {
                                    z6R6ReceiveTypes.Add(data.ReceiveType);
                                }
                            }
                            else
                            {
                                z6R6ReceiveTypes = new List<string>(0);
                            }
                        }
                        else
                        {
                            _err_msg = string.Format("[CancelByAccountDate] 讀取有 6R6 代收管道的商家代號資料失敗，錯誤訊息={0}", result.Message);
                            return false;
                        }
                    }
                    #endregion

                    bool isCancelOK;
                    bool isDbTimeout;   //紀錄是否發生資料庫 Timeout
                    string problemFlag;
                    string problemDesc;
                    foreach (CancelDebtsEntity cancelDebts in cancelDebtss)
                    {
                        isCancelOK = false;
                        isDbTimeout = false;
                        problemFlag = "";
                        problemDesc = "";

                        #region 處理
                        bool isUPCTCBReceiveType = upctcbReceiveTypes.Contains(cancelDebts.ReceiveType);

                        #region [MDY:20160607] 處理更正交易資料
                        #region 處理邏輯
                        //1. 僅處理臨櫃管道的更正資料
                        //2. 被更正的資料限制以更正資料中的交易日期與時間往前找第一筆相同交易日期、商家代號、虛擬帳號、金額、分行代碼的臨櫃資料
                        //3. 已被更正的資料不能重覆更正
                        //4. 找不被更正的資料僅在日誌檔中記錄，不另外處理
                        //5. 不管更正資料或被更正的資料，都會保留在中心入帳資料表裡 (Cancel_Debts)
                        //5. 被更正的資料如果為問題檔，則將該資料狀態欄位 (Status) 改為被更正，Reserve2 欄位改為被更正，並刪除對應的問題檔
                        //6. 被更正的資料如果為已銷帳，則將該資料狀態欄位 (Status) 改為被更正，Reserve2 欄位改為被更正，並清除學生繳費資料表 (Student_Receive) 銷帳資料相關欄位
                        //7. 更正資料的狀態欄位 (Status) 改為已更正，RollbackDate 設為當下的處理日期時間
                        #endregion

                        if (cancelDebts.Reserve2 == D00I70ECMarkCodeTexts.RECTIFY_CODE)
                        {
                            rectifyCount++;
                            if (ChannelHelper.IsCashChannel(cancelDebts.ReceiveWay))
                            {
                                #region 臨櫃才有更正交易資料
                                CancelDebtsEntity oData = null;     //可被更正資料
                                string errmsg = this.GetCancelDebtsForRectify(factory, cancelDebts, out oData);
                                if (String.IsNullOrEmpty(errmsg))
                                {
                                    if (oData == null)
                                    {
                                        failureCount++;
                                        log.AppendFormat("查無 (商家代號：{0}; 虛擬帳號：{1}; 代收金額：{2}; 代收時間：{3}; 資料來源序號：{4}) 可被更正資料", cancelDebts.ReceiveType, cancelDebts.CancelNo, cancelDebts.ReceiveAmount, cancelDebts.ReceiveTime, cancelDebts.SourceSeq).AppendLine();
                                    }
                                    else if (oData.Reserve2 != D00I70ECMarkCodeTexts.NORMAL_CODE)
                                    {
                                        failureCount++;
                                        log.AppendFormat("查無 (商家代號：{0}; 虛擬帳號：{1}; 代收金額：{2}; 代收時間：{3}; 資料來源序號：{4}) 可被更正資料，錯誤訊息：找到的資料更正記號不正確", cancelDebts.ReceiveType, cancelDebts.CancelNo, cancelDebts.ReceiveAmount, cancelDebts.ReceiveTime, cancelDebts.SourceSeq).AppendLine();
                                    }
                                    else
                                    {
                                        string rectifyLog = null;
                                        errmsg = this.RectifyCancelDebtsData(factory, cancelDebts, oData, processTime, out rectifyLog);
                                        if (String.IsNullOrEmpty(errmsg))
                                        {
                                            successCount++;
                                            log.AppendFormat("處理更正交易 (商家代號：{0}; 虛擬帳號：{1}; 代收金額：{2}; 代收時間：{3}; 資料來源序號：{4}，被更正資料 SNo = {5}) 成功", cancelDebts.ReceiveType, cancelDebts.CancelNo, cancelDebts.ReceiveAmount, cancelDebts.ReceiveTime, cancelDebts.SourceSeq, oData.SNo).AppendLine();
                                        }
                                        else
                                        {
                                            failureCount++;
                                            log.AppendFormat("處理更正交易 (商家代號：{0}; 虛擬帳號：{1}; 代收金額：{2}; 代收時間：{3}; 資料來源序號：{4}，被更正資料 SNo = {5}) 失敗，錯誤訊息={6}", cancelDebts.ReceiveType, cancelDebts.CancelNo, cancelDebts.ReceiveAmount, cancelDebts.ReceiveTime, cancelDebts.SourceSeq, oData.SNo, errmsg).AppendLine();
                                        }
                                        if (!String.IsNullOrEmpty(rectifyLog))
                                        {
                                            log.AppendLine(rectifyLog);
                                        }
                                    }
                                }
                                else
                                {
                                    failureCount++;
                                    log.AppendFormat("查詢可被更正資料失敗，錯誤訊息={0}", errmsg).AppendLine();
                                }
                                #endregion
                            }
                            else
                            {
                                failureCount++;
                                log.AppendFormat("處理更正交易 (商家代號：{0}; 虛擬帳號：{1}; 代收金額：{2}; 代收管道：{3}; 資料來源序號：{4}) 失敗，錯誤訊息：非臨櫃的的更正交易資料", cancelDebts.ReceiveType, cancelDebts.CancelNo, cancelDebts.ReceiveAmount, cancelDebts.ReceiveWay, cancelDebts.SourceSeq).AppendLine();
                            }
                            continue;
                        }
                        #endregion

                        #region [MDY:20170925] 處理正常交易資料，區分支付寶管道與非支付寶管道
                        if (cancelDebts.ReceiveWay == ChannelHelper.ALIPAY)
                        {
                            #region [MDY:20170925] 將支付寶管道的資料獨立出來處理 (因為要從 InboundTxnDtlEntity 取得代收日與手續費)
                            StudentReceiveView6[] datas = null;
                            if (this.GetStudentReceiveView6(factory, cancelDebts.ReceiveType, cancelDebts.CancelNo, out datas))
                            {
                                #region 逐筆檢查支付寶繳費的學生繳費單資料
                                if (datas == null || datas.Length == 0)
                                {
                                    #region 查無資料，視為無此虛擬帳號問題檔 (problem_flag=4)
                                    problemFlag = ProblemFlagCodeTexts.CANCELNO_ERROR;
                                    problemDesc = "無此虛擬帳號（查無該支付寶清算資料）";
                                    failureCount++;
                                    log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, "無此虛擬帳號（已清算的支付寶交易，無此虛擬帳號資料）").AppendLine();
                                    #endregion
                                }
                                else
                                {
                                    #region [MDY:20190410] 改成先找第一筆金額相同的已繳待銷資料
                                    StudentReceiveView6 payedData = null;
                                    {
                                        foreach (StudentReceiveView6 data in datas)
                                        {
                                            if (data.ReceiveWay == cancelDebts.ReceiveWay && data.ReceiveAmount == cancelDebts.ReceiveAmount
                                                && !String.IsNullOrEmpty(data.ReceiveDate) && String.IsNullOrEmpty(data.AccountDate))
                                            {
                                                payedData = data;
                                                break;
                                            }
                                        }
                                        if (payedData != null)
                                        {
                                            #region 找到可銷帳的已繳待銷資料
                                            #region 取得支付寶交易的交易日期時間與手續費
                                            InboundTxnDtlEntity txnData = null;
                                            {
                                                Expression where = new Expression(InboundTxnDtlEntity.Field.Sn, payedData.TxnSN);
                                                result = factory.SelectFirst<InboundTxnDtlEntity>(where, null, out txnData);
                                            }
                                            #endregion

                                            if (result.IsSuccess)
                                            {
                                                if (txnData == null)
                                                {
                                                    #region 無此支付寶交易資料，視為銷帳失敗問題檔 (problem_flag=1)
                                                    problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                    problemDesc = "銷帳失敗（查無該支付寶交易資料）";
                                                    failureCount++;
                                                    log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, String.Format("銷帳失敗 (查無 {0} 支付寶交易資料)", payedData.TxnSN)).AppendLine();
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region 更新學生繳費資料的手續費、入帳日期、銀行代碼、銷帳註記、銷帳日期
                                                    KeyValueList fieldValues = new KeyValueList();
                                                    fieldValues
                                                        .Add(StudentReceiveEntity.Field.ProcessFee, txnData.Fee)                            //支付寶交易資料.手續費
                                                        .Add(StudentReceiveEntity.Field.AccountDate, cancelDebts.AccountDate)
                                                        .Add(StudentReceiveEntity.Field.ReceivebankId, cancelDebts.ReceiveBank)
                                                        .Add(StudentReceiveEntity.Field.CancelFlag, CancelFlagCodeTexts.RECEIVE_SELF)
                                                        .Add(StudentReceiveEntity.Field.CancelDate, processTime.ToString("yyyyMMdd"));

                                                    Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, payedData.ReceiveType)
                                                        .And(StudentReceiveEntity.Field.YearId, payedData.YearId)
                                                        .And(StudentReceiveEntity.Field.TermId, payedData.TermId)
                                                        .And(StudentReceiveEntity.Field.DepId, payedData.DepId)
                                                        .And(StudentReceiveEntity.Field.ReceiveId, payedData.ReceiveId)
                                                        .And(StudentReceiveEntity.Field.StuId, payedData.StuId)
                                                        .And(StudentReceiveEntity.Field.OldSeq, payedData.OldSeq);

                                                    #region 加強更新條件，避免更新到已銷的資料
                                                    {
                                                        where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                                                    }
                                                    #endregion

                                                    int updateCount = 0;
                                                    result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out updateCount);

                                                    if (!result.IsSuccess)
                                                    {
                                                        #region 更新失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                                        #region 判斷是否為資料庫 Timeout，如果是則 isDbTimeout 設為 true
                                                        if (result.Exception != null)
                                                        {
                                                            System.Data.Common.DbException dbException = result.Exception as System.Data.Common.DbException;
                                                            isDbTimeout = (dbException != null && dbException.Message.IndexOf("Timeout", StringComparison.CurrentCultureIgnoreCase) > -1);
                                                        }
                                                        #endregion

                                                        problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                        problemDesc = "銷帳失敗，更新學生繳費資料失敗";
                                                        failureCount++;
                                                        log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, result.Message);
                                                        if (result.Exception != null)
                                                        {
                                                            log.AppendFormat(" ({0})", result.Exception.Message);
                                                        }
                                                        log.AppendLine();
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        isCancelOK = true;
                                                        successCount++;
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                #region 讀取支付寶交易資料失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                                problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                problemDesc = "銷帳失敗（讀取支付寶交易資料失敗）";
                                                failureCount++;
                                                log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, String.Format("銷帳失敗 (讀取 {0} 支付寶交易資料失敗，{1})", payedData.TxnSN, result.Message)).AppendLine();
                                                #endregion
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion

                                    if (payedData == null)
                                    {
                                        #region 沒有已繳待銷資料
                                        #region 找第一筆未銷帳的學生繳費單
                                        StudentReceiveView6 myData = null;
                                        foreach (StudentReceiveView6 data in datas)
                                        {
                                            if (String.IsNullOrEmpty(data.ReceiveWay) && String.IsNullOrEmpty(data.ReceiveDate) && String.IsNullOrEmpty(data.AccountDate))
                                            {
                                                myData = data;
                                                break;
                                            }
                                        }
                                        #endregion

                                        if (myData == null)
                                        {
                                            #region 沒有未銷帳的資料，視為重複繳費問題檔 (problem_flag=5)
                                            problemFlag = ProblemFlagCodeTexts.DUPLICATE_PAYING;
                                            problemDesc = "重複繳費（支付寶清算）";
                                            failureCount++;
                                            log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, "重複繳費（已清算的支付寶交易，此虛擬帳號資料中，無未繳費資料）").AppendLine();
                                            #endregion
                                        }
                                        else
                                        {
                                            if (myData.ReceiveAmount == cancelDebts.ReceiveAmount)
                                            {
                                                #region 繳費金額相符，可銷帳
                                                #region 取得支付寶交易的交易日期時間與手續費
                                                InboundTxnDtlEntity txnData = null;
                                                {
                                                    Expression where = new Expression(InboundTxnDtlEntity.Field.Sn, myData.TxnSN);
                                                    result = factory.SelectFirst<InboundTxnDtlEntity>(where, null, out txnData);
                                                }
                                                #endregion

                                                if (result.IsSuccess)
                                                {
                                                    if (txnData == null)
                                                    {
                                                        #region 無此支付寶交易資料，視為銷帳失敗問題檔 (problem_flag=1)
                                                        problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                        problemDesc = "銷帳失敗（查無此支付寶交易資料）";
                                                        failureCount++;
                                                        log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, String.Format("銷帳失敗 (查無 {0} 支付寶交易資料)", myData.TxnSN)).AppendLine();
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region 更新學生繳費資料的銷帳相關欄位
                                                        KeyValueList fieldValues = new KeyValueList();
                                                        fieldValues
                                                            .Add(StudentReceiveEntity.Field.ReceiveWay, cancelDebts.ReceiveWay)
                                                            .Add(StudentReceiveEntity.Field.ReceiveDate, Common.GetTWDate7(txnData.TxnTime))    //支付寶交易資料.交易日期時間 的日期作為代收日期
                                                            .Add(StudentReceiveEntity.Field.ReceiveTime, txnData.TxnTime.ToString("HHmmss"))    //支付寶交易資料.交易日期時間 的時間作為代收時間
                                                            .Add(StudentReceiveEntity.Field.ProcessFee, txnData.Fee)                            //支付寶交易資料.手續費
                                                            .Add(StudentReceiveEntity.Field.AccountDate, cancelDebts.AccountDate)
                                                            .Add(StudentReceiveEntity.Field.ReceivebankId, cancelDebts.ReceiveBank)
                                                            .Add(StudentReceiveEntity.Field.CancelFlag, CancelFlagCodeTexts.RECEIVE_SELF)
                                                            .Add(StudentReceiveEntity.Field.CancelDate, processTime.ToString("yyyyMMdd"));

                                                        Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, myData.ReceiveType)
                                                            .And(StudentReceiveEntity.Field.YearId, myData.YearId)
                                                            .And(StudentReceiveEntity.Field.TermId, myData.TermId)
                                                            .And(StudentReceiveEntity.Field.DepId, myData.DepId)
                                                            .And(StudentReceiveEntity.Field.ReceiveId, myData.ReceiveId)
                                                            .And(StudentReceiveEntity.Field.StuId, myData.StuId)
                                                            .And(StudentReceiveEntity.Field.OldSeq, myData.OldSeq);

                                                        #region 加強更新條件，避免更新到已銷的資料
                                                        {
                                                            where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                                                        }
                                                        #endregion

                                                        int updateCount = 0;
                                                        result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out updateCount);

                                                        if (!result.IsSuccess)
                                                        {
                                                            #region 更新失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                                            #region [MDY:20160921] 判斷是否為資料庫 Timeout，如果是則 isDbTimeout 設為 true
                                                            if (result.Exception != null)
                                                            {
                                                                System.Data.Common.DbException dbException = result.Exception as System.Data.Common.DbException;
                                                                isDbTimeout = (dbException != null && dbException.Message.IndexOf("Timeout", StringComparison.CurrentCultureIgnoreCase) > -1);
                                                            }
                                                            #endregion

                                                            problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                            problemDesc = "銷帳失敗，更新學生繳費資料失敗";
                                                            failureCount++;
                                                            log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, result.Message);
                                                            if (result.Exception != null)
                                                            {
                                                                log.AppendFormat(" ({0})", result.Exception.Message);
                                                            }
                                                            log.AppendLine();
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            isCancelOK = true;
                                                            successCount++;
                                                        }
                                                        #endregion
                                                    }
                                                }
                                                else
                                                {
                                                    #region 讀取支付寶交易資料失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                                    problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                    problemDesc = "銷帳失敗（讀取支付寶交易資料失敗）";
                                                    failureCount++;
                                                    log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, String.Format("銷帳失敗 (讀取 {0} 支付寶交易資料失敗，{1})", myData.TxnSN, result.Message)).AppendLine();
                                                    #endregion
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region 繳費金額不符，視為金額不符問題檔 (problem_flag=2)
                                                problemFlag = ProblemFlagCodeTexts.AMOUNT_ERROR;
                                                problemDesc = "金額不符（支付寶清算）";
                                                failureCount++;
                                                log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, "金額不符（已清算的支付寶交易，此虛擬帳號且未繳費資料中，無此金額資料）").AppendLine();
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 取得支付寶繳費的學生繳費單資料失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                problemDesc = String.Concat("銷帳失敗（查詢支付寶清算資料失敗），", this.err_msg);
                                //檢查錯誤訊息長度，避免 err_msg 的錯誤訊息太長，無法產生問題檔
                                if (problemDesc.Length > 128)
                                {
                                    problemDesc = problemDesc.Substring(0, 128);
                                }
                                failureCount++;
                                log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, problemDesc).AppendLine();
                                #endregion
                            }
                            #endregion
                        }
                        else if (cancelDebts.ReceiveWay == ChannelHelper.FISC)
                        {
                            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 將財金信用卡管道的資料獨立出來處理，因為中心無法區分國際信用卡
                            StudentReceiveView8[] datas = null;
                            if (this.GetStudentReceiveView8(factory, cancelDebts.ReceiveType, cancelDebts.CancelNo, out datas))
                            {
                                #region 逐筆檢查財金信用卡繳費的學生繳費單資料
                                if (datas == null || datas.Length == 0)
                                {
                                    #region 查無資料，視為無此虛擬帳號問題檔 (problem_flag=4)
                                    problemFlag = ProblemFlagCodeTexts.CANCELNO_ERROR;
                                    problemDesc = "無此虛擬帳號（查無該財金信用卡交易資料）";
                                    failureCount++;
                                    log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, "無此虛擬帳號（財金信用卡交易，無此虛擬帳號資料）").AppendLine();
                                    #endregion
                                }
                                else
                                {
                                    #region 先找第一筆金額相同的已繳待銷資料 (國際信用卡也算財金信用卡管道)
                                    StudentReceiveView8 payedData = null;
                                    {
                                        foreach (StudentReceiveView8 data in datas)
                                        {
                                            if ((data.ReceiveWay == cancelDebts.ReceiveWay || (data.NCCardFlag == "Y" && data.ReceiveWay == ChannelHelper.FISC_NC))
                                                && data.ReceiveAmount == cancelDebts.ReceiveAmount
                                                && !String.IsNullOrEmpty(data.ReceiveDate) && String.IsNullOrEmpty(data.AccountDate))
                                            {
                                                payedData = data;
                                                break;
                                            }
                                        }
                                    }
                                    #endregion

                                    if (payedData != null)
                                    {
                                        #region 找到可銷帳的已繳待銷資料
                                        decimal? handlingFee = null;
                                        if (payedData.NCCardFlag == "Y" || payedData.ReceiveWay == ChannelHelper.FISC_NC)
                                        {
                                            //財金信用卡管道且啟用國際信用卡繳費，一律視為國際信用卡管道，所以要計算手續費
                                            string errmsg = null;
                                            handlingFee = this.GetNCHandlingFee(factory, payedData.ReceiveType, payedData.ReceiveAmount.Value, out errmsg);
                                            if (!handlingFee.HasValue)
                                            {
                                                #region 計算手續費失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                                problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                problemDesc = "銷帳失敗（計算國際信用卡手續費失敗）";
                                                failureCount++;
                                                log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, String.Format("銷帳失敗 ({0})", errmsg)).AppendLine();
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            handlingFee = 0;
                                        }

                                        if (handlingFee.HasValue)  //有值表示成功
                                        {
                                            #region 更新學生繳費資料的手續費、入帳日期、銀行代碼、銷帳註記、銷帳日期
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues
                                                .Add(StudentReceiveEntity.Field.ProcessFee, handlingFee.Value)                            //手續費
                                                .Add(StudentReceiveEntity.Field.AccountDate, cancelDebts.AccountDate)
                                                .Add(StudentReceiveEntity.Field.ReceivebankId, cancelDebts.ReceiveBank)
                                                .Add(StudentReceiveEntity.Field.CancelFlag, CancelFlagCodeTexts.RECEIVE_SELF)
                                                .Add(StudentReceiveEntity.Field.CancelDate, processTime.ToString("yyyyMMdd"));

                                            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, payedData.ReceiveType)
                                                .And(StudentReceiveEntity.Field.YearId, payedData.YearId)
                                                .And(StudentReceiveEntity.Field.TermId, payedData.TermId)
                                                .And(StudentReceiveEntity.Field.DepId, payedData.DepId)
                                                .And(StudentReceiveEntity.Field.ReceiveId, payedData.ReceiveId)
                                                .And(StudentReceiveEntity.Field.StuId, payedData.StuId)
                                                .And(StudentReceiveEntity.Field.OldSeq, payedData.OldSeq);

                                            #region 加強更新條件，避免更新到已銷的資料
                                            {
                                                where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                                            }
                                            #endregion

                                            int updateCount = 0;
                                            result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out updateCount);

                                            if (!result.IsSuccess)
                                            {
                                                #region 更新失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                                #region 判斷是否為資料庫 Timeout，如果是則 isDbTimeout 設為 true
                                                if (result.Exception != null)
                                                {
                                                    System.Data.Common.DbException dbException = result.Exception as System.Data.Common.DbException;
                                                    isDbTimeout = (dbException != null && dbException.Message.IndexOf("Timeout", StringComparison.CurrentCultureIgnoreCase) > -1);
                                                }
                                                #endregion

                                                problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                problemDesc = "銷帳失敗，更新學生繳費資料失敗";
                                                failureCount++;
                                                log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, result.Message);
                                                if (result.Exception != null)
                                                {
                                                    log.AppendFormat(" ({0})", result.Exception.Message);
                                                }
                                                log.AppendLine();
                                                #endregion
                                            }
                                            else
                                            {
                                                isCancelOK = true;
                                                successCount++;
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 沒有已繳待銷資料
                                        #region 找第一筆未銷帳的學生繳費單
                                        StudentReceiveView8 myData = null;
                                        foreach (StudentReceiveView8 data in datas)
                                        {
                                            if (String.IsNullOrEmpty(data.ReceiveWay) && String.IsNullOrEmpty(data.ReceiveDate) && String.IsNullOrEmpty(data.AccountDate))
                                            {
                                                myData = data;
                                                break;
                                            }
                                        }
                                        #endregion

                                        if (myData == null)
                                        {
                                            #region 沒有未銷帳的資料，視為重複繳費問題檔 (problem_flag=5)
                                            problemFlag = ProblemFlagCodeTexts.DUPLICATE_PAYING;
                                            problemDesc = "重複繳費（財金信用卡交易）";
                                            failureCount++;
                                            log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, "重複繳費（財金信用卡交易，此虛擬帳號資料中，無未繳費資料）").AppendLine();
                                            #endregion
                                        }
                                        else
                                        {
                                            if (myData.ReceiveAmount == cancelDebts.ReceiveAmount)
                                            {
                                                #region 繳費金額相符，可銷帳
                                                decimal? handlingFee = null;
                                                if (myData.NCCardFlag == "Y")
                                                {
                                                    //財金信用卡管道且啟用國際信用卡繳費，一律視為國際信用卡管道，所以要計算手續費
                                                    string errmsg = null;
                                                    handlingFee = this.GetNCHandlingFee(factory, myData.ReceiveType, myData.ReceiveAmount.Value, out errmsg);
                                                    if (!handlingFee.HasValue)
                                                    {
                                                        #region 計算手續費失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                                        problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                        problemDesc = "銷帳失敗（計算國際信用卡手續費失敗）";
                                                        failureCount++;
                                                        log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, String.Format("銷帳失敗 ({0})", errmsg)).AppendLine();
                                                        #endregion
                                                    }
                                                }
                                                else
                                                {
                                                    handlingFee = 0;
                                                }

                                                if (handlingFee.HasValue)  //有值表示成功
                                                {
                                                    #region 更新學生繳費資料的銷帳相關欄位
                                                    KeyValueList fieldValues = new KeyValueList();
                                                    fieldValues
                                                        .Add(StudentReceiveEntity.Field.ReceiveWay, (myData.NCCardFlag == "Y" && cancelDebts.ReceiveWay == ChannelHelper.FISC ? ChannelHelper.FISC_NC : cancelDebts.ReceiveWay))
                                                        .Add(StudentReceiveEntity.Field.ReceiveDate, cancelDebts.ReceiveDate)
                                                        .Add(StudentReceiveEntity.Field.ReceiveTime, cancelDebts.ReceiveTime)
                                                        .Add(StudentReceiveEntity.Field.ProcessFee, handlingFee.Value)           //手續費
                                                        .Add(StudentReceiveEntity.Field.AccountDate, cancelDebts.AccountDate)
                                                        .Add(StudentReceiveEntity.Field.ReceivebankId, cancelDebts.ReceiveBank)
                                                        .Add(StudentReceiveEntity.Field.CancelFlag, CancelFlagCodeTexts.RECEIVE_SELF)
                                                        .Add(StudentReceiveEntity.Field.CancelDate, processTime.ToString("yyyyMMdd"));

                                                    Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, myData.ReceiveType)
                                                        .And(StudentReceiveEntity.Field.YearId, myData.YearId)
                                                        .And(StudentReceiveEntity.Field.TermId, myData.TermId)
                                                        .And(StudentReceiveEntity.Field.DepId, myData.DepId)
                                                        .And(StudentReceiveEntity.Field.ReceiveId, myData.ReceiveId)
                                                        .And(StudentReceiveEntity.Field.StuId, myData.StuId)
                                                        .And(StudentReceiveEntity.Field.OldSeq, myData.OldSeq);

                                                    #region 加強更新條件，避免更新到已銷的資料
                                                    {
                                                        where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                                                    }
                                                    #endregion

                                                    int updateCount = 0;
                                                    result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out updateCount);

                                                    if (!result.IsSuccess)
                                                    {
                                                        #region 更新失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                                        #region [MDY:20160921] 判斷是否為資料庫 Timeout，如果是則 isDbTimeout 設為 true
                                                        if (result.Exception != null)
                                                        {
                                                            System.Data.Common.DbException dbException = result.Exception as System.Data.Common.DbException;
                                                            isDbTimeout = (dbException != null && dbException.Message.IndexOf("Timeout", StringComparison.CurrentCultureIgnoreCase) > -1);
                                                        }
                                                        #endregion

                                                        problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                        problemDesc = "銷帳失敗，更新學生繳費資料失敗";
                                                        failureCount++;
                                                        log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, result.Message);
                                                        if (result.Exception != null)
                                                        {
                                                            log.AppendFormat(" ({0})", result.Exception.Message);
                                                        }
                                                        log.AppendLine();
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        isCancelOK = true;
                                                        successCount++;
                                                    }
                                                    #endregion
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region 繳費金額不符，視為金額不符問題檔 (problem_flag=2)
                                                problemFlag = ProblemFlagCodeTexts.AMOUNT_ERROR;
                                                problemDesc = "金額不符（財金信用卡交易）";
                                                failureCount++;
                                                log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, "金額不符（財金信用卡交易，此虛擬帳號且未繳費資料中，無此金額資料）").AppendLine();
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 取得財金信用卡繳費的學生繳費單資料失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                problemDesc = String.Concat("銷帳失敗（查詢財金信用卡交易資料失敗），", this.err_msg);
                                //檢查錯誤訊息長度，避免 err_msg 的錯誤訊息太長，無法產生問題檔
                                if (problemDesc.Length > 128)
                                {
                                    problemDesc = problemDesc.Substring(0, 128);
                                }
                                failureCount++;
                                log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, problemDesc).AppendLine();
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            #region [MDY:20170925] 修正查詢繳費虛擬帳號的學生繳費資料失敗時，誤判為查無此銷帳編號的問題
                            StudentReceiveEntity[] datas = null;
                            if (this.GetStudentReceive2(factory, cancelDebts.ReceiveType, cancelDebts.CancelNo, out datas) > -1)
                            {
                                #region 逐筆檢查學生繳費單資料
                                if (datas == null || datas.Length == 0)
                                {
                                    #region 查無資料，視為無此虛擬帳號問題檔 (problem_flag=4)
                                    problemFlag = ProblemFlagCodeTexts.CANCELNO_ERROR;
                                    failureCount++;
                                    if (isUPCTCBReceiveType)
                                    {
                                        problemDesc = "無此虛擬帳號（免銷帳）";
                                        log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, "無此虛擬帳號，此商家代號為『代收各項費用』種類，免銷帳").AppendLine();
                                    }
                                    else
                                    {
                                        problemDesc = "無此虛擬帳號";
                                        log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, "無此虛擬帳號").AppendLine();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    StudentReceiveEntity myData = null;

                                    #region 如果是有做已繳代銷的管道則先銷該管道的已繳待銷資料 (管道金額都要相同且無入帳日)
                                    bool isPreCancelChannel = ChannelHelper.IsPreCancelChannel(cancelDebts.ReceiveWay);
                                    if (isPreCancelChannel)
                                    {
                                        foreach (StudentReceiveEntity data in datas)
                                        {
                                            if (data.ReceiveWay == cancelDebts.ReceiveWay && data.ReceiveAmount == cancelDebts.ReceiveAmount && String.IsNullOrEmpty(data.AccountDate))
                                            {
                                                myData = data;
                                                break;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 沒有已繳代銷的資料，就找第一筆未銷帳的學生繳費單 (已繳待銷的資料不要，因為應該在上面被處理)
                                    if (myData == null)
                                    {
                                        foreach (StudentReceiveEntity data in datas)
                                        {
                                            if (String.IsNullOrEmpty(data.ReceiveWay) && String.IsNullOrEmpty(data.ReceiveDate) && String.IsNullOrEmpty(data.AccountDate))
                                            {
                                                myData = data;
                                                break;
                                            }
                                        }
                                    }
                                    #endregion

                                    if (myData == null)
                                    {
                                        #region 沒有未銷帳的資料，視為重複繳費問題檔 (problem_flag=5)
                                        problemFlag = ProblemFlagCodeTexts.DUPLICATE_PAYING;
                                        problemDesc = "重複繳費";
                                        failureCount++;
                                        log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, "重複繳費（此虛擬帳號資料中，無未繳費資料）").AppendLine();
                                        #endregion
                                    }
                                    else
                                    {
                                        if (myData.ReceiveAmount == cancelDebts.ReceiveAmount)
                                        {
                                            #region 繳費金額相符，可銷帳 (更新學生繳費資料的銷帳相關欄位)
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues
                                                .Add(StudentReceiveEntity.Field.AccountDate, cancelDebts.AccountDate)
                                                .Add(StudentReceiveEntity.Field.ReceivebankId, cancelDebts.ReceiveBank)
                                                .Add(StudentReceiveEntity.Field.CancelFlag, CancelFlagCodeTexts.RECEIVE_SELF)
                                                .Add(StudentReceiveEntity.Field.CancelDate, processTime.ToString("yyyyMMdd"));

                                            if (!isPreCancelChannel)
                                            {
                                                #region 非做已繳代銷的管道，一律要更新代收管道、代收日期與時間
                                                fieldValues
                                                    .Add(StudentReceiveEntity.Field.ReceiveWay, cancelDebts.ReceiveWay)
                                                    .Add(StudentReceiveEntity.Field.ReceiveDate, cancelDebts.ReceiveDate)
                                                    .Add(StudentReceiveEntity.Field.ReceiveTime, cancelDebts.ReceiveTime);
                                                #endregion
                                            }
                                            else
                                            {
                                                #region 有做已繳代銷的管道
                                                #region 無代收管道時要更新代收管道
                                                if (String.IsNullOrWhiteSpace(myData.ReceiveWay))
                                                {
                                                    fieldValues.Add(StudentReceiveEntity.Field.ReceiveWay, cancelDebts.ReceiveWay);
                                                }
                                                #endregion

                                                #region 無代收日期且為超商 6R6 管道要與更新代收日期時間
                                                if (String.IsNullOrWhiteSpace(myData.ReceiveDate)
                                                    && ChannelHelper.IsSMChannel(cancelDebts.ReceiveWay)
                                                    && cancelDebts.ReceiveAmount >= z6R6MinAmount && cancelDebts.ReceiveAmount <= z6R6MaxAmount
                                                    && z6R6ReceiveTypes != null && z6R6ReceiveTypes.Count > 0 && z6R6ReceiveTypes.Contains(myData.ReceiveType))
                                                {
                                                    fieldValues
                                                        .Add(StudentReceiveEntity.Field.ReceiveDate, cancelDebts.ReceiveDate)
                                                        .Add(StudentReceiveEntity.Field.ReceiveTime, cancelDebts.ReceiveTime);
                                                }
                                                #endregion
                                                #endregion
                                            }

                                            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, myData.ReceiveType)
                                                .And(StudentReceiveEntity.Field.YearId, myData.YearId)
                                                .And(StudentReceiveEntity.Field.TermId, myData.TermId)
                                                .And(StudentReceiveEntity.Field.DepId, myData.DepId)
                                                .And(StudentReceiveEntity.Field.ReceiveId, myData.ReceiveId)
                                                .And(StudentReceiveEntity.Field.StuId, myData.StuId)
                                                .And(StudentReceiveEntity.Field.OldSeq, myData.OldSeq);

                                            #region 加強更新條件，避免更新到已銷的資料
                                            {
                                                where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                                            }
                                            #endregion

                                            int updateCount = 0;
                                            result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out updateCount);

                                            if (!result.IsSuccess)
                                            {
                                                #region 更新失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                                #region [MDY:20160921] 判斷是否為資料庫 Timeout，如果是則 isDbTimeout 設為 true
                                                if (result.Exception != null)
                                                {
                                                    System.Data.Common.DbException dbException = result.Exception as System.Data.Common.DbException;
                                                    isDbTimeout = (dbException != null && dbException.Message.IndexOf("Timeout", StringComparison.CurrentCultureIgnoreCase) > -1);
                                                }
                                                #endregion

                                                problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                                problemDesc = "銷帳失敗，更新學生繳費資料失敗";
                                                failureCount++;
                                                log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, result.Message);
                                                if (result.Exception != null)
                                                {
                                                    log.AppendFormat(" ({0})", result.Exception.Message);
                                                }
                                                log.AppendLine();
                                                #endregion
                                            }
                                            else
                                            {
                                                isCancelOK = true;
                                                successCount++;
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region 繳費金額不符，視為金額不符問題檔 (problem_flag=2)
                                            problemFlag = ProblemFlagCodeTexts.AMOUNT_ERROR;
                                            problemDesc = "金額不符";
                                            failureCount++;
                                            log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, "金額不符（此虛擬帳號且未繳費資料中，無此金額資料）").AppendLine();
                                            #endregion
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 取得學生繳費單資料失敗，視為銷帳失敗問題檔 (problem_flag=1)
                                problemFlag = ProblemFlagCodeTexts.CANCEL_FAIL;
                                problemDesc = String.Concat("銷帳失敗，", this.err_msg);
                                //檢查錯誤訊息長度，避免 err_msg 的錯誤訊息太長，無法產生問題檔
                                if (problemDesc.Length > 128)
                                {
                                    problemDesc = problemDesc.Substring(0, 128);
                                }
                                failureCount++;
                                log.AppendFormat("虛擬帳號{0}、繳費金額{1} 銷帳失敗，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.ReceiveAmount, problemDesc).AppendLine();
                                #endregion
                            }
                            #endregion
                        }
                        #endregion

                        #region [MDY:20160921] 如果發生更新學生繳費資料 Timeout，只寫 log 檔不新增問題檔並將狀態改為待處理
                        if (isDbTimeout)
                        {
                            // log 檔前面已寫了，所以這裡不用再處理

                            #region cancel_debts 狀態改回待處理
                            cancelDebts.Status = CancelDebtsStatusCodeTexts.IS_WAITING_CODE;  //待處理 (1)
                            cancelDebts.ModifyDate = processTime;
                            int updateCount = 0;
                            result = factory.Update((CancelDebtsEntity)cancelDebts, out updateCount);
                            if (!result.IsSuccess)
                            {
                                log.AppendFormat("還原 cancel_debts 為待處理失敗 (虛擬帳號={0}、流水號={1})，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.SNo, result.Message);
                                if (result.Exception != null)
                                {
                                    log.AppendFormat(" ({0})", result.Exception.Message);
                                }
                                log.AppendLine();
                            }
                            #endregion

                            continue;
                        }
                        #endregion

                        #region 銷帳失敗寫問題檔
                        if (isCancelOK == false)
                        {
                            if (isUPCTCBReceiveType && problemFlag == ProblemFlagCodeTexts.CANCELNO_ERROR)
                            {
                                //代收各項費用種類的商家代號且查無此虛擬帳號，免銷帳、不寫問題檔
                            }
                            else
                            {
                                ProblemListEntity problem = new ProblemListEntity();
                                problem.ReceiveType = cancelDebts.ReceiveType;
                                problem.CancelNo = cancelDebts.CancelNo;
                                problem.ProblemFlag = problemFlag;
                                problem.BankId = cancelDebts.ReceiveBank;
                                DateTime receiveDate;
                                if (Common.TryConvertTWDate7(cancelDebts.ReceiveDate, out receiveDate))
                                {
                                    problem.ReceiveDate = receiveDate;
                                }
                                problem.ReceiveTime = cancelDebts.ReceiveTime;
                                DateTime accountDate;
                                if (Common.TryConvertTWDate7(cancelDebts.AccountDate, out accountDate))
                                {
                                    problem.AccountDate = accountDate;
                                }
                                problem.ProcessFee = 0;
                                problem.PayAmount = cancelDebts.ReceiveAmount;
                                problem.ProblemRemark = problemDesc;
                                problem.ReceiveWay = cancelDebts.ReceiveWay;
                                problem.CanceleEBNno = null;
                                problem.UpFlag = null;
                                problem.ReceiveAmount = cancelDebts.ReceiveAmount;
                                problem.CancelReceive = 0;
                                problem.FeeReceivable = 0;
                                problem.FeePayable = 0;
                                problem.PFlag = null;
                                problem.CreateDate = DateTime.Now;
                                problem.CreateMan = "SYSTEM";
                                problem.UpdateDate = null;
                                problem.UpdateMan = null;
                                int insertCount = 0;
                                result = factory.Insert(problem, out insertCount);
                                if (!result.IsSuccess)
                                {
                                    log.AppendFormat("新增 problem_list 失敗 (虛擬帳號={0}、代收金額={1})，錯誤訊息={2}", problem.CancelNo, problem.PayAmount, result.Message);
                                }
                            }
                        }
                        #endregion

                        #region 更新 cancel_debts 狀態
                        {
                            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 暫時不考慮把 CancelDebtsEntity.ReceiveWay 改為國際信用卡
                            //CancelDebtsEntity.ReceiveWay = ChannelHelper.FISC_NC
                            #endregion

                            if (isCancelOK)
                            {
                                cancelDebts.Status = CancelDebtsStatusCodeTexts.HAS_CANCELED_CODE;     //已銷帳
                            }
                            else
                            {
                                if (isUPCTCBReceiveType && problemFlag == ProblemFlagCodeTexts.CANCELNO_ERROR)
                                {
                                    //代收各項費用種類的商家代號且查無此虛擬帳號，免銷帳
                                    cancelDebts.Status = CancelDebtsStatusCodeTexts.IS_EXCLUDED_CODE;  //免銷帳
                                }
                                else
                                {
                                    cancelDebts.Status = CancelDebtsStatusCodeTexts.IS_FAILURE_CODE;   //銷帳失敗
                                }
                            }
                            cancelDebts.ModifyDate = processTime;
                            cancelDebts.CancelDate = processTime;
                            int updateCount = 0;
                            result = factory.Update((CancelDebtsEntity)cancelDebts, out updateCount);
                            if (!result.IsSuccess)
                            {
                                log.AppendFormat("更新 cancel_debts 失敗 (虛擬帳號={0}、流水號={1})，錯誤訊息={2}", cancelDebts.CancelNo, cancelDebts.SNo, result.Message);
                                if (result.Exception != null)
                                {
                                    log.AppendFormat(" ({0})", result.Exception.Message);
                                }
                                log.AppendLine();
                            }
                        }
                        #endregion
                        #endregion
                    }

                    log
                        .AppendLine("銷帳處理結果：")
                        .AppendFormat("共 {0} 筆資料，成功 {1} 筆，失敗 {2} 筆，更正資料 {3} 筆", recordCount, successCount, failureCount, rectifyCount).AppendLine();
                    isOK = true;
                }
            }
            catch (Exception ex)
            {
                isOK = false;
                _err_msg = string.Format("[CancelByAccountDate] 銷帳處理作業發生錯誤，處理的資料日期={0}，錯誤訊息={1}", tw7Date, ex.Message);
            }

            return isOK;
        }
        #endregion


        #region [Old]
        //public bool insertProblem()
        //{
        //    bool rc = false;
        //    ProblemListEntity problem_list = null;

            
        //    return rc;

        //}
        #endregion

        #region 產生每日銷帳結果資料 相關
        #region [Old]
//        /// <summary>
//        /// 產生每日銷帳結果資料
//        /// </summary>
//        /// <param name="accountDate">指定入帳日</param>
//        /// <param name="receiveType">指定商家代號，不指定則計算所有商家代號</param>
//        /// <returns>成功則傳回 true，否則傳回 false (任一個商家代號的資料計算成功就算成功)</returns>
//        public bool GenCancelResultData(DateTime accountDate, string receiveType)
//        {
//            #region 檢查參數
//            if (receiveType != null)
//            {
//                receiveType = receiveType.Trim();
//            }
//            string qAccountDate = Common.GetTWDate7(accountDate);
//            #endregion

//            #region 邏輯
//            //如果有指定代收類別則只計算該代收類別，否則取目前有效的所有代收類別，逐筆計算
//            //台企銀的手續費內含表示企業負擔，外加表示學生負擔，所以不用區分內含外加，直接計算企業負擔
//            //超商：台企銀的手續費內含表示企業負擔，外加表示學生負擔，只影響外加企業負擔一定是 0，內含則計算企業負擔計算 (co_fee) 的部份，所以也不用分內含外加
//            //所有代收類別都計算失敗才算失敗
//            #endregion

//            try
//            {
//                bool isOK = false;
//                using (EntityFactory factory = new EntityFactory())
//                {
//                    #region 取要處理的代收類別
//                    List<string> qReceiveTyps = new List<string>();
//                    if (String.IsNullOrEmpty(receiveType))
//                    {
//                        SchoolRTypeEntity[] datas = null;
//                        Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
//                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
//                        orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
//                        Result result = factory.SelectAll<SchoolRTypeEntity>(where, orderbys, out datas);
//                        if (result.IsSuccess)
//                        {
//                            if (datas == null || datas.Length == 0)
//                            {
//                                this.err_msg = "無任何商家代號資料";
//                                return false;
//                            }
//                            else
//                            {
//                                foreach (SchoolRTypeEntity data in datas)
//                                {
//                                    qReceiveTyps.Add(data.ReceiveType);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            this.err_msg = "讀取商家代號資料失敗：" + result.Message;
//                            return false;
//                        }
//                    }
//                    else
//                    {
//                        int count = 0;
//                        Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL)
//                            .And(SchoolRTypeEntity.Field.ReceiveType, receiveType);
//                        Result result = factory.SelectCount<SchoolRTypeEntity>(where, out count);
//                        if (result.IsSuccess)
//                        {
//                            if (count == 0)
//                            {
//                                this.err_msg = String.Format("查無指定的 {0} 商家代號資料或該商家代號已停用", receiveType);
//                                return false;
//                            }
//                            else
//                            {
//                                qReceiveTyps.Add(receiveType);
//                            }
//                        }
//                        else
//                        {
//                            this.err_msg = "讀取商家代號資料失敗：" + result.Message;
//                            return false;
//                        }
//                    }
//                    #endregion

//                    #region SQL Pattern
//                    string sqlPattern = @"
//DECLARE @QAccountDate char(7);
//DECLARE @QReceiveType varchar(6);
//SET @QAccountDate = '{0}';
//SET @QReceiveType = '{1}';
//
//DECLARE @ReceiveType varchar(6), @YearId char(3), @TermId char(1), @DepId char(1), @ReceiveId varchar(2);
//DECLARE @YearName nvarchar(8), @TermName nvarchar(20), @DepName nvarchar(20), @ReceiveName nvarchar(40);
//
//DECLARE @SOCount int,  @SOAmount decimal(18,0)  -- SO 管道
//DECLARE @YCount int,   @YAmount decimal(18,0)   -- Y 管道
//DECLARE @PCount int,   @PAmount decimal(18,0), @PostFee decimal(18,0) -- P 管道 (Post)
//DECLARE @GCount int,   @GAmount decimal(18,0)   -- G 商管道
//DECLARE @DCount int,   @DAmount decimal(18,0)   -- D 商管道
//DECLARE @NCount int,   @NAmount decimal(18,0)   -- N 管道
//DECLARE @JCount int,   @JAmount decimal(18,0)   -- J 管道
//DECLARE @BCount int,   @BAmount decimal(18,0)   -- B 管道
//DECLARE @KCount int,   @KAmount decimal(18,0)   -- K 管道
//DECLARE @WCount int,   @WAmount decimal(18,0)   -- W 管道
//DECLARE @ACount int,   @AAmount decimal(18,0)   -- A 管道
//DECLARE @ICount int,   @IAmount decimal(18,0)   -- I 管道
//DECLARE @CMFCount int, @CMFAmount decimal(18,0) -- CMF 管道
//DECLARE @HCount int,   @HAmount decimal(18,0)   -- H 管道
//DECLARE @MarketCount int, @MarketFee decimal(18,0) -- Market 小計
//DECLARE @SubCount int, @SubAmount decimal(18,0) -- Sub 小計
//
//DECLARE @DataCount int
//SET @DataCount = 0;
//
//DECLARE myCursor CURSOR FOR
// SELECT RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id
//      , YL.Year_Name, TL.Term_Name, /*DL.Dep_Name*/ '' AS Dep_Name, RL.Receive_Name
//   FROM Receive_List AS RL
//   JOIN Year_List    AS YL ON YL.Year_Id = RL.Year_Id
//   JOIN Term_List    AS TL ON TL.status = '0' AND TL.Receive_Type = RL.Receive_Type AND TL.Year_Id = RL.Year_Id AND TL.Term_Id = RL.Term_Id 
//   --JOIN Dep_List     AS DL ON DL.status = '0' AND DL.Receive_Type = RL.Receive_Type AND DL.Year_Id = RL.Year_Id AND DL.Term_Id = RL.Term_Id AND DL.Dep_Id = RL.Dep_Id
//  WHERE RL.status = '0' AND RL.Receive_Type = @QReceiveType
//  ORDER BY RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id;
//
//OPEN myCursor;
//FETCH NEXT FROM myCursor INTO @ReceiveType, @YearId, @TermId, @DepId, @ReceiveId, @YearName, @TermName, @DepName, @ReceiveName;
//WHILE @@FETCH_STATUS = 0
//BEGIN
//    -- SO 管道 (收款單位自收)
//    SELECT @SOCount = COUNT(1), @SOAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('S');
//
//    -- Y 管道 (EDI)
//    SELECT @YCount = COUNT(1), @YAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('04');
//
//    -- P 管道 (郵局)
//    SELECT @PCount = 0, @PAmount = 0, @PostFee = 0;
//    /*
//    SELECT @PCount = COUNT(1), @PAmount = ISNULL(SUM(Receive_Amount), 0), @PostFee = 0
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('P');
//    */
//
//    -- G 管道 (統一超商)
//    SELECT @GCount = COUNT(1), @GAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('81');
//    -- D 管道 (全家超商)
//    SELECT @DCount = COUNT(1), @DAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('82');
//    -- N 管道 (萊爾富超商)
//    SELECT @NCount = COUNT(1), @NAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('85');
//    -- J 管道 (OK 超商)
//    SELECT @JCount = COUNT(1), @JAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('83');
//
//    -- B 管道 (語音銀行)
//    SELECT @BCount = COUNT(1), @BAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('06');
//
//    -- K 管道 (網路信用卡(財金))
//    SELECT @KCount = COUNT(1), @KAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('87');
//    -- W 管道 (信用卡(中國信託))
//    SELECT @WCount = COUNT(1), @WAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('80');
//
//    -- A 管道 (ATM)
//    SELECT @ACount = COUNT(1), @AAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('03');
//    -- I 管道 (網路銀行)
//    SELECT @ICount = COUNT(1), @IAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('05');
//    -- CMF 管道 (臨櫃)
//    SELECT @CMFCount = COUNT(1), @CMFAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('01');
//    -- H 管道 (匯款)
//    SELECT @HCount = COUNT(1), @HAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('02');
//
//    SELECT @MarketCount = 0, @MarketFee = 0;
//
//    -- Market 小計
//    SELECT @MarketCount = COUNT(1), @MarketFee = ISNULL(SUM(Fee), 0)
//      FROM (SELECT ISNULL((SELECT TOP 1 RC_Charge FROM Receive_Channel WHERE RC_type = SR.Receive_Type AND RC_Channel = 'HILI'
//                       AND (CASE WHEN MoneyLimit = 0 THEN 999999999 ELSE MoneyLimit END) >= SR.Receive_Amount
//                       AND SR.Receive_Amount >= MoneyLowerLimit), 0) AS Fee
//              FROM Student_Receive AS SR
//             WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//              AND Receive_Way IN ('81', '82', '85', '83')
//           ) AS Market;
//
//    SET @SubCount = @PCount + @GCount + @DCount + @NCount + @JCount + @BCount + @KCount + @WCount + @ACount + @ICount + @CMFCount + @HCount;
//    SET @SubAmount = @PAmount + @GAmount + @DAmount + @NAmount + @JAmount + @BAmount + @KAmount + @WAmount + @AAmount + @IAmount + @CMFAmount + @HAmount - @PostFee - @MarketFee;
//
//    -- 刪除舊資料
//    DELETE cancel_result WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId;
//
//    -- 新增新資料
//    INSERT INTO cancel_result (Account_Date, Receive_Type, Year_Id, Year_Name, Term_Id, Term_Name, Dep_Id, Dep_Name, Receive_Id, Receive_Name
//         , SO_Count, SO_Amount, Y_Count, Y_Amount, P_Count, P_Amount, G_Count, G_Amount, D_Count, D_Amount, N_Count, N_Amount, J_Count, J_Amount
//         , B_Count, B_Amount, K_Count, K_Amount, W_Count, W_Amount, A_Count, A_Amount, I_Count, I_Amount, CMF_Count, CMF_Amount, H_Count, H_Amount
//         , Market_Count, Market_Fee, Sub_Count, Sub_Amount, Total_Count, Total_Amount, Create_Date, Post_Fee)
//    SELECT @QAccountDate AS Account_Date, @ReceiveType AS Receive_Type, @YearId AS Year_Id, @YearName AS Year_Name
//         , @TermId AS Term_Id, @TermName AS Term_Name, @DepId AS Dep_Id, @DepName AS Dep_Name
//         , @ReceiveId AS Receive_Id, @ReceiveName AS Receive_Name
//         , @SOCount AS SO_Count, @SOAmount AS SO_Amount
//         , @YCount AS Y_Count, @YAmount AS Y_Amount
//         , @PCount AS P_Count, @PAmount AS P_Amount
//         , @GCount AS G_Count, @GAmount AS G_Amount
//         , @DCount AS D_Count, @DAmount AS D_Amount
//         , @NCount AS N_Count, @NAmount AS N_Amount
//         , @JCount AS J_Count, @JAmount AS J_Amount
//         , @BCount AS B_Count, @BAmount AS B_Amount
//         , @KCount AS K_Count, @KAmount AS K_Amount
//         , @WCount AS W_Count, @WAmount AS W_Amount
//         , @ACount AS A_Count, @AAmount AS A_Amount
//         , @ICount AS I_Count, @IAmount AS I_Amount
//         , @CMFCount AS CMF_Count, @CMFAmount AS CMF_Amount
//         , @HCount AS H_count, @HAmount AS H_amount
//         , @MarketCount AS Market_Count, @MarketFee AS Market_Fee
//         , @SubCount AS Sub_Count, @SubAmount AS Sub_Amount
//         , @SubCount + @SOCount + @YCount AS Total_Count, @SubAmount + @SOAmount + @YAmount AS Total_Amount
//         , GETDATE() AS Create_Date
//         , @PostFee AS post_fee;
//
//    IF @@ERROR = 0
//        SET @DataCount = @DataCount + 1;
//
//    FETCH NEXT FROM myCursor INTO @ReceiveType, @YearId, @TermId, @DepId, @ReceiveId, @YearName, @TermName, @DepName, @ReceiveName;
//END
//
//CLOSE myCursor;
//DEALLOCATE myCursor; 
//
//SELECT @DataCount
//";
//                    #endregion

//                    StringBuilder log = new StringBuilder();
//                    int totalCount = 0;
//                    int failCount = 0;

//                    #region 逐代收類別執行 SQL
//                    foreach (string qReceiveTyp in qReceiveTyps)
//                    {
//                        string sql = String.Format(sqlPattern, qAccountDate, qReceiveTyp);
//                        object dataCount = null;
//                        Result result = factory.ExecuteScalar(sql, (KeyValue[]) null, out dataCount);
//                        if (result.IsSuccess)
//                        {
//                            if (dataCount is int)
//                            {
//                                totalCount += (int)dataCount;
//                            }
//                        }
//                        else
//                        {
//                            log.AppendFormat("商家代號 {0} 的資料計算失敗，錯誤訊息：{1}", qReceiveTyp, result.Message).AppendLine();
//                            failCount++;
//                        }
//                    }
//                    #endregion

//                    isOK = (qReceiveTyps.Count > failCount);
//                    if (failCount == qReceiveTyps.Count)
//                    {
//                        this.err_msg = "所有商家代號的資料計算皆失敗，\r\n" + log.ToString();
//                    }
//                    else if (failCount > 0)
//                    {
//                        this.err_msg = "部份商家代號的資料計算失敗，\r\n" + log.ToString();
//                    }
//                    else
//                    {
//                        this.err_msg = "所有商家代號的資料計算皆成功，\r\n" + log.ToString();
//                    }
//                }
//                return isOK;
//            }
//            catch (Exception ex)
//            {
//                this.err_msg = "產生每日銷帳結果處理發生例外：" + ex.Message;
//                return false;
//            }
//        }
        #endregion

        /// <summary>
        /// 取得所有有效的商家代號或檢查指定商家代號是否存在且有效
        /// </summary>
        /// <param name="factory">資料存取物件</param>
        /// <param name="receiveType">指定檢查的商家代號</param>
        /// <param name="receiveTyps">傳回有效的商家代號</param>
        /// <returns>傳回處理結果</returns>
        private Result GetOrCheckNormalReceiveTypes(EntityFactory factory, string receiveType, out List<string> receiveTyps)
        {
            receiveTyps = null;

            Result result = null;

            Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
            if (String.IsNullOrWhiteSpace(receiveType))
            {
                #region 取所有有效的商家代號
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);

                SchoolRTypeEntity[] datas = null;
                result = factory.SelectAll<SchoolRTypeEntity>(where, orderbys, out datas);
                if (result.IsSuccess)
                {
                    if (datas != null && datas.Length > 0)
                    {
                        receiveTyps = new List<string>(datas.Length);
                        foreach (SchoolRTypeEntity data in datas)
                        {
                            receiveTyps.Add(data.ReceiveType);
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 檢查指定商家代號是否存在且有效
                receiveType = receiveType.Trim();
                where.And(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                int count = 0;
                result = factory.SelectCount<SchoolRTypeEntity>(where, out count);
                if (result.IsSuccess)
                {
                    if (count > 0)
                    {
                        receiveTyps = new List<string>(1);
                        receiveTyps.Add(receiveType);
                    }
                }
                #endregion
            }
            return result;
        }

        /// <summary>
        /// 產生每日銷帳結果資料
        /// </summary>
        /// <param name="accountDate">指定計算的入帳日</param>
        /// <param name="receiveType">指定計算的商家代號，不指定則計算所有有效的商家代號</param>
        /// <param name="retryTimes">指定失敗重試次數</param>
        /// <param name="retrySleep">指定失敗重試間隔(單位分鐘)</param>
        /// <param name="logmsg">傳回處理日誌</param>
        /// <returns>傳回錯誤訊息</returns>
        public string GenCancelResultData(DateTime accountDate, string receiveType, int retryTimes, int retrySleep, out string logmsg)
        {
            #region 檢查參數
            string qAccountDate = Common.GetTWDate7(accountDate);
            if (receiveType != null)
            {
                receiveType = receiveType.Trim();
            }
            if (retrySleep < 0)
            {
                retrySleep = 0;
            }
            #endregion

            #region 邏輯
            //如果有指定商家代號則只計算該商家代號，否則取目前所有有效的商家代號，逐一計算
            //土銀的手續費內含表示企業負擔，外加表示學生負擔，所以不用區分內含外加，直接計算企業負擔
            //超商：土銀的手續費內含表示企業負擔，外加表示學生負擔，只影響外加企業負擔一定是 0，內含則計算企業負擔計算 (co_fee) 的部份，所以也不用分內含外加
            //所有商家代號都計算失敗才算失敗
            #endregion

            string errmsg = null;
            StringBuilder log = new StringBuilder();
            try
            {
                int hasRetryTimes = 0;
                using (EntityFactory factory = new EntityFactory())
                {
                    #region 取要處理的商家代號
                    List<string> qReceiveTyps = null;
                    {
                        Result result = this.GetOrCheckNormalReceiveTypes(factory, receiveType, out qReceiveTyps);
                        if (!result.IsSuccess)
                        {
                            log.AppendFormat("第 {0} 次讀取商家代號資料失敗，錯誤訊息：{1}", hasRetryTimes, result.Message).AppendLine();
                            hasRetryTimes++;
                            for (; hasRetryTimes <= retryTimes; hasRetryTimes++)
                            {
                                if (retrySleep > 0)
                                {
                                    System.Threading.Thread.Sleep(1000 * 60 * retrySleep);
                                }
                                result = this.GetOrCheckNormalReceiveTypes(factory, receiveType, out qReceiveTyps);
                                if (result.IsSuccess)
                                {
                                    break;
                                }
                                else
                                {
                                    log.AppendFormat("第 {0} 次讀取商家代號資料失敗，錯誤訊息：{1}", hasRetryTimes, result.Message).AppendLine();
                                }
                            }
                        }
                        if (result.IsSuccess)
                        {
                            if (qReceiveTyps == null || qReceiveTyps.Count == 0)
                            {
                                if (String.IsNullOrEmpty(receiveType))
                                {
                                    errmsg = String.Format("查無指定的 {0} 商家代號資料或該商家代號已停用", receiveType);
                                }
                                else
                                {
                                    errmsg = "無任何有效的商家代號資料";
                                }
                            }
                            else
                            {
                                log.AppendFormat("讀取商家代號資料成功，共 {0} 筆資料", qReceiveTyps.Count).AppendLine();
                            }
                        }
                        else
                        {
                            errmsg = String.Format("讀取有效的商家代號資料失敗，錯誤訊息：{0}", result.Message);
                        }
                    }
                    #endregion

                    #region 逐商家代號產生銷帳結果資料
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        #region SQL Pattern
                        #region [Old]
//                        #region [MDY:20171127] 增加08-全國繳費網、10-台灣Pay (20170831_01)
//                        #region [Old]
//                        //                        #region [MDY:20170506] 增加 C09 管道 (支付寶)
////                        #region [Old]
////                        //                        string sqlPattern = @"
//////DECLARE @QAccountDate char(7);
//////DECLARE @QReceiveType varchar(6);
//////SET @QAccountDate = '{0}';
//////SET @QReceiveType = '{1}';
//////
//////DECLARE @ReceiveType varchar(6), @YearId char(3), @TermId char(1), @DepId char(1), @ReceiveId varchar(2);
//////DECLARE @YearName nvarchar(8), @TermName nvarchar(20), @DepName nvarchar(20), @ReceiveName nvarchar(40);
//////
//////DECLARE @SOCount int,  @SOAmount decimal(18,0)  -- SO 管道 (收款單位自收)
//////DECLARE @YCount int,   @YAmount decimal(18,0)   -- Y 管道 (EDI)
//////DECLARE @PCount int,   @PAmount decimal(18,0), @PostFee decimal(18,0) -- P 管道 (郵局)
//////DECLARE @GCount int,   @GAmount decimal(18,0)   -- G 商管道 (統一超商)
//////DECLARE @DCount int,   @DAmount decimal(18,0)   -- D 商管道 (全家超商)
//////DECLARE @NCount int,   @NAmount decimal(18,0)   -- N 管道 (萊爾富超商)
//////DECLARE @JCount int,   @JAmount decimal(18,0)   -- J 管道 (OK 超商)
//////DECLARE @BCount int,   @BAmount decimal(18,0)   -- B 管道 (語音銀行)
//////DECLARE @KCount int,   @KAmount decimal(18,0)   -- K 管道 (網路信用卡(財金))
//////DECLARE @WCount int,   @WAmount decimal(18,0)   -- W 管道 (信用卡(中國信託))
//////DECLARE @ACount int,   @AAmount decimal(18,0)   -- A 管道 (ATM)
//////DECLARE @ICount int,   @IAmount decimal(18,0)   -- I 管道 (網路銀行)
//////DECLARE @CMFCount int, @CMFAmount decimal(18,0) -- CMF 管道 (臨櫃)
//////DECLARE @HCount int,   @HAmount decimal(18,0)   -- H 管道 (匯款)
//////DECLARE @MarketCount int, @MarketFee decimal(18,0) -- Market 小計
//////DECLARE @SubCount int, @SubAmount decimal(18,0) -- Sub 小計
//////
//////DECLARE @DataCount int
//////SET @DataCount = 0;
//////
//////DECLARE myCursor CURSOR FOR
////// SELECT RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id
//////      , YL.Year_Name, TL.Term_Name, /*DL.Dep_Name*/ '' AS Dep_Name, RL.Receive_Name
//////   FROM Receive_List AS RL
//////   JOIN Year_List    AS YL ON YL.Year_Id = RL.Year_Id
//////   JOIN Term_List    AS TL ON TL.status = '0' AND TL.Receive_Type = RL.Receive_Type AND TL.Year_Id = RL.Year_Id AND TL.Term_Id = RL.Term_Id 
//////   --JOIN Dep_List     AS DL ON DL.status = '0' AND DL.Receive_Type = RL.Receive_Type AND DL.Year_Id = RL.Year_Id AND DL.Term_Id = RL.Term_Id AND DL.Dep_Id = RL.Dep_Id
//////  WHERE RL.status = '0' AND RL.Receive_Type = @QReceiveType
//////  ORDER BY RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id;
//////
//////OPEN myCursor;
//////FETCH NEXT FROM myCursor INTO @ReceiveType, @YearId, @TermId, @DepId, @ReceiveId, @YearName, @TermName, @DepName, @ReceiveName;
//////WHILE @@FETCH_STATUS = 0
//////BEGIN
//////    -- SO 管道 (收款單位自收)
//////    SELECT @SOCount = COUNT(1), @SOAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('S');
//////
//////    -- Y 管道 (EDI)
//////    SELECT @YCount = COUNT(1), @YAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('04');
//////
//////    -- P 管道 (郵局)
//////    SELECT @PCount = 0, @PAmount = 0, @PostFee = 0;
//////    /*
//////    SELECT @PCount = COUNT(1), @PAmount = ISNULL(SUM(Receive_Amount), 0), @PostFee = 0
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('86');
//////    */
//////
//////    -- G 管道 (統一超商)
//////    SELECT @GCount = COUNT(1), @GAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('81');
//////    -- D 管道 (全家超商)
//////    SELECT @DCount = COUNT(1), @DAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('82');
//////    -- N 管道 (萊爾富超商)
//////    SELECT @NCount = COUNT(1), @NAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('85');
//////    -- J 管道 (OK 超商)
//////    SELECT @JCount = COUNT(1), @JAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('83');
//////
//////    -- B 管道 (語音銀行)
//////    SELECT @BCount = COUNT(1), @BAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('06');
//////
//////    -- K 管道 (網路信用卡(財金))
//////    SELECT @KCount = COUNT(1), @KAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('87');
//////    -- W 管道 (信用卡(中國信託))
//////    SELECT @WCount = COUNT(1), @WAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('80');
//////
//////    -- A 管道 (ATM)
//////    SELECT @ACount = COUNT(1), @AAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('03');
//////    -- I 管道 (網路銀行)
//////    SELECT @ICount = COUNT(1), @IAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('05');
//////    -- CMF 管道 (臨櫃)
//////    SELECT @CMFCount = COUNT(1), @CMFAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('01');
//////    -- H 管道 (匯款)
//////    SELECT @HCount = COUNT(1), @HAmount = ISNULL(SUM(Receive_Amount), 0)
//////      FROM Student_Receive
//////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////       AND Receive_Way IN ('02');
//////
//////    SELECT @MarketCount = 0, @MarketFee = 0;
//////
//////    -- Market 小計
//////    SELECT @MarketCount = COUNT(1), @MarketFee = ISNULL(SUM(Fee), 0)
//////      FROM (SELECT ISNULL((SELECT TOP 1 RC_Charge FROM Receive_Channel WHERE RC_type = SR.Receive_Type AND RC_Channel = 'HILI'
//////                       AND (CASE WHEN MoneyLimit = 0 THEN 999999999 ELSE MoneyLimit END) >= SR.Receive_Amount
//////                       AND SR.Receive_Amount >= MoneyLowerLimit), 0) AS Fee
//////              FROM Student_Receive AS SR
//////             WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//////              AND Receive_Way IN ('81', '82', '85', '83')
//////           ) AS Market;
//////
//////    SET @SubCount = @PCount + @GCount + @DCount + @NCount + @JCount + @BCount + @KCount + @WCount + @ACount + @ICount + @CMFCount + @HCount;
//////    SET @SubAmount = @PAmount + @GAmount + @DAmount + @NAmount + @JAmount + @BAmount + @KAmount + @WAmount + @AAmount + @IAmount + @CMFAmount + @HAmount - @PostFee - @MarketFee;
//////
//////    -- 刪除舊資料
//////    DELETE cancel_result WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId;
//////
//////    -- 新增新資料
//////    INSERT INTO cancel_result (Account_Date, Receive_Type, Year_Id, Year_Name, Term_Id, Term_Name, Dep_Id, Dep_Name, Receive_Id, Receive_Name
//////         , SO_Count, SO_Amount, Y_Count, Y_Amount, P_Count, P_Amount, G_Count, G_Amount, D_Count, D_Amount, N_Count, N_Amount, J_Count, J_Amount
//////         , B_Count, B_Amount, K_Count, K_Amount, W_Count, W_Amount, A_Count, A_Amount, I_Count, I_Amount, CMF_Count, CMF_Amount, H_Count, H_Amount
//////         , Market_Count, Market_Fee, Sub_Count, Sub_Amount, Total_Count, Total_Amount, Create_Date, Post_Fee)
//////    SELECT @QAccountDate AS Account_Date, @ReceiveType AS Receive_Type, @YearId AS Year_Id, @YearName AS Year_Name
//////         , @TermId AS Term_Id, @TermName AS Term_Name, @DepId AS Dep_Id, @DepName AS Dep_Name
//////         , @ReceiveId AS Receive_Id, @ReceiveName AS Receive_Name
//////         , @SOCount AS SO_Count, @SOAmount AS SO_Amount
//////         , @YCount AS Y_Count, @YAmount AS Y_Amount
//////         , @PCount AS P_Count, @PAmount AS P_Amount
//////         , @GCount AS G_Count, @GAmount AS G_Amount
//////         , @DCount AS D_Count, @DAmount AS D_Amount
//////         , @NCount AS N_Count, @NAmount AS N_Amount
//////         , @JCount AS J_Count, @JAmount AS J_Amount
//////         , @BCount AS B_Count, @BAmount AS B_Amount
//////         , @KCount AS K_Count, @KAmount AS K_Amount
//////         , @WCount AS W_Count, @WAmount AS W_Amount
//////         , @ACount AS A_Count, @AAmount AS A_Amount
//////         , @ICount AS I_Count, @IAmount AS I_Amount
//////         , @CMFCount AS CMF_Count, @CMFAmount AS CMF_Amount
//////         , @HCount AS H_count, @HAmount AS H_amount
//////         , @MarketCount AS Market_Count, @MarketFee AS Market_Fee
//////         , @SubCount AS Sub_Count, @SubAmount AS Sub_Amount
//////         , @SubCount + @SOCount + @YCount AS Total_Count, @SubAmount + @SOAmount + @YAmount AS Total_Amount
//////         , GETDATE() AS Create_Date
//////         , @PostFee AS post_fee;
//////
//////    IF @@ERROR = 0
//////        SET @DataCount = @DataCount + 1;
//////
//////    FETCH NEXT FROM myCursor INTO @ReceiveType, @YearId, @TermId, @DepId, @ReceiveId, @YearName, @TermName, @DepName, @ReceiveName;
//////END
//////
//////CLOSE myCursor;
//////DEALLOCATE myCursor;
//////
//////SELECT @DataCount
//////";
////                        #endregion

////                        string sqlPattern = @"
////DECLARE @QAccountDate char(7);
////DECLARE @QReceiveType varchar(6);
////SET @QAccountDate = '{0}';
////SET @QReceiveType = '{1}';
////
////DECLARE @ReceiveType varchar(6), @YearId char(3), @TermId char(1), @DepId char(1), @ReceiveId varchar(2);
////DECLARE @YearName nvarchar(8), @TermName nvarchar(20), @DepName nvarchar(20), @ReceiveName nvarchar(40);
////
////DECLARE @SOCount int,  @SOAmount decimal(18,0)  -- SO 管道 (收款單位自收)
////DECLARE @YCount int,   @YAmount decimal(18,0)   -- Y 管道 (EDI)
////DECLARE @PCount int,   @PAmount decimal(18,0), @PostFee decimal(18,0) -- P 管道 (郵局)
////DECLARE @GCount int,   @GAmount decimal(18,0)   -- G 管道 (統一超商)
////DECLARE @DCount int,   @DAmount decimal(18,0)   -- D 管道 (全家超商)
////DECLARE @NCount int,   @NAmount decimal(18,0)   -- N 管道 (萊爾富超商)
////DECLARE @JCount int,   @JAmount decimal(18,0)   -- J 管道 (OK 超商)
////DECLARE @BCount int,   @BAmount decimal(18,0)   -- B 管道 (語音銀行)
////DECLARE @KCount int,   @KAmount decimal(18,0)   -- K 管道 (網路信用卡(財金))
////DECLARE @WCount int,   @WAmount decimal(18,0)   -- W 管道 (信用卡(中國信託))
////DECLARE @ACount int,   @AAmount decimal(18,0)   -- A 管道 (ATM)
////DECLARE @ICount int,   @IAmount decimal(18,0)   -- I 管道 (網路銀行)
////DECLARE @CMFCount int, @CMFAmount decimal(18,0) -- CMF 管道 (臨櫃)
////DECLARE @HCount int,   @HAmount decimal(18,0)   -- H 管道 (匯款)
////DECLARE @C09Count int, @C09Amount decimal(18,0) -- C09 管道 (支付寶)
////DECLARE @MarketCount int, @MarketFee decimal(18,0) -- Market (超商管道) 小計
////DECLARE @SubCount int, @SubAmount decimal(18,0) -- Sub (P~C09管道) 小計
////
////DECLARE @DataCount int
////SET @DataCount = 0;
////
////DECLARE myCursor CURSOR FOR
//// SELECT RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id
////      , YL.Year_Name, TL.Term_Name, /*DL.Dep_Name*/ '' AS Dep_Name, RL.Receive_Name
////   FROM Receive_List AS RL
////   JOIN Year_List    AS YL ON YL.Year_Id = RL.Year_Id
////   JOIN Term_List    AS TL ON TL.status = '0' AND TL.Receive_Type = RL.Receive_Type AND TL.Year_Id = RL.Year_Id AND TL.Term_Id = RL.Term_Id 
////   --JOIN Dep_List     AS DL ON DL.status = '0' AND DL.Receive_Type = RL.Receive_Type AND DL.Year_Id = RL.Year_Id AND DL.Term_Id = RL.Term_Id AND DL.Dep_Id = RL.Dep_Id
////  WHERE RL.status = '0' AND RL.Receive_Type = @QReceiveType
////  ORDER BY RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id;
////
////OPEN myCursor;
////FETCH NEXT FROM myCursor INTO @ReceiveType, @YearId, @TermId, @DepId, @ReceiveId, @YearName, @TermName, @DepName, @ReceiveName;
////WHILE @@FETCH_STATUS = 0
////BEGIN
////    -- SO 管道 (收款單位自收)
////    SELECT @SOCount = COUNT(1), @SOAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('S');
////
////    -- Y 管道 (EDI)
////    SELECT @YCount = COUNT(1), @YAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('04');
////
////    -- P 管道 (郵局)
////    SELECT @PCount = 0, @PAmount = 0, @PostFee = 0;
////    /*
////    SELECT @PCount = COUNT(1), @PAmount = ISNULL(SUM(Receive_Amount), 0), @PostFee = 0
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('86');
////    */
////
////    -- G 管道 (統一超商)
////    SELECT @GCount = COUNT(1), @GAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('81');
////    -- D 管道 (全家超商)
////    SELECT @DCount = COUNT(1), @DAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('82');
////    -- N 管道 (萊爾富超商)
////    SELECT @NCount = COUNT(1), @NAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('85');
////    -- J 管道 (OK 超商)
////    SELECT @JCount = COUNT(1), @JAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('83');
////
////    -- B 管道 (語音銀行)
////    SELECT @BCount = COUNT(1), @BAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('06');
////
////    -- K 管道 (網路信用卡(財金))
////    SELECT @KCount = COUNT(1), @KAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('87');
////    -- W 管道 (信用卡(中國信託))
////    SELECT @WCount = COUNT(1), @WAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('80');
////
////    -- A 管道 (ATM)
////    SELECT @ACount = COUNT(1), @AAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('03');
////    -- I 管道 (網路銀行)
////    SELECT @ICount = COUNT(1), @IAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('05');
////    -- CMF 管道 (臨櫃)
////    SELECT @CMFCount = COUNT(1), @CMFAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('01');
////    -- H 管道 (匯款)
////    SELECT @HCount = COUNT(1), @HAmount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('02');
////
////    -- C09 管道 (支付寶)
////    SELECT @C09Count = COUNT(1), @C09Amount = ISNULL(SUM(Receive_Amount), 0)
////      FROM Student_Receive
////     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////       AND Receive_Way IN ('09');
////
////    SELECT @MarketCount = 0, @MarketFee = 0;
////
////    -- Market 小計
////    SELECT @MarketCount = COUNT(1), @MarketFee = ISNULL(SUM(Fee), 0)
////      FROM (SELECT ISNULL((SELECT TOP 1 RC_Charge FROM Receive_Channel WHERE RC_type = SR.Receive_Type AND RC_Channel = 'HILI'
////                       AND (CASE WHEN MoneyLimit = 0 THEN 999999999 ELSE MoneyLimit END) >= SR.Receive_Amount
////                       AND SR.Receive_Amount >= MoneyLowerLimit), 0) AS Fee
////              FROM Student_Receive AS SR
////             WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
////              AND Receive_Way IN ('81', '82', '85', '83')
////           ) AS Market;
////
////    SET @SubCount = @PCount + @GCount + @DCount + @NCount + @JCount + @BCount + @KCount + @WCount + @ACount + @ICount + @CMFCount + @HCount + @C09Count;
////    SET @SubAmount = @PAmount + @GAmount + @DAmount + @NAmount + @JAmount + @BAmount + @KAmount + @WAmount + @AAmount + @IAmount + @CMFAmount + @HAmount + @C09Amount - @PostFee - @MarketFee;
////
////    -- 刪除舊資料
////    DELETE cancel_result WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId;
////
////    -- 新增新資料
////    INSERT INTO cancel_result (Account_Date, Receive_Type, Year_Id, Year_Name, Term_Id, Term_Name, Dep_Id, Dep_Name, Receive_Id, Receive_Name
////         , SO_Count, SO_Amount, Y_Count, Y_Amount, P_Count, P_Amount, G_Count, G_Amount, D_Count, D_Amount, N_Count, N_Amount, J_Count, J_Amount
////         , B_Count, B_Amount, K_Count, K_Amount, W_Count, W_Amount, A_Count, A_Amount, I_Count, I_Amount, CMF_Count, CMF_Amount, H_Count, H_Amount
////         , C09_Count, C09_Amount
////         , Market_Count, Market_Fee, Sub_Count, Sub_Amount, Total_Count, Total_Amount, Create_Date, Post_Fee)
////    SELECT @QAccountDate AS Account_Date, @ReceiveType AS Receive_Type, @YearId AS Year_Id, @YearName AS Year_Name
////         , @TermId AS Term_Id, @TermName AS Term_Name, @DepId AS Dep_Id, @DepName AS Dep_Name
////         , @ReceiveId AS Receive_Id, @ReceiveName AS Receive_Name
////         , @SOCount AS SO_Count, @SOAmount AS SO_Amount
////         , @YCount AS Y_Count, @YAmount AS Y_Amount
////         , @PCount AS P_Count, @PAmount AS P_Amount
////         , @GCount AS G_Count, @GAmount AS G_Amount
////         , @DCount AS D_Count, @DAmount AS D_Amount
////         , @NCount AS N_Count, @NAmount AS N_Amount
////         , @JCount AS J_Count, @JAmount AS J_Amount
////         , @BCount AS B_Count, @BAmount AS B_Amount
////         , @KCount AS K_Count, @KAmount AS K_Amount
////         , @WCount AS W_Count, @WAmount AS W_Amount
////         , @ACount AS A_Count, @AAmount AS A_Amount
////         , @ICount AS I_Count, @IAmount AS I_Amount
////         , @CMFCount AS CMF_Count, @CMFAmount AS CMF_Amount
////         , @HCount AS H_Count, @HAmount AS H_Amount
////         , @C09Count AS C09_Count, @C09Amount AS C09_Amount
////         , @MarketCount AS Market_Count, @MarketFee AS Market_Fee
////         , @SubCount AS Sub_Count, @SubAmount AS Sub_Amount
////         , @SubCount + @SOCount + @YCount AS Total_Count, @SubAmount + @SOAmount + @YAmount AS Total_Amount
////         , GETDATE() AS Create_Date
////         , @PostFee AS post_fee;
////
////    IF @@ERROR = 0
////        SET @DataCount = @DataCount + 1;
////
////    FETCH NEXT FROM myCursor INTO @ReceiveType, @YearId, @TermId, @DepId, @ReceiveId, @YearName, @TermName, @DepName, @ReceiveName;
////END
////
////CLOSE myCursor;
////DEALLOCATE myCursor;
////
////SELECT @DataCount
////";
////                        #endregion
//                        #endregion

//                        string sqlPattern = @"
//DECLARE @QAccountDate char(7);
//DECLARE @QReceiveType varchar(6);
//SET @QAccountDate = '{0}';
//SET @QReceiveType = '{1}';
//
//DECLARE @ReceiveType varchar(6), @YearId char(3), @TermId char(1), @DepId char(1), @ReceiveId varchar(2);
//DECLARE @YearName nvarchar(8), @TermName nvarchar(20), @DepName nvarchar(20), @ReceiveName nvarchar(40);
//
//DECLARE @SOCount int,  @SOAmount decimal(18,0)  -- SO 管道 (收款單位自收)
//DECLARE @YCount int,   @YAmount decimal(18,0)   -- Y 管道 (EDI)
//DECLARE @PCount int,   @PAmount decimal(18,0), @PostFee decimal(18,0) -- P 管道 (郵局)
//DECLARE @GCount int,   @GAmount decimal(18,0)   -- G 管道 (統一超商)
//DECLARE @DCount int,   @DAmount decimal(18,0)   -- D 管道 (全家超商)
//DECLARE @NCount int,   @NAmount decimal(18,0)   -- N 管道 (萊爾富超商)
//DECLARE @JCount int,   @JAmount decimal(18,0)   -- J 管道 (OK 超商)
//DECLARE @BCount int,   @BAmount decimal(18,0)   -- B 管道 (語音銀行)
//DECLARE @KCount int,   @KAmount decimal(18,0)   -- K 管道 (網路信用卡(財金))
//DECLARE @WCount int,   @WAmount decimal(18,0)   -- W 管道 (信用卡(中國信託))
//DECLARE @ACount int,   @AAmount decimal(18,0)   -- A 管道 (ATM)
//DECLARE @ICount int,   @IAmount decimal(18,0)   -- I 管道 (網路銀行)
//DECLARE @CMFCount int, @CMFAmount decimal(18,0) -- CMF 管道 (臨櫃)
//DECLARE @HCount int,   @HAmount decimal(18,0)   -- H 管道 (匯款)
//DECLARE @C09Count int, @C09Amount decimal(18,0) -- C09 管道 (支付寶)
//DECLARE @C08Count int, @C08Amount decimal(18,0) -- C08 管道 (全國繳費網)
//DECLARE @C10Count int, @C10Amount decimal(18,0) -- C10 管道 (台灣Pay)
//DECLARE @MarketCount int, @MarketFee decimal(18,0) -- Market (超商管道) 小計
//DECLARE @SubCount int, @SubAmount decimal(18,0) -- Sub (P~C10管道) 小計
//
//DECLARE @DataCount int
//SET @DataCount = 0;
//
//DECLARE myCursor CURSOR FOR
// SELECT RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id
//      , YL.Year_Name, TL.Term_Name, /*DL.Dep_Name*/ '' AS Dep_Name, RL.Receive_Name
//   FROM Receive_List AS RL
//   JOIN Year_List    AS YL ON YL.Year_Id = RL.Year_Id
//   JOIN Term_List    AS TL ON TL.status = '0' AND TL.Receive_Type = RL.Receive_Type AND TL.Year_Id = RL.Year_Id AND TL.Term_Id = RL.Term_Id 
//   --JOIN Dep_List     AS DL ON DL.status = '0' AND DL.Receive_Type = RL.Receive_Type AND DL.Year_Id = RL.Year_Id AND DL.Term_Id = RL.Term_Id AND DL.Dep_Id = RL.Dep_Id
//  WHERE RL.status = '0' AND RL.Receive_Type = @QReceiveType
//  ORDER BY RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id;
//
//OPEN myCursor;
//FETCH NEXT FROM myCursor INTO @ReceiveType, @YearId, @TermId, @DepId, @ReceiveId, @YearName, @TermName, @DepName, @ReceiveName;
//WHILE @@FETCH_STATUS = 0
//BEGIN
//    -- SO 管道 (收款單位自收)
//    SELECT @SOCount = COUNT(1), @SOAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('S');
//
//    -- Y 管道 (EDI)
//    SELECT @YCount = COUNT(1), @YAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('04');
//
//    -- P 管道 (郵局)
//    SELECT @PCount = 0, @PAmount = 0, @PostFee = 0;
//    /*
//    SELECT @PCount = COUNT(1), @PAmount = ISNULL(SUM(Receive_Amount), 0), @PostFee = 0
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('86');
//    */
//
//    -- G 管道 (統一超商)
//    SELECT @GCount = COUNT(1), @GAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('81');
//    -- D 管道 (全家超商)
//    SELECT @DCount = COUNT(1), @DAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('82');
//    -- N 管道 (萊爾富超商)
//    SELECT @NCount = COUNT(1), @NAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('85');
//    -- J 管道 (OK 超商)
//    SELECT @JCount = COUNT(1), @JAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('83');
//
//    -- B 管道 (語音銀行)
//    SELECT @BCount = COUNT(1), @BAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('06');
//
//    -- K 管道 (網路信用卡(財金))
//    SELECT @KCount = COUNT(1), @KAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('87');
//    -- W 管道 (信用卡(中國信託))
//    SELECT @WCount = COUNT(1), @WAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('80');
//
//    -- A 管道 (ATM)
//    SELECT @ACount = COUNT(1), @AAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('03');
//    -- I 管道 (網路銀行)
//    SELECT @ICount = COUNT(1), @IAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('05');
//    -- CMF 管道 (臨櫃)
//    SELECT @CMFCount = COUNT(1), @CMFAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('01');
//    -- H 管道 (匯款)
//    SELECT @HCount = COUNT(1), @HAmount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('02');
//
//    -- C09 管道 (支付寶)
//    SELECT @C09Count = COUNT(1), @C09Amount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('09');
//
//    -- C08 管道 (全國繳費網)
//    SELECT @C08Count = COUNT(1), @C08Amount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('08');
//
//    -- C10 管道 (台灣Pay)
//    SELECT @C10Count = COUNT(1), @C10Amount = ISNULL(SUM(Receive_Amount), 0)
//      FROM Student_Receive
//     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//       AND Receive_Way IN ('10');
//
//    SELECT @MarketCount = 0, @MarketFee = 0;
//
//    -- Market 小計
//    SELECT @MarketCount = COUNT(1), @MarketFee = ISNULL(SUM(Fee), 0)
//      FROM (SELECT ISNULL((SELECT TOP 1 RC_Charge FROM Receive_Channel WHERE RC_type = SR.Receive_Type AND RC_Channel = 'Z'
//                       AND (CASE WHEN MoneyLimit = 0 THEN 999999999 ELSE MoneyLimit END) >= SR.Receive_Amount
//                       AND SR.Receive_Amount >= MoneyLowerLimit), 0) AS Fee
//              FROM Student_Receive AS SR
//             WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
//              AND Receive_Way IN ('81', '82', '85', '83')
//           ) AS Market;
//
//    SET @SubCount = @PCount + @GCount + @DCount + @NCount + @JCount + @BCount + @KCount + @WCount + @ACount + @ICount + @CMFCount + @HCount + @C09Count + @C08Count + @C10Count;
//    SET @SubAmount = @PAmount + @GAmount + @DAmount + @NAmount + @JAmount + @BAmount + @KAmount + @WAmount + @AAmount + @IAmount + @CMFAmount + @HAmount + @C09Amount + @C08Amount + @C10Amount - @PostFee - @MarketFee;
//
//    -- 刪除舊資料
//    DELETE cancel_result WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId;
//
//    -- 新增新資料
//    INSERT INTO cancel_result (Account_Date, Receive_Type, Year_Id, Year_Name, Term_Id, Term_Name, Dep_Id, Dep_Name, Receive_Id, Receive_Name
//         , SO_Count, SO_Amount, Y_Count, Y_Amount, P_Count, P_Amount, G_Count, G_Amount, D_Count, D_Amount, N_Count, N_Amount, J_Count, J_Amount
//         , B_Count, B_Amount, K_Count, K_Amount, W_Count, W_Amount, A_Count, A_Amount, I_Count, I_Amount, CMF_Count, CMF_Amount, H_Count, H_Amount
//         , C09_Count, C09_Amount, C08_Count, C08_Amount, C10_Count, C10_Amount
//         , Market_Count, Market_Fee, Sub_Count, Sub_Amount, Total_Count, Total_Amount, Create_Date, Post_Fee)
//    SELECT @QAccountDate AS Account_Date, @ReceiveType AS Receive_Type, @YearId AS Year_Id, @YearName AS Year_Name
//         , @TermId AS Term_Id, @TermName AS Term_Name, @DepId AS Dep_Id, @DepName AS Dep_Name
//         , @ReceiveId AS Receive_Id, @ReceiveName AS Receive_Name
//         , @SOCount AS SO_Count, @SOAmount AS SO_Amount
//         , @YCount AS Y_Count, @YAmount AS Y_Amount
//         , @PCount AS P_Count, @PAmount AS P_Amount
//         , @GCount AS G_Count, @GAmount AS G_Amount
//         , @DCount AS D_Count, @DAmount AS D_Amount
//         , @NCount AS N_Count, @NAmount AS N_Amount
//         , @JCount AS J_Count, @JAmount AS J_Amount
//         , @BCount AS B_Count, @BAmount AS B_Amount
//         , @KCount AS K_Count, @KAmount AS K_Amount
//         , @WCount AS W_Count, @WAmount AS W_Amount
//         , @ACount AS A_Count, @AAmount AS A_Amount
//         , @ICount AS I_Count, @IAmount AS I_Amount
//         , @CMFCount AS CMF_Count, @CMFAmount AS CMF_Amount
//         , @HCount AS H_Count, @HAmount AS H_Amount
//         , @C09Count AS C09_Count, @C09Amount AS C09_Amount
//         , @C08Count AS C08_Count, @C08Amount AS C08_Amount
//         , @C10Count AS C10_Count, @C10Amount AS C10_Amount
//         , @MarketCount AS Market_Count, @MarketFee AS Market_Fee
//         , @SubCount AS Sub_Count, @SubAmount AS Sub_Amount
//         , @SubCount + @SOCount + @YCount AS Total_Count, @SubAmount + @SOAmount + @YAmount AS Total_Amount
//         , GETDATE() AS Create_Date
//         , @PostFee AS post_fee;
//
//    IF @@ERROR = 0
//        SET @DataCount = @DataCount + 1;
//
//    FETCH NEXT FROM myCursor INTO @ReceiveType, @YearId, @TermId, @DepId, @ReceiveId, @YearName, @TermName, @DepName, @ReceiveName;
//END
//
//CLOSE myCursor;
//DEALLOCATE myCursor;
//
//SELECT @DataCount
//";
//                        #endregion
                        #endregion

                        #region [MDY:20191214] (2019擴充案) 國際信用卡 - NC 代收筆數、代收金額
                        string sqlPattern = @"
DECLARE @QAccountDate char(7), @QReceiveType varchar(6);
SET @QAccountDate = '{0}';
SET @QReceiveType = '{1}';

DECLARE @ReceiveType varchar(6), @YearId char(3), @TermId char(1), @DepId char(1), @ReceiveId varchar(2);
DECLARE @YearName nvarchar(8), @TermName nvarchar(20), @DepName nvarchar(20), @ReceiveName nvarchar(40);

DECLARE @SOCount  int, @SOAmount  decimal(18,0); /* SO = 收款單位自收 */
DECLARE @YCount   int, @YAmount   decimal(18,0); /* Y = EDI */
DECLARE @PCount   int, @PAmount   decimal(18,0), @PostFee decimal(18,0); /* P = 郵局 */
DECLARE @GCount   int, @GAmount   decimal(18,0); /* G = 統一超商 */
DECLARE @DCount   int, @DAmount   decimal(18,0); /* D = 全家超商 */
DECLARE @NCount   int, @NAmount   decimal(18,0); /* N = 萊爾富超商 */
DECLARE @JCount   int, @JAmount   decimal(18,0); /* J = OK 超商 */
DECLARE @BCount   int, @BAmount   decimal(18,0); /* B = 語音銀行 */
DECLARE @KCount   int, @KAmount   decimal(18,0); /* K = 網路信用卡(財金) */
DECLARE @WCount   int, @WAmount   decimal(18,0); /* W = 信用卡(中國信託) */
DECLARE @ACount   int, @AAmount   decimal(18,0); /* A = ATM */
DECLARE @ICount   int, @IAmount   decimal(18,0); /* I = 網路銀行 */
DECLARE @CMFCount int, @CMFAmount decimal(18,0); /* CMF = 臨櫃 */
DECLARE @HCount   int, @HAmount   decimal(18,0); /* H = 匯款 */
DECLARE @C09Count int, @C09Amount decimal(18,0); /* C09 = 支付寶 */
DECLARE @C08Count int, @C08Amount decimal(18,0); /* C08 = 全國繳費網 */
DECLARE @C10Count int, @C10Amount decimal(18,0); /* C10 = 台灣Pay */
DECLARE @NCCount  int, @NCAmount  decimal(18,0); /* NC = 國際信用卡 */

DECLARE @MarketCount int, @MarketFee decimal(18,0); /* Market = 超商管道 小計 */
DECLARE @SubCount int, @SubAmount decimal(18,0); /* Sub = P~NC管道 小計 */

DECLARE @DataCount int;
SET @DataCount = 0;

DECLARE myCursor CURSOR FOR
 SELECT RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id
      , YL.Year_Name, TL.Term_Name, /*DL.Dep_Name*/ '' AS Dep_Name, RL.Receive_Name
   FROM Receive_List AS RL
   JOIN Year_List    AS YL ON YL.Year_Id = RL.Year_Id
   JOIN Term_List    AS TL ON TL.status = '0' AND TL.Receive_Type = RL.Receive_Type AND TL.Year_Id = RL.Year_Id AND TL.Term_Id = RL.Term_Id 
   /* JOIN Dep_List     AS DL ON DL.status = '0' AND DL.Receive_Type = RL.Receive_Type AND DL.Year_Id = RL.Year_Id AND DL.Term_Id = RL.Term_Id AND DL.Dep_Id = RL.Dep_Id */
  WHERE RL.status = '0' AND RL.Receive_Type = @QReceiveType
  ORDER BY RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id;

OPEN myCursor;
FETCH NEXT FROM myCursor INTO @ReceiveType, @YearId, @TermId, @DepId, @ReceiveId, @YearName, @TermName, @DepName, @ReceiveName;
WHILE @@FETCH_STATUS = 0
BEGIN
    /* SO = 收款單位自收 */
    SELECT @SOCount = COUNT(1), @SOAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('S');

    /* Y = EDI */
    SELECT @YCount = COUNT(1), @YAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('04');

    /* P = 郵局 */
    SELECT @PCount = 0, @PAmount = 0, @PostFee = 0;
    /*
    SELECT @PCount = COUNT(1), @PAmount = ISNULL(SUM(Receive_Amount), 0), @PostFee = 0
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('86');
    */

    /* G = 統一超商 */
    SELECT @GCount = COUNT(1), @GAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('81');
    /* D = 全家超商 */
    SELECT @DCount = COUNT(1), @DAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('82');
    /* N = 萊爾富超商 */
    SELECT @NCount = COUNT(1), @NAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('85');
    /* J = OK 超商 */
    SELECT @JCount = COUNT(1), @JAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('83');

    /* B = 語音銀行 */
    SELECT @BCount = COUNT(1), @BAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('06');

    /* K = 網路信用卡(財金) */
    SELECT @KCount = COUNT(1), @KAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('87');
    /* W = 信用卡(中國信託) */
    SELECT @WCount = COUNT(1), @WAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('80');

    /* A = ATM */
    SELECT @ACount = COUNT(1), @AAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('03');
    /* I = 網路銀行 */
    SELECT @ICount = COUNT(1), @IAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('05');
    /* CMF = 臨櫃 */
    SELECT @CMFCount = COUNT(1), @CMFAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('01');
    /* H = 匯款 */
    SELECT @HCount = COUNT(1), @HAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('02');

    /* C09 = 支付寶 */
    SELECT @C09Count = COUNT(1), @C09Amount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('09');

    /* C08 = 全國繳費網 */
    SELECT @C08Count = COUNT(1), @C08Amount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('08');

    /* C10 = 台灣Pay */
    SELECT @C10Count = COUNT(1), @C10Amount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('10');

    /* NC = 國際信用卡 */
    SELECT @NCCount = COUNT(1), @NCAmount = ISNULL(SUM(Receive_Amount), 0)
      FROM Student_Receive
     WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
       AND Receive_Way IN ('NC');

    SELECT @MarketCount = 0, @MarketFee = 0;

    /* Market = 超商管道 小計 */
    SELECT @MarketCount = COUNT(1), @MarketFee = ISNULL(SUM(Fee), 0)
      FROM (SELECT ISNULL((SELECT TOP 1 RC_Charge FROM Receive_Channel WHERE RC_type = SR.Receive_Type AND RC_Channel = 'Z'
                              AND (CASE WHEN MoneyLimit = 0 THEN 999999999 ELSE MoneyLimit END) >= SR.Receive_Amount
                              AND SR.Receive_Amount >= MoneyLowerLimit), 0) AS Fee
              FROM Student_Receive AS SR
             WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId
               AND Receive_Way IN ('81', '82', '85', '83')
           ) AS Market;

    SET @SubCount = @PCount + @GCount + @DCount + @NCount + @JCount + @BCount + @KCount + @WCount + @ACount + @ICount + @CMFCount + @HCount + @C09Count + @C08Count + @C10Count + @NCCount;
    SET @SubAmount = @PAmount + @GAmount + @DAmount + @NAmount + @JAmount + @BAmount + @KAmount + @WAmount + @AAmount + @IAmount + @CMFAmount + @HAmount + @C09Amount + @C08Amount + @C10Amount + @NCAmount - @PostFee - @MarketFee;

    -- 刪除舊資料
    DELETE cancel_result WHERE Account_Date = @QAccountDate AND Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId;

    -- 新增新資料
    INSERT INTO cancel_result (Account_Date, Receive_Type, Year_Id, Year_Name, Term_Id, Term_Name, Dep_Id, Dep_Name, Receive_Id, Receive_Name
         , SO_Count, SO_Amount, Y_Count, Y_Amount, P_Count, P_Amount, G_Count, G_Amount, D_Count, D_Amount, N_Count, N_Amount, J_Count, J_Amount
         , B_Count, B_Amount, K_Count, K_Amount, W_Count, W_Amount, A_Count, A_Amount, I_Count, I_Amount, CMF_Count, CMF_Amount, H_Count, H_Amount
         , C09_Count, C09_Amount, C08_Count, C08_Amount, C10_Count, C10_Amount, NC_Count, NC_Amount
         , Market_Count, Market_Fee, Sub_Count, Sub_Amount, Total_Count, Total_Amount, Create_Date, Post_Fee)
    SELECT @QAccountDate AS Account_Date, @ReceiveType AS Receive_Type, @YearId AS Year_Id, @YearName AS Year_Name
         , @TermId AS Term_Id, @TermName AS Term_Name, @DepId AS Dep_Id, @DepName AS Dep_Name
         , @ReceiveId AS Receive_Id, @ReceiveName AS Receive_Name
         , @SOCount AS SO_Count, @SOAmount AS SO_Amount
         , @YCount AS Y_Count, @YAmount AS Y_Amount
         , @PCount AS P_Count, @PAmount AS P_Amount
         , @GCount AS G_Count, @GAmount AS G_Amount
         , @DCount AS D_Count, @DAmount AS D_Amount
         , @NCount AS N_Count, @NAmount AS N_Amount
         , @JCount AS J_Count, @JAmount AS J_Amount
         , @BCount AS B_Count, @BAmount AS B_Amount
         , @KCount AS K_Count, @KAmount AS K_Amount
         , @WCount AS W_Count, @WAmount AS W_Amount
         , @ACount AS A_Count, @AAmount AS A_Amount
         , @ICount AS I_Count, @IAmount AS I_Amount
         , @CMFCount AS CMF_Count, @CMFAmount AS CMF_Amount
         , @HCount AS H_Count, @HAmount AS H_Amount
         , @C09Count AS C09_Count, @C09Amount AS C09_Amount
         , @C08Count AS C08_Count, @C08Amount AS C08_Amount
         , @C10Count AS C10_Count, @C10Amount AS C10_Amount
         , @NCCount AS NC_Count, @NCAmount AS NC_Amount
         , @MarketCount AS Market_Count, @MarketFee AS Market_Fee
         , @SubCount AS Sub_Count, @SubAmount AS Sub_Amount
         , @SubCount + @SOCount + @YCount AS Total_Count, @SubAmount + @SOAmount + @YAmount AS Total_Amount
         , GETDATE() AS Create_Date
         , @PostFee AS post_fee;

    IF @@ERROR = 0
        SET @DataCount = @DataCount + 1;

    FETCH NEXT FROM myCursor INTO @ReceiveType, @YearId, @TermId, @DepId, @ReceiveId, @YearName, @TermName, @DepName, @ReceiveName;
END

CLOSE myCursor;
DEALLOCATE myCursor;

SELECT @DataCount
";
                        #endregion
                        #endregion

                        KeyValue[] parameters = null;

                        List<string> retryReceiveTypes = new List<string>(qReceiveTyps);    //紀錄剩下要處理的商家代號集合
                        for (; hasRetryTimes <= retryTimes; hasRetryTimes++)
                        {
                            List<string> failReceiveTypes = new List<string>(retryReceiveTypes.Count);  //紀錄失敗的商家代號集合
                            foreach (string retryReceiveType in retryReceiveTypes)
                            {
                                string sql = String.Format(sqlPattern, qAccountDate, retryReceiveType);
                                object dataCount = 0;
                                Result result = factory.ExecuteScalar(sql, parameters, out dataCount);
                                if (!result.IsSuccess)
                                {
                                    failReceiveTypes.Add(retryReceiveType);
                                    log.AppendFormat("第 {0} 次商家代號 {1} 資料計算失敗，錯誤訊息：{2}", hasRetryTimes, retryReceiveType, result.Message).AppendLine();
                                }
                            }

                            retryReceiveTypes.Clear();
                            if (failReceiveTypes.Count > 0)
                            {
                                //把失敗的商家代號放到剩下要處理的商家代號集合
                                retryReceiveTypes.AddRange(failReceiveTypes);
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (retryReceiveTypes.Count == 0)
                        {
                            //沒有剩下要處理的商家代號，表示全部處理成功
                            log.AppendLine("所有商家代號的資料計算皆成功");
                        }
                        else if (qReceiveTyps.Count > retryReceiveTypes.Count)
                        {
                            log.AppendLine("部份商家代號的資料計算失敗");
                        }
                        else
                        {
                            log.AppendLine("所有商家代號的資料計算皆失敗");
                            errmsg = "所有商家代號的資料計算皆失敗";
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Format("發生例外，{0}", ex.Message);
            }
            logmsg = log.ToString();
            return errmsg;
        }
        #endregion

        /// <summary>
        /// 自收銷帳 (銷編與金額要完全相同才能銷帳) (此方法傳回錯訊息，不會異動 err_msg 屬性)
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="cancelNo"></param>
        /// <param name="amount"></param>
        /// <param name="receiveDate"></param>
        /// <param name="accountDate"></param>
        /// <returns></returns>
        public string CancelDataByCANL(EntityFactory factory, string cancelNo, decimal amount, DateTime receiveDate, DateTime accountDate)
        {
            StudentReceiveEntity data = null;
            Expression where = new Expression(StudentReceiveEntity.Field.CancelNo, cancelNo)
                .And(StudentReceiveEntity.Field.ReceiveType, cancelNo.Substring(0, 4))
                .And(StudentReceiveEntity.Field.ReceiveAmount, amount)
                .And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty))
                .And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty))
                .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty));
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
            orderbys.Add(StudentReceiveEntity.Field.YearId, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveEntity.Field.TermId, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveEntity.Field.ReceiveType, OrderByEnum.Asc);
            orderbys.Add(StudentReceiveEntity.Field.CreateDate, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveEntity.Field.OldSeq, OrderByEnum.Desc);
            Result result = factory.SelectFirst<StudentReceiveEntity>(where, orderbys, out data);
            if (result.IsSuccess)
            {
                if (data == null)
                {
                    return "查無符合的未繳費資料";
                }
                else
                {
                    KeyValue[] fieldValues = new KeyValue[] {
                        new KeyValue(StudentReceiveEntity.Field.CancelFlag, CancelFlagCodeTexts.RECEIVE_SELF),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveDate, Common.GetTWDate7(receiveDate)),
                        new KeyValue(StudentReceiveEntity.Field.AccountDate, Common.GetTWDate7(accountDate)),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveTime, DateTime.Now.ToString("HHmmss")),
                        new KeyValue(StudentReceiveEntity.Field.CancelDate, DateTime.Today.ToString("yyyyMMdd")),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveWay, ChannelHelper.SELF),
                        new KeyValue(StudentReceiveEntity.Field.ReceivebankId, string.Empty)
                    };

                    where = new Expression(StudentReceiveEntity.Field.CancelNo, cancelNo)
                        .And(StudentReceiveEntity.Field.ReceiveAmount, amount)
                        .And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty))
                        .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty))
                        .And(StudentReceiveEntity.Field.ReceiveType, data.ReceiveType)
                        .And(StudentReceiveEntity.Field.YearId, data.YearId)
                        .And(StudentReceiveEntity.Field.TermId, data.TermId)
                        .And(StudentReceiveEntity.Field.DepId, data.DepId)
                        .And(StudentReceiveEntity.Field.ReceiveId, data.ReceiveId)
                        .And(StudentReceiveEntity.Field.StuId, data.StuId)
                        .And(StudentReceiveEntity.Field.OldSeq, data.OldSeq);

                    int count = 0;
                    result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                    if (result.IsSuccess)
                    {
                        if (count == 0)
                        {
                            return "更新銷帳註記失敗，錯誤訊息資料不存在";
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return result.Message;
                    }
                }
            }
            else
            {
                return result.Message;
            }
        }


        #region 土銀委扣相關
        /// <summary>
        /// 土銀委扣銷帳回覆檔銷帳處理 (此方法傳回錯訊息，不會異動 err_msg 屬性)
        /// </summary>
        /// <param name="dbLogger"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="fileContent"></param>
        /// <param name="resultText"></param>
        /// <returns></returns>
        public string CancelByBankDeductionResult(DBLogger dbLogger, string receiveType, string yearId, string termId, string depId, string receiveId, string fileContent, out string resultText)
        {
            resultText = null;
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId) || String.IsNullOrEmpty(fileContent))
            {
                return "缺少參數";
            }

            #region 客戶委託代號
            string myDeductId = null;
            {
                SchoolRTypeEntity school = null;
                string errmsg = this.GetSchoolRType(receiveType, out school);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    return errmsg;
                }
                if (school == null || String.IsNullOrEmpty(school.DeductId))
                {
                    return "該商家代號未設定客戶委託代號";
                }
                myDeductId = school.DeductId;
            }
            #endregion

            StringBuilder msg = new StringBuilder();

            #region [MDY:20220530] Checkmarx 調整
            try
            {
                using (System.IO.StringReader reader = new System.IO.StringReader(fileContent))
                {
                    int lineNo = 0;

                    #region 首錄
                    string accountDate = null;
                    string receiveBankId = null;
                    string bankId = null;
                    string twdate8 = null;
                    {
                        string line = reader.ReadLine();
                        lineNo = 1;

                        #region 長度
                        if (line == null || line.Length != 200)
                        {
                            return String.Format("第{0}行資料長度不正確", lineNo);
                        }
                        #endregion

                        #region 錄別 (首筆為1)
                        if (line.Substring(0, 1) != "1")
                        {
                            return String.Format("第{0}行資料的錄別不正確", lineNo);
                        }
                        #endregion

                        #region 客戶委託編號
                        //02    發件單位(客戶委託編號)  002 ~ 009   008   X(8)    土銀資訊室編定之委託代號，左靠右補空白，通常為4碼 
                        if (myDeductId != line.Substring(1, 8).Trim())
                        {
                            return String.Format("第{0}行資料的客戶委託編號不正確", lineNo);
                        }
                        #endregion

                        #region 收件單位
                        //03    收件單位                010 ~ 017   008   X(8)    土銀代號(005)+分行代號+檢查碼，左靠右補空白
                        bankId = line.Substring(9, 8).Trim();
                        if (String.IsNullOrEmpty(bankId) || bankId.Length != 7)
                        {
                            return String.Format("第{0}行資料的收件單位不正確", lineNo);
                        }
                        if (bankId.StartsWith(DataFormat.MyBankID))
                        {
                            //為了與中心銷帳資料一致，所以只取分行代碼 3 碼
                            receiveBankId = bankId.Substring(3, 3);
                        }
                        #endregion

                        #region 轉帳日 (YYYYMMDD(國曆))
                        //05    轉帳日                  023 ~ 030   008   9(8)    YYYYMMDD(國曆)
                        {
                            twdate8 = line.Substring(22, 8).Trim();
                            DateTime date;
                            if (Common.TryConvertTWDate8(twdate8, out date))
                            {
                                accountDate = Common.GetTWDate7(date);
                            }
                            if (String.IsNullOrEmpty(accountDate))
                            {
                                return String.Format("第{0}行資料的轉帳日不正確", lineNo);
                            }
                        }
                        #endregion

                        #region 資料性質別 (2:轉帳結果(處理後退回))
                        //06    資料性質別              031 ~ 031   001   9(1)    1:委託轉帳(原始資料)    2:轉帳結果(處理後退回)
                        if (line.Substring(30, 1) != "2")
                        {
                            return String.Format("第{0}行資料的資料性質別不正確", lineNo);
                        }
                        #endregion
                    }
                    #endregion

                    decimal sumAmount = 0;

                    #region 明細 + 尾錄
                    using (EntityFactory factory = new EntityFactory())
                    {
                        int cancelOkCount = 0;
                        int cancelFailCount = 0;
                        string line = null;
                        while ((line = reader.ReadLine()) != null)
                        {
                            lineNo++;
                            decimal receiveAmount = 0;
                            string stuId = null;
                            string cancelNo = null;
                            string status = null;

                            try
                            {
                                #region 長度
                                if (line.Length != 200)
                                {
                                    msg.AppendFormat("第{0}行資料長度不正確，此資料不處理", lineNo).AppendLine();
                                    continue;
                                }
                                #endregion

                                #region 客戶委託編號
                                //02    發件單位(客戶委託編號)  002 ~ 009   008   X(8)        土銀資訊室編定之委託代號，左靠右補空白，通常為4碼
                                if (myDeductId != line.Substring(1, 8).Trim())
                                {
                                    msg.AppendFormat("第{0}行資料的客戶委託編號不正確，此資料不處理", lineNo).AppendLine();
                                    continue;
                                }
                                #endregion

                                #region 收件單位
                                //03    收件單位                010 ~ 017   008   X(8)        土銀代號(005)+分行代號+檢查碼，左靠右補空白
                                if (bankId != line.Substring(9, 8).Trim())
                                {
                                    msg.AppendFormat("第{0}行資料的收件單位不正確，此資料不處理", lineNo).AppendLine();
                                    continue;
                                }
                                #endregion

                                #region 轉帳日 (YYYYMMDD(國曆))
                                //05    轉帳日                  023 ~ 030   008   9(8)        YYYYMMDD(國曆)
                                if (twdate8 != line.Substring(22, 8).Trim())
                                {
                                    msg.AppendFormat("第{0}行資料的轉帳日不正確，此資料不處理", lineNo).AppendLine();
                                    continue;
                                }
                                #endregion

                                //錄別 (明細每筆為2，尾錄為3)
                                string lineType = line.Substring(0, 1);
                                if (lineType == "2")
                                {
                                    #region 明細
                                    #region 交易金額
                                    //07    交易金額                051 ~ 064   014   9(12)V9(2)  整數位不足時，右靠左補零，小數2位
                                    if (Decimal.TryParse(line.Substring(50, 12).Trim(), out receiveAmount))
                                    {
                                        sumAmount += receiveAmount;
                                    }
                                    else
                                    {
                                        msg.AppendFormat("第{0}行資料的交易金額不正確，此資料不處理", lineNo).AppendLine();
                                        continue;
                                    }
                                    #endregion

                                    #region 狀況代號
                                    //09    狀況代號                073 ~ 076   004   9(4)        通知(即委託時)為9999
                                    status = line.Substring(72, 4);
                                    #endregion

                                    #region 學號
                                    //10a   專用資料區a             077 ~ 115   039   X(39)       學雜費保留 (改成放學號)
                                    stuId = line.Substring(76, 20);
                                    #endregion

                                    #region 虛擬帳號
                                    //10b   專用資料區b             116 ~ 166   051   X(51)       學雜費用來放虛擬帳號
                                    cancelNo = line.Substring(115, 20);
                                    if (String.IsNullOrEmpty(cancelNo))
                                    {
                                        msg.AppendFormat("第{0}行資料的虛擬帳號不正確，此資料不處理", lineNo);
                                        continue;
                                    }
                                    #endregion

                                    string cancelResult = null;
                                    if (status == "0000")
                                    {
                                        cancelResult = this.CancelDataByLB(dbLogger, factory, receiveType, yearId, termId, depId, receiveId, cancelNo.Trim(), receiveAmount, accountDate, receiveBankId);
                                        if (String.IsNullOrEmpty(cancelResult))
                                        {
                                            cancelResult = "銷帳成功";
                                            cancelOkCount++;
                                        }
                                        else
                                        {
                                            cancelFailCount++;
                                        }
                                    }
                                    else
                                    {
                                        cancelResult = "委扣失敗不銷帳";
                                        cancelFailCount++;
                                    }

                                    msg.AppendFormat("學號：{0}  虛擬帳號：{1}  委扣結果：{2}  銷帳處理結果：{3}", stuId, cancelNo, this.GetBankDeductionStatusText(status).PadRight(9, '　'), cancelResult).AppendLine();
                                    #endregion
                                }
                                else if (lineType == "3")
                                {
                                    #region 尾錄
                                    #region 成交總金額
                                    string okTotalAmount = line.Substring(30, 14).Trim();
                                    #endregion

                                    #region 成交總筆數
                                    string okTotalRecord = line.Substring(46, 10).Trim();
                                    #endregion

                                    #region 未成交總金額
                                    string failTotalAmount = line.Substring(56, 14).Trim();
                                    #endregion

                                    #region 未成交總筆數
                                    string failTotalRecord = line.Substring(72, 10).Trim();
                                    #endregion

                                    msg
                                        .AppendFormat("成交總金額　：{0}   成交總筆數　：{1}", okTotalAmount, okTotalRecord).AppendLine()
                                        .AppendFormat("未成交總金額：{0}   未成交總筆數：{1}", failTotalAmount, failTotalRecord).AppendLine();
                                    #endregion
                                }
                                else
                                {
                                    msg.AppendFormat("第{0}行資料的錄別不正確，此資料不處理", lineNo).AppendLine();
                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                msg.AppendFormat("第{0}行資料處理發生例外，{1}", lineNo, ex.Message).AppendLine();
                            }
                        }
                        msg.AppendLine().AppendFormat("銷帳處理：成功 {0} 筆，失敗 {1} 筆", cancelOkCount, cancelFailCount).AppendLine();
                    }
                    #endregion
                }
            }
            catch (Exception)
            {
                return "委扣銷帳回覆檔銷帳處理失敗";
            }
            finally
            {
                resultText = msg.ToString();
            }
            #endregion

            return null;
        }

        /// <summary>
        /// 取得土銀委扣狀況代號的文字說明
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private string GetBankDeductionStatusText(string status)
        {
            switch (status)
            {
                case "0000":
                    return "扣款成功";
                case "0001":
                    return "存款不足";
                case "0002":
                    return "未申請代扣";
                case "0003":
                    return "已中止代扣";
                case "0004":
                    return "用戶號碼錯誤";
                case "0005":
                    return "無此帳號";
                case "0006":
                    return "帳號已銷戶";
                case "0007":
                    return "已移出或有止押質扣";
                case "0098":
                    return "其他不成功原因";
                case "9999":
                    return "送土銀轉帳之起始值";
                default:
                    return String.Empty;
            }
        }

        /// <summary>
        /// 土銀委扣銷帳 (銷編與金額要完全相同才能銷帳)
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="cancelNo"></param>
        /// <param name="receiveAmount"></param>
        /// <param name="accountDate"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        private string CancelDataByLB(DBLogger dbLogger, EntityFactory factory, string receiveType, string yearId, string termId, string depId, string receiveId, string cancelNo, decimal receiveAmount, string accountDate, string receiveBankId)
        {
            StudentReceiveEntity data = null;
            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                .And(StudentReceiveEntity.Field.YearId, yearId)
                .And(StudentReceiveEntity.Field.TermId, termId)
                .And(StudentReceiveEntity.Field.DepId, depId)
                .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                .And(StudentReceiveEntity.Field.CancelNo, cancelNo)
                .And(StudentReceiveEntity.Field.ReceiveAmount, receiveAmount)
                .And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty))
                .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty));

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReceiveEntity.Field.CreateDate, OrderByEnum.Asc);
            Result result = factory.SelectFirst<StudentReceiveEntity>(where, orderbys, out data);
            if (result.IsSuccess)
            {
                if (data == null)
                {
                    return "查無符合的未繳費資料";
                }
                else
                {
                    KeyValue[] fieldValues = new KeyValue[] {
                        new KeyValue(StudentReceiveEntity.Field.CancelFlag, CancelFlagCodeTexts.RECEIVE_SELF),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveDate, accountDate),
                        new KeyValue(StudentReceiveEntity.Field.AccountDate, accountDate),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveTime, String.Empty),
                        new KeyValue(StudentReceiveEntity.Field.CancelDate, DateTime.Today.ToString("yyyyMMdd")),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveWay, ChannelHelper.LB),
                        new KeyValue(StudentReceiveEntity.Field.ReceivebankId, receiveBankId)
                    };

                    where = new Expression(StudentReceiveEntity.Field.ReceiveType, data.ReceiveType)
                        .And(StudentReceiveEntity.Field.YearId, data.YearId)
                        .And(StudentReceiveEntity.Field.TermId, data.TermId)
                        .And(StudentReceiveEntity.Field.DepId, data.DepId)
                        .And(StudentReceiveEntity.Field.ReceiveId, data.ReceiveId)
                        .And(StudentReceiveEntity.Field.StuId, data.StuId)
                        .And(StudentReceiveEntity.Field.OldSeq, data.OldSeq)
                        .And(StudentReceiveEntity.Field.CancelNo, cancelNo)
                        .And(StudentReceiveEntity.Field.ReceiveAmount, receiveAmount)
                        .And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty))
                        .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty));

                    int count = 0;
                    result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);

                    #region 新增日誌資料
                    if (dbLogger != null)
                    {
                        string funcName = "匯入委扣回覆資料";
                        string notation = null;
                        if (result.IsSuccess)
                        {
                            notation = String.Format("[{0}] {1}學生繳費資料銷帳註記相關欄位成功", funcName, LogTypeCodeTexts.UPDATE);
                        }
                        else
                        {
                            notation = String.Format("[{0}] {1}學生繳費資料銷帳註記相關欄位失敗 (錯誤訊息：{2})", funcName, LogTypeCodeTexts.UPDATE, result.Message);
                        }
                        dbLogger.AppendLog(dbLogger.Role, dbLogger.ReceiveType, dbLogger.FunctionId, LogTypeCodeTexts.UPDATE, dbLogger.UserId, notation);
                    }
                    #endregion

                    if (result.IsSuccess)
                    {
                        if (count == 0)
                        {
                            return "更新銷帳註記失敗，資料不存在";
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return "更新銷帳註記失敗，" + result.Message;
                    }
                }
            }
            else
            {
                return "查詢學生繳費資料失敗，" + result.Message;
            }
        }
        #endregion

        #region SchoolRtype 相關
        /// <summary>
        /// 取的所有商家代號資料
        /// </summary>
        /// <param name="schools"></param>
        /// <returns></returns>
        public int getAllSchoolRtype(out SchoolRTypeEntity[] schools)
        {
            int rc = -1;
            _err_msg = "";

            schools=null;
            EntityFactory factory = new EntityFactory();

            Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(SchoolRTypeEntity.Field.CorpType, OrderByEnum.Asc);
            orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);
            orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
            Result xmlResult = factory.SelectAll<SchoolRTypeEntity>(where, orderbys, out schools);
            if (!xmlResult.IsSuccess)
            {
                rc = -9;
                _err_msg = xmlResult.Message;
            }
            else
            {
                if (schools != null && schools.Length>0)
                {
                    rc = 0;
                }
                else
                {
                    rc = -1;
                    _err_msg = "查無資料";
                }
            }

            return rc;
        }

        /// <summary>
        /// 取的有設定 FTP 的商家代號資料
        /// </summary>
        /// <param name="schools"></param>
        /// <returns></returns>
        public bool GetHasFTPSchoolRtypes(out SchoolRTypeEntity[] schools)
        {
            bool isOK = true;
            _err_msg = String.Empty;

            schools = null;
            using (EntityFactory factory = new EntityFactory())
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL)
                    .And(SchoolRTypeEntity.Field.FtpAccount, RelationEnum.NotEqual, String.Empty)
                    .And(SchoolRTypeEntity.Field.FtpAccount, RelationEnum.NotEqual, null)
                    .And(SchoolRTypeEntity.Field.FtpLocation, RelationEnum.NotEqual, String.Empty)
                    .And(SchoolRTypeEntity.Field.FtpLocation, RelationEnum.NotEqual, null);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(SchoolRTypeEntity.Field.CorpType, OrderByEnum.Asc);
                orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);
                orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
                Result result = factory.SelectAll<SchoolRTypeEntity>(where, orderbys, out schools);
                if (!result.IsSuccess)
                {
                    isOK = result.IsSuccess;
                    _err_msg = result.Message;
                }
            }
            return isOK;
        }

        /// <summary>
        /// 取得代收各項費用種類的商家代號 (ReceiveKind=2)
        /// </summary>
        /// <param name="receiveTypes"></param>
        /// <returns></returns>
        public bool GetUPCTCBReceiveTypes(out List<string> receiveTypes)
        {
            _err_msg = String.Empty;
            receiveTypes = null;
            string sql = String.Format("SELECT {0} FROM {1} WHERE {2} = @Status AND {3} = @ReceiveKind ORDER BY {0}"
                , SchoolRTypeEntity.Field.ReceiveType, SchoolRTypeEntity.TABLE_NAME
                , SchoolRTypeEntity.Field.Status, SchoolRTypeEntity.Field.ReceiveKind);
            KeyValue[] parameters = new KeyValue[] {
                new KeyValue("@Status", DataStatusCodeTexts.NORMAL),
                new KeyValue("@ReceiveKind", ReceiveKindCodeTexts.UPCTCB)
            };

            bool isOK = false;
            using (EntityFactory factory = new EntityFactory())
            {
                System.Data.DataTable dt = null;
                Result result = factory.GetDataTable(sql, parameters, 0, 0, out dt);
                if (result.IsSuccess)
                {
                    isOK = true;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        receiveTypes = new List<string>(dt.Rows.Count);
                        foreach (System.Data.DataRow drow in dt.Rows)
                        {
                            string receiveType = !drow.IsNull(0) ? drow[0].ToString().Trim() : null;
                            if (!String.IsNullOrEmpty(receiveType))
                            {
                                receiveTypes.Add(receiveType);
                            }
                        }
                    }
                }
                else
                {
                    #region [MDY:20170925] 調整錯誤訊息
                    #region [Old]
                    //_err_msg = result.Message;
                    #endregion

                    _err_msg = String.Format("[GetUPCTCBReceiveTypes] 讀取代收各項費用種類的商家代號失敗，錯誤訊息={1}", result.Message);
                    #endregion
                }
            }

            if (receiveTypes == null)
            {
                receiveTypes = new List<string>(0);
            }
            return isOK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="school"></param>
        /// <returns></returns>
        public string GetSchoolRType(string receiveType, out SchoolRTypeEntity school)
        {
            school = null;
            using (EntityFactory factory = new EntityFactory())
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                if (!result.IsSuccess)
                {
                    return result.Message;
                }
            }
            return null;
        }
        #endregion

        #region [MDY:20160607] 處理更正交易相關
        /// <summary>
        /// 取得符合指定更正交易資料的可被更正資料
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="hData">指定更正交易資料</param>
        /// <param name="oData">傳回可被更正資料或 null</param>
        /// <returns>.傳回錯誤訊息或 null</returns>
        private string GetCancelDebtsForRectify(EntityFactory factory, CancelDebtsEntity hData, out CancelDebtsEntity oData)
        {
            oData = null;

            //1. 僅處理臨櫃管道的更正資料
            if (hData != null && (hData.Reserve2 == D00I70ECMarkCodeTexts.RECTIFY_CODE && ChannelHelper.IsCashChannel(hData.ReceiveWay)))
            {
                //2. 被更正的資料限制以更正資料中的交易日期與時間往前找第一筆相同交易日期、商家代號、虛擬帳號、金額、分行代碼的臨櫃資料
                //3. 已被更正的資料不能重覆更正
                //隱藏條件：D00I70的資料才處理
                //理論上交易時間一定是 6 碼的 HHmmss，為了避免意外使用 SourceSeq 做檢查與遞減排序
                Expression where = new Expression(CancelDebtsEntity.Field.ReceiveDate, hData.ReceiveDate)   //同交易日期
                    .And(CancelDebtsEntity.Field.ReceiveType, hData.ReceiveType)        //同商家代號
                    .And(CancelDebtsEntity.Field.CancelNo, hData.CancelNo)              //同虛擬帳號
                    .And(CancelDebtsEntity.Field.ReceiveAmount, hData.ReceiveAmount)    //同金額
                    .And(CancelDebtsEntity.Field.ReceiveBank, hData.ReceiveBank)        //分行代碼
                    .And(CancelDebtsEntity.Field.ReceiveWay, hData.ReceiveWay)          //同臨櫃
                    .And(CancelDebtsEntity.Field.ReceiveDate, hData.ReceiveDate)        //同交易日期
                    .And(CancelDebtsEntity.Field.FileName, hData.FileName)              //隱藏條件：D00I70的資料才處理
                    .And(CancelDebtsEntity.Field.SourceSeq, RelationEnum.Less, hData.SourceSeq); //往前找

                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(CancelDebtsEntity.Field.SourceSeq, OrderByEnum.Desc);

                Result result = factory.SelectFirst<CancelDebtsEntity>(where, orderbys, out oData);
                return result.Message;
            }
            else
            {
                return "缺少或不正確的更正交易資料參數";
            }
        }

        /// <summary>
        /// 處理更正交易資料
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="hData">指定請求更正交易資料</param>
        /// <param name="oData">指定要被更正交易資料</param>
        /// <returns></returns>
        private string RectifyCancelDebtsData(EntityFactory factory, CancelDebtsEntity hData, CancelDebtsEntity oData, DateTime mdyTime, out string extraLog)
        {
            extraLog = null;
            if (hData == null || oData == null
                || hData.Reserve2 != D00I70ECMarkCodeTexts.RECTIFY_CODE || hData.Status != CancelDebtsStatusCodeTexts.IS_PROCESSING_CODE || !ChannelHelper.IsCashChannel(hData.ReceiveWay)
                || oData.Reserve2 != D00I70ECMarkCodeTexts.NORMAL_CODE || !ChannelHelper.IsCashChannel(oData.ReceiveWay))
            {
                return "缺少請求更正交易資料或要被更正交易資料";
            }

            #region [MDY:20160921] 判斷是否為資料庫 Timeout
            bool isDbTimeout = false;
            #endregion

            string errmsg = null;
            switch (oData.Status)
            {
                case CancelDebtsStatusCodeTexts.HAS_CANCELED_CODE:
                    #region 已銷帳資料更正 (表示繳費資料要更正)
                    {
                        #region 找出對應的繳費資料，然後還原銷帳相關欄位
                        {
                            //只更正臨櫃的資料，所以 StudentReceiveEntity 的 ReceiveDate、ReceiveTime、AccountDate 一定跟 CancelDebtsEntity 一樣
                            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, oData.ReceiveType)
                                .And(StudentReceiveEntity.Field.CancelNo, oData.CancelNo)
                                .And(StudentReceiveEntity.Field.ReceiveDate, oData.ReceiveDate)
                                .And(StudentReceiveEntity.Field.ReceiveTime, oData.ReceiveTime)
                                .And(StudentReceiveEntity.Field.AccountDate, oData.AccountDate)
                                .And(StudentReceiveEntity.Field.ReceivebankId, oData.ReceiveBank)
                                .And(StudentReceiveEntity.Field.ReceiveWay, oData.ReceiveWay)
                                .And(StudentReceiveEntity.Field.ReceiveAmount, oData.ReceiveAmount)
                                .And(StudentReceiveEntity.Field.CancelDate, (oData.CancelDate == null ? null : oData.CancelDate.Value.ToString("yyyyMMdd")));
                            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);  //這個排序要比照 GetStudentReceive2()
                            orderbys.Add(StudentReceiveEntity.Field.YearId, OrderByEnum.Desc);
                            orderbys.Add(StudentReceiveEntity.Field.TermId, OrderByEnum.Desc);
                            orderbys.Add(StudentReceiveEntity.Field.ReceiveType, OrderByEnum.Asc);
                            orderbys.Add(StudentReceiveEntity.Field.CreateDate, OrderByEnum.Desc);
                            orderbys.Add(StudentReceiveEntity.Field.OldSeq, OrderByEnum.Desc);
                            StudentReceiveEntity data = null;
                            Result result = factory.SelectFirst<StudentReceiveEntity>(where, orderbys, out data);
                            if (result.IsSuccess)
                            {
                                if (data == null)
                                {
                                    errmsg = "查無對應的學生繳費單資料";
                                }
                                else
                                {
                                    KeyValueList fieldValues = new KeyValueList();
                                    fieldValues.Add(StudentReceiveEntity.Field.CancelFlag, String.Empty);
                                    fieldValues.Add(StudentReceiveEntity.Field.ReceiveDate, null);
                                    fieldValues.Add(StudentReceiveEntity.Field.AccountDate, null);
                                    fieldValues.Add(StudentReceiveEntity.Field.ReceiveTime, null);
                                    fieldValues.Add(StudentReceiveEntity.Field.CancelDate, null);
                                    fieldValues.Add(StudentReceiveEntity.Field.ReceiveWay, null);
                                    fieldValues.Add(StudentReceiveEntity.Field.ReceivebankId, null);

                                    where = new Expression(StudentReceiveEntity.Field.ReceiveType, data.ReceiveType)
                                        .And(StudentReceiveEntity.Field.YearId, data.YearId)
                                        .And(StudentReceiveEntity.Field.TermId, data.TermId)
                                        .And(StudentReceiveEntity.Field.DepId, data.DepId)
                                        .And(StudentReceiveEntity.Field.ReceiveId, data.ReceiveId)
                                        .And(StudentReceiveEntity.Field.StuId, data.StuId)
                                        .And(StudentReceiveEntity.Field.OldSeq, data.OldSeq)
                                        .And(StudentReceiveEntity.Field.CancelNo, data.CancelNo)
                                        .And(StudentReceiveEntity.Field.ReceiveDate, data.ReceiveDate)
                                        .And(StudentReceiveEntity.Field.ReceiveTime, data.ReceiveTime)
                                        .And(StudentReceiveEntity.Field.AccountDate, data.AccountDate)
                                        .And(StudentReceiveEntity.Field.ReceivebankId, data.ReceivebankId)
                                        .And(StudentReceiveEntity.Field.ReceiveWay, data.ReceiveWay)
                                        .And(StudentReceiveEntity.Field.ReceiveAmount, data.ReceiveAmount)
                                        .And(StudentReceiveEntity.Field.CancelDate, data.CancelDate);

                                    int count = 0;
                                    result = factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            errmsg = "更新對應的學生繳費單資料失敗，無資料被更新";
                                        }
                                    }
                                    else
                                    {
                                        #region [MDY:20160921] 判斷是否為資料庫 Timeout，如果是則 isDbTimeout 設為 true
                                        System.Data.Common.DbException dbException = result.Exception as System.Data.Common.DbException;
                                        isDbTimeout = (dbException != null && dbException.Message.IndexOf("Timeout", StringComparison.CurrentCultureIgnoreCase) > -1);
                                        if (isDbTimeout)
                                        {
                                            extraLog = "發生資料庫 Timeout，忽略此次更正處理";
                                        }
                                        #endregion

                                        errmsg = String.Format("更新對應的學生繳費單資料失敗，{0}", result.Message);
                                    }
                                }
                            }
                            else
                            {
                                errmsg = String.Format("查詢對應的學生繳費單資料失敗，{0}", result.Message);
                            }
                        }
                        #endregion

                        break;
                    }
                    #endregion

                case CancelDebtsStatusCodeTexts.IS_WAITING_CODE:
                    #region 待處理資料更正 (表示沒有其他資料要更正)
                    {
                        break;
                    }
                    #endregion

                case CancelDebtsStatusCodeTexts.IS_PROCESSING_CODE:
                    #region {MDY:20180520] 要被更正資料狀態為處理中，借用 isDbTimeout = true 讓 hData 請求更正交易資料的狀態改為待處理
                    #region [OLD}
                    //return "要更正交易資料正處理中，忽略此次更正處理";
                    #endregion

                    isDbTimeout = true;
                    errmsg = "要被更正交易資料正處理中，忽略此次更正處理";
                    break;
                    #endregion

                case CancelDebtsStatusCodeTexts.IS_FAILURE_CODE:
                    #region 處理失敗資料更正 (表示該資料進問題檔)
                    {
                        #region 找出對應的問題檔資料，然後刪除 (避免更正處理中斷，問題檔處理失敗不視為錯誤)
                        {
                            Expression where = new Expression(ProblemListEntity.Field.ReceiveType, oData.ReceiveType)
                                .And(ProblemListEntity.Field.CancelNo, oData.CancelNo)
                                .And(ProblemListEntity.Field.ReceiveDate, DataFormat.ConvertDateText(oData.ReceiveDate))
                                .And(ProblemListEntity.Field.ReceiveTime, oData.ReceiveTime)
                                .And(ProblemListEntity.Field.AccountDate, DataFormat.ConvertDateText(oData.AccountDate))
                                .And(ProblemListEntity.Field.BankId, oData.ReceiveBank)
                                .And(ProblemListEntity.Field.ReceiveWay, oData.ReceiveWay)
                                .And(ProblemListEntity.Field.PayAmount, oData.ReceiveAmount)
                                .And(ProblemListEntity.Field.CreateDate, RelationEnum.GreaterEqual, oData.ModifyDate.Date)
                                .And(ProblemListEntity.Field.CreateDate, RelationEnum.Less, oData.ModifyDate.Date.AddDays(1));
                            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                            orderbys.Add(ProblemListEntity.Field.Id, OrderByEnum.Desc);
                            ProblemListEntity data = null;
                            Result result = factory.SelectFirst<ProblemListEntity>(where, orderbys, out data);
                            if (result.IsSuccess)
                            {
                                if (data == null)
                                {
                                    //找不到有可能被人手動刪除，所以不一定是有問題，所以不視為錯誤
                                    extraLog = "查無對應的問題檔";
                                }
                                else
                                {
                                    int count = 0;
                                    result = factory.Delete(data, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            extraLog = "刪除對應的問題檔失敗，無資料被刪除";
                                        }
                                    }
                                    else
                                    {
                                        extraLog = String.Format("刪除對應的問題檔失敗，{0}", result.Message);
                                    }
                                }
                            }
                            else
                            {
                                extraLog = String.Format("查詢對應的問題檔失敗，{0}", result.Message);
                            }
                        }
                        #endregion

                        break;
                    }
                    #endregion

                case CancelDebtsStatusCodeTexts.IS_EXCLUDED_CODE:
                    errmsg = "更正處理失敗，(被更正)資料是免銷帳資料";
                    break;

                case CancelDebtsStatusCodeTexts.HAS_RECTIFIED_CODE:
                    errmsg = "更正處理失敗，(被更正)資料不是正常交易資料";
                    break;

                case CancelDebtsStatusCodeTexts.BE_RECTIFIED_CODE:
                    errmsg = "更正處理失敗，(被更正)資料已更正";
                    break;
            }

            #region 將被更正交易資料的Status更新為已被更正、ModifyDate與RollbackDate更新為處理時間
            if (String.IsNullOrEmpty(errmsg))
            {
                errmsg = this.UpdateCancelDebtsToBeRectified(factory, oData, mdyTime);
            }
            #endregion

            #region 將更正交易資料的Status更新為已更正(或處理失敗)、ModifyDate更新為處理時間
            {
                bool isOK = String.IsNullOrEmpty(errmsg);
                string errmsg2 = this.UpdateCancelDebtsForRECTIFY(factory, hData, isOK, isDbTimeout, mdyTime);
                if (!String.IsNullOrEmpty(errmsg2))
                {
                    errmsg += ";" + errmsg2;
                }
            }
            #endregion

            return errmsg;
        }

        /// <summary>
        /// 更新為被更正資料 (Status=已被更正、ModifyDate與RollbackDate=處理時間)
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="data"></param>
        /// <param name="isOK"></param>
        /// <param name="modifyDate"></param>
        /// <returns></returns>
        private string UpdateCancelDebtsToBeRectified(EntityFactory factory, CancelDebtsEntity data, DateTime modifyDate)
        {
            string errmsg = null;
            KeyValue[] fieldValues = new KeyValue[3] {
                new KeyValue(CancelDebtsEntity.Field.Status, CancelDebtsStatusCodeTexts.BE_RECTIFIED_CODE),
                new KeyValue(CancelDebtsEntity.Field.RollbackDate, modifyDate),
                new KeyValue(CancelDebtsEntity.Field.ModifyDate, modifyDate)
            };
            Expression where = new Expression(CancelDebtsEntity.Field.SNo, data.SNo)
                .And(CancelDebtsEntity.Field.Reserve2, D00I70ECMarkCodeTexts.NORMAL_CODE)
                .And(CancelDebtsEntity.Field.Status, data.Status);
            int count = 0;
            Result result = factory.UpdateFields<CancelDebtsEntity>(fieldValues, where, out count);
            if (result.IsSuccess)
            {
                if (count == 0)
                {
                    errmsg = "更新(被更正)資料失敗，無資料被更新";
                }
            }
            else
            {
                errmsg = String.Format("更新(被更正)資料失敗，{0}", result.Message);
            }
            return errmsg;
        }

        /// <summary>
        /// 更新更正資料 (Status=已更正或處理失敗、ModifyDate=處理時間)
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="data"></param>
        /// <param name="isOK"></param>
        /// <param name="isDbTimeout"></param>
        /// <param name="modifyDate"></param>
        /// <returns></returns>
        private string UpdateCancelDebtsForRECTIFY(EntityFactory factory, CancelDebtsEntity data, bool isOK, bool isDbTimeout, DateTime modifyDate)
        {
            string errmsg = null;
            KeyValue[] fieldValues = new KeyValue[2];
            if (isOK)
            {
                //更正成功 狀態改為已更正
                fieldValues[0] = new KeyValue(CancelDebtsEntity.Field.Status, CancelDebtsStatusCodeTexts.HAS_RECTIFIED_CODE);
            }
            else if (isDbTimeout)
            {
                //DbTimeout 狀態改為待處理
                fieldValues[0] = new KeyValue(CancelDebtsEntity.Field.Status, CancelDebtsStatusCodeTexts.IS_WAITING_CODE);
            }
            else
            {
                //更正失敗 狀態改為處理失敗
                fieldValues[0] = new KeyValue(CancelDebtsEntity.Field.Status, CancelDebtsStatusCodeTexts.IS_FAILURE_CODE);
            }
            fieldValues[1] = new KeyValue(CancelDebtsEntity.Field.ModifyDate, modifyDate);

            Expression where = new Expression(CancelDebtsEntity.Field.SNo, data.SNo)
                .And(CancelDebtsEntity.Field.Reserve2, D00I70ECMarkCodeTexts.RECTIFY_CODE)
                .And(CancelDebtsEntity.Field.Status, data.Status);
            int count = 0;
            Result result = factory.UpdateFields<CancelDebtsEntity>(fieldValues, where, out count);
            if (result.IsSuccess)
            {
                if (count == 0)
                {
                    errmsg = "更新(更正)資料失敗，無資料被更新";
                }
            }
            else
            {
                errmsg = String.Format("更新(更正)資料失敗，{0}", result.Message);
            }
            return errmsg;
        }
        #endregion
    }
}
