using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 聯徵KP3資料狀態 代碼文字定義清單類別
    /// </summary>
    public class KP3StatusCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 建立資料後待匯出 : 10
        /// </summary>
        public const string STATUS10_CODE = "10";

        /// <summary>
        /// 匯出資料中 : 21
        /// </summary>
        public const string STATUS21_CODE = "21";

        /// <summary>
        /// 匯出資料後待回饋 : 20
        /// </summary>
        public const string STATUS20_CODE = "20";

        /// <summary>
        /// 回饋完成 : 40
        /// </summary>
        public const string STATUS40_CODE = "40";
        #endregion

        #region Const Text
        /// <summary>
        /// 建立資料後待匯出 : 10
        /// </summary>
        public const string STATUS10_TEXT = "建立資料後待匯出";

        /// <summary>
        /// 匯出資料中 : 21
        /// </summary>
        public const string STATUS21_TEXT = "匯出資料中";

        /// <summary>
        /// 匯出資料後待回饋 : 20
        /// </summary>
        public const string STATUS20_TEXT = "匯出資料後待回饋";

        /// <summary>
        /// 回饋完成 : 40
        /// </summary>
        public const string STATUS40_TEXT = "回饋完成";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 聯徵KP3資料狀態 代碼文字定義清單類別
        /// </summary>
        public KP3StatusCodeTexts()
        {
            base.Add(STATUS10_CODE, STATUS10_TEXT);
            base.Add(STATUS21_CODE, STATUS21_TEXT);
            base.Add(STATUS20_CODE, STATUS20_TEXT);
            base.Add(STATUS40_CODE, STATUS40_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得聯徵KP3資料狀態代碼對應的文字
        /// </summary>
        /// <param name="code">聯徵KP3資料狀態代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case STATUS10_CODE:
                    return STATUS10_TEXT;
                case STATUS21_CODE:
                    return STATUS21_TEXT;
                case STATUS20_CODE:
                    return STATUS20_TEXT;
                case STATUS40_CODE:
                    return STATUS40_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得聯徵KP3資料狀態代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">聯徵KP3資料狀態代碼</param>
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

    /// <summary>
    /// 報送KP3檔案狀態 代碼文字定義清單類別
    /// </summary>
    public class KP3RenderStatusCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 產生檔案後待上傳 : 20
        /// </summary>
        public const string STATUS20_CODE = "20";

        /// <summary>
        /// 上傳檔案中 : 31
        /// </summary>
        public const string STATUS31_CODE = "31";

        /// <summary>
        /// 上傳檔案失敗 : 32
        /// </summary>
        public const string STATUS32_CODE = "32";

        /// <summary>
        /// 上傳檔案後待回饋 : 30
        /// </summary>
        public const string STATUS30_CODE = "30";

        /// <summary>
        /// 回饋檔案中 : 41
        /// </summary>
        public const string STATUS41_CODE = "41";

        /// <summary>
        /// 回饋檔案失敗 : 42
        /// </summary>
        public const string STATUS42_CODE = "42";

        /// <summary>
        /// 回饋完成 : 40
        /// </summary>
        public const string STATUS40_CODE = "40";
        #endregion

        #region Const Text
        /// <summary>
        /// 產生檔案後待上傳 : 20
        /// </summary>
        public const string STATUS20_TEXT = "產生檔案後待上傳";

        /// <summary>
        /// 上傳檔案中 : 31
        /// </summary>
        public const string STATUS31_TEXT = "上傳檔案中";

        /// <summary>
        /// 上傳檔案失敗 : 32
        /// </summary>
        public const string STATUS32_TEXT = "上傳檔案失敗";

        /// <summary>
        /// 上傳檔案後待回饋 : 30
        /// </summary>
        public const string STATUS30_TEXT = "上傳檔案後待回饋";

        /// <summary>
        /// 回饋檔案中 : 41
        /// </summary>
        public const string STATUS41_TEXT = "回饋檔案中";

        /// <summary>
        /// 回饋檔案失敗 : 42
        /// </summary>
        public const string STATUS42_TEXT = "回饋檔案失敗";

        /// <summary>
        /// 回饋完成 : 40
        /// </summary>
        public const string STATUS40_TEXT = "回饋完成";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 報送KP3檔案狀態 代碼文字定義清單類別
        /// </summary>
        public KP3RenderStatusCodeTexts()
        {
            base.Add(STATUS20_CODE, STATUS20_TEXT);
            base.Add(STATUS31_CODE, STATUS31_TEXT);
            base.Add(STATUS32_CODE, STATUS32_TEXT);
            base.Add(STATUS30_CODE, STATUS30_TEXT);
            base.Add(STATUS41_CODE, STATUS41_TEXT);
            base.Add(STATUS42_CODE, STATUS42_TEXT);
            base.Add(STATUS40_CODE, STATUS40_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得報送KP3檔案狀態代碼對應的文字
        /// </summary>
        /// <param name="code">報送KP3檔案狀態代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case STATUS20_CODE:
                    return STATUS20_TEXT;
                case STATUS31_CODE:
                    return STATUS31_TEXT;
                case STATUS32_CODE:
                    return STATUS32_TEXT;
                case STATUS30_CODE:
                    return STATUS30_TEXT;
                case STATUS41_CODE:
                    return STATUS41_TEXT;
                case STATUS42_CODE:
                    return STATUS42_TEXT;
                case STATUS40_CODE:
                    return STATUS40_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得報送KP3檔案狀態代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">報送KP3檔案狀態代碼</param>
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
