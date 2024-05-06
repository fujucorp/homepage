using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// ExportFile 匯出種類代碼文字定義清單類別
    /// </summary>
    public class ExportFileKindCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 產生學生繳費資料媒體檔(跨學年) : 01
        /// </summary>
        public const string B2100006 = "01";

        /// <summary>
        /// 產生銷帳資料檔(跨學年) : 02
        /// </summary>
        public const string C3700008 = "02";
        #endregion

        #region Const Text
        /// <summary>
        /// 產生學生繳費資料媒體檔(跨學年) : 01
        /// </summary>
        public const string B2100006_TEXT = "產生學生繳費資料媒體檔(跨學年)";

        /// <summary>
        /// 產生銷帳資料檔(跨學年) : 02
        /// </summary>
        public const string C3700008_TEXT = "產生銷帳資料檔(跨學年)";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 ExportFile 匯出種類代碼文字定義清單類別
        /// </summary>
        public ExportFileKindCodeTexts()
        {
            base.Add(B2100006, B2100006_TEXT);
            base.Add(C3700008, C3700008_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得 ExportFile 匯出種類代碼對應的文字
        /// </summary>
        /// <param name="code">匯出種類代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case B2100006:
                    return B2100006_TEXT;
                case C3700008:
                    return C3700008_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得 ExportFile 匯出種類代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">匯出種類代碼</param>
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
        /// 取得指定代碼是否為定義的 ExportConfig 設定種類代碼
        /// </summary>
        /// <param name="code">指定代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            switch (code)
            {
                case B2100006:
                case C3700008:
                    return true;
            }
            return false;
        }
        #endregion
    }
}
