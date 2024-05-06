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
    /// 代收費用別代碼 (維護)
    /// </summary>
    public partial class D1100003M : BasePage
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
        /// 編輯的代收費用別參數
        /// </summary>
        private string EditReceiveId
        {
            get
            {
                return ViewState["EditReceiveId"] as string;
            }
            set
            {
                ViewState["EditReceiveId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的商家代號是否使用兩碼費用別代碼
        /// </summary>
        private string EditBigReceiveIdFlag
        {
            get
            {
                return ViewState["EditBigReceiveIdFlag"] as string;
            }
            set
            {
                ViewState["EditBigReceiveIdFlag"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxReceiveId.Text = String.Empty;
            this.tbxReceiveName.Text = String.Empty;
            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 代收費用別英文名稱
            this.phdReceiveEName.Visible = false;
            this.tbxReceiveEName.Text = String.Empty;
            #endregion
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(ReceiveListEntity data)
        {
            if (data == null)
            {
                this.tbxReceiveId.Text = String.Empty;
                this.tbxReceiveName.Text = String.Empty;
                this.ccbtnOK.Visible = false;

                #region [MDY:202203XX] 2022擴充案 代收費用別英文名稱
                this.phdReceiveEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
                this.tbxReceiveEName.Text = String.Empty;
                #endregion

                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxReceiveId.Enabled = true;
                    this.tbxReceiveName.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.tbxReceiveId.Enabled = false;
                    this.tbxReceiveName.Enabled = true;
                    break;
                default:
                    this.tbxReceiveId.Enabled = false;
                    this.tbxReceiveName.Enabled = false;
                    break;
            }

            this.tbxReceiveId.Text = data.ReceiveId;
            this.tbxReceiveName.Text = data.ReceiveName;
            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 代收費用別英文名稱
            this.phdReceiveEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
            this.tbxReceiveEName.Enabled = this.tbxReceiveName.Enabled;
            this.tbxReceiveEName.Text = data.ReceiveEName;
            #endregion
        }

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private ReceiveListEntity GetEditData()
        {
            ReceiveListEntity data = new ReceiveListEntity();
            data.ReceiveType = this.EditReceiveType;
            data.YearId = this.EditYearId;
            data.TermId = this.EditTermId;
            data.DepId = this.EditDepId;

            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    data.ReceiveId = this.tbxReceiveId.Text.Trim();
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    data.ReceiveId = this.EditReceiveId;
                    break;
            }
            data.ReceiveName = this.tbxReceiveName.Text.Trim();

            #region [MDY:202203XX] 2022擴充案 代收費用別英文名稱
            if (this.phdReceiveEName.Visible)
            {
                data.ReceiveEName = this.tbxReceiveEName.Text.Trim();
            }
            else
            {
                data.ReceiveEName = String.Empty;
            }
            #endregion

            return data;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData(ReceiveListEntity data)
        {
            if (String.IsNullOrEmpty(data.ReceiveId))
            {
                this.ShowMustInputAlert("代收費用別代碼");
                return false;
            }
            int maxReceiveIdSize = this.EditBigReceiveIdFlag == "Y" ? 2 : 1;
            if (!Common.IsNumber(data.ReceiveId, 1, maxReceiveIdSize))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized(String.Format("代收費用別代碼限輸入做多{0}碼數字", maxReceiveIdSize));
                this.ShowJsAlert(msg);
                return false;
            }

            #region [MDY:202203XX] 2022擴充案 修正非刪除才要檢查
            if (this.Action != ActionMode.Delete)
            {
                if (String.IsNullOrEmpty(tbxReceiveName.Text))
                {
                    this.ShowMustInputAlert("代收費用別名稱");
                    return false;
                }

                #region [MDY:202203XX] 2022擴充案 代收費用別英文名稱
                if (this.phdReceiveEName.Visible && String.IsNullOrEmpty(data.ReceiveEName))
                {
                    this.ShowMustInputAlert("代收費用別英文名稱");
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
                this.EditReceiveId = QueryString.TryGetValue("ReceiveId", String.Empty);

                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                    //|| String.IsNullOrEmpty(this.EditDepId)
                    || !ActionMode.IsMaintinaMode(this.Action)
                    || ((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) && String.IsNullOrEmpty(this.EditReceiveId)))
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

                this.ucFilter1.GetDataAndBind(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, null);

                #region 取得是否使用兩碼費用別代碼
                {
                    string action = this.GetLocalized("查詢商家代號資料");
                    SchoolRTypeEntity school = null;
                    Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, this.EditReceiveType);
                    XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this, where, null, out school);
                    if (!xmlResult.IsSuccess)
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        this.ccbtnOK.Visible = false;
                        return;
                    }
                    if (school == null)
                    {
                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                        this.ccbtnOK.Visible = false;
                        return;
                    }
                    this.EditBigReceiveIdFlag = school.BigReceiveIdFlag;
                    if (school.BigReceiveIdFlag == "Y")
                    {
                        this.tbxReceiveId.MaxLength = 2;
                        this.labBigReceiveIdMemo.Visible = true;
                    }
                }
                #endregion

                #region 取得維護資料
                ReceiveListEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new ReceiveListEntity();
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
                            Expression where = new Expression(ReceiveListEntity.Field.ReceiveType, this.EditReceiveType)
                                .And(ReceiveListEntity.Field.YearId, this.EditYearId)
                                .And(ReceiveListEntity.Field.TermId, this.EditTermId)
                                .And(ReceiveListEntity.Field.DepId, this.EditDepId)
                                .And(ReceiveListEntity.Field.ReceiveId, this.EditReceiveId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<ReceiveListEntity>(this, where, null, out data);
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
            ReceiveListEntity data = this.GetEditData();
            if (!this.CheckEditData(data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1100003.aspx";
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

                        #region 因費用別代碼擴充為2碼又可以只輸入1碼，須將 0~9 與 00~09 視為相同，所以需用程式檢查是否已存在
                        int receiveId = Int32.Parse(data.ReceiveId);
                        if (receiveId < 10)
                        {
                            string[] receiveIds = new string[2];
                            receiveIds[0] = receiveId.ToString();
                            receiveIds[1] = "0" + receiveIds[0];
                            Expression where = new Expression(ReceiveListEntity.Field.ReceiveType, data.ReceiveType)
                                .And(ReceiveListEntity.Field.YearId, data.YearId)
                                .And(ReceiveListEntity.Field.TermId, data.TermId)
                                .And(ReceiveListEntity.Field.DepId, data.DepId)
                                .And(ReceiveListEntity.Field.ReceiveId, RelationEnum.In, receiveIds);
                            XmlResult xmlResult2 = DataProxy.Current.SelectCount<ReceiveListEntity>(this, where,  out count);
                            if (xmlResult2.IsSuccess)
                            {
                                if (count > 0)
                                {
                                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                                    return;
                                }
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, xmlResult2.Code, xmlResult2.Message);
                            }
                        }
                        #endregion

                        XmlResult xmlResult = DataProxy.Current.Insert<ReceiveListEntity>(this, data, out count);
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
                        Expression where = new Expression(ReceiveListEntity.Field.ReceiveType, data.ReceiveType)
                            .And(ReceiveListEntity.Field.YearId, data.YearId)
                            .And(ReceiveListEntity.Field.TermId, data.TermId)
                            .And(ReceiveListEntity.Field.DepId, data.DepId)
                            .And(ReceiveListEntity.Field.ReceiveId, data.ReceiveId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(ReceiveListEntity.Field.ReceiveName, data.ReceiveName);

                        #region [MDY:202203XX] 2022擴充案 代收費用別英文名稱
                        fieldValues.Add(ReceiveListEntity.Field.ReceiveEName, data.ReceiveEName);
                        #endregion

                        fieldValues.Add(ReceiveListEntity.Field.MdyUser, this.GetLogonUser().UserId);
                        fieldValues.Add(ReceiveListEntity.Field.MdyDate, DateTime.Now);
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<ReceiveListEntity>(this, where, fieldValues, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<ReceiveListEntity>(this, data, out count);
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