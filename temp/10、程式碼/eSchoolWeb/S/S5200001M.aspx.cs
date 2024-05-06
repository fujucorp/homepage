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
    /// 功能管理 (維護)
    /// </summary>
    public partial class S5200001M : BasePage
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
        /// 編輯的功能代碼
        /// </summary>
        private string EditFuncId
        {
            get
            {
                return ViewState["EditFuncId"] as string;
            }
            set
            {
                ViewState["EditFuncId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxFuncId.Text = String.Empty;
            this.tbxFuncName.Text = String.Empty;
            this.tbxFuncUrl.Text = String.Empty;
            this.tbxSortNo.Text = String.Empty;

            this.GetAndBindParentOption(null, null);

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(FuncMenuEntity data)
        {
            if (data == null)
            {
                this.tbxFuncId.Text = String.Empty;
                this.tbxFuncName.Text = String.Empty;
                this.tbxFuncUrl.Text = String.Empty;
                this.tbxSortNo.Text = String.Empty;

                this.GetAndBindParentOption(null, null);

                this.ccbtnOK.Visible = false;
                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxFuncId.Enabled = true;
                    this.ddlParentId.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.tbxFuncId.Enabled = false;
                    this.ddlParentId.Enabled = false;
                    break;
            }

            this.tbxFuncId.Text = data.FuncId;
            this.tbxFuncName.Text = data.FuncName;
            this.tbxFuncUrl.Text = data.Url;
            this.tbxSortNo.Text = data.SortNo.ToString();

            this.GetAndBindParentOption(data.FuncId, data.ParentId);

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private FuncMenuEntity GetEditData()
        {
            FuncMenuEntity data = new FuncMenuEntity();

            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    data.FuncId = this.tbxFuncId.Text.Trim();
                    break;
                case ActionMode.Modify:     //修改
                    data.FuncId = this.EditFuncId;
                    break;
            }
            data.FuncName = this.tbxFuncName.Text.Trim();
            data.ParentId = ddlParentId.SelectedValue;
            data.Url = this.tbxFuncUrl.Text.Trim();
            int sortNo = 0;
            if (Int32.TryParse(this.tbxSortNo.Text.Trim(), out sortNo))
            {
                data.SortNo = sortNo;
            }
            else
            {
                data.SortNo = 0;
            }
            return data;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool CheckEditData(FuncMenuEntity data)
        {
            if (String.IsNullOrEmpty(data.FuncId))
            {
                this.ShowMustInputAlert("功能代碼");
                return false;
            }
            if ((data.FuncId.Length != 2 && data.FuncId.Length != 3 && data.FuncId.Length != 8)
                || data.FuncId.EndsWith("00000"))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「功能代碼」限輸入兩碼、三碼或八碼的英文、數字或英數字混合，且不可以 00000 結尾");
                this.ShowJsAlert(msg);
                return false;
            }

            if (String.IsNullOrEmpty(data.FuncName))
            {
                this.ShowMustInputAlert("功能名稱");
                return false;
            }

            if (data.SortNo == 0)
            {
                if (String.IsNullOrWhiteSpace(this.tbxSortNo.Text))
                {
                    this.ShowMustInputAlert("顯示排序編號");
                    return false;
                }
                else
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「顯示排序編號」限輸入1 ~ 99999的整數數字");
                    this.ShowJsAlert(msg);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得並結繫指定功能代碼的父層功能選項
        /// </summary>
        /// <param name="funcID">指定功能代碼</param>
        /// <param name="selectedValue">指定已選取得項目值</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetAndBindParentOption(string funcID, string selectedValue)
        {
            bool IsOK = true;
            this.ddlParentId.Items.Clear();

            #region 取得父層功能
            CodeTextList items = new CodeTextList();
            items.Add(String.Empty, "--- 無父層 ---");
            if (!String.IsNullOrEmpty(funcID))
            {
                Expression where = null;
                switch (funcID.Length)
                {
                    case 3: //第二層
                        where = new Expression(FuncMenuEntity.Field.FuncId, funcID.Substring(0, 2));
                        break;
                    case 8: //第二層 或 第三層
                        where = new Expression(FuncMenuEntity.Field.FuncId, funcID.Substring(0, 2))
                            .Or(FuncMenuEntity.Field.FuncId, funcID.Substring(0, 3));
                        break;
                }

                if (where != null)
                {
                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                    orderbys.Add(FuncMenuEntity.Field.FuncId, OrderByEnum.Asc);

                    CodeText[] datas = null;
                    string[] codeFieldNames = new string[] { FuncMenuEntity.Field.FuncId };
                    string codeCombineFormat = null;
                    string[] textFieldNames = new string[] { FuncMenuEntity.Field.FuncName };
                    string textCombineFormat = null;

                    XmlResult xmlResult = DataProxy.Current.GetEntityOptions<FuncMenuEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                    if (xmlResult.IsSuccess)
                    {
                        if (datas != null && datas.Length > 0)
                        {
                            foreach (CodeText data in datas)
                            {
                                data.Text = WebHelper.GetMenuLocalized(data.Code, data.Text);
                            }
                            items.AddRange(datas);
                        }
                    }
                    else
                    {
                        IsOK = false;
                        string action = this.GetLocalized("查詢父層資料");
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    }
                }
            }
            #endregion

            WebHelper.SetDropDownListItems(this.ddlParentId, DefaultItem.Kind.None, false, items, true, false, 0, selectedValue);

            return IsOK;
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
                this.EditFuncId = QueryString.TryGetValue("FuncId", String.Empty);

                if (!ActionMode.IsMaintinaMode(this.Action)
                    || this.Action == ActionMode.Delete
                    || (this.Action == ActionMode.Modify && String.IsNullOrEmpty(this.EditFuncId)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                FuncMenuEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new FuncMenuEntity();
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                        #region 修改
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(FuncMenuEntity.Field.FuncId, this.EditFuncId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<FuncMenuEntity>(this, where, null, out data);
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

        protected void tbxFuncId_TextChanged(object sender, EventArgs e)
        {
            string funcId = tbxFuncId.Text.Trim();
            if ((funcId.Length != 2 && funcId.Length != 3 && funcId.Length != 8)
                 || funcId.EndsWith("00000"))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「功能代碼」限輸入兩碼、三碼或八碼的英文、數字或英數字混合，且不可以 00000 結尾");
                this.ShowJsAlert(msg);
            }

            this.GetAndBindParentOption(funcId, null);
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            FuncMenuEntity data = this.GetEditData();
            if (!this.CheckEditData(data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5200001.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        #region 補齊資料
                        data.Status = DataStatusCodeTexts.NORMAL;
                        data.CrtUser = this.GetLogonUser().UserId;
                        data.CrtDate = DateTime.Now;
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<FuncMenuEntity>(this, data, out count);
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
                        Expression where = new Expression(FuncMenuEntity.Field.FuncId, data.FuncId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(FuncMenuEntity.Field.FuncName, data.FuncName);
                        fieldValues.Add(FuncMenuEntity.Field.ParentId, data.ParentId);
                        fieldValues.Add(FuncMenuEntity.Field.Url, data.Url);
                        fieldValues.Add(FuncMenuEntity.Field.SortNo, data.SortNo);
                        fieldValues.Add(FuncMenuEntity.Field.MdyUser, this.GetLogonUser().UserId);
                        fieldValues.Add(FuncMenuEntity.Field.MdyDate, DateTime.Now);
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<FuncMenuEntity>(this, where, fieldValues, out count);
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