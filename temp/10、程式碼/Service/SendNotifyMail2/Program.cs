using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
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

namespace SendNotifyMail2
{
    /// <summary>
    /// 新服務執行結果通知信
    /// </summary>
    class Program
    {
        #region FileLog 相關
        /// <summary>
        /// 檔案日誌處理類別
        /// </summary>
        private sealed class FileLoger
        {
            #region Static Readonly
            /// <summary>
            /// 日誌根路徑
            /// </summary>
            private static readonly string LOG_ROOT_PATH = @"D:\AP\LOG\";
            #endregion

            #region Member
            /// <summary>
            /// 日誌完整檔名
            /// </summary>
            private string _LogFileName = null;

            /// <summary>
            /// 暫存大小
            /// </summary>
            private Int32 _BufferSize = 1024;

            /// <summary>
            /// 暫存日誌
            /// </summary>
            private StringBuilder _Buffer = null;
            #endregion

            #region Property
            /// <summary>
            /// 應用程式名稱
            /// </summary>
            public string AppName
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌路徑
            /// </summary>
            public string LogPath
            {
                get;
                private set;
            }

            /// <summary>
            /// 是否開啟除錯模式
            /// </summary>
            public bool IsDebug
            {
                get;
                private set;
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 檔案日誌處理類別 物件
            /// </summary>
            /// <param name="appName"></param>
            public FileLoger(string appName)
            {
                #region AppName
                this.AppName = String.IsNullOrWhiteSpace(appName) ? String.Empty : appName.Trim();
                #endregion

                #region IsDebug
                string logMode = ConfigurationManager.AppSettings.Get("LOG_MDOE");
                if (String.IsNullOrWhiteSpace(logMode))
                {
                    this.IsDebug = false;
                }
                else
                {
                    this.IsDebug = "DEBUG".Equals(logMode.Trim(), StringComparison.CurrentCultureIgnoreCase);
                }
                #endregion

                #region LogPath & Initial
                string logSubPath = ConfigurationManager.AppSettings.Get("LOG_SUB_PATH");
                this.Initial(LOG_ROOT_PATH, logSubPath);
                #endregion
            }

            public FileLoger(string appName, bool isDebug, string logPath)
            {
                #region AppName
                this.AppName = String.IsNullOrWhiteSpace(appName) ? String.Empty : appName.Trim();
                #endregion

                #region IsDebug
                this.IsDebug = isDebug;
                #endregion

                #region LogPath & Initial
                this.Initial(logPath);
                #endregion
            }
            #endregion

            #region Private Method
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="rootPath"></param>
            /// <param name="subPath"></param>
            private void Initial(string rootPath, string subPath = null)
            {
                try
                {
                    #region 日誌路徑
                    DirectoryInfo dInfo = null;
                    if (String.IsNullOrWhiteSpace(subPath))
                    {
                        dInfo = new DirectoryInfo(rootPath);
                    }
                    else
                    {
                        dInfo = new DirectoryInfo(Path.Combine(rootPath, subPath));
                        if (!dInfo.FullName.StartsWith(rootPath))
                        {
                            Console.WriteLine("檔案日誌 子目錄 參數不合法");
                            return;
                        }
                    }

                    if (!dInfo.Exists)
                    {
                        dInfo.Create();
                    }
                    this.LogPath = dInfo.FullName;
                    #endregion

                    #region 日誌完整檔名
                    if (String.IsNullOrEmpty(this.AppName))
                    {
                        _LogFileName = Path.Combine(this.LogPath, String.Format("{0:yyyyMMdd}.log", DateTime.Today));
                    }
                    else
                    {
                        _LogFileName = Path.Combine(this.LogPath, String.Format("{0}_{1:yyyyMMdd}.log", this.AppName, DateTime.Today));
                    }
                    #endregion

                    _Buffer = new StringBuilder(_BufferSize * 2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("檔案日誌初始化發生例外。{0}", ex.Message));
                }
            }
            #endregion

            #region Public Method
            /// <summary>
            /// 將暫存內容寫入檔案日誌，並清空暫存
            /// </summary>
            /// <returns></returns>
            public FileLoger Flush()
            {
                if (_Buffer != null && _Buffer.Length > 0)
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(_LogFileName))
                        {
                            using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
                            {
                                sw.Write(_Buffer.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(String.Format("Flush 檔案日誌發生例外。{0}", ex.Message));
                    }
                    finally
                    {
                        _Buffer.Clear();
                    }
                }
                return this;
            }

            /// <summary>
            /// 寫入指定日誌訊息
            /// </summary>
            /// <param name="msg">指定日誌訊息</param>
            /// <param name="newline">指定寫入後是否換行，預設 true</param>
            /// <returns></returns>
            public FileLoger WriteLog(string msg, bool newline = true)
            {
                if (!String.IsNullOrEmpty(_LogFileName))
                {
                    if (newline)
                    {
                        _Buffer.AppendLine(msg);
                    }
                    else
                    {
                        _Buffer.Append(msg);
                    }
                    if (_Buffer.Length > _BufferSize)
                    {
                        this.Flush();
                    }
                }
                return this;
            }

            /// <summary>
            /// 寫入指定格式與參數陣列的格式化日誌訊息
            /// </summary>
            /// <param name="newline">指定寫入後是否換行</param>
            /// <param name="format">指定格式</param>
            /// <param name="args">指定參數陣列</param>
            /// <returns></returns>
            public FileLoger WriteLog(bool newline, string format, params object[] args)
            {
                if (!String.IsNullOrEmpty(_LogFileName) 
                    && !String.IsNullOrEmpty(format) && args != null && args.Length > 0)
                {
                    try
                    {
                        _Buffer.AppendFormat(format, args);
                        if (newline)
                        {
                            _Buffer.AppendLine();
                        }
                        if (_Buffer.Length > _BufferSize)
                        {
                            this.Flush();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                return this;
            }

            /// <summary>
            /// 寫入指定除錯日誌訊息
            /// </summary>
            /// <param name="msg">指定除錯日誌訊息</param>
            /// <param name="newline">指定寫入後是否換行，預設 true</param>
            /// <returns></returns>
            public FileLoger WriteDebugLog(string msg, bool newline = true)
            {
                if (this.IsDebug && !String.IsNullOrEmpty(_LogFileName))
                {
                    if (newline)
                    {
                        _Buffer.Append("[DEBUG] ").AppendLine(msg);
                    }
                    else
                    {
                        _Buffer.Append("[DEBUG] ").Append(msg);
                    }
                    if (_Buffer.Length > _BufferSize)
                    {
                        this.Flush();
                    }
                }
                return this;
            }

            /// <summary>
            /// 寫入指定格式與參數陣列的格式化除錯日誌訊息
            /// </summary>
            /// <param name="newline">指定寫入後是否換行</param>
            /// <param name="format">指定格式</param>
            /// <param name="args">指定參數陣列</param>
            /// <returns></returns>
            public FileLoger WriteDebugLog(bool newline, string format, params object[] args)
            {
                if (!String.IsNullOrEmpty(_LogFileName)
                    && !String.IsNullOrEmpty(format) && args != null && args.Length > 0)
                {
                    try
                    {
                        _Buffer.Append("[DEBUG] ").AppendFormat(format, args);
                        if (newline)
                        {
                            _Buffer.AppendLine();
                        }
                        if (_Buffer.Length > _BufferSize)
                        {
                            this.Flush();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                return this;
            }
            #endregion
        }
        #endregion

        #region 寄信相關
        /// <summary>
        /// 取得收信人參數指定的 MailAddress 集合
        /// </summary>
        /// <param name="arg">收信人參數</param>
        /// <param name="ccMails">成功則傳回 MailAddress 集合，否則傳回 null</param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回 null</returns>
        private static string GetCCMails(string arg, out List<MailAddress> ccMails)
        {
            ccMails = null;
            if (String.IsNullOrWhiteSpace(arg))
            {
                return "未指定 EMAILS 命令參數的設定值";
            }

            string[] values = arg.Trim().Replace("[", "<").Replace("]", ">").Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (values == null || values.Length == 0)
            {
                return "不正確的 EMAILS 命令參數";
            }

            string address = null;
            try
            {
                ccMails = new List<MailAddress>(values.Length);
                foreach (string value in values)
                {
                    address = value.Trim();
                    MailAddress ccMail = new MailAddress(address);
                    ccMails.Add(ccMail);
                }
            }
            catch (Exception)
            {
                return String.Format("{0} 不是合法的 Email", address);
            }
            return null;
        }

        /// <summary>
        /// 發送通知信
        /// </summary>
        /// <param name="ccMails"></param>
        /// <param name="startTime"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        private static string SendNotifyMail(List<MailAddress> ccMails, DateTime qTime, DataSet ds)
        {
            if (ccMails == null || ccMails.Count == 0)
            {
                return "未設定任何收信人 Email";
            }

            StringBuilder errmsg = new StringBuilder();
            try
            {
                #region 標題
                string subject = String.Format("{0:yyyy/MM/dd HH:mm} 學雜費『新服務執行結果』通知信", qTime);
                #endregion

                #region 內容
                string content = null;
                if (ds == null || ds.Tables == null || ds.Tables.Count == 0)
                {
                    content = "查無 新服務執行結果』資料<br/>\r\n";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    DataTableCollection dts = ds.Tables;
                    foreach (DataTable dt in dts)
                    {
                        string code = dt.TableName;
                        string name = JobCubeTypeCodeTexts.GetText(code);
                        if (dt.Rows.Count == 0)
                        {
                            sb.AppendFormat("查無 {0:yyyy/MM/dd HH:mm}  ({1})『{2}』最新三筆處理結果", qTime, code, name).AppendLine("<br/>");
                        }
                        else
                        {
                            sb.AppendFormat("查詢 {0:yyyy/MM/dd HH:mm}  ({1})『{2}』最新三筆處理結果", qTime, code, name).AppendLine("<br/>");
                            foreach (DataRow row in dt.Rows)
                            {
                                string resultId = row.IsNull("JRESULTID") ? null : row["JRESULTID"].ToString();
                                string cDate = row.IsNull("C_Date") ? null : Convert.ToDateTime(row["C_Date"]).ToString("HH:mm:ss");
                                string memo = row.IsNull("Memo") ? null : row["Memo"].ToString();
                                switch (resultId)
                                {
                                    case JobCubeResultCodeTexts.WAIT:
                                    case JobCubeResultCodeTexts.PROCESS:
                                        sb.AppendFormat("[{0:HH:mm:ss}] 執行結果：{1}", cDate, JobCubeResultCodeTexts.GetText(resultId)).AppendLine("<br/>");
                                        break;
                                    case JobCubeResultCodeTexts.SUCCESS:
                                    case JobCubeResultCodeTexts.FAILURE:
                                    case JobCubeResultCodeTexts.BREAK:
                                        sb.AppendFormat("[{0:HH:mm:ss}] 執行結果：{1}，結果說明：{2}", cDate, JobCubeResultCodeTexts.GetText(resultId),memo).AppendLine("<br/>");
                                        break;
                                    default:
                                        sb.AppendFormat("[{0:HH:mm:ss}] 結果代碼：{1}，結果說明：{2}", cDate, resultId, memo).AppendLine("<br/>");
                                        break;
                                }
                            }
                            sb.AppendLine("<br/>");
                        }
                    }
                    content = sb.ToString();
                }
                #endregion

                BSNSHelper helper = new BSNSHelper();
                foreach (MailAddress ccMail in ccMails)
                {
                    string displayName = String.IsNullOrEmpty(ccMail.DisplayName) ? ccMail.User : ccMail.DisplayName;
                    string errmsg2 = helper.SendMail(subject, ccMail.Address, displayName, content, null);
                    if (!String.IsNullOrEmpty(errmsg2))
                    {
                        errmsg.Append(errmsg2).Append(";");
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg.AppendLine(ex.Message);
            }
            return errmsg.ToString();
        }
        #endregion


        /// <summary>
        /// 參數：EMAILS=收信人信箱清單，逗號或分號隔開
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            #region 取得應用程式 Guid 與名稱
            string appGuid = null, appName = null;
            {
                //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
                appName = Path.GetFileNameWithoutExtension(myAssembly.Location);
            }
            #endregion

            FileLoger fileLog = null;
            StringBuilder jobLog = null;

            JobcubeEntity myJob = null;
            JobCubeCheckMode myJobCheckMode = JobCubeCheckMode.ByTime;         //本程式的佇列檢查模式列舉
            string myJobTypeId = Entities.JobCubeTypeCodeTexts.SNM2;           //本程式的作業類別代碼
            string myJobTypeName = JobCubeTypeCodeTexts.GetText(myJobTypeId);  //本程式的作業類別名稱

            int exitCode = 0;
            string exitMsg = null;
            DateTime startTime = DateTime.Now;

            try
            {
                fileLog = new FileLoger(appName);
                jobLog = new StringBuilder();

                #region 紀錄檔案日誌（程式開始）
                fileLog
                    .WriteLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] {1} ({2}, {3}) 開始", DateTime.Now, appName, myJobTypeId, myJobTypeName)
                    .WriteLog(true, "  命令參數：{0}", (args == null || args.Length == 0) ? null : String.Join(" ", args))
                    .Flush();
                #endregion

                #region 處理參數
                List<MailAddress> ccMails = null;
                if (exitCode == 0)
                {
                    string errmsg = null;

                    #region 拆解參數
                    if (args != null && args.Length > 0)
                    {
                        foreach (string arg in args)
                        {
                            string[] kvs = arg.Split('=');
                            if (kvs.Length == 2)
                            {
                                string key = kvs[0].Trim().ToUpper();
                                string value = kvs[1].Trim();
                                switch (key)
                                {
                                    case "EMAILS":
                                        #region 收信人信箱清單，逗號或分號隔開
                                        {
                                            errmsg = GetCCMails(value, out ccMails);
                                        }
                                        #endregion
                                        break;

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

                    #region 檢查必要參數
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        if (ccMails == null || ccMails.Count == 0)
                        {
                            errmsg = "缺少或不正確的 EMAILS 命令參數";
                        }
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -1;
                        exitMsg = String.Format("參數錯誤，錯誤訊息：{0}", errmsg);
                        jobLog.AppendLine(exitMsg);

                        #region 紀錄檔案日誌（參數錯誤）
                        fileLog.WriteLog(exitMsg);
                        if (args == null || args.Length == 0)
                        {
                            fileLog.WriteLog("參數語法：[IDS=查詢的作業類別代碼清單，以逗號區隔] [EMAILS=收信人信箱清單，逗號或分號隔開]");
                        }
                        fileLog.Flush();
                        #endregion
                    }
                }
                #endregion

                #region 新增處理中的 Job
                if (exitCode == 0)
                {
                    myJob = new JobcubeEntity();
                    myJob.Jtypeid = myJobTypeId;
                    myJob.Jparam = String.Join(" ", args);

                    using(JobCubeHelper jobHelper = new JobCubeHelper())
                    {
                        Result result = jobHelper.InsertProcessJob(ref myJob, myJobCheckMode);
                        fileLog.WriteDebugLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] 新增處理中的 Job ({1}, {2})", DateTime.Now, myJob.Jtypeid, myJob.Jparam);
                        if (result.IsSuccess)
                        {
                            #region 紀錄除錯檔案日誌（新增處理中的 Job）
                            fileLog
                                .WriteDebugLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] 新增處理中的 Job 成功({1}, {2})", DateTime.Now, myJob.Jtypeid, myJob.Jparam)
                                .WriteDebugLog(true, "  JobNo = {0}, JobStamp = {1}", myJob.Jno, myJob.Memo)
                                .Flush();
                            #endregion
                        }
                        else
                        {
                            exitCode = -2;
                            exitMsg = String.Format("新增處理中的 Job 失敗。{0}", result.Message);
                            jobLog.AppendLine(exitMsg);

                            #region 紀錄檔案日誌（新增處理中的 Job）
                            fileLog
                                .WriteLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] 新增處理中的 Job 失敗 ({1}, {2})", DateTime.Now, myJob.Jtypeid, myJob.Jparam)
                                .WriteLog(result.Message)
                                .Flush();
                            #endregion
                        }
                    }
                }
                #endregion

                #region 處理資料
                if (exitCode == 0)
                {
                    #region 取資料
                    DataSet ds = new DataSet();
                    DateTime minTime = startTime.Date;  //當天
                    DateTime maxTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, 0);  //當下

                    using (EntityFactory factory = new EntityFactory())
                    {
                        #region 取 D00I70資料下載 最新 3 筆作業結果
                        if (exitCode == 0)
                        {
                            string sql = @"
SELECT TOP 3 JTYPEID, JSTD, JETD, C_Date, JRESULTID, Memo
  FROM JobCube
 WHERE JTYPEID IN ('D70', 'D70B', 'D70C')
   AND @MIN_TIME <= C_Date AND C_Date < @MAX_TIME
 ORDER BY C_Date DESC, JSTD DESC".Trim();
                            KeyValue[] parameters = new KeyValue[] { new KeyValue("@MIN_TIME", minTime), new KeyValue("@MAX_TIME", maxTime) };
                            DataTable dt = null;
                            Result result = factory.GetDataTable(sql, parameters, 0, 3, out dt);
                            if (result.IsSuccess)
                            {
                                dt.TableName = "D70";
                                ds.Tables.Add(dt);
                            }
                            else
                            {
                                exitCode = -3;
                                exitMsg = String.Format("讀取『{0}』最新三筆作業結果失敗，錯誤訊息：{1}", JobCubeTypeCodeTexts.GetText("D70"), result.Message);
                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();

                                #region 紀錄檔案日誌（讀取D70作業結果失敗）
                                fileLog
                                    .WriteLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] 讀取『{1}』最新三筆作業結果失敗。", DateTime.Now, JobCubeTypeCodeTexts.GetText("D70"))
                                    .WriteLog(result.Message)
                                    .Flush();
                                #endregion
                            }
                        }
                        #endregion

                        #region 取 銷帳處理 最新 3 筆作業結果
                        if (exitCode == 0)
                        {
                            string sql = @"
SELECT TOP 3 JTYPEID, JSTD, JETD, C_Date, JRESULTID, Memo
  FROM JobCube
 WHERE JTYPEID = 'SCD'
   AND @MIN_TIME <= C_Date AND C_Date < @MAX_TIME
 ORDER BY C_Date DESC, JSTD DESC".Trim();
                            KeyValue[] parameters = new KeyValue[] { new KeyValue("@MIN_TIME", minTime), new KeyValue("@MAX_TIME", maxTime) };
                            DataTable dt = null;
                            Result result = factory.GetDataTable(sql, parameters, 0, 3, out dt);
                            if (result.IsSuccess)
                            {
                                dt.TableName = "SCD";
                                ds.Tables.Add(dt);
                            }
                            else
                            {
                                exitCode = -3;
                                exitMsg = String.Format("讀取『{0}』最新三筆作業結果失敗，錯誤訊息：{1}", JobCubeTypeCodeTexts.GetText("SCD"), result.Message);
                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();

                                #region 紀錄檔案日誌（讀取SCD作業結果失敗）
                                fileLog
                                    .WriteLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] 讀取『{1}』最新三筆作業結果失敗。", DateTime.Now, JobCubeTypeCodeTexts.GetText("SCD"))
                                    .WriteLog(result.Message)
                                    .Flush();
                                #endregion
                            }
                        }
                        #endregion

                        #region 取 學校銷帳檔3合1 最新 3 筆作業結果
                        if (exitCode == 0)
                        {
                            string sql = @"
SELECT TOP 3 JTYPEID, JSTD, JETD, C_Date, JRESULTID, Memo
  FROM JobCube
 WHERE JTYPEID = 'SC31'
   AND @MIN_TIME <= C_Date AND C_Date < @MAX_TIME
 ORDER BY C_Date DESC, JSTD DESC".Trim();
                            KeyValue[] parameters = new KeyValue[] { new KeyValue("@MIN_TIME", minTime), new KeyValue("@MAX_TIME", maxTime) };
                            DataTable dt = null;
                            Result result = factory.GetDataTable(sql, parameters, 0, 3, out dt);
                            if (result.IsSuccess)
                            {
                                dt.TableName = "SC31";
                                ds.Tables.Add(dt);
                            }
                            else
                            {
                                exitCode = -3;
                                exitMsg = String.Format("讀取『{0}』最新三筆作業結果失敗，錯誤訊息：{1}", JobCubeTypeCodeTexts.GetText("SC31"), result.Message);
                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();

                                #region 紀錄檔案日誌（讀取SC31作業結果失敗）
                                fileLog
                                    .WriteLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] 讀取『{1}』最新三筆作業結果失敗。", DateTime.Now, JobCubeTypeCodeTexts.GetText("SC31"))
                                    .WriteLog(result.Message)
                                    .Flush();
                                #endregion
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region 發送通知信
                    if (exitCode == 0)
                    {
                        string errmsg = SendNotifyMail(ccMails, maxTime, ds);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            exitMsg = "發送通知信成功";
                        }
                        else
                        {
                            exitCode = -4;
                            exitMsg = String.Format("發送通知信失敗，錯誤訊息：{0}", errmsg);
                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();

                            #region 紀錄檔案日誌（發送通知信失敗）
                            fileLog
                                .WriteLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] 發送通知信失敗。", DateTime.Now)
                                .WriteLog(errmsg)
                                .Flush();
                            #endregion
                        }
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;
                exitMsg = String.Format("作業處理發生例外。{0}", ex.Message);
                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外。{2}", DateTime.Now, myJobTypeName, ex.Message).AppendLine();

                #region 紀錄檔案日誌（作業處理發生例外）
                fileLog
                    .WriteLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外。{2}", DateTime.Now, myJobTypeName, ex.Message)
                    .WriteLog(ex.ToString());
                    if (ex.InnerException != null)
                    {
                        fileLog
                            .WriteLog("Inner Exception：")
                            .WriteLog(ex.InnerException.ToString());
                    }
                    fileLog.Flush();
                #endregion
            }
            finally
            {
                #region 更新 Job 為已完成
                if (myJob != null && myJob.Jno > 0)
                {
                    string jobResultId = (exitCode == 0 ? JobCubeResultCodeTexts.SUCCESS : JobCubeResultCodeTexts.FAILURE);
                    using (JobCubeHelper jobHelper = new JobCubeHelper())
                    {
                        Result result = jobHelper.UpdateProcessJobToFinsh(myJob.Jno, myJob.Memo, jobResultId, exitMsg, jobLog.ToString());
                        if (!result.IsSuccess)
                        {
                            #region 紀錄檔案日誌（更新 Job 為已完成）
                            fileLog
                                .WriteLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] 更新 Job 為已完成失敗 ({1}, {2})", DateTime.Now, myJob.Jno, myJob.Memo)
                                .WriteLog(result.Message)
                                .Flush();
                            #endregion
                        }
                    }
                }
                #endregion

                #region 紀錄檔案日誌（程式結束）
                if (fileLog != null)
                {
                    fileLog
                        .WriteLog(true, "[{0:yyyy/MM/dd HH:mm:ss}] {1} ({2}, {3}) 結束", DateTime.Now, appName, myJobTypeId, myJobTypeName)
                        .Flush();
                }
                #endregion
            }

            System.Environment.Exit(exitCode);
        }
    }
}
