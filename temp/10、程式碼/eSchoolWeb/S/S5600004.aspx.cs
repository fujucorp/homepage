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
    /// 代收管道管理
    /// </summary>
    public partial class S5600004 : PagingBasePage
    {
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
            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(ChannelSetEntity.Field.ChannelId, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<ChannelSetEntity>(pagingInfo, ucPagings, this.gvResult);
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
                this.InitialUI();
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            PagingInfo pagingInfo = new PagingInfo(20, 0, 0);
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
            string channelName = args[1];
            #endregion

            switch (e.CommandName)
            {
                case "ChannelWay":
                    #region 繳款方式 
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Query);
                        QueryString.Add("ChannelId", channelId);
                        QueryString.Add("ChannelName", channelName);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("S5600004M1.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("ChannelId", channelId);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("S5600004M.aspx"));
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
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("S5600004M.aspx"));
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
            ChannelSetEntity[] datas = this.gvResult.DataSource as ChannelSetEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                ChannelSetEntity data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1}", data.ChannelId, data.ChannelName);

                bool hasChannelWay = data.ProcessFee == "Y";

                LinkButton lbtnChannelWay = row.FindControl("lbtnChannelWay") as LinkButton;
                if (lbtnChannelWay != null)
                {
                    lbtnChannelWay.CommandArgument = argument;
                    lbtnChannelWay.Visible = hasChannelWay;
                }

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
            QueryString.Add("ChannelId", string.Empty);
            Session["QueryString"] = QueryString;

            Server.Transfer("S5600004M.aspx");
        }

    }
}