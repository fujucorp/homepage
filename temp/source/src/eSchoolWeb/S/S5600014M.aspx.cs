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
    /// 連動製單帳號管理 (編輯)
    /// </summary>
    public partial class S5600014M : BasePage
    {
        #region Keep 頁面參數
        /// <summary>
        /// 操作模式參數
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
        /// 編輯的系統代碼
        /// </summary>
        private string EditSysId
        {
            get
            {
                return ViewState["EditSysId"] as string;
            }
            set
            {
                ViewState["EditSysId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxSysId.Text = String.Empty;
            this.tbxSysName.Text = String.Empty;

            #region [MDY:20220530] Checkmarx 調整
            this.tbxSysPXX.Text = String.Empty;
            #endregion

            this.tbxClientIp.Text = String.Empty;
            this.tbxSchReceiveUrl.Text = String.Empty;

            this.GetAndBindSchIdentyOptions(null);
        }

        /// <summary>
        /// 取得並結繫學校選項
        /// </summary>
        private void GetAndBindSchIdentyOptions(string selectedValue)
        {
            LogonUser logonUser = this.GetLogonUser();

            Expression where = null;
            if (logonUser.IsBankManager)
            {
                where = new Expression();
            }
            else if (logonUser.IsBankUser)
            {
                where = new Expression(SchoolRTypeEntity.Field.BankId, logonUser.BankId);
            }
            else
            {
                //行員專用
                this.ddlSchIdenty.Items.Clear();
                return;
            }

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);

            string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.SchIdenty };
            string codeCombineFormat = null;
            string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
            string textCombineFormat = null;

            CodeText[] datas = null;
            XmlResult xmlResult = DataProxy.Current.GetEntityOptions<SchoolRTypeEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = this.GetLocalized("查詢學校資料");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            WebHelper.SetDropDownListItems(this.ddlSchIdenty, DefaultItem.Kind.Select, false, datas, true, false, 0, selectedValue);

            this.ddlSchIdenty_SelectedIndexChanged(this.ddlSchIdenty, EventArgs.Empty);
        }

        /// <summary>
        /// 取得並結繫商家代號選項
        /// </summary>
        private void GetAndBindReceiveTypeOptions(string schIdenty, string[] selectedValues)
        {
            LogonUser logonUser = this.GetLogonUser();

            CodeText[] items = null;
            if (logonUser.IsBankUser && !String.IsNullOrEmpty(schIdenty))
            {
                XmlResult xmlResult = DataProxy.Current.GetReceiveTypeCodeTextsBySchool(this.Page, schIdenty, out items);
                if (xmlResult.IsSuccess)
                {
                    if (items != null)
                    {
                        foreach (CodeText item in items)
                        {
                            //只顯示代碼就好
                            item.Text = item.Code;
                        }
                    }
                }
                else
                {
                    string action = this.GetLocalized("查詢學校的商家代號資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }

            WebHelper.SetCheckBoxListItems(this.cblReceiveType, items, false, 2, selectedValues);
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(SchoolServiceAccountEntity data)
        {
            if (data == null)
            {
                this.tbxSysId.Text = String.Empty;
                this.tbxSysName.Text = String.Empty;

                #region [MDY:20220530] Checkmarx 調整
                this.tbxSysPXX.Text = String.Empty;
                #endregion

                this.tbxClientIp.Text = String.Empty;
                this.tbxSchReceiveUrl.Text = String.Empty;

                this.ddlSchIdenty.SelectedIndex = -1;
                this.cblReceiveType.SelectedIndex = -1;

                this.ccbtnOK.Visible = false;
                return;
            }

            bool isPKeyEditable = ActionMode.IsPKeyEditableMode(this.Action);
            bool isDataEditable = ActionMode.IsDataEditableMode(this.Action);

            this.tbxSysId.Text = data.SysId;
            this.tbxSysId.Enabled = isPKeyEditable;

            this.tbxSysName.Text = data.SysName;
            this.tbxSysName.Enabled = isDataEditable;

            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            #region [MDY:20160925] 系統驗證碼需解密處理
            string decodeSysPXX = DataFormat.GetSysCWordDecode(data.SysPXX);
            if (String.IsNullOrEmpty(decodeSysPXX))
            {
                string msg = this.GetLocalized("系統驗證碼解密處理失敗");
                decodeSysPXX = data.SysPXX;
            }
            #endregion

            this.tbxSysPXX.Text = decodeSysPXX;
            this.tbxSysPXX.Enabled = isDataEditable;
            #endregion
            #endregion

            this.tbxClientIp.Text = data.ClientIp;
            this.tbxClientIp.Enabled = isDataEditable;

            this.tbxSchReceiveUrl.Text = data.SchReceiveUrl;
            this.tbxSchReceiveUrl.Enabled = isDataEditable;

            WebHelper.SetDropDownListSelectedValue(this.ddlSchIdenty, data.SchIdenty);
            this.ddlSchIdenty.Enabled = isPKeyEditable;

            this.ddlSchIdenty_SelectedIndexChanged(this.ddlSchIdenty, EventArgs.Empty);

            WebHelper.SetCheckBoxListSelectedValues(this.cblReceiveType, data.GetMyReceiveTypes());
            this.cblReceiveType.Enabled = isDataEditable;

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 取得並檢查輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private bool GetAndCheckEditData(out SchoolServiceAccountEntity data)
        {
            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            data = new SchoolServiceAccountEntity();

            if (this.Action == ActionMode.Insert)
            {
                data.SysId = this.tbxSysId.Text.Trim();

                data.Status = DataStatusCodeTexts.NORMAL;
                data.CrtUser = this.GetLogonUser().UserId;
                data.CrtDate = DateTime.Now;
            }
            else
            {
                data.SysId = this.EditSysId;
            }

            data.SysName = this.tbxSysName.Text.Trim();
            data.SysPXX = this.tbxSysPXX.Text.Trim();
            data.ClientIp = this.tbxClientIp.Text.Trim().Replace(" ", "");
            data.SchReceiveUrl = this.tbxSchReceiveUrl.Text.Trim();

            data.SchIdenty = this.ddlSchIdenty.SelectedValue;

            List<string> receiveTypes = new List<string>();
            foreach (ListItem item in this.cblReceiveType.Items)
            {
                if (item.Selected)
                {
                    receiveTypes.Add(item.Value);
                }
            }
            data.ReceiveType = receiveTypes.Count == 0 ? String.Empty : String.Join(",", receiveTypes.ToArray());

            if (this.Action != ActionMode.Delete)
            {
                #region 系統代碼
                if (String.IsNullOrEmpty(data.SysId))
                {
                    this.ShowMustInputAlert("系統代碼");
                    return false;
                }
                if (!Common.IsEnglishNumberDash(data.SysId, 6, 32))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「系統代碼」必須是 6 ~ 32 碼的數字、英文、或英數字混合，可含 Dash 符號，但第一個字不可以是 Dash");
                    this.ShowJsAlert(msg);
                    return false;
                }
                #endregion

                #region 系統名稱
                if (String.IsNullOrEmpty(data.SysName))
                {
                    this.ShowMustInputAlert("系統名稱");
                    return false;
                }
                #endregion

                #region 系統驗證碼
                if (String.IsNullOrEmpty(data.SysPXX))
                {
                    this.ShowMustInputAlert("系統驗證碼");
                    return false;
                }
                if (!Common.IsEnglishNumber(data.SysPXX, 8, 32))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「系統驗證碼」必須是 8 ~ 32 碼的數字、英文、或英數字混合");
                    this.ShowJsAlert(msg);
                    return false;
                }

                #region [MDY:20160925] 系統驗證碼需加密處理
                string encodeSysPXX = DataFormat.GetSysCWordEncode(data.SysPXX);
                if (String.IsNullOrEmpty(encodeSysPXX))
                {
                    string msg = this.GetLocalized("系統驗證碼加密處理失敗");
                    this.ShowJsAlert(msg);
                    return false;
                }
                else
                {
                    data.SysPXX = encodeSysPXX;
                }
                #endregion

                #endregion

                #region 申請的學校
                if (String.IsNullOrEmpty(data.SchIdenty))
                {
                    this.ShowMustInputAlert("申請的學校");
                    return false;
                }
                #endregion

                #region 授權的商家代號
                if (String.IsNullOrEmpty(data.ReceiveType))
                {
                    this.ShowMustInputAlert("授權的商家代號");
                    return false;
                }
                #endregion
            }

            return true;
            #endregion
            #endregion
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
                this.EditSysId = QueryString.TryGetValue("SysId", String.Empty);
                string sysId = this.EditSysId;

                if (!ActionMode.IsMaintinaMode(this.Action)
                    || (!ActionMode.IsPKeyEditableMode(this.Action) && String.IsNullOrEmpty(sysId))
                    )
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                SchoolServiceAccountEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new SchoolServiceAccountEntity();
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            Expression where = new Expression(SchoolServiceAccountEntity.Field.SysId, sysId);
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolServiceAccountEntity>(this.Page, where, null, out data);
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
                        }
                        #endregion
                        break;
                }
                #endregion

                this.BindEditData(data);
            }
        }

        protected void ddlSchIdenty_SelectedIndexChanged(object sender, EventArgs e)
        {
            string schIdenty = this.ddlSchIdenty.SelectedValue;
            this.GetAndBindReceiveTypeOptions(schIdenty, null);
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            SchoolServiceAccountEntity data = null;
            if (!this.GetAndCheckEditData(out data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600014.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<SchoolServiceAccountEntity>(this, data, out count);
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
                        Expression where = new Expression(SchoolServiceAccountEntity.Field.SysId, data.SysId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(SchoolServiceAccountEntity.Field.SysName, data.SysName);

                        #region [MDY:20220530] Checkmarx 調整
                        #region [MDY:20210401] 原碼修正
                        fieldValues.Add(SchoolServiceAccountEntity.Field.SysPXX, data.SysPXX);
                        #endregion
                        #endregion

                        fieldValues.Add(SchoolServiceAccountEntity.Field.ClientIp, data.ClientIp);
                        fieldValues.Add(SchoolServiceAccountEntity.Field.ReceiveType, data.ReceiveType);
                        //fieldValues.Add(SchoolServiceAccountEntity.Field.SchIdenty, data.SchIdenty);
                        fieldValues.Add(SchoolServiceAccountEntity.Field.SchReceiveUrl, data.SchReceiveUrl);

                        fieldValues.Add(SchoolServiceAccountEntity.Field.MdyDate, DateTime.Now);
                        fieldValues.Add(SchoolServiceAccountEntity.Field.MdyUser, this.GetLogonUser().UserId);
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<SchoolServiceAccountEntity>(this, where, fieldValues, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<SchoolServiceAccountEntity>(this, data, out count);
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