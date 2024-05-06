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
    /// 代收管道管理(維護)
    /// </summary>
    public partial class S5600004M : BasePage
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
        /// 編輯的管道代碼
        /// </summary>
        private string EditChannelId
        {
            get
            {
                return ViewState["EditChannelId"] as string;
            }
            set
            {
                ViewState["EditChannelId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

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
                this.EditChannelId = QueryString.TryGetValue("ChannelId", String.Empty);

                if (((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) 
                    && String.IsNullOrEmpty(this.EditChannelId)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                ChannelSetEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new ChannelSetEntity();
                            data.ChannelId = this.EditChannelId;
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(ChannelSetEntity.Field.ChannelId, this.EditChannelId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<ChannelSetEntity>(this, where, null, out data);
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

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            GetAndBindCategoryIdOptions();

            this.tbxChannelName.Text = String.Empty;
            this.tbxChannelId.Text = String.Empty;
            this.tbxMaxMoney.Text = String.Empty;
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 取得並結繫此管道的彙總管道選項
        /// </summary>
        private void GetAndBindCategoryIdOptions()
        {
            CodeText[] items = null;
            Expression where = new Expression();
            where.And(ChannelSetEntity.Field.CategoryId, string.Empty);
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(ChannelSetEntity.Field.ChannelId, OrderByEnum.Asc);

            string[] codeFieldNames = new string[] { ChannelSetEntity.Field.ChannelId };
            string codeCombineFormat = null;
            string[] textFieldNames = new string[] { ChannelSetEntity.Field.ChannelName };
            string textCombineFormat = null;

            XmlResult xmlResult = DataProxy.Current.GetEntityOptions<ChannelSetEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
            if (!xmlResult.IsSuccess)
            {
                string action = this.GetLocalized("查詢管道代碼資料");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            if (xmlResult.IsSuccess)
            {
                if (items != null)
                {
                    for (int idx = 0; idx < items.Length; idx++)
                    {
                        items[idx].Text = string.Format("{0}({1})", items[idx].Text, items[idx].Code);
                    }
                }
            }
            WebHelper.SetDropDownListItems(this.ddlCategoryId, DefaultItem.Kind.Select, false, items, false, false, 0, null);
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(ChannelSetEntity data)
        {
            if (data == null)
            {
                this.tbxChannelName.Text = String.Empty;
                this.tbxChannelId.Text = String.Empty;
                this.tbxMaxMoney.Text = String.Empty;
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxChannelId.Enabled = true;
                    this.tbxChannelName.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.tbxChannelId.Enabled = false;
                    this.tbxChannelName.Enabled = true;
                    break;
                default:
                    this.tbxChannelId.Enabled = false;
                    this.tbxChannelName.Enabled = false;
                    this.tbxMaxMoney.Enabled = false;
                    ddlCategoryId.Enabled = false;
                    rdoDefaultFlag.Enabled = false;
                    rdoProcessFee.Enabled = false;
                    break;
            }

            #region [MDY:20210401] 原碼修正
            this.tbxChannelId.Text = HttpUtility.HtmlEncode(data.ChannelId);
            #endregion

            this.tbxChannelName.Text = data.ChannelName;
            chkIsUsing.Checked = data.IsUsing == null ? true : (bool)data.IsUsing;
            this.tbxMaxMoney.Text = DataFormat.GetAmountText(data.MaxMoney);
            tbxMaxMoney.Text = DataFormat.GetAmountText(data.MaxMoney);
            WebHelper.SetRadioButtonListSelectedValue(this.rdoDefaultFlag, data.DefaultFlag);
            WebHelper.SetRadioButtonListSelectedValue(this.rdoProcessFee, data.ProcessFee);
            WebHelper.SetDropDownListSelectedValue(this.ddlCategoryId, data.CategoryId);
            this.ccbtnOK.Visible = true;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!this.CheckEditData())
            {
                return;
            }

            ChannelSetEntity data = this.GetEditData();

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600004.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<ChannelSetEntity>(this, data, out count);
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
                        #region old
                        //#region 更新條件
                        //Expression where = new Expression(ChannelSetEntity.Field.ChannelId, data.ChannelId);
                        //#endregion

                        //#region 更新欄位
                        //KeyValueList fieldValues = new KeyValueList();
                        //fieldValues.Add(ChannelSetEntity.Field.ChannelName, data.ChannelName);
                        //fieldValues.Add(ChannelSetEntity.Field.CategoryId, data.CategoryId);
                        //fieldValues.Add(ChannelSetEntity.Field.ProcessFee, data.ProcessFee);
                        //fieldValues.Add(ChannelSetEntity.Field.IsUsing, data.IsUsing);
                        //fieldValues.Add(ChannelSetEntity.Field.DefaultFlag, data.DefaultFlag);
                        //fieldValues.Add(ChannelSetEntity.Field.MaxMoney, data.MaxMoney);
                        //#endregion
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Update<ChannelSetEntity>(this, data, out count);
                        //XmlResult xmlResult = DataProxy.Current.UpdateFields<ChannelSetEntity>(this, where, fieldValues, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<ChannelSetEntity>(this, data, out count);
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

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private ChannelSetEntity GetEditData()
        {
            ChannelSetEntity data = new ChannelSetEntity();

            data.ChannelId = this.tbxChannelId.Text.Trim();
            data.ChannelName = this.tbxChannelName.Text.Trim();
            data.CategoryId = ddlCategoryId.SelectedValue;
            data.ProcessFee = rdoProcessFee.SelectedValue;
            data.IsUsing = chkIsUsing.Checked;
            data.DefaultFlag = rdoDefaultFlag.SelectedValue;
            decimal money = 0;
            decimal.TryParse(tbxMaxMoney.Text, out money);
            data.MaxMoney = money;
            return data;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData()
        {
            if (String.IsNullOrEmpty(tbxChannelId.Text.Trim()))
            {
                this.ShowMustInputAlert("管道代碼");
                return false;
            }

            //if (String.IsNullOrEmpty(ddlCategoryId.SelectedValue))
            //{
            //    this.ShowMustInputAlert("此管道的彙總管道");
            //    return false;
            //}

            if (!Common.IsMoney(tbxMaxMoney.Text))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「管道收款上限金額」不是合法的金額");
                this.ShowSystemMessage(msg);
                return false;
            }

            return true;
        }

    }
}