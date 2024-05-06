using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 一般收費標準 View
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class GeneralStandardView : Entity
    {
        #region [Old]
//        protected const string VIEWSQL = @"
//select G.*, C.College_Name, M.Major_Name, Class.Class_Name from General_Standard G 
//inner join College_List C on G.Receive_Type= C.Receive_Type and G.Year_Id=C.Year_Id and G.Term_Id=C.Term_Id and G.Dep_Id=C.Dep_Id and G.College_Id = C.College_Id
//inner join Major_List M on G.Receive_Type= M.Receive_Type and G.Year_Id=M.Year_Id and G.Term_Id=M.Term_Id and G.Dep_Id=M.Dep_Id and G.Major_Id=M.Major_Id
//inner join Class_List Class on G.Receive_Type= Class.Receive_Type and G.Year_Id=Class.Year_Id and G.Term_Id=Class.Term_Id and G.Dep_Id=Class.Dep_Id and G.Major_Id=Class.Class_Id";
        #endregion

        protected const string VIEWSQL = @"
SELECT GS.*
	 , ISNULL((SELECT TOP 1 College_Name FROM College_List AS CO WHERE GS.Receive_Type = CO.Receive_Type AND GS.Year_Id = CO.Year_Id AND GS.Term_Id = CO.Term_Id AND GS.College_Id = CO.College_Id), '') AS College_Name
	 , ISNULL((SELECT TOP 1 Major_Name   FROM Major_List   AS ML WHERE GS.Receive_Type = ML.Receive_Type AND GS.Year_Id = ML.Year_Id AND GS.Term_Id = ML.Term_Id AND GS.Major_Id = ML.Major_Id), '') AS Major_Name
	 , ISNULL((SELECT TOP 1 Class_Name   FROM Class_List   AS CL WHERE GS.Receive_Type = CL.Receive_Type AND GS.Year_Id = CL.Year_Id AND GS.Term_Id = CL.Term_Id AND GS.Major_Id = CL.Class_Id), '') AS Class_Name
  FROM General_Standard AS GS ";

        #region Field Name Const Class
        /// <summary>
        /// GeneralStandardView 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// Receive_Type 欄位名稱常數定義
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// Year_Id 欄位名稱常數定義
            /// </summary>
            public const string YearId = "Year_Id";

            /// <summary>
            /// Term_Id 欄位名稱常數定義
            /// </summary>
            public const string TermId = "Term_Id";

            /// <summary>
            /// Dep_Id 欄位名稱常數定義
            /// </summary>
            public const string DepId = "Dep_Id";

            /// <summary>
            /// Receive_Id 欄位名稱常數定義
            /// </summary>
            public const string ReceiveId = "Receive_Id";

            /// <summary>
            /// College_Id 欄位名稱常數定義
            /// </summary>
            public const string CollegeId = "College_Id";

            /// <summary>
            /// Major_Id 欄位名稱常數定義
            /// </summary>
            public const string MajorId = "Major_Id";

            /// <summary>
            /// Stu_Grade 欄位名稱常數定義
            /// </summary>
            public const string StuGrade = "Stu_Grade";

            /// <summary>
            /// Class_Id 欄位名稱常數定義
            /// </summary>
            public const string ClassId = "Class_Id";
            #endregion

            #region Data
            /// <summary>
            /// General_01 欄位名稱常數定義
            /// </summary>
            public const string General01 = "General_01";

            /// <summary>
            /// General_02 欄位名稱常數定義
            /// </summary>
            public const string General02 = "General_02";

            /// <summary>
            /// General_03 欄位名稱常數定義
            /// </summary>
            public const string General03 = "General_03";

            /// <summary>
            /// General_04 欄位名稱常數定義
            /// </summary>
            public const string General04 = "General_04";

            /// <summary>
            /// General_05 欄位名稱常數定義
            /// </summary>
            public const string General05 = "General_05";

            /// <summary>
            /// General_06 欄位名稱常數定義
            /// </summary>
            public const string General06 = "General_06";

            /// <summary>
            /// General_07 欄位名稱常數定義
            /// </summary>
            public const string General07 = "General_07";

            /// <summary>
            /// General_08 欄位名稱常數定義
            /// </summary>
            public const string General08 = "General_08";

            /// <summary>
            /// General_09 欄位名稱常數定義
            /// </summary>
            public const string General09 = "General_09";

            /// <summary>
            /// General_10 欄位名稱常數定義
            /// </summary>
            public const string General10 = "General_10";

            /// <summary>
            /// General_11 欄位名稱常數定義
            /// </summary>
            public const string General11 = "General_11";

            /// <summary>
            /// General_12 欄位名稱常數定義
            /// </summary>
            public const string General12 = "General_12";

            /// <summary>
            /// General_13 欄位名稱常數定義
            /// </summary>
            public const string General13 = "General_13";

            /// <summary>
            /// General_14 欄位名稱常數定義
            /// </summary>
            public const string General14 = "General_14";

            /// <summary>
            /// General_15 欄位名稱常數定義
            /// </summary>
            public const string General15 = "General_15";

            /// <summary>
            /// General_16 欄位名稱常數定義
            /// </summary>
            public const string General16 = "General_16";

            /// <summary>
            /// General_17 欄位名稱常數定義
            /// </summary>
            public const string General17 = "General_17";

            /// <summary>
            /// General_18 欄位名稱常數定義
            /// </summary>
            public const string General18 = "General_18";

            /// <summary>
            /// General_19 欄位名稱常數定義
            /// </summary>
            public const string General19 = "General_19";

            /// <summary>
            /// General_20 欄位名稱常數定義
            /// </summary>
            public const string General20 = "General_20";

            /// <summary>
            /// General_21 欄位名稱常數定義
            /// </summary>
            public const string General21 = "General_21";

            /// <summary>
            /// General_22 欄位名稱常數定義
            /// </summary>
            public const string General22 = "General_22";

            /// <summary>
            /// General_23 欄位名稱常數定義
            /// </summary>
            public const string General23 = "General_23";

            /// <summary>
            /// General_24 欄位名稱常數定義
            /// </summary>
            public const string General24 = "General_24";

            /// <summary>
            /// General_25 欄位名稱常數定義
            /// </summary>
            public const string General25 = "General_25";

            /// <summary>
            /// General_26 欄位名稱常數定義
            /// </summary>
            public const string General26 = "General_26";

            /// <summary>
            /// General_27 欄位名稱常數定義
            /// </summary>
            public const string General27 = "General_27";

            /// <summary>
            /// General_28 欄位名稱常數定義
            /// </summary>
            public const string General28 = "General_28";

            /// <summary>
            /// General_29 欄位名稱常數定義
            /// </summary>
            public const string General29 = "General_29";

            /// <summary>
            /// General_30 欄位名稱常數定義
            /// </summary>
            public const string General30 = "General_30";

            #region [MDY:20200810] M202008_02 Fix
            /// <summary>
            /// General_31 欄位名稱常數定義
            /// </summary>
            public const string General31 = "General_31";

            /// <summary>
            /// General_32 欄位名稱常數定義
            /// </summary>
            public const string General32 = "General_32";

            /// <summary>
            /// General_33 欄位名稱常數定義
            /// </summary>
            public const string General33 = "General_33";

            /// <summary>
            /// General_34 欄位名稱常數定義
            /// </summary>
            public const string General34 = "General_34";

            /// <summary>
            /// General_35 欄位名稱常數定義
            /// </summary>
            public const string General35 = "General_35";

            /// <summary>
            /// General_36 欄位名稱常數定義
            /// </summary>
            public const string General36 = "General_36";

            /// <summary>
            /// General_37 欄位名稱常數定義
            /// </summary>
            public const string General37 = "General_37";

            /// <summary>
            /// General_38 欄位名稱常數定義
            /// </summary>
            public const string General38 = "General_38";

            /// <summary>
            /// General_39 欄位名稱常數定義
            /// </summary>
            public const string General39 = "General_39";

            /// <summary>
            /// General_40 欄位名稱常數定義
            /// </summary>
            public const string General40 = "General_40";
            #endregion

            /// <summary>
            /// Order_Id 欄位名稱常數定義
            /// </summary>
            public const string OrderId = "Order_Id";

            /// <summary>
            /// status 欄位名稱常數定義
            /// </summary>
            public const string Status = "status";

            /// <summary>
            /// crt_date 欄位名稱常數定義
            /// </summary>
            public const string CrtDate = "crt_date";

            /// <summary>
            /// crt_user 欄位名稱常數定義
            /// </summary>
            public const string CrtUser = "crt_user";

            /// <summary>
            /// mdy_date 欄位名稱常數定義
            /// </summary>
            public const string MdyDate = "mdy_date";

            /// <summary>
            /// mdy_user 欄位名稱常數定義
            /// </summary>
            public const string MdyUser = "mdy_user";

            /// <summary>
            /// College_Name 欄位名稱常數定義
            /// </summary>
            public const string CollegeName = "College_Name";

            /// <summary>
            /// Major_Name 欄位名稱常數定義
            /// </summary>
            public const string MajorName = "Major_Name";

            /// <summary>
            /// Class_Name 欄位名稱常數定義
            /// </summary>
            public const string ClassName= "Class_Name";
            #endregion
        }
        #endregion
        
        #region Constructor
        /// <summary>
        /// GeneralStandardView 類別建構式
        /// </summary>
        public GeneralStandardView()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _ReceiveType = null;
        /// <summary>
        /// Receive_Type 欄位屬性
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
        /// Year_Id 欄位屬性
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
        /// Term_Id 欄位屬性
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
        /// Dep_Id 欄位屬性
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
        /// Receive_Id 欄位屬性
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

        #region [MDY:20200810] M202008_02 科系名稱長度放大到40個中文字
        private string _CollegeId = null;
        /// <summary>
        /// College_Id 欄位屬性
        /// </summary>
        [FieldSpec(Field.CollegeId, true, FieldTypeEnum.VarChar, 20, false)]
        public string CollegeId
        {
            get
            {
                return _CollegeId;
            }
            set
            {
                _CollegeId = value == null ? null : value.Trim();
            }
        }

        private string _MajorId = null;
        /// <summary>
        /// Major_Id 欄位屬性
        /// </summary>
        [FieldSpec(Field.MajorId, true, FieldTypeEnum.NVarChar, 40, false)]
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

        private string _StuGrade = null;
        /// <summary>
        /// Stu_Grade 欄位屬性
        /// </summary>
        [FieldSpec(Field.StuGrade, true, FieldTypeEnum.VarChar, 2, false)]
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

        private string _ClassId = null;
        /// <summary>
        /// Class_Id 欄位屬性
        /// </summary>
        [FieldSpec(Field.ClassId, true, FieldTypeEnum.VarChar, 20, false)]
        public string ClassId
        {
            get
            {
                return _ClassId;
            }
            set
            {
                _ClassId = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion

        #region Data
        /// <summary>
        /// General_01 欄位屬性
        /// </summary>
        [FieldSpec(Field.General01, false, FieldTypeEnum.Decimal, true)]
        public decimal? General01
        {
            get;
            set;
        }

        /// <summary>
        /// General_02 欄位屬性
        /// </summary>
        [FieldSpec(Field.General02, false, FieldTypeEnum.Decimal, true)]
        public decimal? General02
        {
            get;
            set;
        }

        /// <summary>
        /// General_03 欄位屬性
        /// </summary>
        [FieldSpec(Field.General03, false, FieldTypeEnum.Decimal, true)]
        public decimal? General03
        {
            get;
            set;
        }

        /// <summary>
        /// General_04 欄位屬性
        /// </summary>
        [FieldSpec(Field.General04, false, FieldTypeEnum.Decimal, true)]
        public decimal? General04
        {
            get;
            set;
        }

        /// <summary>
        /// General_05 欄位屬性
        /// </summary>
        [FieldSpec(Field.General05, false, FieldTypeEnum.Decimal, true)]
        public decimal? General05
        {
            get;
            set;
        }

        /// <summary>
        /// General_06 欄位屬性
        /// </summary>
        [FieldSpec(Field.General06, false, FieldTypeEnum.Decimal, true)]
        public decimal? General06
        {
            get;
            set;
        }

        /// <summary>
        /// General_07 欄位屬性
        /// </summary>
        [FieldSpec(Field.General07, false, FieldTypeEnum.Decimal, true)]
        public decimal? General07
        {
            get;
            set;
        }

        /// <summary>
        /// General_08 欄位屬性
        /// </summary>
        [FieldSpec(Field.General08, false, FieldTypeEnum.Decimal, true)]
        public decimal? General08
        {
            get;
            set;
        }

        /// <summary>
        /// General_09 欄位屬性
        /// </summary>
        [FieldSpec(Field.General09, false, FieldTypeEnum.Decimal, true)]
        public decimal? General09
        {
            get;
            set;
        }

        /// <summary>
        /// General_10 欄位屬性
        /// </summary>
        [FieldSpec(Field.General10, false, FieldTypeEnum.Decimal, true)]
        public decimal? General10
        {
            get;
            set;
        }

        /// <summary>
        /// General_11 欄位屬性
        /// </summary>
        [FieldSpec(Field.General11, false, FieldTypeEnum.Decimal, true)]
        public decimal? General11
        {
            get;
            set;
        }

        /// <summary>
        /// General_12 欄位屬性
        /// </summary>
        [FieldSpec(Field.General12, false, FieldTypeEnum.Decimal, true)]
        public decimal? General12
        {
            get;
            set;
        }

        /// <summary>
        /// General_13 欄位屬性
        /// </summary>
        [FieldSpec(Field.General13, false, FieldTypeEnum.Decimal, true)]
        public decimal? General13
        {
            get;
            set;
        }

        /// <summary>
        /// General_14 欄位屬性
        /// </summary>
        [FieldSpec(Field.General14, false, FieldTypeEnum.Decimal, true)]
        public decimal? General14
        {
            get;
            set;
        }

        /// <summary>
        /// General_15 欄位屬性
        /// </summary>
        [FieldSpec(Field.General15, false, FieldTypeEnum.Decimal, true)]
        public decimal? General15
        {
            get;
            set;
        }

        /// <summary>
        /// General_16 欄位屬性
        /// </summary>
        [FieldSpec(Field.General16, false, FieldTypeEnum.Decimal, true)]
        public decimal? General16
        {
            get;
            set;
        }

        /// <summary>
        /// General_17 欄位屬性
        /// </summary>
        [FieldSpec(Field.General17, false, FieldTypeEnum.Decimal, true)]
        public decimal? General17
        {
            get;
            set;
        }

        /// <summary>
        /// General_18 欄位屬性
        /// </summary>
        [FieldSpec(Field.General18, false, FieldTypeEnum.Decimal, true)]
        public decimal? General18
        {
            get;
            set;
        }

        /// <summary>
        /// General_19 欄位屬性
        /// </summary>
        [FieldSpec(Field.General19, false, FieldTypeEnum.Decimal, true)]
        public decimal? General19
        {
            get;
            set;
        }

        /// <summary>
        /// General_20 欄位屬性
        /// </summary>
        [FieldSpec(Field.General20, false, FieldTypeEnum.Decimal, true)]
        public decimal? General20
        {
            get;
            set;
        }

        /// <summary>
        /// General_21 欄位屬性
        /// </summary>
        [FieldSpec(Field.General21, false, FieldTypeEnum.Decimal, true)]
        public decimal? General21
        {
            get;
            set;
        }

        /// <summary>
        /// General_22 欄位屬性
        /// </summary>
        [FieldSpec(Field.General22, false, FieldTypeEnum.Decimal, true)]
        public decimal? General22
        {
            get;
            set;
        }

        /// <summary>
        /// General_23 欄位屬性
        /// </summary>
        [FieldSpec(Field.General23, false, FieldTypeEnum.Decimal, true)]
        public decimal? General23
        {
            get;
            set;
        }

        /// <summary>
        /// General_24 欄位屬性
        /// </summary>
        [FieldSpec(Field.General24, false, FieldTypeEnum.Decimal, true)]
        public decimal? General24
        {
            get;
            set;
        }

        /// <summary>
        /// General_25 欄位屬性
        /// </summary>
        [FieldSpec(Field.General25, false, FieldTypeEnum.Decimal, true)]
        public decimal? General25
        {
            get;
            set;
        }

        /// <summary>
        /// General_26 欄位屬性
        /// </summary>
        [FieldSpec(Field.General26, false, FieldTypeEnum.Decimal, true)]
        public decimal? General26
        {
            get;
            set;
        }

        /// <summary>
        /// General_27 欄位屬性
        /// </summary>
        [FieldSpec(Field.General27, false, FieldTypeEnum.Decimal, true)]
        public decimal? General27
        {
            get;
            set;
        }

        /// <summary>
        /// General_28 欄位屬性
        /// </summary>
        [FieldSpec(Field.General28, false, FieldTypeEnum.Decimal, true)]
        public decimal? General28
        {
            get;
            set;
        }

        /// <summary>
        /// General_29 欄位屬性
        /// </summary>
        [FieldSpec(Field.General29, false, FieldTypeEnum.Decimal, true)]
        public decimal? General29
        {
            get;
            set;
        }

        /// <summary>
        /// General_30 欄位屬性
        /// </summary>
        [FieldSpec(Field.General30, false, FieldTypeEnum.Decimal, true)]
        public decimal? General30
        {
            get;
            set;
        }

        #region [MDY:20200810] M202008_02 Fix
        /// <summary>
        /// General_31 欄位屬性
        /// </summary>
        [FieldSpec(Field.General31, false, FieldTypeEnum.Decimal, true)]
        public decimal? General31
        {
            get;
            set;
        }

        /// <summary>
        /// General_32 欄位屬性
        /// </summary>
        [FieldSpec(Field.General32, false, FieldTypeEnum.Decimal, true)]
        public decimal? General32
        {
            get;
            set;
        }

        /// <summary>
        /// General_33 欄位屬性
        /// </summary>
        [FieldSpec(Field.General33, false, FieldTypeEnum.Decimal, true)]
        public decimal? General33
        {
            get;
            set;
        }

        /// <summary>
        /// General_34 欄位屬性
        /// </summary>
        [FieldSpec(Field.General34, false, FieldTypeEnum.Decimal, true)]
        public decimal? General34
        {
            get;
            set;
        }

        /// <summary>
        /// General_35 欄位屬性
        /// </summary>
        [FieldSpec(Field.General35, false, FieldTypeEnum.Decimal, true)]
        public decimal? General35
        {
            get;
            set;
        }

        /// <summary>
        /// General_36 欄位屬性
        /// </summary>
        [FieldSpec(Field.General36, false, FieldTypeEnum.Decimal, true)]
        public decimal? General36
        {
            get;
            set;
        }

        /// <summary>
        /// General_37 欄位屬性
        /// </summary>
        [FieldSpec(Field.General37, false, FieldTypeEnum.Decimal, true)]
        public decimal? General37
        {
            get;
            set;
        }

        /// <summary>
        /// General_38 欄位屬性
        /// </summary>
        [FieldSpec(Field.General38, false, FieldTypeEnum.Decimal, true)]
        public decimal? General38
        {
            get;
            set;
        }

        /// <summary>
        /// General_39 欄位屬性
        /// </summary>
        [FieldSpec(Field.General39, false, FieldTypeEnum.Decimal, true)]
        public decimal? General39
        {
            get;
            set;
        }

        /// <summary>
        /// General_40 欄位屬性
        /// </summary>
        [FieldSpec(Field.General40, false, FieldTypeEnum.Decimal, true)]
        public decimal? General40
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Order_Id 欄位屬性
        /// </summary>
        [FieldSpec(Field.OrderId, false, FieldTypeEnum.Decimal, true)]
        public decimal? OrderId
        {
            get;
            set;
        }

        /// <summary>
        /// status 欄位屬性
        /// </summary>
        [FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// crt_date 欄位屬性
        /// </summary>
        [FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
        public DateTime CrtDate
        {
            get;
            set;
        }

        /// <summary>
        /// crt_user 欄位屬性
        /// </summary>
        [FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
        public string CrtUser
        {
            get;
            set;
        }

        /// <summary>
        /// mdy_date 欄位屬性
        /// </summary>
        [FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? MdyDate
        {
            get;
            set;
        }

        /// <summary>
        /// mdy_user 欄位屬性
        /// </summary>
        [FieldSpec(Field.MdyUser, false, FieldTypeEnum.VarChar, 20, true)]
        public string MdyUser
        {
            get;
            set;
        }

        /// <summary>
        /// College_Name 欄位屬性
        /// </summary>
        [FieldSpec(Field.CollegeName, false, FieldTypeEnum.VarChar, 40, true)]
        public string CollegeName
        {
            get;
            set;
        }

        #region [MDY:20200810] M202008_02 科系名稱長度放大到40個中文字
        /// <summary>
        /// 系所名稱
        /// </summary>
        [FieldSpec(Field.MajorName, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MajorName
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 班別名稱
        /// </summary>
        [FieldSpec(Field.ClassName, false, FieldTypeEnum.VarChar, 40, true)]
        public string ClassName
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
