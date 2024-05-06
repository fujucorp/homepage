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

namespace eSchoolWeb.S
{
    /// <summary>
    /// D38資料上傳
    /// </summary>
    public partial class S5400006 : BasePage
    {
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

            this.tabResult.Visible = false;

            #region 處理五個下拉選項
            string receiveType = null;
            string yearId = null;
            string termId = null;
            string depId = null;
            string receiveId = null;
            WebHelper.GetFilterArguments(out receiveType, out yearId, out termId, out depId, out receiveId);

            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
            if (xmlResult.IsSuccess)
            {
                receiveId = this.ucFilter2.SelectedReceiveID;
            }
            this.EditReceiveType = receiveType;
            this.EditYearId = yearId;
            this.EditTermId = termId;
            this.EditDepId = "";
            this.EditReceiveId = receiveId;
            #endregion

            //#region 檢查商家代號授權
            //if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
            //{
            //    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該商家代號");
            //    this.ccbtnOK.Visible = false;
            //    return false;
            //}
            //#endregion

            #region [MDY:20160307] 增加處理類型 (上傳或刪除) 選擇
            {
                CodeText[] items = new CodeText[2] { 
                    new CodeText(JobcubeEntity.D38UpdKind_Update, "上傳資料至中心"), 
                    new CodeText(JobcubeEntity.D38UpdKind_Delete, "刪除已上傳資料") };
                WebHelper.SetRadioButtonListItems(this.rblUpdKind, items, true, 4, null);
            }
            #endregion

            this.ucFilter2_ItemSelectedIndexChanged(this.ucFilter2, null);

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
            this.EditReceiveType = this.ucFilter1.SelectedReceiveType;
            this.EditYearId = this.ucFilter1.SelectedYearID;
            this.EditTermId = this.ucFilter1.SelectedTermID;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //this.EditDepId = this.ucFilter1.SelectedDepID;
            #endregion

            this.EditDepId = "";
            this.EditReceiveId = this.ucFilter2.SelectedReceiveID;
            this.GetAndBindUpNoOptions(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);

            this.ccbtnOK.Enabled = (this.ddlUpNo.Items.Count > 0);

            this.tabResult.Visible = false;
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
            string sSerialNo = null;
            string eSerialNo = null;
            string upNo = null;
            string studentId = null;
            if (this.rbtQType1.Checked)
            {
                //產生所有繳費單
                qType = "1";
            }
            else if (this.rbtQType2.Checked)
            {
                //自訂產生繳費單流水號
                qType = "2";

                sSerialNo = this.tbxSSeriorNo.Text.Trim();
                eSerialNo = this.tbxESeriorNo.Text.Trim();
                if (String.IsNullOrEmpty(sSerialNo) || String.IsNullOrEmpty(eSerialNo))
                {
                    this.ShowMustInputAlert("自訂產生繳費單流水號");
                    return;
                }
                Int64 value = 0;
                if (!Int64.TryParse(sSerialNo, out value) || !Int64.TryParse(eSerialNo, out value))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「自訂產生繳費單流水號」只能輸入數字");
                    this.ShowSystemMessage(msg);
                    return;
                }
            }
            else if (this.rbtQType3.Checked)
            {
                //依批號產生
                qType = "3";

                upNo = this.ddlUpNo.SelectedValue;
                if (String.IsNullOrEmpty(upNo))
                {
                    this.ShowMustInputAlert("批號");
                    return;
                }
            }
            else if (this.rbtQType4.Checked)
            {
                //依學號產生
                qType = "4";

                studentId = this.tbxStudentId.Text.Trim();
                if (String.IsNullOrEmpty(studentId))
                {
                    this.ShowMustInputAlert("學號");
                    return;
                }
            }
            else
            {
                string msg = this.GetLocalized("請設定上傳資料的條件");
                this.ShowSystemMessage(msg);
                return;
            }

            #region [MDY:20160307] 增加處理類型 (上傳或刪除) 選擇
            string updKind = this.rblUpdKind.SelectedValue;
            if (String.IsNullOrEmpty(updKind))
            {
                this.ShowMustInputAlert("處理類型");
                return;
            }
            #endregion

            #region 新增 JobCube
            {
                JobcubeEntity jobCube = new JobcubeEntity();
                jobCube.Jdll = String.Empty;
                jobCube.Jclass = String.Empty;
                jobCube.Jmethod = String.Empty;
                jobCube.Jparam = JobcubeEntity.JoinD38Parameter(updKind, receiveType, yearId, termId, depId, receiveId, qType, sSerialNo, eSerialNo, upNo, studentId);
                jobCube.Jowner = this.GetLogonUser().UserId;
                jobCube.Jrid = receiveType;
                jobCube.Jyear = yearId;
                jobCube.Jterm = termId;
                jobCube.Jdep = depId;
                jobCube.Jrecid = receiveId;
                jobCube.Jprity = 0;
                jobCube.Jtypeid = JobCubeTypeCodeTexts.D38;

                jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
                jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
                jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
                jobCube.CDate = DateTime.Now;
                jobCube.SeriorNo = String.Empty;
                jobCube.Memo = String.Empty;
                jobCube.Chancel = String.Empty;

                string action = this.GetLocalized("新增D38資料上傳工作");
                string jno = null;
                XmlResult xmlResult = DataProxy.Current.InsertJobCubeForD38(this.Page, jobCube, out jno);
                if (xmlResult.IsSuccess)
                {
                    this.tabResult.Visible = true;
                    this.labJobNo.Text = jno.ToString();

                    this.ShowActionSuccessMessage(action);
                }
                else
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion
        }
    }
}