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
    /// 身分註記代碼
    /// </summary>
    public partial class D1100009 : BasePage
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
        /// 儲存身分註記的查詢條件
        /// </summary>
        private string QueryIdentifyType
        {
            get
            {
                return ViewState["QueryIdentifyType"] as string;
            }
            set
            {
                ViewState["QueryIdentifyType"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
            if (this.Request.QueryString["IdentifyType"] != null)
            {
                this.QueryIdentifyType = this.Request.QueryString["IdentifyType"].ToString();
            }
            else
            {
                this.QueryIdentifyType = "1";
            }

            //[MDY:20200309] CHECKMARX Reflected XSS Specific Clients Revision
            WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyType, this.QueryIdentifyType);

            #region 產生身分註記選項
            {
                //this.QueryIdentifyType = "1";
                string txt = this.GetLocalized("身分註記");
                this.ddlIdentifyType.Items.Clear();
                CodeTextList items = new CodeTextList(6);
                for(int idx = 1; idx <= 6; idx++)
                {
                    string code = idx.ToString();
                    string text = String.Concat(txt, code);
                    items.Add(code, text);
                }
                WebHelper.SetDropDownListItems(this.ddlIdentifyType, DefaultItem.Kind.None, false, items, true, false, 0, this.QueryIdentifyType);
            }
            #endregion

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

            int identifyType = 0;
            switch (this.QueryIdentifyType)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                    identifyType = int.Parse(this.QueryIdentifyType);
                    break;
                default:
                    return false;
            }

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
            Expression where = GetWhere(identifyType);
            #endregion

            #region 排序條件
            KeyValueList<OrderByEnum> orderbys = null;
            {
                string[] identifyIdFields = new string[] { 
                    IdentifyList1Entity.Field.IdentifyId, IdentifyList2Entity.Field.IdentifyId, IdentifyList3Entity.Field.IdentifyId, 
                    IdentifyList4Entity.Field.IdentifyId, IdentifyList5Entity.Field.IdentifyId, IdentifyList6Entity.Field.IdentifyId };

                orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(identifyIdFields[identifyType - 1], OrderByEnum.Asc);
            }
            #endregion

            object[] datas = null;
            XmlResult xmlResult = null;
            switch (this.QueryIdentifyType)
            {
                case "1":
                    {
                        IdentifyList1Entity[] instances = null;
                        xmlResult = DataProxy.Current.SelectAll<IdentifyList1Entity>(this, where, orderbys, out instances);
                        datas = instances;
                    }
                    break;
                case "2":
                    {
                        IdentifyList2Entity[] instances = null;
                        xmlResult = DataProxy.Current.SelectAll<IdentifyList2Entity>(this, where, orderbys, out instances);
                        datas = instances;
                    }
                    break;
                case "3":
                    {
                        IdentifyList3Entity[] instances = null;
                        xmlResult = DataProxy.Current.SelectAll<IdentifyList3Entity>(this, where, orderbys, out instances);
                        datas = instances;
                    }
                    break;
                case "4":
                    {
                        IdentifyList4Entity[] instances = null;
                        xmlResult = DataProxy.Current.SelectAll<IdentifyList4Entity>(this, where, orderbys, out instances);
                        datas = instances;
                    }
                    break;
                case "5":
                    {
                        IdentifyList5Entity[] instances = null;
                        xmlResult = DataProxy.Current.SelectAll<IdentifyList5Entity>(this, where, orderbys, out instances);
                        datas = instances;
                    }
                    break;
                case "6":
                    {
                        IdentifyList6Entity[] instances = null;
                        xmlResult = DataProxy.Current.SelectAll<IdentifyList6Entity>(this, where, orderbys, out instances);
                        datas = instances;
                    }
                    break;
            }
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

        private Expression GetWhere(int identifyType)
        {
            int idx = identifyType - 1;
            Expression where = new Expression();

            string[] receiveTypeFields = new string[] {
                IdentifyList1Entity.Field.ReceiveType, IdentifyList2Entity.Field.ReceiveType, IdentifyList3Entity.Field.ReceiveType, 
                IdentifyList4Entity.Field.ReceiveType, IdentifyList5Entity.Field.ReceiveType, IdentifyList6Entity.Field.ReceiveType };
            string[] yearIdFields = new string[] { 
                IdentifyList1Entity.Field.YearId, IdentifyList2Entity.Field.YearId, IdentifyList3Entity.Field.YearId, 
                IdentifyList4Entity.Field.YearId, IdentifyList5Entity.Field.YearId, IdentifyList6Entity.Field.YearId };
            string[] termIdFields = new string[] { 
                IdentifyList1Entity.Field.TermId, IdentifyList2Entity.Field.TermId, IdentifyList3Entity.Field.TermId, 
                IdentifyList4Entity.Field.TermId, IdentifyList5Entity.Field.TermId, IdentifyList6Entity.Field.TermId };
            string[] depIdFields = new string[] { 
                IdentifyList1Entity.Field.DepId, IdentifyList2Entity.Field.DepId, IdentifyList3Entity.Field.DepId, 
                IdentifyList4Entity.Field.DepId, IdentifyList5Entity.Field.DepId, IdentifyList6Entity.Field.DepId };

            string receiveType = this.QueryReceiveType;
            if (!String.IsNullOrEmpty(receiveType))
            {
                where.And(receiveTypeFields[idx], receiveType);
            }

            string yearId = this.QueryYearID;
            if (!String.IsNullOrEmpty(yearId))
            {
                where.And(yearIdFields[idx], yearId);
            }

            string termId = this.QueryTermID;
            if (!String.IsNullOrEmpty(termId))
            {
                where.And(termIdFields[idx], termId);
            }

            string DepId = this.QueryDepID;
            if (!String.IsNullOrEmpty(DepId))
            {
                where.And(depIdFields[idx], DepId);
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

        protected void ddlIdentifyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.QueryIdentifyType = ddlIdentifyType.SelectedValue;
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

            if (String.IsNullOrEmpty(this.QueryIdentifyType))
            {
                this.ShowMustInputAlert(this.GetLocalized("身分註記"));
                return;
            }

            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("Action", ActionMode.Insert);
            QueryString.Add("ReceiveType", this.QueryReceiveType);
            QueryString.Add("YearId", this.QueryYearID);
            QueryString.Add("TermId", this.QueryTermID);
            QueryString.Add("DepId", this.QueryDepID);
            QueryString.Add("IdentifyType", this.QueryIdentifyType);
            Session["QueryString"] = QueryString;

            Server.Transfer("D1100009M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            object[] datas = null;
            string identifyType = this.QueryIdentifyType;
            switch (identifyType)
            {
                case "1":
                    datas = this.gvResult.DataSource as IdentifyList1Entity[];
                    break;
                case "2":
                    datas = this.gvResult.DataSource as IdentifyList2Entity[];
                    break;
                case "3":
                    datas = this.gvResult.DataSource as IdentifyList3Entity[];
                    break;
                case "4":
                    datas = this.gvResult.DataSource as IdentifyList4Entity[];
                    break;
                case "5":
                    datas = this.gvResult.DataSource as IdentifyList5Entity[];
                    break;
                case "6":
                    datas = this.gvResult.DataSource as IdentifyList6Entity[];
                    break;
            }

            if (datas == null || datas.Length == 0)
            {
                return;
            }

            //資料參數
            string argument = null;
            foreach (GridViewRow row in this.gvResult.Rows)
            {
                switch (identifyType)
                {
                    case "1":
                        {
                            IdentifyList1Entity data = datas[row.RowIndex] as IdentifyList1Entity;
                            argument = String.Format("{0},{1},{2},{3},{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.IdentifyId);
                        }
                        break;
                    case "2":
                        {
                            IdentifyList2Entity data = datas[row.RowIndex] as IdentifyList2Entity;
                            argument = String.Format("{0},{1},{2},{3},{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.IdentifyId);
                        }
                        break;
                    case "3":
                        {
                            IdentifyList3Entity data = datas[row.RowIndex] as IdentifyList3Entity;
                            argument = String.Format("{0},{1},{2},{3},{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.IdentifyId);
                        }
                        break;
                    case "4":
                        {
                            IdentifyList4Entity data = datas[row.RowIndex] as IdentifyList4Entity;
                            argument = String.Format("{0},{1},{2},{3},{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.IdentifyId);
                        }
                        break;
                    case "5":
                        {
                            IdentifyList5Entity data = datas[row.RowIndex] as IdentifyList5Entity;
                            argument = String.Format("{0},{1},{2},{3},{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.IdentifyId);
                        }
                        break;
                    case "6":
                        {
                            IdentifyList6Entity data = datas[row.RowIndex] as IdentifyList6Entity;
                            argument = String.Format("{0},{1},{2},{3},{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.IdentifyId);
                        }
                        break;
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

            string identifyType = this.QueryIdentifyType;
            if (String.IsNullOrEmpty(identifyType))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("取無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            string editUrl = "D1100009M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("IdentifyType", this.QueryIdentifyType);
                        QueryString.Add("ReceiveType", args[0]);
                        QueryString.Add("YearId", args[1]);
                        QueryString.Add("TermId", args[2]);
                        QueryString.Add("DepId", args[3]);
                        QueryString.Add("IdentifyId", args[4]);
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
                        QueryString.Add("IdentifyType", this.QueryIdentifyType);
                        QueryString.Add("ReceiveType", args[0]);
                        QueryString.Add("YearId", args[1]);
                        QueryString.Add("TermId", args[2]);
                        QueryString.Add("DepId", args[3]);
                        QueryString.Add("IdentifyId", args[4]);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion

                        #region [Old]
                        //string action = this.GetLocalized("刪除資料");
                        //int count = 0;
                        //XmlResult xmlResult = null;

                        //switch (identifyType)
                        //{
                        //    case "1":
                        //        {
                        //            IdentifyList1Entity data = new IdentifyList1Entity();
                        //            data.ReceiveType = args[0];
                        //            data.YearId = args[1];
                        //            data.TermId = args[2];
                        //            data.DepId = args[3];
                        //            data.IdentifyId = args[4];
                        //            xmlResult = DataProxy.Current.Delete<IdentifyList1Entity>(this, data, out count);
                        //        }
                        //        break;
                        //    case "2":
                        //        {
                        //            IdentifyList2Entity data = new IdentifyList2Entity();
                        //            data.ReceiveType = args[0];
                        //            data.YearId = args[1];
                        //            data.TermId = args[2];
                        //            data.DepId = args[3];
                        //            data.IdentifyId = args[4];
                        //            xmlResult = DataProxy.Current.Delete<IdentifyList2Entity>(this, data, out count);
                        //        }
                        //        break;
                        //    case "3":
                        //        {
                        //            IdentifyList3Entity data = new IdentifyList3Entity();
                        //            data.ReceiveType = args[0];
                        //            data.YearId = args[1];
                        //            data.TermId = args[2];
                        //            data.DepId = args[3];
                        //            data.IdentifyId = args[4];
                        //            xmlResult = DataProxy.Current.Delete<IdentifyList3Entity>(this, data, out count);
                        //        }
                        //        break;
                        //    case "4":
                        //        {
                        //            IdentifyList4Entity data = new IdentifyList4Entity();
                        //            data.ReceiveType = args[0];
                        //            data.YearId = args[1];
                        //            data.TermId = args[2];
                        //            data.DepId = args[3];
                        //            data.IdentifyId = args[4];
                        //            xmlResult = DataProxy.Current.Delete<IdentifyList4Entity>(this, data, out count);
                        //        }
                        //        break;
                        //    case "5":
                        //        {
                        //            IdentifyList5Entity data = new IdentifyList5Entity();
                        //            data.ReceiveType = args[0];
                        //            data.YearId = args[1];
                        //            data.TermId = args[2];
                        //            data.DepId = args[3];
                        //            data.IdentifyId = args[4];
                        //            xmlResult = DataProxy.Current.Delete<IdentifyList5Entity>(this, data, out count);
                        //        }
                        //        break;
                        //    case "6":
                        //        {
                        //            IdentifyList6Entity data = new IdentifyList6Entity();
                        //            data.ReceiveType = args[0];
                        //            data.YearId = args[1];
                        //            data.TermId = args[2];
                        //            data.DepId = args[3];
                        //            data.IdentifyId = args[4];
                        //            xmlResult = DataProxy.Current.Delete<IdentifyList6Entity>(this, data, out count);
                        //        }
                        //        break;
                        //    default:
                        //        xmlResult = new XmlResult(false, "取無法取得要處理資料的參數");
                        //        break;
                        //}

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