using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 批次處理佇列的狀態代碼文字定義清單類別
    /// </summary>
    public class JobCubeStatusCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 待處理 : 1
        /// </summary>
        public const string WAIT = "1";

        /// <summary>
        /// 處理中 : 2
        /// </summary>
        public const string PROCESS = "2";

        /// <summary>
        /// 已完成 : 3
        /// </summary>
        public const string FINISH = "3";

        /// <summary>
        /// 已中止 : 4
        /// </summary>
        public const string BREAK = "4";
        #endregion

        #region Const Text
        /// <summary>
        /// 待處理 : 1
        /// </summary>
        public const string WAIT_TEXT = "待處理";

        /// <summary>
        /// 處理中 : 2
        /// </summary>
        public const string PROCESS_TEXT = "處理中";

        /// <summary>
        /// 已完成 : 3
        /// </summary>
        public const string FINISH_TEXT = "已完成";

        /// <summary>
        /// 已中止 : 4
        /// </summary>
        public const string BREAK_TEXT = "已中止";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構批次處理佇列的狀態代碼文字定義清單類別
        /// </summary>
        public JobCubeStatusCodeTexts()
        {
            base.Add(WAIT, WAIT_TEXT);
            base.Add(PROCESS, PROCESS_TEXT);
            base.Add(FINISH, FINISH_TEXT);
            base.Add(BREAK, BREAK_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得批次處理佇列的狀態代碼的文字
        /// </summary>
        /// <param name="code">批次處理佇列的狀態代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case WAIT:
                    return WAIT_TEXT;
                case PROCESS:
                    return PROCESS_TEXT;
                case FINISH:
                    return FINISH_TEXT;
                case BREAK:
                    return BREAK_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得批次處理佇列的狀態代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">批次處理佇列的狀態代碼</param>
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
        /// 取得指定的代碼是否為定義的批次處理佇列的狀態代碼
        /// </summary>
        /// <param name="code">指定要檢查的代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            return !String.IsNullOrEmpty(GetText(code));
        }

        /// <summary>
        /// 取得批次處理佇列的狀態代碼是否表示已處理結束
        /// </summary>
        /// <param name="code">批次處理佇列的狀態代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsEnding(string code)
        {
            return (code == FINISH || code == BREAK);
        }

        /// <summary>
        /// 取得表示已處理結束的批次處理佇列的狀態代碼陣列
        /// </summary>
        /// <returns>傳回狀態代碼陣列</returns>
        public static string[] GetEndingCode()
        {
            return new string[] { FINISH, BREAK };
        }

        /// <summary>
        /// 取得表示未處理結束的批次處理佇列的狀態代碼陣列
        /// </summary>
        /// <returns>傳回狀態代碼陣列</returns>
        public static string[] GetUnEndingCode()
        {
            return new string[] { WAIT, PROCESS };
        }
        #endregion
    }
}
