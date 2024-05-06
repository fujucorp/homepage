using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 學生繳費資料計算方式的代碼文字定義清單類別
    /// </summary>
    public class BillingTypeCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 依標準計算 : 1
        /// </summary>
        public const string BY_STANDARD = "1";

        /// <summary>
        /// 依金額計算 : 2
        /// </summary>
        public const string BY_AMOUNT = "2";
        #endregion

        #region Const Text
        /// <summary>
        /// 依標準計算 : 1
        /// </summary>
        public const string BY_STANDARD_TEXT = "依標準計算";

        /// <summary>
        /// 依金額計算 : 2
        /// </summary>
        public const string BY_AMOUNT_TEXT = "依金額計算";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構學生繳費資料計算方式的代碼文字定義清單類別
        /// </summary>
        public BillingTypeCodeTexts()
        {
            base.Add(BY_STANDARD, BY_STANDARD_TEXT);
            base.Add(BY_AMOUNT, BY_AMOUNT_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得學生繳費資料計算方式代碼對應的文字
        /// </summary>
        /// <param name="code">學生繳費資料計算方式代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case BY_STANDARD:
                    return BY_AMOUNT;
                case BY_AMOUNT:
                    return BY_AMOUNT_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得學生繳費資料計算方式代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">學生繳費資料計算方式代碼</param>
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
