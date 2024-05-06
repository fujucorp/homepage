using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.S
{
    /// <summary>
    /// 首頁功能日誌查詢
    /// </summary>
    public partial class S5400015 : PagingBasePage
    {
        #region Const
        private const int MaxDays = 30;
        #endregion

        #region Property
        /// <summary>
        /// 儲存查詢的日誌日期區間起日
        /// </summary>
        private DateTime? QueryLogTimeSDate
        {
            get
            {
                object value = ViewState["QueryLogTimeSDate"];
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
                ViewState["QueryLogTimeSDate"] = value;
            }
        }

        /// <summary>
        /// 儲存查詢的日誌日期區間迄日
        /// </summary>
        private DateTime? QueryLogTimeEDate
        {
            get
            {
                object value = ViewState["QueryLogTimeEDate"];
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
                ViewState["QueryLogTimeEDate"] = value;
            }
        }

        /// <summary>
        /// 儲存查詢的功能
        /// </summary>
        private string QueryRequestId
        {
            get
            {
                return ViewState["QueryRequestId"] as string;
            }
            set
            {
                ViewState["QueryRequestId"] = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的操作
        /// </summary>
        private string QueryRequestKind
        {
            get
            {
                return ViewState["QueryRequestKind"] as string;
            }
            set
            {
                ViewState["QueryRequestKind"] = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的用戶 IP
        /// </summary>
        private string QueryClientIP
        {
            get
            {
                return ViewState["QueryClientIP"] as string;
            }
            set
            {
                ViewState["QueryClientIP"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的網站主機
        /// </summary>
        private string QueryWebMachine
        {
            get
            {
                return ViewState["QueryWebMachine"] as string;
            }
            set
            {
                ViewState["QueryWebMachine"] = value == null ? null : value.Trim();
            }
        }

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
        /// 儲存查詢的虛擬帳號
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
            where = new Expression();
            orderbys = null;

            #region 日誌日期區間 條件
            {
                //日誌日期區間為必要條件，沒條件就讓系統查無資料
                DateTime? qSDate = this.QueryLogTimeSDate;
                if (qSDate.HasValue)
                {
                    where.And(WebLogView.Field.LogTime, RelationEnum.GreaterEqual, qSDate.Value);
                }
                else
                {
                    where.And(WebLogView.Field.LogTime, RelationEnum.Equal, null);
                }

                DateTime? qEDate = this.QueryLogTimeEDate;
                if (qEDate.HasValue)
                {
                    //因為 LogTime 包含時間所以要小於迄日的隔日
                    where.And(WebLogView.Field.LogTime, RelationEnum.Less, qEDate.Value.AddDays(1));
                }
                else
                {
                    where.And(WebLogView.Field.LogTime, RelationEnum.Equal, null);
                }
            }
            #endregion

            #region 功能 條件
            {
                string qRequestId = this.QueryRequestId;
                if (!String.IsNullOrEmpty(qRequestId))
                {
                    where.And(WebLogView.Field.RequestId, RelationEnum.Equal, qRequestId);
                }
            }
            #endregion

            #region 操作 條件
            {
                string qRequestKind = this.QueryRequestKind;
                if (!String.IsNullOrEmpty(qRequestKind))
                {
                    where.And(WebLogView.Field.RequestKind, RelationEnum.Equal, qRequestKind);
                }
            }
            #endregion

            #region 用戶 IP 條件
            {
                string qClientIP = this.QueryClientIP;
                if (!String.IsNullOrEmpty(qClientIP))
                {
                    where.And(WebLogView.Field.ClientIp, RelationEnum.Equal, qClientIP);
                }
            }
            #endregion

            #region 網站主機 條件
            {
                string qWebMachine = this.QueryWebMachine;
                if (!String.IsNullOrEmpty(qWebMachine))
                {
                    where.And(WebLogView.Field.WebMachine, RelationEnum.Equal, qWebMachine);
                }
            }
            #endregion

            #region 商家代號 條件
            {
                string qReceiveType = this.QueryReceiveType;
                if (!String.IsNullOrEmpty(qReceiveType))
                {
                    where.And(WebLogView.Field.IndexReceiveType, RelationEnum.Equal, qReceiveType);
                }
            }
            #endregion

            #region 虛擬帳號 條件
            {
                string qCancelNo = this.QueryCancelNo;
                if (!String.IsNullOrEmpty(qCancelNo))
                {
                    where.And(WebLogView.Field.IndexCancelNo, RelationEnum.Equal, qCancelNo);
                }
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(WebLogView.Field.LogTime, OrderByEnum.Desc);
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
            XmlResult xmlResult = base.QueryDataAndBind<WebLogView>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }

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

        #region Private Mehod
        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region 日誌日期區間
            {
                DateTime now = DateTime.Now;
                this.tbxQSDate.Text = now.AddDays((MaxDays - 1) * -1).ToString("yyyy/MM/dd");  //間隔要比天數少 1
                this.tbxQEDate.Text = now.ToString("yyyy/MM/dd");
            }
            #endregion

            WebLogRequestList list = new WebLogRequestList();

            #region 功能
            {
                List<CodeText> items = list.GetAllIdNames();
                string selectedValue = items[0].Code;
                WebHelper.SetDropDownListItems(this.ddlQRequestId, DefaultItem.Kind.All, false, items, false, true, 0, selectedValue);
            }
            #endregion

            #region 網頁參數
            {
                this.ChangeRequestId(list);
            }
            #endregion

            #region 查詢結果初始化
            {
                ////因為有新增與回上一頁的按鈕，所以不適合把 divResult 隱藏
                ////改為隱藏分頁按鈕，並結繫 null
                ////this.divResult.Visible = false;
                //this.gvResult.DataSource = null;
                //this.gvResult.DataBind();
                Paging[] ucPagings = this.GetPagingControls();
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = false;
                }
            }
            #endregion
            return true;
        }

        private void ChangeRequestId(WebLogRequestList list = null)
        {
            if (list == null)
            {
                list = new WebLogRequestList();
            }

            WebLogRequestItem item = list.Find(this.ddlQRequestId.SelectedValue);
            string[] argKinds = null;
            if (item == null)
            {
                argKinds = new string[0];

                #region 操作
                {
                    WebHelper.SetDropDownListItems(this.ddlQRequestKind, DefaultItem.Kind.All, false, null, false, false, 0, null);
                }
                #endregion
            }
            else
            {
                argKinds = item.ArgumentKinds ?? new string[0];

                #region 操作
                WebHelper.SetDropDownListItems(this.ddlQRequestKind, DefaultItem.Kind.All, false, item.RequestKinds, false, true, 0, null);
                #endregion
            }

            #region 網頁參數
            if (argKinds.Length > 0)
            {
                this.trQRequestArgs.Visible = true;

                #region 商家代號
                this.divQReceiveType.Visible = (Array.IndexOf(argKinds, "商家代號") > -1);
                #endregion

                #region 虛擬帳號
                this.divQCancelNo.Visible = (Array.IndexOf(argKinds, "虛擬帳號") > -1);
                #endregion
            }
            else
            {
                this.trQRequestArgs.Visible = false;
            }
            #endregion
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>傳回處理結果</returns>
        private bool GetAndKeepQueryCondition()
        {
            #region 查詢的日誌日期區間
            {
                #region 起日
                DateTime qSDate;
                {
                    string sDate = this.tbxQSDate.Text.Trim();
                    if (String.IsNullOrEmpty(sDate))
                    {
                        this.ShowMustInputAlert("日誌日期區間的起日");
                        return false;
                    }
                    if (!DateTime.TryParse(sDate, out qSDate))
                    {
                        string msg = this.GetLocalized("日誌日期區間的起日不是有效的日期格式");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                }
                #endregion

                #region 迄日
                DateTime qEDate;
                {
                    string sDate = this.tbxQEDate.Text.Trim();
                    if (String.IsNullOrEmpty(sDate))
                    {
                        this.ShowMustInputAlert("日誌日期區間的迄日");
                        return false;
                    }
                    if (!DateTime.TryParse(sDate, out qEDate))
                    {
                        string msg = this.GetLocalized("日誌日期區間的迄日不是有效的日期格式");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                }
                #endregion

                if (qSDate > qEDate)
                {
                    string msg = this.GetLocalized("日誌日期區間的起日不可以大於迄日");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                if (qSDate < new DateTime(2019, 1, 1))
                {
                    string msg = this.GetLocalized("日誌日期區間的起日不可以小於 2019/01/01");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                if (qEDate > DateTime.Today)
                {
                    string msg = this.GetLocalized("日誌日期區間的迄日不可以大於今天");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                if (qSDate.AddDays(MaxDays - 1) < qEDate)  //間隔要比天數少 1
                {
                    string msg = this.GetLocalized(String.Format("日誌日期區間不可超過 {0} 天", MaxDays));
                    this.ShowSystemMessage(msg);
                    return false;
                }

                this.QueryLogTimeSDate = qSDate;
                this.QueryLogTimeEDate = qEDate;
            }
            #endregion

            #region 功能
            string qRequestId = this.QueryRequestId = this.ddlQRequestId.SelectedValue;
            #endregion

            #region 操作
            if (String.IsNullOrEmpty(qRequestId))
            {
                this.QueryRequestKind = null;
            }
            else
            {
                this.QueryRequestKind = this.ddlQRequestKind.SelectedValue;
            }
            #endregion

            #region 用戶 IP
            this.QueryClientIP = this.tbxQClientIP.Text.Trim();
            #endregion

            #region 網站主機
            this.QueryWebMachine = this.tbxQWebMachine.Text.Trim();
            #endregion

            #region 網頁參數
            this.QueryReceiveType = null;
            this.QueryCancelNo = null;
            if (!String.IsNullOrEmpty(qRequestId) && this.trQRequestArgs.Visible)
            {
                #region 商家代號
                if (this.divQReceiveType.Visible)
                {
                    this.QueryReceiveType = this.tbxQReceiveType.Text.Trim();
                }
                #endregion

                #region 虛擬帳號
                if (this.divQReceiveType.Visible)
                {
                    this.QueryCancelNo = this.tbxQConcelNo.Text.Trim();
                }
                #endregion
            }
            #endregion

            return true;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查查詢權限
                if (!this.HasQueryAuth())
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("無查詢權限");
                    this.ShowJsAlert(msg);
                    return;
                }
                #endregion
            }
        }

        protected void ddlQRequestId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeRequestId();
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
                    //[TODO] 變動顯示訊息怎麼多語系
                    this.ShowSystemMessage(xmlResult.Message);
                }
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            WebLogView[] datas = this.gvResult.DataSource as WebLogView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            string action = WebHelper.GetControlLocalizedByResourceKey("Detail", "明細");

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                WebLogView data = datas[row.RowIndex];

                //資料參數
                string argument = data.TaskNo;

                #region 功能說明 & 網站主機
                Literal litFuncInfo = row.FindControl("litFuncInfo") as Literal;
                if (litFuncInfo != null)
                {
                    litFuncInfo.Text = String.Format("{0}\r\n{1}", Server.HtmlEncode(data.RequestDesc), Server.HtmlEncode(data.WebMachine));
                }
                #endregion

                #region 日誌時間 & 狀態代碼
                Literal litTimeInfo = row.FindControl("litTimeInfo") as Literal;
                if (litTimeInfo != null)
                {
                    litTimeInfo.Text = String.Format("{0:yyyy/MM/dd HH:mm:ss}\r\n{1}", data.LogTime, Server.HtmlEncode(data.StatusCode));
                }
                #endregion

                #region 用戶帳號 & 用戶 IP
                Literal litUserInfo = row.FindControl("litUserInfo") as Literal;
                if (litUserInfo != null)
                {
                    string unitName = data.UserUnitName;
                    if (String.IsNullOrEmpty(unitName))
                    {
                        litUserInfo.Text = String.Format("{0}\r\n{1}", Server.HtmlEncode(data.UserLoginId), Server.HtmlEncode(data.ClientIp));
                    }
                    else
                    {
                        litUserInfo.Text = String.Format("{0}-{1}\r\n{2}", Server.HtmlEncode(data.UserLoginId), Server.HtmlEncode(unitName), Server.HtmlEncode(data.ClientIp));
                    }
                }
                #endregion

                #region 日誌時間 & 狀態代碼
                Literal litRequestArgs = row.FindControl("litRequestArgs") as Literal;
                if (litRequestArgs != null)
                {
                    litRequestArgs.Text = Server.HtmlEncode(data.RequestArgs);
                }
                #endregion

                LinkButton lbtnDetail = row.FindControl("lbtnDetail") as LinkButton;
                if (lbtnDetail != null)
                {
                    lbtnDetail.Text = action;
                    lbtnDetail.CommandArgument = argument;
                }
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無維護權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            #region 處理資料參數
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            string viewPage = "S5400015D.aspx";
            switch (e.CommandName)
            {
                case "Detail":
                    #region 明細
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.View);
                        QueryString.Add("TaskNo", argument);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(viewPage));
                        #endregion
                    }
                    #endregion
                    break;
            }
        }
    }
}