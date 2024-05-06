using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Web;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 選單資訊類別
    /// </summary>
    public class MenuInfo : IComparable<MenuInfo>
    {
        #region Member
        /// <summary>
        /// 子選單資料清單
        /// </summary>
        private List<MenuInfo> _Childs = new List<MenuInfo>();
        #endregion

        #region Property
        private string _ID = null;
        /// <summary>
        /// 選單代碼
        /// </summary>
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Name = null;
        /// <summary>
        /// 選單名稱
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value == null ? String.Empty : value.Trim();
            }
        }

        private string _ParentID = null;
        /// <summary>
        /// 父層選單代碼
        /// </summary>
        public string ParentID
        {
            get
            {
                return _ParentID;
            }
            set
            {
                _ParentID = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Url = null;
        /// <summary>
        /// 選單連結網址
        /// </summary>
        public string Url
        {
            get
            {
                return _Url;
            }
            set
            {
                _Url = value == null ? String.Empty : value.Trim();
            }
        }

        private int _SortNo = 500;
        /// <summary>
        /// 選單排序編號，最小值為 1，預設 500
        /// </summary>
        public int SortNo
        {
            get
            {
                return _SortNo;
            }
            set
            {
                _SortNo = value < 1 ? 1 : value;
            }
        }

        /// <summary>
        /// 取得是否有子選單資料
        /// </summary>
        public bool HasChild
        {
            get
            {
                return (_Childs.Count > 0);
            }
        }

        private string _CSSClass = null;
        /// <summary>
        /// 使用 CSS 的 Class
        /// </summary>
        public string CSSClass
        {
            get
            {
                return _CSSClass;
            }
            set
            {
                _CSSClass = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        #region Implement IComparable<MenuInfo>'s Method
        /// <summary>
        /// 將目前的執行個體與另一個具有相同型別的物件相比較
        /// </summary>
        /// <param name="other">要與這個物件相互比較的物件。</param>
        /// <returns>傳回值的意義如下：小於零這個物件小於 other 參數。零這個物件等於 other。大於零這個物件大於 other。</returns>
        public int CompareTo(MenuInfo other)
        {
            // A null value means that this object is greater.
            if (other == null)
            {
                return 1;
            }
            else
            {
                return this.SortNo.CompareTo(other.SortNo);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構選單資訊類別
        /// </summary>
        public MenuInfo()
        {
            _ID = String.Empty;
            _Name = String.Empty;
            _ParentID = String.Empty;
            _Url = String.Empty;
        }

        /// <summary>
        /// 建構選單資訊類別
        /// </summary>
        /// <param name="id">選單代碼</param>
        /// <param name="name">選單名稱</param>
        /// <param name="parentID">父層選單代碼</param>
        /// <param name="url">選單連結網址</param>
        /// <param name="sortNo">選單排序編號</param>
        public MenuInfo(string id, string name, string parentID, string url, int sortNo)
        {
            this.ID = id;
            this.Name = name;
            this.ParentID = parentID;
            this.Url = url;
            this.SortNo = sortNo;
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得此物件的資料是否準備好。這個方法只檢查該有資料的欄位是否有資料，不判斷資料是否正確
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false。</returns>
        public bool IsReady()
        {
            return (!String.IsNullOrEmpty(this.ID) && !String.IsNullOrEmpty(this.Name));
        }

        /// <summary>
        /// 快取子選單資料
        /// </summary>
        private MenuInfo[] _CacheChilds = null;
        /// <summary>
        /// 取得所有子選單資訊陣列
        /// </summary>
        /// <returns>傳回子選單資訊陣列。</returns>
        public MenuInfo[] GetChilds()
        {
            if (_CacheChilds == null)
            {
                _Childs.Sort();
                _CacheChilds = _Childs.ToArray();
            }
            return _CacheChilds;
        }

        /// <summary>
        /// 加入子選單資訊到子選單資料清單結尾
        /// </summary>
        /// <param name="child">要加入的子選單資訊。</param>
        /// <returns>成功則傳回 true，否則傳回 false。子選單的父選單必須是此物件，且選單代碼不可重複。</returns>
        public bool AddChild(MenuInfo child)
        {
            if (child != null && child.ParentID == this.ID 
                && (_Childs.Count == 0 || !_Childs.Exists(x => x.ID == child.ID)))
            {
                _CacheChilds = null;
                _Childs.Add(child);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除子選單清單中指定選單代碼的資料
        /// </summary>
        /// <param name="childID">要移除的選單代碼。</param>
        /// <returns>成功則傳回 true，否則傳回 false。</returns>
        public bool RemoveChild(string childID)
        {
            if (childID != null && _Childs.Count > 0)
            {
                childID = childID.Trim();
                int index = _Childs.FindIndex(x => x.ID == childID);
                if (index > -1)
                {
                    _CacheChilds = null;
                    _Childs.RemoveAt(index);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 取得符合指定選單代碼的子選單資訊
        /// </summary>
        /// <param name="childID">指定子選單的選單代碼。</param>
        /// <returns>找到則傳回子選單資訊，否則傳回 null。</returns>
        public MenuInfo FindChild(string childID)
        {
            if (childID != null && _Childs.Count > 0)
            {
                childID = childID.Trim();
                return _Childs.Find(x => x.ID == childID);
            }
            return null;
        }

        /// <summary>
        /// 取得符合指定選單代碼的任何一層的子選單資訊
        /// </summary>
        /// <param name="childID"></param>
        /// <returns></returns>
        public MenuInfo FindAnyLevelChild(string childID)
        {
            if (childID != null && _Childs.Count > 0)
            {
                childID = childID.Trim();
                foreach (MenuInfo child in _Childs)
                {
                    if (child.ID == childID)
                    {
                        return child;
                    }
                    else if (child.HasChild)
                    {
                        MenuInfo myChild = child.FindAnyLevelChild(childID);
                        if (myChild != null)
                        {
                            return myChild;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 排序子選單資料清單，依 SortNo 由小到大。
        /// </summary>
        public void SortChilds()
        {
            if (_Childs.Count > 0)
            {
                _Childs.Sort();
            }
        }


        /// <summary>
        /// 取得不分層的所有子選單代碼
        /// </summary>
        /// <returns>有子選單則傳回子選單代碼集合，否則傳回 null</returns>
        public List<string> GetAllChildIDs()
        {
            if (this.HasChild)
            {
                List<string> allChildIDs = new List<string>();
                MenuInfo[] childs = this.GetChilds();
                foreach (MenuInfo child in childs)
                {
                    allChildIDs.Add(child.ID);
                    if (child.HasChild)
                    {
                        allChildIDs.AddRange(child.GetAllChildIDs());
                    }
                }
                return allChildIDs;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }

    /// <summary>
    /// 選單資訊工具類別
    /// </summary>
    public class MenuHelper
    {
        #region Static Member And Property
        /// <summary>
        /// 儲存目前的選單資訊工具類別物的成員變數
        /// </summary>
        private static MenuHelper _Current;

        /// <summary>
        /// 取得目前的選單資訊工具類別物件，如果目前沒有則傳回一個新建的物件
        /// </summary>
        public static MenuHelper Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new MenuHelper();
                }
                else if ((DateTime.Now - _Current._LastCheckDataTime).TotalSeconds > 30)
                {
                    _Current._LastCheckDataTime = DateTime.Now;
                    string file = HttpContext.Current.Server.MapPath("~/App_Data/SchoolMenus.xml");
                    if (_Current._LoadDataLastWriteTime < File.GetLastWriteTime(file))
                    {
                        _Current = new MenuHelper();
                    }
                }
                return _Current;
            }
        }

        /// <summary>
        /// 取得匿名者授權的選單(功能)資料陣列
        /// </summary>
        /// <returns>傳回匿名者授權的選單(功能)資料陣列</returns>
        public static MenuAuth[] GetAnonymousMenuAuths()
        {
            #region [Old]
            ////[TODO] 為了方便測試，開放所有功能
            //List<MenuAuth> menuAuths = new List<MenuAuth>();

            //MenuInfo[] infos = MenuHelper.Current.GetMainMenus();
            //if (infos != null && infos.Length > 0)
            //{
            //    foreach (MenuInfo info in infos)
            //    {
            //        menuAuths.Add(new MenuAuth(info.ID, AuthCodeEnum.All));
            //        if (info.HasChild)
            //        {
            //            List<string> allChildIDs = info.GetAllChildIDs();
            //            foreach (string childID in allChildIDs)
            //            {
            //                menuAuths.Add(new MenuAuth(childID, AuthCodeEnum.All));
            //            }
            //        }
            //    }
            //}

            //return menuAuths.ToArray();
            #endregion

            return new MenuAuth[0];
        }
        #endregion

        #region Member And Property
        /// <summary>
        /// 學雜費根選單
        /// </summary>
        private MenuInfo _SchoolRoot = new MenuInfo("SCHOOL", "學雜費", null, null, 1);

        /// <summary>
        /// 載入資料的最後被寫入的時間
        /// </summary>
        private DateTime _LoadDataLastWriteTime = DateTime.MinValue;

        /// <summary>
        /// 最近一次檢查資料的時間
        /// </summary>
        private DateTime _LastCheckDataTime = DateTime.MinValue;

        /// <summary>
        /// 取得是否有選單資料
        /// </summary>
        public bool HasMenu
        {
            get
            {
                return _SchoolRoot.HasChild;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構選單資訊工具類別
        /// </summary>
        protected MenuHelper()
        {
            string errmsg = this.Load();
        }
        #endregion

        #region Method
        /// <summary>
        /// 載入資料
        /// </summary>
        /// <returns></returns>
        public string Load()
        {
            string errmsg = null;
            string file = HttpContext.Current.Server.MapPath("~/App_Data/SchoolMenus.xml");
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(file);
                _LoadDataLastWriteTime = File.GetLastWriteTime(file);
                XmlElement xRoot = xDoc.DocumentElement;
                if (xRoot.HasChildNodes)
                {
                    this.ParseChildNodes(_SchoolRoot, xRoot.ChildNodes);
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg ?? String.Empty;
        }

        /// <summary>
        /// 取得主選單(第一層)資訊陣列
        /// </summary>
        /// <returns></returns>
        public MenuInfo[] GetMainMenus()
        {
            return _SchoolRoot.GetChilds();
        }

        /// <summary>
        /// 取得指定選單代碼的子選單訊息陣列
        /// </summary>
        /// <param name="parentID">指定選單代碼。</param>
        /// <returns>傳回子選單訊息陣列。</returns>
        public MenuInfo[] GetChildMenus(string parentID)
        {
            MenuInfo parent = _SchoolRoot.FindAnyLevelChild(parentID);
            if (parent != null && parent.HasChild)
            {
                return parent.GetChilds();
            }
            return null;
        }

        /// <summary>
        /// 取得指定選單代碼的父選單資訊
        /// </summary>
        /// <param name="menuID">指定選單代碼。</param>
        /// <returns>有父選單則傳回選單訊息，否則傳回 null。</returns>
        public MenuInfo GetParentMenu(string menuID)
        {
            MenuInfo menu = _SchoolRoot.FindAnyLevelChild(menuID);
            if (menu != null && !String.IsNullOrEmpty(menu.ParentID))
            {
                return _SchoolRoot.FindAnyLevelChild(menu.ParentID);
            }
            return null;
        }

        /// <summary>
        /// 取得指定選單代碼的選單訊息
        /// </summary>
        /// <param name="menuID">指定選單代碼。</param>
        /// <returns>傳回選單訊息。</returns>
        public MenuInfo GetMenu(string menuID)
        {
            return _SchoolRoot.FindAnyLevelChild(menuID);
        }

        /// <summary>
        /// 取得指定選單代碼的所有上層選單資訊陣列
        /// </summary>
        /// <param name="menuID">指定選單代碼。</param>
        /// <returns>傳回所有上層的選單資訊陣列。</returns>
        public MenuInfo[] GetHistoryMenus(string menuID)
        {
            List<MenuInfo> menus = new List<MenuInfo>(3);
            MenuInfo menu1 = _SchoolRoot.FindAnyLevelChild(menuID);
            if (menu1 != null)
            {
                menus.Add(menu1);
                if (!String.IsNullOrEmpty(menu1.ParentID))
                {
                    MenuInfo menu2 = _SchoolRoot.FindAnyLevelChild(menu1.ParentID);
                    if (menu2 != null)
                    {
                        menus.Insert(0, menu2);
                        if (!String.IsNullOrEmpty(menu2.ParentID))
                        {
                            MenuInfo menu3 = _SchoolRoot.FindAnyLevelChild(menu2.ParentID);
                            if (menu3 != null)
                            {
                                menus.Insert(0, menu3);
                                if (!String.IsNullOrEmpty(menu3.ParentID))
                                {
                                    MenuInfo menu4 = _SchoolRoot.FindAnyLevelChild(menu3.ParentID);
                                    if (menu4 != null)
                                    {
                                        //假設最多四層
                                        menus.Insert(0, menu4);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return menus.ToArray();
        }

        /// <summary>
        /// 拆解子節點資料集合，並附加到指定選單資訊的子選單資料清單結尾
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="xNodes"></param>
        private void ParseChildNodes(MenuInfo menu, XmlNodeList xNodes)
        {
            if (xNodes != null && xNodes.Count > 0)
            {
                foreach (XmlNode xNode in xNodes)
                {
                    MenuInfo child = this.ParseNode(xNode, menu.ID);
                    if (child != null)
                    {
                        menu.AddChild(child);
                        if (xNode.HasChildNodes)
                        {
                            this.ParseChildNodes(child, xNode.ChildNodes);
                            child.SortChilds();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 拆解節點資料，傳回選單資訊物件
        /// </summary>
        /// <param name="xNode">要猜解的節點。</param>
        /// <param name="parentID">父選單代碼。</param>
        /// <returns>成功則傳回選單資訊物件，否則傳回 null。</returns>
        private MenuInfo ParseNode(XmlNode xNode, string parentID)
        {
            if (xNode != null && xNode.LocalName == "menu" && xNode.Attributes.Count > 0)
            {
                MenuInfo menu = new MenuInfo();
                XmlNode xAttr = null;

                xAttr = xNode.Attributes.GetNamedItem("id");
                if (xAttr == null)
                {
                    return null;
                }
                menu.ID = xAttr.Value;

                xAttr = xNode.Attributes.GetNamedItem("name");
                if (xAttr == null)
                {
                    return null;
                }
                menu.Name = xAttr.Value;

                xAttr = xNode.Attributes.GetNamedItem("url");
                if (xAttr != null)
                {
                    menu.Url = xAttr.Value;
                }

                int sortNo = 0;
                xAttr = xNode.Attributes.GetNamedItem("sortNo");
                if (xAttr != null && int.TryParse(xAttr.Value, out sortNo))
                {
                    menu.SortNo = sortNo;
                }

                xAttr = xNode.Attributes.GetNamedItem("cssClass");
                if (xAttr != null)
                {
                    menu.CSSClass = xAttr.Value;
                }

                menu.ParentID = parentID;
                return menu;
            }
            return null;
        }
        #endregion
    }
}