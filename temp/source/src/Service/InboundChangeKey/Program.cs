using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

using Entities;

namespace InboundChangeKey
{
    /// <summary>
    /// 財金境外支付換KEY (INBK)
    /// </summary>
    class Program
    {
        #region MyHelper 處理工具類別
        private class MyHelper : IDisposable
        {
            #region Static Readonly Member
            /// <summary>
            /// 財金支付寶系統參數設定資料的 ConfigKey
            /// </summary>
            public static readonly string InbounConfigKey = "Inbound";
            #endregion

            #region Member
            /// <summary>
            /// 儲存 資料存取物件 的變數
            /// </summary>
            private EntityFactory _Factory = null;
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 MyHelper 處理工具類別
            /// </summary>
            public MyHelper()
            {
            }
            #endregion

            #region Destructor
            /// <summary>
            /// 解構 MyHelper 處理工具類別
            /// </summary>
            ~MyHelper()
            {
                this.Dispose(false);
            }
            #endregion

            #region Implement IDisposable
            /// <summary>
            /// Track whether Dispose has been called.
            /// </summary>
            private bool _Disposed = false;

            /// <summary>
            /// 執行與釋放 (Free)、釋放 (Release) 或重設 Unmanaged 資源相關聯之應用程式定義的工作
            /// </summary>
            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// 釋放資源
            /// </summary>
            /// <param name="disposing">指定是否釋放資源。</param>
            private void Dispose(bool disposing)
            {
                if (!_Disposed)
                {
                    if (disposing)
                    {
                        if (_Factory != null)
                        {
                            _Factory.Dispose();
                            _Factory = null;
                        }
                    }
                    _Disposed = true;
                }
            }
            #endregion

            #region DB 相關 Method
            private EntityFactory GetFactory()
            {
                if (_Factory == null)
                {
                    _Factory = new EntityFactory();
                }
                return _Factory;
            }

            /// <summary>
            /// 讀取支付寶相關設定，並找出工作的驗證參數
            /// </summary>
            /// <param name="workKey">傳回工作的驗證參數</param>
            /// <returns>失敗則傳回錯誤訊息，否則傳回 null</returns>
            public string GetInboundConfigWorkKey(out string workKey)
            {
                workKey = null;

                try
                {
                    ConfigEntity config = null;
                    Expression where = new Expression(ConfigEntity.Field.ConfigKey, InbounConfigKey);
                    using (EntityFactory factory = this.GetFactory())
                    {
                        Result result = factory.SelectFirst<ConfigEntity>(where, null, out config);
                        if (!result.IsSuccess)
                        {
                            return result.Message;
                        }
                    }

                    if (config == null)
                    {
                        return "查無資料";
                    }
                    else if (String.IsNullOrWhiteSpace(config.ConfigValue))
                    {
                        return "缺少設定值";
                    }
                    else
                    {
                        #region 解析設定並找出工作的驗證參數
                        string initKey = null;
                        string[] items = config.ConfigValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in items)
                        {
                            int index = item.IndexOf("=");
                            if (index < 0)
                            {
                                continue;
                            }

                            string key = item.Substring(0, index).Trim();
                            string value = item.Substring(index + 1);
                            switch (key.ToUpper())
                            {
                                case "INITKEY":
                                    initKey = value.Trim();
                                    break;
                                case "KEY":
                                    workKey = value.Trim();
                                    break;
                            }
                            if (!String.IsNullOrEmpty(workKey))
                            {
                                break;
                            }
                        }
                        if (String.IsNullOrEmpty(workKey))
                        {
                            workKey = initKey;
                        }
                        #endregion

                        if (String.IsNullOrEmpty(workKey))
                        {
                            return "缺少驗證參數";
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            #endregion

            #region 呼叫 FiscService 相關 Method
            /// <summary>
            /// 取得 FiscService 服務的 Client
            /// </summary>
            /// <param name="webUrl"></param>
            /// <param name="client"></param>
            /// <returns></returns>
            private string GetFiscServiceClient(Uri webUrl, out FiscService.FiscServiceSoapClient client)
            {
                client = null;
                string scheme = webUrl == null ? String.Empty : webUrl.Scheme;
                string endpointConfigurationName = null;
                if (scheme == Uri.UriSchemeHttp)
                {
                    endpointConfigurationName = "FiscServiceSoap_HTTP";
                }
                else if (scheme == Uri.UriSchemeHttps)
                {
                    endpointConfigurationName = "FiscServiceSoap_HTTPS";
                }
                else
                {
                    return "缺少 WEB 端主機參數或通訊協定不正確";
                }

                try
                {
                    string url = (new Uri(webUrl, "FiscService.asmx")).AbsoluteUri;
                    client = new FiscService.FiscServiceSoapClient(endpointConfigurationName);
                    client.Endpoint.Address = new System.ServiceModel.EndpointAddress(url);
                    return null;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            #region 忽略不信任的憑證
            /// <summary>
            /// 處理不信任的憑證 (忽略)
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="certificate"></param>
            /// <param name="chain"></param>
            /// <param name="sslPolicyErrors"></param>
            /// <returns></returns>
            private static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }
            #endregion

            /// <summary>
            /// 呼叫 FiscService 服務的 Inbound 換 KEY 作業
            /// </summary>
            /// <param name="webUrl"></param>
            /// <param name="jobNo"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            public string CallFiscServiceChanegKey(Uri webUrl, int jobNo, string checkCode, out string result)
            {
                result = null;
                string errmsg = null;

                try
                {
                    FiscService.FiscServiceSoapClient client = null;
                    string errmsg2 = this.GetFiscServiceClient(webUrl, out client);
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        #region  指定 SecurityProtocol 為 TLS 1.1 (768) 與 TLS 1.2 (3072)
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate
                        {
                            return true;
                        };
                        #endregion

                        #region 忽略不信任的憑證
                        if (client.Endpoint.Address.Uri.Scheme == Uri.UriSchemeHttps)
                        {
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                        }
                        #endregion

                        result = client.InboundChangeKey(jobNo, checkCode);
                        if (result.StartsWith("FAILURE。", StringComparison.CurrentCultureIgnoreCase))
                        {
                            errmsg = "Inbound 換 KEY 作業失敗";
                        }
                        else if (result.StartsWith("SUCCESS。", StringComparison.CurrentCultureIgnoreCase))
                        {

                        }
                        else
                        {
                            errmsg = "Inbound 換 KEY 作業回傳訊息無法判斷成功或失敗";
                        }
                    }
                    else
                    {
                        errmsg = String.Concat("產生 FiscService 服務物件失敗，", errmsg2);
                    }
                }
                catch (Exception ex)
                {
                    errmsg = String.Concat("呼叫 FiscService 服務發生例外，", ex.Message);
                }
                return errmsg;
            }
            #endregion

            #region WriteLog 相關
            /// <summary>
            /// 寫入日誌檔
            /// </summary>
            /// <param name="logFileName"></param>
            /// <param name="log"></param>
            public void WriteFileLog(string logFileName, StringBuilder log)
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
        #endregion

        /// <summary>
        /// 財金境外支付換KEY，作業類別代碼固定為 INBK，不提供 retry 機制，同一時間 (分鐘) 排程僅允許一個作業
        /// </summary>
        /// <param name="args">
        /// host = 指定 Web 端主機的 Domain 或 IP
        /// ssl = 指定是否使用 SSL (true / false 不分大小寫)
        /// </param>
        static void Main(string[] args)
        {
            #region Initial
            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);

            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = JobCubeTypeCodeTexts.INBK;                   //作業類別代碼
            string jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);   //作業類別名稱

            int exitCode = 0;
            string exitMsg = null;
            #endregion

            DateTime startTime = DateTime.Now;
            string argsLine = (args == null || args.Length == 0) ? String.Empty : String.Join(" ", args);

            MyHelper myHelper = new MyHelper();
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
                Uri webUrl = null;
                if (exitCode == 0)
                {
                    fileLog.AppendFormat("命令參數：{0}", argsLine).AppendLine();
                    string argsMemo = "參數語法：host=指定Web端主機的Domain或IP ssl=指定是否使用SSL(true/false不分大小寫)";

                    string errmsg = null;
                    string argHost = null;
                    string argSsl = null;
                    if (args != null && args.Length > 0)
                    {
                        #region 拆解命令參數
                        foreach (string arg in args)
                        {
                            string[] kvs = arg.Split('=');
                            if (kvs.Length == 2)
                            {
                                string key = kvs[0].Trim().ToLower();
                                string value = kvs[1].Trim();
                                switch (key)
                                {
                                    case "host":
                                        argHost = value;
                                        break;
                                    case "ssl":
                                        argSsl = value;
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
                        #endregion
                    }
                    else
                    {
                        errmsg = "未指定命令參數";
                    }

                    #region 檢查命令參數值
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        if (String.IsNullOrEmpty(argHost))
                        {
                            errmsg = "host 命令參數值必須為 Web 端主機的 Domain 或 IP";
                        }
                        else
                        {
                            try
                            {
                                if (argSsl.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    webUrl = new Uri("https://" + argHost);
                                }
                                else if (argSsl.Equals("false", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    webUrl = new Uri("http://" + argHost);
                                }
                                else
                                {
                                    errmsg = "ssl 命令參數值必須為 true 或 false";
                                }
                            }
                            catch (Exception)
                            {
                                errmsg = "host 命令參數值必須為 Web 端主機的 Domain 或 IP";
                            }
                        }
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -1;
                        exitMsg = errmsg;
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLine(exitMsg);
                        jobLog.AppendLine(argsMemo);
                        fileLog.AppendLine(argsMemo);
                    }
                    else if (webUrl == null)
                    {
                        exitCode = -1;
                        exitMsg = "命令參數錯誤，無法產生 Host 的 Uri 物件";
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLine(exitMsg);
                        jobLog.AppendLine(argsMemo);
                        fileLog.AppendLine(argsMemo);
                    }
                }
                #endregion

                #region 讀取支付寶相關設定，並找出工作的驗證參數
                string workKey = null;
                if (exitCode == 0)
                {
                    string errmsg = myHelper.GetInboundConfigWorkKey(out workKey);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -2;
                        exitMsg = String.Concat("讀取支付寶相關設定失敗，", errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLine(exitMsg);
                    }
                }
                #endregion

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
                            #region [MDY:20220910] Checkmarx - Use Of Broken Or Risky Cryptographic Algorithm 誤判調整
                            string checkCode = Fuju.Web.Focas.AlipayHelper.GenFiscServiceCheckCode(jobNo, workKey);
                            #endregion

                            fileLog.AppendFormat("CallFiscServiceChanegKey() 參數：webUrl={0}, jobNo={1}, checkCode={2}", webUrl, jobNo, checkCode).AppendLine();
                            string result = null;
                            string errmsg = myHelper.CallFiscServiceChanegKey(webUrl, jobNo, checkCode, out result);
                            if (String.IsNullOrEmpty(errmsg))
                            {
                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] Inbound 換 KEY 作業成功，回傳訊息：{1}", DateTime.Now, result).AppendLine();
                                fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] Inbound 換 KEY 作業成功，回傳訊息：{1}", DateTime.Now, result).AppendLine();
                            }
                            else
                            {
                                exitCode = -4;
                                exitMsg = errmsg;
                                if (String.IsNullOrEmpty(result))
                                {
                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();
                                    fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();
                                }
                                else
                                {
                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}，回傳訊息：{2}", DateTime.Now, errmsg, result).AppendLine();
                                    fileLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}，回傳訊息：{2}", DateTime.Now, errmsg, result).AppendLine();
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
                fileLog.AppendFormat("發生例外，命令參數：{0}，錯誤訊息：{1}", argsLine, ex.Message).AppendLine();
            }
            finally
            {
                #region 更新 Job
                if (jobNo > 0)
                {
                    string jobResultId = null;
                    if (exitCode == 0)
                    {
                        exitMsg = "Inbound 換 KEY 作業成功";
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

                myHelper.WriteFileLog(logFileName, fileLog);

                if (myHelper != null)
                {
                    myHelper.Dispose();
                    myHelper = null;
                }

                System.Environment.Exit(exitCode);
            }
        }

        //private static void WriteFileLog(string logFileName, StringBuilder log)
        //{
        //    if (log == null || log.Length == 0 || String.IsNullOrWhiteSpace(logFileName))
        //    {
        //        return;
        //    }
        //    string logPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
        //    if (String.IsNullOrEmpty(logPath))
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        DirectoryInfo info = new DirectoryInfo(logPath);
        //        if (!info.Exists)
        //        {
        //            info.Create();
        //        }

        //        string logFileFullName = Path.Combine(logPath, logFileName);
        //        using (StreamWriter sw = new StreamWriter(logFileFullName, true, Encoding.Default))
        //        {
        //            sw.WriteLine(log.ToString());
        //            sw.Flush();
        //        }
        //    }
        //    catch(Exception)
        //    {
        //    }
        //}
    }
}
