using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
    public partial class school_login : LocalizedPage   //System.Web.UI.Page, IMenuPage
    {
        #region Override LocalizedPage's IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "school_login";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "學校專區登入";
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

        #region Log 相關
        private const string _MethodName = "LOGIN";
        private string _LogPath = null;

        /// <summary>
        /// 取得 Log 檔完整路徑檔名
        /// </summary>
        /// <returns></returns>
        private string GetLogFileName()
        {
            if (_LogPath == null)
            {
                _LogPath = ConfigurationManager.AppSettings.Get("log_path");
                if (_LogPath == null)
                {
                    _LogPath = String.Empty;
                }
                else
                {
                    _LogPath = _LogPath.Trim();
                }
                if (!String.IsNullOrEmpty(_LogPath))
                {
                    try
                    {
                        if (!Directory.Exists(_LogPath))
                        {
                            Directory.CreateDirectory(_LogPath);
                        }
                    }
                    catch (Exception)
                    {
                        _LogPath = String.Empty;
                    }
                }
            }

            if (String.IsNullOrEmpty(_LogPath))
            {
                return null;
            }
            else
            {
                return Path.Combine(_LogPath, String.Format("{0}_{1:yyyyMMdd}.log", _MethodName, DateTime.Today));
            }
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 寫 Log
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="userId"></param>
        /// <param name="userPXX"></param>
        /// <param name="clientIP"></param>
        /// <param name="xmlResult"></param>
        private void WriteLog(string schoolId, string userId, string userPXX, string clientIP, XmlResult xmlResult)
        {
            if (xmlResult == null)
            {
                return;
            }
            string logFileName = this.GetLogFileName();
            if (String.IsNullOrEmpty(logFileName))
            {
                return;
            }

            StringBuilder log = new StringBuilder();
            log
                .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] [學校登入] (schoolId={1}; userId={2}; userPXX={3}; clientIP={4};)", DateTime.Now, schoolId, userId, userPXX, clientIP).AppendLine()
                .AppendFormat("結果：Message={0}; Code={1};", xmlResult.Message, xmlResult.Code).AppendLine()
                .AppendLine();

            this.WriteLogFile(logFileName, log.ToString());
        }
        #endregion

        /// <summary>
        /// 寫入 Log 檔
        /// </summary>
        /// <param name="fileName">Log 檔名</param>
        /// <param name="log">Log 內容</param>
        private void WriteLogFile(string fileName, string log)
        {
            try
            {
                File.AppendAllText(fileName, log, Encoding.Default);
            }
            catch (Exception)
            {
                //_logPath = String.Empty;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            UCPageNews.PageId = BoardTypeCodeTexts.SCHOOL;
            if(!IsPostBack)
            {
                this.txtSchoolId.Value = "";
                this.txtSchoolName.Text = "";

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
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("驗證碼錯誤，請重新輸入.")).AppendLine();

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
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("學校選擇有誤，請重新選擇學校.")).AppendLine();

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
                this.ShowMustInputAlert("使用者帳號");
                return;
            }
            if (String.IsNullOrEmpty(user_pxx))
            {
                this.ShowMustInputAlert("使用者密碼");
                return;
            }
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
            //        Response.Write("<script>alert('請選擇學校')</script>");
            //        return;
            //    }
            //    if (String.IsNullOrEmpty(user_id))
            //    {
            //        Response.Write("<script>alert('請輸入帳號')</script>");
            //        return;
            //    }
            //    if (String.IsNullOrEmpty(user_pwd))
            //    {
            //        Response.Write("<script>alert('請輸入密碼')</script>");
            //        return;
            //    }
            //}
            //#endregion
            #endregion
            #endregion

            LogonUser logonUser = null;
            XmlResult xmlResult = DataProxy.Current.LogonForSchool(school_id, user_id, user_pxx, WebHelper.GetClientIP(), language, out logonUser);

            this.WriteLog(school_id, user_id, user_pxx, WebHelper.GetClientIP(), xmlResult);

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
                                logonUser.IsRemindChangePXX = (xmlResult.Code == ErrorCode.L_PASSWORD_CHANEG_MEMO);

                                #region [Old]
                                ////Response.Redirect("Main.aspx");
                                ////Response.Redirect("~/D/D1100000.aspx");
                                //string url = null;
                                //foreach (MenuAuth menuAith in logonUser.AuthMenus)
                                //{
                                //    if (menuAith.MenuID.Length > 2)
                                //    {
                                //        MenuInfo menu = MenuHelper.Current.GetMenu(menuAith.MenuID);
                                //        if (menu != null && !String.IsNullOrEmpty(menu.Url))
                                //        {
                                //            url = menu.Url;
                                //            break;
                                //        }
                                //    }
                                //}
                                //if (String.IsNullOrEmpty(url))
                                //{
                                //    //無任何有效頁面的授權
                                //    WebHelper.SetErrorPageInfo(new ErrorPageInfo("LOGON", "學校經辦登入", ErrorCode.S_NO_AUTHORIZE, "無任何有效頁面的授權"));
                                //    Server.ClearError();
                                //    Server.Transfer("~/ErrorPage.aspx");
                                //}
                                //else
                                //{
                                //    this.Response.Redirect("~/" + url);
                                //}
                                #endregion

                                #region 轉址
                                //string firstUrl = null;
                                //string dxxUrl = null;
                                //string bxxUrl = null;
                                //string sxxUrl = null;
                                //string unPUrl = null;
                                //string url = null;
                                //foreach (MenuAuth menuAith in logonUser.AuthMenus)
                                //{
                                //    string menuID = menuAith.MenuID;
                                //    if (menuID.Length > 2)
                                //    {
                                //        MenuInfo menu = MenuHelper.Current.GetMenu(menuAith.MenuID);
                                //        if (menu != null && !String.IsNullOrEmpty(menu.Url))
                                //        {
                                //            url = menu.Url;
                                //            if (firstUrl == null)
                                //            {
                                //                firstUrl = url;
                                //            }
                                //            string preKey = menuID.Substring(0, 1);
                                //            if (menuID.Length == 2)
                                //            {
                                //                if (preKey == "D" && dxxUrl == null)
                                //                {
                                //                    dxxUrl = url;
                                //                }
                                //                else if (preKey == "B" && bxxUrl == null)
                                //                {
                                //                    bxxUrl = url;
                                //                }
                                //                else if (preKey == "S" && sxxUrl == null)
                                //                {
                                //                    sxxUrl = url;
                                //                }
                                //            }
                                //            else if (preKey != "P" && unPUrl == null)
                                //            {
                                //                unPUrl = url;
                                //            }
                                //        }
                                //    }
                                //    if (!String.IsNullOrEmpty(dxxUrl))
                                //    {
                                //        break;
                                //    }
                                //}
                                //if (!String.IsNullOrEmpty(dxxUrl))
                                //{
                                //    this.Response.Redirect("~/" + dxxUrl);
                                //}
                                //else if (!String.IsNullOrEmpty(bxxUrl))
                                //{
                                //    this.Response.Redirect("~/" + bxxUrl);
                                //}
                                //else if (!String.IsNullOrEmpty(sxxUrl))
                                //{
                                //    this.Response.Redirect("~/" + sxxUrl);
                                //}
                                //else if (!String.IsNullOrEmpty(unPUrl))
                                //{
                                //    this.Response.Redirect("~/" + unPUrl);
                                //}
                                //else if (!String.IsNullOrEmpty(firstUrl))
                                //{
                                //    this.Response.Redirect("~/" + firstUrl);
                                //}
                                //else if (!String.IsNullOrEmpty(url))
                                //{
                                //    this.Response.Redirect("~/" + url);
                                //}
                                //else
                                //{
                                //    //無任何有效頁面的授權
                                //    WebHelper.SetErrorPageInfo(new ErrorPageInfo("LOGON", "學校經辦登入", ErrorCode.S_NO_AUTHORIZE, "無任何有效頁面的授權"));
                                //    Server.ClearError();
                                //    Server.Transfer("~/ErrorPage.aspx");
                                //}
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