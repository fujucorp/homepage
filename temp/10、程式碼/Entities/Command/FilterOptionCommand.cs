using System;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

namespace Entities
{
    /// <summary>
    /// 查詢主要條件選項資料命令類別
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ServiceParameter))]
    [XmlInclude(typeof(ServiceParameterList))]
    public class FilterOptionCommand : ServiceCommand<FilterOptionCommand>
    {
        #region Static Reaonly
        /// <summary>
        /// 命令的名稱定義
        /// </summary>
        public static readonly string COMMAND_NAME = "SELECT_FILTER_OPTION";

        /// <summary>
        /// 命令請求者資訊的參數名稱定義
        /// </summary>
        public static readonly string COMMAND_ASKER = "COMMAND_ASKER";

        /// <summary>
        /// 查詢資料模式
        /// </summary>
        public static readonly string DATA_MODE = "DATA_MODE";

        /// <summary>
        /// 查詢的代收類別代碼
        /// </summary>
        public static readonly string RECEIVE_TYPE = "RECEIVE_TYPE";

        /// <summary>
        /// 查詢的學年代碼
        /// </summary>
        public static readonly string YEAR_ID = "YEAR_ID";

        /// <summary>
        /// 查詢的學期代碼
        /// </summary>
        public static readonly string TERM_ID = "TERM_ID";

        /// <summary>
        /// 查詢的部別代碼
        /// </summary>
        public static readonly string DEP_ID = "DEP_ID";

        /// <summary>
        /// 查詢的代收費用別代碼
        /// </summary>
        public static readonly string RECEIVE_ID = "RECEIVE_ID";

        /// <summary>
        /// 查詢的商家代號的代收種類代碼
        /// </summary>
        public static readonly string RECEIVE_KIND = "RECEIVE_KIND";

        /// <summary>
        /// 查詢的各項目預設模式
        /// </summary>
        public static readonly string DEFAULT_MODES = "DEFAULT_MODES";

        #region [MDY:202203XX] 2022擴充案 英文名稱相關
        /// <summary>
        /// 是否英文介面
        /// </summary>
        public static readonly string IS_ENG_UI = "IS_ENG_UI";
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// 建構查詢主要條件選項資料命令類別
        /// </summary>
        public FilterOptionCommand()
        {
            base.CommandName = COMMAND_NAME;
        }

        #region [MDY:202203XX] 2022擴充案 英文名稱相關
        /// <summary>
        /// 建構查詢主要條件選項資料命令類別，並指定所有參數
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊</param>
        /// <param name="dataMode">指定查詢的資料模式</param>
        /// <param name="receiveType">指定查詢的代收類別代碼</param>
        /// <param name="yearID">指定查詢的學年代碼</param>
        /// <param name="termID">指定查詢的學期代碼</param>
        /// <param name="receiveID">指定查詢的代收費用別代碼</param>
        /// <param name="defaultModes">指定查詢的各項目預設模式</param>
        /// <param name="isEngUI">指定是否英文介面</param>
        protected FilterOptionCommand(CommandAsker commandAsker, string dataMode
            , string receiveType, string yearID, string termID, string receiveID, string receiveKind
            , string defaultModes, bool isEngUI)
        {
            base.CommandName = COMMAND_NAME;
            base.Parameters = new ServiceParameterList(7);
            base.Parameters.Add(COMMAND_ASKER, commandAsker);
            base.Parameters.Add(DATA_MODE, dataMode);
            base.Parameters.Add(RECEIVE_TYPE, receiveType);
            base.Parameters.Add(YEAR_ID, yearID);
            base.Parameters.Add(TERM_ID, termID);
            //base.Parameters.Add(DEP_ID, depID);
            base.Parameters.Add(RECEIVE_ID, receiveID);
            base.Parameters.Add(RECEIVE_KIND, receiveKind);
            base.Parameters.Add(DEFAULT_MODES, defaultModes);
            base.Parameters.Add(IS_ENG_UI, isEngUI);
        }
        #endregion

        /// <summary>
        /// 建構查詢首筆資料命令類別物件，並指定命令參數集合
        /// </summary>
        /// <param name="parameters">指定命令參數集合。</param>
        protected FilterOptionCommand(ServiceParameterList parameters)
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
                && (base.Parameters.GetKeyFirstIndex(COMMAND_ASKER) > -1)
                && (base.Parameters.GetKeyFirstIndex(DATA_MODE) > -1))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得命令請求者資訊 (必要參數)
        /// </summary>
        /// <param name="commandAsker">傳回命令請求者資訊。</param>
        /// <returns>取參數值成功則傳回 true，否則傳回 false。</returns>
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
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 取得查詢的資料模式 (必要參數)
        /// </summary>
        /// <param name="dataMode">傳回查詢的資料模式。</param>
        /// <returns>取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetDataMode(out string dataMode)
        {
            dataMode = null;
            ServiceParameter parameter = base.Parameters[DATA_MODE];
            if (parameter != null)
            {
                if (parameter.TryGetData<string>(out dataMode) && !String.IsNullOrEmpty(dataMode))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 取得查詢的代收類別代碼 (非必要參數)
        /// </summary>
        /// <param name="receiveType">傳回查詢的代收類別代碼。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetReceiveType(out string receiveType)
        {
            receiveType = null;
            ServiceParameter parameter = base.Parameters[RECEIVE_TYPE];
            if (parameter != null)
            {
                if (!parameter.TryGetData<string>(out receiveType))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得查詢的學年代碼 (非必要參數)
        /// </summary>
        /// <param name="yearID">傳回查詢的學年代碼。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetYearID(out string yearID)
        {
            yearID = null;
            ServiceParameter parameter = base.Parameters[YEAR_ID];
            if (parameter != null)
            {
                if (!parameter.TryGetData<string>(out yearID))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得查詢的學期代碼 (非必要參數)
        /// </summary>
        /// <param name="termID">傳回查詢的學期代碼。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetTermID(out string termID)
        {
            termID = null;
            ServiceParameter parameter = base.Parameters[TERM_ID];
            if (parameter != null)
            {
                if (!parameter.TryGetData<string>(out termID))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得查詢的部別代碼 (非必要參數)
        /// </summary>
        /// <param name="depID">傳回查詢的部別代碼。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetDepID(out string depID)
        {
            depID = null;
            ServiceParameter parameter = base.Parameters[DEP_ID];
            if (parameter != null)
            {
                if (!parameter.TryGetData<string>(out depID))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得查詢的代收費用別代碼 (非必要參數)
        /// </summary>
        /// <param name="receiveID">傳回查詢的代收費用別代碼。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetReceiveID(out string receiveID)
        {
            receiveID = null;
            ServiceParameter parameter = base.Parameters[RECEIVE_ID];
            if (parameter != null)
            {
                if (!parameter.TryGetData<string>(out receiveID))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得查詢的商家代號的代收種類代碼 (非必要參數)
        /// </summary>
        /// <param name="receiveKind">傳回查詢的商家代號的代收種類代碼。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetReceiveKind(out string receiveKind)
        {
            receiveKind = null;
            ServiceParameter parameter = base.Parameters[RECEIVE_KIND];
            if (parameter != null)
            {
                if (!parameter.TryGetData<string>(out receiveKind))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得查詢的各項目預設模式 (非必要參數)
        /// </summary>
        /// <param name="defaultModes">傳回查詢的各項目預設模式。</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetDefaultModes(out string defaultModes)
        {
            defaultModes = null;
            ServiceParameter parameter = base.Parameters[DEFAULT_MODES];
            if (parameter != null)
            {
                if (!parameter.TryGetData<string>(out defaultModes))
                {
                    return false;
                }
            }
            return true;
        }

        #region [MDY:202203XX] 2022擴充案 英文名稱相關
        /// <summary>
        /// 取得是否英文介面 (非必要參數)
        /// </summary>
        /// <param name="isEngUI">傳回是否英文介面</param>
        /// <returns>無此參數或取參數值成功則傳回 true，否則傳回 false。</returns>
        public bool GetIsEngUI(out bool isEngUI)
        {
            isEngUI = false;
            ServiceParameter parameter = base.Parameters[IS_ENG_UI];
            if (parameter != null)
            {
                if (!parameter.TryGetData<bool>(out isEngUI))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        #endregion

        #region Static Method
        #region [MDY:202203XX] 2022擴充案 英文名稱相關
        /// <summary>
        /// 建立查詢主要條件選項資料命令類別物件
        /// </summary>
        /// <param name="commandAsker">指定命令請求者資訊</param>
        /// <param name="dataMode">指定查詢的資料模式</param>
        /// <param name="receiveType">指定查詢的商家代號代碼</param>
        /// <param name="yearID">指定查詢的學年代碼</param>
        /// <param name="termID">指定查詢的學期代碼</param>
        /// <param name="receiveID">指定查詢的代收費用別代碼</param>
        /// <param name="receiveKind">指定查詢的商家代號的代收種類代碼</param>
        /// <param name="defaultModes">查詢的各項目預設模式 (每個字元由左至右 (索引值為 0 ~ 4) 依序代表代收類別、學年、學期、部別、代收費用別的預設模式)</param>
        /// <param name="isEngUI">指定是否英文介面</param>
        /// <returns>傳回查詢資料命令類別物件</returns>
        public static FilterOptionCommand Create(CommandAsker commandAsker, string dataMode
            , string receiveType, string yearID, string termID, string receiveID, string receiveKind
            , string defaultModes, bool isEngUI)
        {
            dataMode = dataMode == null ? null : dataMode.Trim();
            receiveType = receiveType == null ? null : receiveType.Trim();
            yearID = yearID == null ? null : yearID.Trim();
            termID = termID == null ? null : termID.Trim();
            //depID = depID == null ? null : depID.Trim();
            receiveID = receiveID == null ? null : receiveID.Trim();
            defaultModes = defaultModes == null ? null : defaultModes.Trim();
            FilterOptionCommand command = new FilterOptionCommand(commandAsker, dataMode, receiveType, yearID, termID, receiveID, receiveKind, defaultModes, isEngUI);
            return command;
        }
        #endregion

        /// <summary>
        /// 建立查詢主要條件選項資料命令類別物件
        /// </summary>
        /// <param name="command">指定資料處理服務命令(介面)物件。</param>
        /// <returns>成功則傳回查詢資料命令類別物件，否則傳回 null。</returns>
        public static FilterOptionCommand Create(IServiceCommand command)
        {
            if (command != null && command.CommandName == FilterOptionCommand.COMMAND_NAME)
            {
                return new FilterOptionCommand(command.Parameters);
            }
            return null;
        }
        #endregion
    }
}
