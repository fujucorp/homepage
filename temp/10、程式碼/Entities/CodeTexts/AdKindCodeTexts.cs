using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 廣告種類代碼文字定義清單類別
    /// </summary>
    public class AdKindCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 圖檔內容 : C
        /// </summary>
        public const string CONTENT = "C";

        /// <summary>
        /// 圖檔網址 : U
        /// </summary>
        public const string URL = "U";
        #endregion

        #region Const Text
        /// <summary>
        /// 圖檔內容 : C
        /// </summary>
        public const string CONTENT_TEXT = "圖檔內容";

        /// <summary>
        /// 圖檔網址 : U
        /// </summary>
        public const string URL_TEXT = "圖檔網址";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構廣告種類代碼文字定義清單類別
        /// </summary>
        public AdKindCodeTexts()
        {
            base.Add(CONTENT, CONTENT_TEXT);
            base.Add(URL, URL_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得廣告種類代碼對應的文字
        /// </summary>
        /// <param name="code">廣告種類</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case CONTENT:
                    return CONTENT_TEXT;
                case URL:
                    return URL_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得廣告種類代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">廣告種類</param>
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
