using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Entities
{
    /// <summary>
    /// 登入者資料類別
    /// </summary>
    [Serializable]
    public class LogonUser
    {
        #region Static Readonly
        /// <summary>
        /// 匿名者 (未登入) 的使用者帳號 : ANONYMOUS
        /// </summary>
        public static readonly string ANONYMOUS_USERID = "ANONYMOUS";
        #endregion

        #region Property
        #region 使用者資料
        private string _UserQual = String.Empty;
        /// <summary>
        /// 使用者的身分別 (1:銀行使用者、2:學校使用者、3:學生使用者 請參考 UserQualCodeTexts)
        /// </summary>
        public string UserQual
        {
            get
            {
                return _UserQual;
            }
            set
            {
                _UserQual = value == null ? String.Empty : value.Trim();
            }
        }

        private string _UserId = String.Empty;
        /// <summary>
        /// 使用者的帳號 (登入帳號) (UserQual為 1:使用者帳號(行員工號) / 2:使用者帳號 / 3:學生學號)
        /// </summary>
        public string UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value == null ? String.Empty : value.Trim();
            }
        }

        #region User 相關 Key
        private string _ReceiveType = String.Empty;
        /// <summary>
        /// 使用者的商家代號 (UserQual 為 2 或 3 才有值)
        /// </summary>
        public string ReceiveType
        {
            get
            {
                return _ReceiveType;
            }
            set
            {
                _ReceiveType = value == null ? String.Empty : value.Trim();
            }
        }

        private string _GroupId = String.Empty;
        /// <summary>
        /// 使用者的群組代碼 (UserQual 為 1 或 2 才有值)
        /// </summary>
        public string GroupId
        {
            get
            {
                return _GroupId;
            }
            set
            {
                _GroupId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _BankId = String.Empty;
        /// <summary>
        /// 使用者的銀行代碼 (UserQual 為 1:分行代碼(6碼) / 2:學校代碼 / 3: 商家代號?)
        /// </summary>
        public string BankId
        {
            get
            {
                return _BankId;
            }
            set
            {
                _BankId = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        #region Student 相關 Key
        private string _DepId = String.Empty;
        /// <summary>
        /// 學生的部別代碼 (UserQual 為 3 才有值)
        /// </summary>
        public string DepId
        {
            get
            {
                return _DepId;
            }
            set
            {
                _DepId = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        #region SSO 相關 Key
        private string _SSOPusid = String.Empty;
        /// <summary>
        /// 使用 SSO 登入的 pusid (土銀使用 AD 驗證所以沒有 SSO)
        /// </summary>
        public string SSOPusid
        {
            get
            {
                return _SSOPusid;
            }
            set
            {
                _SSOPusid = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        private string _UserName = String.Empty;
        /// <summary>
        /// 使用者的名稱
        /// </summary>
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value == null ? String.Empty : value.Trim();
            }
        }

        private string _ReceiveTypeName = String.Empty;
        /// <summary>
        /// 商家代號名稱 (學校名稱) (UserQual為 2 或 3 才有值)
        /// </summary>
        public string ReceiveTypeName
        {
            get
            {
                return _ReceiveTypeName;
            }
            set
            {
                _ReceiveTypeName = value == null ? String.Empty : value.Trim();
            }
        }

        #region [MDY:202203XX] 2022擴充案 學校英文名稱
        private string _ReceiveTypeEName = String.Empty;
        /// <summary>
        /// 商家代號英文名稱 (學校英文名稱) (UserQual為 2 或 3 才有值)
        /// </summary>
        public string ReceiveTypeEName
        {
            get
            {
                return _ReceiveTypeEName;
            }
            set
            {
                _ReceiveTypeEName = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion
        #endregion

        #region 單位資料
        private string _UnitId = String.Empty;
        /// <summary>
        /// 單位代碼 (UserQual為 1:分行代碼(6碼) / 2:學校代碼 / 3:商家代號)
        /// </summary>
        public string UnitId
        {
            get
            {
                return _UnitId;
            }
            set
            {
                _UnitId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _UnitName = String.Empty;
        /// <summary>
        /// 單位名稱 (UserQual為 1:分行名稱 / 2:學校名稱 / 3:學校名稱)
        /// </summary>
        public string UnitName
        {
            get
            {
                return _UnitName;
            }
            set
            {
                _UnitName = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        #region 權限資料
        private string _RoleType = RoleTypeCodeTexts.USER;
        /// <summary>
        /// 使用者的權限角色代碼 (2:使用者 (經辦)、3:管理者)
        /// </summary>
        public string RoleType
        {
            get
            {
                return _RoleType;
            }
            set
            {
                if (value == RoleTypeCodeTexts.MANAGER)
                {
                    _RoleType = value;
                }
                else
                {
                    _RoleType = RoleTypeCodeTexts.USER;
                }
            }
        }

        /// <summary>
        /// 授權的選單(功能)資料陣列 (UserQual 為 3:學生的權限固定，所以放空陣列 / 其他放授權)
        /// </summary>
        [XmlElement(typeof(MenuAuth[]))]
        public MenuAuth[] AuthMenus
        {
            get;
            set;
        }

        private string[] _MyReceiveTypes = new string[0];
        /// <summary>
        /// 授權的商家代號 (UserQual為 1 且 RoleType為 3:無資料 / UserQual為 1 且 RoleType為 2 或 UserQual為 2:授權的商家代號 / 3:學生登入使用學校代碼，這裡存放該學校的所屬商家代號)
        /// </summary>
        public string[] MyReceiveTypes
        {
            get
            {
                return _MyReceiveTypes;
            }
            set
            {
                _MyReceiveTypes = value == null ? new string[0] : value;
            }
        }

        private string[] _MySchIdentys = new string[0];
        /// <summary>
        /// 授權的學校代碼 (UserQual為 1 且 RoleType為 2 :授權的學校代碼 / 其他:無資料)
        /// </summary>
        public string[] MySchIdentys
        {
            get
            {
                return _MySchIdentys;
            }
            set
            {
                _MySchIdentys = value == null ? new string[0] : value;
            }
        }
        #endregion

        #region 登入資料
        private string _ClientIP = String.Empty;
        /// <summary>
        /// 登入 IP xxx.xxx.xxx.xxx
        /// </summary>
        public string ClientIP
        {
            get
            {
                return _ClientIP;
            }
            set
            {
                _ClientIP = value == null ? String.Empty : value.Trim();
            }
        }

        private string _CultureName = "zh-tw";
        /// <summary>
        /// 使用語系 (預設 "zh-tw")
        /// </summary>
        public string CultureName
        {
            get
            {
                return _CultureName;
            }
            set
            {
                value = value == null ? String.Empty : value.Trim().ToLower();
                if (value.Length == 0)
                {
                    _CultureName = "zh-tw";
                }
                else
                {
                    _CultureName = value;
                }
            }
        }

        private DateTime? _LogonTime = null;
        /// <summary>
        /// 登入時間
        /// </summary>
        public DateTime? LogonTime
        {
            get
            {
                return _LogonTime;
            }
            set
            {
                _LogonTime = value;
            }
        }

        private string _LogonSN = null;
        /// <summary>
        /// 登入序號
        /// </summary>
        public string LogonSN
        {
            get
            {
                return _LogonSN;
            }
            set
            {
                _LogonSN = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 額外資料
        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 是否需要提醒更改密碼
        /// </summary>
        public bool IsRemindChangePXX
        {
            get;
            set;
        }
        #endregion
        #endregion
        #endregion

        #region Readonly Property
        /// <summary>
        /// 取得是否為行員的使用者
        /// </summary>
        [XmlIgnore]
        public bool IsBankUser
        {
            get
            {
                return UserQualCodeTexts.IsBankUser(this.UserQual);
            }
        }

        /// <summary>
        /// 取得是否為總行的使用者
        /// </summary>
        [XmlIgnore]
        public bool IsBankManager
        {
            get
            {
                #region [MDY:20161010] 統一由 BankADGroupCodeTexts.IsHeadOffice 判斷
                #region [Old]
                //return (this.IsBankUser && this.RoleType == RoleTypeCodeTexts.MANAGER);
                #endregion

                return (this.IsBankUser && BankADGroupCodeTexts.IsHeadOffice(this.GroupId));
                #endregion
            }
        }

        /// <summary>
        /// 取得是否為學校的使用者
        /// </summary>
        [XmlIgnore]
        public bool IsSchoolUser
        {
            get
            {
                return UserQualCodeTexts.IsSchoolUser(this.UserQual);
            }
        }

        /// <summary>
        /// 取得是否為學校的主管
        /// </summary>
        [XmlIgnore]
        public bool IsSchoolManager
        {
            get
            {
                return (this.IsSchoolUser && this.RoleType == RoleTypeCodeTexts.MANAGER);
            }
        }

        /// <summary>
        /// 取得是否為學生的使用者
        /// </summary>
        [XmlIgnore]
        public bool IsStudentUser
        {
            get
            {
                return UserQualCodeTexts.IsStudentUser(this.UserQual);
            }
        }

        /// <summary>
        /// 取得是否為匿名者 (未登入)。無帳號或帳號為 ANONYMOUS 或 身份別不正確 或 同時沒有所屬商家代號與所屬銀行，傳回 true。
        /// </summary>
        [XmlIgnore]
        public bool IsAnonymous
        {
            get
            {
                return ((this.UserId == ANONYMOUS_USERID || String.IsNullOrEmpty(this.UserId))
                    || !UserQualCodeTexts.IsDefine(this.UserQual)
                    || (String.IsNullOrEmpty(this.ReceiveType) && String.IsNullOrEmpty(this.BankId)));
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構登入者資料類別
        /// </summary>
        public LogonUser()
        {
            //this.UserSN = string.Empty;
            //this.UserID = string.Empty;
            //this.UserName = string.Empty;
            //this.UserPWD = string.Empty;
            //this.UserQual = string.Empty;
            //this.UserEmail = string.Empty;
            //this.UserTel = string.Empty;
            //this.UserRole = string.Empty;
            //this.UserTitle = string.Empty;
            //this.Department = string.Empty;
            //this.IsAdmin = false;
            //this.PWDChangeDate = null;
            //this.PWDWrongTimes = 0;

            ////this.PayerID = string.Empty;

            //this.UnitID = string.Empty;
            //this.UnitName = string.Empty;

            //this.GroupIDs = new string[0];
            //this.VIDs = new string[0];
            //this.FunctionRights = new FunctionRight[0];

            //this.ClientIP = string.Empty;
            //this.BrowserType = string.Empty;
            //this.BrowserPlatform = string.Empty;
            //this.LoginTime = string.Empty;
            //this.LoginWay = String.Empty;
            //this.CultureName = "zh-tw";
        }
        #endregion


        #region 檢查授權相關 Method
        /// <summary>
        /// 取得指定選單(功能)代碼是否有授權
        /// </summary>
        /// <param name="menuID">要檢查的選單(功能)代碼</param>
        /// <returns>有授權則傳回 true，否則傳回 false</returns>
        public bool IsAuthMenuID(string menuID)
        {
            MenuAuth menuAuth = null;
            MenuAuth[] authMenus = this.AuthMenus;
            if (!String.IsNullOrEmpty(menuID) && authMenus != null && authMenus.Length > 0)
            {
                menuAuth = authMenus.FirstOrDefault<MenuAuth>(x => x.MenuID.Equals(menuID));
                if (menuAuth != null)
                {
                    return menuAuth.HasAnyone();
                }
            }
            return false;
        }

        /// <summary>
        /// 取得指定選單(功能)代碼的授權資料
        /// </summary>
        /// <param name="menuID">選單(功能)代碼</param>
        /// <returns>傳回授權資料</returns>
        public MenuAuth GetMenuAuth(string menuID)
        {
            MenuAuth menuAuth = null;
            MenuAuth[] authMenus = this.AuthMenus;
            if (!String.IsNullOrEmpty(menuID) && authMenus != null && authMenus.Length > 0)
            {
                menuAuth = authMenus.FirstOrDefault<MenuAuth>(x => x.MenuID.Equals(menuID));
            }
            if (menuAuth == null)
            {
                menuAuth = new MenuAuth(menuID, AuthCodeEnum.None);
            }
            return menuAuth;
        }

        /// <summary>
        /// 取得指定商家代號是否有授權
        /// </summary>
        /// <returns>有授權則傳回 true，否則傳回 false</returns>
        public bool IsAuthReceiveTypes(string receiveType)
        {
            if (this.IsBankManager)
            {
                return true;
            }
            string[] receiveTypes = this.MyReceiveTypes;
            if (receiveTypes != null && receiveTypes.Length > 0)
            {
                return receiveTypes.Contains(receiveType);
            }
            return false;
        }

        /// <summary>
        /// 取得指定學校代碼是否有授權
        /// </summary>
        /// <returns>有授權則傳回 true，否則傳回 false</returns>
        public bool IsMySchIdenty(string schIdenty)
        {
            if (this.IsBankManager)
            {
                return true;
            }
            string[] schIdentys = this.MySchIdentys;
            if (schIdentys != null && schIdentys.Length > 0)
            {
                return schIdentys.Contains(schIdenty);
            }
            return false;
        }

        /// <summary>
        /// 取得是否有任何授權的選單(功能)
        /// </summary>
        /// <returns>有授權的選單(功能)則傳回 true，否則傳回 false</returns>
        public bool HasAuthMenus()
        {
            return (this.AuthMenus != null && this.AuthMenus.Length > 0);
        }

        /// <summary>
        /// 取得是否有任何授權的商家代號
        /// </summary>
        /// <returns>有授權的商家代號(或銀行管理者)則傳回 true，否則傳回 false</returns>
        public bool HasAuthReceiveType()
        {
            return (this.IsBankManager || (this.MyReceiveTypes != null && this.MyReceiveTypes.Length > 0));
        }
        #endregion

        #region Static Member
        /// <summary>
        /// 產生匿名者資料
        /// </summary>
        /// <returns></returns>
        public static LogonUser GenAnonymous()
        {
            LogonUser anonymous = new LogonUser();
            anonymous.UserQual = null;
            anonymous.UserId = ANONYMOUS_USERID;
            anonymous.ReceiveType = null;
            anonymous.GroupId = null;
            anonymous.BankId = null;
            anonymous.DepId = null;
            anonymous.SSOPusid = null;

            anonymous.UserName = "匿名者";
            anonymous.ReceiveTypeName = null;

            #region [MDY:202203XX] 2022擴充案 學校英文名稱
            anonymous.ReceiveTypeEName = null;
            #endregion

            anonymous.UnitId = null;
            anonymous.UnitName = null;

            anonymous.RoleType = null;
            anonymous.AuthMenus = null;

            anonymous.LogonTime = null;
            return anonymous;
        }

        #region [Old] 測試用，暫時保留
        ///// <summary>
        ///// 產生測試用的銀行管理者
        ///// </summary>
        ///// <returns></returns>
        //public static LogonUser GenTestManager()
        //{
        //    LogonUser manager = new LogonUser();
        //    manager.UserQual = UserQualCodeTexts.BANK;
        //    manager.UserId = "MANAGER";
        //    manager.RoleType = RoleTypeCodeTexts.MANAGER;
        //    manager.ReceiveType = null;
        //    manager.GroupId = "1";
        //    manager.BankId = "0041020";
        //    manager.DepId = null;
        //    manager.SSOPusid = null;

        //    manager.UserName = "測試管理者";
        //    manager.ReceiveTypeName = null;

        //    manager.UnitId = "0041020";
        //    manager.UnitName = "資訊室";

        //    manager.RoleType = RoleTypeCodeTexts.MANAGER;
        //    manager.AuthMenus = null;

        //    manager.LogonTime = DateTime.Now;
        //    return manager;
        //}
        #endregion
        #endregion
    }
}
