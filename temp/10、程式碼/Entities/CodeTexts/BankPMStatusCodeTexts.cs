using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 檔案庫的狀態代碼文字定義清單類別
    /// </summary>
    public class BankPMStatusCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 狀態 B : B
        /// </summary>
        public const string B = "1";
        #endregion

        #region Const Text
        /// <summary>
        /// 狀態 B : B
        /// </summary>
        public const string B_TEXT = "狀態 B";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構檔案庫的狀態代碼文字定義清單類別
        /// </summary>
        public BankPMStatusCodeTexts()
        {
            base.Add(B, B_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得檔案庫的狀態代碼的文字
        /// </summary>
        /// <param name="code">檔案庫的狀態代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case B:
                    return B_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得檔案庫的狀態代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">檔案庫的狀態代碼</param>
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
        /// 取得指定的代碼是否為定義的檔案庫的狀態代碼
        /// </summary>
        /// <param name="code">指定要檢查的代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            return !String.IsNullOrEmpty(GetText(code));
        }
        #endregion
    }
}
