using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Fuju;
using Fuju.Configuration;
using Fuju.DB;
using Fuju.DB.Data;

using Fuju.FTP;

using Entities;

namespace KP3Render
{
    /// <summary>
    /// 聯徵KP3資料匯出報送
    /// </summary>
    class Program
    {
        #region FileLoger 日誌檔類別
        /// <summary>
        /// 日誌檔類別
        /// </summary>
        private class FileLoger
        {
            #region Member
            /// <summary>
            /// 日誌內容暫存大小 (512 * 1024)
            /// </summary>
            private const int _BufferSize = 64 * 1024;

            /// <summary>
            /// 日誌內容暫存
            /// </summary>
            private StringBuilder _Buffer = new StringBuilder(_BufferSize + 1024);
            #endregion

            #region Property
            private string _LogName = null;
            /// <summary>
            /// 日誌名稱
            /// </summary>
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
            /// <summary>
            /// 日誌檔路徑
            /// </summary>
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

            /// <summary>
            /// 是否啟用除錯模式
            /// </summary>
            public bool IsDebug
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌檔完整路徑檔名
            /// </summary>
            public string LogFileName
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌序號
            /// </summary>
            public string LogSN
            {
                get;
                private set;
            }

            /// <summary>
            /// 取得日誌檔物件是否準備好
            /// </summary>
            public bool IsReady
            {
                get;
                private set;
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 日誌檔類別 物件，日誌檔路徑與是否啟用除錯模式取自 Config 的 appSettings 的 LOG_PATH 與 LOG_MDOE 參數設定
            /// </summary>
            /// <param name="logName">日誌名稱 (通常使用應用程式執行檔名稱)</param>
            public FileLoger(string logName)
            {
                this.LogName = logName;
                this.LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                string logMode = ConfigurationManager.AppSettings.Get("LOG_MDOE");
                if (String.IsNullOrWhiteSpace(logMode))
                {
                    this.IsDebug = false;
                }
                else
                {
                    this.IsDebug = (logMode.Trim().Equals("DEBUG", StringComparison.CurrentCultureIgnoreCase));
                }
                this.Initial();
            }

            /// <summary>
            /// 建構 日誌檔類別 物件
            /// </summary>
            /// <param name="logName">日誌名稱 (通常使用應用程式執行檔名稱)</param>
            /// <param name="logPath">日誌檔路徑</param>
            /// <param name="isDebug">是否啟用除錯模式</param>
            public FileLoger(string logName, string logPath, bool isDebug)
            {
                this.LogName = logName;
                this.LogPath = logPath;
                this.IsDebug = isDebug;
                this.Initial();
            }
            #endregion

            #region Private Method
            /// <summary>
            /// 日誌檔物件初始化
            /// </summary>
            /// <returns></returns>
            private string Initial()
            {
                this.LogSN = String.Concat("SN", DateTime.Now.Ticks.ToString("X16"));

                string errmsg = null;
                if (String.IsNullOrEmpty(this.LogPath))
                {
                    errmsg = "未指定日誌檔路徑";
                }
                else
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
                            this.LogFileName = Path.Combine(info.FullName, fileName);
                        }
                        else
                        {
                            string fileName = String.Format("{0}_{1:yyyyMMdd}.log", this.LogName, DateTime.Today);
                            this.LogFileName = Path.Combine(info.FullName, fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        errmsg = ex.Message;
                    }
                }
                this.IsReady = String.IsNullOrEmpty(errmsg);
                return errmsg;
            }

            /// <summary>
            /// 刷新日誌內容暫存
            /// </summary>
            private void RefreshBuffer()
            {
                if (_Buffer.Length > 0)
                {
                    this.WriteLogBuffer();
                    _Buffer.Clear();
                }
            }

            /// <summary>
            /// 將日誌內容暫存寫入日誌檔
            /// </summary>
            private void WriteLogBuffer()
            {
                if (this.IsReady && _Buffer != null && _Buffer.Length > 0)
                {
                    try
                    {
                        #region [MDY:20220618] Checkmarx 調整 (Information Exposure Through an Error Message)
                        #region [OLD] 土銀建議改用 File.AppendAllText()，都是寫檔有什麼差別，很奇怪
                        //using (StreamWriter sw = new StreamWriter(this.LogFileName, true, Encoding.Default))
                        //{
                        //    sw.WriteLine(String.Format(@"[日誌序號：{0}] ------\\", this.LogSN));
                        //    sw.WriteLine(_Buffer);
                        //    sw.WriteLine(String.Format(@"[日誌序號：{0}] ------//", this.LogSN));
                        //    sw.WriteLine();
                        //    sw.Flush();
                        //}
                        #endregion

                        string content = $@"
[日誌序號：{this.LogSN}] ------\\
{_Buffer.ToString()}
[日誌序號：{this.LogSN}] ------//
";
                        File.AppendAllText(this.LogFileName, content, Encoding.Default);
                        #endregion
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            /// <summary>
            /// 新增日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="logTime">是否包含日誌時間</param>
            /// <param name="msg">日誌資訊</param>
            /// <returns></returns>
            private FileLoger AppendLogLine(bool logTime, string msg)
            {
                if (this.IsReady)
                {
                    if (logTime)
                    {
                        _Buffer.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                    }
                    _Buffer.AppendLine(msg);
                    if (_Buffer.Length > _BufferSize)
                    {
                        this.RefreshBuffer();
                    }
                }
                return this;
            }

            /// <summary>
            /// 新增格式化的日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="logTime">是否包含日誌時間</param>
            /// <param name="format">格式化字串</param>
            /// <param name="args">日誌資訊陣列</param>
            /// <returns></returns>
            private FileLoger AppendLogFormatLine(bool logTime, string format, params object[] args)
            {
                if (this.IsReady)
                {
                    if (logTime)
                    {
                        _Buffer.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                    }
                    _Buffer.AppendFormat(format, args).AppendLine();
                    if (_Buffer.Length > _BufferSize)
                    {
                        this.RefreshBuffer();
                    }
                }
                return this;
            }
            #endregion

            #region Public Method
            /// <summary>
            /// 新增日誌資訊至日誌檔
            /// </summary>
            /// <param name="msg">日誌資訊</param>
            public FileLoger AppendLog(string msg)
            {
                if (this.IsReady)
                {
                    _Buffer.Append(msg);
                    if (_Buffer.Length > _BufferSize)
                    {
                        this.RefreshBuffer();
                    }
                }
                return this;
            }

            /// <summary>
            /// 新增包含日誌時間的日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="msg">日誌資訊</param>
            /// <returns></returns>
            public FileLoger AppendLogStartLine(string msg)
            {
                return this.AppendLogLine(true, msg);
            }

            /// <summary>
            /// 新增日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="msg">日誌資訊</param>
            public FileLoger AppendLogLine(string msg)
            {
                return this.AppendLogLine(false, msg);
            }

            /// <summary>
            /// 新增格式化的日誌資訊至日誌檔
            /// </summary>
            /// <param name="format">格式化字串</param>
            /// <param name="args">日誌資訊陣列</param>
            public FileLoger AppendLogFormat(string format, params object[] args)
            {
                if (this.IsReady)
                {
                    _Buffer.AppendFormat(format, args);
                    if (_Buffer.Length > _BufferSize)
                    {
                        this.RefreshBuffer();
                    }
                }
                return this;
            }

            /// <summary>
            /// 新增格式化的包含日誌時間的日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="format"></param>
            /// <param name="args"></param>
            /// <returns></returns>
            public FileLoger AppendLogFormatStartLine(string format, params object[] args)
            {
                return this.AppendLogFormatLine(true, format, args);
            }

            /// <summary>
            /// 新增格式化的日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="format">格式化字串</param>
            /// <param name="args">日誌資訊陣列</param>
            public FileLoger AppendLogFormatLine(string format, params object[] args)
            {
                return this.AppendLogFormatLine(false, format, args);
            }

            /// <summary>
            /// 新增除錯資訊至日誌檔並換行
            /// </summary>
            /// <param name="msg">除錯資訊</param>
            public FileLoger AppendDebugLine(string msg)
            {
                if (this.IsReady && this.IsDebug)
                {
                    this.AppendLogLine("[DEBUG] " + msg);
                }
                return this;
            }

            /// <summary>
            /// 新增格式化的除錯資訊至日誌檔並換行
            /// </summary>
            /// <param name="format">格式化字串</param>
            /// <param name="args">除錯資訊陣列</param>
            public FileLoger AppendDebugFormatLine(string format, params object[] args)
            {
                if (this.IsReady && this.IsDebug)
                {
                    this.AppendLogFormatLine("[DEBUG] " + format, args);
                }
                return this;
            }

            /// <summary>
            /// 將日誌內容暫存寫入日誌檔並清空暫存
            /// </summary>
            /// <returns></returns>
            public FileLoger Flush()
            {
                if (this.IsReady)
                {
                    this.RefreshBuffer();
                }
                return this;
            }
            #endregion
        }
        #endregion

        #region Feedback 結果資料承載類別
        /// <summary>
        /// Feedback 結果資料承載類別
        /// </summary>
        private class FeedbackResult
        {
            #region Property
            /// <summary>
            /// 報送KP3資料序號
            /// </summary>
            public string RenderSN
            {
                get;
                set;
            }

            /// <summary>
            /// 特約機構代號 (Item7)
            /// </summary>
            public string ReciUserNum
            {
                get;
                set;
            }

            /// <summary>
            /// 資料更新日期
            /// </summary>
            public string DataDate
            {
                get;
                set;
            }

            /// <summary>
            /// 回饋處理結果
            /// </summary>
            public string Result
            {
                get;
                set;
            }

            /// <summary>
            /// 報送處理結果
            /// </summary>
            public bool IsSuccess
            {
                get;
                set;
            }
            #endregion

            #region Constructor
            public FeedbackResult()
            {

            }
            #endregion
        }
        #endregion

        /// <summary>
        /// 下載並處理KP3回饋檔 然後 匯出並送出KP3報送檔
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>
        /// 參數說明：不分大小寫
        /// Feedback：執行回饋作業 （回饋檔下載 -> 回饋檔解析）
        /// Upload  ：執行報送作業 （報送檔匯出 -> 報送檔上傳）
        /// Feedback Upload：依序執行 回饋作業 與 報送作業
        /// </remarks>
        static void Main(string[] args)
        {
            #region Initial
            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);     //這個是組態的執行檔的名稱 (去掉副檔名)

            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = JobCubeTypeCodeTexts.KP3;                    //作業類別代碼
            string jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);   //作業類別名稱

            FileLoger fileLog = new FileLoger(appName);

            int exitCode = 0;
            string exitMsg = null;
            #endregion

            string argsLine = (args == null || args.Length == 0) ? String.Empty : String.Join(" ", args);

            JobCubeHelper jobHelper = new JobCubeHelper();
            int jobNo = 0;
            string jobStamp = null;
            StringBuilder jobLog = new StringBuilder();     //job 日誌紀錄

            try
            {
                fileLog.AppendLogFormatStartLine("{0} 開始", appName);

                #region 處理命令參數
                bool doFeedback = false;
                bool doUpload = false;
                if (exitCode == 0)
                {
                    fileLog.AppendLogFormatLine("命令參數：{0}", argsLine);

                    string errmsg = null;
                    if (args != null && args.Length > 0)
                    {
                        #region 拆解命令參數
                        foreach (string arg in args)
                        {
                            switch (arg.ToLower())
                            {
                                case "feedback":
                                    doFeedback = true;
                                    break;
                                case "upload":
                                    doUpload = true;
                                    break;
                                default:
                                    errmsg = String.Format("不支援 {0} 命令參數", arg);
                                    break;
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

                    #region 檢查必要命令參數
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        if (!doFeedback && !doUpload)
                        {
                            errmsg = "Feedback 與 Upload 參數至少要指定一種";
                        }
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -1;
                        exitMsg = String.Format("命令參數錯誤，錯誤訊息：{0}", errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);

                        if (args == null || args.Length == 0)
                        {
                            fileLog.AppendLogLine("命令參數語法：[Feedback | Upload]");
                        }
                    }
                }
                #endregion

                if (exitCode == 0)
                {
                    string mutexName = "Global\\" + appGuid;
                    using (Mutex m = new Mutex(false, mutexName))    //全域不可重複執行
                    {
                        //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                        if (m.WaitOne(0, false))
                        {
                            #region 新增處理中的 Job
                            if (exitCode == 0)
                            {
                                JobcubeEntity job = new JobcubeEntity();
                                job.Jtypeid = jobTypeId;
                                job.Jparam = argsLine;
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
                                    fileLog.AppendLogLine(exitMsg);
                                }
                            }
                            #endregion

                            KP3Helper kp3Helper = new KP3Helper();

                            #region 取得 KP3Config 與 HostConfig
                            KP3Config kp3Config = null;
                            HostConfig ftpConfig = null;
                            if (exitCode == 0)
                            {
                                string errmsg = kp3Helper.GetConfig(out kp3Config, out ftpConfig);
                                if (!String.IsNullOrEmpty(errmsg))
                                {
                                    exitCode = -4;
                                    exitMsg = errmsg;
                                    fileLog.AppendLogLine(exitMsg);
                                }
                            }
                            #endregion

                            if (exitCode == 0)
                            {
                                #region 執行回饋處理
                                Result feedbackResult = null;
                                if (doFeedback)
                                {
                                    feedbackResult = kp3Helper.DoFeedbackParse(kp3Config, ftpConfig, jobLog, fileLog);
                                }
                                #endregion

                                #region 執行報送作業
                                Result uploadResult = null;
                                if (doUpload)
                                {
                                    uploadResult = kp3Helper.DoExportUpload(kp3Config, ftpConfig, jobLog, fileLog);
                                }
                                #endregion

                                #region 處理 exitCode 與 exitMsg
                                if (feedbackResult != null && uploadResult != null)
                                {
                                    if (!feedbackResult.IsSuccess && !uploadResult.IsSuccess)
                                    {
                                        exitCode = -7;
                                    }
                                    else if (!feedbackResult.IsSuccess)
                                    {
                                        exitCode = -5;
                                    }
                                    else if (!uploadResult.IsSuccess)
                                    {
                                        exitCode = -6;
                                    }
                                    exitMsg = String.Concat(feedbackResult.Message, " 且 ", uploadResult.Message);
                                }
                                else if (feedbackResult != null)
                                {
                                    if (!feedbackResult.IsSuccess)
                                    {
                                        exitCode = -5;
                                    }
                                    exitMsg = feedbackResult.Message;
                                }
                                else if (uploadResult != null)
                                {
                                    if (!uploadResult.IsSuccess)
                                    {
                                        exitCode = -6;
                                    }
                                    exitMsg = uploadResult.Message;
                                }
                                else
                                {
                                    exitCode = -8;
                                    exitMsg = "執行參數不正確";
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            exitCode = -2;
                            exitMsg = String.Format("執行緒中已存在 {0}，不重複執行", mutexName);
                            jobLog.AppendLine(exitMsg);
                            fileLog.AppendLogLine(exitMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exitCode = -9;
                exitMsg = String.Format("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
                fileLog.AppendLogFormatStartLine("發生例外，命令參數：{0}，錯誤訊息：{1}", argsLine, ex.Message);
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
                        fileLog.AppendLogFormatStartLine("更新批次處理佇列為已完成失敗，{0}", result.Message);
                    }
                }
                jobHelper.Dispose();
                jobHelper = null;
                #endregion

                fileLog.AppendLogFormatStartLine("{0} 結束", appName);
                fileLog.Flush();

                System.Environment.Exit(exitCode);
            }
        }

        #region KP3 處理類別
        private class KP3Helper
        {
            #region Member
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 MyHelper 處理工具類別
            /// </summary>
            public KP3Helper()
            {

            }
            #endregion

            #region Private Method
            string BatchNoToString(int batchNo)
            {
                string text = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                if (batchNo > 0 && batchNo < text.Length)
                {
                    return text[batchNo].ToString();
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 產生 RenderSN (3碼的民國年+1碼Hex的月+2碼的日+2碼的時+2碼的分+5碼Hex的[秒x100000+毫秒])
            /// </summary>
            /// <returns></returns>
            private string GenRenderSN(DateTime now)
            {
                return String.Format("{0:000}{1:X}{2:00}{3:00}{4:00}{5:X5}"
                        , (now.Year - 1911)
                        , now.Month
                        , now.Day
                        , now.Hour
                        , now.Minute
                        , (now.Second * 100000 + now.Millisecond));
            }

            /// <summary>
            /// 解析回饋內容
            /// </summary>
            /// <param name="content"></param>
            /// <param name="result">傳回每筆資料的處理結果</param>
            /// <returns></returns>
            private string ParseFeedbackContent(string renderSN, string content, out List<KP3Entity> kp3Results, FileLoger fileLog)
            {
                kp3Results = null;

                try
                {
                    //[MEMO] 因為 <records> 只紀錄報送錯誤的錯誤訊息，不是所有 KP3 資料，所以要先取得該次報送的 KP3 資料
                    #region 取得該次報送的所有 KP3
                    KP3Entity[] kp3Datas = null;
                    using (EntityFactory factory = new EntityFactory())
                    {
                        Expression where = new Expression(KP3Entity.Field.RenderSN, renderSN);
                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1)
                        {
                            new KeyValue<OrderByEnum>(KP3Entity.Field.SN, OrderByEnum.Asc)
                        };
                        Result result = factory.SelectAll<KP3Entity>(where, orderbys, out kp3Datas);
                        if (!result.IsSuccess)
                        {
                            return String.Format("讀取 {0} 報送的 KP3 資料失敗。{1}", renderSN, result.Message);
                        }
                        if (kp3Datas == null || kp3Datas.Length == 0)
                        {
                            return String.Format("查無 {0} 報送的 KP3 資料。", renderSN);
                        }
                    }
                    kp3Results = new List<KP3Entity>(kp3Datas.Length);
                    #endregion

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(content);

                    XmlNode xRootNode = xDoc.DocumentElement.SelectSingleNode("records");
                    if (xRootNode == null)
                    {
                        return "回饋內容找不到 records 節點";
                    }

                    XmlNodeList xRecordNodes = xRootNode.HasChildNodes ? xRootNode.SelectNodes("record") : null;
                    if (xRecordNodes == null || xRecordNodes.Count == 0)
                    {
                        fileLog.AppendLogStartLine("回饋檔無 record 節點，所有報送資料都成功。");

                        #region 沒 record 表示沒錯誤，所有 KP3 都註記報送成功
                        foreach (KP3Entity kp3Result in kp3Datas)
                        {
                            if (kp3Result.Status == KP3StatusCodeTexts.STATUS20_CODE)
                            {
                                //狀態為 20 （匯出資料後待回饋）的資料才要
                                kp3Result.Status = KP3StatusCodeTexts.STATUS40_CODE;
                                kp3Result.FeedbackStatus = "Y";  //成功
                                kp3Result.FeedbackResult = "報送成功";
                                kp3Results.Add(kp3Result);
                                fileLog.AppendLogFormatLine("KP3 資料 ({0}) 特約機構代號 {1} 資料更新日期 {2} 報送成功", kp3Result.SN, kp3Result.Item07, kp3Result.Item40);
                            }
                            else
                            {
                                fileLog.AppendLogFormatLine("KP3 資料 ({0}) 特約機構代號 {1} 資料更新日期 {2} 狀態 {3} 不更新", kp3Result.SN, kp3Result.Item07, kp3Result.Item40, kp3Result.Status);
                            }
                        }
                        #endregion

                        return null;
                    }

                    #region 有 record 表示有錯誤，逐筆處理
                    foreach (XmlNode xRecordNode in xRecordNodes)
                    {
                        string recordNo = xRecordNode.Attributes["idx"].Value.Trim();  //這個指的是報送資料的順序，從 1 開始

                        #region errorCount [這個指的是錯誤數量，不是錯誤欄位數量]
                        int errorCount = 0;
                        {
                            XmlNode xErrorCount = xRecordNode.SelectSingleNode("errorCount");
                            if (xErrorCount == null && xErrorCount.NodeType != XmlNodeType.Element)
                            {
                                return String.Format("record {0} 節點下找不到 errorCount 子節點", recordNo);
                            }
                            string nodeText = xErrorCount.InnerText.Trim();
                            if (!Int32.TryParse(nodeText, out errorCount) || errorCount < 0)
                            {
                                return String.Format("record {0} 的 errorCount 節點值 ({1}) 不合法", recordNo, nodeText);
                            }
                        }
                        #endregion

                        #region warningCount
                        int warningCount = 0;
                        {
                            XmlNode xWarningCount = xRecordNode.SelectSingleNode("warningCount");
                            if (xWarningCount == null && xWarningCount.NodeType != XmlNodeType.Element)
                            {
                                return String.Format("record {0} 節點下找不到 warningCount 子節點", recordNo);
                            }
                            string nodeText = xWarningCount.InnerText.Trim();
                            if (!Int32.TryParse(nodeText, out warningCount) || warningCount < 0)
                            {
                                return String.Format("record {0} 的 warningCount 節點值 ({1}) 不合法", recordNo, nodeText);
                            }
                        }
                        #endregion

                        #region 檢查錯誤與警告數量
                        if (errorCount == 0 && warningCount == 0)
                        {
                            //不可能兩個都是 0
                            return "record {0} 節點下的 errorCount 與 warningCount 都是 0，無法判斷錯誤數量";
                        }
                        #endregion

                        #region 處理 fields/field 節點，取得特約機構代號、資料更新日期與錯誤訊息
                        string reciUserNum = null, dataDate = null;
                        StringBuilder sb = new StringBuilder();
                        {
                            XmlNodeList xFieldNodes = xRecordNode.SelectNodes("fields/field");
                            if (xFieldNodes == null || xFieldNodes.Count != 41)
                            {
                                return String.Format("record {0} 節點下找不到 fields/field 子節點", recordNo);
                            }

                            foreach (XmlNode xFieldNode in xFieldNodes)
                            {
                                #region id Node
                                XmlNode xIdNode = xFieldNode.SelectSingleNode("id");
                                if (xIdNode == null)
                                {
                                    return String.Format("record {0} 的 fields/field 節點下找不到 id 子節點", recordNo);
                                }
                                string itemNo = xIdNode.InnerText.Trim();
                                #endregion

                                #region value Node
                                XmlNode xValueNode = xFieldNode.SelectSingleNode("value");
                                if (xValueNode == null)
                                {
                                    return String.Format("record {0} 的 fields/field[id={1}] 節點下找不到 value 子節點", recordNo, itemNo);
                                }
                                string value = xValueNode.InnerText.Trim();
                                #endregion

                                #region status Node
                                XmlNode xStatusNode = xFieldNode.SelectSingleNode("status");
                                if (xStatusNode == null)
                                {
                                    return String.Format("record {0} 的 fields/field[id={1}] 節點下找不到 status 子節點", recordNo, itemNo);
                                }
                                string status = xStatusNode.InnerText.Trim();
                                #endregion

                                #region messages Node
                                if (status != "ok")
                                {
                                    sb.AppendFormat("{0} - {1}：", status, itemNo);

                                    XmlNodeList xStmtNodes = xFieldNode.SelectNodes("messages/message/stmt");
                                    if (xStmtNodes == null || xStmtNodes.Count == 0)
                                    {
                                        return String.Format("record {0} 的 fields/field[id={1}] 節點下找不到 messages/message/stmt 子節點", recordNo, itemNo);
                                    }
                                    foreach (XmlNode xStmtNode in xStmtNodes)
                                    {
                                        sb.AppendFormat("{0}。", xStmtNode.InnerXml.Trim());
                                    }
                                    sb.AppendLine();
                                }
                                #endregion

                                switch (itemNo)
                                {
                                    case "7":
                                        #region 特約機構代號
                                        if (String.IsNullOrEmpty(value))
                                        {
                                            return String.Format("record {0} 的特約機構代號 (fields/field[id={1}]/value) 子節點無資料", recordNo, itemNo);
                                        }
                                        else
                                        {
                                            reciUserNum = value;
                                        }
                                        #endregion
                                        break;
                                    case "40":
                                        #region 資料更新日期
                                        if (String.IsNullOrEmpty(value))
                                        {
                                            return String.Format("record {0} 的資料更新日期 (fields/field[id={1}]/value) 子節點無資料", recordNo, itemNo);
                                        }
                                        else
                                        {
                                            dataDate = value;
                                        }
                                        #endregion
                                        break;
                                }
                            }
                        }
                        if (String.IsNullOrEmpty(reciUserNum))
                        {
                            return String.Format("record {0} 的缺少特約機構代號 (fields/field[id=7]) 子節點", recordNo);
                        }
                        if (String.IsNullOrEmpty(dataDate))
                        {
                            return String.Format("record {0} 的缺少資料更新日期 (fields/field[id=40]) 子節點", recordNo);
                        }
                        #endregion

                        #region 找出對應的 KP3 資料，更新回饋處理相關欄位
                        KP3Entity kp3Result = kp3Datas.FirstOrDefault(x => x.Item07 == reciUserNum && x.Item40 == dataDate);
                        if (kp3Result == null)
                        {
                            return String.Format("找不到 record {0} 的對應的 KP3 資料 (特約機構代號={1}; 資料更新日期={2})", recordNo, reciUserNum, dataDate);
                        }
                        if (kp3Result.Status != KP3StatusCodeTexts.STATUS20_CODE)
                        {
                            fileLog.AppendLogFormatLine("KP3 資料 ({0}) 特約機構代號 {1} 資料更新日期 {2} 狀態 {3} 回饋狀態 {4} 回饋結果 {5} 覆寫回饋處理資訊", kp3Result.SN, kp3Result.Item07, kp3Result.Item40, kp3Result.Status, kp3Result.FeedbackStatus, kp3Result.FeedbackResult);
                        }
                        kp3Result.Status = KP3StatusCodeTexts.STATUS40_CODE;
                        kp3Result.FeedbackStatus = "N";  //失敗
                        kp3Result.FeedbackResult = sb.ToString();
                        kp3Results.Add(kp3Result);
                        fileLog.AppendLogFormatStartLine("KP3 資料 ({0}) 特約機構代號 {1} 資料更新日期 {2} 報送失敗。錯誤訊息：{3}", kp3Result.SN, kp3Result.Item07, kp3Result.Item40, kp3Result.FeedbackResult);
                        #endregion
                    }
                    #endregion

                    #region 其他不在 record 中的資料表示報送成功
                    if (kp3Results.Count != kp3Datas.Length)
                    {
                        foreach (KP3Entity kp3Data in kp3Datas)
                        {
                            if (kp3Results.FindIndex(x => x.SN == kp3Data.SN) < 0)
                            {
                                if (kp3Data.Status == KP3StatusCodeTexts.STATUS20_CODE)
                                {
                                    //狀態為 20 （匯出資料後待回饋）的資料才要
                                    kp3Data.Status = KP3StatusCodeTexts.STATUS40_CODE;
                                    kp3Data.FeedbackStatus = "Y";  //成功
                                    kp3Data.FeedbackResult = "報送成功";
                                    kp3Results.Add(kp3Data);
                                    fileLog.AppendLogFormatLine("KP3 資料 ({0}) 特約機構代號 {1} 資料更新日期 {2} 報送成功", kp3Data.SN, kp3Data.Item07, kp3Data.Item40);
                                }
                                else
                                {
                                    fileLog.AppendLogFormatLine("KP3 資料 ({0}) 特約機構代號 {1} 資料更新日期 {2} 狀態 {3} 不更新", kp3Data.SN, kp3Data.Item07, kp3Data.Item40, kp3Data.Status);
                                }
                            }
                        }
                    }
                    #endregion

                    return null;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            /// <summary>
            /// 更新 KP3 回饋處理結果
            /// </summary>
            /// <param name="kp3Result"></param>
            /// <param name="factory"></param>
            /// <param name="fileLog"></param>
            /// <returns></returns>
            private string UpdateKP3FeedbackResult(KP3Entity kp3Result, EntityFactory factory, FileLoger fileLog)
            {
                string errmsg = null;

                string sql = @"
UPDATE KP3
   SET Status = @STATUS
     , Feedback_Status = @FEEDBACK_STATUS
     , Feedback_Result = @FEEDBACK_RESULT
 WHERE SN = @SN
   AND Render_SN = @RENDER_SN
   AND Status = @FEEDBACK_WAIT_STATUS
   AND Item07 = @RECI_USER_NUM
".Trim();
                KeyValue[] parameters = new KeyValue[7]
                {
                    new KeyValue("@STATUS", KP3StatusCodeTexts.STATUS40_CODE),
                    new KeyValue("@FEEDBACK_STATUS", kp3Result.FeedbackStatus),
                    new KeyValue("@FEEDBACK_RESULT", kp3Result.FeedbackResult.Length > 100 ? kp3Result.FeedbackResult.Substring(0, 96) + " ..." : kp3Result.FeedbackResult),

                    new KeyValue("@SN", kp3Result.SN),
                    new KeyValue("@RENDER_SN", kp3Result.RenderSN),
                    new KeyValue("@FEEDBACK_WAIT_STATUS", KP3StatusCodeTexts.STATUS20_CODE),
                    new KeyValue("@RECI_USER_NUM", kp3Result.Item07)
                };
                int count = 0;
                Result result = factory.ExecuteNonQuery(sql, parameters, out count);
                if (result.IsSuccess)
                {
                    if (fileLog != null)
                    {
                        fileLog.AppendLogFormatStartLine("更新報送資料 ({0}) 回饋處理結果成功。共 {1} 筆", kp3Result.SN, count);
                    }
                    return null;
                }
                else
                {
                    errmsg = String.Format("更新報送資料 ({0}) 回饋處理結果失敗，{1}", kp3Result.SN, result.Message);
                    if (fileLog != null)
                    {
                        fileLog.AppendLogStartLine(errmsg);
                    }
                    return errmsg;
                }
            }
            #endregion

            #region Public Method
            /// <summary>
            /// 取得 KP3Config 與 HostConfig 資料
            /// </summary>
            /// <param name="kp3Config"></param>
            /// <param name="ftpConfig"></param>
            /// <returns></returns>
            public string GetConfig(out KP3Config kp3Config, out HostConfig ftpConfig)
            {
                kp3Config = null;
                ftpConfig = null;

                ConfigEntity configData = null;
                Result result = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, KP3Config.RootLocalName);
                using (EntityFactory factory = new EntityFactory())
                {
                    result = factory.SelectFirst<ConfigEntity>(where, null, out configData);
                }
                if (result.IsSuccess)
                {
                    if (configData == null)
                    {
                        return "缺少KP3參數設定資料，請先設定";
                    }
                    else
                    {
                        kp3Config = KP3Config.Create(configData.ConfigValue);
                        if (kp3Config == null)
                        {
                            return "KP3參數設定資料解讀失敗";
                        }
                        else
                        {
                            bool isSSL = false;
                            bool isSFTP = false;
                            bool isOK = false;
                            Uri uri = null;
                            if (kp3Config.FTPUrl.StartsWith("ftps://", StringComparison.CurrentCultureIgnoreCase))
                            {
                                isSSL = true;
                                isOK = Uri.TryCreate("ftp" + kp3Config.FTPUrl.Substring(4), UriKind.Absolute, out uri);
                            }
                            else if (kp3Config.FTPUrl.StartsWith("sftp://", StringComparison.CurrentCultureIgnoreCase))
                            {
                                isSFTP = true;
                                isOK = Uri.TryCreate("ftp" + kp3Config.FTPUrl.Substring(4), UriKind.Absolute, out uri);
                            }
                            else
                            {
                                isOK = Uri.TryCreate(kp3Config.FTPUrl, UriKind.Absolute, out uri);
                            }

                            #region [MDY:20220530] Checkmarx 調整
                            if (isOK && !String.IsNullOrEmpty(kp3Config.FTPAcct) && !String.IsNullOrEmpty(kp3Config.FTPPXX))
                            {
                                if (isSFTP)
                                {
                                    ftpConfig = HostConfig.CreateBySFTP(uri.Host, kp3Config.FTPAcct, kp3Config.FTPPXX, uri.Port, workingDirectory: uri.PathAndQuery);
                                }
                                else
                                {
                                    ftpConfig = HostConfig.CreateByFTPx(uri.Host, kp3Config.FTPAcct, kp3Config.FTPPXX, uri.Port, isSSL, workingDirectory: uri.PathAndQuery);
                                }
                                return null;
                            }
                            else
                            {
                                return "缺少 FTP相關設定或設定不正確";
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    return String.Format("讀取KP3參數設定資料失敗。{0}", result.Message);
                }
            }

            /// <summary>
            /// 執行回饋作業
            /// </summary>
            /// <param name="kp3Config"></param>
            /// <param name="ftpConfig"></param>
            /// <param name="jobLog"></param>
            /// <param name="fileLog"></param>
            /// <returns></returns>
            public Result DoFeedbackParse(KP3Config kp3Config, HostConfig ftpConfig, StringBuilder jobLog, FileLoger fileLog)
            {
                Result doResult = null;
                List<string> failFileNames = new List<string>();
                try
                {
                    DateTime now = DateTime.Now;

                    #region 戳記要下載的回饋資料後取回
                    KP3RenderLogEntity[] kp3RenderLogs = null;
                    {
                        using (EntityFactory factory = new EntityFactory())
                        {
                            #region 戳記資料
                            int count = 0;
                            {
                                string sql = @"
UPDATE KP3_Render_Log
   SET Status = @FEEDBACKING_STATUS
     , Feedback_Date = @FEEDBACK_DATE
     , Feedback_Status = '1'
     , Feedback_Result = @STAMP
 WHERE Status = @FEEDBACK_WAIT_STATUS
   AND Feedback_Status = '0'
   AND (Feedback_File_Name IS NOT NULL AND Feedback_File_Name != '')
".Trim();
                                KeyValue[] parameters = new KeyValue[4]
                                {
                                    new KeyValue("@FEEDBACKING_STATUS", KP3RenderStatusCodeTexts.STATUS41_CODE),
                                    new KeyValue("@FEEDBACK_DATE", now),
                                    new KeyValue("@STAMP", Environment.MachineName),
                                    new KeyValue("@FEEDBACK_WAIT_STATUS", KP3RenderStatusCodeTexts.STATUS30_CODE)
                                };
                                Result result = factory.ExecuteNonQuery(sql, parameters, out count);
                                if (result.IsSuccess)
                                {
                                    fileLog.AppendLogFormatStartLine("戳記要回饋的報送日誌成功。共 {0} 筆", count);
                                }
                                else
                                {
                                    string errmsg = String.Format("戳記要回饋的報送日誌失敗，{0}", result.Message);
                                    jobLog.AppendLine(errmsg);
                                    fileLog.AppendLogStartLine(errmsg);
                                    return (doResult = new Result(false, errmsg, null, null));
                                }
                            }
                            #endregion

                            #region 取回資料
                            {
                                Thread.Sleep(800);  //小睡 0.8 秒
                                Expression where = new Expression(KP3RenderLogEntity.Field.Status, KP3RenderStatusCodeTexts.STATUS41_CODE)
                                   .And(KP3RenderLogEntity.Field.FeedbackStatus, "1")                       //0=待處理; 1=處理中 2=成功; 3=失敗
                                   .And(KP3RenderLogEntity.Field.FeedbackResult, Environment.MachineName);
                                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                                orderbys.Add(KP3RenderLogEntity.Field.UploadDate, OrderByEnum.Asc);
                                Result result = factory.SelectAll<KP3RenderLogEntity>(where, orderbys, out kp3RenderLogs);
                                if (result.IsSuccess)
                                {
                                    int dataCount = kp3RenderLogs == null ? 0 : kp3RenderLogs.Length;
                                    fileLog.AppendLogFormatStartLine("取回戳記報送日誌成功。共 {0} 筆", dataCount);
                                    if (dataCount != count)
                                    {
                                        fileLog.AppendDebugFormatLine("注意：戳記的報送日誌數 ({0}) 與取得的報送日誌數 ({1}) 不同", count, dataCount);
                                    }
                                    if (dataCount == 0)
                                    {
                                        jobLog.AppendLine("今日無需要回饋處理的報送資料");
                                        fileLog.AppendLogStartLine("今日無需要回饋處理的報送資料");
                                        return (doResult = new Result(true, "今日無需要回饋處理的報送資料", null, null));
                                    }
                                }
                                else
                                {
                                    string errmsg = String.Format("讀取戳記的報送日誌失敗，{0}", result.Message);
                                    jobLog.AppendLine(errmsg);
                                    fileLog.AppendLogStartLine(errmsg);
                                    return (doResult = new Result(false, errmsg, null, null));
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 逐筆 FTP 下載、解析並寫回資料表
                    {
                        int okCount = 0;
                        Encoding big5 = Encoding.GetEncoding("big5");
                        using (EntityFactory factory = new EntityFactory())
                        {
                            FTPHelper ftpHelper = new FTPHelper();
                            foreach (KP3RenderLogEntity kp3RenderLog in kp3RenderLogs)
                            {
                                try
                                {
                                    Result downloadResult = null;  //FTP下載且寫回資料表成功才算成功

                                    #region 下載檔案
                                    //[MEMO] 檔案不存在或下載失敗，都還原戳記
                                    kp3RenderLog.FeedbackFileContent = null;
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        string errmsg = ftpHelper.DownloadFile(ftpConfig, kp3RenderLog.FeedbackFileName, ms);
                                        if (String.IsNullOrEmpty(errmsg))
                                        {
                                            ms.Position = 0;
                                            kp3RenderLog.FeedbackFileContent = big5.GetString(ms.ToArray());
                                            downloadResult = new Result(true);
                                            fileLog
                                                .AppendLogFormatStartLine("下載 {0} 檔案成功。", kp3RenderLog.FeedbackFileName)
                                                .AppendLogFormatLine("檔案內容：\r\n{0}", kp3RenderLog.FeedbackFileContent);
                                        }
                                        else
                                        {
                                            downloadResult = new Result(errmsg);
                                            failFileNames.Add(kp3RenderLog.FeedbackFileName);
                                            jobLog.AppendFormat("下載 {0} 檔案失敗。{1}", kp3RenderLog.FeedbackFileName, errmsg).AppendLine();
                                            fileLog.AppendLogFormatStartLine("下載 {0} 檔案失敗。{1}", kp3RenderLog.FeedbackFileName, errmsg);
                                        }
                                    }
                                    #endregion

                                    #region 更新 KP3_Render_Log 回饋處理結果（不管下載成功或失敗都要處理日誌檔，成功只更新回饋內容與回饋結果，失敗則還原戳記）
                                    {
                                        if (downloadResult.IsSuccess)
                                        {
                                            string sql = @"
UPDATE KP3_Render_Log
   SET Feedback_Result = @FEEDBACK_RESULT
     , Feedback_File_Content = @FEEDBACK_FILE_CONTENT
 WHERE SN = @SN
   AND Status = @FEEDBACKING_STATUS
   AND Feedback_Status = '1'
".Trim();
                                            KeyValueList parameters = new KeyValueList(4);
                                            parameters.Add("@FEEDBACK_RESULT", "下載檔案成功");
                                            parameters.Add("@FEEDBACK_FILE_CONTENT", kp3RenderLog.FeedbackFileContent);
                                            parameters.Add("@SN", kp3RenderLog.SN);
                                            parameters.Add("@FEEDBACKING_STATUS", KP3RenderStatusCodeTexts.STATUS41_CODE);

                                            int count = 0;
                                            //[MEMO] 更新失敗也要還原戳記，以便下次重新處理
                                            downloadResult = factory.ExecuteNonQuery(sql, parameters, out count);
                                            if (downloadResult.IsSuccess)
                                            {
                                                //kp3Render.Status = KP3RenderStatusCodeTexts.STATUS40_CODE;
                                                kp3RenderLog.FeedbackResult = "下載檔案成功";
                                                fileLog.AppendLogFormatStartLine("更新報送日誌回饋處理結果成功。共 {0} 筆", count);
                                            }
                                            else
                                            {
                                                string errmsg2 = String.Format("更新報送日誌回饋處理結果失敗，{0}", downloadResult.Message);
                                                jobLog.AppendLine(errmsg2);
                                                fileLog.AppendLogStartLine(errmsg2);

                                                #region 發送失敗通知信
                                                {
                                                    this.SendFailMail(kp3Config.GetManagers(), String.Format("{0} 的回饋作業處理", kp3RenderLog.SN), downloadResult.Message, fileLog);
                                                }
                                                #endregion

                                                Thread.Sleep(30 * 1000);  //更新資料失敗就睡 30 秒，以避開資料連線暫時失效的問題
                                            }
                                        }
                                        if (!downloadResult.IsSuccess)
                                        {
                                            string sql = @"
UPDATE KP3_Render_Log
   SET Status = @STATUS
     , Feedback_Status = @FEEDBACK_STATUS
     , Feedback_Result = @FEEDBACK_RESULT
     , Feedback_File_Content = @FEEDBACK_FILE_CONTENT
 WHERE SN = @SN
   AND Status = @FEEDBACKING_STATUS
   AND Feedback_Status = '1'
".Trim();
                                            //下載失敗或寫回日誌失敗，就還原戳記，然後換下一筆
                                            KeyValueList parameters = new KeyValueList(6);
                                            parameters.Add("@STATUS", KP3RenderStatusCodeTexts.STATUS30_CODE);
                                            parameters.Add("@FEEDBACK_STATUS", "0");  //0=待處理; 1=處理中 2=成功; 3=失敗
                                            parameters.Add("@FEEDBACK_RESULT", "檔案不存在");
                                            parameters.Add("@FEEDBACK_FILE_CONTENT", kp3RenderLog.FeedbackFileContent);

                                            parameters.Add("@SN", kp3RenderLog.SN);
                                            parameters.Add("@FEEDBACKING_STATUS", KP3RenderStatusCodeTexts.STATUS41_CODE);

                                            int count = 0;
                                            Result result2 = factory.ExecuteNonQuery(sql, parameters, out count);
                                            if (result2.IsSuccess)
                                            {
                                                fileLog.AppendLogFormatStartLine("還原戳記回饋的報送日誌成功。共 {0} 筆", count);
                                            }
                                            else
                                            {
                                                string errmsg2 = String.Format("還原戳記回饋的報送日誌失敗，{0}", result2.Message);
                                                jobLog.AppendLine(errmsg2);
                                                fileLog.AppendLogStartLine(errmsg2);
                                            }

                                            continue;//此筆資料無法處理，換下一筆
                                        }
                                    }
                                    #endregion

                                    #region 處理回饋內容
                                    if (downloadResult.IsSuccess)
                                    {
                                        string errmsg = null;

                                        #region 解析回饋內容
                                        List<KP3Entity> kp3Results;
                                        {
                                            errmsg = this.ParseFeedbackContent(kp3RenderLog.SN, kp3RenderLog.FeedbackFileContent, out kp3Results, fileLog);
                                            if (String.IsNullOrEmpty(errmsg))
                                            {
                                                kp3RenderLog.FeedbackStatus = "2";  //0=待處理; 1=處理中 2=成功; 3=失敗
                                                kp3RenderLog.FeedbackResult = "回饋檔案處理成功";
                                            }
                                            else
                                            {
                                                kp3RenderLog.FeedbackStatus = "3";  //0=待處理; 1=處理中 2=成功; 3=失敗
                                                kp3RenderLog.FeedbackResult = errmsg;
                                            }
                                            kp3RenderLog.Status = KP3RenderStatusCodeTexts.STATUS40_CODE;  //不管解析成功或失敗，本次的報送都算結束
                                        }
                                        #endregion

                                        #region 處理解析結果
                                        if (String.IsNullOrEmpty(errmsg))
                                        {
                                            #region 解析成功則逐筆更新報送資料的回饋處理結果
                                            int failCount = 0;
                                            foreach (KP3Entity kp3Result in kp3Results)
                                            {
                                                string errmsg2 = this.UpdateKP3FeedbackResult(kp3Result, factory, fileLog);
                                                if (!String.IsNullOrEmpty(errmsg2))
                                                {
                                                    jobLog.AppendFormat("KP3 資料 ({0}) 更新回饋處理結果失敗。{1}", kp3Result.SN, errmsg2).AppendLine();
                                                    fileLog.AppendLogFormatStartLine("KP3 資料 ({0}) 更新回饋處理結果失敗。{1}", kp3Result.SN, errmsg2);
                                                    failCount++;
                                                }

                                                #region 發送 回饋處理結果 通知信
                                                if (!this.SendFeedbackMail(kp3Config.GetManagers(), kp3Result, fileLog))
                                                {
                                                    jobLog.AppendFormat("發送 KP3 ({0}) 的回饋處理結果通知信失敗", kp3Result.SN);
                                                }
                                                #endregion
                                            }
                                            #endregion

                                            if (failCount > 0)
                                            {
                                                #region 發送失敗通知信
                                                this.SendFailMail(kp3Config.GetManagers(), String.Format("{0} 的回饋作業處理", kp3RenderLog.SN), String.Format("{0} 筆報送資料回饋處理結果更新失敗。請查看日誌", failCount), fileLog);
                                                #endregion
                                            }
                                            else
                                            {
                                                okCount++;
                                            }
                                        }
                                        else
                                        {
                                            jobLog.AppendFormat("解析回饋檔 {0} 內容失敗。{1}", kp3RenderLog.FeedbackFileName, errmsg).AppendLine();
                                            fileLog.AppendLogFormatStartLine("解析回饋檔 {0} 內容失敗。{1}", kp3RenderLog.FeedbackFileName, errmsg);

                                            //需要人工介入，所以不更新報送資料的回饋處理結果
                                            #region 解析失敗則發送失敗通知信
                                            this.SendFailMail(kp3Config.GetManagers(), String.Format("{0} 的回饋作業處理", kp3RenderLog.SN), String.Format("解析回饋檔 {0} 內容失敗。{1}", kp3RenderLog.FeedbackFileName, errmsg), fileLog);
                                            #endregion
                                        }
                                        #endregion

                                        #region 不管解析重功或失敗，本次的報送都算結束，一律更新回饋處理結果
                                        {
                                            string sql = @"
UPDATE KP3_Render_Log
   SET Status=  @STATUS
     , Feedback_Status = @FEEDBACK_STATUS
     , Feedback_Result = @FEEDBACK_RESULT
 WHERE SN = @RENDER_SN
   AND Status = @FEEDBACK_WAIT_STATUS
   AND Feedback_Status = '1'
".Trim();
                                            KeyValue[] parameters = new KeyValue[5]
                                            {
                                                new KeyValue("@STATUS", kp3RenderLog.Status),
                                                new KeyValue("@FEEDBACK_STATUS", kp3RenderLog.FeedbackStatus),
                                                new KeyValue("@FEEDBACK_RESULT", (kp3RenderLog.FeedbackResult.Length > 100 ? kp3RenderLog.FeedbackResult.Substring(0, 96) + " ..." : kp3RenderLog.FeedbackResult)),
                                                new KeyValue("@RENDER_SN", kp3RenderLog.SN),
                                                new KeyValue("@FEEDBACK_WAIT_STATUS", KP3RenderStatusCodeTexts.STATUS41_CODE),
                                            };
                                            int count = 0;
                                            Result result = factory.ExecuteNonQuery(sql, parameters, out count);
                                            if (result.IsSuccess)
                                            {
                                                fileLog.AppendLogFormatStartLine("更新報送日誌回饋處理結果成功。共 {0} 筆", count);
                                            }
                                            else
                                            {
                                                string errmsg2 = String.Format("更新報送日誌回饋處理結果失敗，{0}", result.Message);
                                                jobLog.AppendLine(errmsg2);
                                                fileLog.AppendLogStartLine(errmsg2);
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                catch (Exception ex2)
                                {
                                    string errmsg2 = String.Format("執行 {0} 的回饋作業發生例外，{1}", kp3RenderLog.SN, ex2.Message);
                                    jobLog.AppendLine(errmsg2);
                                    fileLog
                                        .AppendLogLine(errmsg2)
                                        .AppendLogFormatLine(@"例外訊息：
{0}", ex2);
                                    if (ex2.InnerException != null)
                                    {
                                        fileLog.AppendLogFormatLine(@"Inner 例外訊息：
{0}", ex2.InnerException);
                                    }

                                    #region 發送失敗通知信
                                    {
                                        this.SendFailMail(kp3Config.GetManagers(), String.Format("{0} 的回饋作業處理", kp3RenderLog.SN), errmsg2, fileLog);
                                    }
                                    #endregion
                                }
                            }
                        }
                        if (okCount == 0)
                        {
                            doResult = new Result("回饋作業執行失敗。");
                        }
                        else
                        {
                            doResult = new Result(true, "回饋作業執行完成。", null, null);
                        }
                    }
                    #endregion

                    return doResult;
                }
                catch (Exception ex)
                {
                    string errmsg = String.Format("執行回饋作業發生例外，{0}", ex.Message);
                    jobLog.AppendLine(errmsg);
                    fileLog
                        .AppendLogLine(errmsg)
                        .AppendLogFormatLine(@"例外訊息：
{0}", ex);
                    if (ex.InnerException != null)
                    {
                        fileLog.AppendLogFormatLine(@"Inner 例外訊息：
{0}", ex.InnerException);
                    }
                    return (doResult = new Result(false, errmsg, null, null));
                }
                finally
                {
                    if (doResult != null && !doResult.IsSuccess)
                    {
                        #region 發送失敗通知信
                        {
                            this.SendFailMail(kp3Config.GetManagers(), "回饋作業處理", doResult.Message, fileLog);
                        }
                        #endregion
                    }
                }
            }

            /// <summary>
            /// 執行報送作業
            /// </summary>
            /// <param name="kp3Config"></param>
            /// <param name="ftpConfig"></param>
            /// <param name="jobLog"></param>
            /// <param name="fileLog"></param>
            /// <returns></returns>
            public Result DoExportUpload(KP3Config kp3Config, HostConfig ftpConfig, StringBuilder jobLog, FileLoger fileLog)
            {
                Result doResult = null;
                string renderSN = null;
                int rollbackFlag = 0;
                try
                {
                    DateTime now = DateTime.Now;
                    string renderDate = String.Format("{0:000}{1:MMdd}", (now.Year - 1911), now);
                    renderSN = this.GenRenderSN(now);

                    #region 戳記要匯出的報送資料後取回並取得報送日批號
                    KP3Entity[] kp3Datas = null;
                    int renderBatchNo = 0;
                    {
                        using (EntityFactory factory = new EntityFactory())
                        {
                            #region 戳記資料
                            int count = 0;
                            {
                                string sql = @"
UPDATE KP3
   SET Status = @EXPORTING_STATUS
     , Render_SN = @RENDER_SN
     , Feedback_Result = @STAMP
 WHERE Status = @WAIT_STATUS
   AND (Render_SN IS NULl OR Render_SN = '')
   AND (Feedback_Result IS NULL OR Feedback_Result = '')
".Trim();
                                KeyValue[] parameters = new KeyValue[4]
                                {
                                    new KeyValue("@EXPORTING_STATUS", KP3StatusCodeTexts.STATUS21_CODE),
                                    new KeyValue("@RENDER_SN", renderSN),
                                    new KeyValue("@STAMP", Environment.MachineName),
                                    new KeyValue("@WAIT_STATUS", KP3StatusCodeTexts.STATUS10_CODE)
                                };
                                Result result = factory.ExecuteNonQuery(sql, parameters, out count);
                                if (result.IsSuccess)
                                {
                                    rollbackFlag = 1;
                                    fileLog.AppendLogFormatStartLine("戳記匯出的報送資料成功。共 {0} 筆", count);
                                }
                                else
                                {
                                    string errmsg = String.Format("戳記匯出的報送資料失敗，{0}", result.Message);
                                    jobLog.AppendLine(errmsg);
                                    fileLog.AppendLogStartLine(errmsg);
                                    return (doResult = new Result(false, errmsg, null, null));
                                }
                            }
                            #endregion

                            #region 取回資料
                            {
                                Thread.Sleep(800);  //小睡 0.8 秒
                                Expression where = new Expression(KP3Entity.Field.Status, KP3StatusCodeTexts.STATUS21_CODE)
                                   .And(KP3Entity.Field.RenderSN, renderSN)
                                   .And(KP3Entity.Field.FeedbackResult, Environment.MachineName);
                                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                                orderbys.Add(KP3Entity.Field.CreateDate, OrderByEnum.Asc);
                                Result result = factory.SelectAll<KP3Entity>(where, orderbys, out kp3Datas);
                                if (result.IsSuccess)
                                {
                                    int dataCount = kp3Datas == null ? 0 : kp3Datas.Length;
                                    fileLog.AppendLogFormatStartLine("取回戳記的報送資料成功。共 {0} 筆", dataCount);
                                    if (dataCount != count)
                                    {
                                        fileLog.AppendDebugFormatLine("注意：戳記的報送資料數 ({0}) 與取得的報送資料數 ({1}) 不同", count, dataCount);
                                    }
                                    if (dataCount == 0)
                                    {
                                        jobLog.AppendLine("今日無需要報送處理的報送資料");
                                        fileLog.AppendLogStartLine("今日無需要報送處理的報送資料");
                                        return (doResult = new Result(true, "今日無需要報送處理的報送資料", null, null));
                                    }
                                }
                                else
                                {
                                    string errmsg = String.Format("讀取戳記的報送資料失敗，{0}", result.Message);
                                    jobLog.AppendLine(errmsg);
                                    fileLog.AppendLogStartLine(errmsg);
                                    return (doResult = new Result(false, errmsg, null, null));
                                }
                            }
                            #endregion

                            #region 取得報送日批號
                            {
                                string sql = "SELECT ISNULL(MAX(Render_Batch_No) + 1, 1) AS NEXT_BATCH_NO FROM KP3_Render_Log WHERE Render_Date = @RENDER_DATE ";
                                KeyValue[] parameters = new KeyValue[1] { new KeyValue("@RENDER_DATE", renderDate) };
                                object value = null;
                                Result result = factory.ExecuteScalar(sql, parameters, out value);
                                if (result.IsSuccess)
                                {
                                    renderBatchNo = Convert.ToInt32(value);
                                    if (renderBatchNo > 25)
                                    {
                                        string errmsg = "本日報送批號已用完";
                                        jobLog.AppendLine(errmsg);
                                        fileLog.AppendLogStartLine(errmsg);
                                        return (doResult = new Result(false, errmsg, null, null));
                                    }
                                    fileLog.AppendLogFormatStartLine("取得本次報送批號成功（{0}）", renderBatchNo);
                                }
                                else
                                {
                                    string errmsg = String.Format("取得本次報送批號失敗，{0}", result.Message);
                                    jobLog.AppendLine(errmsg);
                                    fileLog.AppendLogStartLine(errmsg);
                                    return (doResult = new Result(false, errmsg, null, null));
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 匯出報送檔案內容並新增報送日誌
                    StringBuilder fileContent = new StringBuilder();
                    string renderFileName = String.Format("{0}{1:MMdd}{2}.kp3", kp3Config.Unit, now, this.BatchNoToString(renderBatchNo));
                    {
                        #region 檔頭
                        //1  資訊格式代號      X(18)  1-18
                        //2  報送單位代號      X(3)   19-21
                        //3  FILLER            X(5)   22-26
                        //4  報送日期          X(7)   27-33
                        //5  本日檔案序號      X(2)   34-35
                        //6  FILLER            X(10)  36-45
                        //7  聯絡電話          X(16)  46-61
                        //8  聯絡人資訊或訊息  X(80)  62-141
                        fileContent
                            .Append(kp3Config.HeadItem01.PadRight(18, ' '))
                            .Append(kp3Config.Unit.PadRight(3, ' '))
                            .Append("".PadRight(5, ' '))
                            .Append(renderDate)
                            .AppendFormat("{0:00}", renderBatchNo)
                            .Append("".PadRight(10, ' '))
                            .Append(kp3Config.HeadItem07.PadRight(16, ' '))
                            .Append(kp3Config.HeadItem08.PadRight(80, ' '))
                            .AppendLine();
                        #endregion

                        #region 檔身
                        foreach (KP3Entity kp3Data in kp3Datas)
                        {
                            string dataLine = kp3Data.GetDataLine();
                            if (String.IsNullOrEmpty(dataLine))
                            {
                                string errmsg = String.Format("特約機構 {0} 的資料不完整", kp3Data.Item07);
                                jobLog.AppendLine(errmsg);
                                fileLog.AppendLogStartLine(errmsg);
                                return (doResult = new Result(false, errmsg, null, null));
                            }
                            fileContent.AppendLine(dataLine);
                        }
                        #endregion

                        #region 檔尾
                        //1  末筆標示    X(4)  1-4
                        //2  資料總筆數  X(8)  5-12
                        fileContent
                            .Append("TRLR")
                            .AppendFormat("{0:00000000}", kp3Datas.Length)
                            .AppendLine();
                        #endregion

                        fileLog.AppendLogStartLine("匯出報送檔案內容成功");

                        KP3RenderLogEntity renderLog = new KP3RenderLogEntity();
                        renderLog.SN = renderSN;
                        renderLog.RenderDate = renderDate;
                        renderLog.RenderBatchNo = renderBatchNo;
                        renderLog.RenderFileName = renderFileName;
                        renderLog.RenderFileContent = fileContent.ToString();
                        renderLog.Status = KP3RenderStatusCodeTexts.STATUS31_CODE;
                        renderLog.UploadDate = DateTime.Now;
                        renderLog.UploadStatus = "1";  //處理中
                        renderLog.UploadResult = Environment.MachineName;  //戳記
                        renderLog.FeedbackDate = null;
                        renderLog.FeedbackFileName = renderLog.RenderFileName + ".xml";
                        renderLog.FeedbackFileContent = null;
                        renderLog.FeedbackStatus = "0";
                        renderLog.FeedbackResult = null;

                        Result result = null;
                        int count = 0;
                        using (EntityFactory factory = new EntityFactory())
                        {
                            result = factory.Insert(renderLog, out count);
                        }
                        if (result.IsSuccess)
                        {
                            rollbackFlag = 2;
                            fileLog.AppendLogStartLine("新增報送檔案日誌成功");
                        }
                        else
                        {
                            string errmsg = String.Format("新增報送日誌資料失敗，{0}", result.Message);
                            jobLog.AppendLine(errmsg);
                            fileLog.AppendLogStartLine(errmsg);
                            return (doResult = new Result(false, errmsg, null, null));
                        }
                    }
                    #endregion

                    #region FTP 上傳
                    {
                        string errmsg = null;
                        Byte[] buffer = Encoding.GetEncoding("big5").GetBytes(fileContent.ToString());
                        using (MemoryStream ms = new MemoryStream(buffer))
                        {
                            FTPHelper ftpHelper = new FTPHelper();
                            errmsg = ftpHelper.UploadFile(ftpConfig, ms, renderFileName);
                        }
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            fileLog.AppendLogFormatStartLine("上傳 {0} 檔案成功", renderFileName);
                        }
                        else
                        {
                            errmsg = String.Format("上傳 {0} 檔案失敗。{1}", renderFileName, errmsg);
                            jobLog.AppendLine(errmsg);
                            fileLog.AppendLogStartLine(errmsg);
                            return (doResult = new Result(false, errmsg, null, null));
                        }
                    }
                    #endregion

                    #region 更新報送完成處理結果
                    {
                        using (EntityFactory factory = new EntityFactory())
                        {
                            Result result = null;

                            #region 更新報送資料報送完成結果
                            {
                                string sql = @"
UPDATE KP3
   SET Status = @EXPORT_OK_STATUS
     , Feedback_Result = ''
 WHERE Status = @EXPORTING_STATUS
   AND Render_SN = @RENDER_SN
   AND Feedback_Result = @STAMP
".Trim();
                                KeyValue[] parameters = new KeyValue[4]
                                {
                                    new KeyValue("@EXPORT_OK_STATUS", KP3StatusCodeTexts.STATUS20_CODE),
                                    new KeyValue("@EXPORTING_STATUS", KP3StatusCodeTexts.STATUS21_CODE),
                                    new KeyValue("@RENDER_SN", renderSN),
                                    new KeyValue("@STAMP", Environment.MachineName)
                                };
                                int count = 0;
                                result = factory.ExecuteNonQuery(sql, parameters, out count);
                                if (result.IsSuccess)
                                {
                                    fileLog.AppendLogFormatStartLine("更新報送資料報送完成結果成功。共 {0} 筆", count);
                                }
                                else
                                {
                                    string errmsg = String.Format("更新報送資料報送完成結果失敗，{0}", result.Message);
                                    jobLog.AppendLine(errmsg);
                                    fileLog.AppendLogStartLine(errmsg);
                                    return (doResult = new Result(false, errmsg, null, null));
                                }
                            }
                            #endregion

                            #region 更新報送日誌報送完成結果
                            {
                                string sql = @"
UPDATE KP3_Render_Log
   SET Upload_Status = '2'
     , Upload_Result = @UPLOAD_RESULT
     , Feedback_Status = '0' 
     , Status = @UPLOAD_OK_STATUS
 WHERE SN = @RENDER_SN
   AND Upload_Result = @STAMP
".Trim();
                                KeyValue[] parameters = new KeyValue[4]
                                {
                                    new KeyValue("@UPLOAD_RESULT", String.Format("報送完成。共 {0} 筆資料。", kp3Datas.Length)),
                                    new KeyValue("@UPLOAD_OK_STATUS", KP3RenderStatusCodeTexts.STATUS30_CODE),
                                    new KeyValue("@RENDER_SN", renderSN),
                                    new KeyValue("@STAMP", Environment.MachineName)
                                };
                                int count = 0;
                                result = factory.ExecuteNonQuery(sql, parameters, out count);
                                if (result.IsSuccess)
                                {
                                    fileLog.AppendLogFormatLine("更新報送日誌報送完成結果成功（SN = {0}）", renderSN);
                                }
                                else
                                {
                                    string errmsg = String.Format("更新報送日誌報送完成結果失敗，{0}", result.Message);
                                    jobLog.AppendLine(errmsg);
                                    fileLog.AppendLogStartLine(errmsg);
                                    return (doResult = new Result(false, errmsg, null, null));
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 發送 OP 通知信
                    {
                        if (!this.SendOPMail(kp3Config.GetManagers(), renderFileName, kp3Datas.Length, fileLog))
                        {
                            jobLog.AppendLine("發送開立 OP 單通知信失敗");
                        }
                    }
                    #endregion

                    return (doResult = new Result(true, "報送作業執行完成", null, null));
                }
                catch (Exception ex)
                {
                    string errmsg = String.Format("執行報送作業發生例外，{0}", ex.Message);
                    jobLog.AppendLine(errmsg);
                    fileLog
                        .AppendLogLine(errmsg)
                        .AppendLogFormatLine(@"例外訊息：
{0}", ex);
                    if (ex.InnerException != null)
                    {
                        fileLog.AppendLogFormatLine(@"Inner 例外訊息：
{0}", ex.InnerException);
                    }
                    return (doResult = new Result(false, errmsg, null, null));
                }
                finally
                {
                    if (doResult != null && !doResult.IsSuccess)
                    {
                        #region 還原資料
                        if (rollbackFlag >= 1)
                        {
                            try
                            {
                                using (EntityFactory factory = new EntityFactory())
                                {
                                    #region 還原戳記的報送資料
                                    {
                                        string sql = @"
UPDATE KP3
   SET Status = @WAIT_STATUS
     , Render_SN = ''
     , Feedback_Result = ''
 WHERE Status = @EXPORTING_STATUS
   AND Render_SN = @RENDER_SN
   AND  Feedback_Result = @STAMP
".Trim();
                                        KeyValue[] parameters = new KeyValue[4]
                                        {
                                            new KeyValue("@WAIT_STATUS", KP3StatusCodeTexts.STATUS10_CODE),
                                            new KeyValue("@EXPORTING_STATUS", KP3StatusCodeTexts.STATUS21_CODE),
                                            new KeyValue("@RENDER_SN", renderSN),
                                            new KeyValue("@STAMP", Environment.MachineName)
                                        };
                                        int count = 0;
                                        Result result = factory.ExecuteNonQuery(sql, parameters, out count);
                                        if (result.IsSuccess)
                                        {
                                            fileLog.AppendLogFormatLine("還原戳記的報送資料成功（SN = {0}）", renderSN);
                                        }
                                        else
                                        {
                                            string errmsg2 = String.Format("還原戳記的報送資料失敗，{0}", result.Message);
                                            jobLog.AppendLine(errmsg2);
                                            fileLog.AppendLogStartLine(errmsg2);
                                        }
                                    }
                                    #endregion

                                    #region 更新報送日誌報送失敗結果
                                    if (rollbackFlag >= 2)
                                    {
                                        string sql = @"
UPDATE KP3_Render_Log
   SET Upload_Status = '3'
     , Upload_Result = @UPLOAD_RESULT
     , Status = @UPLOAD_FAIL_STATUS
 WHERE SN = @RENDER_SN
   AND Upload_Result = @STAMP
".Trim();
                                        KeyValue[] parameters = new KeyValue[4]
                                        {
                                            new KeyValue("@UPLOAD_RESULT", (doResult.Message.Length > 100 ? doResult.Message.Substring(0, 100) : doResult.Message)),
                                            new KeyValue("@UPLOAD_FAIL_STATUS", KP3RenderStatusCodeTexts.STATUS32_CODE),
                                            new KeyValue("@RENDER_SN", renderSN),
                                            new KeyValue("@STAMP", Environment.MachineName)
                                        };
                                        int count = 0;
                                        Result result = factory.ExecuteNonQuery(sql, parameters, out count);
                                        if (result.IsSuccess)
                                        {
                                            fileLog.AppendLogFormatLine("更新報送日誌資料成功（SN = {0}）", renderSN);
                                        }
                                        else
                                        {
                                            string errmsg2 = String.Format("更新報送日誌資料失敗，{0}", result.Message);
                                            jobLog.AppendLine(errmsg2);
                                            fileLog.AppendLogStartLine(errmsg2);
                                        }
                                    }
                                    #endregion
                                }
                            }
                            catch (Exception ex2)
                            {
                                string errmsg2 = String.Format("還原報送失敗相關資料失敗。{0}", ex2.Message);
                                jobLog.AppendLine(errmsg2);

                                fileLog
                                    .AppendLogLine(errmsg2)
                                    .AppendLogFormatLine(@"例外訊息：
{0}", ex2);
                                if (ex2.InnerException != null)
                                {
                                    fileLog.AppendLogFormatLine(@"Inner 例外訊息：
{0}", ex2.InnerException);
                                }
                            }
                        }
                        #endregion

                        #region 發送失敗通知信
                        {
                            this.SendFailMail(kp3Config.GetManagers(), "報送作業處理", doResult.Message, fileLog);
                        }
                        #endregion
                    }
                }
            }
            #endregion

            #region 通知信相關 Method
            /// <summary>
            /// 發送 聯徵KP3報送開立 OP 單通知信
            /// </summary>
            /// <param name="managers"></param>
            /// <param name="fileName"></param>
            /// <param name="dataCount"></param>
            /// <param name="fileLog"></param>
            /// <returns></returns>
            public bool SendOPMail(ICollection<string> managers, string fileName, int dataCount, FileLoger fileLog)
            {
                if (managers == null || managers.Count == 0)
                {
                    if (fileLog != null)
                    {
                        fileLog.AppendLogStartLine("未設定管理人清單，無法發送 聯徵KP3報送開立 OP 單通知信");
                    }
                    return false;
                }

                bool isFail = false;
                try
                {
                    string subject = String.Format("{0:yyyy/MM/dd} 聯徵KP3報送開立 OP 單通知信", DateTime.Today);
                    string content = String.Format(@"
聯徵KP3報送開立 OP 單通知<br/>
檔名：{0}<br/>
筆數：{1}
"
                        , fileName
                        , dataCount).Trim();

                    BSNSHelper helper = new BSNSHelper();
                    foreach (string manager in managers)
                    {
                        string address = String.Concat(manager, "@landbank.com.tw");
                        string errmsg = helper.SendMail(subject, address, "", content, null);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            if (fileLog != null)
                            {
                                fileLog.AppendLogFormatStartLine("發送 {0} 給 {1} 成功。", subject, address);
                            }
                        }
                        else
                        {
                            isFail = true;
                            if (fileLog != null)
                            {
                                fileLog.AppendLogFormatStartLine("發送 {0} 給 {1} 失敗。{2}", subject, address, errmsg);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFail = true;
                    if (fileLog != null)
                    {
                        fileLog.AppendLogFormatStartLine("發送 聯徵KP3報送開立 OP 單通知信 發生例外。{0}", ex.Message);
                    }
                }
                return !isFail;
            }

            /// <summary>
            /// 發送失敗通知信
            /// </summary>
            /// <param name="managers"></param>
            /// <param name="title"></param>
            /// <param name="msg"></param>
            /// <param name="fileLog"></param>
            public void SendFailMail(ICollection<string> managers, string title, string msg, FileLoger fileLog)
            {
                if (managers == null || managers.Count == 0)
                {
                    if (fileLog != null)
                    {
                        fileLog.AppendLogFormatStartLine("未設定管理人清單，無法發送 {0} 失敗通知信", title);
                    }
                    return;
                }

                try
                {
                    string subject = String.Format("{0:yyyy/MM/dd} {1} 失敗通知信", DateTime.Today, title);
                    string content = String.Format(@"
{0} 失敗通知<br/>
{1}
"
                        , title
                        , msg.Replace("\r\n", "<br/>\r\n")).Trim();

                    BSNSHelper helper = new BSNSHelper();
                    foreach (string manager in managers)
                    {
                        string address = String.Concat(manager, "@landbank.com.tw");
                        string errmsg = helper.SendMail(subject, address, "", content, null);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            if (fileLog != null)
                            {
                                fileLog.AppendLogFormatStartLine("發送 {0} 給 {1} 成功。", subject, address);
                            }
                        }
                        else if (fileLog != null)
                        {
                            fileLog.AppendLogFormatStartLine("發送 {0} 給 {1} 失敗。{2}", subject, address, errmsg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (fileLog != null)
                    {
                        fileLog.AppendLogFormatStartLine("發送 {0} 失敗通知信發生例外。{1}", title, ex.Message);
                    }
                }
            }

            /// <summary>
            /// 發送 回饋處理結果 通知信
            /// </summary>
            /// <param name="managers"></param>
            /// <param name="kp3Result"></param>
            /// <param name="fileLog"></param>
            /// <returns></returns>
            public bool SendFeedbackMail(ICollection<string> managers, KP3Entity kp3Result, FileLoger fileLog)
            {
                if (managers == null || managers.Count == 0 || String.IsNullOrEmpty(kp3Result.CreateUser))
                {
                    if (fileLog != null)
                    {
                        fileLog.AppendLogStartLine("未設定管理人清單或缺資料建立者，無法發送 回饋處理結果 通知信");
                    }
                    return false;
                }

                bool isFail = false;
                try
                {
                    string subject = String.Format("{0:yyyy/MM/dd} 特約機構代號 {1} 回饋處理結果通知信", DateTime.Today, kp3Result.Item07);
                    string content = String.Format(@"
特約機構代號 {0} 回饋處理結果通知<br/>
更新日期：{1}
報送結果：{2}
回饋訊息：{3}
"
                        , kp3Result.Item07
                        , kp3Result.Item40
                        , kp3Result.FeedbackStatus == "Y" ? "成功" : "失敗"
                        , kp3Result.FeedbackResult.Replace("\r\n", "<br/>\r\n")).Trim();

                    BSNSHelper helper = new BSNSHelper();

                    #region 不管成功失敗都要寄給資料建立者
                    {
                        string address = String.Concat(kp3Result.CreateUser, "@landbank.com.tw");
                        string errmsg = helper.SendMail(subject, address, "", content, null);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            if (fileLog != null)
                            {
                                fileLog.AppendLogFormatStartLine("發送 {0} 給 {1} 成功。", subject, address);
                            }
                        }
                        else
                        {
                            isFail = true;
                            if (fileLog != null)
                            {
                                fileLog.AppendLogFormatStartLine("發送 {0} 給 {1} 失敗。{2}", subject, address, errmsg);
                            }

                            #region 通知管理人轉通知 (先不要)
                            //                            {
                            //                                string subject2 = String.Concat(subject, " （請轉告資料建立者）");
                            //                                string content2 = String.Format(@"
                            //發送回饋處理結果給 {0} 失敗。請轉告資料建立者<br/>
                            //原通知信內容：<br/>
                            //{1}
                            //", address, content);
                            //                                foreach (string bankUser in managers)
                            //                                {
                            //                                    string address2 = String.Concat(bankUser, "@landbank.com.tw");
                            //                                    string errmsg2 = helper.SendMail(subject2, address2, "", content2, null);
                            //                                    if (!String.IsNullOrEmpty(errmsg2) && fileLog != null)
                            //                                    {
                            //                                        fileLog.AppendLogFormatStartLine("發送 回饋處理結果 轉告 通知信給 {0} 失敗。{1}", address2, errmsg2);
                            //                                    }
                            //                                }
                            //                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 土銀要求不管成功失敗都要寄給管理者
                    foreach (string manager in managers)
                    {
                        string address = String.Concat(manager, "@landbank.com.tw");
                        string errmsg = helper.SendMail(subject, address, "", content, null);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            if (fileLog != null)
                            {
                                fileLog.AppendLogFormatStartLine("發送 {0} 給 {1} 成功。", subject, address);
                            }
                        }
                        else
                        {
                            isFail = true;
                            if (fileLog != null)
                            {
                                fileLog.AppendLogFormatStartLine("發送 {0} 給 {1} 失敗。{2}", subject, address, errmsg);
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    isFail = true;
                    if (fileLog != null)
                    {
                        fileLog.AppendLogFormatStartLine("發送 特約機構代號 {0} 回饋處理結果通知信 發生例外。{1}", kp3Result.Item07, ex.Message);
                    }
                }
                return !isFail;
            }
            #endregion
        }
        #endregion
    }
}
