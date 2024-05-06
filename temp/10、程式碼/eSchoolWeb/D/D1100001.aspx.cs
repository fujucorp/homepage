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

namespace eSchoolWeb.D
{
    /// <summary>
    /// 學期代碼
    /// </summary>
    public partial class D1100001 : BasePage
    {
        #region Property
        /// <summary>
        /// 儲存業務別碼代碼的查詢條件
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
        /// 儲存學年代碼的查詢條件
        /// </summary>
        private string QueryYearID
        {
            get
            {
                return ViewState["QueryYearID"] as string;
            }
            set
            {
                ViewState["QueryYearID"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
            string receiveType = null;
            string yearID = null;
            string termID = null;
            string depID = null;
            string ReceiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out ReceiveID)
                || String.IsNullOrEmpty(receiveType)
                || (this.ucFilter1.YearVisible && String.IsNullOrEmpty(yearID))
                || (this.ucFilter1.TermVisible && String.IsNullOrEmpty(termID)))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
                this.ShowSystemMessage(msg);
                return false;
            }

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                return false;
            }
            #endregion

            this.QueryReceiveType = receiveType;
            this.QueryYearID = yearID;

            return this.GetDataAndBind(receiveType, yearID);
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetDataAndBind(string qReceiveType, string qYearID)
        {
            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
                return false;
            }
            #endregion

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(qReceiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                return false;
            }
            #endregion

            #region 查詢條件
            Expression where = new Expression();
            where
                .And(TermListEntity.Field.ReceiveType, qReceiveType)
                .And(TermListEntity.Field.YearId, qYearID);
            #endregion

            #region 排序條件
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(TermListEntity.Field.TermId, OrderByEnum.Asc);
            #endregion

            TermListEntity[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<TermListEntity>(this, where, orderbys, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            //else if (datas == null || datas.Length == 0)
            //{
            //    this.ShowErrorMessage(ErrorCode.D_QUERY_NO_DATA, "查無資料");
            //}

            this.gvResult.DataSource = datas;
            this.gvResult.DataBind();
            return xmlResult.IsSuccess;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
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
            QueryString.Add("ReceiveType", this.QueryReceiveType);
            QueryString.Add("YearId", this.QueryYearID);
            Session["QueryString"] = QueryString;

            Server.Transfer("D1100001M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            TermListEntity[] datas = this.gvResult.DataSource as TermListEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                TermListEntity data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1},{2}", data.ReceiveType, data.YearId, data.TermId);

                //特殊邏輯 : 代碼 1 或 2 固定不提供刪除
                bool visible = (data.TermId == "1" || data.TermId == "2") ? false : true;

                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                    ccbtnModify.Visible = true;
                }

                MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
                if (ccbtnDelete != null)
                {
                    ccbtnDelete.CommandArgument = argument;
                    ccbtnDelete.Visible = visible;
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
            if (args.Length != 3)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("取無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            string editUrl = "D1100001M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("ReceiveType", args[0]);
                        QueryString.Add("YearId", args[1]);
                        QueryString.Add("TermId", args[2]);
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
                        QueryString.Add("YearId", args[1]);
                        QueryString.Add("TermId", args[2]);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion

                        #region [Old]
                        //TermListEntity data = new TermListEntity();
                        //data.ReceiveType = args[0];
                        //data.YearId = args[1];
                        //data.TermId = args[2];

                        //string action = this.GetLocalized("刪除資料");
                        //int count = 0;
                        //XmlResult xmlResult = DataProxy.Current.Delete<TermListEntity>(this, data, out count);
                        //if (xmlResult.IsSuccess)
                        //{
                        //    if (count == 0)
                        //    {
                        //        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                        //    }
                        //    else
                        //    {
                        //        this.ShowActionSuccessMessage(action);

                        //        this.GetDataAndBind(this.QueryReceiveType, this.QueryYearID);
                        //    }
                        //}
                        //else
                        //{
                        //    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        //}
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