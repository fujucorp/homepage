using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    #region Enum
    /// <summary>
    /// UCLoginBox 使用者控制項的登入類別舉值
    /// </summary>
    public enum LoginKindEnum
    {
        /// <summary>
        /// 未指定
        /// </summary>
        Empty = 0,

        /// <summary>
        /// 學校登入
        /// </summary>
        School = 1,

        /// <summary>
        /// 學生登入
        /// </summary>
        Student = 2,

        /// <summary>
        /// 行員登入
        /// </summary>
        Bank = 3
    }
    #endregion

    /// <summary>
    /// 使用者登入控制項
    /// </summary>
    public partial class UCLoginBox : BaseUserControl
    {
        #region Propery
        /// <summary>
        /// 登入類別
        /// </summary>
        [DefaultValue(LoginKindEnum.School)]
        [Themeable(false)]
        public LoginKindEnum Kind
        {
            get
            {
                string txt = HttpUtility.HtmlEncode(ViewState["Kind"]);
                LoginKindEnum value = LoginKindEnum.School;
                if (Enum.TryParse<LoginKindEnum>(txt, out value))
                {
                    return value;
                }
                else
                {
                    return LoginKindEnum.School;
                }
            }
            protected set
            {
                ViewState["Kind"] = value;
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            switch (this.Kind)
            {
                case LoginKindEnum.School:
                    this.ShowSchoolLoginUI();
                    break;
                case LoginKindEnum.Student:
                    this.ShowStudentLoginUI();
                    break;
                case LoginKindEnum.Bank:
                    this.ShowBankLoginUI();
                    break;
                default:
                    this.Visible = false;
                    break;
            }
        }

        private void ShowSchoolLoginUI()
        {
            this.pTitle.InnerText = this.GetControlLocalized("UCLoginBox", "Title", "學校經辦登入");
            this.labUnitId.Text = this.GetControlLocalized("UCLoginBox", "UnitId", "統一編號");
            this.labUserId.Text = this.GetControlLocalized("UCLoginBox", "UserId", "使用者代號");

            #region [MDY:20220530] Checkmarx 調整
            this.labUserPXX.Text = this.GetControlLocalized("UCLoginBox", "UserPwd", "使用者密碼");
            #endregion

            this.lbtnSchoolLogon.Text = this.GetControlLocalized("UCLoginBox", "OK", "　確認　");

            #region 學校統編下拉選項
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.SchIdenty };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
                string textCombineFormat = null;

                CodeText[] datas = null;
                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<SchoolRTypeEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("讀取學校資料");
                    this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
                    this.lbtnStudentLogon.Enabled = false;
                }
                WebHelper.SetDropDownListItems(this.ddlUnit, DefaultItem.Kind.Select, false, datas, true, false, 0, null);
            }
            #endregion

            this.divSchool.Visible = true;
            this.divStudent.Visible = false;
            this.Visible = true;
        }

        private void ShowStudentLoginUI()
        {
            this.pTitle.InnerText = this.GetControlLocalized("UCLoginBox", "Title", "學生查詢登入");
            this.labSchool.Text = this.GetControlLocalized("UCLoginBox", "School", "學校");
            this.labStuId.Text = this.GetControlLocalized("UCLoginBox", "StuId", "學號");
            //this.labLoginKey.Text = this.GetControlLocalized("UCLoginBox", "LoginKey", LoginKeyTypeCodeTexts.Default.Text);
            this.labLoginKey.Text = this.GetControlLocalized("UCLoginBox", "LoginKey", "使用者密碼");
            this.lbtnStudentLogon.Text = this.GetControlLocalized("UCLoginBox", "OK", "　確認　");

            #region 學校業務別下拉選項
            {
                SchoolConfigView[] datas = null;
                Expression where = new Expression();
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(2);
                orderbys.Add(SchoolConfigView.Field.SchIdenty, OrderByEnum.Asc);
                orderbys.Add(SchoolConfigView.Field.ReceiveType, OrderByEnum.Asc);
                XmlResult xmlResult = DataProxy.Current.SelectAll<SchoolConfigView>(this.Page, where, orderbys, out datas);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("讀取學校資料");
                    this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
                    this.lbtnStudentLogon.Enabled = false;
                }
                else
                {
                    CodeTextList items = null;
                    if (datas == null || datas.Length == 0)
                    {
                        items = new CodeTextList(0);
                    }
                    else
                    {
                        List<string> chkIds = new List<string>();
                        items = new CodeTextList(datas.Length);
                        foreach (SchoolConfigView data in datas)
                        {
                            if (chkIds.Contains(data.SchIdenty) || String.IsNullOrEmpty(data.SchIdenty))
                            {
                                continue;
                            }
                            string code = String.Format("{0}-{1}", data.LoginKeyType, data.SchIdenty);
                            string text = String.Format("{0}-{1}", data.SchIdenty, data.SchName);
                            items.Add(code, text);
                            chkIds.Add(data.SchIdenty);
                        }
                    }
                    WebHelper.SetDropDownListItems(this.ddlSchool, DefaultItem.Kind.Select, false, items, false, false, 0, null);
                    this.lbtnStudentLogon.Enabled = true;
                }
            }
            #endregion

            this.divSchool.Visible = false;
            this.divStudent.Visible = true;
            this.divBank.Visible = false;
            this.Visible = true;
        }

        private void ShowBankLoginUI()
        {
            this.divSchool.Visible = false;
            this.divStudent.Visible = false;
            this.divBank.Visible = true;
            this.Visible = true;
        }

        public void ChangeLoginKind(LoginKindEnum kind)
        {
            this.Kind = kind;
            switch (this.Kind)
            {
                case LoginKindEnum.School:
                    this.ShowSchoolLoginUI();
                    break;
                case LoginKindEnum.Student:
                    this.ShowStudentLoginUI();
                    break;
                case LoginKindEnum.Bank:
                    this.ShowBankLoginUI();
                    break;
                default:
                    this.Visible = false;
                    break;
            }
        }

        public string GetKeyTypeNames()
        {
            return String.Format("'{0}','{1}'", this.GetLocalized(LoginKeyTypeCodeTexts.PERSONAL_ID_TEXT), this.GetLocalized(LoginKeyTypeCodeTexts.BIRTHDAY_TEXT));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void ddlSchool_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lbtnBankLogon_Click(object sender, EventArgs e)
        {
            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            string userId = this.tbxBankUID.Text.Trim();
            string userPXX = this.tbxBankPXX.Text.Trim();

            #region [MDY:202203XX] 2022擴充案 修正取得語系
            #region [OLD]
            //string language = "tw-zh";
            #endregion

            string language = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
            #endregion

            #region 檢查輸入資料
            {
                if (String.IsNullOrEmpty(userId))
                {
                    this.ShowMustInputAlert(this.labUserId.Text);
                    return;
                }
                if (String.IsNullOrEmpty(userPXX))
                {
                    this.ShowMustInputAlert(this.labUserPXX.Text);
                    return;
                }
            }
            #endregion

            LogonUser logonUser = null;
            string action = this.pTitle.InnerText;
            XmlResult xmlResult = DataProxy.Current.LogonForBank(userId, userPXX, WebHelper.GetClientIP(), language, out logonUser);
            if (xmlResult.IsSuccess)
            {
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

                                #region 轉址
                                string firstUrl = null;
                                string dxxUrl = null;
                                string bxxUrl = null;
                                string sxxUrl = null;
                                string unPUrl = null;
                                string url = null;
                                foreach (MenuAuth menuAith in logonUser.AuthMenus)
                                {
                                    string menuID = menuAith.MenuID;
                                    if (menuID.Length > 2)
                                    {
                                        MenuInfo menu = MenuHelper.Current.GetMenu(menuAith.MenuID);
                                        if (menu != null && !String.IsNullOrEmpty(menu.Url))
                                        {
                                            url = menu.Url;
                                            if (firstUrl == null)
                                            {
                                                firstUrl = url;
                                            }
                                            string preKey = menuID.Substring(0, 1);
                                            if (menuID.Length == 2)
                                            {
                                                if (preKey == "D" && dxxUrl == null)
                                                {
                                                    dxxUrl = url;
                                                }
                                                else if (preKey == "B" && bxxUrl == null)
                                                {
                                                    bxxUrl = url;
                                                }
                                                else if (preKey == "S" && sxxUrl == null)
                                                {
                                                    sxxUrl = url;
                                                }
                                            }
                                            else if (preKey != "P" && unPUrl == null)
                                            {
                                                unPUrl = url;
                                            }
                                        }
                                    }
                                    if (!String.IsNullOrEmpty(dxxUrl))
                                    {
                                        break;
                                    }
                                }

                                #region [MDY:20210521] 原碼修正
                                if (!String.IsNullOrEmpty(dxxUrl))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + dxxUrl));
                                }
                                else if (!String.IsNullOrEmpty(bxxUrl))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + bxxUrl));
                                }
                                else if (!String.IsNullOrEmpty(sxxUrl))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + sxxUrl));
                                }
                                else if (!String.IsNullOrEmpty(unPUrl))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + unPUrl));
                                }
                                else if (!String.IsNullOrEmpty(firstUrl))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + firstUrl));
                                }
                                else if (!String.IsNullOrEmpty(url))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + url));
                                }
                                else
                                {
                                    //無任何有效頁面的授權
                                    WebHelper.SetErrorPageInfo(new ErrorPageInfo("LOGON", "學校經辦登入", ErrorCode.S_NO_AUTHORIZE, "無任何有效頁面的授權"));
                                    Server.ClearError();
                                    Server.Transfer(WebHelper.GenRNUrl("~/ErrorPage.aspx"));
                                }
                                #endregion
                                #endregion
                            }
                            else
                            {
                                //無任何授權的功能項或業務別碼
                                WebHelper.SetErrorPageInfo(new ErrorPageInfo("LOGON", "學校經辦登入", ErrorCode.S_NO_AUTHORIZE, "無任何授權的功能項或業務別碼"));
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
                            this.ShowJsAlertAndGoUrl(xmlResult.Message, this.GetResolveUrl("~/ChangePWD.aspx"));
                        }
                        #endregion
                        break;
                    case ErrorCode.L_PASSWORD_OVERDUE:
                        #region 密碼到期
                        {
                            //[TODO] 不知道要怎麽處理，先顯示訊息
                            this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
                        }
                        #endregion
                        break;
                    default:
                        #region 其他錯誤
                        {
                            this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
                        }
                        #endregion
                        break;
                }
            }
            else
            {
                this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
            }
            #endregion
            #endregion
        }

        protected void lbtnSchoolLogon_Click(object sender, EventArgs e)
        {
            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            string unitId = this.ddlUnit.SelectedValue;
            string userId = this.tbxUserId.Value.Trim();
            string userPXX = this.tbxUserPXX.Value.Trim();

            #region [MDY:202203XX] 2022擴充案 修正取得語系
            #region [OLD]
            //string language = "tw-zh";
            #endregion

            string language = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
            #endregion

            #region 檢查輸入資料
            {
                if (String.IsNullOrEmpty(unitId))
                {
                    this.ShowMustInputAlert(this.labUnitId.Text);
                    return;
                }
                if (String.IsNullOrEmpty(userId))
                {
                    this.ShowMustInputAlert(this.labUserId.Text);
                    return;
                }
                if (String.IsNullOrEmpty(userPXX))
                {
                    this.ShowMustInputAlert(this.labUserPXX.Text);
                    return;
                }
            }
            #endregion

            LogonUser logonUser = null;
            string action = this.pTitle.InnerText;
            XmlResult xmlResult = DataProxy.Current.LogonForSchool(unitId, userId, userPXX, WebHelper.GetClientIP(), language, out logonUser);
            if (xmlResult.IsSuccess)
            {
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
                                string firstUrl = null;
                                string dxxUrl = null;
                                string bxxUrl = null;
                                string sxxUrl = null;
                                string unPUrl = null;
                                string url = null;
                                foreach (MenuAuth menuAith in logonUser.AuthMenus)
                                {
                                    string menuID = menuAith.MenuID;
                                    if (menuID.Length > 2)
                                    {
                                        MenuInfo menu = MenuHelper.Current.GetMenu(menuAith.MenuID);
                                        if (menu != null && !String.IsNullOrEmpty(menu.Url))
                                        {
                                            url = menu.Url;
                                            if (firstUrl == null)
                                            {
                                                firstUrl = url;
                                            }
                                            string preKey = menuID.Substring(0, 1);
                                            if (menuID.Length == 2)
                                            {
                                                if (preKey == "D" && dxxUrl == null)
                                                {
                                                    dxxUrl = url;
                                                }
                                                else if (preKey == "B" && bxxUrl == null)
                                                {
                                                    bxxUrl = url;
                                                }
                                                else if (preKey == "S" && sxxUrl == null)
                                                {
                                                    sxxUrl = url;
                                                }
                                            }
                                            else if (preKey != "P" && unPUrl == null)
                                            {
                                                unPUrl = url;
                                            }
                                        }
                                    }
                                    if (!String.IsNullOrEmpty(dxxUrl))
                                    {
                                        break;
                                    }
                                }

                                #region [MDY:20210521] 原碼修正
                                if (!String.IsNullOrEmpty(dxxUrl))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + dxxUrl));
                                }
                                else if (!String.IsNullOrEmpty(bxxUrl))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + bxxUrl));
                                }
                                else if (!String.IsNullOrEmpty(sxxUrl))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + sxxUrl));
                                }
                                else if (!String.IsNullOrEmpty(unPUrl))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + unPUrl));
                                }
                                else if (!String.IsNullOrEmpty(firstUrl))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + firstUrl));
                                }
                                else if (!String.IsNullOrEmpty(url))
                                {
                                    this.Response.Redirect(WebHelper.GenRNUrl("~/" + url));
                                }
                                else
                                {
                                    //無任何有效頁面的授權
                                    WebHelper.SetErrorPageInfo(new ErrorPageInfo("LOGON", "學校經辦登入", ErrorCode.S_NO_AUTHORIZE, "無任何有效頁面的授權"));
                                    Server.ClearError();
                                    Server.Transfer(WebHelper.GenRNUrl("~/ErrorPage.aspx"));
                                }
                                #endregion
                                #endregion
                            }
                            else
                            {
                                //無任何授權的功能項或業務別碼
                                WebHelper.SetErrorPageInfo(new ErrorPageInfo("LOGON", "學校經辦登入", ErrorCode.S_NO_AUTHORIZE, "無任何授權的功能項或業務別碼"));
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
                            this.ShowJsAlertAndGoUrl(xmlResult.Message, this.GetResolveUrl("~/ChangePWD.aspx"));
                        }
                        #endregion
                        break;
                    case ErrorCode.L_PASSWORD_OVERDUE:
                        #region 密碼到期
                        {
                            //[TODO] 不知道要怎麽處理，先顯示訊息
                            this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
                        }
                        #endregion
                        break;
                    default:
                        #region 其他錯誤
                        {
                            this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
                        }
                        #endregion
                        break;
                }
            }
            else
            {
                this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
            }
            #endregion
            #endregion
        }

        protected void lbtnStudentLogon_Click(object sender, EventArgs e)
        {
            string schIdentity = this.ddlSchool.SelectedValue;
            string studentId = this.tbxStuId.Text.Trim();
            string loginKey = this.tbxLoginKey.Text.Trim();

            #region [MDY:202203XX] 2022擴充案 修正取得語系
            #region [OLD]
            //string language = "tw-zh";
            #endregion

            string language = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
            #endregion

            #region 檢查輸入資料
            {
                if (String.IsNullOrEmpty(schIdentity))
                {
                    this.ShowMustInputAlert(this.labSchool.Text);
                    return;
                }
                string loginKeyType = schIdentity.Substring(0, 1);
                schIdentity = schIdentity.Substring(2);
                if (String.IsNullOrEmpty(studentId))
                {
                    this.ShowMustInputAlert(this.labStuId.Text);
                    return;
                }
                if (String.IsNullOrEmpty(loginKey))
                {
                    string fieldName = null;
                    switch (loginKeyType)
                    {
                        case LoginKeyTypeCodeTexts.PERSONAL_ID:
                            fieldName = this.GetLocalized(LoginKeyTypeCodeTexts.PERSONAL_ID_TEXT);
                            break;
                        case LoginKeyTypeCodeTexts.BIRTHDAY:
                            fieldName = this.GetLocalized(LoginKeyTypeCodeTexts.BIRTHDAY_TEXT);
                            break;
                    }
                    this.ShowMustInputAlert(fieldName);
                    return;
                }
            }
            #endregion

            LogonUser logonUser = null;
            string action = this.pTitle.InnerText;
            XmlResult xmlResult = DataProxy.Current.LogonForStudent(schIdentity, studentId, loginKey, WebHelper.GetClientIP(), language, out logonUser);
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
                this.ShowActionFailureAlert(action, xmlResult.Code, xmlResult.Message);
            }
        }
    }
}