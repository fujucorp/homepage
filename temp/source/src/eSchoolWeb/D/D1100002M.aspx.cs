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
    /// 部別代碼 (維護)
    /// </summary>
    public partial class D1100002M : BasePage
    {
        #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
        //#region Keep 頁面參數
        ///// <summary>
        ///// 編輯模式參數
        ///// </summary>
        //private string Action
        //{
        //    get
        //    {
        //        return ViewState["ACTION"] as string;
        //    }
        //    set
        //    {
        //        ViewState["ACTION"] = value == null ? null : value.Trim().ToUpper();
        //    }
        //}

        ///// <summary>
        ///// 編輯的業務別碼參數
        ///// </summary>
        //private string EditReceiveType
        //{
        //    get
        //    {
        //        return ViewState["EditReceiveType"] as string;
        //    }
        //    set
        //    {
        //        ViewState["EditReceiveType"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 編輯的學年參數
        ///// </summary>
        //private string EditYearId
        //{
        //    get
        //    {
        //        return ViewState["EditYearId"] as string;
        //    }
        //    set
        //    {
        //        ViewState["EditYearId"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 編輯的學期參數
        ///// </summary>
        //private string EditTermId
        //{
        //    get
        //    {
        //        return ViewState["EditTermId"] as string;
        //    }
        //    set
        //    {
        //        ViewState["EditTermId"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 編輯的部別參數
        ///// </summary>
        //private string EditDepId
        //{
        //    get
        //    {
        //        return ViewState["EditDepId"] as string;
        //    }
        //    set
        //    {
        //        ViewState["EditDepId"] = value == null ? null : value.Trim();
        //    }
        //}
        //#endregion

        ///// <summary>
        ///// 初始化使用介面
        ///// </summary>
        //private void InitialUI()
        //{
        //    this.tbxDepId.Text = String.Empty;
        //    this.tbxDepName.Text = String.Empty;
        //    this.ccbtnOK.Visible = true;
        //}

        ///// <summary>
        ///// 結繫維護資料
        ///// </summary>
        ///// <param name="data">維護資料</param>
        //private void BindEditData(DepListEntity data)
        //{
        //    if (data == null)
        //    {
        //        this.tbxDepId.Text = String.Empty;
        //        this.tbxDepName.Text = String.Empty;
        //        this.ccbtnOK.Visible = false;
        //        return;
        //    }

        //    switch (this.Action)
        //    {
        //        case ActionMode.Insert:
        //            this.tbxDepId.Enabled = true;
        //            this.tbxDepName.Enabled = true;
        //            break;
        //        case ActionMode.Modify:
        //            this.tbxDepId.Enabled = false;
        //            this.tbxDepName.Enabled = true;
        //            break;
        //        default:
        //            this.tbxDepId.Enabled = false;
        //            this.tbxDepName.Enabled = false;
        //            break;
        //    }
        //    this.tbxDepId.Text = data.DepId;
        //    this.tbxDepName.Text = data.DepName;
        //    this.ccbtnOK.Visible = true;
        //}

        ///// <summary>
        ///// 取得輸入的維護資料
        ///// </summary>
        ///// <returns>傳回輸入的維護資料</returns>
        //private DepListEntity GetEditData()
        //{
        //    DepListEntity data = new DepListEntity();
        //    data.ReceiveType = this.EditReceiveType;
        //    data.YearId = this.EditYearId;
        //    data.TermId = this.EditTermId;

        //    switch (this.Action)
        //    {
        //        case ActionMode.Insert:     //新增
        //            data.DepId = this.tbxDepId.Text.Trim();
        //            break;
        //        case ActionMode.Modify:     //修改
        //        case ActionMode.Delete:     //刪除
        //            data.DepId = this.EditDepId;
        //            break;
        //    }
        //    data.DepName = this.tbxDepName.Text.Trim();
        //    return data;
        //}

        ///// <summary>
        ///// 檢查輸入的維護資料
        ///// </summary>
        ///// <returns>成功則傳回 true，否則傳回 false</returns>
        //private bool CheckEditData(DepListEntity data)
        //{
        //    if (String.IsNullOrEmpty(data.DepId))
        //    {
        //        this.ShowMustInputAlert("部別代碼");
        //        return false;
        //    }
        //    if (!Common.IsNumber(data.DepId, 1))
        //    {
        //        //[TODO] 固定顯示訊息的收集
        //        string msg = this.GetLocalized("部別代碼限輸入1碼數字");
        //        this.ShowJsAlert(msg);
        //        return false;
        //    }
        //    if (String.IsNullOrEmpty(data.DepName))
        //    {
        //        this.ShowMustInputAlert("部別名稱");
        //        return false;
        //    }
        //    return true;
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!this.IsPostBack)
        //    {
        //        this.InitialUI();

        //        #region 檢查維護權限
        //        if (!this.HasMaintainAuth())
        //        {
        //            this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
        //            return;
        //        }
        //        #endregion

        //        #region 處理參數
        //        KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
        //        if (QueryString == null || QueryString.Count == 0)
        //        {
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("缺少網頁參數");
        //            this.ShowSystemMessage(msg);
        //            this.ccbtnOK.Visible = false;
        //            return;
        //        }

        //        this.Action = QueryString.TryGetValue("Action", String.Empty);
        //        this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
        //        this.EditYearId = QueryString.TryGetValue("YearId", String.Empty);
        //        this.EditTermId = QueryString.TryGetValue("TermId", String.Empty);
        //        this.EditDepId = QueryString.TryGetValue("DepId", String.Empty);

        //        if (String.IsNullOrEmpty(this.EditReceiveType)
        //            || String.IsNullOrEmpty(this.EditYearId)
        //            || String.IsNullOrEmpty(this.EditTermId)
        //            || !ActionMode.IsMaintinaMode(this.Action)
        //            || ((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) && String.IsNullOrEmpty(this.EditDepId)))
        //        {
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("網頁參數不正確");
        //            this.ShowSystemMessage(msg);
        //            this.ccbtnOK.Visible = false;
        //            return;
        //        }
        //        #endregion

        //        #region 檢查業務別碼授權
        //        if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
        //        {
        //            this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
        //            this.ccbtnOK.Visible = false;
        //            return;
        //        }
        //        #endregion

        //        #region 取得維護資料
        //        DepListEntity data = null;
        //        switch (this.Action)
        //        {
        //            case ActionMode.Insert:   //新增
        //                #region 新增
        //                {
        //                    //空的資料
        //                    data = new DepListEntity();
        //                    data.ReceiveType = this.EditReceiveType;
        //                    data.YearId = this.EditYearId;
        //                    data.TermId = this.EditTermId;
        //                }
        //                #endregion
        //                break;
        //            case ActionMode.Modify:   //修改
        //            case ActionMode.Delete:   //刪除
        //                #region 修改 | 刪除
        //                {
        //                    string action = this.GetLocalized("查詢要維護的資料");

        //                    #region 查詢條件
        //                    Expression where = new Expression(DepListEntity.Field.ReceiveType, this.EditReceiveType)
        //                        .And(DepListEntity.Field.YearId, this.EditYearId)
        //                        .And(DepListEntity.Field.TermId, this.EditTermId)
        //                        .And(DepListEntity.Field.DepId, this.EditDepId);
        //                    #endregion

        //                    #region 查詢資料
        //                    XmlResult xmlResult = DataProxy.Current.SelectFirst<DepListEntity>(this, where, null, out data);
        //                    if (!xmlResult.IsSuccess)
        //                    {
        //                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                        this.ccbtnOK.Visible = false;
        //                        return;
        //                    }
        //                    if (data == null)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                        this.ccbtnOK.Visible = false;
        //                        return;
        //                    }
        //                    #endregion
        //                }
        //                #endregion
        //                break;
        //        }
        //        #endregion

        //        this.ucFilter1.GetDataAndBind(this.EditReceiveType, this.EditYearId, this.EditTermId);

        //        this.BindEditData(data);
        //    }
        //}

        //protected void ccbtnOK_Click(object sender, EventArgs e)
        //{
        //    DepListEntity data = this.GetEditData();
        //    if (!this.CheckEditData(data))
        //    {
        //        return;
        //    }

        //    string action = ActionMode.GetActionLocalized(this.Action);
        //    string backUrl = "D1100002.aspx";
        //    switch (this.Action)
        //    {
        //        case ActionMode.Insert:     //新增
        //            #region 新增
        //            {
        //                #region 補齊資料
        //                data.Status = DataStatusCodeTexts.NORMAL;
        //                data.CrtUser = this.GetLogonUser().UserId;
        //                data.CrtDate = DateTime.Now;
        //                #endregion

        //                int count = 0;
        //                XmlResult xmlResult = DataProxy.Current.Insert<DepListEntity>(this, data, out count);
        //                if (xmlResult.IsSuccess)
        //                {
        //                    if (count < 1)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
        //                    }
        //                    else
        //                    {
        //                        WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, string.Empty, string.Empty);

        //                        this.ShowActionSuccessAlert(action, backUrl);
        //                    }
        //                }
        //                else
        //                {
        //                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                }
        //            }
        //            #endregion
        //            break;
        //        case ActionMode.Modify:     //修改
        //            #region 修改
        //            {
        //                #region 更新條件
        //                Expression where = new Expression(DepListEntity.Field.ReceiveType, data.ReceiveType)
        //                    .And(DepListEntity.Field.YearId, data.YearId)
        //                    .And(DepListEntity.Field.TermId, data.TermId)
        //                    .And(DepListEntity.Field.DepId, data.DepId);
        //                #endregion

        //                #region 更新欄位
        //                KeyValueList fieldValues = new KeyValueList();
        //                fieldValues.Add(DepListEntity.Field.DepName, data.DepName);
        //                fieldValues.Add(DepListEntity.Field.MdyUser, this.GetLogonUser().UserId);
        //                fieldValues.Add(DepListEntity.Field.MdyDate, DateTime.Now);
        //                #endregion

        //                int count = 0;
        //                XmlResult xmlResult = DataProxy.Current.UpdateFields<DepListEntity>(this, where, fieldValues, out count);
        //                if (xmlResult.IsSuccess)
        //                {
        //                    if (count < 1)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                    }
        //                    else
        //                    {
        //                        WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, string.Empty, string.Empty);

        //                        this.ShowActionSuccessAlert(action, backUrl);
        //                    }
        //                }
        //                else
        //                {
        //                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                }
        //            }
        //            #endregion
        //            break;
        //        case ActionMode.Delete:     //刪除
        //            #region 刪除
        //            {
        //                int count = 0;
        //                XmlResult xmlResult = DataProxy.Current.Delete<DepListEntity>(this, data, out count);
        //                if (xmlResult.IsSuccess)
        //                {
        //                    if (count < 1)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                    }
        //                    else
        //                    {
        //                        WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, string.Empty, string.Empty);

        //                        this.ShowActionSuccessAlert(action, backUrl);
        //                    }
        //                }
        //                else
        //                {
        //                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                }
        //            }
        //            #endregion
        //            break;
        //    }
        //}
        #endregion

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
        /// 編輯的土銀專用部別參數
        /// </summary>
        private string EditDeptId
        {
            get
            {
                return ViewState["EditDeptId"] as string;
            }
            set
            {
                ViewState["EditDeptId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxDeptId.Text = String.Empty;
            this.tbxDeptName.Text = String.Empty;
            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 部別英文名稱
            this.phdDeptEName.Visible = false;
            this.tbxDeptEName.Text = String.Empty;
            #endregion
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(DeptListEntity data)
        {
            if (data == null)
            {
                this.tbxDeptId.Text = String.Empty;
                this.tbxDeptName.Text = String.Empty;
                this.ccbtnOK.Visible = false;

                #region [MDY:202203XX] 2022擴充案 部別英文名稱
                this.phdDeptEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
                this.tbxDeptEName.Text = String.Empty;
                #endregion

                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxDeptId.Enabled = true;
                    this.tbxDeptName.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.tbxDeptId.Enabled = false;
                    this.tbxDeptName.Enabled = true;
                    break;
                default:
                    this.tbxDeptId.Enabled = false;
                    this.tbxDeptName.Enabled = false;
                    break;
            }
            this.tbxDeptId.Text = data.DeptId;
            this.tbxDeptName.Text = data.DeptName;
            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 部別英文名稱
            this.phdDeptEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
            this.tbxDeptEName.Enabled = this.tbxDeptName.Enabled;
            this.tbxDeptEName.Text = data.DeptEName;
            #endregion
        }

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private DeptListEntity GetEditData()
        {
            DeptListEntity data = new DeptListEntity();
            data.ReceiveType = this.EditReceiveType;
            data.YearId = this.EditYearId;
            data.TermId = this.EditTermId;

            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    data.DeptId = this.tbxDeptId.Text.Trim();
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    data.DeptId = this.EditDeptId;
                    break;
            }
            data.DeptName = this.tbxDeptName.Text.Trim();

            #region [MDY:202203XX] 2022擴充案 部別英文名稱
            if (this.phdDeptEName.Visible)
            {
                data.DeptEName = this.tbxDeptEName.Text.Trim();
            }
            else
            {
                data.DeptEName = String.Empty;
            }
            #endregion

            return data;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData(DeptListEntity data)
        {
            if (String.IsNullOrEmpty(data.DeptId))
            {
                this.ShowMustInputAlert("部別代碼");
                return false;
            }

            #region [MDY:202203XX] 2022擴充案 修正新增時才限制 1～20 碼英數
            if (this.Action == ActionMode.Insert)
            {
                if (!Common.IsEnglishNumber(data.DeptId, 1, 20))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("部別代碼限輸入1~20碼的英文、數字或英數字混合");
                    this.ShowJsAlert(msg);
                    return false;
                }
            }
            else if (String.IsNullOrWhiteSpace(data.DeptId))
            {
                string msg = this.GetLocalized("無法取得部別代碼");
                this.ShowJsAlert(msg);
                return false;
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 修正非刪除才要檢查
            if (this.Action != ActionMode.Delete)
            {
                if (String.IsNullOrEmpty(data.DeptName))
                {
                    this.ShowMustInputAlert("部別名稱");
                    return false;
                }

                #region [MDY:202203XX] 2022擴充案 部別英文名稱
                if (this.phdDeptEName.Visible && String.IsNullOrEmpty(data.DeptEName))
                {
                    this.ShowMustInputAlert("部別英文名稱");
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
                this.EditDeptId = QueryString.TryGetValue("DeptId", String.Empty);

                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                    || !ActionMode.IsMaintinaMode(this.Action)
                    || ((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) && String.IsNullOrEmpty(this.EditDeptId)))
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
                DeptListEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new DeptListEntity();
                            data.ReceiveType = this.EditReceiveType;
                            data.YearId = this.EditYearId;
                            data.TermId = this.EditTermId;
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(DeptListEntity.Field.ReceiveType, this.EditReceiveType)
                                .And(DeptListEntity.Field.YearId, this.EditYearId)
                                .And(DeptListEntity.Field.TermId, this.EditTermId)
                                .And(DeptListEntity.Field.DeptId, this.EditDeptId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<DeptListEntity>(this, where, null, out data);
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

                this.ucFilter1.GetDataAndBind(this.EditReceiveType, this.EditYearId, this.EditTermId);

                this.BindEditData(data);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            DeptListEntity data = this.GetEditData();
            if (!this.CheckEditData(data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1100002.aspx";
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
                        XmlResult xmlResult = DataProxy.Current.Insert<DeptListEntity>(this, data, out count);
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
                        Expression where = new Expression(DeptListEntity.Field.ReceiveType, data.ReceiveType)
                            .And(DeptListEntity.Field.YearId, data.YearId)
                            .And(DeptListEntity.Field.TermId, data.TermId)
                            .And(DeptListEntity.Field.DeptId, data.DeptId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(DeptListEntity.Field.DeptName, data.DeptName);

                        #region [MDY:202203XX] 2022擴充案 部別英文名稱
                        fieldValues.Add(DeptListEntity.Field.DeptEName, data.DeptEName);
                        #endregion

                        fieldValues.Add(DeptListEntity.Field.MdyUser, this.GetLogonUser().UserId);
                        fieldValues.Add(DeptListEntity.Field.MdyDate, DateTime.Now);
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<DeptListEntity>(this, where, fieldValues, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<DeptListEntity>(this, data, out count);
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
            }
        }
    }
}