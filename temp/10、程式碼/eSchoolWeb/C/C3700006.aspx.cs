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
    /// 中心入帳資料查詢
    /// </summary>
    public partial class C3700006 : PagingBasePage
    {
        #region Property
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
        /// 儲存查詢的虛擬帳號
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
            #region 商家代號 條件
            where = new Expression(CancelDebtsView.Field.ReceiveType, this.QueryReceiveType);
            #endregion

            #region 繳款方式 條件
            if (!String.IsNullOrEmpty(this.QueryReceiveWay))
            {
                where.And(CancelDebtsView.Field.ReceiveWay, this.QueryReceiveWay);
            }
            #endregion

            #region 代收日區間 條件
            {
                if (!String.IsNullOrEmpty(this.QueryReceiveDateStart))
                {
                    where.And(CancelDebtsView.Field.ReceiveDate, RelationEnum.GreaterEqual, this.QueryReceiveDateStart);
                }
                if (!String.IsNullOrEmpty(this.QueryReceiveDateEnd))
                {
                    where.And(CancelDebtsView.Field.ReceiveDate, RelationEnum.LessEqual, this.QueryReceiveDateEnd);
                }
            }
            #endregion

            #region 虛擬帳號 條件
            if (!String.IsNullOrEmpty(this.QueryCancelNo))
            {
                where.And(CancelDebtsView.Field.CancelNo, this.QueryCancelNo);
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(2);
            orderbys.Add(CancelDebtsView.Field.ReceiveDate, OrderByEnum.Desc);
            orderbys.Add(CancelDebtsView.Field.SNo, OrderByEnum.Asc);

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
            XmlResult xmlResult = base.QueryDataAndBind<CancelDebtsView>(pagingInfo, ucPagings, this.gvResult);
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
                        xmlResult2 = DataProxy.Current.GetC3700006Summary(this.Page, where, out sumDatas);
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
                    foreach (KeyValue<Decimal> sumData in sumDatas)
                    {
                        switch (sumData.Key)
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

                WebHelper.SetDropDownListItems(this.ddlReceiveWay, DefaultItem.Kind.All, false, datas, true, false, 0, null);
            }
            #endregion

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
            #region 查詢的商家代號
            string qReceiveType = this.ucFilter1.SelectedReceiveType;
            if (String.IsNullOrEmpty(qReceiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return false;
            }
            #endregion

            string qReceiveDateStart = null;
            string qReceiveDateEnd = null;

            #region 查詢的代收日區間的起日
            {
                string txt = this.tbxReceiveDateS.Text.Trim();
                if (!String.IsNullOrEmpty(txt))
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
                if (!String.IsNullOrEmpty(txt))
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

            this.QueryReceiveType = qReceiveType;
            this.QueryReceiveDateStart = qReceiveDateStart;
            this.QueryReceiveDateEnd = qReceiveDateEnd;

            #region 查詢的繳款方式
            this.QueryReceiveWay = this.ddlReceiveWay.SelectedValue;
            #endregion

            #region 查詢的虛擬帳號
            this.QueryCancelNo = this.tbxCancelNo.Text.Trim();
            #endregion

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ccbtnQuery.Visible = this.InitialUI();
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
                    string funcId = "C3700006";
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
                            string fileName = String.Format("{0}_查詢結果.{1}", HttpUtility.UrlEncode(this.QueryReceiveType), extName);
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
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
    }
}