using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;

namespace eSchoolWeb
{
    public partial class UCEntryPageAD : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 大廣告_1
                {
                    string imgUrl = null;
                    string linkUrl = null;
                    CacheHelper.GetADData(AdCodeTexts.AD001, out imgUrl, out linkUrl);
                    if (String.IsNullOrEmpty(imgUrl))
                    {
                        this.imgAD001.Src = WebHelper.GetResolveUrl(this.Page, this.imgAD001.Src);
                    }
                    else
                    {
                        this.imgAD001.Src = WebHelper.GetResolveUrl(this.Page, imgUrl);
                    }
                    if (!String.IsNullOrEmpty(linkUrl))
                    {
                        this.lnkAD001.HRef = linkUrl;
                    }
                    if (!this.lnkAD001.HRef.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase) && String.IsNullOrEmpty(this.lnkAD001.Target))
                    {
                        this.lnkAD001.Target = "_blank";
                    }
                }
                #endregion

                #region 小廣告_2
                {
                    string imgUrl = null;
                    string linkUrl = null;
                    CacheHelper.GetADData(AdCodeTexts.AD002, out imgUrl, out linkUrl);
                    if (String.IsNullOrEmpty(imgUrl))
                    {
                        this.imgAD002.Src = WebHelper.GetResolveUrl(this.Page, this.imgAD002.Src);
                    }
                    else
                    {
                        this.imgAD002.Src = WebHelper.GetResolveUrl(this.Page, imgUrl);
                    }
                    if (!String.IsNullOrEmpty(linkUrl))
                    {
                        this.lnkAD002.HRef = linkUrl;
                    }
                    if (!this.lnkAD002.HRef.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase) && String.IsNullOrEmpty(this.lnkAD002.Target))
                    {
                        this.lnkAD002.Target = "_blank";
                    }
                }
                #endregion

                #region 小廣告_3
                {
                    string imgUrl = null;
                    string linkUrl = null;
                    CacheHelper.GetADData(AdCodeTexts.AD003, out imgUrl, out linkUrl);
                    if (String.IsNullOrEmpty(imgUrl))
                    {
                        this.imgAD003.Src = WebHelper.GetResolveUrl(this.Page, this.imgAD003.Src);
                    }
                    else
                    {
                        this.imgAD003.Src = WebHelper.GetResolveUrl(this.Page, imgUrl);
                    }
                    if (!String.IsNullOrEmpty(linkUrl))
                    {
                        this.lnkAD003.HRef = linkUrl;
                    }
                    if (!this.lnkAD003.HRef.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase) && String.IsNullOrEmpty(this.lnkAD003.Target))
                    {
                        this.lnkAD003.Target = "_blank";
                    }
                }
                #endregion

                #region 小廣告_4
                {
                    string imgUrl = null;
                    string linkUrl = null;
                    CacheHelper.GetADData(AdCodeTexts.AD004, out imgUrl, out linkUrl);
                    if (String.IsNullOrEmpty(imgUrl))
                    {
                        this.imgAD004.Src = WebHelper.GetResolveUrl(this.Page, this.imgAD004.Src);
                    }
                    else
                    {
                        this.imgAD004.Src = WebHelper.GetResolveUrl(this.Page, imgUrl);
                    }
                    if (!String.IsNullOrEmpty(linkUrl))
                    {
                        this.lnkAD004.HRef = linkUrl;
                    }
                    if (!this.lnkAD004.HRef.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase) && String.IsNullOrEmpty(this.lnkAD004.Target))
                    {
                        this.lnkAD004.Target = "_blank";
                    }
                }
                #endregion

                #region 小廣告_5
                {
                    string imgUrl = null;
                    string linkUrl = null;
                    CacheHelper.GetADData(AdCodeTexts.AD005, out imgUrl, out linkUrl);
                    if (String.IsNullOrEmpty(imgUrl))
                    {
                        this.imgAD005.Src = WebHelper.GetResolveUrl(this.Page, this.imgAD005.Src);
                    }
                    else
                    {
                        this.imgAD005.Src = WebHelper.GetResolveUrl(this.Page, imgUrl);
                    }
                    if (!String.IsNullOrEmpty(linkUrl))
                    {
                        this.lnkAD005.HRef = linkUrl;
                    }
                    if (!this.lnkAD005.HRef.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase) && String.IsNullOrEmpty(this.lnkAD005.Target))
                    {
                        this.lnkAD005.Target = "_blank";
                    }
                }
                #endregion
            }
        }
    }
}