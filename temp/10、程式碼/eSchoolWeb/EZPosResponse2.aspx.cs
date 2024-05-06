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
using Fuju.DB;
using Fuju.Web;

using Entities;

using Fuju.Web.Focas;

namespace eSchoolWeb
{
    public partial class EZPosResponse2 : LocalizedPage
    {
        #region Member
        private EncodeTypeCode _EncodeType = EncodeTypeCode.BIG5;

        private string _LogPath = null;
        private string _LogName = "EZPosResponse";
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
                    Page.Title = "土地銀行 - 代收學雜費服務網 - 國際信用卡繳費 (接收)";
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

                    #region LogPath
                    _LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                    #endregion
                }
                #endregion

                #region 回應資料
                FocasHelper helper = new FocasHelper(_LogPath, _LogName);
                AuthTxnRspData rspData = helper.GetAuthTxnRspData();
                if (rspData == null || String.IsNullOrEmpty(rspData.TxnId) || String.IsNullOrEmpty(rspData.CheckCode))
                {
                    this.labResult.Text = @"<span style=""color:red"">無法取得交易結果資料，本次交易無法判斷是否交易成功</span>";
                    this.divResult.Visible = false;
                    return;
                }
                if (String.IsNullOrEmpty(rspData.Status) || (rspData.Status != "0" && String.IsNullOrEmpty(rspData.ErrCode)))
                {
                    this.labResult.Text = @"<span style=""color:red"">無法判斷授權結果狀態，本次交易無法判斷是否交易成功</span>";
                    this.divResult.Visible = false;
                    return;
                }

                string myStatus = null, txnResult = null, txnRemark = null;
                if (rspData.Status == "0" && !String.IsNullOrEmpty(rspData.AuthCode))
                {
                    myStatus = "2";
                    txnResult = rspData.LastPan4;
                    txnRemark = String.Empty;

                    this.labResult.Text = @"交易成功，相關訊息說明如下：";
                    this.thErrCode.Visible = false;
                    this.thErrDesc.Visible = false;
                    this.divResult.Visible = true;
                }
                else
                {
                    myStatus = "3";
                    txnResult = String.Concat(rspData.Status, ":", rspData.ErrCode);
                    txnRemark = rspData.ErrDesc;

                    this.labResult.Text = @"交易失敗，相關訊息說明如下：";
                    this.thErrCode.Visible = true;
                    this.thErrDesc.Visible = true;
                    this.divResult.Visible = true;
                }
                #endregion

                #region 結繫
                this.tbxTxnId.Text = rspData.TxnId;
                this.tbxLidm.Text = rspData.Lidm;
                this.tbxAuthAmt.Text = rspData.AuthAmt;
                this.tbxCurrency.Text = rspData.Currency;
                this.tbxStatus.Text = rspData.Status;
                this.tbxAuthCode.Text = rspData.AuthCode;
                this.tbxXid.Text = rspData.Xid;
                this.tbxLastPan4.Text = rspData.LastPan4;
                this.tbxErrCode.Text = rspData.ErrCode;
                this.tbxErrDesc.Text = rspData.ErrDesc;
                #endregion

                #region 取得交易資料
                bool toUpdate = true;
                CCardTxnDtlEntity txn = null;
                {
                    Expression where = new Expression(CCardTxnDtlEntity.Field.TxnId, rspData.TxnId);
                    XmlResult xmlResult = DataProxy.Current.SelectFirst<CCardTxnDtlEntity>(this.Page, where, null, out txn);
                    if (xmlResult.IsSuccess)
                    {
                        if (txn == null)
                        {
                            toUpdate = false;
                            this.labMemo.Text = @"<span style=""color:red"">注意：信用卡繳費平台回應資料與學雜費系統紀錄的交易資料不一致，系統將不更新該繳費單狀態</span>";
                            helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) 查無 {2} 交易資料 (CCardTxnDtlEntity)，不更新相關資料 \r\n\r\n", DateTime.Now, rspData.TxnId, this.MenuID);
                        }
                        else if (txn.Rid != rspData.Lidm)
                        {
                            toUpdate = false;
                            this.labMemo.Text = @"<span style=""color:red"">注意：信用卡繳費平台回應資料與學雜費系統紀錄的交易資料不一致，系統將不更新該繳費單狀態</span>";
                            helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) {2} 交易資料 (CCardTxnDtlEntity) 的虛擬帳號 ({3}) 與回應資料 ({4}) 不一致，不更新相關資料 \r\n\r\n", DateTime.Now, rspData.TxnId, this.MenuID, txn.Rid, rspData.Lidm);
                        }
                        else if (txn.Status != "1")
                        {
                            toUpdate = false;
                            if (txn.Status != myStatus)
                            {
                                this.labMemo.Text = @"<span style=""color:red"">注意：信用卡繳費平台回應資料與學雜費系統紀錄的交易資料不一致，系統將不更新該繳費單狀態</span>";
                                helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) {2} 交易資料 (CCardTxnDtlEntity) 的狀態 ({3}) 與回應資料 ({4}) 不一致，不更新相關資料 \r\n\r\n", DateTime.Now, rspData.TxnId, this.MenuID, txn.Status, myStatus);
                            }
                            else
                            {
                                helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) {2} 交易資料 (CCardTxnDtlEntity) 的狀態非交易處理中 ({3})，不更新相關資料 \r\n\r\n", DateTime.Now, rspData.TxnId, this.MenuID, txn.Status);
                            }
                        }
                    }
                    else
                    {
                        toUpdate = false;
                        this.labMemo.Text = @"<span style=""color:red"">注意：無法取得學雜費系統紀錄的交易資料，系統將不更新該繳費單狀態</span>";
                        helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) 讀取 {2} 交易資料 (CCardTxnDtlEntity) 發生錯誤，錯誤訊息：{3} \r\n\r\n", DateTime.Now, rspData.TxnId, this.MenuID, xmlResult.Message);
                    }
                }
                #endregion

                DateTime? authRespTime = rspData.GetAuthRespTime();

                #region 更新 EZPos 交易資料
                if (toUpdate)
                {
                    Expression where = new Expression(CCardTxnDtlEntity.Field.TxnId, rspData.TxnId)
                        .And(CCardTxnDtlEntity.Field.Rid, rspData.Lidm)
                        .And(CCardTxnDtlEntity.Field.Status, "1");

                    KeyValue[] fieldValues = null;
                    if (authRespTime.HasValue)
                    {
                        fieldValues = new KeyValue[] {
                            new KeyValue(CCardTxnDtlEntity.Field.Xid, rspData.Xid),
                            new KeyValue(CCardTxnDtlEntity.Field.TxnResult, txnResult),
                            new KeyValue(CCardTxnDtlEntity.Field.TxnAuthCode, rspData.AuthCode),
                            new KeyValue(CCardTxnDtlEntity.Field.TxnAuthDate, authRespTime.Value),
                            new KeyValue(CCardTxnDtlEntity.Field.TxnRemark, txnRemark),
                            new KeyValue(CCardTxnDtlEntity.Field.UpdateDate, DateTime.Now),
                            new KeyValue(CCardTxnDtlEntity.Field.TxnStatus, rspData.Status),
                            new KeyValue(CCardTxnDtlEntity.Field.Status, myStatus)
                        };
                    }
                    else
                    {
                        fieldValues = new KeyValue[] {
                            new KeyValue(CCardTxnDtlEntity.Field.Xid, rspData.Xid),
                            new KeyValue(CCardTxnDtlEntity.Field.TxnResult, txnResult),
                            new KeyValue(CCardTxnDtlEntity.Field.TxnAuthCode, rspData.AuthCode),
                            new KeyValue(CCardTxnDtlEntity.Field.TxnRemark, txnRemark),
                            new KeyValue(CCardTxnDtlEntity.Field.UpdateDate, DateTime.Now),
                            new KeyValue(CCardTxnDtlEntity.Field.TxnStatus, rspData.Status),
                            new KeyValue(CCardTxnDtlEntity.Field.Status, myStatus)
                        };
                    }

                    int count = 0;
                    XmlResult xmlResult = DataProxy.Current.UpdateFields<CCardTxnDtlEntity>(this.Page, where, fieldValues, out count);
                    if (xmlResult.IsSuccess)
                    {
                        if (count > 0)
                        {
                            helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) 更新 {2} 交易資料 (CCardTxnDtlEntity) 成功 \r\n\r\n", DateTime.Now, rspData.TxnId, this.MenuID);
                        }
                        else
                        {
                            helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) 更新 {2} 交易資料 (CCardTxnDtlEntity) 失敗，無任何資料被更新 \r\n\r\n", DateTime.Now, rspData.TxnId, this.MenuID);
                        }
                    }
                    else
                    {
                        helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] (TxnId={1}) 更新 {2} 交易資料 (CCardTxnDtlEntity) 發生錯誤，錯誤訊息：{3} \r\n\r\n", DateTime.Now, rspData.TxnId, this.MenuID, xmlResult.Message);
                    }
                }
                #endregion

                //[TODO] 不確定更新後是否會影響異業代收檔的預銷處理
                #region 更新繳費單狀態
                //如果授權成功，更新學生資料繳費明細檔(STUDENT_RECEIVE)的繳款方式 + 代收日
                //更新條件 1：代收類別 (由銷帳編號擷取) + 學年 + 學期 + '' + 代收費用別 + 學號 + 資料序號 + 銷帳編號 + 未繳 (+ 金額)
                //更新條件 2：代收類別 (由銷帳編號擷取) + 銷帳編號 + 學號 + 未繳 (+ 金額)
                //如果更新失敗，不要影響目前的流程
                if (toUpdate && myStatus == "2" && txn != null)
                {
                    string cancelNo = txn.Rid;
                    string receiveType = cancelNo.Substring(0, 4);

                    string yearId = null, termId = null, depId = "", receiveId = null;
                    int oldSeq = -1;

                    #region 取得 StudentReceive 資料的 PKEY
                    if (txn.ReceiveType == receiveType 
                        && !String.IsNullOrWhiteSpace(txn.YearId) 
                        && !String.IsNullOrWhiteSpace(txn.TermId) 
                        && txn.DepId != null 
                        && !String.IsNullOrWhiteSpace(txn.ReceiveId)
                        && txn.OldSeq.HasValue && txn.OldSeq.Value > -1)
                    {
                        yearId = txn.YearId.Trim();
                        termId = txn.TermId.Trim();
                        depId = txn.DepId;
                        receiveId = txn.ReceiveId.Trim();
                        oldSeq = txn.OldSeq.Value;
                    }
                    #endregion

                    Expression where = null;

                    if (oldSeq < 0)
                    {
                        where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                            .And(StudentReceiveEntity.Field.CancelNo, cancelNo)
                            .And(StudentReceiveEntity.Field.ReceiveAmount, txn.Amount.Value)
                            .And(StudentReceiveEntity.Field.StuId, txn.StudentNo)
                            .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty))
                            .And(new Expression(StudentReceiveEntity.Field.CancelDate, null).Or(StudentReceiveEntity.Field.CancelDate, String.Empty))
                            .And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));
                    }
                    else
                    {
                        where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                            .And(StudentReceiveEntity.Field.YearId, yearId)
                            .And(StudentReceiveEntity.Field.TermId, termId)
                            .And(StudentReceiveEntity.Field.DepId, depId)
                            .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                            .And(StudentReceiveEntity.Field.StuId, txn.StudentNo)
                            .And(StudentReceiveEntity.Field.OldSeq, oldSeq)
                            .And(StudentReceiveEntity.Field.CancelNo, cancelNo)
                            .And(StudentReceiveEntity.Field.ReceiveAmount, txn.Amount.Value)
                            .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty))
                            .And(new Expression(StudentReceiveEntity.Field.CancelDate, null).Or(StudentReceiveEntity.Field.CancelDate, String.Empty))
                            .And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));
                    }

                    DateTime receiveDateTime = authRespTime.HasValue ? authRespTime.Value : txn.CreateDate;

                    KeyValue[] fieldValues = new KeyValue[] {
                        new KeyValue(StudentReceiveEntity.Field.ReceiveWay, ChannelHelper.FISC_NC),  //財金國際信用卡
                        new KeyValue(StudentReceiveEntity.Field.ReceiveDate, Common.GetTWDate7(receiveDateTime)),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveTime, receiveDateTime.ToString("HHmmss"))
                    };

                    int count = 0;
                    XmlResult xmlResult = DataProxy.Current.UpdateFields<StudentReceiveEntity>(this.Page, where, fieldValues, out count);
                    if (xmlResult.IsSuccess)
                    {
                        helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] 更新 學生繳費資料 (StudentReceiveEntity) 成功 ({1}筆) \r\n\r\n", DateTime.Now, count);
                    }
                    else
                    {
                        helper.WriteLog("[{0:yyyy/MM/dd HH:mm:ss}] 更新 學生繳費資料 (StudentReceiveEntity) 發生錯誤，錯誤訊息：{1} \r\n\r\n", DateTime.Now, xmlResult.Message);
                    }
                }
                #endregion
            }
        }
    }
}