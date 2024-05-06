using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

using System.Reflection;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 財金支付寶處理共用方法類別
    /// </summary>
    public class Fisc
    {
        #region Error Code
        /// <summary>
        /// 錯誤列舉
        /// </summary>
        public enum ErrorCode
        {
            /// <summary>
            /// Web.Config 參數設定錯誤
            /// </summary>
            WebConfigError,
            /// <summary>
            /// 取 Key 發生錯誤
            /// </summary>
            KeyNotFound,
            /// <summary>
            /// 頁面導至財金發生錯誤
            /// </summary>
            RedirectError
        }

        /// <summary>
        /// 取得指定錯誤列舉的錯誤訊息
        /// </summary>
        /// <param name="errorCode">錯誤列舉</param>
        /// <returns>傳回錯誤訊息或空字串</returns>
        public static string GetErrorMessage(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.WebConfigError:
                    return "請檢查 Web.Config 參數設定";
                case ErrorCode.KeyNotFound:
                    return "取 Key 發生錯誤";
                case ErrorCode.RedirectError:
                    return "頁面導向財金發生錯誤";
            }
            return String.Empty;
        }
        #endregion

        public static string GetString(Dictionary<string, string> datas)
        {
            if (datas == null || datas.Count <= 0)
            {
                return "";
            }
            List<string> content = new List<string>();
            foreach (KeyValuePair<string, string> data in datas)
            {
                content.Add(string.Format("{0}={1}", data.Key.Trim(), data.Value.Trim()));
            }
            return string.Join("&", content.ToArray<string>());
        }

        public static Dictionary<string, string> ParsePostData(HttpRequest request)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            foreach (string key in request.Form.AllKeys)
            {
                datas.Add(key, HttpUtility.UrlDecode(Convert.ToString(request.Form[key])));
            }
            return datas;
        }

        public static string GetKeyValue(Dictionary<string, string> datas, string key)
        {
            if (datas.ContainsKey(key))
            {
                return datas[key];
            }
            else
            {
                return null;
            }
        }

        public static string GetTxnType(Dictionary<string, string> datas)
        {
            string key = "txnType";
            return GetKeyValue(datas, key);
        }

        public static string GetFormattedErrorMessage(string error_message)
        {
            return string.Format("{0} {1}", DateTime.Now.ToString("HH:mm:ss"), error_message);
        }

        public static string GetLogPath()
        {
            string folder = "";
            try
            {
                folder = ConfigurationManager.AppSettings["log_path"].Trim();
                DirectoryInfo di = new DirectoryInfo(folder);
                if (!di.Exists)
                {
                    di.Create();
                }
            }
            catch (Exception)
            {
                folder = Path.GetTempPath();
            }
            folder = CompletePath(folder);
            return folder;
        }

        public static string CompletePath(string path)
        {
            return path.EndsWith(@"\") ? path : string.Concat(path, @"\");
        }

        public static bool WriteLog(string ap_name, string msg)
        {
            bool rc = false;

            #region [MDY:20190906] (2019擴充案) 修正日誌檔被鎖住時會丟出例外的問題
            try
            {
                string log_file = string.Format("{0}{1}_{2}.log", GetLogPath(), ap_name, DateTime.Now.ToString("yyyyMMdd"));
                using (StreamWriter sw = new StreamWriter(log_file, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(GetFormattedErrorMessage(msg));
                    rc = true;
                }
            }
            catch (Exception)
            {

            }
            #endregion

            return rc;
        }

        #region [MDY:20180224]
        /// <summary>
        /// Byte 陣列轉成16進位字串
        /// </summary>
        /// <param name="bytes">指定 Byte 陣列</param>
        /// <param name="toUpper">是否使用大寫，預設 true</param>
        /// <returns></returns>
        public static string GetHexString(byte[] bytes, bool toUpper = true)
        {
            if (bytes != null && bytes.Length > 0)
            {
                string hexPattern = toUpper ? "{0:X2}" : "{0:x2}";
                StringBuilder hex = new StringBuilder();
                foreach (byte value in bytes)
                {
                    hex.AppendFormat(hexPattern, value);
                }
                return hex.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 16進位字串規則運算式
        /// </summary>
        public static readonly System.Text.RegularExpressions.Regex HexStringRegex = new System.Text.RegularExpressions.Regex("^([0-9A-F]{2})+$", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        /// <summary>
        /// 16進位字串轉成 Byte 陣列
        /// </summary>
        /// <param name="hexString">指定16進位字串</param>
        /// <param name="bytes">傳回 Byte 陣列</param>
        /// <returns></returns>
        public static string GetHexBytes(string hexString, out byte[] bytes)
        {
            if (hexString.Length == 0)
            {
                bytes = new byte[0];
            }
            else
            {
                bytes = null;
                if (!HexStringRegex.IsMatch(hexString))
                {
                    return "hexString 字串不合法";
                }

                bytes = new byte[hexString.Length / 2];
                for (int idx = 0; idx < bytes.Length; idx++)
                {
                    string hex = hexString.Substring(idx * 2, 2);
                    //bytes[idx] = Byte.Parse(hex, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
                    bytes[idx] = Convert.ToByte(hex, 16);
                }
            }
            return null;
        }

        /// <summary>
        /// 3DES ECB 解密
        /// </summary>
        /// <param name="valueBytes"></param>
        /// <param name="keyBytes"></param>
        /// <param name="decodedText"></param>
        /// <returns></returns>
        private static string Decrypt3DES(byte[] valueBytes, byte[] keyBytes, out string decodedText)
        {
            decodedText = null;
            if (keyBytes == null || keyBytes.Length != 16)
            {
                return "Key 長度不正確";
            }

            string errmsg = null;
            try
            {
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.None;

                ICryptoTransform desICrypto = null;
                if (TripleDESCryptoServiceProvider.IsWeakKey(keyBytes))
                {
                    Type tdesType = tdes.GetType();
                    FieldInfo fiKeyValue = tdesType.GetField("KeyValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    fiKeyValue.SetValue(tdes, keyBytes);

                    FieldInfo fiKeySizeValue = tdesType.GetField("KeySizeValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    fiKeySizeValue.SetValue(tdes, keyBytes.Length * 8);

                    Type modeType = Type.GetType("System.Security.Cryptography.CryptoAPITransformMode");
                    object obj = modeType.GetField("Decrypt", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).GetValue(modeType);
                    MethodInfo mi = tdesType.GetMethod("_NewEncryptor", BindingFlags.Instance | BindingFlags.NonPublic);
                    desICrypto = (ICryptoTransform)mi.Invoke(tdes, new object[] { keyBytes, CipherMode.ECB, null, 0, obj });
                }
                else
                {
                    tdes.Key = keyBytes;
                    desICrypto = tdes.CreateDecryptor();
                }
                byte[] decodedBytes = desICrypto.TransformFinalBlock(valueBytes, 0, valueBytes.Length);
                decodedText = GetHexString(decodedBytes);
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }

        /// <summary>
        /// 3DES ECB 加密
        /// </summary>
        /// <param name="valueBytes"></param>
        /// <param name="keyBytes"></param>
        /// <param name="encodedText"></param>
        /// <returns></returns>
        private static string Encrypt3DES(byte[] valueBytes, byte[] keyBytes, out string encodedText)
        {
            encodedText = null;
            if (keyBytes == null || keyBytes.Length != 16)
            {
                return "Key 長度不正確";
            }

            string errmsg = null;
            try
            {
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.None;

                ICryptoTransform desICrypto = null;
                if (TripleDESCryptoServiceProvider.IsWeakKey(keyBytes))
                {
                    Type tdesType = tdes.GetType();
                    FieldInfo fiKeyValue = tdesType.GetField("KeyValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    fiKeyValue.SetValue(tdes, keyBytes);

                    FieldInfo fiKeySizeValue = tdesType.GetField("KeySizeValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    fiKeySizeValue.SetValue(tdes, keyBytes.Length * 8);

                    Type modeType = Type.GetType("System.Security.Cryptography.CryptoAPITransformMode");
                    object obj = modeType.GetField("Encrypt", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).GetValue(modeType);
                    MethodInfo mi = tdesType.GetMethod("_NewEncryptor", BindingFlags.Instance | BindingFlags.NonPublic);
                    desICrypto = (ICryptoTransform)mi.Invoke(tdes, new object[] { keyBytes, CipherMode.ECB, null, 0, obj });
                }
                else
                {
                    tdes.Key = keyBytes;
                    desICrypto = tdes.CreateEncryptor();
                }
                byte[] eecodedBytes = desICrypto.TransformFinalBlock(valueBytes, 0, valueBytes.Length);
                encodedText = GetHexString(eecodedBytes);
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }
        #endregion

        #region 財金支付寶 支付交易請求
        /// <summary>
        /// 取得付款交易請求的REQ驗證碼
        /// </summary>
        /// <param name="platformNo">平台代號</param>
        /// <param name="orderNumber">訂單編號</param>
        /// <param name="acqBank">收單行代碼</param>
        /// <param name="merchantId">特店代號</param>
        /// <param name="terminalId">端末代號</param>
        /// <param name="purchAmt">交易金額</param>
        /// <param name="localDate">交易日期</param>
        /// <param name="localTime">交易時間</param>
        /// <param name="key">驗證參數</param>
        /// <returns></returns>
        private static string GetPaymentReqSignature(string platformNo, string orderNumber, string acqBank, string merchantId, string terminalId, string purchAmt, string localDate, string localTime, string key)
        {
            //SHA-256(平台代號&訂單編號&收單行代碼&特店代號&端末代號&交易金額&交易日期&交易時間&驗證參數)
            string data = string.Format("{0}&{1}&{2}&{3}&{4}&{5}&{6}&{7}&{8}", platformNo, orderNumber, acqBank, merchantId, terminalId, purchAmt, localDate, localTime, key);
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetByteCount(data));

            return GetHexString(crypto);
        }

        /// <summary>
        /// 產生送給財金支付寶的付款交易請求 Html
        /// </summary>
        /// <param name="inboundUrl">財金URL</param>
        /// <param name="acqBank">收單行代碼</param>
        /// <param name="merchantId">特店代號</param>
        /// <param name="terminalId">端末代號</param>
        /// <param name="key">驗證參數</param>
        /// <param name="authResUrl">交易結果回傳網址</param>
        /// <param name="orderNumber">訂單編號</param>
        /// <param name="itemName">商品名稱</param>
        /// <param name="itemDesc">商品描述 (虛擬帳號)</param>
        /// <param name="purchAmt">交易金額</param>
        /// <returns></returns>
        public static string GenPaymentPostForm(string inboundUrl, string acqBank, string merchantId, string terminalId, string key, string authResUrl, string orderNumber, string itemName, string itemDesc, decimal purchAmt)
        {
            DateTime now = DateTime.Now;
            string localDate = now.ToString("yyyyMMdd");
            string localTime = now.ToString("HHmmss");
            string reqSignature = GetPaymentReqSignature(InboundConfig.PlatformNo, orderNumber, acqBank, merchantId, terminalId, purchAmt.ToString("0"), localDate, localTime, key);
            string html = "";
            html += "<html>";
            html += "<head></head>";
            html += "<body onload=\"document.form1.submit();\">";
            html += "  <form id=\"form1\" name=\"form1\" action=\"{inboundUrl}\" method=\"post\">".Replace("{inboundUrl}", inboundUrl);
            html += "    <div>資料處理中，請稍後....<br/>&nbsp;<br/>請勿關閉此頁面</div>";
            html += "    <table style=\"display:none\">";
            html += "      <tr colspan=\"2\"><th></th></tr>";
            html += "      <tr><th>txnStatus</th><td><input type=\"text\" id=\"txnStatus\" name=\"txnStatus\" value=\"{txnStatus}\" /></td></tr>".Replace("{txnStatus}", "0"); //交易狀態 0：交易請求
            html += "      <tr><th>txnType</th><td><input type=\"text\" id=\"txnType\" name=\"txnType\" value=\"{txnType}\" /></td></tr>".Replace("{txnType}", "0001");        //交易類型，付款交易：0001
            html += "      <tr><th>platformNo</th><td><input type=\"text\" id=\"platformNo\" name=\"platformNo\" value=\"{platformNo}\" /></td></tr>".Replace("{platformNo}", InboundConfig.PlatformNo); //平台代號，支付寶：0001004156
            html += "      <tr><th>orderNumber</th><td><input type=\"text\" id=\"orderNumber\" name=\"orderNumber\" value=\"{orderNumber}\" /></td></tr>".Replace("{orderNumber}", orderNumber); //訂單編號
            html += "      <tr><th>acqBank</th><td><input type=\"text\" id=\"acqBank\" name=\"acqBank\" value=\"{acqBank}\" /></td></tr>".Replace("{acqBank}", acqBank);                     //收單行代碼
            html += "      <tr><th>merchantId</th><td><input type=\"text\" id=\"merchantId\" name=\"merchantId\"  value=\"{merchantId}\" /></td></tr>".Replace("{merchantId}", merchantId);  //特店代號
            html += "      <tr><th>terminalId</th><td><input type=\"text\" id=\"terminalId\" name=\"terminalId\" value=\"{terminalId}\" /></td></tr>".Replace("{terminalId}", terminalId);   //端末代號
            html += "      <tr><th>rrn</th><td><input type=\"text\" id=\"purchAmt\" name=\"purchAmt\" value=\"{purchAmt}\" /></td></tr>".Replace("{purchAmt}", purchAmt.ToString("0"));      //交易金額
            html += "      <tr><th>LocalDate</th><td><input type=\"text\" id=\"LocalDate\" name=\"LocalDate\" value=\"{LocalDate}\" /></td></tr>".Replace("{LocalDate}", localDate);        //交易日期
            html += "      <tr><th>LocalTime</th><td><input type=\"text\" id=\"LocalTime\" name=\"LocalTime\" value=\"{LocalTime}\" /></td></tr>".Replace("{LocalTime}", localTime);         //交易時間
            html += "      <tr><th>itemName</th><td><input type=\"text\" id=\"itemName\" value=\"{itemName}\" name=\"itemName\" /></td></tr>".Replace("{itemName}", itemName);               //商品名稱
            html += "      <tr><th>itemDesc</th><td><input type=\"text\" id=\"itemDesc\" value=\"{itemDesc}\" name=\"itemDesc\" /></td></tr>".Replace("{itemDesc}", itemDesc);               //商品描述 (虛擬帳號)
            html += "      <tr><th>AuthResURL</th><td><input type=\"text\" id=\"AuthResURL\" name=\"AuthResURL\" value=\"{AuthResURL}\" /></td></tr>".Replace("{AuthResURL}", authResUrl);   //交易結果回傳網址
            html += "      <tr><th>reqSignature</th><td><input type=\"text\" id=\"reqSignature\" name=\"reqSignature\" value=\"{reqSignature}\" /></td></tr>".Replace("{reqSignature}", reqSignature); //REQ驗證碼
            html += "      <tr><th>respSignature</th><td><input type=\"text\" id=\"respSignature\" name=\"respSignature\" value=\"\" /></td></tr>"; //RESP驗證碼
            html += "      <tr><th>responseCode</th><td><input type=\"text\" id=\"responseCode\" name=\"responseCode\" value=\"\" /></td></tr>"; //回應碼
            html += "      <tr><th>otherInfo</th><td><input type=\"text\" id=\"otherInfo\" name=\"otherInfo\" value=\"\" /></td></tr>"; //附加資訊
            html += "      <tr><th>signatureType</th><td><input type=\"text\" id=\"signatureType\" name=\"signatureType\" value=\"1\" /></td></tr>"; //驗證碼押碼方式 1:SHAR256
            html += "      <tr><th>errDesc</th><td><input type=\"text\" id=\"errDesc\" name=\"errDesc\" value=\"\" /></td></tr>"; //錯誤描述
            //html += "      <tr><td colspan=\"2\"><input type=\"submit\" /></td></tr>";
            html += "    </table>";
            html += "  </form>";
            html += "</body>";
            html += "</html>";

            return html;
        }

        #region [MDY:20181213] 交易回覆驗證相關
        /// <summary>
        /// 取得付款交易請求的RESP驗證碼
        /// </summary>
        /// <param name="platformNo">平台代號</param>
        /// <param name="orderNumber">訂單編號</param>
        /// <param name="acqBank">收單行代碼</param>
        /// <param name="merchantId">特店代號</param>
        /// <param name="terminalId">端末代號</param>
        /// <param name="responseCode">回應碼</param>
        /// <param name="transRespTime">交易處理回應時間</param>
        /// <param name="key">驗證參數</param>
        /// <returns></returns>
        private static string GetPaymentRespSignature(string platformNo, string orderNumber, string acqBank, string merchantId, string terminalId, string responseCode, string transRespTime, string key)
        {
            //SHA-256(平台代號&訂單編號&收單行代碼&特店代號&端末代號&回應碼&交易處理回應時間&驗證參數)
            string data = string.Format("{0}&{1}&{2}&{3}&{4}&{5}&{6}&{7}", platformNo, orderNumber, acqBank, merchantId, terminalId, responseCode, transRespTime, key);
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetByteCount(data));

            return GetHexString(crypto);
        }

        /// <summary>
        /// 檢查付款交易請求的傳回資料
        /// </summary>
        /// <param name="orderNumber">訂單編號</param>
        /// <param name="merchantId">特店代號</param>
        /// <param name="terminalId">端末代號</param>
        /// <param name="responseCode">回應碼</param>
        /// <param name="transRespTime">交易處理回應時間</param>
        /// <param name="respSignature">RESP交易驗證</param>
        /// <returns></returns>
        public static bool CheckPaymentResponseData(string orderNumber, string acqBank, string merchantId, string terminalId, string responseCode, string transRespTime, string respSignature)
        {
            string errmsg = null;
            InboundConfig inboundConfig = GetInboundConfig(out errmsg);
            if (String.IsNullOrEmpty(errmsg))
            {
                if (InboundConfig.AcqBank == acqBank && inboundConfig.MerchantId == merchantId && inboundConfig.TerminalId == terminalId)
                {
                    string myRespSignature = GetPaymentRespSignature(InboundConfig.PlatformNo, orderNumber, acqBank, merchantId, terminalId, responseCode, transRespTime, inboundConfig.Key);
                    return (myRespSignature == respSignature);
                }
            }
            return false;
        }
        #endregion
        #endregion

        #region [MDY:20180224] 財金支付寶 換KEY交易請求
        /// <summary>
        /// 換KEY交易 交易類型 9001
        /// </summary>
        public static readonly string TxnType_ChangeKey = "9001";

        /// <summary>
        /// 換KEY交易請求的REQ/RESP驗證碼
        /// </summary>
        /// <param name="acqBank">收單行代碼</param>
        /// <param name="merchantId">特店代號</param>
        /// <param name="terminalId">端末代號</param>
        /// <param name="rrn">交易追蹤號</param>
        /// <param name="LocalDate">交易日期</param>
        /// <param name="LocalTime">交易時間</param>
        /// <param name="key">原驗證參數</param>
        /// <returns></returns>
        public static string GetChangeKeySignature(string acqBank, string merchantId, string terminalId, string rrn, string LocalDate, string LocalTime, string key)
        {
            string data = string.Format("{0}&{1}&{2}&{3}&{4}&{5}&{6}", acqBank, merchantId, terminalId, rrn, LocalDate, LocalTime, key);
            SHA256Managed crypt = new SHA256Managed();
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] crypto = crypt.ComputeHash(dataBytes, 0, dataBytes.Length);

            return GetHexString(crypto);
        }

        /// <summary>
        /// 產生換 KEY 交易請求的用的 HttpRequest Post 參數字串
        /// </summary>
        /// <param name="acqBank">收單行代碼</param>
        /// <param name="merchantId">特店代號</param>
        /// <param name="terminalId">端末代號</param>
        /// <param name="key">原驗證參數</param>
        /// <param name="authResUrl">交易結果回傳網址</param>
        /// <param name="rrn">交易追蹤號</param>
        /// <param name="localDate">交易日期</param>
        /// <param name="localTime">交易時間</param>
        /// <returns></returns>
        public static string GenChangeKeyPostData(string acqBank, string merchantId, string terminalId, string key, string authResUrl, out string rrn, out string localDate, out string localTime)
        {
            DateTime txTime = DateTime.Now;
            rrn = String.Format("{0:000}{1:HHmmssfff}", txTime.DayOfYear, txTime);  //交易追蹤號
            localDate = txTime.ToString("yyyyMMdd");    //交易日期
            localTime = txTime.ToString("HHmmss");      //交易時間

            string reqSignature = Fisc.GetChangeKeySignature(acqBank, merchantId, terminalId, rrn, localDate, localTime, key); //REQ驗證碼

            KeyValueList parameters = new KeyValueList();
            parameters.Add("txnStatus", "0");              //0:交易請求
            parameters.Add("txnType", TxnType_ChangeKey);  //交易類型 9001:換KEY交易
            parameters.Add("acqBank", acqBank);            //收單行代碼
            parameters.Add("merchantId", merchantId);      //特店代號
            parameters.Add("terminalId", terminalId);      //端末代號
            parameters.Add("rrn", rrn);                    //交易追蹤號，唯一即可，財金建議用太陽日
            parameters.Add("LocalDate", localDate);        //交易日期
            parameters.Add("LocalTime", localTime);        //交易時間
            parameters.Add("AuthResURL", authResUrl);      //交易結果回傳網址
            parameters.Add("reqSignature", reqSignature);  //REQ驗證碼

            parameters.Add("respSignature", "");    //RESP驗證碼
            parameters.Add("responseCode", "");     //回應碼
            parameters.Add("otherInfo", "");        //附加資訊
            parameters.Add("signatureType", "1");   //驗證碼押碼方式 1:SHA256
            parameters.Add("errDesc", "");          //錯誤描述

            return parameters.ToString("=", "&");
        }

        /// <summary>
        /// 換 KEY 回傳資料解密
        /// </summary>
        /// <param name="encodedNewKey">新驗證碼資訊</param>
        /// <param name="checkValue">新驗證碼檢查值</param>
        /// <param name="oldKey">原驗證參數</param>
        /// <param name="newKey">傳回解密後的新驗證參數</param>
        /// <returns></returns>
        public static string GetDecodedChangeKey(string encodedNewKey, string checkValue, string oldKey, out string newKey)
        {
            newKey = null;
            byte[] valueBytes = null;
            string errmsg = GetHexBytes(encodedNewKey, out valueBytes);
            if (String.IsNullOrEmpty(errmsg))
            {
                if (oldKey.Length == 16)
                {
                    oldKey += oldKey;
                }
                byte[] keyBytes = null;
                errmsg = GetHexBytes(oldKey, out keyBytes);
                if (String.IsNullOrEmpty(errmsg))
                {
                    errmsg = Decrypt3DES(valueBytes, keyBytes, out newKey);

                    #region 檢查 checkValue
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        byte[] value2Bytes = null;
                        string errmsg2 = Fisc.GetHexBytes("".PadLeft(16, '0'), out value2Bytes);
                        if (String.IsNullOrEmpty(errmsg2))
                        {
                            byte[] key2Bytes = null;
                            string key2 = newKey.Length == 16 ? newKey + newKey : newKey;
                            errmsg2 = Fisc.GetHexBytes(key2, out key2Bytes);
                            if (String.IsNullOrEmpty(errmsg2))
                            {
                                string tmp = null;
                                errmsg2 = Encrypt3DES(value2Bytes, key2Bytes, out tmp);
                                if (String.IsNullOrEmpty(errmsg2))
                                {
                                    if (tmp.Length < 6 || tmp.Substring(tmp.Length - 6) != checkValue)
                                    {
                                        errmsg2 = "與計算值不合";
                                    }
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(errmsg2))
                        {
                            errmsg = "tag10 值檢核失敗，" + errmsg2;
                        }
                    }
                    #endregion
                }
            }
            return errmsg;
        }
        #endregion

        #region 財金支付寶設定
        /// <summary>
        /// 財金支付寶系統參數設定資料的 ConfigKey
        /// </summary>
        public static readonly string InbounConfigKey = "Inbound";

        #region [MDY:20180101] 增加財金支付寶授權商家代號設定資料
        /// <summary>
        /// 財金支付寶授權商家代號設定資料的 ConfigKey
        /// </summary>
        public static readonly string InbounReceiveTypeConfigKey = "Inbound_ReceiveType";
        #endregion

        /// <summary>
        /// 取得財金支付寶設定資料
        /// </summary>
        /// <param name="errmsg">成功則傳回 null，否則傳回錯誤訊息</param>
        /// <returns>成功則傳回財金支付寶設定資料物件，否則傳回 null</returns>
        public static InboundConfig GetInboundConfig(out string errmsg)
        {
            errmsg = null;
            InboundConfig data = null;

            #region [MDY:20180101] 增加財金支付寶授權商家代號設定資料
            #region [Old]
            //ConfigEntity config = null;
            //Expression where = new Expression(ConfigEntity.Field.ConfigKey, InbounConfigKey);
            //XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(null, where, null, out config);
            //if (xmlResult.IsSuccess)
            //{
            //    if (config == null)
            //    {
            //        errmsg = "本系統未開通使用支付寶繳費 (缺少支付寶系統參數設定)";
            //    }
            //    else
            //    {
            //        data = ConfigValue2InboundConfig(config.ConfigValue, out errmsg);
            //        if (!String.IsNullOrEmpty(errmsg))
            //        {
            //            errmsg = String.Concat("查詢支付寶相關設定資料失敗：", errmsg);
            //        }
            //    }
            //}
            //else
            //{
            //    errmsg = String.Concat("查詢支付寶相關設定資料失敗：", xmlResult.Message);
            //}
            #endregion

            ConfigEntity[] configs = null;
            Expression where = new Expression(ConfigEntity.Field.ConfigKey, new string[] { InbounConfigKey, InbounReceiveTypeConfigKey });
            XmlResult xmlResult = DataProxy.Current.SelectAll<ConfigEntity>(null, where, null, out configs);
            if (xmlResult.IsSuccess)
            {
                string inbounConfigValue = null;            //支付寶系統參數設定值
                string inbounReceiveTypeConfigValue = null; //支付寶授權商家代號設定值
                if (configs != null && configs.Length > 0)
                {
                    #region 支付寶系統參數設定
                    {
                        ConfigEntity config = configs.FirstOrDefault(x => x.ConfigKey == InbounConfigKey);
                        if (config != null)
                        {
                            inbounConfigValue = config.ConfigValue == null ? null : config.ConfigValue.Trim();
                        }
                    }
                    #endregion

                    #region 授權商家代號設定
                    {
                        ConfigEntity config = configs.FirstOrDefault(x => x.ConfigKey == InbounReceiveTypeConfigKey);
                        if (config != null)
                        {
                            inbounReceiveTypeConfigValue = config.ConfigValue == null ? null : config.ConfigValue.Trim();
                        }
                    }
                    #endregion
                }

                if (String.IsNullOrEmpty(inbounConfigValue))
                {
                    errmsg = "本系統未開通使用支付寶繳費 (缺少支付寶系統參數設定)";
                }
                else
                {
                    errmsg = ConfigValue2InboundConfig(inbounConfigValue, inbounReceiveTypeConfigValue, out data);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        errmsg = String.Concat("解析支付寶相關設定資料失敗：", errmsg);
                    }
                }
            }
            else
            {
                errmsg = String.Concat("查詢支付寶相關設定資料失敗：", xmlResult.Message);
            }
            #endregion

            return data;
        }

        /// <summary>
        /// 儲存財金支付寶設定資料
        /// </summary>
        /// <param name="inbound_config">指定財金支付寶設定資料</param>
        /// <param name="errmsg">成功則傳回 null，否則傳回錯誤訊息</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool SaveInboundConfig(InboundConfig inbound_config, out string errmsg)
        {
            errmsg = null;

            XmlResult xmlResult = null;

            #region 讀取資料
            ConfigEntity config = null;
            Expression where = new Expression(ConfigEntity.Field.ConfigKey, InbounConfigKey);
            xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(null, where, null, out config);
            if (!xmlResult.IsSuccess)
            {
                errmsg = string.Concat("查詢支付寶相關設定資料失敗：", xmlResult.Message);
                return false;
            }
            #endregion

            #region 儲存資料
            string configValue = InboundConfig2ConfigValue(inbound_config);

            int count = 0;
            if (config != null)
            {
                config.ConfigValue = configValue;
                xmlResult = DataProxy.Current.Update(null, config, out count);
                if (!xmlResult.IsSuccess)
                {
                    errmsg = "更新資料失敗：" + xmlResult.Message;
                }
            }
            else
            {
                config = new ConfigEntity();
                config.ConfigKey = InbounConfigKey;
                config.ConfigValue = configValue;
                xmlResult = DataProxy.Current.Insert(null, config, out count);
                if (!xmlResult.IsSuccess)
                {
                    errmsg = "新增資料失敗：" + xmlResult.Message;
                }
            }
            #endregion

            return String.IsNullOrEmpty(errmsg);
        }

        #region [MDY:20180101] 儲存財金支付寶授權商家代號設定資料
        /// <summary>
        /// 儲存財金支付寶授權商家代號設定資料
        /// </summary>
        /// <param name="inbound_config">指定財金支付寶設定資料</param>
        /// <param name="errmsg">成功則傳回 null，否則傳回錯誤訊息</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static string SaveInboundReceiveTypeConfig(System.Web.UI.Page page, string configValue)
        {
            #region 讀取支付寶授權商家代號設定資料
            ConfigEntity config = null;
            {
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, InbounReceiveTypeConfigKey);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(page, where, null, out config);
                if (!xmlResult.IsSuccess)
                {
                    string errmsg = string.Concat("讀取支付寶授權商家代號設定資料失敗：", xmlResult.Message);
                    return errmsg;
                }
            }
            #endregion

            #region 儲存支付寶授權商家代號設定資料
            int count = 0;
            if (config != null)
            {
                config.ConfigValue = configValue;
                XmlResult xmlResult = DataProxy.Current.Update(page, config, out count);
                if (!xmlResult.IsSuccess)
                {
                    string errmsg = string.Concat("更新支付寶授權商家代號設定資料失敗：", xmlResult.Message);
                    return errmsg;
                }
            }
            else
            {
                config = new ConfigEntity();
                config.ConfigKey = InbounReceiveTypeConfigKey;
                config.ConfigValue = configValue;
                XmlResult xmlResult = DataProxy.Current.Insert(page, config, out count);
                if (!xmlResult.IsSuccess)
                {
                    string errmsg = "新增支付寶授權商家代號設定資料失敗：" + xmlResult.Message;
                    return errmsg;
                }
            }
            #endregion

            return null;
        }
        #endregion

        #region [MDY:20180101] 直接使用 InboundConfig 的 CheckValue() 方法檢查設定資料即可
        ///// <summary>
        ///// 檢查指定的InboundConfig 物件的設定資料是否有不合法的值
        ///// </summary>
        ///// <param name="inbound_config">指定的 InboundConfig 物件</param>
        ///// <param name="errmsg">傳回 null 或錯誤訊息</param>
        ///// <returns>合法則傳回 true，否則傳回 false</returns>
        //public static bool CheckInboundConfig(InboundConfig inbound_config, out string errmsg)
        //{
        //    errmsg = null;

        //    if (inbound_config == null)
        //    {
        //        errmsg = "未指定設定資料承載物件";
        //        return false;
        //    }

        //    errmsg = inbound_config.CheckValue();
        //    return String.IsNullOrEmpty(errmsg);
        //}
        #endregion

        #region [MDY:20180101] ConfigValue2InboundConfig 增加財金支付寶授權商家代號設定資料
        #region [Old]
        ///// <summary>
        ///// 將指定的 Config 設定值轉成 InboundConfig 物件
        ///// </summary>
        ///// <param name="config">指定的 Config 設定值</param>
        ///// <param name="errmsg">成功則傳回 null，否則傳回錯誤訊息</param>
        ///// <returns>成功則傳回 InboundConfig 物件，否則傳回 null</returns>
        //private static InboundConfig ConfigValue2InboundConfig(string configValue, out string errmsg)
        //{
        //    InboundConfig data = null;
        //    errmsg = null;
        //    if (String.IsNullOrWhiteSpace(configValue))
        //    {
        //        errmsg = "未指定設定值";
        //    }
        //    else
        //    {
        //        try
        //        {
        //            data = new InboundConfig();

        //            #region 解析設定值
        //            string[] items = configValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        //            foreach (string item in items)
        //            {
        //                string[] temps = item.Split('=');
        //                string key = temps[0].Trim();
        //                string value = null;
        //                if (temps.Length == 2)
        //                {
        //                    value = temps[1].Trim();
        //                }
        //                else if (temps.Length > 2)
        //                {
        //                    value = String.Join("=", temps, 1, temps.Length - 1);
        //                }

        //                switch (key.ToUpper())
        //                {
        //                    case "INBOUNDURL":
        //                        data.InboundUrl = value;
        //                        break;
        //                    case "MERCHANTID":
        //                        data.MerchantId = value;
        //                        break;
        //                    case "TERMINALID":
        //                        data.TerminalId = value;
        //                        break;
        //                    case "INITKEY":
        //                        data.InitKey = value;
        //                        break;
        //                    case "KEY":
        //                        data.Key = value;
        //                        break;
        //                    case "CHARGE":
        //                        data.Charge = value;
        //                        break;
        //                    case "AUTHRESURL":
        //                        data.AuthResUrl = value;
        //                        break;
        //                }
        //            }
        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            data = null;
        //            errmsg = String.Concat("解析設定值失敗，", ex.Message);
        //        }
        //    }
        //    return data;
        //}
        #endregion

        /// <summary>
        /// 將指定的設定值轉成 InboundConfig 物件
        /// </summary>
        /// <param name="inbounSystemConfigValue">指定的支付寶系統參數設定值</param>
        /// <param name="inbounReceiveTypeConfigValue">指定的支付寶授權商家代號設定值</param>
        /// <param name="InboundConfig">成功則傳回 InboundConfig 物件，否則傳回 null</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        private static string ConfigValue2InboundConfig(string inbounSystemConfigValue, string inbounReceiveTypeConfigValue, out InboundConfig data)
        {
            string errmsg = null;

            try
            {
                data = new InboundConfig();

                #region 解析支付寶系統參數設定值
                if (!String.IsNullOrWhiteSpace(inbounSystemConfigValue))
                {
                    string[] items = inbounSystemConfigValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in items)
                    {
                        int index = item.IndexOf("=");
                        if (index < 0)
                        {
                            continue;
                        }

                        string key = item.Substring(0, index).Trim();
                        string value = item.Substring(index + 1);

                        switch (key.ToUpper())
                        {
                            case "INBOUNDURL":
                                data.InboundUrl = value;
                                break;
                            case "MERCHANTID":
                                data.MerchantId = value;
                                break;
                            case "TERMINALID":
                                data.TerminalId = value;
                                break;
                            case "INITKEY":
                                data.InitKey = value;
                                break;
                            case "KEY":
                                data.Key = value;
                                break;
                            case "CHARGE":
                                data.Charge = value;
                                break;
                            case "AUTHRESURL":
                                data.AuthResUrl = value;
                                break;
                        }
                    }
                }
                #endregion

                #region 解析授權商家代號設定值
                #region [MDY:20180926] 改用 ReceiveTypeConfigValue 屬性
                #region [OLD]
                //data.AuthReceiveType = inbounReceiveTypeConfigValue;
                //data.SortAuthReceiveType();
                #endregion

                data.ReceiveTypeConfigValue = inbounReceiveTypeConfigValue;
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                data = null;

                #region [MDY:20220618] Checkmarx 調整 (Information Exposure Through an Error Message)
                #region [OLD] 不要直接把例外訊息傳回，改成將例外寫入 Log
                //errmsg = ex.Message;
                #endregion

                WriteLog("Alipay", $"[{nameof(Fisc)}] (FuncName={nameof(ConfigValue2InboundConfig)}) {ex.Message}");
                errmsg = "取得系統參數設定值發生例外";
                #endregion
            }

            return errmsg;
        }
        #endregion

        /// <summary>
        /// 將指定的 InboundConfig 物件的設定資料轉成 Config 設定值
        /// </summary>
        /// <param name="config">指定的 InboundConfig 物件</param>
        /// <returns>傳回 Config 設定值</returns>
        private static string InboundConfig2ConfigValue(InboundConfig config)
        {
            if (config != null)
            {
                return string.Format("InboundUrl={0};MerchantId={1};TerminalId={2};InitKey={3};Key={4};Charge={5};AuthResUrl={6}"
                    , config.InboundUrl
                    , config.MerchantId
                    , config.TerminalId
                    , config.InitKey
                    , config.Key
                    , config.Charge
                    , config.AuthResUrl);
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion

        #region 支付寶交易紀錄
        /// <summary>
        /// 新增支付寶交易紀錄
        /// </summary>
        /// <param name="data">支付寶交易紀錄</param>
        /// <param name="errmsg">傳回錯誤訊息或 null</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool InsertInboundTxnDtl(InboundTxnDtlEntity data, out string errmsg)
        {
            errmsg = null;
            int count = 0;
            XmlResult result = DataProxy.Current.Insert<InboundTxnDtlEntity>(null, data, out count);
            if (!result.IsSuccess)
            {
                errmsg = result.Message;
            }
            return result.IsSuccess;
        }

        /// <summary>
        /// 取得支付寶交易紀錄
        /// </summary>
        /// <param name="orderNumber">訂單編號</param>
        /// <param name="errmsg">傳回錯誤訊息或 null</param>
        /// <returns>成功則傳回 InboundTxnDtlEntity，否則傳回 null</returns>
        public static InboundTxnDtlEntity GetInboundTxnDtlByOrderNumber(string orderNumber, out string errmsg)
        {
            errmsg = null;
            InboundTxnDtlEntity InboundTxnDtl = null;
            Expression where = new Expression(InboundTxnDtlEntity.Field.OrderNumber, orderNumber);
            KeyValueList<OrderByEnum> orderbys = null;
            XmlResult result = DataProxy.Current.SelectFirst<InboundTxnDtlEntity>(null, where, orderbys, out InboundTxnDtl);
            if (!result.IsSuccess)
            {
                errmsg = result.Message;
            }
            return InboundTxnDtl;
        }

        #region [Old]
        //public static InboundTxnDtlEntity[] GetInboundTxnDtlByCancelNo(string cancel_no, out string log)
        //{
        //    log = "";
        //    InboundTxnDtlEntity[] InboundTxnDtls = null;

        //    return InboundTxnDtls;
        //}
        #endregion

        /// <summary>
        /// 更新支付寶交易紀錄
        /// </summary>
        /// <param name="data">支付寶交易紀錄</param>
        /// <param name="errmsg">傳回錯誤訊息或 null</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool UpdateInboundTxnDtl(InboundTxnDtlEntity data, out string errmsg)
        {
            errmsg = null;
            int count = 0;
            XmlResult result = DataProxy.Current.Update<InboundTxnDtlEntity>(null, data, out count);
            if (!result.IsSuccess)
            {
                errmsg = result.Message;
            }
            return result.IsSuccess;
        }
        #endregion

        #region [MDY:20170828] 因為土銀的支付寶合約未完成，增加是否啟用的判斷
        /// <summary>
        /// 取得是否啟用支付寶繳費
        /// </summary>
        /// <returns></returns>
        public static bool IsInboundEnabled()
        {
            bool isEnabled = false;
            System.Web.Caching.Cache cache = HttpContext.Current.Cache;
            string cacheKey = "IS_INBOUND_ENABLED";
            object value = cache.Get(cacheKey);
            if (value is bool)
            {
                isEnabled = (bool)value;
            }
            else
            {
                string errmsg = null;
                InboundConfig config = GetInboundConfig(out errmsg);

                #region [MDY:20180926] 授權商家代號改用 ReceiveTypeConfigValue 屬性
                #region [OLD]
                //#region [MDY:20180101] 增加授權商家代號檢查
                //#region [Old]
                ////if (config != null && String.IsNullOrEmpty(errmsg) && !String.IsNullOrWhiteSpace(config.Key))
                ////{
                ////    isEnabled = true;
                ////}
                //#endregion

                //if (config != null && String.IsNullOrEmpty(errmsg) && !String.IsNullOrWhiteSpace(config.Key) && !String.IsNullOrWhiteSpace(config.AuthReceiveType))
                //{
                //    isEnabled = true;
                //}
                //#endregion
                #endregion

                if (config != null && String.IsNullOrEmpty(errmsg) && !String.IsNullOrWhiteSpace(config.Key) && !String.IsNullOrWhiteSpace(config.ReceiveTypeConfigValue))
                {
                    isEnabled = true;
                }
                #endregion

                //cache.Insert(cacheKey, isEnabled, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
                DateTime now = DateTime.Now;
                DateTime absoluteExpiration;
                if (now.Hour == 23 && now.Minute >= 30)
                {
                    absoluteExpiration = (new DateTime(now.Year, now.Month, now.Day)).AddDays(1);
                }
                else
                {
                    absoluteExpiration = new DateTime(now.Year, now.Month, now.Day, (now.Minute >= 30 ? (now.Hour + 1) % 24 : now.Hour), (now.Minute < 30 ? 30 : 0), 0);
                }
                cache.Insert(cacheKey, isEnabled, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
            }
            return isEnabled;
        }
        #endregion
    }

    /// <summary>
    /// 財金支付寶相關設定資料承載類別
    /// </summary>
    [Serializable]
    public class InboundConfig
    {
        #region Static Readonly
        /// <summary>
        /// 收單行代碼 005
        /// </summary>
        public static readonly string AcqBank = "005";

        /// <summary>
        /// 平台代號 0001004156
        /// </summary>
        public static readonly string PlatformNo = "0001004156";
        #endregion

        #region Property
        #region 系統參數
        private string _InboundUrl = null;
        /// <summary>
        /// 財金網址
        /// </summary>
        public string InboundUrl
        {
            get
            {
                return _InboundUrl;
            }
            set
            {
                _InboundUrl = value == null ? null : value.Trim();
            }
        }

        private string _MerchantId = null;
        /// <summary>
        /// 特店代號
        /// </summary>
        public string MerchantId
        {
            get
            {
                return _MerchantId;
            }
            set
            {
                _MerchantId = value == null ? null : value.Trim();
            }
        }

        private string _TerminalId = null;
        /// <summary>
        /// 端末代號
        /// </summary>
        public string TerminalId
        {
            get
            {
                return _TerminalId;
            }
            set
            {
                _TerminalId = value == null ? null : value.Trim();
            }
        }

        private string _InitKey = null;
        /// <summary>
        /// 初始驗證參數
        /// </summary>
        public string InitKey
        {
            get
            {
                return _InitKey;
            }
            set
            {
                _InitKey = value == null ? null : value.Trim();
            }
        }

        private string _Key = null;
        /// <summary>
        /// 目前驗證參數
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value == null ? null : value.Trim();
            }
        }

        private string _Charge = null;
        /// <summary>
        /// 手續費率
        /// </summary>
        public string Charge
        {
            get
            {
                return _Charge;
            }
            set
            {
                _Charge = value == null ? null : value.Trim();
            }
        }

        private string _AuthResUrl = null;
        /// <summary>
        /// 回傳網址
        /// </summary>
        public string AuthResUrl
        {
            get
            {
                return _AuthResUrl;
            }
            set
            {
                _AuthResUrl = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region [MDY:20180926] 授權商家代號改用 ReceiveTypeConfigValue 屬性
        #region [OLD]
        //#region [MDY:20180101] 授權商家代號
        //private string _AuthReceiveType = String.Empty;
        ///// <summary>
        ///// 授權商家代號清單
        ///// </summary>
        //public string AuthReceiveType
        //{
        //    get
        //    {
        //        return _AuthReceiveType;
        //    }
        //    set
        //    {
        //        _AuthReceiveType = value == null ? String.Empty : value.Trim();
        //    }
        //}
        //#endregion
        #endregion

        /// <summary>
        /// 授權商家代號清單 ConfigValue 分隔字串
        /// </summary>
        private static readonly string _ReceiveTypeConfigValueSeparator = " ::: ";
        /// <summary>
        /// 授權商家代號清單
        /// </summary>
        private string[] _AuthReceiveTypes = null;
        /// <summary>
        /// 特殊商家代號清單 (開放可繳4000元以下繳費單)
        /// </summary>
        private string[] _SpecialReceiveTypes = null;

        private string _ReceiveTypeConfigValue = String.Empty;
        /// <summary>
        /// 授權商家代號清單 ConfigValue
        /// </summary>
        public string ReceiveTypeConfigValue
        {
            get
            {
                return _ReceiveTypeConfigValue;
            }
            set
            {
                this.ParseReceiveTypeConfigValue(value);
            }
        }
        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// 建構財金支付寶相關設定資料承載物件
        /// </summary>
        public InboundConfig()
        {

        }
        #endregion

        #region Method
        /// <summary>
        /// 檢查系統參數設定資料是否合法
        /// </summary>
        /// <returns>合法則傳回 null，否則傳回 錯誤訊息</returns>
        public string CheckValue()
        {
            #region 財金網址
            if (string.IsNullOrEmpty(this.InboundUrl))
            {
                return "未指定 財金網址";
            }
            else if (!this.InboundUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) && !this.InboundUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                return "財金網址 必須包含 http:// 或 https://";
            }
            #endregion

            #region 特店代號
            if (string.IsNullOrEmpty(this.MerchantId))
            {
                return "未指定 特店代號";
            }
            #endregion

            #region 端末代號
            if (string.IsNullOrEmpty(this.TerminalId))
            {
                return "未指定 端末代號";
            }
            #endregion

            #region [MDY:20180224] 修改 初始驗證參數 與 目前驗證參數 的驗證規則
            #region 初始驗證參數
            {
                if (string.IsNullOrEmpty(this.InitKey))
                {
                    return "未指定 初始驗證參數";
                }
                int initKeySize = this.InitKey.Length;
                if ((initKeySize != 16 && initKeySize != 32) || !Fisc.HexStringRegex.IsMatch(this.InitKey))
                {
                    return "初始驗證參數 必須是 16 或 32 碼的 Hex 字串";
                }
            }
            #endregion

            #region 目前驗證參數
            if (!string.IsNullOrEmpty(this.Key))
            {
                int keySize = this.Key.Length;
                if ((keySize != 16 && keySize != 32) || !Fisc.HexStringRegex.IsMatch(this.Key))
                {
                    return "目前驗證參數 必須是 16 或 32 碼的 Hex 字串";
                }
            }
            #endregion
            #endregion

            #region 手續費率
            if (string.IsNullOrEmpty(this.Charge))
            {
                return "未指定 手續費率";
            }
            else if (this.GetChargeValue() == null)
            {
                return "手續費率 必須是大於或等於 0 的數值";
            }
            #endregion

            #region 回傳網址
            if (string.IsNullOrEmpty(this.AuthResUrl))
            {
                return "未指定 回傳網址";
            }
            else if (!this.AuthResUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) && !this.AuthResUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                return "回傳網址 必須包含 http:// 或 https://";
            }
            #endregion

            return null;
        }

        /// <summary>
        /// 取得手續費率數值 (未設定或非數值或小於 0 則傳回 null)
        /// </summary>
        /// <returns>成功則傳回手續費率數值，否則傳回 null</returns>
        public Decimal? GetChargeValue()
        {
            decimal charge = 0;
            if (!String.IsNullOrEmpty(this.Charge) && Decimal.TryParse(this.Charge, out charge) && charge >= 0)
            {
                return charge;
            }
            return null;
        }

        /// <summary>
        /// 傳回系統參數設定值字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}={1}", "InboundUrl", this.InboundUrl).Append("&");
            sb.AppendFormat("{0}={1}", "MerchantId", this.MerchantId).Append("&");
            sb.AppendFormat("{0}={1}", "TerminalId", this.TerminalId).Append("&");
            sb.AppendFormat("{0}={1}", "InitKey", this.InitKey).Append("&");
            sb.AppendFormat("{0}={1}", "Key", this.Key).Append("&");
            sb.AppendFormat("{0}={1}", "Charge", this.Charge).Append("&");
            sb.AppendFormat("{0}={1}", "AuthResUrl", this.AuthResUrl);

            return sb.ToString();
        }

        #region [MDY:20180926] 授權商家代號改用 ReceiveTypeConfigValue 屬性
        #region [OLD]
        //#region [MDY:20180101] 增加授權商家代號相關處理方法
        ///// <summary>
        ///// 排序並移除空資料與重複的授權商家代號
        ///// </summary>
        //public void SortAuthReceiveType()
        //{
        //    if (!String.IsNullOrEmpty(this.AuthReceiveType))
        //    {
        //        string[] values = this.AuthReceiveType.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        //        if (values != null && values.Length > 0)
        //        {
        //            List<string> list = new List<string>(values.Length);
        //            foreach (string value in values)
        //            {
        //                string val = value.Trim();
        //                if (!list.Contains(val))
        //                {
        //                    list.Add(val);
        //                }
        //            }
        //            list.Sort();
        //            this.AuthReceiveType = String.Join(",", list.ToArray());
        //        }
        //        else
        //        {
        //            this.AuthReceiveType = String.Empty;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 取得指定商家代號是否為授權的商家代號
        ///// </summary>
        ///// <param name="receiveType">指定商家代號</param>
        ///// <returns>是則傳回 true，否則傳回 false</returns>
        //public bool IsAuthReceiveType(string receiveType)
        //{
        //    if (Common.IsNumber(receiveType, 4))
        //    {
        //        string tmp = "," + receiveType + ",";
        //        return (("," + this.AuthReceiveType + ",").IndexOf(tmp) > -1);
        //    }
        //    return false;
        //}
        //#endregion
        #endregion

        /// <summary>
        /// 取得指定商家代號是否為授權的商家代號
        /// </summary>
        /// <param name="receiveType"></param>
        /// <returns></returns>
        public bool IsAuthReceiveType(string receiveType)
        {
            if (Common.IsNumber(receiveType, 4) && _AuthReceiveTypes != null && _AuthReceiveTypes.Length > 0)
            {
                return _AuthReceiveTypes.Contains(receiveType);
            }
            return false;
        }

        /// <summary>
        /// 取得指定商家代號是否為特殊商家代號清單 (開放可繳4000元以下繳費單)
        /// </summary>
        /// <param name="receiveType"></param>
        /// <returns></returns>
        public bool IsSpecialReceiveType(string receiveType)
        {
            if (Common.IsNumber(receiveType, 4) && _SpecialReceiveTypes != null && _SpecialReceiveTypes.Length > 0)
            {
                return _SpecialReceiveTypes.Contains(receiveType);
            }
            return false;
        }

        /// <summary>
        /// 取得授權商家代號陣列
        /// </summary>
        /// <returns></returns>
        public string[] GetAuthReceiveTypes()
        {
            if (_AuthReceiveTypes == null || _AuthReceiveTypes.Length == 0)
            {
                return new string[0];
            }

            string[] values = new string[_AuthReceiveTypes.Length];
            _AuthReceiveTypes.CopyTo(values, 0);
            return values;
        }

        /// <summary>
        /// 取得特殊商家代號陣列 (開放可繳4000元以下繳費單)
        /// </summary>
        /// <returns></returns>
        public string[] GetSpecialReceiveTypes()
        {
            if (_SpecialReceiveTypes == null || _SpecialReceiveTypes.Length == 0)
            {
                return new string[0];
            }

            string[] values = new string[_SpecialReceiveTypes.Length];
            _SpecialReceiveTypes.CopyTo(values, 0);
            return values;
        }

        /// <summary>
        /// 以逗號拆解字串，並排除重複元素，然後傳回排序後的陣列
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private string[] SplitCommaAndSort(string txt)
        {
            List<string> list = null;
            if (!String.IsNullOrWhiteSpace(txt))
            {
                string[] values = txt.Trim().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (values != null && values.Length > 0)
                {
                    list = new List<string>(values.Length);
                    for (int idx = 0; idx < values.Length; idx++)
                    {
                        string value = values[idx].Trim();
                        if (!list.Contains(value))
                        {
                            list.Add(value);
                        }
                    }
                    list.Sort();
                }
            }
            if (list == null || list.Count == 0)
            {
                return new string[0];
            }
            else
            {
                return list.ToArray();
            }
        }

        /// <summary>
        /// 拆解授權商家代號清單 ConfigValue
        /// </summary>
        /// <param name="configValue"></param>
        /// <returns></returns>
        private void ParseReceiveTypeConfigValue(string configValue)
        {
            if (String.IsNullOrWhiteSpace(configValue))
            {
                _AuthReceiveTypes = new string[0];
                _SpecialReceiveTypes = new string[0];
            }
            else
            {
                string[] values = configValue.Trim().Split(new string[] { _ReceiveTypeConfigValueSeparator }, StringSplitOptions.RemoveEmptyEntries);
                _AuthReceiveTypes = this.SplitCommaAndSort(values[0]);

                if (values.Length > 1)
                {
                    _SpecialReceiveTypes = this.SplitCommaAndSort(values[1]);
                }
                else
                {
                    _SpecialReceiveTypes = new string[0];
                }
            }

            _ReceiveTypeConfigValue = GenReceiveTypeConfigValue(_AuthReceiveTypes, _SpecialReceiveTypes);
        }
        #endregion

        #region [MDY:20180224] 取得工作的驗證參數
        /// <summary>
        /// 取得工作的驗證參數 (如果 Key 無值則傳回 InitKey 否則傳回 Key)
        /// </summary>
        /// <returns>傳回驗證參</returns>
        public string GetWorkKey()
        {
            return String.IsNullOrEmpty(this.Key) ? this.InitKey : this.Key;
        }
        #endregion
        #endregion

        #region Static Method
        /// <summary>
        /// 產生 ReceiveTypeConfigValue
        /// </summary>
        /// <param name="authReceiveTypes"></param>
        /// <param name="specialReceiveTypes"></param>
        /// <returns></returns>
        public static string GenReceiveTypeConfigValue(ICollection<string> authReceiveTypes, ICollection<string> specialReceiveTypes)
        {
            if (authReceiveTypes != null && authReceiveTypes.Count > 0)
            {
                if (specialReceiveTypes != null && specialReceiveTypes.Count > 0)
                {
                    return String.Concat(String.Join(",", authReceiveTypes), _ReceiveTypeConfigValueSeparator, String.Join(",", specialReceiveTypes));
                }
                else
                {
                    return String.Join(",", authReceiveTypes);
                }
            }
            else if (specialReceiveTypes != null && specialReceiveTypes.Count > 0)
            {
                return String.Join(",", specialReceiveTypes);
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion
    }

    /// <summary>
    /// 支付寶繳費頁面 Keep 參數介面
    /// </summary>
    public interface IAlipayPayPage
    {
        /// <summary>
        /// Transfer 交易的訂單編號
        /// </summary>
        string TransferOrderNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Transfer 交易的銷帳編號
        /// </summary>
        string TransferCancelNo
        {
            get;
            set;
        }

        /// <summary>
        /// Transfer 交易的應繳總額
        /// </summary>
        decimal TransferAmount
        {
            get;
            set;
        }

        /// <summary>
        /// Transfer 財金支付寶相關設定資料
        /// </summary>
        InboundConfig TransferInboundConfig
        {
            get;
            set;
        }

        /// <summary>
        /// 取得 Transfer 資料的字串格式
        /// </summary>
        /// <returns></returns>
        string GetTransferDataText();
    }
}