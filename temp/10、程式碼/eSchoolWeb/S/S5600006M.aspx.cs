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
    /// 廣告管理 (維護)
    /// </summary>
    public partial class S5600006M : BasePage
    {
        #region Keep 頁面參數
        /// <summary>
        /// 編輯模式參數
        /// </summary>
        private string Action
        {
            get
            {
                return ViewState["ACTION"] as string;
            }
            set
            {
                ViewState["ACTION"] = value == null ? null : value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// 編輯的廣告代碼
        /// </summary>
        private string EditAdId
        {
            get
            {
                return ViewState["EditAdId"] as string;
            }
            set
            {
                ViewState["EditAdId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.labAdId.Text = String.Empty;
            this.tbxImgUrl.Text = String.Empty;
            this.tbxLinkUrl.Text = String.Empty;

            WebHelper.SetDropDownListItems(this.ddlKind, DefaultItem.Kind.None, false, new AdKindCodeTexts(), false, true, 0, AdKindCodeTexts.URL);

            this.ccbtnOK.Visible = false;
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(AdEntity data)
        {
            if (data == null)
            {
                this.labAdId.Text = String.Empty;
                this.tbxImgUrl.Text = String.Empty;
                this.tbxLinkUrl.Text = String.Empty;

                this.ddlKind.SelectedIndex = 0;

                this.ccbtnOK.Visible = false;
                return;
            }

            this.labAdId.Text = AdCodeTexts.GetText(data.Id);
            this.tbxImgUrl.Text = data.ImgUrl;
            this.tbxLinkUrl.Text = data.LinkUrl;

            WebHelper.SetDropDownListSelectedValue(this.ddlKind, data.Kind);

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 取得並檢查輸入的維護資料
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool GetAndCheckEditData(out AdEntity data)
        {
            data = new AdEntity();
            data.Id = this.EditAdId;

            data.Kind = this.ddlKind.SelectedValue;
            if (data.Kind == AdKindCodeTexts.URL)
            {
                data.ImgUrl = this.tbxImgUrl.Text.Trim();
                if (String.IsNullOrEmpty(data.ImgUrl))
                {
                    this.ShowMustInputAlert("圖檔網址");
                    return false;
                }
                data.ImgContent = null;
            }
            else
            {
                data.ImgUrl = String.Empty;
                data.ImgContent = this.fupImgContent.FileBytes;
                if (data.ImgContent == null || data.ImgContent.Length == 0)
                {
                    this.ShowMustInputAlert("上傳圖檔");
                    return false;
                }

                #region [MDY:20201227] 檢查副檔名限制 .JPG、.PNG、.GIF
                string extName = System.IO.Path.GetExtension(this.fupImgContent.FileName).ToLower();
                if (extName != ".jpg" && extName != ".png" && extName != ".gif")
                {
                    string msg = this.GetLocalized("僅支援 圖檔 的 jpg | png | gif");
                    this.ShowJsAlert(msg);
                    return false;
                }
                #endregion
            }

            data.LinkUrl = this.tbxLinkUrl.Text.Trim();
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    return;
                }
                #endregion

                #region 處理參數
                KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                if (QueryString == null || QueryString.Count == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    return;
                }

                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.EditAdId = QueryString.TryGetValue("AdId", String.Empty);
                if (this.Action != ActionMode.Modify || AdCodeTexts.GetCodeText(this.EditAdId) == null)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                string action = this.GetLocalized("查詢要維護的資料");
                AdEntity data = null;
                Expression where = new Expression(AdEntity.Field.Id, this.EditAdId);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<AdEntity>(this, where, null, out data);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    return;
                }
                if (data == null)
                {
                    data = new AdEntity();
                    data.Id = this.EditAdId;
                    data.Kind = AdKindCodeTexts.URL;
                }
                #endregion

                this.BindEditData(data);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            AdEntity data = null;
            if (!this.GetAndCheckEditData(out data))
            {
                return;
            }

            string action = this.GetLocalized("設定資料");
            string backUrl = "S5600006.aspx";

            int count = 0;
            Expression where = new Expression(AdEntity.Field.Id, data.Id);
            XmlResult xmlResult = DataProxy.Current.SelectCount<AdEntity>(this.Page, where, out count);
            if (xmlResult.IsSuccess)
            {
                if (count == 0)
                {
                    xmlResult = DataProxy.Current.Insert<AdEntity>(this.Page, data, out count);
                    if (xmlResult.IsSuccess)
                    {
                        if (count < 1)
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                        }
                        else
                        {
                            this.ShowActionSuccessAlert(action, backUrl);
                        }
                        return;
                    }
                }
                else
                {
                    xmlResult = DataProxy.Current.Update<AdEntity>(this.Page, data, out count);
                    if (xmlResult.IsSuccess)
                    {
                        if (count < 1)
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                        }
                        else
                        {
                            this.ShowActionSuccessAlert(action, backUrl);
                        }
                        return;
                    }
                }
            }

            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        }
    }
}