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
using Fuju.DB;
using Fuju.DB.Data;

using Entities;

namespace CallEAI
{
    /// <summary>
    /// 發送電文 (DKEY：EAI 每日換 Key、D70：D00I70資料下載、D70B：D00I70資料下載(補)、D70C：D00I70資料下載(C時段))
    /// </summary>
    class Program
    {
        #region [OLD]
//        #region FileLog 相關
//        private class FileLoger
//        {
//            private string _LogName = null;
//            public string LogName
//            {
//                get
//                {
//                    return _LogName;
//                }
//                private set
//                {
//                    _LogName = value == null ? null : value.Trim();
//                }
//            }

//            private string _LogPath = null;
//            public string LogPath
//            {
//                get
//                {
//                    return _LogPath;
//                }
//                private set
//                {
//                    _LogPath = value == null ? String.Empty : value.Trim();
//                }
//            }

//            public bool IsDebug
//            {
//                get;
//                private set;
//            }

//            private string _LogFileName = null;

//            public FileLoger(string logName)
//            {
//                this.LogName = logName;
//                this.LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
//                string logMode = ConfigurationManager.AppSettings.Get("LOG_MDOE");
//                if (String.IsNullOrEmpty(logMode))
//                {
//                    this.IsDebug = false;
//                }
//                else
//                {
//                    this.IsDebug = (logMode.Trim().Equals("DEBUG", StringComparison.CurrentCultureIgnoreCase));
//                }
//                this.Initial();
//            }

//            public FileLoger(string logName, string path, bool isDebug)
//            {
//                this.LogName = logName;
//                this.LogPath = path;
//                this.IsDebug = isDebug;
//                this.Initial();
//            }

//            public string Initial()
//            {
//                if (!String.IsNullOrEmpty(this.LogPath))
//                {
//                    try
//                    {
//                        DirectoryInfo info = new DirectoryInfo(this.LogPath);
//                        if (!info.Exists)
//                        {
//                            info.Create();
//                        }
//                        if (String.IsNullOrEmpty(this.LogName))
//                        {
//                            string fileName = String.Format("{0:yyyyMMdd}.log", DateTime.Today);
//                            _LogFileName = Path.Combine(info.FullName, fileName);
//                        }
//                        else
//                        {
//                            string fileName = String.Format("{0}_{1:yyyyMMdd}.log", this.LogName, DateTime.Today);
//                            _LogFileName = Path.Combine(info.FullName, fileName);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        return ex.Message;
//                    }
//                }
//                return null;
//            }

//            public void WriteLog(string msg)
//            {
//                if (!String.IsNullOrEmpty(_LogFileName) && msg != null)
//                {
//                    try
//                    {
//                        using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
//                        {
//                            if (String.IsNullOrEmpty(msg))
//                            {
//                                sw.WriteLine(String.Empty);
//                            }
//                            else
//                            {
//                                sw.WriteLine("[{0:HH:mm:ss}] {1}", DateTime.Now, msg);
//                            }
//                        }
//                    }
//                    catch (Exception)
//                    {
//                    }
//                }
//            }

//            public void WriteLog(string format, params object[] args)
//            {
//                if (!String.IsNullOrEmpty(format) && args != null && args.Length > 0)
//                {
//                    try
//                    {
//                        this.WriteLog(String.Format(format, args));
//                    }
//                    catch (Exception)
//                    {
//                    }
//                }
//            }

//            public void WriteLog(StringBuilder msg)
//            {
//                if (!String.IsNullOrEmpty(_LogFileName) && msg != null && msg.Length > 0)
//                {
//                    try
//                    {
//                        using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
//                        {
//                            sw.WriteLine(msg);
//                        }
//                    }
//                    catch (Exception)
//                    {
//                    }
//                }
//            }

//            public void WriteDebugLog(string msg)
//            {
//                if (this.IsDebug)
//                {
//                    this.WriteLog(msg);
//                }
//            }

//            public void WriteDebugLog(string format, params object[] args)
//            {
//                if (this.IsDebug)
//                {
//                    this.WriteLog(format, args);
//                }
//            }
//        }
//        #endregion

//        /// <summary>
//        /// 參數：service_id=所屬作業類別代碼(DKEY、D70、D70B、D70C) [txdate=D70、D70B、D70C交易日期(TODAY, YESTERDAY 或 yyyyMMdd)] [machine=伺服器名稱]
//        /// </summary>
//        /// <param name="args"></param>
//        static void Main(string[] args)
//        {
//            #region Initial
//            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
//            Assembly myAssembly = Assembly.GetExecutingAssembly();
//            string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
//            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);

//            FileLoger fileLog = new FileLoger(appName);
//            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
//            string jobTypeId = null;        //作業類別代碼
//            string jobTypeName = null;      //作業類別名稱
//            int exitCode = 0;
//            string exitMsg = null;
//            #endregion

//            StringBuilder log = new StringBuilder();    //日誌紀錄

//            //不提供 retry 因為此程式會執行多次
//            //int retryTimes = 5;     //re-try 次數 (預設為5次)
//            //int retrySleep = 15;    //re-try 間隔，單位分鐘 (預設為15分鐘)

//            DateTime startTime = DateTime.Now;

//            JobCubeHelper jobHelper = new JobCubeHelper();
//            int jobNo = 0;
//            string jobStamp = null;
//            StringBuilder jobLog = new StringBuilder();

//            #region [MDY:20190506] 資料讀取失敗 retry 參數
//            int retryMaxTime = 3;  //retry 最大次數
//            int retryIntervalSecond = 20;  //retry 間隔秒數
//            #endregion

//            try
//            {
//                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 開始", DateTime.Now, appName).AppendLine();

//                #region 處理參數
//                string eaiKind = null;
//                DateTime? txDate = null;    //D00I70交易日期

//                #region [MDY:20181007] 不同伺服器使用自己的 CustLoginId，不同 CustLoginId 使用自己的每日的交易 KEY
//                List<string> machineNames = new List<string>(4);
//                #endregion

//                if (exitCode == 0)
//                {
//                    log.AppendFormat("命令參數：{0}", (args == null || args.Length == 0) ? null : String.Join(" ", args)).AppendLine();

//                    string errmsg = null;

//                    if (args != null && args.Length > 0)
//                    {
//                        #region 拆解參數
//                        foreach (string arg in args)
//                        {
//                            string[] kvs = arg.Split('=');
//                            if (kvs.Length == 2)
//                            {
//                                string key = kvs[0].Trim().ToLower();
//                                string value = kvs[1].Trim();
//                                switch (key)
//                                {
//                                    case "txdate":
//                                        #region txDate (D00I70交易日期)
//                                        if (!String.IsNullOrEmpty(value))
//                                        {
//                                            switch (value.ToUpper())
//                                            {
//                                                case "TODAY":
//                                                    txDate = startTime.Date;
//                                                    break;
//                                                case "YESTERDAY":
//                                                    txDate = startTime.Date.AddDays(-1);
//                                                    break;
//                                                default:
//                                                    txDate = DataFormat.ConvertDateText(value);
//                                                    if (txDate == null)
//                                                    {
//                                                        errmsg = "txdate 參數值不是有效的日期格式 (TODAY, YESTERDAY 或 yyyyMMdd 格式)";
//                                                    }
//                                                    break;
//                                            }
//                                        }
//                                        break;
//                                        #endregion

//                                    case "service_id":
//                                        #region jobTypeId (所屬作業類別代碼)
//                                        {
//                                            jobTypeId = value;
//                                            switch (jobTypeId)
//                                            {
//                                                case JobCubeTypeCodeTexts.DKEY:
//                                                    eaiKind = EAILogEntity.KIND_CHANGEKEY;
//                                                    jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);
//                                                    break;
//                                                case JobCubeTypeCodeTexts.D70:
//                                                case JobCubeTypeCodeTexts.D70B:
//                                                case JobCubeTypeCodeTexts.D70C:
//                                                    eaiKind = EAILogEntity.KIND_D0070;
//                                                    jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);
//                                                    break;
//                                                default:
//                                                    errmsg = "service_id 參數值不是正確的作業類別代碼 (DKEY、D70、D70B、D70C)";
//                                                    break;
//                                            }
//                                        }
//                                        break;
//                                        #endregion

//                                    #region [MDY:20181007] 不同伺服器使用自己的 CustLoginId，不同 CustLoginId 使用自己的每日的交易 KEY
//                                    case "machine":
//                                        #region 伺服器名稱
//                                        if (!String.IsNullOrEmpty(value))
//                                        {
//                                            string[] vals = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
//                                            foreach (string val in vals)
//                                            {
//                                                if (!String.IsNullOrWhiteSpace(val))
//                                                {
//                                                    machineNames.Add(val.Trim());
//                                                }
//                                            }
//                                        }
//                                        break;
//                                        #endregion
//                                    #endregion

//                                    default:
//                                        errmsg = String.Format("不支援 {0} 命令參數", arg);
//                                        break;
//                                }
//                            }
//                            else
//                            {
//                                errmsg = String.Format("不支援 {0} 命令參數", arg);
//                            }
//                            if (!String.IsNullOrEmpty(errmsg))
//                            {
//                                break;
//                            }
//                        }
//                        #endregion
//                    }
//                    else
//                    {
//                        errmsg = "未指定命令參數";
//                    }

//                    #region 檢查必要參數
//                    if (String.IsNullOrEmpty(errmsg))
//                    {
//                        List<string> lost = new List<string>();

//                        if (String.IsNullOrEmpty(eaiKind))
//                        {
//                            lost.Add("service_id");
//                        }

//                        if (eaiKind == EAILogEntity.KIND_D0070 && txDate == null)
//                        {
//                            lost.Add("txdate");
//                        }

//                        if (lost.Count > 0)
//                        {
//                            errmsg = String.Format("缺少 {0} 參數", String.Join(", ", lost));
//                        }
//                    }
//                    #endregion

//                    if (!String.IsNullOrEmpty(errmsg))
//                    {
//                        exitCode = -1;
//                        exitMsg = String.Format("參數錯誤，錯誤訊息：{0}", errmsg);
//                        jobLog.AppendLine(exitMsg);
//                        log.AppendLine(exitMsg);

//                        if (args == null || args.Length == 0)
//                        {
//                            log.AppendLine("參數語法：service_id=所屬作業類別代碼(DKEY、D70、D70B、D70C) [txdate=D70、D70B、D70C交易日期(TODAY, YESTERDAY 或 yyyyMMdd)]");
//                        }
//                    }
//                }
//                #endregion

//                #region 取得 EAIHelper
//                EAIHelper helper = null;
//                if (exitCode == 0)
//                {
//                    #region [MDY:20181007] 使用新的 EAIHelper (一律取自專案組態的參數)
//                    #region [OLD]
//                    //helper = new EAIHelper(true);
//                    #endregion

//                    helper = new EAIHelper();
//                    #endregion

//                    if (!helper.IsEAIArgsReady())
//                    {
//                        exitCode = -1;
//                        exitMsg = "EAI 的執行參數未設定好";
//                        jobLog.AppendLine(exitMsg);
//                        log.AppendLine(exitMsg);
//                    }
//                }
//                #endregion

//                #region 新增處理中的 Job
//                if (exitCode == 0)
//                {
//                    JobcubeEntity job = new JobcubeEntity();
//                    job.Jtypeid = jobTypeId;
//                    job.Jparam = String.Join(" ", args);
//                    Result result = jobHelper.InsertProcessJob(ref job, jobCheckMode);
//                    if (result.IsSuccess)
//                    {
//                        jobNo = job.Jno;
//                        jobStamp = job.Memo;
//                    }
//                    else
//                    {
//                        exitCode = -2;
//                        log.AppendLine(result.Message);
//                    }
//                }
//                #endregion

//                #region 發送電文
//                if (exitCode == 0)
//                {
//                    string chkName = "Global\\" + eaiKind + "_" + appGuid;
//                    using (Mutex m = new Mutex(false, chkName))    //全域不可重複執行
//                    {
//                        //檢查是否同名Mutex已存在(表示另一份程式正在執行)
//                        if (m.WaitOne(0, false))
//                        {
//                            if (eaiKind == EAILogEntity.KIND_CHANGEKEY)
//                            {
//                                #region 換 Key
//                                #region [MDY:20181007] 不同伺服器使用自己的 CustLoginId，不同 CustLoginId 使用自己的每日的交易 KEY
//                                if (machineNames.Count > 0)
//                                {
//                                    foreach (string machineName in machineNames)
//                                    {
//                                        helper = new EAIHelper(machineName);
//                                        bool isOK = helper.ChangeDailyKey();
//                                        log.AppendFormat("({1}) ChangeDailyKey Log : {0}", helper.Log, machineName).AppendLine();
//                                        if (isOK)
//                                        {
//                                            exitMsg = String.Concat(machineName, " 換 Key 成功");
//                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                        }
//                                        else
//                                        {
//                                            exitCode = -4;
//                                            exitMsg = String.Format("{1} 換 Key 失敗，錯誤訊息：{0}", helper.ErrMsg, machineName);
//                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    bool isOK = helper.ChangeDailyKey();
//                                    log.AppendFormat("ChangeDailyKey Log : {0}", helper.Log).AppendLine();
//                                    if (isOK)
//                                    {
//                                        exitMsg = "換 Key 成功";
//                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                    }
//                                    else
//                                    {
//                                        exitCode = -4;
//                                        exitMsg = String.Format("換 Key 失敗，錯誤訊息：{0}", helper.ErrMsg);
//                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                    }
//                                }
//                                #endregion
//                                #endregion
//                            }
//                            else if (eaiKind == EAILogEntity.KIND_D0070)
//                            {
//                                #region D00I70
//                                string txDay = Common.GetTWDate8(txDate.Value);

//                                #region [MDY:20170818] M201708_01 取 SC31ReceiveType 系統參數 (20170531_01)
//                                List<string> sc31ReceiveTypes = null;
//                                if (exitCode == 0)
//                                {
//                                    #region [MDY:20190506] 失敗則 retry
//                                    int sleepMillisecond = retryIntervalSecond * 1000;
//                                    Result result = null;
//                                    Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.SC31_RECEIVETYPE);
//                                    for (int retryTimes = 0; retryTimes <= retryMaxTime; retryTimes++)
//                                    {
//                                        ConfigEntity config = null;
//                                        using (EntityFactory factory = new EntityFactory())
//                                        {
//                                            result = factory.SelectFirst<ConfigEntity>(where, null, out config);
//                                        }
//                                        if (result.IsSuccess)
//                                        {
//                                            if (config != null && !String.IsNullOrWhiteSpace(config.ConfigValue))
//                                            {
//                                                string[] values = config.ConfigValue.Trim().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
//                                                sc31ReceiveTypes = new List<string>(values.Length);
//                                                for (int idx = 0; idx < values.Length; idx++)
//                                                {
//                                                    string value = values[idx].Trim();
//                                                    if (!String.IsNullOrEmpty(value) && !sc31ReceiveTypes.Contains(value))
//                                                    {
//                                                        sc31ReceiveTypes.Add(value);
//                                                    }
//                                                }
//                                            }
//                                            if (sc31ReceiveTypes == null)
//                                            {
//                                                sc31ReceiveTypes = new List<string>(0);
//                                            }
//                                            break;
//                                        }
//                                        else
//                                        {
//                                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 第 {1} 次查詢學校銷帳檔3合1的商家代號系統參失敗，錯誤訊息：{2}", DateTime.Now, retryTimes, result.Message).AppendLine();
//                                            Thread.Sleep(sleepMillisecond);
//                                        }
//                                    }
//                                    if (!result.IsSuccess)
//                                    {
//                                        exitCode = -3;
//                                        exitMsg = String.Format("查詢學校銷帳檔3合1的商家代號系統參失敗，錯誤訊息：{0}", result.Message);
//                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                    }
//                                    #endregion
//                                }
//                                #endregion

//                                #region 取得所有有效的商家代號與下筆查詢序號清單
//                                KeyValueList<int> schools = null;
//                                if (exitCode == 0)
//                                {
//                                    #region [MDY:20190506] 失敗則 retry
//                                    int sleepMillisecond = retryIntervalSecond * 1000;
//                                    Result result = null;
//                                    string sql = @"
//SELECT Receive_Type
//     , (SELECT ISNULL(MAX([Source_Seq]), 0) + 1 FROM [Cancel_Debts] AS CD WHERE CD.[Receive_Type] = SR.[Receive_Type] AND CD.[File_Name] = 'D00I70_' + SR.[Receive_Type] + '_' + @TXDAY) AS NEXT_SEQ
//  FROM School_Rtype AS SR 
// WHERE status = @NormalStatus".Trim();
//                                    KeyValue[] parameters = new KeyValue[] { new KeyValue("@NormalStatus", DataStatusCodeTexts.NORMAL), new KeyValue("@TXDAY", txDay) };
//                                    for (int retryTimes = 0; retryTimes <= retryMaxTime; retryTimes++)
//                                    {
//                                        System.Data.DataTable dt = null;
//                                        using (EntityFactory factory = new EntityFactory())
//                                        {
//                                            result = factory.GetDataTable(sql, parameters, 0, 0, out dt);
//                                        }
//                                        if (result.IsSuccess)
//                                        {
//                                            if (dt != null && dt.Rows.Count > 0)
//                                            {
//                                                schools = new KeyValueList<int>(dt.Rows.Count);
//                                                foreach (System.Data.DataRow drow in dt.Rows)
//                                                {
//                                                    string appNo = drow["Receive_Type"].ToString().Trim();

//                                                    #region [MDY:20170818] M201708_01 排除 SC31ReceiveType 的商家代號 (20170531_01)
//                                                    if (sc31ReceiveTypes != null && sc31ReceiveTypes.Contains(appNo))
//                                                    {
//                                                        continue;
//                                                    }
//                                                    #endregion

//                                                    int sTxSeq = Convert.ToInt32(drow["NEXT_SEQ"]);
//                                                    schools.Add(appNo, sTxSeq);
//                                                }
//                                            }
//                                            break;
//                                        }
//                                        else
//                                        {
//                                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 第 {1} 次查詢商家代號與最後序號資料失敗，錯誤訊息：{2}", DateTime.Now, retryTimes, result.Message).AppendLine();
//                                            Thread.Sleep(sleepMillisecond);
//                                        }
//                                    }
//                                    if (!result.IsSuccess)
//                                    {
//                                        exitCode = -3;
//                                        exitMsg = String.Format("查詢商家代號與最後序號資料失敗，錯誤訊息：{0}", result.Message);
//                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                    }
//                                    #endregion
//                                }
//                                #endregion

//                                #region 發送電文
//                                if (exitCode == 0)
//                                {
//                                    if (schools != null && schools.Count > 0)
//                                    {
//                                        #region 逐筆發送電文，並寫入 CancelDebtsEntity
//                                        int okCount = 0;    //發送電文成功筆數
//                                        int dataCount = 0;  //新增 CancelDebtsEntity 資料筆數
//                                        foreach (KeyValue<int> school in schools)
//                                        {
//                                            #region 為了避免 log 太大，造成 out of memory，處理每筆商家代號前先儲存日誌
//                                            fileLog.WriteLog(log);
//                                            log.Clear();
//                                            #endregion

//                                            CancelDebtsEntity[] datas = null;
//                                            string appNo = school.Key;
//                                            int sTxSeq = school.Value;
//                                            int eTxSeq = sTxSeq + 500;  //一次最多處理500筆，避免電文太大或處理時間過長，導致下一次啟動時重複抓資料
//                                            bool isOK = helper.SendD00I70(appNo, txDay, sTxSeq, eTxSeq, out datas);
//                                            log.AppendFormat("SendD00I70 Log : {0}", helper.Log).AppendLine();
//                                            if (isOK)
//                                            {
//                                                okCount++;

//                                                #region 寫入 CancelDebtsEntity
//                                                if (datas == null || datas.Length == 0)
//                                                {
//                                                    string msg = String.Format("發送 D00I70 (appNo={0}; txDay={1}; sTxSeq={2}; eTxSeq={3}) 電文成功，但無資料", appNo, txDay, sTxSeq, eTxSeq);
//                                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, msg).AppendLine();
//                                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, msg).AppendLine();
//                                                }
//                                                else
//                                                {
//                                                    using (EntityFactory factory = new EntityFactory())
//                                                    {
//                                                        int count = 0;
//                                                        Result result = null;
//                                                        foreach (CancelDebtsEntity data in datas)
//                                                        {
//                                                            #region 檢查資料來源，避免重複新增
//                                                            {
//                                                                Expression where = new Expression(CancelDebtsEntity.Field.FileName, data.FileName)
//                                                                    .And(CancelDebtsEntity.Field.SourceSeq, data.SourceSeq);
//                                                                result = factory.SelectCount<CancelDebtsEntity>(where, out count);
//                                                                if (result.IsSuccess)
//                                                                {
//                                                                    if (count > 0)
//                                                                    {
//                                                                        string msg = String.Format("此筆資料已存在 (FileName={0}; SourceSeq={1})，忽略不處理", data.FileName, data.SourceSeq);
//                                                                        log.AppendLine(msg);
//                                                                        continue;
//                                                                    }
//                                                                }
//                                                                else
//                                                                {
//                                                                    string errmsg = String.Format("查詢資料 (FileName={0}; SourceSeq={1}) 是否重複失敗，錯誤訊息：{2}", data.FileName, data.SourceSeq, result.Message);
//                                                                    log.AppendLine(errmsg);
//                                                                    continue;
//                                                                }
//                                                            }
//                                                            #endregion

//                                                            #region 檢查是否資料已存在 (可能是 D338 或分行上傳 D00I70 新增的)
//                                                            {
//                                                                Expression where = new Expression(CancelDebtsEntity.Field.ReceiveType, data.ReceiveType)
//                                                                    .And(CancelDebtsEntity.Field.CancelNo, data.CancelNo)
//                                                                    .And(CancelDebtsEntity.Field.ReceiveAmount, data.ReceiveAmount)
//                                                                    .And(CancelDebtsEntity.Field.ReceiveDate, data.ReceiveDate)
//                                                                    .And(CancelDebtsEntity.Field.ReceiveTime, data.ReceiveTime)
//                                                                    .And(CancelDebtsEntity.Field.ReceiveWay, data.ReceiveWay)
//                                                                    .And(CancelDebtsEntity.Field.AccountDate, data.AccountDate)
//                                                                    .And(CancelDebtsEntity.Field.ReceiveBank, data.ReceiveBank);
//                                                                result = factory.SelectCount<CancelDebtsEntity>(where, out count);
//                                                                if (result.IsSuccess)
//                                                                {
//                                                                    if (count > 0)
//                                                                    {
//                                                                        string msg = String.Format("此筆資料已存在 (ReceiveType={0}; CancelNo={1}; ReceiveAmount={2}; ReceiveDate={3}; ReceiveTime={4}; ReceiveWay={5}; AccountDate={6}; ReceiveBank={7})，忽略不處理"
//                                                                            , data.ReceiveType, data.CancelNo, data.ReceiveAmount, data.ReceiveDate, data.ReceiveTime, data.ReceiveWay, data.AccountDate, data.ReceiveBank);
//                                                                        log.AppendLine(msg);
//                                                                        continue;
//                                                                    }
//                                                                }
//                                                                else
//                                                                {
//                                                                    string errmsg = String.Format("查詢資料 (ReceiveType={0}; CancelNo={1}; ReceiveAmount={2}; ReceiveDate={3}; ReceiveTime={4}; ReceiveWay={5}; AccountDate={6}; ReceiveBank={7}) 是否重複失敗，錯誤訊息：{8}",
//                                                                        data.ReceiveType, data.CancelNo, data.ReceiveAmount, data.ReceiveDate, data.ReceiveTime, data.ReceiveWay, data.AccountDate, data.ReceiveBank, result.Message);
//                                                                    log.AppendLine(errmsg);
//                                                                    continue;
//                                                                }
//                                                            }
//                                                            #endregion

//                                                            result = factory.Insert(data, out count);
//                                                            if (result.IsSuccess)
//                                                            {
//                                                                dataCount++;
//                                                            }
//                                                            else
//                                                            {
//                                                                string errmsg = String.Format("新增銷帳資料 (ReceiveType={0}; CancelNo={1}; ReceiveAmount={2}; ReceiveDate={3}; ReceiveWay={4}; ReceiveBank={5}; DataSeq={6})失敗，錯誤訊息：{7}"
//                                                                    , data.ReceiveType, data.CancelNo, data.ReceiveAmount, data.ReceiveDate, data.ReceiveWay, data.ReceiveBank, data.SourceSeq, result.Message);
//                                                                log.AppendLine(errmsg);
//                                                            }
//                                                        }
//                                                    }
//                                                }
//                                                #endregion
//                                            }
//                                            else
//                                            {
//                                                string errmsg = String.Format("發送 D00I70 (appNo={0}; txDay={1}; sTxSeq={2}; eTxSeq={3}) 電文失敗，錯誤訊息：{4}", appNo, txDay, sTxSeq, eTxSeq, helper.ErrMsg);
//                                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();
//                                                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();
//                                            }
//                                        }

//                                        exitMsg = String.Format("共 {0} 筆商家代號須處理，發送電文成功 {1} 筆，新增銷帳資料 {2} 筆", schools.Count, okCount, dataCount);
//                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                        #endregion
//                                    }
//                                    else
//                                    {
//                                        exitMsg = "查無任何有效的商家代號資料";
//                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                                    }
//                                }
//                                #endregion
//                                #endregion
//                            }
//                        }
//                        else
//                        {
//                            #region [MDY:20181007] 不重複執行也要有日誌資訊
//                            exitCode = -5;
//                            exitMsg = String.Format("{0} 正在執行，不可重複執行", eaiKind);
//                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
//                            #endregion
//                        }
//                    }
//                }
//                #endregion
//            }
//            catch (Exception ex)
//            {
//                exitCode = -9;
//                exitMsg = String.Format("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
//                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
//                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 發生例外，參數：{1}，錯誤訊息：{2}", DateTime.Now, String.Join(" ", args), ex.Message).AppendLine();
//            }
//            finally
//            {
//                #region 更新 Job
//                if (jobNo > 0)
//                {
//                    string jobResultId = null;
//                    if (exitCode == 0)
//                    {
//                        jobResultId = JobCubeResultCodeTexts.SUCCESS;
//                    }
//                    else
//                    {
//                        jobResultId = JobCubeResultCodeTexts.FAILURE;
//                    }

//                    Result result = jobHelper.UpdateProcessJobToFinsh(jobNo, jobStamp, jobResultId, exitMsg, jobLog.ToString());
//                    if (!result.IsSuccess)
//                    {
//                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 更新批次處理佇列為已完成失敗，{1}", DateTime.Now, result.Message).AppendLine();
//                    }
//                }
//                jobHelper.Dispose();
//                jobHelper = null;
//                #endregion

//                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 結束", DateTime.Now, appName).AppendLine();

//                fileLog.WriteLog(log);
//            }

//            System.Environment.Exit(exitCode);
//        }
        #endregion

        #region [MDY:20191214] (2019擴充案) 重寫並增加 days 參數
        #region 日誌檔物件
        /// <summary>
        /// 日誌檔物件（銷帳處理預期每次資料量都不大，所以日誌內容全部組完再寫入檔案）
        /// </summary>
        private class FileLoger
        {
            #region Member
            /// <summary>
            /// 日誌檔完整路徑
            /// </summary>
            private string LogFileName = null;

            /// <summary>
            /// 日誌內容
            /// </summary>
            private StringBuilder Log = null;
            #endregion

            #region Property
            /// <summary>
            /// 任務編號
            /// </summary>
            public string TaskNo
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌檔名稱
            /// </summary>
            public string LogName
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌檔路徑
            /// </summary>
            public string LogPath
            {
                get;
                private set;
            }

            /// <summary>
            /// 是否為除錯模式
            /// </summary>
            public bool IsDebug
            {
                get;
                private set;
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 空的日誌檔物件
            /// </summary>
            private FileLoger()
            {

            }

            /// <summary>
            /// 建構 日誌檔物件
            /// </summary>
            /// <param name="logName">日誌檔名稱</param>
            /// <param name="path">日誌檔路徑</param>
            /// <param name="isDebug">是否為除錯模式</param>
            private FileLoger(string taskNo, string logName, string logPath, bool isDebug)
            {
                this.TaskNo = taskNo == null ? null : taskNo.Trim();
                this.LogName = logName == null ? String.Empty : logName.Trim();
                this.LogPath = logPath == null ? String.Empty : logPath.Trim();
                this.IsDebug = isDebug;
            }
            #endregion

            #region Private Method
            /// <summary>
            /// 初始化
            /// </summary>
            /// <returns>傳回錯誤訊息</returns>
            private string Initial()
            {
                if (String.IsNullOrEmpty(this.LogName))
                {
                    return "未指定日誌檔路徑，系統將不紀錄日誌";
                }

                string errmsg = null;
                try
                {
                    DirectoryInfo info = new DirectoryInfo(this.LogPath);
                    if (!info.Exists)
                    {
                        info.Create();
                    }

                    if (String.IsNullOrEmpty(this.LogName))
                    {
                        this.LogFileName = Path.Combine(info.FullName, String.Format("{0:yyyyMMdd}.log", DateTime.Today));
                    }
                    else
                    {
                        this.LogFileName = Path.Combine(info.FullName, String.Format("{0}_{1:yyyyMMdd}.log", this.LogName, DateTime.Today));
                    }
                    this.Log = new StringBuilder();
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
                return errmsg;
            }

            /// <summary>
            /// 取得目前日誌內容是否以 NewLine 結尾
            /// </summary>
            /// <returns></returns>
            private bool IsNewLineEnd()
            {
                if (this.Log != null)
                {
                    if (this.Log.Length == 0)
                    {
                        return true;
                    }
                    if (this.Log.Length >= Environment.NewLine.Length)
                    {
                        return (this.Log.ToString(this.Log.Length - Environment.NewLine.Length, Environment.NewLine.Length) == Environment.NewLine);
                    }
                }
                return false;
            }

            /// <summary>
            /// 新增格式化訊息至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="format">訊息格式</param>
            /// <param name="args">訊息參數陣列</param>
            /// <returns>傳回日誌檔物件自己</returns>
            private FileLoger AppendFormat(bool isStart, string format, params object[] args)
            {
                if (this.Log != null && this.LogFileName != null)
                {
                    try
                    {
                        if (isStart)
                        {
                            if (!this.IsNewLineEnd())
                            {
                                this.Log.AppendLine();
                            }
                            this.Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                        }
                        this.Log.AppendFormat(format, args);
                    }
                    catch (Exception ex)
                    {
                        this.Log.AppendFormat("注意：FileLoger.AppendStartFormat() 發生例外，{0}", ex.Message).AppendLine();
                        if (this.IsDebug)
                        {
                            this.Log
                                .AppendFormat("    format 參數：{0}", format).AppendLine()
                                .Append("    args 參數：");
                            foreach (object arg in args)
                            {
                                this.Log.AppendFormat("{0}; ", arg);
                            }
                            this.Log.AppendLine().Append(ex).AppendLine();
                        }
                    }
                }
                return this;
            }

            /// <summary>
            /// 新增訊息至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="arg">訊息</param>
            /// <returns>傳回日誌檔物件自己</returns>
            private FileLoger Append(bool isStart, string arg = null)
            {
                if (this.Log != null && this.LogFileName != null)
                {
                    try
                    {
                        if (!String.IsNullOrWhiteSpace(arg))
                        {
                            if (isStart)
                            {
                                if (!this.IsNewLineEnd())
                                {
                                    this.Log.AppendLine();
                                }
                                this.Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                            }
                            this.Log.Append(arg);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log.AppendFormat("注意：FileLoger.AppendStartFormat() 發生例外，{0}", ex.Message).AppendLine();
                        if (this.IsDebug)
                        {
                            this.Log
                                .AppendFormat("    arg 參數：{0}", arg).AppendLine()
                                .AppendLine().Append(ex).AppendLine();
                        }
                    }
                }
                return this;
            }

            /// <summary>
            /// 新增訊息與換行至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="arg">訊息</param>
            /// <returns>傳回日誌檔物件自己</returns>
            private FileLoger AppendLine(bool isStart, string arg = null)
            {
                if (this.Log != null && this.LogFileName != null)
                {
                    try
                    {
                        if (String.IsNullOrWhiteSpace(arg))
                        {
                            this.Log.AppendLine();
                        }
                        else
                        {
                            if (isStart)
                            {
                                if (!this.IsNewLineEnd())
                                {
                                    this.Log.AppendLine();
                                }
                                this.Log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                            }
                            this.Log.AppendLine(arg);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log.AppendFormat("注意：FileLoger.AppendStartFormat() 發生例外，{0}", ex.Message).AppendLine();
                        if (this.IsDebug)
                        {
                            this.Log
                                .AppendFormat("    arg 參數：{0}", arg).AppendLine()
                                .AppendLine().Append(ex).AppendLine();
                        }
                    }
                }
                return this;
            }
            #endregion

            #region Public Method
            #region 日誌訊息
            /// <summary>
            /// 以新行開始，新增目前日期時間與格式化日誌訊息至日誌內容
            /// </summary>
            /// <param name="format">日誌訊息格式</param>
            /// <param name="args">日誌訊息參數陣列</param>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendFormatStartLog(string format, params object[] args)
            {
                return this.AppendFormat(true, format, args);
            }

            /// <summary>
            /// 新增格式化日誌訊息至日誌內容
            /// </summary>
            /// <param name="format">日誌訊息格式</param>
            /// <param name="args">日誌訊息參數陣列</param>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendFormatLog(string format, params object[] args)
            {
                return this.AppendFormat(false, format, args);
            }

            /// <summary>
            /// 新增日誌訊息至日誌內容
            /// </summary>
            /// <param name="arg">日誌訊息</param>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendLog(string arg)
            {
                return this.Append(false, arg);
            }

            /// <summary>
            /// 以新行開始，新增目前日期時間、日誌訊息與換行至日誌內容
            /// </summary>
            /// <param name="arg">日誌訊息</param>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendLineStartLog(string arg)
            {
                return this.AppendLine(true, arg);
            }

            /// <summary>
            /// 新增日誌訊息與換行至日誌內容
            /// </summary>
            /// <param name="arg">日誌訊息</param>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendLineLog(string arg)
            {
                return this.AppendLine(false, arg);
            }

            /// <summary>
            /// 新增換行的日誌訊息至日誌內容
            /// </summary>
            /// <returns>傳回日誌檔物件自己</returns>
            public FileLoger AppendLineLog()
            {
                return this.AppendLine(false, null);
            }
            #endregion

            #region 除錯訊息
            /// <summary>
            /// 新增格式化除錯訊息至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="format">除錯訊息格式</param>
            /// <param name="args">除錯訊息參數陣列</param>
            /// <returns></returns>
            public FileLoger AppendFormatDebugLog(bool isStart, string format, params object[] args)
            {
                if (this.IsDebug)
                {
                    this.AppendFormat(isStart, format, args);
                }
                return this;
            }

            /// <summary>
            /// 新增除錯訊息至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="arg">除錯訊息</param>
            /// <returns></returns>
            public FileLoger AppendDebugLog(bool isStart, string arg)
            {
                if (this.IsDebug)
                {
                    this.Append(isStart, arg);
                }
                return this;
            }

            /// <summary>
            /// 新增除錯訊息與換行至日誌內容
            /// </summary>
            /// <param name="isStart">是否以新行開始並紀錄目前日期時間</param>
            /// <param name="arg">除錯訊息</param>
            /// <returns></returns>
            public FileLoger AppendLineDebugLog(bool isStart, string arg)
            {
                if (this.IsDebug)
                {
                    this.AppendLine(isStart, arg);
                }
                return this;
            }

            /// <summary>
            /// 新增換行除錯訊息至日誌內容
            /// </summary>
            /// <returns></returns>
            public FileLoger AppendLineDebugLog()
            {
                if (this.IsDebug)
                {
                    this.AppendLine(false, null);
                }
                return this;
            }
            #endregion

            #region 寫檔
            /// <summary>
            /// 將日誌內容寫入日誌檔
            /// </summary>
            /// <param name="jobNo">指定作業編號</param>
            /// <returns>失敗傳回錯誤訊息，否則傳回 null</returns>
            public string WriteLogToFile(string jobNo = null)
            {
                string errmsg = null;
                if (this.LogFileName != null && this.Log != null)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(this.LogFileName, true, Encoding.Default))
                        {
                            if (this.Log.Length == 0)
                            {
                                sw.WriteLine(String.Empty);
                            }
                            else
                            {
                                sw.WriteLine("// [任務：{0}] [作業：{1}] 日誌內容開始 -----------------------------------------------------------------------------------------------\\", this.TaskNo, jobNo);
                                sw.WriteLine(this.Log);
                                sw.WriteLine("\\ [任務：{0}] [作業：{1}] 日誌內容結束 -----------------------------------------------------------------------------------------------//", this.TaskNo, jobNo);
                                sw.WriteLine();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errmsg = ex.Message;
                    }
                    finally
                    {
                        this.Log.Clear();  //寫檔後不管成功失敗一律清空日誌內容
                    }
                }
                return errmsg;
            }
            #endregion
            #endregion

            #region Static Method
            /// <summary>
            /// 建立 日誌檔物件
            /// </summary>
            /// <param name="logName">日誌檔名稱</param>
            /// <param name="logPath">日誌檔路徑</param>
            /// <param name="isDebug">是否啟用除錯模式</param>
            /// <param name="taskNo">任務編號</param>
            /// <returns>失敗則傳回錯誤訊息</returns>
            public static FileLoger Create(string logName, out string errmsg, string logPath = null, bool? isDebug = null, string taskNo = null)
            {
                FileLoger loger = null;

                errmsg = null;
                try
                {
                    if (String.IsNullOrWhiteSpace(logPath))
                    {
                        logPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                    }

                    if (!isDebug.HasValue)
                    {
                        string logMode = ConfigurationManager.AppSettings.Get("LOG_MDOE");
                        if (!String.IsNullOrWhiteSpace(logMode))
                        {
                            isDebug = "DEBUG".Equals(logMode.Trim(), StringComparison.CurrentCultureIgnoreCase);
                        }
                        else
                        {
                            isDebug = false;
                        }
                    }

                    loger = new FileLoger(taskNo, logName, logPath, isDebug.Value);
                    errmsg = loger.Initial();
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }

                if (loger == null)
                {
                    return new FileLoger();
                }
                else
                {
                    return loger;
                }
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// 發送電文主程序
        /// </summary>
        /// <param name="args">執行參數陣列</param>
        /// <remarks>
        /// 執行參數說明
        /// service_id：指定排程作業類別代碼，可指定每日換 Key(DKEY)、D00I70資料下載(D70)、D00I70資料下載(補)(D70B)、D00I70資料下載(C時段)(D70C) 。必要參數
        /// txdate：指定 D70、D70B、D70C 的交易日期，可指定今天（TODAY）、昨天（YESTERDAY）、特定日期（西元年 yyyyMMdd） 。對 D70、D70B、D70C 為必要參數
        /// machine：伺服器名稱清單，以逗號隔開，不指定則使用本機名稱。非必要參數
        /// days：指定 D70B 的回溯天數 (從 txdate 開始往回處理的天數)，可指定 0 ~ 7，限 D70B 有效。非必要參數
        /// </remarks>
        static void Main(string[] args)
        {
            #region 變數初始化
            string appGuid = null;  //執行程式 Guid
            string appName = null;  //執行程式 檔名
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
                appName = Path.GetFileNameWithoutExtension(myAssembly.Location);
            }

            int exitCode = 0;       //執行結果回傳代碼
            string exitMsg = null;

            JobCubeHelper jobHelper = new JobCubeHelper();
            int jobNo = 0;
            string jobStamp = null;
            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = null;        //作業類別代碼 (DKEY、D70、D70B、D70C)
            string jobTypeName = null;      //作業類別名稱
            StringBuilder jobLog = new StringBuilder();

            DateTime startTime = DateTime.Now;
            string fileLogError = null;
            FileLoger fileLog = FileLoger.Create(appName, out fileLogError, taskNo: startTime.ToString("yyyyMMddHHmmss"));
            if (!String.IsNullOrEmpty(fileLogError))
            {
                jobLog.AppendFormat("注意！日誌檔物件建立失敗，{0}", fileLogError);
            }
            #endregion

            #region [MEMO]
            //不提供 retry 因為此程式會執行多次
            //int retryTimes = 5;     //re-try 次數 (預設為5次)
            //int retrySleep = 15;    //re-try 間隔，單位分鐘 (預設為15分鐘)
            #endregion

            #region [MDY:20190506] 資料讀取失敗 retry 參數
            int retryMaxTime = 3;  //retry 最大次數
            int retryIntervalSecond = 20;  //retry 間隔秒數
            #endregion

            try
            {
                fileLog.AppendFormatStartLog("{0} 開始執行，參數：{1}", appName, ((args == null || args.Length == 0) ? null : String.Join(" ", args))).AppendLineLog();

                #region 處理參數
                string eaiKind = null;      //EAI 電文種類
                DateTime? txDate = null;    //D00I70交易日期
                Int32? days = null;         //回溯天數

                #region [MDY:20181007] 不同伺服器使用自己的 CustLoginId，不同 CustLoginId 使用自己的每日的交易 KEY
                List<string> machineNames = new List<string>(4);
                #endregion

                if (exitCode == 0)
                {
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
                                        #region jobTypeId (作業類別代碼)
                                        {
                                            jobTypeId = value;
                                            jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);
                                            switch (jobTypeId)
                                            {
                                                case JobCubeTypeCodeTexts.DKEY:
                                                    eaiKind = EAILogEntity.KIND_CHANGEKEY;
                                                    break;
                                                case JobCubeTypeCodeTexts.D70:
                                                case JobCubeTypeCodeTexts.D70B:
                                                case JobCubeTypeCodeTexts.D70C:
                                                    eaiKind = EAILogEntity.KIND_D0070;
                                                    break;
                                            }
                                            if (String.IsNullOrEmpty(eaiKind) || String.IsNullOrEmpty(jobTypeName))
                                            {
                                                errmsg = "service_id 參數值不是正確的作業類別代碼";
                                            }
                                        }
                                        break;
                                        #endregion

                                    case "txdate":
                                        #region txDate (D00I70交易日期)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            switch (value.ToUpper())
                                            {
                                                case "TODAY":
                                                    txDate = startTime.Date;
                                                    break;
                                                case "YESTERDAY":
                                                    txDate = startTime.Date.AddDays(-1);
                                                    break;
                                                default:
                                                    txDate = DataFormat.ConvertDateText(value);
                                                    if (txDate == null)
                                                    {
                                                        errmsg = "txdate 參數值不是有效的日期格式 (TODAY, YESTERDAY 或 yyyyMMdd 格式)";
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                        #endregion

                                    #region [MDY:20181007] 不同伺服器使用自己的 CustLoginId，不同 CustLoginId 使用自己的每日的交易 KEY
                                    case "machine":
                                        #region machineNames 伺服器名稱清單
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            string[] vals = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (string val in vals)
                                            {
                                                if (!String.IsNullOrWhiteSpace(val))
                                                {
                                                    machineNames.Add(val.Trim());
                                                }
                                            }
                                        }
                                        break;
                                        #endregion
                                    #endregion

                                    case "days":
                                        #region days (處理的天數)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            Int32 val = 0;
                                            if (Int32.TryParse(value, out val) && val >= 0 && val <= 7)
                                            {
                                                days = val;
                                            }
                                            else
                                            {
                                                errmsg = "days 參數值不正確 (0 ~ 7)";
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

                        if (String.IsNullOrEmpty(eaiKind))
                        {
                            lost.Add("service_id");
                        }

                        if (eaiKind == EAILogEntity.KIND_D0070 && !txDate.HasValue)
                        {
                            lost.Add("txdate");
                        }

                        if (lost.Count > 0)
                        {
                            errmsg = String.Format("缺少 {0} 參數", String.Join(", ", lost));
                        }
                    }
                    #endregion

                    #region 檢查配對使用的參數
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        if (jobTypeId == JobCubeTypeCodeTexts.D70B)
                        {
                            if (!days.HasValue)
                            {
                                days = 0;
                            }
                        }
                        else
                        {
                            if (days.HasValue)
                            {
                                errmsg = "service_id 不是 " + JobCubeTypeCodeTexts.D70B + " 時 days 參數不可使用";
                            }
                        }
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -1;  //參數錯誤回傳代碼
                        exitMsg = String.Format("參數錯誤，錯誤訊息：{0}", errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog
                            .AppendLineLog(exitMsg)
                            .AppendLineLog("參數語法：service_id=作業類別代碼(DKEY、D70、D70B、D70C) txdate=D70、D70B、D70C交易日期(TODAY, YESTERDAY 或 yyyyMMdd) [machine=伺服器名稱清單(以逗號隔開，不指定則使用本機名稱)] [days=回溯天數(0~7 限D70B使用)]");
                    }
                }
                #endregion

                #region 取得 EAIHelper
                EAIHelper helper = null;
                if (exitCode == 0)
                {
                    helper = new EAIHelper();
                    if (!helper.IsEAIArgsReady())
                    {
                        exitCode = -1;  //組態參數錯誤回傳代碼
                        exitMsg = "EAI 的執行參數未設定好";
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLineLog(exitMsg);
                    }
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
                        exitCode = -2;  //新增處理作業資料失敗回傳代碼
                        fileLog.AppendFormatLog("新增處理中的批次處理佇列資料失敗，{0}", result.Message).AppendLineLog();
                    }
                }
                #endregion

                #region 發送電文
                if (exitCode == 0)
                {
                    string chkName = "Global\\" + eaiKind + "_" + appGuid;
                    using (Mutex m = new Mutex(false, chkName))    //全域不可重複執行
                    {
                        //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                        if (m.WaitOne(0, false))
                        {
                            if (eaiKind == EAILogEntity.KIND_CHANGEKEY)
                            {
                                #region 換 Key
                                #region [MDY:20181007] 不同伺服器使用自己的 CustLoginId，不同 CustLoginId 使用自己的每日的交易 KEY
                                if (machineNames.Count > 0)
                                {
                                    foreach (string machineName in machineNames)
                                    {
                                        helper = new EAIHelper(machineName);
                                        bool isOK = helper.ChangeDailyKey();
                                        fileLog.AppendFormatLog("({0}) ChangeDailyKey Log : {1}", machineName, helper.Log).AppendLineLog();
                                        if (isOK)
                                        {
                                            exitMsg = String.Concat(machineName, " 換 Key 成功");
                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            fileLog.AppendLineStartLog(exitMsg);
                                        }
                                        else
                                        {
                                            exitCode = -4;  //換 Key 失敗回傳代碼
                                            exitMsg = String.Concat(machineName, " 換 Key 失敗，錯誤訊息：{1}", helper.ErrMsg);
                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            fileLog.AppendLineStartLog(exitMsg);
                                        }
                                    }
                                }
                                else
                                {
                                    bool isOK = helper.ChangeDailyKey();
                                    fileLog.AppendFormatLog("ChangeDailyKey Log : {0}", helper.Log).AppendLineLog();
                                    if (isOK)
                                    {
                                        exitMsg =  String.Concat(Environment.MachineName, " 換 Key 成功");
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                        fileLog.AppendLineStartLog(exitMsg);
                                    }
                                    else
                                    {
                                        exitCode = -4;  //換 Key 失敗回傳代碼
                                        exitMsg = String.Concat(Environment.MachineName, " 換 Key 失敗，錯誤訊息：", helper.ErrMsg);
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                        fileLog.AppendLineStartLog(exitMsg);
                                    }
                                }
                                #endregion
                                #endregion
                            }
                            else if (eaiKind == EAILogEntity.KIND_D0070)
                            {
                                #region D00I70
                                int sleepMillisecond = retryIntervalSecond * 1000;

                                #region [MDY:20170818] M201708_01 取 SC31ReceiveType 系統參數 (20170531_01)
                                List<string> sc31ReceiveTypes = null;
                                if (exitCode == 0)
                                {
                                    #region [MDY:20190506] 失敗則 retry
                                   Result result = null;
                                    Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.SC31_RECEIVETYPE);
                                    for (int retryTimes = 0; retryTimes <= retryMaxTime; retryTimes++)
                                    {
                                        ConfigEntity config = null;
                                        using (EntityFactory factory = new EntityFactory())
                                        {
                                            result = factory.SelectFirst<ConfigEntity>(where, null, out config);
                                        }
                                        if (result.IsSuccess)
                                        {
                                            if (config != null && !String.IsNullOrWhiteSpace(config.ConfigValue))
                                            {
                                                string[] values = config.ConfigValue.Trim().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                sc31ReceiveTypes = new List<string>(values.Length);
                                                for (int idx = 0; idx < values.Length; idx++)
                                                {
                                                    string value = values[idx].Trim();
                                                    if (!String.IsNullOrEmpty(value) && !sc31ReceiveTypes.Contains(value))
                                                    {
                                                        sc31ReceiveTypes.Add(value);
                                                    }
                                                }
                                            }
                                            if (sc31ReceiveTypes == null)
                                            {
                                                sc31ReceiveTypes = new List<string>(0);
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            fileLog.AppendFormatStartLog(" 第 {0} 次查詢學校銷帳檔3合1的商家代號系統參失敗，錯誤訊息：{1}", retryTimes, result.Message).AppendLineLog();
                                            Thread.Sleep(sleepMillisecond);
                                        }
                                    }
                                    if (!result.IsSuccess)
                                    {
                                        exitCode = -3;  //D00I70 失敗回傳代碼
                                        exitMsg = String.Format("查詢學校銷帳檔3合1的商家代號系統參失敗，錯誤訊息：{0}", result.Message);
                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                        fileLog.AppendLineStartLog(exitMsg);
                                    }
                                    #endregion
                                }
                                #endregion

                                DateTime chkDate = txDate.Value;
                                DateTime endDate = jobTypeId == JobCubeTypeCodeTexts.D70B && days.Value > 0 ? chkDate.AddDays(days.Value * -1) : chkDate;
                                while (chkDate >= endDate)
                                {
                                    string txDay = Common.GetTWDate8(chkDate);

                                    #region 取得所有有效的商家代號與下筆查詢序號清單
                                    KeyValueList<int> schools = null;
                                    if (exitCode == 0)
                                    {
                                        #region [MDY:20190506] 失敗則 retry
                                        Result result = null;
                                        string sql = @"
SELECT Receive_Type
     , (SELECT ISNULL(MAX([Source_Seq]), 0) + 1 FROM [Cancel_Debts] AS CD WHERE CD.[Receive_Type] = SR.[Receive_Type] AND CD.[File_Name] = 'D00I70_' + SR.[Receive_Type] + '_' + @TXDAY) AS NEXT_SEQ
  FROM School_Rtype AS SR 
 WHERE status = @NormalStatus".Trim();
                                        KeyValue[] parameters = new KeyValue[] { new KeyValue("@NormalStatus", DataStatusCodeTexts.NORMAL), new KeyValue("@TXDAY", txDay) };
                                        for (int retryTimes = 0; retryTimes <= retryMaxTime; retryTimes++)
                                        {
                                            System.Data.DataTable dt = null;
                                            using (EntityFactory factory = new EntityFactory())
                                            {
                                                result = factory.GetDataTable(sql, parameters, 0, 0, out dt);
                                            }
                                            if (result.IsSuccess)
                                            {
                                                if (dt != null && dt.Rows.Count > 0)
                                                {
                                                    schools = new KeyValueList<int>(dt.Rows.Count);
                                                    foreach (System.Data.DataRow drow in dt.Rows)
                                                    {
                                                        string appNo = drow["Receive_Type"].ToString().Trim();

                                                        #region [MDY:20170818] M201708_01 排除 SC31ReceiveType 的商家代號 (20170531_01)
                                                        if (sc31ReceiveTypes != null && sc31ReceiveTypes.Contains(appNo))
                                                        {
                                                            continue;
                                                        }
                                                        #endregion

                                                        int sTxSeq = Convert.ToInt32(drow["NEXT_SEQ"]);
                                                        schools.Add(appNo, sTxSeq);
                                                    }
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                fileLog.AppendFormatStartLog(" 第 {0} 次查詢商家代號與最後序號資料失敗，錯誤訊息：{1}",  retryTimes, result.Message).AppendLineLog();
                                                Thread.Sleep(sleepMillisecond);
                                            }
                                        }
                                        if (!result.IsSuccess)
                                        {
                                            exitCode = -3;
                                            exitMsg = String.Format("查詢商家代號與最後序號資料失敗，錯誤訊息：{0}", result.Message);
                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            fileLog.AppendLineStartLog(exitMsg);
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region 發送電文
                                    if (exitCode == 0)
                                    {
                                        if (schools != null && schools.Count > 0)
                                        {
                                            #region 為了避免 log 太大，造成 out of memory，處理每筆商家代號前先儲存日誌
                                            fileLogError = fileLog.WriteLogToFile(jobNo.ToString());
                                            #endregion

                                            #region 逐商家代號發送電文，並寫入 CancelDebtsEntity
                                            int okCount = 0;    //發送電文成功筆數
                                            int dataCount = 0;  //新增 CancelDebtsEntity 資料筆數
                                            foreach (KeyValue<int> school in schools)
                                            {
                                                CancelDebtsEntity[] datas = null;
                                                string appNo = school.Key;
                                                int sTxSeq = school.Value;
                                                int eTxSeq = sTxSeq + 500;  //一次最多處理500筆，避免電文太大或處理時間過長，導致下一次啟動時重複抓資料
                                                bool isOK = helper.SendD00I70(appNo, txDay, sTxSeq, eTxSeq, out datas);
                                                fileLog.AppendFormatLog("SendD00I70 Log : {0}", helper.Log).AppendLineLog();
                                                if (isOK)
                                                {
                                                    okCount++;

                                                    #region 寫入 CancelDebtsEntity
                                                    if (datas == null || datas.Length == 0)
                                                    {
                                                        string msg = String.Format("發送 D00I70 (appNo={0}; txDay={1}; sTxSeq={2}; eTxSeq={3}) 電文成功，但無資料", appNo, txDay, sTxSeq, eTxSeq);
                                                        jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, msg).AppendLine();
                                                        fileLog.AppendLineStartLog(msg);
                                                    }
                                                    else
                                                    {
                                                        using (EntityFactory factory = new EntityFactory())
                                                        {
                                                            int count = 0;
                                                            Result result = null;
                                                            foreach (CancelDebtsEntity data in datas)
                                                            {
                                                                #region 檢查資料來源，避免重複新增
                                                                {
                                                                    Expression where = new Expression(CancelDebtsEntity.Field.FileName, data.FileName)
                                                                        .And(CancelDebtsEntity.Field.SourceSeq, data.SourceSeq);
                                                                    result = factory.SelectCount<CancelDebtsEntity>(where, out count);
                                                                    if (result.IsSuccess)
                                                                    {
                                                                        if (count > 0)
                                                                        {
                                                                            string msg = String.Format("此筆資料已存在 (FileName={0}; SourceSeq={1})，忽略不處理", data.FileName, data.SourceSeq);
                                                                            fileLog.AppendLineLog(msg);
                                                                            continue;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        string errmsg = String.Format("查詢資料 (FileName={0}; SourceSeq={1}) 是否重複失敗，錯誤訊息：{2}", data.FileName, data.SourceSeq, result.Message);
                                                                        fileLog.AppendLineLog(errmsg);
                                                                        continue;
                                                                    }
                                                                }
                                                                #endregion

                                                                #region 檢查是否資料已存在 (可能是 D338 或分行上傳 D00I70 新增的)
                                                                {
                                                                    Expression where = new Expression(CancelDebtsEntity.Field.ReceiveType, data.ReceiveType)
                                                                        .And(CancelDebtsEntity.Field.CancelNo, data.CancelNo)
                                                                        .And(CancelDebtsEntity.Field.ReceiveAmount, data.ReceiveAmount)
                                                                        .And(CancelDebtsEntity.Field.ReceiveDate, data.ReceiveDate)
                                                                        .And(CancelDebtsEntity.Field.ReceiveTime, data.ReceiveTime)
                                                                        .And(CancelDebtsEntity.Field.ReceiveWay, data.ReceiveWay)
                                                                        .And(CancelDebtsEntity.Field.AccountDate, data.AccountDate)
                                                                        .And(CancelDebtsEntity.Field.ReceiveBank, data.ReceiveBank);
                                                                    result = factory.SelectCount<CancelDebtsEntity>(where, out count);
                                                                    if (result.IsSuccess)
                                                                    {
                                                                        if (count > 0)
                                                                        {
                                                                            string msg = String.Format("此筆資料已存在 (ReceiveType={0}; CancelNo={1}; ReceiveAmount={2}; ReceiveDate={3}; ReceiveTime={4}; ReceiveWay={5}; AccountDate={6}; ReceiveBank={7})，忽略不處理"
                                                                                , data.ReceiveType, data.CancelNo, data.ReceiveAmount, data.ReceiveDate, data.ReceiveTime, data.ReceiveWay, data.AccountDate, data.ReceiveBank);
                                                                            fileLog.AppendLineLog(msg);
                                                                            continue;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        string errmsg = String.Format("查詢資料 (ReceiveType={0}; CancelNo={1}; ReceiveAmount={2}; ReceiveDate={3}; ReceiveTime={4}; ReceiveWay={5}; AccountDate={6}; ReceiveBank={7}) 是否重複失敗，錯誤訊息：{8}",
                                                                            data.ReceiveType, data.CancelNo, data.ReceiveAmount, data.ReceiveDate, data.ReceiveTime, data.ReceiveWay, data.AccountDate, data.ReceiveBank, result.Message);
                                                                        fileLog.AppendLineLog(errmsg);
                                                                        continue;
                                                                    }
                                                                }
                                                                #endregion

                                                                result = factory.Insert(data, out count);
                                                                if (result.IsSuccess)
                                                                {
                                                                    dataCount++;
                                                                }
                                                                else
                                                                {
                                                                    string errmsg = String.Format("新增銷帳資料 (ReceiveType={0}; CancelNo={1}; ReceiveAmount={2}; ReceiveDate={3}; ReceiveWay={4}; ReceiveBank={5}; DataSeq={6})失敗，錯誤訊息：{7}"
                                                                        , data.ReceiveType, data.CancelNo, data.ReceiveAmount, data.ReceiveDate, data.ReceiveWay, data.ReceiveBank, data.SourceSeq, result.Message);
                                                                    fileLog.AppendLineLog(errmsg);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    string errmsg = String.Format("發送 D00I70 (appNo={0}; txDay={1}; sTxSeq={2}; eTxSeq={3}) 電文失敗，錯誤訊息：{4}", appNo, txDay, sTxSeq, eTxSeq, helper.ErrMsg);
                                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();
                                                    fileLog.AppendLineStartLog(errmsg);
                                                }

                                                #region 為了避免 log 太大，造成 out of memory，處理每筆商家代號後先儲存日誌
                                                fileLogError = fileLog.WriteLogToFile(jobNo.ToString());
                                                #endregion
                                            }

                                            exitMsg = String.Format("共 {0} 筆商家代號須處理，發送電文成功 {1} 筆，新增銷帳資料 {2} 筆", schools.Count, okCount, dataCount);
                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            fileLog.AppendLineStartLog(exitMsg);
                                            #endregion
                                        }
                                        else
                                        {
                                            exitMsg = "查無任何有效的商家代號資料";
                                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                                            fileLog.AppendLineStartLog(exitMsg);
                                        }
                                    }
                                    #endregion

                                    chkDate = chkDate.AddDays(-1);
                                }
                                #endregion
                            }

                            #region [MDY:20210401] 原碼修正
                            m.ReleaseMutex();
                            #endregion
                        }
                        else
                        {
                            #region [MDY:20181007] 不重複執行也要有日誌資訊
                            exitCode = -5;  //不重複執行回傳代碼
                            exitMsg = String.Format("{0} 正在執行，不可重複執行", eaiKind);
                            jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, exitMsg).AppendLine();
                            fileLog.AppendLineStartLog(exitMsg);
                            #endregion
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;  //發生例外回傳代碼
                exitMsg = String.Format("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
                fileLog.AppendFormatStartLog("{0} 發生例外，錯誤訊息：{1}", jobTypeName, ex.Message).AppendLineLog();
                fileLog.AppendFormatDebugLog(false, "[DEBUG] 例外訊息：{0}", ex).AppendLineDebugLog();
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
                        fileLog.AppendFormatStartLog("更新批次處理佇列為已完成失敗，{0}", result.Message).AppendLineLog();
                    }
                }
                jobHelper.Dispose();
                jobHelper = null;
                #endregion

                fileLog.AppendFormatStartLog("{0} 執行結束", appName).AppendLineLog();
                fileLogError = fileLog.WriteLogToFile(jobNo.ToString());
            }

            System.Environment.Exit(exitCode);
        }
        #endregion
    }
}
