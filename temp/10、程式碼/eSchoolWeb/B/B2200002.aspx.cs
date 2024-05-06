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
    /// 產生銷帳編號
    /// </summary>
    public partial class B2200002 : BasePage
    {
        #region Const
        private const string _JobCubeTypeId = "CI";
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

            #region [Old]
            //this.GetAndBindSMChannelWay(receiveType);
            #endregion

            this.BindSeriorSortFieldOptions();

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

        #region [Old] 取消
        ///// <summary>
        ///// 取得並結繫是否有超商管道
        ///// </summary>
        ///// <param name="receiveType"></param>
        //private void GetAndBindSMChannelWay(string receiveType)
        //{
        //    #region 取資料
        //    int count = 0;
        //    if (!String.IsNullOrEmpty(receiveType))
        //    {
        //        Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType)
        //            .And(ReceiveChannelEntity.Field.ChannelId, ChannelHelper.GetSMChannelIds());

        //        XmlResult xmlResult = DataProxy.Current.SelectCount<ReceiveChannelEntity>(this.Page, where, out count);
        //        if (!xmlResult.IsSuccess)
        //        {
        //            string action = this.GetLocalized("查詢是否有超商管道資料");
        //            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //        }
        //    }
        //    #endregion

        //    this.cbxChannel2.Checked = (count > 0);
        //}
        #endregion

        /// <summary>
        /// 結繫流水號排序原則選項
        /// </summary>
        private void BindSeriorSortFieldOptions()
        {
            DropDownList[] ddlControls = new DropDownList[] {
                this.ddlSeriorSortField1, this.ddlSeriorSortField2, this.ddlSeriorSortField3, this.ddlSeriorSortField4, this.ddlSeriorSortField5
            };
            CodeText[] items = new CodeText[] {
                new CodeText("College_Id", this.GetLocalized("院別")), new CodeText("Major_Id", this.GetLocalized("科系")),
                new CodeText("Stu_Grade", this.GetLocalized("年級")), new CodeText("Class_Id", this.GetLocalized("班別")),
                new CodeText("Stu_Id", this.GetLocalized("學號"))
            };
            int idx = 0;
            foreach (DropDownList ddlControl in ddlControls)
            {
                foreach (CodeText item in items)
                {
                    WebHelper.SetDropDownListItems(ddlControl, DefaultItem.Kind.None, false, items, false, false, 0, items[idx].Code);
                }
                idx++;
            }
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

            #region 上傳批號
            string upNo = this.ddlUpNo.SelectedValue;
            if (String.IsNullOrEmpty(upNo))
            {
                this.ShowMustInputAlert("上傳批號");
                return;
            }
            #endregion

            string startSeriorNo = String.Empty;

            #region [Old] 取消指定流水號
            //#region 流水號起始位置
            //string startSeriorNo = null;
            //if (this.rbtSeriorType1.Checked)
            //{
            //    startSeriorNo = String.Empty;
            //}
            //else if (this.rbtSeriorType2.Checked)
            //{
            //    startSeriorNo = this.tbxSeriorNoStart.Text.Trim();
            //    if (String.IsNullOrEmpty(startSeriorNo))
            //    {
            //        this.ShowMustInputAlert("自定流水號位置");
            //        return;
            //    }
            //    Int64 value = 0;
            //    if (!Int64.TryParse(startSeriorNo, out value))
            //    {
            //        //[TODO] 固定顯示訊息的收集
            //        string msg = this.GetLocalized("「自定流水號位置」僅限輸入數字");
            //        this.ShowSystemMessage(msg);
            //        return;
            //    }
            //}
            //else
            //{
            //    this.ShowMustInputAlert("流水號起始位置");
            //    return;
            //}
            //#endregion

            //#region 代收管道
            //string temp = "";
            //if (this.cbxChannel2.Checked)
            //{
            //    temp = "2";
            //}
            //#endregion
            #endregion

            #region 流水號排序原則
            string sortType = "";
            string sortFields = "";
            if (this.rbtSeriorSortType1.Checked)
            {
                sortType = "1";
                string[] fields = new string[5] {
                    this.ddlSeriorSortField1.SelectedValue, this.ddlSeriorSortField2.SelectedValue,
                    this.ddlSeriorSortField3.SelectedValue, this.ddlSeriorSortField4.SelectedValue,
                    this.ddlSeriorSortField5.SelectedValue
                };
                for (int idx = 1; idx < fields.Length; idx++)
                {
                    if (fields[0] == fields[idx])
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「照下列順序編排流水號」的順序不可重複");
                        this.ShowSystemMessage(msg);
                        return;
                    }
                }
                sortFields = String.Join("|", fields);
            }
            else if (this.rbtSeriorSortType2.Checked)
            {
                sortType = "2";
            }
            else
            {
                this.ShowMustInputAlert("流水號排序原則");
                return;
            }
            #endregion

            #region 新增 JobCube
            {
                JobcubeEntity jobCube = new JobcubeEntity();
                jobCube.Jdll = String.Empty;
                jobCube.Jclass = String.Empty;
                jobCube.Jmethod = String.Empty;
                jobCube.Jparam = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", sortType, startSeriorNo, yearId, receiveType, termId, depId, receiveId, sortFields, upNo);
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
                jobCube.SeriorNo = upNo;
                jobCube.Memo = String.Empty;

                #region [Old] 取消
                //jobCube.Chancel = temp;
                #endregion

                int count = 0;
                XmlResult xmlResult = DataProxy.Current.Insert<JobcubeEntity>(this.Page, jobCube, out count);

                if (xmlResult.IsSuccess)
                {
                    KeyValueList<string> QueryString = new KeyValueList<string>();
                    QueryString.Add("Action", ActionMode.View);
                    QueryString.Add("ReceiveType", receiveType);
                    QueryString.Add("YearId", yearId);
                    QueryString.Add("TermId", termId);
                    QueryString.Add("DepId", depId);
                    QueryString.Add("ReceiveId", receiveId);
                    QueryString.Add("UpNo", upNo);
                    Session["QueryString"] = QueryString;

                    #region [MDY:20210521] 原碼修正
                    Server.Transfer(WebHelper.GenRNUrl("B2200002D.aspx"));
                    #endregion
                }
                else
                {
                    string action = this.GetLocalized("新增產生虛擬帳號排程工作");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion
        }
    }
}