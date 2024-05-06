using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    #region Enum
    /// <summary>
    /// Filter1 與 Filter2 使用者控制項的使用介面模式列舉值
    /// </summary>
    public enum FilterUIModeEnum
    {
        /// <summary>
        /// 提供變更選項模式
        /// </summary>
        Option = 0,

        /// <summary>
        /// 顯示選擇項目模式
        /// </summary>
        Label = 1
    }
    #endregion

    #region Const
    /// <summary>
    /// Filter1 與 Filter2 使用者控制項的下拉選項項目常數
    /// </summary>
    public abstract class FilterItemConst
    {
        /// <summary>
        /// 商家代號 : ReceiveType
        /// </summary>
        public const string ReceiveType = "ReceiveType";

        /// <summary>
        /// 學年 : Year
        /// </summary>
        public const string Year = "Year";

        /// <summary>
        /// 學期 : Term
        /// </summary>
        public const string Term = "Term";

        /// <summary>
        /// 部別 : Dep
        /// </summary>
        public const string Dep = "Dep";

        /// <summary>
        /// 代收費用別 : Receive
        /// </summary>
        public const string Receive = "Receive";
    }
    #endregion

    #region EventArgs
    /// <summary>
    /// Filter1 與 Filter2 使用者控制項的事件資料類別
    /// </summary>
    public class FilterEventArgs : EventArgs
    {
        #region Property
        private string _FilterItem = null;
        /// <summary>
        /// 改變選取值的項目 (ReceiveType：商家代號、Year：學年、Term：學期、Dep：部別、Receive：代收費用別)
        /// </summary>
        public string FilterItem
        {
            get
            {
                return _FilterItem;
            }
            set
            {
                _FilterItem = value == null ? null : value.Trim();
            }
        }

        private string _SelectedValue = null;
        /// <summary>
        /// 選取的值
        /// </summary>
        public string SelectedValue
        {
            get
            {
                return _SelectedValue;
            }
            set
            {
                _SelectedValue = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 Filter1 與 Filter2 使用者控制項的事件資料類別
        /// </summary>
        public FilterEventArgs()
        {
        }

        /// <summary>
        /// 建構 Filter1 與 Filter2 使用者控制項的事件資料類別
        /// </summary>
        /// <param name="changeFilter">改變選取值的項目</param>
        /// <param name="selectedValue">選取的值</param>
        public FilterEventArgs(string filterItem, string selectedValue)
        {
            this.FilterItem = filterItem;
            this.SelectedValue = selectedValue;
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// 商家代號、學年、學期連動下拉選項使用者控制項
    /// </summary>
    [DefaultEvent("ItemSelectedIndexChanged")]
    public partial class Filter1 : BaseUserControl
    {
        #region Member
        /// <summary>
        /// 儲存是否已結繫資料的成員變數
        /// </summary>
        private bool _HasDataBound = false;
        #endregion

        #region Propery
        #region 控制屬性
        private FilterUIModeEnum _UIMode = FilterUIModeEnum.Option;
        /// <summary>
        /// 取得或指定介面呈現模式，預設為 Option (提供變更選項模式)
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">將此屬性設定為不屬於 UIModeEnum 列舉型別值</exception>
        [DefaultValue(FilterUIModeEnum.Option)]
        [Themeable(false)]
        public FilterUIModeEnum UIMode
        {
            get
            {
                return _UIMode;
            }
            set
            {
                _UIMode = value;
                this.ChangeUIControl(value);
            }
        }

        private bool _AutoGetDataBound = true;
        /// <summary>
        /// 取得或指定是否在載入此控制項事件時，自動執行 GetDataBound
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">將此屬性設定為不屬於 bool 型別值</exception>
        [DefaultValue(true)]
        [Themeable(false)]
        public bool AutoGetDataBound
        {
            get
            {
                return _AutoGetDataBound;
            }
            set
            {
                _AutoGetDataBound = value;
            }
        }

        /// <summary>
        /// 取得或指定是否在變更最後選項時，自動回傳事件
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">將此屬性設定為不屬於 bool 型別值</exception>
        [DefaultValue(true)]
        [Themeable(false)]
        public bool AutoPostBack
        {
            get
            {
                object value = ViewState["AutoPostBack"];
                if (value is bool)
                {
                    return (bool)value;
                }
                return true;
            }
            set
            {
                ViewState["AutoPostBack"] = value;
            }
        }

        private string _ReceiveKind = ReceiveKindCodeTexts.SCHOOL;
        /// <summary>
        /// 取得或指定限定商家代號的代收種類 (預設為 1，請參考 ReceiveKindCodeTexts)
        /// </summary>
        [DefaultValue(ReceiveKindCodeTexts.SCHOOL)]
        [Themeable(false)]
        public string ReceiveKind
        {
            get
            {
                return _ReceiveKind;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    _ReceiveKind = String.Empty;
                }
                else
                {
                    string receiveKind = value.Trim();
                    if (ReceiveKindCodeTexts.IsDefine(receiveKind))
                    {
                        _ReceiveKind = receiveKind;
                    }
                    else
                    {
                        _ReceiveKind = String.Empty;
                    }
                }
            }
        }

        #region 選項是否顯示
        private bool _ReceiveTypeReadonly = false;
        /// <summary>
        /// 取得或指定商家代號控制項是否唯讀 (顯示 label，UIMode 為 Option 有效)
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        public bool ReceiveTypeReadonly
        {
            get
            {
                return _ReceiveTypeReadonly;
            }
            set
            {
                _ReceiveTypeReadonly = value;
                this.ChangeUIControl(this.UIMode);
            }
        }

        private bool _YearVisible = true;
        /// <summary>
        /// 取得或指定學年控制項是否顯示
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        public bool YearVisible
        {
            get
            {
                return _YearVisible;
            }
            set
            {
                _YearVisible = value;
                this.tdYear.Visible = _YearVisible;
            }
        }

        private bool _TermVisible = true;
        /// <summary>
        /// 取得或指定學期控制項是否顯示
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        public bool TermVisible
        {
            get
            {
                return _TermVisible;
            }
            set
            {
                _TermVisible = value;
                this.tdTerm.Visible = _TermVisible;
            }
        }
        #endregion

        #region 預設項目模式
        private DefaultItem.Mode _ReceiveTypeDefaultMode = DefaultItem.Mode.First;
        /// <summary>
        /// 取得或指定商家代號預設項目模式，預設為 First (預選第一個有效的項目)
        /// </summary>
        [DefaultValue(DefaultItem.Mode.First)]
        [Themeable(false)]
        public DefaultItem.Mode ReceiveTypeDefaultMode
        {
            get
            {
                return _ReceiveTypeDefaultMode;
            }
            set
            {
                _ReceiveTypeDefaultMode = value;
            }
        }

        private DefaultItem.Mode _YearDefaultMode = DefaultItem.Mode.First;
        /// <summary>
        /// 取得或指定學年預設項目模式，預設為 First (預選第一個有效的項目)
        /// </summary>
        [DefaultValue(DefaultItem.Mode.First)]
        [Themeable(false)]
        public DefaultItem.Mode YearDefaultMode
        {
            get
            {
                return _YearDefaultMode;
            }
            set
            {
                _YearDefaultMode = value;
            }
        }

        private DefaultItem.Mode _TermDefaultMode = DefaultItem.Mode.First;
        /// <summary>
        /// 取得或指定學期預設項目模式，預設為 First (預選第一個有效的項目)
        /// </summary>
        [DefaultValue(DefaultItem.Mode.First)]
        [Themeable(false)]
        public DefaultItem.Mode TermDefaultMode
        {
            get
            {
                return _TermDefaultMode;
            }
            set
            {
                _TermDefaultMode = value;
            }
        }
        #endregion

        #region 預設項目種類
        private DefaultItem.Kind _ReceiveTypeDefaultKind = DefaultItem.Kind.None;
        /// <summary>
        /// 取得或指定商家代號預設項目種類，預設為 None (不產生預設項目)
        /// </summary>
        [DefaultValue(DefaultItem.Kind.None)]
        [Themeable(false)]
        public DefaultItem.Kind ReceiveTypeDefaultKind
        {
            get
            {
                return _ReceiveTypeDefaultKind;
            }
            set
            {
                _ReceiveTypeDefaultKind = value;
            }
        }

        private DefaultItem.Kind _YearDefaultKind = DefaultItem.Kind.None;
        /// <summary>
        /// 取得或指定學年預設項目種類，預設為 None (不產生預設項目)
        /// </summary>
        [DefaultValue(DefaultItem.Kind.None)]
        [Themeable(false)]
        public DefaultItem.Kind YearDefaultKind
        {
            get
            {
                return _YearDefaultKind;
            }
            set
            {
                _YearDefaultKind = value;
            }
        }

        private DefaultItem.Kind _TermDefaultKind = DefaultItem.Kind.None;
        /// <summary>
        /// 取得或指定學期預設項目種類，預設為 None (不產生預設項目)
        /// </summary>
        [DefaultValue(DefaultItem.Kind.None)]
        [Themeable(false)]
        public DefaultItem.Kind TermDefaultKind
        {
            get
            {
                return _TermDefaultKind;
            }
            set
            {
                _TermDefaultKind = value;
            }
        }
        #endregion
        #endregion

        #region 選擇的商家代號
        private string _SelectedReceiveType = null;
        private string _SelectedReceiveTypeName = null;

        /// <summary>
        /// 取得選擇的商家代號代碼
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Themeable(false)]
        public string SelectedReceiveType
        {
            get
            {
                if (_SelectedReceiveType == null)
                {
                    object value = ViewState["SelectedReceiveType"];
                    if (value is string)
                    {
                        _SelectedReceiveType = value as string;
                    }
                    else
                    {
                        _SelectedReceiveType = String.Empty;
                        ViewState["SelectedReceiveType"] = _SelectedReceiveType;
                    }
                }
                return _SelectedReceiveType;
            }
            private set
            {
                _SelectedReceiveType = value == null ? String.Empty : value.Trim();
                ViewState["SelectedReceiveType"] = _SelectedReceiveType;
            }
        }

        /// <summary>
        /// 取得或指定選擇的商家代號名稱
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Themeable(false)]
        public string SelectedReceiveTypeName
        {
            get
            {
                if (_SelectedReceiveTypeName == null)
                {
                    object value = ViewState["SelectedReceiveTypeName"];
                    if (value is string)
                    {
                        _SelectedReceiveTypeName = value as string;
                    }
                    else
                    {
                        _SelectedReceiveTypeName = String.Empty;
                        ViewState["SelectedReceiveTypeName"] = _SelectedReceiveTypeName;
                    }
                }
                return _SelectedReceiveTypeName;
            }
            private set
            {
                _SelectedReceiveTypeName = value == null ? String.Empty : value.Trim();
                ViewState["SelectedReceiveTypeName"] = _SelectedReceiveTypeName;
            }
        }
        #endregion

        #region 選擇的學年
        private string _SelectedYearID = null;
        private string _SelectedYearName = null;

        /// <summary>
        /// 取得選擇的學年代碼
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Themeable(false)]
        public string SelectedYearID
        {
            get
            {
                if (_SelectedYearID == null)
                {
                    object value = ViewState["SelectedYearID"];
                    if (value is string)
                    {
                        _SelectedYearID = value as string;
                    }
                    else
                    {
                        _SelectedYearID = String.Empty;
                        ViewState["SelectedYearID"] = _SelectedYearID;
                    }
                }
                return _SelectedYearID;
            }
            private set
            {
                _SelectedYearID = value == null ? String.Empty : value.Trim();
                ViewState["SelectedYearID"] = _SelectedYearID;
            }
        }

        /// <summary>
        /// 取得選擇的學年名稱
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Themeable(false)]
        public string SelectedYearName
        {
            get
            {
                if (_SelectedYearName == null)
                {
                    object value = ViewState["SelectedYearName"];
                    if (value is string)
                    {
                        _SelectedYearName = value as string;
                    }
                    else
                    {
                        _SelectedYearName = String.Empty;
                        ViewState["SelectedYearName"] = _SelectedYearName;
                    }
                }
                return _SelectedYearName;
            }
            private set
            {
                _SelectedYearName = value == null ? String.Empty : value.Trim();
                ViewState["SelectedYearName"] = _SelectedYearName;
            }
        }
        #endregion

        #region 選擇的學期
        private string _SelectedTermID = null;
        private string _SelectedTermName = null;

        /// <summary>
        /// 取得選擇的學期代碼
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Themeable(false)]
        public string SelectedTermID
        {
            get
            {
                if (_SelectedTermID == null)
                {
                    object value = ViewState["SelectedTermID"];
                    if (value is string)
                    {
                        _SelectedTermID = value as string;
                    }
                    else
                    {
                        _SelectedTermID = String.Empty;
                        ViewState["SelectedTermID"] = _SelectedTermID;
                    }
                }
                return _SelectedTermID;
            }
            set
            {
                _SelectedTermID = value == null ? String.Empty : value.Trim();
                ViewState["SelectedTermID"] = _SelectedTermID;
            }
        }

        /// <summary>
        /// 取得選擇的學期名稱
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Themeable(false)]
        public string SelectedTermName
        {
            get
            {
                if (_SelectedTermName == null)
                {
                    object value = ViewState["SelectedTermName"];
                    if (value is string)
                    {
                        _SelectedTermName = value as string;
                    }
                    else
                    {
                        _SelectedTermName = String.Empty;
                        ViewState["SelectedTermName"] = _SelectedTermName;
                    }
                }
                return _SelectedTermName;
            }
            set
            {
                _SelectedTermName = value == null ? String.Empty : value.Trim();
                ViewState["SelectedTermName"] = _SelectedTermName;
            }
        }
        #endregion

        #region 連動 Filter2 控制項
        /// <summary>
        /// 要連動的 Filter2 控制項 ID
        /// </summary>
        public string Filter2ControlID
        {
            get
            {
                return ViewState["Filter2ControlID"] as string;
            }
            set
            {
                ViewState["Filter2ControlID"] = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion
        #endregion

        #region 事件與處理常式
        #region 改變下拉選項選擇項目的事件與處理常式
        /// <summary>
        /// 發生於當下拉選項的選擇項目在發佈至伺服器期間變更時。
        /// </summary>
        public event EventHandler<FilterEventArgs> ItemSelectedIndexChanged = null;

        /// <summary>
        /// 引發下拉選項選擇項目變更事件，這允許您提供該事件的自訂處理常式
        /// </summary>
        /// <param name="e">包含事件資料</param>
        protected virtual void OnItemSelectedIndexChanged(FilterEventArgs e)
        {
            if (this.ItemSelectedIndexChanged != null)
            {
                this.ItemSelectedIndexChanged(this, e);
            }
        }
        #endregion
        #endregion

        #region Method
        /// <summary>
        /// 依據指定的介面呈現模式列舉值改變顯示的控制項
        /// </summary>
        /// <param name="uiMode">指定的介面呈現模式列舉值</param>
        private void ChangeUIControl(FilterUIModeEnum uiMode)
        {
            bool showLabel = uiMode == FilterUIModeEnum.Label;
            bool showOption = uiMode == FilterUIModeEnum.Option;

            this.labReceiveType.Visible = showLabel;
            this.labYear.Visible = showLabel;
            this.labTerm.Visible = showLabel;

            this.ddlReceiveType.Visible = showOption;
            this.ddlYear.Visible = showOption;
            this.ddlTerm.Visible = showOption;

            if (showOption)
            {
                this.ddlReceiveType.Visible = !this.ReceiveTypeReadonly;
                this.labReceiveType.Visible = this.ReceiveTypeReadonly;
            }
        }

        /// <summary>
        /// 連動 Filter2 控制項結繫資料
        /// </summary>
        /// <param name="option">要結繫的主要條件選項資料</param>
        /// <param name="receiveID">原始的代收費用別代碼。</param>
        private void Filter2BindData(FilterOption option, string receiveID)
        {
            Filter2 filter2 = null;
            string controlID = this.Filter2ControlID;
            if (!String.IsNullOrEmpty(controlID))
            {
                filter2 = this.FindFilter2Control(controlID);
                if (filter2 != null)
                {
                    #region 處理 filter2 的 Selected 項目
                    //if (filter2.DepDefaultKind != DefaultItem.Kind.None && String.IsNullOrWhiteSpace(depID))
                    //{
                    //    option.SelectedDepID = DefaultItem.GetCode(filter2.DepDefaultKind);
                    //}
                    if (filter2.ReceiveDefaultKind != DefaultItem.Kind.None && String.IsNullOrWhiteSpace(receiveID))
                    {
                        option.SelectedReceiveID = DefaultItem.GetCode(filter2.ReceiveDefaultKind);
                    }
                    #endregion

                    filter2.BindData(option);
                }
            }
        }

        /// <summary>
        /// 取得 Filter2 控制項的 DataMode
        /// </summary>
        /// <returns></returns>
        private string GetFilter2DataMode()
        {
            Filter2 filter2 = null;
            string controlID = this.Filter2ControlID;
            if (!String.IsNullOrEmpty(controlID))
            {
                filter2 = this.FindFilter2Control(controlID);
            }
            return (filter2 != null && filter2.UIMode == FilterUIModeEnum.Label) ? "L" : "O";
        }

        /// <summary>
        /// 取得 Filter2 控制項的部別、代收費用別的預設項目模式
        /// </summary>
        /// <param name="depDefaultMode">傳回部別預設項目模式，無 Filter2 控制項時傳回 DefaultItem.Mode.ByKind。</param>
        /// <param name="receiveDefaultMode">傳回代收費用別預設項目模式，無 Filter2 控制項時傳回 DefaultItem.Mode.ByKind。</param>
        /// <returns>如果有 Filter2 控制項則傳回 true，否則傳回 false。</returns>
        private bool GetFilter2DefaultModes(out DefaultItem.Mode depDefaultMode, out DefaultItem.Mode receiveDefaultMode)
        {
            depDefaultMode = DefaultItem.Mode.ByKind;
            receiveDefaultMode = DefaultItem.Mode.ByKind;

            string controlID = this.Filter2ControlID;
            if (!String.IsNullOrEmpty(controlID))
            {
                Filter2 filter2 = this.FindFilter2Control(controlID);
                if (filter2 != null)
                {
                    depDefaultMode = filter2.DepDefaultMode;
                    receiveDefaultMode = filter2.ReceiveDefaultMode;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 找出 Filter2 控制項
        /// </summary>
        /// <returns>找到則傳回 filter2 控制項物件</returns>
        private Filter2 FindFilter2Control(string controlID)
        {
            Filter2 filter2 = null;
            if (!String.IsNullOrEmpty(controlID))
            {
                if (controlID.IndexOf("$") > -1)
                {
                    filter2 = this.Page.FindControl(controlID) as Filter2;
                }
                else
                {
                    filter2 = this.Parent.FindControl(controlID) as Filter2;
                }
            }

            return filter2;
        }

        /// <summary>
        /// 取得主要條件選項資料
        /// </summary>
        /// <param name="receiveType">商家代號代碼</param>
        /// <param name="yearID">學年代碼</param>
        /// <param name="termID">學期代碼</param>
        /// <param name="depID">部別代碼</param>
        /// <param name="receiveID">代收費用別代碼</param>
        /// <param name="receiveKind">商家代號代收種類</param>
        /// <param name="option">傳回主要條件選項資料</param>
        /// <returns>傳回處理結果</returns>
        private XmlResult GetData(string receiveType, string yearID, string termID, string depID, string receiveID
            , string receiveKind, out FilterOption option)
        {
            string f1Mode = this.UIMode == FilterUIModeEnum.Label ? "L" : "O";
            string f2Mode = this.GetFilter2DataMode();

            DefaultItem.Mode depDefaultMode = DefaultItem.Mode.ByKind;
            DefaultItem.Mode receiveDefaultMode = DefaultItem.Mode.ByKind;
            this.GetFilter2DefaultModes(out depDefaultMode, out receiveDefaultMode);

            #region [MDY:202203XX] 2022擴充案 英文名稱相關
            bool isEngUI = this.isEngUI();

            XmlResult xmlResult = DataProxy.Current.GetFilterOption(this.Page, (f1Mode + f2Mode)
                , receiveType, yearID, termID, depID, receiveID, receiveKind, isEngUI
                , this.ReceiveTypeDefaultMode, this.YearDefaultMode, this.TermDefaultMode, depDefaultMode, receiveDefaultMode
                , out option);
            #endregion

            return xmlResult;
        }

        /// <summary>
        /// 取得指定條件的選項資料並結繫
        /// </summary>
        /// <param name="receiveType">商家代號代碼</param>
        /// <param name="yearID">學年代碼</param>
        /// <param name="termID">學期代碼</param>
        /// <returns>傳回處理結果</returns>
        public XmlResult GetDataAndBind(string receiveType, string yearID, string termID)
        {
            return this.GetDataAndBind(receiveType, yearID, termID, null, null);
        }

        /// <summary>
        /// 取得指定條件的選項資料並結繫
        /// </summary>
        /// <param name="receiveType">商家代號代碼</param>
        /// <param name="yearID">學年代碼</param>
        /// <param name="termID">學期代碼</param>
        /// <param name="depID">部別代碼</param>
        /// <param name="receiveID">代收費用別代碼</param>
        /// <returns>傳回處理結果</returns>
        public XmlResult GetDataAndBind(string receiveType, string yearID, string termID, string depID, string receiveID)
        {
            FilterOption option = null;
            XmlResult xmlResult = this.GetData(receiveType, yearID, termID, depID, receiveID, this.ReceiveKind, out option);

            this.BindData(option);

            //連動 Filter2
            //this.Filter2BindData(option, depID, receiveID);
            this.Filter2BindData(option, receiveID);

            return xmlResult;
        }

        /// <summary>
        /// 結繫主要條件選項資料
        /// </summary>
        /// <param name="option">主要條件選項資料</param>
        public void BindData(FilterOption option)
        {
            if (option == null)
            {
                #region 商家代號
                this.SelectedReceiveType = null;
                this.SelectedReceiveTypeName = null;
                this.ddlReceiveType.Items.Clear();
                this.labReceiveType.Text = String.Empty;    // receiveType;
                #endregion

                #region 學年
                this.SelectedYearID = null;
                this.SelectedYearName = null;
                this.ddlYear.Items.Clear();
                this.labYear.Text = String.Empty;    //yearID;
                #endregion

                #region 學期
                this.SelectedTermID = null;
                this.SelectedTermName = null;
                this.ddlTerm.Items.Clear();
                this.labTerm.Text = String.Empty;    //termID;
                #endregion
            }
            else
            {
                #region 商家代號
                {
                    string selectedValue = WebHelper.SetDropDownListItems(this.ddlReceiveType, this.ReceiveTypeDefaultKind, false, option.ReceiveTypeDatas, true, false, 0, option.SelectedReceiveType);
                    if (selectedValue != option.SelectedReceiveType)
                    {
                        option.SelectedReceiveType = selectedValue;
                    }

                    this.SelectedReceiveType = option.SelectedReceiveType;
                    this.SelectedReceiveTypeName = option.GetSelectedReceiveTypeName();

                    this.labReceiveType.Text = HttpUtility.HtmlEncode(String.Format("{0}({1})", this.SelectedReceiveType, this.SelectedReceiveTypeName));
                }
                #endregion

                #region 學年
                {
                    string selectedValue = WebHelper.SetDropDownListItems(this.ddlYear, this.YearDefaultKind, false, option.YearDatas, true, false, 0, option.SelectedYearID);
                    if (selectedValue != option.SelectedReceiveType)
                    {
                        option.SelectedYearID = selectedValue;
                    }

                    this.SelectedYearID = option.SelectedYearID;
                    this.SelectedYearName = option.GetSelectedYearName();

                    this.labYear.Text = HttpUtility.HtmlEncode(String.Format("{0}({1})", this.SelectedYearName, this.SelectedYearID));
                }
                #endregion

                #region 學期
                {
                    string selectedValue = WebHelper.SetDropDownListItems(this.ddlTerm, this.TermDefaultKind, false, option.TermDatas, true, false, 0, option.SelectedTermID);
                    if (selectedValue != option.SelectedTermID)
                    {
                        option.SelectedTermID = selectedValue;
                    }

                    this.SelectedTermID = option.SelectedTermID;
                    this.SelectedTermName = option.GetSelectedTermName();

                    #region [MDY:20200309] CHECKMARX Reflected XSS All Clients Revision
                    this.labTerm.Text = HttpUtility.HtmlEncode(String.Format("{0}({1})", this.SelectedTermName, this.SelectedTermID));
                    #endregion
                }
                #endregion
            }

            #region 註記已結繫資料
            //因為 Page 的 Page_Load 比 控制項的 Page_Load 早執行，
            //設定此變數為 true，避免 Page_Load 在次結繫資料
            _HasDataBound = true;
            #endregion
        }

        /// <summary>
        /// 以指定的資料結繫
        /// </summary>
        /// <param name="receiveType">商家代號代碼</param>
        /// <param name="receiveTypeName">商家代號名稱</param>
        /// <param name="yearID">學年代碼</param>
        /// <param name="yearName">學年名稱</param>
        /// <param name="termID">學期代碼</param>
        /// <param name="termName">學期名稱</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        /// <remarks>此方法主要是用在將此物件當做 Label 顯示且已知個選項的資料時。要注意的是</remarks>
        public void BindData(string receiveType, string receiveTypeName
            , string yearID, string yearName
            , string termID, string termName)
        {
            #region 商家代號
            {
                CodeText[] items = null;
                if (String.IsNullOrWhiteSpace(yearID))
                {
                    this.SelectedReceiveType = null;
                    this.SelectedReceiveTypeName = null;
                }
                else
                {
                    this.SelectedReceiveType = receiveType;
                    this.SelectedReceiveTypeName = receiveTypeName;
                    items = new CodeText[] { new CodeText(this.SelectedReceiveType, this.SelectedReceiveTypeName) };
                }

                WebHelper.SetDropDownListItems(this.ddlReceiveType, this.ReceiveTypeDefaultKind, false, items, true, false, 0, this.SelectedReceiveType);
                this.labReceiveType.Text = HttpUtility.HtmlEncode(String.Format("{0}({1})", this.SelectedReceiveType, this.SelectedReceiveTypeName));
            }
            #endregion

            #region 學年
            {
                CodeText[] items = null;
                if (String.IsNullOrWhiteSpace(yearID))
                {
                    this.SelectedYearID = null;
                    this.SelectedYearName = null;
                }
                else
                {
                    this.SelectedYearID = yearID;
                    this.SelectedYearName = yearName;
                    items = new CodeText[] { new CodeText(this.SelectedYearID, this.SelectedYearName) };
                }

                WebHelper.SetDropDownListItems(this.ddlYear, this.YearDefaultKind, false, items, true, false, 0, this.SelectedYearID);
                this.labYear.Text = HttpUtility.HtmlEncode(String.Format("{0}({1})", this.SelectedYearName, this.SelectedYearID));
            }
            #endregion

            #region 學期
            {
                CodeText[] items = null;
                if (String.IsNullOrWhiteSpace(termID))
                {
                    this.SelectedTermID = null;
                    this.SelectedTermName = null;
                }
                else
                {
                    this.SelectedTermID = termID;
                    this.SelectedTermName = termName;
                    items = new CodeText[] { new CodeText(this.SelectedTermID, this.SelectedTermName) };
                }

                WebHelper.SetDropDownListItems(this.ddlTerm, this.TermDefaultKind, false, items, true, false, 0, this.SelectedTermID);
                this.labTerm.Text = HttpUtility.HtmlEncode(String.Format("{0}({1})", this.SelectedTermName, this.SelectedTermID));
            }
            #endregion

            #region 註記已結繫資料
            //因為 Page 的 Page_Load 比 控制項的 Page_Load 早執行，
            //設定此變數為 true，避免 Page_Load 在次結繫資料
            _HasDataBound = true;
            #endregion
        }
        #endregion

        #region Override BaseUserControl's Method
        protected override void OnPreRender(EventArgs e)
        {
            this.ddlReceiveType.AutoPostBack = true;
            this.ddlYear.AutoPostBack = true;
            this.ddlTerm.AutoPostBack = true;
            if (!this.AutoPostBack)
            {
                if (this.TermVisible)
                {
                    this.ddlTerm.AutoPostBack = false;
                }
                else if (this.YearVisible)
                {
                    this.ddlYear.AutoPostBack = false;
                }
                else
                {
                    this.ddlReceiveType.AutoPostBack = false;
                }
            }
            base.OnPreRender(e);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //因為 Page 的 Page_Load 比 控制項的 Page_Load 早執行，
            //所以如果已結繫資料 (_HasDataBound == true) 就不要再執行 GetDataBound
            if (!this.IsPostBack && this.AutoGetDataBound && !_HasDataBound)
            {
                string receiveType = null;
                string yearID = null;
                string termID = null;
                string depID = null;
                string receiveID = null;
                WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out receiveID);
                this.GetDataAndBind(receiveType, yearID, termID, depID, receiveID);
            }
        }

        protected void ddlReceiveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item = this.ddlReceiveType.SelectedItem;
            this.SelectedReceiveType = (item == null ? String.Empty : item.Value);
            this.SelectedTermID = String.Empty;
            this.GetDataAndBind(this.SelectedReceiveType, this.SelectedYearID, this.SelectedTermID);

            FilterEventArgs eArgs = new FilterEventArgs(FilterItemConst.ReceiveType, this.SelectedReceiveType);
            this.OnItemSelectedIndexChanged(eArgs);
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item = this.ddlYear.SelectedItem;
            this.SelectedYearID = (item == null ? String.Empty : item.Value);
            this.SelectedTermID = String.Empty;
            this.GetDataAndBind(this.SelectedReceiveType, this.SelectedYearID, this.SelectedTermID);

            FilterEventArgs eArgs = new FilterEventArgs(FilterItemConst.Year, this.SelectedYearID);
            this.OnItemSelectedIndexChanged(eArgs);
        }

        protected void ddlTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item = this.ddlTerm.SelectedItem;
            this.SelectedTermID = (item == null ? String.Empty : item.Value);
            if (!String.IsNullOrEmpty(this.Filter2ControlID))
            {
                this.GetDataAndBind(this.SelectedReceiveType, this.SelectedYearID, this.SelectedTermID);
            }

            FilterEventArgs eArgs = new FilterEventArgs(FilterItemConst.Term, this.SelectedTermID);
            this.OnItemSelectedIndexChanged(eArgs);
        }
    }
}