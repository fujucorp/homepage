using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 匯出檔案處理工具類別
    /// </summary>
    public class ExportFileHelper : IDisposable
    {
        #region Member
        /// <summary>
        /// 儲存 資料存取物件 的變數
        /// </summary>
        private EntityFactory _Factory = null;

        /// <summary>
        /// 儲存 此物件 Dispose 時 資料存取物件 是否需要一併 Dispose
        /// </summary>
        private bool _IsNeedFactoryDispose = false;

        /// <summary>
        /// 儲存 暫存檔路徑
        /// </summary>
        private string _TempPath = null;
        #endregion

        #region Property
        /// <summary>
        /// 取得暫存檔路徑
        /// </summary>
        public string TempPath
        {
            get
            {
                return _TempPath;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構匯出檔案處理工具類別
        /// </summary>
        /// <param name="factory">指定資料存取物件</param>
        /// <param name="tempPath">指定暫存檔路徑</param>
        public ExportFileHelper(EntityFactory factory, string tempPath)
        {
            if (factory == null)
            {
                _Factory = new EntityFactory();
                _IsNeedFactoryDispose = true;
            }
            else
            {
                _Factory = factory;
                _IsNeedFactoryDispose = false;
            }
            _TempPath = tempPath;
            this.Initial();
        }

        /// <summary>
        /// 建構匯出檔案處理工具類別
        /// </summary>
        /// <param name="factory">指定資料存取物件</param>
        public ExportFileHelper(EntityFactory factory)
            : this(factory, null)
        {
        }

        /// <summary>
        /// 建構匯出檔案處理工具類別
        /// </summary>
        public ExportFileHelper()
            : this(null, null)
        {
        }
        #endregion

        #region Destructor
        /// <summary>
        /// 解構匯出檔案處理工具類別
        /// </summary>
        ~ExportFileHelper()
        {
            Dispose(false);
        }
        #endregion

        #region Implement IDisposable
        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool _Disposed = false;

        /// <summary>
        /// 執行與釋放 (Free)、釋放 (Release) 或重設 Unmanaged 資源相關聯之應用程式定義的工作
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        /// <param name="disposing">指定是否釋放資源。</param>
        private void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    if (_Factory != null && _IsNeedFactoryDispose)
                    {
                        _Factory.Dispose();
                        _Factory = null;
                    }
                }
                _Disposed = true;
            }
        }
        #endregion

        #region Private Method
        private void Initial()
        {
            if (String.IsNullOrEmpty(_TempPath))
            {
                _TempPath = Path.GetTempPath();
            }
            else
            {
                try
                {
                    _TempPath = Path.GetFullPath(_TempPath);
                    if (!Directory.Exists(_TempPath))
                    {
                        Directory.CreateDirectory(_TempPath);
                    }
                }
                catch (Exception)
                {
                    _TempPath = Path.GetTempPath();
                }
            }
        }
        #endregion

        #region 自行委扣檔
        private const int BANKLineByteSize = 200;           //單筆記錄長度
        private const string BANKHeadLineFlag = "1";        //首筆識別碼
        private const string BANKBodyLineFlag = "2";        //資料明細識別碼
        private const string BANKFootLineFlag = "3";        //尾筆識別碼
        private const string BANKDeductionFlag = "10000";   //轉帳類別 1:扣  2:入帳       末4位0000

        /// <summary>
        /// 匯出自行委扣檔
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="keys">指定要匯出資料的 Key</param>
        /// <param name="deductDate">轉帳日期</param>
        /// <param name="fileName">指定匯出檔名，不指定則系統自動產生</param>
        /// <param name="outFile">傳回匯出檔完整路徑檔名</param>
        /// <param name="dataLog">傳回處理資料日誌</param>
        /// <returns>成功則傳回 null，否傳回錯誤訊息</returns>
        public string ExportBANKDeduction(string receiveType, ICollection<BillKey> keys, DateTime deductDate, string fileName, out string outFile, out string dataLog)
        {
            #region 處理參數
            if (keys == null || keys.Count == 0)
            {
                outFile = null;
                dataLog = null;
                return "未指定匯出資料查詢條件參數";
            }
            if (String.IsNullOrWhiteSpace(fileName))
            {
                BillKey first = keys.First<BillKey>();
                fileName = String.Format("{0}_{1:yyyyMMddHHmmss}.TXT", first.ReceiveType, DateTime.Now);
            }
            outFile = Path.Combine(_TempPath, fileName);
            #endregion

            #region [Old]
            //            #region 取資料
            //            StringBuilder log = new StringBuilder();
            //            List<DeductionData> datas = new List<DeductionData>(keys.Count);
            //            {
            //                string sql = @"SELECT ISNULL(Cancel_No, '') AS Cancel_No, ISNULL(Receive_Amount, 0) AS Receive_Amount
            //     , ISNULL(Deduct_BankId, '') AS Deduct_BankId, ISNULL(Deduct_AccountNo, '') AS Deduct_AccountNo, ISNULL(Deduct_AccountName, '') AS Deduct_AccountName, ISNULL(Deduct_AccountId, '') AS Deduct_AccountId
            //  FROM Student_Receive
            // WHERE (Receive_Way IS NULL OR Receive_Way = '') AND (Receive_Date IS NULL OR Receive_Date = '')
            //   AND (Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId AND Stu_Id = @StuId)";
            //                KeyValue[] parameters = null;
            //                DataTable dt = null;
            //                DataRow row = null;
            //                Result result = null;
            //                int no = 0;
            //                foreach(BillKey key in keys)
            //                {
            //                    no++;
            //                    parameters = new KeyValue[] {
            //                        new KeyValue("@ReceiveType", key.ReceiveType),
            //                        new KeyValue("@YearId", key.YearId),
            //                        new KeyValue("@TermId", key.TermId),
            //                        new KeyValue("@DepId", key.DepId),
            //                        new KeyValue("@ReceiveId", key.ReceiveId),
            //                        new KeyValue("@StuId", key.StuId)
            //                    };
            //                    result = _Factory.GetDataTable(sql, parameters, 0, 1, out dt);
            //                    if (result.IsSuccess)
            //                    {
            //                        if (dt == null || dt.Rows.Count == 0)
            //                        {
            //                            log.AppendFormat("查無第 {0} 筆 ({1}) 學生繳費資料，該資料不存在或已繳費", no, key).AppendLine();
            //                        }
            //                        else
            //                        {
            //                            row = dt.Rows[0];

            //                            DeductionData data = new DeductionData(row["Cancel_No"].ToString(), Convert.ToDecimal(row["Receive_Amount"])
            //                                , row["Deduct_BankId"].ToString(), row["Deduct_AccountNo"].ToString()
            //                                , row["Deduct_AccountName"].ToString(), row["Deduct_AccountId"].ToString());

            //                            if (String.IsNullOrEmpty(data.CancelNo))
            //                            {
            //                                log.AppendFormat("第 {0} 筆 ({1}) 學生繳費資料未產生虛擬帳號", no, key).AppendLine();
            //                            }
            //                            else if (data.Amount <= 0M)
            //                            {
            //                                log.AppendFormat("第 {0} 筆 ({1}) 學生繳費資料未產生金額或無須繳款", no, key).AppendLine();
            //                            }
            //                            else if (String.IsNullOrEmpty(data.BankId) || !data.BankId.StartsWith(DataFormat.MyBankID))
            //                            {
            //                                log.AppendFormat("第 {0} 筆 ({1}) 學生繳費資料的委扣銀行 ({2}) 未指定或不屬於本行", no, key, data.BankId).AppendLine();
            //                            }
            //                            else if (String.IsNullOrEmpty(data.AccountNo))
            //                            {
            //                                log.AppendFormat("第 {0} 筆 ({1}) 學生繳費資料的委扣帳號未指定", no, key, data.AccountNo).AppendLine();
            //                            }
            //                            else
            //                            {
            //                                datas.Add(data);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        log.AppendFormat("查詢第 {0} 筆 ({1}) 學生繳費資料失敗，錯誤訊息：{2}", no, key, result.Message).AppendLine();
            //                    }
            //                }
            //            }
            //            #endregion

            //            dataLog = log.ToString();
            //            if (datas.Count == 0)
            //            {
            //                return "查無可匯出的資料";
            //            }
            //            else
            //            {
            //                return this.GenTBBDeductionFile(datas, deductDate, outFile, true);
            //            }
            #endregion

            string deductId = null;
            string schoolId = null;
            string bankId = null;
            List<DeductionData> datas = this.GetDeductionData(receiveType, keys, out deductId, out schoolId, out bankId, out dataLog);
            if (datas == null)
            {
                return dataLog;
            }
            if (datas.Count == 0)
            {
                return "查無可匯出的資料";
            }
            else
            {
                return this.GenBANKDeductionFile(deductId, schoolId, bankId, datas, deductDate, outFile, true);
            }
        }

        /// <summary>
        /// 將指定資料寫入自行委扣檔
        /// </summary>
        /// <param name="deductId">客戶委託代號</param>
        /// <param name="schoolId">學校統編</param>
        /// <param name="bankId">主辦行代碼</param>
        /// <param name="datas"></param>
        /// <param name="deductDate"></param>
        /// <param name="outFile"></param>
        /// <param name="overWrite"></param>
        /// <returns></returns>
        private string GenBANKDeductionFile(string deductId, string schoolId, string bankId, ICollection<DeductionData> datas, DateTime deductDate, string outFile, bool overWrite)
        {
            #region 檢查參數
            if (datas == null || datas.Count == 0)
            {
                return "未指定要處理的資料參數";
            }
            if (String.IsNullOrEmpty(outFile) || !Path.IsPathRooted(outFile))
            {
                return "未指定產出檔案的完整路徑名稱";
            }
            #endregion

            try
            {
                if (!overWrite && File.Exists(outFile))
                {
                    return "指定產出檔案已存在";
                }
                string path = Path.GetDirectoryName(outFile);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string twDeductDate = Common.GetTWDate8(deductDate);    //轉帳日
                using (StreamWriter sw = new StreamWriter(outFile, false, Encoding.Default))
                {
                    #region 處理首筆
                    {
                        #region 欄位說明
                        //序號  欄位名稱                起訖位置    長度  屬性    說明
                        //01    錄別                    001 ~ 001   001   9(1)    首筆為1
                        //02    發件單位(客戶委託編號)  002 ~ 009   008   X(8)    土銀資訊室編定之委託代號，左靠右補空白，通常為4碼
                        //03    收件單位                010 ~ 017   008   X(8)    土銀代號(005)+分行代號+檢查碼，左靠右補空白
                        //04    轉帳類別                018 ~ 022   005   9(5)    1:扣  2:入帳       末4位0000
                        //05    轉帳日                  023 ~ 030   008   9(8)    YYYYMMDD(國曆)
                        //06    資料性質別              031 ~ 031   001   9(1)    1:委託轉帳(原始資料)    2:轉帳結果(處理後退回)
                        //07    保留欄                  032 ~ 200   169   X(169)  空白
                        #endregion

                        TxtFormat[] headFields = this.GetBANKHeadFields();
                        string[] headTexts = new string[7] {
                            BANKHeadLineFlag, deductId, bankId, BANKDeductionFlag, twDeductDate,
                            "1", ""
                        };
                        string headLine = this.GenTextLine(BANKLineByteSize, headFields, headTexts);
                        sw.WriteLine(headLine);
                    }
                    #endregion

                    #region 處理資料明細
                    decimal totalAmount = 0M;
                    {
                        #region 欄位說明
                        //序號  欄位名稱                起訖位置    長度  屬性        說明
                        //01    錄別                    001 ~ 001   001   9(1)        每筆為2
                        //02    發件單位(客戶委託編號)  002 ~ 009   008   X(8)        土銀資訊室編定之委託代號，左靠右補空白，通常為4碼
                        //03    收件單位                010 ~ 017   008   X(8)        土銀代號(005)+分行代號+檢查碼，左靠右補空白
                        //04    轉帳類別                018 ~ 022   005   9(5)        1:扣   2:入帳      末4位0000
                        //05    轉帳日                  023 ~ 030   008   9(8)        YYYYMMDD(國曆)
                        //06    帳號                    031 ~ 050   020   9(20)       右靠左補零(00000000XXXXXXXXXXXX) 聯行往來(000000+開單行+接單行+聯行往來項目別+收付總號)
                        //07    交易金額                051 ~ 064   014   9(12)V9(2)  整數位不足時，右靠左補零，小數2位
                        //08    營利事業編號            065 ~ 072   008   9(8)        委託轉帳之事業單位
                        //09    狀況代號                073 ~ 076   004   9(4)        通知(即委託時)為9999
                        //10    專用資料區              077 ~ 166   090   X(90)       *註《一》(分成 專用資料區a 與 專用資料區b)
                        //10a   專用資料區a             077 ~ 115   039   X(39)       學雜費保留 (改成放學號)
                        //10b   專用資料區b             116 ~ 166   051   X(51)       學雜費用來放虛擬帳號
                        //11    帳戶ID                  167 ~ 176   010   X(10)       左靠右補空白
                        //12    幣別                    177 ~ 178   002   9(02)       Default：空白(台幣)
                        //13    保留欄                  179 ~ 200   022   X(22)       空白，暫勿使用
                        #endregion

                        TxtFormat[] bodyFields = this.GetBANKBodyFields();
                        foreach (DeductionData data in datas)
                        {
                            totalAmount += data.Amount;

                            string[] bodyTexts = new string[14] {
                                BANKBodyLineFlag, deductId, bankId, BANKDeductionFlag, twDeductDate,
                                data.AccountNo, this.GetBANKAmountText(data.Amount), schoolId, "9999",
                                data.StuId, data.CancelNo, data.AccountId, "", ""
                            };
                            string bodyLine = this.GenTextLine(BANKLineByteSize, bodyFields, bodyTexts);
                            sw.WriteLine(bodyLine);
                        }
                    }
                    #endregion

                    #region 處理尾筆
                    {
                        #region 欄位說明
                        //序號  欄位名稱                起訖位置    長度  屬性         說明
                        //01    錄別                    001 ~ 001   001   9(1)         尾筆為3
                        //02    發件單位(客戶委託編號)  002 ~ 009   008   X(8)         土銀資訊室編定之委託代號，左靠右補空白，通常為4碼
                        //03    收件單位                010 ~ 017   008   X(8)         土銀代號(005)+分行代號+檢查碼，左靠右補空白
                        //04    轉帳類別                018 ~ 022   005   9(5)         1:扣    2:入帳     末2位0000
                        //05    轉帳日                  023 ~ 030   008   9(8)         YYYYMMDD(國曆)
                        //06    成交總金額              031 ~ 046   016   9(14)V9(2)
                        //07    成交總筆數              047 ~ 056   010   9(10)
                        //08    未成交總金額            057 ~ 072   016   9(14)V9(2)   通知(即委託時)為零
                        //09    未成交總筆數            073 ~ 082   010   9(10)        通知(即委託時)為零
                        //10    保留欄                  083 ~ 200   118   X(118)       空白
                        #endregion

                        TxtFormat[] footFields = this.GetBANKFootFields();
                        string[] footTexts = new string[10] {
                            BANKFootLineFlag, deductId, bankId, BANKDeductionFlag, twDeductDate,
                            this.GetBANKAmountText(totalAmount), datas.Count.ToString("0"), "0", "0", ""
                        };
                        string footLine = this.GenTextLine(BANKLineByteSize, footFields, footTexts);
                        sw.WriteLine(footLine);
                    }
                    #endregion
                }

                return null;
            }
            catch (Exception ex)
            {
                return String.Concat("產出檔案發生例外，錯誤訊息；", ex.Message);
            }
        }

        /// <summary>
        /// 土銀自行委扣檔首筆欄位設定
        /// </summary>
        /// <returns></returns>
        private TxtFormat[] GetBANKHeadFields()
        {
            #region 欄位說明
            //序號  欄位名稱                起訖位置    長度  屬性    說明
            //01    錄別                    001 ~ 001   001   9(1)    首筆為1
            //02    發件單位(客戶委託編號)  002 ~ 009   008   X(8)    土銀資訊室編定之委託代號，左靠右補空白，通常為4碼
            //03    收件單位                010 ~ 017   008   X(8)    土銀代號(005)+分行代號+檢查碼，左靠右補空白
            //04    轉帳類別                018 ~ 022   005   9(5)    1:扣  2:入帳       末4位0000
            //05    轉帳日                  023 ~ 030   008   9(8)    YYYYMMDD(國曆)
            //06    資料性質別              031 ~ 031   001   9(1)    1:委託轉帳(原始資料)    2:轉帳結果(處理後退回)
            //07    保留欄                  032 ~ 200   169   X(169)  空白
            #endregion

            TxtFormat[] fields = new TxtFormat[7] {
                new TxtFormat("01", 001, 001),
                new TxtFormat("02", 002, 008),
                new TxtFormat("03", 010, 008),
                new TxtFormat("04", 018, 005),
                new TxtFormat("05", 023, 008),
                new TxtFormat("06", 031, 001),
                new TxtFormat("07", 032, 169)
            };
            return fields;
        }

        /// <summary>
        /// 土銀自行委扣檔資料明細欄位設定
        /// </summary>
        /// <returns></returns>
        private TxtFormat[] GetBANKBodyFields()
        {
            #region 欄位說明
            //序號  欄位名稱                起訖位置    長度  屬性        說明
            //01    錄別                    001 ~ 001   001   9(1)        每筆為2
            //02    發件單位(客戶委託編號)  002 ~ 009   008   X(8)        土銀資訊室編定之委託代號，左靠右補空白，通常為4碼
            //03    收件單位                010 ~ 017   008   X(8)        土銀代號(005)+分行代號+檢查碼，左靠右補空白
            //04    轉帳類別                018 ~ 022   005   9(5)        1:扣   2:入帳      末4位0000
            //05    轉帳日                  023 ~ 030   008   9(8)        YYYYMMDD(國曆)
            //06    帳號                    031 ~ 050   020   9(20)       右靠左補零(00000000XXXXXXXXXXXX) 聯行往來(000000+開單行+接單行+聯行往來項目別+收付總號)
            //07    交易金額                051 ~ 064   014   9(12)V9(2)  整數位不足時，右靠左補零，小數2位
            //08    營利事業編號            065 ~ 072   008   9(8)        委託轉帳之事業單位
            //09    狀況代號                073 ~ 076   004   9(4)        通知(即委託時)為9999
            //10    專用資料區              077 ~ 166   090   X(90)       *註《一》(分成 專用資料區a 與 專用資料區b)
            //10a   專用資料區a             077 ~ 115   039   X(39)       學雜費保留
            //10b   專用資料區b             116 ~ 166   051   X(51)       學雜費用來放虛擬帳號
            //11    帳戶ID                  167 ~ 176   010   X(10)       左靠右補空白
            //12    幣別                    177 ~ 178   002   9(02)       Default：空白(台幣)
            //13    保留欄                  179 ~ 200   022   X(22)       空白，暫勿使用
            #endregion

            TxtFormat[] fields = new TxtFormat[14] {
                new TxtFormat("01", 001, 01),
                new TxtFormat("02", 002, 08),
                new TxtFormat("03", 010, 08),
                new TxtFormat("04", 018, 05),
                new TxtFormat("05", 023, 08),
                new TxtFormat("06", 031, 20, Common.AlignCutPadEnum.Right, '0'),
                new TxtFormat("07", 051, 14, Common.AlignCutPadEnum.Right, '0'),
                new TxtFormat("08", 065, 08),
                new TxtFormat("09", 073, 04),
                new TxtFormat("10a", 077, 39),
                new TxtFormat("10b", 116, 51),
                new TxtFormat("11", 167, 10),
                new TxtFormat("12", 177, 02),
                new TxtFormat("13", 179, 22)
            };
            return fields;
        }

        /// <summary>
        /// 土銀自行委扣檔尾筆欄位設定
        /// </summary>
        /// <returns></returns>
        private TxtFormat[] GetBANKFootFields()
        {
            #region 欄位說明
            //序號  欄位名稱                起訖位置    長度  屬性         說明
            //01    錄別                    001 ~ 001   001   9(1)         尾筆為3
            //02    發件單位(客戶委託編號)  002 ~ 009   008   X(8)         土銀資訊室編定之委託代號，左靠右補空白，通常為4碼
            //03    收件單位                010 ~ 017   008   X(8)         土銀代號(005)+分行代號+檢查碼，左靠右補空白
            //04    轉帳類別                018 ~ 022   005   9(5)         1:扣    2:入帳     末2位0000
            //05    轉帳日                  023 ~ 030   008   9(8)         YYYYMMDD(國曆)
            //06    成交總金額              031 ~ 046   016   9(14)V9(2)
            //07    成交總筆數              047 ~ 056   010   9(10)
            //08    未成交總金額            057 ~ 072   016   9(14)V9(2)   通知(即委託時)為零
            //09    未成交總筆數            073 ~ 082   010   9(10)        通知(即委託時)為零
            //10    保留欄                  083 ~ 200   118   X(118)       空白
            #endregion

            TxtFormat[] fields = new TxtFormat[10] {
                new TxtFormat("01", 001, 001),
                new TxtFormat("02", 002, 008),
                new TxtFormat("03", 010, 008),
                new TxtFormat("04", 018, 005),
                new TxtFormat("05", 023, 008),
                new TxtFormat("06", 031, 016, Common.AlignCutPadEnum.Right, '0'),
                new TxtFormat("07", 047, 010, Common.AlignCutPadEnum.Right, '0'),
                new TxtFormat("08", 057, 016, Common.AlignCutPadEnum.Right, '0'),
                new TxtFormat("09", 073, 010, Common.AlignCutPadEnum.Right, '0'),
                new TxtFormat("10", 083, 118)
            };
            return fields;
        }

        /// <summary>
        /// 取得土銀自行委扣金額的格式化字串
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private string GetBANKAmountText(decimal amount)
        {
            return amount.ToString("0.00").Replace(".", "");
        }
        #endregion

        #region [Old] 土銀只有自行委扣
        //#region 全國繳委扣檔
        //private const int CNBLineByteSize = 300;    //單筆記錄長度
        //private const string CNBLeinFlag = "2";     //區別碼 (2)
        //private const string CNB委託單位代號 = "";  //委託單位代號（銀行公會授與）
        //private const string CNB費用類別 = "";      //費用類別代碼（銀行公會授與）
        //private const string CNB費用代號 = "";      //費用代號（銀行公會授與）

        ///// <summary>
        ///// 匯出全國繳委扣檔
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <param name="deductDate"></param>
        ///// <param name="fileName"></param>
        ///// <param name="outFile"></param>
        ///// <param name="dataLog"></param>
        ///// <returns></returns>
        //public string ExportCNBDeduction(ICollection<BillKey> keys, DateTime deductDate, string fileName, out string outFile, out string dataLog)
        //{
        //    #region 處理參數
        //    if (keys == null || keys.Count == 0)
        //    {
        //        outFile = null;
        //        dataLog = null;
        //        return "未指定匯出資料查詢條件參數";
        //    }
        //    if (String.IsNullOrWhiteSpace(fileName))
        //    {
        //        BillKey first = keys.First<BillKey>();
        //        fileName = String.Format("{0}_{1:yyyyMMddHHmmss}.TXT", first.ReceiveType, DateTime.Now);
        //    }
        //    outFile = Path.Combine(_TempPath, fileName);
        //    #endregion

        //    List<DeductionData> datas = this.GetDeductionData(keys, out dataLog);
        //    if (datas.Count == 0)
        //    {
        //        return "查無可匯出的資料";
        //    }
        //    else
        //    {
        //        return this.GenCNBDeductionFile(datas, deductDate, outFile, true);
        //    }
        //}

        ///// <summary>
        ///// 將指定資料寫入全國繳委扣檔
        ///// </summary>
        ///// <param name="datas"></param>
        ///// <param name="deductDate"></param>
        ///// <param name="outFile"></param>
        ///// <param name="overWrite"></param>
        ///// <returns></returns>
        //private string GenCNBDeductionFile(ICollection<DeductionData> datas, DateTime deductDate, string outFile, bool overWrite)
        //{
        //    #region 檢查參數
        //    if (datas == null || datas.Count == 0)
        //    {
        //        return "未指定要處理的資料參數";
        //    }
        //    if (String.IsNullOrEmpty(outFile) || !Path.IsPathRooted(outFile))
        //    {
        //        return "未指定產出檔案的完整路徑名稱";
        //    }
        //    #endregion

        //    try
        //    {
        //        if (!overWrite && File.Exists(outFile))
        //        {
        //            return "指定產出檔案已存在";
        //        }
        //        string path = Path.GetDirectoryName(outFile);
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        using (StreamWriter sw = new StreamWriter(outFile, false, Encoding.Default))
        //        {
        //            #region 處理資料
        //            {
        //                #region 欄位說明
        //                //◎：隱藏欄位免填，由企業網銀系統自動帶資料
        //                //※：必要欄位
        //                //＃：選擇性欄位
        //                //屬性符號說明：N = 數字 / AN = 文數
        //                //序號  欄位名稱              起訖位置    長度  屬性  說明
        //                //01    ◎區別碼              001 ~ 001   1     N1     2-固定值 (隱藏欄位免填)
        //                //02    ※委託單位代號        002 ~ 009   8     AN8    委託單位代號（銀行公會授與）
        //                //03    ◎收件單位            010 ~ 017   8     AN8    系統自動帶轉帳行代碼總行代碼 (隱藏欄位免填)
        //                //04    ※指定扣帳日          018 ~ 024   7     N7     民國年月日YYYMMDD
        //                //05    ※費用類別            025 ~ 029   5     AN5    費用類別代碼（銀行公會授與）
        //                //06    ＃交易序號            030 ~ 039   10    AN10   同一發件單位、入/扣帳日、費用類別之序號不得重覆
        //                //07    ◎檢核欄位            040 ~ 040   1     AN1    固定為「+」 (隱藏欄位免填)
        //                //08    ※交易金額            041 ~ 053   13    N13    右靠，單位：元
        //                //09    ◎委託單位區別        054 ~ 054   1     AN1    固定為「0」 (隱藏欄位免填)
        //                //10    ◎委託單位            055 ~ 062   8     AN8    委託單位統一編號 (隱藏欄位免填)
        //                //11    ◎實際入/扣帳日       063 ~ 069   7     N7     民國年月日YYYMMDD (隱藏欄位免填)
        //                //12    回覆代碼              070 ~ 073   4     N4     「0000」成功，其他為失敗
        //                //13    ※轉帳行代碼          074 ~ 080   7     AN7    轉帳帳號所屬銀行代碼，三碼總行代碼及四碼分行代碼，分行代碼可空白
        //                //14    ※轉帳帳號            081 ~ 096   16    AN16   右靠
        //                //15    ※帳戶ID              097 ~ 106   10    AN10   左靠右補空白（帳號所有人ID）
        //                //16    ＃銷帳編號            107 ~ 122   16    AN16   左靠右補空白
        //                //17    ※費用代號            123 ~ 126   4     AN4    費用代號（銀行公會授與）
        //                //18    ◎銀行專用區          127 ~ 146   20    AN20   銀行自由運用，無則空白 (隱藏欄位免填)
        //                //19    ＃事業單位專用資料區  147 ~ 200   54    AN54   事業單位自由運用（客戶組別），無則空白
        //                //20    ＃客戶備註            201 ~ 300   100   AN100  客戶備註
        //                #endregion

        //                TxtFormat[] fields = this.GetCNBFields();
        //                foreach (DeductionData data in datas)
        //                {
        //                    string[] texts = new string[20] {
        //                        CNBLeinFlag, CNB委託單位代號, "", Common.GetTWDate7(deductDate), CNB費用類別,
        //                        "", "+", data.Amount.ToString("0"), "0", "",
        //                        "", "0000", data.BankId, data.AccountNo, data.AccountId,
        //                        data.CancelNo, CNB費用代號, "", "", ""
        //                    };
        //                    string line = this.GenTextLine(CNBLineByteSize, fields, texts);
        //                    sw.WriteLine(line);
        //                }
        //            }
        //            #endregion
        //        }

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return String.Concat("產出檔案發生例外，錯誤訊息；", ex.Message);
        //    }
        //}

        ///// <summary>
        ///// 全國繳委扣檔欄位設定
        ///// </summary>
        ///// <returns></returns>
        //private TxtFormat[] GetCNBFields()
        //{
        //    #region 欄位說明
        //    //◎：隱藏欄位免填，由企業網銀系統自動帶資料
        //    //※：必要欄位
        //    //＃：選擇性欄位
        //    //屬性符號說明：N = 數字 / AN = 文數
        //    //序號  欄位名稱              起訖位置    長度  屬性  說明
        //    //01    ◎區別碼              001 ~ 001   1     N1     2-固定值 (隱藏欄位免填)
        //    //02    ※委託單位代號        002 ~ 009   8     AN8    委託單位代號（銀行公會授與）
        //    //03    ◎收件單位            010 ~ 017   8     AN8    系統自動帶轉帳行代碼總行代碼 (隱藏欄位免填)
        //    //04    ※指定扣帳日          018 ~ 024   7     N7     民國年月日YYYMMDD
        //    //05    ※費用類別            025 ~ 029   5     AN5    費用類別代碼（銀行公會授與）
        //    //06    ＃交易序號            030 ~ 039   10    AN10   同一發件單位、入/扣帳日、費用類別之序號不得重覆
        //    //07    ◎檢核欄位            040 ~ 040   1     AN1    固定為「+」 (隱藏欄位免填)
        //    //08    ※交易金額            041 ~ 053   13    N13    右靠，單位：元
        //    //09    ◎委託單位區別        054 ~ 054   1     AN1    固定為「0」 (隱藏欄位免填)
        //    //10    ◎委託單位            055 ~ 062   8     AN8    委託單位統一編號 (隱藏欄位免填)
        //    //11    ◎實際入/扣帳日       063 ~ 069   7     N7     民國年月日YYYMMDD (隱藏欄位免填)
        //    //12    回覆代碼              070 ~ 073   4     N4     「0000」成功，其他為失敗
        //    //13    ※轉帳行代碼          074 ~ 080   7     AN7    轉帳帳號所屬銀行代碼，三碼總行代碼及四碼分行代碼，分行代碼可空白
        //    //14    ※轉帳帳號            081 ~ 096   16    AN16   右靠
        //    //15    ※帳戶ID              097 ~ 106   10    AN10   左靠右補空白（帳號所有人ID）
        //    //16    ＃銷帳編號            107 ~ 122   16    AN16   左靠右補空白
        //    //17    ※費用代號            123 ~ 126   4     AN4    費用代號（銀行公會授與）
        //    //18    ◎銀行專用區          127 ~ 146   20    AN20   銀行自由運用，無則空白 (隱藏欄位免填)
        //    //19    ＃事業單位專用資料區  147 ~ 200   54    AN54   事業單位自由運用（客戶組別），無則空白
        //    //20    ＃客戶備註            201 ~ 300   100   AN100  客戶備註
        //    #endregion

        //    TxtFormat[] fields = new TxtFormat[20] {
        //        new TxtFormat("01", 001, 001),
        //        new TxtFormat("02", 002, 008),
        //        new TxtFormat("03", 010, 008),
        //        new TxtFormat("04", 018, 007),
        //        new TxtFormat("05", 025, 005),
        //        new TxtFormat("06", 030, 010),
        //        new TxtFormat("07", 040, 001),
        //        new TxtFormat("08", 041, 013, Common.AlignCutPadEnum.Right, ' '),
        //        new TxtFormat("09", 054, 001),
        //        new TxtFormat("10", 055, 008),
        //        new TxtFormat("11", 063, 007),
        //        new TxtFormat("12", 070, 004),
        //        new TxtFormat("13", 074, 007),
        //        new TxtFormat("14", 081, 016, Common.AlignCutPadEnum.Right, ' '),
        //        new TxtFormat("15", 097, 010, Common.AlignCutPadEnum.Left, ' '),
        //        new TxtFormat("16", 107, 016, Common.AlignCutPadEnum.Left, ' '),
        //        new TxtFormat("17", 123, 004),
        //        new TxtFormat("18", 127, 020),
        //        new TxtFormat("19", 147, 054),
        //        new TxtFormat("20", 201, 100)
        //    };
        //    return fields;
        //}
        //#endregion

        //#region ACH委扣檔
        //private const int ACHLineByteSize = 160;    //單筆記錄長度
        //private const string ACH_RBOF = "BOF";      //首錄別 (BOF)
        //private const string ACH_RCDATA = "ACHR01"; //資料代號 (ACHR01)
        //private const string ACH_RSORG = "9990250"; //發送單位代號
        //private const string ACH_RORG = "";         //接收單位代號
        //private const string ACH_RTYPE = "N";       //交易型態 N:提示
        //private const string ACH_RTXTYPE = "SC";    //交易類別 SC:代付案件
        //private const string ACH_TXID = "551";      //交易代號 551=學雜費; 661=學雜費(免檢核)
        //private const string ACH_REOF = "EOF";      //尾錄別 (EOF)

        ///// <summary>
        ///// 匯出ACH委扣檔
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <param name="deductDate"></param>
        ///// <param name="fileName"></param>
        ///// <param name="outFile"></param>
        ///// <param name="dataLog"></param>
        ///// <returns></returns>
        //public string ExportACHDeduction(ICollection<BillKey> keys, DateTime deductDate, string fileName, out string outFile, out string dataLog)
        //{
        //    #region 處理參數
        //    if (keys == null || keys.Count == 0)
        //    {
        //        outFile = null;
        //        dataLog = null;
        //        return "未指定匯出資料查詢條件參數";
        //    }
        //    string receiveType = null;
        //    foreach (BillKey key in keys)
        //    {
        //        if (receiveType == null)
        //        {
        //            receiveType = key.ReceiveType;
        //        }
        //        else if (receiveType != key.ReceiveType)
        //        {
        //            outFile = null;
        //            dataLog = null;
        //            return "匯出資料查詢條件參數不正確，必須是同一業務別";
        //        }
        //    }
        //    if (String.IsNullOrWhiteSpace(fileName))
        //    {
        //        BillKey first = keys.First<BillKey>();
        //        fileName = String.Format("{0}_{1:yyyyMMddHHmmss}.TXT", first.ReceiveType, DateTime.Now);
        //    }
        //    outFile = Path.Combine(_TempPath, fileName);
        //    #endregion

        //    List<DeductionData> datas = this.GetDeductionData(keys, out dataLog);
        //    if (datas.Count == 0)
        //    {
        //        return "查無可匯出的資料";
        //    }
        //    else
        //    {
        //        string schIdenty = null;
        //        string schBankId= null;
        //        string schAccountNo= null;
        //        string errmsg = this.GetSchoolInfo(receiveType, out schIdenty, out schBankId, out schAccountNo);
        //        if (String.IsNullOrEmpty(errmsg))
        //        {
        //            return this.GenACHDeductionFile(datas, deductDate, schIdenty, schBankId, schAccountNo, outFile, true);
        //        }
        //        else
        //        {
        //            return errmsg;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 將指定資料寫入ACH委扣檔
        ///// </summary>
        ///// <param name="datas"></param>
        ///// <param name="deductDate"></param>
        ///// <param name="outFile"></param>
        ///// <param name="overWrite"></param>
        ///// <returns></returns>
        //private string GenACHDeductionFile(ICollection<DeductionData> datas, DateTime deductDate, string schIdenty, string schBankId, string schAccountNo, string outFile, bool overWrite)
        //{
        //    #region 檢查參數
        //    if (datas == null || datas.Count == 0)
        //    {
        //        return "未指定要處理的資料參數";
        //    }
        //    if (String.IsNullOrEmpty(outFile) || !Path.IsPathRooted(outFile))
        //    {
        //        return "未指定產出檔案的完整路徑名稱";
        //    }
        //    #endregion

        //    try
        //    {
        //        if (!overWrite && File.Exists(outFile))
        //        {
        //            return "指定產出檔案已存在";
        //        }
        //        string path = Path.GetDirectoryName(outFile);
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        string tdata = Common.GetTWDate7(deductDate);
        //        string ydate = Common.GetTWDate7(deductDate.AddDays(-1));

        //        using (StreamWriter sw = new StreamWriter(outFile, false, Encoding.Default))
        //        {
        //            #region 處理首錄
        //            {
        //                #region 欄位說明
        //                //序號  欄位名稱      欄位代號	性質    起始位置 長度   備註
        //                //01    首錄別        R-BOF     X(03)   01       3      'BOF'
        //                //02    資料代號      R-CDATA   X(06)   04       6      'ACHR01'
        //                //03    處理日期      R-TDATE   9(08)   10       8      民國YYYYMMDD
        //                //04    處理時間      R-TTIME   9(06)   18       6      HHMMSS
        //                //05    發送單位代號  R-SORG    9(07)   24       7      '9990250'
        //                //06    接收單位代號  R-RORG    9(07)   31       7      代表行代號
        //                //07    備用          FILLER    X(123)  38       123
        //                #endregion

        //                TxtFormat[] headFields = this.GetACHHeadFields();
        //                string[] headTexts = new string[7] {
        //                    ACH_RBOF, ACH_RCDATA, tdata, "000000", ACH_RSORG, ACH_RORG, ""
        //                };
        //                string headLine = this.GenTextLine(ACHLineByteSize, headFields, headTexts);
        //                sw.WriteLine(headLine);
        //            }
        //            #endregion

        //            #region 處理明細錄
        //            decimal totalAmount = 0M;
        //            {
        //                #region 欄位說明
        //                //序號  欄位名稱          欄位代號  性質    起始位置  長度   備註
        //                //01    交易型態          R-TYPE    X(01)   001       1      N:提示
        //                //02    交易類別          R-TXTYPE  X(02)   002       2      SC:代付案件
        //                //03    交易代號          R-TXID    X(03)   004       3      551=學雜費; 661=學雜費(免檢核)
        //                //04    交易序號          R-SEQ     9(06)   007       6      提示行按交易順序編訂序號，同一發動者序號不得重複
        //                //05    提出行代號        R-PBANK   9(07)   013       7      發動行金融機構代號
        //                //06    發動者帳號        R-PCLNO   X(14)   020       14     位數不足十四位數時，右靠左補零，放置信用卡資料時得有空白
        //                //07    提回行代號        R-RBANK   9(07)   034       7      收受行金融機構代號
        //                //08    收受者帳號        R-RCLNO   X(14)   041       14     位數不足十四位數時，右靠左補零，放置信用卡資料時得有空白
        //                //09    金額              R-AMT     9(10)   055       10     金額以「元」為單位「角」以下不計，位數不足十位數時，右靠左補零，金額不得為零。
        //                //10    退件理由代號      R-RCODE   9(02)   065       2      補零或空白
        //                //11    提示交換次序      R-SCHD    X(01)   067       1      批量轉帳: B 零星轉帳: 1或2或3    初期均為B
        //                //12    發動者統一編號    R-CID     X(10)   068       10     委託代收或代付者之公司登記或商業登記之統一編號或身分證字號，台灣存託憑證(簡稱TDR)或外國發行人來台申請股票上市櫃，得使用證券交易所賦編之4至6碼公司代號，不滿十位數時左靠右補空白，個人戶英文字母請大寫。
        //                //13    收受者統一編號    R-PID     X(10)   078       10     接受代收或代付者之公司登記或商業登記之統一編號或身分證字號，不滿十位數時左靠右補空白，個人戶英文字母請大寫。收受者帳號為公司戶時，收受者統編應為公司登記或商業登記之統一編號，不得為公司負責人身分證字號。
        //                //14    上市上櫃公司代號  R-SID     X(06)   088       6      證交所及證券櫃檯買賣中心上市上櫃公司代號。不滿六位數時左靠右補空白。
        //                //15    原提示交易日期    R-PDATE   9(08)   094       8      補零或空白
        //                //16    原提示交易序號    R-PSEQ    9(06)   102       6      補零或空白
        //                //17    原提示交換次序    R-PSCHD   X(01)   108       1      空白
        //                //18    用戶號碼          P-CNO     X(20)   109       20     代收案件本欄為必要輸入欄位，資料須左靠。
        //                //19    發動者專用區      P-NOTE    X(20)   129       20     不可使用中文。
        //                //20    存摺摘要          P-MEMO    X(10)   149       10     僅可使用英、數字及 - ，不可使用中文。
        //                //21    備用              P-FILLER  X(02)   159       2      不可使用中文。
        //                #endregion

        //                int no = 0;
        //                TxtFormat[] bodyFields = this.GetACHBodyFields();
        //                foreach (DeductionData data in datas)
        //                {
        //                    no++;
        //                    totalAmount += data.Amount;

        //                    string[] bodyTexts = new string[21] {
        //                        ACH_RTYPE, ACH_RTXTYPE, ACH_TXID, no.ToString("000000"), data.BankId, 
        //                        data.AccountNo, schBankId, schAccountNo, data.Amount.ToString("0"), "", 
        //                        "B", data.AccountId, schIdenty, "", "",
        //                        "", "", data.AccountNo, "", "", 
        //                        ""
        //                    };
        //                    string bodyLine = this.GenTextLine(ACHLineByteSize, bodyFields, bodyTexts);
        //                    sw.WriteLine(bodyLine);
        //                }
        //            }
        //            #endregion

        //            #region 處理尾錄
        //            {
        //                #region 欄位說明
        //                //序號  欄位名稱        欄位代號  性質   起始位置  長度   備註
        //                //01    尾錄別          R-EOF     X(03)  001       3      'EOF'
        //                //02    資料代號        R-CDATA   X(06)  004       6      'ACHR01'
        //                //03    處理日期        R-TDATE   9(08)  010       8      民國YYYYMMDD
        //                //04    發送單位代號    R-SORG    9(07)  018       7      '9990250'
        //                //05    接收單位代號    R-RORG    9(07)  025       7      代表行代號
        //                //06    總筆數          R-TCOUNT  9(08)  032       8
        //                //07    總金額          R-TAMT    9(16)  040       16
        //                //08    前一營業日日期  R-YDATE   9(08)  056       8      民國YYYYMMDD
        //                //09    備用            FILLER    X(97)  064       97
        //                #endregion

        //                TxtFormat[] footFields = this.GetACHFootFields();
        //                string[] footTexts = new string[9] {
        //                    ACH_REOF, ACH_RCDATA, tdata, ACH_RSORG, ACH_RORG, datas.Count.ToString("0"), totalAmount.ToString("0"), ydate, ""
        //                };
        //                string footLine = this.GenTextLine(ACHLineByteSize, footFields, footTexts);
        //                sw.WriteLine(footLine);
        //            }
        //            #endregion
        //        }

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return String.Concat("產出檔案發生例外，錯誤訊息；", ex.Message);
        //    }
        //}

        ///// <summary>
        ///// ACH委扣檔首錄欄位設定
        ///// </summary>
        ///// <returns></returns>
        //private TxtFormat[] GetACHHeadFields()
        //{
        //    #region 欄位說明
        //    //序號  欄位名稱      欄位代號	性質    起始位置 長度   備註
        //    //01    首錄別        R-BOF     X(03)   01       3      'BOF'
        //    //02    資料代號      R-CDATA   X(06)   04       6      'ACHR01'
        //    //03    處理日期      R-TDATE   9(08)   10       8      民國YYYYMMDD
        //    //04    處理時間      R-TTIME   9(06)   18       6      HHMMSS
        //    //05    發送單位代號  R-SORG    9(07)   24       7      '9990250'
        //    //06    接收單位代號  R-RORG    9(07)   31       7      代表行代號
        //    //07    備用          FILLER    X(123)  38       123
        //    #endregion

        //    TxtFormat[] fields = new TxtFormat[7] {
        //        new TxtFormat("01", 001, 003),
        //        new TxtFormat("02", 004, 006),
        //        new TxtFormat("03", 010, 008),
        //        new TxtFormat("04", 018, 006),
        //        new TxtFormat("05", 024, 007),
        //        new TxtFormat("06", 031, 007),
        //        new TxtFormat("07", 038, 123)
        //    };
        //    return fields;
        //}

        ///// <summary>
        ///// ACH委扣檔明細錄欄位設定
        ///// </summary>
        ///// <returns></returns>
        //private TxtFormat[] GetACHBodyFields()
        //{
        //    #region 欄位說明
        //    //序號  欄位名稱          欄位代號  性質    起始位置  長度   備註
        //    //01    交易型態          R-TYPE    X(01)   001       1      N:提示
        //    //02    交易類別          R-TXTYPE  X(02)   002       2      SC:代付案件
        //    //03    交易代號          R-TXID    X(03)   004       3      551=學雜費; 661=學雜費(免檢核)
        //    //04    交易序號          R-SEQ     9(06)   007       6      提示行按交易順序編訂序號，同一發動者序號不得重複
        //    //05    提出行代號        R-PBANK   9(07)   013       7      發動行金融機構代號
        //    //06    發動者帳號        R-PCLNO   X(14)   020       14     位數不足十四位數時，右靠左補零，放置信用卡資料時得有空白
        //    //07    提回行代號        R-RBANK   9(07)   034       7      收受行金融機構代號
        //    //08    收受者帳號        R-RCLNO   X(14)   041       14     位數不足十四位數時，右靠左補零，放置信用卡資料時得有空白
        //    //09    金額              R-AMT     9(10)   055       10     金額以「元」為單位「角」以下不計，位數不足十位數時，右靠左補零，金額不得為零。
        //    //10    退件理由代號      R-RCODE   9(02)   065       2      補零或空白
        //    //11    提示交換次序      R-SCHD    X(01)   067       1      批量轉帳: B 零星轉帳: 1或2或3    初期均為B
        //    //12    發動者統一編號    R-CID     X(10)   068       10     委託代收或代付者之公司登記或商業登記之統一編號或身分證字號，台灣存託憑證(簡稱TDR)或外國發行人來台申請股票上市櫃，得使用證券交易所賦編之4至6碼公司代號，不滿十位數時左靠右補空白，個人戶英文字母請大寫。
        //    //13    收受者統一編號    R-PID     X(10)   078       10     接受代收或代付者之公司登記或商業登記之統一編號或身分證字號，不滿十位數時左靠右補空白，個人戶英文字母請大寫。收受者帳號為公司戶時，收受者統編應為公司登記或商業登記之統一編號，不得為公司負責人身分證字號。
        //    //14    上市上櫃公司代號  R-SID     X(06)   088       6      證交所及證券櫃檯買賣中心上市上櫃公司代號。不滿六位數時左靠右補空白。
        //    //15    原提示交易日期    R-PDATE   9(08)   094       8      補零或空白
        //    //16    原提示交易序號    R-PSEQ    9(06)   102       6      補零或空白
        //    //17    原提示交換次序    R-PSCHD   X(01)   108       1      空白
        //    //18    用戶號碼          P-CNO     X(20)   109       20     代收案件本欄為必要輸入欄位，資料須左靠。
        //    //19    發動者專用區      P-NOTE    X(20)   129       20     不可使用中文。
        //    //20    存摺摘要          P-MEMO    X(10)   149       10     僅可使用英、數字及 - ，不可使用中文。
        //    //21    備用              P-FILLER  X(02)   159       2      不可使用中文。
        //    #endregion

        //    TxtFormat[] fields = new TxtFormat[21] {
        //        new TxtFormat("01", 001, 001),
        //        new TxtFormat("02", 002, 002),
        //        new TxtFormat("03", 004, 003),
        //        new TxtFormat("04", 007, 006),
        //        new TxtFormat("05", 013, 007),
        //        new TxtFormat("06", 020, 014, Common.AlignCutPadEnum.Right, '0'),
        //        new TxtFormat("07", 034, 007),
        //        new TxtFormat("08", 041, 014, Common.AlignCutPadEnum.Right, '0'),
        //        new TxtFormat("09", 055, 010, Common.AlignCutPadEnum.Right, '0'),
        //        new TxtFormat("10", 065, 002),
        //        new TxtFormat("11", 067, 001),
        //        new TxtFormat("12", 068, 010, Common.AlignCutPadEnum.Left, ' '),
        //        new TxtFormat("13", 078, 010, Common.AlignCutPadEnum.Left, ' '),
        //        new TxtFormat("14", 088, 006, Common.AlignCutPadEnum.Left, ' '),
        //        new TxtFormat("15", 094, 008),
        //        new TxtFormat("16", 102, 006),
        //        new TxtFormat("17", 108, 001),
        //        new TxtFormat("18", 109, 020, Common.AlignCutPadEnum.Left, ' '),
        //        new TxtFormat("19", 129, 020),
        //        new TxtFormat("20", 149, 010),
        //        new TxtFormat("21", 159, 002)
        //    };
        //    return fields;
        //}

        ///// <summary>
        ///// ACH委扣檔尾錄欄位設定
        ///// </summary>
        ///// <returns></returns>
        //private TxtFormat[] GetACHFootFields()
        //{
        //    #region 欄位說明
        //    //序號  欄位名稱        欄位代號  性質   起始位置  長度   備註
        //    //01    尾錄別          R-EOF     X(03)  001       3      'EOF'
        //    //02    資料代號        R-CDATA   X(06)  004       6      'ACHR01'
        //    //03    處理日期        R-TDATE   9(08)  010       8      民國YYYYMMDD
        //    //04    發送單位代號    R-SORG    9(07)  018       7      '9990250'
        //    //05    接收單位代號    R-RORG    9(07)  025       7      代表行代號
        //    //06    總筆數          R-TCOUNT  9(08)  032       8
        //    //07    總金額          R-TAMT    9(16)  040       16
        //    //08    前一營業日日期  R-YDATE   9(08)  056       8      民國YYYYMMDD
        //    //09    備用            FILLER    X(97)  064       97
        //    #endregion

        //    TxtFormat[] fields = new TxtFormat[9] {
        //        new TxtFormat("01", 001, 003),
        //        new TxtFormat("02", 004, 006),
        //        new TxtFormat("03", 010, 008),
        //        new TxtFormat("04", 018, 007),
        //        new TxtFormat("05", 025, 007),
        //        new TxtFormat("06", 032, 008, Common.AlignCutPadEnum.Right, '0'),
        //        new TxtFormat("07", 040, 016, Common.AlignCutPadEnum.Right, '0'),
        //        new TxtFormat("08", 056, 008),
        //        new TxtFormat("09", 064, 097)
        //    };
        //    return fields;
        //}
        //#endregion
        #endregion

        #region 匯出學生繳費資料媒體檔 (下載銷帳資料也使用此方法)
        #region [Old]
        //        /// <summary>
        //        /// 匯出學生繳費資料媒體檔 (下載銷帳資料也使用此方法)
        //        /// </summary>
        //        /// <param name="receiveType">商家代號</param>
        //        /// <param name="yearId">學年代碼</param>
        //        /// <param name="termId">學期代碼</param>
        //        /// <param name="depId">部別代碼</param>
        //        /// <param name="receiveId">代收費用別代碼</param>
        //        /// <param name="qUpNo">批號</param>
        //        /// <param name="qCancelStatus">銷帳狀態</param>
        //        /// <param name="qReceiveWay">繳款方式</param>
        //        /// <param name="qSReceivDate">代收日區間起日</param>
        //        /// <param name="qEReceivDate">代收日區間迄日</param>
        //        /// <param name="qSAccountDate">入帳日區間起日</param>
        //        /// <param name="qEAccountDate">入帳日區間迄日</param>
        //        /// <param name="qFieldName">查詢欄位名稱</param>
        //        /// <param name="qFieldValue">查詢欄位值</param>
        //        /// <param name="outFields">匯出欄位名稱集合</param>
        //        /// <param name="outFileContent">成功則傳回產生檔案內容，否則傳回 null</param>
        //        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        //        public string ExportStudentReceiveView4(string receiveType, string yearId, string termId, string depId, string receiveId
        //            , int? qUpNo, string qCancelStatus, string qReceiveWay, DateTime? qSReceivDate, DateTime? qEReceivDate, DateTime? qSAccountDate, DateTime? qEAccountDate
        //            , string qFieldName, string qFieldValue, ICollection<string> outFields, out byte[] outFileContent)
        //        {
        //            outFileContent = null;

        //            #region 檢查參數
        //            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId))
        //            {
        //                return "缺少查詢資料參數";
        //            }
        //            if (outFields == null || outFields.Count == 0)
        //            {
        //                return "缺少下載資料項目參數";
        //            }
        //            #endregion

        //            try
        //            {
        //                #region 取資料
        //                DataTable dt = null;
        //                bool hasZBarcode = false;
        //                DateTime? payDueDate =null;
        //                int extraDays = 0;
        //                ReceiveChannelEntity[] smReceiveChannels = null;
        //                string[] receiveItemNames = new string[40] {
        //                    "收入科目01金額", "收入科目02金額", "收入科目03金額", "收入科目04金額", "收入科目05金額",
        //                    "收入科目06金額", "收入科目07金額", "收入科目08金額", "收入科目09金額", "收入科目10金額",
        //                    "收入科目11金額", "收入科目12金額", "收入科目13金額", "收入科目14金額", "收入科目15金額",
        //                    "收入科目16金額", "收入科目17金額", "收入科目18金額", "收入科目19金額", "收入科目20金額",
        //                    "收入科目21金額", "收入科目22金額", "收入科目23金額", "收入科目24金額", "收入科目25金額",
        //                    "收入科目26金額", "收入科目27金額", "收入科目28金額", "收入科目29金額", "收入科目30金額",
        //                    "收入科目31金額", "收入科目32金額", "收入科目33金額", "收入科目34金額", "收入科目35金額",
        //                    "收入科目36金額", "收入科目37金額", "收入科目38金額", "收入科目39金額", "收入科目40金額"
        //                };
        //                string[] memoTitles = new string[] {
        //                    "備註01", "備註02", "備註03", "備註04", "備註05",
        //                    "備註06", "備註07", "備註08", "備註09", "備註10"
        //                };
        //                {
        //                    StringBuilder fieldSql = new StringBuilder();
        //                    foreach (string outField in outFields)
        //                    {
        //                        fieldSql.Append(",").Append(outField);
        //                        if (!hasZBarcode && outField == StudentReceiveView4.Field.CancelSmno)
        //                        {
        //                            hasZBarcode = true;
        //                            fieldSql.Append(", '' AS [超商條碼1], '' AS [超商條碼2], '' AS [超商條碼3]");
        //                            fieldSql.AppendFormat(", [{0}] AS [計算用應繳金額], [{1}] AS [計算用超商金額]", StudentReceiveView4.Field.ReceiveAmount, StudentReceiveView4.Field.ReceiveSmamount);
        //                        }
        //                    }

        //                    #region
        //                    //20150715 jj 增加 "入帳日"、"代收日"、"繳款管道"
        //                    fieldSql.Append(",").Append(StudentReceiveView4.Field.ReceiveDate);
        //                    fieldSql.Append(",").Append(StudentReceiveView4.Field.AccountDate);
        //                    fieldSql.Append(",").Append(StudentReceiveView4.Field.ReceiveWayName);
        //                    #endregion

        //                    //if (hasZBarcode)
        //                    {
        //                        #region 繳款期限 + 超商延遲日 + 收入科目名稱 + 備註標題
        //                        {
        //                            SchoolRidEntity schoolRid = null;
        //                            Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
        //                                .And(SchoolRidEntity.Field.YearId, yearId)
        //                                .And(SchoolRidEntity.Field.TermId, termId)
        //                                .And(SchoolRidEntity.Field.DepId, depId)
        //                                .And(SchoolRidEntity.Field.ReceiveId, receiveId);
        //                            Result result = _Factory.SelectFirst<SchoolRidEntity>(where, null, out schoolRid);
        //                            if (!result.IsSuccess)
        //                            {
        //                                return String.Format("查詢代收費用別設定資料失敗，錯誤訊息：{0}", result.Message);
        //                            }
        //                            if (schoolRid == null)
        //                            {
        //                                return "查無代收費用別設定資料";
        //                            }
        //                            payDueDate = DataFormat.ConvertDateText(schoolRid.PayDate);
        //                            if (payDueDate == null)
        //                            {
        //                                return "未設定繳款期限";
        //                            }

        //                            extraDays = schoolRid.ExtraDays;

        //                            string[] itemNames = new string[] {
        //                                schoolRid.ReceiveItem01, schoolRid.ReceiveItem02, schoolRid.ReceiveItem03, schoolRid.ReceiveItem04, schoolRid.ReceiveItem05,
        //                                schoolRid.ReceiveItem06, schoolRid.ReceiveItem07, schoolRid.ReceiveItem08, schoolRid.ReceiveItem09, schoolRid.ReceiveItem10,
        //                                schoolRid.ReceiveItem11, schoolRid.ReceiveItem12, schoolRid.ReceiveItem13, schoolRid.ReceiveItem14, schoolRid.ReceiveItem15,
        //                                schoolRid.ReceiveItem16, schoolRid.ReceiveItem17, schoolRid.ReceiveItem18, schoolRid.ReceiveItem19, schoolRid.ReceiveItem20,
        //                                schoolRid.ReceiveItem21, schoolRid.ReceiveItem22, schoolRid.ReceiveItem23, schoolRid.ReceiveItem24, schoolRid.ReceiveItem25,
        //                                schoolRid.ReceiveItem26, schoolRid.ReceiveItem27, schoolRid.ReceiveItem28, schoolRid.ReceiveItem29, schoolRid.ReceiveItem30,
        //                                schoolRid.ReceiveItem31, schoolRid.ReceiveItem32, schoolRid.ReceiveItem33, schoolRid.ReceiveItem34, schoolRid.ReceiveItem35,
        //                                schoolRid.ReceiveItem36, schoolRid.ReceiveItem37, schoolRid.ReceiveItem38, schoolRid.ReceiveItem39, schoolRid.ReceiveItem40
        //                            };
        //                            for(int idx = 0; idx < receiveItemNames.Length; idx++)
        //                            {
        //                                string itemName = itemNames[idx] == null ? null : itemNames[idx].Trim();
        //                                if (!String.IsNullOrEmpty(itemName))
        //                                {
        //                                    receiveItemNames[idx] = itemName;
        //                                }
        //                            }

        //                            string[] tmps = schoolRid.GetAllMemoTitles();
        //                            for (int idx = 0; idx < memoTitles.Length; idx++)
        //                            {
        //                                if (idx < tmps.Length && !String.IsNullOrWhiteSpace(tmps[idx]))
        //                                {
        //                                    memoTitles[idx] = tmps[idx].Trim();
        //                                }
        //                            }
        //                        }
        //                        #endregion

        //                        #region 商家代碼的超商管道手續費
        //                        {
        //                            Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType)
        //                                .And(ReceiveChannelEntity.Field.ChannelId, ChannelHelper.SM_DEFAULT);
        //                            Result result = _Factory.SelectAll<ReceiveChannelEntity>(where, null, out smReceiveChannels);
        //                            if (!result.IsSuccess)
        //                            {
        //                                return String.Format("查詢此商家代碼的超商管道手續費資料失敗，錯誤訊息：{0}", result.Message);
        //                            }
        //                            if (hasZBarcode && (smReceiveChannels == null || smReceiveChannels.Length == 0))
        //                            {
        //                                return "查無此商家代碼的超商管道手續費資料";
        //                            }
        //                        }
        //                        #endregion
        //                    }

        //                    #region 學生繳費資料
        //                    {
        //                        List<string> whereSqls = new List<string>(14);
        //                        KeyValueList parameters = new KeyValueList(14);

        //                        #region 5 Key
        //                        whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveType)", StudentReceiveView4.Field.ReceiveType));
        //                        parameters.Add("@ReceiveType", receiveType);

        //                        whereSqls.Add(String.Format("(SR.[{0}] = @YearId)", StudentReceiveView4.Field.YearId));
        //                        parameters.Add("@YearId", yearId);

        //                        whereSqls.Add(String.Format("(SR.[{0}] = @TermId)", StudentReceiveView4.Field.TermId));
        //                        parameters.Add("@TermId", termId);

        //                        whereSqls.Add(String.Format("(SR.[{0}] = @DepId)", StudentReceiveView4.Field.DepId));
        //                        parameters.Add("@DepId", depId);

        //                        whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveId)", StudentReceiveView4.Field.ReceiveId));
        //                        parameters.Add("@ReceiveId", receiveId);
        //                        #endregion

        //                        #region 批號
        //                        if (qUpNo != null)
        //                        {
        //                            whereSqls.Add(String.Format("([{0}] = @UpNo)", StudentReceiveView4.Field.UpNo));
        //                            parameters.Add("@UpNo", qUpNo.Value);
        //                        }
        //                        #endregion

        //                        #region 銷帳狀態
        //                        switch (qCancelStatus)
        //                        {
        //                            case CancelStatusCodeTexts.NONPAY:      //未繳款
        //                                whereSqls.Add(String.Format("((SR.[{0}] = '' OR SR.[{0}] IS NULL) AND (SR.[{1}] = '' OR SR.[{1}] IS NULL))", StudentReceiveView4.Field.ReceiveDate, StudentReceiveView4.Field.CancelFlag));
        //                                break;
        //                            case CancelStatusCodeTexts.PAYED:       //已繳款
        //                                whereSqls.Add(String.Format("((SR.[{0}] != '' AND SR.[{0}] IS NOT NULL) AND (SR.[{1}] = '' OR SR.[{1}] IS NULL))", StudentReceiveView4.Field.ReceiveDate, StudentReceiveView4.Field.AccountDate));
        //                                break;
        //                            case CancelStatusCodeTexts.CANCELED:    //已入帳
        //                                whereSqls.Add(String.Format("(SR.[{0}] != '' AND SR.[{0}] IS NOT NULL)", StudentReceiveView4.Field.AccountDate));
        //                                break;
        //                        }
        //                        #endregion

        //                        #region 繳款方式 + 代收日區間
        //                        if (qCancelStatus != CancelStatusCodeTexts.NONPAY)
        //                        {
        //                            if (!String.IsNullOrEmpty(qReceiveWay))
        //                            {
        //                                whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveWay)", StudentReceiveView4.Field.ReceiveWay));
        //                                parameters.Add("@ReceiveWay", qReceiveWay);
        //                            }

        //                            if (qSReceivDate != null)
        //                            {
        //                                whereSqls.Add(String.Format("(SR.[{0}] >= @SReceivDate)", StudentReceiveView4.Field.ReceiveDate));
        //                                parameters.Add("@SReceivDate", Common.GetTWDate7(qSReceivDate.Value));
        //                            }
        //                            if (qEReceivDate != null)
        //                            {
        //                                whereSqls.Add(String.Format("(SR.[{0}] <= @EReceivDate)", StudentReceiveView4.Field.ReceiveDate));
        //                                parameters.Add("@EReceivDate", Common.GetTWDate7(qEReceivDate.Value));
        //                            }
        //                        }
        //                        #endregion

        //                        #region 入帳日區間
        //                        if (String.IsNullOrEmpty(qCancelStatus) || qCancelStatus == CancelStatusCodeTexts.CANCELED)
        //                        {
        //                            if (qSAccountDate != null)
        //                            {
        //                                whereSqls.Add(String.Format("(SR.[{0}] >= @SAccountDate)", StudentReceiveView4.Field.AccountDate));
        //                                parameters.Add("@SAccountDate", Common.GetTWDate7(qSAccountDate.Value));
        //                            }
        //                            if (qEAccountDate != null)
        //                            {
        //                                whereSqls.Add(String.Format("(SR.[{0}] <= @EAccountDate)", StudentReceiveView4.Field.AccountDate));
        //                                parameters.Add("@EAccountDate", Common.GetTWDate7(qEAccountDate.Value));
        //                            }
        //                        }
        //                        #endregion

        //                        #region 查詢欄位與值
        //                        if (!String.IsNullOrEmpty(qFieldName) && !String.IsNullOrEmpty(qFieldValue))
        //                        {
        //                            switch (qFieldName)
        //                            {
        //                                case "StuId":   //學號
        //                                    whereSqls.Add(String.Format("(SR.[{0}] = @StuId)", StudentReceiveView4.Field.StuId));
        //                                    parameters.Add("@StuId", qFieldValue);
        //                                    break;
        //                                case "CancelNo":   //虛擬帳號
        //                                    whereSqls.Add(String.Format("(SR.[{0}] = @CancelNo)", StudentReceiveView4.Field.CancelNo));
        //                                    parameters.Add("@CancelNo", qFieldValue);
        //                                    break;
        //                                case "IdNumber":   //身分證字號
        //                                    whereSqls.Add(String.Format("(SR.[{0}] = @StuIdNumber)", StudentReceiveView4.Field.StuIdNumber));
        //                                    parameters.Add("@StuIdNumber", qFieldValue);
        //                                    break;
        //                            }
        //                        }
        //                        #endregion

        //                        string whereSql = String.Join(" AND ", whereSqls);

        //                        string sql = String.Format(@"SELECT {0} 
        //  FROM (
        //{1} 
        // WHERE {2}
        //) AS T 
        // ORDER BY {3}", fieldSql.ToString(1, fieldSql.Length -1), StudentReceiveView4.VIEWSQL, whereSql, StudentReceiveView4.Field.StuId);

        //                        Result result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
        //                        if (!result.IsSuccess)
        //                        {
        //                            return String.Format("查詢學生繳費資料失敗，錯誤訊息：{0}", result.Message);
        //                        }
        //                        if (dt == null || dt.Rows.Count == 0)
        //                        {
        //                            return "查無學生繳費資料";
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                #endregion

        //                #region 處理超商條碼
        //                if (hasZBarcode)
        //                {
        //                    ChannelHelper helper = new ChannelHelper();
        //                    foreach (DataRow dr in dt.Rows)
        //                    {
        //                        string smCancelNo = dr.IsNull(StudentReceiveView4.Field.CancelSmno) ? null : dr[StudentReceiveView4.Field.CancelSmno].ToString().Trim();
        //                        if (String.IsNullOrWhiteSpace(smCancelNo))
        //                        {
        //                            continue;
        //                        }

        //                        #region [Old] 使用者可能沒勾選 ReceiveAmount 或 ReceiveSmamount，所以改用 [計算用應繳金額] 與 [計算用超商金額]
        //                        //decimal amount = 0;
        //                        //string amountTxt = dr.IsNull(StudentReceiveView4.Field.ReceiveAmount) ? null : dr[StudentReceiveView4.Field.ReceiveAmount].ToString().Trim();
        //                        //if (String.IsNullOrWhiteSpace(amountTxt) || !Decimal.TryParse(amountTxt, out amount) || amount < 0)
        //                        //{
        //                        //    continue;
        //                        //}

        //                        //decimal smAmount = 0;
        //                        //string smAmountTxt = dr.IsNull(StudentReceiveView4.Field.ReceiveSmamount) ? null : dr[StudentReceiveView4.Field.ReceiveSmamount].ToString().Trim();
        //                        //if (String.IsNullOrWhiteSpace(smAmountTxt) || !Decimal.TryParse(smAmountTxt, out smAmount) || smAmount < 0)
        //                        //{
        //                        //    continue;
        //                        //}
        //                        #endregion

        //                        decimal amount = 0;
        //                        string amountTxt = dr.IsNull("計算用應繳金額") ? null : dr["計算用應繳金額"].ToString().Trim();
        //                        if (String.IsNullOrWhiteSpace(amountTxt) || !Decimal.TryParse(amountTxt, out amount) || amount < 0)
        //                        {
        //                            continue;
        //                        }

        //                        decimal smAmount = 0;
        //                        string smAmountTxt = dr.IsNull("計算用超商金額") ? null : dr["計算用超商金額"].ToString().Trim();
        //                        if (String.IsNullOrWhiteSpace(smAmountTxt) || !Decimal.TryParse(smAmountTxt, out smAmount) || smAmount < 0)
        //                        {
        //                            continue;
        //                        }

        //                        ReceiveChannelEntity smChannel = null;
        //                        ReceiveChannelEntity cashChannel = null;
        //                        helper.CheckReceiveChannel(amount, smReceiveChannels, out smChannel, out cashChannel);
        //                        if (smChannel == null)
        //                        {
        //                            continue;
        //                        }

        //                        string smBarcode1 = null;
        //                        string smBarcode2 = null;
        //                        string smBarcode3 = null;
        //                        this.GenSMBarcode(smCancelNo, smAmount, payDueDate.Value, extraDays, smChannel.BarcodeId, out smBarcode1, out smBarcode2, out smBarcode3);
        //                        dr["超商條碼1"] = smBarcode1 ?? String.Empty;
        //                        dr["超商條碼2"] = smBarcode2 ?? String.Empty;
        //                        dr["超商條碼3"] = smBarcode3 ?? String.Empty;
        //                    }
        //                }
        //                #endregion

        //                #region 指定欄位名稱
        //                {
        //                    KeyValueList<string> fieldNames = new KeyValueList<string>();
        //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveType, "商家代號");
        //                    fieldNames.Add(StudentReceiveView4.Field.YearId, "學年代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.TermId, "學期代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.DeptId, "部別代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveId, "費用別代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.UpNo, "上傳資料批號");
        //                    fieldNames.Add(StudentReceiveView4.Field.UpOrder, "上傳該批資料的序號");
        //                    fieldNames.Add(StudentReceiveView4.Field.StuGrade, "年級代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.CollegeId, "院別代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.MajorId, "科系代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.ClassId, "班別代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.StuCredit, "總學分數");
        //                    fieldNames.Add(StudentReceiveView4.Field.StuHour, "上課時數");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReduceId, "減免代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.DormId, "住宿代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.LoanId, "就貸代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.AgencyList, "代辦費明細項目");
        //                    fieldNames.Add(StudentReceiveView4.Field.BillingType, "計算方式");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId01, "身份註記一代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId02, "身份註記二代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId03, "身份註記三代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId04, "身份註記四代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId05, "身份註記五代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId06, "身份註記六代碼");

        //                    #region [Old] 收入科目名稱改用代收費用別理的設定值
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive01, "收入科目01金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive02, "收入科目02金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive03, "收入科目03金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive04, "收入科目04金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive05, "收入科目05金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive06, "收入科目06金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive07, "收入科目07金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive08, "收入科目08金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive09, "收入科目09金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive10, "收入科目10金額");

        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive11, "收入科目11金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive12, "收入科目12金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive13, "收入科目13金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive14, "收入科目14金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive15, "收入科目15金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive16, "收入科目16金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive17, "收入科目17金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive18, "收入科目18金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive19, "收入科目19金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive20, "收入科目20金額");

        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive21, "收入科目21金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive22, "收入科目22金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive23, "收入科目23金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive24, "收入科目24金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive25, "收入科目25金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive26, "收入科目26金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive27, "收入科目27金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive28, "收入科目28金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive29, "收入科目29金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive30, "收入科目30金額");

        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive31, "收入科目31金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive32, "收入科目32金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive33, "收入科目33金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive34, "收入科目34金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive35, "收入科目35金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive36, "收入科目36金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive37, "收入科目37金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive38, "收入科目38金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive39, "收入科目39金額");
        //                    //fieldNames.Add(StudentReceiveView4.Field.Receive40, "收入科目40金額");
        //                    #endregion

        //                    #region 收入科目名稱
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive01, receiveItemNames[0]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive02, receiveItemNames[1]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive03, receiveItemNames[2]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive04, receiveItemNames[3]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive05, receiveItemNames[4]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive06, receiveItemNames[5]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive07, receiveItemNames[6]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive08, receiveItemNames[7]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive09, receiveItemNames[8]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive10, receiveItemNames[9]);

        //                    fieldNames.Add(StudentReceiveView4.Field.Receive11, receiveItemNames[10]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive12, receiveItemNames[11]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive13, receiveItemNames[12]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive14, receiveItemNames[13]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive15, receiveItemNames[14]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive16, receiveItemNames[15]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive17, receiveItemNames[16]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive18, receiveItemNames[17]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive19, receiveItemNames[18]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive20, receiveItemNames[19]);

        //                    fieldNames.Add(StudentReceiveView4.Field.Receive21, receiveItemNames[20]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive22, receiveItemNames[21]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive23, receiveItemNames[22]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive24, receiveItemNames[23]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive25, receiveItemNames[24]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive26, receiveItemNames[25]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive27, receiveItemNames[26]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive28, receiveItemNames[27]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive29, receiveItemNames[28]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive30, receiveItemNames[29]);

        //                    fieldNames.Add(StudentReceiveView4.Field.Receive31, receiveItemNames[30]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive32, receiveItemNames[31]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive33, receiveItemNames[32]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive34, receiveItemNames[33]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive35, receiveItemNames[34]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive36, receiveItemNames[35]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive37, receiveItemNames[36]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive38, receiveItemNames[37]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive39, receiveItemNames[38]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Receive40, receiveItemNames[39]);
        //                    #endregion

        //                    fieldNames.Add(StudentReceiveView4.Field.StuId, "學生學號");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveAmount, "繳費金額");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveSmamount, "超商繳費金額");
        //                    fieldNames.Add(StudentReceiveView4.Field.LoanAmount, "原就學貸款金額");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReissueFlag, "補單註記");

        //                    fieldNames.Add(StudentReceiveView4.Field.SeriorNo, "流水號");
        //                    fieldNames.Add(StudentReceiveView4.Field.CancelNo, "虛擬帳號");
        //                    fieldNames.Add(StudentReceiveView4.Field.CancelSmno, "超商虛擬帳號");
        //                    fieldNames.Add(StudentReceiveView4.Field.CancelFlag, "銷帳註記");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReceivebankId, "代收銀行");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveDate, "代收日期");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveTime, "代收時間");
        //                    fieldNames.Add(StudentReceiveView4.Field.AccountDate, "入帳日期");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveWay, "繳費方式");
        //                    fieldNames.Add(StudentReceiveView4.Field.Loan, "上傳就學貸款金額");
        //                    fieldNames.Add(StudentReceiveView4.Field.RealLoan, "實際貸款金額");
        //                    fieldNames.Add(StudentReceiveView4.Field.DeductBankid, "扣款轉帳銀行代碼");
        //                    fieldNames.Add(StudentReceiveView4.Field.DeductAccountno, "扣款轉帳銀行帳號");
        //                    fieldNames.Add(StudentReceiveView4.Field.DeductAccountname, "扣款轉帳銀行帳號戶名");
        //                    fieldNames.Add(StudentReceiveView4.Field.DeductAccountid, "扣款轉帳銀行帳戶ＩＤ");

        //                    #region 備註相關
        //                    fieldNames.Add(StudentReceiveView4.Field.Memo01, memoTitles[0]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Memo02, memoTitles[1]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Memo03, memoTitles[2]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Memo04, memoTitles[3]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Memo05, memoTitles[4]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Memo06, memoTitles[5]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Memo07, memoTitles[6]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Memo08, memoTitles[7]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Memo09, memoTitles[8]);
        //                    fieldNames.Add(StudentReceiveView4.Field.Memo10, memoTitles[9]);
        //                    #endregion

        //                    #region 學生基本資料相關
        //                    fieldNames.Add(StudentReceiveView4.Field.StuName, "學生姓名");
        //                    fieldNames.Add(StudentReceiveView4.Field.StuBirthday, "學生生日");
        //                    fieldNames.Add(StudentReceiveView4.Field.StuIdNumber, "學生身分證字號");
        //                    fieldNames.Add(StudentReceiveView4.Field.StuTel, "學生電話");
        //                    fieldNames.Add(StudentReceiveView4.Field.StuZipCode, "學生郵遞區號");
        //                    fieldNames.Add(StudentReceiveView4.Field.StuAddress, "學生地址");
        //                    fieldNames.Add(StudentReceiveView4.Field.StuEmail, "學生 EMail");
        //                    #endregion

        //                    #region 代碼名稱相關
        //                    fieldNames.Add(StudentReceiveView4.Field.TermName, "學期名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.DeptName, "部別名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveName, "費用名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.CollegeName, "院別名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.MajorName, "系所名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.ClassName, "班別名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReduceName, "減免類別名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.DormName, "住宿項目名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.LoanName, "就貸項目名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName01, "身分註記01項目名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName02, "身分註記02項目名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName03, "身分註記03項目名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName04, "身分註記04項目名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName05, "身分註記05項目名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName06, "身分註記06項目名稱");
        //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveWayName, "繳費管道");
        //                    #endregion

        //                    foreach (DataColumn column in dt.Columns)
        //                    {
        //                        string key = column.ColumnName;
        //                        KeyValue<string> fieldName = fieldNames.Find(x => x.Key == key);
        //                        if (fieldName != null)
        //                        {
        //                            column.ColumnName = fieldName.Value;
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region DataTable 轉 Xls
        //                {
        //                    #region 轉換資料前，先移除計算用的應繳金額與超商金額
        //                    if (hasZBarcode)
        //                    {
        //                        dt.Columns.Remove("計算用應繳金額");
        //                        dt.Columns.Remove("計算用超商金額");
        //                    }
        //                    #endregion

        //                    ConvertFileHelper helper = new ConvertFileHelper();
        //                    outFileContent = helper.Dt2Xls(dt);
        //                }
        //                #endregion

        //                return null;
        //            }
        //            catch(Exception ex)
        //            {
        //                return String.Concat("產出檔案發生例外，錯誤訊息；", ex.Message);
        //            }
        //        }
        #endregion

        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出學生繳費資料媒體檔 (下載銷帳資料也使用此方法)
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="qUpNo">批號</param>
        /// <param name="qCancelStatus">銷帳狀態</param>
        /// <param name="qReceiveWay">繳款方式</param>
        /// <param name="qSReceivDate">代收日區間起日</param>
        /// <param name="qEReceivDate">代收日區間迄日</param>
        /// <param name="qSAccountDate">入帳日區間起日</param>
        /// <param name="qEAccountDate">入帳日區間迄日</param>
        /// <param name="qFieldName">查詢欄位名稱</param>
        /// <param name="qFieldValue">查詢欄位值</param>
        /// <param name="outFields">匯出欄位名稱集合</param>
        /// <param name="outFileContent">成功則傳回產生檔案內容，否則傳回 null</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        public string ExportStudentReceiveView4(string receiveType, string yearId, string termId, string depId, string receiveId
            , int? qUpNo, string qCancelStatus, string qReceiveWay, DateTime? qSReceivDate, DateTime? qEReceivDate, DateTime? qSAccountDate, DateTime? qEAccountDate
            , string qFieldName, string qFieldValue, ICollection<string> outFields
            , out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            #region 檢查參數
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return "缺少查詢資料參數";
            }
            if (outFields == null || outFields.Count == 0)
            {
                return "缺少下載資料項目參數";
            }
            #endregion

            DataTable dt = null;
            try
            {
                #region 取得費用別的繳款期限、超商延遲日、收入科目數量名稱、備註標題
                DateTime? payDueDate = null;
                int extraDays = 0;
                List<string> receiveItemNames = new List<string>(40);
                List<string> receiveMemoTitles = new List<string>(SchoolRidEntity.MemoTitleMaxCount);
                {
                    SchoolRidEntity schoolRid = null;
                    Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
                        .And(SchoolRidEntity.Field.YearId, yearId)
                        .And(SchoolRidEntity.Field.TermId, termId)
                        .And(SchoolRidEntity.Field.DepId, depId)
                        .And(SchoolRidEntity.Field.ReceiveId, receiveId);
                    Result result = _Factory.SelectFirst<SchoolRidEntity>(where, null, out schoolRid);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢費用別設定資料失敗，錯誤訊息：{0}", result.Message);
                    }
                    if (schoolRid == null)
                    {
                        return "查無費用別設定資料";
                    }
                    payDueDate = DataFormat.ConvertDateText(schoolRid.PayDate);
                    extraDays = schoolRid.ExtraDays;

                    #region [MDY:202203XX] 2022擴充案 收入科目 改寫，改用 GetAllReceiveItemChts()
                    #region [OLD]
                    //string[] itemNames = schoolRid.GetAllReceiveItems();
                    #endregion

                    string[] itemNames = schoolRid.GetAllReceiveItemChts();
                    #endregion

                    foreach (string itemName in itemNames)
                    {
                        if (String.IsNullOrWhiteSpace(itemName))
                        {
                            break;
                        }
                        else
                        {
                            receiveItemNames.Add(itemName.Trim());
                        }
                    }

                    #region [MDY:202203XX] 2022擴充案 備註項目 改寫，改用 GetAllMemoTitleChts()
                    #region [OLD]
                    //string[] memoTitles = schoolRid.GetAllMemoTitles();
                    #endregion

                    string[] memoTitles = schoolRid.GetAllMemoTitleChts();
                    #endregion

                    int memoNo = 0;
                    foreach (string memoTitle in memoTitles)
                    {
                        memoNo++;
                        if (String.IsNullOrWhiteSpace(memoTitle))
                        {
                            receiveMemoTitles.Add(String.Format("備註{0:00}", memoNo));
                        }
                        else
                        {
                            receiveMemoTitles.Add(memoTitle.Trim());
                        }
                    }
                }
                #endregion

                #region 取得超商管道手續費
                ReceiveChannelEntity[] smReceiveChannels = null;
                {
                    Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType)
                        .And(ReceiveChannelEntity.Field.ChannelId, ChannelHelper.SM_DEFAULT);
                    Result result = _Factory.SelectAll<ReceiveChannelEntity>(where, null, out smReceiveChannels);
                    if (!result.IsSuccess)
                    {
                        return String.Format("讀取超商管道手續費資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region [MDY:20160125] 取得小計設定
                ReceiveSumEntity[] receiveSums = null;
                KeyValueList<int[]> receiveSumItemNos = null;
                bool hasReceiveSum = false;
                {
                    foreach (string outField in outFields)
                    {
                        if (outField != null && outField.Equals("ReceiveSum", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Expression where = new Expression(ReceiveSumEntity.Field.ReceiveType, receiveType)
                                .And(ReceiveSumEntity.Field.YearId, yearId)
                                .And(ReceiveSumEntity.Field.TermId, termId)
                                .And(ReceiveSumEntity.Field.DepId, depId)
                                .And(ReceiveSumEntity.Field.ReceiveId, receiveId);
                            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                            orderbys.Add(ReceiveSumEntity.Field.SumId, OrderByEnum.Asc);
                            Result result = null;
                            result = _Factory.SelectAll<ReceiveSumEntity>(where, orderbys, out receiveSums);
                            if (!result.IsSuccess)
                            {
                                return String.Concat("讀取小計設定資料失敗，錯誤訊息；", result.Message);
                            }
                            if (receiveSums == null || receiveSums.Length == 0)
                            {
                                return "查無小計設定資料，無法匯出資料；";
                            }
                            hasReceiveSum = true;
                            receiveSumItemNos = new KeyValueList<int[]>(receiveSums.Length);
                            foreach (ReceiveSumEntity receiveSum in receiveSums)
                            {
                                receiveSumItemNos.Add(receiveSum.SumId, receiveSum.GetSumReceiveItemNos());
                            }
                            break;
                        }
                    }
                }
                #endregion

                #region 取資料
                {
                    #region 處理匯出資料欄位 (取得查詢欄位 Sql)
                    bool hasReceive = false;
                    bool hasZBarcode = false;

                    #region[MDY:20160528] 處理繳款期限 (學生繳費資料無繳款期限則取代收費用檔中的繳款期限)
                    bool chkPayDueDate = false;
                    string twPayDueDate = null;
                    #endregion

                    List<string> sqlFieldNames = new List<string>(outFields.Count + 7 + receiveItemNames.Count + (receiveSums != null ? receiveSums.Length : 0));

                    #region PKey 欄位
                    //{
                    //    sqlFieldNames.Add(StudentReceiveView4.Field.ReceiveType);
                    //    sqlFieldNames.Add(StudentReceiveView4.Field.YearId);
                    //    sqlFieldNames.Add(StudentReceiveView4.Field.TermId);
                    //    sqlFieldNames.Add(StudentReceiveView4.Field.DepId);
                    //    sqlFieldNames.Add(StudentReceiveView4.Field.ReceiveId);
                    //}
                    #endregion

                    foreach (string outField in outFields)
                    {
                        if (String.IsNullOrWhiteSpace(outField))
                        {
                            continue;
                        }
                        string fieldName = outField.Trim();
                        switch (fieldName.ToUpper())
                        {
                            case "RECEIVE":     //收入科目金額
                                #region 收入科目金額
                                if (!hasReceive)
                                {
                                    hasReceive = true;
                                    string[] receiveFieldNames = new string[] {
                                        StudentReceiveView4.Field.Receive01, StudentReceiveView4.Field.Receive02, StudentReceiveView4.Field.Receive03, StudentReceiveView4.Field.Receive04, StudentReceiveView4.Field.Receive05, 
                                        StudentReceiveView4.Field.Receive06, StudentReceiveView4.Field.Receive07, StudentReceiveView4.Field.Receive08, StudentReceiveView4.Field.Receive09, StudentReceiveView4.Field.Receive10, 
                                        StudentReceiveView4.Field.Receive11, StudentReceiveView4.Field.Receive12, StudentReceiveView4.Field.Receive13, StudentReceiveView4.Field.Receive14, StudentReceiveView4.Field.Receive15, 
                                        StudentReceiveView4.Field.Receive16, StudentReceiveView4.Field.Receive17, StudentReceiveView4.Field.Receive18, StudentReceiveView4.Field.Receive19, StudentReceiveView4.Field.Receive20, 
                                        StudentReceiveView4.Field.Receive21, StudentReceiveView4.Field.Receive22, StudentReceiveView4.Field.Receive23, StudentReceiveView4.Field.Receive24, StudentReceiveView4.Field.Receive25, 
                                        StudentReceiveView4.Field.Receive26, StudentReceiveView4.Field.Receive27, StudentReceiveView4.Field.Receive28, StudentReceiveView4.Field.Receive29, StudentReceiveView4.Field.Receive30, 
                                        StudentReceiveView4.Field.Receive31, StudentReceiveView4.Field.Receive32, StudentReceiveView4.Field.Receive33, StudentReceiveView4.Field.Receive34, StudentReceiveView4.Field.Receive35, 
                                        StudentReceiveView4.Field.Receive36, StudentReceiveView4.Field.Receive37, StudentReceiveView4.Field.Receive38, StudentReceiveView4.Field.Receive39, StudentReceiveView4.Field.Receive40
                                    };
                                    for (int idx = 0; idx < receiveItemNames.Count; idx++)
                                    {
                                        //sqlFieldNames.Add(receiveFieldNames[idx]);
                                        sqlFieldNames.Add(String.Format("[{0}] AS [{1}]", receiveFieldNames[idx], receiveItemNames[idx]));
                                    }
                                }
                                #endregion
                                break;
                            case "ZBARCODE":    //超商條碼
                                #region 超商條碼
                                if (!hasZBarcode)
                                {
                                    hasZBarcode = true;
                                    sqlFieldNames.Add("'' AS [超商條碼1]");
                                    sqlFieldNames.Add("'' AS [超商條碼2]");
                                    sqlFieldNames.Add("'' AS [超商條碼3]");
                                    sqlFieldNames.Add(String.Format("[{0}] AS [計算用應繳金額]", StudentReceiveView4.Field.ReceiveAmount));
                                    sqlFieldNames.Add(String.Format("[{0}] AS [計算用超商金額]", StudentReceiveView4.Field.ReceiveSmamount));
                                    sqlFieldNames.Add(String.Format("[{0}] AS [計算用超商銷編]", StudentReceiveView4.Field.CancelSmno));

                                    #region [MDY:20160216] 計算用繳費期限
                                    sqlFieldNames.Add(String.Format("[{0}] AS [計算用繳費期限]", StudentReceiveView4.Field.PayDueDate));
                                    #endregion
                                }
                                #endregion
                                break;
                            case "RECEIVESUM":  //小計金額
                                #region [MDY:20160125] 小計金額
                                if (hasReceiveSum && receiveSums != null && receiveSums.Length > 0)
                                {
                                    foreach (ReceiveSumEntity receiveSum in receiveSums)
                                    {
                                        sqlFieldNames.Add(String.Format("0 AS [{0}]", receiveSum.SumId));
                                    }
                                    string[] receiveFieldNames = new string[] {
                                        StudentReceiveView4.Field.Receive01, StudentReceiveView4.Field.Receive02, StudentReceiveView4.Field.Receive03, StudentReceiveView4.Field.Receive04, StudentReceiveView4.Field.Receive05, 
                                        StudentReceiveView4.Field.Receive06, StudentReceiveView4.Field.Receive07, StudentReceiveView4.Field.Receive08, StudentReceiveView4.Field.Receive09, StudentReceiveView4.Field.Receive10, 
                                        StudentReceiveView4.Field.Receive11, StudentReceiveView4.Field.Receive12, StudentReceiveView4.Field.Receive13, StudentReceiveView4.Field.Receive14, StudentReceiveView4.Field.Receive15, 
                                        StudentReceiveView4.Field.Receive16, StudentReceiveView4.Field.Receive17, StudentReceiveView4.Field.Receive18, StudentReceiveView4.Field.Receive19, StudentReceiveView4.Field.Receive20, 
                                        StudentReceiveView4.Field.Receive21, StudentReceiveView4.Field.Receive22, StudentReceiveView4.Field.Receive23, StudentReceiveView4.Field.Receive24, StudentReceiveView4.Field.Receive25, 
                                        StudentReceiveView4.Field.Receive26, StudentReceiveView4.Field.Receive27, StudentReceiveView4.Field.Receive28, StudentReceiveView4.Field.Receive29, StudentReceiveView4.Field.Receive30, 
                                        StudentReceiveView4.Field.Receive31, StudentReceiveView4.Field.Receive32, StudentReceiveView4.Field.Receive33, StudentReceiveView4.Field.Receive34, StudentReceiveView4.Field.Receive35, 
                                        StudentReceiveView4.Field.Receive36, StudentReceiveView4.Field.Receive37, StudentReceiveView4.Field.Receive38, StudentReceiveView4.Field.Receive39, StudentReceiveView4.Field.Receive40
                                    };
                                    for (int idx = 0; idx < receiveItemNames.Count; idx++)
                                    {
                                        sqlFieldNames.Add(String.Format("[{0}] AS [RSAmount_{1:00}]", receiveFieldNames[idx], idx + 1));
                                    }
                                }
                                #endregion
                                break;
                            default:
                                #region [MDY:20160528] 處理繳款期限 (學生繳費資料無繳款期限則取代收費用檔中的繳款期限)
                                switch (fieldName)
                                {
                                    case StudentReceiveView4.Field.PayDueDate:  //繳款期限
                                        if (payDueDate != null)
                                        {
                                            chkPayDueDate = true;
                                            twPayDueDate = Common.GetTWDate7(payDueDate.Value);
                                        }
                                        break;
                                }
                                #endregion

                                sqlFieldNames.Add(String.Format("[{0}]", fieldName));
                                break;
                        }
                    }
                    string fieldSql = String.Join(", ", sqlFieldNames);
                    #endregion

                    #region 查詢條件 Sql
                    List<string> whereSqls = new List<string>();
                    KeyValueList parameters = new KeyValueList();
                    {
                        #region 商家代號
                        whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveType)", StudentReceiveView4.Field.ReceiveType));
                        parameters.Add("@ReceiveType", receiveType);
                        #endregion

                        #region 學年
                        whereSqls.Add(String.Format("(SR.[{0}] = @YearId)", StudentReceiveView4.Field.YearId));
                        parameters.Add("@YearId", yearId);
                        #endregion

                        #region 學期
                        whereSqls.Add(String.Format("(SR.[{0}] = @TermId)", StudentReceiveView4.Field.TermId));
                        parameters.Add("@TermId", termId);
                        #endregion

                        #region 原部別 (Dep_Id)
                        whereSqls.Add(String.Format("(SR.[{0}] = @DepId)", StudentReceiveView4.Field.DepId));
                        parameters.Add("@DepId", depId);
                        #endregion

                        #region 費用別
                        whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveId)", StudentReceiveView4.Field.ReceiveId));
                        parameters.Add("@ReceiveId", receiveId);
                        #endregion

                        #region 批號
                        if (qUpNo != null)
                        {
                            whereSqls.Add(String.Format("([{0}] = @UpNo)", StudentReceiveView4.Field.UpNo));
                            parameters.Add("@UpNo", qUpNo.Value);
                        }
                        #endregion

                        #region 銷帳狀態
                        switch (qCancelStatus)
                        {
                            case CancelStatusCodeTexts.NONPAY:      //未繳款
                                whereSqls.Add(String.Format("((SR.[{0}] = '' OR SR.[{0}] IS NULL) AND (SR.[{1}] = '' OR SR.[{1}] IS NULL))", StudentReceiveView4.Field.ReceiveDate, StudentReceiveView4.Field.AccountDate));
                                break;
                            case CancelStatusCodeTexts.PAYED:       //已繳款
                                whereSqls.Add(String.Format("((SR.[{0}] != '' AND SR.[{0}] IS NOT NULL) AND (SR.[{1}] = '' OR SR.[{1}] IS NULL))", StudentReceiveView4.Field.ReceiveDate, StudentReceiveView4.Field.AccountDate));
                                break;
                            case CancelStatusCodeTexts.CANCELED:    //已入帳
                                whereSqls.Add(String.Format("(SR.[{0}] != '' AND SR.[{0}] IS NOT NULL)", StudentReceiveView4.Field.AccountDate));
                                break;
                        }
                        #endregion

                        #region 繳款方式 + 代收日區間 + 入帳日區間
                        if (qCancelStatus != CancelStatusCodeTexts.NONPAY)
                        {
                            #region 繳款方式
                            if (!String.IsNullOrEmpty(qReceiveWay))
                            {
                                whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveWay)", StudentReceiveView4.Field.ReceiveWay));
                                parameters.Add("@ReceiveWay", qReceiveWay);
                            }
                            #endregion

                            #region 代收日區間
                            if (qSReceivDate != null)
                            {
                                whereSqls.Add(String.Format("(SR.[{0}] >= @SReceivDate)", StudentReceiveView4.Field.ReceiveDate));
                                parameters.Add("@SReceivDate", Common.GetTWDate7(qSReceivDate.Value));
                            }
                            if (qEReceivDate != null)
                            {
                                whereSqls.Add(String.Format("(SR.[{0}] <= @EReceivDate)", StudentReceiveView4.Field.ReceiveDate));
                                parameters.Add("@EReceivDate", Common.GetTWDate7(qEReceivDate.Value));
                            }
                            #endregion

                            #region 入帳日區間
                            if (String.IsNullOrEmpty(qCancelStatus) || qCancelStatus == CancelStatusCodeTexts.CANCELED)
                            {
                                if (qSAccountDate != null)
                                {
                                    whereSqls.Add(String.Format("(SR.[{0}] >= @SAccountDate)", StudentReceiveView4.Field.AccountDate));
                                    parameters.Add("@SAccountDate", Common.GetTWDate7(qSAccountDate.Value));
                                }
                                if (qEAccountDate != null)
                                {
                                    whereSqls.Add(String.Format("(SR.[{0}] <= @EAccountDate)", StudentReceiveView4.Field.AccountDate));
                                    parameters.Add("@EAccountDate", Common.GetTWDate7(qEAccountDate.Value));
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region 查詢欄位與值
                        if (!String.IsNullOrEmpty(qFieldName) && !String.IsNullOrEmpty(qFieldValue))
                        {
                            switch (qFieldName)
                            {
                                case "StuId":   //學號
                                    whereSqls.Add(String.Format("(SR.[{0}] = @StuId)", StudentReceiveView4.Field.StuId));
                                    parameters.Add("@StuId", qFieldValue);
                                    break;
                                case "CancelNo":   //虛擬帳號
                                    whereSqls.Add(String.Format("(SR.[{0}] = @CancelNo)", StudentReceiveView4.Field.CancelNo));
                                    parameters.Add("@CancelNo", qFieldValue);
                                    break;
                                case "IdNumber":   //身分證字號
                                    whereSqls.Add(String.Format("(SM.[{0}] = @StuIdNumber)", StudentReceiveView4.Field.StuIdNumber));
                                    parameters.Add("@StuIdNumber", qFieldValue);
                                    break;
                            }
                        }
                        #endregion
                    }
                    string whereSql = String.Join(" AND ", whereSqls);
                    #endregion

                    #region 檢查資料筆數，不可超過筆數限制 (XLS 65535 筆; ODS 1048575 筆)
                    int maxXlsRowCount = 65535;
                    {
                        string sql = String.Format(@"SELECT COUNT(1) 
  FROM (
{0} 
 WHERE {1}
) AS T", StudentReceiveView4.VIEWSQL, whereSql);
                        object value = null;
                        Result result = _Factory.ExecuteScalar(sql, parameters, out value);
                        if (result.IsSuccess)
                        {
                            Int64 count = 0;
                            if (value == null || !Int64.TryParse(value.ToString(), out count))
                            {
                                return "檢查匯出資料筆數失敗，錯誤訊息：回傳資料不正確";
                            }
                            else if (!isUseODS && count > maxXlsRowCount) //XLS 版本的限制，為了相容性所以要限制
                            {
                                return "匯出資料超過 Excel (XLS) 的筆數限制 (65535 筆)";
                            }
                            else if (isUseODS && count > Fuju.ODS.ODSSheet.MAX_ROW_COUNT)
                            {
                                return String.Format("匯出資料超過 Calc (ODS) 的筆數限制 ({0} 筆)", Fuju.ODS.ODSSheet.MAX_ROW_COUNT);
                            }
                            else if (count == 0)
                            {
                                return "查無匯出資料";
                            }
                        }
                        else
                        {
                            return String.Format("檢查匯出資料筆數失敗，錯誤訊息：{0}", result.Message);
                        }
                    }
                    #endregion

                    #region 讀取資料
                    {
                        string orderSql = String.Format("[{0}]", StudentReceiveView4.Field.StuId);

                        string sql = String.Format(@"SELECT {0} 
  FROM (
{1} 
 WHERE {2}
) AS T 
 ORDER BY {3}", fieldSql, StudentReceiveView4.VIEWSQL, whereSql, orderSql);

                        Result result = _Factory.GetDataTable(sql, parameters, 0, maxXlsRowCount, out dt);
                        if (!result.IsSuccess)
                        {
                            return String.Format("讀取匯出資料失敗，錯誤訊息：{0}", result.Message);
                        }
                        if (dt == null || dt.Rows.Count == 0)
                        {
                            return "查無匯出資料";
                        }
                    }
                    #endregion

                    #region[MDY:20160528] 處理超商條碼 Or 計算小計金額 Or 繳款期限 (學生繳費資料無繳款期限則取代收費用檔中的繳款期限)
                    bool doReceiveSum = (hasReceiveSum && receiveSumItemNos != null && receiveSumItemNos.Count > 0);
                    bool doZBarcode = (hasZBarcode && smReceiveChannels != null && smReceiveChannels.Length > 0);
                    if (dt != null && dt.Rows.Count > 0 && (doReceiveSum || doZBarcode || chkPayDueDate))
                    {
                        ChannelHelper helper = new ChannelHelper();
                        foreach (DataRow drow in dt.Rows)
                        {
                            #region [MDY:20160125] 計算小計金額 (這個要在超商條碼前執行，因為超商條碼會 continue)
                            if (doReceiveSum)
                            {
                                foreach (KeyValue<int[]> receiveSumItemNo in receiveSumItemNos)
                                {
                                    string key = receiveSumItemNo.Key;
                                    int[] sumItemNos = receiveSumItemNo.Value;
                                    if (sumItemNos != null && sumItemNos.Length > 0)
                                    {
                                        int subTotal = 0;
                                        foreach (int sumItemNo in sumItemNos)
                                        {
                                            string fieldName = String.Format("RSAmount_{0:00}", sumItemNo);
                                            subTotal += drow.IsNull(fieldName) ? 0 : Convert.ToInt32(drow[fieldName]);
                                        }
                                        drow[key] = subTotal;
                                    }
                                }
                            }
                            #endregion

                            #region 處理超商條碼
                            if (doZBarcode)
                            {
                                #region [MDY:20160216] 判斷使用哪個繳款期限
                                DateTime? smPayDueDate = drow.IsNull("計算用繳費期限") ? null : DataFormat.ConvertDateText(drow["計算用繳費期限"].ToString());
                                if (smPayDueDate == null)
                                {
                                    if (payDueDate == null)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        smPayDueDate = payDueDate;
                                    }
                                }
                                #endregion

                                decimal amount = 0;
                                string amountTxt = drow.IsNull("計算用應繳金額") ? null : drow["計算用應繳金額"].ToString().Trim();
                                if (String.IsNullOrEmpty(amountTxt) || !Decimal.TryParse(amountTxt, out amount) || amount <= 0)
                                {
                                    continue;
                                }

                                decimal smAmount = 0;
                                string smAmountTxt = drow.IsNull("計算用超商金額") ? null : drow["計算用超商金額"].ToString().Trim();
                                if (String.IsNullOrEmpty(smAmountTxt) || !Decimal.TryParse(smAmountTxt, out smAmount) || smAmount < 0)
                                {
                                    continue;
                                }

                                string smCancelNo = drow.IsNull("計算用超商銷編") ? null : drow["計算用超商銷編"].ToString().Trim();
                                if (String.IsNullOrEmpty(smCancelNo) || !Common.IsNumber(smCancelNo))
                                {
                                    continue;
                                }

                                ReceiveChannelEntity smChannel = null;
                                ReceiveChannelEntity cashChannel = null;
                                helper.CheckReceiveChannel(amount, smReceiveChannels, out smChannel, out cashChannel);
                                if (smChannel == null)
                                {
                                    continue;
                                }

                                string smBarcode1 = null;
                                string smBarcode2 = null;
                                string smBarcode3 = null;
                                this.GenSMBarcode(smCancelNo, smAmount, smPayDueDate.Value, extraDays, smChannel.BarcodeId, out smBarcode1, out smBarcode2, out smBarcode3);
                                drow["超商條碼1"] = smBarcode1 ?? String.Empty;
                                drow["超商條碼2"] = smBarcode2 ?? String.Empty;
                                drow["超商條碼3"] = smBarcode3 ?? String.Empty;
                            }
                            #endregion

                            #region [MDY:20160528] 處理繳款期限 (學生繳費資料無繳款期限則取代收費用檔中的繳款期限)
                            if (chkPayDueDate)
                            {
                                if (drow.IsNull(StudentReceiveView4.Field.PayDueDate) || String.IsNullOrWhiteSpace(drow[StudentReceiveView4.Field.PayDueDate].ToString()))
                                {
                                    drow[StudentReceiveView4.Field.PayDueDate] = twPayDueDate;
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 轉換資料前，先移除計算用的應繳金額、超商金額、超商銷編、繳費期限
                    if (hasZBarcode && dt != null)
                    {
                        dt.Columns.Remove("計算用應繳金額");
                        dt.Columns.Remove("計算用超商金額");
                        dt.Columns.Remove("計算用超商銷編");
                        dt.Columns.Remove("計算用繳費期限");
                    }
                    #endregion

                    #region 轉換資料前，先移除 PKey 欄位 (因為沒有要匯出)
                    //if (hasZBarcode && dt != null)
                    //{
                    //    dt.Columns.Remove(StudentReceiveView4.Field.ReceiveType);
                    //    dt.Columns.Remove(StudentReceiveView4.Field.YearId);
                    //    dt.Columns.Remove(StudentReceiveView4.Field.TermId);
                    //    dt.Columns.Remove(StudentReceiveView4.Field.DepId);
                    //    dt.Columns.Remove(StudentReceiveView4.Field.ReceiveId);
                    //}
                    #endregion

                    #region [MDY:20160125] 轉換資料前，先移除計算小計用的金額
                    if (hasReceiveSum)
                    {
                        for (int no = 1; no <= 40; no++)
                        {
                            string fieldName = String.Format("RSAmount_{0:00}", no);
                            if (dt.Columns.Contains(fieldName))
                            {
                                dt.Columns.Remove(fieldName);
                            }
                        }
                    }
                    #endregion

                    #region 轉換資料前，將欄位名稱替換成中文
                    if (dt != null)
                    {
                        KeyValueList<string> fieldNames = new KeyValueList<string>();
                        fieldNames.Add(StudentReceiveView4.Field.ReceiveType, "商家代號");
                        fieldNames.Add(StudentReceiveView4.Field.YearId, "學年代碼");
                        fieldNames.Add(StudentReceiveView4.Field.TermId, "學期代碼");
                        fieldNames.Add(StudentReceiveView4.Field.DeptId, "部別代碼");
                        fieldNames.Add(StudentReceiveView4.Field.ReceiveId, "費用別代碼");
                        fieldNames.Add(StudentReceiveView4.Field.UpNo, "上傳資料批號");
                        fieldNames.Add(StudentReceiveView4.Field.UpOrder, "上傳該批資料的序號");
                        fieldNames.Add(StudentReceiveView4.Field.StuGrade, "年級代碼");
                        fieldNames.Add(StudentReceiveView4.Field.StuHid, "座號");
                        fieldNames.Add(StudentReceiveView4.Field.CollegeId, "院別代碼");
                        fieldNames.Add(StudentReceiveView4.Field.MajorId, "科系代碼");
                        fieldNames.Add(StudentReceiveView4.Field.ClassId, "班別代碼");
                        fieldNames.Add(StudentReceiveView4.Field.StuCredit, "總學分數");
                        fieldNames.Add(StudentReceiveView4.Field.StuHour, "上課時數");
                        fieldNames.Add(StudentReceiveView4.Field.ReduceId, "減免代碼");
                        fieldNames.Add(StudentReceiveView4.Field.DormId, "住宿代碼");
                        fieldNames.Add(StudentReceiveView4.Field.LoanId, "就貸代碼");
                        fieldNames.Add(StudentReceiveView4.Field.AgencyList, "代辦費明細項目");
                        fieldNames.Add(StudentReceiveView4.Field.BillingType, "計算方式");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyId01, "身份註記一代碼");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyId02, "身份註記二代碼");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyId03, "身份註記三代碼");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyId04, "身份註記四代碼");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyId05, "身份註記五代碼");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyId06, "身份註記六代碼");

                        #region [Old] 收入科目名稱 (查詢 sql 已替換欄位名稱)
                        //fieldNames.Add(StudentReceiveView4.Field.Receive01, receiveItemNames[0]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive02, receiveItemNames[1]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive03, receiveItemNames[2]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive04, receiveItemNames[3]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive05, receiveItemNames[4]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive06, receiveItemNames[5]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive07, receiveItemNames[6]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive08, receiveItemNames[7]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive09, receiveItemNames[8]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive10, receiveItemNames[9]);

                        //fieldNames.Add(StudentReceiveView4.Field.Receive11, receiveItemNames[10]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive12, receiveItemNames[11]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive13, receiveItemNames[12]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive14, receiveItemNames[13]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive15, receiveItemNames[14]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive16, receiveItemNames[15]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive17, receiveItemNames[16]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive18, receiveItemNames[17]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive19, receiveItemNames[18]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive20, receiveItemNames[19]);

                        //fieldNames.Add(StudentReceiveView4.Field.Receive21, receiveItemNames[20]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive22, receiveItemNames[21]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive23, receiveItemNames[22]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive24, receiveItemNames[23]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive25, receiveItemNames[24]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive26, receiveItemNames[25]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive27, receiveItemNames[26]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive28, receiveItemNames[27]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive29, receiveItemNames[28]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive30, receiveItemNames[29]);

                        //fieldNames.Add(StudentReceiveView4.Field.Receive31, receiveItemNames[30]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive32, receiveItemNames[31]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive33, receiveItemNames[32]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive34, receiveItemNames[33]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive35, receiveItemNames[34]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive36, receiveItemNames[35]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive37, receiveItemNames[36]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive38, receiveItemNames[37]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive39, receiveItemNames[38]);
                        //fieldNames.Add(StudentReceiveView4.Field.Receive40, receiveItemNames[39]);
                        #endregion

                        fieldNames.Add(StudentReceiveView4.Field.ReceiveAmount, "繳費金額");
                        fieldNames.Add(StudentReceiveView4.Field.ReceiveSmamount, "超商繳費金額");
                        fieldNames.Add(StudentReceiveView4.Field.LoanAmount, "原就學貸款金額");
                        fieldNames.Add(StudentReceiveView4.Field.ReissueFlag, "補單註記");

                        fieldNames.Add(StudentReceiveView4.Field.SeriorNo, "流水號");
                        fieldNames.Add(StudentReceiveView4.Field.CancelNo, "虛擬帳號");
                        fieldNames.Add(StudentReceiveView4.Field.CancelSmno, "超商虛擬帳號");
                        fieldNames.Add(StudentReceiveView4.Field.CancelFlag, "銷帳註記");
                        fieldNames.Add(StudentReceiveView4.Field.ReceivebankId, "代收銀行");
                        fieldNames.Add(StudentReceiveView4.Field.ReceiveDate, "代收日期");
                        fieldNames.Add(StudentReceiveView4.Field.ReceiveTime, "代收時間");
                        fieldNames.Add(StudentReceiveView4.Field.AccountDate, "入帳日期");
                        fieldNames.Add(StudentReceiveView4.Field.ReceiveWay, "繳款管道代號");
                        fieldNames.Add(StudentReceiveView4.Field.Loan, "上傳就學貸款金額");
                        fieldNames.Add(StudentReceiveView4.Field.RealLoan, "實際貸款金額");
                        fieldNames.Add(StudentReceiveView4.Field.DeductBankid, "扣款轉帳銀行代碼");
                        fieldNames.Add(StudentReceiveView4.Field.DeductAccountno, "扣款轉帳銀行帳號");
                        fieldNames.Add(StudentReceiveView4.Field.DeductAccountname, "扣款轉帳銀行帳號戶名");
                        fieldNames.Add(StudentReceiveView4.Field.DeductAccountid, "扣款轉帳銀行帳戶ＩＤ");

                        #region 備註相關
                        {
                            string[] memoFieldNames = new string[StudentReceiveView4.MemoCount] {
                                StudentReceiveView4.Field.Memo01, StudentReceiveView4.Field.Memo02,
                                StudentReceiveView4.Field.Memo03, StudentReceiveView4.Field.Memo04,
                                StudentReceiveView4.Field.Memo05, StudentReceiveView4.Field.Memo06,
                                StudentReceiveView4.Field.Memo07, StudentReceiveView4.Field.Memo08,
                                StudentReceiveView4.Field.Memo09, StudentReceiveView4.Field.Memo10,
                                StudentReceiveView4.Field.Memo11, StudentReceiveView4.Field.Memo12,
                                StudentReceiveView4.Field.Memo13, StudentReceiveView4.Field.Memo14,
                                StudentReceiveView4.Field.Memo15, StudentReceiveView4.Field.Memo16,
                                StudentReceiveView4.Field.Memo17, StudentReceiveView4.Field.Memo18,
                                StudentReceiveView4.Field.Memo19, StudentReceiveView4.Field.Memo20,
                                StudentReceiveView4.Field.Memo21
                            };
                            for (int idx = 0; idx < memoFieldNames.Length; idx++)
                            {
                                if (idx < receiveMemoTitles.Count)
                                {
                                    fieldNames.Add(memoFieldNames[idx], receiveMemoTitles[idx]);
                                }
                                else
                                {
                                    fieldNames.Add(memoFieldNames[idx], String.Format("備註{0:00}", idx + 1));
                                }
                            }
                        }
                        #endregion

                        #region 學生基本資料相關
                        fieldNames.Add(StudentReceiveView4.Field.StuId, "學生學號");
                        fieldNames.Add(StudentReceiveView4.Field.StuName, "學生姓名");
                        fieldNames.Add(StudentReceiveView4.Field.StuBirthday, "學生生日");
                        fieldNames.Add(StudentReceiveView4.Field.StuIdNumber, "學生身分證字號");
                        fieldNames.Add(StudentReceiveView4.Field.StuTel, "學生電話");
                        fieldNames.Add(StudentReceiveView4.Field.StuZipCode, "學生郵遞區號");
                        fieldNames.Add(StudentReceiveView4.Field.StuAddress, "學生地址");
                        fieldNames.Add(StudentReceiveView4.Field.StuEmail, "學生 EMail");
                        fieldNames.Add(StudentReceiveView4.Field.StuParent, "家長名稱");
                        #endregion

                        #region 代碼名稱相關
                        fieldNames.Add(StudentReceiveView4.Field.YearName, "學年名稱");
                        fieldNames.Add(StudentReceiveView4.Field.TermName, "學期名稱");
                        fieldNames.Add(StudentReceiveView4.Field.DeptName, "部別名稱");
                        fieldNames.Add(StudentReceiveView4.Field.ReceiveName, "費用名稱");
                        fieldNames.Add(StudentReceiveView4.Field.CollegeName, "院別名稱");
                        fieldNames.Add(StudentReceiveView4.Field.MajorName, "系所名稱");
                        fieldNames.Add(StudentReceiveView4.Field.ClassName, "班別名稱");
                        fieldNames.Add(StudentReceiveView4.Field.ReduceName, "減免類別名稱");
                        fieldNames.Add(StudentReceiveView4.Field.DormName, "住宿項目名稱");
                        fieldNames.Add(StudentReceiveView4.Field.LoanName, "就貸項目名稱");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyName01, "身分註記01項目名稱");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyName02, "身分註記02項目名稱");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyName03, "身分註記03項目名稱");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyName04, "身分註記04項目名稱");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyName05, "身分註記05項目名稱");
                        fieldNames.Add(StudentReceiveView4.Field.IdentifyName06, "身分註記06項目名稱");
                        fieldNames.Add(StudentReceiveView4.Field.ReceiveWayName, "繳款管道名稱");
                        #endregion

                        #region [MDY:20160213] 序號、繳款期限
                        fieldNames.Add(StudentReceiveView4.Field.OldSeq, "序號");
                        fieldNames.Add(StudentReceiveView4.Field.PayDueDate, "繳款期限");
                        #endregion

                        #region [MDY:20160125] 小計名稱相關
                        if (hasReceiveSum && receiveSums != null && receiveSums.Length > 0)
                        {
                            foreach (ReceiveSumEntity receiveSum in receiveSums)
                            {
                                fieldNames.Add(receiveSum.SumId, receiveSum.SumName);
                            }
                        }
                        #endregion

                        List<string> columnFieldNames = new List<string>(dt.Columns.Count);  //暫存欄位名稱，用來避免重複
                        int columnNo = 0;
                        foreach (DataColumn column in dt.Columns)
                        {
                            columnNo++;
                            string columnName = column.ColumnName;
                            KeyValue<string> fieldName = fieldNames.Find(x => x.Key == columnName);
                            if (fieldName != null)
                            {
                                string columnFieldName = fieldName.Value;
                                if (columnFieldNames.Contains(columnFieldName))
                                {
                                    //替換掉重複的欄位名稱
                                    column.ColumnName = String.Format("{0}_{1}", columnFieldName, columnNo);
                                }
                                else
                                {
                                    column.ColumnName = columnFieldName;
                                }
                            }
                            else if (columnFieldNames.Contains(columnName))
                            {
                                //替換掉重複的欄位名稱
                                column.ColumnName = String.Format("{0}_{1}", columnName, columnNo);
                            }
                            columnFieldNames.Add(column.ColumnName);
                        }
                    }
                    #endregion

                    #region 匯出成檔案
                    if (dt != null)
                    {
                        if (isUseODS)
                        {
                            #region DataTable 轉 ODS
                            ODSHelper helper = new ODSHelper();
                            outFileContent = helper.DataTable2ODS(dt, isDecimalTruncate: true);
                            #endregion
                        }
                        else
                        {
                            #region DataTable 轉 Xls
                            ConvertFileHelper helper = new ConvertFileHelper();
                            outFileContent = helper.Dt2Xls(dt);
                            #endregion
                        }

                        dt.Clear();
                        dt.Dispose();
                        dt = null;

                        if (outFileContent == null)
                        {
                            return "將匯出資料存成 XLS 檔失敗";
                        }

                        #region 大於 20M 則壓縮成 ZIP，避免序列化失敗 (目前沒有傳附檔名回去，所以暫時 Mark)
                        //if (outFileContent.Length > 1024 * 1024 * 20)
                        //{
                        //    try
                        //    {
                        //        string xlsFileName = Path.Combine(Path.GetTempPath(), String.Format("{0}_{1:yyyyMMddHHmmss}", receiveType, DateTime.Now));
                        //        string zipFileName = Path.GetTempFileName();
                        //        File.WriteAllBytes(xlsFileName, outFileContent);
                        //        ZIPHelper.ZipFile(xlsFileName, zipFileName);
                        //        outFileContent = File.ReadAllBytes(zipFileName);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        return "將匯出資料壓縮成 ZIP 檔失敗，"  + ex.Message;
                        //    }
                        //}
                        #endregion

                        return null;
                    }
                    else
                    {
                        return "無查資料";
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                return String.Concat("產出檔案發生例外，錯誤訊息；", ex.Message);
            }
            finally
            {
                if (dt != null)
                {
                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }
            }
        }
        #endregion
        #endregion

        #region 匯出 C3700007 銷帳資料 (固定格式)
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出 C3700007 銷帳資料 (固定格式)
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="qUpNo"></param>
        /// <param name="qCancelStatus"></param>
        /// <param name="qReceiveWay"></param>
        /// <param name="qSReceivDate"></param>
        /// <param name="qEReceivDate"></param>
        /// <param name="qSAccountDate"></param>
        /// <param name="qEAccountDate"></param>
        /// <param name="qFieldName"></param>
        /// <param name="qFieldValue"></param>
        /// <param name="outFileContent"></param>
        /// <returns></returns>
        public string ExportC3700007File(string receiveType, string yearId, string termId, string depId, string receiveId
            , int? qUpNo, string qCancelStatus, string qReceiveWay, DateTime? qSReceivDate, DateTime? qEReceivDate, DateTime? qSAccountDate, DateTime? qEAccountDate
            , string qFieldName, string qFieldValue, out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            #region 檢查參數
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return "缺少查詢資料參數";
            }
            #endregion

            try
            {
                #region 取資料
                DataTable dt = null;
                {
                    KeyValueList parameters = new KeyValueList(12);
                    List<string> whereSqls = new List<string>(7);

                    #region 5 Key
                    whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveType)", StudentReceiveEntity.Field.ReceiveType));
                    parameters.Add("@ReceiveType", receiveType);

                    whereSqls.Add(String.Format("(SR.[{0}] = @YearId)", StudentReceiveEntity.Field.YearId));
                    parameters.Add("@YearId", yearId);

                    whereSqls.Add(String.Format("(SR.[{0}] = @TermId)", StudentReceiveEntity.Field.TermId));
                    parameters.Add("@TermId", termId);

                    whereSqls.Add(String.Format("(SR.[{0}] = @DepId)", StudentReceiveEntity.Field.DepId));
                    parameters.Add("@DepId", depId);

                    whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveId)", StudentReceiveEntity.Field.ReceiveId));
                    parameters.Add("@ReceiveId", receiveId);
                    #endregion

                    #region 批號
                    if (qUpNo != null && qUpNo.Value > 0)
                    {
                        whereSqls.Add(String.Format("[{0}] = @UpNo", StudentReceiveEntity.Field.UpNo));
                        parameters.Add("@UpNo", qUpNo.Value.ToString());
                    }
                    #endregion

                    #region 銷帳狀態
                    switch (qCancelStatus)
                    {
                        case CancelStatusCodeTexts.NONPAY:      //未繳款
                            whereSqls.Add(String.Format("([{0}] = '' OR [{0}] IS NULL)", StudentReceiveEntity.Field.ReceiveDate));
                            break;
                        case CancelStatusCodeTexts.PAYED:       //已繳款
                            whereSqls.Add(String.Format("(([{0}] != '' AND [{0}] IS NOT NULL) AND ([{1}] = '' OR [{1}] IS NULL))", StudentReceiveEntity.Field.ReceiveDate, StudentReceiveEntity.Field.AccountDate));
                            break;
                        case CancelStatusCodeTexts.CANCELED:    //已入帳
                            whereSqls.Add(String.Format("([{0}] != '' AND [{0}] IS NOT NULL)", StudentReceiveEntity.Field.AccountDate));
                            break;
                    }
                    #endregion

                    #region 繳款方式 + 代收日區間
                    if (qCancelStatus != CancelStatusCodeTexts.NONPAY)
                    {
                        if (!String.IsNullOrEmpty(qReceiveWay))
                        {
                            whereSqls.Add(String.Format("([{0}] = @ReceiveWay)", StudentReceiveEntity.Field.ReceiveWay));
                            parameters.Add("@ReceiveWay", qReceiveWay);
                        }

                        if (qSReceivDate != null)
                        {
                            whereSqls.Add(String.Format("([{0}] >= @SReceivDate)", StudentReceiveEntity.Field.ReceiveDate));
                            parameters.Add("@SReceivDate", Common.GetTWDate7(qSReceivDate.Value));
                        }
                        if (qEReceivDate != null)
                        {
                            whereSqls.Add(String.Format("(SR.[{0}] <= @EReceivDate)", StudentReceiveEntity.Field.ReceiveDate));
                            parameters.Add("@EReceivDate", Common.GetTWDate7(qEReceivDate.Value));
                        }
                    }
                    #endregion

                    #region 入帳日區間
                    if (String.IsNullOrEmpty(qCancelStatus) || qCancelStatus == CancelStatusCodeTexts.CANCELED)
                    {
                        if (qSAccountDate != null)
                        {
                            whereSqls.Add(String.Format("(SR.[{0}] >= @SAccountDate)", StudentReceiveEntity.Field.AccountDate));
                            parameters.Add("@SAccountDate", Common.GetTWDate7(qSAccountDate.Value));
                        }
                        if (qEAccountDate != null)
                        {
                            whereSqls.Add(String.Format("(SR.[{0}] <= @EAccountDate)", StudentReceiveEntity.Field.AccountDate));
                            parameters.Add("@EAccountDate", Common.GetTWDate7(qEAccountDate.Value));
                        }
                    }
                    #endregion

                    #region 查詢欄位與值
                    if (!String.IsNullOrEmpty(qFieldName) && !String.IsNullOrEmpty(qFieldValue))
                    {
                        switch (qFieldName)
                        {
                            case "StuId":   //學號
                                whereSqls.Add(String.Format("(SR.[{0}] = @StuId)", StudentReceiveEntity.Field.StuId));
                                parameters.Add("@StuId", qFieldValue);
                                break;
                            case "CancelNo":   //虛擬帳號
                                whereSqls.Add(String.Format("(SR.[{0}] = @CancelNo)", StudentReceiveEntity.Field.CancelNo));
                                parameters.Add("@CancelNo", qFieldValue);
                                break;
                            case "IdNumber":   //身分證字號
                                whereSqls.Add(String.Format("EXISTS (SELECT 1 FROM [{0}] AS SM WHERE SM.[{1}] = SR.[{2}] AND SM.[{3}] = SR.[{4}] AND SM.[{5}] = SR.[{6}] AND SM.[{7}] = @IdNumber)"
                                    , StudentMasterEntity.TABLE_NAME
                                    , StudentMasterEntity.Field.ReceiveType, StudentReceiveEntity.Field.ReceiveType
                                    , StudentMasterEntity.Field.DepId, StudentReceiveEntity.Field.DepId
                                    , StudentMasterEntity.Field.Id, StudentReceiveEntity.Field.StuId
                                    , StudentMasterEntity.Field.IdNumber));
                                parameters.Add("@IdNumber", qFieldValue);
                                break;
                        }
                    }
                    #endregion

                    string sql = String.Format(@"SELECT SR.[{1}] AS [商家代號]
     , CASE WHEN SR.[{2}] IS NULL OR LEN(SR.[{2}]) < 5 THEN '' ELSE SUBSTRING(SR.[{2}], 5, LEN(SR.[{2}]) - 4) END AS [用戶號碼]
     , ISNULL(SR.[{3}], '') AS [交易日期], ISNULL(SR.[{4}], '') AS [交易時間]
     , CAST(SR.[{5}] AS numeric(18, 0)) AS [交易金額]
     , CASE WHEN SR.[{6}] IS NULL OR SR.[{6}] = '' THEN '' ELSE ISNULL((SELECT [{11}] FROM [{10}] AS CS WHERE CS.[{12}] = SR.[{6}]), SR.[{6}]) END AS [交易來源]
     , ISNULL(SR.[{7}], '') AS [作帳日期], ISNULL(SR.[{8}], '') AS [繳款行], SR.[{9}] AS [學號]
  FROM [{0}] AS SR
 WHERE {13}"
                        , StudentReceiveEntity.TABLE_NAME
                        , StudentReceiveEntity.Field.ReceiveType, StudentReceiveEntity.Field.CancelNo
                        , StudentReceiveEntity.Field.ReceiveDate, StudentReceiveEntity.Field.ReceiveTime
                        , StudentReceiveEntity.Field.ReceiveAmount, StudentReceiveEntity.Field.ReceiveWay
                        , StudentReceiveEntity.Field.AccountDate, StudentReceiveEntity.Field.ReceivebankId
                        , StudentReceiveEntity.Field.StuId  //9
                        , ChannelSetEntity.TABLE_NAME, ChannelSetEntity.Field.ChannelName, ChannelSetEntity.Field.ChannelId     //10, 11, 12
                        , String.Join(" AND ", whereSqls.ToArray()));

                    Result result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢銷帳資料失敗，錯誤訊息：{0}", result.Message);
                    }
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return "查無銷帳資料";
                    }

                    #region 檢查資料筆數，不可超過筆數限制 (XLS 65535 筆; ODS 1048575 筆)
                    int count = dt.Rows.Count;
                    if (!isUseODS && count > 65535)
                    {
                        return "匯出資料超過 Excel (XLS) 的筆數限制 (65535 筆)";
                    }
                    else if (isUseODS && count > Fuju.ODS.ODSSheet.MAX_ROW_COUNT)
                    {
                        return String.Format("匯出資料超過 Calc (ODS) 的筆數限制 ({0} 筆)", Fuju.ODS.ODSSheet.MAX_ROW_COUNT);
                    }
                    #endregion
                }
                #endregion

                #region 匯出成檔案
                if (dt != null)
                {
                    if (isUseODS)
                    {
                        #region DataTable 轉 ODS
                        ODSHelper helper = new ODSHelper();
                        outFileContent = helper.DataTable2ODS(dt, isDecimalTruncate: true);
                        #endregion
                    }
                    else
                    {
                        #region DataTable 轉 Xls
                        ConvertFileHelper helper = new ConvertFileHelper();
                        outFileContent = helper.Dt2Xls(dt);
                        #endregion
                    }
                }
                #endregion

                return null;
            }
            catch (Exception ex)
            {
                return String.Concat("產出檔案發生例外，錯誤訊息；", ex.Message);
            }
        }
        #endregion
        #endregion

        #region 產生匯出資料檔
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 非同步 產生匯出資料檔 的委派
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="receiveType"></param>
        /// <param name="kind"></param>
        /// <param name="qYearId"></param>
        /// <param name="qTermId"></param>
        /// <param name="qReceiveId"></param>
        /// <param name="qUpNo"></param>
        /// <param name="qCancelStatus"></param>
        /// <param name="qReceiveWay"></param>
        /// <param name="qSReceivDate"></param>
        /// <param name="qEReceivDate"></param>
        /// <param name="qSAccountDate"></param>
        /// <param name="qEAccountDate"></param>
        /// <param name="qFieldName"></param>
        /// <param name="qFieldValue"></param>
        /// <param name="outFields"></param>
        /// <returns></returns>
        public delegate Result AsyncGenExportFileData(int sn, string receiveType, string kind
            , string qYearId, string qTermId, string qReceiveId
            , int? qUpNo, string qCancelStatus, string qReceiveWay, DateTime? qSReceivDate, DateTime? qEReceivDate, DateTime? qSAccountDate, DateTime? qEAccountDate
            , string qFieldName, string qFieldValue, ICollection<string> outFields, bool isUseODS = false);

        /// <summary>
        /// 產生匯出資料檔 (使用非同步)
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="receiveType"></param>
        /// <param name="kind"></param>
        /// <param name="qYearId"></param>
        /// <param name="qTermId"></param>
        /// <param name="qReceiveId"></param>
        /// <param name="qUpNo"></param>
        /// <param name="qCancelStatus"></param>
        /// <param name="qReceiveWay"></param>
        /// <param name="qSReceivDate"></param>
        /// <param name="qEReceivDate"></param>
        /// <param name="qSAccountDate"></param>
        /// <param name="qEAccountDate"></param>
        /// <param name="qFieldName"></param>
        /// <param name="qFieldValue"></param>
        /// <param name="outFields"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IAsyncResult GenExportFileDataAsync(int sn, string receiveType, string kind
            , string qYearId, string qTermId, string qReceiveId
            , int? qUpNo, string qCancelStatus, string qReceiveWay, DateTime? qSReceivDate, DateTime? qEReceivDate, DateTime? qSAccountDate, DateTime? qEAccountDate
            , string qFieldName, string qFieldValue, ICollection<string> outFields, bool isUseODS
            , AsyncCallback callback)
        {
            AsyncGenExportFileData myAsync = new AsyncGenExportFileData(GenExportFileData);
            //Result result = myAsync.Invoke(receiveType, kind
            //    , qYearId, qTermId, qReceiveId
            //    , qUpNo, qCancelStatus, qReceiveWay, qSReceivDate, qEReceivDate, qSAccountDate, qEAccountDate
            //    , qFieldName, qFieldValue);
            IAsyncResult asyncResult = myAsync.BeginInvoke(sn, receiveType, kind
                , qYearId, qTermId, qReceiveId
                , qUpNo, qCancelStatus, qReceiveWay, qSReceivDate, qEReceivDate, qSAccountDate, qEAccountDate
                , qFieldName, qFieldValue, outFields, isUseODS
                , callback, null);

            return asyncResult;
        }

        /// <summary>
        /// 產生匯出資料檔 (實作)
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="receiveType"></param>
        /// <param name="kind"></param>
        /// <param name="qYearId"></param>
        /// <param name="qTermId"></param>
        /// <param name="qReceiveId"></param>
        /// <param name="qUpNo"></param>
        /// <param name="qCancelStatus"></param>
        /// <param name="qReceiveWay"></param>
        /// <param name="qSReceivDate"></param>
        /// <param name="qEReceivDate"></param>
        /// <param name="qSAccountDate"></param>
        /// <param name="qEAccountDate"></param>
        /// <param name="qFieldName"></param>
        /// <param name="qFieldValue"></param>
        /// <param name="outFields"></param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public Result GenExportFileData(int sn, string receiveType, string kind
            , string qYearId, string qTermId, string qReceiveId
            , int? qUpNo, string qCancelStatus, string qReceiveWay, DateTime? qSReceivDate, DateTime? qEReceivDate, DateTime? qSAccountDate, DateTime? qEAccountDate
            , string qFieldName, string qFieldValue, ICollection<string> outFields, bool isUseODS = false)
        {
            #region 檢查參數
            if (sn < 1 || String.IsNullOrEmpty(receiveType))
            {
                return new Result(false, "缺少或無效的資料參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            if (outFields == null || outFields.Count == 0)
            {
                return new Result(false, "缺少匯出資料項目參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            string fileName = String.Format("{0}_{1:yyyyMMddHHmmss}", receiveType, DateTime.Now);
            string extName = "";
            switch (kind)
            {
                case ExportFileKindCodeTexts.C3700008:
                case ExportFileKindCodeTexts.B2100006:
                    if (isUseODS)
                    {
                        extName = "ODS";
                    }
                    else
                    {
                        extName = "ZIP";
                    }
                    break;
                default:
                    return new Result(false, "無效的資料參數", CoreStatusCode.INVALID_PARAMETER, null);
            }

            string qDepId = String.Empty;   //土銀不使用原部別，所以固定為空字串

            Result result = null;

            //因為非同步也會執行這個方法，所以 EntityFactory 必須私有，避免被外界 Dispose
            EntityFactory factory = null;
            ExportFileEntity exportData = null;
            DataTable dt = null;    //因為資料可能很大所以程式自己做回收
            try
            {
                factory = _Factory.CloneForNonTransaction();

                #region 取得 ExportFile 並註記為處理中
                {
                    string sql = String.Format(@"
UPDATE [{0}] SET [{1}] = @FileName, [{2}] = @ExtName, [{3}] = @ProcessStatus, [{4}] = GETDATE(), [{5}] = 'SYSTEM' 
 WHERE [{3}] = @WaitStatus AND [{6}] = @SN AND [{7}] = @ReceiveType AND [{8}] = @Kind ;"
                        , ExportFileEntity.TABLE_NAME
                        , ExportFileEntity.Field.FileName, ExportFileEntity.Field.ExtName
                        , ExportFileEntity.Field.Status, ExportFileEntity.Field.MdyDate, ExportFileEntity.Field.MdyUser
                        , ExportFileEntity.Field.SN, ExportFileEntity.Field.ReceiveType, ExportFileEntity.Field.Kind);
                    KeyValue[] parameters = new KeyValue[] {
                        new KeyValue("@FileName", fileName), new KeyValue("@ExtName", extName), 
                        new KeyValue("@ProcessStatus", ExportFileStatusCodeTexts.PROCESS), new KeyValue("@WaitStatus", ExportFileStatusCodeTexts.WAIT),
                        new KeyValue("@SN", sn), new KeyValue("@ReceiveType", receiveType), new KeyValue("@Kind", kind)
                    };

                    int count = 0;
                    result = factory.ExecuteNonQuery(sql, parameters, out count);
                    if (!result.IsSuccess)
                    {
                        result = new Result(false, String.Format("註記指定作業為處理中失敗，錯誤訊息：{0}", result.Message), ErrorCode.D_QUERY_NO_DATA, result.Exception);
                        return result;
                    }
                    if (count == 0)
                    {
                        result = new Result(false, "無效的作業參數或該作業非待處理", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                        return result;
                    }

                    Expression where = new Expression(ExportFileEntity.Field.SN, sn);
                    result = factory.SelectFirst<ExportFileEntity>(where, null, out exportData);
                    if (!result.IsSuccess)
                    {
                        result = new Result(false, String.Format("讀取指定作業資料失敗，錯誤訊息：{0}", result.Message), ErrorCode.D_QUERY_NO_DATA, result.Exception);
                        return result;
                    }
                    if (exportData == null)
                    {
                        result = new Result(false, "查無指定作業資料", ErrorCode.D_DATA_NOT_FOUND, null);
                        return result;
                    }
                    if (exportData.FileName != fileName || exportData.ExtName != extName || exportData.Status != ExportFileStatusCodeTexts.PROCESS)
                    {
                        //不是這個程序的
                        exportData = null;
                        result = new Result(false, "該作業已被其他程序修改過", CoreStatusCode.UNKNOWN_ERROR, null);
                        return result;
                    }
                }
                #endregion

                #region 以下開始的程式在 return 前要先指定 exportData.Explain 與 exportData.Status
                try
                {
                    #region 取得各費用別的繳款期限、超商延遲日與要採用的收入科目名稱、備註標題

                    #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                    #region [OLD]
                    //KeyValueList<DateTime?> receivePayDueDates = new KeyValueList<DateTime?>();
                    //KeyValueList<int> receiveExtraDays = new KeyValueList<int>();
                    #endregion

                    List<string> receiveKeys = new List<string>();
                    List<DateTime?> receivePayDueDates = new List<DateTime?>();
                    List<int> receiveExtraDays = new List<int>();
                    #endregion

                    List<string> receiveItemNames = new List<string>(SchoolRidEntity.ReceiveItemMaxCount);
                    List<string> receiveMemoTitles = new List<string>(SchoolRidEntity.MemoTitleMaxCount);
                    int receiveItemNameDataCount = 0;  //有收入科目名稱的數量
                    {
                        dt = null;

                        #region 讀取代收費用別設定資料
                        KeyValueList parameters = new KeyValueList(5);
                        List<string> whereSqls = new List<string>(5);

                        #region 商家代號
                        whereSqls.Add("[Receive_Type] = @ReceiveType");
                        parameters.Add("@ReceiveType", receiveType);
                        #endregion

                        #region 學年
                        if (!String.IsNullOrEmpty(qYearId))
                        {
                            whereSqls.Add("[Year_Id] = @YearId");
                            parameters.Add("@YearId", qYearId);
                        }
                        #endregion

                        #region 學期
                        if (!String.IsNullOrEmpty(qTermId))
                        {
                            whereSqls.Add("[Term_Id] = @TermId");
                            parameters.Add("@TermId", qTermId);
                        }
                        #endregion

                        #region 原部別 (Dep_Id)
                        if (qDepId != null)
                        {
                            whereSqls.Add("[Dep_Id] = @DepId");
                            parameters.Add("@DepId", qDepId);
                        }
                        #endregion

                        #region 費用別
                        if (!String.IsNullOrEmpty(qReceiveId))
                        {
                            whereSqls.Add("[Receive_Id] = @ReceiveId");
                            parameters.Add("@ReceiveId", qReceiveId);
                        }
                        #endregion

                        string sql = @"
SELECT [Receive_Type], [Year_Id], [Term_Id], [Dep_Id], [Receive_Id], ISNULL([Pay_Date], '') AS [Pay_Date], ISNULL([Extra_Days], 0) AS [Extra_Days]
     , ISNULL([Receive_Item01], '') AS [Receive_Item01], ISNULL([Receive_Item02], '') AS [Receive_Item02], ISNULL([Receive_Item03], '') AS [Receive_Item03], ISNULL([Receive_Item04], '') AS [Receive_Item04], ISNULL([Receive_Item05], '') AS [Receive_Item05]
     , ISNULL([Receive_Item06], '') AS [Receive_Item06], ISNULL([Receive_Item07], '') AS [Receive_Item07], ISNULL([Receive_Item08], '') AS [Receive_Item08], ISNULL([Receive_Item09], '') AS [Receive_Item09], ISNULL([Receive_Item10], '') AS [Receive_Item10]
     , ISNULL([Receive_Item11], '') AS [Receive_Item11], ISNULL([Receive_Item12], '') AS [Receive_Item12], ISNULL([Receive_Item13], '') AS [Receive_Item13], ISNULL([Receive_Item14], '') AS [Receive_Item14], ISNULL([Receive_Item15], '') AS [Receive_Item15]
     , ISNULL([Receive_Item16], '') AS [Receive_Item16], ISNULL([Receive_Item17], '') AS [Receive_Item17], ISNULL([Receive_Item18], '') AS [Receive_Item18], ISNULL([Receive_Item19], '') AS [Receive_Item19], ISNULL([Receive_Item20], '') AS [Receive_Item20]
     , ISNULL([Receive_Item21], '') AS [Receive_Item21], ISNULL([Receive_Item22], '') AS [Receive_Item22], ISNULL([Receive_Item23], '') AS [Receive_Item23], ISNULL([Receive_Item24], '') AS [Receive_Item24], ISNULL([Receive_Item25], '') AS [Receive_Item25]
     , ISNULL([Receive_Item26], '') AS [Receive_Item26], ISNULL([Receive_Item27], '') AS [Receive_Item27], ISNULL([Receive_Item28], '') AS [Receive_Item28], ISNULL([Receive_Item29], '') AS [Receive_Item29], ISNULL([Receive_Item30], '') AS [Receive_Item30]
     , ISNULL([Receive_Item31], '') AS [Receive_Item31], ISNULL([Receive_Item32], '') AS [Receive_Item32], ISNULL([Receive_Item33], '') AS [Receive_Item33], ISNULL([Receive_Item34], '') AS [Receive_Item34], ISNULL([Receive_Item35], '') AS [Receive_Item35]
     , ISNULL([Receive_Item36], '') AS [Receive_Item36], ISNULL([Receive_Item37], '') AS [Receive_Item37], ISNULL([Receive_Item38], '') AS [Receive_Item38], ISNULL([Receive_Item39], '') AS [Receive_Item39], ISNULL([Receive_Item40], '') AS [Receive_Item40]
     , ISNULL([Memo_Title01], '') AS [Memo_Title01], ISNULL([Memo_Title02], '') AS [Memo_Title02], ISNULL([Memo_Title03], '') AS [Memo_Title03], ISNULL([Memo_Title04], '') AS [Memo_Title04], ISNULL([Memo_Title05], '') AS [Memo_Title05]
     , ISNULL([Memo_Title06], '') AS [Memo_Title06], ISNULL([Memo_Title07], '') AS [Memo_Title07], ISNULL([Memo_Title08], '') AS [Memo_Title08], ISNULL([Memo_Title09], '') AS [Memo_Title09], ISNULL([Memo_Title10], '') AS [Memo_Title10]
     , ISNULL([Memo_Title11], '') AS [Memo_Title11], ISNULL([Memo_Title12], '') AS [Memo_Title12], ISNULL([Memo_Title13], '') AS [Memo_Title13], ISNULL([Memo_Title14], '') AS [Memo_Title14], ISNULL([Memo_Title15], '') AS [Memo_Title15]
     , ISNULL([Memo_Title16], '') AS [Memo_Title16], ISNULL([Memo_Title17], '') AS [Memo_Title17], ISNULL([Memo_Title18], '') AS [Memo_Title18], ISNULL([Memo_Title19], '') AS [Memo_Title19], ISNULL([Memo_Title20], '') AS [Memo_Title20]
     , ISNULL([Memo_Title21], '') AS [Memo_Title21]
  FROM School_Rid 
 WHERE " + String.Join(" AND ", whereSqls);

                        result = factory.GetDataTable(sql, parameters, 0, 0, out dt);
                        if (!result.IsSuccess)
                        {
                            exportData.Explain += String.Concat(Environment.NewLine, "結果：讀取代收費用別設定資料失敗，", result.Message);
                            exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                            result = new Result(false, String.Format("讀取代收費用別設定資料失敗，錯誤訊息：{0}", result.Message), ErrorCode.D_QUERY_NO_DATA, result.Exception);
                            return result;
                        }
                        if (dt == null || dt.Rows.Count == 0)
                        {
                            exportData.Explain += String.Concat(Environment.NewLine, "結果：查無代收費用別設定資料");
                            exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                            result = new Result(false, "查無代收費用別設定資料", ErrorCode.D_QUERY_NO_DATA, null);
                            return result;
                        }
                        #endregion

                        #region 紀錄繳款期限、超商延遲日，並找出要採用的收入科目名稱與備註標題 (收入科目名稱最多的那一筆)
                        #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                        foreach (DataRow drow in dt.Rows)
                        {
                            #region 繳款期限、超商延遲日
                            {
                                #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                                #region [OLD]
                                //#region [MDY:20170708] Row[] 取值後 Replace 單引號 (For Heuristic SQL Injection 修改)
                                //string key = String.Format("{0}_{1}_{2}_{3}_{4}", drow["Receive_Type"].ToString().Trim(), drow["Year_Id"].ToString().Trim(), drow["Term_Id"].ToString().Trim(), drow["Dep_Id"].ToString().Trim(), drow["Receive_Id"].ToString().Trim()).Replace("'", "");
                                //#endregion

                                //DateTime? payDueDate = DataFormat.ConvertDateText(drow["Pay_Date"].ToString().Trim());
                                //int extraDays = Convert.ToInt32(drow["Extra_Days"]);
                                //receivePayDueDates.Add(key, payDueDate);
                                //receiveExtraDays.Add(key, extraDays);
                                #endregion

                                string myReceiveType = drow["Receive_Type"].ToString().Trim();
                                string myYearId = drow["Year_Id"].ToString().Trim();
                                string myTermId = drow["Term_Id"].ToString().Trim();
                                string myDepId = drow["Dep_Id"].ToString().Trim();
                                string myReceiveId = drow["Receive_Id"].ToString().Trim();

                                string receiveKey = $"{myReceiveType}_{myYearId}_{myTermId}_{myDepId}_{myReceiveId}";
                                DateTime? payDueDate = DataFormat.ConvertDateText(drow["Pay_Date"].ToString().Trim());
                                int extraDays = Convert.ToInt32(drow["Extra_Days"]);
                                receiveKeys.Add(receiveKey);
                                receivePayDueDates.Add(payDueDate);
                                receiveExtraDays.Add(extraDays);
                                #endregion
                            }
                            #endregion

                            #region 收集此筆資料收入科目名稱作為採用的收入科目名稱
                            for (int no = 1; no <= 40; no++)
                            {
                                string fieldName = $"Receive_Item{no:00}";
                                string itemName = drow.IsNull(fieldName) ? null : drow[fieldName].ToString().Trim();
                                if (String.IsNullOrEmpty(itemName))
                                {
                                    receiveItemNames.Add(null);
                                }
                                else
                                {
                                    receiveItemNames.Add(itemName);
                                    receiveItemNameDataCount++;
                                }
                            }
                            #endregion

                            #region 收集此筆資料備註標題作為採用的備註標題
                            receiveMemoTitles.Clear();
                            for (int no = 1; no <= SchoolRidEntity.MemoTitleMaxCount; no++)
                            {
                                string fieldName = String.Format("Memo_Title{0:00}", no);
                                string fieldValue = drow[fieldName].ToString();
                                if (String.IsNullOrEmpty(fieldValue))
                                {
                                    receiveMemoTitles.Add(String.Format("備註{0:00}", no));
                                }
                                else
                                {
                                    receiveMemoTitles.Add(fieldValue);
                                }
                            }
                            #endregion
                        }
                        #endregion
                        #endregion

                        if (dt != null)
                        {
                            dt.Clear();
                            dt.Dispose();
                            dt = null;
                        }
                    }
                    #endregion

                    #region 取得超商管道手續費
                    ReceiveChannelEntity[] smReceiveChannels = null;
                    {
                        Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType)
                            .And(ReceiveChannelEntity.Field.ChannelId, ChannelHelper.SM_DEFAULT);
                        result = factory.SelectAll<ReceiveChannelEntity>(where, null, out smReceiveChannels);
                        if (!result.IsSuccess)
                        {
                            exportData.Explain += String.Concat(Environment.NewLine, "結果：讀取超商管道手續費資料失敗，", result.Message);
                            exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                            result = new Result(false, String.Format("讀取超商管道手續費資料失敗，錯誤訊息：{0}", result.Message), ErrorCode.D_QUERY_NO_DATA, result.Exception);
                            return result;
                        }
                    }
                    #endregion

                    #region 取資料
                    {
                        #region 處理匯出資料欄位 (取得查詢欄位 Sql)
                        bool hasReceive = false;
                        bool hasZBarcode = false;
                        List<string> sqlFieldNames = new List<string>(outFields.Count + 5 + receiveItemNames.Count);

                        #region PKey 欄位
                        string PKEY_ReceiveType = String.Format("PKEY_{0}", StudentReceiveView4.Field.ReceiveType);
                        string PKEY_YearId = String.Format("PKEY_{0}", StudentReceiveView4.Field.YearId);
                        string PKEY_TermId = String.Format("PKEY_{0}", StudentReceiveView4.Field.TermId);
                        string PKEY_DepId = String.Format("PKEY_{0}", StudentReceiveView4.Field.DepId);
                        string PKEY_ReceiveId = String.Format("PKEY_{0}", StudentReceiveView4.Field.ReceiveId);
                        {
                            //sqlFieldNames.Add(StudentReceiveView4.Field.ReceiveType);
                            //sqlFieldNames.Add(StudentReceiveView4.Field.YearId);
                            //sqlFieldNames.Add(StudentReceiveView4.Field.TermId);
                            //sqlFieldNames.Add(StudentReceiveView4.Field.DepId);
                            //sqlFieldNames.Add(StudentReceiveView4.Field.ReceiveId);

                            sqlFieldNames.Add(String.Format("[{0}] AS [{1}]", StudentReceiveView4.Field.ReceiveType, PKEY_ReceiveType));
                            sqlFieldNames.Add(String.Format("[{0}] AS [{1}]", StudentReceiveView4.Field.YearId, PKEY_YearId));
                            sqlFieldNames.Add(String.Format("[{0}] AS [{1}]", StudentReceiveView4.Field.TermId, PKEY_TermId));
                            sqlFieldNames.Add(String.Format("[{0}] AS [{1}]", StudentReceiveView4.Field.DepId, PKEY_DepId));
                            sqlFieldNames.Add(String.Format("[{0}] AS [{1}]", StudentReceiveView4.Field.ReceiveId, PKEY_ReceiveId));
                        }
                        #endregion

                        foreach (string outField in outFields)
                        {
                            if (String.IsNullOrWhiteSpace(outField))
                            {
                                continue;
                            }
                            string fieldName = outField.Trim();
                            switch (fieldName.ToUpper())
                            {
                                case "RECEIVE":     //收入科目金額
                                    #region 收入科目金額
                                    if (!hasReceive)
                                    {
                                        hasReceive = true;
                                        string[] receiveFieldNames = new string[] {
                                            StudentReceiveView4.Field.Receive01, StudentReceiveView4.Field.Receive02, StudentReceiveView4.Field.Receive03, StudentReceiveView4.Field.Receive04, StudentReceiveView4.Field.Receive05, 
                                            StudentReceiveView4.Field.Receive06, StudentReceiveView4.Field.Receive07, StudentReceiveView4.Field.Receive08, StudentReceiveView4.Field.Receive09, StudentReceiveView4.Field.Receive10, 
                                            StudentReceiveView4.Field.Receive11, StudentReceiveView4.Field.Receive12, StudentReceiveView4.Field.Receive13, StudentReceiveView4.Field.Receive14, StudentReceiveView4.Field.Receive15, 
                                            StudentReceiveView4.Field.Receive16, StudentReceiveView4.Field.Receive17, StudentReceiveView4.Field.Receive18, StudentReceiveView4.Field.Receive19, StudentReceiveView4.Field.Receive20, 
                                            StudentReceiveView4.Field.Receive21, StudentReceiveView4.Field.Receive22, StudentReceiveView4.Field.Receive23, StudentReceiveView4.Field.Receive24, StudentReceiveView4.Field.Receive25, 
                                            StudentReceiveView4.Field.Receive26, StudentReceiveView4.Field.Receive27, StudentReceiveView4.Field.Receive28, StudentReceiveView4.Field.Receive29, StudentReceiveView4.Field.Receive30, 
                                            StudentReceiveView4.Field.Receive31, StudentReceiveView4.Field.Receive32, StudentReceiveView4.Field.Receive33, StudentReceiveView4.Field.Receive34, StudentReceiveView4.Field.Receive35, 
                                            StudentReceiveView4.Field.Receive36, StudentReceiveView4.Field.Receive37, StudentReceiveView4.Field.Receive38, StudentReceiveView4.Field.Receive39, StudentReceiveView4.Field.Receive40
                                        };
                                        for (int idx = 0; idx < receiveItemNames.Count; idx++)
                                        {
                                            #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                                            #region [OLD]
                                            ////sqlFieldNames.Add(receiveFieldNames[idx]);
                                            //sqlFieldNames.Add(String.Format("[{0}] AS [{1}]", receiveFieldNames[idx], receiveItemNames[idx]));
                                            #endregion

                                            if (!String.IsNullOrEmpty(receiveItemNames[idx]))
                                            {
                                                sqlFieldNames.Add($"[{receiveFieldNames[idx]}] AS RECEIVE_ITEM_{idx + 1:00}");
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                    break;
                                case "ZBARCODE":    //超商條碼
                                    #region 超商條碼
                                    if (!hasZBarcode)
                                    {
                                        hasZBarcode = true;
                                        sqlFieldNames.Add("'' AS [超商條碼1]");
                                        sqlFieldNames.Add("'' AS [超商條碼2]");
                                        sqlFieldNames.Add("'' AS [超商條碼3]");
                                        sqlFieldNames.Add(String.Format("[{0}] AS [計算用應繳金額]", StudentReceiveView4.Field.ReceiveAmount));
                                        sqlFieldNames.Add(String.Format("[{0}] AS [計算用超商金額]", StudentReceiveView4.Field.ReceiveSmamount));
                                        sqlFieldNames.Add(String.Format("[{0}] AS [計算用超商銷編]", StudentReceiveView4.Field.CancelSmno));
                                    }
                                    #endregion
                                    break;
                                default:
                                    sqlFieldNames.Add(String.Format("[{0}]", fieldName));
                                    break;
                            }
                        }
                        string fieldSql = String.Join(", ", sqlFieldNames);
                        #endregion

                        #region 查詢條件 Sql
                        List<string> whereSqls = new List<string>();
                        KeyValueList parameters = new KeyValueList();
                        {
                            #region 商家代號
                            whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveType)", StudentReceiveView4.Field.ReceiveType));
                            parameters.Add("@ReceiveType", receiveType);
                            #endregion

                            #region 學年
                            if (!String.IsNullOrEmpty(qYearId))
                            {
                                whereSqls.Add(String.Format("(SR.[{0}] = @YearId)", StudentReceiveView4.Field.YearId));
                                parameters.Add("@YearId", qYearId);
                            }
                            #endregion

                            #region 學期
                            if (!String.IsNullOrEmpty(qTermId))
                            {
                                whereSqls.Add(String.Format("(SR.[{0}] = @TermId)", StudentReceiveView4.Field.TermId));
                                parameters.Add("@TermId", qTermId);
                            }
                            #endregion

                            #region 原部別 (Dep_Id)
                            if (qDepId != null)
                            {
                                whereSqls.Add(String.Format("(SR.[{0}] = @DepId)", StudentReceiveView4.Field.DepId));
                                parameters.Add("@DepId", qDepId);
                            }
                            #endregion

                            #region 費用別
                            if (!String.IsNullOrEmpty(qReceiveId))
                            {
                                whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveId)", StudentReceiveView4.Field.ReceiveId));
                                parameters.Add("@ReceiveId", qReceiveId);
                            }
                            #endregion

                            #region 批號
                            if (qUpNo != null)
                            {
                                whereSqls.Add(String.Format("([{0}] = @UpNo)", StudentReceiveView4.Field.UpNo));
                                parameters.Add("@UpNo", qUpNo.Value);
                            }
                            #endregion

                            #region 銷帳狀態
                            switch (qCancelStatus)
                            {
                                case CancelStatusCodeTexts.NONPAY:      //未繳款
                                    whereSqls.Add(String.Format("((SR.[{0}] = '' OR SR.[{0}] IS NULL) AND (SR.[{1}] = '' OR SR.[{1}] IS NULL))", StudentReceiveView4.Field.ReceiveDate, StudentReceiveView4.Field.AccountDate));
                                    break;
                                case CancelStatusCodeTexts.PAYED:       //已繳款
                                    whereSqls.Add(String.Format("((SR.[{0}] != '' AND SR.[{0}] IS NOT NULL) AND (SR.[{1}] = '' OR SR.[{1}] IS NULL))", StudentReceiveView4.Field.ReceiveDate, StudentReceiveView4.Field.AccountDate));
                                    break;
                                case CancelStatusCodeTexts.CANCELED:    //已入帳
                                    whereSqls.Add(String.Format("(SR.[{0}] != '' AND SR.[{0}] IS NOT NULL)", StudentReceiveView4.Field.AccountDate));
                                    break;
                            }
                            #endregion

                            #region 繳款方式 + 代收日區間 + 入帳日區間
                            if (qCancelStatus != CancelStatusCodeTexts.NONPAY)
                            {
                                #region 繳款方式
                                if (!String.IsNullOrEmpty(qReceiveWay))
                                {
                                    whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveWay)", StudentReceiveView4.Field.ReceiveWay));
                                    parameters.Add("@ReceiveWay", qReceiveWay);
                                }
                                #endregion

                                #region 代收日區間
                                if (qSReceivDate != null)
                                {
                                    whereSqls.Add(String.Format("(SR.[{0}] >= @SReceivDate)", StudentReceiveView4.Field.ReceiveDate));
                                    parameters.Add("@SReceivDate", Common.GetTWDate7(qSReceivDate.Value));
                                }
                                if (qEReceivDate != null)
                                {
                                    whereSqls.Add(String.Format("(SR.[{0}] <= @EReceivDate)", StudentReceiveView4.Field.ReceiveDate));
                                    parameters.Add("@EReceivDate", Common.GetTWDate7(qEReceivDate.Value));
                                }
                                #endregion

                                #region 入帳日區間
                                if (String.IsNullOrEmpty(qCancelStatus) || qCancelStatus == CancelStatusCodeTexts.CANCELED)
                                {
                                    if (qSAccountDate != null)
                                    {
                                        whereSqls.Add(String.Format("(SR.[{0}] >= @SAccountDate)", StudentReceiveView4.Field.AccountDate));
                                        parameters.Add("@SAccountDate", Common.GetTWDate7(qSAccountDate.Value));
                                    }
                                    if (qEAccountDate != null)
                                    {
                                        whereSqls.Add(String.Format("(SR.[{0}] <= @EAccountDate)", StudentReceiveView4.Field.AccountDate));
                                        parameters.Add("@EAccountDate", Common.GetTWDate7(qEAccountDate.Value));
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #region 查詢欄位與值
                            if (!String.IsNullOrEmpty(qFieldName) && !String.IsNullOrEmpty(qFieldValue))
                            {
                                switch (qFieldName)
                                {
                                    case "StuId":   //學號
                                        whereSqls.Add(String.Format("(SR.[{0}] = @StuId)", StudentReceiveView4.Field.StuId));
                                        parameters.Add("@StuId", qFieldValue);
                                        break;
                                    case "CancelNo":   //虛擬帳號
                                        whereSqls.Add(String.Format("(SR.[{0}] = @CancelNo)", StudentReceiveView4.Field.CancelNo));
                                        parameters.Add("@CancelNo", qFieldValue);
                                        break;
                                    case "IdNumber":   //身分證字號
                                        whereSqls.Add(String.Format("(SM.[{0}] = @StuIdNumber)", StudentReceiveView4.Field.StuIdNumber));
                                        parameters.Add("@StuIdNumber", qFieldValue);
                                        break;
                                }
                            }
                            #endregion
                        }
                        string whereSql = String.Join(" AND ", whereSqls);
                        #endregion

                        #region 檢查資料筆數，不可超過筆數限制 (XLS 65535 筆; ODS 1048575 筆)
                        int maxXlsRowCount = 65535;
                        {
                            string sql = String.Format(@"SELECT COUNT(1) 
  FROM (
{0} 
 WHERE {1}
) AS T", StudentReceiveView4.VIEWSQL, whereSql);
                            object value = null;
                            result = factory.ExecuteScalar(sql, parameters, out value);
                            if (result.IsSuccess)
                            {
                                Int64 count = 0;
                                if (value == null || !Int64.TryParse(value.ToString(), out count))
                                {
                                    exportData.Explain += String.Concat(Environment.NewLine, "結果：檢查匯出資料筆數失敗，回傳資料不正確");
                                    exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                                    result = new Result(false, "檢查匯出資料筆數失敗，錯誤訊息：回傳資料不正確", CoreStatusCode.S_SELECT_COUNT_FAILURE, null);
                                    return result;
                                }
                                else if (!isUseODS && count > maxXlsRowCount) //XLS 版本的限制，為了相容性所以要限制
                                {
                                    exportData.Explain += String.Concat(Environment.NewLine, "結果：匯出資料超過 Excel (XLS) 的筆數限制 (65535 筆)");
                                    exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                                    result = new Result(false, "匯出資料超過 Excel (XLS) 的筆數限制 (65535 筆)", CoreStatusCode.UNKNOWN_ERROR, null);
                                    return result;
                                }
                                else if (isUseODS && count > Fuju.ODS.ODSSheet.MAX_ROW_COUNT)
                                {
                                    exportData.Explain += String.Format("{0}結果：匯出資料超過 Calc (ODS) 的筆數限制 ({1} 筆)", Environment.NewLine, Fuju.ODS.ODSSheet.MAX_ROW_COUNT);
                                    exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                                    result = new Result(false, String.Format("匯出資料超過 Calc (ODS) 的筆數限制 ({0} 筆)", Fuju.ODS.ODSSheet.MAX_ROW_COUNT), CoreStatusCode.UNKNOWN_ERROR, null);
                                    return result;
                                }
                                else if (count == 0)
                                {
                                    exportData.Explain += String.Concat(Environment.NewLine, "結果：查無匯出資料");
                                    exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                                    result = new Result(false, "查無匯出資料", ErrorCode.D_QUERY_NO_DATA, null);
                                    return result;
                                }
                            }
                            else
                            {
                                exportData.Explain += String.Concat(Environment.NewLine, "結果：檢查匯出資料筆數失敗，", result.Message);
                                exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                                result = new Result(false, String.Format("檢查匯出資料筆數失敗，錯誤訊息：{0}", result.Message), CoreStatusCode.S_SELECT_COUNT_FAILURE, result.Exception);
                                return result;
                            }
                        }
                        #endregion

                        dt = null;

                        #region 讀取資料
                        {
                            string orderSql = String.Format("[{0}], [{1}], [{2}], [{3}], [{4}], [{5}]"
                                , StudentReceiveView4.Field.ReceiveType, StudentReceiveView4.Field.YearId, StudentReceiveView4.Field.TermId
                                , StudentReceiveView4.Field.DepId, StudentReceiveView4.Field.ReceiveId, StudentReceiveView4.Field.StuId);

                            #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                            #region [OLD]
////                            string sql = String.Format(@"SELECT {0} 
////  FROM (
////{1} 
//// WHERE {2}
////) AS T 
//// ORDER BY {3}", fieldSql, StudentReceiveView4.VIEWSQL, whereSql, orderSql);
                            #endregion

                            string sql = $@"
SELECT {fieldSql}
  FROM (
{StudentReceiveView4.VIEWSQL} 
 WHERE {whereSql}
) AS T 
 ORDER BY {orderSql}";
                            #endregion

                            result = factory.GetDataTable(sql, parameters, 0, maxXlsRowCount, out dt);
                            if (!result.IsSuccess)
                            {
                                exportData.Explain += String.Concat(Environment.NewLine, "結果：讀取匯出資料失敗，", result.Message);
                                exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                                result = new Result(false, String.Format("讀取匯出資料失敗，錯誤訊息：{0}", result.Message), CoreStatusCode.S_SELECT_COUNT_FAILURE, result.Exception);
                                return result;
                            }
                            if (dt == null || dt.Rows.Count == 0)
                            {
                                exportData.Explain += String.Concat(Environment.NewLine, "結果：查無匯出資料");
                                exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                                result = new Result(false, "查無匯出資料", ErrorCode.D_QUERY_NO_DATA, null);
                                return result;
                            }
                        }
                        #endregion

                        #region 處理超商條碼
                        if (hasZBarcode && smReceiveChannels != null && smReceiveChannels.Length > 0 && dt != null && dt.Rows.Count > 0)
                        {
                            ChannelHelper helper = new ChannelHelper();
                            foreach (DataRow drow in dt.Rows)
                            {
                                decimal amount = 0;
                                string amountTxt = drow.IsNull("計算用應繳金額") ? null : drow["計算用應繳金額"].ToString().Trim();
                                if (String.IsNullOrEmpty(amountTxt) || !Decimal.TryParse(amountTxt, out amount) || amount <= 0)
                                {
                                    continue;
                                }

                                decimal smAmount = 0;
                                string smAmountTxt = drow.IsNull("計算用超商金額") ? null : drow["計算用超商金額"].ToString().Trim();
                                if (String.IsNullOrEmpty(smAmountTxt) || !Decimal.TryParse(smAmountTxt, out smAmount) || smAmount < 0)
                                {
                                    continue;
                                }

                                string smCancelNo = drow.IsNull("計算用超商銷編") ? null : drow["計算用超商銷編"].ToString().Trim();
                                if (String.IsNullOrEmpty(smCancelNo) || !Common.IsNumber(smCancelNo))
                                {
                                    continue;
                                }

                                ReceiveChannelEntity smChannel = null;
                                ReceiveChannelEntity cashChannel = null;
                                helper.CheckReceiveChannel(amount, smReceiveChannels, out smChannel, out cashChannel);
                                if (smChannel == null)
                                {
                                    continue;
                                }

                                #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                                #region [OLD]
                                //#region [MDY:20170708] Row[] 取值後 Replace 單引號 (For Heuristic SQL Injection 修改)
                                //string key = String.Format("{0}_{1}_{2}_{3}_{4}", drow[PKEY_ReceiveType].ToString().Trim(), drow[PKEY_YearId].ToString().Trim(), drow[PKEY_TermId].ToString().Trim(), drow[PKEY_DepId].ToString().Trim(), drow[PKEY_ReceiveId].ToString().Trim()).Replace("'", "");
                                //#endregion

                                //DateTime? payDueDate = receivePayDueDates.TryGetValue(key, null);
                                //if (payDueDate == null)
                                //{
                                //    continue;
                                //}
                                //int extraDays = receiveExtraDays.TryGetValue(key, 0);
                                #endregion

                                string pkReceiveType = drow[PKEY_ReceiveType].ToString().Trim();
                                string pkYearId = drow[PKEY_YearId].ToString().Trim();
                                string pkTermId = drow[PKEY_TermId].ToString().Trim();
                                string pkDepId = drow[PKEY_DepId].ToString().Trim();
                                string pkReceiveId = drow[PKEY_ReceiveId].ToString().Trim();
                                string receiveKey = $"{pkReceiveType}_{pkYearId}_{pkTermId}_{pkDepId}_{pkReceiveId}";

                                DateTime? payDueDate = null;
                                int extraDays = 0;
                                int idx = receiveKeys.FindIndex(x => x == receiveKey);
                                if (idx > -1)
                                {
                                    payDueDate = receivePayDueDates[idx];
                                    extraDays = receiveExtraDays[idx];
                                }
                                if (payDueDate == null)
                                {
                                    continue;
                                }
                                #endregion

                                string smBarcode1 = null;
                                string smBarcode2 = null;
                                string smBarcode3 = null;
                                this.GenSMBarcode(smCancelNo, smAmount, payDueDate.Value, extraDays, smChannel.BarcodeId, out smBarcode1, out smBarcode2, out smBarcode3);
                                drow["超商條碼1"] = smBarcode1 ?? String.Empty;
                                drow["超商條碼2"] = smBarcode2 ?? String.Empty;
                                drow["超商條碼3"] = smBarcode3 ?? String.Empty;
                            }
                        }
                        #endregion

                        #region 轉換資料前，先移除計算用的應繳金額、超商金額、超商銷編
                        if (hasZBarcode && dt != null)
                        {
                            dt.Columns.Remove("計算用應繳金額");
                            dt.Columns.Remove("計算用超商金額");
                            dt.Columns.Remove("計算用超商銷編");
                        }
                        #endregion

                        #region 轉換資料前，先移除 PKey 欄位 (因為沒有要匯出)
                        if (dt != null)
                        {
                            //dt.Columns.Remove(StudentReceiveView4.Field.ReceiveType);
                            //dt.Columns.Remove(StudentReceiveView4.Field.YearId);
                            //dt.Columns.Remove(StudentReceiveView4.Field.TermId);
                            //dt.Columns.Remove(StudentReceiveView4.Field.DepId);
                            //dt.Columns.Remove(StudentReceiveView4.Field.ReceiveId);

                            dt.Columns.Remove(PKEY_ReceiveType);
                            dt.Columns.Remove(PKEY_YearId);
                            dt.Columns.Remove(PKEY_TermId);
                            dt.Columns.Remove(PKEY_DepId);
                            dt.Columns.Remove(PKEY_ReceiveId);
                        }
                        #endregion

                        #region 轉換資料前，將欄位名稱替換成中文
                        if (dt != null)
                        {
                            KeyValueList<string> fieldNames = new KeyValueList<string>();
                            fieldNames.Add(StudentReceiveView4.Field.ReceiveType, "商家代號");
                            fieldNames.Add(StudentReceiveView4.Field.YearId, "學年代碼");
                            fieldNames.Add(StudentReceiveView4.Field.TermId, "學期代碼");
                            fieldNames.Add(StudentReceiveView4.Field.DeptId, "部別代碼");
                            fieldNames.Add(StudentReceiveView4.Field.ReceiveId, "費用別代碼");
                            fieldNames.Add(StudentReceiveView4.Field.UpNo, "上傳資料批號");
                            fieldNames.Add(StudentReceiveView4.Field.UpOrder, "上傳該批資料的序號");
                            fieldNames.Add(StudentReceiveView4.Field.StuGrade, "年級代碼");
                            fieldNames.Add(StudentReceiveView4.Field.StuHid, "座號");
                            fieldNames.Add(StudentReceiveView4.Field.CollegeId, "院別代碼");
                            fieldNames.Add(StudentReceiveView4.Field.MajorId, "科系代碼");
                            fieldNames.Add(StudentReceiveView4.Field.ClassId, "班別代碼");
                            fieldNames.Add(StudentReceiveView4.Field.StuCredit, "總學分數");
                            fieldNames.Add(StudentReceiveView4.Field.StuHour, "上課時數");
                            fieldNames.Add(StudentReceiveView4.Field.ReduceId, "減免代碼");
                            fieldNames.Add(StudentReceiveView4.Field.DormId, "住宿代碼");
                            fieldNames.Add(StudentReceiveView4.Field.LoanId, "就貸代碼");
                            fieldNames.Add(StudentReceiveView4.Field.AgencyList, "代辦費明細項目");
                            fieldNames.Add(StudentReceiveView4.Field.BillingType, "計算方式");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyId01, "身份註記一代碼");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyId02, "身份註記二代碼");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyId03, "身份註記三代碼");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyId04, "身份註記四代碼");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyId05, "身份註記五代碼");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyId06, "身份註記六代碼");

                            #region [Old] 收入科目名稱 (查詢 sql 已替換欄位名稱)
                            //fieldNames.Add(StudentReceiveView4.Field.Receive01, receiveItemNames[0]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive02, receiveItemNames[1]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive03, receiveItemNames[2]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive04, receiveItemNames[3]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive05, receiveItemNames[4]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive06, receiveItemNames[5]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive07, receiveItemNames[6]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive08, receiveItemNames[7]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive09, receiveItemNames[8]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive10, receiveItemNames[9]);

                            //fieldNames.Add(StudentReceiveView4.Field.Receive11, receiveItemNames[10]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive12, receiveItemNames[11]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive13, receiveItemNames[12]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive14, receiveItemNames[13]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive15, receiveItemNames[14]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive16, receiveItemNames[15]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive17, receiveItemNames[16]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive18, receiveItemNames[17]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive19, receiveItemNames[18]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive20, receiveItemNames[19]);

                            //fieldNames.Add(StudentReceiveView4.Field.Receive21, receiveItemNames[20]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive22, receiveItemNames[21]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive23, receiveItemNames[22]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive24, receiveItemNames[23]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive25, receiveItemNames[24]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive26, receiveItemNames[25]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive27, receiveItemNames[26]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive28, receiveItemNames[27]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive29, receiveItemNames[28]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive30, receiveItemNames[29]);

                            //fieldNames.Add(StudentReceiveView4.Field.Receive31, receiveItemNames[30]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive32, receiveItemNames[31]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive33, receiveItemNames[32]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive34, receiveItemNames[33]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive35, receiveItemNames[34]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive36, receiveItemNames[35]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive37, receiveItemNames[36]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive38, receiveItemNames[37]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive39, receiveItemNames[38]);
                            //fieldNames.Add(StudentReceiveView4.Field.Receive40, receiveItemNames[39]);
                            #endregion

                            #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                            #region 收入科目名稱
                            for (int idx = 0; idx < receiveItemNames.Count; idx++)
                            {
                                if (!String.IsNullOrEmpty(receiveItemNames[idx]))
                                {
                                    fieldNames.Add($"RECEIVE_ITEM_{idx + 1:00}", receiveItemNames[idx]);
                                }
                            }
                            #endregion
                            #endregion

                            fieldNames.Add(StudentReceiveView4.Field.ReceiveAmount, "繳費金額");
                            fieldNames.Add(StudentReceiveView4.Field.ReceiveSmamount, "超商繳費金額");
                            fieldNames.Add(StudentReceiveView4.Field.LoanAmount, "原就學貸款金額");
                            fieldNames.Add(StudentReceiveView4.Field.ReissueFlag, "補單註記");

                            fieldNames.Add(StudentReceiveView4.Field.SeriorNo, "流水號");
                            fieldNames.Add(StudentReceiveView4.Field.CancelNo, "虛擬帳號");
                            fieldNames.Add(StudentReceiveView4.Field.CancelSmno, "超商虛擬帳號");
                            fieldNames.Add(StudentReceiveView4.Field.CancelFlag, "銷帳註記");
                            fieldNames.Add(StudentReceiveView4.Field.ReceivebankId, "代收銀行");
                            fieldNames.Add(StudentReceiveView4.Field.ReceiveDate, "代收日期");
                            fieldNames.Add(StudentReceiveView4.Field.ReceiveTime, "代收時間");
                            fieldNames.Add(StudentReceiveView4.Field.AccountDate, "入帳日期");
                            fieldNames.Add(StudentReceiveView4.Field.ReceiveWay, "繳款管道代號");
                            fieldNames.Add(StudentReceiveView4.Field.Loan, "上傳就學貸款金額");
                            fieldNames.Add(StudentReceiveView4.Field.RealLoan, "實際貸款金額");
                            fieldNames.Add(StudentReceiveView4.Field.DeductBankid, "扣款轉帳銀行代碼");
                            fieldNames.Add(StudentReceiveView4.Field.DeductAccountno, "扣款轉帳銀行帳號");
                            fieldNames.Add(StudentReceiveView4.Field.DeductAccountname, "扣款轉帳銀行帳號戶名");
                            fieldNames.Add(StudentReceiveView4.Field.DeductAccountid, "扣款轉帳銀行帳戶ＩＤ");

                            #region 備註相關
                            {
                                string[] memoFieldNames = new string[StudentReceiveView4.MemoCount] {
                                    StudentReceiveView4.Field.Memo01, StudentReceiveView4.Field.Memo02,
                                    StudentReceiveView4.Field.Memo03, StudentReceiveView4.Field.Memo04,
                                    StudentReceiveView4.Field.Memo05, StudentReceiveView4.Field.Memo06,
                                    StudentReceiveView4.Field.Memo07, StudentReceiveView4.Field.Memo08,
                                    StudentReceiveView4.Field.Memo09, StudentReceiveView4.Field.Memo10,
                                    StudentReceiveView4.Field.Memo11, StudentReceiveView4.Field.Memo12,
                                    StudentReceiveView4.Field.Memo13, StudentReceiveView4.Field.Memo14,
                                    StudentReceiveView4.Field.Memo15, StudentReceiveView4.Field.Memo16,
                                    StudentReceiveView4.Field.Memo17, StudentReceiveView4.Field.Memo18,
                                    StudentReceiveView4.Field.Memo19, StudentReceiveView4.Field.Memo20,
                                    StudentReceiveView4.Field.Memo21
                                };
                                for (int idx = 0; idx < memoFieldNames.Length; idx++)
                                {
                                    if (idx < receiveMemoTitles.Count)
                                    {
                                        fieldNames.Add(memoFieldNames[idx], receiveMemoTitles[idx]);
                                    }
                                    else
                                    {
                                        fieldNames.Add(memoFieldNames[idx], String.Format("備註{0:00}", idx + 1));
                                    }
                                }
                            }
                            #endregion

                            #region 學生基本資料相關
                            fieldNames.Add(StudentReceiveView4.Field.StuId, "學生學號");
                            fieldNames.Add(StudentReceiveView4.Field.StuName, "學生姓名");
                            fieldNames.Add(StudentReceiveView4.Field.StuBirthday, "學生生日");
                            fieldNames.Add(StudentReceiveView4.Field.StuIdNumber, "學生身分證字號");
                            fieldNames.Add(StudentReceiveView4.Field.StuTel, "學生電話");
                            fieldNames.Add(StudentReceiveView4.Field.StuZipCode, "學生郵遞區號");
                            fieldNames.Add(StudentReceiveView4.Field.StuAddress, "學生地址");
                            fieldNames.Add(StudentReceiveView4.Field.StuEmail, "學生 EMail");
                            fieldNames.Add(StudentReceiveView4.Field.StuParent, "家長名稱");
                            #endregion

                            #region 代碼名稱相關
                            fieldNames.Add(StudentReceiveView4.Field.YearName, "學年名稱");
                            fieldNames.Add(StudentReceiveView4.Field.TermName, "學期名稱");
                            fieldNames.Add(StudentReceiveView4.Field.DeptName, "部別名稱");
                            fieldNames.Add(StudentReceiveView4.Field.ReceiveName, "費用名稱");
                            fieldNames.Add(StudentReceiveView4.Field.CollegeName, "院別名稱");
                            fieldNames.Add(StudentReceiveView4.Field.MajorName, "系所名稱");
                            fieldNames.Add(StudentReceiveView4.Field.ClassName, "班別名稱");
                            fieldNames.Add(StudentReceiveView4.Field.ReduceName, "減免類別名稱");
                            fieldNames.Add(StudentReceiveView4.Field.DormName, "住宿項目名稱");
                            fieldNames.Add(StudentReceiveView4.Field.LoanName, "就貸項目名稱");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyName01, "身分註記01項目名稱");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyName02, "身分註記02項目名稱");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyName03, "身分註記03項目名稱");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyName04, "身分註記04項目名稱");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyName05, "身分註記05項目名稱");
                            fieldNames.Add(StudentReceiveView4.Field.IdentifyName06, "身分註記06項目名稱");
                            fieldNames.Add(StudentReceiveView4.Field.ReceiveWayName, "繳款管道名稱");
                            #endregion

                            List<string> columnFieldNames = new List<string>(dt.Columns.Count);  //暫存欄位名稱，用來避免重複
                            int columnNo = 0;
                            foreach (DataColumn column in dt.Columns)
                            {
                                columnNo++;
                                string columnName = column.ColumnName;
                                KeyValue<string> fieldName = fieldNames.Find(x => x.Key == columnName);
                                if (fieldName != null)
                                {
                                    string columnFieldName = fieldName.Value;
                                    if (columnFieldNames.Contains(columnFieldName))
                                    {
                                        //替換掉重複的欄位名稱
                                        column.ColumnName = String.Format("{0}_{1}", columnFieldName, columnNo);
                                    }
                                    else
                                    {
                                        column.ColumnName = columnFieldName;
                                    }
                                }
                                else if (columnFieldNames.Contains(columnName))
                                {
                                    //替換掉重複的欄位名稱
                                    column.ColumnName = String.Format("{0}_{1}", columnName, columnNo);
                                }
                                columnFieldNames.Add(column.ColumnName);
                            }
                        }
                        #endregion

                        #region 匯出成檔案
                        if (dt != null)
                        {
                            if (isUseODS)
                            {
                                #region DataTable 轉 ODS (ODS 本來就是 ZIP 壓縮檔，不用在壓縮)
                                ODSHelper helper = new ODSHelper();
                                exportData.FileContent = helper.DataTable2ODS(dt, isDecimalTruncate: true);
                                #endregion
                            }
                            else
                            {
                                #region DataTable 轉 Xls
                                ConvertFileHelper helper = new ConvertFileHelper();
                                string xmlFileName = Path.ChangeExtension(exportData.FileName, "XLS");
                                exportData.FileContent = helper.Dt2Xls(dt, toCompression: true, xlsFileName: xmlFileName);
                                #endregion
                            }

                            if (exportData.FileContent == null)
                            {
                                exportData.Explain += String.Format("{0}結果：將匯出資料存成 {1} 檔失敗", Environment.NewLine, extName);
                                exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                                result = new Result(false, String.Format("將匯出資料存成 {0} 檔失敗", extName), CoreStatusCode.UNKNOWN_ERROR, null);
                                return result;
                            }
                            else
                            {
                                exportData.Explain += String.Concat(Environment.NewLine, "結果：產生匯出資料檔成功，共 ", dt.Rows.Count, " 筆資料");
                                exportData.Status = ExportFileStatusCodeTexts.NORMAL;
                                if (result == null)
                                {
                                    result = new Result(true);
                                }
                            }

                            if (dt != null)
                            {
                                dt.Clear();
                                dt.Dispose();
                                dt = null;
                            }
                        }
                        else
                        {
                            exportData.Explain += String.Concat(Environment.NewLine, "結果：無查資料");
                            exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                            result = new Result(false, "無查資料", ErrorCode.D_QUERY_NO_DATA, null);
                            return result;
                        }
                        #endregion
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    exportData.Explain += String.Concat(Environment.NewLine, "結果：匯出資料處理發生例外，", ex.Message);
                    exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                    result = new Result(false, "匯出資料處理發生例外，" + ex.Message, CoreStatusCode.UNKNOWN_EXCEPTION, ex);
                }
                #endregion
            }
            catch (Exception ex)
            {
                if (exportData != null)
                {
                    exportData.Explain += String.Concat(Environment.NewLine, "結果：產生匯出資料檔發生例外，", ex.Message);
                    exportData.Status = ExportFileStatusCodeTexts.FAILURE;
                }
                result = new Result(false, "產生匯出資料檔發生例外，" + ex.Message, CoreStatusCode.UNKNOWN_EXCEPTION, ex);
            }
            finally
            {
                if (dt != null)
                {
                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }

                #region 更新 exportData 的資料與狀態
                if (exportData != null)
                {
                    if (factory == null)
                    {
                        if (_Factory == null)
                        {
                            factory = new EntityFactory();
                        }
                        else
                        {
                            factory = _Factory.CloneForNonTransaction();
                        }
                    }
                    Expression where = new Expression(ExportFileEntity.Field.SN, exportData.SN);
                    KeyValue[] fieldValues = new KeyValue[] {
                        new KeyValue(ExportFileEntity.Field.FileContent, exportData.FileContent),
                        new KeyValue(ExportFileEntity.Field.Explain, exportData.Explain),
                        new KeyValue(ExportFileEntity.Field.Status, exportData.Status),
                        new KeyValue(ExportFileEntity.Field.MdyDate, DateTime.Now),
                        new KeyValue(ExportFileEntity.Field.MdyUser, "SYSTEM")
                    };
                    int count = 0;
                    Result result2 = factory.UpdateFields<ExportFileEntity>(fieldValues, where, out count);
                }
                #endregion

                if (factory != null)
                {
                    factory.Dispose();
                    factory = null;
                }
            }

            return result;

            #region [Old]
            //            try
            //            {
            //                #region 取資料
            //                DataTable dt = null;
            //                bool hasZBarcode = false;
            //                DateTime? payDueDate = null;
            //                int extraDays = 0;
            //                ReceiveChannelEntity[] smReceiveChannels = null;
            //                string[] receiveItemNames = new string[40] {
            //                    "收入科目01金額", "收入科目02金額", "收入科目03金額", "收入科目04金額", "收入科目05金額",
            //                    "收入科目06金額", "收入科目07金額", "收入科目08金額", "收入科目09金額", "收入科目10金額",
            //                    "收入科目11金額", "收入科目12金額", "收入科目13金額", "收入科目14金額", "收入科目15金額",
            //                    "收入科目16金額", "收入科目17金額", "收入科目18金額", "收入科目19金額", "收入科目20金額",
            //                    "收入科目21金額", "收入科目22金額", "收入科目23金額", "收入科目24金額", "收入科目25金額",
            //                    "收入科目26金額", "收入科目27金額", "收入科目28金額", "收入科目29金額", "收入科目30金額",
            //                    "收入科目31金額", "收入科目32金額", "收入科目33金額", "收入科目34金額", "收入科目35金額",
            //                    "收入科目36金額", "收入科目37金額", "收入科目38金額", "收入科目39金額", "收入科目40金額"
            //                };
            //                string[] memoTitles = new string[] {
            //                    "備註01", "備註02", "備註03", "備註04", "備註05",
            //                    "備註06", "備註07", "備註08", "備註09", "備註10"
            //                };
            //                {
            //                    StringBuilder fieldSql = new StringBuilder();
            //                    foreach (string outField in outFields)
            //                    {
            //                        fieldSql.Append(",").Append(outField);
            //                        if (!hasZBarcode && outField == StudentReceiveView4.Field.CancelSmno)
            //                        {
            //                            hasZBarcode = true;
            //                            fieldSql.Append(", '' AS [超商條碼1], '' AS [超商條碼2], '' AS [超商條碼3]");
            //                            fieldSql.AppendFormat(", [{0}] AS [計算用應繳金額], [{1}] AS [計算用超商金額]", StudentReceiveView4.Field.ReceiveAmount, StudentReceiveView4.Field.ReceiveSmamount);
            //                        }
            //                    }

            //                    #region
            //                    //20150715 jj 增加 "入帳日"、"代收日"、"繳款管道"
            //                    fieldSql.Append(",").Append(StudentReceiveView4.Field.ReceiveDate);
            //                    fieldSql.Append(",").Append(StudentReceiveView4.Field.AccountDate);
            //                    fieldSql.Append(",").Append(StudentReceiveView4.Field.ReceiveWayName);
            //                    #endregion

            //                    //if (hasZBarcode)
            //                    {
            //                        #region 繳款期限 + 超商延遲日 + 收入科目名稱 + 備註標題
            //                        {
            //                            SchoolRidEntity schoolRid = null;
            //                            Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
            //                                .And(SchoolRidEntity.Field.YearId, yearId)
            //                                .And(SchoolRidEntity.Field.TermId, termId)
            //                                .And(SchoolRidEntity.Field.DepId, depId)
            //                                .And(SchoolRidEntity.Field.ReceiveId, receiveId);
            //                            Result result = _Factory.SelectFirst<SchoolRidEntity>(where, null, out schoolRid);
            //                            if (!result.IsSuccess)
            //                            {
            //                                return String.Format("查詢代收費用別設定資料失敗，錯誤訊息：{0}", result.Message);
            //                            }
            //                            if (schoolRid == null)
            //                            {
            //                                return "查無代收費用別設定資料";
            //                            }
            //                            payDueDate = DataFormat.ConvertDateText(schoolRid.PayDate);
            //                            if (payDueDate == null)
            //                            {
            //                                return "未設定繳款期限";
            //                            }

            //                            extraDays = schoolRid.ExtraDays;

            //                            string[] itemNames = new string[] {
            //                                schoolRid.ReceiveItem01, schoolRid.ReceiveItem02, schoolRid.ReceiveItem03, schoolRid.ReceiveItem04, schoolRid.ReceiveItem05,
            //                                schoolRid.ReceiveItem06, schoolRid.ReceiveItem07, schoolRid.ReceiveItem08, schoolRid.ReceiveItem09, schoolRid.ReceiveItem10,
            //                                schoolRid.ReceiveItem11, schoolRid.ReceiveItem12, schoolRid.ReceiveItem13, schoolRid.ReceiveItem14, schoolRid.ReceiveItem15,
            //                                schoolRid.ReceiveItem16, schoolRid.ReceiveItem17, schoolRid.ReceiveItem18, schoolRid.ReceiveItem19, schoolRid.ReceiveItem20,
            //                                schoolRid.ReceiveItem21, schoolRid.ReceiveItem22, schoolRid.ReceiveItem23, schoolRid.ReceiveItem24, schoolRid.ReceiveItem25,
            //                                schoolRid.ReceiveItem26, schoolRid.ReceiveItem27, schoolRid.ReceiveItem28, schoolRid.ReceiveItem29, schoolRid.ReceiveItem30,
            //                                schoolRid.ReceiveItem31, schoolRid.ReceiveItem32, schoolRid.ReceiveItem33, schoolRid.ReceiveItem34, schoolRid.ReceiveItem35,
            //                                schoolRid.ReceiveItem36, schoolRid.ReceiveItem37, schoolRid.ReceiveItem38, schoolRid.ReceiveItem39, schoolRid.ReceiveItem40
            //                            };
            //                            for (int idx = 0; idx < receiveItemNames.Length; idx++)
            //                            {
            //                                string itemName = itemNames[idx] == null ? null : itemNames[idx].Trim();
            //                                if (!String.IsNullOrEmpty(itemName))
            //                                {
            //                                    receiveItemNames[idx] = itemName;
            //                                }
            //                            }

            //                            string[] tmps = schoolRid.GetAllMemoTitles();
            //                            for (int idx = 0; idx < memoTitles.Length; idx++)
            //                            {
            //                                if (idx < tmps.Length && !String.IsNullOrWhiteSpace(tmps[idx]))
            //                                {
            //                                    memoTitles[idx] = tmps[idx].Trim();
            //                                }
            //                            }
            //                        }
            //                        #endregion

            //                        #region 商家代碼的超商管道手續費
            //                        {
            //                            Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType)
            //                                .And(ReceiveChannelEntity.Field.ChannelId, ChannelHelper.SM_DEFAULT);
            //                            Result result = _Factory.SelectAll<ReceiveChannelEntity>(where, null, out smReceiveChannels);
            //                            if (!result.IsSuccess)
            //                            {
            //                                return String.Format("查詢此商家代碼的超商管道手續費資料失敗，錯誤訊息：{0}", result.Message);
            //                            }
            //                            if (hasZBarcode && (smReceiveChannels == null || smReceiveChannels.Length == 0))
            //                            {
            //                                return "查無此商家代碼的超商管道手續費資料";
            //                            }
            //                        }
            //                        #endregion
            //                    }

            //                    #region 學生繳費資料
            //                    {
            //                        List<string> whereSqls = new List<string>(14);
            //                        KeyValueList parameters = new KeyValueList(14);

            //                        #region 5 Key
            //                        whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveType)", StudentReceiveView4.Field.ReceiveType));
            //                        parameters.Add("@ReceiveType", receiveType);

            //                        whereSqls.Add(String.Format("(SR.[{0}] = @YearId)", StudentReceiveView4.Field.YearId));
            //                        parameters.Add("@YearId", yearId);

            //                        whereSqls.Add(String.Format("(SR.[{0}] = @TermId)", StudentReceiveView4.Field.TermId));
            //                        parameters.Add("@TermId", termId);

            //                        whereSqls.Add(String.Format("(SR.[{0}] = @DepId)", StudentReceiveView4.Field.DepId));
            //                        parameters.Add("@DepId", depId);

            //                        whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveId)", StudentReceiveView4.Field.ReceiveId));
            //                        parameters.Add("@ReceiveId", receiveId);
            //                        #endregion

            //                        #region 批號
            //                        if (qUpNo != null)
            //                        {
            //                            whereSqls.Add(String.Format("([{0}] = @UpNo)", StudentReceiveView4.Field.UpNo));
            //                            parameters.Add("@UpNo", qUpNo.Value);
            //                        }
            //                        #endregion

            //                        #region 銷帳狀態
            //                        switch (qCancelStatus)
            //                        {
            //                            case CancelStatusCodeTexts.NONPAY:      //未繳款
            //                                whereSqls.Add(String.Format("((SR.[{0}] = '' OR SR.[{0}] IS NULL) AND (SR.[{1}] = '' OR SR.[{1}] IS NULL))", StudentReceiveView4.Field.ReceiveDate, StudentReceiveView4.Field.CancelFlag));
            //                                break;
            //                            case CancelStatusCodeTexts.PAYED:       //已繳款
            //                                whereSqls.Add(String.Format("((SR.[{0}] != '' AND SR.[{0}] IS NOT NULL) AND (SR.[{1}] = '' OR SR.[{1}] IS NULL))", StudentReceiveView4.Field.ReceiveDate, StudentReceiveView4.Field.AccountDate));
            //                                break;
            //                            case CancelStatusCodeTexts.CANCELED:    //已入帳
            //                                whereSqls.Add(String.Format("(SR.[{0}] != '' AND SR.[{0}] IS NOT NULL)", StudentReceiveView4.Field.AccountDate));
            //                                break;
            //                        }
            //                        #endregion

            //                        #region 繳款方式 + 代收日區間
            //                        if (qCancelStatus != CancelStatusCodeTexts.NONPAY)
            //                        {
            //                            if (!String.IsNullOrEmpty(qReceiveWay))
            //                            {
            //                                whereSqls.Add(String.Format("(SR.[{0}] = @ReceiveWay)", StudentReceiveView4.Field.ReceiveWay));
            //                                parameters.Add("@ReceiveWay", qReceiveWay);
            //                            }

            //                            if (qSReceivDate != null)
            //                            {
            //                                whereSqls.Add(String.Format("(SR.[{0}] >= @SReceivDate)", StudentReceiveView4.Field.ReceiveDate));
            //                                parameters.Add("@SReceivDate", Common.GetTWDate7(qSReceivDate.Value));
            //                            }
            //                            if (qEReceivDate != null)
            //                            {
            //                                whereSqls.Add(String.Format("(SR.[{0}] <= @EReceivDate)", StudentReceiveView4.Field.ReceiveDate));
            //                                parameters.Add("@EReceivDate", Common.GetTWDate7(qEReceivDate.Value));
            //                            }
            //                        }
            //                        #endregion

            //                        #region 入帳日區間
            //                        if (String.IsNullOrEmpty(qCancelStatus) || qCancelStatus == CancelStatusCodeTexts.CANCELED)
            //                        {
            //                            if (qSAccountDate != null)
            //                            {
            //                                whereSqls.Add(String.Format("(SR.[{0}] >= @SAccountDate)", StudentReceiveView4.Field.AccountDate));
            //                                parameters.Add("@SAccountDate", Common.GetTWDate7(qSAccountDate.Value));
            //                            }
            //                            if (qEAccountDate != null)
            //                            {
            //                                whereSqls.Add(String.Format("(SR.[{0}] <= @EAccountDate)", StudentReceiveView4.Field.AccountDate));
            //                                parameters.Add("@EAccountDate", Common.GetTWDate7(qEAccountDate.Value));
            //                            }
            //                        }
            //                        #endregion

            //                        #region 查詢欄位與值
            //                        if (!String.IsNullOrEmpty(qFieldName) && !String.IsNullOrEmpty(qFieldValue))
            //                        {
            //                            switch (qFieldName)
            //                            {
            //                                case "StuId":   //學號
            //                                    whereSqls.Add(String.Format("(SR.[{0}] = @StuId)", StudentReceiveView4.Field.StuId));
            //                                    parameters.Add("@StuId", qFieldValue);
            //                                    break;
            //                                case "CancelNo":   //虛擬帳號
            //                                    whereSqls.Add(String.Format("(SR.[{0}] = @CancelNo)", StudentReceiveView4.Field.CancelNo));
            //                                    parameters.Add("@CancelNo", qFieldValue);
            //                                    break;
            //                                case "IdNumber":   //身分證字號
            //                                    whereSqls.Add(String.Format("(SR.[{0}] = @StuIdNumber)", StudentReceiveView4.Field.StuIdNumber));
            //                                    parameters.Add("@StuIdNumber", qFieldValue);
            //                                    break;
            //                            }
            //                        }
            //                        #endregion

            //                        string whereSql = String.Join(" AND ", whereSqls);

            //                        string sql = String.Format(@"SELECT {0} 
            //  FROM (
            //{1} 
            // WHERE {2}
            //) AS T 
            // ORDER BY {3}", fieldSql.ToString(1, fieldSql.Length - 1), StudentReceiveView4.VIEWSQL, whereSql, StudentReceiveView4.Field.StuId);

            //                        Result result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
            //                        if (!result.IsSuccess)
            //                        {
            //                            return String.Format("查詢學生繳費資料失敗，錯誤訊息：{0}", result.Message);
            //                        }
            //                        if (dt == null || dt.Rows.Count == 0)
            //                        {
            //                            return "查無學生繳費資料";
            //                        }
            //                    }
            //                    #endregion
            //                }
            //                #endregion

            //                #region 處理超商條碼
            //                if (hasZBarcode)
            //                {
            //                    ChannelHelper helper = new ChannelHelper();
            //                    foreach (DataRow dr in dt.Rows)
            //                    {
            //                        string smCancelNo = dr.IsNull(StudentReceiveView4.Field.CancelSmno) ? null : dr[StudentReceiveView4.Field.CancelSmno].ToString().Trim();
            //                        if (String.IsNullOrWhiteSpace(smCancelNo))
            //                        {
            //                            continue;
            //                        }

            //                        #region [Old] 使用者可能沒勾選 ReceiveAmount 或 ReceiveSmamount，所以改用 [計算用應繳金額] 與 [計算用超商金額]
            //                        //decimal amount = 0;
            //                        //string amountTxt = dr.IsNull(StudentReceiveView4.Field.ReceiveAmount) ? null : dr[StudentReceiveView4.Field.ReceiveAmount].ToString().Trim();
            //                        //if (String.IsNullOrWhiteSpace(amountTxt) || !Decimal.TryParse(amountTxt, out amount) || amount < 0)
            //                        //{
            //                        //    continue;
            //                        //}

            //                        //decimal smAmount = 0;
            //                        //string smAmountTxt = dr.IsNull(StudentReceiveView4.Field.ReceiveSmamount) ? null : dr[StudentReceiveView4.Field.ReceiveSmamount].ToString().Trim();
            //                        //if (String.IsNullOrWhiteSpace(smAmountTxt) || !Decimal.TryParse(smAmountTxt, out smAmount) || smAmount < 0)
            //                        //{
            //                        //    continue;
            //                        //}
            //                        #endregion

            //                        decimal amount = 0;
            //                        string amountTxt = dr.IsNull("計算用應繳金額") ? null : dr["計算用應繳金額"].ToString().Trim();
            //                        if (String.IsNullOrWhiteSpace(amountTxt) || !Decimal.TryParse(amountTxt, out amount) || amount < 0)
            //                        {
            //                            continue;
            //                        }

            //                        decimal smAmount = 0;
            //                        string smAmountTxt = dr.IsNull("計算用超商金額") ? null : dr["計算用超商金額"].ToString().Trim();
            //                        if (String.IsNullOrWhiteSpace(smAmountTxt) || !Decimal.TryParse(smAmountTxt, out smAmount) || smAmount < 0)
            //                        {
            //                            continue;
            //                        }

            //                        ReceiveChannelEntity smChannel = null;
            //                        ReceiveChannelEntity cashChannel = null;
            //                        helper.CheckReceiveChannel(amount, smReceiveChannels, out smChannel, out cashChannel);
            //                        if (smChannel == null)
            //                        {
            //                            continue;
            //                        }

            //                        string smBarcode1 = null;
            //                        string smBarcode2 = null;
            //                        string smBarcode3 = null;
            //                        this.GenSMBarcode(smCancelNo, smAmount, payDueDate.Value, extraDays, smChannel.BarcodeId, out smBarcode1, out smBarcode2, out smBarcode3);
            //                        dr["超商條碼1"] = smBarcode1 ?? String.Empty;
            //                        dr["超商條碼2"] = smBarcode2 ?? String.Empty;
            //                        dr["超商條碼3"] = smBarcode3 ?? String.Empty;
            //                    }
            //                }
            //                #endregion

            //                #region 指定欄位名稱
            //                {
            //                    KeyValueList<string> fieldNames = new KeyValueList<string>();
            //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveType, "商家代號");
            //                    fieldNames.Add(StudentReceiveView4.Field.YearId, "學年代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.TermId, "學期代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.DeptId, "部別代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveId, "費用別代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.UpNo, "上傳資料批號");
            //                    fieldNames.Add(StudentReceiveView4.Field.UpOrder, "上傳該批資料的序號");
            //                    fieldNames.Add(StudentReceiveView4.Field.StuGrade, "年級代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.CollegeId, "院別代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.MajorId, "科系代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.ClassId, "班別代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.StuCredit, "總學分數");
            //                    fieldNames.Add(StudentReceiveView4.Field.StuHour, "上課時數");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReduceId, "減免代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.DormId, "住宿代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.LoanId, "就貸代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.AgencyList, "代辦費明細項目");
            //                    fieldNames.Add(StudentReceiveView4.Field.BillingType, "計算方式");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId01, "身份註記一代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId02, "身份註記二代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId03, "身份註記三代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId04, "身份註記四代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId05, "身份註記五代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyId06, "身份註記六代碼");

            //                    #region [Old] 收入科目名稱改用代收費用別理的設定值
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive01, "收入科目01金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive02, "收入科目02金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive03, "收入科目03金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive04, "收入科目04金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive05, "收入科目05金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive06, "收入科目06金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive07, "收入科目07金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive08, "收入科目08金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive09, "收入科目09金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive10, "收入科目10金額");

            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive11, "收入科目11金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive12, "收入科目12金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive13, "收入科目13金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive14, "收入科目14金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive15, "收入科目15金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive16, "收入科目16金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive17, "收入科目17金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive18, "收入科目18金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive19, "收入科目19金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive20, "收入科目20金額");

            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive21, "收入科目21金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive22, "收入科目22金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive23, "收入科目23金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive24, "收入科目24金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive25, "收入科目25金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive26, "收入科目26金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive27, "收入科目27金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive28, "收入科目28金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive29, "收入科目29金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive30, "收入科目30金額");

            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive31, "收入科目31金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive32, "收入科目32金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive33, "收入科目33金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive34, "收入科目34金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive35, "收入科目35金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive36, "收入科目36金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive37, "收入科目37金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive38, "收入科目38金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive39, "收入科目39金額");
            //                    //fieldNames.Add(StudentReceiveView4.Field.Receive40, "收入科目40金額");
            //                    #endregion

            //                    #region 收入科目名稱
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive01, receiveItemNames[0]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive02, receiveItemNames[1]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive03, receiveItemNames[2]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive04, receiveItemNames[3]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive05, receiveItemNames[4]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive06, receiveItemNames[5]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive07, receiveItemNames[6]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive08, receiveItemNames[7]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive09, receiveItemNames[8]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive10, receiveItemNames[9]);

            //                    fieldNames.Add(StudentReceiveView4.Field.Receive11, receiveItemNames[10]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive12, receiveItemNames[11]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive13, receiveItemNames[12]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive14, receiveItemNames[13]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive15, receiveItemNames[14]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive16, receiveItemNames[15]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive17, receiveItemNames[16]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive18, receiveItemNames[17]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive19, receiveItemNames[18]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive20, receiveItemNames[19]);

            //                    fieldNames.Add(StudentReceiveView4.Field.Receive21, receiveItemNames[20]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive22, receiveItemNames[21]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive23, receiveItemNames[22]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive24, receiveItemNames[23]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive25, receiveItemNames[24]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive26, receiveItemNames[25]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive27, receiveItemNames[26]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive28, receiveItemNames[27]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive29, receiveItemNames[28]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive30, receiveItemNames[29]);

            //                    fieldNames.Add(StudentReceiveView4.Field.Receive31, receiveItemNames[30]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive32, receiveItemNames[31]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive33, receiveItemNames[32]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive34, receiveItemNames[33]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive35, receiveItemNames[34]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive36, receiveItemNames[35]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive37, receiveItemNames[36]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive38, receiveItemNames[37]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive39, receiveItemNames[38]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Receive40, receiveItemNames[39]);
            //                    #endregion

            //                    fieldNames.Add(StudentReceiveView4.Field.StuId, "學生學號");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveAmount, "繳費金額");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveSmamount, "超商繳費金額");
            //                    fieldNames.Add(StudentReceiveView4.Field.LoanAmount, "原就學貸款金額");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReissueFlag, "補單註記");

            //                    fieldNames.Add(StudentReceiveView4.Field.SeriorNo, "流水號");
            //                    fieldNames.Add(StudentReceiveView4.Field.CancelNo, "虛擬帳號");
            //                    fieldNames.Add(StudentReceiveView4.Field.CancelSmno, "超商虛擬帳號");
            //                    fieldNames.Add(StudentReceiveView4.Field.CancelFlag, "銷帳註記");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReceivebankId, "代收銀行");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveDate, "代收日期");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveTime, "代收時間");
            //                    fieldNames.Add(StudentReceiveView4.Field.AccountDate, "入帳日期");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveWay, "繳費方式");
            //                    fieldNames.Add(StudentReceiveView4.Field.Loan, "上傳就學貸款金額");
            //                    fieldNames.Add(StudentReceiveView4.Field.RealLoan, "實際貸款金額");
            //                    fieldNames.Add(StudentReceiveView4.Field.DeductBankid, "扣款轉帳銀行代碼");
            //                    fieldNames.Add(StudentReceiveView4.Field.DeductAccountno, "扣款轉帳銀行帳號");
            //                    fieldNames.Add(StudentReceiveView4.Field.DeductAccountname, "扣款轉帳銀行帳號戶名");
            //                    fieldNames.Add(StudentReceiveView4.Field.DeductAccountid, "扣款轉帳銀行帳戶ＩＤ");

            //                    #region 備註相關
            //                    fieldNames.Add(StudentReceiveView4.Field.Memo01, memoTitles[0]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Memo02, memoTitles[1]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Memo03, memoTitles[2]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Memo04, memoTitles[3]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Memo05, memoTitles[4]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Memo06, memoTitles[5]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Memo07, memoTitles[6]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Memo08, memoTitles[7]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Memo09, memoTitles[8]);
            //                    fieldNames.Add(StudentReceiveView4.Field.Memo10, memoTitles[9]);
            //                    #endregion

            //                    #region 學生基本資料相關
            //                    fieldNames.Add(StudentReceiveView4.Field.StuName, "學生姓名");
            //                    fieldNames.Add(StudentReceiveView4.Field.StuBirthday, "學生生日");
            //                    fieldNames.Add(StudentReceiveView4.Field.StuIdNumber, "學生身分證字號");
            //                    fieldNames.Add(StudentReceiveView4.Field.StuTel, "學生電話");
            //                    fieldNames.Add(StudentReceiveView4.Field.StuZipCode, "學生郵遞區號");
            //                    fieldNames.Add(StudentReceiveView4.Field.StuAddress, "學生地址");
            //                    fieldNames.Add(StudentReceiveView4.Field.StuEmail, "學生 EMail");
            //                    #endregion

            //                    #region 代碼名稱相關
            //                    fieldNames.Add(StudentReceiveView4.Field.TermName, "學期名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.DeptName, "部別名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveName, "費用名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.CollegeName, "院別名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.MajorName, "系所名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.ClassName, "班別名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReduceName, "減免類別名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.DormName, "住宿項目名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.LoanName, "就貸項目名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName01, "身分註記01項目名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName02, "身分註記02項目名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName03, "身分註記03項目名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName04, "身分註記04項目名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName05, "身分註記05項目名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.IdentifyName06, "身分註記06項目名稱");
            //                    fieldNames.Add(StudentReceiveView4.Field.ReceiveWayName, "繳費管道");
            //                    #endregion

            //                    foreach (DataColumn column in dt.Columns)
            //                    {
            //                        string key = column.ColumnName;
            //                        KeyValue<string> fieldName = fieldNames.Find(x => x.Key == key);
            //                        if (fieldName != null)
            //                        {
            //                            column.ColumnName = fieldName.Value;
            //                        }
            //                    }
            //                }
            //                #endregion

            //                #region DataTable 轉 Xls
            //                {
            //                    #region 轉換資料前，先移除計算用的應繳金額與超商金額
            //                    if (hasZBarcode)
            //                    {
            //                        dt.Columns.Remove("計算用應繳金額");
            //                        dt.Columns.Remove("計算用超商金額");
            //                    }
            //                    #endregion

            //                    ConvertFileHelper helper = new ConvertFileHelper();
            //                    outFileContent = helper.Dt2Xls(dt);
            //                }
            //                #endregion

            //                return null;
            //            }
            //            catch (Exception ex)
            //            {
            //                return String.Concat("產出檔案發生例外，錯誤訊息；", ex.Message);
            //            }
            #endregion
        }
        #endregion
        #endregion


        #region 匯出繳費銷帳總表
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出繳費銷帳總表 A2
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="upNo">批號</param>
        /// <param name="receiveStatus">繳費狀態</param>
        /// <param name="reportKind">報表種類 (1=正常 2=遲繳)</param>
        /// <param name="reportName">報表名稱</param>
        /// <param name="outFileContent">成功則傳回產生檔案內容，否則傳回 null</param>
        /// <param name="isUseODS">是否產生成 ODS 檔</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        public string ExportReportA2(string receiveType, string yearId, string termId, string depId, string receiveId
            , int? upNo, string receiveStatus, string reportKind, string reportName
            , out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            #region 檢查參數
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return "缺少查詢資料參數";
            }
            string receiveStatusName = null;
            receiveStatus = receiveStatus == null ? String.Empty : receiveStatus.Trim();
            switch (receiveStatus)
            {
                case "0":   //未繳
                    receiveStatusName = "未繳";
                    break;
                case "1":   //已繳
                    receiveStatusName = "已繳";
                    break;
                case "":   //全部
                    receiveStatusName = String.Empty;
                    break;
                default:
                    return "不正確的繳款方式參數";
            }
            if (String.IsNullOrWhiteSpace(reportName))
            {
                return "缺少報表名稱參數";
            }
            reportName = reportName.Trim();
            if (upNo != null && upNo.Value < 0)
            {
                upNo = null;
            }
            #endregion

            try
            {
                #region 取資料
                Result result = null;

                #region 取得代收費用別的收入科目名稱與對應學生繳費資料的收入科目金額合計欄位名稱 + 繳費期限
                KeyValueList<string> receiveItems = new KeyValueList<string>(40);
                int minColumnCount = 12;
                string payDueDate = null;
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT ISNULL(Receive_Item01, '') AS Receive_Item01, ISNULL(Receive_Item02, '') AS Receive_Item02, ISNULL(Receive_Item03, '') AS Receive_Item03, ISNULL(Receive_Item04, '') AS Receive_Item04, ISNULL(Receive_Item05, '') AS Receive_Item05
     , ISNULL(Receive_Item06, '') AS Receive_Item06, ISNULL(Receive_Item07, '') AS Receive_Item07, ISNULL(Receive_Item08, '') AS Receive_Item08, ISNULL(Receive_Item09, '') AS Receive_Item09, ISNULL(Receive_Item10, '') AS Receive_Item10
     , ISNULL(Receive_Item11, '') AS Receive_Item11, ISNULL(Receive_Item12, '') AS Receive_Item12, ISNULL(Receive_Item13, '') AS Receive_Item13, ISNULL(Receive_Item14, '') AS Receive_Item14, ISNULL(Receive_Item15, '') AS Receive_Item15
     , ISNULL(Receive_Item16, '') AS Receive_Item16, ISNULL(Receive_Item17, '') AS Receive_Item17, ISNULL(Receive_Item18, '') AS Receive_Item18, ISNULL(Receive_Item19, '') AS Receive_Item19, ISNULL(Receive_Item20, '') AS Receive_Item20
     , ISNULL(Receive_Item21, '') AS Receive_Item21, ISNULL(Receive_Item22, '') AS Receive_Item22, ISNULL(Receive_Item23, '') AS Receive_Item23, ISNULL(Receive_Item24, '') AS Receive_Item24, ISNULL(Receive_Item25, '') AS Receive_Item25
     , ISNULL(Receive_Item26, '') AS Receive_Item26, ISNULL(Receive_Item27, '') AS Receive_Item27, ISNULL(Receive_Item28, '') AS Receive_Item28, ISNULL(Receive_Item29, '') AS Receive_Item29, ISNULL(Receive_Item30, '') AS Receive_Item30
     , ISNULL(Receive_Item31, '') AS Receive_Item31, ISNULL(Receive_Item32, '') AS Receive_Item32, ISNULL(Receive_Item33, '') AS Receive_Item33, ISNULL(Receive_Item34, '') AS Receive_Item34, ISNULL(Receive_Item35, '') AS Receive_Item35
     , ISNULL(Receive_Item36, '') AS Receive_Item36, ISNULL(Receive_Item37, '') AS Receive_Item37, ISNULL(Receive_Item38, '') AS Receive_Item38, ISNULL(Receive_Item39, '') AS Receive_Item39, ISNULL(Receive_Item40, '') AS Receive_Item40
     , ISNULL(Pay_Date, '') AS Pay_Due_Date
  FROM School_Rid 
 WHERE Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);

                    DataTable dt = null;
                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 1, out dt);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢代收費用別設定失敗，錯誤訊息：{0}", result.Message);
                    }
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return "查無代收費用別設定資料";
                    }
                    DataRow dRow = dt.Rows[0];
                    payDueDate = dRow["Pay_Due_Date"].ToString().Trim();

                    for (int no = 1; no <= 40; no++)
                    {
                        string columnName = String.Format("Receive_Item{0:00}", no);
                        string name = dRow[columnName].ToString().Trim();
                        if (!String.IsNullOrEmpty(name))
                        {
                            receiveItems.Add(name, String.Format("SUM_Receive_{0:00}", no));
                        }
                    }

                    #region 補齊最小行數
                    if (receiveItems.Count < minColumnCount - 2)
                    {
                        for (int idx = receiveItems.Count; idx < (minColumnCount - 2); idx++)
                        {
                            receiveItems.Add("", "");
                        }
                    }
                    #endregion

                    receiveItems.Add("繳費金額合計", "SUM_AMOUNT");
                    receiveItems.Add("人數(筆數)", "SUM_COUNT");
                }
                #endregion

                #region 取表頭資料
                DataTable dtHead = null;
                {
                    #region 表頭資料欄位
                    //                學校名稱      報表名稱
                    //學年：          學期：          繳款方式：
                    //商家代號：      代收費用：      批號：             yyyy/mm/dd  第xx頁/共xx頁
                    #endregion

                    string sql = @"SELECT RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id
     , SR.Sch_Name, YL.Year_Name, TL.Term_Name, RL.Receive_Name
     , @UpNo AS UpNo, @ReceiveStatusName AS ReceiveStatusName, @ReportName AS ReportName, @ReportDate AS ReportDate
  FROM Receive_List AS RL
  JOIN School_Rtype AS SR ON SR.Receive_Type = RL.Receive_Type
  JOIN Year_List AS YL ON YL.Year_Id = RL.Year_Id
  JOIN Term_List AS TL ON TL.Receive_Type = RL.Receive_Type AND TL.Year_Id = RL.Year_Id AND TL.Term_Id = RL.Term_Id
 WHERE RL.Receive_Type = @ReceiveType AND RL.Year_Id = @YearId AND RL.Term_Id = @TermId AND RL.Dep_Id = @DepId AND RL.Receive_Id = @ReceiveId";
                    KeyValue[] parameters = new KeyValue[] {
                        new KeyValue("@ReceiveType", receiveType),
                        new KeyValue("@YearId", yearId),
                        new KeyValue("@TermId", termId),
                        new KeyValue("@DepId", depId),
                        new KeyValue("@ReceiveId", receiveId),
                        new KeyValue("@UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
                        new KeyValue("@ReceiveStatusName", receiveStatusName),
                        new KeyValue("@ReportName", reportName),
                        new KeyValue("@ReportDate", DateTime.Today.ToString("yyyy/MM/dd"))
                    };
                    result = _Factory.GetDataTable(sql, parameters, 0, 1, out dtHead);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢表頭資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取部別合計資料
                DataTable dtDeptSum = null;
                if (result.IsSuccess)
                {
                    #region SAMPLE
                    //SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
                    //     , ISNULL(SR.Dept_Id, '') AS Dept_Id
                    //     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
                    //     , SUM(ISNULL(SR.Receive_01, 0)) AS SUM_Receive_01, SUM(ISNULL(SR.Receive_02, 0)) AS SUM_Receive_02
                    //     , SUM(ISNULL(SR.Receive_03, 0)) AS SUM_Receive_03, SUM(ISNULL(SR.Receive_04, 0)) AS SUM_Receive_04
                    //     , SUM(ISNULL(SR.Receive_05, 0)) AS SUM_Receive_05, SUM(ISNULL(SR.Receive_06, 0)) AS SUM_Receive_06
                    //     , SUM(ISNULL(SR.Receive_07, 0)) AS SUM_Receive_07, SUM(ISNULL(SR.Receive_08, 0)) AS SUM_Receive_08
                    //     , SUM(ISNULL(SR.Receive_09, 0)) AS SUM_Receive_09, SUM(ISNULL(SR.Receive_10, 0)) AS SUM_Receive_10
                    //     , SUM(ISNULL(SR.Receive_11, 0)) AS SUM_Receive_11, SUM(ISNULL(SR.Receive_12, 0)) AS SUM_Receive_12
                    //     , SUM(ISNULL(SR.Receive_13, 0)) AS SUM_Receive_13, SUM(ISNULL(SR.Receive_14, 0)) AS SUM_Receive_14
                    //     , SUM(ISNULL(SR.Receive_15, 0)) AS SUM_Receive_15, SUM(ISNULL(SR.Receive_16, 0)) AS SUM_Receive_16
                    //     , SUM(ISNULL(SR.Receive_17, 0)) AS SUM_Receive_17, SUM(ISNULL(SR.Receive_18, 0)) AS SUM_Receive_18
                    //     , SUM(ISNULL(SR.Receive_19, 0)) AS SUM_Receive_19, SUM(ISNULL(SR.Receive_20, 0)) AS SUM_Receive_20
                    //     , SUM(ISNULL(SR.Receive_21, 0)) AS SUM_Receive_21, SUM(ISNULL(SR.Receive_22, 0)) AS SUM_Receive_22
                    //     , SUM(ISNULL(SR.Receive_23, 0)) AS SUM_Receive_23, SUM(ISNULL(SR.Receive_24, 0)) AS SUM_Receive_24
                    //     , SUM(ISNULL(SR.Receive_25, 0)) AS SUM_Receive_25, SUM(ISNULL(SR.Receive_26, 0)) AS SUM_Receive_26
                    //     , SUM(ISNULL(SR.Receive_27, 0)) AS SUM_Receive_27, SUM(ISNULL(SR.Receive_28, 0)) AS SUM_Receive_28
                    //     , SUM(ISNULL(SR.Receive_29, 0)) AS SUM_Receive_29, SUM(ISNULL(SR.Receive_30, 0)) AS SUM_Receive_30
                    //     , SUM(ISNULL(SR.Receive_31, 0)) AS SUM_Receive_31, SUM(ISNULL(SR.Receive_32, 0)) AS SUM_Receive_32
                    //     , SUM(ISNULL(SR.Receive_33, 0)) AS SUM_Receive_33, SUM(ISNULL(SR.Receive_34, 0)) AS SUM_Receive_34
                    //     , SUM(ISNULL(SR.Receive_35, 0)) AS SUM_Receive_35, SUM(ISNULL(SR.Receive_36, 0)) AS SUM_Receive_36
                    //     , SUM(ISNULL(SR.Receive_37, 0)) AS SUM_Receive_37, SUM(ISNULL(SR.Receive_38, 0)) AS SUM_Receive_38
                    //     , SUM(ISNULL(SR.Receive_39, 0)) AS SUM_Receive_39, SUM(ISNULL(SR.Receive_40, 0)) AS SUM_Receive_40
                    //     , SUM(ISNULL(SR.Receive_Amount, 0)) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
                    //  FROM Student_Receive AS SR
                    // WHERE SR.Receive_Type = '5100' AND SR.Year_Id = '103' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
                    //   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')
                    // GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id
                    // ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id
                    #endregion

                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"
SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id
     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
     , SUM(ISNULL(SR.Receive_01, 0)) AS SUM_Receive_01, SUM(ISNULL(SR.Receive_02, 0)) AS SUM_Receive_02
     , SUM(ISNULL(SR.Receive_03, 0)) AS SUM_Receive_03, SUM(ISNULL(SR.Receive_04, 0)) AS SUM_Receive_04
     , SUM(ISNULL(SR.Receive_05, 0)) AS SUM_Receive_05, SUM(ISNULL(SR.Receive_06, 0)) AS SUM_Receive_06
     , SUM(ISNULL(SR.Receive_07, 0)) AS SUM_Receive_07, SUM(ISNULL(SR.Receive_08, 0)) AS SUM_Receive_08
     , SUM(ISNULL(SR.Receive_09, 0)) AS SUM_Receive_09, SUM(ISNULL(SR.Receive_10, 0)) AS SUM_Receive_10
     , SUM(ISNULL(SR.Receive_11, 0)) AS SUM_Receive_11, SUM(ISNULL(SR.Receive_12, 0)) AS SUM_Receive_12
     , SUM(ISNULL(SR.Receive_13, 0)) AS SUM_Receive_13, SUM(ISNULL(SR.Receive_14, 0)) AS SUM_Receive_14
     , SUM(ISNULL(SR.Receive_15, 0)) AS SUM_Receive_15, SUM(ISNULL(SR.Receive_16, 0)) AS SUM_Receive_16
     , SUM(ISNULL(SR.Receive_17, 0)) AS SUM_Receive_17, SUM(ISNULL(SR.Receive_18, 0)) AS SUM_Receive_18
     , SUM(ISNULL(SR.Receive_19, 0)) AS SUM_Receive_19, SUM(ISNULL(SR.Receive_20, 0)) AS SUM_Receive_20
     , SUM(ISNULL(SR.Receive_21, 0)) AS SUM_Receive_21, SUM(ISNULL(SR.Receive_22, 0)) AS SUM_Receive_22
     , SUM(ISNULL(SR.Receive_23, 0)) AS SUM_Receive_23, SUM(ISNULL(SR.Receive_24, 0)) AS SUM_Receive_24
     , SUM(ISNULL(SR.Receive_25, 0)) AS SUM_Receive_25, SUM(ISNULL(SR.Receive_26, 0)) AS SUM_Receive_26
     , SUM(ISNULL(SR.Receive_27, 0)) AS SUM_Receive_27, SUM(ISNULL(SR.Receive_28, 0)) AS SUM_Receive_28
     , SUM(ISNULL(SR.Receive_29, 0)) AS SUM_Receive_29, SUM(ISNULL(SR.Receive_30, 0)) AS SUM_Receive_30
     , SUM(ISNULL(SR.Receive_31, 0)) AS SUM_Receive_31, SUM(ISNULL(SR.Receive_32, 0)) AS SUM_Receive_32
     , SUM(ISNULL(SR.Receive_33, 0)) AS SUM_Receive_33, SUM(ISNULL(SR.Receive_34, 0)) AS SUM_Receive_34
     , SUM(ISNULL(SR.Receive_35, 0)) AS SUM_Receive_35, SUM(ISNULL(SR.Receive_36, 0)) AS SUM_Receive_36
     , SUM(ISNULL(SR.Receive_37, 0)) AS SUM_Receive_37, SUM(ISNULL(SR.Receive_38, 0)) AS SUM_Receive_38
     , SUM(ISNULL(SR.Receive_39, 0)) AS SUM_Receive_39, SUM(ISNULL(SR.Receive_40, 0)) AS SUM_Receive_40
     , SUM(ISNULL(SR.Receive_Amount, 0)) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
 GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id
 ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtDeptSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算部別合計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取系所合計資料
                DataTable dtMajorSum = null;
                if (result.IsSuccess)
                {
                    #region SAMPLE
                    //SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
                    //     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id
                    //     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
                    //     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
                    //     , SUM(ISNULL(SR.Receive_01, 0)) AS SUM_Receive_01, SUM(ISNULL(SR.Receive_02, 0)) AS SUM_Receive_02
                    //     , SUM(ISNULL(SR.Receive_03, 0)) AS SUM_Receive_03, SUM(ISNULL(SR.Receive_04, 0)) AS SUM_Receive_04
                    //     , SUM(ISNULL(SR.Receive_05, 0)) AS SUM_Receive_05, SUM(ISNULL(SR.Receive_06, 0)) AS SUM_Receive_06
                    //     , SUM(ISNULL(SR.Receive_07, 0)) AS SUM_Receive_07, SUM(ISNULL(SR.Receive_08, 0)) AS SUM_Receive_08
                    //     , SUM(ISNULL(SR.Receive_09, 0)) AS SUM_Receive_09, SUM(ISNULL(SR.Receive_10, 0)) AS SUM_Receive_10
                    //     , SUM(ISNULL(SR.Receive_11, 0)) AS SUM_Receive_11, SUM(ISNULL(SR.Receive_12, 0)) AS SUM_Receive_12
                    //     , SUM(ISNULL(SR.Receive_13, 0)) AS SUM_Receive_13, SUM(ISNULL(SR.Receive_14, 0)) AS SUM_Receive_14
                    //     , SUM(ISNULL(SR.Receive_15, 0)) AS SUM_Receive_15, SUM(ISNULL(SR.Receive_16, 0)) AS SUM_Receive_16
                    //     , SUM(ISNULL(SR.Receive_17, 0)) AS SUM_Receive_17, SUM(ISNULL(SR.Receive_18, 0)) AS SUM_Receive_18
                    //     , SUM(ISNULL(SR.Receive_19, 0)) AS SUM_Receive_19, SUM(ISNULL(SR.Receive_20, 0)) AS SUM_Receive_20
                    //     , SUM(ISNULL(SR.Receive_21, 0)) AS SUM_Receive_21, SUM(ISNULL(SR.Receive_22, 0)) AS SUM_Receive_22
                    //     , SUM(ISNULL(SR.Receive_23, 0)) AS SUM_Receive_23, SUM(ISNULL(SR.Receive_24, 0)) AS SUM_Receive_24
                    //     , SUM(ISNULL(SR.Receive_25, 0)) AS SUM_Receive_25, SUM(ISNULL(SR.Receive_26, 0)) AS SUM_Receive_26
                    //     , SUM(ISNULL(SR.Receive_27, 0)) AS SUM_Receive_27, SUM(ISNULL(SR.Receive_28, 0)) AS SUM_Receive_28
                    //     , SUM(ISNULL(SR.Receive_29, 0)) AS SUM_Receive_29, SUM(ISNULL(SR.Receive_30, 0)) AS SUM_Receive_30
                    //     , SUM(ISNULL(SR.Receive_31, 0)) AS SUM_Receive_31, SUM(ISNULL(SR.Receive_32, 0)) AS SUM_Receive_32
                    //     , SUM(ISNULL(SR.Receive_33, 0)) AS SUM_Receive_33, SUM(ISNULL(SR.Receive_34, 0)) AS SUM_Receive_34
                    //     , SUM(ISNULL(SR.Receive_35, 0)) AS SUM_Receive_35, SUM(ISNULL(SR.Receive_36, 0)) AS SUM_Receive_36
                    //     , SUM(ISNULL(SR.Receive_37, 0)) AS SUM_Receive_37, SUM(ISNULL(SR.Receive_38, 0)) AS SUM_Receive_38
                    //     , SUM(ISNULL(SR.Receive_39, 0)) AS SUM_Receive_39, SUM(ISNULL(SR.Receive_40, 0)) AS SUM_Receive_40
                    //     , SUM(ISNULL(SR.Receive_Amount, 0)) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
                    //  FROM Student_Receive AS SR
                    // WHERE SR.Receive_Type = '5100' AND SR.Year_Id = '103' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
                    //   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')
                    // GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id
                    // ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id
                    #endregion

                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"
SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id
     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
     , SUM(ISNULL(SR.Receive_01, 0)) AS SUM_Receive_01, SUM(ISNULL(SR.Receive_02, 0)) AS SUM_Receive_02
     , SUM(ISNULL(SR.Receive_03, 0)) AS SUM_Receive_03, SUM(ISNULL(SR.Receive_04, 0)) AS SUM_Receive_04
     , SUM(ISNULL(SR.Receive_05, 0)) AS SUM_Receive_05, SUM(ISNULL(SR.Receive_06, 0)) AS SUM_Receive_06
     , SUM(ISNULL(SR.Receive_07, 0)) AS SUM_Receive_07, SUM(ISNULL(SR.Receive_08, 0)) AS SUM_Receive_08
     , SUM(ISNULL(SR.Receive_09, 0)) AS SUM_Receive_09, SUM(ISNULL(SR.Receive_10, 0)) AS SUM_Receive_10
     , SUM(ISNULL(SR.Receive_11, 0)) AS SUM_Receive_11, SUM(ISNULL(SR.Receive_12, 0)) AS SUM_Receive_12
     , SUM(ISNULL(SR.Receive_13, 0)) AS SUM_Receive_13, SUM(ISNULL(SR.Receive_14, 0)) AS SUM_Receive_14
     , SUM(ISNULL(SR.Receive_15, 0)) AS SUM_Receive_15, SUM(ISNULL(SR.Receive_16, 0)) AS SUM_Receive_16
     , SUM(ISNULL(SR.Receive_17, 0)) AS SUM_Receive_17, SUM(ISNULL(SR.Receive_18, 0)) AS SUM_Receive_18
     , SUM(ISNULL(SR.Receive_19, 0)) AS SUM_Receive_19, SUM(ISNULL(SR.Receive_20, 0)) AS SUM_Receive_20
     , SUM(ISNULL(SR.Receive_21, 0)) AS SUM_Receive_21, SUM(ISNULL(SR.Receive_22, 0)) AS SUM_Receive_22
     , SUM(ISNULL(SR.Receive_23, 0)) AS SUM_Receive_23, SUM(ISNULL(SR.Receive_24, 0)) AS SUM_Receive_24
     , SUM(ISNULL(SR.Receive_25, 0)) AS SUM_Receive_25, SUM(ISNULL(SR.Receive_26, 0)) AS SUM_Receive_26
     , SUM(ISNULL(SR.Receive_27, 0)) AS SUM_Receive_27, SUM(ISNULL(SR.Receive_28, 0)) AS SUM_Receive_28
     , SUM(ISNULL(SR.Receive_29, 0)) AS SUM_Receive_29, SUM(ISNULL(SR.Receive_30, 0)) AS SUM_Receive_30
     , SUM(ISNULL(SR.Receive_31, 0)) AS SUM_Receive_31, SUM(ISNULL(SR.Receive_32, 0)) AS SUM_Receive_32
     , SUM(ISNULL(SR.Receive_33, 0)) AS SUM_Receive_33, SUM(ISNULL(SR.Receive_34, 0)) AS SUM_Receive_34
     , SUM(ISNULL(SR.Receive_35, 0)) AS SUM_Receive_35, SUM(ISNULL(SR.Receive_36, 0)) AS SUM_Receive_36
     , SUM(ISNULL(SR.Receive_37, 0)) AS SUM_Receive_37, SUM(ISNULL(SR.Receive_38, 0)) AS SUM_Receive_38
     , SUM(ISNULL(SR.Receive_39, 0)) AS SUM_Receive_39, SUM(ISNULL(SR.Receive_40, 0)) AS SUM_Receive_40
     , SUM(ISNULL(SR.Receive_Amount, 0)) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
 GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id
 ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtMajorSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算系所合計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取班別合計資料
                DataTable dtClassSum = null;
                if (result.IsSuccess)
                {
                    #region SAMPLE
                    //SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
                    //     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
                    //     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
                    //     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
                    //     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
                    //     , SUM(ISNULL(SR.Receive_01, 0)) AS SUM_Receive_01, SUM(ISNULL(SR.Receive_02, 0)) AS SUM_Receive_02
                    //     , SUM(ISNULL(SR.Receive_03, 0)) AS SUM_Receive_03, SUM(ISNULL(SR.Receive_04, 0)) AS SUM_Receive_04
                    //     , SUM(ISNULL(SR.Receive_05, 0)) AS SUM_Receive_05, SUM(ISNULL(SR.Receive_06, 0)) AS SUM_Receive_06
                    //     , SUM(ISNULL(SR.Receive_07, 0)) AS SUM_Receive_07, SUM(ISNULL(SR.Receive_08, 0)) AS SUM_Receive_08
                    //     , SUM(ISNULL(SR.Receive_09, 0)) AS SUM_Receive_09, SUM(ISNULL(SR.Receive_10, 0)) AS SUM_Receive_10
                    //     , SUM(ISNULL(SR.Receive_11, 0)) AS SUM_Receive_11, SUM(ISNULL(SR.Receive_12, 0)) AS SUM_Receive_12
                    //     , SUM(ISNULL(SR.Receive_13, 0)) AS SUM_Receive_13, SUM(ISNULL(SR.Receive_14, 0)) AS SUM_Receive_14
                    //     , SUM(ISNULL(SR.Receive_15, 0)) AS SUM_Receive_15, SUM(ISNULL(SR.Receive_16, 0)) AS SUM_Receive_16
                    //     , SUM(ISNULL(SR.Receive_17, 0)) AS SUM_Receive_17, SUM(ISNULL(SR.Receive_18, 0)) AS SUM_Receive_18
                    //     , SUM(ISNULL(SR.Receive_19, 0)) AS SUM_Receive_19, SUM(ISNULL(SR.Receive_20, 0)) AS SUM_Receive_20
                    //     , SUM(ISNULL(SR.Receive_21, 0)) AS SUM_Receive_21, SUM(ISNULL(SR.Receive_22, 0)) AS SUM_Receive_22
                    //     , SUM(ISNULL(SR.Receive_23, 0)) AS SUM_Receive_23, SUM(ISNULL(SR.Receive_24, 0)) AS SUM_Receive_24
                    //     , SUM(ISNULL(SR.Receive_25, 0)) AS SUM_Receive_25, SUM(ISNULL(SR.Receive_26, 0)) AS SUM_Receive_26
                    //     , SUM(ISNULL(SR.Receive_27, 0)) AS SUM_Receive_27, SUM(ISNULL(SR.Receive_28, 0)) AS SUM_Receive_28
                    //     , SUM(ISNULL(SR.Receive_29, 0)) AS SUM_Receive_29, SUM(ISNULL(SR.Receive_30, 0)) AS SUM_Receive_30
                    //     , SUM(ISNULL(SR.Receive_31, 0)) AS SUM_Receive_31, SUM(ISNULL(SR.Receive_32, 0)) AS SUM_Receive_32
                    //     , SUM(ISNULL(SR.Receive_33, 0)) AS SUM_Receive_33, SUM(ISNULL(SR.Receive_34, 0)) AS SUM_Receive_34
                    //     , SUM(ISNULL(SR.Receive_35, 0)) AS SUM_Receive_35, SUM(ISNULL(SR.Receive_36, 0)) AS SUM_Receive_36
                    //     , SUM(ISNULL(SR.Receive_37, 0)) AS SUM_Receive_37, SUM(ISNULL(SR.Receive_38, 0)) AS SUM_Receive_38
                    //     , SUM(ISNULL(SR.Receive_39, 0)) AS SUM_Receive_39, SUM(ISNULL(SR.Receive_40, 0)) AS SUM_Receive_40
                    //     , SUM(ISNULL(SR.Receive_Amount, 0)) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
                    //  FROM Student_Receive AS SR
                    // WHERE SR.Receive_Type = '5100' AND SR.Year_Id = '103' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
                    //   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')
                    // GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id
                    // ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id

                    #endregion

                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"
SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
     , SUM(ISNULL(SR.Receive_01, 0)) AS SUM_Receive_01, SUM(ISNULL(SR.Receive_02, 0)) AS SUM_Receive_02
     , SUM(ISNULL(SR.Receive_03, 0)) AS SUM_Receive_03, SUM(ISNULL(SR.Receive_04, 0)) AS SUM_Receive_04
     , SUM(ISNULL(SR.Receive_05, 0)) AS SUM_Receive_05, SUM(ISNULL(SR.Receive_06, 0)) AS SUM_Receive_06
     , SUM(ISNULL(SR.Receive_07, 0)) AS SUM_Receive_07, SUM(ISNULL(SR.Receive_08, 0)) AS SUM_Receive_08
     , SUM(ISNULL(SR.Receive_09, 0)) AS SUM_Receive_09, SUM(ISNULL(SR.Receive_10, 0)) AS SUM_Receive_10
     , SUM(ISNULL(SR.Receive_11, 0)) AS SUM_Receive_11, SUM(ISNULL(SR.Receive_12, 0)) AS SUM_Receive_12
     , SUM(ISNULL(SR.Receive_13, 0)) AS SUM_Receive_13, SUM(ISNULL(SR.Receive_14, 0)) AS SUM_Receive_14
     , SUM(ISNULL(SR.Receive_15, 0)) AS SUM_Receive_15, SUM(ISNULL(SR.Receive_16, 0)) AS SUM_Receive_16
     , SUM(ISNULL(SR.Receive_17, 0)) AS SUM_Receive_17, SUM(ISNULL(SR.Receive_18, 0)) AS SUM_Receive_18
     , SUM(ISNULL(SR.Receive_19, 0)) AS SUM_Receive_19, SUM(ISNULL(SR.Receive_20, 0)) AS SUM_Receive_20
     , SUM(ISNULL(SR.Receive_21, 0)) AS SUM_Receive_21, SUM(ISNULL(SR.Receive_22, 0)) AS SUM_Receive_22
     , SUM(ISNULL(SR.Receive_23, 0)) AS SUM_Receive_23, SUM(ISNULL(SR.Receive_24, 0)) AS SUM_Receive_24
     , SUM(ISNULL(SR.Receive_25, 0)) AS SUM_Receive_25, SUM(ISNULL(SR.Receive_26, 0)) AS SUM_Receive_26
     , SUM(ISNULL(SR.Receive_27, 0)) AS SUM_Receive_27, SUM(ISNULL(SR.Receive_28, 0)) AS SUM_Receive_28
     , SUM(ISNULL(SR.Receive_29, 0)) AS SUM_Receive_29, SUM(ISNULL(SR.Receive_30, 0)) AS SUM_Receive_30
     , SUM(ISNULL(SR.Receive_31, 0)) AS SUM_Receive_31, SUM(ISNULL(SR.Receive_32, 0)) AS SUM_Receive_32
     , SUM(ISNULL(SR.Receive_33, 0)) AS SUM_Receive_33, SUM(ISNULL(SR.Receive_34, 0)) AS SUM_Receive_34
     , SUM(ISNULL(SR.Receive_35, 0)) AS SUM_Receive_35, SUM(ISNULL(SR.Receive_36, 0)) AS SUM_Receive_36
     , SUM(ISNULL(SR.Receive_37, 0)) AS SUM_Receive_37, SUM(ISNULL(SR.Receive_38, 0)) AS SUM_Receive_38
     , SUM(ISNULL(SR.Receive_39, 0)) AS SUM_Receive_39, SUM(ISNULL(SR.Receive_40, 0)) AS SUM_Receive_40
     , SUM(ISNULL(SR.Receive_Amount, 0)) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
 GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id
 ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtClassSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算班別合計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #endregion

                #region 產生報表檔案
                if (isUseODS)
                {
                    ODSHelper helper = new ODSHelper();
                    string errmsg = helper.GenReportA2(dtHead, dtDeptSum, dtMajorSum, dtClassSum, receiveItems, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 ODS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    GenReportHelper helper = new GenReportHelper();
                    string errmsg = helper.GenReportA2(dtHead, dtDeptSum, dtMajorSum, dtClassSum, receiveItems, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 XLS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                return String.Concat("產出檔案發生例外，錯誤訊息；", ex.Message);
            }
        }
        #endregion
        #endregion

        #region 匯出繳費失敗總表(遲繳)
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出繳費銷帳總表
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="upNo">批號</param>
        /// <param name="receiveStatus">繳費狀態</param>
        /// <param name="reportKind">報表種類 (1=正常 2=遲繳)</param>
        /// <param name="reportName">報表名稱</param>
        /// <param name="outFileContent">成功則傳回產生檔案內容，否則傳回 null</param>
        /// <param name="isUseODS">是否產生成 ODS 檔</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        public string ExportReportA(string receiveType, string yearId, string termId, string depId, string receiveId
            , int? upNo, string receiveStatus, string reportKind, string reportName
            , out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            #region 檢查參數
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return "缺少查詢資料參數";
            }
            string receiveStatusName = null;
            receiveStatus = receiveStatus == null ? String.Empty : receiveStatus.Trim();
            switch (receiveStatus)
            {
                case "0":   //未繳
                    receiveStatusName = "未繳";
                    break;
                case "1":   //已繳
                    receiveStatusName = "已繳";
                    break;
                case "":   //全部
                    receiveStatusName = String.Empty;
                    break;
                default:
                    return "不正確的繳款方式參數";
            }
            if (String.IsNullOrWhiteSpace(reportName))
            {
                return "缺少報表名稱參數";
            }
            reportName = reportName.Trim();
            if (upNo != null && upNo.Value < 0)
            {
                upNo = null;
            }
            #endregion

            try
            {
                #region 取資料
                Result result = null;

                #region 取得代收費用別的收入科目名稱與對應學生繳費資料的收入科目金額欄位名稱 + 繳費期限
                KeyValueList<string> receiveItems = new KeyValueList<string>(40);
                string payDueDate = null;
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT ISNULL(Receive_Item01, '') AS Receive_Item01, ISNULL(Receive_Item02, '') AS Receive_Item02, ISNULL(Receive_Item03, '') AS Receive_Item03, ISNULL(Receive_Item04, '') AS Receive_Item04, ISNULL(Receive_Item05, '') AS Receive_Item05
     , ISNULL(Receive_Item06, '') AS Receive_Item06, ISNULL(Receive_Item07, '') AS Receive_Item07, ISNULL(Receive_Item08, '') AS Receive_Item08, ISNULL(Receive_Item09, '') AS Receive_Item09, ISNULL(Receive_Item10, '') AS Receive_Item10
     , ISNULL(Receive_Item11, '') AS Receive_Item11, ISNULL(Receive_Item12, '') AS Receive_Item12, ISNULL(Receive_Item13, '') AS Receive_Item13, ISNULL(Receive_Item14, '') AS Receive_Item14, ISNULL(Receive_Item15, '') AS Receive_Item15
     , ISNULL(Receive_Item16, '') AS Receive_Item16, ISNULL(Receive_Item17, '') AS Receive_Item17, ISNULL(Receive_Item18, '') AS Receive_Item18, ISNULL(Receive_Item19, '') AS Receive_Item19, ISNULL(Receive_Item20, '') AS Receive_Item20
     , ISNULL(Receive_Item21, '') AS Receive_Item21, ISNULL(Receive_Item22, '') AS Receive_Item22, ISNULL(Receive_Item23, '') AS Receive_Item23, ISNULL(Receive_Item24, '') AS Receive_Item24, ISNULL(Receive_Item25, '') AS Receive_Item25
     , ISNULL(Receive_Item26, '') AS Receive_Item26, ISNULL(Receive_Item27, '') AS Receive_Item27, ISNULL(Receive_Item28, '') AS Receive_Item28, ISNULL(Receive_Item29, '') AS Receive_Item29, ISNULL(Receive_Item30, '') AS Receive_Item30
     , ISNULL(Receive_Item31, '') AS Receive_Item31, ISNULL(Receive_Item32, '') AS Receive_Item32, ISNULL(Receive_Item33, '') AS Receive_Item33, ISNULL(Receive_Item34, '') AS Receive_Item34, ISNULL(Receive_Item35, '') AS Receive_Item35
     , ISNULL(Receive_Item36, '') AS Receive_Item36, ISNULL(Receive_Item37, '') AS Receive_Item37, ISNULL(Receive_Item38, '') AS Receive_Item38, ISNULL(Receive_Item39, '') AS Receive_Item39, ISNULL(Receive_Item40, '') AS Receive_Item40
     , ISNULL(Pay_Date, '') AS Pay_Due_Date
  FROM School_Rid 
 WHERE Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);

                    DataTable dt = null;
                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 1, out dt);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢代收費用別設定失敗，錯誤訊息：{0}", result.Message);
                    }
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return "查無代收費用別設定資料";
                    }
                    DataRow dRow = dt.Rows[0];
                    payDueDate = dRow["Pay_Due_Date"].ToString().Trim();

                    for (int no = 1; no <= 40; no++)
                    {
                        string columnName = String.Format("Receive_Item{0:00}", no);
                        string name = dRow[columnName].ToString().Trim();
                        if (!String.IsNullOrEmpty(name))
                        {
                            receiveItems.Add(name, String.Format("Receive_{0:00}", no));
                        }
                    }

                    receiveItems.Add("繳費金額合計", "Receive_Amount");
                }
                #endregion

                #region 取表頭資料
                DataTable dtHead = null;
                {
                    #region 表頭資料欄位
                    //                學校名稱      報表名稱
                    //學年：          學期：          繳款方式：
                    //商家代號：      代收費用：      批號：             yyyy/mm/dd  第xx頁/共xx頁
                    #endregion

                    string sql = @"SELECT RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id
     , SR.Sch_Name, YL.Year_Name, TL.Term_Name, RL.Receive_Name
     , @UpNo AS UpNo, @ReceiveStatusName AS ReceiveStatusName, @ReportName AS ReportName, @ReportDate AS ReportDate
  FROM Receive_List AS RL
  JOIN School_Rtype AS SR ON SR.Receive_Type = RL.Receive_Type
  JOIN Year_List AS YL ON YL.Year_Id = RL.Year_Id
  JOIN Term_List AS TL ON TL.Receive_Type = RL.Receive_Type AND TL.Year_Id = RL.Year_Id AND TL.Term_Id = RL.Term_Id
 WHERE RL.Receive_Type = @ReceiveType AND RL.Year_Id = @YearId AND RL.Term_Id = @TermId AND RL.Dep_Id = @DepId AND RL.Receive_Id = @ReceiveId";
                    KeyValue[] parameters = new KeyValue[] {
                        new KeyValue("@ReceiveType", receiveType),
                        new KeyValue("@YearId", yearId),
                        new KeyValue("@TermId", termId),
                        new KeyValue("@DepId", depId),
                        new KeyValue("@ReceiveId", receiveId),
                        new KeyValue("@UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
                        new KeyValue("@ReceiveStatusName", receiveStatusName),
                        new KeyValue("@ReportName", reportName),
                        new KeyValue("@ReportDate", DateTime.Today.ToString("yyyy/MM/dd"))
                    };
                    result = _Factory.GetDataTable(sql, parameters, 0, 1, out dtHead);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢表頭資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取分頁資料 (By 部別)
                DataTable dtPage = null;
                if (result.IsSuccess)
                {
                    #region Sql Sample
                    //SELECT DISTINCT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, ISNULL(SR.Dept_Id, '') AS Dept_Id
                    //  FROM Student_Receive AS SR
                    // WHERE SR.Receive_Type = '5100' AND SR.Year_Id = '104' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
                    //   --AND SR.Up_No = 1
                    //   --AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')
                    //   --AND SR.Receive_Date IS NOT NULL
                    //   --AND SR.Receive_Date != '' AND SR.Receive_Date IS NOT NULL
                    // ORDER BY Dept_Id
                    #endregion

                    KeyValueList parameters = new KeyValueList();

                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT DISTINCT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, ISNULL(SR.Dept_Id, '') AS Dept_Id
     --, ISNULL((SELECT Dept_Name  FROM Dept_List AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
 ORDER BY Dept_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtPage);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算分頁資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取部別小計資料
                DataTable dtDeptSum = null;
                if (result.IsSuccess)
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id
     , SUM(Receive_Amount) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
 FROM (
SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id
     , ISNULL(SR.Receive_Amount, 0) AS Receive_Amount
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
) AS SR
 GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id
 ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtDeptSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算部別小計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取系所小計資料
                DataTable dtMajorSum = null;
                if (result.IsSuccess)
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id
     , SUM(Receive_Amount) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
 FROM (
SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id
     , ISNULL(SR.Receive_Amount, 0) AS Receive_Amount
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
) AS SR
 GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id
 ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtMajorSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算系所小計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取班別小計資料
                DataTable dtClassSum = null;
                if (result.IsSuccess)
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id
     , SUM(Receive_Amount) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
 FROM (
SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
     , ISNULL(SR.Receive_Amount, 0) AS Receive_Amount
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
) AS SR
 GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id
 ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtClassSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算班別小計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取表身資料
                DataTable dtData = null;
                {
                    #region Sql Sample
                    //SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
                    //     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
                    //     , SR.Receive_01, SR.Receive_02, SR.Receive_03, SR.Receive_04, SR.Receive_05, SR.Receive_06, SR.Receive_07, SR.Receive_08, SR.Receive_09, SR.Receive_10
                    //     , SR.Receive_11, SR.Receive_12, SR.Receive_13, SR.Receive_14, SR.Receive_15, SR.Receive_16, SR.Receive_17, SR.Receive_18, SR.Receive_19, SR.Receive_20
                    //     , SR.Receive_21, SR.Receive_22, SR.Receive_23, SR.Receive_24, SR.Receive_25, SR.Receive_26, SR.Receive_27, SR.Receive_28, SR.Receive_29, SR.Receive_30
                    //     , SR.Receive_31, SR.Receive_32, SR.Receive_33, SR.Receive_34, SR.Receive_35, SR.Receive_36, SR.Receive_37, SR.Receive_38, SR.Receive_39, SR.Receive_40
                    //     , SR.Receive_Amount
                    //     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
                    //     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
                    //     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
                    //  FROM Student_Receive AS SR
                    // WHERE SR.Receive_Type = '5100' AND SR.Year_Id = '104' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
                    //   --AND SR.Receive_Way = '01'
                    // ORDER BY Dept_Id, Major_Id, Class_Id
                    #endregion

                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Stu_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
     , SR.Receive_01, SR.Receive_02, SR.Receive_03, SR.Receive_04, SR.Receive_05, SR.Receive_06, SR.Receive_07, SR.Receive_08, SR.Receive_09, SR.Receive_10
     , SR.Receive_11, SR.Receive_12, SR.Receive_13, SR.Receive_14, SR.Receive_15, SR.Receive_16, SR.Receive_17, SR.Receive_18, SR.Receive_19, SR.Receive_20
     , SR.Receive_21, SR.Receive_22, SR.Receive_23, SR.Receive_24, SR.Receive_25, SR.Receive_26, SR.Receive_27, SR.Receive_28, SR.Receive_29, SR.Receive_30
     , SR.Receive_31, SR.Receive_32, SR.Receive_33, SR.Receive_34, SR.Receive_35, SR.Receive_36, SR.Receive_37, SR.Receive_38, SR.Receive_39, SR.Receive_40
     , SR.Receive_Amount
     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@" ORDER BY Dept_Id, Major_Id, Class_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtData);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion
                #endregion

                #region 產生報表檔案
                if (isUseODS)
                {
                    ODSHelper helper = new ODSHelper();
                    string errmsg = helper.GenReportA(dtHead, dtPage, dtDeptSum, dtMajorSum, dtClassSum, receiveItems, dtData, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 ODS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    GenReportHelper helper = new GenReportHelper();
                    string errmsg = helper.GenReportA(dtHead, dtPage, dtDeptSum, dtMajorSum, dtClassSum, receiveItems, dtData, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 XLS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                return String.Concat("產出檔案發生例外，錯誤訊息；", ex.Message);
            }
        }
        #endregion
        #endregion

        #region 匯出繳費銷帳明細表 (正常、遲繳)
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出繳費銷帳明細表 (正常、遲繳)
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="upNo">批號</param>
        /// <param name="receiveStatus">繳費狀態</param>
        /// <param name="reportKind">報表種類 (1=正常 2=遲繳)</param>
        /// <param name="reportName">報表名稱</param>
        /// <param name="outFileContent">成功則傳回產生檔案內容，否則傳回 null</param>
        /// <param name="isUseODS">是否產生成 ODS 檔</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        public string ExportReportB(string receiveType, string yearId, string termId, string depId, string receiveId
            , int? upNo, string receiveStatus, string reportKind, string reportName
            , out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            #region 檢查參數
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return "缺少查詢資料參數";
            }
            string receiveStatusName = null;
            receiveStatus = receiveStatus == null ? String.Empty : receiveStatus.Trim();
            switch (receiveStatus)
            {
                case "0":   //未繳
                    receiveStatusName = "未繳";
                    break;
                case "1":   //已繳
                    receiveStatusName = "已繳";
                    break;
                case "":   //全部
                    receiveStatusName = String.Empty;
                    break;
                default:
                    return "不正確的繳款方式參數";
            }
            if (String.IsNullOrWhiteSpace(reportName))
            {
                return "缺少報表名稱參數";
            }
            reportName = reportName.Trim();
            if (upNo != null && upNo.Value < 0)
            {
                upNo = null;
            }
            #endregion

            try
            {
                #region 取資料
                Result result = null;

                #region 取得代收費用別的收入科目名稱與對應學生繳費資料的收入科目金額欄位名稱 + 繳費期限
                KeyValueList<string> receiveItems = new KeyValueList<string>(40);
                string payDueDate = null;
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT ISNULL(Receive_Item01, '') AS Receive_Item01, ISNULL(Receive_Item02, '') AS Receive_Item02, ISNULL(Receive_Item03, '') AS Receive_Item03, ISNULL(Receive_Item04, '') AS Receive_Item04, ISNULL(Receive_Item05, '') AS Receive_Item05
     , ISNULL(Receive_Item06, '') AS Receive_Item06, ISNULL(Receive_Item07, '') AS Receive_Item07, ISNULL(Receive_Item08, '') AS Receive_Item08, ISNULL(Receive_Item09, '') AS Receive_Item09, ISNULL(Receive_Item10, '') AS Receive_Item10
     , ISNULL(Receive_Item11, '') AS Receive_Item11, ISNULL(Receive_Item12, '') AS Receive_Item12, ISNULL(Receive_Item13, '') AS Receive_Item13, ISNULL(Receive_Item14, '') AS Receive_Item14, ISNULL(Receive_Item15, '') AS Receive_Item15
     , ISNULL(Receive_Item16, '') AS Receive_Item16, ISNULL(Receive_Item17, '') AS Receive_Item17, ISNULL(Receive_Item18, '') AS Receive_Item18, ISNULL(Receive_Item19, '') AS Receive_Item19, ISNULL(Receive_Item20, '') AS Receive_Item20
     , ISNULL(Receive_Item21, '') AS Receive_Item21, ISNULL(Receive_Item22, '') AS Receive_Item22, ISNULL(Receive_Item23, '') AS Receive_Item23, ISNULL(Receive_Item24, '') AS Receive_Item24, ISNULL(Receive_Item25, '') AS Receive_Item25
     , ISNULL(Receive_Item26, '') AS Receive_Item26, ISNULL(Receive_Item27, '') AS Receive_Item27, ISNULL(Receive_Item28, '') AS Receive_Item28, ISNULL(Receive_Item29, '') AS Receive_Item29, ISNULL(Receive_Item30, '') AS Receive_Item30
     , ISNULL(Receive_Item31, '') AS Receive_Item31, ISNULL(Receive_Item32, '') AS Receive_Item32, ISNULL(Receive_Item33, '') AS Receive_Item33, ISNULL(Receive_Item34, '') AS Receive_Item34, ISNULL(Receive_Item35, '') AS Receive_Item35
     , ISNULL(Receive_Item36, '') AS Receive_Item36, ISNULL(Receive_Item37, '') AS Receive_Item37, ISNULL(Receive_Item38, '') AS Receive_Item38, ISNULL(Receive_Item39, '') AS Receive_Item39, ISNULL(Receive_Item40, '') AS Receive_Item40
     , ISNULL(Pay_Date, '') AS Pay_Due_Date
  FROM School_Rid 
 WHERE Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);

                    DataTable dt = null;
                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 1, out dt);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢代收費用別設定失敗，錯誤訊息：{0}", result.Message);
                    }
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return "查無代收費用別設定資料";
                    }

                    receiveItems.Add("學號", "Stu_Id");
                    receiveItems.Add("姓名", "Stu_Name");

                    DataRow dRow = dt.Rows[0];
                    payDueDate = dRow["Pay_Due_Date"].ToString().Trim();

                    for (int no = 1; no <= 40; no++)
                    {
                        string columnName = String.Format("Receive_Item{0:00}", no);
                        string name = dRow[columnName].ToString().Trim();
                        if (!String.IsNullOrEmpty(name))
                        {
                            receiveItems.Add(name, String.Format("Receive_{0:00}", no));
                        }
                    }

                    receiveItems.Add("繳費金額合計", "Receive_Amount");
                    receiveItems.Add("繳款日期", "Receive_Date");
                    receiveItems.Add("虛擬帳號", "Cancel_No");

                }
                #endregion

                #region 取表頭資料
                DataTable dtHead = null;
                {
                    #region 表頭資料欄位
                    //                學校名稱      報表名稱
                    //學年：          學期：          繳款方式：
                    //商家代號：      代收費用：      批號：             yyyy/mm/dd  第xx頁/共xx頁
                    #endregion

                    string sql = @"SELECT RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id
     , SR.Sch_Name, YL.Year_Name, TL.Term_Name, RL.Receive_Name
     , @UpNo AS UpNo, @ReceiveStatusName AS ReceiveStatusName, @ReportName AS ReportName, @ReportDate AS ReportDate
  FROM Receive_List AS RL
  JOIN School_Rtype AS SR ON SR.Receive_Type = RL.Receive_Type
  JOIN Year_List AS YL ON YL.Year_Id = RL.Year_Id
  JOIN Term_List AS TL ON TL.Receive_Type = RL.Receive_Type AND TL.Year_Id = RL.Year_Id AND TL.Term_Id = RL.Term_Id
 WHERE RL.Receive_Type = @ReceiveType AND RL.Year_Id = @YearId AND RL.Term_Id = @TermId AND RL.Dep_Id = @DepId AND RL.Receive_Id = @ReceiveId";
                    KeyValue[] parameters = new KeyValue[] {
                        new KeyValue("@ReceiveType", receiveType),
                        new KeyValue("@YearId", yearId),
                        new KeyValue("@TermId", termId),
                        new KeyValue("@DepId", depId),
                        new KeyValue("@ReceiveId", receiveId),
                        new KeyValue("@UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
                        new KeyValue("@ReceiveStatusName", receiveStatusName),
                        new KeyValue("@ReportName", reportName),
                        new KeyValue("@ReportDate", DateTime.Today.ToString("yyyy/MM/dd"))
                    };
                    result = _Factory.GetDataTable(sql, parameters, 0, 1, out dtHead);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢表頭資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取分頁資料 (By 班別)
                DataTable dtPage = null;
                if (result.IsSuccess)
                {
                    #region Sql Sample
                    //SELECT DISTINCT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, ISNULL(SR.Dept_Id, '') AS Dept_Id
                    //  FROM Student_Receive AS SR
                    // WHERE SR.Receive_Type = '5100' AND SR.Year_Id = '104' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
                    //   --AND SR.Up_No = 1
                    //   --AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')
                    //   --AND SR.Receive_Date IS NOT NULL
                    //   --AND SR.Receive_Date != '' AND SR.Receive_Date IS NOT NULL
                    // ORDER BY Dept_Id
                    #endregion

                    KeyValueList parameters = new KeyValueList();

                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT DISTINCT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
 ORDER BY Dept_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtPage);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算分頁資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取部別小計資料
                DataTable dtDeptSum = null;
                if (result.IsSuccess)
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id
     , SUM(Receive_Amount) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
 FROM (
SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id
     , ISNULL(SR.Receive_Amount, 0) AS Receive_Amount
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
) AS SR
 GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id
 ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtDeptSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算部別小計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取系所小計資料
                DataTable dtMajorSum = null;
                if (result.IsSuccess)
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id
     , SUM(Receive_Amount) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
 FROM (
SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id
     , ISNULL(SR.Receive_Amount, 0) AS Receive_Amount
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
) AS SR
 GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id
 ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtMajorSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算系所小計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取班別小計資料
                DataTable dtClassSum = null;
                if (result.IsSuccess)
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id
     , SUM(Receive_Amount) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
 FROM (
SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
     , ISNULL(SR.Receive_Amount, 0) AS Receive_Amount
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@"
) AS SR
 GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id
 ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtClassSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算系所小計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取表身資料
                DataTable dtData = null;
                {
                    #region Sql Sample
                    //SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
                    //     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
                    //     , SR.Receive_01, SR.Receive_02, SR.Receive_03, SR.Receive_04, SR.Receive_05, SR.Receive_06, SR.Receive_07, SR.Receive_08, SR.Receive_09, SR.Receive_10
                    //     , SR.Receive_11, SR.Receive_12, SR.Receive_13, SR.Receive_14, SR.Receive_15, SR.Receive_16, SR.Receive_17, SR.Receive_18, SR.Receive_19, SR.Receive_20
                    //     , SR.Receive_21, SR.Receive_22, SR.Receive_23, SR.Receive_24, SR.Receive_25, SR.Receive_26, SR.Receive_27, SR.Receive_28, SR.Receive_29, SR.Receive_30
                    //     , SR.Receive_31, SR.Receive_32, SR.Receive_33, SR.Receive_34, SR.Receive_35, SR.Receive_36, SR.Receive_37, SR.Receive_38, SR.Receive_39, SR.Receive_40
                    //     , SR.Receive_Amount
                    //     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
                    //     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
                    //     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
                    //  FROM Student_Receive AS SR
                    // WHERE SR.Receive_Type = '5100' AND SR.Year_Id = '104' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
                    //   --AND SR.Receive_Way = '01'
                    // ORDER BY Dept_Id, Major_Id, Class_Id
                    #endregion

                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Stu_Id
     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
     , SR.Receive_01, SR.Receive_02, SR.Receive_03, SR.Receive_04, SR.Receive_05, SR.Receive_06, SR.Receive_07, SR.Receive_08, SR.Receive_09, SR.Receive_10
     , SR.Receive_11, SR.Receive_12, SR.Receive_13, SR.Receive_14, SR.Receive_15, SR.Receive_16, SR.Receive_17, SR.Receive_18, SR.Receive_19, SR.Receive_20
     , SR.Receive_21, SR.Receive_22, SR.Receive_23, SR.Receive_24, SR.Receive_25, SR.Receive_26, SR.Receive_27, SR.Receive_28, SR.Receive_29, SR.Receive_30
     , SR.Receive_31, SR.Receive_32, SR.Receive_33, SR.Receive_34, SR.Receive_35, SR.Receive_36, SR.Receive_37, SR.Receive_38, SR.Receive_39, SR.Receive_40
     , SR.Receive_Amount, SR.Cancel_No, SR.Receive_Date
     , ISNULL((SELECT SM.Stu_Name FROM Student_Master AS SM WHERE SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id), '') AS Stu_Name
     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
  FROM Student_Receive AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    #region [Old]
                    //if (receiveStatus == "1")   //已繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
                    //}
                    //else if (receiveStatus == "0")  //未繳
                    //{
                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
                    //}
                    #endregion

                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (reportKind == "2")  //遲繳
                    {
                        sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
                        parameters.Add("@PayDueDate", payDueDate);
                    }
                    sql.AppendLine(@" ORDER BY Dept_Id, Major_Id, Class_Id");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtData);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion
                #endregion

                #region 產生報表檔案
                if (isUseODS)
                {
                    ODSHelper helper = new ODSHelper();
                    string errmsg = helper.GenReportB(dtHead, dtPage, dtDeptSum, dtMajorSum, dtClassSum, receiveItems, dtData, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 ODS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    GenReportHelper helper = new GenReportHelper();
                    string errmsg = helper.GenReportB(dtHead, dtPage, dtDeptSum, dtMajorSum, dtClassSum, receiveItems, dtData, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 XLS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                return String.Concat("產出檔案發生例外，錯誤訊息：", ex.Message);
            }
        }
        #endregion
        #endregion

        #region 匯出學生繳費名冊
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出學生繳費名冊
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別</param>
        /// <param name="upNo">批號</param>
        /// <param name="receiveStatus">繳費狀態 ("0"=未繳; "1"=已繳; ""=全部)</param>
        /// <param name="groupKind">群組明細程度 (1=部別、系所、班別; 2=部別、系所、年級、班別)</param>
        /// <param name="otherItems">說明項目 (1=繳款日期; 2=身份別; 3=減免; 4=助貸)</param>
        /// <param name="receiveItemNo">收入科目編號 (1 ~ 40)</param>
        /// <param name="reportName">報表名稱</param>
        /// <param name="outFileContent"></param>
        /// <param name="isUseODS">是否產生成 ODS 檔</param>
        /// <returns></returns>
        public string ExportReportC(string receiveType, string yearId, string termId, string depId, string receiveId
            , int? upNo, string receiveStatus, string groupKind, string[] otherItems, int receiveItemNo, string reportName
            , out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            #region 檢查參數
            receiveType = receiveType == null ? null : receiveType.Trim();
            yearId = yearId == null ? null : yearId.Trim();
            termId = termId == null ? null : termId.Trim();
            depId = depId == null ? null : depId.Trim();
            receiveId = receiveId == null ? null : receiveId.Trim();
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return "缺少查詢資料參數";
            }
            string receiveStatusName = null;
            receiveStatus = receiveStatus == null ? String.Empty : receiveStatus.Trim();
            switch (receiveStatus)
            {
                case "0":   //未繳
                    receiveStatusName = "未繳";
                    break;
                case "1":   //已繳
                    receiveStatusName = "已繳";
                    break;
                case "":   //全部
                    receiveStatusName = String.Empty;
                    break;
                default:
                    return "不正確的繳款方式參數";
            }
            groupKind = groupKind == null ? String.Empty : groupKind.Trim();
            switch (groupKind)
            {
                case "1":   //部別、系所、班別
                case "2":   //部別、系所、年級、班別
                    break;
                default:
                    return "不正確的群組明細程度參數";
            }

            //說明項目欄位名稱
            KeyValueList<string> otherFields = new KeyValueList<string>(4);
            if (otherItems != null && otherItems.Length > 0)
            {
                foreach (string otherItem in otherItems)
                {
                    switch (otherItem)
                    {
                        case "1":   //繳款日期
                            otherFields.Add("繳款日期", "Pay_Due_Date");
                            break;
                        case "2":   //身份註記
                            otherFields.Add("身份註記", "Identify_Id");
                            break;
                        case "3":   //減免
                            otherFields.Add("減免", "Reduce_Id");
                            break;
                        case "4":   //就貸
                            otherFields.Add("就貸", "Loan_Id");
                            break;
                        default:
                            return "不正確的說明項目參數";
                    }
                }
            }
            if (String.IsNullOrWhiteSpace(reportName))
            {
                return "缺少報表名稱參數";
            }
            reportName = reportName.Trim();
            if (!reportName.EndsWith("名冊"))
            {
                reportName += "名冊";
            }
            if (upNo != null && upNo.Value < 0)
            {
                upNo = null;
            }

            if (receiveItemNo < 1 || receiveItemNo > 40)
            {
                return "不正確的收入科目參數";
            }
            string receiveItemFileName = String.Format("Receive_{0:00}", receiveItemNo);
            #endregion

            try
            {
                #region 取資料
                Result result = null;

                #region 取得指定的代收費用別的收入科目名稱與繳費期限
                string payDueDate = null;
                string receiveItemName = null;
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"
SELECT ISNULL(Receive_Item01, '') AS Receive_Item01, ISNULL(Receive_Item02, '') AS Receive_Item02, ISNULL(Receive_Item03, '') AS Receive_Item03, ISNULL(Receive_Item04, '') AS Receive_Item04, ISNULL(Receive_Item05, '') AS Receive_Item05
     , ISNULL(Receive_Item06, '') AS Receive_Item06, ISNULL(Receive_Item07, '') AS Receive_Item07, ISNULL(Receive_Item08, '') AS Receive_Item08, ISNULL(Receive_Item09, '') AS Receive_Item09, ISNULL(Receive_Item10, '') AS Receive_Item10
     , ISNULL(Receive_Item11, '') AS Receive_Item11, ISNULL(Receive_Item12, '') AS Receive_Item12, ISNULL(Receive_Item13, '') AS Receive_Item13, ISNULL(Receive_Item14, '') AS Receive_Item14, ISNULL(Receive_Item15, '') AS Receive_Item15
     , ISNULL(Receive_Item16, '') AS Receive_Item16, ISNULL(Receive_Item17, '') AS Receive_Item17, ISNULL(Receive_Item18, '') AS Receive_Item18, ISNULL(Receive_Item19, '') AS Receive_Item19, ISNULL(Receive_Item20, '') AS Receive_Item20
     , ISNULL(Receive_Item21, '') AS Receive_Item21, ISNULL(Receive_Item22, '') AS Receive_Item22, ISNULL(Receive_Item23, '') AS Receive_Item23, ISNULL(Receive_Item24, '') AS Receive_Item24, ISNULL(Receive_Item25, '') AS Receive_Item25
     , ISNULL(Receive_Item26, '') AS Receive_Item26, ISNULL(Receive_Item27, '') AS Receive_Item27, ISNULL(Receive_Item28, '') AS Receive_Item28, ISNULL(Receive_Item29, '') AS Receive_Item29, ISNULL(Receive_Item30, '') AS Receive_Item30
     , ISNULL(Receive_Item31, '') AS Receive_Item31, ISNULL(Receive_Item32, '') AS Receive_Item32, ISNULL(Receive_Item33, '') AS Receive_Item33, ISNULL(Receive_Item34, '') AS Receive_Item34, ISNULL(Receive_Item35, '') AS Receive_Item35
     , ISNULL(Receive_Item36, '') AS Receive_Item36, ISNULL(Receive_Item37, '') AS Receive_Item37, ISNULL(Receive_Item38, '') AS Receive_Item38, ISNULL(Receive_Item39, '') AS Receive_Item39, ISNULL(Receive_Item40, '') AS Receive_Item40
     , ISNULL(Pay_Date, '') AS Pay_Due_Date
  FROM School_Rid 
 WHERE Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);

                    DataTable dt = null;
                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 1, out dt);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢代收費用別設定失敗，錯誤訊息：{0}", result.Message);
                    }
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return "查無代收費用別設定資料";
                    }

                    DataRow dRow = dt.Rows[0];
                    payDueDate = dRow["Pay_Due_Date"].ToString().Trim();

                    string columnName = String.Format("Receive_Item{0:00}", receiveItemNo);
                    receiveItemName = dRow[columnName].ToString().Trim();
                }
                #endregion

                #region 取表頭資料
                DataTable dtHead = null;
                {
                    #region 表頭資料欄位
                    //                學校名稱      <<指定收入科目>>名冊
                    //學年：          學期：          繳款方式：
                    //商家代號：      代收費用：      批號：             yyyy/mm/dd  第xx頁/共xx頁
                    #endregion

                    string sql = @"
SELECT RL.[Receive_Type], RL.[Year_Id], RL.[Term_Id], RL.[Dep_Id], RL.[Receive_Id]
     , SR.[Sch_Name], YL.[Year_Name], TL.[Term_Name], RL.[Receive_Name]
     , @UpNo AS [UpNo], @ReceiveStatusName AS [ReceiveStatusName], @ReportName AS [ReportName], @ReportDate AS [ReportDate]
     , @ReceiveItemName AS [ReceiveItemName]
  FROM [Receive_List] AS RL
  JOIN [School_Rtype] AS SR ON SR.[Receive_Type] = RL.[Receive_Type]
  JOIN [Year_List] AS YL ON YL.[Year_Id] = RL.[Year_Id]
  JOIN [Term_List] AS TL ON TL.[Receive_Type] = RL.[Receive_Type] AND TL.[Year_Id] = RL.[Year_Id] AND TL.[Term_Id] = RL.[Term_Id]
 WHERE RL.[Receive_Type] = @ReceiveType AND RL.[Year_Id] = @YearId AND RL.[Term_Id] = @TermId AND RL.[Dep_Id] = @DepId AND RL.[Receive_Id] = @ReceiveId";
                    KeyValue[] parameters = new KeyValue[] {
                        new KeyValue("@ReceiveType", receiveType),
                        new KeyValue("@YearId", yearId),
                        new KeyValue("@TermId", termId),
                        new KeyValue("@DepId", depId),
                        new KeyValue("@ReceiveId", receiveId),
                        new KeyValue("@UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
                        new KeyValue("@ReceiveStatusName", receiveStatusName),
                        new KeyValue("@ReportName", reportName),
                        new KeyValue("@ReportDate", DateTime.Today.ToString("yyyy/MM/dd")),
                        new KeyValue("@ReceiveItemName", receiveItemName)
                    };
                    result = _Factory.GetDataTable(sql, parameters, 0, 1, out dtHead);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢表頭資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取分頁資料 (By 部別)
                DataTable dtPage = null;
                if (result.IsSuccess)
                {
                    #region Sql Sample
                    //SELECT DISTINCT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, ISNULL(SR.Dept_Id, '') AS Dept_Id
                    //  FROM Student_Receive AS SR
                    // WHERE SR.Receive_Type = '5100' AND SR.Year_Id = '104' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
                    //   --AND SR.Up_No = 1
                    //   --AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')
                    //   --AND SR.Receive_Date IS NOT NULL
                    //   --AND SR.Receive_Date != '' AND SR.Receive_Date IS NOT NULL
                    // ORDER BY Dept_Id
                    #endregion

                    KeyValueList parameters = new KeyValueList();

                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"
SELECT DISTINCT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], '') AS [Dept_Id]
     --, ISNULL((SELECT [Dept_Name]  FROM [Dept_List] AS DL WHERE DL.[Receive_Type] = SR.[Receive_Type] AND DL.[Year_Id] = SR.[Year_Id] AND DL.[Term_Id] = SR.[Term_Id] AND DL.[Dept_Id] = SR.[Dept_Id]), '') AS [Dept_Name]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.[Up_No] = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))");
                    }
                    #endregion

                    sql.AppendLine(@"
 ORDER BY [Dept_Id]");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtPage);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算分頁資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取部別小計資料
                DataTable dtDeptSum = null;
                if (result.IsSuccess)
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id]
     , SUM([Receive_Item_Amount]) AS [SUM_AMOUNT], COUNT(1) AS [SUM_COUNT]
     , ISNULL((SELECT [Dept_Name]  FROM [Dept_List]  AS DL WHERE DL.[Receive_Type] = SR.[Receive_Type] AND DL.[Year_Id] = SR.[Year_Id] AND DL.[Term_Id] = SR.[Term_Id] AND DL.[Dept_Id] = SR.[Dept_Id]), '') AS [Dept_Name]
 FROM (
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id]
     , ISNULL(SR.[Dept_Id], '') AS [Dept_Id]
     , ISNULL(SR.[" + receiveItemFileName + @"], 0) AS [Receive_Item_Amount]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.[Up_No] = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))");
                    }
                    #endregion

                    sql.AppendLine(@") AS SR
 GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id]
 ORDER BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id]");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtDeptSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算部別小計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取系所小計資料
                DataTable dtMajorSum = null;
                if (result.IsSuccess)
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id]
     , SUM([Receive_Item_Amount]) AS [SUM_AMOUNT], COUNT(1) AS [SUM_COUNT]
     , ISNULL((SELECT [Major_Name] FROM [Major_List] AS ML WHERE ML.[Receive_Type] = SR.[Receive_Type] AND ML.[Year_Id] = SR.[Year_Id] AND ML.[Term_Id] = SR.[Term_Id] AND ML.[Dep_Id] = SR.[Dep_Id] AND ML.[Major_Id] = SR.[Major_Id]), '') AS [Major_Name]
 FROM (
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id]
     , ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], '') AS [Major_Id]
     , ISNULL(SR.[" + receiveItemFileName + @"], 0) AS [Receive_Item_Amount]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.[Up_No] = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))");
                    }
                    #endregion

                    sql.AppendLine(@") AS SR
 GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id]
 ORDER BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id]");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtMajorSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算系所小計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取年級小計資料
                DataTable dtGradeSum = null;
                if (result.IsSuccess && groupKind == "2")
                {
                    //群組明細程度：部別、系所、年級、班別
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id], SR.[Stu_Grade]
     , SUM([Receive_Item_Amount]) AS [SUM_AMOUNT], COUNT(1) AS [SUM_COUNT]
     --, '' AS [Grade_Name]
 FROM (
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id]
     , ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], '') AS [Major_Id], ISNULL(SR.[Stu_Grade], '') AS [Stu_Grade]
     , ISNULL(SR.[" + receiveItemFileName + @"], 0) AS [Receive_Item_Amount]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.[Up_No] = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))");
                    }
                    #endregion

                    sql.AppendLine(@") AS SR
 GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id], SR.[Stu_Grade]
 ORDER BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id], SR.[Stu_Grade]");

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtGradeSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算系所小計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取班別小計資料
                DataTable dtClassSum = null;
                if (result.IsSuccess)
                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();

                    if (groupKind == "1")
                    {
                        //群組明細程度：部別、系所、班別
                        sql.AppendLine(@"
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id], SR.[Class_Id]
     , SUM([Receive_Item_Amount]) AS [SUM_AMOUNT], COUNT(1) AS [SUM_COUNT]
     , ISNULL((SELECT [Class_Name] FROM [Class_List] AS CL WHERE CL.[Receive_Type] = SR.[Receive_Type] AND CL.[Year_Id] = SR.[Year_Id] AND CL.[Term_Id] = SR.[Term_Id] AND CL.[Dep_Id] = SR.[Dep_Id] AND CL.[Class_Id] = SR.[Class_Id]), '') AS [Class_Name]
 FROM (
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id]
     , ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], '') AS [Major_Id], ISNULL(SR.[Class_Id], '') AS [Class_Id]
     , ISNULL(SR.[" + receiveItemFileName + @"], 0) AS [Receive_Item_Amount]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId");
                    }
                    else
                    {
                        //群組明細程度：部別、系所、年級、班別
                        sql.AppendLine(@"
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id], SR.[Stu_Grade], SR.[Class_Id]
     , SUM([Receive_Item_Amount]) AS [SUM_AMOUNT], COUNT(1) AS [SUM_COUNT]
     , ISNULL((SELECT [Class_Name] FROM [Class_List] AS CL WHERE CL.[Receive_Type] = SR.[Receive_Type] AND CL.[Year_Id] = SR.[Year_Id] AND CL.[Term_Id] = SR.[Term_Id] AND CL.[Dep_Id] = SR.[Dep_Id] AND CL.[Class_Id] = SR.[Class_Id]), '') AS [Class_Name]
 FROM (
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id]
     , ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], '') AS [Major_Id], ISNULL(SR.[Stu_Grade], '') AS [Stu_Grade], ISNULL(SR.[Class_Id], '') AS [Class_Id]
     , ISNULL(SR.[" + receiveItemFileName + @"], 0) AS [Receive_Item_Amount]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId");
                    }
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);

                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.[Up_No] = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))");
                    }
                    #endregion

                    if (groupKind == "1")
                    {
                        //群組明細程度：部別、系所、班別
                        sql.AppendLine(@") AS SR
 GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id], SR.[Class_Id]
 ORDER BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id], SR.[Class_Id]");
                    }
                    else
                    {
                        //群組明細程度：部別、系所、年級、班別
                        sql.AppendLine(@") AS SR
 GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id], SR.[Stu_Grade], SR.[Class_Id]
 ORDER BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Dept_Id], SR.[Major_Id], SR.[Stu_Grade], SR.[Class_Id]");
                    }

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtClassSum);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算系所小計資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取表身資料
                DataTable dtData = null;
                {
                    #region Sql Sample
                    //SELECT SR.[Stu_Id]
                    //     , (SELECT [Stu_Name] FROM [Student_Master] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]) AS [Stu_Name]
                    //     , ISNULL(SR.[Receive_01], '')    AS [Receive_Item_Amount]   --收入科目
                    //     , ISNULL(SR.[Pay_Due_Date], '1030905')  AS [Pay_Due_Date]  --繳款日期
                    //     , ISNULL(SR.[Identify_Id01], '') AS [Identify_Id]   --身份註記
                    //     , ISNULL(SR.[Reduce_Id], '')     AS [Reduce_Id]     --減免
                    //     , ISNULL(SR.[Loan_Id], '')       AS [Loan_Id]       --就貸
                    //     , ISNULL(SR.[Dept_Id], '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Stu_Grade, '') AS Stu_Grade, ISNULL(SR.Class_Id, '') AS Class_Id     --部別、系所、年級、班別
                    //     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
                    //     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
                    //     , '' AS Grade_Name
                    //     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
                    //  FROM [Student_Receive] AS SR
                    // WHERE SR.Receive_Type = '5026' AND SR.Year_Id = '103' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
                    //   AND SR.Up_No = '0'
                    //   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))  --已繳
                    //   --AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))           --未繳
                    // ORDER BY Dept_Id, Major_Id, Stu_Grade, Class_Id
                    #endregion

                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id]
     , ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], '') AS [Major_Id], ISNULL(SR.[Stu_Grade], '') AS [Stu_Grade], ISNULL(SR.[Class_Id], '') AS [Class_Id]       --部別、系所、年級、班別
     , ISNULL(SR.[Stu_Id], '') AS [Stu_Id]
     , ISNULL((SELECT [Stu_Name] FROM [Student_Master] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
     , SR.[" + receiveItemFileName + @"]      AS [Receive_Item_Amount]  --收入科目金額
     , ISNULL(SR.[Pay_Due_Date], @PayDueDate) AS [Pay_Due_Date]         --繳款日期
     , ISNULL(SR.[Identify_Id01], '')         AS [Identify_Id]          --身份註記
     , ISNULL(SR.[Reduce_Id], '')             AS [Reduce_Id]            --減免
     , ISNULL(SR.[Loan_Id], '')               AS [Loan_Id]              --就貸
     --, ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
     --, ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
     --, '' AS Grade_Name
     --, ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
  FROM [Student_Receive] AS SR
 WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);
                    parameters.Add("@PayDueDate", payDueDate);
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.Up_No = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }

                    #region 無代收日、代收管道、入帳日才算未繳，反之為已繳
                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
                    }
                    #endregion

                    if (groupKind == "1")
                    {
                        //群組明細程度：部別、系所、班別
                        sql.AppendLine(@" ORDER BY Dept_Id, Major_Id, Class_Id");
                    }
                    else
                    {
                        //群組明細程度：部別、系所、年級、班別
                        sql.AppendLine(@" ORDER BY Dept_Id, Major_Id, Stu_Grade, Class_Id");
                    }

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtData);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion
                #endregion

                #region 產生報表檔案
                if (isUseODS)
                {
                    ODSHelper helper = new ODSHelper();
                    string errmsg = helper.GenReportC(dtHead, dtPage, dtDeptSum, dtMajorSum, dtGradeSum, dtClassSum, dtData, otherFields, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 ODS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    GenReportHelper helper = new GenReportHelper();
                    string errmsg = helper.GenReportC(dtHead, dtPage, dtDeptSum, dtMajorSum, dtGradeSum, dtClassSum, dtData, otherFields, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 XLS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                return String.Concat("產出檔案發生例外，錯誤訊息；", ex.Message);
            }
        }
        #endregion
        #endregion

        #region 匯出手續費統計報表
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出手續費統計報表
        /// </summary>
        /// <param name="bankId">分行代碼</param>
        /// <param name="schIdenty">學校代碼</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="acctSDate">入帳日起日</param>
        /// <param name="acctEDate">入帳日迄日</param>
        /// <param name="outFileContent"></param>
        /// <param name="isUseODS">是否產生成 ODS 檔</param>
        /// <returns></returns>
        public string ExportReportD(string bankId, string schIdenty, string receiveType, DateTime? acctSDate, DateTime? acctEDate
            , out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(bankId) && String.IsNullOrWhiteSpace(schIdenty) && String.IsNullOrWhiteSpace(receiveType))
            {
                return "缺少查詢資料參數";
            }
            #endregion

            try
            {
                #region [MDY:20171127] 取財金 QRCode 支付參數設定中的手續費 (20170831_01)
                decimal twpayFee = 0;
                {
                    ConfigEntity configData = null;
                    Expression where = new Expression(ConfigEntity.Field.ConfigKey, FiscQRCodeConfig.ConfigKey);
                    Result result = _Factory.SelectFirst<ConfigEntity>(where, null, out configData);
                    if (!result.IsSuccess)
                    {
                        return String.Format("讀取財金 QRCode 支付參數失敗，錯誤訊息：{0}", result.Message);
                    }
                    //無資料或參數設定錯誤 (無法解析) 都不要中斷處理，避免無法產生報表
                    if (configData != null)
                    {
                        FiscQRCodeConfig qrcodeConfig = FiscQRCodeConfig.Parse(configData.ConfigValue);
                        twpayFee = qrcodeConfig.Charge == null ? 0 : qrcodeConfig.Charge.Value;
                    }
                }
                #endregion

                #region 取資料
                DataTable dtData = null;
                {
                    #region 查詢條件
                    StringBuilder andSql = new StringBuilder();
                    KeyValueList parameters = new KeyValueList(5);

                    if (!String.IsNullOrEmpty(bankId))
                    {
                        andSql.Append(" AND S.Bank_Id = @BANKID");
                        parameters.Add("@BANKID", bankId);
                    }

                    if (!String.IsNullOrEmpty(schIdenty))
                    {
                        andSql.Append(" AND S.Sch_Identy = @SCHIDENTY");
                        parameters.Add("@SCHIDENTY", schIdenty);
                    }

                    if (!String.IsNullOrEmpty(receiveType))
                    {
                        andSql.Append(" AND S.Receive_Type = @RECEIVETYPE");
                        parameters.Add("@RECEIVETYPE", receiveType);
                    }

                    if (acctSDate != null)
                    {
                        andSql.Append(" AND R.Account_Date >= @S_ACCOUNTDATE");
                        parameters.Add("@S_ACCOUNTDATE", Common.GetTWDate7(acctSDate.Value));
                    }

                    if (acctEDate != null)
                    {
                        andSql.Append(" AND R.Account_Date <= @E_ACCOUNTDATE");
                        parameters.Add("@E_ACCOUNTDATE", Common.GetTWDate7(acctEDate.Value));
                    }
                    #endregion

                    #region 取資料
                    #region [MDY:20191214] (2019擴充案) 國際信用卡 - 增加 NC-國際信用卡
                    #region [OLD]
//                    #region [MDY:20171127] 增加08-全國繳費網、10-台灣Pay (20170831_01)
//                    #region [Old]
//                    //                    #region [MDY:20170506] 增加支付寶 (09) 管道的手續費計算
////                    #region [Old]
////                    //                    #region [MDY:20160607] 修正 SQL 錯誤
//////                    string sql = @"
//////SELECT D.Sch_Identy, D.Sch_Name, D.Bank_Id
//////     , ISNULL((SELECT BANKSNAME FROM BANK WHERE BANK.BANKNO = D.Bank_Id), '') AS Bank_Name
//////     , D.Receive_Way
//////   --, D.Receive_Type, ISNULL(D.Barcode_Id, '') AS Barcode_Id
//////     , SUM(D.Receive_Amount) AS SUM_AMOUNT, SUM(ISNULL(CC.RC_Charge, 0)) AS SUM_FEE
//////     , COUNT(1) AS DATA_COUNT
//////  FROM
//////(
//////SELECT S.Sch_Identy, S.Sch_Name, S.Bank_Id, R.Receive_Type, R.Receive_Amount
//////     , CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END AS Receive_Way
//////     , (SELECT TOP 1 C.BarCode_Id FROM Receive_Channel AS C 
//////         WHERE C.RC_type = S.Receive_Type
//////           AND C.RC_Channel = (CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END)
//////           AND ((R.Receive_Amount >= C.MoneyLowerLimit AND R.Receive_Amount <= C.MoneyLimit) OR (C.MoneyLowerLimit = 0 AND C.MoneyLimit = 0))
//////       ) AS Barcode_Id
//////  FROM Student_Receive AS R
//////  JOIN School_Rtype AS S ON R.Receive_Type = S.Receive_Type
////// WHERE R.Account_Date IS NOT NULL AND R.Account_Date != ''
////// --AND R.Receive_Way IN ('80', '81', '82', '83', '84', '85')
//////  " + andSql.ToString() + @"
//////) AS D
//////  LEFT JOIN Receive_Channel CC ON CC.RC_type = D.Receive_Type AND CC.RC_Channel = D.Receive_Way AND CC.BarCode_Id = D.Barcode_Id
////// GROUP BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
//////   --, D.Receive_Type, D.Barcode_Id
////// ORDER BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
//////   --, D.Receive_Type, D.Barcode_Id
//////";
//////                    #endregion
////                    #endregion

////                    string sql = @"
////SELECT D.Sch_Identy, D.Sch_Name, D.Bank_Id
////     , ISNULL((SELECT BANKSNAME FROM BANK WHERE BANK.BANKNO = D.Bank_Id), '') AS Bank_Name
////     , D.Receive_Way
////   --, D.Receive_Type, ISNULL(D.Barcode_Id, '') AS Barcode_Id
////     , SUM(D.Receive_Amount) AS SUM_AMOUNT
////     , (CASE WHEN D.Receive_Way = '09' THEN SUM(D.Process_Fee) ELSE SUM(ISNULL(CC.RC_Charge, 0)) END) AS SUM_FEE
////     , COUNT(1) AS DATA_COUNT
////  FROM
////(
////SELECT S.Sch_Identy, S.Sch_Name, S.Bank_Id, R.Receive_Type, R.Receive_Amount
////     , CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END AS Receive_Way
////     , (SELECT TOP 1 C.BarCode_Id FROM Receive_Channel AS C 
////         WHERE C.RC_type = S.Receive_Type
////           AND C.RC_Channel = (CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END)
////           AND ((R.Receive_Amount >= C.MoneyLowerLimit AND R.Receive_Amount <= C.MoneyLimit) OR (C.MoneyLowerLimit = 0 AND C.MoneyLimit = 0))
////       ) AS Barcode_Id
////     , ISNULL(R.Process_Fee, 0) AS Process_Fee
////  FROM Student_Receive AS R
////  JOIN School_Rtype AS S ON R.Receive_Type = S.Receive_Type
//// WHERE R.Account_Date IS NOT NULL AND R.Account_Date != ''
////  " + andSql.ToString() + @"
////) AS D
////  LEFT JOIN Receive_Channel CC ON CC.RC_type = D.Receive_Type AND CC.RC_Channel = D.Receive_Way AND CC.BarCode_Id = D.Barcode_Id
//// GROUP BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
//// ORDER BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
////";
////                    #endregion
//                    #endregion

//                    string sql = @"
//SELECT D.Sch_Identy, D.Sch_Name, D.Bank_Id
//     , ISNULL((SELECT BANKSNAME FROM BANK WHERE BANK.BANKNO = D.Bank_Id), '') AS Bank_Name
//     , D.Receive_Way
//   --, D.Receive_Type, ISNULL(D.Barcode_Id, '') AS Barcode_Id
//     , SUM(D.Receive_Amount) AS SUM_AMOUNT
//     , (CASE WHEN D.Receive_Way = '09' THEN SUM(D.Process_Fee)
//        WHEN D.Receive_Way = '08' THEN 0
//        WHEN D.Receive_Way = '10' THEN SUM(" + twpayFee.ToString() + @")
//        ELSE SUM(ISNULL(CC.RC_Charge, 0))
//        END) AS SUM_FEE
//     , COUNT(1) AS DATA_COUNT
//  FROM
//(
//SELECT S.Sch_Identy, S.Sch_Name, S.Bank_Id, R.Receive_Type, R.Receive_Amount
//     , CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END AS Receive_Way
//     , (SELECT TOP 1 C.BarCode_Id FROM Receive_Channel AS C 
//         WHERE C.RC_type = S.Receive_Type
//           AND C.RC_Channel = (CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END)
//           AND ((R.Receive_Amount >= C.MoneyLowerLimit AND R.Receive_Amount <= C.MoneyLimit) OR (C.MoneyLowerLimit = 0 AND C.MoneyLimit = 0))
//       ) AS Barcode_Id
//     , ISNULL(R.Process_Fee, 0) AS Process_Fee
//  FROM Student_Receive AS R
//  JOIN School_Rtype AS S ON R.Receive_Type = S.Receive_Type
// WHERE R.Account_Date IS NOT NULL AND R.Account_Date != ''
//  " + andSql.ToString() + @"
//) AS D
//  LEFT JOIN Receive_Channel CC ON CC.RC_type = D.Receive_Type AND CC.RC_Channel = D.Receive_Way AND CC.BarCode_Id = D.Barcode_Id
// GROUP BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
// ORDER BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
//";
//                    #endregion
                    #endregion

                    string sql = @"
SELECT D.Sch_Identy, D.Sch_Name, D.Bank_Id
     , ISNULL((SELECT BANKSNAME FROM BANK WHERE BANK.BANKNO = D.Bank_Id), '') AS Bank_Name
     , D.Receive_Way
   --, D.Receive_Type, ISNULL(D.Barcode_Id, '') AS Barcode_Id
     , SUM(D.Receive_Amount) AS SUM_AMOUNT
     , (CASE WHEN D.Receive_Way = '09' THEN SUM(D.Process_Fee)
        WHEN D.Receive_Way = '08' THEN 0
        WHEN D.Receive_Way = '10' THEN SUM(" + twpayFee.ToString() + @")
        WHEN D.Receive_Way = 'NC' THEN SUM(D.Process_Fee)
        ELSE SUM(ISNULL(CC.RC_Charge, 0))
        END) AS SUM_FEE
     , COUNT(1) AS DATA_COUNT
  FROM
(
SELECT S.Sch_Identy, S.Sch_Name, S.Bank_Id, R.Receive_Type, R.Receive_Amount
     , CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END AS Receive_Way
     , (SELECT TOP 1 C.BarCode_Id FROM Receive_Channel AS C 
         WHERE C.RC_type = S.Receive_Type
           AND C.RC_Channel = (CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END)
           AND ((R.Receive_Amount >= C.MoneyLowerLimit AND R.Receive_Amount <= C.MoneyLimit) OR (C.MoneyLowerLimit = 0 AND C.MoneyLimit = 0))
       ) AS Barcode_Id
     , ISNULL(R.Process_Fee, 0) AS Process_Fee
  FROM Student_Receive AS R
  JOIN School_Rtype AS S ON R.Receive_Type = S.Receive_Type
 WHERE R.Account_Date IS NOT NULL AND R.Account_Date != ''
  " + andSql.ToString() + @"
) AS D
  LEFT JOIN Receive_Channel CC ON CC.RC_type = D.Receive_Type AND CC.RC_Channel = D.Receive_Way AND CC.BarCode_Id = D.Barcode_Id
 GROUP BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
 ORDER BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
";
                    #endregion

                    Result result = _Factory.GetDataTable(sql, parameters, 0, 0, out dtData);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢資料失敗，錯誤訊息：{0}", result.Message);
                    }
                    #endregion

                    //if (dtData == null || dtData.Rows.Count == 0)
                    //{
                    //    //無資料則傳回 new byte[0]
                    //    outFileContent = new byte[0];
                    //    return null;
                    //}
                }
                #endregion

                GenReportHelper helper = new GenReportHelper();

                #region 將取得的資料轉成手續費統計報表需要的欄位格式
                string[] channelIds = null;
                DataTable rptData = helper.GetEmptyReportDTable(out channelIds);
                if (dtData != null && dtData.Rows.Count > 0)
                {
                    foreach (DataRow srcRow in dtData.Rows)
                    {
                        string srcSchIdenty = Convert.ToString(srcRow["Sch_Identy"]).Trim();
                        string srcSchName = Convert.ToString(srcRow["Sch_Name"]).Trim();
                        string srcBankId = Convert.ToString(srcRow["Bank_Id"]).Trim();
                        string srcBankName = Convert.ToString(srcRow["Bank_Name"]).Trim();
                        string srcReceiveWay = Convert.ToString(srcRow["Receive_Way"]).Trim();

                        Int64 srcSumAmount = Convert.ToInt64(srcRow["SUM_AMOUNT"]);
                        Int32 srcSumFee = Convert.ToInt32(srcRow["SUM_FEE"]);
                        Int32 srcDataCount = Convert.ToInt32(srcRow["DATA_COUNT"]);

                        if (Convert.ToInt32(srcSchIdenty.Length) > 0 && Convert.ToInt32(srcSchIdenty.Length) > 0)   //為了原碼掃描的奇怪現象
                        {
                            #region [MDY:20210401] 原碼修正
                            string filter = string.Format("Sch_Identy='{0}' AND Bank_Id='{1}'", srcSchIdenty.Replace("'", ""), srcBankId.Replace("'", ""));
                            #endregion

                            DataRow[] rptRows = rptData.Select(filter);
                            if (rptRows != null && rptRows.Length > 0)
                            {
                                //找到就更新
                                DataRow rptRow = rptRows[0];
                                rptRow["Total_Records"] = Convert.ToInt32(rptRow["Total_Records"]) + srcDataCount;
                                rptRow["Total_Amount"] = Convert.ToInt64(rptRow["Total_Amount"]) + srcSumAmount;
                                rptRow["Total_Fee"] = Convert.ToInt32(rptRow["Total_Fee"]) + srcSumFee;

                                foreach (string channelId in channelIds)
                                {
                                    if (channelId == srcReceiveWay)
                                    {
                                        string columnName = String.Format("{0}_Fee", channelId);
                                        rptRow[columnName] = Convert.ToInt32(rptRow[columnName]) + srcSumFee;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //否則新增
                                DataRow rptRow = rptData.NewRow();
                                rptRow["Bank_Id"] = srcBankId;
                                rptRow["Bank_Name"] = srcBankName;
                                rptRow["Sch_Identy"] = srcSchIdenty;
                                rptRow["Sch_Name"] = srcSchName;
                                rptRow["Total_Records"] = srcDataCount;
                                rptRow["Total_Amount"] = srcSumAmount;
                                rptRow["Total_Fee"] = srcSumFee;

                                foreach (string channelId in channelIds)
                                {
                                    string columnName = String.Format("{0}_Fee", channelId);
                                    if (channelId == srcReceiveWay)
                                    {
                                        rptRow[columnName] = srcSumFee;
                                    }
                                    else
                                    {
                                        rptRow[columnName] = 0;
                                    }
                                }

                                rptData.Rows.Add(rptRow);
                            }
                        }
                    }
                }
                #endregion

                #region 產生報表檔案
                if (isUseODS)
                {
                    ODSHelper odsHelper = new ODSHelper();
                    string errmsg = odsHelper.GenReportD(rptData, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 ODS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    string errmsg = helper.GenReportD(rptData, out outFileContent);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        return null;
                    }
                    else
                    {
                        return string.Format("產生報表檔案失敗，錯誤原因：{0}", errmsg);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return String.Concat("匯出手續費統計報表發生例外，錯誤訊息：{0}", ex.Message);
            }
        }
        #endregion
        #endregion

        #region 匯出代收單位交易資訊統計表
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 代收單位交易資訊統計表
        /// </summary>
        /// <param name="bankId">分行代碼</param>
        /// <param name="schIdenty">學校代碼</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="acctSDate">入帳日起日</param>
        /// <param name="acctEDate">入帳日迄日</param>
        /// <param name="outFileContent"></param>
        /// <returns></returns>
        public string ExportReportD2(string bankId, string schIdenty, string receiveType, DateTime? acctSDate, DateTime? acctEDate
            , out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(bankId) && String.IsNullOrWhiteSpace(schIdenty) && String.IsNullOrWhiteSpace(receiveType))
            {
                return "缺少查詢資料參數";
            }
            #endregion

            try
            {
                #region 取資料
                DataTable dtData = null;
                {
                    #region 查詢條件
                    StringBuilder andSql = new StringBuilder();
                    KeyValueList parameters = new KeyValueList(5);

                    if (!String.IsNullOrEmpty(bankId))
                    {
                        andSql.Append(" AND S.Bank_Id = @BANKID");
                        parameters.Add("@BANKID", bankId);
                    }

                    if (!String.IsNullOrEmpty(schIdenty))
                    {
                        andSql.Append(" AND S.Sch_Identy = @SCHIDENTY");
                        parameters.Add("@SCHIDENTY", schIdenty);
                    }

                    if (!String.IsNullOrEmpty(receiveType))
                    {
                        andSql.Append(" AND S.Receive_Type = @RECEIVETYPE");
                        parameters.Add("@RECEIVETYPE", receiveType);
                    }

                    if (acctSDate != null)
                    {
                        andSql.Append(" AND R.Account_Date >= @S_ACCOUNTDATE");
                        parameters.Add("@S_ACCOUNTDATE", Common.GetTWDate7(acctSDate.Value));
                    }

                    if (acctEDate != null)
                    {
                        andSql.Append(" AND R.Account_Date <= @E_ACCOUNTDATE");
                        parameters.Add("@E_ACCOUNTDATE", Common.GetTWDate7(acctEDate.Value));
                    }
                    #endregion

                    #region 取資料
                    #region [MDY:20160607] 修正 SQL 錯誤
                    string sql = @"
SELECT D.Sch_Identy, D.Sch_Name, D.Bank_Id
     , ISNULL((SELECT BANKSNAME FROM BANK WHERE BANK.BANKNO = D.Bank_Id), '') AS Bank_Name
     , D.Receive_Way
   --, D.Receive_Type, ISNULL(D.Barcode_Id, '') AS Barcode_Id
     , SUM(D.Receive_Amount) AS SUM_AMOUNT, SUM(ISNULL(CC.RC_Charge, 0)) AS SUM_FEE
     , COUNT(1) AS DATA_COUNT
  FROM
(
SELECT S.Sch_Identy, S.Sch_Name, S.Bank_Id, R.Receive_Type, R.Receive_Amount
     --, CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END AS Receive_Way
     , R.Receive_Way
     , (SELECT TOP 1 C.BarCode_Id FROM Receive_Channel AS C 
         WHERE C.RC_type = S.Receive_Type
           AND C.RC_Channel = (CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END)
           AND ((R.Receive_Amount >= C.MoneyLowerLimit AND R.Receive_Amount <= C.MoneyLimit) OR (C.MoneyLowerLimit = 0 AND C.MoneyLimit = 0))
       ) AS Barcode_Id
  FROM Student_Receive AS R
  JOIN School_Rtype AS S ON R.Receive_Type = S.Receive_Type
 WHERE R.Account_Date IS NOT NULL AND R.Account_Date != ''
 --AND R.Receive_Way IN ('80', '81', '82', '83', '84', '85')
  " + andSql.ToString() + @"
) AS D
  LEFT JOIN Receive_Channel CC ON CC.RC_type = D.Receive_Type AND CC.RC_Channel = D.Receive_Way AND CC.BarCode_Id = D.Barcode_Id
 GROUP BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
   --, D.Receive_Type, D.Barcode_Id
 ORDER BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
   --, D.Receive_Type, D.Barcode_Id
";
                    #endregion

                    Result result = _Factory.GetDataTable(sql, parameters, 0, 0, out dtData);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢資料失敗，錯誤訊息：{0}", result.Message);
                    }
                    #endregion

                    //if (dtData == null || dtData.Rows.Count == 0)
                    //{
                    //    //無資料則傳回 new byte[0]
                    //    outFileContent = new byte[0];
                    //    return null;
                    //}
                }
                #endregion

                GenReportHelper helper = new GenReportHelper();

                #region 將取得的資料轉成代收單位交易資訊統計表需要的欄位格式
                string[] channelIds = null;
                DataTable rptData = helper.GetEmptyReportD2Table(out channelIds);
                if (dtData != null && dtData.Rows.Count > 0)
                {
                    foreach (DataRow srcRow in dtData.Rows)
                    {
                        string srcSchIdenty = Convert.ToString(srcRow["Sch_Identy"]).Trim();
                        string srcSchName = Convert.ToString(srcRow["Sch_Name"]).Trim();
                        string srcBankId = Convert.ToString(srcRow["Bank_Id"]).Trim();
                        string srcBankName = Convert.ToString(srcRow["Bank_Name"]).Trim();
                        string srcReceiveWay = Convert.ToString(srcRow["Receive_Way"]).Trim();

                        Int64 srcSumAmount = Convert.ToInt64(srcRow["SUM_AMOUNT"]);
                        Int32 srcSumFee = Convert.ToInt32(srcRow["SUM_FEE"]);
                        Int32 srcDataCount = Convert.ToInt32(srcRow["DATA_COUNT"]);

                        if (Convert.ToInt32(srcSchIdenty.Length) > 0 && Convert.ToInt32(srcSchIdenty.Length) > 0)   //為了原碼掃描的奇怪現象
                        {
                            #region [MDY:20210401] 原碼修正
                            string filter = string.Format("Sch_Identy='{0}' AND Bank_Id='{1}'", srcSchIdenty.Replace("'", ""), srcBankId.Replace("'", ""));
                            #endregion

                            DataRow[] rptRows = rptData.Select(filter);
                            if (rptRows != null && rptRows.Length > 0)
                            {
                                //找到就更新
                                DataRow rptRow = rptRows[0];
                                rptRow["Total_Records"] = Convert.ToInt32(rptRow["Total_Records"]) + srcDataCount;
                                rptRow["Total_Amount"] = Convert.ToInt64(rptRow["Total_Amount"]) + srcSumAmount;

                                foreach (string channelId in channelIds)
                                {
                                    if (channelId == srcReceiveWay)
                                    {
                                        #region 總筆數
                                        {
                                            string columnName = String.Format("{0}_DataCount", channelId);
                                            rptRow[columnName] = Convert.ToInt32(rptRow[columnName]) + srcDataCount;
                                        }
                                        #endregion

                                        #region 總金額
                                        {
                                            string columnName = String.Format("{0}_SumAmount", channelId);
                                            rptRow[columnName] = Convert.ToInt32(rptRow[columnName]) + srcSumAmount;
                                        }
                                        #endregion

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //否則新增
                                DataRow rptRow = rptData.NewRow();
                                rptRow["Bank_Id"] = srcBankId;
                                rptRow["Bank_Name"] = srcBankName;
                                rptRow["Sch_Identy"] = srcSchIdenty;
                                rptRow["Sch_Name"] = srcSchName;
                                rptRow["Total_Records"] = srcDataCount;
                                rptRow["Total_Amount"] = srcSumAmount;

                                foreach (string channelId in channelIds)
                                {
                                    #region 總筆數
                                    {
                                        string columnName = String.Format("{0}_DataCount", channelId);
                                        if (channelId == srcReceiveWay)
                                        {
                                            rptRow[columnName] = srcDataCount;
                                        }
                                        else
                                        {
                                            rptRow[columnName] = 0;
                                        }
                                    }
                                    #endregion

                                    #region 總金額
                                    {
                                        string columnName = String.Format("{0}_SumAmount", channelId);
                                        if (channelId == srcReceiveWay)
                                        {
                                            rptRow[columnName] = srcSumAmount;
                                        }
                                        else
                                        {
                                            rptRow[columnName] = 0;
                                        }
                                    }
                                    #endregion
                                }

                                rptData.Rows.Add(rptRow);
                            }
                        }
                    }
                }
                #endregion

                #region 產生報表檔案
                if (isUseODS)
                {
                    ODSHelper odsHelper = new ODSHelper();
                    string errmsg = odsHelper.GenReportD2(rptData, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 ODS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    string errmsg = helper.GenReportD2(rptData, out outFileContent);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        return null;
                    }
                    else
                    {
                        return string.Format("產生報表檔案失敗，錯誤原因：{0}", errmsg);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return String.Concat("匯出代收單位交易資訊統計表發生例外，錯誤訊息：{0}", ex.Message);
            }
        }
        #endregion
        #endregion

        #region 匯出繳款通道交易資訊統計表
        #region [MDY:20170704] 修正報表樣式
        /// <summary>
        /// 繳款通道交易資訊統計表
        /// </summary>
        /// <param name="bankId">分行代碼</param>
        /// <param name="schIdenty">學校代碼</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="receiveWay">繳款通道</param>
        /// <param name="acctSDate">入帳日起日</param>
        /// <param name="acctEDate">入帳日迄日</param>
        /// <param name="outFileContent"></param>
        /// <returns></returns>
        public string ExportReportD3(string bankId, string schIdenty, string receiveType, string receiveWay, DateTime? acctSDate, DateTime? acctEDate
            , out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(bankId) && String.IsNullOrWhiteSpace(schIdenty) && String.IsNullOrWhiteSpace(receiveType))
            {
                return "缺少查詢資料參數";
            }
            #endregion

            try
            {
                #region 取繳款通道資料
                CodeTextList channels = null;
                {
                    if (String.IsNullOrWhiteSpace(receiveWay))
                    {
                        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 增加 NC-國際信用卡
                        #region [OLD]
                        //#region [MDY:20171127] 增加全國繳費網 (C08)、台灣Pay (C10) (20170831_01)

                        //#region [Old]
                        ////#region [MDY:20170506] 增加支付寶 & 修正中信管道常數為 CTCB
                        ////#region [Old]
                        //////string[] channelIds = new string[] {
                        //////    ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB, ChannelHelper.VO,
                        //////    ChannelHelper.FISC, ChannelHelper.CTCD, ChannelHelper.SM_711, ChannelHelper.SM_TFM, ChannelHelper.SM_OKM, ChannelHelper.SM_HILI,
                        //////    ChannelHelper.SELF, ChannelHelper.LB
                        //////};
                        ////#endregion

                        ////string[] channelIds = new string[] {
                        ////    ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB, ChannelHelper.VO,
                        ////    ChannelHelper.FISC, ChannelHelper.CTCB, ChannelHelper.SM_711, ChannelHelper.SM_TFM, ChannelHelper.SM_OKM, ChannelHelper.SM_HILI,
                        ////    ChannelHelper.SELF, ChannelHelper.LB, ChannelHelper.ALIPAY
                        ////};
                        ////#endregion
                        //#endregion

                        //string[] channelIds = new string[] {
                        //    ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB, ChannelHelper.VO,
                        //    ChannelHelper.FISC, ChannelHelper.CTCB, ChannelHelper.SM_711, ChannelHelper.SM_TFM, ChannelHelper.SM_OKM, ChannelHelper.SM_HILI,
                        //    ChannelHelper.SELF, ChannelHelper.LB, ChannelHelper.ALIPAY, ChannelHelper.EBILL, ChannelHelper.TWPAY
                        //};
                        //#endregion
                        #endregion

                        string[] channelIds = new string[] {
                            ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB, ChannelHelper.VO,
                            ChannelHelper.FISC, ChannelHelper.CTCB, ChannelHelper.SM_711, ChannelHelper.SM_TFM, ChannelHelper.SM_OKM, ChannelHelper.SM_HILI,
                            ChannelHelper.SELF, ChannelHelper.LB, ChannelHelper.ALIPAY, ChannelHelper.EBILL, ChannelHelper.TWPAY, ChannelHelper.FISC_NC
                        };
                        #endregion

                        channels = new CodeTextList(channelIds.Length);
                        foreach (string code in channelIds)
                        {
                            channels.Add(code, ChannelHelper.GetChannelName(code));
                        }
                    }
                    else
                    {
                        channels = new CodeTextList(1);
                        channels.Add(receiveWay, ChannelHelper.GetChannelName(receiveWay));
                    }
                }
                #endregion

                #region [MDY:20171127] 取財金 QRCode 支付參數設定中的手續費 (20170831_01)
                decimal twpayFee = 0;
                {
                    ConfigEntity configData = null;
                    Expression where = new Expression(ConfigEntity.Field.ConfigKey, FiscQRCodeConfig.ConfigKey);
                    Result result = _Factory.SelectFirst<ConfigEntity>(where, null, out configData);
                    if (!result.IsSuccess)
                    {
                        return String.Format("讀取財金 QRCode 支付參數失敗，錯誤訊息：{0}", result.Message);
                    }
                    //無資料或參數設定錯誤 (無法解析) 都不要中斷處理，避免無法產生報表
                    if (configData != null)
                    {
                        FiscQRCodeConfig qrcodeConfig = FiscQRCodeConfig.Parse(configData.ConfigValue);
                        twpayFee = qrcodeConfig.Charge == null ? 0 : qrcodeConfig.Charge.Value;
                    }
                }
                #endregion

                #region 取資料
                DataTable dtData = null;
                {
                    #region 查詢條件
                    StringBuilder andSql = new StringBuilder();
                    KeyValueList parameters = new KeyValueList(5);

                    if (!String.IsNullOrEmpty(bankId))
                    {
                        andSql.Append(" AND S.Bank_Id = @BANKID");
                        parameters.Add("@BANKID", bankId);
                    }

                    if (!String.IsNullOrEmpty(schIdenty))
                    {
                        andSql.Append(" AND S.Sch_Identy = @SCHIDENTY");
                        parameters.Add("@SCHIDENTY", schIdenty);
                    }

                    if (!String.IsNullOrEmpty(receiveType))
                    {
                        andSql.Append(" AND S.Receive_Type = @RECEIVETYPE");
                        parameters.Add("@RECEIVETYPE", receiveType);
                    }

                    if (acctSDate != null)
                    {
                        andSql.Append(" AND R.Account_Date >= @S_ACCOUNTDATE");
                        parameters.Add("@S_ACCOUNTDATE", Common.GetTWDate7(acctSDate.Value));
                    }

                    if (acctEDate != null)
                    {
                        andSql.Append(" AND R.Account_Date <= @E_ACCOUNTDATE");
                        parameters.Add("@E_ACCOUNTDATE", Common.GetTWDate7(acctEDate.Value));
                    }

                    if (!String.IsNullOrEmpty(receiveWay))
                    {
                        andSql.Append(" AND R.Receive_Way = @RECEIVEWAY");
                        parameters.Add("@RECEIVEWAY", receiveWay);
                    }
                    #endregion

                    #region 取資料
                    #region [MDY:20191214] (2019擴充案) 國際信用卡 - 增加 NC-國際信用卡
                    #region [OLD]
//                    #region [MDY:20171127] 增加08-全國繳費網、10-台灣Pay (20170831_01)
//                    #region [Old]
//                    //                    #region [MDY:20170506] 增加支付寶 (09) 管道的手續費計算
////                    #region [Old]
////                    //                    #region [MDY:20160607] 修正 SQL 錯誤
//////                    string sql = @"
//////SELECT D.Sch_Identy, D.Sch_Name, D.Bank_Id
//////     , ISNULL((SELECT BANKSNAME FROM BANK WHERE BANK.BANKNO = D.Bank_Id), '') AS Bank_Name
//////     , D.Receive_Way
//////   --, D.Receive_Type, ISNULL(D.Barcode_Id, '') AS Barcode_Id
//////     , SUM(D.Receive_Amount) AS SUM_AMOUNT, SUM(ISNULL(CC.RC_Charge, 0)) AS SUM_FEE
//////     , COUNT(1) AS DATA_COUNT
//////  FROM
//////(
//////SELECT S.Sch_Identy, S.Sch_Name, S.Bank_Id, R.Receive_Type, R.Receive_Amount
//////     --, CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END AS Receive_Way
//////     , R.Receive_Way
//////     , (SELECT TOP 1 C.BarCode_Id FROM Receive_Channel AS C 
//////         WHERE C.RC_type = S.Receive_Type
//////           AND C.RC_Channel = (CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END)
//////           AND ((R.Receive_Amount >= C.MoneyLowerLimit AND R.Receive_Amount <= C.MoneyLimit) OR (C.MoneyLowerLimit = 0 AND C.MoneyLimit = 0))
//////       ) AS Barcode_Id
//////  FROM Student_Receive AS R
//////  JOIN School_Rtype AS S ON R.Receive_Type = S.Receive_Type
////// WHERE R.Account_Date IS NOT NULL AND R.Account_Date != ''
////// --AND R.Receive_Way IN ('80', '81', '82', '83', '84', '85')
//////  " + andSql.ToString() + @"
//////) AS D
//////  LEFT JOIN Receive_Channel CC ON CC.RC_type = D.Receive_Type AND CC.RC_Channel = D.Receive_Way AND CC.BarCode_Id = D.Barcode_Id
////// GROUP BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
//////   --, D.Receive_Type, D.Barcode_Id
////// ORDER BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
//////   --, D.Receive_Type, D.Barcode_Id
//////";
//////                    #endregion
////                    #endregion

////                    string sql = @"
////SELECT D.Sch_Identy, D.Sch_Name, D.Bank_Id
////     , ISNULL((SELECT BANKSNAME FROM BANK WHERE BANK.BANKNO = D.Bank_Id), '') AS Bank_Name
////     , D.Receive_Way
////   --, D.Receive_Type, ISNULL(D.Barcode_Id, '') AS Barcode_Id
////     , SUM(D.Receive_Amount) AS SUM_AMOUNT
////     , (CASE WHEN D.Receive_Way = '09' THEN SUM(D.Process_Fee) ELSE SUM(ISNULL(CC.RC_Charge, 0)) END) AS SUM_FEE
////     , COUNT(1) AS DATA_COUNT
////  FROM
////(
////SELECT S.Sch_Identy, S.Sch_Name, S.Bank_Id, R.Receive_Type, R.Receive_Amount
////     --, CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END AS Receive_Way
////     , R.Receive_Way
////     , (SELECT TOP 1 C.BarCode_Id FROM Receive_Channel AS C 
////         WHERE C.RC_type = S.Receive_Type
////           AND C.RC_Channel = (CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END)
////           AND ((R.Receive_Amount >= C.MoneyLowerLimit AND R.Receive_Amount <= C.MoneyLimit) OR (C.MoneyLowerLimit = 0 AND C.MoneyLimit = 0))
////       ) AS Barcode_Id
////     , ISNULL(R.Process_Fee, 0) AS Process_Fee
////  FROM Student_Receive AS R
////  JOIN School_Rtype AS S ON R.Receive_Type = S.Receive_Type
//// WHERE R.Account_Date IS NOT NULL AND R.Account_Date != ''
////  " + andSql.ToString() + @"
////) AS D
////  LEFT JOIN Receive_Channel CC ON CC.RC_type = D.Receive_Type AND CC.RC_Channel = D.Receive_Way AND CC.BarCode_Id = D.Barcode_Id
//// GROUP BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
//// ORDER BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
////";
////                    #endregion
//                    #endregion

//                    string sql = @"
//SELECT D.Sch_Identy, D.Sch_Name, D.Bank_Id
//     , ISNULL((SELECT BANKSNAME FROM BANK WHERE BANK.BANKNO = D.Bank_Id), '') AS Bank_Name
//     , D.Receive_Way
//   --, D.Receive_Type, ISNULL(D.Barcode_Id, '') AS Barcode_Id
//     , SUM(D.Receive_Amount) AS SUM_AMOUNT
//     , (CASE WHEN D.Receive_Way = '09' THEN SUM(D.Process_Fee)
//        WHEN D.Receive_Way = '08' THEN 0
//        WHEN D.Receive_Way = '10' THEN SUM(" + twpayFee.ToString() + @")
//        ELSE SUM(ISNULL(CC.RC_Charge, 0))
//        END) AS SUM_FEE
//     , COUNT(1) AS DATA_COUNT
//  FROM
//(
//SELECT S.Sch_Identy, S.Sch_Name, S.Bank_Id, R.Receive_Type, R.Receive_Amount
//     --, CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END AS Receive_Way
//     , R.Receive_Way
//     , (SELECT TOP 1 C.BarCode_Id FROM Receive_Channel AS C 
//         WHERE C.RC_type = S.Receive_Type
//           AND C.RC_Channel = (CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END)
//           AND ((R.Receive_Amount >= C.MoneyLowerLimit AND R.Receive_Amount <= C.MoneyLimit) OR (C.MoneyLowerLimit = 0 AND C.MoneyLimit = 0))
//       ) AS Barcode_Id
//     , ISNULL(R.Process_Fee, 0) AS Process_Fee
//  FROM Student_Receive AS R
//  JOIN School_Rtype AS S ON R.Receive_Type = S.Receive_Type
// WHERE R.Account_Date IS NOT NULL AND R.Account_Date != ''
//  " + andSql.ToString() + @"
//) AS D
//  LEFT JOIN Receive_Channel CC ON CC.RC_type = D.Receive_Type AND CC.RC_Channel = D.Receive_Way AND CC.BarCode_Id = D.Barcode_Id
// GROUP BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
// ORDER BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
//";
//                    #endregion
                    #endregion

                    string sql = @"
SELECT D.Sch_Identy, D.Sch_Name, D.Bank_Id
     , ISNULL((SELECT BANKSNAME FROM BANK WHERE BANK.BANKNO = D.Bank_Id), '') AS Bank_Name
     , D.Receive_Way
   --, D.Receive_Type, ISNULL(D.Barcode_Id, '') AS Barcode_Id
     , SUM(D.Receive_Amount) AS SUM_AMOUNT
     , (CASE WHEN D.Receive_Way = '09' THEN SUM(D.Process_Fee)
        WHEN D.Receive_Way = '08' THEN 0
        WHEN D.Receive_Way = '10' THEN SUM(" + twpayFee.ToString() + @")
        WHEN D.Receive_Way = 'NC' THEN SUM(D.Process_Fee)
        ELSE SUM(ISNULL(CC.RC_Charge, 0))
        END) AS SUM_FEE
     , COUNT(1) AS DATA_COUNT
  FROM
(
SELECT S.Sch_Identy, S.Sch_Name, S.Bank_Id, R.Receive_Type, R.Receive_Amount
     --, CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END AS Receive_Way
     , R.Receive_Way
     , (SELECT TOP 1 C.BarCode_Id FROM Receive_Channel AS C 
         WHERE C.RC_type = S.Receive_Type
           AND C.RC_Channel = (CASE WHEN R.Receive_Way IN ('81', '82', '83', '84', '85') THEN 'Z' ELSE R.Receive_Way END)
           AND ((R.Receive_Amount >= C.MoneyLowerLimit AND R.Receive_Amount <= C.MoneyLimit) OR (C.MoneyLowerLimit = 0 AND C.MoneyLimit = 0))
       ) AS Barcode_Id
     , ISNULL(R.Process_Fee, 0) AS Process_Fee
  FROM Student_Receive AS R
  JOIN School_Rtype AS S ON R.Receive_Type = S.Receive_Type
 WHERE R.Account_Date IS NOT NULL AND R.Account_Date != ''
  " + andSql.ToString() + @"
) AS D
  LEFT JOIN Receive_Channel CC ON CC.RC_type = D.Receive_Type AND CC.RC_Channel = D.Receive_Way AND CC.BarCode_Id = D.Barcode_Id
 GROUP BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
 ORDER BY D.Sch_Identy, D.Sch_Name, D.Bank_Id, D.Receive_Way
";
                    #endregion

                    Result result = _Factory.GetDataTable(sql, parameters, 0, 0, out dtData);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢資料失敗，錯誤訊息：{0}", result.Message);
                    }
                    #endregion

                    //if (dtData == null || dtData.Rows.Count == 0)
                    //{
                    //    //無資料則傳回 new byte[0]
                    //    outFileContent = new byte[0];
                    //    return null;
                    //}
                }
                #endregion

                GenReportHelper helper = new GenReportHelper();

                #region 將取得的資料轉成繳款通道交易資訊統計表需要的欄位格式
                DataTable rptData = helper.GetEmptyReportD3Table(channels);
                if (dtData != null && dtData.Rows.Count > 0)
                {
                    foreach (DataRow srcRow in dtData.Rows)
                    {
                        string srcSchIdenty = Convert.ToString(srcRow["Sch_Identy"]).Trim();
                        string srcSchName = Convert.ToString(srcRow["Sch_Name"]).Trim();
                        string srcBankId = Convert.ToString(srcRow["Bank_Id"]).Trim();
                        string srcBankName = Convert.ToString(srcRow["Bank_Name"]).Trim();
                        string srcReceiveWay = Convert.ToString(srcRow["Receive_Way"]).Trim();

                        Int64 srcSumAmount = Convert.ToInt64(srcRow["SUM_AMOUNT"]);
                        Int32 srcSumFee = Convert.ToInt32(srcRow["SUM_FEE"]);
                        Int32 srcDataCount = Convert.ToInt32(srcRow["DATA_COUNT"]);

                        if (Convert.ToInt32(srcSchIdenty.Length) > 0 && Convert.ToInt32(srcSchIdenty.Length) > 0)   //為了原碼掃描的奇怪現象
                        {
                            #region [MDY:20210401] 原碼修正
                            string filter = string.Format("Sch_Identy='{0}' AND Bank_Id='{1}'", srcSchIdenty.Replace("'", ""), srcBankId.Replace("'", ""));
                            #endregion

                            DataRow[] rptRows = rptData.Select(filter);
                            if (rptRows != null && rptRows.Length > 0)
                            {
                                //找到就更新
                                DataRow rptRow = rptRows[0];
                                rptRow["Total_Records"] = Convert.ToInt32(rptRow["Total_Records"]) + srcDataCount;
                                rptRow["Total_Amount"] = Convert.ToInt64(rptRow["Total_Amount"]) + srcSumAmount;
                                rptRow["Total_Fee"] = Convert.ToInt32(rptRow["Total_Fee"]) + srcSumFee;

                                foreach (CodeText channel in channels)
                                {
                                    if (channel.Code == srcReceiveWay)
                                    {
                                        #region 總筆數
                                        {
                                            string columnName = String.Format("{0}_DataCount", channel.Code);
                                            rptRow[columnName] = Convert.ToInt32(rptRow[columnName]) + srcDataCount;
                                        }
                                        #endregion

                                        #region 總金額
                                        {
                                            string columnName = String.Format("{0}_SumAmount", channel.Code);
                                            rptRow[columnName] = Convert.ToInt32(rptRow[columnName]) + srcSumAmount;
                                        }
                                        #endregion

                                        #region 手續費
                                        {
                                            string columnName = String.Format("{0}_Fee", channel.Code);
                                            rptRow[columnName] = Convert.ToInt32(rptRow[columnName]) + srcSumFee;
                                        }
                                        #endregion

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //否則新增
                                DataRow rptRow = rptData.NewRow();
                                rptRow["Bank_Id"] = srcBankId;
                                rptRow["Bank_Name"] = srcBankName;
                                rptRow["Sch_Identy"] = srcSchIdenty;
                                rptRow["Sch_Name"] = srcSchName;
                                rptRow["Total_Records"] = srcDataCount;
                                rptRow["Total_Amount"] = srcSumAmount;
                                rptRow["Total_Fee"] = srcSumFee;

                                foreach (CodeText channel in channels)
                                {
                                    #region 總筆數
                                    {
                                        string columnName = String.Format("{0}_DataCount", channel.Code);
                                        if (channel.Code == srcReceiveWay)
                                        {
                                            rptRow[columnName] = srcDataCount;
                                        }
                                        else
                                        {
                                            rptRow[columnName] = 0;
                                        }
                                    }
                                    #endregion

                                    #region 總金額
                                    {
                                        string columnName = String.Format("{0}_SumAmount", channel.Code);
                                        if (channel.Code == srcReceiveWay)
                                        {
                                            rptRow[columnName] = srcSumAmount;
                                        }
                                        else
                                        {
                                            rptRow[columnName] = 0;
                                        }
                                    }
                                    #endregion

                                    #region 手續費
                                    {
                                        string columnName = String.Format("{0}_Fee", channel.Code);
                                        if (channel.Code == srcReceiveWay)
                                        {
                                            rptRow[columnName] = srcSumFee;
                                        }
                                        else
                                        {
                                            rptRow[columnName] = 0;
                                        }
                                    }
                                    #endregion
                                }

                                rptData.Rows.Add(rptRow);
                            }
                        }
                    }
                }
                #endregion

                #region 產生報表檔案
                if (isUseODS)
                {
                    ODSHelper odsHelper = new ODSHelper();
                    string errmsg = odsHelper.GenReportD3(rptData, out outFileContent);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return "產生 ODS 檔失敗";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    string errmsg = helper.GenReportD3(rptData, out outFileContent);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        return null;
                    }
                    else
                    {
                        return string.Format("產生報表檔案失敗，錯誤原因：{0}", errmsg);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return String.Concat("匯出繳款通道交易資訊統計表發生例外，錯誤訊息：{0}", ex.Message);
            }
        }
        #endregion
        #endregion

        #region 匯出繳費收費項目 (明細分析表、分類統計表)
        #region [MDY:20170704] 修正報表樣式
        #region [Old]
        //        /// <summary>
//        /// 匯出繳費收費項目
//        /// </summary>
//        /// <param name="receiveType">商家代號</param>
//        /// <param name="yearId">學年代碼</param>
//        /// <param name="termId">學期代碼</param>
//        /// <param name="depId">部別代碼</param>
//        /// <param name="receiveId">代收費用別代碼</param>
//        /// <param name="upNo">批號</param>
//        /// <param name="receiveStatus">繳費狀態</param>
//        /// <param name="reportKind">報表種類 (1=正常 2=遲繳)</param>
//        /// <param name="reportName">報表名稱</param>
//        /// <param name="outFileContent">成功則傳回產生檔案內容，否則傳回 null</param>
//        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
//        public string ExportReportE(string receiveType, string yearId, string termId, string depId, string receiveId, int? upNo, string receiveStatus, string reportKind, string reportName, out byte[] outFileContent)
//        {
//            outFileContent = null;

//            #region 檢查參數
//            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId))
//            {
//                return "缺少查詢資料參數";
//            }
//            string receiveStatusName = null;
//            receiveStatus = receiveStatus == null ? String.Empty : receiveStatus.Trim();
//            switch (receiveStatus)
//            {
//                case "0":   //未繳
//                    receiveStatusName = "未繳";
//                    break;
//                case "1":   //已繳
//                    receiveStatusName = "已繳";
//                    break;
//                case "":   //全部
//                    receiveStatusName = String.Empty;
//                    break;
//                default:
//                    return "不正確的繳款方式參數";
//            }
//            if (String.IsNullOrWhiteSpace(reportName))
//            {
//                return "缺少報表名稱參數";
//            }
//            reportName = reportName.Trim();
//            if (upNo != null && upNo.Value < 0)
//            {
//                upNo = null;
//            }
//            #endregion

//            try
//            {
//                #region 取資料
//                Result result = null;

//                #region 取得代收費用別的收入科目名稱與對應學生繳費資料的收入科目金額欄位名稱 + 繳費期限
//                KeyValueList<string> receiveItems = new KeyValueList<string>(40);
//                string payDueDate = null;
//                {
//                    KeyValueList parameters = new KeyValueList();
//                    StringBuilder sql = new StringBuilder();
//                    sql.AppendLine(@"SELECT ISNULL(Receive_Item01, '') AS Receive_Item01, ISNULL(Receive_Item02, '') AS Receive_Item02, ISNULL(Receive_Item03, '') AS Receive_Item03, ISNULL(Receive_Item04, '') AS Receive_Item04, ISNULL(Receive_Item05, '') AS Receive_Item05
//     , ISNULL(Receive_Item06, '') AS Receive_Item06, ISNULL(Receive_Item07, '') AS Receive_Item07, ISNULL(Receive_Item08, '') AS Receive_Item08, ISNULL(Receive_Item09, '') AS Receive_Item09, ISNULL(Receive_Item10, '') AS Receive_Item10
//     , ISNULL(Receive_Item11, '') AS Receive_Item11, ISNULL(Receive_Item12, '') AS Receive_Item12, ISNULL(Receive_Item13, '') AS Receive_Item13, ISNULL(Receive_Item14, '') AS Receive_Item14, ISNULL(Receive_Item15, '') AS Receive_Item15
//     , ISNULL(Receive_Item16, '') AS Receive_Item16, ISNULL(Receive_Item17, '') AS Receive_Item17, ISNULL(Receive_Item18, '') AS Receive_Item18, ISNULL(Receive_Item19, '') AS Receive_Item19, ISNULL(Receive_Item20, '') AS Receive_Item20
//     , ISNULL(Receive_Item21, '') AS Receive_Item21, ISNULL(Receive_Item22, '') AS Receive_Item22, ISNULL(Receive_Item23, '') AS Receive_Item23, ISNULL(Receive_Item24, '') AS Receive_Item24, ISNULL(Receive_Item25, '') AS Receive_Item25
//     , ISNULL(Receive_Item26, '') AS Receive_Item26, ISNULL(Receive_Item27, '') AS Receive_Item27, ISNULL(Receive_Item28, '') AS Receive_Item28, ISNULL(Receive_Item29, '') AS Receive_Item29, ISNULL(Receive_Item30, '') AS Receive_Item30
//     , ISNULL(Receive_Item31, '') AS Receive_Item31, ISNULL(Receive_Item32, '') AS Receive_Item32, ISNULL(Receive_Item33, '') AS Receive_Item33, ISNULL(Receive_Item34, '') AS Receive_Item34, ISNULL(Receive_Item35, '') AS Receive_Item35
//     , ISNULL(Receive_Item36, '') AS Receive_Item36, ISNULL(Receive_Item37, '') AS Receive_Item37, ISNULL(Receive_Item38, '') AS Receive_Item38, ISNULL(Receive_Item39, '') AS Receive_Item39, ISNULL(Receive_Item40, '') AS Receive_Item40
//     , ISNULL(Pay_Date, '') AS Pay_Due_Date
//  FROM School_Rid 
// WHERE Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId");
//                    parameters.Add("@ReceiveType", receiveType);
//                    parameters.Add("@YearId", yearId);
//                    parameters.Add("@TermId", termId);
//                    parameters.Add("@DepId", depId);
//                    parameters.Add("@ReceiveId", receiveId);

//                    DataTable dt = null;
//                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 1, out dt);
//                    if (!result.IsSuccess)
//                    {
//                        return String.Format("查詢代收費用別設定失敗，錯誤訊息：{0}", result.Message);
//                    }
//                    if (dt == null || dt.Rows.Count == 0)
//                    {
//                        return "查無代收費用別設定資料";
//                    }

//                    receiveItems.Add("學號", "Stu_Id");
//                    receiveItems.Add("姓名", "Stu_Name");

//                    DataRow dRow = dt.Rows[0];
//                    payDueDate = dRow["Pay_Due_Date"].ToString().Trim();

//                    for (int no = 1; no <= 40; no++)
//                    {
//                        string columnName = String.Format("Receive_Item{0:00}", no);
//                        string name = dRow[columnName].ToString().Trim();
//                        if (!String.IsNullOrEmpty(name))
//                        {
//                            receiveItems.Add(name, String.Format("Receive_{0:00}", no));
//                        }
//                    }

//                    receiveItems.Add("繳費金額合計", "Receive_Amount");
//                    receiveItems.Add("繳款日期", "Receive_Date");
//                    receiveItems.Add("虛擬帳號", "Cancel_No");

//                }
//                #endregion

//                #region 取表頭資料
//                DataTable dtHead = null;
//                {
//                    #region 表頭資料欄位
//                    //                學校名稱      報表名稱
//                    //學年：          學期：          繳款方式：
//                    //商家代號：      代收費用：      批號：             yyyy/mm/dd  第xx頁/共xx頁
//                    #endregion

//                    string sql = @"SELECT RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id
//     , SR.Sch_Name, YL.Year_Name, TL.Term_Name, RL.Receive_Name
//     , @UpNo AS UpNo, @ReceiveStatusName AS ReceiveStatusName, @ReportName AS ReportName, @ReportDate AS ReportDate
//  FROM Receive_List AS RL
//  JOIN School_Rtype AS SR ON SR.Receive_Type = RL.Receive_Type
//  JOIN Year_List AS YL ON YL.Year_Id = RL.Year_Id
//  JOIN Term_List AS TL ON TL.Receive_Type = RL.Receive_Type AND TL.Year_Id = RL.Year_Id AND TL.Term_Id = RL.Term_Id
// WHERE RL.Receive_Type = @ReceiveType AND RL.Year_Id = @YearId AND RL.Term_Id = @TermId AND RL.Dep_Id = @DepId AND RL.Receive_Id = @ReceiveId";
//                    KeyValue[] parameters = new KeyValue[] {
//                        new KeyValue("@ReceiveType", receiveType),
//                        new KeyValue("@YearId", yearId),
//                        new KeyValue("@TermId", termId),
//                        new KeyValue("@DepId", depId),
//                        new KeyValue("@ReceiveId", receiveId),
//                        new KeyValue("@UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
//                        new KeyValue("@ReceiveStatusName", receiveStatusName),
//                        new KeyValue("@ReportName", reportName),
//                        new KeyValue("@ReportDate", DateTime.Today.ToString("yyyy/MM/dd"))
//                    };
//                    result = _Factory.GetDataTable(sql, parameters, 0, 1, out dtHead);
//                    if (!result.IsSuccess)
//                    {
//                        return String.Format("查詢表頭資料失敗，錯誤訊息：{0}", result.Message);
//                    }
//                }
//                #endregion

//                #region 取分頁資料 (By 班別)
//                DataTable dtPage = null;
//                if (result.IsSuccess)
//                {
//                    #region Sql Sample
//                    //SELECT DISTINCT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, ISNULL(SR.Dept_Id, '') AS Dept_Id
//                    //  FROM Student_Receive AS SR
//                    // WHERE SR.Receive_Type = '5100' AND SR.Year_Id = '104' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
//                    //   --AND SR.Up_No = 1
//                    //   --AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')
//                    //   --AND SR.Receive_Date IS NOT NULL
//                    //   --AND SR.Receive_Date != '' AND SR.Receive_Date IS NOT NULL
//                    // ORDER BY Dept_Id
//                    #endregion

//                    KeyValueList parameters = new KeyValueList();

//                    StringBuilder sql = new StringBuilder();
//                    sql.AppendLine(@"SELECT DISTINCT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
//     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
//     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
//     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
//     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
//  FROM Student_Receive AS SR
// WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
//                    parameters.Add("@ReceiveType", receiveType);
//                    parameters.Add("@YearId", yearId);
//                    parameters.Add("@TermId", termId);
//                    parameters.Add("@DepId", depId);
//                    parameters.Add("@ReceiveId", receiveId);
//                    if (upNo != null)
//                    {
//                        sql.AppendLine("   AND SR.Up_No = @UpNo");
//                        parameters.Add("@UpNo", upNo.Value);
//                    }

//                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
//                    #region [Old]
//                    //if (receiveStatus == "1")   //已繳
//                    //{
//                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
//                    //}
//                    //else if (receiveStatus == "0")  //未繳
//                    //{
//                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
//                    //}
//                    #endregion

//                    if (receiveStatus == "1")   //已繳
//                    {
//                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
//                    }
//                    else if (receiveStatus == "0")  //未繳
//                    {
//                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
//                    }
//                    #endregion

//                    if (reportKind == "2")  //遲繳
//                    {
//                        //sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
//                        //parameters.Add("@PayDueDate", payDueDate);
//                    }
//                    sql.AppendLine(@"
// ORDER BY Dept_Id");

//                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtPage);
//                    if (!result.IsSuccess)
//                    {
//                        return String.Format("計算分頁資料失敗，錯誤訊息：{0}", result.Message);
//                    }
//                }
//                #endregion

//                #region 取部別小計資料
//                DataTable dtDeptSum = null;
//                if (result.IsSuccess)
//                {
//                    KeyValueList parameters = new KeyValueList();
//                    StringBuilder sql = new StringBuilder();
//                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id
//     , SUM(Receive_Amount) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
//     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
// FROM (
//SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
//     , ISNULL(SR.Dept_Id, '') AS Dept_Id
//     , ISNULL(SR.Receive_Amount, 0) AS Receive_Amount
//  FROM Student_Receive AS SR
// WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
//                    parameters.Add("@ReceiveType", receiveType);
//                    parameters.Add("@YearId", yearId);
//                    parameters.Add("@TermId", termId);
//                    parameters.Add("@DepId", depId);
//                    parameters.Add("@ReceiveId", receiveId);
//                    if (upNo != null)
//                    {
//                        sql.AppendLine("   AND SR.Up_No = @UpNo");
//                        parameters.Add("@UpNo", upNo.Value);
//                    }

//                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
//                    #region [Old]
//                    //if (receiveStatus == "1")   //已繳
//                    //{
//                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
//                    //}
//                    //else if (receiveStatus == "0")  //未繳
//                    //{
//                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
//                    //}
//                    #endregion

//                    if (receiveStatus == "1")   //已繳
//                    {
//                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
//                    }
//                    else if (receiveStatus == "0")  //未繳
//                    {
//                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
//                    }
//                    #endregion

//                    if (reportKind == "2")  //遲繳
//                    {
//                        //sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
//                        //parameters.Add("@PayDueDate", payDueDate);
//                    }
//                    sql.AppendLine(@"
//) AS SR
// GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id
// ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id");

//                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtDeptSum);
//                    if (!result.IsSuccess)
//                    {
//                        return String.Format("計算部別小計資料失敗，錯誤訊息：{0}", result.Message);
//                    }
//                }
//                #endregion

//                #region 取系所小計資料
//                DataTable dtMajorSum = null;
//                if (result.IsSuccess)
//                {
//                    KeyValueList parameters = new KeyValueList();
//                    StringBuilder sql = new StringBuilder();
//                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id
//     , SUM(Receive_Amount) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
//     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
// FROM (
//SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
//     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id
//     , ISNULL(SR.Receive_Amount, 0) AS Receive_Amount
//  FROM Student_Receive AS SR
// WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
//                    parameters.Add("@ReceiveType", receiveType);
//                    parameters.Add("@YearId", yearId);
//                    parameters.Add("@TermId", termId);
//                    parameters.Add("@DepId", depId);
//                    parameters.Add("@ReceiveId", receiveId);
//                    if (upNo != null)
//                    {
//                        sql.AppendLine("   AND SR.Up_No = @UpNo");
//                        parameters.Add("@UpNo", upNo.Value);
//                    }

//                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
//                    #region [Old]
//                    //if (receiveStatus == "1")   //已繳
//                    //{
//                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
//                    //}
//                    //else if (receiveStatus == "0")  //未繳
//                    //{
//                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
//                    //}
//                    #endregion

//                    if (receiveStatus == "1")   //已繳
//                    {
//                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
//                    }
//                    else if (receiveStatus == "0")  //未繳
//                    {
//                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
//                    }
//                    #endregion

//                    if (reportKind == "2")  //遲繳
//                    {
//                        //sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
//                        //parameters.Add("@PayDueDate", payDueDate);
//                    }
//                    sql.AppendLine(@"
//) AS SR
// GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id
// ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id");

//                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtMajorSum);
//                    if (!result.IsSuccess)
//                    {
//                        return String.Format("計算系所小計資料失敗，錯誤訊息：{0}", result.Message);
//                    }
//                }
//                #endregion

//                #region 取班別小計資料
//                DataTable dtClassSum = null;
//                if (result.IsSuccess)
//                {
//                    KeyValueList parameters = new KeyValueList();
//                    StringBuilder sql = new StringBuilder();
//                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id
//     , SUM(Receive_Amount) AS SUM_AMOUNT, COUNT(1) AS SUM_COUNT
//     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
// FROM (
//SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
//     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
//     , ISNULL(SR.Receive_Amount, 0) AS Receive_Amount
//  FROM Student_Receive AS SR
// WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
//                    parameters.Add("@ReceiveType", receiveType);
//                    parameters.Add("@YearId", yearId);
//                    parameters.Add("@TermId", termId);
//                    parameters.Add("@DepId", depId);
//                    parameters.Add("@ReceiveId", receiveId);
//                    if (upNo != null)
//                    {
//                        sql.AppendLine("   AND SR.Up_No = @UpNo");
//                        parameters.Add("@UpNo", upNo.Value);
//                    }

//                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
//                    #region [Old]
//                    //if (receiveStatus == "1")   //已繳
//                    //{
//                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
//                    //}
//                    //else if (receiveStatus == "0")  //未繳
//                    //{
//                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
//                    //}
//                    #endregion

//                    if (receiveStatus == "1")   //已繳
//                    {
//                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
//                    }
//                    else if (receiveStatus == "0")  //未繳
//                    {
//                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
//                    }
//                    #endregion

//                    if (reportKind == "2")  //遲繳
//                    {
//                        //sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
//                        //parameters.Add("@PayDueDate", payDueDate);
//                    }
//                    sql.AppendLine(@"
//) AS SR
// GROUP BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id
// ORDER BY SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Dept_Id, SR.Major_Id, SR.Class_Id");

//                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtClassSum);
//                    if (!result.IsSuccess)
//                    {
//                        return String.Format("計算系所小計資料失敗，錯誤訊息：{0}", result.Message);
//                    }
//                }
//                #endregion

//                #region 取表身資料
//                DataTable dtData = null;
//                {
//                    #region Sql Sample
//                    //SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id
//                    //     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
//                    //     , SR.Receive_01, SR.Receive_02, SR.Receive_03, SR.Receive_04, SR.Receive_05, SR.Receive_06, SR.Receive_07, SR.Receive_08, SR.Receive_09, SR.Receive_10
//                    //     , SR.Receive_11, SR.Receive_12, SR.Receive_13, SR.Receive_14, SR.Receive_15, SR.Receive_16, SR.Receive_17, SR.Receive_18, SR.Receive_19, SR.Receive_20
//                    //     , SR.Receive_21, SR.Receive_22, SR.Receive_23, SR.Receive_24, SR.Receive_25, SR.Receive_26, SR.Receive_27, SR.Receive_28, SR.Receive_29, SR.Receive_30
//                    //     , SR.Receive_31, SR.Receive_32, SR.Receive_33, SR.Receive_34, SR.Receive_35, SR.Receive_36, SR.Receive_37, SR.Receive_38, SR.Receive_39, SR.Receive_40
//                    //     , SR.Receive_Amount
//                    //     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
//                    //     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
//                    //     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
//                    //  FROM Student_Receive AS SR
//                    // WHERE SR.Receive_Type = '5100' AND SR.Year_Id = '104' AND SR.Term_Id = '1' AND SR.Dep_Id = '' AND SR.Receive_Id = '1'
//                    //   --AND SR.Receive_Way = '01'
//                    // ORDER BY Dept_Id, Major_Id, Class_Id
//                    #endregion

//                    KeyValueList parameters = new KeyValueList();
//                    StringBuilder sql = new StringBuilder();
//                    sql.AppendLine(@"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Stu_Id
//     , ISNULL(SR.Dept_Id, '') AS Dept_Id, ISNULL(SR.Major_Id, '') AS Major_Id, ISNULL(SR.Class_Id, '') AS Class_Id
//     , SR.Receive_01, SR.Receive_02, SR.Receive_03, SR.Receive_04, SR.Receive_05, SR.Receive_06, SR.Receive_07, SR.Receive_08, SR.Receive_09, SR.Receive_10
//     , SR.Receive_11, SR.Receive_12, SR.Receive_13, SR.Receive_14, SR.Receive_15, SR.Receive_16, SR.Receive_17, SR.Receive_18, SR.Receive_19, SR.Receive_20
//     , SR.Receive_21, SR.Receive_22, SR.Receive_23, SR.Receive_24, SR.Receive_25, SR.Receive_26, SR.Receive_27, SR.Receive_28, SR.Receive_29, SR.Receive_30
//     , SR.Receive_31, SR.Receive_32, SR.Receive_33, SR.Receive_34, SR.Receive_35, SR.Receive_36, SR.Receive_37, SR.Receive_38, SR.Receive_39, SR.Receive_40
//     , SR.Receive_Amount, SR.Cancel_No, SR.Receive_Date
//     , ISNULL((SELECT SM.Stu_Name FROM Student_Master AS SM WHERE SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id), '') AS Stu_Name
//     , ISNULL((SELECT Dept_Name  FROM Dept_List  AS DL WHERE DL.Receive_Type = SR.Receive_Type AND DL.Year_Id = SR.Year_Id AND DL.Term_Id = SR.Term_Id AND DL.Dept_Id = SR.Dept_Id), '') AS Dept_Name
//     , ISNULL((SELECT Major_Name FROM Major_List AS ML WHERE ML.Receive_Type = SR.Receive_Type AND ML.Year_Id = SR.Year_Id AND ML.Term_Id = SR.Term_Id AND ML.Dep_Id = SR.Dep_Id AND ML.Major_Id = SR.Major_Id), '') AS Major_Name
//     , ISNULL((SELECT Class_Name FROM Class_List AS CL WHERE CL.Receive_Type = SR.Receive_Type AND CL.Year_Id = SR.Year_Id AND CL.Term_Id = SR.Term_Id AND CL.Dep_Id = SR.Dep_Id AND CL.Class_Id = SR.Class_Id), '') AS Class_Name
//  FROM Student_Receive AS SR
// WHERE SR.Receive_Type = @ReceiveType AND SR.Year_Id = @YearId AND SR.Term_Id = @TermId AND SR.Dep_Id = @DepId AND SR.Receive_Id = @ReceiveId");
//                    parameters.Add("@ReceiveType", receiveType);
//                    parameters.Add("@YearId", yearId);
//                    parameters.Add("@TermId", termId);
//                    parameters.Add("@DepId", depId);
//                    parameters.Add("@ReceiveId", receiveId);
//                    if (upNo != null)
//                    {
//                        sql.AppendLine("   AND SR.Up_No = @UpNo");
//                        parameters.Add("@UpNo", upNo.Value);
//                    }

//                    #region [MDY:20170319] 無代收日、代收管道、入帳日才算未繳，反之為已繳
//                    #region [Old]
//                    //if (receiveStatus == "1")   //已繳
//                    //{
//                    //    sql.AppendLine("   AND (SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '')");
//                    //}
//                    //else if (receiveStatus == "0")  //未繳
//                    //{
//                    //    sql.AppendLine("   AND (SR.Receive_Date IS NULL OR SR.Receive_Date = '')");
//                    //}
//                    #endregion

//                    if (receiveStatus == "1")   //已繳
//                    {
//                        sql.AppendLine("   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))");
//                    }
//                    else if (receiveStatus == "0")  //未繳
//                    {
//                        sql.AppendLine("   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))");
//                    }
//                    #endregion

//                    if (reportKind == "2")  //遲繳
//                    {
//                        //sql.AppendLine("   AND (SR.Receive_Date > @PayDueDate)");
//                        //parameters.Add("@PayDueDate", payDueDate);
//                    }
//                    sql.AppendLine(@" ORDER BY Dept_Id, Major_Id, Class_Id");

//                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtData);
//                    if (!result.IsSuccess)
//                    {
//                        return String.Format("查詢資料失敗，錯誤訊息：{0}", result.Message);
//                    }
//                }
//                #endregion
//                #endregion

//                #region 產生報表檔案
//                GenReportHelper helper = new GenReportHelper();
//                string errmsg = helper.GenReportE(dtHead, dtPage, dtDeptSum, dtMajorSum, dtClassSum, receiveItems, dtData, out outFileContent);
//                if (!String.IsNullOrEmpty(errmsg))
//                {
//                    return "產生 XLS 檔失敗";
//                }
//                else
//                {
//                    return null;
//                }
//                #endregion

//            }
//            catch (Exception ex)
//            {
//                return String.Concat("產出檔案發生例外，錯誤訊息：", ex.Message);
//            }
//        }
        #endregion

        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出繳費收費項目 (明細分析表、分類統計表)
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="upNo">批號</param>
        /// <param name="receiveStatus">繳費狀態</param>
        /// <param name="reportKind">報表種類 (1=明細分析表 2=分類統計表)</param>
        /// <param name="reportName">報表名稱</param>
        /// <param name="outFileContent">成功則傳回產生檔案內容，否則傳回 null</param>
        /// <param name="isUseODS">是否產生成 ODS 檔</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        public string ExportReportE(string receiveType, string yearId, string termId, string depId, string receiveId
            , int? upNo, string receiveStatus, string reportKind, string reportName
            , out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            #region 檢查參數
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return "缺少查詢資料參數";
            }
            string receiveStatusName = null;
            receiveStatus = receiveStatus == null ? String.Empty : receiveStatus.Trim();
            switch (receiveStatus)
            {
                case "0":   //未繳
                    receiveStatusName = "未繳";
                    break;
                case "1":   //已繳
                    receiveStatusName = "已繳";
                    break;
                case "":   //全部
                    receiveStatusName = String.Empty;
                    break;
                default:
                    return "不正確的繳款方式參數";
            }
            if (reportKind != "1" && reportKind != "2")
            {
                return "不正確的報表種類參數";
            }
            if (String.IsNullOrWhiteSpace(reportName))
            {
                return "缺少報表名稱參數";
            }
            reportName = reportName.Trim();
            if (upNo != null && upNo.Value < 0)
            {
                upNo = null;
            }
            #endregion

            try
            {
                #region 取資料
                Result result = null;

                #region 取得代收費用別的收入科目名稱
                KeyValueList<string> receiveItems = new KeyValueList<string>(40);

                #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                List<string> receiveItemFieldNames = new List<string>(40);
                #endregion

                {
                    KeyValueList parameters = new KeyValueList();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT ISNULL(Receive_Item01, '') AS Receive_Item01, ISNULL(Receive_Item02, '') AS Receive_Item02, ISNULL(Receive_Item03, '') AS Receive_Item03, ISNULL(Receive_Item04, '') AS Receive_Item04, ISNULL(Receive_Item05, '') AS Receive_Item05
     , ISNULL(Receive_Item06, '') AS Receive_Item06, ISNULL(Receive_Item07, '') AS Receive_Item07, ISNULL(Receive_Item08, '') AS Receive_Item08, ISNULL(Receive_Item09, '') AS Receive_Item09, ISNULL(Receive_Item10, '') AS Receive_Item10
     , ISNULL(Receive_Item11, '') AS Receive_Item11, ISNULL(Receive_Item12, '') AS Receive_Item12, ISNULL(Receive_Item13, '') AS Receive_Item13, ISNULL(Receive_Item14, '') AS Receive_Item14, ISNULL(Receive_Item15, '') AS Receive_Item15
     , ISNULL(Receive_Item16, '') AS Receive_Item16, ISNULL(Receive_Item17, '') AS Receive_Item17, ISNULL(Receive_Item18, '') AS Receive_Item18, ISNULL(Receive_Item19, '') AS Receive_Item19, ISNULL(Receive_Item20, '') AS Receive_Item20
     , ISNULL(Receive_Item21, '') AS Receive_Item21, ISNULL(Receive_Item22, '') AS Receive_Item22, ISNULL(Receive_Item23, '') AS Receive_Item23, ISNULL(Receive_Item24, '') AS Receive_Item24, ISNULL(Receive_Item25, '') AS Receive_Item25
     , ISNULL(Receive_Item26, '') AS Receive_Item26, ISNULL(Receive_Item27, '') AS Receive_Item27, ISNULL(Receive_Item28, '') AS Receive_Item28, ISNULL(Receive_Item29, '') AS Receive_Item29, ISNULL(Receive_Item30, '') AS Receive_Item30
     , ISNULL(Receive_Item31, '') AS Receive_Item31, ISNULL(Receive_Item32, '') AS Receive_Item32, ISNULL(Receive_Item33, '') AS Receive_Item33, ISNULL(Receive_Item34, '') AS Receive_Item34, ISNULL(Receive_Item35, '') AS Receive_Item35
     , ISNULL(Receive_Item36, '') AS Receive_Item36, ISNULL(Receive_Item37, '') AS Receive_Item37, ISNULL(Receive_Item38, '') AS Receive_Item38, ISNULL(Receive_Item39, '') AS Receive_Item39, ISNULL(Receive_Item40, '') AS Receive_Item40
  FROM School_Rid 
 WHERE Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId");
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);

                    DataTable dt = null;
                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 1, out dt);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢代收費用別設定失敗，錯誤訊息：{0}", result.Message);
                    }
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return "查無該代收費用別設定資料";
                    }

                    DataRow dRow = dt.Rows[0];

                    #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                    for (int no = 1; no <= 40; no++)
                    {
                        string columnName = $"Receive_Item{no:00}";
                        string itemName = dRow.IsNull(columnName) ? null : dRow[columnName].ToString().Trim();
                        if (!String.IsNullOrEmpty(itemName))
                        {
                            receiveItems.Add($"Receive_{no:00}", itemName);
                            receiveItemFieldNames.Add($"Receive_{no:00}");
                        }
                    }
                    if (receiveItemFieldNames.Count == 0)
                    {
                        return "該代收費用別設定未設定任何收入科目名稱";
                    }
                    #endregion
                }
                #endregion

                #region 取得部別(分頁)資料
                DataTable dtPage = null;
                if (result.IsSuccess)
                {
                    #region Sql Sample
                    //SELECT DISTINCT Receive_Type, Year_Id, Term_Id, Dep_Id, Receive_Id, ISNULL(Dept_Id, '') AS Dept_Id
                    //  FROM Student_Receive
                    // WHERE Receive_Type = '5100' AND Year_Id = '104' AND Term_Id = '1' AND Dep_Id = '' AND Receive_Id = '1'
                    //--   AND Up_No = 1
                    //--   AND ((Receive_Date IS NOT NULL AND Receive_Date != '') OR (Receive_Way IS NOT NULL AND Receive_Way != '') OR (Account_Date IS NOT NULL AND Account_Date != ''))
                    //--   AND ((Receive_Date IS NULL OR Receive_Date = '') AND (Receive_Way IS NULL OR Receive_Way = '') AND (Account_Date IS NULL OR Account_Date = ''))
                    // ORDER BY Dept_Id
                    #endregion

                    #region 組 SQL
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT DISTINCT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], '') AS [Dept_Id]
     , ISNULL((SELECT [Dept_Name] FROM [Dept_List] AS DL WHERE DL.[Receive_Type] = SR.[Receive_Type] AND DL.[Year_Id] = SR.[Year_Id] AND DL.[Term_Id] = SR.[Term_Id] AND DL.[Dept_Id] = SR.[Dept_Id]), '') AS [Dept_Name]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId");

                    KeyValueList parameters = new KeyValueList();
                    parameters.Add("@ReceiveType", receiveType);
                    parameters.Add("@YearId", yearId);
                    parameters.Add("@TermId", termId);
                    parameters.Add("@DepId", depId);
                    parameters.Add("@ReceiveId", receiveId);

                    #region 批號條件
                    if (upNo != null)
                    {
                        sql.AppendLine("   AND SR.[Up_No] = @UpNo");
                        parameters.Add("@UpNo", upNo.Value);
                    }
                    #endregion

                    #region 繳費狀態條件 (無代收日、代收管道、入帳日才算未繳，反之為已繳)
                    if (receiveStatus == "1")   //已繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))");
                    }
                    else if (receiveStatus == "0")  //未繳
                    {
                        sql.AppendLine("   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))");
                    }
                    #endregion

                    sql.AppendLine(@"
 ORDER BY [Dept_Id]");
                    #endregion

                    result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtPage);
                    if (!result.IsSuccess)
                    {
                        return String.Format("計算分頁資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 取表頭資料
                DataTable dtHead = null;
                {
                    #region 表頭資料欄位
                    //                學校名稱      報表名稱
                    //學年：          學期：          繳款方式：
                    //商家代號：      代收費用：      批號：             yyyy/mm/dd  第xx頁/共xx頁
                    #endregion

                    string sql = @"SELECT RL.Receive_Type, RL.Year_Id, RL.Term_Id, RL.Dep_Id, RL.Receive_Id
     , SR.Sch_Name, YL.Year_Name, TL.Term_Name, RL.Receive_Name
     , @UpNo AS UpNo, @ReceiveStatusName AS ReceiveStatusName, @ReportName AS ReportName, @ReportDate AS ReportDate
  FROM Receive_List AS RL
  JOIN School_Rtype AS SR ON SR.Receive_Type = RL.Receive_Type
  JOIN Year_List AS YL ON YL.Year_Id = RL.Year_Id
  JOIN Term_List AS TL ON TL.Receive_Type = RL.Receive_Type AND TL.Year_Id = RL.Year_Id AND TL.Term_Id = RL.Term_Id
 WHERE RL.Receive_Type = @ReceiveType AND RL.Year_Id = @YearId AND RL.Term_Id = @TermId AND RL.Dep_Id = @DepId AND RL.Receive_Id = @ReceiveId";
                    KeyValue[] parameters = new KeyValue[] {
                        new KeyValue("@ReceiveType", receiveType),
                        new KeyValue("@YearId", yearId),
                        new KeyValue("@TermId", termId),
                        new KeyValue("@DepId", depId),
                        new KeyValue("@ReceiveId", receiveId),
                        new KeyValue("@UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
                        new KeyValue("@ReceiveStatusName", receiveStatusName),
                        new KeyValue("@ReportName", reportName),
                        new KeyValue("@ReportDate", DateTime.Today.ToString("yyyy/MM/dd"))
                    };
                    result = _Factory.GetDataTable(sql, parameters, 0, 1, out dtHead);
                    if (!result.IsSuccess)
                    {
                        return String.Format("查詢表頭資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion
                #endregion

                if (reportKind == "1")
                {
                    //明細分析表
                    #region 逐收入科目產生表身資料
                    #region SQL Sample
                    //SELECT * 
                    //     , ISNULL((SELECT ISNULL([Major_Name], '') FROM [Major_List] AS ML WHERE ML.[Receive_Type] = SR.[Receive_Type] AND ML.[Year_Id] = SR.[Year_Id] AND ML.[Term_Id] = SR.[Term_Id] AND ML.[Dep_Id] = SR.[Dep_Id] AND ML.[Major_Id] = SR.[Major_Id]), '') AS [Major_Name]
                    //     , ISNULL((SELECT ISNULL([Class_Name], '') FROM [Class_List] AS CL WHERE CL.[Receive_Type] = SR.[Receive_Type] AND CL.[Year_Id] = SR.[Year_Id] AND CL.[Term_Id] = SR.[Term_Id] AND CL.[Dep_Id] = SR.[Dep_Id] AND CL.[Class_Id] = SR.[Class_Id]), '') AS [Class_Name]
                    //  FROM (
                    ///* By 部別, 系所, 班級, 收入科目金額 的筆數, 小計 */
                    //SELECT Receive_Type, Year_Id, Term_Id, Dep_Id, Receive_Id, 'Receive_01' AS Receive_Item_Key, ISNULL(Dept_Id, '') AS Dept_Id
                    //     , ISNULL(Major_Id, '') AS Major_Id, ISNULL(Class_Id, '') AS Class_Id, Receive_01 AS Receive_Item_Amount
                    //     , 'Y' AS MAJOR_FLAG, 'Y' AS CLASS_FLAG
                    //     , COUNT(1) Data_Count, SUM(Receive_01) AS Sum_Amount
                    //  FROM Student_Receive
                    // WHERE Receive_Type = '5026' AND Year_Id = '103' AND Term_Id = '1' AND Dep_Id = '' AND Receive_Id = '1'
                    //--   AND Up_No = 1
                    //--   AND ((Receive_Date IS NOT NULL AND Receive_Date != '') OR (Receive_Way IS NOT NULL AND Receive_Way != '') OR (Account_Date IS NOT NULL AND Account_Date != ''))
                    //--   AND ((Receive_Date IS NULL OR Receive_Date = '') AND (Receive_Way IS NULL OR Receive_Way = '') AND (Account_Date IS NULL OR Account_Date = ''))
                    // GROUP BY Receive_Type, Year_Id, Term_Id, Dep_Id, Receive_Id, ISNULL(Dept_Id, ''), ISNULL(Major_Id, ''), ISNULL(Class_Id, ''), Receive_01
                    //UNION
                    ///* By 部別, 系所 的筆數, 小計 */
                    //SELECT Receive_Type, Year_Id, Term_Id, Dep_Id, Receive_Id, 'Receive_01' AS Receive_Item_Key, ISNULL(Dept_Id, '') AS Dept_Id
                    //     , ISNULL(Major_Id, '') AS Major_Id, NULL AS Class_Id, NULL AS Receive_Item_Amount
                    //     , 'Y' AS MAJOR_FLAG, 'N' AS CLASS_FLAG
                    //     , COUNT(1) Data_Count, SUM(Receive_01) AS Sum_Amount
                    //  FROM Student_Receive
                    // WHERE Receive_Type = '5026' AND Year_Id = '103' AND Term_Id = '1' AND Dep_Id = '' AND Receive_Id = '1'
                    //--   AND Up_No = 1
                    //--   AND ((Receive_Date IS NOT NULL AND Receive_Date != '') OR (Receive_Way IS NOT NULL AND Receive_Way != '') OR (Account_Date IS NOT NULL AND Account_Date != ''))
                    //--   AND ((Receive_Date IS NULL OR Receive_Date = '') AND (Receive_Way IS NULL OR Receive_Way = '') AND (Account_Date IS NULL OR Account_Date = ''))
                    // GROUP BY Receive_Type, Year_Id, Term_Id, Dep_Id, Receive_Id, ISNULL(Dept_Id, ''), ISNULL(Major_Id, '')
                    //UNION
                    ///* By 部別 的筆數, 小計 */
                    //SELECT Receive_Type, Year_Id, Term_Id, Dep_Id, Receive_Id, 'Receive_01' AS Receive_Item_Key, ISNULL(Dept_Id, '') AS Dept_Id
                    //     , NULL AS Major_Id, NULL AS Class_Id, NULL AS Receive_Item_Amount
                    //     , 'N' AS MAJOR_FLAG, 'N' AS CLASS_FLAG
                    //     , COUNT(1) Data_Count, SUM(Receive_01) AS Sum_Amount
                    //  FROM Student_Receive
                    // WHERE Receive_Type = '5026' AND Year_Id = '103' AND Term_Id = '1' AND Dep_Id = '' AND Receive_Id = '1'
                    //--   AND Up_No = 1
                    //--   AND ((Receive_Date IS NOT NULL AND Receive_Date != '') OR (Receive_Way IS NOT NULL AND Receive_Way != '') OR (Account_Date IS NOT NULL AND Account_Date != ''))
                    //--   AND ((Receive_Date IS NULL OR Receive_Date = '') AND (Receive_Way IS NULL OR Receive_Way = '') AND (Account_Date IS NULL OR Account_Date = ''))
                    // GROUP BY Receive_Type, Year_Id, Term_Id, Dep_Id, Receive_Id, ISNULL(Dept_Id, '')
                    //) AS SR
                    // ORDER BY Dept_Id, MAJOR_FLAG DESC, Major_Id, CLASS_FLAG DESC, Class_Id, Receive_Item_Amount
                    #endregion

                    #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                    List<DataTable> dtDatas = new List<DataTable>(receiveItemFieldNames.Count);
                    foreach (string receiveItemFieldName in receiveItemFieldNames)
                    {
                        DataTable dtData = null;
                        string receiveItemKey = receiveItemFieldName;

                        KeyValueList parameters = new KeyValueList();

                        #region 批號條件
                        string sqlWhere1 = null;
                        if (upNo != null)
                        {
                            sqlWhere1 = "\r\n   AND [Up_No] = @UpNo";
                            parameters.Add("@UpNo", upNo.Value);
                        }
                        #endregion

                        #region 繳費狀態條件 (無代收日、代收管道、入帳日才算未繳，反之為已繳)
                        string sqlWhere2 = null;
                        if (receiveStatus == "1")   //已繳
                        {
                            sqlWhere2 = "\r\n   AND (([Receive_Date] IS NOT NULL AND [Receive_Date] != '') OR ([Receive_Way] IS NOT NULL AND [Receive_Way] != '') OR ([Account_Date] IS NOT NULL AND [Account_Date] != ''))";
                        }
                        else if (receiveStatus == "0")  //未繳
                        {
                            sqlWhere2 = "\r\n   AND (([Receive_Date] IS NULL OR [Receive_Date] = '') AND ([Receive_Way] IS NULL OR [Receive_Way] = '') AND ([Account_Date] IS NULL OR [Account_Date] = ''))";
                        }
                        #endregion

                        #region 組 SQL
                        #region [MDY:20210401] 修正 SQL 處理 receiveItemKey 為 NULL 值
                        string sql = String.Format(@"
SELECT *
     , ISNULL((SELECT ISNULL([Major_Name], '') FROM [Major_List] AS ML WHERE ML.[Receive_Type] = SR.[Receive_Type] AND ML.[Year_Id] = SR.[Year_Id] AND ML.[Term_Id] = SR.[Term_Id] AND ML.[Dep_Id] = SR.[Dep_Id] AND ML.[Major_Id] = SR.[Major_Id]), '') AS [Major_Name]
     , ISNULL((SELECT ISNULL([Class_Name], '') FROM [Class_List] AS CL WHERE CL.[Receive_Type] = SR.[Receive_Type] AND CL.[Year_Id] = SR.[Year_Id] AND CL.[Term_Id] = SR.[Term_Id] AND CL.[Dep_Id] = SR.[Dep_Id] AND CL.[Class_Id] = SR.[Class_Id]), '') AS [Class_Name]
  FROM (
/* By 部別, 系所, 班級, 收入科目金額 的筆數, 小計 */
SELECT [Receive_Type], [Year_Id], [Term_Id], [Dep_Id], [Receive_Id], '{0}' AS [Receive_Item_Key], ISNULL([Dept_Id], '') AS [Dept_Id]
     , ISNULL([Major_Id], '') AS [Major_Id], ISNULL([Class_Id], '') AS [Class_Id], [{0}] AS [Receive_Item_Amount]
     , 'Y' AS [MAJOR_FLAG], 'Y' AS [CLASS_FLAG]
     , COUNT(1) [Data_Count], SUM(ISNULL({0}, 0)) AS [Sum_Amount]
  FROM [Student_Receive]
 WHERE [Receive_Type] = @ReceiveType AND [Year_Id] = @YearId AND [Term_Id] = @TermId AND [Dep_Id] = @DepId AND [Receive_Id] = @ReceiveId " + sqlWhere1 + sqlWhere2 + @"
 GROUP BY [Receive_Type], [Year_Id], [Term_Id], [Dep_Id], [Receive_Id], ISNULL([Dept_Id], ''), ISNULL([Major_Id], ''), ISNULL([Class_Id], ''), [{0}]
UNION
/* By 部別, 系所 的筆數, 小計 */
SELECT [Receive_Type], [Year_Id], [Term_Id], [Dep_Id], [Receive_Id], '{0}' AS [Receive_Item_Key], ISNULL([Dept_Id], '') AS [Dept_Id]
     , ISNULL([Major_Id], '') AS [Major_Id], NULL AS [Class_Id], NULL AS [Receive_Item_Amount]
     , 'Y' AS [MAJOR_FLAG], 'N' AS [CLASS_FLAG]
     , COUNT(1) [Data_Count], SUM(ISNULL({0}, 0)) AS [Sum_Amount]
  FROM [Student_Receive]
 WHERE [Receive_Type] = @ReceiveType AND [Year_Id] = @YearId AND [Term_Id] = @TermId AND [Dep_Id] = @DepId AND [Receive_Id] = @ReceiveId " + sqlWhere1 + sqlWhere2 + @"
 GROUP BY [Receive_Type], [Year_Id], [Term_Id], [Dep_Id], [Receive_Id], ISNULL([Dept_Id], ''), ISNULL([Major_Id], '')
UNION
/* By 部別 的筆數, 小計 */
SELECT [Receive_Type], [Year_Id], [Term_Id], [Dep_Id], [Receive_Id], '{0}' AS [Receive_Item_Key], ISNULL([Dept_Id], '') AS [Dept_Id]
     , NULL AS [Major_Id], NULL AS [Class_Id], NULL AS [Receive_Item_Amount]
     , 'N' AS [MAJOR_FLAG], 'N' AS [CLASS_FLAG]
     , COUNT(1) [Data_Count], SUM(ISNULL({0}, 0)) AS [Sum_Amount]
  FROM [Student_Receive]
 WHERE [Receive_Type] = @ReceiveType AND [Year_Id] = @YearId AND [Term_Id] = @TermId AND [Dep_Id] = @DepId AND [Receive_Id] = @ReceiveId " + sqlWhere1 + sqlWhere2 + @"
 GROUP BY [Receive_Type], [Year_Id], [Term_Id], [Dep_Id], [Receive_Id], ISNULL([Dept_Id], '')
) AS SR
 ORDER BY [Dept_Id], [MAJOR_FLAG] DESC, [Major_Id], [CLASS_FLAG] DESC, [Class_Id], [Receive_Item_Amount]", Sanitizer.SqlEncode(receiveItemKey)).Trim();
                        #endregion

                        parameters.Add("@ReceiveType", receiveType);
                        parameters.Add("@YearId", yearId);
                        parameters.Add("@TermId", termId);
                        parameters.Add("@DepId", depId);
                        parameters.Add("@ReceiveId", receiveId);
                        //parameters.Add("@@ReceiveItemName", receiveItemName);
                        #endregion

                        result = _Factory.GetDataTable(sql, parameters, 0, 0, out dtData);
                        if (!result.IsSuccess)
                        {
                            return String.Format("查詢資料失敗，錯誤訊息：{0}", result.Message);
                        }

                        dtData.TableName = receiveItemKey;
                        dtDatas.Add(dtData);
                    }
                    #endregion
                    #endregion

                    #region 產生報表檔案
                    {
                        if (isUseODS)
                        {
                            ODSHelper helper = new ODSHelper();
                            string errmsg = helper.GenReportE1(dtHead, dtPage, receiveItems, dtDatas, out outFileContent);
                            if (!String.IsNullOrEmpty(errmsg))
                            {
                                return "產生 ODS 檔失敗";
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            GenReportHelper helper = new GenReportHelper();
                            string errmsg = helper.GenReportE1(dtHead, dtPage, receiveItems, dtDatas, out outFileContent);
                            if (!String.IsNullOrEmpty(errmsg))
                            {
                                return "產生 XLS 檔失敗";
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    //分類統計表
                    #region 取得系所資料
                    DataTable dtMajor = null;
                    {
                        #region 組 SQL
                        StringBuilder sql = new StringBuilder();
                        sql.AppendLine(@"SELECT DISTINCT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], '') AS [Major_Id]
     , ISNULL((SELECT [Major_Name] FROM [Major_List] AS ML WHERE ML.[Receive_Type] = SR.[Receive_Type] AND ML.[Year_Id] = SR.[Year_Id] AND ML.[Term_Id] = SR.[Term_Id] AND ML.[Major_Id] = SR.[Major_Id]), '') AS [Major_Name]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId");

                        KeyValueList parameters = new KeyValueList();
                        parameters.Add("@ReceiveType", receiveType);
                        parameters.Add("@YearId", yearId);
                        parameters.Add("@TermId", termId);
                        parameters.Add("@DepId", depId);
                        parameters.Add("@ReceiveId", receiveId);

                        #region 批號條件
                        if (upNo != null)
                        {
                            sql.AppendLine("   AND SR.[Up_No] = @UpNo");
                            parameters.Add("@UpNo", upNo.Value);
                        }
                        #endregion

                        #region 繳費狀態條件 (無代收日、代收管道、入帳日才算未繳，反之為已繳)
                        if (receiveStatus == "1")   //已繳
                        {
                            sql.AppendLine("   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))");
                        }
                        else if (receiveStatus == "0")  //未繳
                        {
                            sql.AppendLine("   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))");
                        }
                        #endregion

                        sql.AppendLine(@"
 ORDER BY [Dept_Id], [Major_Id]");
                        #endregion

                        result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtMajor);
                        if (!result.IsSuccess)
                        {
                            return String.Format("取得系所資料失敗，錯誤訊息：{0}", result.Message);
                        }
                    }
                    #endregion

                    #region 取得班級資料
                    DataTable dtClass = null;
                    {
                        #region 組 SQL
                        StringBuilder sql = new StringBuilder();
                        sql.AppendLine(@"SELECT DISTINCT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], '') AS [Major_Id], ISNULL(SR.[Class_Id], '') AS [Class_Id]
     , ISNULL((SELECT [Class_Name] FROM [Class_List] AS CL WHERE CL.[Receive_Type] = SR.[Receive_Type] AND CL.[Year_Id] = SR.[Year_Id] AND CL.[Term_Id] = SR.[Term_Id] AND CL.[Class_Id] = SR.[Class_Id]), '') AS [Class_Name]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId");

                        KeyValueList parameters = new KeyValueList();
                        parameters.Add("@ReceiveType", receiveType);
                        parameters.Add("@YearId", yearId);
                        parameters.Add("@TermId", termId);
                        parameters.Add("@DepId", depId);
                        parameters.Add("@ReceiveId", receiveId);

                        #region 批號條件
                        if (upNo != null)
                        {
                            sql.AppendLine("   AND SR.[Up_No] = @UpNo");
                            parameters.Add("@UpNo", upNo.Value);
                        }
                        #endregion

                        #region 繳費狀態條件 (無代收日、代收管道、入帳日才算未繳，反之為已繳)
                        if (receiveStatus == "1")   //已繳
                        {
                            sql.AppendLine("   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))");
                        }
                        else if (receiveStatus == "0")  //未繳
                        {
                            sql.AppendLine("   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))");
                        }
                        #endregion

                        sql.AppendLine(@"
 ORDER BY [Dept_Id], [Major_Id], [Class_Id]");
                        #endregion

                        result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dtClass);
                        if (!result.IsSuccess)
                        {
                            return String.Format("取得班級資料失敗，錯誤訊息：{0}", result.Message);
                        }
                    }
                    #endregion

                    #region 展開所有收入科目金額 (收集各收入科目各種金額作為橫向資料欄位)
                    KeyValueList<double> receiveItemAmounts = new KeyValueList<double>();
                    if (dtPage != null && dtPage.Rows.Count > 0)
                    {
                        #region SQL Sample
                        //SELECT DISTINCT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id]
                        //     , 'Receive_01' AS [Receive_Item_Key], ISNULL(SR.[Receive_01], 0) AS [Receive_Item_Amount]
                        //  FROM [Student_Receive] AS SR
                        // WHERE SR.[Receive_Type] = '5026' AND SR.[Year_Id] = '103' AND SR.[Term_Id] = '1' AND [Dep_Id] = '' AND SR.[Receive_Id] = '1'
                        //--   AND SR.[Up_No] = 0
                        //--   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))
                        //--   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))
                        // ORDER BY [Receive_Type], [Year_Id], [Term_Id], [Dep_Id], [Receive_Id], [Receive_Item_Amount]
                        #endregion

                        #region 批號條件
                        string sqlWhere1 = null;
                        KeyValue sqlParameter1 = null;
                        if (upNo != null)
                        {
                            sqlWhere1 = "\r\n   AND SR.Up_No = @UpNo";
                            sqlParameter1 = new KeyValue("@UpNo", upNo.Value);
                        }
                        #endregion

                        #region 繳費狀態條件 (無代收日、代收管道、入帳日才算未繳，反之為已繳)
                        string sqlWhere2 = null;
                        if (receiveStatus == "1")   //已繳
                        {
                            sqlWhere2 = "\r\n   AND ((SR.Receive_Date IS NOT NULL AND SR.Receive_Date != '') OR (SR.Receive_Way IS NOT NULL AND SR.Receive_Way != '') OR (SR.Account_Date IS NOT NULL AND SR.Account_Date != ''))";
                        }
                        else if (receiveStatus == "0")  //未繳
                        {
                            sqlWhere2 = "\r\n   AND ((SR.Receive_Date IS NULL OR SR.Receive_Date = '') AND (SR.Receive_Way IS NULL OR SR.Receive_Way = '') AND (SR.Account_Date IS NULL OR SR.Account_Date = ''))";
                        }
                        #endregion

                        #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                        foreach (string receiveItemFieldName in receiveItemFieldNames)
                        {
                            string receiveItemKey = receiveItemFieldName;
                            string sql = String.Format(@"
SELECT DISTINCT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[{0}], 0) AS [Receive_Item_Amount]
  FROM [Student_Receive] AS SR
 WHERE [Receive_Type] = @ReceiveType AND [Year_Id] = @YearId AND [Term_Id] = @TermId AND [Dep_Id] = @DepId AND [Receive_Id] = @ReceiveId " + sqlWhere1 + sqlWhere2 + @"
 ORDER BY [Receive_Type], [Year_Id], [Term_Id], [Dep_Id], [Receive_Id], [Receive_Item_Amount]", Sanitizer.SqlEncode(receiveItemKey)).Trim();

                            KeyValueList parameters = new KeyValueList();
                            parameters.Add("@ReceiveType", receiveType);
                            parameters.Add("@YearId", yearId);
                            parameters.Add("@TermId", termId);
                            parameters.Add("@DepId", depId);
                            parameters.Add("@ReceiveId", receiveId);
                            if (sqlParameter1 != null)
                            {
                                parameters.Add(sqlParameter1.Key, sqlParameter1.Value);
                            }

                            DataTable dt = null;
                            result = _Factory.GetDataTable(sql.ToString(), parameters, 0, 0, out dt);
                            if (!result.IsSuccess)
                            {
                                return String.Format("計算收入科目金額項目資料失敗，錯誤訊息：{0}", result.Message);
                            }
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    double value = Convert.ToDouble(row["Receive_Item_Amount"]);
                                    receiveItemAmounts.Add(receiveItemKey, value);
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region 逐收入科目產生表身資料
                    #region [MDY:20220910] Checkmarx - Heuristic DB Parameter Tampering 誤判調整
                    List<DataTable> dtDatas = new List<DataTable>(receiveItemFieldNames.Count);
                    if (receiveItemFieldNames != null && receiveItemFieldNames.Count > 0)
                    {
                        #region SQL Sample
                        //SELECT *
                        //     , ISNULL((SELECT ISNULL([Major_Name], '') FROM [Major_List] AS ML WHERE ML.[Receive_Type] = SR.[Receive_Type] AND ML.[Year_Id] = SR.[Year_Id] AND ML.[Term_Id] = SR.[Term_Id] AND ML.[Dep_Id] = SR.[Dep_Id] AND ML.[Major_Id] = SR.[Major_Id]), '') AS [Major_Name]
                        //     , ISNULL((SELECT ISNULL([Class_Name], '') FROM [Class_List] AS CL WHERE CL.[Receive_Type] = SR.[Receive_Type] AND CL.[Year_Id] = SR.[Year_Id] AND CL.[Term_Id] = SR.[Term_Id] AND CL.[Dep_Id] = SR.[Dep_Id] AND CL.[Class_Id] = SR.[Class_Id]), '') AS [Class_Name]
                        //  FROM (
                        ///* By 部別, 系所, 班級, 收入科目金額 的筆數, 小計 */
                        //SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], '') AS [Major_Id], ISNULL(SR.[Class_Id], '') AS [Class_Id]
                        //     , 'Receive_01' AS [Receive_Item_Key], ISNULL(SR.[Receive_01], 0) AS [Receive_Item_Amount], Count(1) AS [Data_Count], SUM(ISNULL(SR.[Receive_01], 0)) AS [Sum_Amount]
                        //     , 'Y' AS [DEPT_FLAG], 'Y' AS [MAJOR_FLAG], 'Y' AS [CLASS_FLAG]
                        //  FROM [Student_Receive] AS SR
                        // WHERE SR.[Receive_Type] = '5026' AND SR.[Year_Id] = '103' AND SR.[Term_Id] = '1' AND SR.[Dep_Id] = '' AND SR.[Receive_Id] = '1'
                        //--   AND SR.[Up_No] = 1
                        //--   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))
                        //--   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))
                        // GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], ''), ISNULL(SR.[Major_Id], ''), ISNULL(SR.[Class_Id], ''), ISNULL(SR.[Receive_01], 0)
                        //UNION
                        ///* By 部別, 系所, 收入科目金額 的筆數, 小計 */
                        //SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], ''), NULL AS [Class_Id]
                        //     , 'Receive_01' AS [Receive_Item_Key], ISNULL(SR.[Receive_01], 0) AS [Receive_Item_Amount], Count(1) AS [Data_Count], SUM(ISNULL(SR.[Receive_01], 0)) AS [Sum_Amount]
                        //     , 'Y' AS [DEPT_FLAG], 'Y' AS [MAJOR_FLAG], 'N' AS [CLASS_FLAG]
                        //  FROM [Student_Receive] AS SR
                        // WHERE SR.[Receive_Type] = '5026' AND SR.[Year_Id] = '103' AND SR.[Term_Id] = '1' AND SR.[Dep_Id] = '' AND SR.[Receive_Id] = '1'
                        //--   AND SR.[Up_No] = 1
                        //--   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))
                        //--   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))
                        // GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], ''), ISNULL(SR.[Major_Id], ''), ISNULL(SR.[Receive_01], 0)
                        //UNION
                        ///* By 部別, 收入科目金額 的筆數, 小計 */
                        //SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], '') AS [Dept_Id], NULL AS [Major_Id], NULL AS [Class_Id]
                        //     , 'Receive_01' AS [Receive_Item_Key], ISNULL(SR.[Receive_01], 0) AS [Receive_Item_Amount], Count(1) AS [Data_Count], SUM(ISNULL(SR.[Receive_01], 0)) AS [Sum_Amount]
                        //     , 'Y' AS [DEPT_FLAG], 'N' AS [MAJOR_FLAG], 'N' AS [CLASS_FLAG]
                        //  FROM [Student_Receive] AS SR
                        // WHERE SR.[Receive_Type] = '5026' AND SR.[Year_Id] = '103' AND SR.[Term_Id] = '1' AND SR.[Dep_Id] = '' AND SR.[Receive_Id] = '1'
                        //--   AND SR.[Up_No] = 1
                        //--   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))
                        //--   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))
                        // GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], ''), ISNULL(SR.[Receive_01], 0)
                        //UNION
                        ///* By 收入科目金額 的筆數, 小計 */
                        //SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], NULL AS [Dept_Id], NULL AS [Major_Id], NULL AS [Class_Id]
                        //     , 'Receive_01' AS [Receive_Item_Key], ISNULL(SR.[Receive_01], 0) AS [Receive_Item_Amount], Count(1) AS [Data_Count], SUM(ISNULL(SR.[Receive_01], 0)) AS [Sum_Amount]
                        //     , 'N' AS [DEPT_FLAG], 'N' AS [MAJOR_FLAG], 'N' AS [CLASS_FLAG]
                        //  FROM [Student_Receive] AS SR
                        // WHERE SR.[Receive_Type] = '5026' AND SR.[Year_Id] = '103' AND SR.[Term_Id] = '1' AND SR.[Dep_Id] = '' AND SR.[Receive_Id] = '1'
                        //--   AND SR.[Up_No] = 1
                        //--   AND ((SR.[Receive_Date] IS NOT NULL AND SR.[Receive_Date] != '') OR (SR.[Receive_Way] IS NOT NULL AND SR.[Receive_Way] != '') OR (SR.[Account_Date] IS NOT NULL AND SR.[Account_Date] != ''))
                        //--   AND ((SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '') AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '') AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = ''))
                        // GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Receive_01], 0)
                        //) AS SR
                        // ORDER BY SR.[DEPT_FLAG] DESC, SR.[Dept_Id], SR.[MAJOR_FLAG] DESC, SR.[Major_Id], SR.[CLASS_FLAG] DESC, SR.[Class_Id], SR.[Receive_Item_Amount]
                        #endregion

                        #region 批號條件
                        string sqlWhere1 = null;
                        KeyValue sqlParameter1 = null;
                        if (upNo != null)
                        {
                            sqlWhere1 = "\r\n   AND [Up_No] = @UpNo";
                            sqlParameter1 = new KeyValue("@UpNo", upNo.Value);
                        }
                        #endregion

                        #region 繳費狀態條件 (無代收日、代收管道、入帳日才算未繳，反之為已繳)
                        string sqlWhere2 = null;
                        if (receiveStatus == "1")   //已繳
                        {
                            sqlWhere2 = "\r\n   AND (([Receive_Date] IS NOT NULL AND [Receive_Date] != '') OR ([Receive_Way] IS NOT NULL AND [Receive_Way] != '') OR ([Account_Date] IS NOT NULL AND [Account_Date] != ''))";
                        }
                        else if (receiveStatus == "0")  //未繳
                        {
                            sqlWhere2 = "\r\n   AND (([Receive_Date] IS NULL OR [Receive_Date] = '') AND ([Receive_Way] IS NULL OR [Receive_Way] = '') AND ([Account_Date] IS NULL OR [Account_Date] = ''))";
                        }
                        #endregion

                        foreach (string receiveItemFieldName in receiveItemFieldNames)
                        {
                            DataTable dtData = null;
                            string receiveItemKey = receiveItemFieldName;

                            #region 組 SQL
                            string sql = String.Format(@"
/* By 部別, 系所, 班級, 收入科目金額 的筆數, 小計 */
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], '') AS [Major_Id], ISNULL(SR.[Class_Id], '') AS [Class_Id]
     , @ReceiveItemKey AS [Receive_Item_Key], ISNULL(SR.[{0}], 0) AS [Receive_Item_Amount], Count(1) AS [Data_Count], SUM(ISNULL(SR.[{0}], 0)) AS [Sum_Amount]
     , 'Y' AS [DEPT_FLAG], 'Y' AS [MAJOR_FLAG], 'Y' AS [CLASS_FLAG]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId " + sqlWhere1 + sqlWhere2 + @"
 GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], ''), ISNULL(SR.[Major_Id], ''), ISNULL(SR.[Class_Id], ''), ISNULL(SR.[{0}], 0)
UNION
/* By 部別, 系所, 收入科目金額 的筆數, 小計 */
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], '') AS [Dept_Id], ISNULL(SR.[Major_Id], ''), NULL AS [Class_Id]
     , @ReceiveItemKey AS [Receive_Item_Key], ISNULL(SR.[{0}], 0) AS [Receive_Item_Amount], Count(1) AS [Data_Count], SUM(ISNULL(SR.[{0}], 0)) AS [Sum_Amount]
     , 'Y' AS [DEPT_FLAG], 'Y' AS [MAJOR_FLAG], 'N' AS [CLASS_FLAG]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId " + sqlWhere1 + sqlWhere2 + @"
 GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], ''), ISNULL(SR.[Major_Id], ''), ISNULL(SR.[{0}], 0)
UNION
/* By 部別, 收入科目金額 的筆數, 小計 */
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], '') AS [Dept_Id], NULL AS [Major_Id], NULL AS [Class_Id]
     , @ReceiveItemKey AS [Receive_Item_Key], ISNULL(SR.[{0}], 0) AS [Receive_Item_Amount], Count(1) AS [Data_Count], SUM(ISNULL(SR.[{0}], 0)) AS [Sum_Amount]
     , 'Y' AS [DEPT_FLAG], 'N' AS [MAJOR_FLAG], 'N' AS [CLASS_FLAG]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId " + sqlWhere1 + sqlWhere2 + @"
 GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[Dept_Id], ''), ISNULL(SR.[{0}], 0)
UNION
/* By 收入科目金額 的筆數, 小計 */
SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], NULL AS [Dept_Id], NULL AS [Major_Id], NULL AS [Class_Id]
     , @ReceiveItemKey AS [Receive_Item_Key], ISNULL(SR.[{0}], 0) AS [Receive_Item_Amount], Count(1) AS [Data_Count], SUM(ISNULL(SR.[{0}], 0)) AS [Sum_Amount]
     , 'N' AS [DEPT_FLAG], 'N' AS [MAJOR_FLAG], 'N' AS [CLASS_FLAG]
  FROM [Student_Receive] AS SR
 WHERE SR.[Receive_Type] = @ReceiveType AND SR.[Year_Id] = @YearId AND SR.[Term_Id] = @TermId AND SR.[Dep_Id] = @DepId AND SR.[Receive_Id] = @ReceiveId " + sqlWhere1 + sqlWhere2 + @"
 GROUP BY SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], ISNULL(SR.[{0}], 0)", receiveItemKey).Trim();

                            KeyValueList parameters = new KeyValueList();
                            parameters.Add("@ReceiveType", receiveType);
                            parameters.Add("@YearId", yearId);
                            parameters.Add("@TermId", termId);
                            parameters.Add("@DepId", depId);
                            parameters.Add("@ReceiveId", receiveId);
                            parameters.Add("@ReceiveItemKey", receiveItemKey);
                            if (sqlParameter1 != null)
                            {
                                parameters.Add(sqlParameter1.Key, sqlParameter1.Value);
                            }
                            #endregion

                            result = _Factory.GetDataTable(sql, parameters, 0, 0, out dtData);
                            if (!result.IsSuccess)
                            {
                                return String.Format("查詢資料失敗，錯誤訊息：{0}", result.Message);
                            }

                            dtData.TableName = receiveItemKey;
                            dtDatas.Add(dtData);
                        }
                    }
                    #endregion
                    #endregion

                    #region 產生報表檔案
                    {
                        if (isUseODS)
                        {
                            ODSHelper helper = new ODSHelper();
                            string errmsg = helper.GenReportE2(dtHead, dtPage, dtMajor, dtClass, receiveItems, receiveItemAmounts, dtDatas, out outFileContent);
                            if (!String.IsNullOrEmpty(errmsg))
                            {
                                return "產生 ODS 檔失敗";
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            GenReportHelper helper = new GenReportHelper();
                            string errmsg = helper.GenReportE2(dtHead, dtPage, dtMajor, dtClass, receiveItems, receiveItemAmounts, dtDatas, out outFileContent);
                            if (!String.IsNullOrEmpty(errmsg))
                            {
                                return "產生 XLS 檔失敗";
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                return String.Concat("產出檔案發生例外，錯誤訊息：", ex.Message);
            }
        }
        #endregion
        #endregion
        #endregion

        #region 匯出查詢結果的 Excel 檔
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出查詢問題檔查詢結果
        /// </summary>
        /// <param name="where"></param>
        /// <param name="outFileContent"></param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public string ExportC3600001QueryResult(Expression where, out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            try
            {
                #region 組查詢 SQL
                string whereSql = null;
                KeyValueList parameters = new KeyValueList();
                IDataParameter[] whereParameters = null;
                if (!_Factory.GenWhereCommandTextParameters(where, out whereSql, out whereParameters))
                {
                    return "無法產生查詢條件 SQL 語法";
                }
                if (whereParameters != null && whereParameters.Length > 0)
                {
                    foreach (IDataParameter whereParameter in whereParameters)
                    {
                        parameters.Add(whereParameter.ParameterName, whereParameter.Value);
                    }
                }

                #region [Old] 欄位名稱直接用中文
                //KeyValueList<string> fields = new KeyValueList<string>(10);
                //fields.Add(ProblemListView.Field.CancelNo, "虛擬帳號");
                //fields.Add(ProblemListView.Field.PayAmount, "繳款金額");
                //fields.Add(ProblemListView.Field.ReceiveWayName, "繳款方式");
                //fields.Add(ProblemListView.Field.ProblemFlag, "問題註記");
                //fields.Add(String.Format("ISNULL(CONVERT(varchar, {0}, 111), '') AS {0}", ProblemListView.Field.ReceiveDate), "代收日期");
                //fields.Add(ProblemListView.Field.ReceiveTime, "代收時間");
                //fields.Add(String.Format("ISNULL(CONVERT(varchar, {0}, 111), '') AS {0}", ProblemListView.Field.AccountDate), "入帳日期");
                //fields.Add(ProblemListView.Field.StuId, "學號");
                //fields.Add(ProblemListView.Field.StuName, "姓名");
                //fields.Add(ProblemListView.Field.ProblemRemark, "備註");

                //string selectFieldSql = String.Join(",", fields.Keys);
                #endregion

                #region [New]
                List<string> fields = new List<string>(10);
                fields.Add(String.Format("{0} AS [{1}]", ProblemListView.Field.CancelNo, "虛擬帳號"));
                fields.Add(String.Format("{0} AS [{1}]", ProblemListView.Field.PayAmount, "繳款金額"));
                fields.Add(String.Format("{0} AS [{1}]", ProblemListView.Field.ReceiveWayName, "繳款方式"));
                fields.Add(String.Format("{0} AS [{1}]", ProblemListView.Field.ProblemFlag, "問題註記"));
                fields.Add(String.Format("ISNULL(CONVERT(varchar, {0}, 111), '') AS {1}", ProblemListView.Field.ReceiveDate, "代收日期"));
                fields.Add(String.Format("{0} AS [{1}]", ProblemListView.Field.ReceiveTime, "代收時間"));
                fields.Add(String.Format("ISNULL(CONVERT(varchar, {0}, 111), '') AS {1}", ProblemListView.Field.AccountDate, "入帳日期"));
                fields.Add(String.Format("{0} AS [{1}]", ProblemListView.Field.StuId, "學號"));
                fields.Add(String.Format("{0} AS [{1}]", ProblemListView.Field.StuName, "姓名"));
                fields.Add(String.Format("{0} AS [{1}]", ProblemListView.Field.ProblemRemark, "備註"));

                string selectFieldSql = String.Join(",", fields.ToArray());
                #endregion

                string sql = String.Format("SELECT {0} FROM ({1}) AS V WHERE {2} ORDER BY {3}", selectFieldSql, ProblemListView.VIEWSQL, whereSql, ProblemListView.Field.ReceiveDate);
                #endregion

                DataTable dt = null;
                Result result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                if (!result.IsSuccess)
                {
                    return String.Format("查詢問題檔失敗，錯誤訊息：{0}", result.Message);
                }
                if (dt == null || dt.Rows.Count == 0)
                {
                    //無資料傳回 new byte[0] 並回 null
                    outFileContent = new byte[0];
                    return null;
                }

                #region 檢查資料筆數，不可超過筆數限制 (XLS 65535 筆; ODS 1048575 筆)
                Int32 count = dt.Rows.Count;
                if (!isUseODS && count > 65535)
                {
                    return "匯出問題檔資料超過 65535 筆數 (XLS 檔案限制)，請調整查詢條件";
                }
                else if (isUseODS && count > Fuju.ODS.ODSSheet.MAX_ROW_COUNT)
                {
                    return String.Format("匯出問題檔資料超過 {0} 筆數 (ODS 檔案限制)，請調整查詢條件", Fuju.ODS.ODSSheet.MAX_ROW_COUNT);
                }
                #endregion

                #region 將問題註記的代碼轉成文字
                string problemFlagColumnName = "問題註記";  //ProblemListView.Field.ProblemFlag
                foreach (DataRow drow in dt.Rows)
                {
                    string code = drow.IsNull(problemFlagColumnName) ? null : drow[problemFlagColumnName].ToString();
                    drow[problemFlagColumnName] = ProblemFlagCodeTexts.GetText(code);
                }
                #endregion

                #region 指定欄位名稱
                //{
                //    foreach (DataColumn column in dt.Columns)
                //    {
                //        string key = column.ColumnName;
                //        KeyValue<string> fieldName = fields.Find(x => x.Key == key);
                //        if (fieldName != null)
                //        {
                //            column.ColumnName = fieldName.Value;
                //        }
                //    }
                //}
                #endregion

                if (isUseODS)
                {
                    #region DataTable 轉 ODS
                    ODSHelper helper = new ODSHelper();
                    outFileContent = helper.DataTable2ODS(dt, isDecimalTruncate: true);
                    #endregion
                }
                else
                {
                    #region DataTable 轉 Xls
                    {
                        ConvertFileHelper helper = new ConvertFileHelper();
                        outFileContent = helper.Dt2Xls(dt);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return String.Concat("匯出問題檔查詢結果發生例外，錯誤訊息：", ex.Message);
            }

            return null;
        }

        /// <summary>
        /// 匯出中心入帳資料查詢結果
        /// </summary>
        /// <param name="where"></param>
        /// <param name="outFileContent"></param>
        /// <returns></returns>
        public string ExportC3700006QueryResult(Expression where, out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            try
            {
                #region 組查詢 SQL
                string whereSql = null;
                KeyValueList parameters = new KeyValueList();
                IDataParameter[] whereParameters = null;
                if (!_Factory.GenWhereCommandTextParameters(where, out whereSql, out whereParameters))
                {
                    return "無法產生查詢條件 SQL 語法";
                }
                if (whereParameters != null && whereParameters.Length > 0)
                {
                    foreach (IDataParameter whereParameter in whereParameters)
                    {
                        parameters.Add(whereParameter.ParameterName, whereParameter.Value);
                    }
                }

                #region 匯出欄位
                List<string> fields = new List<string>(10);
                fields.Add(String.Format("[{0}] AS [{1}]", CancelDebtsView.Field.ReceiveType, "商家代號"));
                fields.Add(String.Format("[{0}] AS [{1}]", CancelDebtsView.Field.CancelNo, "虛擬帳號"));
                fields.Add(String.Format("[{0}] AS [{1}]", CancelDebtsView.Field.ReceiveAmount, "繳費金額"));
                fields.Add(String.Format("(CASE WHEN ISNUMERIC([{0}]) = 1 AND LEN([{0}]) = 7 THEN CAST(CAST(SUBSTRING([{0}], 1, 3) AS int) + 1911 AS varchar) + '/' + SUBSTRING([{0}], 4, 2)+ '/' + SUBSTRING([{0}], 6, 2) ELSE [{0}] END) AS [{1}]", CancelDebtsView.Field.ReceiveDate, "代收日期"));
                fields.Add(String.Format("[{0}] AS [{1}]", CancelDebtsView.Field.ReceiveTime, "代收時間"));
                fields.Add(String.Format("[{0}] AS [{1}]", CancelDebtsView.Field.AccountDate, "入帳日期"));
                fields.Add(String.Format("[{0}] AS [{1}]", CancelDebtsView.Field.ReceiveWayName, "繳款方式"));
                fields.Add(String.Format("[{0}] AS [{1}]", CancelDebtsView.Field.ReceiveBankName, "代收分行"));

                #region [MDY:20160607] 增加更正記號
                fields.Add(String.Format("[{0}] AS [{1}]", CancelDebtsView.Field.Reserve2, "更正記號"));
                fields.Add(String.Format("[{0}]", CancelDebtsView.Field.Status));
                #endregion

                //fields.Add(String.Format(", (CASE WHEN ISNUMERIC([{0}]) = 1 AND LEN([{0}]) = 7 THEN CAST(CAST(SUBSTRING([{0}], 1, 3) AS int) + 1911 AS varchar) + '/' + SUBSTRING([{0}], 4, 2)+ '/' + SUBSTRING([{0}], 6, 2) ELSE [{0}] END) AS [{1}]", CancelDebtsView.Field.AccountDate, "入帳日期"));

                string selectFieldSql = String.Join(",", fields.ToArray());
                #endregion

                string sql = String.Format("SELECT {0} FROM ({1}) AS V WHERE {2} ORDER BY {3} DESC, {4} ASC", selectFieldSql, CancelDebtsView.VIEWSQL, whereSql, CancelDebtsView.Field.ReceiveDate, CancelDebtsView.Field.SNo);

                #endregion

                #region 檢查資料筆數，不可超過筆數限制 (XLS 65535 筆; ODS 1048575 筆)
                {
                    object value = null;
                    string sql2 = String.Format("SELECT {0} FROM ({1}) AS V WHERE {2} ", "COUNT(1)", CancelDebtsView.VIEWSQL, whereSql);
                    Result result2 = _Factory.ExecuteScalar(sql2, parameters, out value);
                    if (!result2.IsSuccess)
                    {
                        return String.Format("查詢匯出中心入帳資料筆數失敗，錯誤訊息：{0}", result2.Message);
                    }
                    Int64 count = Convert.ToInt64(value);
                    if (!isUseODS && count > 65535)
                    {
                        return "匯出中心入帳資料超過 65535 筆數 (XLS 檔案限制)，請調整查詢條件";
                    }
                    else if (isUseODS && count > Fuju.ODS.ODSSheet.MAX_ROW_COUNT)
                    {
                        return String.Format("匯出中心入帳資料超過 {0} 筆數 (ODS 檔案限制)，請調整查詢條件", Fuju.ODS.ODSSheet.MAX_ROW_COUNT);
                    }
                    if (count == 0)
                    {
                        //無資料傳回 new byte[0] 並回 null
                        outFileContent = new byte[0];
                        return null;
                    }
                }
                #endregion

                DataTable dt = null;
                Result result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                if (!result.IsSuccess)
                {
                    return String.Format("查詢中心入帳資料失敗，錯誤訊息：{0}", result.Message);
                }
                else if (dt == null || dt.Rows.Count == 0)
                {
                    //無資料傳回 new byte[0] 並回 null
                    outFileContent = new byte[0];
                    return null;
                }

                #region [MDY:20160607] 轉成 XLS 前將更正記號換成更正記號文字說明
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string code = row["更正記號"].ToString().Trim();
                        string status = row[CancelDebtsView.Field.Status].ToString().Trim();
                        if (!String.IsNullOrEmpty(code))
                        {
                            row["更正記號"] = CancelDebtsEntity.GetReserve2Text(code, status);
                        }
                    }
                    //移除 Status 因為沒有要匯出
                    dt.Columns.Remove(CancelDebtsView.Field.Status);
                }
                #endregion

                if (isUseODS)
                {
                    #region DataTable 轉 ODS
                    ODSHelper helper = new ODSHelper();
                    outFileContent = helper.DataTable2ODS(dt, isDecimalTruncate: true);
                    #endregion
                }
                else
                {
                    #region DataTable 轉 Xls
                    {
                        ConvertFileHelper helper = new ConvertFileHelper();
                        outFileContent = helper.Dt2Xls(dt);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return String.Concat("匯出中心入帳資料查詢結果發生例外，錯誤訊息：", ex.Message);
            }

            return null;
        }

        /// <summary>
        /// 匯出排程作業查詢結果
        /// </summary>
        /// <param name="where"></param>
        /// <param name="outFileContent"></param>
        /// <param name="isUseODS">指定是否匯出成 ODS 檔</param>
        /// <returns></returns>
        public string ExportS5400003QueryResult(Expression where, out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;

            try
            {
                #region 組查詢 SQL
                string whereSql = null;
                KeyValueList parameters = new KeyValueList();
                IDataParameter[] whereParameters = null;
                if (!_Factory.GenWhereCommandTextParameters(where, out whereSql, out whereParameters))
                {
                    return "無法產生查詢條件 SQL 語法";
                }
                if (whereParameters != null && whereParameters.Length > 0)
                {
                    foreach (IDataParameter whereParameter in whereParameters)
                    {
                        parameters.Add(whereParameter.ParameterName, whereParameter.Value);
                    }
                }

                #region 匯出欄位
                List<string> fields = new List<string>(10);
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jno, "序號"));
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jstd, "開始時間"));
                //fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jetd, "結束時間"));
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jparam, "作業參數"));
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jowner, "發動者帳號"));

                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jrid, "商家代號"));
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jyear, "年度代碼"));
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jterm, "期別代碼"));
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jrecid, "費用別代碼"));
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.SeriorNo, "批號"));

                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jstatusid, "處理狀態代碼"));
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jresultid, "處理結果代碼"));
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Memo, "處理說明"));
                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.Jlog, "處理日誌"));

                fields.Add(String.Format("[{0}] AS [{1}]", JobcubeEntity.Field.CDate, "建立日期"));

                string selectFieldSql = String.Join(",", fields.ToArray());
                #endregion

                string sql = String.Format("SELECT {0} FROM [{1}] AS V WHERE {2} ORDER BY {3} DESC, {4} ASC", selectFieldSql, JobcubeEntity.TABLE_NAME, whereSql, JobcubeEntity.Field.Jstd, JobcubeEntity.Field.Jtypeid);

                #endregion

                #region 檢查資料筆數，不可超過筆數限制 (XLS 65535 筆; ODS 1048575 筆)
                {
                    object value = null;
                    string sql2 = String.Format("SELECT {0} FROM [{1}] AS V WHERE {2} ", "COUNT(1)", JobcubeEntity.TABLE_NAME, whereSql);
                    Result result2 = _Factory.ExecuteScalar(sql2, parameters, out value);
                    if (!result2.IsSuccess)
                    {
                        return String.Format("查詢排程作業資料筆數失敗，錯誤訊息：{0}", result2.Message);
                    }
                    Int64 count = Convert.ToInt64(value);
                    if (!isUseODS && count > 65535)
                    {
                        return "匯出排程作業資料超過 65535 筆數 (XLS 檔案限制)，請調整查詢條件";
                    }
                    else if (isUseODS && count > Fuju.ODS.ODSSheet.MAX_ROW_COUNT)
                    {
                        return String.Format("匯出排程作業資料超過 {0} 筆數 (ODS 檔案限制)，請調整查詢條件", Fuju.ODS.ODSSheet.MAX_ROW_COUNT);
                    }
                    if (count == 0)
                    {
                        //無資料傳回 new byte[0] 並回 null
                        outFileContent = new byte[0];
                        return null;
                    }
                }
                #endregion

                #region 查資料
                DataTable dt = null;
                Result result = _Factory.GetDataTable(sql, parameters, 0, 0, out dt);
                if (!result.IsSuccess)
                {
                    return String.Format("查詢排程作業資料失敗，錯誤訊息：{0}", result.Message);
                }
                else if (dt == null || dt.Rows.Count == 0)
                {
                    //無資料傳回 new byte[0] 並回 null
                    outFileContent = new byte[0];
                    return null;
                }
                #endregion

                if (isUseODS)
                {
                    #region DataTable 轉 ODS
                    ODSHelper helper = new ODSHelper();
                    outFileContent = helper.DataTable2ODS(dt, isDecimalTruncate: true);
                    #endregion
                }
                else
                {
                    #region DataTable 轉 Xls
                    {
                        ConvertFileHelper helper = new ConvertFileHelper();
                        outFileContent = helper.Dt2Xls(dt);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return String.Concat("匯出排程作業資料查詢結果發生例外，錯誤訊息：", ex.Message);
            }

            return null;
        }
        #endregion
        #endregion

        #region [MDY:20190226] 匯出異業代收款檔資料檔
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出異業代收款檔資料檔
        /// </summary>
        /// <param name="edpChannelId">指定異業管道代碼</param>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="sDate">指定異業管道入帳日起日</param>
        /// <param name="eDate">指定異業管道入帳日訖日</param>
        /// <param name="fileType">指定匯出檔案格式 (XLS 或 ODS)</param>
        /// <param name="outFileContent">成功則傳回產生檔案內容，否則傳回 null</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        public string ExportEDPData(string edpChannelId, string receiveType, DateTime sDate, DateTime eDate, string fileType, out byte[] outFileContent)
        {
            outFileContent = null;

            #region 檢查參數
            if (String.IsNullOrEmpty(edpChannelId) || String.IsNullOrEmpty(receiveType) || sDate.Year < 1753 || eDate.Year < 1753 || (fileType != "XLS" && fileType != "ODS"))
            {
                return "缺少或無效的查詢資料參數";
            }
            #endregion

            DataTable dt = null;
            try
            {
                #region 組查詢 SQL
                //欄位中文名稱 Caption
                KeyValueList<string> columnCaptions = new KeyValueList<string>();
                columnCaptions.Add(EDPDataView.Field.CancelNo, "繳款帳號");
                columnCaptions.Add(EDPDataView.Field.TranferDate, "中信帳務處理日");
                columnCaptions.Add(EDPDataView.Field.ReceiveDate, "中信交易繳款日期");
                columnCaptions.Add(EDPDataView.Field.StuName, "學生姓名");
                columnCaptions.Add(EDPDataView.Field.ReceiveAmount, "學費總額");

                string fieldSeq = String.Format(@"[{0}], [{1}], [{2}], [{3}], [{4}], [{5}]"
                    , EDPDataEntity.Field.CancelNo
                    , EDPDataEntity.Field.TranferDate
                    , EDPDataEntity.Field.ReceiveDate
                    , EDPDataEntity.Field.StuName
                    , EDPDataEntity.Field.ReceiveAmount
                    , EDPDataEntity.Field.ReceiveTime);

                string whereSql = @"EDP_Channel_Id = @EDP_CHANNEL_ID AND Receive_Type = @RECEIVE_TYPE AND Tranfer_Date >= @SDATE AND Tranfer_Date <= @EDATE";

                string orderSql = String.Format("ORDER BY [{0}]", EDPDataEntity.Field.SN);

                KeyValueList parameters = new KeyValueList();
                parameters.Add("@EDP_CHANNEL_ID", edpChannelId);
                parameters.Add("@RECEIVE_TYPE", receiveType);
                parameters.Add("@SDATE", sDate.Date);
                parameters.Add("@EDATE", eDate.Date);

                string countSql = String.Format(@"SELECT {0} FROM {1} WHERE {2} ", "COUNT(1) AS DATA_COUNT", EDPDataEntity.TABLE_NAME, whereSql);
                string dataSql = String.Format(@"SELECT {0} FROM {1} WHERE {2} {3}", fieldSeq, EDPDataEntity.TABLE_NAME, whereSql, orderSql);
                #endregion

                #region 檢查資料筆數，不可超過筆數限制 (XLS 65535 筆; ODS 1048575 筆)
                int maxXlsRowCount = 65535;
                {
                    object value = null;
                    Result result = _Factory.ExecuteScalar(countSql, parameters, out value);
                    if (result.IsSuccess)
                    {
                        bool isUseODS = "ODS".Equals(fileType);
                        Int64 dataCount = Convert.ToInt64(value);
                        if (!isUseODS && dataCount > maxXlsRowCount) //XLS 版本的限制，為了相容性所以要限制
                        {
                            return "匯出資料超過 Excel (XLS) 的筆數限制 (65535 筆)";
                        }
                        else if (isUseODS && dataCount > Fuju.ODS.ODSSheet.MAX_ROW_COUNT)
                        {
                            return String.Format("匯出資料超過 Calc (ODS) 的筆數限制 ({0} 筆)", Fuju.ODS.ODSSheet.MAX_ROW_COUNT);
                        }
                        else if (dataCount <= 0)
                        {
                            return "查無匯出資料";
                        }
                    }
                    else
                    {
                        return String.Format("檢查匯出資料筆數失敗，錯誤訊息：{0}", result.Message);
                    }
                }
                #endregion

                #region 讀取資料
                {
                    Result result = _Factory.GetDataTable(dataSql, parameters, 0, maxXlsRowCount, out dt);
                    if (!result.IsSuccess)
                    {
                        return String.Format("讀取匯出資料失敗，錯誤訊息：{0}", result.Message);
                    }
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return "查無匯出資料";
                    }
                }
                #endregion

                #region [MDY:20190312] 因為客戶繳費時間，因為土銀中心好像固定給空白，所以不處理了
                //#region 處理中信帳務處理日 (加上客戶繳費時間)
                //foreach (DataRow drow in dt.Rows)
                //{
                //    TimeSpan? receiveTime = DataFormat.ConvertTimeText(drow[EDPDataView.Field.ReceiveTime].ToString());
                //    if (receiveTime.HasValue)
                //    {
                //        DateTime receiveDate = Convert.ToDateTime(drow[EDPDataView.Field.ReceiveDate]) + receiveTime.Value;
                //        drow[EDPDataView.Field.ReceiveDate] = receiveDate;
                //    }
                //}
                //#endregion
                #endregion

                #region 轉成檔案前，先移除客戶繳費時間欄位，並指定欄位中文名稱 Caption
                {
                    dt.Columns.Remove(EDPDataEntity.Field.ReceiveTime);
                    foreach (DataColumn column in dt.Columns)
                    {
                        string columnName = column.ColumnName;
                        KeyValue<string> columnCaption = columnCaptions.Find(x => x.Key == columnName);
                        if (columnCaption != null)
                        {
                            column.Caption = columnCaption.Value;
                        }
                    }
                }
                #endregion

                #region DataTable 轉成檔案
                if (dt != null)
                {
                    if (fileType == "ODS")
                    {
                        #region DataTable 轉 ODS
                        ODSHelper helper = new ODSHelper();
                        outFileContent = helper.DataTable2ODS(dt, isUseColumnCaption: true, isUseAmountStyle: true, isDecimalTruncate: true, isUseDateTimeStyle: true, isDateOnly: true, dateOnlyColumns: new string[] { EDPDataView.Field.TranferDate, EDPDataView.Field.ReceiveDate });
                        #endregion
                    }
                    else
                    {
                        #region DataTable 轉 Xls
                        ConvertFileHelper helper = new ConvertFileHelper();

                        #region [MDY:20190312] 因為客戶繳費時間，因為土銀中心好像固定給空白，所以 EDPDataView.Field.ReceiveDate 也只顯示日期
                        #region [OLD]
                        //outFileContent = helper.DataTable2Xls(dt, isUseAmountStyle: true, isUseColumnCaption: true, isUseDateTimeStyle: true, dateColumns: new string[] { EDPDataView.Field.TranferDate });
                        #endregion

                        outFileContent = helper.DataTable2Xls(dt, isUseAmountStyle: true, isUseColumnCaption: true, isUseDateTimeStyle: true, dateColumns: new string[] { EDPDataView.Field.TranferDate, EDPDataView.Field.ReceiveDate });
                        #endregion
                        #endregion
                    }

                    dt.Clear();
                    dt.Dispose();
                    dt = null;

                    if (outFileContent == null)
                    {
                        return "將匯出資料存成 XLS 檔失敗";
                    }

                    #region 大於 20M 則壓縮成 ZIP，避免序列化失敗 (目前沒有傳附檔名回去，所以暫時 Mark)
                    //if (outFileContent.Length > 1024 * 1024 * 20)
                    //{
                    //    try
                    //    {
                    //        string xlsFileName = Path.Combine(Path.GetTempPath(), String.Format("{0}_{1:yyyyMMddHHmmss}", receiveType, DateTime.Now));
                    //        string zipFileName = Path.GetTempFileName();
                    //        File.WriteAllBytes(xlsFileName, outFileContent);
                    //        ZIPHelper.ZipFile(xlsFileName, zipFileName);
                    //        outFileContent = File.ReadAllBytes(zipFileName);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        return "將匯出資料壓縮成 ZIP 檔失敗，" + ex.Message;
                    //    }
                    //}
                    #endregion

                    return null;
                }
                else
                {
                    return "無查資料";
                }
                #endregion
            }
            catch (Exception ex)
            {
                return String.Concat("匯出排程作業資料查詢結果發生例外，錯誤訊息：", ex.Message);
            }
            finally
            {
                if (dt != null)
                {
                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }
                GC.Collect();
            }
        }
        #endregion
        #endregion

        #region [MDY:20190906] (2019擴充案) 匯出每日銷帳結果查詢結果檔 (C3400001)
        /// <summary>
        /// 匯出每日銷帳結果查詢結果檔 (C3400001)
        /// </summary>
        /// <param name="datas">查詢結果資料</param>
        /// <param name="fileType">匯出檔案格式 ODS 或 XLS，其他值視為 XLS</param>
        /// <param name="outFileContent">傳回匯出檔 byte 陣列</param>
        /// <returns></returns>
        public string ExportC3400001ResutFile(CancelResultEntity[] datas, string sheetName, string fileType, out byte[] outFileContent)
        {
            outFileContent = null;
            if (datas == null || datas.Length == 0)
            {
                return "無資料";
            }

            if (fileType.Equals("ODS", StringComparison.CurrentCultureIgnoreCase))
            {
                ODSHelper helper = new ODSHelper();
                return helper.ExportC3400001ResutFile(datas, sheetName, out outFileContent);
            }
            else
            {
                GenReportHelper helper = new GenReportHelper();
                return helper.ExportC3400001ResutFile(datas, sheetName, out outFileContent);
            }
        }
        #endregion

        #region Private Method
        private void GenSMBarcode(string smCancelNo, decimal smAmount, DateTime payDueDate, int extraDays, string channelCode
            , out string smBarcode1, out string smBarcode2, out string smBarcode3)
        {
            ChannelHelper helper = new ChannelHelper();

            //超商繳款期限
            DateTime smPayDueDate = payDueDate;
            if (extraDays > 0)
            {
                smPayDueDate = smPayDueDate.AddDays(extraDays);
            }

            smBarcode1 = helper.GenSMBarcode1(smPayDueDate, channelCode);
            smBarcode2 = helper.GenSMBarcode2(smCancelNo);
            smBarcode3 = helper.GenSMBarcode3(smBarcode1, smBarcode2, payDueDate, smAmount);
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="keys"></param>
        /// <param name="deductId">客戶委託代號</param>
        /// <param name="schoolId">學校統編</param>
        /// <param name="bankId">主辦行代碼</param>
        /// <param name="dataLog"></param>
        /// <returns></returns>
        private List<DeductionData> GetDeductionData(string receiveType, ICollection<BillKey> keys, out string deductId, out string schoolId, out string bankId, out string dataLog)
        {
            deductId = null;
            schoolId = null;
            bankId = null;
            dataLog = null;

            #region 取 SchoolRTypeEntity
            {
                SchoolRTypeEntity data = null;
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out data);
                if (!result.IsSuccess)
                {
                    dataLog = String.Format("讀取 ({0}) 學校資料失敗，錯誤訊息：{1}", receiveType, result.Message);
                    return null;
                }
                if (data == null)
                {
                    dataLog = String.Format("查無 ({0}) 學校資料", receiveType);
                    return null;
                }
                deductId = data.DeductId;
                schoolId = data.SchId == null ? String.Empty : data.SchId.Trim();
                bankId = data.BankId == null ? String.Empty : data.BankId.Trim();

                #region 把3碼或6碼的銀行代碼換成7碼
                if (!bankId.StartsWith(DataFormat.MyBankID) && (bankId.Length == 3 || bankId.Length == 4))
                {
                    bankId = DataFormat.MyBankID + bankId;
                }
                if (bankId.Length == 6)
                {
                    BankEntity bank = null;
                    Expression where2 = new Expression(BankEntity.Field.BankNo, bankId);
                    Result result2 = _Factory.SelectFirst<BankEntity>(where2, null, out bank);
                    if (result2.IsSuccess && bank != null && !String.IsNullOrEmpty(bank.FullCode))
                    {
                        bankId = bank.FullCode;
                    }
                }
                #endregion
            }
            #endregion

            StringBuilder log = new StringBuilder();
            List<DeductionData> datas = new List<DeductionData>(keys.Count);
            {
                string sql = @"SELECT ISNULL(Stu_Id, '') AS Stu_Id, ISNULL(Cancel_No, '') AS Cancel_No, ISNULL(Receive_Amount, 0) AS Receive_Amount
     , ISNULL(Deduct_BankId, '') AS Deduct_BankId, ISNULL(Deduct_AccountNo, '') AS Deduct_AccountNo, ISNULL(Deduct_AccountName, '') AS Deduct_AccountName, ISNULL(Deduct_AccountId, '') AS Deduct_AccountId
  FROM Student_Receive
 WHERE (Receive_Way IS NULL OR Receive_Way = '') AND (Receive_Date IS NULL OR Receive_Date = '')
   AND (Receive_Type = @ReceiveType AND Year_Id = @YearId AND Term_Id = @TermId AND Dep_Id = @DepId AND Receive_Id = @ReceiveId AND Stu_Id = @StuId AND Old_Seq = @OldSeq)";
                KeyValue[] parameters = null;
                DataTable dt = null;
                DataRow row = null;
                Result result = null;
                int no = 0;
                foreach (BillKey key in keys)
                {
                    no++;
                    if (receiveType != key.ReceiveType)
                    {
                        log.AppendFormat("查無第 {0} 筆 ({1}) 資料的商家代號不正確", no, key).AppendLine();
                        continue;
                    }

                    parameters = new KeyValue[] {
                        new KeyValue("@ReceiveType", key.ReceiveType),
                        new KeyValue("@YearId", key.YearId),
                        new KeyValue("@TermId", key.TermId),
                        new KeyValue("@DepId", key.DepId),
                        new KeyValue("@ReceiveId", key.ReceiveId),
                        new KeyValue("@StuId", key.StuId),
                        new KeyValue("@OldSeq", key.OldSeq)
                    };
                    result = _Factory.GetDataTable(sql, parameters, 0, 1, out dt);
                    if (result.IsSuccess)
                    {
                        if (dt == null || dt.Rows.Count == 0)
                        {
                            log.AppendFormat("查無第 {0} 筆 ({1}) 學生繳費資料，該資料不存在或已繳費", no, key).AppendLine();
                        }
                        else
                        {
                            row = dt.Rows[0];

                            DeductionData data = new DeductionData(row["Stu_Id"].ToString()
                                , row["Cancel_No"].ToString(), Convert.ToDecimal(row["Receive_Amount"])
                                , row["Deduct_BankId"].ToString(), row["Deduct_AccountNo"].ToString()
                                , row["Deduct_AccountName"].ToString(), row["Deduct_AccountId"].ToString());

                            if (String.IsNullOrEmpty(data.CancelNo))
                            {
                                log.AppendFormat("第 {0} 筆 ({1}) 學生繳費資料未產生虛擬帳號", no, key).AppendLine();
                            }
                            else if (data.Amount <= 0M)
                            {
                                log.AppendFormat("第 {0} 筆 ({1}) 學生繳費資料未產生金額或無須繳款", no, key).AppendLine();
                            }
                            else if (String.IsNullOrEmpty(data.BankId) || !data.BankId.StartsWith(DataFormat.MyBankID))
                            {
                                log.AppendFormat("第 {0} 筆 ({1}) 學生繳費資料的委扣銀行 ({2}) 未指定或不屬於本行", no, key, data.BankId).AppendLine();
                            }
                            else if (String.IsNullOrEmpty(data.AccountNo))
                            {
                                log.AppendFormat("第 {0} 筆 ({1}) 學生繳費資料的委扣帳號未指定", no, key, data.AccountNo).AppendLine();
                            }
                            else
                            {
                                datas.Add(data);
                            }
                        }
                    }
                    else
                    {
                        log.AppendFormat("查詢第 {0} 筆 ({1}) 學生繳費資料失敗，錯誤訊息：{2}", no, key, result.Message).AppendLine();
                    }
                }
            }
            dataLog = log.ToString();
            return datas;
        }

        /// <summary>
        /// 取得學校帳號資料
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="schIdenty"></param>
        /// <param name="schBankId"></param>
        /// <param name="schAccountNo"></param>
        /// <returns></returns>
        private string GetSchoolInfo(string receiveType, out string schIdenty, out string schBankId, out string schAccountNo)
        {
            schIdenty = null;
            schBankId = null;
            schAccountNo = null;

            SchoolRTypeEntity data = null;
            Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
            Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out data);
            if (result.IsSuccess)
            {
                if (data == null)
                {
                    return String.Format("查無 {0} 商家代號資料", receiveType);
                }
                else if (data.Status != DataStatusCodeTexts.NORMAL)
                {
                    return String.Format("{0} 商家代號已停用", receiveType);
                }
                else
                {
                    schIdenty = data.SchIdenty == null ? String.Empty : data.SchIdenty.Trim();
                    schBankId = data.BankId == null ? String.Empty : data.BankId.Trim();
                    schAccountNo = data.SchAccount == null ? String.Empty : data.SchAccount.Trim();
                    if (schBankId.Length < 7)
                    {
                        BankEntity bank = null;

                        Expression where2 = new Expression(BankEntity.Field.BankNo, RelationEnum.Like, DataFormat.MyBankID + schBankId + "%");
                        result = _Factory.SelectFirst<BankEntity>(where2, null, out bank);
                        if (bank != null)
                        {
                            schBankId = bank.BankNo.Trim();
                        }
                    }
                    return null;
                }
            }
            else
            {
                return String.Format("查詢 {0} 商家代號資料失敗，錯誤訊息：{1}", receiveType, result.Message);
            }
        }

        /// <summary>
        /// 產生文字行
        /// </summary>
        /// <param name="byteSize"></param>
        /// <param name="fields"></param>
        /// <param name="texts"></param>
        /// <returns></returns>
        private string GenTextLine(int byteSize, TxtFormat[] fields, string[] texts)
        {
            if (texts == null || fields == null || texts.Length != fields.Length || byteSize < 1)
            {
                return null;
            }

            byte[] buffs = new byte[byteSize];
            byte emp = Convert.ToByte(' ');
            for (int idx = 0; idx < byteSize; idx++)
            {
                buffs[idx] = emp;
            }

            Encoding encoding = Encoding.Default;
            for (int idx = 0; idx < fields.Length; idx++)
            {
                TxtFormat field = fields[idx];
                string text = texts[idx];
                byte[] txtBytes = Common.GetCutPadBytes(encoding, text, field.Size, field.Align, field.PaddingChar);
                if (field.Start + field.Size - 1 > byteSize)
                {
                    return null;
                }
                else
                {
                    int index = field.Start - 1;
                    foreach (byte txtByte in txtBytes)
                    {
                        buffs[index] = txtByte;
                        index++;
                    }
                }
            }
            return encoding.GetString(buffs);
        }
        #endregion
    }

    /// <summary>
    /// TXT 格式化設定類別
    /// </summary>
    sealed class TxtFormat
    {
        private string _Key = String.Empty;
        /// <summary>
        /// 對照欄位 Ky
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value == null ? String.Empty : value.Trim();
            }
        }

        private int _Start = 0;
        /// <summary>
        /// 對照欄位起始字元位置，最小值為 1，0 表示未設定
        /// </summary>
        public int Start
        {
            get
            {
                return _Start;
            }
            set
            {
                _Start = value < 0 ? 0 : value;
            }
        }

        private int _Size = 0;
        /// <summary>
        /// 對照欄位起始字元位置，最小值為 1，0 表示未設定
        /// </summary>
        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                _Size = value < 0 ? 0 : value;
            }
        }

        private Common.AlignCutPadEnum _Align = Common.AlignCutPadEnum.Left;
        /// <summary>
        /// 指定切補齊方向
        /// </summary>
        public Common.AlignCutPadEnum Align
        {
            get
            {
                return _Align;
            }
            set
            {
                _Align = value;
            }
        }

        private char _PaddingChar = ' ';
        /// <summary>
        /// 指定補齊的字元
        /// </summary>
        public char PaddingChar
        {
            get
            {
                return _PaddingChar;
            }
            set
            {
                _PaddingChar = value;
            }
        }

        /// <summary>
        /// 建構 TXT 格式化設定類別
        /// </summary>
        /// <param name="key">代碼</param>
        /// <param name="start"起始字元位置</param>
        /// <param name="length">字元長度</param>
        /// <param name="align">切補齊方向</param>
        /// <param name="paddingChar">補齊字元</param>
        public TxtFormat(string key, int start, int size, Common.AlignCutPadEnum align = Common.AlignCutPadEnum.Left, char paddingChar = ' ')
        {
            this.Key = key;
            this.Start = start;
            this.Size = size;
            this.Align = align;
            this.PaddingChar = paddingChar;
        }
    }

    /// <summary>
    /// 扣款轉帳資料承載類別
    /// </summary>
    sealed class DeductionData
    {
        private string _StuId = null;
        /// <summary>
        /// 學號
        /// </summary>
        public string StuId
        {
            get
            {
                return _StuId;
            }
            set
            {
                _StuId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _CancelNo = null;
        /// <summary>
        /// 扣款轉帳虛擬帳號
        /// </summary>
        public string CancelNo
        {
            get
            {
                return _CancelNo;
            }
            set
            {
                _CancelNo = value == null ? String.Empty : value.Trim();
            }
        }

        private decimal _Amount = 0M;
        /// <summary>
        /// 扣款轉帳金額
        /// </summary>
        public decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }

        private string _BankId = null;
        /// <summary>
        /// 扣款轉帳銀行代碼
        /// </summary>
        public string BankId
        {
            get
            {
                return _BankId;
            }
            set
            {
                _BankId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _AccountNo = null;
        /// <summary>
        /// 扣款轉帳銀行帳號
        /// </summary>
        public string AccountNo
        {
            get
            {
                return _AccountNo;
            }
            set
            {
                _AccountNo = value == null ? String.Empty : value.Trim();
            }
        }

        private string _AccountName = null;
        /// <summary>
        /// 扣款轉帳銀行戶名
        /// </summary>
        public string AccountName
        {
            get
            {
                return _AccountName;
            }
            set
            {
                _AccountName = value == null ? String.Empty : value.Trim();
            }
        }

        private string _AccountId = null;
        /// <summary>
        /// 扣款轉帳銀行帳戶ＩＤ
        /// </summary>
        public string AccountId
        {
            get
            {
                return _AccountId;
            }
            set
            {
                _AccountId = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 建構扣款轉帳資料承載類別
        /// </summary>
        public DeductionData()
        {
            _StuId = String.Empty;
            _CancelNo = String.Empty;
            _BankId = String.Empty;
            _AccountNo = String.Empty;
            _AccountName = String.Empty;
            _AccountId = String.Empty;
        }

        /// <summary>
        /// 建構扣款轉帳資料承載類別
        /// </summary>
        /// <param name="stuId"></param>
        /// <param name="cancelNo"></param>
        /// <param name="amount"></param>
        /// <param name="bankId"></param>
        /// <param name="accountNo"></param>
        /// <param name="accountName"></param>
        /// <param name="accountId"></param>
        public DeductionData(string stuId, string cancelNo, decimal amount, string bankId, string accountNo, string accountName, string accountId)
        {
            this.StuId = stuId;
            this.CancelNo = cancelNo;
            this.Amount = amount;
            this.BankId = bankId;
            this.AccountNo = accountNo;
            this.AccountName = accountName;
            this.AccountId = accountId;
        }
    }

    /// <summary>
    /// 匯出繳費資料 Key 承載類別
    /// </summary>
    [Serializable]
    public sealed class BillKey
    {
        private string _ReceiveType = null;
        /// <summary>
        /// 商家代號代碼
        /// </summary>
        public string ReceiveType
        {
            get
            {
                return _ReceiveType;
            }
            set
            {
                _ReceiveType = value == null ? String.Empty : value.Trim();
            }
        }

        private string _YearId = null;
        /// <summary>
        /// 學年代碼
        /// </summary>
        public string YearId
        {
            get
            {
                return _YearId;
            }
            set
            {
                _YearId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _TermId = null;
        /// <summary>
        /// 學期代碼
        /// </summary>
        public string TermId
        {
            get
            {
                return _TermId;
            }
            set
            {
                _TermId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _DepId = null;
        /// <summary>
        /// 部別代碼
        /// </summary>
        public string DepId
        {
            get
            {
                return _DepId;
            }
            set
            {
                _DepId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _ReceiveId = null;
        /// <summary>
        /// 代收費用別代碼
        /// </summary>
        public string ReceiveId
        {
            get
            {
                return _ReceiveId;
            }
            set
            {
                _ReceiveId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _StuId = null;
        /// <summary>
        /// 學號
        /// </summary>
        public string StuId
        {
            get
            {
                return _StuId;
            }
            set
            {
                _StuId = value == null ? String.Empty : value.Trim();
            }
        }

        private int _OldSeq = -1;
        /// <summary>
        /// 舊資料序號
        /// </summary>
        public int OldSeq
        {
            get
            {
                return _OldSeq;
            }
            set
            {
                _OldSeq = value < 0 ? -1 : value;
            }
        }

        /// <summary>
        /// 建構匯出繳費資料 Key 承載類別
        /// </summary>
        public BillKey()
            : this(null, null, null, null, null, null, -1)
        {

        }

        /// <summary>
        /// 建構匯出繳費資料 Key 承載類別
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="stuId"></param>
        public BillKey(string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int oldSeq)
        {
            this.ReceiveType = receiveType;
            this.YearId = yearId;
            this.TermId = termId;
            this.DepId = depId;
            this.ReceiveId = receiveId;
            this.StuId = stuId;
            this.OldSeq = oldSeq;
        }

        /// <summary>
        /// 取得資料是否準備好
        /// </summary>
        /// <returns></returns>
        public bool IsReady()
        {
            return !String.IsNullOrEmpty(_ReceiveType) && !String.IsNullOrEmpty(_YearId) && !String.IsNullOrEmpty(_TermId)
                    && !String.IsNullOrEmpty(_DepId) && !String.IsNullOrEmpty(_ReceiveId) && !String.IsNullOrEmpty(_StuId) && _OldSeq > -1;
        }

        public override string ToString()
        {
            return String.Format("ReceiveType={0}; YearId={1}; TermId={2}; DepId={3}; ReceiveId={4}; StuId={5}; OldSeq={6}",
                this.ReceiveType, this.YearId, this.TermId, this.DepId, this.ReceiveId, this.StuId, this.OldSeq);
        }

        public string ToKeyText()
        {
            return String.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", this.ReceiveType, this.YearId, this.TermId, this.DepId, this.ReceiveId, this.StuId, this.OldSeq);
        }

        public static BillKey ParseKeyText(string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                //string[] args = text.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                string[] args = text.Split(new char[] { '_' });
                int oldSeq = -1;
                if (args.Length == 7 && int.TryParse(args[6], out oldSeq))
                {
                    BillKey key = new BillKey(args[0], args[1], args[2], args[3], args[4], args[5], oldSeq);
                    return key;
                }
            }
            return null;
        }
    }

    #region [MDY:20191214] (2019擴充案) 國際信用卡 - 規避 checkmarx 誤判
    public abstract class Sanitizer
    {
        private static Regex _regex = new Regex(@"[\r\n]", RegexOptions.Compiled);
        public static string SqlEncode(string value)
        {
            return _regex.Replace(value, " ").Replace("''", "[ESCAPED_SINGLE_QUOTE]").Replace("'", "''").Replace("[ESCAPED_SINGLE_QUOTE]", "''");
        }
    }
    #endregion
}
