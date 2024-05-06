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

namespace eSchoolWeb.R
{
    public partial class R4100001 : PagingBasePage
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
        /// 儲存查詢的查詢類別 學號StuId 銷帳編號CancelNo 身分證字號IdNumber
        /// </summary>
        private string QuerySearchType
        {
            get
            {
                return ViewState["QuerySearchType"] as string;
            }
            set
            {
                ViewState["QuerySearchType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的查詢值
        /// </summary>
        private string QuerySearchString
        {
            get
            {
                return ViewState["QuerySearchString"] as string;
            }
            set
            {
                ViewState["QuerySearchString"] = value == null ? null : value.Trim();
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

            where = new Expression(StudentReturnView.Field.ReceiveType, this.QueryReceiveType);
            where.And(StudentReturnView.Field.YearId, this.QueryYearId);
            where.And(StudentReturnView.Field.TermId, this.QueryTermId);
            where.And(StudentReturnView.Field.DepId, this.QueryDepId);
            where.And(StudentReturnView.Field.ReceiveId, this.QueryReceiveId);

            #region 學號 | 銷帳編號 條件
            {
                string qData = this.QuerySearchString;
                if (!String.IsNullOrEmpty(qData))
                {
                    switch (this.QuerySearchType)
                    {
                        case "StuId":
                            where.And(StudentReturnView.Field.StuId, qData);
                            break;
                        case "CancelNo":
                            where.And(StudentReturnView.Field.CancelNo, qData);
                            break;
                    }
                }
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReturnView.Field.StuId, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<StudentReturnView>(pagingInfo, ucPagings, this.gvResult);
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
            #region Serach
            {
                CodeText[] items = new CodeText[] { new CodeText("StuId", "學號"), new CodeText("CancelNo", "虛擬帳號") };
                WebHelper.SetRadioButtonListItems(this.rdoSearchType, items, true, 2, items[0].Code);
            }
            #endregion

            #region 若是從修改頁回來時，要取上一次的查詢結果
            //查詢的查詢類別 學號StuId 銷帳編號CancelNo 身分證字號IdNumberQuerySearchType
            if (this.Request.QueryString["SearchType"] != null)
            {
                #region 處理五個下拉選項
                {
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
                        string msg = this.GetLocalized("無法取得商家代號、學年或學期參數");
                        this.ShowJsAlert(msg);
                        return false;
                    }

                    //因為控制項的 Page_Load 比頁面的 Page_Load 晚觸發，所以可以先執行 GetDataAndBind 方法，強迫 ucFilter1 結繫資料
                    //因為 ucFilter1 有指定 Filter2ControlID 為 ucFilter2，所以 ucFilter2 頁會被自動結繫資料
                    XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearID, termID, depID, receiveID);
                    if (xmlResult.IsSuccess)
                    {
                        #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
                        //depID = ucFilter2.SelectedDepID;
                        #endregion

                        depID = "";
                        receiveID = ucFilter2.SelectedReceiveID;
                    }

                    //一定要用這個方法將商家代號、學年、學期、部別、代收費用別參數傳給下一頁
                    //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
                    WebHelper.SetFilterArguments(receiveType, yearID, termID, depID, receiveID);

                    this.QueryReceiveType = receiveType;
                    this.QueryYearId = yearID;
                    this.QueryTermId = termID;
                    this.QueryDepId = depID;
                    this.QueryReceiveId = receiveID;
                }
                #endregion

                this.QuerySearchType = this.Request.QueryString["SearchType"].ToString();

                #region [MDY:20210401] 原碼修正
                #region [OLD]
                //rdoSearchType.SelectedValue = this.QuerySearchType;
                #endregion

                WebHelper.SetRadioButtonListSelectedValue(this.rdoSearchType, this.QuerySearchType);
                #endregion
            }
            else
            {
                #region 查詢結果初始化
                {
                    //因為有新增與回上一頁的按鈕，所以不適合把 divResult 隱藏
                    //改為隱藏分頁按鈕，並結繫 null
                    //this.divResult.Visible = false;
                    //this.gvResult.DataSource = null;
                    //this.gvResult.DataBind();
                    Paging[] ucPagings = this.GetPagingControls();
                    foreach (Paging ucPaging in ucPagings)
                    {
                        ucPaging.Visible = false;
                    }
                    return true;
                }
                #endregion
            }

            //查詢的查詢值
            if (this.Request.QueryString["SearchString"] != null)
            {
                this.QuerySearchString = this.Request.QueryString["SearchString"].ToString();

                #region [MDY:20210401] 原碼修正
                tbxSearchString.Text = HttpUtility.HtmlEncode(this.QuerySearchString);
                #endregion
            }
            else
            {
                return false;
            }

            PagingInfo pagingInfo = new PagingInfo(10, 0, 0);
            XmlResult xxmlResult = this.CallQueryDataAndBind(pagingInfo);

            if (!xxmlResult.IsSuccess)
            {
                this.ShowErrorMessage(xxmlResult.Code, xxmlResult.Message);
            }
            #endregion

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void lbtnQuery_Click(object sender, EventArgs e)
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

            #region 5 Key 必要條件
            string receiveType = this.ucFilter1.SelectedReceiveType;
            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }

            string yearId = this.ucFilter1.SelectedYearID;
            if (String.IsNullOrEmpty(yearId))
            {
                this.ShowMustInputAlert("學年");
                return;
            }

            string termId = this.ucFilter1.SelectedTermID;
            if (String.IsNullOrEmpty(termId))
            {
                this.ShowMustInputAlert("學期");
                return;
            }

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //string depId = this.ucFilter2.SelectedDepID;
            //if (String.IsNullOrEmpty(depId))
            //{
            //    this.ShowMustInputAlert("部別");
            //    return;
            //}
            #endregion

            string depId = String.Empty;

            string receiveId = this.ucFilter2.SelectedReceiveID;
            if (String.IsNullOrEmpty(receiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return;
            }

            this.QueryReceiveType = receiveType;
            this.QueryYearId = yearId;
            this.QueryTermId = termId;
            this.QueryDepId = depId;
            this.QueryReceiveId = receiveId;
            #endregion

            //查詢的查詢類別 學號StuId 銷帳編號CancelNo 身分證字號IdNumberQuerySearchType
            this.QuerySearchType = rdoSearchType.SelectedValue;

            //查詢的查詢值
            this.QuerySearchString = tbxSearchString.Text.Trim();

            PagingInfo pagingInfo = new PagingInfo(10, 0, 0);
            XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);

            if (!xmlResult.IsSuccess)
            {
                this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
            }
        }

        /// <summary>
        /// 新增退費資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnInsert_Click(object sender, EventArgs e)
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

            #region 5 Key
            string receiveType = this.ucFilter1.SelectedReceiveType;
            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }

            string yearId = this.ucFilter1.SelectedYearID;
            if (String.IsNullOrEmpty(yearId))
            {
                this.ShowMustInputAlert("學年");
                return;
            }

            string termId = this.ucFilter1.SelectedTermID;
            if (String.IsNullOrEmpty(termId))
            {
                this.ShowMustInputAlert("學期");
                return;
            }

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //string depId = this.ucFilter2.SelectedDepID;
            //if (String.IsNullOrEmpty(depId))
            //{
            //    this.ShowMustInputAlert("部別");
            //    return;
            //}
            #endregion

            string depId = String.Empty;

            string receiveId = this.ucFilter2.SelectedReceiveID;
            if (String.IsNullOrEmpty(receiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return;
            }
            #endregion

            #region 查詢欄位值
            string searchType = this.rdoSearchType.SelectedValue;
            string searchTypeName = null;
            switch (searchType)
            {
                case "StuId":
                    searchTypeName = "學號";
                    break;
                case "CancelNo":
                    searchTypeName = "虛擬帳號";
                    break;
                default:
                    string msg = this.GetLocalized("請選擇「學號」或「虛擬帳號」");
                    this.ShowSystemMessage(msg);
                    return;
            }
            string searchValue = this.tbxSearchString.Text.Trim();
            if (String.IsNullOrEmpty(searchValue))
            {
                this.ShowMustInputAlert(searchTypeName);
                return;
            }
            #endregion

            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                .And(StudentReceiveEntity.Field.YearId, yearId)
                .And(StudentReceiveEntity.Field.TermId, termId)
                .And(StudentReceiveEntity.Field.DepId, depId)
                .And(StudentReceiveEntity.Field.ReceiveId, receiveId);

            switch (searchType)
            {
                case "StuId":
                    where.And(StudentReturnView.Field.StuId, searchValue);
                    break;
                case "CancelNo":
                    where.And(StudentReturnView.Field.CancelNo, searchValue);
                    break;
            }

            //已入帳才能做退費
            where
                .And(StudentReceiveEntity.Field.AccountDate, RelationEnum.NotEqual, string.Empty)
                .And(StudentReceiveEntity.Field.AccountDate, RelationEnum.NotEqual, null);

            StudentReceiveEntity data = null;
            XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveEntity>(this.Page, where, null, out data);
            if (xmlResult.IsSuccess)
            {
                if (data != null)
                {
                    KeyValueList<string> QueryString = new KeyValueList<string>();
                    QueryString.Add("Action", ActionMode.Insert);
                    QueryString.Add("StuId", data.StuId);
                    QueryString.Add("CancelNo", data.CancelNo);
                    QueryString.Add("ReceiveType", data.ReceiveType);
                    QueryString.Add("YearId", data.YearId);
                    QueryString.Add("TermId", data.TermId);
                    QueryString.Add("DepId", data.DepId);
                    QueryString.Add("ReceiveId", data.ReceiveId);
                    QueryString.Add("OldSeq", data.OldSeq.ToString());
                    QueryString.Add("SearchType", this.QuerySearchType);
                    QueryString.Add("SearchString", this.QuerySearchString);
                    Session["QueryString"] = QueryString;

                    #region [MDY:20210521] 原碼修正
                    Server.Transfer(WebHelper.GenRNUrl("R4100001M.aspx"));
                    #endregion
                }
                else
                {
                    string action = this.GetLocalized("新增退費資料");
                    this.ShowActionFailureMessage(action, "查無此學生資料");
                    return;
                }
            }
            else
            {
                string action = this.GetLocalized("查詢學生繳費資料");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return;
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            StudentReturnView[] datas = this.gvResult.DataSource as StudentReturnView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                StudentReturnView data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", data.StuId, data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.OldSeq, data.CancelNo, data.DataNo);

                bool visible = String.IsNullOrWhiteSpace(data.SrNo);    //已有清冊批號的資料不可改

                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                    ccbtnModify.Visible = visible;
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
            if (args.Length != 9)
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
            string cancelno = args[7];
            string datano = args[8];
            #endregion

            string url = "R4100001M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("StuId", stuId);
                        QueryString.Add("CancelNo", cancelno);
                        QueryString.Add("ReceiveType", receiveType);
                        QueryString.Add("YearId", yearId);
                        QueryString.Add("TermId", termId);
                        QueryString.Add("DepId", depId);
                        QueryString.Add("ReceiveId", receiveId);
                        QueryString.Add("OldSeq", oldSeq);
                        QueryString.Add("DataNo", datano);
                        QueryString.Add("SearchType", this.QuerySearchType);
                        QueryString.Add("SearchString", this.QuerySearchString);
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
                        QueryString.Add("StuId", stuId);
                        QueryString.Add("CancelNo", cancelno);
                        QueryString.Add("ReceiveType", receiveType);
                        QueryString.Add("YearId", yearId);
                        QueryString.Add("TermId", termId);
                        QueryString.Add("DepId", depId);
                        QueryString.Add("ReceiveId", receiveId);
                        QueryString.Add("OldSeq", oldSeq);
                        QueryString.Add("DataNo", datano);
                        QueryString.Add("SearchType", this.QuerySearchType);
                        QueryString.Add("SearchString", this.QuerySearchString);
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