/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/12/29 15:13:56
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 使用者 View
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class UsersView : Entity
    {
        protected const string VIEWSQL = @"
SELECT [" + UsersEntity.Field.UId + @"], [" + UsersEntity.Field.URt + @"], [" + UsersEntity.Field.UGrp + @"], [" + UsersEntity.Field.UBank + @"] 
     , [" + UsersEntity.Field.UName + @"], ISNULL([" + UsersEntity.Field.Approver + @"], '') [" + UsersEntity.Field.Approver + @"], [" + UsersEntity.Field.Creator + @"]
     , ISNULL((SELECT TOP 1 [" + BankEntity.Field.BankSName + @"] FROM [" + BankEntity.TABLE_NAME + @"] AS B WHERE B.[" + BankEntity.Field.BankNo + @"] = U.[" + UsersEntity.Field.UBank + @"]), '') AS [BANK_S_NAME]
  FROM [" + UsersEntity.TABLE_NAME + @"] AS U";

        #region Field Name Const Class
        /// <summary>
        /// UsersView 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 使用者帳號 (PK) 欄位名稱常數定義
            /// </summary>
            public const string UserId = UsersEntity.Field.UId;

            /// <summary>
            /// 所屬代收類別代碼 (PK) 欄位名稱常數定義
            /// </summary>
            public const string ReceiveType = UsersEntity.Field.URt;

            /// <summary>
            /// 所屬群組代碼 (PK) 欄位名稱常數定義
            /// </summary>
            public const string GroupId = UsersEntity.Field.UGrp;

            /// <summary>
            /// 所屬分行(7碼) 或銀行(3碼) 代碼 (PK) 欄位名稱常數定義
            /// </summary>
            public const string BankId = UsersEntity.Field.UBank;
            #endregion

            #region Data
            /// <summary>
            /// 使用者名稱 欄位名稱常數定義
            /// </summary>
            public const string UserName = UsersEntity.Field.UName;

            /// <summary>
            /// 資料放行者 欄位名稱常數定義
            /// </summary>
            public const string Approver = UsersEntity.Field.Approver;

            /// <summary>
            /// 資料建立者 欄位名稱常數定義
            /// </summary>
            public const string Creator = UsersEntity.Field.Creator;

            /// <summary>
            /// 所屬分行 或銀行 的簡稱 欄位名稱常數定義
            /// </summary>
            public const string BankSName = "BANK_S_NAME";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// UsersView 類別建構式
        /// </summary>
        public UsersView()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _UserId = null;
        /// <summary>
        /// 使用者帳號 (PK)
        /// </summary>
        [FieldSpec(Field.UserId, true, FieldTypeEnum.VarCharMax, false)]
        public string UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveType = null;
        /// <summary>
        /// 所屬代收類別代碼 (PK)
        /// </summary>
        [FieldSpec(Field.ReceiveType, true, FieldTypeEnum.VarCharMax, false)]
        public string ReceiveType
        {
            get
            {
                return _ReceiveType;
            }
            set
            {
                _ReceiveType = value == null ? null : value.Trim();
            }
        }

        private string _GroupId = null;
        /// <summary>
        /// 所屬群組代碼 (PK)
        /// </summary>
        [FieldSpec(Field.GroupId, true, FieldTypeEnum.VarCharMax, false)]
        public string GroupId
        {
            get
            {
                return _GroupId;
            }
            set
            {
                _GroupId = value == null ? null : value.Trim();
            }
        }

        private string _BankId = null;
        /// <summary>
        /// 所屬分行(7碼) 或銀行(3碼) 代碼 (PK)
        /// </summary>
        [FieldSpec(Field.BankId, true, FieldTypeEnum.VarCharMax, false)]
        public string BankId
        {
            get
            {
                return _BankId;
            }
            set
            {
                _BankId = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Data
        /// <summary>
        /// 使用者名稱
        /// </summary>
        [FieldSpec(Field.UserName, false, FieldTypeEnum.VarCharMax, true)]
        public string UserName
        {
            get;
            set;
        }

        private string _Approver = null;
        /// <summary>
        /// 資料放行者
        /// </summary>
        [FieldSpec(Field.Approver, false, FieldTypeEnum.VarCharMax, true)]
        public string Approver
        {
            get
            {
                return _Approver;
            }
            set
            {
                _Approver = value == null ? null : value.Trim();
            }
        }

        private string _Creator = null;
        /// <summary>
        /// 資料建立者
        /// </summary>
        [FieldSpec(Field.Creator, false, FieldTypeEnum.VarCharMax, true)]
        public string Creator
        {
            get
            {
                return _Creator;
            }
            set
            {
                _Creator = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 所屬分行 或銀行 的簡稱
        /// </summary>
        [FieldSpec(Field.BankSName, false, FieldTypeEnum.VarCharMax, true)]
        public string BankSName
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
