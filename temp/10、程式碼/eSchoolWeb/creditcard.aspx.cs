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
    public partial class creditcard : LocalizedPage //System.Web.UI.Page, IMenuPage
    {
        #region Implement IMenuPage
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
        #endregion

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
                WebHelper.SetDropDownListItems(this.ddlBank, DefaultItem.Kind.None, false, datas, false, false, 0, null);
            }
            else
            {
                WebHelper.SetDropDownListItems(this.ddlBank, DefaultItem.Kind.Select, false, datas, false, false, 0, null);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UCPageNews.PageId = BoardTypeCodeTexts.CREDITCARD;
            if (!IsPostBack)
            {
                string apNo = HttpUtility.HtmlEncode(this.Request.QueryString["apno"]);
                this.GetAndBindBankOption(apNo);

                this.KeepCancelNo = this.Request.QueryString["cno"];
                string cancelNo = this.KeepCancelNo;
                if (!String.IsNullOrEmpty(cancelNo))
                {
                    #region [MDY:20210401] 原碼修正
                    this.txtCancelNo.Text = HttpUtility.HtmlEncode(cancelNo);
                    #endregion

                    string key = this.Request.QueryString["key"];
                    if (!String.IsNullOrWhiteSpace(key))
                    {
                        key = Common.GetBase64Decode(key.Trim());
                        string[] keys = key.Split(new char[] { '_' }, StringSplitOptions.None);
                        if (keys.Length == 7)
                        {
                            this.txtCancelNo.Enabled = false;
                            this.KeepKeys = keys;
                        }
                    }
                }
            }
        }

        #region [MDY:2018xxxx] 先依據原邏輯判斷，無法繳費再判斷另一個繳費平台是否能繳費
        #region [OLD]
        //protected void ccbtnOK_Click(object sender, EventArgs e)
        //{
        //    string pid = this.txtPID.Text.Trim();
        //    string bank_id = this.ddlBank.SelectedValue.Trim();
        //    string cancel_no = this.txtCancelNo.Text.Trim();
        //    string validate_number = this.txtValidateNum.Text.Trim();
        //    string real_validate_number = (string)Session["ValidateNum"];

        //    #region 檢查圖形驗證碼
        //    if (validate_number.ToLower() != real_validate_number.ToLower())
        //    {
        //        this.txtValidateNum.Text = "";

        //        StringBuilder js = new StringBuilder();
        //        js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("驗證碼錯誤，請重新輸入.")).AppendLine();

        //        ClientScriptManager cs = this.ClientScript;
        //        Type myType = this.GetType();
        //        if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
        //        {
        //            cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
        //        }
        //        return;
        //    }
        //    #endregion

        //    #region 檢查發卡行
        //    if (String.IsNullOrEmpty(bank_id))
        //    {
        //        this.ShowJsAlert("請選擇發卡銀行");
        //        return;
        //    }
        //    else
        //    {
        //        string apNo = bank_id.Substring(0, 1);
        //        string bankId = bank_id.Substring(2);
        //        if (apNo == CCardApCodeTexts.CTCB)
        //        {
        //            if (this.txtCancelNo.Enabled)     //表示不是由學生專區過來的
        //            {
        //                #region 檢查銷帳編號
        //                if (cancel_no.Length == 0)
        //                {
        //                    this.ShowJsAlert("請輸入虛擬帳號");
        //                    return;
        //                }
        //                if (!Common.IsNumber(cancel_no, 14, 16))
        //                {
        //                    //[TODO] 固定顯示訊息的收集
        //                    this.ShowJsAlert("虛擬帳號限輸入14  ~ 16 碼數字");
        //                    return;
        //                }
        //                this.KeepCancelNo = cancel_no;
        //                #endregion

        //                #region 檢查帳單
        //                {
        //                    StudentReceiveView studentReceive = null;

        //                    //虛擬帳號 + 未繳 + 金額大於 0
        //                    Expression where = new Expression(StudentReceiveView.Field.ReceiveType, cancel_no.Substring(0, 4))
        //                        .And(StudentReceiveView.Field.CancelNo, cancel_no)
        //                        .And(new Expression(StudentReceiveView.Field.ReceiveWay, String.Empty).Or(StudentReceiveView.Field.ReceiveWay, null))
        //                        //.And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.NotEqual, null)
        //                        .And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);


        //                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
        //                    orderbys.Add(StudentReceiveView.Field.YearId, OrderByEnum.Desc);
        //                    orderbys.Add(StudentReceiveView.Field.TermId, OrderByEnum.Desc);
        //                    orderbys.Add(StudentReceiveView.Field.ReceiveType, OrderByEnum.Asc);
        //                    orderbys.Add(StudentReceiveView.Field.CreateDate, OrderByEnum.Desc);
        //                    orderbys.Add(StudentReceiveView.Field.OldSeq, OrderByEnum.Desc);

        //                    XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveView>(this.Page, where, orderbys, out studentReceive);
        //                    if (xmlResult.IsSuccess)
        //                    {
        //                        if (studentReceive == null)
        //                        {
        //                            this.ShowJsAlert("查無指定的繳款資料");
        //                            return;
        //                        }

        //                        DateTime? payDueDate = DataFormat.ConvertDateText(studentReceive.PayDueDate2);   //信用卡繳款期限
        //                        if (payDueDate != null && payDueDate < DateTime.Today)
        //                        {
        //                            this.ShowJsAlert("已超過中國信託平台的繳費期限");
        //                            return;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this.ShowJsAlert("查詢繳款資料失敗");
        //                        return;
        //                    }
        //                }
        //                #endregion
        //            }

        //            Response.Redirect("https://www.27608818.com/tuipaymt/index.jsp", true);
        //            return;
        //        }
        //        this.KeepBankId = bankId;
        //        this.KeepBankName = this.ddlBank.Items.FindByValue(bank_id).Text;
        //    }
        //    #endregion

        //    #region 檢查身分證
        //    if (String.IsNullOrEmpty(pid))
        //    {
        //        this.ShowJsAlert("請輸入持卡人身分證字號");
        //        return;
        //    }
        //    this.KeepPayerId = pid;
        //    #endregion

        //    #region 檢查銷帳編號
        //    if (cancel_no.Length == 0)
        //    {
        //        this.ShowJsAlert("請輸入虛擬帳號");
        //        return;
        //    }
        //    if (!Common.IsNumber(cancel_no, 14, 16))
        //    {
        //        //[TODO] 固定顯示訊息的收集
        //        this.ShowJsAlert("虛擬帳號限輸入14  ~ 16 碼數字");
        //        return;
        //    }
        //    this.KeepCancelNo = cancel_no;
        //    #endregion

        //    string url = "creditcard_d.aspx";
        //    this.Server.Transfer(url, true);
        //}
        #endregion

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region [MDY:20191023] M201910_02 修正圖形驗證碼弱點
            #region [OLD]
            //#region 檢查圖形驗證碼
            //{
            //    //string inputValidate = this.txtValidateNum.Text.Trim();
            //    //string checkValidate = Session["ValidateNum"] as string;
            //    //if (!inputValidate.Equals(checkValidate, StringComparison.CurrentCultureIgnoreCase))
            //    //{
            //    //    this.txtValidateNum.Text = String.Empty;
            //    //    this.ShowJsAlert("驗證碼錯誤，請重新輸入.");
            //    //    return;
            //    //}
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

            #region 檢查銷帳編號
            string cancelNo = this.KeepCancelNo = this.txtCancelNo.Text;

            #region [MDY:20191023] M201910_02 統一虛擬帳號檢查處理
            #region [OLD]
            //if (cancelNo.Length == 0)
            //{
            //    this.ShowJsAlert("請輸入虛擬帳號");
            //    return;
            //}
            //if (!Common.IsNumber(cancelNo, 14, 16))
            //{
            //    this.ShowJsAlert("虛擬帳號限輸入14  ~ 16 碼數字");
            //    return;
            //}
            #endregion

            if (String.IsNullOrEmpty(cancelNo))
            {
                this.ShowMustInputAlert("虛擬帳號");
                return;
            }
            if (!Common.IsNumber(cancelNo) && cancelNo.Length != 16 && cancelNo.Length != 14)
            {
                this.ShowJsAlert("輸入的虛擬帳號有誤，請重新輸入");
                return;
            }
            #endregion

            string receiveType = cancelNo.Substring(0, 4);
            #endregion

            string apNo = null;    //繳費平台代碼
            string bankId = null;  //發卡行代碼

            #region 檢查發卡行
            {
                string bank = this.ddlBank.SelectedValue.Trim();
                if (String.IsNullOrEmpty(bank))
                {
                    this.ShowJsAlert("請選擇發卡銀行");
                    return;
                }
                if (bank.Length != 5)
                {
                    this.ShowJsAlert("無法取得選擇的發卡銀行");
                    return;
                }
                apNo = bank.Substring(0, 1);
                if (apNo != CCardApCodeTexts.CTCB && apNo != CCardApCodeTexts.EZPOS)
                {
                    this.ShowJsAlert("無法取得該發卡銀行使用的繳費平台");
                    return;
                }
                bankId = bank.Substring(2);
                this.KeepBankId = bankId;
                this.KeepBankName = this.ddlBank.Items.FindByValue(bank).Text;
            }
            #endregion

            #region 取得帳單資料
            DateTime? payDueDateCTCB = null;
            DateTime? payDueDateEZPOS = null;
            {
                //虛擬帳號 + 未繳 + 金額大於 0
                Expression where = new Expression(StudentReceiveView.Field.ReceiveType, receiveType)
                    .And(StudentReceiveView.Field.CancelNo, cancelNo)
                    .And(new Expression(StudentReceiveView.Field.ReceiveWay, String.Empty).Or(StudentReceiveView.Field.ReceiveWay, null))
                    .And(new Expression(StudentReceiveView.Field.ReceiveDate, String.Empty).Or(StudentReceiveView.Field.ReceiveDate, null))
                    .And(new Expression(StudentReceiveView.Field.AccountDate, String.Empty).Or(StudentReceiveView.Field.AccountDate, null))
                    .And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);

                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
                orderbys.Add(StudentReceiveView.Field.YearId, OrderByEnum.Desc);
                orderbys.Add(StudentReceiveView.Field.TermId, OrderByEnum.Desc);
                orderbys.Add(StudentReceiveView.Field.ReceiveType, OrderByEnum.Asc);
                orderbys.Add(StudentReceiveView.Field.CreateDate, OrderByEnum.Desc);
                orderbys.Add(StudentReceiveView.Field.OldSeq, OrderByEnum.Desc);

                StudentReceiveView studentReceive = null;
                XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveView>(this.Page, where, orderbys, out studentReceive);
                if (xmlResult.IsSuccess)
                {
                    if (studentReceive == null)
                    {
                        this.ShowJsAlert(this.GetLocalized("查無指定的繳款資料"));
                        return;
                    }

                    #region [MDY:20191214] (2019擴充案) 國際信用卡 - 啟用國際信用卡繳費則不允許使用此管道
                    if ("Y".Equals(studentReceive.NCCardFlag))
                    {
                        this.ShowJsAlert(this.GetLocalized("此繳費單限定使用國際信用卡繳費"));
                        return;
                    }
                    #endregion

                    payDueDateCTCB = DataFormat.ConvertDateText(studentReceive.PayDueDate2);    //中信平台信用卡繳款期限
                    payDueDateEZPOS = DataFormat.ConvertDateText(studentReceive.PayDueDate3);   //財金平台信用卡繳款期限
                }
                else
                {
                    this.ShowJsAlert(this.GetLocalized("查詢繳款資料失敗"));
                    return;
                }
            }
            #endregion

            #region 取得是否有中信管道
            bool hasCTCBChannel = false;
            {
                Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType)
                    .And(ReceiveChannelEntity.Field.ChannelId, ChannelHelper.CTCB);

                int count = 0;
                XmlResult xmlResult = DataProxy.Current.SelectCount<ReceiveChannelEntity>(this.Page, where, out count);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowJsAlert("查詢學校代收管道設定失敗");
                    return;
                }
                hasCTCBChannel = (count > 0);
            }
            #endregion

            #region 取得是否有設定財金參數
            bool hasEZPOSSetting = false;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType)
                    .And(SchoolRTypeEntity.Field.MerchantId, RelationEnum.NotEqual, String.Empty)
                    .And(SchoolRTypeEntity.Field.MerchantId, RelationEnum.NotEqual, null)
                    .And(SchoolRTypeEntity.Field.TerminalId, RelationEnum.NotEqual, String.Empty)
                    .And(SchoolRTypeEntity.Field.TerminalId, RelationEnum.NotEqual, null)
                    .And(SchoolRTypeEntity.Field.MerId, RelationEnum.NotEqual, String.Empty)
                    .And(SchoolRTypeEntity.Field.MerId, RelationEnum.NotEqual, null);

                int count = 0;
                XmlResult xmlResult = DataProxy.Current.SelectCount<SchoolRTypeEntity>(this.Page, where, out count);
                if (xmlResult.IsSuccess)
                {
                    hasEZPOSSetting = (count > 0);
                }
                else
                {
                    this.ShowJsAlert("查詢學校財金參數資料失敗");
                    return;
                }
            }
            #endregion

            #region 先依據發卡行的繳費平台，檢查是否可繳費
            if (apNo == CCardApCodeTexts.CTCB)  //中信平台
            {
                if (!hasCTCBChannel || (payDueDateCTCB.HasValue && payDueDateCTCB < DateTime.Today))
                {
                    if (hasEZPOSSetting && (!payDueDateEZPOS.HasValue || payDueDateEZPOS.Value >= DateTime.Today))
                    {
                        apNo = CCardApCodeTexts.EZPOS;  //改走財金平台
                    }
                    else
                    {
                        this.ShowJsAlert("已超過中國信託平台的繳費期限");
                        return;
                    }
                }
            }
            else if (apNo == CCardApCodeTexts.EZPOS)  //財金平台
            {
                if (!hasEZPOSSetting || (payDueDateEZPOS.HasValue && payDueDateEZPOS.Value < DateTime.Today))
                {
                    if (!payDueDateCTCB.HasValue || payDueDateCTCB >= DateTime.Today)
                    {
                        apNo = CCardApCodeTexts.CTCB;   //改走中信平台
                    }
                    else
                    {
                        this.ShowJsAlert("已超過財金平台的繳費期限");
                        return;
                    }
                }
            }
            else
            {
                this.ShowJsAlert("無法取得該發卡銀行使用的繳費平台");
                return;
            }
            #endregion

            if (apNo == CCardApCodeTexts.CTCB)
            {
                #region 如果是走中信平台，直接轉中信網站
                #region [MDY:20210823] 中信 i 繳費網址改成 https://www.27608818.com/web/
                #region [MDY:20210521] 原碼修正
                Response.Redirect(WebHelper.GenRNUrl("https://www.27608818.com/web/"), true);
                #endregion
                #endregion

                return;
                #endregion
            }
            else
            {
                #region 檢查身分證
                {
                    string pid = this.KeepPayerId = this.txtPID.Text;
                    if (String.IsNullOrEmpty(pid))
                    {
                        this.ShowJsAlert("請輸入持卡人身分證字號");
                        return;
                    }
                }
                #endregion

                string url = "creditcard_d.aspx";

                #region [MDY:20210521] 原碼修正
                this.Server.Transfer(WebHelper.GenRNUrl(url), true);
                #endregion
            }
        }
        #endregion
    }
}