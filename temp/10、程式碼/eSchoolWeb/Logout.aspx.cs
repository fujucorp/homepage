using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 登出頁面
    /// </summary>
    public partial class Logout : System.Web.UI.Page, IMenuPage
    {
        #region Implement IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public string MenuID
        {
            get
            {
                return "Logout";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public string MenuName
        {
            get
            {
                return "登出";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            XmlResult xmlResult = null;
            string userQual = null;
            LogonUser logonUser = WebHelper.GetLogonUser();
            if (logonUser != null && !String.IsNullOrEmpty(logonUser.LogonSN))
            {
                userQual = logonUser.UserQual;
                xmlResult = DataProxy.Current.LogoutUser(this, logonUser);
            }

            this.Session.Clear();
            this.Session.Abandon();
            this.Response.Redirect("index.aspx");

            #region [MDY:20160329] 修正 FORTIFY 弱點
            //switch (userQual)
            //{
            //    //case UserQualCodeTexts.BANK:
            //    //    Response.Redirect("~/StaffLogin.aspx");
            //    //    break;
            //    //case UserQualCodeTexts.SCHOOL:
            //    //    Response.Redirect("~/SchoolLogin.aspx");
            //    //    break;
            //    //case UserQualCodeTexts.STUDENT:
            //    //    Response.Redirect("~/StudentLogin.aspx");
            //    //    break;
            //    //case UserQualCodeTexts.OTHERBANK:
            //    //    Response.Redirect("~/PartnerLogin.aspx");
            //    //    break;
            //    default:
            //        string ec = this.Request.QueryString["ec"];
            //        int value = 0;
            //        if (int.TryParse(ec, out value))
            //        {
            //            this.Response.Redirect("index.aspx?ec=" + value.ToString());
            //        }
            //        else
            //        {
            //            this.Response.Redirect("index.aspx");
            //        }
            //        break;
            //}
            #endregion
        }
    }
}