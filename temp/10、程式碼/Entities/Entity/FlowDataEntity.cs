using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 流程資料 承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class FlowDataEntity : Entity
    {
        public const string TABLE_NAME = "FlowData";

        #region Field Name Const Class
        /// <summary>
        /// FlowDataEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 流程 Guid
            /// </summary>
            public const string Guid = "Guid";
            #endregion

            #region Data
            #region 表單相關資料
            /// <summary>
            /// 表單 (待辦事項) 代碼 (請參考 FormCodeTexts)
            /// </summary>
            public const string FormId = "Form_Id";

            /// <summary>
            /// 表單描述
            /// </summary>
            public const string FormDesc = "Form_Desc";

            /// <summary>
            /// 表單資料 (Xml)
            /// </summary>
            public const string FormData = "Form_Data";
            #endregion

            #region 申請者相關資料
            /// <summary>
            /// 申請種類 (I=新增 U=更新 D=刪除) (請參考 ApplyKindCodeTexts)
            /// </summary>
            public const string ApplyKind = "Apply_Kind";

            /// <summary>
            /// 申請者身分別代碼 (請參考 UserQualCodeTexts)
            /// </summary>
            public const string ApplyUserQual = "Apply_User_Qual";

            /// <summary>
            /// 申請者單位代碼
            /// </summary>
            public const string ApplyUnitId = "Apply_Unit_Id";

            /// <summary>
            /// 申請者代碼
            /// </summary>
            public const string ApplyUserId = "Apply_User_Id";

            /// <summary>
            /// 申請者名稱
            /// </summary>
            public const string ApplyUserName = "Apply_User_Name";

            /// <summary>
            /// 申請日期時間
            /// </summary>
            public const string ApplyDate = "Apply_Date";
            #endregion

            #region 資料索引相關資料
            /// <summary>
            /// 資料索引的單位代碼 (Role等於1時存放分行代號6碼; Role等於2時存放學校代碼)
            /// </summary>
            public const string DataUnitId = "Data_Unit_Id";

            /// <summary>
            /// 資料索引的角色 (1=行員; 2=學校，請參考 RoleCodeTexts)
            /// </summary>
            public const string DataRole = "Data_Role";

            /// <summary>
            /// 資料索引的權限角色 (2=經辦; 3=主管，請參考 RoleTypeCodeTexts)
            /// </summary>
            public const string DataRoleType = "Data_Role_Type";

            /// <summary>
            /// 資料索引的鍵值
            /// </summary>
            public const string DataKey = "Data_Key";

            /// <summary>
            /// 資料索引的商家代號
            /// </summary>
            public const string DataReceiveType = "Data_Receive_Type";

            /// <summary>
            /// 資料索引的學年代碼
            /// </summary>
            public const string DataYearId = "Data_Year_Id";

            /// <summary>
            /// 資料索引的學期代碼
            /// </summary>
            public const string DataTermId = "Data_Term_Id";

            /// <summary>
            /// 資料索引的部別代碼
            /// </summary>
            public const string DataDepId = "Data_Dep_Id";

            /// <summary>
            /// 資料索引的代收費用別代碼
            /// </summary>
            public const string DataReceiveId = "Data_Receive_Id";
            #endregion

            #region 處理者相關資料
            /// <summary>
            /// 處理種類 (A=放行 R=駁回) (請參考 ProcessKindCodeTexts)
            /// </summary>
            public const string ProcessKind = "Process_Kind";

            /// <summary>
            /// 處理備註
            /// </summary>
            public const string ProcessMemo = "Process_Memo";

            /// <summary>
            /// 處理者身分別代碼 (請參考 UserQualCodeTexts)
            /// </summary>
            public const string ProcessUserQual = "Process_User_Qual";

            /// <summary>
            /// 處理者單位代碼
            /// </summary>
            public const string ProcessUnitId = "Process_Unit_Id";

            /// <summary>
            /// 處理者代碼
            /// </summary>
            public const string ProcessUserId = "Process_User_id";

            /// <summary>
            /// 處理者名稱
            /// </summary>
            public const string ProcessUserName = "Process_User_Name";

            /// <summary>
            /// 處理日期時間
            /// </summary>
            public const string ProcessDate = "Process_Date";
            #endregion

            /// <summary>
            /// 流程狀態代碼 (0=待覆核 1=已覆核 2=處理中) (請參考 FlowStatusCodeTexts)
            /// </summary>
            public const string Status = "Status";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// FlowDataEntity 類別建構式
        /// </summary>
        public FlowDataEntity()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _Guid = null;
        /// <summary>
        /// 流程 Guid
        /// </summary>
        [FieldSpec(Field.Guid, true, FieldTypeEnum.VarChar, 32, false)]
        public string Guid
        {
            get
            {
                return _Guid;
            }
            set
            {
                _Guid = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Data
        #region 表單 (待辦事項) 相關資料
        private string _FormId = null;
        /// <summary>
        /// 表單 (待辦事項) 代碼 (請參考 FormCodeTexts)
        /// </summary>
        [FieldSpec(Field.FormId, false, FieldTypeEnum.VarChar, 20, false)]
        public string FormId
        {
            get
            {
                return _FormId;
            }
            set
            {
                _FormId = value == null ? null : value.Trim();
            }
        }

        private string _FormDesc = null;
        /// <summary>
        /// 表單備註
        /// </summary>
        [FieldSpec(Field.FormDesc, false, FieldTypeEnum.NVarChar, 100, false)]
        public string FormDesc
        {
            get
            {
                return _FormDesc;
            }
            set
            {
                _FormDesc = value == null ? null : value.Trim();
            }
        }

        private string _FormData = null;
        /// <summary>
        /// 表單資料 (Xml)
        /// </summary>
        [FieldSpec(Field.FormData, false, FieldTypeEnum.NVarCharMax, false)]
        public string FormData
        {
            get
            {
                return _FormData;
            }
            set
            {
                _FormData = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 申請者相關資料
        private string _ApplyKind = null;
        /// <summary>
        /// 申請種類 (I=新增 U=更新 D=刪除) (請參考 ApplyKindCodeTexts)
        /// </summary>
        [FieldSpec(Field.ApplyKind, false, FieldTypeEnum.VarChar, 5, false)]
        public string ApplyKind
        {
            get
            {
                return _ApplyKind;
            }
            set
            {
                _ApplyKind = value == null ? null : value.Trim();
            }
        }

        private string _UserQual = null;
        /// <summary>
        /// 申請者身分別代碼 (請參考 UserQualCodeTexts)
        /// </summary>
        [FieldSpec(Field.ApplyUserQual, false, FieldTypeEnum.Char, 1, false)]
        public string ApplyUserQual
        {
            get
            {
                return _UserQual;
            }
            set
            {
                _UserQual = value == null ? null : value.Trim();
            }
        }

        private string _ApplyUnitId = null;
        /// <summary>
        /// 申請者單位代碼
        /// </summary>
        [FieldSpec(Field.ApplyUnitId, false, FieldTypeEnum.VarChar, 10, false)]
        public string ApplyUnitId
        {
            get
            {
                return _ApplyUnitId;
            }
            set
            {
                _ApplyUnitId = value == null ? null : value.Trim();
            }
        }

        private string _ApplyUserId = null;
        /// <summary>
        /// 申請者代碼
        /// </summary>
        [FieldSpec(Field.ApplyUserId, false, FieldTypeEnum.VarChar, 30, false)]
        public string ApplyUserId
        {
            get
            {
                return _ApplyUserId;
            }
            set
            {
                _ApplyUserId = value == null ? null : value.Trim();
            }
        }

        private string _ApplyUserName = String.Empty;
        /// <summary>
        /// 申請者名稱
        /// </summary>
        [FieldSpec(Field.ApplyUserName, false, FieldTypeEnum.NVarChar, 40, false)]
        public string ApplyUserName
        {
            get
            {
                return _ApplyUserName;
            }
            set
            {
                _ApplyUserName = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 申請日期時間
        /// </summary>
        [FieldSpec(Field.ApplyDate, false, FieldTypeEnum.DateTime, false)]
        public DateTime ApplyDate
        {
            get;
            set;
        }
        #endregion

        #region 資料索引相關資料
        private string _DataKey = String.Empty;
        /// <summary>
        /// 資料索引的鍵值
        /// </summary>
        [FieldSpec(Field.DataKey, false, FieldTypeEnum.VarChar, 50, false)]
        public string DataKey
        {
            get
            {
                return _DataKey;
            }
            set
            {
                _DataKey = value == null ? String.Empty : value.Trim();
            }
        }

        private string _DataUnitId = String.Empty;
        /// <summary>
        /// 資料索引的單位代碼 (Role等於1時存放分行代號6碼; Role等於2時存放學校代碼)
        /// </summary>
        [FieldSpec(Field.DataUnitId, false, FieldTypeEnum.VarChar, 10, true)]
        public string DataUnitId
        {
            get
            {
                return _DataUnitId;
            }
            set
            {
                _DataUnitId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _DataRole = String.Empty;
        /// <summary>
        /// 資料索引的角色 (1=行員; 2=學校，請參考 RoleCodeTexts)
        /// </summary>
        [FieldSpec(Field.DataRole, false, FieldTypeEnum.Char, 1, true)]
        public string DataRole
        {
            get
            {
                return _DataRole;
            }
            set
            {
                _DataRole = value == null ? String.Empty : value.Trim();
            }
        }

        private string _DataRoleType = String.Empty;
        /// <summary>
        /// 資料索引的權限角色 (2=經辦; 3=主管，請參考 RoleTypeCodeTexts)
        /// </summary>
        [FieldSpec(Field.DataRoleType, false, FieldTypeEnum.Char, 1, true)]
        public string DataRoleType
        {
            get
            {
                return _DataRoleType;
            }
            set
            {
                _DataRoleType = value == null ? String.Empty : value.Trim();
            }
        }

        private string _DataReceiveType = String.Empty;
        /// <summary>
        /// 資料索引的商家代號
        /// </summary>
        [FieldSpec(Field.DataReceiveType, false, FieldTypeEnum.VarChar, 6, true)]
        public string DataReceiveType
        {
            get
            {
                return _DataReceiveType;
            }
            set
            {
                _DataReceiveType = value == null ? String.Empty : value.Trim();
            }
        }

        private string _DataYearId = String.Empty;
        /// <summary>
        /// 資料索引的學年代碼
        /// </summary>
        [FieldSpec(Field.DataYearId, false, FieldTypeEnum.VarChar, 4, true)]
        public string DataYearId
        {
            get
            {
                return _DataYearId;
            }
            set
            {
                _DataYearId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _DataTermId = String.Empty;
        /// <summary>
        /// 資料索引的學期代碼
        /// </summary>
        [FieldSpec(Field.DataTermId, false, FieldTypeEnum.VarChar, 4, true)]
        public string DataTermId
        {
            get
            {
                return _DataTermId;
            }
            set
            {
                _DataTermId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _DataDepId = String.Empty;
        /// <summary>
        /// 資料索引的部別代碼
        /// </summary>
        [FieldSpec(Field.DataDepId, false, FieldTypeEnum.VarChar, 4, true)]
        public string DataDepId
        {
            get
            {
                return _DataDepId;
            }
            set
            {
                _DataDepId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _DataReceiveId = String.Empty;
        /// <summary>
        /// 資料索引的代收費用別代碼
        /// </summary>
        [FieldSpec(Field.DataReceiveId, false, FieldTypeEnum.VarChar, 4, true)]
        public string DataReceiveId
        {
            get
            {
                return _DataReceiveId;
            }
            set
            {
                _DataReceiveId = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        #region 處理者相關資料
        private string _ProcessKind = String.Empty;
        /// <summary>
        /// 處理種類 (A=放行 R=駁回) (請參考 ProcessKindCodeTexts)
        /// </summary>
        [FieldSpec(Field.ProcessKind, false, FieldTypeEnum.VarChar, 5, true)]
        public string ProcessKind
        {
            get
            {
                return _ProcessKind;
            }
            set
            {
                _ProcessKind = value == null ? String.Empty : value.Trim();
            }
        }

        private string _ProcessMemo = String.Empty;
        /// <summary>
        /// 處理備註
        /// </summary>
        [FieldSpec(Field.ProcessMemo, false, FieldTypeEnum.NVarChar, 100, true)]
        public string ProcessMemo
        {
            get
            {
                return _ProcessMemo;
            }
            set
            {
                _ProcessMemo = value == null ? String.Empty : value.Trim();
            }
        }

        private string _ProcessUserQual = String.Empty;
        /// <summary>
        /// 處理者身分別代碼 (請參考 UserQualCodeTexts)
        /// </summary>
        [FieldSpec(Field.ProcessUserQual, false, FieldTypeEnum.Char, 1, true)]
        public string ProcessUserQual
        {
            get
            {
                return _ProcessUserQual;
            }
            set
            {
                _ProcessUserQual = value == null ? String.Empty : value.Trim();
            }
        }

        private string _ProcessUnitId = String.Empty;
        /// <summary>
        /// 處理者單位代碼
        /// </summary>
        [FieldSpec(Field.ProcessUnitId, false, FieldTypeEnum.VarChar, 10, true)]
        public string ProcessUnitId
        {
            get
            {
                return _ProcessUnitId;
            }
            set
            {
                _ProcessUnitId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _ProcessUserId = String.Empty;
        /// <summary>
        /// 處理者代碼
        /// </summary>
        [FieldSpec(Field.ProcessUserId, false, FieldTypeEnum.VarChar, 30, true)]
        public string ProcessUserId
        {
            get
            {
                return _ProcessUserId;
            }
            set
            {
                _ProcessUserId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _ProcessUserName = String.Empty;
        /// <summary>
        /// 處理者名稱
        /// </summary>
        [FieldSpec(Field.ProcessUserName, false, FieldTypeEnum.NVarChar, 40, true)]
        public string ProcessUserName
        {
            get
            {
                return _ProcessUserName;
            }
            set
            {
                _ProcessUserName = value == null ? String.Empty : value.Trim();
            }
        }

        private string _ProcessDate = String.Empty;
        /// <summary>
        /// 處理日期時間
        /// </summary>
        [FieldSpec(Field.ProcessDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? ProcessDate
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 流程狀態代碼 (0=待覆核 1=已覆核 2=處理中) (請參考 FlowStatusCodeTexts)
        /// </summary>
        [FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 5, false)]
        public string Status
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Readonly Property
        /// <summary>
        /// 取得表單 (待辦事項) 名稱
        /// </summary>
        [XmlIgnore]
        public string FormName
        {
            get
            {
                return FormCodeTexts.GetText(this.FormId);
            }
        }

        /// <summary>
        /// 取得申請種類名稱
        /// </summary>
        [XmlIgnore]
        public string ApplyKindName
        {
            get
            {
                return ApplyKindCodeTexts.GetText(this.ApplyKind);
            }
        }
        #endregion
    }
}
