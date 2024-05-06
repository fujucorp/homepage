using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 模板適用單位類別的代碼文字定義清單類別 (台企銀固定只用 1 : 學校)
    /// </summary>
    public class BillFormUserCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 學校 : 1
        /// </summary>
        public const string SCHOOL = "1";

        /// <summary>
        /// 企業 : 2
        /// </summary>
        public const string CORP = "2";
        #endregion

        #region Const Text
        /// <summary>
        /// 學校 : 1
        /// </summary>
        public const string SCHOOL_TEXT = "學校";

        /// <summary>
        /// 企業 : 2
        /// </summary>
        public const string CORP_TEXT = "企業";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構模板適用單位類別的代碼文字定義清單類別
        /// </summary>
        public BillFormUserCodeTexts()
        {
            base.Add(SCHOOL, SCHOOL_TEXT);
            base.Add(CORP, CORP_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得模板適用單位類別代碼對應的文字
        /// </summary>
        /// <param name="code">模板適用單位類別代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case SCHOOL:
                    return SCHOOL_TEXT;
                case CORP:
                    return CORP_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得模板適用單位類別代碼對應的代碼與文字對照類別
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
