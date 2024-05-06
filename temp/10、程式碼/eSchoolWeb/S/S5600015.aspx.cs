using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 支付寶相關設定
    /// </summary>
    public partial class S5600015 : BasePage
    {
        /// <summary>
        /// 結繫支付寶系統參數設定
        /// </summary>
        /// <param name="data"></param>
        private void BindInboundSystem(InboundConfig data)
        {
            if (data == null)
            {
                data = new InboundConfig();
            }

            this.txtInboundURL.Text = Server.HtmlEncode(data.InboundUrl);
            this.txtmerchantId.Text = Server.HtmlEncode(data.MerchantId);
            this.txtterminalId.Text = Server.HtmlEncode(data.TerminalId);
            this.txtInitKey.Text = Server.HtmlEncode(data.InitKey);
            this.txtKey.Text = Server.HtmlEncode(data.Key);
            this.txtCharge.Text = Server.HtmlEncode(data.Charge);
            this.txtAuthResURL.Text = Server.HtmlEncode(data.AuthResUrl);
        }

        #region [MDY:20180926] 增加特殊商家代號清單 (開放可繳4000元以下繳費單)
        #region [OLD]
        ///// <summary>
        ///// 結繫授權商家代號設定
        ///// </summary>
        ///// <param name="configValue">授權商家代號設定值</param>
        //private void BindInboundReceiveType(string configValue)
        //{
        //    string[] values = null;
        //    if (!String.IsNullOrWhiteSpace(configValue))
        //    {
        //        values = configValue.Replace(" ", "").Replace("　", "").Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        //    }
        //    this.BindInboundReceiveType(values);
        //}

        ///// <summary>
        ///// 結繫授權商家代號設定
        ///// </summary>
        ///// <param name="values">授權商家代號陣列</param>
        //private void BindInboundReceiveType(string[] values)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (values != null && values.Length > 0)
        //    {
        //        for (int startIndex = 0; startIndex < values.Length; startIndex += 10)
        //        {
        //            int count = values.Length - startIndex;
        //            sb.AppendLine(String.Join(",", values, startIndex, (count > 10 ? 10 : count)));
        //        }
        //    }
        //    this.txtAuthReceiveType.Text = Server.HtmlEncode(sb.ToString().Trim());
        //}
        #endregion

        private void BindInboundReceiveType(TextBox tbx, ICollection<string> values)
        {
            StringBuilder sb = new StringBuilder();
            if (values != null && values.Count > 0)
            {
                int no = 0;
                foreach (string value in values)
                {
                    no++;
                    sb.AppendFormat(",{0}", value);
                    if (no % 10 == 0)
                    {
                        sb.AppendLine();
                    }
                }
            }
            tbx.Text = Server.HtmlEncode(sb.ToString().Trim(new char[] { ',', '\r', '\n'}));
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ccbtnOK1.Visible = false;
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    return;
                }
                #endregion

                string errmsg = null;
                InboundConfig data = Fisc.GetInboundConfig(out errmsg);
                if (data == null)
                {
                    this.ShowSystemMessage(errmsg);
                    return;
                }

                #region [MDY:20180101] 改用 BindInboundSystem() 結繫支付寶系統參數設定
                #region [Old]
                //this.txtInboundURL.Text = inboundConfig.InboundUrl.Trim();
                //this.txtmerchantId.Text = inboundConfig.MerchantId.Trim();
                //this.txtterminalId.Text = inboundConfig.TerminalId.Trim();
                //this.txtInitKey.Text = inboundConfig.InitKey.Trim();
                //this.txtKey.Text = inboundConfig.Key.Trim();
                //this.txtCharge.Text = inboundConfig.Charge.Trim();
                //this.txtAuthResURL.Text = inboundConfig.AuthResUrl.Trim();
                #endregion

                this.BindInboundSystem(data);
                #endregion

                #region [MDY:20180926] 增加特殊商家代號清單 (開放可繳4000元以下繳費單)
                #region [OLD]
                //#region [MDY:20180101] 增加財金支付寶授權商家代號設定資料
                //this.BindInboundReceiveType(data.AuthReceiveType);
                //#endregion
                #endregion

                this.BindInboundReceiveType(this.txtAuthReceiveType, data.GetAuthReceiveTypes());
                this.BindInboundReceiveType(this.txtSpecialReceiveType, data.GetSpecialReceiveTypes());
                #endregion
            }
        }

        #region [OLD]
        //protected void ccbtnOK_Click(object sender, EventArgs e)
        //{
        //    #region 檢查維護權限
        //    if (!this.HasMaintainAuth())
        //    {
        //        this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
        //        return;
        //    }
        //    #endregion

        //    string errmsg = null;

        //    InboundConfig inbound_config = new InboundConfig();
        //    inbound_config.InboundUrl = this.txtInboundURL.Text.Trim();
        //    inbound_config.MerchantId = this.txtmerchantId.Text.Trim();
        //    inbound_config.TerminalId = this.txtterminalId.Text.Trim();
        //    inbound_config.InitKey = this.txtInitKey.Text.Trim();
        //    inbound_config.Key = this.txtKey.Text.Trim();
        //    inbound_config.Charge = this.txtCharge.Text.Trim();
        //    inbound_config.AuthResUrl = this.txtAuthResURL.Text.Trim();
        //    if (!Fisc.CheckInboundConfig(inbound_config, out errmsg))
        //    {
        //        this.ShowSystemMessage(errmsg);
        //        return;
        //    }

        //    if (!Fisc.SaveInboundConfig(inbound_config, out errmsg))
        //    {
        //        this.ShowSystemMessage(errmsg);
        //        return;
        //    }
        //    else
        //    {
        //        this.ShowSystemMessage("資料設定成功");
        //        return;
        //    }
        //}
        #endregion

        protected void ccbtnOK1_Click(object sender, EventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            #region 處理輸入資料
            InboundConfig data = new InboundConfig();
            data.InboundUrl = this.txtInboundURL.Text;
            data.MerchantId = this.txtmerchantId.Text;
            data.TerminalId = this.txtterminalId.Text;
            data.InitKey = this.txtInitKey.Text;
            data.Key = this.txtKey.Text;
            data.Charge = this.txtCharge.Text;
            data.AuthResUrl = this.txtAuthResURL.Text;

            string errmsg = data.CheckValue();
            if (!String.IsNullOrEmpty(errmsg))
            {
                this.ShowSystemMessage(errmsg);
                return;
            }
            #endregion

            #region 儲存資料
            string action = this.GetLocalized("儲存支付寶系統參數設定");
            if (!Fisc.SaveInboundConfig(data, out errmsg))
            {
                this.ShowActionFailureMessage(action, errmsg);
                return;
            }
            else
            {
                this.ShowActionSuccessMessage(action);
                this.BindInboundSystem(data);
                return;
            }
            #endregion
        }

        protected void ccbtnOK2_Click(object sender, EventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            #region [MDY:20180926] 增加特殊商家代號清單 (開放可繳4000元以下繳費單)
            #region [OLD]
            //#region 處理輸入資料
            //string configValue = String.Empty;
            //if (!String.IsNullOrWhiteSpace(this.txtAuthReceiveType.Text))
            //{
            //    string[] args = this.txtAuthReceiveType.Text.Trim().Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //    List<string> values = new List<string>(args.Length);
            //    foreach (string arg in args)
            //    {
            //        string value = arg.Trim();
            //        if (!String.IsNullOrEmpty(value))
            //        {
            //            if (Common.IsNumber(value, 4))
            //            {
            //                if (!values.Contains(value))
            //                {
            //                    values.Add(value);
            //                }
            //            }
            //            else
            //            {
            //                this.ShowSystemMessage("每個商家代號必須是4碼的數值");
            //                return;
            //            }
            //        }
            //    }
            //    values.Sort();
            //    configValue = String.Join(",", values.ToArray());
            //    this.BindInboundReceiveType(configValue);
            //}
            //else
            //{
            //    this.txtAuthReceiveType.Text = String.Empty;
            //}
            //#endregion

            //#region 儲存資料
            //string action = this.GetLocalized("儲存授權商家代號設定");
            //string errmsg = Fisc.SaveInboundReceiveTypeConfig(this.Page, configValue);
            //if (String.IsNullOrEmpty(errmsg))
            //{
            //    this.ShowActionSuccessMessage(action);
            //    this.BindInboundReceiveType(configValue);
            //    return;
            //}
            //else
            //{
            //    this.ShowActionFailureMessage(action, errmsg);
            //    return;
            //}
            //#endregion
            #endregion

            #region 授權商家代號清單
            List<string> authReceiveTypes = null;
            {
                string txt = this.txtAuthReceiveType.Text;
                if (!String.IsNullOrWhiteSpace(txt))
                {
                    string[] values = txt.Trim().Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    authReceiveTypes = new List<string>(values.Length);
                    foreach (string value in values)
                    {
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            string receiveType = value.Trim();
                            if (Common.IsNumber(receiveType, 4))
                            {
                                if (!authReceiveTypes.Contains(receiveType))
                                {
                                    authReceiveTypes.Add(receiveType);
                                }
                            }
                            else
                            {
                                this.ShowSystemMessage("【授權商家代號清單】的每個商家代號必須是4碼的數值");
                                return;
                            }
                        }
                    }
                    authReceiveTypes.Sort();
                }
                this.BindInboundReceiveType(this.txtAuthReceiveType, authReceiveTypes);
            }
            #endregion

            #region 特殊商家代號清單
            List<string> specialReceiveTypes = null;
            {
                string txt = this.txtSpecialReceiveType.Text;
                if (!String.IsNullOrWhiteSpace(txt))
                {
                    string[] values = txt.Trim().Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    specialReceiveTypes = new List<string>(values.Length);
                    foreach (string value in values)
                    {
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            string receiveType = value.Trim();
                            if (Common.IsNumber(receiveType, 4))
                            {
                                if (!authReceiveTypes.Contains(receiveType))
                                {
                                    this.ShowSystemMessage(String.Format("【特殊商家代號清單】的 {0} 必須同時是【授權商家代號清單】的商家代號", receiveType));
                                    return;
                                }
                                if (!specialReceiveTypes.Contains(receiveType))
                                {
                                    specialReceiveTypes.Add(receiveType);
                                }
                            }
                            else
                            {
                                this.ShowSystemMessage("【特殊商家代號清單】的每個商家代號必須是4碼的數值");
                                return;
                            }
                        }
                    }
                    specialReceiveTypes.Sort();
                }
                this.BindInboundReceiveType(this.txtSpecialReceiveType, specialReceiveTypes);
            }
            #endregion

            #region 儲存資料
            string action = this.GetLocalized("儲存授權商家代號設定");
            string receiveTypeConfigValue = InboundConfig.GenReceiveTypeConfigValue(authReceiveTypes, specialReceiveTypes);
            string errmsg = Fisc.SaveInboundReceiveTypeConfig(this.Page, receiveTypeConfigValue);
            if (String.IsNullOrEmpty(errmsg))
            {
                this.ShowActionSuccessMessage(action);
                //this.BindInboundReceiveType(this.txtAuthReceiveType, authReceiveTypes);
                //this.BindInboundReceiveType(this.txtSpecialReceiveType, specialReceiveTypes);
                return;
            }
            else
            {
                this.ShowActionFailureMessage(action, errmsg);
                return;
            }
            #endregion
            #endregion
        }

    }
}