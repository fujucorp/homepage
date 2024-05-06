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
    public partial class M0001 : LocalizedPage  //System.Web.UI.Page, IMenuPage
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

        protected string _SchoolItemsJson = null;

        private void InitialUi()
        {
            #region [Old]
            //CodeText[] datas = CacheHelper.GetSchoolItems();
            //if (datas != null && datas.Length > 0)
            //{
            //    string[] items = new string[datas.Length];
            //    int idx = 0;
            //    foreach (CodeText data in datas)
            //    {
            //        items[idx] = String.Format("{{ value: '{0}', label: '{1}', lkind: '{2}' }}", data.Code.Trim(), data.Text.Trim());
            //        idx++;
            //    }
            //    _SchoolItemsJson = String.Join(",", items);
            //}
            //else
            //{
            //    _SchoolItemsJson = String.Empty;
            //}
            #endregion

            #region [Old]
            //SchoolConfigView[] datas = CacheHelper.GetSchoolConfigs();
            //if (datas != null && datas.Length > 0)
            //{
            //    string[] items = new string[datas.Length];
            //    int idx = 0;
            //    foreach (SchoolConfigView data in datas)
            //    {
            //        items[idx] = String.Format("{{ value: '{0}', label: '{1}', pwdkind: '{2}' }}", data.SchIdenty.Trim(), data.SchName.Trim(), data.LoginKeyType);
            //        idx++;
            //    }
            //    _SchoolItemsJson = HttpUtility.JavaScriptStringEncode(String.Join(",", items));
            //}
            //else
            //{
            //    _SchoolItemsJson = String.Empty;
            //}
            #endregion

            #region [MDY:20200916] M202008_02 取得「顯示使用者密碼的學校代碼」參數設定值 (2020819_01)
            List<string> schoolIds = null;
            {
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.FIXVERIFY_SCHOOLID);
                KeyValueList<OrderByEnum> orderbys = null;
                ConfigEntity config = null;
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this.Page, where, orderbys, out config);
                if (xmlResult.IsSuccess && config != null && !String.IsNullOrWhiteSpace(config.ConfigValue))
                {
                    string[] values = config.ConfigValue.Trim().Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    schoolIds = new List<string>(values.Length);
                    foreach (string value in values)
                    {
                        schoolIds.Add(value.Trim());
                    }
                }
            }
            #endregion

            #region [MDY:20180520] 學校欄位改用下拉式選單 (20180518_01)
            {
                SchoolConfigView[] datas = CacheHelper.GetSchoolConfigs();
                if (datas != null && datas.Length > 0)
                {
                    #region [MDY:202203XX] 2022擴充案 取得是否使用英文介面
                    bool isEngUI = "en-US".Equals(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(), StringComparison.CurrentCultureIgnoreCase);
                    #endregion

                    StringBuilder json = new StringBuilder();
                    foreach (SchoolConfigView data in datas)
                    {
                        #region [MDY:20190218] 排除不開放學生專區的資料 (20190212_01)
                        if (data.OpenStudentArea == "N")
                        {
                            continue;
                        }
                        #endregion

                        string schIdenty = HttpUtility.JavaScriptStringEncode(data.SchIdenty.Trim());

                        #region [MDY:202203XX] 2022擴充案 改用 GetSchName() 取得學校名稱
                        #region [OLD]
                        //string schName = HttpUtility.JavaScriptStringEncode(data.SchName.Trim());
                        #endregion

                        string schName = HttpUtility.JavaScriptStringEncode(data.GetSchName(isEngUI));
                        #endregion

                        string keyType = HttpUtility.JavaScriptStringEncode(data.LoginKeyType.Trim());
                        string schType = HttpUtility.JavaScriptStringEncode(data.CorpType.Trim());

                        #region [MDY:20200815] M202008_02 依據驗證欄位種類、顯示使用者密碼的學校代碼參數產生 chooseSchool() 的參數 (2020806_01)(2020819_01)
                        #region [OLD]
                        //json.AppendFormat("{{ value: '{0}', label: '{1}', pwdkind: '{2}', schType: '{3}' }}", schIdenty, schName, keyType, schType).AppendLine();
                        #endregion

                        string loginKeyKind = "Y".Equals(keyType, StringComparison.CurrentCultureIgnoreCase) ? "B" : "I";
                        if (schoolIds != null && schoolIds.Contains(schIdenty))
                        {
                            loginKeyKind += "2P";
                        }
                        json.AppendFormat("{{ value: '{0}', label: '{1}', pwdkind: '{2}', schType: '{3}' }}", schIdenty, schName, loginKeyKind, schType).AppendLine();
                        #endregion
                    }
                    _SchoolItemsJson = json.ToString().Trim().Replace("\r\n", ",\r\n");
                }
                else
                {
                    _SchoolItemsJson = String.Empty;
                }
            }
            #endregion
        }

        private void Logon()
        {
            #region [MDY:20191023] M201910_02 修正圖形驗證碼弱點
            #region [OLD]
            //#region 檢查圖形驗證碼
            //string validate_number = this.txtValidateNum.Text.Trim();
            //string real_validate_number = (string)Session["ValidateNum"];
            //if (validate_number.ToLower() != real_validate_number.ToLower())
            //{
            //    this.txtValidateNum.Text = "";

            //    this.ShowJsAlert(this.GetLocalized("驗證碼錯誤請重新輸入"));
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

            #region [MDY:20210401] 原碼修正
            string schName = this.tbxSchName.Text.Trim();
            string schIdenty = this.hidSchIdenty.Value.Trim();
            string stuId = this.tbxStuID.Text.Trim();
            string stuPWord = this.tbxPWord.Text.Trim();

            #region [MDY:20191023] M201910_02 修正取得語系
            #region [OLD]
            //string language = "tw-zh";
            #endregion

            string language = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
            #endregion

            #region 檢查輸入資料
            if (String.IsNullOrEmpty(schName) || String.IsNullOrEmpty(schIdenty))
            {
                if (this.tbxSchName.Visible)
                {
                    this.ShowJsAlert(this.GetLocalized("學校選擇有誤請重新選擇學校"));
                    return;
                }
                else
                {
                    SchoolConfigView myData = null;
                    schIdenty = Request.Form["selSchIdenty"];
                    if (!String.IsNullOrWhiteSpace(schIdenty))
                    {
                        schIdenty = schIdenty.Trim();
                        SchoolConfigView[] datas = CacheHelper.GetSchoolConfigs();
                        if (datas != null && datas.Length > 0)
                        {
                            foreach (SchoolConfigView data in datas)
                            {
                                if (data.SchIdenty == schIdenty)
                                {
                                    myData = data;
                                    break;
                                }
                            }
                        }
                    }
                    if (myData == null)
                    {
                        this.ShowJsAlert(this.GetLocalized("學校選擇有誤請重新選擇學校"));
                        return;
                    }
                    schIdenty = myData.SchIdenty;
                }
            }
            if (String.IsNullOrEmpty(stuId))
            {
                this.ShowJsAlert(this.GetLocalized("請輸入學號"));
                return;
            }
            if (String.IsNullOrEmpty(stuPWord))
            {
                this.ShowJsAlert(this.GetLocalized("請輸入密碼"));
                return;
            }
            #endregion

            LogonUser logonUser = null;
            XmlResult xmlResult = DataProxy.Current.LogonForStudent(schIdenty, stuId, stuPWord, WebHelper.GetClientIP(), language, out logonUser);
            if (xmlResult.IsSuccess)
            {
                logonUser.AuthMenus = new MenuAuth[] { new MenuAuth("X1100001", AuthCodeEnum.All) };    //學生的權限固定
                WebHelper.SetLogonUser(logonUser);

                //string url = null;
                //foreach (MenuAuth menuAith in logonUser.AuthMenus)
                //{
                //    MenuInfo menu = MenuHelper.Current.GetMenu(menuAith.MenuID);
                //    if (menu != null && !String.IsNullOrEmpty(menu.Url))
                //    {
                //        url = menu.Url;
                //        break;
                //    }
                //}
                //this.Response.Redirect("~/" + url);

                #region [MDY:20210521] 原碼修正
                this.Response.Redirect(WebHelper.GenRNUrl("~/student/student001m.aspx"));
                #endregion
            }
            else
            {
                this.ShowJsAlert(xmlResult.Message);
            }
            #endregion
        }

        #region [OLD]
        //private void ShowJsAlert(string msg)
        //{
        //    if (!String.IsNullOrEmpty(msg))
        //    {
        //        StringBuilder js = new StringBuilder();
        //        js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(msg)).AppendLine();

        //        ClientScriptManager cs = this.ClientScript;
        //        Type myType = this.GetType();
        //        string jsKey = "SHOW_JS_ALERT";
        //        if (!cs.IsClientScriptBlockRegistered(myType, jsKey))
        //        {
        //            cs.RegisterClientScriptBlock(myType, jsKey, js.ToString(), true);
        //        }
        //    }
        //}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack )
            {
                UCPageNews.PageId = BoardTypeCodeTexts.STUDENT;
            }
            this.InitialUi();
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            this.Logon();
        }
    }
}