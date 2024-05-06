using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    public partial class creditcard_d : LocalizedPage, ICreditCardPage
    {
        #region Override IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "CreditCard";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "信用卡繳費";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public override bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public override bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public override bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region Keep 頁面參數
        /// <summary>
        /// Keep 學生頁面傳過來的 Student_Receive 的 Cancel_No
        /// </summary>
        public string KeepCancelNo
        {
            get
            {
                return ViewState["KeepCancelNo"] as string ?? String.Empty;
            }
            set
            {
                ViewState["KeepCancelNo"] = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// Keep 學生頁面傳過來的 Student_Receive 的 PKey
        /// </summary>
        public string[] KeepKeys
        {
            get
            {
                return ViewState["KeepKeys"] as string[];
            }
            set
            {
                ViewState["KeepKeys"] = value;
            }
        }


        /// <summary>
        /// Keep 發卡銀行
        /// </summary>
        public string KeepBankId
        {
            get
            {
                return ViewState["KeepBankId"] as string ?? String.Empty;
            }
            set
            {
                ViewState["KeepBankId"] = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// Keep 發卡銀行名稱
        /// </summary>
        public string KeepBankName
        {
            get
            {
                return ViewState["KeepBankName"] as string ?? String.Empty;
            }
            set
            {
                ViewState["KeepBankName"] = value == null ? String.Empty : value.Trim();
            }
        }

        #region [MDY:20190218] 因為走中信平台一律直接轉址，此頁只能走財金平台，所以不需要此 Property
        ///// <summary>
        ///// Keep 發卡銀行使用交易平台
        ///// </summary>
        //public string KeepBankApNo
        //{
        //    get
        //    {
        //        #region [MDY:20181116] 因為 checkmarx 會誤判所以做了無聊的轉換
        //        #region [OLD]
        //        //return ViewState["KeepBankApNo"] as string ?? String.Empty;
        //        #endregion

        //        string apno = ViewState["KeepBankApNo"] as string;
        //        switch  (apno)
        //        {
        //            case CCardApCodeTexts.EZPOS:
        //                return CCardApCodeTexts.EZPOS;
        //            case CCardApCodeTexts.CTCB:
        //                return CCardApCodeTexts.CTCB;
        //            default:
        //                return String.Empty;
        //        }
        //        #endregion
        //    }
        //    set
        //    {
        //        ViewState["KeepBankApNo"] = value == null ? String.Empty : value.Trim();
        //    }
        //}
        #endregion

        /// <summary>
        /// Keep 持卡人身分證字號
        /// </summary>
        public string KeepPayerId
        {
            get
            {
                return ViewState["KeepPayerId"] as string ?? String.Empty;
            }
            set
            {
                ViewState["KeepPayerId"] = value == null ? String.Empty : value.Trim();
            }
        }


        /// <summary>
        /// Keep 應繳金額
        /// </summary>
        public decimal KeepReceiveAmount
        {
            get
            {
                object value = ViewState["KeepReceiveAmount"];
                if (value is decimal)
                {
                    return (decimal)value;
                }
                return 0M;
            }
            set
            {
                ViewState["KeepReceiveAmount"] = value;
            }
        }

        /// <summary>
        /// Keep 學生身份證字號
        /// </summary>
        public string KeepStudentPId
        {
            get
            {
                return ViewState["KeepStudentPId"] as string ?? String.Empty;
            }
            set
            {
                ViewState["KeepStudentPId"] = value;
            }
        }

        /// <summary>
        /// Keep 學生學號
        /// </summary>
        public string KeepStudentNo
        {
            get
            {
                return ViewState["KeepStudentNo"] as string ?? String.Empty;
            }
            set
            {
                ViewState["KeepStudentNo"] = value;
            }
        }


        /// <summary>
        /// Keep 學校財金特店代碼參數
        /// </summary>
        public string KeepMerchantId
        {
            get
            {
                return ViewState["KeepMerchantId"] as string ?? String.Empty;
            }
            set
            {
                ViewState["KeepMerchantId"] = value;
            }
        }

        /// <summary>
        /// Keep 學校財金端末機代號參數
        /// </summary>
        public string KeepTerminalId
        {
            get
            {
                return ViewState["KeepTerminalId"] as string ?? String.Empty;
            }
            set
            {
                ViewState["KeepTerminalId"] = value;
            }
        }

        /// <summary>
        /// Keep 學校財金特店編號參數
        /// </summary>
        public string KeepMerId
        {
            get
            {
                return ViewState["KeepMerId"] as string ?? String.Empty;
            }
            set
            {
                ViewState["KeepMerId"] = value;
            }
        }

        #region [MDY:20191214] (2019擴充案) 紀錄 StudentReceive 的 PKey
        /// <summary>
        /// Keep 繳費參數
        /// </summary>
        public KeyValueList<string> KeepPayArgs
        {
            get
            {
                return ViewState["KeepPayArgs"] as KeyValueList<string>;
            }
            private set
            {
                ViewState["KeepPayArgs"] = value;
            }
        }
        #endregion
        #endregion


        private string InitialUI()
        {
            creditcard prePage = this.PreviousPage as creditcard;
            if (prePage == null)
            {
                return "請按流程操作";
            }

            #region Check CnacnelNo
            string cancelNo = prePage.KeepCancelNo;
            this.KeepCancelNo = cancelNo;
            if (String.IsNullOrEmpty(cancelNo))
            {
                return "網頁參數錯誤";
            }
            #endregion

            #region Check Key
            string[] keys = prePage.KeepKeys;
            string receiveType = null;
            string yearId = null;
            string termId = null;
            string depId = null;
            string receiveId = null;
            string stuId = null;
            int oldSeq = -1;
            if (keys != null && keys.Length == 7)
            {
                this.KeepKeys = keys;
                receiveType = keys[0].Trim();
                yearId = keys[1].Trim();
                termId = keys[2].Trim();
                depId = keys[3].Trim();
                receiveId = keys[4].Trim();
                stuId = keys[5].Trim();
                if (!Int32.TryParse(keys[6].Trim(), out oldSeq) || oldSeq < 0)
                {
                    return "網頁參數錯誤";
                }
            }
            else if (keys != null && keys.Length > 0)
            {
                return "網頁參數錯誤";
            }
            #endregion

            #region 信用卡繳費

            #region 信用卡繳費使用的參數
            {
                #region 發卡銀行
                string bankId = prePage.KeepBankId;
                this.KeepBankId = bankId;
                if (String.IsNullOrEmpty(bankId))
                {
                    return "網頁參數錯誤";
                }
                this.KeepBankName = prePage.KeepBankName;

                #region [MDY:20210401] 原碼修正
                this.txtBank.Text = HttpUtility.HtmlEncode(this.KeepBankName);
                #endregion
                #endregion

                #region 持卡人身分證字號
                string payerId = prePage.KeepPayerId;
                this.KeepPayerId = payerId;
                if (String.IsNullOrEmpty(payerId))
                {
                    return "網頁參數錯誤";
                }

                #region [MDY:20210401] 原碼修正
                this.txtPayerId.Text = HttpUtility.HtmlEncode(payerId);
                #endregion
                #endregion

                this.ccbtnOK.PostBackUrl = String.Empty;
            }
            #endregion

            #region 取得發卡銀行使用的繳費平台代碼
            #region [MDY:2018xxxx] 因為前頁走中信平台一律直接轉址，所以這頁只能走財金平台
            #region [OLD]
            //string bankApNo = null;
            //{
            //    CCardBankIdDtlEntity bank = null;
            //    Expression where = new Expression(CCardBankIdDtlEntity.Field.BankId, this.KeepBankId);
            //    XmlResult xmlResult = DataProxy.Current.SelectFirst<CCardBankIdDtlEntity>(this.Page, where, null, out bank);
            //    if (!xmlResult.IsSuccess || bank == null)
            //    {
            //        return "無法取得該發卡銀行使用的繳費平台";
            //    }
            //    else
            //    {
            //        bankApNo = bank.ApNo.ToString();
            //        this.KeepBankApNo = bankApNo;
            //    }
            //}
            #endregion

            string bankApNo = CCardApCodeTexts.EZPOS;
            #endregion
            #endregion

            #endregion

            string errmsg = this.GetAndBindBillData(cancelNo, receiveType, yearId, termId, depId, receiveId, stuId, oldSeq, bankApNo);
            if (!String.IsNullOrEmpty(errmsg))
            {
                return errmsg;
            }

            return null;
        }

        private string GetAndBindBillData(string cancelNo, string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int oldSeq, string bankApNo)
        {
            StudentReceiveView studentReceive = null;

            #region 虛擬帳號 + 未繳條件 + 金額大於 0 (View 已做 NULL 轉空字串 處理)
            Expression where = new Expression(StudentReceiveView.Field.ReceiveType, cancelNo.Substring(0, 4))
                .And(StudentReceiveView.Field.CancelNo, cancelNo)
                .And(new Expression(StudentReceiveView.Field.ReceiveWay, String.Empty).Or(StudentReceiveView.Field.ReceiveWay, null))
                //.And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.NotEqual, null)
                .And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);
            //Expression where = new Expression(StudentReceiveView.Field.CancelNo, cancelNo)
            //    .And(StudentReceiveView.Field.ReceiveWay, String.Empty)
            //    .And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.NotEqual, null)
            //    .And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);
            #endregion

            #region 指定明確的 PKey
            if (!String.IsNullOrEmpty(receiveType))
            {
                where.And(StudentReceiveView.Field.ReceiveType, receiveType)
                    .And(StudentReceiveView.Field.YearId, yearId)
                    .And(StudentReceiveView.Field.TermId, termId)
                    .And(StudentReceiveView.Field.DepId, depId)
                    .And(StudentReceiveView.Field.ReceiveId, receiveId)
                    .And(StudentReceiveView.Field.StuId, stuId)
                    .And(StudentReceiveView.Field.OldSeq, oldSeq);
            }
            #endregion

            #region [Old] 信用卡繳款期限 (View 已做 NULL 轉空字串 處理)
            //Expression w1 = null;
            //string twd7Today = Common.GetTWDate7();
            //switch (bankApNo)
            //{
            //    case CCardApCodeTexts.CTCB:
            //        w1 = new Expression(StudentReceiveView3.Field.PayDueDate2, String.Empty).Or(StudentReceiveView3.Field.PayDueDate2,  RelationEnum.GreaterEqual, twd7Today);
            //        break;
            //    default:
            //        w1 = new Expression(StudentReceiveView3.Field.PayDueDate3, String.Empty).Or(StudentReceiveView3.Field.PayDueDate3,  RelationEnum.GreaterEqual, twd7Today);
            //        break;
            //}
            //where.And(w1);
            #endregion

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
            orderbys.Add(StudentReceiveView.Field.YearId, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveView.Field.TermId, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveView.Field.ReceiveType, OrderByEnum.Asc);
            orderbys.Add(StudentReceiveView.Field.CreateDate, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveView.Field.OldSeq, OrderByEnum.Desc);

            DateTime? payDueDate = null;    //繳費期限

            XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveView>(this.Page, where, orderbys, out studentReceive);
            if (xmlResult.IsSuccess)
            {
                if (studentReceive == null)
                {
                    return "查無指定的繳款資料";
                }

                #region 繳費期限
                switch (bankApNo)
                {
                    case CCardApCodeTexts.CTCB:
                        payDueDate = DataFormat.ConvertDateText(studentReceive.PayDueDate2);   //信用卡繳款期限
                        break;
                    default:
                        payDueDate = DataFormat.ConvertDateText(studentReceive.PayDueDate3);   //財金繳款期限
                        break;
                }
                #endregion

                StudentMasterEntity student = null;
                Expression where2 = new Expression(StudentMasterEntity.Field.ReceiveType, studentReceive.ReceiveType)
                    .And(StudentMasterEntity.Field.DepId, studentReceive.DepId)
                    .And(StudentMasterEntity.Field.Id, studentReceive.StuId);
                xmlResult = DataProxy.Current.SelectFirst<StudentMasterEntity>(this.Page, where2, null, out student);
                if (xmlResult.IsSuccess)
                {
                    if (student == null)
                    {
                        return "查無指定的學生資料";
                    }

                    SchoolRTypeEntity school = null;
                    Expression where3 = new Expression(SchoolRTypeEntity.Field.ReceiveType, studentReceive.ReceiveType);
                    xmlResult = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this.Page, where3, null, out school);
                    if (xmlResult.IsSuccess)
                    {
                        this.txtSchool.Text = school.SchName;

                        #region [Old] 姓名不遮了
                        //this.txtStudent.Text = DataFormat.MaskText(student.Name, Entities.DataFormat.MaskDataType.Name);
                        #endregion

                        this.txtStudent.Text = student.Name;

                        #region [MDY:20210401] 原碼修正
                        this.txtCancelNo.Text = HttpUtility.HtmlEncode(cancelNo);
                        #endregion

                        this.txtAmount.Text = DataFormat.GetAmountText(studentReceive.ReceiveAmount);

                        this.KeepReceiveAmount = studentReceive.ReceiveAmount.Value;
                        this.KeepStudentPId = student.IdNumber;
                        this.KeepStudentNo = studentReceive.StuId;

                        #region 財金參數
                        this.KeepMerchantId = school.MerchantId;
                        this.KeepTerminalId = school.TerminalId;
                        this.KeepMerId = school.MerId;
                        if (bankApNo == CCardApCodeTexts.EZPOS
                            && (String.IsNullOrWhiteSpace(school.MerchantId) || String.IsNullOrWhiteSpace(school.TerminalId) || String.IsNullOrWhiteSpace(school.MerId)))
                        {
                            return "此學校未參加該發卡行的信用卡繳費(EZPos管道)";
                        }
                        #endregion

                        #region [MDY:20191214] (2019擴充案) 紀錄 StudentReceive 的 PKey
                        if (bankApNo == CCardApCodeTexts.EZPOS)
                        {
                            KeyValueList<string> args = new KeyValueList<string>();
                            args.Add("StudentReceive.ReceiveType", studentReceive.ReceiveType);
                            args.Add("StudentReceive.YearId", studentReceive.YearId);
                            args.Add("StudentReceive.TermId", studentReceive.TermId);
                            args.Add("StudentReceive.DepId", studentReceive.DepId);
                            args.Add("StudentReceive.ReceiveId", studentReceive.ReceiveId);
                            args.Add("StudentReceive.StuId", studentReceive.StuId);
                            args.Add("StudentReceive.OldSeq", studentReceive.OldSeq.ToString());

                            args.Add("StudentReceive.CancelNo", studentReceive.CancelNo);
                            args.Add("StudentReceive.ReceiveAmount", studentReceive.ReceiveAmount.Value.ToString("0"));  //只能是整數
                            args.Add("StudentReceive.TermName", studentReceive.TermName);
                            args.Add("StudentReceive.ReceiveName", studentReceive.ReceiveName);

                            args.Add("StudentMaster.Name", student.Name);
                            args.Add("StudentMaster.IdNumber", student.IdNumber);
                            args.Add("SchoolRType.SchName", school.SchName);

                            this.KeepPayArgs = args;
                        }
                        #endregion
                    }
                }
            }
            if (!xmlResult.IsSuccess)
            {
                return "查詢指定繳款資料失敗";
            }

            if (payDueDate != null && payDueDate < DateTime.Today)
            {
                return "已超過信用卡繳費期限";
            }
            return null;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string errmsg = this.InitialUI();
                if (!String.IsNullOrEmpty(errmsg))
                {
                    this.ccbtnOK.Visible = false;
                    this.labErrMsg.Text = errmsg;
                }
                else
                {
                    this.ccbtnOK.Visible = true;
                    this.labErrMsg.Text = String.Empty;
                }
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region [MDY:20190218] 因為走中信平台一律直接轉址，所以此頁只能走財金平台
            #region [OLD]
            //if (this.KeepBankApNo == CCardApCodeTexts.EZPOS)
            //{
            //    this.Server.Transfer("EZPosEntry.aspx", true);
            //}
            //else if (this.KeepBankApNo == CCardApCodeTexts.CTCB)
            //{
            //    //this.ShowJsAlert("該發卡銀行使用的繳費平台(中國信托)未開放");
            //    //this.Server.Transfer("https://www.27608818.com/tuipaymt/index.jsp", true);
            //    this.Response.Redirect("https://www.27608818.com/tuipaymt/index.jsp", true);
            //}
            //else
            //{
            //    this.ShowJsAlert("無法判斷該發卡銀行使用的繳費平台");
            //}
            #endregion

            if (String.IsNullOrWhiteSpace(this.KeepMerchantId) || String.IsNullOrWhiteSpace(this.KeepTerminalId) || String.IsNullOrWhiteSpace(this.KeepMerId))
            {
                string msg = this.GetLocalized("此學校未參加財金管道的信用卡繳費");
                this.ShowJsAlert(msg);
            }
            else
            {
                #region [MDY:20210521] 原碼修正
                this.Server.Transfer(WebHelper.GenRNUrl("EZPosEntry.aspx"), true);
                #endregion
            }
            #endregion
        }
    }
}