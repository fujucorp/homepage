using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.S
{
    /// <summary>
    /// 報稅資料產生
    /// </summary>
    public partial class S5500005 : BasePage
    {
        /// <summary>
        /// 初始化使用介面
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
            tbxSchoolId.Text = string.Empty;
            tbxSchoolIdenty.Text = string.Empty;
            tbxTaxYear.Text = string.Empty;

            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
                return false;
            }
            #endregion

            LogonUser logonUser = this.GetLogonUser();
            if (!logonUser.IsSchoolUser)
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "此功能僅限學校使用");
                return false;
            }

            return this.GetDataAndBind();
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetDataAndBind()
        {
            LogonUser logonUser = this.GetLogonUser();

            Expression where = new Expression(SchoolRTypeEntity.Field.SchIdenty, logonUser.BankId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);

            string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.SchIdenty };
            string codeCombineFormat = null;
            string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
            string textCombineFormat = null;

            CodeText[] datas = null;
            XmlResult xmlResult = DataProxy.Current.GetEntityOptions<SchoolRTypeEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = this.GetLocalized("查詢學校資料");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }

            WebHelper.SetCheckBoxListItems(this.cblReceiveType, datas, false, 2, null);

            return xmlResult.IsSuccess;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ccbtnOK.Visible = this.InitialUI();
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region 商家代碼
            List<string> receiveTypes = new List<string>();
            foreach (ListItem item in this.cblReceiveType.Items)
            {
                if (item.Selected)
                {
                    receiveTypes.Add(item.Value);
                }
            }
            if (receiveTypes.Count == 0)
            {
                this.ShowMustInputAlert("商家代碼");
                return;
            }
            #endregion

            #region 報稅年度
            string taxYear = this.tbxTaxYear.Text.Trim();
            if (String.IsNullOrEmpty(taxYear))
            {
                this.ShowMustInputAlert("報稅年度");
                return;
            }
            if (!Common.IsNumber(taxYear, 3))
            {
                this.ShowSystemMessage("報稅年度限輸入3碼的民國年數字");
                return;
            }
            #endregion

            #region 學校代號
            string schoolId = this.tbxSchoolId.Text.Trim();
            if (String.IsNullOrEmpty(schoolId))
            {
                this.ShowMustInputAlert("學校代號");
                return;
            }
            #endregion

            #region 學校統編
            string schoolIdenty = this.tbxSchoolIdenty.Text.Trim();
            if (String.IsNullOrEmpty(schoolIdenty))
            {
                this.ShowMustInputAlert("學校統編");
                return;
            }
            #endregion

            #region 學制
            string schLevel = this.ddlSchLevel.SelectedValue;
            if (String.IsNullOrEmpty(schLevel))
            {
                this.ShowMustInputAlert("學制");
                return;
            }
            #endregion

            #region 新增 JobCube
            {
                JobcubeEntity jobCube = new JobcubeEntity();
                jobCube.Jdll = String.Empty;
                jobCube.Jclass = String.Empty;
                jobCube.Jmethod = String.Empty;
                //sch_pid(統編),sch_id(學校代號),y(民國年),receive_types(用 ; 分開),sch_level(學制),proc_date(yyyyMMdd)
                jobCube.Jparam = string.Format("{0}, {1}, {2}, {3}, {4}, {5}", schoolIdenty, schoolId, taxYear, String.Join(";", receiveTypes.ToArray()), schLevel, DateTime.Now.ToString("yyyyMMdd"));
                jobCube.Jowner = this.GetLogonUser().UserId;
                jobCube.Jrid = string.Empty;
                jobCube.Jyear = string.Empty;
                jobCube.Jterm = string.Empty;
                jobCube.Jdep = string.Empty;
                jobCube.Jrecid = string.Empty;
                jobCube.Jprity = 0;
                jobCube.Jtypeid = "SGG";

                jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
                jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
                jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
                jobCube.CDate = DateTime.Now;
                jobCube.SeriorNo = string.Empty;
                jobCube.Memo = String.Empty;
                jobCube.Chancel = String.Empty;

                int count = 0;
                XmlResult xmlResult = DataProxy.Current.Insert<JobcubeEntity>(this.Page, jobCube, out count);

                if (xmlResult.IsSuccess)
                {
                    string msg = this.GetLocalized("新增報稅資料排程工作成功");
                    this.ShowSystemMessage(msg);
                }
                else
                {
                    string action = this.GetLocalized("新增報稅資料排程工作");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion
        }
    }
}