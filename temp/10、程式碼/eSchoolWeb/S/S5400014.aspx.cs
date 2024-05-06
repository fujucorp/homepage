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
    /// 電文日誌查詢
    /// </summary>
    public partial class S5400014 : PagingBasePage
    {
        #region Property
        /// <summary>
        /// 儲存查詢的電文類別
        /// </summary>
        private string QueryKind
        {
            get
            {
                return ViewState["QueryKind"] as string;
            }
            set
            {
                ViewState["QueryKind"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的伺服器名稱
        /// </summary>
        private string QueryMachineName
        {
            get
            {
                return ViewState["QueryMachineName"] as string;
            }
            set
            {
                ViewState["QueryMachineName"] = value == null ? null : value.Trim();
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
            where = new Expression();
            orderbys = null;

            #region 電文類別 條件
            {
                string qKind = this.QueryKind;
                if (!String.IsNullOrEmpty(qKind))
                {
                    where.And(EAILogEntity.Field.Kind, qKind);
                }
            }
            #endregion

            #region 伺服器名稱 條件
            {
                string qMachineName = this.QueryMachineName;
                if (!String.IsNullOrEmpty(qMachineName))
                {
                    where.And(EAILogEntity.Field.MachineName, qMachineName);
                }
            }
            #endregion

            #region 發動日期 條件
            {
                DateTime? qSDate = this.QuerySDate;
                if (qSDate != null)
                {
                    where.And(EAILogEntity.Field.SendTime, RelationEnum.GreaterEqual, qSDate.Value);
                }

                DateTime? qEDate = this.QueryEDate;
                if (qEDate != null)
                {
                    where.And(EAILogEntity.Field.SendTime, RelationEnum.Less, qEDate.Value.AddDays(1));
                }
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(EAILogEntity.Field.SN, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<EAILogEntity>(pagingInfo, ucPagings, this.gvResult);
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
            #region 電文類別選項
            {
                CodeTextList items = EAILogEntity.GetAllKindList();
                WebHelper.SetDropDownListItems(this.ddlKind, DefaultItem.Kind.All, false, items, true, true, 0, null);
            }
            #endregion

            Paging[] ucPagings = this.GetPagingControls();
            foreach (Paging ucPaging in ucPagings)
            {
                ucPaging.Visible = false;
            }
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>傳回處理結果</returns>
        private bool GetAndKeepQueryCondition()
        {
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

                if (sDate != null && eDate != null && sDate.Value > eDate.Value)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「發動日期的起日」不可以大於「發動日期的迄日」");
                    this.ShowJsAlert(msg);
                    return false;
                }

                this.QuerySDate = sDate;
                this.QueryEDate = eDate;
            }
            #endregion

            #region 電文類別
            this.QueryKind = this.ddlKind.SelectedValue;
            #endregion

            #region 伺服器名稱
            this.QueryMachineName = this.tbxMachineName.Text.Trim();
            #endregion

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查查詢權限
                if (!this.HasQueryAuth())
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("無查詢權限");
                    this.ShowJsAlert(msg);
                    this.ccbtnQuery.Visible = false;
                    return;
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
                    string action = ActionMode.GetActionLocalized(ActionMode.Query);
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            EAILogEntity[] datas = this.gvResult.DataSource as EAILogEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                EAILogEntity data = datas[row.RowIndex];

                Literal labMachineName = row.FindControl("labMachineName") as Literal;
                if (labMachineName != null)
                {
                    labMachineName.Text = String.Format("{0}<br/>{1}", HttpUtility.HtmlEncode(data.MachineName), HttpUtility.HtmlEncode(data.RqUID));
                }

                Literal labRsStatus = row.FindControl("labRsStatus") as Literal;
                if (labRsStatus != null)
                {
                    labRsStatus.Text = String.Format("代碼：{0}<br/>說明：{1}", HttpUtility.HtmlEncode(data.RsStatusCode), HttpUtility.HtmlEncode(data.RsStatusDesc));
                }

                Literal labSendResult = row.FindControl("labSendResult") as Literal;
                if (labSendResult != null)
                {
                    labSendResult.Text = String.Format("{0:yyyy/MM/dd HH:mm:ss}<br/>{1}", data.SendTime, HttpUtility.HtmlEncode(data.SendResult));
                }

                Literal litXml = row.FindControl("litXml") as Literal;
                if (litXml != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb
                        .AppendLine("RqXml：")
                        .AppendLine(HttpUtility.HtmlEncode(data.RqXml)).AppendLine("<br/>")
                        .AppendLine("RsXml：")
                        .AppendLine(HttpUtility.HtmlEncode(data.RsXml)).AppendLine();
                    litXml.Text = sb.ToString();
                }
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
    }
}