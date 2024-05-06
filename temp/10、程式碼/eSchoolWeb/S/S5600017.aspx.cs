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

namespace eSchoolWeb.S
{
    /// <summary>
    /// 財金QRCode支付相關設定
    /// </summary>
    public partial class S5600017 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ccbtnOK.Visible = false;
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    return;
                }
                #endregion

                FiscQRCodeConfig config = null;
                string errmsg = this.GetData(out config);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    this.ShowSystemMessage(errmsg);
                }

                this.BindData(config);
            }
        }

        /// <summary>
        /// 讀取 財金QRCode支付相關設定 資料
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private string GetData(out FiscQRCodeConfig config)
        {
            config = null;
            string errmsg = null;

            ConfigEntity data = null;
            Expression where = new Expression(ConfigEntity.Field.ConfigKey, FiscQRCodeConfig.ConfigKey);
            XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this.Page, where, null, out data);
            if (xmlResult.IsSuccess)
            {
                if (data != null)
                {
                    config = FiscQRCodeConfig.Parse(data.ConfigValue, false);
                    if (config == null)
                    {
                        errmsg = "財金QRCode支付相關設定資料錯誤";
                    }
                }
            }
            else
            {
                errmsg = String.Concat("讀取財金QRCode支付相關設定資料失敗：", xmlResult.Message);
            }
            return errmsg;
        }

        /// <summary>
        /// 結繫 財金QRCode支付相關設定 資料
        /// </summary>
        /// <param name="config"></param>
        private void BindData(FiscQRCodeConfig config)
        {
            if (config == null)
            {
                this.tbxMerchantName.Text = String.Empty;
                this.tbxMerchantId.Text = String.Empty;
                this.tbxTerminalId.Text = String.Empty;
                this.tbxCostId.Text = String.Empty;
                this.tbxSecureCode.Text = String.Empty;
                this.tbxPaymentType.Text = String.Empty;
                this.tbxCountryCode.Text = String.Empty;
                this.tbxCharge.Text = String.Empty;

                #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
                this.tbxPayUrl.Text = String.Empty;
                #endregion
            }
            else
            {
                this.tbxMerchantName.Text = config.MerchantName;
                this.tbxMerchantId.Text = config.MerchantId;
                this.tbxTerminalId.Text = config.TerminalId;
                this.tbxCostId.Text = config.CostId;
                this.tbxSecureCode.Text = config.SecureCode;
                this.tbxPaymentType.Text = config.PaymentType;
                this.tbxCountryCode.Text = config.CountryCode;
                this.tbxCharge.Text = config.Charge == null ? String.Empty : config.Charge.Value.ToString("0.00");

                #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
                this.tbxPayUrl.Text = config.PayUrl;
                #endregion
            }
        }

        /// <summary>
        /// 儲存 財金QRCode支付相關設定 資料
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private bool SaveData(FiscQRCodeConfig config)
        {
            string errmsg = null;

            ConfigEntity data = new ConfigEntity();
            data.ConfigKey = FiscQRCodeConfig.ConfigKey;
            data.ConfigValue = config.ToString();

            #region 更新資料
            int count = 0;
            DataProxy proxy = DataProxy.Current;
            string action = "讀取資料";
            Expression where = new Expression(ConfigEntity.Field.ConfigKey, data.ConfigKey);
            XmlResult xmlResult = proxy.SelectCount<ConfigEntity>(this.Page, where, out count);
            if (xmlResult.IsSuccess)
            {
                if (count > 0)
                {
                    action = "更新資料";
                    xmlResult = proxy.Update(this.Page, data, out count);
                }
                else
                {
                    action = "新增資料";
                    xmlResult = proxy.Insert(this.Page, data, out count);
                }
            }

            if (!xmlResult.IsSuccess)
            {
                errmsg = String.Concat(action, "失敗，", xmlResult.Message);
            }

            if (String.IsNullOrEmpty(errmsg))
            {
                this.ShowSystemMessage(action + "成功");
                return true;
            }
            else
            {
                this.ShowSystemMessage(errmsg);
                return false;
            }
            #endregion
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
            FiscQRCodeConfig config = new FiscQRCodeConfig(this.tbxMerchantName.Text
                , this.tbxMerchantId.Text
                , this.tbxTerminalId.Text
                , this.tbxCostId.Text
                , this.tbxSecureCode.Text
                , this.tbxPaymentType.Text
                , this.tbxCountryCode.Text
                , this.tbxCharge.Text
                , this.tbxPayUrl.Text);
            #endregion

            #region 特店名稱
            if (String.IsNullOrEmpty(config.MerchantName))
            {
                this.ShowMustInputAlert(this.cclabMerchantName.LocalizedText);
                return;
            }
            #endregion

            #region 特店名稱
            if (String.IsNullOrEmpty(config.MerchantId))
            {
                this.ShowMustInputAlert(this.cclabMerchantId.LocalizedText);
                return;
            }
            #endregion

            #region 特店代號
            if (String.IsNullOrEmpty(config.TerminalId))
            {
                this.ShowMustInputAlert(this.cclabTerminalId.LocalizedText);
                return;
            }
            #endregion

            #region 費用代號
            if (String.IsNullOrEmpty(config.CostId))
            {
                this.ShowMustInputAlert(this.cclabCostId.LocalizedText);
                return;
            }
            #endregion

            #region 安全碼
            if (String.IsNullOrEmpty(config.SecureCode))
            {
                this.ShowMustInputAlert(this.cclabSecureCode.LocalizedText);
                return;
            }
            #endregion

            #region 支付工具型態
            if (String.IsNullOrEmpty(config.PaymentType))
            {
                this.ShowMustInputAlert(this.cclabPaymentType.LocalizedText);
                return;
            }
            #endregion

            #region 國別碼
            if (String.IsNullOrEmpty(config.CountryCode))
            {
                this.ShowMustInputAlert(this.cclabCountryCode.LocalizedText);
                return;
            }
            #endregion

            #region 使用者支付手續費
            if (config.Charge == null)
            {
                if (String.IsNullOrWhiteSpace(this.tbxCharge.Text))
                {
                    this.ShowMustInputAlert(this.cclabCharge.LocalizedText);
                    return;
                }
                else
                {
                    this.ShowSystemMessage("使用者支付手續費必須為 0 ~ 9999.99 的數值");
                    return;
                }
            }
            #endregion

            #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
            if (!String.IsNullOrEmpty(config.PayUrl))
            {
                if (config.PayUrl.IndexOf(FiscQRCodeConfig.Separator) > -1)
                {
                    this.ShowSystemMessage(String.Format("繳費網址不可包含 \"{0}\" 字串", FiscQRCodeConfig.Separator));
                    return;
                }
                if (!config.PayUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)
                    && !config.PayUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.ShowSystemMessage("繳費網址必須為 http:// 或 https:// 開頭");
                    return;
                }
            }
            #endregion

            string errmsg = config.CheckValue();
            if (!String.IsNullOrEmpty(errmsg))
            {
                this.ShowSystemMessage(errmsg);
                return;
            }

            if (this.SaveData(config))
            {
                this.BindData(config);
            }
        }
    }
}