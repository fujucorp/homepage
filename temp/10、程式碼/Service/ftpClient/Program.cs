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

using Common.Utility;

namespace ftpClient
{
    /// <summary>
    /// 異業代收資料檔下載 (F80：下載中國信託中心媒體檔、F81：下載統一超商中心媒體檔、F82：下載全家超商中心媒體檔、F83：下載OK超商中心媒體檔、F85：下載萊爾富超商中心媒體檔、F87：下載財金中心媒體檔)
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


        #region
        /// <summary>
        /// 將特殊的日期符號換成對應的日期字串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string ReplaceDateSymbol2DateText(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return String.Empty;
            }
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);
            return text.Trim()
                .Replace("<date8>", today.ToString("yyyyMMdd"))
                .Replace("<date7>", String.Format("{0:000}{1:MMdd}", today.Year - 1911, today))
                .Replace("<yesterday>", yesterday.ToString("yyyyMMdd"))
                .Replace("<yesterday7>", String.Format("{0:000}{1:MMdd}", yesterday.Year - 1911, yesterday));
        }

        /// <summary>
        /// 檢查資料夾路徑，如果資料夾不存在則嘗試建立
        /// </summary>
        /// <param name="path">指定檢查的資料夾路徑</param>
        /// <param name="chkWrite">指定是否檢查寫入權限</param>
        /// <returns>傳回錯誤訊息</returns>
        private static string CheckFolder(string path, bool chkWrite = false)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return "未指定資料夾路徑";
            }
            else
            {
                try
                {
                    DirectoryInfo info = new DirectoryInfo(path.Trim());
                    if (!info.Exists)
                    {
                        info.Create();
                    }

                    #region 測試寫入權限
                    if (chkWrite)
                    {
                        try
                        {
                            string fileName = Path.GetTempFileName();
                            File.WriteAllText(fileName, "測試寫入權限...");
                            File.Delete(fileName);
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return null;
            }
        }

        #region 下載檔案用方法
        /// <summary>
        /// 拆解指定的FTP清單項目字串
        /// </summary>
        /// <param name="listItem">指定的FTP清單項目字串</param>
        /// <param name="itemType">成功則傳回項目類型 (FILE:檔案 / DIR:目錄)</param>
        /// <param name="itemDate">成功則傳回項目日期 (不含時間)</param>
        /// <param name="itemTime">成功則傳回項目時間 (hh:mmpm)</param>
        /// <param name="itemSize">成功則傳回項目大小 (itemType=FILE時才有值，否則為 0)</param>
        /// <param name="itemName">成功則傳回項目名稱</param>
        /// <param name="errmsg">成功則傳回 null，否則傳回錯誤訊</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private static bool ParseListItem(string listItem, out string itemType, out DateTime? itemDate, out string itemTime, out int? itemSize, out string itemName, out string errmsg)
        {
            bool isOK = false;
            itemType = null;
            itemDate = null;
            itemTime = null;
            itemSize = null;
            itemName = null;
            errmsg = null;
            try
            {
                //日期 (mm-dd-yy) + " " + 時間 (hh:mmpm) + " " + 屬性 (<DIR>:目錄 / 數字:檔案 Size) + " " + 名稱
                string[] itemInfos = listItem.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (itemInfos.Length == 4)
                {
                    #region 處理日期 (mm-dd-yy)
                    {
                        DateTime date;
                        string dateTxt = itemInfos[0].Trim(); //mm-dd-yy
                        string[] dateParts = dateTxt.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                        if (dateParts.Length == 3 && DateTime.TryParse(String.Format("20{0}/{1}/{2}", dateParts[2], dateParts[0], dateParts[1]), out date))
                        {
                            itemDate = date;
                        }
                        else
                        {
                            errmsg = string.Format("日期格式錯誤 ({0})", dateTxt);
                            return false;
                        }
                    }
                    #endregion

                    #region 處理時間 (hh:mmpm)
                    itemTime = itemInfos[1].Trim(); //hh:mmpm
                    #endregion

                    #region 處理屬性 (<DIR>:目錄 / 數字:檔案 Size)
                    {
                        int value = 0;
                        string info3 = itemInfos[2].Trim(); //<DIR>:為目錄，數字為檔案 Size，其他 ??
                        if (info3.Equals("<DIR>", StringComparison.CurrentCultureIgnoreCase))
                        {
                            itemType = "DIR";
                            itemSize = 0;
                        }
                        else if (Int32.TryParse(info3, out value))
                        {
                            itemType = "FILE";
                            itemSize = value;
                        }
                        else
                        {
                            itemType = info3;
                            itemSize = 0;
                        }
                    }
                    #endregion

                    #region 處理名稱
                    itemName = itemInfos[3].Trim(); //名稱
                    #endregion

                    isOK = true;
                }
                else
                {
                    errmsg = "清單項目格式錯誤";
                    isOK = false;
                }
            }
            catch (Exception ex)
            {
                errmsg = string.Format("拆解發生例外，錯誤訊息：{0}", ex.Message);
                isOK = false;
            }
            return isOK;
        }

        /// <summary>
        /// 從指定的FTP清單陣列中找出符合指定檔名與最小日期限制的檔案名稱集合
        /// </summary>
        /// <param name="listItems">指定的FTP清單陣列</param>
        /// <param name="fileName">指定檔案名稱</param>
        /// <param name="lastDate">指定最小日期限制</param>
        /// <param name="log">傳回處理日誌</param>
        /// <returns>傳回符合的檔案名稱集合</returns>
        private static List<string> FindFiles(string[] listItems, string fileName, DateTime? lastDate, out string log)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                log = "未指定要找尋的檔案名稱";
                return new List<string>(0);
            }
            if (listItems == null || listItems.Length == 0)
            {
                log = "未指定FTP清單陣列，或無資料";
                return new List<string>(0);
            }

            fileName = fileName.Trim();
            if (lastDate != null)
            {
                lastDate = lastDate.Value.Date; //去除時間的部分
            }

            StringBuilder msg = new StringBuilder();
            List<string> fileNames = new List<string>(listItems.Length);

            foreach (string listItem in listItems)
            {
                string itemType = null;
                DateTime? itemDate = null;
                string itemTime = null;
                int? itemSize = null;
                string itemName = null;
                string errmsg = null;
                if (ParseListItem(listItem, out itemType, out itemDate, out itemTime, out itemSize, out itemName, out errmsg))
                {
                    if (itemType != "FILE")
                    {
                        msg.AppendFormat("清單項目 ({0}) 不是檔案類型", listItem).AppendLine();
                        continue;
                    }
                    if (itemDate == null)
                    {
                        msg.AppendFormat("清單項目 ({0}) 無法取得日期", listItem).AppendLine();
                        continue;
                    }
                    if (lastDate != null && lastDate.Value > itemDate.Value)
                    {
                        msg.AppendFormat("清單項目 ({0}) 的日期 ({1:yyyy/MM/dd}) 小於最小日期限制 ({2:yyyy/MM/dd})", listItem, itemDate.Value, lastDate.Value).AppendLine();
                        continue;
                    }
                    if (fileName == "*" || fileName.Equals(itemName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        fileNames.Add(itemName);
                    }
                    else
                    {
                        msg.AppendFormat("清單項目 ({0}) 的檔名 ({1}) 與指定檔名不符 ({2})", listItem, itemName, fileName).AppendLine();
                        continue;
                    }
                }
                else
                {
                    msg.AppendFormat("清單項目 ({0}) 拆解字串格式失敗。", listItem).AppendLine();
                }
            }

            log = msg.ToString();
            return fileNames;
        }

        /// <summary>
        /// 取得 FTP 上指定路徑中符合條件的檔案清單
        /// </summary>
        /// <param name="ftpHelper">FTP處理工具</param>
        /// <param name="fileName">指定檔案名稱</param>
        /// <param name="lastDate">指定最小日期限制</param>
        /// <param name="log">指定日誌紀錄 StringBuilder 物件</param>
        /// <param name="fileNames">傳回符合的檔案名稱集合</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        private static string GetFTPFileList(FTPUtility ftpHelper, string fileName, DateTime? lastDate, int retryTimes, int retrySleep, ref StringBuilder log, out List<string> fileNames)
        {
            fileNames = null;
            string errmsg = null;
            for (int timeNo = 0; timeNo <= retryTimes; timeNo++)
            {
                try
                {
                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始第 {1} 次讀取遠端資料夾 ({2}) 清單", DateTime.Now, timeNo, ftpHelper.CurrentFolder).AppendLine();

                    string listText = "";
                    if (ftpHelper.GetListDirectory(out listText))
                    {
                        if (String.IsNullOrWhiteSpace(listText))
                        {
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 遠端資料夾 ({1}) 無任何清單", DateTime.Now, ftpHelper.CurrentFolder).AppendLine();
                            errmsg = String.Format("讀取遠端資料夾 ({0}) 清單失敗，無任何清單", ftpHelper.CurrentFolder);
                        }
                        else
                        {
                            string[] listItems = listText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                            string log2 = null;
                            fileName = ReplaceDateSymbol2DateText(fileName);
                            fileNames = FindFiles(listItems, fileName, lastDate, out log2);
                            if (fileNames == null || fileNames.Count == 0)
                            {
                                string lastDateText = lastDate == null ? String.Empty : lastDate.Value.ToString("yyyyMMdd");
                                log
                                    .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 遠端資料夾 ({1}) 下無任何符合條件 (fileName={2}, lastDate={3}) 的檔案", DateTime.Now, ftpHelper.CurrentFolder, fileName, lastDateText).AppendLine()
                                    .AppendLine("遠端資料夾清單：")
                                    .AppendLine(listText)
                                    .AppendLine()
                                    .AppendLine("檢查結果：")
                                    .AppendLine(log2)
                                    .AppendLine();
                                errmsg = String.Format("讀取遠端資料夾 ({0}) 清單失敗，無任何符合條件 (fileName={1}, lastDate={2}) 的檔案", ftpHelper.CurrentFolder, fileName, lastDateText);
                            }
                            else
                            {
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 共找到 {1} 的檔案 ({2})", DateTime.Now, fileNames.Count, String.Join(",", fileNames.ToArray())).AppendLine();
                                errmsg = null;
                            }
                        }
                    }
                    else
                    {
                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 讀取遠端資料夾 ({1}) 清單失敗", DateTime.Now, ftpHelper.CurrentFolder).AppendLine();
                        errmsg = String.Format("讀取遠端資料夾 ({0}) 清單失敗", ftpHelper.CurrentFolder);
                    }
                }
                catch (Exception ex)
                {
                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 讀取遠端資料夾 ({1}) 的清單發生例外，錯誤訊息：{2}", DateTime.Now, ftpHelper.CurrentFolder, ex.Message).AppendLine();
                    errmsg = String.Format("讀取遠端資料夾 ({0}) 清單發生例外，錯誤訊息：{1}", ftpHelper.CurrentFolder, ex.Message);
                }

                if (String.IsNullOrEmpty(errmsg))
                {
                    break;
                }
                else if (retryTimes > 0 && timeNo < retryTimes)
                {
                    Thread.Sleep(1000 * 60 * retrySleep);
                }
            }
            return errmsg;
        }

        /// <summary>
        /// 下載 FTP 上指定檔名的檔案
        /// </summary>
        /// <param name="ftpHelper">FTP處理工具</param>
        /// <param name="remoteFiles">指定遠端的檔案名稱</param>
        /// <param name="localPath">指定本地端的路徑</param>
        /// <param name="log">指定日誌紀錄 StringBuilder 物件</param>
        /// <param name="failFiles">傳回下載失敗的檔案名稱集合</param>
        /// <returns></returns>
        private static string DownloadFTPFiles(FTPUtility ftpHelper, List<string> remoteFiles, string localPath, string tempPath, int retryTimes, int retrySleep, ref StringBuilder log, out List<string> failFiles)
        {
            failFiles = null;
            string errmsg = null;
            if (remoteFiles != null && remoteFiles.Count > 0)
            {
                string[] dwFiles = remoteFiles.ToArray();
                failFiles = new List<string>(dwFiles.Length);
                int timeNo = 0;
                for (timeNo = 0; timeNo <= retryTimes; timeNo++)
                {
                    failFiles.Clear();
                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始第 {1} 次下載檔案 ({2})", DateTime.Now, timeNo, String.Join(",", dwFiles)).AppendLine();

                    #region 逐檔下載
                    foreach (string remoteFile in dwFiles)
                    {
                        try
                        {
                            string tempFileFullName = Path.Combine(tempPath, remoteFile + ".tmp");
                            string localFileFullName = Path.Combine(localPath, remoteFile);
                            if (ftpHelper.Download(remoteFile, tempFileFullName, true))
                            {
                                #region 移動暫存檔至 localPath
                                string moveError = null;
                                try
                                {
                                    if (File.Exists(localFileFullName))
                                    {
                                        File.Delete(localFileFullName);
                                    }
                                    File.Move(tempFileFullName, localFileFullName);
                                }
                                catch (Exception ex)
                                {
                                    moveError = String.Format("將下載暫存檔 {0} 移至 {1} 失敗，錯誤訊息：{2}", tempFileFullName, localFileFullName, ex.Message);
                                }
                                #endregion

                                if (String.IsNullOrEmpty(moveError))
                                {
                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 下載檔案 ({1}) 成功", DateTime.Now, remoteFile).AppendLine();
                                }
                                else
                                {
                                    failFiles.Add(remoteFile);
                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 下載檔案 ({1}) 成功，但是{2}", DateTime.Now, remoteFile, moveError).AppendLine();
                                }
                            }
                            else
                            {
                                failFiles.Add(remoteFile);
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 下載檔案 ({1}) 失敗", DateTime.Now, remoteFile).AppendLine();
                            }
                        }
                        catch (Exception ex)
                        {
                            failFiles.Add(remoteFile);
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 下載檔案 ({1}) 發生例外，錯誤訊息：{2}", DateTime.Now, remoteFile, ex.Message).AppendLine();
                        }
                    }
                    #endregion

                    if (failFiles.Count == 0)
                    {
                        break;
                    }
                    else if (retryTimes > 0 && timeNo < retryTimes)
                    {
                        dwFiles = failFiles.ToArray();

                        Thread.Sleep(1000 * 60 * retrySleep);
                    }
                }
                if (failFiles.Count > 0)
                {
                    errmsg = String.Format("下載檔案 ({0}) 失敗", String.Join("、", failFiles.ToArray()));
                }
                else
                {
                    errmsg = null;
                }
            }
            else
            {
                errmsg = "沒有要下載的檔案";
            }
            return errmsg;
        }
        #endregion

        #region 上傳檔案用方法
        /// <summary>
        /// 取得本機端指定路徑中符合檔名條件的檔案清單
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="localFile"></param>
        /// <param name="log"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string GetLocalFileList(string localPath, string localFile, int retryTimes, int retrySleep, ref StringBuilder log, out List<string> files)
        {
            files = null;
            if (String.IsNullOrWhiteSpace(localPath))
            {
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 未指定本地端資料夾", DateTime.Now).AppendLine();
                return "未指定本地端資料夾";
            }
            if (String.IsNullOrWhiteSpace(localFile))
            {
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 未指定本地端檔案", DateTime.Now).AppendLine();
                return "未指定本地端檔案";
            }

            string errmsg = null;
            for (int timeNo = 0; timeNo <= retryTimes; timeNo++)
            {
                try
                {
                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始第 {1} 次讀取本地端資料夾 ({2}) 檔案清單", DateTime.Now, timeNo, localPath).AppendLine();

                    DirectoryInfo dirInfo = new DirectoryInfo(localPath);
                    if (dirInfo.Exists)
                    {
                        FileInfo[] fileInfos = null;
                        if (localFile == "*")
                        {
                            fileInfos = dirInfo.GetFiles();
                            if (fileInfos == null || fileInfos.Length == 0)
                            {
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 本地端資料夾 ({1}) 無任何檔案", DateTime.Now, localPath).AppendLine();
                                errmsg = String.Format("讀取本地端資料夾 ({0}) 檔案清單失敗，無任何檔案", localPath);
                            }
                        }
                        else
                        {
                            localFile = ReplaceDateSymbol2DateText(localFile);
                            fileInfos = dirInfo.GetFiles(localFile);
                            if (fileInfos == null || fileInfos.Length == 0)
                            {
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 本地端資料夾 ({1}) 下無任何符合條件 (localFile={2}) 的檔案", DateTime.Now, localPath, localFile).AppendLine();
                                errmsg = String.Format("讀取本地端資料夾 ({0}) 檔案清單失敗，無任何符合條件 (localFile={1}) 的檔案", localPath, localFile);
                            }
                        }

                        if (fileInfos != null && fileInfos.Length > 0)
                        {
                            files = new List<string>(fileInfos.Length);
                            foreach (FileInfo fileInfo in fileInfos)
                            {
                                files.Add(fileInfo.Name);
                            }
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 共找到 {1} 的檔案 ({2})", DateTime.Now, files.Count, String.Join(",", files.ToArray())).AppendLine();
                            errmsg = null;
                        }
                      }
                    else
                    {
                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 本地端資料夾 ({1}) 不存在", DateTime.Now, localPath).AppendLine();
                        errmsg = String.Format("讀取本地端資料夾 ({0}) 檔案清單失敗，資料夾不存在", localPath);
                    }
                }
                catch (Exception ex)
                {
                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 讀取本地端資料夾 ({1}) 檔案清單發生例外，錯誤訊息：{2}", DateTime.Now, localPath, ex.Message).AppendLine();
                    errmsg = String.Format("讀取本地端資料夾 ({0}) 檔案清單發生例外，錯誤訊息：{1}", localPath, ex.Message);
                }

                if (String.IsNullOrEmpty(errmsg))
                {
                    break;
                }
                else if (retryTimes > 0 && timeNo < retryTimes)
                {
                    Thread.Sleep(1000 * 60 * retrySleep);
                }
            }
            return errmsg;
        }

        /// <summary>
        /// 上傳指定檔名的檔案到 FTP 伺服器
        /// </summary>
        /// <param name="ftpHelper"></param>
        /// <param name="localPath"></param>
        /// <param name="localFiles"></param>
        /// <param name="needBak"></param>
        /// <param name="bakPath"></param>
        /// <param name="retryTimes"></param>
        /// <param name="retrySleep"></param>
        /// <param name="log"></param>
        /// <param name="failFiles"></param>
        /// <returns></returns>
        private static string UploadFTPFiles(FTPUtility ftpHelper, string localPath, List<string> localFiles, bool needBak, string bakPath, int retryTimes, int retrySleep, ref StringBuilder log, out List<string> failFiles)
        {
            failFiles = null;
            string errmsg = null;
            if (localFiles != null && localFiles.Count > 0)
            {
                string[] upFiles = localFiles.ToArray();
                failFiles = new List<string>(upFiles.Length);
                int timeNo = 0;
                for (timeNo = 0; timeNo <= retryTimes; timeNo++)
                {
                    failFiles.Clear();
                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始第 {1} 次上傳檔案 ({2})", DateTime.Now, timeNo, String.Join(",", upFiles)).AppendLine();

                    #region 逐檔上傳
                    foreach (string localFile in upFiles)
                    {
                        try
                        {
                            string localFileFullName = Path.Combine(localPath, localFile);
                            if (ftpHelper.Upload(localFileFullName, localFile))
                            {
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 上傳檔案 ({1}) 成功", DateTime.Now, localFile).AppendLine();

                                #region 備份檔案
                                if (needBak && !String.IsNullOrWhiteSpace(bakPath))
                                {
                                    string bakFileFullName = null;
                                    try
                                    {
                                        bakFileFullName = Path.Combine(bakPath, String.Format("{0}.{1:yyyyMMddHHmmss}", localFile, DateTime.Now));
                                        if (!Directory.Exists(bakPath))
                                        {
                                            Directory.CreateDirectory(bakPath);
                                        }
                                        File.Move(localFileFullName, bakFileFullName);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 上傳檔案 ({1}) 成功，但備分該檔案至 {2} 失敗，錯誤訊息：{3}", DateTime.Now, localFile, bakFileFullName, ex.Message).AppendLine();
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                failFiles.Add(localFile);
                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 上傳檔案 ({1}) 失敗", DateTime.Now, localFile).AppendLine();
                            }
                        }
                        catch (Exception ex)
                        {
                            failFiles.Add(localFile);
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 上傳檔案 ({1}) 發生例外，錯誤訊息：{2}", DateTime.Now, localFile, ex.Message).AppendLine();
                        }
                    }
                    #endregion

                    if (failFiles.Count == 0)
                    {
                        break;
                    }
                    else if (retryTimes > 0 && timeNo < retryTimes)
                    {
                        upFiles = failFiles.ToArray();

                        Thread.Sleep(1000 * 60 * retrySleep);
                    }
                }
                if (failFiles.Count > 0)
                {
                    errmsg = String.Format("下載檔案 ({0}) 失敗", String.Join("、", failFiles.ToArray()));
                }
                else
                {
                    errmsg = null;
                }
            }
            else
            {
                errmsg = "沒有要上傳的檔案";
            }
            return errmsg;
        }
        #endregion
        #endregion

        /// <summary>
        /// 最大失敗重試次數 : 8
        /// </summary>
        private const int _MaxRetryTimes = 8;
        /// <summary>
        /// 最大失敗重試間隔(單位分鐘) : 60
        /// </summary>
        private const int _MaxRetrySleep = 60;

        /// <summary>
        /// 參數：host=FTP伺服器 [port=FTP埠號(預設21)] [ssl=是否使用SSL協定(true/false，預設false)] [codepage=FTP編碼(預設950)] uid=FTP帳號 ｐｗｄ=FTP密碼 method=執行FTP指令(get/put) remote_path=遠端路徑 [remote_file=遠端檔案(get時必須指定)] local_path=本地端路徑 [local_file=本地端檔案(put時必須指定)] [bak=上傳檔案是否備份(true/false)] [last_date=限制下載檔案的最小日期(TODAY, YESTERDAY 或 yyyyMMdd)] service_id=所屬作業類別代碼(F80、F81、F82、F83、F85、F87) [retry_times=重試次數(0~8，預設5)] [retry_sleep=重試間隔，單位分鐘(0~60，預設15)]
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>ｐｗｄ參數實際是半形英文，因為 checkmarx 過敏只好用全形做說明</remarks>
        static void Main(string[] args)
        {
            #region Initial
            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);

            FileLoger fileLog = new FileLoger(appName);
            string tempPath = GetTempPath();
            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = null;        //作業類別代碼
            string jobTypeName = null;      //作業類別名稱
            int exitCode = 0;
            string exitMsg = null;
            #endregion

            StringBuilder log = new StringBuilder();    //日誌紀錄

            int retryTimes = 5;     //re-try 次數 (預設為5次)
            int retrySleep = 15;    //re-try 間隔，單位分鐘 (預設為15分鐘)

            DateTime startTime = DateTime.Now;

            JobCubeHelper jobHelper = new JobCubeHelper();
            int jobNo = 0;
            string jobStamp = null;
            StringBuilder jobLog = new StringBuilder();

            try
            {
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 開始", DateTime.Now, appName).AppendLine();

                #region 處理參數
                string method = null;           //執行 FTP 指令 (get/put)
                string remote_path = null;      //遠端路徑
                string remote_file = null;      //遠端檔案
                string local_path = null;       //本地端路徑
                string local_file = null;       //本地端檔案
                string bak_path = null;         //本地端備份檔路徑

                DateTime? lastDate = null;      //限制下載檔案的最小日期 (為了超商檔案，超商檔案的檔名都一樣，因為抓了不能砍掉，所以必須判斷檔案日期)
                bool needBak = false;           //是否備份檔案

                FTPUtility ftpHelper = null;    //FTP Utility 工具

                if (exitCode == 0)
                {
                    #region 參數說明
                    //host: FTP Server IP (必要參數，格式：xxx.xxx.xxx.xxx)
                    //port: FTP Server Port (非必要參數，預設 21)
                    //uid: FTP 帳號 (必要參數)
                    //ｐｗｄ: FTP 密碼 (必要參數) 註：實際參數是半形英文，因為 checkmarx 過敏只好用全形做說明
                    //ssl: 是否使用 SSL 協定 (非必要參數，true/false 預設 false)
                    //codepage: FTP Server 編碼 (非必要參數，預設 950)
                    //method: 執行 FTP 指令 (必要參數 get/put)
                    //remote_path: FTP Server 端路徑 (get 時為必要參數)
                    //remote_file: FTP Server 端檔案 (get 時為必要參數，* 表示 remote_path 下所有檔案)
                    //local_path: 本地端路徑 (必要參數)
                    //local_file: 本地端檔案 (put 時為必要參數)
                    //last_date: 限制下載檔案的最小日期 (非必要參數，get 時有效，格式：yyyymmdd)
                    //bak: 是否備份檔案 (非必要參數，put 時有效 true/false 預設 false)(備份檔案使用檔名)
                    //move: 是否更名備份檔案 (非必要參數，put 且 bak 為 false 時有效，預設 false)(備份檔案會在原檔名後面串上 ".yyyyMMddHHmmss")
                    //service_id: 服務代碼 (非必要參數，識別用)
                    //retry_times: re-try 次數 (非必要參數，0 ~ 8 預設 5，小於0視為0大於8視為8)
                    //retry_sleep: re-try 延遲時間，單位分鐘 (非必要參數，1 ~ 60 預設 15，小於1視為1大於60視為60)
                    #endregion

                    log.AppendFormat("命令參數：{0}", (args == null || args.Length == 0) ? null : String.Join(" ", args)).AppendLine();

                    string errmsg = null;

                    #region [MDY:20210401] 原碼修正
                    string host = "";           //FTP 伺服器
                    string port = "21";         //FTP 埠號
                    string uid = "";            //FTP 帳號
                    string pword = "";          //FTP 密碼
                    bool useSSL = false;        //是否使用 SSL 協定
                    int codepage = 950;         //FTP 編碼

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
                                    case "host":
                                        #region host (FTP 伺服器)
                                        host = value.ToLower();
                                        break;
                                        #endregion
                                    case "port":
                                        #region port (FTP 埠號)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            int val = 0;
                                            if (Int32.TryParse(value, out val) && val > 0)
                                            {
                                                port = val.ToString();
                                            }
                                            else
                                            {
                                                errmsg = "port 參數值不正確";
                                            }
                                        }
                                        break;
                                        #endregion
                                    case "uid":
                                        #region uid (FTP 帳號)
                                        uid = value;
                                        break;
                                        #endregion
                                    case "pwd":
                                        #region pwd (FTP 密碼)
                                        pword = value;
                                        break;
                                        #endregion
                                    case "ssl":
                                        #region useSSL (是否使用 SSL 協定)
                                        useSSL = value.Equals("true", StringComparison.CurrentCultureIgnoreCase);
                                        break;
                                        #endregion
                                    case "codepage":
                                        #region codepage (FTP 編碼)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            int val = 0;
                                            if (Int32.TryParse(value, out val) && val > 0)
                                            {
                                                codepage = val;
                                            }
                                            else
                                            {
                                                errmsg = "codepage 參數值不正確";
                                            }
                                        }
                                        break;
                                        #endregion

                                    case "method":
                                        #region method (執行 FTP 指令，put 或 get)
                                        method = value.ToLower();
                                        if (method != "get" && method != "put")
                                        {
                                            errmsg = "method 參數值不正確 (put 或 get)";
                                        }
                                        break;
                                        #endregion
                                    case "remote_path":
                                        #region remote_path (遠端路徑)
                                        remote_path = value;
                                        if (!String.IsNullOrEmpty(remote_path) && !remote_path.StartsWith(@"/"))
                                        {
                                            errmsg = "remote_path 參數值不正確 (必須是相對路徑)";
                                        }
                                        break;
                                        #endregion
                                    case "remote_file":
                                        #region remote_file (遠端檔案)
                                        remote_file = value;
                                        break;
                                        #endregion
                                    case "local_path":
                                        #region local_path (本地端路徑)
                                        local_path = value;
                                        if (!String.IsNullOrEmpty(local_path))
                                        {
                                            try
                                            {
                                                if (!Path.IsPathRooted(local_path))
                                                {
                                                    errmsg = "local_path 參數值不正確 (必須是絕對路徑)";
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                errmsg = "local_path 參數值不是合法的路徑";
                                            }
                                        }
                                        break;
                                        #endregion
                                    case "local_file":
                                        #region local_file (本地端檔案)
                                        local_file = value;
                                        break;
                                        #endregion

                                    case "last_date":
                                        #region lastDate (限制下載檔案的最小日期)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            switch (value.ToUpper())
                                            {
                                                case "TODAY":
                                                    lastDate = startTime.Date;
                                                    break;
                                                case "YESTERDAY":
                                                    lastDate = startTime.Date.AddDays(-1);
                                                    break;
                                                default:
                                                    lastDate = DataFormat.ConvertDateText(value);
                                                    if (lastDate == null)
                                                    {
                                                        errmsg = "last_date 參數值不是有效的日期格式 (TODAY, YESTERDAY 或 yyyyMMdd 格式)";
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                        #endregion
                                    case "bak":
                                        #region needBak (是否備份檔案)
                                        needBak = value.Equals("true", StringComparison.CurrentCultureIgnoreCase);
                                        break;
                                        #endregion

                                    case "service_id":
                                        #region jobTypeId (所屬作業類別代碼)
                                        {
                                            jobTypeId = value;
                                            jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);
                                            if (String.IsNullOrEmpty(jobTypeName))
                                            {
                                                errmsg = "service_id 參數值不是正確的作業類別代碼";
                                            }
                                        }
                                        break;
                                        #endregion
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
                    else
                    {
                        errmsg = "未指定命令參數";
                    }

                    #region 檢查必要參數
                    if (String.IsNullOrEmpty(errmsg))
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

                        if (String.IsNullOrEmpty(method))
                        {
                            lost.Add("method");
                        }

                        if (String.IsNullOrEmpty(remote_path))
                        {
                            lost.Add("remote_path");
                        }
                        else if (!remote_path.EndsWith(@"/"))
                        {
                            remote_path = remote_path + @"/";
                        }

                        if (method == "get" && String.IsNullOrEmpty(remote_file))
                        {
                            lost.Add("remote_file");
                        }

                        if (String.IsNullOrEmpty(local_path))
                        {
                            lost.Add("local_path");
                        }
                        else
                        {
                            if (!local_path.EndsWith(@"\"))
                            {
                                local_path = local_path + @"\";
                            }
                            bak_path = local_path + @"bak\";
                        }

                        if (method == "put" && String.IsNullOrEmpty(local_file))
                        {
                            lost.Add("local_file");
                        }

                        if (String.IsNullOrEmpty(jobTypeId))
                        {
                            lost.Add("service_id");
                        }

                        if (lost.Count > 0)
                        {
                            errmsg = String.Format("缺少 {0} 參數", String.Join(", ", lost));
                        }
                    }

                    if (String.IsNullOrEmpty(errmsg) && method == "put" && needBak)
                    {
                        errmsg = CheckFolder(bak_path);
                    }
                    #endregion

                    if (String.IsNullOrEmpty(errmsg))
                    {
                        ftpHelper = new FTPUtility();
                        ftpHelper.Hostname = host;
                        ftpHelper.Port = port;
                        ftpHelper.Username = uid;
                        ftpHelper.Password = pword;
                        ftpHelper.CurrentFolder = remote_path;
                        ftpHelper.EnableSSL = useSSL;
                        ftpHelper.CodePage = codepage;
                    }
                    else
                    {
                        exitCode = -1;
                        exitMsg = String.Format("參數錯誤，錯誤訊息：{0}", errmsg);
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);

                        if (args == null || args.Length == 0)
                        {
                            log.AppendLine("參數語法：host=FTP伺服器 [port=FTP埠號(預設21)] [ssl=是否使用SSL協定(true/false，預設false)] [codepage=FTP編碼(預設950)] uid=FTP帳號 pwd=FTP密碼 method=執行FTP指令(get/put) remote_path=遠端路徑 [remote_file=遠端檔案(get時必須指定)] local_path=本地端路徑 [local_file=本地端檔案(put時必須指定)] [bak=上傳檔案是否備份(true/false)] [last_date=限制下載檔案的最小日期(TODAY, YESTERDAY 或 yyyyMMdd)] service_id=所屬作業類別代碼(F80、F81、F82、F83、F85、F87) [retry_times=重試次數(0~8，預設5)] [retry_sleep=重試間隔，單位分鐘(0~60，預設15)]");
                        }
                    }
                    #endregion
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

                #region 作業處理
                if (exitCode == 0)
                {
                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 作業處理開始", DateTime.Now, jobTypeName).AppendLine();

                    #region 執行 FTP
                    switch (method)
                    {
                        case "get":
                            #region 下載檔案
                            {
                                #region 讀取檔案清單
                                List<string> remoteFiles = null;
                                {
                                    string errmsg = GetFTPFileList(ftpHelper, remote_file, lastDate, retryTimes, retrySleep, ref log, out remoteFiles);
                                    if (!String.IsNullOrWhiteSpace(errmsg))
                                    {
                                        exitCode = -3;
                                        exitMsg = errmsg;
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 讀取遠端檔案清單失敗，錯誤訊息：{1}", DateTime.Now, errmsg).AppendLine();
                                    }
                                }
                                #endregion

                                #region 下載指定檔案
                                if (exitCode == 0)
                                {
                                    string errmsg = null;
                                    if (remoteFiles != null && remoteFiles.Count > 0)
                                    {
                                        List<string> failFiles = null;
                                        errmsg = DownloadFTPFiles(ftpHelper, remoteFiles, local_path, tempPath, retryTimes, retrySleep, ref log, out failFiles);
                                    }
                                    else
                                    {
                                        errmsg = "遠端伺服器上無任何要下載的檔案";
                                    }

                                    if (!String.IsNullOrWhiteSpace(errmsg))
                                    {
                                        exitCode = -3;
                                        exitMsg = errmsg;
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 下載遠端檔案失敗，錯誤訊息：{1}", DateTime.Now, errmsg).AppendLine();
                                    }

                                    if (exitCode == 0)
                                    {
                                        exitMsg = String.Format("下載遠端檔案 ({0}) 成功", String.Join(", ", remoteFiles.ToArray()));
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                    }
                                }
                                #endregion
                            }
                            #endregion
                            break;
                        case "put":
                            #region 上傳檔案
                            {
                                #region 讀取檔案清單
                                List<string> localFiles = null;
                                {
                                    string errmsg = GetLocalFileList(local_path, local_file, retryTimes, retrySleep, ref log, out localFiles);
                                    if (!String.IsNullOrWhiteSpace(errmsg))
                                    {
                                        exitCode = -4;
                                        exitMsg = errmsg;
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 讀取本地檔案清單失敗，錯誤訊息：{1}", DateTime.Now, errmsg).AppendLine();
                                    }
                                }
                                #endregion

                                #region 上傳指定檔案
                                if (exitCode == 0)
                                {
                                    string errmsg = null;
                                    if (localFiles != null && localFiles.Count > 0)
                                    {
                                        List<string> failFiles = null;
                                        errmsg = UploadFTPFiles(ftpHelper, local_path, localFiles, needBak, bak_path, retryTimes, retrySleep, ref log, out failFiles);
                                    }
                                    else
                                    {
                                        errmsg = "本地端上無任何要上載的檔案";
                                    }

                                    if (!String.IsNullOrWhiteSpace(errmsg))
                                    {
                                        exitCode = -4;
                                        exitMsg = errmsg;
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 上傳本地檔案失敗，錯誤訊息：{1}", DateTime.Now, errmsg).AppendLine();
                                    }

                                    if (exitCode == 0)
                                    {
                                        exitMsg = String.Format("上傳本地檔案 ({0}) 成功", String.Join(", ", localFiles.ToArray()));
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                    }
                                }
                                #endregion
                            }
                            #endregion
                            break;
                        default:
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 參數錯誤，參數：{1}", DateTime.Now, String.Join(" ", args));
                            break;
                    }
                    #endregion

                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 作業處理結束", DateTime.Now, jobTypeName).AppendLine();
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
