using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 商家代號費用小記定義檔
    /// </summary>
    public partial class D1200002M : BasePage
    {
        #region [MDY:202203XX] 2022擴充案 改寫所以 MARK
        //#region Readonly
        //public readonly string[] _SumIds = new string[] { 
        //    "Other_01", "Other_02", "Other_03", "Other_04", "Other_05", "Other_06", "Other_07", "Other_08", "Other_09", "Other_10",
        //    "Other_11", "Other_12", "Other_13", "Other_14", "Other_15", "Other_16", "Other_17", "Other_18", "Other_19", "Other_20",
        //    "Other_21"
        //};
        //#endregion
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
        /// 編輯的合計項目代號參數
        /// </summary>
        private string EditSumId
        {
            get
            {
                return ViewState["EditSumId"] as string;
            }
            set
            {
                ViewState["EditSumId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的代收費用檔
        /// </summary>
        private ReceiveSumEntity EditReceiveSum
        {
            get
            {
                return ViewState["EditReceiveSum"] as ReceiveSumEntity;
            }
            set
            {
                ViewState["EditReceiveSum"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ccbtnOK.Visible = false;

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
                    return;
                }

                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.EditYearId = QueryString.TryGetValue("YearId", String.Empty);
                this.EditTermId = QueryString.TryGetValue("TermId", String.Empty);
                this.EditDepId = QueryString.TryGetValue("DepId", String.Empty);
                this.EditReceiveId = QueryString.TryGetValue("ReceiveId", String.Empty);
                this.EditSumId = QueryString.TryGetValue("SumId", String.Empty);

                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                    //|| String.IsNullOrEmpty(this.EditDepId)
                    || String.IsNullOrEmpty(this.EditReceiveId))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }

                XmlResult xmlResult = ucFilter1.GetDataAndBind(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                    return;
                }
                #endregion

                #region 檢查業務別碼授權
                if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                    return;
                }
                #endregion

                this.InitialUI();

                #region 取得查詢資料
                ReceiveSumEntity data = null;
                bool isOK = this.GetQueryData(out data);
                #endregion

                this.BindEditData(data);

                this.ccbtnOK.Visible = isOK;
            }
        }

        /// <summary>
        /// 結繫查詢的資料
        /// </summary>
        private void BindEditData(ReceiveSumEntity data)
        {
            if (this.Action != ActionMode.Insert && data == null)
            {
                return;
            }

            this.EditReceiveSum = data;

            #region [MDY:20160104] 小計代碼改為下拉選項
            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.ddlSumId.Enabled = true;
                    this.tbxSumName.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.ddlSumId.Enabled = false;
                    this.tbxSumName.Enabled = true;
                    break;
                default:
                    this.ddlSumId.Enabled = false;
                    this.tbxSumName.Enabled = false;
                    break;
            }
            #endregion

            CheckBox[] checkBoxs = this.GetReceiveCheckBoxs();

            #region  收入科目項目
            SchoolRidEntity schoolRid;
            if (GetSchoolRid(out schoolRid))
            {
                #region [MDY:202203XX] 2022擴充案 改寫
                #region [OLD]
                //for (int i = 1; i < 41; i++)
                //{
                //    Result rt = new Result();
                //    string ReceiveItemName = "Receive_Item" + i.ToString("00");
                //    object receiveitem = schoolRid.GetValue(ReceiveItemName, out rt);
                //    if (receiveitem != null)
                //    {
                //        ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
                //        CheckBox ck = (CheckBox)cph.FindControl("cbxReceiveItem" + i.ToString("00"));
                //        if (ck != null)
                //        {
                //            ck.Text = receiveitem.ToString();
                //        }
                //    }
                //}
                #endregion

                int idx = 0;
                string[] receiveItems = schoolRid.GetAllReceiveItemChts();
                foreach (CheckBox checkBox in checkBoxs)
                {
                    checkBox.Text = receiveItems[idx];
                    idx++;
                }
                #endregion
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 小計英文名稱
            this.cclabSumEName.Visible = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
            this.tbxSumEName.Visible = this.cclabSumEName.Visible;
            this.tbxSumEName.Enabled = this.tbxSumName.Enabled;
            #endregion

            if (this.Action != ActionMode.Insert)
            {
                if (data == null)
                {
                    this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "查無設定資料");
                    return;
                }

                #region [MDY:20160104] 小計代碼改為下拉選項
                WebHelper.SetDropDownListSelectedValue(this.ddlSumId, data.SumId.Trim());
                #endregion

                tbxSumName.Text = data.SumName.Trim();

                #region [MDY:202203XX] 2022擴充案 小計英文名稱
                this.tbxSumEName.Text = data.SumEName;
                #endregion

                #region [MDY:202203XX] 2022擴充案 改寫
                #region [OLD]
                //for (int i = 1; i < 41; i++)
                //{
                //    Result rt = new Result();
                //    string fieldName = "Receive_" + i.ToString("00");
                //    object obS = data.GetValue(fieldName, out rt);
                //    if (obS != null)
                //    {
                //        ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
                //        CheckBox ck = (CheckBox)cph.FindControl("cbxReceiveItem" + i.ToString("00"));
                //        if (ck != null)
                //        {
                //            ck.Checked = obS.ToString().Trim() == "Y" ? true : false;
                //            if (this.Action == ActionMode.Delete)
                //            {
                //                ck.Enabled = false;
                //            }
                //            else
                //            {
                //                ck.Enabled = true;
                //            }
                //        }
                //    }
                //}
                #endregion

                int idx = 0;
                string[] receives = data.GetAllReceives();
                foreach (CheckBox checkBox in checkBoxs)
                {
                    checkBox.Checked = "Y".Equals(receives[idx]);
                    checkBox.Enabled = this.tbxSumName.Enabled;
                    idx++;
                }
                #endregion
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            #region [MDY:20160104] 小計代碼改為下拉選項
            {
                #region [MDY:202203XX] 2022擴充案 改寫
                #region [OLD]
                //CodeTextList items = new CodeTextList(_SumIds.Length);
                //foreach(string sumId in _SumIds)
                //{
                //    items.Add(sumId, sumId);
                //}
                #endregion

                CodeTextList items = new CodeTextList(21);
                for (int no = 1; no <= 21; no++)
                {
                    string value = $"Other_{no:00}";
                    items.Add(value, value);
                }
                #endregion

                WebHelper.SetDropDownListItems(this.ddlSumId, DefaultItem.Kind.Select, false, items, false, false, 0, null);
            }
            #endregion

            this.tbxSumName.Text = string.Empty;

            #region [MDY:202203XX] 2022擴充案 小計英文名稱
            this.cclabSumEName.Visible = false;
            this.tbxSumEName.Visible = false;
            this.tbxSumEName.Text = String.Empty;
            #endregion
        }

        CheckBox[] _ReceiveCheckBoxs = null;
        private CheckBox[] GetReceiveCheckBoxs()
        {
            if (_ReceiveCheckBoxs == null)
            {
                _ReceiveCheckBoxs = new CheckBox[40]
                {
                    this.cbxReceiveItem01, this.cbxReceiveItem02, this.cbxReceiveItem03, this.cbxReceiveItem04, this.cbxReceiveItem05,
                    this.cbxReceiveItem06, this.cbxReceiveItem07, this.cbxReceiveItem08, this.cbxReceiveItem09, this.cbxReceiveItem10,
                    this.cbxReceiveItem11, this.cbxReceiveItem12, this.cbxReceiveItem13, this.cbxReceiveItem14, this.cbxReceiveItem15,
                    this.cbxReceiveItem16, this.cbxReceiveItem17, this.cbxReceiveItem18, this.cbxReceiveItem19, this.cbxReceiveItem20,
                    this.cbxReceiveItem21, this.cbxReceiveItem22, this.cbxReceiveItem23, this.cbxReceiveItem24, this.cbxReceiveItem25,
                    this.cbxReceiveItem26, this.cbxReceiveItem27, this.cbxReceiveItem28, this.cbxReceiveItem29, this.cbxReceiveItem30,
                    this.cbxReceiveItem31, this.cbxReceiveItem32, this.cbxReceiveItem33, this.cbxReceiveItem34, this.cbxReceiveItem35,
                    this.cbxReceiveItem36, this.cbxReceiveItem37, this.cbxReceiveItem38, this.cbxReceiveItem39, this.cbxReceiveItem40,
                };
            }
            return _ReceiveCheckBoxs;
        }

        protected bool GetSchoolRid(out SchoolRidEntity schoolRid)
        {
            schoolRid = null;
            string action = this.GetLocalized("查詢要維護的資料");

            Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, this.EditReceiveType)
                .And(SchoolRidEntity.Field.YearId, this.EditYearId)
                .And(SchoolRidEntity.Field.TermId, this.EditTermId)
                .And(SchoolRidEntity.Field.DepId, this.EditDepId)
                .And(SchoolRidEntity.Field.ReceiveId, this.EditReceiveId);
            XmlResult result = DataProxy.Current.SelectFirst<SchoolRidEntity>(this, where, null, out schoolRid);
            if (!result.IsSuccess)
            {
                this.ShowActionFailureMessage(action, result.Code, result.Message);
                return false;
            }
            if (schoolRid == null)
            {
                this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "無該代收費用設定資料");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 取得查詢的資料
        /// </summary>
        private bool GetQueryData(out ReceiveSumEntity data)
        {
            data = null;

            string action = this.GetLocalized("查詢要維護的資料");

            #region ReceiveSumEntity
            {
                Expression where = new Expression(ReceiveSumEntity.Field.ReceiveType, this.EditReceiveType)
                    .And(ReceiveSumEntity.Field.YearId, this.EditYearId)
                    .And(ReceiveSumEntity.Field.TermId, this.EditTermId)
                    .And(ReceiveSumEntity.Field.DepId, this.EditDepId)
                    .And(ReceiveSumEntity.Field.ReceiveId, this.EditReceiveId)
                    .And(ReceiveSumEntity.Field.SumId, this.EditSumId);
                XmlResult result = DataProxy.Current.SelectFirst<ReceiveSumEntity>(this, where, null, out data);
                if (!result.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, result.Code, result.Message);
                    return false;
                }
                if (this.Action != ActionMode.Insert && data == null)
                {
                    this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "無該合計項目設定資料");
                    return false;
                }
            }
            #endregion

            return true;
        }
        
        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private ReceiveSumEntity GetEditData()
        {
            ReceiveSumEntity data = new ReceiveSumEntity();
            switch(this.Action)
            {
                case ActionMode.Insert:
                    data.ReceiveType = this.EditReceiveType;
                    data.YearId = this.EditYearId;
                    data.TermId = this.EditTermId;
                    data.DepId = this.EditDepId;
                    data.ReceiveId = this.EditReceiveId;

                    #region [MDY:20160104] 小計代碼改為下拉選項
                    data.SumId = this.ddlSumId.SelectedValue;
                    #endregion
                    break;
                case ActionMode.Modify:
                case ActionMode.Delete:
                    data = this.EditReceiveSum;

                    data.SumId = this.EditSumId;
                    break;
            }

            data.SumName = tbxSumName.Text.Trim();

            #region [MDY:202203XX] 2022擴充案 小計英文名稱
            if (this.tbxSumEName.Visible)
            {
                data.SumEName = this.tbxSumEName.Text.Trim();
            }
            else
            {
                data.SumEName = null;
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 改寫
            #region [OLD]
            //ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            //for (int i = 1; i < 41; i++)
            //{
            //    string fieldName = "Receive_" + i.ToString("00");
            //    CheckBox ck = (CheckBox)cph.FindControl("cbxReceiveItem" + i.ToString("00"));
            //    if (ck != null)
            //    {
            //        data.SetValue(fieldName, ck.Checked ? "Y" : "N");
            //    }
            //}
            #endregion

            CheckBox[] checkBoxs = this.GetReceiveCheckBoxs();
            int no = 0;
            foreach (CheckBox checkBox in checkBoxs)
            {
                no++;
                data.SetReceiveByNo(no, checkBox.Checked ? "Y" : "N");
            }
            #endregion

            return data;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData()
        {
            #region [MDY:20160104] 小計代碼改為下拉選項
            string sumId = null;
            switch(this.Action)
            {
                case ActionMode.Insert:
                    sumId = this.ddlSumId.SelectedValue;
                    break;
                default:
                    sumId = this.EditSumId;
                    break;
            }
            if (String.IsNullOrEmpty(sumId))
            {
                this.ShowMustInputAlert("小計代碼");
                return false;
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 修正非刪除才要檢查
            if (this.Action != ActionMode.Delete)
            {
                if (String.IsNullOrWhiteSpace(tbxSumName.Text))
                {
                    this.ShowMustInputAlert("小計中文名稱");
                    return false;
                }

                #region [MDY:202203XX] 2022擴充案 小計英文名稱
                if (this.tbxSumEName.Visible && String.IsNullOrWhiteSpace(this.tbxSumEName.Text))
                {
                    this.ShowMustInputAlert("小計英文名稱");
                    return false;
                }
                #endregion

                #region [MDY:202203XX] 改寫
                #region [OLD]
                //bool isOk = false;
                //ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
                //for (int i = 1; i < 41; i++)
                //{
                //    CheckBox ck = (CheckBox)cph.FindControl("cbxReceiveItem" + i.ToString("00"));
                //    if (ck != null)
                //    {
                //        if (ck.Checked)
                //        {
                //            isOk = true;
                //        }
                //    }
                //}
                //if (!isOk)
                //{

                //    this.ShowSystemMessage("至少勾選一收入科目");
                //    return false;
                //}
                #endregion

                CheckBox[] checkBoxs = this.GetReceiveCheckBoxs();
                if (checkBoxs.FirstOrDefault(x => x.Checked == true) == null)
                {
                    this.ShowSystemMessage("至少勾選一收入科目");
                    return false;
                }
                #endregion
            }
            #endregion

            return true;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!this.CheckEditData())
            {
                return;
            }

            ReceiveSumEntity data = this.GetEditData();

            string backUrl = "D1200002.aspx";
            int count = 0;
            XmlResult xmlResult = new XmlResult();

            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    data.Status = DataStatusCodeTexts.NORMAL;
                    data.CrtDate = DateTime.Now;
                    data.CrtUser = this.GetLogonUser().UserId;
                    xmlResult = DataProxy.Current.Insert<ReceiveSumEntity>(this, data, out count);
                    break;
                case ActionMode.Modify:     //修改
                    data.MdyDate = DateTime.Now;
                    data.MdyUser = this.GetLogonUser().UserId;
                    xmlResult = DataProxy.Current.Update<ReceiveSumEntity>(this, data, out count);
                    break;
                case ActionMode.Delete:     //刪除
                    xmlResult = DataProxy.Current.Delete<ReceiveSumEntity>(this, data, out count);
                    break;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            if (xmlResult.IsSuccess)
            {
                if (count < 1)
                {
                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                }
                else
                {
                    WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);
                    this.ShowActionSuccessAlert(action, backUrl);
                }
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }
    }
}