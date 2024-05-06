/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:32:47
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 學生就貸 資料表 Entity 類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class StudentLoanEntity : Entity
    {
        public const string TABLE_NAME = "Student_Loan";

        #region Field Name Const Class
        /// <summary>
        /// StudentLoanEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 商家代碼 欄位名稱常數定義
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
            /// 部別代碼 (土銀不使用原部別，此欄位固定為空字串) 欄位名稱常數定義
            /// </summary>
            public const string DepId = "Dep_Id";

            /// <summary>
            /// 費用別代碼 欄位名稱常數定義
            /// </summary>
            public const string ReceiveId = "Receive_Id";

            /// <summary>
            /// 學號 欄位名稱常數定義
            /// </summary>
            public const string StuId = "Stu_Id";

            /// <summary>
            /// 舊資料序號 (非舊學雜費轉置的資料，固定為 0) 欄位名稱常數定義
            /// </summary>
            public const string OldSeq = "Old_Seq";

            /// <summary>
            /// 就貸代號 欄位名稱常數定義
            /// </summary>
            public const string LoanId = "Loan_Id";
            #endregion

            #region Data
            #region 就貸明細 01 ~ 40
            /// <summary>
            /// 就貸金額項目01 (收入科目01就貸金額) 欄位名稱常數定義
            /// </summary>
            public const string Loan01 = "Loan_01";

            /// <summary>
            /// Loan_02 欄位名稱常數定義
            /// </summary>
            public const string Loan02 = "Loan_02";

            /// <summary>
            /// Loan_03 欄位名稱常數定義
            /// </summary>
            public const string Loan03 = "Loan_03";

            /// <summary>
            /// Loan_04 欄位名稱常數定義
            /// </summary>
            public const string Loan04 = "Loan_04";

            /// <summary>
            /// Loan_05 欄位名稱常數定義
            /// </summary>
            public const string Loan05 = "Loan_05";

            /// <summary>
            /// Loan_06 欄位名稱常數定義
            /// </summary>
            public const string Loan06 = "Loan_06";

            /// <summary>
            /// Loan_07 欄位名稱常數定義
            /// </summary>
            public const string Loan07 = "Loan_07";

            /// <summary>
            /// Loan_08 欄位名稱常數定義
            /// </summary>
            public const string Loan08 = "Loan_08";

            /// <summary>
            /// Loan_09 欄位名稱常數定義
            /// </summary>
            public const string Loan09 = "Loan_09";

            /// <summary>
            /// Loan_10 欄位名稱常數定義
            /// </summary>
            public const string Loan10 = "Loan_10";

            /// <summary>
            /// Loan_11 欄位名稱常數定義
            /// </summary>
            public const string Loan11 = "Loan_11";

            /// <summary>
            /// Loan_12 欄位名稱常數定義
            /// </summary>
            public const string Loan12 = "Loan_12";

            /// <summary>
            /// Loan_13 欄位名稱常數定義
            /// </summary>
            public const string Loan13 = "Loan_13";

            /// <summary>
            /// Loan_14 欄位名稱常數定義
            /// </summary>
            public const string Loan14 = "Loan_14";

            /// <summary>
            /// Loan_15 欄位名稱常數定義
            /// </summary>
            public const string Loan15 = "Loan_15";

            /// <summary>
            /// Loan_16 欄位名稱常數定義
            /// </summary>
            public const string Loan16 = "Loan_16";

            /// <summary>
            /// Loan_17 欄位名稱常數定義
            /// </summary>
            public const string Loan17 = "Loan_17";

            /// <summary>
            /// Loan_18 欄位名稱常數定義
            /// </summary>
            public const string Loan18 = "Loan_18";

            /// <summary>
            /// Loan_19 欄位名稱常數定義
            /// </summary>
            public const string Loan19 = "Loan_19";

            /// <summary>
            /// Loan_20 欄位名稱常數定義
            /// </summary>
            public const string Loan20 = "Loan_20";

            /// <summary>
            /// Loan_21 欄位名稱常數定義
            /// </summary>
            public const string Loan21 = "Loan_21";

            /// <summary>
            /// Loan_22 欄位名稱常數定義
            /// </summary>
            public const string Loan22 = "Loan_22";

            /// <summary>
            /// Loan_23 欄位名稱常數定義
            /// </summary>
            public const string Loan23 = "Loan_23";

            /// <summary>
            /// Loan_24 欄位名稱常數定義
            /// </summary>
            public const string Loan24 = "Loan_24";

            /// <summary>
            /// Loan_25 欄位名稱常數定義
            /// </summary>
            public const string Loan25 = "Loan_25";

            /// <summary>
            /// Loan_26 欄位名稱常數定義
            /// </summary>
            public const string Loan26 = "Loan_26";

            /// <summary>
            /// Loan_27 欄位名稱常數定義
            /// </summary>
            public const string Loan27 = "Loan_27";

            /// <summary>
            /// Loan_28 欄位名稱常數定義
            /// </summary>
            public const string Loan28 = "Loan_28";

            /// <summary>
            /// Loan_29 欄位名稱常數定義
            /// </summary>
            public const string Loan29 = "Loan_29";

            /// <summary>
            /// Loan_30 欄位名稱常數定義
            /// </summary>
            public const string Loan30 = "Loan_30";

            /// <summary>
            /// Loan_31 欄位名稱常數定義
            /// </summary>
            public const string Loan31 = "Loan_31";

            /// <summary>
            /// Loan_32 欄位名稱常數定義
            /// </summary>
            public const string Loan32 = "Loan_32";

            /// <summary>
            /// Loan_33 欄位名稱常數定義
            /// </summary>
            public const string Loan33 = "Loan_33";

            /// <summary>
            /// Loan_34 欄位名稱常數定義
            /// </summary>
            public const string Loan34 = "Loan_34";

            /// <summary>
            /// Loan_35 欄位名稱常數定義
            /// </summary>
            public const string Loan35 = "Loan_35";

            /// <summary>
            /// Loan_36 欄位名稱常數定義
            /// </summary>
            public const string Loan36 = "Loan_36";

            /// <summary>
            /// Loan_37 欄位名稱常數定義
            /// </summary>
            public const string Loan37 = "Loan_37";

            /// <summary>
            /// Loan_38 欄位名稱常數定義
            /// </summary>
            public const string Loan38 = "Loan_38";

            /// <summary>
            /// Loan_39 欄位名稱常數定義
            /// </summary>
            public const string Loan39 = "Loan_39";

            /// <summary>
            /// Loan_40 欄位名稱常數定義
            /// </summary>
            public const string Loan40 = "Loan_40";
            #endregion

            /// <summary>
            /// 計算後的可貸金額 欄位名稱常數定義
            /// </summary>
            public const string LoanAmount = "Loan_Amount";

            /// <summary>
            /// 上傳的可貸金額 欄位名稱常數定義
            /// </summary>
            public const string LoanFixamount = "Loan_fixAmount";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// StudentLoanEntity 類別建構式
        /// </summary>
        public StudentLoanEntity()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _ReceiveType = null;
        /// <summary>
        /// 商家代碼
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
        /// 部別代碼 (土銀不使用原部別，此欄位固定為空字串)
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
        /// 費用別代碼
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
        /// 舊資料序號 (非舊學雜費轉置的資料，固定為 0) (為了對應至 Student_Receive)
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

        private string _LoanId = null;
        /// <summary>
        /// 就貸代號
        /// </summary>
        [FieldSpec(Field.LoanId, true, FieldTypeEnum.VarChar, 20, false)]
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
        #endregion

        #region Data
        #region 就貸明細 01 ~ 40
        /// <summary>
        /// 就貸金額項目01 (收入科目01就貸金額)
        /// </summary>
        [FieldSpec(Field.Loan01, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan01
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_02 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan02, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan02
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_03 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan03, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan03
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_04 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan04, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan04
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_05 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan05, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan05
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_06 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan06, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan06
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_07 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan07, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan07
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_08 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan08, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan08
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_09 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan09, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan09
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_10 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan10, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan10
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_11 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan11, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan11
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_12 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan12, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan12
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_13 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan13, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan13
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_14 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan14, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan14
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_15 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan15, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan15
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_16 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan16, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan16
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_17 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan17, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan17
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_18 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan18, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan18
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_19 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan19, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan19
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_20 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan20, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan20
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_21 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan21, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan21
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_22 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan22, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan22
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_23 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan23, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan23
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_24 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan24, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan24
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_25 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan25, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan25
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_26 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan26, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan26
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_27 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan27, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan27
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_28 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan28, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan28
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_29 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan29, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan29
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_30 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan30, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan30
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_31 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan31, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan31
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_32 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan32, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan32
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_33 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan33, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan33
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_34 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan34, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan34
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_35 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan35, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan35
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_36 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan36, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan36
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_37 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan37, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan37
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_38 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan38, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan38
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_39 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan39, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan39
        {
            get;
            set;
        }

        /// <summary>
        /// Loan_40 欄位屬性
        /// </summary>
        [FieldSpec(Field.Loan40, false, FieldTypeEnum.Decimal, true)]
        public decimal? Loan40
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 計算後的可貸金額
        /// </summary>
        [FieldSpec(Field.LoanAmount, false, FieldTypeEnum.Decimal, true)]
        public decimal? LoanAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 上傳的可貸金額
        /// </summary>
        [FieldSpec(Field.LoanFixamount, false, FieldTypeEnum.Decimal, true)]
        public decimal? LoanFixamount
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Method
        /// <summary>
        /// 設定指定項目編號的就貸明細金額 (1 ~ 40)
        /// </summary>
        /// <param name="no">項目編號 (1 ~ 40)</param>
        /// <param name="amount">項目就貸金額</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool SetLoanItemAmount(int no, decimal amount)
        {
            string itemName = String.Format("Loan_{0:00}", no);
            return this.SetLoanItemAmount(itemName, amount);
        }

        /// <summary>
        /// 設定指定項目名稱的就貸明細金額 (Loan_01 ~ Loan_40)
        /// </summary>
        /// <param name="itemName">項目名稱 (Loan_01 ~ Loan_40，不區分大小寫)</param>
        /// <param name="amount">項目就貸金額</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool SetLoanItemAmount(string itemName, decimal? amount)
        {
            if (!String.IsNullOrWhiteSpace(itemName) && itemName.StartsWith("Loan_", StringComparison.CurrentCultureIgnoreCase))
            {
                itemName = itemName.ToLower().Replace("loan_", "Loan_");
                switch (itemName)
                {
                    #region 01 ~ 10
                    case Field.Loan01:
                        this.Loan01 = amount;
                        return true;
                    case Field.Loan02:
                        this.Loan02 = amount;
                        return true;
                    case Field.Loan03:
                        this.Loan03 = amount;
                        return true;
                    case Field.Loan04:
                        this.Loan04 = amount;
                        return true;
                    case Field.Loan05:
                        this.Loan05 = amount;
                        return true;
                    case Field.Loan06:
                        this.Loan06 = amount;
                        return true;
                    case Field.Loan07:
                        this.Loan07 = amount;
                        return true;
                    case Field.Loan08:
                        this.Loan08 = amount;
                        return true;
                    case Field.Loan09:
                        this.Loan09 = amount;
                        return true;
                    case Field.Loan10:
                        this.Loan10 = amount;
                        return true;
                    #endregion

                    #region 11 ~ 20
                    case Field.Loan11:
                        this.Loan11 = amount;
                        return true;
                    case Field.Loan12:
                        this.Loan12 = amount;
                        return true;
                    case Field.Loan13:
                        this.Loan13 = amount;
                        return true;
                    case Field.Loan14:
                        this.Loan14 = amount;
                        return true;
                    case Field.Loan15:
                        this.Loan15 = amount;
                        return true;
                    case Field.Loan16:
                        this.Loan16 = amount;
                        return true;
                    case Field.Loan17:
                        this.Loan17 = amount;
                        return true;
                    case Field.Loan18:
                        this.Loan18 = amount;
                        return true;
                    case Field.Loan19:
                        this.Loan19 = amount;
                        return true;
                    case Field.Loan20:
                        this.Loan20 = amount;
                        return true;
                    #endregion

                    #region 21 ~ 30
                    case Field.Loan21:
                        this.Loan21 = amount;
                        return true;
                    case Field.Loan22:
                        this.Loan22 = amount;
                        return true;
                    case Field.Loan23:
                        this.Loan23 = amount;
                        return true;
                    case Field.Loan24:
                        this.Loan24 = amount;
                        return true;
                    case Field.Loan25:
                        this.Loan25 = amount;
                        return true;
                    case Field.Loan26:
                        this.Loan26 = amount;
                        return true;
                    case Field.Loan27:
                        this.Loan27 = amount;
                        return true;
                    case Field.Loan28:
                        this.Loan28 = amount;
                        return true;
                    case Field.Loan29:
                        this.Loan29 = amount;
                        return true;
                    case Field.Loan30:
                        this.Loan30 = amount;
                        return true;
                    #endregion

                    #region 31 ~ 40
                    case Field.Loan31:
                        this.Loan31 = amount;
                        return true;
                    case Field.Loan32:
                        this.Loan32 = amount;
                        return true;
                    case Field.Loan33:
                        this.Loan33 = amount;
                        return true;
                    case Field.Loan34:
                        this.Loan34 = amount;
                        return true;
                    case Field.Loan35:
                        this.Loan35 = amount;
                        return true;
                    case Field.Loan36:
                        this.Loan36 = amount;
                        return true;
                    case Field.Loan37:
                        this.Loan37 = amount;
                        return true;
                    case Field.Loan38:
                        this.Loan38 = amount;
                        return true;
                    case Field.Loan39:
                        this.Loan39 = amount;
                        return true;
                    case Field.Loan40:
                        this.Loan40 = amount;
                        return true;
                    #endregion
                }
            }
            return false;
        }

        /// <summary>
        /// 取得指定項目編號的就貸明細金額 (1 ~ 40)
        /// </summary>
        /// <param name="no">項目編號 (1 ~ 40)</param>
        /// <returns>傳回項目就貸金額或 null</returns>
        public decimal? GetLoanItemAmount(int no)
        {
            string itemName = String.Format("Loan_{0:00}", no);
            return this.GetLoanItemAmount(itemName);
        }

        /// <summary>
        /// 取得指定項目名稱的就貸明細金額 (Loan_01 ~ Loan_40)
        /// </summary>
        /// <param name="itemName">項目名稱 (Loan_01 ~ Loan_40，不區分大小寫)</param>
        /// <returns>傳回項目就貸金額或 null</returns>
        public decimal? GetLoanItemAmount(string itemName)
        {
            if (!String.IsNullOrWhiteSpace(itemName) && itemName.StartsWith("Loan_", StringComparison.CurrentCultureIgnoreCase))
            {
                itemName = itemName.ToLower().Replace("loan_", "Loan_");
                switch (itemName)
                {
                    #region 01 ~ 10
                    case Field.Loan01:
                        return this.Loan01;
                    case Field.Loan02:
                        return this.Loan02;
                    case Field.Loan03:
                        return this.Loan03;
                    case Field.Loan04:
                        return this.Loan04;
                    case Field.Loan05:
                        return this.Loan05;
                    case Field.Loan06:
                        return this.Loan06;
                    case Field.Loan07:
                        return this.Loan07;
                    case Field.Loan08:
                        return this.Loan08;
                    case Field.Loan09:
                        return this.Loan09;
                    case Field.Loan10:
                        return this.Loan10;
                    #endregion

                    #region 11 ~ 20
                    case Field.Loan11:
                        return this.Loan11;
                    case Field.Loan12:
                        return this.Loan12;
                    case Field.Loan13:
                        return this.Loan13;
                    case Field.Loan14:
                        return this.Loan14;
                    case Field.Loan15:
                        return this.Loan15;
                    case Field.Loan16:
                        return this.Loan16;
                    case Field.Loan17:
                        return this.Loan17;
                    case Field.Loan18:
                        return this.Loan18;
                    case Field.Loan19:
                        return this.Loan19;
                    case Field.Loan20:
                        return this.Loan20;
                    #endregion

                    #region 21 ~ 30
                    case Field.Loan21:
                        return this.Loan21;
                    case Field.Loan22:
                        return this.Loan22;
                    case Field.Loan23:
                        return this.Loan23;
                    case Field.Loan24:
                        return this.Loan24;
                    case Field.Loan25:
                        return this.Loan25;
                    case Field.Loan26:
                        return this.Loan26;
                    case Field.Loan27:
                        return this.Loan27;
                    case Field.Loan28:
                        return this.Loan28;
                    case Field.Loan29:
                        return this.Loan29;
                    case Field.Loan30:
                        return this.Loan30;
                    #endregion

                    #region 31 ~ 40
                    case Field.Loan31:
                        return this.Loan31;
                    case Field.Loan32:
                        return this.Loan32;
                    case Field.Loan33:
                        return this.Loan33;
                    case Field.Loan34:
                        return this.Loan34;
                    case Field.Loan35:
                        return this.Loan35;
                    case Field.Loan36:
                        return this.Loan36;
                    case Field.Loan37:
                        return this.Loan37;
                    case Field.Loan38:
                        return this.Loan38;
                    case Field.Loan39:
                        return this.Loan39;
                    case Field.Loan40:
                        return this.Loan40;
                    #endregion
                }
            }
            return null;
        }

        /// <summary>
        /// 取得指定項目名稱的就貸明細金額 (Loan_01 ~ Loan_40) 並嘗試轉成 Int32 型別
        /// </summary>
        /// <param name="itemName">項目名稱 (Loan_01 ~ Loan_40，不區分大小寫)</param>
        /// <returns>傳回Int32 型別的項目就貸金額或 0</returns>
        public int TryGetLoanItemAmountByInt32(string itemName)
        {
            decimal? amount = this.GetLoanItemAmount(itemName);
            if (amount == null || amount.Value > Int32.MaxValue)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(amount.Value);
            }
        }
        #endregion
    }
}
