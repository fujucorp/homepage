using System;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

namespace Entities
{
    /// <summary>
    /// 查詢首筆資料命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class SelectFirstCommand : ServiceCommand<SelectFirstCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "SELECT_FIRST";

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
        #endregion

        #region Constructor
        /// <summary>
        /// 建構查詢首筆資料命令類別物件
        /// </summary>
        public SelectFirstCommand()
            : base(COMMAND_NAME)
        {
        }

        /// <summary>
        /// 建構查詢首筆資料命令類別物件，並指定 commandAsker、entityType、where、orderbys
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="entityType">指定 Entity 型別。</param>
        /// <param name="where">指定查詢條件。</param>
        /// <param name="orderbys">指定資料排序。</param>
        protected SelectFirstCommand(CommandAsker commandAsker, Type entityType, Expression where, KeyValueList<OrderByEnum> orderbys)
            : base(COMMAND_NAME)
        {
            base.Parameters = new ServiceParameterList(4);
            base.Parameters.Add(COMMAND_ASKER, commandAsker);
            base.Parameters.Add(ENTITY_TYPE_FULL_NAME, entityType == null ? null : entityType.FullName);
            base.Parameters.Add(WHERE, where);
            base.Parameters.Add(ORDER_BYS, orderbys);
        }

        /// <summary>
        /// 建構查詢首筆資料命令類別物件，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected SelectFirstCommand(ServiceParameterList parameters)
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
        #endregion

        #region Static Method
        /// <summary>
        /// 建立查詢首筆資料命令類別物件
        /// </summary>
        /// <typeparam name="T">指定 Entity 型別。</typeparam>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="where">指定查詢條件。</param>
        /// <param name="orderbys">指定資料排序。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static SelectFirstCommand Create<T>(CommandAsker commandAsker, Expression where, KeyValueList<OrderByEnum> orderbys) where T : class, IEntity
        {
            SelectFirstCommand command = new SelectFirstCommand(commandAsker, typeof(T), where, orderbys);
            return command;
        }

        /// <summary>
        /// 建立查詢首筆資料命令類別物件
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static SelectFirstCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == SelectFirstCommand.COMMAND_NAME)
            {
                return new SelectFirstCommand(command.Parameters);
            }
            return null;
        }
        #endregion

        #region [TMP] 好像用不到，先 Mark
        ///// <summary>
        ///// 建立查詢首筆資料命令類別物件
        ///// </summary>
        ///// <param name="commandAsker">指定命令請求者資訊。</param>
        ///// <param name="entityType">指定 Entity 型別。</param>
        ///// <param name="where">指定查詢條件。</param>
        ///// <param name="orderbys">指定資料排序。</param>
        ///// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        //public static SelectFirstCommand Create(CommandAsker commandAsker, Type entityType, Expression where, KeyValueList<OrderByEnum> orderbys)
        //{
        //    SelectFirstCommand command = new SelectFirstCommand(commandAsker, entityType, where, orderbys);
        //    return command;
        //}

        ///// <summary>
        ///// 建立查詢首筆資料命令類別物件
        ///// </summary>
        ///// <param name="commandAsker">指定命令請求者資訊。</param>
        ///// <param name="entityTypeFullName">指定 Entity 型別全名。</param>
        ///// <param name="where">指定查詢條件。</param>
        ///// <param name="orderbys">指定資料排序。</param>
        ///// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        //public static SelectFirstCommand Create(CommandAsker commandAsker, string entityTypeFullName, Expression where, KeyValueList<OrderByEnum> orderbys)
        //{
        //    ServiceParameterList parameters = new ServiceParameterList(4);
        //    parameters.Add(COMMAND_ASKER, commandAsker);
        //    parameters.Add(ENTITY_TYPE_FULL_NAME, entityTypeFullName);
        //    parameters.Add(WHERE, where);
        //    parameters.Add(ORDER_BYS, orderbys);
        //    SelectFirstCommand command = new SelectFirstCommand(parameters);
        //    return command;
        //}
        #endregion
    }
}
