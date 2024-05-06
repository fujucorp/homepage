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

namespace MoveToHistory
{
    /// <summary>
    /// 線上資料搬移與歷史資料刪除 (MDH)
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

        #region 批次處理佇列 相關
        /// <summary>
        /// 取得批次處理佇列的檢查模式 (預設 ByTime)
        /// </summary>
        /// <returns>傳回檢查模式</returns>
        private static JobCubeCheckMode GetJobCubeCheckMode()
        {
            string value = ConfigManager.Current.GetProjectConfigValue("JobCube", "CheckMode", StringComparison.CurrentCultureIgnoreCase);
            if (String.IsNullOrWhiteSpace(value))
            {
                return JobCubeCheckMode.ByTime;
            }
            else
            {
                return JobCubeHelper.ConvertCheckMode(value);
            }
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
        /// 參數：[retry_times=重試次數(0~8，預設5)] [retry_sleep=重試間隔，單位分鐘(0~60，預設5)]
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
            JobCubeCheckMode jobCheckMode = GetJobCubeCheckMode();
            string jobTypeId = JobCubeTypeCodeTexts.MDH;                        //作業類別代碼
            string jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);       //作業類別名稱
            int exitCode = 0;
            #endregion

            StringBuilder log = new StringBuilder();

            int retryTimes = 5;     //re-try 次數 (預設為5次)
            int retrySleep = 5;     //re-try 間隔，單位分鐘 (預設為5分鐘)

            DateTime startTime = DateTime.Now;

            JobCubeHelper jobHelper = new JobCubeHelper();
            int jobNo = 0;
            string jobStamp = null;
            StringBuilder jobMsg = new StringBuilder();
            StringBuilder jobLog = new StringBuilder();

            try
            {
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 開始", DateTime.Now, appName).AppendLine();

                #region 處理參數
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

                    #region 檢查必要參數
                    //沒有必要參數
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -1;
                        errmsg = String.Format("參數錯誤，錯誤訊息：{0}", errmsg);
                        jobMsg.AppendLine(errmsg);
                        jobLog.AppendLine(errmsg);
                        log.AppendLine(errmsg);

                        if (args == null || args.Length == 0)
                        {
                            log.AppendLine("參數語法：[retry_times=重試次數(0~8，預設5)] [retry_sleep=重試間隔，單位分鐘(0~60，預設5)]");
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
                        log.AppendLine(result.Message);
                    }
                }
                #endregion

                #region 處理資料
                if (exitCode == 0)
                {
                    using (Mutex m = new Mutex(false, "Global\\" + appGuid))    //全域不可重複執行
                    {
                        //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                        if (m.WaitOne(0, false))
                        {
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始 {1} 作業處理", DateTime.Now, jobTypeName).AppendLine();

                            HistoryHelper helper = new HistoryHelper(fileLog.LogPath);

                            DateTime checkDate = startTime.Date;

                            #region 移動線上資料到歷史資料表
                            {
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始 {1} 作業處理", DateTime.Now, "移動線上資料到歷史資料表").AppendLine();
                                //jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始 {1} 作業處理", DateTime.Now, "移動線上資料到歷史資料表").AppendLine();

                                string resultMsg = null;
                                string errmsg = helper.MoveOnlineData(checkDate, out resultMsg);
                                if (!String.IsNullOrEmpty(errmsg))
                                {
                                    for (int times = 1; times <= retryTimes; times++)
                                    {
                                        if (retrySleep > 0)
                                        {
                                            Thread.Sleep(1000 * 60 * retrySleep);
                                        }
                                        errmsg = helper.MoveOnlineData(checkDate, out resultMsg);
                                        if (String.IsNullOrEmpty(errmsg))
                                        {
                                            break;
                                        }
                                    }
                                    if (!String.IsNullOrEmpty(errmsg))
                                    {
                                        exitCode = -3;
                                        jobMsg.AppendFormat("移動線上資料到歷史資料表處理失敗，{0}；", errmsg);
                                    }
                                    else
                                    {
                                        jobMsg.Append("移動線上資料到歷史資料表處理成功；");
                                    }
                                }
                                log.AppendLine(resultMsg);
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束 {1} 作業處理", DateTime.Now, "移動線上資料到歷史資料表").AppendLine();

                                jobLog.AppendLine(resultMsg);
                                //jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束 {1} 作業處理", DateTime.Now, "移動線上資料到歷史資料表").AppendLine();
                            }
                            #endregion

                            #region 刪除歷史資料
                            {
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始 {1} 作業處理", DateTime.Now, "清除歷史資料").AppendLine();
                                //jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始 {1} 作業處理", DateTime.Now, "清除歷史資料").AppendLine();

                                string resultMsg = null;
                                string errmsg = helper.ClearHistoryData(checkDate, out resultMsg);
                                if (!String.IsNullOrEmpty(errmsg))
                                {
                                    for (int times = 1; times <= retryTimes; times++)
                                    {
                                        if (retrySleep > 0)
                                        {
                                            Thread.Sleep(1000 * 60 * retrySleep);
                                        }
                                        errmsg = helper.ClearHistoryData(checkDate, out resultMsg);
                                        if (String.IsNullOrEmpty(errmsg))
                                        {
                                            break;
                                        }
                                    }
                                    if (!String.IsNullOrEmpty(errmsg))
                                    {
                                        exitCode = -4;
                                        jobMsg.AppendFormat("清除歷史資料處理失敗，{0}；", errmsg);
                                    }
                                    else
                                    {
                                        jobMsg.Append("清除歷史資料處理成功；");
                                    }
                                }
                                log.AppendLine(resultMsg);
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束 {1} 作業處理", DateTime.Now, "清除歷史資料").AppendLine();

                                jobLog.AppendLine(resultMsg);
                                //jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束 {1} 作業處理", DateTime.Now, "清除歷史資料").AppendLine();
                            }
                            #endregion

                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束 {1} 作業處理", DateTime.Now, jobTypeName).AppendLine();

                            #region [MDY:20210401] 原碼修正
                            m.ReleaseMutex();
                            #endregion
                        }
                        else
                        {
                            exitCode = -8;
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 已在執行中，忽略此次處理", DateTime.Now, jobTypeName).AppendLine();

                            jobMsg.AppendFormat("{0} 已在執行中，忽略此次處理；", jobTypeName);
                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 已在執行中，忽略此次處理", DateTime.Now, jobTypeName).AppendLine();
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 發生例外，參數：{1}，錯誤訊息：{2}", DateTime.Now, String.Join(" ", args), ex.Message).AppendLine();

                jobMsg.AppendFormat("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
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
                        //jobMsg.Append(JobCubeResultCodeTexts.SUCCESS_TEXT);
                    }
                    else
                    {
                        jobResultId = JobCubeResultCodeTexts.FAILURE;
                    }

                    Result result = jobHelper.UpdateProcessJobToFinsh(jobNo, jobStamp, jobResultId, jobMsg.ToString(), jobLog.ToString());
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
            return;
        }
    }
}
