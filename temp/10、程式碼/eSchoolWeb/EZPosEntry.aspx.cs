using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.Configuration;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    public partial class EZPosEntry : LocalizedPage
    {
        #region Log 相關
        private const string _MethodName = "EZPosEntry";
        private string _LogPath = null;

        /// <summary>
        /// 取得 Log 檔完整路徑檔名
        /// </summary>
        /// <returns></returns>
        private string GetLogFileName()
        {
            if (_LogPath == null)
            {
                _LogPath = ConfigurationManager.AppSettings.Get("log_path");
                if (_LogPath == null)
                {
                    _LogPath = String.Empty;
                }
                else
                {
                    _LogPath = _LogPath.Trim();
                }
                if (!String.IsNullOrEmpty(_LogPath))
                {
                    try
                    {
                        if (!Directory.Exists(_LogPath))
                        {
                            Directory.CreateDirectory(_LogPath);
                        }
                    }
                    catch (Exception)
                    {
                        _LogPath = String.Empty;
                    }
                }
            }

            if (String.IsNullOrEmpty(_LogPath))
            {
                return null;
            }
            else
            {
                return Path.Combine(_LogPath, String.Format("{0}_{1:yyyyMMdd}.log", _MethodName, DateTime.Today));
            }
        }

        /// <summary>
        /// 寫 Log
        /// </summary>
        /// <param name="msg">訊息</param>
        private void WriteLog(string msg)
        {
            if (String.IsNullOrEmpty(msg))
            {
                return;
            }
            string logFileName = this.GetLogFileName();
            if (String.IsNullOrEmpty(logFileName))
            {
                return;
            }

            StringBuilder log = new StringBuilder();
            log
                .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, _MethodName).AppendLine()
                .AppendLine(msg)
                .AppendLine();

            this.WriteLogFile(logFileName, log.ToString());
        }

        /// <summary>
        /// 寫入 Log 檔
        /// </summary>
        /// <param name="fileName">Log 檔名</param>
        /// <param name="log">Log 內容</param>
        private void WriteLogFile(string fileName, string log)
        {
            try
            {
                File.AppendAllText(fileName, log, Encoding.Default);
            }
            catch (Exception)
            {
                //_logPath = String.Empty;
            }
        }
        #endregion

        #region EZPos 測試設定
        // EZPosUrl (EZPos 的網址): https://www.focas-test.fisc.com.tw/FOCAS_WEBPOS/online/
        // AuthResUrl (授權回應網址) : http://localhost:49861/EZPosResponse.aspx
        // MerID (特店編號) : 17280856
        // MerchantID (收單行特店代號) : 005172808569001
        // TerminalID (收單行端末代號) : 90010001
        // MerchantName (特店名稱) : 土銀代收學雜費系統
        #endregion

        private string _EZPosUrl = null;
        private string _AuthResURL = null;

        private bool Initial()
        {
            ConfigManager config = ConfigManager.Current;

            #region [MDY:20160416] 配合共用 Config 設定，改用 GetMyMachineProjectConfigValue
            #region Old
            //_EZPosUrl = config.GetProjectConfigValue("EZPOS", "EZPosUrl", StringComparison.CurrentCultureIgnoreCase);
            //_AuthResURL = config.GetProjectConfigValue("EZPOS", "AuthResURL", StringComparison.CurrentCultureIgnoreCase);
            #endregion

            _EZPosUrl = config.GetMyMachineProjectConfigValue("EZPOS", "EZPosUrl", StringComparison.CurrentCultureIgnoreCase);
            _AuthResURL = config.GetMyMachineProjectConfigValue("EZPOS", "AuthResURL", StringComparison.CurrentCultureIgnoreCase);
            #endregion

            if (String.IsNullOrEmpty(_EZPosUrl) || String.IsNullOrEmpty(_AuthResURL))
            {
                this.divForm.InnerHtml = "本系統未開通使用 EZPOS 交易 (缺少 EZPOS 參數設定)";
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 取得交易序號
        /// </summary>
        /// <returns>傳回交易序號</returns>
        private string GetTxnId()
        {
            return DateTime.Now.Ticks.ToString();
        }

        /// <summary>
        /// 取得授權回應網址
        /// </summary>
        /// <param name="txnId">交易序號</param>
        /// <returns>傳回授權回應網址</returns>
        private string GetAuthResUrl(string txnId)
        {
            return String.Format("{0}?Key={1}", _AuthResURL, txnId);
        }

        #region Override IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "EZPos";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "信用卡繳費-財金";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public override bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public override bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public override bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (this.PreviousPage == null)
                {
                    this.Response.Write("請按流程操作");
                    this.Response.End();
                }

                ICreditCardPage prePage = this.PreviousPage as ICreditCardPage;
                if (prePage == null)
                {
                    this.Response.Write("請按流程操作");
                    this.Response.End();
                }

                #region Initial
                if (!this.Initial())
                {
                    return;
                }
                #endregion

                #region 銷帳編號
                //string cancelNo = null;
                //TextBox txtCancelNo = this.PreviousPage.FindControl("txtCancelNo") as TextBox;
                //if (txtCancelNo != null)
                //{
                //    cancelNo = txtCancelNo.Text.Trim();
                //}
                string cancelNo = prePage.KeepCancelNo;
                #endregion

                #region 交易總金額
                //decimal amount = 0;
                //TextBox txtAmount = this.PreviousPage.FindControl("txtAmount") as TextBox;
                //if (txtAmount != null)
                //{
                //    decimal val = 0;
                //    if (Decimal.TryParse(txtAmount.Text.Trim(), out val) && val > 0)
                //    {
                //        amount = val;
                //    }
                //}
                decimal amount = prePage.KeepReceiveAmount;
                #endregion

                #region 持卡人身份證號
                //string payerId = null;
                //TextBox txtPayerId = this.PreviousPage.FindControl("txtPayerId") as TextBox;
                //if (txtPayerId != null)
                //{
                //    payerId = txtPayerId.Text.Trim();
                //}
                string payerId = prePage.KeepPayerId;
                #endregion

                #region 發卡銀行代碼
                //string bankId = null;
                //TextBox txtPayerId = this.PreviousPage.FindControl("txtPayerId") as TextBox;
                //if (txtPayerId != null)
                //{
                //    payerId = txtPayerId.Text.Trim();
                //}
                string bankId = prePage.KeepBankId;
                #endregion

                #region 學生身份證字號
                string studentPId = null;
                //if (prePage != null)
                //{
                    studentPId = prePage.KeepStudentPId;
                //}
                #endregion

                #region 學生學號
                string studentNo = null;
                //if (prePage != null)
                //{
                    studentNo = prePage.KeepStudentNo;
                //}
                #endregion

                #region 財金參數
                string merchantId = prePage.KeepMerchantId;
                string terminalId = prePage.KeepTerminalId;
                string merId = prePage.KeepMerId;
                #endregion

                #region [MDY:20191214] (2019擴充案) 取得 StudentReceive 的 PKey
                #region [MDY:20210706] FIX BUG
                KeyValueList<string> payArgs = null;
                if (this.PreviousPage is creditcard_d)
                {
                    payArgs = (this.PreviousPage as creditcard_d).KeepPayArgs;
                }
                else if (this.PreviousPage is M0003)
                {
                    payArgs = (this.PreviousPage as M0003).KeepPayArgs;
                }
                #endregion
                if (payArgs == null || payArgs.Count == 0)
                {
                    this.Response.Write("無法取得網頁參數，請按流程操作");
                    this.Response.End();
                    return;
                }

                string receiveType = payArgs.TryGetValue("StudentReceive.ReceiveType", null);
                string yearId = payArgs.TryGetValue("StudentReceive.YearId", null);
                string termId = payArgs.TryGetValue("StudentReceive.TermId", null);
                string depId = payArgs.TryGetValue("StudentReceive.DepId", null);
                string receiveId = payArgs.TryGetValue("StudentReceive.ReceiveId", null);
                string stuId = payArgs.TryGetValue("StudentReceive.StuId", null);
                int oldSeq = 0;
                if (!Int32.TryParse(payArgs.TryGetValue("StudentReceive.OldSeq", null), out oldSeq))
                {
                    oldSeq = -1;
                }
                if (String.IsNullOrWhiteSpace(receiveType)
                    || String.IsNullOrWhiteSpace(yearId)
                    || String.IsNullOrWhiteSpace(termId)
                    || depId == null
                    || String.IsNullOrWhiteSpace(receiveId)
                    || String.IsNullOrWhiteSpace(stuId)
                    || oldSeq < 0)
                {
                    this.Response.Write("網頁參數不正確，請按流程操作");
                    this.Response.End();
                }
                #endregion

                #region 檢查參數
                if (String.IsNullOrEmpty(cancelNo) || amount <= 0 || String.IsNullOrEmpty(payerId))
                {
                    this.Response.Write("請按流程操作");
                    this.Response.End();
                }
                if (String.IsNullOrWhiteSpace(merchantId) || String.IsNullOrWhiteSpace(terminalId) || String.IsNullOrWhiteSpace(merId))
                {
                    this.Response.Write("請按流程操作");
                    this.Response.End();
                }
                #endregion

                StringBuilder log = new StringBuilder();

                #region 產生交易資料 Form 的 html
                Encoding big5 = Encoding.GetEncoding("big5");
                string txnId = this.GetTxnId();                 //信用卡交易編號(行內)
                string esposUrl = _EZPosUrl;                    //EZPos 的網址
                string authResURL = this.GetAuthResUrl(txnId);  //授權回應網址
                string merID = merId.Trim();                    //特店編號
                string merchantID = merchantId.Trim();          //收單行特店代號
                string terminalID = terminalId.Trim();          //收單行端末代號
                string merchantName = "土銀代收學雜費系統";     //特店名稱
                string customize = "1";                         //有無客製化網頁(0:無,1:繁體中文化)
                string lidm = cancelNo;                         //銷帳編號
                string purchAmt = amount.ToString("0");         //交易總金額 (改成取整數)
                string currencyNote = "";                       //備註文字(交易記錄傳送至此)
                string antExp = "0";                            //幣值指數
                string autoCap = "1";                           //是否轉入請款作業(0:否,1:是)

                #region 紀錄交易參數
                log
                    .AppendFormat("交易參數： txnId={0}; merID={1}; merchantID={2}; terminalID={3}; merchantName={4}; customize={5}; lidm={6}; purchAmt={7}; currencyNote={8}; antExp={9}; autoCap={10}"
                        , txnId, merID, merchantID, terminalID, merchantName, customize, lidm, purchAmt, currencyNote, antExp, autoCap).AppendLine()
                    .AppendFormat("EZPos 網址 = {0}; 授權回應網址 = {1}", esposUrl, authResURL).AppendLine();
                #endregion

                #region [Old]
                //Response.Write("<form name='TransRedirect' action='" + esposUrl + "' method='post'>");
                //Response.Write("<input type='hidden' name='merID' value='" + merID + "'>");
                //Response.Write("<input type='hidden' name='MerchantID' value='" + MerchantID + "'>");
                //Response.Write("<input type='hidden' name='TerminalID' value='" + TerminalID + "'>");
                //Response.Write("<input type='hidden' name='MerchantName' value='");
                //Response.BinaryWrite(Encoding.Convert(Encoding.Default, big5, Encoding.Default.GetBytes(MerchantName)));
                //Response.Write("'>");
                //Response.Write("<input type='hidden' name='customize' value='" + customize + "'>");
                //Response.Write("<input type='hidden' name='lidm' value='" + lidm + "'>");
                //Response.Write("<input type='hidden' name='purchAmt' value='" + purchAmt + "'>");
                //Response.Write("<input type='hidden' name='CurrencyNote' value='" + CurrencyNote + "'>");
                //Response.Write("<input type='hidden' name='antExp' value='" + antExp + "'>");
                //Response.Write("<input type='hidden' name='AutoCap' value='" + AutoCap + "'>");
                //Response.Write("<input type='hidden' name='AuthResURL' value='" + AuthResURL + "'>");
                //Response.Write("</form>");
                //Response.Write("<script>TransRedirect.submit();</script>");
                #endregion

                StringBuilder html = new StringBuilder();
                html
                    .AppendFormat("<form name='TransRedirect' action='{0}' method='post'>", esposUrl).AppendLine()
                    .AppendFormat("<input type='hidden' name='merID' value='{0}'>", merID).AppendLine()
                    .AppendFormat("<input type='hidden' name='MerchantID' value='{0}'>", merchantID).AppendLine()
                    .AppendFormat("<input type='hidden' name='TerminalID' value='{0}'>", terminalID).AppendLine()
                    .AppendFormat("<input type='hidden' name='MerchantName' value='{0}'>", merchantName).AppendLine()
                    .AppendFormat("<input type='hidden' name='customize' value='{0}'>", customize).AppendLine()
                    .AppendFormat("<input type='hidden' name='lidm' value='{0}'>", lidm).AppendLine()
                    .AppendFormat("<input type='hidden' name='purchAmt' value='{0}'>", purchAmt).AppendLine()
                    .AppendFormat("<input type='hidden' name='CurrencyNote' value='{0}'>", currencyNote).AppendLine()
                    .AppendFormat("<input type='hidden' name='antExp' value='{0}'>", antExp).AppendLine()
                    .AppendFormat("<input type='hidden' name='AutoCap' value='{0}'>", autoCap).AppendLine()
                    .AppendFormat("<input type='hidden' name='AuthResURL' value='{0}'>", authResURL).AppendLine()
                    .AppendLine("</form>")
                    .AppendLine("<script>TransRedirect.submit();</script>");
                #endregion

                #region 新增 EZPos 交易資料
                {
                    CCardTxnDtlEntity data = new CCardTxnDtlEntity();
                    data.TxnId = txnId;
                    data.Rid = cancelNo;
                    data.PayId = payerId;
                    data.StudentId = studentPId;
                    data.StudentNo = studentNo;
                    data.Amount = amount;
                    data.ApNo = 1;
                    data.Xid = String.Empty;
                    data.Status = "1";
                    data.TxnBankId = bankId;
                    data.TxnResult = String.Empty;
                    data.TxnAuthCode = String.Empty;
                    data.TxnAuthDate = null;
                    data.TxnSettleDate = null;
                    data.TxnHostDate = null;
                    data.TxnRemark = String.Empty;
                    data.CreateDate = DateTime.Now;
                    data.UpdateDate = null;
                    data.TxnStatus = String.Empty;
                    data.UpdateMan = null;

                    #region [MDY:20191214] (2019擴充案) 紀錄 StudentReceive 的 PKey
                    data.ReceiveType = receiveType;
                    data.YearId = yearId;
                    data.TermId = termId;
                    data.DepId = String.Empty;
                    data.ReceiveId = receiveId;
                    data.OldSeq = oldSeq;
                    #endregion

                    int count = 0;
                    XmlResult xmlResult = DataProxy.Current.Insert<CCardTxnDtlEntity>(this.Page, data, out count);
                    bool isOK = false;
                    if (xmlResult.IsSuccess)
                    {
                        if (count > 0)
                        {
                            log.AppendLine("新增 EZPos 交易資料 (CCardTxnDtlEntity) 成功").AppendLine();
                            isOK = true;
                        }
                        else
                        {
                            log.AppendLine("新增 EZPos 交易資料 (CCardTxnDtlEntity) 失敗，無任何資料被新增").AppendLine();
                        }
                    }
                    else
                    {
                        log.AppendFormat("新增 EZPos 交易資料 (CCardTxnDtlEntity) 發生錯誤，錯誤訊息：{0}", xmlResult.Message).AppendLine().AppendLine();
                    }

                    if (!isOK)
                    {
                        html.Clear();
                        html
                            .AppendLine("<font color='red'>很抱歉，系統發生錯誤！</font> 如有需要，請詢問系統管理員！<br/>")
                            .AppendFormat("交易編號：{0}", txnId).AppendLine("<br/>")
                            .AppendFormat("持卡人身份證字號：{0}", payerId).AppendLine("<br/>")
                            .AppendFormat("虛擬帳號：{0}", cancelNo).AppendLine("<br/>")
                            .AppendFormat("交易金額：{0:0}", DataFormat.GetAmountText(amount)).AppendLine("<br/>")
                            .AppendFormat("錯誤代碼：{0}", CoreStatusCode.D_NOT_DATA_INSERT).AppendLine("<br/>")
                            .AppendLine("&nbsp;<br/>")
                            .AppendLine("<input type='button' name='button' value='關閉視窗' onclick='window.close();'>");
                    }

                    this.Response.ContentEncoding = big5;
                    this.divForm.InnerHtml = html.ToString();

                    this.WriteLog(log.ToString());
                }
                #endregion
            }
        }
    }
}