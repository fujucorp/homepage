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

using Fuju.Web.Focas;

namespace eSchoolWeb
{
    /// <summary>
    /// 國際信用卡繳費
    /// </summary>
    public partial class EZPosEntry2 : LocalizedPage
    {
        #region Member
        private EncodeTypeCode _EncodeType = EncodeTypeCode.BIG5;

        private string _EZPosUrl = null;
        private string _AuthResURL = null;
        private string _LogPath = null;
        private string _LogName = "EZPosEntry";
        #endregion

        #region Override IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "EZPos2";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "國際信用卡繳費";
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
                #region Initial
                {
                    #region Title
                    Page.Title = "土地銀行 - 代收學雜費服務網 - 國際信用卡繳費 (傳送)";
                    #endregion

                    #region Encode & Charset Meta
                    {
                        System.Web.UI.HtmlControls.HtmlMeta encode = new System.Web.UI.HtmlControls.HtmlMeta();
                        encode.HttpEquiv = "Content-Type";
                        if (_EncodeType.Value == EncodeTypeCode.BIG5.Value)
                        {
                            encode.Content = "text/html; charset=BIG5";
                            this.Response.Charset = "BIG5";
                            this.Response.ContentEncoding = Encoding.GetEncoding("big5");
                        }
                        else if (_EncodeType.Value == EncodeTypeCode.UTF8.Value)
                        {
                            encode.Content = "text/html; charset=UTF-8";
                            this.Response.Charset = "UTF-8";
                            this.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                        }
                        Page.Header.Controls.Add(encode);
                    }
                    #endregion

                    #region EZPosUrl & AuthResURL
                    {
                        ConfigManager config = ConfigManager.Current;

                        #region 配合共用 Config 設定，使用 GetMyMachineProjectConfigValue
                        _EZPosUrl = config.GetMyMachineProjectConfigValue("EZPOS", "EZPosUrl", StringComparison.CurrentCultureIgnoreCase);
                        _AuthResURL = config.GetMyMachineProjectConfigValue("EZPOS", "AuthResURL2", StringComparison.CurrentCultureIgnoreCase);
                        #endregion

                        if (String.IsNullOrEmpty(_EZPosUrl) || String.IsNullOrEmpty(_AuthResURL))
                        {
                            this.divMessage.InnerHtml = @"<span style=""color:red"">本系統未開通使用國際信用卡交易</span>";
                            return;
                        }
                    }
                    #endregion

                    #region LogPath
                    _LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                    #endregion
                }
                #endregion

                #region 網頁參數 & 結繫
                {
                    creditcard2 prePage = this.PreviousPage as creditcard2;
                    if (prePage == null)
                    {
                        this.divMessage.InnerHtml = @"<span style=""color:red"">請按流程操作</span>";
                        return;
                    }

                    KeyValueList<string> payArgs = prePage.KeepPayArgs;
                    if (payArgs == null || payArgs.Count == 0)
                    {
                        this.divMessage.InnerHtml = @"<span style=""color:red"">無法取得網頁參數，請按流程操作</span>";
                        return;
                    }

                    int oldSeq = 0, receiveAmount = 0, apNo = 0;
                    string receiveType = payArgs.TryGetValue("StudentReceive.ReceiveType", null);
                    string yearId = payArgs.TryGetValue("StudentReceive.YearId", null);
                    string termId = payArgs.TryGetValue("StudentReceive.TermId", null);
                    string depId = payArgs.TryGetValue("StudentReceive.DepId", null);
                    string receiveId = payArgs.TryGetValue("StudentReceive.ReceiveId", null);
                    string stuId = payArgs.TryGetValue("StudentReceive.StuId", null);

                    if (!Int32.TryParse(payArgs.TryGetValue("StudentReceive.OldSeq", null), out oldSeq))
                    {
                        oldSeq = -1;
                    }

                    string cancelNo = payArgs.TryGetValue("StudentReceive.CancelNo", null);

                    if (!Int32.TryParse(payArgs.TryGetValue("StudentReceive.ReceiveAmount", null), out receiveAmount))
                    {
                        receiveAmount = -1;
                    }

                    string termName = payArgs.TryGetValue("StudentReceive.TermName", null);
                    string receiveName = payArgs.TryGetValue("StudentReceive.ReceiveName", null);
                    string studentName = payArgs.TryGetValue("StudentMaster.Name", null);
                    string studentIdNumber = payArgs.TryGetValue("StudentMaster.IdNumber", null);
                    string schoolName = payArgs.TryGetValue("SchoolRType.SchName", null);

                    if (!Int32.TryParse(payArgs.TryGetValue("Focas.ApNo", null), out apNo))
                    {
                        apNo = -1;
                    }

                    string merId = payArgs.TryGetValue("Focas.MerId", null);
                    string merchantId = payArgs.TryGetValue("Focas.MerchantId", null);
                    string terminalId = payArgs.TryGetValue("Focas.TerminalId", null);
                    if (String.IsNullOrWhiteSpace(receiveType)
                        || String.IsNullOrWhiteSpace(yearId)
                        || String.IsNullOrWhiteSpace(termId)
                        || depId == null
                        || String.IsNullOrWhiteSpace(receiveId)
                        || String.IsNullOrWhiteSpace(stuId)
                        || oldSeq < 0

                        || String.IsNullOrWhiteSpace(cancelNo)
                        || receiveAmount <= 0

                        || (apNo != 4)
                        || String.IsNullOrWhiteSpace(merId)
                        || String.IsNullOrWhiteSpace(merchantId)
                        || String.IsNullOrWhiteSpace(terminalId))
                    {
                        this.divMessage.InnerHtml = @"<span style=""color:red"">網頁參數不正確，請按流程操作</span>";
                        return;
                    }

                    FocasHelper helper = new FocasHelper(_LogPath, _LogName);
                    AuthTxnReqData reqData = helper.GenAuthTxnReqData(merId, merchantId, terminalId, "土銀代收學雜費系統", cancelNo, receiveAmount, _EZPosUrl, _AuthResURL);

                    #region 結繫資料
                    this.tbxSchoolName.Text = Server.HtmlEncode(schoolName);
                    this.tbxStudentName.Text = Server.HtmlEncode(studentName);
                    this.tbxYearId.Text = Server.HtmlEncode(schoolName);
                    this.tbxTermName.Text = Server.HtmlEncode(termName);
                    this.tbxReceiveName.Text = Server.HtmlEncode(receiveName);
                    this.tbxCancelNo.Text = Server.HtmlEncode(cancelNo);
                    this.tbxTxnId.Text = Server.HtmlEncode(reqData.TxnId);
                    this.tbxReceiveAmount.Text = DataFormat.GetAmountText(receiveAmount);
                    this.divInfo.Visible = true;
                    #endregion

                    if (!helper.GenCCardAuthTxnRequest(this.divForm, reqData))
                    {
                        this.divMessage.InnerHtml = @"<span style=""color:red"">產生傳送資料失敗</span>";
                        return;
                    }

                    #region 新增 EZPos 交易資料
                    {
                        CCardTxnDtlEntity data = new CCardTxnDtlEntity();
                        data.TxnId = reqData.TxnId;
                        data.Rid = cancelNo;
                        data.PayId = String.Empty;
                        data.StudentId = studentIdNumber;
                        data.StudentNo = stuId;
                        data.Amount = reqData.PurchAmount;
                        data.ApNo = apNo;
                        data.Xid = String.Empty;
                        data.Status = "1";  //狀態 (1=交易處理中; 2=交易成功; 3=交易失敗)
                        data.TxnBankId = String.Empty;
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
                        data.ReceiveType = receiveType;
                        data.YearId = yearId;
                        data.TermId = termId;
                        data.DepId = String.Empty;
                        data.ReceiveId = receiveId;
                        data.OldSeq = oldSeq;
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<CCardTxnDtlEntity>(this.Page, data, out count);
                        bool isOK = false;
                        if (xmlResult.IsSuccess)
                        {
                            if (count > 0)
                            {
                                helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) 新增 {2} 交易資料 (CCardTxnDtlEntity) 成功 \r\n\r\n", DateTime.Now, data.TxnId, this.MenuID);
                                isOK = true;
                            }
                            else
                            {
                                helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) 新增 {2} 交易資料 (CCardTxnDtlEntity) 失敗，無任何資料被新增 \r\n\r\n", DateTime.Now, data.TxnId, this.MenuID);
                            }
                        }
                        else
                        {
                            helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) 新增 {2} 交易資料 (CCardTxnDtlEntity) 發生錯誤，錯誤訊息：{3} \r\n\r\n", DateTime.Now, data.TxnId, this.MenuID, xmlResult.Message);
                        }

                        if (isOK)
                        {
                            this.divForm.Visible = true;
                            this.divMessage.InnerHtml = "資料傳送中，即將轉至財金信用卡網路收單網站，請勿關閉瀏覽器。";
                        }
                        else
                        {
                            this.divForm.Visible = false;
                            this.divMessage.InnerHtml = @"<span style=""color:red"">新增交易紀錄失敗</span>";
                        }
                    }
                    #endregion

                }
                #endregion

            }
        }
    }
}