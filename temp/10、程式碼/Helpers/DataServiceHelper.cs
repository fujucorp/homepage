using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

using Entities;

namespace Helpers
{
    /// <summary>
    /// 資料處理服務的資料存取工具類別
    /// </summary>
    public partial class DataServiceHelper
    {
        #region Member
        /// <summary>
        /// 儲存 資料存取物件 的變數
        /// </summary>
        private EntityFactory _Factory = null;

        #region Property
        private string _TempPath = null;
        /// <summary>
        /// 設定或取得檔案暫存路徑
        /// </summary>
        public string TempPath
        {
            get
            {
                return _TempPath;
            }
            set
            {
                _TempPath = value == null ? null : value.Trim();
                if (String.IsNullOrEmpty(_TempPath))
                {
                    _TempPath = Path.GetTempPath();
                }
                else
                {
                    try
                    {
                        if (!Directory.Exists(_TempPath))
                        {
                            Directory.CreateDirectory(_TempPath);
                        }
                    }
                    catch (Exception)
                    {
                        _TempPath = Path.GetTempPath();
                    }
                }
            }
        }
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// 建構資料處理服務的資料存取工具類別
        /// </summary>
        public DataServiceHelper()
            : this(new EntityFactory(), null)
        {
        }

        /// <summary>
        /// 建構資料處理服務的資料存取工具類別
        /// </summary>
        /// <param name="factory">指定資料存取物件。</param>
        public DataServiceHelper(EntityFactory factory)
            : this(factory, null)
        {
        }

        /// <summary>
        /// 建構資料處理服務的資料存取工具類別
        /// </summary>
        /// <param name="tempPath">指定暫存檔路徑</param>
        public DataServiceHelper(string tempPath)
            : this(new EntityFactory(), tempPath)
        {
        }

        /// <summary>
        /// 建構資料處理服務的資料存取工具類別
        /// </summary>
        /// <param name="factory">指定資料存取物件。</param>
        /// <param name="tempPath">指定暫存檔路徑</param>
        private DataServiceHelper(EntityFactory factory, string tempPath)
        {
            _Factory = factory;
            _TempPath = tempPath;
        }
        #endregion

        #region IsReady
        /// <summary>
        /// 取得此物件是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false。</returns>
        public bool IsReady()
        {
            return (_Factory != null && _Factory.IsReady());
        }
        #endregion

        #region 資料庫日誌記錄器 相關
        /// <summary>
        /// 取得初始化的資料庫日誌記錄器
        /// </summary>
        /// <param name="asker">服務命令請求者資料物件。</param>
        /// <returns>成功則傳回資料庫日誌記錄器物件，否則傳回 null。</returns>
        private DBLogger InitialDBLogger(CommandAsker asker)
        {
            DBLogger dbLogger = DBLoggerHelper.CreateSystemDBLogger(null, asker.UserQual, asker.UnitId, asker.FuncId, asker.UserId);
            _Factory.SetDBLogger(dbLogger);
            return dbLogger;
        }

        /// <summary>
        /// 儲存日誌資料並釋放資料庫日誌記錄器
        /// </summary>
        /// <param name="isUseAsync">是否使用非同步儲存日誌資料。</param>
        /// <returns>傳回錯誤訊息。</returns>
        private string ReleaseDBLogger(bool isUseAsync)
        {
            if (_Factory.DBLogger == null)
            {
                return String.Empty;
            }

            string errmsg = null;
            if (_Factory.DBLogger is DBLogger)
            {
                if (isUseAsync)
                {
                    errmsg = (_Factory.DBLogger as DBLogger).AsyncSave();
                }
                else
                {
                    errmsg = (_Factory.DBLogger as DBLogger).Save();
                }
            }
            _Factory.SetDBLogger(null);
            return errmsg ?? String.Empty;
        }
        #endregion

        #region 新增資料
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="count">傳回受影響的資料列數目。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result Insert(IServiceCommand command, out CommandAsker commandAsker
            , out int count)
        {
            commandAsker = null;
            count = 0;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            InsertCommand myCommand = command is InsertCommand ? (InsertCommand)command : InsertCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", InsertCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", InsertCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region INSTANCE
            IEntity instance = null;
            if (!myCommand.GetInstance(out instance)
                || instance == null)	//不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", InsertCommand.INSTANCE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 新增資料
            {
                Result result = _Factory.Insert(instance, out count);
                if (!result.IsSuccess)
                {
                    count = 0;

                    System.Data.SqlClient.SqlException sqlException = result.Exception as System.Data.SqlClient.SqlException;
                    if (sqlException != null && sqlException.Number == 2627)
                    {
                        result = new Result(result.IsSuccess, "該資料已存在或違犯唯一值限制", CoreStatusCode.D_NOT_DATA_INSERT, result.Exception);
                    }
                    else
                    {
                        if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_INSERT_DATA_EXCEPTION, result.Exception);
                        }
                        else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_INSERT_DATA_FAILURE, result.Exception);
                        }
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string keyText = dbLogger.GetEntityPKeyValueText(instance);

                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}資料成功 (Key：{2}，共{3}筆)", commandAsker.FuncName, LogTypeCodeTexts.INSERT_TEXT, keyText, count);
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}資料失敗 (Key：{2}，錯誤訊息：{3})", commandAsker.FuncName, LogTypeCodeTexts.INSERT_TEXT, keyText, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.INSERT, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 新增多筆資料
        /// <summary>
        /// 新增多筆資料
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="count">傳回受影響的資料列數目。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result InsertAll(IServiceCommand command, out CommandAsker commandAsker, out int count
            , out int[] failIndexs)
        {
            commandAsker = null;
            count = 0;
            failIndexs = null;
            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            InsertAllCommand myCommand = command is InsertAllCommand ? (InsertAllCommand)command : InsertAllCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", InsertAllCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", InsertAllCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region INSTANCES
            IEntity[] instances = null;
            if (!myCommand.GetInstances(out instances)
                || instances == null)	//不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", InsertAllCommand.INSTANCES);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region IS_BATCH
            bool isBatch = true;
            if (!myCommand.GetIsBatch(out isBatch))	//允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", InsertAllCommand.IS_BATCH);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 新增多筆資料
            {
                Result result = _Factory.Insert(instances, isBatch, out count, out failIndexs);
                if (!result.IsSuccess)
                {
                    if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_INSERT_DATA_EXCEPTION, result.Exception);
                    }
                    else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_INSERT_DATA_FAILURE, result.Exception);
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string keyText = dbLogger.GetEntityPKeyValueText(instances);

                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}資料成功 (Key：{2}，共{3}筆)", commandAsker.FuncName, LogTypeCodeTexts.INSERT_TEXT, keyText, count);
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}資料失敗 (Key：{2}，錯誤訊息：{3})", commandAsker.FuncName, LogTypeCodeTexts.INSERT_TEXT, keyText, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.INSERT, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 更新資料
        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="count">傳回受影響的資料列數目。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result Update(IServiceCommand command, out CommandAsker commandAsker
            , out int count)
        {
            commandAsker = null;
            count = 0;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            UpdateCommand myCommand = command is UpdateCommand ? (UpdateCommand)command : UpdateCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UpdateCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UpdateCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region INSTANCE
            IEntity instance = null;
            if (!myCommand.GetInstance(out instance)
                || instance == null)	//不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UpdateCommand.INSTANCE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 更新資料
            {
                Result result = _Factory.Update(instance, out count);
                if (!result.IsSuccess)
                {
                    count = 0;
                    if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_INSERT_DATA_EXCEPTION, result.Exception);
                    }
                    else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_INSERT_DATA_FAILURE, result.Exception);
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string keyText = dbLogger.GetEntityPKeyValueText(instance);

                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}資料成功 (Key：{2}，共{3}筆)", commandAsker.FuncName, LogTypeCodeTexts.UPDATE_TEXT, keyText, count);
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}資料失敗 (Key：{2}，錯誤訊息：{3})", commandAsker.FuncName, LogTypeCodeTexts.UPDATE_TEXT, keyText, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.UPDATE, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 更新指定欄位值
        /// <summary>
        /// 更新欄位資料
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="count">傳回受影響的資料列數目。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result UpdateFields(IServiceCommand command, out CommandAsker commandAsker
            , out int count)
        {
            commandAsker = null;
            count = 0;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            UpdateFieldsCommand myCommand = command is UpdateFieldsCommand ? (UpdateFieldsCommand)command : UpdateFieldsCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UpdateFieldsCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UpdateFieldsCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region WHERE
            Expression where = null;
            if (!myCommand.GetWhere(out where)
                || where == null || !where.IsReady())	//允許無查詢條件但不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UpdateFieldsCommand.WHERE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region FIELD_VALUES
            KeyValue[] fieldValues = null;
            if (!myCommand.GetFieldValues(out fieldValues)
                || fieldValues == null || fieldValues.Length == 0)	//不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UpdateFieldsCommand.FIELD_VALUES);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 更新指定欄位值
            {
                Result result = _Factory.UpdateFields(entityType, fieldValues, where, out count);
                if (!result.IsSuccess)
                {
                    count = 0;
                    if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_INSERT_DATA_EXCEPTION, result.Exception);
                    }
                    else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_INSERT_DATA_FAILURE, result.Exception);
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string keyText = dbLogger.GetWhereKeyValueText(where);

                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}欄位資料成功 (Key：{2}，共{3}筆)", commandAsker.FuncName, LogTypeCodeTexts.UPDATE_TEXT, keyText, count);
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}欄位資料失敗 (Key：{2}，錯誤訊息：{3})", commandAsker.FuncName, LogTypeCodeTexts.UPDATE_TEXT, keyText, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.UPDATE, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 刪除資料
        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="count">傳回受影響的資料列數目。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result Delete(IServiceCommand command, out CommandAsker commandAsker
            , out int count)
        {
            commandAsker = null;
            count = 0;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            DeleteCommand myCommand = command is DeleteCommand ? (DeleteCommand)command : DeleteCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", DeleteCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", DeleteCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region INSTANCE
            IEntity instance = null;
            if (!myCommand.GetInstance(out instance)
                || instance == null)	//不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", DeleteCommand.INSTANCE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 刪除資料
            {
                Result result = _Factory.Delete(instance, out count);
                if (!result.IsSuccess)
                {
                    count = 0;
                    if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_INSERT_DATA_EXCEPTION, result.Exception);
                    }
                    else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_INSERT_DATA_FAILURE, result.Exception);
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string keyText = dbLogger.GetEntityPKeyValueText(instance);

                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}資料成功 (Key：{2}，共{3}筆)", commandAsker.FuncName, LogTypeCodeTexts.DELETE_TEXT, keyText, count);
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}資料失敗 (Key：{2}，錯誤訊息：{3})", commandAsker.FuncName, LogTypeCodeTexts.DELETE_TEXT, keyText, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.DELETE, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 刪除多筆資料
        /// <summary>
        /// 刪除多筆資料
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="count">傳回受影響的資料列數目。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result DeleteAll(IServiceCommand command, out CommandAsker commandAsker, out int count
            , out int[] failIndexs)
        {
            commandAsker = null;
            count = 0;
            failIndexs = null;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            DeleteAllCommand myCommand = command is DeleteAllCommand ? (DeleteAllCommand)command : DeleteAllCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 a{0} 命令參數", DeleteAllCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 b{0} 命令參數", DeleteAllCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region INSTANCE
            IEntity[] instances = null;
            if (!myCommand.GetInstances(out instances)
                || instances == null)	//不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 c{0} 命令參數", DeleteAllCommand.INSTANCES);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 刪除多筆資料
            {
                Result result = _Factory.Delete(instances, true, out count, out failIndexs);
                if (!result.IsSuccess)
                {
                    if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_DELETE_DATA_EXCEPTION, result.Exception);
                    }
                    else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_DELETE_DATA_FAILURE, result.Exception);
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string keyText = dbLogger.GetEntityPKeyValueText(instances);

                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}資料成功 (Key：{2}，共{3}筆)", commandAsker.FuncName, LogTypeCodeTexts.DELETE_TEXT, keyText, count);
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}資料失敗 (Key：{2}，錯誤訊息：{3})", commandAsker.FuncName, LogTypeCodeTexts.DELETE_TEXT, keyText, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.DELETE, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 查詢資料
        /// <summary>
        /// 查詢資料
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="datas">傳回查詢結果的資料陣列。(嘗試轉型成 Entity 陣列)</param>
        /// <param name="totalCount">傳回符合查詢結果的總筆數。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result Select(IServiceCommand command, out CommandAsker commandAsker
            , out Entity[] datas, out int totalCount)
        {
            commandAsker = null;
            datas = null;
            totalCount = 0;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            SelectCommand myCommand = command is SelectCommand ? (SelectCommand)command : SelectCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region WHERE
            Expression where = null;
            if (!myCommand.GetWhere(out where)
                || where == null || !where.IsReady())	//允許無查詢條件但不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectCommand.WHERE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ORDER_BYS
            KeyValueList<OrderByEnum> orderbys = null;
            if (!myCommand.GetOrderBys(out orderbys))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", SelectCommand.ORDER_BYS);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region START_INDEX
            int? startIndex = null;
            if (!myCommand.GetStartIndex(out startIndex))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", SelectCommand.START_INDEX);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (startIndex == null || startIndex < 0)
            {
                startIndex = 0;
            }
            #endregion

            #region MAX_RECORDS
            int? maxRecords = null;
            if (!myCommand.GetMaxRecords(out maxRecords))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", SelectCommand.MAX_RECORDS);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (maxRecords == null || maxRecords < 0)
            {
                maxRecords = 0;
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 查詢筆數
            {
                Result result = _Factory.SelectCount(entityType, where, out totalCount);
                if (!result.IsSuccess)
                {
                    //#region 轉換錯誤代碼
                    //if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    //{
                    //    result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                    //}
                    //else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    //{
                    //    result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                    //}
                    //#endregion
                }

                #region 新增日誌資料
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}資料總筆數成功 (資料筆數：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, totalCount);
                    }
                    else
                    {
                        if (result.Exception == null)
                        {
                            notation = String.Format("[{0}] {1}資料總筆數失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                        }
                        else
                        {
                            notation = String.Format("[{0}] {1}資料總筆數失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Exception.Message);
                        }
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                }
                #endregion

                if (totalCount == 0 || !result.IsSuccess)
                {
                    #region 儲存日誌資料並釋放資料庫日誌記錄器
                    if (dbLogger != null)
                    {
                        string logErrmsg = this.ReleaseDBLogger(true);
                        dbLogger.Dispose();
                        dbLogger = null;
                    }
                    #endregion

                    return result;
                }
            }
            #endregion

            #region 查詢資料
            {
                IEntity[] instances = null;
                Result result = _Factory.Select(entityType, where, orderbys, startIndex.Value, maxRecords.Value, out instances);
                if (result.IsSuccess)
                {
                    //因為 IEntity 為 interface 無法序列化，所以要轉成 Entity
                    datas = EntityUtility.TryConvert<Entity>(instances);

                    //檢查是否全部轉型成功
                    int count1 = instances == null ? 0 : instances.Length;
                    int count2 = datas == null ? 0 : datas.Length;
                    if (count2 != count1)
                    {
                        result = new Result(false, "查詢結果無法全部轉換成 Entity 型別", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                    }
                }
                else
                {
                    if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                    }
                    else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}資料成功 (共{2}筆)", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, (instances == null ? 0 : instances.Length));
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}資料失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 查詢首筆資料
        /// <summary>
        /// 查詢首筆資料
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="data">傳回查詢結果的資料。(嘗試轉型成 Entity)</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result SelectFirst(IServiceCommand command, out CommandAsker commandAsker
            , out Entity data)
        {
            commandAsker = null;
            data = null;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            SelectFirstCommand myCommand = command is SelectFirstCommand ? (SelectFirstCommand)command : SelectFirstCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectFirstCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectFirstCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region WHERE
            Expression where = null;
            if (!myCommand.GetWhere(out where)
                || where == null || !where.IsReady())	//允許無查詢條件但不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectFirstCommand.WHERE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ORDER_BYS
            KeyValueList<OrderByEnum> orderbys = null;
            if (!myCommand.GetOrderBys(out orderbys))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", SelectFirstCommand.ORDER_BYS);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 查詢首筆資料
            {
                IEntity instance = null;
                Result result = _Factory.SelectFirst(entityType, where, orderbys, out instance);
                if (result.IsSuccess)
                {
                    //因為 IEntity 為 interface 無法序列化，所以要轉成 Entity
                    data = instance as Entity;

                    //檢查是否轉型成功
                    if (data == null && instance != null)
                    {
                        result = new Result(false, "查詢結果無法轉換成 Entity 型別", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                    }
                }
                else
                {
                    if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                    }
                    else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}首筆資料成功", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT);
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}首筆資料失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 查詢所有資料
        /// <summary>
        /// 查詢所有資料
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="datas">傳回查詢結果的資料陣列。(嘗試轉型成 Entity 陣列)</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result SelectAll(IServiceCommand command, out CommandAsker commandAsker
            , out Entity[] datas)
        {
            commandAsker = null;
            datas = null;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            SelectAllCommand myCommand = command is SelectAllCommand ? (SelectAllCommand)command : SelectAllCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectAllCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectAllCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region WHERE
            Expression where = null;
            if (!myCommand.GetWhere(out where)
                || where == null || !where.IsReady())	//允許無查詢條件但不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectAllCommand.WHERE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ORDER_BYS
            KeyValueList<OrderByEnum> orderbys = null;
            if (!myCommand.GetOrderBys(out orderbys))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", SelectAllCommand.ORDER_BYS);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 查詢所有資料
            {
                IEntity[] instances = null;
                Result result = _Factory.SelectAll(entityType, where, orderbys, out instances);
                if (result.IsSuccess)
                {
                    //因為 IEntity 為 interface 無法序列化，所以要轉成 Entity
                    datas = EntityUtility.TryConvert<Entity>(instances);

                    //檢查是否全部轉型成功
                    int count1 = instances == null ? 0 : instances.Length;
                    int count2 = datas == null ? 0 : datas.Length;
                    if (count2 != count1)
                    {
                        result = new Result(false, "查詢結果無法全部轉換成 Entity 型別", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                    }
                }
                else
                {
                    if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                    }
                    else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}所有資料成功 (共{2}筆)", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, (instances == null ? 0 : instances.Length));
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}所有資料失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 查詢資料筆數
        /// <summary>
        /// 查詢資料筆數
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="count">傳回查詢結果的資料筆數。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result SelectCount(IServiceCommand command, out CommandAsker commandAsker
            , out int count)
        {
            commandAsker = null;
            count = 0;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            SelectCountCommand myCommand = command is SelectCountCommand ? (SelectCountCommand)command : SelectCountCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectCountCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectCountCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region WHERE
            Expression where = null;
            if (!myCommand.GetWhere(out where)
                || where == null || !where.IsReady())	//允許無查詢條件但不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", SelectCountCommand.WHERE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 查詢資料筆數
            {
                Result result = _Factory.SelectCount(entityType, where, out count);
                if (!result.IsSuccess)
                {
                    count = 0;
                    if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                    }
                    else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}資料筆數成功", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT);
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}資料筆數失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 行員登入
        /// <summary>
        /// 行員登入
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandAsker"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual Result BankLogon(IServiceCommand command, out CommandAsker commandAsker
            , out LogonUser data)
        {
            commandAsker = null;
            data = null;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            BankLogonCommand myCommand = command is BankLogonCommand ? (BankLogonCommand)command : BankLogonCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", BankLogonCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region [MDY:20220530] Checkmarx 調整
            #region USER_PXX
            string pxx = null;
            if (!myCommand.GetUserPXX(out pxx)
                || String.IsNullOrEmpty(pxx))   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", BankLogonCommand.USER_PXX);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region CLIENT_IP
            string clientIP = null;
            if (!myCommand.GetClientIP(out clientIP))
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", BankLogonCommand.CLIENT_IP);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 行員登入
            {
                LogonHelper helper = new LogonHelper(_Factory);

                #region [MDY:20220530] Checkmarx 調整
                Result result = helper.BankLogon(commandAsker, pxx, clientIP, out data);

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] (身分別：{1}, 單位：{2}, 帳號：{3}, IP：{4}) 登入成功",
                            commandAsker.FuncName, commandAsker.UserQual, commandAsker.UnitId, commandAsker.UserId, clientIP);
                    }
                    else
                    {
                        notation = String.Format("[{0}] (身分別：{1}, 單位：{2}, 帳號：{3}, 密碼：{4}, IP：{5}) 登入失敗 (錯誤訊息：{6})",
                            commandAsker.FuncName, commandAsker.UserQual, commandAsker.UnitId, commandAsker.UserId, pxx, clientIP, result.Message);
                    }
                    string receiveType = data != null ? data.UnitId : commandAsker.UnitId;  //因為行員登入不用輸入分行代碼，所以在這裡加工處理
                    dbLogger.AppendLog(commandAsker.UserQual, receiveType, commandAsker.FuncId, LogTypeCodeTexts.EXECUTE, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 學校登入
        /// <summary>
        /// 學校登入
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="data">登入成功則傳回登入者資料，否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result SchoolLogon(IServiceCommand command, out CommandAsker commandAsker
            , out LogonUser data)
        {
            commandAsker = null;
            data = null;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            UserLogonCommand myCommand = command is UserLogonCommand ? (UserLogonCommand)command : UserLogonCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UserLogonCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region [MDY:20220530] Checkmarx 調整
            #region USER_PXX
            string pxx = null;
            if (!myCommand.GetUserPXX(out pxx)
                || String.IsNullOrEmpty(pxx))   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UserLogonCommand.USER_PXX);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region CLIENT_IP
            string clientIP = null;
            if (!myCommand.GetClientIP(out clientIP))
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UserLogonCommand.CLIENT_IP);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 學校登入
            {
                LogonHelper helper = new LogonHelper(_Factory);
                Result result = helper.SchoolLogon(commandAsker, pxx, clientIP, out data);

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] (身分別：{1}, 單位：{2}, 帳號：{3}, IP：{4}) 登入成功",
                            commandAsker.FuncName, commandAsker.UserQual, commandAsker.UnitId, commandAsker.UserId, clientIP);
                    }
                    else
                    {
                        notation = String.Format("[{0}] (身分別：{1}, 單位：{2}, 帳號：{3}, 密碼：{4}, IP：{5}) 登入失敗 (錯誤訊息：{6})",
                            commandAsker.FuncName, commandAsker.UserQual, commandAsker.UnitId, commandAsker.UserId, pxx, clientIP, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.EXECUTE, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 學生登入
        /// <summary>
        /// 學生登入
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="data">登入成功則傳回登入者資料，否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result StudentLogon(IServiceCommand command, out CommandAsker commandAsker
            , out LogonUser data)
        {
            commandAsker = null;
            data = null;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            StudentLogonCommand myCommand = command is StudentLogonCommand ? (StudentLogonCommand)command : StudentLogonCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", StudentLogonCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region SCHOOL_IDENTITY
            string schIdentity = null;
            if (!myCommand.GetSchoolIdentity(out schIdentity)
                || String.IsNullOrEmpty(schIdentity))   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", StudentLogonCommand.SCHOOL_IDENTITY);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region STUDENT_ID
            string studentId = null;
            if (!myCommand.GetStudentId(out studentId)
                || String.IsNullOrEmpty(studentId))   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", StudentLogonCommand.STUDENT_ID);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region LOGIN_KEY
            string loginKey = null;
            if (!myCommand.GetLoginKey(out loginKey)
                || String.IsNullOrEmpty(loginKey))   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", StudentLogonCommand.LOGIN_KEY);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region CLIENT_IP
            string clientIP = null;
            if (!myCommand.GetClientIP(out clientIP))
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UserLogonCommand.CLIENT_IP);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 學生登入
            {
                LogonHelper helper = new LogonHelper(_Factory);
                Result result = helper.StudentLogon(commandAsker, schIdentity, studentId, loginKey, clientIP, out data);

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] (商家代號：{1}, 學號：{2}, 登入 Key：{3}, IP：{4}) 登入成功",
                            commandAsker.FuncName, schIdentity, studentId, loginKey, clientIP);
                    }
                    else
                    {
                        notation = String.Format("[{0}] (商家代號：{1}, 學號：{2}, 登入 Key：{3}, IP：{4}) 登入失敗 (錯誤訊息：{5})",
                            commandAsker.FuncName, schIdentity, studentId, loginKey, clientIP, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.EXECUTE, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region [Old] 行員 SSO 登入 (土銀沒有)
        ///// <summary>
        ///// 行員 SSO 登入
        ///// </summary>
        ///// <param name="command">指定資料處理服務命令(介面)物件。</param>
        ///// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        ///// <param name="data">登入成功則傳回登入者資料，否則傳回 null。</param>
        ///// <returns>傳回處理結果。</returns>
        //public virtual Result SSOLogon(IServiceCommand command, out CommandAsker commandAsker
        //    , out LogonUser data)
        //{
        //    commandAsker = null;
        //    data = null;

        //    #region 檢查資料存取物件
        //    if (!this.IsReady())
        //    {
        //        return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
        //    }
        //    #endregion

        //    #region 檢查參數
        //    SSOLogonCommand myCommand = command is SSOLogonCommand ? (SSOLogonCommand)command : SSOLogonCommand.Create(command);
        //    if (myCommand == null || !myCommand.IsReady())
        //    {
        //        return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
        //    }
        //    if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
        //    {
        //        return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
        //    }
        //    #endregion

        //    #region 處理服務命令參數
        //    #region COMMAND_ASKER
        //    if (!myCommand.GetCommandAsker(out commandAsker)
        //        || commandAsker == null)    //不允許無此參數
        //    {
        //        string errmsg = String.Format("缺少或無效的 {0} 命令參數", StudentLogonCommand.COMMAND_ASKER);
        //        return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
        //    }
        //    #endregion

        //    #region SSO_PUSID
        //    string pusid = null;
        //    if (!myCommand.GetPUSID(out pusid)
        //        || String.IsNullOrEmpty(pusid))   //不允許無此參數
        //    {
        //        string errmsg = String.Format("缺少或無效的 {0} 命令參數", SSOLogonCommand.SSO_PUSID);
        //        return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
        //    }
        //    #endregion

        //    #region SSO_USER_ID
        //    string userId = null;
        //    if (!myCommand.GetUserId(out userId)
        //        || String.IsNullOrEmpty(userId))   //不允許無此參數
        //    {
        //        string errmsg = String.Format("缺少或無效的 {0} 命令參數", SSOLogonCommand.SSO_USER_ID);
        //        return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
        //    }
        //    #endregion

        //    #region SSO_USER_NAME
        //    string userName = null;
        //    if (!myCommand.GetUserName(out userName)
        //        || String.IsNullOrEmpty(userName))   //不允許無此參數
        //    {
        //        string errmsg = String.Format("缺少或無效的 {0} 命令參數", SSOLogonCommand.SSO_USER_NAME);
        //        return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
        //    }
        //    #endregion

        //    #region SSO_ROLE_IDS
        //    string[] roleIds = null;
        //    if (!myCommand.GetGroupIds(out roleIds))
        //    {
        //        string errmsg = String.Format("缺少或無效的 {0} 命令參數", SSOLogonCommand.SSO_GROUP_IDS);
        //        return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
        //    }
        //    #endregion

        //    #region SSO_BRANCH_ID
        //    string branchId = null;
        //    if (!myCommand.GetBranchId(out branchId)
        //        || String.IsNullOrEmpty(branchId))   //不允許無此參數
        //    {
        //        string errmsg = String.Format("缺少或無效的 {0} 命令參數", SSOLogonCommand.SSO_BRANCH_ID);
        //        return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
        //    }
        //    #endregion
        //    #endregion

        //    #region 取得初始化的資料庫日誌記錄器
        //    DBLogger dbLogger = this.InitialDBLogger(commandAsker);
        //    #endregion

        //    #region SSO 登入
        //    {
        //        LogonHelper helper = new LogonHelper(_Factory);
        //        Result result = helper.SSOLogon(pusid, userId, userName, roleIds, branchId, out data);

        //        #region 儲存日誌資料並釋放資料庫日誌記錄器
        //        if (dbLogger != null)
        //        {
        //            string notation = null;
        //            if (result.IsSuccess)
        //            {
        //                notation = String.Format("[{0}] (pusid:{1},userId:{2},userName:{3}) 登入成功",
        //                    commandAsker.FuncName, pusid, userId, userName);
        //            }
        //            else
        //            {
        //                string roleText = roleIds == null || roleIds.Length == 0 ? "Null or Empty" : String.Join("|", roleIds);
        //                notation = String.Format("[{0}] (pusid:{1},userId:{2},userName:{3},roleIds:{4},branchId:{5}) 登入失敗 (錯誤訊息：{6})",
        //                    commandAsker.FuncName, pusid, userId, userName, roleText, branchId, result.Message);
        //            }
        //            dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.EXECUTE, commandAsker.UserId, notation);
        //            string logErrmsg = this.ReleaseDBLogger(true);
        //            dbLogger.Dispose();
        //            dbLogger = null;
        //        }
        //        #endregion

        //        return result;
        //    }
        //    #endregion
        //}
        #endregion

        #region 檢查登入與功能狀態
        /// <summary>
        /// 檢查登入與功能狀態
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="data">傳回檢查結果代碼，請參考 CheckLogonResultCodeTexts。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result CheckLogon(IServiceCommand command, out CommandAsker commandAsker
            , out string data)
        {
            commandAsker = null;
            data = CheckLogonResultCodeTexts.CHECK_FAILURE;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            CheckLogonCommand myCommand = command is CheckLogonCommand ? (CheckLogonCommand)command : CheckLogonCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", CheckLogonCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region LOGON_SN
            string logonSN = null;
            if (!myCommand.GetLogonSN(out logonSN)
                || String.IsNullOrEmpty(logonSN))   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", CheckLogonCommand.LOGON_SN);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region CHECK_FUNC_ID
            string checkFuncId = null;
            if (!myCommand.GetCheckFuncId(out checkFuncId))   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", CheckLogonCommand.CHECK_FUNC_ID);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 檢查登入與功能狀態
            {
                LogonHelper helper = new LogonHelper(_Factory);
                Result result = helper.CheckLogonAndFunc(commandAsker, logonSN, checkFuncId, out data);

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] (logonSN:{1},checkFuncId:{2},funcId:{3}) {4}",
                            commandAsker.FuncName, logonSN, checkFuncId, commandAsker.FuncId, CheckLogonResultCodeTexts.GetText(data));
                    }
                    else
                    {
                        notation = String.Format("[{0}] (logonSN:{1},checkFuncId:{2},funcId:{3}) 處理失敗 (錯誤訊息：{4})",
                            commandAsker.FuncName, logonSN, checkFuncId, commandAsker.FuncId, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.EXECUTE, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 使用者登出
        /// <summary>
        /// 使用者登出
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result UserLogout(IServiceCommand command, out CommandAsker commandAsker)
        {
            commandAsker = null;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            UserLogoutCommand myCommand = command is UserLogoutCommand ? (UserLogoutCommand)command : UserLogoutCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UserLogoutCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region LOGON_SN
            string logonSN = null;
            if (!myCommand.GetLogonSN(out logonSN)
                || String.IsNullOrEmpty(logonSN))   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", UserLogoutCommand.LOGON_SN);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 使用者登出
            {
                LogonHelper helper = new LogonHelper(_Factory);
                Result result = helper.UserLogout(commandAsker, logonSN);

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] (登入序號:{1}) 登出成功", commandAsker.FuncName, logonSN);
                    }
                    else
                    {
                        notation = String.Format("[{0}] (登入序號:{1}) 登出失敗 (錯誤訊息：{5})", commandAsker.FuncName, logonSN, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.EXECUTE, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return result;
            }
            #endregion
        }
        #endregion

        #region 取得主要條件選項資料
        /// <summary>
        /// 取得主要條件選項資料
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="data">傳回主要條件選項資料。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result GetFilterOption(IServiceCommand command, out CommandAsker commandAsker
            , out FilterOption data)
        {
            commandAsker = null;
            data = null;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            FilterOptionCommand myCommand = command is FilterOptionCommand ? (FilterOptionCommand)command : FilterOptionCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady
                || commandAsker.LogonTime == null)    //不允許無此參數、或未登入
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", FilterOptionCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region DATA_MODE
            string dataMode = null;
            if (!myCommand.GetDataMode(out dataMode)
                || String.IsNullOrEmpty(dataMode) || dataMode.Length != 2)    //不允許無此參數，且只能兩碼
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", FilterOptionCommand.DATA_MODE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }

            //不是 L 一律視為 O
            string f1Mode = dataMode.Substring(0, 1).ToUpper();
            string f2Mode = dataMode.Substring(1, 1).ToUpper();
            if (f1Mode != "L")
            {
                f1Mode = "O";
            }
            if (f2Mode != "L")
            {
                f2Mode = "O";
            }
            #endregion

            #region RECEIVE_TYPE
            string receiveType = null;
            if (!myCommand.GetReceiveType(out receiveType))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", FilterOptionCommand.RECEIVE_TYPE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region YEAR_ID
            string yearID = null;
            if (!myCommand.GetYearID(out yearID))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", FilterOptionCommand.YEAR_ID);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region TERM_ID
            string termID = null;
            if (!myCommand.GetTermID(out termID))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", FilterOptionCommand.TERM_ID);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region DEP_ID
            string depID = null;
            if (!myCommand.GetDepID(out depID))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", FilterOptionCommand.DEP_ID);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region RECEIVE_ID
            string receiveID = null;
            if (!myCommand.GetReceiveID(out receiveID))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", FilterOptionCommand.RECEIVE_ID);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region RECEIVE_KIND
            string receiveKind = null;
            if (!myCommand.GetReceiveKind(out receiveKind))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", FilterOptionCommand.RECEIVE_KIND);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region DEFAULT_MODES
            string defaultModes = null;
            if (!myCommand.GetDefaultModes(out defaultModes))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", FilterOptionCommand.DEFAULT_MODES);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }

            //DefaultMdoe 定義：1 = 預選第一個有效的項目, 2 = 只有一個項目時，預選該項目, 3 不預選
            string receiveTypeDefaultMdoe = null;
            string yearDefaultMdoe = null;
            string termDefaultMdoe = null;
            string depDefaultMdoe = null;
            string receiveDefaultMdoe = null;

            if (String.IsNullOrEmpty(defaultModes))
            {
                //無此參數則預設為 1
                receiveTypeDefaultMdoe = "1";
                yearDefaultMdoe = "1";
                termDefaultMdoe = "1";
                depDefaultMdoe = "1";
                receiveDefaultMdoe = "1";
            }
            else if (defaultModes.Length == 5)
            {
                receiveTypeDefaultMdoe = defaultModes.Substring(0, 1);
                yearDefaultMdoe = defaultModes.Substring(1, 1);
                termDefaultMdoe = defaultModes.Substring(2, 1);
                depDefaultMdoe = defaultModes.Substring(3, 1);
                receiveDefaultMdoe = defaultModes.Substring(4, 1);
                //不是 1 或 2 或 DataMode 為 L 一律以 3 處理
                if ((receiveTypeDefaultMdoe != "1" && receiveTypeDefaultMdoe != "2") || f1Mode == "L")
                {
                    receiveTypeDefaultMdoe = "3";
                }
                if ((yearDefaultMdoe != "1" && yearDefaultMdoe != "2") || f1Mode == "L")
                {
                    yearDefaultMdoe = "3";
                }
                if ((termDefaultMdoe != "1" && termDefaultMdoe != "2") || f1Mode == "L")
                {
                    termDefaultMdoe = "3";
                }
                if ((depDefaultMdoe != "1" && depDefaultMdoe != "2") || f2Mode == "L")
                {
                    depDefaultMdoe = "3";
                }
                if ((receiveDefaultMdoe != "1" && receiveDefaultMdoe != "2") || f2Mode == "L")
                {
                    receiveDefaultMdoe = "3";
                }
            }
            else
            {
                string errmsg = String.Format("無效的 {0} 命令參數", FilterOptionCommand.DEFAULT_MODES);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 英文名稱相關 IS_ENG_UI
            bool isEngUI = false;
            if (!myCommand.GetIsEngUI(out isEngUI))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", FilterOptionCommand.IS_ENG_UI);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 查詢資料
            {
                data = new FilterOption();

                Result result = null;

                #region [MDY:202203XX] 2022擴充案 英文名稱相關
                #region 查詢商家代號資料
                {
                    KeyValueList parameters = new KeyValueList(1);
                    string sql2 = null;
                    if (f1Mode == "L")
                    {
                        if (String.IsNullOrWhiteSpace(receiveKind))
                        {
                            sql2 = String.Concat("AND [Receive_Type] = @Receive_Type_L");
                            parameters.Add("@Receive_Type_L", receiveType);
                        }
                        else
                        {
                            sql2 = String.Concat("AND [Receive_Type] = @Receive_Type_L AND [Receive_Kind] = @Receive_Kind");
                            parameters.Add("@Receive_Type_L", receiveType);
                            parameters.Add("@Receive_Kind", receiveKind);
                        }
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(receiveKind))
                        {
                            sql2 = "ORDER BY [Receive_Type]";
                        }
                        else
                        {
                            sql2 = String.Concat("AND [Receive_Kind] = @Receive_Kind ORDER BY [Receive_Type]");
                            parameters.Add("@Receive_Kind", receiveKind);
                        }
                    }

                    string sql = null;
                    switch (commandAsker.UserQual)
                    {
                        case UserQualCodeTexts.BANK:
                            #region 銀行使用者
                            {
                                if (commandAsker.IsBankManager)
                                {
                                    //銀行的管理者單位可查全部的商家代號
                                    sql = "SELECT [Receive_Type], [Sch_Name], [c_flag], [Eng_Enabled], [Sch_EName] FROM [School_Rtype] WHERE [status] = @NormalStatus " + sql2;
                                    parameters.Add("@NormalStatus", DataStatusCodeTexts.NORMAL);
                                }
                                else
                                {
                                    //銀行的使用者單位可查所屬分行的商家代號
                                    sql = "SELECT [Receive_Type], [Sch_Name], [c_flag], [Eng_Enabled], [Sch_EName] FROM [School_Rtype] WHERE [status] = @NormalStatus AND [Bank_Id] = @Bank_Id " + sql2;
                                    parameters.Add("@NormalStatus", DataStatusCodeTexts.NORMAL);
                                    parameters.Add("@Bank_Id", commandAsker.UnitId);
                                }
                            }
                            #endregion
                            break;
                        case UserQualCodeTexts.SCHOOL:
                            #region 學校使用者
                            {
                                UsersEntity asker = null;
                                Result result2 = this.GetUserByCommandAsker(null, commandAsker, out asker);
                                if (!result2.IsSuccess)
                                {
                                    return new Result(false, "無法取得命令請求者的使用者資料", CoreStatusCode.INVALID_PARAMETER, null);
                                }
                                if (asker == null)
                                {
                                    return new Result(false, "無效的命令請求者", CoreStatusCode.INVALID_PARAMETER, null);
                                }
                                string sql3 = null;
                                if (String.IsNullOrEmpty(asker.URt))
                                {
                                    sql3 = " AND [Receive_Type] IS NULL ";
                                }
                                else if (asker.URt != "*")
                                {
                                    string[] inValues = asker.URt.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string[] inKeys = new string[inValues.Length];
                                    for (int idx = 0; idx < inKeys.Length; idx++)
                                    {
                                        inKeys[idx] = String.Format("@RT{0}", idx);
                                        parameters.Add(inKeys[idx], inValues[idx]);
                                    }
                                    sql3 = " AND [Receive_Type] IN (" + String.Join(",", inKeys) + ")";
                                }

                                //string sql3 = " AND EXISTS (SELECT 1 FROM [Users] WHERE U_ID = @UserId AND U_Grp = @GroupId AND U_Bank = @UnitId AND [Users].U_RT LIKE '%,' + [School_Rtype].[Receive_Type] + ',%') ";
                                //parameters.Add("@UserId", commandAsker.GroupId);
                                //parameters.Add("@GroupId", commandAsker.UserId);
                                //parameters.Add("@UnitId", commandAsker.UnitId);

                                //學校使用者只能查自己的商家代號
                                sql = "SELECT [Receive_Type], [Sch_Name], [c_flag], [Eng_Enabled], [Sch_EName] FROM [School_Rtype] WHERE [status] = @NormalStatus AND [Sch_Identy] = @Sch_Identy " + sql3 + sql2;
                                parameters.Add("@NormalStatus", DataStatusCodeTexts.NORMAL);
                                parameters.Add("@Sch_Identy", commandAsker.UnitId);
                            }
                            #endregion
                            break;
                        case UserQualCodeTexts.STUDENT:
                            #region 學生使用者
                            {
                                //學生使用者只能查自己的商家代號
                                sql = "SELECT [Receive_Type], [Sch_Name], [c_flag], [Eng_Enabled], [Sch_EName] FROM [School_Rtype] WHERE [status] = @NormalStatus AND [Receive_Type] = @Receive_Type " + sql2;
                                parameters.Add("@NormalStatus", DataStatusCodeTexts.NORMAL);
                                parameters.Add("@Receive_Type", commandAsker.UnitId);
                            }
                            #endregion
                            break;
                    }

                    string firstReceiveType = null;
                    string firstCFlag = null;
                    string selectedReceiveType = null;
                    string selectedCFlag = null;
                    bool firstEngEnabled = false;
                    bool selectedEngEnabled = false;
                    DataTable dt = null;
                    result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                    if (result.IsSuccess)
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            #region 第一筆的 cFlag
                            {
                                DataRow dRow = dt.Rows[0];
                                firstReceiveType = dRow.IsNull("Receive_Type") ? String.Empty : dRow["Receive_Type"].ToString();
                                firstCFlag = dRow.IsNull("c_flag") ? String.Empty : dRow["c_flag"].ToString();
                                firstEngEnabled = dRow.IsNull("Eng_Enabled") ? false : "Y".Equals(dRow["Eng_Enabled"].ToString());
                            }
                            #endregion

                            #region 逐筆處理
                            {
                                CodeText[] items = new CodeText[dt.Rows.Count];
                                int idx = 0;
                                foreach (DataRow dRow in dt.Rows)
                                {
                                    string code = dRow.IsNull("Receive_Type") ? String.Empty : dRow["Receive_Type"].ToString();
                                    string schName = dRow.IsNull("Sch_Name") ? String.Empty : dRow["Sch_Name"].ToString();
                                    string schEName = dRow.IsNull("Sch_EName") ? String.Empty : dRow["Sch_EName"].ToString();
                                    bool engEnabled = dRow.IsNull("Eng_Enabled") ? false : "Y".Equals(dRow["Eng_Enabled"].ToString());
                                    string text = null;
                                    if (isEngUI && engEnabled && !String.IsNullOrEmpty(schEName))
                                    {
                                        text = schEName;
                                    }
                                    else
                                    {
                                        text = schName;
                                    }
                                    items[idx] = new CodeText(code, text);
                                    if (selectedReceiveType == null && receiveType == code)
                                    {
                                        selectedReceiveType = code;
                                        selectedCFlag = dRow.IsNull("c_flag") ? String.Empty : dRow["c_flag"].ToString();
                                        selectedEngEnabled = engEnabled;
                                    }
                                    idx++;
                                }
                                data.ReceiveTypeDatas = items;
                            }
                            #endregion
                        }
                        else
                        {
                            selectedReceiveType = String.Empty;
                            selectedCFlag = string.Empty;
                            selectedEngEnabled = false;
                        }
                    }
                    else
                    {
                        data = null;

                        if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                        }
                        else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                        }
                    }

                    #region 新增日誌資料
                    if (dbLogger != null)
                    {
                        string notation = null;
                        if (result.IsSuccess)
                        {
                            notation = String.Format("[{0}] {1}商家代號選項資料成功", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT);
                        }
                        else
                        {
                            notation = String.Format("[{0}] {1}商家代號選項資料失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                        }
                        dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                    }
                    #endregion

                    if (result.IsSuccess)
                    {
                        if (f1Mode != "L" && String.IsNullOrEmpty(receiveType))
                        {
                            //Option Mode 且未指定 receiveType
                            if (receiveTypeDefaultMdoe == "1")
                            {
                                //預選第一個
                                selectedReceiveType = firstReceiveType;
                                selectedCFlag = firstCFlag;
                                selectedEngEnabled = firstEngEnabled;
                            }
                            else if (receiveTypeDefaultMdoe == "2")
                            {
                                //只有一個項目時，預選該項目
                                if (data.ReceiveTypeDatas != null && data.ReceiveTypeDatas.Length == 1)
                                {
                                    selectedReceiveType = firstReceiveType;
                                    selectedCFlag = firstCFlag;
                                    selectedEngEnabled = firstEngEnabled;
                                }
                            }
                        }
                        data.SelectedReceiveType = selectedReceiveType;
                        data.SelectedReceiveTypeCFlag = selectedCFlag;
                    }
                    else
                    {
                        #region 儲存日誌資料並釋放資料庫日誌記錄器
                        if (dbLogger != null)
                        {
                            string logErrmsg = this.ReleaseDBLogger(true);
                            dbLogger.Dispose();
                            dbLogger = null;
                        }
                        #endregion

                        return result;
                    }

                    //以要回傳的 SelectedReceiveType 為準
                    #region [MDY:20180610] 避開 Checkmarx 的 Heuristic SQL Injection 誤判
                    #region [Old]
                    //receiveType = data.SelectedReceiveType;
                    #endregion

                    receiveType = data.SelectedReceiveType.Replace("'", "''");
                    isEngUI &= selectedEngEnabled;
                    #endregion
                }
                #endregion

                #region 設定的預設的學年、學期
                string defaultYearID = null;
                string defaultTermID = null;
                if (!String.IsNullOrEmpty(receiveType))
                {
                    DataTable dt = null;
                    string sql = "SELECT TOP 1 [Year_Now], [Term_Now] FROM [Receive_Config] WHERE [Receive_Type] = @Receive_Type";
                    KeyValue[] parameters = new KeyValue[] { new KeyValue("@Receive_Type", receiveType) };

                    result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                    if (result.IsSuccess)
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow dRow = dt.Rows[0];
                            defaultYearID = dRow.IsNull("Year_Now") ? String.Empty : dRow["Year_Now"].ToString();
                            defaultTermID = dRow.IsNull("Term_Now") ? String.Empty : dRow["Term_Now"].ToString();
                        }
                    }
                    else
                    {
                        if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                        }
                        else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                        }
                    }

                    #region 新增日誌資料
                    if (dbLogger != null)
                    {
                        string notation = null;
                        if (result.IsSuccess)
                        {
                            notation = String.Format("[{0}] {1}預設的學年、學期資料成功 (Year_Now={2}, Term_Now={3})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, defaultYearID, defaultTermID);
                        }
                        else
                        {
                            notation = String.Format("[{0}] {1}預設的學年、學期資料失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                        }
                        dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                    }
                    #endregion
                }
                //未指定學年以資料庫設定的預設學年取代
                if (String.IsNullOrEmpty(yearID) && !String.IsNullOrEmpty(defaultYearID) && yearDefaultMdoe != "3")
                {
                    #region [MDY:20180610] 避開 Checkmarx 的 Heuristic SQL Injection 誤判
                    #region [Old]
                    //yearID = defaultYearID;
                    #endregion

                    yearID = defaultYearID.Replace("'", "''");
                    #endregion
                }
                //未指定學期以資料庫設定的預設學期取代
                if (String.IsNullOrEmpty(termID) && !String.IsNullOrEmpty(defaultTermID) && termDefaultMdoe != "3")
                {
                    #region [MDY:20180610] 避開 Checkmarx 的 Heuristic SQL Injection 誤判
                    #region [Old]
                    //termID = defaultTermID;
                    #endregion

                    termID = defaultTermID.Replace("'", "''");
                    #endregion
                }
                #endregion

                #region 取學年資料
                {
                    KeyValueList parameters = new KeyValueList(1);
                    string sql2 = null;
                    if (f1Mode == "L")
                    {
                        sql2 = String.Concat("WHERE [Year_Id] = @Year_Id");
                        parameters.Add("@Year_Id", yearID);
                    }
                    else
                    {
                        sql2 = "ORDER BY [Year_Id] DESC";
                    }

                    string sql = "SELECT [Year_Id], [Year_Name], [Year_EName] FROM [Year_List] " + sql2;

                    string firstYearID = null;
                    string selectedYearID = null;
                    DataTable dt = null;
                    result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                    if (result.IsSuccess)
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            #region 逐筆處理
                            CodeText[] items = new CodeText[dt.Rows.Count];
                            {
                                int idx = 0;
                                foreach (DataRow dRow in dt.Rows)
                                {
                                    string code = dRow.IsNull("Year_Id") ? String.Empty : dRow["Year_Id"].ToString();
                                    string yearName = dRow.IsNull("Year_Name") ? String.Empty : dRow["Year_Name"].ToString();
                                    string yearEName = dRow.IsNull("Year_EName") ? String.Empty : dRow["Year_EName"].ToString();
                                    string text = null;
                                    if (isEngUI && !String.IsNullOrEmpty(yearEName))
                                    {
                                        text = yearEName;
                                    }
                                    else
                                    {
                                        text = yearName;
                                    }
                                    items[idx] = new CodeText(code, text);
                                    if (selectedYearID == null && yearID == code)
                                    {
                                        selectedYearID = code;
                                    }
                                    idx++;
                                }
                                data.YearDatas = items;
                            }
                            #endregion

                            #region 第一筆
                            {
                                firstYearID = items[0].Code;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        data = null;

                        if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                        }
                        else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                        }
                    }

                    #region 新增日誌資料
                    if (dbLogger != null)
                    {
                        string notation = null;
                        if (result.IsSuccess)
                        {
                            notation = String.Format("[{0}] {1}學年選項資料成功", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT);
                        }
                        else
                        {
                            notation = String.Format("[{0}] {1}學年選項資料失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                        }
                        dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                    }
                    #endregion

                    if (result.IsSuccess)
                    {
                        if (f1Mode != "L" && String.IsNullOrEmpty(yearID))
                        {
                            //Option Mode 且未指定 yearID
                            if (yearDefaultMdoe == "1")
                            {
                                //預選第一個
                                selectedYearID = firstYearID;
                            }
                            else if (yearDefaultMdoe == "2")
                            {
                                //只有一個項目時，預選該項目
                                if (data.YearDatas != null && data.YearDatas.Length == 1)
                                {
                                    selectedYearID = firstYearID;
                                }
                            }
                        }
                        data.SelectedYearID = selectedYearID;
                    }
                    else
                    {
                        #region 儲存日誌資料並釋放資料庫日誌記錄器
                        if (dbLogger != null)
                        {
                            string logErrmsg = this.ReleaseDBLogger(true);
                            dbLogger.Dispose();
                            dbLogger = null;
                        }
                        #endregion

                        return result;
                    }

                    //以要回傳的 SelectedYearID 為準
                    #region [MDY:20180610] 避開 Checkmarx 的 Heuristic SQL Injection 誤判
                    #region [Old]
                    //yearID = data.SelectedYearID;
                    #endregion

                    yearID = data.SelectedYearID.Replace("'", "''");
                    #endregion
                }
                #endregion

                #region 取學期資料
                if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearID))
                {
                    KeyValueList parameters = new KeyValueList(3);
                    string sql2 = null;
                    if (f1Mode == "L")
                    {
                        sql2 = String.Concat("AND [Term_Id] = @Term_Id");
                        parameters.Add("@Term_Id", termID);
                    }
                    else
                    {
                        sql2 = "ORDER BY [Term_Id]";
                    }

                    string sql = "SELECT [Term_Id], [Term_Name], [Term_EName] FROM [Term_List] WHERE [Receive_Type] = @Receive_Type AND [Year_Id] = @Year_Id " + sql2;
                    parameters.Add("@Receive_Type", receiveType);
                    parameters.Add("@Year_Id", yearID);

                    string firstTermID = null;
                    string selectedTermID = null;
                    DataTable dt = null;
                    result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                    if (result.IsSuccess)
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            #region 逐筆處理
                            CodeText[] items = new CodeText[dt.Rows.Count];
                            {
                                int idx = 0;
                                foreach (DataRow dRow in dt.Rows)
                                {
                                    string code = dRow.IsNull("Term_Id") ? String.Empty : dRow["Term_Id"].ToString();
                                    string termName = dRow.IsNull("Term_Name") ? String.Empty : dRow["Term_Name"].ToString();
                                    string termEName = dRow.IsNull("Term_EName") ? String.Empty : dRow["Term_EName"].ToString();
                                    string text = null;
                                    if (isEngUI && !String.IsNullOrEmpty(termEName))
                                    {
                                        text = termEName;
                                    }
                                    else
                                    {
                                        text = termName;
                                    }
                                    items[idx] = new CodeText(code, text);
                                    if (selectedTermID == null &&termID == code)
                                    {
                                        selectedTermID = code;
                                    }
                                    idx++;
                                }
                                data.TermDatas = items;
                            }
                            #endregion

                            #region 第一筆
                            {
                                firstTermID = items[0].Code;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        data = null;

                        if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                        }
                        else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                        }
                    }

                    #region 新增日誌資料
                    if (dbLogger != null)
                    {
                        string notation = null;
                        if (result.IsSuccess)
                        {
                            notation = String.Format("[{0}] {1}學期選項資料成功", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT);
                        }
                        else
                        {
                            notation = String.Format("[{0}] {1}學期選項資料失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                        }
                        dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                    }
                    #endregion

                    if (result.IsSuccess)
                    {
                        if (f1Mode != "L" && String.IsNullOrEmpty(termID))
                        {
                            //Option Mode 且未指定 termID
                            if (termDefaultMdoe == "1")
                            {
                                //預選第一個
                                selectedTermID = firstTermID;
                            }
                            else if (termDefaultMdoe == "2")
                            {
                                //只有一個項目時，預選該項目
                                if (data.TermDatas != null && data.TermDatas.Length == 1)
                                {
                                    selectedTermID = firstTermID;
                                }
                            }
                        }
                        data.SelectedTermID = selectedTermID;
                    }
                    else
                    {
                        #region 儲存日誌資料並釋放資料庫日誌記錄器
                        if (dbLogger != null)
                        {
                            string logErrmsg = this.ReleaseDBLogger(true);
                            dbLogger.Dispose();
                            dbLogger = null;
                        }
                        #endregion

                        return result;
                    }

                    //以要回傳的 SelectedTermID 為準
                    #region [MDY:20180610] 避開 Checkmarx 的 Heuristic SQL Injection 誤判
                    #region [Old]
                    //termID = data.SelectedTermID;
                    #endregion

                    termID = data.SelectedTermID.Replace("'", "''");
                    #endregion
                }
                #endregion

                #region 取部別資料  //土銀的部別不是 Key
                data.SelectedDepID = " ";
                depID = data.SelectedDepID;

                #region [Old] 土銀不使用原有部別 Det_List，改用專用部別 Dept_List
                //if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearID) && !String.IsNullOrEmpty(termID))
                //{
                //    KeyValueList parameters = new KeyValueList(4);
                //    string sql2 = null;
                //    if (f2Mode == "L")
                //    {
                //        sql2 = String.Concat("AND [Dep_Id] = @Dep_Id");
                //        parameters.Add("@Dep_Id", depID);
                //    }
                //    else
                //    {
                //        sql2 = "ORDER BY [Dep_Id]";
                //    }

                //    string sql = "SELECT [Dep_Id], [Dep_Name] FROM [Dep_List] WHERE [Receive_Type] = @Receive_Type AND [Year_Id] = @Year_Id AND [Term_Id] = @Term_Id " + sql2;
                //    parameters.Add("@Receive_Type", receiveType);
                //    parameters.Add("@Year_Id", yearID);
                //    parameters.Add("@Term_Id", termID);

                //    string firstDepID = null;
                //    string selectedDepID = null;
                //    DataTable dt = null;
                //    result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                //    if (result.IsSuccess)
                //    {
                //        if (dt != null && dt.Rows.Count > 0)
                //        {
                //            #region 逐筆處理
                //            CodeText[] items = new CodeText[dt.Rows.Count];
                //            {
                //                int idx = 0;
                //                foreach (DataRow dRow in dt.Rows)
                //                {
                //                    string code = dRow.IsNull("Dep_Id") ? String.Empty : dRow["Dep_Id"].ToString();
                //                    string text = dRow.IsNull("Dep_Name") ? String.Empty : dRow["Dep_Name"].ToString();
                //                    items[idx] = new CodeText(code, text);
                //                    if (selectedDepID == null && depID == code)
                //                    {
                //                        selectedDepID = code;
                //                    }
                //                    idx++;
                //                }
                //                data.DepDatas = items;
                //            }
                //            #endregion

                //            #region 第一筆
                //            {
                //                firstDepID = items[0].Code;
                //            }
                //            #endregion
                //        }
                //    }
                //    else
                //    {
                //        data = null;

                //        if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                //        {
                //            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                //        }
                //        else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                //        {
                //            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                //        }
                //    }

                //    #region 新增日誌資料
                //    if (dbLogger != null)
                //    {
                //        string notation = null;
                //        if (result.IsSuccess)
                //        {
                //            notation = String.Format("[{0}] {1}部別選項資料成功", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT);
                //        }
                //        else
                //        {
                //            notation = String.Format("[{0}] {1}部別選項資料失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                //        }
                //        dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                //    }
                //    #endregion

                //    if (result.IsSuccess)
                //    {
                //        if (f2Mode != "L" && String.IsNullOrEmpty(depID))
                //        {
                //            //Option Mode 且未指定 depID
                //            if (depDefaultMdoe == "1")
                //            {
                //                //預選第一個
                //                selectedDepID = firstDepID;
                //            }
                //            else if (depDefaultMdoe == "2")
                //            {
                //                //只有一個項目時，預選該項目
                //                if (data.DepDatas != null && data.DepDatas.Length == 1)
                //                {
                //                    selectedDepID = firstDepID;
                //                }
                //            }
                //        }
                //        data.SelectedDepID = selectedDepID;
                //    }
                //    else
                //    {
                //        #region 儲存日誌資料並釋放資料庫日誌記錄器
                //        if (dbLogger != null)
                //        {
                //            string logErrmsg = this.ReleaseDBLogger(true);
                //            dbLogger.Dispose();
                //            dbLogger = null;
                //        }
                //        #endregion

                //        return result;
                //    }

                //    //以要回傳的 SelectedDepID 為準
                //    depID = data.SelectedDepID;
                //}
                #endregion
                #endregion

                #region 取代收費用別資料
                if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearID) && !String.IsNullOrEmpty(termID))
                {
                    KeyValueList parameters = new KeyValueList(5);
                    string sql2 = null;
                    if (f2Mode == "L")
                    {
                        sql2 = String.Concat("AND [Receive_Id] = @Receive_Id");
                        parameters.Add("@Receive_Id", receiveID);
                    }
                    else
                    {
                        sql2 = "ORDER BY [Receive_Id]";
                    }

                    string sql = "SELECT [Receive_Id], [Receive_Name], [Receive_EName] FROM [Receive_List] WHERE [Receive_Type] = @Receive_Type AND [Year_Id] = @Year_Id AND [Term_Id] = @Term_Id AND [Dep_Id] = @Dep_Id " + sql2;
                    parameters.Add("@Receive_Type", receiveType);
                    parameters.Add("@Year_Id", yearID);
                    parameters.Add("@Term_Id", termID);
                    parameters.Add("@Dep_Id", depID);

                    string firstReceiveID = null;
                    string selectedReceiveID = null;
                    DataTable dt = null;
                    result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                    if (result.IsSuccess)
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            #region 逐筆處理
                            CodeText[] items = new CodeText[dt.Rows.Count];
                            {
                                int idx = 0;
                                foreach (DataRow dRow in dt.Rows)
                                {
                                    string code = dRow.IsNull("Receive_Id") ? String.Empty : dRow["Receive_Id"].ToString();
                                    string receiveName = dRow.IsNull("Receive_Name") ? String.Empty : dRow["Receive_Name"].ToString();
                                    string receiveEName = dRow.IsNull("Receive_EName") ? String.Empty : dRow["Receive_EName"].ToString();
                                    string text = null;
                                    if (isEngUI && !String.IsNullOrEmpty(receiveEName))
                                    {
                                        text = receiveEName;
                                    }
                                    else
                                    {
                                        text = receiveName;
                                    }
                                    items[idx] = new CodeText(code, text);
                                    if (selectedReceiveID == null && receiveID == code)
                                    {
                                        selectedReceiveID = code;
                                    }
                                    idx++;
                                }
                                data.ReceiveDatas = items;
                            }
                            #endregion

                            #region 第一筆
                            {
                                firstReceiveID = items[0].Code;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        data = null;

                        if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                        }
                        else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                        {
                            result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                        }
                    }

                    #region 新增日誌資料
                    if (dbLogger != null)
                    {
                        string notation = null;
                        if (result.IsSuccess)
                        {
                            notation = String.Format("[{0}] {1}部別選項資料成功", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT);
                        }
                        else
                        {
                            notation = String.Format("[{0}] {1}部別選項資料失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                        }
                        dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                    }
                    #endregion

                    if (result.IsSuccess)
                    {
                        if (f2Mode != "L" && String.IsNullOrEmpty(receiveID))
                        {
                            //Option Mode 且未指定 receiveID
                            if (receiveDefaultMdoe == "1")
                            {
                                //預選第一個
                                selectedReceiveID = firstReceiveID;
                            }
                            else if (depDefaultMdoe == "2")
                            {
                                //只有一個項目時，預選該項目
                                if (data.ReceiveDatas != null && data.ReceiveDatas.Length == 1)
                                {
                                    selectedReceiveID = firstReceiveID;
                                }
                            }
                        }
                        data.SelectedReceiveID = selectedReceiveID;
                    }
                    else
                    {
                        #region 儲存日誌資料並釋放資料庫日誌記錄器
                        if (dbLogger != null)
                        {
                            string logErrmsg = this.ReleaseDBLogger(true);
                            dbLogger.Dispose();
                            dbLogger = null;
                        }
                        #endregion

                        return result;
                    }

                    //以要回傳的 SelectedReceiveID 為準
                    receiveID = data.SelectedReceiveID;
                }
                #endregion
                #endregion

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                return new Result(true);
            }
            #endregion
        }
        #endregion

        #region 取得資料選項陣列
        /// <summary>
        /// 取得資料選項陣列 (此方法僅適用 SQL 資料庫)
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <param name="commandAsker">傳回服務命令請求者資料物件。</param>
        /// <param name="datas">傳回查詢結果的 CodeText 陣列。</param>
        /// <returns>傳回處理結果。</returns>
        public virtual Result GetEntityOptions(IServiceCommand command, out CommandAsker commandAsker
            , out CodeText[] datas)
        {
            commandAsker = null;
            datas = null;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            EntityOptionsCommand myCommand = command is EntityOptionsCommand ? (EntityOptionsCommand)command : EntityOptionsCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            if (myCommand.Parameters == null || myCommand.Parameters.Count == 0)
            {
                return new Result("缺少資料處理服務命令參數集合參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", EntityOptionsCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ENTITY_TYPE_FULL_NAME
            Type entityType = null;
            if (!myCommand.GetEntityType(out entityType)
                || entityType == null)   //不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", EntityOptionsCommand.ENTITY_TYPE_FULL_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region WHERE
            Expression where = null;
            if (!myCommand.GetWhere(out where)
                || where == null || !where.IsReady())	//允許無查詢條件但不允許無此參數
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", EntityOptionsCommand.WHERE);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region ORDER_BYS
            KeyValueList<OrderByEnum> orderbys = null;
            if (!myCommand.GetOrderBys(out orderbys))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", EntityOptionsCommand.ORDER_BYS);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region CODE_FIELD_NAMES
            string[] codeFieldNames = null;
            if (!myCommand.GetCodeFieldNames(out codeFieldNames))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", EntityOptionsCommand.CODE_FIELD_NAMES);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region CODE_COMBINE_FORMAT
            string codeFormat = null;
            if (!myCommand.GetCodeCombineFormat(out codeFormat))
            {
                string errmsg = String.Format("無效的 {0} 命令參數", EntityOptionsCommand.CODE_COMBINE_FORMAT);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region TEXT_FIELD_NAMES
            string[] textFieldNames = null;
            if (!myCommand.GetTextFieldNames(out textFieldNames)
                || textFieldNames == null || textFieldNames.Length == 0)
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", EntityOptionsCommand.TEXT_FIELD_NAMES);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region TEXT_COMBINE_FORMAT
            string textFormat = null;
            if (!myCommand.GetTextCombineFormat(out textFormat)
                || (textFieldNames.Length > 1 && String.IsNullOrEmpty(textFormat)))
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", EntityOptionsCommand.TEXT_COMBINE_FORMAT);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion
            #endregion

            #region 組查詢的命令指令與命令參數
            string sql = null;
            KeyValueList parameters = null;
            {
                #region WHERE SQL
                string whereSql = null;
                IDataParameter[] whereParameters = null;
                if (!_Factory.GenWhereCommandTextParameters(where, out whereSql, out whereParameters))
                {
                    return new Result(false, "無法產生查詢條件指令", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
                whereSql = whereSql.Trim();
                if (!String.IsNullOrEmpty(whereSql) && !whereSql.StartsWith("WHERE", StringComparison.CurrentCultureIgnoreCase))
                {
                    whereSql = "WHERE " + whereSql;
                }

                //因為 _Factory.GetDataTable 無法傳入 IDataParameter[] '參數，所以要先轉成 KeyValueList
                if (whereParameters != null && whereParameters.Length > 0)
                {
                    parameters = new KeyValueList(whereParameters.Length);
                    foreach (IDataParameter whereParameter in whereParameters)
                    {
                        parameters.Add(whereParameter.ParameterName, whereParameter.Value);
                    }
                }
                #endregion

                #region ORDER BY SQL
                string orderbySql = null;
                if (orderbys != null && orderbys.Count > 0)
                {
                    string[] orderItems = new string[orderbys.Count];
                    for (int idx = 0; idx < orderbys.Count; idx++)
                    {
                        KeyValue<OrderByEnum> orderby = orderbys[idx];
                        orderItems[idx] = String.Format("[{0}] {1}", orderby.Key, orderby.Value.ToString());
                    }
                    orderbySql = String.Concat("ORDER BY ", String.Join(", ", orderItems));
                }
                #endregion

                #region SELECT SQL
                string selectSql = null;
                {
                    EntitySpec espec = EntityUtility.FindEntitySpec(entityType);
                    if (espec == null)
                    {
                        string errmsg = String.Format("無效的 {0} 命令參數，查無該型別", EntityOptionsCommand.ENTITY_TYPE_FULL_NAME);
                        return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
                    }

                    if (codeFieldNames == null || codeFieldNames.Length == 0)
                    {
                        FieldSpec[] fspecs = EntityUtility.FindPrimaryKeyFieldSpecs(entityType);
                        if (fspecs == null || fspecs.Length == 0)
                        {
                            string errmsg = String.Format("缺少 {0} 命令參數，因為 查詢的 Entity 無 Primary Key", EntityOptionsCommand.CODE_FIELD_NAMES);
                            return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
                        }
                        else
                        {
                            codeFieldNames = new string[fspecs.Length];
                            for(int idx = 0; idx < fspecs.Length; idx++)
                            {
                                FieldSpec fspec = fspecs[idx];
                                codeFieldNames[idx] = fspec.FieldName;
                            }
                        }
                    }

                    List<string> allFields = new List<string>(codeFieldNames.Length + textFieldNames.Length);
                    StringBuilder fields = new StringBuilder();
                    foreach (string fieldName in codeFieldNames)
                    {
                        if (!allFields.Contains(fieldName))
                        {
                            allFields.Add(fieldName);
                            fields.AppendFormat("[{0}], ", fieldName);
                        }
                    }

                    foreach (string fieldName in textFieldNames)
                    {
                        if (!allFields.Contains(fieldName))
                        {
                            allFields.Add(fieldName);
                            fields.AppendFormat("[{0}], ", fieldName);
                        }
                    }

                    if (espec.TableType == TableTypeEnum.ViewSql)
                    {
                        selectSql = String.Format("SELECT DISTINCT {0} FROM ({1})", fields.ToString(0, fields.Length - 2), espec.TableName);
                    }
                    else
                    {
                        selectSql = String.Format("SELECT DISTINCT {0} FROM [{1}]", fields.ToString(0, fields.Length - 2), espec.TableName);
                    }
                }
                #endregion

                sql = String.Format("{0} {1} {2}", selectSql, whereSql, orderbySql);
            }
            #endregion

            #region 取得初始化的資料庫日誌記錄器
            DBLogger dbLogger = this.InitialDBLogger(commandAsker);
            #endregion

            #region 查詢資料
            {
                DataTable dt = null;
                Result result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                if (result.IsSuccess)
                {
                    try
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            datas = new CodeText[dt.Rows.Count];
                            for (int idx = 0; idx < dt.Rows.Count; idx++)
                            {
                                DataRow row = dt.Rows[idx];

                                #region 組 code
                                string code = null;
                                {
                                    object[] codeFieldValues = new object[codeFieldNames.Length];
                                    for (int jdx = 0; jdx < codeFieldNames.Length; jdx++)
                                    {
                                        string fieldName = codeFieldNames[jdx];
                                        codeFieldValues[jdx] = row.IsNull(fieldName) ? String.Empty : row[fieldName];
                                    }
                                    if (codeFieldValues.Length > 1)
                                    {
                                        if (String.IsNullOrEmpty(codeFormat))
                                        {
                                            StringBuilder sb = new StringBuilder();
                                            foreach (object codeFieldValue in codeFieldValues)
                                            {
                                                sb.AppendFormat("{0},", codeFieldValue);
                                            }
                                            code = sb.ToString(0, sb.Length - 1);
                                        }
                                        else
                                        {
                                            try
                                            {
                                                code = String.Format(codeFormat, codeFieldValues);
                                            }
                                            catch (Exception)
                                            {
                                                string errmsg = String.Format("無效的 {0} 命令參數，因為格式化資料失敗", EntityOptionsCommand.CODE_COMBINE_FORMAT);
                                                result = new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        code = codeFieldValues[0].ToString();
                                    }
                                }
                                #endregion

                                #region 組 text
                                string text = null;
                                {
                                    object[] textFieldValues = new object[textFieldNames.Length];
                                    for (int jdx = 0; jdx < textFieldNames.Length; jdx++)
                                    {
                                        string fieldName = textFieldNames[jdx];
                                        textFieldValues[jdx] = row.IsNull(fieldName) ? String.Empty : row[fieldName];
                                    }
                                    if (textFieldValues.Length > 1)
                                    {
                                        text = String.Format(textFormat, textFieldValues);
                                    }
                                    else
                                    {
                                        text = textFieldValues[0].ToString();
                                    }
                                }
                                #endregion

                                datas[idx] = new CodeText(code, text);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        datas = null;
                        result = new Result(false, "處理代碼與文字欄位值時發生例外", CoreStatusCode.UNKNOWN_EXCEPTION, ex);
                    }
                }
                else
                {
                    if (result.Code == CoreStatusCode.UNKNOWN_EXCEPTION)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.E_SELECT_DATA_EXCEPTION, result.Exception);
                    }
                    else if (result.Code == CoreStatusCode.UNKNOWN_ERROR)
                    {
                        result = new Result(result.IsSuccess, result.Message, CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                    }
                }

                #region 儲存日誌資料並釋放資料庫日誌記錄器
                if (dbLogger != null)
                {
                    string notation = null;
                    if (result.IsSuccess)
                    {
                        notation = String.Format("[{0}] {1}資料成功 (共{2}筆)", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, (dt == null ? 0 : dt.Rows.Count));
                    }
                    else
                    {
                        notation = String.Format("[{0}] {1}資料失敗 (錯誤訊息：{2})", commandAsker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                    }
                    dbLogger.AppendLog(commandAsker.UserQual, commandAsker.UnitId, commandAsker.FuncId, LogTypeCodeTexts.SELECT, commandAsker.UserId, notation);
                    string logErrmsg = this.ReleaseDBLogger(true);
                    dbLogger.Dispose();
                    dbLogger = null;
                }
                #endregion

                //保證處理成功時，回傳資料一定是 CodeText[]
                if (result.IsSuccess && datas == null)
                {
                    datas = new CodeText[0];
                }

                return result;
            }
            #endregion
        }
        #endregion

        #region 呼叫後台服務方法
        public virtual Result CallMethod(IServiceCommand command, out CommandAsker commandAsker
            , out object data)
        {
            commandAsker = null;
            data = null;

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                return new Result(false, "缺少或無效的資料存取物件", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            CallMethodCommand myCommand = command is CallMethodCommand ? (CallMethodCommand)command : CallMethodCommand.Create(command);
            if (myCommand == null || !myCommand.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region 處理服務命令參數
            #region COMMAND_ASKER
            if (!myCommand.GetCommandAsker(out commandAsker)
                || commandAsker == null || !commandAsker.IsReady)    //不允許無此參數、或未登入
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", CallMethodCommand.COMMAND_ASKER);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region METHOD_NAME
            string methodName = null;
            if (!myCommand.GetMethodName(out methodName))
            {
                string errmsg = String.Format("缺少或無效的 {0} 命令參數", CallMethodCommand.METHOD_NAME);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #region METHOD_ARGUMENTS
            ICollection<KeyValue<string>> methodArguments = null;
            if (!myCommand.GetMethodArguments(out methodArguments))    //允許無此參數
            {
                string errmsg = String.Format("無效的 {0} 命令參數", CallMethodCommand.METHOD_ARGUMENTS);
                return new Result(errmsg, ServiceStatusCode.P_LOST_PARAMETER);
            }
            #endregion

            #endregion

            #region 呼叫方法
            {
                Result result = null;

                //[TODO] 提供新的方法時，要在這裡加一個該方法要執行的實際方法或程式碼
                switch (methodName)
                {
                    #region 複製代碼檔
                    case CallMethodName.CopyDep:
                        result = this.CopyDep(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyReceive:
                        result = this.CopyReceive(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyCollege:
                        result = this.CopyCollege(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyMajor:
                        result = this.CopyMajor(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyClass:
                        result = this.CopyClass(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyReduce:
                        result = this.CopyReduce(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyDorm:
                        result = this.CopyDorm(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyReturn:
                        result = this.CopyReturn(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyLoan:
                        result = this.CopyLoan(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyIdentify:
                        result = this.CopyIdentify(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    #region 複製標準檔
                    case CallMethodName.CopyGeneralStandard:
                        result = this.CopyGeneralStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyCredit2Standard:
                        result = this.CopyCredit2Standard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyCreditStandard:
                        result = this.CopyCreditStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyDormStandard:
                        result = this.CopyDormStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyCreditbStandard:
                        result = this.CopyCreditbStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyIdentifyStandard:
                        result = this.CopyIdentifyStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyClassStandard:
                        result = this.CopyClassStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyReduceStandard:
                        result = this.CopyReduceStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyLoanStandard:
                        result = this.CopyLoanStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyReturnStandard:
                        result = this.CopyReturnStandard(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    #region 刪除代碼檔
                    case CallMethodName.DelDep:
                        result = this.DelDep(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelReceive:
                        result = this.DelReceive(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelCollege:
                        result = this.DelCollege(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelMajor:
                        result = this.DelMajor(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelClass:
                        result = this.DelClass(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelReduce:
                        result = this.DelReduce(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelDorm:
                        result = this.DelDorm(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelReturn:
                        result = this.DelReturn(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelLoan:
                        result = this.DelLoan(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelIdentify:
                        result = this.DelIdentify(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    #region 刪除標準檔
                    case CallMethodName.DelGeneralStandard:
                        result = this.DelGeneralStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelCredit2Standard:
                        result = this.DelCredit2Standard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelCreditStandard:
                        result = this.DelCreditStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelDormStandard:
                        result = this.DelDormStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelCreditbStandard:
                        result = this.DelCreditbStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelIdentifyStandard:
                        result = this.DelIdentifyStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelClassStandard:
                        result = this.DelClassStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelReduceStandard:
                        result = this.DelReduceStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelLoanStandard:
                        result = this.DelLoanStandard(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DelReturnStandard:
                        result = this.DelReturnStandard(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    #region 複製 / 刪除 代碼檔與標準檔
                    case CallMethodName.CopyBaseData:
                        result = this.CopyBaseData(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DeleteBaseData:
                        result = this.DeleteBaseData(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    case CallMethodName.RenewGroupRight:
                        result = this.RenewGroupRight(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyMappingXlsData:
                        result = this.CopyMappingXlsData(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.CopyMappingTxtData:
                        result = this.CopyMappingTxtData(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.S530001GetEditData:
                        result = this.S530001GetEditData(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.S530001InsertData:
                        result = this.S530001InsertData(commandAsker, methodArguments);
                        break;
                    case CallMethodName.S530001UpdateData:
                        result = this.S530001UpdateData(commandAsker, methodArguments);
                        break;
                    case CallMethodName.S530001DeleteData:
                        result = this.S530001DeleteData(commandAsker, methodArguments);
                        break;

                    #region [Old] 取消放行
                    //case CallMethodName.S530001ApproveData:
                    //    result = this.S530001ApproveData(commandAsker, methodArguments, out data);
                    //    break;
                    #endregion

                    case CallMethodName.GetMyReceiveTypeCodeTexts:
                        result = this.GetMyReceiveTypeCodeTexts(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.DeleteAllClassStandard:
                        result = this.DeleteAllClassStandard(commandAsker, methodArguments, out data);
                        break;

                    #region [MDY:20220530] Checkmarx 調整
                    case CallMethodName.ChangeUserPXX:
                        result = this.ChangeUserPXX(commandAsker, methodArguments);
                        break;
                    #endregion

                    case CallMethodName.InsertJobCubeForD15:
                        result = this.InsertJobCubeForD15(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.InsertJobCubeForPDF:
                        result = this.InsertJobCubeForPDF(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.InsertJobCubeForPDFByAsync:
                        result = this.InsertJobCubeForPDFByAsync(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.InsertJobCubeForD38:
                        result = this.InsertJobCubeForD38(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ExecB2100002Request:
                        result = this.ExecB2100002Request(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.GetB2100002Summary:
                        result = this.GetB2100002Summary(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.GetC3700006Summary:
                        result = this.GetC3700006Summary(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.InsertB2300003Data:
                        result = this.InsertB2300003Data(commandAsker, methodArguments);
                        break;

                    case CallMethodName.GetC3400001Result:
                        result = this.GetC3400001Result(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ExportB2100005File:
                        result = this.ExportB2100005File(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ImportB2100007File:
                        result = this.ImportB2100007File(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ExportB2100003File:
                        result = this.ExportB2100003File(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ExportC3700007File:
                        result = this.ExportC3700007File(commandAsker, methodArguments, out data);
                        break;


                    case CallMethodName.GenExportFileData:
                        result = this.GenExportFileData(commandAsker, methodArguments, out data);
                        break;


                    case CallMethodName.ExportReportA:
                        result = this.ExportReportA(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ExportReportA2:
                        result = this.ExportReportA2(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ExportReportB:
                        result = this.ExportReportB(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ExportReportC:
                        result = this.ExportReportC(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ExportReportD:
                        result = this.ExportReportD(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ExportReportE:
                        result = this.ExportReportE(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ExportQueryResult:
                        result = this.ExportQueryResult(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.GetBankPMTempFile:
                        result = this.GetBankPMTempFile(commandAsker, methodArguments, out data);
                        break;

                    #region [Old] 20150605 取消前擋後，改回後踢前
                    //case CallMethodName.ForcedLogoutUser:
                    //    result = this.ForcedLogoutUser(commandAsker, methodArguments);
                    //    break;
                    #endregion

                    #region [Old] 土銀沒有
                    //case CallMethodName.CallN162Request:
                    //    result = this.CallN162Request(commandAsker, methodArguments);
                    //    break;
                    #endregion

                    case CallMethodName.CallD00I71Request:
                        result = this.CallD00I71Request(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.UpdateCancelDatas:
                        result = this.UpdateCancelDatas(commandAsker, methodArguments, out data);
                        break;

                    #region [MDY:20160125] 整批刪除學生資料
                    case CallMethodName.DeleteStudentReceiveByPKeys:  //整批刪除學生資料 (勾選的資料)
                        result = this.DeleteStudentReceiveByPKeys(commandAsker, methodArguments, out data);
                        break;
                    case CallMethodName.DeleteStudentReceiveByWhere:  //整批刪除學生資料 (條件的資料)
                        result = this.DeleteStudentReceiveByWhere(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    case CallMethodName.DeleteProblemListDatas:
                        result = this.DeleteProblemListDatas(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.UpdateStudentReturnDatas:
                        result = this.UpdateStudentReturnDatas(commandAsker, methodArguments, out data);
                        break;


                    case CallMethodName.InsertTermListDatas:
                        result = this.InsertTermListDatas(commandAsker, methodArguments, out data);
                        break;


                    case CallMethodName.ClearCancelNo:
                        result = this.ClearCancelNo(commandAsker, methodArguments, out data);
                        break;


                    case CallMethodName.ImportQueueCTCBFile:
                        #region [MDY:20210301] 改執行 ImportQueueCTCBFileForDebug()
                        #region [OLD]
                        //result = this.ImportQueueCTCBFile(commandAsker, methodArguments, out data);
                        #endregion

                        result = this.ImportQueueCTCBFileForDebug(commandAsker, methodArguments, out data);
                        #endregion
                        break;

                    case CallMethodName.ProcessFlowData:
                        result = this.ProcessFlowData(commandAsker, methodArguments);
                        break;


                    case CallMethodName.GetC3700009AllOptions:
                        result = this.GetC3700009AllOptions(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.ImportD00I70File:
                        result = this.ImportD00I70File(commandAsker, methodArguments, out data);
                        break;

                    #region [MDY:20160321] 維護就貸資料
                    case CallMethodName.EditStudentLoanData:
                        result = this.EditStudentLoanData(commandAsker, methodArguments, out data);
                        break;
                    #endregion


                    #region [MDY:2018xxxx] 取得指定代碼資料表的資料選項陣列
                    case CallMethodName.GetCodeTableAllOptions:
                        result = this.GetCodeTableAllOptions(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    #region [MDY:2018xxxx] 取得指定代收費用檔、合計項目設定、學生基本資料、學生繳費單的資料
                    case CallMethodName.GetStudentBillDatas:
                        result = this.GetStudentBillDatas(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    #region [MDY:2018xxxx] 更新學生基本資料、學生繳費單的資料
                    case CallMethodName.UpdateStudentBillDatas:
                        result = this.UpdateStudentBillDatas(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    #region [MDY:20190226] 匯出異業代收款檔資料檔
                    case CallMethodName.ExportEDPData:
                        result = this.ExportEDPData(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    #region [MDY:20191014] M201910_01 (2019擴充案+小修正) 產生測試繳款單
                    case CallMethodName.GenSMBarcodePDF:
                        result = this.GenSMBarcodePDF(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    #region [MDY:202203XX] 2022擴充案 取得商家代號是否啟用英文資料
                    case CallMethodName.GetReceiveTypeEngEabled:
                        result = this.GetReceiveTypeEngEabled(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    #region [MDY:202203XX] 2022擴充案 學生專區取得繳費單相關
                    case CallMethodName.GetStudentReceiveViews:
                        result = this.GetStudentReceiveViews(commandAsker, methodArguments, out data);
                        break;

                    case CallMethodName.GetStudentReceiveView:
                        result = this.GetStudentReceiveView(commandAsker, methodArguments, out data);
                        break;
                    #endregion

                    default:
                        result = new Result(false, "未實作此方法", CoreStatusCode.S_NO_SUPPORT, null);
                        break;
                }

                return result;
            }
            #endregion
        }
        #endregion
    }
}
