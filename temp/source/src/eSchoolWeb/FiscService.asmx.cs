using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using Entities;

namespace eSchoolWeb
{
    /// <summary>
    ///FiscService 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class FiscService : System.Web.Services.WebService
    {
        #region Inbound 換 KEY 交易相關
        /// <summary>
        /// Inbound 換 KEY 作業
        /// </summary>
        /// <param name="jobId">作業序號</param>
        /// <param name="checkCode">檢查碼</param>
        /// <returns></returns>
        [WebMethod]
        public string InboundChangeKey(int jobNo, string checkCode)
        {
            DateTime logDate = DateTime.Now;
            StringBuilder log = new StringBuilder();
            string clientIP = null;
            string errmsg = null;
            string result = null;
            string newKey = null;
            try
            {
                clientIP = WebHelper.GetClientIP();

                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 開始換 KEY (jobNo = {2}; checkCode = {3})", DateTime.Now, clientIP, jobNo, checkCode).AppendLine();

                #region 讀取支付寶相關設定
                InboundConfig config = null;
                string workKey = null;
                if (String.IsNullOrEmpty(errmsg))
                {
                    string errmsg2 = this.GetInboundConfig(out config);
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        workKey = config.GetWorkKey();
                        log.AppendFormat("本次交易使用的驗證參數為 {0}", workKey).AppendLine();
                    }
                    else
                    {
                        errmsg = String.Concat("讀取支付寶相關設定資料失敗，", errmsg2);
                    }
                }
                #endregion

                #region 比對檢查碼
                if (String.IsNullOrEmpty(errmsg))
                {
                    #region [MDY:20220910] Checkmarx - Use Of Broken Or Risky Cryptographic Algorithm 誤判調整
                    string myCheckCode = Fuju.Web.Focas.AlipayHelper.GenFiscServiceCheckCode(jobNo, workKey);
                    #endregion

                    if (myCheckCode != checkCode)
                    {
                        errmsg = "檢查碼不正確";
                    }
                }
                #endregion

                #region 檢查 JobNo
                if (String.IsNullOrEmpty(errmsg))
                {
                    if (jobNo < 0)
                    {
                        errmsg = "作業序號不正確";
                    }
                    else
                    {
                        int count = 0;
                        Expression where = new Expression(JobcubeEntity.Field.Jno, jobNo)
                            .And(JobcubeEntity.Field.Jstatusid, JobCubeStatusCodeTexts.PROCESS)
                            .And(JobcubeEntity.Field.Jtypeid, JobCubeTypeCodeTexts.INBK);
                        XmlResult xmlResult = DataProxy.Current.SelectCount<JobcubeEntity>(null, where, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count == 0)
                            {
                                errmsg = "作業序號不存在";
                            }
                        }
                        else
                        {
                            errmsg = String.Concat("檢查作業序號失敗，", xmlResult.Message);
                        }
                    }
                }
                #endregion

                #region Http Post 換 KEY 資料
                string responseData = null;
                string rqRRN = null;
                string rqLocalDate = null;
                string rqLocalTime = null;
                if (String.IsNullOrEmpty(errmsg))
                {
                    try
                    {
                        DateTime txTime = DateTime.Now;
                        string url = config.InboundUrl;         //財金URL
                        string acqBank = InboundConfig.AcqBank; //收單行代碼
                        string merchantId = config.MerchantId;  //特店代號
                        string terminalId = config.TerminalId;  //端末代號
                        //string key = config.InitKey;
                        string authResUrl = config.AuthResUrl; //交易結果回傳網址
                        //string authResUrl = "https://eschooltestweb.landbank.tw/fisc_inbound_response.aspx";    //交易結果回傳網址

                        string postData = Fisc.GenChangeKeyPostData(acqBank, merchantId, terminalId, workKey, authResUrl, out rqRRN, out rqLocalDate, out rqLocalTime);
                        byte[] postBytes = Encoding.ASCII.GetBytes(postData);

                        log.AppendFormat("HttpWebRequest post 網址：{0}", url).AppendLine();
                        log.AppendFormat("HttpWebRequest post 資料：{0}", postData).AppendLine();

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        ServicePointManager.ServerCertificateValidationCallback = delegate
                        {
                            return true;
                        };

                        HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                        request.Method = "POST";    // 方法
                        request.KeepAlive = true;   //是否保持連線
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = postBytes.Length;

                        using (Stream reqStream = request.GetRequestStream())
                        {
                            reqStream.Write(postBytes, 0, postBytes.Length);
                        }

                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            {
                                responseData = sr.ReadToEnd();
                            }
                        }
                        log.AppendFormat("HttpWebRequest response 資料：{0}{1}", Environment.NewLine, responseData).AppendLine();
                    }
                    catch (Exception ex)
                    {
                        errmsg = String.Concat("Http Post 換 KEY 資料發生例外，", ex.Message);
                        log.AppendFormat("HttpWebRequest 發生例外：{0}{1}", Environment.NewLine, ex).AppendLine();
                    }
                }
                #endregion

                #region 解析回傳資料
                CodeTextList rsDatas = new CodeTextList(13);
                string rsDataText = null;
                if (String.IsNullOrEmpty(errmsg))
                {
                    errmsg = this.ParseChangeKeyResponseData(responseData, out rsDatas, out rsDataText);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        log.AppendFormat("解析換 KEY 回傳資料結果：{0}{1}", Environment.NewLine, rsDataText).AppendLine();
                    }
                    else
                    {
                        log.AppendLine(errmsg);
                    }
                }
                #endregion

                #region 驗證回傳資料
                string responseCode = null;
                string errDesc = null;
                string otherInfo = null;
                if (String.IsNullOrEmpty(errmsg))
                {
                    string errmsg2 = null;

                    #region txnType (交易類型 9001:換KEY交易)
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "txnType");
                        if (one == null || one.Text != Fisc.TxnType_ChangeKey)
                        {
                            errmsg2 = "回傳資料缺少 txnType 欄位或回傳值不正確";
                            log.AppendFormat("{0} (必須是 {1})", errmsg2, Fisc.TxnType_ChangeKey).AppendLine();
                        }
                    }
                    #endregion

                    #region acqBank (收單行代碼)
                    string acqBank = InboundConfig.AcqBank;
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "acqBank");
                        if (one == null || one.Text != acqBank)
                        {
                            errmsg2 = "回傳資料缺少 acqBank 欄位或回傳值不正確";
                            log.AppendFormat("{0} (必須是 {1})", errmsg2, acqBank).AppendLine();
                        }
                    }
                    #endregion

                    #region merchantId (特店代號)
                    string merchantId = config.MerchantId;
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "merchantId");
                        if (one == null || one.Text != merchantId)
                        {
                            errmsg2 = "回傳資料缺少 merchantId 欄位或回傳值不正確";
                            log.AppendFormat("{0} (必須是 {1})", errmsg2, merchantId).AppendLine();
                        }
                    }
                    #endregion

                    #region terminalId (端末代號)
                    string terminalId = config.TerminalId;
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "terminalId");
                        if (one == null || one.Text != terminalId)
                        {
                            errmsg2 = "回傳資料缺少 terminalId 欄位或回傳值不正確";
                            log.AppendFormat("{0} (必須是 {1})", errmsg2, terminalId).AppendLine();
                        }
                    }
                    #endregion

                    #region rrn (交易追蹤號)
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "rrn");
                        if (one == null || one.Text != rqRRN)
                        {
                            errmsg2 = "回傳資料缺少 rrn 欄位或回傳值不正確";
                            log.AppendFormat("{0} (必須是 {1})", errmsg2, rqRRN).AppendLine();
                        }
                    }
                    #endregion

                    #region LocalDate (交易日期)
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "LocalDate");
                        if (one == null || one.Text != rqLocalDate)
                        {
                            errmsg2 = "回傳資料缺少 LocalDate 欄位或回傳值不正確";
                            log.AppendFormat("{0} (必須是 {1})", errmsg2, rqLocalDate).AppendLine();
                        }
                    }
                    #endregion

                    #region LocalTime (交易時間)
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "LocalTime");
                        if (one == null || one.Text != rqLocalTime)
                        {
                            errmsg2 = "回傳資料缺少 LocalTime 欄位或回傳值不正確";
                            log.AppendFormat("{0} (必須是 {1})", errmsg2, rqLocalTime).AppendLine();
                        }
                    }
                    #endregion

                    #region signatureType (驗證碼押碼方式)
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "signatureType");
                        if (one == null || one.Text != "1")
                        {
                            errmsg2 = "回傳資料缺少 signatureType 欄位或回傳值不正確";
                            log.AppendFormat("{0} (必須是 1)", errmsg2).AppendLine();
                        }
                    }
                    #endregion

                    #region txnStatus (交易回覆)
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "txnStatus");
                        if (one == null || one.Text != "1")
                        {
                            errmsg2 = "回傳資料缺少 txnStatus 欄位或回傳值不正確";
                            log.AppendFormat("{0} (必須是 1)", errmsg2).AppendLine();
                        }
                    }
                    #endregion

                    #region responseCode (回應碼)
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "responseCode");
                        if (one == null || String.IsNullOrWhiteSpace(one.Text))
                        {
                            errmsg2 = "回傳資料缺少 responseCode 欄位或回傳值不正確";
                            log.AppendLine(errmsg2);
                        }
                        else
                        {
                            responseCode = one.Text;
                        }
                    }
                    #endregion

                    #region errDesc (錯誤描述)
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "errDesc");
                        if (one == null)
                        {
                            errmsg2 = "回傳資料缺少 errDesc 欄位";
                            log.AppendLine(errmsg2);
                        }
                        else
                        {
                            errDesc = HttpUtility.UrlDecode(one.Text);
                        }
                    }
                    #endregion

                    #region otherInfo (附加資訊)
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "otherInfo");
                        if (one == null)
                        {
                            errmsg2 = "回傳資料缺少 otherInfo 欄位";
                            log.AppendLine(errmsg2);
                        }
                        else
                        {
                            otherInfo = one.Text;
                            if (responseCode == "0000")
                            {
                                //Regex keyReg = new Regex(@"^\{""tag06"":""([0-9A-Z]{16})"",""tag10"":""([0-9A-Z]+)""\}$");
                                Regex keyReg = new Regex(@"^\{""tag06"":""([0-9A-Z]{16}|[0-9A-Z]{32})"",""tag10"":""([0-9A-Z]+)""\}$");
                                Match match = keyReg.Match(otherInfo);
                                if (match.Success)
                                {
                                    string encodeNewKey = match.Groups[1].Value;
                                    string checkValue = match.Groups[2].Value;
                                    string errmsg3 = Fisc.GetDecodedChangeKey(encodeNewKey, checkValue, workKey, out newKey);
                                    if (String.IsNullOrEmpty(errmsg3))
                                    {
                                        log.AppendFormat("解密後的新驗證參數：{0}", newKey).AppendLine();
                                    }
                                    else
                                    {
                                        errmsg2 = String.Concat("回傳資料 otherInfo 無法解密，", errmsg3);
                                        log.AppendLine(errmsg2);
                                    }
                                }
                                else
                                {
                                    errmsg2 = "回傳資料 otherInfo 格式不正確";
                                    log.AppendLine(errmsg2);
                                }
                            }
                        }
                    }
                    #endregion

                    #region respSignature (RESP驗證碼)
                    if (String.IsNullOrEmpty(errmsg2))
                    {
                        CodeText one = rsDatas.Find(x => x.Code == "respSignature");
                        if (one == null || String.IsNullOrWhiteSpace(one.Text))
                        {
                            errmsg2 = "回傳資料缺少 respSignature 欄位";
                            log.AppendLine(errmsg2);
                        }
                        else if (responseCode == "0000")
                        {
                            //驗證 RESP 資料
                            string respSignature = Fisc.GetChangeKeySignature(acqBank, merchantId, terminalId, rqRRN, rqLocalDate, rqLocalTime, newKey);
                            if (!respSignature.Equals(one.Text, StringComparison.CurrentCultureIgnoreCase))
                            {
                                errmsg2 = "回傳資料 respSignature 驗證檢核有誤";
                                log.AppendFormat("{0} (必須是 {1})", errmsg2, respSignature).AppendLine();
                            }
                        }
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg2))
                    {
                        errmsg = String.Concat(errmsg2, "，回傳資料：", rsDataText);
                    }
                    else if (responseCode != "0000")
                    {
                        errmsg = String.Format("換 KEY 失敗，錯誤代碼：{0}，錯誤訊息：{1}，回傳資料：{2}", responseCode, errDesc, rsDataText);
                    }
                }
                #endregion

                #region 更新支付寶相關設定
                if (String.IsNullOrEmpty(errmsg))
                {
                    config.Key = newKey;
                    string errmsg2 = null;
                    if (Fisc.SaveInboundConfig(config, out errmsg2))
                    {
                        log.AppendFormat("更新支付寶相關設定成功，{0}", config).AppendLine();
                    }
                    else
                    {
                        errmsg = String.Concat("更新支付寶相關設定失敗，", errmsg2);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                errmsg = String.Concat("發生例外，", ex.Message);
                log.AppendFormat("換 KEY 發生例外：{0}{1}", Environment.NewLine, ex).AppendLine();
            }
            finally
            {
                if (String.IsNullOrEmpty(errmsg))
                {
                    result = String.Concat("SUCCESS。新驗證參數：", newKey);
                }
                else
                {
                    result = String.Concat("FAILURE。", errmsg);
                }
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 結束換 KEY (回傳：{2})", DateTime.Now, clientIP, result).AppendLine();

                string errmsg2 = this.WriteLog(logDate, log);
                if (!String.IsNullOrEmpty(errmsg2))
                {
                    result = String.Format("{0} (注意：Web 端日誌檔寫入失敗，{1})", result, errmsg2);
                }
            }

            return result;
        }

        /// <summary>
        /// 解析換 KEY 交易回傳資料 (HTML)
        /// </summary>
        /// <param name="responseData">換 KEY 交易回傳資料 (HTML)</param>
        /// <param name="rsDatas">傳回解析後參數名稱 (CODE)與參數值 (TEXT) 對照集合</param>
        /// <param name="rsDataText">傳回解析後參數資料字串 (參數名稱=參數值&....)</param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回 null</returns>
        private string ParseChangeKeyResponseData(string responseData, out CodeTextList rsDatas, out string rsDataText)
        {
            rsDatas = new CodeTextList(13);
            rsDataText = null;
            string errmsg = null;
            try
            {
                StringBuilder txt = new StringBuilder();

                #region 解析
                //Regex txtReg = new Regex(@"(<input type=(['""]?)text\2 name=(['""]?)(txnType|acqBank|merchantId|terminalId|rrn|LocalDate|LocalTime|responseCode|errDesc|otherInfo|respSignature|signatureType|txnStatus)\3 value=(['""]?)(.{0,})\5( [^>]+)?>)", RegexOptions.IgnoreCase);
                Regex txtReg = new Regex(@"(<input +type=(['""]?)text\2 +name=(['""]?)(txnType|acqBank|merchantId|terminalId|rrn|LocalDate|LocalTime|responseCode|errDesc|otherInfo|respSignature|signatureType|txnStatus)\3 +value=(['""]?)([^>]*)\5( [^>]*)*>)", RegexOptions.IgnoreCase);
                MatchCollection matchs = txtReg.Matches(responseData);
                foreach (Match match in matchs)
                {
                    GroupCollection groups = match.Groups;
                    string name = groups[4].Value.Trim();
                    string value = groups[6].Value.Trim();
                    rsDatas.Add(name, value);
                    txt.AppendFormat("&{0}={1}", name, value);
                }
                #endregion

                rsDataText = txt.ToString(1, txt.Length - 1);
            }
            catch(Exception ex)
            {
                errmsg = String.Concat("解析換 KEY 回傳資料失敗，", ex.Message);
            }
            return errmsg;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 取得支付寶相關設定資料，並檢查設定值
        /// </summary>
        /// <param name="config">傳回支付寶相關設定資料</param>
        /// <returns>傳回錯誤訊息</returns>
        private string GetInboundConfig(out InboundConfig config)
        {
            string errmsg = null;
            config = Fisc.GetInboundConfig(out errmsg);
            if (String.IsNullOrEmpty(errmsg))
            {
                #region 財金網址
                if (string.IsNullOrEmpty(config.InboundUrl)
                    || (!config.InboundUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) && !config.InboundUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                    )
                {
                    return "缺少財金網址或設定值不正確";
                }
                #endregion

                #region 特店代號
                if (string.IsNullOrEmpty(config.MerchantId))
                {
                    return "缺少特店代號";
                }
                #endregion

                #region 端末代號
                if (string.IsNullOrEmpty(config.TerminalId))
                {
                    return "缺少端末代號";
                }
                #endregion

                #region 初始驗證參數
                {
                    if (string.IsNullOrEmpty(config.InitKey))
                    {
                        return "缺少初始驗證參數";
                    }
                    int InitKeySize = config.InitKey.Length;
                    if ((InitKeySize != 16 && InitKeySize != 32) || !Fisc.HexStringRegex.IsMatch(config.InitKey))
                    {
                        return "初始驗證參數 必須是 16 或 32 碼的 Hex 字串";
                    }
                }
                #endregion

                #region 目前驗證參數
                if (!string.IsNullOrEmpty(config.Key))
                {
                    int keySize = config.Key.Length;
                    if ((keySize != 16 && keySize != 32) || !Fisc.HexStringRegex.IsMatch(config.Key))
                    {
                        return "目前驗證參數 必須是 16 或 32 碼的 Hex 字串";
                    }
                }
                #endregion

                return null;
            }
            else
            {
                return errmsg;
            }
        }

        private string WriteLog(DateTime logDate, StringBuilder log)
        {
            string errmsg = null;
            string logPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
            if (!String.IsNullOrWhiteSpace(logPath) && log != null && log.Length > 0)
            {
                try
                {
                    string logFile = String.Format("FiscService_{0:yyyyMMdd}.log", logDate);
                    using (StreamWriter sw = new StreamWriter(Path.Combine(logPath, logFile), true, Encoding.Default))
                    {
                        sw.WriteLine(log.ToString());
                    }
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
            }
            return errmsg;
        }
        #endregion
    }
}
