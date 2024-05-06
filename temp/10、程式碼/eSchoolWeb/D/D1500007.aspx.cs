using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.D
{
    /// <summary>
    /// 查閱檔案上傳結果
    /// </summary>
    public partial class D1500007 : BasePage
    {
        #region Property
        /// <summary>
        /// 儲存業務別碼代碼的查詢條件
        /// </summary>
        private string QueryReceiveType
        {
            get
            {
                return ViewState["QueryReceiveType"] as string;
            }
            set
            {
                ViewState["QueryReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存年度代碼的查詢條件
        /// </summary>
        private string QueryYearId
        {
            get
            {
                return ViewState["QueryYearId"] as string;
            }
            set
            {
                ViewState["QueryYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存期別代碼的查詢條件
        /// </summary>
        private string QueryTermId
        {
            get
            {
                return ViewState["QueryTermId"] as string;
            }
            set
            {
                ViewState["QueryTermId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
            string receiveType = null;
            string yearID = null;
            string termID = null;
            string depID = null;
            string ReceiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out ReceiveID)
                || String.IsNullOrEmpty(receiveType)
                || (this.ucFilter1.YearVisible && String.IsNullOrEmpty(yearID))
                || (this.ucFilter1.TermVisible && String.IsNullOrEmpty(termID)))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
                this.ShowSystemMessage(msg);
                return false;
            }

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
            {
                string msg = this.GetErrorLocalized(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearID, termID, depID, ReceiveID);
            if (xmlResult.IsSuccess)
            {
                this.QueryReceiveType = this.ucFilter1.SelectedReceiveType;
                this.QueryYearId = this.ucFilter1.SelectedYearID;
                this.QueryTermId = this.ucFilter1.SelectedTermID;
            }
            else
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
                this.ShowSystemMessage(msg);
                return false;
            }

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
            {
                string msg = this.GetLocalized("該業務別碼未授權");
                this.ShowJsAlert(msg);
                return false;
            }
            #endregion

            return this.GetDataAndBind(receiveType, yearID, termID);
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetDataAndBind(string qReceiveType, string qYearID, string qTermID)
        {
            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
                return false;
            }
            #endregion

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(qReceiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                return false;
            }
            #endregion

            #region 查詢條件
            #region [Old] 增加 BUG 上傳學生基本資料
            //Expression where = new Expression();
            //where
            //    .And(JobCubeView.Field.Jrid, qReceiveType)
            //    .And(JobCubeView.Field.Jyear, qYearID)
            //    .And(JobCubeView.Field.Jterm, qTermID)
            //    .And(JobCubeView.Field.Jtypeid, RelationEnum.In, new string[] { JobCubeTypeCodeTexts.BUA, JobCubeTypeCodeTexts.BUB, JobCubeTypeCodeTexts.BUC, JobCubeTypeCodeTexts.BUD, JobCubeTypeCodeTexts.BUE, JobCubeTypeCodeTexts.BUF });
            #endregion

            #region [New] 增加 BUG 上傳學生基本資料
            Expression w1 = new Expression(JobCubeView.Field.Jyear, qYearID)
                .And(JobCubeView.Field.Jterm, qTermID)
                .And(JobCubeView.Field.Jtypeid, RelationEnum.In, new string[] { JobCubeTypeCodeTexts.BUA, JobCubeTypeCodeTexts.BUB, JobCubeTypeCodeTexts.BUC, JobCubeTypeCodeTexts.BUD, JobCubeTypeCodeTexts.BUE, JobCubeTypeCodeTexts.BUF });
            Expression w2 = new Expression(JobCubeView.Field.Jtypeid, JobCubeTypeCodeTexts.BUG);    //BUG 沒有學年、學期
            Expression where = new Expression(JobCubeView.Field.Jrid, qReceiveType)
                .And(w1.Or(w2));
            #endregion
            #endregion

            #region 排序條件
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(JobCubeView.Field.Jno, OrderByEnum.Desc);
            #endregion

            JobCubeView[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<JobCubeView>(this, where, orderbys, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            //else if (datas == null || datas.Length == 0)
            //{
            //    this.ShowErrorMessage(ErrorCode.D_QUERY_NO_DATA, "查無資料");
            //}

            this.gvResult.DataSource = datas;
            this.gvResult.DataBind();
            return xmlResult.IsSuccess;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            JobCubeView[] datas = this.gvResult.DataSource as JobCubeView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            string action = WebHelper.GetControlLocalizedByResourceKey("Detail", "明細");

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                JobCubeView data = datas[row.RowIndex];
                row.Cells[0].Text = JobCubeTypeCodeTexts.GetText(data.Jtypeid);
                //row.Cells[3].Text = String.Format("{0}({1})", data.DepName, data.Jdep);         //部別
                if (!String.IsNullOrEmpty(data.Jrecid))
                {
                    row.Cells[3].Text = String.Format("{0}({1})", data.ReceiveName, data.Jrecid);   //費用別
                }
                row.Cells[5].Text = JobCubeStatusCodeTexts.GetText(data.Jstatusid);

                //資料參數
                string argument = String.Format("{0}", data.Jno.ToString());

                LinkButton lbtnDetail = row.FindControl("lbtnDetail") as LinkButton;
                if (lbtnDetail != null)
                {
                    lbtnDetail.Text = action;
                    lbtnDetail.CommandArgument = argument;
                }

                //MyLinkButton cclbtnDetail = row.FindControl("cclbtnDetail") as MyLinkButton;
                //if (cclbtnDetail != null)
                //{
                //    cclbtnDetail.CommandArgument = argument;
                //}
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            #region 處理資料參數
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("取無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            string editUrl = "D1500007D.aspx";
            switch (e.CommandName)
            {
                case "Detail":
                    #region 明細
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.View);
                        QueryString.Add("Jno", argument);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion
                    }
                    #endregion
                    break;
            }
        }
    }
}