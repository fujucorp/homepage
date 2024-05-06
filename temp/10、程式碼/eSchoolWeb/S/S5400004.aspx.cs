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

namespace eSchoolWeb.S
{
    /// <summary>
    /// 繳費資料查詢
    /// </summary>
    public partial class S5400004 : PagingBasePage
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
            where = new Expression(StudentReceiveView.Field.ReceiveType, this.QueryReceiveType);

            #region 銷帳狀態 條件
            {
                switch (this.QueryCancelStatus)
                {
                    case CancelStatusCodeTexts.NONPAY:
                        //未繳款
                        where
                            .And((new Expression(StudentReceiveView.Field.CancelFlag, String.Empty)).Or(StudentReceiveView.Field.CancelFlag, null))
                            .And((new Expression(StudentReceiveView.Field.ReceiveDate, String.Empty)).Or(StudentReceiveView.Field.ReceiveDate, null));
                        break;
                    case CancelStatusCodeTexts.PAYED:
                        //已繳款(未入帳)
                        where
                            .And(StudentReceiveView.Field.ReceiveDate, RelationEnum.NotEqual, String.Empty)
                            .And(StudentReceiveView.Field.ReceiveDate, RelationEnum.NotEqual, null)
                            .And((new Expression(StudentReceiveView.Field.AccountDate, String.Empty)).Or(StudentReceiveView.Field.AccountDate, null));
                        break;
                    case CancelStatusCodeTexts.CANCELED:
                        //已入帳
                        where
                            .And(StudentReceiveView.Field.AccountDate, RelationEnum.NotEqual, String.Empty)
                            .And(StudentReceiveView.Field.AccountDate, RelationEnum.NotEqual, null);
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
                    //        where.And(StudentReceiveView.Field.ReceiveWay, ChannelHelper.SM_HILI);
                    //        break;
                    //    case "2":   //ATM
                    //        where.And(StudentReceiveView.Field.ReceiveWay, new string[] { ChannelHelper.ATM, ChannelHelper.ATMA });
                    //        break;
                    //    case "3":   //臨櫃
                    //        where.And(StudentReceiveView.Field.ReceiveWay, ChannelHelper.TABS);
                    //        break;
                    //}
                    #endregion
                }
                #endregion

                #region 入帳日區間 條件
                {
                    if (!String.IsNullOrEmpty(this.QueryAccountDateStart))
                    {
                        where.And(StudentReceiveView.Field.AccountDate, RelationEnum.GreaterEqual, this.QueryAccountDateStart);
                    }
                    if (!String.IsNullOrEmpty(this.QueryAccountDateEnd))
                    {
                        where.And(StudentReceiveView.Field.AccountDate, RelationEnum.LessEqual, this.QueryAccountDateEnd);
                    }
                }
                #endregion
            }

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReceiveView.Field.StuId, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<StudentReceiveView>(pagingInfo, ucPagings, this.gvResult);
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
            this.QueryReceiveType = ucFilter1.SelectedReceiveType;

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
                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<ChannelSetEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);

                //CodeText[] items = new CodeText[] { new CodeText("1", "超商"), new CodeText("2", "ATM"), new CodeText("3", "臨櫃") };
                WebHelper.SetDropDownListItems(this.ddlReceiveWay, DefaultItem.Kind.All, false, datas, true, false, 0, null);
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
            if (String.IsNullOrEmpty(this.QueryReceiveType))
            {
                this.ShowMustInputAlert("業務別碼");
                return false;
            }
            #endregion

            //查詢的銷帳選擇
            this.QueryCancelStatus = this.ddlCancelStatus.SelectedValue;

            if (this.QueryCancelStatus != CancelStatusCodeTexts.NONPAY)
            {
                #region 查詢的繳款方式
                this.QueryReceiveWay = this.ddlReceiveWay.SelectedValue;
                #endregion

                if (this.QueryCancelStatus == CancelStatusCodeTexts.CANCELED || String.IsNullOrEmpty(this.QueryCancelStatus))
                {
                    DateTime accountDateS = new DateTime();
                    DateTime accountDateE = new DateTime();

                    #region 查詢的入帳日區間的起日
                    {
                        string txt = this.tbxAccountDateS.Text.Trim();
                        if (!String.IsNullOrEmpty(txt))
                        {
                            DateTime date;
                            if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                            {
                                accountDateS = date;
                                this.QueryAccountDateStart = Common.GetTWDate7(date);
                            }
                            else
                            {
                                this.QueryAccountDateStart = null;
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("「入帳日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                        else
                        {
                            this.QueryAccountDateStart = String.Empty;
                        }
                    }
                    #endregion

                    #region 查詢的入帳日區間的迄日
                    {
                        string txt = this.tbxAccountDateE.Text.Trim();
                        if (!String.IsNullOrEmpty(txt))
                        {
                            DateTime date;
                            if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                            {
                                accountDateE = date;
                                this.QueryAccountDateEnd = Common.GetTWDate7(date);
                            }
                            else
                            {
                                this.QueryAccountDateEnd = null;
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("「入帳日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                        else
                        {
                            this.QueryAccountDateEnd = String.Empty;
                        }
                    }
                    #endregion

                    #region 查詢最多 1個月區間
                    if (accountDateS != DateTime.MinValue && accountDateE != DateTime.MinValue)
                    {
                        TimeSpan Total = accountDateE.Subtract(accountDateS); //日期相減
                        if (Total.Days > 31)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("查詢最多 1個月區間");
                            this.ShowJsAlert(msg);
                            return false;
                        }
                    }
                    #endregion
                }
                else
                {
                    this.QueryAccountDateStart = String.Empty;
                    this.QueryAccountDateEnd = String.Empty;
                }
            }
            else
            {
                this.QueryReceiveWay = null;
                this.QueryAccountDateStart = null;
                this.QueryAccountDateEnd = null;
            }

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
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
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            StudentReceiveView[] datas = this.gvResult.DataSource as StudentReceiveView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                StudentReceiveView data = datas[row.RowIndex];

                #region [MDY:20160607] 修正資料參數
                #region [Old]
                ////資料參數
                //string argument = String.Format("{0},{1},{2},{3},{4},{5}", data.StuId, data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);
                #endregion

                string argument = String.Format("{0},{1},{2},{3},{4},{5},{6}", data.StuId, data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.OldSeq);
                #endregion

                row.Cells[5].Text = CancelStatusCodeTexts.GetText(data.GetCancelStatus());
                row.Cells[6].Text = string.Format("{0}<br/>{1}", data.StuId, data.StuName);

                LinkButton lbtnDetail = row.FindControl("lbtnDetail") as LinkButton;
                if (lbtnDetail != null)
                {
                    lbtnDetail.CommandArgument = argument;
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
                case "Detail":
                    #region 明細
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
                        Server.Transfer(WebHelper.GenRNUrl("S5400004D.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
            }
        }
    }
}