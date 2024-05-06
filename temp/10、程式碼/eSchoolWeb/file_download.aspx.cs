using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Fuju;
using Fuju.DB;
using Fuju.Web;
using Entities;

namespace eSchoolWeb
{
    public partial class file_download : LocalizedPage  //System.Web.UI.Page, IMenuPage
    {
        #region Override LocalizedPage's IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "file_download";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "檔案下載";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public override bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public override bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public override bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        private bool getFilePool(out FilePoolView[] file_pools, out string msg)
        {
            bool rc = false;
            file_pools = null;
            msg = "";

            Expression where = new Expression(FilePoolView.Field.Status, DataStatusCodeTexts.NORMAL);
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(FilePoolView.Field.Sn, OrderByEnum.Asc);

            XmlResult xmlResult = DataProxy.Current.SelectAll<FilePoolView>(this.Page, where, orderbys, out file_pools);
            if (!xmlResult.IsSuccess)
            {
                msg = string.Format("[getFilePool] 取得檔案清單發生錯誤，錯誤訊息={0}", xmlResult.Message);
            }
            else
            {
                rc = true;
            }

            return rc;
        }

        //private void ResponseFile(string fileName, byte[] content)
        //{
        //    string browser = Context.Request.Browser.Browser.ToUpper();
        //    if (browser == "IE" || browser == "INTERNETEXPLORER")
        //    {
        //        fileName = HttpUtility.UrlPathEncode(fileName);
        //    }
        //    this.Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        //    this.Context.Response.AddHeader("Content-Language", "utf-8");
        //    this.Context.Response.ContentType = this.GetContentType(Path.GetExtension(fileName));
        //    this.Context.Response.BinaryWrite(content);
        //}

        //private string GetContentType(string extName)
        //{
        //    extName = extName == null ? string.Empty : extName.Trim().ToUpper();
        //    switch (extName)
        //    {
        //        case "PDF":
        //            return "application/pdf";
        //        case "TXT":
        //            return "application/txt";
        //        case "XLS":
        //            return "application/vnd.ms-excel";
        //        case "MDB":
        //            return "application/vnd.ms-access";
        //        case "DOC":
        //            return "application/msword";
        //    }
        //    return "application/octet-stream";
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                FilePoolView[] file_pools = null;
                string msg = "";
                if (getFilePool(out file_pools,out msg))
                {
                    this.gvResult.DataSource = file_pools;
                    this.gvResult.DataBind();
                }
                
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region 處理資料參數
            #region [Old]
            //string argument = e.CommandArgument as string;
            //if (String.IsNullOrEmpty(argument))
            //{
            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("取無法取得要處理資料的參數")).AppendLine();

            //    ClientScriptManager cs = this.ClientScript;
            //    Type myType = this.GetType();
            //    if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
            //    {
            //        cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
            //    }
            //    return;
            //}
            //string[] args = argument.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //if (args.Length != 3)
            //{
            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("取無法取得要處理資料的參數")).AppendLine();

            //    ClientScriptManager cs = this.ClientScript;
            //    Type myType = this.GetType();
            //    if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
            //    {
            //        cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
            //    }
            //    return;
            //}
            //string qual = args[0];
            //string url = args[1];
            //string file_name = args[2];
            #endregion

            int sn = 0;
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument) || !Int32.TryParse(argument.Trim(), out sn) || sn < 1)
            {
                StringBuilder js = new StringBuilder();
                js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("取無法取得要處理資料的參數")).AppendLine();

                ClientScriptManager cs = this.ClientScript;
                Type myType = this.GetType();
                if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                {
                    cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                }
                return;
            }
            #endregion

            switch (e.CommandName)
            {
                case ButtonCommandName.Download:
                    #region 下載檔案
                    {
                        #region [Old]
                        //if(qual=="1")
                        //{
                        //    string s = "window.open('" + url + "', 'popup_window', 'width=800,height=500,left=200,top=100,resizable=yes');";
                        //    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                        //}
                        //else
                        //{
                        //    WebClient wc = new WebClient();
                        //    byte[] byteFile = null;
                        //    string path = Server.MapPath("~/file_pool");
                        //    string full_file_name = @"C:\jobs\土銀\學雜費\程式碼\eSchoolWeb\file_pool\" + file_name;
                        //    byteFile = wc.DownloadData(full_file_name);
                        //    Response.AppendHeader("content-disposition", string.Format("attachment;filename={0}", HttpContext.Current.Server.UrlEncode(file_name)));
                        //    Response.ContentType = "application/octet-stream";
                        //    Response.BinaryWrite(byteFile);
                        //    Response.End();
                        //}
                        #endregion

                        FilePoolEntity data = null;
                        Expression where = new Expression(FilePoolEntity.Field.Sn, sn);
                        XmlResult xmlResult = DataProxy.Current.SelectFirst<FilePoolEntity>(this.Page, where, null, out data);
                        if (xmlResult.IsSuccess)
                        {
                            if (data == null || data.File == null || data.File.Length == 0)
                            {
                                StringBuilder js = new StringBuilder();
                                js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("檔案不存在")).AppendLine();

                                ClientScriptManager cs = this.ClientScript;
                                Type myType = this.GetType();
                                if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                                {
                                    cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                                }
                            }
                            else
                            {
                                this.ResponseFile(data.FileName, data.File);
                            }
                        }
                        else
                        {
                            StringBuilder js = new StringBuilder();
                            js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("讀取檔案失敗，請稍後再試")).AppendLine();

                            ClientScriptManager cs = this.ClientScript;
                            Type myType = this.GetType();
                            if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                            {
                                cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                            }
                        }
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            FilePoolView[] datas = this.gvResult.DataSource as FilePoolView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                FilePoolView data = datas[row.RowIndex];
                //資料參數

                string qual = data.FileQual;

                #region [Old]
                //string file_name = data.FileName;
                //string arg = string.Format("{0},{1},{2}",qual,url,file_name);
                #endregion

                MyDownloadButton ccbtnDownload = row.FindControl("ccbtnDownload") as MyDownloadButton;
                if (ccbtnDownload != null)
                {
                    if (qual == "1")
                    {
                        string url = data.Url;
                        ccbtnDownload.OnClientClick = String.Format("javascript:window.open('{0}'); return false;", url);
                    }
                    else
                    {
                        ccbtnDownload.CommandArgument = data.Sn.ToString();
                    }
                }
            }
        }
    }
}