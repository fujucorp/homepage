using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 網站日誌 (WebLogEntity) 的 view
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class WebLogView : Entity
    {
        #region VIEWSQL
        protected const string VIEWSQL = @"
SELECT [Task_No], [Log_Time], [Request_Id], [Request_Kind], [Client_IP], [Web_Machine]
     , [Index_Receive_Type], [Index_Cancel_No]
     , [Request_Desc], [Request_Args], [Status_Code]
     , [User_Unit_Kind], [User_Unit_Id], [User_Login_Id]
     , (CASE WHEN [User_Unit_Kind] = '1' THEN ISNULL((SELECT TOP 1 [BANKSNAME] FROM [BANK] WHERE [BANK].[BANKNO] = [Web_Log].[User_Unit_Id]), [User_Unit_Id])
             WHEN [User_Unit_Kind] = '2' THEN ISNULL((SELECT TOP 1 [Sch_Name] FROM [School_Rtype] WHERE [School_Rtype].[Sch_Identy] = [User_Unit_Id]), [User_Unit_Id])
             WHEN [User_Unit_Kind] = '3' THEN ISNULL((SELECT TOP 1 [Sch_Name] FROM [School_Rtype] WHERE [School_Rtype].Receive_Type = [User_Unit_Id]), [User_Unit_Id])
             ELSE '' END) AS [User_Unit_NAME]
  FROM [Web_Log]";
        #endregion

        #region Field Name Const Class
        /// <summary>
        /// WebLogView 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 任務編號
            /// </summary>
            public const string TaskNo = "Task_No";
            #endregion

            #region Data
            /// <summary>
            /// 日誌時間
            /// </summary>
            public const string LogTime = "Log_Time";

            /// <summary>
            /// Request 代碼 (功能代碼)
            /// </summary>
            public const string RequestId = "Request_Id";

            /// <summary>
            /// Request 類別 (A=新增; U=修改; D=刪除; S=查詢; E=處理，參考 LogTypeCodeTexts)
            /// </summary>
            public const string RequestKind = "Request_Kind";

            /// <summary>
            /// Client IP
            /// </summary>
            public const string ClientIp = "Client_IP";

            /// <summary>
            /// Web 主機名稱
            /// </summary>
            public const string WebMachine = "Web_Machine";

            /// <summary>
            /// 索引用商家代號
            /// </summary>
            public const string IndexReceiveType = "Index_Receive_Type";

            /// <summary>
            /// 索引用虛擬帳號
            /// </summary>
            public const string IndexCancelNo = "Index_Cancel_No";

            /// <summary>
            /// Request 說明
            /// </summary>
            public const string RequestDesc = "Request_Desc";

            /// <summary>
            /// Request 參數
            /// </summary>
            public const string RequestArgs = "Request_Args";

            /// <summary>
            /// 狀態代碼
            /// </summary>
            public const string StatusCode = "Status_Code";

            /// <summary>
            /// 使用者單位類別 (1:銀行使用者、2:學校使用者、3:學生使用者 請參考 UserQualCodeTexts)
            /// </summary>
            public const string UserUnitKind = "User_Unit_Kind";

            /// <summary>
            /// 使用者單位代碼 (UserUnitKind 為 1:分行代碼(6碼) / 2:學校代碼 / 3:商家代號)
            /// </summary>
            public const string UserUnitId = "User_Unit_Id";

            /// <summary>
            /// 使用者登入帳號 (UserUnitKind 為 1:行員工號 / 2:使用者帳號 / 3:學生學號)
            /// </summary>
            public const string UserLoginId = "User_Login_Id";

            /// <summary>
            /// 使用者單位名稱 (UserUnitKind 為 1:分行縮寫 / 2:學校名稱 / 3:學校名稱)
            /// </summary>
            public const string UserUnitName = "User_Unit_Name";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// UsersView 類別建構式
        /// </summary>
        public WebLogView()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _TaskNo = null;
        /// <summary>
        /// 任務編號
        /// </summary>
        [FieldSpec(Field.TaskNo, true, FieldTypeEnum.VarChar, 36, false)]
        public string TaskNo
        {
            get
            {
                return _TaskNo;
            }
            set
            {
                _TaskNo = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Data
        /// <summary>
        /// 日誌時間
        /// </summary>
        [FieldSpec(Field.LogTime, false, FieldTypeEnum.DateTime, false)]
        public DateTime LogTime
        {
            get;
            set;
        }

        private string _RequestId = null;
        /// <summary>
        /// Request 代碼 (功能代碼)
        /// </summary>
        [FieldSpec(Field.RequestId, false, FieldTypeEnum.VarChar, 20, false)]
        public string RequestId
        {
            get
            {
                return _RequestId;
            }
            set
            {
                _RequestId = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _RequestKind = null;
        /// <summary>
        /// Request 類別 (A=新增; U=修改; D=刪除; S=查詢; E=處理，參考 LogTypeCodeTexts)
        /// </summary>
        [FieldSpec(Field.RequestKind, false, FieldTypeEnum.VarChar, 2, false)]
        public string RequestKind
        {
            get
            {
                return _RequestKind;
            }
            set
            {
                _RequestKind = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _ClientIP = null;
        /// <summary>
        /// Client IP
        /// </summary>
        [FieldSpec(Field.ClientIp, false, FieldTypeEnum.VarChar, 40, false)]
        public string ClientIp
        {
            get
            {
                return _ClientIP;
            }
            set
            {
                _ClientIP = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _WebMachine = null;
        /// <summary>
        /// Web 主機名稱
        /// </summary>
        [FieldSpec(Field.WebMachine, false, FieldTypeEnum.VarChar, 20, false)]
        public string WebMachine
        {
            get
            {
                return _WebMachine;
            }
            set
            {
                _WebMachine = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _IndexReceiveType = null;
        /// <summary>
        /// 索引用商家代號
        /// </summary>
        [FieldSpec(Field.IndexReceiveType, false, FieldTypeEnum.VarChar, 6, true)]
        public string IndexReceiveType
        {
            get
            {
                return _IndexReceiveType;
            }
            set
            {
                _IndexReceiveType = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _IndexCancelNo = null;
        /// <summary>
        /// 索引用虛擬帳號
        /// </summary>
        [FieldSpec(Field.IndexCancelNo, false, FieldTypeEnum.VarChar, 16, true)]
        public string IndexCancelNo
        {
            get
            {
                return _IndexCancelNo;
            }
            set
            {
                _IndexCancelNo = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        /// <summary>
        /// Request 說明
        /// </summary>
        [FieldSpec(Field.RequestDesc, false, FieldTypeEnum.NVarChar, 100, false)]
        public string RequestDesc
        {
            get;
            set;
        }

        private string _RequestArgs = null;
        /// <summary>
        /// Request 參數
        /// </summary>
        [FieldSpec(Field.RequestArgs, false, FieldTypeEnum.NVarChar, 1000, false)]
        public string RequestArgs
        {
            get
            {
                return _RequestArgs;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    _RequestArgs = String.Empty;
                }
                else if (value.Length > 1000)
                {
                    _RequestArgs = value.Substring(0, 100);
                }
                else
                {
                    _RequestArgs = value;
                }
            }
        }

        private string _StatusCode = String.Empty;
        /// <summary>
        /// 狀態代碼
        /// </summary>
        [FieldSpec(Field.StatusCode, false, FieldTypeEnum.VarChar, 5, false)]
        public string StatusCode
        {
            get
            {
                return _StatusCode;
            }
            set
            {
                _StatusCode = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _UserUnitKind = null;
        /// <summary>
        /// 使用者單位類別 (1:銀行使用者、2:學校使用者、3:學生使用者 請參考 UserQualCodeTexts)
        /// </summary>
        [FieldSpec(Field.UserUnitKind, false, FieldTypeEnum.Char, 1, true)]
        public string UserUnitKind
        {
            get
            {
                return _UserUnitKind;
            }
            set
            {
                _UserUnitKind = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _UserUnitId = null;
        /// <summary>
        /// 使用者單位代碼 (UserUnitKind 為 1:分行代碼(6碼) / 2:學校代碼 / 3:商家代號)
        /// </summary>
        [FieldSpec(Field.UserUnitId, false, FieldTypeEnum.VarChar, 10, true)]
        public string UserUnitId
        {
            get
            {
                return _UserUnitId;
            }
            set
            {
                _UserUnitId = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _UserLoginId = null;
        /// <summary>
        /// 使用者登入帳號 (UserUnitKind 為 1:行員工號 / 2:使用者帳號 / 3:學生學號)
        /// </summary>
        [FieldSpec(Field.UserLoginId, false, FieldTypeEnum.VarChar, 20, true)]
        public string UserLoginId
        {
            get
            {
                return _UserLoginId;
            }
            set
            {
                _UserLoginId = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _UserUnitName = null;
        /// <summary>
        /// 使用者單位類別 (1:銀行使用者、2:學校使用者、3:學生使用者 請參考 UserQualCodeTexts)
        /// </summary>
        [FieldSpec(Field.UserUnitKind, false, FieldTypeEnum.NVarChar, 60, true)]
        public string UserUnitName
        {
            get
            {
                return _UserUnitName;
            }
            set
            {
                _UserUnitName = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }
        #endregion
        #endregion
    }
}
