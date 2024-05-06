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

namespace eSchoolWeb.B
{
    /// <summary>
    /// 查詢繳費金額及銷帳編號之結果 (明細)
    /// </summary>
    public partial class B2200003D : BasePage
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

        /// <summary>
        /// 儲存業務別碼代碼的查詢條件
        /// </summary>
        private string QueryJno
        {
            get
            {
                return ViewState["QueryJno"] as string;
            }
            set
            {
                ViewState["QueryJno"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.labReceiveType.Text = string.Empty;
            this.labYearId.Text = string.Empty;
            this.labTermId.Text = string.Empty;
            this.labJmethod.Text = string.Empty;
            this.labOwner.Text = string.Empty;
            this.labC_Date.Text = string.Empty;
            //this.labDepId.Text = string.Empty;
            this.labReceiveId.Text = string.Empty;
            this.labSerior_No.Text = string.Empty;
            this.labStatus.Text = string.Empty;
            this.labLog.Text = string.Empty;
        }

        /// <summary>
        /// 結繫明細資料
        /// </summary>
        /// <param name="data">明細資料</param>
        private void BindViewData(JobCubeView data)
        {
            if (data == null)
            {
                this.labReceiveType.Text = string.Empty;
                this.labYearId.Text = string.Empty;
                this.labTermId.Text = string.Empty;
                this.labJmethod.Text = string.Empty;
                this.labOwner.Text = string.Empty;
                this.labC_Date.Text = string.Empty;
                //this.labDepId.Text = string.Empty;
                this.labReceiveId.Text = string.Empty;
                this.labSerior_No.Text = string.Empty;
                this.labStatus.Text = string.Empty;
                this.labLog.Text = string.Empty;
            }
            else
            {
                this.labReceiveType.Text = data.Jrid;
                this.labYearId.Text = String.Format("{0}({1})", data.YearName, data.Jyear);
                this.labTermId.Text = String.Format("{0}({1})", data.TermName, data.Jterm);
                this.labJmethod.Text = JobCubeTypeCodeTexts.GetText(data.Jtypeid);
                this.labOwner.Text = data.Jowner;
                this.labC_Date.Text = data.CDate == null ? String.Empty : data.CDate.Value.ToString("yyyy/MM/dd HH:mm:ss");
                //this.labDepId.Text = String.Format("{0}({1})", data.DepName, data.Jdep);
                this.labReceiveId.Text = String.Format("{0}({1})", data.ReceiveName, data.Jrecid);
                this.labSerior_No.Text = data.SeriorNo;
                this.labStatus.Text = JobCubeStatusCodeTexts.GetText(data.Jstatusid);
                this.labLog.Text = "<pre>" + data.Jlog + "</pre>";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查查詢權限
                if (!this.HasQueryAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無查詢權限");
                    return;
                }
                #endregion

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
                this.QueryJno = QueryString.TryGetValue("Jno", String.Empty);
                if (String.IsNullOrEmpty(this.QueryJno)
                    || this.Action != ActionMode.View)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }
                #endregion

                #region 取得資料
                JobCubeView data = null;
                {
                    string action = this.GetLocalized("查詢明細資料");

                    #region 查詢條件
                    Expression where = new Expression(JobCubeView.Field.Jno, this.QueryJno);
                    #endregion

                    #region 查詢資料
                    XmlResult xmlResult = DataProxy.Current.SelectFirst<JobCubeView>(this, where, null, out data);
                    if (!xmlResult.IsSuccess)
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        return;
                    }
                    if (data == null)
                    {
                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                        return;
                    }
                    #endregion
                }
                #endregion

                this.BindViewData(data);
            }
        }
    }
}