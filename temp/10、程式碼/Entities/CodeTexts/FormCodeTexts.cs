using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 流程表單代碼文字定義清單類別
    /// </summary>
    public class FormCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 權限管理 : S5200002
        /// </summary>
        public const string S5200002 = "S5200002";

        /// <summary>
        /// 群組管理 : S5200003
        /// </summary>
        public const string S5200003 = "S5200003";

        /// <summary>
        /// 使用者管理 : S5300001
        /// </summary>
        public const string S5300001 = "S5300001";
        #endregion

        #region Const Text
        /// <summary>
        /// 權限管理 : S5200002
        /// </summary>
        public const string S5200002_TEXT = "權限管理";

        /// <summary>
        /// 群組管理 : S5200003
        /// </summary>
        public const string S5200003_TEXT = "群組管理";

        /// <summary>
        /// 使用者管理 : S5300001
        /// </summary>
        public const string S5300001_TEXT = "使用者管理";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構流程表單代碼文字定義清單類別
        /// </summary>
        public FormCodeTexts()
        {
            base.Add(S5200002, S5200002_TEXT);
            base.Add(S5200003, S5200003_TEXT);
            base.Add(S5300001, S5300001_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得流程表單代碼對應的文字
        /// </summary>
        /// <param name="code">流程表單代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case S5200002:
                    return S5200002_TEXT;
                case S5200003:
                    return S5200003_TEXT;
                case S5300001:
                    return S5300001_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得流程表單代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">流程表單代碼</param>
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
