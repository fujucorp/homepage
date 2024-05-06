using System;
using System.Collections;
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
    /// 支付寶繳費 (輸入頁面)
    /// </summary>
    public partial class alipay : LocalizedPage
    {
        #region Implement IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "Alipay";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "支付寶繳費";
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            UCPageNews.PageId = BoardTypeCodeTexts.CREDITCARD;  //沒有 For 支付寶的公告所以沿用信用卡的公告
            if (!this.IsPostBack)
            {
                #region [MDY:20170828] 因為土銀的支付寶合約未完成，增加是否啟用的判斷
                if (!Fisc.IsInboundEnabled())
                {
                    #region [MDY:20210521] 原碼修正
                    Response.Redirect(WebHelper.GenRNUrl("~/index.aspx"));
                    #endregion
                }
                #endregion

                #region 目前沒有提供學生頁面直接點支付寶繳費，暫時 Mark
                //this.KeepCancelNo = this.Request.QueryString["cno"];
                //string cancelNo = this.KeepCancelNo;
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
            string cancel_no = this.txtCancelNo.Text.Trim();

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

            #region 檢查銷帳編號
            #region [MDY:20191023] M201910_02 統一虛擬帳號檢查處理
            #region [OLD]
            //if (cancel_no.Length == 0)
            //{
            //    this.ShowJsAlert("請輸入虛擬帳號");
            //    return;
            //}
            //if (!Common.IsNumber(cancel_no, 14, 16))
            //{
            //    //[TODO] 固定顯示訊息的收集
            //    this.ShowJsAlert("虛擬帳號限輸入14  ~ 16 碼數字");
            //    return;
            //}
            #endregion

            if (String.IsNullOrEmpty(cancel_no))
            {
                this.ShowMustInputAlert("虛擬帳號");
                return;
            }
            if (!Common.IsNumber(cancel_no) && cancel_no.Length != 16 && cancel_no.Length != 14)
            {
                this.ShowJsAlert("輸入的虛擬帳號有誤，請重新輸入");
                return;
            }
            #endregion

            this.KeepCancelNo = cancel_no;
            #endregion

            #region [MDY:20180101] 檢查支付保授權商家代號
            #region [MDY:20180926] 增加特殊商家代號判斷 (開放可繳4000元以下繳費單)
            #region [OLD]
            //{
            //    string errmsg = null;
            //    InboundConfig data = Fisc.GetInboundConfig(out errmsg);
            //    if (data == null)
            //    {
            //        this.ShowJsAlert(errmsg);
            //        return;
            //    }
            //    if (!data.IsAuthReceiveType(cancel_no.Substring(0, 4)))
            //    {
            //        this.ShowJsAlert("此虛擬帳號未授權使用支付寶繳費");
            //        return;
            //    }
            //    isSpecialReceiveType = data.IsSpecialReceiveType(cancel_no.Substring(0, 4)
            //}
            #endregion

            bool isSpecialReceiveType = false;
            {
                string errmsg = null;
                InboundConfig data = Fisc.GetInboundConfig(out errmsg);
                if (data == null)
                {
                    this.ShowJsAlert(errmsg);
                    return;
                }
                string receiveType = cancel_no.Substring(0, 4);
                if (!data.IsAuthReceiveType(receiveType))
                {
                    this.ShowJsAlert("此虛擬帳號未授權使用支付寶繳費");
                    return;
                }
                isSpecialReceiveType = data.IsSpecialReceiveType(receiveType);
            }
            #endregion
            #endregion

            #region 檢查銷帳編號是否有效
            {
                StudentReceiveView studentReceive = null;

                #region 虛擬帳號 + 未繳 + 金額大於 0 (View 已做 NULL 轉空字串 處理)
                Expression where = new Expression(StudentReceiveView.Field.ReceiveType, cancel_no.Substring(0, 4))
                    .And(StudentReceiveView.Field.CancelNo, cancel_no)
                    .And(new Expression(StudentReceiveView.Field.ReceiveWay, String.Empty).Or(StudentReceiveView.Field.ReceiveWay, null))
                    .And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);
                #endregion

                #region 有指定明確的 PKey
                int oldSeq = -1;
                string[] keys = this.KeepKeys;
                if (keys != null && keys.Length == 7 && Int32.TryParse(keys[6].Trim(), out oldSeq) && oldSeq >= 0)
                {
                    where.And(StudentReceiveView.Field.ReceiveType, keys[0].Trim())
                        .And(StudentReceiveView.Field.YearId, keys[1].Trim())
                        .And(StudentReceiveView.Field.TermId, keys[2].Trim())
                        .And(StudentReceiveView.Field.DepId, keys[3].Trim())
                        .And(StudentReceiveView.Field.ReceiveId, keys[4].Trim())
                        .And(StudentReceiveView.Field.StuId, keys[5].Trim())
                        .And(StudentReceiveView.Field.OldSeq, oldSeq);
                }
                #endregion

                #region 依最新的學年、學期、建立日期、資料序號排序
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
                orderbys.Add(StudentReceiveView.Field.YearId, OrderByEnum.Desc);
                orderbys.Add(StudentReceiveView.Field.TermId, OrderByEnum.Desc);
                orderbys.Add(StudentReceiveView.Field.ReceiveType, OrderByEnum.Asc);
                orderbys.Add(StudentReceiveView.Field.CreateDate, OrderByEnum.Desc);
                orderbys.Add(StudentReceiveView.Field.OldSeq, OrderByEnum.Desc);
                #endregion

                XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentReceiveView>(this.Page, where, orderbys, out studentReceive);
                if (xmlResult.IsSuccess)
                {
                    if (studentReceive == null)
                    {
                        this.ShowJsAlert("查無指定的繳款資料");
                        return;
                    }

                    #region 檢查繳費期限 (支付寶走財金管道，但沒說繳款期限要用哪一個，所以用一般的繳費期限)
                    DateTime? payDueDate = DataFormat.ConvertDateText(studentReceive.PayDueDate);    //繳款單的繳費期限 StudentReceive.PayDueDate
                    if (payDueDate == null)
                    {
                        payDueDate = DataFormat.ConvertDateText(studentReceive.PayDueDate1);   //費用別的繳費期限 SchoolRid.PayDate
                    }
                    if (payDueDate == null || payDueDate.Value < DateTime.Today)
                    {
                        this.ShowJsAlert("已超過繳費期限");
                        return;
                    }
                    #endregion

                    #region [MDY:20180212] 檢查帳單金額，小於4000顯示「此筆繳款單無法使用支付寶通路繳款，請選擇其他繳款通路進行繳款。」提醒訊息
                    #region [MDY:20180926] 排除特殊商家代號清單 (開放可繳4000元以下繳費單)
                    #region [OLD]
                    //if (studentReceive.ReceiveAmount != null && studentReceive.ReceiveAmount.Value < 4000)
                    //{
                    //    this.ShowJsAlert("此筆繳款單無法使用支付寶通路繳款，請選擇其他繳款通路進行繳款。");
                    //    return;
                    //}
                    #endregion

                    if (studentReceive.ReceiveAmount.HasValue && studentReceive.ReceiveAmount.Value < 4000 && !isSpecialReceiveType)
                    {
                        this.ShowJsAlert("此筆繳款單無法使用支付寶通路繳款，請選擇其他繳款通路進行繳款。");
                        return;
                    }
                    #endregion
                    #endregion
                }
                else
                {
                    this.ShowJsAlert("查詢指定繳款資料失敗");
                    return;
                }

                #region 儲存明確指定 PKey
                this.KeepKeys = new string[7] { studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId
                    , studentReceive.DepId, studentReceive.ReceiveId, studentReceive.StuId, studentReceive.OldSeq.ToString() };
                #endregion
            }
            #endregion

            string url = "alipay_d.aspx";
            this.Server.Transfer(url, true);
        }
    }
}