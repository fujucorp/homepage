using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb
{
    public partial class ChangePWD : LocalizedPage
    {
        #region Override IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "ChangePWD";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "首次登入變更密碼";
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

        #region Keep 頁面參數
        private string UnitID
        {
            get
            {
                return ViewState["UnitID"] as String;
            }
            set
            {
                ViewState["UnitID"] = value == null ? String.Empty : value.Trim();
            }
        }

        private string UserID
        {
            get
            {
                return ViewState["UserID"] as String;
            }
            set
            {
                ViewState["UserID"] = value == null ? String.Empty : value.Trim();
            }
        }

        private string GroupID
        {
            get
            {
                return ViewState["GroupID"] as String;
            }
            set
            {
                ViewState["GroupID"] = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LogonUser logonUser = WebHelper.GetLogonUser();
            if (logonUser == null || logonUser.IsAnonymous)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("未登入或頁面逾時");
                this.ShowJsAlert(msg);
                this.lbtnOK.Visible = false;
                return;
            }
            if (!logonUser.IsSchoolUser)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("非學校帳號，無法變更密碼");
                this.ShowJsAlert(msg);
                this.lbtnOK.Visible = false;
                return;
            }

            if (!this.IsPostBack)
            {
                #region [Old]
                //KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                //if (QueryString == null || QueryString.Count == 0)
                //{
                //    //[TODO] 固定顯示訊息的收集
                //    string msg = this.GetLocalized("缺少網頁參數");
                //    this.ShowJsAlert(msg);
                //    this.lbtnOK.Visible = false;
                //    return;
                //}

                //this.labUnitID.Text = this.UnitID = QueryString.TryGetValue("UnidID", String.Empty);
                //this.labUserID.Text = this.UserID = QueryString.TryGetValue("UserID", String.Empty);
                //this.GroupID = QueryString.TryGetValue("GroupID", String.Empty);
                #endregion

                this.labUnitID.Text = this.UnitID = logonUser.UnitId;
                this.labUserID.Text = this.UserID = logonUser.UserId;
                this.GroupID = logonUser.GroupId;
            }
        }

        protected void lbtnOK_Click(object sender, EventArgs e)
        {
            #region [MDY:20220530] Checkmarx 調整
            string unitId = this.UnitID;
            string userId = this.UserID;
            string groupId = this.GroupID;
            string oldPXX = this.tbxOldPXX.Text.Trim();
            string newPXX = this.tbxNewPXX.Text.Trim();
            string chkPXX = this.tbxChkPXX.Text.Trim();
            if (String.IsNullOrEmpty(oldPXX))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("請輸入舊的密碼");
                this.ShowJsAlert(msg);
                return;
            }
            if (String.IsNullOrEmpty(newPXX))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("請輸入新的密碼");
                this.ShowJsAlert(msg);
                return;
            }
            if (newPXX != chkPXX)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("確認密碼與新的密碼不同");
                this.ShowJsAlert(msg);
                return;
            }
            if (newPXX == oldPXX)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("新舊密碼不可相同");
                this.ShowJsAlert(msg);
                return;
            }
            if (!DataFormat.CheckUserPXXFormat(newPXX))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized(String.Format("「使用者密碼」限輸入 {0} ~ {1} 碼英數字，且不可是連續6個相同或遞增或遞減的英文、數字", DataFormat.UserPXXMinSize, DataFormat.UserPXXMaxSize));
                this.ShowJsAlert(msg);
                this.ShowJsAlert(msg);
                return;
            }

            XmlResult xmlResult = DataProxy.Current.ChangeUserPXX(this, unitId, userId, groupId, oldPXX, newPXX);
            if (xmlResult.IsSuccess)
            {
                this.Session.Clear();
                this.Session.Abandon();
                this.ShowJsAlertAndGoUrl("變更密碼成功", "index.aspx");
            }
            else
            {
                this.ShowJsAlert("變更密碼失敗，" + xmlResult.Message);
            }
            #endregion
        }
    }
}