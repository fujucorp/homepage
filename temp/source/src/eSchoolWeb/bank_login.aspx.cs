using System;
using System.Collections.Generic;
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
    public partial class bank_login : LocalizedPage
    {
        #region Implement IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "bank_login";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "銀行專區登入";
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
            UCPageNews.PageId = BoardTypeCodeTexts.BANK;
            if(!IsPostBack)
            {
                this.txtUserId.Text = String.Empty;

                #region [MDY:20220530] Checkmarx 調整
                this.txtPXX.Text = String.Empty;
                #endregion

                #region [MDY:20200712] M202007_01 增加圖形驗證碼
                this.txtValidateCode.Text = String.Empty;
                #endregion

                //this.ccbtnOK.Attributes.Add("onclick", "comingsoon();");
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            string user_id = this.txtUserId.Text.Trim();
            string user_pxx = this.txtPXX.Text.Trim();

            #region [OLD]
            //#region [MDY:20191023] M201910_02 行員登入不使用圖形驗證碼
            //#region [Old]
            ////string validate_number = this.txtValidateNum.Text.Trim();
            ////string real_validate_number = (string)Session["ValidateNum"];

            ////string language = "tw-zh";

            ////#region [Old] 先檢查圖形驗證碼
            //////if (validate_number.ToLower() != real_validate_number.ToLower())
            //////{
            //////    this.txtValidateNum.Text = "";

            //////    this.ShowJsAlert("驗證碼錯誤，請重新輸入.");
            //////    return;
            //////}
            ////#endregion
            //#endregion
            //#endregion
            #endregion

            #region [MDY:20200712] M202007_01 增加圖形驗證碼
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

            #region [MDY:20191023] M201910_02 修正取得語系
            string language = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
            #endregion

            #region 檢查輸入資料
            {
                if (String.IsNullOrEmpty(user_id))
                {
                    this.ShowMustInputAlert("使用者帳號");
                    return;
                }
                if (String.IsNullOrEmpty(user_pxx))
                {
                    this.ShowMustInputAlert("使用者密碼");
                    return;
                }
            }
            #endregion

            #region 開始登入流程
            LogonUser logonUser = null;
            XmlResult xmlResult = DataProxy.Current.LogonForBank(user_id, user_pxx, WebHelper.GetClientIP(), language, out logonUser);
            if (xmlResult.IsSuccess)
            {
                #region 成功
                WebHelper.SetLogonUser(logonUser);

                switch (xmlResult.Code)
                {
                    case ErrorCode.NORMAL_STATUS:
                    case ErrorCode.L_PASSWORD_CHANEG_MEMO:
                        #region 登入成功 / 提醒密碼變更
                        {
                            if (logonUser.HasAuthMenus() && logonUser.HasAuthReceiveType())
                            {
                                #region [MDY:20220530] Checkmarx 調整
                                logonUser.IsRemindChangePXX = (xmlResult.Code == ErrorCode.L_PASSWORD_CHANEG_MEMO);
                                #endregion

                                #region [MDY:20210521] 原碼修正
                                Response.Redirect(WebHelper.GenRNUrl("Main.aspx"));
                                #endregion
                            }
                            else
                            {
                                //無任何授權的功能項或商家代號
                                WebHelper.SetErrorPageInfo(new ErrorPageInfo(this.MenuID, this.MenuName, ErrorCode.S_NO_AUTHORIZE, "無任何授權的功能項或商家代號"));
                                Server.ClearError();

                                #region [MDY:20210521] 原碼修正
                                Server.Transfer(WebHelper.GenRNUrl("~/ErrorPage.aspx"));
                                #endregion
                            }
                        }
                        #endregion
                        break;
                    case ErrorCode.L_PASSWORD_CHANEG_MUST:
                        #region 強迫密碼變更
                        {
                            //this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
                            KeyValueList<string> QueryString = new KeyValueList<string>();
                            QueryString.Add("UnidID", logonUser.UnitId);
                            QueryString.Add("UserID", logonUser.UserId);
                            QueryString.Add("GroupID", logonUser.GroupId);
                            Session["QueryString"] = QueryString;
                            //this.ShowJsAlertAndGoUrl(xmlResult.Message, this.GetResolveUrl("~/ChangePWD.aspx"));
                        }
                        #endregion
                        break;
                    case ErrorCode.L_PASSWORD_OVERDUE:
                        #region 密碼到期
                        {
                            //[TODO] 不知道要怎麽處理，先顯示訊息
                            //this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
                        }
                        #endregion
                        break;
                    default:
                        #region 其他錯誤
                        {
                            //this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
                        }
                        #endregion
                        break;
                }
                #endregion
            }
            else
            {
                #region 失敗
                #region [MDY:20191023] M201910_02 統一改用 ShowJsAlert() 方法顯示錯誤
                #region [OLD]
                //StringBuilder js = new StringBuilder();
                //switch (xmlResult.Code)
                //{
                //    case ErrorCode.L_ACCOUNT_LOCKED:
                //        js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("此帳號已鎖住，請洽管理人員.")).AppendLine();
                //        break;
                //    case ErrorCode.L_ACCOUNT_HAS_LOGON:
                //        js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("此帳號已登入，請洽管理人員.")).AppendLine();
                //        break;
                //    case ErrorCode.L_LOGON_FAILURE:
                //        js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("登入失敗，帳號或密碼不正確.")).AppendLine();
                //        break;
                //    default:
                //        js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("登入發生錯誤，請稍後再試.")).AppendLine();
                //        break;
                //}

                //ClientScriptManager cs = this.ClientScript;
                //Type myType = this.GetType();
                //if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                //{
                //    cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                //}
                #endregion

                switch (xmlResult.Code)
                {
                    case ErrorCode.L_ACCOUNT_LOCKED:
                        this.ShowJsAlert("此帳號已鎖住，請洽管理人員");
                        break;
                    case ErrorCode.L_ACCOUNT_HAS_LOGON:
                        this.ShowJsAlert("此帳號已登入，請洽管理人員");
                        break;
                    case ErrorCode.L_LOGON_FAILURE:
                        this.ShowJsAlert("登入失敗，帳號或密碼不正確");
                        break;
                    default:
                        this.ShowJsAlert("登入發生錯誤，請稍後再試");
                        break;
                }
                #endregion

                return;
                #endregion
            }
            #endregion
            #endregion
            #endregion
        }
    }
}