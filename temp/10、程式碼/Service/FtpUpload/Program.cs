using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

using Fuju;

using Fuju.DB;
using Fuju.DB.Data;

using Fuju.Configuration;

using Fuju.Web;
using Fuju.Web.Proxy;
using Fuju.Web.Services;

using Entities;

#region [OLD:20211001] M202110_01 改用 Fuju.FTP 元件 (2021擴充案先做)
//using Common.Utility;
#endregion

using Fuju.FTP;

namespace FtpUpload
{
    /// <summary>
    /// FTP 檔案上傳
    /// </summary>
    class Program
    {
        #region FileLog 相關
        /// <summary>
        /// 日誌檔類別
        /// </summary>
        private class FileLoger
        {
            #region Property
            private string _LogName = null;
            /// <summary>
            /// 日誌檔主要名稱 (App 名稱)
            /// </summary>
            public string LogName
            {
                get
                {
                    return _LogName;
                }
                private set
                {
                    _LogName = value == null ? String.Empty : value.Trim();
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
            /// 日誌檔完整路徑名稱 (日誌檔路徑 / 主要名稱 + "_" + 當天日期(yyyyMMdd) + ".log")
            /// </summary>
            private string _LogFileName = null;
            public string LogFileName
            {
                get
                {
                    return _LogFileName;
                }
                private set
                {
                    _LogFileName = value == null ? String.Empty : value.Trim();
                }
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 日誌檔類別 物件
            /// </summary>
            /// <param name="logName"></param>
            public FileLoger(string logName)
            {
                this.LogName = logName;
                this.LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                string logMode = ConfigurationManager.AppSettings.Get("LOG_MODE");
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

            /// <summary>
            /// 建構 日誌檔類別 物件
            /// </summary>
            /// <param name="logName"></param>
            /// <param name="logPath"></param>
            /// <param name="isDebug"></param>
            public FileLoger(string logName, string logPath, bool isDebug)
            {
                this.LogName = logName;
                this.LogPath = logPath;
                this.IsDebug = isDebug;
                this.Initial();
            }
            #endregion

            #region Initial
            private string Initial()
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
            #endregion

            #region WriteLog 相關
            /// <summary>
            /// 寫入日誌訊息
            /// </summary>
            /// <param name="msg">日誌訊息</param>
            /// <param name="timeInfo">是否在訊息前面紀錄時間資訊</param>
            public void WriteLog(string msg, bool timeInfo = true)
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
                                if (timeInfo)
                                {
                                    sw.WriteLine("[{0:HH:mm:ss}] {1}", DateTime.Now, msg);
                                }
                                else
                                {
                                    sw.WriteLine(msg);
                                }
                            }
                        }
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

            public void WriteDebugLog(string msg, bool timeInfo = true)
            {
                if (this.IsDebug)
                {
                    this.WriteLog(msg, timeInfo);
                }
            }

            public void WriteDebugLog(StringBuilder msg)
            {
                if (this.IsDebug)
                {
                    this.WriteLog(msg);
                }
            }
            #endregion
        }
        #endregion

        #region UploadHelper 相關
        private class UploadHelper
        {
            #region Property
            private string _LocalPath = null;
            /// <summary>
            /// 本地端路徑
            /// </summary>
            public string LocalPath
            {
                get
                {
                    return _LocalPath;
                }
                private set
                {
                    _LocalPath = value == null ? String.Empty : value.Trim();
                }
            }

            private string _BakPath = null;
            /// <summary>
            /// 備份路徑
            /// </summary>
            public string BakPath
            {
                get
                {
                    return _BakPath;
                }
                private set
                {
                    _BakPath = value == null ? String.Empty : value.Trim();
                }
            }
            #endregion

            #region Constructor
            public UploadHelper(string localPath, string bakPath)
            {
                this.LocalPath = localPath;
                this.BakPath = bakPath;
            }
            #endregion

            #region Method
            #region [MDY:20211001] M202110_01 改用 Fuju.FTP 元件 (2021擴充案先做)
            FTPHelper _FTPHelper = new FTPHelper();
            private string[] separator = new string[] { " " };
            /// <summary>
            /// 上傳參數指定的檔案
            /// </summary>
            /// <param name="argTxt">參數</param>
            /// <param name="result"></param>
            /// <returns></returns>
            public bool Upload(string argTxt, out string result)
            {
                bool isOK = true;
                result = null;
                StringBuilder log = new StringBuilder();

                try
                {
                    #region 拆解參數
                    HostConfig hostConfig = null;   //FTP 遠端伺服器設定
                    string remoteFile = null;       //遠端檔案
                    string localFile = null;        //本地端檔案
                    {
                        string protocol = "FTP";    //FTP 協定（預設 FTP 以便與舊指示檔相容）
                        string host = null;         //FTP 伺服器
                        Int32? port = null;         //FTP 埠號
                        string uid = null;          //FTP 帳號
                        string pword = null;        //FTP 密碼

                        //TODO：[待確定] 舊的指示檔如果沒有用到 ssl、codepage 參數就移除
                        bool useSSL = false;        //是否使用 SSL 協定
                        int codepage = 950;         //FTP 編碼
                        Encoding encoding = Encoding.GetEncoding(codepage);

                        string remotePath = null;   //遠端路徑
                        if (!String.IsNullOrWhiteSpace(argTxt))
                        {
                            #region 解析參數
                            string[] args = argTxt.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string arg in args)
                            {
                                string[] kvs = arg.Split('=');
                                if (kvs.Length == 2)
                                {
                                    string key = kvs[0].Trim().ToLower();
                                    string value = kvs[1].Trim();
                                    switch (key)
                                    {
                                        case "protocol":
                                            #region protocol (FTP 協定)
                                            {
                                                protocol = value.ToUpper();
                                                if (protocol != "FTP" && protocol != "FTPS" && protocol != "SFTP")
                                                {
                                                    isOK = false;
                                                    log.AppendLine("protocol 參數值不正確");
                                                }
                                            }
                                            break;
                                            #endregion
                                        case "host":
                                            #region host (FTP 伺服器)
                                            {
                                                host = value.ToLower();
                                            }
                                            break;
                                            #endregion
                                        case "port":
                                            #region port (FTP 埠號)
                                            if (!String.IsNullOrEmpty(value))
                                            {
                                                int val = 0;
                                                if (Int32.TryParse(value, out val) && val > 0)
                                                {
                                                    port = val;
                                                }
                                                else
                                                {
                                                    isOK = false;
                                                    log.AppendLine("port 參數值不正確");
                                                }
                                            }
                                            break;
                                            #endregion
                                        case "uid":
                                            #region uid (FTP 帳號)
                                            {
                                                uid = value;
                                            }
                                            break;
                                            #endregion
                                        case "pwd":
                                            #region pwd (FTP 密碼)
                                            {
                                                pword = value;
                                            }
                                            break;
                                            #endregion

                                        //TODO：[待確定] 舊的指示檔如果沒有用到 ssl、codepage 參數就移除
                                        case "ssl":
                                            #region useSSL (是否使用 SSL 協定)
                                            {
                                                useSSL = value.Equals("true", StringComparison.CurrentCultureIgnoreCase);
                                            }
                                            break;
                                            #endregion
                                        case "codepage":
                                            #region codepage (FTP 編碼)
                                            if (!String.IsNullOrEmpty(value))
                                            {
                                                try
                                                {
                                                    codepage = Int32.Parse(value);
                                                    encoding = Encoding.GetEncoding(codepage);
                                                }
                                                catch
                                                {
                                                    isOK = false;
                                                    log.AppendLine("codepage 參數值不正確");
                                                }
                                            }
                                            break;
                                            #endregion

                                        case "remote_path":
                                            #region remote_path (遠端路徑)
                                            if (!String.IsNullOrEmpty(value))
                                            {
                                                remotePath = value;
                                                if (!remotePath.StartsWith(@"/"))
                                                {
                                                    isOK = false;
                                                    log.AppendLine("remote_path 參數值不正確 (必須是相對路徑)");
                                                }
                                            }
                                            break;
                                            #endregion
                                        case "remote_file":
                                            #region remote_file (遠端檔案)
                                            if (!String.IsNullOrEmpty(value))
                                            {
                                                remoteFile = value;
                                                if (remoteFile.IndexOf(@"/") > -1 || remoteFile.IndexOf(@"\") > -1)
                                                {
                                                    isOK = false;
                                                    log.AppendLine("remote_file 參數值不正確 (不可包含路徑)");
                                                }
                                            }
                                            break;
                                            #endregion

                                        case "local_file":
                                            #region local_file (本地端檔案)
                                            if (!String.IsNullOrEmpty(value))
                                            {
                                                localFile = value;
                                                if (localFile.IndexOf(@"/") > -1 || localFile.IndexOf(@"\") > -1)
                                                {
                                                    isOK = false;
                                                    log.AppendLine("local_file 參數值不正確 (不可包含路徑)");
                                                }
                                            }
                                            break;
                                            #endregion

                                        default:
                                            isOK = false;
                                            log.AppendFormat("不支援 {0} 命令參數", arg).AppendLine();
                                            break;
                                    }
                                }
                                else
                                {
                                    isOK = false;
                                    log.AppendFormat("不支援 {0} 命令參數", arg).AppendLine();
                                }
                                if (!isOK)
                                {
                                    break;
                                }
                            }
                            #endregion

                            #region 檢查必要參數
                            if (isOK)
                            {
                                List<string> lost = new List<string>();

                                if (String.IsNullOrEmpty(host))
                                {
                                    lost.Add("host");
                                }

                                if (String.IsNullOrEmpty(uid))
                                {
                                    lost.Add("uid");
                                }

                                if (String.IsNullOrEmpty(pword))
                                {
                                    lost.Add("pwd");
                                }

                                if (String.IsNullOrEmpty(remotePath))
                                {
                                    lost.Add("remote_path");
                                }
                                else if (!remotePath.EndsWith(@"/"))
                                {
                                    remotePath += @"/";
                                }

                                if (String.IsNullOrEmpty(localFile))
                                {
                                    lost.Add("local_file");
                                }
                                else if (String.IsNullOrEmpty(remoteFile))
                                {
                                    remoteFile = localFile;
                                }

                                if (protocol == "FTP" && useSSL)
                                {
                                    protocol = "FTPS";
                                }
                                else if (protocol == "FTPS")
                                {
                                    useSSL = true;
                                }

                                if (lost.Count > 0)
                                {
                                    isOK = false;
                                    log.AppendFormat("缺少 {0} 參數", String.Join(", ", lost)).AppendLine();
                                }
                            }
                            #endregion

                            if (isOK)
                            {
                                if (protocol == "FTP" || protocol == "FTPS")
                                {
                                    #region [MDY:20211115] M202111_01 使用FTP/FTPS時預設使用主動模式 (2021擴充案先做)
                                    bool usePassive = false;    //是否使用被動模式 (false = 主動模式)
                                    hostConfig = HostConfig.CreateByFTPx(host, uid, pword, port, useSSL, usePassive, encoding: encoding, workingDirectory: remotePath);
                                    #endregion
                                }
                                else if (protocol == "SFTP")
                                {
                                    hostConfig = HostConfig.CreateBySFTP(host, uid, pword, port, encoding: encoding, workingDirectory: remotePath);
                                }
                                else
                                {
                                    isOK = false;
                                    log.AppendLine("protocol 參數值不正確");
                                }
                            }
                        }
                        else
                        {
                            isOK = false;
                            log.AppendLine("未指定命令參數");
                        }
                    }
                    #endregion

                    #region 上傳檔案
                    if (isOK)
                    {
                        string localFileFullName = Path.Combine(this.LocalPath, localFile);
                        if (File.Exists(localFileFullName))
                        {
                            string errmsg = _FTPHelper.UploadFile(hostConfig, localFileFullName, remoteFile);
                            if (String.IsNullOrEmpty(errmsg))
                            {
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 上傳檔案成功 (本地：{1}; 遠端：{2})", DateTime.Now, localFileFullName, remoteFile).AppendLine();

                                #region 備份檔案
                                if (!String.IsNullOrEmpty(this.BakPath))
                                {
                                    string bakFileFullName = null;
                                    try
                                    {
                                        bakFileFullName = Path.Combine(this.BakPath, localFile);
                                        if (!Directory.Exists(this.BakPath))
                                        {
                                            Directory.CreateDirectory(this.BakPath);
                                        }
                                        File.Move(localFileFullName, bakFileFullName);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 備分本地端檔案至 {1} 失敗，錯誤訊息：{2}", DateTime.Now, bakFileFullName, ex.Message).AppendLine();
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                isOK = false;
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 上傳檔案失敗 (本地：{1}; 遠端：{2})，錯誤訊息：{3}", DateTime.Now, localFileFullName, remoteFile, errmsg).AppendLine();
                            }
                        }
                        else
                        {
                            isOK = false;
                            log.AppendFormat("指定的本地端檔案不存在 ({0})", localFileFullName).AppendLine();
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    isOK = false;
                    log.AppendFormat("發生例外，錯誤訊息：{0}", ex.Message).AppendLine();
                }

                result = log.ToString();
                return isOK;
            }
            #endregion
            #endregion
        }
        #endregion

        #region [MDY:2018xxxx] 取消寄信，增加作業代碼，並將結果回寫該作業日誌
        #region [OLD]
        //#region 寄信相關
        ///// <summary>
        ///// 取得 CC_1 ~ CC_5 參數設定
        ///// </summary>
        ///// <param name="ccMails"></param>
        ///// <returns></returns>
        //private static string GetCCMails(out List<MailAddress> ccMails)
        //{
        //    ccMails = new List<MailAddress>(5);

        //    ConfigManager config = ConfigManager.Current;
        //    for (int no = 1; no <= 5; no++)
        //    {
        //        string configName = String.Format("CC{0}", no);
        //        string address = config.GetProjectConfigValue("SendMail", configName, StringComparison.CurrentCultureIgnoreCase);
        //        if (!String.IsNullOrWhiteSpace(address))
        //        {
        //            try
        //            {
        //                MailAddress ccMail = new MailAddress(address);
        //                ccMails.Add(ccMail);
        //            }
        //            catch (Exception)
        //            {
        //                return String.Format("{0} 的設定值不是合法的 Email ({1})", configName, address);
        //            }
        //        }
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 發送執行結果通知信
        ///// </summary>
        ///// <param name="ccMails"></param>
        ///// <param name="date"></param>
        ///// <param name="kindName"></param>
        ///// <param name="exitMsg"></param>
        ///// <param name="result"></param>
        ///// <returns></returns>
        //private static string SendResultMail(List<MailAddress> ccMails, DateTime date, string appName, string kindName, string exitMsg, string result)
        //{
        //    if (ccMails == null || ccMails.Count == 0)
        //    {
        //        return "未設定任何收信人 Email";
        //    }

        //    StringBuilder errmsg = new StringBuilder();
        //    try
        //    {
        //        string subject = String.Format("{0:yyyy/MM/dd} 學雜費 {1} ({2}) 執行結果通知", date, appName, kindName);

        //        StringBuilder sb = new StringBuilder();
        //        sb.AppendFormat("執行結果：{0}", exitMsg).AppendLine("<br/>");
        //        sb.AppendLine("執行結果：<br/>");
        //        sb.AppendLine(result.Replace("\r\n", "<br/>")).AppendLine("<br/>");
        //        string content = sb.ToString();

        //        BSNSHelper helper = new BSNSHelper();
        //        foreach (MailAddress ccMail in ccMails)
        //        {
        //            string displayName = String.IsNullOrEmpty(ccMail.DisplayName) ? ccMail.User : ccMail.DisplayName;
        //            string errmsg2 = helper.SendMail(subject, ccMail.Address, displayName, content, null);
        //            if (!String.IsNullOrEmpty(errmsg2))
        //            {
        //                errmsg.Append(errmsg2).Append(";");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errmsg.AppendLine(ex.Message);
        //    }
        //    return errmsg.ToString();
        //}
        //#endregion

        ///// <summary>
        ///// 參數：kind=類別代碼 work_path=工作路徑 list_file=清單檔案 bak_path=備份路徑
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
        //    StringBuilder result = new StringBuilder(); //處理結果，用來記錄通知信的內容
        //    int exitCode = 0;
        //    string exitMsg = null;
        //    string kindName = null;     //類別名稱
        //    DateTime startDate = DateTime.Now;
        //    #endregion

        //    string cmdArg = (args == null || args.Length == 0) ? String.Empty : String.Join(" ", args); //命令參數字串
        //    string kind = null;         //類別代碼
        //    string workPath = null;     //工作路徑
        //    string listFile = null;     //清單檔案
        //    string bakPath = null;      //備份路徑

        //    try
        //    {
        //        result.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始執行 {1}，命令參數：{2}", DateTime.Now, appName, cmdArg).AppendLine();
        //        fileLog.WriteLog(String.Format("開始執行 {0}，命令參數：{1}", appName, cmdArg));

        //        #region 處理參數
        //        if (exitCode == 0)
        //        {
        //            string errmsg = null;

        //            //Debug
        //            fileLog.WriteDebugLog("開始處理參數");

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
        //                            case "kind":
        //                                #region 類別代碼
        //                                if (!String.IsNullOrEmpty(value))
        //                                {
        //                                    kind = value;
        //                                    switch (kind)
        //                                    {
        //                                        case "CTCB_DATA":
        //                                            kindName = "中國信託學校檔和學生檔";
        //                                            break;
        //                                        case "CANCELED_DATA":
        //                                            kindName = "學校銷帳檔";
        //                                            break;
        //                                        //default:
        //                                        //    kindName = kind;
        //                                        //    break;
        //                                    }
        //                                }
        //                                break;
        //                                #endregion
        //                            case "work_path":
        //                                #region 工作路徑
        //                                if (!String.IsNullOrEmpty(value))
        //                                {
        //                                    workPath = value;
        //                                    try
        //                                    {
        //                                        if (!Directory.Exists(workPath))
        //                                        {
        //                                            errmsg = "work_path 指定路徑不存在";
        //                                        }
        //                                    }
        //                                    catch (Exception)
        //                                    {
        //                                        errmsg = "work_path 參數不正確";
        //                                    }
        //                                }
        //                                break;
        //                                #endregion
        //                            case "list_file":
        //                                #region 清單檔案
        //                                if (!String.IsNullOrEmpty(value))
        //                                {
        //                                    listFile = value;
        //                                }
        //                                break;
        //                                #endregion
        //                            case "bak_path":
        //                                #region 備份路徑
        //                                if (!String.IsNullOrEmpty(value))
        //                                {
        //                                    bakPath = value;
        //                                    try
        //                                    {
        //                                        if (!Directory.Exists(bakPath))
        //                                        {
        //                                            Directory.CreateDirectory(bakPath);
        //                                        }
        //                                    }
        //                                    catch (Exception)
        //                                    {
        //                                        errmsg = "bak_path 參數不正確或建立該資料夾失敗";
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

        //                if (String.IsNullOrEmpty(kind))
        //                {
        //                    lost.Add("kind");
        //                }

        //                if (String.IsNullOrEmpty(workPath))
        //                {
        //                    lost.Add("work_path");
        //                }

        //                if (String.IsNullOrEmpty(listFile))
        //                {
        //                    lost.Add("list_file");
        //                }

        //                if (String.IsNullOrEmpty(bakPath))
        //                {
        //                    lost.Add("bak_path");
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

        //                result.AppendLine(exitMsg);
        //                fileLog.WriteLog(exitMsg, false);
        //            }
        //        }
        //        #endregion

        //        #region 讀取清單檔案內容
        //        string[] lines = null;
        //        if (exitCode == 0)
        //        {
        //            //Debug
        //            fileLog.WriteDebugLog("開始讀取清單檔案內容");

        //            string listFileFullName = null;
        //            try
        //            {
        //                listFileFullName = Path.Combine(workPath, listFile);
        //                if (File.Exists(listFileFullName))
        //                {
        //                    lines = File.ReadAllLines(listFileFullName, Encoding.Default);
        //                    if (lines == null || lines.Length == 0)
        //                    {
        //                        exitCode = -1;
        //                        exitMsg = String.Format("清單檔案無內容 ({0})", listFileFullName);
        //                        result.AppendLine(exitMsg);
        //                        fileLog.WriteLog(exitMsg);
        //                    }
        //                }
        //                else
        //                {
        //                    exitCode = -1;
        //                    exitMsg = String.Format("清單檔案不存在 ({0})", listFileFullName);
        //                    result.AppendLine(exitMsg);
        //                    fileLog.WriteLog(exitMsg);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                exitCode = -1;
        //                exitMsg = String.Format("讀取清單檔案失敗 ({0})，錯誤訊息：{1}", listFileFullName, ex.Message);
        //                result.AppendLine(exitMsg);
        //                fileLog.WriteLog(exitMsg, false);
        //            }
        //        }
        //        #endregion

        //        #region 依據清單內容上傳檔案
        //        if (exitCode == 0)
        //        {
        //            //Debug
        //            fileLog.WriteDebugLog("開始依據清單檔案內容上傳檔案");

        //            string upResult = null;
        //            UploadHelper helper = new UploadHelper(workPath, bakPath);
        //            for(int idx = 0; idx < lines.Length; idx++)
        //            {
        //                string msg = null;
        //                string argTxt = lines[idx].Trim();
        //                if (helper.Upload(argTxt, out upResult))
        //                {
        //                    msg = String.Format("上傳第 {0} 個檔案成功，處理日誌：", idx);
        //                }
        //                else
        //                {
        //                    exitCode = -2;  //任一個失敗就算錯，用來判斷是否要送信
        //                    msg = String.Format("上傳第 {0} 個檔案失敗，處理日誌：", idx);
        //                }
        //                result.AppendLine(msg);
        //                fileLog.WriteLog(msg);
        //                result.AppendLine(upResult);
        //                fileLog.WriteLog(upResult, false);
        //            }
        //            if (exitCode != 0)
        //            {
        //                exitMsg = "部分檔案上傳失敗，請參考處理日誌";
        //            }
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        exitCode = -9;
        //        exitMsg = String.Format("{0} 執行發生例外，錯誤訊息：{1}；", appName, ex.Message);
        //        result.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
        //        fileLog.WriteLog(exitMsg);
        //    }
        //    finally
        //    {
        //        #region 將清單檔案移至備份路徑，如果工作路徑為空的就刪除
        //        if (!String.IsNullOrEmpty(workPath) && !String.IsNullOrEmpty(listFile) && !String.IsNullOrEmpty(bakPath))
        //        {
        //            try
        //            {
        //                #region 將清單檔案移至備份路徑
        //                {
        //                    string src = Path.Combine(workPath, listFile);
        //                    string dest = Path.Combine(bakPath, listFile);
        //                    if (File.Exists(src))
        //                    {
        //                        File.Move(src, dest);
        //                    }
        //                }
        //                #endregion

        //                #region 如果工作路徑為空的就刪除
        //                {
        //                    string[] files = Directory.GetFiles(workPath);
        //                    if (files == null || files.Length == 0)
        //                    {
        //                        Directory.Delete(workPath);
        //                    }
        //                }
        //                #endregion
        //            }
        //            catch (Exception ex)
        //            {
        //                result.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 備份清單檔案或刪除空的工作路徑失敗，錯誤訊息：{1}", DateTime.Now, ex.Message).AppendLine();
        //                fileLog.WriteLog("備份清單檔案或刪除空的工作路徑失敗，錯誤訊息：" + ex.Message);
        //            }
        //        }
        //        #endregion

        //        #region 將 result 內容存成檔案方便看
        //        {
        //            try
        //            {
        //                string resultFile = null;
        //                if (!String.IsNullOrEmpty(bakPath))
        //                {
        //                    resultFile = Path.Combine(bakPath, "result.log");
        //                }
        //                else if (!String.IsNullOrEmpty(workPath))
        //                {
        //                    resultFile = Path.Combine(bakPath, "result.log");
        //                }
        //                if (!String.IsNullOrEmpty(resultFile))
        //                {
        //                    File.WriteAllText(resultFile, result.ToString());
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                fileLog.WriteLog("處理結果檔儲存失敗，錯誤訊息：" + ex.Message);
        //            }
        //        }
        //        #endregion

        //        #region 如果失敗發送通知信
        //        if (exitCode != 0)
        //        {
        //            string errmsg = null;

        //            #region 讀取收件人Email設定
        //            List<MailAddress> ccMails = null;
        //            errmsg = GetCCMails(out ccMails);
        //            if (String.IsNullOrEmpty(errmsg) && (ccMails == null || ccMails.Count == 0))
        //            {
        //                errmsg = "未設定任何收件人Email";
        //            }
        //            if (!String.IsNullOrEmpty(errmsg))
        //            {
        //                fileLog.WriteLog(String.Format("讀取 Config 中收件人Email設定失敗，錯誤訊息：{0}", errmsg));
        //            }
        //            #endregion

        //            #region 發送通知信
        //            if (String.IsNullOrEmpty(errmsg))
        //            {
        //                errmsg = SendResultMail(ccMails, startDate, appName, kindName, exitMsg, result.ToString());
        //                if (!String.IsNullOrEmpty(errmsg))
        //                {
        //                    fileLog.WriteLog(String.Format("發送通知信失敗，錯誤訊息：{0}", errmsg));
        //                }
        //            }
        //            #endregion
        //        }
        //        #endregion
        //    }

        //    System.Environment.Exit(exitCode);
        //}
        #endregion

        /// <summary>
        /// 參數：jobno=作業代碼 jobtypeid=作業類別代碼 workpath=工作路徑 configfile=發送指示檔檔名 bakpath=備份路徑
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
            StringBuilder result = new StringBuilder(); //處理結果，用來記錄作業日誌內容
            int exitCode = 0;
            string exitMsg = null;
            DateTime startDate = DateTime.Now;
            #endregion

            string cmdArg = (args == null || args.Length == 0) ? String.Empty : String.Join(" ", args); //命令參數字串
            int jobNo = 0;              //作業代碼
            string jobTypeId = null;    //作業類別代碼
            string workPath = null;     //工作路徑
            string configFile = null;   //發送指示檔檔名
            string bakPath = null;      //備份路徑
            string configFileFullName = null;  //發送指示檔完整路徑檔名

            try
            {
                fileLog.WriteLog(String.Format("開始執行 {0}，命令參數：{1}", appName, cmdArg));

                #region 處理參數
                if (exitCode == 0)
                {
                    string errmsg = null;

                    //Debug
                    fileLog.WriteDebugLog("開始處理參數");

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
                                    case "jobno":
                                        #region 作業代碼
                                        {
                                            Int32 no = 0;
                                            if (!String.IsNullOrEmpty(value) && Int32.TryParse(value, out no) && no > 0)
                                            {
                                                jobNo = no;
                                            }
                                            else
                                            {
                                                errmsg = "作業代碼參數不正確";
                                            }
                                        }
                                        break;
                                        #endregion
                                    case "jobtypeid":
                                        #region 作業類別代碼
                                        {
                                            jobTypeId = value;
                                        }
                                        break;
                                        #endregion
                                    case "workpath":
                                        #region 工作路徑
                                        {
                                            workPath = value;
                                        }
                                        break;
                                        #endregion
                                    case "configfile":
                                        #region 發送指示檔檔名
                                        {
                                            configFile = value;
                                        }
                                        break;
                                        #endregion
                                    case "bakpath":
                                        #region 備份路徑
                                        {
                                            bakPath = value;
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

                    #region 檢查參數值
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        if (jobNo < 1)
                        {
                            errmsg = "缺少作業代碼參數";
                        }
                        else if (String.IsNullOrEmpty(jobTypeId))
                        {
                            errmsg = "缺少作業類別代碼參數";
                        }
                        else if (String.IsNullOrEmpty(workPath))
                        {
                            errmsg = "缺少工作路徑參數";
                        }
                        else if (String.IsNullOrEmpty(configFile))
                        {
                            errmsg = "缺少發送指示檔檔名參數";
                        }
                        else if (String.IsNullOrEmpty(bakPath))
                        {
                            errmsg = "缺少備份路徑參數";
                        }

                        #region 工作路徑
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            try
                            {
                                if (!Directory.Exists(workPath))
                                {
                                    errmsg = "工作路徑不存在";
                                }
                            }
                            catch (Exception)
                            {
                                errmsg = "工作路徑不正確";
                            }
                        }
                        #endregion

                        #region 備份路徑
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            try
                            {
                                if (!Directory.Exists(bakPath))
                                {
                                    Directory.CreateDirectory(bakPath);
                                }
                            }
                            catch (Exception)
                            {
                                errmsg = "備份路徑不正確或建立失敗";
                            }
                        }
                        #endregion
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -1;
                        exitMsg = String.Format("參數錯誤，錯誤訊息：{0}", errmsg);
                        result.AppendLine(exitMsg);
                        fileLog.WriteLog(exitMsg, false);
                    }
                }
                #endregion

                #region 讀取發送指示檔內容
                string[] cmdLines = null;
                if (exitCode == 0)
                {
                    //Debug
                    fileLog.WriteDebugLog("開始讀取發送指示檔內容");

                    #region [MDY:20220530] Checkmarx 調整
                    configFile = configFile.Replace("/", "").Replace("..", "");
                    #endregion

                    configFileFullName = Path.Combine(workPath, configFile);
                    try
                    {
                        if (File.Exists(configFileFullName))
                        {
                            cmdLines = File.ReadAllLines(configFileFullName, Encoding.Default);
                            if (cmdLines == null || cmdLines.Length == 0)
                            {
                                exitCode = -1;
                                exitMsg = String.Format("發送指示檔 ({0}) 無內容", configFileFullName);
                                result.AppendLine(exitMsg);
                                fileLog.WriteLog(exitMsg);
                            }
                        }
                        else
                        {
                            exitCode = -1;
                            exitMsg = String.Format("發送指示檔 ({0}) 不存在", configFileFullName);
                            result.AppendLine(exitMsg);
                            fileLog.WriteLog(exitMsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        exitCode = -1;
                        exitMsg = String.Format("讀取發送指示檔 ({0}) 發生例外，{1}", configFileFullName, ex.Message);
                        result.AppendLine(exitMsg);
                        fileLog.WriteLog(exitMsg, false);
                    }
                }
                #endregion

                #region 依據發送指示內容上傳檔案
                if (exitCode == 0)
                {
                    //Debug
                    fileLog.WriteDebugLog("開始依據發送指示內容上傳檔案");

                    string upResult = null;
                    UploadHelper helper = new UploadHelper(workPath, bakPath);
                    for (int idx = 0; idx < cmdLines.Length; idx++)
                    {
                        string msg = null;
                        string argTxt = cmdLines[idx].Trim();
                        if (helper.Upload(argTxt, out upResult))
                        {
                            msg = String.Format("上傳第 {0} 個檔案成功，處理日誌：", idx);
                        }
                        else
                        {
                            exitCode = -2;  //任一個失敗就算錯，用來判斷是否要送信
                            msg = String.Format("上傳第 {0} 個檔案失敗，處理日誌：", idx);
                        }
                        result.AppendLine(msg);
                        result.AppendLine(upResult);
                        fileLog.WriteLog(msg);
                        fileLog.WriteLog(upResult, false);
                    }

                    if (exitCode != 0)
                    {
                        exitMsg = "部分檔案上傳失敗，請參考處理日誌";
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;
                exitMsg = String.Format("{0} 執行發生例外，錯誤訊息：{1}；", appName, ex.Message);
                result.AppendFormat("執行發生例外，{0}", ex.Message);
                fileLog.WriteLog(exitMsg);
            }
            finally
            {
                #region 將發送指示檔移至備份路徑，如果工作路徑為空的就刪除
                if (!String.IsNullOrEmpty(workPath) && !String.IsNullOrEmpty(bakPath) && !String.IsNullOrEmpty(configFile))
                {
                    try
                    {
                        #region 將發送指示檔移至備份路徑
                        {
                            string src = Path.Combine(workPath, configFile);
                            string dest = Path.Combine(bakPath, configFile);
                            if (File.Exists(src))
                            {
                                File.Move(src, dest);
                            }
                        }
                        #endregion

                        #region 如果工作路徑為空的就刪除
                        {
                            string[] files = Directory.GetFiles(workPath);
                            if (files == null || files.Length == 0)
                            {
                                Directory.Delete(workPath);
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        fileLog.WriteLog("備份發送指示檔或刪除空的工作路徑失敗，錯誤訊息：" + ex.Message);
                    }
                }
                #endregion

                #region 將處理結果回寫該作業日誌
                try
                {
                    JobcubeEntity jobCube = null;
                    Expression where = new Expression(JobcubeEntity.Field.Jno, jobNo)
                         .And(JobcubeEntity.Field.Jtypeid, jobTypeId);
                    XmlResult xmlResult = DataProxy.Current.SelectFirst<JobcubeEntity>(where, null, out jobCube);
                    if (xmlResult.IsSuccess)
                    {
                        if (jobCube == null)
                        {
                            fileLog.WriteLog(String.Format("查無該作業資料 (JobNo = {0}; JobTypeId = {1})", jobNo, jobTypeId));
                        }
                        else
                        {
                            List<KeyValue> fieldValues = new List<KeyValue>(2);
                            if (exitCode != 0)
                            {
                                fieldValues.Add(new KeyValue(JobcubeEntity.Field.Jresultid, JobCubeResultCodeTexts.FAILURE));
                                jobCube.Memo += "\r\n[FTP 檔案上傳作業結果] 部份檔案上傳失敗，請參考處理日誌";
                            }
                            else
                            {
                                jobCube.Memo += "\r\n[FTP 檔案上傳作業結果] 檔案上傳成功";
                            }
                            if (jobCube.Memo.Length > 2000)
                            {
                                jobCube.Memo = jobCube.Memo.Substring(0, 2000);
                            }
                            jobCube.Jlog += "\r\n[FTP 檔案上傳作業結果]" + result.ToString();
                            fieldValues.Add(new KeyValue(JobcubeEntity.Field.Memo, jobCube.Memo));
                            fieldValues.Add(new KeyValue(JobcubeEntity.Field.Jlog, jobCube.Jlog));

                            int count = 0;
                            xmlResult = DataProxy.Current.UpdateFields<JobcubeEntity>(where, fieldValues, out count);
                            if (!xmlResult.IsSuccess)
                            {
                                fileLog.WriteLog(String.Format("更新作業日誌 (JobNo = {0}; JobTypeId = {1}) 失敗，{2}", jobNo, jobTypeId, xmlResult.Message));
                            }
                        }
                    }
                    else
                    {
                        fileLog.WriteLog(String.Format("查詢該作業資料 (JobNo = {0}; JobTypeId = {1}) 失敗，{2}", jobNo, jobTypeId, xmlResult.Message));
                    }
                }
                catch (Exception ex)
                {
                    fileLog.WriteLog(String.Format("回寫該作業日誌 (JobNo = {0}; JobTypeId = {1}) 發生例外，{2}", jobNo, jobTypeId, ex.Message));
                }
                #endregion

                fileLog.WriteLog(String.Format("結束執行 {0} \r\n", appName));
            }

            System.Environment.Exit(exitCode);
        }

        #region DataProxy Class
        private class DataProxy : BaseProxy
        {
            #region Static Property
            private static DataProxy _Current;
            /// <summary>
            /// 取得目前的資料處理代理類別物件，如果目前沒有則傳回一個新的預設參數資料處理代理類別物件
            /// </summary>
            public static DataProxy Current
            {
                get
                {
                    if (_Current == null)
                    {
                        _Current = new DataProxy();
                    }
                    return _Current;
                }
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構資料處理代理類別
            /// </summary>
            private DataProxy()
                : base()
            {
                //ServicePointManager.ServerCertificateValidationCallback = delegate
                //{
                //    return true;
                //};
            }
            #endregion

            #region 查詢資料
            /// <summary>
            /// 查詢首筆資料
            /// </summary>
            /// <typeparam name="T">指定查詢資料的 Entity 型別。</typeparam>
            /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
            /// <param name="where">指定查詢的條件。必要參數，無條件時傳入空的條件。</param>
            /// <param name="orderbys">指定資料的排序方式。</param>
            /// <param name="instance">成功則傳回查詢結果的首筆資料，否則傳回 default(T)。</param>
            /// <returns>傳回處理結果。</returns>
            public XmlResult SelectFirst<T>(Expression where, KeyValueList<OrderByEnum> orderbys
                , out T instance) where T : class, IEntity
            {
                instance = default(T);

                #region 檢查資料處理代理物件
                if (!this.IsReady())
                {
                    return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
                }
                #endregion

                #region 檢查參數
                if (where == null || !where.IsReady())
                {
                    return new XmlResult(false, "缺少或無效的查詢條件參數", ErrorCode.S_INVALID_PARAMETER, null);
                }
                #endregion

                #region 取得服務命令請求者資料
                CommandAsker cmdAsker = this.TryGetCommandAsker();
                if (cmdAsker == null || !cmdAsker.IsReady)
                {
                    return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
                }
                #endregion

                #region 產生服務命令
                SelectFirstCommand command = SelectFirstCommand.Create<T>(cmdAsker, where, orderbys);
                #endregion

                #region 呼叫後端服務命令，並處理回傳結果
                XmlResult xmlResult = this.ExecuteCommand(command);
                if (xmlResult.IsSuccess)
                {
                    object data = null;
                    if (xmlResult.TryGetData(out data))
                    {
                        if (data is T)
                        {
                            instance = (T)data;
                            xmlResult = new XmlResult(true);
                        }
                        else if (Reflector.TryCast<T>(data, out instance))
                        {
                            xmlResult = new XmlResult(true);
                        }
                        else
                        {
                            xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                        }
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                    }
                }
                return xmlResult;
                #endregion
            }
            #endregion

            #region 更新欄位值
            /// <summary>
            /// 整批更新指定欄位值
            /// </summary>
            /// <typeparam name="T">指定更新資料的 Entity 型別。</typeparam>
            /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
            /// <param name="where">指定更新的條件。必要參數，且不可以是空的條件。</param>
            /// <param name="fieldValues">指定更新的資料 (欄位名稱與值的集合)。必要參數，可傳入 KeyValue[] 或 KeyValueList 型別。</param>
            /// <param name="count">成功則傳回受影響的資料筆數，否則傳回 0。</param>
            /// <returns>傳回處理結果。</returns>
            public XmlResult UpdateFields<T>(Expression where, ICollection<KeyValue> fieldValues
                , out int count) where T : class, IEntity
            {
                count = 0;

                #region 檢查資料處理代理物件
                if (!this.IsReady())
                {
                    return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
                }
                #endregion

                #region 檢查參數
                if (where == null || where.IsEmpty() || !where.IsReady())
                {
                    return new XmlResult(false, "缺少或無效的更新條件參數", ErrorCode.S_INVALID_PARAMETER);
                }
                if (fieldValues == null || fieldValues.Count == 0)
                {
                    return new XmlResult(false, "缺少或無效的更新欄位名稱與值參數", ErrorCode.S_INVALID_PARAMETER);
                }
                #endregion

                #region 取得服務命令請求者資料
                CommandAsker cmdAsker = this.TryGetCommandAsker();
                if (cmdAsker == null || !cmdAsker.IsReady)
                {
                    return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
                }
                #endregion

                #region 產生服務命令
                UpdateFieldsCommand command = UpdateFieldsCommand.Create<T>(cmdAsker, where, fieldValues);
                #endregion

                #region 呼叫後端服務命令，並處理回傳結果
                XmlResult xmlResult = this.ExecuteCommand(command);
                if (xmlResult.IsSuccess)
                {
                    object data = null;
                    if (xmlResult.TryGetData(out data))
                    {
                        if (data is int)
                        {
                            count = (int)data;
                            xmlResult = new XmlResult(true);
                        }
                        else
                        {
                            xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                        }
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                    }
                }
                return xmlResult;
                #endregion
            }
            #endregion

            #region Private Method
            /// <summary>
            /// 嘗試由指定頁面取得服務命令請求者資料
            /// </summary>
            /// <param name="page">指定頁面</param>
            /// <returns>成功則傳回服務命令請求者資料，否則傳回 null。</returns>
            private CommandAsker TryGetCommandAsker()
            {
                LogonUser logonUser = LogonUser.GenAnonymous();
                return CommandAsker.Create(logonUser, "FtpUpload", "FTP 檔案上傳");
            }
            #endregion
        }
        #endregion

        #endregion
    }
}
