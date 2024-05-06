using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 資料狀態代碼文字定義清單類別
    /// </summary>
    public class DataStatusCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 正常 (啟用) : 0
        /// </summary>
        public const string NORMAL = "0";

        /// <summary>
        /// 停用 : D
        /// </summary>
        public const string DISABLED = "D";
        #endregion

        #region Const Text
        /// <summary>
        /// 正常 (啟用) : 0
        /// </summary>
        public const string NORMAL_TEXT = "正常";

        /// <summary>
        /// 停用 : D
        /// </summary>
        public const string DISABLED_TEXT = "停用";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構資料狀態代碼文字定義清單類別
        /// </summary>
        public DataStatusCodeTexts()
        {
            base.Add(NORMAL, NORMAL_TEXT);
            base.Add(DISABLED, DISABLED_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得資料狀態代碼對應的文字
        /// </summary>
        /// <param name="code">資料狀態代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case NORMAL:
                    return NORMAL_TEXT;
                case DISABLED:
                    return DISABLED_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得資料狀態代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">資料狀態代碼</param>
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
