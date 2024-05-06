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

namespace eSchoolWeb.S
{
    /// <summary>
    /// 廣告管理
    /// </summary>
    public partial class S5600006 : BasePage
    {
        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.GetDataAndBind();
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetDataAndBind()
        {
            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
                return false;
            }
            #endregion

            #region 取資料
            AdEntity ad001 = null;
            AdEntity ad002 = null;
            AdEntity ad003 = null;
            AdEntity ad004 = null;
            AdEntity ad005 = null;
            {
                Expression where = new Expression();
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(AdEntity.Field.Id, OrderByEnum.Asc);

                AdEntity[] datas = null;
                XmlResult xmlResult = DataProxy.Current.SelectAll<AdEntity>(this, where, orderbys, out datas);
                if (!xmlResult.IsSuccess)
                {
                    string action = ActionMode.GetActionLocalized(ActionMode.Query);
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    return false;
                }

                foreach (AdEntity data in datas)
                {
                    switch (data.Id)
                    {
                        case AdCodeTexts.AD001:
                            ad001 = data;
                            break;
                        case AdCodeTexts.AD002:
                            ad002 = data;
                            break;
                        case AdCodeTexts.AD003:
                            ad003 = data;
                            break;
                        case AdCodeTexts.AD004:
                            ad004 = data;
                            break;
                        case AdCodeTexts.AD005:
                            ad005 = data;
                            break;
                    }
                }
            }
            #endregion

            #region 結繫
            {
                bool hasMaintain = this.HasMaintainAuth();

                #region AD001
                this.labAD001Name.Text = AdCodeTexts.AD001_TEXT;
                if (ad001 != null)
                {
                    this.labAD001Kind.Text = AdKindCodeTexts.GetText(ad001.Kind);
                    this.labAD001Url.Text = ad001.LinkUrl;
                    if (ad001.Kind == AdKindCodeTexts.URL)
                    {
                        this.ImgAD001.ImageUrl = GetMyUrl(ad001.ImgUrl);
                    }
                    else
                    {
                        this.ImgAD001.ImageUrl = this.GetResolveUrl("~/api/AD.ashx?id=" + ad001.Id);
                    }
                    this.ccbtnAD001Set.Visible = hasMaintain;
                }
                #endregion

                #region AD002
                this.labAD002Name.Text = AdCodeTexts.AD002_TEXT;
                if (ad002 != null)
                {
                    this.labAD002Kind.Text = AdKindCodeTexts.GetText(ad002.Kind);
                    this.labAD002Url.Text = ad002.LinkUrl;
                    if (ad002.Kind == AdKindCodeTexts.URL)
                    {
                        this.ImgAD002.ImageUrl = GetMyUrl(ad002.ImgUrl);
                    }
                    else
                    {
                        this.ImgAD002.ImageUrl = this.GetResolveUrl("~/api/AD.ashx?id=" + ad002.Id);
                    }
                    this.ccbtnAD002Set.Visible = hasMaintain;
                }
                #endregion

                #region AD003
                this.labAD003Name.Text = AdCodeTexts.AD003_TEXT;
                if (ad003 != null)
                {
                    this.labAD003Kind.Text = AdKindCodeTexts.GetText(ad003.Kind);
                    this.labAD003Url.Text = ad003.LinkUrl;
                    if (ad003.Kind == AdKindCodeTexts.URL)
                    {
                        this.ImgAD003.ImageUrl = GetMyUrl(ad003.ImgUrl);
                    }
                    else
                    {
                        this.ImgAD003.ImageUrl = this.GetResolveUrl("~/api/AD.ashx?id=" + ad003.Id);
                    }
                    this.ccbtnAD003Set.Visible = hasMaintain;
                }
                #endregion

                #region AD004
                this.labAD004Name.Text = AdCodeTexts.AD004_TEXT;
                if (ad004 != null)
                {
                    this.labAD004Kind.Text = AdKindCodeTexts.GetText(ad004.Kind);
                    this.labAD004Url.Text = ad004.LinkUrl;
                    if (ad004.Kind == AdKindCodeTexts.URL)
                    {
                        this.ImgAD004.ImageUrl = GetMyUrl(ad004.ImgUrl);
                    }
                    else
                    {
                        this.ImgAD004.ImageUrl = this.GetResolveUrl("~/api/AD.ashx?id=" + ad004.Id);
                    }
                    this.ccbtnAD004Set.Visible = hasMaintain;
                }
                #endregion

                #region AD005
                this.labAD005Name.Text = AdCodeTexts.AD005_TEXT;
                if (ad005 != null)
                {
                    this.labAD005Kind.Text = AdKindCodeTexts.GetText(ad005.Kind);
                    this.labAD005Url.Text = ad005.LinkUrl;
                    if (ad005.Kind == AdKindCodeTexts.URL)
                    {
                        this.ImgAD005.ImageUrl = GetMyUrl(ad005.ImgUrl);
                    }
                    else
                    {
                        this.ImgAD005.ImageUrl = this.GetResolveUrl("~/api/AD.ashx?id=" + ad005.Id);
                    }
                    this.ccbtnAD005Set.Visible = hasMaintain;
                }
                #endregion
            }
            #endregion

            return true;
        }

        private string GetMyUrl(string url)
        {
            url = url == null ? null : url.Trim();
            if (!String.IsNullOrEmpty(url))
            {
                if (!url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)
                    && !url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase)
                    && !url.StartsWith("~", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (url.Substring(0, 1) == ("/"))
                    {
                        return this.GetResolveUrl("~" + url);
                    }
                    else
                    {
                        return this.GetResolveUrl("~/" + url);
                    }
                }
            }
            return url;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void ccbtnSet_Click(object sender, EventArgs e)
        {
            MyLinkButton mybtn = sender as MyLinkButton;
            string adId = mybtn == null ? null : mybtn.CommandArgument;
            if (String.IsNullOrEmpty(adId))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }

            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("Action", ActionMode.Modify);
            QueryString.Add("AdId", adId);
            Session["QueryString"] = QueryString;
            Server.Transfer("S5600006M.aspx");
        }
    }
}