using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 流程處理種類代碼文字定義清單類別
    /// </summary>
    public class ProcessKindCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 放行 : A
        /// </summary>
        public const string APPROVE = "A";

        /// <summary>
        /// 駁回 : R
        /// </summary>
        public const string REJECT = "R";
        #endregion

        #region Const Text
        /// <summary>
        /// 放行 : A
        /// </summary>
        public const string APPROVE_TEXT = "放行";

        /// <summary>
        /// 駁回 : R
        /// </summary>
        public const string REJECT_TEXT = "駁回";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構流程處理種類代碼文字定義清單類別
        /// </summary>
        public ProcessKindCodeTexts()
        {
            base.Add(APPROVE, APPROVE_TEXT);
            base.Add(REJECT, REJECT_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得流程處理種類對應的文字
        /// </summary>
        /// <param name="code">流程處理種類代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case APPROVE:
                    return APPROVE_TEXT;
                case REJECT:
                    return REJECT_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得流程處理種類代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">流程處理種類代碼</param>
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
