using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;

using Fuju.Web;

using Entities;

namespace eSchoolWeb.api
{
    /// <summary>
    ///檢查登入狀態的 api
    /// </summary>
    public class CheckLogon : IHttpHandler, IRequiresSessionState, IMenuPage
    {
        #region Implement IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public virtual string MenuID
        {
            get
            {
                return "CheckLogo";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public virtual string MenuName
        {
            get
            {
                return "檢查登入狀態的";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public virtual bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public virtual bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public virtual bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        private HttpContext Context = null;

        private void ResponseJson(bool isOnline)
        {
            StringBuilder json = new StringBuilder();
            json.AppendFormat("{{ \"status\" : \"{0}\"}}", isOnline ? "online" : "logout");
            this.Context.Response.AddHeader("Content-type", "application/json");
            this.Context.Response.ContentType = "application/json";
            this.Context.Response.Write(json.ToString());
        }

        public void ProcessRequest(HttpContext context)
        {
            this.Context = context;

            LogonUser logonUser = WebHelper.GetLogonUser();
            if (logonUser != null)
            {
                string resultCode = null;
                XmlResult xmlResult = DataProxy.Current.CheckLogon(null, logonUser, false, out resultCode);
                if (xmlResult.IsSuccess)
                {
                    if (resultCode == CheckLogonResultCodeTexts.IS_OK)
                    {
                        this.ResponseJson(true);
                        return;
                    }
                }
            }
            this.ResponseJson(false);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}