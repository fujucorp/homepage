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
    /// <summary>
    /// 部別、代收費用別連動下拉選項使用者控制項
    /// </summary>
    [DefaultEvent("ItemSelectedIndexChanged")]
    public partial class Filter2 : BaseUserControl
    {
        #region Private Property
        /// <summary>
        /// 儲存是否已結繫資料的成員變數
        /// </summary>
        private bool _HasDataBound = false;

        /// <summary>
        /// 儲存查詢或結繫資料的業務別碼代碼
        /// </summary>
        private string QueryReceiveType
        {
            get
            {
                return ViewState["QueryReceiveType"] as string;
            }
            set
            {
                ViewState["QueryReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢或結繫資料的學年代碼
        /// </summary>
        private string QueryYearID
        {
            get
            {
                return ViewState["QueryYearID"] as string;
            }
            set
            {
                ViewState["QueryYearID"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢或結繫資料的學期代碼
        /// </summary>
        private string QueryTermID
        {
            get
            {
                return ViewState["QueryTermID"] as string;
            }
            set
            {
                ViewState["QueryTermID"] = value == null ? null : value.Trim();
            }
        }
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

        //private string _ReceiveKind = ReceiveKindCodeTexts.SCHOOL;
        ///// <summary>
        ///// 取得或指定限定商家代號的代收種類 (預設為 1，請參考 ReceiveKindCodeTexts)
        ///// </summary>
        //[DefaultValue(ReceiveKindCodeTexts.SCHOOL)]
        //[Themeable(false)]
        //public string ReceiveKind
        //{
        //    get
        //    {
        //        return _ReceiveKind;
        //    }
        //    set
        //    {
        //        if (String.IsNullOrWhiteSpace(value))
        //        {
        //            _ReceiveKind = String.Empty;
        //        }
        //        else
        //        {
        //            string receiveKind = value.Trim();
        //            if (ReceiveKindCodeTexts.IsDefine(receiveKind))
        //            {
        //                _ReceiveKind = receiveKind;
        //            }
        //            else
        //            {
        //                _ReceiveKind = String.Empty;
        //            }
        //        }
        //    }
        //}

        #region 選項是否顯示
        private bool _ReceiveVisible = true;
        /// <summary>
        /// 取得或指定代收費用別控制項是否顯示
        /// </summary>
        [DefaultValue(true)]
        [Themeable(false)]
        public bool ReceiveVisible
        {
            get
            {
                return _ReceiveVisible;
            }
            set
            {
                _ReceiveVisible = value;
                this.tdReceive.Visible = _ReceiveVisible;
            }
        }
        #endregion

        #region 預設項目模式
        private DefaultItem.Mode _DepDefaultMode = DefaultItem.Mode.First;
        /// <summary>
        /// 取得或指定部別預設項目模式，預設為 First (預選第一個有效的項目)
        /// </summary>
        [DefaultValue(DefaultItem.Mode.First)]
        [Themeable(false)]
        public DefaultItem.Mode DepDefaultMode
        {
            get
            {
                return _DepDefaultMode;
            }
            set
            {
                _DepDefaultMode = value;
            }
        }

        private DefaultItem.Mode _ReceiveDefaultMode = DefaultItem.Mode.First;
        /// <summary>
        /// 取得或指定代收費用別預設項目模式，預設為 First (預選第一個有效的項目)
        /// </summary>
        [DefaultValue(DefaultItem.Mode.First)]
        [Themeable(false)]
        public DefaultItem.Mode ReceiveDefaultMode
        {
            get
            {
                return _ReceiveDefaultMode;
            }
            set
            {
                _ReceiveDefaultMode = value;
            }
        }
        #endregion

        #region 預設項目種類
        //private DefaultItem.Kind _DepDefaultKind = DefaultItem.Kind.None;
        ///// <summary>
        ///// 取得或指定部別預設項目種類，預設為 None (不產生預設項目)
        ///// </summary>
        //[DefaultValue(DefaultItem.Kind.None)]
        //[Themeable(false)]
        //public DefaultItem.Kind DepDefaultKind
        //{
        //    get
        //    {
        //        return _DepDefaultKind;
        //    }
        //    set
        //    {
        //        _DepDefaultKind = value;
        //    }
        //}

        private DefaultItem.Kind _ReceiveDefaultKind = DefaultItem.Kind.None;
        /// <summary>
        /// 取得或指定代收費用別預設項目種類，預設為 None (不產生預設項目)
        /// </summary>
        [DefaultValue(DefaultItem.Kind.None)]
        [Themeable(false)]
        public DefaultItem.Kind ReceiveDefaultKind
        {
            get
            {
                return _ReceiveDefaultKind;
            }
            set
            {
                _ReceiveDefaultKind = value;
            }
        }
        #endregion
        #endregion

        #region 選擇的部別
        //private string _SelectedDepID = null;
        //private string _SelectedDepName = null;

        ///// <summary>
        ///// 取得或指定選擇的部別代碼
        ///// </summary>
        //[DefaultValue("")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[Browsable(false)]
        //[Themeable(false)]
        //public string SelectedDepID
        //{
        //    get
        //    {
        //        if (_SelectedDepID == null)
        //        {
        //            object value = ViewState["SelectedDepID"];
        //            if (value is string)
        //            {
        //                _SelectedDepID = value as string;
        //            }
        //            else
        //            {
        //                _SelectedDepID = String.Empty;
        //                ViewState["SelectedDepID"] = _SelectedDepID;
        //            }
        //        }
        //        return _SelectedDepID;
        //    }
        //    private set
        //    {
        //        _SelectedDepID = value == null ? String.Empty : value.Trim();
        //        ViewState["SelectedDepID"] = _SelectedDepID;
        //    }
        //}

        ///// <summary>
        ///// 取得或指定選擇的部別名稱
        ///// </summary>
        //[DefaultValue("")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[Browsable(false)]
        //[Themeable(false)]
        //public string SelectedDepName
        //{
        //    get
        //    {
        //        if (_SelectedDepName == null)
        //        {
        //            object value = ViewState["SelectedDepName"];
        //            if (value is string)
        //            {
        //                _SelectedDepName = value as string;
        //            }
        //            else
        //            {
        //                _SelectedDepName = String.Empty;
        //                ViewState["SelectedDepName"] = _SelectedDepName;
        //            }
        //        }
        //        return _SelectedDepName;
        //    }
        //    private set
        //    {
        //        _SelectedDepName = value == null ? String.Empty : value.Trim();
        //        ViewState["SelectedDepName"] = _SelectedDepName;
        //    }
        //}
        #endregion

        #region 選擇的代收費用別
        private string _SelectedReceiveID = null;
        private string _SelectedReceiveName = null;

        /// <summary>
        /// 取得或指定選擇的代收費用別代碼
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Themeable(false)]
        public string SelectedReceiveID
        {
            get
            {
                if (_SelectedReceiveID == null)
                {
                    object value = ViewState["SelectedReceiveID"];
                    if (value is string)
                    {
                        _SelectedReceiveID = value as string;
                    }
                    else
                    {
                        _SelectedReceiveID = String.Empty;
                        ViewState["SelectedReceiveID"] = _SelectedReceiveID;
                    }
                }
                return _SelectedReceiveID;
            }
            private set
            {
                _SelectedReceiveID = value == null ? String.Empty : value.Trim();
                ViewState["SelectedReceiveID"] = _SelectedReceiveID;
            }
        }

        /// <summary>
        /// 取得或指定選擇的代收費用別名稱
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Themeable(false)]
        public string SelectedReceiveName
        {
            get
            {
                if (_SelectedReceiveName == null)
                {
                    object value = ViewState["SelectedReceiveName"];
                    if (value is string)
                    {
                        _SelectedReceiveName = value as string;
                    }
                    else
                    {
                        _SelectedReceiveName = String.Empty;
                        ViewState["SelectedReceiveName"] = _SelectedReceiveName;
                    }
                }
                return _SelectedReceiveName;
            }
            private set
            {
                _SelectedReceiveName = value == null ? String.Empty : value.Trim();
                ViewState["SelectedReceiveName"] = _SelectedReceiveName;
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

            //this.labDep.Visible = showLabel;
            this.labReceive.Visible = showLabel;

            //this.ddlDep.Visible = showOption;
            this.ddlReceive.Visible = showOption;
        }

        /// <summary>
        /// 取得主要條件選項資料
        /// </summary>
        /// <param name="receiveType">業務別碼代碼</param>
        /// <param name="yearID">學年代碼</param>
        /// <param name="termID">學期代碼</param>
        /// <param name="depID">部別代碼</param>
        /// <param name="receiveID">代收費用別代碼</param>
        /// <param name="option">傳回主要條件選項資料</param>
        /// <returns>傳回處理結果</returns>
        private XmlResult GetData(string receiveType, string yearID, string termID, string depID, string receiveID, out FilterOption option)
        {
            string f1Mode = "L";    //Filter2 不會連動 Filte1，所以 Filte1 使用 "L" 的 DataMode 即可
            string f2Mode = this.UIMode == FilterUIModeEnum.Label ? "L" : "O";

            string receiveKind = String.Empty;  //因為這裡要找的是符合 receiveType, yearID, termID 的資料，所以 receiveKind 不限定

            //Filter2 不會連動 Filte1，所以 Filte1 DefaultMode 一律使用 DefaultItem.Mode.ByKind
            DefaultItem.Mode receiveTypeDefaultMode = DefaultItem.Mode.ByKind;
            DefaultItem.Mode yearDefaultMode = DefaultItem.Mode.ByKind;
            DefaultItem.Mode termDefaultMode = DefaultItem.Mode.ByKind;

            #region [MDY:202203XX] 2022擴充案 英文名稱相關
            bool isEngUI = this.isEngUI();

            XmlResult xmlResult = DataProxy.Current.GetFilterOption(this.Page, (f1Mode + f2Mode)
                , receiveType, yearID, termID, depID, receiveID, receiveKind, isEngUI
                , receiveTypeDefaultMode, yearDefaultMode, termDefaultMode, this.DepDefaultMode, this.ReceiveDefaultMode
                , out option);
            #endregion

            return xmlResult;
        }

        /// <summary>
        /// 取得指定條件的選項資料並結繫
        /// </summary>
        /// <param name="receiveType">業務別碼代碼</param>
        /// <param name="yearID">學年代碼</param>
        /// <param name="termID">學期代碼</param>
        /// <param name="depID">部別代碼</param>
        /// <param name="receiveID">代收費用別代碼</param>
        /// <returns>傳回處理結果</returns>
        public XmlResult GetDataAndBind(string receiveType, string yearID, string termID, string depID, string receiveID)
        {
            FilterOption option = null;
            XmlResult xmlResult = this.GetData(receiveType, yearID, termID, depID, receiveID, out option);
            this.BindData(option);

            this.QueryReceiveType = receiveType;
            this.QueryYearID = yearID;
            this.QueryTermID = termID;

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
                this.QueryReceiveType = String.Empty;
                this.QueryYearID = String.Empty;
                this.QueryTermID = String.Empty;

                #region 部別
                //this.SelectedDepID = null;
                //this.SelectedDepName = null;
                //this.ddlDep.Items.Clear();
                //this.labDep.Text = String.Empty;
                #endregion

                #region 代收費用別
                this.SelectedReceiveID = null;
                this.SelectedReceiveName = null;
                this.ddlReceive.Items.Clear();
                this.labReceive.Text = String.Empty;
                #endregion
            }
            else
            {
                this.QueryReceiveType = option.SelectedReceiveType;
                this.QueryYearID = option.SelectedYearID;
                this.QueryTermID = option.SelectedTermID;

                #region 部別
                {
                    //string selectedValue = WebHelper.SetDropDownListItems(this.ddlDep, this.DepDefaultKind, false, option.DepDatas, true, false, 0, option.SelectedDepID);
                    //if (selectedValue != option.SelectedDepID)
                    //{
                    //    option.SelectedDepID = selectedValue;
                    //}

                    //this.SelectedDepID = option.SelectedDepID;
                    //this.SelectedDepName = option.GetSelectedDepName();

                    //this.labDep.Text = String.Format("{0}({1})", this.SelectedDepName, this.SelectedDepID);
                }
                #endregion

                #region 代收費用別
                {
                    string selectedValue = WebHelper.SetDropDownListItems(this.ddlReceive, this.ReceiveDefaultKind, false, option.ReceiveDatas, true, false, 0, option.SelectedReceiveID);
                    if (selectedValue != option.SelectedReceiveID)
                    {
                        option.SelectedReceiveID = selectedValue;
                    }

                    this.SelectedReceiveID = option.SelectedReceiveID;
                    this.SelectedReceiveName = option.GetSelectedReceiveName();

                    #region [MDY:20200309] CHECKMARX Reflected XSS All Clients Revision
                    this.labReceive.Text = HttpUtility.HtmlEncode(String.Format("{0}({1})", this.SelectedReceiveName, this.SelectedReceiveID));
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
        /// <param name="depID">部別代碼</param>
        /// <param name="depName">部別名稱</param>
        /// <param name="receiveID">代收費用別代碼</param>
        /// <param name="receiveName">代收費用別名稱</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        /// <remarks>此方法主要是用在將此物件當做 Label 顯示且已知個選項的資料時。要注意的是</remarks>
        public void BindData(string depID, string depName, string receiveID, string receiveName)
        {
            #region 部別
            {
                //CodeText[] items = null;
                //if (String.IsNullOrWhiteSpace(depID))
                //{
                //    this.SelectedDepID = null;
                //    this.SelectedDepName = null;
                //}
                //else
                //{
                //    this.SelectedDepID = depID;
                //    this.SelectedDepName = depName;
                //    items = new CodeText[] { new CodeText(this.SelectedDepID, this.SelectedDepName) };
                //}

                //WebHelper.SetDropDownListItems(this.ddlDep, this.DepDefaultKind, false, items, true, false, 0, this.SelectedDepID);
                //this.labDep.Text = String.Format("{0}({1})", this.SelectedDepName, this.SelectedDepID);
            }
            #endregion

            #region 代收費用別
            {
                CodeText[] items = null;
                if (String.IsNullOrWhiteSpace(receiveID))
                {
                    this.SelectedReceiveID = null;
                    this.SelectedReceiveName = null;
                }
                else
                {
                    this.SelectedReceiveID = receiveID;
                    this.SelectedReceiveName = receiveName;
                    items = new CodeText[] { new CodeText(this.SelectedReceiveID, this.SelectedReceiveName) };
                }

                WebHelper.SetDropDownListItems(this.ddlReceive, this.ReceiveDefaultKind, false, items, true, false, 0, this.SelectedReceiveID);
                this.labReceive.Text = HttpUtility.HtmlEncode(String.Format("{0}({1})", this.SelectedReceiveName, this.SelectedReceiveID));
            }
            #endregion

            #region 註記已結繫資料
            //因為 Page 的 Page_Load 比 控制項的 Page_Load 早執行，
            //設定此變數為 true，避免 Page_Load 在次結繫資料
            _HasDataBound = true;
            #endregion
        }

        /// <summary>
        /// 取得費用別選項的代碼文字集合
        /// </summary>
        /// <returns></returns>
        public CodeTextList GetReceiveItems()
        {
            CodeTextList list = new CodeTextList();
            if (this.ddlReceive.Visible && this.ddlReceive.Items.Count > 0)
            {
                foreach (ListItem item in this.ddlReceive.Items)
                {
                    if (!String.IsNullOrEmpty(item.Value))
                    {
                        list.Add(item.Value, item.Text);
                    }
                }
            }
            return list;
        }

        public void ChangeSelectedReceiveId(string receiveId, bool raiseEvent = false)
        {
            WebHelper.SetDropDownListSelectedValue(this.ddlReceive, receiveId);
            if (raiseEvent)
            {
                this.ddlReceive_SelectedIndexChanged(this.ddlReceive, EventArgs.Empty);
            }
            else
            {
                ListItem item = this.ddlReceive.SelectedItem;
                this.SelectedReceiveID = (item == null ? String.Empty : item.Value);
            }
        }
        #endregion

        #region Override BaseUserControl's Method
        protected override void OnPreRender(EventArgs e)
        {
            //this.ddlDep.AutoPostBack = true;
            this.ddlReceive.AutoPostBack = true;
            if (!this.AutoPostBack)
            {
                if (this.ReceiveVisible)
                {
                    this.ddlReceive.AutoPostBack = false;
                }
                //else
                //{
                //    this.ddlDep.AutoPostBack = false;
                //}
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

        //protected void ddlDep_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ListItem item = this.ddlDep.SelectedItem;
        //    this.SelectedDepID = (item == null ? String.Empty : item.Value);
        //    this.SelectedReceiveID = String.Empty;
        //    this.GetDataAndBind(this.QueryReceiveType, this.QueryYearID, this.QueryTermID, this.SelectedDepID, this.SelectedReceiveID);

        //    FilterEventArgs eArgs = new FilterEventArgs(FilterItemConst.Dep, this.SelectedDepID);
        //    this.OnItemSelectedIndexChanged(eArgs);
        //}

        protected void ddlReceive_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item = this.ddlReceive.SelectedItem;
            this.SelectedReceiveID = (item == null ? String.Empty : item.Value);
            //this.GetDataAndBind(this.QueryReceiveType, this.QueryYearID, this.QueryTermID, this.SelectedDepID, this.SelectedReceiveID);
            this.GetDataAndBind(this.QueryReceiveType, this.QueryYearID, this.QueryTermID, string.Empty, this.SelectedReceiveID);
            FilterEventArgs eArgs = new FilterEventArgs(FilterItemConst.Receive, this.SelectedReceiveID);
            this.OnItemSelectedIndexChanged(eArgs);
        }
    }
}