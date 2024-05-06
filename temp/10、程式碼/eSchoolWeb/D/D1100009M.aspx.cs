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
    /// 身分註記代碼 (維護)
    /// </summary>
    public partial class D1100009M : BasePage
    {
        #region Keep 頁面參數
        /// <summary>
        /// 編輯模式參數
        /// </summary>
        private string Action
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["ACTION"] as string);
            }
            set
            {
                ViewState["ACTION"] = value == null ? null : value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// 編輯的身分註記種類參數
        /// </summary>
        private string EditIdentifyType
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditIdentifyType"] as string);
            }
            set
            {
                ViewState["EditIdentifyType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的業務別碼參數
        /// </summary>
        private string EditReceiveType
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditReceiveType"] as string);
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
                return HttpUtility.HtmlEncode(ViewState["EditYearId"] as string);
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
                return HttpUtility.HtmlEncode(ViewState["EditTermId"] as string);
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
                return HttpUtility.HtmlEncode(ViewState["EditDepId"] as string);
            }
            set
            {
                ViewState["EditDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的身分註記代碼參數
        /// </summary>
        private string EditIdentifyId
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditIdentifyId"] as string);
            }
            set
            {
                ViewState["EditIdentifyId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.labIdentifyTypeText.Text = String.Empty;
            this.tbxIdentifyId.Text = String.Empty;
            this.tbxIdentifyName.Text = String.Empty;
            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
            this.phdIdentifyEName.Visible = false;
            this.tbxIdentifyEName.Text = String.Empty;
            #endregion
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="identifyType">身分註記種類</param>
        /// <param name="identifyData">維護資料</param>
        private void BindEditData(string identifyType, object identifyData)
        {
            CodeText data = null;

            #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
            string identifyEName = null;
            switch (identifyType)
            {
                case "1":
                    {
                        IdentifyList1Entity instance = identifyData as IdentifyList1Entity;
                        if (instance != null)
                        {
                            data = new CodeText(instance.IdentifyId ?? String.Empty, instance.IdentifyName ?? String.Empty);
                            identifyEName = instance.IdentifyEName;
                        }
                    }
                    break;
                case "2":
                    {
                        IdentifyList2Entity instance = identifyData as IdentifyList2Entity;
                        if (instance != null)
                        {
                            data = new CodeText(instance.IdentifyId ?? String.Empty, instance.IdentifyName ?? String.Empty);
                            identifyEName = instance.IdentifyEName;
                        }
                    }
                    break;
                case "3":
                    {
                        IdentifyList3Entity instance = identifyData as IdentifyList3Entity;
                        if (instance != null)
                        {
                            data = new CodeText(instance.IdentifyId ?? String.Empty, instance.IdentifyName ?? String.Empty);
                            identifyEName = instance.IdentifyEName;
                        }
                    }
                    break;
                case "4":
                    {
                        IdentifyList4Entity instance = identifyData as IdentifyList4Entity;
                        if (instance != null)
                        {
                            data = new CodeText(instance.IdentifyId ?? String.Empty, instance.IdentifyName ?? String.Empty);
                            identifyEName = instance.IdentifyEName;
                        }
                    }
                    break;
                case "5":
                    {
                        IdentifyList5Entity instance = identifyData as IdentifyList5Entity;
                        if (instance != null)
                        {
                            data = new CodeText(instance.IdentifyId ?? String.Empty, instance.IdentifyName ?? String.Empty);
                            identifyEName = instance.IdentifyEName;
                        }
                    }
                    break;
                case "6":
                    {
                        IdentifyList6Entity instance = identifyData as IdentifyList6Entity;
                        if (instance != null)
                        {
                            data = new CodeText(instance.IdentifyId ?? String.Empty, instance.IdentifyName ?? String.Empty);
                            identifyEName = instance.IdentifyEName;
                        }
                    }
                    break;
            }
            #endregion

            if (data == null)
            {
                this.tbxIdentifyId.Text = String.Empty;
                this.tbxIdentifyName.Text = String.Empty;
                this.ccbtnOK.Visible = false;

                #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                this.phdIdentifyEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
                this.tbxIdentifyEName.Text = String.Empty;
                #endregion
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxIdentifyId.Enabled = true;
                    this.tbxIdentifyName.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.tbxIdentifyId.Enabled = false;
                    this.tbxIdentifyName.Enabled = true;
                    break;
                default:
                    this.tbxIdentifyId.Enabled = false;
                    this.tbxIdentifyName.Enabled = false;
                    break;
            }

            this.tbxIdentifyId.Text = data.Code;
            this.tbxIdentifyName.Text = data.Text;
            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
            this.phdIdentifyEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
            this.tbxIdentifyEName.Enabled = this.tbxIdentifyName.Enabled;
            this.tbxIdentifyEName.Text = identifyEName;
            #endregion
        }

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <param name="identifyType">身分註記種類</param>
        /// <returns>傳回輸入的維護資料</returns>
        private object GetEditData(string identifyType)
        {
            object data = null;

            string identifyId = null;
            string identifyName = null;
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    identifyId = this.tbxIdentifyId.Text.Trim();
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    identifyId = this.EditIdentifyId;
                    break;
            }
            identifyName = this.tbxIdentifyName.Text.Trim();

            switch (identifyType)
            {
                case "1":
                    {
                        IdentifyList1Entity instance = new IdentifyList1Entity();
                        instance.ReceiveType = this.EditReceiveType;
                        instance.YearId = this.EditYearId;
                        instance.TermId = this.EditTermId;
                        instance.DepId = this.EditDepId;
                        instance.IdentifyId = identifyId;
                        instance.IdentifyName = identifyName;

                        #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                        if (this.phdIdentifyEName.Visible)
                        {
                            instance.IdentifyEName = this.tbxIdentifyEName.Text.Trim();
                        }
                        else
                        {
                            instance.IdentifyEName = String.Empty;
                        }
                        #endregion

                        data = instance;
                    }
                    break;
                case "2":
                    {
                        IdentifyList2Entity instance = new IdentifyList2Entity();
                        instance.ReceiveType = this.EditReceiveType;
                        instance.YearId = this.EditYearId;
                        instance.TermId = this.EditTermId;
                        instance.DepId = this.EditDepId;
                        instance.IdentifyId = identifyId;
                        instance.IdentifyName = identifyName;

                        #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                        if (this.phdIdentifyEName.Visible)
                        {
                            instance.IdentifyEName = this.tbxIdentifyEName.Text.Trim();
                        }
                        else
                        {
                            instance.IdentifyEName = String.Empty;
                        }
                        #endregion

                        data = instance;
                    }
                    break;
                case "3":
                    {
                        IdentifyList3Entity instance = new IdentifyList3Entity();
                        instance.ReceiveType = this.EditReceiveType;
                        instance.YearId = this.EditYearId;
                        instance.TermId = this.EditTermId;
                        instance.DepId = this.EditDepId;
                        instance.IdentifyId = identifyId;
                        instance.IdentifyName = identifyName;

                        #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                        if (this.phdIdentifyEName.Visible)
                        {
                            instance.IdentifyEName = this.tbxIdentifyEName.Text.Trim();
                        }
                        else
                        {
                            instance.IdentifyEName = String.Empty;
                        }
                        #endregion

                        data = instance;
                    }
                    break;
                case "4":
                    {
                        IdentifyList4Entity instance = new IdentifyList4Entity();
                        instance.ReceiveType = this.EditReceiveType;
                        instance.YearId = this.EditYearId;
                        instance.TermId = this.EditTermId;
                        instance.DepId = this.EditDepId;
                        instance.IdentifyId = identifyId;
                        instance.IdentifyName = identifyName;

                        #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                        if (this.phdIdentifyEName.Visible)
                        {
                            instance.IdentifyEName = this.tbxIdentifyEName.Text.Trim();
                        }
                        else
                        {
                            instance.IdentifyEName = String.Empty;
                        }
                        #endregion

                        data = instance;
                    }
                    break;
                case "5":
                    {
                        IdentifyList5Entity instance = new IdentifyList5Entity();
                        instance.ReceiveType = this.EditReceiveType;
                        instance.YearId = this.EditYearId;
                        instance.TermId = this.EditTermId;
                        instance.DepId = this.EditDepId;
                        instance.IdentifyId = identifyId;
                        instance.IdentifyName = identifyName;

                        #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                        if (this.phdIdentifyEName.Visible)
                        {
                            instance.IdentifyEName = this.tbxIdentifyEName.Text.Trim();
                        }
                        else
                        {
                            instance.IdentifyEName = String.Empty;
                        }
                        #endregion

                        data = instance;
                    }
                    break;
                case "6":
                    {
                        IdentifyList6Entity instance = new IdentifyList6Entity();
                        instance.ReceiveType = this.EditReceiveType;
                        instance.YearId = this.EditYearId;
                        instance.TermId = this.EditTermId;
                        instance.DepId = this.EditDepId;
                        instance.IdentifyId = identifyId;
                        instance.IdentifyName = identifyName;

                        #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                        if (this.phdIdentifyEName.Visible)
                        {
                            instance.IdentifyEName = this.tbxIdentifyEName.Text.Trim();
                        }
                        else
                        {
                            instance.IdentifyEName = String.Empty;
                        }
                        #endregion

                        data = instance;
                    }
                    break;
            }
            return data;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <param name="identifyType">身分註記種類</param>
        /// <param name="identifyData">維護資料</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData(string identifyType, object identifyData)
        {
            string identifyId = null;
            string identifyName = null;

            #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
            string identifyEName = null;
            switch (identifyType)
            {
                case "1":
                    if (identifyData is IdentifyList1Entity)
                    {
                        IdentifyList1Entity data = identifyData as IdentifyList1Entity;
                        identifyId = data.IdentifyId;
                        identifyName = data.IdentifyName;
                        identifyEName = data.IdentifyEName;
                    }
                    break;
                case "2":
                    if (identifyData is IdentifyList2Entity)
                    {
                        IdentifyList2Entity data = identifyData as IdentifyList2Entity;
                        identifyId = data.IdentifyId;
                        identifyName = data.IdentifyName;
                        identifyEName = data.IdentifyEName;
                    }
                    break;
                case "3":
                    if (identifyData is IdentifyList3Entity)
                    {
                        IdentifyList3Entity data = identifyData as IdentifyList3Entity;
                        identifyId = data.IdentifyId;
                        identifyName = data.IdentifyName;
                        identifyEName = data.IdentifyEName;
                    }
                    break;
                case "4":
                    if (identifyData is IdentifyList4Entity)
                    {
                        IdentifyList4Entity data = identifyData as IdentifyList4Entity;
                        identifyId = data.IdentifyId;
                        identifyName = data.IdentifyName;
                        identifyEName = data.IdentifyEName;
                    }
                    break;
                case "5":
                    if (identifyData is IdentifyList5Entity)
                    {
                        IdentifyList5Entity data = identifyData as IdentifyList5Entity;
                        identifyId = data.IdentifyId;
                        identifyName = data.IdentifyName;
                        identifyEName = data.IdentifyEName;
                    }
                    break;
                case "6":
                    if (identifyData is IdentifyList6Entity)
                    {
                        IdentifyList6Entity data = identifyData as IdentifyList6Entity;
                        identifyId = data.IdentifyId;
                        identifyName = data.IdentifyName;
                        identifyEName = data.IdentifyEName;
                    }
                    break;
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 修正新增時才限制 1～20 碼英數
            if (this.Action == ActionMode.Insert)
            {
                if (!Common.IsEnglishNumber(identifyId, 1, 20))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("身分註記代碼最多輸入20碼的英文、數字或英數字混合");
                    this.ShowJsAlert(msg);
                    return false;
                }
            }
            else if (String.IsNullOrWhiteSpace(identifyId))
            {
                string msg = this.GetLocalized("無法取得身分註記代碼");
                this.ShowJsAlert(msg);
                return false;
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 修正非刪除才要檢查
            if (this.Action != ActionMode.Delete)
            {
                if (String.IsNullOrEmpty(identifyName))
                {
                    this.ShowMustInputAlert("身分註記名稱");
                    return false;
                }

                #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                if (this.phdIdentifyEName.Visible && String.IsNullOrEmpty(identifyEName))
                {
                    this.ShowMustInputAlert("身分註記英文名稱");
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
                this.EditIdentifyType = QueryString.TryGetValue("IdentifyType", String.Empty);
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.EditYearId = QueryString.TryGetValue("YearId", String.Empty);
                this.EditTermId = QueryString.TryGetValue("TermId", String.Empty);
                this.EditDepId = QueryString.TryGetValue("DepId", String.Empty);
                this.EditIdentifyId = QueryString.TryGetValue("IdentifyId", String.Empty);

                if (String.IsNullOrEmpty(this.EditIdentifyType)
                    || String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                    //|| String.IsNullOrEmpty(this.EditDepId)
                    || !ActionMode.IsMaintinaMode(this.Action)
                    || ((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) && String.IsNullOrEmpty(this.EditIdentifyId)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 檢查 IdentifyType
                int identifyType = 0;
                switch (this.EditIdentifyType)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                        identifyType = int.Parse(this.EditIdentifyType);
                        this.labIdentifyTypeText.Text = this.GetLocalized("身分註記") + this.EditIdentifyType;
                        break;
                    default:
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
                object data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            switch (this.EditIdentifyType)
                            {
                                case "1":
                                    {
                                        IdentifyList1Entity identifyData = new IdentifyList1Entity();
                                        identifyData.ReceiveType = this.EditReceiveType;
                                        identifyData.YearId = this.EditYearId;
                                        identifyData.TermId = this.EditTermId;
                                        identifyData.DepId = this.EditDepId;
                                        data = identifyData;
                                    }
                                    break;
                                case "2":
                                    {
                                        IdentifyList2Entity identifyData = new IdentifyList2Entity();
                                        identifyData.ReceiveType = this.EditReceiveType;
                                        identifyData.YearId = this.EditYearId;
                                        identifyData.TermId = this.EditTermId;
                                        identifyData.DepId = this.EditDepId;
                                        data = identifyData;
                                    }
                                    break;
                                case "3":
                                    {
                                        IdentifyList3Entity identifyData = new IdentifyList3Entity();
                                        identifyData.ReceiveType = this.EditReceiveType;
                                        identifyData.YearId = this.EditYearId;
                                        identifyData.TermId = this.EditTermId;
                                        identifyData.DepId = this.EditDepId;
                                        data = identifyData;
                                    }
                                    break;
                                case "4":
                                    {
                                        IdentifyList4Entity identifyData = new IdentifyList4Entity();
                                        identifyData.ReceiveType = this.EditReceiveType;
                                        identifyData.YearId = this.EditYearId;
                                        identifyData.TermId = this.EditTermId;
                                        identifyData.DepId = this.EditDepId;
                                        data = identifyData;
                                    }
                                    break;
                                case "5":
                                    {
                                        IdentifyList5Entity identifyData = new IdentifyList5Entity();
                                        identifyData.ReceiveType = this.EditReceiveType;
                                        identifyData.YearId = this.EditYearId;
                                        identifyData.TermId = this.EditTermId;
                                        identifyData.DepId = this.EditDepId;
                                        data = identifyData;
                                    }
                                    break;
                                case "6":
                                    {
                                        IdentifyList6Entity identifyData = new IdentifyList6Entity();
                                        identifyData.ReceiveType = this.EditReceiveType;
                                        identifyData.YearId = this.EditYearId;
                                        identifyData.TermId = this.EditTermId;
                                        identifyData.DepId = this.EditDepId;
                                        data = identifyData;
                                    }
                                    break;
                            }
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            string[] receiveTypeFields = new string[] {
                                IdentifyList1Entity.Field.ReceiveType, IdentifyList2Entity.Field.ReceiveType, IdentifyList3Entity.Field.ReceiveType, 
                                IdentifyList4Entity.Field.ReceiveType, IdentifyList5Entity.Field.ReceiveType, IdentifyList6Entity.Field.ReceiveType };
                            string[] yearIdFields = new string[] { 
                                IdentifyList1Entity.Field.YearId, IdentifyList2Entity.Field.YearId, IdentifyList3Entity.Field.YearId, 
                                IdentifyList4Entity.Field.YearId, IdentifyList5Entity.Field.YearId, IdentifyList6Entity.Field.YearId };
                            string[] termIdFields = new string[] { 
                                IdentifyList1Entity.Field.TermId, IdentifyList2Entity.Field.TermId, IdentifyList3Entity.Field.TermId, 
                                IdentifyList4Entity.Field.TermId, IdentifyList5Entity.Field.TermId, IdentifyList6Entity.Field.TermId };
                            string[] depIdFields = new string[] { 
                                IdentifyList1Entity.Field.DepId, IdentifyList2Entity.Field.DepId, IdentifyList3Entity.Field.DepId, 
                                IdentifyList4Entity.Field.DepId, IdentifyList5Entity.Field.DepId, IdentifyList6Entity.Field.DepId };
                            string[] identifyIdFields = new string[] { 
                                IdentifyList1Entity.Field.IdentifyId, IdentifyList2Entity.Field.IdentifyId, IdentifyList3Entity.Field.IdentifyId, 
                                IdentifyList4Entity.Field.IdentifyId, IdentifyList5Entity.Field.IdentifyId, IdentifyList6Entity.Field.IdentifyId };

                            #region 查詢條件
                            Expression where = new Expression(receiveTypeFields[identifyType - 1], this.EditReceiveType)
                                .And(yearIdFields[identifyType - 1], this.EditYearId)
                                .And(termIdFields[identifyType - 1], this.EditTermId)
                                .And(depIdFields[identifyType - 1], this.EditDepId)
                                .And(identifyIdFields[identifyType - 1], this.EditIdentifyId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = null;
                            switch (this.EditIdentifyType)
                            {
                                case "1":
                                    {
                                        IdentifyList1Entity instance = null;
                                        xmlResult = DataProxy.Current.SelectFirst<IdentifyList1Entity>(this, where, null, out instance);
                                        data = instance;
                                    }
                                    break;
                                case "2":
                                    {
                                        IdentifyList2Entity instance = null;
                                        xmlResult = DataProxy.Current.SelectFirst<IdentifyList2Entity>(this, where, null, out instance);
                                        data = instance;
                                    }
                                    break;
                                case "3":
                                    {
                                        IdentifyList3Entity instance = null;
                                        xmlResult = DataProxy.Current.SelectFirst<IdentifyList3Entity>(this, where, null, out instance);
                                        data = instance;
                                    }
                                    break;
                                case "4":
                                    {
                                        IdentifyList4Entity instance = null;
                                        xmlResult = DataProxy.Current.SelectFirst<IdentifyList4Entity>(this, where, null, out instance);
                                        data = instance;
                                    }
                                    break;
                                case "5":
                                    {
                                        IdentifyList5Entity instance = null;
                                        xmlResult = DataProxy.Current.SelectFirst<IdentifyList5Entity>(this, where, null, out instance);
                                        data = instance;
                                    }
                                    break;
                                case "6":
                                    {
                                        IdentifyList6Entity instance = null;
                                        xmlResult = DataProxy.Current.SelectFirst<IdentifyList6Entity>(this, where, null, out instance);
                                        data = instance;
                                    }
                                    break;
                            }

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

                this.BindEditData(this.EditIdentifyType, data);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            object data = this.GetEditData(this.EditIdentifyType);
            if (!this.CheckEditData(this.EditIdentifyType, data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1100009.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;
                        XmlResult xmlResult = null;

                        switch (this.EditIdentifyType)
                        {
                            case "1":
                                {
                                    #region 補齊資料
                                    IdentifyList1Entity instance = data as IdentifyList1Entity;
                                    instance.Status = DataStatusCodeTexts.NORMAL;
                                    instance.CrtUser = this.GetLogonUser().UserId;
                                    instance.CrtDate = DateTime.Now;
                                    #endregion

                                    xmlResult = DataProxy.Current.Insert<IdentifyList1Entity>(this, instance, out count);
                                }
                                break;
                            case "2":
                                {
                                    #region 補齊資料
                                    IdentifyList2Entity instance = data as IdentifyList2Entity;
                                    instance.Status = DataStatusCodeTexts.NORMAL;
                                    instance.CrtUser = this.GetLogonUser().UserId;
                                    instance.CrtDate = DateTime.Now;
                                    #endregion

                                    xmlResult = DataProxy.Current.Insert<IdentifyList2Entity>(this, instance, out count);
                                }
                                break;
                            case "3":
                                {
                                    #region 補齊資料
                                    IdentifyList3Entity instance = data as IdentifyList3Entity;
                                    instance.Status = DataStatusCodeTexts.NORMAL;
                                    instance.CrtUser = this.GetLogonUser().UserId;
                                    instance.CrtDate = DateTime.Now;
                                    #endregion

                                    xmlResult = DataProxy.Current.Insert<IdentifyList3Entity>(this, instance, out count);
                                }
                                break;
                            case "4":
                                {
                                    #region 補齊資料
                                    IdentifyList4Entity instance = data as IdentifyList4Entity;
                                    instance.Status = DataStatusCodeTexts.NORMAL;
                                    instance.CrtUser = this.GetLogonUser().UserId;
                                    instance.CrtDate = DateTime.Now;
                                    #endregion

                                    xmlResult = DataProxy.Current.Insert<IdentifyList4Entity>(this, instance, out count);
                                }
                                break;
                            case "5":
                                {
                                    #region 補齊資料
                                    IdentifyList5Entity instance = data as IdentifyList5Entity;
                                    instance.Status = DataStatusCodeTexts.NORMAL;
                                    instance.CrtUser = this.GetLogonUser().UserId;
                                    instance.CrtDate = DateTime.Now;
                                    #endregion

                                    xmlResult = DataProxy.Current.Insert<IdentifyList5Entity>(this, instance, out count);
                                }
                                break;
                            case "6":
                                {
                                    #region 補齊資料
                                    IdentifyList6Entity instance = data as IdentifyList6Entity;
                                    instance.Status = DataStatusCodeTexts.NORMAL;
                                    instance.CrtUser = this.GetLogonUser().UserId;
                                    instance.CrtDate = DateTime.Now;
                                    #endregion

                                    xmlResult = DataProxy.Current.Insert<IdentifyList6Entity>(this, instance, out count);
                                }
                                break;
                        }

                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, string.Empty);

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
                        #region [MDY:20210713] FIX BUG
                        int identifyType = int.Parse(this.EditIdentifyType);
                        #endregion

                        #region Where 條件
                        string[] receiveTypeFields = new string[] {
                            IdentifyList1Entity.Field.ReceiveType, IdentifyList2Entity.Field.ReceiveType, IdentifyList3Entity.Field.ReceiveType, 
                            IdentifyList4Entity.Field.ReceiveType, IdentifyList5Entity.Field.ReceiveType, IdentifyList6Entity.Field.ReceiveType };
                        string[] yearIdFields = new string[] { 
                            IdentifyList1Entity.Field.YearId, IdentifyList2Entity.Field.YearId, IdentifyList3Entity.Field.YearId, 
                            IdentifyList4Entity.Field.YearId, IdentifyList5Entity.Field.YearId, IdentifyList6Entity.Field.YearId };
                        string[] termIdFields = new string[] { 
                            IdentifyList1Entity.Field.TermId, IdentifyList2Entity.Field.TermId, IdentifyList3Entity.Field.TermId, 
                            IdentifyList4Entity.Field.TermId, IdentifyList5Entity.Field.TermId, IdentifyList6Entity.Field.TermId };
                        string[] depIdFields = new string[] { 
                            IdentifyList1Entity.Field.DepId, IdentifyList2Entity.Field.DepId, IdentifyList3Entity.Field.DepId, 
                            IdentifyList4Entity.Field.DepId, IdentifyList5Entity.Field.DepId, IdentifyList6Entity.Field.DepId };
                        string[] identifyIdFields = new string[] { 
                                IdentifyList1Entity.Field.IdentifyId, IdentifyList2Entity.Field.IdentifyId, IdentifyList3Entity.Field.IdentifyId, 
                                IdentifyList4Entity.Field.IdentifyId, IdentifyList5Entity.Field.IdentifyId, IdentifyList6Entity.Field.IdentifyId };

                        Expression where = new Expression(receiveTypeFields[identifyType - 1], this.EditReceiveType)
                            .And(yearIdFields[identifyType - 1], this.EditYearId)
                            .And(termIdFields[identifyType - 1], this.EditTermId)
                            .And(depIdFields[identifyType - 1], this.EditDepId)
                            .And(identifyIdFields[identifyType - 1], this.EditIdentifyId);
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = null;

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        switch (this.EditIdentifyType)
                        {
                            case "1":
                                {
                                    IdentifyList1Entity instance = data as IdentifyList1Entity;
                                    fieldValues.Add(IdentifyList1Entity.Field.MdyUser, this.GetLogonUser().UserId);
                                    fieldValues.Add(IdentifyList1Entity.Field.MdyDate, DateTime.Now);
                                    fieldValues.Add(IdentifyList1Entity.Field.IdentifyName, instance.IdentifyName);

                                    #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                                    fieldValues.Add(IdentifyList1Entity.Field.IdentifyEName, instance.IdentifyEName);
                                    #endregion

                                    xmlResult = DataProxy.Current.UpdateFields<IdentifyList1Entity>(this, where, fieldValues, out count);
                                }
                                break;
                            case "2":
                                {
                                    IdentifyList2Entity instance = data as IdentifyList2Entity;
                                    fieldValues.Add(IdentifyList2Entity.Field.MdyUser, this.GetLogonUser().UserId);
                                    fieldValues.Add(IdentifyList2Entity.Field.MdyDate, DateTime.Now);
                                    fieldValues.Add(IdentifyList2Entity.Field.IdentifyName, instance.IdentifyName);

                                    #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                                    fieldValues.Add(IdentifyList2Entity.Field.IdentifyEName, instance.IdentifyEName);
                                    #endregion

                                    xmlResult = DataProxy.Current.UpdateFields<IdentifyList2Entity>(this, where, fieldValues, out count);
                                }
                                break;
                            case "3":
                                {
                                    IdentifyList3Entity instance = data as IdentifyList3Entity;
                                    fieldValues.Add(IdentifyList3Entity.Field.MdyUser, this.GetLogonUser().UserId);
                                    fieldValues.Add(IdentifyList3Entity.Field.MdyDate, DateTime.Now);
                                    fieldValues.Add(IdentifyList3Entity.Field.IdentifyName, instance.IdentifyName);

                                    #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                                    fieldValues.Add(IdentifyList3Entity.Field.IdentifyEName, instance.IdentifyEName);
                                    #endregion

                                    xmlResult = DataProxy.Current.UpdateFields<IdentifyList3Entity>(this, where, fieldValues, out count);
                                }
                                break;
                            case "4":
                                {
                                    IdentifyList4Entity instance = data as IdentifyList4Entity;
                                    fieldValues.Add(IdentifyList4Entity.Field.MdyUser, this.GetLogonUser().UserId);
                                    fieldValues.Add(IdentifyList4Entity.Field.MdyDate, DateTime.Now);
                                    fieldValues.Add(IdentifyList4Entity.Field.IdentifyName, instance.IdentifyName);

                                    #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                                    fieldValues.Add(IdentifyList4Entity.Field.IdentifyEName, instance.IdentifyEName);
                                    #endregion

                                    xmlResult = DataProxy.Current.UpdateFields<IdentifyList4Entity>(this, where, fieldValues, out count);
                                }
                                break;
                            case "5":
                                {
                                    IdentifyList5Entity instance = data as IdentifyList5Entity;
                                    fieldValues.Add(IdentifyList5Entity.Field.MdyUser, this.GetLogonUser().UserId);
                                    fieldValues.Add(IdentifyList5Entity.Field.MdyDate, DateTime.Now);
                                    fieldValues.Add(IdentifyList5Entity.Field.IdentifyName, instance.IdentifyName);

                                    #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                                    fieldValues.Add(IdentifyList5Entity.Field.IdentifyEName, instance.IdentifyEName);
                                    #endregion

                                    xmlResult = DataProxy.Current.UpdateFields<IdentifyList5Entity>(this, where, fieldValues, out count);
                                }
                                break;
                            case "6":
                                {
                                    IdentifyList6Entity instance = data as IdentifyList6Entity;
                                    fieldValues.Add(IdentifyList6Entity.Field.MdyUser, this.GetLogonUser().UserId);
                                    fieldValues.Add(IdentifyList6Entity.Field.MdyDate, DateTime.Now);
                                    fieldValues.Add(IdentifyList6Entity.Field.IdentifyName, instance.IdentifyName);

                                    #region [MDY:202203XX] 2022擴充案 身分註記英文名稱
                                    fieldValues.Add(IdentifyList6Entity.Field.IdentifyEName, instance.IdentifyEName);
                                    #endregion

                                    xmlResult = DataProxy.Current.UpdateFields<IdentifyList6Entity>(this, where, fieldValues, out count);
                                }
                                break;
                        }
                        #endregion

                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, string.Empty);

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
                        XmlResult xmlResult = null;
                        switch (this.EditIdentifyType)
                        {
                            case "1":
                                {
                                    IdentifyList1Entity instance = data as IdentifyList1Entity;
                                    xmlResult = DataProxy.Current.Delete<IdentifyList1Entity>(this, instance, out count);
                                }
                                break;
                            case "2":
                                {
                                    IdentifyList2Entity instance = data as IdentifyList2Entity;
                                    xmlResult = DataProxy.Current.Delete<IdentifyList2Entity>(this, instance, out count);
                                }
                                break;
                            case "3":
                                {
                                    IdentifyList3Entity instance = data as IdentifyList3Entity;
                                    xmlResult = DataProxy.Current.Delete<IdentifyList3Entity>(this, instance, out count);
                                }
                                break;
                            case "4":
                                {
                                    IdentifyList4Entity instance = data as IdentifyList4Entity;
                                    xmlResult = DataProxy.Current.Delete<IdentifyList4Entity>(this, instance, out count);
                                }
                                break;
                            case "5":
                                {
                                    IdentifyList5Entity instance = data as IdentifyList5Entity;
                                    xmlResult = DataProxy.Current.Delete<IdentifyList5Entity>(this, instance, out count);
                                }
                                break;
                            case "6":
                                {
                                    IdentifyList6Entity instance = data as IdentifyList6Entity;
                                    xmlResult = DataProxy.Current.Delete<IdentifyList6Entity>(this, instance, out count);
                                }
                                break;
                        } 

                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, string.Empty);

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

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Server.Transfer("D1100009.aspx?IdentifyType=" + this.EditIdentifyType);
        }
    }
}