using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 學生繳費資料 + 學生資料 (名稱+身分證) + 就貸名稱 View 的資料承載類別 (就貸用)
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class StudentReceiveView5 : Entity
    {
        protected const string VIEWSQL = @"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Stu_Id, SR.Old_Seq
     , SR.Loan_Id, SR.Major_Id, SR.Stu_Grade, SR.Up_No, SR.Cancel_No, SR.Receive_Amount
     , SR.loan, SR.real_loan
     , SR.Receive_Date, SR.Receive_Way, SR.Account_Date
     , SM.Stu_Name, SM.Id_Number
     , (SELECT Loan_Name FROM Loan_List AS LL WHERE LL.Receive_Type = SR.Receive_Type AND LL.Year_Id = SR.Year_Id 
           AND LL.Term_Id = SR.Term_Id AND SR.Dep_Id = SR.Dep_Id AND LL.Loan_Id = SR.Loan_Id) AS Loan_Name
  FROM [Student_Receive] AS SR
  JOIN [Student_Master] AS SM ON SR.Receive_Type = SM.Receive_Type AND SR.Dep_Id = SM.Dep_Id AND SR.Stu_Id = SM.Stu_Id";

        #region Field Name Const Class
        /// <summary>
        /// StudentReceiveEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 商家代號 欄位名稱常數定義
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
            /// 部別代碼 (土銀不使用) 欄位名稱常數定義
            /// </summary>
            public const string DepId = "Dep_Id";

            /// <summary>
            /// 代收費用別代碼 欄位名稱常數定義
            /// </summary>
            public const string ReceiveId = "Receive_Id";

            /// <summary>
            /// 學號 欄位名稱常數定義
            /// </summary>
            public const string StuId = "Stu_Id";

            /// <summary>
            /// (舊)資料序號 (不指定則預設為 0，指定時只允許1~999，超過則為轉置的舊資料序號) 欄位名稱常數定義
            /// </summary>
            public const string OldSeq = "Old_Seq";
            #endregion

            #region Data
            /// <summary>
            /// 就貸代碼 欄位名稱常數定義
            /// </summary>
            public const string LoanId = "Loan_Id";

            /// <summary>
            /// 科系代碼 欄位名稱常數定義
            /// </summary>
            public const string MajorId = "Major_Id";

            /// <summary>
            /// 年級代碼 欄位名稱常數定義
            /// </summary>
            public const string StuGrade = "Stu_Grade";

            /// <summary>
            /// 上傳資料批號 欄位名稱常數定義
            /// </summary>
            public const string UpNo = "Up_No";

            /// <summary>
            /// 銷帳編號 欄位名稱常數定義
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// 繳費金額合計 欄位名稱常數定義
            /// </summary>
            public const string ReceiveAmount = "Receive_Amount";

            /// <summary>
            /// 上傳就學貸款金額 (BUD上傳的就貸金額) 欄位名稱常數定義
            /// </summary>
            public const string Loan = "loan";

            /// <summary>
            /// 實際貸款金額 (BUD上傳的就貸金額或由BUD上傳的就貸明細總額) 欄位名稱常數定義
            /// </summary>
            public const string RealLoan = "real_loan";

            /// <summary>
            /// 代收日期 (民國年日期7碼) 欄位名稱常數定義
            /// </summary>
            public const string ReceiveDate = "Receive_Date";

            /// <summary>
            /// 繳費方式 (參考管道代碼) 欄位名稱常數定義
            /// </summary>
            public const string ReceiveWay = "Receive_Way";

            /// <summary>
            /// 入帳日期 (民國年日期7碼) 欄位名稱常數定義
            /// </summary>
            public const string AccountDate = "Account_Date";

            /// <summary>
            /// 學生姓名 欄位名稱常數定義
            /// </summary>
            public const string StuName = "Stu_Name";

            /// <summary>
            /// 學生身分證字號 欄位名稱常數定義
            /// </summary>
            public const string StuIdNumber = "Id_Number";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構學生繳費資料 + 學生資料 (名稱+身分證) + 就貸名稱 View 的資料承載類別 (就貸用)
        /// </summary>
        public StudentReceiveView5()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _ReceiveType = null;
        /// <summary>
        /// 商家代號
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

        private string _DepId = String.Empty;
        /// <summary>
        /// 部別代碼 (土銀不使用，固定設為空字串)
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
        [FieldSpec(Field.ReceiveId, true, FieldTypeEnum.VarChar, 2, false)]
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
        /// (舊)資料序號 (不指定則預設為 0，指定時只允許1~999，超過則為轉置的舊資料序號)
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
        private string _LoanId = null;
        /// <summary>
        /// 就貸代碼
        /// </summary>
        [FieldSpec(Field.LoanId, false, FieldTypeEnum.VarChar, 20, true)]
        public string LoanId
        {
            get
            {
                return _LoanId;
            }
            set
            {
                _LoanId = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20200810] M202008_02 科系名稱長度放大到40個中文字
        private string _MajorId = null;
        /// <summary>
        /// 科系代碼
        /// </summary>
        [FieldSpec(Field.MajorId, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MajorId
        {
            get
            {
                return _MajorId;
            }
            set
            {
                _MajorId = value == null ? null : value.Trim();
            }
        }
        #endregion

        private string _StuGrade = null;
        /// <summary>
        /// 年級代碼
        /// </summary>
        [FieldSpec(Field.StuGrade, false, FieldTypeEnum.VarChar, 2, true)]
        public string StuGrade
        {
            get
            {
                return _StuGrade;
            }
            set
            {
                _StuGrade = value == null ? null : value.Trim();
            }
        }

        private string _UpNo = null;
        /// <summary>
        /// 上傳資料批號
        /// </summary>
        [FieldSpec(Field.UpNo, false, FieldTypeEnum.VarChar, 4, true)]
        public string UpNo
        {
            get
            {
                return _UpNo;
            }
            set
            {
                _UpNo = value == null ? null : value.Trim();
            }
        }

        private string _CancelNo = null;
        /// <summary>
        /// 銷帳編號
        /// </summary>
        [FieldSpec(Field.CancelNo, false, FieldTypeEnum.Char, 16, true)]
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
        /// 繳費金額合計
        /// </summary>
        [FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, true)]
        public decimal? ReceiveAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 上傳就學貸款金額 (BUD上傳的就貸金額)
        /// </summary>
        [FieldSpec(Field.Loan, false, FieldTypeEnum.Decimal, false)]
        public decimal Loan
        {
            get;
            set;
        }

        /// <summary>
        /// 實際貸款金額 (BUD上傳的就貸金額或由BUD上傳的就貸明細總額)
        /// </summary>
        [FieldSpec(Field.RealLoan, false, FieldTypeEnum.Decimal, false)]
        public decimal? RealLoan
        {
            get;
            set;
        }

        private string _ReceiveDate = null;
        /// <summary>
        /// 代收日期 (民國年日期7碼)
        /// </summary>
        [FieldSpec(Field.ReceiveDate, false, FieldTypeEnum.Char, 7, true)]
        public string ReceiveDate
        {
            get
            {
                return _ReceiveDate;
            }
            set
            {
                _ReceiveDate = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveWay = null;
        /// <summary>
        /// 繳費方式 (參考管道代碼)
        /// </summary>
        [FieldSpec(Field.ReceiveWay, false, FieldTypeEnum.VarChar, 4, true)]
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

        private string _AccountDate = null;
        /// <summary>
        /// 入帳日期 (民國年日期7碼)
        /// </summary>
        [FieldSpec(Field.AccountDate, false, FieldTypeEnum.Char, 7, true)]
        public string AccountDate
        {
            get
            {
                return _AccountDate;
            }
            set
            {
                _AccountDate = value == null ? null : value.Trim();
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

        private string _StuIdNumber = null;
        /// <summary>
        /// 學生身分證字號
        /// </summary>
        [FieldSpec(Field.StuIdNumber, false, FieldTypeEnum.Char, 12, true)]
        public string StuIdNumber
        {
            get
            {
                return _StuIdNumber;
            }
            set
            {
                _StuIdNumber = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion
    }
}
