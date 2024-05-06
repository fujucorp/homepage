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
    /// 手續費管理(維護)
    /// </summary>
    public partial class S5600011M : BasePage
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
        /// 編輯的代收類別
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
        /// 編輯的代收類別名稱
        /// </summary>
        private string EditReceiveTypeName
        {
            get
            {
                return ViewState["EditReceiveTypeName"] as string;
            }
            set
            {
                ViewState["EditReceiveTypeName"] = value == null ? null : value.Trim();
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

        /// <summary>
        /// 編輯的管道代碼
        /// </summary>
        private ReceiveChannelEntity EditReceiveChannelEntity
        {
            get
            {
                return ViewState["EditReceiveChannelEntity"] as ReceiveChannelEntity;
            }
            set
            {
                ViewState["EditReceiveChannelEntity"] = value == null ? null : value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
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
                this.EditReceiveTypeName = QueryString.TryGetValue("ReceiveTypeName", String.Empty);
                this.EditChannelId = QueryString.TryGetValue("ChannelId", String.Empty);
                this.EditBarcodeId = QueryString.TryGetValue("BarcodeId", String.Empty);

                if (((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete)
                    && (String.IsNullOrEmpty(this.EditChannelId) 
                    || String.IsNullOrEmpty(this.EditBarcodeId) 
                    || String.IsNullOrEmpty(this.EditReceiveType))))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.InitialUI();

                #region 取得維護資料
                ReceiveChannelEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new ReceiveChannelEntity();
                            data.ReceiveType = this.EditReceiveType;
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(ReceiveChannelEntity.Field.ChannelId, this.EditChannelId);
                            where.And(ReceiveChannelEntity.Field.BarcodeId, this.EditBarcodeId);
                            where.And(ReceiveChannelEntity.Field.ReceiveType, this.EditReceiveType);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<ReceiveChannelEntity>(this, where, null, out data);
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
                            this.EditReceiveChannelEntity = data;
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
            this.tbxChannelCharge.Text = String.Empty;
            this.tbxRCSPay.Text = String.Empty;
            this.tbxRCBPay.Text = String.Empty;
            this.tbxMinMoney.Text = String.Empty;
            this.tbxMaxMoney.Text = String.Empty;
            this.labReceiveTypeName.Text = HttpUtility.HtmlEncode(this.EditReceiveTypeName);

            this.GetAndBindChannelIdOptions();

            this.GetAndBindBarcodeIdOptions();

            this.ccbtnOK.Visible = true;
        }

        #region [MDY:20190608] 超商行動版管道檢核
        /// <summary>
        /// 檢核是否有相同的級距、手續費
        /// </summary>
        private bool CheckSMMobile(ReceiveChannelEntity data)
        {
            int count = 0;
            Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, data.ReceiveType)
                .And(ReceiveChannelEntity.Field.ChannelId, ChannelHelper.SM_DEFAULT)
                .And(ReceiveChannelEntity.Field.ChannelCharge, data.ChannelCharge)
                .And(ReceiveChannelEntity.Field.MaxMoney, data.MaxMoney)
                .And(ReceiveChannelEntity.Field.MinMoney, data.MinMoney);
            XmlResult xmlResult = DataProxy.Current.SelectCount<ReceiveChannelEntity>(this.Page, where, out count);
            if (xmlResult.IsSuccess && count == 0)
            {
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(ReceiveChannelEntity data)
        {
            if (data == null)
            {
                this.tbxChannelCharge.Text = String.Empty;
                this.tbxRCSPay.Text = String.Empty;
                this.tbxRCBPay.Text = String.Empty;
                this.tbxMinMoney.Text = String.Empty;
                this.tbxMaxMoney.Text = String.Empty;
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.ddlChannelId.Enabled = true;
                    this.ddlBarcodeId.Enabled = true;
                    this.tbxChannelCharge.Enabled = true;
                    this.tbxMinMoney.Enabled = true;
                    this.tbxMaxMoney.Enabled = true;
                    this.tbxRCSPay.Enabled = true;
                    this.tbxRCBPay.Enabled = true;
                    rdoRCFlag.Enabled = true;
                    break;
                case ActionMode.Modify:
                    this.ddlChannelId.Enabled = false;
                    this.ddlBarcodeId.Enabled = false;
                    this.tbxChannelCharge.Enabled = true;
                    this.tbxMinMoney.Enabled = true;
                    this.tbxMaxMoney.Enabled = true;
                    this.tbxRCSPay.Enabled = true;
                    this.tbxRCBPay.Enabled = true;
                    rdoRCFlag.Enabled = true;
                    WebHelper.SetDropDownListSelectedValue(ddlChannelId, data.ChannelId);
                    this.GetAndBindBarcodeIdOptions();
                    WebHelper.SetDropDownListSelectedValue(ddlBarcodeId, data.BarcodeId);
                    WebHelper.SetRadioButtonListSelectedValue(rdoRCFlag, data.RCFlag);
                    break;
                default:
                    this.ddlChannelId.Enabled = false;
                    this.ddlBarcodeId.Enabled = false;
                    this.tbxChannelCharge.Enabled = false;
                    this.tbxMinMoney.Enabled = false;
                    this.tbxMaxMoney.Enabled = false;
                    this.tbxRCSPay.Enabled = false;
                    this.tbxRCBPay.Enabled = false;
                    rdoRCFlag.Enabled = false;
                    WebHelper.SetDropDownListSelectedValue(ddlChannelId, data.ChannelId);
                    this.GetAndBindBarcodeIdOptions();
                    WebHelper.SetDropDownListSelectedValue(ddlBarcodeId, data.BarcodeId);
                    WebHelper.SetRadioButtonListSelectedValue(rdoRCFlag, data.RCFlag);
                    break;
            }

            this.tbxChannelCharge.Text = DataFormat.GetAmountText(data.ChannelCharge);
            this.tbxMinMoney.Text = DataFormat.GetAmountText(data.MinMoney);
            this.tbxMaxMoney.Text = DataFormat.GetAmountText(data.MaxMoney);
            this.tbxRCSPay.Text = DataFormat.GetAmountText(data.RCSPay);
            this.tbxRCBPay.Text = DataFormat.GetAmountText(data.RCBPay);

            this.ccbtnOK.Visible = true;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!this.CheckEditData())
            {
                return;
            }

            ReceiveChannelEntity data = this.GetEditData();

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600011.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<ReceiveChannelEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                            }
                            else
                            {
                                #region [MDY:20190608] 超商行動版管道檢核
                                if (data.ChannelId == ChannelHelper.SM_MOBILE && !this.CheckSMMobile(data))
                                {
                                    this.ShowJsAlertAndGoUrl("新增成功。注意無相同級距與手續費的超商(手續費)設定。", backUrl);
                                }
                                #endregion

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
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Update<ReceiveChannelEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                #region [MDY:20190608] 超商行動版管道檢核
                                if (data.ChannelId == ChannelHelper.SM_MOBILE && !this.CheckSMMobile(data))
                                {
                                    this.ShowJsAlertAndGoUrl("修改成功。注意無相同級距與手續費的超商(手續費)設定。", backUrl);
                                }
                                #endregion

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
                        XmlResult xmlResult = DataProxy.Current.Delete<ReceiveChannelEntity>(this, data, out count);
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
        private ReceiveChannelEntity GetEditData()
        {
            ReceiveChannelEntity data = new ReceiveChannelEntity();

            switch (this.Action)
            {
                case ActionMode.Insert:
                    data.ReceiveType = this.EditReceiveType;
                    data.ChannelId = ddlChannelId.SelectedValue;
                    data.BarcodeId = ddlBarcodeId.SelectedValue;
                    data.IncludePay = string.Empty;
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    data = this.EditReceiveChannelEntity;
                    //data.ReceiveType = this.EditReceiveType;
                    //data.ChannelId = this.EditChannelId;
                    //data.BarcodeId = this.EditBarcodeId;
                    break;
            }

            decimal money = 0;
            data.RCFlag = rdoRCFlag.SelectedValue;

            decimal.TryParse(tbxChannelCharge.Text, out money);
            data.ChannelCharge = money;
            decimal.TryParse(tbxRCSPay.Text, out money);
            data.RCSPay = money;
            decimal.TryParse(tbxRCBPay.Text, out money);
            data.RCBPay = money;
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
            if (String.IsNullOrEmpty(ddlBarcodeId.SelectedValue))
            {
                this.ShowMustInputAlert("繳款方式");
                return false;
            }

            if (String.IsNullOrEmpty(ddlChannelId.SelectedValue))
            {
                this.ShowMustInputAlert("繳費管道");
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
                string msg = this.GetLocalized("「繳款人手續」不是合法的金額");
                this.ShowSystemMessage(msg);
                return false;
            }

            if (!Common.IsMoney(tbxRCBPay.Text))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「學校負擔手續」不是合法的金額");
                this.ShowSystemMessage(msg);
                return false;
            }

            if (!Common.IsMoney(tbxMinMoney.Text))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「預設繳費下限 (含)」不是合法的金額");
                this.ShowSystemMessage(msg);
                return false;
            }

            if (!Common.IsMoney(tbxMaxMoney.Text))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「預設繳費上限 (含)」不是合法的金額");
                this.ShowSystemMessage(msg);
                return false;
            }

            return true;
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Server.Transfer("S5600011.aspx");
        }

        protected void ddlChannelId_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAndBindBarcodeIdOptions();
        }

        /// <summary>
        /// 取得並結繫管道代碼選項
        /// </summary>
        private void GetAndBindChannelIdOptions()
        {
            CodeText[] items = null;
            Expression where = new Expression(ChannelSetEntity.Field.IsUsing, "1")
                .And(ChannelSetEntity.Field.CategoryId, string.Empty)
                .And(ChannelSetEntity.Field.ProcessFee, "Y");
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
            WebHelper.SetDropDownListItems(this.ddlChannelId, DefaultItem.Kind.Select, false, items, false, false, 0, null);
        }

        /// <summary>
        /// 取得並結繫管道代碼選項
        /// </summary>
        private void GetAndBindBarcodeIdOptions()
        {
            CodeText[] items = null;
            Expression where = new Expression();
            where.And(ChannelWayEntity.Field.ChannelId, ddlChannelId.SelectedValue);
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(ChannelWayEntity.Field.BarcodeId, OrderByEnum.Asc);

            string[] codeFieldNames = new string[] { ChannelWayEntity.Field.BarcodeId };
            string codeCombineFormat = null;
            string[] textFieldNames = new string[] { ChannelWayEntity.Field.BarcodeId };
            string textCombineFormat = null;

            XmlResult xmlResult = DataProxy.Current.GetEntityOptions<ChannelWayEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
            if (!xmlResult.IsSuccess)
            {
                string action = this.GetLocalized("查詢繳費方式代碼資料");
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
            WebHelper.SetDropDownListItems(this.ddlBarcodeId, DefaultItem.Kind.Select, false, items, false, false, 0, null);
        }

        protected void ddlBarcodeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetAndBindChannelWayData();
        }

        /// <summary>
        /// 取得代收管道手續費級距資料
        /// </summary>
        private void GetAndBindChannelWayData()
        {
            Expression where = new Expression();
            where.And(ChannelWayEntity.Field.ChannelId, this.ddlChannelId.SelectedValue);
            where.And(ChannelWayEntity.Field.BarcodeId, this.ddlBarcodeId.SelectedValue);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(ChannelWayEntity.Field.BarcodeId, OrderByEnum.Asc);

            ChannelWayEntity data = null;
            XmlResult xmlResult = DataProxy.Current.SelectFirst<ChannelWayEntity>(this, where, orderbys, out data);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return;
            }

            this.tbxMinMoney.Text = DataFormat.GetAmountText(data.MinMoney);
            this.tbxMaxMoney.Text = DataFormat.GetAmountText(data.MaxMoney);
            this.tbxChannelCharge.Text = DataFormat.GetAmountText(data.ChannelCharge);
            this.rdoRCFlag.SelectedValue = data.RCFlag;
            this.tbxRCSPay.Text = DataFormat.GetAmountText(data.RCSPay);
            this.tbxRCBPay.Text = DataFormat.GetAmountText(data.RCBPay);
        }
    }
}