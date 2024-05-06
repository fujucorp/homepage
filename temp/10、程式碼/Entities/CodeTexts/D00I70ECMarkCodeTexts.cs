using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// D00I70 電文更正記號的代碼文字定義清單類別
    /// </summary>
    public class D00I70ECMarkCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 正常交易 : 0
        /// </summary>
        public const string NORMAL_CODE = "0";

        /// <summary>
        /// 更正交易 : 1
        /// </summary>
        public const string RECTIFY_CODE = "1";
        #endregion

        #region Const Text
        /// <summary>
        /// 正常交易 : 0
        /// </summary>
        public const string NORMAL_TEXT = "正常交易";

        /// <summary>
        /// 更正交易 : 1
        /// </summary>
        public const string RECTIFY_TEXT = "更正交易";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構銷帳狀態的代碼文字定義清單類別
        /// </summary>
        public D00I70ECMarkCodeTexts()
        {
            base.Add(NORMAL_CODE, NORMAL_TEXT);
            base.Add(RECTIFY_CODE, RECTIFY_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得代碼對應的文字
        /// </summary>
        /// <param name="code">更正記號代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case NORMAL_CODE:
                    return NORMAL_TEXT;
                case RECTIFY_CODE:
                    return RECTIFY_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">更正記號代碼</param>
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

        /// <summary>
        /// 取得是否為定義的代碼
        /// </summary>
        /// <param name="code">更正記號代碼</param>
        /// <returns></returns>
        public static bool IsDefine(string code)
        {
            return !string.IsNullOrEmpty(GetText(code));
        }
        #endregion
    }
}
