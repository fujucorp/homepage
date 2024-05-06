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
    public partial class student_login : LocalizedPage  //System.Web.UI.Page, IMenuPage
    {
        #region Override LocalizedPage's IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "student_login";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "student_login";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            UCPageNews.PageId = BoardTypeCodeTexts.STUDENT;
            if (!IsPostBack)
            {
                this.txtSchoolId.Value = "";
                this.txtSchoolName.Text = "";
                this.txtUserId.Text = "";

                #region [MDY:20220530] Checkmarx 調整
                this.txtPXX.Text = "";
                #endregion

                #region [MDY:20191023] M201910_02 修正圖形驗證碼弱點
                #region [OLD]
                //this.txtValidateNum.Text = "";
                #endregion

                this.txtValidateCode.Text = "";
                #endregion

                this.txtSchoolName.Attributes.Add("onfocus", "showSchoolList();");
                //this.ccbtnOK.Attributes.Add("onclick", "comingsoon();");
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            string school_name = this.txtSchoolName.Text.Trim();
            string school_id = this.txtSchoolId.Value.Trim();
            string user_id = this.txtUserId.Text.Trim();
            string user_pxx = this.txtPXX.Text.Trim();

            #region [MDY:20191023] M201910_02 修正圖形驗證碼弱點 & 統一參數檢查處理
            #region [OLD]
            //string validate_number = this.txtValidateNum.Text.Trim();
            //string real_validate_number = (string)Session["ValidateNum"];

            //string language = "tw-zh";

            //#region 先檢查圖形驗證碼
            //if (validate_number.ToLower() != real_validate_number.ToLower())
            //{
            //    this.txtValidateNum.Text = "";

            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(this.GetLocalized("驗證碼錯誤請重新輸入"))).AppendLine();

            //    ClientScriptManager cs = this.ClientScript;
            //    Type myType = this.GetType();
            //    if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
            //    {
            //        cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
            //    }

            //    //Response.Write(string.Format("<script>alert('驗證碼錯誤，請從新輸入。{0} : {1})</script>", real_validate_number, validate_number));
            //    return;
            //}
            //#endregion

            //#region 檢查學校
            //if (school_id == "")
            //{
            //    //表示學校不是用選的是用打的
            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(this.GetLocalized("學校選擇有誤請重新選擇學校"))).AppendLine();

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

            #region 檢查輸入資料
            if (String.IsNullOrEmpty(school_name))
            {
                this.ShowMustInputAlert("學校");
                return;
            }
            if (String.IsNullOrEmpty(school_id))
            {
                this.ShowJsAlert("學校選擇有誤，請重新選擇學校");
                return;
            }
            if (String.IsNullOrEmpty(user_id))
            {
                this.ShowMustInputAlert("學號");
                return;
            }

            #region [MDY:20200815] M202008_02 使用身份證字號作為登入依據時，身分證字號不可與學號相同 (2020806_01)
            string loginKeyKind = this.hidLoginKeyKind.Value.ToUpper();
            string loginKeyKindName = null;
            switch (loginKeyKind)
            {
                case "B":  //驗證生日欄位
                    loginKeyKindName = "生日";
                    break;
                case "I":  //驗證身份證號
                    loginKeyKindName = "身份證字號";
                    break;
                case "B2P":  //驗證生日欄位但顯示成使用者密碼
                case "I2P":  //驗證身份證號但顯示成使用者密碼
                default:
                    loginKeyKindName = "使用者密碼";
                    break;
            }
            if (String.IsNullOrEmpty(user_pxx))
            {
                this.ShowMustInputAlert(loginKeyKindName);
                return;
            }
            if (user_id == user_pxx && (loginKeyKind == "I" || loginKeyKind == "I2P"))
            {
                this.ShowJsAlert(String.Format("{0} 不可與學號相同", loginKeyKindName));
                return;
            }
            #endregion

            #endregion
            #endregion

            #region [MDY:20191023] M201910_02 修正取得語系
            string language = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
            #endregion

            #region 開始登入流程

            #region [MDY:20191023] M201910_02 統一參數檢查處理
            #region [OLD]
            //#region 檢查輸入資料
            //{
            //    if (String.IsNullOrEmpty(school_id))
            //    {
            //        this.ShowJsAlert(this.GetLocalized("請選擇學校"));
            //        return;
            //    }
            //    if (String.IsNullOrEmpty(user_id))
            //    {
            //        this.ShowJsAlert(this.GetLocalized("請輸入學號"));
            //        return;
            //    }
            //    if (String.IsNullOrEmpty(user_pwd))
            //    {
            //        this.ShowJsAlert(this.GetLocalized("請輸入密碼"));
            //        return;
            //    }
            //}
            //#endregion
            #endregion
            #endregion

            LogonUser logonUser = null;
            XmlResult xmlResult = DataProxy.Current.LogonForStudent(school_id, user_id, user_pxx, WebHelper.GetClientIP(), language, out logonUser);
            if (xmlResult.IsSuccess)
            {
                logonUser.AuthMenus = new MenuAuth[] { new MenuAuth("X1100001", AuthCodeEnum.All) };    //學生的權限固定
                WebHelper.SetLogonUser(logonUser);

                string url = null;
                foreach (MenuAuth menuAith in logonUser.AuthMenus)
                {
                    MenuInfo menu = MenuHelper.Current.GetMenu(menuAith.MenuID);
                    if (menu != null && !String.IsNullOrEmpty(menu.Url))
                    {
                        url = menu.Url;
                        break;
                    }
                }

                #region [MDY:20210521] 原碼修正
                this.Response.Redirect(WebHelper.GenRNUrl("~/" + url));
                #endregion
            }
            else
            {
                #region [MDY:20191023] M201910_02 統一改用 ShowJsAlert() 方法顯示錯誤
                #region [OLD]
                //StringBuilder js = new StringBuilder();
                //js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(xmlResult.Message)).AppendLine();

                //ClientScriptManager cs = this.ClientScript;
                //Type myType = this.GetType();
                //if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                //{
                //    cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                //}
                #endregion

                this.ShowJsAlert(xmlResult.Message);
                #endregion
            }
            #endregion
            #endregion
            #endregion
        }
    }
}