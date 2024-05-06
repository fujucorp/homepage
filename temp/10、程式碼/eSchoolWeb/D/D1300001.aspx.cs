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
    /// 一般收費標準檔
    /// </summary>
    public partial class D1300001 : PagingBasePage
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
        /// 儲存部門別代碼的查詢條件
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
        /// 儲存收費別代碼的查詢條件
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
        private void InitialUI()
        {
            string receiveType = null;
            string yearID = null;
            string termID = null;
            string depID = null;
            string receiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out receiveID)
                || String.IsNullOrEmpty(receiveType)
                || (this.ucFilter1.YearVisible && String.IsNullOrEmpty(yearID))
                || (this.ucFilter1.TermVisible && String.IsNullOrEmpty(termID)))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
                this.ShowSystemMessage(msg);
                return;
            }

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
            {
                string msg = this.GetErrorLocalized(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearID, termID, depID, receiveID);
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
                return;
            }
        }

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
            where.And(GeneralStandardView.Field.ReceiveType, this.QueryReceiveType);
            where.And(GeneralStandardView.Field.YearId, this.QueryYearID);
            where.And(GeneralStandardView.Field.TermId, this.QueryTermID);
            //where.And(GeneralStandardView.Field.DepId, this.QueryDepID);
            where.And(GeneralStandardView.Field.ReceiveId, this.QueryReceiveID);

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(GeneralStandardView.Field.OrderId, OrderByEnum.Asc);
            return new XmlResult(true);
        }

        /// <summary>
        /// 呼叫 QueryDataAndBind 方法
        /// </summary>
        /// <param name="pagingInfo">指定分頁訊息</param>
        /// <returns>傳回處理結果</returns>
        protected override XmlResult CallQueryDataAndBind(PagingInfo pagingInfo)
        {
            #region 檢查查詢權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
                return null;
            }
            #endregion

            Paging[] ucPagings = this.GetPagingControls();
            XmlResult xmlResult = base.QueryDataAndBind<GeneralStandardView>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }


            return xmlResult;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                PagingInfo pagingInfo = new PagingInfo();
                XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);
            }
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            //this.QueryDepID = ucFilter2.SelectedDepID;
            this.QueryReceiveID = ucFilter2.SelectedReceiveID;
            WebHelper.SetFilterArguments(this.QueryReceiveType, this.QueryYearID, this.QueryTermID, this.QueryDepID, this.QueryReceiveID);

            PagingInfo pagingInfo = new PagingInfo();
            XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);
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
            QueryString.Add("Action", "A");
            QueryString.Add("ReceiveType", this.QueryReceiveType);
            QueryString.Add("YearId", this.QueryYearID);
            QueryString.Add("TermId", this.QueryTermID);
            QueryString.Add("DepId", this.QueryDepID);
            QueryString.Add("ReceiveId", this.QueryReceiveID);
            Session["QueryString"] = QueryString;

            Server.Transfer("D1300001M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            GeneralStandardView[] datas = this.gvResult.DataSource as GeneralStandardView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                GeneralStandardView data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.CollegeId, data.MajorId, data.StuGrade, data.ClassId);

                row.Cells[2].Text = GradeCodeTexts.GetText(data.StuGrade);
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
            if (args.Length != 9)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("取無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }

            string receiveType = args[0].Trim();
            string yearId = args[1].Trim();
            string termId = args[2].Trim();
            string depId = args[3].Trim();
            string receiveId = args[4].Trim();
            string collegeId = args[5].Trim();
            string majorId = args[6].Trim();
            string stuGrade = args[7].Trim();
            string classId = args[8].Trim();
            #endregion

            string editUrl = "D1300001M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("ReceiveType", receiveType);
                        QueryString.Add("YearId", yearId);
                        QueryString.Add("TermId", termId);
                        QueryString.Add("DepId", depId);
                        QueryString.Add("ReceiveId", receiveId);
                        QueryString.Add("CollegeId", collegeId);
                        QueryString.Add("MajorId", majorId);
                        QueryString.Add("StuGrade", stuGrade);
                        QueryString.Add("ClassId", classId);
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
                        //KeyValueList<string> QueryString = new KeyValueList<string>();
                        //QueryString.Add("Action", ActionMode.Delete);
                        //QueryString.Add("ReceiveType", receiveType);
                        //QueryString.Add("YearId", yearId);
                        //QueryString.Add("TermId", termId);
                        //QueryString.Add("DepId", depId);
                        //QueryString.Add("ReceiveId", receiveId);
                        //QueryString.Add("CollegeId", collegeId);
                        //QueryString.Add("MajorId", majorId);
                        //QueryString.Add("StuGrade", stuGrade);
                        //QueryString.Add("ClassId", classId);
                        //Session["QueryString"] = QueryString;
                        //Server.Transfer(editUrl);

                        #region 刪除條件
                        GeneralStandardEntity data = new GeneralStandardEntity();
                        data.ReceiveType = receiveType;
                        data.YearId = yearId;
                        data.TermId = termId;
                        data.DepId = depId;
                        data.ReceiveId = receiveId;
                        data.CollegeId = collegeId;
                        data.MajorId = majorId;
                        data.StuGrade = stuGrade;
                        data.ClassId = classId;
                        #endregion

                        string action = this.GetLocalized("刪除資料");
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Delete<GeneralStandardEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count == 0)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                this.ShowActionSuccessMessage(action);

                                this.CallQueryDataAndBind(this.ucPaging1.GetPagingInfo());
                            }
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }
    }
}