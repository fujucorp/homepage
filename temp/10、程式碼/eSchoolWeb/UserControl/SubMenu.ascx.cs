using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;

namespace eSchoolWeb
{
    #region EventArgs
    /// <summary>
    /// SubMenu 使用者控制項的事件資料類別
    /// </summary>
    public class SubMenuEventArgs : EventArgs
    {
        #region Property
        private string _MenuID = null;
        /// <summary>
        /// 選單(功能)代碼
        /// </summary>
        public string MenuID
        {
            get
            {
                return _MenuID;
            }
            set
            {
                _MenuID = value == null ? null : value.Trim();
            }
        }

        private string _MenuName = null;
        /// <summary>
        /// 選單(功能)名稱
        /// </summary>
        public string MenuName
        {
            get
            {
                return _MenuName;
            }
            set
            {
                _MenuName = value == null ? null : value.Trim();
            }
        }

        private string _MenuUrl = null;
        /// <summary>
        /// 選單(功能)網址
        /// </summary>
        public string MenuUrl
        {
            get
            {
                return _MenuUrl;
            }
            set
            {
                _MenuUrl = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 SubMenu 使用者控制項的事件資料類別
        /// </summary>
        public SubMenuEventArgs()
        {
        }

        /// <summary>
        /// 建構 SubMenu 使用者控制項的事件資料類別
        /// </summary>
        /// <param name="menuID">選單(功能)代碼</param>
        /// <param name="menuName">選單(功能)名稱</param>
        /// <param name="menuUrl">選單(功能)網址</param>
        public SubMenuEventArgs(string menuID, string menuName, string menuUrl)
        {
            this.MenuID = menuID;
            this.MenuName = menuName;
            this.MenuUrl = menuUrl;
        }

        /// <summary>
        /// 建構 SubMenu 使用者控制項的事件資料類別
        /// </summary>
        /// <param name="menu">選單資訊</param>
        public SubMenuEventArgs(MenuInfo menu)
        {
            if (menu != null)
            {
                this.MenuID = menu.ID;
                this.MenuName = menu.Name;
                this.MenuUrl = menu.Url;
            }
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// 登入後主內容次選單的使用者控制項
    /// </summary>
    [DefaultProperty("SubMenuID")]
    [DefaultEvent("MenuClick")]
    public partial class SubMenu : BaseUserControl, IPostBackEventHandler
    {
        #region
        private const int DefaultRepeatColumns = 3;
        #endregion

        #region Property
        private string _SubMenuID = null;
        /// <summary>
        /// 要顯示的次選單(功能)代碼
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        public string SubMenuID
        {
            get
            {
                return _SubMenuID;
            }
            set
            {
                _SubMenuID = value == null ? String.Empty : value;
            }
        }

        /// <summary>
        /// 點選子選單(功能)時是否自動回傳事件
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        public bool AutoPostBack
        {
            get
            {
                object value = ViewState["AutoPostBack"];
                if (value is bool)
                {
                    return (bool)value;
                }
                return true;
            }
            set
            {
                ViewState["AutoPostBack"] = value;
            }
        }

        /// <summary>
        /// 控制項中可顯示的行數
        /// </summary>
        [DefaultValue(DefaultRepeatColumns)]
        [Themeable(false)]
        public int RepeatColumns
        {
            get
            {
                object value = ViewState["RepeatColumns"];
                if (value is int)
                {
                    return (int)value;
                }
                return DefaultRepeatColumns;
            }
            set
            {
                ViewState["RepeatColumns"] = value < 1 ? 1 :value;
            }
        }
        #endregion

        #region 事件與處理常式
        /// <summary>
        /// 發生於按一下子選單控制項時
        /// </summary>
        public event EventHandler<SubMenuEventArgs> MenuClick = null;

        /// <summary>
        /// 引發選單控制項的 Click 事件
        /// </summary>
        /// <param name="e">Click 事件資料</param>
        protected virtual void OnMenuClick(SubMenuEventArgs e)
        {
            if (this.MenuClick != null)
            {
                this.MenuClick(this, e);
            }
        }
        #endregion

        #region Implement IPostBackEventHandler's Method
        /// <summary>
        /// 定義 ASP.NET 伺服器控制項必須實作以處理回傳事件的方法
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            string childMenuID = eventArgument;
            if (!string.IsNullOrEmpty(childMenuID))
            {
                #region 如果未指定 SubMenuID，則取頁面可能對應的選單代碼
                string menuID = null;
                if (String.IsNullOrEmpty(this.SubMenuID))
                {
                    bool isEditPage = false;
                    bool isSubPage = false;
                    menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
                }
                else
                {
                    menuID = this.SubMenuID;
                }
                #endregion

                MenuHelper helper = MenuHelper.Current;
                MenuInfo menu = helper.GetMenu(childMenuID);
                SubMenuEventArgs e = new SubMenuEventArgs(menu);
                this.OnMenuClick(e);
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 產生次選單介面
        /// </summary>
        private void GenSubMenuUI()
        {
            #region 如果未指定 SubMenuID，則取頁面可能對應的選單代碼
            string menuID = null;
            if (String.IsNullOrEmpty(this.SubMenuID))
            {
                bool isEditPage = false;
                bool isSubPage = false;
                menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
            }
            else
            {
                menuID = this.SubMenuID;
            }
            #endregion

            #region 取得登入者資料，並檢查是否有授權
            LogonUser logonUser = WebHelper.GetLogonUser();
            if (!logonUser.IsAuthMenuID(menuID))
            {
                this.litSubMenu.Text = "此頁面未授權";
                return;
            }
            #endregion

            #region 取得子選單資料
            MenuInfo[] menus = null;
            if (!String.IsNullOrEmpty(menuID))
            {
                MenuHelper helper = MenuHelper.Current;
                menus = helper.GetChildMenus(menuID);
            }
            #endregion

            #region Html
            if (menus != null && menus.Length > 0)
            {
                ClientScriptManager cs = this.Page.ClientScript;

                bool isPostBack = this.AutoPostBack;
                int maxRowCount = this.RepeatColumns;
                StringBuilder html = new StringBuilder();
                html.AppendLine("<table class=\"index\">");
                int rowCount = 0;
                foreach (MenuInfo menu in menus)
                {
                    if (rowCount == 0)
                    {
                        html.AppendLine("<tr>");
                    }

                    //檢查是否有授權
                    bool hasAuth = logonUser.IsAuthMenuID(menu.ID);
                    string name = WebHelper.GetMenuLocalized(menu.ID, menu.Name);
                    if (hasAuth)
                    {
                        if (isPostBack)
                        {
                            string lnk = this.GetPostBackLinkHtml(cs, menu.ID, name, menu.Url);
                            html.AppendFormat("<td>{0}</td>", lnk);
                        }
                        else
                        {
                            bool isOtherWeb = false;
                            string url = this.GetLinkResolveUrl(menu.Url, out isOtherWeb);
                            html.AppendFormat("<td><a href=\"{0}\" title=\"{3}\" {2}>{1}</a></td>", url, name, (isOtherWeb ? "target=\"OtherWeb\"" : String.Empty), menu.ID);
                        }

                        rowCount = (rowCount + 1) % maxRowCount;
                        if (rowCount == 0)
                        {
                            html.AppendLine("</tr>");
                        }
                    }
                    else
                    {
                        //沒權限的項目土地銀行不顯示
                        //html.AppendFormat("<td>{0}</td>", name);
                    }
                }
                if (rowCount != 0)
                {
                    html.AppendLine("</tr>");
                }
                html.AppendLine("</table>");

                this.litSubMenu.Text = html.ToString();
            }
            else
            {
                this.litSubMenu.Text = String.Empty;
            }
            #endregion
        }

        /// <summary>
        /// 取得 PostBack 的 Link Html
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="menuID"></param>
        /// <param name="menuName"></param>
        /// <param name="menuUrl"></param>
        /// <returns></returns>
        private string GetPostBackLinkHtml(ClientScriptManager cs, string menuID, string menuName, string menuUrl)
        {
            if (String.IsNullOrEmpty(menuUrl))
            {
                return String.Format("<a href=\"javascript:void(0)\" title=\"{1}\">{0}</a>", menuName, menuID);
            }

            if (menuUrl.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase))
            {
                return String.Format("<a href=\"{0}\" title=\"{2}\">{1}</a></td>", menuUrl, menuName, menuID);
            }

            if (menuUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)
                || menuUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                return String.Format("<a href=\"{0}\" title=\"{2}\" target=\"OtherWeb\">{1}</a>", menuUrl, menuName, menuID);
            }

            string jsClick = cs.GetPostBackEventReference(this, menuID);
            return String.Format("<a href=\"javascript:void(0)\" onclick=\"{0}\" title=\"{2}\">{1}</a>", jsClick, menuName, menuID);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.GenSubMenuUI();
            }
        }
    }
}