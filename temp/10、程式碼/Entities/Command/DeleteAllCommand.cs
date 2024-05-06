﻿using System;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Log;
using Fuju.Web.Services;

namespace Entities
{
    /// <summary>
    /// 刪除多筆資料命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class DeleteAllCommand : ServiceCommand<DeleteAllCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "DELETE_ALL";

        /// <summary>
        /// 命令請求者資訊的參數名稱定義
        /// </summary>
        public static readonly string COMMAND_ASKER = "COMMAND_ASKER";
        /// <summary>
        /// Entity 型別全名的參數名稱定義
        /// </summary>
        public static readonly string ENTITY_TYPE_FULL_NAME = "ENTITY_TYPE_FULL_NAME";
        /// <summary>
        /// 刪除資料物件的參數名稱定義
        /// </summary>
        public static readonly string INSTANCES = "INSTANCES";
        /// <summary>
        /// 是否批次處理的參數名稱定義
        /// </summary>
        public static readonly string IS_BATCH = "IS_BATCH";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構刪除多筆資料命令類別物件
        /// </summary>
        public DeleteAllCommand()
        {
            base.CommandName = COMMAND_NAME;
        }

        /// <summary>
        /// 建構刪除多筆資料命令類別物件，並指定 commandAsker、entityType、instance
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="entityType">指定 Entity 型別。</param>
        /// <param name="instance">指定刪除多筆資料物件。</param>
        protected DeleteAllCommand(CommandAsker commandAsker, Type entityType, IEntity[] instances, bool isBatch)
        {
            base.CommandName = COMMAND_NAME;
            base.Parameters = new ServiceParameterList(3);
            base.Parameters.Add(COMMAND_ASKER, commandAsker);
            base.Parameters.Add(ENTITY_TYPE_FULL_NAME, entityType == null ? null : entityType.FullName);
            base.Parameters.Add(INSTANCES, instances);
            base.Parameters.Add(IS_BATCH, isBatch);
        }

        /// <summary>
        /// 建構刪除多筆資料命令類別物件，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected DeleteAllCommand(ServiceParameterList parameters)
            : base(COMMAND_NAME, parameters)
        {
        }
        #endregion

        #region Override ServiceCommand's Property
        /// <summary>
        /// 命令名稱 (唯讀)
        /// </summary>
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
                && (base.Parameters.GetKeyFirstIndex(ENTITY_TYPE_FULL_NAME) > -1)
                && (base.Parameters.GetKeyFirstIndex(INSTANCES) > -1))
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
        /// 取得刪除多筆資料物件
        /// </summary>
        /// <param name="instance">傳回刪除資料物件。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetInstances(out IEntity[] instances)
        {
            instances = null;
            ServiceParameter parameter = base.Parameters[INSTANCES];
            if (parameter != null)
            {
                IEntity[] data = null;
                if (parameter.TryGetData<IEntity[]>(out data))
                {
                    instances = data;
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
        /// 建立刪除多筆資料命令類別物件
        /// </summary>
        /// <typeparam name="T">指定 Entity 型別。</typeparam>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="instance">指定刪除資料物件。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static DeleteAllCommand Create<T>(CommandAsker commandAsker, T[] instances, bool isBatch) where T : class, IEntity
        {
            DeleteAllCommand command = new DeleteAllCommand(commandAsker, typeof(T), instances, isBatch);
            return command;
        }

        /// <summary>
        /// 建立刪除多筆資料命令類別物件
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="instance">指定刪除資料物件。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static DeleteAllCommand Create(CommandAsker commandAsker, IEntity[] instances, bool isBatch)
        {
            if (instances == null || instances.Length == 0)
            {
                return null;
            }
            DeleteAllCommand command = new DeleteAllCommand(commandAsker, instances[0].GetType(), instances, isBatch);
            return command;
        }

        /// <summary>
        /// 建立刪除多筆資料命令類別物件
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="entityTypeFullName">指定 Entity 型別全名。</param>
        /// <param name="instance">指定刪除資料物件。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static DeleteAllCommand Create(CommandAsker commandAsker, string entityTypeFullName, IEntity[] instances, bool isBatch)
        {
            ServiceParameterList parameters = new ServiceParameterList(4);
            parameters.Add(COMMAND_ASKER, commandAsker);
            parameters.Add(ENTITY_TYPE_FULL_NAME, entityTypeFullName);
            parameters.Add(INSTANCES, instances);
            parameters.Add(IS_BATCH, isBatch);
            DeleteAllCommand command = new DeleteAllCommand(parameters);
            return command;
        }

        /// <summary>
        /// 建立刪除多筆資料命令類別物件
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static DeleteAllCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == DeleteAllCommand.COMMAND_NAME)
            {
                return new DeleteAllCommand(command.Parameters);
            }
            return null;
        }
        #endregion
    }
}
