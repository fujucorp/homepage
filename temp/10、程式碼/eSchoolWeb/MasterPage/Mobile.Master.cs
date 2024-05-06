using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju.Web;
namespace eSchoolWeb.MasterPage
{
    public partial class Mobile : System.Web.UI.MasterPage
    {
        #region [MDY:20210325] 原碼修正 取得用戶端可使用 Url 相關 Method
        /// <summary>
        /// 取得指定網址的用戶端可用網址
        /// </summary>
        /// <param name="url">指定網址。未指定或指定 javascript: 則傳回空字串</param>
        /// <returns>傳回用戶端可用網址</returns>
        protected string GetResolveUrl(string url)
        {
            #region [MDY:20210401] 原碼修正
            if (String.IsNullOrEmpty(url)
                || url.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase))
            {
                return String.Empty;
            }
            if (url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) 
                || url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                return Uri.EscapeUriString(url);
            }

            #region [MDY:20200705] 修正特殊字串路徑 "/(Z(%22onerror=%22alert'XSS'%22))/" 的 XSS 問題
            #region [OLD]
            //if (url.StartsWith("~/"))
            //{
            //    return this.ResolveUrl(url);
            //}
            //if (url.StartsWith("/"))
            //{
            //    return this.ResolveUrl("~" + url);
            //}
            //return this.ResolveUrl("~/" + url);
            #endregion

            if (url.StartsWith("/"))
            {
                return Uri.EscapeUriString(String.Concat(Request.ApplicationPath, url.Substring(1)));
            }
            else if (url.StartsWith("~/"))
            {
                return Uri.EscapeUriString(String.Concat(Request.ApplicationPath, url.Substring(2)));
            }
            else if (url.StartsWith("./"))
            {
                return Uri.EscapeUriString(String.Concat(String.Join("", Request.Url.Segments, 0, Request.Url.Segments.Length - 1), url.Substring(2)));
            }
            else if (url.StartsWith("../"))
            {
                return Uri.EscapeUriString(String.Concat(String.Join("", Request.Url.Segments, 0, Request.Url.Segments.Length - 2), url.Substring(2)));
            }
            else
            {
                return Uri.EscapeUriString(String.Concat(Request.ApplicationPath, url));
            }
            #endregion
            #endregion
        }
        #endregion

        #region [MDY:20190901] Localized
        /// <summary>
        /// 取得指定文字的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="text">指定文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回指定文字或空字串</returns>
        protected string GetLocalized(string text)
        {
            return WebHelper.GetLocalized(text);
        }
        #endregion

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
                    this.lbEN.Visible = true;
                    this.lbTW.Visible = true;
                }
                else
                {
                    language = "zh-TW";
                    this.lbEN.Visible = false;
                    this.lbTW.Visible = false;
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

                #region [MDY:20210325] 原碼修正
                #region [OLD]
                //this.Response.Redirect(this.Page.Request.Url.PathAndQuery);
                #endregion

                this.InitializeCulture();
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

                #region [MDY:20210325] 原碼修正
                #region [OLD]
                //this.Response.Redirect(this.Page.Request.Url.PathAndQuery);
                #endregion

                this.InitializeCulture();
                #endregion
            }
        }

        #region [MDY:20210325] 原碼修正
        private void InitializeCulture()
        {
            HttpCookie cookie = Request.Cookies["Localized"];
            if (cookie != null)
            {
                cookie.HttpOnly = true;
                CultureInfo currentInfo = CultureInfo.CurrentCulture;
                CultureInfo currentUIInfo = CultureInfo.CurrentUICulture;
                string localized = cookie.Value.Trim();
                switch (localized)
                {
                    case "zh-TW":
                    case "0x0404":
                    case "1028":
                        currentInfo = new CultureInfo("zh-TW");
                        currentUIInfo = CultureInfo.CreateSpecificCulture("zh-TW");
                        break;
                    case "en-US":
                    case "0x0409":
                    case "1033":
                        currentInfo = new CultureInfo("en-US");
                        currentUIInfo = CultureInfo.CreateSpecificCulture("en-US");
                        break;
                    case "ja-JP":
                    case "0x0411":
                    case "1041":
                        currentInfo = new CultureInfo("ja-JP");
                        currentUIInfo = CultureInfo.CreateSpecificCulture("ja-JP");
                        break;
                    case "zh-CN":
                    case "0x0804":
                    case "2052":
                        currentInfo = new CultureInfo("zh-CN");
                        currentUIInfo = CultureInfo.CreateSpecificCulture("zh-CN");
                        break;
                }
                Thread.CurrentThread.CurrentUICulture = currentInfo;
                Thread.CurrentThread.CurrentCulture = currentUIInfo;
            }
        }
        #endregion
    }
}