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
    /// 繳費銷帳報表
    /// </summary>
    public partial class C3500001 : BasePage
    {
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
                #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
                //depId = this.ucFilter2.SelectedDepID;
                #endregion

                depId = String.Empty;
                receiveId = this.ucFilter2.SelectedReceiveID;
            }
            #endregion

            #region 報表名稱
            this.tbxReportName.Text = String.Empty;
            #endregion

            #region 批號
            if (!this.GetAndBindUpNoOptions(receiveType, yearId, termId, depId, receiveId))
            {
                return false;
            }
            #endregion

            #region 繳費狀態
            CodeText[] items = new CodeText[] { new CodeText("0", "未繳"), new CodeText("1", "已繳") };
            WebHelper.SetDropDownListItems(this.ddlReceiveStatus, DefaultItem.Kind.All, false, items, false, true, 0, null);
            #endregion

            //#region 繳款方式
            //if (!this.GetAndBindReceiveWayOptions())
            //{
            //    return false;
            //}
            //#endregion

            return true;
        }

        /// <summary>
        /// 取得並結繫批號選項
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        private bool GetAndBindUpNoOptions(string receiveType, string yearId, string termId, string depId, string receiveId)
        {
            bool isOK = true;

            #region 取資料
            CodeText[] datas = null;

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

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<StudentReceiveEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (xmlResult.IsSuccess)
                {
                    if (datas == null || datas.Length == 0)
                    {
                        string action = this.GetLocalized("查詢上傳批號資料");
                        this.ShowActionFailureMessage(action, ErrorCode.D_QUERY_NO_DATA, "查無任何批號");
                        isOK = false;
                    }
                }
                else
                {
                    string action = this.GetLocalized("查詢上傳批號資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    isOK = false;
                }
            }
            #endregion

            #region [MDY:2018xxxx] 批號改用數值遞減排序
            if (datas != null)
            {
                WebHelper.SortItemsByValueDesc(ref datas);
            }
            #endregion
            WebHelper.SetDropDownListItems(this.ddlUpNo, DefaultItem.Kind.All, false, datas, false, false, 0, null);
            return isOK;
        }

        ///// <summary>
        ///// 取得並結繫繳款方式選項
        ///// </summary>
        ///// <returns></returns>
        //private bool GetAndBindReceiveWayOptions()
        //{
        //    bool isOK = true;

        //    #region 取資料
        //    CodeText[] datas = null;
        //    Expression where = new Expression(ChannelSetEntity.Field.CategoryId, String.Empty);
        //    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
        //    orderbys.Add(ChannelSetEntity.Field.ChannelId, OrderByEnum.Asc);

        //    string[] codeFieldNames = new string[] { ChannelSetEntity.Field.ChannelId };
        //    string codeCombineFormat = null;
        //    string[] textFieldNames = new string[] { ChannelSetEntity.Field.ChannelName };
        //    string textCombineFormat = null;

        //    XmlResult xmlResult = DataProxy.Current.GetEntityOptions<ChannelSetEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
        //    if (xmlResult.IsSuccess)
        //    {
        //        if (datas == null || datas.Length == 0)
        //        {
        //            string action = this.GetLocalized("查詢繳款方式資料");
        //            this.ShowActionFailureMessage(action, ErrorCode.D_QUERY_NO_DATA, "查無任何繳款方式");
        //            isOK = false;
        //        }
        //    }
        //    else
        //    {
        //        string action = this.GetLocalized("查詢繳款方式資料");
        //        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //        isOK = false;
        //    }
        //    #endregion

        //    WebHelper.SetDropDownListItems(this.ddlReceiveWay, DefaultItem.Kind.All, false, datas, true, false, 0, null);
        //    return isOK;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bool isOK = this.InitialUI();
                this.ccbtnGenRpeortA1.Visible = isOK;
                this.ccbtnGenRpeortB1.Visible = isOK;
                this.ccbtnGenRpeortA2.Visible = isOK;
                this.ccbtnGenRpeortB2.Visible = isOK;
                this.ccbtnGenRpeortE1.Visible = isOK;
                this.ccbtnGenRpeortE2.Visible = isOK;
            }
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            string receiveType = this.ucFilter1.SelectedReceiveType;
            string yearId = this.ucFilter1.SelectedYearID;
            string termId = this.ucFilter1.SelectedTermID;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //depId = this.ucFilter2.SelectedDepID;
            #endregion

            string depId = "";

            string receiveId = this.ucFilter2.SelectedReceiveID;

            bool isOK = this.GetAndBindUpNoOptions(receiveType, yearId, termId, depId, receiveId);
            this.ccbtnGenRpeortA1.Visible = isOK;
            this.ccbtnGenRpeortB1.Visible = isOK;
            this.ccbtnGenRpeortA2.Visible = isOK;
            this.ccbtnGenRpeortB2.Visible = isOK;
        }

        protected void ccbtnGenRpeort_Click(object sender, EventArgs e)
        {
            string reportName = HttpUtility.HtmlEncode(this.tbxReportName.Text.Trim());
            if (String.IsNullOrEmpty(reportName))
            {
                this.ShowMustInputAlert("報表名稱");
                return;
            }

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

            string depId = "";

            string receiveId = this.ucFilter2.SelectedReceiveID;
            if (String.IsNullOrEmpty(receiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return;
            }

            int? upNo = null;
            string upNoTxt = this.ddlUpNo.SelectedValue;
            int value = 0;
            if (!String.IsNullOrEmpty(upNoTxt) && int.TryParse(upNoTxt, out value))
            {
                upNo = value;
            }

            string receiveStatus = this.ddlReceiveStatus.SelectedValue;

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            bool isUseODS = (this.rblFileFormat.SelectedValue == "ODS");
            #endregion

            string action = null;
            byte[] fileContent = null;
            XmlResult xmlResult = null;
            if (sender == this.ccbtnGenRpeortA1)
            {
                action = this.ccbtnGenRpeortA1.Text;

                #region [OLD]
                //xmlResult = DataProxy.Current.ExportReportA(this.Page, receiveType, yearId, termId, depId, receiveId, upNo, receiveStatus, "1", reportName, out fileContent);
                #endregion

                #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                xmlResult = DataProxy.Current.ExportReportA2(this.Page, receiveType, yearId, termId, depId, receiveId, upNo, receiveStatus, "1", reportName, out fileContent, isUseODS);
                #endregion
            }
            else if (sender == this.ccbtnGenRpeortB1)
            {
                action = this.ccbtnGenRpeortB1.Text;

                #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                xmlResult = DataProxy.Current.ExportReportB(this.Page, receiveType, yearId, termId, depId, receiveId, upNo, receiveStatus, "1", reportName, out fileContent, isUseODS);
                #endregion
            }
            else if (sender == this.ccbtnGenRpeortA2)
            {
                action = this.ccbtnGenRpeortA2.Text;

                #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                xmlResult = DataProxy.Current.ExportReportA(this.Page, receiveType, yearId, termId, depId, receiveId, upNo, receiveStatus, "2", reportName, out fileContent, isUseODS);
                #endregion
            }
            else if (sender == this.ccbtnGenRpeortB2)
            {
                action = this.ccbtnGenRpeortB1.Text;

                #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                xmlResult = DataProxy.Current.ExportReportB(this.Page, receiveType, yearId, termId, depId, receiveId, upNo, receiveStatus, "2", reportName, out fileContent, isUseODS);
                #endregion
            }
            else if (sender == this.ccbtnGenRpeortE1)
            {
                action = this.ccbtnGenRpeortE1.Text;

                #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                xmlResult = DataProxy.Current.ExportReportE(this.Page, receiveType, yearId, termId, depId, receiveId, upNo, receiveStatus, "1", reportName, out fileContent, isUseODS);
                #endregion
            }
            else if (sender == this.ccbtnGenRpeortE2)
            {
                action = this.ccbtnGenRpeortE2.Text;

                #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                xmlResult = DataProxy.Current.ExportReportE(this.Page, receiveType, yearId, termId, depId, receiveId, upNo, receiveStatus, "2", reportName, out fileContent, isUseODS);
                #endregion
            }
            else
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要下載的報表種類");
                this.ShowSystemMessage(msg);
                return;
            }
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
                    if (isUseODS)
                    {
                        string fileName = String.Concat(HttpUtility.UrlEncode(reportName), ".ODS");
                        this.ResponseFile(fileName, fileContent, "ODS");
                    }
                    else
                    {
                        string fileName = String.Concat(HttpUtility.UrlEncode(reportName), ".XLS");
                        this.ResponseFile(fileName, fileContent, "XLS");
                    }
                    #endregion
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