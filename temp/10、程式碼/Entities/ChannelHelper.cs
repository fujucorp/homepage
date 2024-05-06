using System;
using System.Collections.Generic;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    #region EDP檔案物件
    public class EDPHeaderRecord
    {
        public string record_type = ""; //錄別 X(1)，固定為1
        public string channel_id = ""; //超商代號。中國信託：‘CNTRUST’,統一：‘7111111’,全家：‘TFM’,OK：‘OKCVS’,萊爾富：‘HILIFE’,財金信用卡：‘CREDIT’
        public string tranfer_date = "";//傳輸日期。民國年(4碼)
        public string sm_account_date = "";//超商入帳日。寫OB070A首錄
        public Int32[] fields_len = { 1, 8, 8, 8 };
    }

    public class EDPDetailRecord
    {
        public string record_type = ""; //錄別 X(1)，固定為2
        public string store_id = "";//代收門市店號(郵局支局號)。空白
        public string cancel_no = "";//虛擬帳號
        public string receive_date = "";//客戶繳費日期。民國年(4碼)
        public string receive_amount = "";//繳費金額
        public string sm_account_date = "";//超商入帳日。會與實際入帳日晚一個營業日。民國年(4碼)
        public string DSOURCE = "";//出帳來源
        public string receive_time = "";//客戶繳費時間。HHMMSS
        public string receive_way = ""; //管道代碼
        public string row_data = "";//原始資料
        public Int32[] fields_len = { 1, 8, 14, 8, 9, 8, 2, 6 };
    }

    public class EDPTailRecord
    {
        public string record_type = ""; //錄別 X(1)，固定為3
        public string total_amount = "";//代收成功金額
        public string total_records = "";//代收成功筆數
        public Int32[] fields_len = { 1, 14, 10 };
    }
    #endregion

    #region EDPHelper
    public class EDPHelper
    {
        public EDPHelper()
        {

        }

        /// <summary>
        /// 判斷是否為首錄
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool isHeaderRecord(string data)
        {
            bool rc = false;
            if (data.Substring(0, 1) == "1")
            {
                rc = true;
            }
            return rc;
        }

        /// <summary>
        /// 判斷是否為明細
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool isDetailRecord(string data)
        {
            bool rc = false;
            if (data.Substring(0, 1) == "2")
            {
                rc = true;
            }
            return rc;
        }

        /// <summary>
        /// 判斷是否為尾錄
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool isTailRecord(string data)
        {
            bool rc = false;
            if (data.Substring(0, 1) == "3")
            {
                rc = true;
            }
            return rc;
        }


        public static bool parseHeader(string data, out EDPHeaderRecord record, out string msg)
        {
            bool rc = false;
            msg = "";
            record = new EDPHeaderRecord();

            Int32[] fields_len = record.fields_len;
            int pos = 0;
            int idx = 0;

            try
            {
                #region 欄位處理
                pos = 0;
                idx = 0;
                record.record_type = data.Substring(idx, fields_len[pos]);
                if (record.record_type != "1")
                {
                    msg = string.Format("[parseHeader] 此筆資料非首錄，data={0}", data);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.channel_id = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.tranfer_date = data.Substring(idx, fields_len[pos]).Trim();
                record.tranfer_date = record.tranfer_date.Substring(1, 7);

                idx += fields_len[pos];
                pos++;
                record.sm_account_date = data.Substring(idx, fields_len[pos]).Trim();
                record.sm_account_date = record.sm_account_date.Substring(1, 7);
                #endregion
                rc = true;
            }
            catch (Exception ex)
            {
                msg = string.Format("[parseHeader] 此筆資料有誤，data={0}，錯誤訊息={1}", data, ex.Message);
                record = null;
                return rc;
            }

            return rc;
        }

        public static bool parseDetail(string data, out EDPDetailRecord record, out string msg)
        {
            bool rc = false;
            record = new EDPDetailRecord();
            msg = "";

            Int32[] fields_len = record.fields_len;
            int pos = 0;
            int idx = 0;

            try
            {
                #region 欄位處理
                pos = 0;
                idx = 0;
                record.record_type = data.Substring(idx, fields_len[pos]).Trim();
                if (record.record_type != "2")
                {
                    msg = string.Format("[parseDetail] 此筆資料非明細，data={0}", data);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.store_id = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.cancel_no = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.receive_date = data.Substring(idx, fields_len[pos]).Trim();
                record.receive_date = record.receive_date.Substring(1, 7);

                idx += fields_len[pos];
                pos++;
                record.receive_amount = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.sm_account_date = data.Substring(idx, fields_len[pos]).Trim();
                record.sm_account_date = record.sm_account_date.Substring(1, 7);

                idx += fields_len[pos];
                pos++;
                record.DSOURCE = data.Substring(idx, fields_len[pos]).Trim();
                record.receive_way = record.DSOURCE;

                idx += fields_len[pos];
                pos++;
                record.receive_time = data.Substring(idx, fields_len[pos]).Trim();

                #endregion

                rc = true;
            }
            catch (Exception ex)
            {
                msg = string.Format("[parseDetail] 此筆資料有誤，data={0}，錯誤訊息={1}", data, ex.Message);
                record = null;
                return rc;
            }
            return rc;
        }

        public static bool parseTail(string data, out EDPTailRecord record, out string msg)
        {
            bool rc = false;
            record = new EDPTailRecord();
            msg = "";

            Int32[] fields_len = record.fields_len;
            int pos = 0;
            int idx = 0;

            try
            {
                #region 欄位處理
                pos = 0;
                idx = 0;
                record.record_type = data.Substring(idx, fields_len[pos]).Trim();
                if (record.record_type != "3")
                {
                    msg = string.Format("[parseTail] 此筆資料非尾錄，data={0}", data);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.total_amount = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.total_records = data.Substring(idx, fields_len[pos]).Trim();
                #endregion

                rc = true;
            }
            catch (Exception ex)
            {
                msg = string.Format("[parseTail] 此筆資料有誤，data={0}，錯誤訊息={1}", data, ex.Message);
                record = null;
                return rc;
            }

            return rc;
        }


        public static EDPHeaderRecord SMHeader2EDPHeader(SMFileHeaderRecord sm_header)
        {
            EDPHeaderRecord edp_header = new EDPHeaderRecord();
            edp_header.record_type = "1";
            edp_header.channel_id = sm_header.corp_code;
            edp_header.tranfer_date = sm_header.send_date;

            return edp_header;
        }
    }
    #endregion

    #region 超商代收款檔物件
    /// <summary>
    /// 超商代收款檔首錄
    /// </summary>
    public class SMFileHeaderRecord
    {
        //總長度120bytes
        public string record_type = ""; //錄別 X(1)，固定為1
        public string corp_code = "";//公司代號 X(8) 左靠右補空白
        public string sm_id = "";//代收機構代號 X(8) 統一：‘7111111’OK：‘OKCVS’萊爾富：‘HILIFE’ 左靠右補空白
        public string transfer_type = "";//轉帳類別 9(3) 固定值‘000’
        public string transfer_qual = "";//轉帳性質別 X(1) 2’扣款回送
        public string send_date = "";//傳輸日期 9(8) yyyymmdd
        public string reserve = "";//保留欄位 X(91) 空白
        public Int32[] fields_len = { 1, 8, 8, 3, 1, 8, 91 };
    }

    /// <summary>
    /// 超商代收款檔明細
    /// </summary>
    public class SMFileDetailRecord
    {
        //總長度120bytes
        public string record_type = ""; //錄別 X(1)，固定為2
        public string corp_code = "";//公司代號 X(8) 左靠右補空白
        public string sm_id = "";//代收機構代號 X(8) 統一：‘7111111’OK：‘OKCVS’萊爾富：‘HILIFE’ 左靠右補空白
        public string sm_store_id = "";//代收門市店號 X(8) 左靠右補空白
        public string tranfer_pay_account = "";//轉帳代繳帳號 9(14) 補‘0’
        public string transfer_type = "";//轉帳類別 9(3) 固定值‘000’
        public string debits_status = "";//扣繳狀況 9(2)	‘00’扣款成功
        public string send_date = "";//傳輸日期 9(8) yyyymmdd
        public string receive_date = "";//消費者繳費日 9(8) yyyymmdd
        public string barcode1 = "";//barcode1 X(9)	繳費期限yymmdd + 代收項目(3)
        public string barcode2 = "";//barcode2 X(20)	交易序號(16) 不足時，左靠右補空白
        public string barcode3 = "";//barcode3 X(15) 列帳日期mmdd + 檢碼(2) + 應繳金額(9)
        public string reserve = "";//保留欄位 X(16) 空白
        public string pay_due_date = "";//繳款期限 yymmdd，yy民國年
        public string sm_code = "";//超商代碼(代收項目)
        public string cancel_no = "";//銷帳編號
        public string amount = "";//應繳金額
        public Int32[] fields_len = { 1, 8, 8, 8, 14, 3, 2, 8, 8, 9, 20, 15, 16 };
    }

    /// <summary>
    /// 超商代收款檔尾錄
    /// </summary>
    public class SMFileTailRecord
    {
        //總長度120bytes
        public string record_type = ""; //錄別 X(1)，固定為3
        public string corp_code = "";//公司代號 X(8) 左靠右補空白
        public string sm_id = "";//代收機構代號 X(8) 統一：‘7111111’OK：‘OKCVS’萊爾富：‘HILIFE’ 左靠右補空白
        public string transfer_type = "";//轉帳類別 9(3) 固定值‘000’
        public string send_date = "";//傳輸日期 9(8) yyyymmdd
        public string total_amount = "";//代收成功總金額	9(14)V99	整數位不足時，右靠左補0
        public string total_records = "";//代收成功總筆數	9(10)	整數位不足時，右靠左補0
        public string reserve = "";//保留欄位	X(66)	空白
        public Int32[] fields_len = { 1, 8, 8, 3, 8, 16, 10, 66 };
    }
    #endregion

    public class SMFileHelper
    {
        /// <summary>
        /// 判斷是否為首錄
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool isHeaderRecord(string data)
        {
            bool rc = false;
            if(data.Substring(0,1)=="1")
            {
                rc = true;
            }
            return rc;
        }

        /// <summary>
        /// 判斷是否為明細
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool isDetailRecord(string data)
        {
            bool rc = false;
            if (data.Substring(0, 1) == "2")
            {
                rc = true;
            }
            return rc;
        }

        /// <summary>
        /// 判斷是否為尾錄
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool isTailRecord(string data)
        {
            bool rc = false;
            if (data.Substring(0, 1) == "3")
            {
                rc = true;
            }
            return rc;
        }


        public static bool parseHeader(string data, out SMFileHeaderRecord record, out string msg)
        {
            bool rc = false;
            msg = "";
            record = new SMFileHeaderRecord();

            Int32[] fields_len = record.fields_len;
            int pos = 0;
            int idx = 0;

            try
            {
                #region 欄位處理
                pos = 0;
                idx=0;
                record.record_type = data.Substring(idx, fields_len[pos]);
                if (record.record_type != "1")
                {
                    msg = string.Format("[parseHeader] 此筆資料非首錄，data={0}", data);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.corp_code = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.sm_id = data.Substring(idx, fields_len[pos]).Trim().ToUpper();
                if (record.sm_id != "7111111" && record.sm_id != "OKCVS" && record.sm_id != "HILIFE" && record.sm_id != "FAMILY")
                {
                    //msg = string.Format("[parseHeader] 此筆資料代收機構代碼有誤，data={0}", data);
                    //record = null;
                    //return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.transfer_type = data.Substring(idx, fields_len[pos]).Trim();
                if (record.transfer_type != "000")
                {
                    //msg = string.Format("[parseHeader] 此筆資料轉帳類別有誤，data={0}", data);
                    //record = null;
                    //return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.transfer_qual = data.Substring(idx, fields_len[pos]).Trim();
                if (record.transfer_qual != "2")
                {
                    //msg = string.Format("[parseHeader] 此筆資料轉帳性質別有誤，data={0}", data);
                    //record = null;
                    //return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.send_date = data.Substring(idx, fields_len[pos]).Trim();
                if (record.send_date.Length != 8)
                {
                    msg = string.Format("[parseHeader] 此筆資料傳送日期有誤，data={0}", data);
                    record = null;
                    return rc;
                }
                try
                {
                    DateTime dt = DateTime.Parse(record.send_date);
                }
                catch (Exception ex1)
                {
                    msg = string.Format("[parseHeader] 此筆資料傳送日期有誤，data={0}，錯誤訊息={1}", data,ex1.Message);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.reserve = data.Substring(idx, fields_len[pos]).Trim();
                #endregion
                rc = true;
            }
            catch(Exception ex)
            {
                msg = string.Format("[parseHeader] 此筆資料有誤，data={0}，錯誤訊息={1}", data,ex.Message);
                record = null;
                return rc;
            }
            
            return rc;
        }

        public static bool parseDetail(string data,out SMFileDetailRecord record,out string msg)
        {
            bool rc = false;
            record = new SMFileDetailRecord();
            msg = "";

            Int32[] fields_len = record.fields_len;
            int pos = 0;
            int idx = 0;

            try
            {
                #region 欄位處理
                pos = 0;
                idx = 0;
                record.record_type = data.Substring(idx, fields_len[pos]).Trim();
                if (record.record_type != "2")
                {
                    msg = string.Format("[parseDetail] 此筆資料非明細，data={0}", data);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.corp_code = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.sm_id = data.Substring(idx, fields_len[pos]).Trim();
                if (record.sm_id != "7111111" && record.sm_id != "OKCVS" && record.sm_id != "HILIFE" && record.sm_id != "FAMILY")
                {
                    //msg = string.Format("[parseDetail] 此筆資料代收機構代碼有誤，data={0}", data);
                    //record = null;
                    //return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.sm_store_id = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.tranfer_pay_account = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.transfer_type = data.Substring(idx, fields_len[pos]).Trim();
                if (record.transfer_type != "000")
                {

                }

                idx += fields_len[pos];
                pos++;
                record.debits_status = data.Substring(idx, fields_len[pos]).Trim();
                if (record.debits_status != "00")
                {

                }

                idx += fields_len[pos];
                pos++;
                record.send_date = data.Substring(idx, fields_len[pos]).Trim();
                if (record.send_date.Length != 8)
                {
                    msg = string.Format("[parseDetail] 此筆資料傳送日期有誤，data={0}", data);
                    record = null;
                    return rc;
                }
                try
                {
                    DateTime dt = DateTime.Parse(record.send_date);
                }
                catch (Exception ex1)
                {
                    msg = string.Format("[parseDetail] 此筆資料傳送日期有誤，data={0}，錯誤訊息={1}", data, ex1.Message);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.receive_date = data.Substring(idx, fields_len[pos]);
                if (record.receive_date.Length != 8)
                {
                    msg = string.Format("[parseDetail] 此筆資料消費者繳費日有誤，data={0}", data);
                    record = null;
                    return rc;
                }
                try
                {
                    DateTime dt = DateTime.Parse(record.receive_date);
                }
                catch (Exception ex1)
                {
                    msg = string.Format("[parseDetail] 此筆資料消費者繳費日有誤，data={0}，錯誤訊息={1}", data, ex1.Message);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.barcode1 = data.Substring(idx, fields_len[pos]).Trim();
                try
                {
                    record.pay_due_date = record.barcode1.Substring(0, 6);
                    record.sm_code = record.barcode1.Substring(6, 3);
                }
                catch(Exception ex1)
                {
                    msg = string.Format("[parseDetail] 此筆資料barcode1有誤，data={0}，錯誤訊息={1}", data, ex1.Message);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.barcode2 = data.Substring(idx, fields_len[pos]).Trim();
                record.cancel_no = record.barcode2;

                idx += fields_len[pos];
                pos++;
                record.barcode3 = data.Substring(idx, fields_len[pos]).Trim();
                try
                {
                    record.amount = record.barcode3.Substring(6, 9);
                }
                catch(Exception ex1)
                {
                    msg = string.Format("[parseDetail] 此筆資料barcode3有誤，data={0}，錯誤訊息={1}", data, ex1.Message);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.reserve = data.Substring(idx, fields_len[pos]).Trim();
                #endregion

                rc = true;
            }
            catch(Exception ex)
            {
                msg = string.Format("[parseDetail] 此筆資料有誤，data={0}，錯誤訊息={1}", data, ex.Message);
                record = null;
                return rc;
            }
            return rc;
        }

        public static bool parseTail(string data, out SMFileTailRecord record, out string msg)
        {
            bool rc = false;
            record = new SMFileTailRecord();
            msg = "";

            Int32[] fields_len = record.fields_len;
            int pos = 0;
            int idx = 0;

            try
            {
                #region 欄位處理
                pos = 0;
                idx = 0;
                record.record_type = data.Substring(idx, fields_len[pos]).Trim();
                if (record.record_type != "3")
                {
                    msg = string.Format("[parseTail] 此筆資料非尾錄，data={0}", data);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.corp_code = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.sm_id = data.Substring(idx, fields_len[pos]).Trim();
                if (record.sm_id != "7111111" && record.sm_id != "OKCVS" && record.sm_id != "HILIFE" && record.sm_id != "FAMILY")
                {
                    //msg = string.Format("[parseTail] 此筆資料代收機構代碼有誤，data={0}", data);
                    //record = null;
                    //return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.transfer_type = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.send_date = data.Substring(idx, fields_len[pos]).Trim();
                if (record.send_date.Length != 8)
                {
                    msg = string.Format("[parseTail] 此筆資料傳送日期有誤，data={0}", data);
                    record = null;
                    return rc;
                }
                try
                {
                    DateTime dt = DateTime.Parse(record.send_date);
                }
                catch (Exception ex1)
                {
                    msg = string.Format("[parseTail] 此筆資料傳送日期有誤，data={0}，錯誤訊息={1}", data, ex1.Message);
                    record = null;
                    return rc;
                }

                idx += fields_len[pos];
                pos++;
                record.total_amount = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.total_records = data.Substring(idx, fields_len[pos]).Trim();

                idx += fields_len[pos];
                pos++;
                record.reserve = data.Substring(idx, fields_len[pos]).Trim();
                #endregion

                rc = true;
            }
            catch(Exception ex)
            {
                msg = string.Format("[parseTail] 此筆資料有誤，data={0}，錯誤訊息={1}", data, ex.Message);
                record = null;
                return rc;
            }

            return rc;
        }


    }


    /// <summary>
    /// 管道 Helper 類別
    /// </summary>
    public sealed class ChannelHelper
    {
        #region Const
        /// <summary>
        /// 臨櫃 : 01
        /// </summary>
        public const string TABS = "01";

        /// <summary>
        /// 匯款 : 02
        /// </summary>
        public const string RM = "02";

        /// <summary>
        /// ATM : 03
        /// </summary>
        public const string ATM = "03";

        /// <summary>
        /// EDI : 04
        /// </summary>
        public const string EDI = "04";

        /// <summary>
        /// 網路銀行 : 05
        /// </summary>
        public const string NB = "05";

        /// <summary>
        /// 語音銀行 : 06
        /// </summary>
        public const string VO = "06";

        #region [MDY:20170506] 增加境外支付 (支付寶)
        /// <summary>
        /// 支付寶 (財金境外支付) : 09
        /// </summary>
        public const string ALIPAY = "09";
        #endregion

        #region [MDY:20171127] 增加08-全國繳費網、10-台灣Pay (20170831_01)
        /// <summary>
        /// 全國繳費網 : 08
        /// </summary>
        public const string EBILL = "08";

        /// <summary>
        /// 台灣Pay (QRCode) : 10
        /// </summary>
        public const string TWPAY = "10";
        #endregion

        #region 超商
        /// <summary>
        /// 超商7-11 : 81
        /// </summary>
        public const string SM_711 = "81";
        /// <summary>
        /// 超商全家 : 82
        /// </summary>
        public const string SM_TFM = "82";
        /// <summary>
        /// 超商OK : 83
        /// </summary>
        public const string SM_OKM = "83";
        /// <summary>
        /// 超商福客多 : 84
        /// </summary>
        public const string SM_NIK = "84";
        /// <summary>
        /// 超商萊爾富 : 85
        /// </summary>
        public const string SM_HILI = "85";

        /// <summary>
        /// 超商(手續費用) : Z
        /// </summary>
        public const string SM_DEFAULT = "Z";
        #endregion

        #region [MDY:20190906] (2019擴充案) 超商行動版 (虛擬的管道代碼，僅行動版產生超商條碼用，計算超商金額不使用此設定，帳務中心的管道不會有這個代碼)
        /// <summary>
        /// 超商行動版(手續費用) : Z2 (虛擬的管道代碼，僅行動版產生超商條碼用，計算超商金額不使用此設定，帳務中心的管道不會有這個代碼)
        /// </summary>
        public const string SM_MOBILE = "Z2";
        #endregion

        #region 信用卡
        /// <summary>
        /// 信用卡(財金) : 87
        /// </summary>
        public const string FISC = "87";

        #region [MDY:20191214] (2019擴充案) 財金國際信用卡 : NC
        /// <summary>
        /// 財金國際信用卡 : NC
        /// </summary>
        public const string FISC_NC = "NC";
        #endregion

        #region [MDY:20170506] 修正中信管道常數為 CTCB
        #region [Old]
        ///// <summary>
        ///// 信用卡(中國信託) : CTCD
        ///// </summary>
        //public const string CTCD = "80";
        #endregion
        /// <summary>
        /// 信用卡(中國信託) : 80
        /// </summary>
        public const string CTCB = "80";
        #endregion
        #endregion

        /// <summary>
        /// 郵局 : 86
        /// </summary>
        public const string PO = "86";

        /// <summary>
        /// 商家自收 : S
        /// </summary>
        public const string SELF = "S";

        /// <summary>
        /// 土銀委扣 : B
        /// </summary>
        public const string LB = "B";
        #endregion

        #region Constructor
        public ChannelHelper()
        {

        }
        #endregion

        #region Method
        /// <summary>
        /// 取得指定 CodeText 集合中是否有超商管道
        /// </summary>
        /// <param name="datas">指定 CodeText 集合</param>
        /// <returns>有則傳回 true，否則傳回 false</returns>
        public bool HasSMChannel(ICollection<CodeText> datas)
        {
            if (datas != null && datas.Count > 0)
            {
                foreach (CodeText data in datas)
                {
                    if (IsSMChannel(data.Code))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 找出指定 CodeText 集合中的超商管道
        /// </summary>
        /// <param name="datas">指定 CodeText 集合</param>
        /// <returns>傳回超商管道的 CodeTextList</returns>
        public CodeTextList FindSMChannels(ICollection<CodeText> datas)
        {
            CodeTextList myDatas = new CodeTextList();
            if (datas != null && datas.Count > 0)
            {
                foreach (CodeText data in datas)
                {
                    if (IsSMChannel(data.Code))
                    {
                        myDatas.Add(data);
                    }
                }
            }
            return myDatas;
        }

        /// <summary>
        /// 產生超商第一段條碼 (9碼：繳費期限 民國yymmdd 6 碼 + 代收項目 = 手序費代碼 3 碼)
        /// </summary>
        /// <param name="payDueDate">指定繳費期限，僅允許 Date8 或 TWDate7 格式的日期字串</param>
        /// <param name="barcodeId">指定手序費代碼</param>
        /// <returns>成功則傳回條碼，否則傳回空字串</returns>
        public string GenSMBarcode1(string payDueDate, string barcodeId)
        {
            if (!String.IsNullOrWhiteSpace(payDueDate))
            {
                DateTime date;
                if (Common.TryConvertTWDate7(payDueDate, out date))
                {
                    return this.GenSMBarcode1(date, barcodeId);
                }
                else if (Common.TryConvertTWDate8(payDueDate, out date))
                {
                    return this.GenSMBarcode1(date, barcodeId);
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 產生超商第一段條碼 (9碼：繳費期限 民國yymmdd 6 碼 + 代收項目 = 手序費代碼 3 碼)
        /// </summary>
        /// <param name="payDueDate">指定繳費期限</param>
        /// <param name="barcodeId">指定手序費代碼</param>
        /// <returns>成功則傳回條碼，否則傳回空字串</returns>
        public string GenSMBarcode1(DateTime payDueDate, string barcodeId)
        {
            if (!String.IsNullOrWhiteSpace(barcodeId) && (barcodeId = barcodeId.Trim()).Length == 3)
            {
                return String.Format("{0}{1:MMdd}{2}", GetYY(payDueDate), payDueDate, barcodeId);
            }
            return String.Empty;
        }

        /// <summary>
        /// 產生超商第二段條碼 (16碼：交易序號業者自行訂定 = 銷帳編號，不足位數者，由位數小者補0 ??)
        /// </summary>
        /// <param name="cancelNo">指定銷帳編號</param>
        /// <returns>成功則傳回條碼，否則傳回空字串</returns>
        public string GenSMBarcode2(string cancelNo)
        {
            if (!String.IsNullOrWhiteSpace(cancelNo) || (cancelNo = cancelNo.Trim()).Length <= 16)
            {
                return cancelNo.PadRight(16, '0');
            }
            return String.Empty;
        }

        #region 只給GenSMBarcode1使用
        /// <summary>
        /// 取得民國年後兩碼
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string GetYY(DateTime date)
        {
            string yy = "";
            string y = Convert.ToString(date.Year - 1911);
            if (y.Length == 3)
            {
                yy = y.Substring(1, 2);
            }
            else if (y.Length == 2)
            {
                yy = y;
            }
            return yy;
        }
        #endregion

        /// <summary>
        /// 產生超商第三段條碼 (15碼：應繳年月(民國年月) yymm 4碼 + 檢碼 2碼 + 應繳金額 9碼)
        /// </summary>
        /// <param name="barcode1">指定超商第一段條碼 (9碼)</param>
        /// <param name="barcode2">指定超商第二段條碼 (16碼)</param>
        /// <param name="payDueDate">應繳日期</param>
        /// <param name="amount">指定應繳金額</param>
        /// <returns>傳回完整的第三段條碼</returns>
        public string GenSMBarcode3(string barcode1, string barcode2, DateTime payDueDate, decimal amount)
        {
            #region 檢碼計算邏輯
            // Barcode 中的英文字依下表轉換成數值後計算
            //   'A'=1;  'B'=2;  'C'=3;  'D'=4;  'E'=5;  'F'=6;  'G'=7;  'H'=:8;  'I'=9;
            //   'J'=1;  'K'=2;  'L'=3;  'M'=4;  'N'=5;  'O'=6;  'P'=7;  'Q'=:8;  'R'=9
            //   'S'=2;  'T'=3;  'U'=4;  'V'=5;  'W'=6;  'X'=7;  'Y'=8;  'Z'=:9
            //   '+'=1;  '%'=2;  '-'=6'  '.'=7;  ' '=8'  '$'=9;  '/'=0
            // 檢碼第一碼計算公式：
            //   各段 Barcode 之「基數位 (以1開始算)」之值的加總值，再取除以 11 後的「餘數」
            //   ，若餘數為 0 則放 A，若餘數為10則放 B，否則放餘數
            // 檢碼第二碼計算公式：
            //   各段 Barcode 之「偶數位 (以1開始算)」之值的加總值，再取除以 11 後的「餘數」
            //   ，若餘數為 0 則放 X，若餘數為10則放 Y，否則放餘數
            #endregion

            if ((String.IsNullOrWhiteSpace(barcode1) || (barcode1 = barcode1.Trim()).Length != 9)
                || (String.IsNullOrWhiteSpace(barcode2) || (barcode2 = barcode2.Trim()).Length != 16)
                || amount < 1)
            {
                return String.Empty;
            }

            int[] sum = new int[2];
            sum[0] = 0;   //奇數位數的和
            sum[1] = 0;   //偶數位數的和

            #region 處理第一段條碼
            {
                char[] chrs = barcode1.ToCharArray();
                for (int idx = 0; idx < chrs.Length; idx++)
                {
                    sum[idx % 2] += this.GetSMCharValue(chrs[idx]);
                }
            }
            #endregion

            #region 處理第二段條碼
            {
                char[] chrs = barcode2.ToCharArray();
                for (int idx = 0; idx < chrs.Length; idx++)
                {
                    sum[idx % 2] += this.GetSMCharValue(chrs[idx]);
                }
            }
            #endregion

            #region 處理第三段條碼 (code31(應繳日期) + chk1(檢碼第一碼) + chk2(檢碼第二碼) + code33(應繳金額))
            {
                string code31 = Common.GetTWDate6(payDueDate).Substring(0,4);
                string code33 = String.Format("{0:000000000}", amount);     //只取整數

                sum[0] += this.GetSMCharValue(code31[0]);
                sum[1] += this.GetSMCharValue(code31[1]);
                sum[0] += this.GetSMCharValue(code31[2]);
                sum[1] += this.GetSMCharValue(code31[3]);

                char[] chrs = code33.ToCharArray();
                for (int idx = 0; idx < chrs.Length; idx++)
                {
                    sum[idx % 2] += this.GetSMCharValue(chrs[idx]);
                }

                sum[0] %= 11;
                sum[1] %= 11;

                //檢碼第一碼 (奇數位數的和除以 11 後的餘數，若為 0 則放 A，若為 1 0則放 B，否則放餘數)
                string chk1 = sum[0] == 0 ? "A" : (sum[0] == 10 ? "B" : sum[0].ToString());

                //檢碼第一碼 (偶數位數的和除以 11 後的餘數，若為 0 則放 X，若為 1 0則放 Y，否則放餘數)
                string chk2 = sum[1] == 0 ? "X" : (sum[1] == 10 ? "Y" : sum[1].ToString());

                return String.Format("{0}{1}{2}{3}", code31, chk1, chk2, code33);
            }
            #endregion

        }

        /// <summary>
        /// 取得超商條碼字元對應的數值
        /// </summary>
        /// <param name="chr">指定字元</param>
        /// <returns>傳回數值</returns>
        private int GetSMCharValue(char chr)
        {
            string chrString = chr.ToString().ToUpper();
            switch (chrString)
            {
                case "A":
                    return 1;
                case "B":
                    return 2;
                case "C":
                    return 3;
                case "D":
                    return 4;
                case "E":
                    return 5;
                case "F":
                    return 6;
                case "G":
                    return 7;
                case "H":
                    return 8;
                case "I":
                    return 9;
                case "J":
                    return 1;
                case "K":
                    return 2;
                case "L":
                    return 3;
                case "M":
                    return 4;
                case "N":
                    return 5;
                case "O":
                    return 6;
                case "P":
                    return 7;
                case "Q":
                    return 8;
                case "R":
                    return 9;
                case "S":
                    return 2;
                case "T":
                    return 3;
                case "U":
                    return 4;
                case "V":
                    return 5;
                case "W":
                    return 6;
                case "X":
                    return 7;
                case "Y":
                    return 8;
                case "Z":
                    return 9;
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    return int.Parse(chrString);
                case "+":
                    return 1;
                case "%":
                    return 2;
                case "-":
                    return 6;
                case ".":
                    return 7;
                case " ":
                    return 8;
                case "$":
                    return 9;
                case "/":
                    return 0;
                default:
                    throw new Exception("不支援的BarCode字元 : " + chrString);
            }
        }

        /// <summary>
        /// 取得指定業務別是否有超商與臨櫃管道設定
        /// </summary>
        /// <param name="receiveType">指定業務別</param>
        /// <param name="hasSMChannel">有超商管道則傳回 true，否則傳回 false</param>
        /// <param name="hasCashChannel">有臨櫃管道則傳回 true，否則傳回 false</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        public string CheckReceiveChannel(string receiveType, out bool hasSMChannel, out bool hasCashChannel)
        {
            hasSMChannel = false;
            hasCashChannel = false;

            Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType)
                .And(ReceiveChannelEntity.Field.ChannelId, new string[] { TABS, SM_DEFAULT });

            Result result = null;
            ReceiveChannelEntity[] datas = null;
            using (EntityFactory factroy = new EntityFactory())
            {
                result = factroy.SelectAll<ReceiveChannelEntity>(where, null, out datas);
            }
            if (!result.IsSuccess)
            {
                return string.Format("取得 {0} 的管道設定失敗，錯誤訊息：{1}", receiveType, result.Message);
            }
            if (datas != null)
            {
                foreach (ReceiveChannelEntity data in datas)
                {
                    switch (data.ChannelId)
                    {
                        case TABS:
                            hasCashChannel = true;
                            break;
                        case SM_DEFAULT:
                            hasSMChannel = true;
                            break;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 取得指定金額在指定管道手續費設定中適用的超商與臨櫃管道手續費設定
        /// </summary>
        /// <param name="amount">指定金額</param>
        /// <param name="receiveChannels">指定管道手續費設定</param>
        /// <param name="smChannel">傳回適用的超商管道手續費設定</param>
        /// <param name="cashChannel">傳回適用的臨櫃管道手續費設定</param>
        public void CheckReceiveChannel(decimal amount, ReceiveChannelEntity[] receiveChannels, out ReceiveChannelEntity smChannel, out ReceiveChannelEntity cashChannel)
        {
            smChannel = null;
            cashChannel = null;
            if (receiveChannels != null && receiveChannels.Length > 0 && amount > 0)
            {
                foreach (ReceiveChannelEntity receiveChannel in receiveChannels)
                {
                    decimal maxAcount = receiveChannel.MaxMoney ?? 0;
                    decimal minAmount = receiveChannel.MinMoney;
                    if (amount >= minAmount && (amount <= maxAcount || maxAcount == 0))
                    {
                        switch (receiveChannel.ChannelId)
                        {
                            case TABS:
                                cashChannel = receiveChannel;
                                break;
                            case SM_DEFAULT:
                                smChannel = receiveChannel;
                                break;
                        }
                    }
                    if (cashChannel != null && smChannel != null)
                    {
                        break;
                    }
                }
            }
        }

        #region [Old] 沒用到先 Mark
        ///// <summary>
        ///// 取得指定業務別的超商與臨櫃管道設定
        ///// </summary>
        ///// <param name="receiveType">指定業務別</param>
        ///// <param name="smChannels">傳回超商管道設定</param>
        ///// <param name="cashChannel">傳回臨櫃管道設定</param>
        ///// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        //public string GetReceiveChannels(string receiveType, out ReceiveChannelEntity[] smChannels, out ReceiveChannelEntity cashChannel)
        //{
        //    Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType)
        //        .And(ReceiveChannelEntity.Field.ChannelId, new string[] { TABS, SM_DEFAULT });
        //    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(2);
        //    orderbys.Add(ReceiveChannelEntity.Field.ChannelId, OrderByEnum.Asc);
        //    orderbys.Add(ReceiveChannelEntity.Field.BarcodeId, OrderByEnum.Asc);

        //    Result result = null;
        //    ReceiveChannelEntity[] datas = null;
        //    using (EntityFactory factroy = new EntityFactory())
        //    {
        //        result = factroy.SelectAll<ReceiveChannelEntity>(where, orderbys, out datas);
        //    }
        //    if (!result.IsSuccess)
        //    {
        //        smChannels = null;
        //        cashChannel = null;
        //        return string.Format("取得 {0} 的管道設定失敗，錯誤訊息：{1}", receiveType, result.Message);
        //    }
        //    if (datas != null)
        //    {
        //        cashChannel = null;
        //        List<ReceiveChannelEntity> list = new List<ReceiveChannelEntity>(datas.Length);
        //        foreach (ReceiveChannelEntity data in datas)
        //        {
        //            switch (data.ChannelId)
        //            {
        //                case TABS:
        //                    cashChannel = data;
        //                    break;
        //                case SM_DEFAULT:
        //                    list.Add(data);
        //                    break;
        //            }
        //        }
        //        smChannels = list.ToArray();
        //    }
        //    else
        //    {
        //        smChannels = new ReceiveChannelEntity[0];
        //        cashChannel = null;
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 取得指定業務別與金額的超商、郵局、臨櫃的應繳金額與超商代碼
        ///// </summary>
        ///// <param name="receive_type">指定業務別</param>
        ///// <param name="amount">指定金額</param>
        ///// <param name="sm_amount">傳回超商應繳金額</param>
        ///// <param name="sm_code">傳回超商手序費代碼</param>
        ///// <param name="po_amount">傳回臨櫃應繳金額</param>
        ///// <param name="cash_amount">傳回臨櫃應繳金額</param>
        ///// <param name="msg">傳回錯誤訊息</param>
        ///// <returns>成功則傳回 treu，否則傳回 false</returns>
        //public bool GetChannelFee(string receive_type,decimal amount,out decimal sm_amount,out string sm_code,out decimal po_amount,out decimal cash_amount,out string msg)
        //{
        //    bool rc = false;
        //    sm_amount = 0;
        //    sm_code = "";
        //    po_amount = 0;
        //    cash_amount = 0;
        //    msg = "";

        //    string key = string.Format("receive_type={0}",receive_type);

        //    Expression where = null;
        //    KeyValueList<OrderByEnum> orderbys = null;
        //    Result result = null;

        //    //get Receive_Channel
        //    ReceiveChannelEntity[] receive_channels = null;
        //    where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receive_type);
        //    orderbys = new KeyValueList<OrderByEnum>(2);
        //    orderbys.Add(ReceiveChannelEntity.Field.ChannelId, OrderByEnum.Asc);
        //    orderbys.Add(ReceiveChannelEntity.Field.BarcodeId, OrderByEnum.Asc);
        //    result = null;
        //    using (EntityFactory factroy = new EntityFactory())
        //    {
        //        result = factroy.Select<ReceiveChannelEntity>(where, orderbys, 0, 0, out receive_channels);
        //    }
        //    if (!result.IsSuccess)
        //    {
        //        msg = string.Format("[GetChannelFee] 取得管道設定發生錯誤，錯誤訊息={0}，key={1}",result.Message,key);
        //        return rc;
        //    }
        //    else
        //    {
        //        if(receive_channels==null || receive_channels.Length<=0)
        //        {
        //            msg = string.Format("[GetChannelFee] 取得管道設定發生錯誤，錯誤訊息={0}，key={1}", "未設定管道資料",key);
        //            return rc;
        //        }
        //        else
        //        {

        //        }
        //    }

        //    ReceiveChannelEntity sm_channel = null;
        //    ReceiveChannelEntity po_channel = null;
        //    ReceiveChannelEntity cash_channel = null;

        //    rc = GetChannelFee(amount, receive_channels, out sm_amount, out sm_channel, out po_amount, out po_channel, out cash_amount, out cash_channel);
        //    if(!rc)
        //    {
        //        msg = string.Format("[GetChannelFee] [GetChannelFee]計算管道金額發生錯誤，錯誤訊息={0}，key={1},amount={2}", "未設定管道資料", key, amount);
        //    }
        //    else
        //    {
        //        if (sm_channel != null)
        //        {
        //            sm_code = sm_channel.BarcodeId;
        //        }
        //        else
        //        {
        //            sm_code = String.Empty;
        //        }
        //    }
        //    return rc;
        //}

        ///// <summary>
        ///// 取得指定金額對指定管道手續費的關係 (台企銀的邏輯是手續費內含就是企業負擔，外加就是繳款人負擔，所以各管道的金額都一樣，不用計算各管道金額)
        ///// </summary>
        ///// <param name="amount">指定金額</param>
        ///// <param name="receiveChannels">指定所有管道手續費資料</param>
        ///// <param name="smAmount">超商應繳金額</param>
        ///// <param name="smChannel">超商管道手續費資料</param>
        ///// <param name="poAmount">郵局應繳金額</param>
        ///// <param name="poChannel">郵局管道手續費資料</param>
        ///// <param name="cashAmount">臨櫃應繳金額</param>
        ///// <param name="cashChannel">臨櫃管道手續費資料</param>
        ///// <returns>成功則傳回 true，否則傳回 false</returns>
        //public bool GetChannelFee(decimal amount, ReceiveChannelEntity[] receiveChannels
        //    , out decimal smAmount, out ReceiveChannelEntity smChannel
        //    , out decimal poAmount, out ReceiveChannelEntity poChannel
        //    , out decimal cashAmount, out ReceiveChannelEntity cashChannel)
        //{
        //    smAmount = 0M;
        //    smChannel = null;
        //    poAmount = 0M;
        //    poChannel = null;
        //    cashAmount = 0M;
        //    cashChannel = null;

        //    if (receiveChannels == null || receiveChannels.Length == 0 || amount <= 0)
        //    {
        //        return false;
        //    }

        //    #region 計算手續費
        //    foreach (ReceiveChannelEntity receiveChannel in receiveChannels)
        //    {
        //        decimal maxAcount = receiveChannel.MaxMoney ?? 0;
        //        decimal minAmount = receiveChannel.MinMoney;
        //        if (amount >= minAmount && (amount <= maxAcount || maxAcount == 0))
        //        {
        //            if (receiveChannel.IsPOChannel())
        //            {
        //                poChannel = receiveChannel;
        //                if (receiveChannel.IncludePay == "1")   //內含手續費
        //                {
        //                    if (receiveChannel.ChannelCharge == null)
        //                    {
        //                        return false;
        //                    }
        //                    poAmount = amount + receiveChannel.ChannelCharge.Value;
        //                }
        //                else
        //                {
        //                    poAmount = amount;
        //                }
        //            }
        //            else if (receiveChannel.IsSMChannel())
        //            {
        //                smChannel = receiveChannel;
        //                if (receiveChannel.IncludePay == "1")   //內含手續費
        //                {
        //                    if (receiveChannel.ChannelCharge == null)
        //                    {
        //                        return false;
        //                    }
        //                    smAmount = amount + receiveChannel.ChannelCharge.Value;
        //                }
        //                else
        //                {
        //                    smAmount = amount;
        //                }
        //            }
        //            else if (receiveChannel.IsCashChannel())
        //            {
        //                cashChannel = receiveChannel;
        //                if (receiveChannel.IncludePay == "1")   //內含手續費
        //                {
        //                    if (receiveChannel.ChannelCharge == null)
        //                    {
        //                        return false;
        //                    }
        //                    cashAmount = amount + receiveChannel.ChannelCharge.Value;
        //                }
        //                else
        //                {
        //                    cashAmount = amount;
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    return true;
        //}
        #endregion
        #endregion

        #region Static Method
        /// <summary>
        /// 取得指定管道代碼是否為超商管道 （SM_711, SM_TFM, SM_OKM, SM_HILI)
        /// </summary>
        /// <param name="channelId">指定管道代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsSMChannel(string channelId)
        {
            return (channelId == SM_711 || channelId == SM_TFM || channelId == SM_OKM || channelId == SM_NIK || channelId == SM_HILI);
        }

        /// <summary>
        /// 取得指定管道代碼是否為預設超商管道 (SM_HILI)
        /// </summary>
        /// <param name="channelId">指定管道代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefaultSMChannel(string channelId)
        {
            return (channelId == SM_DEFAULT);
        }

        /// <summary>
        /// 取得指定管道代碼是否為臨櫃管道 (TABS)
        /// </summary>
        /// <param name="channelId">指定管道代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsCashChannel(string channelId)
        {
            return (channelId == TABS);
        }

        /// <summary>
        /// 取得指定管道代碼是否為郵局管道 (PO)
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static bool IsPOChannel(string channelId)
        {
            return (channelId == PO);
        }

        /// <summary>
        /// 取得所有超商的管道代碼
        /// </summary>
        /// <returns>傳回超商的管道代碼陣列</returns>
        public static string[] GetSMChannelIds()
        {
            return new string[] { SM_711, SM_TFM, SM_OKM, SM_NIK, SM_HILI };
        }

        /// <summary>
        /// 取得代收管道代碼的名稱
        /// </summary>
        /// <param name="channelId">指定代收管道代碼</param>
        /// <returns>傳回代收管道名稱</returns>
        public static string GetChannelName(string channelId)
        {
            if (String.IsNullOrWhiteSpace(channelId))
            {
                return String.Empty;
            }

            switch (channelId)
            {
                case TABS:
                    return "臨櫃";

                case RM:
                    return "匯款";

                case ATM:
                    return "ATM";

                case EDI:
                    return "EDI";

                case NB:
                    return "網路銀行";

                case VO:
                    return "語音銀行";

                #region [MDY:20170506] 增加境外支付 (支付寶)
                case ALIPAY:    //支付寶 (財金境外支付)
                    return "支付寶";
                #endregion

                #region [MDY:20171127] 增加08-全國繳費網、10-台灣Pay (20170831_01)
                case EBILL:    //全國繳費網
                    return "全國繳費網";
                case TWPAY:    //台灣Pay (QRCode)
                    return "台灣Pay";
                #endregion

                #region 超商
                case SM_711:
                    return "超商7-11";
                case SM_TFM:
                    return "超商全家";
                case SM_OKM:
                    return "超商OK";
                case SM_NIK:
                    return "超商福客多";
                case SM_HILI:
                    return "超商萊爾富";
                case SM_DEFAULT:
                    return "超商(手續費用)";
                #endregion

                #region [MDY:20190906] (2019擴充案) 超商行動版 (虛擬的管道代碼，僅行動版產生超商條碼用，計算超商金額不使用此設定，帳務中心的管道不會有這個代碼)
                case SM_MOBILE:
                    return "超商行動版(手續費用)";
                #endregion

                #region 信用卡
                case FISC:
                    return "信用卡(財金)";

                #region [MDY:20191214] (2019擴充案) 財金國際信用卡 : NC
                case FISC_NC:
                    return "國際信用卡";
                #endregion

                #region [MDY:20170506] 修正中信管道常數為 CTCB
                #region [Old]
                //case CTCD:
                //    return "信用卡(中國信託)";
                #endregion

                case CTCB:
                    return "信用卡(中國信託)";
                #endregion
                #endregion

                case PO:
                    return "郵局";

                case SELF:
                    return "商家自收";

                case LB:
                    return "土銀委扣";

                default:
                    return "其他";
            }
        }
        #endregion

        #region [Old]
        //#region 異業代收管道相關方法

        //#region [MDY:20170506] 增加支付寶 & 修正中信管道常數為 CTCB
        //#region [Old]
        /////// <summary>
        /////// 異業代收管道代碼 (四大超商、中信銀、銀聯卡、財金)
        /////// </summary>
        ////private static readonly string[] _EDPChannelIds = new string[] { SM_711, SM_TFM, SM_OKM, SM_HILI, FISC, CTCD };
        //#endregion

        ///// <summary>
        ///// 異業代收管道代碼 (四大超商、財金、中信銀(含銀聯卡)、支付寶)
        ///// </summary>
        //private static readonly string[] _EDPChannelIds = new string[] { SM_711, SM_TFM, SM_OKM, SM_HILI, FISC, CTCB, ALIPAY };
        //#endregion

        ///// <summary>
        ///// 取得指定管道代碼是否為異業代收管道
        ///// </summary>
        ///// <param name="channelId"></param>
        ///// <returns></returns>
        //public static bool IsEDPChannel(string channelId)
        //{
        //    channelId = channelId == null ? String.Empty : channelId.Trim();
        //    return (Array.IndexOf(_EDPChannelIds, channelId) > -1);
        //}

        ///// <summary>
        ///// 取得異業代收管道代碼 (四大超商、中信銀、銀聯卡、財金、支付寶)
        ///// </summary>
        ///// <returns></returns>
        //public static string[] GetEDPChannelIds()
        //{
        //    string[] datas = new string[_EDPChannelIds.Length];
        //    _EDPChannelIds.CopyTo(datas, 0);
        //    return datas;
        //}
        //#endregion
        #endregion

        #region [MDY:20170506] 有做已繳代銷的代收管道相關方法 (因為支付寶沒做已繳代銷所以要分開)

        #region [MDY:20191214] (2019擴充案) 增加國際信用卡
        /// <summary>
        /// 有做已繳代銷的代收管道代碼 (四大超商、中信銀(含銀聯卡)、財金、財金國際信用卡)
        /// </summary>
        private static readonly string[] _PreCancelChannelIds = new string[] { SM_711, SM_TFM, SM_OKM, SM_HILI, CTCB, FISC, FISC_NC };
        #endregion

        /// <summary>
        ///取得指定管道代碼是否為有做已繳代銷的代收管道
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static bool IsPreCancelChannel(string channelId)
        {
            channelId = channelId == null ? String.Empty : channelId.Trim();
            return (Array.IndexOf(_PreCancelChannelIds, channelId) > -1);
        }

        /// <summary>
        /// 取得有做已繳代銷的代收管道代碼 (四大超商、中信銀(含銀聯卡)、財金、財金國際信用卡)
        /// </summary>
        /// <returns></returns>
        public static string[] GetPreCancelChannelIds()
        {
            string[] datas = new string[_PreCancelChannelIds.Length];
            _PreCancelChannelIds.CopyTo(datas, 0);
            return datas;
        }
        #endregion
    }
}
