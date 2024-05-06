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
using Fuju.DB;
using Fuju.DB.Data;

using Entities;

namespace GenCancelData
{
    /// <summary>
    /// 產生學校銷帳檔 (SC1)
    /// </summary>
    class Program
    {
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
        }
        #endregion

        #region
        /// <summary>
        /// 檢查或建立指定路徑資料夾
        /// </summary>
        /// <param name="path">指定的資料夾路徑</param>
        /// <param name="fgClear">指定是否清空資料夾，預設否</param>
        /// <returns>傳回錯誤訊息</returns>
        private static string CheckOrCreateFolder(string path, bool fgClear = false)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return "未指定資料夾路徑";
            }
            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(path);
                if (dinfo.Exists)
                {
                    #region 資料夾存
                    if (fgClear)
                    {
                        try
                        {
                            FileInfo[] finfos = dinfo.GetFiles();
                            foreach (FileInfo finfo in finfos)
                            {
                                finfo.Delete();
                            }
                        }
                        catch (Exception ex)
                        {
                            return String.Format("無法清空 {0} 資料夾，錯誤訊息：{1}", path, ex.Message);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 資料夾不存在就建立
                    try
                    {
                        dinfo.Create();
                    }
                    catch (Exception ex)
                    {
                        return String.Format("無法建立 {0} 資料夾，錯誤訊息：{1}", path, ex.Message);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return String.Format("不正確的資料夾路徑 ({0})，錯誤訊息：{1}", path, ex.Message);
            }
            return null;
        }

        //private static string getDate7(DateTime d)
        //{
        //    return (d.Year - 1911).ToString("000") + d.ToString("MMdd");
        //}

        private static string getDate6(DateTime d)
        {
            return (d.Year - 1911).ToString("000").Substring(1, 2) + d.ToString("MMdd");
        }

        //private static bool checkFolder(string folder, out string msg)
        //{
        //    bool rc = false;
        //    msg = "";
        //    try
        //    {
        //        DirectoryInfo di = new DirectoryInfo(folder);
        //        if (!di.Exists)
        //        {
        //            di.Create();
        //        }
        //        rc = true;
        //    }
        //    catch(Exception ex)
        //    {
        //        msg = ex.Message;
        //    }
        //    return rc;
        //}


        #region [MDY:20160126] 沒用到 Mark
        //private static string getKey(string receive_type)
        //{
        //    return receive_type + getDate7(DateTime.Now).Substring(1, 6) + DateTime.Now.ToString("HHmmss");
        //}

        //private static bool isDate(string date,out string date7)
        //{
        //    bool rc = false;
        //    date7 = "";
        //    string date8 = "";

        //    Int32 d = 0;
        //    date = date.Replace("/", String.Empty).Replace("-", String.Empty);
        //    try
        //    {
        //        d = Int32.Parse(date);
        //    }
        //    catch (Exception ex)
        //    {
        //        return rc;
        //    }

        //    if (date.Trim().Length == 6)//視為民國年
        //    {
        //        date8 = Convert.ToString(d + 19110000);
        //    }
        //    else if (date.Trim().Length == 7)//視為民國年
        //    {
        //        date8 = Convert.ToString(d + 19110000);
        //    }
        //    else if (date.Trim().Length == 8)//視為西元年
        //    {
        //        date8 = date;
        //    }
        //    else
        //    {
        //        return rc;
        //    }

        //    DateTime dd;
        //    try
        //    {
        //        dd = DateTime.Parse(date8);
        //        date7 = getDate7(dd);
        //    }
        //    catch (Exception ex)
        //    {
        //        return rc;
        //    }

        //    rc = true;
        //    return rc;
        //}
        #endregion

        //private static bool initPath(string path,out string log)
        //{
        //    bool rc = false;
        //    log = "";
        //    StringBuilder logs = new System.Text.StringBuilder();

        //    System.IO.DirectoryInfo di = new DirectoryInfo(path);
        //    if(di.Exists)
        //    {
        //        #region 資料夾存在就清空
        //        System.IO.FileInfo[] fis = di.GetFiles();
        //        if(fis!=null && fis.Length>0)
        //        {
        //            foreach(System.IO.FileInfo fi in fis)
        //            {
        //                try
        //                {
        //                    fi.Delete();
        //                }
        //                catch(Exception ex)
        //                {
        //                    log = string.Format("刪除檔案{0}發生錯誤，錯誤訊息={1}", fi.FullName, ex.Message);
        //                    logs.AppendLine(log);
        //                }
        //            }
        //            if(logs!=null && logs.Length>0)
        //            {
        //                log = logs.ToString();
        //            }
        //            //rc = true; //因為檔名是以時間命名，所以不會重複，刪不掉也沒關係
        //        }
        //        #endregion
        //        rc = true; //因為檔名是以時間命名，所以不會重複，刪不掉也沒關係
        //    }
        //    else
        //    {
        //        #region 資料夾不存在就建立
        //        try
        //        {
        //            di.Create();
        //            rc = true;
        //        }
        //        catch(Exception ex)
        //        {
        //            log = string.Format("建立資料夾{0}發生錯誤，錯誤訊息={1}", path, ex.Message);
        //        }
        //        #endregion
        //    }

        //    return rc;
        //}
        #endregion

        #region [Old] 不提供 retry，因為此程式會執行多次
        ///// <summary>
        ///// 最大失敗重試次數 : 8
        ///// </summary>
        //private static int _MaxRetryTimes = 8;
        ///// <summary>
        ///// 最大失敗重試間隔(單位分鐘) : 60
        ///// </summary>
        //private static int _MaxRetrySleep = 60;
        #endregion

        /// <summary>
        /// 參數：service_id=所屬作業類別代碼(SC1、SC1B) receive_date=要處理的代收日(TODAY, YESTERDAY 或 yyyyMMdd)
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            #region Initial
            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);

            FileLoger fileLog = new FileLoger(appName);
            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = null;        //作業類別代碼
            string jobTypeName = null;      //作業類別名稱
            int exitCode = 0;
            string exitMsg = null;
            #endregion

            StringBuilder log = new StringBuilder();

            #region [Old] 不提供 retry，因為此程式會執行多次
            //int retryTimes = 5;     //re-try 次數 (預設為5次)
            //int retrySleep = 5;     //re-try 間隔，單位分鐘 (預設為5分鐘)
            #endregion

            DateTime startTime = DateTime.Now;

            JobCubeHelper jobHelper = new JobCubeHelper();
            int jobNo = 0;
            string jobStamp = null;
            StringBuilder jobLog = new StringBuilder();

            try
            {
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 開始", DateTime.Now, appName).AppendLine();

                #region 處理參數
                DateTime? receiveDate = null;
                if (exitCode == 0)
                {
                    log.AppendFormat("命令參數：{0}", (args == null || args.Length == 0) ? null : String.Join(" ", args)).AppendLine();

                    string errmsg = null;

                    if (args != null && args.Length > 0)
                    {
                        #region 拆解參數
                        foreach (string arg in args)
                        {
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
                                            switch (jobTypeId)
                                            {
                                                case JobCubeTypeCodeTexts.SC1:
                                                case JobCubeTypeCodeTexts.SC1B:
                                                    jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);
                                                    break;
                                                default:
                                                    errmsg = "service_id 參數值不是正確的作業類別代碼 (SC1、SC1B)";
                                                    break;
                                            }
                                        }
                                        break;
                                    #endregion
                                    case "receive_date":
                                        #region receiveDate (要處理的代收日)
                                        {
                                            switch (value.ToUpper())
                                            {
                                                case "TODAY":
                                                    receiveDate = startTime.Date;
                                                    break;
                                                case "YESTERDAY":
                                                    receiveDate = startTime.Date.AddDays(-1);
                                                    break;
                                                default:
                                                    receiveDate = DataFormat.ConvertDateText(value);
                                                    if (receiveDate == null)
                                                    {
                                                        errmsg = "receive_date 參數值不是有效的日期格式 (TODAY, YESTERDAY 或 yyyyMMdd 格式)";
                                                    }
                                                    break;
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
                        if (String.IsNullOrEmpty(jobTypeId))
                        {
                            lost.Add("service_id");
                        }
                        if (receiveDate == null)
                        {
                            lost.Add("receive_date");
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
                        log.AppendLine(exitMsg);

                        if (args == null || args.Length == 0)
                        {
                            log.AppendLine("參數語法：service_id=所屬作業類別代碼(SC1、SC1B) receive_date=要處理的代收日(TODAY, YESTERDAY 或 yyyyMMdd)");
                        }
                    }
                }
                #endregion

                #region 處理 Config 參數
                #region 產出檔案路徑
                string dataPath = null;     //存放資料的路徑
                string sendPath = null;     //要傳送檔案的存放路徑
                string bakPath = null;      //備份檔案的存放路徑
                if (exitCode == 0)
                {
                    dataPath = ConfigurationManager.AppSettings.Get("DATA_PATH");
                    if (String.IsNullOrWhiteSpace(dataPath))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定產出檔案路徑 (DATA_PATH) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                    else
                    {
                        dataPath = dataPath.Trim();
                        sendPath = Path.Combine(dataPath, "send");
                        bakPath = Path.Combine(dataPath, "bak");

                        string errmsg = CheckOrCreateFolder(dataPath);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            errmsg = CheckOrCreateFolder(sendPath);
                            if (!String.IsNullOrEmpty(errmsg))
                            {
                                errmsg = CheckOrCreateFolder(bakPath);
                            }
                        }
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            exitCode = -1;
                            exitMsg = String.Format("Config (DATA_PATH)參數錯誤，{0}", errmsg);
                            jobLog.AppendLine(exitMsg);
                            log.AppendLine(exitMsg);
                        }
                    }
                }
                #endregion

                #region 指示檔名稱
                string configFileName = null;
                if (exitCode == 0)
                {
                    configFileName = ConfigurationManager.AppSettings.Get("config_file_name");
                    if (String.IsNullOrEmpty(configFileName))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定指示檔名稱 (config_file_name) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                }
                #endregion
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
                        log.AppendLine(result.Message);
                    }
                }
                #endregion

                #region 處理資料
                if (exitCode == 0)
                {
                    string chkName = "Global\\" + appGuid;
                    using (Mutex m = new Mutex(false, chkName))    //全域不可重複執行
                    {
                        //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                        if (m.WaitOne(0, false))
                        {
                            CancelHelper cancel_helper = new CancelHelper();

                            #region 取得要處理的商家代號 (有設定 FTP 的商家代號)
                            SchoolRTypeEntity[] schools = null;

                            if (!cancel_helper.GetHasFTPSchoolRtypes(out schools))
                            {
                                exitCode = -3;
                                exitMsg = string.Format("查詢要處理的商家代號資料失敗，錯誤訊息：{0}", cancel_helper.err_msg);
                                jobLog.AppendLine(exitMsg);
                                log.AppendLine(exitMsg);
                            }
                            #endregion

                            #region [MDY:20170818] M201708_01 排除 SC31ReceiveType 的商家代號 (20170531_01)
                            if (exitCode == 0 && schools != null && schools.Length > 0)
                            {
                                #region 取 SC31ReceiveType 系統參數
                                Result result = null;
                                ConfigEntity config = null;
                                Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.SC31_RECEIVETYPE);
                                using (EntityFactory factory = new EntityFactory())
                                {
                                    result = factory.SelectFirst<ConfigEntity>(where, null, out config);
                                }
                                #endregion

                                if (result.IsSuccess)
                                {
                                    #region 排除 SC31ReceiveType 的商家代號
                                    if (config != null && !String.IsNullOrWhiteSpace(config.ConfigValue))
                                    {
                                        string[] values = config.ConfigValue.Trim().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                        List<string> sc31ReceiveTypes = new List<string>(values.Length);
                                        for (int idx = 0; idx < values.Length; idx++)
                                        {
                                            string value = values[idx].Trim();
                                            if (!String.IsNullOrEmpty(value) && !sc31ReceiveTypes.Contains(value))
                                            {
                                                sc31ReceiveTypes.Add(value);
                                            }
                                        }

                                        List<SchoolRTypeEntity> list = new List<SchoolRTypeEntity>(schools.Length);
                                        foreach (SchoolRTypeEntity school in schools)
                                        {
                                            if (!sc31ReceiveTypes.Contains(school.ReceiveType))
                                            {
                                                list.Add(school);
                                            }
                                        }
                                        schools = list.ToArray();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    exitCode = -3;
                                    exitMsg = String.Format("查詢學校銷帳檔3合1的商家代號系統參失敗，錯誤訊息：{0}", result.Message);
                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                }
                            }
                            #endregion

                            #region 產生檔案
                            if (exitCode == 0)
                            {
                                if (schools == null || schools.Length == 0)
                                {
                                    exitMsg = "查無要處理的商家代號資料";
                                    jobLog.AppendLine(exitMsg);
                                    log.AppendLine(exitMsg);
                                }
                                else
                                {
                                    Encryption encrypt = new Encryption();
                                    string qTWDate7 = Common.GetTWDate7(receiveDate.Value);
                                    string bakTWDate7 = Common.GetTWDate7(startTime);
                                    foreach (SchoolRTypeEntity school in schools)
                                    {
                                        //讀取資料時應該就濾掉了，這裡只是 double check
                                        if (String.IsNullOrWhiteSpace(school.FtpAccount) || String.IsNullOrWhiteSpace(school.FtpLocation))
                                        {
                                            continue;
                                        }

                                        #region 睡1秒鐘以避免檔名重複
                                        Thread.Sleep(1000 * 1);
                                        #endregion

                                        string receive_type = school.ReceiveType.Trim();

                                        #region 每一個商家代號一個檔，對應一個 key 檔
                                        StringBuilder content = new StringBuilder();
                                        Int32 dataCount = 0;

                                        #region 取得資料並產生檔案內容
                                        CancelDebtsEntity[] cancel_debtss = null;
                                        {
                                            string errmsg = cancel_helper.GetCancelDebtsForGenCancelData(receive_type, qTWDate7, out cancel_debtss);
                                            if (String.IsNullOrEmpty(errmsg))
                                            {
                                                if (cancel_debtss != null && cancel_debtss.Length > 0)
                                                {
                                                    dataCount = cancel_debtss.Length;

                                                    #region 檔案內容格式
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

                                                    #region 組檔案內容
                                                    foreach (CancelDebtsEntity cancel_debts in cancel_debtss)
                                                    {
                                                        string seq = (cancel_debts.SourceSeq == null ? "" : cancel_debts.SourceSeq.ToString()).PadLeft(6, '0');
                                                        string serial_no = cancel_debts.CancelNo.Trim();
                                                        if (serial_no.Length == 14)
                                                        {
                                                            //14碼
                                                            serial_no = serial_no.Substring(4, 10);
                                                        }
                                                        else
                                                        {
                                                            //16碼
                                                            serial_no = serial_no.Substring(4, 12);
                                                        }
                                                        string receive_date = cancel_debts.ReceiveDate.PadLeft(8, '0');
                                                        string receive_time = cancel_debts.ReceiveTime;
                                                        string receive_amount = cancel_debts.ReceiveAmount.ToString("00000000000") + "00";

                                                        #region 更正記號處理 (0:正常交易 1:更正)
                                                        #region [Old]
                                                        //string flag = "0";
                                                        #endregion

                                                        string flag = cancel_debts.Reserve2;
                                                        #endregion

                                                        string receive_way = cancel_debts.ReceiveWay;
                                                        string account_date = cancel_debts.AccountDate.PadLeft(8, '0');
                                                        string bank_id = cancel_debts.ReceiveBank.Substring(3, 3);
                                                        string filler = "                   ";
                                                        string line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", seq, receive_type, serial_no, receive_date, receive_time, receive_amount, flag, receive_way, account_date, bank_id, filler);
                                                        content.AppendLine(line);
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    content.AppendLine(""); //無資料組空檔
                                                    jobLog.AppendFormat("查無 {0} 銷帳資料", receive_type, errmsg).AppendLine();
                                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 查無 {1} 銷帳資料", DateTime.Now, receive_type).AppendLine();
                                                }
                                            }
                                            else
                                            {
                                                content.AppendLine(""); //失敗組空檔
                                                jobLog.AppendFormat("讀取 {0} 銷帳資料失敗，錯誤訊息：{1}", receive_type, errmsg).AppendLine();
                                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 讀取 {1} 銷帳資料失敗，錯誤訊息：{2}", DateTime.Now, receive_type, errmsg).AppendLine();
                                            }
                                        }
                                        #endregion
                                        #endregion

                                        #region 寫檔
                                        try
                                        {
                                            string errmsg = null;

                                            string sendFilePath = Path.Combine(sendPath, receive_type);
                                            errmsg = CheckOrCreateFolder(sendFilePath, true);
                                            if (!String.IsNullOrEmpty(errmsg))
                                            {
                                                jobLog.AppendFormat("產生 {0} 的銷帳檔失敗，錯誤訊息：{1}", receive_type, errmsg).AppendLine();
                                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 產生  {1} 的銷帳檔失敗，錯誤訊息：{2}", DateTime.Now, receive_type, errmsg).AppendLine();
                                                continue; //沒法建立資料夾，所以沒法寫檔案，跳至下一個商家代號
                                            }

                                            string bakFilePath = Path.Combine(bakPath, receive_type, bakTWDate7);
                                            errmsg = CheckOrCreateFolder(bakFilePath);
                                            if (!String.IsNullOrEmpty(errmsg))
                                            {
                                                jobLog.AppendFormat("產生 {0} 的銷帳檔失敗，錯誤訊息：{1}", receive_type, errmsg).AppendLine();
                                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 產生  {1} 的銷帳檔失敗，錯誤訊息：{2}", DateTime.Now, receive_type, errmsg).AppendLine();
                                                continue; //沒法建立資料夾，所以沒法寫檔案，跳至下一個商家代號
                                            }

                                            DateTime now = DateTime.Now;

                                            #region 原始檔相關
                                            #region 產生原始資料檔
                                            string srcFileName = String.Format("{0}{1:HHmmss}.src", Common.GetTWDate7(now), now);
                                            string srcFileFullName = Path.Combine(sendFilePath, srcFileName);
                                            File.WriteAllText(srcFileFullName, content.ToString(), Encoding.Default);
                                            jobLog.AppendFormat("產生 {0} 的銷帳檔 ({1}) 成功，共 {2} 筆資料", receive_type, srcFileName, dataCount);
                                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 產生 {1} 的銷帳檔 ({2}) 成功，共 {3} 筆資料", DateTime.Now, receive_type, srcFileName, dataCount);
                                            #endregion

                                            #region 備份原始資料檔
                                            string bakFileFullName = Path.Combine(bakFilePath, srcFileName);
                                            try
                                            {
                                                File.Copy(srcFileFullName, bakFileFullName);
                                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 備份檔案 {1} 至 {2} 成功", DateTime.Now, srcFileFullName, bakFileFullName);
                                            }
                                            catch (Exception ex)
                                            {
                                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 備份檔案 {1} 至 {2} 發生例外，錯誤訊息：{3}", DateTime.Now, srcFileFullName, bakFileFullName, ex.Message);
                                            }
                                            #endregion
                                            #endregion

                                            #region 發送檔相關
                                            string sendFileName = srcFileName.Replace(".src", ".txt");
                                            string sendFileFullName = Path.Combine(sendFilePath, sendFileName);

                                            #region 產生 key 檔
                                            string keyFileName = sendFileName.Replace(".txt", ".key");
                                            string keyFileFullName = Path.Combine(sendFilePath, keyFileName);
                                            //key值：receive_type+ccMMddHHmmss。共16碼
                                            string key = String.Format("{0}{1}{2:HHmmss}", receive_type, Common.GetTWDate6(now), now);

                                            #region [MDY:20211114] M202110_05 使用 SFTP 的商家代號不做內容加密 (2021擴充案先做)
                                            if ("SFTP".Equals(school.FtpKind, StringComparison.CurrentCultureIgnoreCase))
                                            {
                                                key = "未加密";
                                            }
                                            #endregion

                                            try
                                            {
                                                File.WriteAllText(keyFileFullName, key, Encoding.Default);
                                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 產生 {1} 的 key 檔 ({2}) 成功", DateTime.Now, receive_type, keyFileFullName);
                                            }
                                            catch (Exception ex)
                                            {
                                                jobLog.AppendFormat("產生 {0} 的 key 檔 ({1}) 發生例外，錯誤訊息：{2}", receive_type, keyFileFullName, ex.Message);
                                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 產生 {1} 的 key 檔 ({2}) 發生例外，錯誤訊息：{3}", DateTime.Now, receive_type, keyFileFullName, ex.Message);
                                            }
                                            #endregion

                                            #region [MDY:20211114] M202110_05 使用 SFTP 的商家代號不做內容加密 (2021擴充案先做)
                                            if ("SFTP".Equals(school.FtpKind, StringComparison.CurrentCultureIgnoreCase))
                                            {
                                                #region 不加密，所以複製原始檔作為發送檔
                                                try
                                                {
                                                    File.Copy(srcFileFullName, sendFileFullName);
                                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 複製 {1} 的原始資料檔 ({2}) 成功", DateTime.Now, receive_type, srcFileFullName);
                                                }
                                                catch (Exception ex)
                                                {
                                                    jobLog.AppendFormat("複製 {0} 的原始資料檔 ({1}) 作為發送資料檔 ({2}) 失敗，錯誤訊息：{3}", receive_type, srcFileFullName, sendFileFullName, ex.Message);
                                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 複製 {1} 的原始資料檔 ({2}) 作為發送資料檔 ({3}) 失敗，錯誤訊息：{4}", DateTime.Now, receive_type, srcFileFullName, sendFileFullName, ex.Message);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region 加密資料檔成為發送資料檔
                                                string msg = null;
                                                if (encrypt.DESEncryptFile(srcFileFullName, sendFileFullName, key, out msg))
                                                {
                                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 產生 {1} 的加密資料檔 ({2}) 成功", DateTime.Now, receive_type, srcFileFullName);
                                                }
                                                else
                                                {
                                                    jobLog.AppendFormat("產生 {0} 的加密資料檔 ({1}) 失敗，錯誤訊息：{2}", receive_type, srcFileFullName, msg);
                                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 產生 {1} 的加密資料檔 ({2}) 失敗，錯誤訊息：{3}", DateTime.Now, receive_type, srcFileFullName, msg);
                                                }
                                                #endregion
                                            }
                                            #endregion

                                            #region 產生指示檔
                                            string configFileFullName = Path.Combine(sendFilePath, configFileName);
                                            {
                                                #region [MDY:20211001] M202110_01 支援 FTP/FTPS/SFTP (2021擴充案先做)
                                                string ftpKind = String.IsNullOrEmpty(school.FtpKind) ? "FTP" : school.FtpKind.Trim();
                                                string ftpHost = school.FtpLocation == null ? String.Empty : school.FtpLocation.Trim();
                                                string ftpPort = school.FtpPort == null ? String.Empty : school.FtpPort.Trim();
                                                string ftpUid = school.FtpAccount == null ? String.Empty : school.FtpAccount.Trim();

                                                #region [MDY:20220530] Checkmarx 調整
                                                string ftpPXX = school.FtpPXX == null ? String.Empty : school.FtpPXX;
                                                string remotePath = "/";
                                                StringBuilder cmd = new StringBuilder();
                                                cmd.AppendFormat("protocol={0} host={1} port={2} uid={3} pwd={4} remote_path={5} remote_file={6} local_file={7}", ftpKind, ftpHost, ftpPort, ftpUid, ftpPXX, remotePath, sendFileName, sendFileName).AppendLine();
                                                cmd.AppendFormat("protocol={0} host={1} port={2} uid={3} pwd={4} remote_path={5} remote_file={6} local_file={7}", ftpKind, ftpHost, ftpPort, ftpUid, ftpPXX, remotePath, keyFileName, keyFileName).AppendLine();
                                                #endregion
                                                #endregion

                                                try
                                                {
                                                    File.WriteAllText(configFileFullName, cmd.ToString(), Encoding.Default);
                                                    log.AppendFormat("產生 {0} 的指示檔案 ({1}) 成功", receive_type, configFileFullName);
                                                }
                                                catch (Exception ex)
                                                {
                                                    jobLog.AppendFormat("產生 {0} 的指示檔案 ({1}) 發生例外，錯誤訊息：{2}", receive_type, configFileFullName, ex.Message);
                                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 產生 {1} 的指示檔案 ({2}) 發生例外，錯誤訊息：{3}", DateTime.Now, receive_type, configFileFullName, ex.Message);
                                                }
                                            }
                                            #endregion

                                            #region 刪除原始資料檔避免被送到 WEB 端
                                            try
                                            {
                                                File.Delete(srcFileFullName);
                                                log.AppendFormat("刪除 {0} 的原始資料檔 ({1}) 成功", receive_type, srcFileFullName);
                                            }
                                            catch (Exception ex)
                                            {
                                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 刪除 {1} 的原始資料檔 ({2}) 發生例外，錯誤訊息：{3}", DateTime.Now, receive_type, srcFileFullName, ex.Message);
                                            }
                                            #endregion

                                            #region 產生 ok 檔
                                            string okFileFullName = Path.Combine(sendFilePath, "ok");
                                            try
                                            {
                                                File.WriteAllText(okFileFullName, "", Encoding.Default);
                                                log.AppendFormat("產生 {0} 的 ok 檔 ({1}) 成功", receive_type, okFileFullName);
                                            }
                                            catch (Exception ex)
                                            {
                                                jobLog.AppendFormat("產生 {0} 的 ok 檔 ({1}) 發生例外，錯誤訊息：{2}", receive_type, okFileFullName, ex.Message);
                                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 產生 {1} 的 ok 檔 ({2}) 發生例外，錯誤訊息：{3}", DateTime.Now, receive_type, okFileFullName, ex.Message);
                                            }
                                            #endregion
                                            #endregion

                                            #region 更新資料庫
                                            bool isUpdateOK = cancel_helper.BatchUpdateCancelDebtsResver1(DateTime.Now, cancel_debtss);
                                            if (!isUpdateOK)
                                            {
                                                //是否要刪除以產生的檔案
                                            }
                                            #endregion

                                        }
                                        catch (Exception ex)
                                        {
                                            jobLog.AppendFormat("產生 {0} 的銷帳檔失敗，錯誤訊息：{1}", receive_type, ex.Message);
                                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 產生  {1} 的銷帳檔失敗，錯誤訊息：{2}", DateTime.Now, receive_type, ex.Message);
                                            continue; //沒法建立資料夾，所以沒法寫檔案，跳至下一個商家代號
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region [MDY:20210401] 原碼修正
                            m.ReleaseMutex();
                            #endregion
                        }
                        else
                        {
                            #region [MDY:20210401] 不重複執行也要有日誌資訊
                            exitCode = -5;  //不重複執行回傳代碼
                            exitMsg = String.Format("執行緒中已存在 {0}，不重複執行", chkName);
                            jobLog.AppendLine(exitMsg);
                            log.AppendLine(exitMsg);
                            #endregion
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;
                exitMsg = String.Format("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 發生例外，參數：{1}，錯誤訊息：{2}", DateTime.Now, String.Join(" ", args), ex.Message).AppendLine();
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
                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 更新批次處理佇列為已完成失敗，{1}", DateTime.Now, result.Message).AppendLine();
                    }
                }
                jobHelper.Dispose();
                jobHelper = null;
                #endregion

                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 結束", DateTime.Now, appName).AppendLine();

                fileLog.WriteLog(log);
            }

            System.Environment.Exit(exitCode);
        }
    }
}
