using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 批次處理佇列檢查模式列舉
    /// </summary>
    public enum JobCubeCheckMode
    {
        /// <summary>
        /// 不檢查
        /// </summary>
        Skip = 0,
        /// <summary>
        /// 檢查到時間 (同一分鐘只有一個 job 在處理)
        /// </summary>
        ByTime = 1,
        /// <summary>
        /// 檢查到日期 (同一天只有一個 job 在處理)
        /// </summary>
        ByDate = 2
    }

    /// <summary>
    /// JobCube 處理工具類別
    /// </summary>
    public sealed class JobCubeHelper : IDisposable
    {
        #region Member
        /// <summary>
        /// 儲存 資料存取物件 的變數
        /// </summary>
        private EntityFactory _Factory = null;

        /// <summary>
        /// 儲存 此物件 Dispose 時 資料存取物件 是否需要一併 Dispose
        /// </summary>
        private bool _IsNeedFactoryDispose = false;
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 JobCube 處理工具類別
        /// </summary>
        /// <param name="factory"></param>
        public JobCubeHelper(EntityFactory factory)
        {
            _Factory = factory;
            _IsNeedFactoryDispose = false;
        }

        /// <summary>
        /// 建構 JobCube 處理工具類別
        /// </summary>
        public JobCubeHelper()
            : this(new EntityFactory())
        {
            //_Factory = new EntityFactory();
            _IsNeedFactoryDispose = true;
        }
        #endregion

        #region Destructor
        /// <summary>
        /// 解構匯入檔案處理工具類別
        /// </summary>
        ~JobCubeHelper()
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
                    if (_Factory != null && _IsNeedFactoryDispose)
                    {
                        _Factory.Dispose();
                        _Factory = null;
                    }
                }
                _Disposed = true;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得此物件是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false。</returns>
        public bool IsReady()
        {
            return (_Factory != null && _Factory.IsReady());
        }
        #endregion

        #region [MDY:20160312] 新增批次處理佇列資料相關
        /// <summary>
        /// 新增指定批次處理佇列資料
        /// </summary>
        /// <param name="job">指定批次處理佇列資料</param>
        /// <returns>傳回處理結果</returns>
        public Result InsertJob(JobcubeEntity job)
        {
            #region 檢查 IsReady
            if (!this.IsReady())
            {
                return new Result(false, "資料存取物件未準備好", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job == null)
            {
                return new Result(false, "未指定作業參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            else if (job.Jno > 0 || String.IsNullOrEmpty(job.Jtypeid) || JobCubeStatusCodeTexts.GetCodeText(job.Jstatusid) == null)
            {
                return new Result(false, "作業參數不正確", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (JobCubeStatusCodeTexts.IsEnding(job.Jstatusid) && String.IsNullOrEmpty(job.Jpid))
            {
                job.Jpid = Environment.MachineName;
            }
            if (String.IsNullOrEmpty(job.Jowner))
            {
                job.Jowner = "SYSTEM";
            }
            if (job.CDate == null)
            {
                job.CDate = DateTime.Now;
            }
            #endregion

            int count = 0;
            Result result = _Factory.Insert(job, out count);
            if (result.IsSuccess && count == 0)
            {
                result = new Result(false, "無資料被新增", CoreStatusCode.D_NOT_DATA_INSERT, null);
            }
            return result;
        }

        /// <summary>
        /// 新增符合檢查模式的處理中的批次處理佇列資料
        /// </summary>
        /// <param name="job">指定批次處理佇列資料</param>
        /// <param name="checkMode">指定批次處理佇列檢查模式</param>
        /// <returns>傳回處理結果</returns>
        public Result InsertProcessJob(ref JobcubeEntity job, JobCubeCheckMode checkMode = JobCubeCheckMode.Skip)
        {
            #region 檢查 IsReady
            if (!this.IsReady())
            {
                return new Result(false, "資料存取物件未準備好", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job == null)
            {
                return new Result(false, "未指定批次處理佇列資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            else if (job.Jno > 0 || String.IsNullOrEmpty(job.Jtypeid))
            {
                return new Result(false, "批次處理佇列資料參數不正確", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrEmpty(job.Jpid))
            {
                job.Jpid = Environment.MachineName;
            }
            if (String.IsNullOrEmpty(job.Jowner))
            {
                job.Jowner = "SYSTEM";
            }
            string stamp = Common.GetGUID();
            #endregion

            #region 組 SQL
            #region SQL SAMPLE
            //DECLARE @START_TIME datetime, @CHK_TIME datetime;
            //SET @START_TIME = GETDATE();
            //SET @CHK_TIME = CONVERT(datetime, CONVERT(varchar(16), DATEADD(MINUTE, -1, @START_TIME), 120));

            //INSERT INTO [JobCube] ([JSTD], [JETD], [JDLL], [JCLASS], [JMETHOD]
            //     , [JPARAM], [JPID], [JOWNER]
            //     , [JRID], [JYEAR], [JTERM], [JDEP], [JRECID], [JPRITY]
            //     , [JTYPEID], [JSTATUSID], [JRESULTID], [JLOG]
            //     , [C_Date], [M_Date], [Serior_No], [Memo], [chancel])
            //OUTPUT INSERTED.[JNO]
            //SELECT @START_TIME AS [JSTD], NULL AS [JETD], @JDLL AS [JDLL], @JCLASS AS [JCLASS], @JMETHOD AS [JMETHOD]
            //     , @JPARAM AS [JPARAM], @JPID AS [JPID], @JOWNER AS [JOWNER]
            //     , @JRID AS [JRID], @JYEAR AS [JYEAR], @JTERM AS [JTERM], @JDEP AS [JDEP], @JRECID AS [JRECID], @JPRITY AS [JPRITY]
            //     , @JTYPEID AS [JTYPEID], @STATUS_PROCESS AS [JSTATUSID], @RESULT_PROCESS AS [JRESULTID], '' AS [JLOG]
            //     , GETDATE() AS [C_Date], NULL AS [M_Date], @SERIOR_NO AS [Serior_No], @RESULT_PROCESS_TEXT AS [Memo], @CHANCEL AS [chancel]
            // WHERE 0 = (SELECT COUNT(1) FROM [JobCube] WHERE [JTYPEID] = @JTYPEID AND [JSTATUSID] IN (@STATUS_WAIT, @STATUS_PROCESS) AND [JSTD] >= @CHK_TIME);

            //--SELECT SCOPE_IDENTITY()
            #endregion

            string sql = null;
            KeyValue[] parameters = new KeyValue[] {
                new KeyValue("@JTYPEID", job.Jtypeid),
                new KeyValue("@STATUS_WAIT", JobCubeStatusCodeTexts.WAIT),
                new KeyValue("@STATUS_PROCESS", JobCubeStatusCodeTexts.PROCESS),
                new KeyValue("@RESULT_PROCESS", JobCubeResultCodeTexts.PROCESS),
                new KeyValue("@RESULT_PROCESS_TEXT", JobCubeResultCodeTexts.PROCESS_TEXT),
                new KeyValue("@STAMP", stamp),

                new KeyValue("@JDLL", job.Jdll),
                new KeyValue("@JCLASS", job.Jclass),
                new KeyValue("@JMETHOD", job.Jmethod),
                new KeyValue("@JPARAM", job.Jparam),
                new KeyValue("@JPID", job.Jpid),
                new KeyValue("@JOWNER", job.Jowner),

                new KeyValue("@JRID", job.Jrid),
                new KeyValue("@JYEAR", job.Jyear),
                new KeyValue("@JTERM", job.Jterm),
                new KeyValue("@JDEP", job.Jdep),
                new KeyValue("@JRECID", job.Jrecid),
                new KeyValue("@JPRITY", job.Jprity),

                new KeyValue("@SERIOR_NO", job.SeriorNo),
                new KeyValue("@CHANCEL", job.Chancel)
            };
            switch (checkMode)
            {
                case JobCubeCheckMode.ByTime:
                    #region 如果同一分鐘沒有未完成的 job 則新增，並傳回 JNO
                    {
                        sql = @"DECLARE @START_TIME datetime, @CHK_TIME datetime;
SET @START_TIME = GETDATE();
SET @CHK_TIME = CONVERT(datetime, CONVERT(varchar(16), DATEADD(MINUTE, -1, @START_TIME), 120));

INSERT INTO [JobCube] ([JSTD], [JETD], [JDLL], [JCLASS], [JMETHOD]
     , [JPARAM], [JPID], [JOWNER]
     , [JRID], [JYEAR], [JTERM], [JDEP], [JRECID], [JPRITY]
     , [JTYPEID], [JSTATUSID], [JRESULTID], [JLOG]
	 , [C_Date], [M_Date], [Serior_No], [Memo], [chancel])
OUTPUT INSERTED.[JNO]
SELECT @START_TIME AS [JSTD], NULL AS [JETD], @JDLL AS [JDLL], @JCLASS AS [JCLASS], @JMETHOD AS [JMETHOD]
     , @JPARAM AS [JPARAM], @JPID AS [JPID], @JOWNER AS [JOWNER]
     , @JRID AS [JRID], @JYEAR AS [JYEAR], @JTERM AS [JTERM], @JDEP AS [JDEP], @JRECID AS [JRECID], @JPRITY AS [JPRITY]
     , @JTYPEID AS [JTYPEID], @STATUS_PROCESS AS [JSTATUSID], @RESULT_PROCESS AS [JRESULTID], @RESULT_PROCESS_TEXT AS [JLOG]
     , GETDATE() AS [C_Date], NULL AS [M_Date], @SERIOR_NO AS [Serior_No], @STAMP AS [Memo], @CHANCEL AS [chancel]
 WHERE 0 = (SELECT COUNT(1) FROM [JobCube] WHERE [JTYPEID] = @JTYPEID AND [JSTATUSID] IN (@STATUS_WAIT, @STATUS_PROCESS) AND [JSTD] >= @CHK_TIME);

--SELECT SCOPE_IDENTITY()";
                    }
                    #endregion
                    break;
                case JobCubeCheckMode.ByDate:
                    #region 如果同一日沒有未完成的 job 則新增，並傳回 JNO
                    {
                        sql = @"DECLARE @START_TIME datetime, @CHK_TIME datetime;
SET @START_TIME = GETDATE();
SET @CHK_TIME = CONVERT(datetime, CONVERT(varchar(16), DATEADD(DAY, -1, @START_TIME), 120));

INSERT INTO [JobCube] ([JSTD], [JETD], [JDLL], [JCLASS], [JMETHOD]
     , [JPARAM], [JPID], [JOWNER]
     , [JRID], [JYEAR], [JTERM], [JDEP], [JRECID], [JPRITY]
     , [JTYPEID], [JSTATUSID], [JRESULTID], [JLOG]
	 , [C_Date], [M_Date], [Serior_No], [Memo], [chancel])
OUTPUT INSERTED.[JNO]
SELECT @START_TIME AS [JSTD], NULL AS [JETD], @JDLL AS [JDLL], @JCLASS AS [JCLASS], @JMETHOD AS [JMETHOD]
     , @JPARAM AS [JPARAM], @JPID AS [JPID], @JOWNER AS [JOWNER]
     , @JRID AS [JRID], @JYEAR AS [JYEAR], @JTERM AS [JTERM], @JDEP AS [JDEP], @JRECID AS [JRECID], @JPRITY AS [JPRITY]
     , @JTYPEID AS [JTYPEID], @STATUS_PROCESS AS [JSTATUSID], @RESULT_PROCESS AS [JRESULTID], @RESULT_PROCESS_TEXT AS [JLOG]
	 , GETDATE() AS [C_Date], NULL AS [M_Date], @SERIOR_NO AS [Serior_No], @STAMP AS [Memo], @CHANCEL AS [chancel]
 WHERE 0 = (SELECT COUNT(1) FROM [JobCube] WHERE [JTYPEID] = @JTYPEID AND [JSTATUSID] IN (@STATUS_WAIT, @STATUS_PROCESS) AND [JSTD] >= @CHK_TIME);

--SELECT SCOPE_IDENTITY()";
                    }
                    #endregion
                    break;
                default:
                    #region 不用檢查
                    {
                        sql = @"INSERT INTO [JobCube] ([JSTD], [JETD], [JDLL], [JCLASS], [JMETHOD]
     , [JPARAM], [JPID], [JOWNER]
     , [JRID], [JYEAR], [JTERM], [JDEP], [JRECID], [JPRITY]
     , [JTYPEID], [JSTATUSID], [JRESULTID], [JLOG]
	 , [C_Date], [M_Date], [Serior_No], [Memo], [chancel])
OUTPUT INSERTED.[JNO]
SELECT GETDATE() AS [JSTD], NULL AS [JETD], @JDLL AS [JDLL], @JCLASS AS [JCLASS], @JMETHOD AS [JMETHOD]
     , @JPARAM AS [JPARAM], @JPID AS [JPID], @JOWNER AS [JOWNER]
     , @JRID AS [JRID], @JYEAR AS [JYEAR], @JTERM AS [JTERM], @JDEP AS [JDEP], @JRECID AS [JRECID], @JPRITY AS [JPRITY]
     , @JTYPEID AS [JTYPEID], @STATUS_PROCESS AS [JSTATUSID], @RESULT_PROCESS AS [JRESULTID], @RESULT_PROCESS_TEXT AS [JLOG]
	 , GETDATE() AS [C_Date], NULL AS [M_Date], @SERIOR_NO AS [Serior_No], @STAMP AS [Memo], @CHANCEL AS [chancel];

--SELECT SCOPE_IDENTITY()";
                    }
                    #endregion
                    break;
            }
            #endregion

            #region 執行 SQL
            Result result = null;
            {
                object value = null;
                result = _Factory.ExecuteScalar(sql, parameters, out value);
                if (result.IsSuccess)
                {
                    Int32 jno = 0;
                    if (value == null || !Int32.TryParse(value.ToString(), out jno) || jno < 1)
                    {
                        result = new Result(false, "此作業已由其他程序處理中，無法重複執行", ErrorCode.D_DATA_EXISTS, null);
                    }
                    else
                    {
                        JobcubeEntity newJob = null;
                        Expression where = new Expression(JobcubeEntity.Field.Jno, jno);
                        result = _Factory.SelectFirst<JobcubeEntity>(where, null, out newJob);
                        if (result.IsSuccess)
                        {
                            if (newJob == null)
                            {
                                string errmsg = String.Format("讀取新增處理中批次處理佇列資料 ({0}) 失敗，錯誤訊息：{1}", jno, "資料不存在");
                                result = new Result(false, errmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                            }
                            else if (newJob.Jstatusid != JobCubeStatusCodeTexts.PROCESS)
                            {
                                string errmsg = String.Format("讀取新增處理中批次處理佇列資料 ({0}) 的狀態代碼 ({1}) 不正確 ({2})", jno, newJob.Jstatusid, JobCubeStatusCodeTexts.PROCESS);
                                result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                            }
                            else if (newJob.Memo != stamp)
                            {
                                string errmsg = String.Format("讀取新增處理中批次處理佇列資料 ({0}) 的戳記號碼 ({1}) 不正確 ({2})", jno, newJob.Memo, stamp);
                                result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                            }
                            else if (newJob.Jpid != job.Jpid)
                            {
                                string errmsg = String.Format("讀取新增處理中批次處理佇列資料 ({0}) 的伺服器名稱 ({1}) 不正確 ({2})", jno, newJob.Jpid, job.Jpid);
                                result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                            }
                            else
                            {
                                job = newJob;
                            }
                        }
                        else
                        {
                            string errmsg = String.Format("讀取新增處理中批次處理佇列資料 ({0}) 失敗，錯誤訊息：{1}", jno, result.Message);
                            result = new Result(false, errmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                    }
                }
                else
                {
                    string errmsg = String.Format("新增處理中批次處理佇列資料 (jobTypeId={0}; checkMode={1}) 資料失敗，錯誤訊息：{2}", job.Jtypeid, checkMode, result.Message);
                    result = new Result(false, errmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            return result;
            #endregion
        }
        #endregion

        #region [MDY:20160312] 取得待處理並註記為處理中的批次處理佇列資料相關
        /// <summary>
        /// 取得指定作業類別代碼的第一筆待處理批次處理佇列資料 (以建立日期遞增排序 + 優先權遞減排序)，並將其註記為處理中
        /// </summary>
        /// <param name="jobTypeId">指定作業類別代碼</param>
        /// <param name="job">成功則傳回處理中的批次處理佇列資料，否則傳回 null</param>
        /// <returns>傳回處理結果</returns>
        public Result GetWaitJobToProcess(string jobTypeId, out JobcubeEntity job)
        {
            job = null;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                return new Result(false, "資料存取物件未準備好", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (!JobCubeTypeCodeTexts.IsDefine(jobTypeId))
            {
                return new Result(false, "缺少或不正確的作業類別代碼", ErrorCode.S_INVALID_PARAMETER, null);
            }
            string stamp = Common.GetGUID();
            string machineName = Environment.MachineName;
            #endregion

            #region 取得待處理的 JNO 並將該資料設為處理中
            int jno = 0;
            {
                string sql = @"DECLARE @JNO int; 
UPDATE [JobCube] SET @JNO = [JNO], [JSTD] = GETDATE(), [JSTATUSID] = @STATUS_PROCESS, [JRESULTID] = @RESULT_PROCESS, [JLOG] = @RESULT_PROCESS_TEXT
     , [Memo] = @STAMP, [JPID] = @JPID
 WHERE [JNO] = (SELECT TOP 1 [JNO] FROM [JobCube] WHERE [JTYPEID] = @JTYPEID AND [JSTATUSID] = @STATUS_WAIT ORDER BY [C_Date], [JPRITY] DESC); 
SELECT ISNULL(@JNO, 0) AS JSNO;";

                KeyValue[] parameters = new KeyValue[] {
                    new KeyValue("@STATUS_PROCESS", JobCubeStatusCodeTexts.PROCESS),
                    new KeyValue("@RESULT_PROCESS", JobCubeResultCodeTexts.PROCESS),
                    new KeyValue("@RESULT_PROCESS_TEXT", JobCubeResultCodeTexts.PROCESS_TEXT),
                    new KeyValue("@STAMP", stamp),
                    new KeyValue("@JPID", machineName),
                    new KeyValue("@JTYPEID", jobTypeId),
                    new KeyValue("@STATUS_WAIT", JobCubeStatusCodeTexts.WAIT)
                };

                object value = null;
                Result result = _Factory.ExecuteScalar(sql, parameters, out value);
                if (!result.IsSuccess)
                {
                    string errmsg = String.Format("註記待處理批次處理佇列資料為處理中失敗，錯誤訊息：{0}", result.Message);
                    return new Result(false, errmsg, result.Code, result.Exception);
                }

                jno = Int32.Parse(value.ToString());
            }
            #endregion

            #region 取得 Job 資料
            if (jno > 0)
            {
                Expression where = new Expression(JobcubeEntity.Field.Jno, jno);
                Result result = _Factory.SelectFirst<JobcubeEntity>(where, null, out job);
                if (result.IsSuccess)
                {
                    if (job == null)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 失敗，錯誤訊息：{1}", jno, "資料不存在");
                        result = new Result(false, errmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    else if (job.Jstatusid != JobCubeStatusCodeTexts.PROCESS)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 的狀態代碼 ({1}) 不正確 ({2})", jno, job.Jstatusid, JobCubeStatusCodeTexts.PROCESS);
                        result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    else if (job.Memo != stamp)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 的戳記號碼 ({1}) 不正確 ({2})", jno, job.Memo, stamp);
                        result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    else if (job.Jpid != machineName)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 的伺服器名稱 ({1}) 不正確 ({2})", jno, job.Jpid, machineName);
                        result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                }
                else
                {
                    string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 失敗，錯誤訊息：{1}", jno, result.Message);
                    result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
                return result;
            }
            else
            {
                //無待處理的 Job
                return new Result(true);
            }
            #endregion
        }

        /// <summary>
        /// 取得指定作業類別代碼且所於本伺服器的第一筆待處理批次處理佇列資料 (以建立日期遞增排序 + 優先權遞減排序)，並將其註記為處理中
        /// </summary>
        /// <param name="jobTypeId">指定作業類別代碼</param>
        /// <param name="job">成功則傳回處理中的批次處理佇列資料，否則傳回 null</param>
        /// <returns>傳回處理結果</returns>
        public Result GetMyWaitJobToProcess(string jobTypeId, out JobcubeEntity job)
        {
            job = null;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                return new Result(false, "資料存取物件未準備好", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (!JobCubeTypeCodeTexts.IsDefine(jobTypeId))
            {
                return new Result(false, "缺少或不正確的作業類別代碼", ErrorCode.S_INVALID_PARAMETER, null);
            }
            string stamp = Common.GetGUID();
            string machineName = Environment.MachineName;
            #endregion

            #region 取得待處理的 JNO 並將該資料設為處理中
            int jno = 0;
            {
                string sql = @"DECLARE @JNO int; 
UPDATE [JobCube] SET @JNO = [JNO], [JSTD] = GETDATE(), [JSTATUSID] = @STATUS_PROCESS, [JRESULTID] = @RESULT_PROCESS, [JLOG] = @RESULT_PROCESS_TEXT, [Memo] = @STAMP
 WHERE [JNO] = (SELECT TOP 1 [JNO] FROM [JobCube] WHERE [JTYPEID] = @JTYPEID AND [JSTATUSID] = @STATUS_WAIT AND [JPID] = @JPID ORDER BY [C_Date], [JPRITY] DESC); 
SELECT ISNULL(@JNO, 0) AS JSNO;";

                KeyValue[] parameters = new KeyValue[] {
                    new KeyValue("@STATUS_PROCESS", JobCubeStatusCodeTexts.PROCESS),
                    new KeyValue("@RESULT_PROCESS", JobCubeResultCodeTexts.PROCESS),
                    new KeyValue("@RESULT_PROCESS_TEXT", JobCubeResultCodeTexts.PROCESS_TEXT),
                    new KeyValue("@STAMP", stamp),
                    new KeyValue("@JTYPEID", jobTypeId),
                    new KeyValue("@STATUS_WAIT", JobCubeStatusCodeTexts.WAIT),
                    new KeyValue("@JPID", machineName)
                };

                object value = null;
                Result result = _Factory.ExecuteScalar(sql, parameters, out value);
                if (!result.IsSuccess)
                {
                    string errmsg = String.Format("註記待處理批次處理佇列資料為處理中失敗，錯誤訊息：{0}", result.Message);
                    return new Result(false, errmsg, result.Code, result.Exception);
                }

                jno = Int32.Parse(value.ToString());
            }
            #endregion

            #region 取得 Job 資料
            if (jno > 0)
            {
                Expression where = new Expression(JobcubeEntity.Field.Jno, jno);
                Result result = _Factory.SelectFirst<JobcubeEntity>(where, null, out job);
                if (result.IsSuccess)
                {
                    if (job == null)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 失敗，錯誤訊息：{1}", jno, "資料不存在");
                        result = new Result(false, errmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    else if (job.Jstatusid != JobCubeStatusCodeTexts.PROCESS)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 的狀態代碼 ({1}) 不正確 ({2})", jno, job.Jstatusid, JobCubeStatusCodeTexts.PROCESS);
                        result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    else if (job.Memo != stamp)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 的戳記號碼 ({1}) 不正確 ({2})", jno, job.Memo, stamp);
                        result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    else if (job.Jpid != machineName)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 的伺服器名稱 ({1}) 不正確 ({2})", jno, job.Jpid, machineName);
                        result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                }
                else
                {
                    string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 失敗，錯誤訊息：{1}", jno, result.Message);
                    result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
                return result;
            }
            else
            {
                //無待處理的 Job
                return new Result(true);
            }
            #endregion
        }

        /// <summary>
        /// 取得指定序號的待處理批次處理佇列資料 (以建立日期遞增排序 + 優先權遞減排序)，並將其註記為處理中
        /// </summary>
        /// <param name="jobCubeNo">指定批次處理佇列序號</param>
        /// <param name="job">成功則傳回處理中的批次處理佇列資料，否則傳回 null</param>
        /// <returns>傳回處理結果</returns>
        private Result GetWaitJobToProcessByJobCubeNo(string jobCubeNo, string stamp, out JobcubeEntity job)
        {
            job = null;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                return new Result(false, "資料存取物件未準備好", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            int jno = 0;
            if (String.IsNullOrWhiteSpace(jobCubeNo) || !Int32.TryParse(jobCubeNo, out jno) || jno < 1)
            {
                return new Result(false, "缺少或不正確的批次處理佇列序號", ErrorCode.S_INVALID_PARAMETER, null);
            }
            stamp = stamp == null ? String.Empty : stamp.Trim();
            string machineName = Environment.MachineName;
            #endregion

            #region 將指定 JNO 的待處理資料設為處理中
            {
                string sql = @"UPDATE [JobCube] 
   SET @JNO = [JNO], [JSTD] = GETDATE(), [JSTATUSID] = @STATUS_PROCESS, [JRESULTID] = @RESULT_PROCESS, [JLOG] = @RESULT_PROCESS_TEXT
     , [Memo] = @STAMP, [JPID] = @JPID
 WHERE JNO = @JNO AND [JSTATUSID] = @STATUS_WAIT;";

                KeyValue[] parameters = new KeyValue[] {
                    new KeyValue("@STATUS_PROCESS", JobCubeStatusCodeTexts.PROCESS),
                    new KeyValue("@RESULT_PROCESS", JobCubeResultCodeTexts.PROCESS),
                    new KeyValue("@RESULT_PROCESS_TEXT", JobCubeResultCodeTexts.PROCESS_TEXT),
                    new KeyValue("@STAMP", stamp),
                    new KeyValue("@JPID", machineName),
                    new KeyValue("@JNO", jno),
                    new KeyValue("@STATUS_WAIT", JobCubeStatusCodeTexts.WAIT)
                };

                int count = 0;
                Result result = _Factory.ExecuteNonQuery(sql, parameters, out count);
                if (!result.IsSuccess)
                {
                    string errmsg = String.Format("註記待處理批次處理佇列資料 ({0}) 為處理中失敗，錯誤訊息：{1}", jno, result.Message);
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (count == 0)
                {
                    string errmsg = String.Format("註記待處理批次處理佇列資料 ({0}) 為處理中失敗，錯誤訊息：{1}", jno, "無資料被更新");
                    return new Result(false, errmsg, CoreStatusCode.D_NOT_DATA_UPDATE, null);
                }
            }
            #endregion

            #region 取得 Job 資料
            {
                Expression where = new Expression(JobcubeEntity.Field.Jno, jno);
                Result result = _Factory.SelectFirst<JobcubeEntity>(where, null, out job);
                if (result.IsSuccess)
                {
                    if (job == null)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 失敗，錯誤訊息：{1}", jno, "資料不存在");
                        result = new Result(false, errmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    else if (job.Jstatusid != JobCubeStatusCodeTexts.PROCESS)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 的狀態代碼 ({1}) 不正確 ({2})", jno, job.Jstatusid, JobCubeStatusCodeTexts.PROCESS);
                        result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    else if (job.Memo != stamp)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 的戳記號碼 ({1}) 不正確 ({2})", jno, job.Memo, stamp);
                        result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    else if (job.Jpid != machineName)
                    {
                        string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 的伺服器名稱 ({1}) 不正確 ({2})", jno, job.Jpid, machineName);
                        result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                }
                else
                {
                    string errmsg = String.Format("讀取註記處理中批次處理佇列資料 ({0}) 失敗，錯誤訊息：{1}", jno, result.Message);
                    result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
                return result;
            }
            #endregion
        }
        #endregion

        #region [MDY:20160312] 更新處理中的批次處理佇列資料為已完成相關
        /// <summary>
        /// 更新指定序號的處理中批次處理佇列資料成為已完成
        /// </summary>
        /// <param name="jno">指定批次處理佇列的序號</param>
        /// <param name="stamp">指定批次處理佇列的戳記號碼</param>
        /// <param name="resultId">指定處理結果代碼</param>
        /// <param name="resultMsg">指定處理結果訊息</param>
        /// <param name="log">指定處理日誌</param>
        /// <returns>傳回處理結果</returns>
        public Result UpdateProcessJobToFinsh(int jno, string stamp, string resultId, string resultMsg, string log)
        {
            string statusId = JobCubeStatusCodeTexts.FINISH;
            return this.UpdateProcessJobToEnding(jno, stamp, statusId, resultId, resultMsg, log);
        }

        /// <summary>
        /// 更新指定序號的處理中批次處理佇列資料成為已中止
        /// </summary>
        /// <param name="jno">指定批次處理佇列的序號</param>
        /// <param name="stamp">指定批次處理佇列的戳記號碼</param>
        /// <param name="resultId">指定處理結果代碼</param>
        /// <param name="resultMsg">指定處理結果訊息</param>
        /// <param name="log">指定處理日誌</param>
        /// <returns>傳回處理結果</returns>
        public Result UpdateProcessJobToBreak(int jno, string stamp, string resultId, string resultMsg, string log)
        {
            string statusId = JobCubeStatusCodeTexts.BREAK;
            return this.UpdateProcessJobToEnding(jno, stamp, statusId, resultId, resultMsg, log);
        }

        /// <summary>
        /// 更新指定序號的處理中批次處理佇列資料成為處理結束
        /// </summary>
        /// <param name="jno">指定批次處理佇列的序號</param>
        /// <param name="stamp">指定批次處理佇列的戳記號碼</param>
        /// <param name="statusId">指定處理狀態代碼</param>
        /// <param name="resultId">指定處理結果代碼</param>
        /// <param name="resultMsg">指定處理結果訊息</param>
        /// <param name="log">指定處理日誌</param>
        /// <returns>傳回處理結果</returns>
        private Result UpdateProcessJobToEnding(int jno, string stamp, string statusId, string resultId, string resultMsg, string log)
        {
            #region 檢查 IsReady
            if (!this.IsReady())
            {
                return new Result(false, "資料存取物件未準備好", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (jno < 1)
            {
                return new Result(false, "缺少或不正確的批次處理佇列序號參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (!JobCubeResultCodeTexts.IsDefine(resultId))
            {
                return new Result(false, "缺少或不正確的處理結果代碼參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (!JobCubeStatusCodeTexts.IsEnding(statusId))
            {
                return new Result(false, "缺少或不正確的處理狀態代碼參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            resultMsg = resultMsg == null ? String.Empty : resultMsg.Trim();

            #region [MDY:20160705] Memo 欄位放大為 nvarchar(2000) ，超過長度就剪掉，避免更新失敗
            if (resultMsg.Length > 2000)
            {
                resultMsg = resultMsg.Substring(0, 2000);
            }
            #endregion

            log = log == null ? String.Empty : log.Trim();
            string statusName = JobCubeStatusCodeTexts.GetText(statusId);
            #endregion

            #region 檢查資料是否存在
            //{
            //    string sql = "SELECT ISNULL([Memo], '') AS [Memo] FROM [JobCube] WHERE [JNO] = @JNO AND [JSTATUSID] = @STATUS_PROCESS";
            //    KeyValue[] parameters = new KeyValue[] { new KeyValue("@JNO", jno), new KeyValue("@STATUS_PROCESS", JobCubeStatusCodeTexts.PROCESS) };
            //    object value = null;
            //    Result result = _Factory.ExecuteScalar(sql, parameters, out value);
            //    if (result.IsSuccess)
            //    {
            //        if (value == null)
            //        {
            //            return new Result(false, String.Format("無法取得編號 {0} 的待處理批次處理佇列資料", jno), ErrorCode.D_DATA_NOT_FOUND, null);
            //        }
            //        if (!String.IsNullOrEmpty(stamp) && value.ToString() != stamp)
            //        {
            //            return new Result(false, "該批次處理佇列資料的戳記號碼不正確", ErrorCode.D_DATA_NOT_FOUND, null);
            //        }
            //    }
            //    else
            //    {
            //        return result;
            //    }
            //}
            #endregion

            #region 更新資料
            {
                string sql = null;
                KeyValue[] parameters = null;
                if (String.IsNullOrWhiteSpace(stamp))
                {
                    sql = @"UPDATE [JobCube] 
   SET [JETD] = GETDATE(), [JSTATUSID] = @STATUS_ENDING, [JRESULTID] = @RESULT_ID, [Memo] = @RESULT_MSG, [JLOG] = @LOG, [M_Date] = GETDATE()
 WHERE [JNO] = @JNO";
                    parameters = new KeyValue[] {
                        new KeyValue("@STATUS_ENDING", statusId),
                        new KeyValue("@RESULT_ID", resultId),
                        new KeyValue("@RESULT_MSG", resultMsg),
                        new KeyValue("@LOG", log),
                        new KeyValue("@JNO", jno)
                    };
                }
                else
                {
                    sql = @"UPDATE [JobCube] 
   SET [JETD] = GETDATE(), [JSTATUSID] = @STATUS_ENDING, [JRESULTID] = @RESULT_ID, [Memo] = @RESULT_MSG, [JLOG] = @LOG, [M_Date] = GETDATE()
 WHERE [JNO] = @JNO AND [Memo] = @STAMP";
                    parameters = new KeyValue[] {
                        new KeyValue("@STATUS_ENDING", statusId),
                        new KeyValue("@RESULT_ID", resultId),
                        new KeyValue("@RESULT_MSG", resultMsg),
                        new KeyValue("@LOG", log),
                        new KeyValue("@JNO", jno),
                        new KeyValue("@STAMP", stamp.Trim())
                    };
                }

                int count = 0;
                Result result = _Factory.ExecuteNonQuery(sql, parameters, out count);

                #region 避免臨時資料更新失敗，造成狀態永遠為處理中，試 4 次 每次睡 15 秒
                if (!result.IsSuccess)
                {
                    int retryTimes = 4;
                    int retrySleep = 1000 * 15;
                    for (int times = 1; times <= retryTimes; times++)
                    {
                        System.Threading.Thread.Sleep(retrySleep);
                        result = _Factory.ExecuteNonQuery(sql, parameters, out count);
                        if (result.IsSuccess)
                        {
                            break;
                        }
                    }
                }
                #endregion

                if (result.IsSuccess)
                {
                    if (count == 0)
                    {
                        string errmsg = String.Format("更新處理中批次處理佇列資料 (JNO={0}, MEMO={1}) 為{2}失敗，錯誤訊息：{3}", jno, stamp, statusName, "資料不存在");
                        result = new Result(false, errmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                }
                else
                {
                    string errmsg = String.Format("更新處理中批次處理佇列資料 (JNO={0}, MEMO={1}) 為{2}已完成失敗，錯誤訊息：{3}", jno, stamp, statusName, result.Message);
                    return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
                return result;
            }
            #endregion
        }
        #endregion

        #region [MDY:20160312] Static Method
        /// <summary>
        /// 轉換指定字串為批次處理佇列檢查模式列舉
        /// </summary>
        /// <param name="value">指定字串</param>
        /// <returns>傳回批次處理佇列檢查模式列舉值</returns>
        public static JobCubeCheckMode ConvertCheckMode(string value)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                switch (value.Trim().ToUpper())
                {
                    case "BYTIME":
                        return JobCubeCheckMode.ByTime;
                    case "BYDATE":
                        return JobCubeCheckMode.ByDate;
                }
            }
            return JobCubeCheckMode.Skip;
        }
        #endregion


        #region Job 處理 Method
        #region [MDY:20180414] 增加傳回無虛擬帳號筆數
        /// <summary>
        /// 處理 CI (產生銷帳編號) 批次處理序列作業
        /// </summary>
        /// <param name="job">指定要處理的 jobcube</param>
        /// <param name="logmsg">傳回處理日誌</param>
        /// <param name="totalCount">傳回處理筆數</param>
        /// <param name="successCount">傳回處理成功筆數</param>
        /// <param name="noATMCancelNoCount">傳回無虛擬帳號筆數</param>
        /// <returns>傳回處理結果</returns>
        public Result ProcessCIJob(JobcubeEntity job, out string logmsg, out Int32 totalCount, out Int32 successCount, out Int32 noATMCancelNoCount)
        {
            logmsg = null;
            totalCount = 0;
            successCount = 0;
            noATMCancelNoCount = 0;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                logmsg = "資料存取物件未準備好";
                return new Result(false, logmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job.Jtypeid != JobCubeTypeCodeTexts.CI)
            {
                logmsg = String.Format("批次處理序列 {0} 的類別不符合", job.Jno);
                return new Result(false, logmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string yearId = job.Jyear;
            string termId = job.Jterm;
            string depId = job.Jdep;
            string receiveId = job.Jrecid;
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || String.IsNullOrEmpty(receiveId))
            {
                logmsg = String.Format("批次處理序列 {0} 缺少業務別碼、學年代碼、學期代碼或代收費用別代碼的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得商家代號資料
            SchoolRTypeEntity school = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                if (!result.IsSuccess)
                {
                    logmsg = String.Format("讀取商家代號 {0} 的資料失敗，{1}", receiveType, result.Message);
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (school == null)
                {
                    logmsg = String.Format("查無商家代號 {0} 的資料", receiveType);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            #endregion

            #region 取得虛擬帳號模組資料
            CancelNoHelper helper = new CancelNoHelper();
            CancelNoHelper.Module module = helper.GetModuleByReceiveType(receiveType);
            if (module == null)
            {
                logmsg = String.Format("無法取得商家代號 {0} 的虛擬帳號模組資訊", receiveType);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region 拆解 JobcubeEntity 參數
            bool isParamOK = false;
            int upNo = 0;
            string sortType = null;
            string[] sortFields = null;

            #region [Old] 取消自訂流水號
            //Int64? startSeriorNo = null;
            #endregion
            {
                string pReceiveType = null;
                string pYearId = null;
                string pTermId = null;
                string pDepId = null;
                string pReceiveId = null;
                string pUpNo = null;
                string pStartSeriorNo = null;
                isParamOK = JobcubeEntity.ParseCIParameter(job.Jparam, out pReceiveType, out pYearId, out pTermId, out pDepId, out pReceiveId
                                , out pUpNo, out pStartSeriorNo, out sortType, out sortFields);
                if (String.IsNullOrEmpty(pUpNo) || !Int32.TryParse(pUpNo, out upNo))
                {
                    logmsg = "批次處理序列缺少上傳繳費資料的批號的參數或參數值不正確";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                if ((sortType != "1" && sortType != "2")
                    || (sortType == "1" && (sortFields == null || sortFields.Length == 0)))
                {
                    logmsg = "批次處理序列缺少排序原則、排序欄位的參數或參數值不正確";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }

                #region [Old] 取消自訂流水號
                //if (!String.IsNullOrEmpty(pStartSeriorNo))
                //{
                //    Int64 value = 0;
                //    if (Int64.TryParse(pStartSeriorNo, out value))
                //    {
                //        startSeriorNo = value;
                //    }
                //    else
                //    {
                //        logmsg = "批次處理序列的流水號起始位置參數值不正確";
                //        return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                //    }
                //}
                #endregion
            }
            #endregion

            #region 取得繳費期限
            //string payDueDate = null;
            //if (module.PayDueDateKind != CancelNoHelper.YearDayKind.Empty)
            //{
            //    SchoolRidEntity schoolRid = null;
            //    Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
            //        .And(SchoolRidEntity.Field.YearId, yearId)
            //        .And(SchoolRidEntity.Field.TermId, termId)
            //        .And(SchoolRidEntity.Field.DepId, depId)
            //        .And(SchoolRidEntity.Field.ReceiveId, receiveId);
            //    Result result = _Factory.SelectFirst<SchoolRidEntity>(where, null, out schoolRid);
            //    if (!result.IsSuccess)
            //    {
            //        logmsg = "讀取繳費期限資料失敗，錯誤訊息：" + result.Message;
            //        return new Result(false, logmsg, result.Code, result.Exception);
            //    }
            //    if (schoolRid == null)
            //    {
            //        logmsg = "查無代收費用別設定";
            //        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
            //    }
            //    payDueDate = schoolRid.PayDate;
            //}
            #endregion

            #region 取繳費資料
            StudentReceiveEntity[] studentReceives = null;
            {
                Expression w1 = new Expression(StudentReceiveEntity.Field.CancelFlag, null).Or(StudentReceiveEntity.Field.CancelFlag, "");      //未銷帳
                Expression w2 = new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, "");    //未繳費
                Expression w3 = new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, "");    //未入帳
                Expression w4 = new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, "");      //未繳費&未入帳
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                    .And(StudentReceiveEntity.Field.YearId, yearId)
                    .And(StudentReceiveEntity.Field.TermId, termId)
                    .And(StudentReceiveEntity.Field.DepId, depId)
                    .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                    .And(StudentReceiveEntity.Field.UpNo, upNo)
                    .And(w1)    //未銷帳
                    .And(w2)    //未繳費
                    .And(w3)    //未銷帳
                    .And(w4);   //未繳費&未入帳

                #region OrderBy
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                if (sortType == "1")
                {
                    //指定排序欄位
                    foreach (string sortField in sortFields)
                    {
                        switch (sortField)
                        {
                            case "College_Id":
                                orderbys.Add(StudentReceiveEntity.Field.CollegeId, OrderByEnum.Asc);
                                break;
                            case "Major_Id":
                                orderbys.Add(StudentReceiveEntity.Field.MajorId, OrderByEnum.Asc);
                                break;
                            case "Stu_Grade":
                                orderbys.Add(StudentReceiveEntity.Field.StuGrade, OrderByEnum.Asc);
                                break;
                            case "Class_Id":
                                orderbys.Add(StudentReceiveEntity.Field.ClassId, OrderByEnum.Asc);
                                break;
                            case "Stu_Id":
                                orderbys.Add(StudentReceiveEntity.Field.StuId, OrderByEnum.Asc);
                                break;
                        }
                    }
                }
                else
                {
                    //照原接收資料順序
                    orderbys.Add(StudentReceiveEntity.Field.UpOrder, OrderByEnum.Asc);
                }
                #endregion

                Result result = _Factory.SelectAll<StudentReceiveEntity>(where, orderbys, out studentReceives);
                if (!result.IsSuccess)
                {
                    logmsg = "讀取繳費單資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (studentReceives == null || studentReceives.Length == 0)
                {
                    logmsg = "查無任何可計算的繳費資料";
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
            }
            totalCount = studentReceives == null ? 0 : studentReceives.Length;
            #endregion

            #region 逐筆產生流水號與虛擬帳號，並寫入資料庫 (不使用交易)
            {
                bool isBigReceiveId = school.IsBigReceiveId();

                CancelNoHelper cnoHelper = new CancelNoHelper();
                SNoHelper snohelper = new SNoHelper();
                Int64 maxSeriorNo = module.GetMaxSeriorNo(isBigReceiveId);    // Convert.ToInt64(Math.Pow(10, module.SeriorNoSize));
                string snoKey = snohelper.GetKeyForStudentReceiveSeriorNo(receiveType, yearId, termId, depId, receiveId);

                StringBuilder log = new StringBuilder();
                foreach (StudentReceiveEntity studentReceive in studentReceives)
                {
                    string orgCancelNo = studentReceive.CancelNo;

                    #region [Old]
                    //#region 是否產生虛擬帳號邏輯
                    ////1. 檢碼規則 9 的學校如有上傳虛擬帳號，不論金額是否大於 0，皆以上傳資料為準
                    ////2. 其他檢碼規則的學校不論是否有上傳虛擬帳號，金額大於 0 才產生，否則清為空字串
                    //#endregion

                    //#region [Old] 改成只檢查是否有計算金額
                    ////#region 檢查是否有金額
                    ////if (studentReceive.ReceiveAmount == null || studentReceive.ReceiveAmount <= 0)
                    ////{
                    ////    log.AppendFormat("學號 {0} 的繳費資料金額未計算或小於等於 0", studentReceive.StuId).AppendLine();
                    ////    continue;
                    ////}
                    ////#endregion
                    //#endregion

                    //#region [New] 改成只檢查是否有計算金額
                    //if (studentReceive.ReceiveAmount == null)
                    //{
                    //    log.AppendFormat("學號 {0} 的繳費資料金額未計算", studentReceive.StuId).AppendLine();
                    //    continue;
                    //}
                    //#endregion

                    //#region 產生流水號與各種虛擬帳號
                    //string cancelNo = null;
                    //string smCancelNo = null;
                    //string atmCancelNo = null;
                    //string seriorNo = null;
                    //string errmsg = null;
                    //bool isNeedSaveSeriorNo = true;
                    //bool isNeedSaveCancelNo = true;
                    //bool hasUploadSeriorNo = studentReceive.HasUploadSeriorNo();
                    //bool hasUploadCancelNo = studentReceive.HasUploadCancelNo();
                    //if (hasUploadSeriorNo && hasUploadCancelNo)
                    //{
                    //    #region 上傳資料含流水號與虛擬帳號
                    //    isNeedSaveSeriorNo = false;
                    //    isNeedSaveCancelNo = false;

                    //    if (module.IsD38Kind && !String.IsNullOrWhiteSpace(studentReceive.CancelNo))
                    //    {
                    //        #region 特別處理 module.IsD38Kind 類，直接使用 CanceNo
                    //        if (String.IsNullOrEmpty(errmsg))
                    //        {
                    //            cancelNo = studentReceive.CancelNo.Trim();
                    //            if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                    //            {
                    //                studentReceive.CancelAtmno = cancelNo;
                    //            }
                    //            else
                    //            {
                    //                studentReceive.CancelAtmno = String.Empty;
                    //            }
                    //        }
                    //        if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                    //        {
                    //            if (studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                    //            {
                    //                studentReceive.CancelSmno = cancelNo;
                    //            }
                    //            else
                    //            {
                    //                studentReceive.CancelSmno = String.Empty;
                    //            }
                    //        }
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        if (studentReceive.ReceiveAmount > 0)
                    //        {
                    //            //金額大於 0 才要產生虛擬帳號
                    //            #region 檢查流水號與虛擬帳號
                    //            if (String.IsNullOrWhiteSpace(studentReceive.SeriorNo))
                    //            {
                    //                errmsg = String.Format("學號 {0} 的上傳繳費資料缺少流水號", studentReceive.StuId);
                    //                log.AppendLine(errmsg);
                    //                continue;
                    //            }
                    //            if (String.IsNullOrWhiteSpace(studentReceive.CancelNo))
                    //            {
                    //                errmsg = String.Format("學號 {0} 的上傳繳費資料缺少虛擬帳號", studentReceive.StuId);
                    //                log.AppendLine(errmsg);
                    //                continue;
                    //            }
                    //            #endregion

                    //            #region [Old] 取得身份證字號
                    //            //string pId = null;
                    //            //if (String.IsNullOrEmpty(errmsg) && module.IsHasPersonalId)
                    //            //{
                    //            //    errmsg = this.GetStudentPId(studentReceive.ReceiveType, studentReceive.StuId, out pId);
                    //            //    if (!String.IsNullOrEmpty(errmsg))
                    //            //    {
                    //            //        log.AppendLine(errmsg);
                    //            //        continue;
                    //            //    }
                    //            //}
                    //            #endregion

                    //            #region 檢查虛擬帳號
                    //            if (String.IsNullOrEmpty(errmsg))
                    //            {
                    //                string myReceiveType = null;
                    //                string myCustomNo = null;
                    //                string myChecksum = null;
                    //                string myYearId = null;
                    //                string myTermId = null;
                    //                string myReceiveId = null;
                    //                string mySeriorNo = null;
                    //                string customNo = null;
                    //                string checksum = null;
                    //                if (module.TryParseCancelNo(studentReceive.CancelNo, isBigReceiveId, out myReceiveType, out myCustomNo, out myChecksum, out myYearId, out myTermId, out myReceiveId, out mySeriorNo))
                    //                {
                    //                    if (studentReceive.SeriorNo != mySeriorNo)
                    //                    {
                    //                        errmsg = String.Format("學號 {0} 的上傳虛擬帳號與上傳流水號不合", studentReceive.StuId);
                    //                    }
                    //                    else
                    //                    {
                    //                        errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveAmount.Value, out cancelNo, out customNo, out checksum);
                    //                        if (String.IsNullOrEmpty(errmsg) && (cancelNo != myCustomNo || checksum != myChecksum))
                    //                        {
                    //                            errmsg = String.Format("學號 {0} 的上傳虛擬帳號與系統產生的虛擬帳號不同", studentReceive.StuId);
                    //                        }
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    errmsg = String.Format("學號 {0} 的上傳虛擬帳號不正確不法拆解", studentReceive.StuId);
                    //                }
                    //                if (!String.IsNullOrEmpty(errmsg))
                    //                {
                    //                    log.AppendLine(errmsg);
                    //                    continue;
                    //                }
                    //            }
                    //            #endregion

                    //            #region 產生虛擬帳號
                    //            if (String.IsNullOrEmpty(errmsg))
                    //            {
                    //                string customNo = null;
                    //                string checksum = null;
                    //                if (String.IsNullOrEmpty(errmsg) && studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                    //                {
                    //                    errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveSmamount.Value, out smCancelNo, out customNo, out checksum);
                    //                }
                    //                if (String.IsNullOrEmpty(errmsg) && studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                    //                {
                    //                    errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveAtmamount.Value, out atmCancelNo, out customNo, out checksum);
                    //                }
                    //                if (!String.IsNullOrEmpty(errmsg))
                    //                {
                    //                    log.AppendLine(errmsg);
                    //                    continue;
                    //                }
                    //            }
                    //            #endregion
                    //        }
                    //        else
                    //        {
                    //            //否則清除虛擬帳號 (但不清除流水號)
                    //            cancelNo = String.Empty;
                    //            smCancelNo = String.Empty;
                    //            atmCancelNo = String.Empty;
                    //            log.AppendFormat("學號 {0} 的繳費資料金額小於等於 0，所以清除虛擬帳號", studentReceive.StuId).AppendLine();
                    //        }
                    //    }
                    //    #endregion
                    //}
                    //else if (hasUploadSeriorNo)
                    //{
                    //    if (studentReceive.ReceiveAmount > 0)
                    //    {
                    //        //金額大於 0 才要產生虛擬帳號
                    //        #region 上傳資料含流水號
                    //        isNeedSaveSeriorNo = false;
                    //        isNeedSaveCancelNo = true;

                    //        #region 檢查流水號
                    //        if (String.IsNullOrEmpty(studentReceive.SeriorNo))
                    //        {
                    //            errmsg = String.Format("學號 {0} 的上傳繳費資料缺少流水號", studentReceive.StuId);
                    //            log.AppendLine(errmsg);
                    //            continue;
                    //        }
                    //        #endregion

                    //        #region [Old] 取得身份證字號
                    //        //string pId = null;
                    //        //if (String.IsNullOrEmpty(errmsg) && module.IsHasPersonalId)
                    //        //{
                    //        //    errmsg = this.GetStudentPId(studentReceive.ReceiveType, studentReceive.StuId, out pId);
                    //        //    if (!String.IsNullOrEmpty(errmsg))
                    //        //    {
                    //        //        log.AppendLine(errmsg);
                    //        //        continue;
                    //        //    }
                    //        //}
                    //        #endregion

                    //        #region 產生虛擬帳號
                    //        if (String.IsNullOrEmpty(errmsg))
                    //        {
                    //            string customNo = null;
                    //            string checksum = null;
                    //            errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveAmount.Value, out cancelNo, out customNo, out checksum);
                    //            if (String.IsNullOrEmpty(errmsg) && studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount > 0)
                    //            {
                    //                errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveSmamount.Value, out smCancelNo, out customNo, out checksum);
                    //            }
                    //            if (String.IsNullOrEmpty(errmsg) && studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount > 0)
                    //            {
                    //                errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveAtmamount.Value, out atmCancelNo, out customNo, out checksum);
                    //            }
                    //            if (!String.IsNullOrEmpty(errmsg))
                    //            {
                    //                log.AppendLine(errmsg);
                    //                continue;
                    //            }
                    //        }
                    //        #endregion
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        //否則清除虛擬帳號 (但不清除流水號)
                    //        cancelNo = String.Empty;
                    //        smCancelNo = String.Empty;
                    //        atmCancelNo = String.Empty;
                    //        log.AppendFormat("學號 {0} 的繳費資料金額小於等於 0，所以清除虛擬帳號", studentReceive.StuId).AppendLine();
                    //    }
                    //}
                    //else if (hasUploadCancelNo)
                    //{
                    //    if (module.IsD38Kind && !String.IsNullOrWhiteSpace(studentReceive.CancelNo))
                    //    {
                    //        isNeedSaveSeriorNo = false;
                    //        isNeedSaveCancelNo = false;

                    //        #region 特別處理 module.IsD38Kind 類，直接使用 CanceNo
                    //        if (String.IsNullOrEmpty(errmsg))
                    //        {
                    //            cancelNo = studentReceive.CancelNo.Trim();
                    //            if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                    //            {
                    //                studentReceive.CancelAtmno = cancelNo;
                    //            }
                    //            else
                    //            {
                    //                studentReceive.CancelAtmno = String.Empty;
                    //            }
                    //        }
                    //        if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                    //        {
                    //            if (studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                    //            {
                    //                studentReceive.CancelSmno = cancelNo;
                    //            }
                    //            else
                    //            {
                    //                studentReceive.CancelSmno = String.Empty;
                    //            }
                    //        }
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        if (studentReceive.ReceiveAmount > 0)
                    //        {
                    //            #region 上傳資料含虛擬帳號
                    //            isNeedSaveSeriorNo = true;
                    //            isNeedSaveCancelNo = false;

                    //            #region 檢查虛擬帳號
                    //            if (String.IsNullOrWhiteSpace(studentReceive.CancelNo))
                    //            {
                    //                errmsg = String.Format("學號 {0} 的上傳繳費資料缺少虛擬帳號", studentReceive.StuId);
                    //                log.AppendLine(errmsg);
                    //                continue;
                    //            }
                    //            #endregion

                    //            #region [Old] 取得身份證字號
                    //            //string pId = null;
                    //            //if (String.IsNullOrEmpty(errmsg) && module.IsHasPersonalId)
                    //            //{
                    //            //    errmsg = this.GetStudentPId(studentReceive.ReceiveType, studentReceive.StuId, out pId);
                    //            //    if (!String.IsNullOrEmpty(errmsg))
                    //            //    {
                    //            //        log.AppendLine(errmsg);
                    //            //        continue;
                    //            //    }
                    //            //}
                    //            #endregion

                    //            #region 檢查虛擬帳號
                    //            if (String.IsNullOrEmpty(errmsg))
                    //            {
                    //                string myReceiveType = null;
                    //                string myCustomNo = null;
                    //                string myChecksum = null;
                    //                string myYearId = null;
                    //                string myTermId = null;
                    //                string myReceiveId = null;
                    //                string mySeriorNo = null;
                    //                string customNo = null;
                    //                string checksum = null;
                    //                if (module.TryParseCancelNo(studentReceive.CancelNo, isBigReceiveId, out myReceiveType, out myCustomNo, out myChecksum, out myYearId, out myTermId, out myReceiveId, out mySeriorNo))
                    //                {
                    //                    seriorNo = mySeriorNo;
                    //                    errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, seriorNo, isBigReceiveId, studentReceive.ReceiveAmount.Value
                    //                        , out cancelNo, out customNo, out checksum);
                    //                    if (String.IsNullOrEmpty(errmsg) && (customNo != myCustomNo || checksum != myChecksum))
                    //                    {
                    //                        errmsg = String.Format("學號 {0} 的上傳虛擬帳號與系統產生的虛擬帳號不同", studentReceive.StuId);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    errmsg = String.Format("學號 {0} 的上傳虛擬帳號不正確不法拆解", studentReceive.StuId);
                    //                }
                    //                if (!String.IsNullOrEmpty(errmsg))
                    //                {
                    //                    log.AppendLine(errmsg);
                    //                    continue;
                    //                }
                    //            }
                    //            #endregion

                    //            #region 產生虛擬帳號
                    //            if (String.IsNullOrEmpty(errmsg))
                    //            {
                    //                string customNo = null;
                    //                string checksum = null;
                    //                if (String.IsNullOrEmpty(errmsg) && studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount > 0)
                    //                {
                    //                    errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveSmamount.Value, out smCancelNo, out customNo, out checksum);
                    //                }
                    //                if (String.IsNullOrEmpty(errmsg) && studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount > 0)
                    //                {
                    //                    errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveAtmamount.Value, out atmCancelNo, out customNo, out checksum);
                    //                }
                    //                if (!String.IsNullOrEmpty(errmsg))
                    //                {
                    //                    log.AppendLine(errmsg);
                    //                    continue;
                    //                }
                    //            }
                    //            #endregion
                    //            #endregion
                    //        }
                    //        else
                    //        {
                    //            //否則清除虛擬帳號 (但不清除流水號)
                    //            cancelNo = String.Empty;
                    //            smCancelNo = String.Empty;
                    //            atmCancelNo = String.Empty;
                    //            log.AppendFormat("學號 {0} 的繳費資料金額小於等於 0，所以清除虛擬帳號", studentReceive.StuId).AppendLine();
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (studentReceive.ReceiveAmount > 0)
                    //    {
                    //        #region 須取流水號
                    //        isNeedSaveSeriorNo = true;
                    //        isNeedSaveCancelNo = true;

                    //        //有舊的流水號就用就的流水號，為了避免舊流水號位數不同，一律轉成數值後重新轉成字串，並一律更新
                    //        Int64 oldSeriroNo = 0;
                    //        if (!String.IsNullOrEmpty(studentReceive.SeriorNo) && Int64.TryParse(studentReceive.SeriorNo, out oldSeriroNo))
                    //        {
                    //            seriorNo = oldSeriroNo.ToString().PadLeft(module.SeriorNoSize, '0');
                    //            studentReceive.SeriorNo = seriorNo;
                    //        }
                    //        else
                    //        {
                    //            #region 取得流水號
                    //            Int64 nextSNo = snohelper.GetNextSNo(_Factory, snoKey, maxSeriorNo, false);
                    //            if (nextSNo > maxSeriorNo)
                    //            {
                    //                errmsg = String.Format("流水號已超過此虛擬帳號模組允許的最大值 {0} 的限制", maxSeriorNo);
                    //                log.AppendLine(errmsg);
                    //                continue;
                    //            }
                    //            else if (nextSNo == 0)
                    //            {
                    //                errmsg = String.Format("無法取得 {0} 的下一個流水號", snoKey);
                    //                log.AppendLine(errmsg);
                    //                continue;
                    //            }
                    //            else
                    //            {
                    //                seriorNo = nextSNo.ToString().PadLeft(module.SeriorNoSize, '0');
                    //                studentReceive.SeriorNo = seriorNo;
                    //            }
                    //            #endregion
                    //        }

                    //        #region [Old] 取得身份證字號
                    //        //string pId = null;
                    //        //if (String.IsNullOrEmpty(errmsg) && module.IsHasPersonalId)
                    //        //{
                    //        //    errmsg = this.GetStudentPId(studentReceive.ReceiveType, studentReceive.StuId, out pId);
                    //        //    if (!String.IsNullOrEmpty(errmsg))
                    //        //    {
                    //        //        log.AppendLine(errmsg);
                    //        //        continue;
                    //        //    }
                    //        //}
                    //        #endregion

                    //        #region 產生虛擬帳號
                    //        if (String.IsNullOrEmpty(errmsg))
                    //        {
                    //            string customNo = null;
                    //            string checksum = null;
                    //            errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveAmount.Value, out cancelNo, out customNo, out checksum);
                    //            if (String.IsNullOrEmpty(errmsg) && studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount > 0)
                    //            {
                    //                errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveSmamount.Value, out smCancelNo, out customNo, out checksum);
                    //            }
                    //            if (String.IsNullOrEmpty(errmsg) && studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount > 0)
                    //            {
                    //                errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveAtmamount.Value, out atmCancelNo, out customNo, out checksum);
                    //            }
                    //            if (!String.IsNullOrEmpty(errmsg))
                    //            {
                    //                log.AppendLine(errmsg);
                    //                continue;
                    //            }
                    //        }
                    //        #endregion
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        //否則清除虛擬帳號 (但不清除流水號)
                    //        cancelNo = String.Empty;
                    //        smCancelNo = String.Empty;
                    //        atmCancelNo = String.Empty;
                    //        log.AppendFormat("學號 {0} 的繳費資料金額小於等於 0，所以清除虛擬帳號", studentReceive.StuId).AppendLine();
                    //    }
                    //}
                    //#endregion
                    #endregion

                    #region 檢查金額
                    if (studentReceive.ReceiveAmount == null)
                    {
                        log.AppendFormat("學號 {0} 的繳費資料金額未計算", studentReceive.StuId).AppendLine();
                        continue;
                    }
                    #endregion

                    #region 產生流水號與各種虛擬帳號
                    string errmsg = null;
                    string seriorNo = null;
                    string cancelNo = null;
                    string smCancelNo = null;
                    string atmCancelNo = null;
                    bool isOK = false;

                    bool hasUploadSeriorNo = studentReceive.HasUploadSeriorNo();
                    bool hasUploadCancelNo = studentReceive.HasUploadCancelNo();

                    if (hasUploadCancelNo)
                    {
                        #region 自訂虛擬帳號
                        #region 檢查自訂虛擬帳號是否正確
                        if (String.IsNullOrEmpty(studentReceive.CancelNo))
                        {
                            errmsg = String.Format("學號 {0} 的繳費資料缺少自訂虛擬帳號", studentReceive.StuId);
                            log.AppendLine(errmsg);
                            continue;
                        }
                        else
                        {
                            errmsg = helper.CheckCustomCancelNo(studentReceive.CancelNo, module, studentReceive.CancelNo, studentReceive.ReceiveAmount.Value);
                            if (!String.IsNullOrEmpty(errmsg))
                            {
                                errmsg = String.Concat("自訂虛擬帳號不正確，錯誤訊息：", errmsg);
                                log.AppendLine(errmsg);
                                continue;
                            }
                        }
                        #endregion

                        #region 直接用自訂虛擬帳號
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            seriorNo = studentReceive.SeriorNo;

                            cancelNo = studentReceive.CancelNo;
                            if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                            {
                                atmCancelNo = cancelNo;
                            }
                            else
                            {
                                atmCancelNo = String.Empty;
                                noATMCancelNoCount++;
                            }
                            if (studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                            {
                                smCancelNo = cancelNo;
                            }
                            else
                            {
                                smCancelNo = String.Empty;
                            }
                            isOK = true;
                        }
                        #endregion
                        #endregion
                    }
                    else
                    {
                        #region 非自訂虛擬帳號
                        if (studentReceive.ReceiveAmount > 0)
                        {
                            #region 處理流水號
                            if (hasUploadSeriorNo)
                            {
                                #region 檢查流水號
                                if (String.IsNullOrEmpty(studentReceive.SeriorNo))
                                {
                                    errmsg = String.Format("學號 {0} 的繳費資料缺少自訂流水號", studentReceive.StuId);
                                    log.AppendLine(errmsg);
                                    continue;
                                }
                                else
                                {
                                    Int64 oldSeriroNo = 0;
                                    if (!Int64.TryParse(studentReceive.SeriorNo, out oldSeriroNo))
                                    {
                                        errmsg = String.Format("學號 {0} 的繳費資料的自訂流水號不是數字", studentReceive.StuId);
                                        log.AppendLine(errmsg);
                                        continue;
                                    }
                                    else if (oldSeriroNo > maxSeriorNo)
                                    {
                                        errmsg = String.Format("學號 {0} 的繳費資料的自訂流水號超過最大值 ({1})", studentReceive.StuId, maxSeriorNo);
                                        log.AppendLine(errmsg);
                                        continue;
                                    }
                                    else
                                    {
                                        seriorNo = oldSeriroNo.ToString().PadLeft(module.SeriorNoSize, '0');
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 處理流水號
                                Int64 oldSeriroNo = 0;
                                if (!String.IsNullOrEmpty(studentReceive.SeriorNo) && Int64.TryParse(studentReceive.SeriorNo, out oldSeriroNo))
                                {
                                    #region 有舊的流水號就用就的流水號，為了避免舊流水號位數不同，一律轉成數值後重新轉成字串
                                    if (oldSeriroNo > maxSeriorNo)
                                    {
                                        errmsg = String.Format("學號 {0} 的繳費資料的原流水號超過最大值 ({1})", studentReceive.StuId, maxSeriorNo);
                                        log.AppendLine(errmsg);
                                        continue;
                                    }
                                    else
                                    {
                                        seriorNo = oldSeriroNo.ToString().PadLeft(module.SeriorNoSize, '0');
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 取得流水號
                                    Int64 nextSNo = snohelper.GetNextSNo(_Factory, snoKey, maxSeriorNo, false);
                                    if (nextSNo > maxSeriorNo)
                                    {
                                        errmsg = String.Format("流水號已超過此虛擬帳號模組允許的最大值 {0} 的限制", maxSeriorNo);
                                        log.AppendLine(errmsg);
                                        continue;
                                    }
                                    else if (nextSNo == 0)
                                    {
                                        errmsg = String.Format("無法取得 {0} 的下一個流水號", snoKey);
                                        log.AppendLine(errmsg);
                                        continue;
                                    }
                                    else
                                    {
                                        seriorNo = nextSNo.ToString().PadLeft(module.SeriorNoSize, '0');
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion

                            #region 處理虛擬帳號
                            if (String.IsNullOrEmpty(errmsg))
                            {
                                string customNo = null;
                                string checksum = null;
                                //由系統產生虛擬帳號
                                errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, seriorNo, isBigReceiveId, studentReceive.ReceiveAmount.Value, out cancelNo, out customNo, out checksum);
                                if (String.IsNullOrEmpty(errmsg))
                                {
                                    //土銀的超商與臨櫃收續費不是0 就是外加 或學校負擔，所以金額與虛擬帳號都不變
                                    if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                                    {
                                        atmCancelNo = cancelNo;
                                    }
                                    else
                                    {
                                        atmCancelNo = String.Empty;
                                        noATMCancelNoCount++;
                                    }
                                    if (studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                                    {
                                        smCancelNo = cancelNo;
                                    }
                                    else
                                    {
                                        smCancelNo = String.Empty;
                                    }
                                    isOK = true;
                                }
                                else
                                {
                                    errmsg = String.Format("學號 {0} 的繳費資料產生虛擬帳號失敗，錯誤訊息：{1}", studentReceive.StuId, errmsg);
                                    log.AppendLine(errmsg);
                                    continue;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            //否則清除虛擬帳號 (但不清除流水號)
                            isOK = true;
                            seriorNo = studentReceive.SeriorNo;
                            cancelNo = String.Empty;
                            smCancelNo = String.Empty;
                            atmCancelNo = String.Empty;
                            noATMCancelNoCount++;
                            log.AppendFormat("學號 {0} 的繳費資料金額小於等於 0，所以清除虛擬帳號", studentReceive.StuId).AppendLine();
                        }
                        #endregion
                    }
                    #endregion

                    #region 更新資料
                    if (String.IsNullOrEmpty(errmsg) && isOK)
                    {
                        Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, studentReceive.ReceiveType)
                            .And(StudentReceiveEntity.Field.YearId, studentReceive.YearId)
                            .And(StudentReceiveEntity.Field.TermId, studentReceive.TermId)
                            .And(StudentReceiveEntity.Field.DepId, studentReceive.DepId)
                            .And(StudentReceiveEntity.Field.ReceiveId, studentReceive.ReceiveId)
                            .And(StudentReceiveEntity.Field.StuId, studentReceive.StuId)
                            .And(StudentReceiveEntity.Field.OldSeq, studentReceive.OldSeq);

                        #region [20150915] 加強更新條件，避免更新到已繳、已銷的資料
                        {
                            where.And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty));
                            where.And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));
                            where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                        }
                        #endregion

                        KeyValueList fieldValues = new KeyValueList(6);
                        if (seriorNo != studentReceive.SeriorNo)
                        {
                            fieldValues.Add(StudentReceiveEntity.Field.SeriorNo, seriorNo ?? String.Empty);
                        }
                        if (cancelNo != studentReceive.CancelNo && !hasUploadCancelNo)
                        {
                            fieldValues.Add(StudentReceiveEntity.Field.CancelNo, cancelNo ?? String.Empty);
                        }
                        fieldValues.Add(StudentReceiveEntity.Field.CancelSmno, smCancelNo ?? String.Empty);
                        fieldValues.Add(StudentReceiveEntity.Field.CancelAtmno, atmCancelNo ?? String.Empty);

                        #region 虛擬帳號有異動則中信資料發送旗標清為 0，並更新 UpdateDate
                        if (orgCancelNo != cancelNo)
                        {
                            fieldValues.Add(StudentReceiveEntity.Field.CFlag, "0");

                            fieldValues.Add(StudentReceiveEntity.Field.UpdateDate, DateTime.Now);
                        }
                        #endregion

                        int count = 0;
                        Result result = _Factory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                        if (result.IsSuccess)
                        {
                            successCount++;
                            //log.AppendFormat("處理學號 {0} 的虛擬帳號 {1} 成功", studentReceive.StuId, studentReceive.CancelNo).AppendLine();
                        }
                        else
                        {
                            log.AppendFormat("儲存學號 {0} 的虛擬帳號資料失敗，錯誤訊息：{1}", studentReceive.StuId, result.Message).AppendLine();
                            continue;
                        }
                    }
                    else
                    {
                        log.AppendFormat("產生學號 {0} 的虛擬帳號失敗，錯誤訊息：{1}", studentReceive.StuId, errmsg).AppendLine();
                        continue;
                    }
                    #endregion
                }

                logmsg = log.ToString();
            }
            #endregion

            if (successCount == 0)
            {
                return new Result(false, "無任何一筆資料被處理成功", CoreStatusCode.D_NOT_DATA_UPDATE, null);
            }
            else
            {
                //任一筆成功就算成功
                return new Result(true);
            }
        }
        #endregion

        /// <summary>
        /// 處理 CP (產生繳費金額) 批次處理序列作業
        /// </summary>
        /// <param name="job">指定要處理的 jobcube</param>
        /// <param name="logmsg">傳回處理日誌</param>
        /// <param name="totalCount">傳回處理筆數</param>
        /// <param name="successCount">傳回處理成功筆數</param>
        /// <returns>傳回處理結果</returns>
        public Result ProcessCPJob(JobcubeEntity job, out string logmsg, out Int32 totalCount, out Int32 successCount)
        {
            logmsg = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                logmsg = "資料存取物件未準備好";
                return new Result(false, logmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job.Jtypeid != JobCubeTypeCodeTexts.CP)
            {
                logmsg = String.Format("[產生金額]批次處理序列 {0} 的類別不符合", job.Jno);
                return new Result(false, logmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string yearId = job.Jyear;
            string termId = job.Jterm;
            string depId = job.Jdep;
            string receiveId = job.Jrecid;
            string para = job.Jparam.Trim();
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || String.IsNullOrEmpty(receiveId) || String.IsNullOrEmpty(para))
            {
                logmsg = String.Format("[產生金額]批次處理序列 {0} 缺少業務別碼、學年代碼、學期代碼或代收費用別代碼的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region 拆解 JobcubeEntity 參數
            bool isParamOK = false;
            int upNo = 0;

            string pReceiveType = null;
            string pYearId = null;
            string pTermId = null;
            string pDepId = null;
            string pReceiveId = null;
            string pUpNo = null;
            string pType = null;
            isParamOK = JobcubeEntity.ParseCPParameter(para, out pReceiveType, out pYearId, out pTermId, out pDepId, out pReceiveId
                            , out pUpNo, out pType);
            if (String.IsNullOrEmpty(pUpNo) || !Int32.TryParse(pUpNo, out upNo))
            {
                logmsg = "[產生金額]批次處理序列缺少上傳繳費資料的批號的參數或參數值不正確";
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }
            if (pType != "1" && pType != "2")
            {
                logmsg = "[產生金額]批次處理序列缺少計算方式或參數值不正確";
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region 取繳費資料
            StudentReceiveEntity[] studentReceives = null;
            Expression w1 = new Expression(StudentReceiveEntity.Field.CancelFlag, null).Or(StudentReceiveEntity.Field.CancelFlag, "");      //未銷帳
            Expression w2 = new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, "");    //未繳費
            Expression w3 = new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, "");    //未入帳
            Expression w4 = new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, "");      //未繳費&未入帳
            //TODO: 暫時不檔，因為擋了有銷編的資料沒辦法算
            //Expression w3 = new Expression(StudentReceiveEntity.Field.CancelNo, null).Or(StudentReceiveEntity.Field.CancelNo, "");    //無銷編
            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, pReceiveType)
                .And(StudentReceiveEntity.Field.YearId, pYearId)
                .And(StudentReceiveEntity.Field.TermId, pTermId)
                .And(StudentReceiveEntity.Field.DepId, pDepId)
                .And(StudentReceiveEntity.Field.ReceiveId, pReceiveId)
                .And(StudentReceiveEntity.Field.UpNo, pUpNo)
                .And(w1)    //未銷帳
                .And(w2)    //未繳費
                .And(w3)    //未銷帳
                .And(w4);   //未繳費&未入帳
                //TODO: 暫時不檔，因為擋了有銷編的資料沒辦法算
                //.And(w3) //銷帳編號要為空白或是null
            KeyValueList<OrderByEnum> orderbys = null;

            Result result = _Factory.SelectAll<StudentReceiveEntity>(where, orderbys, out studentReceives);
            if (!result.IsSuccess)
            {
                logmsg = "讀取繳費單資料失敗，錯誤訊息：" + result.Message;
                return new Result(false, logmsg, result.Code, result.Exception);
            }
            if (studentReceives == null || studentReceives.Length == 0)
            {
                logmsg = "查無任何可計算的繳費資料";
                return new Result(false, logmsg, result.Code, result.Exception);
            }
            totalCount = studentReceives == null ? 0 : studentReceives.Length;
            #endregion

            #region 計算金額
            BillAmountHelper amountHelper = new BillAmountHelper();
            BillAmountHelper.CalculateType calc_type = BillAmountHelper.CalculateType.byAmount;
            if (pType == BillingTypeCodeTexts.BY_STANDARD)
            {
                calc_type = BillAmountHelper.CalculateType.byStandard;
            }
            if (!amountHelper.CalcBillsAmount(ref studentReceives, calc_type)) //表示是整批失敗
            {
                logmsg = amountHelper.err_mgs;
                return new Result(false, logmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }
            else
            {
                logmsg = amountHelper.err_mgs;

                int[] failindexs = null;
                int count = 0;
                //TODO: Update ? UpdateFileds ?
                result = _Factory.Update(studentReceives, false, out count, out failindexs);
                string log = "";
                StringBuilder logs = new StringBuilder();
                if (failindexs != null && failindexs.Length>0)
                {
                    foreach(int i in failindexs)
                    {
                        log = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},stu_id={5}更新失敗", studentReceives[i].ReceiveType, studentReceives[i].YearId, studentReceives[i].TermId, studentReceives[i].DepId, studentReceives[i].ReceiveId, studentReceives[i].StuId);
                        logs.AppendLine(log);
                    }
                }
                log = log.ToString();
                if (log != "")
                {
                    logmsg += System.Environment.NewLine + log;
                }
                return new Result(true, logmsg, ErrorCode.NORMAL_STATUS, null);
            }
            #endregion
        }


        /// <summary>
        /// 取得指定代收類別與學號的學生身份證字號
        /// </summary>
        /// <param name="reseiveType">指定代收類別</param>
        /// <param name="stuId">指定代收類別</param>
        /// <param name="pId">傳回學生身份證字號</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        private string GetStudentPId(string reseiveType, string stuId, out string pId)
        {
            pId = null;
            StudentMasterEntity student = null;
            Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, reseiveType)
                .And(StudentMasterEntity.Field.Id, stuId);
            Result result = _Factory.SelectFirst<StudentMasterEntity>(where, null, out student);
            if (!result.IsSuccess)
            {
                return String.Format("查詢學號 {0} 的身份證字號失敗，錯誤訊息：{1}", stuId, result.Message);
            }
            if (student != null)
            {
                pId = student.IdNumber;
            }
            if (String.IsNullOrWhiteSpace(pId))
            {
                return String.Format("查無學號 {0} 的學生資料或該學生無身份證字號", stuId);
            }
            return null;
        }

        #region [MDY:20220820] 2022擴充案 取中、英文 StudentReceiveView
        /// <summary>
        /// 取得符合指定條件的 StudentReceiveView 集合
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="qType"></param>
        /// <param name="qValue"></param>
        /// <param name="allAmount"></param>
        /// <param name="pdfType"></param>
        /// <param name="module"></param>
        /// <param name="isEngEnabled"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        private Result GetStudentReceiveViews(string receiveType, string yearId, string termId, string depId, string receiveId
            , string qType, string qValue, bool allAmount, GenPDFHelper.PDFType pdfType
            , CancelNoHelper.Module module, bool isEngEnabled, out StudentReceiveView[] datas)
        {
            datas = null;

            #region 組 SQL
            KeyValueList parameters = new KeyValueList(6);

            parameters.Add("@RECEIVE_TYPE", receiveType);
            parameters.Add("@YEAR_ID", yearId);
            parameters.Add("@TERM_ID", termId);
            parameters.Add("@DEP_ID", depId);
            parameters.Add("@RECEIVE_ID", receiveId);

            string orderbySql = null;
            List<string> andSqls = new List<string>(5);

            string errmsg = null;
            switch (qType)
            {
                case "1":
                    #region 產生所有繳費單
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.StuId}";
                    }
                    #endregion
                    break;
                case "2":
                    #region 自訂產生繳費單流水號
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.SeriorNo}";

                        if (String.IsNullOrWhiteSpace(qValue))
                        {
                            errmsg = "批次處理序列缺少條件值的參數或參數值不正確";
                            return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                        }
                        string[] sNoRanges = qValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sNoRanges.Length != 2)
                        {
                            errmsg = "批次處理序列缺少條件值的參數或參數值不正確";
                            return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                        }
                        andSqls.Add($"   AND (@SERIORNO_S <= {StudentReceiveView.Field.SeriorNo} AND {StudentReceiveView.Field.SeriorNo} <= @SERIORNO_E)");
                        parameters.Add("@SERIORNO_S", sNoRanges[0].PadLeft(module.SeriorNoSize, '0'));
                        parameters.Add("@SERIORNO_E", sNoRanges[1].PadLeft(module.SeriorNoSize, '0'));
                    }
                    #endregion
                    break;
                case "3":
                    #region 依批號產生
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.UpOrder}";

                        andSqls.Add($"   AND {StudentReceiveView.Field.UpNo} = @UP_NO");
                        parameters.Add("@UP_NO", qValue);
                    }
                    #endregion
                    break;
                case "4":
                    #region 依學號產生
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.OldSeq}";

                        andSqls.Add($"   AND {StudentReceiveView.Field.StuId} = @STU_ID");
                        parameters.Add("@STU_ID", qValue);
                    }
                    #endregion
                    break;
                case "5":
                    #region 依科系產生
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.CreateDate}";

                        andSqls.Add($"   AND {StudentReceiveView.Field.MajorId} = @MAJOR_ID");
                        parameters.Add("@MAJOR_ID", qValue);
                    }
                    #endregion
                    break;
                case "6":
                    #region 依年級產生
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.CreateDate}";

                        andSqls.Add($"   AND {StudentReceiveView.Field.StuGrade} = @STU_GRADE");
                        parameters.Add("@STU_GRADE", qValue);
                    }
                    #endregion
                    break;
                default:
                    errmsg = "批次處理序列缺少上條件類別參數或參數值不正確";
                    return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            //只取應繳金額大於 0 的資料
            if (!allAmount)
            {
                andSqls.Add($"   AND {StudentReceiveView.Field.ReceiveAmount} > 0");
            }

            if (pdfType == GenPDFHelper.PDFType.Bill)
            {
                //繳費單：只取未繳費
                andSqls.Add($"   AND ({StudentReceiveView.Field.ReceiveDate} IS NULL OR {StudentReceiveView.Field.ReceiveDate} = '')");
            }
            else
            {
                //收據：只取已銷
                andSqls.Add($"   AND ({StudentReceiveView.Field.AccountDate} IS NOT NULL AND {StudentReceiveView.Field.AccountDate} != '')");
            }

            andSqls.Add(orderbySql);

            string sql = null;
            if (isEngEnabled)
            {
                sql = $@"
SELECT SR.*
     , ISNULL(RI.[Bill_Valid_Date], '')    AS [Bill_Valid_Date]
     , ISNULL(RI.[Bill_Close_Date], '')    AS [Bill_Close_Date]
     , ISNULL(RI.[Invoice_Close_Date], '') AS [Invoice_Close_Date]
     , ISNULL(RI.[Pay_Date], '')           AS [Pay_Due_Date1]
     , ISNULL(RI.[Pay_Due_Date2], '')      AS [Pay_Due_Date2]
     , ISNULL(RI.[Pay_Due_Date3], '')      AS [Pay_Due_Date3]
     , ISNULL((SELECT [Stu_Name] FROM [{StudentMasterEntity.TABLE_NAME}] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Year_EName],     [Year_Name])     ELSE [Year_Name]     END) FROM [Year_List]      AS YL  WHERE YL.[Year_Id]       = SR.[Year_Id]), '')                                                                                                                                                   AS [Year_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Term_EName],     [Term_Name])     ELSE [Term_Name]     END) FROM [Term_List]      AS TL  WHERE TL.[Receive_Type]  = SR.[Receive_Type] AND TL.[Year_Id]  = SR.[Year_Id] AND TL.[Term_Id]  = SR.[Term_Id]), '')                                                                            AS [Term_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Dept_EName],     [Dept_Name])     ELSE [Dept_Name]     END) FROM [Dept_List]      AS DTL WHERE DTL.[Receive_Type] = SR.[Receive_Type] AND DTL.[Year_Id] = SR.[Year_Id] AND DTL.[Term_Id] = SR.[Term_Id] AND DTL.[Dept_Id] = SR.[Dept_Id]), '')                                           AS [Dept_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Receive_EName],  [Receive_Name])  ELSE [Receive_Name]  END) FROM [Receive_List]   AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Receive_Id]   = SR.[Receive_Id]), '')    AS [Receive_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([College_EName],  [College_Name])  ELSE [College_Name]  END) FROM [College_List]   AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[College_Id]   = SR.[College_Id]), '')    AS [College_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Major_EName],    [Major_Name])    ELSE [Major_Name]    END) FROM [Major_List]     AS ML  WHERE ML.[Receive_Type]  = SR.[Receive_Type] AND ML.[Year_Id]  = SR.[Year_Id] AND ML.[Term_Id]  = SR.[Term_Id] AND ML.[Dep_Id]   = SR.[Dep_Id] AND ML.[Major_Id]     = SR.[Major_Id]), '')      AS [Major_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Class_EName],    [Class_Name])    ELSE [Class_Name]    END) FROM [Class_List]     AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[Class_Id]     = SR.[Class_Id]), '')      AS [Class_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Reduce_EName],   [Reduce_Name])   ELSE [Reduce_Name]   END) FROM [Reduce_List]    AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Reduce_Id]    = SR.[Reduce_Id]), '')     AS [Reduce_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Dorm_EName],     [Dorm_Name])     ELSE [Dorm_Name]     END) FROM [Dorm_List]      AS DL  WHERE DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id] AND DL.[Dorm_Id]      = SR.[Dorm_Id]), '')       AS [Dorm_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Loan_EName],     [Loan_Name])     ELSE [Loan_Name]     END) FROM [Loan_List]      AS LL  WHERE LL.[Receive_Type]  = SR.[Receive_Type] AND LL.[Year_Id]  = SR.[Year_Id] AND LL.[Term_Id]  = SR.[Term_Id] AND LL.[Dep_Id]   = SR.[Dep_Id] AND LL.[Loan_Id]      = SR.[Loan_Id]), '')       AS [Loan_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List1] AS IL1 WHERE IL1.[Receive_Type] = SR.[Receive_Type] AND IL1.[Year_Id] = SR.[Year_Id] AND IL1.[Term_Id] = SR.[Term_Id] AND IL1.[Dep_Id]  = SR.[Dep_Id] AND IL1.[Identify_Id] = SR.[Identify_Id01]), '') AS [Identify_Name01]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List2] AS IL2 WHERE IL2.[Receive_Type] = SR.[Receive_Type] AND IL2.[Year_Id] = SR.[Year_Id] AND IL2.[Term_Id] = SR.[Term_Id] AND IL2.[Dep_Id]  = SR.[Dep_Id] AND IL2.[Identify_Id] = SR.[Identify_Id02]), '') AS [Identify_Name02]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List3] AS IL3 WHERE IL3.[Receive_Type] = SR.[Receive_Type] AND IL3.[Year_Id] = SR.[Year_Id] AND IL3.[Term_Id] = SR.[Term_Id] AND IL3.[Dep_Id]  = SR.[Dep_Id] AND IL3.[Identify_Id] = SR.[Identify_Id03]), '') AS [Identify_Name03]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List4] AS IL4 WHERE IL4.[Receive_Type] = SR.[Receive_Type] AND IL4.[Year_Id] = SR.[Year_Id] AND IL4.[Term_Id] = SR.[Term_Id] AND IL4.[Dep_Id]  = SR.[Dep_Id] AND IL4.[Identify_Id] = SR.[Identify_Id04]), '') AS [Identify_Name04]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List5] AS IL5 WHERE IL5.[Receive_Type] = SR.[Receive_Type] AND IL5.[Year_Id] = SR.[Year_Id] AND IL5.[Term_Id] = SR.[Term_Id] AND IL5.[Dep_Id]  = SR.[Dep_Id] AND IL5.[Identify_Id] = SR.[Identify_Id05]), '') AS [Identify_Name05]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List6] AS IL6 WHERE IL6.[Receive_Type] = SR.[Receive_Type] AND IL6.[Year_Id] = SR.[Year_Id] AND IL6.[Term_Id] = SR.[Term_Id] AND IL6.[Dep_Id]  = SR.[Dep_Id] AND IL6.[Identify_Id] = SR.[Identify_Id06]), '') AS [Identify_Name06]
     , ISNULL((SELECT [Channel_Name] FROM [{ChannelSetEntity.TABLE_NAME}] AS CS WHERE CS.[Channel_Id] = SR.[Receive_Way]), '') AS [Receive_Way_Name]
  FROM [{StudentReceiveEntity.TABLE_NAME}] AS SR
  LEFT JOIN [{SchoolRidEntity.TABLE_NAME}] AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]
  LEFT JOIN [{SchoolRTypeEntity.TABLE_NAME}] AS SC ON SC.[Receive_Type] = SR.[Receive_Type]
 WHERE SR.[Receive_Type] = @RECEIVE_TYPE AND SR.[Year_Id] = @YEAR_ID AND SR.[Term_Id] = @TERM_ID AND SR.[Dep_Id] = @DEP_ID AND SR.[Receive_Id] = @RECEIVE_ID
{(andSqls.Count == 0 ? null : String.Join(Environment.NewLine, andSqls))}
".Trim();
            }
            else
            {
                sql = $@"
SELECT SR.*
     , ISNULL(RI.[Bill_Valid_Date], '')    AS [Bill_Valid_Date]
     , ISNULL(RI.[Bill_Close_Date], '')    AS [Bill_Close_Date]
     , ISNULL(RI.[Invoice_Close_Date], '') AS [Invoice_Close_Date]
     , ISNULL(RI.[Pay_Date], '')           AS [Pay_Due_Date1]
     , ISNULL(RI.[Pay_Due_Date2], '')      AS [Pay_Due_Date2]
     , ISNULL(RI.[Pay_Due_Date3], '')      AS [Pay_Due_Date3]
     , ISNULL((SELECT [Stu_Name] FROM [{StudentMasterEntity.TABLE_NAME}] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
     , ISNULL((SELECT [Year_Name]     FROM [Year_List]      AS YL  WHERE YL.[Year_Id]       = SR.[Year_Id]), '')                                                                                                                                                   AS [Year_Name]
     , ISNULL((SELECT [Term_Name]     FROM [Term_List]      AS TL  WHERE TL.[Receive_Type]  = SR.[Receive_Type] AND TL.[Year_Id]  = SR.[Year_Id] AND TL.[Term_Id]  = SR.[Term_Id]), '')                                                                            AS [Term_Name]
     , ISNULL((SELECT [Dept_Name]     FROM [Dept_List]      AS DTL WHERE DTL.[Receive_Type] = SR.[Receive_Type] AND DTL.[Year_Id] = SR.[Year_Id] AND DTL.[Term_Id] = SR.[Term_Id] AND DTL.[Dept_Id] = SR.[Dept_Id]), '')                                           AS [Dept_Name]
     , ISNULL((SELECT [Receive_Name]  FROM [Receive_List]   AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Receive_Id]   = SR.[Receive_Id]), '')    AS [Receive_Name]
     , ISNULL((SELECT [College_Name]  FROM [College_List]   AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[College_Id]   = SR.[College_Id]), '')    AS [College_Name]
     , ISNULL((SELECT [Major_Name]    FROM [Major_List]     AS ML  WHERE ML.[Receive_Type]  = SR.[Receive_Type] AND ML.[Year_Id]  = SR.[Year_Id] AND ML.[Term_Id]  = SR.[Term_Id] AND ML.[Dep_Id]   = SR.[Dep_Id] AND ML.[Major_Id]     = SR.[Major_Id]), '')      AS [Major_Name]
     , ISNULL((SELECT [Class_Name]    FROM [Class_List]     AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[Class_Id]     = SR.[Class_Id]), '')      AS [Class_Name]
     , ISNULL((SELECT [Reduce_Name]   FROM [Reduce_List]    AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Reduce_Id]    = SR.[Reduce_Id]), '')     AS [Reduce_Name]
     , ISNULL((SELECT [Dorm_Name]     FROM [Dorm_List]      AS DL  WHERE DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id] AND DL.[Dorm_Id]      = SR.[Dorm_Id]), '')       AS [Dorm_Name]
     , ISNULL((SELECT [Loan_Name]     FROM [Loan_List]      AS LL  WHERE LL.[Receive_Type]  = SR.[Receive_Type] AND LL.[Year_Id]  = SR.[Year_Id] AND LL.[Term_Id]  = SR.[Term_Id] AND LL.[Dep_Id]   = SR.[Dep_Id] AND LL.[Loan_Id]      = SR.[Loan_Id]), '')       AS [Loan_Name]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List1] AS IL1 WHERE IL1.[Receive_Type] = SR.[Receive_Type] AND IL1.[Year_Id] = SR.[Year_Id] AND IL1.[Term_Id] = SR.[Term_Id] AND IL1.[Dep_Id]  = SR.[Dep_Id] AND IL1.[Identify_Id] = SR.[Identify_Id01]), '') AS [Identify_Name01]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List2] AS IL2 WHERE IL2.[Receive_Type] = SR.[Receive_Type] AND IL2.[Year_Id] = SR.[Year_Id] AND IL2.[Term_Id] = SR.[Term_Id] AND IL2.[Dep_Id]  = SR.[Dep_Id] AND IL2.[Identify_Id] = SR.[Identify_Id02]), '') AS [Identify_Name02]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List3] AS IL3 WHERE IL3.[Receive_Type] = SR.[Receive_Type] AND IL3.[Year_Id] = SR.[Year_Id] AND IL3.[Term_Id] = SR.[Term_Id] AND IL3.[Dep_Id]  = SR.[Dep_Id] AND IL3.[Identify_Id] = SR.[Identify_Id03]), '') AS [Identify_Name03]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List4] AS IL4 WHERE IL4.[Receive_Type] = SR.[Receive_Type] AND IL4.[Year_Id] = SR.[Year_Id] AND IL4.[Term_Id] = SR.[Term_Id] AND IL4.[Dep_Id]  = SR.[Dep_Id] AND IL4.[Identify_Id] = SR.[Identify_Id04]), '') AS [Identify_Name04]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List5] AS IL5 WHERE IL5.[Receive_Type] = SR.[Receive_Type] AND IL5.[Year_Id] = SR.[Year_Id] AND IL5.[Term_Id] = SR.[Term_Id] AND IL5.[Dep_Id]  = SR.[Dep_Id] AND IL5.[Identify_Id] = SR.[Identify_Id05]), '') AS [Identify_Name05]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List6] AS IL6 WHERE IL6.[Receive_Type] = SR.[Receive_Type] AND IL6.[Year_Id] = SR.[Year_Id] AND IL6.[Term_Id] = SR.[Term_Id] AND IL6.[Dep_Id]  = SR.[Dep_Id] AND IL6.[Identify_Id] = SR.[Identify_Id06]), '') AS [Identify_Name06]
     , ISNULL((SELECT [Channel_Name] FROM [{ChannelSetEntity.TABLE_NAME}] AS CS WHERE CS.[Channel_Id] = SR.[Receive_Way]), '') AS [Receive_Way_Name]
  FROM [{StudentReceiveEntity.TABLE_NAME}] AS SR
  LEFT JOIN [{SchoolRidEntity.TABLE_NAME}] AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]
 WHERE SR.[Receive_Type] = @RECEIVE_TYPE AND SR.[Year_Id] = @YEAR_ID AND SR.[Term_Id] = @TERM_ID AND SR.[Dep_Id] = @DEP_ID AND SR.[Receive_Id] = @RECEIVE_ID
{(andSqls.Count == 0 ? null : String.Join(Environment.NewLine, andSqls))}
".Trim();
            }
            #endregion

            Result result = _Factory.SelectSql<StudentReceiveView>(sql, parameters, 0, 1, out datas);
            return result;
        }
        #endregion

        /// <summary>
        /// 處理 CP (產生繳費單) 與 CI (產生繳費收據) 批次處理序列作業  (這個好像被 ProcessPDFxJobByAsync 取代了)
        /// </summary>
        /// <param name="job">指定要處理的 jobcube</param>
        /// <param name="tempPath">指定暫存檔路徑</param>
        /// <param name="totalCount">傳回處理筆數</param>
        /// <param name="pdfFile">傳回產生的 PDF 檔路徑檔名</param>
        /// <returns>傳回處理結果</returns>
        public Result ProcessPDFxJob(JobcubeEntity job, string outPath, out Int32 totalCount, out string pdfFile)
        {
            totalCount = 0;
            pdfFile = null;
            string errmsg = null;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                errmsg = "資料存取物件未準備好";
                return new Result(false, errmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            GenPDFHelper.PDFType pdfType = GenPDFHelper.PDFType.None;
            switch (job.Jtypeid)
            {
                case JobCubeTypeCodeTexts.PDFB:
                    pdfType = GenPDFHelper.PDFType.Bill;
                    break;
                case JobCubeTypeCodeTexts.PDFR:
                    pdfType = GenPDFHelper.PDFType.Receipt;
                    break;
                default:
                    errmsg = String.Format("批次處理序列 {0} 的類別不符合", job.Jno);
                    return new Result(false, errmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string yearId = job.Jyear;
            string termId = job.Jterm;
            string depId = job.Jdep;
            string receiveId = job.Jrecid;
            if (String.IsNullOrEmpty(receiveType)
                || String.IsNullOrEmpty(yearId)
                || String.IsNullOrEmpty(termId)
                || String.IsNullOrEmpty(receiveId))
            {
                errmsg = String.Format("批次處理序列 {0} 缺少業務別碼、學年代碼、學期代碼或代收費用別代碼的資料參數或資料不正確", job.Jno);
                return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            if (String.IsNullOrEmpty(outPath))
            {
                outPath = Path.GetTempPath();
            }
            else
            {
                outPath = outPath.Trim();
            }
            #endregion

            #region 取得虛擬帳號模組資料
            CancelNoHelper cnoHelper = new CancelNoHelper();
            CancelNoHelper.Module module = cnoHelper.GetModuleByReceiveType(receiveType);
            if (module == null)
            {
                errmsg = String.Format("無法取得業務別碼 {0} 的虛擬帳號模組資訊", receiveType);
                return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region 拆解 JobcubeEntity 參數
            #region [MDY:202203XX] 2022擴充案 是否英文介面
            bool isParamOK = false;
            string qType = null;
            string qValue = null;
            bool allAmount = false;
            bool isEngUI = false;
            {
                string pReceiveType = null;
                string pYearId = null;
                string pTermId = null;
                string pDepId = null;
                string pReceiveId = null;
                isParamOK = JobcubeEntity.ParsePDBxParameter(job.Jparam, out pReceiveType, out pYearId, out pTermId, out pDepId, out pReceiveId
                                , out qType, out qValue, out allAmount, out isEngUI);
            }
            #endregion
            #endregion

            #region StudentReceiveView
            StudentReceiveView[] studentReceives = null;
            {
                #region [MDY:20220820] 2022擴充案 改用 GetStudentReceiveViews() 取英文資料
                #region [OLD]
                //KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                //Expression where = new Expression(StudentReceiveView.Field.ReceiveType, receiveType)
                //    .And(StudentReceiveView.Field.YearId, yearId)
                //    .And(StudentReceiveView.Field.TermId, termId)
                //    .And(StudentReceiveView.Field.DepId, depId)
                //    .And(StudentReceiveView.Field.ReceiveId, receiveId);
                //switch (qType)
                //{
                //    case "1":   //產生所有繳費單
                //        orderbys.Add(StudentReceiveView.Field.StuId, OrderByEnum.Asc);
                //        break;
                //    case "2":   //自訂產生繳費單流水號
                //        #region
                //        {
                //            orderbys.Add(StudentReceiveView.Field.SeriorNo, OrderByEnum.Asc);
                //            if (String.IsNullOrWhiteSpace(qValue))
                //            {
                //                errmsg = "批次處理序列缺少條件值的參數或參數值不正確";
                //                return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                //            }
                //            string[] sNoRanges = qValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                //            if (sNoRanges.Length != 2)
                //            {
                //                errmsg = "批次處理序列缺少條件值的參數或參數值不正確";
                //                return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                //            }
                //            where
                //                .And(StudentReceiveView.Field.SeriorNo, RelationEnum.GreaterEqual, sNoRanges[0].PadLeft(module.SeriorNoSize, '0'))
                //                .And(StudentReceiveView.Field.SeriorNo, RelationEnum.LessEqual, sNoRanges[1].PadLeft(module.SeriorNoSize, '0'));
                //        }
                //        #endregion
                //        break;
                //    case "3":   //依批號產生
                //        orderbys.Add(StudentReceiveView.Field.UpOrder, OrderByEnum.Asc);
                //        where.And(StudentReceiveView.Field.UpNo, qValue);
                //        break;
                //    case "4":   //依學號產生
                //        where.And(StudentReceiveView.Field.StuId, qValue);
                //        break;
                //    case "5":   //依科系產生
                //        where.And(StudentReceiveView.Field.MajorId, qValue);
                //        break;
                //    case "6":   //依年級產生
                //        where.And(StudentReceiveView.Field.StuGrade, qValue);
                //        break;
                //    default:
                //        errmsg = "批次處理序列缺少上條件類別參數或參數值不正確";
                //        return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                //}

                ////只取應繳金額大於 0 的資料
                //if (!allAmount)
                //{
                //    where.And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);
                //}

                //if (pdfType == GenPDFHelper.PDFType.Bill)   //繳費單
                //{
                //    //未繳費
                //    where.And(new Expression(StudentReceiveView.Field.ReceiveDate, null).Or(StudentReceiveView.Field.ReceiveDate, String.Empty));
                //}
                //else
                //{
                //    //已銷
                //    where
                //        .And(StudentReceiveView.Field.AccountDate, RelationEnum.NotEqual, null)
                //        .And(StudentReceiveView.Field.AccountDate, RelationEnum.NotEqual, String.Empty);
                //}

                //Result result = _Factory.SelectAll<StudentReceiveView>(where, orderbys, out studentReceives);
                #endregion

                Result result = this.GetStudentReceiveViews(receiveType, yearId, termId, depId, receiveId
                    , qType, qValue, allAmount, pdfType, module, isEngUI, out studentReceives);
                #endregion

                if (!result.IsSuccess)
                {
                    errmsg = "讀取繳費單資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (studentReceives == null || studentReceives.Length == 0)
                {
                    errmsg = "查無繳費單資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            totalCount = studentReceives == null ? 0 : studentReceives.Length;
            #endregion

            #region SchoolRTypeEntity
            SchoolRTypeEntity school = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                if (!result.IsSuccess)
                {
                    errmsg = String.Format("讀取商家代號 {0} 的資料失敗，{1}", receiveType, result.Message);
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (school == null)
                {
                    errmsg = String.Format("查無商家代號 {0} 的資料", receiveType);
                    return new Result(false, errmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 取消 SchoolRidView2 改用 SchoolRidEntity 就夠
            SchoolRidEntity schoolRid = null;
            {
                Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
                    .And(SchoolRidEntity.Field.YearId, yearId)
                    .And(SchoolRidEntity.Field.TermId, termId)
                    .And(SchoolRidEntity.Field.DepId, depId)
                    .And(SchoolRidEntity.Field.ReceiveId, receiveId);
                Result result = _Factory.SelectFirst<SchoolRidEntity>(where, null, out schoolRid);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取代收費用別資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (schoolRid == null)
                {
                    errmsg = "查無代收費用別資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region ReceiveChannelEntity
            ReceiveChannelEntity[] receiveChannels = null;
            {
                Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectAll<ReceiveChannelEntity>(where, null, out receiveChannels);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取代收管道手續費資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (receiveChannels == null || receiveChannels.Length == 0)
                {
                    errmsg = "查無代收管道手續費資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region StudentMasterEntity
            StudentMasterEntity[] students = null;
            {
                Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, receiveType)
                    .And(StudentMasterEntity.Field.DepId, depId);
                Result result = _Factory.SelectAll<StudentMasterEntity>(where, null, out students);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取學生資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (students == null || students.Length == 0)
                {
                    errmsg = "查無學生資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region BankEntity
            BankEntity bank = null;
            {
                Expression where = null;
                string bankId = school.BankId == null ? String.Empty : school.BankId.Trim();
                if (bankId.Length == 7)
                {
                    where = new Expression(BankEntity.Field.FullCode, bankId);
                }
                else if (bankId.Length == 6)
                {
                    where = new Expression(BankEntity.Field.BankNo, bankId);
                }
                else if (bankId.Length == 3)
                {
                    where = new Expression(BankEntity.Field.BankNo, DataFormat.MyBankID + bankId);
                }
                else
                {
                    where = new Expression(BankEntity.Field.BankNo, RelationEnum.Like, DataFormat.MyBankID + bankId + "%");
                }

                Result result = _Factory.SelectFirst<BankEntity>(where, null, out bank);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取主辦行資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                //[TODO] 暫時不檢查
                //if (bank == null)
                //{
                //    errmsg = "查無主辦行資料";
                //    return new Result(false, errmsg, result.Code, result.Exception);
                //}
            }
            #endregion

            #region ReceiveSumEntity
            ReceiveSumEntity[] receiveSums = null;
            {
                Expression where = new Expression(ReceiveSumEntity.Field.ReceiveType, schoolRid.ReceiveType)
                    .And(ReceiveSumEntity.Field.YearId, schoolRid.YearId)
                    .And(ReceiveSumEntity.Field.TermId, schoolRid.TermId)
                    .And(ReceiveSumEntity.Field.DepId, schoolRid.DepId)
                    .And(ReceiveSumEntity.Field.ReceiveId, schoolRid.ReceiveId)
                    .And(ReceiveSumEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(ReceiveSumEntity.Field.SumId, OrderByEnum.Asc);

                Result result = _Factory.SelectAll<ReceiveSumEntity>(where, orderbys, out receiveSums);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取合計項目設定資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region templetContent
            byte[] templetContent = null;
            {
                #region [MDY:202203XX] 2022擴充案 中英文模板
                #region [OLD]
                //string sql = String.Format(@"SELECT [{0}] FROM [{1}] WHERE  [{2}] = @BillFormType AND [{3}] = @BillFormId"
                //    , BillFormEntity.Field.BillFormImage
                //    , BillFormEntity.TABLE_NAME
                //    , BillFormEntity.Field.BillFormType
                //    , BillFormEntity.Field.BillFormId);      //10
                //KeyValueList parameters = new KeyValueList(2);
                //if (pdfType == GenPDFHelper.PDFType.Bill)
                //{
                //    parameters.Add("@BillFormType", BillFormTypeCodeTexts.BILLING);
                //    parameters.Add("@BillFormId", schoolRid.BillformId);
                //}
                //else
                //{
                //    parameters.Add("@BillFormType", BillFormTypeCodeTexts.RECEIPT);
                //    parameters.Add("@BillFormId", schoolRid.InvoiceformId);
                //}
                #endregion

                bool useEngDataUI = isEngUI && school.IsEngEnabled();

                string sql = $@"
SELECT [{BillFormEntity.Field.BillFormImage}]
  FROM [{BillFormEntity.TABLE_NAME}]
 WHERE [{BillFormEntity.Field.BillFormType}] = @BillFormType AND[{BillFormEntity.Field.BillFormId}] = @BillFormId";

                KeyValueList parameters = new KeyValueList(2);
                if (pdfType == GenPDFHelper.PDFType.Bill)
                {
                    parameters.Add("@BillFormType", BillFormTypeCodeTexts.BILLING);
                    if (useEngDataUI)
                    {
                        parameters.Add("@BillFormId", schoolRid.BillFormEId);
                    }
                    else
                    {
                        parameters.Add("@BillFormId", schoolRid.BillformId);
                    }
                }
                else
                {
                    parameters.Add("@BillFormType", BillFormTypeCodeTexts.RECEIPT);
                    if (useEngDataUI)
                    {
                        parameters.Add("@BillFormId", schoolRid.InvoiceFormEId);
                    }
                    else
                    {
                        parameters.Add("@BillFormId", schoolRid.InvoiceformId);
                    }
                }
                #endregion

                object value = null;
                Result result = _Factory.ExecuteScalar(sql, parameters, out value);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取繳費單模板資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                templetContent = value as byte[];
                if (templetContent == null || templetContent.Length == 0)
                {
                    errmsg = "查無繳費單模板資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region [MDY:20171127] 取財金 QRCode 支付參數設定 (20170831_01)
            FiscQRCodeConfig qrcodeConfig = null;
            {
                ConfigEntity configData = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, FiscQRCodeConfig.ConfigKey);
                Result result = _Factory.SelectFirst<ConfigEntity>(where, null, out configData);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取財金 QRCode 支付參數失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                //無資料或參數設定錯誤 (無法解析) 都不要中斷處理，避免使用無 QRCode 的模板也無法產生 PDF
                if (configData != null)
                {
                    qrcodeConfig = FiscQRCodeConfig.Parse(configData.ConfigValue);
                }
            }
            #endregion

            DateTime startTime;
            DateTime endTime;
            {
                GenPDFHelper helper = new GenPDFHelper();

                #region [MDY:20171127] 財金 QRCode 支付 (20170831_01)
                #region [Old]
                //Result result = helper.GenPDFFiles(school, schoolRid, receiveChannels, studentReceives, students, bank, receiveSums, false
                //    , pdfType, templetContent, outPath, job.Jowner, out startTime, out endTime, out pdfFile);
                #endregion

                #region [MDY:202203XX] 2022擴充案 是否英文介面
                Result result = helper.GenPDFFiles(school, schoolRid, receiveChannels, studentReceives, students
                    , bank, receiveSums, qrcodeConfig, false, isEngUI
                    , pdfType, templetContent, outPath, job.Jowner, out startTime, out endTime, out pdfFile);
                #endregion
                #endregion
                return result;
            }

        }

        /// <summary>
        /// 非同步產生 年度繳費一覽表 的委派
        /// </summary>
        /// <param name="jobID">指定工作代碼。</param>
        /// <param name="processOwner">指定處理者。</param>
        /// <returns>傳回處理結果。</returns>
        public delegate Result AsyncProcessPDFxJob(string jobCubeNo, string stamp, string outPath);

        public IAsyncResult ProcessPDFxJobByAsync(string jobCubeNo, string stamp, string outPath, AsyncCallback callback)
        {
            AsyncProcessPDFxJob myAsync = new AsyncProcessPDFxJob(GenPDFxJobByAsync);
            //Result result = myAsync.Invoke(jobCubeNo, outPath);
            IAsyncResult asyncResult = myAsync.BeginInvoke(jobCubeNo, stamp, outPath, callback, null);

            //Result result = this.GenBillListReportByJobID(jobCubeNo, outPath);
            return asyncResult;
        }

        public Result GenPDFxJobByAsync(string jobCubeNo, string stamp, string outPath)
        {
            string errmsg = null;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                errmsg = "資料存取物件未準備好";
                return new Result(false, errmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            jobCubeNo = jobCubeNo == null ? String.Empty : jobCubeNo.Trim();
            if (String.IsNullOrEmpty(jobCubeNo))
            {
                return new Result(false, "缺少或無效的工作代碼參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            outPath = outPath == null ? String.Empty : outPath.Trim();
            if (String.IsNullOrEmpty(outPath))
            {
                return new Result(false, "缺少或無效的產出檔路徑者參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得工作資料並註記為處理中
            JobcubeEntity job = null;
            //string stamp = Guid.NewGuid().ToString("N");
            {
                Result result = this.GetWaitJobToProcessByJobCubeNo(jobCubeNo, stamp, out job);
                if (!result.IsSuccess)
                {
                    return result;
                }
            }
            #endregion

            #region 檢查 jobcube 參數
            GenPDFHelper.PDFType pdfType = GenPDFHelper.PDFType.None;
            switch (job.Jtypeid)
            {
                case JobCubeTypeCodeTexts.PDFB:
                    pdfType = GenPDFHelper.PDFType.Bill;
                    break;
                case JobCubeTypeCodeTexts.PDFR:
                    pdfType = GenPDFHelper.PDFType.Receipt;
                    break;
                default:
                    errmsg = String.Format("批次處理序列 {0} 的類別不符合", job.Jno);
                    Result subResult = this.UpdateProcessJobToFinsh(job.Jno, stamp, JobCubeResultCodeTexts.FAILURE, errmsg, errmsg);
                    return new Result(false, errmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string yearId = job.Jyear;
            string termId = job.Jterm;
            string depId = job.Jdep;
            string receiveId = job.Jrecid;
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || String.IsNullOrEmpty(receiveId))
            {
                errmsg = String.Format("批次處理序列 {0} 缺少業務別碼、學年代碼、學期代碼或代收費用別代碼的資料參數或資料不正確", job.Jno);
                Result subResult = this.UpdateProcessJobToFinsh(job.Jno, stamp, JobCubeResultCodeTexts.FAILURE, errmsg, errmsg);
                return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region 拆解 JobcubeEntity 參數
            #region [MDY:202203XX] 2022擴充案 是否英文介面
            bool isParamOK = false;
            string qType = null;
            string qValue = null;
            bool allAmount = false;
            bool isEngUI = false;
            {
                string pReceiveType = null;
                string pYearId = null;
                string pTermId = null;
                string pDepId = null;
                string pReceiveId = null;
                isParamOK = JobcubeEntity.ParsePDBxParameter(job.Jparam, out pReceiveType, out pYearId, out pTermId, out pDepId, out pReceiveId
                                , out qType, out qValue, out allAmount, out isEngUI);
            }
            #endregion

            if (!isParamOK)
            {
                errmsg = String.Format("批次處理序列 {0} 的參數不正確", job.Jno);
                Result subResult = this.UpdateProcessJobToFinsh(job.Jno, stamp, JobCubeResultCodeTexts.FAILURE, errmsg, errmsg);
                return new Result(false, errmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            try
            {
                int totalCount = 0;
                string[] outPDFFiles = null;
                GenPDFHelper helper = new GenPDFHelper();

                #region [MDY:202203XX] 2022擴充案 是否英文介面
                Result genResult = helper.GenPDFFiles(_Factory, receiveType, yearId, termId, depId, receiveId
                    , qType, qValue, allAmount, false, isEngUI, pdfType, outPath, job.Jowner
                    , out totalCount, out outPDFFiles);
                #endregion

                if (genResult.IsSuccess)
                {
                    BankpmEntity[] datas = new BankpmEntity[outPDFFiles.Length];
                    for (int idx = 0; idx < outPDFFiles.Length; idx++)
                    {
                        BankpmEntity data = new BankpmEntity();
                        data.Filename = String.Format("{0}_{1}.PDF", stamp, idx + 1);
                        data.Filedetail = String.Format("{0}_{1}_{2}_{3}_{4}_繳費單_{5}.PDF", receiveType, yearId, termId, depId, receiveId, idx + 1);
                        data.Status = DataStatusCodeTexts.NORMAL;
                        data.Cdate = DateTime.Today.ToString("yyyy/MM/dd");
                        data.Udate = data.Cdate;
                        data.ReceiveType = receiveType;
                        data.Tempfile = File.ReadAllBytes(outPDFFiles[idx]);
                        datas[idx] = data;
                    }
                    int count = 0;
                    int[] failIndexs = null;
                    genResult = _Factory.Insert(datas, true, out count, out failIndexs);
                }
                if (genResult.IsSuccess)
                {
                    string log = String.Format("共 {0} 資料，產生 {1} 個檔案", totalCount, outPDFFiles.Length);
                    Result subResult = this.UpdateProcessJobToFinsh(job.Jno, stamp, JobCubeResultCodeTexts.SUCCESS, JobCubeResultCodeTexts.SUCCESS_TEXT, log);
                }
                else
                {
                    errmsg = "產生 PDF 發生錯誤：" + genResult.Message;
                    Result subResult = this.UpdateProcessJobToFinsh(job.Jno, stamp, JobCubeResultCodeTexts.FAILURE, errmsg, errmsg);
                }
                return genResult;
            }
            catch (Exception ex)
            {
                errmsg = "產生 PDF 發生錯誤：" + ex.Message;
                Result subResult = this.UpdateProcessJobToFinsh(job.Jno, stamp, JobCubeResultCodeTexts.FAILURE, errmsg, errmsg);
                return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, ex);
            }
        }


        #region[Old]
        ///// <summary>
        ///// 處理 SBM (寄送繳費通知信) 批次處理序列作業
        ///// </summary>
        ///// <param name="job">傳回 jobcube</param>
        ///// <param name="totalCount">傳回發送筆數</param>
        ///// <returns>傳回處理結果</returns>
        //public Result ProcessSBMJob(out JobcubeEntity job, out Int32 totalCount)
        //{
        //    job = null;
        //    totalCount = 0;
        //    string errmsg = null;

        //    #region 檢查 IsReady
        //    if (!this.IsReady())
        //    {
        //        errmsg = "資料存取物件未準備好";
        //        return new Result(false, errmsg, ErrorCode.S_INVALID_FACTORY, null);
        //    }
        //    #endregion

        //    #region New JobcubeEntity
        //    DateTime now = DateTime.Now;
        //    job = new JobcubeEntity();
        //    job.Jstd = DateTime.Now;
        //    job.Jetd = null;
        //    job.Jdll = String.Empty;
        //    job.Jclass = String.Empty;
        //    job.Jmethod = String.Empty;
        //    job.Jparam = String.Empty;
        //    job.Jpid = String.Empty;
        //    job.Jowner = "SYSTEM";
        //    job.Jrid = String.Empty;
        //    job.Jyear = String.Empty;
        //    job.Jterm = String.Empty;
        //    job.Jdep = String.Empty;
        //    job.Jrecid = String.Empty;
        //    job.Jprity = 0;
        //    job.Jtypeid = JobCubeTypeCodeTexts.SBM;
        //    job.Jstatusid = JobCubeStatusCodeTexts.PROCESS;
        //    job.Jlog = JobCubeStatusCodeTexts.PROCESS_TEXT;
        //    job.Jresultid = JobCubeResultCodeTexts.PROCESS;
        //    job.Memo = JobCubeResultCodeTexts.PROCESS_TEXT;
        //    job.CDate = now;
        //    job.MDate = null;
        //    job.SeriorNo = String.Empty;
        //    job.Chancel = String.Empty;
        //    #endregion

        //    #region 先將 SendDate 為 null 設定 now 作為戳記
        //    {
        //        string sql = String.Format("UPDATE [{0}] SET [{1}] = @SendDate WHERE [{1}] IS NULL", EmailDataEntity.TABLE_NAME, EmailDataEntity.Field.SendDate);
        //        KeyValue[] fieldValues = new KeyValue[] { new KeyValue("@SendDate", now) };
        //        int count = 0;
        //        Result result = _Factory.ExecuteNonQuery(sql, fieldValues, out count);
        //        if (!result.IsSuccess)
        //        {
        //            job.Jetd = DateTime.Now;
        //            job.Jstatusid = JobCubeStatusCodeTexts.FINISH;
        //            job.Jlog = String.Format("註記繳費通知信資料失敗，錯誤訊息：{0}", result.Message);
        //            job.Jresultid = JobCubeResultCodeTexts.FAILURE;
        //            job.Memo = job.Jlog;
        //            return new Result(false, job.Jlog, result.Code, result.Exception);
        //        }
        //    }
        //    #endregion

        //    BSNSHelper helper = new BSNSHelper();
        //    if (!helper.IsReady())
        //    {
        //        job.Jetd = DateTime.Now;
        //        job.Jstatusid = JobCubeStatusCodeTexts.FINISH;
        //        job.Jlog = "發送通知信的系統參數未設定好";
        //        job.Jresultid = JobCubeResultCodeTexts.FAILURE;
        //        job.Memo = job.Jlog;
        //        return new Result(false, job.Jlog, CoreStatusCode.UNKNOWN_ERROR, null);
        //    }

        //    #region 取得 SendDate 為 now 的 EmailData
        //    EmailDataEntity[] emailDatas = null;
        //    {
        //        Expression where = new Expression(EmailDataEntity.Field.SendDate, now);
        //        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
        //        orderbys.Add(EmailDataEntity.Field.DueDate, OrderByEnum.Asc);
        //        orderbys.Add(EmailDataEntity.Field.EmailDate, OrderByEnum.Asc);
        //        orderbys.Add(EmailDataEntity.Field.ReceiveType, OrderByEnum.Asc);
        //        orderbys.Add(EmailDataEntity.Field.YearId, OrderByEnum.Asc);
        //        orderbys.Add(EmailDataEntity.Field.TermId, OrderByEnum.Asc);
        //        orderbys.Add(EmailDataEntity.Field.DepId, OrderByEnum.Asc);
        //        orderbys.Add(EmailDataEntity.Field.ReceiveId, OrderByEnum.Asc);
        //        orderbys.Add(EmailDataEntity.Field.StuId, OrderByEnum.Asc);
        //        Result result = _Factory.SelectAll<EmailDataEntity>(where, orderbys, out emailDatas);
        //        if (result.IsSuccess)
        //        {
        //            if (emailDatas == null || emailDatas.Length == 0)
        //            {
        //                job.Jetd = DateTime.Now;
        //                job.Jstatusid = JobCubeStatusCodeTexts.FINISH;
        //                job.Jlog = "無繳費通知信需要寄送";
        //                job.Jresultid = JobCubeResultCodeTexts.SUCCESS;
        //                job.Memo = JobCubeResultCodeTexts.SUCCESS_TEXT;
        //                return new Result(true, "無繳費通知信需要寄送", ErrorCode.D_DATA_NOT_FOUND, null);
        //            }
        //        }
        //        else
        //        {
        //            job.Jetd = DateTime.Now;
        //            job.Jstatusid = JobCubeStatusCodeTexts.FINISH;
        //            job.Jlog = String.Format("讀取繳費通知信資料失敗，錯誤訊息：{0}", result.Message);
        //            job.Jresultid = JobCubeResultCodeTexts.FAILURE;
        //            job.Memo = job.Jlog;
        //            return new Result(false, job.Jlog, result.Code, result.Exception);
        //        }
        //    }
        //    totalCount = emailDatas.Length;
        //    #endregion

        //    #region 發送通知信
        //    {
        //        StringBuilder logs = new StringBuilder();
        //        foreach (EmailDataEntity emailData in emailDatas)
        //        {
        //            //信件主旨：土地銀行代收學雜費服務網繳費通知+原有的信函編號
        //            string subject = "土地銀行代收學雜費服務網繳費通知 ";

        //            #region 自組 Html
        //            //信件內容 (可 html)
        //            string content = this.GenSBMContent(emailData);

        //            errmsg = helper.SendMail(subject, emailData.StuEmail, emailData.StuName, content, null);
        //            #endregion

        //            #region [Old] 改用套版變數
        //            //CodeText[] contentPARAMs = this.GenSBMContentPARAMs(emailData);

        //            //errmsg = helper.SendMail(subject, emailData.StuEmail, emailData.StuName, null, contentPARAMs);
        //            #endregion

        //            if (!String.IsNullOrEmpty(errmsg))
        //            {
        //                logs.AppendFormat("學號 {0} 的通知信發送失敗，錯誤訊息：{1}", emailData.StuId, errmsg).AppendLine();
        //            }
        //        }

        //        job.Jetd = DateTime.Now;
        //        job.Jstatusid = JobCubeStatusCodeTexts.FINISH;
        //        job.Jlog = String.Format("共 {0} 筆資料，日誌：{1}", emailDatas.Length, logs.ToString());
        //        job.Jresultid = JobCubeResultCodeTexts.SUCCESS;
        //        job.Memo = job.Jlog;
        //    }
        //    #endregion

        //    //因為目前無法判斷 SendMailNow 回傳 code 怎麽判斷是否成功，所以一律視為成功
        //    return new Result(true);
        //}

//        private static readonly string _SBMContentPattern = @"<html>
//<head>
//	<title>LandBank Mail Notification</title>
//	<meta content-type=""text/html; charset=utf-8"" />
//	<style type=""text/css"">
//	body {{
//		margin-top: 0px;
//		background-color: #b3b3b3;
//		margin-bottom: 0px;
//	}}
//	</style>
//</head>
//<body>
//	<div align=""center"">
//		<div style=""width:670px;"">
//		<div style=""width:100%""><img src=""http://www.landbank.com.tw/images/CPImages/image001.gif"" style=""width:670px;"" alt=""header""></div>
//		<div style=""background-color:#FFFFFF; width:670px; min-height:200px; text-align:left;"">
//			<div style=""margin-left:40px;"">
//			親愛的 {0} 同學：<br/><br/>
//			這是由土地銀行代收學雜費服務網系統自動發送的 {1} 學年的繳費通知信<br/><br/>
//			繳費單虛擬帳號為 {2}<br/><br/>
//			詳細帳單資訊請至土地銀行入口網站/代收學雜費服務網查詢
//			</div>
//		</div>
//		<div style=""width:100%""><img src=""http://www.landbank.com.tw/images/CPImages/image002.gif"" style=""width:670px;"" alt=""footer""></div>
//		</div>
//	</div>
//</body>
        //</html>";
        #endregion


        ///// <summary>
        ///// 產生 SBM (寄送繳費通知信) 的內容套版變數集合
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //private CodeText[] GenSBMContentPARAMs(EmailDataEntity data)
        //{
        //    return new CodeText[] {
        //        new CodeText("PARAM1", data.StuName),   //學生姓名
        //        new CodeText("PARAM2", data.YearId),    //學年
        //        new CodeText("PARAM3", data.CancelNo),   //虛擬帳號
        //        new CodeText("PARAM1", data.StuName),
        //    };
        //}
        #endregion

        #region [MDY:20160312] 發送繳費通知信 (SBM) 作業相關
        /// <summary>
        /// 處理 SBM (寄送繳費通知信) 批次處理序列作業
        /// </summary>
        /// <param name="job">傳回 jobcube</param>
        /// <param name="totalCount">傳回發送筆數</param>
        /// <returns>傳回處理結果</returns>
        public Result ProcessSBMJob(int retryTimes, int retrySleep, out string logMsg, out string summaryMsg)
        {
            logMsg = null;
            summaryMsg = null;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                return new Result(false, "資料存取物件未準備好", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查發送通知信的系統參數
            BSNSHelper helper = new BSNSHelper();
            if (!helper.IsReady())
            {
                return new Result(false, "發送通知信的系統參數未設定好", CoreStatusCode.UNKNOWN_ERROR, null);
            }
            #endregion

            DateTime now = DateTime.Now;
            int hasRetryTimes = 0;    //已經重次數

            #region 先將 SendDate 為 null 設定 now 作為戳記
            {
                string sql = String.Format("UPDATE [{0}] SET [{1}] = @SendDate WHERE [{1}] IS NULL", EmailDataEntity.TABLE_NAME, EmailDataEntity.Field.SendDate);
                KeyValue[] fieldValues = new KeyValue[] { new KeyValue("@SendDate", now) };

                int count = 0;
                Result result = _Factory.ExecuteNonQuery(sql, fieldValues, out count);
                if (!result.IsSuccess)
                {
                    hasRetryTimes++;
                    for (; hasRetryTimes <= retryTimes; hasRetryTimes++)
                    {
                        if (retrySleep > 0)
                        {
                            System.Threading.Thread.Sleep(1000 * 60 * retrySleep);
                        }
                        result = _Factory.ExecuteNonQuery(sql, fieldValues, out count);
                        if (result.IsSuccess)
                        {
                            break;
                        }
                    }
                }

                if (!result.IsSuccess)
                {
                    string errmsg = String.Format("註記繳費通知信資料失敗，錯誤訊息：{0}", result.Message);
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (count == 0)
                {
                    logMsg = "無繳費通知信需要寄送";
                    summaryMsg = "無繳費通知信需要寄送";
                    return new Result(true);
                }
            }
            #endregion

            #region 取得註記的資料
            EmailDataEntity[] emailDatas = null;
            {
                Expression where = new Expression(EmailDataEntity.Field.SendDate, now);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(EmailDataEntity.Field.DueDate, OrderByEnum.Asc);
                orderbys.Add(EmailDataEntity.Field.EmailDate, OrderByEnum.Asc);
                orderbys.Add(EmailDataEntity.Field.ReceiveType, OrderByEnum.Asc);
                orderbys.Add(EmailDataEntity.Field.YearId, OrderByEnum.Asc);
                orderbys.Add(EmailDataEntity.Field.TermId, OrderByEnum.Asc);
                orderbys.Add(EmailDataEntity.Field.DepId, OrderByEnum.Asc);
                orderbys.Add(EmailDataEntity.Field.ReceiveId, OrderByEnum.Asc);
                orderbys.Add(EmailDataEntity.Field.StuId, OrderByEnum.Asc);

                Result result = _Factory.SelectAll<EmailDataEntity>(where, orderbys, out emailDatas);
                if (!result.IsSuccess)
                {
                    hasRetryTimes++;
                    for (; hasRetryTimes <= retryTimes; hasRetryTimes++)
                    {
                        if (retrySleep > 0)
                        {
                            System.Threading.Thread.Sleep(1000 * 60 * retrySleep);
                        }
                        result = _Factory.SelectAll<EmailDataEntity>(where, orderbys, out emailDatas);
                        if (result.IsSuccess)
                        {
                            break;
                        }
                    }
                }

                if (!result.IsSuccess)
                {
                    string errmsg = String.Format("讀取註記的繳費通知信資料失敗，錯誤訊息：{0}", result.Message);
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (emailDatas == null || emailDatas.Length == 0)
                {
                    logMsg = "無繳費通知信需要寄送";
                    summaryMsg = "無繳費通知信需要寄送";
                    return new Result(true);
                }
            }
            #endregion

            #region 發送通知信
            {
                StringBuilder log = new StringBuilder();
                int totalCount = emailDatas.Length;
                int okCount = 0;
                int failCount = 0;
                string exMsg = null;
                string subject = "土地銀行代收學雜費服務網繳費通知 ";
                try
                {
                    foreach (EmailDataEntity emailData in emailDatas)
                    {
                        //信件主旨：土地銀行代收學雜費服務網繳費通知+原有的信函編號 (目前沒有編號，暫時不處理)

                        //信件內容 (可 html)
                        string content = this.GenSBMContent(emailData);

                        string errmsg = null;
                        try
                        {
                            errmsg = helper.SendMail(subject, emailData.StuEmail, emailData.StuName, content, null);
                        }
                        catch (Exception ex)
                        {
                            errmsg = ex.Message;
                        }

                        if (String.IsNullOrEmpty(errmsg))
                        {
                            okCount++;
                        }
                        else
                        {
                            failCount++;
                            log.AppendFormat("虛擬帳號 {0} 通知信發送失敗，錯誤訊息：{1}", emailData.CancelNo, errmsg).AppendLine();
                        }
                    }

                }
                catch (Exception ex)
                {
                    exMsg = String.Format("逐筆發送通知信發生例外，錯誤訊息：{0}", ex.Message);
                }
                finally
                {
                    summaryMsg = String.Format("共 {0} 筆資料，成功 {1} 筆，失敗 {2} 筆。{3}", totalCount, okCount, failCount, exMsg);
                    log.Insert(0, summaryMsg + "\r\n");
                    logMsg = log.ToString();
                }


                //因為目前無法判斷 SendMailNow 回傳 code 怎麽判斷是否成功，所以一律視為成功
                return new Result(true);
            }
            #endregion

        }

        #region Const
        /// <summary>
        /// 寄送繳費通知信內容 Pattern
        /// </summary>
        private static readonly string _SBMContentPattern = @"<html>
<head>
	<title>LandBank Mail Notification</title>
	<meta content-type=""text/html; charset=utf-8"" />
	<style type=""text/css"">
	body {{
		margin-top: 0px;
		background-color: #b3b3b3;
		margin-bottom: 0px;
	}}
</style>
</head>
<body>
<div align=""center"">
	<table bgcolor=""#ffffff"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""670"">
	<tr>
		<td><img src=""http://www.landbank.com.tw/images/CPImages/image001.gif"" style=""width:670px;"" alt=""header""></td>
	</tr>
	<tr>
		<td>
		<div style=""margin-left:40px; min-height:200px"">
		親愛的 {0} 同學：<br/><br/>
		這是由土地銀行代收學雜費服務網系統自動發送的 {1} 學年的繳費通知信<br/><br/>
		繳費單虛擬帳號為 {2}<br/><br/>
		詳細帳單資訊請至土地銀行入口網站/代收學雜費服務網查詢
		</div>
		</td>
	</tr>
	<tr>
		<td><img src=""http://www.landbank.com.tw/images/CPImages/image002.gif"" style=""width:670px;"" alt=""footer""></td>
	</tr>
	</table>
</div>
</body>
</html>";
        #endregion

        /// <summary>
        /// 產生 SBM (寄送繳費通知信) 的內容
        /// </summary>
        /// <param name="emailData">指定通知信資料</param>
        /// <returns>傳回通知信內容</returns>
        private string GenSBMContent(EmailDataEntity data)
        {
            #region Sample
            //            return String.Format(@"親愛的 {0} 同學：<br/><br/>
            //這是由土地銀行代收學雜費服務網系統自動發送的 {1} 學年的繳費通知信<br/><br/>
            //繳費單虛擬帳號為 {2}<br/><br/>
            //詳細帳單資訊請至土地銀行入口網站/代收學雜費服務網查詢",
            //                data.StuName, data.YearId, data.CancelNo);
            #endregion

            return String.Format(_SBMContentPattern, data.StuName, data.YearId, data.CancelNo);
        }
        #endregion

    }

}
