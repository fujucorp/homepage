using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.S
{
    /// <summary>
    /// D38資料查詢
    /// </summary>
    public partial class S5400007 : PagingBasePage
    {
        #region Property
        /// <summary>
        /// 儲存查詢的工作序號
        /// </summary>
        private int? QueryJobNo
        {
            get
            {
                object value = ViewState["QueryJobNo"];
                if (value is int)
                {
                    return (int)value;
                }
                return null;
            }
            set
            {
                ViewState["QueryJobNo"] = value;
            }
        }

        /// <summary>
        /// 儲存查詢的商家代號
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
        /// 儲存查詢的學校代碼
        /// </summary>
        private string QuerySchIdenty
        {
            get
            {
                return ViewState["QuerySchIdenty"] as string;
            }
            set
            {
                ViewState["QuerySchIdenty"] = value == null ? null : value.Trim();
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
        /// 儲存查詢的發動日期起日
        /// </summary>
        private DateTime? QuerySDate
        {
            get
            {
                object value = ViewState["QuerySDate"];
                if (value is DateTime)
                {
                    return (DateTime)value;
                }
                return null;
            }
            set
            {
                ViewState["QuerySDate"] = value;
            }
        }

        /// <summary>
        /// 儲存查詢的發動日期迄日
        /// </summary>
        private DateTime? QueryEDate
        {
            get
            {
                object value = ViewState["QueryEDate"];
                if (value is DateTime)
                {
                    return (DateTime)value;
                }
                return null;
            }
            set
            {
                ViewState["QueryEDate"] = value;
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

            LogonUser logonUser = WebHelper.GetLogonUser();
            if (logonUser.IsBankManager)
            {
                where = new Expression();
            }
            else if (logonUser.IsBankUser || logonUser.IsSchoolUser)
            {
                where = new Expression(D38DataEntity.Field.ReceiveType, RelationEnum.In, logonUser.MyReceiveTypes);
            }
            else
            {
                return new XmlResult(false, "無使用權限，此功能行員專用", ErrorCode.S_NO_AUTHORIZE);
            }

            #region 工作序號 條件
            {
                int? qJobNo = this.QueryJobNo;
                if (qJobNo != null && qJobNo.Value > 0)
                {
                    where.And(D38DataEntity.Field.JobNo, qJobNo.Value);
                }
            }
            #endregion

            #region 商家代號 條件
            {
                string qReceiveType = this.QueryReceiveType;
                if (!String.IsNullOrEmpty(qReceiveType))
                {
                    where.And(D38DataEntity.Field.ReceiveType, qReceiveType);
                }
            }
            #endregion

            #region 學校代號 條件
            {
                string qSchIdenty = this.QuerySchIdenty;
                if (!String.IsNullOrEmpty(qSchIdenty))
                {
                    where.And(D38DataEntity.Field.SchIdenty, qSchIdenty);
                }
            }
            #endregion

            #region 銷帳編號 條件
            {
                string qCancelNo = this.QueryCancelNo;
                if (!String.IsNullOrEmpty(qCancelNo))
                {
                    where.And(D38DataEntity.Field.CancelNo, qCancelNo);
                }
            }
            #endregion

            #region 發動日期 條件
            {
                DateTime? qSDate = this.QuerySDate;
                if (qSDate != null)
                {
                    where.And(D38DataEntity.Field.CrtDate, RelationEnum.GreaterEqual, qSDate.Value);
                }

                DateTime? qEDate = this.QueryEDate;
                if (qEDate != null)
                {
                    #region [MDY:20160308] 因為 CrtDate 紀錄到時間的部分，所以改用小於 qEDate + 1 天作為查詢條件
                    where.And(D38DataEntity.Field.CrtDate, RelationEnum.Less, qEDate.Value.Date.AddDays(1));
                    #endregion
                }
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(D38DataEntity.Field.Sn, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<D38DataEntity>(pagingInfo, ucPagings, this.gvResult);
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
        private void InitialUI()
        {
            this.GetAndBindReceiveTypeOptions();

            Paging[] ucPagings = this.GetPagingControls();
            foreach (Paging ucPaging in ucPagings)
            {
                ucPaging.Visible = false;
            }

            this.ccbtnQuery.Visible = false;
        }

        /// <summary>
        /// 取得並結繫業務別碼選項
        /// </summary>
        private void GetAndBindReceiveTypeOptions()
        {
            CodeText[] items = null;

            #region [Old] 因為學校使用者可跨分商家代號且會變動，所以此方法不適用
            //XmlResult xmlResult = DataProxy.Current.GetMyReceiveTypeCodeTextsBySchool(this, out items);
            #endregion

            XmlResult xmlResult = DataProxy.Current.GetMyReceiveTypeCodeTexts(this, out items, ReceiveKindCodeTexts.SCHOOL);
            if (!xmlResult.IsSuccess)
            {
                string action = this.GetLocalized("查詢所屬學校的商家代號資料");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            WebHelper.SetDropDownListItems(this.ddlReceiveType, DefaultItem.Kind.Select, false, items, true, false, 0, null);
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>傳回處理結果</returns>
        private bool GetAndKeepQueryCondition()
        {
            #region 工作序號
            {
                string jobNoTxt = this.tbxJobNo.Text.Trim();
                if (!String.IsNullOrEmpty(jobNoTxt))
                {
                    int jobNo = 0;
                    if (!Int32.TryParse(jobNoTxt, out jobNo) || jobNo < 1)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「工作序號」僅可輸入 1 ~ 999999999 的數字");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                    this.QueryJobNo = jobNo;
                }
                else
                {
                    this.QueryJobNo = null;
                }
            }
            #endregion

            #region 商家代號
            this.QueryReceiveType = this.ddlReceiveType.SelectedValue;
            #endregion

            #region 學校代碼
            this.QuerySchIdenty = this.tbxSchIdenty.Text.Trim();
            #endregion

            #region 銷帳編號
            this.QueryCancelNo = this.tbxCancelNo.Text.Trim();
            #endregion

            #region 發動日期
            {
                DateTime? sDate = null;
                string sDateTxt = this.tbxSDate.Text.Trim();
                if (!String.IsNullOrEmpty(sDateTxt))
                {
                    sDate = DataFormat.ConvertDateText(sDateTxt);
                    if (sDate == null)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「發動日期的起日」不是合法的的日期格式");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
                this.QuerySDate = sDate;

                DateTime? eDate = null;
                string eDateTxt = this.tbxEDate.Text.Trim();
                if (!String.IsNullOrEmpty(eDateTxt))
                {
                    eDate = DataFormat.ConvertDateText(eDateTxt);
                    if (eDate == null)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「發動日期的迄日」不是合法的的日期格式");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
                this.QueryEDate = eDate;

                if (sDate != null && eDate != null && sDate.Value > eDate.Value)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「發動日期的起日」不可以大於「發動日期的迄日」");
                    this.ShowJsAlert(msg);
                    return false;
                }
            }
            #endregion

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region [TODO] 檢查身份
                //LogonUser logonUser = WebHelper.GetLogonUser();
                //if (!logonUser.IsBankUser)
                //{
                //    XmlResult xmlResult = new XmlResult(false, "無使用權限，此功能行員專用", ErrorCode.S_NO_AUTHORIZE);
                //    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                //    return;
                //}
                #endregion

                #region 檢查查詢權限
                if (!this.HasQueryAuth())
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("無查詢權限");
                    this.ShowJsAlert(msg);
                    return;
                }
                #endregion

                this.ccbtnQuery.Visible = true;
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
                    string action = ActionMode.GetActionLocalized(ActionMode.Query);
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            D38DataEntity[] datas = this.gvResult.DataSource as D38DataEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                D38DataEntity data = datas[row.RowIndex];

                row.Cells[5].Text = String.Concat(HttpUtility.HtmlEncode(data.StuId), "<br/>", HttpUtility.HtmlEncode(data.StuName));

                #region [MDY:20160705] 增加執行結果 (如果 Result 跟 Status 不同，則串上 Result 欄位)
                if (data.Status != data.Result && !String.IsNullOrEmpty(data.Result))
                {
                    row.Cells[7].Text = String.Format("{0} ({1})", HttpUtility.HtmlEncode(data.Status), HttpUtility.HtmlEncode(data.Result));
                }
                #endregion
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
    }
}