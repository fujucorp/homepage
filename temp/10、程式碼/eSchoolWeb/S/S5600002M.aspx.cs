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
    /// 金融機構代碼 (維護)
    /// </summary>
    public partial class S5600002M : BasePage
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
        /// 編輯的金融機構代碼參數
        /// </summary>
        private string EditBankNo
        {
            get
            {
                return ViewState["EditBankNo"] as string;
            }
            set
            {
                ViewState["EditBankNo"] = value == null ? null : value.Trim();
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
                this.EditBankNo = QueryString.TryGetValue("BankNo", String.Empty);

                if (((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) && String.IsNullOrEmpty(this.EditBankNo)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                BankEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new BankEntity();
                            data.BankNo = this.EditBankNo;
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(BankEntity.Field.BankNo, this.EditBankNo);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<BankEntity>(this, where, null, out data);
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
            this.tbxBankNo.Text = String.Empty;
            this.tbxBankFName.Text = String.Empty;
            this.tbxTel.Text = String.Empty;
            this.tbxFullCode.Text = String.Empty;
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(BankEntity data)
        {
            if (data == null)
            {
                this.tbxBankNo.Text = String.Empty;
                this.tbxBankFName.Text = String.Empty;
                this.tbxTel.Text = String.Empty;
                this.tbxFullCode.Text = String.Empty;
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxBankNo.Enabled = true;
                    this.tbxBankFName.Enabled = true;
                    this.tbxTel.Enabled = true;
                    this.tbxFullCode.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.tbxBankNo.Enabled = false;
                    this.tbxBankFName.Enabled = true;
                    this.tbxTel.Enabled = true;
                    this.tbxFullCode.Enabled = true;
                    break;
                default:
                    this.tbxBankNo.Enabled = false;
                    this.tbxBankFName.Enabled = false;
                    this.tbxTel.Enabled = false;
                    this.tbxFullCode.Enabled = false;
                    break;
            }

            #region [MDY:20210401] 原碼修正
            this.tbxBankNo.Text = HttpUtility.HtmlEncode(data.BankNo);
            #endregion

            this.tbxBankFName.Text = data.BankFName;
            this.tbxTel.Text = data.Tel;
            this.tbxFullCode.Text = data.FullCode;
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData()
        {
            string bankNo = this.tbxBankNo.Text.Trim();
            if (String.IsNullOrEmpty(bankNo))
            {
                this.ShowMustInputAlert("分行代碼");
                return false;
            }

            #region [MDY:20160814] 修正分行代碼判斷 (因為總行類的分行第6碼為英文)
            #region [Old]
            //if (!Common.IsNumber(bankNo, 6) || !bankNo.StartsWith(DataFormat.MyBankID))
            //{
            //    //[TODO] 固定顯示訊息的收集
            //    string msg = this.GetLocalized(String.Format("分行代碼必須是{0}開頭的6碼數字", DataFormat.MyBankID));
            //    this.ShowSystemMessage(msg);
            //    return false;
            //}
            #endregion

            if (!DataFormat.IsMyBankCode(bankNo))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized(String.Format("分行代碼必須是{0}開頭的6碼數字 (總行的第六碼可以為英文)", DataFormat.MyBankID));
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            if (String.IsNullOrEmpty(tbxBankFName.Text.Trim()))
            {
                this.ShowMustInputAlert("分行名稱");
                return false;
            }

            string fullCode = this.tbxFullCode.Text.Trim();
            if (!String.IsNullOrEmpty(fullCode))
            {
                if (fullCode.Length != 7 || !fullCode.StartsWith(bankNo))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("銀行金融代號必須是7碼數字，且前6碼必須與分行代碼相同");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            return true;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!this.CheckEditData())
            {
                return;
            }

            BankEntity data = this.GetEditData();

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600002.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<BankEntity>(this, data, out count);
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
                        Expression where = new Expression(BankEntity.Field.BankNo, data.BankNo);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(BankEntity.Field.BankFName, data.BankFName);
                        if (!String.IsNullOrEmpty(data.BankFName))
                        {
                            fieldValues.Add(BankEntity.Field.BankSName, data.BankSName);
                        }
                        fieldValues.Add(BankEntity.Field.Tel, data.Tel);
                        fieldValues.Add(BankEntity.Field.FullCode, data.FullCode);
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<BankEntity>(this, where, fieldValues, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<BankEntity>(this, data, out count);
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
        private BankEntity GetEditData()
        {
            BankEntity data = new BankEntity();
            data.BankNo = tbxBankNo.Text.Trim();
            data.BankFName = this.tbxBankFName.Text.Trim();
            if (data.BankFName.Length < 36)
            {
                data.BankSName = data.BankFName.Replace("台灣土地銀行", "");
            }
            data.Tel = this.tbxTel.Text.Trim();
            data.FullCode = this.tbxFullCode.Text.Trim();
            return data;
        }
    }
}