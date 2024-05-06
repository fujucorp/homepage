using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 流程申請種類代碼文字定義清單類別
    /// </summary>
    public class ApplyKindCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 新增資料 : I
        /// </summary>
        public const string INSERT = "I";

        /// <summary>
        /// 更新資料 : U
        /// </summary>
        public const string UPDATE = "U";

        /// <summary>
        /// 刪除資料 : D
        /// </summary>
        public const string DELETE = "D";
        #endregion

        #region Const Text
        /// <summary>
        /// 新增資料 : I
        /// </summary>
        public const string INSERT_TEXT = "新增資料";

        /// <summary>
        /// 更新資料 : U
        /// </summary>
        public const string UPDATE_TEXT = "更新資料";

        /// <summary>
        /// 刪除資料 : D
        /// </summary>
        public const string DELETE_TEXT = "刪除資料";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構流程申請種類代碼文字定義清單類別
        /// </summary>
        public ApplyKindCodeTexts()
        {
            base.Add(INSERT, INSERT_TEXT);
            base.Add(UPDATE, UPDATE_TEXT);
            base.Add(DELETE, DELETE_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得流程申請種類代碼對應的文字
        /// </summary>
        /// <param name="code">流程申請種類代碼</param>
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
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得流程申請種類代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">流程申請種類代碼</param>
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
