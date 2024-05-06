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

namespace SendBillMail
{
    /// <summary>
    /// 發送繳費通知信 (SBM)
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

        #region 系統參數 相關
        private static string GetTempPath()
        {
            string tempPath = ConfigurationManager.AppSettings.Get("TEMP_PATH");
            if (!String.IsNullOrWhiteSpace(tempPath))
            {
                tempPath = tempPath.Trim();
                try
                {
                    DirectoryInfo info = new DirectoryInfo(tempPath);
                    if (!info.Exists)
                    {
                        info.Create();
                    }
                }
                catch (Exception)
                {
                    tempPath = null;
                }
            }
            if (String.IsNullOrEmpty(tempPath))
            {
                tempPath = Path.GetTempPath();
            }
            return tempPath;
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
            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = JobCubeTypeCodeTexts.SBM;                        //作業類別代碼
            string jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);       //作業類別名稱
            int exitCode = 0;
            string exitMsg = null;
            #endregion

            StringBuilder log = new StringBuilder();

            int retryTimes = 5;     //re-try 次數 (預設為5次)
            int retrySleep = 5;     //re-try 間隔，單位分鐘 (預設為5分鐘)

            DateTime startTime = DateTime.Now;

            JobCubeHelper jobHelper = new JobCubeHelper();
            int jobNo = 0;
            string jobStamp = null;
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
                        exitMsg = String.Format("參數錯誤，錯誤訊息：{0}", errmsg);
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);

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
                    string logMsg = null;
                    string summaryMsg = null;
                    Result result = jobHelper.ProcessSBMJob(retryTimes, retrySleep, out logMsg, out summaryMsg);
                    if (result.IsSuccess)
                    {
                        exitMsg = summaryMsg;
                        jobLog.AppendLine(logMsg);

                        log.AppendLine("發送繳費通知信成功。").AppendLine(logMsg);
                    }
                    else
                    {
                        exitCode = -3;
                        exitMsg = String.Format("發送繳費通知信失敗，錯誤訊息：{0}", result.Message);
                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
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
