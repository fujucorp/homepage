using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Configuration;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.student
{
    public partial class student002 : LocalizedPage
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
        /// Keep 查詢的商家代號參數
        /// </summary>
        private string KeepReceiveType
        {
            get
            {
                #region [MDY:20190906] (2019擴充案) 原掃誤判，所以多做轉換
                string value = ViewState["KeepReceiveType"] as string;
                return value == null ? null : Server.HtmlEncode(value);
                #endregion
            }
            set
            {
                ViewState["KeepReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// Keep 查詢的學年參數
        /// </summary>
        private string KeepYearId
        {
            get
            {
                #region [MDY:20190906] (2019擴充案) 原掃誤判，所以多做轉換
                string value = ViewState["KeepYearId"] as string;
                return value == null ? null : Server.HtmlEncode(value);
                #endregion
            }
            set
            {
                ViewState["KeepYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// Keep 查詢的學期參數
        /// </summary>
        private string KeepTermId
        {
            get
            {
                #region [MDY:20190906] (2019擴充案) 原掃誤判，所以多做轉換
                string value = ViewState["KeepTermId"] as string;
                return value == null ? null : Server.HtmlEncode(value);
                #endregion
            }
            set
            {
                ViewState["KeepTermId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// Keep 查詢的部別參數
        /// </summary>
        private string KeepDepId
        {
            get
            {
                #region [MDY:20190906] (2019擴充案) 原掃誤判，所以多做轉換
                string value = ViewState["KeepDepId"] as string;
                return value == null ? null : Server.HtmlEncode(value);
                #endregion
            }
            set
            {
                ViewState["KeepDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// Keep 查詢的代收費用別參數
        /// </summary>
        private string KeepReceiveId
        {
            get
            {
                #region [MDY:20190906] (2019擴充案) 原掃誤判，所以多做轉換
                string value = ViewState["KeepReceiveId"] as string;
                return value == null ? null : Server.HtmlEncode(value);
                #endregion
            }
            set
            {
                ViewState["KeepReceiveId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// Keep 查詢的學號參數
        /// </summary>
        private string KeepStuId
        {
            get
            {
                #region [MDY:20190906] (2019擴充案) 原掃誤判，所以多做轉換
                string value = ViewState["KeepStuId"] as string;
                return value == null ? null : Server.HtmlEncode(value);
                #endregion
            }
            set
            {
                ViewState["KeepStuId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// Keep 查詢的舊資料序號參數
        /// </summary>
        private int KeepOldSeq
        {
            get
            {
                object value = ViewState["KeepOldSeq"];
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
                ViewState["KeepOldSeq"] = value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// Keep 查詢的銷帳編號
        /// </summary>
        private string KeepCancelNo
        {
            get
            {
                return ViewState["KeepCancelNo"] as string;
            }
            set
            {
                ViewState["KeepCancelNo"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// Keey 資料的銷帳狀態
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
        #endregion

        #region [MDY:20200807] M202008_01 Keep 學生姓名是否要遮罩頁面參數
        /// <summary>
        /// 學生姓名是否要遮罩
        /// </summary>
        private bool KeepIsMaskReceiveType
        {
            get
            {
                object value = ViewState["KeepIsMaskReceiveType"];
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
                ViewState["KeepIsMaskReceiveType"] = value;
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
            this.BindMemoBlockUI(null, null, false);
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

            #region 按鈕
            this.lbtnGenPDF.Visible = false;
            this.labGenBillMemo.Visible = false;

            #region [MDY:20190906] (2019擴充案) 多語系
            this.labGenBillMemo.Text = this.GetLocalized("如要採用實體ATM超商臨櫃方式繳款請點此按鈕");
            #endregion

            this.lbtnATMPay.Visible = false;
            this.lbtnNetPay.Visible = false;
            this.lbtnCardPay1.Visible = false;
            this.lbtnCardPay3.Visible = false;
            this.lbtnUnionPay.Visible = false;
            #endregion
        }

        #region [MDY:202203XX] 2022擴充案 英文資料啟用
        /// <summary>
        /// 取得查詢資料
        /// </summary>
        /// <param name="student">傳回學生基本資料</param>
        /// <param name="receive">傳回學生繳費資料</param>
        /// <param name="receiveItem">傳回代收費用別</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetQueryData(out StudentMasterEntity student, out StudentReceiveView receive, out SchoolRidEntity receiveItem
            , out SchoolRTypeEntity school, out bool hasCTCBChannel, out bool hasFISCChannel)
        {
            student = null;
            receive = null;
            receiveItem = null;
            school = null;
            hasCTCBChannel = false;
            hasFISCChannel = false;
            string action = ActionMode.GetActionLocalized(ActionMode.Query);

            #region 學生基本資料
            {
                Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, this.KeepReceiveType)
                    .And(StudentMasterEntity.Field.DepId, this.KeepDepId)
                    .And(StudentMasterEntity.Field.Id, this.KeepStuId);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentMasterEntity>(this, where, null, out student);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    return false;
                }
                if (student == null)
                {
                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, this.GetLocalized("學生基本資料不存在"));
                    return false;
                }
            }
            #endregion

            #region 學生繳費資料
            {
                #region [MDY:202203XX] 2022擴充案 改用 GetStudentReceiveView() 取資料
                #region [OLD]
                //Expression where = new Expression(StudentReceiveView.Field.ReceiveType, this.KeepReceiveType)
                //    .And(StudentReceiveView.Field.YearId, this.KeepYearId)
                //    .And(StudentReceiveView.Field.TermId, this.KeepTermId)
                //    .And(StudentReceiveView.Field.DepId, this.KeepDepId)
                //    .And(StudentReceiveView.Field.ReceiveId, this.KeepReceiveId)
                //    .And(StudentReceiveView.Field.StuId, this.KeepStuId)
                //    .And(StudentReceiveView.Field.OldSeq, this.KeepOldSeq);
                //XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveView>(this, where, null, out receive);
                #endregion

                XmlResult xmlResult = DataProxy.Current.GetStudentReceiveView(this, this.KeepReceiveType, this.KeepYearId, this.KeepTermId, this.KeepReceiveId, this.KeepStuId, this.KeepOldSeq, this.isEngUI(), out receive);

                if (!xmlResult.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    return false;
                }
                if (receive == null)
                {
                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, this.GetLocalized("學生繳費資料不存在"));
                    return false;
                }
                #endregion
            }
            #endregion

            #region 代收費用別
            {
                Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, this.KeepReceiveType)
                    .And(SchoolRidEntity.Field.YearId, this.KeepYearId)
                    .And(SchoolRidEntity.Field.TermId, this.KeepTermId)
                    .And(SchoolRidEntity.Field.DepId, this.KeepDepId)
                    .And(SchoolRidEntity.Field.ReceiveId, this.KeepReceiveId);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRidEntity>(this, where, null, out receiveItem);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    return false;
                }
                if (receiveItem == null)
                {
                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, this.GetLocalized("代收費用別資料不存在"));
                    return false;
                }
            }
            #endregion

            #region 代收管道
            {
                #region 檢查中信管道
                {
                    int count = 0;

                    #region [MDY:20170506] 修正中信管道常數為 CTCB
                    #region [Old]
                    //Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, this.KeepReceiveType)
                    //    .And(ReceiveChannelEntity.Field.ChannelId, ChannelHelper.CTCD);
                    #endregion

                    Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, this.KeepReceiveType)
                        .And(ReceiveChannelEntity.Field.ChannelId, ChannelHelper.CTCB);
                    #endregion

                    XmlResult xmlResult = DataProxy.Current.SelectCount<ReceiveChannelEntity>(this.Page, where, out count);
                    if (!xmlResult.IsSuccess)
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        return false;
                    }
                    hasCTCBChannel = (count > 0);
                }
                #endregion

                #region 檢查財金管道
                {
                    Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, this.KeepReceiveType);
                    XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this.Page, where, null, out school);
                    if (!xmlResult.IsSuccess)
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        return false;
                    }

                    hasFISCChannel = !String.IsNullOrEmpty(school.MerchantId) && !String.IsNullOrEmpty(school.TerminalId) && !String.IsNullOrEmpty(school.MerId);
                }
                #endregion
            }
            #endregion

            #region [MDY:2018xxxx] 有該管道的發卡行才顯示該管道
            {
                #region 中信平台
                if (hasCTCBChannel)
                {
                    int count = 0;
                    Expression where = new Expression(CCardBankIdDtlEntity.Field.ApNo, CCardApCodeTexts.CTCB);
                    XmlResult xmlResult = DataProxy.Current.SelectCount<CCardBankIdDtlEntity>(this.Page, where, out count);
                    if (!xmlResult.IsSuccess)
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        return false;
                    }
                    hasCTCBChannel &= (count > 0);
                }
                #endregion

                #region 財金平台
                if (hasFISCChannel)
                {
                    int count = 0;
                    Expression where = new Expression(CCardBankIdDtlEntity.Field.ApNo, CCardApCodeTexts.EZPOS);
                    XmlResult xmlResult = DataProxy.Current.SelectCount<CCardBankIdDtlEntity>(this.Page, where, out count);
                    if (!xmlResult.IsSuccess)
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        return false;
                    }
                    hasFISCChannel &= (count > 0);
                }
                #endregion
            }
            #endregion

            #region [MDY:20200807] M202008_01 取得指定商家代號是否學生姓名要遮罩 (2020806_01)
            this.KeepIsMaskReceiveType = this.IsMaskReceiveType(student.ReceiveType);
            #endregion

            return true;
        }
        #endregion

        #region [MDY:20200807] M202008_01 檢查指定商家代號是否學生姓名要遮罩 (2020806_01)
        /// <summary>
        /// 檢查指定商家代號是否學生姓名要遮罩
        /// </summary>
        /// <param name="receiveType">指定商家代號</param>
        /// <returns>是則傳回 true，否則傳回 false，檢查失敗也傳回 false</returns>
        private bool IsMaskReceiveType(string receiveType)
        {
            if (!String.IsNullOrWhiteSpace(receiveType) && receiveType.Length == 4)
            {
                #region 讀取學生姓名要遮罩的商家代號系統參數
                ConfigEntity data = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.MASK_RECEIVETYPE);
                KeyValueList<OrderByEnum> orderbys = null;
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this.Page, where, orderbys, out data);
                #endregion

                if (xmlResult.IsSuccess && data != null)
                {
                    if (("," + data.ConfigValue + ",").IndexOf("," + receiveType + ",") > -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region [MDY:202203XX] 2022擴充案 英文名稱
        /// <summary>
        /// 結繫查詢資料
        /// </summary>
        /// <param name="student">學生基本資料</param>
        /// <param name="studentReceive">學生繳費資料</param>
        /// <param name="schoolRid">代收費用別</param>
        private void BindQueryData(StudentMasterEntity student, StudentReceiveView studentReceive, SchoolRidEntity schoolRid
            , SchoolRTypeEntity school, bool hasCTCBChannel, bool hasFISCChannel)
        {
            #region 無學生或繳費資料就清空介面
            if (student == null || studentReceive == null)
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
                this.BindMemoBlockUI(null, null, false);

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

                #region 按鈕
                this.lbtnGenPDF.Visible = false;
                this.labGenBillMemo.Visible = false;
                this.lbtnATMPay.Visible = false;
                this.lbtnNetPay.Visible = false;
                this.lbtnCardPay1.Visible = false;
                this.lbtnCardPay3.Visible = false;
                this.lbtnUnionPay.Visible = false;
                #endregion

                return;
            }
            #endregion

            this.KeepCancelNo = studentReceive.CancelNo;

            ucFilter1.GetDataAndBind(this.KeepReceiveType, this.KeepYearId, this.KeepTermId, this.KeepDepId, this.KeepReceiveId);

            #region 學生基本資料
            this.labStuId.Text = student.Id;

            #region [MDY:20200807] M202008_01 依據學生姓名要遮罩的商家代號系統參數決定是否要遮罩 (2020806_01)
            #region [Old]
            //#region [MDY:20200705] 姓名改要遮罩
            //#region 姓名遮罩
            //this.labName.Text = DataFormat.MaskText(student.Name.Trim(), DataFormat.MaskDataType.Name);
            //#endregion

            //#region [Old] 姓名不遮罩
            ////this.labName.Text = student.Name;
            //#endregion
            //#endregion
            #endregion

            if (this.KeepIsMaskReceiveType)
            {
                this.labName.Text = DataFormat.MaskText(student.Name.Trim(), DataFormat.MaskDataType.Name);
            }
            else
            {
                this.labName.Text = student.Name;
            }
            #endregion

            this.labIdNumber.Text = student.IdNumber == null ? string.Empty : DataFormat.MaskText(student.IdNumber.Trim(), DataFormat.MaskDataType.ID);

            #region [Old] 生日、電話、地址要遮
            //this.labTel.Text = student.Tel;
            //this.labBirthday.Text = DataFormat.FormatDateText(student.Birthday);
            //this.labAddress.Text = student.Address;
            #endregion

            this.labTel.Text = student.Tel == null ? String.Empty : DataFormat.MaskText(student.Tel, DataFormat.MaskDataType.Tel);
            this.labBirthday.Text = student.Birthday == null ? String.Empty : DataFormat.MaskText(DataFormat.FormatDateText(student.Birthday), DataFormat.MaskDataType.Birthday);
            this.labAddress.Text = student.Address == null ? String.Empty : DataFormat.MaskText(student.Address, DataFormat.MaskDataType.Address);

            this.labZipCode.Text = student.ZipCode;
            this.labEmail.Text = student.Email;
            this.labStuParent.Text = student.StuParent == null ? String.Empty : DataFormat.MaskText(student.StuParent.Trim(), DataFormat.MaskDataType.Name);
            #endregion

            bool isEngDataUI = this.isEngUI() && school.IsEngEnabled();

            #region 繳費資料 (取回的 studentReceive 已經過是否顯示英文資料介面 處理)
            this.labDeptName.Text = studentReceive.DeptName;
            this.labCollegeName.Text = studentReceive.CollegeName;
            this.labMajorName.Text = studentReceive.MajorName;

            #region [MDY:202203XX] 2022擴充案 學年英文名稱
            this.labStuGrade.Text = GradeCodeTexts.GetText(studentReceive.StuGrade, isEngDataUI);
            #endregion

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
            #endregion

            bool useEngDataUI = this.isEngUI() && school.IsEngEnabled();

            #region 收入明細
            this.litReceiveItemHtml.Text = this.GenReceiveItemHtml(schoolRid, studentReceive, useEngDataUI);
            #endregion

            #region 備註
            this.BindMemoBlockUI(schoolRid, studentReceive, useEngDataUI);

            #region [Old]
            //this.labMemo01.Text = receive.Memo01;
            //this.labMemo02.Text = receive.Memo02;
            //this.labMemo03.Text = receive.Memo03;
            //this.labMemo04.Text = receive.Memo04;
            //this.labMemo05.Text = receive.Memo05;
            //this.labMemo06.Text = receive.Memo06;
            //this.labMemo07.Text = receive.Memo07;
            //this.labMemo08.Text = receive.Memo08;
            //this.labMemo09.Text = receive.Memo09;
            //this.labMemo10.Text = receive.Memo10;
            #endregion
            #endregion

            #region 繳費/銷帳資料
            string cancelStatus = studentReceive.GetCancelStatus();
            this.KeepCancelStatus = cancelStatus;
            this.labReceiveAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveAmount);
            this.labCancelNo.Text = studentReceive.CancelNo;
            this.labReceiveATMAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveAtmamount);
            this.labCancelATMNo.Text = studentReceive.CancelAtmno;
            this.labReceiveSMAmount.Text = DataFormat.GetAmountCommaText(studentReceive.ReceiveSmamount);
            this.labCancelSMNo.Text = studentReceive.CancelSmno;

            #region [MDY:20190906] (2019擴充案) 多語系
            this.labCancelStatus.Text = this.GetLocalized(CancelStatusCodeTexts.GetText(cancelStatus));
            this.labReceiveWay.Text = this.GetLocalized(studentReceive.ReceiveWayName);
            #endregion

            this.labReceiveBankId.Text = studentReceive.ReceivebankId;
            this.labReceiveDate.Text = DataFormat.FormatDateText(studentReceive.ReceiveDate);
            this.labAccountDate.Text = DataFormat.FormatDateText(studentReceive.AccountDate);
            #endregion

            #region [Old]
            //#region 組 收入明細 Html
            //this.litHtml.Text = String.Empty;
            //if (schoolRid != null)
            //{
            //    string[] itemNames = new string[] {
            //        schoolRid.ReceiveItem01, schoolRid.ReceiveItem02, schoolRid.ReceiveItem03, schoolRid.ReceiveItem04, schoolRid.ReceiveItem05,
            //        schoolRid.ReceiveItem06, schoolRid.ReceiveItem07, schoolRid.ReceiveItem08, schoolRid.ReceiveItem09, schoolRid.ReceiveItem10,
            //        schoolRid.ReceiveItem11, schoolRid.ReceiveItem12, schoolRid.ReceiveItem13, schoolRid.ReceiveItem14, schoolRid.ReceiveItem15,
            //        schoolRid.ReceiveItem16, schoolRid.ReceiveItem17, schoolRid.ReceiveItem18, schoolRid.ReceiveItem19, schoolRid.ReceiveItem20,
            //        schoolRid.ReceiveItem21, schoolRid.ReceiveItem22, schoolRid.ReceiveItem23, schoolRid.ReceiveItem24, schoolRid.ReceiveItem25,
            //        schoolRid.ReceiveItem26, schoolRid.ReceiveItem27, schoolRid.ReceiveItem28, schoolRid.ReceiveItem29, schoolRid.ReceiveItem30,
            //        schoolRid.ReceiveItem31, schoolRid.ReceiveItem32, schoolRid.ReceiveItem33, schoolRid.ReceiveItem34, schoolRid.ReceiveItem35,
            //        schoolRid.ReceiveItem36, schoolRid.ReceiveItem37, schoolRid.ReceiveItem38, schoolRid.ReceiveItem39, schoolRid.ReceiveItem40
            //    };
            //    decimal?[] itemValues = new decimal?[] {
            //        studentReceive.Receive01, studentReceive.Receive02, studentReceive.Receive03, studentReceive.Receive04, studentReceive.Receive05,
            //        studentReceive.Receive06, studentReceive.Receive07, studentReceive.Receive08, studentReceive.Receive09, studentReceive.Receive10,
            //        studentReceive.Receive11, studentReceive.Receive12, studentReceive.Receive13, studentReceive.Receive14, studentReceive.Receive15,
            //        studentReceive.Receive16, studentReceive.Receive17, studentReceive.Receive18, studentReceive.Receive19, studentReceive.Receive20,
            //        studentReceive.Receive21, studentReceive.Receive22, studentReceive.Receive23, studentReceive.Receive24, studentReceive.Receive25,
            //        studentReceive.Receive26, studentReceive.Receive27, studentReceive.Receive28, studentReceive.Receive29, studentReceive.Receive30,
            //        studentReceive.Receive31, studentReceive.Receive32, studentReceive.Receive33, studentReceive.Receive34, studentReceive.Receive35,
            //        studentReceive.Receive36, studentReceive.Receive37, studentReceive.Receive38, studentReceive.Receive39, studentReceive.Receive40
            //    };

            //    StringBuilder html = new StringBuilder();
            //    for (int idx = 0; idx < itemNames.Length; idx++)
            //    {
            //        string itemName = itemNames[idx];
            //        decimal? amount = itemValues[idx];
            //        if (!String.IsNullOrWhiteSpace(itemName))
            //        {
            //            html
            //                .AppendLine("<tr>")
            //                .AppendFormat("<td width=\"60%\">{0}</td>", itemName).AppendLine()
            //                .AppendFormat("<td style=\"text-align:left\">{0}</td>", DataFormat.GetAmountCommaText(amount)).AppendLine()
            //                .AppendLine("</tr>");
            //        }
            //    }
            //    this.litHtml.Text = html.ToString();
            //}
            //#endregion
            #endregion

            #region 處理按鈕
            {
                #region 按鈕顯示邏輯
                //開放列印期限內：(開放日期為空白，或是開放日期小於等於系統日) & (關閉日期為空白，或是關閉日期大於系統日)
                //未繳款 + 開放列印期限內 + 有效帳單：可以列印繳費單、線上繳款(網路ATM、網路銀行)
                //未繳款 + 開放列印期限內 + 有效帳單 + 有財金管道 + 財金期限內：財金平台信用卡繳款
                //未繳款 + 開放列印期限內 + 有效帳單 + 有中信管道 + 中信期限內：中信平台信用卡繳款、銀聯卡繳款
                //已繳款(未入帳)：可以列印收據
                //已入帳：可以列印收據
                #endregion

                DateTime today = DateTime.Today;
                //DateTime? payDueDate = DataFormat.ConvertDateText(receiveItem.PayDate);
                DateTime? openDate = DataFormat.ConvertDateText(schoolRid.BillValidDate);
                DateTime? closeDate = DataFormat.ConvertDateText(schoolRid.BillCloseDate);
                bool isOpened = ((openDate == null || openDate.Value <= today) && (closeDate == null || closeDate.Value > today));
                switch (cancelStatus)
                {
                    case CancelStatusCodeTexts.NONPAY:
                        #region 未繳款
                        {
                            //bool hasAmount = studentReceive.ReceiveAmount != null;
                            //bool isPayable = (!String.IsNullOrEmpty(studentReceive.CancelNo) && hasAmount && studentReceive.ReceiveAmount > 0);
                            //bool isVisible = (hasAmount && ((studentReceive.ReceiveAmount > 0 && !String.IsNullOrEmpty(studentReceive.CancelNo)) || studentReceive.ReceiveAmount <= 0));
                            //bool isVisible = (!String.IsNullOrEmpty(receive.CancelNo) && receive.ReceiveAmount != null);
                            bool isBillable = (studentReceive.ReceiveAmount != null && ((studentReceive.ReceiveAmount > 0 && !String.IsNullOrEmpty(studentReceive.CancelNo)) || studentReceive.ReceiveAmount <= 0));    //有效帳單

                            this.lbtnGenPDF.LocationText = "產生PDF繳費單";
                            //this.lbtnGenPDF.Visible = ((payDueDate != null && payDueDate.Value >= today)
                            //        && (openDate == null || openDate.Value <= today)
                            //        && (closeDate == null || closeDate.Value >= today)
                            //        && isVisible);
                            //this.lbtnGenPDF.Visible = isOpened && isBillable;
                            //this.labGenBillMemo.Visible = this.lbtnGenPDF.Visible;

                            #region 網路ATM繳款
                            string eatmPayUrl = this.GetEATMPayUrl(studentReceive);
                            if (!String.IsNullOrEmpty(eatmPayUrl))
                            {
                                this.lbtnATMPay.OnClientClick = "window.open('" + eatmPayUrl + "', '網路ATM繳款', config='toolbar=no,resizable=yes,scrollbars=yes,location=no,menubar=no,status=no'); return false;";
                            }
                            else
                            {
                                this.lbtnATMPay.OnClientClick = "alert('本功能即將開放，敬請期待'); return false;";
                            }
                            #endregion

                            #region 網銀繳款
                            string netBankPayUrl = this.GetNetBankPayUrl(studentReceive);
                            if (!String.IsNullOrEmpty(netBankPayUrl))
                            {
                                this.lbtnNetPay.OnClientClick = "window.open('" + netBankPayUrl + "', '網銀繳款', config='toolbar=no,resizable=yes,scrollbars=yes,location=no,menubar=no,status=no'); return false;";
                            }
                            else
                            {
                                this.lbtnNetPay.OnClientClick = "alert('本功能即將開放，敬請期待'); return false;";
                            }
                            #endregion

                            //this.lbtnCardPay.OnClientClick = "window.open('" + GetCardPayUrl(studentReceive) + "', '信用卡繳款', config='toolbar=no,resizable=yes,scrollbars=yes,location=no,menubar=no,status=no'); return false;";

                            this.lbtnCardPay1.OnClientClick = "window.open('" + GetCardPayUrl(studentReceive, CCardApCodeTexts.EZPOS) + "', '信用卡繳款', config='toolbar=no,resizable=yes,scrollbars=yes,location=no,menubar=no,status=no'); return false;";

                            #region [MDY:2018xxxx] 改成直接連中信的 i繳費
                            #region [OLD]
                            //this.lbtnCardPay3.OnClientClick = "window.open('" + GetCardPayUrl(studentReceive, CCardApCodeTexts.CTCB) + "', '信用卡繳款', config='toolbar=no,resizable=yes,scrollbars=yes,location=no,menubar=no,status=no'); return false;";
                            #endregion

                            #region [MDY:20210823] 中信 i 繳費網址改成 https://www.27608818.com/web/
                            this.lbtnCardPay3.OnClientClick = "window.open('https://www.27608818.com/web/', '信用卡繳款', config='toolbar=no,resizable=yes,scrollbars=yes,location=no,menubar=no,status=no'); return false;";
                            #endregion
                            #endregion

                            if (isOpened && isBillable)
                            {
                                this.lbtnGenPDF.Visible = true;
                                this.lbtnATMPay.Visible = true;
                                this.lbtnNetPay.Visible = true;

                                StringBuilder memo = new StringBuilder();
                                if (hasCTCBChannel)
                                {
                                    DateTime? payDueDate2 = DataFormat.ConvertDateText(schoolRid.PayDueDate2);
                                    if (payDueDate2 != null && payDueDate2 >= today)
                                    {
                                        this.lbtnCardPay3.Visible = true;
                                        this.lbtnUnionPay.Visible = true;
                                    }
                                    else
                                    {
                                        this.lbtnCardPay3.Visible = false;
                                        this.lbtnUnionPay.Visible = false;

                                        #region [MDY:2018xxxx] 沒有設定日期不顯示過期訊息
                                        if (payDueDate2 != null)
                                        {
                                            #region [MDY:20190906] (2019擴充案) 多語系
                                            memo.AppendLine(this.GetLocalized("已過中信平台管道繳費期限"));
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    this.lbtnCardPay3.Visible = false;
                                    this.lbtnUnionPay.Visible = false;

                                    #region [MDY:20190906] (2019擴充案) 多語系
                                    memo.AppendLine(this.GetLocalized("未提供中信平台管道繳費"));
                                    #endregion
                                }

                                if (hasFISCChannel)
                                {
                                    DateTime? payDueDate3 = DataFormat.ConvertDateText(schoolRid.PayDueDate3);
                                    if (payDueDate3 != null && payDueDate3 >= today)
                                    {
                                        this.lbtnCardPay1.Visible = true;
                                    }
                                    else
                                    {
                                        this.lbtnCardPay1.Visible = false;

                                        #region [MDY:2018xxxx] 沒有設定日期不顯示過期訊息
                                        if (payDueDate3 != null)
                                        {
                                            #region [MDY:20190906] (2019擴充案) 多語系
                                            memo.AppendLine(this.GetLocalized("已過財金平台管道繳費期限"));
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    this.lbtnCardPay1.Visible = false;

                                    #region [MDY:20190906] (2019擴充案) 多語系
                                    memo.AppendLine(this.GetLocalized("未提供財金平台管道繳費"));
                                    #endregion
                                }

                                this.divMemo.InnerText = memo.ToString();
                                this.divMemo.Visible = (memo.Length > 0);
                            }
                            else
                            {
                                #region [MDY:20190906] (2019擴充案) 多語系
                                this.divMemo.InnerText = this.GetLocalized("此繳費單未開放或是無效的繳費單");
                                #endregion

                                this.lbtnGenPDF.Visible = false;
                                this.lbtnATMPay.Visible = false;
                                this.lbtnNetPay.Visible = false;
                                this.lbtnCardPay1.Visible = false;
                                this.lbtnCardPay3.Visible = false;
                                this.lbtnUnionPay.Visible = false;
                            }

                            this.labGenBillMemo.Visible = this.lbtnGenPDF.Visible;
                        }
                        #endregion
                        break;
                    case CancelStatusCodeTexts.PAYED:
                        #region 已繳款(未入帳)
                        {
                            this.lbtnGenPDF.LocationText = "產生PDF憑單";

                            #region [MDY:2018xxxx] 判斷 列印收據關閉日 決定是否顯示 產生PDF憑單按鈕
                            #region [OLD]
                            //this.lbtnGenPDF.Visible = true;
                            #endregion

                            DateTime? invoiceCloseDate = DataFormat.ConvertDateText(studentReceive.InvoiceCloseDate);
                            this.lbtnGenPDF.Visible = (!invoiceCloseDate.HasValue || invoiceCloseDate.Value.Date > today);
                            #endregion

                            this.labGenBillMemo.Visible = false;

                            this.lbtnATMPay.Visible = false;
                            this.lbtnNetPay.Visible = false;
                            this.lbtnCardPay1.Visible = false;
                            this.lbtnCardPay3.Visible = false;
                            this.lbtnUnionPay.Visible = false;

                            this.labGenBillMemo.Visible = false;
                        }
                        #endregion
                        break;
                    case CancelStatusCodeTexts.CANCELED:
                        #region 已入帳
                        {
                            this.lbtnGenPDF.LocationText = "產生PDF收據";

                            #region [MDY:2018xxxx] 判斷 列印收據關閉日 決定是否顯示 產生PDF收據按鈕
                            #region [OLD]
                            //this.lbtnGenPDF.Visible = true;
                            #endregion

                            DateTime? invoiceCloseDate = DataFormat.ConvertDateText(studentReceive.InvoiceCloseDate);
                            this.lbtnGenPDF.Visible = (!invoiceCloseDate.HasValue || invoiceCloseDate.Value.Date > today);
                            #endregion

                            this.labGenBillMemo.Visible = false;

                            this.lbtnATMPay.Visible = false;
                            this.lbtnNetPay.Visible = false;
                            this.lbtnCardPay1.Visible = false;
                            this.lbtnCardPay3.Visible = false;
                            this.lbtnUnionPay.Visible = false;

                            this.labGenBillMemo.Visible = false;
                        }
                        #endregion
                        break;
                }
            }
            #endregion
        }

        /// <summary>
        /// 結繫備註區塊介面
        /// </summary>
        /// <param name="schoolRid"></param>
        /// <param name="studentReceive"></param>
        /// <param name="enabled"></param>
        private void BindMemoBlockUI(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive, bool useEngDataUI)
        {
            HtmlTableRow[] trMemoRows = new System.Web.UI.HtmlControls.HtmlTableRow[] {
                this.trMemoRow00,
                this.trMemoRow01, this.trMemoRow02, this.trMemoRow03, this.trMemoRow04, this.trMemoRow05,
                this.trMemoRow06, this.trMemoRow07, this.trMemoRow08, this.trMemoRow09, this.trMemoRow10,
                this.trMemoRow11
            };

            #region [MDY:202203XX] 2022擴充案 備註項目 改寫，改用 GetAllMemoTitle()
            #region [OLD]
            //string[] memoTitles = schoolRid == null ? null : schoolRid.GetAllMemoTitles();
            #endregion

            string[] memoTitles = schoolRid?.GetAllMemoTitle(useEngDataUI);
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
        private string GenReceiveItemHtml(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive, bool useEngDataUI)
        {
            if (schoolRid == null || studentReceive == null)
            {
                return String.Empty;
            }

            #region [MDY:202203XX] 2022擴充案 收入科目 改寫，改用 GetAllReceiveItems()
            #region [OLD]
            //string[] receiveItemNames = schoolRid.GetAllReceiveItems();
            #endregion

            string[] receiveItemNames = schoolRid.GetAllReceiveItems(useEngDataUI);
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
        #endregion

        #region [Old]
        ///// <summary>
        ///// 結繫備註區塊介面
        ///// </summary>
        ///// <param name="schoolRid"></param>
        ///// <param name="studentReceive"></param>
        //private void BindMemoBlockUI(SchoolRidEntity schoolRid, StudentReceiveEntity studentReceive)
        //{
        //    string[] memoTitles = schoolRid == null ? null : schoolRid.GetAllMemoTitles();

        //    if (memoTitles == null || memoTitles.Length == 0)
        //    {
        //        this.trMemoRow0.Visible = false;
        //        this.trMemoRow1.Visible = false;
        //        this.trMemoRow2.Visible = false;
        //        this.trMemoRow3.Visible = false;
        //        this.trMemoRow4.Visible = false;
        //        this.trMemoRow5.Visible = false;
        //        this.trMemoRow0.Visible = false;
        //    }
        //    else
        //    {
        //        System.Web.UI.HtmlControls.HtmlTableRow[] trMemoRows = new System.Web.UI.HtmlControls.HtmlTableRow[] {
        //            this.trMemoRow1, this.trMemoRow2, this.trMemoRow3, this.trMemoRow4, this.trMemoRow5,
        //        };
        //        Label[] labMemoTitles = new Label[] {
        //            this.labMemoTitle01, this.labMemoTitle02, this.labMemoTitle03, this.labMemoTitle04, this.labMemoTitle05,
        //            this.labMemoTitle06, this.labMemoTitle07, this.labMemoTitle08, this.labMemoTitle09, this.labMemoTitle10
        //        };
        //        Label[] labMemos = new Label[] {
        //            this.labMemo01, this.labMemo02, this.labMemo03, this.labMemo04, this.labMemo05,
        //            this.labMemo06, this.labMemo07, this.labMemo08, this.labMemo09, this.labMemo10
        //        };

        //        string[] memoValues = null;
        //        if (studentReceive != null)
        //        {
        //            memoValues = new string[] {
        //                studentReceive.Memo01, studentReceive.Memo02, studentReceive.Memo03, studentReceive.Memo04, studentReceive.Memo05,
        //                studentReceive.Memo06, studentReceive.Memo07, studentReceive.Memo08, studentReceive.Memo09, studentReceive.Memo10
        //            };
        //        }

        //        //要先將上層物件 Visible 設為 true，否則這一層的 Visible 無法設為 true
        //        for (int rowIndex = 0; rowIndex < trMemoRows.Length; rowIndex++)
        //        {
        //            trMemoRows[rowIndex].Visible = true;
        //        }

        //        for (int idx = 0; idx < labMemoTitles.Length; idx++)
        //        {
        //            Label labMemoTitle = labMemoTitles[idx];
        //            Label labMemo = labMemos[idx];
        //            string memoTitle = null;
        //            if (idx < memoTitles.Length)
        //            {
        //                memoTitle = memoTitles[idx] == null ? null : memoTitles[idx].Trim();
        //            }
        //            string memoValue = null;
        //            if (idx < memoValues.Length)
        //            {
        //                memoValue = memoValues[idx] == null ? String.Empty : memoValues[idx].Trim();
        //            }
        //            if (String.IsNullOrEmpty(memoTitle))
        //            {
        //                labMemoTitle.Text = String.Empty;
        //                labMemoTitle.Visible = false;
        //                labMemo.Text = String.Empty;
        //                labMemo.Visible = false;
        //            }
        //            else
        //            {
        //                labMemoTitle.Text = memoTitle + "：";
        //                labMemoTitle.Visible = true;
        //                labMemo.Text = memoValue;
        //                labMemo.Visible = true;
        //            }
        //        }

        //        bool hasMemeo = false;
        //        for (int rowIndex = 0; rowIndex < trMemoRows.Length; rowIndex++)
        //        {
        //            int idx = rowIndex * 2;
        //            if (labMemoTitles[idx].Visible || labMemoTitles[idx + 1].Visible)
        //            {
        //                trMemoRows[rowIndex].Visible = true;
        //                hasMemeo = true;
        //            }
        //            else
        //            {
        //                trMemoRows[rowIndex].Visible = false;
        //            }
        //        }
        //        this.trMemoRow0.Visible = hasMemeo;
        //    }
        //}
        #endregion

        /// <summary>
        /// 取得網路ATM繳款網址
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetEATMPayUrl(StudentReceiveView data)
        {
            #region Sample
            //https://10.253.21.126/2008eatm.aspx?id=eatm&vacno=59221234567890&amt=10000
            #endregion

            #region [MDY:20160416] 配合共用 Config 設定，改用 GetMyMachineProjectConfigValue
            #region Old
            //string eatmPayUrl = ConfigManager.Current.GetProjectConfigValue("PayUrl", "EATM");
            #endregion

            string eatmPayUrl = ConfigManager.Current.GetMyMachineProjectConfigValue("PayUrl", "EATM");
            #endregion

            if (String.IsNullOrWhiteSpace(eatmPayUrl))
            {
                return null;
            }

            string cancelNo = data.CancelNo == null ? "" : data.CancelNo.Trim();
            string receiveAmount = data.ReceiveAmount == null ? "0" : data.ReceiveAmount.Value.ToString("0");
            string url = String.Format("{0}&vacno={1}&amt={2}", eatmPayUrl, cancelNo, receiveAmount);
            return url;
        }

        /// <summary>
        /// 取得網銀繳款網址
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetNetBankPayUrl(StudentReceiveView data)
        {
            #region Sample
            //https://10.253.21.137/ConsumerBank/Auth/logon.aspx?ReturnUrl=%2fconsumerbank%2fdesktopmodules%2ffundstransfer%2fnbtuition.aspx%3fvacno%3d50529110036587%26amt%3d36270&vacno=50529110036587&amt=36270
            #endregion

            #region [MDY:20160416] 配合共用 Config 設定，改用 GetMyMachineProjectConfigValue
            #region Old
            //string netBankPayUrl = ConfigManager.Current.GetProjectConfigValue("PayUrl", "NetBank");
            #endregion

            string netBankPayUrl = ConfigManager.Current.GetMyMachineProjectConfigValue("PayUrl", "NetBank");
            #endregion

            if (String.IsNullOrWhiteSpace(netBankPayUrl))
            {
                return null;
            }

            string cancelNo = data.CancelNo == null ? "" : data.CancelNo.Trim();
            string receiveAmount = data.ReceiveAmount == null ? "0" : data.ReceiveAmount.Value.ToString("0");
            string url = String.Format(netBankPayUrl, cancelNo, receiveAmount);
            return url;
        }

        private string GetCardPayUrl(StudentReceiveView data, string cardApNo)
        {
            #region [MDY:20210706] FIX BUG 原碼修正
            #region [OLD]
            //string arg = String.Format("&cno={0}&apno={1}", data.CancelNo, cardApNo);
            //string keyVal = Common.GetBase64Encode(String.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.StuId, data.OldSeq));
            //string key = String.Format("&key={0}", keyVal);
            //return String.Format("{0}?{1}{2}", this.GetResolveUrl("~/creditcard.aspx"), arg, key);
            #endregion

            string keyVal = Common.GetBase64Encode(String.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.StuId, data.OldSeq));
            string arg = HttpUtility.UrlPathEncode(String.Format("cno={0}&apno={1}&key={2}", data.CancelNo, cardApNo, keyVal));
            return String.Format("{0}?{1}", this.GetResolveUrl("~/creditcard.aspx"), arg);
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

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
                this.KeepReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.KeepYearId = QueryString.TryGetValue("YearId", String.Empty);
                this.KeepTermId = QueryString.TryGetValue("TermId", String.Empty);
                this.KeepDepId = QueryString.TryGetValue("DepId", String.Empty);
                this.KeepReceiveId = QueryString.TryGetValue("ReceiveId", String.Empty);
                this.KeepStuId = QueryString.TryGetValue("StuId", String.Empty);
                string oldSeq = QueryString.TryGetValue("OldSeq", String.Empty);

                #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
                //if (String.IsNullOrEmpty(this.KeepReceiveType)
                //    || String.IsNullOrEmpty(this.KeepYearId)
                //    || String.IsNullOrEmpty(this.KeepTermId)
                //    || String.IsNullOrEmpty(this.KeepDepId)
                //    || String.IsNullOrEmpty(this.KeepReceiveId)
                //    || String.IsNullOrEmpty(this.KeepStuId))
                //{
                //    //[TODO] 固定顯示訊息的收集
                //    string msg = this.GetLocalized("網頁參數不正確");
                //    this.ShowSystemMessage(msg);
                //    return;
                //}
                #endregion

                int editOldSeq = 0;
                if (String.IsNullOrEmpty(this.KeepReceiveType)
                    || String.IsNullOrEmpty(this.KeepYearId)
                    || String.IsNullOrEmpty(this.KeepTermId)
                    || this.KeepDepId == null
                    || String.IsNullOrEmpty(this.KeepReceiveId)
                    || String.IsNullOrEmpty(this.KeepStuId)
                    || !Int32.TryParse(oldSeq, out editOldSeq) || editOldSeq < 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }
                this.KeepOldSeq = editOldSeq;
                #endregion

                StudentMasterEntity student = null;
                StudentReceiveView receive = null;
                SchoolRidEntity receiveItem = null;
                bool hasCTCBChannel = false;
                bool hasFISCChannel = false;

                #region [MDY:202203XX] 2022擴充案 英文資料啟用
                SchoolRTypeEntity school = null;
                if (this.GetQueryData(out student, out receive, out receiveItem, out school, out hasCTCBChannel, out hasFISCChannel))
                {
                    this.BindQueryData(student, receive, receiveItem, school, hasCTCBChannel, hasFISCChannel);
                }
                #endregion
            }
        }

        #region [Old]
        ///// <summary>
        ///// 取得管道名稱
        ///// </summary>
        ///// <param name="receiveWay">管道代碼</param>
        ///// <returns></returns>
        //private string GetChannelName(string receiveWay)
        //{
        //    ChannelSetEntity entity = null;

        //    Expression where = new Expression();
        //    where.And(ChannelSetEntity.Field.ChannelId, receiveWay);

        //    XmlResult result = DataProxy.Current.SelectFirst<ChannelSetEntity>(this, where, null, out entity);
        //    if (!result.IsSuccess || entity == null)
        //    {
        //        return string.Empty;
        //    }
        //    else
        //    {
        //        return entity.ChannelName;
        //    }
        //}
        #endregion

        protected void lbtnGenPDF_Click(object sender, EventArgs e)
        {
            string command = null;
            string pdfName = null;
            bool isEngUI = this.isEngUI();
            switch (this.KeepCancelStatus)
            {
                case CancelStatusCodeTexts.NONPAY:
                    command = "GENBILL";
                    pdfName = isEngUI ? "Bill" : "繳費單";
                    break;
                case CancelStatusCodeTexts.PAYED:
                    command = "GENRECEIPT";
                    pdfName = isEngUI ? "Proofs" : "憑單";
                    break;
                case CancelStatusCodeTexts.CANCELED:
                    command = "GENRECEIPT";
                    pdfName = isEngUI ? "Receipt" : "收據";
                    break;
            }

            byte[] pdfContent = null;

            #region [MDY:20200807] M202008_01 依據學生姓名要遮罩的商家代號系統參數決定是否要遮罩 (2020806_01)
            XmlResult xmlResult = DataProxy.Current.ExecB2100002Request(this.Page, command
                , this.KeepReceiveType, this.KeepYearId, this.KeepTermId, this.KeepDepId, this.KeepReceiveId, this.KeepStuId, this.KeepOldSeq
                , this.KeepIsMaskReceiveType, isEngUI, out pdfContent);
            #endregion

            if (xmlResult.IsSuccess)
            {
                #region [MDY:20190906] (2019擴充案) 多語系
                #region [MDY:20210401] 原碼修正
                string fileName = String.Format("{0}_{1}.PDF", HttpUtility.UrlEncode(KeepStuId), pdfName);
                #endregion
                #endregion

                this.ResponseFile(fileName, pdfContent);
            }
            else
            {
                this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
            }
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Server.Transfer("student001.aspx");
        }

        #region [Old]
        ///// <summary>
        ///// 取得 所屬收入科目
        ///// </summary>
        //private ListItem[] GetReceiveItemOptions()
        //{
        //    List<ListItem> list = new List<ListItem>();

        //    SchoolRidEntity entity = null;

        //    Expression where = new Expression();
        //    where.And(SchoolRidEntity.Field.ReceiveType, this.EditReceiveType);
        //    where.And(SchoolRidEntity.Field.YearId, this.EditYearId);
        //    where.And(SchoolRidEntity.Field.TermId, this.EditTermId);
        //    where.And(SchoolRidEntity.Field.DepId, this.EditDepId);
        //    where.And(SchoolRidEntity.Field.ReceiveId, this.EditReceiveId);

        //    XmlResult result = DataProxy.Current.SelectFirst<SchoolRidEntity>(this, where, null, out entity);
        //    if (!result.IsSuccess || entity == null)
        //    {
        //        return null;
        //    }

        //    for (int i = 1; i < 31; i++)
        //    {
        //        string fieldName = "Receive_Item" + i.ToString("00");

        //        Result rt = new Result();
        //        object ob = entity.GetValue(fieldName, out rt);
        //        if (ob != null)
        //        {
        //            if (!String.IsNullOrEmpty(ob.ToString()))
        //            {
        //                list.Add(new ListItem(ob.ToString(), i.ToString()));
        //            }
        //        }
        //    }

        //    return list.ToArray();
        //}
        #endregion
    }
}