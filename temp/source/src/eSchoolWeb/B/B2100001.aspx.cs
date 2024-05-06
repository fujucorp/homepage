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
    /// 單筆新增學生繳費資料
    /// </summary>
    public partial class B2100001 : BasePage
    {
        //[MDY:2018xxxx] 修改很大，整個重寫

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
                ViewState["ACTION"] = value == null ? String.Empty : value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// 編輯的商家代號參數
        /// </summary>
        private string EditReceiveType
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditReceiveType"]) as string;
            }
            set
            {
                ViewState["EditReceiveType"] = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學年參數
        /// </summary>
        private string EditYearId
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditYearId"]) as string;
            }
            set
            {
                ViewState["EditYearId"] = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學期參數
        /// </summary>
        private string EditTermId
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditTermId"]) as string;
            }
            set
            {
                ViewState["EditTermId"] = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的部別參數 (土銀沒有部別，固定傳回空字串)
        /// </summary>
        private string EditDepId
        {
            get
            {
                //土銀沒有部別，固定傳回空字串
                return String.Empty;
            }
            set
            {
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
                ViewState["EditReceiveId"] = value == null ? String.Empty : value.Trim();
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
                ViewState["EditStuId"] = value == null ? String.Empty : value.Trim();
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
                    return -1;
                }
            }
            set
            {
                ViewState["EditOldSeq"] = value < 0 ? -1 : value;
            }
        }

        /// <summary>
        /// 是否固定學號 (B2100002.aspx 轉過來的，針對某學號的新增)
        /// </summary>
        private bool IsFixStuId
        {
            get
            {
                object value = ViewState["IsFixStuId"];
                if (value is bool)
                {
                    return (bool)value;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["IsFixStuId"] = value;
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            #region 學生基本資料區塊
            this.tbxStuId.Text = String.Empty;
            this.tbxName.Text = String.Empty;
            this.tbxIdNumber.Text = String.Empty;
            this.tbxTel.Text = String.Empty;
            this.tbxBirthday.Text = String.Empty;
            this.tbxZipCode.Text = String.Empty;
            this.tbxAddress.Text = String.Empty;
            this.tbxEmail.Text = String.Empty;
            this.tbxStuParent.Text = String.Empty;
            #endregion

            #region 繳費資料區塊
            {
                #region [MDY:20150201] 批號、資料序號與繳款期限
                this.labUpNo.Text = String.Empty;
                this.tbxPayDueDate.Text = String.Empty;
                #endregion

                #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
                {
                    #region 檢查是否有設定國際信用卡財金參數
                    bool hasNCCardSetting = false;
                    {
                        //因為 InitialUI 在 PageLoad 執行時 ucFilter1 還沒初始化，所以要用 WebHelper.GetFilterArguments 取得 receiveType
                        string receiveType = null;
                        string yearId = null;
                        string termId = null;
                        string depId = null;
                        string receiveId = null;
                        if (WebHelper.GetFilterArguments(out receiveType, out yearId, out termId, out depId, out receiveId) && !String.IsNullOrEmpty(receiveType))
                        {
                            Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType)
                                .And(SchoolRTypeEntity.Field.MerchantId2, RelationEnum.NotEqual, String.Empty)
                                .And(SchoolRTypeEntity.Field.MerchantId2, RelationEnum.NotEqual, null)
                                .And(SchoolRTypeEntity.Field.TerminalId2, RelationEnum.NotEqual, String.Empty)
                                .And(SchoolRTypeEntity.Field.TerminalId2, RelationEnum.NotEqual, null)
                                .And(SchoolRTypeEntity.Field.MerId2, RelationEnum.NotEqual, String.Empty)
                                .And(SchoolRTypeEntity.Field.MerId2, RelationEnum.NotEqual, null);

                            int count = 0;
                            XmlResult xmlResult = DataProxy.Current.SelectCount<SchoolRTypeEntity>(this.Page, where, out count);
                            if (xmlResult.IsSuccess)
                            {
                                hasNCCardSetting = (count > 0);
                            }
                            else
                            {
                                this.ShowJsAlert("查詢學校的國際信用卡參數資料失敗");
                            }
                        }
                    }
                    #endregion

                    this.labNCCardFlag.Text = String.Format("{0}：", this.GetLocalized("國際信用卡繳費"));
                    this.labNCCardFlag.Visible = hasNCCardSetting;

                    CodeText[] items = new CodeText[2] { new CodeText("Y", "啟用"), new CodeText("N", "停用") };
                    WebHelper.SetDropDownListItems(this.ddlNCCardFlag, DefaultItem.Kind.None, false, items, false, true, 0, items[1].Code);
                    this.ddlNCCardFlag.Visible = hasNCCardSetting;
                }
                #endregion

                #region [MDY:202203XX] 這個階段還沒選商家代號且 Initial() 之後會執行，所以 MARK
                //#region 部別、院別、科系、班別、住宿、減免、就貸、身份註記 1 ~ 6
                //this.GetAndBindOptions();
                //#endregion
                #endregion

                #region [MDY:202203XX] 這個階段還沒選商家代號，所以移到 Bind 資料時執行
                //#region 年級
                //bool useEngDataUI = this.UseEngDataUI(this.EditReceiveType, !this.IsPostBack);
                //WebHelper.SetDropDownListItems(this.ddlStuGrade, DefaultItem.Kind.Omit, false, new GradeCodeTexts(useEngDataUI), false, true, 0, null);
                //#endregion
                #endregion

                #region 補單註記、計算方式
                WebHelper.SetDropDownListItems(this.ddlReissueFlag, DefaultItem.Kind.Omit, false, new ReissueFlagCodeTexts(), false, true, 0, null);
                WebHelper.SetDropDownListItems(this.ddlBillingType, DefaultItem.Kind.None, false, new BillingTypeCodeTexts(), false, true, 0, BillingTypeCodeTexts.BY_AMOUNT);
                #endregion

                #region 座號、上傳就學貸款金額、可貸金額、學分數、上課時數
                this.tbxStuHid.Text = String.Empty;
                this.tbxLoan.Text = String.Empty;
                this.tbxLoanAmount.Text = String.Empty;
                this.tbxStuCredit.Text = String.Empty;
                this.tbxStuHour.Text = String.Empty;
                #endregion
            }
            #endregion

            #region 收入明細區塊
            {
                HtmlTableRow[] trItemRows = this.GetReceiveItemHtmlTableRows();
                foreach (HtmlTableRow trItemRow in trItemRows)
                {
                    trItemRow.Visible = false;
                }
            }
            #endregion

            #region 合計項目區塊
            this.litReceiveSumHtml.Text = String.Empty;
            #endregion

            #region 備註區塊
            {
                HtmlTableRow[] trMemoRows = this.GetMemoHtmlTableRows();
                foreach (HtmlTableRow trMemoRow in trMemoRows)
                {
                    trMemoRow.Visible = false;
                }
            }
            #endregion

            #region 學生扣款轉帳資料區塊
            this.tbxDeductBankId.Text = String.Empty;
            this.tbxDeductAccountNo.Text = String.Empty;
            this.tbxDeductAccountName.Text = String.Empty;
            this.tbxDeductAccountId.Text = String.Empty;
            #endregion

            #region 繳費/銷帳資料區塊
            this.labReceiveAmount.Text = String.Empty;
            this.labCancelNo.Text = String.Empty;
            this.labReceiveATMAmount.Text = String.Empty;
            this.labCancelATMNo.Text = String.Empty;
            this.labReceiveSMAmount.Text = String.Empty;
            this.labCancelSMNo.Text = String.Empty;
            #endregion

            #region Button
            this.ccbtnSave.Visible = false;
            this.ccbtnCalc.Visible = false;
            this.ccbtnGenBill.Visible = false;
            this.ccbtnGenEngBill.Visible = false;
            this.ccbtnNewData.Visible = false;
            #endregion
        }

        /// <summary>
        /// 取得並結繫部別、院別、科系、班別、住宿、減免、就貸、身份註記 1 ~ 6 下拉選項
        /// </summary>
        private void GetAndBindOptions()
        {
            this.ddlDeptId.Items.Clear();
            this.ddlCollegeId.Items.Clear();
            this.ddlMajorId.Items.Clear();
            this.ddlClassId.Items.Clear();
            this.ddlDormId.Items.Clear();
            this.ddlReduceId.Items.Clear();
            this.ddlLoanId.Items.Clear();
            this.ddlIdentifyId01.Items.Clear();
            this.ddlIdentifyId02.Items.Clear();
            this.ddlIdentifyId03.Items.Clear();
            this.ddlIdentifyId04.Items.Clear();
            this.ddlIdentifyId05.Items.Clear();
            this.ddlIdentifyId06.Items.Clear();

            CodeText[] deptDatas = null;
            CodeText[] collegeDatas = null;
            CodeText[] majorDatas = null;
            CodeText[] classDatas = null;
            CodeText[] dormDatas = null;
            CodeText[] reduceDatas = null;
            CodeText[] loanDatas = null;
            CodeText[] identify01Datas = null;
            CodeText[] identify02Datas = null;
            CodeText[] identify03Datas = null;
            CodeText[] identify04Datas = null;
            CodeText[] identify05Datas = null;
            CodeText[] identify06Datas = null;

            #region [MDY:2018xxxx] 整合成一個後台呼叫方法
            if (!String.IsNullOrEmpty(this.EditReceiveType) && !String.IsNullOrEmpty(this.EditYearId) && !String.IsNullOrEmpty(this.EditTermId))
            {
                #region [MDY:202203XX] 2022擴充案 英文資料介面
                bool useEngDataUI = this.UseEngDataUI(this.EditReceiveType, !this.IsPostBack);

                string[] codeTables = new string[] { "ALL" };
                XmlResult xmlResult = DataProxy.Current.GetCodeTableAllOptions(this.Page
                    , this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId
                    , codeTables, useEngDataUI
                    , out deptDatas, out collegeDatas, out majorDatas, out classDatas
                    , out dormDatas, out reduceDatas, out loanDatas
                    , out identify01Datas, out identify02Datas, out identify03Datas
                    , out identify04Datas, out identify05Datas, out identify06Datas);
                #endregion

                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, "讀取選項資料失敗" + xmlResult.Message);
                }
            }
            #endregion

            WebHelper.SetDropDownListItems(this.ddlDeptId, DefaultItem.Kind.Omit, false, deptDatas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlCollegeId, DefaultItem.Kind.Omit, false, collegeDatas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlMajorId, DefaultItem.Kind.Omit, false, majorDatas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlClassId, DefaultItem.Kind.Omit, false, classDatas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlDormId, DefaultItem.Kind.Omit, false, dormDatas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlReduceId, DefaultItem.Kind.Omit, false, reduceDatas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlLoanId, DefaultItem.Kind.Omit, false, loanDatas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlIdentifyId01, DefaultItem.Kind.Omit, false, identify01Datas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlIdentifyId02, DefaultItem.Kind.Omit, false, identify02Datas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlIdentifyId03, DefaultItem.Kind.Omit, false, identify03Datas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlIdentifyId04, DefaultItem.Kind.Omit, false, identify04Datas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlIdentifyId05, DefaultItem.Kind.Omit, false, identify05Datas, true, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlIdentifyId06, DefaultItem.Kind.Omit, false, identify06Datas, true, false, 0, null);
        }

        /// <summary>
        /// 取得維護的相關資料
        /// </summary>
        /// <param name="schoolRid">成功則傳回代收費用檔資料，否則傳回 null</param>
        /// <param name="receiveSums">成功則傳回合計項目設定資料陣列，否則傳回 null</param>
        /// <param name="studentMaster">成功則傳回要編輯的學生基本資料，否則傳回 null</param>
        /// <param name="studentReceive">成功則傳回要編輯的學生繳費資料，否則傳回 null</param>
        /// <param name="lastStudentReceive">如有指定學號但查無學生繳費資料，則傳回最近一筆學生繳費資料，否則傳回 null</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetEditData(out SchoolRidEntity schoolRid, out ReceiveSumEntity[] receiveSums, out StudentMasterEntity studentMaster, out StudentReceiveEntity studentReceive
            , out StudentReceiveEntity lastStudentReceive)
        {
            schoolRid = null;
            receiveSums = null;
            studentMaster = null;
            studentReceive = null;
            lastStudentReceive = null;

            string action = this.GetLocalized("查詢維護的相關資料");

            List<string> dataKinds = new List<string>(5);

            string editReceiveId = this.EditReceiveId;
            if (!String.IsNullOrEmpty(editReceiveId))
            {
                dataKinds.Add("SchoolRid");
                dataKinds.Add("ReceiveSum");
            }
            string editStuId = this.EditStuId;
            int? editOldSeq = null;
            if (!String.IsNullOrEmpty(editStuId))
            {
                dataKinds.Add("StudentMaster");
                int oldSeq = this.EditOldSeq;
                if (oldSeq > -1)
                {
                    editOldSeq = oldSeq;
                    dataKinds.Add("StudentReceive");
                    dataKinds.Add("LastStudentReceive");
                }
                else if (this.Action == ActionMode.Insert)
                {
                    dataKinds.Add("LastStudentReceive");
                }
            }
            else
            {
                if (this.Action == ActionMode.Modify)
                {
                    string msg = this.GetLocalized("無法取得學號參數");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }

            if (dataKinds.Count > 0)
            {
                #region [MDY:2018xxxx] 整合成一個後台呼叫方法
                StudentLoanEntity studentLoan = null;
                XmlResult result = DataProxy.Current.GetStudentBillDatas(this.Page, this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, editReceiveId, editStuId, editOldSeq, dataKinds,
                    out schoolRid, out studentMaster, out studentReceive, out receiveSums, out studentLoan, out lastStudentReceive);
                if (!result.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, result.Code, result.Message);
                    return false;
                }
                #endregion

                if (this.Action == ActionMode.Modify)
                {
                    if (schoolRid == null)
                    {
                        string msg = this.GetLocalized("查無該代收費用檔資料");
                        this.ShowSystemMessage(ErrorCode.D_DATA_NOT_FOUND, msg);
                        return false;
                    }
                    if (studentMaster == null)
                    {
                        string msg = this.GetLocalized("查無該學生基本資料");
                        this.ShowSystemMessage(ErrorCode.D_DATA_NOT_FOUND, msg);
                        return false;
                    }
                    if (studentReceive == null)
                    {
                        string msg = this.GetLocalized("查無該學生繳費資料");
                        this.ShowSystemMessage(ErrorCode.D_DATA_NOT_FOUND, msg);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 結繫編輯資料
        /// </summary>
        /// <param name="schoolRid">指定代收費用檔資料</param>
        /// <param name="receiveSums">指定合計項目設定資料</param>
        /// <param name="studentMaster">指定學生基本資料</param>
        /// <param name="studentReceive">指定學生繳費單資料</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool BindEditData(SchoolRidEntity schoolRid, ReceiveSumEntity[] receiveSums, StudentMasterEntity studentMaster, StudentReceiveEntity studentReceive, StudentReceiveEntity lastStuReceive = null)
        {
            bool isFixStuId = this.IsFixStuId;

            #region 學生基本資料區塊
            this.BindStudentBlockUI(studentMaster);

            #region [TMP]
            //{
            //    this.tbxStuId.Enabled = !isFixStuId;
            //    this.tbxName.Enabled = true;
            //    this.tbxIdNumber.Enabled = true;
            //    this.tbxTel.Enabled = true;
            //    this.tbxBirthday.Enabled = true;
            //    this.tbxZipCode.Enabled = true;
            //    this.tbxAddress.Enabled = true;
            //    this.tbxEmail.Enabled = true;
            //    this.tbxStuParent.Enabled = true;

            //    if (isFixStuId && studentMaster != null)
            //    {
            //        this.tbxName.Enabled = false;
            //        this.tbxIdNumber.Enabled = false;
            //        this.tbxTel.Enabled = false;
            //        this.tbxBirthday.Enabled = false;
            //        this.tbxZipCode.Enabled = false;
            //        this.tbxAddress.Enabled = false;
            //        this.tbxEmail.Enabled = false;
            //        this.tbxStuParent.Enabled = false;
            //    }

            //    if (studentMaster == null)
            //    {
            //        studentMaster = new StudentMasterEntity();
            //        studentMaster.ReceiveType = this.EditReceiveType;
            //        studentMaster.DepId = this.EditDepId;
            //        studentMaster.Id = this.EditStuId;
            //    }

            //    this.tbxStuId.Text = studentMaster.Id;
            //    this.tbxName.Text = studentMaster.Name;
            //    this.tbxIdNumber.Text = studentMaster.IdNumber;
            //    this.tbxTel.Text = studentMaster.Tel;

            //    DateTime? date = DataFormat.ConvertDateText(studentMaster.Birthday);
            //    if (date.HasValue)
            //    {
            //        this.tbxBirthday.Text = DataFormat.GetDateText(date.Value);
            //    }
            //    else
            //    {
            //        this.tbxBirthday.Text = studentMaster.Birthday;
            //    }

            //    this.tbxZipCode.Text = studentMaster.ZipCode;
            //    this.tbxAddress.Text = studentMaster.Address;
            //    this.tbxEmail.Text = studentMaster.Email;
            //    this.tbxStuParent.Text = studentMaster.StuParent;
            //}
            #endregion
            #endregion

            #region [MDY:202203XX] 2022擴充案 是否使用英文資料介面
            bool useEngDataUI = this.UseEngDataUI(this.EditReceiveType, !this.IsPostBack);
            bool isEngEabled = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
            #endregion

            #region [MDY:202203XX] 2022擴充案 年級英文名稱相關
            WebHelper.SetDropDownListItems(this.ddlStuGrade, DefaultItem.Kind.Omit, false, new GradeCodeTexts(useEngDataUI), false, true, 0, null);
            #endregion

            #region 繳費資料區塊
            if (studentReceive != null)
            {
                #region [MDY:20150201] 批號、資料序號與繳款期限
                this.labUpNo.Text = String.Format("{0}、{1}", studentReceive.UpNo, studentReceive.OldSeq);
                this.tbxPayDueDate.Text = DataFormat.ConvertTWDate7ToDate(studentReceive.PayDueDate);
                #endregion

                #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
                {
                    string nCCard = "Y".Equals(studentReceive.NCCardFlag) ? "Y" : "N";
                    WebHelper.SetDropDownListSelectedValue(this.ddlNCCardFlag, nCCard);
                }
                #endregion

                #region 部別、院別、科系、年級、班別
                WebHelper.SetDropDownListSelectedValue(this.ddlDeptId, studentReceive.DeptId);
                WebHelper.SetDropDownListSelectedValue(this.ddlCollegeId, studentReceive.CollegeId);
                WebHelper.SetDropDownListSelectedValue(this.ddlMajorId, studentReceive.MajorId);
                WebHelper.SetDropDownListSelectedValue(this.ddlStuGrade, studentReceive.StuGrade);
                WebHelper.SetDropDownListSelectedValue(this.ddlClassId, studentReceive.ClassId);
                #endregion

                #region 住宿、減免、就貸、身份註記 1 ~ 6
                WebHelper.SetDropDownListSelectedValue(this.ddlDormId, studentReceive.DormId);
                WebHelper.SetDropDownListSelectedValue(this.ddlReduceId, studentReceive.ReduceId);
                WebHelper.SetDropDownListSelectedValue(this.ddlLoanId, studentReceive.LoanId);

                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId01, studentReceive.IdentifyId01);
                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId02, studentReceive.IdentifyId02);
                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId03, studentReceive.IdentifyId03);
                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId04, studentReceive.IdentifyId04);
                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId05, studentReceive.IdentifyId05);
                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId06, studentReceive.IdentifyId06);
                #endregion

                #region 補單註記、計算方式
                WebHelper.SetDropDownListSelectedValue(this.ddlReissueFlag, studentReceive.ReissueFlag);
                if (BillingTypeCodeTexts.GetCodeText(studentReceive.BillingType) == null)
                {
                    WebHelper.SetDropDownListSelectedValue(this.ddlBillingType, BillingTypeCodeTexts.BY_AMOUNT);  //預設依金額計算
                }
                else
                {
                    WebHelper.SetDropDownListSelectedValue(this.ddlBillingType, studentReceive.BillingType);
                }
                #endregion

                #region 座號、上傳就學貸款金額、可貸金額、學分數、上課時數
                this.tbxStuHid.Text = studentReceive.StuHid;
                this.tbxLoan.Text = DataFormat.GetAmountText(studentReceive.Loan);
                this.tbxLoanAmount.Text = DataFormat.GetAmountText(studentReceive.LoanAmount);
                this.tbxStuCredit.Text = DataFormat.GetAmountText(studentReceive.StuCredit);
                this.tbxStuHour.Text = DataFormat.GetAmountText(studentReceive.StuHour);
                #endregion
            }
            else
            {
                #region [MDY:20150201] 批號、資料序號與繳款期限
                this.labUpNo.Text = String.Empty;
                this.tbxPayDueDate.Text = String.Empty;
                #endregion

                #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
                {
                    WebHelper.SetDropDownListSelectedValue(this.ddlNCCardFlag, "N");
                }
                #endregion

                #region 部別、院別、科系、年級、班別
                if (lastStuReceive != null)
                {
                    WebHelper.SetDropDownListSelectedValue(this.ddlDeptId, lastStuReceive.DeptId);
                    WebHelper.SetDropDownListSelectedValue(this.ddlCollegeId, lastStuReceive.CollegeId);
                    WebHelper.SetDropDownListSelectedValue(this.ddlMajorId, lastStuReceive.MajorId);
                    WebHelper.SetDropDownListSelectedValue(this.ddlStuGrade, lastStuReceive.StuGrade);
                    WebHelper.SetDropDownListSelectedValue(this.ddlClassId, lastStuReceive.ClassId);
                }
                else
                {
                    WebHelper.SetDropDownListSelectedValue(this.ddlDeptId, String.Empty);
                    WebHelper.SetDropDownListSelectedValue(this.ddlCollegeId, String.Empty);
                    WebHelper.SetDropDownListSelectedValue(this.ddlMajorId, String.Empty);
                    WebHelper.SetDropDownListSelectedValue(this.ddlStuGrade, String.Empty);
                    WebHelper.SetDropDownListSelectedValue(this.ddlClassId, String.Empty);
                }
                #endregion

                #region 住宿、減免、就貸、身份註記 1 ~ 6
                WebHelper.SetDropDownListSelectedValue(this.ddlDormId, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlReduceId, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlLoanId, String.Empty);

                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId01, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId02, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId03, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId04, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId05, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyId06, String.Empty);
                #endregion

                #region 補單註記、計算方式
                WebHelper.SetDropDownListSelectedValue(this.ddlReissueFlag, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlBillingType, BillingTypeCodeTexts.BY_AMOUNT);  //預設依金額計算
                #endregion

                #region 座號、上傳就學貸款金額、可貸金額、學分數、上課時數
                this.tbxStuHid.Text = String.Empty;
                this.tbxLoan.Text = String.Empty;
                this.tbxLoanAmount.Text = String.Empty;
                this.tbxStuCredit.Text = String.Empty;
                this.tbxStuHour.Text = String.Empty;
                #endregion
            }
            #endregion

            bool isOK = true;

            #region 收入明細區塊
            #region [MDY:202203XX] 2022擴充案 是否使用英文資料介面
            isOK &= this.BindReceiveItemBolckUI(schoolRid, studentReceive, useEngDataUI);
            #endregion
            #endregion

            #region 合計項目區塊
            #region [MDY:202203XX] 2022擴充案 是否使用英文資料介面
            isOK &= this.BindReceiveSumHtml(receiveSums, studentReceive, useEngDataUI);
            #endregion
            #endregion

            #region 備註區塊
            isOK &= this.BindMemoBlockUI(schoolRid, studentReceive);
            #endregion

            #region 學生扣款轉帳資料區塊
            if (studentReceive != null)
            {
                this.tbxDeductBankId.Text = studentReceive.DeductBankid;
                this.tbxDeductAccountNo.Text = studentReceive.DeductAccountno;
                this.tbxDeductAccountName.Text = studentReceive.DeductAccountname;
                this.tbxDeductAccountId.Text = studentReceive.DeductAccountid;
            }
            else
            {
                this.tbxDeductBankId.Text = String.Empty;
                this.tbxDeductAccountNo.Text = String.Empty;
                this.tbxDeductAccountName.Text = String.Empty;
                this.tbxDeductAccountId.Text = String.Empty;
            }
            #endregion

            #region 繳費/銷帳資料區塊
            if (studentReceive != null)
            {
                this.labReceiveAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveAmount);
                this.labCancelNo.Text = studentReceive.CancelNo;
                this.labReceiveATMAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveAtmamount);
                this.labCancelATMNo.Text = studentReceive.CancelAtmno;
                this.labReceiveSMAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveSmamount);
                this.labCancelSMNo.Text = studentReceive.CancelSmno;
            }
            else
            {
                this.labReceiveAmount.Text = String.Empty;
                this.labCancelNo.Text = String.Empty;
                this.labReceiveATMAmount.Text = String.Empty;
                this.labCancelATMNo.Text = String.Empty;
                this.labReceiveSMAmount.Text = String.Empty;
                this.labCancelSMNo.Text = String.Empty;
            }
            #endregion

            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.ccbtnSave.Visible = isOK;
                    this.ccbtnCalc.Visible = false;
                    this.ccbtnGenBill.Visible = false;
                    this.ccbtnGenEngBill.Visible = isEngEabled && this.ccbtnGenBill.Visible;
                    this.ccbtnNewData.Visible = false;
                    break;
                case ActionMode.Modify:
                    this.DisableEditUI();
                    this.ccbtnSave.Visible = false;
                    if (isOK && studentReceive != null)
                    {
                        #region 學校端是否可以列印繳費單邏輯 (土銀特別允許金額小於 0 的資料也可以列印)
                        //1. 未繳費
                        //2. 金額大於 0 且有虛擬帳號的資料 或 金額小於或等於 0 (不管是否有虛擬帳號)
                        #endregion

                        if (studentReceive.ReceiveAmount.HasValue)
                        {
                            this.ccbtnCalc.Visible = false;
                            this.ccbtnGenBill.Visible = String.IsNullOrEmpty(studentReceive.ReceiveWay)
                                && ((studentReceive.ReceiveAmount > 0 && !String.IsNullOrEmpty(studentReceive.CancelNo)) || studentReceive.ReceiveAmount <= 0);
                            this.ccbtnGenEngBill.Visible = isEngEabled && this.ccbtnGenBill.Visible;
                        }
                        else
                        {
                            this.ccbtnCalc.Visible = true;
                            this.ccbtnGenBill.Visible = false;
                            this.ccbtnGenEngBill.Visible = isEngEabled && this.ccbtnGenBill.Visible;
                        }
                        this.ccbtnNewData.Visible = !isFixStuId;
                    }
                    else
                    {
                        this.ccbtnCalc.Visible = false;
                        this.ccbtnGenBill.Visible = false;
                        this.ccbtnGenEngBill.Visible = isEngEabled && this.ccbtnGenBill.Visible;
                        this.ccbtnNewData.Visible = false;
                    }
                    break;
                default:
                    this.ccbtnSave.Visible = false;
                    this.ccbtnCalc.Visible = false;
                    this.ccbtnGenBill.Visible = false;
                    this.ccbtnGenEngBill.Visible = isEngEabled && this.ccbtnGenBill.Visible;
                    this.ccbtnNewData.Visible = false;
                    break;
            }

            return isOK;
        }

        /// <summary>
        /// Disable 所有編輯控制項
        /// </summary>
        private void DisableEditUI()
        {
            ucFilter2.UIMode = FilterUIModeEnum.Label;

            #region 學生基本資料區塊
            this.tbxStuId.Enabled = false;
            this.tbxName.Enabled = false;
            this.tbxIdNumber.Enabled = false;
            this.tbxBirthday.Enabled = false;
            this.tbxTel.Enabled = false;
            this.tbxZipCode.Enabled = false;
            this.tbxAddress.Enabled = false;
            this.tbxEmail.Enabled = false;
            this.tbxStuParent.Enabled = false;
            #endregion

            #region 繳費資料區塊
            #region [MDY:20150201] 繳款期限
            this.tbxPayDueDate.Enabled = false;
            #endregion

            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
            this.ddlNCCardFlag.Enabled = false;
            #endregion

            this.ddlDeptId.Enabled = false;
            this.ddlCollegeId.Enabled = false;
            this.ddlMajorId.Enabled = false;
            this.ddlClassId.Enabled = false;
            this.ddlDormId.Enabled = false;
            this.ddlReduceId.Enabled = false;
            this.ddlLoanId.Enabled = false;
            this.ddlIdentifyId01.Enabled = false;
            this.ddlIdentifyId02.Enabled = false;
            this.ddlIdentifyId03.Enabled = false;
            this.ddlIdentifyId04.Enabled = false;
            this.ddlIdentifyId05.Enabled = false;
            this.ddlIdentifyId06.Enabled = false;

            this.ddlReissueFlag.Enabled = false;
            this.ddlBillingType.Enabled = false;
            this.ddlStuGrade.Enabled = false;

            this.tbxStuHid.Enabled = false;
            this.tbxLoan.Enabled = false;
            this.tbxLoanAmount.Enabled = false;
            this.tbxStuCredit.Enabled = false;
            this.tbxStuHour.Enabled = false;
            #endregion

            #region 收入明細區塊
            {
                TextBox[] tbxItemValues = this.GetReceiveItemValueTextBoxs();
                foreach (TextBox tbxItemValue in tbxItemValues)
                {
                    tbxItemValue.Enabled = false;
                }
            }
            #endregion

            #region 備註區塊
            {
                TextBox[] tbxMemoValues = this.GetMemoValueTextBoxs();
                foreach (TextBox tbxMemoValue in tbxMemoValues)
                {
                    tbxMemoValue.Enabled = false;
                }
            }
            #endregion

            #region 學生扣款轉帳資料區塊
            this.tbxDeductBankId.Enabled = false;
            this.tbxDeductAccountNo.Enabled = false;
            this.tbxDeductAccountName.Enabled = false;
            this.tbxDeductAccountId.Enabled = false;
            #endregion
        }

        #region 學生基本資料相關
        /// <summary>
        /// 結繫學生基本資料區塊介面
        /// </summary>
        /// <param name="studentMaster">指定學生基本資料</param>
        /// <param name="stuId">指定學號</param>
        private void BindStudentBlockUI(StudentMasterEntity studentMaster, string stuId = null)
        {
            bool noStudent = false;
            if (studentMaster == null)
            {
                noStudent = true;
                studentMaster = new StudentMasterEntity();
                studentMaster.ReceiveType = this.EditReceiveType;
                studentMaster.DepId = this.EditDepId;
                studentMaster.Id = this.EditStuId;
                if (String.IsNullOrEmpty(studentMaster.Id) && !String.IsNullOrWhiteSpace(stuId))
                {
                    studentMaster.Id = stuId.Trim();
                }
            }

            bool isFixStuId = this.IsFixStuId;

            #region 指定 Enabled 屬性
            this.tbxStuId.Enabled = !isFixStuId;
            this.tbxName.Enabled = noStudent;
            this.tbxIdNumber.Enabled = noStudent;
            this.tbxTel.Enabled = noStudent;
            this.tbxBirthday.Enabled = noStudent;
            this.tbxZipCode.Enabled = noStudent;
            this.tbxAddress.Enabled = noStudent;
            this.tbxEmail.Enabled = noStudent;
            this.tbxStuParent.Enabled = noStudent;
            #endregion

            #region 指定輸入格的值
            #region [MDY:2018xxxx] 避免原碼掃描誤判
            #region [OLD]
            //this.tbxStuId.Text = studentMaster.Id;
            #endregion

            this.tbxStuId.Text = Server.HtmlEncode(studentMaster.Id);
            #endregion

            this.tbxName.Text = studentMaster.Name;
            this.tbxIdNumber.Text = studentMaster.IdNumber;
            this.tbxTel.Text = studentMaster.Tel;

            DateTime? date = DataFormat.ConvertDateText(studentMaster.Birthday);
            if (date.HasValue)
            {
                this.tbxBirthday.Text = DataFormat.GetDateText(date.Value);
            }
            else
            {
                this.tbxBirthday.Text = studentMaster.Birthday;
            }

            this.tbxZipCode.Text = studentMaster.ZipCode;
            this.tbxAddress.Text = studentMaster.Address;
            this.tbxEmail.Text = studentMaster.Email;
            this.tbxStuParent.Text = studentMaster.StuParent;
            #endregion
        }
        #endregion

        #region 收入明細相關
        /// <summary>
        /// 取得收入科目 TR 的 HtmlTableRow 控制項陣列
        /// </summary>
        /// <returns></returns>
        private HtmlTableRow[] GetReceiveItemHtmlTableRows()
        {
            return new HtmlTableRow[] {
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

        /// <summary>
        /// 取得收入科目 名稱 的 Label 控制項陣列
        /// </summary>
        /// <returns></returns>
        private Label[] GetReceiveItemNameLabels()
        {
            return new Label[] {
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

        /// <summary>
        /// 取得收入科目 金額 的 TextBox 控制項陣列
        /// </summary>
        /// <returns></returns>
        private TextBox[] GetReceiveItemValueTextBoxs()
        {
            return new TextBox[] {
                this.tbxItemValue01, this.tbxItemValue02, this.tbxItemValue03, this.tbxItemValue04, this.tbxItemValue05,
                this.tbxItemValue06, this.tbxItemValue07, this.tbxItemValue08, this.tbxItemValue09, this.tbxItemValue10,
                this.tbxItemValue11, this.tbxItemValue12, this.tbxItemValue13, this.tbxItemValue14, this.tbxItemValue15,
                this.tbxItemValue16, this.tbxItemValue17, this.tbxItemValue18, this.tbxItemValue19, this.tbxItemValue20,
                this.tbxItemValue21, this.tbxItemValue22, this.tbxItemValue23, this.tbxItemValue24, this.tbxItemValue25,
                this.tbxItemValue26, this.tbxItemValue27, this.tbxItemValue28, this.tbxItemValue29, this.tbxItemValue30,
                this.tbxItemValue31, this.tbxItemValue32, this.tbxItemValue33, this.tbxItemValue34, this.tbxItemValue35,
                this.tbxItemValue36, this.tbxItemValue37, this.tbxItemValue38, this.tbxItemValue39, this.tbxItemValue40
            };
        }

        #region [MDY:202203XX] 2022擴充案 收入科目中文/英文名稱
        /// <summary>
        /// 結繫收入明細區塊介面
        /// </summary>
        /// <param name="schoolRid">指定代收費用檔</param>
        /// <param name="studentReceive">指定學生繳費資料</param>
        /// <param name="useEngDataUI">指定是否使用英文資料介面</param>
        /// <returns>失敗傳回 false，否則傳回 true</returns>
        private bool BindReceiveItemBolckUI(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive, bool useEngDataUI)
        {
            HtmlTableRow[] trItemRows = this.GetReceiveItemHtmlTableRows();
            if (schoolRid == null)
            {
                foreach (HtmlTableRow trItemRow in trItemRows)
                {
                    trItemRow.Visible = false;
                }
                return true;
            }

            #region [MDY:202203XX] 2022擴充案 收入科目 改寫，改用 GetAllReceiveItems()
            #region [OLD]
            //string[] itemNames = schoolRid.GetAllReceiveItems();
            #endregion

            string[] itemNames = schoolRid.GetAllReceiveItems(useEngDataUI);
            #endregion

            bool isOK = true;
            Label[] labItemNames = this.GetReceiveItemNameLabels();
            TextBox[] tbxItemValues = this.GetReceiveItemValueTextBoxs();
            Decimal?[] itemAmounts = studentReceive == null ? new Decimal?[tbxItemValues.Length] : studentReceive.GetAllReceiveItemAmounts();
            if (labItemNames.Length != tbxItemValues.Length)
            {
                string msg = this.GetLocalized("收入科目相關控制項個數不一致");
                this.ShowSystemMessage(msg);
                isOK = false;
            }
            else if (labItemNames.Length > itemNames.Length)
            {
                string msg = this.GetLocalized("收入科目名稱控制項個數不正確");
                this.ShowSystemMessage(msg);
                isOK = false;
            }
            else if (tbxItemValues.Length > itemAmounts.Length)
            {
                string msg = this.GetLocalized("收入科目金額控制項個數不正確");
                this.ShowSystemMessage(msg);
                isOK = false;
            }
            else if (labItemNames.Length != trItemRows.Length)
            {
                string msg = this.GetLocalized("收入科目行控制項個數不正確");
                this.ShowSystemMessage(msg);
                isOK = false;
            }

            //要先設定上層物件 Visible，否則下層控制項的 Visible 無法設為 true
            foreach (HtmlTableRow trItemRow in trItemRows)
            {
                trItemRow.Visible = isOK;
            }
            if (isOK)
            {
                for (int idx = 0; idx < labItemNames.Length; idx++)
                {
                    Label labItemName = labItemNames[idx];
                    TextBox tbxItemValue = tbxItemValues[idx];
                    string itemName = itemNames[idx];
                    if (String.IsNullOrWhiteSpace(itemName))
                    {
                        trItemRows[idx].Visible = false;
                        labItemName.Text = String.Empty;
                        labItemName.Visible = false;
                        tbxItemValue.Text = String.Empty;
                        tbxItemValue.Visible = false;
                    }
                    else
                    {
                        trItemRows[idx].Visible = true;
                        labItemName.Text = Server.HtmlEncode(itemName);
                        labItemName.Visible = true;
                        tbxItemValue.Text = DataFormat.GetAmountText(itemAmounts[idx]);
                        tbxItemValue.Visible = true;
                    }
                }
            }
            return isOK;
        }
        #endregion
        #endregion

        #region 合計項目相關
        #region [MDY:202203XX] 2022擴充案 合計項目中文/英文名稱
        /// <summary>
        /// 結繫合計項目 Html
        /// </summary>
        /// <param name="receiveSums">指定合計項目設定陣列</param>
        /// <param name="studentReceive">指定學生繳費資料</param>
        /// <param name="useEngDataUI">指定是否使用英文資料介面</param>
        /// <returns>失敗傳回 false，否則傳回 true</returns>
        private bool BindReceiveSumHtml(ReceiveSumEntity[] receiveSums, StudentReceiveEntity studentReceive, bool useEngDataUI)
        {
            bool isOK = true;
            StringBuilder html = new StringBuilder();
            if (receiveSums != null && receiveSums.Length > 0 && studentReceive != null)
            {
                string errmsg = null;
                html.AppendLine("<table width=\"100%\">")
                    .AppendLine("<tr><td colspan=\"2\" align=\"center\">合計項目</td></tr>");
                foreach (ReceiveSumEntity receiveSum in receiveSums)
                {
                    SubTotalAmount subTotal = SubTotalAmount.Create(studentReceive, receiveSum, true, useEngDataUI, out errmsg);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        html
                            .AppendLine("<tr>")
                            .AppendFormat("<td width=\"60%\">{0}</td>", Server.HtmlEncode(receiveSum.SumName)).AppendLine()
                            .AppendFormat("<td>{0}</td>", subTotal == null ? Server.HtmlEncode(errmsg) : DataFormat.GetAmountText(subTotal.Amount)).AppendLine()
                            .AppendLine("</tr>");
                    }
                    else
                    {
                        isOK = false;
                        this.ShowSystemMessage(String.Format("計算合計項目 {0} 失敗，{1}", receiveSum.SumId, errmsg));
                        break;
                    }
                }
                html.AppendLine("</table>");
            }
            this.litReceiveSumHtml.Text = html.ToString();
            return isOK;
        }
        #endregion
        #endregion

        #region 備註相關
        /// <summary>
        /// 取得備註 TR 的 HtmlTableRow 控制項陣列
        /// </summary>
        /// <returns></returns>
        private HtmlTableRow[] GetMemoHtmlTableRows()
        {
            return new HtmlTableRow[] {
                this.trMemoRow00,
                this.trMemoRow01, this.trMemoRow02, this.trMemoRow03, this.trMemoRow04, this.trMemoRow05,
                this.trMemoRow06, this.trMemoRow07, this.trMemoRow08, this.trMemoRow09, this.trMemoRow10,
                this.trMemoRow11
            };
        }

        /// <summary>
        /// 取得備註 名稱 的 Label 控制項陣列
        /// </summary>
        /// <returns></returns>
        private Label[] GetMemoTitleLabels()
        {
            return new Label[] {
                this.labMemoTitle01, this.labMemoTitle02, this.labMemoTitle03, this.labMemoTitle04, this.labMemoTitle05,
                this.labMemoTitle06, this.labMemoTitle07, this.labMemoTitle08, this.labMemoTitle09, this.labMemoTitle10,
                this.labMemoTitle11, this.labMemoTitle12, this.labMemoTitle13, this.labMemoTitle14, this.labMemoTitle15,
                this.labMemoTitle16, this.labMemoTitle17, this.labMemoTitle18, this.labMemoTitle19, this.labMemoTitle20,
                this.labMemoTitle21
            };
        }

        /// <summary>
        /// 取得備註 內容 的 TextBox 控制項陣列
        /// </summary>
        /// <returns></returns>
        private TextBox[] GetMemoValueTextBoxs()
        {
            TextBox[] tbxMemos = new TextBox[] {
                this.tbxMemoValue01, this.tbxMemoValue02, this.tbxMemoValue03, this.tbxMemoValue04, this.tbxMemoValue05,
                this.tbxMemoValue06, this.tbxMemoValue07, this.tbxMemoValue08, this.tbxMemoValue09, this.tbxMemoValue10,
                this.tbxMemoValue11, this.tbxMemoValue12, this.tbxMemoValue13, this.tbxMemoValue14, this.tbxMemoValue15,
                this.tbxMemoValue16, this.tbxMemoValue17, this.tbxMemoValue18, this.tbxMemoValue19, this.tbxMemoValue20,
                this.tbxMemoValue21
            };
            return tbxMemos;
        }

        /// <summary>
        /// 結繫備註區塊介面
        /// </summary>
        /// <param name="schoolRid">指定代收費用檔</param>
        /// <param name="studentReceive">指定學生繳費資料</param>
        /// <returns>失敗傳回 false，否則傳回 true</returns>
        private bool BindMemoBlockUI(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive)
        {
            HtmlTableRow[] trMemoRows = this.GetMemoHtmlTableRows();

            #region [MDY:202203XX] 2022擴充案 備註項目 改寫，改用 GetAllMemoTitle()
            #region [OLD]
            //string[] memoTitles = schoolRid == null ? null : schoolRid.GetAllMemoTitles();
            #endregion

            bool useEngDataUI = this.UseEngDataUI(this.EditReceiveType, !this.IsPostBack);
            string[] memoTitles = schoolRid?.GetAllMemoTitle(useEngDataUI);
            #endregion

            if (memoTitles == null || memoTitles.Length == 0)
            {
                foreach (HtmlTableRow trMemoRow in trMemoRows)
                {
                    trMemoRow.Visible = false;
                }
                return true;
            }

            bool isOK = true;
            Label[] labMemoTitles = this.GetMemoTitleLabels();
            TextBox[] tbxMemoValues = this.GetMemoValueTextBoxs();
            string[] memoValues = studentReceive == null ? new string[tbxMemoValues.Length] : studentReceive.GetAllMemoItemValues();
            if (labMemoTitles.Length != tbxMemoValues.Length)
            {
                string msg = this.GetLocalized("備註相關控制項個數不一致");
                this.ShowSystemMessage(msg);
                isOK = false;
            }
            else if (labMemoTitles.Length > memoTitles.Length)
            {
                string msg = this.GetLocalized("備註名稱控制項個數不正確");
                this.ShowSystemMessage(msg);
                isOK = false;
            }
            else if (tbxMemoValues.Length > memoValues.Length)
            {
                string msg = this.GetLocalized("備註設定值控制項個數不正確");
                this.ShowSystemMessage(msg);
                isOK = false;
            }
            else if ((labMemoTitles.Length / 2) > trMemoRows.Length)
            {
                string msg = this.GetLocalized("備註行控制項個數不正確");
                this.ShowSystemMessage(msg);
                isOK = false;
            }

            //要先設定上層物件 Visible，否則下層控制項的 Visible 無法設為 true
            foreach (HtmlTableRow trMemoRow in trMemoRows)
            {
                trMemoRow.Visible = isOK;
            }
            if (isOK)
            {
                bool hasMemeo = false;
                for (int idx = 0; idx < labMemoTitles.Length; idx++)
                {
                    Label labMemoTitle = labMemoTitles[idx];
                    TextBox tbxMemoValue = tbxMemoValues[idx];
                    string memoTitle = memoTitles[idx];
                    if (String.IsNullOrWhiteSpace(memoTitle))
                    {
                        labMemoTitle.Text = String.Empty;
                        labMemoTitle.Visible = false;
                        tbxMemoValue.Text = String.Empty;
                        tbxMemoValue.Visible = false;
                    }
                    else
                    {
                        labMemoTitle.Text = String.Concat(Server.HtmlEncode(memoTitle.Trim()), "：");
                        labMemoTitle.Visible = true;
                        tbxMemoValue.Text = memoValues[idx];
                        tbxMemoValue.Visible = true;
                        hasMemeo = true;
                    }

                    if (idx % 2 == 1)
                    {
                        int rowIndex = 1 + (idx / 2);
                        trMemoRows[rowIndex].Visible = (labMemoTitle.Visible || labMemoTitles[idx - 1].Visible);
                    }
                }
                this.trMemoRow00.Visible = hasMemeo;
            }
            return isOK;
        }
        #endregion

        /// <summary>
        /// 檢查並取得輸入資料
        /// </summary>
        /// <param name="studentMaster">成功則傳回輸入的學生基本資料，否則傳回 null</param>
        /// <param name="studentReceive">成功則傳回輸入的學生繳費資料，否則傳回 null</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckAndGetInputData(out StudentMasterEntity studentMaster, out StudentReceiveEntity studentReceive)
        {
            studentMaster = null;
            studentReceive = null;

            #region 商家代號、學年、學期
            if (String.IsNullOrEmpty(this.EditReceiveType) || String.IsNullOrEmpty(this.EditYearId) || String.IsNullOrEmpty(this.EditTermId))
            {
                string msg = this.GetLocalized("無法取得商家代號、學年或學期參數");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            #region 代收費用別
            if (String.IsNullOrEmpty(this.EditReceiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return false;
            }
            #endregion

            #region StudentMaster
            {
                StudentMasterEntity data = new StudentMasterEntity();

                data.ReceiveType = this.EditReceiveType;
                data.DepId = this.EditDepId;

                #region 學號
                if (this.IsFixStuId)
                {
                    data.Id = this.EditStuId;
                }
                else
                {
                    data.Id = this.tbxStuId.Text.Trim();
                }
                if (String.IsNullOrEmpty(data.Id))
                {
                    this.ShowMustInputAlert("學號");
                    return false;
                }
                #endregion

                #region 姓名
                //因為有學校會先建(新生)繳費單，等學生報到後才會知道姓名，所以不要求一定要有值
                data.Name = this.tbxName.Text.Trim();
                if (data.Name.Length > 60)
                {
                    string msg = this.GetLocalized("「姓名」最多 60 個中文字");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                #region 身分證字號
                //因為不一定是真的身分證字號，所以不檢查身分證字號邏輯
                data.IdNumber = this.tbxIdNumber.Text.Trim().ToUpper();
                if (data.IdNumber.Length > 0 && Encoding.UTF8.GetBytes(data.IdNumber).Length > 10)
                {
                    string msg = this.GetLocalized("「身分證字號」最多 10 碼");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                #region 電話
                data.Tel = this.tbxTel.Text.Trim();
                if (data.Tel.Length > 0 && Encoding.UTF8.GetBytes(data.Tel).Length > 14)
                {
                    string msg = this.GetLocalized("「電話」最多 14 碼");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                #region 生日
                data.Birthday = this.tbxBirthday.Text.Trim();
                if (!String.IsNullOrEmpty(data.Birthday))
                {
                    DateTime date;
                    if (DateTime.TryParse(data.Birthday, out date) && date.Year >= 1911)
                    {
                        data.Birthday = Common.GetTWDate7(date);
                    }
                    else
                    {
                        string msg = this.GetLocalized("「生日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                }
                #endregion

                #region 郵遞區號
                data.ZipCode = this.tbxZipCode.Text.Trim();
                if (data.ZipCode.Length > 0 && Encoding.UTF8.GetBytes(data.ZipCode).Length > 5)
                {
                    string msg = this.GetLocalized("「郵遞區號」最多 5 碼");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                #region 住址
                data.Address = this.tbxAddress.Text.Trim();
                if (data.Address.Length > 50)
                {
                    string msg = this.GetLocalized("「住址」最多 50 個中文字");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                #region 電子郵件
                data.Email = this.tbxEmail.Text.Trim();
                if (!String.IsNullOrEmpty(data.Email))
                {
                    if (Encoding.UTF8.GetBytes(data.Email).Length > 50)
                    {
                        string msg = this.GetLocalized("「電子郵件」最多 50 個字");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                    if (!Common.IsEmail(data.Email))
                    {
                        string msg = this.GetLocalized("「電子郵件」不是合法的電子郵件格式");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                }
                #endregion

                #region 家長名稱
                data.StuParent = this.tbxStuParent.Text.Trim();
                if (data.StuParent.Length > 60)
                {
                    string msg = this.GetLocalized("「家長名稱」最多 60 個中文字");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                #region [MDY:20200815] M202008_02 學生身分證字號不可與學號相同 (2020806_01)
                if (!String.IsNullOrEmpty(data.IdNumber) && data.IdNumber.Equals(data.Id))
                {
                    string msg = this.GetLocalized("「身分證字號」不可與「學號」相同");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                data.CrtDate = DateTime.Now;

                studentMaster = data;
            }
            #endregion

            #region StudentReceive
            {
                StudentReceiveEntity data = new StudentReceiveEntity();
                data.ReceiveType = this.EditReceiveType;
                data.YearId = this.EditYearId;
                data.TermId = this.EditTermId;
                data.DepId = this.EditDepId;
                data.ReceiveId = this.EditReceiveId;
                data.StuId = studentMaster.Id;

                #region [MDY:20150201] 繳款期限
                data.PayDueDate = this.tbxPayDueDate.Text.Trim();
                if (!String.IsNullOrEmpty(data.PayDueDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(data.PayDueDate, out date))
                    {
                        data.PayDueDate = Common.GetTWDate7(date);
                    }
                    else
                    {
                        string msg = this.GetLocalized("「繳款期限」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                }
                #endregion

                #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
                if (this.ddlNCCardFlag.Visible)
                {
                    data.NCCardFlag = this.ddlNCCardFlag.SelectedValue;
                    if (data.NCCardFlag != "Y" && data.NCCardFlag != "N")
                    {
                        this.ShowMustInputAlert("國際信用卡繳費");
                        return false;
                    }
                }
                else
                {
                    data.NCCardFlag = "N";
                }
                #endregion

                #region 部別、院別、科系、年級、班別、住宿、減免、就貸
                data.DeptId = this.ddlDeptId.SelectedValue;
                data.CollegeId = this.ddlCollegeId.SelectedValue;
                data.MajorId = this.ddlMajorId.SelectedValue;
                data.StuGrade = this.ddlStuGrade.SelectedValue;
                data.ClassId = this.ddlClassId.SelectedValue;
                data.DormId = this.ddlDormId.SelectedValue;
                data.ReduceId = this.ddlReduceId.SelectedValue;
                data.LoanId = this.ddlLoanId.SelectedValue;
                #endregion

                #region 身份註記一 ~ 六
                data.IdentifyId01 = this.ddlIdentifyId01.SelectedValue;
                data.IdentifyId02 = this.ddlIdentifyId02.SelectedValue;
                data.IdentifyId03 = this.ddlIdentifyId03.SelectedValue;
                data.IdentifyId04 = this.ddlIdentifyId04.SelectedValue;
                data.IdentifyId05 = this.ddlIdentifyId05.SelectedValue;
                data.IdentifyId06 = this.ddlIdentifyId06.SelectedValue;
                #endregion

                #region 補單註記、計算方式
                data.ReissueFlag = this.ddlReissueFlag.SelectedValue;
                data.BillingType = this.ddlBillingType.SelectedValue;
                #endregion

                #region 座號
                data.StuHid = this.tbxStuHid.Text.Trim();
                if (data.StuHid.Length > 0 && Encoding.UTF8.GetBytes(data.StuHid).Length > 10)
                {
                    string msg = this.GetLocalized("「座號」最多 10 碼");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                #region 上傳就學貸款金額
                data.Loan = 0;
                {
                    string loan = this.tbxLoan.Text.Trim();
                    if (!String.IsNullOrEmpty(loan))
                    {
                        decimal amount;
                        if (Decimal.TryParse(loan, out amount) && amount >= 0)
                        {
                            data.Loan = amount;
                        }
                        else
                        {
                            string msg = this.GetLocalized("「上傳就學貸款金額」不是合法的金額");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                    }
                }
                #endregion

                #region 可貸金額 (原就學貸款金額)
                data.LoanAmount = null;
                {
                    string loanAmount = this.tbxLoanAmount.Text.Trim();
                    if (!String.IsNullOrEmpty(loanAmount))
                    {
                        decimal amount;
                        if (Decimal.TryParse(loanAmount, out amount) && amount >= 0)
                        {
                            data.LoanAmount = amount;
                        }
                        else
                        {
                            string msg = this.GetLocalized("「可貸金額」不是合法的金額");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                    }
                }
                #endregion

                #region 學分數
                data.StuCredit = null;
                {
                    string stuCredit = this.tbxStuCredit.Text.Trim();
                    if (!String.IsNullOrEmpty(stuCredit))
                    {
                        Int32 value;
                        if (Int32.TryParse(stuCredit, out value) && value >= 0)
                        {
                            data.StuCredit = value;
                        }
                        else
                        {
                            string msg = this.GetLocalized("「學分數」不是合法的數字");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                    }
                }
                #endregion

                #region 上課時數
                data.StuHour = null;
                {
                    string stuHour = this.tbxStuHour.Text.Trim();
                    if (!String.IsNullOrEmpty(stuHour))
                    {
                        Int32 value;
                        if (Int32.TryParse(stuHour, out value) && value >= 0)
                        {
                            data.StuHour = value;
                        }
                        else
                        {
                            string msg = this.GetLocalized("「上課時數」不是合法的數字");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                    }
                }
                #endregion

                #region 收入科目金額
                {
                    Label[] labItemNames = this.GetReceiveItemNameLabels();
                    TextBox[] tbxItemValues = this.GetReceiveItemValueTextBoxs();
                    for (int idx = 0; idx < tbxItemValues.Length; idx++)
                    {
                        int no = idx + 1;
                        TextBox tbxItemValue = tbxItemValues[idx];
                        if (tbxItemValue.Visible)
                        {
                            decimal amount;
                            string value = tbxItemValue.Text.Trim();
                            if (!String.IsNullOrEmpty(value))
                            {
                                if (Decimal.TryParse(value, out amount))
                                {
                                    data.SetReceiveItemAmount(no, amount);
                                }
                                else
                                {
                                    string msg = String.Format("「{0}」{1}", labItemNames[idx].Text, this.GetLocalized("不是合法的金額"));
                                    this.ShowSystemMessage(msg);
                                    return false;
                                }
                            }
                            else
                            {
                                #region [MDY:20181207] 空白以 0 處理 (20181201_07)
                                #region [OLD]
                                //this.ShowMustInputAlert(labItemNames[idx].Text);
                                //return false;
                                #endregion

                                data.SetReceiveItemAmount(no, 0);
                                #endregion
                            }
                        }
                        else
                        {
                            data.SetReceiveItemAmount(no, null);
                        }
                    }
                }
                #endregion

                #region 備註內容
                {
                    Label[] labMemoTitles = this.GetMemoTitleLabels();
                    TextBox[] tbxMemoValues = this.GetMemoValueTextBoxs();
                    for (int idx = 0; idx < tbxMemoValues.Length; idx++)
                    {
                        int no = idx + 1;
                        TextBox tbxMemoValue = tbxMemoValues[idx];
                        if (tbxMemoValue.Visible)
                        {
                            string memoValue = tbxMemoValue.Text.Trim();
                            if (memoValue.Length <= 50)
                            {
                                data.SetMemoItemValue(no, memoValue);
                            }
                            else
                            {
                                string msg = String.Format("「{0}」{1}", labMemoTitles[idx], this.GetLocalized("最多 50 個中文字"));
                                this.ShowSystemMessage(msg);
                                return false;
                            }
                        }
                        else
                        {
                            data.SetMemoItemValue(no, String.Empty);
                        }
                    }
                }
                #endregion

                #region 學生扣款轉帳資料
                data.DeductBankid = this.tbxDeductBankId.Text.Trim();
                if (!Common.IsNumber(data.DeductBankid, 0, 7))
                {
                    string msg = this.GetLocalized("「扣款轉帳銀行代碼」最多 7 碼數字");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                data.DeductAccountno = this.tbxDeductAccountNo.Text.Trim();
                if (!Common.IsNumber(data.DeductAccountno, 0, 16))
                {
                    string msg = this.GetLocalized("「扣款轉帳銀行帳號」最多 16 碼數字");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                data.DeductAccountname = this.tbxDeductAccountName.Text.Trim();
                if (data.DeductAccountname.Length > 60)
                {
                    string msg = this.GetLocalized("「扣款轉帳銀行帳號戶名」最多 60 個中文字");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                data.DeductAccountid = this.tbxDeductAccountId.Text.Trim();
                if (data.DeductAccountid.Length > 10)
                {
                    string msg = this.GetLocalized("「扣款轉帳銀行帳戶ＩＤ」最多 10 碼");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                #region 不提供維護的欄位的預設值
                data.Exportreceivedata = string.Empty;
                data.MappingType = string.Empty;
                data.MappingId = string.Empty;
                data.Remark = string.Empty;

                data.UpNo = "0";    //單筆新增 批號 固定設為 0
                data.UpOrder = "";  //單筆新增 上傳該批資料的序號 固定設為空字串

                data.CreateDate = DateTime.Now;
                data.OldSeq = 0;    //系統新增的資料預設為 0
                #endregion

                studentReceive = data;
            }
            #endregion

            return true;
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

                #region 取消 Stu QueryString 參數，改用 Session 的 RedirectFrom 參數判斷是否由 B2100002.aspx 轉過來的，針對某學號的新增
                bool isFixStuId = false;
                KeyValueList<string> queryStrings = Session["QueryString"] as KeyValueList<string>;
                if (queryStrings != null && queryStrings.Count > 0)
                {
                    isFixStuId = (queryStrings.TryGetValue("RedirectFrom", null) == "B2100002");
                }
                #endregion

                this.IsFixStuId = isFixStuId;
                if (isFixStuId)
                {
                    #region 處理參數 (五KEY下拉選項)
                    string action = queryStrings.TryGetValue("Action", null);
                    string receiveType = queryStrings.TryGetValue("ReceiveType", null);
                    string yearId = queryStrings.TryGetValue("YearId", null);
                    string termId = queryStrings.TryGetValue("TermId", null);
                    string depId = queryStrings.TryGetValue("DepId", null);
                    string receiveId = queryStrings.TryGetValue("ReceiveId", null);
                    string stuId = queryStrings.TryGetValue("StuId", null);

                    //土銀不使用原有的部別 DepListEntity 改用專用的 DeptListEntity，所以 depId 預設值為空字串
                    if (!String.IsNullOrWhiteSpace(receiveType)
                        && !String.IsNullOrWhiteSpace(yearId)
                        && !String.IsNullOrWhiteSpace(termId)
                        && depId != null
                        && !String.IsNullOrWhiteSpace(receiveId)
                        && !String.IsNullOrWhiteSpace(stuId)
                        && (action == ActionMode.Insert))
                    {
                        #region 先保留五 Key的值，如果需要還原才有資料
                        string orgReceiveType = null;
                        string orgYearId = null;
                        string orgTermId = null;
                        string orgDepId = null;
                        string orgReceiveId = null;
                        bool orgFilterOK = WebHelper.GetFilterArguments(out orgReceiveType, out orgYearId, out orgTermId, out orgDepId, out orgReceiveId);
                        #endregion

                        this.ucFilter2.UIMode = FilterUIModeEnum.Label;
                        XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
                        if (xmlResult.IsSuccess)
                        {
                            if (receiveId != ucFilter2.SelectedReceiveID)
                            {
                                if (orgFilterOK)
                                {
                                    //一定要用這個方法將商家代號、學年、學期、部別、代收費用別參數傳保留起來
                                    //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
                                    WebHelper.SetFilterArguments(orgReceiveType, orgYearId, orgTermId, orgDepId, orgReceiveId);
                                }
                                string msg = this.GetLocalized("網頁參數不正確");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                        }
                        else
                        {
                            if (orgFilterOK)
                            {
                                //一定要用這個方法將商家代號、學年、學期、部別、代收費用別參數傳保留起來
                                //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
                                WebHelper.SetFilterArguments(orgReceiveType, orgYearId, orgTermId, orgDepId, orgReceiveId);
                            }
                            this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                            return;
                        }
                    }
                    else
                    {
                        string msg = this.GetLocalized("網頁參數錯誤");
                        this.ShowSystemMessage(msg);
                        return;
                    }

                    //一定要用這個方法將商家代號、學年、學期、部別、代收費用別參數傳保留起來
                    //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
                    WebHelper.SetFilterArguments(receiveType, yearId, termId, depId, receiveId);
                    #endregion

                    this.Action = action;
                    this.EditReceiveType = receiveType;
                    this.EditYearId = yearId;
                    this.EditTermId = termId;
                    this.EditDepId = depId;
                    this.EditReceiveId = receiveId;
                    this.EditStuId = stuId;
                    this.EditOldSeq = -1;
                }
                else
                {
                    #region 處理五KEY下拉選項
                    this.ucFilter2.UIMode = FilterUIModeEnum.Option;

                    string receiveType = null;
                    string yearId = null;
                    string termId = null;
                    string depId = null;
                    string receiveId = null;
                    if (!WebHelper.GetFilterArguments(out receiveType, out yearId, out termId, out depId, out receiveId)
                        || String.IsNullOrEmpty(receiveType)
                        || String.IsNullOrEmpty(yearId))
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("無法取得商家代號或學年參數");
                        this.ShowSystemMessage(msg);
                        return;
                    }

                    //因為控制項的 Page_Load 比頁面的 Page_Load 晚觸發，所以可以先執行 GetDataAndBind 方法，強迫 ucFilter1 結繫資料
                    //因為 ucFilter1 有指定 Filter2ControlID 為 ucFilter2，所以 ucFilter2 頁會被自動結繫資料
                    XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
                    if (xmlResult.IsSuccess)
                    {
                        depId = "";
                        receiveId = ucFilter2.SelectedReceiveID;
                    }

                    //一定要用這個方法將商家代號、學年、學期、部別、代收費用別參數傳保留起來
                    //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
                    WebHelper.SetFilterArguments(receiveType, yearId, termId, depId, receiveId);
                    #endregion

                    this.Action = ActionMode.Insert;
                    this.EditReceiveType = receiveType;
                    this.EditYearId = yearId;
                    this.EditTermId = termId;
                    this.EditDepId = depId;
                    this.EditReceiveId = receiveId;
                    this.EditStuId = null;
                    this.EditOldSeq = -1;
                }

                #region 檢查商家代號授權
                if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                    return;
                }
                #endregion

                #region 部別、院別、科系、班別、住宿、減免、就貸、身份註記 1 ~ 6
                this.GetAndBindOptions();
                #endregion

                #region 取資料並結繫
                SchoolRidEntity schoolRid = null;
                ReceiveSumEntity[] receiveSums = null;
                StudentMasterEntity studentMaster = null;
                StudentReceiveEntity studentReceive = null;
                StudentReceiveEntity lastStuReceive = null;
                if (this.GetEditData(out schoolRid, out receiveSums, out studentMaster, out studentReceive, out lastStuReceive))
                {
                    if (this.BindEditData(schoolRid, receiveSums, studentMaster, studentReceive, lastStuReceive))
                    {
                    }
                }
                #endregion
            }
            else
            {
                #region [MDY:2018xxxx] PostBack 後就不需要 Session 參數，清除 Session 參數
                if (Session["QueryString"] != null)
                {
                    Session.Remove("QueryString");
                }
                #endregion
            }
        }

        protected void ccbtnSave_Click(object sender, EventArgs e)
        {
            StudentMasterEntity studentMaster = null;
            StudentReceiveEntity studentReceive = null;

            if (!this.CheckAndGetInputData(out studentMaster, out studentReceive))
            {
                return;
            }

            string action = this.GetLocalized("儲存資料");

            bool isOk = true;

            #region [MDY:20150201] 開放同一費用別下同一學號可以有多筆資料，所以改成取最大 OldSeq
            {
                StudentReceiveEntity oldReceive = null;
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, studentReceive.ReceiveType)
                    .And(StudentReceiveEntity.Field.YearId, studentReceive.YearId)
                    .And(StudentReceiveEntity.Field.TermId, studentReceive.TermId)
                    .And(StudentReceiveEntity.Field.DepId, studentReceive.DepId)
                    .And(StudentReceiveEntity.Field.ReceiveId, studentReceive.ReceiveId)
                    .And(StudentReceiveEntity.Field.StuId, studentReceive.StuId)
                    .And(StudentReceiveEntity.Field.OldSeq, RelationEnum.LessEqual, StudentReceiveEntity.MaxOldSeq);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(StudentReceiveEntity.Field.OldSeq, OrderByEnum.Desc);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveEntity>(this.Page, where, orderbys, out oldReceive);
                if (xmlResult.IsSuccess)
                {
                    if (oldReceive != null)
                    {
                        studentReceive.OldSeq = oldReceive.OldSeq + 1;
                    }
                }
                else
                {
                    isOk = false;
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            #region 新增或修改 StudentMasterEntity
            if (isOk)
            {
                int count = 0;
                Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, studentMaster.ReceiveType)
                    .And(StudentMasterEntity.Field.DepId, studentMaster.DepId)
                    .And(StudentMasterEntity.Field.Id, studentMaster.Id);
                XmlResult xmlResult = DataProxy.Current.SelectCount<StudentMasterEntity>(this.Page, where, out count);
                if (xmlResult.IsSuccess)
                {
                    if (count > 0)
                    {
                        #region 更新
                        Expression where2 = new Expression(StudentMasterEntity.Field.ReceiveType, studentMaster.ReceiveType)
                            .And(StudentMasterEntity.Field.DepId, studentMaster.DepId)
                            .And(StudentMasterEntity.Field.Id, studentMaster.Id);
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(StudentMasterEntity.Field.Name, studentMaster.Name);
                        fieldValues.Add(StudentMasterEntity.Field.Birthday, studentMaster.Birthday);
                        fieldValues.Add(StudentMasterEntity.Field.IdNumber, studentMaster.IdNumber);
                        fieldValues.Add(StudentMasterEntity.Field.Tel, studentMaster.Tel);
                        fieldValues.Add(StudentMasterEntity.Field.ZipCode, studentMaster.ZipCode);
                        fieldValues.Add(StudentMasterEntity.Field.Address, studentMaster.Address);
                        fieldValues.Add(StudentMasterEntity.Field.Email, studentMaster.Email);

                        xmlResult = DataProxy.Current.UpdateFields<StudentMasterEntity>(this.Page, where2, fieldValues, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                isOk = false;
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                        }
                        else
                        {
                            isOk = false;
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 新增
                        xmlResult = DataProxy.Current.Insert<StudentMasterEntity>(this.Page, studentMaster, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                isOk = false;
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                            }
                        }
                        else
                        {
                            isOk = false;
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                        #endregion
                    }
                }
                else
                {
                    isOk = false;
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            #region 新增 StudentReceiveEntity
            if (isOk)
            {
                int count = 0;
                XmlResult xmlResult = DataProxy.Current.Insert<StudentReceiveEntity>(this.Page, studentReceive, out count);
                if (xmlResult.IsSuccess)
                {
                    if (count < 1)
                    {
                        isOk = false;
                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                    }
                }
                else
                {
                    isOk = false;
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            if (isOk)
            {
                #region 新增成功後，改用編輯模式重新結細
                {
                    this.Action = ActionMode.Modify;
                    this.EditStuId = studentReceive.StuId;
                    this.EditOldSeq = studentReceive.OldSeq;

                    SchoolRidEntity schoolRid = null;
                    ReceiveSumEntity[] receiveSums = null;
                    StudentReceiveEntity lastStuReceive = null;
                    if (this.GetEditData(out schoolRid, out receiveSums, out studentMaster, out studentReceive, out lastStuReceive))
                    {
                        if (this.BindEditData(schoolRid, receiveSums, studentMaster, studentReceive, lastStuReceive))
                        {
                        }
                    }
                }
                #endregion

                this.ShowActionSuccessMessage(action);
            }
        }

        protected void ccbtnCalc_Click(object sender, EventArgs e)
        {
            #region 計算金額(與銷編)
            {
                byte[] pdfContent = null;
                XmlResult xmlResult = DataProxy.Current.ExecB2100002Request(this.Page, "GENAMOUNT"
                    , this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId, this.EditStuId, this.EditOldSeq
                    , false, this.isEngUI(), out pdfContent);
                if (xmlResult.IsSuccess)
                {
                    SchoolRidEntity schoolRid = null;
                    ReceiveSumEntity[] receiveSums = null;
                    StudentMasterEntity studentMaster = null;
                    StudentReceiveEntity studentReceive = null;
                    StudentReceiveEntity lastStuReceive = null;
                    if (this.GetEditData(out schoolRid, out receiveSums, out studentMaster, out studentReceive, out lastStuReceive))
                    {
                        if (this.BindEditData(schoolRid, receiveSums, studentMaster, studentReceive, lastStuReceive))
                        {
                        }
                    }
                }
                else
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion
        }

        protected void ccbtnGenBill_Click(object sender, EventArgs e)
        {
            byte[] pdfContent = null;
            XmlResult xmlResult = DataProxy.Current.ExecB2100002Request(this.Page, "GENBILL"
                , this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId, this.EditStuId, this.EditOldSeq
                , false, false, out pdfContent);
            if (xmlResult.IsSuccess)
            {
                #region [MDY:20210401] 原碼修正
                string fileName = String.Format("{0}繳費單.PDF", HttpUtility.UrlEncode(EditStuId));
                #endregion
                this.ResponseFile(fileName, pdfContent);
            }
            else
            {
                this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
            }
        }

        protected void ccbtnGenEngBill_Click(object sender, EventArgs e)
        {
            byte[] pdfContent = null;
            XmlResult xmlResult = DataProxy.Current.ExecB2100002Request(this.Page, "GENBILL"
                , this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId, this.EditStuId, this.EditOldSeq
                , false, true, out pdfContent);
            if (xmlResult.IsSuccess)
            {
                #region [MDY:20210401] 原碼修正
                string fileName = String.Format("{0}_Bill.PDF", HttpUtility.UrlEncode(EditStuId));
                #endregion
                this.ResponseFile(fileName, pdfContent);
            }
            else
            {
                this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
            }
        }

        protected void ccbtnNewData_Click(object sender, EventArgs e)
        {
            //新增下一筆不需要 Session 參數，清除 Session 參數
            if (Session["QueryString"] != null)
            {
                Session.Remove("QueryString");
            }

            this.Response.Redirect("B2100001.aspx");
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            if (this.IsFixStuId)
            {
                return;
            }

            this.EditDepId = "";
            this.EditReceiveId = ucFilter2.SelectedReceiveID;

            //一定要用這個方法將商家代號、學年、學期、部別、代收費用別參數傳保留起來
            //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
            WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);

            SchoolRidEntity schoolRid = null;
            ReceiveSumEntity[] receiveSums = null;
            StudentMasterEntity studentMaster = null;
            StudentReceiveEntity studentReceive = null;
            StudentReceiveEntity lastStuReceive = null;
            if (this.GetEditData(out schoolRid, out receiveSums, out studentMaster, out studentReceive, out lastStuReceive))
            {
                if (this.BindEditData(schoolRid, receiveSums, studentMaster, studentReceive, lastStuReceive))
                {
                }
            }
        }

        protected void tbxStuId_TextChanged(object sender, EventArgs e)
        {
            string stuId = this.tbxStuId.Text.Trim();

            XmlResult xmlResult = null;
            StudentMasterEntity studentMaster = null;
            StudentReceiveEntity lastStudentReceive = null;
            if (!String.IsNullOrEmpty(stuId))
            {
                #region [MDY:2018xxxx] 整合成一個後台呼叫方法
                SchoolRidEntity schoolRid = null;
                ReceiveSumEntity[]  receiveSums = null;
                StudentReceiveEntity studentReceive = null;
                StudentLoanEntity studentLoan = null;
                string[] dataKinds = new string[2] { "StudentMaster", "LastStudentReceive" };
                XmlResult result = DataProxy.Current.GetStudentBillDatas(this.Page, this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, null, stuId, null, dataKinds,
                    out schoolRid, out studentMaster, out studentReceive, out receiveSums, out studentLoan, out lastStudentReceive);
                if (!result.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                    return;
                }
                #endregion
            }

            #region 學生基本資料區塊
            this.BindStudentBlockUI(studentMaster, stuId);
            #endregion

            #region [MDY:202203XX] 2022擴充案 是否使用英文資料介面
            bool useEngDataUI = this.UseEngDataUI(this.EditReceiveType, !this.IsPostBack);
            #endregion

            #region [MDY:202203XX] 2022擴充案 年級英文名稱相關
            WebHelper.SetDropDownListItems(this.ddlStuGrade, DefaultItem.Kind.Omit, false, new GradeCodeTexts(useEngDataUI), false, true, 0, null);
            #endregion

            #region 部別、院別、科系、年級、班別
            if (lastStudentReceive == null)
            {
                WebHelper.SetDropDownListSelectedValue(this.ddlDeptId, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlCollegeId, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlMajorId, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlStuGrade, String.Empty);
                WebHelper.SetDropDownListSelectedValue(this.ddlClassId, String.Empty);
            }
            else
            {
                WebHelper.SetDropDownListSelectedValue(this.ddlDeptId, lastStudentReceive.DeptId);
                WebHelper.SetDropDownListSelectedValue(this.ddlCollegeId, lastStudentReceive.CollegeId);
                WebHelper.SetDropDownListSelectedValue(this.ddlMajorId, lastStudentReceive.MajorId);
                WebHelper.SetDropDownListSelectedValue(this.ddlStuGrade, lastStudentReceive.StuGrade);
                WebHelper.SetDropDownListSelectedValue(this.ddlClassId, lastStudentReceive.ClassId);
            }
            #endregion
        }

        protected void ccbtnGoBack_Click(object sender, EventArgs e)
        {
            if (this.IsFixStuId)
            {
                //一定要用這個方法將商家代號、學年、學期、部別、代收費用別參數傳保留起來
                //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
                //ReceiveId 參數一定要是空字串，否則 B2100001 介面不會正確
                WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, String.Empty);

                KeyValueList<string> queryStrings = new KeyValueList<string>();
                queryStrings.Add("Action", ActionMode.Query);
                queryStrings.Add("StuId", this.EditStuId);
                queryStrings.Add("RedirectFrom", "B2100001");
                Session["QueryString"] = queryStrings;

                #region [MDY:20210521] 原碼修正
                this.Response.Redirect(WebHelper.GenRNUrl("B2100002.aspx"));
                #endregion
            }
            else
            {
                #region [MDY:20210521] 原碼修正
                this.Response.Redirect(WebHelper.GenRNUrl("B2100000.aspx"));
                #endregion
            }
        }
    }
}