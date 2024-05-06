using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 模板版本類別的代碼文字定義清單類別
    /// </summary>
    public class BillFormEditionCodeTexts : CodeTextList
    {
        #region [Old]
        //#region Const Code
        ///// <summary>
        ///// 系統公版 : 1
        ///// </summary>
        //public const string SYSTEM = "1";

        ///// <summary>
        ///// 學制、類別公版 : 2
        ///// </summary>
        //public const string CATEGORY = "2";

        ///// <summary>
        ///// 專屬 : 3
        ///// </summary>
        //public const string EXCLUSIVE = "3";
        //#endregion

        //#region Const Text
        ///// <summary>
        ///// 系統公版 : 1
        ///// </summary>
        //public const string SYSTEM_TEXT = "系統公版";

        ///// <summary>
        ///// 學制、類別公版 : 2
        ///// </summary>
        //public const string CATEGORY_TEXT = "學制、類別公版";

        ///// <summary>
        ///// 專屬 : 3
        ///// </summary>
        //public const string EXCLUSIVE_TEXT = "收據";
        //#endregion
        #endregion

        #region Const Code
        /// <summary>
        /// 公版 : 2
        /// </summary>
        public const string PUBLIC = "2";

        /// <summary>
        /// 專屬 : 3
        /// </summary>
        public const string PRIVATE = "3";
        #endregion

        #region Const Text
        /// <summary>
        /// 公版 : 2
        /// </summary>
        public const string PUBLIC_TEXT = "公版";

        /// <summary>
        /// 專屬 : 3
        /// </summary>
        public const string PRIVATE_TEXT = "專屬";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構模板版本類別的代碼文字定義清單類別
        /// </summary>
        public BillFormEditionCodeTexts()
        {
            #region [Old]
            //base.Add(SYSTEM, SYSTEM_TEXT);
            //base.Add(CATEGORY, CATEGORY_TEXT);
            //base.Add(EXCLUSIVE, EXCLUSIVE_TEXT);
            #endregion

            base.Add(PUBLIC, PUBLIC_TEXT);
            base.Add(PRIVATE, PRIVATE_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得模板版本類別代碼對應的文字
        /// </summary>
        /// <param name="code">模板版本類別代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            #region [Old]
            //switch (code)
            //{
            //    case SYSTEM:
            //        return SYSTEM_TEXT;
            //    case CATEGORY:
            //        return CATEGORY_TEXT;
            //    case EXCLUSIVE:
            //        return EXCLUSIVE_TEXT;
            //}
            #endregion

            switch (code)
            {
                case PUBLIC:
                    return PUBLIC_TEXT;
                case PRIVATE:
                    return PRIVATE_TEXT;
            }

            return string.Empty;
        }

        /// <summary>
        /// 取得模板版本類別代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">模板版本類別代碼</param>
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
