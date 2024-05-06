using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 模板類別的代碼文字定義清單類別
    /// </summary>
    public class BillFormTypeCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 繳費單 : 0
        /// </summary>
        public const string BILLING = "0";

        /// <summary>
        /// 收據 : 1
        /// </summary>
        public const string RECEIPT = "1";
        #endregion

        #region Const Text
        /// <summary>
        /// 繳費單 : 0
        /// </summary>
        public const string BILLING_TEXT = "繳費單";

        /// <summary>
        /// 收據 : 1
        /// </summary>
        public const string RECEIPT_TEXT = "收據";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構模板類別的代碼文字定義清單類別
        /// </summary>
        public BillFormTypeCodeTexts()
        {
            base.Add(BILLING, BILLING_TEXT);
            base.Add(RECEIPT, RECEIPT_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得模板類別代碼對應的文字
        /// </summary>
        /// <param name="code">模板類別代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case BILLING:
                    return BILLING_TEXT;
                case RECEIPT:
                    return RECEIPT_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得模板類別代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">模板類別代碼</param>
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
