using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 商家代號的代收種類代碼文字定義清單類別
    /// </summary>
    public class ReceiveKindCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 學雜費 : 1
        /// </summary>
        public const string SCHOOL = "1";

        /// <summary>
        /// 代收各項費用 (只上傳中國信託資料) : 2
        /// </summary>
        public const string UPCTCB = "2";
        #endregion

        #region Const Text
        /// <summary>
        /// 學雜費 : 1
        /// </summary>
        public const string SCHOOL_TEXT = "學雜費";

        /// <summary>
        /// 代收各項費用 (只上傳中國信託資料) : 2
        /// </summary>
        public const string UPCTCB_TEXT = "代收各項費用";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構商家代號的代收種類代碼文字定義清單類別
        /// </summary>
        public ReceiveKindCodeTexts()
        {
            base.Add(SCHOOL, SCHOOL_TEXT);
            base.Add(UPCTCB, UPCTCB_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得商家代號的代收種類代碼對應的文字
        /// </summary>
        /// <param name="code">商家代號的代收種類代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case SCHOOL:
                    return SCHOOL_TEXT;
                case UPCTCB:
                    return UPCTCB_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得商家代號的代收種類代碼與文字對照類別
        /// </summary>
        /// <param name="code">商家代號的代收種類代碼</param>
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
        /// 取得是否為定義的商家代號的代收種類代碼
        /// </summary>
        /// <param name="code">商家代號的代收種類代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            return !String.IsNullOrEmpty(GetText(code));
        }
        #endregion
    }
}
