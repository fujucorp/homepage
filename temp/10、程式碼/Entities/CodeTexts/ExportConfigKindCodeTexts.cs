using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// ExportConfig 設定種類代碼文字定義清單類別
    /// </summary>
    public class ExportConfigKindCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 學生繳費單資料 : 01
        /// </summary>
        public const string StudentBill = "01";

        /// <summary>
        /// 銷帳資料 : 02
        /// </summary>
        public const string CanceledBill = "02";

        /// <summary>
        /// 學生繳費資料媒體檔(跨學年) : 03
        /// </summary>
        public const string StudentBill2 = "03";
        #endregion

        #region Const Text
        /// <summary>
        /// 學生繳費單資料 : 01
        /// </summary>
        public const string StudentBill_TEXT = "學生繳費單資料";

        /// <summary>
        /// 銷帳資料 : 02
        /// </summary>
        public const string CanceledBill_TEXT = "銷帳資料";

        /// <summary>
        /// 學生繳費單資料(跨學年) : 03
        /// </summary>
        public const string StudentBill2_TEXT = "學生繳費單資料(跨學年)";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 ExportConfig 設定種類代碼文字定義清單類別
        /// </summary>
        public ExportConfigKindCodeTexts()
        {
            base.Add(StudentBill, StudentBill_TEXT);
            base.Add(CanceledBill, CanceledBill_TEXT);
            base.Add(StudentBill2, StudentBill2_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得 ExportConfig 設定種類代碼對應的文字
        /// </summary>
        /// <param name="code">匯出種類代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case StudentBill:
                    return StudentBill_TEXT;
                case CanceledBill:
                    return CanceledBill_TEXT;
                case StudentBill2:
                    return StudentBill2_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得 ExportConfig 設定種類代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">匯出種類代碼</param>
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
        /// 取得指定代碼是否為定義的 ExportConfig 設定種類代碼
        /// </summary>
        /// <param name="code">指定代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            switch (code)
            {
                case StudentBill:
                case CanceledBill:
                case StudentBill2:
                    return true;
            }
            return false;
        }
        #endregion
    }
}
