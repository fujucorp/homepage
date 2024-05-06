using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Fuju;
using Fuju.Configuration;
using Fuju.DB;
using Fuju.DB.Data;
using Entities;

namespace ImportSMFile
{
    /// <summary>
    /// 異業代收資料匯入 (C80：匯入中國信託中心媒體檔、C81：匯入統一超商中心媒體檔、C82：匯入全家超商中心媒體檔、C83：匯入OK超商中心媒體檔、C85：匯入萊爾富超商中心媒體檔、C87：匯入財金中心媒體檔)
    /// </summary>
    class Program
    {
        #region FileLog 相關
        private class FileLoger
        {
            #region Property
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
            #endregion

            #region Constructor
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
            #endregion

            #region Method
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
                        using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
                        {
                            if (String.IsNullOrEmpty(msg))
                            {
                                sw.WriteLine(String.Empty);
                            }
                            else
                            {
                                sw.WriteLine("[{0:HH:mm:ss}] {1}", DateTime.Now, msg);
                            }
                        }
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

            public void WriteLog(StringBuilder msg)
            {
                if (!String.IsNullOrEmpty(_LogFileName) && msg != null && msg.Length > 0)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
                        {
                            sw.WriteLine(msg);
                        }
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
            #endregion
        }
        #endregion

        #region 路徑、檔案處理相關
        #region [MDY:20220910] Checkmarx - Improper Resource Shutdown or Release 誤判調整
        //[MEMO] Checkmarx 見不得 new StreamReader 從 function 回傳
        ///// <summary>
        ///// 以 Read 開啟指定的檔案
        ///// </summary>
        ///// <param name="service_id"></param>
        ///// <param name="fileFullName"></param>
        ///// <param name="retryTimes"></param>
        ///// <param name="retrySleep"></param>
        ///// <param name="log"></param>
        ///// <returns></returns>
        //private static StreamReader OpenReadFile(string fileFullName, int retryTimes, int retrySleep, ref StringBuilder log, out string errmsg)
        //{
        //    StreamReader sr = null;
        //    errmsg = null;

        //    for (int timeNo = 0; timeNo <= retryTimes; timeNo++)
        //    {
        //        if (File.Exists(fileFullName))
        //        {
        //            try
        //            {
        //                sr = new StreamReader(fileFullName, Encoding.Default);
        //            }
        //            catch (Exception ex)
        //            {
        //                sr = null;
        //                errmsg = String.Format("第 {0} 次開啟檔案 {1} 失敗，錯誤訊息：{2}", timeNo, fileFullName, ex.Message);
        //                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();
        //            }
        //        }
        //        else
        //        {
        //            sr = null;
        //            errmsg = String.Format("第 {0} 次開啟檔案 {1} 失敗，錯誤訊息：檔案不存在", timeNo, fileFullName);
        //            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();
        //        }

        //        if (sr != null)
        //        {
        //            errmsg = null;
        //            break;
        //        }
        //        else if (retryTimes > 0 && timeNo < retryTimes)
        //        {
        //            Thread.Sleep(retrySleep * 1000 * 60);
        //        }
        //    }

        //    return sr;
        //}
        #endregion

        /// <summary>
        /// 檢查資料夾路徑，如果資料夾不存在則嘗試建立
        /// </summary>
        /// <param name="path">指定檢查的資料夾路徑</param>
        /// <param name="chkWrite">指定是否檢查寫入權限</param>
        /// <returns>傳回錯誤訊息</returns>
        private static string CheckFolder(string path, bool chkWrite = false)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return "未指定資料夾路徑";
            }
            else
            {
                try
                {
                    DirectoryInfo info = new DirectoryInfo(path.Trim());
                    if (!info.Exists)
                    {
                        info.Create();
                    }

                    #region 測試寫入權限
                    if (chkWrite)
                    {
                        try
                        {
                            string fileName = Path.GetTempFileName();
                            File.WriteAllText(fileName, "測試寫入權限...");
                            File.Delete(fileName);
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return null;
            }
        }
        #endregion

        #region [MDY:20190226] EDPData 處理相關方法
        /// <summary>
        /// 產生
        /// </summary>
        /// <param name="header"></param>
        /// <param name="detail"></param>
        /// <param name="stuId"></param>
        /// <param name="stuName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GenEDPData(string edpChannelId, DateTime tranferDate, DateTime edpAccountDate
            , EDPDetailRecord detail, out EDPDataEntity data)
        {
            data = new EDPDataEntity();

            #region header
            data.EDPChannelId = edpChannelId;
            data.TranferDate = tranferDate;
            data.EDPAccountDate = edpAccountDate;
            #endregion

            #region detail
            data.StoreId = detail.store_id;

            data.CancelNo = detail.cancel_no;
            if (data.CancelNo.Length != 12 && data.CancelNo.Length != 14)  //[MEMO] 目前學雜費虛擬帳號有12碼（特殊）與14碼（一般），16碼未使用
            {
                return String.Format("不合法的虛擬帳號 ({0})", data.CancelNo);
            }

            DateTime? receiveDate = DataFormat.ConvertTWDateText(detail.receive_date);
            if (receiveDate.HasValue)
            {
                data.ReceiveDate = receiveDate.Value;
            }
            else
            {
                return String.Format("不合法的客戶繳費日期 ({0})", detail.receive_date);
            }

            decimal amount;
            if (Decimal.TryParse(detail.receive_amount, out amount))
            {
                data.ReceiveAmount = amount;
            }
            else
            {
                return String.Format("不合法的繳費金額 ({0})", detail.receive_amount);
            }

            DateTime? accountDate = DataFormat.ConvertTWDateText(detail.sm_account_date);
            if (accountDate.HasValue)
            {
                data.AccountDate = accountDate.Value;
            }
            else
            {
                return String.Format("不合法的超商入帳日期 ({0})", detail.sm_account_date);
            }

            data.DSource = detail.DSOURCE;

            #region [MDY:20190312] 取消驗證客戶繳費時間，因為土銀中心好像固定給空白
            #region [OLD]
            //TimeSpan? receiveTime = DataFormat.ConvertTimeText(detail.receive_time);
            //if (receiveTime.HasValue)
            //{
            //    data.ReceiveTime = detail.receive_time;
            //    //data.ReceiveTime = DataFormat.GetTime8Text(receiveTime);
            //}
            //else
            //{
            //    return String.Format("不合法的客戶繳費時間 ({0})", detail.receive_time);
            //}
            #endregion

            data.ReceiveTime = detail.receive_time;
            #endregion

            data.RowData = detail.row_data;

            data.ReceiveType = data.CancelNo.Substring(0, 4);  //目前土銀只有 4 碼的商家代號

            data.ReceiveWay = detail.receive_way;
            #endregion

            data.CrtDate = DateTime.Now;

            return null;
        }
        #endregion

        #region Const
        /// <summary>
        /// 最大失敗重試次數 : 8
        /// </summary>
        private const int _MaxRetryTimes = 8;
        /// <summary>
        /// 最大失敗重試間隔(單位分鐘) : 60
        /// </summary>
        private const int _MaxRetrySleep = 60;
        #endregion

        /// <summary>
        /// 參數：要匯入的檔名(必須為第一個參數) service_id=所屬作業類別代碼(C80、C81、C82、C83、C85、C87) [retry_times=重試次數(0~8，預設5)] [retry_sleep=重試間隔，單位分鐘(0~60，預設15)]
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            #region Initial
            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);

            FileLoger fileLoger = new FileLoger(appName);
            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.Skip;  //因為檔案不確定被哪台伺服器下載，所以每台伺服器都要執行，所以不用檢查
            string jobTypeId = null;        //作業類別代碼
            string jobTypeName = null;      //作業類別名稱
            int exitCode = 0;               //最後處理結束代碼
            string exitMsg = null;          //最後處理結束訊息
            #endregion

            int retryTimes = 5;     //re-try 次數 (預設為5次)
            int retrySleep = 15;    //re-try 間隔，單位分鐘 (預設為15分鐘)

            DateTime startTime = DateTime.Now;

            JobCubeHelper jobHelper = new JobCubeHelper();
            int jobNo = 0;
            string jobStamp = null;
            StringBuilder jobLog = new StringBuilder();
            StringBuilder fileLog = new StringBuilder();

            try
            {
                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 開始", DateTime.Now, appName).AppendLine();

                #region 讀取資料檔路徑 Config 參數
                string dataPath = ConfigurationManager.AppSettings.Get("DATA_PATH");    //資料檔路徑
                string bakPath = null;  //備份檔路徑
                if (String.IsNullOrWhiteSpace(dataPath))
                {
                    exitCode = -1;
                    exitMsg = "讀取 Config 中資料來源路徑設定失敗，錯誤訊息：未設定 DATA_PATH 參數值";
                    jobLog.AppendLine(exitMsg);
                    fileLog.AppendLine(exitMsg);
                }
                else
                {
                    dataPath = dataPath.Trim();
                    bakPath = Path.Combine(dataPath, "bak");
                    string errmsg = CheckFolder(bakPath);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -1;
                        exitMsg = String.Format("檢查資料夾 {0} 失敗，錯誤訊息：{1}", bakPath, errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLine(exitMsg);
                    }
                }
                #endregion

                #region 處理參數
                string srcFileName = null;      //匯入檔案的檔名
                string srcFileFullName = null;  //匯入檔案的完整路徑檔名
                string bakFileFullName = null;  //備份檔案的完整路徑檔名
                if (exitCode == 0)
                {
                    fileLog.AppendFormat("命令參數：{0}", (args == null || args.Length == 0) ? null : String.Join(" ", args)).AppendLine();

                    string errmsg = null;

                    if (args != null && args.Length > 0)
                    {
                        #region srcFileName (要匯入的檔名)
                        srcFileName = args[0].Trim();
                        if (srcFileName.IndexOf("=") > -1)
                        {
                            errmsg = "第一個命令參數必須為要匯入的檔名";
                        }
                        else
                        {
                            srcFileFullName = Path.Combine(dataPath, srcFileName);
                            bakFileFullName = Path.Combine(bakPath, String.Format("{0}.{1:yyyyMMddHHmmss}", srcFileName, startTime));
                        }
                        #endregion

                        #region 拆解參數
                        if (String.IsNullOrEmpty(errmsg) && args.Length > 1)
                        {
                            for (int idx = 1; idx < args.Length; idx++)
                            {
                                string arg = args[idx];
                                string[] kvs = arg.Split('=');
                                if (kvs.Length == 2)
                                {
                                    string key = kvs[0].Trim().ToLower();
                                    string value = kvs[1].Trim();
                                    switch (key)
                                    {
                                        case "service_id":
                                            #region jobTypeId (所屬作業類別代碼)
                                            {
                                                jobTypeId = value;
                                                jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);
                                                if (String.IsNullOrEmpty(jobTypeName))
                                                {
                                                    errmsg = "service_id 參數值不是正確的作業類別代碼";
                                                }
                                            }
                                            break;
                                            #endregion
                                        case "retry_times":
                                            #region retryTimes (re-try 次數)
                                            if (!String.IsNullOrEmpty(value))
                                            {
                                                int times = 0;
                                                if (int.TryParse(value, out times))
                                                {
                                                    if (times <= 0)
                                                    {
                                                        retryTimes = 0;
                                                    }
                                                    else if (times >= _MaxRetryTimes)
                                                    {
                                                        retryTimes = _MaxRetryTimes;
                                                    }
                                                    else
                                                    {
                                                        retryTimes = times;
                                                    }
                                                }
                                            }
                                            break;
                                            #endregion
                                        case "retry_sleep":
                                            #region retrySleep (re-try 間隔，單位分鐘)
                                            if (!String.IsNullOrEmpty(value))
                                            {
                                                int minutes = 0;
                                                if (int.TryParse(value, out minutes))
                                                {
                                                    if (minutes <= 1)
                                                    {
                                                        retrySleep = 1;
                                                    }
                                                    else if (minutes >= _MaxRetrySleep)
                                                    {
                                                        retrySleep = _MaxRetrySleep;
                                                    }
                                                    else
                                                    {
                                                        retrySleep = minutes;
                                                    }
                                                }
                                            }
                                            break;
                                            #endregion
                                        default:
                                            errmsg = String.Format("不支援 {0} 命令參數", arg);
                                            break;
                                    }
                                }
                                else
                                {
                                    errmsg = String.Format("不支援 {0} 命令參數", arg);
                                }
                                if (!String.IsNullOrEmpty(errmsg))
                                {
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        errmsg = "未指定命令參數";
                    }

                    #region 檢查必要參數
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        List<string> lost = new List<string>();
                        if (String.IsNullOrEmpty(srcFileName))
                        {
                            lost.Add("要匯入的檔名");
                        }
                        if (String.IsNullOrEmpty(jobTypeId))
                        {
                            lost.Add("service_id");
                        }
                        if (lost.Count > 0)
                        {
                            errmsg = String.Format("缺少 {0} 參數", String.Join(", ", lost));
                        }
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -1;
                        exitMsg = String.Format("參數錯誤，錯誤訊息：{0}", errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLine(exitMsg);

                        if (args == null || args.Length == 0)
                        {
                            fileLog.AppendLine("參數語法：要匯入的檔名(必須為第一個參數) service_id=所屬作業類別代碼(C80、C81、C82、C83、C85、C87) [retry_times=重試次數(0~8，預設5)] [retry_sleep=重試間隔，單位分鐘(0~60，預設15)]");
                        }
                    }
                }
                #endregion

                #region 新增處理中的 Job
                if (exitCode == 0)
                {
                    JobcubeEntity job = new JobcubeEntity();
                    job.Jtypeid = jobTypeId;
                    job.Jparam = String.Join(" ", args);
                    Result result = jobHelper.InsertProcessJob(ref job, jobCheckMode);
                    if (result.IsSuccess)
                    {
                        jobNo = job.Jno;
                        jobStamp = job.Memo;
                    }
                    else
                    {
                        exitCode = -2;

                        #region [MDY:20190226] 調整 LOG 訊息
                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 新增作業處理中紀錄失敗，錯誤訊息：{1}", DateTime.Now, result.Message).AppendLine();
                        #endregion
                    }
                }
                #endregion

                #region [MDY:20190226] 作業處理重寫
                if (exitCode == 0)
                {
                    fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 作業處理開始", DateTime.Now, jobTypeName).AppendLine();

                    StreamReader sr = null;
                    try
                    {
                        #region 開啟檔案
                        {
                            string errmsg = null;

                            #region [MDY:20220910] Checkmarx - Improper Resource Shutdown or Release 誤判調整
                            #region [OLD] Checkmarx 見不得 new StreamReader 從 function 回傳
                            //sr = OpenReadFile(srcFileFullName, retryTimes, retrySleep, ref fileLog, out errmsg);
                            #endregion

                            {
                                for (int timeNo = 0; timeNo <= retryTimes; timeNo++)
                                {
                                    if (File.Exists(srcFileFullName))
                                    {
                                        sr = new StreamReader(srcFileFullName, Encoding.Default);
                                        break;
                                    }
                                    else
                                    {
                                        sr = null;
                                        errmsg = String.Format("第 {0} 次開啟檔案 {1} 失敗，錯誤訊息：檔案不存在", timeNo, srcFileFullName);
                                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();

                                        if (retryTimes > 0 && timeNo < retryTimes)
                                        {
                                            Thread.Sleep(retrySleep * 1000 * 60);
                                        }
                                    }
                                }
                            }
                            #endregion

                            if (sr == null)
                            {
                                exitCode = -3;
                                exitMsg = String.Format("開啟檔案 {0} 失敗，錯誤訊息：{1}", srcFileFullName, errmsg);
                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                            }
                        }
                        #endregion

                        bool toEDPData = false;  //註記是否要處理 EDPData
                        int kind1DataCount = 0;  //學雜費的資料筆數
                        int kind2DataCount = 0;  //代收各項費用的資料筆數
                        int cancelOKCount = 0;   //預銷處理 OK 筆數
                        int edpDataOKCount = 0;  //新增 EDPData OK 筆數
                        List<EDPDataEntity> datas = new List<EDPDataEntity>();

                        #region [MDY:20190226] 處理檔案內容，將檔案內容以 EDPData 承載，有誤就整檔不處理
                        if (exitCode == 0)
                        {
                            try
                            {
                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始處理檔案 {1} ", DateTime.Now, srcFileFullName).AppendLine();

                                decimal dataTotalAmount = 0;  //明細總金額
                                decimal tailTotalAmount = 0;  //檔尾的總金額資料
                                int tailRecordCount = 0;      //檔尾的總筆數資料
                                EDPHelper edpHelper = new EDPHelper();
                                EDPHeaderRecord header = null;
                                EDPTailRecord tail = null;

                                #region 逐筆處理，有誤就整檔不處理
                                {
                                    DateTime? tranferDate = null;
                                    DateTime? edpAccountDate = null;

                                    string line = null;
                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        if (EDPHelper.isHeaderRecord(line))
                                        {
                                            #region 檔頭
                                            if (header != null)
                                            {
                                                exitCode = -3;
                                                exitMsg = "發現檔案資料錯誤，錯誤訊息：有兩個檔頭資料";
                                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            }
                                            else
                                            {
                                                string errmsg = null;
                                                if (!EDPHelper.parseHeader(line, out header, out errmsg))
                                                {
                                                    exitCode = -3;
                                                    exitMsg = String.Format("檔頭資料解析失敗，錯誤訊息：{0}", errmsg);
                                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                    fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                }
                                                else
                                                {
                                                    #region 檢查 channel_id
                                                    if (exitCode == 0)
                                                    {
                                                        switch (header.channel_id.Trim())
                                                        {
                                                            case "CNTRUST": //中國信託
                                                                toEDPData = true;  //目前只有中國信託才要寫入異業代收款檔
                                                                break;
                                                            case "CREDIT":  //財金
                                                            case "7111111": //統一
                                                            case "TFM":     //全家
                                                            case "OKCVS":   //OK
                                                            case "HILIFE":  //萊爾富
                                                                break;
                                                            default:
                                                                exitCode = -3;
                                                                exitMsg = String.Format("發現檔頭資料錯誤，錯誤訊息：不合法的代收管道代碼 ({0})", header.channel_id);
                                                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                                break;
                                                        }
                                                    }
                                                    #endregion

                                                    #region 檢查 tranfer_date
                                                    if (exitCode == 0)
                                                    {
                                                        tranferDate = DataFormat.ConvertTWDateText(header.tranfer_date);
                                                        if (!tranferDate.HasValue)
                                                        {
                                                            exitCode = -3;
                                                            exitMsg = String.Format("發現檔頭資料錯誤，錯誤訊息：不合法的傳輸日期 ({0})", header.tranfer_date);
                                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                            fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                        }
                                                    }
                                                    #endregion

                                                    #region 檢查 sm_account_date
                                                    if (exitCode == 0)
                                                    {
                                                        edpAccountDate = DataFormat.ConvertTWDateText(header.sm_account_date);
                                                        if (!edpAccountDate.HasValue)
                                                        {
                                                            exitCode = -3;
                                                            exitMsg = String.Format("發現檔頭資料錯誤，錯誤訊息：不合法的入帳日期 ({0})", header.sm_account_date);
                                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                            fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (EDPHelper.isDetailRecord(line))
                                        {
                                            #region 明細
                                            if (header == null)
                                            {
                                                exitCode = -3;
                                                exitMsg = "發現檔案資料錯誤，錯誤訊息：在檔頭資料前已出現明細資料";
                                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            }
                                            else if (tail != null)
                                            {
                                                exitCode = -3;
                                                exitMsg = "發現檔案資料錯誤，錯誤訊息：在檔尾資料後又出現明細資料";
                                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            }
                                            else
                                            {
                                                string errmsg = null;
                                                EDPDetailRecord detail = null;
                                                if (!EDPHelper.parseDetail(line, out detail, out errmsg))
                                                {
                                                    exitCode = -3;
                                                    exitMsg = String.Format("明細資料解析失敗，錯誤訊息：{0}", errmsg);
                                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                    fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                }
                                                else
                                                {
                                                    EDPDataEntity data = null;
                                                    errmsg = GenEDPData(header.channel_id, tranferDate.Value, edpAccountDate.Value, detail, out data);
                                                    if (String.IsNullOrEmpty(errmsg))
                                                    {
                                                        data.RowData = line;
                                                        datas.Add(data);
                                                        dataTotalAmount += data.ReceiveAmount;
                                                    }
                                                    else
                                                    {
                                                        exitCode = -3;
                                                        exitMsg = String.Format("發現明細資料錯誤，錯誤訊息：{0}", errmsg);
                                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (EDPHelper.isTailRecord(line))
                                        {
                                            #region 檔尾
                                            if (header == null)
                                            {
                                                exitCode = -3;
                                                exitMsg = "發現檔案資料錯誤，錯誤訊息：在檔頭資料前已出現檔尾資料";
                                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            }
                                            else if (tail != null)
                                            {
                                                exitCode = -3;
                                                exitMsg = "發現檔案資料錯誤，錯誤訊息：有兩個檔尾資料";
                                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            }
                                            else
                                            {
                                                string errmsg = null;
                                                if (!EDPHelper.parseTail(line, out tail, out errmsg))
                                                {
                                                    exitCode = -3;
                                                    exitMsg = String.Format("檔尾資料解析失敗，錯誤訊息：{0}", errmsg);
                                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                    fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                }
                                                else
                                                {
                                                    #region 檢查 total_amount
                                                    if (exitCode == 0)
                                                    {
                                                        decimal amount = 0;
                                                        if (Decimal.TryParse(tail.total_amount.Trim(), out amount) && amount >= 0)
                                                        {
                                                            tailTotalAmount = amount;
                                                        }
                                                        else
                                                        {
                                                            exitCode = -3;
                                                            exitMsg = String.Format("發現檔尾資料錯誤，錯誤訊息：不合法的代收成功金額 ({0})", tail.total_amount);
                                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                            fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                        }
                                                    }
                                                    #endregion

                                                    #region 檢查 total_records
                                                    if (exitCode == 0)
                                                    {
                                                        int count = 0;
                                                        if (Int32.TryParse(tail.total_records.Trim(), out count) && count >= 0)
                                                        {
                                                            tailRecordCount = count;
                                                        }
                                                        else
                                                        {
                                                            exitCode = -3;
                                                            exitMsg = String.Format("發現檔尾資料錯誤，錯誤訊息：不合法的代收成功筆數 ({0})", tail.total_records);
                                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                            fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (!String.IsNullOrWhiteSpace(line))
                                        {
                                            #region 非空白行的無效資料
                                            exitCode = -3;
                                            exitMsg = String.Format("發現檔案資料錯誤，錯誤訊息：包含非空白行的無效資料 ({0})", line);
                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            #endregion
                                        }

                                        if (exitCode != 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                                #endregion

                                #region 檢查總筆數，總金額
                                if (exitCode == 0)
                                {
                                    if (header == null)
                                    {
                                        exitCode = -3;
                                        exitMsg = "發現檔案資料錯誤，錯誤訊息：找不到檔頭資料";
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                    }
                                    else if (tail == null)
                                    {
                                        exitCode = -3;
                                        exitMsg = "發現檔案資料錯誤，錯誤訊息：找不到檔尾資料";
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                    }
                                    else if (tailTotalAmount != dataTotalAmount)
                                    {
                                        exitCode = -3;
                                        exitMsg = "發現檔案資料錯誤，錯誤訊息：總金額不符";
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                    }
                                    else if (tailRecordCount != datas.Count)
                                    {
                                        exitCode = -3;
                                        exitMsg = "發現檔案資料錯誤，錯誤訊息：總筆數不符";
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                    }
                                    else
                                    {
                                        if (datas.Count == 0)
                                        {
                                            //沒有明細資料不算錯
                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, "此檔案無明細資料").AppendLine();
                                            fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, "此檔案無明細資料").AppendLine();
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                exitCode = -8;
                                exitMsg = String.Format("處理檔案發生例外，錯誤訊息：{0}", ex.Message);
                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                            }
                            finally
                            {
                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束處理檔案 {1} ", DateTime.Now, srcFileFullName).AppendLine();
                            }
                        }
                        #endregion

                        #region [MDY:20190226] 處理資料 (預銷處理、新增 EDPData 處理)
                        if (exitCode == 0 && datas.Count > 0)
                        {
                            try
                            {
                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始資料處理 ", DateTime.Now).AppendLine();

                                CancelHelper helper = new CancelHelper();

                                #region [MDY:20190226] 取得代收各項費用的商家代號，失敗了後面不處理
                                List<string> upctcbReceiveTypes = null;
                                {
                                    if (!helper.GetUPCTCBReceiveTypes(out upctcbReceiveTypes))
                                    {
                                        exitCode = -3;
                                        exitMsg = String.Format("讀取代收各項費用種類的商家代號失敗，錯誤訊息：{0}", helper.err_msg);
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                    }
                                }
                                #endregion

                                if (exitCode == 0)
                                {
                                    #region [MDY:20190226] 查詢 QueueCTCBEntity 的 OrderBy
                                    KeyValueList<OrderByEnum> upctcbOrderBys = new KeyValueList<OrderByEnum>(1);
                                    upctcbOrderBys.Add(QueueCTCBEntity.Field.CrtDate, OrderByEnum.Desc);
                                    #endregion

                                    using (EntityFactory factory = new EntityFactory())
                                    {
                                        foreach (EDPDataEntity edpData in datas)
                                        {
                                            #region 逐筆處理 (處理失敗不中斷)
                                            try
                                            {
                                                bool isUPCTCBReceiveType = upctcbReceiveTypes.Contains(edpData.ReceiveType);
                                                if (isUPCTCBReceiveType)
                                                {
                                                    #region 代收各項費用的商家代號
                                                    kind2DataCount++;

                                                    #region 只寫 LOG 不需要做預銷處理
                                                    {
                                                        string msg = String.Format("虛擬帳號 {0} 不需要做預銷處理", edpData.CancelNo);
                                                        //jobLog.AppendLine(msg);
                                                        fileLog.AppendLine(msg);
                                                    }
                                                    #endregion

                                                    #region 處理 EDPData
                                                    if (toEDPData)
                                                    {
                                                        #region 找出最新且相符的 QueueCTCBEntity 的學生學號與學生姓名
                                                        {
                                                            QueueCTCBEntity ctcb = null;
                                                            Expression where = new Expression(QueueCTCBEntity.Field.ReceiveType, edpData.ReceiveType)
                                                                .And(QueueCTCBEntity.Field.CancelNo, edpData.CancelNo);
                                                            Result result = factory.SelectFirst<QueueCTCBEntity>(where, upctcbOrderBys, out ctcb);
                                                            if (result.IsSuccess)
                                                            {
                                                                if (ctcb != null)
                                                                {
                                                                    edpData.StuId = ctcb.StuId;
                                                                    edpData.StuName = ctcb.StuName;
                                                                }
                                                                else
                                                                {
                                                                    #region 找不到該虛擬帳號的繳費單資料，寫 LOG
                                                                    string errmsg = String.Format("查無 {0} 上傳中國信託繳費單資料", edpData.CancelNo);
                                                                    //jobLog.AppendLine(errmsg);
                                                                    fileLog.AppendLine(errmsg);
                                                                    #endregion
                                                                }
                                                            }
                                                            else
                                                            {
                                                                #region 取資料失敗，寫 LOG
                                                                string errmsg = null;
                                                                if (result.Exception != null)
                                                                {
                                                                    errmsg = String.Format("讀取 {0} 上傳中國信託繳費單資料 失敗，例外訊息：{1}", edpData.CancelNo, result.Exception.Message);
                                                                }
                                                                else
                                                                {
                                                                    errmsg = String.Format("讀取 {0} 上傳中國信託繳費單資料 失敗，錯誤訊息：{1}", edpData.CancelNo, result.Message);
                                                                }
                                                                jobLog.AppendLine(errmsg);
                                                                fileLog.AppendLine(errmsg);
                                                                #endregion
                                                            }
                                                        }
                                                        #endregion

                                                        #region 新增 EDPDataEntity 資料
                                                        {
                                                            int count = 0;
                                                            Result result = factory.Insert(edpData, out count);
                                                            if (result.IsSuccess)
                                                            {
                                                                edpDataOKCount++;
                                                            }
                                                            else
                                                            {
                                                                string errmsg = null;
                                                                if (result.Exception != null)
                                                                {
                                                                    errmsg = String.Format("新增 {0} 異業代收款檔資料 失敗，例外訊息：{1}", edpData.CancelNo, result.Exception.Message);
                                                                }
                                                                else
                                                                {
                                                                    errmsg = String.Format("新增 {0} 異業代收款檔資料 失敗，錯誤訊息：{1}", edpData.CancelNo, result.Message);
                                                                }
                                                                jobLog.AppendLine(errmsg);
                                                                fileLog.AppendLine(errmsg);
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                    #endregion
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region 學雜費的商家代號
                                                    kind1DataCount++;

                                                    #region 預銷處理 (已繳待銷處理)
                                                    {
                                                        StudentReceiveView7[] studentReceives = null;
                                                        Result result = helper.GetStudentReceiveView7(factory, edpData.ReceiveType, edpData.CancelNo, out studentReceives);
                                                        if (result.IsSuccess)
                                                        {
                                                            #region 取資料成功
                                                            if (studentReceives != null && studentReceives.Length > 0)
                                                            {
                                                                #region 逐資料比對
                                                                bool isCanceled = false;
                                                                bool isPreCanceled = false;
                                                                string errmsg = null;
                                                                foreach (StudentReceiveView7 studentReceive in studentReceives)
                                                                {
                                                                    if (string.IsNullOrWhiteSpace(studentReceive.ReceiveDate)
                                                                        && string.IsNullOrWhiteSpace(studentReceive.ReceiveWay)
                                                                        && string.IsNullOrWhiteSpace(studentReceive.AccountDate))
                                                                    {
                                                                        //因為土銀的超商管道都是外加或學校負擔，所以應繳金額不會因管道而不同，直接用 ReceiveAmount 比對即可
                                                                        if (studentReceive.ReceiveAmount.HasValue && studentReceive.ReceiveAmount.Value == edpData.ReceiveAmount)
                                                                        {
                                                                            edpData.StuId = studentReceive.StuId;
                                                                            edpData.StuName = studentReceive.StuName;

                                                                            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 財金信用卡異業代收 且 繳費單啟用國際信用卡繳費，代收管道改用 ChannelHelper.FISC_NC
                                                                            if (ChannelHelper.FISC.Equals(edpData.ReceiveWay) && "Y".Equals(studentReceive.NCCardFlag))
                                                                            {
                                                                                result = helper.UpdateStudentReceive(factory, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.DepId, studentReceive.ReceiveId, studentReceive.StuId, studentReceive.OldSeq, studentReceive.CancelNo
                                                                                    , Common.GetTWDate7(edpData.ReceiveDate), edpData.ReceiveTime, ChannelHelper.FISC_NC);
                                                                            }
                                                                            else
                                                                            {
                                                                                result = helper.UpdateStudentReceive(factory, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.DepId, studentReceive.ReceiveId, studentReceive.StuId, studentReceive.OldSeq, studentReceive.CancelNo
                                                                                    , Common.GetTWDate7(edpData.ReceiveDate), edpData.ReceiveTime, edpData.ReceiveWay);
                                                                            }
                                                                            #endregion

                                                                            if (result.IsSuccess)
                                                                            {
                                                                                isCanceled = true;
                                                                                cancelOKCount++;
                                                                            }
                                                                            else
                                                                            {
                                                                                #region 更新預銷相關欄位失敗，寫 LOG
                                                                                if (result.Exception != null)
                                                                                {
                                                                                    errmsg = String.Format("虛擬帳號 {0} 預銷處理 失敗，例外訊息：更新預銷相關欄發生例外，{1}", edpData.CancelNo, result.Exception.Message);
                                                                                }
                                                                                else
                                                                                {
                                                                                    errmsg = String.Format("虛擬帳號 {0} 預銷處理 失敗，錯誤訊息：更新預銷相關欄失敗，{1}", edpData.CancelNo, result.Message);
                                                                                }
                                                                                jobLog.AppendLine(errmsg);
                                                                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();
                                                                                #endregion
                                                                            }
                                                                            break;
                                                                        }
                                                                    }
                                                                    #region [MDY:20191214] (調整) 中信與財金回覆的已繳代銷，表示是有收到平台回覆訊息，另外寫 LOG
                                                                    else if (string.IsNullOrWhiteSpace(studentReceive.AccountDate)
                                                                        && (ChannelHelper.FISC.Equals(edpData.ReceiveWay) || ChannelHelper.CTCB.Equals(edpData.ReceiveWay)))
                                                                    {
                                                                        //因為土銀的超商管道都是外加或學校負擔，所以應繳金額不會因管道而不同，直接用 ReceiveAmount 比對即可
                                                                        if ((studentReceive.ReceiveWay == edpData.ReceiveWay || (ChannelHelper.FISC.Equals(edpData.ReceiveWay) && ChannelHelper.FISC_NC.Equals(studentReceive.ReceiveWay)))
                                                                            && (Common.GetTWDate7(edpData.ReceiveDate) == studentReceive.ReceiveDate)
                                                                            && (studentReceive.ReceiveAmount.HasValue && studentReceive.ReceiveAmount.Value == edpData.ReceiveAmount))
                                                                        {
                                                                            isPreCanceled = true;
                                                                        }
                                                                    }
                                                                    #endregion
                                                                }

                                                                #region [MDY:20191214] (調整) 中信與財金回覆的已繳代銷，另外寫 LOG
                                                                if (!isCanceled && isPreCanceled)
                                                                {
                                                                    isCanceled = true;
                                                                    string msg = String.Format("虛擬帳號 {0} 已由平台交易結果做過預銷處理", edpData.CancelNo);
                                                                    jobLog.AppendLine(msg);
                                                                    fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, msg).AppendLine();
                                                                }
                                                                #endregion

                                                                if (!isCanceled)
                                                                {
                                                                    #region [MDY:20191214] (調整) 沒銷帳又沒有錯誤，表示找不到可以預銷的繳費單資料
                                                                    if (!String.IsNullOrEmpty(errmsg))
                                                                    {
                                                                        errmsg = String.Format("虛擬帳號 ({0}) 預銷處理失敗，失敗息：找不到符合的繳費單資料", edpData.CancelNo);
                                                                        jobLog.AppendLine(errmsg);
                                                                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();
                                                                    }
                                                                    #endregion
                                                                }
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                #region 找不到該虛擬帳號的繳費單資料，寫 LOG
                                                                string errmsg = String.Format("虛擬帳號 ({0}) 預銷處理失敗，錯誤訊息：查無該虛擬帳號的繳費單資料", edpData.CancelNo);
                                                                jobLog.AppendLine(errmsg);
                                                                fileLog.AppendLine(errmsg);
                                                                #endregion
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            #region 取資料失敗，寫 LOG
                                                            string errmsg = null;
                                                            if (result.Exception != null)
                                                            {
                                                                errmsg = String.Format("虛擬帳號 {0} 預銷處理 失敗，例外訊息：讀取學生繳費資料發生例外，{1}", edpData.CancelNo, result.Exception.Message);
                                                            }
                                                            else
                                                            {
                                                                errmsg = String.Format("虛擬帳號 {0} 預銷處理 失敗，錯誤訊息：讀取學生繳費資料失敗，{1}", edpData.CancelNo, result.Message);
                                                            }
                                                            jobLog.AppendLine(errmsg);
                                                            fileLog.AppendLine(errmsg);
                                                            #endregion
                                                        }
                                                    }
                                                    #endregion

                                                    #region 處理 EDPData
                                                    if (toEDPData)
                                                    {
                                                        #region 新增 EDPDataEntity 資料
                                                        {
                                                            int count = 0;
                                                            Result result = factory.Insert(edpData, out count);
                                                            if (result.IsSuccess)
                                                            {
                                                                edpDataOKCount++;
                                                            }
                                                            else
                                                            {
                                                                string errmsg = null;
                                                                if (result.Exception != null)
                                                                {
                                                                    errmsg = String.Format("新增 {0} 異業代收款檔資料 失敗，例外訊息：{1}", edpData.CancelNo, result.Exception.Message);
                                                                }
                                                                else
                                                                {
                                                                    errmsg = String.Format("新增 {0} 異業代收款檔資料 失敗，錯誤訊息：{1}", edpData.CancelNo, result.Message);
                                                                }
                                                                jobLog.AppendLine(errmsg);
                                                                fileLog.AppendLine(errmsg);
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                    #endregion
                                                    #endregion
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                string errmsg = String.Format("虛擬帳號 {0} 資料處理發生例外，例外訊息：{1}", edpData.CancelNo, ex.Message);
                                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                exitCode = -7;
                                exitMsg = String.Format("處理資料發生例外，錯誤訊息：{0}", ex.Message);
                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                            }
                            finally
                            {
                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束資料處理 ", DateTime.Now).AppendLine();

                                if (exitCode == 0)
                                {
                                    if (toEDPData)
                                    {
                                        exitMsg = String.Format("共 {0} 筆資料，學雜費資料 {1} 筆，代收各項費用 {2} 筆，預銷成功 {3} 筆，新增異業代收款檔資料成功 {4} 筆 ", datas.Count, kind1DataCount, kind2DataCount, cancelOKCount, edpDataOKCount);
                                    }
                                    else
                                    {
                                        exitMsg = String.Format("共 {0} 筆資料，學雜費資料 {1} 筆，代收各項費用 {2} 筆，預銷成功 {3} 筆，無須新增異業代收款檔資料 ", datas.Count, kind1DataCount, kind2DataCount, cancelOKCount);
                                    }
                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                    fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                }
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        exitCode = -8;
                        exitMsg = String.Format("作業處理發生例外，參數：{0}，錯誤訊息：{1}", String.Join(" ", args), ex.Message);
                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                    }
                    finally
                    {
                        if (sr != null)
                        {
                            sr.Close();
                            sr.Dispose();
                            sr = null;
                        }

                        #region 備份檔案
                        if (!String.IsNullOrEmpty(srcFileFullName) && !String.IsNullOrEmpty(bakFileFullName))
                        {
                            try
                            {
                                File.Move(srcFileFullName, bakFileFullName);
                            }
                            catch (Exception ex)
                            {
                                string errmsg = String.Format("[{0:yyyy/MM/dd HH:mm:ss}] 移動 {1} 檔案至 {2} 發生例外，錯誤訊息：{3}", DateTime.Now, srcFileFullName, bakFileFullName, ex.Message);
                                fileLog.AppendLine(errmsg);
                                jobLog.AppendLine(errmsg);
                            }
                        }
                        #endregion

                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 作業處理結束", DateTime.Now, jobTypeName).AppendLine();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;
                exitMsg = String.Format("作業處理發生例外，參數：{0}，錯誤訊息：{1}", String.Join(" ", args), ex.Message);
                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 發生例外，參數：{1}，錯誤訊息：{2}", DateTime.Now, String.Join(" ", args), ex.Message).AppendLine();
            }
            finally
            {
                #region 更新 Job
                if (jobNo > 0)
                {
                    string jobResultId = null;
                    if (exitCode == 0)
                    {
                        jobResultId = JobCubeResultCodeTexts.SUCCESS;
                    }
                    else
                    {
                        jobResultId = JobCubeResultCodeTexts.FAILURE;
                    }

                    Result result = jobHelper.UpdateProcessJobToFinsh(jobNo, jobStamp, jobResultId, exitMsg, jobLog.ToString());
                    if (!result.IsSuccess)
                    {
                        #region [MDY:20190226] 調整 LOG 訊息
                        fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 更新作業處理完成失敗，參數：jobNo={1} jobStamp={2} jobResultId={3} exitMsg={4}，錯誤訊息：{5}", DateTime.Now, jobNo, jobStamp, jobResultId, exitMsg, result.Message).AppendLine();
                        #endregion
                    }
                }
                jobHelper.Dispose();
                jobHelper = null;
                #endregion

                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 結束", DateTime.Now, appName).AppendLine();

                fileLoger.WriteLog(fileLog);
            }

            System.Environment.Exit(exitCode);
        }
    }
}
