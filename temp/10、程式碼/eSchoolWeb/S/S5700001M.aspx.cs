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
    /// (專屬)繳費單模板管理 (維護)
    /// </summary>
    public partial class S5700001M : BasePage
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
        /// 編輯的模板代碼
        /// </summary>
        private int EditBillFormId
        {
            get
            {
                object value = ViewState["EditBillFormId"];
                if (value is int)
                {
                    return (int)value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["EditBillFormId"] = value < 0 ? 0 : value;
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.rdoBillFormType.SelectedIndex = 0;
            this.tbxBillFormId.Text = String.Empty;
            this.tbxBillFormName.Text = String.Empty;

            #region 商家代碼選項
            CodeText[] items = null;
            XmlResult xmlResult = DataProxy.Current.GetMyReceiveTypeCodeTexts(this.Page, out items, ReceiveKindCodeTexts.SCHOOL);
            if (!xmlResult.IsSuccess)
            {
                this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
            }
            WebHelper.SetDropDownListItems(this.ddlReceiveType, DefaultItem.Kind.Select, false, items, true, false, 0, null);
            #endregion

            this.trFileUpload.Visible = true;
            this.ccbtnOK.Visible = xmlResult.IsSuccess;
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(BillFormEntity data)
        {
            if (data == null)
            {
                this.rdoBillFormType.SelectedIndex = -1;
                this.tbxBillFormId.Text = String.Empty;
                this.tbxBillFormName.Text = String.Empty;
                this.trFileUpload.Visible = false;
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.rdoBillFormType.Enabled = true;
                    this.tbxBillFormId.Enabled = true;
                    this.tbxBillFormName.Enabled = true;
                    this.ddlReceiveType.Enabled = true;
                    this.trFileUpload.Visible = true;
                    this.spnPrefix.Visible = true;
                    break;
                case ActionMode.Modify:
                    this.rdoBillFormType.Enabled = false;
                    this.tbxBillFormId.Enabled = false;
                    this.tbxBillFormName.Enabled = true;
                    this.ddlReceiveType.Enabled = true;
                    this.trFileUpload.Visible = true;
                    this.spnPrefix.Visible = false;
                    break;
                default:
                    this.rdoBillFormType.Enabled = false;
                    this.tbxBillFormId.Enabled = false;
                    this.tbxBillFormName.Enabled = false;
                    this.ddlReceiveType.Enabled = false;
                    this.trFileUpload.Visible = false;
                    this.spnPrefix.Visible = false;
                    break;
            }

            WebHelper.SetRadioButtonListSelectedValue(this.rdoBillFormType, data.BillFormType);

            #region 如果是專屬模板，把 billFormId 拆成 ReceiveType + 自訂編號，這是為了避免編號容易重複的情形
            //if (data.BillFormEdition == BillFormEditionCodeTexts.PRIVATE)
            //{
            //    string billFormId = data.BillFormId.ToString();
            //    if (billFormId.Length > 4 && billFormId.StartsWith(data.ReceiveType.Trim()))
            //    {
            //        this.tbxBillFormId.Text = billFormId.Substring(4);
            //    }
            //    else
            //    {
            //        this.tbxBillFormId.Text = billFormId;
            //    }
            //}
            //else
            //{
            //    this.tbxBillFormId.Text = data.BillFormId.ToString();
            //}
            this.tbxBillFormId.Text = data.BillFormId.ToString();
            #endregion

            this.tbxBillFormName.Text = data.BillFormName;

            WebHelper.SetDropDownListSelectedValue(this.ddlReceiveType, data.ReceiveType);

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 取得並檢查輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private bool GetAndCheckEditData(out BillFormEntity data)
        {
            data = new BillFormEntity();

            #region 專屬模板
            data.BillFormEdition = BillFormEditionCodeTexts.PRIVATE;
            #endregion

            #region 商家代號
            data.ReceiveType = this.ddlReceiveType.SelectedValue;
            if (String.IsNullOrEmpty(data.ReceiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return false;
            }
            #endregion

            #region 模板種類
            data.BillFormType = this.rdoBillFormType.SelectedValue;
            if (String.IsNullOrEmpty(data.BillFormType))
            {
                this.ShowMustInputAlert("模板種類");
                return false;
            }
            #endregion

            #region 模板代號
            switch (this.Action)
            {
                case ActionMode.Insert:
                    string billFormId = this.tbxBillFormId.Text.Trim();
                    if (String.IsNullOrEmpty(billFormId))
                    {
                        this.ShowMustInputAlert("模板代號");
                        return false;
                    }
                    int value = 0;
                    if (!Int32.TryParse(billFormId, out value) || value < 1 || value > 9999)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("模板代號限輸入 1 ~ 9999 的數字");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                    data.BillFormId = int.Parse(data.ReceiveType + value.ToString());
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    data.BillFormId = this.EditBillFormId;
                    break;
            }
            #endregion

            #region 模板名稱
            data.BillFormName = this.tbxBillFormName.Text.Trim();
            if (String.IsNullOrEmpty(data.BillFormName))
            {
                this.ShowMustInputAlert("模板名稱");
                return false;
            }
            #endregion

            #region 上傳檔案
            string fileName = this.fileUpload.FileName;
            if (!String.IsNullOrWhiteSpace(fileName) && !System.IO.Path.GetExtension(fileName).Equals(".PDF", StringComparison.CurrentCultureIgnoreCase))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("上傳檔案限 PDF 副檔名");
                this.ShowJsAlert(msg);
                return false;
            }
            switch (this.Action)
            {
                case ActionMode.Insert:
                    if (!this.fileUpload.HasFile)
                    {
                        this.ShowMustInputAlert("上傳檔案");
                        return false;
                    }
                    data.BillFormImage = this.fileUpload.FileBytes;
                    if (data.BillFormImage == null || data.BillFormImage.Length == 0)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("上傳檔案無內容");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                    break;
                case ActionMode.Modify:
                    if (this.fileUpload.HasFile)
                    {
                        data.BillFormImage = this.fileUpload.FileBytes;
                        if (data.BillFormImage == null || data.BillFormImage.Length == 0)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("上傳檔案無內容");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                    }
                    else
                    {
                        data.BillFormImage = null;
                    }
                    break;
            }
            #endregion

            if (this.Action == ActionMode.Insert)
            {
                #region 補齊資料
                data.BizType = string.Empty;
                data.BillFormUser = BillFormUserCodeTexts.SCHOOL;

                data.Status = DataStatusCodeTexts.NORMAL;
                data.CrtUser = this.GetLogonUser().UserId;
                data.CrtDate = DateTime.Now;
                #endregion
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
                string billFormId = QueryString.TryGetValue("BillFormId", String.Empty);

                int editBillFormId = 0;
                if (!ActionMode.IsMaintinaMode(this.Action)
                    || (!ActionMode.IsPKeyEditableMode(this.Action) && !int.TryParse(billFormId, out editBillFormId))
                    )
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                this.EditBillFormId = editBillFormId;
                #endregion

                #region 取得維護資料
                BillFormEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new BillFormEntity();
                            data.BillFormId = 0;
                            data.BillFormEdition = BillFormEditionCodeTexts.PRIVATE;
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(BillFormEntity.Field.BillFormId, this.EditBillFormId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<BillFormEntity>(this, where, null, out data);
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
            BillFormEntity data = null;
            if (!this.GetAndCheckEditData(out data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5700001.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<BillFormEntity>(this, data, out count);
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
                        Expression where = new Expression(BillFormEntity.Field.BillFormId, data.BillFormId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        //fieldValues.Add(BillFormEntity.Field.BillFormEdition, data.BillFormEdition);
                        //fieldValues.Add(BillFormEntity.Field.BillFormType, data.BillFormType);
                        if (data.BillFormEdition == BillFormEditionCodeTexts.PRIVATE)
                        {
                            fieldValues.Add(BillFormEntity.Field.ReceiveType, data.ReceiveType);
                        }
                        fieldValues.Add(BillFormEntity.Field.BillFormName, data.BillFormName);
                        if (data.BillFormImage != null && data.BillFormImage.Length > 0)
                        {
                            fieldValues.Add(BillFormEntity.Field.BillFormImage, data.BillFormImage);
                        }
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<BillFormEntity>(this, where, fieldValues, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<BillFormEntity>(this, data, out count);
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