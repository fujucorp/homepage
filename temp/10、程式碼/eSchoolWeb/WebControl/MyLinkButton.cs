using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;

namespace eSchoolWeb
{
    #region MyLinkButton 的 Button Text 定義類別
    /// <summary>
    /// 按鈕文字常數定義抽象類別
    /// </summary>
    public abstract class ButtonText
    {
        /// <summary>
        /// 查詢
        /// </summary>
        public const string Query = "查詢";
        /// <summary>
        /// 新增
        /// </summary>
        public const string Insert = "新增";
        /// <summary>
        /// 修改
        /// </summary>
        public const string Modify = "修改";
        /// <summary>
        /// 刪除
        /// </summary>
        public const string Delete = "刪除";
        /// <summary>
        /// 複製
        /// </summary>
        public const string Copy = "複製";

        /// <summary>
        /// 核可
        /// </summary>
        public const string Approve = "核可";

        /// <summary>
        /// 啟用
        /// </summary>
        public const string Enable = "啟用";
        /// <summary>
        /// 停用
        /// </summary>
        public const string Disable = "停用";


        /// <summary>
        /// 回上一頁
        /// </summary>
        public const string GoBack = "回上一頁";
        /// <summary>
        /// 離開
        /// </summary>
        public const string GoAway = "離開";
        /// <summary>
        /// 確定
        /// </summary>
        public const string OK = "確定";
        /// <summary>
        /// 下載
        /// </summary>
        public const string Download = "下載";
        /// <summary>
        /// 列印
        /// </summary>
        public const string Print = "列印";
    }
    #endregion

    #region MyLinkButton 的 Command Name 定義類別
    /// <summary>
    /// 按鈕命令名稱常數定義抽象類別
    /// </summary>
    public abstract class ButtonCommandName
    {
        /// <summary>
        /// 查詢 : QueryData
        /// </summary>
        public const string Query = "QueryData";
        /// <summary>
        /// 新增 : InsertData
        /// </summary>
        public const string Insert = "InsertData";
        /// <summary>
        /// 修改 : ModifyData
        /// </summary>
        public const string Modify = "ModifyData";
        /// <summary>
        /// 刪除 : DeleteData
        /// </summary>
        public const string Delete = "DeleteData";
        /// <summary>
        /// 複製 : CopyData
        /// </summary>
        public const string Copy = "CopyData";

        /// <summary>
        /// 核可 : ApproveData
        /// </summary>
        public const string Approve = "ApproveData";

        /// <summary>
        /// 啟用 : EnableData
        /// </summary>
        public const string Enable = "EnableData";
        /// <summary>
        /// 停用 : DisableData
        /// </summary>
        public const string Disable = "DisableData";

        /// <summary>
        /// 回上一頁 : GoBack
        /// </summary>
        public const string GoBack = "GoBack";
        /// <summary>
        /// 離開 : GoAway
        /// </summary>
        public const string GoAway = "GoAway";
        /// <summary>
        /// 確定 : OK
        /// </summary>
        public const string OK = "OK";

        /// <summary>
        /// 下載 : Download
        /// </summary>
        public const string Download = "Download";

        /// <summary>
        /// 列印 : Print
        /// </summary>
        public const string Print = "Print";
    }
    #endregion

    /// <summary>
    /// 支援 Location 的 LinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyLinkButton runat=server></{0}:MyLinkButton>")]
    public class MyLinkButton : LinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MyLinkButton 伺服器控制項
        /// </summary>
        public MyLinkButton()
            : base()
        {
            //base.ViewStateMode = ViewStateMode.Disabled;
        }
        #endregion

        #region Override LinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Localize(false);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (!String.IsNullOrEmpty(this.LocationText) && !this.IsLocalized)
            {
                this.Localize(false);
            }

            if (String.IsNullOrEmpty(this.LocationText))
            {
                base.RenderContents(output);
            }
            else
            {
                string keepText = this.Text;
                this.Text = this.LocalizedText;
                base.RenderContents(output);
                this.Text = keepText;
            }
        }
        #endregion

        #region Localized 相關
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected virtual bool CanChanegLocationText
        {
            get
            {
                return true;
            }
        }

        private string _LocationText = String.Empty;
        /// <summary>
        /// 要 Localized 的文字
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字")]
        public virtual string LocationText
        {
            get
            {
                return _LocationText;
            }
            set
            {
                if (this.CanChanegLocationText)
                {
                    _LocationText = value == null ? String.Empty : value.Trim();
                }
            }
        }

        private string _LocalizedText = String.Empty;
        /// <summary>
        /// 已 Localized 的文字
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("已 Localized 的文字")]
        public virtual string LocalizedText
        {
            get
            {
                return _LocalizedText;
            }
            protected set
            {
                _LocalizedText = value ?? String.Empty;
                this.IsLocalized = false;
            }
        }

        private bool _IsLocalized = false;
        /// <summary>
        /// 是否已執行 Localize 處理
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Browsable(false)]
        public virtual bool IsLocalized
        {
            get
            {
                return _IsLocalized;
            }
            protected set
            {
                _IsLocalized = value;
            }
        }

        /// <summary>
        /// 執行 Localize 處理，執行後 IsLocalized 屬性會設為 true 並更新 LocalizedText 屬性值
        /// </summary>
        /// <param name="renew">指定是否強迫處理 Localize，如果指定 false，則不會重複處理。</param>
        public virtual void Localize(bool renew)
        {
            if (this.IsLocalized && !renew)
            {
                return;
            }

            string locationText = this.LocationText;
            if (!String.IsNullOrEmpty(locationText))
            {
                this.LocalizedText = WebHelper.GetButtonControlLocalized(locationText);
            }
            else if (!String.IsNullOrEmpty(this.LocalizedText))
            {
                this.LocalizedText = String.Empty;
            }
            this.IsLocalized = true;
        }
        #endregion
    }

    /// <summary>
    /// 查詢的 MyLinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyQueryButton runat=server></{0}:MyQueryButton>")]
    public class MyQueryButton : MyLinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MyQueryButton 伺服器控制項
        /// </summary>
        public MyQueryButton()
            : base()
        {
        }
        #endregion

        #region Override MyLinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(base.CommandName))
            {
                base.CommandName = ButtonCommandName.Query;
            }

            //if (this.Page != null && !this.Page.IsPostBack)
            if (this.Page != null)
            {
                #region 檢查權限
                if (this.AutoCheckAuth)
                {
                    this.Enabled = this.HasAuth();
                    if (this.Enabled)
                    {
                        this.ToolTip = String.Empty;
                    }
                    else
                    {
                        this.ToolTip = WebHelper.GetLocalized("CTL_NoQueryAuth", "無查詢權限");
                    }
                }
                #endregion
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.Enabled)
            {
                string msg = WebHelper.GetLocalized("CTL_NoQueryAuth", "無查詢權限");
                base.OnClientClick = String.Concat("return confirm('", msg, "');");
            }

            base.OnPreRender(e);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                this.Localize(false);
                base.CommandName = ButtonCommandName.Query;
            }
            base.RenderContents(output);
        }
        #endregion

        #region Override MyLinkButton's Property
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected override bool CanChanegLocationText
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 要 Localized 的文字，固定為 ButtonText.Query 常數，設定無效
        /// </summary>
        [DefaultValue(ButtonText.Query)]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字，固定為 ButtonText.Query 常數，設定無效")]
        public override string LocationText
        {
            get
            {
                return ButtonText.Query;
            }
        }
        #endregion

        #region 權限相關
        private bool _AutoCheckAuth = true;
        /// <summary>
        /// 是否自動檢查權限
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("是否自動檢查權限，預設為 true")]
        public virtual bool AutoCheckAuth
        {
            get
            {
                return _AutoCheckAuth;
            }
            set
            {
                _AutoCheckAuth = value;
            }
        }

        private bool? _HasAuth = null;
        /// <summary>
        /// 取得是否有授權
        /// </summary>
        /// <returns></returns>
        private bool HasAuth()
        {
            if (_HasAuth == null)
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                bool isEditPage = false;
                bool isSubPage = false;
                string menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
                MenuAuth menuAuth = logonUser.GetMenuAuth(menuID);
                //_HasAuth = menuAuth.HasAnyone();
                _HasAuth = menuAuth.HasSelect() || menuAuth.HasMaintain();
            }
            return _HasAuth.Value;
        }
        #endregion
    }

    /// <summary>
    /// 新增的 MyLinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyInsertButton runat=server></{0}:MyInsertButton>")]
    public class MyInsertButton : MyLinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MyInsertButton 伺服器控制項
        /// </summary>
        public MyInsertButton()
            : base()
        {
        }
        #endregion

        #region Override MyLinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(base.CommandName))
            {
                base.CommandName = ButtonCommandName.Insert;
            }

            //if (this.Page != null && !this.Page.IsPostBack)
            if (this.Page != null)
            {
                #region 權限檢查
                if (this.AutoCheckAuth)
                {
                    this.Enabled = this.HasAuth();
                    if (this.Enabled)
                    {
                        this.ToolTip = String.Empty;
                    }
                    else
                    {
                        this.ToolTip = WebHelper.GetLocalized("CTL_NoMaintainAuth", "無維護權限");
                    }
                }
                #endregion
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.Enabled)
            {
                string msg = WebHelper.GetLocalized("CTL_NoInsertAuth", "無新增權限");
                base.OnClientClick = String.Concat("return confirm('", msg, "');");
            }

            base.OnPreRender(e);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                this.Localize(false);
                base.CommandName = ButtonCommandName.Insert;
            }
            base.RenderContents(output);
        }
        #endregion

        #region Override MyLinkButton's Property
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected override bool CanChanegLocationText
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 要 Localized 的文字，固定為 ButtonText.Insert 常數，設定無效
        /// </summary>
        [DefaultValue(ButtonText.Insert)]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字，固定為 ButtonText.Insert 常數，設定無效")]
        public override string LocationText
        {
            get
            {
                return ButtonText.Insert;
            }
        }
        #endregion

        #region 權限相關
        private bool _AutoCheckAuth = true;
        /// <summary>
        /// 是否自動檢查權限
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("是否自動檢查權限，預設為 true")]
        public virtual bool AutoCheckAuth
        {
            get
            {
                return _AutoCheckAuth;
            }
            set
            {
                _AutoCheckAuth = value;
            }
        }

        private bool? _HasAuth = null;
        /// <summary>
        /// 取得是否有授權
        /// </summary>
        /// <returns></returns>
        private bool HasAuth()
        {
            if (_HasAuth == null)
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                bool isEditPage = false;
                bool isSubPage = false;
                string menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
                MenuAuth menuAuth = logonUser.GetMenuAuth(menuID);
                //_HasAuth = menuAuth.HasMaintain();
                _HasAuth = menuAuth.HasInsert();
            }
            return _HasAuth.Value;
        }
        #endregion
    }

    /// <summary>
    /// 修改的 MyLinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyModifyButton runat=server></{0}:MyModifyButton>")]
    public class MyModifyButton : MyLinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MyModifyButton 伺服器控制項
        /// </summary>
        public MyModifyButton()
            : base()
        {
            this.ViewStateMode = ViewStateMode.Inherit;
        }
        #endregion

        #region Override MyLinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(base.CommandName))
            {
                base.CommandName = ButtonCommandName.Modify;
            }

            //if (this.Page != null && !this.Page.IsPostBack)
            if (this.Page != null)
            {
                #region 權限檢查
                if (this.AutoCheckAuth)
                {
                    this.Enabled = this.HasAuth();
                    if (this.Enabled)
                    {
                        this.ToolTip = String.Empty;
                    }
                    else
                    {
                        this.ToolTip = WebHelper.GetLocalized("CTL_NoMaintainAuth", "無維護權限");
                    }
                }
                #endregion
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.Enabled)
            {
                string msg = WebHelper.GetLocalized("CTL_NoModifyAuth", "無修改權限");
                base.OnClientClick = String.Concat("return confirm('", msg, "');");
            }

            base.OnPreRender(e);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                this.Localize(false);
                base.CommandName = ButtonCommandName.Modify;
            }
            base.RenderContents(output);
        }
        #endregion

        #region Override MyLinkButton's Property
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected override bool CanChanegLocationText
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 要 Localized 的文字，固定為 ButtonText.Modify 常數，設定無效
        /// </summary>
        [DefaultValue(ButtonText.Modify)]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字，固定為 ButtonText.Modify 常數，設定無效")]
        public override string LocationText
        {
            get
            {
                return ButtonText.Modify;
            }
        }
        #endregion

        #region 權限相關
        private bool _AutoCheckAuth = true;
        /// <summary>
        /// 是否自動檢查權限
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("是否自動檢查權限，預設為 true")]
        public virtual bool AutoCheckAuth
        {
            get
            {
                return _AutoCheckAuth;
            }
            set
            {
                _AutoCheckAuth = value;
            }
        }

        private bool? _HasAuth = null;
        /// <summary>
        /// 取得是否有授權
        /// </summary>
        /// <returns></returns>
        private bool HasAuth()
        {
            if (_HasAuth == null)
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                bool isEditPage = false;
                bool isSubPage = false;
                string menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
                MenuAuth menuAuth = logonUser.GetMenuAuth(menuID);
                //_HasAuth = menuAuth.HasMaintain();
                _HasAuth = menuAuth.HasUpdate();
            }
            return _HasAuth.Value;
        }
        #endregion
    }

    /// <summary>
    /// 刪除的 MyLinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyDeleteButton runat=server></{0}:MyDeleteButton>")]
    public class MyDeleteButton : MyLinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MyDeleteButton 伺服器控制項
        /// </summary>
        public MyDeleteButton()
            : base()
        {
            this.ViewStateMode = ViewStateMode.Inherit;
        }
        #endregion

        #region Property
        /// <summary>
        /// 取得或設定是否要使用預設的 Javscript 確認訊息，預設為 true
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("取得或設定是否要使用預設的 Javscript 確認訊息")]
        public bool UseDefaultJSConfirm
        {
            get
            {
                object value = ViewState["UseDefaultJSConfirm"];
                if (value is bool)
                {
                    return (bool)value;
                }
                return true;
            }
            set
            {
                ViewState["UseDefaultJSConfirm"] = value;
            }
        }
        #endregion

        #region Override MyLinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(base.CommandName))
            {
                base.CommandName = ButtonCommandName.Delete;
            }

            //if (this.Page != null && !this.Page.IsPostBack)
            if (this.Page != null)
            {
                #region 權限檢查
                if (this.AutoCheckAuth)
                {
                    this.Enabled = this.HasAuth();
                    if (this.Enabled)
                    {
                        this.ToolTip = String.Empty;
                    }
                    else
                    {
                        this.ToolTip = WebHelper.GetLocalized("CTL_NoMaintainAuth", "無維護權限");
                    }
                }
                #endregion
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.Enabled)
            {
                string msg = WebHelper.GetLocalized("CTL_NoDeleteAuth", "無刪除權限");
                base.OnClientClick = String.Concat("return confirm('", msg, "');");
            }
            else if (this.UseDefaultJSConfirm && String.IsNullOrEmpty(base.OnClientClick))
            {
                string msg = WebHelper.GetLocalized("CTL_DeleteConfirm", String.Concat("確定", ButtonText.Delete, "此資料"));
                base.OnClientClick = String.Concat("return confirm('", msg, "');");
            }

            base.OnPreRender(e);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                this.Localize(false);
                base.CommandName = ButtonCommandName.Delete;
            }
            base.RenderContents(output);
        }
        #endregion

        #region Override MyLinkButton's Property
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected override bool CanChanegLocationText
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 要 Localized 的文字，固定為 ButtonText.Delete 常數，設定無效
        /// </summary>
        [DefaultValue(ButtonText.Delete)]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字，固定為 ButtonText.Delete 常數，設定無效")]
        public override string LocationText
        {
            get
            {
                return ButtonText.Delete;
            }
        }
        #endregion

        #region 權限相關
        private bool _AutoCheckAuth = true;
        /// <summary>
        /// 是否自動檢查權限
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("是否自動檢查權限，預設為 true")]
        public virtual bool AutoCheckAuth
        {
            get
            {
                return _AutoCheckAuth;
            }
            set
            {
                _AutoCheckAuth = value;
            }
        }

        private bool? _HasAuth = null;
        /// <summary>
        /// 取得是否有授權
        /// </summary>
        /// <returns></returns>
        private bool HasAuth()
        {
            if (_HasAuth == null)
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                bool isEditPage = false;
                bool isSubPage = false;
                string menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
                MenuAuth menuAuth = logonUser.GetMenuAuth(menuID);
                //_HasAuth = menuAuth.HasMaintain();
                _HasAuth = menuAuth.HasDelete();
            }
            return _HasAuth.Value;
        }
        #endregion
    }

    /// <summary>
    /// 核可的 MyLinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyApproveButton runat=server></{0}:MyApproveButton>")]
    public class MyApproveButton : MyLinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MyApproveButton 伺服器控制項
        /// </summary>
        public MyApproveButton()
            : base()
        {
            this.ViewStateMode = ViewStateMode.Inherit;
        }
        #endregion

        #region Property
        /// <summary>
        /// 取得或設定是否要使用預設的 Javscript 確認訊息，預設為 true
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("取得或設定是否要使用預設的 Javscript 確認訊息")]
        public bool UseDefaultJSConfirm
        {
            get
            {
                object value = ViewState["UseDefaultJSConfirm"];
                if (value is bool)
                {
                    return (bool)value;
                }
                return true;
            }
            set
            {
                ViewState["UseDefaultJSConfirm"] = value;
            }
        }
        #endregion

        #region Override MyLinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(base.CommandName))
            {
                base.CommandName = ButtonCommandName.Approve;
            }

            //if (this.Page != null && !this.Page.IsPostBack)
            if (this.Page != null)
            {
                #region 權限檢查
                if (this.AutoCheckAuth)
                {
                    this.Enabled = this.HasAuth();
                    if (this.Enabled)
                    {
                        this.ToolTip = String.Empty;
                    }
                    else
                    {
                        this.ToolTip = WebHelper.GetLocalized("CTL_NoMaintainAuth", "無維護權限");
                    }
                }
                #endregion
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.Enabled)
            {
                string msg = WebHelper.GetLocalized("CTL_NoMaintainAuth", "無維護權限");
                base.OnClientClick = String.Concat("return confirm('", msg, "');");
            }
            else if (this.UseDefaultJSConfirm && String.IsNullOrEmpty(base.OnClientClick))
            {
                string msg = WebHelper.GetLocalized("CTL_ApproveConfirm", String.Concat("確定", ButtonText.Approve, "此資料"));
                base.OnClientClick = String.Concat("return confirm('", msg, "');");
            }

            base.OnPreRender(e);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                this.Localize(false);
                base.CommandName = ButtonCommandName.Approve;
            }
            base.RenderContents(output);
        }
        #endregion

        #region Override MyLinkButton's Property
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected override bool CanChanegLocationText
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 要 Localized 的文字，固定為 ButtonText.Approve 常數，設定無效
        /// </summary>
        [DefaultValue(ButtonText.Approve)]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字，固定為 ButtonText.Approve 常數，設定無效")]
        public override string LocationText
        {
            get
            {
                return ButtonText.Approve;
            }
        }
        #endregion

        #region 權限相關
        private bool _AutoCheckAuth = true;
        /// <summary>
        /// 是否自動檢查權限
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("是否自動檢查權限，預設為 true")]
        public virtual bool AutoCheckAuth
        {
            get
            {
                return _AutoCheckAuth;
            }
            set
            {
                _AutoCheckAuth = value;
            }
        }

        private bool? _HasAuth = null;
        /// <summary>
        /// 取得是否有授權
        /// </summary>
        /// <returns></returns>
        private bool HasAuth()
        {
            if (_HasAuth == null)
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                bool isEditPage = false;
                bool isSubPage = false;
                string menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
                MenuAuth menuAuth = logonUser.GetMenuAuth(menuID);
                _HasAuth = menuAuth.HasMaintain();
            }
            return _HasAuth.Value;
        }
        #endregion
    }

    /// <summary>
    /// 開關(啟用或停用)的 MyLinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MySwitchButton runat=server></{0}:MySwitchButton>")]
    public class MySwitchButton : MyLinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MySwitchButton 伺服器控制項
        /// </summary>
        public MySwitchButton()
            : base()
        {
            this.ViewStateMode = ViewStateMode.Inherit;
        }
        #endregion

        #region Property
        /// <summary>
        /// 取得或設定是否要使用預設的 Javscript 確認訊息，預設為 true
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("取得或設定是否要使用預設的 Javscript 確認訊息")]
        public bool UseDefaultJSConfirm
        {
            get
            {
                object value = ViewState["UseDefaultJSConfirm"];
                if (value is bool)
                {
                    return (bool)value;
                }
                return false;
            }
            set
            {
                ViewState["UseDefaultJSConfirm"] = value;
            }
        }
        #endregion

        #region Override MyLinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.ChangeCommandName(this.ShowOnButton);

            //if (this.Page != null && !this.Page.IsPostBack)
            if (this.Page != null)
            {
                #region 權限檢查
                if (this.AutoCheckAuth)
                {
                    this.Enabled = this.HasAuth();
                    if (this.Enabled)
                    {
                        this.ToolTip = String.Empty;
                    }
                    else
                    {
                        this.ToolTip = WebHelper.GetLocalized("CTL_NoMaintainAuth", "無維護權限");
                    }
                }
                #endregion
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.Enabled)
            {
                string msg = WebHelper.GetLocalized("CTL_NoMaintainAuth", "無維護權限");
                base.OnClientClick = String.Concat("return confirm('", msg, "');");
            }
            else if (this.UseDefaultJSConfirm && String.IsNullOrEmpty(base.OnClientClick))
            {
                string msg = null;
                if (this.ShowOnButton)
                {
                    msg = WebHelper.GetLocalized("CTL_EnableConfirm", String.Concat("確定", ButtonText.Enable, "此資料"));
                    base.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    msg = WebHelper.GetLocalized("CTL_DisableConfirm", String.Concat("確定", ButtonText.Disable, "此資料"));
                    base.ForeColor = ForeColor;
                }
                base.OnClientClick = String.Concat("return confirm('", msg, "');");
            }

            base.OnPreRender(e);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                this.Localize(false);
                this.ChangeCommandName(this.ShowOnButton);
            }
            base.RenderContents(output);
        }
        #endregion

        #region Override MyLinkButton's Property
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected override bool CanChanegLocationText
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 要 Localized 的文字，預設為 ButtonText.Enable 常數，設定無效
        /// </summary>
        [DefaultValue(ButtonText.Enable)]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字，預設為 ButtonText.Enable，設定無效")]
        public override string LocationText
        {
            get
            {
                return this.ShowOnButton ? ButtonText.Enable : ButtonText.Disable;
            }
        }
        #endregion

        #region Switch 相關
        /// <summary>
        /// 取得或設定是否顯示為 On 的按鈕 (啟用按鈕)
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        public bool ShowOnButton
        {
            get
            {
                object value = ViewState["ShowOnButton"];
                if (value is bool)
                {
                    return (bool)value;
                }
                else
                {
                    //因為預設是 true
                    return true;
                }
            }
            set
            {
                ViewState["ShowOnButton"] = value;
                this.IsLocalized = false;
                this.ChangeCommandName(value);
            }
        }

        /// <summary>
        /// 改變按鈕的命令名稱
        /// </summary>
        /// <param name="isOnStatus">指定是否顯示 On 按鈕</param>
        private void ChangeCommandName(bool isShowOnButton)
        {
            if (String.IsNullOrEmpty(this.CommandName)
                || this.CommandName == ButtonCommandName.Enable
                || this.CommandName == ButtonCommandName.Disable)
            {
                base.CommandName = isShowOnButton ? ButtonCommandName.Enable : ButtonCommandName.Disable;
            }
        }
        #endregion

        #region 權限相關
        private bool _AutoCheckAuth = true;
        /// <summary>
        /// 是否自動檢查權限
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("是否自動檢查權限，預設為 true")]
        public bool AutoCheckAuth
        {
            get
            {
                return _AutoCheckAuth;
            }
            set
            {
                _AutoCheckAuth = value;
            }
        }

        private bool? _HasAuth = null;
        /// <summary>
        /// 取得是否有授權
        /// </summary>
        /// <returns></returns>
        private bool HasAuth()
        {
            if (_HasAuth == null)
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                bool isEditPage = false;
                bool isSubPage = false;
                string menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
                MenuAuth menuAuth = logonUser.GetMenuAuth(menuID);
                _HasAuth = menuAuth.HasMaintain();
            }
            return _HasAuth.Value;
        }
        #endregion
    }

    /// <summary>
    /// 回上一頁/離開的 MyLinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyGoBackButton runat=server></{0}:MyGoBackButton>")]
    public class MyGoBackButton : MyLinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MyGoBackButton 伺服器控制項
        /// </summary>
        public MyGoBackButton()
            : base()
        {
        }
        #endregion

        #region Override MyLinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Page page = this.Page;
            if (this.Page != null && (!this.Page.IsPostBack || !this.IsViewStateEnabled))
            {
                #region 檢查是否要使用預設的 BackUrl 與 IsEditPage
                bool isEditPage = false;
                bool isSubPage = false;
                {
                    string backUrl = this.BackUrl;
                    //未指定 backUrl
                    if (String.IsNullOrEmpty(backUrl))
                    {
                        string menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
                        if (!String.IsNullOrEmpty(menuID))
                        {
                            MenuInfo menu = null;
                            if (isSubPage)
                            {
                                //編輯頁取選單(功能)代碼自己
                                menu = MenuHelper.Current.GetMenu(menuID);
                            }
                            else
                            {
                                //非編輯頁取選單(功能)代碼的父選單
                                menu = MenuHelper.Current.GetParentMenu(menuID);
                            }
                            if (menu != null)
                            {
                                this.BackUrl = menu.Url;
                            }
                        }
                    }
                }
                #endregion

                this.IsOnSubPage = isSubPage;
            }

            this.ChangeCommandName(this.IsOnSubPage);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                this.Localize(false);
                this.ChangeCommandName(this.IsOnSubPage);
            }
            base.RenderContents(output);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (this.CancelBack)
            {
                return;
            }
            this.GoBack();
        }
        #endregion

        #region Override MyLinkButton's Property
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected override bool CanChanegLocationText
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 要 Localized 的文字，預設為 ButtonText.GoBack 常數，設定無效
        /// </summary>
        [DefaultValue(ButtonText.GoBack)]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字，預設為 ButtonText.GoBack，設定無效")]
        public override string LocationText
        {
            get
            {
                return this.IsOnSubPage ? ButtonText.GoAway : ButtonText.GoBack;
            }
        }
        #endregion

        #region GoBack 相關
        /// <summary>
        /// 要回去的網址 (回上一頁/離開的網址)
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("要回去的網址")]
        public string BackUrl
        {
            get
            {
                object value = ViewState["BackUrl"];
                if (value is string)
                {
                    return value as string;
                }
                return String.Empty;
            }
            set
            {
                ViewState["BackUrl"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 此控制項所在的頁面是否為延伸頁
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        private bool IsOnSubPage
        {
            get
            {
                object value = ViewState["IsOnSubPage"];
                if (value is bool)
                {
                    return (bool)value;
                }
                return false;
            }
            set
            {
                ViewState["IsOnSubPage"] = value;
                this.IsLocalized = false;
            }
        }

        /// <summary>
        /// 改變按鈕的命令名稱
        /// </summary>
        /// <param name="isOnStatus">指定是否在延伸頁面</param>
        private void ChangeCommandName(bool isOnSubPage)
        {
            if (String.IsNullOrEmpty(this.CommandName)
                || this.CommandName == ButtonCommandName.GoAway
                || this.CommandName == ButtonCommandName.GoBack)
            {
                base.CommandName = isOnSubPage ? ButtonCommandName.GoAway : ButtonCommandName.GoBack;
            }
        }

        /// <summary>
        /// 是否取消回上一頁 (離開)
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        public bool CancelBack
        {
            get;
            set;
        }

        /// <summary>
        /// 執行回去的事件
        /// </summary>
        private void GoBack()
        {
            string backUrl = WebHelper.GetResolveUrl(this.Page, this.BackUrl);
            if (this.Page != null && !String.IsNullOrEmpty(backUrl))
            {
                #region [MDY:20210521] 原碼修正
                this.Page.Response.Redirect(WebHelper.GenRNUrl(backUrl));
                #endregion
            }
        }
        #endregion
    }

    /// <summary>
    /// 確定的 MyLinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyOKButton runat=server></{0}:MyOKButton>")]
    public class MyOKButton : MyLinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MyOKButton 伺服器控制項
        /// </summary>
        public MyOKButton()
            : base()
        {
        }
        #endregion

        #region Override MyLinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(base.CommandName))
            {
                base.CommandName = ButtonCommandName.OK;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                this.Localize(false);
                base.CommandName = ButtonCommandName.OK;
            }
            base.RenderContents(output);
        }
        #endregion

        #region Override MyLinkButton's Property
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected override bool CanChanegLocationText
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 要 Localized 的文字，固定為 ButtonText.OK 常數，設定無效
        /// </summary>
        [DefaultValue(ButtonText.OK)]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字，固定為 ButtonText.OK 常數，設定無效")]
        public override string LocationText
        {
            get
            {
                return ButtonText.OK;
            }
        }
        #endregion
    }

    /// <summary>
    /// 確定的 MyLinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyDownloadButton runat=server></{0}:MyDownloadButton>")]
    public class MyDownloadButton : MyLinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MyDownloadButton 伺服器控制項
        /// </summary>
        public MyDownloadButton()
            : base()
        {
        }
        #endregion

        #region Override MyLinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(base.CommandName))
            {
                base.CommandName = ButtonCommandName.Download;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                this.Localize(false);
                base.CommandName = ButtonCommandName.Download;
            }
            base.RenderContents(output);
        }
        #endregion

        #region Override MyLinkButton's Property
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected override bool CanChanegLocationText
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 要 Localized 的文字，固定為 ButtonText.OK 常數，設定無效
        /// </summary>
        [DefaultValue(ButtonText.OK)]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字，固定為 ButtonText.Download 常數，設定無效")]
        public override string LocationText
        {
            get
            {
                return ButtonText.Download;
            }
        }
        #endregion
    }

    /// <summary>
    /// 確定的 MyLinkButton 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyPrintButton runat=server></{0}:MyPrintButton>")]
    public class MyPrintButton : MyLinkButton
    {
        #region Constructor
        /// <summary>
        /// 建構 MyPrintButton 伺服器控制項
        /// </summary>
        public MyPrintButton()
            : base()
        {
        }
        #endregion

        #region Override MyLinkButton's Method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(base.CommandName))
            {
                base.CommandName = ButtonCommandName.Print;
            }

            if (this.Page != null)
            {
                #region 檢查權限
                if (this.AutoCheckAuth)
                {
                    this.Enabled = this.HasAuth();
                    if (this.Enabled)
                    {
                        this.ToolTip = String.Empty;
                    }
                    else
                    {
                        this.ToolTip = WebHelper.GetLocalized("CTL_NoQueryAuth", "無查詢權限");
                    }
                }
                #endregion
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.Enabled)
            {
                string msg = WebHelper.GetLocalized("CTL_NoPrintAuth", "無列印權限");
                base.OnClientClick = String.Concat("return confirm('", msg, "');");
            }

            base.OnPreRender(e);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                this.Localize(false);
                base.CommandName = ButtonCommandName.Print;
            }
            base.RenderContents(output);
        }
        #endregion

        #region Override MyLinkButton's Property
        /// <summary>
        /// 取得是否可改變 LocationText
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(false)]
        [ReadOnly(true)]
        protected override bool CanChanegLocationText
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 要 Localized 的文字，固定為 ButtonText.OK 常數，設定無效
        /// </summary>
        [DefaultValue(ButtonText.OK)]
        [Themeable(false)]
        [ReadOnly(true)]
        [Category("自訂屬性")]
        [Description("要 Localized 的文字，固定為 ButtonText.Download 常數，設定無效")]
        public override string LocationText
        {
            get
            {
                return ButtonText.Download;
            }
        }
        #endregion

        #region 權限相關
        private bool _AutoCheckAuth = true;
        /// <summary>
        /// 是否自動檢查權限
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("是否自動檢查權限，預設為 true")]
        public virtual bool AutoCheckAuth
        {
            get
            {
                return _AutoCheckAuth;
            }
            set
            {
                _AutoCheckAuth = value;
            }
        }

        private bool? _HasAuth = null;
        /// <summary>
        /// 取得是否有授權
        /// </summary>
        /// <returns></returns>
        private bool HasAuth()
        {
            if (_HasAuth == null)
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                bool isEditPage = false;
                bool isSubPage = false;
                string menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
                MenuAuth menuAuth = logonUser.GetMenuAuth(menuID);
                //_HasAuth = menuAuth.HasAnyone();
                _HasAuth = menuAuth.HasPrint();
            }
            return _HasAuth.Value;
        }
        #endregion
    }
}
