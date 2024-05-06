using System;
using System.Xml.Serialization;

namespace Entities
{
    /// <summary>
    /// 服務命令請求者資料類別
    /// </summary>
    [Serializable]
    public class CommandAsker
    {
        #region Property
        private string _UserQual = String.Empty;
        /// <summary>
        /// 請求者的身分別 (1:銀行使用者、2:學校使用者、3:學生使用者 請參考 UserQualCodeTexts)
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

        private string _UnitId = String.Empty;
        /// <summary>
        /// 請求者的登入單位代碼 (UserQual為 1:分行代碼全碼(6碼) / 2:學校代碼 / 3:商家代號)
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

        private string _UserId = String.Empty;
        /// <summary>
        /// 請求者的帳號 (UserQual為 1:使用者帳號(行員工號) 2:使用者帳號 / 3:學生學號)
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

        private string _GroupId = String.Empty;
        /// <summary>
        /// 請求者的群組
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

        private string _RoleType = String.Empty;
        /// <summary>
        /// 請求者的權限角色代碼 (請參考 RoleTypeCodeTexts)
        /// </summary>
        public string RoleType
        {
            get
            {
                return _RoleType;
            }
            set
            {
                _RoleType = value == null ? String.Empty : value.Trim();
            }
        }

        private string _CultureName = "zh-tw";
        /// <summary>
        /// 請求者的使用語系 (預設 "zh-tw")
        /// </summary>
        public string CultureName
        {
            get
            {
                return _CultureName;
            }
            set
            {
                if (value != null)
                {
                    value = value.Trim().ToLower();
                }
                if (String.IsNullOrEmpty(value))
                {
                    _CultureName = "zh-tw";
                }
                else
                {
                    _CultureName = value;
                }
            }
        }

        /// <summary>
        /// 請求者的登入時間
        /// </summary>
        public DateTime? LogonTime
        {
            get;
            set;
        }

        private string _LogonSN = null;
        /// <summary>
        /// 請求者的登入序號
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

        private string _FuncId = String.Empty;
        /// <summary>
        /// 呼叫服務的前端(選單)功能代碼
        /// </summary>
        public string FuncId
        {
            get
            {
                return _FuncId;
            }
            set
            {
                _FuncId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _FuncName = String.Empty;
        /// <summary>
        /// 呼叫服務的前端(選單)功能名稱
        /// </summary>
        public string FuncName
        {
            get
            {
                return _FuncName;
            }
            set
            {
                _FuncName = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        #region Readonly Propery
        /// <summary>
        /// 基本資料是否 ready
        /// </summary>
        [XmlIgnore]
        public bool IsReady
        {
            get
            {
                #region [Old]
                //if (!String.IsNullOrEmpty(this.UserId) && !String.IsNullOrEmpty(this.GroupId))
                //{
                //    if ((!String.IsNullOrEmpty(this.UnitId) && UserQualCodeTexts.IsDefine(this.UserQual))
                //        || this.UserId == LogonUser.ANONYMOUS_USERID)
                //    {
                //        return true;
                //    }
                //}
                #endregion

                if (this.UserId == LogonUser.ANONYMOUS_USERID)
                {
                    return true;
                }
                else if (!String.IsNullOrEmpty(this.UserId) && !String.IsNullOrEmpty(this.UnitId) && UserQualCodeTexts.IsDefine(this.UserQual))
                {
                   if ((this.UserQual == UserQualCodeTexts.SCHOOL || this.UserQual == UserQualCodeTexts.BANK) && String.IsNullOrEmpty(this.GroupId))
                   {
                       return false;
                   }
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 是否為行員的使用者
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
        /// 是否為總行的使用者
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
        /// 是否為學校的使用者
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
        /// 是否為學生的使用者
        /// </summary>
        [XmlIgnore]
        public bool IsStudentUser
        {
            get
            {
                return UserQualCodeTexts.IsStudentUser(this.UserQual);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 服務命令請求者資料類別
        /// </summary>
        public CommandAsker()
        {
        }
        #endregion

        #region Static Metyhod
        /// <summary>
        /// 建立服務命令請求者資料物件
        /// </summary>
        /// <param name="logonUser">指定登入者資料。</param>
        /// <param name="funcID">指定執行功能代碼。</param>
        /// <param name="funcName">指定執行功能名稱。</param>
        /// <returns>傳回服務命令請求者資料物件。</returns>
        public static CommandAsker Create(LogonUser logonUser, string funcID, string funcName)
        {
            if (logonUser == null)
            {
                return null;
            }

            CommandAsker asker = new CommandAsker();
            asker.UserQual = logonUser.UserQual;
            asker.UnitId = logonUser.UnitId;
            asker.UserId = logonUser.UserId;
            asker.GroupId = logonUser.GroupId;
            asker.RoleType = logonUser.RoleType;

            asker.CultureName = logonUser.CultureName;
            asker.LogonTime = logonUser.LogonTime;
            asker.LogonSN = logonUser.LogonSN;

            asker.FuncId = funcID;
            asker.FuncName = funcName;
            return asker;
        }
        #endregion
    }
}
