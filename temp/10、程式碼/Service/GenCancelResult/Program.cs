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

using Entities;

namespace GenCancelResult
{
    /// <summary>
    /// 計算每日銷帳結果 (SCR：每日銷帳結果處理、SCR2：每日銷帳結果處理(補))
    /// </summary>
    class Program
    {
        #region [OLD]
        //#region FileLog 相關
        //private class FileLoger
        //{
        //    private string _LogName = null;
        //    public string LogName
        //    {
        //        get
        //        {
        //            return _LogName;
        //        }
        //        private set
        //        {
        //            _LogName = value == null ? null : value.Trim();
        //        }
        //    }

        //    private string _LogPath = null;
        //    public string LogPath
        //    {
        //        get
        //        {
        //            return _LogPath;
        //        }
        //        private set
        //        {
        //            _LogPath = value == null ? String.Empty : value.Trim();
        //        }
        //    }

        //    public bool IsDebug
        //    {
        //        get;
        //        private set;
        //    }

        //    private string _LogFileName = null;

        //    public FileLoger(string logName)
        //    {
        //        this.LogName = logName;
        //        this.LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
        //        string logMode = ConfigurationManager.AppSettings.Get("LOG_MDOE");
        //        if (String.IsNullOrEmpty(logMode))
        //        {
        //            this.IsDebug = false;
        //        }
        //        else
        //        {
        //            this.IsDebug = (logMode.Trim().Equals("DEBUG", StringComparison.CurrentCultureIgnoreCase));
        //        }
        //        this.Initial();
        //    }

        //    public FileLoger(string logName, string path, bool isDebug)
        //    {
        //        this.LogName = logName;
        //        this.LogPath = path;
        //        this.IsDebug = isDebug;
        //        this.Initial();
        //    }

        //    public string Initial()
        //    {
        //        if (!String.IsNullOrEmpty(this.LogPath))
        //        {
        //            try
        //            {
        //                DirectoryInfo info = new DirectoryInfo(this.LogPath);
        //                if (!info.Exists)
        //                {
        //                    info.Create();
        //                }
        //                if (String.IsNullOrEmpty(this.LogName))
        //                {
        //                    string fileName = String.Format("{0:yyyyMMdd}.log", DateTime.Today);
        //                    _LogFileName = Path.Combine(info.FullName, fileName);
        //                }
        //                else
        //                {
        //                    string fileName = String.Format("{0}_{1:yyyyMMdd}.log", this.LogName, DateTime.Today);
        //                    _LogFileName = Path.Combine(info.FullName, fileName);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return ex.Message;
        //            }
        //        }
        //        return null;
        //    }

        //    public void WriteLog(string msg)
        //    {
        //        if (!String.IsNullOrEmpty(_LogFileName) && msg != null)
        //        {
        //            try
        //            {
        //                using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
        //                {
        //                    if (String.IsNullOrEmpty(msg))
        //                    {
        //                        sw.WriteLine(String.Empty);
        //                    }
        //                    else
        //                    {
        //                        sw.WriteLine("[{0:HH:mm:ss}] {1}", DateTime.Now, msg);
        //                    }
        //                }
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    }

        //    public void WriteLog(string format, params object[] args)
        //    {
        //        if (!String.IsNullOrEmpty(format) && args != null && args.Length > 0)
        //        {
        //            try
        //            {
        //                this.WriteLog(String.Format(format, args));
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    }

        //    public void WriteLog(StringBuilder msg)
        //    {
        //        if (!String.IsNullOrEmpty(_LogFileName) && msg != null && msg.Length > 0)
        //        {
        //            try
        //            {
        //                using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
        //                {
        //                    sw.WriteLine(msg);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    }

        //    public void WriteDebugLog(string msg)
        //    {
        //        if (this.IsDebug)
        //        {
        //            this.WriteLog(msg);
        //        }
        //    }

        //    public void WriteDebugLog(string format, params object[] args)
        //    {
        //        if (this.IsDebug)
        //        {
        //            this.WriteLog(format, args);
        //        }
        //    }
        //}
        //#endregion

        //#region 系統參數 相關
        //private static string GetTempPath()
        //{
        //    string tempPath = ConfigurationManager.AppSettings.Get("TEMP_PATH");
        //    if (!String.IsNullOrWhiteSpace(tempPath))
        //    {
        //        tempPath = tempPath.Trim();
        //        try
        //        {
        //            DirectoryInfo info = new DirectoryInfo(tempPath);
        //            if (!info.Exists)
        //            {
        //                info.Create();
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            tempPath = null;
        //        }
        //    }
        //    if (String.IsNullOrEmpty(tempPath))
        //    {
        //        tempPath = Path.GetTempPath();
        //    }
        //    return tempPath;
        //}
        //#endregion

        ///// <summary>
        ///// 最大失敗重試次數 : 8
        ///// </summary>
        //private static int _MaxRetryTimes = 8;
        ///// <summary>
        ///// 最大失敗重試間隔(單位分鐘) : 60
        ///// </summary>
        //private static int _MaxRetrySleep = 60;

        ///// <summary>
        ///// 參數：account_date=要計算的入帳日(TODAY, YESTERDAY 或 yyyyMMdd) service_id=所屬作業類別代碼(SCR、SCR2) [receive_type=要計算的商家代號] [retry_times=重試次數(0~8，預設5)] [retry_sleep=重試間隔，單位分鐘(0~60，預設5)]
        ///// </summary>
        ///// <param name="args"></param>
        //static void Main(string[] args)
        //{
        //    #region Initial
        //    //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        //    Assembly myAssembly = Assembly.GetExecutingAssembly();
        //    string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
        //    string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);

        //    FileLoger fileLog = new FileLoger(appName);
        //    JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
        //    string jobTypeId = null;        //作業類別代碼
        //    string jobTypeName = null;      //作業類別名稱
        //    int exitCode = 0;
        //    string exitMsg = null;
        //    #endregion

        //    StringBuilder log = new StringBuilder();

        //    int retryTimes = 5;     //re-try 次數 (預設為5次)
        //    int retrySleep = 5;     //re-try 間隔，單位分鐘 (預設為5分鐘)

        //    DateTime startTime = DateTime.Now;

        //    JobCubeHelper jobHelper = new JobCubeHelper();
        //    int jobNo = 0;
        //    string jobStamp = null;
        //    StringBuilder jobLog = new StringBuilder();

        //    try
        //    {
        //        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 開始", DateTime.Now, appName).AppendLine();

        //        #region 處理參數
        //        DateTime? accountDate = null;
        //        string receiveType = null;
        //        if (exitCode == 0)
        //        {
        //            log.AppendFormat("命令參數：{0}", (args == null || args.Length == 0) ? null : String.Join(" ", args)).AppendLine();

        //            string errmsg = null;

        //            if (args != null && args.Length > 0)
        //            {
        //                #region 拆解參數
        //                foreach (string arg in args)
        //                {
        //                    string[] kvs = arg.Split('=');
        //                    if (kvs.Length == 2)
        //                    {
        //                        string key = kvs[0].Trim().ToLower();
        //                        string value = kvs[1].Trim();
        //                        switch (key)
        //                        {
        //                            case "account_date":
        //                                #region accountDate (要計算的入帳日)
        //                                {
        //                                    switch (value.ToUpper())
        //                                    {
        //                                        case "TODAY":
        //                                            accountDate = startTime.Date;
        //                                            break;
        //                                        case "YESTERDAY":
        //                                            accountDate = startTime.Date.AddDays(-1);
        //                                            break;
        //                                        default:
        //                                            accountDate = DataFormat.ConvertDateText(value);
        //                                            if (accountDate == null)
        //                                            {
        //                                                errmsg = "account_date 參數值不是有效的日期格式 (TODAY, YESTERDAY 或 yyyyMMdd 格式)";
        //                                            }
        //                                            break;
        //                                    }
        //                                }
        //                                break;
        //                                #endregion
        //                            case "receive_type":
        //                                #region receiveType (要計算的商家代號)
        //                                if (!String.IsNullOrEmpty(value))
        //                                {
        //                                    if (!Common.IsNumber(value, 4, 4))
        //                                    {
        //                                        errmsg = "receive_type 命令參數不是有效的商家代號格式 (4碼數字格式)";
        //                                    }
        //                                    else
        //                                    {
        //                                        #region [MDY:20170722] 避免 Checkmarx 的 Heuristic SQL Injection 誤判，加工處理
        //                                        #region [Old]
        //                                        //receiveType = value;
        //                                        #endregion

        //                                        receiveType = int.Parse(value).ToString();
        //                                        #endregion
        //                                    }
        //                                }
        //                                break;
        //                                #endregion

        //                            case "service_id":
        //                                #region jobTypeId (所屬作業類別代碼)
        //                                {
        //                                    jobTypeId = value;
        //                                    jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);
        //                                    if (String.IsNullOrEmpty(jobTypeName))
        //                                    {
        //                                        errmsg = "service_id 參數值不是正確的作業類別代碼";
        //                                    }
        //                                }
        //                                break;
        //                                #endregion
        //                            case "retry_times":
        //                                #region retryTimes (re-try 次數)
        //                                if (!String.IsNullOrEmpty(value))
        //                                {
        //                                    int times = 0;
        //                                    if (int.TryParse(value, out times))
        //                                    {
        //                                        if (times <= 0)
        //                                        {
        //                                            retryTimes = 0;
        //                                        }
        //                                        else if (times >= _MaxRetryTimes)
        //                                        {
        //                                            retryTimes = _MaxRetryTimes;
        //                                        }
        //                                        else
        //                                        {
        //                                            retryTimes = times;
        //                                        }
        //                                    }
        //                                }
        //                                break;
        //                                #endregion
        //                            case "retry_sleep":
        //                                #region retrySleep (re-try 間隔，單位分鐘)
        //                                if (!String.IsNullOrEmpty(value))
        //                                {
        //                                    int minutes = 0;
        //                                    if (int.TryParse(value, out minutes))
        //                                    {
        //                                        if (minutes <= 1)
        //                                        {
        //                                            retrySleep = 1;
        //                                        }
        //                                        else if (minutes >= _MaxRetrySleep)
        //                                        {
        //                                            retrySleep = _MaxRetrySleep;
        //                                        }
        //                                        else
        //                                        {
        //                                            retrySleep = minutes;
        //                                        }
        //                                    }
        //                                }
        //                                break;
        //                                #endregion
        //                            default:
        //                                errmsg = String.Format("不支援 {0} 命令參數", arg);
        //                                break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        errmsg = String.Format("不支援 {0} 命令參數", arg);
        //                    }
        //                    if (!String.IsNullOrEmpty(errmsg))
        //                    {
        //                        break;
        //                    }
        //                }
        //                #endregion
        //            }
        //            else
        //            {
        //                errmsg = "未指定命令參數";
        //            }

        //            #region 檢查必要參數
        //            if (String.IsNullOrEmpty(errmsg))
        //            {
        //                List<string> lost = new List<string>();
        //                if (accountDate == null)
        //                {
        //                    lost.Add("account_date");
        //                }
        //                if (String.IsNullOrEmpty(jobTypeId))
        //                {
        //                    lost.Add("service_id");
        //                }
        //                if (lost.Count > 0)
        //                {
        //                    errmsg = String.Format("缺少 {0} 參數", String.Join(", ", lost));
        //                }
        //            }
        //            #endregion

        //            if (!String.IsNullOrEmpty(errmsg))
        //            {
        //                exitCode = -1;
        //                exitMsg = String.Format("參數錯誤，錯誤訊息：{0}", errmsg);
        //                jobLog.AppendLine(exitMsg);
        //                log.AppendLine(exitMsg);

        //                if (args == null || args.Length == 0)
        //                {
        //                    log.AppendLine("參數語法：account_date=要計算的入帳日(TODAY, YESTERDAY 或 yyyyMMdd) service_id=所屬作業類別代碼(SCR、SCR2) [retry_times=重試次數(0~8，預設5)] [retry_sleep=重試間隔，單位分鐘(0~60，預設5)]");
        //                }
        //            }
        //        }
        //        #endregion

        //        #region 新增處理中的 Job
        //        if (exitCode == 0)
        //        {
        //            JobcubeEntity job = new JobcubeEntity();
        //            job.Jtypeid = jobTypeId;
        //            job.Jparam = String.Join(" ", args);
        //            Result result = jobHelper.InsertProcessJob(ref job, jobCheckMode);
        //            if (result.IsSuccess)
        //            {
        //                jobNo = job.Jno;
        //                jobStamp = job.Memo;
        //            }
        //            else
        //            {
        //                exitCode = -2;
        //                log.AppendLine(result.Message);
        //            }
        //        }
        //        #endregion

        //        #region 處理資料
        //        if (exitCode == 0)
        //        {
        //            string resultLog = null;
        //            CancelHelper helper = new CancelHelper();
        //            string errmsg = helper.GenCancelResultData(accountDate.Value, receiveType, retryTimes, retrySleep, out resultLog);
        //            jobLog.AppendLine(resultLog);
        //            log.AppendLine(resultLog);
        //            if (String.IsNullOrEmpty(errmsg))
        //            {
        //                exitMsg = "產生每日銷帳結果資料完成";
        //            }
        //            else
        //            {
        //                exitCode = -1;
        //                exitMsg = String.Format("產生每日銷帳結果資料失敗，錯誤訊息：{0}；", errmsg);
        //            }
        //            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
        //            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        exitCode = -9;
        //        exitMsg = String.Format("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
        //        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
        //        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 發生例外，參數：{1}，錯誤訊息：{2}", DateTime.Now, String.Join(" ", args), ex.Message).AppendLine();
        //    }
        //    finally
        //    {
        //        #region 更新 Job
        //        if (jobNo > 0)
        //        {
        //            string jobResultId = null;
        //            if (exitCode == 0)
        //            {
        //                jobResultId = JobCubeResultCodeTexts.SUCCESS;
        //            }
        //            else
        //            {
        //                jobResultId = JobCubeResultCodeTexts.FAILURE;
        //            }

        //            Result result = jobHelper.UpdateProcessJobToFinsh(jobNo, jobStamp, jobResultId, exitMsg, jobLog.ToString());
        //            if (!result.IsSuccess)
        //            {
        //                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 更新批次處理佇列為已完成失敗，{1}", DateTime.Now, result.Message).AppendLine();
        //            }
        //        }
        //        jobHelper.Dispose();
        //        jobHelper = null;
        //        #endregion

        //        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 結束", DateTime.Now, appName).AppendLine();

        //        fileLog.WriteLog(log);
        //    }

        //    System.Environment.Exit(exitCode);
        //}
        #endregion

        #region [MDY:20191214] (2019擴充案) 重寫並增加 days 參數
        #region 日誌檔物件
        /// <summary>
        /// 日誌檔物件（銷帳處理預期每次資料量都不大，所以日誌內容全部組完再寫入檔案）
        /// </summary>
        private class FileLoger
        {
            #region Member
            /// <summary>
            /// 日誌檔完整路徑
            /// </summary>
            private string LogFileName = null;

            /// <summary>
            /// 日誌內容
            /// </summary>
            private StringBuilder Log = null;
            #endregion

            #region Property
            /// <summary>
            /// 任務編號
            /// </summary>
            public string TaskNo
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌檔名稱
            /// </summary>
            public string LogName
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌檔路徑
            /// </summary>
            public string LogPath
            {
                get;
                private set;
            }

            /// <summary>
            /// 是否為除錯模式
            /// </summary>
            public bool IsDebug
            {
                get;
                private set;
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 空的日誌檔物件
            /// </summary>
            private FileLoger()
            {

            }

            /// <summary>
            /// 建構 日誌檔物件
            /// </summary>
            /// <param name="logName">日誌檔名稱</param>
            /// <param name="path">日誌檔路徑</param>
            /// <param name="isDebug">是否為除錯模式</param>
            private FileLoger(string taskNo, string logName, string logPath, bool isDebug)
            {
                this.TaskNo = taskNo == null ? null : taskNo.Trim();
                this.LogName = logName == null ? String.Empty : logName.Trim();
                this.LogPath = logPath == null ? String.Empty : logPath.Trim();
                this.IsDebug = isDebug;
            }
            #endregion

            #region Private Method
            /// <summary>
            /// 初始化
            /// </summary>
            /// <returns>傳回錯誤訊息</returns>
            private string Initial()
            {
                if (String.IsNullOrEmpty(this.LogName))
                {
                    return "未指定日誌檔路徑，系統將不紀錄日誌";
                }

                string errmsg = null;
                try
                {
                    DirectoryInfo info = new DirectoryInfo(this.LogPath);
                    if (!info.Exists)
                    {
                        info.Create();
                    }

                    if (String.IsNullOrEmpty(this.LogName))
                    {
                        this.LogFileName = Path.Combine(info.FullName, String.Format("{0:yyyyMMdd}.log", DateTime.Today));
                    }
                    else
                    {
                        this.LogFileName = Path.Combine(info.FullName, String.Format("{0}_{1:yyyyMMdd}.log", this.LogName, DateTime.Today));
                    }
                    this.Log = new StringBuilder();
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
                return errmsg;
            }

            /// <summary>
            /// 取得目前日誌內容是否以 NewLine 結尾
            /// </summary>
            /// <returns></returns>
            private bool IsNewLineEnd()
            {
                if (this.Log != null)
                {
                    if (this.Log.Length == 0)
                    {
                        return true;
                    }
                    if (this.Log.Length >= Environment.NewLine.Length)
                    {
                        return (this.Log.ToString(this.Log.Length - Environment.NewLine.Length, Environment.NewLine.Length) == Environment.NewLine);
                    }
                }
                return false;
            }

            /// <summary>
            /// 新增格式化訊息至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="format">訊息格式</param>
            /// <param name="args">訊息參數陣列</param>
            /// <returns>傳回日誌檔物件自己</returns>
            private FileLoger AppendFormat(bool isStart, string format, params object[] args)
            {
                if (this.Log != null && this.LogFileName != null)
                {
                    try
                    {
                        if (isStart)
                        {
                            if (!this.IsNewLineEnd())
                            {
                                this.Log.AppendLine();
                            }
                            this.Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                        }
                        this.Log.AppendFormat(format, args);
                    }
                    catch (Exception ex)
                    {
                        this.Log.AppendFormat("注意：FileLoger.AppendStartFormat() 發生例外，{0}", ex.Message).AppendLine();
                        if (this.IsDebug)
                        {
                            this.Log
                                .AppendFormat("    format 參數：{0}", format).AppendLine()
                                .Append("    args 參數：");
                            foreach (object arg in args)
                            {
                                this.Log.AppendFormat("{0}; ", arg);
                            }
                            this.Log.AppendLine().Append(ex).AppendLine();
                        }
                    }
                }
                return this;
            }

            /// <summary>
            /// 新增訊息至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="arg">訊息</param>
            /// <returns>傳回日誌檔物件自己</returns>
            private FileLoger Append(bool isStart, string arg = null)
            {
                if (this.Log != null && this.LogFileName != null)
                {
                    try
                    {
                        if (!String.IsNullOrWhiteSpace(arg))
                        {
                            if (isStart)
                            {
                                if (!this.IsNewLineEnd())
                                {
                                    this.Log.AppendLine();
                                }
                                this.Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                            }
                            this.Log.Append(arg);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log.AppendFormat("注意：FileLoger.AppendStartFormat() 發生例外，{0}", ex.Message).AppendLine();
                        if (this.IsDebug)
                        {
                            this.Log
                                .AppendFormat("    arg 參數：{0}", arg).AppendLine()
                                .AppendLine().Append(ex).AppendLine();
                        }
                    }
                }
                return this;
            }

            /// <summary>
            /// 新增訊息與換行至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="arg">訊息</param>
            /// <returns>傳回日誌檔物件自己</returns>
            private FileLoger AppendLine(bool isStart, string arg = null)
            {
                if (this.Log != null && this.LogFileName != null)
                {
                    try
                    {
                        if (String.IsNullOrWhiteSpace(arg))
                        {
                            this.Log.AppendLine();
                        }
                        else
                        {
                            if (isStart)
                            {
                                if (!this.IsNewLineEnd())
                                {
                                    this.Log.AppendLine();
                                }
                                this.Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                            }
                            this.Log.AppendLine(arg);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log.AppendFormat("注意：FileLoger.AppendStartFormat() 發生例外，{0}", ex.Message).AppendLine();
                        if (this.IsDebug)
                        {
                            this.Log
                                .AppendFormat("    arg 參數：{0}", arg).AppendLine()
                                .AppendLine().Append(ex).AppendLine();
                        }
                    }
                }
                return this;
            }
            #endregion

            #region Public Method
            #region 日誌訊息
            /// <summary>
            /// 以新行開始，新增目前日期時間與格式化日誌訊息至日誌內容
            /// </summary>
            /// <param name="format">日誌訊息格式</param>
            /// <param name="args">日誌訊息參數陣列</param>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendFormatStartLog(string format, params object[] args)
            {
                return this.AppendFormat(true, format, args);
            }

            /// <summary>
            /// 新增格式化日誌訊息至日誌內容
            /// </summary>
            /// <param name="format">日誌訊息格式</param>
            /// <param name="args">日誌訊息參數陣列</param>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendFormatLog(string format, params object[] args)
            {
                return this.AppendFormat(false, format, args);
            }

            /// <summary>
            /// 新增日誌訊息至日誌內容
            /// </summary>
            /// <param name="arg">日誌訊息</param>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendLog(string arg)
            {
                return this.Append(false, arg);
            }

            /// <summary>
            /// 以新行開始，新增目前日期時間、日誌訊息與換行至日誌內容
            /// </summary>
            /// <param name="arg">日誌訊息</param>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendLineStartLog(string arg)
            {
                return this.AppendLine(true, arg);
            }

            /// <summary>
            /// 新增日誌訊息與換行至日誌內容
            /// </summary>
            /// <param name="arg">日誌訊息</param>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendLineLog(string arg)
            {
                return this.AppendLine(false, arg);
            }

            /// <summary>
            /// 新增換行的日誌訊息至日誌內容
            /// </summary>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendLineLog()
            {
                return this.AppendLine(false, null);
            }
            #endregion

            #region 除錯訊息
            /// <summary>
            /// 新增格式化除錯訊息至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="format">除錯訊息格式</param>
            /// <param name="args">除錯訊息參數陣列</param>
            /// <returns></returns>
            public FileLoger AppendFormatDebugLog(bool isStart, string format, params object[] args)
            {
                if (this.IsDebug)
                {
                    this.AppendFormat(isStart, format, args);
                }
                return this;
            }

            /// <summary>
            /// 新增除錯訊息至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="arg">除錯訊息</param>
            /// <returns></returns>
            public FileLoger AppendDebugLog(bool isStart, string arg)
            {
                if (this.IsDebug)
                {
                    this.Append(isStart, arg);
                }
                return this;
            }

            /// <summary>
            /// 新增除錯訊息與換行至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="arg">除錯訊息</param>
            /// <returns></returns>
            public FileLoger AppendLineDebugLog(bool isStart, string arg)
            {
                if (this.IsDebug)
                {
                    this.AppendLine(isStart, arg);
                }
                return this;
            }

            /// <summary>
            /// 新增換行除錯訊息至日誌內容
            /// </summary>
            /// <returns></returns>
            public FileLoger AppendLineDebugLog()
            {
                if (this.IsDebug)
                {
                    this.AppendLine(false, null);
                }
                return this;
            }
            #endregion

            #region 寫檔
            /// <summary>
            /// 將日誌內容寫入日誌檔
            /// </summary>
            /// <param name="jobNo">指定作業編號</param>
            /// <returns>失敗傳回錯誤訊息，否則傳回 null</returns>
            public string WriteLogToFile(string jobNo = null)
            {
                string errmsg = null;
                if (this.LogFileName != null && this.Log != null)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(this.LogFileName, true, Encoding.Default))
                        {
                            if (this.Log.Length == 0)
                            {
                                sw.WriteLine(String.Empty);
                            }
                            else
                            {
                                sw.WriteLine("// [任務：{0}] [作業：{1}] 日誌內容開始 -----------------------------------------------------------------------------------------------\\", this.TaskNo, jobNo);
                                sw.WriteLine(this.Log);
                                sw.WriteLine("\\ [任務：{0}] [作業：{1}] 日誌內容結束 -----------------------------------------------------------------------------------------------//", this.TaskNo, jobNo);
                                sw.WriteLine();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errmsg = ex.Message;
                    }
                    finally
                    {
                        this.Log.Clear();  //寫檔後不管成功失敗一律清空日誌內容
                    }
                }
                return errmsg;
            }
            #endregion
            #endregion

            #region Static Method
            /// <summary>
            /// 建立 日誌檔物件
            /// </summary>
            /// <param name="logName">日誌檔名稱</param>
            /// <param name="logPath">日誌檔路徑</param>
            /// <param name="isDebug">是否啟用除錯模式</param>
            /// <param name="taskNo">任務編號</param>
            /// <returns>失敗則傳回錯誤訊息</returns>
            public static FileLoger Create(string logName, out string errmsg, string logPath = null, bool? isDebug = null, string taskNo = null)
            {
                FileLoger loger = null;

                errmsg = null;
                try
                {
                    if (String.IsNullOrWhiteSpace(logPath))
                    {
                        logPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                    }

                    if (!isDebug.HasValue)
                    {
                        string logMode = ConfigurationManager.AppSettings.Get("LOG_MDOE");
                        if (!String.IsNullOrWhiteSpace(logMode))
                        {
                            isDebug = "DEBUG".Equals(logMode.Trim(), StringComparison.CurrentCultureIgnoreCase);
                        }
                        else
                        {
                            isDebug = false;
                        }
                    }

                    loger = new FileLoger(taskNo, logName, logPath, isDebug.Value);
                    errmsg = loger.Initial();
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }

                if (loger == null)
                {
                    return new FileLoger();
                }
                else
                {
                    return loger;
                }
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// 最大失敗重試次數 : 8
        /// </summary>
        private static int _MaxRetryTimes = 8;
        /// <summary>
        /// 最大失敗重試間隔(單位分鐘) : 60
        /// </summary>
        private static int _MaxRetrySleep = 60;

        /// <summary>
        /// 計算每日銷帳結果主程序
        /// </summary>
        /// <param name="args">執行參數陣列</param>
        /// <remarks>
        /// 執行參數說明
        /// service_id：指定排程作業類別代碼，可指定每日銷帳結果處理（SCR）、每日銷帳結果處理(補))(補)（SCR2）。必要參數
        /// account_date：指定要計算的入帳日期，可指定今天（TODAY）、昨天（YESTERDAY）、特定日期（西元年 yyyyMMdd）。必要參數
        /// days：指定 SCR2 的回溯天數 (從 account_date 開始往回處理的天數)，可指定 0 ~ 7，限 SCR2 有效。非必要參數
        /// receive_type：要計算的商家代號。非必要參數
        /// retry_times：重試次數，可指定 0 ~ 8，預設 5。非必要參數
        /// retry_sleep：重試間隔，單位分鐘，可指定 0 ~ 60，預設 5。非必要參數
        /// </remarks>
        static void Main(string[] args)
        {
            #region 變數初始化
            string appGuid = null;  //執行程式 Guid
            string appName = null;  //執行程式 檔名
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
                appName = Path.GetFileNameWithoutExtension(myAssembly.Location);
            }

            int exitCode = 0;       //執行結果回傳代碼
            string exitMsg = null;

            JobCubeHelper jobHelper = new JobCubeHelper();
            int jobNo = 0;
            string jobStamp = null;
            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = null;        //作業類別代碼 (SCR、SCR2)
            string jobTypeName = null;      //作業類別名稱
            StringBuilder jobLog = new StringBuilder();

            DateTime startTime = DateTime.Now;
            string fileLogError = null;
            FileLoger fileLog = FileLoger.Create(appName, out fileLogError, taskNo: startTime.ToString("yyyyMMddHHmmss"));
            if (!String.IsNullOrEmpty(fileLogError))
            {
                jobLog.AppendFormat("注意！日誌檔物件建立失敗，{0}", fileLogError);
            }

            int retryTimes = 5;     //re-try 次數，預設為 5 次
            int retrySleep = 5;     //re-try 間隔，單位分鐘，預設為 5 分鐘
            #endregion

            try
            {
                fileLog.AppendFormatStartLog("{0} 開始執行，參數：{1}", appName, ((args == null || args.Length == 0) ? null : String.Join(" ", args))).AppendLineLog();

                #region 處理參數
                DateTime? accountDate = null;  //入帳日期
                Int32? days = null;            //回溯天數
                string receiveType = null;     //商家代號

                if (exitCode == 0)
                {
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
                                        #region jobTypeId (作業類別代碼)
                                        {
                                            jobTypeId = value;
                                            jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);
                                            if ((jobTypeId != JobCubeTypeCodeTexts.SCR && jobTypeId != JobCubeTypeCodeTexts.SCR2) || String.IsNullOrEmpty(jobTypeName))
                                            {
                                                errmsg = "service_id 參數值不是正確的作業類別代碼";
                                            }
                                        }
                                        break;
                                        #endregion

                                    case "account_date":
                                        #region accountDate (處理的入帳日)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            switch (value.ToUpper())
                                            {
                                                case "TODAY":
                                                    accountDate = startTime.Date;
                                                    break;
                                                case "YESTERDAY":
                                                    accountDate = startTime.Date.AddDays(-1);
                                                    break;
                                                default:
                                                    accountDate = DataFormat.ConvertDateText(value);
                                                    if (!accountDate.HasValue)
                                                    {
                                                        errmsg = "account_date 參數值不是有效的日期格式 (TODAY, YESTERDAY 或 yyyyMMdd 格式)";
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                        #endregion

                                    case "days":
                                        #region days (處理的天數)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            Int32 val = 0;
                                            if (Int32.TryParse(value, out val) && val >= 0 && val <= 7)
                                            {
                                                days = val;
                                            }
                                            else
                                            {
                                                errmsg = "days 參數值不正確 (0 ~ 7)";
                                            }
                                        }
                                        break;
                                        #endregion

                                    case "receive_type":
                                        #region receiveType (處理的商家代號)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            if (!Common.IsNumber(value, 4, 4))
                                            {
                                                errmsg = "receive_type 命令參數不是有效的商家代號格式 (4碼數字格式)";
                                            }
                                            else
                                            {
                                                #region [MDY:20170722] 避免 Checkmarx 的 Heuristic SQL Injection 誤判，加工處理
                                                receiveType = int.Parse(value).ToString();
                                                #endregion
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

                        if (accountDate == null)
                        {
                            lost.Add("account_date");
                        }

                        if (lost.Count > 0)
                        {
                            errmsg = String.Format("缺少 {0} 參數", String.Join(", ", lost));
                        }
                    }
                    #endregion

                    #region 檢查配對使用的參數
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        if (jobTypeId == JobCubeTypeCodeTexts.SCR2)
                        {
                            if (!days.HasValue)
                            {
                                days = 0;
                            }
                        }
                        else
                        {
                            if (days.HasValue)
                            {
                                errmsg = "service_id 不是 " + JobCubeTypeCodeTexts.SCR2 + " 時 days 參數不可使用";
                            }
                        }
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -1;  //參數錯誤回傳代碼
                        exitMsg = String.Format("參數錯誤，錯誤訊息：{0}", errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog
                            .AppendLineLog(exitMsg)
                            .AppendLineLog("參數語法：service_id=作業類別代碼(SCR、SCR2) account_date=入帳日期(TODAY、YESTERDAY、yyyyMMdd) [days=回溯天數(0~7 限SCR2使用)] [retry_times=重試次數(0~8，預設5)] [retry_sleep=重試間隔，單位分鐘(0~60，預設5)]");
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
                        exitCode = -2;  //新增處理作業資料失敗回傳代碼
                        fileLog.AppendFormatLog("新增處理中的批次處理佇列資料失敗，{0}", result.Message).AppendLineLog();
                    }
                }
                #endregion

                #region 處理資料
                if (exitCode == 0)
                {
                    CancelHelper cancelHelper = new CancelHelper();
                    if (jobTypeId == JobCubeTypeCodeTexts.SCR)
                    {
                        #region SCR 銷帳處理
                        string resultLog = null;
                        string errmsg = cancelHelper.GenCancelResultData(accountDate.Value, receiveType, retryTimes, retrySleep, out resultLog);
                        jobLog.AppendLine(resultLog);

                        if (String.IsNullOrEmpty(errmsg))
                        {
                            exitMsg = "產生每日銷帳結果資料完成";
                        }
                        else
                        {
                            exitCode = -3;  //銷帳處理失敗回傳代碼
                            exitMsg = String.Format("產生每日銷帳結果資料失敗，錯誤訊息：{0}；", errmsg);
                        }

                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                        fileLog.AppendLineStartLog(exitMsg);
                        #endregion
                    }
                    else
                    {
                        #region SCR2 銷帳處理(補)
                        DateTime checkDate = accountDate.Value;
                        for (int day = 0; day <= days.Value; day++)
                        {
                            jobLog.AppendFormat("開始處理 {0} 的資料", checkDate).AppendLine();
                            fileLog.AppendFormatLog("開始處理 {0} 的資料", checkDate).AppendLineLog();

                            string resultLog = null;
                            string errmsg = cancelHelper.GenCancelResultData(checkDate, receiveType, retryTimes, retrySleep, out resultLog);
                            jobLog.AppendLine(resultLog);
                            if (String.IsNullOrEmpty(errmsg))
                            {
                                if (String.IsNullOrWhiteSpace(cancelHelper.err_msg))
                                {
                                    exitMsg = "產生每日銷帳結果完成";
                                }
                                else
                                {
                                    exitMsg = String.Format("產生每日銷帳結果完成，訊息：{0}", cancelHelper.err_msg);
                                }
                            }
                            else
                            {
                                exitCode = -3;  //銷帳處理失敗回傳代碼
                                exitMsg = String.Format("產生每日銷帳結果資料失敗，錯誤訊息：{0}；", errmsg);
                            }
                            checkDate = checkDate.AddDays(-1);
                        }
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;  //發生例外回傳代碼
                exitMsg = String.Format("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
                fileLog.AppendFormatStartLog("{0} 發生例外，錯誤訊息：{1}", jobTypeName, ex.Message).AppendLineLog();
                fileLog.AppendFormatDebugLog(false, "[DEBUG] 例外訊息：{0}", ex).AppendLineDebugLog();
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
                        fileLog.AppendFormatStartLog("更新批次處理佇列為已完成失敗，{0}", result.Message).AppendLineLog();
                    }
                }
                jobHelper.Dispose();
                jobHelper = null;
                #endregion

                fileLog.AppendFormatStartLog("{0} 執行結束", appName).AppendLineLog();
                fileLogError = fileLog.WriteLogToFile(jobNo.ToString());
            }

            System.Environment.Exit(exitCode);
        }
        #endregion
    }
}
