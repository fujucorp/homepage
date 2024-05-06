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

namespace eSchoolWeb.C
{
    /// <summary>
    /// 還原銷帳註記
    /// </summary>
    public partial class C3700005 : PagingBasePage
    {
        #region Property
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

        /// <summary>
        /// 儲存查詢的學年參數
        /// </summary>
        private string QueryYearId
        {
            get
            {
                return ViewState["QueryYearId"] as string;
            }
            set
            {
                ViewState["QueryYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的學期參數
        /// </summary>
        private string QueryTermId
        {
            get
            {
                return ViewState["QueryTermId"] as string;
            }
            set
            {
                ViewState["QueryTermId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的部別參數
        /// </summary>
        private string QueryDepId
        {
            get
            {
                return ViewState["QueryDepId"] as string;
            }
            set
            {
                ViewState["QueryDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的代收費用別參數
        /// </summary>
        private string QueryReceiveId
        {
            get
            {
                return ViewState["QueryReceiveId"] as string;
            }
            set
            {
                ViewState["QueryReceiveId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的查詢欄位 (StuId=學號 / CancelNo=銷帳編號 / IdNumber=身分證字號)
        /// </summary>
        private string QuerySearchField
        {
            get
            {
                return ViewState["QuerySearchField"] as string;
            }
            set
            {
                ViewState["QuerySearchField"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的查詢值
        /// </summary>
        private string QuerySearchValue
        {
            get
            {
                return ViewState["QuerySearchValue"] as string;
            }
            set
            {
                ViewState["QuerySearchValue"] = value == null ? null : value.Trim();
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
            #region 5 Key 條件
            where = new Expression(StudentView.Field.ReceiveType, this.QueryReceiveType)
                        .And(StudentView.Field.YearId, this.QueryYearId)
                        .And(StudentView.Field.TermId, this.QueryTermId)
                        .And(StudentView.Field.DepId, this.QueryDepId)
                        .And(StudentView.Field.ReceiveId, this.QueryReceiveId);
            #endregion

            #region 查詢欄位與值 條件 (StuId=學號 / CancelNo=銷帳編號 / IdNumber=身分證字號)
            {
                string qValue = this.QuerySearchValue;
                if (!String.IsNullOrEmpty(qValue))
                {
                    switch (this.QuerySearchField)
                    {
                        case "StuId":
                            where.And(StudentView.Field.StuId, qValue);
                            break;
                        case "CancelNo":
                            where.And(StudentView.Field.CancelNo, qValue);
                            break;
                        case "IdNumber":
                            where.And(StudentView.Field.IdNumber, qValue);
                            break;
                    }
                }
            }
            #endregion

            where.And(StudentView.Field.CancelFlagFields, RelationEnum.NotEqual, null);
            where.And(StudentView.Field.CancelFlagFields, RelationEnum.NotEqual, String.Empty);

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentView.Field.StuId, OrderByEnum.Asc);

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
            XmlResult xmlResult = base.QueryDataAndBind<StudentView>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
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
                this.ccbtnQuery.Visible = this.InitialUI();
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region 處理五個下拉選項
            string receiveType = null;
            string yearID = null;
            string termID = null;
            string depID = null;
            string receiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out receiveID)
                || String.IsNullOrEmpty(receiveType)
                || String.IsNullOrEmpty(yearID)
                || String.IsNullOrEmpty(termID))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
                this.ShowJsAlert(msg);
                return false;
            }

            //因為控制項的 Page_Load 比頁面的 Page_Load 晚觸發，所以可以先執行 GetDataAndBind 方法，強迫 ucFilter1 結繫資料
            //因為 ucFilter1 有指定 Filter2ControlID 為 ucFilter2，所以 ucFilter2 頁會被自動結繫資料
            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearID, termID, depID, receiveID);
            if (xmlResult.IsSuccess)
            {
                depID = "";
                receiveID = ucFilter2.SelectedReceiveID;
            }

            //一定要用這個方法將業務別碼、學年、學期、部別、代收費用別參數傳給下一頁
            //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
            WebHelper.SetFilterArguments(receiveType, yearID, termID, depID, receiveID);
            #endregion

            this.QueryReceiveType = receiveType;
            this.QueryYearId = yearID;
            this.QueryTermId = termID;
            this.QueryDepId = depID;
            this.QueryReceiveId = receiveID;
            
            #region 查詢特定欄位值
            {
                CodeText[] items = new CodeText[] { new CodeText("StuId", "學號"), new CodeText("CancelNo", "虛擬帳號"), new CodeText("IdNumber", "身分證字號") };
                WebHelper.SetRadioButtonListItems(this.rdoSearchField, items, true, 2, items[0].Code);
            }
            #endregion

            #region 查詢結果初始化
            {
                //因為有新增與回上一頁的按鈕，所以不適合把 divResult 隱藏
                //改為隱藏分頁按鈕，並結繫 null
                //this.divResult.Visible = false;
                this.gvResult.DataSource = null;
                this.gvResult.DataBind();
                Paging[] ucPagings = this.GetPagingControls();
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>成功傳回 true，否則傳回 false</returns>
        private bool GetAndKeepQueryCondition()
        {
            #region 5 Key
            this.QueryReceiveType = this.ucFilter1.SelectedReceiveType;
            this.QueryYearId = this.ucFilter1.SelectedYearID;
            this.QueryTermId = this.ucFilter1.SelectedTermID;

            #region 土銀不使用原有部別 DepList，改用專用的部別 DeptList
            //this.QueryDepId = this.ucFilter2.SelectedDepID;
            #endregion

            this.QueryReceiveId = this.ucFilter2.SelectedReceiveID;
            if (String.IsNullOrEmpty(this.QueryReceiveType))
            {
                this.ShowMustInputAlert("業務別碼");
                return false;
            }
            if (String.IsNullOrEmpty(this.QueryYearId))
            {
                this.ShowMustInputAlert("學年");
                return false;
            }
            if (String.IsNullOrEmpty(this.QueryTermId))
            {
                this.ShowMustInputAlert("學期");
                return false;
            }

            #region 土銀不使用原有部別 DepList，改用專用的部別 DeptList
            //if (String.IsNullOrEmpty(this.QueryDepId))
            //{
            //    this.ShowMustInputAlert("部別");
            //    return false;
            //}
            #endregion

            if (String.IsNullOrEmpty(this.QueryReceiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return false;
            }
            #endregion

            //查詢的查詢欄位
            this.QuerySearchField = this.rdoSearchField.SelectedValue;

            //查詢的查詢值
            this.QuerySearchValue = this.tbxSearchValue.Text.Trim();

            return true;
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
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                }
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            StudentView[] datas = this.gvResult.DataSource as StudentView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            bool isVisible = this.HasMaintainAuth();

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                StudentView data = datas[row.RowIndex];

                #region [MDY:20160607] 修正資料參數
                #region [Old]
                ////資料參數
                //string argument = String.Format("{0},{1},{2},{3},{4},{5}", data.StuId, data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);
                #endregion

                string argument = String.Format("{0},{1},{2},{3},{4},{5},{6}", data.StuId, data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.OldSeq);
                #endregion

                CodeText cancelStatus = CancelStatusCodeTexts.GetCancelStatus(data.ReceiveDate, data.AccountDate);
                row.Cells[4].Text = cancelStatus.Text;

                LinkButton ccbtnModify = row.FindControl("ccbtnModify") as LinkButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                    ccbtnModify.Visible = isVisible;
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
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            string[] args = argument.Split(new char[] { ',' }, StringSplitOptions.None);

            #region [MDY:20160607] 修正資料參數
            #region [Old]
            //if (args.Length != 6)
            //{
            //    //[TODO] 固定顯示訊息的收集
            //    string msg = this.GetLocalized("無法取得要處理資料的參數");
            //    this.ShowSystemMessage(msg);
            //    return;
            //}

            //string stuId = args[0];
            //string receiveType = args[1];
            //string yearId = args[2];
            //string termId = args[3];
            //string depId = args[4];
            //string receiveId = args[5];
            #endregion

            if (args.Length != 7)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }

            string stuId = args[0];
            string receiveType = args[1];
            string yearId = args[2];
            string termId = args[3];
            string depId = args[4];
            string receiveId = args[5];
            string oldSeq = args[6];
            #endregion

            #endregion

            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 還原
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Query);
                        QueryString.Add("StuId", stuId);
                        QueryString.Add("ReceiveType", receiveType);
                        QueryString.Add("YearId", yearId);
                        QueryString.Add("TermId", termId);
                        QueryString.Add("DepId", depId);
                        QueryString.Add("ReceiveId", receiveId);

                        #region [MDY:20160607] 修正資料參數
                        QueryString.Add("OldSeq", oldSeq);
                        #endregion

                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("C3700005M.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
            }
        }
    }
}