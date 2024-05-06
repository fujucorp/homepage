using System;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

namespace Entities
{
    /// <summary>
    /// 行員登入命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class BankLogonCommand : ServiceCommand<BankLogonCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "BANK_LOGON";

        /// <summary>
        /// 命令請求者資訊的參數名稱定義
        /// </summary>
        public static readonly string COMMAND_ASKER = "COMMAND_ASKER";

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 行員登入密碼的參數名稱定義
        /// </summary>
        public static readonly string USER_PXX = "USER_PXX";
        #endregion

        /// <summary>
        /// 行員登入 IP 的參數名稱定義
        /// </summary>
        public static readonly string CLIENT_IP = "CLIENT_IP";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構使用者登入命令類別
        /// </summary>
        public BankLogonCommand()
            : base(COMMAND_NAME)
        {
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 建構行員登入命令類別，並指定 commandAsker、userPXX、clientIP
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="userPXX">指定使用者登入密碼。</param>
        /// <param name="clientIP">指定使用者登入 IP。</param>
        protected BankLogonCommand(CommandAsker commandAsker, string userPXX, string clientIP)
            : base(COMMAND_NAME)
        {
            base.Parameters = new ServiceParameterList(3);
            base.Parameters.Add(COMMAND_ASKER, commandAsker);
            base.Parameters.Add(USER_PXX, userPXX);
            base.Parameters.Add(CLIENT_IP, clientIP);
        }
        #endregion

        /// <summary>
        /// 建構行員登入命令類別，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected BankLogonCommand(ServiceParameterList parameters)
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
                && (base.Parameters.GetKeyFirstIndex(USER_PXX) > -1))
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

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 取得使用者的登入密碼
        /// </summary>
        /// <param name="pxx">傳回登入密碼。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetUserPXX(out string pxx)
        {
            pxx = null;
            ServiceParameter parameter = base.Parameters[USER_PXX];
            if (parameter != null)
            {
                string value = null;
                if (parameter.TryGetData<string>(out value))
                {
                    pxx = value;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 取得使用者的登入 IP
        /// </summary>
        /// <param name="clientIP">傳回登入 IP。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetClientIP(out string clientIP)
        {
            clientIP = null;
            ServiceParameter parameter = base.Parameters[CLIENT_IP];
            if (parameter != null)
            {
                string ip = null;
                if (parameter.TryGetData<string>(out ip))
                {
                    clientIP = ip;
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
        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 建立使用者登入命令類別物件
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="pxx">指定使用者的登入密碼。</param>
        /// <param name="clientIP">指定使用者的傳回登入 IP。</param>
        /// <returns>成功則傳回使用者登入命令類別物件，否則傳回 null。</returns>
        private static BankLogonCommand Create(CommandAsker commandAsker, string pxx, string clientIP)
        {
            if (pxx != null)
            {
                pxx = pxx.Trim();
            }

            BankLogonCommand command = new BankLogonCommand(commandAsker, pxx, clientIP);
            return command;
        }

        /// <summary>
        /// 建立銀行使用者登入命令類別物件
        /// </summary>
        /// <param name="userId">指定使用者的登入帳號。</param>
        /// <param name="pxx">指定使用者的登入密碼。</param>
        /// <param name="clientIP">指定使用者的登入 IP。</param>
        /// <param name="cultureName">指定使用者的登入語系名稱。非必要參數。</param>
        /// <returns>成功則傳回使用者登入命令類別物件，否則傳回 null。</returns>
        public static BankLogonCommand CreateForBank(string userId, string pxx, string clientIP, string cultureName)
        {
            CommandAsker asker = new CommandAsker();
            asker.UserQual = UserQualCodeTexts.BANK;
            asker.UnitId = String.Empty;
            asker.UserId = userId;
            asker.LogonTime = null;
            asker.CultureName = cultureName;
            asker.FuncId = "LOGON";
            asker.FuncName = "銀行使用者登入";

            return Create(asker, pxx, clientIP);
        }
        #endregion

        /// <summary>
        /// 建立建構使用者登入命令類別
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回使用者登入命令類別物件，否則傳回 null。</returns>
        public static BankLogonCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == BankLogonCommand.COMMAND_NAME)
            {
                return new BankLogonCommand(command.Parameters);
            }
            return null;
        }
        #endregion
    }
}
