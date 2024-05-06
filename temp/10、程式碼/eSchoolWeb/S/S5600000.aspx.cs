using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb.S
{
    /// <summary>
    /// 選單(功能)代碼 S56 的首頁 (系統設定管理的次選單(功能)頁面)
    /// </summary>
    public partial class S5600000 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ucSubMenu_MenuClick(object sender, SubMenuEventArgs e)
        {
            if (String.IsNullOrEmpty(e.MenuID) || String.IsNullOrEmpty(e.MenuUrl))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得點選的子選單參數");
                this.ShowJsAlert(msg);
                return;
            }

            Response.Redirect(this.GetResolveUrl(e.MenuUrl));
        }
    }
}