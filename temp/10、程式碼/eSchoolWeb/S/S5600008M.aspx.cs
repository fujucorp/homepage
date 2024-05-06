using System;
using System.Collections.Generic;
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
    /// 信用卡服務銀行管理 (維護)
    /// </summary>
    public partial class S5600008M : BasePage
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
        /// 編輯的銀行代碼參數
        /// </summary>
        private string EditBankId
        {
            get
            {
                return ViewState["EditBankId"] as string;
            }
            set
            {
                ViewState["EditBankId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxBankId.Text = String.Empty;
            this.tbxBankName.Text = String.Empty;

            WebHelper.SetDropDownListItems(this.ddlAp, DefaultItem.Kind.Select, false, new CCardApCodeTexts(), false, true, 0, null);

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(CCardBankIdDtlEntity data)
        {
            if (data == null)
            {
                this.tbxBankId.Text = String.Empty;
                this.tbxBankName.Text = String.Empty;
                this.ddlAp.SelectedIndex = -1;
                this.ccbtnOK.Visible = false;
                return;
            }

            #region PKey 欄位
            bool isPKeyEditable = ActionMode.IsPKeyEditableMode(this.Action);
            this.tbxBankId.Enabled = isPKeyEditable;
            #endregion

            #region 資料欄位
            bool isDataEditable = ActionMode.IsDataEditableMode(this.Action);
            this.tbxBankName.Enabled = isDataEditable;
            this.ddlAp.Enabled = isDataEditable;
            #endregion

            this.tbxBankId.Text = data.BankId;
            this.tbxBankName.Text = data.BankName;
            WebHelper.SetDropDownListSelectedValue(this.ddlAp, data.ApNo.ToString());

            this.ccbtnOK.Visible = true;
        }

        private CCardBankIdDtlEntity GetEditData()
        {
            CCardBankIdDtlEntity data = new CCardBankIdDtlEntity();

            #region PKey 欄位
            if (ActionMode.IsPKeyEditableMode(this.Action))
            {
                data.BankId = this.tbxBankId.Text.Trim();
            }
            else
            {
                data.BankId = this.EditBankId;
            }
            #endregion

            #region 資料欄位
            data.BankName = this.tbxBankName.Text.Trim();
            int apNo = 0;
            if (Int32.TryParse(this.ddlAp.SelectedValue, out apNo))
            {
                data.ApNo = apNo;
            }
            else
            {
                data.ApNo = 0;
            }
            #endregion

            return data;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData(CCardBankIdDtlEntity data)
        {
            if (String.IsNullOrEmpty(data.BankId))
            {
                this.ShowMustInputAlert("銀行代碼");
                return false;
            }
            if (!Common.IsNumber(data.BankId, 1, 3))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「銀行代碼」限輸入 3 碼的數字");
                this.ShowJsAlert(msg);
                return false;
            }

            if (ActionMode.IsDataEditableMode(this.Action))
            {
                if (String.IsNullOrEmpty(data.BankName))
                {
                    this.ShowMustInputAlert("銀行名稱");
                    return false;
                }

                if (String.IsNullOrEmpty(data.ApName))
                {
                    this.ShowMustInputAlert("信用卡繳費平台");
                    return false;
                }
            }

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    return;
                }
                #endregion

                #region 處理參數
                KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                if (QueryString == null || QueryString.Count == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }

                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.EditBankId = QueryString.TryGetValue("BankId", String.Empty);

                if (!ActionMode.IsMaintinaMode(this.Action)
                    || !ActionMode.IsPKeyEditableMode(this.Action) && String.IsNullOrEmpty(this.EditBankId))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                CCardBankIdDtlEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new CCardBankIdDtlEntity();
                        }
                        #endregion
                        break;
                    default:
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(CCardBankIdDtlEntity.Field.BankId, this.EditBankId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<CCardBankIdDtlEntity>(this, where, null, out data);
                            if (!xmlResult.IsSuccess)
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                this.ccbtnOK.Visible = false;
                                return;
                            }
                            if (data == null)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                this.ccbtnOK.Visible = false;
                                return;
                            }
                            #endregion
                        }
                        #endregion
                        break;
                }
                #endregion

                this.BindEditData(data);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            CCardBankIdDtlEntity data = this.GetEditData();
            if (!this.CheckEditData(data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600008.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        #region 補齊資料
                        data.UpdateWho = this.GetLogonUser().UserId;
                        data.CreateDate = DateTime.Now;
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<CCardBankIdDtlEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                            }
                            else
                            {
                                this.ShowActionSuccessAlert(action, backUrl);
                            }
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                    }
                    #endregion
                    break;
                case ActionMode.Modify:     //修改
                    #region 修改
                    {
                        #region 更新條件
                        Expression where = new Expression(CCardBankIdDtlEntity.Field.BankId, data.BankId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(CCardBankIdDtlEntity.Field.BankName, data.BankName);
                        fieldValues.Add(CCardBankIdDtlEntity.Field.ApNo, data.ApNo);
                        fieldValues.Add(CCardBankIdDtlEntity.Field.UpdateWho, this.GetLogonUser().UserId);
                        fieldValues.Add(CCardBankIdDtlEntity.Field.UpdateDate, DateTime.Now);
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<CCardBankIdDtlEntity>(this, where, fieldValues.ToArray(), out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                this.ShowActionSuccessAlert(action, backUrl);
                            }
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                    }
                    #endregion
                    break;
                case ActionMode.Delete:     //刪除
                    #region 刪除
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Delete<CCardBankIdDtlEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                this.ShowActionSuccessAlert(action, backUrl);
                            }
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                    }
                    #endregion
                    break;
            }
        }
    }
}