using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 問題註記的代碼文字定義清單類別
    /// </summary>
    public class ProblemFlagCodeTexts : CodeTextList
    {
        #region Const Code
        #region [MDY:20170925] 補銷帳失敗的問題註記
        /// <summary>
        /// 銷帳失敗 : 1
        /// </summary>
        public const string CANCEL_FAIL = "1";
        #endregion

        /// <summary>
        /// 金額不符 : 2
        /// </summary>
        public const string AMOUNT_ERROR = "2";
        /// <summary>
        /// 檢碼不符 : 3
        /// </summary>
        public const string CHECKSUM_ERROR = "3";
        /// <summary>
        /// 無此虛擬帳號 : 4
        /// </summary>
        public const string CANCELNO_ERROR = "4";

        #region [MDY:20170925] 修改重複繳費問題註記常數
        #region [Old]
        ///// <summary>
        ///// 重複繳費 : 5
        ///// </summary>
        //public const string DOUBLE_AMOUNT = "5";
        #endregion

        /// <summary>
        /// 重複繳費 : 5
        /// </summary>
        public const string DUPLICATE_PAYING = "5";
        #endregion

        /// <summary>
        /// 尚未全繳 : 6
        /// </summary>
        public const string LESS_AMOUNT = "6";
        #endregion

        #region Const Text
        #region [MDY:20170925] 補銷帳失敗的問題註記
        /// <summary>
        /// 銷帳失敗 : 1
        /// </summary>
        public const string CANCEL_FAIL_TEXT = "銷帳失敗";
        #endregion

        /// <summary>
        /// 金額不符 : 2
        /// </summary>
        public const string AMOUNT_ERROR_TEXT = "金額不符";
        /// <summary>
        /// 檢碼不符 : 3
        /// </summary>
        public const string CHECKSUM_ERROR_TEXT = "檢碼不符";
        /// 無此虛擬帳號 : 4
        /// </summary>
        public const string CANCELNO_ERROR_TEXT = "無此虛擬帳號";

        #region [MDY:20170925] 修改重複繳費問題註記常數
        #region [Old]
        ///// <summary>
        ///// 重複繳費 : 5
        ///// </summary>
        //public const string DOUBLE_AMOUNT_TEXT = "重複繳費";
        #endregion

        /// <summary>
        /// 重複繳費 : 5
        /// </summary>
        public const string DUPLICATE_PAYING_TEXT = "重複繳費";
        #endregion

        /// <summary>
        /// 尚未全繳 : 6
        /// </summary>
        public const string LESS_AMOUNT_TEXT = "尚未全繳";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構檢查登入與功能狀態結果的代碼文字定義清單類別
        /// </summary>
        public ProblemFlagCodeTexts()
        {
            #region [MDY:20170925] 補銷帳失敗的問題註記
            base.Add(CANCEL_FAIL, CANCEL_FAIL_TEXT);
            #endregion

            base.Add(AMOUNT_ERROR, AMOUNT_ERROR_TEXT);
            base.Add(CHECKSUM_ERROR, CHECKSUM_ERROR_TEXT);
            base.Add(CANCELNO_ERROR, CANCELNO_ERROR_TEXT);

            #region [MDY:20170925] 修改重複繳費問題註記常數
            #region [Old]
            //base.Add(DOUBLE_AMOUNT, DOUBLE_AMOUNT_TEXT);
            #endregion

            base.Add(DUPLICATE_PAYING, DUPLICATE_PAYING_TEXT);
            #endregion

            base.Add(LESS_AMOUNT, LESS_AMOUNT_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得代碼對應的文字
        /// </summary>
        /// <param name="code">模板適用單位類別代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                #region [MDY:20170925] 補銷帳失敗的問題註記
                case CANCEL_FAIL:
                    return CANCEL_FAIL_TEXT;
                #endregion

                case AMOUNT_ERROR:
                    return AMOUNT_ERROR_TEXT;
                case CHECKSUM_ERROR:
                    return CHECKSUM_ERROR_TEXT;
                case CANCELNO_ERROR:
                    return CANCELNO_ERROR_TEXT;

                #region [MDY:20170925] 修改重複繳費問題註記常數
                #region [Old]
                //case DOUBLE_AMOUNT:
                //    return DOUBLE_AMOUNT_TEXT;
                #endregion

                case DUPLICATE_PAYING:
                    return DUPLICATE_PAYING_TEXT;
                #endregion


                case LESS_AMOUNT:
                    return LESS_AMOUNT_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得代碼對應的代碼與文字對照類別
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
