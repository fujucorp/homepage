using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 銷帳(入帳)資料狀態的代碼文字定義清單類別
    /// </summary>
    public class CancelDebtsStatusCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 已銷帳 : 0
        /// </summary>
        public const string HAS_CANCELED_CODE = "0";

        /// <summary>
        /// 待處理 : 1
        /// </summary>
        public const string IS_WAITING_CODE = "1";

        /// <summary>
        /// 處理中 : 2
        /// </summary>
        public const string IS_PROCESSING_CODE = "2";

        /// <summary>
        /// 處理失敗 : 3
        /// </summary>
        public const string IS_FAILURE_CODE = "3";

        /// <summary>
        /// 免銷帳 : 4
        /// </summary>
        public const string IS_EXCLUDED_CODE = "4";

        /// <summary>
        /// 已更正 : 5
        /// </summary>
        public const string HAS_RECTIFIED_CODE = "5";

        /// <summary>
        /// 已被更正 : 6
        /// </summary>
        public const string BE_RECTIFIED_CODE = "6";
        #endregion

        #region Const Text
        /// <summary>
        /// 已銷帳 : 0
        /// </summary>
        public const string HAS_CANCELED_TEXT = "已銷帳";

        /// <summary>
        /// 待處理 : 1
        /// </summary>
        public const string IS_WAITING_TEXT = "待處理";

        /// <summary>
        /// 處理中 : 2
        /// </summary>
        public const string IS_PROCESSING_TEXT = "處理中";

        /// <summary>
        /// 處理失敗 : 3
        /// </summary>
        public const string IS_FAILURE_TEXT = "處理失敗";

        /// <summary>
        /// 免銷帳 : 4
        /// </summary>
        public const string IS_EXCLUDED_TEXT = "免銷帳";

        /// <summary>
        /// 已更正 : 5
        /// </summary>
        public const string HAS_RECTIFIED_TEXT = "已更正";

        /// <summary>
        /// 已被更正 : 6
        /// </summary>
        public const string BE_RECTIFIED_TEXT = "已被更正";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構銷帳狀態的代碼文字定義清單類別
        /// </summary>
        public CancelDebtsStatusCodeTexts()
        {
            base.Add(HAS_CANCELED_CODE, HAS_CANCELED_TEXT);
            base.Add(IS_WAITING_CODE, IS_WAITING_TEXT);
            base.Add(IS_PROCESSING_CODE, IS_PROCESSING_TEXT);
            base.Add(IS_FAILURE_CODE, IS_FAILURE_TEXT);
            base.Add(IS_EXCLUDED_CODE, IS_EXCLUDED_TEXT);
            base.Add(HAS_RECTIFIED_CODE, HAS_RECTIFIED_TEXT);
            base.Add(BE_RECTIFIED_CODE, BE_RECTIFIED_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得代碼對應的文字
        /// </summary>
        /// <param name="code">狀態代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case HAS_CANCELED_CODE:
                    return HAS_CANCELED_TEXT;
                case IS_WAITING_CODE:
                    return IS_WAITING_TEXT;
                case IS_PROCESSING_CODE:
                    return IS_PROCESSING_TEXT;
                case IS_FAILURE_CODE:
                    return IS_FAILURE_TEXT;
                case IS_EXCLUDED_CODE:
                    return IS_EXCLUDED_TEXT;
                case HAS_RECTIFIED_CODE:
                    return HAS_RECTIFIED_TEXT;
                case BE_RECTIFIED_CODE:
                    return BE_RECTIFIED_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">狀態代碼</param>
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
