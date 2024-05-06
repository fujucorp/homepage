using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 補單註記的代碼文字定義清單類別
    /// 補單註記 (9=原公式 / 7=原公式但已銷帳 / 8=強迫更改金額 / 6=強迫更改但已銷帳)
    /// </summary>
    public class ReissueFlagCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 原公式 : 9
        /// </summary>
        public const string ORGCAL = "9";
        /// <summary>
        /// 原公式但已銷帳 : 7
        /// </summary>
        public const string ORGCALCANCEL = "7";
        /// <summary>
        /// 強迫更改金額 : 8
        /// </summary>
        public const string CHANGAMOUNT = "8";
        /// <summary>
        /// 強迫更改但已銷帳 : 6
        /// </summary>
        public const string CHANGCANCEL = "6";
        #endregion

        #region Const Text
        /// <summary>
        /// 原公式 : 9
        /// </summary>
        public const string ORGCAL_TEXT = "原公式";
        /// <summary>
        /// 原公式但已銷帳 : 7
        /// </summary>
        public const string ORGCALCANCEL_TEXT = "原公式但已銷帳";
        /// <summary>
        /// 強迫更改金額 : 8
        /// </summary>
        public const string CHANGAMOUNT_TEXT = "強迫更改金額";
        /// <summary>
        /// 強迫更改但已銷帳 : 6
        /// </summary>
        public const string CHANGCANCEL_TEXT = "強迫更改但已銷帳";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構檢查登入與功能狀態結果的代碼文字定義清單類別
        /// </summary>
        public ReissueFlagCodeTexts()
        {
            base.Add(ORGCAL, ORGCAL_TEXT);
            base.Add(ORGCALCANCEL, ORGCALCANCEL_TEXT);
            base.Add(CHANGAMOUNT, CHANGAMOUNT_TEXT);
            base.Add(CHANGCANCEL, CHANGCANCEL_TEXT);
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
                case ORGCAL:
                    return ORGCAL_TEXT;
                case ORGCALCANCEL:
                    return ORGCALCANCEL_TEXT;
                case CHANGAMOUNT:
                    return CHANGAMOUNT_TEXT;
                case CHANGCANCEL:
                    return CHANGCANCEL_TEXT;
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
