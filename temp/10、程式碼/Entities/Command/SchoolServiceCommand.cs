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
    /// 後台連動製單服務方法名稱定義抽象類別
    /// </summary>
    public abstract class SchoolServiceMethodName
    {
        /// <summary>
        /// 單筆新增、修改、刪除學生繳費資料
        /// </summary>
        public const string Bill = "Bill";

        /// <summary>
        /// 學生繳費資料查詢
        /// </summary>
        public const string BillQuery = "BillQuery";

        /// <summary>
        /// 入金資料查詢
        /// </summary>
        public const string PayQuery = "PayQuery";

        #region [MDY:20191219] 新增繳費資訊查詢
        /// <summary>
        /// 繳費資訊查詢
        /// </summary>
        public const string QueryData = "QueryData";
        #endregion

        public static bool IsDefine(string name)
        {
            #region [MDY:20191219] 新增繳費資訊查詢
            switch (name)
            {
                case Bill:
                case BillQuery:
                case PayQuery:
                case QueryData:
                    return true;
            }
            #endregion
            return false;
        }
    }

    /// <summary>
    /// 呼叫後台連動製單服務方法命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class SchoolServiceCommand : ServiceCommand<SchoolServiceCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "CALL_SCHOOL_SERVICE";

        /// <summary>
        /// 系統代碼的參數名稱定義
        /// </summary>
        public static readonly string SYS_ID = "SYS_ID";

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 系統驗證碼的參數名稱定義
        /// </summary>
        public static readonly string SYS_PXX = "SYS_PXX";
        #endregion

        /// <summary>
        /// 呼叫端 IP 的參數名稱定義
        /// </summary>
        public static readonly string CLIENT_IP = "CLIENT_IP";

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
        /// 建構呼叫後台連動製單服務方法命令類別
        /// </summary>
        public SchoolServiceCommand()
            : base(COMMAND_NAME)
        {
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 建構呼叫後台連動製單服務方法命令類別，並指定 sysId、sysPXX、clientIP、methodName、methodArguments
        /// </summary>
        /// <param name="sysId">指定系統代碼。</param>
        /// <param name="sysPXX">指定系統驗證碼。</param>
        /// <param name="clientIP">指定呼叫端 IP。</param>
        /// <param name="methodName">要呼叫的方法名稱。</param>
        /// <param name="methodArguments">要傳給呼叫方法的參數集合。</param>
        protected SchoolServiceCommand(string sysId, string sysPXX, string clientIP, string methodName, ICollection<KeyValue<string>> methodArguments)
            : base(COMMAND_NAME)
        {
            base.Parameters = new ServiceParameterList(5);
            base.Parameters.Add(SYS_ID, sysId);
            base.Parameters.Add(SYS_PXX, sysPXX);
            base.Parameters.Add(CLIENT_IP, clientIP);
            base.Parameters.Add(METHOD_NAME, methodName);
            base.Parameters.Add(METHOD_ARGUMENTS, methodArguments);
        }
        #endregion

        /// <summary>
        /// 建構呼叫後台服務方法命令類別，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected SchoolServiceCommand(ServiceParameterList parameters)
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
            #region [MDY:20220530] Checkmarx 調整
            if (base.IsReady()
                && (base.Parameters.GetKeyFirstIndex(SYS_ID) > -1)
                && (base.Parameters.GetKeyFirstIndex(SYS_PXX) > -1)
                && (base.Parameters.GetKeyFirstIndex(CLIENT_IP) > -1)
                && (base.Parameters.GetKeyFirstIndex(METHOD_NAME) > -1))
            {
                return true;
            }
            #endregion

            return false;
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得系統代碼
        /// </summary>
        /// <param name="sysId">傳回系統代碼。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetSysId(out string sysId)
        {
            sysId = null;
            ServiceParameter parameter = base.Parameters[SYS_ID];
            if (parameter != null)
            {
                string data = null;
                if (parameter.TryGetData<string>(out data))
                {
                    sysId = data;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        #region [MDY:20220530] Checkmarx 調整
        #region [MDY:20210401] 原碼修正
        /// <summary>
        /// 取得系統驗證碼
        /// </summary>
        /// <param name="sysPXX">傳回系統驗證碼。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetSysPXX(out string sysPXX)
        {
            sysPXX = null;
            ServiceParameter parameter = base.Parameters[SYS_PXX];
            if (parameter != null)
            {
                string data = null;
                if (parameter.TryGetData<string>(out data))
                {
                    sysPXX = data;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        #endregion

        /// <summary>
        /// 取得呼叫端 IP
        /// </summary>
        /// <param name="sysId">傳回呼叫端 IP。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetClientIP(out string clientIP)
        {
            clientIP = null;
            ServiceParameter parameter = base.Parameters[CLIENT_IP];
            if (parameter != null)
            {
                string data = null;
                if (parameter.TryGetData<string>(out data))
                {
                    clientIP = data;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得呼叫的方法名稱
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
        /// 取得呼叫方法的參數集合
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
        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 建立呼叫後台連動製單服務方法命令類別物件
        /// </summary>
        /// <param name="sysId">指定系統代碼。</param>
        /// <param name="sysPXX">指定系統驗證碼。</param>
        /// <param name="clientIP">指定呼叫端 IP。</param>
        /// <param name="methodName">要呼叫的方法名稱。</param>
        /// <param name="methodArguments">要傳給呼叫方法的參數集合。</param>
        /// <returns>成功則傳回呼叫後台服務方法命令類別物件，否則傳回 null。</returns>
        public static SchoolServiceCommand Create(string sysId, string sysPXX, string clientIP, string methodName, ICollection<KeyValue<string>> methodArguments)
        {
            SchoolServiceCommand command = new SchoolServiceCommand(sysId, sysPXX, clientIP, methodName, methodArguments);
            return command;
        }
        #endregion

        /// <summary>
        /// 建立呼叫後台連動製單服務方法命令類別物件
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static SchoolServiceCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == SchoolServiceCommand.COMMAND_NAME)
            {
                return new SchoolServiceCommand(command.Parameters);
            }
            return null;
        }
        #endregion
    }
}
