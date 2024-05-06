using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 信用卡繳費平台代碼文字定義清單類別
    /// </summary>
    public class CCardApCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 財金 : 1
        /// </summary>
        public const string EZPOS = "1";

        #region 土銀沒有 e 政府
        ///// <summary>
        ///// e 政府 : 2
        ///// </summary>
        //public const string EGOV = "2";
        #endregion

        /// <summary>
        /// 中國信託 : 3
        /// </summary>
        public const string CTCB = "3";
        #endregion

        #region Const Text
        /// <summary>
        /// 財金 : 1
        /// </summary>
        public const string EZPOS_TEXT = "財金";

        #region 土銀沒有 e 政府
        ///// <summary>
        ///// e 政府 : 2
        ///// </summary>
        //public const string EGOV_TEXT = "e 政府";
        #endregion

        /// <summary>
        /// 中國信託 : 3
        /// </summary>
        public const string CTCB_TEXT = "中國信託";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構信用卡繳費平台代碼文字定義清單類別
        /// </summary>
        public CCardApCodeTexts()
        {
            base.Add(EZPOS, EZPOS_TEXT);

            #region 土銀沒有 e 政府
            //base.Add(EGOV, EGOV_TEXT);
            #endregion

            #region [TODO] 土銀未簽約暫不提供
            base.Add(CTCB, CTCB_TEXT);
            #endregion
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得信用卡繳費平台代碼文字對應的文字
        /// </summary>
        /// <param name="code">設定代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case EZPOS:
                    return EZPOS_TEXT;

                #region 土銀沒有 e 政府
                //case EGOV:
                //    return EGOV_TEXT;
                #endregion

                case CTCB:
                    return CTCB_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得設定代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">設定代碼</param>
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
