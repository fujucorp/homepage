using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

using Entities;

namespace ClearDB
{
    /// <summary>
    /// 清除過期的暫存與日誌資料
    /// </summary>
    class Program
    {
        /// <summary>
        /// 清除過期的暫存與日誌資料，作業類別代碼固定為 CLDB，無須參數，不提供 retry 機制，同一時間 (天) 排程僅允許一個作業
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            #region Initial
            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);

            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = JobCubeTypeCodeTexts.CLDB;                   //作業類別代碼
            string jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);   //作業類別名稱

            int exitCode = 0;
            string exitMsg = null;
            #endregion

            DateTime startTime = DateTime.Now;

            JobCubeHelper jobHelper = new JobCubeHelper();

            int jobNo = 0;
            string jobStamp = null;
            StringBuilder jobLog = new StringBuilder();     //job 日誌紀錄
            StringBuilder fileLog = new StringBuilder();
            string logFileName = String.Format("{0}_{1:yyyyMMdd}.log", appName, startTime);

            try
            {
                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 開始", startTime, appName).AppendLine();

                #region 處理命令參數
                //無須參數
                #endregion

                #region 新增處理中的 Job
                if (exitCode == 0)
                {
                    JobcubeEntity job = new JobcubeEntity();
                    job.Jtypeid = jobTypeId;
                    job.Jparam = String.Empty;
                    Result result = jobHelper.InsertProcessJob(ref job, jobCheckMode);
                    if (result.IsSuccess)
                    {
                        jobNo = job.Jno;
                        jobStamp = job.Memo;
                    }
                    else
                    {
                        exitCode = -3;
                        exitMsg = String.Format("新增處理中的 Job 失敗，錯誤訊息：{0}", result.Message);
                        fileLog.AppendLine(exitMsg);
                    }
                }
                #endregion

                #region 處理資料
                if (exitCode == 0)
                {
                    string mutexName = "Global\\" + appGuid;
                    using (Mutex m = new Mutex(false, mutexName))    //全域不可重複執行
                    {
                        //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                        if (m.WaitOne(0, false))
                        {
                            using (EntityFactory factory = new EntityFactory())
                            {
                                object value = null;
                                KeyValue[] parameters = new KeyValue[1] {new KeyValue("JNO", jobNo)};
                                Result result = factory.ExecuteSPScalar("usp_ClearTempAndLog", parameters, out value);
                                if (result.IsSuccess)
                                {
                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 執行預存程序成功，回傳訊息：{1}", DateTime.Now, value).AppendLine();
                                    fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 執行預存程序成功，回傳訊息：{1}", DateTime.Now, value).AppendLine();
                                }
                                else
                                {
                                    exitCode = -4;
                                    exitMsg = "執行預存程序失敗，" + result.Message;
                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 執行預存程序失敗，錯誤訊息：{1}", DateTime.Now, result.Message).AppendLine();
                                    fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 執行預存程序失敗，錯誤訊息：{1}", DateTime.Now, result.Message).AppendLine();
                                }
                            }

                            #region [MDY:20210401] 原碼修正
                            m.ReleaseMutex();
                            #endregion
                        }
                        else
                        {
                            exitCode = -5;
                            exitMsg = String.Format("執行緒中已存在 {0}，不重複執行", mutexName);
                            jobLog.AppendLine(exitMsg);
                            fileLog.AppendLine(exitMsg);
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
                fileLog.AppendFormat("處理 {0} 作業發生例外，錯誤訊息：{1}", jobTypeName, ex.Message).AppendLine();
            }
            finally
            {
                #region 更新 Job
                if (jobNo > 0)
                {
                    string jobResultId = null;
                    if (exitCode == 0)
                    {
                        exitMsg = "清除過期的暫存與日誌資料成功";
                        jobResultId = JobCubeResultCodeTexts.SUCCESS;
                    }
                    else
                    {
                        jobResultId = JobCubeResultCodeTexts.FAILURE;
                    }

                    Result result = jobHelper.UpdateProcessJobToFinsh(jobNo, jobStamp, jobResultId, exitMsg, jobLog.ToString());
                    if (!result.IsSuccess)
                    {
                        fileLog.AppendFormat("更新批次處理佇列為已完成失敗，{0}", result.Message);
                    }
                }
                jobHelper.Dispose();
                jobHelper = null;
                #endregion

                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 結束", DateTime.Now, appName).AppendLine();

                WriteFileLog(logFileName, fileLog);

                System.Environment.Exit(exitCode);
            }
        }

        #region FileLoger 日誌檔
        static void WriteFileLog(string logFileName, StringBuilder log)
        {
            if (log == null || log.Length == 0 || String.IsNullOrWhiteSpace(logFileName))
            {
                return;
            }
            string logPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
            if (String.IsNullOrEmpty(logPath))
            {
                return;
            }

            try
            {
                DirectoryInfo info = new DirectoryInfo(logPath);
                if (!info.Exists)
                {
                    info.Create();
                }

                string logFileFullName = Path.Combine(logPath, logFileName);
                using (StreamWriter sw = new StreamWriter(logFileFullName, true, Encoding.Default))
                {
                    sw.WriteLine(log.ToString());
                    sw.Flush();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
