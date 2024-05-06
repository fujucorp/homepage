using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
namespace Entities
{
    public class D00I70
    {
        #region
        public class Layout01
        {
            public const int RecordLens = 46;
            public static int[] FiledsLen = { 6, 4, 10, 6, 6, 13, 1 };

            public Layout01()
            {

            }

            #region
            /// <summary>
            /// 交易序號
            /// </summary>
            public string TxnNo = "";
            /// <summary>
            /// 商家代號(虛擬帳號前4碼)
            /// </summary>
            public string ReceiveType = "";
            /// <summary>
            /// 用戶號碼(虛擬帳號後10碼)
            /// </summary>
            public string CustomerNo = "";
            /// <summary>
            /// 交易日期 yyMMdd 民國年2碼
            /// </summary>
            public string TxnDate6 = "";
            /// <summary>
            /// 交易時間 HHmmss
            /// </summary>
            public string TxnTime6 = "";
            /// <summary>
            /// 交易金額
            /// </summary>
            public decimal TxnAmount = 0;
            /// <summary>
            /// 更正註記 0:正常交易 1:更正
            /// </summary>
            public string ECMark = "";
            /// <summary>
            /// 原始資料
            /// </summary>
            public string Data = "";
            #endregion

        }

        public class Layout02
        {
            public const int RecordLens = 80;
            public static int[] FiledsLen = { 6, 4, 10, 6, 6, 13, 1, 2, 6, 26 };

            public Layout02()
            {

            }

            #region
            /// <summary>
            /// 交易序號
            /// </summary>
            public string TxnNo = "";
            /// <summary>
            /// 商家代號(虛擬帳號前4碼)
            /// </summary>
            public string ReceiveType = "";
            /// <summary>
            /// 用戶號碼(虛擬帳號後10碼)
            /// </summary>
            public string CustomerNo = "";
            /// <summary>
            /// 交易日期 yyMMdd 民國年2碼
            /// </summary>
            public string TxnDate6 = "";
            /// <summary>
            /// 交易時間 HHmmss
            /// </summary>
            public string TxnTime6 = "";
            /// <summary>
            /// 交易金額
            /// </summary>
            public decimal TxnAmount = 0;
            /// <summary>
            /// 更正註記 0:正常交易 1:更正
            /// </summary>
            public string ECMark = "";
            /// <summary>
            /// 交易來源 01.臨櫃 02.匯款 03.ATM  04.EDI  05.網路銀行 06.語音銀行
            /// </summary>
            public string TxnWay = "";

            /// <summary>
            /// 作帳日期 民國年YYMMDD
            /// </summary>
            public string AccountDate = "";

            /// <summary>
            /// 保留欄位
            /// </summary>
            public string filler = "";

            /// <summary>
            /// 原始資料
            /// </summary>
            public string Data = "";
            #endregion

        }

        public class Layout03
        {
            public const int RecordLens = 80;

            public class Header
            {
                public static int[] FiledsLen = { 1, 1, 6, 6, 6, 6, 54 };
                public Header()
                {

                }

                /// <summary>
                /// 錄別 首錄：1
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 查詢碼 1:依交易日 2:依作帳日
                /// </summary>
                public string qCode = "";
                /// <summary>
                /// 查詢起日
                /// </summary>
                public string qStartDate = "";
                /// <summary>
                /// 查詢迄日
                /// </summary>
                public string qEndDate = "";
                /// <summary>
                /// 查詢起號
                /// </summary>
                public string qStartNo = "";
                /// <summary>
                /// 查詢迄號
                /// </summary>
                public string qEndNo = "";
                /// <summary>
                /// 保留
                /// </summary>
                public string Filler = "";

            }

            public class Detail
            {
                public static int[] FiledsLen = { 1, 6, 4, 10, 6, 6, 13, 1, 2, 6, 3, 22 };
                public Detail()
                {

                }

                /// <summary>
                /// 錄別 資料：2
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 交易序號
                /// </summary>
                public string TxnNo = "";
                /// <summary>
                /// 商家代號(虛擬帳號前4碼)
                /// </summary>
                public string ReceiveType = "";
                /// <summary>
                /// 用戶號碼(虛擬帳號後10碼)
                /// </summary>
                public string CustomerNo = "";
                /// <summary>
                /// 交易日期 yyMMdd 民國年2碼
                /// </summary>
                public string TxnDate6 = "";
                /// <summary>
                /// 交易時間 HHmmss
                /// </summary>
                public string TxnTime6 = "";
                /// <summary>
                /// 交易金額
                /// </summary>
                public decimal TxnAmount = 0;
                /// <summary>
                /// 更正註記 0:正常交易 1:更正
                /// </summary>
                public string ECMark = "";
                /// <summary>
                /// 交易來源 01.臨櫃 02.匯款  03.ATM   04.EDI   05.網路銀行  06.語音銀行 07.中油營收
                /// </summary>
                public string TxnWay = "";
                /// <summary>
                /// 作帳日期 民國年YYMMDD
                /// </summary>
                public string AccountDate = "";
                /// <summary>
                /// 繳款行 3碼
                /// </summary>
                public string TxnBank = "";
                /// <summary>
                /// 保留欄位
                /// </summary>
                public string filler = "";
                /// <summary>
                /// 原始資料
                /// </summary>
                public string Data = "";
            }

            public class Footer
            {
                public static int[] FiledsLen = { 1, 10, 16, 53 };
                public Footer()
                {

                }

                /// <summary>
                /// 錄別 尾路：3
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 合計筆數
                /// </summary>
                public decimal TotalCount = 0;
                /// <summary>
                /// 合計金額
                /// </summary>
                public decimal TotalAmount = 0;
                /// <summary>
                /// 保留
                /// </summary>
                public string Filler = "";
            }
        }

        public class Layout04
        {
            public const int RecordLens = 48;
            public static int[] FiledsLen = { 6, 4, 10, 8, 6, 13, 1 };

            public Layout04()
            {

            }

            #region
            /// <summary>
            /// 交易序號
            /// </summary>
            public string TxnNo = "";
            /// <summary>
            /// 商家代號(虛擬帳號前4碼)
            /// </summary>
            public string ReceiveType = "";
            /// <summary>
            /// 用戶號碼(虛擬帳號後10碼)
            /// </summary>
            public string CustomerNo = "";
            /// <summary>
            /// 交易日期 yyMMdd 民國年2碼
            /// </summary>
            public string TxnDate6 = "";
            /// <summary>
            /// 交易時間 HHmmss
            /// </summary>
            public string TxnTime6 = "";
            /// <summary>
            /// 交易金額
            /// </summary>
            public decimal TxnAmount = 0;
            /// <summary>
            /// 更正註記 0:正常交易 1:更正
            /// </summary>
            public string ECMark = "";
            /// <summary>
            /// 原始資料
            /// </summary>
            public string Data = "";
            #endregion
        }

        public class Layout05
        {
            public const int RecordLens = 80;
            public static int[] FiledsLen = { 6, 4, 10, 8, 6, 13, 1, 2, 8, 3, 19 };

            public Layout05()
            {

            }

            #region
            /// <summary>
            /// 交易序號
            /// </summary>
            public string TxnNo = "";
            /// <summary>
            /// 商家代號(虛擬帳號前4碼)
            /// </summary>
            public string ReceiveType = "";
            /// <summary>
            /// 用戶號碼(虛擬帳號後10碼)
            /// </summary>
            public string CustomerNo = "";
            /// <summary>
            /// 交易日期 yyMMdd 民國年2碼
            /// </summary>
            public string TxnDate6 = "";
            /// <summary>
            /// 交易時間 HHmmss
            /// </summary>
            public string TxnTime6 = "";
            /// <summary>
            /// 交易金額
            /// </summary>
            public decimal TxnAmount = 0;
            /// <summary>
            /// 更正註記 0:正常交易 1:更正
            /// </summary>
            public string ECMark = "";
            /// <summary>
            /// 交易來源 00.全部 01.臨櫃 02.匯款 03.ATM 04.EDI 05.網路銀行 06.語音銀行 07.中油營收 80.中國信託 81.超商(7-11) 82.超商(全家) 83.超商(OK) 84.超商(福客多) 85.超商(萊爾富) 86.郵局 87.信用卡 91.分行手續費(7-11) 92.分行手續費(全家) 93.分行手續費(OK) 94.分行手續費(福客多) 95分行手續費(萊爾富) 99.分行手續費
            /// </summary>
            public string TxnWay = "";
            /// <summary>
            /// 作帳日期 民國年YYMMDD
            /// </summary>
            public string AccountDate = "";
            /// <summary>
            /// 繳款行 3碼
            /// </summary>
            public string TxnBank = "";
            /// <summary>
            /// 保留欄位
            /// </summary>
            public string filler = "";
            /// <summary>
            /// 原始資料
            /// </summary>
            public string Data = "";
            #endregion
        }

        public class Layout06
        {
            public const int RecordLens = 80;

            public class Header
            {
                public static int[] FiledsLen = { 1, 1, 8, 8, 6, 6, 50 };
                public Header()
                {

                }

                /// <summary>
                /// 錄別 首錄：1
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 查詢碼 1:依交易日 2:依作帳日
                /// </summary>
                public string qCode = "";
                /// <summary>
                /// 查詢起日
                /// </summary>
                public string qStartDate = "";
                /// <summary>
                /// 查詢迄日
                /// </summary>
                public string qEndDate = "";
                /// <summary>
                /// 查詢起號
                /// </summary>
                public string qStartNo = "";
                /// <summary>
                /// 查詢迄號
                /// </summary>
                public string qEndNo = "";
                /// <summary>
                /// 保留
                /// </summary>
                public string Filler = "";
            }

            public class Detail
            {
                public static int[] FiledsLen = { 1, 6, 4, 10, 8, 6, 13, 1, 2, 8, 3, 18 };
                public Detail()
                {

                }

                /// <summary>
                /// 錄別 資料：2
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 交易序號
                /// </summary>
                public string TxnNo = "";
                /// <summary>
                /// 商家代號(虛擬帳號前4碼)
                /// </summary>
                public string ReceiveType = "";
                /// <summary>
                /// 用戶號碼(虛擬帳號後10碼)
                /// </summary>
                public string CustomerNo = "";
                /// <summary>
                /// 交易日期 yyMMdd 民國年2碼
                /// </summary>
                public string TxnDate6 = "";
                /// <summary>
                /// 交易時間 HHmmss
                /// </summary>
                public string TxnTime6 = "";
                /// <summary>
                /// 交易金額
                /// </summary>
                public decimal TxnAmount = 0;
                /// <summary>
                /// 更正註記 0:正常交易 1:更正
                /// </summary>
                public string ECMark = "";
                /// <summary>
                /// 交易來源 00.全部 01.臨櫃 02.匯款 03.ATM 04.EDI 05.網路銀行 06.語音銀行 07.中油營收 80.中國信託 81.超商(7-11) 82.超商(全家) 83.超商(OK) 84.超商(福客多) 85.超商(萊爾富) 86.郵局 87.信用卡 91.分行手續費(7-11) 92.分行手續費(全家) 93.分行手續費(OK) 94.分行手續費(福客多) 95分行手續費(萊爾富) 99.分行手續費
                /// </summary>
                public string TxnWay = "";
                /// <summary>
                /// 作帳日期 民國年YYMMDD
                /// </summary>
                public string AccountDate = "";
                /// <summary>
                /// 繳款行 3碼
                /// </summary>
                public string TxnBank = "";
                /// <summary>
                /// 保留欄位
                /// </summary>
                public string filler = "";
                /// <summary>
                /// 原始資料
                /// </summary>
                public string Data = "";
            }

            public class Footer
            {
                public static int[] FiledsLen = { 1, 10, 16, 53 };
                public Footer()
                {

                }

                /// <summary>
                /// 錄別 尾路：3
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 合計筆數
                /// </summary>
                public decimal TotalCount = 0;
                /// <summary>
                /// 合計金額
                /// </summary>
                public decimal TotalAmount = 0;
                /// <summary>
                /// 保留
                /// </summary>
                public string Filler = "";
            }
        }

        public class Layout07
        {
            public const int RecordLens = 80;

            public class Header
            {
                public static int[] FiledsLen = { 1, 1, 8, 8, 6, 6, 50 };
                public Header()
                {

                }

                /// <summary>
                /// 錄別 首錄：1
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 查詢碼 1:依交易日 2:依作帳日
                /// </summary>
                public string qCode = "";
                /// <summary>
                /// 查詢起日 : 民國年8碼
                /// </summary>
                public string qStartDate = "";
                /// <summary>
                /// 查詢迄日 : 民國年8碼
                /// </summary>
                public string qEndDate = "";
                /// <summary>
                /// 查詢起號
                /// </summary>
                public string qStartNo = "";
                /// <summary>
                /// 查詢迄號
                /// </summary>
                public string qEndNo = "";
                /// <summary>
                /// 保留
                /// </summary>
                public string Filler = "";
            }

            public class Detail
            {
                public static int[] FiledsLen = { 1, 6, 4, 12, 8, 6, 13, 1, 2, 8, 3, 16 };
                public Detail()
                {

                }

                /// <summary>
                /// 錄別 資料：2
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 交易序號
                /// </summary>
                public string TxnNo = "";
                /// <summary>
                /// 商家代號(虛擬帳號前4碼)
                /// </summary>
                public string ReceiveType = "";
                /// <summary>
                /// 用戶號碼(虛擬帳號後10碼)
                /// </summary>
                public string CustomerNo = "";
                /// <summary>
                /// 交易日期 yyMMdd 民國年8碼
                /// </summary>
                public string TxnDate8 = "";
                /// <summary>
                /// 交易時間 HHmmss
                /// </summary>
                public string TxnTime6 = "";
                /// <summary>
                /// 交易金額
                /// </summary>
                public decimal TxnAmount = 0;
                /// <summary>
                /// 更正註記 0:正常交易 1:更正
                /// </summary>
                public string ECMark = "";
                /// <summary>
                /// 交易來源 00.全部 01.臨櫃 02.匯款 03.ATM 04.EDI 05.網路銀行 06.語音銀行 07.中油營收 80.中國信託 81.超商(7-11) 82.超商(全家) 83.超商(OK) 84.超商(福客多) 85.超商(萊爾富) 86.郵局 87.信用卡 91.分行手續費(7-11) 92.分行手續費(全家) 93.分行手續費(OK) 94.分行手續費(福客多) 95分行手續費(萊爾富) 99.分行手續費
                /// </summary>
                public string TxnWay = "";
                /// <summary>
                /// 作帳日期 民國年YYYYMMDD
                /// </summary>
                public string AccountDate = "";
                /// <summary>
                /// 繳款行 3碼
                /// </summary>
                public string TxnBank = "";
                /// <summary>
                /// 保留欄位
                /// </summary>
                public string filler = "";
                /// <summary>
                /// 原始資料
                /// </summary>
                public string Data = "";
            }

            public class Footer
            {
                public static int[] FiledsLen = { 1, 10, 16, 53 };
                public Footer()
                {

                }

                /// <summary>
                /// 錄別 尾路：3
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 合計筆數
                /// </summary>
                public decimal TotalCount = 0;
                /// <summary>
                /// 合計金額
                /// </summary>
                public decimal TotalAmount = 0;
                /// <summary>
                /// 保留
                /// </summary>
                public string Filler = "";
            }
        }

        public class Layout08
        {
            public const int RecordLens = 120;

            public class Header
            {
                public static int[] FiledsLen = { 1, 1, 8, 8, 6, 6, 50, 40 };
                public Header()
                {

                }

                /// <summary>
                /// 錄別 首錄：1
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 查詢碼 1:依交易日 2:依作帳日
                /// </summary>
                public string qCode = "";
                /// <summary>
                /// 查詢起日
                /// </summary>
                public string qStartDate = "";
                /// <summary>
                /// 查詢迄日
                /// </summary>
                public string qEndDate = "";
                /// <summary>
                /// 查詢起號
                /// </summary>
                public string qStartNo = "";
                /// <summary>
                /// 查詢迄號
                /// </summary>
                public string qEndNo = "";
                /// <summary>
                /// 保留1
                /// </summary>
                public string Filler1 = "";
                /// <summary>
                /// 保留2
                /// </summary>
                public string Filler2 = "";
            }

            public class Detail
            {
                public static int[] FiledsLen = { 1, 6, 4, 12, 8, 6, 13, 1, 2, 8, 3, 16, 40 };
                public Detail()
                {

                }

                /// <summary>
                /// 錄別 資料：2
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 交易序號
                /// </summary>
                public string TxnNo = "";
                /// <summary>
                /// 商家代號(虛擬帳號前4碼)
                /// </summary>
                public string ReceiveType = "";
                /// <summary>
                /// 用戶號碼(虛擬帳號後10碼)
                /// </summary>
                public string CustomerNo = "";
                /// <summary>
                /// 交易日期 yyMMdd 民國年2碼
                /// </summary>
                public string TxnDate6 = "";
                /// <summary>
                /// 交易時間 HHmmss
                /// </summary>
                public string TxnTime6 = "";
                /// <summary>
                /// 交易金額
                /// </summary>
                public decimal TxnAmount = 0;
                /// <summary>
                /// 更正註記 0:正常交易 1:更正
                /// </summary>
                public string ECMark = "";
                /// <summary>
                /// 交易來源 00.全部 01.臨櫃 02.匯款 03.ATM 04.EDI 05.網路銀行 06.語音銀行 07.中油營收 80.中國信託 81.超商(7-11) 82.超商(全家) 83.超商(OK) 84.超商(福客多) 85.超商(萊爾富) 86.郵局 87.信用卡 91.分行手續費(7-11) 92.分行手續費(全家) 93.分行手續費(OK) 94.分行手續費(福客多) 95分行手續費(萊爾富) 99.分行手續費
                /// </summary>
                public string TxnWay = "";
                /// <summary>
                /// 作帳日期 民國年YYMMDD
                /// </summary>
                public string AccountDate = "";
                /// <summary>
                /// 繳款行 3碼
                /// </summary>
                public string TxnBank = "";
                /// <summary>
                /// 保留欄位
                /// </summary>
                public string filler = "";
                /// <summary>
                /// 備註
                /// </summary>
                public string Remark = "";
                /// <summary>
                /// 原始資料
                /// </summary>
                public string Data = "";
            }

            public class Footer
            {
                public static int[] FiledsLen = { 1, 10, 16, 53,40 };
                public Footer()
                {

                }

                /// <summary>
                /// 錄別 尾路：3
                /// </summary>
                public string Type = "";
                /// <summary>
                /// 合計筆數
                /// </summary>
                public decimal TotalCount = 0;
                /// <summary>
                /// 合計金額
                /// </summary>
                public decimal TotalAmount = 0;
                /// <summary>
                /// 保留1
                /// </summary>
                public string Filler1 = "";
                /// <summary>
                /// 保留2
                /// </summary>
                public string Filler2 = "";
            }
        }
        #endregion

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        private string _err_msg = "";
        public string ErrMsg
        {
            get { return _err_msg; }
        }

        public D00I70()
        {

        }

        #region private method
        ///// <summary>
        ///// 檢查6碼的日期
        ///// </summary>
        ///// <param name="date6"></param>
        ///// <returns></returns>
        //private bool CheckDate6(string date6,out string msg)
        //{
        //    bool rc = false;
        //    msg = "";

        //    if (date6 == null || date6.Trim().Length != 6)
        //    {
        //        msg = string.Format("日期資料錯誤");
        //        return rc;
        //    }
        //    date6 = date6.Trim();

        //    try
        //    {
        //        Int32 dd = Int32.Parse(date6);
        //    }
        //    catch(Exception ex)
        //    {
        //        msg = string.Format("日期格式錯誤");
        //        return rc;
        //    }

        //    rc = true;
        //    return rc;
        //}
        #endregion

        #region layout1
        public bool ParseLayout1(string data, out Layout01 layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout01();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout01.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout01.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.TxnNo = FieldDatas[idx];

            idx++;
            layout.ReceiveType = FieldDatas[idx];

            idx++;
            layout.CustomerNo = FieldDatas[idx];

            idx++;
            layout.TxnDate6 = FieldDatas[idx];

            idx++;
            layout.TxnTime6 = FieldDatas[idx];

            idx++;
            try
            {
                layout.TxnAmount = decimal.Parse(FieldDatas[idx].Substring(0, 11));//末兩碼是角分
            }
            catch (Exception)
            {
                _err_msg = string.Format("交易金額欄位錯誤，交易金額={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.ECMark = FieldDatas[idx];
            if (layout.ECMark != D00I70ECMarkCodeTexts.NORMAL_CODE && layout.ECMark != D00I70ECMarkCodeTexts.RECTIFY_CODE)
            {
                _err_msg = string.Format("更正記號欄位錯誤，更正記號={0}", FieldDatas[idx]);
                return rc;
            }

            layout.Data = data;
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseFileFormat01(string full_file_name, out Layout01[] datas)
        {
            bool rc = false;
            _err_msg = "";
            datas = null;

            #region 判斷檔案在不在
            System.IO.FileInfo fi = new System.IO.FileInfo(full_file_name);
            if (!fi.Exists)
            {
                _err_msg = string.Format("檔案不存在，檔名={0}", full_file_name);
                return rc;
            }
            #endregion

            #region 拆解檔案
            System.Text.StringBuilder logs = new StringBuilder();
            Int32 count = 0;
            Int32 countSuccess = 0;
            Int32 countFail = 0;
            List<Layout01> buffs = new List<Layout01>();
            Layout01 layout = new Layout01();
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(full_file_name, System.Text.Encoding.Default))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        count++;
                        #region
                        try
                        {
                            if (!ParseLayout1(line, out layout))
                            {
                                countFail++;
                                logs.AppendLine(string.Format("第{0}筆，拆解失敗，錯誤訊息={1}", count, _err_msg));
                            }
                            else
                            {
                                countSuccess++;
                                logs.AppendLine(string.Format("第{0}筆，拆解成功", count));
                                buffs.Add(layout);
                            }
                        }
                        catch (Exception ex1)
                        {
                            countFail++;
                            logs.AppendLine(string.Format("第{0}筆，拆解發生錯誤，錯誤訊息={1}", count, ex1.Message));
                        }
                        #endregion
                    }
                }
                datas = buffs.ToArray<Layout01>();
                rc = true;
            }
            catch (Exception ex)
            {
                logs.AppendLine(string.Format("拆解檔案發生錯誤，錯誤訊息={1}", count, ex.Message));
            }
            #endregion

            return rc;
        }
        #endregion

        #region layout2
        public bool ParseLayout2(string data, out Layout02 layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout02();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout02.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout02.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.TxnNo = FieldDatas[idx];

            idx++;
            layout.ReceiveType = FieldDatas[idx];

            idx++;
            layout.CustomerNo = FieldDatas[idx];

            idx++;
            layout.TxnDate6 = FieldDatas[idx];

            idx++;
            layout.TxnTime6 = FieldDatas[idx];

            idx++;
            try
            {
                layout.TxnAmount = decimal.Parse(FieldDatas[idx].Substring(0, 11));//末兩碼是角分
            }
            catch (Exception)
            {
                _err_msg = string.Format("交易金額欄位錯誤，交易金額={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.ECMark = FieldDatas[idx];
            if (layout.ECMark != D00I70ECMarkCodeTexts.NORMAL_CODE && layout.ECMark != D00I70ECMarkCodeTexts.RECTIFY_CODE)
            {
                _err_msg = string.Format("更正記號欄位錯誤，更正記號={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.TxnWay = FieldDatas[idx];

            idx++;
            layout.AccountDate = FieldDatas[idx];

            idx++;
            layout.filler = FieldDatas[idx];

            layout.Data = data;
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseFileFormat02(string full_file_name,out Layout02[] datas)
        {
            bool rc = false;
            _err_msg = "";
            datas = null;

            #region 判斷檔案在不在
            System.IO.FileInfo fi = new System.IO.FileInfo(full_file_name);
            if(!fi.Exists)
            {
                _err_msg = string.Format("檔案不存在，檔名={0}", full_file_name);
                return rc;
            }
            #endregion

            #region 拆解檔案
            System.Text.StringBuilder logs = new StringBuilder();
            Int32 count = 0;
            Int32 countSuccess = 0;
            Int32 countFail = 0;
            List<Layout02> buffs = new List<Layout02>();
            Layout02 layout = new Layout02();
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(full_file_name, System.Text.Encoding.Default))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        count++;
                        #region
                        try
                        {
                            if (!ParseLayout2(line, out layout))
                            {
                                countFail++;
                                logs.AppendLine(string.Format("第{0}筆，拆解失敗，錯誤訊息={1}", count, _err_msg));
                            }
                            else
                            {
                                countSuccess++;
                                logs.AppendLine(string.Format("第{0}筆，拆解成功", count));
                                buffs.Add(layout);
                            }
                        }
                        catch (Exception ex1)
                        {
                            countFail++;
                            logs.AppendLine(string.Format("第{0}筆，拆解發生錯誤，錯誤訊息={1}", count, ex1.Message));
                        }
                        #endregion
                    }
                }
                datas = buffs.ToArray<Layout02>();
                rc = true;
            }
            catch (Exception ex)
            {
                logs.AppendLine(string.Format("拆解檔案發生錯誤，錯誤訊息={1}", count, ex.Message));
            }
            #endregion

            _err_msg = logs.ToString();
            return rc;
        }

        public bool Layout2ToCancelDebts(string full_file_name,Layout02[] datas, out CancelDebtsEntity[] CancelDebts)
        {
            bool rc = false;
            _err_msg = "";
            CancelDebts = null;
            List<CancelDebtsEntity> buffs = new List<CancelDebtsEntity>();
            DateTime now = DateTime.Now;

            if (datas != null && datas.Length > 0)
            {
                string fileName = "UPFILE:" + Path.GetFileName(full_file_name);
                foreach (Layout02 layout in datas)
                {
                    #region 組cancel_debts
                    CancelDebtsEntity cancel_debts = new CancelDebtsEntity();
                    cancel_debts.ReceiveType = layout.ReceiveType;
                    cancel_debts.CancelNo = layout.ReceiveType + layout.CustomerNo;
                    cancel_debts.AccountDate = "1" + layout.AccountDate;
                    cancel_debts.ReceiveTime = layout.TxnTime6;
                    cancel_debts.ReceiveBank = "";
                    cancel_debts.PayDueDate = "";
                    cancel_debts.Reserve1 = "";
                    cancel_debts.Remark = "";
                    cancel_debts.ReceiveDate = "1" + layout.TxnDate6;
                    cancel_debts.ReceiveWay = layout.TxnWay;
                    cancel_debts.ReceiveAmount = layout.TxnAmount;

                    #region [MDY:20160607] 紀錄更正記號
                    cancel_debts.Reserve2 = layout.ECMark;
                    #endregion

                    cancel_debts.FileName = fileName;
                    cancel_debts.ModifyDate = now;
                    cancel_debts.RollbackDate = null;
                    cancel_debts.CancelDate = null;
                    cancel_debts.Status = CancelDebtsStatusCodeTexts.IS_WAITING_CODE;
                    cancel_debts.SourceData = layout.Data;
                    cancel_debts.SourceSeq = Int32.Parse(layout.TxnNo);
                    buffs.Add(cancel_debts);
                    #endregion
                }

                CancelDebts = buffs.ToArray<CancelDebtsEntity>();
            }

            rc = true;
            return rc;
        }
        #endregion

        #region layout3
        public bool ParseLayout3Header(string data, out Layout03.Header layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout03.Header();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout03.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout03.Header.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            layout.qCode = FieldDatas[idx];

            idx++;
            layout.qStartDate = FieldDatas[idx];

            idx++;
            layout.qEndDate = FieldDatas[idx];

            idx++;
            layout.qStartNo = FieldDatas[idx];

            idx++;
            layout.qEndNo = FieldDatas[idx];

            idx++;
            layout.Filler = FieldDatas[idx];
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseLayout3Detail(string data, out Layout03.Detail layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout03.Detail();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout03.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout03.Detail.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            layout.TxnNo = FieldDatas[idx];

            idx++;
            layout.ReceiveType = FieldDatas[idx];

            idx++;
            layout.CustomerNo = FieldDatas[idx];

            idx++;
            layout.TxnDate6 = FieldDatas[idx];

            idx++;
            layout.TxnTime6 = FieldDatas[idx];

            idx++;
            try
            {
                layout.TxnAmount = decimal.Parse(FieldDatas[idx].Substring(0, 11));//末兩碼是角分
            }
            catch (Exception)
            {
                _err_msg = string.Format("交易金額欄位錯誤，交易金額={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.ECMark = FieldDatas[idx];
            if (layout.ECMark != D00I70ECMarkCodeTexts.NORMAL_CODE && layout.ECMark != D00I70ECMarkCodeTexts.RECTIFY_CODE)
            {
                _err_msg = string.Format("更正記號欄位錯誤，更正記號={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.TxnWay = FieldDatas[idx];

            idx++;
            layout.AccountDate = FieldDatas[idx];

            idx++;
            layout.TxnBank = FieldDatas[idx];

            idx++;
            layout.filler = FieldDatas[idx];

            layout.Data = data;
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseLayout3Footer(string data, out Layout03.Footer layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout03.Footer();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout03.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout03.Footer.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            try
            {
                layout.TotalCount = decimal.Parse(FieldDatas[idx]);
            }
            catch(Exception)
            {
                _err_msg = string.Format("總筆數欄位資料錯誤，總筆數欄位={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            try
            {
                layout.TotalAmount = decimal.Parse(FieldDatas[idx]) / 100;
            }
            catch(Exception)
            {
                _err_msg = string.Format("總金額欄位資料錯誤，總金額欄位={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.Filler = FieldDatas[idx];
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseFileFormat03(string full_file_name, out Layout03.Detail[] datas)
        {
            bool rc = false;
            _err_msg = "";
            datas = null;

            #region 判斷檔案在不在
            System.IO.FileInfo fi = new System.IO.FileInfo(full_file_name);
            if (!fi.Exists)
            {
                _err_msg = string.Format("檔案不存在，檔名={0}", full_file_name);
                return rc;
            }
            #endregion

            #region 拆解檔案
            System.Text.StringBuilder logs = new StringBuilder();
            Int32 count = 0;
            decimal total = 0;
            Int32 countSuccess = 0;
            Int32 countFail = 0;
            Layout03.Header header = new Layout03.Header();
            List<Layout03.Detail> buffs = new List<Layout03.Detail>();
            Layout03.Footer footer = new Layout03.Footer();
            Layout03.Detail layout = new Layout03.Detail();
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(full_file_name, System.Text.Encoding.Default))
                {
                    string line = "";
                    Int32 LineNo = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        LineNo++;
                        #region 逐筆處理
                        try
                        {
                            string type = line.Substring(0, 1);
                            if (type == "1")
                            {
                                #region 首筆
                                if (!ParseLayout3Header(line, out header))
                                {
                                    header = null;
                                    logs.AppendLine(string.Format("拆解首筆失敗，錯誤訊息={0}",  _err_msg));
                                    _err_msg = logs.ToString();
                                    return rc;
                                }
                                #endregion
                            }
                            else if(type=="2")
                            {
                                count++;
                                #region 資料
                                if (!ParseLayout3Detail(line, out layout))
                                {
                                    countFail++;
                                    logs.AppendLine(string.Format("第{0}筆拆解失敗，錯誤訊息={1}", count, _err_msg));
                                }
                                else
                                {
                                    countSuccess++;
                                    total += layout.TxnAmount;
                                    buffs.Add(layout);
                                }
                                #endregion
                            }
                            else if (type=="3")
                            {
                                #region 尾筆
                                if (!ParseLayout3Footer(line, out footer))
                                {
                                    footer = null;
                                    logs.AppendLine(string.Format("拆解尾筆失敗，錯誤訊息={0}",  _err_msg));
                                    _err_msg = logs.ToString();
                                    return rc;
                                }
                                #endregion
                            }
                            else
                            {
                                #region 檔案錯誤
                                _err_msg = string.Format("檔案出現非首、尾筆或是資料列，錯誤資料={0}",line);
                                datas = null;
                                return rc;
                                #endregion
                            }
                        }
                        catch (Exception ex1)
                        {
                            countFail++;
                            logs.AppendLine(string.Format("第{0}列，拆解發生錯誤，錯誤訊息={1}", LineNo, ex1.Message));
                        }
                        #endregion
                    }
                }

                #region 檢查是否有首、尾筆
                if (header == null)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("檔案沒有首筆")).ToString();
                    return rc;
                }
                if(footer == null)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("檔案沒有尾筆")).ToString();
                    return rc;
                }
                #endregion

                #region 檢察總筆數、總金額
                if (count != footer.TotalCount)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("總筆數不合，檔案筆數={0}，資料筆數={1}", footer.TotalCount, count)).ToString();
                    return rc;
                }
                if (total != footer.TotalAmount)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("總金額不合，檔案金額={0}，資料金額={1}", footer.TotalAmount, total)).ToString();
                    return rc;
                }
                #endregion

                datas = buffs.ToArray<Layout03.Detail>();
                rc = true;
            }
            catch (Exception ex)
            {
                logs.AppendLine(string.Format("拆解檔案發生錯誤，錯誤訊息={1}", count, ex.Message));
                _err_msg = logs.ToString();
            }
            #endregion

            return rc;
        }

        public bool Layout3DetailToCancelDebts(string full_file_name, Layout03.Detail[] datas, out CancelDebtsEntity[] CancelDebts)
        {
            bool rc = false;
            _err_msg = "";
            CancelDebts = null;
            List<CancelDebtsEntity> buffs = new List<CancelDebtsEntity>();
            DateTime now = DateTime.Now;

            if (datas != null && datas.Length > 0)
            {
                string fileName = "UPFILE:" + Path.GetFileName(full_file_name);
                foreach (Layout03.Detail layout in datas)
                {
                    #region 組cancel_debts
                    CancelDebtsEntity cancel_debts = new CancelDebtsEntity();
                    cancel_debts.ReceiveType = layout.ReceiveType;
                    cancel_debts.CancelNo = layout.ReceiveType + layout.CustomerNo;
                    cancel_debts.AccountDate = "1" + layout.AccountDate;
                    cancel_debts.ReceiveTime = layout.TxnTime6;
                    cancel_debts.ReceiveBank = layout.TxnBank;
                    cancel_debts.PayDueDate = "";
                    cancel_debts.Reserve1 = "";
                    cancel_debts.Remark = "";
                    cancel_debts.ReceiveDate = "1" + layout.TxnDate6;
                    cancel_debts.ReceiveWay = layout.TxnWay;
                    cancel_debts.ReceiveAmount = layout.TxnAmount;

                    #region [MDY:20160607] 紀錄更正記號
                    cancel_debts.Reserve2 = layout.ECMark;
                    #endregion

                    cancel_debts.FileName = fileName;
                    cancel_debts.ModifyDate = now;
                    cancel_debts.RollbackDate = null;
                    cancel_debts.CancelDate = null;
                    cancel_debts.Status = CancelDebtsStatusCodeTexts.IS_WAITING_CODE;
                    cancel_debts.SourceData = layout.Data;
                    cancel_debts.SourceSeq = Int32.Parse(layout.TxnNo);
                    buffs.Add(cancel_debts);
                    #endregion
                }

                CancelDebts = buffs.ToArray<CancelDebtsEntity>();
            }

            rc = true;
            return rc;
        }
        #endregion

        #region layout4
        public bool ParseLayout4(string data, out Layout04 layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout04();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout04.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout04.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.TxnNo = FieldDatas[idx];

            idx++;
            layout.ReceiveType = FieldDatas[idx];

            idx++;
            layout.CustomerNo = FieldDatas[idx];

            idx++;
            layout.TxnDate6 = FieldDatas[idx];

            idx++;
            layout.TxnTime6 = FieldDatas[idx];

            idx++;
            try
            {
                layout.TxnAmount = decimal.Parse(FieldDatas[idx].Substring(0, 11));//末兩碼是角分
            }
            catch (Exception)
            {
                _err_msg = string.Format("交易金額欄位錯誤，交易金額={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.ECMark = FieldDatas[idx];
            if (layout.ECMark != D00I70ECMarkCodeTexts.NORMAL_CODE && layout.ECMark != D00I70ECMarkCodeTexts.RECTIFY_CODE)
            {
                _err_msg = string.Format("更正記號欄位錯誤，更正記號={0}", FieldDatas[idx]);
                return rc;
            }

            layout.Data = data;
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseFileFormat04(string full_file_name, out Layout04[] datas)
        {
            bool rc = false;
            _err_msg = "";
            datas = null;

            #region 判斷檔案在不在
            System.IO.FileInfo fi = new System.IO.FileInfo(full_file_name);
            if (!fi.Exists)
            {
                _err_msg = string.Format("檔案不存在，檔名={0}", full_file_name);
                return rc;
            }
            #endregion

            #region 拆解檔案
            System.Text.StringBuilder logs = new StringBuilder();
            Int32 count = 0;
            Int32 countSuccess = 0;
            Int32 countFail = 0;
            List<Layout04> buffs = new List<Layout04>();
            Layout04 layout = new Layout04();
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(full_file_name, System.Text.Encoding.Default))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        count++;
                        #region
                        try
                        {
                            if (!ParseLayout4(line, out layout))
                            {
                                countFail++;
                                logs.AppendLine(string.Format("第{0}筆，拆解失敗，錯誤訊息={1}", count, _err_msg));
                            }
                            else
                            {
                                countSuccess++;
                                logs.AppendLine(string.Format("第{0}筆，拆解成功", count));
                                buffs.Add(layout);
                            }
                        }
                        catch (Exception ex1)
                        {
                            countFail++;
                            logs.AppendLine(string.Format("第{0}筆，拆解發生錯誤，錯誤訊息={1}", count, ex1.Message));
                        }
                        #endregion
                    }
                }
                datas = buffs.ToArray<Layout04>();
                rc = true;
            }
            catch (Exception ex)
            {
                logs.AppendLine(string.Format("拆解檔案發生錯誤，錯誤訊息={1}", count, ex.Message));
            }
            #endregion

            return rc;
        }
        #endregion

        #region layout5
        public bool ParseLayout5(string data, out Layout05 layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout05();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout05.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout05.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.TxnNo = FieldDatas[idx];

            idx++;
            layout.ReceiveType = FieldDatas[idx];

            idx++;
            layout.CustomerNo = FieldDatas[idx];

            idx++;
            layout.TxnDate6 = FieldDatas[idx];

            idx++;
            layout.TxnTime6 = FieldDatas[idx];

            idx++;
            try
            {
                layout.TxnAmount = decimal.Parse(FieldDatas[idx].Substring(0, 11));//末兩碼是角分
            }
            catch (Exception)
            {
                _err_msg = string.Format("交易金額欄位錯誤，交易金額={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.ECMark = FieldDatas[idx];
            if (layout.ECMark != D00I70ECMarkCodeTexts.NORMAL_CODE && layout.ECMark != D00I70ECMarkCodeTexts.RECTIFY_CODE)
            {
                _err_msg = string.Format("更正記號欄位錯誤，更正記號={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.TxnWay = FieldDatas[idx];

            idx++;
            layout.AccountDate = FieldDatas[idx];

            idx++;
            layout.TxnBank = FieldDatas[idx];

            idx++;
            layout.filler = FieldDatas[idx];

            layout.Data = data;
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseFileFormat05(string full_file_name, out Layout05[] datas)
        {
            bool rc = false;
            _err_msg = "";
            datas = null;

            #region 判斷檔案在不在
            System.IO.FileInfo fi = new System.IO.FileInfo(full_file_name);
            if (!fi.Exists)
            {
                _err_msg = string.Format("檔案不存在，檔名={0}", full_file_name);
                return rc;
            }
            #endregion

            #region 拆解檔案
            System.Text.StringBuilder logs = new StringBuilder();
            Int32 count = 0;
            Int32 countSuccess = 0;
            Int32 countFail = 0;
            List<Layout05> buffs = new List<Layout05>();
            Layout05 layout = new Layout05();
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(full_file_name, System.Text.Encoding.Default))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        count++;
                        #region
                        try
                        {
                            if (!ParseLayout5(line, out layout))
                            {
                                countFail++;
                                logs.AppendLine(string.Format("第{0}筆，拆解失敗，錯誤訊息={1}", count, _err_msg));
                            }
                            else
                            {
                                countSuccess++;
                                logs.AppendLine(string.Format("第{0}筆，拆解成功", count));
                                buffs.Add(layout);
                            }
                        }
                        catch (Exception ex1)
                        {
                            countFail++;
                            logs.AppendLine(string.Format("第{0}筆，拆解發生錯誤，錯誤訊息={1}", count, ex1.Message));
                        }
                        #endregion
                    }
                }
                datas = buffs.ToArray<Layout05>();
                rc = true;
            }
            catch (Exception ex)
            {
                logs.AppendLine(string.Format("拆解檔案發生錯誤，錯誤訊息={1}", count, ex.Message));
            }
            #endregion

            _err_msg = logs.ToString();
            return rc;
        }

        public bool Layout5ToCancelDebts(string full_file_name, Layout05[] datas, out CancelDebtsEntity[] CancelDebts)
        {
            bool rc = false;
            _err_msg = "";
            CancelDebts = null;
            List<CancelDebtsEntity> buffs = new List<CancelDebtsEntity>();
            DateTime now = DateTime.Now;

            if (datas != null && datas.Length > 0)
            {
                string fileName = "UPFILE:" + Path.GetFileName(full_file_name);
                foreach (Layout05 layout in datas)
                {
                    #region 組cancel_debts
                    CancelDebtsEntity cancel_debts = new CancelDebtsEntity();
                    cancel_debts.ReceiveType = layout.ReceiveType;
                    cancel_debts.CancelNo = layout.ReceiveType + layout.CustomerNo;
                    cancel_debts.AccountDate = "1" + layout.AccountDate;
                    cancel_debts.ReceiveTime = layout.TxnTime6;
                    cancel_debts.ReceiveBank = layout.TxnBank;
                    cancel_debts.PayDueDate = "";
                    cancel_debts.Reserve1 = "";
                    cancel_debts.Remark = "";
                    cancel_debts.ReceiveDate = "1" + layout.TxnDate6;
                    cancel_debts.ReceiveWay = layout.TxnWay;
                    cancel_debts.ReceiveAmount = layout.TxnAmount;

                    #region [MDY:20160607] 紀錄更正記號
                    cancel_debts.Reserve2 = layout.ECMark;
                    #endregion

                    cancel_debts.FileName = fileName;
                    cancel_debts.ModifyDate = now;
                    cancel_debts.RollbackDate = null;
                    cancel_debts.CancelDate = null;
                    cancel_debts.Status = CancelDebtsStatusCodeTexts.IS_WAITING_CODE;
                    cancel_debts.SourceData = layout.Data;
                    cancel_debts.SourceSeq = Int32.Parse(layout.TxnNo);
                    buffs.Add(cancel_debts);
                    #endregion
                }

                CancelDebts = buffs.ToArray<CancelDebtsEntity>();
            }

            rc = true;
            return rc;
        }
        #endregion

        #region layout6
        public bool ParseLayout6Header(string data, out Layout06.Header layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout06.Header();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout06.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout06.Header.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            layout.qCode = FieldDatas[idx];

            idx++;
            layout.qStartDate = FieldDatas[idx];

            idx++;
            layout.qEndDate = FieldDatas[idx];

            idx++;
            layout.qStartNo = FieldDatas[idx];

            idx++;
            layout.qEndNo = FieldDatas[idx];

            idx++;
            layout.Filler = FieldDatas[idx];
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseLayout6Detail(string data, out Layout06.Detail layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout06.Detail();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout06.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout06.Detail.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            layout.TxnNo = FieldDatas[idx];

            idx++;
            layout.ReceiveType = FieldDatas[idx];

            idx++;
            layout.CustomerNo = FieldDatas[idx];

            idx++;
            layout.TxnDate6 = FieldDatas[idx];

            idx++;
            layout.TxnTime6 = FieldDatas[idx];

            idx++;
            try
            {
                layout.TxnAmount = decimal.Parse(FieldDatas[idx].Substring(0, 11));//末兩碼是角分
            }
            catch (Exception)
            {
                _err_msg = string.Format("交易金額欄位錯誤，交易金額={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.ECMark = FieldDatas[idx];
            if (layout.ECMark != D00I70ECMarkCodeTexts.NORMAL_CODE && layout.ECMark != D00I70ECMarkCodeTexts.RECTIFY_CODE)
            {
                _err_msg = string.Format("更正記號欄位錯誤，更正記號={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.TxnWay = FieldDatas[idx];

            idx++;
            layout.AccountDate = FieldDatas[idx];

            idx++;
            layout.TxnBank = FieldDatas[idx];

            idx++;
            layout.filler = FieldDatas[idx];

            layout.Data = data;
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseLayout6Footer(string data, out Layout06.Footer layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout06.Footer();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout06.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout06.Footer.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            try
            {
                layout.TotalCount = decimal.Parse(FieldDatas[idx]);
            }
            catch (Exception)
            {
                _err_msg = string.Format("總筆數欄位資料錯誤，總筆數欄位={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            try
            {
                layout.TotalAmount = decimal.Parse(FieldDatas[idx]) / 100;
            }
            catch (Exception)
            {
                _err_msg = string.Format("總金額欄位資料錯誤，總金額欄位={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.Filler = FieldDatas[idx];
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseFileFormat06(string full_file_name, out Layout06.Detail[] datas)
        {
            bool rc = false;
            _err_msg = "";
            datas = null;

            #region 判斷檔案在不在
            System.IO.FileInfo fi = new System.IO.FileInfo(full_file_name);
            if (!fi.Exists)
            {
                _err_msg = string.Format("檔案不存在，檔名={0}", full_file_name);
                return rc;
            }
            #endregion

            #region 拆解檔案
            System.Text.StringBuilder logs = new StringBuilder();
            Int32 count = 0;
            decimal total = 0;
            Int32 countSuccess = 0;
            Int32 countFail = 0;
            Layout06.Header header = new Layout06.Header();
            List<Layout06.Detail> buffs = new List<Layout06.Detail>();
            Layout06.Footer footer = new Layout06.Footer();
            Layout06.Detail layout = new Layout06.Detail();
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(full_file_name, System.Text.Encoding.Default))
                {
                    string line = "";
                    Int32 LineNo = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        LineNo++;
                        #region 逐筆處理
                        try
                        {
                            string type = line.Substring(0, 1);
                            if (type == "1")
                            {
                                #region 首筆
                                if (!ParseLayout6Header(line, out header))
                                {
                                    header = null;
                                    logs.AppendLine(string.Format("拆解首筆失敗，錯誤訊息={0}", _err_msg));
                                    _err_msg = logs.ToString();
                                    return rc;
                                }
                                #endregion
                            }
                            else if (type == "2")
                            {
                                count++;
                                #region 資料
                                if (!ParseLayout6Detail(line, out layout))
                                {
                                    countFail++;
                                    logs.AppendLine(string.Format("第{0}筆拆解失敗，錯誤訊息={1}", count, _err_msg));
                                }
                                else
                                {
                                    countSuccess++;
                                    total += layout.TxnAmount;
                                    buffs.Add(layout);
                                }
                                #endregion
                            }
                            else if (type == "3")
                            {
                                #region 尾筆
                                if (!ParseLayout6Footer(line, out footer))
                                {
                                    footer = null;
                                    logs.AppendLine(string.Format("拆解尾筆失敗，錯誤訊息={0}", _err_msg));
                                    _err_msg = logs.ToString();
                                    return rc;
                                }
                                #endregion
                            }
                            else
                            {
                                #region 檔案錯誤
                                _err_msg = string.Format("檔案出現非首、尾筆或是資料列，錯誤資料={0}", line);
                                datas = null;
                                return rc;
                                #endregion
                            }
                        }
                        catch (Exception ex1)
                        {
                            countFail++;
                            logs.AppendLine(string.Format("第{0}列，拆解發生錯誤，錯誤訊息={1}", LineNo, ex1.Message));
                        }
                        #endregion
                    }
                }

                #region 檢查是否有首、尾筆
                if (header == null)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("檔案沒有首筆")).ToString();
                    return rc;
                }
                if (footer == null)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("檔案沒有尾筆")).ToString();
                    return rc;
                }
                #endregion

                #region 檢察總筆數、總金額
                if (count != footer.TotalCount)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("總筆數不合，檔案筆數={0}，資料筆數={1}", footer.TotalCount, count)).ToString();
                    return rc;
                }
                if (total != footer.TotalAmount)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("總金額不合，檔案金額={0}，資料金額={1}", footer.TotalAmount, total)).ToString();
                    return rc;
                }
                #endregion

                datas = buffs.ToArray<Layout06.Detail>();
                rc = true;
            }
            catch (Exception ex)
            {
                logs.AppendLine(string.Format("拆解檔案發生錯誤，錯誤訊息={1}", count, ex.Message));
                _err_msg = logs.ToString();
            }
            #endregion

            return rc;
        }

        public bool Layout6DetailToCancelDebts(string full_file_name, Layout06.Detail[] datas, out CancelDebtsEntity[] CancelDebts)
        {
            bool rc = false;
            _err_msg = "";
            CancelDebts = null;
            List<CancelDebtsEntity> buffs = new List<CancelDebtsEntity>();
            DateTime now = DateTime.Now;

            if (datas != null && datas.Length > 0)
            {
                string fileName = "UPFILE:" + Path.GetFileName(full_file_name);
                foreach (Layout06.Detail layout in datas)
                {
                    #region 組cancel_debts
                    CancelDebtsEntity cancel_debts = new CancelDebtsEntity();
                    cancel_debts.ReceiveType = layout.ReceiveType;
                    cancel_debts.CancelNo = layout.ReceiveType + layout.CustomerNo;
                    cancel_debts.AccountDate = "1" + layout.AccountDate;
                    cancel_debts.ReceiveTime = layout.TxnTime6;
                    cancel_debts.ReceiveBank = layout.TxnBank;
                    cancel_debts.PayDueDate = "";
                    cancel_debts.Reserve1 = "";
                    cancel_debts.Remark = "";
                    cancel_debts.ReceiveDate = "1" + layout.TxnDate6;
                    cancel_debts.ReceiveWay = layout.TxnWay;
                    cancel_debts.ReceiveAmount = layout.TxnAmount;

                    #region [MDY:20160607] 紀錄更正記號
                    cancel_debts.Reserve2 = layout.ECMark;
                    #endregion

                    cancel_debts.FileName = fileName;
                    cancel_debts.ModifyDate = now;
                    cancel_debts.RollbackDate = null;
                    cancel_debts.CancelDate = null;
                    cancel_debts.Status = CancelDebtsStatusCodeTexts.IS_WAITING_CODE;
                    cancel_debts.SourceData = layout.Data;
                    cancel_debts.SourceSeq = Int32.Parse(layout.TxnNo);
                    buffs.Add(cancel_debts);
                    #endregion
                }

                CancelDebts = buffs.ToArray<CancelDebtsEntity>();
            }

            rc = true;
            return rc;
        }
        #endregion

        #region layout7
        public bool ParseLayout7Header(string data, out Layout07.Header layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout07.Header();

            //不會有中文字
            #region 檢查資料長度
            if (String.IsNullOrWhiteSpace(data) || data.Length != Layout07.RecordLens)
            {
                int lens = data == null ? 0 : data.Length;
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            //data = data.Trim();

            int[] FieldLens = Layout07.Header.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            layout.qCode = FieldDatas[idx];

            idx++;
            layout.qStartDate = FieldDatas[idx];

            idx++;
            layout.qEndDate = FieldDatas[idx];

            idx++;
            layout.qStartNo = FieldDatas[idx];

            idx++;
            layout.qEndNo = FieldDatas[idx];

            idx++;
            layout.Filler = FieldDatas[idx];
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseLayout7Detail(string data, out Layout07.Detail layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout07.Detail();

            //不會有中文字
            #region 檢查資料長度
            if (String.IsNullOrWhiteSpace(data) || data.Length != Layout07.RecordLens)
            {
                int lens = data == null ? 0 : data.Length;
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            //data = data.Trim();

            int[] FieldLens = Layout07.Detail.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            layout.TxnNo = FieldDatas[idx];

            idx++;
            layout.ReceiveType = FieldDatas[idx];

            idx++;
            layout.CustomerNo = FieldDatas[idx];

            #region 交易金額
            {
                idx++;

                #region [Old]
                //layout.TxnDate8 = FieldDatas[idx];
                #endregion

                DateTime date;
                layout.TxnDate8 = FieldDatas[idx].Trim();
                if (!Common.TryConvertTWDate8(layout.TxnDate8, out date))
                {
                    _err_msg = string.Format("交易日期欄位錯誤，交易日期={0}", FieldDatas[idx]);
                    return rc;
                }
            }
            #endregion

            idx++;
            layout.TxnTime6 = FieldDatas[idx];

            #region 交易金額 (11.2 末兩碼是角分)
            {
                idx++;

                #region [Old]
                //try
                //{
                //    layout.TxnAmount = decimal.Parse(FieldDatas[idx].Substring(0, 11));//末兩碼是角分
                //}
                //catch (Exception ex)
                //{
                //    _err_msg = string.Format("交易金額欄位錯誤，交易金額={0}", FieldDatas[idx]);
                //    return rc;
                //}
                #endregion

                decimal amount = 0;
                string txt = FieldDatas[idx].Substring(0, 11) + "." + FieldDatas[idx].Substring(11);
                if (Decimal.TryParse(txt, out amount))
                {
                    layout.TxnAmount = amount;
                }
                else
                {
                    _err_msg = string.Format("交易金額欄位錯誤，交易金額={0}", FieldDatas[idx]);
                    return rc;
                }
            }
            #endregion

            idx++;
            layout.ECMark = FieldDatas[idx];
            if (layout.ECMark != D00I70ECMarkCodeTexts.NORMAL_CODE && layout.ECMark != D00I70ECMarkCodeTexts.RECTIFY_CODE)
            {
                _err_msg = string.Format("更正記號欄位錯誤，更正記號={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.TxnWay = FieldDatas[idx];

            #region 交易金額
            {
                idx++;

                #region [Old]
                //layout.AccountDate = FieldDatas[idx];
                #endregion

                DateTime date;
                layout.AccountDate = FieldDatas[idx].Trim();
                if (!Common.TryConvertTWDate8(layout.AccountDate, out date))
                {
                    _err_msg = string.Format("作帳日期欄位錯誤，作帳日期={0}", FieldDatas[idx]);
                    return rc;
                }
            }
            #endregion

            idx++;
            layout.TxnBank = FieldDatas[idx];

            idx++;
            layout.filler = FieldDatas[idx];

            layout.Data = data;
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseLayout7Footer(string data, out Layout07.Footer layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout07.Footer();

            //不會有中文字
            #region 檢查資料長度
            if (String.IsNullOrWhiteSpace(data) || data.Length != Layout07.RecordLens)
            {
                int lens = data == null ? 0 : data.Length;
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            //data = data.Trim();

            int[] FieldLens = Layout07.Footer.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            try
            {
                layout.TotalCount = decimal.Parse(FieldDatas[idx]);
            }
            catch (Exception)
            {
                _err_msg = string.Format("總筆數欄位資料錯誤，總筆數欄位={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            try
            {
                layout.TotalAmount = decimal.Parse(FieldDatas[idx]) / 100;
            }
            catch (Exception)
            {
                _err_msg = string.Format("總金額欄位資料錯誤，總金額欄位={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.Filler = FieldDatas[idx];
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseFileFormat07(string full_file_name, out Layout07.Detail[] datas)
        {
            bool rc = false;
            _err_msg = "";
            datas = null;

            #region 判斷檔案在不在
            System.IO.FileInfo fi = new System.IO.FileInfo(full_file_name);
            if (!fi.Exists)
            {
                _err_msg = string.Format("檔案不存在，檔名={0}", full_file_name);
                return rc;
            }
            #endregion

            #region 拆解檔案
            System.Text.StringBuilder logs = new StringBuilder();
            Int32 count = 0;
            decimal total = 0;
            Int32 countSuccess = 0;
            Int32 countFail = 0;
            Layout07.Header header = new Layout07.Header();
            List<Layout07.Detail> buffs = new List<Layout07.Detail>();
            Layout07.Footer footer = new Layout07.Footer();
            Layout07.Detail layout = new Layout07.Detail();
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(full_file_name, System.Text.Encoding.Default))
                {
                    string line = "";
                    Int32 LineNo = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        LineNo++;
                        #region 逐筆處理
                        try
                        {
                            string type = line.Substring(0, 1);
                            if (type == "1")
                            {
                                #region 首筆
                                if (!ParseLayout7Header(line, out header))
                                {
                                    header = null;
                                    logs.AppendLine(string.Format("拆解首筆失敗，錯誤訊息={0}", _err_msg));
                                    _err_msg = logs.ToString();
                                    return rc;
                                }
                                #endregion
                            }
                            else if (type == "2")
                            {
                                count++;
                                #region 資料
                                if (!ParseLayout7Detail(line, out layout))
                                {
                                    countFail++;
                                    logs.AppendLine(string.Format("第{0}筆拆解失敗，錯誤訊息={1}", count, _err_msg));
                                }
                                else
                                {
                                    countSuccess++;
                                    total += layout.TxnAmount;
                                    buffs.Add(layout);
                                }
                                #endregion
                            }
                            else if (type == "3")
                            {
                                #region 尾筆
                                if (!ParseLayout7Footer(line, out footer))
                                {
                                    footer = null;
                                    logs.AppendLine(string.Format("拆解尾筆失敗，錯誤訊息={0}", _err_msg));
                                    _err_msg = logs.ToString();
                                    return rc;
                                }
                                #endregion
                            }
                            else
                            {
                                #region 檔案錯誤
                                _err_msg = string.Format("檔案出現非首、尾筆或是資料列，錯誤資料={0}", line);
                                datas = null;
                                return rc;
                                #endregion
                            }
                        }
                        catch (Exception ex1)
                        {
                            countFail++;
                            logs.AppendLine(string.Format("第{0}列，拆解發生錯誤，錯誤訊息={1}", LineNo, ex1.Message));
                        }
                        #endregion
                    }
                }

                #region 檢查是否有首、尾筆
                if (header == null)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("檔案沒有首筆")).ToString();
                    return rc;
                }
                if (footer == null)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("檔案沒有尾筆")).ToString();
                    return rc;
                }
                #endregion

                #region 檢察總筆數、總金額
                if (count != footer.TotalCount)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("總筆數不合，檔案筆數={0}，資料筆數={1}", footer.TotalCount, count)).ToString();
                    return rc;
                }
                if (total != footer.TotalAmount)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("總金額不合，檔案金額={0}，資料金額={1}", footer.TotalAmount, total)).ToString();
                    return rc;
                }
                #endregion

                datas = buffs.ToArray<Layout07.Detail>();
                rc = true;
            }
            catch (Exception ex)
            {
                logs.AppendLine(string.Format("拆解檔案發生錯誤，錯誤訊息={0}", ex.Message));
                _err_msg = logs.ToString();
            }
            #endregion

            return rc;
        }

        public bool Layout7DetailToCancelDebts(string full_file_name, Layout07.Detail[] datas, out CancelDebtsEntity[] CancelDebts)
        {
            bool rc = false;
            _err_msg = "";
            CancelDebts = null;
            List<CancelDebtsEntity> buffs = new List<CancelDebtsEntity>();
            DateTime now = DateTime.Now;

            if (datas != null && datas.Length > 0)
            {
                string fileName = "UPFILE:" + Path.GetFileName(full_file_name);
                foreach (Layout07.Detail layout in datas)
                {
                    #region 組cancel_debts
                    CancelDebtsEntity cancel_debts = new CancelDebtsEntity();
                    cancel_debts.ReceiveType = layout.ReceiveType;
                    cancel_debts.CancelNo = layout.ReceiveType + layout.CustomerNo;
                    cancel_debts.AccountDate = !String.IsNullOrWhiteSpace(layout.AccountDate) && layout.AccountDate.Length == 8 ? layout.AccountDate.Substring(1) : layout.AccountDate;
                    cancel_debts.ReceiveTime = layout.TxnTime6;
                    cancel_debts.ReceiveBank = layout.TxnBank;
                    cancel_debts.PayDueDate = "";
                    cancel_debts.Reserve1 = "";
                    cancel_debts.Remark = "";
                    cancel_debts.ReceiveDate = !String.IsNullOrWhiteSpace(layout.TxnDate8) && layout.TxnDate8.Length == 8 ? layout.TxnDate8.Substring(1) : layout.TxnDate8;
                    cancel_debts.ReceiveWay = layout.TxnWay;
                    cancel_debts.ReceiveAmount = layout.TxnAmount;

                    #region [MDY:20160607] 紀錄更正記號
                    cancel_debts.Reserve2 = layout.ECMark;
                    #endregion

                    cancel_debts.FileName = fileName;
                    cancel_debts.ModifyDate = now;
                    cancel_debts.RollbackDate = null;
                    cancel_debts.CancelDate = null;
                    cancel_debts.Status = CancelDebtsStatusCodeTexts.IS_WAITING_CODE;
                    cancel_debts.SourceData = layout.Data;
                    cancel_debts.SourceSeq = Int32.Parse(layout.TxnNo);
                    buffs.Add(cancel_debts);
                    #endregion
                }

                CancelDebts = buffs.ToArray<CancelDebtsEntity>();
            }

            rc = true;
            return rc;
        }
        #endregion

        #region layout8
        public bool ParseLayout8Header(string data, out Layout08.Header layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout08.Header();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout08.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout08.Header.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            layout.qCode = FieldDatas[idx];

            idx++;
            layout.qStartDate = FieldDatas[idx];

            idx++;
            layout.qEndDate = FieldDatas[idx];

            idx++;
            layout.qStartNo = FieldDatas[idx];

            idx++;
            layout.qEndNo = FieldDatas[idx];

            idx++;
            layout.Filler1 = FieldDatas[idx];

            idx++;
            layout.Filler2 = FieldDatas[idx];
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseLayout8Detail(string data, out Layout08.Detail layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout08.Detail();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout08.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout08.Detail.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            layout.TxnNo = FieldDatas[idx];

            idx++;
            layout.ReceiveType = FieldDatas[idx];

            idx++;
            layout.CustomerNo = FieldDatas[idx];

            idx++;
            layout.TxnDate6 = FieldDatas[idx];

            idx++;
            layout.TxnTime6 = FieldDatas[idx];

            idx++;
            try
            {
                layout.TxnAmount = decimal.Parse(FieldDatas[idx].Substring(0, 11));//末兩碼是角分
            }
            catch (Exception)
            {
                _err_msg = string.Format("交易金額欄位錯誤，交易金額={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.ECMark = FieldDatas[idx];
            if (layout.ECMark != D00I70ECMarkCodeTexts.NORMAL_CODE && layout.ECMark != D00I70ECMarkCodeTexts.RECTIFY_CODE)
            {
                _err_msg = string.Format("更正記號欄位錯誤，更正記號={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.TxnWay = FieldDatas[idx];

            idx++;
            layout.AccountDate = FieldDatas[idx];

            idx++;
            layout.TxnBank = FieldDatas[idx];

            idx++;
            layout.filler = FieldDatas[idx];

            idx++;
            layout.Remark = FieldDatas[idx];

            layout.Data = data;
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseLayout8Footer(string data, out Layout08.Footer layout)
        {
            bool rc = false;
            _err_msg = "";

            layout = new Layout08.Footer();

            //不會有中文字
            #region 檢查資料長度
            if (data == null || data.Trim() == "" || data.Trim().Length != Layout08.RecordLens)
            {
                int lens = 0;
                if (data != null && data.Trim() != "") { lens = data.Trim().Length; }
                _err_msg = string.Format("資料長度錯誤，資料長度={0}", lens);
                return rc;
            }
            #endregion
            data = data.Trim();

            int[] FieldLens = Layout08.Footer.FiledsLen;
            string[] FieldDatas = new string[FieldLens.Length];
            #region 拆解資料
            int idx = 0;
            for (int i = 0; i < FieldLens.Length; i++)
            {
                FieldDatas[i] = data.Substring(idx, FieldLens[i]).Trim();
                idx += FieldLens[i];
            }
            #endregion

            #region 每個欄位處理
            idx = 0;
            layout.Type = FieldDatas[idx];

            idx++;
            try
            {
                layout.TotalCount = decimal.Parse(FieldDatas[idx]);
            }
            catch (Exception)
            {
                _err_msg = string.Format("總筆數欄位資料錯誤，總筆數欄位={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            try
            {
                layout.TotalAmount = decimal.Parse(FieldDatas[idx]) / 100;
            }
            catch (Exception)
            {
                _err_msg = string.Format("總金額欄位資料錯誤，總金額欄位={0}", FieldDatas[idx]);
                return rc;
            }

            idx++;
            layout.Filler1 = FieldDatas[idx];

            idx++;
            layout.Filler2 = FieldDatas[idx];
            #endregion

            rc = true;
            return rc;
        }

        public bool ParseFileFormat08(string full_file_name, out Layout08.Detail[] datas)
        {
            bool rc = false;
            _err_msg = "";
            datas = null;

            #region 判斷檔案在不在
            System.IO.FileInfo fi = new System.IO.FileInfo(full_file_name);
            if (!fi.Exists)
            {
                _err_msg = string.Format("檔案不存在，檔名={0}", full_file_name);
                return rc;
            }
            #endregion

            #region 拆解檔案
            System.Text.StringBuilder logs = new StringBuilder();
            Int32 count = 0;
            decimal total = 0;
            Int32 countSuccess = 0;
            Int32 countFail = 0;
            Layout08.Header header = new Layout08.Header();
            List<Layout08.Detail> buffs = new List<Layout08.Detail>();
            Layout08.Footer footer = new Layout08.Footer();
            Layout08.Detail layout = new Layout08.Detail();
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(full_file_name, System.Text.Encoding.Default))
                {
                    string line = "";
                    Int32 LineNo = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        LineNo++;
                        #region 逐筆處理
                        try
                        {
                            string type = line.Substring(0, 1);
                            if (type == "1")
                            {
                                #region 首筆
                                if (!ParseLayout8Header(line, out header))
                                {
                                    header = null;
                                    logs.AppendLine(string.Format("拆解首筆失敗，錯誤訊息={0}", _err_msg));
                                    _err_msg = logs.ToString();
                                    return rc;
                                }
                                #endregion
                            }
                            else if (type == "2")
                            {
                                count++;
                                #region 資料
                                if (!ParseLayout8Detail(line, out layout))
                                {
                                    countFail++;
                                    logs.AppendLine(string.Format("第{0}筆拆解失敗，錯誤訊息={1}", count, _err_msg));
                                }
                                else
                                {
                                    countSuccess++;
                                    total += layout.TxnAmount;
                                    buffs.Add(layout);
                                }
                                #endregion
                            }
                            else if (type == "3")
                            {
                                #region 尾筆
                                if (!ParseLayout8Footer(line, out footer))
                                {
                                    footer = null;
                                    logs.AppendLine(string.Format("拆解尾筆失敗，錯誤訊息={0}", _err_msg));
                                    _err_msg = logs.ToString();
                                    return rc;
                                }
                                #endregion
                            }
                            else
                            {
                                #region 檔案錯誤
                                _err_msg = string.Format("檔案出現非首、尾筆或是資料列，錯誤資料={0}", line);
                                datas = null;
                                return rc;
                                #endregion
                            }
                        }
                        catch (Exception ex1)
                        {
                            countFail++;
                            logs.AppendLine(string.Format("第{0}列，拆解發生錯誤，錯誤訊息={1}", LineNo, ex1.Message));
                        }
                        #endregion
                    }
                }

                #region 檢查是否有首、尾筆
                if (header == null)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("檔案沒有首筆")).ToString();
                    return rc;
                }
                if (footer == null)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("檔案沒有尾筆")).ToString();
                    return rc;
                }
                #endregion

                #region 檢察總筆數、總金額
                if (count != footer.TotalCount)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("總筆數不合，檔案筆數={0}，資料筆數={1}", footer.TotalCount, count)).ToString();
                    return rc;
                }
                if (total != footer.TotalAmount)
                {
                    datas = null;
                    _err_msg = logs.AppendLine(string.Format("總金額不合，檔案金額={0}，資料金額={1}", footer.TotalAmount, total)).ToString();
                    return rc;
                }
                #endregion

                datas = buffs.ToArray<Layout08.Detail>();
                rc = true;
            }
            catch (Exception ex)
            {
                logs.AppendLine(string.Format("拆解檔案發生錯誤，錯誤訊息={1}", count, ex.Message));
                _err_msg = logs.ToString();
            }
            #endregion

            return rc;
        }

        public bool Layout8DetailToCancelDebts(string full_file_name, Layout08.Detail[] datas, out CancelDebtsEntity[] CancelDebts)
        {
            bool rc = false;
            _err_msg = "";
            CancelDebts = null;
            List<CancelDebtsEntity> buffs = new List<CancelDebtsEntity>();
            DateTime now = DateTime.Now;

            if (datas != null && datas.Length > 0)
            {
                string fileName = "UPFILE:" + Path.GetFileName(full_file_name);
                foreach (Layout08.Detail layout in datas)
                {
                    #region 組cancel_debts
                    CancelDebtsEntity cancel_debts = new CancelDebtsEntity();
                    cancel_debts.ReceiveType = layout.ReceiveType;
                    cancel_debts.CancelNo = layout.ReceiveType + layout.CustomerNo;
                    cancel_debts.AccountDate = "1" + layout.AccountDate;
                    cancel_debts.ReceiveTime = layout.TxnTime6;
                    cancel_debts.ReceiveBank = layout.TxnBank;
                    cancel_debts.PayDueDate = "";
                    cancel_debts.Reserve1 = "";
                    cancel_debts.Remark = "";
                    cancel_debts.ReceiveDate = "1" + layout.TxnDate6;
                    cancel_debts.ReceiveWay = layout.TxnWay;
                    cancel_debts.ReceiveAmount = layout.TxnAmount;

                    #region [MDY:20160607] 紀錄更正記號
                    cancel_debts.Reserve2 = layout.ECMark;
                    #endregion

                    cancel_debts.FileName = fileName;
                    cancel_debts.ModifyDate = now;
                    cancel_debts.RollbackDate = null;
                    cancel_debts.CancelDate = null;
                    cancel_debts.Status = CancelDebtsStatusCodeTexts.IS_WAITING_CODE;
                    cancel_debts.SourceData = layout.Data;
                    cancel_debts.SourceSeq = Int32.Parse(layout.TxnNo);
                    buffs.Add(cancel_debts);
                    #endregion
                }

                CancelDebts = buffs.ToArray<CancelDebtsEntity>();
            }

            rc = true;
            return rc;
        }
        #endregion

        public bool InsertCancelDebts(CancelDebtsEntity[] CancelDebtss)
        {
            bool rc = false;
            _err_msg = "";

            StringBuilder logs = new StringBuilder();
            using (EntityFactory factory = new EntityFactory())
            {
                Result result = null;
                Expression where = null;
                foreach (CancelDebtsEntity cancel_debts in CancelDebtss)
                {
                    #region 檢查有沒有重複
                    bool isOK = false;
                    {
                        int count = 0;
                        where = new Expression(CancelDebtsEntity.Field.ReceiveType, cancel_debts.ReceiveType)
                                .And(CancelDebtsEntity.Field.ReceiveWay, cancel_debts.ReceiveWay)
                                .And(CancelDebtsEntity.Field.ReceiveDate, cancel_debts.ReceiveDate)
                                .And(CancelDebtsEntity.Field.ReceiveTime, cancel_debts.ReceiveTime)
                                .And(CancelDebtsEntity.Field.AccountDate, cancel_debts.AccountDate)
                                .And(CancelDebtsEntity.Field.ReceiveBank, cancel_debts.ReceiveBank)
                                .And(CancelDebtsEntity.Field.CancelNo, cancel_debts.CancelNo)
                                .And(CancelDebtsEntity.Field.ReceiveAmount, cancel_debts.ReceiveAmount);
                        result = factory.SelectCount<CancelDebtsEntity>(where, out count);
                        if (result.IsSuccess)
                        {
                            if (count > 0)
                            {
                                logs.AppendFormat("{0} 入帳資料已存在", cancel_debts.CancelNo).AppendLine();
                            }
                            else
                            {
                                isOK = true;
                            }
                        }
                        else
                        {
                            logs.AppendFormat("查詢 {0} 入帳資料失敗，錯誤訊息：{1}", cancel_debts.CancelNo, result.Message).AppendLine();
                        }
                    }
                    #endregion

                    if (isOK)
                    {
                        int count = 0;
                        result = factory.Insert(cancel_debts, out count);
                        if (result.IsSuccess)
                        {
                            if (count == 0)
                            {
                                logs.AppendFormat("新增 {0} 入帳資料失敗，錯誤訊息：無資料被新增", cancel_debts.CancelNo).AppendLine();
                            }
                        }
                        else
                        {
                            logs.AppendFormat("新增 {0} 入帳資料失敗，錯誤訊息：{1}", cancel_debts.CancelNo, result.Message).AppendLine();
                        }
                    }
                }
            }
            rc = logs.Length == 0;
            _err_msg = logs.ToString();
            return rc;
        }

        public bool ImportD00I70File(string bank_id3, string file_name, out int dataCount, out string msg)
        {
            dataCount = 0;
            bool rc = false;
            msg = "";

            Layout07.Detail[] datas=null;
            CancelDebtsEntity[] CancelDebtss = null;
            if(!ParseFileFormat07(file_name,out datas))
            {
                msg = _err_msg;
            }
            else
            {
                if (!Layout7DetailToCancelDebts(file_name, datas, out CancelDebtss))
                {
                    msg = _err_msg;
                }
                else
                {
                    dataCount = CancelDebtss.Length;
                    if(InsertCancelDebts(CancelDebtss))
                    {
                        rc = true;
                    }
                    else
                    {
                        msg = _err_msg;
                        rc = false;
                    }
                }
            }

            return rc;
        }

        public bool ImportD00I70File(string bank_id3, byte[] fileContent, out int dataCount, out string msg)
        {
            dataCount = 0;
            bool rc = false;
            msg = "";
            string file_name = Path.GetTempFileName();
            try
            {
                File.WriteAllBytes(file_name, fileContent);
                rc = ImportD00I70File(bank_id3, file_name, out dataCount, out msg);
            }
            catch (Exception ex)
            {
                msg = string.Format("{0} {1}", ex.Source, ex.Message);
            }
            finally
            {
                #region [MDY:20220716] Checkmarx 調整 (Leaving Temporary Files)
                try
                {
                    File.Delete(file_name);
                }
                catch
                {
                }
                #endregion
            }

            return rc;
        }
    }
}
