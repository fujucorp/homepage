using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Entities
{
    /// <summary>
    /// 選單(功能)授權資料類別
    /// </summary>
    [Serializable]
    public class MenuAuth
    {
        #region Property
        private string _MenuID = null;
        /// <summary>
        /// 選單(功能)代碼
        /// </summary>
        public string MenuID
        {
            get
            {
                return _MenuID;
            }
            set
            {
                _MenuID = value == null ? String.Empty : value.Trim();
            }
        }

        private AuthCodeEnum _AuthCode = AuthCodeEnum.None;
        /// <summary>
        /// 授權代碼
        /// </summary>
        public AuthCodeEnum AuthCode
        {
            get
            {
                return _AuthCode;
            }
            set
            {
                _AuthCode = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構選單(功能)授權資料類別
        /// </summary>
        public MenuAuth()
        {
        }

        /// <summary>
        /// 建構選單(功能)授權資料類別
        /// </summary>
        /// <param name="menuID"></param>
        /// <param name="authCode"></param>
        public MenuAuth(string menuID, AuthCodeEnum authCode)
        {
            this.MenuID = menuID;
            this.AuthCode = authCode;
        }
        #endregion

        #region
        /// <summary>
        /// 取得是否有新增授權
        /// </summary>
        /// <returns></returns>
        public bool HasInsert()
        {
            return AuthCodeHelper.HasInsert(this.AuthCode);
        }

        /// <summary>
        /// 取得是否有修改授權
        /// </summary>
        /// <returns></returns>
        public bool HasUpdate()
        {
            return AuthCodeHelper.HasUpdate(this.AuthCode);
        }

        /// <summary>
        /// 取得是否有刪除授權
        /// </summary>
        /// <returns></returns>
        public bool HasDelete()
        {
            return AuthCodeHelper.HasDelete(this.AuthCode);
        }

        /// <summary>
        /// 取得是否有查詢授權
        /// </summary>
        /// <returns></returns>
        public bool HasSelect()
        {
            return AuthCodeHelper.HasSelect(this.AuthCode);
        }

        /// <summary>
        /// 取得是否有列印授權
        /// </summary>
        /// <returns></returns>
        public bool HasPrint()
        {
            return AuthCodeHelper.HasPrint(this.AuthCode);
        }

        /// <summary>
        /// 取得是否有維護授權
        /// </summary>
        /// <returns></returns>
        public bool HasMaintain()
        {
            return AuthCodeHelper.HasMaintain(this.AuthCode);
        }

        /// <summary>
        /// 取得是否有查詢授權
        /// </summary>
        /// <returns></returns>
        public bool HasQuery()
        {
            return AuthCodeHelper.HasQuery(this.AuthCode);
        }

        /// <summary>
        /// 取得是否有任何授權
        /// </summary>
        /// <returns></returns>
        public bool HasAnyone()
        {
            return AuthCodeHelper.HasAnyone(this.AuthCode);
        }
        #endregion
    }
}
