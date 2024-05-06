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

namespace InboundTransfer
{
    /// <summary>
    /// 下載、轉換財金支付寶清算檔成土銀清算檔後上傳 (INBT)
    /// </summary>
    class Program
    {
        public static Encoding _Encoding = Encoding.Default;
        public static string _LogPath = null;
        public static string _LogFileName = null;

        /// <summary>
        /// 轉換財金支付寶檔案為中心要的檔案，因為是轉檔，所以只要有錯就全部不處理，然後發信件(目前不單獨發信)，不會產生檔案。如果是正確，沒有資料就產生空檔
        /// 處理原則
        /// 拆解檔案只要有錯，就全部不處理
        /// 拆解檔案成功，寫檔時如果訂單編號無法找到原始的交易紀錄僅寫入日誌檔
        /// </summary>
        /// <param name="args">參數：DATE=檔名的日期(TODAY/YESTERDAY/yyyyMMdd，非必要參數，預設為當天) FTPDW=是否下載財金清算檔案(TRUE/FALSE，非必要參數，預設為TRUE) FTPUP=是否上傳土銀清算檔案(TRUE/FALSE，非必要參數，預設為TRUE)</param>
        static void Main(string[] args)
        {
            #region Initial
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string appGuid = (Attribute.GetCustomAttribute(myAssembly, typeof(GuidAttribute)) as GuidAttribute).Value;
            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);     //這個是組太的執行檔的名稱 (去掉副檔名)

            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = JobCubeTypeCodeTexts.INBT;                   //作業類別代碼
            string jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);   //作業類別名稱
            int exitCode = 0;
            string exitMsg = null;
            #endregion

            int retryTimes = 0;  //re-try 次數
            int retrySleep = 0;  //re-try 間隔，單位分鐘
            DateTime startTime = DateTime.Now;

            JobCubeHelper jobHelper = new JobCubeHelper();
            int jobNo = 0;
            string jobStamp = null;
            StringBuilder jobLog = new StringBuilder();

            #region 日誌檔初始化
            {
                _LogPath = ConfigurationManager.AppSettings["LOG_PATH"];
                if (String.IsNullOrWhiteSpace(_LogPath))
                {
                    _LogPath = Path.GetTempPath();
                }
                else
                {
                    try
                    {
                        DirectoryInfo dInfo = new DirectoryInfo(_LogPath);
                        if (!dInfo.Exists)
                        {
                            dInfo.Create();
                        }
                        _LogFileName = Path.Combine(_LogPath, String.Format("{0}_{1:yyyyMMdd}.log", appName, startTime));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            #endregion

            string filePattern = null;
            DirectoryInfo dirData = null;
            DirectoryInfo dirTarget = null;
            DirectoryInfo dirBackup = null;
            EntityFactory factory = null;

            try
            {
                Start();

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
                        exitMsg = result.Message;
                        WriteLog(result.Message);
                    }
                }
                #endregion

                #region [MDY:20170309] 土銀反悔，檔名改成 "Inbound" + 8碼民國年日期 + ".txt" 所以這個參數用不到了
                #region [Old]
                //#region 取得檔名樣式
                //string fileNamePattern = ConfigurationManager.AppSettings["FILENAME_PATTERN"];
                //if (String.IsNullOrWhiteSpace(fileNamePattern))
                //{
                //    exitCode = -1;
                //    exitMsg = "Config 未指定檔名樣式 (FILENAME_PATTERN) 參數";
                //    jobLog.AppendLine(exitMsg);
                //    WriteLog(exitMsg);
                //}
                //#endregion
                #endregion
                #endregion

                #region 取得資料檔路徑、目的路徑、備份路徑
                {
                    string dataPath = ConfigurationManager.AppSettings["DATA_PATH"];
                    if (String.IsNullOrWhiteSpace(dataPath))
                    {
                        exitCode = -1;
                        exitMsg = "Config 未指定資料檔路徑 (DATA_PATH) 參數";
                        jobLog.AppendLine(exitMsg);
                        WriteLog(exitMsg);
                    }
                    try
                    {
                        dirData = new DirectoryInfo(dataPath);
                        if (!dirData.Exists)
                        {
                            exitCode = -1;
                            exitMsg = String.Format("Config 指定的 {0} 資料檔路徑 (DATA_PATH) 不存在", dataPath);
                            jobLog.AppendLine(exitMsg);
                            WriteLog(exitMsg);
                        }
                    }
                    catch (Exception)
                    {
                        exitCode = -1;
                        exitMsg = String.Format("Config 指定的 {0} 資料檔路徑 (DATA_PATH) 不合法", dataPath);
                        jobLog.AppendLine(exitMsg);
                        WriteLog(exitMsg);
                    }

                    string targetPath = Path.Combine(dataPath, "transferred");
                    try
                    {
                        dirTarget = new DirectoryInfo(targetPath);
                        if (!dirTarget.Exists)
                        {
                            dirTarget.Create();
                        }
                    }
                    catch (Exception ex)
                    {
                        exitCode = -1;
                        exitMsg = String.Format("建立 {0} 資料夾失敗，錯誤訊息：{1}", targetPath, ex.Message);
                        jobLog.AppendLine(exitMsg);
                        WriteLog(exitMsg);
                    }

                    string backupPath = Path.Combine(dataPath, "backup");
                    try
                    {
                        dirBackup = new DirectoryInfo(backupPath);
                        if (!dirBackup.Exists)
                        {
                            dirBackup.Create();
                        }
                    }
                    catch (Exception ex)
                    {
                        exitCode = -1;
                        exitMsg = String.Format("建立 {0} 資料夾失敗，錯誤訊息：{1}", backupPath, ex.Message);
                        jobLog.AppendLine(exitMsg);
                        WriteLog(exitMsg);
                    }
                }
                #endregion

                #region [MDY:20220530] Checkmarx 調整
                #region [MDY:20210401] 原碼修正
                #region 處理參數
                DateTime? fileNameDate = null;  //檔名的日期
                bool isNeedFTPDW = true;        //是否FTP下載財金清算檔案
                bool isNeedFTPUP = true;        //是否FTP上傳土銀清算檔案

                string host = "";           //FTP 伺服器
                int port = 21;              //FTP 埠號
                string uid = "";            //FTP 帳號
                string pxx = "";            //FTP 密碼
                bool useSSL = false;        //是否使用 SSL 協定
                string remotePath = null;   //遠端路徑

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
                                case "DATE":
                                    #region 檔名的日期
                                    switch (value.ToUpper())
                                    {
                                        case "TODAY":
                                            fileNameDate = startTime.Date;
                                            break;
                                        case "YESTERDAY":
                                            fileNameDate = startTime.Date.AddDays(-1);
                                            break;
                                        default:
                                            fileNameDate = DataFormat.ConvertDateText(value);
                                            if (fileNameDate == null)
                                            {
                                                exitCode = -1;
                                                exitMsg = "DATE 參數值不是有效的日期格式 (TODAY, YESTERDAY 或 yyyyMMdd 格式)";
                                                jobLog.AppendLine(exitMsg);
                                                WriteLog(exitMsg);
                                            }
                                            break;
                                    }
                                    break;
                                    #endregion

                                case "FTPDW":
                                    #region 是否下載財金清算檔案
                                    isNeedFTPDW = (value.ToUpper() == "TRUE");
                                    break;
                                    #endregion

                                case "FTPUP":
                                    #region 是否上傳土銀清算檔案
                                    isNeedFTPUP = (value.ToUpper() == "TRUE");
                                    break;
                                    #endregion

                                #region [MDY:20181203] FTP 相關命令參數 (20181201_05)
                                case "HOST":
                                    #region HOST (FTP 伺服器)
                                    host = value.ToLower();
                                    break;
                                    #endregion
                                case "PORT":
                                    #region PORT (FTP 埠號)
                                    if (!String.IsNullOrEmpty(value))
                                    {
                                        int val = 0;
                                        if (Int32.TryParse(value, out val) && val > 0)
                                        {
                                            port = val;
                                        }
                                        else
                                        {
                                            exitCode = -1;
                                            exitMsg = "port 參數值不正確";
                                            jobLog.AppendLine(exitMsg);
                                            WriteLog(exitMsg);
                                        }
                                    }
                                    break;
                                    #endregion
                                case "UID":
                                    #region uid (FTP 帳號)
                                    uid = value;
                                    break;
                                    #endregion
                                case "PWD":
                                    #region pwd (FTP 密碼)
                                    pxx = value;
                                    break;
                                    #endregion
                                case "SSL":
                                    #region useSSL (是否使用 SSL 協定)
                                    useSSL = value.Equals("true", StringComparison.CurrentCultureIgnoreCase);
                                    break;
                                    #endregion
                                case "REMOTE_PATH":
                                    #region remote_path (遠端路徑)
                                    remotePath = value;
                                    if (!String.IsNullOrEmpty(remotePath) && !remotePath.StartsWith(@"/"))
                                    {
                                        exitCode = -1;
                                        exitMsg = "remote_path 參數值不正確 (必須是相對路徑)";
                                        jobLog.AppendLine(exitMsg);
                                        WriteLog(exitMsg);
                                    }
                                    break;
                                    #endregion
                                #endregion
                            }
                        }
                    }
                }
                #endregion

                #region 取得 FTP
                FTPUtility ftp = null;
                if (exitCode == 0)
                {
                    if (isNeedFTPDW || isNeedFTPUP)
                    {
                        #region [MDY:20181203] 改用 FTP 相關命令參數 (20181201_05)
                        #region [OLD]
                        //ftp = CreateFTPUtility();
                        #endregion

                        ftp = CreateFTPUtility(host, port, useSSL, uid, pxx, remotePath);
                        #endregion

                        if (ftp == null)
                        {
                            exitCode = -1;
                            exitMsg = "Config 未指定 FTP 相關參數或參數錯誤";
                            jobLog.AppendLine(exitMsg);
                            WriteLog(exitMsg);
                        }
                    }
                }
                #endregion

                #region 處理程序
                if (exitCode == 0)
                {
                    using (Mutex m = new Mutex(false, "Global\\" + appGuid))    //全域不可重複執行
                    {
                        //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                        if (m.WaitOne(0, false))
                        {
                            #region [MDY:20170309] 土銀反悔，檔名改成 "Inbound" + 8碼民國年日期 + ".txt"
                            #region [Old]
                            //filePattern = string.Format(fileNamePattern, GetCCMMDD(fileNameDate));
                            #endregion

                            if (fileNameDate == null)
                            {
                                fileNameDate = startTime.Date;
                            }
                            filePattern = string.Format("Inbound{0}.txt", Fuju.Common.GetTWDate8(fileNameDate.Value));
                            #endregion

                            jobLog.AppendFormat("開始處理程序，檔名樣式：{0}", filePattern).AppendLine();
                            WriteLog("開始處理程序，檔名樣式：" + filePattern);

                            #region FTP 下載
                            if (exitCode == 0)
                            {
                                if (isNeedFTPDW)
                                {
                                    StringBuilder ftpLog = new StringBuilder();
                                    List<string> remoteFiles = null;
                                    string errmsg = GetFTPFileList(ftp, filePattern, null, retryTimes, retrySleep, ref ftpLog, out remoteFiles);

                                    if (String.IsNullOrEmpty(errmsg))
                                    {
                                        if (remoteFiles != null && remoteFiles.Count > 0)
                                        {
                                            string tempPath = GetTempPath();
                                            string localPath = dirData.FullName;
                                            List<string> failFiles = null;
                                            errmsg = DownloadFTPFiles(ftp, remoteFiles, localPath, tempPath, retryTimes, retrySleep, ref ftpLog, out failFiles);
                                        }
                                    }

                                    if (String.IsNullOrEmpty(errmsg))
                                    {
                                        if (remoteFiles == null || remoteFiles.Count == 0)
                                        {
                                            string msg = "FTP 上無符合的財金清算檔案";
                                            jobLog.AppendLine(msg);
                                            WriteLog(msg);
                                        }
                                        else
                                        {
                                            string msg = String.Format("FTP 下載財金清算檔案成功，共 {0} 個檔案", remoteFiles.Count);
                                            jobLog.AppendLine(msg);
                                            WriteLog(msg);
                                        }
                                    }
                                    else
                                    {
                                        exitCode = -3;
                                        exitMsg = "FTP 下載財金清算檔案失敗，" + errmsg;
                                        jobLog.AppendLine(exitMsg);
                                        WriteLog(exitMsg);
                                    }
                                }
                                else
                                {
                                    string msg = "不執行 FTP 下載財金清算檔案";
                                    jobLog.AppendLine(msg);
                                    WriteLog(msg);
                                }
                            }
                            #endregion

                            #region 開始處理檔案
                            List<string> localFiles = null;
                            if (exitCode == 0)
                            {
                                FileInfo[] fInfos = dirData.GetFiles(filePattern);
                                if (fInfos != null && fInfos.Length > 0)
                                {
                                    localFiles = new List<string>(fInfos.Length);
                                    factory = new EntityFactory();

                                    foreach (FileInfo fInfo in fInfos)
                                    {
                                        StringBuilder log = new StringBuilder();
                                        log.AppendFormat("開始處理 {0}", fInfo.Name).AppendLine();

                                        #region 逐檔處理
                                        try
                                        {
                                            InboundHeader header = null;
                                            InboundTail tail = null;
                                            List<InboundDetail> details = new List<InboundDetail>();
                                            Int32 detailNo = 0;
                                            Int32 successCount = 0;     //拆解明細成功行數
                                            Int32 failureCount = 0;     //拆解失敗行數

                                            #region 拆解檔案
                                            using (StreamReader sr = new StreamReader(fInfo.FullName, _Encoding))
                                            {
                                                string err = null;
                                                string line = null;
                                                while ((line = sr.ReadLine()) != null)
                                                {
                                                    RecordType type = GetRecordType(line);
                                                    switch (type)
                                                    {
                                                        case RecordType.header:
                                                            #region 處理首筆
                                                            {
                                                                header = ParseInboundHeader(line, out err);
                                                                if (header == null)
                                                                {
                                                                    failureCount++;
                                                                    log.AppendFormat("首筆錯誤，錯誤訊息：{0}", err).AppendLine();
                                                                }
                                                            }
                                                            #endregion
                                                            break;
                                                        case RecordType.tail:
                                                            #region 處理尾筆
                                                            {
                                                                tail = ParseInboundTail(line, out err);
                                                                if (tail == null)
                                                                {
                                                                    failureCount++;
                                                                    log.AppendFormat("尾筆錯誤，錯誤訊息：{0}", err).AppendLine();
                                                                }
                                                            }
                                                            #endregion
                                                            break;
                                                        case RecordType.detail:
                                                            #region 處理明細
                                                            {
                                                                detailNo++;
                                                                InboundDetail detail = ParseInboundDetail(line, out err);
                                                                if (detail == null)
                                                                {
                                                                    failureCount++;
                                                                    log.AppendFormat("第{0}筆錯誤，錯誤訊息：{1}", detailNo, err).AppendLine();
                                                                }
                                                                else
                                                                {
                                                                    successCount++;
                                                                    details.Add(detail);
                                                                }
                                                            }
                                                            #endregion
                                                            break;
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region 轉換檔案 (如果有錯誤就整個檔案都不做)
                                            if (failureCount == 0)
                                            {
                                                bool isBreak = false;

                                                #region [MDY:20170309] 土銀反悔，回傳檔的檔名改成 原檔名後串 "1"
                                                #region [Old]
                                                //string fileName = "Inbound_" + fInfo.Name;
                                                #endregion

                                                string fileName = String.Format("{0}1{1}", Path.GetFileNameWithoutExtension(fInfo.Name), fInfo.Extension);
                                                #endregion

                                                string targetFileName = Path.Combine(dirTarget.FullName, fileName);

                                                #region 檢查檔案，如果存在就刪除
                                                if (File.Exists(targetFileName))
                                                {
                                                    try
                                                    {
                                                        File.Delete(targetFileName);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        isBreak = true;
                                                        log.AppendFormat("{0} 檔案已存在且刪除失敗，錯誤訊息：{1}", targetFileName, ex.Message).AppendLine();
                                                        jobLog.AppendFormat("{0} 檔案已存在且刪除失敗，錯誤訊息：{1}", targetFileName, ex.Message).AppendLine();
                                                    }
                                                }
                                                #endregion

                                                if (!isBreak)
                                                {
                                                    if (details == null || details.Count <= 0)
                                                    {
                                                        #region 寫空檔
                                                        try
                                                        {
                                                            using (StreamWriter sw = new StreamWriter(targetFileName, false, _Encoding))
                                                            {
                                                                sw.Write("");
                                                            }
                                                            log.AppendFormat("寫 {0} 檔成功(空檔)", targetFileName).AppendLine();
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            isBreak = true;
                                                            log.AppendFormat("寫 {0} 檔發生錯誤(空檔)，錯誤訊息：{1}", targetFileName, ex.Message).AppendLine();
                                                            jobLog.AppendFormat("寫 {0} 檔發生錯誤(空檔)，錯誤訊息：{1}", targetFileName, ex.Message).AppendLine();
                                                        }
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region 產生土銀要的檔案內容
                                                        StringBuilder content = new StringBuilder();
                                                        int contentCount = 0;   //轉檔資料筆數
                                                        int discardCount = 0;   //無效資料筆數
                                                        int upFailCount = 0;   //更新 InboundTxnDtlEntity 的 InboundFile 與 InboundData 失敗筆數
                                                        foreach (InboundDetail detail in details)
                                                        {
                                                            if (detail.field01.Substring(0, 2) == "52" && detail.field03 == "E" && detail.field55 == "L")
                                                            {
                                                                string err = null;
                                                                //string orderNumber = detail.field47;
                                                                //[MEMO:20170305] 目前看起來 field47 的資料格式為 [土銀代碼005] + [銷帳編號16碼] + [未知的7碼] + [訂單編號19碼]
                                                                //string orderNumber = detail.field47.Trim().Substring(detail.field47.Trim().Length - 19, 19);
                                                                string orderNumber = detail.field47.Substring(detail.field47.Length - 19, 19);
                                                                //[MEMO:20170305] field13 雖然是清算金額但單位不是台幣，又沒有匯率資料所以沒用
                                                                //decimal settleAmount = decimal.Parse(detail.field13) / 100;
                                                                InboundTxnDtlEntity inboundTxnDtl = GetInboundTxnDtlByOrderNumber(factory, orderNumber, out err);
                                                                if (inboundTxnDtl == null)
                                                                {
                                                                    //查無資料或失敗仍要產生檔案，避免一筆失敗其他資料都不會產生
                                                                    if (String.IsNullOrEmpty(err))
                                                                    {
                                                                        log.AppendFormat("查無 {0} 交易資料", orderNumber).AppendLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        log.AppendFormat("查詢 {0} 交易資料失敗，錯誤訊息：{1}", orderNumber, err).AppendLine();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    contentCount++;
                                                                    content.AppendLine(GetLandBankLine(inboundTxnDtl));

                                                                    #region [MDY:20170506] 更新 InboundTxnDtlEntity 的 InboundFile 與 InboundData
                                                                    if (String.IsNullOrEmpty(inboundTxnDtl.InboundFile))
                                                                    {
                                                                        string errmsg = UpdateInboundTxnDtlForInboundFile(factory, inboundTxnDtl.Sn, fInfo.Name, detail.orgData);
                                                                        if (!String.IsNullOrEmpty(errmsg))
                                                                        {
                                                                            upFailCount++;
                                                                            log.AppendFormat("更新 {0} 交易的 InboundFile 失敗，錯誤訊息：{1}", orderNumber, errmsg).AppendLine();
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (inboundTxnDtl.InboundFile.Equals(fInfo.Name, StringComparison.CurrentCultureIgnoreCase))
                                                                        {
                                                                            //log.AppendFormat("{0} 交易資料已處理過", orderNumber).AppendLine();
                                                                        }
                                                                        else
                                                                        {
                                                                            log.AppendFormat("{0} 交易資料已在 {1} 已處理過", orderNumber, inboundTxnDtl.InboundFile).AppendLine();
                                                                        }
                                                                    }
                                                                    #endregion
                                                                }
                                                            }
                                                            else
                                                            {
                                                                discardCount++;
                                                            }
                                                        }
                                                        #endregion

                                                        #region 寫檔
                                                        try
                                                        {
                                                            using (StreamWriter sw = new StreamWriter(targetFileName, false, _Encoding))
                                                            {
                                                                sw.Write(content.ToString());
                                                            }
                                                            if (content == null || content.Length <= 0)
                                                            {
                                                                log.AppendFormat("寫 {0} 檔成功(沒有符合的資料)", targetFileName).AppendLine();
                                                                jobLog.AppendFormat("寫 {0} 檔成功(沒有符合的資料)", targetFileName).AppendLine();
                                                            }
                                                            else
                                                            {
                                                                log.AppendFormat("寫 {0} 檔成功。原始資料 {1} 筆，轉檔資料 {2} 筆，無效資料 {3} 筆", targetFileName, details.Count, contentCount, discardCount).AppendLine();
                                                                jobLog.AppendFormat("寫 {0} 檔成功。原始資料 {1} 筆，轉檔資料 {2} 筆，無效資料 {3} 筆", targetFileName, details.Count, contentCount, discardCount).AppendLine();

                                                                if (upFailCount > 0)
                                                                {
                                                                    jobLog.AppendFormat("注意：更新交易資料的回覆檔資料欄位失敗 {0} 筆)", upFailCount).AppendLine();
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            isBreak = true;
                                                            log.AppendFormat("寫 {0} 檔發生錯誤，錯誤訊息：{1}", targetFileName, ex.Message).AppendLine();
                                                            jobLog.AppendFormat("寫 {0} 檔發生錯誤，錯誤訊息：{1}", targetFileName, ex.Message).AppendLine();
                                                        }
                                                        #endregion
                                                    }
                                                }

                                                if (!isBreak)
                                                {
                                                    localFiles.Add(fileName);
                                                }
                                            }
                                            else
                                            {
                                                log.AppendFormat("{0} 處理完畢，因為檔案有誤所以不轉換", fInfo.Name).AppendLine();
                                                jobLog.AppendFormat("{0} 處理完畢，因為檔案有誤所以不轉換", fInfo.Name).AppendLine();
                                            }
                                            #endregion

                                            #region 把檔案搬走
                                            string backupFileName = Path.Combine(dirBackup.FullName, fInfo.Name);
                                            try
                                            {
                                                if (File.Exists(backupFileName))
                                                {
                                                    File.Delete(backupFileName);
                                                }
                                                File.Move(fInfo.FullName, backupFileName);
                                            }
                                            catch (Exception ex)
                                            {
                                                log.AppendFormat("搬移 {0} 檔發生錯誤，錯誤訊息：{1}", backupFileName, ex.Message).AppendLine();
                                            }
                                            #endregion

                                        }
                                        catch (Exception ex)
                                        {
                                            log.AppendFormat("{0} 檔案處理發生例外，錯誤訊息：", fInfo.FullName, ex.Message).AppendLine();
                                            jobLog.AppendFormat("{0} 檔案處理發生例外，錯誤訊息：", fInfo.FullName, ex.Message).AppendLine();
                                        }
                                        #endregion

                                        WriteLog(log.ToString());
                                    }
                                }
                                else
                                {
                                    //exitCode = 0;
                                    //exitMsg = string.Format("{0} 沒有符合 {1} 檔名的檔案", dirData.FullName, filePattern);
                                    //jobLog.AppendLine(exitMsg);
                                    //WriteLog(exitMsg);
                                    string msg = string.Format("{0} 沒有符合 {1} 檔名的檔案", dirData.FullName, filePattern);
                                    jobLog.AppendLine(msg);
                                    WriteLog(msg);
                                }
                            }
                            #endregion

                            #region FTP 上傳
                            if (exitCode == 0)
                            {
                                if (isNeedFTPUP)
                                {
                                    if (localFiles == null || localFiles.Count == 0)
                                    {
                                        string msg = "未產生任何土銀清算檔案";
                                        jobLog.AppendLine(msg);
                                        WriteLog(msg);
                                    }
                                    else
                                    {
                                        StringBuilder ftpLog = new StringBuilder();
                                        string localPath = dirTarget.FullName;
                                        string bakPath = dirBackup.FullName;
                                        List<string> failFiles = null;
                                        string errmsg = UploadFTPFiles(ftp, localPath, localFiles, true, bakPath, retryTimes, retrySleep, ref ftpLog, out failFiles);

                                        if (String.IsNullOrEmpty(errmsg))
                                        {
                                            string msg = String.Format("FTP 上傳土銀清算檔案成功，共 {0} 個檔案", localFiles.Count);
                                            jobLog.AppendLine(msg);
                                            WriteLog(msg);
                                        }
                                        else
                                        {
                                            exitCode = -2;
                                            exitMsg = "FTP 上傳土銀清算檔案失敗，" + errmsg;
                                            jobLog.AppendLine(exitMsg);
                                            WriteLog(exitMsg);
                                        }
                                    }
                                }
                                else
                                {
                                    string msg = "不執行 FTP 上傳土銀清算檔案";
                                    jobLog.AppendLine(msg);
                                    WriteLog(msg);
                                }
                            }
                            #endregion

                            jobLog.AppendLine("結束處理程序");
                            WriteLog("結束處理程序");

                            #region [MDY:20210401] 原碼修正
                            m.ReleaseMutex();
                            #endregion
                        }
                        else
                        {
                            exitCode = -3;
                            exitMsg = "此程式已在執行中，不可重複執行";
                            jobLog.AppendLine(exitMsg);
                            WriteLog(exitMsg);
                        }
                    }
                }
                #endregion
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;
                exitMsg = String.Format("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
                WriteLog(String.Format("發生例外，參數：{0}，錯誤訊息：{1}", String.Join(" ", args), ex.Message));
            }
            finally
            {
                if (factory != null)
                {
                    factory.Dispose();
                    factory = null;
                }

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
                        WriteLog("更新批次處理佇列為已完成失敗，" + result.Message);
                    }
                }
                jobHelper.Dispose();
                jobHelper = null;
                #endregion

                Finish(null, exitCode);
            }
        }

        #region Log 相關
        public static void WriteLog(string msg)
        {
            if (!String.IsNullOrEmpty(_LogFileName))
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

        public static void Start()
        {
            #region 寫log
            Console.WriteLine("開始");
            WriteLog("開始");
            #endregion
        }

        public static void Finish(string log,int exitCode)
        {
            #region 寫log
            Console.WriteLine("結束");
            if (!String.IsNullOrWhiteSpace(log))
            {
                WriteLog(log);
            }
            WriteLog("結束\r\n");
            #endregion

            #region 發mail
            #endregion

            System.Environment.Exit(exitCode);
        }

        #endregion

        #region [MDY:20170309] 土銀反悔，檔名改成 "Inbound" + 8碼民國年日期 + ".txt" 所以這個方法用不到了
        #region [Old]
        ///// <summary>
        ///// 取得台灣曆法的日期字串 (yyMMdd)
        ///// </summary>
        ///// <returns></returns>
        //public static string GetCCMMDD(DateTime? date)
        //{
        //    System.Globalization.CultureInfo tc = new System.Globalization.CultureInfo("zh-TW");
        //    tc.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
        //    DateTime dt = date == null ? DateTime.Now : date.Value;
        //    return dt.ToString("yyMMdd",tc).Substring(1,6);
        //}
        #endregion
        #endregion

        public static RecordType GetRecordType(string line)
        {
            if(line.Substring(0,1).Equals("H",StringComparison.InvariantCultureIgnoreCase ))
            {
                return RecordType.header;
            }
            else if(line.Substring(0,1).Equals("T",StringComparison.InvariantCultureIgnoreCase ))
            {
                return RecordType.tail;
            }
            else
            {
                return RecordType.detail;
            }
        }

        public static InboundHeader ParseInboundHeader(string data,out string log)
        {
            log = "";
            InboundHeader header = new InboundHeader();
            int[] Filed_Len = new int[] { 1, 8, 8, 4, 8, 8, 2, 409 };
            int total_len = 0;
            foreach (int len in Filed_Len)
            {
                total_len += len;
            }

            if (GetBytes(data) != total_len)
            {
                log = "長度錯誤";
                return null;
            }

            int pos = 0;
            int idx = 0;
            try
            {
                #region
                header.type = SubString(data, pos, Filed_Len[idx]);
                if (header.type!="H")
                {
                    throw new Exception("必須是H");
                }

                pos += Filed_Len[idx];
                idx++;
                header.from = SubString(data, pos, Filed_Len[idx]);

                pos += Filed_Len[idx];
                idx++;
                header.to = SubString(data, pos, Filed_Len[idx]);

                pos += Filed_Len[idx];
                idx++;
                header.ENV = SubString(data, pos, Filed_Len[idx]);
                //if (header.ENV != "PROD") 上線時恢復
                //{
                //    throw new Exception("必須是營運檔案");
                //}

                pos += Filed_Len[idx];
                idx++;
                header.date = SubString(data, pos, Filed_Len[idx]);
                if (!isDate8(header.date))
                {
                    throw new Exception("必須是日期格式YYYYMMDD");
                }

                pos += Filed_Len[idx];
                idx++;
                header.file_type = SubString(data, pos, Filed_Len[idx]);

                pos += Filed_Len[idx];
                idx++;
                header.file_sn = SubString(data, pos, Filed_Len[idx]);

                pos += Filed_Len[idx];
                idx++;
                header.reserve = SubString(data, pos, Filed_Len[idx]);
                #endregion
            }
            catch (Exception ex)
            {
                header = null;
                log = string.Format("第{0}個欄位錯誤，{1}", idx, ex.Message);
            }
            return header;
        }

        public static InboundDetail ParseInboundDetail(string data, out string log)
        {
            log = "";
            InboundDetail detail = new InboundDetail();
            detail.orgData = data;
            int[] Filed_Len = new int[] { 4, 1, 1, 8, 8, 8, 8, 19, 12, 3, 12, 3, 12, 3, 8, 20, 4, 40, 25, 13, 3, 5, 3, 2, 15, 6, 6, 6, 1, 1, 1, 15, 6, 4, 6, 1, 2, 4, 23, 1, 4, 10, 1, 1, 3, 1, 45, 2, 1, 1, 4, 2, 1, 1, 1, 3, 2, 2, 5, 35 };
            int total_len = 0;
            foreach (int len in Filed_Len)
            {
                total_len += len;
            }

            if (GetBytes(data) != total_len)
            {
                log = string.Format("長度錯誤，應該長度{0}，實際長度{1}", total_len, GetBytes(data));
                return null;
            }

            int pos = 0;
            int idx = 0;
            try
            {
                #region
                detail.field01 = SubString(data, pos, Filed_Len[idx]).Trim();
                //if (detail.field01.Substring(0,2) != "52")
                //{
                //    throw new Exception("必須是52xx");
                //}

                pos += Filed_Len[idx];
                idx++;
                detail.field02 = SubString(data, pos, Filed_Len[idx]).Trim();
                if (detail.field02!="1")
                {
                    throw new Exception("必須是1");
                }

                pos += Filed_Len[idx];
                idx++;
                detail.field03 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field04 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field05 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field06 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field07 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field08 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field09 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field10 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field11 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field12 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field13 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field14 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field15 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field16 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field17 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field18 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field19 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field20 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field21 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field21 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field23 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field24 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field25 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field26 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field27 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field28 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field29 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field30 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field31 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field32 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field33 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field34 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field35 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field36 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field37 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field38 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field39 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field40 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field41 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field42 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field43 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field44 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field45 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field46 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field47 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field48 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field49 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field50 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field51 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field52 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field53 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field54 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field55 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field56 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field57 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field58 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field59 = SubString(data, pos, Filed_Len[idx]).Trim();

                pos += Filed_Len[idx];
                idx++;
                detail.field60 = SubString(data, pos, Filed_Len[idx]).Trim();

                //pos += Filed_Len[idx];
                //idx++;
                //detail.field61 = SubString(data, pos, Filed_Len[idx]);
                #endregion
            }
            catch (Exception ex)
            {
                detail = null;
                log = string.Format("第{0}個欄位錯誤，{1}", idx, ex.Message);
            }
            return detail;
        }

        public static InboundTail ParseInboundTail(string data, out string log)
        {
            log = "";
            InboundTail tail = new InboundTail();
            int[] Filed_Len = new int[] { 1, 8, 8, 15, 416 };
            int total_len = 0;
            foreach (int len in Filed_Len)
            {
                total_len += len;
            }

            if (GetBytes(data) != total_len)
            {
                log = "長度錯誤";
                return null;
            }

            int pos = 0;
            int idx = 0;
            try
            {
                #region
                tail.type = SubString(data, pos, Filed_Len[idx]);

                pos += Filed_Len[idx];
                idx++;
                tail.lines = SubString(data, pos, Filed_Len[idx]);

                pos += Filed_Len[idx];
                idx++;
                tail.records = SubString(data, pos, Filed_Len[idx]);

                pos += Filed_Len[idx];
                idx++;
                tail.amount = SubString(data, pos, Filed_Len[idx]);

                pos += Filed_Len[idx];
                idx++;
                tail.reserve = SubString(data, pos, Filed_Len[idx]);
                #endregion
            }
            catch(Exception ex)
            {
                tail = null;
                log = string.Format("第{0}個欄位錯誤，{1}", idx, ex.Message);
            }
            
            return tail;
        }

        public static Int32 GetBytes(string line)
        {
            return _Encoding.GetByteCount(line);
        }

        public static string SubString(string line,int start,int lenght)
        {
            try
            {
                Byte[] bytes = _Encoding.GetBytes(line);
                return _Encoding.GetString(bytes, start, lenght);
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static bool isDate8(string data)
        {
            bool rc = false;
            try
            {
                if (data.Length != 8) throw new Exception("長度不足8碼");

                DateTime d = DateTime.Parse(string.Format("{0}/{1}/{2}", data.Substring(0, 4), data.Substring(4, 2), data.Substring(6, 2)));

                rc = true;
            }
            catch(Exception)
            {

            }
            return rc;
        }

        /// <summary>
        /// 產生土銀的清算檔資料行字串
        /// </summary>
        /// <param name="inboundTxnDtl">交易資料</param>
        /// <returns>傳回資料行字串</returns>
        public static string GetLandBankLine(InboundTxnDtlEntity inboundTxnDtl)
        {
            string cancelNo = inboundTxnDtl.CancelNo.Trim().PadLeft(16, '0').Substring(0, 16);
            string amount = inboundTxnDtl.ReceiveAmount.ToString("0000000000000.00").Replace(".", "");
            string fee = inboundTxnDtl.Fee.ToString("0000000000000.00").Replace(".", "");
            return string.Format("{0}{1}{2}", cancelNo, amount, fee);
        }

        public static InboundTxnDtlEntity GetInboundTxnDtlByOrderNumber(EntityFactory factory, string orderNumber, out string errmsg)
        {
            errmsg = null;
            InboundTxnDtlEntity data = null;
            Expression where = new Expression(InboundTxnDtlEntity.Field.OrderNumber, orderNumber);
            KeyValueList<OrderByEnum> orderbys = null;
            Result result = factory.SelectFirst<InboundTxnDtlEntity>(where, orderbys, out data);
            if (!result.IsSuccess)
            {
                errmsg = result.Message;
            }
            return data;
        }

        private static string sqlUpdateInboundTxnDtlForInboundFile = String.Format("UPDATE [{0}] SET [{1}] = @InboundFile, [{2}] = @InboundData WHERE [{3}] = @Sn",
            InboundTxnDtlEntity.TABLE_NAME, InboundTxnDtlEntity.Field.InboundFile, InboundTxnDtlEntity.Field.InboundData, InboundTxnDtlEntity.Field.Sn);

        #region [MDY:20170506] 更新 InboundTxnDtlEntity 的 InboundFile 與 InboundData
        /// <summary>
        /// 更新 InboundTxnDtlEntity 的 InboundFile 與 InboundData
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="sn"></param>
        /// <param name="inboundFile"></param>
        /// <param name="inboundData"></param>
        /// <returns></returns>
        public static string UpdateInboundTxnDtlForInboundFile(EntityFactory factory, Int64 sn, string inboundFile, string inboundData)
        {
            string errmsg = null;
            try
            {
                //string sql = String.Format("UPDATE [{0}] SET [{1}] = @InboundFile, [{2}] = @InboundData WHERE [{3}] = @Sn",
                //    InboundTxnDtlEntity.TABLE_NAME, InboundTxnDtlEntity.Field.InboundFile, InboundTxnDtlEntity.Field.InboundData, InboundTxnDtlEntity.Field.Sn);
                KeyValue[] parameters = new KeyValue[] { new KeyValue("@InboundFile", inboundFile.Trim()), new KeyValue("@InboundData", inboundData), new KeyValue("@Sn", sn) };
                int count = 0;
                Result result = factory.ExecuteNonQuery(sqlUpdateInboundTxnDtlForInboundFile, parameters, out count);
                if (!result.IsSuccess)
                {
                    errmsg = result.Message;
                }
                else if (count == 0)
                {
                    errmsg = "無資料被更新";
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }
        #endregion

        #region FTP 相關
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

        #region [MDY:20181203] 改用 FTP 相關命令參數 (20181201_05)
        #region [MDY:20220530] Checkmarx 調整
        #region [MDY:20210401] 原碼修正
        private static FTPUtility CreateFTPUtility(string host, int port, bool useSSL, string uid, string pxx, string remotePath)
        {
            int codepage = 950;
            if (!String.IsNullOrWhiteSpace(host)
                && port> 0
                && !String.IsNullOrWhiteSpace(uid)
                && !String.IsNullOrWhiteSpace(pxx)
                && !String.IsNullOrWhiteSpace(remotePath))
            {
                FTPUtility ftp = new FTPUtility();
                ftp.Hostname = host;
                ftp.Port = port.ToString();
                ftp.Username = uid;
                ftp.Password = pxx;
                ftp.CurrentFolder = remotePath;
                ftp.EnableSSL = useSSL;
                ftp.CodePage = codepage;
                return ftp;
            }
            else
            {
                return null;
            }
        }
        #endregion
        #endregion
        #endregion

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
                        if (fileName.EndsWith("*") && itemName.StartsWith(fileName.Substring(0, fileName.Length - 1)))
                        {
                            fileNames.Add(itemName);
                        }
                        else
                        {
                            msg.AppendFormat("清單項目 ({0}) 的檔名 ({1}) 與指定檔名不符 ({2})", listItem, itemName, fileName).AppendLine();
                            continue;
                        }
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
                    errmsg = String.Format("上傳檔案 ({0}) 失敗", String.Join("、", failFiles.ToArray()));
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
    }

    public enum RecordType
    {
        header,
        detail,
        tail
    }

    public class InboundHeader
    {
        /// <summary>
        /// RECORD識別碼 AN 1 H：Header
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 資料產生單位 N 8
        /// </summary>
        public string from { get; set; }

        /// <summary>
        /// 資料接收單位 N 8
        /// </summary>
        public string to { get; set; }

        /// <summary>
        /// 檔案性質 AN 4 PROD：營運檔案 | TEST：測試檔案
        /// </summary>
        public string ENV { get; set; }

        /// <summary>
        /// 檔案產出日期 N 8 YYYYMMDD
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// 檔案產出類型 AN 8 ICF01QND
        /// </summary>
        public string file_type { get; set; }

        /// <summary>
        /// 檔案產出序號 N 2 同檔名序號
        /// </summary>
        public string file_sn { get; set; }

        /// <summary>
        /// 保留欄位 AN 409
        /// </summary>
        public string reserve { get; set; }
    }

    public class InboundDetail
    {
        /// <summary>
        /// 交易代號 52XX： 跨境電子支付平台交易，提供收單行
        /// </summary>
        public string field01 { get; set; }
        /// <summary>
        /// RECORD識別碼 Must be ‘1’
        /// </summary>
        public string field02 { get; set; }
        /// <summary>
        /// 卡片類型 E：Electronic Payment(電子支付平台)
        /// </summary>
        public string field03 { get; set; }
        /// <summary>
        /// 參加單位代號 
        /// </summary>
        public string field04 { get; set; }
        /// <summary>
        /// 傳送端 
        /// </summary>
        public string field05 { get; set; }
        /// <summary>
        /// 接收端
        /// </summary>
        public string field06 { get; set; }
        /// <summary>
        /// 代理單位代號
        /// </summary>
        public string field07 { get; set; }
        /// <summary>
        /// 卡片號碼
        /// </summary>
        public string field08 { get; set; }
        /// <summary>
        /// 來源端金額 整數10位,小數2位
        /// </summary>
        public string field09 { get; set; }
        /// <summary>
        /// 來源端幣別 
        /// </summary>
        public string field10 { get; set; }
        /// <summary>
        /// 目的端金額 整數10位,小數2位
        /// </summary>
        public string field11 { get; set; }
        /// <summary>
        /// 目的端幣別
        /// </summary>
        public string field12 { get; set; }
        /// <summary>
        /// 清算金額  整數10位,小數2位
        /// </summary>
        public string field13 { get; set; }
        /// <summary>
        /// 清算幣別
        /// </summary>
        public string field14 { get; set; }
        /// <summary>
        /// 清算匯率 第1位為小數位數，第2-8位為匯率資訊
        /// </summary>
        public string field15 { get; set; }
        /// <summary>
        /// 特約商店代號
        /// </summary>
        public string field16 { get; set; }
        /// <summary>
        /// 特約商店行業類別碼
        /// </summary>
        public string field17 { get; set; }
        /// <summary>
        /// 特店中文名稱
        /// </summary>
        public string field18 { get; set; }
        /// <summary>
        /// 特約商店英文名稱
        /// </summary>
        public string field19 { get; set; }
        /// <summary>
        /// 特約商店所在地/城市
        /// </summary>
        public string field20 { get; set; }
        /// <summary>
        /// 特約商店所在省份
        /// </summary>
        public string field21 { get; set; }
        /// <summary>
        /// 特約商店郵遞區號
        /// </summary>
        public string field22 { get; set; }
        /// <summary>
        /// 特約商店國家代號
        /// </summary>
        public string field23 { get; set; }
        /// <summary>
        /// 特殊條件識別碼
        /// </summary>
        public string field24 { get; set; }
        /// <summary>
        /// 次特店代號
        /// </summary>
        public string field25 { get; set; }
        /// <summary>
        /// 購貨日期 MMDDYY
        /// </summary>
        public string field26 { get; set; }
        /// <summary>
        /// 購貨時間 hhmmss
        /// </summary>
        public string field27 { get; set; }
        /// <summary>
        /// 授權碼
        /// </summary>
        public string field28 { get; set; }
        /// <summary>
        /// 電話/郵購識別碼
        /// </summary>
        public string field29 { get; set; }
        /// <summary>
        /// POS Environment
        /// </summary>
        public string field30 { get; set; } 
        /// <summary>
        /// 自助端末機識別碼/CAT LEVEL
        /// </summary>
        public string field31 { get; set; }
        /// <summary>
        /// 端末機代碼
        /// </summary>
        public string field32 { get; set; }
        /// <summary>
        /// 端末機傳送資料日期 rrmmdd
        /// </summary>
        public string field33 { get; set; }
        /// <summary>
        /// 資料批次號碼
        /// </summary>
        public string field34 { get; set; }
        /// <summary>
        /// 端末機交易序號
        /// </summary>
        public string field35 { get; set; }
        /// <summary>
        /// POS端末機性能
        /// </summary>
        public string field36 { get; set; }
        /// <summary>
        /// POS輸入型態
        /// </summary>
        public string field37 { get; set; }
        /// <summary>
        /// VISA/MarstarCard 處理日期
        /// </summary>
        public string field38 { get; set; }
        /// <summary>
        /// 微縮影片代號
        /// </summary>
        public string field39 { get; set; }
        /// <summary>
        /// 使用碼
        /// </summary>
        public string field40 { get; set; } 
        /// <summary>
        /// 沖正駁回理由碼
        /// </summary>
        public string field41 { get; set; }
        /// <summary>
        /// 沖正參考號碼
        /// 跨境電子支付平台手續費 跨境電子支付平台收取/退還之手續費 整數8位,小數2位
        /// </summary>
        public string field42 { get; set; }
        /// <summary>
        /// 特殊沖正識別碼/VCRFS(/MASTERCOM)識別碼
        /// </summary>
        public string field43 { get; set; }
        /// <summary>
        /// 附寄文件識別碼
        /// </summary>
        public string field44 { get; set; }
        /// <summary>
        /// Service Code
        /// </summary>
        public string field45 { get; set; }
        /// <summary>
        /// UCAF
        /// </summary>
        public string field46 { get; set; }
        /// <summary>
        /// 訂單編號/銷帳編號
        /// </summary>
        public string field47 { get; set; }
        /// <summary>
        /// 費用/稅目種類代碼
        /// </summary>
        public string field48 { get; set; }
        /// <summary>
        /// 交易處理費屬性
        /// </summary>
        public string field49 { get; set; } 
        /// <summary>
        /// 清算識別碼
        /// </summary>
        public string field50 { get; set; }
        /// <summary>
        /// 交易退回時原交易代號(VISA)
        /// </summary>
        public string field51 { get; set; }
        /// <summary>
        /// 交易退回理由碼
        /// </summary>
        public string field52 { get; set; }
        /// <summary>
        /// 支付型態
        /// </summary>
        public string field53 { get; set; }
        /// <summary>
        /// 電子支付平台代碼 A：Alipay
        /// </summary>
        public string field54 { get; set; }
        /// <summary>
        /// 電子支付平台交易狀態 L：已清算
        /// </summary>
        public string field55 { get; set; }
        /// <summary>
        /// 申報性質別
        /// </summary>
        public string field56 { get; set; }
        /// <summary>
        /// 帳單類別
        /// </summary>
        public string field57 { get; set; }
        /// <summary>
        /// 特店類型
        /// </summary>
        public string field58 { get; set; }
        /// <summary>
        /// 繳費項目
        /// </summary>
        public string field59 { get; set; }
        /// <summary>
        /// 保留欄位
        /// </summary>
        public string field60 { get; set; }


        /// <summary>
        /// 原始資料
        /// </summary>
        public string orgData { get; set; }
    }

    public class InboundTail
    {
        /// <summary>
        /// RECORD識別碼 AN 1 T：Trail
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 檔案交易總筆數 N 8
        /// </summary>
        public string lines { get; set; }

        /// <summary>
        /// 檔案Record總數 N 8
        /// </summary>
        public string records { get; set; }

        /// <summary>
        /// 檔案交易總來源金額 N 15
        /// </summary>
        public string amount { get; set; }

        /// <summary>
        /// 保留欄位 AN 416
        /// </summary>
        public string reserve { get; set; }
    }
}
