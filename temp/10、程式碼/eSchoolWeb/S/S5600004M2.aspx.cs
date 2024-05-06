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
    /// 代收管道管理 - 代收代號(維護)
    /// </summary>
    public partial class S5600004M2 : BasePage
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

        /// <summary>
        /// 編輯的手序費代碼
        /// </summary>
        private string EditBarcodeId
        {
            get
            {
                return ViewState["EditBarcodeId"] as string;
            }
            set
            {
                ViewState["EditBarcodeId"] = value == null ? null : value.Trim();
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
                this.EditBarcodeId = QueryString.TryGetValue("BarcodeId", String.Empty);

                if ((String.IsNullOrEmpty(this.EditChannelId) || 
                    (this.Action == ActionMode.Modify || this.Action == ActionMode.Delete)
                    && String.IsNullOrEmpty(this.EditBarcodeId)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                ChannelWayEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new ChannelWayEntity();
                            data.ChannelId = this.EditChannelId;
                            data.BarcodeId = this.EditBarcodeId;
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(ChannelWayEntity.Field.ChannelId, this.EditChannelId);
                            where.And(ChannelWayEntity.Field.BarcodeId, this.EditBarcodeId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<ChannelWayEntity>(this, where, null, out data);
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
            this.tbxBarcodeId.Text = String.Empty;
            this.tbxChannelCharge.Text = String.Empty;
            this.tbxRCSPay.Text = String.Empty;
            this.tbxMinMoney.Text = String.Empty;
            this.tbxMaxMoney.Text = String.Empty;
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(ChannelWayEntity data)
        {
            if (data == null)
            {
                this.tbxBarcodeId.Text = String.Empty;
                this.tbxChannelCharge.Text = String.Empty;
                this.tbxRCSPay.Text = String.Empty;
                this.tbxMinMoney.Text = String.Empty;
                this.tbxMaxMoney.Text = String.Empty;
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.tbxBarcodeId.Enabled = true;
                    this.tbxChannelCharge.Enabled = true;
                    this.tbxRCSPay.Enabled = true;
                    this.tbxMinMoney.Enabled = true;
                    this.tbxMaxMoney.Enabled = true;
                    rdoIncludePay.Enabled = true;
                    rdoRCFlag.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.tbxBarcodeId.Enabled = false;
                    this.tbxChannelCharge.Enabled = true;
                    this.tbxRCSPay.Enabled = true;
                    this.tbxMinMoney.Enabled = true;
                    this.tbxMaxMoney.Enabled = true;
                    rdoIncludePay.Enabled = true;
                    rdoRCFlag.Enabled = true;
                    WebHelper.SetRadioButtonListSelectedValue(rdoIncludePay, data.IncludePay);
                    WebHelper.SetRadioButtonListSelectedValue(rdoRCFlag, data.RCFlag);
                    break;
                default:
                    this.tbxBarcodeId.Enabled = false;
                    this.tbxChannelCharge.Enabled = false;
                    this.tbxRCSPay.Enabled = false;
                    this.tbxMinMoney.Enabled = false;
                    this.tbxMaxMoney.Enabled = false;
                    rdoIncludePay.Enabled = false;
                    rdoRCFlag.Enabled = false;
                    WebHelper.SetRadioButtonListSelectedValue(rdoIncludePay, data.IncludePay);
                    WebHelper.SetRadioButtonListSelectedValue(rdoRCFlag, data.RCFlag);
                    break;
            }

            //[MDY:20200309] CHECKMARX Reflected XSS Specific Clients Revision
            this.tbxBarcodeId.Text = HttpUtility.HtmlEncode(data.BarcodeId);
            this.tbxChannelCharge.Text = DataFormat.GetAmountText(data.ChannelCharge);
            this.tbxRCSPay.Text = DataFormat.GetAmountText(data.RCSPay);
            this.tbxMinMoney.Text = DataFormat.GetAmountText(data.MinMoney);
            this.tbxMaxMoney.Text = DataFormat.GetAmountText(data.MaxMoney);

            this.ccbtnOK.Visible = true;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!this.CheckEditData())
            {
                return;
            }

            ChannelWayEntity data = this.GetEditData();

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600004M1.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<ChannelWayEntity>(this, data, out count);
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
                        Expression where = new Expression(ChannelWayEntity.Field.ChannelId, data.ChannelId);
                        where.And(ChannelWayEntity.Field.BarcodeId, data.BarcodeId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(ChannelWayEntity.Field.ChannelCharge, data.ChannelCharge);
                        fieldValues.Add(ChannelWayEntity.Field.IncludePay, data.IncludePay);
                        fieldValues.Add(ChannelWayEntity.Field.RCFlag, data.RCFlag);
                        fieldValues.Add(ChannelWayEntity.Field.RCSPay, data.RCSPay);
                        fieldValues.Add(ChannelWayEntity.Field.MinMoney, data.MinMoney);
                        fieldValues.Add(ChannelWayEntity.Field.MaxMoney, data.MaxMoney);
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<ChannelWayEntity>(this, where, fieldValues, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<ChannelWayEntity>(this, data, out count);
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
        private ChannelWayEntity GetEditData()
        {
            ChannelWayEntity data = new ChannelWayEntity();

            switch (this.Action)
            {
                case ActionMode.Insert:
                    data.ChannelId = this.EditChannelId;
                    data.BarcodeId = this.tbxBarcodeId.Text.Trim();
                    data.RCBPay = 0;
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    data.ChannelId = this.EditChannelId;
                    data.BarcodeId = this.EditBarcodeId;
                    break;
            }

            decimal money = 0;
            data.IncludePay = rdoIncludePay.SelectedValue;
            data.RCFlag = rdoRCFlag.SelectedValue;

            decimal.TryParse(tbxChannelCharge.Text, out money);
            data.ChannelCharge = money;
            decimal.TryParse(tbxRCSPay.Text, out money);
            data.RCSPay = money;
            decimal.TryParse(tbxMinMoney.Text, out money);
            data.MinMoney = money;
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
            if (String.IsNullOrEmpty(tbxBarcodeId.Text))
            {
                this.ShowMustInputAlert("代收代碼");
                return false;
            }

            if (!Common.IsMoney(tbxChannelCharge.Text))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「預設手續費」不是合法的金額");
                this.ShowSystemMessage(msg);
                return false;
            }

            if (!Common.IsMoney(tbxRCSPay.Text))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「預設繳款人/學校手續費」不是合法的金額");
                this.ShowSystemMessage(msg);
                return false;
            }

            if (!Common.IsMoney(tbxMinMoney.Text))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「預設繳費下限」不是合法的金額");
                this.ShowSystemMessage(msg);
                return false;
            }

            if (!Common.IsMoney(tbxMaxMoney.Text))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「預設繳費上限」不是合法的金額");
                this.ShowSystemMessage(msg);
                return false;
            }
            return true;
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Server.Transfer("S5600004M1.aspx");
        }

    }
}