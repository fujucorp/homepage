using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fuju;
using Fuju.Configuration;
using Fuju.DB;
using Fuju.DB.Configuration;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 資料庫日誌記錄器類別
    /// </summary>
    public class DBLogger : IDBLogger, IDisposable
    {
        public delegate Result AsyncSaveLog(EntityFactory factory, LogTableEntity[] logDatas);

        /// <summary>
        /// 記錄日誌模式(位元旗標)列舉值 (可做位元運算)
        /// </summary>
        [Flags]
        public enum ModeEnum
        {
            //未指定 (停止記錄日誌) : 0
            None = 0,

            /// <summary>
            /// 記錄一般操作(增、修、刪、執行)的日誌 : 1
            /// </summary>
            Normal = 1,

            /// <summary>
            /// 記錄查詢操作的日誌 : 2
            /// </summary>
            Select = 2,

            /// <summary>
            /// 記錄資料庫存取事件的日誌 : 4
            /// </summary>
            Event = 4,

            /// <summary>
            /// 記錄所有操作 (一般 + 查詢+ 資料庫存取事件) 的日誌 : 7
            /// </summary>
            All = Normal | Select | Event
        }

        #region Member & Property
        /// <summary>
        /// 儲存日誌 Buffer 的成員變數
        /// </summary>
        private List<LogTableEntity> _Logs = new List<LogTableEntity>();

        /// <summary>
        /// 儲存 Entity 資料存取物件的成員變數
        /// </summary>
        private EntityFactory _Factory = null;

        private string _DBConfigName = null;
        /// <summary>
        /// 資料庫設定組態名稱
        /// </summary>
        public string DBConfigName
        {
            get
            {
                return _DBConfigName;
            }
            protected set
            {
                _DBConfigName = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveType = null;
        /// <summary>
        /// 記錄日誌的代收類別代碼
        /// </summary>
        public string ReceiveType
        {
            get
            {
                return _ReceiveType;
            }
            protected set
            {
                _ReceiveType = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Role = null;
        /// <summary>
        /// 記錄日誌的請求者的身分別
        /// </summary>
        public string Role
        {
            get
            {
                return _Role;
            }
            protected set
            {
                _Role = value == null ? String.Empty : value.Trim();
            }
        }

        private string _FunctionId = null;
        /// <summary>
        /// 記錄日誌的功能代碼
        /// </summary>
        public string FunctionId
        {
            get
            {
                return _FunctionId;
            }
            protected set
            {
                _FunctionId = value == null ? null : value.Trim();
            }
        }

        private string _UserId = null;
        /// <summary>
        /// 記錄日誌的使用者代碼
        /// </summary>
        public string UserId
        {
            get
            {
                return _UserId;
            }
            protected set
            {
                _UserId = value == null ? null : value.Trim();
            }
        }

        private ModeEnum _Mode = ModeEnum.Normal;
        /// <summary>
        /// 記錄日誌的模式，預設 ModeEnum.Normal
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ModeEnum Mode
        {
            get
            {
                return _Mode;
            }
            protected set
            {
                _Mode = value;
            }
        }

        /// <summary>
        /// 取得日誌模式是否為忽略資料庫存取事件的日誌
        /// </summary>
        public bool IsSkipEventLog
        {
            get
            {
                return (this.Mode & ModeEnum.Event) != ModeEnum.Event;
            }
        }

        /// <summary>
        /// 取得日誌模式是否為忽略查詢操作的日誌
        /// </summary>
        public bool IsSkipSelectLog
        {
            get
            {
                return (this.Mode & ModeEnum.Select) != ModeEnum.Select;
            }
        }

        /// <summary>
        /// 取得日誌模式是否為忽略一般操作(增、修、刪、執行)的日誌
        /// </summary>
        public bool IsSkipNormalLog
        {
            get
            {
                return (this.Mode & ModeEnum.Normal) != ModeEnum.Normal;
            }
        }

        /// <summary>
        /// 取得日誌模式是否為忽略所有日誌 (停止紀錄)
        /// </summary>
        public bool IsSkipAllLog
        {
            get
            {
                return (this.Mode & ModeEnum.All) == ModeEnum.None;
            }
        }

        /// <summary>
        /// 初始化的錯誤訊息
        /// </summary>
        public string IntitalError
        {
            get;
            protected set;
        }
        #endregion

        #region Constructor
        #region [MDY:20220910] Checkmarx - Improper Resource Shutdown or Release 誤判調整
        /// <summary>
        /// 建構資料庫日誌記錄器類別
        /// </summary>
        /// <param name="role">指定記錄日誌的請求者的身分別</param>
        /// <param name="receiveType">指定記錄日誌的代收類別代碼</param>
        /// <param name="functionId">指定記錄日誌的功能代碼</param>
        /// <param name="userId">指定記錄日誌的使用者代碼</param>
        /// <param name="configGroupName">指定日誌組態群組名稱</param>
        /// <param name="mode">指定記錄日誌模式</param>
        public DBLogger(string role, string receiveType, string functionId, string userId
            , string configGroupName = null, ModeEnum? mode = null)
        {
            #region 取組態設定
            string dbConfigName = null;
            {
                if (String.IsNullOrWhiteSpace(configGroupName))
                {
                    configGroupName = "DBLogger";
                }
                else
                {
                    configGroupName = configGroupName.Trim();
                }
                KeyValueList<string> configs = ConfigManager.Current.GetSystemConfigs(configGroupName, StringComparison.CurrentCultureIgnoreCase);

                if (configs == null || configs.Count == 0)
                {
                    mode = DBLogger.ModeEnum.None;
                    this.IntitalError = "日誌組態未設定";
                }
                else
                {
                    #region DBConfigName
                    {
                        int idx = configs.GetKeyFirstIndex("DBConfigName", StringComparison.CurrentCultureIgnoreCase);
                        if (idx > -1)
                        {
                            dbConfigName = configs[idx].Value.Trim();
                        }
                    }
                    #endregion

                    #region Mode
                    if (!mode.HasValue)
                    {
                        int idx = configs.GetKeyFirstIndex("Mode", StringComparison.CurrentCultureIgnoreCase);
                        if (idx > -1)
                        {
                            int value = 0;
                            if (Int32.TryParse(configs[idx].Value, out value) && value >= 0)
                            {
                                value &= (int)DBLogger.ModeEnum.All;
                                mode = (DBLogger.ModeEnum)value;
                            }
                            else
                            {
                                mode = ModeEnum.Normal;
                            }
                        }
                        else
                        {
                            mode = ModeEnum.Normal;
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region 設定屬性值
            this.DBConfigName = dbConfigName;
            this.Role = role;
            this.ReceiveType = receiveType;
            this.FunctionId = functionId;
            this.UserId = userId;
            this.Mode = mode.HasValue ? mode.Value : DBLogger.ModeEnum.None;

            try
            {
                _Factory = new EntityFactory(this.DBConfigName, false);
                if (!_Factory.IsReady())
                {
                    this.Mode = ModeEnum.None;
                    this.IntitalError = "資料庫存取物件未準備好";
                }
            }
            catch (Exception ex)
            {
                this.Mode = ModeEnum.None;
                this.IntitalError = ex.Message;
            }
            #endregion
        }

        #region [OLD]
        ///// <summary>
        ///// 建構資料庫日誌記錄器類別
        ///// </summary>
        ///// <param name="dbConfigName">資料庫設定組態名稱。</param>
        ///// <param name="receiveType">記錄日誌的代收類別代碼。</param>
        ///// <param name="functionId">記錄日誌的功能代碼。</param>
        ///// <param name="userId">記錄日誌的使用者代碼。</param>
        //public DBLogger(string dbConfigName, string role, string receiveType, string functionId, string userId)
        //    : this(ModeEnum.Normal, dbConfigName, role, receiveType, functionId, userId)
        //{
        //}

        ///// <summary>
        ///// 建構資料庫日誌記錄器類別
        ///// </summary>
        ///// <param name="mode">記錄日誌的模式。</param>
        ///// <param name="dbConfigName">資料庫設定組態名稱。</param>
        ///// <param name="role">記錄日誌的請求者的身分別</param>
        ///// <param name="receiveType">記錄日誌的代收類別代碼</param>
        ///// <param name="functionId">記錄日誌的功能代碼。</param>
        ///// <param name="userId">記錄日誌的使用者代碼。</param>
        //internal DBLogger(ModeEnum mode, string dbConfigName, string role, string receiveType, string functionId, string userId)
        //{
        //    this.DBConfigName = dbConfigName;
        //    this.Role = role;
        //    this.ReceiveType = receiveType;
        //    this.FunctionId = functionId;
        //    this.UserId = userId;
        //    this.Mode = mode;

        //    try
        //    {
        //        _Factory = new EntityFactory(this.DBConfigName, false);
        //        if (!_Factory.IsReady())
        //        {
        //            this.IntitalError = "資料庫存取物件未準備好";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.IntitalError = ex.Message;
        //    }
        //}
        #endregion
        #endregion
        #endregion

        #region Destructor
        /// <summary>
        /// 解構資料庫日誌記錄器類別
        /// </summary>
        ~DBLogger()
        {
            Dispose(false);
        }
        #endregion

        #region Implement IDispose
        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        protected bool _Disposed = false;

        /// <summary>
        /// 執行與釋放 (Free)、釋放 (Release) 或重設 Unmanaged 資源相關聯之應用程式定義的工作
        /// </summary>
        public virtual void Dispose()
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
                    if (_Factory != null)
                    {
                        _Factory.Dispose();
                        _Factory = null;
                    }
                    if (_Logs != null && _Logs.Count > 0)
                    {
                        _Logs.Clear();
                        _Logs = null;
                    }
                }
                _Disposed = true;
            }
        }
        #endregion

        #region Implement IDBLogger's Method
        /// <summary>
        /// 附加資料庫處理事件記錄
        /// </summary>
        /// <param name="sender">引發事件的物件 (IDBFactory)。</param>
        /// <param name="e">指定引發事件的參數資料。</param>
        public void AppendEventLog(object sender, DBEventArgs e)
        {
            if (e == null || e == DBEventArgs.Empty)
            {
                return;
            }
            //[TODO] 可以考慮如果失敗或發生例外，一定要寫日誌
            if (this.IsSkipLog(e))
            {
                return;
            }
            if (!String.IsNullOrEmpty(this.IntitalError))
            {
                return;
            }

            LogTableEntity log = new LogTableEntity(DateTime.Now);
            log.Role = this.Role;
            log.ReceiveType = this.ReceiveType;
            log.FunctionId = this.FunctionId;
            log.UserId = this.UserId;
            log.LogType = this.GetLogType(e.Action);

            if (e is DBDataTableEventArgs)
            {
                log.Notation = this.GenLogNotation(e as DBDataTableEventArgs);
            }
            else if (e is DBExecuteNonQueryEventArgs)
            {
                log.Notation = this.GenLogNotation(e as DBExecuteNonQueryEventArgs);
            }
            else if (e is DBExecuteScalarEventArgs)
            {
                log.Notation = this.GenLogNotation(e as DBExecuteScalarEventArgs);
            }
            else if (e is DBRecordNoEventArgs)
            {
                log.Notation = this.GenLogNotation(e as DBRecordNoEventArgs);
            }
            else
            {
                log.Notation = this.GenLogNotation(e);
            }
            _Logs.Add(log);
        }

        /// <summary>
        /// 取得資料庫操作列舉型別對應的資料庫處理操作代碼
        /// </summary>
        /// <param name="dbAction">資料庫操作列舉型別。</param>
        /// <returns>傳回資料庫處理操作代碼。</returns>
        private string GetLogType(DBActionEnum dbAction)
        {
            switch (dbAction)
            {
                case  DBActionEnum.Insert:
                    return LogTypeCodeTexts.INSERT;
                case DBActionEnum.Update:
                    return LogTypeCodeTexts.UPDATE;
                case DBActionEnum.Delete:
                    return LogTypeCodeTexts.DELETE;
                case DBActionEnum.Execute:
                    return LogTypeCodeTexts.EXECUTE;
                case DBActionEnum.Select:
                    return LogTypeCodeTexts.SELECT;
                default:
                    return String.Empty;
            }
        }

        /// <summary>
        /// 產生資料庫處理結果
        /// </summary>
        /// <param name="eArg">GetDataTable 的事件參數</param>
        /// <returns></returns>
        private string GenLogNotation(DBDataTableEventArgs eArg)
        {
            if (eArg.Result != null && !eArg.Result.IsSuccess)
            {
                return eArg.Result.Message;
            }

            if (eArg.Value == null)
            {
                return "無法取得資料";
            }
            else
            {
                //TODO:如要記錄更詳細的資料可修改這裡
                return String.Format("取得資料 {0} 筆", eArg.Value.Rows.Count);
            }
        }

        /// <summary>
        /// 產生資料庫處理結果
        /// </summary>
        /// <param name="eArg">ExecuteNonQuery 的事件參數</param>
        /// <returns>傳回要記錄的資料庫處理結果 (Notation)</returns>
        private string GenLogNotation(DBExecuteNonQueryEventArgs eArg)
        {
            if (eArg.Result != null && !eArg.Result.IsSuccess)
            {
                return eArg.Result.Message;
            }

            string actionName = null;
            switch (eArg.Action)
            {
                case DBActionEnum.Insert:
                    actionName = "新增資料成功";
                    break;
                case DBActionEnum.Update:
                    actionName = "修改資料成功";
                    break;
                case DBActionEnum.Delete:
                    actionName = "刪除資料成功";
                    break;
                default:
                    actionName = "執行命令成功";
                    break;
            }

            //TODO:如要記錄更詳細的資料可修改這裡
            return String.Format("{0}，共 {1} 筆", actionName, eArg.Value);
        }

        /// <summary>
        /// 產生資料庫處理結果
        /// </summary>
        /// <param name="eArg">ExecuteScalar 的事件參數</param>
        /// <returns>傳回要記錄的資料庫處理結果 (Notation)</returns>
        private string GenLogNotation(DBExecuteScalarEventArgs eArg)
        {
            if (eArg.Result != null && !eArg.Result.IsSuccess)
            {
                return eArg.Result.Message;
            }

            if (eArg.Value == null)
            {
                return "無法取得資料";
            }
            else
            {
                string actionName = null;
                if (eArg.Action == DBActionEnum.Select)
                {
                    actionName = "查詢資料";
                }
                else
                {
                    actionName = "執行命令";
                }

                //TODO:如要記錄更詳細的資料可修改這裡
                return String.Format("{0}成功，取得欄位值為 {1} ", actionName, eArg.Value);
            }
        }

        /// <summary>
        /// 產生資料庫處理結果
        /// </summary>
        /// <param name="eArg">記錄處理資料編號(第幾筆)的資料庫存取事件參數</param>
        /// <returns>傳回要記錄的資料庫處理結果 (Notation)</returns>
        private string GenLogNotation(DBRecordNoEventArgs eArg)
        {
            string pattern = null;
            switch (eArg.Action)
            {
                case DBActionEnum.Insert:
                    pattern = "新增第 {0} 筆資料{1}";
                    break;
                case DBActionEnum.Update:
                    pattern = "修改第 {0} 筆資料{1}";
                    break;
                case DBActionEnum.Delete:
                    pattern = "刪除第 {0} 筆資料{1}";
                    break;
                case DBActionEnum.Select:
                    pattern = "查詢第 {0} 筆資料{1}";
                    break;
                default:
                    pattern = "處理第 {0} 筆資料{1}";
                    break;
            }

            //TODO:如要記錄更詳細的資料可修改這裡
            if (eArg.Result != null && !eArg.Result.IsSuccess)
            {
                return String.Format(pattern, eArg.RecordNo, "失敗，錯誤訊息：" + eArg.Result.Message);
            }
            else
            {
                return String.Format(pattern, eArg.RecordNo, "成功");
            }
        }

        /// <summary>
        /// 產生資料庫處理結果
        /// </summary>
        /// <param name="eArg">DBEventArgs 的事件參數</param>
        /// <returns>傳回要記錄的資料庫處理結果 (Notation)</returns>
        private string GenLogNotation(DBEventArgs eArg)
        {
            //TODO:如要記錄更詳細的資料可修改這裡
            string pattern = null;
            switch (eArg.Action)
            {
                case DBActionEnum.Insert:
                    pattern = "新增資料{0}";
                    break;
                case DBActionEnum.Update:
                    pattern = "修改資料{0}";
                    break;
                case DBActionEnum.Delete:
                    pattern = "刪除資料{0}";
                    break;
                case DBActionEnum.Select:
                    pattern = "查詢資料{0}";
                    break;
                default:
                    pattern = "處理資料{0}";
                    break;
            }
            if (eArg.Result != null && !eArg.Result.IsSuccess)
            {
                return String.Format(pattern, "失敗，錯誤訊息：" + eArg.Result.Message);
            }
            else
            {
                return String.Format(pattern, "成功");
            }
        }

        /// <summary>
        /// 取得指定資料庫存取類別事件是否為要忽略日誌
        /// </summary>
        /// <param name="e">資料庫存取類別事件參數</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        private bool IsSkipLog(DBEventArgs e)
        {
            if (this.IsSkipAllLog || this.IsSkipEventLog)
            {
                return true;
            }
            if (e.Action == DBActionEnum.Select)
            {
                return this.IsSkipSelectLog;
            }
            else
            {
                return this.IsSkipNormalLog;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 此物件是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false。</returns>
        public bool IsReady()
        {
            return String.IsNullOrEmpty(this.IntitalError);
        }

        /// <summary>
        /// 取得待儲存的日誌資料筆數
        /// </summary>
        /// <returns>傳回待儲存的日誌資料筆數。</returns>
        public int GetLogCount()
        {
            return _Logs.Count;
        }

        /// <summary>
        /// 附加資料庫處理日誌
        /// </summary>
        /// <param name="role">請求者的身分別</param>
        /// <param name="receiveType">代收類別代碼</param>
        /// <param name="functionId">功能代碼</param>
        /// <param name="logType">操作代碼</param>
        /// <param name="userId">使用者代碼</param>
        /// <param name="notation">處理結果</param>
        public void AppendLog(string role, string receiveType, string functionId, string logType, string userId, string notation)
        {
            if (this.IsSkipAllLog)
            {
                return;
            }
            if (!String.IsNullOrEmpty(this.IntitalError))
            {
                return;
            }

            //[TODO] 可以考慮判斷 logType 決定是否寫日誌
            LogTableEntity log = new LogTableEntity(DateTime.Now);
            log.Role = role;
            log.ReceiveType = receiveType;
            log.FunctionId = functionId;
            log.LogType = logType;
            log.UserId = userId;
            log.Notation = notation;
            _Logs.Add(log);
        }

        /// <summary>
        /// 附加資料庫處理日誌
        /// </summary>
        /// <param name="log">處理日誌</param>
        public void AppendLog(LogTableEntity log)
        {
            if (this.IsSkipAllLog)
            {
                return;
            }
            if (!String.IsNullOrEmpty(this.IntitalError))
            {
                return;
            }

            //[TODO] 可以考慮判斷 logType 決定是否寫日誌
            LogTableEntity newLog = new LogTableEntity();
            newLog.Role = log.Role;
            newLog.ReceiveType = log.ReceiveType;
            newLog.FunctionId = log.FunctionId;
            newLog.LogType = log.LogType;
            newLog.UserId = log.UserId;
            newLog.Notation = log.Notation;
            newLog.LogDate = log.LogDate;
            newLog.LogTime = log.LogTime;
            _Logs.Add(newLog);
        }

        /// <summary>
        /// 非同步儲存日誌資料
        /// </summary>
        /// <returns>傳回錯誤訊息。</returns>
        public string AsyncSave()
        {
            if (!String.IsNullOrEmpty(this.IntitalError))
            {
                return this.IntitalError;
            }
            if (_Factory == null || !_Factory.IsReady())
            {
                return "資料庫存取物件未準備好";
            }
            if (this.GetLogCount() < 1 || this.IsSkipAllLog)
            {
                return String.Empty;
            }

            string errmsg = null;
            try
            {
                LogTableEntity[] logDatas = _Logs.ToArray();
                _Logs.Clear();

                AsyncSaveLog myAsync = new AsyncSaveLog(AsyncSaveLogs);
                IAsyncResult result = myAsync.BeginInvoke(_Factory.CloneForTransaction(), logDatas, null, null);
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg ?? String.Empty;
        }

        /// <summary>
        /// (同步)儲存日誌資料
        /// </summary>
        /// <returns>傳回錯誤訊息。</returns>
        public string Save()
        {
            if (!String.IsNullOrEmpty(this.IntitalError))
            {
                return this.IntitalError;
            }
            if (_Factory == null || !_Factory.IsReady())
            {
                return "資料庫存取物件未準備好";
            }
            if (this.GetLogCount() < 1 || this.IsSkipAllLog)
            {
                return String.Empty;
            }

            string errmsg = null;
            try
            {
                LogTableEntity[] logDatas = _Logs.ToArray();
                _Logs.Clear();

                Result result = this.SaveLogs(_Factory, logDatas);
                if (!result.IsSuccess)
                {
                    errmsg = result.Message;
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg ?? String.Empty;
        }

        /// <summary>
        /// 非同步儲存日誌資料到資料庫
        /// </summary>
        /// <param name="factory">資料存取物件</param>
        /// <param name="logDatas">日誌資料陣列</param>
        /// <returns>傳回處理結果</returns>
        private Result AsyncSaveLogs(EntityFactory factory, LogTableEntity[] logDatas)
        {
            Result result = this.SaveLogs(factory, logDatas);
            if (factory != null)
            {
                factory.Dispose();
                factory = null;
            }
            return result;
        }

        /// <summary>
        /// (同步)儲存日誌資料到資料庫
        /// </summary>
        /// <param name="factory">資料存取物件</param>
        /// <param name="logDatas">日誌資料陣列</param>
        /// <returns>傳回處理結果</returns>
        private Result SaveLogs(EntityFactory factory, LogTableEntity[] logDatas)
        {
            if (factory == null || !factory.IsReady())
            {
                return new Result(false, "資料庫存取物件未準備好", ErrorCode.S_INVALID_FACTORY, null);
            }

            Result result = null;
            try
            {
                int count = 0;
                int[] failIndexs = null;
                result = factory.Insert(logDatas, false, out count, out failIndexs);
                factory.Commit();
            }
            catch (Exception ex)
            {
                result = new Result(ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    factory.Rollback();
                }
            }
            return result;
        }
        #endregion

        #region [MDY:20160310] Key Value 相關 Method
        /// <summary>
        /// 取得 Where 條件的 Key=Value 文字表示
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public string GetWhereKeyValueText(Expression where)
        {
            if (where != null && !where.IsEmpty())
            {
                StringBuilder sb = new StringBuilder();
                this.GenWhereKeyValueText(ref sb, where);
                if (sb.Length > 2)
                {
                    return sb.ToString(0, sb.Length - 2);
                }
                else
                {
                    sb.ToString();
                }
            }
            return String.Empty;
        }

        private void GenWhereKeyValueText(ref StringBuilder sb, Expression where)
        {
            if (where != null)
            {
                sb.AppendFormat("{0}={1}; ", where.Field, where.Value);
                Expression[] others = where.Others;
                if (others != null && others.Length > 0)
                {
                    foreach (Expression other in others)
                    {
                        GenWhereKeyValueText(ref sb, other);
                    }
                }
            }
        }

        public string GetEntityPKeyValueText(IEntity data)
        {
            FieldSpec[] fields = data == null ? null : data.GetPrimaryKeyFieldSpec();
            if (fields != null && fields.Length > 0)
            {
                Result result = null;
                StringBuilder sb = new StringBuilder();
                foreach (FieldSpec field in fields)
                {
                    object value = data.GetValue(field.FieldName, out result);
                    if (result.IsSuccess)
                    {
                        sb.AppendFormat("{0}={1}; ", field.FieldName, value);
                    }
                    else
                    {
                        sb.AppendFormat("{0}={1}; ", field.FieldName, "??");
                    }
                }
                return sb.ToString(0, sb.Length - 2);
            }
            return String.Empty;
        }

        public string GetEntityPKeyValueText(IEntity[] datas)
        {
            if (datas != null && datas.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (IEntity data in datas)
                {
                    string text = this.GetEntityPKeyValueText(data);
                    sb.Append(text).Append(" & ");
                }
                return sb.ToString(0, sb.Length - 3);
            }
            return String.Empty;
        }
        #endregion
    }

    #region [MDY:20220910] Checkmarx - Improper Resource Shutdown or Release 誤判調整
    #region [OLD]
    ///// <summary>
    ///// 資料庫日誌記錄器的工具類別
    ///// </summary>
    //public class DBLoggerHelper
    //{
    //    #region Constructor
    //    /// <summary>
    //    /// 建構式
    //    /// </summary>
    //    private DBLoggerHelper()
    //    {
    //    }
    //    #endregion

    //    #region Static Method
    //    /// <summary>
    //    /// 以指定系統設定組態群組名稱的設定，建立資料庫日誌記錄器
    //    /// </summary>
    //    /// <param name="configGroupName">指定系統設定組態群組名稱。不指定則使用預設的組態群組名稱 (DBLogger)。</param>
    //    /// <param name="role">請求者的身分別</param>
    //    /// <param name="receiveType">記錄日誌的代收類別代碼</param>
    //    /// <param name="functionId">記錄日誌的功能代碼。</param>
    //    /// <param name="userId">記錄日誌的使用者代碼。</param>
    //    /// <returns>成功則傳回資料庫日誌記錄器物件，否則傳回 null。</returns>
    //    public static DBLogger CreateSystemDBLogger(string configGroupName, string role, string receiveType, string functionId, string userId)
    //    {
    //        #region 檢查參數
    //        if (Common.IsNullOrSpace(configGroupName))
    //        {
    //            configGroupName = "DBLogger";
    //        }
    //        else
    //        {
    //            configGroupName = configGroupName.Trim();
    //        }
    //        #endregion

    //        #region 取組態設定
    //        KeyValueList<string> configs = ConfigManager.Current.GetSystemConfigs(configGroupName, StringComparison.CurrentCultureIgnoreCase);
    //        if (configs == null || configs.Count == 0)
    //        {
    //            return null;
    //        }
    //        #endregion

    //        #region Mode
    //        DBLogger.ModeEnum mode = DBLogger.ModeEnum.None;
    //        {
    //            int idx = configs.GetKeyFirstIndex("Mode", StringComparison.CurrentCultureIgnoreCase);
    //            if (idx < 0)
    //            {
    //                return null;
    //            }
    //            int value = 0;
    //            string text = configs[idx].Value;
    //            if (String.IsNullOrEmpty(text) || !Int32.TryParse(text, out value))
    //            {
    //                return null;
    //            }
    //            value &= (int) DBLogger.ModeEnum.All;
    //            mode = (DBLogger.ModeEnum)value;
    //        }
    //        #endregion

    //        #region DBConfigName
    //        string dbConfigName = null;
    //        {
    //            int idx = configs.GetKeyFirstIndex("DBConfigName", StringComparison.CurrentCultureIgnoreCase);
    //            if (idx > -1)
    //            {
    //                dbConfigName = configs[idx].Value;
    //            }
    //        }
    //        #endregion

    //        DBLogger dbLogger = new DBLogger(mode, dbConfigName, role, receiveType, functionId, userId);
    //        return dbLogger;
    //    }
    //    #endregion
    //}
    #endregion
    #endregion
}
