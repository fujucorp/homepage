using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;


namespace Entities
{
    /// <summary>
    /// 學生繳費資料 (銷帳相關欄位) + 學生資料 (學號、名稱、身分證號) View 的資料承載類別 (銷帳用，只取有虛擬帳號與繳款金額的資料)
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class StudentReceiveView7 : Entity
    {
        #region VIEWSQL
        #region [OLD]
//        protected const string VIEWSQL = @"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Stu_Id, SR.Old_Seq
//     , SR.Cancel_No, SR.Cancel_ATMNo, SR.Cancel_SMNo
//     , SR.Receive_Amount, SR.Receive_ATMAmount, SR.Receive_SMAmount
//     , SR.Receive_Date, SR.Receive_Time, SR.Receive_Way, SR.Account_Date, SR.Receivebank_Id, SR.Cancel_Flag
//     , SM.Stu_Name, SM.Id_Number
//     , SR.create_date
//  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS SR
//  LEFT JOIN [" + StudentMasterEntity.TABLE_NAME + @"] AS SM ON SR.Receive_Type = SM.Receive_Type AND SR.Dep_Id = SM.Dep_Id AND SR.Stu_Id = SM.Stu_Id
// WHERE (SR.Cancel_No IS NOT NULL AND SR.Cancel_No != '') AND (SR.Receive_Amount IS NOT NULL AND SR.Receive_Amount > 0) ";
        #endregion

        #region #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
        protected const string VIEWSQL = @"SELECT SR.Receive_Type, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_Id, SR.Stu_Id, SR.Old_Seq
     , SR.Cancel_No, SR.Cancel_ATMNo, SR.Cancel_SMNo
     , SR.Receive_Amount, SR.Receive_ATMAmount, SR.Receive_SMAmount
     , SR.Receive_Date, SR.Receive_Time, SR.Receive_Way, SR.Account_Date, SR.Receivebank_Id, SR.Cancel_Flag
     , SM.Stu_Name, SM.Id_Number
     , SR.create_date, SR.NCCardFlag
  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS SR
  LEFT JOIN [" + StudentMasterEntity.TABLE_NAME + @"] AS SM ON SR.Receive_Type = SM.Receive_Type AND SR.Dep_Id = SM.Dep_Id AND SR.Stu_Id = SM.Stu_Id
 WHERE (SR.Cancel_No IS NOT NULL AND SR.Cancel_No != '') AND (SR.Receive_Amount IS NOT NULL AND SR.Receive_Amount > 0) ";
        #endregion

        #endregion

        #region Field Name Const Class
        /// <summary>
        /// StudentReceiveView7 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 商家代號
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
            /// (舊)資料序號 (不指定則預設為 0，指定時只允許1~999，超過則為轉置的舊資料序號)
            /// </summary>
            public const string OldSeq = "Old_Seq";
            #endregion

            #region Data
            #region 銷帳編號相關欄位
            /// <summary>
            /// 銷帳編號
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// ATM銷帳編號
            /// </summary>
            public const string CancelAtmno = "Cancel_ATMNo";

            /// <summary>
            /// 超商銷帳編號
            /// </summary>
            public const string CancelSmno = "Cancel_SMNo";
            #endregion

            #region 繳費金額相關欄位
            /// <summary>
            /// 繳費金額合計
            /// </summary>
            public const string ReceiveAmount = "Receive_Amount";

            /// <summary>
            /// ATM繳費金額
            /// </summary>
            public const string ReceiveAtmamount = "Receive_ATMAmount";

            /// <summary>
            /// 超商繳費金額
            /// </summary>
            public const string ReceiveSmamount = "Receive_SMAmount";
            #endregion

            #region 銷帳相關欄位
            /// <summary>
            /// 代收日期 (民國年日期7碼)
            /// </summary>
            public const string ReceiveDate = "Receive_Date";

            /// <summary>
            /// 代收時間 (HHmmdd / HHmm)
            /// </summary>
            public const string ReceiveTime = "Receive_Time";

            /// <summary>
            /// 繳費方式 (參考管道代碼)
            /// </summary>
            public const string ReceiveWay = "Receive_Way";

            /// <summary>
            /// 入帳日期 (民國年日期7碼)
            /// </summary>
            public const string AccountDate = "Account_Date";

            /// <summary>
            /// 代收銀行/分行
            /// </summary>
            public const string ReceivebankId = "Receivebank_Id";

            /// <summary>
            /// 銷帳註記 (1=連線 / 2=金額不符 / 3=檢碼不符 / 7=銷問題檔 / 8=人工銷帳 / 9=離線) (參考 CancelFlagCodeTexts) 欄位名稱常數定義
            /// </summary>
            public const string CancelFlag = "Cancel_Flag";
            #endregion

            #region 學生資料相關欄位
            /// <summary>
            /// 學生姓名
            /// </summary>
            public const string StuName = "Stu_Name";

            /// <summary>
            /// 學生身分證號
            /// </summary>
            public const string StuIdNumber = "Id_Number";
            #endregion

            /// <summary>
            /// 建立日期時間
            /// </summary>
            public const string CreateDate = "create_date";

            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
            /// <summary>
            /// 是否啟用國際信用卡繳費旗標
            /// </summary>
            public const string NCCardFlag = "NCCardFlag";
            #endregion
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 學生繳費資料 (銷帳相關欄位) + 學生資料 (學號、名稱、身分證號) View 的資料承載類別
        /// </summary>
        public StudentReceiveView7()
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
        #region 銷帳編號相關欄位
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

        private string _CancelAtmno = null;
        /// <summary>
        /// ATM銷帳編號
        /// </summary>
        [FieldSpec(Field.CancelAtmno, false, FieldTypeEnum.Char, 16, true)]
        public string CancelAtmno
        {
            get
            {
                return _CancelAtmno;
            }
            set
            {
                _CancelAtmno = value == null ? null : value.Trim();
            }
        }

        private string _CancelSmno = null;
        /// <summary>
        /// 超商銷帳編號
        /// </summary>
        [FieldSpec(Field.CancelSmno, false, FieldTypeEnum.Char, 16, true)]
        public string CancelSmno
        {
            get
            {
                return _CancelSmno;
            }
            set
            {
                _CancelSmno = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 繳費金額相關欄位
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
        /// ATM繳費金額 欄位屬性
        /// </summary>
        [FieldSpec(Field.ReceiveAtmamount, false, FieldTypeEnum.Decimal, true)]
        public decimal? ReceiveAtmamount
        {
            get;
            set;
        }

        /// <summary>
        /// 超商繳費金額
        /// </summary>
        [FieldSpec(Field.ReceiveSmamount, false, FieldTypeEnum.Decimal, true)]
        public decimal? ReceiveSmamount
        {
            get;
            set;
        }
        #endregion

        #region 銷帳相關欄位
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

        private string _ReceiveTime = null;
        /// <summary>
        /// 代收時間 (HHmmdd / HHmm)
        /// </summary>
        [FieldSpec(Field.ReceiveTime, false, FieldTypeEnum.Char, 6, true)]
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

        private string _ReceiveBankId = null;
        /// <summary>
        /// 代收銀行/分行
        /// </summary>
        [FieldSpec(Field.ReceivebankId, false, FieldTypeEnum.Char, 7, true)]
        public string ReceivebankId
        {
            get
            {
                return _ReceiveBankId;
            }
            set
            {
                _ReceiveBankId = value == null ? null : value.Trim();
            }
        }
        private string _CancelFlag = null;
        /// <summary>
        /// 銷帳註記 (1=連線 / 2=金額不符 / 3=檢碼不符 / 7=銷問題檔 / 8=人工銷帳 / 9=離線) (參考 CancelFlagCodeTexts)
        /// </summary>
        [FieldSpec(Field.CancelFlag, false, FieldTypeEnum.Char, 1, true)]
        public string CancelFlag
        {
            get
            {
                return _CancelFlag;
            }
            set
            {
                _CancelFlag = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 學生資料相關欄位
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

        /// <summary>
        /// 建立日期時間
        /// </summary>
        [FieldSpec(Field.CreateDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? CreateDate
        {
            get;
            set;
        }

        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
        private string _NCCardFlag = "N";
        /// <summary>
        /// 是否啟用國際信用卡繳費旗標 (Y=是; N=否; 預設值 N)
        /// </summary>
        [FieldSpec(Field.NCCardFlag, false, FieldTypeEnum.Char, 1, true)]
        public string NCCardFlag
        {
            get
            {
                return _NCCardFlag;
            }
            set
            {
                _NCCardFlag = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion
        #endregion
    }
}
