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
    /// 檔案下載 (維護)
    /// </summary>
    public partial class S5600012M : BasePage
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
        /// 編輯的序號
        /// </summary>
        private int EditSn
        {
            get
            {
                object value = ViewState["EditSn"];
                return value is int ? (int)value : 0;
            }
            set
            {
                ViewState["EditSn"] = value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// 編輯的型態
        /// </summary>
        private string EditFileQual
        {
            get
            {
                return ViewState["EditFileQual"] as string;
            }
            set
            {
                ViewState["EditFileQual"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxExplain.Text = String.Empty;
            this.tbxUrl.Text = String.Empty;
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(FilePoolView data)
        {
            if (data == null)
            {
                this.tbxExplain.Text = String.Empty;
                this.tbxUrl.Text = String.Empty;
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxExplain.Enabled = true;
                    this.tbxUrl.Enabled = true;
                    this.FileUpload1.Enabled = true;
                    this.ddlFileQual.Enabled = true;
                    this.labOldFileName.Visible = false;
                    break;
                case ActionMode.Modify:
                    this.tbxExplain.Enabled = true;
                    this.tbxUrl.Enabled = true;
                    this.FileUpload1.Enabled = true;
                    this.ddlFileQual.Enabled = true;
                    this.labOldFileName.Visible = true;
                    break;
                default:
                    this.tbxExplain.Enabled = false;
                    this.tbxUrl.Enabled = false;
                    this.FileUpload1.Enabled = false;
                    this.ddlFileQual.Enabled = false;
                    this.labOldFileName.Visible = true;
                    break;
            }
            this.tbxExplain.Text = data.Explain;
            this.tbxUrl.Text = data.Url;
            this.labOldFileName.Text = String.Format("已上傳的檔案名稱：{0}<br/>", data.FileName);
            WebHelper.SetDropDownListSelectedValue(this.ddlFileQual, data.FileQual);
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData()
        {
            if (this.Action == ActionMode.Delete)
            {
                return true;
            }

            string explain = this.tbxExplain.Text.Trim();
            if (String.IsNullOrWhiteSpace(explain))
            {
                this.ShowMustInputAlert("說明");
                return false;
            }

            string fileQual = this.ddlFileQual.SelectedValue;
            switch (fileQual)
            {
                case "1":   //連結
                    #region 連結
                    if (String.IsNullOrWhiteSpace(this.tbxUrl.Text))
                    {
                        //[TODO] 固定顯示訊息的收集
                        this.ShowMustInputAlert("連結網址");
                        return false;
                    }
                    #endregion
                    break;
                case "2":   //檔案
                    #region 檔案
                    {
                        if (this.Action == ActionMode.Insert || this.EditFileQual != fileQual)
                        {
                            if (!this.FileUpload1.HasFile || String.IsNullOrEmpty(this.FileUpload1.FileName))
                            {
                                //[TODO] 固定顯示訊息的收集
                                this.ShowMustInputAlert("上傳檔案");
                                return false;
                            }
                        }
                        if (this.FileUpload1.HasFile && (this.FileUpload1.FileBytes == null || this.FileUpload1.FileBytes.Length == 0))
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("上傳檔案無內容");
                            this.ShowJsAlert(msg);
                            return false;
                        }
                        if (this.FileUpload1.HasFile && this.FileUpload1.FileName.Length > 100)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("上傳檔案名稱限制最多100個中文字");
                            this.ShowJsAlert(msg);
                            return false;
                        }
                    }
                    #endregion
                    break;
                default:
                    {
                        //[TODO] 固定顯示訊息的收集
                        this.ShowMustInputAlert("型態");
                        return false;
                    }
            }

            return true;
        }

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private FilePoolEntity GetEditData()
        {
            FilePoolEntity data = new FilePoolEntity();
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    data.Status = DataStatusCodeTexts.NORMAL;
                    data.CrtDate = DateTime.Now;
                    data.CrtUser = this.GetLogonUser().UserId;
                    break;
                case ActionMode.Modify:     //修改
                    data.Sn = this.EditSn;
                    data.MdyDate = DateTime.Now;
                    data.MdyUser = this.GetLogonUser().UserId;
                    break;
                case ActionMode.Delete:     //刪除
                    data.Sn = this.EditSn;
                    return data;
            }

            data.Explain = this.tbxExplain.Text.Trim();
            data.FileQual = this.ddlFileQual.SelectedValue;
            switch (data.FileQual)
            {
                case "1":   //連結
                    data.Url = this.tbxUrl.Text.Trim();
                    data.FileName = String.Empty;
                    data.ExtName = String.Empty;
                    data.File = null;
                    break;
                case "2":   //檔案
                    data.Url = String.Empty;
                    if (this.FileUpload1.HasFile)
                    {
                        data.FileName = this.FileUpload1.FileName;
                        data.ExtName = System.IO.Path.GetExtension(this.FileUpload1.FileName);
                        data.File = this.FileUpload1.FileBytes;
                    }
                    else
                    {
                        data.FileName = String.Empty;
                        data.ExtName = String.Empty;
                        data.File = null;
                    }
                    break;
            }

            return data;
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
                string sn = QueryString.TryGetValue("Sn", String.Empty);
                int editSN = 0;
                if (!String.IsNullOrWhiteSpace(sn) && int.TryParse(sn.Trim(), out editSN))
                {
                    this.EditSn = editSN;
                }

                if (((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) && this.EditSn <= 0))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                FilePoolView data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new FilePoolView();
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(FilePoolView.Field.Sn, this.EditSn);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<FilePoolView>(this, where, null, out data);
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

                            this.EditFileQual = data.FileQual;
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
            if (!this.CheckEditData())
            {
                return;
            }

            FilePoolEntity data = this.GetEditData();

            #region [MDY:20201227] 檢查副檔名限制 .PDF、.DOC、.DOCX、.XLS、.XLSX、.PPT、.PPTX
            if (!String.IsNullOrEmpty(data.FileName))
            {
                string extName = String.IsNullOrEmpty(data.ExtName) ? String.Empty : data.ExtName.ToLower();
                string[] allowExtNames = new string[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" };
                if (!(new List<string>(allowExtNames)).Contains(extName))
                {
                    string msg = this.GetLocalized("僅支援副檔名 " + String.Join(" | ", allowExtNames));
                    this.ShowJsAlert(msg);
                    return;
                }
            }
            #endregion

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600012.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<FilePoolEntity>(this.Page, data, out count);
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
                        int count = 0;
                        Expression where = new Expression(FilePoolEntity.Field.Sn, this.EditSn);
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(FilePoolEntity.Field.Explain, data.Explain);
                        fieldValues.Add(FilePoolEntity.Field.FileQual, data.FileQual);
                        fieldValues.Add(FilePoolEntity.Field.MdyUser, data.MdyUser);
                        fieldValues.Add(FilePoolEntity.Field.MdyDate, data.MdyDate);

                        fieldValues.Add(FilePoolEntity.Field.Url, data.Url);
                        if (data.FileQual == "1")
                        {
                            //連結 要清除 File，
                            fieldValues.Add(FilePoolEntity.Field.FileName, data.FileName);
                            fieldValues.Add(FilePoolEntity.Field.ExtName, data.ExtName);
                            fieldValues.Add(FilePoolEntity.Field.File, data.File);
                        }
                        else if (data.FileQual == "2" && data.File != null)
                        {
                            //檔案且有上傳 要更新 File，
                            fieldValues.Add(FilePoolEntity.Field.FileName, data.FileName);
                            fieldValues.Add(FilePoolEntity.Field.ExtName, data.ExtName);
                            fieldValues.Add(FilePoolEntity.Field.File, data.File);
                        }

                        XmlResult xmlResult = DataProxy.Current.UpdateFields<FilePoolEntity>(this.Page, where, fieldValues, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<FilePoolEntity>(this.Page, data, out count);
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