using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 最新消息公告位置的代碼文字定義清單類別
    /// </summary>
    public class BoardTypeCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 首頁 : 1
        /// </summary>
        public const string INDEX = "1";
        /// <summary>
        /// 學校專區 : 2
        /// </summary>
        public const string SCHOOL = "2";
        /// <summary>
        /// 學生專區 : 3
        /// </summary>
        public const string STUDENT = "3";
        /// <summary>
        /// 銀行專區 : 4
        /// </summary>
        public const string BANK = "4";
        /// <summary>
        /// 信用卡繳費 : 5
        /// </summary>
        public const string CREDITCARD = "5";
        /// <summary>
        /// 銀聯卡繳費 : 6
        /// </summary>
        public const string UNITCARD = "6";
        /// <summary>
        /// 查詢繳費狀態 : 7
        /// </summary>
        public const string QUERYPAY = "7";
        /// <summary>
        /// 查詢列印繳費單 : 8
        /// </summary>
        public const string QUERYPRINT = "8";
        /// <summary>
        /// 列印收據 : 9
        /// </summary>
        public const string PRINTRECEIVE = "9";
        #endregion

        #region Const Text
        /// <summary>
        /// 首頁 : 1
        /// </summary>
        public const string INDEX_TEXT = "首頁";
        /// <summary>
        /// 學校專區 : 2
        /// </summary>
        public const string SCHOOL_TEXT = "學校專區";
        /// <summary>
        /// 學生專區 : 3
        /// </summary>
        public const string STUDENT_TEXT = "學生專區";
        /// <summary>
        /// 銀行專區 : 4
        /// </summary>
        public const string BANK_TEXT = "銀行專區";
        /// <summary>
        /// 信用卡繳費 : 5
        /// </summary>
        public const string CREDITCARD_TEXT = "信用卡繳費";
        /// <summary>
        /// 銀聯卡繳費 : 6
        /// </summary>
        public const string UNITCARD_TEXT = "銀聯卡繳費";
        /// <summary>
        /// 查詢繳費狀態 : 7
        /// </summary>
        public const string QUERYPAY_TEXT = "查詢繳費狀態";
        /// <summary>
        /// 查詢列印繳費單 : 8
        /// </summary>
        public const string QUERYPRINT_TEXT = "查詢列印繳費單";
        /// <summary>
        /// 列印收據 : 9
        /// </summary>
        public const string PRINTRECEIVE_TEXT = "列印收據";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構檢查登入與功能狀態結果的代碼文字定義清單類別
        /// </summary>
        public BoardTypeCodeTexts()
        {
            base.Add(INDEX, INDEX_TEXT);
            base.Add(SCHOOL, SCHOOL_TEXT);
            base.Add(STUDENT, STUDENT_TEXT);
            base.Add(BANK, BANK_TEXT);
            base.Add(CREDITCARD, CREDITCARD_TEXT);
            base.Add(UNITCARD, UNITCARD_TEXT);
            base.Add(QUERYPAY, QUERYPAY_TEXT);
            base.Add(QUERYPRINT, QUERYPRINT_TEXT);
            base.Add(PRINTRECEIVE, PRINTRECEIVE_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得代碼對應的文字
        /// </summary>
        /// <param name="code">銷帳註記代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case INDEX:
                    return INDEX_TEXT;
                case SCHOOL:
                    return SCHOOL_TEXT;
                case STUDENT:
                    return STUDENT_TEXT;
                case BANK:
                    return BANK_TEXT;
                case CREDITCARD:
                    return CREDITCARD_TEXT;
                case UNITCARD:
                    return UNITCARD_TEXT;
                case QUERYPAY:
                    return QUERYPAY_TEXT;
                case QUERYPRINT:
                    return QUERYPRINT_TEXT;
                case PRINTRECEIVE:
                    return PRINTRECEIVE_TEXT;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">銷帳註記代碼</param>
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
