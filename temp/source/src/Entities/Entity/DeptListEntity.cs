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
    /// 土銀專用的部別資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class DeptListEntity : Entity
    {
        public const string TABLE_NAME = "Dept_List";

        #region Field Name Const Class
        /// <summary>
        /// 部別資料 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 代收類別代碼 欄位名稱常數定義
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 學年代碼 欄位名稱常數定義
            /// </summary>
            public const string YearId = "Year_Id";

            /// <summary>
            /// 學期代碼 欄位名稱常數定義
            /// </summary>
            public const string TermId = "Term_Id";

            /// <summary>
            /// 部別代碼 欄位名稱常數定義
            /// </summary>
            public const string DeptId = "Dept_Id";
            #endregion

            #region Data
            /// <summary>
            /// 部別名稱 欄位名稱常數定義
            /// </summary>
            public const string DeptName = "Dept_Name";

            #region [MDY:20220808] 2022擴充案 部別英文名稱 新增欄位
            /// <summary>
            /// 部別英文名稱
            /// </summary>
            public const string DeptEName = "Dept_EName";
            #endregion
            #endregion

            #region 狀態相關欄位
            /// <summary>
            /// 資料狀態 (0=正常 / D=停用) (請參考 DataStatusCodeTexts) 欄位名稱常數定義
            /// </summary>
            public const string Status = "status";

            /// <summary>
            /// 資料建立日期 (含時間) 欄位名稱常數定義
            /// </summary>
            public const string CrtDate = "crt_date";

            /// <summary>
            /// 資料建立者。暫時儲存使用者帳號 (UserId) 欄位名稱常數定義
            /// </summary>
            public const string CrtUser = "crt_user";

            /// <summary>
            /// 資料最後修改日期 (含時間) 欄位名稱常數定義
            /// </summary>
            public const string MdyDate = "mdy_date";

            /// <summary>
            /// 資料最後修改者。暫時儲存使用者帳號 (UserId) 欄位名稱常數定義
            /// </summary>
            public const string MdyUser = "mdy_user";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構土銀專用的部別資料承載類別
        /// </summary>
        public DeptListEntity()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _ReceiveType = null;
        /// <summary>
        /// 代收類別代碼
        /// </summary>
        [FieldSpec(Field.ReceiveType, true, FieldTypeEnum.VarChar, 6, false)]
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

        private string _YearId = null;
        /// <summary>
        /// 學年代碼
        /// </summary>
        [FieldSpec(Field.YearId, true, FieldTypeEnum.Char, 3, false)]
        public string YearId
        {
            get
            {
                return _YearId;
            }
            set
            {
                _YearId = value == null ? null : value.Trim();
            }
        }

        private string _TermId = null;
        /// <summary>
        /// 學期代碼
        /// </summary>
        [FieldSpec(Field.TermId, true, FieldTypeEnum.Char, 1, false)]
        public string TermId
        {
            get
            {
                return _TermId;
            }
            set
            {
                _TermId = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 部別代碼型別調整為 NVarChar(140)
        private string _DeptId = null;
        /// <summary>
        /// 部別代碼
        /// </summary>
        [FieldSpec(Field.DeptId, true, FieldTypeEnum.NVarChar, 140, false)]
        public string DeptId
        {
            get
            {
                return _DeptId;
            }
            set
            {
                _DeptId = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion

        #region Data
        #region [MDY:20220808] 2022擴充案 部別名稱型別調整為 NVarChar(140)
        /// <summary>
        /// 部別名稱
        /// </summary>
        [FieldSpec(Field.DeptName, false, FieldTypeEnum.NVarChar, 140, true)]
        public string DeptName
        {
            get;
            set;
        }
        #endregion

        #region [MDY:20220808] 2022擴充案 部別英文名稱 新增欄位並調整為 NVarChar(140)
        /// <summary>
        /// 部別英文名稱
        /// </summary>
        [FieldSpec(Field.DeptEName, false, FieldTypeEnum.NVarChar, 140, true)]
        public string DeptEName
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region 狀態相關欄位
        /// <summary>
        /// 資料狀態 (0=正常 / D=停用) (請參考 DataStatusCodeTexts)
        /// </summary>
        [FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// 資料建立日期 (含時間)
        /// </summary>
        [FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
        public DateTime CrtDate
        {
            get;
            set;
        }

        /// <summary>
        /// 資料建立者。暫時儲存使用者帳號 (UserId)
        /// </summary>
        [FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
        public string CrtUser
        {
            get;
            set;
        }

        /// <summary>
        /// 資料最後修改日期 (含時間)
        /// </summary>
        [FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? MdyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 資料最後修改者。暫時儲存使用者帳號 (UserId)
        /// </summary>
        [FieldSpec(Field.MdyUser, false, FieldTypeEnum.VarChar, 20, true)]
        public string MdyUser
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
