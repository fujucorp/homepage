using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 銷帳問題檔 + 學生資料主檔 + 學生繳費資料檔
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class ProblemListView : ProblemListEntity
    {
        #region [MDY:20191014] M201910_01 (2019擴充案+小修正) 改成學生資料以學年遞減排序
        #region [Old]
        //        protected const string VIEWSQL = @"SELECT SR.Stu_Id, SM.Stu_Name, PL.* from Problem_List PL 
//        inner join Student_Receive SR on PL.Cancel_No=SR.Cancel_No and PL.Receive_Type = SR.Receive_Type 
//        inner join Student_Master SM on SR.Stu_Id=SM.Stu_Id and SR.Receive_Type=SM.Receive_Type ";
        #endregion

        #region [OLD]
//        public const string VIEWSQL = @"
//SELECT PL.* 
//     , ISNULL((SELECT Channel_Name FROM Channel_Set AS CS WHERE CS.Channel_Id = PL.Receive_Way), '') AS Receive_Way_Name
//     , ISNULL((SELECT TOP 1 Stu_Id FROM Student_Receive AS SR WHERE SR.Receive_Type = PL.Receive_Type AND SR.Cancel_No = PL.Cancel_No), '') AS Stu_Id
//     , ISNULL((SELECT TOP 1 (SELECT SM.Stu_Name FROM Student_Master AS SM WHERE SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id) AS Stu_Name
//                 FROM Student_Receive AS SR WHERE SR.Receive_Type = PL.Receive_Type AND SR.Cancel_No = PL.Cancel_No), '') AS Stu_Name
//  FROM Problem_List PL";
        #endregion

        public const string VIEWSQL = @"
SELECT PL.* 
     , ISNULL((SELECT Channel_Name FROM Channel_Set AS CS WHERE CS.Channel_Id = PL.Receive_Way), '') AS Receive_Way_Name 
     , ISNULL((SELECT TOP 1 Stu_Id FROM Student_Receive AS SR WHERE SR.Receive_Type = PL.Receive_Type AND SR.Cancel_No = PL.Cancel_No ORDER BY CAST(SR.Year_Id AS INT) DESC), '') AS Stu_Id 
     , ISNULL((SELECT TOP 1 (SELECT SM.Stu_Name FROM Student_Master AS SM WHERE SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id) AS Stu_Name 
                 FROM Student_Receive AS SR WHERE SR.Receive_Type = PL.Receive_Type AND SR.Cancel_No = PL.Cancel_No ORDER BY CAST(SR.Year_Id AS INT) DESC), '') AS Stu_Name 
  FROM Problem_List PL
";
        #endregion

        #region Field Name Const Class
        /// <summary>
        /// 銷帳問題檔 + 學生資料主檔 + 學生繳費資料檔 欄位名稱定義抽象類別
        /// </summary>
        public abstract new class Field : ProblemListEntity.Field
        {
            #region 代碼名稱
            /// <summary>
            /// 學號 欄位名稱常數定義
            /// </summary>
            public const string ReceiveWayName = "Receive_Way_Name";

			/// <summary>
			/// 學號 欄位名稱常數定義
			/// </summary>
			public const string StuId = "Stu_Id";

			/// <summary>
			/// 姓名 欄位名稱常數定義
			/// </summary>
			public const string StuName = "Stu_Name";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構銷帳問題檔 + 學生資料主檔 + 學生繳費資料檔 View 的資料承載類別
        /// </summary>
        public ProblemListView()
            : base()
        {
        }
        #endregion

        #region Property
        #region 代碼名稱
        private string _ReceiveWayName = null;
        /// <summary>
        /// 代收管道名稱
        /// </summary>
        [FieldSpec(Field.ReceiveWayName, true, FieldTypeEnum.NVarChar, 50, false)]
        public string ReceiveWayName
        {
            get
            {
                return _ReceiveWayName;
            }
            set
            {
                _ReceiveWayName = value == null ? null : value.Trim();
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

        /// <summary>
        /// 姓名
        /// </summary>
        [FieldSpec(Field.StuName, false, FieldTypeEnum.NVarChar, 60, true)]
        public string StuName
        {
            get;
            set;
        }
        #endregion
        #endregion

    }
}
