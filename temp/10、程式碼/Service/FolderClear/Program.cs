using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#region
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
#endregion

#region
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Reflection;
//using System.Xml;
#endregion

namespace FolderClear
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0].Equals("/Setting", StringComparison.CurrentCultureIgnoreCase))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmSettingList());
            }
            else
            {
                Helper.Run(args);
            }
        }
    }

    #region
    //class Program2
    //{
    //    static void Main(string[] args)
    //    {
    //        #region Initial
    //        //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;    //這個是組件的名稱
    //        Assembly myAssembly = Assembly.GetExecutingAssembly();
    //        string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
    //        string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);     //這個是執行檔的名稱 (去掉副檔名)

    //        DateTime runTime = DateTime.Now;
    //        FileLoger fileLog = null;
    //        int exitCode = 0;
    //        string errmsg = null;
    //        #endregion

    //        try
    //        {
    //            string logPath = null;
    //            List<Setting> settings = Setting.LoadBySettingFile(out logPath, out errmsg);
    //            fileLog = new FileLoger(appName, logPath, runTime);
    //            if (String.IsNullOrEmpty(errmsg))
    //            {
    //                using (Mutex m = new Mutex(false, "Global\\" + appGuid))    //全域不可重複執行
    //                {
    //                    //檢查是否同名Mutex已存在(表示另一份程式正在執行)
    //                    if (m.WaitOne(0, false))
    //                    {
    //                        if (settings != null && settings.Count > 0)
    //                        {
    //                            #region 逐設定刪除檔案
    //                            foreach (Setting setting in settings)
    //                            {
    //                                if (!setting.IsReady())
    //                                {
    //                                    continue;
    //                                }

    //                                StringBuilder log = new StringBuilder();
    //                                try
    //                                {
    //                                    DateTime chkTime = runTime.AddDays(setting.KeepDays * -1).Date;

    //                                    log.AppendFormat("[{0:HH:mm:ss}] 開始處理 {1} 資料夾，保留最小檔案日期 {2:yyyy/MM/dd}：", DateTime.Now, setting.FolderPath, chkTime).AppendLine();

    //                                    DirectoryInfo dir = new DirectoryInfo(setting.FolderPath);
    //                                    if (dir.Exists)
    //                                    {
    //                                        #region 處理資料夾下指定的檔案
    //                                        if (!String.IsNullOrEmpty(setting.FileFilter))
    //                                        {
    //                                            string log2 = ClearFolder(dir, setting.FileFilter, chkTime, false);
    //                                            log.AppendLine(log2);
    //                                        }
    //                                        #endregion

    //                                        #region 處理資料夾下指定的子資料夾
    //                                        if (!String.IsNullOrEmpty(setting.SubFolderFilter))
    //                                        {
    //                                            Regex reg = setting.SubFolderRegex;
    //                                            string searchPattern = reg != null ? "*" : setting.SubFolderFilter;
    //                                            SearchOption searchOption = SearchOption.TopDirectoryOnly;

    //                                            log.AppendFormat("處理 {0} 下符合 {1} 的子資料夾：", dir.Name, setting.SubFolderFilter).AppendLine();
    //                                            try
    //                                            {
    //                                                DirectoryInfo[] subDirs = dir.GetDirectories(searchPattern, searchOption);
    //                                                if (subDirs != null && subDirs.Length > 0)
    //                                                {
    //                                                    foreach (DirectoryInfo subDir in subDirs)
    //                                                    {
    //                                                        #region 檢查是否符合正規表示式條件
    //                                                        if (reg != null)
    //                                                        {
    //                                                            if (!reg.IsMatch(subDir.Name))
    //                                                            {
    //                                                                continue;
    //                                                            }
    //                                                        }
    //                                                        #endregion

    //                                                        #region 刪除子資料夾下指定的檔案
    //                                                        if (!String.IsNullOrEmpty(setting.SubFileFilter))
    //                                                        {
    //                                                            string log2 = ClearFolder(subDir, setting.SubFileFilter, chkTime, true);
    //                                                            log.AppendLine(log2);
    //                                                        }
    //                                                        #endregion
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    log.AppendLine("找不到符合的子資料夾");
    //                                                }
    //                                            }
    //                                            catch (Exception ex)
    //                                            {
    //                                                log.AppendFormat("發生例外，{0}", ex.Message).AppendLine();
    //                                            }
    //                                        }
    //                                        #endregion
    //                                    }
    //                                    else
    //                                    {
    //                                        log.AppendLine("資料夾不存在");
    //                                    }
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    log.AppendFormat("發生例外，{0}", ex.Message).AppendLine();
    //                                }

    //                                fileLog.WriteLog(log);

    //                                #region 每換一個設定檔 sleep 1 秒避免硬碟太忙
    //                                Thread.Sleep(1000);
    //                                #endregion
    //                            }
    //                            #endregion
    //                        }
    //                        else
    //                        {
    //                            exitCode = -2;
    //                            errmsg = "讀取設定檔失敗，錯誤訊息：設定檔無資料";
    //                        }
    //                    }
    //                    else
    //                    {
    //                        exitCode = -3;
    //                        errmsg = "此程式已在執行中，不可重複執行";
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                exitCode = -2;
    //                errmsg = String.Format("讀取設定檔失敗，錯誤訊息：{0}", errmsg);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            exitCode = -1;
    //            errmsg = String.Format("執行 {0} 發生例外，錯誤訊息：{1}", appName, ex.Message);
    //        }

    //        if (fileLog != null)
    //        {
    //            if (String.IsNullOrEmpty(errmsg))
    //            {
    //                fileLog.WriteLog("執行結束\r\n");
    //            }
    //            else
    //            {
    //                fileLog.WriteLog(errmsg + "\r\n");
    //            }
    //        }

    //        System.Environment.Exit(exitCode);
    //        return;
    //    }

    //    /// <summary>
    //    /// 清除指定資料頰下指定的檔案
    //    /// </summary>
    //    /// <param name="folder">指定資料夾</param>
    //    /// <param name="searchPattern">指定過濾的檔名 Pattern</param>
    //    /// <param name="chkTime">指定檢查檔案時間</param>
    //    /// <param name="deleteEmptyFolder">指定是否刪除空的資料夾</param>
    //    /// <returns></returns>
    //    private static string ClearFolder(DirectoryInfo folder, string searchPattern, DateTime chkTime, bool deleteEmptyFolder = false)
    //    {
    //        StringBuilder log = new StringBuilder();

    //        if (folder != null &&　folder.Exists && !String.IsNullOrEmpty(searchPattern))
    //        {
    //            SearchOption searchOption = SearchOption.TopDirectoryOnly;

    //            int totalCount = 0;     //檔名符合的數量
    //            int skipCount = 0;      //時間不符的數量
    //            int okCount = 0;    //刪除成功的數量
    //            int failCount = 0;      //刪除失敗的數量
    //            log.AppendFormat("刪除 {0} 下符合 {1} 的檔案：", folder.Name, searchPattern).AppendLine();

    //            try
    //            {
    //                FileInfo[] fInfos = null;
    //                if (searchPattern == "*" || searchPattern == "*.*")
    //                {
    //                    fInfos = folder.GetFiles();
    //                }
    //                else
    //                {
    //                    fInfos = folder.GetFiles(searchPattern, searchOption);
    //                }
    //                if (fInfos != null && fInfos.Length > 0)
    //                {
    //                    totalCount = fInfos.Length;
    //                    int count = 0;
    //                    foreach (FileInfo fInfo in fInfos)
    //                    {
    //                        try
    //                        {
    //                            if (fInfo.LastWriteTime < chkTime)
    //                            {
    //                                fInfo.Delete();
    //                                okCount++;
    //                            }
    //                            else
    //                            {
    //                                skipCount++;
    //                            }
    //                        }
    //                        catch (Exception)
    //                        {
    //                            failCount++;
    //                        }

    //                        #region 每刪 50 個檔 sleep 0.1 秒避免硬碟太忙
    //                        if (count >= 50)
    //                        {
    //                            count = 1;
    //                            Thread.Sleep(100);
    //                        }
    //                        #endregion
    //                    }
    //                }

    //                #region 刪除空的資料夾
    //                if (deleteEmptyFolder && failCount == 0)
    //                {
    //                    fInfos = folder.GetFiles();
    //                    if (fInfos == null || fInfos.Length == 0)
    //                    {
    //                        try
    //                        {
    //                            folder.Delete();
    //                            log.AppendLine("移除本資料夾");
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            log.AppendFormat("刪除空資料夾 {0} 失敗，{1}", folder.FullName, ex.Message).AppendLine();
    //                        }
    //                    }
    //                }
    //                #endregion
    //            }
    //            catch (Exception ex)
    //            {
    //                log.AppendFormat("發生例外，{0}", ex.Message).AppendLine();
    //            }

    //            log.AppendFormat("處理結果：找到 {0} 個檔案，時間不符 {1} 檔案，刪除成功 {2} 個檔案，刪除失敗 {3} 的檔案", totalCount, skipCount, okCount, failCount).AppendLine();
    //        }

    //        return log.ToString();
    //    }

    //    #region 日誌檔類別
    //    /// <summary>
    //    /// 日誌檔類別
    //    /// </summary>
    //    private class FileLoger
    //    {
    //        #region Property
    //        private string _LogName = null;
    //        /// <summary>
    //        /// 日誌檔主要名稱 (App 名稱)
    //        /// </summary>
    //        public string LogName
    //        {
    //            get
    //            {
    //                return _LogName;
    //            }
    //            private set
    //            {
    //                _LogName = value == null ? String.Empty : value.Trim();
    //            }
    //        }

    //        private string _LogPath = null;
    //        /// <summary>
    //        /// 日誌檔路徑
    //        /// </summary>
    //        public string LogPath
    //        {
    //            get
    //            {
    //                return _LogPath;
    //            }
    //            private set
    //            {
    //                _LogPath = value == null ? String.Empty : value.Trim();
    //            }
    //        }

    //        /// <summary>
    //        /// 日誌檔完整路徑名稱 (日誌檔路徑 / 主要名稱 + "_" + 當天日期(yyyyMMdd) + ".log")
    //        /// </summary>
    //        private string _LogFileName = null;
    //        public string LogFileName
    //        {
    //            get
    //            {
    //                return _LogFileName;
    //            }
    //            private set
    //            {
    //                _LogFileName = value == null ? String.Empty : value.Trim();
    //            }
    //        }
    //        #endregion

    //        #region Constructor
    //        /// <summary>
    //        /// 建構 日誌檔類別 物件
    //        /// </summary>
    //        /// <param name="logName"></param>
    //        /// <param name="logPath"></param>
    //        /// <param name="isDebug"></param>
    //        public FileLoger(string logName, string logPath, DateTime timeStamp)
    //        {
    //            this.LogName = logName;
    //            this.LogPath = logPath;
    //            this.Initial(timeStamp);
    //        }
    //        #endregion

    //        #region Initial
    //        /// <summary>
    //        /// 初始化
    //        /// </summary>
    //        /// <param name="timeStamp"></param>
    //        /// <returns></returns>
    //        private string Initial(DateTime timeStamp)
    //        {
    //            if (!String.IsNullOrEmpty(this.LogPath))
    //            {
    //                try
    //                {
    //                    DirectoryInfo info = new DirectoryInfo(this.LogPath);
    //                    if (!info.Exists)
    //                    {
    //                        info.Create();
    //                    }
    //                    if (String.IsNullOrEmpty(this.LogName))
    //                    {
    //                        string fileName = String.Format("{0:yyyyMMdd}.log", timeStamp);
    //                        _LogFileName = Path.Combine(info.FullName, fileName);
    //                    }
    //                    else
    //                    {
    //                        string fileName = String.Format("{0}_{1:yyyyMMdd}.log", this.LogName, timeStamp);
    //                        _LogFileName = Path.Combine(info.FullName, fileName);
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    return ex.Message;
    //                }
    //            }
    //            return null;
    //        }
    //        #endregion

    //        #region WriteLog 相關
    //        /// <summary>
    //        /// 寫入日誌訊息
    //        /// </summary>
    //        /// <param name="msg">日誌訊息</param>
    //        /// <param name="timeInfo">是否在訊息前面紀錄時間資訊</param>
    //        public void WriteLog(string msg, bool timeInfo = true)
    //        {
    //            if (!String.IsNullOrEmpty(_LogFileName) && msg != null)
    //            {
    //                try
    //                {
    //                    using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
    //                    {
    //                        if (String.IsNullOrEmpty(msg))
    //                        {
    //                            sw.WriteLine(String.Empty);
    //                        }
    //                        else
    //                        {
    //                            if (timeInfo)
    //                            {
    //                                sw.WriteLine("[{0:HH:mm:ss}] {1}", DateTime.Now, msg);
    //                            }
    //                            else
    //                            {
    //                                sw.WriteLine(msg);
    //                            }
    //                        }
    //                    }
    //                }
    //                catch (Exception)
    //                {
    //                }
    //            }
    //        }

    //        /// <summary>
    //        /// 寫入日誌訊息
    //        /// </summary>
    //        /// <param name="msg"></param>
    //        public void WriteLog(StringBuilder msg)
    //        {
    //            if (!String.IsNullOrEmpty(_LogFileName) && msg != null && msg.Length > 0)
    //            {
    //                try
    //                {
    //                    using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
    //                    {
    //                        sw.WriteLine(msg);
    //                    }
    //                }
    //                catch (Exception)
    //                {
    //                }
    //            }
    //        }
    //        #endregion
    //    }
    //    #endregion

    //    #region 設定資料承載類別
    //    /// <summary>
    //    /// 設定資料承載類別
    //    /// </summary>
    //    private class Setting
    //    {
    //        #region Property
    //        private string _FolderPath = null;
    //        /// <summary>
    //        /// 資料夾路徑 (預設 null)
    //        /// </summary>
    //        public string FolderPath
    //        {
    //            get
    //            {
    //                return _FolderPath;
    //            }
    //            private set
    //            {
    //                _FolderPath = value == null ? null : value.Trim();
    //            }
    //        }

    //        private string _FileFilter = null;
    //        /// <summary>
    //        /// 檔名過濾字串 (預設 null)
    //        /// </summary>
    //        public string FileFilter
    //        {
    //            get
    //            {
    //                return _FileFilter;
    //            }
    //            private set
    //            {
    //                _FileFilter = value == null ? null : value.Trim();
    //            }
    //        }

    //        private string _SubFolderFilter = null;
    //        /// <summary>
    //        /// 子資料夾過濾字串 (預設 null)
    //        /// </summary>
    //        public string SubFolderFilter
    //        {
    //            get
    //            {
    //                return _SubFolderFilter;
    //            }
    //            private set
    //            {
    //                _SubFolderFilter = value == null ? null : value.Trim();
    //            }
    //        }

    //        private string _SubFileFilter = null;
    //        /// <summary>
    //        /// 子資料夾的檔名過濾字串 (預設 null)
    //        /// </summary>
    //        public string SubFileFilter
    //        {
    //            get
    //            {
    //                return _SubFileFilter;
    //            }
    //            private set
    //            {
    //                _SubFileFilter = value == null ? null : value.Trim();
    //            }
    //        }

    //        private int _KeepDays = 30;
    //        /// <summary>
    //        /// 保留檔案天數 (允許值 1 ~ 3650，預設 30)
    //        /// </summary>
    //        public int KeepDays
    //        {
    //            get
    //            {
    //                return _KeepDays;
    //            }
    //            private set
    //            {
    //                if (value < 1)
    //                {
    //                    _KeepDays = 1;
    //                }
    //                else if (value > 3650)
    //                {
    //                    _KeepDays = 3650;
    //                }
    //                else
    //                {
    //                    _KeepDays = value;
    //                }
    //            }
    //        }

    //        private Regex _SubFolderRegex = null;
    //        /// <summary>
    //        /// 子資料夾過濾正規表示式
    //        /// </summary>
    //        public Regex SubFolderRegex
    //        {
    //            get
    //            {
    //                return _SubFolderRegex;
    //            }
    //            private set
    //            {
    //                _SubFolderRegex = value;
    //            }
    //        }
    //        #endregion

    //        #region Constructor
    //        private Setting()
    //        {
    //        }
    //        #endregion

    //        #region Method
    //        /// <summary>
    //        /// 取得設定是否準備好
    //        /// </summary>
    //        /// <returns></returns>
    //        public bool IsReady()
    //        {
    //            if (!String.IsNullOrEmpty(this.FolderPath))
    //            {
    //                if (!String.IsNullOrEmpty(this.FileFilter))
    //                {
    //                    return true;
    //                }
    //                if (!String.IsNullOrEmpty(this.SubFolderFilter) && !String.IsNullOrEmpty(this.SubFileFilter))
    //                {
    //                    return true;
    //                }
    //            }
    //            return false;
    //        }
    //        #endregion

    //        #region Static Method
    //        /// <summary>
    //        /// 載入設定檔
    //        /// </summary>
    //        /// <param name="logPath"></param>
    //        /// <param name="errmsg">傳回錯誤訊息或 null</param>
    //        /// <returns>傳回設定資料承載物件集合</returns>
    //        public static List<Setting> LoadBySettingFile(out string logPath, out string errmsg)
    //        {
    //            logPath = null;
    //            errmsg = null;
    //            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
    //            string fileName = Path.Combine(filePath, "Setting.xml");
    //            List<Setting> settings = null;

    //            try
    //            {
    //                if (File.Exists(fileName))
    //                {
    //                    string xml = File.ReadAllText(fileName, Encoding.Default);
    //                    XmlDocument xDoc = new XmlDocument();
    //                    xDoc.LoadXml(xml);

    //                    #region logPath
    //                    {
    //                        XmlNode xNode = xDoc.SelectSingleNode("/settings/logPath");
    //                        if (xNode != null)
    //                        {
    //                            if (xNode.NodeType != XmlNodeType.Element)
    //                            {
    //                                errmsg = "logPath 節點設定不正確";
    //                            }
    //                            else if (xNode.InnerText != null)
    //                            {
    //                                logPath = xNode.InnerText.Trim();
    //                                if (!Path.IsPathRooted(logPath))
    //                                {
    //                                    errmsg = "logPath 設定值不正確 (必須為絕對路徑)";
    //                                }
    //                            }
    //                        }
    //                    }
    //                    #endregion

    //                    #region setting
    //                    if (String.IsNullOrEmpty(errmsg))
    //                    {
    //                        XmlNodeList xNodes = xDoc.SelectNodes("/settings/setting");
    //                        if (xNodes == null || xNodes.Count == 0)
    //                        {
    //                            errmsg = "找不到任何設定節點 (setting)";
    //                        }
    //                        else
    //                        {
    //                            string regPrefix = "[REGEX:";
    //                            string regSuffix = "]";

    //                            settings = new List<Setting>(xNodes.Count);
    //                            int no = 0;
    //                            foreach (XmlNode xNode in xNodes)
    //                            {
    //                                no++;
    //                                Setting setting = new Setting();

    //                                #region folder_path (資料夾路徑)
    //                                if (String.IsNullOrEmpty(errmsg))
    //                                {
    //                                    XmlNode xMyNode = xNode.SelectSingleNode("folder_path");
    //                                    if (xMyNode == null || xMyNode.NodeType != XmlNodeType.Element)
    //                                    {
    //                                        errmsg = String.Format("第{0}個設定節點缺少 folder_path 節點或設定不正確", no);
    //                                    }
    //                                    else
    //                                    {
    //                                        setting.FolderPath = xMyNode.InnerText;
    //                                        if (!Path.IsPathRooted(setting.FolderPath))
    //                                        {
    //                                            errmsg = String.Format("第{0}個設定節點的 folder_path 設定值不正確 (必須為絕對路徑)", no);
    //                                        }
    //                                    }
    //                                }
    //                                #endregion

    //                                #region file_filter (檔名過濾字串)
    //                                if (String.IsNullOrEmpty(errmsg))
    //                                {
    //                                    XmlNode xMyNode = xNode.SelectSingleNode("file_filter");
    //                                    if (xMyNode != null)
    //                                    {
    //                                        if (xMyNode.NodeType != XmlNodeType.Element)
    //                                        {
    //                                            errmsg = String.Format("第{0}個設定節點的 file_filter 設定不正確", no);
    //                                        }
    //                                        else
    //                                        {
    //                                            setting.FileFilter = xMyNode.InnerText;
    //                                        }
    //                                    }
    //                                }
    //                                #endregion

    //                                #region sub_folder_filter (子資料夾過濾字串)
    //                                if (String.IsNullOrEmpty(errmsg))
    //                                {
    //                                    XmlNode xMyNode = xNode.SelectSingleNode("sub_folder_filter");
    //                                    if (xMyNode != null)
    //                                    {
    //                                        if (xMyNode.NodeType != XmlNodeType.Element)
    //                                        {
    //                                            errmsg = String.Format("第{0}個設定節點的 sub_folder_filter 設定不正確", no);
    //                                        }
    //                                        else
    //                                        {
    //                                            setting.SubFolderFilter = xMyNode.InnerText;

    //                                            #region 處理 sub_folder_filter 的正規表示式
    //                                            if (!String.IsNullOrEmpty(setting.SubFolderFilter) && setting.SubFolderFilter.StartsWith(regPrefix) && setting.SubFolderFilter.EndsWith(regSuffix))
    //                                            {
    //                                                try
    //                                                {
    //                                                    string pattern = setting.SubFolderFilter.Substring(regPrefix.Length, setting.SubFolderFilter.Length - regPrefix.Length - 1).Trim();
    //                                                    if (pattern.Length > 0)
    //                                                    {
    //                                                        setting.SubFolderRegex = new Regex(pattern, RegexOptions.Compiled);
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        errmsg = String.Format("第{0}個設定節點的 sub_folder_filter 設定值不正確 (缺少正規表示式 pattern)", no);
    //                                                    }
    //                                                }
    //                                                catch
    //                                                {
    //                                                    errmsg = String.Format("第{0}個設定節點的 sub_folder_filter 設定值不正確 (正規表示式 pattern 不正確)", no);
    //                                                }
    //                                            }
    //                                            #endregion
    //                                        }
    //                                    }
    //                                }
    //                                #endregion

    //                                #region 檢查檔名過濾字串 與 子資料夾過濾字串 不可都沒值
    //                                if (String.IsNullOrEmpty(errmsg))
    //                                {
    //                                    if (String.IsNullOrEmpty(setting.FileFilter) && String.IsNullOrEmpty(setting.SubFolderFilter))
    //                                    {
    //                                        errmsg = String.Format("第{0}個設定節點的設定無效 (file_filter 與 sub_folder_filter 至少要一個有值)", no);
    //                                    }
    //                                }
    //                                #endregion

    //                                #region sub_file_filter (子資料夾的檔名過濾字串)
    //                                if (String.IsNullOrEmpty(errmsg))
    //                                {
    //                                    XmlNode xMyNode = xNode.SelectSingleNode("sub_file_filter");
    //                                    if (xMyNode != null)
    //                                    {
    //                                        if (xMyNode.NodeType != XmlNodeType.Element)
    //                                        {
    //                                            errmsg = String.Format("第{0}個設定節點的 sub_file_filter 設定不正確", no);
    //                                        }
    //                                        else
    //                                        {
    //                                            setting.SubFileFilter = xMyNode.InnerText;
    //                                        }
    //                                    }
    //                                }
    //                                #endregion

    //                                #region 檢查 子資料夾過濾字串 與 子資料夾的檔名過濾字串 必須同時有值或同時沒值
    //                                if (String.IsNullOrEmpty(errmsg))
    //                                {
    //                                    if ((String.IsNullOrEmpty(setting.SubFolderFilter) && !String.IsNullOrEmpty(setting.SubFileFilter))
    //                                        || (!String.IsNullOrEmpty(setting.SubFolderFilter) && String.IsNullOrEmpty(setting.SubFileFilter)))
    //                                    {
    //                                        errmsg = String.Format("第{0}個設定節點的設定無效 (sub_folder_filter 與 sub_file_filter 必須成對設定)", no);
    //                                    }
    //                                }
    //                                #endregion

    //                                #region keep_days (保留檔案天數)
    //                                if (String.IsNullOrEmpty(errmsg))
    //                                {
    //                                    XmlNode xMyNode = xNode.SelectSingleNode("keep_days");
    //                                    if (xMyNode != null)
    //                                    {
    //                                        int keepDays = 0;
    //                                        if (xMyNode.NodeType != XmlNodeType.Element)
    //                                        {
    //                                            errmsg = String.Format("第{0}個設定節點的 keep_days 設定不正確", no);
    //                                        }
    //                                        else if (!Int32.TryParse(xMyNode.InnerText, out keepDays) || keepDays < 1 || keepDays > 3650)
    //                                        {
    //                                            errmsg = String.Format("第{0}個設定節點的 keep_days 設定值不正確 (必須為 1 ~ 3650 的整值)", no);
    //                                        }
    //                                        else
    //                                        {
    //                                            setting.KeepDays = keepDays;
    //                                        }
    //                                    }
    //                                }
    //                                #endregion

    //                                if (String.IsNullOrEmpty(errmsg))
    //                                {
    //                                    settings.Add(setting);
    //                                }
    //                                else
    //                                {
    //                                    break;
    //                                }
    //                            }
    //                        }
    //                    }
    //                    #endregion
    //                }
    //                else
    //                {
    //                    errmsg = String.Format("找不到設定檔 ({0})", fileName);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                errmsg = String.Format("載入設定檔失敗，錯誤訊息：{0}", ex.Message);
    //            }

    //            return settings ?? new List<Setting>(0);
    //        }
    //        #endregion

    //    }
    //    #endregion
    //}
    #endregion
}
