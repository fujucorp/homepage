using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Web.Services;

namespace eSchoolWeb.api
{
    /// <summary>
    /// 廣告圖檔讀取處理常式
    /// </summary>
    public class AD : IHttpHandler
    {
        #region Implement IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public virtual string MenuID
        {
            get
            {
                return "AD";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public virtual string MenuName
        {
            get
            {
                return "廣告";
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

        public void ProcessRequest(HttpContext context)
        {
            this.Context = context;

            #region [MDY:20210401] 原碼修正
            string id = context.Request.QueryString["id"];
            #endregion

            byte[] content = CacheHelper.GetADImgContent(id);
            if (content != null)
            {
                #region [MDY:20210401] 原碼修正
                #region [OLD]
                //context.Response.AddHeader("Content-Language", "utf-8");
                //context.Response.ContentType = "image/gif";
                #endregion

                context.Response.ContentType = "image/jpeg";
                #endregion

                context.Response.BinaryWrite(content);
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.Status = "404 Not Found";
                context.Response.SuppressContent = true;
            }
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