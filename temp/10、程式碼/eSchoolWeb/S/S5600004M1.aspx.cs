using System;
using System.Collections.Generic;
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
    /// 代收管道管理 - 代收代號
    /// </summary>
    public partial class S5600004M1 : PagingBasePage
    {
        #region Keep 頁面參數
        /// <summary>
        /// 編輯的管道代碼
        /// </summary>
        private string EditChannelId
        {
            get
            {
                return ViewState["EditChannelId"] as string;
            }
            set
            {
                ViewState["EditChannelId"] = value == null ? null : value.Trim();
            }
        }
        /// <summary>
        /// 編輯的管道名稱
        /// </summary>
        private string EditChannelName
        {
            get
            {
                return ViewState["EditChannelName"] as string;
            }
            set
            {
                ViewState["EditChannelName"] = value == null ? null : value.Trim();
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
            where.And(ChannelWayEntity.Field.ChannelId, this.EditChannelId);

            orderbys = null;
            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(ChannelWayEntity.Field.BarcodeId, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<ChannelWayEntity>(pagingInfo, ucPagings, this.gvResult);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    return;
                }
                #endregion

                #region 處理參數
                KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                if (QueryString == null || QueryString.Count == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    return;
                }

                this.EditChannelId = QueryString.TryGetValue("ChannelId", String.Empty);
                this.EditChannelName = QueryString.TryGetValue("ChannelName", String.Empty);

                if (String.IsNullOrEmpty(this.EditChannelId))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }
                #endregion

                this.InitialUI();
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            PagingInfo pagingInfo = new PagingInfo();
            XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            return xmlResult.IsSuccess;
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
            string[] args = argument.Split(new char[] { ',' }, StringSplitOptions.None);
            if (args.Length != 2)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }

            string channelId = args[0];
            string barcodeId = args[1];
            #endregion

            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("ChannelId", channelId);
                        QueryString.Add("BarcodeId", barcodeId);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("S5600004M2.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Delete:
                    #region 刪除資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Delete);
                        QueryString.Add("ChannelId", channelId);
                        QueryString.Add("BarcodeId", barcodeId);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("S5600004M2.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            ChannelWayEntity[] datas = this.gvResult.DataSource as ChannelWayEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            string channelName = GetChannelName(this.EditChannelId);
            foreach (GridViewRow row in this.gvResult.Rows)
            {
                ChannelWayEntity data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1}", data.ChannelId, data.BarcodeId);
                row.Cells[0].Text = channelName;
                row.Cells[2].Text = data.IncludePay.Trim() == "1" ? "包含" : "不包含";
                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                }

                MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
                if (ccbtnDelete != null)
                {
                    ccbtnDelete.CommandArgument = argument;
                }
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
            QueryString.Add("ChannelId", this.EditChannelId);
            QueryString.Add("BarcodeId", string.Empty);
            Session["QueryString"] = QueryString;

            Server.Transfer("S5600004M2.aspx");
        }

        private string GetChannelName(string channelid)
        {
            if (string.IsNullOrEmpty(channelid))
            {
                return string.Empty;
            }

            Expression where = new Expression();
            where.And(ChannelSetEntity.Field.ChannelId, channelid);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(ChannelSetEntity.Field.ChannelId, OrderByEnum.Asc);

            ChannelSetEntity data = null;
            XmlResult xmlResult = DataProxy.Current.SelectFirst<ChannelSetEntity>(this, where, orderbys, out data);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return string.Empty;
            }

            return data.ChannelName;
        }
    }
}