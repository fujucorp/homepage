using System;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

namespace Entities
{
    /// <summary>
    /// 使用者登出命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class UserLogoutCommand : ServiceCommand<UserLogoutCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "USER_LOGOUT";

        /// <summary>
        /// 命令請求者資訊的參數名稱定義
        /// </summary>
        public static readonly string COMMAND_ASKER = "COMMAND_ASKER";
        /// <summary>
        /// 使用者登出序號的參數名稱定義
        /// </summary>
        public static readonly string LOGON_SN = "LOGON_SN";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構使用者登出命令類別
        /// </summary>
        public UserLogoutCommand()
            : base(COMMAND_NAME)
        {
        }

        /// <summary>
        /// 建構使用者登出命令類別，並指定 commandAsker、logonSN
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="logonSN">指定使用者登出序號。</param>
        protected UserLogoutCommand(CommandAsker commandAsker, string logonSN)
            : base(COMMAND_NAME)
        {
            base.Parameters = new ServiceParameterList(3);
            base.Parameters.Add(COMMAND_ASKER, commandAsker);
            base.Parameters.Add(LOGON_SN, logonSN);
        }

        /// <summary>
        /// 建構使用者登出命令類別，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected UserLogoutCommand(ServiceParameterList parameters)
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
                && (base.Parameters.GetKeyFirstIndex(LOGON_SN) > -1))
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
        /// 取得使用者的登入密碼
        /// </summary>
        /// <param name="logonSN">傳回登入序號。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetLogonSN(out string logonSN)
        {
            logonSN = null;
            ServiceParameter parameter = base.Parameters[LOGON_SN];
            if (parameter != null)
            {
                string val = null;
                if (parameter.TryGetData<string>(out val))
                {
                    logonSN = val;
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
        /// 建立使用者登出命令類別物件
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="logonSN">指定使用者的登入序號。</param>
        /// <returns>成功則傳回使用者登出命令類別物件，否則傳回 null。</returns>
        public static UserLogoutCommand Create(CommandAsker commandAsker, string logonSN)
        {
            if (logonSN != null)
            {
                logonSN = logonSN.Trim();
            }

            commandAsker.FuncId = "LOGOUT";
            commandAsker.FuncName = "使用者登出";

            UserLogoutCommand command = new UserLogoutCommand(commandAsker, logonSN);
            return command;
        }

        /// <summary>
        /// 建立建構使用者登出命令類別
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回使用者登出命令類別物件，否則傳回 null。</returns>
        public static UserLogoutCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == UserLogoutCommand.COMMAND_NAME)
            {
                return new UserLogoutCommand(command.Parameters);
            }
            return null;
        }
        #endregion
    }
}
