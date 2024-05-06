using System;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 異業代收款檔 部份欄位 View 資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class EDPDataView : Entity
    {
        #region VIEWSQL
        protected const string VIEWSQL = @"
SELECT SN, EDP_Channel_Id, Tranfer_Date, EDP_Account_Date, Cancel_No, Receive_Amount, Receive_Date, Receive_Time, Account_Date, Receive_Type, Receive_Way, Stu_Name
  FROM " + EDPDataEntity.TABLE_NAME;
        #endregion

        #region Field Name Const Class
        /// <summary>
        /// EDPDataView 欄位名稱定義抽象類別
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
            /// 資料傳送日期 (中信帳務處理日)
            /// </summary>
            public const string TranferDate = "Tranfer_Date";

            /// <summary>
            /// 異業管道入帳日
            /// </summary>
            public const string EDPAccountDate = "EDP_Account_Date";

            /// <summary>
            /// 虛擬帳號
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// 繳費金額
            /// </summary>
            public const string ReceiveAmount = "Receive_Amount";

            /// <summary>
            /// 客戶繳費日期
            /// </summary>
            public const string ReceiveDate = "Receive_Date";

            /// <summary>
            /// 客戶繳費時間
            /// </summary>
            public const string ReceiveTime = "Receive_Time";

            /// <summary>
            /// 入帳日 (會與實際入帳日晚一個營業日)
            /// </summary>
            public const string AccountDate = "Account_Date";

            /// <summary>
            /// 商家代號
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 代收管道代碼
            /// </summary>
            public const string ReceiveWay = "Receive_Way";

            /// <summary>
            /// 學生姓名
            /// </summary>
            public const string StuName = "Stu_Name";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 異業代收款檔 (EDP_Data) View 的資料承載類別
        /// </summary>
        public EDPDataView()
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
        /// 資料傳送日期 (中信帳務處理日)
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
        /// 繳費金額
        /// </summary>
        [FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal ReceiveAmount
        {
            get;
            set;
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
        /// 入帳日 (會與實際入帳日晚一個營業日)
        /// </summary>
        [FieldSpec(Field.AccountDate, false, FieldTypeEnum.Date, false)]
        public DateTime AccountDate
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
        #endregion
        #endregion

        #region Readonly Property
        /// <summary>
        /// 客戶繳費日期時間 (ReceiveDate + ReceiveTime) (中信交易繳款日期)
        /// </summary>
        public DateTime ReceiveDateTime
        {
            get
            {
                TimeSpan time;
                if (this.ReceiveTime.Length == 6 && TimeSpan.TryParse(this.ReceiveTime.Insert(4, ":").Insert(2, ":"), out time))
                {
                    DateTime date = this.ReceiveDate + time;
                    return date;
                }
                else
                {
                    return this.ReceiveDate;
                }
            }
        }
        #endregion
    }
}
