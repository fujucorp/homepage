using System;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

namespace Entities
{
    /// <summary>
    /// 查詢資料命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class SelectCommand : ServiceCommand<SelectCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "SELECT";

        /// <summary>
        /// 命令請求者資訊的參數名稱定義
        /// </summary>
        public static readonly string COMMAND_ASKER = "COMMAND_ASKER";
        /// <summary>
        /// Entity 型別全名的參數名稱定義
        /// </summary>
        public static readonly string ENTITY_TYPE_FULL_NAME = "ENTITY_TYPE_FULL_NAME";
        /// <summary>
        /// 查詢條件的參數名稱定義
        /// </summary>
        public static readonly string WHERE = "WHERE";
        /// <summary>
        /// 資料排序的參數名稱定義
        /// </summary>
        public static readonly string ORDER_BYS = "ORDER_BYS";
        /// <summary>
        /// 讀取查詢結果起始索引的參數名稱定義
        /// </summary>
        public static readonly string START_INDEX = "START_INDEX";
        /// <summary>
        /// 讀取查詢結果最大筆數的參數名稱定義
        /// </summary>
        public static readonly string MAX_RECORDS = "MAX_RECORDS";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構查詢資料命令類別物件
        /// </summary>
        public SelectCommand()
            : base(COMMAND_NAME)
        {
        }

        /// <summary>
        /// 建構查詢資料命令類別物件，並指定 commandAsker、entityType、where、orderbys、startIndex、maxRecords
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="entityType">指定 Entity 型別。</param>
        /// <param name="where">指定查詢條件。</param>
        /// <param name="orderbys">指定資料排序。</param>
        /// <param name="startIndex">指定讀取查詢結果的起始索引。</param>
        /// <param name="maxRecords">指定讀取查詢結果的最大筆數。</param>
        protected SelectCommand(CommandAsker commandAsker, Type entityType, Expression where, KeyValueList<OrderByEnum> orderbys, int startIndex, int maxRecords)
            : base(COMMAND_NAME)
        {
            base.Parameters = new ServiceParameterList(6);
            base.Parameters.Add(COMMAND_ASKER, commandAsker);
            base.Parameters.Add(ENTITY_TYPE_FULL_NAME, entityType == null ? null : entityType.FullName);
            base.Parameters.Add(WHERE, where);
            base.Parameters.Add(ORDER_BYS, orderbys);
            base.Parameters.Add(START_INDEX, startIndex);
            base.Parameters.Add(MAX_RECORDS, maxRecords);
        }

        /// <summary>
        /// 建構查詢資料命令類別物件，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected SelectCommand(ServiceParameterList parameters)
            : base(COMMAND_NAME, parameters)
        {
        }
        #endregion

        #region Override ServiceCommand's Property
        /// <summary>
        /// 命令名稱 (唯讀)
        /// </summary>
        /// <remarks>override 此屬性是為了保證命令名稱不會被改變</remarks>
        [XmlIgnore]
        public override string CommandName
        {
            get
            {
                return COMMAND_NAME;
            }
        }
        #endregion

        #region Override ServiceCommand's Method
        /// <summary>
        /// 取得此命令是否準備好 (僅檢查必要參數是否存在)
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false。</returns>
        public override bool IsReady()
        {
            if (base.IsReady()
                && (base.Parameters.GetKeyFirstIndex(COMMAND_ASKER) > -1)
                && (base.Parameters.GetKeyFirstIndex(ENTITY_TYPE_FULL_NAME) > -1)
                && (base.Parameters.GetKeyFirstIndex(WHERE) > -1))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得命令請求者資訊
        /// </summary>
        /// <param name="commandAsker">傳回命令請求者資訊。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetCommandAsker(out CommandAsker commandAsker)
        {
            commandAsker = null;
            ServiceParameter parameter = base.Parameters[COMMAND_ASKER];
            if (parameter != null)
            {
                CommandAsker data = null;
                if (parameter.TryGetData<CommandAsker>(out data))
                {
                    commandAsker = data;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得 Entity 型別
        /// </summary>
        /// <param name="entityType">傳回 Entity 型別。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetEntityType(out Type entityType)
        {
            entityType = null;
            ServiceParameter parameter = base.Parameters[ENTITY_TYPE_FULL_NAME];
            if (parameter != null)
            {
                string typeFullName = null;
                if (parameter.TryGetData<string>(out typeFullName) && !String.IsNullOrEmpty(typeFullName))
                {
                    Result result = null;
                    Type data = Reflector.TryGetType(typeFullName, out result);
                    if (result.IsSuccess)
                    {
                        entityType = data;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得查詢條件
        /// </summary>
        /// <param name="where">傳回查詢條件。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetWhere(out Expression where)
        {
            where = null;
            ServiceParameter parameter = base.Parameters[WHERE];
            if (parameter != null)
            {
                Expression data = null;
                if (parameter.TryGetData<Expression>(out data))
                {
                    where = data;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得資料排序
        /// </summary>
        /// <param name="orderbys">傳回資料排序。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetOrderBys(out KeyValueList<OrderByEnum> orderbys)
        {
            orderbys = null;
            ServiceParameter parameter = base.Parameters[ORDER_BYS];
            if (parameter != null)
            {
                KeyValueList<OrderByEnum> data = null;
                if (parameter.TryGetData<KeyValueList<OrderByEnum>>(out data))
                {
                    orderbys = data;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得讀取查詢結果的起始索引
        /// </summary>
        /// <param name="startIndex">傳回讀取查詢結果的起始索引。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetStartIndex(out int? startIndex)
        {
            startIndex = null;
            ServiceParameter parameter = base.Parameters[START_INDEX];
            if (parameter != null)
            {
                int data = 0;
                if (parameter.TryGetData<int>(out data))
                {
                    startIndex = data;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得讀取查詢結果的最大筆數
        /// </summary>
        /// <param name="maxRecords">傳回讀取查詢結果的最大筆數。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetMaxRecords(out int? maxRecords)
        {
            maxRecords = null;
            ServiceParameter parameter = base.Parameters[MAX_RECORDS];
            if (parameter != null)
            {
                int data = 0;
                if (parameter.TryGetData<int>(out data))
                {
                    maxRecords = data;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 建立查詢資料命令類別物件
        /// </summary>
        /// <typeparam name="T">指定 Entity 型別。</typeparam>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="where">指定查詢條件。</param>
        /// <param name="orderbys">指定資料排序。</param>
        /// <param name="startIndex">指定讀取查詢結果的起始索引。</param>
        /// <param name="maxRecords">指定讀取查詢結果的最大筆數。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static SelectCommand Create<T>(CommandAsker commandAsker, Expression where, KeyValueList<OrderByEnum> orderbys, int startIndex, int maxRecords) where T : class, IEntity
        {
            SelectCommand command = new SelectCommand(commandAsker, typeof(T), where, orderbys, startIndex, maxRecords);
            return command;
        }

        /// <summary>
        /// 建立查詢資料命令類別物件
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static SelectCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == SelectCommand.COMMAND_NAME)
            {
                return new SelectCommand(command.Parameters);
            }
            return null;
        }
        #endregion

        #region [TMP] 好像用不到，先 Mark
        ///// <summary>
        ///// 建立查詢資料命令類別物件
        ///// </summary>
        ///// <param name="commandAsker">指定命令請求者資訊。</param>
        ///// <param name="entityType">指定 Entity 型別。</param>
        ///// <param name="where">指定查詢條件。</param>
        ///// <param name="orderbys">指定資料排序。</param>
        ///// <param name="startIndex">指定讀取查詢結果的起始索引。</param>
        ///// <param name="maxRecords">指定讀取查詢結果的最大筆數。</param>
        ///// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        //public static SelectCommand Create(CommandAsker commandAsker, Type entityType, Expression where, KeyValueList<OrderByEnum> orderbys, int startIndex, int maxRecords)
        //{
        //    SelectCommand command = new SelectCommand(commandAsker, entityType, where, orderbys, startIndex, maxRecords);
        //    return command;
        //}

        ///// <summary>
        ///// 建立查詢資料命令類別物件
        ///// </summary>
        ///// <param name="commandAsker">指定命令請求者資訊。</param>
        ///// <param name="entityTypeFullName">指定 Entity 型別全名。</param>
        ///// <param name="where">指定查詢條件。</param>
        ///// <param name="orderbys">指定資料排序。</param>
        ///// <param name="startIndex">指定讀取查詢結果的起始索引。</param>
        ///// <param name="maxRecords">指定讀取查詢結果的最大筆數。</param>
        ///// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        //public static SelectCommand Create(CommandAsker commandAsker, string entityTypeFullName, Expression where, KeyValueList<OrderByEnum> orderbys, int startIndex, int maxRecords)
        //{
        //    ServiceParameterList parameters = new ServiceParameterList(6);
        //    parameters.Add(COMMAND_ASKER, commandAsker);
        //    parameters.Add(ENTITY_TYPE_FULL_NAME, entityTypeFullName);
        //    parameters.Add(WHERE, where);
        //    parameters.Add(ORDER_BYS, orderbys);
        //    parameters.Add(START_INDEX, startIndex);
        //    parameters.Add(MAX_RECORDS, maxRecords);
        //    SelectCommand command = new SelectCommand(parameters);
        //    return command;
        //}
        #endregion
    }
}
