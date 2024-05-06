using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb
{
    /// <summary>
    /// 支援 Location 的 BoundField 伺服器控制項
    /// </summary>
    [DefaultProperty("HeaderText")]
    [ToolboxData("<{0}:MyBoundField runat=server></{0}:MyBoundField>")]
    public class MyBoundField : BoundField
    {
        #region Override BoundField's Method
        /// <summary>
        /// 初始化指定的 System.Web.UI.WebControls.TableCell 物件為指定的資料列狀態
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <param name="rowIndex"></param>
        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            switch (cellType)
            {
                case DataControlCellType.Header:
                    if (!String.IsNullOrEmpty(this.LocationHeaderText))
                    {
                        cell.Text = this.LocalizedHeaderText;
                    }
                    break;
                case DataControlCellType.Footer:
                    if (!String.IsNullOrEmpty(this.LocationFooterText))
                    {
                        cell.Text = this.LocalizedFooterText;
                    }
                    break;
            }
        }
        #endregion

        #region Localized 相關
        private string _LocationHeaderText = String.Empty;
        /// <summary>
        /// 要 Localized 的 Header 文字
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("要 Localized 的 Header 文字")]
        public virtual string LocationHeaderText
        {
            get
            {
                return _LocationHeaderText;
            }
            set
            {
                _LocationHeaderText = value == null ? String.Empty : value.Trim();
                if (String.IsNullOrEmpty(_LocationHeaderText))
                {
                    _LocalizedHeaderText = String.Empty;
                }
                else
                {
                    this.LocalizedHeaderText = WebHelper.GetLocalized(_LocationHeaderText, _LocationHeaderText);
                }
            }
        }

        private string _LocalizedHeaderText = String.Empty;
        /// <summary>
        /// 已 Localized 的 Header 文字
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("已 Localized 的 Header 文字")]
        public virtual string LocalizedHeaderText
        {
            get
            {
                return _LocalizedHeaderText;
            }
            protected set
            {
                _LocalizedHeaderText = value == null ? String.Empty : value;
            }
        }

        private string _LocationFooterText = String.Empty;
        /// <summary>
        /// 要 Localized 的 Footer 文字
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("要 Localized 的 Footer 文字")]
        public virtual string LocationFooterText
        {
            get
            {
                return _LocationFooterText;
            }
            set
            {
                _LocationFooterText = value == null ? String.Empty : value.Trim();
                if (String.IsNullOrEmpty(_LocationFooterText))
                {
                    _LocalizedFooterText = String.Empty;
                }
                else
                {
                    this.LocalizedFooterText = WebHelper.GetLocalized(_LocationFooterText, _LocationFooterText);
                }
            }
        }

        private string _LocalizedFooterText = String.Empty;
        /// <summary>
        /// 已 Localized 的 Footer 文字
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Category("自訂屬性")]
        [Description("已 Localized 的 Footer 文字")]
        public virtual string LocalizedFooterText
        {
            get
            {
                return _LocalizedFooterText;
            }
            protected set
            {
                _LocalizedFooterText = value == null ? String.Empty : value;
            }
        }
        #endregion
    }
}
