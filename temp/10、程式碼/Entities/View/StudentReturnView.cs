using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 查詢學分費退費資料用的 View 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public class StudentReturnView : StudentReturnEntity
    {
        #region [Old]
//        protected const string VIEWSQL = @"
//SELECT A.*, ISNULL(B.Cancel_Flag, ''), ISNULL(B.Receive_Way, ''), ISNULL(B.Account_Date, '')
//     , ISNULL((SELECT TOP 1 Stu_Name FROM Student_Master AS SM WHERE SM.Receive_Type = A.Receive_Type AND SM.Dep_Id = A.Dep_Id AND SM.Stu_Id = A.Stu_Id), '') AS Stu_Name
//  FROM Student_Return AS A 
//  LEFT JOIN Student_Receive AS B 
//    ON B.Receive_Type = A.Receive_Type AND B.Year_Id = A.Year_Id AND B.Term_Id = A.Term_Id 
//   AND B.Dep_Id = A.Dep_Id AND B.Receive_Id = A.Receive_Id AND A.Stu_Id = B.Stu_Id
// WHERE ISNULL(Receive_Way, '') != '' AND ISNULL(Account_Date, '') != '' ";
        #endregion

        protected const string VIEWSQL = @"SELECT A.*
     , ISNULL((SELECT TOP 1 Stu_Name FROM Student_Master AS SM WHERE SM.Receive_Type = A.Receive_Type AND SM.Dep_Id = A.Dep_Id AND SM.Stu_Id = A.Stu_Id), '') AS Stu_Name
  FROM Student_Return AS A ";

        #region Field Name Const Class
        /// <summary>
        /// 查詢學分費退費資料用的 View 欄位名稱定義抽象類別
        /// </summary>
        public abstract new class Field : StudentReturnEntity.Field
        {
            #region 代碼名稱
            /// <summary>
            /// 學生姓名
            /// </summary>
            public const string StuName = "Stu_Name";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 學生繳費資料 + 學生資料主檔 + 學分費退費資料 View 欄位名稱定義抽象類別
        /// </summary>
        public StudentReturnView()
            : base()
        {
        }
        #endregion

        #region Property
        #region 代碼名稱
        /// <summary>
        /// 學生姓名
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
