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
    /// 院別代碼
    /// </summary>
    public partial class D1100004 : BasePage
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
            orderbys.Add(CollegeListEntity.Field.CollegeId, OrderByEnum.Asc);
            #endregion

            CollegeListEntity[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<CollegeListEntity>(this, where, orderbys, out datas);
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
                where.And(CollegeListEntity.Field.ReceiveType, receiveType);
            }

            string yearId = this.QueryYearID;
            if (!String.IsNullOrEmpty(yearId))
            {
                where.And(CollegeListEntity.Field.YearId, yearId);
            }

            string termId = this.QueryTermID;
            if (!String.IsNullOrEmpty(termId))
            {
                where.And(CollegeListEntity.Field.TermId, termId);
            }

            string DepId = this.QueryDepID;
            if (!String.IsNullOrEmpty(DepId))
            {
                where.And(CollegeListEntity.Field.DepId, DepId);
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

            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("Action", ActionMode.Insert);
            QueryString.Add("ReceiveType", this.QueryReceiveType);
            QueryString.Add("YearId", this.QueryYearID);
            QueryString.Add("TermId", this.QueryTermID);
            QueryString.Add("DepId", this.QueryDepID);
            Session["QueryString"] = QueryString;

            Server.Transfer("D1100004M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            CollegeListEntity[] datas = this.gvResult.DataSource as CollegeListEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                CollegeListEntity data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1},{2},{3},{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.CollegeId);

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

            string editUrl = "D1100004M.aspx";
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
                        QueryString.Add("CollegeId", args[4]);
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
                        QueryString.Add("CollegeId", args[4]);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion

                        #region [Old]
                        //CollegeListEntity data = new CollegeListEntity();
                        //data.ReceiveType = args[0];
                        //data.YearId = args[1];
                        //data.TermId = args[2];
                        //data.DepId = args[3];
                        //data.CollegeId = args[4];

                        //string action = this.GetLocalized("刪除資料");
                        //int count = 0;
                        //XmlResult xmlResult = DataProxy.Current.Delete<CollegeListEntity>(this, data, out count);
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