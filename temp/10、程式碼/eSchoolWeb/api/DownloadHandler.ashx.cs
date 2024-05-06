using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.Services;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.api
{
    /// <summary>
    /// DownloadHandler 的摘要描述
    /// </summary>
    public class DownloadHandler : IHttpHandler, IRequiresSessionState, IMenuPage
    {
        #region Implement IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public virtual string MenuID
        {
            get
            {
                return "Download";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public virtual string MenuName
        {
            get
            {
                return "下載檔案";
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
            string t = context.Request.QueryString["T"];
            if (t == "1")
            {
                string k = context.Request.QueryString["K"];
                if (String.IsNullOrEmpty(k))
                {
                    this.ResponseError("參數錯誤");
                    return;
                }
                this.DownloadBankPM(k + ".PDF");
                return;
            }

            string errmsg = string.Empty;
            string methodName = HttpUtility.HtmlEncode(context.Request.QueryString["methodName"]);
            string Id = HttpUtility.HtmlEncode(context.Request.QueryString["Id"]);

            #region [MDY:20210401] 原碼修正
            string fileExtName = HttpUtility.UrlEncode(context.Request.QueryString["fileExtName"]);
            #endregion

            methodName = methodName == null ? string.Empty : methodName.Trim();
            Id = Id == null ? string.Empty : Id.Trim();
            fileExtName = fileExtName == null ? string.Empty : fileExtName.Trim();
            #endregion

            switch (methodName)
            {
                case DownloadFileMethodCode.BILLFORM:        //繳費單模板管理
                    System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
                    errmsg = this.PreviewBillForm(page, Id, fileExtName);
                    if (errmsg.Length > 0)
                    {
                        this.ResponseError(errmsg);
                    }
                    break;
            }

        }

        /// <summary>
        /// 繳費單模板管理
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="billFormId">模板代碼</param>
        /// <param name="fileExtName">開啟的檔案副檔名</param>
        /// <returns></returns>
        private string PreviewBillForm(System.Web.UI.Page page, string billFormId, string fileExtName)
        {
            string errmsg = string.Empty;

            Expression where = new Expression(BillFormEntity.Field.BillFormId, billFormId);

            BillFormEntity data = null;
            XmlResult xmlResult = DataProxy.Current.SelectFirst<BillFormEntity>(page, where, null, out data);
            if (xmlResult.IsSuccess)
            {
                if (data != null && data.BillFormImage != null && data.BillFormImage.Length > 0)
                {
                    byte[] content = data.BillFormImage;
                    this.ResponseFile(this.CombineExtName(data.BillFormName, fileExtName), content);
                }
                else
                {
                    errmsg = "檔案不存在 ...";
                }
            }

            return errmsg;
        }

        private void DownloadBankPM(string filename)
        {
            string errmsg = string.Empty;
            System.Web.UI.Page page = this.Context.Handler as System.Web.UI.Page;
            byte[] content = null;
            XmlResult xmlResult = DataProxy.Current.GetBankPMTempFile(page, filename, out content);
            if (xmlResult.IsSuccess)
            {
                if (content != null)
                {
                    string fileName = "繳費單.PDF";
                    this.ResponseFile(fileName, content);
                }
                else
                {
                    this.ResponseError("檔案不存在 ...");
                }
            }
            return;
        }

        private void ResponseFile(string fileName, byte[] content)
        {
            string browser = Context.Request.Browser.Browser.ToUpper();
            if (browser == "IE" || browser == "INTERNETEXPLORER")
            {
                fileName = HttpUtility.UrlPathEncode(fileName);
            }
            this.Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            this.Context.Response.AddHeader("Content-Language", "utf-8");
            this.Context.Response.ContentType = this.GetContentType(Path.GetExtension(fileName));
            this.Context.Response.BinaryWrite(content);
        }

        private void ResponseError(string errmsg)
        {
            this.Context.Response.ContentType = "text/plain";
            this.Context.Response.Write(errmsg);
        }

        private void ResponseWaiting(string msg, int reloadSecond, string downloadID, string qJobID)
        {
            this.Context.Response.ContentType = "text/html";
            msg = msg == null || msg.Length == 0 ? "檔案處理中，請稍後 ..." : msg.Trim();
            StringBuilder html = new StringBuilder();
            html
                .AppendLine("<html>")
                .AppendLine("<head></head>")
                .AppendLine("<body>");

            html.AppendLine(msg);
            if (reloadSecond > 0)
            {
                //string reloadUrl = String.Format("{0}&Txx={1}", Context.Request.Url.AbsoluteUri, DateTime.Now.Ticks);
                string reloadUrl = String.Format("{0}?id={1}&QJID={2}&Txx={3}", Context.Request.Url.AbsolutePath, downloadID, qJobID, DateTime.Now.Ticks);
                html
                    .AppendLine("<script type='text/javascript'>")
                    .AppendFormat("	window.setTimeout(\"window.location.href='{1}'\", {0});", reloadSecond * 1000, reloadUrl)
                    .AppendLine("</script>");
            }
            html
                .AppendLine("</body>")
                .AppendLine("</html>");
            this.Context.Response.Write(html.ToString());
        }

        private void ResponseDownloadHtml(string downloadID)
        {
            this.Context.Response.ContentType = "text/html";
            StringBuilder html = new StringBuilder();
            html
                .AppendLine("<html>")
                .AppendLine("<head></head>")
                .AppendLine("<body>");

            string url = String.Format("{0}?id={1}&Txx={2}", Context.Request.Url.AbsolutePath, downloadID, DateTime.Now.Ticks);
            html
                .AppendLine("<script type='text/javascript'>")
                .AppendLine("function openDownload() { ")
                .AppendFormat("	window.open('{0}');", url).AppendLine()
                .AppendLine("} ")
                .AppendLine("openDownload();")
                .AppendLine("</script>");

            html.AppendLine("檔案已處理完成，程式將自動下載<br/>如果無法自動下載，請點此 <a href=\"javascript:void(0);\" onclick=\"openDownload();\">下載</a> 連結");

            html
                .AppendLine("</body>")
                .AppendLine("</html>");
            this.Context.Response.Write(html.ToString());
        }

        private string GetContentType(string extName)
        {
            extName = extName == null ? string.Empty : extName.Trim().ToUpper();
            switch (extName)
            {
                case "PDF":
                    return "application/pdf";
                case "TXT":
                    return "application/txt";
                case "XLS":
                    return "application/vnd.ms-excel";
                case "MDB":
                    return "application/vnd.ms-access";
                case "DOC":
                    return "application/msword";
            }
            return "application/octet-stream";
        }

        private string CombineExtName(string fileName, string extName)
        {
            fileName = fileName == null ? String.Empty : fileName.Trim();
            extName = extName == null ? String.Empty : extName.Trim();
            if (fileName.Length > 0 && extName.Length > 0)
            {
                if (extName.StartsWith("."))
                {
                    return String.Concat(fileName, extName);
                }
                else
                {
                    return String.Concat(fileName, ".", extName);
                }
            }
            return fileName + extName;
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