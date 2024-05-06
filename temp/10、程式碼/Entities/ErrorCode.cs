using System;

namespace Entities
{
    /// <summary>
    /// 錯誤代碼定義抽象類別
    /// </summary>
    public abstract class ErrorCode
    {
        #region 一般類錯誤
        /// <summary>
        /// 正確/成功
        /// </summary>
        public const string NORMAL_STATUS = "00000";

        #endregion

        #region 系統類錯誤
        /// <summary>
        /// 缺少或無效的資料處理代理物件
        /// </summary>
        public const string S_INVALID_PROXY = "BS001";

        /// <summary>
        /// 缺少或無效的資料庫存取物件
        /// </summary>
        public const string S_INVALID_FACTORY = "BS002";

        /// <summary>
        /// 無效的服務命令請求者資料
        /// </summary>
        public const string S_INVALID_COMMAND_ASKER = "BS003";

        /// <summary>
        /// 缺少或無效的參數
        /// </summary>
        public const string S_INVALID_PARAMETER = "BS004";

        /// <summary>
        /// 序列化資料失敗
        /// </summary>
        public const string S_SERIALIZED_FAILURE = "BS901";

        /// <summary>
        /// 反序列化資料失敗
        /// </summary>
        public const string S_DESERIALIZED_FAILURE = "BS902";

        /// <summary>
        /// 執行逾時
        /// </summary>
        public const string S_EXECUTE_TIMEOUT = "BS903";

        /// <summary>
        /// 不正確的回傳資料 (型別錯誤或轉型失敗)
        /// </summary>
        public const string S_INVALID_RETURN_VALUE = "BS904";
        #endregion

        #region 授權類錯誤
        /// <summary>
        /// 未授權
        /// </summary>
        public const string S_NO_AUTHORIZE = "BS905";

        /// <summary>
        /// 功能停用
        /// </summary>
        public const string S_FUNC_DISABLED = "BS906";

        /// <summary>
        /// 閒置逾時
        /// </summary>
        public const string S_SESSION_TIMEOUT = "BS907";

        /// <summary>
        /// 強迫登出
        /// </summary>
        public const string S_FORCED_LOGOUT = "BS908";

        /// <summary>
        /// 未授權該業務別
        /// </summary>
        public const string S_NO_AUTHORIZE_FOR_RECEIVETYPE = "BS910";

        /// <summary>
        /// 未授權該功能
        /// </summary>
        public const string S_NO_AUTHORIZE_FOR_FUNCTION = "BS911";

        /// <summary>
        /// 未授權查詢權限
        /// </summary>
        public const string S_NO_AUTHORIZE_FOR_QUERY = "BS912";

        /// <summary>
        /// 未授權維護權限
        /// </summary>
        public const string S_NO_AUTHORIZE_FOR_MAINTAIN = "BS913";
        #endregion

        #region 登入錯誤類
        /// <summary>
        /// 缺少或無效的登入身分別
        /// </summary>
        public const string L_INVALID_QUAL = "BL001";

        /// <summary>
        /// 缺少或無效的登入帳號
        /// </summary>
        public const string L_INVALID_USERID = "BL002";

        ///// <summary>
        ///// 缺少或無效的登入密碼
        ///// </summary>
        //public const string L_INVALID_PASSWORD = "BL003";

        /// <summary>
        /// 缺少或無效的登入代收類別
        /// </summary>
        public const string L_INVALID_RECEIVETYPE = "BL004";

        /// <summary>
        /// 缺少或無效的登入銀行
        /// </summary>
        public const string L_INVALID_BANKID = "BL005";

        /// <summary>
        /// 缺少或無效的登入身分證字號
        /// </summary>
        public const string L_INVALID_PERSONALID = "BL006";

        /// <summary>
        /// 缺少或無效的登入生日
        /// </summary>
        public const string L_INVALID_BIRTHDAY = "BL007";

        /// <summary>
        /// 登入失敗 (帳號或密碼錯誤)
        /// </summary>
        public const string L_LOGON_FAILURE = "BL008";

        /// <summary>
        /// 此帳號暫停使用(鎖住)
        /// </summary>
        public const string L_ACCOUNT_LOCKED = "BL009";

        /// <summary>
        /// 此帳號暫停使用(未覆核)
        /// </summary>
        public const string L_ACCOUNT_AUDITING = "BL010";

        /// <summary>
        /// 密碼已到期
        /// </summary>
        public const string L_PASSWORD_OVERDUE = "BL011";

        /// <summary>
        /// 強迫密碼變更
        /// </summary>
        public const string L_PASSWORD_CHANEG_MUST = "BL012";

        /// <summary>
        /// 提醒密碼變更
        /// </summary>
        public const string L_PASSWORD_CHANEG_MEMO = "BL013";

        /// <summary>
        /// 此帳號已登入
        /// </summary>
        public const string L_ACCOUNT_HAS_LOGON = "BL014";


        #endregion

        #region 資料類錯誤
        /// <summary>
        /// 資料已存在
        /// </summary>
        public const string D_DATA_EXISTS = "BD001";

        /// <summary>
        /// 資料不存在
        /// </summary>
        public const string D_DATA_NOT_FOUND = "BD002";

        /// <summary>
        /// 查無資料
        /// </summary>
        public const string D_QUERY_NO_DATA = "BD003";
        #endregion
    }
}
