using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 批次處理佇列的作業類別代碼文字定義清單類別
    /// </summary>
    public class JobCubeTypeCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 上傳學生繳費資料 : BUA
        /// </summary>
        public const string BUA = "BUA";

        /// <summary>
        /// 上傳學分費退費資料 : BUB
        /// </summary>
        public const string BUB = "BUB";

        /// <summary>
        /// 上傳課程收費標準 : BUC
        /// </summary>
        public const string BUC = "BUC";

        /// <summary>
        /// 上傳已產生銷帳編號就貸資料 : BUD
        /// </summary>
        public const string BUD = "BUD";

        /// <summary>
        /// 上傳已產生銷帳編號減免資料 : BUE
        /// </summary>
        public const string BUE = "BUE";

        /// <summary>
        /// 上傳簡易學生繳費資料 : BUF
        /// </summary>
        public const string BUF = "BUF";

        /// <summary>
        /// 上傳學生基本資料 : BUG
        /// </summary>
        public const string BUG = "BUG";


        /// <summary>
        /// 下載中國信託中心媒體檔 : F80
        /// </summary>
        public const string F80 = "F80";

        /// <summary>
        /// 下載統一超商中心媒體檔 : F81
        /// </summary>
        public const string F81 = "F81";

        /// <summary>
        /// 下載全家超商中心媒體檔 : F82
        /// </summary>
        public const string F82 = "F82";

        /// <summary>
        /// 下載OK超商中心媒體檔 : F83
        /// </summary>
        public const string F83 = "F83";

        /// <summary>
        /// 下載萊爾富超商中心媒體檔 : F85
        /// </summary>
        public const string F85 = "F85";

        /// <summary>
        /// 下載財金中心媒體檔 : F87
        /// </summary>
        public const string F87 = "F87";

        /// <summary>
        /// 匯入中國信託中心媒體檔 : C80
        /// </summary>
        public const string C80 = "C80";

        /// <summary>
        /// 匯入統一超商中心媒體檔 : C81
        /// </summary>
        public const string C81 = "C81";

        /// <summary>
        /// 匯入全家超商中心媒體檔 : C82
        /// </summary>
        public const string C82 = "C82";

        /// <summary>
        /// 匯入OK超商中心媒體檔 : C83
        /// </summary>
        public const string C83 = "C83";

        /// <summary>
        /// 匯入萊爾富超商中心媒體檔 : C85
        /// </summary>
        public const string C85 = "C85";

        /// <summary>
        /// 匯入財金中心媒體檔 : C87
        /// </summary>
        public const string C87 = "C87";


        /// <summary>
        /// 產生學校銷帳檔 : SC1
        /// </summary>
        public const string SC1 = "SC1";

        /// <summary>
        /// 產生學校銷帳檔(補) : SC1B
        /// </summary>
        public const string SC1B = "SC1B";

        /// <summary>
        /// 傳送學校銷帳檔 : SC2
        /// </summary>
        public const string SC2 = "SC2";

        /// <summary>
        /// 傳送學校銷帳檔(補) : SC2B
        /// </summary>
        public const string SC2B = "SC2B";

        /// <summary>
        /// 產生中國信託學校檔和學生檔 : CTC1
        /// </summary>
        public const string CTC1 = "CTC1";

        /// <summary>
        /// 傳送中國信託學校檔和學生檔 : CTC2
        /// </summary>
        public const string CTC2 = "CTC2";


        /// <summary>
        /// 產生繳費金額 : CP
        /// </summary>
        public const string CP = "CP";

        /// <summary>
        /// 產生銷帳編號 : CI
        /// </summary>
        public const string CI = "CI";


        /// <summary>
        /// 產生繳費單 : PDFB
        /// </summary>
        public const string PDFB = "PDFB";

        /// <summary>
        /// 產生繳費收據 : PDFR
        /// </summary>
        public const string PDFR = "PDFR";


        /// <summary>
        /// 發送繳費通知信 : SBM
        /// </summary>
        public const string SBM = "SBM";

        /// <summary>
        /// 銷帳處理 : SCD
        /// </summary>
        public const string SCD = "SCD";

        /// <summary>
        /// 銷帳處理(補) : SCD2
        /// </summary>
        public const string SCD2 = "SCD2";

        /// <summary>
        /// 每日銷帳結果處理 : SCR
        /// </summary>
        public const string SCR = "SCR";

        /// <summary>
        /// 每日銷帳結果處理(補) : SCR2
        /// </summary>
        public const string SCR2 = "SCR2";

        /// <summary>
        /// 報稅資料處理 : SGG
        /// </summary>
        public const string SGG = "SGG";


        /// <summary>
        /// EAI 每日換 Key : DKEY
        /// </summary>
        public const string DKEY = "DKEY";

        /// <summary>
        /// D00I70資料下載 : D70
        /// </summary>
        public const string D70 = "D70";

        /// <summary>
        /// D00I70資料下載(補) : D70B
        /// </summary>
        public const string D70B = "D70B";

        /// <summary>
        /// D00I70資料下載(C時段) : D70C
        /// </summary>
        public const string D70C = "D70C";

        /// <summary>
        /// D38資料上傳 : D38
        /// </summary>
        public const string D38 = "D38";

        /// <summary>
        /// TRS資料轉置 : TRS
        /// </summary>
        public const string TRS = "TRS";

        #region [MDY:20160305] 補齊功能 (線上資料搬移與歷史資料刪除)
        /// <summary>
        /// 線上資料搬移與歷史資料刪除 : MDH
        /// </summary>
        public const string MDH = "MDH";

        /// <summary>
        /// 發送服務執行結果通知信 : SNM
        /// </summary>
        public const string SNM = "SNM";
        #endregion

        #region [MDY:20160305] 財金境外支付清算檔處理
        /// <summary>
        /// 財金境外支付清算檔處理 ： INBT
        /// </summary>
        public const string INBT = "INBT";
        #endregion

        #region [MDY:20211102] M202110_05 聯徵KP3資料匯出報送 (2021擴充案先做)
        /// <summary>
        /// 聯徵KP3資料匯出報送 : KP3
        /// </summary>
        public const string KP3 = "KP3";
        #endregion
        #endregion

        #region Const Text
        /// <summary>
        /// 上傳學生繳費資料 : BUA
        /// </summary>
        public const string BUA_TEXT = "上傳學生繳費資料";

        /// <summary>
        /// 上傳學分費退費資料 : BUB
        /// </summary>
        public const string BUB_TEXT = "上傳學分費退費資料";

        /// <summary>
        /// 上傳課程收費標準 : BUC
        /// </summary>
        public const string BUC_TEXT = "上傳課程收費標準";

        /// <summary>
        /// 上傳已產生銷帳編號就貸資料 : BUD
        /// </summary>
        public const string BUD_TEXT = "上傳已產生虛擬帳號就貸資料";

        /// <summary>
        /// 上傳已產生銷帳編號減免資料 : BUE
        /// </summary>
        public const string BUE_TEXT = "上傳已產生虛擬帳號減免資料";

        /// <summary>
        /// 上傳簡易學生繳費資料 : BUF
        /// </summary>
        public const string BUF_TEXT = "上傳簡易學生繳費資料";

        /// <summary>
        /// 上傳學生基本資料 : BUG
        /// </summary>
        public const string BUG_TEXT = "上傳學生基本資料";


        /// <summary>
        /// 下載中國信託中心媒體檔 : F80
        /// </summary>
        public const string F80_TEXT = "下載中國信託中心媒體檔";

        /// <summary>
        /// 下載統一超商中心媒體檔 : F81
        /// </summary>
        public const string F81_TEXT = "下載統一超商中心媒體檔";

        /// <summary>
        /// 下載全家超商中心媒體檔 : F82
        /// </summary>
        public const string F82_TEXT = "下載全家超商中心媒體檔";

        /// <summary>
        /// 下載OK超商中心媒體檔 : F83
        /// </summary>
        public const string F83_TEXT = "下載OK超商中心媒體檔";

        /// <summary>
        /// 下載萊爾富超商中心媒體檔 : F85
        /// </summary>
        public const string F85_TEXT = "下載萊爾富超商中心媒體檔";

        /// <summary>
        /// 下載財金中心媒體檔 : F87
        /// </summary>
        public const string F87_TEXT = "下載財金中心媒體檔";

        /// <summary>
        /// 匯入中國信託中心媒體檔 : C80
        /// </summary>
        public const string C80_TEXT = "匯入中國信託中心媒體檔";

        /// <summary>
        /// 匯入統一超商中心媒體檔 : C81
        /// </summary>
        public const string C81_TEXT = "匯入統一超商中心媒體檔";

        /// <summary>
        /// 匯入全家超商中心媒體檔 : C82
        /// </summary>
        public const string C82_TEXT = "匯入全家超商中心媒體檔";

        /// <summary>
        /// 匯入OK超商中心媒體檔 : C83
        /// </summary>
        public const string C83_TEXT = "匯入OK超商中心媒體檔";

        /// <summary>
        /// 匯入萊爾富超商中心媒體檔 : C85
        /// </summary>
        public const string C85_TEXT = "匯入萊爾富超商中心媒體檔";

        /// <summary>
        /// 匯入財金中心媒體檔 : C87
        /// </summary>
        public const string C87_TEXT = "匯入財金中心媒體檔";


        /// <summary>
        /// 產生學校銷帳檔 : SC1
        /// </summary>
        public const string SC1_TEXT = "產生學校銷帳檔";

        /// <summary>
        /// 產生學校銷帳檔(補) : SC1B
        /// </summary>
        public const string SC1B_TEXT = "產生學校銷帳檔(補)";

        /// <summary>
        /// 傳送學校銷帳檔 : SC2
        /// </summary>
        public const string SC2_TEXT = "傳送學校銷帳檔";

        /// <summary>
        /// 傳送學校銷帳檔(補) : SC2B
        /// </summary>
        public const string SC2B_TEXT = "傳送學校銷帳檔(補)";

        /// <summary>
        /// 產生中國信託學校檔和學生檔 : CTC1
        /// </summary>
        public const string CTC1_TEXT = "產生中國信託學校檔和學生檔";

        /// <summary>
        /// 傳送中國信託學校檔和學生檔 : CTC2
        /// </summary>
        public const string CTC2_TEXT = "傳送中國信託學校檔和學生檔";


        /// <summary>
        /// 產生繳費金額 : CP
        /// </summary>
        public const string CP_TEXT = "產生繳費金額";

        /// <summary>
        /// 產生銷帳編號 : CI
        /// </summary>
        public const string CI_TEXT = "產生虛擬帳號";


        /// <summary>
        /// 產生繳費單 : PDFB
        /// </summary>
        public const string PDFB_TEXT = "PDFB";

        /// <summary>
        /// 產生繳費收據 : PDFR
        /// </summary>
        public const string PDFR_TEXT = "PDFR";


        /// <summary>
        /// 發送繳費通知信 : SMB
        /// </summary>
        public const string SBM_TEXT = "發送繳費通知信";

        /// <summary>
        /// 銷帳處理 : SCD
        /// </summary>
        public const string SCD_TEXT = "銷帳處理";

        /// <summary>
        /// 銷帳處理(補) : SCD2
        /// </summary>
        public const string SCD2_TEXT = "銷帳處理(補)";

        /// <summary>
        /// 每日銷帳結果處理 : SCR
        /// </summary>
        public const string SCR_TEXT = "每日銷帳結果處理";

        /// <summary>
        /// 每日銷帳結果處理(補) : SCR2
        /// </summary>
        public const string SCR2_TEXT = "每日銷帳結果處理(補)";

        /// <summary>
        /// 報稅資料處理 : SGG
        /// </summary>
        public const string SGG_TEXT = "報稅資料處理";


        /// <summary>
        /// EAI 每日換 Key : DKEY
        /// </summary>
        public const string DKEY_TEXT = "EAI 每日換 Key";

        /// <summary>
        /// D00I70資料下載 : D70
        /// </summary>
        public const string D70_TEXT = "D00I70資料下載";

        /// <summary>
        /// D00I70資料下載(補) : D70B
        /// </summary>
        public const string D70B_TEXT = "D00I70資料下載(補)";

        /// <summary>
        /// D00I70資料下載(C時段) : D70C
        /// </summary>
        public const string D70C_TEXT = "D00I70資料下載(C時段)";

        /// <summary>
        /// D38資料上傳 : D38
        /// </summary>
        public const string D38_TEXT = "D38資料上傳";

        /// <summary>
        /// TRS資料轉置 : TRS
        /// </summary>
        public const string TRS_TEXT = "TRS資料轉置";

        #region [MDY:20160305] 補齊功能 (線上資料搬移與歷史資料刪除)
        /// <summary>
        /// 線上資料搬移與歷史資料刪除 : MDH
        /// </summary>
        public const string MDH_TEXT = "線上資料搬移與歷史資料刪除";

        /// <summary>
        /// 發送服務執行結果通知信 : SNM
        /// </summary>
        public const string SNM_TEXT = "發送服務執行結果通知信";
        #endregion

        #region [MDY:20160305] 財金境外支付清算檔處理
        /// <summary>
        /// 財金境外支付清算檔處理 : INBT
        /// </summary>
        public const string INBT_TEXT = "財金境外支付清算檔處理";
        #endregion

        #region [MDY:20211102] M202110_05 聯徵KP3資料匯出報送 (2021擴充案先做)
        /// <summary>
        /// 聯徵KP3資料匯出報送 : KP3
        /// </summary>
        public const string KP3_TEXT = "聯徵KP3資料匯出報送";
        #endregion
        #endregion

        #region [MDY:20170531] 學校銷帳檔3合1 (M201705_02)
        /// <summary>
        /// 學校銷帳檔3合1 ： SC31
        /// </summary>
        public const string SC31 = "SC31";

        /// <summary>
        /// 學校銷帳檔3合1 ： SC31
        /// </summary>
        public const string SC31_TEXT = "學校銷帳檔3合1";
        #endregion

        #region [MDY:20180224] 財金境外支付換KEY
        /// <summary>
        /// 財金境外支付換KEY ： INBK
        /// </summary>
        public const string INBK = "INBK";

        /// <summary>
        /// 財金境外支付換KEY : INBK
        /// </summary>
        public const string INBK_TEXT = "財金境外支付換KEY";
        #endregion

        #region [MDY:20180616] 清除過期的暫存與日誌資料
        /// <summary>
        /// 清除過期的暫存與日誌資料 ： CLDB
        /// </summary>
        public const string CLDB = "CLDB";

        /// <summary>
        /// 清除過期的暫存與日誌資料 : CLDB
        /// </summary>
        public const string CLDB_TEXT = "清除過期的暫存與日誌資料";
        #endregion

        #region [MDY:20220326] M202203_01 新服務執行結果通知信
        /// <summary>
        /// 新服務執行結果通知信 : SNM2
        /// </summary>
        public const string SNM2 = "SNM2";

        /// <summary>
        /// 新服務執行結果通知信 : SNM2
        /// </summary>
        public const string SNM2_TEXT = "新服務執行結果通知信";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構批次處理佇列的作業類別代碼文字定義清單類別
        /// </summary>
        public JobCubeTypeCodeTexts()
        {
            base.Add(BUA, BUA_TEXT);
            base.Add(BUB, BUB_TEXT);
            base.Add(BUC, BUC_TEXT);
            base.Add(BUD, BUD_TEXT);
            base.Add(BUE, BUE_TEXT);
            base.Add(BUF, BUF_TEXT);
            base.Add(BUG, BUG_TEXT);

            base.Add(F80, F80_TEXT);
            base.Add(F81, F81_TEXT);
            base.Add(F82, F82_TEXT);
            base.Add(F83, F83_TEXT);
            base.Add(F85, F85_TEXT);
            base.Add(F87, F87_TEXT);

            base.Add(C80, C80_TEXT);
            base.Add(C81, C81_TEXT);
            base.Add(C82, C82_TEXT);
            base.Add(C83, C83_TEXT);
            base.Add(C85, C85_TEXT);
            base.Add(C87, C87_TEXT);

            base.Add(SC1, SC1_TEXT);
            base.Add(SC1B, SC1B_TEXT);
            base.Add(SC2, SC2_TEXT);
            base.Add(SC2B, SC2B_TEXT);
            base.Add(CTC1, CTC1_TEXT);
            base.Add(CTC2, CTC2_TEXT);

            base.Add(CP, CP_TEXT);
            base.Add(CI, CI_TEXT);

            base.Add(PDFB, PDFB_TEXT);
            base.Add(PDFR, PDFR_TEXT);

            base.Add(SBM, SBM_TEXT);
            base.Add(SCD, SCD_TEXT);
            base.Add(SCD2, SCD2_TEXT);
            base.Add(SCR, SCR_TEXT);
            base.Add(SCR2, SCR2_TEXT);

            base.Add(SGG, SGG_TEXT);

            base.Add(DKEY, DKEY_TEXT);
            base.Add(D70, D70_TEXT);
            base.Add(D70B, D70B_TEXT);
            base.Add(D70C, D70C_TEXT);
            base.Add(D38, D38_TEXT);

            base.Add(TRS, TRS_TEXT);

            #region [MDY:20160305] 補齊功能 (線上資料搬移與歷史資料刪除)
            base.Add(MDH, MDH_TEXT);

            base.Add(SNM, SNM_TEXT);
            #endregion

            #region [MDY:20160305] 財金境外支付清算檔處理
            base.Add(INBT, INBT_TEXT);
            #endregion

            #region [MDY:20211102] M202110_05 聯徵KP3資料匯出報送 (2021擴充案先做)
            base.Add(KP3, KP3_TEXT);
            #endregion

            #region [MDY:20220326] M202203_01 新服務執行結果通知信
            base.Add(SNM2, SNM2_TEXT);
            #endregion

            #region [MDY:20170531] 學校銷帳檔3合1 (M201705_02)
            base.Add(SC31, SC31_TEXT);
            #endregion

            #region [MDY:20180224] 財金境外支付換KEY
            base.Add(INBK, INBK_TEXT);
            #endregion

            #region [MDY:20180616] 清除過期的暫存與日誌資料
            base.Add(CLDB, CLDB_TEXT);
            #endregion
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得批次處理佇列的作業類別代碼的文字
        /// </summary>
        /// <param name="code">批次處理佇列的作業類別代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            switch (code)
            {
                case BUA:
                    return BUA_TEXT;
                case BUB:
                    return BUB_TEXT;
                case BUC:
                    return BUC_TEXT;
                case BUD:
                    return BUD_TEXT;
                case BUE:
                    return BUE_TEXT;
                case BUF:
                    return BUF_TEXT;
                case BUG:
                    return BUG_TEXT;

                case F80:
                    return F80_TEXT;
                case F81:
                    return F81_TEXT;
                case F82:
                    return F82_TEXT;
                case F83:
                    return F83_TEXT;
                case F85:
                    return F85_TEXT;
                case F87:
                    return F87_TEXT;

                case C80:
                    return C80_TEXT;
                case C81:
                    return C81_TEXT;
                case C82:
                    return C82_TEXT;
                case C83:
                    return C83_TEXT;
                case C85:
                    return C85_TEXT;
                case C87:
                    return C87_TEXT;

                case SC1:
                    return SC1_TEXT;
                case SC1B:
                    return SC1B_TEXT;
                case SC2:
                    return SC2_TEXT;
                case SC2B:
                    return SC2B_TEXT;
                case CTC1:
                    return CTC1_TEXT;
                case CTC2:
                    return CTC2_TEXT;

                case CP:
                    return CP_TEXT;
                case CI:
                    return CI_TEXT;

                case PDFB:
                    return PDFB_TEXT;
                case PDFR:
                    return PDFR_TEXT;

                case SBM:
                    return SBM_TEXT;
                case SCD:
                    return SCD_TEXT;
                case SCD2:
                    return SCD2_TEXT;
                case SCR:
                    return SCR_TEXT;
                case SCR2:
                    return SCR2_TEXT;

                case SGG:
                    return SGG_TEXT;

                case DKEY:
                    return DKEY_TEXT;
                case D70:
                    return D70_TEXT;
                case D70B:
                    return D70B_TEXT;
                case D70C:
                    return D70C_TEXT;
                case D38:
                    return D38_TEXT;

                case TRS:
                    return TRS_TEXT;

                #region [MDY:20160305] 補齊功能 (線上資料搬移與歷史資料刪除)
                case MDH:
                    return MDH_TEXT;
                case SNM:
                    return SNM_TEXT;
                #endregion

                #region [MDY:20160305] 財金境外支付清算檔處理
                case INBT:
                    return INBT_TEXT;
                #endregion

                #region #region [MDY:20211102] M202110_05 聯徵KP3資料匯出報送 (2021擴充案先做)
                case KP3:
                    return KP3_TEXT;
                #endregion

                #region #region [MDY:20220326] M202203_01 新服務執行結果通知信
                case SNM2:
                    return SNM2_TEXT;
                #endregion

                #region [MDY:20170531] 學校銷帳檔3合1 (M201705_02)
                case SC31:
                    return SC31_TEXT;
                #endregion

                #region [MDY:20180224] 財金境外支付換KEY
                case INBK:
                    return INBK_TEXT;
                #endregion

                #region [MDY:20180616] 清除過期的暫存與日誌資料
                case CLDB:
                    return CLDB_TEXT;
                #endregion
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得批次處理佇列的作業類別代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">批次處理佇列的作業類別代碼</param>
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
        /// 取得指定的代碼是否為定義的批次處理佇列的作業類別代碼
        /// </summary>
        /// <param name="code">指定要檢查的代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            return !String.IsNullOrEmpty(GetText(code));
        }

        /// <summary>
        /// 取得指定代碼是否為匯入檔案類作業類別
        /// </summary>
        /// <param name="code">指定代碼</param>
        /// <returns></returns>
        public static bool IsImportFileJob(string code)
        {
            switch (code)
            {
                case BUA:
                case BUB:
                case BUC:
                case BUD:
                case BUE:
                case BUF:
                case BUG:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 取得指定代碼是否為產生 PDF 檔案類作業類別
        /// </summary>
        /// <param name="code">指定代碼</param>
        /// <returns></returns>
        public static bool IsGenPDFFileJob(string code)
        {
            switch (code)
            {
                case PDFB:
                case PDFR:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 取得指定代碼是否為即時執性類作業類別
        /// </summary>
        /// <param name="code">指定代碼</param>
        /// <returns></returns>
        public static bool IsRealTimeJob(string code)
        {
            switch (code)
            {
                case PDFB:
                case PDFR:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 取得非即時處理的作業類別
        /// </summary>
        /// <returns></returns>
        public static CodeTextList GetNonRealTimeJob()
        {
            CodeTextList list = new CodeTextList();
            JobCubeTypeCodeTexts some = new JobCubeTypeCodeTexts();
            foreach(CodeText one in some)
            {
                if (!IsRealTimeJob(one.Code))
                {
                    list.Add(one);
                }
            }
            return list;
        }
        #endregion
    }
}
