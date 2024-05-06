using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 流程狀態代碼文字定義清單類別
    /// </summary>
    public class FlowStatusCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 待覆核 : 0
        /// </summary>
        public const string FLOWING = "0";

        /// <summary>
        /// 已覆核 : 1
        /// </summary>
        public const string ENDING = "1";

        /// <summary>
        /// 處理中 : 2
        /// </summary>
        public const string PROCESSING = "2";
        #endregion

        #region Const Text
        /// <summary>
        /// 待覆核 : 0
        /// </summary>
        public const string FLOWING_TEXT = "待覆核";

        /// <summary>
        /// 已覆核 : 1
        /// </summary>
        public const string ENDING_TEXT = "已覆核";

        /// <summary>
        /// 處理中 : 2
        /// </summary>
        public const string PROCESSING_TEXT = "處理中";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構流程狀態代碼文字定義清單類別
        /// </summary>
        public FlowStatusCodeTexts()
        {
            base.Add(FLOWING, FLOWING_TEXT);
            base.Add(ENDING, ENDING_TEXT);
            base.Add(PROCESSING, PROCESSING_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得流程狀態對應的文字
        /// </summary>
        /// <param name="code">流程狀態代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case FLOWING:
                    return FLOWING_TEXT;
                case ENDING:
                    return ENDING_TEXT;
                case PROCESSING:
                    return PROCESSING_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得流程狀態代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">流程狀態代碼</param>
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
