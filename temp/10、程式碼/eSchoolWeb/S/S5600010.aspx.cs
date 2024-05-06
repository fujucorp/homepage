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
using Helpers;

namespace eSchoolWeb.S
{
    /// <summary>
    /// 繳費單模板管理 (行員專用)
    /// </summary>
    public partial class S5600010 : PagingBasePage
    {
        #region Property
        /// <summary>
        /// 儲存是否只查詢公版
        /// </summary>
        private bool QueryPublicOnly
        {
            get
            {
                object value = ViewState["QueryPublicOnly"];
                if (value is bool)
                {
                    return (bool)value;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["QueryPublicOnly"] = value;
            }
        }

        /// <summary>
        /// 儲存查詢的統一編號
        /// </summary>
        private string QuerySchIdenty
        {
            get
            {
                return ViewState["QuerySchIdenty"] as string;
            }
            set
            {
                ViewState["QuerySchIdenty"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的業務別碼代碼
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

            LogonUser logonUser = this.GetLogonUser();
            if (logonUser.IsBankManager)
            {
                where = new Expression();
            }
            else if (logonUser.IsBankUser)
            {
                Expression w1 = new Expression(BillFormView.Field.SchBankId, logonUser.BankId);
                w1.Or(BillFormView.Field.BillFormEdition, BillFormEditionCodeTexts.PUBLIC);

                where = new Expression().And(w1);
            }
            else
            {
                return new XmlResult(false, "無權限", ErrorCode.S_NO_AUTHORIZE, null);
            }

            if (this.QueryPublicOnly)
            {
                where.And(BillFormView.Field.BillFormEdition, BillFormEditionCodeTexts.PUBLIC);
            }

            string schIdenty = this.QuerySchIdenty;
            if (!string.IsNullOrEmpty(schIdenty))
            {
                where.And(BillFormView.Field.SchIdenty, RelationEnum.Like, "%" + schIdenty + "%");
            }

            string receiveType = this.QueryReceiveType;
            if (!string.IsNullOrEmpty(receiveType))
            {
                where.And(BillFormView.Field.ReceiveType, RelationEnum.Like, "%" + receiveType + "%");
            }

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(BillFormView.Field.BillFormId, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<BillFormView>(pagingInfo, ucPagings, this.gvResult);
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
            LogonUser logonUser = this.GetLogonUser();
            if (!logonUser.IsBankUser)
            {
                string msg = this.GetLocalized("此功能行員專用，您無權限");
                this.ShowJsAlert(msg);
                return false;
            }

            Paging[] ucPagings = this.GetPagingControls();
            foreach (Paging ucPaging in ucPagings)
            {
                ucPaging.Visible = false;
            }

            return true;
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus
        /// </summary>
        /// <returns>傳回處理結果</returns>
        private bool GetAndKeepQueryCondition()
        {
            this.QueryPublicOnly = this.chkPublicOnly.Checked;

            this.QuerySchIdenty = this.tbxSchIdenty.Text.Trim();

            this.QueryReceiveType = this.tbxReceiveType.Text.Trim();

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bool isOK = this.InitialUI();
                this.ccbtnQuery.Visible = isOK;
                this.ccbtnInsert.Visible = isOK;
            }
        }

        protected void ccbtnInsert_Click(object sender, EventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("Action", ActionMode.Insert);
            Session["QueryString"] = QueryString;

            Server.Transfer("S5600010M.aspx");
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
                    string action = ActionMode.GetActionLocalized(ActionMode.Query);
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            BillFormView[] datas = this.gvResult.DataSource as BillFormView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            bool isBankManager = this.GetLogonUser().IsBankManager;
            string txtView = this.GetLocalized("預覽");

            decimal[] nonDeleteIds = new decimal[] { 1, 2, 3 };    //二聯式繳費單, 三聯式繳費單, 收據

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                BillFormView data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0}", data.BillFormId);
                bool deleteable = !nonDeleteIds.Contains(data.BillFormId) && (isBankManager || data.BillFormEdition == BillFormEditionCodeTexts.PRIVATE);
                bool modifyable = (isBankManager || data.BillFormEdition == BillFormEditionCodeTexts.PRIVATE);

                row.Cells[1].Text = BillFormEditionCodeTexts.GetText(data.BillFormEdition);

                LinkButton lbtnView = row.FindControl("lbtnView") as LinkButton;
                if (lbtnView != null)
                {
                    lbtnView.Text = txtView;
                    lbtnView.CommandArgument = argument;
                }

                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                    ccbtnModify.Visible = modifyable;
                }

                MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
                if (ccbtnDelete != null)
                {
                    ccbtnDelete.CommandArgument = argument;
                    ccbtnDelete.Visible = deleteable;
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
            int billFormId = 0;
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument) || !Int32.TryParse(argument, out billFormId) || billFormId < 1)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            switch (e.CommandName)
            {
                case "View":
                    #region 預覽
                    {
                        #region [MDY:20210521] 原碼修正
                        Response.Redirect(WebHelper.GenRNUrl(string.Format("~/api/DownloadHandler.ashx?methodName={0}&Id={1}&fileExtName={2}", DownloadFileMethodCode.BILLFORM, billFormId, "PDF")));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("BillFormId", billFormId.ToString());
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("S5600010M.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Delete:
                    #region 刪除資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Delete);
                        QueryString.Add("BillFormId", billFormId.ToString());
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("S5600010M.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }
    }
}