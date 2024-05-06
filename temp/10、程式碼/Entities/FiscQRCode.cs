using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entities
{
    /// <summary>
    /// 財金 QRCode 支付設定承載類別
    /// </summary>
    public class FiscQRCodeConfig
    {
        #region Static Member
        /// <summary>
        /// 財金 QRCode 支付設定的 設定代碼
        /// </summary>
        public static readonly string ConfigKey = "FiscQRCode";

        /// <summary>
        /// 設定字串 分隔字串
        /// </summary>
        public static readonly string Separator = ";;;";
        #endregion

        #region Member
        /// <summary>
        /// 收單行代碼 / 帳代行代號 (005：土銀)
        /// </summary>
        [XmlIgnore]
        public readonly string AcqBank = "005";

        /// <summary>
        /// 交易型態 (03：繳費)
        /// </summary>
        [XmlIgnore]
        public readonly string TxnType = "03";

        /// <summary>
        /// 版本 (V1)
        /// </summary>
        [XmlIgnore]
        public readonly string Version = "V1";
        #endregion

        #region Property
        /// <summary>
        /// 特店名稱 (土地銀行代收學雜費)
        /// </summary>
        public string MerchantName
        {
            get;
            private set;
        }

        /// <summary>
        /// 特店代號 (005037003011006)
        /// </summary>
        public string MerchantId
        {
            get;
            private set;
        }

        /// <summary>
        /// 端末代號 (90010001)
        /// </summary>
        public string TerminalId
        {
            get;
            private set;
        }

        /// <summary>
        /// 費用代號 (00064019)
        /// </summary>
        public string CostId
        {
            get;
            private set;
        }

        /// <summary>
        /// 安全碼 (AR9zBRoUn8F9)
        /// </summary>
        public string SecureCode
        {
            get;
            private set;
        }

        /// <summary>
        /// 支付工具型態 (00:預設)
        /// </summary>
        public string PaymentType
        {
            get;
            private set;
        }

        /// <summary>
        /// 國別碼 (158)
        /// </summary>
        public string CountryCode
        {
            get;
            private set;
        }

        /// <summary>
        /// 使用者支付手續費 (台灣Pay)
        /// </summary>
        public Decimal? Charge
        {
            get;
            private set;
        }

        #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
        /// <summary>
        /// 繳費網址 (台灣Pay)
        /// </summary>
        public string PayUrl
        {
            get;
            private set;
        }
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 財金 QRCode 支付設定承載類別 物件
        /// </summary>
        private FiscQRCodeConfig()
        {
            this.MerchantName = String.Empty;
            this.MerchantId = String.Empty;
            this.TerminalId = String.Empty;
            this.CostId = String.Empty;
            this.SecureCode = String.Empty;
            this.PaymentType = String.Empty;
            this.CountryCode = String.Empty;
            this.Charge = null;

            #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
            this.PayUrl = String.Empty;
            #endregion
        }

        #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
        /// <summary>
        /// 建構 財金 QRCode 支付設定承載類別 物件
        /// </summary>
        /// <param name="merchantName">特店名稱</param>
        /// <param name="merchantId">特店代號</param>
        /// <param name="terminalId">端末代號</param>
        /// <param name="costId">費用代號</param>
        /// <param name="secureCode">安全碼</param>
        /// <param name="paymentType">支付工具型態</param>
        /// <param name="countryCode">國別碼</param>
        /// <param name="charge">使用者支付手續費</param>
        /// <param name="payUrl">一般繳費網址</param>
        public FiscQRCodeConfig(string merchantName, string merchantId, string terminalId, string costId, string secureCode, string paymentType, string countryCode, string charge, string payUrl)
        {
            this.MerchantName = merchantName == null ? String.Empty : merchantName.Trim();
            this.MerchantId = merchantId == null ? String.Empty : merchantId.Trim();
            this.TerminalId = terminalId == null ? String.Empty : terminalId.Trim();
            this.CostId = costId == null ? String.Empty : costId.Trim();
            this.SecureCode = secureCode == null ? String.Empty : secureCode.Trim();
            this.PaymentType = paymentType == null ? String.Empty : paymentType.Trim();
            this.CountryCode = countryCode == null ? String.Empty : countryCode.Trim();

            decimal val = 0M;
            if (!String.IsNullOrWhiteSpace(charge) && Decimal.TryParse(charge.Trim(), out val))
            {
                this.Charge = val;
            }
            else
            {
                this.Charge = null;
            }

            this.PayUrl = payUrl == null ? String.Empty : payUrl.Trim();
        }
        #endregion
        #endregion

        #region Method
        /// <summary>
        /// 取得資料是否準備好
        /// </summary>
        /// <returns></returns>
        public bool IsReady()
        {
            string errmsg = this.CheckValue();
            return String.IsNullOrEmpty(errmsg);
        }

        /// <summary>
        /// 檢查設定值，傳回錯誤訊息
        /// </summary>
        /// <returns>傳回錯誤訊息 或 null</returns>
        public string CheckValue()
        {
            if (String.IsNullOrEmpty(this.MerchantName))
            {
                return "缺少特店名稱";
            }

            if (!Fuju.Common.IsEnglishNumber(this.MerchantId, 15))
            {
                return "特店代號必須為15碼英數字";
            }

            if (!Fuju.Common.IsEnglishNumber(this.TerminalId, 8))
            {
                return "端末代號必須為8碼英數字";
            }

            if (!Fuju.Common.IsEnglishNumber(this.CostId, 8))
            {
                return "費用代號必須為8碼英數字";
            }

            if (!Fuju.Common.IsEnglishNumber(this.SecureCode, 12))
            {
                return "安全碼必須為12碼英數字";
            }

            if (this.Charge != null && (this.Charge.Value < 0 || this.Charge.Value > 9999.99M))
            {
                return "使用者支付手續費必須為 0 ~ 9999.99 的數值";
            }

            return null;
        }

        /// <summary>
        /// 取得繳費交易(交易型態=03)的收單行資訊
        /// </summary>
        /// <returns></returns>
        public string GetAcqInfo()
        {
            //收單行資訊 = 支付工具型態 + "," + 收單資訊 (不同支付工具型態採用分號 ";" 分開，最後筆無須分號)
            //收單資訊 = 
            //  A. 購物交易：A1. 支付工具型態非51 = 收單行代號(3 bytes) + 特店代號(15 bytes) + 端末代號(8 bytes)
            //               A2. 支付工具型態為51 = 轉入行代碼(3 bytes) + 轉入帳號(16 bytes)
            //  B. 繳費交易：帳代行代號(3 bytes) + 特店代號(15 bytes) + 端末代號(8 bytes)+費用代號(8 bytes)
            return String.Concat(this.PaymentType, ",", this.AcqBank, this.MerchantId, this.TerminalId, this.CostId);
        }

        /// <summary>
        /// 取得 QRCode 字串
        /// </summary>
        /// <param name="amount">交易金額</param>
        /// <param name="cancelNo">銷帳編號</param>
        /// <param name="payDuDate">繳款期限</param>
        /// <param name="feeName">費用名稱</param>
        /// <returns></returns>
        public string GetQRCode(decimal amount, string cancelNo, DateTime payDuDate, string feeName)
        {
            #region 格式範例
            //資料格式： 標準規格識別碼 :// 特店名稱  / 國別碼 / 交易型態 / 版本 ? D類明文區 & E類密文區 & O類其他擴充區
            //標準規格識別碼 = 財金 QRCode 支付固定使用『TWQRP』
            //特店名稱 = 申請時土銀指定，學雜費使用『土地銀行代收學雜費』
            //國別碼 = 申請時土銀指定，學雜費使用『158』
            //交易型態 = 01：購物、02：轉帳、03：繳費：學雜費固定使用 03
            //版本 = 目前應該固定為 V1
            //D類明文區 = 格式為 D類資料代碼=D類資料字串
            //  D類資料代碼    中文名稱           系統參數名稱       長度   Fmt                      MOC(M:必要欄位；O:選擇欄位；C:條件式欄位；X:不使用欄位)
            //  D1             交易金額           txnAmt             1~12   N (最後2碼為小數)        M
            //  D2             訂單編號           orderNbr           1~19   ANS                      X
            //  D3             安全碼             secureCode         12     ANS                      M
            //  D4             繳納期限(截止日)   deadlinefinal      6~8    N (西元yyyyMMdd)         M
            //  D5             轉入行代碼         transfereeBank     3      N                        X
            //  D6             轉入帳號           transfereeAccount  16     N                        X
            //  D7             銷帳編號           noticeNbr          1~16   AN                       M
            //  D8             其他資訊           otherInfo          1~50   ANS                      O
            //  D9             備註               note               1~20   ANS                      X
            //  D10            交易幣別           txnCurrencyCode    3      N                        O
            //  D11            收單行資訊         acqInfo            1~256  ANS                      M
            //  D12            QR Code效期        qrExpirydate       14     N (西元yyyyMMddHHmmss)   O
            //
            //  D14            費用資訊           feeInfo            1~50   ANS                      C
            //  D15            使用者支付手續費   charge             1~6    N (最後2碼為小數)        O
            //  D16            費用名稱           feeName            1~30   ANS                      O
            //交易型態指定為 03 (繳費) 時，D類資料代碼用到 1、3、4、7、8、10、11、12、14、15、16
            //E類密文區 = 格式為 E類資料代碼=E類資料字串，E類資料代碼與D類資料代碼是對應的，資料需要加密時，改用對應的E類 (學雜費不使用)
            //O類其他擴充區 = 保留銀行自訂欄位 (學雜費不使用)
            //資料範例：
            //  TWQRP://臺灣電力公司/158/03/V1?D1=89900&D3=AVnVbcN9xxRv&D4=20170401&D8=電費&D10=901&D11=00,0060061112223334440000000112345678&E7=AUEkfcxUfQNgUHcnNndMDzU=&D12=20170630130000&D15=300&D16=電費
            #endregion

            string sD01 = (amount * 100).ToString("000");       //交易金額 (小數下兩位)
            string sD04 = payDuDate.ToString("yyyyMMdd");       //繳納期限
            string sD10 = "901";                                //交易幣別 (學雜費固定用 901 台幣)
            string sD11 = this.GetAcqInfo();                    //收單行資訊
            string sD15 = this.Charge == null ? String.Empty : (this.Charge.Value * 100).ToString("000");   //使用者支付手續費 (小數下兩位)
            string sD16 = feeName.Length > 15 ? feeName.Substring(0, 15) : feeName;                         //費用名稱
            //土銀學雜費，D類資料代碼用到 1、3、4、7、10、11、15、16
            string sTWQRP = String.Format("TWQRP://{0}/{1}/{2}/{3}?D1={4}&D3={5}&D4={6}&D7={7}&D10={8}&D11={9}&D15={10}&D16={11}", 
                this.MerchantName, this.CountryCode, this.TxnType, this.Version,
                sD01, this.SecureCode, sD04, cancelNo, sD10, sD11, sD15, sD16);

            return sTWQRP;
        }

        /// <summary>
        /// 取得 FiscQRCodeConfig 物件的設定字串
        /// </summary>
        /// <returns>傳回設定字串</returns>
        public override string ToString()
        {
            #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
            bool hasPayUrl = !String.IsNullOrEmpty(this.PayUrl);
            string[] args = hasPayUrl ? new string[9] : new string[8];
            #endregion

            args[0] = "MerchantName:" + this.MerchantName;
            args[1] = "MerchantId:" + this.MerchantId;
            args[2] = "TerminalId:" + this.TerminalId;
            args[3] = "CostId:" + this.CostId;
            args[4] = "SecureCode:" + this.SecureCode;
            args[5] = "PaymentType:" + this.PaymentType;
            args[6] = "CountryCode:" + this.CountryCode;
            if (this.Charge == null)
            {
                args[7] = "Charge:";
            }
            else
            {
                args[7] = "Charge:" + this.Charge.Value.ToString("0.00");
            }

            #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
            if (hasPayUrl)
            {
                args[8] = "PayUrl:" + this.PayUrl;
            }
            #endregion

            return String.Join(Separator, args);
        }
        #endregion

        #region Static Method
        #region [OLD]
        ///// <summary>
        ///// 取得測試用的設定資料
        ///// </summary>
        ///// <returns></returns>
        //public static FiscQRCodeConfig GetTestConfig()
        //{
        //    FiscQRCodeConfig config = new FiscQRCodeConfig("土地銀行代收學雜費", "005037003011006", "90010001", "00064019", "AR9zBRoUn8F9", "00", "158", "15");
        //    return config;
        //}
        #endregion

        /// <summary>
        /// 解析設定字串，轉成 FiscQRCodeConfig 物件
        /// </summary>
        /// <param name="txtConfig">指定設定字串</param>
        /// <param name="isStrict">指定是否嚴格解析，預設 true</param>
        /// <returns>成功則傳回 FiscQRCodeConfig 物件，否則傳回 null</returns>
        public static FiscQRCodeConfig Parse(string txtConfig, bool isStrict = true)
        {
            FiscQRCodeConfig config = null;
            if (!String.IsNullOrWhiteSpace(txtConfig))
            {
                string[] args = txtConfig.Trim().Split(new string[] { Separator }, StringSplitOptions.None);

                #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
                if (!isStrict || args.Length == 8 || args.Length == 9)
                {
                    config = new FiscQRCodeConfig();
                    foreach (string arg in args)
                    {
                        if (arg.StartsWith("MerchantName:"))
                        {
                            config.MerchantName = arg.Substring("MerchantName:".Length).Trim();
                        }
                        else if (arg.StartsWith("MerchantId:"))
                        {
                            config.MerchantId = arg.Substring("MerchantId:".Length).Trim();
                        }
                        else if (arg.StartsWith("TerminalId:"))
                        {
                            config.TerminalId = arg.Substring("TerminalId:".Length).Trim();
                        }
                        else if (arg.StartsWith("CostId:"))
                        {
                            config.CostId = arg.Substring("CostId:".Length).Trim();
                        }
                        else if (arg.StartsWith("SecureCode:"))
                        {
                            config.SecureCode = arg.Substring("SecureCode:".Length).Trim();
                        }
                        else if (arg.StartsWith("PaymentType:"))
                        {
                            config.PaymentType = arg.Substring("PaymentType:".Length).Trim();
                        }
                        else if (arg.StartsWith("CountryCode:"))
                        {
                            config.CountryCode = arg.Substring("CountryCode:".Length).Trim();
                        }
                        else if (arg.StartsWith("Charge:"))
                        {
                            string txt = arg.Substring("Charge:".Length).Trim();
                            if (!String.IsNullOrEmpty(txt))
                            {
                                decimal val = 0M;
                                if (Decimal.TryParse(txt, out val))
                                {
                                    config.Charge = val;
                                }
                                else if (isStrict)
                                {
                                    config = null;
                                    break;
                                }
                                else
                                {
                                    config.Charge = null;
                                }
                            }
                            else
                            {
                                config.Charge = null;
                            }
                        }
                        else if (arg.StartsWith("PayUrl:"))
                        {
                            config.PayUrl = arg.Substring("PayUrl:".Length).Trim();
                        }
                        else if (isStrict)
                        {
                            config = null;
                            break;
                        }
                    }
                }
                #endregion
            }
            return config;
        }
        #endregion
    }
}
