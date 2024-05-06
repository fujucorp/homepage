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
    /// 學生繳費名冊
    /// </summary>
    public partial class C3500002 : BasePage
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
            {
                CodeText[] items = new CodeText[] { new CodeText("0", "未繳"), new CodeText("1", "已繳") };
                WebHelper.SetDropDownListItems(this.ddlReceiveStatus, DefaultItem.Kind.All, false, items, false, true, 0, null);
            }
            #endregion

            #region 群組明細程度
            {
                CodeText[] items = new CodeText[] { new CodeText("1", "部別、系所、班別"), new CodeText("2", "部別、系所、年級、班別") };
                WebHelper.SetRadioButtonListItems(this.rblGroupKind, items, true, 2, items[0].Code);
            }
            #endregion

            #region 說明項目
            {
                CodeText[] items = new CodeText[] { new CodeText("1", "繳款日期"), new CodeText("2", "身份註記"), new CodeText("3", "減免"), new CodeText("4", "就貸") };
                WebHelper.SetCheckBoxListItems(this.cblOtherItems, items, true, 2, null);
            }
            #endregion

            #region 收入科目
            if (!this.GetAndBindReceiveItems(receiveType, yearId, termId, depId, receiveId))
            {
                return false;
            }
            #endregion

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
                string action = this.GetLocalized("查詢上傳批號資料");
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
                        this.ShowActionFailureMessage(action, ErrorCode.D_QUERY_NO_DATA, "查無任何批號");
                        isOK = false;
                    }
                }
                else
                {
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

        /// <summary>
        /// 取得並結繫收入科目選項
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <returns></returns>
        private bool GetAndBindReceiveItems(string receiveType, string yearId, string termId, string depId, string receiveId)
        {
            bool isOK = true;

            #region 取資料
            CodeTextList items = null;

            #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
            //if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) && !String.IsNullOrEmpty(termId)
            //    && !String.IsNullOrEmpty(depId) && !String.IsNullOrEmpty(receiveId))
            #endregion

            if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId)
                && !String.IsNullOrEmpty(termId) && !String.IsNullOrEmpty(receiveId))
            {
                string action = this.GetLocalized("查詢代收費用別設定");
                Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
                    .And(SchoolRidEntity.Field.YearId, yearId)
                    .And(SchoolRidEntity.Field.TermId, termId)
                    .And(SchoolRidEntity.Field.DepId, depId)
                    .And(SchoolRidEntity.Field.ReceiveId, receiveId);

                SchoolRidEntity data = null;
                XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRidEntity>(this.Page, where, null, out data);
                if (xmlResult.IsSuccess)
                {
                    if (data == null)
                    {
                        this.ShowActionFailureMessage(action, ErrorCode.D_QUERY_NO_DATA, "查無該代收費用別設定");
                        isOK = false;
                    }
                    else
                    {
                        items = new CodeTextList(40);
                        string[] itemNames = new string[] {
                            "", 
                            data.ReceiveItem01, data.ReceiveItem02, data.ReceiveItem03, data.ReceiveItem04, data.ReceiveItem05,
                            data.ReceiveItem06, data.ReceiveItem07, data.ReceiveItem08, data.ReceiveItem09, data.ReceiveItem10,
                            data.ReceiveItem11, data.ReceiveItem12, data.ReceiveItem13, data.ReceiveItem14, data.ReceiveItem15,
                            data.ReceiveItem16, data.ReceiveItem17, data.ReceiveItem18, data.ReceiveItem19, data.ReceiveItem20,
                            data.ReceiveItem21, data.ReceiveItem22, data.ReceiveItem23, data.ReceiveItem24, data.ReceiveItem25,
                            data.ReceiveItem26, data.ReceiveItem27, data.ReceiveItem28, data.ReceiveItem29, data.ReceiveItem30,
                            data.ReceiveItem31, data.ReceiveItem32, data.ReceiveItem33, data.ReceiveItem34, data.ReceiveItem35,
                            data.ReceiveItem36, data.ReceiveItem37, data.ReceiveItem38, data.ReceiveItem39, data.ReceiveItem40
                        };
                        for (int no = 1; no < itemNames.Length; no++)
                        {
                            string itemName = itemNames[no];
                            if (!String.IsNullOrWhiteSpace(itemName))
                            {
                                items.Add(no.ToString(), itemName);
                            }
                        }
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    isOK = false;
                }
            }
            #endregion

            WebHelper.SetRadioButtonListItems(this.rblReceiveItems, items, false, 2, null);
            return isOK;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bool isOK = this.InitialUI();
                this.ccbtnGenRpeortC.Visible = isOK;
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
            isOK &= this.GetAndBindReceiveItems(receiveType, yearId, termId, depId, receiveId);
            this.ccbtnGenRpeortC.Visible = isOK;
        }

        protected void ccbtnGenRpeortC_Click(object sender, EventArgs e)
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

            string reportName = this.tbxReportName.Text.Trim();
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
            if (!String.IsNullOrEmpty(upNoTxt) && Int32.TryParse(upNoTxt, out value))
            {
                upNo = value;
            }

            string receiveStatus = this.ddlReceiveStatus.SelectedValue;

            string groupKind = this.rblGroupKind.SelectedValue;
            if (String.IsNullOrEmpty(groupKind))
            {
                this.ShowMustInputAlert("群組明細程度");
                return;
            }

            #region 說明項目
            List<string> otherItems = new List<string>(4);
            foreach (ListItem item in this.cblOtherItems.Items)
            {
                if (item.Selected)
                {
                    otherItems.Add(item.Value);
                }
            }
            #endregion

            #region 收入科目
            int receiveItemNo = 0;
            {
                string text = this.rblReceiveItems.SelectedValue;
                if (String.IsNullOrEmpty(text) || !Int32.TryParse(text, out receiveItemNo) || receiveItemNo < 1 || receiveItemNo > 40)
                {
                    this.ShowMustInputAlert("收入科目");
                    return;
                }
            }
            #endregion

            string action = this.GetLocalized("學生繳費名冊");
            byte[] fileContent = null;

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            XmlResult xmlResult = DataProxy.Current.ExportReportC(this.Page, receiveType, yearId, termId, depId, receiveId
                , upNo, receiveStatus, groupKind, (otherItems.Count > 0 ? otherItems.ToArray() : new string[0]), receiveItemNo, reportName
                , out fileContent, (extName == "ODS"));
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
                    string fileName = String.Format("{0}.{1}", HttpUtility.UrlEncode(reportName), extName);
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