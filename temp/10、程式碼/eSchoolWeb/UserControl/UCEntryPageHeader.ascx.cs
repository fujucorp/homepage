using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb
{
    public partial class UCEntryPageHeader : BaseUserControl
    {
        private bool? _IsMultiLanguageEnabled = null;
        /// <summary>
        /// 取得是否啟用多語系
        /// </summary>
        /// <returns></returns>
        private bool IsMultiLanguageEnabled()
        {
            if (_IsMultiLanguageEnabled == null)
            {
                string value = Fuju.Configuration.ConfigManager.Current.GetProjectConfigValue("MultiLanguage", "Enabled");
                if (value != null && value.Trim().Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    _IsMultiLanguageEnabled = true;
                }
                else
                {
                    _IsMultiLanguageEnabled = false;
                }
            }
            return _IsMultiLanguageEnabled.Value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string language = null;
                if (this.IsMultiLanguageEnabled())
                {
                    HttpCookie cookie = Request.Cookies["Localized"];
                    string lang = cookie == null ? null : cookie.Value;
                    switch (lang)
                    {
                        case "zh-TW":
                            lang = "tw";
                            language = "zh-TW";
                            break;
                        case "en-US":
                            language = "en-US";
                            lang = "en";
                            break;
                        default:
                            lang = "tw";
                            language = "zh-TW";
                            break;
                    }
                    this.spnLanguage.Visible = true;
                }
                else
                {
                    language = "zh-TW";
                    this.spnLanguage.Visible = false;
                }

                Response.Cookies.Remove("Localized");
                Response.Cookies.Add(new HttpCookie("Localized", language));
                //this.Response.Redirect(this.Page.Request.Url.PathAndQuery);
            }
        }

        protected void lbEN_Click(object sender, EventArgs e)
        {
            if (this.IsMultiLanguageEnabled())
            {
                string language = "en-US";
                Response.Cookies.Remove("Localized");
                Response.Cookies.Add(new HttpCookie("Localized", language));

                #region [MDY:20210706] 原碼修正
                this.Response.Redirect(WebHelper.GenRNUrl(WebHelper.RemoveRNQueryString(this.Page.Request.Url.PathAndQuery)));
                #endregion
            }
        }

        protected void lbTW_Click(object sender, EventArgs e)
        {
            if (this.IsMultiLanguageEnabled())
            {
                string language = "zh-TW";
                Response.Cookies.Remove("Localized");
                Response.Cookies.Add(new HttpCookie("Localized", language));

                #region [MDY:20210706] 原碼修正
                this.Response.Redirect(WebHelper.GenRNUrl(WebHelper.RemoveRNQueryString(this.Page.Request.Url.PathAndQuery)));
                #endregion
            }
        }
    }
}