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
    /// 查詢歷史(繳費)資料(明細)
    /// </summary>
    public partial class C3700009D : BasePage
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
        /// 編輯的流水號參數
        /// </summary>
        private Int64 EditSN
        {
            get
            {
                object value = ViewState["EditSN"];
                if (value is Int64)
                {
                    return (Int64)value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["EditSN"] = value;
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
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
            //this.BindMemoBlockUI(null, null);
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

        private void BindEditData(HistoryEntity data)
        {
            this.InitialUI();

            if (data == null)
            {
                return;
            }

            HistoryHelper helper = new HistoryHelper(null);

            #region 學生基本資料
            StudentMasterEntity student = null;
            if (!String.IsNullOrEmpty(data.StudentXml))
            {
                student = helper.DeStudentXml(data.StudentXml);
                if (student == null)
                {
                    this.ShowSystemMessage("學生基本資料無法反序列化");
                    return;
                }
            }

            if (student != null)
            {
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
            }
            #endregion

            #region 學生繳費資料
            StudentReceiveView studentReceive = null;
            if (!String.IsNullOrEmpty(data.StudentReceiveXml))
            {
                studentReceive = helper.DeStudentReceiveXml(data.StudentReceiveXml);
                if (studentReceive == null)
                {
                    this.ShowSystemMessage("學生繳費資料無法反序列化");
                    return;
                }
            }
            {
                this.labUpNo.Text = String.Format("{0}、{1}", studentReceive.UpNo, studentReceive.OldSeq);
                this.labDeptName.Text = studentReceive.DeptName;
                this.labCollegeName.Text = studentReceive.CollegeName;
                this.labMajorName.Text = studentReceive.MajorName;
                this.labStuGrade.Text = studentReceive.StuGradeName;
                this.labClassName.Text = studentReceive.ClassName;
                this.labStuHid.Text = studentReceive.StuHid;
                this.labDormName.Text = studentReceive.DormName;
                this.labReduceName.Text = studentReceive.ReduceName;                        //減免
                this.labLoan.Text = DataFormat.GetAmountText(studentReceive.Loan);          //上傳就學貸款金額
                this.labLoanName.Text = studentReceive.LoanName;                            //就貸
                this.labRealLoan.Text = DataFormat.GetAmountText(studentReceive.RealLoan);  //原就學貸款金額
                this.labIdentifyId01Name.Text = studentReceive.IdentifyName01;      //身份註記一
                this.labIdentifyId02Name.Text = studentReceive.IdentifyName02;      //身份註記二 
                this.labIdentifyId03Name.Text = studentReceive.IdentifyName03;      //身份註記三
                this.labIdentifyId04Name.Text = studentReceive.IdentifyName04;      //身份註記四
                this.labIdentifyId05Name.Text = studentReceive.IdentifyName05;      //身份註記五
                this.labIdentifyId06Name.Text = studentReceive.IdentifyName06;      //身份註記六
                this.labStuCredit.Text = DataFormat.GetIntegerText(studentReceive.StuCredit);
                this.labStuHour.Text = DataFormat.GetIntegerText(studentReceive.StuHour);
                this.labReissueFlag.Text = ReissueFlagCodeTexts.GetText(studentReceive.ReissueFlag);
            }
            #endregion

            #region 代收費用別資料
            SchoolRidView3 schoolRid = null;
            if (!String.IsNullOrEmpty(data.SchoolRidXml))
            {
                schoolRid = helper.DeSchoolRidXml(data.SchoolRidXml);
                if (schoolRid == null)
                {
                    this.ShowSystemMessage("代收費用別資料無法反序列化");
                    return;
                }
            }
            #endregion

            #region 收入明細
            this.litReceiveItemHtml.Text = this.GenReceiveItemHtml(schoolRid, studentReceive);
            #endregion

            #region 備註
            this.BindMemoBlockUI(schoolRid, studentReceive);
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
            this.labReceiveWay.Text = studentReceive.ReceiveWayName;
            this.labReceiveBankId.Text = studentReceive.ReceivebankId;
            this.labReceiveDate.Text = DataFormat.FormatDateText(studentReceive.ReceiveDate);
            this.labAccountDate.Text = DataFormat.FormatDateText(studentReceive.AccountDate);
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
                string snTxt = QueryString.TryGetValue("SN", String.Empty);
                Int64 sn = 0;

                if (String.IsNullOrWhiteSpace(snTxt)
                    || !Int64.TryParse(snTxt, out sn) || sn < 1
                    || this.Action != ActionMode.Query)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }
                #endregion

                this.EditSN = sn;

                #region 取資料
                HistoryEntity data = null;
                {
                    Expression where = new Expression(HistoryEntity.Field.SN, sn);
                    XmlResult xmlResult = DataProxy.Current.SelectFirst<HistoryEntity>(this.Page, where, null, out data);
                    if (!xmlResult.IsSuccess)
                    {
                        this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                    }
                }
                #endregion

                #region 結繫資料
                this.BindEditData(data);
                #endregion
            }
        }
    }
}