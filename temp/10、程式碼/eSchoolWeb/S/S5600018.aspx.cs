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

namespace eSchoolWeb.S
{
    /// <summary>
    /// [MDY:2018xxxx] 資料保留年限設定
    /// </summary>
    public partial class S5600018 : BasePage
    {
        #region Keep 頁面參數
        /// <summary>
        /// 編輯模式參數
        /// </summary>
        private string Action
        {
            get
            {
                return ViewState["ACTION"] as string;
            }
            set
            {
                ViewState["ACTION"] = value == null ? null : value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// 編輯的商家代號參數
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
        /// 編輯的學校代碼參數
        /// </summary>
        private string EditSchIdenty
        {
            get
            {
                return ViewState["EditSchIdenty"] as string;
            }
            set
            {
                ViewState["EditSchIdenty"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 介面初始化
        /// </summary>
        private void InitialUI()
        {
            if (!this.IsPostBack)
            {
                #region 線上資料保留學年數
                {
                    CodeTextList items = new CodeTextList();
                    items.Add("0", "不限學年數");
                    for (int year = 1; year <= SchoolRTypeEntity.MaxKeepDataYear; year++)
                    {
                        string code = year.ToString();
                        string text = String.Format("{0:00}個學年", year);
                        items.Add(code, text);
                    }
                    WebHelper.SetDropDownListItems(this.ddlKeepDataYear, DefaultItem.Kind.Select, false, items, false, false, 0, null);
                }
                #endregion

                #region 歷史資料保留學年數
                {
                    CodeTextList items = new CodeTextList();
                    items.Add("0", "不限學年數");
                    for (int year = 1; year <= SchoolRTypeEntity.MaxKeepHistoryYear; year++)
                    {
                        string code = year.ToString();
                        string text = String.Format("{0:00}個學年", year);
                        items.Add(code, text);
                    }
                    WebHelper.SetDropDownListItems(this.ddlKeepHistoryYear, DefaultItem.Kind.Select, false, items, false, false, 0, null);
                }
                #endregion

                #region 套用保留學年數設定
                {
                    LogonUser logonUser = this.GetLogonUser();
                    CodeTextList items = new CodeTextList(3);
                    items.Add("ONE", "僅此商家代號套用");
                    if (logonUser.IsBankUser)
                    {
                        items.Add("SCH", "該校所有商家代號皆套用");
                    }
                    if (logonUser.IsBankManager)
                    {
                        items.Add("ALL", "系統所有商家代號皆套用");
                    }
                    WebHelper.SetDropDownListItems(this.ddlCopyTo, DefaultItem.Kind.None, false, items, false, false, 0, "ONE");
                }
                #endregion
            }
        }

        /// <summary>
        /// 讀取並結繫編輯資料
        /// </summary>
        /// <param name="receiveType"></param>
        private void GetAndBindEditData(string receiveType)
        {
            this.EditSchIdenty = null;

            #region 讀取資料
            SchoolRTypeEntity data = null;
            if (!String.IsNullOrEmpty(receiveType))
            {
                string action = this.GetLocalized("讀取商家代號資料");

                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                XmlResult result = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this, where, null, out data);
                if (result.IsSuccess)
                {
                    if (data == null)
                    {
                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "查無商家代號資料");
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(action, result.Code, result.Message);
                }
            }
            #endregion

            #region 結繫資料
            {
                string keepDataYear = null, keepHistoryYear = null;
                if (data == null)
                {
                    keepDataYear = String.Empty;
                    keepHistoryYear = String.Empty;
                }
                else
                {
                    keepDataYear = data.KeepDataYear.HasValue ? data.KeepDataYear.ToString() : "0";           //預設不限學年數
                    keepHistoryYear = data.KeepHistoryYear.HasValue ? data.KeepHistoryYear.ToString() : "0";  //預設不限學年數
                    this.EditSchIdenty = data.SchIdenty;
                }
                WebHelper.SetDropDownListSelectedValue(this.ddlKeepDataYear, keepDataYear);
                WebHelper.SetDropDownListSelectedValue(this.ddlKeepHistoryYear, keepHistoryYear);
            }
            #endregion
        }

        /// <summary>
        /// 取得並檢查編輯資料
        /// </summary>
        /// <param name="keepDataYear">傳回線上資料保留學年數</param>
        /// <param name="keepHistoryYear">傳回歷史資料保留學年數</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetAndCheckEditData(out int? keepDataYear, out int? keepHistoryYear, out string copyTo)
        {
            keepDataYear = null;
            keepHistoryYear = null;
            copyTo = null;

            #region 線上資料保留學年數
            {
                string txtDataYear = this.ddlKeepDataYear.SelectedValue;
                if (String.IsNullOrEmpty(txtDataYear))
                {
                    this.ShowMustInputAlert("線上資料保留學年數");
                    return false;
                }
                int value = 0;
                if (Int32.TryParse(txtDataYear, out value) && value >= 0 && value <= SchoolRTypeEntity.MaxKeepDataYear)
                {
                    keepDataYear = value;
                }
                else
                {
                    this.ShowSystemMessage("無法取得【線上資料保留學年數】設定值");
                    return false;
                }
            }
            #endregion

            #region 歷史資料保留學年數
            {
                string txtHistorYear = this.ddlKeepHistoryYear.SelectedValue;
                if (String.IsNullOrEmpty(txtHistorYear))
                {
                    this.ShowMustInputAlert("歷史資料保留學年數");
                    return false;
                }
                int value = 0;
                if (Int32.TryParse(txtHistorYear, out value) && value >= 0 && value <= SchoolRTypeEntity.MaxKeepHistoryYear)
                {
                    keepHistoryYear = value;
                }
                else
                {
                    this.ShowSystemMessage("無法取【得歷史資料保留學年數】設定值");
                    return false;
                }
            }
            #endregion

            #region 檢查 歷史資料保留學年數 必須大於 線上資料保留學年數
            if (keepDataYear.Value > 0)
            {
                if (keepHistoryYear.Value != 0 && keepHistoryYear.Value <= keepDataYear.Value)
                {
                    this.ShowSystemMessage("歷史資料保留學年數必須大於線上資料保留學年數，或指定為「不限學年數」");
                    return false;
                }
            }
            else
            {
                if (keepHistoryYear.Value != 0)
                {
                    this.ShowSystemMessage("線上資料保留學年數設定為「不限學年數」時，歷史資料保留學年數必須為「不限學年數」");
                    return false;
                }
            }
            #endregion

            #region 套用保留學年數設定
            {
                copyTo = this.ddlCopyTo.SelectedValue;
                if (String.IsNullOrEmpty(copyTo))
                {
                    this.ShowMustInputAlert("套用保留學年數設定");
                    return false;
                }
                LogonUser logonUser = this.GetLogonUser();
                switch (copyTo)
                {
                    case "ONE":
                        #region 僅此商家代號套用
                        if (!logonUser.IsAuthReceiveTypes(this.EditReceiveType))
                        {
                            this.ShowSystemMessage("未授權此商家代號");
                            return false;
                        }
                        #endregion
                        break;
                    case "SCH":
                        #region 該校所有商家代號皆套用
                        if (!logonUser.IsBankUser)
                        {
                            this.ShowSystemMessage("非銀行人員不可指定「該校所有商家代號皆套用」");
                            return false;
                        }
                        if (!logonUser.IsMySchIdenty(this.EditSchIdenty))
                        {
                            this.ShowSystemMessage("未授權此學校");
                            return false;
                        }
                        #endregion
                        break;
                    case "ALL":
                        #region 系統所有商家代號皆套用
                        if (!logonUser.IsBankManager)
                        {
                            this.ShowSystemMessage("非總行人員不可指定「系統所有商家代號皆套用」");
                            return false;
                        }
                        #endregion
                        break;
                    default:
                        this.ShowSystemMessage("無法取得【套用保留學年數設定】設定值");
                        return false;
                }
            }
            #endregion

            return true;
        }

        private bool UpdateEditData(string schIdenty, string receiveType, int keepDataYear, int keepHistoryYear, string copyTo)
        {
            string action = this.GetLocalized("更新資料");

            #region 組更新條件
            Expression where = null;
            switch (copyTo)
            {
                case "ONE":
                    #region 僅此商家代號套用
                    {
                        where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType)
                            .And(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                    }
                    #endregion
                    break;
                case "SCH":
                    #region 該校所有商家代號皆套用
                    {
                        where = new Expression(SchoolRTypeEntity.Field.SchIdenty, schIdenty)
                            .And(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                    }
                    #endregion
                    break;
                case "ALL":
                    #region 系統所有商家代號皆套用
                    {
                        where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                    }
                    #endregion
                    break;
                default:
                    this.ShowSystemMessage("不正確的套用保留學年數設定");
                    return false;
            }
            #endregion

            #region 組更新欄位
            KeyValueList fieldValues = new KeyValueList(4);
            fieldValues.Add(SchoolRTypeEntity.Field.KeepDataYear, keepDataYear);
            fieldValues.Add(SchoolRTypeEntity.Field.KeepHistoryYear, keepHistoryYear);
            fieldValues.Add(SchoolRTypeEntity.Field.MdyDate, DateTime.Now);
            fieldValues.Add(SchoolRTypeEntity.Field.MdyUser, this.GetLogonUser().UserId);
            #endregion

            #region 更新資料
            int count = 0;
            XmlResult xmlResult = DataProxy.Current.UpdateFields<SchoolRTypeEntity>(this, where, fieldValues, out count);
            if (xmlResult.IsSuccess)
            {
                if (count < 1)
                {
                    this.ShowActionFailureMessage(action, "無資料被更新");
                    return false;
                }
                else
                {
                    this.ShowActionSuccessMessage(action);
                    return true;
                }
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return false;
            }
            #endregion
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ccbtnOK.Visible = false;
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    return;
                }
                #endregion

                this.EditReceiveType = ucFilter1.SelectedReceiveType;

                this.GetAndBindEditData(this.EditReceiveType);
            }
        }

        protected void ucFilter1_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ccbtnOK.Visible = false;
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            this.EditReceiveType = ucFilter1.SelectedReceiveType;

            this.GetAndBindEditData(this.EditReceiveType);
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ccbtnOK.Visible = false;
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            int? keepDataYear = null, keepHistoryYear = null;
            string copyTo = null;
            if (this.GetAndCheckEditData(out keepDataYear, out keepHistoryYear, out copyTo))
            {
                this.UpdateEditData(this.EditSchIdenty, this.EditReceiveType, keepDataYear.Value, keepHistoryYear.Value, copyTo);
            }
        }
    }
}