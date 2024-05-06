using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 年級代碼文字定義清單類別
    /// </summary>
    public class GradeCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 一年級 : 1
        /// </summary>
        public const string G01 = "1";

        /// <summary>
        /// 二年級 : 2
        /// </summary>
        public const string G02 = "2";

        /// <summary>
        /// 三年級 : 2
        /// </summary>
        public const string G03 = "3";

        /// <summary>
        /// 四年級 : 4
        /// </summary>
        public const string G04 = "4";

        /// <summary>
        /// 五年級 : 5
        /// </summary>
        public const string G05 = "5";

        /// <summary>
        /// 六年級 : 6
        /// </summary>
        public const string G06 = "6";

        /// <summary>
        /// 七年級 : 7
        /// </summary>
        public const string G07 = "7";

        /// <summary>
        /// 八年級 : 8
        /// </summary>
        public const string G08 = "8";

        /// <summary>
        /// 九年級 : 9
        /// </summary>
        public const string G09 = "9";

        /// <summary>
        /// 十年級 : 10
        /// </summary>
        public const string G10 = "10";

        /// <summary>
        /// 十一年級 : 11
        /// </summary>
        public const string G11 = "11";

        /// <summary>
        /// 十二年級 : 12
        /// </summary>
        public const string G12 = "12";
        #endregion

        #region Const Text
        /// <summary>
        /// 一年級 : 1
        /// </summary>
        public const string G01_TEXT = "一年級";

        /// <summary>
        /// 二年級 : 2
        /// </summary>
        public const string G02_TEXT = "二年級";

        /// <summary>
        /// 三年級 : 3
        /// </summary>
        public const string G03_TEXT = "三年級";

        /// <summary>
        /// 四年級 : 4
        /// </summary>
        public const string G04_TEXT = "四年級";

        /// <summary>
        /// 五年級 : 5
        /// </summary>
        public const string G05_TEXT = "五年級";

        /// <summary>
        /// 六年級 : 6
        /// </summary>
        public const string G06_TEXT = "六年級";

        /// <summary>
        /// 七年級 : 7
        /// </summary>
        public const string G07_TEXT = "七年級";

        /// <summary>
        /// 八年級 : 8
        /// </summary>
        public const string G08_TEXT = "八年級";

        /// <summary>
        /// 九年級 : 9
        /// </summary>
        public const string G09_TEXT = "九年級";

        /// <summary>
        /// 十年級 : 10
        /// </summary>
        public const string G10_TEXT = "十年級";

        /// <summary>
        /// 十一年級 : 11
        /// </summary>
        public const string G11_TEXT = "十一年級";

        /// <summary>
        /// 十二年級 : 12
        /// </summary>
        public const string G12_TEXT = "十二年級";
        #endregion

        #region [MDY:202203XX] 2022擴充案 英文名稱相關
        #region Const EngText
        /// <summary>
        /// First Grade : 1
        /// </summary>
        public const string G01_ENGTEXT = "First Grade";

        /// <summary>
        /// Second Grade : 2
        /// </summary>
        public const string G02_ENGTEXT = "Second Grade";

        /// <summary>
        /// Third Grade : 3
        /// </summary>
        public const string G03_ENGTEXT = "Third Grade";

        /// <summary>
        /// Fourth Grade : 4
        /// </summary>
        public const string G04_ENGTEXT = "Fourth Grade";

        /// <summary>
        /// Fifth Grade : 5
        /// </summary>
        public const string G05_ENGTEXT = "Fifth Grade";

        /// <summary>
        /// Sixth Grade : 6
        /// </summary>
        public const string G06_ENGTEXT = "Sixth Grade";

        /// <summary>
        /// Seventh Grade : 7
        /// </summary>
        public const string G07_ENGTEXT = "Seventh Grade";

        /// <summary>
        /// Eighth Grade : 8
        /// </summary>
        public const string G08_ENGTEXT = "Eighth Grade";

        /// <summary>
        /// Ninth Grade : 9
        /// </summary>
        public const string G09_ENGTEXT = "Ninth Grade";

        /// <summary>
        /// Tenth Grade : 10
        /// </summary>
        public const string G10_ENGTEXT = "Tenth Grade";

        /// <summary>
        /// Eleventh Grade : 11
        /// </summary>
        public const string G11_ENGTEXT = "Eleventh Grade";

        /// <summary>
        /// Twelfth Grade : 12
        /// </summary>
        public const string G12_ENGTEXT = "Twelfth Grade";
        #endregion
        #endregion

        #region Constructor
        #region [MDY:202203XX] 2022擴充案 英文名稱相關
        /// <summary>
        /// 建構年級代碼文字定義清單類別
        /// </summary>
        /// <param name="useEngDataUI">指定是否英文資料介面使用，預設 false</param>
        public GradeCodeTexts(bool useEngDataUI = false)
        {
            if (useEngDataUI)
            {
                base.Add(G01, G01_ENGTEXT);
                base.Add(G02, G02_ENGTEXT);
                base.Add(G03, G03_ENGTEXT);
                base.Add(G04, G04_ENGTEXT);
                base.Add(G05, G05_ENGTEXT);
                base.Add(G06, G06_ENGTEXT);
                base.Add(G07, G07_ENGTEXT);
                base.Add(G08, G08_ENGTEXT);
                base.Add(G09, G09_ENGTEXT);
                base.Add(G10, G10_ENGTEXT);
                base.Add(G11, G11_ENGTEXT);
                base.Add(G12, G12_ENGTEXT);
            }
            else
            {
                base.Add(G01, G01_TEXT);
                base.Add(G02, G02_TEXT);
                base.Add(G03, G03_TEXT);
                base.Add(G04, G04_TEXT);
                base.Add(G05, G05_TEXT);
                base.Add(G06, G06_TEXT);
                base.Add(G07, G07_TEXT);
                base.Add(G08, G08_TEXT);
                base.Add(G09, G09_TEXT);
                base.Add(G10, G10_TEXT);
                base.Add(G11, G11_TEXT);
                base.Add(G12, G12_TEXT);
            }
        }
        #endregion
        #endregion

        #region Static Method
        #region [MDY:202203XX] 2022擴充案 英文名稱相關
        /// <summary>
        /// 取得年級代碼對應的文字
        /// </summary>
        /// <param name="code">年級代碼</param>
        /// <param name="useEngDataUI">指定是否英文資料介面使用，預設 false</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code, bool useEngDataUI = false)
        {
            if (useEngDataUI)
            {
                switch (code)
                {
                    case G01:
                        return G01_ENGTEXT;
                    case G02:
                        return G02_ENGTEXT;
                    case G03:
                        return G03_ENGTEXT;
                    case G04:
                        return G04_ENGTEXT;
                    case G05:
                        return G05_ENGTEXT;
                    case G06:
                        return G06_ENGTEXT;
                    case G07:
                        return G07_ENGTEXT;
                    case G08:
                        return G08_ENGTEXT;
                    case G09:
                        return G09_ENGTEXT;
                    case G10:
                        return G10_ENGTEXT;
                    case G11:
                        return G11_ENGTEXT;
                    case G12:
                        return G12_ENGTEXT;
                }
            }
            else
            {
                switch (code)
                {
                    case G01:
                        return G01_TEXT;
                    case G02:
                        return G02_TEXT;
                    case G03:
                        return G03_TEXT;
                    case G04:
                        return G04_TEXT;
                    case G05:
                        return G05_TEXT;
                    case G06:
                        return G06_TEXT;
                    case G07:
                        return G07_TEXT;
                    case G08:
                        return G08_TEXT;
                    case G09:
                        return G09_TEXT;
                    case G10:
                        return G10_TEXT;
                    case G11:
                        return G11_TEXT;
                    case G12:
                        return G12_TEXT;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得年級代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">年級代碼</param>
        /// <param name="useEngDataUI">指定是否英文資料介面使用，預設 false</param>
        /// <returns>傳回對應的代碼與文字對照類別，無對應則傳回 null</returns>
        public static CodeText GetCodeText(string code, bool useEngDataUI = false)
        {
            string text = GetText(code, useEngDataUI);
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
        #endregion
    }
}
