using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 財金支付寶 Response 頁面
    /// </summary>
    public partial class fisc_inbound_response : System.Web.UI.Page
    {
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

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
            Fisc.WriteLog("Alipay", String.Format("[fisc_inbound_response] (IP={0}) (FuncName={1}) {2}", this.GetClientIP(), funcName, msg));
        }

        #region
        //private void ChangeKey(Dictionary<string, string> datas)
        //{
        //    string otherInfo = fisc.GetKeyValue(datas, "otherInfo");
        //    string responseCode = fisc.GetKeyValue(datas, "responseCode");
        //    string errDesc = fisc.GetKeyValue(datas, "errDesc");
        //    string respSignature = fisc.GetKeyValue(datas, "respSignature");
        //}
        #endregion

        #region [MDY:20170722] 修改 Checkmarx 的 Reflected XSS All Clients 弱點
        public string GetHtmlEncode(string text)
        {
            return String.IsNullOrEmpty(text) ? "" : Server.HtmlEncode(text);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

            if (!this.IsPostBack)
            {
                #region 取得參數
                Dictionary<string, string> datas = Fisc.ParsePostData(Request);

                string logmsg = string.Format("Requested {0}", Fisc.GetString(datas));
                this.WriteLog("Page_Load", logmsg);
                #endregion

                string TxnType = Fisc.GetTxnType(datas);

                if (TxnType == "0001")  //付款交易
                {
                    #region [MDY:20170828] 因為土銀的支付寶合約未完成，增加是否啟用的判斷
                    if (!Fisc.IsInboundEnabled())
                    {
                        #region [MDY:20210521] 原碼修正
                        Response.Redirect(WebHelper.GenRNUrl("~/index.aspx"));
                        #endregion
                    }
                    #endregion

                    #region [MDY:20170722] 修改 Checkmarx 的 Reflected XSS All Clients 弱點
                    //string itemDesc = Fisc.GetKeyValue(datas,"itemDesc");
                    string purchAmt = this.GetHtmlEncode(Fisc.GetKeyValue(datas, "purchAmt"));
                    string orderNumber = this.GetHtmlEncode(Fisc.GetKeyValue(datas, "orderNumber"));
                    string transRespTime = this.GetHtmlEncode(Fisc.GetKeyValue(datas, "transRespTime"));
                    string responseCode = this.GetHtmlEncode(Fisc.GetKeyValue(datas, "responseCode"));
                    string errDesc = this.GetHtmlEncode(Server.UrlDecode(Fisc.GetKeyValue(datas, "errDesc")));
                    #endregion

                    #region [MDY:20181213] 驗證回傳資料(RESP交易驗證)正確性 (20181207_02)
                    string acqBank = this.GetHtmlEncode(Fisc.GetKeyValue(datas, "acqBank"));
                    string merchantId = this.GetHtmlEncode(Fisc.GetKeyValue(datas, "merchantId"));
                    string terminalId = this.GetHtmlEncode(Fisc.GetKeyValue(datas, "terminalId"));
                    string respSignature = this.GetHtmlEncode(Fisc.GetKeyValue(datas, "respSignature"));
                    bool isResponseDataOK = Fisc.CheckPaymentResponseData(orderNumber, acqBank, merchantId, terminalId, responseCode, transRespTime, respSignature);
                    if (!isResponseDataOK)
                    {
                        this.labMessage.Text = "交易回傳資料驗證失敗！";
                        this.WriteLog("CheckPaymentResponseData", "交易回傳資料驗證失敗！");
                    }
                    #endregion

                    #region
                    string messgae = null;
                    string errmsg = null;
                    InboundTxnDtlEntity InboundTxnDtl = Fisc.GetInboundTxnDtlByOrderNumber(orderNumber, out errmsg);
                    if (InboundTxnDtl != null)
                    {
                        InboundTxnDtl.TxnMsg = string.Format("{0} {1} {2}", transRespTime, responseCode, errDesc);
                        InboundTxnDtl.MdyTime = DateTime.Now;
                        InboundTxnDtl.MdyUser = "response";
                        if (Fisc.UpdateInboundTxnDtl(InboundTxnDtl, out errmsg))
                        {
                            messgae = "交易完成";
                        }
                        else
                        {
                            messgae = string.Concat("更新交易紀錄失敗，", errmsg);
                        }

                        #region [MDY:20181213] 更新 Student_Receive 為已繳待銷：(20181207_02)
                        if (isResponseDataOK)
                        {
                            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, InboundTxnDtl.ReceiveType)
                                .And(StudentReceiveEntity.Field.YearId, InboundTxnDtl.YearId)
                                .And(StudentReceiveEntity.Field.TermId, InboundTxnDtl.TermId)
                                .And(StudentReceiveEntity.Field.DepId, InboundTxnDtl.DepId)
                                .And(StudentReceiveEntity.Field.ReceiveId, InboundTxnDtl.ReceiveId)
                                .And(StudentReceiveEntity.Field.StuId, InboundTxnDtl.StuId)
                                .And(StudentReceiveEntity.Field.OldSeq, InboundTxnDtl.Seq)
                                .And(StudentReceiveEntity.Field.CancelNo, InboundTxnDtl.CancelNo);

                            where.And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty));
                            where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                            where.And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));

                            //銷帳處理以 TxnTime 作為代收日期與代收時間，這裡比照辦理
                            KeyValue[] fieldValues = new KeyValue[3] {
                                new KeyValue(StudentReceiveEntity.Field.ReceiveWay, ChannelHelper.ALIPAY),
                                new KeyValue(StudentReceiveEntity.Field.ReceiveDate, Common.GetTWDate7(InboundTxnDtl.TxnTime)),
                                new KeyValue(StudentReceiveEntity.Field.ReceiveTime, InboundTxnDtl.TxnTime.ToString("HHmmss"))
                            };

                            int count = 0;
                            XmlResult xmlResult = DataProxy.Current.UpdateFields<StudentReceiveEntity>(this, where, fieldValues, out count);
                            if (xmlResult.IsSuccess)
                            {
                                if (count == 0)
                                {
                                    this.WriteLog("UpdateFields", "已繳待銷資料更新失敗：無資料被更新");
                                }
                                else
                                {
                                    this.WriteLog("UpdateFields", "已繳待銷資料更新成功");
                                }
                            }
                            else
                            {
                                this.WriteLog("UpdateFields", "已繳待銷資料更新失敗：" + xmlResult.Message);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            messgae = String.Format("查無 {0} 交易紀錄", orderNumber);
                        }
                        else
                        {
                            messgae = string.Concat("查詢交易紀錄失敗，", errmsg);
                        }
                    }
                    if (!String.IsNullOrEmpty(messgae))
                    {
                        this.WriteLog("Page_Load", messgae);
                    }
                    #endregion

                    #region [MDY:20190104] 增加交易結果欄位，並調整欄位順序
                    #region [OLD]
                    //#region [MDY:20180212] 改用 Table 顯示，無錯誤描述則該欄位顯示「無」
                    //#region [Old]
                    ////string html = "";
                    ////html += "<table>";
                    //////html += "<tr><td>銷帳編號</td><td>{itemDesc}</td></tr>".Replace("{itemDesc}", itemDesc);
                    ////html += "<tr><td>交易金額</td><td>{purchAmt}</td></tr>".Replace("{purchAmt}", purchAmt);
                    ////html += "<tr><td>訂單編號</td><td>{orderNumber}</td></tr>".Replace("{orderNumber}", orderNumber);
                    ////html += "<tr style=\"display:none\"><td>交易處理回應時間</td><td>{transRespTime}</td></tr>".Replace("{transRespTime}", transRespTime);
                    ////html += "<tr><td>回應碼</td><td>{responseCode}</td></tr>".Replace("{responseCode}", responseCode);
                    ////html += "<tr><td>錯誤描述</td><td>{errDesc}</td></tr>".Replace("{errDesc}", errDesc);
                    ////html += "</table>";
                    ////html += "{message}".Replace("{message}", messgae);

                    ////this.litMessage.Text = html;
                    //#endregion

                    //this.labPurchAmt.Text = purchAmt;
                    //this.labOrderNumber.Text = orderNumber;
                    ////this.labTransRespTime.Text = transRespTime;
                    //this.labResponseCode.Text = responseCode;
                    //this.labErrDesc.Text = String.IsNullOrWhiteSpace(errDesc) ? "無" : errDesc;
                    //#endregion
                    #endregion

                    this.labOrderNumber.Text = orderNumber;
                    this.labPurchAmt.Text = purchAmt;
                    //this.labTransRespTime.Text = transRespTime;
                    this.labResponseCode.Text = responseCode;
                    this.labResult.Text = responseCode == "0000" ? "交易成功" : "交易失敗";
                    this.labErrDesc.Text = String.IsNullOrWhiteSpace(errDesc) ? "無" : errDesc;
                    #endregion

                    return;
                }

                #region [OLD]
                //if (TxnType == "9000")//查詢
                //{
                //    return;
                //}

                //if (TxnType == "9001")//換key
                //{
                //    ChangeKey(datas);
                //    return;
                //}
                #endregion
            }
        }
    }
}