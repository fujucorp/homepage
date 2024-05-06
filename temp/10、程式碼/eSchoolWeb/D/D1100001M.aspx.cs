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

namespace eSchoolWeb.D
{
    /// <summary>
    /// 學期代碼 (維護)
    /// </summary>
    public partial class D1100001M : BasePage
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
        /// 編輯的業務別碼參數
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

        /// <summary>
        /// 編輯的學期參數
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
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxTermId.Text = String.Empty;
            this.tbxTermName.Text = String.Empty;
            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 學期英文名稱
            this.phdTermEName.Visible = false;
            this.tbxTermEName.Text = String.Empty;
            #endregion
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(TermListEntity data)
        {
            if (data == null)
            {
                this.tbxTermId.Text = String.Empty;
                this.tbxTermName.Text = String.Empty;
                this.ccbtnOK.Visible = false;

                #region [MDY:202203XX] 2022擴充案 學期英文名稱
                this.phdTermEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
                this.tbxTermEName.Text = String.Empty;
                #endregion

                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxTermId.Enabled = true;
                    this.tbxTermName.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.tbxTermId.Enabled = false;
                    this.tbxTermName.Enabled = true;
                    break;
                default:
                    this.tbxTermId.Enabled = false;
                    this.tbxTermName.Enabled = false;
                    break;
            }
            this.tbxTermId.Text = data.TermId;
            this.tbxTermName.Text = data.TermName;
            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 學期英文名稱
            this.phdTermEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
            this.tbxTermEName.Enabled = this.tbxTermName.Enabled;
            this.tbxTermEName.Text = data.TermEName;
            #endregion
        }

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private TermListEntity GetEditData()
        {
            TermListEntity data = new TermListEntity();
            data.ReceiveType = this.EditReceiveType;
            data.YearId = this.EditYearId;

            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    data.TermId = this.tbxTermId.Text.Trim();
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    data.TermId = this.EditTermId;
                    break;
            }
            data.TermName = this.tbxTermName.Text.Trim();

            #region [MDY:202203XX] 2022擴充案 學期英文名稱
            if (this.phdTermEName.Visible)
            {
                data.TermEName = this.tbxTermEName.Text.Trim();
            }
            else
            {
                data.TermEName = String.Empty;
            }
            #endregion

            return data;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData(TermListEntity data)
        {
            if (String.IsNullOrEmpty(data.TermId))
            {
                this.ShowMustInputAlert("學期代碼");
                return false;
            }
            if (!Common.IsNumber(data.TermId, 1))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("學期代碼限輸入1碼數字");
                this.ShowJsAlert(msg);
                return false;
            }

            #region [MDY:202203XX] 2022擴充案 修正非刪除才要檢查
            if (this.Action != ActionMode.Delete)
            {
                if (String.IsNullOrEmpty(data.TermName))
                {
                    this.ShowMustInputAlert("學期名稱");
                    return false;
                }

                #region [MDY:202203XX] 2022擴充案 學期英文名稱
                if (this.phdTermEName.Visible && String.IsNullOrEmpty(data.TermEName))
                {
                    this.ShowMustInputAlert("學期英文名稱");
                    return false;
                }
                #endregion
            }
            #endregion

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
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.EditYearId = QueryString.TryGetValue("YearId", String.Empty);
                this.EditTermId = QueryString.TryGetValue("TermId", String.Empty);

                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || !ActionMode.IsMaintinaMode (this.Action)
                    || ((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) && String.IsNullOrEmpty(this.EditTermId)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 檢查商家代號授權
                if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                TermListEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new TermListEntity();
                            data.ReceiveType = this.EditReceiveType;
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
                            Expression where = new Expression(TermListEntity.Field.ReceiveType, this.EditReceiveType)
                                .And(TermListEntity.Field.YearId, this.EditYearId)
                                .And(TermListEntity.Field.TermId, this.EditTermId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<TermListEntity>(this, where, null, out data);
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

                this.ucFilter1.GetDataAndBind(this.EditReceiveType, this.EditYearId, null);

                this.BindEditData(data);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            TermListEntity data = this.GetEditData();
            if (!this.CheckEditData(data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1100001.aspx";
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
                        XmlResult xmlResult = DataProxy.Current.Insert<TermListEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, string.Empty, string.Empty);

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
                        Expression where = new Expression(TermListEntity.Field.ReceiveType, data.ReceiveType)
                            .And(TermListEntity.Field.YearId, data.YearId)
                            .And(TermListEntity.Field.TermId, data.TermId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(TermListEntity.Field.TermName, data.TermName);

                        #region [MDY:202203XX] 2022擴充案 學期英文名稱
                        fieldValues.Add(TermListEntity.Field.TermEName, data.TermEName);
                        #endregion

                        fieldValues.Add(TermListEntity.Field.MdyUser, this.GetLogonUser().UserId);
                        fieldValues.Add(TermListEntity.Field.MdyDate, DateTime.Now);
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<TermListEntity>(this, where, fieldValues, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, string.Empty, string.Empty);

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
                        XmlResult xmlResult = DataProxy.Current.Delete<TermListEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, string.Empty, string.Empty, string.Empty);

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