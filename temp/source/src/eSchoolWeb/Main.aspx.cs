using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 登入後首頁
    /// </summary>
    public partial class Main1 : BasePage
    {
        #region Override BasePage
        /// <summary>
        /// 取得是否需要檢查是否有任何授權，此頁面為登入後的首頁，沒有功能代碼，不檢查
        /// </summary>
        public override bool IsNeedCheckAuth
        {
            get
            {
                return false;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //這個頁面不顯示 SubHeader1 與 SubHeader2
            Main master = (this.Master as Main);
            if (master != null)
            {
                Control control = null;
                control = master.FindControl("ucSubHeader1");
                if (control != null)
                {
                    control.Visible = false;
                }
                control = master.FindControl("ucSubHeader2");
                if (control != null)
                {
                    control.Visible = false;
                }
            }

            LogonUser logonUser = WebHelper.GetLogonUser();

            #region [MDY:20220530] Checkmarx 調整
            if (logonUser != null && !String.IsNullOrEmpty(logonUser.LogonSN))
            {
                this.cclitWelcome.Visible = true;

                //[TODO] 更改密碼頁面未定
                //SAMPLE: <p><span>提醒您～您已經6個月未更改密碼。</span><a href="javascript:void(0)">→立即更改密碼</a></p>
                if (logonUser.IsRemindChangePXX)
                {
                    string remind1 = this.GetLocalized("RemindChangePWD1", "提醒您～您已經6個月未更改密碼。");
                    string remind2 = this.GetLocalized("RemindChangePWD2", "立即更改密碼");
                    this.litChangePXX.Text = String.Format("<p><span>{0}</span><a href=\"{2}\">→{1}</a></p>", remind1, remind2, this.GetResolveUrl("~/P/P7100001.aspx"));
                    this.litChangePXX.Visible = true;
                    logonUser.IsRemindChangePXX = false;
                }
                else
                {
                    this.litChangePXX.Visible = false;
                }
            }
            else
            {
                this.cclitWelcome.Visible = false;
                this.litChangePXX.Visible = false;
            }
            #endregion
        }
    }
}