using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

using Entities;

namespace Deliver
{
    /// <summary>
    /// 傳送檔案至 Web 端 (CTC2：傳送中國信託學校檔和學生檔、SC2, SC2B：傳送學校銷帳檔)
    /// </summary>
    class Program
    {
        #region [MDY:2018xxxx] 取消 FileKind 列舉改用 FileKind_xxx 常數
        #region [OLD]
        //enum FileKind : int
        //{
        //    CTCB,
        //    CancelData
        //}
        #endregion

        #region Const
        /// <summary>
        /// 中信帳單檔類別
        /// </summary>
        private const string FileKind_CTCB = "CTCB_DATA";

        /// <summary>
        /// 學校銷帳檔類別
        /// </summary>
        private const string FileKind_Canceled = "CANCELED_DATA";
        #endregion
        #endregion

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

        #region Private Method
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

        #region [MDY:2018xxxx] 服務改呼叫 FTPUpload() 方法
        #region [OLD]
        //private static string SendFile(FileKind kind, string full_file_name)
        //{
        //    string log = "";
        //    string ap_id = ConfigurationManager.AppSettings.Get("ap_id");

        //    #region [MDY:20170717] 為了避過源碼掃描敏感字詞，所有密碼相關變數名稱改用 ap_pword
        //    string ap_pword = ConfigurationManager.AppSettings.Get("ap_pword");
        //    #endregion

        //    string file_kind = "";
        //    if (kind == FileKind.CTCB)
        //    {
        //        file_kind = "CTCB_DATA";
        //    }
        //    else if (kind == FileKind.CancelData)
        //    {
        //        file_kind = "CANCELED_DATA";
        //    }

        //    string config_file_name = ConfigurationManager.AppSettings.Get("config_file_name");
        //    byte[] contents = File.ReadAllBytes(full_file_name);

        //    #region [MDY:20170416] 配合系統參數設定機制，FileService 網址改取自 FileServiceUrl 系統參數值
        //    #region [Old]
        //    //string endpointConfigurationName = ConfigHelper.Current.GetFileServiceSoapEndpointConfigurationName();
        //    //FileService.FileServiceSoapClient client = new FileService.FileServiceSoapClient(endpointConfigurationName);
        //    #endregion

        //    string errmsg = null;
        //    FileService.FileServiceSoapClient client = GetFileServiceClient(out errmsg);
        //    if (!String.IsNullOrEmpty(errmsg))
        //    {
        //        return "取得 FileService 服務物件失敗，" + errmsg;
        //    }
        //    #endregion

        //    try
        //    {
        //        #region {MDY:20170717] 忽略不信任的憑證
        //        if (client.Endpoint.Address.Uri.Scheme == Uri.UriSchemeHttps)
        //        {
        //            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
        //        }
        //        #endregion

        //        log = client.ReceiveFile(ap_id, ap_pword, file_kind, config_file_name, contents);
        //    }
        //    catch (Exception ex)
        //    {
        //        log = ex.Message;
        //    }
        //    return log;
        //}
        #endregion

        /// <summary>
        /// 傳送檔案
        /// </summary>
        /// <param name="jobNo">作業代碼</param>
        /// <param name="jobTypeId">作業類別代碼</param>
        /// <param name="fileKind">檔案類別</param>
        /// <param name="fileFullName">發送檔完整路徑檔名</param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回 null</returns>
        private static string SendFile(int jobNo, string jobTypeId, string fileKind, string fileFullName)
        {
            string procName = null;  //處理程序名稱
            try
            {
                #region 檢查參數
                procName = "檢查參數";

                if (jobNo < 0)
                {
                    return "作業代碼參數不正確";
                }
                if (fileKind != FileKind_CTCB && fileKind != FileKind_Canceled)
                {
                    return "檔案類別參數不正確";
                }
                if (String.IsNullOrWhiteSpace(fileFullName))
                {
                    return "缺少發送檔參數或參數值不正確";
                }
                FileInfo fileInfo = new FileInfo(fileFullName);
                if (!fileInfo.Exists)
                {
                    return "發送檔不存在";
                }
                #endregion

                #region 讀取並檢查服務連線參數
                procName = "讀取服務連線參數";

                string apId = ConfigurationManager.AppSettings.Get("ap_id");
                if (String.IsNullOrWhiteSpace(apId))
                {
                    return "Config 未設定 Web 端檔案服務連線帳號參數";
                }

                string apCode = ConfigurationManager.AppSettings.Get("ap_code");
                if (String.IsNullOrWhiteSpace(apCode))
                {
                    return "Config 未設定 Web 端檔案服務連線代碼參數";
                }
                #endregion

                #region 產生服務執行參數
                procName = "產生服務執行參數";

                string apData = Fuju.Common.GetBase64Encode(String.Format("{0}_{1}_{2}_{3}_{4}", apId, jobNo, jobTypeId, fileKind, apCode));
                byte[] fileContents = File.ReadAllBytes(fileFullName);
                #endregion

                #region 呼叫服務
                procName = "呼叫服務";

                string errmsg = null;
                FileService.FileServiceSoapClient client = GetFileServiceClient(out errmsg);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    return String.Concat("取得 FileService 服務物件失敗，", errmsg);
                }

                #region 忽略不信任的憑證
                if (client.Endpoint.Address.Uri.Scheme == Uri.UriSchemeHttps)
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                }
                #endregion

                string result = client.FTPUpload(apData, jobNo, jobTypeId, fileKind, fileContents);
                if ("OK".Equals(result, StringComparison.CurrentCultureIgnoreCase))
                {
                    return null;
                }
                else
                {
                    return String.Concat("服務回傳訊息：", result);
                }
                #endregion
            }
            catch (Exception ex)
            {
                return String.Format("傳送檔案發生例外 [{0}]：{1}", procName, ex.Message);
            }
        }
        #endregion

        #region [MDY:20170416] 配合系統參數設定機制，FileService 網址改取自 FileServiceUrl 系統參數值
        /// <summary>
        /// FileService 網址系統參 KEY
        /// </summary>
        private static string FileServiceUrl_ConfigKey = "FileServiceUrl";

        /// <summary>
        /// 取得 FileServiceSoapClient
        /// </summary>
        /// <returns></returns>
        private static FileService.FileServiceSoapClient GetFileServiceClient(out string errmsg)
        {
            errmsg = null;
            FileService.FileServiceSoapClient client = null;

            try
            {
                #region 取得FileServiceUrl 系統參數值
                string fileServiceUrl = null;
                using (EntityFactory factory = new EntityFactory())
                {
                    ConfigEntity data = null;
                    Expression where = new Expression(ConfigEntity.Field.ConfigKey, FileServiceUrl_ConfigKey);
                    Result result = factory.SelectFirst<ConfigEntity>(where, null, out data);
                    if (result.IsSuccess)
                    {
                        if (data != null && data.ConfigValue != null)
                        {
                            fileServiceUrl = data.ConfigValue.Trim();
                        }
                        if (String.IsNullOrEmpty(fileServiceUrl))
                        {
                            errmsg = String.Format("未設定 {0} 系統參數值", FileServiceUrl_ConfigKey);
                        }
                    }
                    else
                    {
                        errmsg = result.Message;
                    }
                }
                #endregion

                if (String.IsNullOrEmpty(errmsg))
                {
                    if (fileServiceUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                    {
                        client = new FileService.FileServiceSoapClient("FileServiceSoap_HTTPS");
                        client.Endpoint.Address = new System.ServiceModel.EndpointAddress(fileServiceUrl);
                    }
                    else if (fileServiceUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase))
                    {
                        client = new FileService.FileServiceSoapClient("FileServiceSoap_HTTP");
                        client.Endpoint.Address = new System.ServiceModel.EndpointAddress(fileServiceUrl);
                    }
                    else
                    {
                        errmsg = String.Format("{0} 系統參數值不合法", FileServiceUrl_ConfigKey);
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }

            return client;
        }
        #endregion

        #region {MDY:20170717] 忽略不信任的憑證
        /// <summary>
        /// 處理不信任的憑證 (忽略)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion
        #endregion

        /// <summary>
        /// 參數：service_id=所屬作業類別代碼(CTC2、SC2、SC2B)
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
            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.Skip;  //因為產生的檔案不確定是哪一台伺服器，所以不檢查
            string jobTypeId = null;        //作業類別代碼
            string jobTypeName = null;      //作業類別名稱
            int exitCode = 0;
            string exitMsg = null;
            #endregion

            StringBuilder log = new StringBuilder();    //日誌紀錄

            //暫時不提供 retry 
            //int retryTimes = 5;     //re-try 次數 (預設為5次)
            //int retrySleep = 15;    //re-try 間隔，單位分鐘 (預設為15分鐘)

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
                                    case "service_id":
                                        #region jobTypeId (所屬作業類別代碼)
                                        {
                                            jobTypeId = value;
                                            switch (jobTypeId)
                                            {
                                                case JobCubeTypeCodeTexts.CTC2:
                                                case JobCubeTypeCodeTexts.SC2:
                                                case JobCubeTypeCodeTexts.SC2B:
                                                    jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);
                                                    break;
                                                default:
                                                    errmsg = "service_id 參數值不是正確的作業類別代碼 (CTC2、SC2、SC2B)";
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
                    if (String.IsNullOrEmpty(errmsg) && String.IsNullOrEmpty(jobTypeId))
                    {
                        errmsg = "缺少 service_id 參數";
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
                            log.AppendLine("參數語法：service_id=所屬作業類別代碼(CTC2、SC2、SC2B)");
                        }
                    }
                }
                #endregion

                #region 處理 Config 參數
                #region 指示檔名稱
                string config_file_name = null;
                if (exitCode == 0)
                {
                    config_file_name = ConfigurationManager.AppSettings.Get("config_file_name");
                    if (String.IsNullOrWhiteSpace(config_file_name))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定指示檔名稱 (config_file_name) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                    else
                    {
                        config_file_name = config_file_name.Trim();
                    }
                }
                #endregion

                #region 待傳送檔案路徑
                string data_path = null;
                if (exitCode == 0)
                {
                    data_path = ConfigurationManager.AppSettings.Get("DATA_PATH");
                    if (String.IsNullOrWhiteSpace(data_path))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定待傳送檔案路徑 (DATA_PATH) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                    else
                    {
                        data_path = data_path.Trim();
                    }
                }
                #endregion

                #region 暫存檔路徑
                string temp_path = null;
                if (exitCode == 0)
                {
                    temp_path = ConfigurationManager.AppSettings.Get("TEMP_PATH");
                    if (String.IsNullOrWhiteSpace(temp_path))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定暫存檔路徑 (TEMP_PATH) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                    else
                    {
                        temp_path = temp_path.Trim();
                    }
                }
                #endregion

                #region 備份檔路徑
                string bak_path = null;
                if (exitCode == 0)
                {
                    bak_path = ConfigurationManager.AppSettings.Get("BAK_PATH");
                    if (String.IsNullOrWhiteSpace(bak_path))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定備份檔路徑 (BAK_PATH) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                    else
                    {
                        bak_path = bak_path.Trim();
                        string errmsg = CheckOrCreateFolder(bak_path);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            exitCode = -1;
                            exitMsg = String.Format("Config (BAK_PATH)參數錯誤，{0}", errmsg);
                            jobLog.AppendLine(exitMsg);
                            log.AppendLine(exitMsg);
                        }
                    }
                }
                #endregion

                #region 學校銷帳資料檔路徑
                string cancel_data_path = null;
                if (exitCode == 0)
                {
                    cancel_data_path = ConfigurationManager.AppSettings.Get("cancel_data_path");
                    if (String.IsNullOrWhiteSpace(cancel_data_path))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定學校銷帳資料檔路徑 (cancel_data_path) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                    else
                    {
                        cancel_data_path = cancel_data_path.Trim();
                    }
                }
                #endregion

                #region 中國信託資料檔路徑
                string ctcb_data_path = null;
                if (exitCode == 0)
                {
                    ctcb_data_path = ConfigurationManager.AppSettings.Get("ctcb_data_path");
                    if (String.IsNullOrWhiteSpace(ctcb_data_path))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定中國信託資料檔路徑 (ctcb_data_path) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                    else
                    {
                        ctcb_data_path = ctcb_data_path.Trim();
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

                #region 傳送檔案
                if (exitCode == 0)
                {
                    string chkKind = jobTypeId == JobCubeTypeCodeTexts.CTC2 ? "CTC2" : "SC2";
                    string chkName = "Global\\" + chkKind + "_" + appGuid;
                    using (Mutex m = new Mutex(false, chkName))    //全域不可重複執行
                    {
                        //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                        if (m.WaitOne(0, false))
                        {
                            switch (jobTypeId)
                            {
                                case JobCubeTypeCodeTexts.SC2:
                                case JobCubeTypeCodeTexts.SC2B:
                                    #region 處理學校銷帳檔
                                    {
                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始處理學校銷帳檔", DateTime.Now).AppendLine();

                                        try
                                        {
                                            #region 處理暫存檔路徑
                                            {
                                                temp_path = Path.Combine(temp_path, "CANCELED_DATA");
                                                string errmsg = CheckOrCreateFolder(temp_path, true);
                                                if (!String.IsNullOrEmpty(errmsg))
                                                {
                                                    exitCode = -3;
                                                    exitMsg = String.Format("處理學校銷帳暫存檔路徑失敗，{0}", errmsg);
                                                }
                                            }
                                            #endregion

                                            if (exitCode == 0)
                                            {
                                                #region 搜尋每個資料夾，搬檔案，並且收集指示檔內容
                                                bool hasFile = false;
                                                List<string> cmds = new List<string>(); //儲存每個指示檔內容
                                                {
                                                    DirectoryInfo di = new DirectoryInfo(cancel_data_path);
                                                    if (di.Exists)
                                                    {
                                                        DirectoryInfo[] subDInfos = di.GetDirectories();
                                                        foreach (DirectoryInfo subDInfo in subDInfos)
                                                        {
                                                            #region [Old] 確認是否有ok檔
                                                            #region [Old]
                                                            //if (!findFile(d.FullName, "ok"))
                                                            //{
                                                            //    //沒有找到ok檔，表示該資料夾沒處理完成，或是有錯誤，該資料夾就跳過
                                                            //    continue;
                                                            //}
                                                            #endregion

                                                            //{
                                                            //    FileInfo[] okFInfos = subDInfo.GetFiles("ok", SearchOption.TopDirectoryOnly);
                                                            //    if (okFInfos == null || okFInfos.Length == 0)
                                                            //    {
                                                            //        //沒有找到ok檔，表示該資料夾沒處理完成，或是有錯誤，該資料夾就跳過
                                                            //        fileLog.WriteLog("資料夾 ({0}) 無 ok 檔", subDInfo.FullName);
                                                            //        continue;
                                                            //    }
                                                            //}
                                                            #endregion

                                                            #region 處理每個資料夾下的檔案，資料夾下應該有3個檔案
                                                            {
                                                                bool isOK = true;
                                                                string cmd = null;
                                                                List<string> dataFileNames = new List<string>();
                                                                bool hasOK = false;
                                                                bool hasCmd = false;
                                                                bool hasData = false;
                                                                FileInfo[] fInfos = subDInfo.GetFiles();
                                                                foreach (FileInfo fInfo in fInfos)
                                                                {
                                                                    string fileName = fInfo.Name.ToLower();
                                                                    if (fileName.Equals(config_file_name, StringComparison.CurrentCultureIgnoreCase))
                                                                    {
                                                                        #region 指示檔
                                                                        try
                                                                        {
                                                                            hasCmd = true;
                                                                            cmd = File.ReadAllText(fInfo.FullName).Trim(new char[] { '\r', '\n' });
                                                                            log.AppendFormat("指示檔 ({0}) 內容：\r\n{1}", fInfo.FullName, cmd).AppendLine();
                                                                            //fileLog.WriteLog("指示檔 ({0}) 內容：\r\n{1}", fInfo.FullName, cmd);

                                                                            //讀完就刪除
                                                                            File.Delete(fInfo.FullName);
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            isOK = false;
                                                                            log.AppendFormat("處理指示檔 ({0}) 發生例外，錯誤訊息：{1}", fInfo.FullName, ex.Message).AppendLine();
                                                                            //fileLog.WriteLog("處理指示檔 ({0}) 發生例外，錯誤訊息：{1}", fInfo.FullName, ex.Message);
                                                                        }
                                                                        #endregion
                                                                    }
                                                                    else if (fileName == "ok")
                                                                    {
                                                                        #region OK 檔
                                                                        try
                                                                        {
                                                                            hasOK = true;
                                                                            File.Delete(fInfo.FullName);
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            isOK = false;
                                                                            log.AppendFormat("刪除 OK 檔 ({0}) 發生例外，錯誤訊息：{1}", fInfo.FullName, ex.Message).AppendLine();
                                                                            //fileLog.WriteLog("刪除 OK 檔 ({0}) 發生例外，錯誤訊息：{1}", fInfo.FullName, ex.Message);
                                                                        }
                                                                        #endregion
                                                                    }
                                                                    else
                                                                    {
                                                                        #region 資料檔
                                                                        hasData = true;

                                                                        //搬檔案，準備壓縮
                                                                        string tmpFileFullName = Path.Combine(temp_path, fInfo.Name);
                                                                        try
                                                                        {
                                                                            dataFileNames.Add(tmpFileFullName);
                                                                            File.Move(fInfo.FullName, tmpFileFullName);
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            isOK = false;
                                                                            log.AppendFormat("移動資料檔 ({0}) 至 {1} 發生例外，錯誤訊息：{2}", fInfo.FullName, tmpFileFullName, ex.Message).AppendLine();
                                                                            //fileLog.WriteLog("移動資料檔 ({0}) 至 {1} 發生例外，錯誤訊息：{2}", fInfo.FullName, tmpFileFullName, ex.Message);
                                                                        }
                                                                        #endregion
                                                                    }
                                                                }

                                                                if (!hasOK)
                                                                {
                                                                    isOK = false;
                                                                    log.AppendFormat("資料夾 ({0}) 缺少 OK 檔", subDInfo.FullName).AppendLine();
                                                                    //fileLog.WriteLog("資料夾 ({0}) 缺少 OK 檔", subDInfo.FullName);
                                                                }
                                                                if (!hasData)
                                                                {
                                                                    isOK = false;
                                                                    log.AppendFormat("資料夾 ({0}) 缺少資料檔", subDInfo.FullName).AppendLine();
                                                                    //fileLog.WriteLog("資料夾 ({0}) 缺少資料檔", subDInfo.FullName);
                                                                }
                                                                if (!hasCmd)
                                                                {
                                                                    isOK = false;
                                                                    log.AppendFormat("資料夾 ({0}) 缺少指示檔", subDInfo.FullName).AppendLine();
                                                                    //fileLog.WriteLog("資料夾 ({0}) 缺少指示檔", subDInfo.FullName);
                                                                }

                                                                if (isOK)
                                                                {
                                                                    hasFile = true;
                                                                    cmds.Add(cmd);
                                                                }
                                                                else
                                                                {
                                                                    log.AppendFormat("資料夾 ({0}) 檔案不完整或處理失敗，此資料夾的資料不寄送", subDInfo.FullName).AppendLine();
                                                                    //fileLog.WriteLog("資料夾 ({0}) 檔案不完整或處理失敗，此資料夾的資料不寄送", subDInfo.FullName);
                                                                    if (dataFileNames.Count > 0)
                                                                    {
                                                                        foreach (string dataFileName in dataFileNames)
                                                                        {
                                                                            try
                                                                            {
                                                                                File.Delete(dataFileName);
                                                                            }
                                                                            catch (Exception)
                                                                            {
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                }
                                                #endregion

                                                if (hasFile)
                                                {
                                                    bool isBreak = false;
                                                    bool hasZipFile = false;

                                                    #region 寫指示檔
                                                    //string full_config_file_name = string.Format("{0}{1}", temp_path, config_file_name);
                                                    string full_config_file_name = Path.Combine(temp_path, config_file_name);
                                                    try
                                                    {
                                                        using (StreamWriter sw = new StreamWriter(full_config_file_name, false, Encoding.Default))
                                                        {
                                                            foreach (string cmd in cmds)
                                                            {
                                                                sw.WriteLine(cmd);
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        isBreak = true;
                                                        exitCode = -3;
                                                        exitMsg = String.Format("產生指示檔 ({0}) 發生例外，錯誤訊息：{1}", full_config_file_name, ex.Message);
                                                        jobLog.AppendLine(exitMsg);
                                                        log.AppendLine(exitMsg);
                                                        //fileLog.WriteLog("產生指示檔 ({0}) 發生例外，錯誤訊息：{1}", full_config_file_name, ex.Message);
                                                    }
                                                    #endregion

                                                    #region 壓縮
                                                    string zip_file_name = string.Format("canceldata.{0}.zip", DateTime.Now.ToString("yyyyMMddHHmmss"));
                                                    //string full_zip_file_name = string.Format("{0}{1}", data_path, zip_file_name);
                                                    string full_zip_file_name = Path.Combine(data_path, zip_file_name);
                                                    if (!isBreak)
                                                    {
                                                        try
                                                        {
                                                            ZIPHelper.ZipDir(temp_path, full_zip_file_name);
                                                            hasZipFile = true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            isBreak = true;
                                                            exitCode = -3;
                                                            exitMsg = String.Format("壓縮資料夾 ({0}) 產生壓縮檔 ({1}) 發生例外，錯誤訊息：{2}", temp_path, full_zip_file_name, ex.Message);
                                                            jobLog.AppendLine(exitMsg);
                                                            log.AppendLine(exitMsg);
                                                            //fileLog.WriteLog("壓縮資料夾 ({0}) 產生壓縮檔 ({1}) 發生例外，錯誤訊息：{2}", temp_path, full_zip_file_name, ex.Message);
                                                        }
                                                    }
                                                    #endregion

                                                    #region 傳送
                                                    if (!isBreak)
                                                    {
                                                        #region [MDY:2018xxxx] 服務改呼叫 FTPUpload() 方法
                                                        #region [OLD]
                                                        //string result = SendFile(FileKind.CancelData, full_zip_file_name);
                                                        //if (result != null && result.Equals("ok", StringComparison.CurrentCultureIgnoreCase))
                                                        //{
                                                        //    exitMsg = String.Format("傳送檔案 ({0}) 至 Web 端成功", full_zip_file_name);
                                                        //    jobLog.AppendLine(exitMsg);
                                                        //    log.AppendLine(exitMsg);
                                                        //    //fileLog.WriteLog("傳送檔案 ({0}) 至 Web 端成功", full_zip_file_name);
                                                        //}
                                                        //else
                                                        //{
                                                        //    exitCode = -3;
                                                        //    exitMsg = String.Format("傳送檔案 ({0}) 至 Web 端成功失敗，回傳訊息：{1}", full_zip_file_name, result);
                                                        //    jobLog.AppendLine(exitMsg);
                                                        //    log.AppendLine(exitMsg);
                                                        //    //fileLog.WriteLog("傳送檔案 ({0}) 至 Web 端成功失敗，回傳訊息：{1}", full_zip_file_name, result);
                                                        //}
                                                        #endregion

                                                        string errmsg = SendFile(jobNo, jobTypeId, FileKind_Canceled, full_zip_file_name);
                                                        if (String.IsNullOrWhiteSpace(errmsg))
                                                        {
                                                            exitMsg = String.Format("傳送檔案 ({0}) 至 Web 端成功。", full_zip_file_name);
                                                            jobLog.AppendLine(exitMsg);
                                                            log.AppendLine(exitMsg);
                                                        }
                                                        else
                                                        {
                                                            exitCode = -3;
                                                            exitMsg = String.Format("傳送檔案 ({0}) 至 Web 端失敗。{1}", full_zip_file_name, errmsg);
                                                            jobLog.AppendLine(exitMsg);
                                                            log.AppendLine(exitMsg);
                                                        }
                                                        #endregion
                                                    }
                                                    #endregion

                                                    #region 備份
                                                    if (hasZipFile)
                                                    {
                                                        try
                                                        {
                                                            //File.Move(full_zip_file_name, string.Format("{0}{1}", bak_path, zip_file_name));
                                                            File.Move(full_zip_file_name, Path.Combine(bak_path, zip_file_name));
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            log.AppendFormat("備份檔案 ({0}) 發生例外，錯誤訊息：{1}", full_zip_file_name, ex.Message).AppendLine();
                                                            //fileLog.WriteLog("備份檔案 ({0}) 發生例外，錯誤訊息：{1}", full_zip_file_name, ex.Message);
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    exitCode = -3;
                                                    exitMsg = "找不到任何有效的資料檔";
                                                    jobLog.AppendLine(exitMsg);
                                                    log.AppendLine(exitMsg);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            exitCode = -8;
                                            exitMsg = String.Format("處理學校銷帳檔發生例外，錯誤訊息：{0}", ex.Message);
                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                        }

                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束處理學校銷帳檔", DateTime.Now).AppendLine();
                                    }
                                    #endregion
                                    break;
                                case JobCubeTypeCodeTexts.CTC2:
                                    #region 處理中國信託檔案
                                    {
                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始處理中國信託檔案", DateTime.Now).AppendLine();

                                        try
                                        {
                                            #region 處理暫存檔路徑
                                            {
                                                temp_path = Path.Combine(temp_path, "CTCB_DATA");
                                                string errmsg = CheckOrCreateFolder(temp_path, true);
                                                if (!String.IsNullOrEmpty(errmsg))
                                                {
                                                    exitCode = -3;
                                                    exitMsg = String.Format("處理中國信託暫存檔路徑失敗，{0}", errmsg);
                                                }
                                            }
                                            #endregion

                                            if (exitCode == 0)
                                            {
                                                #region 準備檔案
                                                bool hasFile = false;
                                                List<string> cmds = new List<string>();

                                                DirectoryInfo dInfo = new DirectoryInfo(ctcb_data_path);
                                                if (dInfo.Exists)
                                                {
                                                    #region [Old]
                                                    //if (findFile(di.FullName, "ok"))
                                                    //{
                                                    //    FileInfo[] fis = di.GetFiles();
                                                    //    foreach (FileInfo fi in fis)
                                                    //    {
                                                    //        string file_name = fi.Name;
                                                    //        if (file_name.ToLower() == config_file_name.ToLower())
                                                    //        {
                                                    //            #region 讀指示檔
                                                    //            try
                                                    //            {
                                                    //                using (StreamReader sr = new StreamReader(fi.FullName, Encoding.Default))
                                                    //                {
                                                    //                    string content = sr.ReadToEnd();
                                                    //                    cmds.Add(content);
                                                    //                }
                                                    //                //sr.Close();
                                                    //                File.Delete(fi.FullName);
                                                    //            }
                                                    //            catch (Exception ex)
                                                    //            {

                                                    //            }
                                                    //            #endregion
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            #region 搬檔案，準備壓縮
                                                    //            if (file_name == "ok")
                                                    //            {
                                                    //                File.Delete(fi.FullName);
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                hasFile = true;
                                                    //                //File.Move(fi.FullName, string.Format("{0}", temp_path, file_name));
                                                    //                File.Move(fi.FullName, Path.Combine(temp_path, file_name));
                                                    //            }
                                                    //            #endregion
                                                    //        }
                                                    //    }
                                                    //}
                                                    //else
                                                    //{

                                                    //}
                                                    #endregion

                                                    bool isOK = true;
                                                    string cmd = null;
                                                    List<string> dataFileNames = new List<string>();
                                                    bool hasOK = false;
                                                    bool hasCmd = false;
                                                    bool hasData = false;
                                                    FileInfo[] fInfos = dInfo.GetFiles();
                                                    foreach (FileInfo fInfo in fInfos)
                                                    {
                                                        string fileName = fInfo.Name.ToLower();
                                                        if (fileName.Equals(config_file_name, StringComparison.CurrentCultureIgnoreCase))
                                                        {
                                                            #region 指示檔
                                                            try
                                                            {
                                                                hasCmd = true;
                                                                cmd = File.ReadAllText(fInfo.FullName).Trim(new char[] { '\r', '\n' });
                                                                log.AppendFormat("指示檔 ({0}) 內容：\r\n{1}", fInfo.FullName, cmd).AppendLine();
                                                                //fileLog.WriteLog("指示檔 ({0}) 內容：\r\n{1}", fInfo.FullName, cmd);

                                                                //讀完就刪除
                                                                File.Delete(fInfo.FullName);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                isOK = false;
                                                                log.AppendFormat("處理指示檔 ({0}) 發生例外，錯誤訊息：{1}", fInfo.FullName, ex.Message).AppendLine();
                                                                //fileLog.WriteLog("處理指示檔 ({0}) 發生例外，錯誤訊息：{1}", fInfo.FullName, ex.Message);
                                                            }
                                                            #endregion
                                                        }
                                                        else if (fileName == "ok")
                                                        {
                                                            #region OK 檔
                                                            try
                                                            {
                                                                hasOK = true;
                                                                File.Delete(fInfo.FullName);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                isOK = false;
                                                                log.AppendFormat("刪除 OK 檔 ({0}) 發生例外，錯誤訊息：{1}", fInfo.FullName, ex.Message).AppendLine();
                                                                //fileLog.WriteLog("刪除 OK 檔 ({0}) 發生例外，錯誤訊息：{1}", fInfo.FullName, ex.Message);
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            #region 資料檔
                                                            hasData = true;

                                                            //搬檔案，準備壓縮
                                                            string tmpFileFullName = Path.Combine(temp_path, fInfo.Name);
                                                            try
                                                            {
                                                                dataFileNames.Add(tmpFileFullName);
                                                                File.Move(fInfo.FullName, tmpFileFullName);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                isOK = false;
                                                                log.AppendFormat("移動資料檔 ({0}) 至 {1} 發生例外，錯誤訊息：{2}", fInfo.FullName, tmpFileFullName, ex.Message).AppendLine();
                                                                //fileLog.WriteLog("移動資料檔 ({0}) 至 {1} 發生例外，錯誤訊息：{2}", fInfo.FullName, tmpFileFullName, ex.Message);
                                                            }
                                                            #endregion
                                                        }
                                                    }

                                                    if (!hasOK)
                                                    {
                                                        isOK = false;
                                                        log.AppendFormat("資料夾 ({0}) 缺少 OK 檔", dInfo.FullName).AppendLine();
                                                        //fileLog.WriteLog("資料夾 ({0}) 缺少 OK 檔", dInfo.FullName);
                                                    }
                                                    if (!hasData)
                                                    {
                                                        isOK = false;
                                                        log.AppendFormat("資料夾 ({0}) 缺少資料檔", dInfo.FullName).AppendLine();
                                                        //fileLog.WriteLog("資料夾 ({0}) 缺少資料檔", dInfo.FullName);
                                                    }
                                                    if (!hasCmd)
                                                    {
                                                        isOK = false;
                                                        log.AppendFormat("資料夾 ({0}) 缺少指示檔", dInfo.FullName).AppendLine();
                                                        //fileLog.WriteLog("資料夾 ({0}) 缺少指示檔", dInfo.FullName);
                                                    }

                                                    if (isOK)
                                                    {
                                                        hasFile = true;
                                                        cmds.Add(cmd);
                                                    }
                                                    else
                                                    {
                                                        log.AppendFormat("資料夾 ({0}) 檔案不完整或處理失敗，此資料夾的資料不寄送", dInfo.FullName).AppendLine().AppendLine();
                                                        //fileLog.WriteLog("資料夾 ({0}) 檔案不完整或處理失敗，此資料夾的資料不寄送", dInfo.FullName);
                                                        if (dataFileNames.Count > 0)
                                                        {
                                                            foreach (string dataFileName in dataFileNames)
                                                            {
                                                                try
                                                                {
                                                                    File.Delete(dataFileName);
                                                                }
                                                                catch (Exception)
                                                                {
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion

                                                if (hasFile)
                                                {
                                                    bool isBreak = false;
                                                    bool hasZipFile = false;

                                                    #region 寫指示檔
                                                    //string full_config_file_name = string.Format("{0}{1}", temp_path, config_file_name);
                                                    string full_config_file_name = Path.Combine(temp_path, config_file_name);
                                                    try
                                                    {
                                                        using (StreamWriter sw = new StreamWriter(full_config_file_name, false, Encoding.Default))
                                                        {
                                                            foreach (string cmd in cmds)
                                                            {
                                                                sw.WriteLine(cmd);
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        isBreak = true;
                                                        exitCode = -3;
                                                        exitMsg = String.Format("產生指示檔 ({0}) 發生例外，錯誤訊息：{1}", full_config_file_name, ex.Message);
                                                        jobLog.AppendLine(exitMsg);
                                                        log.AppendLine(exitMsg);
                                                        //fileLog.WriteLog("產生指示檔 ({0}) 發生例外，錯誤訊息：{1}", full_config_file_name, ex.Message);
                                                    }
                                                    #endregion

                                                    #region 壓縮
                                                    string zip_file_name = string.Format("ctcbdata.{0}.zip", DateTime.Now.ToString("yyyyMMddHHmmss"));
                                                    //string full_zip_file_name = string.Format("{0}{1}", data_path, zip_file_name);
                                                    string full_zip_file_name = Path.Combine(data_path, zip_file_name);
                                                    if (!isBreak)
                                                    {
                                                        try
                                                        {
                                                            ZIPHelper.ZipDir(temp_path, full_zip_file_name);
                                                            hasZipFile = true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            isBreak = true;
                                                            exitCode = -3;
                                                            exitMsg = String.Format("壓縮資料夾 ({0}) 產生壓縮檔 ({1}) 發生例外，錯誤訊息：{2}", temp_path, full_zip_file_name, ex.Message);
                                                            jobLog.AppendLine(exitMsg);
                                                            log.AppendLine(exitMsg);
                                                            //fileLog.WriteLog("壓縮資料夾 ({0}) 產生壓縮檔 ({1}) 發生例外，錯誤訊息：{2}", temp_path, full_zip_file_name, ex.Message);
                                                        }
                                                    }
                                                    #endregion

                                                    #region 傳送
                                                    if (!isBreak)
                                                    {
                                                        #region [MDY:2018xxxx] 服務改呼叫 FTPUpload() 方法
                                                        #region [OLD]
                                                        //string result = SendFile(FileKind.CTCB, full_zip_file_name);
                                                        //if (result != null && result.Equals("ok", StringComparison.CurrentCultureIgnoreCase))
                                                        //{
                                                        //    exitMsg = String.Format("傳送檔案 ({0}) 至 Web 端成功", full_zip_file_name);
                                                        //    jobLog.AppendLine(exitMsg);
                                                        //    log.AppendLine(exitMsg);
                                                        //    //fileLog.WriteLog("傳送檔案 ({0}) 至 Web 端成功", full_zip_file_name);
                                                        //}
                                                        //else
                                                        //{
                                                        //    exitCode = -3;
                                                        //    exitMsg = String.Format("傳送檔案 ({0}) 至 Web 端成功失敗，回傳訊息：{1}", full_zip_file_name, result);
                                                        //    jobLog.AppendLine(exitMsg);
                                                        //    log.AppendLine(exitMsg);
                                                        //    //fileLog.WriteLog("傳送檔案 ({0}) 至 Web 端成功失敗，回傳訊息：{1}", full_zip_file_name, result);
                                                        //}
                                                        #endregion

                                                        string errmsg = SendFile(jobNo, jobTypeId, FileKind_CTCB, full_zip_file_name);
                                                        if (String.IsNullOrWhiteSpace(errmsg))
                                                        {
                                                            exitMsg = String.Format("傳送檔案 ({0}) 至 Web 端成功。", full_zip_file_name);
                                                            jobLog.AppendLine(exitMsg);
                                                            log.AppendLine(exitMsg);
                                                        }
                                                        else
                                                        {
                                                            exitCode = -3;
                                                            exitMsg = String.Format("傳送檔案 ({0}) 至 Web 端失敗。{1}", full_zip_file_name, errmsg);
                                                            jobLog.AppendLine(exitMsg);
                                                            log.AppendLine(exitMsg);
                                                        }
                                                        #endregion
                                                    }
                                                    #endregion

                                                    #region 備份
                                                    if (hasZipFile)
                                                    {
                                                        try
                                                        {
                                                            //File.Move(full_zip_file_name, string.Format("{0}{1}", bak_path, zip_file_name));
                                                            File.Move(full_zip_file_name, Path.Combine(bak_path, zip_file_name));
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            log.AppendFormat("備份檔案 ({0}) 發生例外，錯誤訊息：{1}", full_zip_file_name, ex.Message).AppendLine();
                                                            //fileLog.WriteLog("備份檔案 ({0}) 發生例外，錯誤訊息：{1}", full_zip_file_name, ex.Message);
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    exitCode = -3;
                                                    exitMsg = "找不到任何有效的資料檔";
                                                    jobLog.AppendLine(exitMsg);
                                                    log.AppendLine(exitMsg);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            exitCode = -8;
                                            exitMsg = String.Format("處理學校銷帳檔發生例外，錯誤訊息：{0}", ex.Message);
                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                        }

                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束處理中國信託檔案", DateTime.Now).AppendLine();
                                    }
                                    #endregion
                                    break;
                            }

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
