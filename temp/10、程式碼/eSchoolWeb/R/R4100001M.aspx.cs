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

namespace eSchoolWeb.R
{
    /// <summary>
    /// 退費處理 (維護)
    /// </summary>
    public partial class R4100001M : BasePage
    {
        #region Keep 頁面參數
        /// <summary>
        /// 編輯模式參數
        /// </summary>
        private string Action
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["ACTION"] as string);
            }
            set
            {
                ViewState["ACTION"] = value == null ? null : value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// 編輯的業務別碼參數
        /// </summary>
        private string EditReceiveType
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditReceiveType"] as string);
            }
            set
            {
                ViewState["EditReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學年參數
        /// </summary>
        private string EditYearId
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditYearId"] as string);
            }
            set
            {
                ViewState["EditYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學期參數
        /// </summary>
        private string EditTermId
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditTermId"] as string);
            }
            set
            {
                ViewState["EditTermId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的部別參數
        /// </summary>
        private string EditDepId
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditDepId"] as string);
            }
            set
            {
                ViewState["EditDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的代收費用別參數
        /// </summary>
        private string EditReceiveId
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditReceiveId"] as string);
            }
            set
            {
                ViewState["EditReceiveId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學號參數
        /// </summary>
        private string EditStuId
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditStuId"] as string);
            }
            set
            {
                ViewState["EditStuId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的舊資料序號參數
        /// </summary>
        private int EditOldSeq
        {
            get
            {
                object value = ViewState["EditOldSeq"];
                if (value is int)
                {
                    return (int)value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["EditOldSeq"] = value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// 編輯的銷帳編號參數
        /// </summary>
        private string EditCancelNo
        {
            get
            {
                return ViewState["EditCancelNo"] as string;
            }
            set
            {
                ViewState["EditCancelNo"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的搜尋參數
        /// </summary>
        private string EditSearchType
        {
            get
            {
                return ViewState["EditSearchType"] as string;
            }
            set
            {
                ViewState["EditSearchType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的搜尋參數
        /// </summary>
        private string EditSearchString
        {
            get
            {
                return ViewState["EditSearchString"] as string;
            }
            set
            {
                ViewState["EditSearchString"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的搜尋參數
        /// </summary>
        private string EditDataNo
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditDataNo"] as string);
            }
            set
            {
                ViewState["EditDataNo"] = value == null ? null : value.Trim();
            }
        }

        private string[] KeepReceiveItemNames
        {
            get
            {
                return ViewState["KeepReceiveItemNames"] as string[];
            }
            set
            {
                ViewState["KeepReceiveItemNames"] = value;
            }
        }

        private decimal?[] KeepReceiveItemAmount
        {
            get
            {
                return ViewState["KeepReceiveItemAmount"] as decimal?[];
            }
            set
            {
                ViewState["KeepReceiveItemAmount"] = value;
            }
        }

        private decimal?[] KeepAlreadyReturnAmount
        {
            get
            {
                return ViewState["KeepAlreadyReturnAmount"] as decimal?[];
            }
            set
            {
                ViewState["KeepAlreadyReturnAmount"] = value;
            }
        }

        /// <summary>
        /// 編輯的SchoolRidEntity 
        /// </summary>
        private SchoolRidEntity EditSchoolRid
        {
            get
            {
                return ViewState["EditSchoolRid"] as SchoolRidEntity;
            }
            set
            {
                ViewState["EditSchoolRid"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 編輯的 StudentReceiveEntity  
        /// </summary>
        private StudentReceiveEntity EditStudentReceive
        {
            get
            {
                return ViewState["EditStudentReceive"] as StudentReceiveEntity;
            }
            set
            {
                ViewState["EditStudentReceive"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 編輯的 StudentReturnEntity  
        /// </summary>
        private StudentReturnEntity EditStudentReturn
        {
            get
            {
                return ViewState["EditStudentReturn"] as StudentReturnEntity;
            }
            set
            {
                ViewState["EditStudentReturn"] = value == null ? null : value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
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
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.EditYearId = QueryString.TryGetValue("YearId", String.Empty);
                this.EditTermId = QueryString.TryGetValue("TermId", String.Empty);
                this.EditDepId = QueryString.TryGetValue("DepId", String.Empty);
                this.EditReceiveId = QueryString.TryGetValue("ReceiveId", String.Empty);
                this.EditStuId = QueryString.TryGetValue("StuId", String.Empty);
                string oldSeq = QueryString.TryGetValue("OldSeq", String.Empty);
                this.EditCancelNo = QueryString.TryGetValue("CancelNo", String.Empty);
                this.EditSearchType = QueryString.TryGetValue("SearchType", String.Empty);
                this.EditSearchString = QueryString.TryGetValue("SearchString", String.Empty);
                this.EditDataNo = QueryString.TryGetValue("DataNo", String.Empty);

                int editOldSeq = 0;
                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                    //|| String.IsNullOrEmpty(this.EditDepId)
                    || String.IsNullOrEmpty(this.EditReceiveId)
                    || String.IsNullOrEmpty(this.EditStuId)
                    || !Int32.TryParse(oldSeq, out editOldSeq) || editOldSeq < 0
                    || String.IsNullOrEmpty(this.EditCancelNo)
                    || (this.Action != ActionMode.Insert && this.Action != ActionMode.Modify && this.Action != ActionMode.Delete))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }

                this.EditOldSeq = editOldSeq;

                XmlResult xmlResult = ucFilter1.GetDataAndBind(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                    return;
                }
                #endregion

                #region 檢查業務別碼授權
                if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                SchoolRidEntity schoolRid = null;
                StudentReceiveEntity studentReceive = null;
                StudentReturnEntity studentReturn = null;
                StudentMasterEntity student = null;
                bool isOK = this.GetEditData(out schoolRid, out studentReceive, out studentReturn, out student);
                #endregion

                if (!isOK)
                {
                    string msg = this.GetLocalized("查無此學生資料");
                    this.ShowSystemMessage(msg);
                    return;
                }

                this.BindEditData(schoolRid, studentReceive, studentReturn, student);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            #region 學生基本資料
            this.labStuId.Text = String.Empty;
            this.labStuName.Text = String.Empty;
            this.labIdNumber.Text = String.Empty;
            this.labTel.Text = String.Empty;
            this.labBirthday.Text = String.Empty;
            this.labZipCode.Text = String.Empty;
            this.labAccount.Text = String.Empty;
            this.labEmail.Text = String.Empty;
            #endregion

            #region 退費記錄
            this.labReturnDate.Text = String.Empty;
            this.labReturnId.Text = String.Empty;
            this.labReturnRemark.Text = String.Empty;
            this.labReturnWay.Text = String.Empty;
            #endregion

            #region 退費處理
            this.labCancelNo.Text = String.Empty;
            #endregion

            #region 退費科目金額
            this.litReceiveItemHtml.Text = String.Empty;
            #endregion

            this.ccbtnOK.Visible = false;
        }

        /// <summary>
        /// 取得要編輯的資料
        /// </summary>
        /// <param name="schoolRid">成功則傳回代收費用設定資料</param>
        /// <param name="studentReceive">成功則傳回要編輯的學生繳費資料</param>
        /// <param name="student">成功則傳回要編輯的學生基本資料</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetEditData(out SchoolRidEntity schoolRid, out StudentReceiveEntity studentReceive, out StudentReturnEntity studentReturn, out StudentMasterEntity student)
        {
            studentReturn = null;
            studentReceive = null;
            student = null;

            string action = this.GetLocalized("查詢要維護的資料");

            #region SchoolRidEntity
            {
                Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, this.EditReceiveType)
                    .And(SchoolRidEntity.Field.YearId, this.EditYearId)
                    .And(SchoolRidEntity.Field.TermId, this.EditTermId)
                    .And(SchoolRidEntity.Field.DepId, this.EditDepId)
                    .And(SchoolRidEntity.Field.ReceiveId, this.EditReceiveId);
                XmlResult result = DataProxy.Current.SelectFirst<SchoolRidEntity>(this, where, null, out schoolRid);
                if (!result.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, result.Code, result.Message);
                    return false;
                }
                if (schoolRid == null)
                {
                    this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "無該代收費用設定資料");
                    return false;
                }
            }
            #endregion

            #region StudentReceiveEntity
            {
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, this.EditReceiveType)
                    .And(StudentReceiveEntity.Field.YearId, this.EditYearId)
                    .And(StudentReceiveEntity.Field.TermId, this.EditTermId)
                    .And(StudentReceiveEntity.Field.DepId, this.EditDepId)
                    .And(StudentReceiveEntity.Field.ReceiveId, this.EditReceiveId)
                    .And(StudentReceiveEntity.Field.StuId, this.EditStuId);
                XmlResult result = DataProxy.Current.SelectFirst<StudentReceiveEntity>(this, where, null, out studentReceive);
                if (!result.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, result.Code, result.Message);
                    return false;
                }
                if (studentReceive == null)
                {
                    this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "無該學生繳費資料");
                    return false;
                }
            }
            #endregion

            if (this.Action != ActionMode.Insert)
            {
                #region studentReturnEntity
                {
                    Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, this.EditReceiveType)
                        .And(StudentReturnEntity.Field.YearId, this.EditYearId)
                        .And(StudentReturnEntity.Field.TermId, this.EditTermId)
                        .And(StudentReturnEntity.Field.DepId, this.EditDepId)
                        .And(StudentReturnEntity.Field.ReceiveId, this.EditReceiveId)
                        .And(StudentReturnEntity.Field.StuId, this.EditStuId)
                        .And(StudentReturnEntity.Field.OldSeq, this.EditOldSeq)
                        .And(StudentReturnEntity.Field.DataNo, this.EditDataNo);
                    XmlResult result = DataProxy.Current.SelectFirst<StudentReturnEntity>(this, where, null, out studentReturn);
                    if (!result.IsSuccess)
                    {
                        this.ShowActionFailureMessage(action, result.Code, result.Message);
                        return false;
                    }
                    if (studentReturn == null)
                    {
                        this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "無該學生退費資料");
                        return false;
                    }
                }
                #endregion
            }

            #region StudentMasterEntity
            {
                Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, this.EditReceiveType)
                    .And(StudentMasterEntity.Field.DepId, this.EditDepId)
                    .And(StudentMasterEntity.Field.Id, this.EditStuId);
                XmlResult result = DataProxy.Current.SelectFirst<StudentMasterEntity>(this, where, null, out student);
                if (!result.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, result.Code, result.Message);
                    return false;
                }
                if (student == null)
                {
                    this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "無該學生基本資料");
                    return false;
                }
            }
            #endregion

            this.EditSchoolRid = schoolRid;
            this.EditStudentReceive = studentReceive;
            this.EditStudentReturn = studentReturn;

            return true;
        }

        /// <summary>
        /// 結繫要編輯的資料
        /// </summary>
        /// <param name="schoolRid">代收費用設定資料</param>
        /// <param name="studentReceive">編輯的學生繳費資料</param>
        /// <param name="student">學生基本資料</param>
        private void BindEditData(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive, StudentReturnEntity studentReturn, StudentMasterEntity student)
        {
            #region 如果 Null, Return
            if (schoolRid == null || studentReceive == null || student == null)
            {
                #region 學生基本資料
                this.labStuId.Text = String.Empty;
                this.labStuName.Text = String.Empty;
                this.labIdNumber.Text = String.Empty;
                this.labTel.Text = String.Empty;
                this.labBirthday.Text = String.Empty;
                this.labZipCode.Text = String.Empty;
                this.labAccount.Text = String.Empty;
                this.labEmail.Text = String.Empty;
                #endregion

                #region 退費記錄
                this.labReturnDate.Text = String.Empty;
                this.labReturnId.Text = String.Empty;
                this.labReturnRemark.Text = String.Empty;
                this.labReturnWay.Text = String.Empty;
                #endregion

                #region 退費處理
                this.labCancelNo.Text = String.Empty;
                #endregion

                #region 退費科目金額
                this.litReceiveItemHtml.Text = String.Empty;
                #endregion

                return;
            }
            #endregion

            bool isEditable = this.Action != ActionMode.Delete;

            #region 學生基本資料
            this.labStuId.Text = student.Id == null ? String.Empty : student.Id.Trim();
            this.labStuName.Text = student.Name == null ? String.Empty : student.Name.Trim();
            this.labIdNumber.Text = student.IdNumber == null ? String.Empty : student.IdNumber.Trim();
            this.labTel.Text = student.Tel == null ? String.Empty : student.Tel.Trim();

            string birthday = student.Birthday == null ? String.Empty : student.Birthday.Trim();
            DateTime date;
            if (!String.IsNullOrEmpty(birthday) && Common.TryConvertTWDate7(birthday, out date))
            {
                this.labBirthday.Text = DataFormat.GetDateText(date);
            }
            else
            {
                this.labBirthday.Text = student.Birthday;
            }

            this.labZipCode.Text = student.ZipCode == null ? String.Empty : student.ZipCode.Trim();
            this.labAccount.Text = student.Account == null ? String.Empty : student.Account.Trim();
            this.labEmail.Text = student.Email == null ? String.Empty : student.Email.Trim();
            #endregion

            StudentReturnEntity pStudentReturn = GetPreStudentReturn();

            if (pStudentReturn != null)      //有前一筆退費記錄
            {
                #region 退費記錄
                this.labReturnDate.Text = pStudentReturn.ReturnDate == null ? String.Empty : pStudentReturn.ReturnDate.Trim();
                #region [MDY:20210714] FIX BUG 原碼修正
                this.labReturnId.Text = pStudentReturn.ReturnId == "0" ? "依輸入金額" : HttpUtility.HtmlEncode(pStudentReturn.ReturnId);  //目前只有 0=依輸入金額
                #endregion
                this.labReturnRemark.Text = pStudentReturn.ReturnRemark == "y" ? "是" : "否";
                #region [MDY:20210714] FIX BUG 原碼修正
                this.labReturnWay.Text = pStudentReturn.ReturnWay == "1" ? "現金" : HttpUtility.HtmlEncode(pStudentReturn.ReturnWay);  //目前只有 1=現金
                #endregion
                #endregion
            }
            else
            {   //無退費記錄
                #region 退費記錄
                this.labReturnDate.Text = Common.GetTWDate7(DateTime.Now);
                #region [MDY:20210714] FIX BUG 原碼修正
                this.labReturnId.Text = HttpUtility.HtmlEncode(ddlReturnId.SelectedItem.Text);  //目前只有 0=依輸入金額
                #endregion
                this.labReturnRemark.Text = "否";
                #region [MDY:20210714] FIX BUG 原碼修正
                this.labReturnWay.Text = HttpUtility.HtmlEncode(ddlReturnWay.SelectedItem.Text);  //目前只有 1=現金
                #endregion
                #endregion
            }

            #region 收入明細
            this.litReceiveItemHtml.Text = this.GenReceiveItemHtml(schoolRid, studentReceive, studentReturn);
            #endregion

            this.ccbtnOK.Visible = true;
        }


        private StudentReturnEntity GetPreStudentReturn()
        {
            StudentReturnEntity data = null;

            Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, this.EditReceiveType)
                .And(StudentReturnEntity.Field.YearId, this.EditYearId)
                .And(StudentReturnEntity.Field.TermId, this.EditTermId)
                .And(StudentReturnEntity.Field.DepId, this.EditDepId)
                .And(StudentReturnEntity.Field.ReceiveId, this.EditReceiveId)
                .And(StudentReturnEntity.Field.StuId, this.EditStuId)
                .And(StudentReturnEntity.Field.OldSeq, this.EditOldSeq);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReturnEntity.Field.DataNo, OrderByEnum.Desc);

            XmlResult result = DataProxy.Current.SelectFirst<StudentReturnEntity>(this, where, orderbys, out data);
            if (!result.IsSuccess)
            {
                this.ShowSystemMessage(string.Format("讀取退費檔錯誤，") + result.Message);
                return null;
            }

            return data;
        }

        private string GetNextReSeq()
        {
            StudentReturnEntity data = null;

            Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, this.EditReceiveType)
                .And(StudentReturnEntity.Field.YearId, this.EditYearId)
                .And(StudentReturnEntity.Field.TermId, this.EditTermId)
                .And(StudentReturnEntity.Field.DepId, this.EditDepId)
                .And(StudentReturnEntity.Field.ReceiveId, this.EditReceiveId)
                .And(StudentReturnEntity.Field.StuId, this.EditStuId)
                .And(StudentReturnEntity.Field.OldSeq, this.EditOldSeq);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReturnEntity.Field.DataNo, OrderByEnum.Desc);

            XmlResult result = DataProxy.Current.SelectFirst<StudentReturnEntity>(this, where, orderbys, out data);
            if (!result.IsSuccess)
            {
                this.ShowSystemMessage(string.Format("讀取退費檔錯誤，") + result.Message);
                return null;
            }

            int iReSeq = 1;
            if (data != null)
            {
                if (int.TryParse(data.ReSeq, out iReSeq))
                {
                    iReSeq++;
                }
            }

            return iReSeq.ToString();
        }

        /// <summary>
        /// 計算已退款金額
        /// </summary>
        /// <returns></returns>
        private decimal?[] GetAllReturnMoney()
        {
            decimal?[] returnItemValues = new decimal?[30];

            StudentReturnEntity[] datas = null;

            Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, this.EditReceiveType)
                .And(StudentReturnEntity.Field.YearId, this.EditYearId)
                .And(StudentReturnEntity.Field.TermId, this.EditTermId)
                .And(StudentReturnEntity.Field.DepId, this.EditDepId)
                .And(StudentReturnEntity.Field.ReceiveId, this.EditReceiveId)
                .And(StudentReturnEntity.Field.StuId, this.EditStuId)
                .And(StudentReturnEntity.Field.OldSeq, this.EditOldSeq);
             if (Common.IsNumber(this.EditDataNo))
             {
                 where.And(StudentReturnEntity.Field.DataNo, RelationEnum.NotEqual, this.EditDataNo);
             }

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReturnEntity.Field.DataNo, OrderByEnum.Desc);

            XmlResult result = DataProxy.Current.SelectAll<StudentReturnEntity>(this, where, orderbys, out datas);
            if (!result.IsSuccess)
            {
                this.ShowSystemMessage(string.Format("讀取退費檔錯誤，") + result.Message);
                return null;
            }

            foreach (StudentReturnEntity data in datas)
            {
                for (int i = 1; i < 31; i++)
                {
                    Result rt = new Result();
                    string fieldName = "Return_" + i.ToString("00");
                    object obS = data.GetValue(fieldName, out rt);
                    if (obS != null)
                    {
                        decimal money = returnItemValues[i - 1] == null ? 0 : Convert.ToDecimal(returnItemValues[i - 1]);
                        returnItemValues[i - 1] = money + Convert.ToDecimal(obS.ToString());
                    }
                }
            }

            this.KeepAlreadyReturnAmount = returnItemValues;

            return returnItemValues;
        }

        /// <summary>
        /// 產生收入科目的 Html
        /// </summary>
        /// <param name="schoolRid"></param>
        /// <param name="studentReceive"></param>
        /// <param name="isEditable"></param>
        /// <returns></returns>
        private string GenReceiveItemHtml(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive, StudentReturnEntity studentReturn)
        {
            if (studentReceive == null)
            {
                return String.Empty;
            }

            //收入科目名稱
            string[] receiveItemNames = new string[] {
                schoolRid.ReceiveItem01, schoolRid.ReceiveItem02, schoolRid.ReceiveItem03, schoolRid.ReceiveItem04, schoolRid.ReceiveItem05,
                schoolRid.ReceiveItem06, schoolRid.ReceiveItem07, schoolRid.ReceiveItem08, schoolRid.ReceiveItem09, schoolRid.ReceiveItem10,
                schoolRid.ReceiveItem11, schoolRid.ReceiveItem12, schoolRid.ReceiveItem13, schoolRid.ReceiveItem14, schoolRid.ReceiveItem15,
                schoolRid.ReceiveItem16, schoolRid.ReceiveItem17, schoolRid.ReceiveItem18, schoolRid.ReceiveItem19, schoolRid.ReceiveItem20,
                schoolRid.ReceiveItem21, schoolRid.ReceiveItem22, schoolRid.ReceiveItem23, schoolRid.ReceiveItem24, schoolRid.ReceiveItem25,
                schoolRid.ReceiveItem26, schoolRid.ReceiveItem27, schoolRid.ReceiveItem28, schoolRid.ReceiveItem29, schoolRid.ReceiveItem30
            };

            //收入科目金額
            decimal?[] receiveItemValues = new decimal?[30];
            for (int i = 1; i < 31; i++)
            {
                Result rt = new Result();
                string fieldName = "Receive_" + i.ToString("00");
                object obS = studentReceive.GetValue(fieldName, out rt);
                receiveItemValues[i - 1] = (decimal?)obS;
            }

            //退費金額
            decimal?[] returnItemValues = new decimal?[30];
            if (studentReturn != null)
            {
                for (int i = 1; i < 31; i++)
                {
                    Result rt = new Result();
                    string fieldName = "Return_" + i.ToString("00");
                    object obS = studentReturn.GetValue(fieldName, out rt);
                    returnItemValues[i - 1] = (decimal?)obS;
                }
            }

            //已退費金額
            decimal?[] alreadyReturns = GetAllReturnMoney();

            this.KeepReceiveItemNames = receiveItemNames;
            this.KeepReceiveItemAmount = receiveItemValues;

            string disbale = this.Action == ActionMode.Delete ? "disabled" : String.Empty;

            decimal total = 0;
            StringBuilder html = new StringBuilder();
            for (int idx = 1; idx < receiveItemNames.Length + 1; idx++)
            {
                string receiveItemName = receiveItemNames[idx - 1];
                decimal? receiveItemValue = receiveItemValues[idx - 1];
                decimal? returnItemValue = returnItemValues[idx - 1];
                decimal? alreadyReturn = alreadyReturns[idx - 1];
                if (!String.IsNullOrWhiteSpace(receiveItemName))
                {
                    string tbxName = String.Format("tbxReturnMoney{0}", idx.ToString("00"));
                    string tbxReturnMoney = DataFormat.GetAmountText(returnItemValue);
                    //目前可退費金額 = 退費金額 - 已退費金額
                    decimal okReturnMoney = 0;
                    decimal money = receiveItemValue == null ? 0 : (decimal)receiveItemValue;
                    decimal almoney = alreadyReturn == null ? 0 : (decimal)alreadyReturn;
                    okReturnMoney = money - almoney;

                    string tbxReceiveMoney = DataFormat.GetAmountText(okReturnMoney);

                    total += tbxReturnMoney.Trim() != "" ? Convert.ToDecimal(tbxReturnMoney) : 0;
                    html
                        .AppendLine("<tr>")
                        .AppendFormat("<td width=\"60%\">{0}</td>", receiveItemName).AppendLine()
                        .AppendFormat("<td width=\"60%\">{0}</td>", tbxReceiveMoney).AppendLine()
                        //.AppendFormat("<td width=\"60%\">{0}</td>", DataFormat.GetAmountText(receiveItemValue)).AppendLine()
                        .AppendFormat("<td><input type=\"text\" name=\"{0}\" id=\"{0}\" value=\"{1}\" onblur=\"addmon()\" {2} /></td>", tbxName, tbxReturnMoney, disbale).AppendLine()
                        .AppendLine("</tr>");
                }
            }

            //累計退費金額合計 = total
            labReturnAmount.Text = DataFormat.GetAmountText(total);

            return html.ToString();
        }


        /// <summary>
        /// 重新 Bind 收入科目的 Html (因為 ReceiveItem 是 Gen 出來的 Html，輸入資料不會 Keep 所以要重新 Bind)
        /// </summary>
        /// <param name="studentReceive"></param>
        /// <param name="isEditable"></param>
        private void ReBindReceiveItemHtml(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive, StudentReturnEntity studentReturn)
        {
            if (studentReturn == null)
            {
                return;
            }

            string[] receiveItemNames = new string[] {
                schoolRid.ReceiveItem01, schoolRid.ReceiveItem02, schoolRid.ReceiveItem03, schoolRid.ReceiveItem04, schoolRid.ReceiveItem05,
                schoolRid.ReceiveItem06, schoolRid.ReceiveItem07, schoolRid.ReceiveItem08, schoolRid.ReceiveItem09, schoolRid.ReceiveItem10,
                schoolRid.ReceiveItem11, schoolRid.ReceiveItem12, schoolRid.ReceiveItem13, schoolRid.ReceiveItem14, schoolRid.ReceiveItem15,
                schoolRid.ReceiveItem16, schoolRid.ReceiveItem17, schoolRid.ReceiveItem18, schoolRid.ReceiveItem19, schoolRid.ReceiveItem20,
                schoolRid.ReceiveItem21, schoolRid.ReceiveItem22, schoolRid.ReceiveItem23, schoolRid.ReceiveItem24, schoolRid.ReceiveItem25,
                schoolRid.ReceiveItem26, schoolRid.ReceiveItem27, schoolRid.ReceiveItem28, schoolRid.ReceiveItem29, schoolRid.ReceiveItem30
            };
            decimal?[] receiveItemValues = new decimal?[30];
            for (int i = 1; i < 31; i++)
            {
                Result rt = new Result();
                string fieldName = "Receive_" + i.ToString("00");
                object obS = studentReceive.GetValue(fieldName, out rt);
                receiveItemValues[i - 1] = (decimal?)obS;
            }
            this.KeepReceiveItemAmount = receiveItemValues;

            StringBuilder html = new StringBuilder();
            for (int idx = 1; idx < receiveItemNames.Length + 1; idx++)
            {
                string receiveItemName = receiveItemNames[idx - 1];
                decimal? receiveItemValue = receiveItemValues[idx - 1];
                if (!String.IsNullOrWhiteSpace(receiveItemName))
                {
                    string tbxName = String.Format("tbxReturnMoney{0}", idx.ToString("00"));
                    string tbxReturnMoney = DataFormat.GetAmountText(receiveItemValue);
                    html
                        .AppendLine("<tr>")
                        .AppendFormat("<td width=\"60%\">{0}</td>", receiveItemName).AppendLine()
                        .AppendFormat("<td width=\"60%\">{0}</td>", tbxReturnMoney).AppendLine()
                        .AppendFormat("<td><input type=\"text\" name=\"{0}\" id=\"{0}\" value=\"\" onblur=\"addmon()\" /></td>", tbxName).AppendLine()
                        .AppendLine("</tr>");
                }
            }

            this.litReceiveItemHtml.Text = html.ToString();
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!CheckEditData())
            {
                return;
            }

            this.SaveEditData();
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData()
        {
            if (this.Action == ActionMode.Delete)
            {
                return true;
            }

            #region  收入明細
            {
                for (int no = 1; no < 31; no++)
                {
                    string crlName = String.Format("tbxReturnMoney{0}", no.ToString("00"));
                    string ctlValue = this.Request.Form[crlName];
                    if (!String.IsNullOrWhiteSpace(ctlValue) && !Common.IsMoney(ctlValue))
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「退費金額」含有不是合法的金額");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                }
            }
            #endregion

            #region 判斷輸入金額是否大於退費金額
            for (int no = 1; no < 31; no++)
            {
                string crlName = String.Format("tbxReturnMoney{0}", no.ToString("00"));
                string ctlValue = this.Request.Form[crlName];
                if (Common.IsMoney(ctlValue))
                {
                    decimal? total = this.KeepReceiveItemAmount[no - 1];
                    decimal? almoney = KeepAlreadyReturnAmount[no - 1];
                    if (total != null && almoney != null)
                    {
                        if (Convert.ToDecimal(ctlValue) > (total - almoney))
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("「退費金額」大於目前可退金額");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                    }
                }
            }
            #endregion

            return true;
        }

        private bool SaveEditData(bool showOk = true)
        {
            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "R4100001.aspx";

            switch (this.Action)
            {
                case ActionMode.Insert:
                    #region 新增
                    {
                        #region StudentReturn
                        {
                            StudentReturnEntity data = new StudentReturnEntity();
                            data.ReceiveType = this.EditReceiveType;
                            data.YearId = this.EditYearId;
                            data.TermId = this.EditTermId;
                            data.DepId = this.EditDepId;
                            data.ReceiveId = this.EditReceiveId;
                            data.StuId = this.EditStuId;
                            data.OldSeq = this.EditOldSeq;
                            data.ReSeq = GetNextReSeq();

                            #region [MDY:20210714] FIX BUG
                            data.ReturnWay = ddlReturnWay.SelectedValue;   //1 = 現金; 2 = 匯款; 目前只有 1=現金
                            data.ReturnId = ddlReturnId.SelectedValue;   //目前只有 0=依輸入金額
                            #endregion

                            data.ReturnRemark = rdoReturnRemark.SelectedValue;
                            data.SrNo = string.Empty;
                            data.ReturnDate = Common.GetTWDate7(DateTime.Now);
                            data.CancelNo = this.EditCancelNo;

                            //decimal amt = 0;
                            //if (decimal.TryParse(labReturnAmount.Text, out amt))
                            //{
                            //    data.ReturnAmount = amt;
                            //}

                            decimal sum = 0;
                            //收入科目
                            KeyValueList fieldValues = new KeyValueList();
                            for (int i = 1; i < 31; i++)
                            {
                                string fieldName = "Return_" + i.ToString("00");
                                string crlName = String.Format("tbxReturnMoney{0}", i.ToString("00"));
                                string ctlValue = this.Request.Form[crlName];
                                decimal value;
                                if (!String.IsNullOrWhiteSpace(ctlValue) && Decimal.TryParse(ctlValue, out value))
                                {
                                    sum += value;
                                    data.SetValue(fieldName, value);
                                }
                            }
                            data.ReturnAmount = sum;

                            int count = 0;
                            XmlResult xmlResult = DataProxy.Current.Insert<StudentReturnEntity>(this, data, out count);
                            if (xmlResult.IsSuccess)
                            {
                                if (count < 1)
                                {
                                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                    return false;
                                }
                                else
                                {
                                    WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);
                                    this.ShowActionSuccessAlert(action, string.Format("{0}?SearchType={1}&SearchString={2}", backUrl, this.EditSearchType, this.EditSearchString));
                                    return true;
                                }
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                return false;
                            }
                        }
                        #endregion
                    }
                    #endregion
                case ActionMode.Modify:     //修改
                    #region 修改
                    {
                        if (!this.CheckEditData())
                        {
                            return false;
                        }
                        StudentReturnEntity studentReturn = this.GetEditStudentReturnData();

                        bool isOK = true;

                        #region StudentReturn
                        Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, studentReturn.ReceiveType)
                        .And(StudentReturnEntity.Field.YearId, studentReturn.YearId)
                        .And(StudentReturnEntity.Field.TermId, studentReturn.TermId)
                        .And(StudentReturnEntity.Field.DepId, studentReturn.DepId)
                        .And(StudentReturnEntity.Field.ReceiveId, studentReturn.ReceiveId)
                        .And(StudentReturnEntity.Field.StuId, studentReturn.StuId)
                        .And(StudentReturnEntity.Field.OldSeq, this.EditOldSeq)
                        .And(StudentReturnEntity.Field.DataNo, studentReturn.DataNo);

                        decimal sum = 0;
                        KeyValueList fieldValues = new KeyValueList();
                        for (int i = 1; i < 31; i++)
                        {
                            string fieldName = "Return_" + i.ToString("00");
                            string crlName = String.Format("tbxReturnMoney{0}", i.ToString("00"));
                            string ctlValue = this.Request.Form[crlName];
                            decimal value;
                            if (!String.IsNullOrWhiteSpace(ctlValue) && Decimal.TryParse(ctlValue, out value))
                            {
                                sum += value;
                                fieldValues.Add(fieldName, value);
                            }
                        }
                        fieldValues.Add(StudentReturnEntity.Field.ReturnAmount, sum);

                        #region [MDY:20210714] FIX BUG
                        fieldValues.Add(StudentReturnEntity.Field.ReturnWay, ddlReturnWay.SelectedValue);  //目前只有 1=現金
                        fieldValues.Add(StudentReturnEntity.Field.ReturnId, ddlReturnId.SelectedValue);   //目前只有 0=依輸入金額
                        fieldValues.Add(StudentReturnEntity.Field.ReturnRemark, rdoReturnRemark.SelectedValue);
                        #endregion

                        //decimal amt = 0;
                        //if (decimal.TryParse(labReturnAmount.Text, out amt))
                        //{
                        //    fieldValues.Add(StudentReturnEntity.Field.ReturnAmount, amt);
                        //}

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<StudentReturnEntity>(this.Page, where, fieldValues, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                isOK = false;
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                if (showOk)
                                {
                                    WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);
                                    this.ShowActionSuccessAlert(action, string.Format("{0}?SearchType={1}&SearchString={2}", backUrl, this.EditSearchType, this.EditSearchString));
                                }
                                else
                                {
                                    this.ReBindReceiveItemHtml(this.EditSchoolRid, this.EditStudentReceive, this.EditStudentReturn);
                                }
                            }
                        }
                        else
                        {
                            isOK = false;
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                        #endregion

                        return isOK;
                    }
                    #endregion
                case ActionMode.Delete:     //刪除
                    #region 刪除
                    {
                        #region StudentReturn
                        {
                            StudentReturnEntity data = new StudentReturnEntity();
                            data.ReceiveType = this.EditReceiveType;
                            data.YearId = this.EditYearId;
                            data.TermId = this.EditTermId;
                            data.DepId = this.EditDepId;
                            data.ReceiveId = this.EditReceiveId;
                            data.StuId = this.EditStuId;
                            long no = 0;
                            long.TryParse(this.EditDataNo, out no);
                            data.DataNo = no;

                            int count = 0;
                            XmlResult xmlResult = DataProxy.Current.Delete<StudentReturnEntity>(this, data, out count);
                            if (xmlResult.IsSuccess)
                            {
                                if (count < 1)
                                {
                                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                    return false;
                                }
                                else
                                {
                                    WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);
                                    this.ShowActionSuccessAlert(action, string.Format("{0}?SearchType={1}&SearchString={2}", backUrl, this.EditSearchType, this.EditSearchString));
                                    return true;
                                }
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                return false;
                            }
                        }
                        #endregion
                    }
                    #endregion
                default:
                    return false;
            }
        }

        /// <summary>
        /// 取得輸入的學生退費資料
        /// </summary>
        /// <returns></returns>
        private StudentReturnEntity GetEditStudentReturnData()
        {
            StudentReturnEntity data = new StudentReturnEntity();
            data.ReceiveType = this.EditReceiveType;
            data.YearId = this.EditYearId;
            data.TermId = this.EditTermId;
            data.DepId = this.EditDepId;
            data.ReceiveId = this.EditReceiveId;
            data.StuId = this.EditStuId;
            long no = 0;
            long.TryParse(this.EditDataNo, out no);
            data.DataNo = no;

            //處理收入科目
            decimal?[] receiveItemValues = new decimal?[30];
            for (int idx = 1; idx < 31; idx++)
            {
                string crlName = String.Format("tbxReturnMoney{0}", idx.ToString("00"));
                string ctlValue = this.Request.Form[crlName];
                decimal value;
                if (!String.IsNullOrWhiteSpace(ctlValue) && Decimal.TryParse(ctlValue, out value))
                {
                    receiveItemValues[idx - 1] = value;
                }
                else
                {
                    receiveItemValues[idx - 1] = null;
                }
            }

            for (int i = 1; i < 31; i++)
            {
                string fieldName = "Return_" + i.ToString("00");

                data.SetValue(fieldName, receiveItemValues[i - 1]);
            }

            #region [ old ]
            //data.Return01 = receiveItemValues[0];
            //data.Return02 = receiveItemValues[1];
            //data.Return03 = receiveItemValues[2];
            //data.Return04 = receiveItemValues[3];
            //data.Return05 = receiveItemValues[4];
            //data.Return06 = receiveItemValues[5];
            //data.Return07 = receiveItemValues[6];
            //data.Return08 = receiveItemValues[7];
            //data.Return09 = receiveItemValues[8];
            //data.Return10 = receiveItemValues[9];

            //data.Return11 = receiveItemValues[10];
            //data.Return12 = receiveItemValues[11];
            //data.Return13 = receiveItemValues[12];
            //data.Return14 = receiveItemValues[13];
            //data.Return15 = receiveItemValues[14];
            //data.Return16 = receiveItemValues[15];
            //data.Return17 = receiveItemValues[16];
            //data.Return18 = receiveItemValues[17];
            //data.Return19 = receiveItemValues[18];
            //data.Return20 = receiveItemValues[19];

            //data.Return21 = receiveItemValues[20];
            //data.Return22 = receiveItemValues[21];
            //data.Return23 = receiveItemValues[22];
            //data.Return24 = receiveItemValues[23];
            //data.Return25 = receiveItemValues[24];
            //data.Return26 = receiveItemValues[25];
            //data.Return27 = receiveItemValues[26];
            //data.Return28 = receiveItemValues[27];
            //data.Return29 = receiveItemValues[28];
            //data.Return30 = receiveItemValues[29];
            #endregion

            return data;
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);
            string url = string.Format("R4100001.aspx?SearchType={0}&SearchString={1}", this.EditSearchType, this.EditSearchString);
            Server.Transfer(url);
        }
    }
}