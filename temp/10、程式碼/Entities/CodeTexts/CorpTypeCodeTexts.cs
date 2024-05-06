using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 學制的代碼文字定義清單類別
    /// </summary>
    public class CorpTypeCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 二專
        /// </summary>
        public const string CORPTYPE1 = "2";
        /// <summary>
        /// 五專
        /// </summary>
        public const string CORPTYPE2 = "5";
        /// <summary>
        /// 大學
        /// </summary>
        public const string CORPTYPE3 = "B1";
        /// <summary>
        /// 四技
        /// </summary>
        public const string CORPTYPE4 = "B2";
        /// <summary>
        /// 二技
        /// </summary>
        public const string CORPTYPE5 = "C";
        /// <summary>
        /// 博士
        /// </summary>
        public const string CORPTYPE6 = "D";
        /// <summary>
        /// 碩士
        /// </summary>
        public const string CORPTYPE7 = "M";
        #endregion

        #region Const Text
        /// <summary>
        /// 二專
        /// </summary>
        public const string CORPTYPE1_TEXT = "二專";
        /// <summary>
        /// 五專
        /// </summary>
        public const string CORPTYPE2_TEXT = "五專";
        /// <summary>
        /// 大學
        /// </summary>
        public const string CORPTYPE3_TEXT = "大學";
        /// <summary>
        /// 四技
        /// </summary>
        public const string CORPTYPE4_TEXT = "四技";
        /// <summary>
        /// 二技
        /// </summary>
        public const string CORPTYPE5_TEXT = "二技";
        /// <summary>
        /// 博士
        /// </summary>
        public const string CORPTYPE6_TEXT = "博士";
        /// <summary>
        /// 碩士
        /// </summary>
        public const string CORPTYPE7_TEXT = "碩士";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構檢查登入與功能狀態結果的代碼文字定義清單類別
        /// </summary>
        public CorpTypeCodeTexts()
        {
            base.Add(CORPTYPE1, CORPTYPE1_TEXT);
            base.Add(CORPTYPE2, CORPTYPE2_TEXT);
            base.Add(CORPTYPE3, CORPTYPE3_TEXT);
            base.Add(CORPTYPE4, CORPTYPE4_TEXT);
            base.Add(CORPTYPE5, CORPTYPE5_TEXT);
            base.Add(CORPTYPE6, CORPTYPE6_TEXT);
            base.Add(CORPTYPE7, CORPTYPE7_TEXT);
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
                case CORPTYPE1:
                    return CORPTYPE1_TEXT;
                case CORPTYPE2:
                    return CORPTYPE2_TEXT;
                case CORPTYPE3:
                    return CORPTYPE3_TEXT;
                case CORPTYPE4:
                    return CORPTYPE4_TEXT;
                case CORPTYPE5:
                    return CORPTYPE5_TEXT;
                case CORPTYPE6:
                    return CORPTYPE6_TEXT;
                case CORPTYPE7:
                    return CORPTYPE7_TEXT;
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
