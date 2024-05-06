using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.R
{
    public partial class R4200002 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ccbtnOK.Visible = false;
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {

        }
    }
}