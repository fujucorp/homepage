using System;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

namespace Entities
{
    /// <summary>
    /// 檢查登入與功能狀態命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class CheckLogonCommand : ServiceCommand<CheckLogonCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "CHECK_LOGON";

        /// <summary>
        /// 命令請求者資訊的參數名稱定義
        /// </summary>
        public static readonly string COMMAND_ASKER = "COMMAND_ASKER";
        /// <summary>
        /// 登入序號的參數名稱定義
        /// </summary>
        public static readonly string LOGON_SN = "LOGON_SN";
        /// <summary>
        /// 要檢查功能狀態的代碼的參數名稱定義
        /// </summary>
        public static readonly string CHECK_FUNC_ID = "CHECK_FUNC_ID";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構檢查登入與功能狀態命令類別
        /// </summary>
        public CheckLogonCommand()
            : base(COMMAND_NAME)
        {
        }

        /// <summary>
        /// 建構檢查登入與功能狀態命令類別，並指定 commandAsker、logonSN、checkFuncId
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="logonSN">指定登入序號。</param>
        /// <param name="checkFuncId">指定要檢查功能狀態的代碼。</param>
        protected CheckLogonCommand(CommandAsker commandAsker, string logonSN, string checkFuncId)
            : base(COMMAND_NAME)
        {
            base.Parameters = new ServiceParameterList(3);
            base.Parameters.Add(COMMAND_ASKER, commandAsker);
            base.Parameters.Add(LOGON_SN, logonSN);
            base.Parameters.Add(CHECK_FUNC_ID, checkFuncId == null ? String.Empty : checkFuncId.Trim());
        }

        /// <summary>
        /// 建構檢查登入與功能狀態命令類別，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected CheckLogonCommand(ServiceParameterList parameters)
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
                && (base.Parameters.GetKeyFirstIndex(LOGON_SN) > -1)
                && (base.Parameters.GetKeyFirstIndex(CHECK_FUNC_ID) > -1))
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
        /// 取得登入序號
        /// </summary>
        /// <param name="logonSN">傳回登入序號。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetLogonSN(out string logonSN)
        {
            logonSN = null;
            ServiceParameter parameter = base.Parameters[LOGON_SN];
            if (parameter != null)
            {
                string sn = null;
                if (parameter.TryGetData<string>(out sn))
                {
                    logonSN = sn;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得要是否檢查功能狀態的代碼，不檢查則傳回空字串
        /// </summary>
        /// <param name="checkFuncId">傳回要檢查功能狀態的代碼，不檢查則傳回空字串。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetCheckFuncId(out string checkFuncId)
        {
            checkFuncId = null;
            ServiceParameter parameter = base.Parameters[CHECK_FUNC_ID];
            if (parameter != null)
            {
                object data = null;
                if (parameter.TryGetData(out data) && (data == null || data is String))
                {
                    checkFuncId = data == null ? String.Empty : data.ToString().Trim();
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
        /// 建立檢查登入與功能狀態命令類別物件
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="logonSN">指定登入序號。</param>
        /// <param name="checkFuncId">指定要檢查功能狀態的代碼，指定 null 或空字串表示不檢查。</param>
        /// <returns>成功則傳回學生登入命令類別物件，否則傳回 null。</returns>
        public static CheckLogonCommand Create(CommandAsker commandAsker, string logonSN, string checkFuncId)
        {
            if (logonSN != null)
            {
                logonSN = logonSN.Trim();
            }

            commandAsker.FuncId = "CHECK_LOGON";
            commandAsker.FuncName = "檢查登入與功能狀態";

            CheckLogonCommand command = new CheckLogonCommand(commandAsker, logonSN, checkFuncId);
            return command;
        }

        /// <summary>
        /// 建立檢查登入與功能狀態命令類別物件
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回學生登入命令類別物件，否則傳回 null。</returns>
        public static CheckLogonCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == CheckLogonCommand.COMMAND_NAME)
            {
                return new CheckLogonCommand(command.Parameters);
            }
            return null;
        }
        #endregion
    }
}
