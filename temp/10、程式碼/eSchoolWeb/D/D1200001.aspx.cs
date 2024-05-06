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
    /// 代收費用檔
    /// </summary>
    public partial class D1200001 : BasePage
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

        /// <summary>
        /// 儲存學期代碼的查詢條件
        /// </summary>
        private string QueryTermID
        {
            get
            {
                return ViewState["QueryTermID"] as string;
            }
            set
            {
                ViewState["QueryTermID"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存部別代碼的查詢條件
        /// </summary>
        private string QueryDepID
        {
            get
            {
                return ViewState["QueryDepID"] as string;
            }
            set
            {
                ViewState["QueryDepID"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存代收費用別的查詢條件
        /// </summary>
        private string QueryReceiveID
        {
            get
            {
                return ViewState["QueryReceiveID"] as string;
            }
            set
            {
                ViewState["QueryReceiveID"] = value == null ? null : value.Trim();
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
                string msg = this.GetErrorLocalized(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearID, termID, depID, ReceiveID);
            if (xmlResult.IsSuccess)
            {
                this.QueryReceiveType = this.ucFilter1.SelectedReceiveType;
                this.QueryYearID = this.ucFilter1.SelectedYearID;
                this.QueryTermID = this.ucFilter1.SelectedTermID;
                this.QueryDepID = " ";
                this.QueryReceiveID = this.ucFilter2.SelectedReceiveID;
            }
            else
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
                this.ShowSystemMessage(msg);
                return false;
            }

            return this.GetAndBindQueryData();
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetAndBindQueryData()
        {
            //if (String.IsNullOrEmpty(this.QueryDepID))
            //{
            //    return false;
            //}

            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
                return false;
            }
            #endregion

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(this.QueryReceiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                return false;
            }
            #endregion

            #region 查詢條件
            Expression where = GetWhere();
            #endregion

            #region 排序條件
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(SchoolRidView.Field.ReceiveId, OrderByEnum.Asc);
            #endregion

            SchoolRidView[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<SchoolRidView>(this, where, orderbys, out datas);
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

        private Expression GetWhere()
        {
            Expression where = new Expression();

            string receiveType = this.QueryReceiveType;
            if (!String.IsNullOrEmpty(receiveType))
            {
                where.And(SchoolRidView.Field.ReceiveType, receiveType);
            }

            string yearId = this.QueryYearID;
            if (!String.IsNullOrEmpty(yearId))
            {
                where.And(SchoolRidView.Field.YearId, yearId);
            }

            string termId = this.QueryTermID;
            if (!String.IsNullOrEmpty(termId))
            {
                where.And(SchoolRidView.Field.TermId, termId);
            }

            string DepId = this.QueryDepID;
            if (!String.IsNullOrEmpty(DepId))
            {
                where.And(SchoolRidView.Field.DepId, DepId);
            }

            string receiveId = this.QueryReceiveID;
            if (!String.IsNullOrEmpty(receiveId))
            {
                where.And(SchoolRidView.Field.ReceiveId, receiveId);
            }

            return where;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            //this.QueryDepID = ucFilter2.SelectedDepID;
            this.QueryReceiveID = ucFilter2.SelectedReceiveID;
            this.GetAndBindQueryData();
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

            //if (String.IsNullOrEmpty(this.QueryDepID))
            //{
            //    this.ShowMustInputAlert("Filter2", "Dep", "部別");
            //    return;
            //}

            if (String.IsNullOrEmpty(this.QueryReceiveID))
            {
                this.ShowMustInputAlert("Filter2", "ReceiveID", "代收費用別");
                return;
            }
            else if (this.gvResult.Rows.Count > 0)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("該代收費用別已設定，無法新增");
                this.ShowJsAlert(msg);
                return;
            }

            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("Action", ActionMode.Insert);
            QueryString.Add("ReceiveType", this.QueryReceiveType);
            QueryString.Add("YearId", this.QueryYearID);
            QueryString.Add("TermId", this.QueryTermID);
            QueryString.Add("DepId", this.QueryDepID);
            QueryString.Add("ReceiveId", this.QueryReceiveID);
            Session["QueryString"] = QueryString;

            Server.Transfer("D1200001M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            SchoolRidView[] datas = this.gvResult.DataSource as SchoolRidView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                SchoolRidView data = datas[row.RowIndex];
                //資料參數 (ReceiveStatus 固定為 1 所以不用傳)
                string argument = String.Format("{0},{1},{2},{3},{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);

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

            string[] args = argument.Split(new char[] { ',' });
            if (args.Length != 5)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("取無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            string editUrl = "D1200001M.aspx";
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
                        QueryString.Add("DepId", args[3]);
                        QueryString.Add("ReceiveId", args[4]);
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
                        QueryString.Add("DepId", args[3]);
                        QueryString.Add("ReceiveId", args[4]);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion

                        #region [Old]
                        //SchoolRidEntity data = new SchoolRidEntity();
                        //data.ReceiveType = args[0];
                        //data.YearId = args[1];
                        //data.TermId = args[2];
                        //data.DepId = args[3];
                        //data.ReceiveId = args[4];

                        //string action = this.GetLocalized("刪除資料");
                        //int count = 0;
                        //XmlResult xmlResult = DataProxy.Current.Delete<SchoolRidEntity>(this, data, out count);
                        //if (xmlResult.IsSuccess)
                        //{
                        //    if (count == 0)
                        //    {
                        //        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                        //    }
                        //    else
                        //    {
                        //        this.ShowActionSuccessMessage(action);

                        //        this.GetAndBindQueryData();
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