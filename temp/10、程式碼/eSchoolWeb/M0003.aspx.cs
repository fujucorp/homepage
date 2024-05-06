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
    public partial class M0003 : LocalizedPage, ICreditCardPage
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
        /// Keep 發卡銀行使用交易平台
        /// </summary>
        public string KeepBankApNo
        {
            get
            {
                return ViewState["KeepBankApNo"] as string ?? String.Empty;
            }
            set
            {
                ViewState["KeepBankApNo"] = value == null ? String.Empty : value.Trim();
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

        #region [MDY:20210706] FIX BUG (2019擴充案) 紀錄 StudentReceive 的 PKey
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

        private void InitialUI(string apNo)
        {
            this.tbxS1PID.Text = String.Empty;
            this.tbxS1CancelNo.Text = String.Empty;

            this.GetAndBindBankOption(apNo);

            this.tbxS2School.Text = String.Empty;
            this.tbxS2Student.Text = String.Empty;
            this.tbxS2CancelNo.Text = String.Empty;
            this.tbxS2Amount.Text = String.Empty;
            this.tbxS2Bank.Text = String.Empty;
            this.tbxS2PayerId.Text = String.Empty;

            this.tabStep1.Visible = true;
            this.tabStep2.Visible = false;
        }

        private void GetAndBindBankOption(string apNo)
        {
            Expression where = new Expression();
            if (apNo == CCardApCodeTexts.CTCB || apNo == CCardApCodeTexts.EZPOS)
            {
                where = new Expression(CCardBankIdDtlEntity.Field.ApNo, apNo);
            }
            else
            {
                where = new Expression();
            }

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(CCardBankIdDtlEntity.Field.BankId, OrderByEnum.Asc);

            string[] codeFieldNames = new string[] { CCardBankIdDtlEntity.Field.ApNo, CCardBankIdDtlEntity.Field.BankId };
            string codeCombineFormat = "{0}-{1}";
            string[] textFieldNames = new string[] { CCardBankIdDtlEntity.Field.BankId, CCardBankIdDtlEntity.Field.BankName };
            string textCombineFormat = "{0} - {1}";

            CodeText[] datas = null;
            XmlResult xmlResult = DataProxy.Current.GetEntityOptions<CCardBankIdDtlEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
            if (!xmlResult.IsSuccess)
            {
                #region [MDY:20191023] M201910_02 統一改用 ShowJsAlert() 方法顯示錯誤
                #region [OLD]
                //StringBuilder js = new StringBuilder();
                //js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("無法取得發卡銀行選項資料")).AppendLine();

                //ClientScriptManager cs = this.ClientScript;
                //Type myType = this.GetType();
                //if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                //{
                //    cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                //}
                #endregion

                this.ShowJsAlert("無法取得發卡銀行選項資料");
                #endregion
            }
            if (datas == null || datas.Length == 1)
            {
                WebHelper.SetDropDownListItems(this.ddlS1Bank, DefaultItem.Kind.None, false, datas, false, false, 0, null);
            }
            else
            {
                WebHelper.SetDropDownListItems(this.ddlS1Bank, DefaultItem.Kind.Select, false, datas, false, false, 0, null);
            }
        }

        private void ShowStep2()
        {
            #region 虛擬帳號
            string cancelNo = this.KeepCancelNo;

            #region [MDY:20210401] 原碼修正
            this.tbxS2CancelNo.Text = HttpUtility.HtmlEncode(cancelNo);
            #endregion
            #endregion

            #region 6 Key
            string receiveType = null;
            string yearId = null;
            string termId = null;
            string depId = null;
            string receiveId = null;
            string stuId = null;
            string[] keys = this.KeepKeys;
            if (keys != null && keys.Length == 6)
            {
                receiveType = keys[0].Trim();
                yearId = keys[1].Trim();
                termId = keys[2].Trim();
                depId = keys[3].Trim();
                receiveId = keys[4].Trim();
                stuId = keys[5].Trim();
            }
            #endregion

            #region 發卡銀行
            string bankApNo = this.KeepBankApNo;
            string bankId = this.KeepBankId;

            #region [MDY:20210401] 原碼修正
            this.tbxS2Bank.Text = HttpUtility.HtmlEncode(this.KeepBankName);
            #endregion
            #endregion

            #region 持卡人身分證字號
            string payerId = this.KeepPayerId;

            #region [MDY:20210401] 原碼修正
            this.tbxS2PayerId.Text = HttpUtility.HtmlEncode(payerId);
            #endregion
            #endregion

            string errmsg = this.GetAndBindBillData(cancelNo, receiveType, yearId, termId, depId, receiveId, stuId, bankApNo);
            if (!String.IsNullOrEmpty(errmsg))
            {
                this.ccbtnPay.Visible = false;
                this.labErrMsg.Text = errmsg;
            }
            else
            {
                this.ccbtnPay.Visible = true;
            }

            this.tabStep1.Visible = false;
            this.tabStep2.Visible = true;
        }

        private void ShowStep1()
        {
            this.tabStep1.Visible = true;
            this.tabStep2.Visible = false;
        }

        private string GetAndBindBillData(string cancelNo, string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, string bankApNo)
        {
            #region [MDY:20210706] FIX BUG (2019擴充案) 不用 StudentReceiveView3 改用 StudentReceiveView
            StudentReceiveView studentReceive = null;

            #region 虛擬帳號 + 未繳條件 + 金額大於 0 (View 已做 NULL 轉空字串 處理)
            Expression where = new Expression(StudentReceiveView.Field.ReceiveType, cancelNo.Substring(0, 4))
                .And(StudentReceiveView.Field.CancelNo, cancelNo)
                .And(new Expression(StudentReceiveView.Field.ReceiveWay, String.Empty).Or(StudentReceiveView.Field.ReceiveWay, null))
                .And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);
            #endregion

            #region 指定明確的 PKey
            if (!String.IsNullOrEmpty(receiveType))
            {
                where.And(StudentReceiveView.Field.ReceiveType, receiveType)
                    .And(StudentReceiveView.Field.YearId, yearId)
                    .And(StudentReceiveView.Field.TermId, termId)
                    .And(StudentReceiveView.Field.DepId, depId)
                    .And(StudentReceiveView.Field.ReceiveId, receiveId)
                    .And(StudentReceiveView.Field.StuId, stuId);
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

            DateTime? payDueDate = null;    //繳費期限
            XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveView>(this.Page, where, null, out studentReceive);
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
                        this.tbxS2School.Text = school.SchName;

                        #region [Old] 姓名不遮了
                        //this.tbxS2Student.Text = DataFormat.MaskText(student.Name, Entities.DataFormat.MaskDataType.Name);
                        #endregion

                        this.tbxS2Student.Text = student.Name;

                        #region [MDY:20210401] 原碼修正
                        this.tbxS2CancelNo.Text = HttpUtility.HtmlEncode(cancelNo);
                        #endregion
                        this.tbxS2Amount.Text = DataFormat.GetAmountText(studentReceive.ReceiveAmount);

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

                        #region [MDY:20210706] FIX BUG (2019擴充案) 紀錄 StudentReceive 的 PKey
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
            #endregion

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
            if (!IsPostBack)
            {
                string apNo = HttpUtility.HtmlEncode(this.Request.QueryString["apno"]);
                this.InitialUI(apNo);

                this.KeepCancelNo = this.Request.QueryString["cno"];
                string cancelNo = this.KeepCancelNo;
                if (!String.IsNullOrEmpty(cancelNo))
                {
                    #region [MDY:20210401] 原碼修正
                    this.tbxS1CancelNo.Text = HttpUtility.HtmlEncode(cancelNo);
                    #endregion
                    string key = this.Request.QueryString["key"];
                    if (!String.IsNullOrWhiteSpace(key))
                    {
                        key = Common.GetBase64Decode(key.Trim()).Replace(" ", "");
                        string[] keys = key.Split(new char[] { '_' }, StringSplitOptions.None);
                        if (keys.Length == 7)
                        {
                            this.tbxS1CancelNo.Enabled = false;
                            this.KeepKeys = keys;
                        }
                    }
                }
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string pid = this.tbxS1PID.Text.Trim();
            string bank_id = this.ddlS1Bank.SelectedValue.Trim();

            #region [MDY:20191023] M201910_02 修正圖形驗證碼弱點
            #region [OLD]
            //string validate_number = this.txtValidateNum.Text.Trim();
            //string real_validate_number = (string)Session["ValidateNum"];

            //#region 檢查圖形驗證碼
            //if (validate_number.ToLower() != real_validate_number.ToLower())
            //{
            //    this.txtValidateNum.Text = "";

            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("驗證碼錯誤，請重新輸入.")).AppendLine();

            //    ClientScriptManager cs = this.ClientScript;
            //    Type myType = this.GetType();
            //    if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
            //    {
            //        cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
            //    }
            //    return;
            //}
            //#endregion
            #endregion

            #region 檢查圖形驗證碼
            {
                string validateCode = this.txtValidateCode.Text.Trim();
                this.txtValidateCode.Text = String.Empty;
                if (!(new ValidatePic()).CheckValidateCode(validateCode))
                {
                    this.ShowJsAlert("驗證碼錯誤，請重新輸入");
                    return;
                }
            }
            #endregion
            #endregion

            #region 檢查發卡行
            if (String.IsNullOrEmpty(bank_id))
            {
                this.ShowJsAlert("請選擇發卡銀行");
                return;
            }
            else
            {
                string apNo = bank_id.Substring(0, 1);
                string bankId = bank_id.Substring(2);
                if (apNo == CCardApCodeTexts.CTCB)
                {
                    if (this.tbxS1CancelNo.Enabled)     //表示不是由學生專區過來的
                    {
                        #region 檢查銷帳編號
                        string cancelNo = this.tbxS1CancelNo.Text.Trim();
                        if (cancelNo.Length == 0)
                        {
                            this.ShowJsAlert("請輸入虛擬帳號");
                            return;
                        }
                        if (!Common.IsNumber(cancelNo, 14, 16))
                        {
                            //[TODO] 固定顯示訊息的收集
                            this.ShowJsAlert("虛擬帳號限輸入14  ~ 16 碼數字");
                            return;
                        }
                        this.KeepCancelNo = cancelNo;
                        #endregion

                        #region 檢查帳單
                        {
                            StudentReceiveView studentReceive = null;

                            //虛擬帳號 + 未繳 + 金額大於 0
                            Expression where = new Expression(StudentReceiveView.Field.ReceiveType, cancelNo.Substring(0, 4))
                                .And(StudentReceiveView.Field.CancelNo, cancelNo)
                                .And(new Expression(StudentReceiveView.Field.ReceiveWay, String.Empty).Or(StudentReceiveView.Field.ReceiveWay, null))
                                //.And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.NotEqual, null)
                                .And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);


                            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
                            orderbys.Add(StudentReceiveView.Field.YearId, OrderByEnum.Desc);
                            orderbys.Add(StudentReceiveView.Field.TermId, OrderByEnum.Desc);
                            orderbys.Add(StudentReceiveView.Field.ReceiveType, OrderByEnum.Asc);
                            orderbys.Add(StudentReceiveView.Field.CreateDate, OrderByEnum.Desc);
                            orderbys.Add(StudentReceiveView.Field.OldSeq, OrderByEnum.Desc);

                            XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveView>(this.Page, where, orderbys, out studentReceive);
                            if (xmlResult.IsSuccess)
                            {
                                if (studentReceive == null)
                                {
                                    this.ShowJsAlert("查無指定的繳款資料");
                                    return;
                                }

                                DateTime? payDueDate = DataFormat.ConvertDateText(studentReceive.PayDueDate2);   //信用卡繳款期限
                                if (payDueDate != null && payDueDate < DateTime.Today)
                                {
                                    this.ShowJsAlert("已超過中國信託平台的繳費期限");
                                    return;
                                }
                            }
                            else
                            {
                                this.ShowJsAlert("查詢繳款資料失敗");
                                return;
                            }
                        }
                        #endregion
                    }

                    #region [MDY:20210823] 中信 i 繳費網址改成 https://www.27608818.com/web/
                    #region [MDY:20210521] 原碼修正
                    Response.Redirect(WebHelper.GenRNUrl("https://www.27608818.com/web/"), true);
                    #endregion
                    #endregion
                    return;
                }
                this.KeepBankApNo = apNo;
                this.KeepBankId = bankId;
                this.KeepBankName = this.ddlS1Bank.Items.FindByValue(bank_id).Text;
            }
            #endregion

            #region 檢查身分證
            if (String.IsNullOrEmpty(pid))
            {
                this.ShowJsAlert("請輸入持卡人身分證字號");
                return;
            }
            this.KeepPayerId = pid;
            #endregion

            #region 檢查銷帳編號
            string cancel_no = null;
            if (this.KeepKeys == null || this.KeepKeys.Length != 6)
            {
                cancel_no = this.tbxS1CancelNo.Text.Trim();
                if (cancel_no.Length == 0)
                {
                    this.ShowJsAlert("請輸入虛擬帳號");
                    return;
                }
                if (!Common.IsNumber(cancel_no, 14, 16))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowJsAlert("虛擬帳號限輸入14  ~ 16 碼數字");
                    return;
                }
                this.KeepCancelNo = cancel_no;
            }
            else
            {
                cancel_no = this.KeepCancelNo;
                if (cancel_no.Length == 0)
                {
                    this.ShowJsAlert("網頁參數錯誤，請按流程操作");
                    return;
                }
            }
            #endregion

            this.ShowStep2();
        }

        protected void ccbtnPay_Click(object sender, EventArgs e)
        {
            if (this.KeepBankApNo == CCardApCodeTexts.EZPOS)
            {
                #region [MDY:20210521] 原碼修正
                this.Server.Transfer(WebHelper.GenRNUrl("EZPosEntry.aspx"), true);
                #endregion
            }
            else if (this.KeepBankApNo == CCardApCodeTexts.CTCB)
            {
                //this.ShowJsAlert("該發卡銀行使用的繳費平台(中國信托)未開放");
                //this.Server.Transfer("https://www.27608818.com/tuipaymt/index.jsp", true);

                #region [MDY:20210823] 中信 i 繳費網址改成 https://www.27608818.com/web/
                #region [MDY:20210521] 原碼修正
                this.Response.Redirect(WebHelper.GenRNUrl("https://www.27608818.com/web/"), true);
                #endregion
                #endregion
            }
            else
            {
                this.ShowJsAlert("無法判斷該發卡銀行使用的繳費平台");
            }
        }

        protected void ccbtnGoBack_Click(object sender, EventArgs e)
        {
            this.ShowStep1();
        }
    }
}