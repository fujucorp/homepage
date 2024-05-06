using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 預設項目相關定義抽象類別
    /// </summary>
    public abstract class DefaultItem
    {
        #region 預設項目模式列舉
        /// <summary>
        /// 預設項目模式列舉
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// 預選第一個有效的項目
            /// </summary>
            First = 1,
            /// <summary>
            /// 只有一個項目時，預選該項目
            /// </summary>
            OnlyOne = 2,
            /// <summary>
            /// 預選 Kind 設定的項目
            /// </summary>
            ByKind = 3
        }
        #endregion

        #region 預設項目種類列舉
        /// <summary>
        /// 預設項目種類列舉
        /// </summary>
        public enum Kind
        {
            /// <summary>
            /// 無項目 / 不產生預設項目
            /// </summary>
            None = 0,
            /// <summary>
            /// 請選擇
            /// </summary>
            Select = 1,
            /// <summary>
            /// 全部
            /// </summary>
            All = 2,
            /// <summary>
            /// 忽略
            /// </summary>
            Omit = 3
        }
        #endregion


        #region 項目代碼、文字定義
        /// <summary>
        /// 無清單項目時，預設清單項目的代碼 (空字串)
        /// </summary>
        public static readonly string NoneItemCode = "";
        /// <summary>
        /// 無清單項目時，預設清單項目的文字 (無選項)
        /// </summary>
        public static readonly string NoneItemText = "--- 無選項 ---";

        /// <summary>
        /// 必選項目時，預設清單項目的代碼 (空字串)
        /// </summary>
        public static readonly string SelectItemCode = "";
        /// <summary>
        /// 必選項目時，預設清單項目的文字 (請選擇)
        /// </summary>
        public static readonly string SelectItemText = "--- 請選擇 ---";

        /// <summary>
        /// 非必選項目時，預設清單項目的代碼 (空字串)
        /// </summary>
        public static readonly string AllItemCode = "";
        /// <summary>
        /// 非必選項目時，預設清單項目的文字 (全部)
        /// </summary>
        public static readonly string AllItemText = "--- 全部 ---";

        /// <summary>
        /// 非必選項目時，預設清單項目的代碼 (空字串)
        /// </summary>
        public static readonly string OmitItemCode = "";
        /// <summary>
        /// 非必選項目時，預設清單項目的文字 (忽略)
        /// </summary>
        public static readonly string OmitItemText = "--- 忽略 ---";
        #endregion

        #region Static Method
        /// <summary>
        /// 取得指定預設項目種類對應的項目代碼
        /// </summary>
        /// <param name="kind">指定預設項目種類。</param>
        /// <returns>傳回項目代碼或空字串。</returns>
        public static string GetCode(Kind kind)
        {
            switch (kind)
            {
                case Kind.None:
                    return NoneItemCode;
                case Kind.Select:
                    return SelectItemCode;
                case Kind.All:
                    return AllItemCode;
                case Kind.Omit:
                    return OmitItemCode;
                default:
                    return String.Empty;
            }
        }

        /// <summary>
        /// 取得指定預設項目種類對應的項目文字
        /// </summary>
        /// <param name="kind">指定預設項目種類。</param>
        /// <returns>傳回項目文字或列舉值字串。</returns>
        public static string GetText(Kind kind)
        {
            string defaultText = null;
            switch (kind)
            {
                case DefaultItem.Kind.None:
                    defaultText = DefaultItem.NoneItemText;
                    break;
                case DefaultItem.Kind.Select:
                    defaultText = DefaultItem.SelectItemText;
                    break;
                case DefaultItem.Kind.All:
                    defaultText = DefaultItem.AllItemText;
                    break;
                case DefaultItem.Kind.Omit:
                    defaultText = DefaultItem.OmitItemText;
                    break;
                default:
                    defaultText = kind.ToString();
                    break;
            }
            string resourceKey = String.Concat("ITEM_TXT_", kind.ToString().ToUpper());
            return WebHelper.GetLocalized(resourceKey, defaultText);
        }

        /// <summary>
        /// 取得指定預設項目種類對應的項目物件 (ListItem)
        /// </summary>
        /// <param name="kind">指定預設項目種類。</param>
        /// <returns>傳回項目物件。</returns>
        public static ListItem GetItem(Kind kind)
        {
            string text = DefaultItem.GetText(kind);
            string value = DefaultItem.GetCode(kind);
            ListItem item = new ListItem(text, value);
            return item;
        }
        #endregion
    }

    /// <summary>
    /// 網頁公用方法抽象類別
    /// </summary>
    public abstract class WebHelper : WebCommon
    {
        #region CodeText 轉 ListItem 相關方法
        /// <summary>
        /// 產生指定項目資料的代碼文字對照類別泛型集合的 ListItem 陣列
        /// </summary>
        /// <param name="kind">指定預設項目的種類。</param>
        /// <param name="showNone">指定如果沒有項目資料時是否產生「無項目」的項目。</param>
        /// <param name="datas">指定項目資料的代碼文字對照類別泛型集合。</param>
        /// <param name="showValue">指定每個項目的文字是否需要顯示項目的值。</param>
        /// <param name="needLocalized">指定每個項目的文字是否需要做 Localized 處理。</param>
        /// <param name="spaceString">指定每個項目的文字後面附加的空白字串。</param>
        /// <returns>傳回 ListItem 陣列或 null。</returns>
        public static ListItem[] GenListItems(DefaultItem.Kind kind, bool showNone, ICollection<CodeText> datas, bool showValue, bool needLocalized, string spaceString)
        {
            if (datas == null || datas.Count == 0)
            {
                if (showNone)
                {
                    ListItem item = DefaultItem.GetItem(DefaultItem.Kind.None);
                    return new ListItem[] { item };
                }
                return null;
            }

            int idx = 0;
            ListItem[] items = null;
            if (kind == DefaultItem.Kind.None)
            {
                items = new ListItem[datas.Count];
            }
            else
            {
                items = new ListItem[datas.Count + 1];
                items[0] = DefaultItem.GetItem(kind);
                idx++;
            }

            foreach (CodeText data in datas)
            {
                string text = DataFormat.GetItemText(data, showValue, needLocalized, spaceString);
                items[idx++] = new ListItem(text, data.Code);
            }
            return items;
        }

        /// <summary>
        /// 產生 RadioButton 控制項用的 ListItem 陣列
        /// </summary>
        /// <param name="datas">指定代碼文字對照類別泛型集合。</param>
        /// <param name="needLocalized">指定每個項目的文字是否需要做 Localized 處理。</param>
        /// <param name="spaceCount">指定每個項目的文字後面附加的空白 Html 符號數量，有效值 1 ~ 10，超過有效值則不處理。</param>
        /// <returns>傳回 ListItem 陣列或 null。</returns>
        private static ListItem[] GenListItemsForRadioButton(ICollection<CodeText> datas, bool needLocalized, int spaceCount)
        {
            ListItem[] items = null;
            if (datas != null && datas.Count > 0)
            {
                string specString = spaceCount >= 1 && spaceCount <= 10 ? Common.Repeat("&nbsp;", spaceCount) : null;
                items = new ListItem[datas.Count];
                int idx = 0;
                foreach (CodeText data in datas)
                {
                    string text = DataFormat.GetItemText(data, false, needLocalized, specString);
                    items[idx++] = new ListItem(text, data.Code);
                }
            }
            return items;
        }

        /// <summary>
        /// 產生 DropDownList 控制項用的 ListItem 陣列
        /// </summary>
        /// <param name="kind">指定預設項目的種類。</param>
        /// <param name="showNone">指定如果沒有項目資料時是否產生「無項目」的項目。</param>
        /// <param name="datas">指定項目資料的代碼文字對照類別泛型集合。</param>
        /// <param name="showValue">指定每個項目的文字是否需要顯示項目的值。</param>
        /// <param name="needLocalized">指定每個項目的文字是否需要做 Localized 處理。</param>
        /// <param name="spaceCount">指定每個項目的文字後面附加的空白字元數量，有效值 1 ~ 10，超過有效值則不處理。</param>
        /// <returns>傳回 ListItem 陣列或 null。</returns>
        private static ListItem[] GenListItemsForDropDownList(DefaultItem.Kind kind, bool showNone, ICollection<CodeText> datas, bool showValue, bool needLocalized, int spaceCount)
        {
            ListItem[] items = null;
            if (datas != null && datas.Count > 0)
            {
                string specString = spaceCount >= 1 && spaceCount <= 10 ? "".PadRight(spaceCount, ' ') : null;
                items = GenListItems(kind, showNone, datas, showValue, needLocalized, specString);
            }
            return items;
        }
        #endregion

        #region DropDownList 控制項處理相關方法
        /// <summary>
        /// 設定指定 DropDownList 控制項的項目與已選取項目
        /// </summary>
        /// <param name="control">指定 DropDownList 控制項。</param>
        /// <param name="kind">指定預設項目的種類。</param>
        /// <param name="showNone">指定如果沒有項目資料時是否產生「無項目」的項目。</param>
        /// <param name="datas">指定項目資料的代碼文字對照類別泛型集合。</param>
        /// <param name="showValue">指定每個項目的文字是否需要顯示項目的值。</param>
        /// <param name="needLocalized">指定每個項目的文字是否需要做 Localized 處理。</param>
        /// <param name="spaceCount">指定每個項目的文字後面附加的空白字元數量，有效值 1 ~ 10，超過有效值則不處理。</param>
        /// <param name="selectedValue">指定已選取項目的值。</param>
        /// <param name="defaultSelectedOne">指定如果沒有任何項目被選取，是否預設選取第一項。</param>
        /// <returns>傳回選取項目的值，如無項目被選取得傳回 null。</returns>
        public static string SetDropDownListItems(DropDownList control, DefaultItem.Kind kind, bool showNone, ICollection<CodeText> datas, bool showValue, bool needLocalized, int spaceCount, string selectedValue, bool defaultSelectedOne)
        {
            if (control == null)
            {
                return null;
            }
            control.Items.Clear();
            control.SelectedIndex = -1;

            ListItem[] items = GenListItemsForDropDownList(kind, showNone, datas, showValue, needLocalized, spaceCount);
            if (items != null && items.Length > 0)
            {
                control.Items.AddRange(items);

                if (selectedValue != null)
                {
                    ListItem item = control.Items.FindByValue(selectedValue);
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }

                if (defaultSelectedOne && control.SelectedIndex == -1)
                {
                    control.SelectedIndex = 0;
                }
            }

            return control.SelectedValue;
        }

        /// <summary>
        /// 設定指定 DropDownList 控制項的項目與已選取項目，並預設選取第一項
        /// </summary>
        /// <param name="control">指定 DropDownList 控制項。</param>
        /// <param name="kind">指定預設項目的種類。</param>
        /// <param name="showNone">指定如果沒有項目資料時是否產生「無項目」的項目。</param>
        /// <param name="datas">指定項目資料的代碼文字對照類別泛型集合。</param>
        /// <param name="showValue">指定每個項目的文字是否需要顯示項目的值。</param>
        /// <param name="needLocalized">指定每個項目的文字是否需要做 Localized 處理。</param>
        /// <param name="spaceCount">指定每個項目的文字後面附加的空白字元數量，有效值 1 ~ 10，超過有效值則不處理。</param>
        /// <param name="selectedValue">指定已選取項目的值。</param>
        /// <returns>傳回選取項目的值，如無項目被選取得傳回 null。</returns>
        public static string SetDropDownListItems(DropDownList control, DefaultItem.Kind kind, bool showNone, ICollection<CodeText> datas, bool showValue, bool needLocalized, int spaceCount, string selectedValue)
        {
            return SetDropDownListItems(control, kind, showNone, datas, showValue, needLocalized, spaceCount, selectedValue, true);
        }

        #region [MDY:20181116] 因為 checkmarx 會誤判所以 Overload 不回傳值的方法
        /// <summary>
        /// 設定指定 DropDownList 控制項的項目與已選取項目
        /// </summary>
        /// <param name="control">指定 DropDownList 控制項。</param>
        /// <param name="datas">指定項目資料的代碼文字對照類別泛型集合。</param>
        /// <param name="kind">指定預設項目的種類。</param>
        /// <param name="showNone">指定如果沒有項目資料時是否產生「無項目」的項目。</param>
        /// <param name="showValue">指定每個項目的文字是否需要顯示項目的值。</param>
        /// <param name="needLocalized">指定每個項目的文字是否需要做 Localized 處理。</param>
        /// <param name="spaceCount">指定每個項目的文字後面附加的空白字元數量，有效值 1 ~ 10，超過有效值則不處理。</param>
        /// <param name="selectedValue">指定已選取項目的值。</param>
        /// <param name="defaultSelectedOne">指定如果沒有任何項目被選取，是否預設選取第一項。</param>
        /// <returns>傳回選取項目的值，如無項目被選取得傳回 null。</returns>
        public static void SetDropDownListItems(DropDownList control, ICollection<CodeText> datas, DefaultItem.Kind kind, bool showNone = false, bool showValue = true, bool needLocalized = false, int spaceCount = 0, string selectedValue = null, bool defaultSelectedOne = true)
        {
            if (control != null)
            {
                control.Items.Clear();
                control.SelectedIndex = -1;

                ListItem[] items = GenListItemsForDropDownList(kind, showNone, datas, showValue, needLocalized, spaceCount);
                if (items != null && items.Length > 0)
                {
                    control.Items.AddRange(items);

                    if (selectedValue != null)
                    {
                        ListItem item = control.Items.FindByValue(selectedValue);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }

                    if (defaultSelectedOne && control.SelectedIndex == -1)
                    {
                        control.SelectedIndex = 0;
                    }
                }
            }
        }
        #endregion
        #endregion

        #region RadioButtonList 控制項處理相關方法
        /// <summary>
        /// 設定指定 RadioButtonList 控制項的選項與選取項目
        /// </summary>
        /// <param name="control">指定 RadioButtonList 控制項。</param>
        /// <param name="datas">指定代碼文字對照類別集合。</param>
        /// <param name="needLocalized">指定代碼文字對照類別的文字是否需要做多語系處理。</param>
        /// <param name="spaceCount">指定每個選項後面附加的空白 Html 符號數量，最小值為 0 小於 0 以 0 處理。</param>
        /// <param name="selectedValue">指定選取項目的值。</param>
        public static void SetRadioButtonListItems(RadioButtonList control, ICollection<CodeText> datas, bool needLocalized, int spaceCount, string selectedValue)
        {
            if (control == null)
            {
                return;
            }
            control.Items.Clear();
            control.SelectedIndex = -1;

            ListItem[] items = GenListItemsForRadioButton(datas, needLocalized, spaceCount);
            if (items != null && items.Length > 0)
            {
                control.Items.AddRange(items);

                if (selectedValue != null)
                {
                    ListItem item = control.Items.FindByValue(selectedValue);
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
        }
        #endregion

        #region CheckBoxList 控制項處理相關方法
        /// <summary>
        /// 設定指定 CheckBoxList 控制項的選項與選取項目
        /// </summary>
        /// <param name="control">指定 CheckBoxList 控制項。</param>
        /// <param name="datas">指定代碼文字對照類別集合。</param>
        /// <param name="needLocalized">指定代碼文字對照類別的文字是否需要做多語系處理。</param>
        /// <param name="spaceCount">指定每個選項後面附加的空白 Html 符號數量，最小值為 0 小於 0 以 0 處理。</param>
        /// <param name="selectedValues">指定選取項目的值的陣列。</param>
        public static void SetCheckBoxListItems(CheckBoxList control, ICollection<CodeText> datas, bool needLocalized, int spaceCount, ICollection<string> selectedValues)
        {
            if (control == null)
            {
                return;
            }

            control.Items.Clear();
            control.SelectedIndex = -1;

            ListItem[] items = GenListItemsForRadioButton(datas, needLocalized, spaceCount);
            if (items != null && items.Length > 0)
            {
                control.Items.AddRange(items);

                if (selectedValues != null && selectedValues.Count > 0)
                {
                    foreach (string selectedValue in selectedValues)
                    {
                        ListItem item = control.Items.FindByValue(selectedValue);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 設定 CheckBoxList 控制項的選取項目值
        /// </summary>
        /// <param name="control"></param>
        /// <param name="selectedValues"></param>
        public static void SetCheckBoxListSelectedValues(CheckBoxList control, ICollection<string> selectedValues)
        {
            if (control == null)
            {
                return;
            }

            foreach (ListItem item in control.Items)
            {
                item.Selected = false;
                if (selectedValues != null && selectedValues.Count > 0)
                {
                    foreach (string selectedValue in selectedValues)
                    {
                        if (item.Value == selectedValue)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region [MDY:2018xxxx] 以數值遞減排序
        /// <summary>
        /// 將指定選項 (CodeText 陣列) 以數值遞減排序，Code 非數值時以前補 0 方式補齊 4 碼後遞減排序
        /// </summary>
        /// <param name="items"></param>
        public static void SortItemsByValueDesc(ref CodeText[] items)
        {
            if (items != null && items.Length > 0)
            {
                Array.Sort<CodeText>(items, delegate(CodeText a, CodeText b)
                {
                    int aCode = 0, bCode = 0;
                    if (Int32.TryParse(a.Code, out aCode) && Int32.TryParse(b.Code, out bCode))
                    {
                        return bCode.CompareTo(aCode);
                    }
                    else
                    {
                        return b.Code.Trim().PadLeft(4, '0').CompareTo(a.Code.Trim().PadLeft(4, '0'));
                    }
                });
            }
        }
        #endregion

        #region 取得登入者資料
        private const string SESSION_KEY_FOR_LOGON_USER = "LOGON_USER";
        /// <summary>
        /// 取得 (Session) 登入者資料，未登入 (Session 無資料) 則傳回 Anonymous 資料
        /// </summary>
        /// <returns></returns>
        public static LogonUser GetLogonUser()
        {
            HttpSessionState session = HttpContext.Current.Session;
            LogonUser user = null;
            if (session != null)
            {
                user = session[SESSION_KEY_FOR_LOGON_USER] as LogonUser;
            }
            if (user == null)
            {
                user = LogonUser.GenAnonymous();
                //user = LogonUser.GenTestManager();
                user.AuthMenus = MenuHelper.GetAnonymousMenuAuths();
                if (session != null)
                {
                    session[SESSION_KEY_FOR_LOGON_USER] = user;
                }
            }
            return user;
        }

        /// <summary>
        /// 設定 (Session) 登入者資料
        /// </summary>
        /// <param name="logonUser">登入者資料</param>
        /// <param name="fgClear">是否先清除所有 Session 並取消目前工作階段</param>
        public static void SetLogonUser(LogonUser logonUser, bool fgClear = false)
        {
            HttpSessionState session = HttpContext.Current.Session;
            if (session != null)
            {
                if (fgClear)
                {
                    session.RemoveAll();
                    session.Abandon();
                }
                session[SESSION_KEY_FOR_LOGON_USER] = logonUser;
            }
        }
        #endregion

        #region 取得頁面訊息
        /// <summary>
        /// 取得目前的頁面
        /// </summary>
        public static Page GetCurrentPage()
        {
            Page CurrentPge = null;
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                CurrentPge = context.Handler as Page;
            }
            return CurrentPge;
        }

        private static Regex _MenuPageFileReg = new Regex(@"^[A-Z][0-9]{7}[MD]{0,1}[0-9]{0,1}[A-Z]{0,1}\.ASPX$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex _EditPageFileReg = new Regex(@"^[A-Z][0-9]{7}M[0-9]{0,1}[A-Z]{0,1}\.ASPX$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex _SubPageFileReg = new Regex(@"^[A-Z][0-9]{7}[MD][0-9]{0,1}[A-Z]{0,1}\.ASPX$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// 取得指定頁面可能對應的選單(功能)代碼 (此方法是由指定頁面實體檔名來判斷，即使有傳回值也不一定是真的選單(功能)代碼)
        /// </summary>
        /// <param name="page">指定頁面。</param>
        /// <param name="isEditPage">傳回是否為延伸編輯頁面。</param>
        /// <param name="isSubPage">傳回是否為延伸頁面。</param>
        /// <returns>指定頁面實體檔名符合選單(功能)命名規則時傳回可能的選單(功能)代碼，否則傳回 null。</returns>
        public static string GetPageMenuID(Page page, out bool isEditPage, out bool isSubPage)
        {
            isEditPage = false;
            isSubPage = false;
            if (page != null)
            {
                return GetFileMenuID(page.Request.CurrentExecutionFilePath, out isEditPage, out isSubPage);
            }
            return null;
        }

        /// <summary>
        /// 取得目前網頁可能對應的選單(功能)代碼 (此方法是由目前網頁實體檔名來判斷，即使有傳回值也不一定是真的選單(功能)代碼)
        /// </summary>
        /// <param name="isEditPage">傳回是否為延伸編輯頁面。</param>
        /// <param name="isSubPage">傳回是否為延伸頁面。</param>
        /// <returns>目前網頁實體檔名符合選單(功能)命名規則時傳回可能的選單(功能)代碼，否則傳回 null。</returns>
        public static string GetCurrentPageMenuID(out bool isEditPage, out bool isSubPage)
        {
            isEditPage = false;
            isSubPage = false;
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                HttpRequest request = context.Request;
                return GetFileMenuID(request.CurrentExecutionFilePath, out isEditPage, out isSubPage);
            }
            return null;
        }

        /// <summary>
        /// 取得指定檔案名稱可能對應的選單(功能)代碼 (即使有傳回值也不一定是真的選單(功能)代碼)
        /// </summary>
        /// <param name="fileName">指定檔案名稱</param>
        /// <param name="isEditPage">傳回是否為延伸編輯頁面。</param>
        /// <param name="isSubPage">傳回是否為延伸頁面。</param>
        /// <returns>檔名符合選單(功能)命名規則時傳回可能的選單(功能)代碼，否則傳回 null。</returns>
        public static string GetFileMenuID(string fileName, out bool isEditPage, out bool isSubPage)
        {
            isEditPage = false;
            isSubPage = false;
            if (String.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }
            fileName = Path.GetFileName(fileName.Trim());
            if (_MenuPageFileReg.IsMatch(fileName))
            {
                string menuID = fileName.Substring(0, 8);
                if (menuID.EndsWith("00000"))
                {
                    return menuID.Substring(0, 3);
                }
                else
                {
                    isEditPage = _EditPageFileReg.IsMatch(fileName);
                    isSubPage = _SubPageFileReg.IsMatch(fileName);
                    return menuID;
                }
            }
            else
            {
                #region [Old] 改用 IMenuPage 機制
                //if (fileName.Equals("index.aspx", StringComparison.CurrentCultureIgnoreCase))
                //{
                //    isEditPage = false;
                //    isSubPage = false;
                //    return "L1100001";
                //}
                #endregion

                if (fileName.Equals("student001.aspx", StringComparison.CurrentCultureIgnoreCase))
                {
                    isEditPage = false;
                    isSubPage = false;
                    return "X1100001";
                }
                else if (fileName.Equals("student002.aspx", StringComparison.CurrentCultureIgnoreCase))
                {
                    isEditPage = false;
                    isSubPage = false;
                    return "X1100001";
                }
            }

            return null;
        }

        /// <summary>
        /// 取得指定網址在指定頁面時的用戶端可用網址
        /// </summary>
        /// <param name="page">指定頁面</param>
        /// <param name="url">指定網址</param>
        /// <returns>傳回用戶端可用網址</returns>
        public static string GetResolveUrl(Page page, string url)
        {
            #region [MDY:20210401] 原碼修正
            if (String.IsNullOrEmpty(url)
                || url.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase))
            {
                return String.Empty;
            }
            if (url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) 
                || url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                return Uri.EscapeUriString(url);
            }
            if (page == null)
            {
                return url;
            }

            #region [MDY:20200705] 修正特殊字串路徑 "/(Z(%22onerror=%22alert'XSS'%22))/" 的 XSS 問題
            #region [OLD]
            //if (url.StartsWith("~/"))
            //{
            //    return page.ResolveUrl(url);
            //}
            //if (url.StartsWith("/"))
            //{
            //    return page.ResolveUrl(String.Concat("~", url));
            //}
            //return page.ResolveUrl(String.Concat("~/", url));
            #endregion

            if (url.StartsWith("/"))
            {
                return Uri.EscapeUriString(String.Concat(page.Request.ApplicationPath, url.Substring(1)));
            }
            else if (url.StartsWith("~/"))
            {
                return Uri.EscapeUriString(String.Concat(page.Request.ApplicationPath, url.Substring(2)));
            }
            else if (url.StartsWith("./"))
            {
                return Uri.EscapeUriString(String.Concat(String.Join("", page.Request.Url.Segments, 0, page.Request.Url.Segments.Length - 1), url.Substring(2)));
            }
            else if (url.StartsWith("../"))
            {
                return Uri.EscapeUriString(String.Concat(String.Join("", page.Request.Url.Segments, 0, page.Request.Url.Segments.Length - 2), url.Substring(2)));
            }
            else
            {
                return Uri.EscapeUriString(String.Concat(page.Request.ApplicationPath, url));
            }
            #endregion
            #endregion
        }



        /// <summary>
        /// 取得指定選單(功能)代碼是否符合命名規則
        /// </summary>
        /// <param name="menuID">指定選單(功能)代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsMatchMenuID(string menuID)
        {
            return _MenuPageFileReg.IsMatch(menuID);
        }
        #endregion

        #region Localized
        #region [MDY:2018xxxx] 避免原碼掃描誤判
        /// <summary>
        /// 取得指定文字的 Localized 並做 HtmlEncode (多語系翻譯)
        /// </summary>
        /// <param name="text">指定文字</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回空字串</returns>
        public static string GetHtmlEncodeLocalized(string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                object resource = HttpContext.GetGlobalResourceObject("Localized", text.Trim());
                if (resource != null)
                {
                    return HttpUtility.HtmlEncode(resource.ToString());
                }
            }
            return String.Empty;
        }
        #endregion

        /// <summary>
        /// 取得指定文字的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="text">指定文字。</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回指定文字或空字串。</returns>
        public static string GetLocalized(string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                object resource = HttpContext.GetGlobalResourceObject("Localized", text.Trim());
                if (resource != null)
                {
                    return resource.ToString();
                }
            }
            return text ?? String.Empty;
        }

        /// <summary>
        /// 取得指定資源索引鍵的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="key">指定資源索引鍵。</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字。</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回預設文字或空字串。</returns>
        public static string GetLocalized(string resourceKey, string defaultText)
        {
            if (!String.IsNullOrWhiteSpace(resourceKey))
            {
                string resource = HttpContext.GetGlobalResourceObject("Localized", resourceKey.Trim()) as string;
                if (resource != null && resource != resourceKey)
                {
                    return resource.ToString();
                }
            }
            return defaultText ?? String.Empty;
        }

        /// <summary>
        /// 取得 Button 類控制項用文字的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="text">指定 Button 類控制項的原始文字。</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回原始文字或空字串。</returns>
        public static string GetButtonControlLocalized(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                string resourceKey = "CTL_BTN_" + text;
                return GetLocalized(resourceKey, text);
            }
            return text ?? String.Empty;
        }

        /// <summary>
        /// 取得指定資源索引鍵的控制項用文字的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="resourceKey">指定資源索引鍵。</param>
        /// <param name="defaultText">找不到資源時的回傳預設文字。</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回預設文字或空字串。</returns>
        public static string GetControlLocalizedByResourceKey(string resourceKey, string defaultText)
        {
            if (!String.IsNullOrWhiteSpace(resourceKey) && (String.IsNullOrEmpty(defaultText) || CultureInfo.CurrentCulture.Name != "zh-TW"))
            {
                resourceKey = resourceKey.Trim();
                if (resourceKey.StartsWith("CTL_", StringComparison.CurrentCultureIgnoreCase))
                {
                    return GetLocalized(resourceKey, defaultText);
                }
                else
                {
                    return GetLocalized(String.Concat("CTL_" + resourceKey), defaultText);
                }
            }
            return defaultText ?? String.Empty;
        }

        /// <summary>
        /// 取得指定選單代碼的選單名稱的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="menuId">指定選單代碼。</param>
        /// <param name="menuName">指定預設的選單名稱。</param>
        /// <returns>找到則傳回 Localized 文字，否則傳回預設選單名稱或空字串。</returns>
        public static string GetMenuLocalized(string menuId, string menuName)
        {
            if (!String.IsNullOrWhiteSpace(menuId) && (String.IsNullOrEmpty(menuName) || CultureInfo.CurrentCulture.Name != "zh-TW"))
            {
                string resourceKey = menuId.Trim();
                if (resourceKey.StartsWith("MENU_", StringComparison.CurrentCultureIgnoreCase))
                {
                    return GetLocalized(resourceKey, menuName);
                }
                else
                {
                    return GetLocalized(String.Concat("MENU_" + resourceKey), menuName);
                }
            }
            return menuName ?? String.Empty;
        }

        /// <summary>
        /// 取得指定錯誤代碼的文字的 Localized (多語系翻譯)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static string GetErrorCodeLocalized(string code, string defaultText)
        {
            if (!String.IsNullOrWhiteSpace(code) && (String.IsNullOrEmpty(defaultText) || CultureInfo.CurrentCulture.Name != "zh-TW"))
            {
                code = code.Trim();
                if (code.StartsWith("ERR_", StringComparison.CurrentCultureIgnoreCase))
                {
                    return GetLocalized(code, defaultText);
                }
                else
                {
                    return GetLocalized(String.Concat("ERR_" + code), defaultText);
                }
            }
            return defaultText ?? String.Empty;
        }
        #endregion


        #region Session For ErrorPageInfo
        public const string SESSION_KEY_ERROR_PAGE_INFO = "ERROR_PAGE_INFO";

        /// <summary>
        /// 設定錯誤頁訊息
        /// </summary>
        /// <param name="info"></param>
        public static void SetErrorPageInfo(ErrorPageInfo info)
        {
            HttpSessionState session = HttpContext.Current.Session;
            if (session != null)
            {
                session[SESSION_KEY_ERROR_PAGE_INFO] = info;
            }
        }

        /// <summary>
        /// 取得錯誤頁訊息
        /// </summary>
        /// <returns></returns>
        public static ErrorPageInfo GetErrorPageInfo()
        {
            HttpSessionState session = HttpContext.Current.Session;
            if (session != null)
            {
                return session[SESSION_KEY_ERROR_PAGE_INFO] as ErrorPageInfo;
            }
            return null;
        }
        #endregion

        #region Session For FilterArguments
        /// <summary>
        /// Filter1、Filter2 使用者控制項傳遞參數用的 Session Key
        /// </summary>
        public const string SESSION_KEY_FILTER_ARGUMENTS = "SUBMENU_ARGUMENTS";

        /// <summary>
        /// Filter1、Filter2 使用者控制項參數的分隔符號
        /// </summary>
        private const string FILTER_ARGUMENTS_SEPARATOR = "||";

        /// <summary>
        /// 設定要傳給下一頁的 Filter1、Filter2 使用者控制項的參數
        /// </summary>
        /// <param name="receiveType">指定業務別碼代碼參數</param>
        /// <param name="yearID">指定學年代碼參數</param>
        /// <param name="termID">指定學期代碼參數</param>
        /// <param name="depID">指定部別代碼參數</param>
        /// <param name="receiveID">指定代收費用別代碼參數</param>
        public static void SetFilterArguments(string receiveType, string yearID, string termID, string depID, string receiveID)
        {
            HttpSessionState session = HttpContext.Current.Session;
            if (session != null)
            {
                session[SESSION_KEY_FILTER_ARGUMENTS] = String.Format("{0}{5}{1}{5}{2}{5}{3}{5}{4}", receiveType, yearID, termID, depID, receiveID, FILTER_ARGUMENTS_SEPARATOR);
            }
        }

        /// <summary>
        /// 取得上一頁指定的 Filter1、Filter2 使用者控制項的參數
        /// </summary>
        /// <param name="receiveType">取得業務別碼代碼參數</param>
        /// <param name="yearID">取得學年代碼參數</param>
        /// <param name="termID">取得學期代碼參數</param>
        /// <param name="depID">取得部別代碼參數</param>
        /// <param name="receiveID">取得代收費用別代碼參數</param>
        /// <returns>如果參數存在且格式正確則傳回 true，否則傳回 false。</returns>
        public static bool GetFilterArguments(out string receiveType, out string yearID, out string termID, out string depID, out string receiveID)
        {
            receiveType = null;
            yearID = null;
            termID = null;
            depID = null;
            receiveID = null;
            HttpSessionState session = HttpContext.Current.Session;
            if (session != null)
            {
                bool isOK = false;
                object value = session[SESSION_KEY_FILTER_ARGUMENTS];
                if (value is string)
                {
                    string[] args = (value as string).Split(new string[] { FILTER_ARGUMENTS_SEPARATOR }, StringSplitOptions.None);
                    if (args.Length == 5)
                    {
                        receiveType = args[0];
                        yearID = args[1];
                        termID = args[2];
                        depID = args[3];
                        receiveID = args[4];
                        isOK = true;
                    }
                }
                return isOK;
            }
            return false;
        }

        /// <summary>
        /// 清除 Filter1、Filter2 使用者控制項的參數
        /// </summary>
        public static void ClearFilterArguments()
        {
            HttpSessionState session = HttpContext.Current.Session;
            if (session != null)
            {
                session.Remove(SESSION_KEY_FILTER_ARGUMENTS);
            }
        }
        #endregion


        #region [MDY:20210706]  FIX BUG 原碼修正 產生/移除 Url 隨機參數
        /// <summary>
        /// 產生串上 RN 隨機參數後的 URL
        /// </summary>
        /// <param name="url">指定 Url</param>
        /// <returns></returns>
        public static string GenRNUrl(string url)
        {
            using (System.Security.Cryptography.RNGCryptoServiceProvider RNG = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[24];
                RNG.GetBytes(buffer);
                if (!String.IsNullOrWhiteSpace(url) && url.IndexOf('?') > 0)
                {
                    return String.Format("{0}&RN={1}", url, Convert.ToBase64String(buffer));
                }
                else
                {
                    return String.Format("{0}?RN={1}", url, Convert.ToBase64String(buffer));
                }
            }
        }

        /// <summary>
        /// 移除指定 Url 中的 RN 隨機參數
        /// </summary>
        /// <param name="url">指定 Url</param>
        /// <returns>傳回移除後的 Url</returns>
        public static string RemoveRNQueryString(string url)
        {
            if (String.IsNullOrWhiteSpace(url))
            {
                return url;
            }

            int size = url.Length;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(size);
            int startIndex = 0;
            int lastIndex = size - 1;
            bool needQuestionMark = false;

            #region 移除 ?RN=
            {
                int endIndex = url.IndexOf("?RN=", startIndex);
                if (endIndex >= startIndex)
                {
                    needQuestionMark = true;
                    if (endIndex > startIndex)
                    {
                        sb.Append(url, startIndex, endIndex - startIndex);
                    }
                    int nextIndex = url.IndexOf("&", endIndex + 1);
                    if (nextIndex > endIndex)
                    {
                        startIndex = nextIndex;
                    }
                    else
                    {
                        startIndex = size;
                    }
                }
            }
            #endregion

            #region 移除 &RN=
            while (startIndex < lastIndex)
            {
                int endIndex = url.IndexOf("&RN=", startIndex);
                if (endIndex >= startIndex)
                {
                    if (endIndex > startIndex)
                    {
                        if (needQuestionMark)
                        {
                            needQuestionMark = false;
                            sb.Append("?").Append(url, startIndex + 1, endIndex - startIndex - 1);
                        }
                        else
                        {
                            sb.Append(url, startIndex, endIndex - startIndex);
                        }
                    }
                    int nextIndex = url.IndexOf("&", endIndex + 1);
                    if (nextIndex > endIndex)
                    {
                        startIndex = nextIndex;
                    }
                    else
                    {
                        startIndex = size;
                        break;
                    }
                }
                else
                {
                    if (startIndex == 0)
                    {
                        return url;
                    }
                    else if (needQuestionMark)
                    {
                        needQuestionMark = false;
                        sb.Append("?").Append(url, startIndex + 1, size - startIndex - 1);
                    }
                    else
                    {
                        sb.Append(url, startIndex, size - startIndex);
                    }
                    startIndex = size;
                    break;
                }
            }
            return sb.ToString();
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// 錯誤頁訊息承載類別
    /// </summary>
    [Serializable]
    public class ErrorPageInfo
    {
        #region Property
        public string MenuID
        {
            get;
            set;
        }

        public string MenuName
        {
            get;
            set;
        }

        public string ErrorCode
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public Exception Exception
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構錯誤頁訊息承載類別
        /// </summary>
        /// <param name="menuID">錯誤頁面的選單(功能)代碼</param>
        /// <param name="menuName">錯誤頁面的選單(功能)名稱</param>
        /// <param name="errorCode">發生的錯誤代碼</param>
        /// <param name="errorMessage">發生的錯誤訊息</param>
        public ErrorPageInfo(string menuID, string menuName, string errorCode, string errorMessage)
        {
            this.MenuID = menuID == null ? null : menuID.Trim();
            this.MenuName = menuName == null ? null : menuName.Trim();
            this.ErrorCode = errorCode == null ? null : errorCode.Trim();
            this.ErrorMessage = errorMessage == null ? null : errorMessage.Trim();
        }

        /// <summary>
        /// 建構錯誤頁訊息承載類別
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="ex"></param>
        public ErrorPageInfo(MenuInfo menu, Exception ex)
        {
            if (menu != null)
            {
                this.MenuID = menu.ID;
                this.MenuName = menu.Name;
            }
            this.Exception = ex;
        }

        /// <summary>
        /// 建構錯誤頁訊息承載類別
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="result"></param>
        public ErrorPageInfo(MenuInfo menu, XmlResult result)
        {
            if (menu != null)
            {
                this.MenuID = menu.ID;
                this.MenuName = menu.Name;
            }
            if (result != null)
            {
                this.ErrorCode = result.Code;
                this.ErrorMessage = result.Message;
            }
        }
        #endregion

        #region Method
        public bool IsNoMessage()
        {
            if (String.IsNullOrWhiteSpace(this.ErrorMessage)
                && String.IsNullOrWhiteSpace(this.ErrorCode)
                && this.Exception == null)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}