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

namespace eSchoolWeb.C
{
    /// <summary>
    /// 查詢繳費資料(明細)
    /// </summary>
    public partial class C3700001D : BasePage
    {
        /// <summary>
        /// 備註數量
        /// </summary>
        private const int MemoCount = StudentReceiveEntity.MemoCount;

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
        /// 編輯的業務別碼參數
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
        /// 編輯的序號參數
        /// </summary>
        private string EditOldSeq
        {
            get
            {
                return ViewState["EditOldSeq"] as string;
            }
            set
            {
                ViewState["EditOldSeq"] = value == null ? null : value.Trim();
            }
        }

        ///// <summary>
        ///// 編輯的銷帳編號
        ///// </summary>
        //private string EditCancelNo
        //{
        //    get
        //    {
        //        return ViewState["EditCancelNo"] as string;
        //    }
        //    set
        //    {
        //        ViewState["EditCancelNo"] = value == null ? null : value.Trim();
        //    }
        //}

        //private bool EditIsCanceled
        //{
        //    get
        //    {
        //        object value = ViewState["EditIsCanceled"];
        //        if (value is bool)
        //        {
        //            return (bool)value;
        //        }
        //        return false;
        //    }
        //    set
        //    {
        //        ViewState["EditIsCanceled"] = value;
        //    }
        //}
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            //一定要用這個方法將業務別碼、學年、學期、部別、代收費用別參數傳給下一頁
            //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
            //WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);

            #region 學生基本資料
            this.labStuId.Text = String.Empty;
            this.labName.Text = String.Empty;
            this.labIdNumber.Text = String.Empty;
            this.labTel.Text = String.Empty;
            this.labBirthday.Text = String.Empty;
            this.labZipCode.Text = String.Empty;
            this.labAddress.Text = String.Empty;
            this.labEmail.Text = String.Empty;
            this.labStuParent.Text = String.Empty;
            #endregion

            #region 繳費資料
            this.labUpNo.Text = String.Empty;
            this.labDeptName.Text = String.Empty;
            this.labCollegeName.Text = String.Empty;
            this.labMajorName.Text = String.Empty;
            this.labStuGrade.Text = String.Empty;
            this.labClassName.Text = String.Empty;
            this.labStuHid.Text = String.Empty;
            this.labDormName.Text = String.Empty;
            this.labReduceName.Text = String.Empty;          //減免
            this.labLoan.Text = String.Empty;                //上傳就學貸款金額
            this.labLoanName.Text = String.Empty;            //就貸
            this.labRealLoan.Text = String.Empty;            //原就學貸款金額
            this.labIdentifyId01Name.Text = String.Empty;    //身份註記一
            this.labIdentifyId02Name.Text = String.Empty;    //身份註記二 
            this.labIdentifyId03Name.Text = String.Empty;    //身份註記三
            this.labIdentifyId04Name.Text = String.Empty;    //身份註記四
            this.labIdentifyId05Name.Text = String.Empty;    //身份註記五
            this.labIdentifyId06Name.Text = String.Empty;    //身份註記六
            this.labStuCredit.Text = String.Empty;
            this.labStuHour.Text = String.Empty;
            this.labReissueFlag.Text = String.Empty;
            #endregion

            #region 收入明細
            this.litReceiveItemHtml.Text = String.Empty;
            #endregion

            #region 備註
            this.BindMemoBlockUI(null, null);

            #region [Old]
            //this.labMemo01.Text = String.Empty;
            //this.labMemo02.Text = String.Empty;
            //this.labMemo03.Text = String.Empty;
            //this.labMemo04.Text = String.Empty;
            //this.labMemo05.Text = String.Empty;
            //this.labMemo06.Text = String.Empty;
            //this.labMemo07.Text = String.Empty;
            //this.labMemo08.Text = String.Empty;
            //this.labMemo09.Text = String.Empty;
            //this.labMemo10.Text = String.Empty;
            #endregion
            #endregion

            #region 繳費/銷帳資料
            this.labReceiveAmount.Text = String.Empty;
            this.labCancelNo.Text = String.Empty;
            this.labReceiveSMAmount.Text = String.Empty;
            this.labCancelSMNo.Text = String.Empty;
            this.labReceiveATMAmount.Text = String.Empty;
            this.labCancelATMNo.Text = String.Empty;
            this.labCancelStatus.Text = String.Empty;
            this.labReceiveWay.Text = String.Empty;
            this.labReceiveBankId.Text = String.Empty;
            this.labReceiveDate.Text = String.Empty;
            this.labAccountDate.Text = String.Empty;
            #endregion
        }

        /// <summary>
        /// 取得查詢的資料
        /// </summary>
        /// <param name="schoolRid">成功則傳回代收費用設定資料</param>
        /// <param name="studentReceive">成功則傳回學生繳費資料</param>
        /// <param name="student">成功則傳回要編輯的學生基本資料</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetQueryData(out SchoolRidEntity schoolRid, out StudentReceiveView studentReceive, out StudentMasterEntity student)
        {
            schoolRid = null;
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

            #region StudentReceiveView
            {
                Expression where = new Expression(StudentReceiveView.Field.ReceiveType, this.EditReceiveType)
                    .And(StudentReceiveView.Field.YearId, this.EditYearId)
                    .And(StudentReceiveView.Field.TermId, this.EditTermId)
                    .And(StudentReceiveView.Field.DepId, this.EditDepId)
                    .And(StudentReceiveView.Field.ReceiveId, this.EditReceiveId)
                    .And(StudentReceiveView.Field.StuId, this.EditStuId)
                    .And(StudentReceiveView.Field.OldSeq, this.EditOldSeq);
                XmlResult result = DataProxy.Current.SelectFirst<StudentReceiveView>(this, where, null, out studentReceive);
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
                if (studentReceive == null)
                {
                    this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "無該學生基本資料");
                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 結繫查詢的資料
        /// </summary>
        /// <param name="schoolRid">代收費用設定資料</param>
        /// <param name="studentReceive">學生繳費資料</param>
        /// <param name="student">學生基本資料</param>
        private void BindQueryData(SchoolRidEntity schoolRid, StudentReceiveView studentReceive, StudentMasterEntity student)
        {
            if (schoolRid == null || studentReceive == null || student == null)
            {
                #region 學生基本資料
                this.labStuId.Text = String.Empty;
                this.labName.Text = String.Empty;
                this.labIdNumber.Text = String.Empty;
                this.labTel.Text = String.Empty;
                this.labBirthday.Text = String.Empty;
                this.labZipCode.Text = String.Empty;
                this.labAddress.Text = String.Empty;
                this.labEmail.Text = String.Empty;
                this.labStuParent.Text = String.Empty;
                #endregion

                #region 繳費資料
                this.labUpNo.Text = String.Empty;
                this.labDeptName.Text = String.Empty;
                this.labCollegeName.Text = String.Empty;
                this.labMajorName.Text = String.Empty;
                this.labStuGrade.Text = String.Empty;
                this.labClassName.Text = String.Empty;
                this.labStuHid.Text = String.Empty;
                this.labDormName.Text = String.Empty;
                this.labReduceName.Text = String.Empty;          //減免
                this.labLoan.Text = String.Empty;                //上傳就學貸款金額
                this.labLoanName.Text = String.Empty;            //就貸
                this.labRealLoan.Text = String.Empty;            //原就學貸款金額
                this.labIdentifyId01Name.Text = String.Empty;    //身份註記一
                this.labIdentifyId02Name.Text = String.Empty;    //身份註記二 
                this.labIdentifyId03Name.Text = String.Empty;    //身份註記三
                this.labIdentifyId04Name.Text = String.Empty;    //身份註記四
                this.labIdentifyId05Name.Text = String.Empty;    //身份註記五
                this.labIdentifyId06Name.Text = String.Empty;    //身份註記六
                this.labStuCredit.Text = String.Empty;
                this.labStuHour.Text = String.Empty;
                this.labReissueFlag.Text = String.Empty;
                #endregion

                #region 收入明細
                this.litReceiveItemHtml.Text = String.Empty;
                #endregion

                #region 備註
                this.BindMemoBlockUI(null, null);
                #endregion

                #region 繳費/銷帳資料
                this.labReceiveAmount.Text = String.Empty;
                this.labCancelNo.Text = String.Empty;
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

                return;
            }

            #region 學生基本資料
            this.labStuId.Text = student.Id;
            this.labName.Text = student.Name;
            this.labIdNumber.Text = student.IdNumber;
            this.labTel.Text = student.Tel;
            DateTime? birthday = DataFormat.ConvertDateText(student.Birthday);
            this.labBirthday.Text = birthday == null ? student.Birthday : DataFormat.GetDateText(birthday.Value);
            this.labZipCode.Text = student.ZipCode;
            this.labAddress.Text = student.Address;
            this.labEmail.Text = student.Email;
            this.labStuParent.Text = student.StuParent;
            #endregion

            #region 繳費資料
            this.labUpNo.Text = String.Format("{0}、{1}", studentReceive.UpNo, studentReceive.OldSeq);
            this.labDeptName.Text = studentReceive.DeptName;
            this.labCollegeName.Text = studentReceive.CollegeName;
            this.labMajorName.Text = studentReceive.MajorName;
            this.labStuGrade.Text = studentReceive.StuGradeName;
            this.labClassName.Text = studentReceive.ClassName;
            this.labStuHid.Text = studentReceive.StuHid;
            this.labDormName.Text = studentReceive.DormName;
            this.labReduceName.Text = studentReceive.ReduceName;                //減免
            this.labLoan.Text = DataFormat.GetAmountText(studentReceive.Loan);  //上傳就學貸款金額
            this.labLoanName.Text = studentReceive.LoanName;                    //就貸
            this.labRealLoan.Text = DataFormat.GetAmountText(studentReceive.RealLoan);  //原就學貸款金額
            this.labIdentifyId01Name.Text = studentReceive.IdentifyName01;      //身份註記一
            this.labIdentifyId02Name.Text = studentReceive.IdentifyName01;      //身份註記二 
            this.labIdentifyId03Name.Text = studentReceive.IdentifyName01;      //身份註記三
            this.labIdentifyId04Name.Text = studentReceive.IdentifyName01;      //身份註記四
            this.labIdentifyId05Name.Text = studentReceive.IdentifyName01;      //身份註記五
            this.labIdentifyId06Name.Text = studentReceive.IdentifyName01;      //身份註記六
            this.labStuCredit.Text = DataFormat.GetIntegerText(studentReceive.StuCredit);
            this.labStuHour.Text = DataFormat.GetIntegerText(studentReceive.StuHour);
            this.labReissueFlag.Text = ReissueFlagCodeTexts.GetText(studentReceive.ReissueFlag);
            #endregion

            #region 收入明細
            this.litReceiveItemHtml.Text = this.GenReceiveItemHtml(schoolRid, studentReceive);
            #endregion

            #region 備註
            this.BindMemoBlockUI(schoolRid, studentReceive);

            #region [Old]
            //this.labMemo01.Text = studentReceive.Memo01;
            //this.labMemo02.Text = studentReceive.Memo02;
            //this.labMemo03.Text = studentReceive.Memo03;
            //this.labMemo04.Text = studentReceive.Memo04;
            //this.labMemo05.Text = studentReceive.Memo05;
            //this.labMemo06.Text = studentReceive.Memo06;
            //this.labMemo07.Text = studentReceive.Memo07;
            //this.labMemo08.Text = studentReceive.Memo08;
            //this.labMemo09.Text = studentReceive.Memo09;
            //this.labMemo10.Text = studentReceive.Memo10;
            #endregion
            #endregion

            #region 繳費/銷帳資料
            string cancelStatus = studentReceive.GetCancelStatus();
            this.labReceiveAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveAmount);
            this.labCancelNo.Text = studentReceive.CancelNo;
            this.labReceiveATMAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveAtmamount);
            this.labCancelATMNo.Text = studentReceive.CancelAtmno;
            this.labReceiveSMAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveSmamount);
            this.labCancelSMNo.Text = studentReceive.CancelSmno;

            this.labCancelStatus.Text = CancelStatusCodeTexts.GetText(cancelStatus);
            this.labReceiveWay.Text = this.GetChannelName(studentReceive.ReceiveWay);
            this.labReceiveBankId.Text = studentReceive.ReceivebankId;

            DateTime? receiveDate = DataFormat.ConvertDateText(studentReceive.ReceiveDate);
            this.labReceiveDate.Text = receiveDate == null ? studentReceive.ReceiveDate : DataFormat.GetDateText(receiveDate.Value);
            DateTime? accountDate = DataFormat.ConvertDateText(studentReceive.AccountDate);
            this.labAccountDate.Text = accountDate == null ? studentReceive.AccountDate : DataFormat.GetDateText(accountDate.Value);
            #endregion
        }

        /// <summary>
        /// 結繫備註區塊介面
        /// </summary>
        /// <param name="schoolRid"></param>
        /// <param name="studentReceive"></param>
        /// <param name="enabled"></param>
        private void BindMemoBlockUI(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive)
        {
            HtmlTableRow[] trMemoRows = new System.Web.UI.HtmlControls.HtmlTableRow[] {
                this.trMemoRow00,
                this.trMemoRow01, this.trMemoRow02, this.trMemoRow03, this.trMemoRow04, this.trMemoRow05,
                this.trMemoRow06, this.trMemoRow07, this.trMemoRow08, this.trMemoRow09, this.trMemoRow10,
                this.trMemoRow11
            };

            #region [MDY:202203XX] 2022擴充案 備註項目 改寫，改用 GetAllMemoTitleChts()
            #region [OLD]
            //string[] memoTitles = schoolRid == null ? null : schoolRid.GetAllMemoTitles();
            #endregion

            string[] memoTitles = schoolRid?.GetAllMemoTitleChts();
            #endregion

            if (memoTitles == null || memoTitles.Length == 0)
            {
                foreach (HtmlTableRow trMemoRow in trMemoRows)
                {
                    trMemoRow.Visible = false;
                }
            }
            else
            {
                Label[] labMemoTitles = new Label[MemoCount] {
                    this.labMemoTitle01, this.labMemoTitle02, this.labMemoTitle03, this.labMemoTitle04, this.labMemoTitle05,
                    this.labMemoTitle06, this.labMemoTitle07, this.labMemoTitle08, this.labMemoTitle09, this.labMemoTitle10,
                    this.labMemoTitle11, this.labMemoTitle12, this.labMemoTitle13, this.labMemoTitle14, this.labMemoTitle15,
                    this.labMemoTitle16, this.labMemoTitle17, this.labMemoTitle18, this.labMemoTitle19, this.labMemoTitle20,
                    this.labMemoTitle21
                };
                Label[] labMemos = new Label[MemoCount] {
                    this.labMemo01, this.labMemo02, this.labMemo03, this.labMemo04, this.labMemo05,
                    this.labMemo06, this.labMemo07, this.labMemo08, this.labMemo09, this.labMemo10,
                    this.labMemo11, this.labMemo12, this.labMemo13, this.labMemo14, this.labMemo15,
                    this.labMemo16, this.labMemo17, this.labMemo18, this.labMemo19, this.labMemo20,
                    this.labMemo21
                };

                string[] memoValues = null;
                if (studentReceive != null)
                {
                    memoValues = studentReceive.GetAllMemoItemValues();
                }

                //要先將上層物件 Visible 設為 true，否則這一層的 Visible 無法設為 true
                foreach (HtmlTableRow trMemoRow in trMemoRows)
                {
                    trMemoRow.Visible = true;
                }

                for (int idx = 0; idx < labMemoTitles.Length; idx++)
                {
                    Label labMemoTitle = labMemoTitles[idx];
                    Label labMemo = labMemos[idx];
                    string memoTitle = null;
                    if (idx < memoTitles.Length)
                    {
                        memoTitle = memoTitles[idx];
                    }
                    if (String.IsNullOrWhiteSpace(memoTitle))
                    {
                        labMemoTitle.Text = String.Empty;
                        labMemoTitle.Visible = false;
                        labMemo.Text = String.Empty;
                        labMemo.Visible = false;
                    }
                    else
                    {
                        labMemoTitle.Text = String.Concat(memoTitle.Trim(), "：");
                        labMemoTitle.Visible = true;
                        if (memoValues != null && idx < memoValues.Length)
                        {
                            string memoValue = memoValues[idx];
                            labMemo.Text = memoValue == null ? String.Empty : memoValue.Trim();
                        }
                        else
                        {
                            labMemo.Text = String.Empty;
                        }
                        labMemo.Visible = true;
                    }

                }

                bool hasMemeo = false;
                for (int rowIndex = 1; rowIndex < trMemoRows.Length; rowIndex++)
                {
                    int idx1 = (rowIndex - 1) * 2;
                    int idx2 = idx1 + 1;
                    if (labMemoTitles[idx1].Visible || (idx2 < labMemoTitles.Length && labMemoTitles[idx2].Visible))
                    {
                        trMemoRows[rowIndex].Visible = true;
                        hasMemeo = true;
                    }
                    else
                    {
                        trMemoRows[rowIndex].Visible = false;
                    }
                }
                this.trMemoRow00.Visible = hasMemeo;
            }
        }

        /// <summary>
        /// 產生收入科目的 Html
        /// </summary>
        /// <param name="schoolRid"></param>
        /// <param name="studentReceive"></param>
        /// <param name="isEditable"></param>
        /// <returns></returns>
        private string GenReceiveItemHtml(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive)
        {
            if (schoolRid == null || studentReceive == null)
            {
                return String.Empty;
            }

            #region [Old]
            //string[] receiveItemNames = new string[] {
            //    schoolRid.ReceiveItem01, schoolRid.ReceiveItem02, schoolRid.ReceiveItem03, schoolRid.ReceiveItem04, schoolRid.ReceiveItem05,
            //    schoolRid.ReceiveItem06, schoolRid.ReceiveItem07, schoolRid.ReceiveItem08, schoolRid.ReceiveItem09, schoolRid.ReceiveItem10,
            //    schoolRid.ReceiveItem11, schoolRid.ReceiveItem12, schoolRid.ReceiveItem13, schoolRid.ReceiveItem14, schoolRid.ReceiveItem15,
            //    schoolRid.ReceiveItem16, schoolRid.ReceiveItem17, schoolRid.ReceiveItem18, schoolRid.ReceiveItem19, schoolRid.ReceiveItem20,
            //    schoolRid.ReceiveItem21, schoolRid.ReceiveItem22, schoolRid.ReceiveItem23, schoolRid.ReceiveItem24, schoolRid.ReceiveItem25,
            //    schoolRid.ReceiveItem26, schoolRid.ReceiveItem27, schoolRid.ReceiveItem28, schoolRid.ReceiveItem29, schoolRid.ReceiveItem30,
            //    schoolRid.ReceiveItem31, schoolRid.ReceiveItem32, schoolRid.ReceiveItem33, schoolRid.ReceiveItem34, schoolRid.ReceiveItem35,
            //    schoolRid.ReceiveItem36, schoolRid.ReceiveItem37, schoolRid.ReceiveItem38, schoolRid.ReceiveItem39, schoolRid.ReceiveItem40
            //};
            //decimal?[] receiveItemValues = new decimal?[] {
            //    studentReceive.Receive01, studentReceive.Receive02, studentReceive.Receive03, studentReceive.Receive04, studentReceive.Receive05,
            //    studentReceive.Receive06, studentReceive.Receive07, studentReceive.Receive08, studentReceive.Receive09, studentReceive.Receive10,
            //    studentReceive.Receive11, studentReceive.Receive12, studentReceive.Receive13, studentReceive.Receive14, studentReceive.Receive15,
            //    studentReceive.Receive16, studentReceive.Receive17, studentReceive.Receive18, studentReceive.Receive19, studentReceive.Receive20,
            //    studentReceive.Receive21, studentReceive.Receive22, studentReceive.Receive23, studentReceive.Receive24, studentReceive.Receive25,
            //    studentReceive.Receive26, studentReceive.Receive27, studentReceive.Receive28, studentReceive.Receive29, studentReceive.Receive30,
            //    studentReceive.Receive31, studentReceive.Receive32, studentReceive.Receive33, studentReceive.Receive34, studentReceive.Receive35,
            //    studentReceive.Receive36, studentReceive.Receive37, studentReceive.Receive38, studentReceive.Receive39, studentReceive.Receive40
            //};
            #endregion

            #region [MDY:202203XX] 2022擴充案 收入科目 改寫，改用 GetAllReceiveItemChts()
            #region [OLD]
            //string[] receiveItemNames = schoolRid.GetAllReceiveItems();
            #endregion

            string[] receiveItemNames = schoolRid.GetAllReceiveItemChts();
            #endregion

            decimal?[] receiveItemValues = studentReceive.GetAllReceiveItemAmounts();

            StringBuilder html = new StringBuilder();
            for (int idx = 0; idx < receiveItemNames.Length; idx++)
            {
                string receiveItemName = receiveItemNames[idx];
                decimal? receiveItemValue = receiveItemValues[idx];
                if (!String.IsNullOrWhiteSpace(receiveItemName))
                {
                    html
                        .AppendLine("<tr>")
                        .AppendFormat("<td width=\"60%\">{0}</td>", receiveItemName).AppendLine()
                        .AppendFormat("<td>{0}</td>", DataFormat.GetAmountText(receiveItemValue)).AppendLine()
                        .AppendLine("</tr>");
                }
            }

            return html.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
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
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.EditYearId = QueryString.TryGetValue("YearId", String.Empty);
                this.EditTermId = QueryString.TryGetValue("TermId", String.Empty);
                this.EditDepId = QueryString.TryGetValue("DepId", String.Empty);
                this.EditReceiveId = QueryString.TryGetValue("ReceiveId", String.Empty);
                this.EditStuId = QueryString.TryGetValue("StuId", String.Empty);
                this.EditOldSeq = QueryString.TryGetValue("OldSeq", String.Empty);

                #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
                //if (String.IsNullOrEmpty(this.EditReceiveType)
                //    || String.IsNullOrEmpty(this.EditYearId)
                //    || String.IsNullOrEmpty(this.EditTermId)
                //    || String.IsNullOrEmpty(this.EditDepId)
                //    || String.IsNullOrEmpty(this.EditReceiveId)
                //    || String.IsNullOrEmpty(this.EditStuId)
                //    || this.Action != ActionMode.Query)
                //{
                //    //[TODO] 固定顯示訊息的收集
                //    string msg = this.GetLocalized("網頁參數不正確");
                //    this.ShowSystemMessage(msg);
                //    return;
                //}
                #endregion

                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                    || this.EditDepId == null
                    || String.IsNullOrEmpty(this.EditReceiveId)
                    || String.IsNullOrEmpty(this.EditStuId)
                    || String.IsNullOrEmpty(this.EditOldSeq)
                    || this.Action != ActionMode.Query)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }

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
                    return;
                }
                #endregion

                this.InitialUI();

                #region 取得查詢資料
                SchoolRidEntity schoolRid = null;
                StudentReceiveView studentReceive = null;
                StudentMasterEntity student = null;
                bool isOK = this.GetQueryData(out schoolRid, out studentReceive, out student);
                #endregion

                this.BindQueryData(schoolRid, studentReceive, student);
            }
        }

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
    }
}