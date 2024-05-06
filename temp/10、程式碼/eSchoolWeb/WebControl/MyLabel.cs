using System;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb
{
    /// <summary>
    /// 支援 Location 的 Label 伺服器控制項
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyLabel runat=server></{0}:MyLabel>")]
    public class MyLabel : Label
    {
        #region Constructor
        /// <summary>
        /// 建構 MyLabel 伺服器控制項
        /// </summary>
        public MyLabel()
            : base()
        {
            base.ViewStateMode = ViewStateMode.Disabled;
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
                _LocationText = value == null ? String.Empty : value.Trim();
                this.IsLocalized = false;
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

            if (!String.IsNullOrEmpty(this.LocationText))
            {
                this.LocalizedText = WebHelper.GetLocalized(this.LocationText, this.LocationText);
            }
            else if (!String.IsNullOrEmpty(this.LocalizedText))
            {
                this.LocalizedText = String.Empty;
            }
            this.IsLocalized = true;
        }
        #endregion
    }
}
