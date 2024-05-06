using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 學生繳費資料 + 學生基本資料 + 院別、系所、班別、減免類別、住宿項目、就貸項目、身分註記1 ~ 身分註記6 名稱 View 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class StudentReceiveView4 : StudentReceiveEntity
    {
        #region [Old]
//        public const string VIEWSQL = @"SELECT SR.*
//     , SM.[Stu_Name], SM.[Stu_Birthday], SM.[Id_Number], SM.[Stu_Tel], SM.[Stu_Addcode], SM.[Stu_Address], SM.[Stu_Email]
//     , ISNULL((SELECT [Term_Name]       FROM [Term_List]      AS TL  WHERE TL.[Receive_Type]  = SR.[Receive_Type] AND TL.[Year_Id]  = SR.[Year_Id] AND TL.[Term_Id]  = SR.[Term_Id]), '') AS [Term_Name]
//   --, ISNULL((SELECT [Dep_Name]        FROM [Dep_List]       AS DL  WHERE DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id]), '') AS [Dep_Name]
//     , ISNULL((SELECT [Dept_Name]       FROM [Dept_List]      AS DTL WHERE DTL.[Receive_Type] = SR.[Receive_Type] AND DTL.[Year_Id] = SR.[Year_Id] AND DTL.[Term_Id] = SR.[Term_Id] AND DTL.[Dept_Id] = SR.[Dept_Id]), '') AS [Dept_Name]
//     , ISNULL((SELECT [Receive_Name]    FROM [Receive_List]   AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Receive_Id]   = SR.[Receive_Id]), '') AS [Receive_Name]
//     , ISNULL((SELECT [College_Name]    FROM [College_List]   AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[College_Id]   = SR.[College_Id]), '') AS [College_Name]
//     , ISNULL((SELECT [Major_Name]      FROM [Major_List]     AS ML  WHERE ML.[Receive_Type]  = SR.[Receive_Type] AND ML.[Year_Id]  = SR.[Year_Id] AND ML.[Term_Id]  = SR.[Term_Id] AND ML.[Dep_Id]   = SR.[Dep_Id] AND ML.[Major_Id]     = SR.[Major_Id]), '') AS [Major_Name]
//     , ISNULL((SELECT [Class_Name]      FROM [Class_List]     AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[Class_Id]     = SR.[Class_Id]), '') AS [Class_Name]
//     , ISNULL((SELECT [Reduce_Name]     FROM [Reduce_List]    AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Reduce_Id]    = SR.[Reduce_Id]), '') AS [Reduce_Name]
//     , ISNULL((SELECT [Dorm_Name]       FROM [Dorm_List]      AS DL  WHERE DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id] AND DL.[Dorm_Id]      = SR.[Dorm_Id]), '') AS [Dorm_Name]
//     , ISNULL((SELECT [Loan_Name]       FROM [Loan_List]      AS LL  WHERE LL.[Receive_Type]  = SR.[Receive_Type] AND LL.[Year_Id]  = SR.[Year_Id] AND LL.[Term_Id]  = SR.[Term_Id] AND LL.[Dep_Id]   = SR.[Dep_Id] AND LL.[Loan_Id]      = SR.[Loan_Id]), '') AS [Loan_Name]
//     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List1] AS IL1 WHERE IL1.[Receive_Type] = SR.[Receive_Type] AND IL1.[Year_Id] = SR.[Year_Id] AND IL1.[Term_Id] = SR.[Term_Id] AND IL1.[Dep_Id]  = SR.[Dep_Id] AND IL1.[Identify_Id] = SR.[Identify_Id01]), '') AS [Identify_Name01]
//     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List2] AS IL2 WHERE IL2.[Receive_Type] = SR.[Receive_Type] AND IL2.[Year_Id] = SR.[Year_Id] AND IL2.[Term_Id] = SR.[Term_Id] AND IL2.[Dep_Id]  = SR.[Dep_Id] AND IL2.[Identify_Id] = SR.[Identify_Id02]), '') AS [Identify_Name02]
//     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List3] AS IL3 WHERE IL3.[Receive_Type] = SR.[Receive_Type] AND IL3.[Year_Id] = SR.[Year_Id] AND IL3.[Term_Id] = SR.[Term_Id] AND IL3.[Dep_Id]  = SR.[Dep_Id] AND IL3.[Identify_Id] = SR.[Identify_Id03]), '') AS [Identify_Name03]
//     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List4] AS IL4 WHERE IL4.[Receive_Type] = SR.[Receive_Type] AND IL4.[Year_Id] = SR.[Year_Id] AND IL4.[Term_Id] = SR.[Term_Id] AND IL4.[Dep_Id]  = SR.[Dep_Id] AND IL4.[Identify_Id] = SR.[Identify_Id04]), '') AS [Identify_Name04]
//     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List5] AS IL5 WHERE IL5.[Receive_Type] = SR.[Receive_Type] AND IL5.[Year_Id] = SR.[Year_Id] AND IL5.[Term_Id] = SR.[Term_Id] AND IL5.[Dep_Id]  = SR.[Dep_Id] AND IL5.[Identify_Id] = SR.[Identify_Id05]), '') AS [Identify_Name05]
//     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List6] AS IL6 WHERE IL6.[Receive_Type] = SR.[Receive_Type] AND IL6.[Year_Id] = SR.[Year_Id] AND IL6.[Term_Id] = SR.[Term_Id] AND IL6.[Dep_Id]  = SR.[Dep_Id] AND IL6.[Identify_Id] = SR.[Identify_Id06]), '') AS [Identify_Name06]
//     , ISNULL((SELECT [Channel_Name]    FROM [Channel_Set]    AS CS  WHERE CS.[Channel_Id] = SR.[Receive_Way]), '') AS [Receive_Way_Name]
//  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS SR
//  JOIN [" + StudentMasterEntity.TABLE_NAME + @"] AS SM ON SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id ";
        #endregion

        public const string VIEWSQL = @"SELECT SR.*
     , SM.[Stu_Name], SM.[Stu_Birthday], SM.[Id_Number], SM.[Stu_Tel], SM.[Stu_Addcode], SM.[Stu_Address], SM.[Stu_Email], SM.[Stu_Parent]
     , ISNULL((SELECT [Year_Name]       FROM [Year_List]      AS YL  WHERE YL.[Year_Id]  = SR.[Year_Id]), '') AS [Year_Name]
     , ISNULL((SELECT [Term_Name]       FROM [Term_List]      AS TL  WHERE TL.[Receive_Type]  = SR.[Receive_Type] AND TL.[Year_Id]  = SR.[Year_Id] AND TL.[Term_Id]  = SR.[Term_Id]), '') AS [Term_Name]
   --, ISNULL((SELECT [Dep_Name]        FROM [Dep_List]       AS DL  WHERE DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id]), '') AS [Dep_Name]
     , ISNULL((SELECT [Dept_Name]       FROM [Dept_List]      AS DTL WHERE DTL.[Receive_Type] = SR.[Receive_Type] AND DTL.[Year_Id] = SR.[Year_Id] AND DTL.[Term_Id] = SR.[Term_Id] AND DTL.[Dept_Id] = SR.[Dept_Id]), '') AS [Dept_Name]
     , ISNULL((SELECT [Receive_Name]    FROM [Receive_List]   AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Receive_Id]   = SR.[Receive_Id]), '') AS [Receive_Name]
     , ISNULL((SELECT [College_Name]    FROM [College_List]   AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[College_Id]   = SR.[College_Id]), '') AS [College_Name]
     , ISNULL((SELECT [Major_Name]      FROM [Major_List]     AS ML  WHERE ML.[Receive_Type]  = SR.[Receive_Type] AND ML.[Year_Id]  = SR.[Year_Id] AND ML.[Term_Id]  = SR.[Term_Id] AND ML.[Dep_Id]   = SR.[Dep_Id] AND ML.[Major_Id]     = SR.[Major_Id]), '') AS [Major_Name]
     , ISNULL((SELECT [Class_Name]      FROM [Class_List]     AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[Class_Id]     = SR.[Class_Id]), '') AS [Class_Name]
     , ISNULL((SELECT [Reduce_Name]     FROM [Reduce_List]    AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Reduce_Id]    = SR.[Reduce_Id]), '') AS [Reduce_Name]
     , ISNULL((SELECT [Dorm_Name]       FROM [Dorm_List]      AS DL  WHERE DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id] AND DL.[Dorm_Id]      = SR.[Dorm_Id]), '') AS [Dorm_Name]
     , ISNULL((SELECT [Loan_Name]       FROM [Loan_List]      AS LL  WHERE LL.[Receive_Type]  = SR.[Receive_Type] AND LL.[Year_Id]  = SR.[Year_Id] AND LL.[Term_Id]  = SR.[Term_Id] AND LL.[Dep_Id]   = SR.[Dep_Id] AND LL.[Loan_Id]      = SR.[Loan_Id]), '') AS [Loan_Name]
     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List1] AS IL1 WHERE IL1.[Receive_Type] = SR.[Receive_Type] AND IL1.[Year_Id] = SR.[Year_Id] AND IL1.[Term_Id] = SR.[Term_Id] AND IL1.[Dep_Id]  = SR.[Dep_Id] AND IL1.[Identify_Id] = SR.[Identify_Id01]), '') AS [Identify_Name01]
     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List2] AS IL2 WHERE IL2.[Receive_Type] = SR.[Receive_Type] AND IL2.[Year_Id] = SR.[Year_Id] AND IL2.[Term_Id] = SR.[Term_Id] AND IL2.[Dep_Id]  = SR.[Dep_Id] AND IL2.[Identify_Id] = SR.[Identify_Id02]), '') AS [Identify_Name02]
     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List3] AS IL3 WHERE IL3.[Receive_Type] = SR.[Receive_Type] AND IL3.[Year_Id] = SR.[Year_Id] AND IL3.[Term_Id] = SR.[Term_Id] AND IL3.[Dep_Id]  = SR.[Dep_Id] AND IL3.[Identify_Id] = SR.[Identify_Id03]), '') AS [Identify_Name03]
     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List4] AS IL4 WHERE IL4.[Receive_Type] = SR.[Receive_Type] AND IL4.[Year_Id] = SR.[Year_Id] AND IL4.[Term_Id] = SR.[Term_Id] AND IL4.[Dep_Id]  = SR.[Dep_Id] AND IL4.[Identify_Id] = SR.[Identify_Id04]), '') AS [Identify_Name04]
     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List5] AS IL5 WHERE IL5.[Receive_Type] = SR.[Receive_Type] AND IL5.[Year_Id] = SR.[Year_Id] AND IL5.[Term_Id] = SR.[Term_Id] AND IL5.[Dep_Id]  = SR.[Dep_Id] AND IL5.[Identify_Id] = SR.[Identify_Id05]), '') AS [Identify_Name05]
     , ISNULL((SELECT [Identify_Name]   FROM [Identify_List6] AS IL6 WHERE IL6.[Receive_Type] = SR.[Receive_Type] AND IL6.[Year_Id] = SR.[Year_Id] AND IL6.[Term_Id] = SR.[Term_Id] AND IL6.[Dep_Id]  = SR.[Dep_Id] AND IL6.[Identify_Id] = SR.[Identify_Id06]), '') AS [Identify_Name06]
     , ISNULL((SELECT [Channel_Name]    FROM [Channel_Set]    AS CS  WHERE CS.[Channel_Id] = SR.[Receive_Way]), '') AS [Receive_Way_Name]
  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS SR
  JOIN [" + StudentMasterEntity.TABLE_NAME + @"] AS SM ON SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id ";

        #region Field Name Const Class
        /// <summary>
        /// 學生繳費資料 + 院別、系所、班別、減免類別、住宿項目、就貸項目、身分註記1 ~ 身分註記6 名稱 View 欄位名稱定義抽象類別
        /// </summary>
        public abstract new class Field : StudentReceiveEntity.Field
        {
            #region 學生基本資料相關
            /// <summary>
            /// 學生姓名
            /// </summary>
            public const string StuName = "Stu_Name";

            /// <summary>
            /// 學生生日 (民國年月日 7 碼)
            /// </summary>
            public const string StuBirthday = "Stu_Birthday";

            /// <summary>
            /// 學生身分證字號
            /// </summary>
            public const string StuIdNumber = "Id_Number";

            /// <summary>
            /// 學生電話
            /// </summary>
            public const string StuTel = "Stu_Tel";

            /// <summary>
            /// 學生郵遞區號
            /// </summary>
            public const string StuZipCode = "Stu_Addcode";

            /// <summary>
            /// 學生地址
            /// </summary>
            public const string StuAddress = "Stu_Address";

            /// <summary>
            /// 學生 EMail
            /// </summary>
            public const string StuEmail = "Stu_Email";

            /// <summary>
            /// 家長名稱
            /// </summary>
            public const string StuParent = "Stu_Parent";
            #endregion

            #region 代碼名稱
            /// <summary>
            /// 學年名稱 欄位名稱常數定義
            /// </summary>
            public const string YearName = "Year_Name";

            /// <summary>
            /// 學期名稱 欄位名稱常數定義
            /// </summary>
            public const string TermName = "Term_Name";

            #region [Old] 土銀不使用
            ///// <summary>
            ///// 部別名稱 欄位名稱常數定義
            ///// </summary>
            //public const string DepName = "Dep_Name";
            #endregion

            #region [New] 土銀專用部別
            /// <summary>
            /// 部別名稱 (土銀專用) 欄位名稱常數定義
            /// </summary>
            public const string DeptName = "Dept_Name";
            #endregion

            /// <summary>
            /// 費用名稱 欄位名稱常數定義
            /// </summary>
            public const string ReceiveName = "Receive_Name";

            /// <summary>
            /// 院別名稱 欄位名稱常數定義
            /// </summary>
            public const string CollegeName = "College_Name";

            /// <summary>
            /// 系所名稱 欄位名稱常數定義
            /// </summary>
            public const string MajorName = "Major_Name";

            /// <summary>
            /// 班別名稱 欄位名稱常數定義
            /// </summary>
            public const string ClassName = "Class_Name";

            /// <summary>
            /// 減免類別名稱 欄位名稱常數定義
            /// </summary>
            public const string ReduceName = "Reduce_Name";

            /// <summary>
            /// 住宿項目名稱 欄位名稱常數定義
            /// </summary>
            public const string DormName = "Dorm_Name";

            /// <summary>
            /// 就貸項目名稱 欄位名稱常數定義
            /// </summary>
            public const string LoanName = "Loan_Name";

            /// <summary>
            /// 身分註記01項目名稱 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName01 = "Identify_Name01";

            /// <summary>
            /// 身分註記02項目名稱 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName02 = "Identify_Name02";

            /// <summary>
            /// 身分註記03項目名稱 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName03 = "Identify_Name03";

            /// <summary>
            /// 身分註記04項目名稱 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName04 = "Identify_Name04";

            /// <summary>
            /// 身分註記05項目名稱 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName05 = "Identify_Name05";

            /// <summary>
            /// 身分註記06項目名稱 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName06 = "Identify_Name06";

            /// <summary>
            /// 繳費方式名稱 欄位名稱常數定義
            /// </summary>
            public const string ReceiveWayName = "Receive_Way_Name";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構學生繳費資料 + 院別、系所、班別、減免類別、住宿項目、就貸項目、身分註記1 ~ 身分註記6 名稱 View 的資料承載類別
        /// </summary>
        public StudentReceiveView4()
            : base()
        {
        }
        #endregion

        #region Property
        #region 學生基本資料相關
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
        /// 學生生日 (民國年月日 7 碼)
        /// </summary>
        [FieldSpec(Field.StuBirthday, false, FieldTypeEnum.Char, 7, true)]
        public string StuBirthday
        {
            get;
            set;
        }

        /// <summary>
        /// 學生身分證字號
        /// </summary>
        [FieldSpec(Field.StuIdNumber, false, FieldTypeEnum.Char, 12, true)]
        public string StuIdNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 學生電話
        /// </summary>
        [FieldSpec(Field.StuTel, false, FieldTypeEnum.VarChar, 14, true)]
        public string StuTel
        {
            get;
            set;
        }

        /// <summary>
        /// 學生郵遞區號
        /// </summary>
        [FieldSpec(Field.StuZipCode, false, FieldTypeEnum.VarChar, 5, true)]
        public string StuZipCode
        {
            get;
            set;
        }

        /// <summary>
        /// 學生地址
        /// </summary>
        [FieldSpec(Field.StuAddress, false, FieldTypeEnum.VarChar, 100, true)]
        public string StuAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 學生EMail
        /// </summary>
        [FieldSpec(Field.StuEmail, false, FieldTypeEnum.VarChar, 50, true)]
        public string StuEmail
        {
            get;
            set;
        }

        /// <summary>
        /// 家長名稱
        /// </summary>
        [FieldSpec(Field.StuParent, false, FieldTypeEnum.NVarChar, 60, true)]
        public string StuParent
        {
            get;
            set;
        }
        #endregion

        #region 代碼名稱
        /// <summary>
        /// 學年名稱
        /// </summary>
        [FieldSpec(Field.YearName, false, FieldTypeEnum.VarChar, 20, true)]
        public string YearName
        {
            get;
            set;
        }

        /// <summary>
        /// 學期名稱
        /// </summary>
        [FieldSpec(Field.TermName, false, FieldTypeEnum.VarChar, 20, true)]
        public string TermName
        {
            get;
            set;
        }

        #region [Old] 土銀不使用
        ///// <summary>
        ///// 部別名稱
        ///// </summary>
        //[FieldSpec(Field.DepName, false, FieldTypeEnum.VarChar, 20, true)]
        //public string DepName
        //{
        //    get;
        //    set;
        //}
        #endregion

        #region [New] 土銀專用部別
        /// <summary>
        /// 部別名稱 (土銀專用)
        /// </summary>
        [FieldSpec(Field.DeptName, false, FieldTypeEnum.VarChar, 20, true)]
        public string DeptName
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 費用別名稱
        /// </summary>
        [FieldSpec(Field.ReceiveName, false, FieldTypeEnum.VarChar, 40, true)]
        public string ReceiveName
        {
            get;
            set;
        }

        /// <summary>
        /// 院別名稱
        /// </summary>
        [FieldSpec(Field.CollegeName, false, FieldTypeEnum.VarChar, 40, true)]
        public string CollegeName
        {
            get;
            set;
        }

        /// <summary>
        /// 系所名稱
        /// </summary>
        [FieldSpec(Field.MajorName, false, FieldTypeEnum.VarChar, 40, true)]
        public string MajorName
        {
            get;
            set;
        }

        /// <summary>
        /// 班別名稱
        /// </summary>
        [FieldSpec(Field.ClassName, false, FieldTypeEnum.VarChar, 40, true)]
        public string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 減免類別名稱
        /// </summary>
        [FieldSpec(Field.ReduceName, false, FieldTypeEnum.VarChar, 40, true)]
        public string ReduceName
        {
            get;
            set;
        }

        /// <summary>
        /// 住宿項目名稱
        /// </summary>
        [FieldSpec(Field.DormName, false, FieldTypeEnum.VarChar, 40, true)]
        public string DormName
        {
            get;
            set;
        }

        /// <summary>
        /// 就貸項目名稱
        /// </summary>
        [FieldSpec(Field.LoanName, false, FieldTypeEnum.VarChar, 40, true)]
        public string LoanName
        {
            get;
            set;
        }

        /// <summary>
        /// 身分註記01項目名稱
        /// </summary>
        [FieldSpec(Field.IdentifyName01, false, FieldTypeEnum.NVarChar, 40, true)]
        public string IdentifyName01
        {
            get;
            set;
        }

        /// <summary>
        /// 身分註記02項目名稱
        /// </summary>
        [FieldSpec(Field.IdentifyName02, false, FieldTypeEnum.NVarChar, 40, true)]
        public string IdentifyName02
        {
            get;
            set;
        }

        /// <summary>
        /// 身分註記03項目名稱
        /// </summary>
        [FieldSpec(Field.IdentifyName03, false, FieldTypeEnum.NVarChar, 40, true)]
        public string IdentifyName03
        {
            get;
            set;
        }

        /// <summary>
        /// 身分註記04項目名稱
        /// </summary>
        [FieldSpec(Field.IdentifyName04, false, FieldTypeEnum.NVarChar, 40, true)]
        public string IdentifyName04
        {
            get;
            set;
        }

        /// <summary>
        /// 身分註記05項目名稱
        /// </summary>
        [FieldSpec(Field.IdentifyName05, false, FieldTypeEnum.NVarChar, 40, true)]
        public string IdentifyName05
        {
            get;
            set;
        }

        /// <summary>
        /// 身分註記06項目名稱
        /// </summary>
        [FieldSpec(Field.IdentifyName06, false, FieldTypeEnum.NVarChar, 40, true)]
        public string IdentifyName06
        {
            get;
            set;
        }

        /// <summary>
        /// 繳費方式名稱
        /// </summary>
        [FieldSpec(Field.ReceiveWayName, false, FieldTypeEnum.NVarChar, 50, true)]
        public string ReceiveWayName
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Readonly Property
        /// <summary>
        /// 年級名稱
        /// </summary>
        [XmlIgnore]
        public string StuGradeName
        {
            get
            {
                return GradeCodeTexts.GetText(this.StuGrade);
            }
        }
        #endregion
    }
}
