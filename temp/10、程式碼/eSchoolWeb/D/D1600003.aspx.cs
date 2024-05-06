using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using Fuju.Configuration;

using Entities;
using Helpers;

namespace eSchoolWeb.D
{
    /// <summary>
    /// 查詢中國信託繳費資料 (查詢異業代收款檔-中國信託資料)
    /// </summary>
    public partial class D1600003 : PagingBasePage
    {
        #region Const
        /// <summary>
        /// 此功能只查 中國信託 的異業代收檔
        /// </summary>
        private readonly string QEDPChannelId = "CNTRUST";
        #endregion

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
        /// 儲存查詢的中信帳務處理日區間起日
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
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["QuerySDate"] = value;
            }
        }

        /// <summary>
        /// 儲存查詢的中信帳務處理日區間訖日
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
                else
                {
                    return null;
                }
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

            string qReceiveType = this.QueryReceiveType;
            DateTime? qSDate = this.QuerySDate;
            DateTime? qEDate = this.QueryEDate;

            #region 商家代號
            if (String.IsNullOrEmpty(qReceiveType))
            {
                return new XmlResult(false, "無法取得商家代號查詢條件");
            }
            LogonUser logonUser = this.GetLogonUser();
            if (String.IsNullOrEmpty(qReceiveType) || !logonUser.IsAuthReceiveTypes(qReceiveType))
            {
                return new XmlResult(false, "該商家代號未授權");
            }
            #endregion

            #region 中信帳務處理日區間
            if (!qSDate.HasValue || !qEDate.HasValue)
            {
                return new XmlResult(false, "無法取得中信帳務處理日區間查詢條件");
            }
            #endregion

            #region 預設條件 (異業管道代碼 = CNTRUST)
            where = new Expression(EDPDataView.Field.EDPChannelId, this.QEDPChannelId);  //中國信託
            #endregion

            where.And(EDPDataView.Field.ReceiveType, qReceiveType)
                .And(EDPDataView.Field.TranferDate, RelationEnum.GreaterEqual, qSDate.Value.Date)
                .And(EDPDataView.Field.TranferDate, RelationEnum.LessEqual, qEDate.Value.Date);

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(EDPDataView.Field.SN, OrderByEnum.Asc);

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
            XmlResult xmlResult = base.QueryDataAndBind<EDPDataView>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }

            #region 查詢結果 UI 是否顯示
            {
                //沒資料不顯示分頁控制項
                bool showPaging = this.gvResult.Rows.Count > 0;
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = showPaging;
                }
                //因為無資料會顯示查無資料，所以一律要顯示查詢結果清單
                this.gvResult.Visible = true;
            }
            #endregion

            return xmlResult;
        }
        #endregion

        #region Private Method
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
                || String.IsNullOrEmpty(receiveType))
            {
                receiveType = this.ucFilter1.SelectedReceiveType;
                yearId = null;
                termId = null;
                depId = null;
                receiveId = null;
            }

            //因為控制項的 Page_Load 比頁面的 Page_Load 晚觸發，所以可以先執行 GetDataAndBind 方法，強迫 ucFilter1 結繫資料
            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
            if (xmlResult.IsSuccess && String.IsNullOrEmpty(receiveType))
            {
                receiveType = this.ucFilter1.SelectedReceiveType;
            }

            //一定要用這個方法將商家代號、學年、學期、部別、代收費用別參數傳給下一頁
            //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
            WebHelper.SetFilterArguments(receiveType, yearId, termId, depId, receiveId);
            #endregion

            #region 中信帳務處理日區間
            {
                this.tbxQSDate.Text = DataFormat.GetDateText(DateTime.Today);
                this.tbxQEDate.Text = DataFormat.GetDateText(DateTime.Today);  //因為 checkmarx 會誤判，所以不能直接用 tbxQSDate.Text 只能在 format 一次
            }
            #endregion

            #region 查詢結果初始化
            {
                this.InitialQueryResult();
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
            #region 商家代號
            string qReceiveType = this.ucFilter1.SelectedReceiveType;
            if (String.IsNullOrEmpty(qReceiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return false;
            }
            LogonUser logonUser = this.GetLogonUser();
            if (!logonUser.IsAuthReceiveTypes(qReceiveType))
            {
                string msg = this.GetLocalized("該商家代號未授權");
                this.ShowJsAlert(msg);
                return false;
            }
            #endregion

            #region 記住 5 Key
            WebHelper.SetFilterArguments(qReceiveType, null, null, null, null);
            #endregion

            #region 中信帳務處理日區間
            DateTime? qSDate = null, qEDate = null;
            {
                #region 起日
                {
                    string value = this.tbxQSDate.Text.Trim();
                    if (String.IsNullOrEmpty(value))
                    {
                        this.ShowMustInputAlert("中信帳務處理日區間起日");
                        return false;
                    }
                    DateTime date;
                    if (DateTime.TryParse(value, out date))
                    {
                        qSDate = date;
                    }
                    else
                    {
                        string msg = this.GetLocalized("「中信帳務處理日區間起日」不是合法的日期格式");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
                #endregion

                #region 訖日
                {
                    string value = this.tbxQEDate.Text.Trim();
                    if (String.IsNullOrEmpty(value))
                    {
                        this.ShowMustInputAlert("中信帳務處理日區間訖日");
                        return false;
                    }
                    DateTime date;
                    if (DateTime.TryParse(value, out date))
                    {
                        qEDate = date;
                    }
                    else
                    {
                        string msg = this.GetLocalized("「中信帳務處理日區間訖日」不是合法的日期格式");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
                #endregion

                if (qSDate.HasValue && qEDate.HasValue)
                {
                    if (qSDate.Value > qEDate.Value)
                    {
                        DateTime date = qSDate.Value;
                        qSDate = qEDate.Value;
                        qEDate = date;
                    }
                    if ((qEDate.Value - qSDate.Value).TotalDays >= 30)
                    {
                        string msg = this.GetLocalized("「中信帳務處理日區間」最多只能 30 天");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
                if (qSDate.HasValue)
                {
                    this.tbxQSDate.Text = DataFormat.GetDateText(qSDate.Value);
                }
                if (qEDate.HasValue)
                {
                    this.tbxQEDate.Text = DataFormat.GetDateText(qEDate.Value);
                }
            }
            #endregion

            this.QueryReceiveType = qReceiveType;
            this.QuerySDate = qSDate;
            this.QueryEDate = qEDate;

            return true;
        }

        /// <summary>
        /// 清除查詢結果 (查詢結果初始化)
        /// </summary>
        private void InitialQueryResult()
        {
            this.gvResult.DataSource = null;
            this.gvResult.DataBind();
            Paging[] ucPagings = this.GetPagingControls();
            foreach (Paging ucPaging in ucPagings)
            {
                ucPaging.Visible = false;
            }
            this.gvResult.Visible = false;
        }

        /// <summary>
        /// 匯出資料檔
        /// </summary>
        /// <param name="fileType">指定檔案型別 (XLS 或 ODS)</param>
        private void ExportDataFile(string fileType)
        {
            #region 先清查詢結果
            {
                this.InitialQueryResult();
            }
            #endregion

            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                string msg = this.GetLocalized("無查詢權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            #region 商家代號
            string qReceiveType = this.ucFilter1.SelectedReceiveType;
            if (String.IsNullOrEmpty(qReceiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }
            LogonUser logonUser = this.GetLogonUser();
            if (!logonUser.IsAuthReceiveTypes(qReceiveType))
            {
                string msg = this.GetLocalized("該商家代號未授權");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            #region 記住 5 Key
            WebHelper.SetFilterArguments(qReceiveType, null, null, null, null);
            #endregion

            #region 中信帳務處理日區間
            DateTime? qSDate = null, qEDate = null;
            {
                #region 起日
                {
                    string value = this.tbxQSDate.Text.Trim();
                    if (String.IsNullOrEmpty(value))
                    {
                        this.ShowMustInputAlert("中信帳務處理日區間起日");
                        return;
                    }
                    DateTime date;
                    if (DateTime.TryParse(value, out date))
                    {
                        qSDate = date;
                    }
                    else
                    {
                        string msg = this.GetLocalized("「中信帳務處理日區間起日」不是合法的日期格式");
                        this.ShowJsAlert(msg);
                        return;
                    }
                }
                #endregion

                #region 訖日
                {
                    string value = this.tbxQEDate.Text.Trim();
                    if (String.IsNullOrEmpty(value))
                    {
                        this.ShowMustInputAlert("中信帳務處理日區間訖日");
                        return;
                    }
                    DateTime date;
                    if (DateTime.TryParse(value, out date))
                    {
                        qEDate = date;
                    }
                    else
                    {
                        string msg = this.GetLocalized("「中信帳務處理日區間訖日」不是合法的日期格式");
                        this.ShowJsAlert(msg);
                        return;
                    }
                }
                #endregion

                if (qSDate.HasValue && qEDate.HasValue)
                {
                    if (qSDate.Value > qEDate.Value)
                    {
                        DateTime date = qSDate.Value;
                        qSDate = qEDate.Value;
                        qEDate = date;
                    }
                    if ((qEDate.Value - qSDate.Value).TotalDays >= 30)
                    {
                        string msg = this.GetLocalized("「中信帳務處理日區間」最多只能 30 天");
                        this.ShowJsAlert(msg);
                        return;
                    }
                }
                if (qSDate.HasValue)
                {
                    this.tbxQSDate.Text = DataFormat.GetDateText(qSDate.Value);
                }
                if (qEDate.HasValue)
                {
                    this.tbxQEDate.Text = DataFormat.GetDateText(qEDate.Value);
                }
            }
            #endregion

            KeyValueList<string> args = new KeyValueList<string>();
            args.Add("EDPChannelId", this.QEDPChannelId);
            args.Add("ReceiveType", qReceiveType);
            args.Add("SDate", qSDate.Value.ToString("yyyy/MM/dd"));
            args.Add("EDate", qEDate.Value.ToString("yyyy/MM/dd"));
            args.Add("FileType", fileType);

            object returnData = null;
            XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.ExportEDPData, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                byte[] fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    this.ShowSystemMessage(this.GetLocalized("資料匯出失敗") + "，未回傳資料或回傳資料型別錯誤");
                }
                else
                {
                    this.ResponseFile(String.Format("{0}中國信託繳費資料.{1}", qReceiveType, fileType), fileContent);
                }
            }
            else
            {
                this.ShowSystemMessage(this.GetLocalized("資料匯出失敗") + "，" + xmlResult.Message);
                return;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bool isOK = this.InitialUI();
                this.ccbtnQuery.Visible = isOK;
                this.ccbtnExportXls.Visible = isOK;

                #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                this.ccbtnExportOds.Visible = true;
                #endregion
            }
        }

        protected void ccbtnQuery_Click(object sender, EventArgs e)
        {
            #region 先清查詢結果
            {
                this.InitialQueryResult();
            }
            #endregion

            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                string msg = this.GetLocalized("無查詢權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            if (this.GetAndKeepQueryCondition())
            {
                PagingInfo pagingInfo = new PagingInfo(PagingInfo.DefaultPageSize, 0, 0);
                XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);

                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                }
            }
        }

        protected void ccbtnExportXls_Click(object sender, EventArgs e)
        {
            this.ExportDataFile("XLS");
        }

        protected void ccbtnExportOds_Click(object sender, EventArgs e)
        {
            this.ExportDataFile("ODS");
        }
    }
}