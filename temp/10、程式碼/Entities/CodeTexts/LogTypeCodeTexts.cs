using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 資料庫處理操作代碼文字定義清單類別
    /// </summary>
    public class LogTypeCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 新增 : A
        /// </summary>
        public const string INSERT = "A";

        /// <summary>
        /// 修改 : U
        /// </summary>
        public const string UPDATE = "U";

        /// <summary>
        /// 刪除 : D
        /// </summary>
        public const string DELETE = "D";

        /// <summary>
        /// 查詢 : S
        /// </summary>
        public const string SELECT = "S";

        /// <summary>
        /// 處理 : E
        /// </summary>
        public const string EXECUTE = "E";
        #endregion

        #region Const Text
        /// <summary>
        /// 新增 :A
        /// </summary>
        public const string INSERT_TEXT = "新增";

        /// <summary>
        /// 修改 : U
        /// </summary>
        public const string UPDATE_TEXT = "修改";

        /// <summary>
        /// 刪除 : D
        /// </summary>
        public const string DELETE_TEXT = "刪除";

        /// <summary>
        /// 查詢 : S
        /// </summary>
        public const string SELECT_TEXT = "查詢";

        /// <summary>
        /// 處理 : E
        /// </summary>
        public const string EXECUTE_TEXT = "處理";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構資料庫處理操作代碼文字定義清單類別
        /// </summary>
        public LogTypeCodeTexts()
        {
            base.Add(INSERT, INSERT_TEXT);
            base.Add(UPDATE, UPDATE_TEXT);
            base.Add(DELETE, DELETE_TEXT);
            base.Add(SELECT, SELECT_TEXT);
            base.Add(EXECUTE, EXECUTE_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得資料庫處理操作代碼對應的文字
        /// </summary>
        /// <param name="code">資料庫處理操作代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case INSERT:
                    return INSERT_TEXT;
                case UPDATE:
                    return UPDATE_TEXT;
                case DELETE:
                    return DELETE_TEXT;
                case SELECT:
                    return SELECT_TEXT;
                case EXECUTE:
                    return EXECUTE_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得資料庫處理操作代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">資料庫處理操作代碼</param>
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
