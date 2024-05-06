using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 登入後左側選單的使用者控制項
    /// </summary>
    public partial class UCPageMenu : BaseUserControl
    {
        private MenuHelper _Helper = MenuHelper.Current;

        /// <summary>
        /// 取得登入者資料
        /// </summary>
        private LogonUser _LogonUser = WebHelper.GetLogonUser();

        /// <summary>
        /// 產生主選單介面
        /// </summary>
        private void GenMenuUI()
        {
            MenuInfo[] menus = _Helper.GetMainMenus();
            if (menus != null && menus.Length > 0)
            {
                StringBuilder html = new StringBuilder();
                html.AppendLine("<ul>");
                foreach (MenuInfo menu in menus)
                {
                    //檢查是否有授權
                    bool hasAuth = _LogonUser.IsAuthMenuID(menu.ID);

                    string name = WebHelper.GetMenuLocalized(menu.ID, menu.Name);
                    if (hasAuth)
                    {
                        bool isOtherWeb = false;
                        string url = this.GetLinkResolveUrl(menu.Url, out isOtherWeb);
                        string subMenuHtml = this.GenL2MenuUIHtml(menu.ID);
                        html
                            .AppendFormat("<li id=\"li_{0}\" title=\"{0}\">", menu.ID).AppendLine()
                            .AppendFormat("<a href=\"{0}\" {2} >{1}</a>", url, name, (isOtherWeb ? "target=\"OtherWeb\"" : String.Empty)).AppendLine()
                            .AppendLine(subMenuHtml)
                            .AppendLine("</li>");
                    }
                    else
                    {
                        //沒權限的項目土地銀行不顯示
                        ////沒有授權不用處理第二層選單滑鼠事件
                        //html.AppendFormat("<li id=\"li_{0}\" title=\"此功能未授權\"><a href=\"javascript:void(0)\" >{1}</a></li>", menu.ID, name).AppendLine();
                    }
                }
                html.AppendLine("</ul>");

                this.litMenu.Text = html.ToString();
            }
            else
            {
                this.litMenu.Text = String.Empty;
            }
        }

        /// <summary>
        /// 產生指定主選單代碼的子選單介面的 Html
        /// </summary>
        /// <param name="parentID">指定主選單代碼</param>
        /// <returns>傳回次選單介面的 Html 或空字串</returns>
        private string GenL2MenuUIHtml(string parentID)
        {
            MenuInfo[] menus = _Helper.GetChildMenus(parentID);
            if (menus != null && menus.Length > 0)
            {
                StringBuilder html = new StringBuilder();
                html.AppendLine("<ul>");
                foreach (MenuInfo menu in menus)
                {
                    //檢查是否有授權
                    bool hasAuth = _LogonUser.IsAuthMenuID(menu.ID);

                    string name = WebHelper.GetMenuLocalized(menu.ID, menu.Name);
                    if (hasAuth)
                    {
                        bool isOtherWeb = false;
                        string url = this.GetLinkResolveUrl(menu.Url, out isOtherWeb);
                        html.AppendFormat("<li id=\"li_{0}\" title=\"{0}\"><a href=\"{1}\" {3} >{2}</a></li>", menu.ID, url, name, (isOtherWeb ? "target=\"OtherWeb\"" : String.Empty)).AppendLine();
                    }
                    else
                    {
                        //沒權限的項目土地銀行不顯示
                        //html.AppendFormat("<li id=\"li_{0}\" title=\"此功能未授權\"><a href=\"javascript:void(0)\" >{1}</a></li>", menu.ID, name).AppendLine();
                    }
                }
                html.AppendLine("</ul>");

                return html.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.GenMenuUI();
            }
        }
    }
}