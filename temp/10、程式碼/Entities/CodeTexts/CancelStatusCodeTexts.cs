using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 銷帳狀態的代碼文字定義清單類別
    /// </summary>
    public class CancelStatusCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 未繳款 : 0
        /// </summary>
        public const string NONPAY = "0";

        /// <summary>
        /// 已繳款 : 1
        /// </summary>
        public const string PAYED = "1";

        /// <summary>
        /// 已入帳 : 2
        /// </summary>
        public const string CANCELED = "2";
        #endregion

        #region Const Text
        /// <summary>
        /// 未繳款 : 0
        /// </summary>
        public const string NONPAY_TEXT = "未繳款";

        /// <summary>
        /// 已繳待銷 : 1
        /// </summary>
        public const string PAYED_TEXT = "已繳待銷";

        /// <summary>
        /// 已入帳 : 2
        /// </summary>
        public const string CANCELED_TEXT = "已入帳";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構銷帳狀態的代碼文字定義清單類別
        /// </summary>
        public CancelStatusCodeTexts()
        {
            base.Add(NONPAY, NONPAY_TEXT);
            base.Add(PAYED, PAYED_TEXT);
            base.Add(CANCELED, CANCELED_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得代碼對應的文字
        /// </summary>
        /// <param name="code">銷帳狀態代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case NONPAY:
                    return NONPAY_TEXT;
                case PAYED:
                    return PAYED_TEXT;
                case CANCELED:
                    return CANCELED_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">銷帳狀態代碼</param>
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
        /// 判斷代收日與入帳日，取得銷帳狀態的代碼與文字對照類別
        /// </summary>
        /// <param name="receiveDate">代收日</param>
        /// <param name="accountDate">入帳日</param>
        /// <returns>傳回銷帳狀態的代碼與文字對照類別</returns>
        public static CodeText GetCancelStatus(string receiveDate, string accountDate)
        {
            if (!String.IsNullOrWhiteSpace(accountDate))
            {
                return new CodeText(CancelStatusCodeTexts.CANCELED, CancelStatusCodeTexts.CANCELED_TEXT);
            }
            else if (!String.IsNullOrWhiteSpace(receiveDate))
            {
                return new CodeText(CancelStatusCodeTexts.PAYED, CancelStatusCodeTexts.PAYED_TEXT);
            }
            else
            {
                return new CodeText(CancelStatusCodeTexts.NONPAY, CancelStatusCodeTexts.NONPAY_TEXT);
            }
        }
        #endregion
    }
}
