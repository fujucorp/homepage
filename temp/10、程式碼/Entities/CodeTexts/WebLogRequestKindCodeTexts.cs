using System;
using System.Collections.Generic;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 網站日誌 Request 類別 代碼文字定義清單類別
    /// </summary>
    public class WebLogRequestKindCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 新增 : A
        /// </summary>
        public const string INSERT_CODE = "A";
        /// <summary>
        /// 修改 : M
        /// </summary>
        public const string MODIFY_CODE = "M";
        /// <summary>
        /// 刪除 : D
        /// </summary>
        public const string DELETE_CODE = "D";
        /// <summary>
        /// 查詢 : Q (多筆資料的清單)
        /// </summary>
        public const string QUERY_CODE = "Q";
        /// <summary>
        /// 檢視 : V (單筆資料的顯示)
        /// </summary>
        public const string VIEW_CODE = "V";
        /// <summary>
        /// 匯出 : E
        /// </summary>
        public const string EXPORT_CODE = "E";
        /// <summary>
        /// 匯入 : I
        /// </summary>
        public const string IMPORT_CODE = "I";
        #endregion

        #region Const Text
        /// <summary>
        /// 新增 : A
        /// </summary>
        public const string INSERT_TEXT = "新增";
        /// <summary>
        /// 修改 : M
        /// </summary>
        public const string MODIFY_TEXT = "修改";
        /// <summary>
        /// 刪除 : D
        /// </summary>
        public const string DELETE_TEXT = "刪除";
        /// <summary>
        /// 查詢 : Q (多筆資料的清單)
        /// </summary>
        public const string QUERY_TEXT = "查詢";
        /// <summary>
        /// 檢視 : V (單筆資料的顯示)
        /// </summary>
        public const string VIEW_TEXT = "檢視";
        /// <summary>
        /// 匯出 : E
        /// </summary>
        public const string EXPORT_TEXT = "匯出";
        /// <summary>
        /// 匯入 : I
        /// </summary>
        public const string IMPORT_TEXT = "匯入";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 網站日誌 Request 類別 代碼文字定義清單類別 物件
        /// </summary>
        public WebLogRequestKindCodeTexts()
        {
            base.Add(INSERT_CODE, INSERT_TEXT);
            base.Add(MODIFY_CODE, MODIFY_TEXT);
            base.Add(DELETE_CODE, DELETE_TEXT);
            base.Add(QUERY_CODE, QUERY_TEXT);
            base.Add(VIEW_CODE, VIEW_TEXT);
            base.Add(EXPORT_CODE, EXPORT_TEXT);
            base.Add(IMPORT_CODE, IMPORT_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得指定代碼對應的文字
        /// </summary>
        /// <param name="code">指定代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case INSERT_CODE:
                    return INSERT_TEXT;
                case MODIFY_CODE:
                    return MODIFY_TEXT;
                case DELETE_CODE:
                    return DELETE_TEXT;
                case QUERY_CODE:
                    return QUERY_TEXT;
                case VIEW_CODE:
                    return VIEW_TEXT;
                case EXPORT_CODE:
                    return EXPORT_TEXT;
                case IMPORT_CODE:
                    return IMPORT_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得指定代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">指定代碼</param>
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
        /// 取得指定代碼陣列對應的代碼與文字對照類別陣列
        /// </summary>
        /// <param name="codes">定代碼陣列</param>
        /// <returns>傳回對應的代碼與文字對照類別陣列，無對應則傳回空陣列</returns>
        public static CodeText[] GetCodeTexts(string[] codes)
        {
            if (codes == null || codes.Length == 0)
            {
                return new CodeText[0];
            }

            List<CodeText> list = new List<CodeText>(codes.Length);
            foreach (string code in codes)
            {
                if (!String.IsNullOrWhiteSpace(code))
                {
                    string text = GetText(code);
                    if (!string.IsNullOrEmpty(text))
                    {
                        list.Add(new CodeText(code, text));
                    }
                }
            }
            return list.ToArray();
        }
        #endregion
    }
}
