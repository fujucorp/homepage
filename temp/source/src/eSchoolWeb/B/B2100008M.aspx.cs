using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.B
{
    /// <summary>
    /// 維護就貸資料 (維護)
    /// </summary>
    public partial class B2100008M : BasePage
    {
        #region Keep 頁面參數
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
        /// 編輯的商家代號參數
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
        /// 編輯的學年參數
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
        /// 編輯的學期參數
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
        /// 編輯的部別參數
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
        /// 編輯的代收費用別參數
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

        /// <summary>
        /// 編輯的學號參數
        /// </summary>
        private string EditStuId
        {
            get
            {
                return ViewState["EditStuId"] as string;
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

        ///// <summary>
        ///// 編輯的就貸代碼參數
        ///// </summary>
        //private string EditLoanId
        //{
        //    get
        //    {
        //        return ViewState["EditLoanId"] as string;
        //    }
        //    set
        //    {
        //        ViewState["EditLoanId"] = value == null ? null : value.Trim();
        //    }
        //}

        private int KeepReceiveItemCount
        {
            get
            {
                object value = ViewState["KeepReceiveItemCount"];
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
                ViewState["KeepReceiveItemCount"] = value < 0 ? 0 : value;
            }
        }
        #endregion

        private HtmlTableRow[] _TrItemRowControls = null;
        private HtmlTableRow[] GetTrItemRowControls()
        {
            if (_TrItemRowControls == null)
            {
                _TrItemRowControls = new HtmlTableRow[] {
                    this.trItemRow01, this.trItemRow02, this.trItemRow03, this.trItemRow04, this.trItemRow05,
                    this.trItemRow06, this.trItemRow07, this.trItemRow08, this.trItemRow09, this.trItemRow10,
                    this.trItemRow11, this.trItemRow12, this.trItemRow13, this.trItemRow14, this.trItemRow15,
                    this.trItemRow16, this.trItemRow17, this.trItemRow18, this.trItemRow19, this.trItemRow20,
                    this.trItemRow21, this.trItemRow22, this.trItemRow23, this.trItemRow24, this.trItemRow25,
                    this.trItemRow26, this.trItemRow27, this.trItemRow28, this.trItemRow29, this.trItemRow30,
                    this.trItemRow31, this.trItemRow32, this.trItemRow33, this.trItemRow34, this.trItemRow35,
                    this.trItemRow36, this.trItemRow37, this.trItemRow38, this.trItemRow39, this.trItemRow40
                };
            }
            return _TrItemRowControls;
        }

        private Label[] _LabItemNameControls = null;
        private Label[] GetLabItemNameControls()
        {
            if (_LabItemNameControls == null)
            {
                _LabItemNameControls = new Label[] {
                    this.labItemName01, this.labItemName02, this.labItemName03, this.labItemName04, this.labItemName05,
                    this.labItemName06, this.labItemName07, this.labItemName08, this.labItemName09, this.labItemName10,
                    this.labItemName11, this.labItemName12, this.labItemName13, this.labItemName14, this.labItemName15,
                    this.labItemName16, this.labItemName17, this.labItemName18, this.labItemName19, this.labItemName20,
                    this.labItemName21, this.labItemName22, this.labItemName23, this.labItemName24, this.labItemName25,
                    this.labItemName26, this.labItemName27, this.labItemName28, this.labItemName29, this.labItemName30,
                    this.labItemName31, this.labItemName32, this.labItemName33, this.labItemName34, this.labItemName35,
                    this.labItemName36, this.labItemName37, this.labItemName38, this.labItemName39, this.labItemName40
                };
            }
            return _LabItemNameControls;
        }

        private Label[] _LabItemAmountControls = null;
        private Label[] GetLabItemAmountControls()
        {
            if (_LabItemAmountControls == null)
            {
                _LabItemAmountControls = new Label[] {
                    this.labItemAmount01, this.labItemAmount02, this.labItemAmount03, this.labItemAmount04, this.labItemAmount05,
                    this.labItemAmount06, this.labItemAmount07, this.labItemAmount08, this.labItemAmount09, this.labItemAmount10,
                    this.labItemAmount11, this.labItemAmount12, this.labItemAmount13, this.labItemAmount14, this.labItemAmount15,
                    this.labItemAmount16, this.labItemAmount17, this.labItemAmount18, this.labItemAmount19, this.labItemAmount20,
                    this.labItemAmount21, this.labItemAmount22, this.labItemAmount23, this.labItemAmount24, this.labItemAmount25,
                    this.labItemAmount26, this.labItemAmount27, this.labItemAmount28, this.labItemAmount29, this.labItemAmount30,
                    this.labItemAmount31, this.labItemAmount32, this.labItemAmount33, this.labItemAmount34, this.labItemAmount35,
                    this.labItemAmount36, this.labItemAmount37, this.labItemAmount38, this.labItemAmount39, this.labItemAmount40
                };
            }
            return _LabItemAmountControls;
        }

        private TextBox[] _TbxItemLoanControls = null;
        private TextBox[] GetTbxItemLoanControls()
        {
            if (_TbxItemLoanControls == null)
            {
                _TbxItemLoanControls = new TextBox[] {
                    this.tbxItemLoan01, this.tbxItemLoan02, this.tbxItemLoan03, this.tbxItemLoan04, this.tbxItemLoan05,
                    this.tbxItemLoan06, this.tbxItemLoan07, this.tbxItemLoan08, this.tbxItemLoan09, this.tbxItemLoan10,
                    this.tbxItemLoan11, this.tbxItemLoan12, this.tbxItemLoan13, this.tbxItemLoan14, this.tbxItemLoan15,
                    this.tbxItemLoan16, this.tbxItemLoan17, this.tbxItemLoan18, this.tbxItemLoan19, this.tbxItemLoan20,
                    this.tbxItemLoan21, this.tbxItemLoan22, this.tbxItemLoan23, this.tbxItemLoan24, this.tbxItemLoan25,
                    this.tbxItemLoan26, this.tbxItemLoan27, this.tbxItemLoan28, this.tbxItemLoan29, this.tbxItemLoan30,
                    this.tbxItemLoan31, this.tbxItemLoan32, this.tbxItemLoan33, this.tbxItemLoan34, this.tbxItemLoan35,
                    this.tbxItemLoan36, this.tbxItemLoan37, this.tbxItemLoan38, this.tbxItemLoan39, this.tbxItemLoan40
                };
            }
            return _TbxItemLoanControls;
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.labStuId.Text = String.Empty;
            this.labStuName.Text = String.Empty;
            this.labUpNo.Text = String.Empty;
            this.labOldSeq.Text = String.Empty;
            this.labMajorName.Text = String.Empty;
            this.labStuGrade.Text = String.Empty;
            this.labCancelNo.Text = String.Empty;
            this.labReceiveAmount.Text = String.Empty;

            WebHelper.SetDropDownListSelectedValue(this.ddlLoanId, null);

            this.tbxLoanFixAmount.Text = String.Empty;

            HtmlTableRow[] rows = this.GetTrItemRowControls();
            foreach (HtmlTableRow row in rows)
            {
                row.Visible = false;
            }
        }

        /// <summary>
        /// 取得並結繫就貸代碼選項
        /// </summary>
        /// <returns></returns>
        private void GetAndBindLoanIdOptions(string receiveType, string yearId, string termId, string depId)
        {
            CodeText[] items = null;

            if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) && !String.IsNullOrEmpty(termId))
            {
                Expression where = new Expression(LoanListEntity.Field.ReceiveType, receiveType)
                    .And(LoanListEntity.Field.YearId, yearId)
                    .And(LoanListEntity.Field.TermId, termId)
                    .And(LoanListEntity.Field.DepId, depId);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(LoanListEntity.Field.LoanId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { LoanListEntity.Field.LoanId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { LoanListEntity.Field.LoanName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<LoanListEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢就貸代碼資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }

            WebHelper.SetDropDownListItems(this.ddlLoanId, DefaultItem.Kind.Select, false, items, true, false, 0, null);
        }

        /// <summary>
        /// 取得要編輯的資料
        /// </summary>
        /// <param name="receiveItemNames"></param>
        /// <returns></returns>
        private bool GetEditData(out string[] receiveItemNames, out StudentReceiveView4 studentReceive, out StudentLoanEntity studentLoan)
        {
            receiveItemNames = null;
            studentReceive = null;
            studentLoan = null;

            string action = this.GetLocalized("查詢要維護的資料");

            #region SchoolRidEntity
            {
                SchoolRidEntity schoolRid = null;
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

                #region [MDY:202203XX] 2022擴充案 收入科目 改寫，改用 GetAllReceiveItemChts()
                #region [OLD]
                //receiveItemNames = schoolRid.GetAllReceiveItems();
                #endregion

                receiveItemNames = schoolRid.GetAllReceiveItemChts();
                #endregion
            }
            #endregion

            #region StudentReceiveView4
            {
                Expression where = new Expression(StudentReceiveView4.Field.ReceiveType, this.EditReceiveType)
                    .And(StudentReceiveView4.Field.YearId, this.EditYearId)
                    .And(StudentReceiveView4.Field.TermId, this.EditTermId)
                    .And(StudentReceiveView4.Field.DepId, this.EditDepId)
                    .And(StudentReceiveView4.Field.ReceiveId, this.EditReceiveId)
                    .And(StudentReceiveView4.Field.StuId, this.EditStuId)
                    .And(StudentReceiveView4.Field.OldSeq, this.EditOldSeq);
                XmlResult result = DataProxy.Current.SelectFirst<StudentReceiveView4>(this, where, null, out studentReceive);
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

            #region StudentLoanEntity
            {
                Expression where = new Expression(StudentLoanEntity.Field.ReceiveType, studentReceive.ReceiveType)
                    .And(StudentLoanEntity.Field.YearId, studentReceive.YearId)
                    .And(StudentLoanEntity.Field.TermId, studentReceive.TermId)
                    .And(StudentLoanEntity.Field.DepId, studentReceive.DepId)
                    .And(StudentLoanEntity.Field.ReceiveId, studentReceive.ReceiveId)
                    .And(StudentLoanEntity.Field.StuId, studentReceive.StuId)
                    .And(StudentLoanEntity.Field.OldSeq, studentReceive.OldSeq)
                    .And(StudentLoanEntity.Field.LoanId, studentReceive.LoanId);
                XmlResult result = DataProxy.Current.SelectFirst<StudentLoanEntity>(this, where, null, out studentLoan);
                if (!result.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, result.Code, result.Message);
                    return false;
                }
                if (studentLoan == null)
                {
                    studentLoan = new StudentLoanEntity();
                    studentLoan.ReceiveType = studentReceive.ReceiveType;
                    studentLoan.YearId = studentReceive.YearId;
                    studentLoan.TermId = studentReceive.TermId;
                    studentLoan.DepId = studentReceive.DepId;
                    studentLoan.ReceiveId = studentReceive.ReceiveId;
                    studentLoan.StuId = studentReceive.StuId;
                    studentLoan.OldSeq = studentReceive.OldSeq;
                    studentLoan.LoanId = studentReceive.LoanId;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 結繫要編輯的資料
        /// </summary>
        /// <param name="schoolRid">代收費用設定資料</param>
        /// <param name="studentReceive">編輯的學生繳費資料</param>
        /// <param name="student">學生基本資料</param>
        private void BindEditData(string[] receiveItemNames, StudentReceiveView4 studentReceive, StudentLoanEntity studentLoan)
        {
            int receiveItemCount = 0;

            if (studentReceive == null)
            {
                this.InitialUI();
            }
            else
            {
                bool isEditable = ActionMode.IsDataEditableMode(this.Action);

                this.labStuId.Text = studentReceive.StuId;
                this.labStuName.Text = studentReceive.StuName;
                this.labUpNo.Text = studentReceive.UpNo;
                this.labOldSeq.Text = studentReceive.OldSeq.ToString();
                this.labMajorName.Text = studentReceive.MajorName;
                this.labStuGrade.Text = studentReceive.StuGradeName;
                this.labCancelNo.Text = studentReceive.CancelNo;
                this.labReceiveAmount.Text = DataFormat.GetAmountText(studentReceive.ReceiveAmount);

                WebHelper.SetDropDownListSelectedValue(this.ddlLoanId, studentReceive.LoanId);
                this.ddlLoanId.Enabled = isEditable;

                //this.tbxLoanAmount.Text = studentLoan == null ? null : DataFormat.GetAmountText(studentLoan.LoanAmount);
                this.tbxLoanFixAmount.Text = studentLoan == null ? null : DataFormat.GetAmountText(studentLoan.LoanFixamount);

                if (receiveItemNames != null && receiveItemNames.Length > 0)
                {
                    for(int idx = 0; idx < receiveItemNames.Length; idx++)
                    {
                        if (String.IsNullOrWhiteSpace(receiveItemNames[idx]))
                        {
                            break;
                        }
                        else
                        {
                            receiveItemCount++;
                        }
                    }
                }

                HtmlTableRow[] rows = this.GetTrItemRowControls();
                Label[] labItemNames = this.GetLabItemNameControls();
                Label[] labItemAmounts = this.GetLabItemAmountControls();
                TextBox[] tbxItemLoans = this.GetTbxItemLoanControls();
                decimal?[] receiveAmounts = studentReceive.GetAllReceiveItemAmounts();
                for (int idx = 0; idx < receiveItemCount; idx++)
                {
                    rows[idx].Visible = true;
                    labItemNames[idx].Text = receiveItemNames[idx];
                    labItemAmounts[idx].Text = DataFormat.GetAmountCommaText(receiveAmounts[idx]);
                    tbxItemLoans[idx].Text = DataFormat.GetAmountText(studentLoan.GetLoanItemAmount(idx + 1));
                    tbxItemLoans[idx].Enabled = isEditable;
                }
                for (int idx = receiveItemCount; idx < rows.Length; idx++)
                {
                    rows[idx].Visible = false;
                    labItemNames[idx].Text = null;
                    labItemAmounts[idx].Text = null;
                    tbxItemLoans[idx].Text = null;
                }
            }

            this.KeepReceiveItemCount = receiveItemCount;
        }

        private bool GetAndCheckData(out StudentLoanEntity data)
        {
            data = new StudentLoanEntity();
            data.ReceiveType = this.EditReceiveType;
            data.YearId = this.EditYearId;
            data.TermId = this.EditTermId;
            data.DepId = this.EditDepId;
            data.ReceiveId = this.EditReceiveId;
            data.StuId = this.EditStuId;
            data.OldSeq = this.EditOldSeq;

            #region 就貸代碼
            data.LoanId = this.ddlLoanId.SelectedValue;
            if (String.IsNullOrEmpty(data.LoanId))
            {
                this.ShowMustInputAlert("就貸代碼");
                return false;
            }
            #endregion

            #region 就貸總金額
            {
                string txt = this.tbxLoanFixAmount.Text.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    decimal value = 0;
                    if (Decimal.TryParse(txt, out value) && value > 0)
                    {
                        data.LoanFixamount = value;
                    }
                    else
                    {
                        this.ShowJsAlert("就貸總金額不是合法的金額 (必須大於 0)");
                        return false;
                    }
                }
                if (data.LoanFixamount == null || data.LoanFixamount <= 0)
                {
                    this.ShowMustInputAlert("就貸總金額");
                    return false;
                }
            }
            #endregion

            #region 就貸明細 & 計算後的可貸金額
            int itemLoanCount = 0;
            decimal loanAmount = 0;
            Label[] labItemNams = this.GetLabItemNameControls();
            TextBox[] tbxItemLoans = this.GetTbxItemLoanControls();
            for (int idx = 0; idx < this.KeepReceiveItemCount; idx++)
            {
                string txt = tbxItemLoans[idx].Text.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    itemLoanCount++;
                    decimal value = 0;
                    if (Decimal.TryParse(txt, out value) && value >= 0)
                    {
                        loanAmount += value;
                        data.SetLoanItemAmount(idx + 1, value);
                    }
                    else
                    {
                        this.ShowJsAlert(String.Format("{0}不是合法的金額 (必須大於或等於 0)", HttpUtility.HtmlEncode(labItemNams[idx].Text)));
                        return false;
                    }
                }
            }
            data.LoanAmount = loanAmount;
            #endregion


            if (itemLoanCount > 0 && data.LoanFixamount.Value != loanAmount)
            {
                this.ShowJsAlert("就貸總金額與就貸明細合計金額不合");
                return false;
            }

            return true;
        }

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

                #region [Old] 土銀部使用原有的部別 DepListEntity 改用傳用的 DeptListEntity
                //if (String.IsNullOrEmpty(this.EditReceiveType)
                //    || String.IsNullOrEmpty(this.EditYearId)
                //    || String.IsNullOrEmpty(this.EditTermId)
                //    || String.IsNullOrEmpty(this.EditDepId)
                //    || String.IsNullOrEmpty(this.EditReceiveId)
                //    || String.IsNullOrEmpty(this.EditStuId)
                //    || (this.Action != ActionMode.Modify && this.Action != ActionMode.Delete))
                //{
                //    //[TODO] 固定顯示訊息的收集
                //    string msg = this.GetLocalized("網頁參數不正確");
                //    this.ShowSystemMessage(msg);
                //    return;
                //}
                #endregion

                int editOldSeq = 0;
                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                    || this.EditDepId == null
                    || String.IsNullOrEmpty(this.EditReceiveId)
                    || String.IsNullOrEmpty(this.EditStuId)
                    || !Int32.TryParse(oldSeq, out editOldSeq) || editOldSeq < 0
                    || (this.Action != ActionMode.Modify && this.Action != ActionMode.Delete && this.Action != ActionMode.Insert))
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

                #region 檢查商家代號授權
                if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.GetAndBindLoanIdOptions(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId);

                #region 取得維護資料
                string[] receiveItemNames = null;
                StudentReceiveView4 studentReceive = null;
                StudentLoanEntity studentLoan = null;
                bool isOK = this.GetEditData(out receiveItemNames, out studentReceive, out studentLoan);
                //if (isOK)
                //{
                //    this.EditLoanId = studentReceive.LoanId;
                //}
                #endregion

                this.BindEditData(receiveItemNames, studentReceive, studentLoan);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "B2100008.aspx";
            StudentLoanEntity data = null;
            XmlResult xmlResult = null;
            switch (this.Action)
            {
                case ActionMode.Delete:     //刪除
                    #region 刪除
                    {
                        data = new StudentLoanEntity();
                        data.ReceiveType = this.EditReceiveType;
                        data.YearId = this.EditYearId;
                        data.TermId = this.EditTermId;
                        data.DepId = this.EditDepId;
                        data.ReceiveId = this.EditReceiveId;
                        data.StuId = this.EditStuId;
                        data.OldSeq = this.EditOldSeq;

                        xmlResult = DataProxy.Current.EditStudentLoanData(this.Page, this.Action, data);
                    }
                    break;
                    #endregion
                case ActionMode.Insert:     //新增
                case ActionMode.Modify:     //修改
                    #region 新增 || 修改
                    if (this.GetAndCheckData(out data))
                    {
                        xmlResult = DataProxy.Current.EditStudentLoanData(this.Page, this.Action, data);
                    }
                    else
                    {
                        return;
                    }
                    break;
                    #endregion
                default:
                    xmlResult = new XmlResult(false, "不正確的頁面操作參數", CoreStatusCode.UNKNOWN_ERROR, null);
                    break;
            }

            if (xmlResult.IsSuccess)
            {
                //WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);
                this.ShowActionSuccessAlert(action, backUrl);
                return;
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return;
            }
        }
    }
}