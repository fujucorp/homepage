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

namespace UpD38Data
{
    /// <summary>
    /// D38資料上傳
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

        static void Main(string[] args)
        {
            #region Initial
            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);

            FileLoger fileLog = new FileLoger(appName);
            string jobTypeId = JobCubeTypeCodeTexts.D38;                        //作業類別代碼
            string jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);       //作業類別名稱
            int exitCode = 0;
            #endregion

            StringBuilder log = new StringBuilder();

            //不提供 retry 因為此程式處理的作業是由使用者新增
            //int retryTimes = 5;     //re-try 次數 (預設為5次)
            //int retrySleep = 5;     //re-try 間隔，單位分鐘 (預設為5分鐘)

            DateTime startTime = DateTime.Now;

            int jobNo = 0;
            string jobStamp = null;
            StringBuilder jobMsg = new StringBuilder();
            StringBuilder jobLog = new StringBuilder();

            try
            {
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 開始", DateTime.Now, appName).AppendLine();

                #region 處理參數
                //無參數
                #endregion

                #region 取得一筆 D38 待處理的 job
                JobcubeEntity job = null;
                {
                    Result result = null;
                    using (JobCubeHelper jobHelper = new JobCubeHelper())
                    {
                        result = jobHelper.GetWaitJobToProcess(jobTypeId, out job);
                    }
                    if (result.IsSuccess)
                    {
                        if (job == null)
                        {
                            log.AppendFormat("查無待處理的 {0} 批次處理佇列資料", jobTypeId).AppendLine();
                        }
                        else
                        {
                            jobNo = job.Jno;
                            jobStamp = job.Memo;
                        }
                    }
                    else
                    {
                        exitCode = -2;
                        log.AppendFormat("查詢待處理的 {0} 批次處理佇列資料發生錯誤，錯誤訊息：{1}", jobTypeId, result.Message).AppendLine();
                    }
                }
                #endregion

                #region 處理 D38 作業
                if (job != null && exitCode == 0)
                {
                    #region 處理 job 參數
                    string updKind = null;
                    string receiveType = null;
                    string yearId = null;
                    string termId = null;
                    string depId = null;
                    string receiveId = null;
                    string qType = null;
                    string qValue = null;
                    bool isOK = JobcubeEntity.ParseD38Parameter(job.Jparam, out updKind, out receiveType, out yearId, out termId, out depId, out receiveId, out qType, out qValue);
                    if (isOK)
                    {
                        isOK = ((updKind == JobcubeEntity.D38UpdKind_Update || updKind == JobcubeEntity.D38UpdKind_Delete)
                            && !String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) && !String.IsNullOrEmpty(yearId)
                            && !String.IsNullOrEmpty(receiveId) && !String.IsNullOrEmpty(qType));
                    }
                    if (!isOK)
                    {
                        exitCode = -2;
                        string errmsg = "工作參數不正確，處理失敗";
                        log.AppendLine(errmsg);
                        if (jobNo > 0)
                        {
                            jobMsg.AppendLine(errmsg);
                            jobLog.AppendLine(errmsg);
                        }
                    }
                    #endregion

                    #region 處理 job
                    if (isOK)
                    {
                        Result result = new Result(true);

                        #region 取得要處理的資料
                        SchoolRTypeEntity school = null;
                        StudentReceiveView[] receives = null;
                        if (result.IsSuccess)
                        {
                            using (EntityFactory factory = new EntityFactory())
                            {
                                #region SchoolRTypeEntity
                                {
                                    Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                                    result = factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                                    if (result.IsSuccess && school == null)
                                    {
                                        result = new Result(false, String.Format("查無 {0} 的學校資料", receiveType), ErrorCode.D_DATA_NOT_FOUND, null);
                                    }
                                }
                                #endregion

                                #region 取得虛擬帳號模組資料
                                CancelNoHelper.Module module = null;
                                if (result.IsSuccess)
                                {
                                    CancelNoHelper cnoHelper = new CancelNoHelper();
                                    module = cnoHelper.GetModuleById(school.CancelNoRule);
                                    if (module == null)
                                    {
                                        result = new Result(false, String.Format("{0} 未設定檢碼方式或設定值不正確", receiveType), ErrorCode.D_DATA_NOT_FOUND, null);
                                    }
                                    //else if (!module.IsD38Kind)
                                    //{
                                    //    result = new Result(false, String.Format("{0} 無須做 D38 上傳 (檢碼方式不為 9)", receiveType), CoreStatusCode.UNKNOWN_ERROR, null);
                                    //}
                                }
                                #endregion

                                #region StudentReceiveView
                                if (result.IsSuccess)
                                {
                                    Expression where = new Expression(StudentReceiveView.Field.ReceiveType, receiveType)
                                        .And(StudentReceiveView.Field.YearId, yearId)
                                        .And(StudentReceiveView.Field.TermId, termId)
                                        .And(StudentReceiveView.Field.DepId, depId)
                                        .And(StudentReceiveView.Field.ReceiveId, receiveId)
                                        .And(StudentReceiveView.Field.CancelNo, RelationEnum.NotEqual, String.Empty)
                                        .And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);

                                    switch (qType)
                                    {
                                        case "1":   //所有繳費單
                                            break;
                                        case "2":   //自訂產生繳費單流水號
                                            #region 自訂產生繳費單流水號
                                            {
                                                string[] snArgs = qValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                                if (args.Length == 2)
                                                {
                                                    where
                                                        .And(StudentReceiveView.Field.SeriorNo, RelationEnum.GreaterEqual, snArgs[0].PadLeft(module.SeriorNoSize, '0'))
                                                        .And(StudentReceiveView.Field.SeriorNo, RelationEnum.LessEqual, snArgs[1].PadLeft(module.SeriorNoSize, '0'));
                                                }
                                                else
                                                {
                                                    result = new Result(false, String.Format("工作參數不正確，無效的 qValue ({0}) 值", qValue), ErrorCode.S_INVALID_PARAMETER, null);
                                                }
                                            }
                                            #endregion
                                            break;
                                        case "3":   //依批號產生
                                            #region 依批號產生
                                            {
                                                int upNo = 0;
                                                if (int.TryParse(qValue, out upNo) && upNo >= 0)
                                                {
                                                    where.And(StudentReceiveView.Field.UpNo, upNo);
                                                }
                                                else
                                                {
                                                    result = new Result(false, String.Format("工作參數不正確，無效的 qValue ({0}) 值", qValue), ErrorCode.S_INVALID_PARAMETER, null);
                                                }
                                            }
                                            #endregion
                                            break;
                                        case "4":   //依學號產生
                                            where.And(StudentReceiveView.Field.StuId, qValue);
                                            break;
                                        default:
                                            result = new Result(false, String.Format("工作參數不正確，無效的 qType ({0}) 值", qType), ErrorCode.S_INVALID_PARAMETER, null);
                                            break;
                                    }

                                    if (result.IsSuccess)
                                    {
                                        #region [MDY:20160710] 上傳只處理未繳資料，刪除只處理已上傳過的
                                        #region [Old]
                                        //#region [MDY:20160307] 增加處理類型 (上傳或刪除) 參數
                                        //if (updKind == JobcubeEntity.D38UpdKind_Delete)
                                        //{
                                        //    //刪除中心資料只需取已上傳過的
                                        //    where.And(StudentReceiveView.Field.Exportreceivedata, "Y");
                                        //}
                                        //#endregion
                                        #endregion

                                        if (updKind == JobcubeEntity.D38UpdKind_Delete)
                                        {
                                            //刪除中心資料只取已上傳過的
                                            where.And(StudentReceiveView.Field.Exportreceivedata, "Y");
                                        }
                                        else
                                        {
                                            //上傳資料至中心只取未繳資料
                                            where
                                                .And(new Expression(StudentReceiveView.Field.ReceiveWay, String.Empty).Or(StudentReceiveView.Field.ReceiveWay, null))
                                                .And(new Expression(StudentReceiveView.Field.ReceiveDate, String.Empty).Or(StudentReceiveView.Field.ReceiveDate, null))
                                                .And(new Expression(StudentReceiveView.Field.AccountDate, String.Empty).Or(StudentReceiveView.Field.AccountDate, null));
                                        }
                                        #endregion

                                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                        orderbys.Add(StudentReceiveView.Field.UpNo, OrderByEnum.Asc);
                                        orderbys.Add(StudentReceiveView.Field.UpOrder, OrderByEnum.Asc);
                                        result = factory.SelectAll<StudentReceiveView>(where, orderbys, out receives);
                                    }
                                }
                                #endregion
                            }

                            if (!result.IsSuccess)
                            {
                                string errmsg = String.Format("讀取要處理的資料失敗，錯誤訊息：{0}", result.Message);
                                log.AppendLine(errmsg);
                                if (jobNo > 0)
                                {
                                    jobMsg.AppendLine(errmsg);
                                    jobLog.AppendLine(errmsg);
                                }
                            }
                        }
                        #endregion

                        #region 處理資料
                        if (result.IsSuccess)
                        {
                            if (receives == null || receives.Length == 0)
                            {
                                string errmsg = "無任何要處理的學生繳費資料";
                                log.AppendLine(errmsg);
                                if (jobNo > 0)
                                {
                                    jobMsg.AppendLine(errmsg);
                                    jobLog.AppendLine(errmsg);
                                }
                            }
                            else
                            {
                                #region 取得 EAIHelper
                                #region [MDY:20181007] 使用新的 EAIHelper (一律取自專案組態的參數)
                                #region [OLD]
                                //EAIHelper eaiHelper = new EAIHelper(true);
                                #endregion

                                EAIHelper eaiHelper = new EAIHelper();
                                #endregion

                                {
                                    if (!eaiHelper.IsEAIArgsReady())
                                    {
                                        result = new Result(false, "EAI 的執行參數未設定好", CoreStatusCode.UNKNOWN_ERROR, null);
                                    }
                                    else
                                    {
                                        if (!eaiHelper.IsEAIDailyKeyReady())
                                        {
                                            if (!eaiHelper.ChangeDailyKey())
                                            {
                                                result = new Result(false, "換 Key 失敗，錯誤訊息：" + eaiHelper.ErrMsg, CoreStatusCode.UNKNOWN_ERROR, null);
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region 發 D38 電文
                                if (result.IsSuccess)
                                {
                                    string upd = "I";   //I:新增 U:異動 D:刪除
                                    string schno = school.SchIdenty.Trim();
                                    string actno = school.SchAccount.Trim();
                                    string payno = String.Empty;

                                    int totalCount = receives.Length;
                                    int okCount = 0;

                                    log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 準備發送電文 (共 {1} 筆資料)", DateTime.Now, totalCount).AppendLine();

                                    #region 逐筆發送 D38 並寫入 D38Data
                                    using (EntityFactory factory = new EntityFactory())
                                    {
                                        foreach (StudentReceiveView receive in receives)
                                        {
                                            #region [MDY:20160307] 增加處理類型 (上傳或刪除) 參數
                                            if (updKind == JobcubeEntity.D38UpdKind_Delete)
                                            {
                                                upd = "D";
                                            }
                                            else
                                            {
                                                upd = (receive.Exportreceivedata == "Y") ? "U" : "I";
                                            }
                                            #endregion

                                            #region SendD38
                                            string rqXml = null;
                                            string rsXml = null;
                                            string replycode = null;
                                            string d38Result = null;
                                            bool isD38OK = eaiHelper.SendD38(upd, schno, actno, payno, receive.StuId, receive.StuName, receive.CancelNo, receive.ReceiveAmount.Value, out rqXml, out rsXml, out replycode, out d38Result);
                                            if (isD38OK)
                                            {
                                                okCount++;
                                                //log.AppendFormat("Call SendD38() 成功，replycode={0}\r\nrqXml={1}\r\nrsXml={2}", replycode, rqXml, rsXml).AppendLine();
                                            }
                                            else
                                            {
                                                log.AppendFormat("Call SendD38() 失敗，錯誤訊息：{0}\r\nreplycode={1}\r\nrqXml={2}\r\nrsXml={3}", eaiHelper.ErrMsg, replycode, rqXml, rsXml).AppendLine();
                                                if (jobNo > 0)
                                                {
                                                    jobLog.AppendFormat("上傳 (CancelNo={0}) 資料失敗 (replycode={1})，錯誤訊息：{2}", receive.CancelNo, replycode, eaiHelper.ErrMsg).AppendLine();
                                                }
                                            }
                                            #endregion

                                            #region 新增 D38DataEntity
                                            D38DataEntity data = new D38DataEntity();
                                            data.UpdFlag = upd;
                                            data.SchIdenty = schno;
                                            data.ReceiveType = receive.ReceiveType;
                                            data.CancelNo = receive.CancelNo;
                                            data.ReceiveAmount = receive.ReceiveAmount.Value;
                                            data.StuId = receive.StuId;
                                            data.StuName = receive.StuName;
                                            data.UpdData = rqXml;
                                            data.UpdDate = DateTime.Now;
                                            data.JobNo = jobNo;
                                            data.Status = replycode ?? String.Empty;
                                            data.CrtDate = data.UpdDate.Value;
                                            data.CrtUser = job.Jowner;
                                            data.Result = d38Result ?? String.Empty;

                                            int count = 0;
                                            result = factory.Insert(data, out count);
                                            if (!result.IsSuccess)
                                            {
                                                log.AppendFormat("新增 D38 資料 (CancelNo={0}) 失敗，錯誤訊息：{1}", data.CancelNo, result.Message).AppendLine();
                                                //if (jobNo > 0)
                                                //{
                                                //    jobLog.AppendFormat("新增 D38 資料 (CancelNo={0}) 失敗，錯誤訊息：{1}", data.CancelNo, result.Message).AppendLine();
                                                //}
                                            }
                                            #endregion

                                            #region 更新 StudentReceiveEntity.Exportreceivedata 欄位
                                            if (isD38OK)
                                            {
                                                string newExportReceiveData = null;
                                                if (data.UpdFlag == "D")
                                                {
                                                    //做刪除處理，一律更新為 N
                                                    newExportReceiveData = "N";
                                                }
                                                else if (receive.Exportreceivedata != "Y")
                                                {
                                                    //非刪除處理且 Exportreceivedata 非 Y，則更新為 Y
                                                    newExportReceiveData = "Y";
                                                }
                                                if (!String.IsNullOrEmpty(newExportReceiveData))
                                                {
                                                    #region [MDY:20170316] 修正更新 Exportreceivedata 錯誤的問題
                                                    Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receive.ReceiveType)
                                                        .And(StudentReceiveEntity.Field.YearId, receive.YearId)
                                                        .And(StudentReceiveEntity.Field.TermId, receive.TermId)
                                                        .And(StudentReceiveEntity.Field.DepId, receive.DepId)
                                                        .And(StudentReceiveEntity.Field.ReceiveId, receive.ReceiveId)
                                                        .And(StudentReceiveEntity.Field.StuId, receive.StuId)
                                                        .And(StudentReceiveEntity.Field.OldSeq, receive.OldSeq);
                                                    KeyValue[] feildValues = new KeyValue[] { new KeyValue(StudentReceiveEntity.Field.Exportreceivedata, newExportReceiveData) };
                                                    #endregion

                                                    result = factory.UpdateFields<StudentReceiveEntity>(feildValues, where, out count);
                                                    if (!result.IsSuccess)
                                                    {
                                                        log.AppendFormat("更新 Exportreceivedata 資料 (CancelNo={0}) 失敗，錯誤訊息：{1}", data.CancelNo, result.Message).AppendLine();
                                                        //if (jobNo > 0)
                                                        //{
                                                        //    jobLog.AppendFormat("更新 Exportreceivedata 資料 (CancelNo={0}) 失敗，錯誤訊息：{1}", data.CancelNo, result.Message).AppendLine();
                                                        //}
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion

                                    string summary = String.Format("成功 {0} 筆，失敗 {1} 筆", okCount, totalCount - okCount);
                                    log.AppendFormat(summary).AppendLine();
                                    if (jobNo > 0)
                                    {
                                        jobMsg.AppendLine(summary);
                                        jobLog.AppendLine(summary);
                                    }
                                }
                                else
                                {
                                    string errmsg = String.Format("發送電文失敗，錯誤訊息：{0}", result.Message);
                                    log.AppendLine(errmsg);
                                    if (jobNo > 0)
                                    {
                                        jobMsg.AppendLine(errmsg);
                                        jobLog.AppendLine(errmsg);
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion

                        if (!result.IsSuccess)
                        {
                            exitCode = -3;
                        }
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 發生例外，參數：{1}，錯誤訊息：{2}", DateTime.Now, String.Join(" ", args), ex.Message).AppendLine();
                if (jobNo > 0)
                {
                    jobMsg.AppendFormat("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
                }
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
                        jobMsg.Append(JobCubeResultCodeTexts.SUCCESS_TEXT);
                    }
                    else
                    {
                        jobResultId = JobCubeResultCodeTexts.FAILURE;
                    }

                    using (JobCubeHelper jobHelper = new JobCubeHelper())
                    {
                        Result result = jobHelper.UpdateProcessJobToFinsh(jobNo, jobStamp, jobResultId, jobMsg.ToString(), log.ToString());
                        if (!result.IsSuccess)
                        {
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 更新批次處理佇列為已完成失敗，{1}", DateTime.Now, result.Message).AppendLine();
                        }
                    }
                }
                #endregion

                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 結束", DateTime.Now, appName).AppendLine();

                fileLog.WriteLog(log);
            }

            System.Environment.Exit(exitCode);
            return;
        }
    }
}
