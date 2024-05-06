/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2019/02/24 15:37:01
*/

using System;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 異業代收款檔 資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class EDPDataEntity : Entity
    {
        public const string TABLE_NAME = "EDP_Data";

        #region Field Name Const Class
        /// <summary>
        /// EDPDataEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// PKey 流水號 (Identity)
            /// </summary>
            public const string SN = "SN";
            #endregion

            #region Data
            /// <summary>
            /// 異業管道代碼
            /// </summary>
            public const string EDPChannelId = "EDP_Channel_Id";

            /// <summary>
            /// 資料傳送日期
            /// </summary>
            public const string TranferDate = "Tranfer_Date";

            /// <summary>
            /// 異業管道入帳日
            /// </summary>
            public const string EDPAccountDate = "EDP_Account_Date";

            /// <summary>
            /// 代收門市店號
            /// </summary>
            public const string StoreId = "Store_Id";

            /// <summary>
            /// 虛擬帳號
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// 客戶繳費日期
            /// </summary>
            public const string ReceiveDate = "Receive_Date";

            /// <summary>
            /// 繳費金額
            /// </summary>
            public const string ReceiveAmount = "Receive_Amount";

            /// <summary>
            /// 入帳日 (會與實際入帳日晚一個營業日)
            /// </summary>
            public const string AccountDate = "Account_Date";

            /// <summary>
            /// 出帳來源
            /// </summary>
            public const string DSource = "DSOURCE";

            /// <summary>
            /// 客戶繳費時間
            /// </summary>
            public const string ReceiveTime = "Receive_Time";

            /// <summary>
            /// 原始資料
            /// </summary>
            public const string RowData = "Row_Data";

            /// <summary>
            /// 商家代號
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 代收管道代碼
            /// </summary>
            public const string ReceiveWay = "Receive_Way";

            /// <summary>
            /// 學生學號
            /// </summary>
            public const string StuId = "Stu_Id";

            /// <summary>
            /// 學生姓名
            /// </summary>
            public const string StuName = "Stu_Name";

            /// <summary>
            /// 資料建立日期時間
            /// </summary>
            public const string CrtDate = "crt_date";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// EDPDataEntity 類別建構式
        /// </summary>
        public EDPDataEntity()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        /// <summary>
        /// PKey 流水號 (Identity)
        /// </summary>
        [FieldSpec(Field.SN, true, FieldTypeEnum.Identity, false)]
        public Int64 SN
        {
            get;
            set;
        }
        #endregion

        #region Data
        private string _EDPChannelId = null;
        /// <summary>
        /// 異業管道代碼
        /// </summary>
        [FieldSpec(Field.EDPChannelId, false, FieldTypeEnum.VarChar, 8, false)]
        public string EDPChannelId
        {
            get
            {
                return _EDPChannelId;
            }
            set
            {
                _EDPChannelId = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 資料傳送日期
        /// </summary>
        [FieldSpec(Field.TranferDate, false, FieldTypeEnum.Date, false)]
        public DateTime TranferDate
        {
            get;
            set;
        }

        /// <summary>
        /// 異業管道入帳日
        /// </summary>
        [FieldSpec(Field.EDPAccountDate, false, FieldTypeEnum.Date, false)]
        public DateTime EDPAccountDate
        {
            get;
            set;
        }

        private string _StoreId = null;
        /// <summary>
        /// 代收門市店號
        /// </summary>
        [FieldSpec(Field.StoreId, false, FieldTypeEnum.VarChar, 8, false)]
        public string StoreId
        {
            get
            {
                return _StoreId;
            }
            set
            {
                _StoreId = value == null ? null : value.Trim();
            }
        }

        private string _CancelNo = null;
        /// <summary>
        /// 虛擬帳號
        /// </summary>
        [FieldSpec(Field.CancelNo, false, FieldTypeEnum.VarChar, 16, false)]
        public string CancelNo
        {
            get
            {
                return _CancelNo;
            }
            set
            {
                _CancelNo = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 客戶繳費日期
        /// </summary>
        [FieldSpec(Field.ReceiveDate, false, FieldTypeEnum.Date, false)]
        public DateTime ReceiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// 繳費金額
        /// </summary>
        [FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal ReceiveAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 入帳日 (會與實際入帳日晚一個營業日)
        /// </summary>
        [FieldSpec(Field.AccountDate, false, FieldTypeEnum.Date, false)]
        public DateTime AccountDate
        {
            get;
            set;
        }

        private string _DSource = null;
        /// <summary>
        /// 出帳來源
        /// </summary>
        [FieldSpec(Field.DSource, false, FieldTypeEnum.VarChar, 2, false)]
        public string DSource
        {
            get
            {
                return _DSource;
            }
            set
            {
                _DSource = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveTime = null;
        /// <summary>
        /// 客戶繳費時間
        /// </summary>
        [FieldSpec(Field.ReceiveTime, false, FieldTypeEnum.VarChar, 6, false)]
        public string ReceiveTime
        {
            get
            {
                return _ReceiveTime;
            }
            set
            {
                _ReceiveTime = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 原始資料
        /// </summary>
        [FieldSpec(Field.RowData, false, FieldTypeEnum.VarChar, 56, false)]
        public string RowData
        {
            get;
            set;
        }

        private string _ReceiveType = null;
        /// <summary>
        /// 商家代號
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

        private string _ReceiveWay = null;
        /// <summary>
        /// 代收管道代碼
        /// </summary>
        [FieldSpec(Field.ReceiveWay, false, FieldTypeEnum.VarChar, 4, false)]
        public string ReceiveWay
        {
            get
            {
                return _ReceiveWay;
            }
            set
            {
                _ReceiveWay = value == null ? null : value.Trim();
            }
        }

        private string _StuId = null;
        /// <summary>
        /// 學生學號
        /// </summary>
        [FieldSpec(Field.StuId, false, FieldTypeEnum.VarChar, 20, true)]
        public string StuId
        {
            get
            {
                return _StuId;
            }
            set
            {
                _StuId = value == null ? null : value.Trim();
            }
        }

        private string _StuName = null;
        /// <summary>
        /// 學生姓名
        /// </summary>
        [FieldSpec(Field.StuName, false, FieldTypeEnum.NVarChar, 60, true)]
        public string StuName
        {
            get
            {
                return _StuName;
            }
            set
            {
                _StuName = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 資料建立日期時間
        /// </summary>
        [FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
        public DateTime CrtDate
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
