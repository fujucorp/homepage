using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju.Web;

namespace eSchoolWeb
{
    public partial class Main2 : System.Web.UI.MasterPage
    {
        /// <summary>
        /// 顯示指定訊息
        /// </summary>
        /// <param name="msg">指定訊息，須自行 Localized</param>
        public void ShowMessage(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"respond\">");
            if (String.IsNullOrWhiteSpace(msg))
            {
                sb.AppendLine("&nbsp;");
            }
            else
            {
                sb.AppendFormat("<span>{0}</span>{1}", WebHelper.GetControlLocalizedByResourceKey("Master_Respond_Title", "注意"), msg).AppendLine();
            }
            sb.AppendLine("</div>");
            this.litMessage.Text = sb.ToString();
        }

        /// <summary>
        /// 顯示指定錯誤代碼與錯誤說明的訊息
        /// </summary>
        /// <param name="code">指定錯誤代碼</param>
        /// <param name="text">指定錯誤說明，須自行 Localized，未指定時由錯誤代碼決定錯誤訊息</param>
        /// <returns>傳回顯示的訊息</returns>
        public string ShowMessage(string code, string text)
        {
            string msg = null;
            code = code == null ? null : code.Trim();
            bool hasCode = code.Length > 0;
            if (String.IsNullOrWhiteSpace(text) && hasCode)
            {
                text = WebHelper.GetErrorCodeLocalized(code, String.Format("發生代碼 {0} 的錯誤", code));
            }
            bool hasText = !String.IsNullOrWhiteSpace(text);
            if (hasCode && hasText)
            {
                msg = String.Format("[{0}] {1}", code.Trim(), text);
            }
            else if (hasText)
            {
                msg = text;
            }

            this.ShowMessage(msg);
            return msg;
        }

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

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}