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

namespace eSchoolWeb.C
{
    /// <summary>
    /// 問題檔資料刪除
    /// </summary>
    public partial class C3600002 : BasePage
    {
        #region Const
        public const int MaxRecordCount = 1000;
        #endregion

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
        /// 儲存查詢的繳款金額
        /// </summary>
        private string QueryPayAmount
        {
            get
            {
                return ViewState["QueryPayAmount"] as string;
            }
            set
            {
                ViewState["QueryPayAmount"] = value == null ? null : value.Trim();
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
        /// 儲存查詢的ProblemListView
        /// </summary>
        private ProblemListView[] KeepProblemListViews
        {
            get
            {
                return ViewState["KeepProblemListViews"] as ProblemListView[];
            }
            set
            {
                ViewState["KeepProblemListViews"] = value == null ? null : value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
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

                //CodeText[] items = new CodeText[] { new CodeText("1", "超商"), new CodeText("2", "ATM"), new CodeText("3", "臨櫃") };
                WebHelper.SetDropDownListItems(this.ddlReceiveWay, DefaultItem.Kind.All, false, datas, true, false, 0, null);
            }
            #endregion

            #region 日期區間種類
            {
                CodeText[] datas = new CodeText[] { new CodeText("ReceiveDate", "代收日期"), new CodeText("AccountDate", "入帳日期") };
                WebHelper.SetDropDownListItems(this.ddlQueryDateType, DefaultItem.Kind.Omit, false, datas, false, true, 0, null);
            }
            #endregion

            MyDeleteButton.Visible = false;

            return true;
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetAndBindQueryData()
        {
            this.labMoreMsg.Visible = false;

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
            orderbys.Add(ProblemListView.Field.ReceiveDate, OrderByEnum.Asc);
            #endregion

            ProblemListView[] datas = null;
            int totalCount = 0;
            XmlResult xmlResult = DataProxy.Current.Select<ProblemListView>(this.Page, where, orderbys, 0, MaxRecordCount, out datas, out totalCount);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            else if (totalCount > MaxRecordCount)
            {
                this.labMoreMsg.Text = String.Format("符合查詢條件的資料有 {0} 筆。如需處理其他資料，請所縮小查詢資料範圍。", totalCount);
                this.labMoreMsg.Visible = true;
            }

            this.MyDeleteButton.Visible = (datas != null && datas.Length > 0);
            this.KeepProblemListViews = datas;
            this.gvResult.DataSource = datas;
            this.gvResult.DataBind();
            return xmlResult.IsSuccess;
        }

        private Expression GetWhere()
        {
            Expression where = new Expression();

            #region 代收類別 條件
            if (!String.IsNullOrEmpty(this.QueryReceiveType))
            {
                where.And(ProblemListView.Field.ReceiveType, this.QueryReceiveType);
            }
            #endregion

            #region 銷帳編號 條件
            if (!String.IsNullOrEmpty(this.QueryCancelNo))
            {
                where.And(ProblemListView.Field.CancelNo, this.QueryCancelNo);
            }
            #endregion

            #region 繳款方式 條件
            if (!String.IsNullOrEmpty(this.QueryReceiveWay))
            {
                where.And(StudentReceiveView.Field.ReceiveWay, this.QueryReceiveWay);

                #region [Old]
                //switch (this.QueryReceiveWay)
                //{
                //    case "1":   //超商
                //        where.And(ProblemListView.Field.ReceiveWay, new string[] { ChannelHelper.SM_711, ChannelHelper.SM_HILI, ChannelHelper.SM_OKM, ChannelHelper.SM_TFM });
                //        break;
                //    case "2":   //ATM
                //        where.And(ProblemListView.Field.ReceiveWay, new string[] { ChannelHelper.ATM, ChannelHelper.ATMA });
                //        break;
                //    case "3":   //臨櫃
                //        where.And(ProblemListView.Field.ReceiveWay, ChannelHelper.TABS);
                //        break;
                //}
                #endregion
            }
            #endregion

            #region 繳款金額 條件
            if (!String.IsNullOrEmpty(this.QueryPayAmount))
            {
                where.And(ProblemListView.Field.PayAmount, this.QueryPayAmount);
            }
            #endregion

            #region 代收日區間 條件
            {
                if (!String.IsNullOrEmpty(this.QueryReceiveDateStart))
                {
                    where.And(ProblemListView.Field.ReceiveDate, RelationEnum.GreaterEqual, this.QueryReceiveDateStart);
                }
                if (!String.IsNullOrEmpty(this.QueryReceiveDateEnd))
                {
                    where.And(ProblemListView.Field.ReceiveDate, RelationEnum.LessEqual, this.QueryReceiveDateEnd);
                }
            }
            #endregion

            #region 入帳日區間 條件
            {
                if (!String.IsNullOrEmpty(this.QueryAccountDateStart))
                {
                    where.And(ProblemListView.Field.AccountDate, RelationEnum.GreaterEqual, this.QueryAccountDateStart);
                }
                if (!String.IsNullOrEmpty(this.QueryAccountDateEnd))
                {
                    where.And(ProblemListView.Field.AccountDate, RelationEnum.LessEqual, this.QueryAccountDateEnd);
                }
            }
            #endregion

            return where;
        }

        protected void ccbtnQuery_Click(object sender, EventArgs e)
        {
            if (this.GetAndKeepQueryCondition())
            {
                this.GetAndBindQueryData();
            }
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>成功傳回 true，否則傳回 false</returns>
        private bool GetAndKeepQueryCondition()
        {
            this.QueryReceiveType = this.ucFilter1.SelectedReceiveType;
            if (String.IsNullOrEmpty(this.QueryReceiveType))
            {
                this.ShowMustInputAlert("業務別碼");
                return false;
            }

            #region 查詢的繳款金額
            string cancelNo = tbxCancelNo.Text.Trim();
            this.QueryCancelNo = this.tbxCancelNo.Text.Trim();
            this.QueryPayAmount = this.tbxPayAmount.Text.Trim();
            if (!string.IsNullOrEmpty(this.QueryPayAmount))
            {
                if (!Common.IsMoney(this.QueryPayAmount))
                {
                    this.QueryReceiveDateStart = string.Empty;
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("繳款金額不是合法金額格式");
                    this.ShowJsAlert(msg);
                    return false;
                }
            }
            #endregion

            //繳款方式
            this.QueryReceiveWay = this.ddlReceiveWay.SelectedValue;
            this.QueryReceiveDateStart = String.Empty;
            this.QueryReceiveDateEnd = String.Empty;
            this.QueryAccountDateStart = String.Empty;
            this.QueryAccountDateEnd = String.Empty;

            #region [Old] 代收日區間與入帳日區間條件改為二擇一
            //#region 查詢的代收日區間的起日
            //{
            //    string txt = tbxReceiveDateS.Text.Trim();
            //    if (!String.IsNullOrEmpty(txt))
            //    {
            //        DateTime date;
            //        if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
            //        {
            //            this.QueryReceiveDateStart = date.ToString("yyyy/MM/dd");
            //        }
            //        else
            //        {
            //            this.QueryReceiveDateStart = null;
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「代收日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
            //            this.ShowJsAlert(msg);
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        this.QueryReceiveDateStart = String.Empty;
            //    }
            //}
            //#endregion

            //#region 查詢的代收日區間的迄日
            //{
            //    string txt = tbxReceiveDateE.Text.Trim();
            //    if (!String.IsNullOrEmpty(txt))
            //    {
            //        DateTime date;
            //        if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
            //        {
            //            this.QueryReceiveDateEnd = date.ToString("yyyy/MM/dd");
            //        }
            //        else
            //        {
            //            this.QueryReceiveDateEnd = null;
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「代收日區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
            //            this.ShowJsAlert(msg);
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        this.QueryReceiveDateEnd = String.Empty;
            //    }
            //}
            //#endregion

            //#region 查詢的入帳日區間的起日
            //{
            //    string txt = tbxAccountDateS.Text.Trim();
            //    if (!String.IsNullOrEmpty(txt))
            //    {
            //        DateTime date;
            //        if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
            //        {
            //            this.QueryAccountDateStart = date.ToString("yyyy/MM/dd");
            //        }
            //        else
            //        {
            //            this.QueryAccountDateStart = null;
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「入帳日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
            //            this.ShowJsAlert(msg);
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        this.QueryAccountDateStart = String.Empty;
            //    }
            //}
            //#endregion

            //#region 查詢的入帳日區間的迄日
            //{
            //    string txt = tbxAccountDateE.Text.Trim();
            //    if (!String.IsNullOrEmpty(txt))
            //    {
            //        DateTime date;
            //        if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
            //        {
            //            this.QueryAccountDateEnd = date.ToString("yyyy/MM/dd");
            //        }
            //        else
            //        {
            //            this.QueryAccountDateEnd = null;
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「入帳日區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
            //            this.ShowJsAlert(msg);
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        this.QueryAccountDateEnd = String.Empty;
            //    }
            //}
            //#endregion
            #endregion

            #region [New] 代收日區間與入帳日區間條件改為二擇一
            string queryDateType = this.ddlQueryDateType.SelectedValue;
            if (queryDateType == "ReceiveDate" || queryDateType == "AccountDate")
            {
                string dateTypeName = queryDateType == "ReceiveDate" ? "代收日期" : "入帳日期";

                #region 起日
                string querySDate = this.tbxQuerySDate.Text.Trim();
                if (String.IsNullOrEmpty(querySDate))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowMustInputAlert(dateTypeName + "的起日");
                    return false;
                }
                else
                {
                    DateTime date;
                    if (DateTime.TryParse(querySDate, out date) && date.Year >= 1911)
                    {
                        querySDate = date.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        querySDate = null;
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「" + dateTypeName + "的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
                #endregion

                #region 迄日
                string queryEDate = tbxQueryEDate.Text.Trim();
                if (String.IsNullOrEmpty(queryEDate))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowMustInputAlert(dateTypeName + "的迄日");
                    return false;
                }
                else
                {
                    DateTime date;
                    if (DateTime.TryParse(queryEDate, out date) && date.Year >= 1911)
                    {
                        queryEDate = date.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        queryEDate = null;
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「" + queryEDate + "的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
                #endregion

                if (queryDateType == "ReceiveDate")
                {
                    this.QueryReceiveDateStart = querySDate;
                    this.QueryReceiveDateEnd = queryEDate;
                }
                else
                {
                    this.QueryAccountDateStart = querySDate;
                    this.QueryAccountDateEnd = queryEDate;
                }
            }

            #region 因為查詢結果沒有分頁，為了避免查詢結果資料太多，造成網頁結繫資料過久，虛擬帳號、代收日、入帳日至少要一項
            if (String.IsNullOrEmpty(this.QueryCancelNo) && String.IsNullOrEmpty(queryDateType))
            {
                this.ShowSystemMessage("虛擬帳號、日期區間至少需要設定一個條件");
                return false;
            }
            #endregion
            #endregion

            return true;
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            ProblemListView[] datas = this.gvResult.DataSource as ProblemListView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                ProblemListView data = datas[row.RowIndex];

                string problemText = ProblemFlagCodeTexts.GetText(data.ProblemFlag);
                row.Cells[4].Text = problemText;

                if (data.ReceiveDate != null)
                {
                    row.Cells[5].Text = String.Format("{0:yyyy/MM/dd}<br/>{1}", data.ReceiveDate.Value, data.ReceiveTime);
                }

                row.Cells[7].Text = String.Format("{0}<br/>{1}", data.StuId, data.StuName);

                if (data.ProblemRemark != problemText)
                {
                    row.Cells[8].Text = data.ProblemRemark;
                }
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void MyDeleteButton_Click(object sender, EventArgs e)
        {
            KeyValueList<string> DeleteDatas = new KeyValueList<string>();

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                ProblemListView data = this.KeepProblemListViews[row.RowIndex];

                CheckBox chk = (CheckBox)row.FindControl("chkSelected");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        //資料參數
                        string argument = String.Format("{0},{1}", data.Id, data.CancelNo == null ? String.Empty : data.CancelNo.Trim());
                        DeleteDatas.Add("args", argument);
                    }
                }
            }

            if (DeleteDatas.Count == 0)
            {
                this.ShowSystemMessage("請先勾選要刪除的資料");
                return;
            }

            string action = this.GetLocalized("問題檔資料刪除");

            //執行整批刪除
            object returnData = null;
            XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DeleteProblemListDatas, DeleteDatas, out returnData);
            if (xmlResult.IsSuccess)
            {
                //this.ShowSystemMessage(returnData.ToString());
                this.ShowActionSuccessMessage(action);

                this.labLog.Text = returnData.ToString().Replace("\r\n", "<br/>");

                //重新執行查詢
                if (this.GetAndKeepQueryCondition())
                {
                    this.GetAndBindQueryData();
                }
                return;
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Message);
                //this.ShowSystemMessage(this.GetLocalized("資料刪除失敗") + "，" + xmlResult.Message);
                return;
            }
        }
    }
}