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
    /// 就貸代碼 (維護)
    /// </summary>
    public partial class D1100010M : BasePage
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

        /// <summary>
        /// 編輯的部別參數
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
        /// 編輯的就貸代碼參數
        /// </summary>
        private string EditLoanId
        {
            get
            {
                return ViewState["EditLoanId"] as string;
            }
            set
            {
                ViewState["EditLoanId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxLoanId.Text = String.Empty;
            this.tbxLoanName.Text = String.Empty;
            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 就貸英文名稱
            this.phdLoanEName.Visible = false;
            this.tbxLoanEName.Text = String.Empty;
            #endregion
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(LoanListEntity data)
        {
            if (data == null)
            {
                this.tbxLoanId.Text = String.Empty;
                this.tbxLoanName.Text = String.Empty;
                this.ccbtnOK.Visible = false;

                #region [MDY:202203XX] 2022擴充案 就貸英文名稱
                this.phdLoanEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
                this.tbxLoanEName.Text = String.Empty;
                #endregion

                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxLoanId.Enabled = true;
                    this.tbxLoanName.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.tbxLoanId.Enabled = false;
                    this.tbxLoanName.Enabled = true;
                    break;
                default:
                    this.tbxLoanId.Enabled = false;
                    this.tbxLoanName.Enabled = false;
                    break;
            }

            this.tbxLoanId.Text = data.LoanId;
            this.tbxLoanName.Text = data.LoanName;
            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 就貸英文名稱
            this.phdLoanEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
            this.tbxLoanEName.Enabled = this.tbxLoanName.Enabled;
            this.tbxLoanEName.Text = data.LoanEName;
            #endregion
        }

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private LoanListEntity GetEditData()
        {
            LoanListEntity data = new LoanListEntity();
            data.ReceiveType = this.EditReceiveType;
            data.YearId = this.EditYearId;
            data.TermId = this.EditTermId;
            data.DepId = this.EditDepId;

            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    data.LoanId = this.tbxLoanId.Text.Trim();
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    data.LoanId = this.EditLoanId;
                    break;
            }
            data.LoanName = this.tbxLoanName.Text.Trim();

            #region [MDY:202203XX] 2022擴充案 就貸英文名稱
            if (this.phdLoanEName.Visible)
            {
                data.LoanEName = this.tbxLoanEName.Text.Trim();
            }
            else
            {
                data.LoanEName = String.Empty;
            }
            #endregion

            return data;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData(LoanListEntity data)
        {
            if (String.IsNullOrEmpty(data.LoanId))
            {
                this.ShowMustInputAlert("就貸代碼");
                return false;
            }

            #region [MDY:202203XX] 2022擴充案 修正新增時才限制 1～20 碼英數
            if (this.Action == ActionMode.Insert)
            {
                if (!Common.IsEnglishNumber(data.LoanId, 1, 20))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("就貸代碼最多輸入20碼的英文、數字或英數字混合");
                    this.ShowJsAlert(msg);
                    return false;
                }
            }
            else if (String.IsNullOrWhiteSpace(data.LoanId))
            {
                string msg = this.GetLocalized("無法取得就貸代碼");
                this.ShowJsAlert(msg);
                return false;
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 修正非刪除才要檢查
            if (this.Action != ActionMode.Delete)
            {
                if (String.IsNullOrEmpty(data.LoanName))
                {
                    this.ShowMustInputAlert("就貸名稱");
                    return false;
                }

                #region [MDY:202203XX] 2022擴充案 就貸英文名稱
                if (this.phdLoanEName.Visible && String.IsNullOrEmpty(data.LoanEName))
                {
                    this.ShowMustInputAlert("就貸英文名稱");
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
                this.EditDepId = QueryString.TryGetValue("DepId", String.Empty);
                this.EditLoanId = QueryString.TryGetValue("LoanId", String.Empty);

                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                   // || String.IsNullOrEmpty(this.EditDepId)
                    || !ActionMode.IsMaintinaMode(this.Action)
                    || ((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) && String.IsNullOrEmpty(this.EditLoanId)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 檢查業務別碼授權
                if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                LoanListEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new LoanListEntity();
                            data.ReceiveType = this.EditReceiveType;
                            data.YearId = this.EditYearId;
                            data.TermId = this.EditTermId;
                            data.DepId = this.EditDepId;
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(LoanListEntity.Field.ReceiveType, this.EditReceiveType)
                                .And(LoanListEntity.Field.YearId, this.EditYearId)
                                .And(LoanListEntity.Field.TermId, this.EditTermId)
                                .And(LoanListEntity.Field.DepId, this.EditDepId)
                                .And(LoanListEntity.Field.LoanId, this.EditLoanId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<LoanListEntity>(this, where, null, out data);
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

                this.ucFilter1.GetDataAndBind(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, null);

                this.BindEditData(data);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            LoanListEntity data = this.GetEditData();
            if (!this.CheckEditData(data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1100010.aspx";
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
                        XmlResult xmlResult = DataProxy.Current.Insert<LoanListEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, data.DepId, string.Empty);

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
                        Expression where = new Expression(LoanListEntity.Field.ReceiveType, data.ReceiveType)
                            .And(LoanListEntity.Field.YearId, data.YearId)
                            .And(LoanListEntity.Field.TermId, data.TermId)
                            .And(LoanListEntity.Field.DepId, data.DepId)
                            .And(LoanListEntity.Field.LoanId, data.LoanId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(LoanListEntity.Field.LoanName, data.LoanName);

                        #region [MDY:202203XX] 2022擴充案 就貸英文名稱
                        fieldValues.Add(LoanListEntity.Field.LoanEName, data.LoanEName);
                        #endregion

                        fieldValues.Add(LoanListEntity.Field.MdyUser, this.GetLogonUser().UserId);
                        fieldValues.Add(LoanListEntity.Field.MdyDate, DateTime.Now);
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<LoanListEntity>(this, where, fieldValues, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, data.DepId, string.Empty);

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
                        XmlResult xmlResult = DataProxy.Current.Delete<LoanListEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, data.DepId, string.Empty);

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