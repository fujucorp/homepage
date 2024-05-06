using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// EAI 資料狀態代碼文字定義清單類別
    /// </summary>
    public class EAIDataStatusCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 正常 (啟用) : 0
        /// </summary>
        public const string NORMAL = "0";

        /// <summary>
        /// 待處理 : 1
        /// </summary>
        public const string WAIT = "1";

        /// <summary>
        /// 處理中 : 2
        /// </summary>
        public const string PROCESS = "2";

        /// <summary>
        /// 處理成功 : 3
        /// </summary>
        public const string SUCCESS = "3";

        /// <summary>
        /// 已中止 : 4
        /// </summary>
        public const string BREAK = "4";

        /// <summary>
        /// 處理失敗 : 5
        /// </summary>
        public const string FAILURE = "5";
        #endregion

        #region Const Text
        /// <summary>
        /// 正常 (啟用) : 0
        /// </summary>
        public const string NORMAL_TEXT = "正常";

        /// <summary>
        /// 待處理 : 1
        /// </summary>
        public const string WAIT_TEXT = "待處理";

        /// <summary>
        /// 處理中 : 2
        /// </summary>
        public const string PROCESS_TEXT = "處理中";

        /// <summary>
        /// 處理成功 : 3
        /// </summary>
        public const string SUCCESS_TEXT = "處理成功";

        /// <summary>
        /// 已中止 : 4
        /// </summary>
        public const string BREAK_TEXT = "已中止";

        /// <summary>
        /// 處理失敗 : 5
        /// </summary>
        public const string FAILURE_TEXT = "處理失敗";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 EAI 資料狀態代碼文字定義清單類別
        /// </summary>
        public EAIDataStatusCodeTexts()
        {
            base.Add(NORMAL, NORMAL_TEXT);
            base.Add(WAIT, WAIT_TEXT);
            base.Add(PROCESS, PROCESS_TEXT);
            base.Add(SUCCESS, SUCCESS_TEXT);
            base.Add(BREAK, BREAK_TEXT);
            base.Add(FAILURE, FAILURE_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得 EAI 資料狀態代碼對應的文字
        /// </summary>
        /// <param name="code">資料狀態代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case NORMAL:
                    return NORMAL_TEXT;
                case WAIT:
                    return WAIT_TEXT;
                case PROCESS:
                    return PROCESS_TEXT;
                case SUCCESS:
                    return SUCCESS_TEXT;
                case BREAK:
                    return BREAK_TEXT;
                case FAILURE:
                    return FAILURE_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得 EAI 資料狀態代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">資料狀態代碼</param>
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
