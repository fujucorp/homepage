using System;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

namespace Entities
{
    /// <summary>
    /// 學生登入命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class StudentLogonCommand : ServiceCommand<StudentLogonCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "STUDENT_LOGON";

        /// <summary>
        /// 命令請求者資訊的參數名稱定義
        /// </summary>
        public static readonly string COMMAND_ASKER = "COMMAND_ASKER";
        /// <summary>
        /// 學生登入學校統編的參數名稱定義
        /// </summary>
        public static readonly string SCHOOL_IDENTITY = "SCHOOL_IDENTITY";
        /// <summary>
        /// 學生登入學號的參數名稱定義
        /// </summary>
        public static readonly string STUDENT_ID = "STUDENT_ID";
        /// <summary>
        /// 學生登入 Key 的參數名稱定義
        /// </summary>
        public static readonly string LOGIN_KEY = "LOGIN_KEY";
        /// <summary>
        /// 使用者登入 IP 的參數名稱定義
        /// </summary>
        public static readonly string CLIENT_IP = "CLIENT_IP";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構學生登入命令類別
        /// </summary>
        public StudentLogonCommand()
            : base(COMMAND_NAME)
        {
        }

        /// <summary>
        /// 建構學生登入命令類別，並指定 commandAsker、personalId、birthday、clientIP
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="schIdentity">指定學生登入學校統編。</param>
        /// <param name="studentId">指定學生登入學號。</param>
        /// <param name="loginKey">指定學生登入 Key。</param>
        /// <param name="clientIP">指定使用者登入 IP。</param>
        protected StudentLogonCommand(CommandAsker commandAsker, string schIdentity, string studentId, string loginKey, string clientIP)
            : base(COMMAND_NAME)
        {
            base.Parameters = new ServiceParameterList(5);
            base.Parameters.Add(COMMAND_ASKER, commandAsker);
            base.Parameters.Add(SCHOOL_IDENTITY, schIdentity);
            base.Parameters.Add(STUDENT_ID, studentId);
            base.Parameters.Add(LOGIN_KEY, loginKey);
            base.Parameters.Add(CLIENT_IP, clientIP);
        }

        /// <summary>
        /// 建構學生登入命令類別，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected StudentLogonCommand(ServiceParameterList parameters)
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
                && (base.Parameters.GetKeyFirstIndex(SCHOOL_IDENTITY) > -1)
                && (base.Parameters.GetKeyFirstIndex(STUDENT_ID) > -1)
                && (base.Parameters.GetKeyFirstIndex(LOGIN_KEY) > -1))
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
        /// 取得使用者的登入學校統編
        /// </summary>
        /// <param name="schIdentity">傳回登入學校代收類別。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetSchoolIdentity(out string schIdentity)
        {
            schIdentity = null;
            ServiceParameter parameter = base.Parameters[SCHOOL_IDENTITY];
            if (parameter != null)
            {
                string value = null;
                if (parameter.TryGetData<string>(out value))
                {
                    schIdentity = value;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得使用者的登入學號
        /// </summary>
        /// <param name="studentId">傳回登入學號。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetStudentId(out string studentId)
        {
            studentId = null;
            ServiceParameter parameter = base.Parameters[STUDENT_ID];
            if (parameter != null)
            {
                string value = null;
                if (parameter.TryGetData<string>(out value))
                {
                    studentId = value;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得使用者的登入 Key
        /// </summary>
        /// <param name="loginKey">傳回登入 Key。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetLoginKey(out string loginKey)
        {
            loginKey = null;
            ServiceParameter parameter = base.Parameters[LOGIN_KEY];
            if (parameter != null)
            {
                string value = null;
                if (parameter.TryGetData<string>(out value))
                {
                    loginKey = value;
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
        /// 建立使用者登入命令類別物件
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊。</param>
        /// <param name="schIdentity">指定學生的登入學校統編。</param>
        /// <param name="studentId">指定學生的登入學號。</param>
        /// <param name="loginKey">指定學生的登入 Key。</param>
        /// <param name="clientIP">指定使用者的登入 IP。</param>
        /// <returns>成功則傳回學生登入命令類別物件，否則傳回 null。</returns>
        private static StudentLogonCommand Create(CommandAsker commandAsker, string schIdentity, string studentId, string loginKey, string clientIP)
        {
            if (schIdentity != null)
            {
                schIdentity = schIdentity.Trim();
            }
            if (studentId != null)
            {
                studentId = studentId.Trim();
            }
            if (loginKey != null)
            {
                loginKey = loginKey.Trim();
            }

            StudentLogonCommand command = new StudentLogonCommand(commandAsker, schIdentity, studentId, loginKey, clientIP);
            return command;
        }

        /// <summary>
        /// 建立銀行使用者登入命令類別物件
        /// </summary>
        /// <param name="schIdentity">指定學生的登入學校統編。</param>
        /// <param name="studentId">指定學生的登入學號。</param>
        /// <param name="loginKey">指定學生的登入 Key。</param>
        /// <param name="clientIP">指定使用者的登入 IP。</param>
        /// <param name="cultureName">指定使用者的登入語系名稱。非必要參數。</param>
        /// <returns>成功則傳回學生登入命令類別物件，否則傳回 null。</returns>
        public static StudentLogonCommand Create(string schIdentity, string studentId, string loginKey, string clientIP, string cultureName)
        {
            CommandAsker asker = new CommandAsker();
            asker.UserQual = UserQualCodeTexts.STUDENT;
            asker.UnitId = schIdentity;
            asker.UserId = studentId;
            asker.LogonTime = null;
            asker.CultureName = cultureName;
            asker.FuncId = "LOGON";
            asker.FuncName = "學生查詢登入";

            return Create(asker, schIdentity, studentId, loginKey, clientIP);
        }

        /// <summary>
        /// 建立新增資料命令類別物件
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回學生登入命令類別物件，否則傳回 null。</returns>
        public static StudentLogonCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == StudentLogonCommand.COMMAND_NAME)
            {
                return new StudentLogonCommand(command.Parameters);
            }
            return null;
        }
        #endregion
    }
}
