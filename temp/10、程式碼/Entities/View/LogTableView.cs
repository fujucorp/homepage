using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class LogTableView : Entity
    {
        protected const string VIEWSQL = @"SELECT L.*
     , F.Func_Name
  FROM (
SELECT L.*, U.U_RT, U.U_Name
  FROM [Log_Table] AS L
  LEFT JOIN [Users] AS U ON U.U_ID = L.User_Id AND U.U_Bank = L.Receive_Type
 WHERE L.Role = '2'
UNION ALL
SELECT L.*, '' AS U_RT, '' AS U_Name
  FROM [Log_Table] AS L
 WHERE L.Role = '1'
) AS L
  LEFT JOIN FuncMenu F on L.Function_Id = F.Func_Id";


        #region Field Name Const Class
        /// <summary>
        /// LogTableView 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey 相關欄位
            #endregion

            #region 資料相關欄位
            /// <summary>
            /// 記錄日誌的使用者單位代碼 (Users.U_BANK 學校統編/銀行代碼)
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 記錄日誌的功能代碼
            /// </summary>
            public const string FunctionId = "Function_Id";

            /// <summary>
            /// 資料庫處理操作代碼 (請參考 LogTypeCodeTexts)
            /// </summary>
            public const string LogType = "Log_Type";

            /// <summary>
            /// 記錄日誌的日期 (格式：民國年3碼 + 月2碼 + 日2碼)
            /// </summary>
            public const string LogDate = "Log_Date";

            /// <summary>
            /// 記錄日誌的時間 (格式：HHmmss)
            /// </summary>
            public const string LogTime = "Log_Time";

            /// <summary>
            /// 記錄日誌的使用者代碼
            /// </summary>
            public const string UserId = "User_Id";

            /// <summary>
            /// 資料庫處理結果
            /// </summary>
            public const string Notation = "Notation";

            /// <summary>
            /// 使用者角色 (1=行員 / 2=學校，請參考 RoleCodeTexts)
            /// </summary>
            public const string Role = "Role";

            /// <summary>
            /// 使用者授權的業務別碼 (以逗號區隔，行員是空白)
            /// </summary>
            public const string UserReceiveType = "U_RT";

            /// <summary>
            /// 使用者名稱 (行員是空白)
            /// </summary>
            public const string UserName = "U_Name";

            /// <summary>
            /// 功能選單名稱 欄位名稱常數定義
            /// </summary>
            public const string FuncName = "Func_Name";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// UsersView 類別建構式
        /// </summary>
        public LogTableView()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        #endregion

        #region Data
        private string _ReceiveType = null;
        /// <summary>
        /// 記錄日誌的使用者單位代碼 (Users.U_BANK 學校統編/銀行代碼) 
        /// </summary>
        [FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 6, false)]
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

        private string _FunctionId = null;
        /// <summary>
        /// 記錄日誌的功能代碼
        /// </summary>
        [FieldSpec(Field.FunctionId, false, FieldTypeEnum.Char, 4, false)]
        public string FunctionId
        {
            get
            {
                return _FunctionId;
            }
            set
            {
                _FunctionId = value == null ? null : value.Trim();
            }
        }

        private string _LogType = null;
        /// <summary>
        /// 資料庫處理操作代碼 (請參考 LogTypeCodeTexts)
        /// </summary>
        [FieldSpec(Field.LogType, false, FieldTypeEnum.Char, 2, true)]
        public string LogType
        {
            get
            {
                return _LogType;
            }
            set
            {
                _LogType = value == null ? null : value.Trim();
            }
        }

        private string _LogDate = null;
        /// <summary>
        /// 記錄日誌的日期 (格式：民國年3碼 + 月2碼 + 日2碼)
        /// </summary>
        [FieldSpec(Field.LogDate, false, FieldTypeEnum.Char, 7, false)]
        public string LogDate
        {
            get
            {
                return _LogDate;
            }
            set
            {
                _LogDate = value == null ? null : value.Trim();
            }
        }

        private string _LogTime = null;
        /// <summary>
        /// 記錄日誌的時間 (格式：HHmmss)
        /// </summary>
        [FieldSpec(Field.LogTime, false, FieldTypeEnum.Char, 6, false)]
        public string LogTime
        {
            get
            {
                return _LogTime;
            }
            set
            {
                _LogTime = value == null ? null : value.Trim();
            }
        }

        private string _UserId = null;
        /// <summary>
        /// 記錄日誌的使用者代碼
        /// </summary>
        [FieldSpec(Field.UserId, false, FieldTypeEnum.VarChar, 50, true)]
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

        private string _Notation = null;
        /// <summary>
        /// 資料庫處理結果
        /// </summary>
        [FieldSpec(Field.Notation, false, FieldTypeEnum.NVarCharMax, false)]
        public string Notation
        {
            get
            {
                return _Notation;
            }
            set
            {
                _Notation = value == null ? null : value.Trim();
            }
        }

        private string _Role = null;
        /// <summary>
        /// 使用者角色 (1=行員 / 2=學校，請參考 RoleCodeTexts)
        /// </summary>
        [FieldSpec(Field.Role, false, FieldTypeEnum.Char, 1, false)]
        public string Role
        {
            get
            {
                return _Role;
            }
            set
            {
                _Role = value == null ? null : value.Trim();
            }
        }

        private string _UserReceiveType = null;
        /// <summary>
        /// 使用者授權的業務別碼 (以逗號區隔，行員是空白)
        /// </summary>
        [FieldSpec(Field.UserReceiveType, false, FieldTypeEnum.VarChar, 100, false)]
        public string UserReceiveType
        {
            get
            {
                return _UserReceiveType;
            }
            set
            {
                _UserReceiveType = value == null ? null : value.Trim();
            }
        }

        private string _UserName = null;
        /// <summary>
        /// 使用者名稱 (行員是空白)
        /// </summary>
        [FieldSpec(Field.UserName, false, FieldTypeEnum.NVarChar, 32, false)]
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value == null ? null : value.Trim();
            }
        }

        private string _FuncName = null;
        /// <summary>
        /// 功能選單名稱
        /// </summary>
        [FieldSpec(Field.FuncName, false, FieldTypeEnum.NVarChar, 50, false)]
        public string FuncName
        {
            get
            {
                return _FuncName;
            }
            set
            {
                _FuncName = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion
    }
}
