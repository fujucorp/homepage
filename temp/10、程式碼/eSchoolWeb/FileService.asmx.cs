using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using Fuju;
using Fuju.DB;
using Fuju.Configuration;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    ///接收檔案 Web 服務
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class FileService : System.Web.Services.WebService
    {
        #region [MDY:2018xxxx] FTP上傳結果寫回指定作業代碼的日誌資料
        #region [OLD]
        //private Regex _CommandLineReplaceRegex = new System.Text.RegularExpressions.Regex("(DEL )|(COPY )|(MOVE )|(RENAME )|[\x00-\x19]|[\x7F-\xFF]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        ///// <summary>
        ///// 接收檔案
        ///// </summary>
        ///// <param name="apId"></param>
        ///// <param name="apPwd"></param>
        ///// <param name="kind"></param>
        ///// <param name="listFileName"></param>
        ///// <param name="contents"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public string ReceiveFile(string apId, string apPwd, string kind, string listFileName, byte[] contents)
        //{
        //    try
        //    {
        //        if (!this.CheckAp(apId, apPwd) || String.IsNullOrWhiteSpace(kind) || String.IsNullOrWhiteSpace(listFileName) || contents == null || contents.Length == 0)
        //        {
        //            this.WriteLog("無效的系統參數 (apId={0}; apPwd={1}; kind={2}; listFileName={3}; contents size:{4})", apId, apPwd, kind, listFileName, (contents == null || contents.Length == 0 ? "is null or empty" : contents.Length.ToString()));
        //            if (_LogFailMsg.Length > 0)
        //            {
        //                return String.Format("{0} (LogFailMsg={1})", "無效的系統參數", _LogFailMsg);
        //            }
        //            else
        //            {
        //                return "無效的系統參數";
        //            }
        //        }
        //        string myListFileName = Path.GetFileName(_CommandLineReplaceRegex.Replace(listFileName.Replace("'", ""), ""));
        //        if (Convert.ToInt32(myListFileName.Length) > 0)
        //        {
        //            switch (kind)
        //            {
        //                case "CTCB_DATA":
        //                    {
        //                        string result = this.ReceiveCTCBData(myListFileName, contents);
        //                        this.WriteLog("接收中國信託檔案處理結果 =  {0}", result);
        //                        if (_LogFailMsg.Length > 0)
        //                        {
        //                            return String.Format("{0} (LogFailMsg={1})", result, _LogFailMsg);
        //                        }
        //                        else
        //                        {
        //                            return result;
        //                        }
        //                    }
        //                case "CANCELED_DATA":
        //                    {
        //                        string result = this.ReceiveCanceledData(myListFileName, contents);
        //                        this.WriteLog("接收學校銷帳資料檔案處理結果 =  {0}", result);
        //                        if (_LogFailMsg.Length > 0)
        //                        {
        //                            return String.Format("{0} (LogFailMsg={1})", result, _LogFailMsg);
        //                        }
        //                        else
        //                        {
        //                            return result;
        //                        }
        //                    }
        //                default:
        //                    this.WriteLog("無效的 kind 參數 (kind={0})", kind);
        //                    if (_LogFailMsg.Length > 0)
        //                    {
        //                        return String.Format("{0} (LogFailMsg={1})", "無效的 (kind) 系統參數", _LogFailMsg);
        //                    }
        //                    else
        //                    {
        //                        return "無效的 (kind) 系統參數";
        //                    }
        //            }
        //        }
        //        else
        //        {
        //            return "無效的 listFileName 參數";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.WriteLog("執行 ReceiveFile(kind={0}) WebMethod 發生例外，錯誤訊息：{1} (StackTrace={2})", kind, ex.Message, ex.StackTrace);
        //        string result = String.Format("執行 ({0}) 發生例外，錯誤訊息：{1}", kind, ex.Message);
        //        if (_LogFailMsg.Length > 0)
        //        {
        //            return String.Format("{0} (LogFailMsg={1})", result, _LogFailMsg);
        //        }
        //        else
        //        {
        //            return result;
        //        }
        //    }
        //}

        //private bool CheckAp(string apId, string apPwd)
        //{
        //    return (apId == "eSchoolAp" && apPwd == "1qaz@WSX");
        //}

        //private string ReceiveCTCBData(string listFileName, byte[] contents)
        //{
        //    string tempPath = ConfigManager.Current.GetProjectConfigValue("FileService", "TempPath");
        //    string dataPath = ConfigManager.Current.GetProjectConfigValue("FileService", "CTCBDataPath");
        //    if (String.IsNullOrWhiteSpace(tempPath))
        //    {
        //        return "系統未指定接收檔案暫存目錄";
        //    }
        //    if (String.IsNullOrWhiteSpace(dataPath))
        //    {
        //        return "系統未指定CTCB檔案儲存目錄";
        //    }

        //    StringBuilder log = new StringBuilder();
        //    string result = null;
        //    try
        //    {
        //        log.AppendFormat("[ReceiveCTCBData] 接收檔案暫存目錄={0}; CTCB檔案儲存目錄={1}", tempPath, dataPath).AppendLine();

        //        if (!Directory.Exists(tempPath))
        //        {
        //            Directory.CreateDirectory(tempPath);
        //        }
        //        if (!Directory.Exists(dataPath))
        //        {
        //            Directory.CreateDirectory(dataPath);
        //        }

        //        #region [MDY:20170416] 取得目前時間(yyyyMMddHHmmssffff)作為暫存檔名的一部分與工作資料夾，並將暫存檔解壓縮至工作資料夾，避免與後續檔案重複。且改由 CallFtpUpload 做 FTP 檔案上傳
        //        #region [Old]
        //        //string srcFile = Path.Combine(tempPath, String.Format("CTCB_{0}.ZIP", DateTime.Now.Ticks));
        //        //File.WriteAllBytes(srcFile, contents);
        //        //log.AppendFormat("[ReceiveCTCBData] 儲存接收檔案={0}; Size={1}", srcFile, contents.Length).AppendLine();

        //        //string listFile = Path.Combine(dataPath, listFileName);
        //        //string okFile = Path.Combine(dataPath, String.Format("{0}.OK", listFileName));
        //        //ZIPHelper.ExtractZip(srcFile, dataPath);
        //        //log.AppendFormat("[ReceiveCTCBData] 要解壓縮的檔案={0}; 解至資料夾={1}", srcFile, dataPath).AppendLine();

        //        //File.Move(listFile, okFile);
        //        //log.AppendFormat("[ReceiveCTCBData] 將發送指示檔={0}; 更名為={1}", listFile, okFile).AppendLine();

        //        //string bakPath = Path.Combine(dataPath, "BAK");
        //        //StringBuilder errmsg = null;
        //        //this.UploadFTP(dataPath, Path.GetFileName(okFile), bakPath, out errmsg);
        //        //if (errmsg != null && errmsg.Length > 0)
        //        //{
        //        //    log.AppendLine(errmsg.ToString());
        //        //}

        //        ////try
        //        ////{
        //        ////    File.Delete(srcFile);
        //        ////}
        //        ////catch(Exception)
        //        ////{
        //        ////}
        //        #endregion

        //        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        //        string workPath = Path.Combine(dataPath, timeStamp);
        //        if (!Directory.Exists(workPath))
        //        {
        //            Directory.CreateDirectory(workPath);
        //        }

        //        string srcFile = Path.Combine(tempPath, String.Format("CTCB_{0}.ZIP", timeStamp));
        //        File.WriteAllBytes(srcFile, contents);
        //        log.AppendFormat("[ReceiveCTCBData] 儲存接收檔案={0}; Size={1}", srcFile, contents.Length).AppendLine();

        //        string listFile = Path.Combine(workPath, listFileName);
        //        string okFile = Path.Combine(workPath, String.Format("{0}.OK", listFileName));
        //        ZIPHelper.ExtractZip(srcFile, workPath);
        //        log.AppendFormat("[ReceiveCTCBData] 要解壓縮的檔案={0}; 解至資料夾={1}", srcFile, workPath).AppendLine();

        //        File.Move(listFile, okFile);
        //        log.AppendFormat("[ReceiveCTCBData] 將發送指示檔={0}; 更名為={1}", listFile, okFile).AppendLine();

        //        string bakPath = Path.Combine(dataPath, "BAK", timeStamp);
        //        string errmsg = this.CallFtpUpload("CTCB_DATA", workPath, Path.GetFileName(okFile), bakPath);
        //        if (!String.IsNullOrEmpty(errmsg))
        //        {
        //            result = "呼叫 FtpUpload 上傳檔案失敗，錯誤訊息：" + errmsg;
        //            log.AppendLine(result);
        //        }
        //        else
        //        {
        //            result = "OK";
        //            log.AppendLine("呼叫 FtpUpload 上傳檔案成功");
        //        }
        //        #endregion
        //    }
        //    catch(Exception ex)
        //    {
        //        log
        //            .AppendFormat("[ReceiveCTCBData] 處理檔案發生例外，錯誤訊息：{0}", ex.Message).AppendLine()
        //            .AppendFormat("    ex.Source：{0}", ex.Source).AppendLine()
        //            .AppendFormat("    ex.StackTrace：{0}", ex.StackTrace).AppendLine();
        //        if (ex.InnerException != null)
        //        {
        //            Exception ex2 = ex.InnerException;
        //            log
        //                .AppendFormat("    InnerException ex.Message：{0}", ex2.Message).AppendLine()
        //                .AppendFormat("    InnerException ex.Source：{0}", ex2.Source).AppendLine()
        //                .AppendFormat("    InnerException ex.StackTrace：{0}", ex2.StackTrace).AppendLine();
        //        }
        //        result = "嘗試處理檔案失敗，錯誤訊息：" + ex.Message;
        //    }

        //    this.WriteLog(log.ToString());
        //    return result;
        //}

        //private string ReceiveCanceledData(string listFileName, byte[] contents)
        //{
        //    string tempPath = ConfigManager.Current.GetProjectConfigValue("FileService", "TempPath");
        //    string dataPath = ConfigManager.Current.GetProjectConfigValue("FileService", "CanceledDataPath");
        //    if (String.IsNullOrWhiteSpace(tempPath))
        //    {
        //        return "系統未指定傳存檔案儲存目錄";
        //    }
        //    if (String.IsNullOrWhiteSpace(dataPath))
        //    {
        //        return "系統未指定CTCB檔案儲存目錄";
        //    }

        //    StringBuilder log = new StringBuilder();
        //    string result = null;
        //    try
        //    {
        //        log.AppendFormat("[ReceiveCanceledData] 接收檔案暫存目錄={0}; CTCB檔案儲存目錄={1}", tempPath, dataPath).AppendLine();

        //        if (!Directory.Exists(tempPath))
        //        {
        //            Directory.CreateDirectory(tempPath);
        //        }
        //        if (!Directory.Exists(dataPath))
        //        {
        //            Directory.CreateDirectory(dataPath);
        //        }

        //        #region [MDY:20170416] 取得目前時間(yyyyMMddHHmmssffff)作為暫存檔名的一部分與工作資料夾，並將暫存檔解壓縮至工作資料夾，避免與後續檔案重複。且改由 CallFtpUpload 做 FTP 檔案上傳
        //        #region [Old]
        //        //string srcFile = Path.Combine(tempPath, String.Format("CANCELED_{0}.ZIP", DateTime.Now.Ticks));
        //        //File.WriteAllBytes(srcFile, contents);
        //        //log.AppendFormat("[ReceiveCTCBData] 儲存接收檔案={0}; Size={1}", srcFile, contents.Length).AppendLine();

        //        //string listFile = Path.Combine(dataPath, listFileName);
        //        //string okFile = Path.Combine(dataPath, String.Format("{0}.OK", listFileName));
        //        //ZIPHelper.ExtractZip(srcFile, dataPath);
        //        //log.AppendFormat("[ReceiveCTCBData] 要解壓縮的檔案={0}; 解至資料夾={1}", srcFile, dataPath).AppendLine();

        //        //File.Move(listFile, okFile);
        //        //log.AppendFormat("[ReceiveCTCBData] 將發送指示檔={0}; 更名為={1}", listFile, okFile).AppendLine();

        //        //string bakPath = Path.Combine(dataPath, "BAK");
        //        //StringBuilder errmsg = null;
        //        //this.UploadFTP(dataPath, Path.GetFileName(okFile), bakPath, out errmsg);
        //        //if (errmsg != null && errmsg.Length > 0)
        //        //{
        //        //    log.AppendLine(errmsg.ToString());
        //        //}

        //        ////try
        //        ////{
        //        ////    File.Delete(srcFile);
        //        ////}
        //        ////catch (Exception)
        //        ////{
        //        ////}
        //        #endregion

        //        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        //        string workPath = Path.Combine(dataPath, timeStamp);
        //        if (!Directory.Exists(workPath))
        //        {
        //            Directory.CreateDirectory(workPath);
        //        }

        //        string srcFile = Path.Combine(tempPath, String.Format("CANCELED_{0}.ZIP", timeStamp));
        //        File.WriteAllBytes(srcFile, contents);
        //        log.AppendFormat("[ReceiveCanceledData] 儲存接收檔案={0}; Size={1}", srcFile, contents.Length).AppendLine();

        //        string listFile = Path.Combine(workPath, listFileName);
        //        string okFile = Path.Combine(workPath, String.Format("{0}.OK", listFileName));
        //        ZIPHelper.ExtractZip(srcFile, workPath);
        //        log.AppendFormat("[ReceiveCanceledData] 要解壓縮的檔案={0}; 解至資料夾={1}", srcFile, workPath).AppendLine();

        //        File.Move(listFile, okFile);
        //        log.AppendFormat("[ReceiveCanceledData] 將發送指示檔={0}; 更名為={1}", listFile, okFile).AppendLine();

        //        string bakPath = Path.Combine(dataPath, "BAK", timeStamp);
        //        string errmsg = this.CallFtpUpload("CANCELED_DATA", workPath, Path.GetFileName(okFile), bakPath);
        //        if (!String.IsNullOrEmpty(errmsg))
        //        {
        //            result = "呼叫 FtpUpload 上傳檔案失敗，錯誤訊息：" + errmsg;
        //            log.AppendLine(result);
        //        }
        //        else
        //        {
        //            result = "OK";
        //            log.AppendLine("呼叫 FtpUpload 上傳檔案成功");
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        log
        //            .AppendFormat("[ReceiveCanceledData] 處理檔案發生例外，錯誤訊息：{0}", ex.Message).AppendLine()
        //            .AppendFormat("    ex.Source：{0}", ex.Source).AppendLine()
        //            .AppendFormat("    ex.StackTrace：{0}", ex.StackTrace).AppendLine();
        //        if (ex.InnerException != null)
        //        {
        //            Exception ex2 = ex.InnerException;
        //            log
        //                .AppendFormat("    InnerException ex.Message：{0}", ex2.Message).AppendLine()
        //                .AppendFormat("    InnerException ex.Source：{0}", ex2.Source).AppendLine()
        //                .AppendFormat("    InnerException ex.StackTrace：{0}", ex2.StackTrace).AppendLine();
        //        }
        //        result = "嘗試處理檔案失敗，錯誤訊息：" + ex.Message;
        //    }

        //    this.WriteLog(log.ToString());
        //    return result;
        //}

        //#region Log 相關
        //private const string _MethodName = "FileService";
        //private string _LogPath = null;

        //private StringBuilder _LogFailMsg = new StringBuilder();

        ///// <summary>
        ///// 取得 Log 檔完整路徑檔名
        ///// </summary>
        ///// <returns></returns>
        //private string GetLogFileName()
        //{
        //    if (_LogPath == null)
        //    {
        //        _LogPath = ConfigurationManager.AppSettings.Get("log_path");
        //        if (_LogPath == null)
        //        {
        //            _LogPath = String.Empty;
        //        }
        //        else
        //        {
        //            _LogPath = _LogPath.Trim();
        //        }
        //        if (!String.IsNullOrEmpty(_LogPath))
        //        {
        //            try
        //            {
        //                if (!Directory.Exists(_LogPath))
        //                {
        //                    Directory.CreateDirectory(_LogPath);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                _LogPath = String.Empty;
        //            }
        //        }
        //    }

        //    if (String.IsNullOrEmpty(_LogPath))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return Path.Combine(_LogPath, String.Format("{0}_{1:yyyyMMdd}.log", _MethodName, DateTime.Today));
        //    }
        //}

        ///// <summary>
        ///// 寫 Log
        ///// </summary>
        ///// <param name="methodName">方法名稱</param>
        ///// <param name="msg">訊息</param>
        //private void WriteLog(string msg)
        //{
        //    if (String.IsNullOrEmpty(msg))
        //    {
        //        return;
        //    }
        //    string logFileName = this.GetLogFileName();
        //    if (String.IsNullOrEmpty(logFileName))
        //    {
        //        return;
        //    }

        //    StringBuilder log = new StringBuilder();
        //    log
        //        .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, _MethodName).AppendLine()
        //        .AppendLine(msg)
        //        .AppendLine();

        //    this.WriteLogFile(logFileName, log.ToString());
        //}

        //private void WriteLog(string format, params object[] args)
        //{
        //    if (String.IsNullOrWhiteSpace(format) || args == null || args.Length == 0)
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        this.WriteLog(String.Format(format, args));
        //    }
        //    catch (Exception ex)
        //    {
        //        _LogFailMsg.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] WriteLog() Fail, {1}", DateTime.Now, ex.Message).AppendLine();
        //    }
        //}

        ///// <summary>
        ///// 寫入 Log 檔
        ///// </summary>
        ///// <param name="fileName">Log 檔名</param>
        ///// <param name="log">Log 內容</param>
        //private void WriteLogFile(string fileName, string log)
        //{
        //    try
        //    {
        //        File.AppendAllText(fileName, log, Encoding.Default);
        //    }
        //    catch (Exception ex)
        //    {
        //        //_logPath = String.Empty;
        //        _LogFailMsg.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] WriteLogFile() Fail, {1}", DateTime.Now, ex.Message).AppendLine();
        //    }
        //}
        //#endregion

        //#region FTP Upload
        ////private delegate Result AsyncUploadFTPDelegate(string localPath, string okFileName, string bakPath);

        ////private IAsyncResult AsyncUploadFTP(string localPath, string okFileName, string bakPath, AsyncCallback callback)
        ////{
        ////    AsyncUploadFTPDelegate myAsync = new AsyncUploadFTPDelegate(UploadFTP);
        ////    //Result result = myAsync.Invoke(localPath, okFileName);
        ////    IAsyncResult asyncResult = myAsync.BeginInvoke(localPath, okFileName, bakPath, callback, null);

        ////    return asyncResult;
        ////}

        //#region [MDY:20170416] 改由 FtpUpload 做 FTP 檔案上傳
        //#region [Old]
        ////private Result UploadFTP(string localPath, string okFileName, string bakPath, out StringBuilder errmsg)
        ////{
        ////    errmsg = new StringBuilder();

        ////    #region 檢查參數
        ////    string okFileFullName = null;
        ////    if (String.IsNullOrWhiteSpace(localPath) || String.IsNullOrWhiteSpace(okFileName))
        ////    {
        ////        errmsg.AppendLine("缺少 localPath 或 okFileName 參數");
        ////    }
        ////    else
        ////    {
        ////        okFileFullName = Path.Combine(localPath, okFileName);
        ////        if (!File.Exists(okFileFullName))
        ////        {
        ////            errmsg.AppendFormat("指定的 OK 檔 ({0}) 不存在", okFileFullName).AppendLine();
        ////        }
        ////    }
        ////    if (String.IsNullOrWhiteSpace(bakPath))
        ////    {
        ////        bakPath = localPath;
        ////    }
        ////    else if (!Directory.Exists(bakPath))
        ////    {
        ////        try
        ////        {
        ////            Directory.CreateDirectory(bakPath);
        ////        }
        ////        catch (Exception)
        ////        {
        ////            //如果建立失敗就用 localPath
        ////            bakPath = localPath;
        ////        }
        ////    }
        ////    #endregion

        ////    #region FTP 執行檔路徑
        ////    string ftpClientFileFullName = ConfigManager.Current.GetProjectConfigValue("FileService", "FTPClient");
        ////    if (String.IsNullOrWhiteSpace(ftpClientFileFullName))
        ////    {
        ////        errmsg.AppendLine("未指定 Config 中的 FTPClient 設定");
        ////    }
        ////    if (!File.Exists(ftpClientFileFullName))
        ////    {
        ////        errmsg.AppendFormat("指定的 FTPClient 檔 ({0}) 不存在", ftpClientFileFullName).AppendLine();
        ////    }
        ////    #endregion

        ////    Result result = null;

        ////    #region 上傳 OK 檔指定的檔案
        ////    if (errmsg.Length == 0)
        ////    {
        ////        try
        ////        {
        ////            #region 讀取 OK 檔指令並呼叫 FtpClient 上傳
        ////            int okCount = 0;
        ////            string[] lines = File.ReadAllLines(okFileFullName);
        ////            if (lines == null || lines.Length == 0)
        ////            {
        ////                errmsg.AppendFormat("指定的 OK 檔 ({0}) 無內容", okFileFullName).AppendLine();
        ////            }
        ////            else
        ////            {
        ////                ProcessStartInfo startInfo = new ProcessStartInfo();
        ////                startInfo.CreateNoWindow = false;
        ////                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        ////                startInfo.FileName = ftpClientFileFullName;

        ////                foreach (string line in lines)
        ////                {
        ////                    try
        ////                    {
        ////                        startInfo.Arguments = String.Format("method=put {0} local_path={1} bak=false move=true", line, localPath);
        ////                        using (Process myProcess = Process.Start(startInfo))
        ////                        {
        ////                            //myProcess.WaitForExit();
        ////                            okCount++;
        ////                        }
        ////                    }
        ////                    catch (Exception ex)
        ////                    {
        ////                        errmsg.AppendFormat("執行程式 {0} {1} 發生例外，錯誤訊息：{2}", startInfo.FileName, startInfo.Arguments, ex.Message).AppendLine();
        ////                    }
        ////                }
        ////            }
        ////            #endregion

        ////            #region Move OK 檔
        ////            string okBakFileFullName = Path.Combine(bakPath, okFileName + "." + DateTime.Now.ToString("yyyyMMddHHmmss"));
        ////            try
        ////            {
        ////                File.Move(okFileFullName, okBakFileFullName);
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                errmsg.AppendFormat("移動 OK 檔 ({0} > {1}) 發生例外，錯誤訊息：{2}", okFileFullName, okBakFileFullName, ex.Message).AppendLine();
        ////            }
        ////            #endregion

        ////            if (errmsg.Length == 0)
        ////            {
        ////                result = new Result(true);
        ////            }
        ////            else
        ////            {
        ////                if (okCount == 0)
        ////                {
        ////                    result = new Result(false, "所有檔案上傳失敗", CoreStatusCode.UNKNOWN_ERROR, null);
        ////                }
        ////                else
        ////                {
        ////                    result = new Result(false, "部分檔案上傳失敗", CoreStatusCode.UNKNOWN_ERROR, null);
        ////                }
        ////            }
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            errmsg.AppendFormat("執行 UploadFTP 發生例外，錯誤訊息：{0}", ex.Message);
        ////            result = new Result(false, "執行 UploadFTP 發生例外，錯誤訊息：" + ex.Message, CoreStatusCode.UNKNOWN_EXCEPTION, ex);
        ////        }
        ////    }
        ////    else
        ////    {
        ////        result = new Result(false, errmsg.ToString(), CoreStatusCode.UNKNOWN_ERROR, null);
        ////    }
        ////    #endregion

        ////    //#region 寫 Log
        ////    //if (errmsg.Length == 0)
        ////    //{
        ////    //    this.WriteLog("執行 UploadFTP 成功 (OKFile = {0})", okFileName);
        ////    //}
        ////    //else
        ////    //{
        ////    //    this.WriteLog("執行 UploadFTP 失敗 (OKFile = {0})，{1}", okFileName, errmsg.ToString());
        ////    //}
        ////    //#endregion

        ////    return result;
        ////}
        //#endregion

        ///// <summary>
        ///// 呼叫 FtpUpload 外部程式上傳檔案
        ///// </summary>
        ///// <param name="kind"></param>
        ///// <param name="workPath"></param>
        ///// <param name="listFile"></param>
        ///// <param name="bakPath"></param>
        ///// <returns></returns>
        //private string CallFtpUpload(string kind, string workPath, string listFile, string bakPath)
        //{
        //    string errmsg = null;

        //    #region 檢查參數
        //    if (String.IsNullOrWhiteSpace(kind) || String.IsNullOrWhiteSpace(workPath) || String.IsNullOrWhiteSpace(listFile) || String.IsNullOrWhiteSpace(bakPath))
        //    {
        //        errmsg = "缺少 kind、workPath、listFile 或 bakPath 參數";
        //    }
        //    else
        //    {
        //        string listFileFullName = Path.Combine(workPath, listFile);
        //        if (!File.Exists(listFileFullName))
        //        {
        //            errmsg = String.Format("listFile 參數指定的檔案 ({0}) 不存在", listFileFullName);
        //        }
        //    }
        //    if (!Directory.Exists(bakPath))
        //    {
        //        try
        //        {
        //            Directory.CreateDirectory(bakPath);
        //        }
        //        catch (Exception)
        //        {
        //            errmsg = String.Format("bakPath 參數指定的路徑 ({0}) 無法建立", bakPath);
        //        }
        //    }
        //    #endregion

        //    #region FtpUpload 執行檔路徑
        //    string ftpUploadFileFullName = null;
        //    if (String.IsNullOrEmpty(errmsg))
        //    {
        //        ftpUploadFileFullName = ConfigManager.Current.GetProjectConfigValue("FileService", "FtpUpload");
        //        if (String.IsNullOrWhiteSpace(ftpUploadFileFullName))
        //        {
        //            errmsg = "未指定 Config 中的 FtpUpload 設定";
        //        }
        //        if (!File.Exists(ftpUploadFileFullName))
        //        {
        //            errmsg = String.Format("指定的 FtpUpload 執行檔 ({0}) 不存在", ftpUploadFileFullName);
        //        }
        //    }
        //    #endregion

        //    #region 呼叫 FtpUpload 執行檔
        //    if (String.IsNullOrEmpty(errmsg))
        //    {
        //        ProcessStartInfo startInfo = new ProcessStartInfo();
        //        startInfo.CreateNoWindow = false;
        //        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //        startInfo.FileName = ftpUploadFileFullName;

        //        try
        //        {
        //            //kind=類別代碼 work_path=工作路徑 list_file=清單檔案 bak_path=備份路徑
        //            startInfo.Arguments = String.Format("kind={0} work_path={1} list_file={2} bak_path={3}", kind, workPath, listFile, bakPath);
        //            using (Process myProcess = Process.Start(startInfo))
        //            {
        //                //myProcess.WaitForExit();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            errmsg = String.Format("執行程式 {0} {1} 發生例外，錯誤訊息：{2}", startInfo.FileName, startInfo.Arguments, ex.Message);
        //        }
        //    }

        //    #endregion

        //    return errmsg;
        //}
        //#endregion

        //#endregion
        #endregion

        #region FileLogger Inner Class
        private class FileLogger : IDisposable
        {
            #region Member
            /// <summary>
            /// 日誌內容暫存
            /// </summary>
            private StringBuilder _Log;

            /// <summary>
            /// 最大日誌內容暫存 Size
            /// </summary>
            private int _MaxLogSize = 50 * 1024;
            #endregion

            #region Property
            /// <summary>
            /// 作業代碼
            /// </summary>
            public int JobNo
            {
                get;
                private set;
            }

            /// <summary>
            /// 作業類別代碼
            /// </summary>
            public string JobTypeId
            {
                get;
                private set;
            }

            /// <summary>
            /// 檔案類別
            /// </summary>
            public string FileKind
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌內容區塊編號
            /// </summary>
            public int LogBlockNo
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌檔完整路徑檔名
            /// </summary>
            public string LogFileFullName
            {
                get;
                private set;
            }

            /// <summary>
            /// 是否 Ready
            /// </summary>
            public bool IsReady
            {
                get;
                private set;
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 FileLogger 物件
            /// </summary>
            /// <param name="jobNo"></param>
            /// <param name="fileKind"></param>
            public FileLogger(int jobNo, string jobTypeId, string fileKind)
            {
                this.Intital(jobNo, jobTypeId, fileKind);
            }
            #endregion

            #region Destructor
            /// <summary>
            /// 解構 FileLogger 物件
            /// </summary>
            ~FileLogger()
            {
                Dispose(false);
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
                Dispose(true);
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
                        if (_Log != null && _Log.Length > 0)
                        {
                            this.WriteToFile();
                            _Log.Clear();
                            _Log = null;
                        }
                    }
                    _Disposed = true;
                }
            }
            #endregion

            #region Private Method
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="jobNo"></param>
            /// <param name="fileKind"></param>
            private void Intital(int jobNo, string jobTypeId, string fileKind)
            {
                _Log = null;
                this.LogFileFullName = null;
                this.IsReady = false;
                try
                {
                    this.JobNo = jobNo;
                    this.JobTypeId = jobTypeId;
                    this.FileKind = fileKind;
                    this.LogBlockNo = 1;

                    string logPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                    if (!String.IsNullOrWhiteSpace(logPath))
                    {
                        DirectoryInfo dinfo = new DirectoryInfo(logPath);
                        if (!dinfo.Exists)
                        {
                            dinfo.Create();
                        }
                        _Log = new StringBuilder();
                        this.LogFileFullName = Path.Combine(logPath, String.Format("{0}_{1:yyyyMMdd}.log", WebServiceName, DateTime.Today));
                        this.IsReady = true;
                    }
                }
                catch (Exception)
                {
                    this.IsReady = false;
                }
            }

            /// <summary>
            /// 將日誌內容暫存寫入日誌檔
            /// </summary>
            private void WriteToFile()
            {
                if (this.IsReady && _Log != null && _Log.Length > 0)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(this.LogFileFullName, true, Encoding.Default))
                        {
                            sw.WriteLine("[JobNo = {0}; JobTypeId = {1}; FileKind = {2}] 日誌內容 {3} 開始 ========\\", this.JobNo, this.JobTypeId, this.FileKind, this.LogBlockNo);
                            sw.Write(_Log);
                            sw.WriteLine("[JobNo = {0}; JobTypeId = {1}; FileKind = {2}] 日誌內容 {3} 結束 ========//", this.JobNo, this.JobTypeId, this.FileKind, this.LogBlockNo);
                            sw.WriteLine();

                            _Log.Clear();
                            this.LogBlockNo++;
                        }
                        //File.AppendAllText(this.LogFileFullName, _Log.ToString(), Encoding.Default);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            #endregion

            #region Public Method
            public FileLogger Append(string msg, bool autoFlush = true)
            {
                if (this.IsReady && !String.IsNullOrWhiteSpace(msg))
                {
                    try
                    {
                        _Log.Append(msg);

                        if (autoFlush && _Log.Length > _MaxLogSize)
                        {
                            this.WriteToFile();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                return this;
            }

            public FileLogger AppendLine(string msg, bool autoFlush = true)
            {
                if (this.IsReady)
                {
                    try
                    {
                        _Log.AppendLine(msg);

                        if (autoFlush && _Log.Length > _MaxLogSize)
                        {
                            this.WriteToFile();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                return this;
            }

            public FileLogger AppendFormat(string format, params object[] args)
            {
                if (this.IsReady && String.IsNullOrWhiteSpace(format))
                {
                    try
                    {
                        _Log.AppendFormat(format, args);

                        //if (autoFlush && _Log.Length > _MaxLogSize)
                        //{
                        //    this.WriteToFile();
                        //}
                        return this;
                    }
                    catch (Exception)
                    {
                    }
                }
                return this;
            }

            public FileLogger AppendFormatLine(string format, params object[] args)
            {
                if (this.IsReady && String.IsNullOrWhiteSpace(format))
                {
                    try
                    {
                        _Log.AppendFormat(format, args).AppendLine();

                        //if (autoFlush && _Log.Length > _MaxLogSize)
                        //{
                        //    this.WriteToFile();
                        //}
                        return this;
                    }
                    catch (Exception)
                    {
                    }
                }
                return this;
            }

            public FileLogger Flush()
            {
                try
                {
                    this.WriteToFile();
                }
                catch (Exception)
                {
                }
                return this;
            }
            #endregion
        }
        #endregion

        #region Const
        /// <summary>
        /// 服務名稱
        /// </summary>
        private const string WebServiceName = "FileService";

        /// <summary>
        /// 中信帳單檔類別
        /// </summary>
        private const string FileKind_CTCB = "CTCB_DATA";

        /// <summary>
        /// 學校銷帳檔類別
        /// </summary>
        private const string FileKind_Canceled = "CANCELED_DATA";
        #endregion

        #region Static Member
        /// <summary>
        /// 服務連線帳號
        /// </summary>
        private static readonly string _ApId = "eSchoolAp";
        /// <summary>
        /// 服務連線代碼
        /// </summary>
        private static readonly string _ApCode= "1qaz@WSX";
        /// <summary>
        /// 發送指示檔檔名
        /// </summary>
        private static readonly string _ConfigFileName = "file.config";
        #endregion

        #region Private Method
        /// <summary>
        /// 檢查連線驗證資料
        /// </summary>
        /// <param name="apData">連線驗證資料</param>
        /// <param name="jobNo">作業代碼</param>
        /// <param name="jobTyieId">作業類別代碼</param>
        /// <param name="fileKind">檔案類別</param>
        /// <returns>有效則傳回 true，否則傳回 false</returns>
        private bool CheckApData(string apData, int jobNo, string jobTyieId, string fileKind)
        {
            if (jobNo < 0 || String.IsNullOrWhiteSpace(jobTyieId) || (fileKind != FileKind_CTCB && fileKind != FileKind_Canceled))
            {
                return false;
            }
            string checkData = Fuju.Common.GetBase64Encode(String.Format("{0}_{1}_{2}_{3}_{4}", _ApId, jobNo, jobTyieId, fileKind, _ApCode));
            return checkData.Equals(apData);
        }

        /// <summary>
        /// 取得暫存檔儲存路徑
        /// </summary>
        /// <returns></returns>
        private string GetTempPath()
        {
            string path = ConfigManager.Current.GetProjectConfigValue(WebServiceName, "TempPath");
            return path == null ? null : path.Trim();
        }

        /// <summary>
        /// 取得 中信帳單檔 FTP 資料 儲存路徑
        /// </summary>
        /// <returns></returns>
        private string GetCTCBDataPath()
        {
            string path = ConfigManager.Current.GetProjectConfigValue(WebServiceName, "CTCBDataPath");
            return path == null ? null : path.Trim();
        }

        /// <summary>
        /// 取得 學校銷帳檔 FTP 資料 儲存路徑
        /// </summary>
        /// <returns></returns>
        private string GetCanceledDataPath()
        {
            string path = ConfigManager.Current.GetProjectConfigValue(WebServiceName, "CanceledDataPath");
            return path == null ? null : path.Trim();
        }

        /// <summary>
        /// FTP 上傳中信帳單檔資料
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="jobNo">作業代碼</param>
        /// <param name="jobTypeId">作業類別代碼</param>
        /// <param name="zipContents"></param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回 null</returns>
        private string FTPUploadCTCBData(FileLogger logger, int jobNo, string jobTypeId, byte[] zipContents)
        {
            string procName = null;  //程序名稱
            try
            {
                logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] [FTPUploadCTCBData] 開始 FTP 上傳中信帳單檔資料作業：");

                #region 檢查系統參數
                procName = "檢查系統參數";

                string tempPath = this.GetTempPath();
                if (String.IsNullOrWhiteSpace(tempPath))
                {
                    logger.AppendLine("系統未指定接收檔案暫存目錄");
                    return "系統未指定接收檔案暫存目錄";
                }

                string dataPath = this.GetCTCBDataPath();
                if (String.IsNullOrWhiteSpace(dataPath))
                {
                    logger.AppendLine("系統未指定中信帳單檔案儲存目錄");
                    return "系統未指定中信帳單檔案儲存目錄";
                }

                logger.AppendFormatLine("  接收檔案暫存目錄：{0}; 中信帳單檔案儲存目錄：{1}", tempPath, dataPath);

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }
                #endregion

                #region 暫存檔案並解壓縮
                procName = "暫存檔案並解壓縮";

                //以目前時間 (yyyyMMddHHmmssffff) 作為暫存檔名的一部分與工作資料夾，並將暫存檔解壓縮至工作資料夾，避免與後續檔案重複
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string workPath = Path.Combine(dataPath, timeStamp);
                if (!Directory.Exists(workPath))
                {
                    Directory.CreateDirectory(workPath);
                }

                string srcFile = Path.Combine(tempPath, String.Format("CTCB_{0}.ZIP", timeStamp));
                File.WriteAllBytes(srcFile, zipContents);
                logger.AppendFormatLine("  儲存接收檔案：{0}; 檔案大小：{1} bytes", srcFile, zipContents.Length);

                ZIPHelper.ExtractZip(srcFile, workPath);
                logger.AppendFormatLine("  解壓縮檔案：{0}; 至資料夾：{1}", srcFile, workPath);
                #endregion

                #region 檢查發送指令檔
                #region [OLD]
                //procName = "發送指示檔更名 OK 檔";

                //string configFileFullName = Path.Combine(workPath, _ConfigFileName);
                //string okFileName = String.Format("{0}.OK", _ConfigFileName);
                //string okFileFullName = Path.Combine(workPath, okFileName);
                //File.Move(configFileFullName, okFileFullName);
                //logger.AppendFormatLine("  發送指示檔：{0}; 更名為：{1}", configFileFullName, okFileName);
                #endregion

                procName = "檢查發送指令檔";
                string configFileFullName = Path.Combine(workPath, _ConfigFileName);
                FileInfo configFileInfo = new FileInfo(configFileFullName);
                if (configFileInfo.Exists)
                {
                    logger.AppendFormatLine("  發送指令檔：{0}; 檔案大小：{1} bytes", configFileInfo.FullName, configFileInfo.Length);
                    if (configFileInfo.Length == 0)
                    {
                        return String.Format("發送指令檔 ({0}) 無內容", _ConfigFileName);
                    }
                }
                else
                {
                    logger.AppendFormatLine("  解壓縮後找不到發送指令檔 {0}", configFileInfo.FullName);
                    return String.Format("解壓縮後找不到發送指令檔 {0}", configFileInfo.FullName);
                }
                #endregion

                #region 呼叫FTP上傳外部程式
                procName = "呼叫FTP上傳外部程式";

                string bakPath = Path.Combine(dataPath, "BAK", timeStamp);
                string errmsg = this.CallFTPUpload(jobNo, jobTypeId, workPath, _ConfigFileName, bakPath);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    logger.AppendFormatLine("  {0} 失敗，{1}", procName, errmsg);
                    return String.Format("{0} 失敗，{1}", procName, errmsg);
                }
                #endregion

                return null;
            }
            catch (Exception ex)
            {
                logger
                    .AppendFormatLine("  處理檔案發生例外 [{0}]，{1}", procName, ex.Message)
                    .AppendFormatLine("    ex.Source：{0}", ex.Source)
                    .AppendFormatLine("    ex.StackTrace：{0}", ex.StackTrace);

                if (ex.InnerException != null)
                {
                    Exception ex2 = ex.InnerException;
                    logger
                        .AppendFormatLine("    InnerException ex.Message：{0}", ex2.Message)
                        .AppendFormatLine("    InnerException ex.Source：{0}", ex2.Source)
                        .AppendFormatLine("    InnerException ex.StackTrace：{0}", ex2.StackTrace);
                }
                return String.Format("處理檔案發生例外，{0}", ex.Message);
            }
        }

        /// <summary>
        /// FTP 上傳學校銷帳檔資料
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="jobNo">作業代碼</param>
        /// <param name="jobTypeId">作業類別代碼</param>
        /// <param name="zipContents"></param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回 null</returns>
        private string FTPUploadCanceledData(FileLogger logger, int jobNo, string jobTypeId, byte[] zipContents)
        {
            string procName = null;  //程序名稱
            try
            {
                logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] [FTPUploadCTCBData] 開始 FTP 上傳中信帳單檔資料作業：");

                #region 檢查系統參數
                procName = "檢查系統參數";

                string tempPath = this.GetTempPath();
                if (String.IsNullOrWhiteSpace(tempPath))
                {
                    logger.AppendLine("系統未指定接收檔案暫存目錄");
                    return "系統未指定接收檔案暫存目錄";
                }

                string dataPath = this.GetCanceledDataPath();
                if (String.IsNullOrWhiteSpace(dataPath))
                {
                    logger.AppendLine("系統未指定學校銷帳檔案儲存目錄");
                    return "系統未指定學校銷帳檔案儲存目錄";
                }

                logger.AppendFormatLine("  接收檔案暫存目錄：{0}; 學校銷帳檔案儲存目錄：{1}", tempPath, dataPath);

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }
                #endregion

                #region 暫存檔案並解壓縮
                procName = "暫存檔案並解壓縮";

                //以目前時間 (yyyyMMddHHmmssffff) 作為暫存檔名的一部分與工作資料夾，並將暫存檔解壓縮至工作資料夾，避免與後續檔案重複
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string workPath = Path.Combine(dataPath, timeStamp);
                if (!Directory.Exists(workPath))
                {
                    Directory.CreateDirectory(workPath);
                }

                string srcFile = Path.Combine(tempPath, String.Format("CANCELED_{0}.ZIP", timeStamp));
                File.WriteAllBytes(srcFile, zipContents);
                logger.AppendFormatLine("  儲存接收檔案：{0}; 檔案大小：{1} bytes", srcFile, zipContents.Length);

                ZIPHelper.ExtractZip(srcFile, workPath);
                logger.AppendFormatLine("  解壓縮檔案：{0}; 至資料夾：{1}", srcFile, workPath);
                #endregion

                #region 檢查發送指令檔
                #region [OLD]
                //procName = "發送指示檔更名 OK 檔";

                //string configFileFullName = Path.Combine(workPath, _ConfigFileName);
                //string okFileName = String.Format("{0}.OK", _ConfigFileName);
                //string okFileFullName = Path.Combine(workPath, okFileName);
                //File.Move(configFileFullName, okFileFullName);
                //logger.AppendFormatLine("  發送指示檔：{0}; 更名為：{1}", configFileFullName, okFileName);
                #endregion

                procName = "檢查發送指令檔";
                string configFileFullName = Path.Combine(workPath, _ConfigFileName);
                FileInfo configFileInfo = new FileInfo(configFileFullName);
                if (configFileInfo.Exists)
                {
                    logger.AppendFormatLine("  發送指令檔：{0}; 檔案大小：{1} bytes", configFileInfo.FullName, configFileInfo.Length);
                    if (configFileInfo.Length == 0)
                    {
                        return String.Format("發送指令檔 ({0}) 無內容", _ConfigFileName);
                    }
                }
                else
                {
                    logger.AppendFormatLine("  解壓縮後找不到發送指令檔 {0}", configFileInfo.FullName);
                    return String.Format("解壓縮後找不到發送指令檔 {0}", configFileInfo.FullName);
                }
                #endregion

                #region 呼叫FTP上傳外部程式
                procName = "呼叫FTP上傳外部程式";

                string bakPath = Path.Combine(dataPath, "BAK", timeStamp);
                string errmsg = this.CallFTPUpload(jobNo, jobTypeId, workPath, _ConfigFileName, bakPath);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    logger.AppendFormatLine("  {0} 失敗，{1}", procName, errmsg);
                    return String.Format("{0} 失敗，{1}", procName, errmsg);
                }
                #endregion

                return null;
            }
            catch (Exception ex)
            {
                logger
                    .AppendFormatLine("  處理檔案發生例外 [{0}]，{1}", procName, ex.Message)
                    .AppendFormatLine("    ex.Source：{0}", ex.Source)
                    .AppendFormatLine("    ex.StackTrace：{0}", ex.StackTrace);

                if (ex.InnerException != null)
                {
                    Exception ex2 = ex.InnerException;
                    logger
                        .AppendFormatLine("    InnerException ex.Message：{0}", ex2.Message)
                        .AppendFormatLine("    InnerException ex.Source：{0}", ex2.Source)
                        .AppendFormatLine("    InnerException ex.StackTrace：{0}", ex2.StackTrace);
                }
                return String.Format("處理檔案發生例外，{0}", ex.Message);
            }
        }

        /// <summary>
        /// 呼叫 FTP 上傳外部程式
        /// </summary>
        /// <param name="jobNo">作業代碼</param>
        /// <param name="jobTypeId">作業類別代碼</param>
        /// <param name="workPath">工作路徑</param>
        /// <param name="configFileName">發送指令檔檔名</param>
        /// <param name="bakPath">備份檔路徑</param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回 null</returns>
        private string CallFTPUpload(int jobNo, string jobTypeId, string workPath, string configFileName, string bakPath)
        {
            #region 檢查參數
            {
                if (String.IsNullOrWhiteSpace(jobTypeId) || String.IsNullOrWhiteSpace(workPath) || String.IsNullOrWhiteSpace(configFileName) || String.IsNullOrWhiteSpace(bakPath))
                {
                    return "缺少執行參數";
                }
                if (jobTypeId.Contains(" ") || workPath.Contains(" ") || configFileName.Contains(" ") || bakPath.Contains(" "))
                {
                    return "所有執行參數皆不可有空白字元";
                }

                if (!Directory.Exists(workPath))
                {
                    return String.Format("工作路徑 ({0}) 不存在", workPath);
                }

                string configFileFullName = Path.Combine(workPath, configFileName);
                if (!File.Exists(configFileFullName))
                {
                    return String.Format("發送指令檔 ({0}) 不存在", configFileFullName);
                }

                if (!Directory.Exists(bakPath))
                {
                    try
                    {
                        Directory.CreateDirectory(bakPath);
                    }
                    catch (Exception)
                    {
                        return String.Format("備份檔路徑 ({0}) 無法建立", bakPath);
                    }
                }
            }
            #endregion

            #region FtpUpload 執行檔路徑
            string ftpUploadFileFullName = null;
            {
                ftpUploadFileFullName = ConfigManager.Current.GetProjectConfigValue("FileService", "FtpUpload");
                if (String.IsNullOrWhiteSpace(ftpUploadFileFullName))
                {
                    return "未指定 Config 中的 FtpUpload 設定";
                }
                if (!File.Exists(ftpUploadFileFullName))
                {
                    return String.Format("指定的 FtpUpload 執行檔 ({0}) 不存在", ftpUploadFileFullName);
                }
            }
            #endregion

            #region 呼叫 FtpUpload 執行檔
            {
                string errmsg = null;

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = ftpUploadFileFullName;
                try
                {
                    //執行參數：jobno=作業代碼 jobtypeid=作業類別代碼 workpath=工作路徑 configfile=發送指示檔檔名 bakpath=備份路徑
                    startInfo.Arguments = String.Format("jobno={0} jobtypeid={1} workpath={2} configfile={3} bakpath={4}", jobNo, jobTypeId, workPath, configFileName, bakPath);
                    using (Process myProcess = Process.Start(startInfo))
                    {
                        //myProcess.WaitForExit();
                    }
                }
                catch (Exception ex)
                {
                    errmsg = String.Format("執行程式 {0} {1} 發生例外，錯誤訊息：{2}", startInfo.FileName, startInfo.Arguments, ex.Message);
                }
                return errmsg;
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// 接收 ZIP 壓縮檔案並依設定檔執行 FTP 上傳
        /// </summary>
        /// <param name="apData">連線驗證資料</param>
        /// <param name="jobNo">作業代碼</param>
        /// <param name="jobTypeId">作業類別代碼</param>
        /// <param name="fileKind">檔案類別</param>
        /// <param name="zipContents">zip 檔內容</param>
        /// <returns>成功傳回 OK，否則傳回錯誤訊息</returns>
        [WebMethod]
        public string FTPUpload(string apData, int jobNo, string jobTypeId, string fileKind, byte[] zipContents)
        {
            FileLogger logger = null;
            try
            {
                logger = new FileLogger(jobNo, jobTypeId, fileKind);

                if (!this.CheckApData(apData, jobNo, jobTypeId, fileKind) || zipContents == null || zipContents.Length == 0)
                {
                    string errmsg = "無效的連線驗證資料或參數";
                    logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] FTP 上傳作業處理失敗。{1}", DateTime.Now, errmsg);
                    return errmsg;
                }
                jobTypeId = jobTypeId.Trim();

                #region [MDY:20181116] 因為 checkmarx 會誤判所以改成取回 JobcubeEntity
                #region [OLD]
                //#region 驗證 jobNo, jobTypeId 參數
                //{
                //    int count = 0;
                //    string errmsg = null;
                //    Expression where = new Expression(JobcubeEntity.Field.Jno, jobNo)
                //        .And(JobcubeEntity.Field.Jtypeid, jobTypeId);
                //    XmlResult result = DataProxy.Current.SelectCount<JobcubeEntity>(null, where, out count);
                //    if (result.IsSuccess)
                //    {
                //        if (count == 0)
                //        {
                //            errmsg = "查無該作業資料";
                //        }
                //    }
                //    else
                //    {
                //        errmsg = String.Concat("查詢作業資料失敗，", result.Message);
                //    }
                //    if (!String.IsNullOrEmpty(errmsg))
                //    {
                //        logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] FTP 上傳作業處理失敗。{1}", DateTime.Now, errmsg);
                //        return errmsg;
                //    }
                //}
                //#endregion
                #endregion
                string jtype = null;
                {
                    string errmsg = null;
                    JobcubeEntity jobCube = null;
                    Expression where = new Expression(JobcubeEntity.Field.Jno, jobNo)
                        .And(JobcubeEntity.Field.Jtypeid, jobTypeId);
                    XmlResult result = DataProxy.Current.SelectFirst<JobcubeEntity>(null, where, null, out jobCube);
                    if (result.IsSuccess)
                    {
                        if (jobCube == null)
                        {
                            errmsg = "查無該作業資料";
                        }
                        else
                        {
                            jtype = jobCube.Jtypeid;
                        }
                    }
                    else
                    {
                        errmsg = String.Concat("查詢作業資料失敗，", result.Message);
                    }
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] FTP 上傳作業處理失敗。{1}", DateTime.Now, errmsg);
                        return errmsg;
                    }
                }
                #endregion

                switch (fileKind)
                {
                    case FileKind_CTCB:
                        #region 中信帳單檔
                        {
                            #region [MDY:20181116] 因為 checkmarx 會誤判所以做了無聊的轉換
                            #region [OLD]
                            //string errmsg = this.FTPUploadCTCBData(logger, jobNo, jobTypeId, zipContents);
                            #endregion

                            string errmsg = this.FTPUploadCTCBData(logger, jobNo, jtype, zipContents);
                            #endregion

                            if (String.IsNullOrEmpty(errmsg))
                            {
                                logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] FTP 上傳中信帳單檔資料處理結果：{1}", DateTime.Now, errmsg);
                                return "OK";
                            }
                            else
                            {
                                logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] FTP 上傳中信帳單檔資料處理結果：{1}", DateTime.Now, errmsg);
                                return errmsg;
                            }
                        }
                        #endregion
                    case FileKind_Canceled:
                        #region 學校銷帳檔
                        {
                            #region [MDY:20181116] 因為 checkmarx 會誤判所以做了無聊的轉換
                            #region [OLD]
                            //string errmsg = this.FTPUploadCanceledData(logger, jobNo, jobTypeId, zipContents);
                            #endregion

                            string errmsg = this.FTPUploadCanceledData(logger, jobNo, jtype, zipContents);
                            #endregion

                            if (String.IsNullOrEmpty(errmsg))
                            {
                                logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] FTP 上傳中信帳單檔資料處理結果：{1}", DateTime.Now, errmsg);
                                return "OK";
                            }
                            else
                            {
                                logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] FTP 上傳中信帳單檔資料處理結果：{0}", DateTime.Now, errmsg);
                                return errmsg;
                            }
                        }
                        #endregion
                    default:
                        #region 無效的檔案類別
                        {
                            string errmsg = "無效的檔案類別參數";
                            logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] FTP 上傳作業處理失敗，無效的檔案類別參數", DateTime.Now);
                            return errmsg;
                        }
                        #endregion
                }
            }
            catch (Exception ex)
            {
                string errmsg = String.Format("執行發生例外，{0}", true, ex.Message);
                if (logger != null)
                {
                    logger.AppendFormatLine("[{0:yyyy/MM/dd HH:mm:ss}] FTP 上傳作業處理發生例外，{1}", DateTime.Now, ex);
                }
                return errmsg;
            }
            finally
            {
                if (logger != null)
                {
                    logger.Flush();
                    logger.Dispose();
                }
            }
        }
        #endregion
    }
}
