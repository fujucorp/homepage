using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.P
{
    /// <summary>
    /// 個人資料修改
    /// </summary>
    public partial class P7100001 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogonUser logonUser = this.GetLogonUser();
            if (logonUser == null || logonUser.IsAnonymous)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("未登入或頁面逾時");
                this.ShowJsAlert(msg);
                this.ccbtnOK.Visible = false;
                return;
            }
            if (!logonUser.IsSchoolUser)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("非學校帳號，無法變更密碼");
                this.ShowJsAlert(msg);
                this.ccbtnOK.Visible = false;
                return;
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region [MDY:20220530] Checkmarx 調整
            string oldPXX = this.tbxOldPXX.Text.Trim();
            string newPXX = this.tbxNewPXX.Text.Trim();
            string chkPXX = this.tbxChkPXX.Text.Trim();
            if (String.IsNullOrEmpty(oldPXX))
            {
                this.ShowMustInputAlert("舊的密碼");
                return;
            }
            if (String.IsNullOrEmpty(newPXX))
            {
                this.ShowMustInputAlert("新的密碼");
                return;
            }
            if (newPXX != chkPXX)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("確認密碼與新的密碼不同");
                this.ShowSystemMessage(msg);
                return;
            }
            if (newPXX == oldPXX)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("新舊密碼不可相同");
                this.ShowSystemMessage(msg);
                return;
            }

            #region [MDY:20181206] 改為 不可3個以上(連續)相同或連號的英數字元 (20181201_03)
            if (!DataFormat.CheckUserPXXFormat(newPXX))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized(String.Format("「使用者密碼」限輸入 {0} ~ {1} 碼英數字混合字串，且不可含連續3個(或以上)相同或連號的英文或數字", DataFormat.UserPXXMinSize, DataFormat.UserPXXMaxSize));
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            LogonUser logonUser = this.GetLogonUser();

            #region [MDY:20181206] 增加不可與帳號相同 (20181201_03)
            if (newPXX.Equals(logonUser.UserId))
            {
                this.ShowJsAlert("密碼不可與帳號相同");
                return;
            }
            #endregion

            XmlResult xmlResult = DataProxy.Current.ChangeUserPXX(this, logonUser.UnitId, logonUser.UserId, logonUser.GroupId, oldPXX, newPXX);
            if (xmlResult.IsSuccess)
            {
                this.ShowJsAlertAndGoUrl("變更密碼成功", this.GetResolveUrl("~/Logout.aspx"));
            }
            else
            {
                this.ShowJsAlert("變更密碼失敗，" + xmlResult.Message);
            }
            #endregion
        }
    }
}