using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.C
{
    public partial class C3600003 : BasePage
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

        #region [MDY:20200309] CHECKMARX Reflected XSS All Clients Revision
        #region [OLD]
        ///// <summary>
        ///// 儲存查詢的ProblemListView
        ///// </summary>
        //private ProblemListView[] KeepProblemListViews
        //{
        //    get
        //    {
        //        return ViewState["KeepProblemListViews"] as ProblemListView[];
        //    }
        //    set
        //    {
        //        ViewState["KeepProblemListViews"] = value == null ? null : value;
        //    }
        //}
        #endregion

        /// <summary>
        /// 儲存查詢結果的資料筆數
        /// </summary>
        private Int32 QueryResultDataCount
        {
            get
            {
                object value = ViewState["QueryResultDataCount"];
                if (value is Int32)
                {
                    return (Int32)value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["QueryResultDataCount"] = value < 0 ? 0 : value;
            }
        }
        #endregion

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

            ccbtnGen.Visible = false;

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            ccbtnGenODS.Visible = ccbtnGen.Visible;
            #endregion

            return true;
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetAndBindQueryData()
        {
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
            XmlResult xmlResult = DataProxy.Current.SelectAll<ProblemListView>(this, where, orderbys, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            else if (datas == null || datas.Length == 0)
            {
                this.ShowErrorMessage(ErrorCode.D_QUERY_NO_DATA, "查無資料");
            }

            ccbtnGen.Visible = (datas != null && datas.Length > 0);

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            ccbtnGenODS.Visible = ccbtnGen.Visible;
            #endregion

            #region [MDY:20200309] CHECKMARX Reflected XSS All Clients Revision
            #region [OLD]
            //this.KeepProblemListViews = datas;
            #endregion

            this.QueryResultDataCount = datas == null ? 0 : datas.Length;
            #endregion

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
                where.And(ProblemListView.Field.ReceiveWay, this.QueryReceiveWay);

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

            #region 查詢的代收日區間的起日
            {
                string txt = tbxReceiveDateS.Text.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    DateTime date;
                    if (DateTime.TryParse(txt, out date))
                    {
                        this.QueryReceiveDateStart = date.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        this.QueryReceiveDateStart = null;
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("代收日區間的起日不是合法日期格式");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
                else
                {
                    this.QueryReceiveDateStart = String.Empty;
                }
            }
            #endregion

            #region 查詢的代收日區間的迄日
            {
                string txt = tbxReceiveDateE.Text.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    DateTime date;
                    if (DateTime.TryParse(txt, out date))
                    {
                        this.QueryReceiveDateEnd = date.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        this.QueryReceiveDateEnd = null;
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("代收日區間的迄日不是合法日期格式");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
                else
                {
                    this.QueryReceiveDateEnd = String.Empty;
                }
            }
            #endregion

            #region 查詢的入帳日區間的起日
            {
                string txt = tbxAccountDateS.Text.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    DateTime date;
                    if (DateTime.TryParse(txt, out date))
                    {
                        this.QueryAccountDateStart = date.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        this.QueryAccountDateStart = null;
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("入帳日區間的起日不是合法日期格式");
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
                string txt = tbxAccountDateE.Text.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    DateTime date;
                    if (DateTime.TryParse(txt, out date))
                    {
                        this.QueryAccountDateEnd = date.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        this.QueryAccountDateEnd = null;
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("入帳日區間的起日不是合法日期格式");
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
                row.Cells[3].Text = ProblemFlagCodeTexts.GetText(data.ProblemFlag);
                string receiveDate = data.ReceiveDate == null ? string.Empty : ((DateTime)data.ReceiveDate).ToString("yyyy/MM/dd");
                row.Cells[4].Text = receiveDate;

                string accountDate = data.AccountDate == null ? string.Empty : ((DateTime)data.AccountDate).ToString("yyyy/MM/dd");
                row.Cells[5].Text = accountDate;
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void ccbtnGen_Click(object sender, EventArgs e)
        {
            #region [MDY:20200309] CHECKMARX Reflected XSS All Clients Revision
            #region [OLD]
            //ProblemListView[] datas = this.KeepProblemListViews;
            //if (datas == null || datas.Length == 0)
            //{
            //    return;
            //}
            #endregion

            if (this.QueryResultDataCount <= 0)
            {
                return;
            }
            #endregion

            DataTable dt = MakeDataTable();

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                string cell1 = row.Cells[0].Text == "&nbsp;" ? string.Empty : row.Cells[0].Text;
                string cell2 = row.Cells[1].Text == "&nbsp;" ? string.Empty : row.Cells[1].Text;
                string cell3 = row.Cells[2].Text == "&nbsp;" ? string.Empty : row.Cells[2].Text;
                string cell4 = row.Cells[3].Text == "&nbsp;" ? string.Empty : row.Cells[3].Text;
                string cell5 = row.Cells[4].Text == "&nbsp;" ? string.Empty : row.Cells[4].Text;
                string cell6 = row.Cells[5].Text == "&nbsp;" ? string.Empty : row.Cells[5].Text;
                string cell7 = row.Cells[6].Text == "&nbsp;" ? string.Empty : row.Cells[6].Text;
                string cell8 = row.Cells[7].Text == "&nbsp;" ? string.Empty : row.Cells[7].Text;
                dt.Rows.Add(cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8);
            }

            #region [Old] 因為 Web 端參考 NPOI V2.0，所以改用 ExcelHelper.dll 的 ConvertFileHelper
            //ConvertFileHelper helper = new ConvertFileHelper();

            //byte[] content = helper.Dt2Xls(dt);
            #endregion

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式 & ConvertFileHelper 已改用 NPOI V2.0
            {
                byte[] fileContent = null;
                string fileName = null;

                LinkButton control = sender as LinkButton;
                if (control.CommandArgument == "ODS")
                {
                    #region 產生 ods 的檔案內容
                    ODSHelper helper = new ODSHelper();
                    fileContent = helper.DataTable2ODS(dt, "sheet1");
                    fileName = "ProblemList.ods";
                    #endregion
                }
                else
                {
                    #region 產生 xls 的檔案內容
                    //ExcelHelper.ConvertFileHelper helper = new ExcelHelper.ConvertFileHelper();
                    ConvertFileHelper helper = new ConvertFileHelper();
                    fileContent = helper.DataTable2Xls(dt, "sheet1");
                    fileName = "ProblemList.xls";
                    #endregion
                }

                if (fileContent == null)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("產生檔案失敗");
                    this.ShowSystemMessage(msg);
                    return;
                }
                else
                {
                    this.ResponseFile(fileName, fileContent);
                }
            }
            #endregion
        }

		private DataTable MakeDataTable()
		{
            GridViewRow headerRow = gvResult.HeaderRow;

            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[8]{ 
                    new DataColumn("虛擬帳號", typeof(string)),
                    new DataColumn("繳款金額", typeof(string)),
                    new DataColumn("繳款方式", typeof(string)),
                    new DataColumn("問題註記", typeof(string)),
                    new DataColumn("代收日", typeof(string)),
                    new DataColumn("入帳日", typeof(string)),
                    new DataColumn("學號", typeof(string)),
                    new DataColumn("姓名", typeof(string)) });

            return dt;

		}

    }
}