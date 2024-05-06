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
    /// 維護學生繳費資料 (維護)
    /// </summary>
    public partial class B2100002M : BasePage
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
                return ViewState["EditReceiveType"] as string;
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
                return ViewState["EditYearId"] as string;
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
                return ViewState["EditTermId"] as string;
            }
            set
            {
                ViewState["EditTermId"] = value == null ? String.Empty : value.Trim();
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
                ViewState["EditDepId"] = value == null ? String.Empty : value.Trim();
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
        /// 編輯資料的銷帳狀態
        /// </summary>
        private string KeepCancelStatus
        {
            get
            {
                return ViewState["KeepCancelStatus"] as string;
            }
            set
            {
                ViewState["KeepCancelStatus"] = value;
            }
        }

        /// <summary>
        /// 編輯資料是否有上傳銷帳編號
        /// </summary>
        private bool KeepHasUploadCancelNo
        {
            get
            {
                object value = ViewState["KeepIsUploadCancelNo"];
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
                ViewState["KeepIsUploadCancelNo"] = value;
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

                #region [MDY:20190906] (2019擴充案) 新增減免金額
                this.tbxFeePayable.Text = String.Empty;
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
            this.tbxCancelNo.Text = this.labCancelNo.Text;
            this.tbxCancelNo.Visible = false;
            this.labReceiveATMAmount.Text = String.Empty;
            this.labCancelATMNo.Text = String.Empty;
            this.labReceiveSMAmount.Text = String.Empty;
            this.labCancelSMNo.Text = String.Empty;

            this.labCancelStatus.Text = String.Empty;
            this.labReceiveWay.Text = String.Empty;
            this.labReceiveBankId.Text = String.Empty;
            this.labReceiveDate.Text = String.Empty;
            this.labAccountDate.Text = String.Empty;
            #endregion

            #region [MDY:2018xxxx] 資料建立日期、最後修改日期 欄位
            this.labCreateDate.Text = String.Empty;
            this.labUpdateDate.Text = String.Empty;
            #endregion

            #region Button
            #region [MDY:202203XX] 2022擴充案 英文資料
            this.ccbtnOK.Visible = false;
            this.ccbtnCalc.Visible = false;
            this.ccbtnGenBill.Visible = false;
            this.ccbtnGenEngBill.Visible = false;
            this.ccbtnGenReceipt.Visible = false;
            this.ccbtnGenEngReceipt.Visible = false;
            #endregion
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
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetEditData(out SchoolRidEntity schoolRid, out StudentMasterEntity studentMaster, out StudentReceiveEntity studentReceive, out ReceiveSumEntity[] receiveSums, out StudentLoanEntity studentLoan)
        {
            schoolRid = null;
            studentMaster = null;
            studentReceive = null;
            receiveSums = null;
            studentLoan = null;

            string action = this.GetLocalized("查詢維護的相關資料");

            #region [MDY:2018xxxx] 整合成一個後台呼叫方法
            string[] dataKinds = new string[5] { "SchoolRid", "StudentMaster", "StudentReceive", "ReceiveSum", "StudentLoan" };
            StudentReceiveEntity lastStudentReceive = null;
            XmlResult result = DataProxy.Current.GetStudentBillDatas(this.Page, this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId, this.EditStuId, this.EditOldSeq, dataKinds,
                out schoolRid, out studentMaster, out studentReceive, out receiveSums, out studentLoan, out lastStudentReceive);
            if (!result.IsSuccess)
            {
                this.ShowActionFailureMessage(action, result.Code, result.Message);
                return false;
            }
            #endregion

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
        private bool BindEditData(SchoolRidEntity schoolRid, StudentMasterEntity studentMaster, StudentReceiveEntity studentReceive, ReceiveSumEntity[] receiveSums, StudentLoanEntity studentLoan)
        {
            bool isOK = (schoolRid != null && studentMaster != null && studentReceive != null);

            #region 學生基本資料區塊
            this.BindStudentBlockUI(studentMaster);
            #endregion

            #region [MDY:202203XX] 2022擴充案 是否使用英文資料介面
            bool useEngDataUI = this.UseEngDataUI(this.EditReceiveType, !this.IsPostBack);
            #endregion

            #region [MDY:202203XX] 2022擴充案 年級英文名稱相關
            WebHelper.SetDropDownListItems(this.ddlStuGrade, DefaultItem.Kind.Omit, false, new GradeCodeTexts(useEngDataUI), false, true, 0, studentReceive.StuGrade);
            #endregion

            //依據編輯模式判斷是否可編輯 (只會有修改與刪除模式，所以只有修改模式才能編輯)
            bool isEditable = this.Action == ActionMode.Modify;
            //依據編輯模式與是否繳費判斷是否可編輯 (會影響銷帳編號、應繳金額等值或銷帳處理的欄位，都須以此判斷是否可編輯)
            bool isBillEditable = isEditable 
                && studentReceive != null
                && String.IsNullOrWhiteSpace(studentReceive.ReceiveDate)
                && String.IsNullOrWhiteSpace(studentReceive.AccountDate)
                && String.IsNullOrWhiteSpace(studentReceive.ReceiveWay);

            #region 繳費資料區塊
            {
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

                    #region [MDY:20190906] (2019擴充案) 新增減免金額
                    this.tbxFeePayable.Text = DataFormat.GetAmountText(studentReceive.FeePayable);
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
                    WebHelper.SetDropDownListSelectedValue(this.ddlDeptId, String.Empty);
                    WebHelper.SetDropDownListSelectedValue(this.ddlCollegeId, String.Empty);
                    WebHelper.SetDropDownListSelectedValue(this.ddlMajorId, String.Empty);
                    WebHelper.SetDropDownListSelectedValue(this.ddlStuGrade, String.Empty);
                    WebHelper.SetDropDownListSelectedValue(this.ddlClassId, String.Empty);
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

                    #region [MDY:20190906] (2019擴充案) 新增減免金額
                    this.tbxFeePayable.Text = String.Empty;
                    #endregion
                }

                #region 指定 Enabled 屬性
                #region [MDY:20150201] 繳款期限
                this.tbxPayDueDate.Enabled = isBillEditable;
                #endregion

                #region 住宿、減免、就貸、身份註記 1 ~ 6
                this.ddlDormId.Enabled = isBillEditable;
                this.ddlReduceId.Enabled = isBillEditable;
                this.ddlLoanId.Enabled = isBillEditable;
                this.ddlIdentifyId01.Enabled = isBillEditable;
                this.ddlIdentifyId02.Enabled = isBillEditable;
                this.ddlIdentifyId03.Enabled = isBillEditable;
                this.ddlIdentifyId04.Enabled = isBillEditable;
                this.ddlIdentifyId05.Enabled = isBillEditable;
                this.ddlIdentifyId06.Enabled = isBillEditable;
                #endregion

                #region 補單註記、計算方式
                this.ddlReissueFlag.Enabled = isBillEditable;
                this.ddlBillingType.Enabled = isBillEditable;
                #endregion

                #region 上傳就學貸款金額、可貸金額、學分數、上課時數
                this.tbxLoan.Enabled = isBillEditable;
                this.tbxLoanAmount.Enabled = isBillEditable;
                this.tbxStuCredit.Enabled = isBillEditable;
                this.tbxStuHour.Enabled = isBillEditable;
                #endregion

                #region [MDY:20190906] (2019擴充案) 新增減免金額
                this.tbxFeePayable.Enabled = isBillEditable;
                #endregion

                #region 部別、院別、科系、年級、班別、座號改為繳款後仍可修改
                this.ddlDeptId.Enabled = isEditable;
                this.ddlCollegeId.Enabled = isEditable;
                this.ddlMajorId.Enabled = isEditable;
                this.ddlStuGrade.Enabled = isEditable;
                this.ddlClassId.Enabled = isEditable;
                this.tbxStuHid.Enabled = isEditable;
                #endregion
                #endregion
            }
            #endregion

            #region 收入明細區塊
            isOK &= this.BindReceiveItemBolckUI(schoolRid, studentReceive, useEngDataUI);
            #endregion

            #region 合計項目區塊
            #region [MDY:202203XX] 2022擴充案 合計項目中文/英文名稱
            isOK &= this.BindReceiveSumHtml(receiveSums, studentReceive, useEngDataUI);
            #endregion
            #endregion

            #region 學生就貸區塊
            this.BindStudentLoanHtml(studentLoan, schoolRid);
            #endregion

            #region 備註區塊
            isOK &= this.BindMemoBlockUI(schoolRid, studentReceive, isBillEditable);  //[MEMO] 備註跟銷帳應該沒有關係，但這裡從嚴
            #endregion

            #region 學生扣款轉帳資料區塊
            {
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

                #region 指定 Enabled 屬性
                //[MEMO] 這跟扣款有關，所以繳費後不可修改
                this.tbxDeductBankId.Enabled = isBillEditable;
                this.tbxDeductAccountNo.Enabled = isBillEditable;
                this.tbxDeductAccountName.Enabled = isBillEditable;
                this.tbxDeductAccountId.Enabled = isBillEditable;
                #endregion
            }
            #endregion

            #region 繳費/銷帳資料區塊
            string cancelStatus = null;
            bool hasUploadCancelNo = false;
            if (studentReceive != null)
            {
                this.KeepCancelStatus = cancelStatus = studentReceive.GetCancelStatus();
                this.KeepHasUploadCancelNo = hasUploadCancelNo = studentReceive.HasUploadCancelNo();

                this.labReceiveAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveAmount);
                this.labCancelNo.Text = studentReceive.CancelNo;
                this.tbxCancelNo.Text = this.labCancelNo.Text;
                this.tbxCancelNo.Visible = (hasUploadCancelNo && isBillEditable);
                this.labReceiveATMAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveAtmamount);
                this.labCancelATMNo.Text = studentReceive.CancelAtmno;
                this.labReceiveSMAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveSmamount);
                this.labCancelSMNo.Text = studentReceive.CancelSmno;

                this.labCancelStatus.Text = this.GetLocalized(CancelStatusCodeTexts.GetText(cancelStatus));
                this.labReceiveWay.Text = this.GetChannelName(studentReceive.ReceiveWay);
                this.labReceiveBankId.Text = studentReceive.ReceivebankId;

                DateTime? receiveDate = DataFormat.ConvertDateText(studentReceive.ReceiveDate);
                this.labReceiveDate.Text = receiveDate.HasValue ? DataFormat.GetDateText(receiveDate.Value) : studentReceive.ReceiveDate;
                DateTime? accountDate = DataFormat.ConvertDateText(studentReceive.AccountDate);
                this.labAccountDate.Text = accountDate.HasValue ? DataFormat.GetDateText(accountDate.Value) : studentReceive.AccountDate;
            }
            else
            {
                this.KeepCancelStatus = cancelStatus = String.Empty;
                this.KeepHasUploadCancelNo = hasUploadCancelNo = false;

                this.labReceiveAmount.Text = String.Empty;
                this.labCancelNo.Text = String.Empty;
                this.tbxCancelNo.Text = this.labCancelNo.Text;
                this.tbxCancelNo.Visible = false;
                this.labReceiveATMAmount.Text = String.Empty;
                this.labCancelATMNo.Text = String.Empty;
                this.labReceiveSMAmount.Text = String.Empty;
                this.labCancelSMNo.Text = String.Empty;

                this.labCancelStatus.Text = String.Empty;
                this.labReceiveWay.Text = String.Empty;
                this.labReceiveBankId.Text = String.Empty;

                this.labReceiveDate.Text = String.Empty;
                this.labAccountDate.Text = String.Empty;
            }
            #endregion

            #region [MDY:2018xxxx] 資料建立日期、最後修改日期 欄位
            if (studentReceive == null)
            {
                this.labCreateDate.Text = String.Empty;
                this.labUpdateDate.Text = String.Empty;
            }
            else
            {
                this.labCreateDate.Text = studentReceive.CreateDate.HasValue ? DataFormat.GetDateTimeText(studentReceive.CreateDate.Value) : String.Empty;
                this.labUpdateDate.Text = studentReceive.UpdateDate.HasValue ? DataFormat.GetDateTimeText(studentReceive.UpdateDate.Value) : String.Empty;
            }
            #endregion

            #region Button
            if (isOK)
            {
                #region 存檔
                this.ccbtnOK.Visible = true;
                #endregion

                #region [MDY:202203XX] 2022擴充案 英文資料
                bool isEngEabled = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);
                if (this.Action == ActionMode.Modify)
                {
                    #region 計算金額
                    this.ccbtnCalc.Visible = (cancelStatus == CancelStatusCodeTexts.NONPAY);
                    #endregion

                    #region 產生PDF繳費單
                    {
                        #region 邏輯 (土銀特別允許金額小於 0 的資料也可以列印)
                        //1. 未繳費
                        //2. 金額大於 0 且有虛擬帳號的資料 或 金額小於或等於 0 (不管是否有虛擬帳號)
                        #endregion

                        this.ccbtnGenBill.Visible = false;
                        this.ccbtnGenEngBill.Visible = false;
                        if (cancelStatus == CancelStatusCodeTexts.NONPAY && studentReceive.ReceiveAmount.HasValue)
                        {
                            decimal receiveAmount = studentReceive.ReceiveAmount.Value;
                            if ((receiveAmount > 0 && !String.IsNullOrWhiteSpace(studentReceive.CancelNo))
                                || receiveAmount <= 0)
                            {
                                this.ccbtnGenBill.Visible = true;
                                this.ccbtnGenEngBill.Visible = isEngEabled & true;
                            }
                        }
                    }
                    #endregion

                    #region 產生PDF繳費收據
                    switch (cancelStatus)
                    {
                        case CancelStatusCodeTexts.PAYED:
                            this.ccbtnGenReceipt.LocationText = "產生PDF繳費憑單";
                            this.ccbtnGenReceipt.Visible = true;
                            this.ccbtnGenEngReceipt.LocationText = "產生英文PDF繳費憑單";
                            this.ccbtnGenEngReceipt.Visible = isEngEabled & true;
                            break;
                        case CancelStatusCodeTexts.CANCELED:
                            this.ccbtnGenReceipt.LocationText = "產生PDF繳費收據";
                            this.ccbtnGenReceipt.Visible = true;
                            this.ccbtnGenEngReceipt.LocationText = "產生英文PDF繳費收據";
                            this.ccbtnGenEngReceipt.Visible = isEngEabled & true;
                            break;
                        default:
                            this.ccbtnGenReceipt.Visible = false;
                            this.ccbtnGenEngReceipt.Visible = false;
                            break;
                    }
                    #endregion
                }
                else
                {
                    this.ccbtnCalc.Visible = false;
                    this.ccbtnGenBill.Visible = false;
                    this.ccbtnGenEngBill.Visible = false;
                    this.ccbtnGenReceipt.Visible = false;
                    this.ccbtnGenEngReceipt.Visible = false;
                }
                #endregion
            }
            else
            {
                #region [MDY:202203XX] 2022擴充案 英文資料
                this.ccbtnOK.Visible = false;
                this.ccbtnCalc.Visible = false;
                this.ccbtnGenBill.Visible = false;
                this.ccbtnGenEngBill.Visible = false;
                this.ccbtnGenReceipt.Visible = false;
                this.ccbtnGenEngReceipt.Visible = false;
                #endregion
            }
            #endregion

            return isOK;
        }

        #region 學生基本資料相關
        /// <summary>
        /// 結繫學生基本資料區塊介面
        /// </summary>
        /// <param name="studentMaster">指定學生基本資料</param>
        private void BindStudentBlockUI(StudentMasterEntity studentMaster)
        {
            if (studentMaster == null)
            {
                this.tbxStuId.Text = String.Empty;
                this.tbxName.Text = String.Empty;
                this.tbxIdNumber.Text = String.Empty;
                this.tbxTel.Text = String.Empty;
                this.tbxBirthday.Text = String.Empty;
                this.tbxZipCode.Text = String.Empty;
                this.tbxAddress.Text = String.Empty;
                this.tbxEmail.Text = String.Empty;
                this.tbxStuParent.Text = String.Empty;
            }
            else
            {
                this.tbxStuId.Text = studentMaster.Id;
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
            }

            bool isEditable = this.Action != ActionMode.Delete;

            #region 指定 Enabled 屬性
            this.tbxStuId.Enabled = false;
            this.tbxName.Enabled = isEditable;
            this.tbxIdNumber.Enabled = isEditable;
            this.tbxTel.Enabled = isEditable;
            this.tbxBirthday.Enabled = isEditable;
            this.tbxZipCode.Enabled = isEditable;
            this.tbxAddress.Enabled = isEditable;
            this.tbxEmail.Enabled = isEditable;
            this.tbxStuParent.Enabled = isEditable;
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
                //依據編輯模式與是否繳費判斷是否可編輯 (會影響銷帳編號、應繳金額等值或銷帳處理的欄位，都須以此判斷是否可編輯)
                bool isBillEditable = this.Action == ActionMode.Modify
                    && String.IsNullOrWhiteSpace(studentReceive.ReceiveDate)
                    && String.IsNullOrWhiteSpace(studentReceive.AccountDate)
                    && String.IsNullOrWhiteSpace(studentReceive.ReceiveWay);

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
                    tbxItemValue.Enabled = isBillEditable;
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
                    .AppendFormat("<tr><td colspan=\"2\" align=\"center\">{0}</td></tr>", this.GetLocalized("合計項目")).AppendLine();
                foreach (ReceiveSumEntity receiveSum in receiveSums)
                {
                    SubTotalAmount subTotal = SubTotalAmount.Create(studentReceive, receiveSum, true, useEngDataUI, out errmsg);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        html
                            .AppendLine("<tr>")
                            .AppendFormat("<td width=\"60%\">{0}</td>", Server.HtmlEncode(subTotal.Name)).AppendLine()
                            .AppendFormat("<td>{0}</td>", subTotal == null ? Server.HtmlEncode(errmsg) : DataFormat.GetAmountCommaText(subTotal.Amount)).AppendLine()
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

        #region 學生就貸相關
        /// <summary>
        /// 結繫學生就貸 Html
        /// </summary>
        /// <param name="studentLoan">指定學生就貸</param>
        /// <param name="schoolRid">指定代收費用檔</param>
        /// <returns>失敗傳回 false，否則傳回 true</returns>
        private void BindStudentLoanHtml(StudentLoanEntity studentLoan, SchoolRidEntity schoolRid)
        {
            StringBuilder html = new StringBuilder();
            if (studentLoan != null && schoolRid != null)
            {
                html.AppendLine("<table width=\"100%\">")
                    .AppendFormat("<tr><td colspan=\"2\" align=\"center\">{0}</td></tr>", this.GetLocalized("學生就貸")).AppendLine();

                #region [MDY:202203XX] 2022擴充案 收入科目 改寫，改用 GetAllReceiveItems()
                #region [OLD]
                //string[] receiveItems = schoolRid.GetAllReceiveItems();
                #endregion

                bool useEngDataUI = this.UseEngDataUI(this.EditReceiveType, !this.IsPostBack);
                string[] receiveItems = schoolRid.GetAllReceiveItems(useEngDataUI);
                #endregion

                decimal?[] loans = new decimal?[40] {
                    studentLoan.Loan01, studentLoan.Loan02, studentLoan.Loan03, studentLoan.Loan04, studentLoan.Loan05,
                    studentLoan.Loan06, studentLoan.Loan07, studentLoan.Loan08, studentLoan.Loan09, studentLoan.Loan10,
                    studentLoan.Loan11, studentLoan.Loan12, studentLoan.Loan13, studentLoan.Loan14, studentLoan.Loan15,
                    studentLoan.Loan16, studentLoan.Loan17, studentLoan.Loan18, studentLoan.Loan19, studentLoan.Loan20,
                    studentLoan.Loan21, studentLoan.Loan22, studentLoan.Loan23, studentLoan.Loan24, studentLoan.Loan25,
                    studentLoan.Loan26, studentLoan.Loan27, studentLoan.Loan28, studentLoan.Loan29, studentLoan.Loan30,
                    studentLoan.Loan31, studentLoan.Loan32, studentLoan.Loan33, studentLoan.Loan34, studentLoan.Loan35,
                    studentLoan.Loan36, studentLoan.Loan37, studentLoan.Loan38, studentLoan.Loan39, studentLoan.Loan40,
                };
                for(int idx = 0; idx < receiveItems.Length; idx++)
                {
                    string receiveItem = receiveItems[idx];
                    decimal? loan = loans[idx];
                    if (!String.IsNullOrEmpty(receiveItem))
                    {
                        html
                            .AppendLine("<tr>")
                            .AppendFormat("<td width=\"60%\">{0}</td>", Server.HtmlEncode(receiveItem)).AppendLine()
                            .AppendFormat("<td>{0}</td>", DataFormat.GetAmountCommaText(loan)).AppendLine()
                            .AppendLine("</tr>");
                    }
                    //else if (loan.HasValue && loan.Value > 0)
                    //{
                    //    html
                    //        .AppendLine("<tr>")
                    //        .AppendFormat("<td width=\"60%\">收入科目{0:00}</td>", idx + 1).AppendLine()
                    //        .AppendFormat("<td>{0}</td>", DataFormat.GetAmountCommaText(loan)).AppendLine()
                    //        .AppendLine("</tr>");
                    //}
                }
                html
                    .AppendLine("<tr>")
                    .AppendFormat("<td width=\"60%\">{0}</td>", this.GetLocalized("計算後的可貸金額")).AppendLine()
                    .AppendFormat("<td>{0}</td>", DataFormat.GetAmountCommaText(studentLoan.LoanAmount)).AppendLine()
                    .AppendLine("</tr>");
                html
                    .AppendLine("<tr>")
                    .AppendFormat("<td width=\"60%\">{0}</td>", this.GetLocalized("上傳的可貸金額")).AppendLine()
                    .AppendFormat("<td>{0}</td>", DataFormat.GetAmountCommaText(studentLoan.LoanFixamount)).AppendLine()
                    .AppendLine("</tr>");
                html.AppendLine("</table>");
            }
            this.litLoanListHtml.Text = html.ToString();
        }
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
        /// <param name="isEnabled">指定控制項的 Enabled 屬性值</param>
        /// <returns>失敗傳回 false，否則傳回 true</returns>
        private bool BindMemoBlockUI(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive, bool isEnabled)
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
                    tbxMemoValue.Enabled = isEnabled;

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

            #region 商家代號、學年、學期、代收費用別、學號或資料序號
            if (String.IsNullOrEmpty(this.EditReceiveType) || String.IsNullOrEmpty(this.EditYearId) || String.IsNullOrEmpty(this.EditTermId)
                || String.IsNullOrEmpty(this.EditReceiveId) || String.IsNullOrEmpty(this.EditStuId) || this.EditOldSeq < 0)
            {
                string msg = this.GetLocalized("無法取得商家代號、學年、學期、代收費用別、學號或資料序號參數");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            #region StudentMaster
            {
                StudentMasterEntity data = new StudentMasterEntity();

                data.ReceiveType = this.EditReceiveType;
                data.DepId = this.EditDepId;
                data.Id = this.EditStuId;

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

                data.MdyDate = DateTime.Now;

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
                data.StuId = this.EditStuId;
                data.OldSeq = this.EditOldSeq;

                #region [MDY:20150201] 繳款期限
                data.PayDueDate = this.tbxPayDueDate.Text.Trim();
                if (!String.IsNullOrEmpty(data.PayDueDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(data.PayDueDate, out date) && date.Year >= 1911)
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

                #region [MDY:20190906] (2019擴充案) 增加減免金額
                data.FeePayable = null;
                {
                    string feePayable = this.tbxFeePayable.Text.Trim();
                    if (!String.IsNullOrEmpty(feePayable))
                    {
                        decimal amount;
                        if (Decimal.TryParse(feePayable, out amount) && amount >= 0)
                        {
                            data.FeePayable = amount;
                        }
                        else
                        {
                            string msg = this.GetLocalized("「減免金額」不是合法的金額");
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
                bool isSkipItem = false;  //收入科目是否不連續
                {
                    int lastSkipItemNo = 0;
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
                            if (!isSkipItem)
                            {
                                if (lastSkipItemNo != 0 && lastSkipItemNo + 1 == no)
                                {
                                    lastSkipItemNo = no;
                                }
                                else
                                {
                                    isSkipItem = true;
                                }
                            }

                            data.SetReceiveItemAmount(no, null);
                        }
                    }
                }
                if (isSkipItem && data.BillingType != BillingTypeCodeTexts.BY_AMOUNT)
                {
                    string msg = this.GetLocalized("收入科目不連續時，計算方式必須為「依金額計算」");
                    this.ShowSystemMessage(msg);
                    return false;
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

                #region 有上傳虛擬帳號
                if (this.KeepHasUploadCancelNo && this.tbxCancelNo.Visible)
                {
                    data.CancelNo = this.tbxCancelNo.Text.Trim();

                    #region [MDY:20181202] 修正 BUG (M201812_01)
                    if (String.IsNullOrEmpty(data.CancelNo))
                    {
                        this.ShowMustInputAlert("虛擬帳號");
                        return false;
                    }
                    else if (data.BillingType != BillingTypeCodeTexts.BY_AMOUNT)
                    {
                        string msg = this.GetLocalized("指定虛擬帳號時，計算方式必須為「依金額計算」");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                    #endregion
                }
                #endregion

                #region 不提供維護的欄位的預設值
                data.Exportreceivedata = string.Empty;
                data.MappingType = string.Empty;
                data.MappingId = string.Empty;
                data.Remark = string.Empty;

                data.UpNo = "0";    //單筆新增 批號 固定設為 0
                data.UpOrder = "";  //單筆新增 上傳該批資料的序號 固定設為空字串

                data.UpdateDate = DateTime.Now;
                #endregion

                studentReceive = data;
            }
            #endregion

            return true;
        }
        #endregion

        /// <summary>
        /// 取得管道名稱
        /// </summary>
        /// <param name="receiveWay">ChannelId</param>
        /// <returns></returns>
        private string GetChannelName(string receiveWay)
        {
            ChannelSetEntity entity = null;

            Expression where = new Expression();
            where.And(ChannelSetEntity.Field.ChannelId, receiveWay);

            XmlResult result = DataProxy.Current.SelectFirst<ChannelSetEntity>(this, where, null, out entity);
            if (!result.IsSuccess || entity == null)
            {
                return string.Empty;
            }
            else
            {
                return entity.ChannelName;
            }
        }

        private bool SaveEditData(bool toGenCancelNo)
        {
            string backUrl = "B2100002.aspx";
            switch (this.Action)
            {
                case ActionMode.Delete:     //刪除
                    #region 刪除
                    {
                        string action = ActionMode.GetActionLocalized(this.Action);

                        #region StudentReceive
                        {
                            StudentReceiveEntity data = new StudentReceiveEntity();
                            data.ReceiveType = this.EditReceiveType;
                            data.YearId = this.EditYearId;
                            data.TermId = this.EditTermId;
                            data.DepId = this.EditDepId;
                            data.ReceiveId = this.EditReceiveId;
                            data.StuId = this.EditStuId;
                            data.OldSeq = this.EditOldSeq;

                            int count = 0;
                            XmlResult xmlResult = DataProxy.Current.Delete<StudentReceiveEntity>(this, data, out count);
                            if (xmlResult.IsSuccess)
                            {
                                if (count < 1)
                                {
                                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                    return false;
                                }
                                else
                                {
                                    //WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);
                                    this.ShowActionSuccessAlert(action, backUrl);
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
                        StudentMasterEntity studentMaster = null;
                        StudentReceiveEntity studentReceive = null;
                        if (!this.CheckAndGetInputData(out studentMaster, out studentReceive))
                        {
                            return false;
                        }

                        string action = ActionMode.GetActionLocalized(this.Action);

                        #region [MDY:2018xxxx] 整合成一個後台呼叫方法
                        KeyValueList returnDatas = null;
                        XmlResult xmlResult = DataProxy.Current.UpdateStudentBillDatas(this.Page, studentMaster, studentReceive, out returnDatas, toGenCancelNo, true);
                        if (xmlResult.IsSuccess)
                        {
                            #region 取資料並結繫
                            if (returnDatas == null || returnDatas.Count == 0)
                            {
                                string msg = this.GetLocalized("修改資料成功，但回傳資料不正確");
                                this.ShowSystemMessage(msg);
                            }
                            else
                            {
                                SchoolRidEntity schoolRid = returnDatas.TryGetValue("SchoolRid", null) as SchoolRidEntity;
                                StudentMasterEntity newStudentMaster = returnDatas.TryGetValue("StudentMaster", null) as StudentMasterEntity;
                                StudentReceiveEntity newStudentReceive = returnDatas.TryGetValue("StudentReceive", null) as StudentReceiveEntity;
                                ReceiveSumEntity[] receiveSums = returnDatas.TryGetValue("ReceiveSum", null) as ReceiveSumEntity[];
                                StudentLoanEntity studentLoan = returnDatas.TryGetValue("StudentLoan", null) as StudentLoanEntity;

                                if (schoolRid == null)
                                {
                                    string msg = this.GetLocalized("修改資料成功，但回傳資料缺少代收費用檔資料");
                                    this.ShowSystemMessage(msg);
                                    return false;
                                }
                                if (newStudentMaster == null)
                                {
                                    string msg = this.GetLocalized("修改資料成功，但回傳資料缺少學生基本資料");
                                    this.ShowSystemMessage(msg);
                                    return false;
                                }
                                if (newStudentReceive == null)
                                {
                                    string msg = this.GetLocalized("修改資料成功，但回傳資料缺少學生繳費單資料");
                                    this.ShowSystemMessage(msg);
                                    return false;
                                }
                                if (this.BindEditData(schoolRid, newStudentMaster, newStudentReceive, receiveSums, studentLoan))
                                {
                                }
                                this.ShowActionSuccessMessage(action);
                            }
                            #endregion

                            return true;
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                            return false;
                        }
                        #endregion
                    }
                    #endregion
                default:
                    return false;
            }
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
                KeyValueList<string> queryStrings = Session["QueryString"] as KeyValueList<string>;
                if (queryStrings == null || queryStrings.Count == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    return;
                }

                this.Action = queryStrings.TryGetValue("Action", String.Empty);
                this.EditReceiveType = queryStrings.TryGetValue("ReceiveType", String.Empty);
                this.EditYearId = queryStrings.TryGetValue("YearId", String.Empty);
                this.EditTermId = queryStrings.TryGetValue("TermId", String.Empty);
                this.EditDepId = queryStrings.TryGetValue("DepId", String.Empty);
                this.EditReceiveId = queryStrings.TryGetValue("ReceiveId", String.Empty);
                this.EditStuId = queryStrings.TryGetValue("StuId", String.Empty);
                string oldSeq = queryStrings.TryGetValue("OldSeq", String.Empty);
                int editOldSeq = 0;

                //土銀不使用原有的部別 DepListEntity 改用專用的 DeptListEntity，所以 depId 預設值為空字串
                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                    || this.EditDepId == null
                    || String.IsNullOrEmpty(this.EditReceiveId)
                    || String.IsNullOrEmpty(this.EditStuId)
                    || !Int32.TryParse(oldSeq, out editOldSeq) || editOldSeq < 0
                    || (this.Action != ActionMode.Modify && this.Action != ActionMode.Delete))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }

                this.EditOldSeq = editOldSeq;

                XmlResult xmlResult = ucFilter1.GetDataAndBind(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);
                if (xmlResult.IsSuccess)
                {
                    if (this.EditReceiveId != ucFilter2.SelectedReceiveID)
                    {
                        string msg = this.GetLocalized("網頁參數不正確");
                        this.ShowSystemMessage(msg);
                        return;
                    }
                }
                else
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

                #region 部別、院別、科系、班別、住宿、減免、就貸、身份註記 1 ~ 6
                this.GetAndBindOptions();
                #endregion

                #region 取資料並結繫
                SchoolRidEntity schoolRid = null;
                StudentMasterEntity studentMaster = null;
                StudentReceiveEntity studentReceive = null;
                ReceiveSumEntity[] receiveSums = null;
                StudentLoanEntity studentLoan = null;
                if (this.GetEditData(out schoolRid, out studentMaster, out studentReceive, out receiveSums, out studentLoan))
                {
                    if (this.BindEditData(schoolRid, studentMaster, studentReceive, receiveSums, studentLoan))
                    {
                    }
                }
                #endregion
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            this.SaveEditData(false);
        }

        protected void ccbtnCalc_Click(object sender, EventArgs e)
        {
            bool toGenCancelNo = (!this.KeepHasUploadCancelNo && !this.tbxCancelNo.Visible);  //上傳虛擬帳號必須計算金額與處理虛擬帳號，不能再產生虛擬帳號
            this.SaveEditData(toGenCancelNo);
        }

        #region [MDY:202203XX] 2022擴充案 英文資料
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

        protected void ccbtnGenReceipt_Click(object sender, EventArgs e)
        {
            byte[] pdfContent = null;
            XmlResult xmlResult = DataProxy.Current.ExecB2100002Request(this.Page, "GENRECEIPT"
                , this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId, this.EditStuId, this.EditOldSeq
                , false, false, out pdfContent);
            if (xmlResult.IsSuccess)
            {
                string pdfName = (this.KeepCancelStatus == CancelStatusCodeTexts.CANCELED) ? "收據" : "憑單";
                #region [MDY:20210401] 原碼修正
                string fileName = String.Format("{0}繳費{1}.PDF", HttpUtility.UrlEncode(EditStuId), pdfName);
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

        protected void ccbtnGenEngReceipt_Click(object sender, EventArgs e)
        {
            byte[] pdfContent = null;
            XmlResult xmlResult = DataProxy.Current.ExecB2100002Request(this.Page, "GENRECEIPT"
                , this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId, this.EditStuId, this.EditOldSeq
                , false, true, out pdfContent);
            if (xmlResult.IsSuccess)
            {
                string pdfName = (this.KeepCancelStatus == CancelStatusCodeTexts.CANCELED) ? "Receipt" : "Proofs";
                #region [MDY:20210401] 原碼修正
                string fileName = String.Format("{0}_{1}.PDF", HttpUtility.UrlEncode(EditStuId), pdfName);
                #endregion
                this.ResponseFile(fileName, pdfContent);
            }
            else
            {
                this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
            }
        }
        #endregion
    }
}