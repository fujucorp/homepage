using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 設定代碼文字定義清單類別
    /// </summary>
    public class ConfigKeyCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 服務項目設定 : ServiceConfig
        /// </summary>
        public const string SERVICE_CONFIG = "ServiceConfig";

        #region [MDY:20181007] 因為 EDI 的 DailyKey 要串 CustLoginId 所以縮短 KEY 名稱
        #region [OLD]
        ///// <summary>
        ///// EAI 初始 Key 設定 : EAIInitKey
        ///// </summary>
        //public const string EDI_INIT_KEY = "EAIInitKey";

        ///// <summary>
        ///// EAI 每日 Key 設定 : EDIDailyKey
        ///// </summary>
        //public const string EDI_DAILY_KEY = "EDIDailyKey";
        #endregion

        /// <summary>
        /// EAI 初始 Key 設定 : EAIInitKey
        /// </summary>
        public const string EAI_INIT_KEY = "EAIInitKey";

        /// <summary>
        /// EAI 每日 Key 設定 : EDIDailyKey
        /// </summary>
        public const string EAI_DAILY_KEY = "EAIDKey";

        #endregion

        /// <summary>
        /// Q and A 分類設定 : QNACategory
        /// </summary>
        public const string QNA_CATEGORY = "QNACategory";
        #endregion

        #region Const Text
        /// <summary>
        /// 服務項目設定 : ServiceConfig
        /// </summary>
        public const string SERVICE_CONFIG_TEXT = "服務項目設定";

        /// <summary>
        /// EAI 初始 Key 設定 : EAIInitKey
        /// </summary>
        public const string EDI_INIT_KEY_TEXT = "EAI 初始 Key 設定";

        /// <summary>
        /// EAI 每日 Key 設定: EDIDailyKey
        /// </summary>
        public const string EDI_DAILY_KEY_TEXT = "EAI 每日 Key 設定";

        /// <summary>
        /// Q and A 分類設定 : QNACategory
        /// </summary>
        public const string QNA_CATEGORY_TEXT = "Q and A 分類設定";
        #endregion

        #region [MDY:20170818] M201708_01 學校銷帳檔3合1的商家代號系統參數 SC31ReceiveType (20170531_01)
        /// <summary>
        /// 學校銷帳檔3合1的商家代號系統參數 : SC31ReceiveType
        /// </summary>
        public const string SC31_RECEIVETYPE = "SC31ReceiveType";

        /// <summary>
        /// 學校銷帳檔3合1的商家代號系統參數 : SC31ReceiveType
        /// </summary>
        public const string SC31_RECEIVETYPE_TEXT = "學校銷帳檔3合1的商家代號系統參數";
        #endregion

        #region [MDY:20200807] M202008_01 增加學生姓名要遮罩的商家代號參數 MaskReceiveType (2020806_01)
        /// <summary>
        /// 學生姓名要遮罩的商家代號系統參數 : MaskReceiveTypes
        /// </summary>
        public const string MASK_RECEIVETYPE = "MaskReceiveType";

        /// <summary>
        /// 學生姓名要遮罩的商家代號系統參數 : MaskReceiveTypes
        /// </summary>
        public const string MASK_RECEIVETYPE_TEXT = "學生姓名要遮罩的商家代號系統參數";
        #endregion

        #region [MDY:20200815] M202008_02 增加顯示使用者密碼的學校代碼參數 FixVerifySchoolId (2020819_01)
        /// <summary>
        /// 顯示使用者密碼的學校代碼系統參數 : FixVerifySchoolId
        /// </summary>
        public const string FIXVERIFY_SCHOOLID = "FixVerifySchoolId";

        /// <summary>
        /// 顯示使用者密碼的學校代碼系統參數 : FixVerifySchoolId
        /// </summary>
        public const string FIXVERIFY_SCHOOLID_TEXT = "顯示使用者密碼的學校代碼系統參數";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構設定代碼文字定義清單類別
        /// </summary>
        public ConfigKeyCodeTexts()
        {
            base.Add(SERVICE_CONFIG, SERVICE_CONFIG_TEXT);
            base.Add(EAI_INIT_KEY, EDI_INIT_KEY_TEXT);
            base.Add(EAI_DAILY_KEY, EDI_DAILY_KEY_TEXT);
            base.Add(QNA_CATEGORY, QNA_CATEGORY_TEXT);

            #region [MDY:20170818] M201708_01 學校銷帳檔3合1的商家代號系統參數 SC31ReceiveType (20170531_01)
            base.Add(SC31_RECEIVETYPE, SC31_RECEIVETYPE_TEXT);
            #endregion

            #region [MDY:20200807] M202008_01 增加學生姓名要遮罩的商家代號參數 MaskReceiveType (2020806_01)
            base.Add(MASK_RECEIVETYPE, MASK_RECEIVETYPE_TEXT);
            #endregion

            #region [MDY:20200815] M202008_02 增加顯示使用者密碼的學校代碼參數 FixVerifySchoolId (2020819_01)
            base.Add(FIXVERIFY_SCHOOLID, FIXVERIFY_SCHOOLID_TEXT);
            #endregion
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得設定代碼文字對應的文字
        /// </summary>
        /// <param name="code">設定代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case SERVICE_CONFIG:
                    return SERVICE_CONFIG_TEXT;
                case EAI_INIT_KEY:
                    return EDI_INIT_KEY_TEXT;
                case EAI_DAILY_KEY:
                    return EDI_DAILY_KEY_TEXT;
                case QNA_CATEGORY:
                    return QNA_CATEGORY_TEXT;

                #region [MDY:20170818] M201708_01 學校銷帳檔3合1的商家代號系統參數 SC31ReceiveType (20170531_01)
                case SC31_RECEIVETYPE:
                    return SC31_RECEIVETYPE_TEXT;
                #endregion

                #region [MDY:20200807] M202008_01 增加學生姓名要遮罩的商家代號參數 MaskReceiveType (2020806_01)
                case MASK_RECEIVETYPE:
                    return MASK_RECEIVETYPE_TEXT;
                #endregion

                #region [MDY:20200815] M202008_02 增加顯示使用者密碼的學校代碼參數 FixVerifySchoolId (2020819_01)
                case FIXVERIFY_SCHOOLID:
                    return FIXVERIFY_SCHOOLID_TEXT;
                #endregion
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
