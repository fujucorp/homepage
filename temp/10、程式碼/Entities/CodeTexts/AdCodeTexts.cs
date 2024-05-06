using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 廣告版位代碼文字定義清單類別
    /// </summary>
    public class AdCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 首頁中間大圖廣告 1 : AD001
        /// </summary>
        public const string AD001 = "AD001";

        /// <summary>
        /// 首頁右側小圖廣告 2 : AD002
        /// </summary>
        public const string AD002 = "AD002";

        /// <summary>
        /// 首頁右側小圖廣告 3 : AD003
        /// </summary>
        public const string AD003 = "AD003";

        /// <summary>
        /// 首頁右側小圖廣告 4 : AD004
        /// </summary>
        public const string AD004 = "AD004";

        /// <summary>
        /// 首頁右側小圖廣告 5 : AD005
        /// </summary>
        public const string AD005 = "AD005";
        #endregion

        #region Const Text
        /// <summary>
        /// 首頁中間大圖廣告 1 : AD001
        /// </summary>
        public const string AD001_TEXT = "首頁中間大圖廣告 1";

        /// <summary>
        /// 首頁右側小圖廣告 2 : AD002
        /// </summary>
        public const string AD002_TEXT = "首頁右側小圖廣告 2";

        /// <summary>
        /// 首頁右側小圖廣告 3 : AD003
        /// </summary>
        public const string AD003_TEXT = "首頁右側小圖廣告 3";

        /// <summary>
        /// 首頁右側小圖廣告 4 : AD004
        /// </summary>
        public const string AD004_TEXT = "首頁右側小圖廣告 4";

        /// <summary>
        /// 首頁右側小圖廣告 5 : AD005
        /// </summary>
        public const string AD005_TEXT = "首頁右側小圖廣告 5";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構廣告版位代碼文字定義清單類別
        /// </summary>
        public AdCodeTexts()
        {
            base.Add(AD001, AD001_TEXT);
            base.Add(AD002, AD002_TEXT);
            base.Add(AD003, AD003_TEXT);
            base.Add(AD004, AD004_TEXT);
            base.Add(AD005, AD005_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得廣告版位代碼對應的文字
        /// </summary>
        /// <param name="code">廣告版位</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case AD001:
                    return AD001_TEXT;
                case AD002:
                    return AD002_TEXT;
                case AD003:
                    return AD003_TEXT;
                case AD004:
                    return AD004_TEXT;
                case AD005:
                    return AD005_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得廣告版位代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">廣告版位</param>
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
