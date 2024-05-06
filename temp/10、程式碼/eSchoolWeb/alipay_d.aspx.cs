using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 支付寶繳費 (確認頁面)
    /// </summary>
    public partial class alipay_d : LocalizedPage, IAlipayPayPage
    {
        /// <summary>
        /// InboundTxnDtl 資料的 SessionKey
        /// </summary>
        private const string SESSIONKEY_INBOUNDTXNDTL = "InboundTxnDtl";

        #region Override IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "Alipay";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "支付寶繳費";
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

        #region Implement IAlipayPayPage's Property
        private string _TransferOrderNumber = null;
        /// <summary>
        /// Transfer 交易的訂單編號
        /// </summary>
        public string TransferOrderNumber
        {
            get
            {
                return _TransferOrderNumber;
            }
            set
            {
                _TransferOrderNumber = value == null ? null : value.Trim();
            }
        }

        private string _TransferCancelNo = null;
        /// <summary>
        /// Transfer 交易的銷帳編號
        /// </summary>
        public string TransferCancelNo
        {
            get
            {
                return _TransferCancelNo;
            }
            set
            {
                _TransferCancelNo = value == null ? null : value.Trim();
            }
        }

        private decimal _TransferAmount = 0;
        /// <summary>
        /// Transfer 交易的應繳總額
        /// </summary>
        public decimal TransferAmount
        {
            get
            {
                return _TransferAmount;
            }
            set
            {
                _TransferAmount = value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// Transfer 財金支付寶相關設定資料
        /// </summary>
        public InboundConfig TransferInboundConfig
        {
            get;
            set;
        }
        #endregion

        #region Implement IAlipayPayPage's Method
        /// <summary>
        /// 取得 Transfer 資料的字串格式
        /// </summary>
        /// <returns></returns>
        public string GetTransferDataText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}={1}", "OrderNumber", this.TransferOrderNumber).Append("&");
            sb.AppendFormat("{0}={1}", "CancelNo", this.TransferCancelNo).Append("&");
            sb.AppendFormat("{0}={1}", "Amount", this.TransferAmount).Append("&");
            if (TransferInboundConfig != null)
            {
                sb.Append(TransferInboundConfig.ToString());
            }
            else
            {
                sb.AppendFormat("{0}={1}", "InboundConfig", "null");
            }
            return sb.ToString();
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>傳回錯誤訊息或 null</returns>
        private string InitialUI()
        {
            #region 清空 Session 中要交易的資料
            Session[SESSIONKEY_INBOUNDTXNDTL] = null;
            Session.Remove(SESSIONKEY_INBOUNDTXNDTL);
            #endregion

            alipay prePage = this.PreviousPage as alipay;
            if (prePage == null)
            {
                return "請按流程操作";
            }

            #region Check CnacnelNo
            string cancelNo = prePage.KeepCancelNo;
            if (String.IsNullOrWhiteSpace(cancelNo))
            {
                return "網頁參數錯誤";
            }
            #endregion

            #region Check Keys
            string[] keys = prePage.KeepKeys;
            string receiveType = null;
            string yearId = null;
            string termId = null;
            string depId = null;
            string receiveId = null;
            string stuId = null;
            int oldSeq = -1;
            if (keys != null)
            {
                if (keys.Length == 7)
                {
                    receiveType = keys[0].Trim();
                    yearId = keys[1].Trim();
                    termId = keys[2].Trim();
                    depId = keys[3].Trim();
                    receiveId = keys[4].Trim();
                    stuId = keys[5].Trim();
                    if (!Int32.TryParse(keys[6].Trim(), out oldSeq) || oldSeq < 0)
                    {
                        return "網頁參數錯誤";
                    }
                }
                else
                {
                    return "網頁參數錯誤";
                }
            }
            #endregion

            #region 取得財金支付寶設定資料
            string errmsg = null;
            InboundConfig inboundConfig = Fisc.GetInboundConfig(out errmsg);
            if (inboundConfig == null)
            {
                return errmsg;
            }
            else if (!String.IsNullOrEmpty(inboundConfig.CheckValue()))
            {
                return "支付寶系統參數設定不正確";
            }
            decimal charge = inboundConfig.GetChargeValue().Value;
            #endregion

            errmsg = this.GetAndBindBillData(cancelNo, receiveType, yearId, termId, depId, receiveId, stuId, oldSeq, charge);
            if (!String.IsNullOrEmpty(errmsg))
            {
                return errmsg;
            }

            return null;
        }

        /// <summary>
        /// 取得資料並結繫
        /// </summary>
        /// <param name="cancelNo"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="stuId"></param>
        /// <param name="oldSeq"></param>
        /// <param name="charge"></param>
        /// <returns>傳回錯誤訊息或 null</returns>
        private string GetAndBindBillData(string cancelNo, string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int oldSeq, Decimal charge)
        {
            StudentReceiveView studentReceive = null;

            #region 虛擬帳號 + 未繳條件 + 金額大於 0 (View 已做 NULL 轉空字串 處理)
            Expression where = new Expression(StudentReceiveView.Field.ReceiveType, cancelNo.Substring(0, 4))
                .And(StudentReceiveView.Field.CancelNo, cancelNo)
                .And(new Expression(StudentReceiveView.Field.ReceiveWay, String.Empty).Or(StudentReceiveView.Field.ReceiveWay, null))
                .And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);
            #endregion

            #region 有指定明確的 PKey
            if (!String.IsNullOrEmpty(receiveType))
            {
                where.And(StudentReceiveView.Field.ReceiveType, receiveType)
                    .And(StudentReceiveView.Field.YearId, yearId)
                    .And(StudentReceiveView.Field.TermId, termId)
                    .And(StudentReceiveView.Field.DepId, depId)
                    .And(StudentReceiveView.Field.ReceiveId, receiveId)
                    .And(StudentReceiveView.Field.StuId, stuId)
                    .And(StudentReceiveView.Field.OldSeq, oldSeq);
            }
            #endregion

            #region 依最新的學年、學期、建立日期、資料序號排序
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
            orderbys.Add(StudentReceiveView.Field.YearId, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveView.Field.TermId, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveView.Field.ReceiveType, OrderByEnum.Asc);
            orderbys.Add(StudentReceiveView.Field.CreateDate, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveView.Field.OldSeq, OrderByEnum.Desc);
            #endregion

            DateTime? payDueDate = null;    //繳費期限

            XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveView>(this.Page, where, orderbys, out studentReceive);
            if (xmlResult.IsSuccess)
            {
                if (studentReceive == null)
                {
                    return "查無指定的繳款資料";
                }

                #region 繳費期限 (支付寶走財金管道，但沒說繳款期限要用哪一個，所以用一般的繳費期限)
                payDueDate = DataFormat.ConvertDateText(studentReceive.PayDueDate);    //繳款單的繳費期限 StudentReceive.PayDueDate
                if (payDueDate == null)
                {
                    payDueDate = DataFormat.ConvertDateText(studentReceive.PayDueDate1);   //費用別的繳費期限 SchoolRid.PayDate
                }
                #endregion

                #region 計算手續費與應繳總額
                decimal fee = studentReceive.ReceiveAmount.Value * (charge / 100);
                fee = Math.Ceiling(fee);
                decimal totalAmount = studentReceive.ReceiveAmount.Value + fee;
                #endregion

                SchoolRTypeEntity school = null;
                Expression where3 = new Expression(SchoolRTypeEntity.Field.ReceiveType, studentReceive.ReceiveType);
                xmlResult = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this.Page, where3, null, out school);
                if (xmlResult.IsSuccess)
                {
                    #region 產生 InboundtxndtlEntity
                    InboundTxnDtlEntity inbound_txn_detail = new InboundTxnDtlEntity();
                    inbound_txn_detail.TxnTime = DateTime.Now;
                    inbound_txn_detail.ReceiveType = studentReceive.ReceiveType;
                    inbound_txn_detail.YearId = studentReceive.YearId;
                    inbound_txn_detail.TermId = studentReceive.TermId;
                    inbound_txn_detail.DepId = studentReceive.DepId;
                    inbound_txn_detail.ReceiveId = studentReceive.ReceiveId;
                    inbound_txn_detail.StuId = studentReceive.StuId;
                    inbound_txn_detail.Seq = studentReceive.OldSeq;
                    inbound_txn_detail.CancelNo = studentReceive.CancelNo;
                    inbound_txn_detail.ReceiveAmount = studentReceive.ReceiveAmount.Value;
                    inbound_txn_detail.Fee = fee;
                    inbound_txn_detail.Amount = totalAmount;
                    inbound_txn_detail.OrderNumber = inbound_txn_detail.TxnTime.ToString("yyyyMMddHHmmssfffff");
                    inbound_txn_detail.Status = "0";  //交易請求
                    inbound_txn_detail.TxnMsg = "交易中";
                    #endregion

                    #region 設定 Session 中要交易的資料
                    Session[SESSIONKEY_INBOUNDTXNDTL] = inbound_txn_detail;
                    #endregion

                    #region Bind Data
                    this.txtSchool.Text = school.SchName;
                    this.txtStudent.Text = studentReceive.StuName;
                    this.txtCancelNo.Text = inbound_txn_detail.CancelNo;
                    this.txtAmount.Text = DataFormat.GetAmountText(inbound_txn_detail.ReceiveAmount);
                    this.txtCharge.Text = inbound_txn_detail.Fee.ToString("0");
                    this.txtTotalAmount.Text = inbound_txn_detail.Amount.ToString("0");
                    #endregion
                }
            }
            if (!xmlResult.IsSuccess)
            {
                return "查詢指定繳款資料失敗";
            }
            if (payDueDate == null || payDueDate.Value < DateTime.Today)
            {
                return "已超過繳費期限";
            }
            return null;
        }

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

                string errmsg = this.InitialUI();
                if (!String.IsNullOrEmpty(errmsg))
                {
                    this.ccbtnOK.Visible = false;
                    this.labErrMsg.Text = errmsg;
                }
                else
                {
                    this.ccbtnOK.Visible = true;
                    this.labErrMsg.Text = String.Empty;
                }
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string errmsg = null;

            #region 取得財金支付寶設定資料
            InboundConfig inboundConfig = Fisc.GetInboundConfig(out errmsg);
            if (inboundConfig == null)
            {
                this.ShowJsAlert(errmsg);
                return;
            }
            else if (!String.IsNullOrEmpty(inboundConfig.CheckValue()))
            {
                this.ShowJsAlert("支付寶系統參數設定不正確");
                return;
            }
            this.TransferInboundConfig = inboundConfig;
            #endregion

            #region 取得 Session 中要交易的資料
            InboundTxnDtlEntity inboundTxnDetail = Session[SESSIONKEY_INBOUNDTXNDTL] as InboundTxnDtlEntity;
            if (inboundTxnDetail == null)
            {
                this.ShowJsAlert("頁面閒置逾時");
                return;
            }
            #endregion

            #region 重新指定交易時間與
            inboundTxnDetail.TxnTime = DateTime.Now;
            inboundTxnDetail.OrderNumber = inboundTxnDetail.TxnTime.ToString("yyyyMMddHHmmssfffff");
            this.TransferOrderNumber = inboundTxnDetail.OrderNumber;
            this.TransferCancelNo = inboundTxnDetail.CancelNo;
            this.TransferAmount = inboundTxnDetail.Amount;

            if (!Fisc.InsertInboundTxnDtl(inboundTxnDetail, out errmsg))
            {
                errmsg = string.Concat("新增支付寶交易紀錄失敗，", errmsg);
                this.ShowJsAlert(errmsg);
                return;
            }
            #endregion

            #region [MDY:20210521] 原碼修正
            this.Server.Transfer(WebHelper.GenRNUrl("fisc_inbound_request.aspx"), true);
            #endregion

            #region [Old]
            //string url = "fisc_inbound_request.aspx";
            //string merchantId = inbound_config.merchantId;
            //string terminalId = inbound_config.terminalId;
            //string cancelNo = inbound_txn_detail.CancelNo;
            //decimal amount = inbound_txn_detail.Amount;
            //string key = inbound_config.Key;
            //string html = fisc.GenPostFiscInboundPayForm(url, merchantId, terminalId,inbound_txn_detail.Ordernumber, cancelNo, amount, key);
            //Response.Clear();
            //Response.Write(html);
            //Response.End();
            #endregion
        }
    }
}