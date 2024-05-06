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
    /// <summary>
    /// 產生繳費收據
    /// </summary>
    public partial class B2400001 : BasePage
    {
        #region Const
        private const string _JobCubeTypeId = JobCubeTypeCodeTexts.PDFR;
        #endregion

        #region Property
        /// <summary>
        /// 儲存業務別碼代碼的查詢條件
        /// </summary>
        private string EditReceiveType
        {
            get
            {
                return ViewState["EditReceiveType"] as string;
            }
            set
            {
                ViewState["EditReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存年度代碼的查詢條件
        /// </summary>
        private string EditYearId
        {
            get
            {
                return ViewState["EditYearId"] as string;
            }
            set
            {
                ViewState["EditYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存期別代碼的查詢條件
        /// </summary>
        private string EditTermId
        {
            get
            {
                return ViewState["EditTermId"] as string;
            }
            set
            {
                ViewState["EditTermId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存部門別代碼的查詢條件
        /// </summary>
        private string EditDepId
        {
            get
            {
                return ViewState["EditDepId"] as string;
            }
            set
            {
                ViewState["EditDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存費用別代碼的查詢條件
        /// </summary>
        private string EditReceiveId
        {
            get
            {
                return ViewState["EditReceiveId"] as string;
            }
            set
            {
                ViewState["EditReceiveId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region 檢查維護權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return false;
            }
            #endregion

            string receiveType = null;
            string yearId = null;
            string termId = null;
            string depId = null;
            string receiveId = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearId, out termId, out depId, out receiveId)
                || String.IsNullOrEmpty(receiveType)
                || (this.ucFilter1.YearVisible && String.IsNullOrEmpty(yearId))
                || (this.ucFilter1.TermVisible && String.IsNullOrEmpty(termId)))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
                this.ShowSystemMessage(msg);
                return false;
            }

            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
            if (xmlResult.IsSuccess)
            {
                this.EditReceiveType = receiveType;
                this.EditYearId = yearId;
                this.EditTermId = termId;
                this.EditDepId = "";
                this.EditReceiveId = this.ucFilter2.SelectedReceiveID;
            }

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                this.ccbtnOK.Visible = false;
                return false;
            }
            #endregion

            this.ucFilter2_ItemSelectedIndexChanged(this.ucFilter2, null);

            #region [MDY:202203XX] 2022擴充案 是否英文介面
            bool isEngEabled = this.IsEngEabled(receiveType, !this.IsPostBack);
            if (isEngEabled)
            {
                this.trLang.Visible = true;
                CodeText[] items = new CodeText[]
                {
                    new CodeText("CHT", "中文版"),
                    new CodeText("ENG", "英文版")
                };
                WebHelper.SetRadioButtonListItems(this.rblLang, items, true, 2, "CHT");
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 取得並結繫上傳批號選項
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        private void GetAndBindUpNoOptions(string receiveType, string yearId, string termId, string depId, string receiveId)
        {
            #region 取資料
            CodeText[] items = null;

            #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
            //if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) && !String.IsNullOrEmpty(termId)
            //    && !String.IsNullOrEmpty(depId) && !String.IsNullOrEmpty(receiveId))
            #endregion

            if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) 
                && !String.IsNullOrEmpty(termId) && !String.IsNullOrEmpty(receiveId))
            {
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                    .And(StudentReceiveEntity.Field.YearId, yearId)
                    .And(StudentReceiveEntity.Field.TermId, termId)
                    .And(StudentReceiveEntity.Field.DepId, depId)
                    .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                    .And(StudentReceiveEntity.Field.UpNo, RelationEnum.NotEqual, String.Empty);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(StudentReceiveEntity.Field.UpNo, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { StudentReceiveEntity.Field.UpNo };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { StudentReceiveEntity.Field.UpNo };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<StudentReceiveEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢上傳批號資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            #region [MDY:2018xxxx] 批號改用數值遞減排序
            if (items != null)
            {
                WebHelper.SortItemsByValueDesc(ref items);
            }
            #endregion
            WebHelper.SetDropDownListItems(this.ddlUpNo, DefaultItem.Kind.Select, false, items, false, false, 0, null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ccbtnOK.Visible = this.InitialUI();
            }
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            this.EditDepId = "";
            this.EditReceiveId = this.ucFilter2.SelectedReceiveID;
            this.GetAndBindUpNoOptions(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);

            this.ccbtnOK.Enabled = (this.ddlUpNo.Items.Count > 0);
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region 5 Key
            string receiveType = this.EditReceiveType;
            string yearId = this.EditYearId;
            string termId = this.EditTermId;
            string depId = this.EditDepId;
            string receiveId = this.EditReceiveId;
            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("業務別碼");
                return;
            }
            if (String.IsNullOrEmpty(yearId))
            {
                this.ShowMustInputAlert("學年");
                return;
            }
            if (String.IsNullOrEmpty(termId))
            {
                this.ShowMustInputAlert("學期");
                return;
            }
            //if (String.IsNullOrEmpty(depId))
            //{
            //    this.ShowMustInputAlert("部別");
            //    return;
            //}
            if (String.IsNullOrEmpty(receiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return;
            }

            #region 檢查不允許同時設定多筆未處理的 5 Key 資料
            {
                int count = 0;
                Expression where = new Expression(JobcubeEntity.Field.Jtypeid, _JobCubeTypeId)
                    .And(JobcubeEntity.Field.Jrid, receiveType)
                    .And(JobcubeEntity.Field.Jyear, yearId)
                    .And(JobcubeEntity.Field.Jterm, termId)
                    .And(JobcubeEntity.Field.Jdep, depId)
                    .And(JobcubeEntity.Field.Jrecid, receiveId)
                    .And(JobcubeEntity.Field.Jresultid, JobCubeResultCodeTexts.GetUnEndingCode())
                    .And(JobcubeEntity.Field.Jstatusid, JobCubeStatusCodeTexts.GetUnEndingCode());
                XmlResult xmlResult = DataProxy.Current.SelectCount<JobcubeEntity>(this.Page, where, out count);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢相同未處理資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    return;
                }
                if (count > 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("排程工作已經存在或在工作中、請勿重新執行");
                    this.ShowSystemMessage(msg);
                    return;
                }
            }
            #endregion

            #region 取得優先次序編號
            int schPri = 0;
            {
                SchoolRTypeEntity school = null;
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this.Page, where, null, out school);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢相同未處理資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    return;
                }
                if (school == null)
                {
                    //[TODO] 固定顯示訊息的收集
                    string action = this.GetLocalized("查詢業務別碼");
                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                    return;
                }
                if (!int.TryParse(school.SchPri, out schPri))
                {
                    schPri = 0;
                }
            }
            #endregion
            #endregion

            string qType = null;
            string startSNo = null;
            string endSNo = null;
            string upNo = null;
            string studentId = null;
            if (this.rbtRange0.Checked)
            {
                //產生所有繳費單
                qType = "1";
            }
            else if (this.rbtRange1.Checked)
            {
                //自訂產生繳費單流水號
                startSNo = this.tbxSeriorNoStart.Text.Trim();
                endSNo = this.tbxSeriorNoEnd.Text.Trim();
                if (String.IsNullOrEmpty(startSNo) || String.IsNullOrEmpty(endSNo))
                {
                    this.ShowMustInputAlert("自訂產生繳費單流水號");
                    return;
                }
                int value = 0;
                if (!int.TryParse(startSNo, out value) || !int.TryParse(endSNo, out value))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「自訂產生繳費單流水號」只能輸入數字");
                    this.ShowSystemMessage(msg);
                    return;
                }
                qType = "2";
            }
            else if (this.rbtRange2.Checked)
            {
                //依批號產生
                upNo = this.ddlUpNo.SelectedValue;
                if (String.IsNullOrEmpty(upNo))
                {
                    this.ShowMustInputAlert("批號");
                    return;
                }
                qType = "3";
            }
            else if (this.rbtRange3.Checked)
            {
                //依學號產生
                studentId = this.tbxStudentId.Text.Trim();
                if (String.IsNullOrEmpty(studentId))
                {
                    this.ShowMustInputAlert("學號");
                    return;
                }
                qType = "4";
            }
            else
            {
                string msg = this.GetLocalized("請設定產生繳費單的條件");
                this.ShowSystemMessage(msg);
                return;
            }

            #region [MDY:202203XX] 2022擴充案 是否英文介面
            bool isEngUI = false;
            if (this.trLang.Visible)
            {
                isEngUI = "ENG" == this.rblLang.SelectedValue;
            }
            #endregion

            #region 新增 JobCube
            {
                //[TODO] 沒有照舊格式
                JobcubeEntity jobCube = new JobcubeEntity();
                jobCube.Jdll = String.Empty;
                jobCube.Jclass = String.Empty;
                jobCube.Jmethod = String.Empty;

                #region [MDY:202203XX] 2022擴充案 是否英文介面
                #region [OLD]
                //jobCube.Jparam = String.Format("{0},{1},{2},{3},{4},{5},{6}", receiveType, yearId, termId, depId, receiveId, upNo, index);
                #endregion

                jobCube.Jparam = JobcubeEntity.JoinPDFxParameter(receiveType, yearId, termId, depId, receiveId, qType, startSNo, endSNo, upNo, studentId, isEngUI: isEngUI);
                #endregion

                jobCube.Jowner = this.GetLogonUser().UserId;
                jobCube.Jrid = receiveType;
                jobCube.Jyear = yearId;
                jobCube.Jterm = termId;
                jobCube.Jdep = depId;
                jobCube.Jrecid = receiveId;
                jobCube.Jprity = schPri;
                jobCube.Jtypeid = _JobCubeTypeId;

                jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
                jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
                jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
                jobCube.CDate = DateTime.Now;
                jobCube.SeriorNo = String.Empty;
                jobCube.Memo = String.Empty;
                jobCube.Chancel = String.Empty;

                byte[] pdfContent = null;
                XmlResult xmlResult = DataProxy.Current.InsertJobCubeForPDF(this.Page, jobCube, out pdfContent);
                if (xmlResult.IsSuccess)
                {
                    #region [MDY:20210401] 原碼修正
                    string fileName = String.Format("{0}_{1}_{2}_{3}_{4}繳費收據.PDF", HttpUtility.UrlEncode(receiveType), HttpUtility.UrlEncode(yearId), HttpUtility.UrlEncode(termId), HttpUtility.UrlEncode(depId), HttpUtility.UrlEncode(receiveId));
                    #endregion

                    this.ResponseFile(fileName, pdfContent);
                }
                else
                {
                    string action = this.GetLocalized("新增產生繳費收據工作");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion
        }
    }
}