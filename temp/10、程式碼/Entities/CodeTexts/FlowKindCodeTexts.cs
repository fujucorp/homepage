using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 流程(階層)種類代碼文字定義清單類別
    /// </summary>
    public class FlowKindCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 單階 (免審核) : S
        /// </summary>
        public const string SINGLE = "S";

        /// <summary>
        /// 多階 (需審核) : M
        /// </summary>
        public const string MULTI = "M";
        #endregion

        #region Const Text
        /// <summary>
        /// 單階 (免審核) : S
        /// </summary>
        public const string SINGLE_TEXT = "單階 (免審核)";

        /// <summary>
        /// 多階 (需審核) : M
        /// </summary>
        public const string MULTI_TEXT = "多階 (需審核)";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構流程(階層)種類代碼文字定義清單類別
        /// </summary>
        public FlowKindCodeTexts()
        {
            base.Add(SINGLE, SINGLE_TEXT);
            base.Add(MULTI, MULTI_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得流程(階層)種類代碼對應的文字
        /// </summary>
        /// <param name="code">流程(階層)種類代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case SINGLE:
                    return SINGLE_TEXT;
                case MULTI:
                    return MULTI_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得流程(階層)種類代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">流程(階層)種類代碼</param>
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
