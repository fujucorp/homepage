using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb
{
    /// <summary>
    /// 左側進入點選單自定控制項
    /// </summary>
    public partial class UCEntryPageMenu : BaseUserControl
    {
        #region [MDY:20200705] 修正特殊字串路徑 "/(Z(%22onerror=%22alert'XSS'%22))/" 的 XSS 問題
        //protected string PayByWebUrl
        //{
        //    get
        //    {
        //        return this.Page.ResolveUrl("~/payment.aspx?m=1");
        //    }
        //}

        //protected string PayByCardUrl
        //{
        //    get
        //    {
        //        return this.Page.ResolveUrl("~/payment.aspx?m=2");
        //    }
        //}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region [MDY:20170828] 因為土銀的支付寶合約未完成，增加是否啟用的判斷
            if (!this.IsPostBack)
            {
                this.liAlipay.Visible = Fisc.IsInboundEnabled();
            }
            #endregion
        }

        //protected void lnkSchool_ServerClick(object sender, EventArgs e)
        //{
        //    this.Response.Redirect("~/index.aspx?lk=1");
        //}
    }
}