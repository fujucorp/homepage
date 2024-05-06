using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// (群組的)權限角色代碼名稱對照集合類別
    /// </summary>
    public class RoleTypeCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 經辦 : 2
        /// </summary>
        public const string USER = "2";

        /// <summary>
        /// 主管 : 3
        /// </summary>
        public const string MANAGER = "3";
        #endregion

        #region Const Text
        /// <summary>
        /// 經辦 : 2
        /// </summary>
        public const string USER_TEXT = "經辦";

        /// <summary>
        /// 主管 : 3
        /// </summary>
        public const string MANAGER_TEXT = "主管";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構(群組的)權限角色代碼名稱對照集合類別
        /// </summary>
        public RoleTypeCodeTexts()
        {
            base.Add(USER, USER_TEXT);
            base.Add(MANAGER, MANAGER_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得指定(群組的)權限角色代碼的文字
        /// </summary>
        /// <param name="code">指定(群組的)權限角色代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case USER:
                    return USER_TEXT;
                case MANAGER:
                    return MANAGER_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得指定(群組的)權限角色代碼的代碼名稱對照類別
        /// </summary>
        /// <param name="code">指定(群組的)權限角色代碼</param>
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
        /// 取得指定的代碼是否為定義的(群組的)權限角色代碼
        /// </summary>
        /// <param name="code">指定代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            return !String.IsNullOrEmpty(GetText(code));
        }

        /// <summary>
        /// 取得指定(群組的)權限角色代碼是否為主管
        /// </summary>
        /// <param name="code">指定(群組的)權限角色代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsManager(string code)
        {
            return (code == MANAGER);
        }

        /// <summary>
        /// 取得指定(群組的)權限角色代碼是否為經辦
        /// </summary>
        /// <param name="code">指定(群組的)權限角色代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsUser(string code)
        {
            return (code == USER);
        }
        #endregion
    }
}
