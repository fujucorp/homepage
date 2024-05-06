using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb
{
    public partial class Sitemap : LocalizedPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region [MDY:20170828] 因為土銀的支付寶合約未完成，增加是否啟用的判斷
            if (!this.IsPostBack)
            {
                this.liAlipay.Visible = Fisc.IsInboundEnabled();
            }
            #endregion
        }
    }
}