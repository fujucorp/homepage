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
    /// 部別代碼
    /// </summary>
    public partial class D1100002 : BasePage
    {
        #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
        //#region Property
        ///// <summary>
        ///// 儲存業務別碼代碼的查詢條件
        ///// </summary>
        //private string QueryReceiveType
        //{
        //    get
        //    {
        //        return ViewState["QueryReceiveType"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryReceiveType"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 儲存學年代碼的查詢條件
        ///// </summary>
        //private string QueryYearID
        //{
        //    get
        //    {
        //        return ViewState["QueryYearID"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryYearID"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 儲存學期代碼的查詢條件
        ///// </summary>
        //private string QueryTermID
        //{
        //    get
        //    {
        //        return ViewState["QueryTermID"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryTermID"] = value == null ? null : value.Trim();
        //    }
        //}
        //#endregion

        ///// <summary>
        ///// 初始化使用介面
        ///// </summary>
        ///// <returns>成功則傳回 true</returns>
        //private bool InitialUI()
        //{
        //    string receiveType = null;
        //    string yearID = null;
        //    string termID = null;
        //    string depID = null;
        //    string ReceiveID = null;
        //    if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out ReceiveID)
        //        || String.IsNullOrEmpty(receiveType)
        //        || (this.ucFilter1.YearVisible && String.IsNullOrEmpty(yearID))
        //        || (this.ucFilter1.TermVisible && String.IsNullOrEmpty(termID)))
        //    {
        //        //[TODO] 固定顯示訊息的收集
        //        string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
        //        this.ShowSystemMessage(msg);
        //        return false;
        //    }

        //    #region 檢查業務別碼授權
        //    if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
        //    {
        //        this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
        //        return false;
        //    }
        //    #endregion

        //    this.QueryReceiveType = receiveType;
        //    this.QueryYearID = yearID;
        //    this.QueryTermID = termID;

        //    return this.GetAndBindQueryData(); ;
        //}

        ///// <summary>
        ///// 取得並結繫查詢資料
        ///// </summary>
        ///// <returns>成功則傳回 true</returns>
        //private bool GetAndBindQueryData()
        //{
        //    #region 檢查查詢權限
        //    if (!this.HasQueryAuth())
        //    {
        //        this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
        //        return false;
        //    }
        //    #endregion

        //    #region 檢查業務別碼授權
        //    if (!this.GetLogonUser().IsAuthReceiveTypes(this.QueryReceiveType))
        //    {
        //        this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
        //        return false;
        //    }
        //    #endregion

        //    #region 查詢條件
        //    Expression where = this.GetWhere();
        //    #endregion

        //    #region 排序條件
        //    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
        //    orderbys.Add(DepListEntity.Field.DepId, OrderByEnum.Asc);
        //    #endregion

        //    DepListEntity[] datas = null;
        //    XmlResult xmlResult = DataProxy.Current.SelectAll<DepListEntity>(this, where, orderbys, out datas);
        //    if (!xmlResult.IsSuccess)
        //    {
        //        string action = ActionMode.GetActionLocalized(ActionMode.Query);
        //        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //    }
        //    //else if (datas == null || datas.Length == 0)
        //    //{
        //    //    this.ShowErrorMessage(ErrorCode.D_QUERY_NO_DATA, "查無資料");
        //    //}

        //    this.gvResult.DataSource = datas;
        //    this.gvResult.DataBind();
        //    return xmlResult.IsSuccess;
        //}

        //private Expression GetWhere()
        //{
        //    Expression where = new Expression();

        //    string receiveType = this.QueryReceiveType;
        //    if (!String.IsNullOrEmpty(receiveType))
        //    {
        //        where.And(DepListEntity.Field.ReceiveType, receiveType);
        //    }

        //    string yearId = this.QueryYearID;
        //    if (!String.IsNullOrEmpty(yearId))
        //    {
        //        where.And(DepListEntity.Field.YearId, yearId);
        //    }

        //    string termId = this.QueryTermID;
        //    if (!String.IsNullOrEmpty(termId))
        //    {
        //        where.And(DepListEntity.Field.TermId, termId);
        //    }
        //    return where;
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!this.IsPostBack)
        //    {
        //        this.InitialUI();
        //    }
        //}

        //protected void ccbtnInsert_Click(object sender, EventArgs e)
        //{
        //    #region 檢查維護權限
        //    if (!this.HasMaintainAuth())
        //    {
        //        this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
        //        return;
        //    }
        //    #endregion

        //    KeyValueList<string> QueryString = new KeyValueList<string>();
        //    QueryString.Add("Action", ActionMode.Insert);
        //    QueryString.Add("ReceiveType", this.QueryReceiveType);
        //    QueryString.Add("YearId", this.QueryYearID);
        //    QueryString.Add("TermId", this.QueryTermID);
        //    Session["QueryString"] = QueryString;

        //    Server.Transfer("D1100002M.aspx");
        //}

        //protected void gvResult_PreRender(object sender, EventArgs e)
        //{
        //    DepListEntity[] datas = this.gvResult.DataSource as DepListEntity[];
        //    if (datas == null || datas.Length == 0)
        //    {
        //        return;
        //    }

        //    foreach (GridViewRow row in this.gvResult.Rows)
        //    {
        //        DepListEntity data = datas[row.RowIndex];
        //        //資料參數
        //        string argument = String.Format("{0},{1},{2},{3}", data.ReceiveType, data.YearId, data.TermId, data.DepId);

        //        MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
        //        if (ccbtnModify != null)
        //        {
        //            ccbtnModify.CommandArgument = argument;
        //        }

        //        MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
        //        if (ccbtnDelete != null)
        //        {
        //            ccbtnDelete.CommandArgument = argument;
        //        }
        //    }
        //}

        //protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    #region 檢查維護權限
        //    if (!this.HasMaintainAuth())
        //    {
        //        this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
        //        return;
        //    }
        //    #endregion

        //    #region 處理資料參數
        //    string argument = e.CommandArgument as string;
        //    if (String.IsNullOrEmpty(argument))
        //    {
        //        //[TODO] 固定顯示訊息的收集
        //        string msg = this.GetLocalized("取無法取得要處理資料的參數");
        //        this.ShowSystemMessage(msg);
        //        return;
        //    }
        //    string[] args = argument.Split(new char[] { ',' });
        //    if (args.Length != 4)
        //    {
        //        //[TODO] 固定顯示訊息的收集
        //        string msg = this.GetLocalized("取無法取得要處理資料的參數");
        //        this.ShowSystemMessage(msg);
        //        return;
        //    }
        //    #endregion

        //    string editUrl = "D1100002M.aspx";
        //    switch (e.CommandName)
        //    {
        //        case ButtonCommandName.Modify:
        //            #region 修改資料
        //            {
        //                KeyValueList<string> QueryString = new KeyValueList<string>();
        //                QueryString.Add("Action", ActionMode.Modify);
        //                QueryString.Add("ReceiveType", args[0]);
        //                QueryString.Add("YearId", args[1]);
        //                QueryString.Add("TermId", args[2]);
        //                QueryString.Add("DepId", args[3]);
        //                Session["QueryString"] = QueryString;
        //                Server.Transfer(editUrl);
        //            }
        //            #endregion
        //            break;
        //        case ButtonCommandName.Delete:
        //            #region 刪除資料
        //            {
        //                KeyValueList<string> QueryString = new KeyValueList<string>();
        //                QueryString.Add("Action", ActionMode.Delete);
        //                QueryString.Add("ReceiveType", args[0]);
        //                QueryString.Add("YearId", args[1]);
        //                QueryString.Add("TermId", args[2]);
        //                QueryString.Add("DepId", args[3]);
        //                Session["QueryString"] = QueryString;
        //                Server.Transfer(editUrl);

        //                #region [Old]
        //                //DepListEntity data = new DepListEntity();
        //                //data.ReceiveType = args[0];
        //                //data.YearId = args[1];
        //                //data.TermId = args[2];
        //                //data.DepId = args[3];

        //                //string action = this.GetLocalized("刪除資料");
        //                //int count = 0;
        //                //XmlResult xmlResult = DataProxy.Current.Delete<DepListEntity>(this, data, out count);
        //                //if (xmlResult.IsSuccess)
        //                //{
        //                //    if (count == 0)
        //                //    {
        //                //        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                //    }
        //                //    else
        //                //    {
        //                //        this.ShowActionSuccessMessage(action);

        //                //        this.GetAndBindQueryData();
        //                //    }
        //                //}
        //                //else
        //                //{
        //                //    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                //}
        //                #endregion
        //            }
        //            #endregion
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

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
            this.QueryTermID = termID;

            return this.GetAndBindQueryData(); ;
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetAndBindQueryData()
        {
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
            Expression where = this.GetWhere();
            #endregion

            #region 排序條件
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(DeptListEntity.Field.DeptId, OrderByEnum.Asc);
            #endregion

            DeptListEntity[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<DeptListEntity>(this, where, orderbys, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }

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
                where.And(DeptListEntity.Field.ReceiveType, receiveType);
            }

            string yearId = this.QueryYearID;
            if (!String.IsNullOrEmpty(yearId))
            {
                where.And(DeptListEntity.Field.YearId, yearId);
            }

            string termId = this.QueryTermID;
            if (!String.IsNullOrEmpty(termId))
            {
                where.And(DeptListEntity.Field.TermId, termId);
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
            QueryString.Add("TermId", this.QueryTermID);
            Session["QueryString"] = QueryString;

            Server.Transfer("D1100002M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            DeptListEntity[] datas = this.gvResult.DataSource as DeptListEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                DeptListEntity data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1},{2},{3}", data.ReceiveType, data.YearId, data.TermId, data.DeptId);

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
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            string[] args = argument.Split(new char[] { ',' });
            if (args.Length != 4)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            string editUrl = "D1100002M.aspx";
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
                        QueryString.Add("DeptId", args[3]);
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
                        QueryString.Add("DeptId", args[3]);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion

                        #region [Old]
                        //DeptListEntity data = new DeptListEntity();
                        //data.ReceiveType = args[0];
                        //data.YearId = args[1];
                        //data.TermId = args[2];
                        //data.DeptId = args[3];

                        //string action = this.GetLocalized("刪除資料");
                        //int count = 0;
                        //XmlResult xmlResult = DataProxy.Current.Delete<DeptListEntity>(this, data, out count);
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