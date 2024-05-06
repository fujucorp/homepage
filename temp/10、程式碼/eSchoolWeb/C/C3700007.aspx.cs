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
    /// 下載銷帳資料(固定格式)
    /// </summary>
    public partial class C3700007 : BasePage
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

        ///// <summary>
        ///// 儲存查詢的銷帳狀態
        ///// </summary>
        //private string QueryCancelStatus
        //{
        //    get
        //    {
        //        return ViewState["QueryCancelStatus"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryCancelStatus"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 儲存查詢的繳款方式
        ///// </summary>
        //private string QueryReceiveWay
        //{
        //    get
        //    {
        //        return ViewState["QueryReceiveWay"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryReceiveWay"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 儲存查詢的代收日區間起日
        ///// </summary>
        //private string QueryReceiveDateStart
        //{
        //    get
        //    {
        //        return ViewState["QueryReceiveDateStart"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryReceiveDateStart"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 儲存查詢的代收日區間迄日
        ///// </summary>
        //private string QueryReceiveDateEnd
        //{
        //    get
        //    {
        //        return ViewState["QueryReceiveDateEnd"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryReceiveDateEnd"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 儲存查詢的入帳日區間起日
        ///// </summary>
        //private string QueryAccountDateStart
        //{
        //    get
        //    {
        //        return ViewState["QueryAccountDateStart"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryAccountDateStart"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 儲存查詢的入帳日區間迄日
        ///// </summary>
        //private string QueryAccountDateEnd
        //{
        //    get
        //    {
        //        return ViewState["QueryAccountDateEnd"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryAccountDateEnd"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 儲存查詢的查詢欄位 (StuId=學號 / CancelNo=銷帳編號 / IdNumber=身分證字號)
        ///// </summary>
        //private string QuerySearchField
        //{
        //    get
        //    {
        //        return ViewState["QuerySearchField"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QuerySearchField"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 儲存查詢的查詢值
        ///// </summary>
        //private string QuerySearchValue
        //{
        //    get
        //    {
        //        return ViewState["QuerySearchValue"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QuerySearchValue"] = value == null ? null : value.Trim();
        //    }
        //}
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

            this.ucFilter2_ItemSelectedIndexChanged(this.ucFilter2, null);

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

                WebHelper.SetDropDownListItems(this.ddlReceiveWay, DefaultItem.Kind.All, false, datas, true, false, 0, null);
            }
            #endregion

            #region 查詢特定欄位值
            {
                CodeText[] items = new CodeText[] { new CodeText("StuId", "學號"), new CodeText("CancelNo", "虛擬帳號"), new CodeText("IdNumber", "身分證字號") };
                WebHelper.SetRadioButtonListItems(this.rdoSearchField, items, true, 2, items[0].Code);
            }
            #endregion

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
            WebHelper.SetDropDownListItems(this.ddlUpNo, DefaultItem.Kind.All, false, items, false, false, 0, null);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ccbtnDownload.Visible = this.InitialUI();
            }
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            this.QueryDepId = "";   //土銀不使用原有部別，所以固定為空字串
            this.QueryReceiveId = this.ucFilter2.SelectedReceiveID;
            this.GetAndBindUpNoOptions(this.QueryReceiveType, this.QueryYearId, this.QueryTermId, this.QueryDepId, this.QueryReceiveId);
        }

        protected void ccbtnDownload_Click(object sender, EventArgs e)
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

            #region 查詢條件
            #region 批號
            string qUpNo = this.ddlUpNo.SelectedValue;
            #endregion

            #region 銷帳狀態
            string qCancelStatus = CancelStatusCodeTexts.CANCELED;  //固定為已銷帳
            #endregion

            #region [Old] 代收日區間與入帳日區間條件改為二擇一
            //#region 繳款方式 + 代收日區間
            //string qReceiveWay = null;
            //string qSReceivDate = null;
            //string qEReceivDate = null;
            //if (qCancelStatus != CancelStatusCodeTexts.NONPAY)
            //{
            //    //繳款方式
            //    qReceiveWay = this.ddlReceiveWay.SelectedValue;

            //    #region 代收日區間的起日
            //    qSReceivDate = this.tbxSReceiveDate.Text.Trim();
            //    if (!String.IsNullOrEmpty(qSReceivDate))
            //    {
            //        DateTime date;
            //        if (DateTime.TryParse(qSReceivDate, out date) && date.Year >= 1911)
            //        {
            //            qSReceivDate = date.ToString("yyyy/MM/dd");
            //        }
            //        else
            //        {
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「代收日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
            //            this.ShowJsAlert(msg);
            //            return;
            //        }
            //    }
            //    #endregion

            //    #region 代收日區間的迄日
            //    qEReceivDate = this.tbxEReceiveDate.Text.Trim();
            //    if (!String.IsNullOrEmpty(qEReceivDate))
            //    {
            //        DateTime date;
            //        if (DateTime.TryParse(qEReceivDate, out date) && date.Year >= 1911)
            //        {
            //            qEReceivDate = date.ToString("yyyy/MM/dd");
            //        }
            //        else
            //        {
            //            qEReceivDate = null;
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「代收日區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
            //            this.ShowJsAlert(msg);
            //            return;
            //        }
            //    }
            //    #endregion
            //}
            //#endregion

            //#region 入帳日區間
            //string qSAccountDate = null;
            //string qEAccountDate = null;
            //if (qCancelStatus == CancelStatusCodeTexts.CANCELED)
            //{
            //    #region 入帳日區間的起日
            //    qSAccountDate = this.tbxSAccountDate.Text.Trim();
            //    if (!String.IsNullOrEmpty(qSAccountDate))
            //    {
            //        DateTime date;
            //        if (DateTime.TryParse(qSAccountDate, out date) && date.Year >= 1911)
            //        {
            //            qSAccountDate = date.ToString("yyyy/MM/dd");
            //        }
            //        else
            //        {
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「入帳日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
            //            this.ShowJsAlert(msg);
            //            return;
            //        }
            //    }
            //    #endregion

            //    #region 入帳日區間的迄日
            //    qEAccountDate = this.tbxEAccountDate.Text.Trim();
            //    if (!String.IsNullOrEmpty(qEAccountDate))
            //    {
            //        DateTime date;
            //        if (DateTime.TryParse(qEAccountDate, out date) && date.Year >= 1911)
            //        {
            //            qEAccountDate = date.ToString("yyyy/MM/dd");
            //        }
            //        else
            //        {
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「入帳日區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
            //            this.ShowJsAlert(msg);
            //            return;
            //        }
            //    }
            //    #endregion
            //}
            //#endregion
            #endregion

            #region [New] 代收日區間與入帳日區間條件改為二擇一
            //繳款方式
            string qReceiveWay = this.ddlReceiveWay.SelectedValue;

            string qSReceivDate = null;
            string qEReceivDate = null;
            string qSAccountDate = null;
            string qEAccountDate = null;
            if (this.rbtReceiveDate.Checked)
            {
                #region 代收日區間的起日
                qSReceivDate = this.tbxSReceiveDate.Text.Trim();
                if (String.IsNullOrEmpty(qSReceivDate))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowMustInputAlert("代收日區間的起日");
                    return;
                }
                else
                {
                    DateTime date;
                    if (DateTime.TryParse(qSReceivDate, out date) && date.Year >= 1911)
                    {
                        qSReceivDate = date.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「代收日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                        this.ShowJsAlert(msg);
                        return;
                    }
                }
                #endregion

                #region 代收日區間的迄日
                qEReceivDate = this.tbxEReceiveDate.Text.Trim();
                if (String.IsNullOrEmpty(qEReceivDate))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowMustInputAlert("代收日區間的迄日");
                    return;
                }
                else
                {
                    DateTime date;
                    if (DateTime.TryParse(qEReceivDate, out date) && date.Year >= 1911)
                    {
                        qEReceivDate = date.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「代收日區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                        this.ShowJsAlert(msg);
                        return;
                    }
                }
                #endregion
            }
            else if (this.rbtAccountDate.Checked)
            {
                #region 入帳日區間的起日
                qSAccountDate = this.tbxSAccountDate.Text.Trim();
                if (String.IsNullOrEmpty(qSAccountDate))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowMustInputAlert("入帳日區間的起日");
                    return;
                }
                else
                {
                    DateTime date;
                    if (DateTime.TryParse(qSAccountDate, out date) && date.Year >= 1911)
                    {
                        qSAccountDate = date.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「入帳日區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                        this.ShowJsAlert(msg);
                        return;
                    }
                }
                #endregion

                #region 入帳日區間的迄日
                qEAccountDate = this.tbxEAccountDate.Text.Trim();
                if (String.IsNullOrEmpty(qEAccountDate))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowMustInputAlert("入帳日區間的迄日");
                    return;
                }
                else
                {
                    DateTime date;
                    if (DateTime.TryParse(qEAccountDate, out date) && date.Year >= 1911)
                    {
                        qEAccountDate = date.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「入帳日區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                        this.ShowJsAlert(msg);
                        return;
                    }
                }
                #endregion
            }
            #endregion

            #region 查詢欄位與值
            string qFieldName = this.rdoSearchField.SelectedValue;
            string qFieldValue = this.tbxSearchValue.Text.Trim();
            #endregion
            #endregion

            string action = this.GetLocalized("下載銷帳資料(固定格式)");
            Byte[] fileContent = null;

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            XmlResult xmlResult = DataProxy.Current.ExportC3700007File(this.Page, this.QueryReceiveType, this.QueryYearId, this.QueryTermId, this.QueryDepId, this.QueryReceiveId
                , qUpNo, qCancelStatus, qReceiveWay, qSReceivDate, qEReceivDate, qSAccountDate, qEAccountDate, qFieldName, qFieldValue, out fileContent, (extName == "ODS"));
            #endregion

            if (xmlResult.IsSuccess)
            {
                if (fileContent == null || fileContent.Length == 0)
                {
                    this.ShowActionFailureMessage(action, "無資料被匯出");
                }
                else
                {
                    #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                    #region [MDY:20210401] 原碼修正
                    string fileName = String.Format("{0}_{1}_{2}_{3}_{4:yyyyMMdd}.{5}", HttpUtility.UrlEncode(this.QueryReceiveType), HttpUtility.UrlEncode(this.QueryYearId), HttpUtility.UrlEncode(this.QueryTermId), HttpUtility.UrlEncode(this.QueryReceiveId), DateTime.Now, extName);
                    #endregion
                    this.ResponseFile(fileName, fileContent, extName);
                    #endregion
                }
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }
    }
}