using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb
{
    /// <summary>
    /// 使用者控制項基底抽象類別
    /// </summary>
    public abstract class BaseUserControl : System.Web.UI.UserControl
    {
        #region Const
        /// <summary>
        /// 預設的 Javascript Alert 訊息 Key 的常數
        /// </summary>
        private const string SHOW_JS_ALERT_KEY = "SHOW_JS_ALERT";
        #endregion

        #region 建構式
        protected BaseUserControl()
        {
        }
        #endregion

        #region Localized 相關 Method
        /// <summary>
        /// 取得指定文字的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="text">指定文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回指定文字或空字串</returns>
        protected string GetLocalized(string text)
        {
            return WebHelper.GetLocalized(text);
        }

        /// <summary>
        /// 取得指定資源索引鍵的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="key">指定資源索引鍵</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回預設文字或空字串</returns>
        protected string GetLocalized(string resourceKey, string defaultText)
        {
            return WebHelper.GetLocalized(resourceKey, defaultText);
        }

        /// <summary>
        /// 取得指定控制項的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="ctlName">指定控制項的名稱</param>
        /// <param name="ctlKey">指定控制項的資源索引鍵</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回預設文字或空字串</returns>
        protected string GetControlLocalized(string ctlName, string ctlKey, string defaultText)
        {
            string resourceKey = String.Concat(ctlName, "_", ctlKey);
            return WebHelper.GetControlLocalizedByResourceKey(resourceKey, defaultText);
        }

        /// <summary>
        /// 取得指定錯誤代碼的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="code">指定錯誤代碼</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回預設文字或空字串</returns>
        public string GetErrorLocalized(string code, string defaultText)
        {
            return WebHelper.GetLocalized("ERR_" + code, defaultText);
        }
        #endregion

        #region 顯示訊息 相關 Method
        /// <summary>
        /// 顯示 Javascript 的 Alert 訊息
        /// </summary>
        /// <param name="msg">要顯示的訊息</param>
        protected void ShowJsAlert(string msg)
        {
            this.ShowJsAlert(null, msg);
        }

        /// <summary>
        /// 顯示 Javascript 的 Alert 訊息
        /// </summary>
        /// <param name="key">用戶端指令碼區塊索引鍵</param>
        /// <param name="msg">要顯示的訊息</param>
        protected void ShowJsAlert(string key, string msg)
        {
            if (String.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            key = String.IsNullOrWhiteSpace(key) ? SHOW_JS_ALERT_KEY : key.Trim();
            StringBuilder js = new StringBuilder();
            js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(msg)).AppendLine();

            ClientScriptManager cs = this.Page.ClientScript;
            Type myType = this.GetType();
            if (!cs.IsClientScriptBlockRegistered(myType, key))
            {
                cs.RegisterClientScriptBlock(myType, key, js.ToString(), true);
            }
        }

        /// <summary>
        /// 顯示 Javascript 的 Alert 訊息，並前往指定網址
        /// </summary>
        /// <param name="msg">要顯示的訊息</param>
        /// <param name="url">要前往的網址</param>
        protected void ShowJsAlertAndGoUrl(string msg, string url)
        {
            if (String.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            string key = SHOW_JS_ALERT_KEY;

            StringBuilder js = new StringBuilder();
            js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(msg)).AppendLine();
            if (!String.IsNullOrWhiteSpace(url))
            {
                js.AppendFormat("window.location.href = '{0}';", HttpUtility.JavaScriptStringEncode(url)).AppendLine();
            }

            ClientScriptManager cs = this.Page.ClientScript;
            Type myType = this.GetType();
            if (!cs.IsClientScriptBlockRegistered(myType, key))
            {
                cs.RegisterClientScriptBlock(myType, key, js.ToString(), true);
            }
        }

        /// <summary>
        /// 顯示請指定某欄位的 Alert 訊息
        /// </summary>
        /// <param name="fieldName">指定的欄位名稱，欄位名稱須自行 Localized</param>
        protected void ShowMustInputAlert(string fieldName)
        {
            string ptn = this.GetLocalized("MSG_PTN_INPUT_MUST", "請指定「{0}」");
            string msg = String.Format(ptn, fieldName);
            this.ShowJsAlert(msg);
        }

        /// <summary>
        /// 顯示請指定某控制項的欄位的 Alert 訊息
        /// </summary>
        /// <param name="ctlName">指定控制項的名稱</param>
        /// <param name="ctlKey">指定控制項的欄位資源索引鍵</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        protected void ShowMustInputAlert(string ctlName, string ctlKey, string defaultText)
        {
            string fieldName = this.GetControlLocalized(ctlName, ctlKey, defaultText);
            this.ShowMustInputAlert(fieldName);
        }

        /// <summary>
        /// 顯示某操作失敗的 Alert 訊息
        /// </summary>
        /// <param name="action">指定操作名稱，須自行 Localized</param>
        /// <param name="desc">指定失敗描述，須自行 Localized</param>
        protected void ShowActionFailureAlert(string action, string desc)
        {
            string ptn = null;
            string msg = null;
            if (String.IsNullOrWhiteSpace(desc))
            {
                ptn = this.GetLocalized("MSG_PTN_ACTION_FAILURE", "{0}失敗");
                msg = String.Format(ptn, action);
            }
            else
            {
                ptn = this.GetLocalized("MSG_PTN_ACTION_FAILURE_DESC", "{0}失敗，{1}");
                msg = String.Format(ptn, action, desc);
            }
            this.ShowJsAlert(msg);
        }

        /// <summary>
        /// 顯示某操作失敗的 Alert 訊息
        /// </summary>
        /// <param name="action">指定操作名稱，須自行 Localized</param>
        /// <param name="code">指定錯誤代碼</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        protected void ShowActionFailureAlert(string action, string code, string defaultText)
        {
            string desc = this.GetErrorLocalized(code, defaultText);
            this.ShowActionFailureAlert(action, desc);
        }

        /// <summary>
        /// 顯示某操作成功的 Alert 訊息
        /// </summary>
        /// <param name="action">指定操作名稱，須自行 Localized</param>
        protected void ShowActionSuccessAlert(string action)
        {
            string ptn = this.GetLocalized("MSG_PTN_ACTION_SUCCESS", "{0}成功");
            string msg = String.Format(ptn, action);
            this.ShowJsAlert(msg);
        }
        #endregion

        #region [MDY:20210325] 原碼修正 取得用戶端可使用 Url 相關 Method
        /// <summary>
        /// 取得指定網址的用戶端可用網址
        /// </summary>
        /// <param name="url">指定網址。未指定或指定 javascript: 則傳回空字串</param>
        /// <returns>傳回用戶端可用網址</returns>
        protected virtual string GetResolveUrl(string url)
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

        /// <summary>
        /// 取得指定連結網址的用戶端可用網址
        /// </summary>
        /// <param name="url">指定連結網址</param>
        /// <returns>傳回用戶端可用網址</returns>
        protected virtual string GetLinkResolveUrl(string url, out bool isOtherWeb)
        {
            isOtherWeb = false;
            if (url != null)
            {
                if (url.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase))
                {
                    return url;
                }
                if (url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)
                    || url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                {
                    isOtherWeb = true;
                    return url;
                }
            }

            url = this.GetResolveUrl(url);
            if (String.IsNullOrEmpty(url))
            {
                return "javascript:void(0)";
            }
            return url;
        }
        #endregion

        #region [MDY:202203XX] 2022擴充案 取得是否使用英文介面
        /// <summary>
        /// 取得是否使用英文介面
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        protected bool isEngUI()
        {
            return "en-US".Equals(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(), StringComparison.CurrentCultureIgnoreCase);
        }
        #endregion
    }
}