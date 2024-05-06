#region 檔案說明
// 類別：School.Entities.LogTableEntity (資料庫處理日誌 承載類別)
// 說明：用來承載 Log_Table 資料表，此資料表用來記錄對資料庫處理的日誌
// 日期：2014/10/28
// 備註：已完成，已測試
#endregion

#region 修改說明
// Date     | No          | Modifier | Memo
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 資料庫處理日誌承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME)]
    public class LogTableEntity : Entity
    {
        public const string TABLE_NAME = "Log_Table";

        #region 資料來源欄位名稱常數定義
        /// <summary>
        /// 資料庫處理日誌 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey 相關欄位
            #endregion

            #region 資料相關欄位
            /// <summary>
            /// 記錄日誌的代收類別代碼 欄位名稱常數定義
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 記錄日誌的功能代碼 欄位名稱常數定義
            /// </summary>
            public const string FunctionId = "Function_Id";

            /// <summary>
            /// 資料庫處理操作代碼 欄位名稱常數定義
            /// </summary>
            public const string LogType = "Log_Type";

            /// <summary>
            /// 記錄日誌的日期 欄位名稱常數定義
            /// </summary>
            public const string LogDate = "Log_Date";

            /// <summary>
            /// 記錄日誌的時間 欄位名稱常數定義
            /// </summary>
            public const string LogTime = "Log_Time";

            /// <summary>
            /// 記錄日誌的使用者代碼 欄位名稱常數定義
            /// </summary>
            public const string UserId = "User_Id";

            /// <summary>
            /// 資料庫處理結果 欄位名稱常數定義
            /// </summary>
            public const string Notation = "Notation";

            /// <summary>
            /// 群組角色 (1=行員 / 2=學校，請參考 RoleCodeTexts) 欄位名稱常數定義
            /// </summary>
            public const string Role = "Role";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構資料庫處理日誌承載類別
        /// </summary>
        public LogTableEntity()
            : base()
        {
        }

        /// <summary>
        /// 建構 資料庫處理日誌 承載類別，並指定日誌日期時間
        /// </summary>
        /// <param name="now"></param>
        public LogTableEntity(DateTime logDateTime)
            : base()
        {
            this.LogDate  = Common.GetTWDate7(logDateTime);
            this.LogTime = logDateTime.ToString("HHmmss");
        }
        #endregion

        #region Property
        #region PKey
        #endregion

        #region Data
        private string _ReceiveType = null;
        /// <summary>
        /// 記錄日誌的代收類別代碼
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
        /// 群組角色 (1=行員 / 2=學校，請參考 RoleCodeTexts)
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
        #endregion
        #endregion

        #region Method
        #endregion
    }
}
