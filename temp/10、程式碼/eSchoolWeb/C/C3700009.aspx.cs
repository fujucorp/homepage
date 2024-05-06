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
    /// 查詢歷史(繳費)資料
    /// </summary>
    public partial class C3700009 : PagingBasePage
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

        #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
        ///// <summary>
        ///// 儲存查詢的部別參數
        ///// </summary>
        //private string QueryDepId
        //{
        //    get
        //    {
        //        return ViewState["QueryDepId"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryDepId"] = value == null ? null : value.Trim();
        //    }
        //}
        #endregion

        /// <summary>
        /// 儲存查詢的部別參數
        /// </summary>
        private string QueryDepId
        {
            get
            {
                return String.Empty;
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
        /// 儲存查詢的部別代碼
        /// </summary>
        private string QueryDeptId
        {
            get
            {
                return ViewState["QueryDeptId"] as string;
            }
            set
            {
                ViewState["QueryDeptId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的院別代碼
        /// </summary>
        private string QueryCollegeId
        {
            get
            {
                return ViewState["QueryCollegeId"] as string;
            }
            set
            {
                ViewState["QueryCollegeId"] = value == null ? null : value.Trim();
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
        /// 儲存查詢的班別代碼
        /// </summary>
        private string QueryClassId
        {
            get
            {
                return ViewState["QueryClassId"] as string;
            }
            set
            {
                ViewState["QueryClassId"] = value == null ? null : value.Trim();
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
        /// 儲存查詢的銷帳狀態
        /// </summary>
        private string QueryCancelStatus
        {
            get
            {
                return ViewState["QueryCancelStatus"] as string;
            }
            set
            {
                ViewState["QueryCancelStatus"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的繳款方式
        /// </summary>
        private string QueryReceiveWay
        {
            get
            {
                return ViewState["QueryReceiveWay"] as string;
            }
            set
            {
                ViewState["QueryReceiveWay"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的日期區間種類
        /// </summary>
        private string QueryDateRangeKind
        {
            get
            {
                return ViewState["QueryDateRangeKind"] as string;
            }
            set
            {
                ViewState["QueryDateRangeKind"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的日期區間起日
        /// </summary>
        private DateTime? QueryDateRangStart
        {
            get
            {
                return ViewState["QueryDateRangStart"] as DateTime?;
            }
            set
            {
                ViewState["QueryDateRangStart"] = value;
            }
        }

        /// <summary>
        /// 儲存查詢的日期區間迄日
        /// </summary>
        private DateTime? QueryDateRangEnd
        {
            get
            {
                return ViewState["QueryDateRangEnd"] as DateTime?;
            }
            set
            {
                ViewState["QueryDateRangEnd"] = value;
            }
        }

        /// <summary>
        /// 儲存查詢的欄位種類 (StuId=學號 / CancelNo=銷帳編號 / StuName=姓名)
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
        /// 儲存查詢的欄位值
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
            where = null;
            orderbys = null;

            #region 5 Key 必要條件
            string receiveType = this.QueryReceiveType;
            if (String.IsNullOrEmpty(receiveType))
            {
                return new XmlResult(false, "請指定商家代號");
            }
            string yearId = this.QueryYearId;
            if (String.IsNullOrEmpty(yearId))
            {
                return new XmlResult(false, "請指定學年");
            }
            string termId = this.QueryTermId;
            if (String.IsNullOrEmpty(termId))
            {
                return new XmlResult(false, "請指定學期");
            }

            #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
            //if (String.IsNullOrEmpty(this.QueryDepId))
            //{
            //    return new XmlResult(false, "請指定部別");
            //}
            #endregion

            string depId = this.QueryDepId;

            string receiveId = this.QueryReceiveId;
            if (String.IsNullOrEmpty(receiveId))
            {
                return new XmlResult(false, "請指定代收費用別");
            }

            where = new Expression(HistoryView.Field.ReceiveType, receiveType)
                        .And(HistoryView.Field.YearId, yearId)
                        .And(HistoryView.Field.TermId, termId)
                        .And(HistoryView.Field.DepId, depId)
                        .And(HistoryView.Field.ReceiveId, receiveId);
            #endregion

            #region 部別 條件
            string deptId = this.QueryDeptId;
            if (!String.IsNullOrEmpty(deptId))
            {
                where.And(HistoryView.Field.DeptId, deptId);
            }
            #endregion

            #region 院別 條件
            string collegeId = this.QueryCollegeId;
            if (!String.IsNullOrEmpty(collegeId))
            {
                where.And(HistoryView.Field.CollegeId, collegeId);
            }
            #endregion

            #region 科系 條件
            string majorId = this.QueryMajorId;
            if (!String.IsNullOrEmpty(majorId))
            {
                where.And(HistoryView.Field.MajorId, majorId);
            }
            #endregion

            #region 年級 條件
            string stuGrade = this.QueryStuGrade;
            if (!String.IsNullOrEmpty(stuGrade))
            {
                where.And(HistoryView.Field.StuGrade, stuGrade);
            }
            #endregion

            #region 班別 條件
            string classId = this.QueryClassId;
            if (!String.IsNullOrEmpty(classId))
            {
                where.And(HistoryView.Field.ClassId, classId);
            }
            #endregion

            #region 批號 條件
            string upNo = this.QueryUpNo;
            if (!String.IsNullOrEmpty(upNo))
            {
                where.And(HistoryView.Field.UpNo, upNo);
            }
            #endregion

            #region 銷帳狀態 條件
            {
                switch (this.QueryCancelStatus)
                {
                    case CancelStatusCodeTexts.NONPAY:
                        //未繳款
                        where
                            .And((new Expression(HistoryView.Field.ReceiveDate, String.Empty)).Or(HistoryView.Field.ReceiveDate, null))
                            .And((new Expression(HistoryView.Field.ReceiveWay, String.Empty)).Or(HistoryView.Field.ReceiveWay, null));
                        break;
                    case CancelStatusCodeTexts.PAYED:
                        //已繳款(未入帳)
                        where
                            .And(HistoryView.Field.ReceiveDate, RelationEnum.NotEqual, String.Empty)
                            .And(HistoryView.Field.ReceiveDate, RelationEnum.NotEqual, null)
                            .And((new Expression(HistoryView.Field.AccountDate, String.Empty)).Or(HistoryView.Field.AccountDate, null));
                        break;
                    case CancelStatusCodeTexts.CANCELED:
                        //已入帳
                        where
                            .And(HistoryView.Field.AccountDate, RelationEnum.NotEqual, String.Empty)
                            .And(HistoryView.Field.AccountDate, RelationEnum.NotEqual, null);
                        break;
                }
            }
            #endregion

            if (this.QueryCancelStatus != CancelStatusCodeTexts.NONPAY)
            {
                #region 繳款方式 條件
                string receiveWay = this.QueryReceiveWay;
                if (!String.IsNullOrEmpty(receiveWay))
                {
                    where.And(StudentReceiveView.Field.ReceiveWay, this.QueryReceiveWay);
                }
                #endregion

                #region 日期區間 (ReceiveDate=代收日區間 / AccountDate=入帳日區間)
                string dateRangeKindName = null;
                string dateField = null;
                switch (this.QueryDateRangeKind)
                {
                    case "ReceiveDate":
                        dateRangeKindName = "代收日區間";
                        dateField = HistoryView.Field.ReceiveDate;
                        break;
                    case "AccountDate":
                        dateRangeKindName = "入帳日區間";
                        dateField = HistoryView.Field.AccountDate;
                        break;
                }
                if (!String.IsNullOrEmpty(dateRangeKindName))
                {
                    DateTime? sDate = this.QueryDateRangStart;
                    DateTime? eDate = this.QueryDateRangStart;
                    if (sDate == null)
                    {
                        return new XmlResult(false, String.Format("請指定{0}的起日", dateRangeKindName));
                    }
                    if (eDate == null)
                    {
                        return new XmlResult(false, String.Format("請指定{0}的迄日", dateRangeKindName));
                    }
                    if (eDate.Value < sDate.Value)
                    {
                        return new XmlResult(false, String.Format("{0}的起日不可大於迄日", dateRangeKindName));
                    }

                    where
                        .And(dateField, RelationEnum.GreaterEqual, Common.GetTWDate7(sDate.Value))
                        .And(dateField, RelationEnum.LessEqual, Common.GetTWDate7(eDate.Value));
                }
                #endregion
            }

            #region 查詢欄位與值 條件 (StuId=學號 / CancelNo=銷帳編號 / StuName=姓名)
            {
                string searchValue = this.QuerySearchValue;
                if (!String.IsNullOrEmpty(searchValue))
                {
                    switch (this.QuerySearchField)
                    {
                        case "StuId":
                            where.And(HistoryView.Field.StuId, searchValue);
                            break;
                        case "CancelNo":
                            where.And(HistoryView.Field.CancelNo, searchValue);
                            break;
                        case "StuName":
                            where.And(HistoryView.Field.StuName, searchValue);
                            break;
                        default:
                            return new XmlResult(false, "請指定查詢欄位");
                    }
                }
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(HistoryView.Field.StuId, OrderByEnum.Asc);

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

            XmlResult xmlResult = base.QueryDataAndBind<HistoryView>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }

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

            #region 代收費用別預設為第一個有效的選項
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
            //this.QueryDepId = depId;
            this.QueryReceiveId = receiveId;

            #region 銷帳狀態選項
            WebHelper.SetDropDownListItems(this.ddlCancelStatus, DefaultItem.Kind.All, false, new CancelStatusCodeTexts(), false, true, 0, null);
            #endregion

            #region 繳款方式
            {
                Expression where = new Expression();
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(ChannelSetEntity.Field.ChannelId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { ChannelSetEntity.Field.ChannelId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { ChannelSetEntity.Field.ChannelName };
                string textCombineFormat = null;

                CodeText[] datas = null;
                XmlResult xmlResult2 = DataProxy.Current.GetEntityOptions<ChannelSetEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);

                WebHelper.SetDropDownListItems(this.ddlReceiveWay, DefaultItem.Kind.All, false, datas, true, false, 0, null);
            }
            #endregion

            #region 查詢特定欄位值
            {
                CodeText[] items = new CodeText[] { 
                    new CodeText("StuId", "學號"), new CodeText("CancelNo", "虛擬帳號"), new CodeText("StuName", "姓名")
                };
                WebHelper.SetRadioButtonListItems(this.rdoSearchField, items, true, 2, items[0].Code);
            }
            #endregion

            #region 年級
            {
                WebHelper.SetDropDownListItems(this.ddlGrade, DefaultItem.Kind.All, false, new GradeCodeTexts(), false, false, 0, null);
            }
            #endregion

            #region 查詢結果初始化
            {
                this.gvResult.Visible = false;
                //this.gvResult.DataSource = null;
                //this.gvResult.DataBind();
                Paging[] ucPagings = this.GetPagingControls();
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = false;
                }
            }
            #endregion

            this.ucFilter2_ItemSelectedIndexChanged(this.ucFilter2, null);

            return true;
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>成功傳回 true，否則傳回 false</returns>
        private bool GetAndKeepQueryCondition()
        {
            #region 查詢的 5 Key 必要條件
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

            #region 記住 5 Key
            WebHelper.SetFilterArguments(qReceiveType, qYearId, qTermId, qDepId, qReceiveId);
            #endregion
            #endregion

            #region 查詢的銷帳狀態、繳款方式、日期區間
            string qCancelStatus = this.ddlCancelStatus.SelectedValue;
            string qReceiveWay = null;
            string qDateRangKind = null;
            string qDateRangKindName = null;
            DateTime? qDateRangStart = null;
            DateTime? qDateRangEnd = null;

            #region 查詢的繳款方式
            if (qCancelStatus != CancelStatusCodeTexts.NONPAY)
            {
                qReceiveWay = this.ddlReceiveWay.SelectedValue;
            }
            #endregion

            #region 日期區間
            switch (qCancelStatus)
            {
                case CancelStatusCodeTexts.PAYED:
                    if (this.rbtReceiveDate.Checked)
                    {
                        qDateRangKind = "ReceiveDate";
                        qDateRangKindName = "代收日區間";
                    }
                    else if (this.rbtAccountDate.Checked)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized(String.Format("「銷帳狀態」指定為{0}時，不可指定查詢「入帳日區間」", CancelStatusCodeTexts.PAYED_TEXT));
                        this.ShowJsAlert(msg);
                        return false;
                    }
                    break;
                case CancelStatusCodeTexts.CANCELED:
                    if (this.rbtReceiveDate.Checked)
                    {
                        qDateRangKind = "ReceiveDate";
                        qDateRangKindName = "代收日區間";
                    }
                    else if (this.rbtAccountDate.Checked)
                    {
                        qDateRangKind = "AccountDate";
                        qDateRangKindName = "入帳日區間";
                    }
                    break;
            }
            if (!String.IsNullOrEmpty(qDateRangKind))
            {
                #region 起日
                DateTime sDate;
                {
                    string txt = this.tbxQuerySDate.Text.Trim();
                    if (String.IsNullOrEmpty(txt))
                    {
                        //[TODO] 固定顯示訊息的收集
                        this.ShowMustInputAlert(String.Format("「{0}」的起日", qDateRangKindName));
                        return false;
                    }
                    else
                    {
                        if (DateTime.TryParse(txt, out sDate) && sDate.Year >= 1911)
                        {
                            qDateRangStart = sDate;
                        }
                        else
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(String.Format("「{0}」的起日不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)", qDateRangKindName));
                            this.ShowJsAlert(msg);
                            return false;
                        }
                    }
                }
                #endregion

                #region 迄日
                DateTime eDate;
                {
                    string txt = this.tbxQueryEDate.Text.Trim();
                    if (String.IsNullOrEmpty(txt))
                    {
                        //[TODO] 固定顯示訊息的收集
                        this.ShowMustInputAlert(String.Format("「{0}」的迄日", qDateRangKindName));
                        return false;
                    }
                    else
                    {
                        if (DateTime.TryParse(txt, out eDate) && eDate.Year >= 1911)
                        {
                            qDateRangEnd = eDate;
                        }
                        else
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(String.Format("「{0}」的迄日不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)", qDateRangKindName));
                            this.ShowJsAlert(msg);
                            return false;
                        }
                    }
                }
                #endregion

                if (sDate > eDate)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized(String.Format("「{0}」的起日不可大於迄日", qDateRangKindName));
                    this.ShowJsAlert(msg);
                    return false;
                }
            }
            #endregion
            #endregion

            #region 5 Key 條件
            this.QueryReceiveType = qReceiveType;
            this.QueryYearId = qYearId;
            this.QueryTermId = qTermId;
            //this.QueryDepId = qDepId;
            this.QueryReceiveId = qReceiveId;
            #endregion

            #region 部別 條件
            this.QueryDeptId = this.ddlDept.SelectedValue;
            #endregion

            #region 院別 條件
            this.QueryCollegeId = this.ddlCollege.SelectedValue;
            #endregion

            #region 科系 條件
            this.QueryMajorId = this.ddlMajor.SelectedValue;
            #endregion

            #region 年級 條件
            this.QueryStuGrade = this.ddlGrade.SelectedValue;
            #endregion

            #region 班別 條件
            this.QueryClassId = this.ddlClass.SelectedValue;
            #endregion

            #region 批號 條件
            this.QueryUpNo = this.ddlUpNo.SelectedValue;
            #endregion

            #region 銷帳狀態 條件
            this.QueryCancelStatus = qCancelStatus;
            #endregion

            #region 繳款方式 條件
            this.QueryReceiveWay = qReceiveWay;
            #endregion

            #region 日期區間 條件
            this.QueryDateRangeKind = qDateRangKind;
            this.QueryDateRangStart = qDateRangStart;
            this.QueryDateRangEnd = qDateRangEnd;
            #endregion

            #region 欄位 條件
            string qSearchValue = this.tbxSearchValue.Text.Trim();
            if (String.IsNullOrEmpty(qSearchValue))
            {
                this.QuerySearchValue = String.Empty;
                this.QuerySearchField = null;
            }
            else
            {
                this.QuerySearchValue = qSearchValue;
                this.QuerySearchField = this.rdoSearchField.SelectedValue;
            }
            #endregion

            return true;
        }

        private bool GetAndBindAllOptions(string receiveType, string yearId, string termId, string depId, string receiveId)
        {
            CodeText[] deptDatas = null;
            CodeText[] collegeDatas = null;
            CodeText[] majorDatas = null;
            CodeText[] classDatas = null;
            CodeText[] upNoDatas = null;
            XmlResult xmlResult = DataProxy.Current.GetC3700009AllOptions(this.Page, receiveType, yearId, termId, depId, receiveId
                , out deptDatas, out collegeDatas, out majorDatas, out classDatas, out upNoDatas);

            WebHelper.SetDropDownListItems(this.ddlDept, DefaultItem.Kind.All, false, deptDatas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlCollege, DefaultItem.Kind.All, false, collegeDatas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlMajor, DefaultItem.Kind.All, false, majorDatas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlClass, DefaultItem.Kind.All, false, classDatas, true, false, 0, null);

            #region [MDY:2018xxxx] 批號改用數值遞減排序
            if (upNoDatas != null)
            {
                WebHelper.SortItemsByValueDesc(ref upNoDatas);
            }
            #endregion
            WebHelper.SetDropDownListItems(this.ddlUpNo, DefaultItem.Kind.All, false, upNoDatas, false, false, 0, null);

            return xmlResult.IsSuccess;
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

            string depId = "";

            string receiveId = this.ucFilter2.SelectedReceiveID;

            this.GetAndBindAllOptions(this.QueryReceiveType, this.QueryYearId, this.QueryTermId, depId, receiveId);
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

        protected void ccbtnExport_Click(object sender, EventArgs e)
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

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            string extName = "XLS";
            {
                LinkButton control = sender as LinkButton;
                if (control.CommandArgument == "ODS")
                {
                    extName = "ODS";
                }
            }
            #endregion

            if (this.GetAndKeepQueryCondition())
            {
                Expression where = null;
                KeyValueList<OrderByEnum> orderbys = null;
                XmlResult xmlResult = this.GetWhereAndOrderBys(out where, out orderbys);
                if (xmlResult.IsSuccess)
                {
                    string funcId = "C3700009";
                    byte[] fileContent = null;

                    #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                    xmlResult = DataProxy.Current.ExportQueryResult(this.Page, funcId, where, out fileContent, (extName == "ODS"));
                    #endregion

                    if (xmlResult.IsSuccess)
                    {
                        if (fileContent == null || fileContent.Length == 0)
                        {
                            this.ShowJsAlert("查無資料");
                        }
                        else
                        {
                            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                            #region [MDY:20210401] 原碼修正
                            string fileName = String.Format("{0}_歷史繳費資料查詢結果.{1}", HttpUtility.UrlEncode(this.QueryReceiveType), extName);
                            #endregion
                            this.ResponseFile(fileName, fileContent, extName);
                            #endregion
                        }
                    }
                }

                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("匯出查詢結果");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            HistoryView[] datas = this.gvResult.DataSource as HistoryView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                HistoryView data = datas[row.RowIndex];
                //資料參數
                string argument = data.SN.ToString();

                LinkButton lbtnDetail = row.FindControl("lbtnDetail") as LinkButton;
                if (lbtnDetail != null)
                {
                    lbtnDetail.CommandArgument = argument;
                }
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
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

            #region 處理資料參數
            Int64 sn = 0;
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument) || !Int64.TryParse(argument, out sn) || sn < 1)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            switch (e.CommandName)
            {
                case "Detail":
                    #region 明細
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Query);
                        QueryString.Add("SN", argument);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("C3700009D.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
            }
        }
    }
}