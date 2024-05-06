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
    /// 學年代碼 (維護)
    /// </summary>
    public partial class S5600001M : BasePage
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
        /// 編輯的學年參數
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
                this.EditYearId = QueryString.TryGetValue("YearId", String.Empty);

                if (((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) && String.IsNullOrEmpty(this.EditYearId)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                YearListEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new YearListEntity();
                            data.YearId = this.EditYearId;
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(YearListEntity.Field.YearId, this.EditYearId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<YearListEntity>(this, where, null, out data);
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
            this.tbxYearId.Text = String.Empty;
            this.tbxYearName.Text = String.Empty;

            #region [MDY:202203XX] 2022擴充案 學年英文名稱、資料啟用 欄位
            this.tbxYearEName.Text = String.Empty;

            CodeText[] items = new CodeText[2]
            {
                new CodeText("Y", "啟用"),
                new CodeText("N", "停用"),
            };
            WebHelper.SetDropDownListItems(this.ddlEnabled, items, DefaultItem.Kind.None, showValue: false, needLocalized: true, selectedValue: items[0].Code);
            #endregion

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(YearListEntity data)
        {
            if (data == null)
            {
                this.tbxYearId.Text = String.Empty;
                this.tbxYearName.Text = String.Empty;

                #region [MDY:202203XX] 2022擴充案 學年英文名稱、資料啟用 欄位
                {
                    this.tbxYearEName.Text = String.Empty;
                    ListItem item = this.ddlEnabled.Items.FindByValue("Y");
                    if (item != null)
                    {
                        this.ddlEnabled.SelectedIndex = -1;
                        item.Selected = true;
                    }
                }
                #endregion

                this.ccbtnOK.Visible = false;
                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxYearId.Enabled = true;
                    this.tbxYearName.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.tbxYearId.Enabled = false;
                    this.tbxYearName.Enabled = true;
                    break;
                default:
                    this.tbxYearId.Enabled = false;
                    this.tbxYearName.Enabled = false;
                    break;
            }

            #region [MDY:20210401] 原碼修正
            this.tbxYearId.Text = HttpUtility.HtmlEncode(data.YearId);
            #endregion

            this.tbxYearName.Text = data.YearName;

            #region [MDY:202203XX] 2022擴充案 學年英文名稱、資料啟用 欄位
            {
                this.tbxYearEName.Text = data.YearEName;
                this.tbxYearEName.Enabled = this.tbxYearName.Enabled;

                ListItem item = this.ddlEnabled.Items.FindByValue(data.Enabled);
                if (item != null)
                {
                    this.ddlEnabled.SelectedIndex = -1;
                    item.Selected = true;
                }
                this.ddlEnabled.Enabled = this.tbxYearName.Enabled;
            }
            #endregion

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData(YearListEntity data)
        {
            if (String.IsNullOrEmpty(data.YearId))
            {
                this.ShowMustInputAlert("學年代碼");
                return false;
            }
            if (!Common.IsNumber(data.YearId))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("學年代碼限輸入數字");
                this.ShowJsAlert(msg);
                return false;
            }
            if (String.IsNullOrEmpty(data.YearName))
            {
                this.ShowMustInputAlert("學年名稱");
                return false;
            }

            #region [MDY:202203XX] 2022擴充案 學年英文名稱、資料啟用 欄位
            if (String.IsNullOrEmpty(data.YearEName))
            {
                this.ShowMustInputAlert("學年英文名稱");
                return false;
            }
            if (String.IsNullOrEmpty(data.Enabled))
            {
                this.ShowMustInputAlert("資料啟用");
                return false;
            }
            #endregion

            return true;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            YearListEntity data = this.GetEditData();
            if (!this.CheckEditData(data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600001.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<YearListEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                            }
                            else
                            {
                                //新增此學年下的 所有代收類別的第一學期 第二學期                                
                                object returnData = null;
                                KeyValue<string>[] arguments = new KeyValue<string>[] {
                                        new KeyValue<string>("Type", "1"),
                                        new KeyValue<string>("insertKey", data.YearId)
                                    };
                                xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.InsertTermListDatas, arguments, out returnData);
                                if (xmlResult.IsSuccess)
                                {
                                    this.ShowActionSuccessAlert(action, backUrl);
                                }
                                else
                                {
                                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                }

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
                        Expression where = new Expression(YearListEntity.Field.YearId, data.YearId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(YearListEntity.Field.YearName, data.YearName);

                        #region [MDY:202203XX] 2022擴充案 學年英文名稱、資料啟用 欄位
                        fieldValues.Add(YearListEntity.Field.YearEName, data.YearEName);
                        fieldValues.Add(YearListEntity.Field.Enabled, data.Enabled);
                        #endregion
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<YearListEntity>(this, where, fieldValues, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<YearListEntity>(this, data, out count);
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
        private YearListEntity GetEditData()
        {
            YearListEntity data = new YearListEntity();
            data.YearId = this.EditYearId;

            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    data.YearId = this.tbxYearId.Text.Trim();
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    data.YearId = this.EditYearId;
                    break;
            }
            data.YearName = this.tbxYearName.Text.Trim();

            #region [MDY:202203XX] 2022擴充案 學年英文名稱、資料啟用 欄位
            data.YearEName = this.tbxYearEName.Text.Trim();
            data.Enabled = this.ddlEnabled.SelectedValue;
            #endregion

            return data;
        }
    }
}