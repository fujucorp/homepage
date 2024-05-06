using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace eSchoolWeb
{
    /// <summary>
    /// 功能頁面介面
    /// </summary>
    public interface IMenuPage
    {
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        string MenuID
        {
            get;
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        string MenuName
        {
            get;
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        bool IsEditPage
        {
            get;
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        bool IsSubPage
        {
            get;
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        bool IsMatchMenuID
        {
            get;
        }
    }

    public interface ICreditCardPage
    {
        /// <summary>
        /// 銷帳編號
        /// </summary>
        string KeepCancelNo
        {
            get;
            set;
        }

        /// <summary>
        /// 交易總金額
        /// </summary>
        decimal KeepReceiveAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 持卡人身份證號
        /// </summary>
        string KeepPayerId
        {
            get;
            set;
        }

        /// <summary>
        /// 發卡銀行代碼
        /// </summary>
        string KeepBankId
        {
            get;
            set;
        }

        /// <summary>
        /// 學生身份證字號
        /// </summary>
        string KeepStudentPId
        {
            get;
            set;
        }

        /// <summary>
        /// 學生學號
        /// </summary>
        string KeepStudentNo
        {
            get;
            set;
        }

        /// <summary>
        /// 財金參數 - 特店代碼
        /// </summary>
        string KeepMerchantId
        {
            get;
            set;
        }

        /// <summary>
        /// 財金參數 - 端末機代號
        /// </summary>
        string KeepTerminalId
        {
            get;
            set;
        }

        /// <summary>
        /// 財金參數 - 特店編號
        /// </summary>
        string KeepMerId
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 自動處理 Localized 的網頁基底抽象類別 (不檢查權限)
    /// </summary>
    public abstract class LocalizedPage : Page, IMenuPage
    {
        #region Const
        /// <summary>
        /// 預設的 Javascript Alert 訊息 Key 的常數
        /// </summary>
        private const string SHOW_JS_ALERT_KEY = "SHOW_JS_ALERT";
        #endregion

        /// <summary>
        /// 顯示例外的錯誤訊息
        /// </summary>
        /// <param name="ex">發生的例外</param>
        private void SaveExceptionErrorMessage(Exception ex)
        {
            ////[TODO] 可以在這裡加儲存錯誤 Log
            //if (ex is HttpRequestValidationException)
            //{
            //    //[TODO] 要收集的錯誤代碼
            //    this.ShowSystemMessage("[WE001] 請勿輸入具有潛在危險的字串");
            //}
            //else if (ex is System.Net.WebException)
            //{
            //    //[TODO] 要收集的錯誤代碼
            //    this.ShowSystemMessage("[WE002] 資料存取服務連線失敗");
            //}
            //else
            //{
            //    //[TODO] 要收集的錯誤代碼
            //    this.ShowSystemMessage("[WE009] 網頁發生未預期錯誤");
            //}

            try
            {
                string logPath = System.Configuration.ConfigurationManager.AppSettings.Get("LOG_PATH");
                if (!String.IsNullOrEmpty(logPath))
                {
                    DateTime now = DateTime.Now;
                    string logFileName = String.Format("WebException_{0:yyyyMMdd}.log", now);
                    string logFileFullName = System.IO.Path.Combine(logPath, logFileName);
                    Entities.LogonUser logonUser = WebHelper.GetLogonUser();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb
                        .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 網頁執行發生例外 (LogonUser：LogonSN={1}, UnitId={2}, UnitName={3}, UserId={4}, UserName={5}, UserQual={6}, GroupId={7}, RoleType={8}, ReceiveType={9})", now, logonUser.LogonSN, logonUser.UnitId, logonUser.UnitName, logonUser.UserId, logonUser.UserName, logonUser.UserQual, logonUser.GroupId, logonUser.RoleType, logonUser.ReceiveType).AppendLine()
                        .AppendFormat("ex.Message = {0}", ex.Message).AppendLine()
                        .AppendFormat("ex.Source = {0}", ex.Source).AppendLine()
                        .AppendFormat("ex.TargetSite = {0}", ex.TargetSite).AppendLine()
                        .AppendFormat("ex.StackTrace = {0}", ex.StackTrace).AppendLine();
                    if (ex.InnerException != null)
                    {
                        Exception ex2 = ex.InnerException;
                        sb
                        .AppendFormat("InnerException.Message = {0}", ex2.Message).AppendLine()
                        .AppendFormat("InnerException.Source = {0}", ex2.Source).AppendLine()
                        .AppendFormat("InnerException.TargetSite = {0}", ex2.TargetSite).AppendLine()
                        .AppendFormat("InnerException.StackTrace = {0}", ex2.StackTrace).AppendLine();
                    }
                    sb.AppendLine();

                    System.IO.File.AppendAllText(logFileFullName, sb.ToString());
                }
            }
            catch (Exception)
            {
            }
        }

        #region Override Page
        /// <summary>
        /// 設定頁面目前執行緒的 System.Web.UI.Page.Culture 和 System.Web.UI.Page.UICulture
        /// </summary>
        protected override void InitializeCulture()
        {
            HttpCookie cookie = Request.Cookies["Localized"];
            if (cookie != null)
            {
                cookie.HttpOnly = true;
                CultureInfo currentInfo = CultureInfo.CurrentCulture;
                CultureInfo currentUIInfo = CultureInfo.CurrentUICulture;
                string localized = cookie.Value.Trim();
                switch (localized)
                {
                    case "zh-TW":
                    case "0x0404":
                    case "1028":
                        currentInfo = new CultureInfo("zh-TW");
                        currentUIInfo = CultureInfo.CreateSpecificCulture("zh-TW");
                        break;
                    case "en-US":
                    case "0x0409":
                    case "1033":
                        currentInfo = new CultureInfo("en-US");
                        currentUIInfo = CultureInfo.CreateSpecificCulture("en-US");
                        break;
                    case "ja-JP":
                    case "0x0411":
                    case "1041":
                        currentInfo = new CultureInfo("ja-JP");
                        currentUIInfo = CultureInfo.CreateSpecificCulture("ja-JP");
                        break;
                    case "zh-CN":
                    case "0x0804":
                    case "2052":
                        currentInfo = new CultureInfo("zh-CN");
                        currentUIInfo = CultureInfo.CreateSpecificCulture("zh-CN");
                        break;
                }
                Thread.CurrentThread.CurrentUICulture = currentInfo;
                Thread.CurrentThread.CurrentCulture = currentUIInfo;
            }

            base.InitializeCulture();
        }

        /// <summary>
        /// 引發頁面 Error 的事件處理
        /// </summary>
        /// <param name="e">事件參數資料</param>
        protected override void OnError(EventArgs e)
        {
            Exception ex = Server.GetLastError();
            //this.SaveExceptionErrorMessage(ex);
            WebHelper.SetErrorPageInfo(new ErrorPageInfo(null, ex));
            Server.ClearError();
            Response.Redirect("~/ErrorPage.aspx");
        }
        #endregion

        #region Implement IMenuPage
        private string _MenuID = null;
        private string _MenuName = null;
        private bool? _IsEditPage = false;
        private bool? _IsSubPage = false;
        private bool? _IsMatchMenuID = null;
        private MenuInfo _MenuInfo = null;

        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public virtual string MenuID
        {
            get
            {
                if (_MenuID == null)
                {
                    string fileName = Path.GetFileName(this.Request.CurrentExecutionFilePath);
                    bool isEditPage = false;
                    bool isSubPage = false;
                    _MenuID = WebHelper.GetFileMenuID(fileName, out isEditPage, out isSubPage);
                    if (String.IsNullOrEmpty(_MenuID))
                    {
                        _MenuID = Path.GetFileNameWithoutExtension(fileName);
                        _IsMatchMenuID = false;
                    }
                    else
                    {
                        _IsMatchMenuID = true;
                    }
                    _IsEditPage = isEditPage;
                    _IsSubPage = isSubPage;
                }
                return _MenuID;
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public virtual string MenuName
        {
            get
            {
                if (_MenuName == null)
                {
                    if (_MenuInfo == null && this.IsMatchMenuID)
                    {
                        _MenuInfo = MenuHelper.Current.GetMenu(this.MenuID);
                    }
                    _MenuName = _MenuInfo != null ? _MenuInfo.Name : this.MenuID;
                }
                return _MenuName;
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public virtual bool IsEditPage
        {
            get
            {
                if (_IsEditPage == null)
                {
                    string menuID = this.MenuID;
                }
                return _IsEditPage == null ? false : _IsEditPage.Value;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public virtual bool IsSubPage
        {
            get
            {
                if (_IsSubPage == null)
                {
                    string menuID = this.MenuID;
                }
                return _IsSubPage == null ? false : _IsSubPage.Value;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public virtual bool IsMatchMenuID
        {
            get
            {
                if (_IsMatchMenuID == null)
                {
                    string menuID = this.MenuID;
                }
                return _IsMatchMenuID == null ? false : _IsMatchMenuID.Value;
            }
        }
        #endregion

        #region Localized 相關
        #region [MDY:2018xxxx] 避免原碼掃描誤判
        /// <summary>
        /// 取得指定文字的 Localized 並做 HtmlEncode (多語系翻譯)
        /// </summary>
        /// <param name="text">指定文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回空字串</returns>
        public string GetHtmlEncodeLocalized(string text)
        {
            return WebHelper.GetHtmlEncodeLocalized(text);
        }
        #endregion

        /// <summary>
        /// 取得指定文字的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="text">指定文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回指定文字或空字串</returns>
        public string GetLocalized(string text)
        {
            return WebHelper.GetLocalized(text);
        }

        /// <summary>
        /// 取得指定資源索引鍵的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="key">指定資源索引鍵</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回預設文字或空字串</returns>
        public string GetLocalized(string resourceKey, string defaultText)
        {
            return WebHelper.GetLocalized(resourceKey, defaultText);
        }

        /// <summary>
        /// 取得指定資源索引鍵的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="key">指定資源索引鍵</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回預設文字或空字串</returns>
        public string GetControlLocalized(string ctlName, string ctlKey, string defaultText)
        {
            string resourceKey = String.Concat(ctlName, "_", ctlKey);
            return WebHelper.GetControlLocalizedByResourceKey(resourceKey, defaultText);
        }

        /// <summary>
        /// 取得指定錯誤代碼的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="code">指定錯誤代碼</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回預設文字或空字串</returns>
        public string GetErrorLocalized(string code, string defaultText)
        {
            return WebHelper.GetLocalized("ERR_" + code, defaultText);
        }
        #endregion

        #region 顯示系統訊息相關
        /// <summary>
        /// 顯示純文字的系統訊息，html 會被 Encode
        /// </summary>
        /// <param name="msg">指定要顯示的訊息，須自行 Localized</param>
        /// <param name="fgAlert">指定是否也顯示 Javascript 的 Alert 訊息</param>
        public void ShowSystemMessage(string msg, bool fgAlert = true)
        {
            if (String.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            Main master = this.Master as Main;
            if (master != null)
            {
                master.ShowMessage(HttpUtility.HtmlEncode(msg));
                if (fgAlert)
                {
                    this.ShowJsAlert(msg);
                }
            }
            else
            {
                this.ShowJsAlert(msg);
            }
        }

        /// <summary>
        /// 顯示純文字的指定錯誤代碼與錯誤說明的系統訊息
        /// </summary>
        /// <param name="code">指定錯誤代碼</param>
        /// <param name="text">指定錯誤說明，須自行 Localized，未指定時由錯誤代碼決定錯誤訊息</param>
        /// <param name="fgAlert">指定是否也顯示 Javascript 的 Alert 訊息</param>
        public void ShowSystemMessage(string code, string text, bool fgAlert = true)
        {
            bool hasCode = !String.IsNullOrWhiteSpace(code);
            bool hasText = !String.IsNullOrWhiteSpace(text);
            if (hasCode || hasText)
            {
                Main master = this.Master as Main;
                if (master != null)
                {
                    string msg = master.ShowMessage(code, HttpUtility.HtmlEncode(text));
                    if (fgAlert)
                    {
                        this.ShowJsAlert(msg);
                    }
                }
                else
                {
                    if (!hasText)
                    {
                        text = this.GetErrorLocalized(code, String.Format("發生代碼 {0} 的錯誤", code));
                    }
                    this.ShowJsAlert(text);
                }
            }
        }

        #region [Old]
        ///// <summary>
        ///// 顯示指定處理結果的系統訊息
        ///// </summary>
        ///// <param name="xmlResult">指定處理結果</param>
        ///// <param name="fgAlert">指定是否也顯示 Javascript 的 Alert 訊息</param>
        //public void ShowSystemMessage(XmlResult xmlResult, bool fgAlert = true)
        //{
        //    if (xmlResult == null)
        //    {
        //        return;
        //    }
        //    this.ShowSystemMessage(xmlResult.Code, xmlResult.Message, fgAlert);
        //}
        #endregion

        /// <summary>
        /// 顯示純文字的指定錯誤代碼與錯誤說明的錯誤訊息
        /// </summary>
        /// <param name="code">指定錯誤代碼</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        /// <param name="fgAlert">指定是否也顯示 Javascript 的 Alert 訊息</param>
        public void ShowErrorMessage(string code, string defaultText, bool fgAlert = true)
        {
            if (String.IsNullOrWhiteSpace(code))
            {
                if (!String.IsNullOrWhiteSpace(defaultText))
                {
                    this.ShowSystemMessage(defaultText, fgAlert);
                }
            }
            else
            {
                code = code.Trim();
                string text = null;
                if (String.IsNullOrWhiteSpace(defaultText))
                {
                    text = this.GetErrorLocalized(code, String.Format("發生代碼 {0} 的錯誤", code));
                }
                else
                {
                    text = this.GetErrorLocalized(code, defaultText);
                }
                this.ShowSystemMessage(code, text, fgAlert);
            }
        }

        /// <summary>
        /// 顯示某操作失敗的系統訊息
        /// </summary>
        /// <param name="action">指定操作名稱，須自行 Localized</param>
        /// <param name="desc">指定失敗描述，須自行 Localized</param>
        public void ShowActionFailureMessage(string action, string desc)
        {
            string ptn = null;
            string msg = null;
            if (String.IsNullOrWhiteSpace(desc))
            {
                ptn = this.GetLocalized("MSG_PTN_ACTION_FAILURE", "{0}失敗");
                msg = String.Format(ptn, action);
            }
            else
            {
                ptn = this.GetLocalized("MSG_PTN_ACTION_FAILURE_DESC", "{0}失敗，{1}");
                msg = String.Format(ptn, action, desc);
            }
            this.ShowSystemMessage(msg);
        }

        /// <summary>
        /// 顯示某操作失敗的系統訊息
        /// </summary>
        /// <param name="action">指定操作名稱，須自行 Localized</param>
        /// <param name="code">指定錯誤代碼</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        public void ShowActionFailureMessage(string action, string code, string defaultText)
        {
            string desc = this.GetErrorLocalized(code, defaultText);
            this.ShowActionFailureMessage(action, desc);
        }

        /// <summary>
        /// 顯示某操作成功的系統訊息
        /// </summary>
        /// <param name="action">指定操作名稱，須自行 Localized</param>
        public void ShowActionSuccessMessage(string action)
        {
            string ptn = this.GetLocalized("MSG_PTN_ACTION_SUCCESS", "{0}成功");
            string msg = String.Format(ptn, action);
            this.ShowJsAlert(msg);
        }
        #endregion

        #region 顯示 Javascript Alert 相關
        /// <summary>
        /// 顯示 Javascript 的 Alert 訊息
        /// </summary>
        /// <param name="msg">要顯示的訊息</param>
        public void ShowJsAlert(string msg)
        {
            this.ShowJsAlert(null, msg);
        }

        /// <summary>
        /// 顯示 Javascript 的 Alert 訊息
        /// </summary>
        /// <param name="key">用戶端指令碼區塊索引鍵</param>
        /// <param name="msg">要顯示的訊息</param>
        public void ShowJsAlert(string key, string msg)
        {
            if (String.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            key = String.IsNullOrWhiteSpace(key) ? SHOW_JS_ALERT_KEY : key.Trim();
            StringBuilder js = new StringBuilder();
            js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(msg)).AppendLine();

            ClientScriptManager cs = this.ClientScript;
            Type myType = this.GetType();
            if (!cs.IsClientScriptBlockRegistered(myType, key))
            {
                cs.RegisterClientScriptBlock(myType, key, js.ToString(), true);
            }
        }

        /// <summary>
        /// 顯示 Javascript 的 Alert 訊息，並前往指定網址
        /// </summary>
        /// <param name="msg">要顯示的訊息</param>
        /// <param name="url">要前往的網址</param>
        public void ShowJsAlertAndGoUrl(string msg, string url)
        {
            if (String.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            string key = SHOW_JS_ALERT_KEY;

            StringBuilder js = new StringBuilder();
            js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(msg)).AppendLine();
            if (!String.IsNullOrWhiteSpace(url))
            {
                js.AppendFormat("window.location.href = '{0}';", HttpUtility.JavaScriptStringEncode(url)).AppendLine();
            }

            ClientScriptManager cs = this.ClientScript;
            Type myType = this.GetType();
            if (!cs.IsClientScriptBlockRegistered(myType, key))
            {
                cs.RegisterClientScriptBlock(myType, key, js.ToString(), true);
            }
        }

        /// <summary>
        /// 顯示請指定某欄位的 Alert 訊息
        /// </summary>
        /// <param name="fieldName">指定的欄位名稱，欄位名稱須自行 Localized</param>
        public void ShowMustInputAlert(string fieldName)
        {
            string ptn = this.GetLocalized("MSG_PTN_INPUT_MUST", "請指定「{0}」");
            string msg = String.Format(ptn, fieldName);
            this.ShowJsAlert(msg);
        }

        /// <summary>
        /// 顯示請指定某控制項的欄位的 Alert 訊息
        /// </summary>
        /// <param name="ctlName">指定控制項的名稱</param>
        /// <param name="ctlKey">指定控制項的欄位資源索引鍵</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字</param>
        public void ShowMustInputAlert(string ctlName, string ctlKey, string defaultText)
        {
            string fieldName = this.GetControlLocalized(ctlName, ctlKey, defaultText);
            this.ShowMustInputAlert(fieldName);
        }

        /// <summary>
        /// 顯示某操作成功的 Alert 訊息
        /// </summary>
        /// <param name="action">指定操作名稱，須自行 Localized</param>
        /// <param name="url">要前往的網址</param>
        public void ShowActionSuccessAlert(string action, string url)
        {
            string ptn = this.GetLocalized("MSG_PTN_ACTION_SUCCESS", "{0}成功");
            string msg = String.Format(ptn, action);
            this.ShowJsAlertAndGoUrl(msg, url);
        }
        #endregion

        #region [MDY:20210325] 原碼修正 取得用戶端可使用 Url 相關 Method
        /// <summary>
        /// 取得指定網址的用戶端可用網址
        /// </summary>
        /// <param name="url">指定網址。未指定或指定 javascript: 則傳回空字串</param>
        /// <returns>傳回用戶端可用網址</returns>
        protected string GetResolveUrl(string url)
        {
            #region [MDY:20210401] 原碼修正
            #region [OLD]
            //if (String.IsNullOrEmpty(url)
            //    || url.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    return String.Empty;
            //}
            //if (url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) 
            //    || url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    return HttpUtility.JavaScriptStringEncode(url);
            //}

            //#region [MDY:20200705] 修正特殊字串路徑 "/(Z(%22onerror=%22alert'XSS'%22))/" 的 XSS 問題
            //#region [OLD]
            ////if (url.StartsWith("~/"))
            ////{
            ////    return this.ResolveUrl(url);
            ////}
            ////if (url.StartsWith("/"))
            ////{
            ////    return this.ResolveUrl("~" + url);
            ////}
            ////return this.ResolveUrl("~/" + url);
            //#endregion

            //if (url.StartsWith("/"))
            //{
            //    return HttpUtility.JavaScriptStringEncode(String.Concat(Request.ApplicationPath, url.Substring(1)));
            //}
            //else if (url.StartsWith("~/"))
            //{
            //    return HttpUtility.JavaScriptStringEncode(String.Concat(Request.ApplicationPath, url.Substring(2)));
            //}
            //else if (url.StartsWith("./"))
            //{
            //    return HttpUtility.JavaScriptStringEncode(String.Concat(String.Join("", Request.Url.Segments, 0, Request.Url.Segments.Length - 1), url.Substring(2)));
            //}
            //else if (url.StartsWith("../"))
            //{
            //    return HttpUtility.JavaScriptStringEncode(String.Concat(String.Join("", Request.Url.Segments, 0, Request.Url.Segments.Length - 2), url.Substring(2)));
            //}
            //else
            //{
            //    return HttpUtility.JavaScriptStringEncode(String.Concat(Request.ApplicationPath, url));
            //}
            //#endregion
            #endregion

            if (String.IsNullOrEmpty(url)
                || url.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase))
            {
                return String.Empty;
            }
            if (url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)
                || url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                return Uri.EscapeUriString(url);
            }

            #region [MDY:20200705] 修正特殊字串路徑 "/(Z(%22onerror=%22alert'XSS'%22))/" 的 XSS 問題
            if (url.StartsWith("/"))
            {
                return Uri.EscapeUriString(String.Concat(Request.ApplicationPath, url.Substring(1)));
            }
            else if (url.StartsWith("~/"))
            {
                return Uri.EscapeUriString(String.Concat(Request.ApplicationPath, url.Substring(2)));
            }
            else if (url.StartsWith("./"))
            {
                return Uri.EscapeUriString(String.Concat(String.Join("", Request.Url.Segments, 0, Request.Url.Segments.Length - 1), url.Substring(2)));
            }
            else if (url.StartsWith("../"))
            {
                return Uri.EscapeUriString(String.Concat(String.Join("", Request.Url.Segments, 0, Request.Url.Segments.Length - 2), url.Substring(2)));
            }
            else
            {
                return Uri.EscapeUriString(String.Concat(Request.ApplicationPath, url));
            }
            #endregion
            #endregion
        }
        #endregion

        #region 其他 Method
        /// <summary>
        /// 取得主機名稱
        /// </summary>
        /// <returns></returns>
        public string GetHostName()
        {
            return Convert.ToString(Request.ServerVariables["HTTP_HOST"]);
        }

        ///// <summary>
        ///// 取得選單資訊
        ///// </summary>
        ///// <param name="isEditPage"></param>
        ///// <param name="isSubPage"></param>
        ///// <returns></returns>
        //public virtual MenuInfo GetMenuInfo(out bool isEditPage, out bool isSubPage)
        //{
        //    if (_MenuInfo == null && this.IsMatchMenuID)
        //    {
        //        _MenuInfo = MenuHelper.Current.GetMenu(this.MenuID);
        //    }
        //    if (_MenuInfo == null)
        //    {
        //        _MenuInfo = new MenuInfo(this.MenuID, this.MenuName, String.Empty, this.Request.Url.AbsoluteUri, 0);
        //    }
        //    isEditPage = this.IsEditPage;
        //    isSubPage = this.IsSubPage;
        //    return _MenuInfo;
        //}
        #endregion

        #region ResponseFile
        /// <summary>
        /// 回傳檔案內容
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        protected void ResponseFile(string fileName, byte[] content, string fileType = null)
        {
            #region [MDY:20210401] 原碼修正
            fileName = HttpUtility.HtmlEncode(fileName.Replace("\n", "").Replace("\r", ""));
            #endregion

            string browser = this.Request.Browser.Browser.ToUpper();
            if (browser == "IE" || browser == "INTERNETEXPLORER")
            {
                fileName = HttpUtility.UrlPathEncode(fileName);
            }
            if (String.IsNullOrEmpty(fileType))
            {
                fileType = Path.GetExtension(fileName);
            }
            else
            {
                fileType = fileType.Trim();
            }
            this.Response.Clear();
            this.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            this.Response.AddHeader("Content-Language", "utf-8");
            this.Response.ContentType = this.GetContentType(fileType);
            this.Response.BinaryWrite(content);
            this.Response.End();
        }

        protected string GetContentType(string extName)
        {
            extName = extName == null ? string.Empty : extName.Trim().ToUpper();
            switch (extName)
            {
                case "PDF":
                    return "application/pdf";
                case "TXT":
                    return "text/plain";
                case "XLS":
                    return "application/vnd.ms-excel";
                case "MDB":
                    return "application/vnd.ms-access";
                case "DOC":
                    return "application/msword";
            }
            return "application/octet-stream";
        }
        #endregion

        #region [MDY:202203XX] 2022擴充案 取得商家代號是否啟用英文資料
        /// <summary>
        /// 取得是否使用英文介面
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        protected bool isEngUI()
        {
            return "en-US".Equals(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(), StringComparison.CurrentCultureIgnoreCase);
        }
        #endregion
    }
}