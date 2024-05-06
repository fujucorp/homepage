using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb
{
    /// <summary>
    /// 財金支付寶 Entry 頁面
    /// </summary>
    public partial class fisc_inbound_request : System.Web.UI.Page
    {
        private string _ClientIP = null;
        private string GetClientIP()
        {
            if (_ClientIP == null)
            {
                _ClientIP = WebHelper.GetClientIP();
            }
            return _ClientIP;
        }

        private void WriteLog(string funcName, string msg)
        {
            Fisc.WriteLog("Alipay", String.Format("[fisc_inbound_request] (IP={0}) (FuncName={1}) {2}", this.GetClientIP(), funcName, msg));
        }

        /// <summary>
        /// 發送交易
        /// </summary>
        /// <param name="inboundUrl">財金網址</param>
        /// <param name="acqBank">收單行代碼</param>
        /// <param name="merchantId">特店代號</param>
        /// <param name="terminalId">端末代號</param>
        /// <param name="key">驗證參數</param>
        /// <param name="authResUrl">回傳網址</param>
        /// <param name="orderNumber">訂單編號</param>
        /// <param name="cancelNo">虛擬帳號</param>
        /// <param name="amount">應繳總額</param>
        private void Payment(string inboundUrl, string acqBank, string merchantId, string terminalId, string key, string authResUrl, string orderNumber, string cancelNo, decimal amount)
        {
            try
            {
                string html = Fisc.GenPaymentPostForm(inboundUrl, acqBank, merchantId, terminalId, key, authResUrl, orderNumber, "學雜費", cancelNo, amount);
                Response.Clear();
                Response.Write(html);
            }
            catch (Exception ex)
            {
                Fisc.ErrorCode errCode = Fisc.ErrorCode.RedirectError;
                string errMsg = Fisc.GetErrorMessage(errCode);
                string errDesc = string.Format("[{0}] {1}，請洽系統人員", errCode, errMsg);
                this.litMessage.Text = errDesc;

                string logmsg = string.Format("{0}，錯誤訊息：{1}", errMsg, ex.Message);
                this.WriteLog("Payment", logmsg);
            }
            Response.End();
        }

        #region [Old]
        ///// <summary>
        ///// 換 Key
        ///// </summary>
        ///// <param name="inboundUrl">財金網址</param>
        ///// <param name="acqBank">收單行代碼</param>
        ///// <param name="merchantId">特店代號</param>
        ///// <param name="terminalId">端末代號</param>
        ///// <param name="authResUrl">回傳網址</param>
        ///// <param name="key">驗證參數</param>
        //private void ChangeKey(string inboundUrl, string acqBank, string merchantId, string terminalId, string key, string authResUrl)
        //{
        //    try
        //    {
        //        string html = Fisc.GenChangeKeyPostForm(inboundUrl, acqBank, merchantId, terminalId, authResUrl, key);
        //        Response.Clear();
        //        Response.Write(html);
        //    }
        //    catch (Exception ex)
        //    {
        //        Fisc.ErrorCode errCode = Fisc.ErrorCode.RedirectError;
        //        string errMsg = Fisc.GetErrorMessage(errCode);
        //        string errDesc = string.Format("[{0}] {1}，請洽系統人員", errCode, errMsg);
        //        this.litMessage.Text = errDesc;

        //        string logmsg = string.Format("{0}，錯誤訊息：{1}", errMsg, ex.Message);
        //        this.WriteLog("ChangeKey", logmsg);
        //    }
        //    Response.End();
        //}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region [MDY:20170828] 因為土銀的支付寶合約未完成，增加是否啟用的判斷
                if (!Fisc.IsInboundEnabled())
                {
                    #region [MDY:20210521] 原碼修正
                    Response.Redirect(WebHelper.GenRNUrl("~/index.aspx"));
                    #endregion
                }
                #endregion

                if (this.PreviousPage != null)
                {
                    IAlipayPayPage prePage = this.PreviousPage as IAlipayPayPage;
                    if (prePage == null)
                    {
                        this.litMessage.Text = "缺少網頁參數，請按流程操作";
                        return;
                    }

                    #region 紀錄參數
                    string logmsg = String.Format("Requested {0}", prePage.GetTransferDataText());
                    this.WriteLog("Page_Load", logmsg);
                    #endregion

                    #region 檢查參數
                    string orderNumber = prePage.TransferOrderNumber;
                    string cancelNo = prePage.TransferCancelNo;
                    decimal amount = prePage.TransferAmount;
                    InboundConfig inboundConfig = prePage.TransferInboundConfig;
                    if (String.IsNullOrEmpty(orderNumber) || String.IsNullOrEmpty(cancelNo) || amount <= 0 || inboundConfig == null)
                    {
                        this.litMessage.Text = "網頁參數錯誤，請按流程操作";
                        return;
                    }
                    if (!String.IsNullOrEmpty(inboundConfig.CheckValue()))
                    {
                        this.litMessage.Text = "支付寶系統參數設定不正確";
                        return;
                    }
                    #endregion

                    this.Payment(inboundConfig.InboundUrl, InboundConfig.AcqBank, inboundConfig.MerchantId, inboundConfig.TerminalId, inboundConfig.Key, inboundConfig.AuthResUrl, orderNumber, cancelNo, amount);
                    return;
                }
                else
                {
                    this.litMessage.Text = "請按流程操作";
                }
            }
        }
    }
}