using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;

namespace eSchoolWeb
{
    public partial class PrintBox : LocalizedPage
    {
        public string LogonUserName
        {
            get
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                if (logonUser != null)
                {
                    return logonUser.UserName;
                }
                return String.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}