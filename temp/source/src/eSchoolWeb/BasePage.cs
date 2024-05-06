using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Fuju;
using Fuju.Configuration;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 網頁基底抽象類別 (會檢查權限)
    /// </summary>
    public abstract class BasePage : LocalizedPage
    {
        #region Const
        /// <summary>
        /// 觸發 PostBack 事件的頁面參數名稱
        /// </summary>
        private const string EVENT_TARGET = "__EVENTTARGET";

        /// <summary>
        /// 自定錯誤頁
        /// </summary>
        private const string ERROR_PAGE = "ErrorPage.aspx";

        ///// <summary>
        ///// 預設的 Javascript Alert 訊息 Key 的常數
        ///// </summary>
        //private const string SHOW_JS_ALERT_KEY = "SHOW_JS_ALERT";
        #endregion

        #region 建構式
        protected BasePage()
        {
        }
        #endregion

        #region Override Page
        /// <summary>
        /// 引發頁面初始化的事件處理
        /// </summary>
        /// <param name="e">事件參數資料</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            #region 檢查登入與功能狀態
            {
                LogonUser logonUser = this.GetLogonUser();
                if (logonUser == null || logonUser.LogonTime == null || String.IsNullOrEmpty(logonUser.LogonSN))
                {
                    XmlResult xmlResult = new XmlResult(false, "頁面閒置逾時", ErrorCode.S_SESSION_TIMEOUT, null);
                    bool isEditPage = false;
                    bool isSubPage = false;
                    WebHelper.SetErrorPageInfo(new ErrorPageInfo(this.GetMenuInfo(out isEditPage, out isSubPage), xmlResult));

                    #region [MDY:20210521] 原碼修正
                    this.Response.Redirect(WebHelper.GenRNUrl("~/index.aspx?ec=1"));
                    #endregion

                    //Server.Transfer("~/ErrorPage.aspx", true);
                }
                else
                {
                    bool checkFuncFlag = this.IsNeedCheckAuth;  //如果不需要檢查任何授權，就不用檢查是否有功能權限
                    string resultCode = null;
                    XmlResult xmlResult = DataProxy.Current.CheckLogon(this, logonUser, checkFuncFlag, out resultCode);
                    if (xmlResult.IsSuccess)
                    {
                        switch (resultCode)
                        {
                            case CheckLogonResultCodeTexts.IS_OK:
                                break;
                            case CheckLogonResultCodeTexts.CHECK_FAILURE:
                                xmlResult = new XmlResult(false, "檢查登入狀態發生未知錯誤", CoreStatusCode.UNKNOWN_ERROR, null);
                                break;
                            case CheckLogonResultCodeTexts.NON_LOGON:
                                xmlResult = new XmlResult(false, "帳戶已登出或被系統強迫登出", ErrorCode.S_FORCED_LOGOUT, null);
                                break;
                            case CheckLogonResultCodeTexts.FUNC_DISABLED:
                                xmlResult = new XmlResult(false, "此功能停用", ErrorCode.S_FUNC_DISABLED, null);
                                break;
                        }
                    }
                    if (!xmlResult.IsSuccess)
                    {
                        bool isEditPage = false;
                        bool isSubPage = false;
                        WebHelper.SetErrorPageInfo(new ErrorPageInfo(this.GetMenuInfo(out isEditPage, out isSubPage), xmlResult));

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("~/ErrorPage.aspx"), true);
                        #endregion
                    }
                }
            }
            #endregion

            #region 檢查登入者對目前頁面是否有任何授權
            if (this.IsNeedCheckAuth && !this.HasAnyAuth())
            {
                XmlResult xmlResult = new XmlResult(false, "無權限", ErrorCode.S_NO_AUTHORIZE, null);
                bool isEditPage = false;
                bool isSubPage = false;
                WebHelper.SetErrorPageInfo(new ErrorPageInfo(this.GetMenuInfo(out isEditPage, out isSubPage), xmlResult));
                Server.ClearError();

                #region [MDY:20210521] 原碼修正
                Server.Transfer(WebHelper.GenRNUrl("~/ErrorPage.aspx"));
                #endregion
            }
            #endregion

            string menuName = this.MenuName;
            if (!String.IsNullOrEmpty(menuName) && this.IsMatchMenuID)
            {
                this.Title = String.Concat("土地銀行 - 代收學雜費服務網 - ", menuName);
            }
            else if (String.IsNullOrWhiteSpace(this.Title))
            {
                this.Title = "土地銀行 - 代收學雜費服務網";
            }
        }

        /// <summary>
        /// 引發頁面 Error 的事件處理
        /// </summary>
        /// <param name="e">事件參數資料</param>
        protected override void OnError(EventArgs e)
        {
            Exception ex = Server.GetLastError();
            bool isEditPage = false;
            bool isSubPage = false;
            WebHelper.SetErrorPageInfo(new ErrorPageInfo(this.GetMenuInfo(out isEditPage, out isSubPage), ex));
            Server.ClearError();
            //Server.Transfer("~/ErrorPage.aspx");
            Response.Redirect("~/ErrorPage.aspx");
        }

        /// <summary>
        /// 引發頁面載入物件前的事件處理
        /// </summary>
        /// <param name="e">事件參數資料</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack)
            {
                #region 記錄使用者操作日誌
                if (ConfigManager.Current.GetProjectConfigValue("WriteOPLog", "ENABLED") == "TRUE")
                {
                    this.OperationLog();
                }
                #endregion
            }
        }
        #endregion


        #region 登入者資訊相關
        /// <summary>
        /// 取得登入者資料
        /// </summary>
        /// <returns></returns>
        public LogonUser GetLogonUser()
        {
            return WebHelper.GetLogonUser();
        }
        #endregion

        #region 權限相關
        /// <summary>
        /// 取得是否需要檢查登入者對目前頁面是否有任何授權，不需檢查授權的繼承頁面，需 Override 此屬性為 false
        /// </summary>
        public virtual bool IsNeedCheckAuth
        {
            get
            {
                return true;
            }
        }

        private MenuAuth _MenuAuth = null;

        /// <summary>
        /// 取得目前頁面對應的選單(功能)授權
        /// </summary>
        /// <returns></returns>
        public MenuAuth GetMenuAuth()
        {
            if (_MenuAuth == null)
            {
                LogonUser logonUser = this.GetLogonUser();
                bool isEditPage = false;
                bool isSubPage = false;
                _MenuAuth = logonUser.GetMenuAuth(this.GetMenuID(out isEditPage, out isSubPage));
            }
            return _MenuAuth;
        }

        /// <summary>
        /// 取得是否有新增授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool HasInsertAuth()
        {
            return this.GetMenuAuth().HasInsert();
        }

        /// <summary>
        /// 取得是否有修改授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool HasUpdateAuth()
        {
            return this.GetMenuAuth().HasUpdate();
        }

        /// <summary>
        /// 取得是否有刪除授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool HasDeleteAuth()
        {
            return this.GetMenuAuth().HasDelete();
        }

        /// <summary>
        /// 取得是否有查詢授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool HasSelectAuth()
        {
            return this.GetMenuAuth().HasSelect();
        }

        /// <summary>
        /// 取得是否有列印授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool HasPrintAuth()
        {
            return this.GetMenuAuth().HasPrint();
        }

        /// <summary>
        /// 取得登入者對目前頁面是否有維護授權
        /// </summary>
        /// <returns></returns>
        public bool HasMaintainAuth()
        {
            return this.GetMenuAuth().HasMaintain();
        }

        /// <summary>
        /// 取得登入者對目前頁面是否有查詢授權
        /// </summary>
        /// <returns></returns>
        public bool HasQueryAuth()
        {
            return this.GetMenuAuth().HasQuery();
        }

        /// <summary>
        /// 取得登入者對目前頁面是否有任何授權
        /// </summary>
        /// <returns></returns>
        public bool HasAnyAuth()
        {
            LogonUser logonUser = this.GetLogonUser();
            bool isEditPage = false;
            bool isSubPage = false;
            return logonUser.IsAuthMenuID(this.GetMenuID(out isEditPage, out isSubPage));
        }
        #endregion

        #region 頁面、選單(功能)資訊相關
        private bool _IsEditPage = false;
        private bool _IsSubPage = false;
        private string _MenuID = null;
        /// <summary>
        /// 取得目前頁面對應的選單(功能)代碼
        /// </summary>
        /// <param name="isEditPage">傳回是否為延伸編輯頁面。</param>
        /// <param name="isSubPage">傳回是否為延伸頁面。</param>
        /// <returns>有對應的選單則傳回選單(功能)代碼，否則傳回 null。</returns>
        public virtual string GetMenuID(out bool isEditPage, out bool isSubPage)
        {
            isEditPage = _IsEditPage;
            isSubPage = _IsSubPage;
            if (_MenuID == null)
            {
                _MenuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage) ?? String.Empty;
                _IsEditPage = isEditPage;
                _IsSubPage = isSubPage;
            }
            return _MenuID;
        }

        private MenuInfo _MenuInfo = null;
        /// <summary>
        /// 取得目前頁面對應的選單(功能)資訊
        /// </summary>
        /// <param name="isEditPage">傳回是否為延伸編輯頁面。</param>
        /// <param name="isSubPage">傳回是否為延伸頁面。</param>
        /// <returns>有對應的選單則傳回選單(功能)資訊，否則傳回 null。</returns>
        public virtual MenuInfo GetMenuInfo(out bool isEditPage, out bool isSubPage)
        {
            isEditPage = _IsEditPage;
            isSubPage = _IsSubPage;
            if (_MenuInfo == null)
            {
                string menuID = this.GetMenuID(out isEditPage, out isSubPage);
                if (!String.IsNullOrEmpty(menuID))
                {
                    _MenuInfo = MenuHelper.Current.GetMenu(menuID);
                }
            }
            return _MenuInfo;
        }

        #region [Old]
        ///// <summary>
        ///// 取得目前頁面對應的選單(功能)名稱
        ///// </summary>
        ///// <returns>有對應的選單則傳回選單(功能)名稱，否則傳回 null。</returns>
        //public virtual string GetMenuName()
        //{
        //    bool isEditPage = false;
        //    bool isSubPage = false;
        //    MenuInfo menuInfo = this.GetMenuInfo(out isEditPage, out isSubPage);
        //    return menuInfo == null ? null : menuInfo.Name;
        //}
        #endregion

        private MenuInfo[] _HistoryMenus = null;
        /// <summary>
        /// 取得目前頁面對應的所有上層選單(功能)資訊陣列
        /// </summary>
        /// <param name="isEditPage">傳回是否為延伸編輯頁面。</param>
        /// <param name="isSubPage">傳回是否為延伸頁面。</param>
        /// <returns>傳回對應的所有上層選單(功能)資訊陣列。</returns>
        public virtual MenuInfo[] GetHistoryMenus(out bool isEditPage, out bool isSubPage)
        {
            isEditPage = _IsEditPage;
            isSubPage = _IsSubPage;
            if (_HistoryMenus == null)
            {
                string menuID = this.GetMenuID(out isEditPage, out isSubPage);
                if (!String.IsNullOrEmpty(menuID))
                {
                    _HistoryMenus = MenuHelper.Current.GetHistoryMenus(menuID);
                }
            }
            return _HistoryMenus;
        }
        #endregion

        #region 使用者操作日誌檔處理
        /// <summary>
        /// 取得觸發 PostBack 的控制項 ID
        /// </summary>
        /// <returns>找到則傳回的控制項 ID，否則傳回空字串</returns>
        protected string PostBackControlID()
        {
            Control control = null;
            string ctrlID = HttpUtility.HtmlEncode(Page.Request.Params.Get(EVENT_TARGET));
            if (!String.IsNullOrEmpty(ctrlID))
            {
                control = Page.FindControl(ctrlID);
            }
            else
            {
                //[TODO] 不懂這一段在幹嗎，不能找到 Button / ImageButton 就認為是他觸發的吧 ??
                string ctrlStr = String.Empty;
                Control c = null;
                foreach (string formValue in Page.Request.Form)
                {
                    string ctrl = HttpUtility.HtmlEncode(formValue);

                    //handle ImageButton they having an additional "quasi-property" in their Id which identifies
                    //mouse x and y coordinates
                    if (ctrl.EndsWith(".x") || ctrl.EndsWith(".y"))
                    {
                        ctrlStr = ctrl.Substring(0, ctrl.Length - 2);
                        c = Page.FindControl(ctrlStr);
                    }
                    else
                    {
                        c = Page.FindControl(ctrl);
                    }
                    if (c is System.Web.UI.WebControls.Button ||
                            c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }

            return control == null ? string.Empty : control.ID;
        }

        /// <summary>
        /// 記錄使用者操作日誌
        /// </summary>
        /// <param name="ex"></param>
        protected void OperationLog()
        {
            string path = ConfigManager.Current.GetProjectConfigValue("OperationLog", "Path");
            if (String.IsNullOrEmpty(path)) { return; }

            try
            {
                #region 檢查 Path
                System.IO.DirectoryInfo dinfo = new System.IO.DirectoryInfo(path);
                if (!dinfo.Exists)
                {
                    dinfo.Create();
                }
                #endregion

                DateTime now = DateTime.Now;
                LogonUser logonUser = this.GetLogonUser();

                #region 各層選單(功能)名稱
                string historyMenuName = null;
                string myMenuName = null;
                string myMenuID = null;
                {
                    bool isEditPage = false;
                    bool isSubPage = false;
                    MenuInfo[] menus = this.GetHistoryMenus(out isEditPage, out isSubPage);
                    if (menus != null && menus.Length > 0)
                    {
                        string[] names = new string[menus.Length];
                        for (int idx = 0; idx < menus.Length; idx++)
                        {
                            names[idx] = menus[idx].Name;
                        }
                        historyMenuName = String.Join(" > ", names);

                        MenuInfo myMenu = menus[menus.Length - 1];
                        myMenuName = myMenu.Name;
                        myMenuID = myMenu.ID;
                    }
                }
                #endregion

                #region 組日誌訊息
                StringBuilder log = new StringBuilder();
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}]", now).AppendLine();
                log.AppendFormat("頁面名稱:{0}; 頁面代碼:{1} ({2})", myMenuName, myMenuID, historyMenuName).AppendLine();
                log.AppendFormat("登入單位:{0} ({1}); 登入帳號:{2} ({3})", logonUser.UnitId, logonUser.UnitName, logonUser.UserId, logonUser.UserName).AppendLine();
                log.AppendFormat("操作控制項:{0}", this.PostBackControlID()).AppendLine();

                log.AppendLine();
                #endregion

                string logFile = System.IO.Path.Combine(path, String.Format("OperationLog_{0:yyyyMMdd}.log", now));
                System.IO.File.AppendAllText(logFile, log.ToString(), Encoding.Unicode);
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region [Old] 移至 LocalizedPage
        //#region 顯示 Javascript Alert 相關
        ///// <summary>
        ///// 顯示 Javascript 的 Alert 訊息
        ///// </summary>
        ///// <param name="msg">要顯示的訊息</param>
        //public void ShowJsAlert(string msg)
        //{
        //    this.ShowJsAlert(null, msg);
        //}

        ///// <summary>
        ///// 顯示 Javascript 的 Alert 訊息
        ///// </summary>
        ///// <param name="key">用戶端指令碼區塊索引鍵</param>
        ///// <param name="msg">要顯示的訊息</param>
        //public void ShowJsAlert(string key, string msg)
        //{
        //    if (String.IsNullOrWhiteSpace(msg))
        //    {
        //        return;
        //    }

        //    key = String.IsNullOrWhiteSpace(key) ? SHOW_JS_ALERT_KEY : key.Trim();
        //    StringBuilder js = new StringBuilder();
        //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(msg)).AppendLine();

        //    ClientScriptManager cs = this.ClientScript;
        //    Type myType = this.GetType();
        //    if (!cs.IsClientScriptBlockRegistered(myType, key))
        //    {
        //        cs.RegisterClientScriptBlock(myType, key, js.ToString(), true);
        //    }
        //}

        ///// <summary>
        ///// 顯示 Javascript 的 Alert 訊息，並前往指定網址
        ///// </summary>
        ///// <param name="msg">要顯示的訊息</param>
        ///// <param name="url">要前往的網址</param>
        //public void ShowJsAlertAndGoUrl(string msg, string url)
        //{
        //    if (String.IsNullOrWhiteSpace(msg))
        //    {
        //        return;
        //    }

        //    string key = SHOW_JS_ALERT_KEY;

        //    StringBuilder js = new StringBuilder();
        //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(msg)).AppendLine();
        //    if (!String.IsNullOrWhiteSpace(url))
        //    {
        //        js.AppendFormat("window.location.href = '{0}';", url).AppendLine();
        //    }

        //    ClientScriptManager cs = this.ClientScript;
        //    Type myType = this.GetType();
        //    if (!cs.IsClientScriptBlockRegistered(myType, key))
        //    {
        //        cs.RegisterClientScriptBlock(myType, key, js.ToString(), true);
        //    }
        //}

        ///// <summary>
        ///// 顯示請指定某欄位的 Alert 訊息
        ///// </summary>
        ///// <param name="fieldName">指定的欄位名稱，欄位名稱須自行 Localized</param>
        //public void ShowMustInputAlert(string fieldName)
        //{
        //    string ptn = this.GetLocalized("MSG_PTN_INPUT_MUST", "請指定「{0}」");
        //    string msg = String.Format(ptn, fieldName);
        //    this.ShowJsAlert(msg);
        //}

        ///// <summary>
        ///// 顯示請指定某控制項的欄位的 Alert 訊息
        ///// </summary>
        ///// <param name="ctlName">指定控制項的名稱</param>
        ///// <param name="ctlKey">指定控制項的欄位資源索引鍵</param>
        ///// <param name="defaultText">找不到資源時的回傳預設文字</param>
        //public void ShowMustInputAlert(string ctlName, string ctlKey, string defaultText)
        //{
        //    string fieldName = this.GetControlLocalized(ctlName, ctlKey, defaultText);
        //    this.ShowMustInputAlert(fieldName);
        //}

        ///// <summary>
        ///// 顯示某操作成功的 Alert 訊息
        ///// </summary>
        ///// <param name="action">指定操作名稱，須自行 Localized</param>
        ///// <param name="url">要前往的網址</param>
        //public void ShowActionSuccessAlert(string action, string url)
        //{
        //    string ptn = this.GetLocalized("MSG_PTN_ACTION_SUCCESS", "{0}成功");
        //    string msg = String.Format(ptn, action);
        //    this.ShowJsAlertAndGoUrl(msg, url);
        //}
        //#endregion
        #endregion

        #region [MDY:202203XX] 2022擴充案 取得商家代號是否啟用英文資料
        /// <summary>
        /// 取得指定商家代號是否啟用英文資料
        /// </summary>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="reflesh">指定是否刷新</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        protected bool IsEngEabled(string receiveType, bool reflesh = false)
        {
            if (String.IsNullOrWhiteSpace(receiveType))
            {
                return false;
            }

            string prefix = $"{receiveType.Trim()}=";

            string engEnabled = null;
            string cacheValue = ViewState["ReceiveTypeEngEabled"] as string;
            if (reflesh || String.IsNullOrEmpty(cacheValue) || !cacheValue.StartsWith(prefix))
            {
                object returnData = null;
                KeyValue<string>[] arguments = new KeyValue<string>[] {
                    new KeyValue<string>("ReceiveType", receiveType),
                };
                XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, Helpers.CallMethodName.GetReceiveTypeEngEabled, arguments, out returnData);
                if (xmlResult.IsSuccess)
                {
                    engEnabled = returnData as string;
                    ViewState["ReceiveTypeEngEabled"] = $"{prefix}{engEnabled}";
                }
                else
                {
                    return false;
                    //TODO：要不要寫 Log？
                }
            }
            else
            {
                engEnabled = cacheValue.Substring(prefix.Length);
            }
            return "Y".Equals(engEnabled);
        }

        /// <summary>
        /// 取得指定商家代號是否使用英文資料介面
        /// </summary>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="reflesh">指定是否刷新</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        protected bool UseEngDataUI(string receiveType, bool reflesh = false)
        {
            return this.isEngUI() && this.IsEngEabled(receiveType, false);
        }
        #endregion
    }
}
