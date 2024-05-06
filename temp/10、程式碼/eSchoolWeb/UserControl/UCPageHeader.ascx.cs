using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju.Configuration;

namespace eSchoolWeb
{
    /// <summary>
    /// 頁首的使用者控制項
    /// </summary>
    public partial class UCPageHeader : BaseUserControl
    {
        private void InitialUI()
        {
            this.labReCountDown.Text = this.GetLocalized("CTL_UCPageHeader_ReCountDown", "重新計時");
            this.labLogout.Text = this.GetLocalized("CTL_UCPageHeader_Logout", "登出");
            this.Label1.Text = this.GetLocalized("重新計時");
        }

        /// <summary>
        /// 取得頁面操作逾時的倒數秒數
        /// </summary>
        /// <returns></returns>
        public int GetPageOperatingTimeoutSeconds()
        {
            string value = ConfigManager.Current.GetProjectConfigValue("PageOperating", "TimeoutSeconds");
            int seconds = 0;
            if (Int32.TryParse(value, out seconds) && seconds > 0)
            {
                return seconds;
            }
            return 0;
        }

        /// <summary>
        /// 取得頁面操作逾時的顯示提醒的剩餘秒數
        /// </summary>
        /// <returns></returns>
        public int GetPageOperatingRemindSeconds()
        {
            string value = ConfigManager.Current.GetProjectConfigValue("PageOperating", "RemindSeconds");
            int seconds = 0;
            if (Int32.TryParse(value, out seconds) && seconds > 0)
            {
                return seconds;
            }
            return 0;
        }

        /// <summary>
        /// 取得檢查登入狀態 api 的網址
        /// </summary>
        /// <returns></returns>
        public string GetCheckLogonApiUrl()
        {
            return this.GetResolveUrl("~/api/CheckLogon.ashx");
        }

        /// <summary>
        /// 取得登出頁面的網址
        /// </summary>
        /// <returns></returns>
        public string GetLogoutUrl()
        {
            return this.GetResolveUrl("~/Logout.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }
    }
}