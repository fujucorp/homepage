using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 銀行管理單位代碼名稱定義清單類別
    /// </summary>
    public class ManagerUnitCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 營業部 : 0040030
        /// </summary>
        public const string SALES = "0040030";

        /// <summary>
        /// 資訊室 : 0041020
        /// </summary>
        public const string IT = "0041020";
        #endregion

        #region Const Text
        /// <summary>
        /// 營業部 : 0040030
        /// </summary>
        public const string SALES_TEXT = "營業部";

        /// <summary>
        /// 資訊室 : 0041020
        /// </summary>
        public const string IT_TEXT = "資訊室";
        #endregion

        #region Constructor
        /// <summary>
        /// 銀行管理單位代碼名稱定義清單類別
        /// </summary>
        public ManagerUnitCodeTexts()
        {
            base.Add(SALES, SALES_TEXT);
            base.Add(IT, IT_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得銀行管理單位代碼對應的名稱
        /// </summary>
        /// <param name="code">銀行管理單位代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case SALES:
                    return SALES_TEXT;
                case IT:
                    return IT_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得銀行管理單位代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">銀行管理單位代碼</param>
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
        /// 取得指定的代碼是否為定義的銀行管理單位代碼
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
