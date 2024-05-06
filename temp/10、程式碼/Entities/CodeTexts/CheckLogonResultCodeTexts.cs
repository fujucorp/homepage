using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 檢查登入與功能狀態結果的代碼文字定義清單類別
    /// </summary>
    public class CheckLogonResultCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 正常 (已登入、功能啟用) : 0
        /// </summary>
        public const string IS_OK = "0";

        /// <summary>
        /// 檢查失敗 : 1
        /// </summary>
        public const string CHECK_FAILURE = "1";

        /// <summary>
        /// 未登入 : 2
        /// </summary>
        public const string NON_LOGON = "2";

        /// <summary>
        /// 功能停用 : 3
        /// </summary>
        public const string FUNC_DISABLED = "3";
        #endregion

        #region Const Text
        /// <summary>
        /// 正常 (已登入、功能啟用) : 0
        /// </summary>
        public const string IS_OK_TEXT = "正常";

        /// <summary>
        /// 檢查失敗 : 1
        /// </summary>
        public const string CHECK_FAILURE_TEXT = "檢查失敗";

        /// <summary>
        /// 未登入 : 2
        /// </summary>
        public const string NON_LOGON_TEXT = "未登入";

        /// <summary>
        /// 功能停用 : 3
        /// </summary>
        public const string FUNC_DISABLED_TEXT = "功能停用";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構檢查登入與功能狀態結果的代碼文字定義清單類別
        /// </summary>
        public CheckLogonResultCodeTexts()
        {
            base.Add(IS_OK, IS_OK_TEXT);
            base.Add(CHECK_FAILURE, CHECK_FAILURE_TEXT);
            base.Add(NON_LOGON, NON_LOGON_TEXT);
            base.Add(FUNC_DISABLED, FUNC_DISABLED_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得檢查登入與功能狀態結果代碼對應的文字
        /// </summary>
        /// <param name="code">模板適用單位類別代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case IS_OK:
                    return IS_OK_TEXT;
                case CHECK_FAILURE:
                    return CHECK_FAILURE_TEXT;
                case NON_LOGON:
                    return NON_LOGON_TEXT;
                case FUNC_DISABLED:
                    return FUNC_DISABLED_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得檢查登入與功能狀態結果代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">模板適用單位類別代碼</param>
        /// <returns>傳回對應的代碼與文字對照類別，無對應則傳回 null</returns>
        public static CodeText GetCodeText(string code)
        {
            string text = GetText(code);
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            else
            {
                return new CodeText(code, text);
            }
        }
        #endregion
    }
}
