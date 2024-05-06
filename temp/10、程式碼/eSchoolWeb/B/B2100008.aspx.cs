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

namespace eSchoolWeb.B
{
    /// <summary>
    /// 維護就貸資料
    /// </summary>
    public partial class B2100008 : PagingBasePage
    {
        #region Property
        /// <summary>
        /// 儲存查詢的商家代號代碼
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
        /// 儲存查詢的科系代碼
        /// </summary>
        private string QueryMajorId
        {
            get
            {
                return ViewState["QueryMajorId"] as string;
            }
            set
            {
                ViewState["QueryMajorId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的年級代碼
        /// </summary>
        private string QueryStuGrade
        {
            get
            {
                return ViewState["QueryStuGrade"] as string;
            }
            set
            {
                ViewState["QueryStuGrade"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的批號
        /// </summary>
        private string QueryUpNo
        {
            get
            {
                return ViewState["QueryUpNo"] as string;
            }
            set
            {
                ViewState["QueryUpNo"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的查詢欄位 (StuId=學號 / CancelNo=銷帳編號 / IdNumber=身分證字號 / StuName=姓名)
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
            where = new Expression(StudentReceiveView5.Field.ReceiveType, this.QueryReceiveType)
                        .And(StudentReceiveView5.Field.YearId, this.QueryYearId)
                        .And(StudentReceiveView5.Field.TermId, this.QueryTermId)
                        .And(StudentReceiveView5.Field.DepId, this.QueryDepId)
                        .And(StudentReceiveView5.Field.ReceiveId, this.QueryReceiveId);
            #endregion

            #region 科系 條件
            if (!String.IsNullOrEmpty(this.QueryMajorId))
            {
                where.And(StudentReceiveView5.Field.MajorId, this.QueryMajorId);
            }
            #endregion

            #region 年級 條件
            if (!String.IsNullOrEmpty(this.QueryStuGrade))
            {
                where.And(StudentReceiveView5.Field.StuGrade, this.QueryStuGrade);
            }
            #endregion

            #region 批號 條件
            if (!String.IsNullOrEmpty(this.QueryUpNo))
            {
                where.And(StudentReceiveView5.Field.UpNo, this.QueryUpNo);
            }
            #endregion

            #region 查詢欄位與值 條件 (StuId=學號 / CancelNo=銷帳編號 / IdNumber=身分證字號)
            {
                string qValue = this.QuerySearchValue;
                if (!String.IsNullOrEmpty(qValue))
                {
                    switch (this.QuerySearchField)
                    {
                        case "StuId":
                            where.And(StudentReceiveView5.Field.StuId, qValue);
                            break;
                        case "CancelNo":
                            where.And(StudentReceiveView5.Field.CancelNo, qValue);
                            break;
                        case "IdNumber":
                            where.And(StudentReceiveView5.Field.StuIdNumber, qValue);
                            break;
                        case "StuName":
                            where.And(StudentReceiveView5.Field.StuName, qValue);
                            break;
                    }
                }
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReceiveView5.Field.StuId, OrderByEnum.Asc);

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

            if (String.IsNullOrEmpty(this.QueryReceiveId))
            {
                this.gvResult.Columns[0].Visible = true;   //查詢條件無費用別，則查詢結果要顯示未選擇
            }
            else
            {
                this.gvResult.Columns[0].Visible = false;    //查詢條件有費用別，則查詢結果不顯示未選擇
            }

            XmlResult xmlResult = base.QueryDataAndBind<StudentReceiveView5>(pagingInfo, ucPagings, this.gvResult);
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

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region 處理五個下拉選項
            string receiveType = null;
            string yearId = null;
            string termId = null;
            string depId = null;
            string receiveId = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearId, out termId, out depId, out receiveId)
                || String.IsNullOrEmpty(receiveType)
                || (this.ucFilter1.YearVisible && String.IsNullOrEmpty(yearId))
                || (this.ucFilter1.TermVisible && String.IsNullOrEmpty(termId)))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得商家代號、學年或學期參數");
                this.ShowJsAlert(msg);
                return false;
            }

            //因為控制項的 Page_Load 比頁面的 Page_Load 晚觸發，所以可以先執行 GetDataAndBind 方法，強迫 ucFilter1 結繫資料
            //因為 ucFilter1 有指定 Filter2ControlID 為 ucFilter2，所以 ucFilter2 頁會被自動結繫資料
            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
            if (xmlResult.IsSuccess)
            {
                #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
                //depId = ucFilter2.SelectedDeptID;
                #endregion

                depId = "";
                receiveId = ucFilter2.SelectedReceiveID;
            }

            #region 費用別預設為第一個有效的選項
            if (String.IsNullOrEmpty(receiveId))
            {
                CodeTextList receiveItems = ucFilter2.GetReceiveItems();
                if (receiveItems.Count > 0)
                {
                    receiveId = receiveItems[0].Code;
                    ucFilter2.ChangeSelectedReceiveId(receiveId);
                }
            }
            #endregion

            //一定要用這個方法將商家代號、學年、學期、部別、代收費用別參數傳給下一頁
            //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
            WebHelper.SetFilterArguments(receiveType, yearId, termId, depId, receiveId);
            #endregion

            this.QueryReceiveType = receiveType;
            this.QueryYearId = yearId;
            this.QueryTermId = termId;
            this.QueryDepId = depId;
            this.QueryReceiveId = receiveId;

            #region 查詢特定欄位值
            {
                CodeText[] items = new CodeText[] { 
                    new CodeText("StuId", "學號"), new CodeText("CancelNo", "虛擬帳號"), 
                    new CodeText("IdNumber", "身分證字號"), new CodeText("StuName", "姓名")
                };
                WebHelper.SetRadioButtonListItems(this.rdoSearchField, items, true, 2, items[0].Code);
            }
            #endregion

            #region 科系
            this.GetAndBindMajorOptions(this.QueryReceiveType, this.QueryYearId, this.QueryTermId, this.QueryDepId);
            #endregion

            #region 年級
            {
                WebHelper.SetDropDownListItems(this.ddlGrade, DefaultItem.Kind.All, false, new GradeCodeTexts(), false, false, 0, null);
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

            this.gvResult.Visible = false;

            this.ucFilter2_ItemSelectedIndexChanged(this.ucFilter2, null);

            return true;
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>成功傳回 true，否則傳回 false</returns>
        private bool GetAndKeepQueryCondition()
        {
            #region 查詢的 5 Key
            string qReceiveType = this.ucFilter1.SelectedReceiveType;
            string qYearId = this.ucFilter1.SelectedYearID;
            string qTermId = this.ucFilter1.SelectedTermID;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //string qDepId = this.ucFilter2.SelectedDepID;
            #endregion

            string qDepId = "";

            string qReceiveId = this.ucFilter2.SelectedReceiveID;

            if (String.IsNullOrEmpty(qReceiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return false;
            }
            if (String.IsNullOrEmpty(qYearId))
            {
                this.ShowMustInputAlert("學年");
                return false;
            }
            if (String.IsNullOrEmpty(qTermId))
            {
                this.ShowMustInputAlert("學期");
                return false;
            }

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(qDepIdd))
            //{
            //    this.ShowMustInputAlert("部別");
            //    return false;
            //}
            #endregion

            if (String.IsNullOrEmpty(qReceiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return false;
            }
            #endregion

            #region 記住 5 Key
            WebHelper.SetFilterArguments(qReceiveType, qYearId, qTermId, qDepId, qReceiveId);
            #endregion

            this.QueryReceiveType = qReceiveType;
            this.QueryYearId = qYearId;
            this.QueryTermId = qTermId;
            this.QueryDepId = qDepId;
            this.QueryReceiveId = qReceiveId;

            #region 查詢的科系
            this.QueryMajorId = this.ddlMajor.SelectedValue;
            #endregion

            #region 查詢的年級
            this.QueryStuGrade = this.ddlGrade.SelectedValue;
            #endregion

            #region 查詢的批號
            this.QueryUpNo = this.ddlUpNo.SelectedValue;
            #endregion

            #region 查詢的查詢欄位與值
            this.QuerySearchField = this.rdoSearchField.SelectedValue;
            this.QuerySearchValue = this.tbxSearchValue.Text.Trim();
            #endregion

            return true;
        }

        /// <summary>
        /// 取得並結繫科系選項
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        private void GetAndBindMajorOptions(string receiveType, string yearId, string termId, string depId)
        {
            #region 取資料
            CodeText[] items = null;

            #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
            //if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) && !String.IsNullOrEmpty(termId)
            //    && !String.IsNullOrEmpty(depId))
            #endregion

            if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) && !String.IsNullOrEmpty(termId))
            {
                Expression where = new Expression(MajorListEntity.Field.ReceiveType, receiveType)
                    .And(MajorListEntity.Field.YearId, yearId)
                    .And(MajorListEntity.Field.TermId, termId)
                    .And(MajorListEntity.Field.DepId, depId)
                    .And(MajorListEntity.Field.MajorId, RelationEnum.NotEqual, String.Empty);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(MajorListEntity.Field.MajorId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { MajorListEntity.Field.MajorId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { MajorListEntity.Field.MajorName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<MajorListEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢科系資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            WebHelper.SetDropDownListItems(this.ddlMajor, DefaultItem.Kind.All, false, items, false, false, 0, null);
        }

        /// <summary>
        /// 取得並結繫上傳批號選項
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        private void GetAndBindUpNoOptions(string receiveType, string yearId, string termId, string depId, string receiveId)
        {
            #region 取資料
            CodeText[] items = null;

            #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
            //if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) && !String.IsNullOrEmpty(termId)
            //    && !String.IsNullOrEmpty(depId) && !String.IsNullOrEmpty(receiveId))
            #endregion

            if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId)
                && !String.IsNullOrEmpty(termId) && !String.IsNullOrEmpty(receiveId))
            {
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                    .And(StudentReceiveEntity.Field.YearId, yearId)
                    .And(StudentReceiveEntity.Field.TermId, termId)
                    .And(StudentReceiveEntity.Field.DepId, depId)
                    .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                    .And(StudentReceiveEntity.Field.UpNo, RelationEnum.NotEqual, String.Empty);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(StudentReceiveEntity.Field.UpNo, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { StudentReceiveEntity.Field.UpNo };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { StudentReceiveEntity.Field.UpNo };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<StudentReceiveEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢上傳批號資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            #region [MDY:2018xxxx] 批號改用數值遞減排序
            if (items != null)
            {
                WebHelper.SortItemsByValueDesc(ref items);
            }
            #endregion
            WebHelper.SetDropDownListItems(this.ddlUpNo, DefaultItem.Kind.All, false, items, false, false, 0, null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ccbtnQuery.Visible = this.InitialUI();
            }
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
            //this.QueryDepId = this.ucFilter2.SelectedDepId;
            #endregion

            this.QueryDepId = "";
            this.QueryReceiveId = this.ucFilter2.SelectedReceiveID;

            this.GetAndBindUpNoOptions(this.QueryReceiveType, this.QueryYearId, this.QueryTermId, this.QueryDepId, this.QueryReceiveId);
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
                else
                {
                    this.gvResult.Visible = true;
                }
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            StudentReceiveView5[] datas = this.gvResult.DataSource as StudentReceiveView5[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            //儲存已有資料的費用別代碼
            List<string> hasDataReceiveIds = new List<string>();

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                StudentReceiveView5 data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1},{2},{3},{4},{5},{6}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.StuId, data.OldSeq);

                //收集已有資料的費用別代碼
                if (!hasDataReceiveIds.Contains(data.ReceiveId))
                {
                    hasDataReceiveIds.Add(data.ReceiveId);
                }

                bool isNonPay = String.IsNullOrWhiteSpace(data.ReceiveDate) && String.IsNullOrWhiteSpace(data.AccountDate);
                bool hasLoanId = !String.IsNullOrWhiteSpace(data.LoanId);

                MyInsertButton ccbtnInsert = row.FindControl("ccbtnInsert") as MyInsertButton;
                if (ccbtnInsert != null)
                {
                    ccbtnInsert.CommandArgument = argument;
                    ccbtnInsert.Visible = isNonPay && !hasLoanId;
                }

                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                    ccbtnModify.Visible = isNonPay && hasLoanId;
                }

                MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
                if (ccbtnDelete != null)
                {
                    ccbtnDelete.CommandArgument = argument;
                    ccbtnDelete.Visible = isNonPay && hasLoanId;
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
            //string[] args = argument.Split(new char[] { ',' }, StringSplitOptions.None);
            string[] args = argument.Split(new char[] { ',' });
            if (args.Length != 7)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }

            string receiveType = args[0];
            string yearId = args[1];
            string termId = args[2];
            string depId = args[3];
            string receiveId = args[4];
            string stuId = args[5];
            string oldSeq = args[6];
            #endregion

            string url = "B2100008M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Insert:
                    #region 新增資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Insert);
                        QueryString.Add("ReceiveType", receiveType);
                        QueryString.Add("YearId", yearId);
                        QueryString.Add("TermId", termId);
                        QueryString.Add("DepId", depId);
                        QueryString.Add("ReceiveId", receiveId);
                        QueryString.Add("StuId", stuId);
                        QueryString.Add("OldSeq", oldSeq);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(url));
                        #endregion
                    }
                    #endregion
                    break;
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
                        QueryString.Add("StuId", stuId);
                        QueryString.Add("OldSeq", oldSeq);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(url));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Delete:
                    #region 刪除資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Delete);
                        QueryString.Add("ReceiveType", receiveType);
                        QueryString.Add("YearId", yearId);
                        QueryString.Add("TermId", termId);
                        QueryString.Add("DepId", depId);
                        QueryString.Add("ReceiveId", receiveId);
                        QueryString.Add("StuId", stuId);
                        QueryString.Add("OldSeq", oldSeq);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(url));
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