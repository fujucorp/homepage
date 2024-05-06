using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 使用者身分別代碼名稱對照集合類別
    /// </summary>
    public class UserQualCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 銀行使用者 : 1
        /// </summary>
        public const string BANK = "1";

        /// <summary>
        /// 學校使用者 : 2
        /// </summary>
        public const string SCHOOL = "2";

        /// <summary>
        /// 學生使用者 : 3
        /// </summary>
        public const string STUDENT = "3";
        #endregion

        #region Const Text
        /// <summary>
        /// 銀行使用者 : 1
        /// </summary>
        public const string BANK_TEXT = "銀行使用者";

        /// <summary>
        /// 學校使用者 : 2
        /// </summary>
        public const string SCHOOL_TEXT = "學校使用者";

        /// <summary>
        /// 學生使用者 : 3
        /// </summary>
        public const string STUDENT_TEXT = "學生使用者";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構使用者身分別代碼名稱對照集合類別
        /// </summary>
        public UserQualCodeTexts()
        {
            base.Add(BANK, BANK_TEXT);
            base.Add(SCHOOL, SCHOOL_TEXT);
            base.Add(STUDENT, STUDENT_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得指定使用者身分別代碼的文字
        /// </summary>
        /// <param name="code">指定使用者身分別代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case BANK:
                    return BANK_TEXT;
                case SCHOOL:
                    return SCHOOL_TEXT;
                case STUDENT:
                    return STUDENT_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得指定使用者身分別代碼的代碼名稱對照類別
        /// </summary>
        /// <param name="code">指定使用者身分別代碼</param>
        /// <returns>傳回對應的代碼名稱對照類別，無對應則傳回 null</returns>
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
        /// 取得指定的代碼是否為定義的使用者身分別代碼
        /// </summary>
        /// <param name="code">指定代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            return !String.IsNullOrEmpty(GetText(code));
        }

        /// <summary>
        /// 取得指定使用者身分別代碼是否為銀行使用者
        /// </summary>
        /// <param name="code">指定使用者身分別代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsBankUser(string code)
        {
            return (code == BANK);
        }

        /// <summary>
        /// 取得指定使用者身分別代碼是否為學校使用者
        /// </summary>
        /// <param name="code">指定使用者身分別代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsSchoolUser(string code)
        {
            return (code == SCHOOL);
        }

        /// <summary>
        /// 取得指定使用者身分別代碼是否為學生使用者
        /// </summary>
        /// <param name="code">使用者身分別代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsStudentUser(string code)
        {
            return (code == STUDENT);
        }
        #endregion
    }
}
