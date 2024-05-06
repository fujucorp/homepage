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
    /// 手續費管理維護
    /// </summary>
    public partial class S5600011 : BasePage
    {
        #region Keep 頁面參數
        /// <summary>
        /// 編輯的業務別碼參數
        /// </summary>
        private string EditReceiveType
        {
            get
            {
                return ViewState["EditReceiveType"] as string;
            }
            set
            {
                ViewState["EditReceiveType"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.EditReceiveType = ucFilter1.SelectedReceiveType;
                this.InitialUI();
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
            return this.GetDataAndBind();
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetDataAndBind()
        {
            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
                return false;
            }
            #endregion

            Expression where = new Expression();
            where.And(ReceiveChannelView.Field.ReceiveType, this.EditReceiveType);

            #region 排序條件
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(2);
            orderbys.Add(ReceiveChannelView.Field.BarcodeId, OrderByEnum.Asc);
            orderbys.Add(ReceiveChannelView.Field.ChannelId, OrderByEnum.Asc);
            #endregion

            ReceiveChannelView[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<ReceiveChannelView>(this, where, orderbys, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }

            this.gvResult.DataSource = datas;
            this.gvResult.DataBind();
            return xmlResult.IsSuccess;
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
            QueryString.Add("ReceiveType", this.EditReceiveType);
            QueryString.Add("ReceiveTypeName", ucFilter1.SelectedReceiveTypeName);
            Session["QueryString"] = QueryString;

            Server.Transfer("S5600011M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            ReceiveChannelView[] datas = this.gvResult.DataSource as ReceiveChannelView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                ReceiveChannelView data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1},{2},{3}", data.ReceiveType, data.ChannelId, data.BarcodeId, ucFilter1.SelectedReceiveTypeName);

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

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            #region 處理資料參數
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("取無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            string[] args = argument.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 4)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            string editUrl = "S5600011M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("ReceiveType", args[0]);
                        QueryString.Add("ChannelId", args[1]);
                        QueryString.Add("BarcodeId", args[2]);
                        QueryString.Add("ReceiveTypeName", args[3]);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Delete:
                    #region 刪除資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Delete);
                        QueryString.Add("ReceiveType", args[0]);
                        QueryString.Add("ChannelId", args[1]);
                        QueryString.Add("BarcodeId", args[2]);
                        QueryString.Add("ReceiveTypeName", args[3]);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        protected void ucFilter1_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            this.EditReceiveType = ucFilter1.SelectedReceiveType;
            this.InitialUI();
        }
        
    }
}