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

namespace eSchoolWeb.B
{
    /// <summary>
    /// 清除虛擬帳號
    /// </summary>
    public partial class B2200004 : BasePage
    {
        #region Property
        /// <summary>
        /// 儲存業務別碼代碼的查詢條件
        /// </summary>
        private string EditReceiveType
        {
            get
            {
                return ViewState["EditReceiveType"] as string;
            }
            set
            {
                ViewState["EditReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存年度代碼的查詢條件
        /// </summary>
        private string EditYearId
        {
            get
            {
                return ViewState["EditYearId"] as string;
            }
            set
            {
                ViewState["EditYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存期別代碼的查詢條件
        /// </summary>
        private string EditTermId
        {
            get
            {
                return ViewState["EditTermId"] as string;
            }
            set
            {
                ViewState["EditTermId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存部門別代碼的查詢條件
        /// </summary>
        private string EditDepId
        {
            get
            {
                return ViewState["EditDepId"] as string;
            }
            set
            {
                ViewState["EditDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存費用別代碼的查詢條件
        /// </summary>
        private string EditReceiveId
        {
            get
            {
                return ViewState["EditReceiveId"] as string;
            }
            set
            {
                ViewState["EditReceiveId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return false;
            }
            #endregion

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

            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
            if (xmlResult.IsSuccess)
            {
                this.EditReceiveType = receiveType;
                this.EditYearId = yearId;
                this.EditTermId = termId;
                this.EditDepId = "";
                this.EditReceiveId = this.ucFilter2.SelectedReceiveID;
            }

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                this.ccbtnOK.Visible = false;
                return false;
            }
            #endregion

            this.ucFilter2_ItemSelectedIndexChanged(this.ucFilter2, null);

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
            WebHelper.SetDropDownListItems(this.ddlUpNo, DefaultItem.Kind.Select, false, items, false, false, 0, null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ccbtnOK.Visible = this.InitialUI();
            }
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            #region 土銀不使用原有部別 DepList，改用專用的部別 DeptList
            //this.EditDepId = this.ucFilter2.SelectedDepID;
            #endregion

            this.EditDepId = "";
            this.EditReceiveId = this.ucFilter2.SelectedReceiveID;
            this.GetAndBindUpNoOptions(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);

            this.ccbtnOK.Enabled = (this.ddlUpNo.Items.Count > 0);
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region 5 Key
            string receiveType = this.EditReceiveType;
            string yearId = this.EditYearId;
            string termId = this.EditTermId;
            string depId = this.EditDepId;
            string receiveId = this.EditReceiveId;
            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("業務別碼");
                return;
            }
            if (String.IsNullOrEmpty(yearId))
            {
                this.ShowMustInputAlert("學年");
                return;
            }
            if (String.IsNullOrEmpty(termId))
            {
                this.ShowMustInputAlert("學期");
                return;
            }

            #region 土銀不使用原有部別 DepList，改用專用的部別 DeptList
            //if (String.IsNullOrEmpty(depId))
            //{
            //    this.ShowMustInputAlert("部別");
            //    return;
            //}
            #endregion

            depId = "";

            if (String.IsNullOrEmpty(receiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return;
            }
            #endregion

            string byKind = null;
            string byValue = null;
            string byKindName = null;
            if (this.rbtByUpNo.Checked)
            {
                byKind = "ByUpNo";
                byValue = this.ddlUpNo.SelectedValue;
                byKindName = "指定批號";
            }
            else if (this.rbtByStuId.Checked)
            {
                byKind = "ByStuId";
                byValue = this.tbxStuId.Text.Trim();

                #region [MDY:2018xxxx] 改提供多筆學號，以逗號隔開
                {
                    string[] tmps = byValue.Replace(" ", "").Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmps.Length > 100)
                    {
                        string msg = this.GetLocalized("指定學號最多 100 筆");
                        this.ShowSystemMessage(msg);
                        return;
                    }
                    byValue = String.Join(",", tmps);
                }
                #endregion

                byKindName = "指定學號";
            }
            else if (this.rbtByCancelNo.Checked)
            {
                byKind = "ByCancelNo";
                byValue = this.tbxCancelNo.Text.Trim();
                byKindName = "指定虛擬帳號";
            }
            if (String.IsNullOrEmpty(byKind))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("請選擇「指定批號」、「指定學號」或「指定虛擬帳號」");
                this.ShowSystemMessage(msg);
                return;
            }
            if (String.IsNullOrEmpty(byValue))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized(String.Concat("請設定「", byKindName, "」的值"));
                this.ShowSystemMessage(msg);
                return;
            }

            string action = this.GetLocalized("清除虛擬帳號");
            string log = null;
            XmlResult xmlResult = DataProxy.Current.ClearCancelNo(this.Page, receiveType, yearId, termId, depId, receiveId, byKind, byValue, out log);
            if (xmlResult.IsSuccess)
            {
                this.ShowActionSuccessMessage(action);
                this.labLog.Text = log;
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                this.labLog.Text = String.Empty;
            }
        }
    }
}