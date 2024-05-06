using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.S
{
    /// <summary>
    /// 系統參數設定
    /// </summary>
    public partial class S5600016 : BasePage
    {
        private void InitialUI()
        {
            this.tbxCTCBSpecial.Text = "";
            this.tbxFileServiceUrl.Text = "";

            #region [MDY:20170818] M201708_01 增加學校銷帳檔3合1的商家代號系統參數 SC31ReceiveType (20170531_01)
            this.tbxSC31ReceiveType.Text = "";
            #endregion

            #region [MDY:20200807] M202008_01 增加學生姓名要遮罩的商家代號參數 MaskReceiveType (2020806_01)
            this.tbxMaskReceiveType.Text = "";
            #endregion

            #region [MDY:20200815] M202008_02 增加顯示使用者密碼的學校代碼參數 FixVerifySchoolId (2020819_01)
            this.tbxFixVerifySchoolId.Text = "";
            #endregion
        }

        private bool GetDataAndBind()
        {
            bool isOK = true;

            #region 讀取資料
            ConfigEntity[] datas = null;
            {

                string[] configKeys = new string[] { 
                    "CTCBSpecial", "FileServiceUrl"

                    #region [MDY:20170818] M201708_01 增加學校銷帳檔3合1的商家代號系統參數 SC31ReceiveType (20170531_01)
                    , ConfigKeyCodeTexts.SC31_RECEIVETYPE
                    #endregion

                    #region [MDY:20200807] M202008_01 增加學生姓名要遮罩的商家代號參數 MaskReceiveType (2020806_01)
                    , ConfigKeyCodeTexts.MASK_RECEIVETYPE
                    #endregion

                    #region [MDY:20200815] M202008_02 增加顯示使用者密碼的學校代碼參數 FixVerifySchoolId (2020819_01)
                    , ConfigKeyCodeTexts.FIXVERIFY_SCHOOLID
                    #endregion
                };

                Expression where = new Expression(ConfigEntity.Field.ConfigKey, configKeys);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(ConfigEntity.Field.ConfigKey, OrderByEnum.Asc);
                XmlResult xmlResult = DataProxy.Current.SelectAll<ConfigEntity>(this.Page, where, orderbys, out datas);
                if (!xmlResult.IsSuccess)
                {
                    isOK = false;
                    string action = this.GetLocalized("讀取系統參數設定");
                    this.ShowActionFailureMessage(action, xmlResult.Message);
                }
            }
            #endregion

            #region 結繫資料
            if (datas != null && datas.Length > 0)
            {
                ContentPlaceHolder holder = this.Page.Master.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
                foreach (ConfigEntity data in datas)
                {
                    string id = String.Format("tbx{0}", data.ConfigKey);
                    TextBox tbx = holder.FindControl(id) as TextBox;
                    if (tbx != null)
                    {
                        #region [MDY:20200815] M202008_02 增加顯示使用者密碼的學校代碼參數 FixVerifySchoolId (2020819_01)
                        #region [MDY:20200807] M202008_01 增加學生姓名要遮罩的商家代號參數 MaskReceiveType (2020806_01)
                        #region [MDY:20170818] M201708_01 增加學校銷帳檔3合1的商家代號系統參數 SC31ReceiveType (20170531_01)
                        if (id == this.tbxSC31ReceiveType.ID)
                        {
                            this.BindReceiveTypeTextbox(tbx, data.ConfigValue);
                        }
                        else if (id == this.tbxMaskReceiveType.ID)
                        {
                            this.BindReceiveTypeTextbox(tbx, data.ConfigValue);
                        }
                        else if (id == this.tbxFixVerifySchoolId.ID)
                        {
                            //因為學校代碼與商家代號都是 4 碼數字，所以借用 BindReceiveTypeTextbox() 做格式化 Bind 資料
                            this.BindReceiveTypeTextbox(tbx, data.ConfigValue);
                        }
                        else
                        {
                            tbx.Text = data.ConfigValue;
                        }
                        #endregion
                        #endregion
                        #endregion
                    }
                }
            }
            #endregion

            return isOK;
        }

        /// <summary>
        /// 結繫商家代號的輸入格控制項
        /// </summary>
        /// <param name="tbxControl">要結繫的輸入格控制項</param>
        /// <param name="text">要結繫的輸入格文字</param>
        private void BindReceiveTypeTextbox(TextBox tbxControl, string text)
        {
            StringBuilder sb = new StringBuilder();
            string[] datas = text.Replace(" ", "").Replace("　", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (datas != null && datas.Length > 0)
            {
                for (int startIndex = 0; startIndex < datas.Length;)
                {
                    int count = datas.Length - startIndex;
                    sb.AppendLine(String.Join(",", datas, startIndex, (count > 10 ? 10 : count)));
                    startIndex += 10;
                }
            }
            tbxControl.Text = Server.HtmlEncode(sb.ToString().Trim());
        }

        private string GetConfigKeyText(string configKey)
        {
            switch (configKey)
            {
                case "CTCBSpecial":
                    return "中國信託特殊商家代號";
                case "FileServiceUrl":
                    return "FileService 網址";

                #region [MDY:20170818] M201708_01 增加學校銷帳檔3合1的商家代號系統參數 SC31ReceiveType (20170531_01)
                case ConfigKeyCodeTexts.SC31_RECEIVETYPE:
                    return "學校銷帳檔3合1的商家代號";
                #endregion

                #region [MDY:20200807] M202008_01 增加學生姓名要遮罩的商家代號參數 MaskReceiveType (2020806_01)
                case ConfigKeyCodeTexts.MASK_RECEIVETYPE:
                    return "學生姓名要遮罩的商家代號";
                #endregion

                #region [MDY:20200815] M202008_02 增加顯示使用者密碼的學校代碼參數 FixVerifySchoolId (2020819_01)
                case ConfigKeyCodeTexts.FIXVERIFY_SCHOOLID:
                    return "顯示使用者密碼的學校代碼";
                #endregion

                default:
                    return configKey ?? String.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                this.ccbtnOK.Visible = this.GetDataAndBind();
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            List<ConfigEntity> datas = new List<ConfigEntity>(2);

            #region CTCBSpecial
            {
                string configKey = "CTCBSpecial";
                string configKeyText = this.GetConfigKeyText(configKey);

                string[] args = this.tbxCTCBSpecial.Text.Trim().Split(',');
                List<string> values = new List<string>(args.Length);
                foreach (string arg in args)
                {
                    if (!String.IsNullOrWhiteSpace(arg))
                    {
                        values.Add(arg.Trim());
                    }
                }
                string configValue = String.Join(",", values.ToArray());

                #region [MDY:20170708] HtmlEncode (For Reflected XSS Specific Clients 修改)
                this.tbxCTCBSpecial.Text = Server.HtmlEncode(configValue);
                #endregion

                ConfigEntity data = new ConfigEntity();
                data.ConfigKey = configKey;
                data.ConfigValue = configValue;
                datas.Add(data);
            }
            #endregion

            #region FileServiceUrl
            {
                string configKey = "FileServiceUrl";
                string configKeyText = this.GetConfigKeyText(configKey);

                string url = this.tbxFileServiceUrl.Text.Trim();
                if (String.IsNullOrWhiteSpace(url))
                {
                    this.ShowMustInputAlert(configKeyText);
                    return;
                }
                if (!url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) && !url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.ShowSystemMessage(configKeyText + "必須是 HTTP:// 或 HTTPS:// 開頭");
                    return;
                }

                ConfigEntity data = new ConfigEntity();
                data.ConfigKey = configKey;
                data.ConfigValue = url;
                datas.Add(data);
            }
            #endregion

            #region [MDY:20170818] M201708_01 增加學校銷帳檔3合1的商家代號系統參數 SC31ReceiveType (20170531_01)
            {
                string configKey = ConfigKeyCodeTexts.SC31_RECEIVETYPE;
                string configKeyText = this.GetConfigKeyText(configKey);

                string configValue = String.Empty;
                if (!String.IsNullOrWhiteSpace(this.tbxSC31ReceiveType.Text))
                {
                    string[] args = this.tbxSC31ReceiveType.Text.Trim().Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> values = new List<string>(args.Length);
                    foreach (string arg in args)
                    {
                        if (!String.IsNullOrWhiteSpace(arg))
                        {
                            string value = arg.Trim();
                            if (Common.IsNumber(value, 4))
                            {
                                if (!values.Contains(value))
                                {
                                    values.Add(value);
                                }
                            }
                            else
                            {
                                this.ShowSystemMessage(configKeyText + " 每個商家代號必須是4碼的數值");
                                return;
                            }
                        }
                    }
                    values.Sort();
                    configValue = String.Join(",", values.ToArray());
                }
                this.BindReceiveTypeTextbox(this.tbxSC31ReceiveType, configValue);

                ConfigEntity data = new ConfigEntity();
                data.ConfigKey = configKey;
                data.ConfigValue = configValue;
                datas.Add(data);
            }
            #endregion

            #region [MDY:20200807] M202008_01 增加學生姓名要遮罩的商家代號參數 MaskReceiveType (2020806_01)
            {
                string configKey = ConfigKeyCodeTexts.MASK_RECEIVETYPE;
                string configKeyText = this.GetConfigKeyText(configKey);

                string configValue = String.Empty;
                if (!String.IsNullOrWhiteSpace(this.tbxMaskReceiveType.Text))
                {
                    string[] args = this.tbxMaskReceiveType.Text.Trim().Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> values = new List<string>(args.Length);
                    foreach (string arg in args)
                    {
                        if (!String.IsNullOrWhiteSpace(arg))
                        {
                            string value = arg.Trim();
                            if (Common.IsNumber(value, 4))
                            {
                                if (!values.Contains(value))
                                {
                                    values.Add(value);
                                }
                            }
                            else
                            {
                                this.ShowSystemMessage(configKeyText + " 每個商家代號必須是4碼的數值");
                                return;
                            }
                        }
                    }
                    values.Sort();
                    configValue = String.Join(",", values.ToArray());
                }
                this.BindReceiveTypeTextbox(this.tbxMaskReceiveType, configValue);

                ConfigEntity data = new ConfigEntity();
                data.ConfigKey = configKey;
                data.ConfigValue = configValue;
                datas.Add(data);
            }
            #endregion

            #region [MDY:20200815] M202008_02 增加顯示使用者密碼的學校代碼參數 FixVerifySchoolId (2020819_01)
            {
                string configKey = ConfigKeyCodeTexts.FIXVERIFY_SCHOOLID;
                string configKeyText = this.GetConfigKeyText(configKey);

                string configValue = String.Empty;
                if (!String.IsNullOrWhiteSpace(this.tbxFixVerifySchoolId.Text))
                {
                    string[] args = this.tbxFixVerifySchoolId.Text.Trim().Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> values = new List<string>(args.Length);
                    foreach (string arg in args)
                    {
                        if (!String.IsNullOrWhiteSpace(arg))
                        {
                            string value = arg.Trim();
                            if (Common.IsNumber(value, 4))
                            {
                                if (!values.Contains(value))
                                {
                                    values.Add(value);
                                }
                            }
                            else
                            {
                                this.ShowSystemMessage(configKeyText + " 每個學校代碼必須是4碼的數值");
                                return;
                            }
                        }
                    }
                    values.Sort();
                    configValue = String.Join(",", values.ToArray());
                }
                this.BindReceiveTypeTextbox(this.tbxFixVerifySchoolId, configValue);

                ConfigEntity data = new ConfigEntity();
                data.ConfigKey = configKey;
                data.ConfigValue = configValue;
                datas.Add(data);
            }
            #endregion

            #region 更新資料
            List<string> errmsgs = new List<string>();
            DataProxy proxy = DataProxy.Current;
            foreach (ConfigEntity data in datas)
            {
                int count = 0;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, data.ConfigKey);
                XmlResult xmlResult = proxy.SelectCount<ConfigEntity>(this.Page, where, out count);
                if (xmlResult.IsSuccess)
                {
                    if (count > 0)
                    {
                        xmlResult = proxy.Update(this.Page, data, out count);
                    }
                    else
                    {
                        xmlResult = proxy.Insert(this.Page, data, out count);
                    }
                }
                if (!xmlResult.IsSuccess)
                {
                    errmsgs.Add(String.Format("更新{0}參數值失敗，{1}", this.GetConfigKeyText(data.ConfigKey), xmlResult.Message));
                }
            }

            if (errmsgs.Count == 0)
            {
                this.ShowActionSuccessMessage(this.GetLocalized("系統參數設定"));
            }
            else
            {
                this.ShowSystemMessage(String.Join("\r\n", errmsgs.ToArray()), false);
            }
            #endregion
        }
    }
}