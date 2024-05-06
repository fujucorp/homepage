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
    /// 產生繳費單
    /// </summary>
    public partial class B2300001 : BasePage
    {
        #region Const
        private const string _JobCubeTypeId = JobCubeTypeCodeTexts.PDFB;
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
            if (!this.HasMaintainAuth())
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

            #region 科系
            this.GetAndBindMajorOptions(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId);
            #endregion

            #region 年級
            {
                WebHelper.SetDropDownListItems(this.ddlGrade, DefaultItem.Kind.Select, false, new GradeCodeTexts(), false, false, 0, null);
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
        /// 取得並結繫科系選項
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        private void GetAndBindMajorOptions(string receiveType, string yearId, string termId, string depId)
        {
            #region 取資料
            CodeText[] items = null;

            #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
            //if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) && !String.IsNullOrEmpty(termId)
            //    && !String.IsNullOrEmpty(depId))
            #endregion

            if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) && !String.IsNullOrEmpty(termId))
            {
                Expression where = new Expression(MajorListEntity.Field.ReceiveType, receiveType)
                    .And(MajorListEntity.Field.YearId, yearId)
                    .And(MajorListEntity.Field.TermId, termId)
                    .And(MajorListEntity.Field.DepId, depId)
                    .And(MajorListEntity.Field.MajorId, RelationEnum.NotEqual, String.Empty);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(MajorListEntity.Field.MajorId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { MajorListEntity.Field.MajorId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { MajorListEntity.Field.MajorName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<MajorListEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢科系資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            WebHelper.SetDropDownListItems(this.ddlMajor, DefaultItem.Kind.Select, false, items, false, false, 0, null);
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
            #endregion

            string qType = null;
            string startSNo = null;
            string endSNo = null;
            string upNo = null;
            string studentId = null;
            string majorId = null;
            string stuGrade = null;
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
                Int64 value = 0;
                if (!Int64.TryParse(startSNo, out value) || !Int64.TryParse(endSNo, out value))
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
            else if (this.rbtRange4.Checked)
            {
                //依科系產生
                majorId = this.ddlMajor.SelectedValue;
                if (String.IsNullOrEmpty(majorId))
                {
                    this.ShowMustInputAlert("科系");
                    return;
                }
                qType = "5";
            }
            else if (this.rbtRange5.Checked)
            {
                //依科系產生
                stuGrade = this.ddlGrade.SelectedValue;
                if (String.IsNullOrEmpty(stuGrade))
                {
                    this.ShowMustInputAlert("年級");
                    return;
                }
                qType = "6";
            }
            else
            {
                string msg = this.GetLocalized("請設定產生繳費單的條件");
                this.ShowSystemMessage(msg);
                return;
            }

            bool allAmount = this.cbxAllAmount.Checked;

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

                #region [Old]
                //jobCube.Jparam = String.Format("{0},{1},{2},{3},{4},{5},{6}", receiveType, yearId, termId, depId, receiveId, qType, startSNo, endSNo, upNo, studentId);
                //增加依科系產生
                //jobCube.Jparam = JobcubeEntity.JoinPDFxParameter(receiveType, yearId, termId, depId, receiveId, qType, startSNo, endSNo, upNo, studentId, majorId);
                #endregion

                #region [MDY:202203XX] 2022擴充案 是否英文介面
                //增加依年級產生、所有金額的資料都要產生
                jobCube.Jparam = JobcubeEntity.JoinPDFxParameter(receiveType, yearId, termId, depId, receiveId, qType, startSNo, endSNo, upNo, studentId, majorId, stuGrade, allAmount, isEngUI);
                #endregion

                jobCube.Jowner = this.GetLogonUser().UserId;
                jobCube.Jrid = receiveType;
                jobCube.Jyear = yearId;
                jobCube.Jterm = termId;
                jobCube.Jdep = depId;
                jobCube.Jrecid = receiveId;
                jobCube.Jprity = 0;
                jobCube.Jtypeid = _JobCubeTypeId;

                jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
                jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
                jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
                jobCube.CDate = DateTime.Now;
                jobCube.SeriorNo = String.Empty;
                jobCube.Memo = String.Empty;
                jobCube.Chancel = String.Empty;

                #region [Old]
                //byte[] pdfContent = null;
                //XmlResult xmlResult = DataProxy.Current.InsertJobCubeForPDF(this.Page, jobCube, out pdfContent);
                //if (xmlResult.IsSuccess)
                //{
                //    string fileName = String.Format("{0}_{1}_{2}_{3}_{4}繳費單.PDF", receiveType, yearId, termId, depId, receiveId);
                //    this.ResponseFile(fileName, pdfContent);
                //}
                //else
                //{
                //    string action = this.GetLocalized("新增產生繳費單工作");
                //    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                //}
                #endregion

                string jno = null;
                string stamp = null;
                XmlResult xmlResult = DataProxy.Current.InsertJobCubeForPDF(this.Page, jobCube, out jno, out stamp);
                if (xmlResult.IsSuccess)
                {
                    KeyValueList<string> QueryString = new KeyValueList<string>();
                    QueryString.Add("Action", ActionMode.View);
                    QueryString.Add("Jno", jno);
                    QueryString.Add("Stamp", stamp);
                    Session["QueryString"] = QueryString;

                    #region [MDY:20210521] 原碼修正
                    Server.Transfer(WebHelper.GenRNUrl("B2300001D.aspx"));
                    #endregion
                }
                else
                {
                    string action = this.GetLocalized("新增產生繳費單工作");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion
        }
    }
}