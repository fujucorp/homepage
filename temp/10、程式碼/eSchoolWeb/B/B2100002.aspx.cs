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
    /// 維護學生繳費資料
    /// </summary>
    public partial class B2100002 : PagingBasePage
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
        /// 儲存查詢的學號參數 (跨費用別專用)
        /// </summary>
        private string QueryStuId
        {
            get
            {
                return ViewState["QueryStuId"] as string;
            }
            set
            {
                ViewState["QueryStuId"] = value == null ? null : value.Trim();
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
        /// 儲存查詢的代收日區間起日
        /// </summary>
        private string QueryReceiveDateStart
        {
            get
            {
                return ViewState["QueryReceiveDateStart"] as string;
            }
            set
            {
                ViewState["QueryReceiveDateStart"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的代收日區間迄日
        /// </summary>
        private string QueryReceiveDateEnd
        {
            get
            {
                return ViewState["QueryReceiveDateEnd"] as string;
            }
            set
            {
                ViewState["QueryReceiveDateEnd"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的入帳日區間起日
        /// </summary>
        private string QueryAccountDateStart
        {
            get
            {
                return ViewState["QueryAccountDateStart"] as string;
            }
            set
            {
                ViewState["QueryAccountDateStart"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的入帳日區間迄日
        /// </summary>
        private string QueryAccountDateEnd
        {
            get
            {
                return ViewState["QueryAccountDateEnd"] as string;
            }
            set
            {
                ViewState["QueryAccountDateEnd"] = value == null ? null : value.Trim();
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

        /// <summary>
        /// 儲存查詢結果是否顯示費用別名稱
        /// </summary>
        private bool IsShowReceiveName
        {
            get
            {
                object value = ViewState["IsShowReceiveName"];
                return value is bool ? (bool)value : false;
            }
            set
            {
                ViewState["IsShowReceiveName"] = value;
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
            #region [New] 費用別選擇全部時，只提供學號的查詢，且必須設定學號查詢條件
            if (String.IsNullOrEmpty(this.QueryReceiveId))
            {
                where = new Expression(StudentView.Field.ReceiveType, this.QueryReceiveType)
                    .And(StudentView.Field.YearId, this.QueryYearId)
                    .And(StudentView.Field.TermId, this.QueryTermId)
                    .And(StudentView.Field.DepId, this.QueryDepId)
                    .And(StudentView.Field.StuId, this.QueryStuId);

                orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(StudentView.Field.ReceiveId, OrderByEnum.Asc);

                return new XmlResult(true);
            }
            #endregion

            #region 5 Key 條件
            where = new Expression(StudentView.Field.ReceiveType, this.QueryReceiveType)
                        .And(StudentView.Field.YearId, this.QueryYearId)
                        .And(StudentView.Field.TermId, this.QueryTermId)
                        .And(StudentView.Field.DepId, this.QueryDepId)
                        .And(StudentView.Field.ReceiveId, this.QueryReceiveId);
            #endregion

            #region 科系 條件
            if (!String.IsNullOrEmpty(this.QueryMajorId))
            {
                where.And(StudentView.Field.MajorId, this.QueryMajorId);
            }
            #endregion

            #region 年級 條件
            if (!String.IsNullOrEmpty(this.QueryStuGrade))
            {
                where.And(StudentView.Field.StuGrade, this.QueryStuGrade);
            }
            #endregion

            #region 批號 條件
            if (!String.IsNullOrEmpty(this.QueryUpNo))
            {
                where.And(StudentView.Field.UpNo, this.QueryUpNo);
            }
            #endregion

            #region 銷帳狀態 條件
            {
                switch (this.QueryCancelStatus)
                {
                    case CancelStatusCodeTexts.NONPAY:
                        //未繳款
                        where
                            .And((new Expression(StudentView.Field.CancelFlag, String.Empty)).Or(StudentView.Field.CancelFlag, null))
                            .And((new Expression(StudentView.Field.ReceiveDate, String.Empty)).Or(StudentView.Field.ReceiveDate, null));
                        break;
                    case CancelStatusCodeTexts.PAYED:
                        //已繳款(未入帳)
                        where
                            .And(StudentView.Field.ReceiveDate, RelationEnum.NotEqual, String.Empty)
                            .And(StudentView.Field.ReceiveDate, RelationEnum.NotEqual, null)
                            .And((new Expression(StudentView.Field.AccountDate, String.Empty)).Or(StudentView.Field.AccountDate, null));
                        break;
                    case CancelStatusCodeTexts.CANCELED:
                        //已入帳
                        where
                            .And(StudentView.Field.AccountDate, RelationEnum.NotEqual, String.Empty)
                            .And(StudentView.Field.AccountDate, RelationEnum.NotEqual, null);
                        break;
                }
            }
            #endregion

            if (this.QueryCancelStatus != CancelStatusCodeTexts.NONPAY)
            {
                #region 繳款方式 條件
                if (!String.IsNullOrEmpty(this.QueryReceiveWay))
                {
                    where.And(StudentReceiveView.Field.ReceiveWay, this.QueryReceiveWay);

                    #region [Old]
                    //switch (this.QueryReceiveWay)
                    //{
                    //    case "1":   //超商
                    //        where.And(StudentView.Field.ReceiveWay, ChannelHelper.SM_HILI);
                    //        break;
                    //    case "2":   //ATM
                    //        where.And(StudentView.Field.ReceiveWay, new string[] { ChannelHelper.ATM, ChannelHelper.ATMA });
                    //        break;
                    //    case "3":   //臨櫃
                    //        where.And(StudentView.Field.ReceiveWay, ChannelHelper.TABS);
                    //        break;
                    //}
                    #endregion
                }
                #endregion

                #region 代收日區間 條件
                {
                    if (!String.IsNullOrEmpty(this.QueryReceiveDateStart))
                    {
                        where.And(StudentView.Field.ReceiveDate, RelationEnum.GreaterEqual, this.QueryReceiveDateStart);
                    }
                    if (!String.IsNullOrEmpty(this.QueryReceiveDateEnd))
                    {
                        where.And(StudentView.Field.ReceiveDate, RelationEnum.LessEqual, this.QueryReceiveDateEnd);
                    }
                }
                #endregion

                #region 入帳日區間 條件
                {
                    if (!String.IsNullOrEmpty(this.QueryAccountDateStart))
                    {
                        where.And(StudentView.Field.AccountDate, RelationEnum.GreaterEqual, this.QueryAccountDateStart);
                    }
                    if (!String.IsNullOrEmpty(this.QueryAccountDateEnd))
                    {
                        where.And(StudentView.Field.AccountDate, RelationEnum.LessEqual, this.QueryAccountDateEnd);
                    }
                }
                #endregion
            }

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
                        case "StuName":
                            where.And(StudentView.Field.Name, qValue);
                            break;
                    }
                }
            }
            #endregion

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

            if (String.IsNullOrEmpty(this.QueryReceiveId))
            {
                this.gvResult.Columns[0].Visible = true;   //查詢條件無費用別，則查詢結果要顯示未選擇
            }
            else
            {
                this.gvResult.Columns[0].Visible = false;    //查詢條件有費用別，則查詢結果不顯示未選擇
            }

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

            #region 處理合計資料
            {
                KeyValue<Decimal>[] sumDatas = null;
                if (xmlResult.IsSuccess)
                {
                    Expression where = null;
                    KeyValueList<OrderByEnum> orderbys = null;
                    XmlResult xmlResult2 = this.GetWhereAndOrderBys(out where, out orderbys);
                    if (xmlResult2.IsSuccess)
                    {
                        xmlResult2 = DataProxy.Current.GetB2100002Summary(this.Page, where, out sumDatas);
                    }
                    if (!xmlResult2.IsSuccess)
                    {
                        this.ShowActionFailureMessage("合計資料", xmlResult2.Code, xmlResult2.Message);
                    }
                }
                if (sumDatas == null)
                {
                    this.tabSummary.Visible = false;
                }
                else
                {
                    this.tabSummary.Visible = true;
                    foreach(KeyValue<Decimal> sumData in sumDatas)
                    {
                        switch(sumData.Key)
                        {
                            case "DataCount":
                                this.labDataCount.Text = sumData.Value.ToString("0");
                                break;
                            case "SumAmount":
                                this.labSumAmount.Text = DataFormat.GetAmountCommaText(sumData.Value);
                                break;
                        }
                    }
                }
            }
            #endregion

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
            #endregion

            #region [MDY:2018xxxx] 取消 Stu QueryString 參數，改用 Session 的 RedirectFrom 參數判斷是否由 B2100001.aspx 轉過來的，針對某學號的新增結束
            bool isFixStuId = false;
            {
                KeyValueList<string> queryStrings = Session["QueryString"] as KeyValueList<string>;
                if (queryStrings != null && queryStrings.Count > 0)
                {
                    if (queryStrings.TryGetValue("RedirectFrom", null) == "B2100001")
                    {
                        #region [MDY:20220410] Checkmarx 調整
                        this.tbxStuId.Text = HttpUtility.HtmlEncode(queryStrings.TryGetValue("StuId", String.Empty).Trim());
                        #endregion

                        isFixStuId = (this.tbxStuId.Text.Length > 0 && String.IsNullOrEmpty(receiveId));
                    }
                }
            }
            #endregion

            #region [MDY:2018xxxx] 非固定學號，費用別預設為第一個有效的選項
            if (!isFixStuId && String.IsNullOrEmpty(receiveId))
            {
                CodeTextList receiveItems = ucFilter2.GetReceiveItems();
                if (receiveItems.Count > 0)
                {
                    receiveId = receiveItems[0].Code;
                    ucFilter2.ChangeSelectedReceiveId(receiveId);

                    //一定要用這個方法將商家代號、學年、學期、部別、代收費用別參數傳給下一頁
                    //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
                    WebHelper.SetFilterArguments(receiveType, yearId, termId, depId, receiveId);
                }
            }
            #endregion

            this.QueryReceiveType = receiveType;
            this.QueryYearId = yearId;
            this.QueryTermId = termId;
            this.QueryDepId = depId;
            this.QueryReceiveId = receiveId;

            this.tabQuery1.Visible = true;
            this.tabQuery2.Visible = false;
            this.tabInsert.Visible = false;

            #region 銷帳狀態選項
            #region [MDY:20181116] 因為 checkmarx 會誤判所以使用 Overload 不回傳值的方法
            #region [OLD]
            //WebHelper.SetDropDownListItems(this.ddlCancelStatus, DefaultItem.Kind.All, false, new CancelStatusCodeTexts(), false, true, 0, null);
            #endregion

            WebHelper.SetDropDownListItems(this.ddlCancelStatus, new CancelStatusCodeTexts(), DefaultItem.Kind.All, false, false, true, 0, null, true);
            #endregion
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

                #region [MDY:20181116] 因為 checkmarx 會誤判所以使用 Overload 不回傳值的方法
                #region [OLD]
                ////CodeText[] items = new CodeText[] { new CodeText("1", "超商"), new CodeText("2", "ATM"), new CodeText("3", "臨櫃") };
                //WebHelper.SetDropDownListItems(this.ddlReceiveWay, DefaultItem.Kind.All, false, datas, true, false, 0, null);
                #endregion

                WebHelper.SetDropDownListItems(this.ddlReceiveWay, datas, DefaultItem.Kind.All, false, true, false, 0, null, true);
                #endregion
            }
            #endregion

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
                #region [MDY:20181116] 因為 checkmarx 會誤判所以使用 Overload 不回傳值的方法
                #region [OLD]
                //WebHelper.SetDropDownListItems(this.ddlGrade, DefaultItem.Kind.All, false, new GradeCodeTexts(), false, false, 0, null);
                #endregion

                #region [MDY:202203XX] 2022擴充案 年級英文名稱相關
                bool useEngDataUI = this.UseEngDataUI(this.QueryReceiveType, !this.IsPostBack);
                WebHelper.SetDropDownListItems(this.ddlGrade, new GradeCodeTexts(useEngDataUI), DefaultItem.Kind.All, false, false, false, 0, null, true);
                #endregion
                #endregion
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

            this.tabSummary.Visible = false;

            this.ucFilter2_ItemSelectedIndexChanged(this.ucFilter2, null);

            #region [MDY:2018xxxx] 將此判斷移至前面，修正永遠不會執行 ccbtnQuery_Click() 的問題
            #region [OLD]
            //#region 如果有 Stu 參數表示由 B2100001.aspx 轉過來的，針對某學號的新增結束
            //string qStuId = this.Request.QueryString["Stu"];
            //if (!String.IsNullOrWhiteSpace(qStuId))
            //{
            //    this.tbxStuId.Text = qStuId.Trim();
            //    if (String.IsNullOrEmpty(this.QueryReceiveId))
            //    {
            //        this.ccbtnQuery_Click(this.ccbtnQuery, EventArgs.Empty);
            //    }
            //}
            //#endregion
            #endregion

            if (isFixStuId)
            {
                this.ccbtnQuery_Click(this.ccbtnQuery, EventArgs.Empty);
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 切換查詢條件介面
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        private void ChangeQueryUI(string receiveType, string yearId, string termId, string depId, string receiveId)
        {
            if (String.IsNullOrEmpty(receiveId))
            {
                this.tabQuery1.Visible = false;
                this.tabQuery2.Visible = true;
                this.IsShowReceiveName = true;   //查詢條件無費用別，則查詢結果要顯示未選擇
            }
            else
            {
                this.tabQuery1.Visible = true;
                this.tabQuery2.Visible = false;
                this.IsShowReceiveName = false;    //查詢條件有費用別，則查詢結果不顯示未選擇
                this.GetAndBindUpNoOptions(receiveType, yearId, termId, depId, receiveId);
            }
            this.tabInsert.Visible = false;
        }

        private void BindInsertUI(List<string> hasDataReceiveIds)
        {
            CodeTextList items = this.ucFilter2.GetReceiveItems();
            if (hasDataReceiveIds != null && hasDataReceiveIds.Count > 0)
            {
                foreach (string receiveId in hasDataReceiveIds)
                {
                    items.RemoveAll(x => x.Code == receiveId);
                }
            }
            if (items.Count > 0 && String.IsNullOrEmpty(this.QueryReceiveId))
            {
                #region [MDY:20181116] 因為 checkmarx 會誤判所以使用 Overload 不回傳值的方法
                #region [OLD]
                //WebHelper.SetDropDownListItems(this.ddlReceiveId, DefaultItem.Kind.Select, false, items, false, false, 0, null);
                #endregion

                WebHelper.SetDropDownListItems(this.ddlReceiveId, items, DefaultItem.Kind.Select, false, false, false, 0, null, true);
                #endregion

                this.tabInsert.Visible = true;
            }
            else
            {
                this.ddlReceiveId.Items.Clear();
                this.tabInsert.Visible = false;
            }
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

            #region [Old] 費用別選擇全部時，只提供學號的查詢，且必須設定學號查詢條件
            //if (String.IsNullOrEmpty(qReceiveId))
            //{
            //    this.ShowMustInputAlert("代收費用別");
            //    return false;
            //}
            #endregion

            #endregion

            #region 記住 5 Key
            WebHelper.SetFilterArguments(qReceiveType, qYearId, qTermId, qDepId, qReceiveId);
            #endregion

            #region [New] 費用別選擇全部時，只提供學號的查詢，且必須設定學號查詢條件
            if (String.IsNullOrEmpty(qReceiveId))
            {
                string qStuId = this.tbxStuId.Text.Trim();
                if (String.IsNullOrEmpty(qStuId))
                {
                    this.ShowMustInputAlert("學號");
                    return false;
                }

                this.QueryReceiveType = qReceiveType;
                this.QueryYearId = qYearId;
                this.QueryTermId = qTermId;
                this.QueryDepId = qDepId;
                this.QueryReceiveId = qReceiveId;
                this.QueryStuId = qStuId;

                this.QueryCancelStatus = null;
                this.QueryReceiveWay = null;
                this.QueryReceiveDateStart = null;
                this.QueryReceiveDateEnd = null;
                this.QueryAccountDateStart = null;
                this.QueryAccountDateEnd = null;

                #region 查詢的科系
                this.QueryMajorId = null;
                #endregion

                #region 查詢的年級
                this.QueryStuGrade = null;
                #endregion

                #region 查詢的批號
                this.QueryUpNo = null;
                #endregion

                #region 查詢的查詢欄位與值
                this.QuerySearchField = null;
                this.QuerySearchValue = null;
                #endregion

                return true;
            }
            #endregion

            this.QueryStuId = null;

            #region 查詢的銷帳狀態
            string qCancelStatus = this.ddlCancelStatus.SelectedValue;
            #endregion

            string qReceiveWay = null;
            string qReceiveDateStart = null;
            string qReceiveDateEnd = null;
            string qAccountDateStart = null;
            string qAccountDateEnd = null;

            if (qCancelStatus != CancelStatusCodeTexts.NONPAY)
            {
                #region 查詢的繳款方式
                qReceiveWay = this.ddlReceiveWay.SelectedValue;
                #endregion

                #region [Old] 代收日區間與入帳日區間條件改為二擇一
                //#region 查詢的代收日區間的起日
                //{
                //    string txt = this.tbxReceiveDateS.Text.Trim();
                //    if (!String.IsNullOrEmpty(txt))
                //    {
                //        DateTime date;
                //        if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                //        {
                //            qReceiveDateStart = Common.GetTWDate7(date);
                //        }
                //        else
                //        {
                //            //[TODO] 固定顯示訊息的收集
                //            string msg = this.GetLocalized("「代收日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                //            this.ShowJsAlert(msg);
                //            return false;
                //        }
                //    }
                //}
                //#endregion

                //#region 查詢的代收日區間的迄日
                //{
                //    string txt = this.tbxReceiveDateE.Text.Trim();
                //    if (!String.IsNullOrEmpty(txt))
                //    {
                //        DateTime date;
                //        if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                //        {
                //            qReceiveDateEnd = Common.GetTWDate7(date);
                //        }
                //        else
                //        {
                //            //[TODO] 固定顯示訊息的收集
                //            string msg = this.GetLocalized("「代收日區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                //            this.ShowJsAlert(msg);
                //            return false;
                //        }
                //    }
                //}
                //#endregion

                //if (qCancelStatus == CancelStatusCodeTexts.CANCELED || String.IsNullOrEmpty(qCancelStatus))
                //{
                //    #region 查詢的入帳日區間的起日
                //    {
                //        string txt = tbxAccountDateS.Text.Trim();
                //        if (!String.IsNullOrEmpty(txt))
                //        {
                //            DateTime date;
                //            if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                //            {
                //                qAccountDateStart = Common.GetTWDate7(date);
                //            }
                //            else
                //            {
                //                //[TODO] 固定顯示訊息的收集
                //                string msg = this.GetLocalized("「入帳日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                //                this.ShowJsAlert(msg);
                //                return false;
                //            }
                //        }
                //    }
                //    #endregion

                //    #region 查詢的入帳日區間的迄日
                //    {
                //        string txt = tbxAccountDateE.Text.Trim();
                //        if (!String.IsNullOrEmpty(txt))
                //        {
                //            DateTime date;
                //            if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                //            {
                //                qAccountDateEnd = Common.GetTWDate7(date);
                //            }
                //            else
                //            {
                //                //[TODO] 固定顯示訊息的收集
                //                string msg = this.GetLocalized("「入帳日區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                //                this.ShowJsAlert(msg);
                //                return false;
                //            }
                //        }
                //    }
                //    #endregion
                //}
                #endregion

                #region [New] 代收日區間與入帳日區間條件改為二擇一
                if (this.rbtReceiveDate.Checked)
                {
                    #region 查詢的代收日區間的起日
                    {
                        string txt = this.tbxReceiveDateS.Text.Trim();
                        if (String.IsNullOrEmpty(txt))
                        {
                            //[TODO] 固定顯示訊息的收集
                            this.ShowMustInputAlert("代收日區間的起日");
                            return false;
                        }
                        else
                        {
                            DateTime date;
                            if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                            {
                                qReceiveDateStart = Common.GetTWDate7(date);
                            }
                            else
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("「代收日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                    }
                    #endregion

                    #region 查詢的代收日區間的迄日
                    {
                        string txt = this.tbxReceiveDateE.Text.Trim();
                        if (String.IsNullOrEmpty(txt))
                        {
                            //[TODO] 固定顯示訊息的收集
                            this.ShowMustInputAlert("代收日區間的迄日");
                            return false;
                        }
                        else
                        {
                            DateTime date;
                            if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                            {
                                qReceiveDateEnd = Common.GetTWDate7(date);
                            }
                            else
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("「代收日區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                    }
                    #endregion
                }
                else if (this.rbtAccountDate.Checked && (qCancelStatus == CancelStatusCodeTexts.CANCELED || String.IsNullOrEmpty(qCancelStatus)))
                {
                    #region 查詢的入帳日區間的起日
                    {
                        string txt = tbxAccountDateS.Text.Trim();
                        if (String.IsNullOrEmpty(txt))
                        {
                            //[TODO] 固定顯示訊息的收集
                            this.ShowMustInputAlert("入帳日區間的起日");
                            return false;
                        }
                        else
                        {
                            DateTime date;
                            if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                            {
                                qAccountDateStart = Common.GetTWDate7(date);
                            }
                            else
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("「入帳日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                    }
                    #endregion

                    #region 查詢的入帳日區間的迄日
                    {
                        string txt = tbxAccountDateE.Text.Trim();
                        if (String.IsNullOrEmpty(txt))
                        {
                            //[TODO] 固定顯示訊息的收集
                            this.ShowMustInputAlert("入帳日區間的迄日");
                            return false;
                        }
                        else
                        {
                            DateTime date;
                            if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                            {
                                qAccountDateEnd = Common.GetTWDate7(date);
                            }
                            else
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("「入帳日區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                    }
                    #endregion
                }
                #endregion
            }

            this.QueryReceiveType = qReceiveType;
            this.QueryYearId = qYearId;
            this.QueryTermId = qTermId;
            this.QueryDepId = qDepId;
            this.QueryReceiveId = qReceiveId;

            this.QueryCancelStatus = qCancelStatus;
            this.QueryReceiveWay = qReceiveWay;
            this.QueryReceiveDateStart = qReceiveDateStart;
            this.QueryReceiveDateEnd = qReceiveDateEnd;
            this.QueryAccountDateStart = qAccountDateStart;
            this.QueryAccountDateEnd = qAccountDateEnd;

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

            #region [MDY:20181116] 因為 checkmarx 會誤判所以使用 Overload 不回傳值的方法
            #region [OLD]
            //WebHelper.SetDropDownListItems(this.ddlMajor, DefaultItem.Kind.All, false, items, false, false, 0, null);
            #endregion

            WebHelper.SetDropDownListItems(this.ddlMajor, items, DefaultItem.Kind.All, false, false, false, 0, null, true);
            #endregion
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

            #region [MDY:20181116] 因為 checkmarx 會誤判所以使用 Overload 不回傳值的方法
            #region [OLD]
            //WebHelper.SetDropDownListItems(this.ddlUpNo, DefaultItem.Kind.All, false, items, false, false, 0, null);
            #endregion

            WebHelper.SetDropDownListItems(this.ddlUpNo, items, DefaultItem.Kind.All, false, false, false, 0, null, true);
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ccbtnQuery.Visible = this.InitialUI();
            }
            else
            {
                #region [MDY:2018xxxx] PostBack 後就不需要 Session 參數，清除 Session 參數
                if (Session["QueryString"] != null)
                {
                    Session.Remove("QueryString");
                }
                #endregion
            }
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

        protected void ccbtnInsert_Click(object sender, EventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            string receiveId = this.ddlReceiveId.SelectedValue;
            if (String.IsNullOrEmpty(receiveId))
            {
                this.ShowMustInputAlert("學生繳費資料所屬費用別");
                return;
            }

            #region [MDY:2018xxxx] 取消 Stu QueryString 參數，改用 Session 的 RedirectFrom 參數表示由 B2100002.aspx 轉過來的，針對某學號的新增
            #region [OLD]
            //KeyValueList<string> QueryString = new KeyValueList<string>();
            //QueryString.Add("Action", ActionMode.Insert);
            //QueryString.Add("ReceiveType", this.QueryReceiveType);
            //QueryString.Add("YearId", this.QueryYearId);
            //QueryString.Add("TermId", this.QueryTermId);
            //QueryString.Add("DepId", this.QueryDepId);
            //QueryString.Add("ReceiveId", receiveId);
            //QueryString.Add("StuId", this.QueryStuId);
            //Session["QueryString"] = QueryString;

            //Server.Transfer("B2100001.aspx?Stu=" + this.QueryStuId);
            #endregion

            KeyValueList<string> queryStrings = new KeyValueList<string>();
            queryStrings.Add("RedirectFrom", "B2100002");
            queryStrings.Add("Action", ActionMode.Insert);
            queryStrings.Add("ReceiveType", this.QueryReceiveType);
            queryStrings.Add("YearId", this.QueryYearId);
            queryStrings.Add("TermId", this.QueryTermId);
            queryStrings.Add("DepId", this.QueryDepId);
            queryStrings.Add("ReceiveId", receiveId);
            queryStrings.Add("StuId", this.QueryStuId);
            Session["QueryString"] = queryStrings;

            Server.Transfer("B2100001.aspx");
            #endregion
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            StudentView[] datas = this.gvResult.DataSource as StudentView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            //儲存已有資料的費用別代碼
            List<string> hasDataReceiveIds = new List<string>();

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                StudentView data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1},{2},{3},{4},{5},{6}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.StuId, data.OldSeq);

                //收集已有資料的費用別代碼
                if (!hasDataReceiveIds.Contains(data.ReceiveId))
                {
                    hasDataReceiveIds.Add(data.ReceiveId);
                }

                CodeText cancelStatus = CancelStatusCodeTexts.GetCancelStatus(data.ReceiveDate, data.AccountDate);
                row.Cells[6].Text = this.GetLocalized(cancelStatus.Text);

                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                }

                MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
                if (ccbtnDelete != null)
                {
                    ccbtnDelete.CommandArgument = argument;
                    ccbtnDelete.Visible = (cancelStatus.Code == CancelStatusCodeTexts.NONPAY);
                }
            }

            this.BindInsertUI(hasDataReceiveIds);
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

            string url = "B2100002M.aspx";
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

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
            //this.QueryDepId = this.ucFilter2.SelectedDepId;
            #endregion

            this.QueryDepId = "";
            this.QueryReceiveId = this.ucFilter2.SelectedReceiveID;

            #region [Old] 費用別選擇全部時，只提供學號的查詢，且必須設定學號查詢條件
            //this.GetAndBindUpNoOptions(this.QueryReceiveType, this.QueryYearId, this.QueryTermId, this.QueryDepId, this.QueryReceiveId);
            #endregion

            #region [New] 費用別選擇全部時，只提供學號的查詢，且必須設定學號查詢條件
            this.ChangeQueryUI(this.QueryReceiveType, this.QueryYearId, this.QueryTermId, this.QueryDepId, this.QueryReceiveId);
            #endregion

            #region [MDY:2018xxxx] 清除查詢結果，避免顯示的查詢結果查詢條件不符
            {
                //因為有新增與回上一頁的按鈕，所以不適合把 divResult 隱藏
                //改為隱藏分頁按鈕，並結繫 null
                if (this.gvResult.Rows.Count > 0)
                {
                    this.gvResult.DataSource = null;
                    this.gvResult.DataBind();
                }
                Paging[] ucPagings = this.GetPagingControls();
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = false;
                }
            }
            #endregion
        }
    }
}