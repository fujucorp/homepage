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
    /// 整批刪除學生資料
    /// </summary>
    public partial class B2100004 : PagingBasePage
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
        /// 儲存查詢的銷帳編號
        /// </summary>
        private string QueryCancelNo
        {
            get
            {
                return ViewState["QueryCancelNo"] as string;
            }
            set
            {
                ViewState["QueryCancelNo"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的學號
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

        #region [MDY:20160123] 改存查詢結果的 PKeys
        #region [Old]
        ///// <summary>
        ///// 儲存查詢的StudentReceive
        ///// </summary>
        //private StudentView[] ResultStudentViews
        //{
        //    get
        //    {
        //        return ViewState["ResultStudentViews"] as StudentView[];
        //    }
        //    set
        //    {
        //        ViewState["ResultStudentViews"] = value == null ? null : value;
        //    }
        //}
        #endregion

        /// <summary>
        /// 儲存查詢結果的 PKeys
        /// </summary>
        private List<string> ResultPKeys
        {
            get
            {
                return ViewState["ResultPKeys"] as List<string>;
            }
            set
            {
                ViewState["ResultPKeys"] = value;
            }
        }
        #endregion
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
            #region 查詢條件
            #region 基本條件 (未繳)
            Expression where1 = new Expression(StudentView.Field.ReceiveDate, string.Empty)
                .Or(StudentView.Field.ReceiveDate, null);

            Expression where2 = new Expression(StudentView.Field.ReceiveWay, string.Empty)
                .Or(StudentView.Field.ReceiveWay, null);
            #endregion

            #region 5 Key + 基本條件
            where = new Expression(StudentView.Field.ReceiveType, this.QueryReceiveType)
                .And(StudentView.Field.YearId, this.QueryYearId)
                .And(StudentView.Field.TermId, this.QueryTermId)
                .And(StudentView.Field.DepId, this.QueryDepId)
                .And(StudentView.Field.ReceiveId, this.QueryReceiveId)
                .And(where1)
                .And(where2);
            #endregion

            #region 銷帳編號 條件
            if (!String.IsNullOrWhiteSpace(this.QueryCancelNo))
            {
                where.And(StudentView.Field.CancelNo, this.QueryCancelNo);
            }
            #endregion

            #region 學號 條件
            if (!String.IsNullOrWhiteSpace(this.QueryStuId))
            {
                where.And(StudentView.Field.StuId, this.QueryStuId);
            }
            #endregion

            #region 批號 條件
            if (!String.IsNullOrWhiteSpace(this.QueryUpNo))
            {
                where.And(StudentView.Field.UpNo, this.QueryUpNo);
            }
            #endregion
            #endregion


            #region 排序條件
            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentView.Field.StuId, OrderByEnum.Asc);
            #endregion

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
            int totalCount = 0;
            XmlResult xmlResult = base.QueryDataAndBind2<StudentView>(pagingInfo, ucPagings, this.gvResult, out totalCount);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }

            this.labDataCount.Text = totalCount.ToString();

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
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
            #region 檢查維護權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return false;
            }
            #endregion

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
                string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
                this.ShowSystemMessage(msg);
                return false;
            }

            //因為控制項的 Page_Load 比頁面的 Page_Load 晚觸發，所以可以先執行 GetDataAndBind 方法，強迫 ucFilter1 結繫資料
            //因為 ucFilter1 有指定 Filter2ControlID 為 ucFilter2，所以 ucFilter2 頁會被自動結繫資料
            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
            if (xmlResult.IsSuccess)
            {
                receiveId = this.ucFilter2.SelectedReceiveID;
            }
            this.QueryReceiveType = receiveType;
            this.QueryYearId = yearId;
            this.QueryTermId = termId;
            this.QueryDepId = "";
            this.QueryReceiveId = this.ucFilter2.SelectedReceiveID;
            #endregion

            tbxCancelNo.Text = string.Empty;
            tbxStuId.Text = string.Empty;

            this.ucFilter2_ItemSelectedIndexChanged(this.ucFilter2, null);
            //GetAndBindUpNoOptions(this.QueryReceiveType, this.QueryYearId, this.QueryTermId, this.QueryDepId, this.QueryReceiveId);

            #region 查詢結果初始化
            {
                ////因為有新增與回上一頁的按鈕，所以不適合把 divResult 隱藏
                ////改為隱藏分頁按鈕，並結繫 null
                ////this.divResult.Visible = false;
                //this.gvResult.DataSource = null;
                //this.gvResult.DataBind();
                Paging[] ucPagings = this.GetPagingControls();
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = false;
                }
            }
            #endregion

            this.ccbtnDelete.Visible = false;
            //this.lbtnBacthDelete.Visible = false;

            return true;
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
            WebHelper.SetDropDownListItems(this.ddlUpNo, DefaultItem.Kind.Select, false, items, false, false, 0, null);
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>傳回處理結果</returns>
        private bool GetAndKeepQueryCondition()
        {
            #region 代收費用別
            string qReceiveId = this.ucFilter2.SelectedReceiveID;
            if (String.IsNullOrEmpty(qReceiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return false;
            }
            #endregion

            #region 銷帳編號、學號、批號 至少一項
            string qCancelNo = this.tbxCancelNo.Text.Trim();
            string qStuId = this.tbxStuId.Text.Trim();
            string qUpNo = this.ddlUpNo.SelectedValue;

            if (String.IsNullOrEmpty(qCancelNo) && String.IsNullOrEmpty(qStuId) && String.IsNullOrEmpty(qUpNo))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「虛擬帳號」、「學號」、「批號」至少輸入一查詢條件");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            this.QueryReceiveType = this.ucFilter1.SelectedReceiveType;
            this.QueryYearId = this.ucFilter1.SelectedYearID;
            this.QueryTermId = this.ucFilter1.SelectedTermID;
            this.QueryDepId = "";
            this.QueryReceiveId = qReceiveId;
            this.QueryCancelNo = qCancelNo;
            this.QueryStuId = qStuId;
            this.QueryUpNo = qUpNo;

            return true;
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
                    string action = this.GetLocalized("查詢學生繳費資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }

                this.ccbtnDelete.Visible = (gvResult.Rows.Count > 0);
                //this.lbtnBacthDelete.Visible = this.ccbtnDelete.Visible;
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            #region [MDY:20160123] 改存查詢結果的 PKeys
            #region [Old]
            //StudentView[] datas = this.gvResult.DataSource as StudentView[];
            //if (datas == null || datas.Length == 0)
            //{
            //    return;
            //}

            //this.ResultStudentViews = datas;
            #endregion

            StudentView[] datas = this.gvResult.DataSource as StudentView[];
            if (datas == null || datas.Length == 0)
            {
                this.ResultPKeys = new List<string>(0);
                return;
            }

            List<string> pkeys = new List<string>(datas.Length);
            foreach (StudentView data in datas)
            {
                //順序不能改，不然後端處理會有問題
                pkeys.Add(String.Format("{0},{1},{2},{3},{4},{5},{6}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.StuId, data.OldSeq));
            }
            this.ResultPKeys = pkeys;
            #endregion

            bool isDeletable = this.HasDeleteAuth();
            foreach (GridViewRow row in this.gvResult.Rows)
            {
                StudentView data = datas[row.RowIndex];
                //資料參數
                //string argument = String.Format("{0},{1},{2},{3},{4},{5}", data.StuId, data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);

                CodeText cancelStatus = CancelStatusCodeTexts.GetCancelStatus(data.ReceiveDate, data.AccountDate);
                row.Cells[5].Text = cancelStatus.Text;

                #region [MDY:20160123] 查詢條件限制未繳，所以不用檢查
                //CheckBox chkSelected = row.FindControl("chkSelected") as CheckBox;
                //if (chkSelected != null)
                //{
                //    chkSelected.Visible = isDeletable && cancelStatus.Code == CancelStatusCodeTexts.NONPAY;
                //}
                #endregion
            }
        }

        protected void ccbtnDelete_Click(object sender, EventArgs e)
        {
            #region 檢查刪除權限
            if (!this.HasDeleteAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無刪除權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            #region [MDY:20160123] 改存查詢結果的 PKeys
            #region [Old]
            //KeyValueList<string> DeleteDatas = new KeyValueList<string>();

            //StudentView[] datas = this.ResultStudentViews;
            //if (datas == null || datas.Length == 0)
            //{
            //    //[TODO] 固定顯示訊息的收集
            //    string msg = this.GetLocalized("無資料可刪除");
            //    this.ShowSystemMessage(msg);
            //    return;
            //}

            //foreach (GridViewRow row in this.gvResult.Rows)
            //{
            //    StudentView data = datas[row.RowIndex];

            //    CheckBox chk = (CheckBox)row.FindControl("chkSelected");
            //    if(chk != null)
            //    {
            //        if(chk.Checked)
            //        {
            //            //資料參數
            //            string argument = String.Format("{0},{1},{2},{3},{4},{5}", data.StuId, data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);
            //            DeleteDatas.Add("args", argument);
            //        }
            //    }
            //}
            #endregion

            KeyValueList<string> args = new KeyValueList<string>();
            List<string> pkeys = this.ResultPKeys;
            if (pkeys == null || pkeys.Count == 0)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無資料可刪除");
                this.ShowSystemMessage(msg);
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelected");
                if (chk != null && chk.Checked)
                {
                    string pkey = pkeys[row.RowIndex];
                    args.Add("PKeys", pkey);
                }
            }
            #endregion

            if (args.Count == 0)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("未選取任何資料");
                this.ShowSystemMessage(msg);
                return;
            }

            string action = this.GetLocalized("刪除學生繳費資料");
            //執行刪除
            object returnData = null;
            XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DeleteStudentReceiveByPKeys, args, out returnData);
            if (!xmlResult.IsSuccess)
            {
                this.ShowSystemMessage(this.GetLocalized("資料刪除失敗") + "，" + xmlResult.Message);
                return;
            }

            //顯示刪除結果
            this.ShowSystemMessage(returnData.ToString());

            //重新執行一次查詢
            {
                PagingInfo pagingInfo = new PagingInfo(10, 0, 0);
                xmlResult = this.CallQueryDataAndBind(pagingInfo);

                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                }

                this.ccbtnDelete.Visible = (gvResult.Rows.Count > 0);
                //this.lbtnBacthDelete.Visible = this.ccbtnDelete.Visible;
            }
        }

        protected void lbtnBatcthDelete_Click(object sender, EventArgs e)
        {
            #region 檢查刪除權限
            if (!this.HasDeleteAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無刪除權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            #region [MDY:20160123] 調整效能，改用條件刪除
            #region [Old]
            //XmlResult xmlResult = null;
            //StudentView[] datas = null;
            //if (this.GetAndKeepQueryCondition())
            //{
            //    Expression where = new Expression();
            //    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            //    this.GetWhereAndOrderBys(out where, out orderbys);

            //    xmlResult = DataProxy.Current.SelectAll<StudentView>(this, where, orderbys, out datas);
            //    if (!xmlResult.IsSuccess)
            //    {
            //        string action = this.GetLocalized("整批刪除學生繳費資料");
            //        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            //        return;
            //    }

            //    if (datas == null || datas.Length == 0)
            //    {
            //        //[TODO] 固定顯示訊息的收集
            //        string msg = this.GetLocalized("無資料可刪除");
            //        this.ShowSystemMessage(msg);
            //        return;
            //    }
            //}

            //KeyValueList<string> DeleteDatas = new KeyValueList<string>();
            //foreach (StudentView data in datas)
            //{
            //    //資料參數
            //    string argument = String.Format("{0},{1},{2},{3},{4},{5}", data.StuId, data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);
            //    DeleteDatas.Add("args", argument);
            //}

            ////執行整批刪除
            //object returnData = null;
            //xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DeleteStudentReceiveDatas, DeleteDatas, out returnData);
            //if (!xmlResult.IsSuccess)
            //{
            //    this.ShowSystemMessage(this.GetLocalized("資料刪除失敗") + "，" + xmlResult.Message);
            //    return;
            //}
            #endregion

            KeyValueList<string> args = new KeyValueList<string>(8);

            #region 商家代號
            string receiveType = this.ucFilter1.SelectedReceiveType;
            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }
            args.Add("ReceiveType", receiveType);
            #endregion

            #region 學年
            string yearId = this.ucFilter1.SelectedYearID;
            if (String.IsNullOrEmpty(yearId))
            {
                this.ShowMustInputAlert("學年");
                return;
            }
            args.Add("YearId", yearId);
            #endregion

            #region 學期
            string termId = this.ucFilter1.SelectedTermID;
            if (String.IsNullOrEmpty(termId))
            {
                this.ShowMustInputAlert("學期");
                return;
            }
            args.Add("TermId", termId);
            #endregion

            #region 部別 (土銀不使用這個部別，固定為空字串
            string depId = String.Empty;
            args.Add("DepId", depId);
            #endregion

            #region 代收費用別
            string receiveId = this.ucFilter2.SelectedReceiveID;
            if (String.IsNullOrEmpty(receiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return;
            }
            args.Add("ReceiveId", receiveId);
            #endregion

            #region 虛擬帳號、學號、批號 至少一項
            string cancelNo = this.tbxCancelNo.Text.Trim();
            string stuId = this.tbxStuId.Text.Trim();
            string upNo = this.ddlUpNo.SelectedValue;

            if (String.IsNullOrEmpty(cancelNo) && String.IsNullOrEmpty(stuId) && String.IsNullOrEmpty(upNo))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「虛擬帳號」、「學號」、「批號」至少輸入一查詢條件");
                this.ShowSystemMessage(msg);
                return;
            }
            args.Add("CancelNo", cancelNo);
            args.Add("StuId", stuId);
            args.Add("UpNo", upNo);
            #endregion

            //執行條件刪除
            object returnData = null;
            XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DeleteStudentReceiveByWhere, args, out returnData);
            if (!xmlResult.IsSuccess)
            {
                this.ShowSystemMessage(this.GetLocalized("資料刪除失敗") + "，" + xmlResult.Message);
                return;
            }
            #endregion

            //顯示刪除結果
            this.ShowSystemMessage(returnData.ToString());

            #region [MDY:20160123] 整批刪除與查詢無關了，所以改為清空查詢結果
            #region [Old]
            ////重新執行一次查詢
            //{
            //    PagingInfo pagingInfo = new PagingInfo(10, 0, 0);
            //    xmlResult = this.CallQueryDataAndBind(pagingInfo);

            //    if (!xmlResult.IsSuccess)
            //    {
            //        this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
            //    }

            //    this.ccbtnDelete.Visible = (gvResult.Rows.Count > 0);
            //    //this.lbtnBacthDelete.Visible = this.ccbtnDelete.Visible;
            //}
            #endregion

            {
                this.gvResult.DataSource = null;
                this.gvResult.DataBind();
                Paging[] ucPagings = this.GetPagingControls();
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = false;
                }
                this.ccbtnDelete.Visible = false;
            }
            #endregion
        }
    }
}