using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using Fuju.Configuration;

using Entities;
using Helpers;

namespace eSchoolWeb.B
{
    public partial class B2300001D : BasePage
    {
        #region Property
        /// <summary>
        /// 編輯模式參數
        /// </summary>
        private string Action
        {
            get
            {
                return ViewState["ACTION"] as string;
            }
            set
            {
                ViewState["ACTION"] = value == null ? null : value.Trim().ToUpper();
            }
        }

        private string KeepJobCubeNo
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["KeepJobCubeNo"] as string);
            }
            set
            {
                ViewState["KeepJobCubeNo"] = value == null ? null : value.Trim();
            }
        }

        private string KeepStamp
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["KeepStamp"] as string);
            }
            set
            {
                ViewState["KeepStamp"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 處理參數
                KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                if (QueryString == null || QueryString.Count == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    return;
                }
                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.KeepJobCubeNo = QueryString.TryGetValue("Jno", String.Empty);
                this.KeepStamp = QueryString.TryGetValue("Stamp", String.Empty);

                int value = 0;
                if (this.Action != ActionMode.View
                    || String.IsNullOrEmpty(this.KeepJobCubeNo)
                    || !Int32.TryParse(this.KeepJobCubeNo, out value)
                    || String.IsNullOrEmpty(this.KeepStamp))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }
                #endregion

                this.labStamp.Text = this.KeepStamp;
            }
        }

        protected void lbtnQuery_Click(object sender, EventArgs e)
        {
            JobcubeEntity jobcube = null;
            Expression where = new Expression(JobcubeEntity.Field.Jno, KeepJobCubeNo);
            XmlResult xmlResult = DataProxy.Current.SelectFirst<JobcubeEntity>(this.Page, where, null, out jobcube);
            if (!xmlResult.IsSuccess)
            {
                this.labList.Text = "查詢失敗，錯誤訊息：" + xmlResult.Message;
                return;
            }

            if (jobcube.Jresultid == JobCubeResultCodeTexts.FAILURE)
            {
                this.labList.Text = "產生 PDF 檔失敗，錯誤訊息：" + jobcube.Memo;
                return;
            }
            else if (jobcube.Jresultid == JobCubeResultCodeTexts.SUCCESS)
            {
                int count = 0;
                Expression where2 = new Expression(BankpmEntity.Field.Filename, RelationEnum.Like, this.KeepStamp + "_%");
                xmlResult = DataProxy.Current.SelectCount<BankpmEntity>(this.Page, where2, out count);
                if (xmlResult.IsSuccess)
                {
                    if (count == 0)
                    {
                        this.labList.Text = "查詢無檔案清單資料";
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<div>");
                        for (int no = 1; no <= count; no++)
                        {
                            string k = String.Format("{0}_{1}", this.KeepStamp, no);
                            sb.AppendFormat("<a href='{0}' target='_DW'>下載檔案 {1}</a>", this.GetResolveUrl("~/api/DownloadHandler.ashx?T=1&K=" + k), no).AppendLine("<br/>");
                        }
                        sb.AppendLine("</div>");
                        this.labList.Text = sb.ToString();
                        this.lbtnQuery.Visible = false;
                    }
                    return;
                }
                else
                {
                    this.labList.Text = "查詢檔案清單失敗，錯誤訊息：" + xmlResult.Message;
                    return;
                }
            }
            else if (jobcube.Jresultid == JobCubeResultCodeTexts.WAIT)
            {
                this.labList.Text = "檔案產生中，請耐心等候";
                return;
            }
            else
            {
                this.labList.Text = JobCubeResultCodeTexts.GetText(jobcube.Jresultid);
                return;
            }
        }
    }
}