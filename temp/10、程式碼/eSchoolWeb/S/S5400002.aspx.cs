using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.S
{
    /// <summary>
    /// 交易紀錄查詢
    /// </summary>
    public partial class S5400002 : PagingBasePage
    {
        #region Property
        /// <summary>
        /// 儲存查詢的業務別碼代碼
        /// </summary>
        private string QueryUserUnitId
        {
            get
            {
                return ViewState["QueryUserUnitId"] as string;
            }
            set
            {
                ViewState["QueryUserUnitId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的使用者帳號
        /// </summary>
        private string QueryUserId
        {
            get
            {
                return ViewState["QueryUserId"] as string;
            }
            set
            {
                ViewState["QueryUserId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的交易日期區間起日
        /// </summary>
        private string QuerySDate
        {
            get
            {
                return ViewState["QuerySDate"] as string;
            }
            set
            {
                ViewState["QuerySDate"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的交易日期區間迄日
        /// </summary>
        private string QueryEDate
        {
            get
            {
                return ViewState["QueryEDate"] as string;
            }
            set
            {
                ViewState["QueryEDate"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的功能
        /// </summary>
        private string QueryFunctionId
        {
            get
            {
                return ViewState["QueryFunctionId"] as string;
            }
            set
            {
                ViewState["QueryFunctionId"] = value == null ? null : value.Trim();
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
            where = null;
            orderbys = null;

            where = new Expression();

            #region 單位代碼 條件
            if (!String.IsNullOrEmpty(this.QueryUserUnitId))
            {
                where.And(LogTableView.Field.ReceiveType, this.QueryUserUnitId);
            }
            #endregion

            #region 使用者帳號 條件
            if (!String.IsNullOrEmpty(this.QueryUserId))
            {
                where.And(LogTableView.Field.UserId, this.QueryUserId);
            }
            #endregion

            #region 日期區間 條件
            if (!String.IsNullOrEmpty(this.QuerySDate))
            {
                where.And(LogTableView.Field.LogDate, RelationEnum.GreaterEqual, this.QuerySDate);
            }
            if (!String.IsNullOrEmpty(this.QueryEDate))
            {
                where.And(LogTableView.Field.LogDate, RelationEnum.LessEqual, this.QueryEDate);
            }
            #endregion

            #region 功能 條件
            if (!String.IsNullOrEmpty(this.QueryFunctionId))
            {
                where.And(LogTableView.Field.FunctionId, this.QueryFunctionId);
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(2);
            orderbys.Add(LogTableView.Field.LogDate, OrderByEnum.Desc);
            orderbys.Add(LogTableView.Field.LogTime, OrderByEnum.Desc);
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
            XmlResult xmlResult = base.QueryDataAndBind<LogTableView>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                //[TODO] 變動顯示訊息怎麼多語系
                this.ShowSystemMessage(this.GetLocalized("查詢資料失敗") + "，" + xmlResult.Message);
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

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region ddlFunction
            {
                this.GetAndBindFunctionOption();
            }
            #endregion

            #region 查詢結果初始化
            {
                //因為有新增與回上一頁的按鈕，所以不適合把 divResult 隱藏
                //改為隱藏分頁按鈕，並結繫 null
                //this.divResult.Visible = false;
                this.gvResult.DataSource = null;
                this.gvResult.DataBind();
                Paging[] ucPagings = this.GetPagingControls();
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = false;
                }
            }
            #endregion

            return true;
        }

        private void GetAndBindFunctionOption()
        {
            CodeTextList items = null;

            Expression where = new Expression();
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(FuncMenuView2.Field.FuncId, OrderByEnum.Asc);

            FuncMenuView2[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<FuncMenuView2>(this.Page, where, orderbys, out datas);
            if (xmlResult.IsSuccess)
            {
                #region 加入非 FuncMenuView2 的項目
                CodeText[] others = new CodeText[2] {
                    new CodeText("LOGON", "使用者登入"), new CodeText("CHECK_LOGON", "驗證登入狀態")
                };
                items = new CodeTextList((datas == null ? 0 : datas.Length) + others.Length);
                items.AddRange(others);
                #endregion

                if (datas != null && datas.Length > 0)
                {
                    foreach (FuncMenuView2 data in datas)
                    {
                        items.Add(data.FuncId, data.FuncName);
                    }
                }
            }
            else
            {
                this.ShowErrorMessage(xmlResult.Code, "無法取得功能選項資料");
            }

            WebHelper.SetDropDownListItems(this.ddlFunction, DefaultItem.Kind.All, false, items, true, true, 0, null);
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>傳回處理結果</returns>
        private bool GetAndKeepQueryCondition()
        {
            #region 查詢的代收日區間起日
            DateTime sDate = DateTime.MinValue;
            string qSDate = this.tbxSDate.Text.Trim();
            if (!String.IsNullOrEmpty(qSDate))
            {
                if (DateTime.TryParse(qSDate, out sDate))
                {
                    qSDate = Common.GetTWDate7(sDate);
                }
                else
                {
                    string msg = this.GetLocalized("日期區間的起日不是有效的日期格式");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            #region 查詢的代收日區間迄日
            DateTime eDate = DateTime.MinValue;
            string qEDate = this.tbxEDate.Text.Trim();
            if (!String.IsNullOrEmpty(qEDate))
            {
                if (DateTime.TryParse(qEDate, out eDate))
                {
                    qEDate = Common.GetTWDate7(eDate);
                }
                else
                {
                    string msg = this.GetLocalized("日期區間的迄日不是有效的日期格式");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            if (!String.IsNullOrEmpty(qSDate) && !String.IsNullOrEmpty(qEDate) && sDate > eDate)
            {
                string msg = this.GetLocalized("日期區間的起日不可以大於迄日");
                this.ShowSystemMessage(msg);
                return false;
            }

            //查詢的單位代碼
            this.QueryUserUnitId = this.tbxUserUnitId.Text.Trim();

            //查詢的使用者帳號
            this.QueryUserId = tbxUserId.Text.Trim();

            //查詢的日期區間
            this.QuerySDate = qSDate;
            this.QueryEDate = qEDate;

            //查詢的功能
            this.QueryFunctionId = this.ddlFunction.SelectedValue;

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
                    //[TODO] 變動顯示訊息怎麼多語系
                    this.ShowSystemMessage(xmlResult.Message);
                }
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            LogTableView[] datas = this.gvResult.DataSource as LogTableView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                LogTableView data = datas[row.RowIndex];

                row.Cells[0].Text = RoleCodeTexts.GetText(data.Role);
                row.Cells[1].Text = String.Format("{0}-{1}", data.ReceiveType, data.UserId);
                row.Cells[2].Text = String.Format("{0}-{1}", data.LogDate, data.LogTime);
                if (String.IsNullOrEmpty(data.FuncName))
                {
                    switch (data.FunctionId)
                    {
                        case "LOGON":
                            row.Cells[3].Text = "使用者登入";
                            break;
                        case "CHECK_LOGON":
                            row.Cells[3].Text = "驗證登入狀態";
                            break;
                    }
                }
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
    }
}