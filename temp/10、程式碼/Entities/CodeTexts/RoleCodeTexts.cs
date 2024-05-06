using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fuju;

namespace Entities
{
    /// <summary>
    /// (群組的)群組角色代碼名稱對照集合類別
    /// </summary>
    public class RoleCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 行員 : 1
        /// </summary>
        public const string STAFF = "1";

        /// <summary>
        /// 學校 : 2
        /// </summary>
        public const string SCHOOL = "2";
        #endregion

        #region Const Text
        /// <summary>
        /// 行員 : 1
        /// </summary>
        public const string STAFF_TEXT = "行員";

        /// <summary>
        /// 學校 : 2
        /// </summary>
        public const string SCHOOL_TEXT = "學校";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構(群組的)群組角色代碼名稱對照集合類別
        /// </summary>
        public RoleCodeTexts()
        {
            base.Add(STAFF, STAFF_TEXT);
            base.Add(SCHOOL, SCHOOL_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得指定(群組的)群組角色代碼的文字
        /// </summary>
        /// <param name="code">指定(群組的)群組角色代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case STAFF:
                    return STAFF_TEXT;
                case SCHOOL:
                    return SCHOOL_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得指定(群組的)群組角色代碼的代碼名稱對照類別
        /// </summary>
        /// <param name="code">指定(群組的)群組角色代碼</param>
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
        /// 取得指定的代碼是否為定義的(群組的)群組角色代碼
        /// </summary>
        /// <param name="code">指定代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            return !String.IsNullOrEmpty(GetText(code));
        }

        /// <summary>
        /// 取得指定(群組的)群組角色代碼是否為行員
        /// </summary>
        /// <param name="code">指定(群組的)群組角色代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsStaffUser(string code)
        {
            return (code == STAFF);
        }

        /// <summary>
        /// 取得指定(群組的)群組角色代碼是否為學校
        /// </summary>
        /// <param name="code">指定(群組的)群組角色代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsSchoolUser(string code)
        {
            return (code == SCHOOL);
        }
        #endregion
    }
}
