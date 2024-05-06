using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 學生登入依據類別代碼文字定義清單類別
    /// </summary>
    public class LoginKeyTypeCodeTexts : CodeTextList
    {
        public const string DEFAULT_CODE = BIRTHDAY;
        #region
        /// <summary>
        /// 預設學生登入依據類別
        /// </summary>
        public static readonly CodeText Default = GetCodeText(DEFAULT_CODE);
        #endregion

        #region Const Code
        /// <summary>
        /// 身分證字號 : n
        /// </summary>
        public const string PERSONAL_ID = "n";

        /// <summary>
        /// 生日 : y
        /// </summary>
        public const string BIRTHDAY = "y";
        #endregion

        #region Const Text
        /// <summary>
        /// 身分證字號 : 0
        /// </summary>
        public const string PERSONAL_ID_TEXT = "身分證字號";

        /// <summary>
        /// 生日 : 1
        /// </summary>
        public const string BIRTHDAY_TEXT = "生日";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構學生登入依據類別代碼文字定義清單類別
        /// </summary>
        public LoginKeyTypeCodeTexts()
        {
            base.Add(PERSONAL_ID, PERSONAL_ID_TEXT);
            base.Add(BIRTHDAY, BIRTHDAY_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得學生登入依據類別代碼對應的文字
        /// </summary>
        /// <param name="code">學生登入依據類別代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case PERSONAL_ID:
                    return PERSONAL_ID_TEXT;
                case BIRTHDAY:
                    return BIRTHDAY_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得資學生登入依據類別代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">學生登入依據類別代碼</param>
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
