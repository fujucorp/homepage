using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;


namespace Entities
{
    /// <summary>
    /// 查詢可委扣學生繳費資料用的 View 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public class StudentReceiveView2 : Entity
    {
        protected const string VIEWSQL = @"SELECT Receive_Type, Year_Id, Term_Id, Dep_Id, Receive_Id, Stu_Id, Old_Seq
     , ISNULL((SELECT TOP 1 Stu_Name FROM Student_Master AS SM WHERE SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id), '') AS Stu_Name
     , ISNULL(Cancel_No, '') AS Cancel_No, ISNULL(Receive_Amount, 0) AS Receive_Amount
     , ISNULL(Deduct_BankId, '') AS Deduct_BankId, ISNULL(Deduct_AccountNo, '') AS Deduct_AccountNo, ISNULL(Deduct_AccountName, '') AS Deduct_AccountName, ISNULL(Deduct_AccountId, '') AS Deduct_AccountId
  FROM Student_Receive AS SR
 WHERE (Receive_Way IS NULL OR Receive_Way = '') AND (Receive_Date IS NULL OR Receive_Date = '')
   AND (Deduct_BankId IS NOT NULL AND Deduct_BankId != '') AND (Deduct_AccountNo IS NOT NULL AND Deduct_AccountNo != '')";

        #region Field Name Const Class
        /// <summary>
        /// 學生繳費資料 + 院別、系所、班別、減免類別、住宿項目、就貸項目、身分註記1 ~ 身分註記6 名稱 View 欄位名稱定義抽象類別
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
            /// 部別代碼 (土銀不使用)
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
            /// 銷帳編號
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// 繳費金額合計
            /// </summary>
            public const string ReceiveAmount = "Receive_Amount";
            #endregion

            #region 扣款資料相關欄位
            /// <summary>
            /// 扣款轉帳銀行代碼 欄位名稱常數定義
            /// </summary>
            public const string DeductBankId = "Deduct_BankId";

            /// <summary>
            /// 扣款轉帳銀行帳號 欄位名稱常數定義
            /// </summary>
            public const string DeductAccountNo = "Deduct_AccountNo";

            /// <summary>
            /// 扣款轉帳銀行帳號戶名 欄位名稱常數定義
            /// </summary>
            public const string DeductAccountName = "Deduct_AccountName";

            /// <summary>
            /// 扣款轉帳銀行帳戶ＩＤ 欄位名稱常數定義
            /// </summary>
            public const string DeductAccountId = "Deduct_AccountId";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構學生繳費資料 + 院別、系所、班別、減免類別、住宿項目、就貸項目、身分註記1 ~ 身分註記6 名稱 View 的資料承載類別
        /// </summary>
        public StudentReceiveView2()
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
        /// 部別代碼 (土銀不使用)
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
        [FieldSpec(Field.StuName, false, FieldTypeEnum.NVarChar, 60, true)]
        public string StuName
        {
            get;
            set;
        }

        /// <summary>
        /// 銷帳編號
        /// </summary>
        [FieldSpec(Field.CancelNo, false, FieldTypeEnum.Char, 16, true)]
        public string CancelNo
        {
            get;
            set;
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
        #endregion

        #region 扣款資料相關欄位
        private string _DeductBankId = null;
        /// <summary>
        /// 扣款轉帳銀行代碼 欄位屬性
        /// </summary>
        [FieldSpec(Field.DeductBankId, false, FieldTypeEnum.VarChar, 7, false)]
        public string DeductBankId
        {
            get
            {
                return _DeductBankId;
            }
            set
            {
                _DeductBankId = value == null ? null : value.Trim();
            }
        }

        private string _DeductAccountNo = null;
        /// <summary>
        /// 扣款轉帳銀行帳號 欄位屬性
        /// </summary>
        [FieldSpec(Field.DeductAccountNo, false, FieldTypeEnum.VarChar, 21, false)]
        public string DeductAccountNo
        {
            get
            {
                return _DeductAccountNo;
            }
            set
            {
                _DeductAccountNo = value == null ? null : value.Trim();
            }
        }

        private string _DeductAccountName = null;
        /// <summary>
        /// 扣款轉帳銀行戶名 欄位屬性
        /// </summary>
        [FieldSpec(Field.DeductAccountName, false, FieldTypeEnum.NVarChar, 60, false)]
        public string DeductAccountName
        {
            get
            {
                return _DeductAccountName;
            }
            set
            {
                _DeductAccountName = value == null ? null : value.Trim();
            }
        }

        private string _DeductAccountId = null;
        /// <summary>
        /// 扣款轉帳銀行帳戶ＩＤ 欄位屬性
        /// </summary>
        [FieldSpec(Field.DeductAccountId, false, FieldTypeEnum.NVarChar, 10, false)]
        public string DeductAccountId
        {
            get
            {
                return _DeductAccountId;
            }
            set
            {
                _DeductAccountId = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion

        #region
        public string ToKeyText()
        {
            return String.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", this.ReceiveType, this.YearId, this.TermId, this.DepId, this.ReceiveId, this.StuId, this.OldSeq);
        }
        #endregion
    }
}
