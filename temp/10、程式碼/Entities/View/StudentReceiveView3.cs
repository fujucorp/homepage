using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 查詢學生繳費資料用的 View 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public class StudentReceiveView3 : Entity
    {
        #region [Old]
//        protected const string VIEWSQL = @"SELECT Receive_Type, Year_Id, Term_Id, Dep_Id, Receive_Id, Stu_Id
//     , ISNULL((SELECT TOP 1 Stu_Name FROM Student_Master AS SM WHERE SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id), '') AS Stu_Name
//     , ISNULL(Cancel_No, '') AS Cancel_No, ISNULL(Receive_Amount, 0) AS Receive_Amount
//     , isnull(receive_way,'') as receive_way,isnull(receive_date,'') receive_date,isnull(account_date,'') account_date
//     , ISNULL(Deduct_BankId, '') AS Deduct_BankId, ISNULL(Deduct_AccountNo, '') AS Deduct_AccountNo, ISNULL(Deduct_AccountName, '') AS Deduct_AccountName, ISNULL(Deduct_AccountId, '') AS Deduct_AccountId
//  FROM Student_Receive AS SR
// WHERE (Receive_Way IS NULL OR Receive_Way = '') AND (Receive_Date IS NULL OR Receive_Date = '')
//   AND (Deduct_BankId IS NOT NULL OR Deduct_BankId = '') AND (Deduct_AccountNo IS NOT NULL OR Deduct_AccountNo = '')";
        #endregion

        #region [Old]
//        protected const string VIEWSQL = @"SELECT Receive_Type, Year_Id, Term_Id, Dep_Id, Receive_Id, Stu_Id
//     , ISNULL((SELECT TOP 1 Stu_Name FROM Student_Master AS SM WHERE SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id), '') AS Stu_Name
//     , ISNULL(Cancel_No, '') AS Cancel_No, ISNULL(Receive_Amount, 0) AS Receive_Amount
//     , ISNULL(Receive_Way,'') AS Receive_Way, ISNULL(Receive_Date,'') AS Receive_Date, ISNULL(Account_Date,'') AS Account_Date
//     , ISNULL((SELECT [Bill_Valid_Date] FROM [School_Rid]     AS SRI WHERE SRI.[Receive_Type] = SR.[Receive_Type] AND SRI.[Year_Id] = SR.[Year_Id] AND SRI.[Term_Id] = SR.[Term_Id] AND SRI.[Dep_Id] = SR.[Dep_Id] AND SRI.[Receive_Id] = SR.[Receive_Id]), '') AS [Bill_Valid_Date]
//     , ISNULL((SELECT [Bill_Close_Date] FROM [School_Rid]     AS SRI WHERE SRI.[Receive_Type] = SR.[Receive_Type] AND SRI.[Year_Id] = SR.[Year_Id] AND SRI.[Term_Id] = SR.[Term_Id] AND SRI.[Dep_Id] = SR.[Dep_Id] AND SRI.[Receive_Id] = SR.[Receive_Id]), '') AS [Bill_Close_Date]
//  FROM Student_Receive AS SR";
        #endregion

        #region [Old]
//        protected const string VIEWSQL = @"SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Stu_Id]
//     , ISNULL((SELECT TOP 1 [Stu_Name] FROM [" + StudentMasterEntity.TABLE_NAME + @"] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
//     , ISNULL(SR.[Cancel_No], '') AS [Cancel_No], ISNULL(SR.[Receive_Amount], 0) AS [Receive_Amount]
//     , ISNULL(SR.[Receive_Way],'') AS [Receive_Way], ISNULL(SR.[Receive_Date],'') AS [Receive_Date], ISNULL(SR.[Account_Date],'') AS [Account_Date]
//	 , ISNULL(RI.[Bill_Valid_Date], '') AS [Bill_Valid_Date], ISNULL(RI.[Bill_Close_Date], '') AS [Bill_Close_Date]
//	 , ISNULL(RI.[Pay_Date], '') AS [Pay_Due_Date], ISNULL(RI.[Pay_Due_Date2], '') AS [Pay_Due_Date2], ISNULL(RI.[Pay_Due_Date3], '') AS [Pay_Due_Date3]
//  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS SR
//  JOIN [" + SchoolRidEntity.TABLE_NAME + @"] AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]";
        #endregion

        #region [MDY:20160131] StudentReceive 增加繳款期限，如果沒值才取 SchoolRid 的繳款期限
        #region [Old]
//        protected const string VIEWSQL = @"SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Stu_Id], SR.[Old_Seq]
//     , ISNULL((SELECT TOP 1 [Stu_Name] FROM [" + StudentMasterEntity.TABLE_NAME + @"] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
//     , ISNULL(SR.[Cancel_No], '') AS [Cancel_No], ISNULL(SR.[Receive_Amount], 0) AS [Receive_Amount]
//     , ISNULL(SR.[Receive_Way],'') AS [Receive_Way], ISNULL(SR.[Receive_Date],'') AS [Receive_Date], ISNULL(SR.[Account_Date],'') AS [Account_Date]
//     , ISNULL(RI.[Bill_Valid_Date], '') AS [Bill_Valid_Date], ISNULL(RI.[Bill_Close_Date], '') AS [Bill_Close_Date]
//     , ISNULL(RI.[Pay_Date], '') AS [Pay_Due_Date], ISNULL(RI.[Pay_Due_Date2], '') AS [Pay_Due_Date2], ISNULL(RI.[Pay_Due_Date3], '') AS [Pay_Due_Date3]
//  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS SR
//  JOIN [" + SchoolRidEntity.TABLE_NAME + @"] AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]";
        #endregion

        protected const string VIEWSQL = @"SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Stu_Id], SR.[Old_Seq]
     , ISNULL((SELECT TOP 1 [Stu_Name] FROM [" + StudentMasterEntity.TABLE_NAME + @"] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
     , ISNULL(SR.[Cancel_No], '') AS [Cancel_No], ISNULL(SR.[Receive_Amount], 0) AS [Receive_Amount]
     , ISNULL(SR.[Receive_Way],'') AS [Receive_Way], ISNULL(SR.[Receive_Date],'') AS [Receive_Date], ISNULL(SR.[Account_Date],'') AS [Account_Date]
     , ISNULL(RI.[Bill_Valid_Date], '') AS [Bill_Valid_Date], ISNULL(RI.[Bill_Close_Date], '') AS [Bill_Close_Date]
     , CASE WHEN SR.[Pay_Due_Date] IS NULL OR SR.Pay_Due_Date = '' THEN ISNULL(RI.[Pay_Date], '') ELSE SR.[Pay_Due_Date] END AS [Pay_Due_Date]
     , ISNULL(RI.[Pay_Due_Date2], '') AS [Pay_Due_Date2], ISNULL(RI.[Pay_Due_Date3], '') AS [Pay_Due_Date3]
  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS SR
  JOIN [" + SchoolRidEntity.TABLE_NAME + @"] AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]";
        #endregion

        #region Field Name Const Class
        /// <summary>
        /// 查詢學生繳費資料用的 View 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 代收類別代碼
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 學年代碼
            /// </summary>
            public const string YearId = "Year_Id";

            /// <summary>
            /// 學期代碼
            /// </summary>
            public const string TermId = "Term_Id";

            /// <summary>
            /// 部別代碼
            /// </summary>
            public const string DepId = "Dep_Id";

            /// <summary>
            /// 代收費用別代碼
            /// </summary>
            public const string ReceiveId = "Receive_Id";

            /// <summary>
            /// 學號
            /// </summary>
            public const string StuId = "Stu_Id";

            /// <summary>
            /// 舊資料序號
            /// </summary>
            public const string OldSeq = "Old_Seq";
            #endregion

            #region Data
            /// <summary>
            /// 學生姓名
            /// </summary>
            public const string StuName = "Stu_Name";

            /// <summary>
            /// 虛擬帳號
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// 繳費金額
            /// </summary>
            public const string ReceiveAmount = "Receive_Amount";

            /// <summary>
            /// 代收管道
            /// </summary>
            public const string ReceiveWay = "Receive_Way";

            /// <summary>
            /// 代收日 (民國年 yyymmdd)
            /// </summary>
            public const string ReceiveDate = "Receive_Date";

            /// <summary>
            /// 入帳日 (民國年 yyymmdd)
            /// </summary>
            public const string AccountDate = "Account_Date";

            /// <summary>
            /// 開放列印日期 (西元年 yyyyMMdd)
            /// </summary>
            public const string BillOpenDate = "Bill_Valid_Date";

            /// <summary>
            /// 關閉列印日期 (西元年 yyyyMMdd)
            /// </summary>
            public const string BillCloseDate = "Bill_Close_Date";

            /// <summary>
            /// 繳費期限 (民國年 yyyMMdd) (預設為 StudentReceive.PayDueDate，無值則為 SchoolRid.PayDate)
            /// </summary>
            public const string PayDueDate = "Pay_Due_Date";

            /// <summary>
            /// 繳費期限2 (民國年 yyyMMdd) (土銀用來存放信用卡繳費期限)
            /// </summary>
            public const string PayDueDate2 = "Pay_Due_Date2";

            /// <summary>
            /// 繳費期限3 (民國年 yyyMMdd) (土銀用來存放財金繳費期限)
            /// </summary>
            public const string PayDueDate3 = "Pay_Due_Date3";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 查詢學生繳費資料用的 View 的資料承載類別
        /// </summary>
        public StudentReceiveView3()
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

        private string _DepId = null;
        /// <summary>
        /// 部別代碼
        /// </summary>
        [FieldSpec(Field.DepId, true, FieldTypeEnum.Char, 1, false)]
        public string DepId
        {
            get
            {
                return _DepId;
            }
            set
            {
                _DepId = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveId = null;
        /// <summary>
        /// 代收費用別代碼
        /// </summary>
        [FieldSpec(Field.ReceiveId, true, FieldTypeEnum.Char, 1, false)]
        public string ReceiveId
        {
            get
            {
                return _ReceiveId;
            }
            set
            {
                _ReceiveId = value == null ? null : value.Trim();
            }
        }

        private string _StuId = null;
        /// <summary>
        /// 學號
        /// </summary>
        [FieldSpec(Field.StuId, true, FieldTypeEnum.VarChar, 20, false)]
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

        private int _OldSeq = 0;
        /// <summary>
        /// 舊資料序號
        /// </summary>
        [FieldSpec(Field.OldSeq, true, FieldTypeEnum.Integer, false)]
        public int OldSeq
        {
            get
            {
                return _OldSeq;
            }
            set
            {
                _OldSeq = value < 0 ? 0 : value;
            }
        }
        #endregion

        #region Data
        /// <summary>
        /// 學生姓名
        /// </summary>
        [FieldSpec(Field.StuName, false, FieldTypeEnum.NVarChar, 60, false)]
        public string StuName
        {
            get;
            set;
        }

        /// <summary>
        /// 虛擬帳號
        /// </summary>
        [FieldSpec(Field.CancelNo, false, FieldTypeEnum.Char, 16, false)]
        public string CancelNo
        {
            get;
            set;
        }

        /// <summary>
        /// 繳費金額
        /// </summary>
        [FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal? ReceiveAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 代收管道
        /// </summary>
        [FieldSpec(Field.ReceiveWay, false, FieldTypeEnum.VarChar, 4, false)]
        public string ReceiveWay
        {
            get;
            set;
        }

        /// <summary>
        /// 代收日 (民國年 yyymmdd)
        /// </summary>
        [FieldSpec(Field.ReceiveDate, false, FieldTypeEnum.Char, 7, false)]
        public string ReceiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// 入帳日 (民國年 yyymmdd)
        /// </summary>
        [FieldSpec(Field.AccountDate, false, FieldTypeEnum.Char, 7, false)]
        public string AccountDate
        {
            get;
            set;
        }

        /// <summary>
        /// 開放列印日期 (西元年 yyyyMMdd)
        /// </summary>
        [FieldSpec(Field.BillOpenDate, false, FieldTypeEnum.Char, 8, true)]
        public string BillOpenDate
        {
            get;
            set;
        }

        /// <summary>
        /// 關閉列印日期 (西元年 yyyyMMdd)
        /// </summary>
        [FieldSpec(Field.BillCloseDate, false, FieldTypeEnum.Char, 8, false)]
        public string BillCloseDate
        {
            get;
            set;
        }

        /// <summary>
        /// 繳費期限 (民國年 yyyMMdd) (預設為 StudentReceive.PayDueDate，無值則為 SchoolRid.PayDate)
        /// </summary>
        [FieldSpec(Field.PayDueDate, false, FieldTypeEnum.Char, 7, false)]
        public string PayDueDate
        {
            get;
            set;
        }

        /// <summary>
        /// 繳費期限2 (民國年 yyyMMdd) (土銀用來存放信用卡繳費期限)
        /// </summary>
        [FieldSpec(Field.PayDueDate2, false, FieldTypeEnum.Char, 7, false)]
        public string PayDueDate2
        {
            get;
            set;
        }

        /// <summary>
        /// 繳費期限3 (民國年 yyyMMdd) (土銀用來存放財金繳費期限)
        /// </summary>
        [FieldSpec(Field.PayDueDate3, false, FieldTypeEnum.Char, 7, false)]
        public string PayDueDate3
        {
            get;
            set;
        }
        #endregion

        #region Readonly Property
        #region 學號不遮了
        ///// <summary>
        ///// 取得遮罩後的學號
        ///// </summary>
        //[XmlIgnore]
        //public string MaskedStuId
        //{
        //    get
        //    {
        //        return DataFormat.MaskText(this.StuId, DataFormat.MaskDataType.ID);
        //    }
        //}
        #endregion

        /// <summary>
        /// 取得遮罩後的學生姓名
        /// </summary>
        [XmlIgnore]
        public string MaskedStuName
        {
            get
            {
                return DataFormat.MaskText(this.StuName, DataFormat.MaskDataType.Name);
            }
        }

        /// <summary>
        /// 取得銷帳狀態
        /// </summary>
        [XmlIgnore]
        public string CancelStatus
        {
            get
            {
                Fuju.CodeText data = CancelStatusCodeTexts.GetCancelStatus(this.ReceiveDate, this.AccountDate);
                return data == null ? String.Empty : data.Text;
            }
        }
        #endregion
        #endregion

        #region Method
        /// <summary>
        /// 取得 DateTime? 型別的繳費期限
        /// </summary>
        /// <returns></returns>
        public DateTime? GetPayDueDate()
        {
            DateTime date;
            if (Fuju.Common.TryConvertTWDate7(this.PayDueDate, out date))
            {
                return date;
            }
            return null;
        }

        /// <summary>
        /// 取得 DateTime? 型別的繳費期限2 (土銀用來存放信用卡繳費期限)
        /// </summary>
        /// <returns></returns>
        public DateTime? GetPayDueDate2()
        {
            DateTime date;
            if (Fuju.Common.TryConvertTWDate7(this.PayDueDate2, out date))
            {
                return date;
            }
            return null;
        }

        /// <summary>
        /// 取得 DateTime? 型別的繳費期限3 (土銀用來存放財金繳費期限)
        /// </summary>
        /// <returns></returns>
        public DateTime? GetPayDueDate3()
        {
            DateTime date;
            if (Fuju.Common.TryConvertTWDate7(this.PayDueDate3, out date))
            {
                return date;
            }
            return null;
        }

        public string ToKeyText()
        {
            return String.Format("{0}_{1}_{2}_{3}_{4}_{5}", this.ReceiveType, this.YearId, this.TermId, this.DepId, this.ReceiveId, this.StuId);
        }
        #endregion
    }
}
