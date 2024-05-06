using System;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

namespace Entities
{
    /// <summary>
    /// 行員 SSO 登入命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class SSOLogonCommand : ServiceCommand<SSOLogonCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "SSO_LOGON";

        /// <summary>
        /// 命令請求者資訊的參數名稱定義
        /// </summary>
        public static readonly string COMMAND_ASKER = "COMMAND_ASKER";
        /// <summary>
        /// SSO 的 pusid 參數
        /// </summary>
        public static readonly string SSO_PUSID = "SSO_PUSID";
        /// <summary>
        /// SSO 驗證後使用者帳號
        /// </summary>
        public static readonly string SSO_USER_ID = "SSO_USER_ID";
        /// <summary>
        /// SSO 驗證後使用者名稱
        /// </summary>
        public static readonly string SSO_USER_NAME = "SSO_USER_NAME";
        /// <summary>
        /// SSO 驗證後使用者群組
        /// </summary>
        public static readonly string SSO_GROUP_IDS = "SSO_GROUP_IDS";
        /// <summary>
        /// SSO 驗證後分行代碼
        /// </summary>
        public static readonly string SSO_BRANCH_ID = "SSO_BRANCH_ID";
        /// <summary>
        /// 使用者登入 IP 的參數名稱定義
        /// </summary>
        public static readonly string CLIENT_IP = "CLIENT_IP";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構行員 SSO 登入命令類別
        /// </summary>
        public SSOLogonCommand()
            : base(COMMAND_NAME)
        {
        }

        /// <summary>
        /// 建構行員 SSO 登入命令類別，並指定 pusid、userId、userName、roleIds、branchId
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="pusid">指定 SSO 的 pusid 參數。</param>
        /// <param name="userId">指定 SSO 驗證後使用者帳號。</param>
        /// <param name="userName">指定 SSO 驗證後使用者名稱。</param>
        /// <param name="groupIds">指定 SSO 驗證後使用者群組。</param>
        /// <param name="branchId">指定 SSO 驗證後分行代碼。</param>
        /// <param name="clientIP">指定使用者登入 IP。</param>
        protected SSOLogonCommand(CommandAsker commandAsker, string pusid, string userId, string userName, string[] groupIds, string branchId, string clientIP)
            : base(COMMAND_NAME)
        {
            base.Parameters = new ServiceParameterList(7);
            base.Parameters.Add(COMMAND_ASKER, commandAsker);
            base.Parameters.Add(SSO_PUSID, pusid);
            base.Parameters.Add(SSO_USER_ID, userId);
            base.Parameters.Add(SSO_USER_NAME, userName);
            base.Parameters.Add(SSO_GROUP_IDS, groupIds);
            base.Parameters.Add(SSO_BRANCH_ID, branchId);
            base.Parameters.Add(CLIENT_IP, clientIP);
        }

        /// <summary>
        /// 建構行員 SSO 登入命令類別，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected SSOLogonCommand(ServiceParameterList parameters)
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
                && (base.Parameters.GetKeyFirstIndex(SSO_PUSID) > -1)
                && (base.Parameters.GetKeyFirstIndex(SSO_USER_ID) > -1)
                && (base.Parameters.GetKeyFirstIndex(SSO_USER_NAME) > -1)
                && (base.Parameters.GetKeyFirstIndex(SSO_GROUP_IDS) > -1)
                && (base.Parameters.GetKeyFirstIndex(SSO_BRANCH_ID) > -1))
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
        /// 取得 SSO 的 pusid 參數
        /// </summary>
        /// <param name="pusid">傳回 SSO 的 pusid 參數。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetPUSID(out string pusid)
        {
            pusid = null;
            ServiceParameter parameter = base.Parameters[SSO_PUSID];
            if (parameter != null)
            {
                string value = null;
                if (parameter.TryGetData<string>(out value))
                {
                    pusid = value;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得 SSO 驗證後使用者帳號
        /// </summary>
        /// <param name="personalId">傳回 SSO 驗證後使用者帳號。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetUserId(out string userId)
        {
            userId = null;
            ServiceParameter parameter = base.Parameters[SSO_USER_ID];
            if (parameter != null)
            {
                string value = null;
                if (parameter.TryGetData<string>(out value))
                {
                    userId = value;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得 SSO 驗證後使用者名稱
        /// </summary>
        /// <param name="personalId">傳回 SSO 驗證後使用者名稱。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetUserName(out string userName)
        {
            userName = null;
            ServiceParameter parameter = base.Parameters[SSO_USER_NAME];
            if (parameter != null)
            {
                string value = null;
                if (parameter.TryGetData<string>(out value))
                {
                    userName = value;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得 SSO 驗證後使用者群組
        /// </summary>
        /// <param name="personalId">傳回 SSO 驗證後使用者群組。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetGroupIds(out string[] groupIds)
        {
            groupIds = null;
            ServiceParameter parameter = base.Parameters[SSO_GROUP_IDS];
            if (parameter != null)
            {
                object data = null;
                if (parameter.TryGetData(out data) && data is string[])
                {
                    groupIds = data as string[];
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得 SSO 驗證後分行代碼
        /// </summary>
        /// <param name="birthday">傳回SSO 驗證後分行代碼。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetBranchId(out string branchId)
        {
            branchId = null;
            ServiceParameter parameter = base.Parameters[SSO_BRANCH_ID];
            if (parameter != null)
            {
                string value = null;
                if (parameter.TryGetData<string>(out value))
                {
                    branchId = value;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

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
        /// <summary>
        /// 建立行員 SSO 登入命令類別物件
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="pusid">指定 SSO 的 pusid 參數。</param>
        /// <param name="userId">指定 SSO 驗證後使用者帳號。</param>
        /// <param name="userName">指定 SSO 驗證後使用者名稱。</param>
        /// <param name="groupIds">指定 SSO 驗證後使用者群組。</param>
        /// <param name="branchId">指定 SSO 驗證後分行代碼。</param>
        /// <returns>成功則傳回行員 SSO 登入命令類別物件，否則傳回 null。</returns>
        private static SSOLogonCommand Create(CommandAsker commandAsker, string pusid, string userId, string userName, string[] groupIds, string branchId, string clientIP)
        {
            if (pusid != null)
            {
                pusid = pusid.Trim();
            }
            if (userId != null)
            {
                userId = userId.Trim();
            }
            if (userName != null)
            {
                userName = userName.Trim();
            }
            if (branchId != null)
            {
                branchId = branchId.Trim();
            }

            SSOLogonCommand command = new SSOLogonCommand(commandAsker, pusid, userId, userName, groupIds, branchId, clientIP);
            return command;
        }

        /// <summary>
        /// 建立行員 SSO 登入命令類別物件
        /// </summary>
        /// <param name="pusid">指定 SSO 的 pusid 參數。</param>
        /// <param name="userId">指定 SSO 驗證後使用者帳號。</param>
        /// <param name="userName">指定 SSO 驗證後使用者名稱。</param>
        /// <param name="groupIds">指定 SSO 驗證後使用者群組。</param>
        /// <param name="branchId">指定 SSO 驗證後分行代碼。</param>
        /// <param name="clientIP">指定使用者的登入 IP。</param>
        /// <param name="cultureName">指定使用者的登入語系名稱。非必要參數。</param>
        /// <returns>成功則傳回行員 SSO 登入命令類別物件，否則傳回 null。</returns>
        public static SSOLogonCommand Create(string pusid, string userId, string userName, string[] groupIds, string branchId, string clientIP, string cultureName)
        {
            CommandAsker asker = new CommandAsker();
            asker.UserQual = UserQualCodeTexts.BANK;
            asker.UnitId = branchId;
            asker.UserId = userId;
            asker.LogonTime = null;
            asker.CultureName = cultureName;
            asker.FuncId = "LOGON";
            asker.FuncName = "行員SSO登入";

            return Create(asker, pusid, userId, userName, groupIds, branchId, clientIP);
        }

        /// <summary>
        /// 建立行員 SSO 登入命令類別物件
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回行員 SSO 登入命令類別物件，否則傳回 null。</returns>
        public static SSOLogonCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == SSOLogonCommand.COMMAND_NAME)
            {
                return new SSOLogonCommand(command.Parameters);
            }
            return null;
        }
        #endregion
    }
}
