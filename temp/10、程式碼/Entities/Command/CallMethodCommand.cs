using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

namespace Entities
{
    /// <summary>
    /// 呼叫後台服務方法命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class CallMethodCommand : ServiceCommand<CallMethodCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "CALL_METHOD";

        /// <summary>
        /// 命令請求者資訊的參數名稱定義
        /// </summary>
        public static readonly string COMMAND_ASKER = "COMMAND_ASKER";
        /// <summary>
        /// 要呼叫的方法名稱的參數名稱定義
        /// </summary>
        public static readonly string METHOD_NAME = "METHOD_NAME";
        /// <summary>
        /// 要傳給呼叫方法的參數集合的參數名稱定義
        /// </summary>
        public static readonly string METHOD_ARGUMENTS = "METHOD_ARGUMENTS";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構呼叫後台服務方法命令類別
        /// </summary>
        public CallMethodCommand()
            : base(COMMAND_NAME)
        {
        }

        /// <summary>
        /// 建構呼叫後台服務方法命令類別，並指定 commandAsker、methodName、methodArguments
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="methodName">要呼叫的方法名稱。</param>
        /// <param name="methodArguments">要傳給呼叫方法的參數集合。</param>
        protected CallMethodCommand(CommandAsker commandAsker, string methodName, ICollection<KeyValue<string>> methodArguments)
            : base(COMMAND_NAME)
        {
            base.Parameters = new ServiceParameterList(3);
            base.Parameters.Add(COMMAND_ASKER, commandAsker);
            base.Parameters.Add(METHOD_NAME, methodName);
            base.Parameters.Add(METHOD_ARGUMENTS, methodArguments);
        }

        /// <summary>
        /// 建構呼叫後台服務方法命令類別，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected CallMethodCommand(ServiceParameterList parameters)
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
                && (base.Parameters.GetKeyFirstIndex(METHOD_NAME) > -1))
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
        /// <param name="methodName">傳回要呼叫的方法名稱。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetMethodName(out string methodName)
        {
            methodName = null;
            ServiceParameter parameter = base.Parameters[METHOD_NAME];
            if (parameter != null)
            {
                string name = null;
                if (parameter.TryGetData<string>(out name))
                {
                    methodName = name;
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
        /// <param name="methodArguments">傳回要傳給呼叫方法的參數集合。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetMethodArguments(out ICollection<KeyValue<string>> methodArguments)
        {
            methodArguments = null;
            ServiceParameter parameter = base.Parameters[METHOD_ARGUMENTS];
            if (parameter != null)
            {
                object data = null;
                if (parameter.TryGetData(out data))
                {
                    if (data != null)
                    {
                        if (data is KeyValue<string>[])
                        {
                            methodArguments = data as KeyValue<string>[];
                        }
                        else if (data is KeyValueList<string>)
                        {
                            methodArguments = data as KeyValueList<string>;
                        }
                        else if (data is ICollection<KeyValue<string>>)
                        {
                            methodArguments = data as ICollection<KeyValue<string>>;
                        }
                        else
                        {
                            return false;
                        }
                    }
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
        /// 建立呼叫後台服務方法命令類別物件
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="methodName">要呼叫的方法名稱。</param>
        /// <param name="methodArguments">要傳給呼叫方法的參數集合。</param>
        /// <returns>成功則傳回呼叫後台服務方法命令類別物件，否則傳回 null。</returns>
        public static CallMethodCommand Create(CommandAsker commandAsker, string methodName, ICollection<KeyValue<string>> methodArguments)
        {
            CallMethodCommand command = new CallMethodCommand(commandAsker, methodName, methodArguments);
            return command;
        }

        /// <summary>
        /// 建立呼叫後台服務方法命令類別物件
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static CallMethodCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == CallMethodCommand.COMMAND_NAME)
            {
                return new CallMethodCommand(command.Parameters);
            }
            return null;
        }
        #endregion
    }
}
