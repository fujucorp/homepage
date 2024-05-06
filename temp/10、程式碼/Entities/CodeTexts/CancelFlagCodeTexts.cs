using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 銷帳註記的代碼文字定義清單類別
    /// </summary>
    public class CancelFlagCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 未繳款 : 空字串
        /// </summary>
        public const string NONPAY = "";
        /// <summary>
        /// 連線 : 1
        /// </summary>
        public const string CANCELED = "1";
        /// <summary>
        /// 金額不符 : 2
        /// </summary>
        public const string AMOUNT_ERROR = "2";
        /// <summary>
        /// 檢碼不符 : 3
        /// </summary>
        public const string CHECKSUM_ERROR = "3";
        /// <summary>
        /// 銷問題檔 : 7
        /// </summary>
        public const string BY_PROBLEM = "7";
        /// <summary>
        /// 人工銷帳 : 8
        /// </summary>
        public const string BY_HAND = "8";
        /// <summary>
        /// 離線 : 9
        /// </summary>
        public const string RECEIVE_SELF = "9";
        #endregion

        #region Const Text
        /// <summary>
        /// 未繳款 : 空字串
        /// </summary>
        public const string NONPAY_TEXT = "未繳款";
        /// <summary>
        /// 連線 : 1
        /// </summary>
        public const string CANCELED_TEXT = "連線";
        /// <summary>
        /// 金額不符 : 2
        /// </summary>
        public const string AMOUNT_ERROR_TEXT = "金額不符";
        /// <summary>
        /// 檢碼不符 : 3
        /// </summary>
        public const string CHECKSUM_ERROR_TEXT = "檢碼不符";
        /// <summary>
        /// 銷問題檔 : 7
        /// </summary>
        public const string BY_PROBLEM_TEXT = "銷問題檔";
        /// <summary>
        /// 人工銷帳 : 8
        /// </summary>
        public const string BY_HAND_TEXT = "人工銷帳";
        /// <summary>
        /// 離線 : 9
        /// </summary>
        public const string RECEIVE_SELF_TEXT = "離線";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構檢查登入與功能狀態結果的代碼文字定義清單類別
        /// </summary>
        public CancelFlagCodeTexts()
        {
            base.Add(NONPAY, NONPAY_TEXT);
            base.Add(CANCELED, CANCELED_TEXT);
            base.Add(AMOUNT_ERROR, AMOUNT_ERROR_TEXT);
            base.Add(CHECKSUM_ERROR, CHECKSUM_ERROR_TEXT);
            base.Add(BY_PROBLEM, BY_PROBLEM_TEXT);
            base.Add(BY_HAND, BY_HAND_TEXT);
            base.Add(RECEIVE_SELF, RECEIVE_SELF_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得代碼對應的文字
        /// </summary>
        /// <param name="code">銷帳註記代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case NONPAY:
                    return NONPAY_TEXT;
                case CANCELED:
                    return CANCELED_TEXT;
                case AMOUNT_ERROR:
                    return AMOUNT_ERROR_TEXT;
                case CHECKSUM_ERROR:
                    return CHECKSUM_ERROR_TEXT;
                case BY_PROBLEM:
                    return BY_PROBLEM_TEXT;
                case BY_HAND:
                    return BY_HAND_TEXT;
                case RECEIVE_SELF:
                    return RECEIVE_SELF_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">銷帳註記代碼</param>
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
