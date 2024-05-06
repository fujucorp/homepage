using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

using Fuju;
using Fuju.DB;

using Entities;

namespace GenCancelNo
{
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

        static void Main(string[] args)
        {
            #region Initial
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            FileLoger fileLog = new FileLoger(appName);
            string tempPath = null;
            {
                tempPath = ConfigurationManager.AppSettings.Get("TEMP_PATH");
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
            }
            int exitCode = 0;
            #endregion

            #region 處理參數
            //無參數
            #endregion

            #region 處理 CI 待處理序列資料
            string jobcubeType = JobCubeTypeCodeTexts.CI;
            //string stamp = Guid.NewGuid().ToString("N");
            string stamp = null;
            int jobNo = 0;
            DateTime endTime = DateTime.Now;
            string errmsg = null;
            StringBuilder log = new StringBuilder();
            try
            {
                #region 取得一筆待處理的 job
                JobcubeEntity job = null;
                {
                    Result result = null;
                    using (JobCubeHelper jobHelper = new JobCubeHelper())
                    {
                        result = jobHelper.GetWaitJobToProcess(jobcubeType, out job);
                    }
                    if (result.IsSuccess)
                    {
                        if (job == null)
                        {
                            exitCode = 0;
                            fileLog.WriteDebugLog("查無待處理的 {0} 批次處理佇列資料", jobcubeType);
                            System.Environment.Exit(exitCode);
                            return;
                        }
                    }
                    else
                    {
                        exitCode = -1;
                        errmsg = String.Format("查詢待處理的 {0} 批次處理佇列資料發生錯誤，錯誤訊息：{1}", jobcubeType, result.Message);
                        fileLog.WriteLog(errmsg);
                        System.Environment.Exit(exitCode);
                        return;
                    }
                }
                #endregion

                #region 產生銷帳編號
                if (job != null)
                {
                    jobNo = job.Jno;
                    stamp = job.Memo;
                    Result result = null;
                    string processLog = null;
                    Int32 totalCount = 0;
                    Int32 successCount = 0;

                    #region [MDY:20180414] 增加紀錄無虛擬帳號筆數
                    Int32 noATMCancelNoCount = 0;
                    using (JobCubeHelper helper = new JobCubeHelper())
                    {
                        result = helper.ProcessCIJob(job, out processLog, out totalCount, out successCount, out noATMCancelNoCount);
                        endTime = DateTime.Now;
                    }
                    if (result.IsSuccess)
                    {
                        exitCode = 0;
                        log
                            .AppendFormat("{0} 處理資料處理成功。", jobcubeType).AppendLine()
                            .AppendLine("處理日誌：")
                            .AppendFormat("共 {0} 筆資料，成功 {1} 筆 (其中 {3} 筆無虛擬帳號)，失敗 {2} 筆", totalCount, successCount, totalCount - successCount, noATMCancelNoCount).AppendLine()
                            .AppendLine(processLog);
                        fileLog.WriteDebugLog(log.ToString());
                    }
                    else
                    {
                        exitCode = -2;
                        errmsg = result.Message;
                        log
                            .AppendFormat("{0} 處理資料處理失敗。", jobcubeType).AppendLine()
                            .AppendFormat("錯誤訊息：{0}", result.Message).AppendLine()
                            .AppendLine("處理日誌：")
                            .AppendLine(processLog);
                        fileLog.WriteLog(log.ToString());
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -3;
                errmsg = String.Format("執行處理程序發生例外，錯誤訊息：{0}", ex.Message);
                log.AppendLine("執行處理程序發生例外");
                fileLog.WriteLog(errmsg);
            }
            finally
            {
                #region [Old]
                //#region 更新 job 處理結果 (避免資料更新臨時失敗，造成狀態未更新，試 3 次 每次睡 10 秒)
                //if (jobNo > 0)
                //{
                //    int sleepTimes = 3;
                //    int sleepSecond = 1000 * 10;
                //    StringBuilder updateError = new StringBuilder();
                //    using (JobCubeHelper jobHelper = new JobCubeHelper())
                //    {
                //        Result result = null;
                //        for (int timesNo = 1; timesNo <= sleepTimes; timesNo++)
                //        {
                //            if (String.IsNullOrEmpty(errmsg))
                //            {
                //                result = jobHelper.UpdateProcessJobToFinsh(jobNo, stamp, JobCubeResultCodeTexts.SUCCESS, JobCubeResultCodeTexts.SUCCESS_TEXT, log.ToString());
                //            }
                //            else
                //            {
                //                result = jobHelper.UpdateProcessJobToFinsh(jobNo, stamp, JobCubeResultCodeTexts.FAILURE, errmsg, log.ToString());
                //            }
                //            if (result.IsSuccess)
                //            {
                //                break;
                //            }
                //            else
                //            {
                //                updateError.AppendFormat("第 {0} 次更新編號 {1} 的處理中批次處理佇列資料為已結束發生錯誤，錯誤訊息：{2}", timesNo, jobNo, result.Message).AppendLine();
                //                if (result.Code == ErrorCode.D_DATA_NOT_FOUND || result.Code == ErrorCode.S_INVALID_FACTORY)
                //                {
                //                    break;
                //                }
                //            }
                //            System.Threading.Thread.Sleep(sleepSecond);
                //        }
                //    }
                //    if (updateError.Length > 0)
                //    {
                //        fileLog.WriteLog(updateError.ToString());
                //    }
                //}
                //#endregion
                #endregion

                #region 更新 Job
                if (jobNo > 0)
                {
                    using (JobCubeHelper jobHelper = new JobCubeHelper())
                    {
                        Result result = null;
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            result = jobHelper.UpdateProcessJobToFinsh(jobNo, stamp, JobCubeResultCodeTexts.SUCCESS, JobCubeResultCodeTexts.SUCCESS_TEXT, log.ToString());
                        }
                        else
                        {
                            result = jobHelper.UpdateProcessJobToFinsh(jobNo, stamp, JobCubeResultCodeTexts.FAILURE, errmsg, log.ToString());
                        }
                        if (!result.IsSuccess)
                        {
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 更新批次處理佇列為已完成失敗，{1}", DateTime.Now, result.Message).AppendLine();
                        }
                    }
                }
                #endregion
            }
            #endregion

            System.Environment.Exit(exitCode);
            return;
        }
    }
}
