using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using Fuju.Configuration;

using Entities;
using Helpers;

namespace eSchoolWeb.D
{
    /// <summary>
    /// 查詢上傳中國信託繳費單資料
    /// </summary>
    public partial class D1600002 : PagingBasePage
    {
        #region Property
        /// <summary>
        /// 儲存查詢的商家代號
        /// </summary>
        private string QueryReceiveType
        {
            get
            {
                return ViewState["QueryReceiveType"] as string;
            }
            set
            {
                ViewState["QueryReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的學號
        /// </summary>
        private string QueryStuId
        {
            get
            {
                return ViewState["QueryStuId"] as string;
            }
            set
            {
                ViewState["QueryStuId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的銷帳編號
        /// </summary>
        private string QueryCancelNo
        {
            get
            {
                return ViewState["QueryCancelNo"] as string;
            }
            set
            {
                ViewState["QueryCancelNo"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的繳款期限
        /// </summary>
        private DateTime? QueryPayDueDate
        {
            get
            {
                object value = ViewState["QueryPayDueDate"];
                if (value is DateTime)
                {
                    return (DateTime)value;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["QueryPayDueDate"] = value;
            }
        }
        #endregion

        #region 實作 PagingBasePage's 抽象方法
        /// <summary>
        /// 取得頁面中的分頁控制項陣列
        /// </summary>
        /// <returns>傳回分頁控制項陣列或 null</returns>
        protected override Paging[] GetPagingControls()
        {
            return new Paging[] { this.ucPaging1, this.ucPaging2 };
        }

        /// <summary>
        /// 取得查詢條件與排序方法
        /// </summary>
        /// <param name="where">成功則傳回查詢條件，否則傳回 null</param>
        /// <param name="orderbys">成功則傳回查詢條件，否則傳回 null</param>
        /// <returns>傳回處理結果</returns>
        protected override XmlResult GetWhereAndOrderBys(out Expression where, out KeyValueList<OrderByEnum> orderbys)
        {
            string qStuId = this.QueryStuId;
            string qCancelNo = this.QueryCancelNo;
            DateTime? qPayDueDate = this.QueryPayDueDate;

            where = new Expression(QueueCTCBEntity.Field.ReceiveType, this.QueryReceiveType);
            if (!String.IsNullOrEmpty(qStuId))
            {
                where.And(QueueCTCBEntity.Field.StuId, qStuId);
            }
            if (!String.IsNullOrEmpty(qCancelNo))
            {
                where.And(QueueCTCBEntity.Field.CancelNo, qCancelNo);
            }
            if (qPayDueDate != null)
            {
                where.And(QueueCTCBEntity.Field.PayDueDate, Common.GetTWDate7(qPayDueDate.Value));
            }

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(QueueCTCBEntity.Field.CrtDate, OrderByEnum.Asc);

            return new XmlResult(true);
        }

        /// <summary>
        /// 呼叫 QueryDataAndBind 方法
        /// </summary>
        /// <param name="pagingInfo">指定分頁訊息</param>
        /// <returns>傳回處理結果</returns>
        protected override XmlResult CallQueryDataAndBind(PagingInfo pagingInfo)
        {
            Paging[] ucPagings = this.GetPagingControls();
            XmlResult xmlResult = base.QueryDataAndBind<QueueCTCBEntity>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }

            this.gvResult.Visible = true;
            //因為有新增與回上一頁的按鈕，所以無資料時不適合把 gvResult 隱藏
            //this.divResult.Visible = this.gvResult.Rows.Count > 0;
            bool showPaging = this.gvResult.Rows.Count > 0;
            foreach (Paging ucPaging in ucPagings)
            {
                ucPaging.Visible = showPaging;
            }

            return xmlResult;
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region 處理商家代號
            string receiveType = null;
            string yearId = null;
            string termId = null;
            string depId = null;
            string receiveId = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearId, out termId, out depId, out receiveId)
                || String.IsNullOrEmpty(receiveType))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得商家代號參數");
                this.ShowSystemMessage(msg);
                return false;
            }

            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
            if (xmlResult.IsSuccess)
            {
                this.QueryReceiveType = receiveType;
            }
            #endregion

            #region 檢查商家代號授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(this.QueryReceiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該商家代號");
                this.ccbtnQuery.Visible = false;
                return false;
            }
            #endregion

            this.gvResult.Visible = false;
            return true;
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>成功傳回 true，否則傳回 false</returns>
        private bool GetAndKeepQueryCondition()
        {
            #region 學號
            string qStuId = this.tbxStuId.Text.Trim();
            #endregion

            #region 虛擬帳號
            string qCancelNo = this.tbxCancelNo.Text.Trim();
            #endregion

            #region 繳款期限
            DateTime? qPayDueDate = null;
            string payDueDate = this.tbxPayDueDate.Text.Trim();
            if (!String.IsNullOrEmpty(payDueDate))
            {
                DateTime date;
                if (DateTime.TryParse(payDueDate, out date))
                {
                    qPayDueDate = date;
                }
                else
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「繳款期限」不是合法的日期格式");
                    this.ShowJsAlert(msg);
                    return false;
                }
            }
            #endregion

            this.QueryStuId = qStuId;
            this.QueryCancelNo = qCancelNo;
            this.QueryPayDueDate = qPayDueDate;

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void ccbtnQuery_Click(object sender, EventArgs e)
        {
            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無查詢權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            if (this.GetAndKeepQueryCondition())
            {
                PagingInfo pagingInfo = new PagingInfo(10, 0, 0);
                XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);

                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                }
            }
            else
            {
                this.gvResult.DataSource = null;
                this.gvResult.DataBind();
                Paging[] ucPagings = this.GetPagingControls();
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = false;
                }
                //this.gvResult.Visible = false;
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {

        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}