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
    /// <summary>
    /// 國際信用卡繳費
    /// </summary>
    public partial class creditcard2 : LocalizedPage
    {
        #region Implement IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "CreditCard2";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "國際信用卡繳費";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.UCPageNews.PageId = BoardTypeCodeTexts.CREDITCARD;

            if (!this.IsPostBack)
            {
                #region 初始化
                {
                    this.divQuery.Visible = true;
                    this.UCPageNews.Visible = this.divQuery.Visible;
                    this.divResult.Visible = !this.divQuery.Visible;
                }
                #endregion

                #region 暫時不提供學生專區連過來的參數
                //string cancelNo = this.KeepCancelNo = this.Request.QueryString["cno"];
                //if (!String.IsNullOrEmpty(cancelNo))
                //{
                //    this.txtCancelNo.Text = cancelNo;
                //    string key = this.Request.QueryString["key"];
                //    if (!String.IsNullOrWhiteSpace(key))
                //    {
                //        key = Common.GetBase64Decode(key.Trim());
                //        string[] keys = key.Split(new char[] { '_' }, StringSplitOptions.None);
                //        if (keys.Length == 7)
                //        {
                //            this.txtCancelNo.Enabled = false;
                //            this.KeepKeys = keys;
                //        }
                //    }
                //}
                #endregion
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
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

            #region 檢查虛擬帳號
            string cancelNo = this.tbxQCancelNo.Text.Trim();
            if (String.IsNullOrEmpty(cancelNo))
            {
                this.ShowJsAlert("請輸入虛擬帳號");
                return;
            }
            if (!Common.IsNumber(cancelNo, 14, 16))
            {
                this.ShowJsAlert("虛擬帳號限輸入14  ~ 16 碼數字");
                return;
            }
            string receiveType = cancelNo.Substring(0, 4);
            #endregion

            #region 取得商家代號資料
            SchoolRTypeEntity school = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType)
                    .And(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this.Page, where, null, out school);
                if (xmlResult.IsSuccess)
                {
                    if (school == null)
                    {
                        this.ShowJsAlert("查無該繳款單的商家代號資料");
                        return;
                    }
                }
                else
                {
                    this.ShowJsAlert("查詢該繳款單的商家代號資料失敗");
                    return;
                }
            }
            #endregion

            #region 檢查財金國際信用卡財金參數
            {
                #region [MEMO]
                //因為土銀的財金信用卡管道是學校與財金各簽，不是由土銀統簽。
                //所以直接用學校自己的財金特店參數來判斷是否有財金信用卡管，
                //而不是判斷 ReceiveChannelEntity 設定
                #endregion

                if (String.IsNullOrWhiteSpace(school.MerId2)
                    || String.IsNullOrWhiteSpace(school.MerchantId2)
                    || String.IsNullOrWhiteSpace(school.TerminalId2))
                {
                    this.ShowJsAlert("該學校未提供國際信用卡繳費管道");
                    return;
                }
            }
            #endregion

            #region 取得帳單資料
            StudentReceiveView studentReceive = null;
            {
                //虛擬帳號 + 可使用國際信用卡繳費 + 未繳 + 金額大於 0
                Expression where = new Expression(StudentReceiveView.Field.ReceiveType, receiveType)
                    .And(StudentReceiveView.Field.CancelNo, cancelNo)
                    .And(StudentReceiveView.Field.NCCardFlag, "Y")
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

                XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveView>(this.Page, where, orderbys, out studentReceive);
                if (xmlResult.IsSuccess)
                {
                    if (studentReceive == null)
                    {
                        this.ShowJsAlert("查無指定的繳款單資料，或該繳款單不可使用國際信用卡繳費");
                        return;
                    }

                    DateTime? payDueDateEZPOS = DataFormat.ConvertDateText(studentReceive.PayDueDate3);   //財金平台信用卡繳款期限

                    #region 檢查繳款期限
                    if (payDueDateEZPOS.HasValue && payDueDateEZPOS.Value < DateTime.Today)
                    {
                        this.ShowJsAlert("該繳款單已超過信用卡繳費期限");
                        return;
                    }
                    #endregion
                }
                else
                {
                    this.ShowJsAlert("查詢繳款單資料失敗");
                    return;
                }
            }
            #endregion

            #region 取得學生基本資料
            StudentMasterEntity student = null;
            {
                Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, studentReceive.ReceiveType)
                    .And(StudentMasterEntity.Field.DepId, studentReceive.DepId)
                    .And(StudentMasterEntity.Field.Id, studentReceive.StuId);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentMasterEntity>(this.Page, where, null, out student);
                if (xmlResult.IsSuccess)
                {
                    if (student == null)
                    {
                        this.ShowJsAlert("查無該繳款單的學生資料");
                        return;
                    }
                }
                else
                {
                    this.ShowJsAlert("查詢該繳款單的學生資料失敗");
                    return;
                }
            }
            #endregion

            #region Keep 繳費參數
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

                args.Add("Focas.ApNo", "4");  //4=財金國際信用卡財金參數; 1=財金國內信用卡財金參數
                args.Add("Focas.MerId", school.MerId2);
                args.Add("Focas.MerchantId", school.MerchantId2);
                args.Add("Focas.TerminalId", school.TerminalId2);

                this.KeepPayArgs = args;
            }
            #endregion

            #region 結繫資料
            {
                this.tbxRSchoolName.Text = school.SchName;
                this.tbxRStudentName.Text = student.Name;
                this.tbxRYearId.Text = studentReceive.YearId;
                this.tbxRTermName.Text = studentReceive.TermName;
                this.tbxRReceiveName.Text = studentReceive.ReceiveName;
                this.tbxRCancelNo.Text = studentReceive.CancelNo;
                this.tbxRAmount.Text = DataFormat.GetAmountText(studentReceive.ReceiveAmount);

                this.divQuery.Visible = false;
                this.UCPageNews.Visible = this.divQuery.Visible;
                this.divResult.Visible = !this.divQuery.Visible;
            }
            #endregion
        }

        protected void ccbtnPay_Click(object sender, EventArgs e)
        {
            KeyValueList<string> payArgs = this.KeepPayArgs;
            if (payArgs == null || payArgs.Count == 0)
            {
                string msg = this.GetLocalized("無法取得繳費資料網頁參數");
                this.ShowJsAlert(msg);
                return;
            }

            string[] argKeys = payArgs.Keys;
            if (!argKeys.Contains("Focas.MerId") || !argKeys.Contains("Focas.MerchantId") || !argKeys.Contains("Focas.TerminalId"))
            {
                string msg = this.GetLocalized("此學校未參加國際信用卡繳費");
                this.ShowJsAlert(msg);
                return;
            }

            this.Server.Transfer("EZPosEntry2.aspx", true);
        }
    }
}