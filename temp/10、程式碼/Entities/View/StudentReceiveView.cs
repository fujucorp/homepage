using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 學生繳費資料 + 繳費期限 x 3 + 開關列印日期 + 院別、系所、班別、減免類別、住宿項目、就貸項目、身分註記1 ~ 身分註記6 名稱 View 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class StudentReceiveView : StudentReceiveEntity
    {
        #region [Old]
        //        protected const string VIEWSQL = @"SELECT *
        //     , ISNULL((SELECT [Stu_Name]        FROM [Student_Master] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.Stu_Id = SR.Stu_Id), '') AS [Stu_Name]
        //     , ISNULL((SELECT [Bill_Valid_Date] FROM [School_Rid]     AS SRI WHERE SRI.[Receive_Type] = SR.[Receive_Type] AND SRI.[Year_Id] = SR.[Year_Id] AND SRI.[Term_Id] = SR.[Term_Id] AND SRI.[Dep_Id] = SR.[Dep_Id] AND SRI.[Receive_Id] = SR.[Receive_Id]), '') AS [Bill_Valid_Date]
        //     , ISNULL((SELECT [Bill_Close_Date] FROM [School_Rid]     AS SRI WHERE SRI.[Receive_Type] = SR.[Receive_Type] AND SRI.[Year_Id] = SR.[Year_Id] AND SRI.[Term_Id] = SR.[Term_Id] AND SRI.[Dep_Id] = SR.[Dep_Id] AND SRI.[Receive_Id] = SR.[Receive_Id]), '') AS [Bill_Close_Date]
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
        //  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS SR";
        #endregion

        #region [MDY:202203XX] 2022擴充案 增加學年名稱 (Year_List.Year_Name) 欄位
        #region [MDY:2018xxxx] 增加 列印收據關閉日 (School_Rid.Invoice_Close_Date) 欄位
        #region [OLD]
        //        protected const string VIEWSQL = @"SELECT SR.*
        //     , ISNULL((SELECT [Stu_Name] FROM [" + StudentMasterEntity.TABLE_NAME + @"] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
        //     , ISNULL(RI.[Bill_Valid_Date], '') AS [Bill_Valid_Date], ISNULL(RI.[Bill_Close_Date], '') AS [Bill_Close_Date]
        //     , ISNULL(RI.[Pay_Date], '') AS Pay_Due_Date1, ISNULL(RI.[Pay_Due_Date2], '') AS Pay_Due_Date2, ISNULL(RI.[Pay_Due_Date3], '') AS Pay_Due_Date3
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
        //  LEFT JOIN [" + SchoolRidEntity.TABLE_NAME + @"] AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]";
        #endregion

        #region [OLD]
        //      protected const string VIEWSQL = @"SELECT SR.*
        //   , ISNULL((SELECT [Stu_Name] FROM [" + StudentMasterEntity.TABLE_NAME + @"] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
        //   , ISNULL(RI.[Bill_Valid_Date], '') AS [Bill_Valid_Date], ISNULL(RI.[Bill_Close_Date], '') AS [Bill_Close_Date], ISNULL(RI.[Invoice_Close_Date], '') AS [Invoice_Close_Date]
        //   , ISNULL(RI.[Pay_Date], '') AS Pay_Due_Date1, ISNULL(RI.[Pay_Due_Date2], '') AS Pay_Due_Date2, ISNULL(RI.[Pay_Due_Date3], '') AS Pay_Due_Date3
        //   , ISNULL((SELECT [Term_Name]       FROM [Term_List]      AS TL  WHERE TL.[Receive_Type]  = SR.[Receive_Type] AND TL.[Year_Id]  = SR.[Year_Id] AND TL.[Term_Id]  = SR.[Term_Id]), '') AS [Term_Name]
        // --, ISNULL((SELECT [Dep_Name]        FROM [Dep_List]       AS DL  WHERE DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id]), '') AS [Dep_Name]
        //   , ISNULL((SELECT [Dept_Name]       FROM [Dept_List]      AS DTL WHERE DTL.[Receive_Type] = SR.[Receive_Type] AND DTL.[Year_Id] = SR.[Year_Id] AND DTL.[Term_Id] = SR.[Term_Id] AND DTL.[Dept_Id] = SR.[Dept_Id]), '') AS [Dept_Name]
        //   , ISNULL((SELECT [Receive_Name]    FROM [Receive_List]   AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Receive_Id]   = SR.[Receive_Id]), '') AS [Receive_Name]
        //   , ISNULL((SELECT [College_Name]    FROM [College_List]   AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[College_Id]   = SR.[College_Id]), '') AS [College_Name]
        //   , ISNULL((SELECT [Major_Name]      FROM [Major_List]     AS ML  WHERE ML.[Receive_Type]  = SR.[Receive_Type] AND ML.[Year_Id]  = SR.[Year_Id] AND ML.[Term_Id]  = SR.[Term_Id] AND ML.[Dep_Id]   = SR.[Dep_Id] AND ML.[Major_Id]     = SR.[Major_Id]), '') AS [Major_Name]
        //   , ISNULL((SELECT [Class_Name]      FROM [Class_List]     AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[Class_Id]     = SR.[Class_Id]), '') AS [Class_Name]
        //   , ISNULL((SELECT [Reduce_Name]     FROM [Reduce_List]    AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Reduce_Id]    = SR.[Reduce_Id]), '') AS [Reduce_Name]
        //   , ISNULL((SELECT [Dorm_Name]       FROM [Dorm_List]      AS DL  WHERE DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id] AND DL.[Dorm_Id]      = SR.[Dorm_Id]), '') AS [Dorm_Name]
        //   , ISNULL((SELECT [Loan_Name]       FROM [Loan_List]      AS LL  WHERE LL.[Receive_Type]  = SR.[Receive_Type] AND LL.[Year_Id]  = SR.[Year_Id] AND LL.[Term_Id]  = SR.[Term_Id] AND LL.[Dep_Id]   = SR.[Dep_Id] AND LL.[Loan_Id]      = SR.[Loan_Id]), '') AS [Loan_Name]
        //   , ISNULL((SELECT [Identify_Name]   FROM [Identify_List1] AS IL1 WHERE IL1.[Receive_Type] = SR.[Receive_Type] AND IL1.[Year_Id] = SR.[Year_Id] AND IL1.[Term_Id] = SR.[Term_Id] AND IL1.[Dep_Id]  = SR.[Dep_Id] AND IL1.[Identify_Id] = SR.[Identify_Id01]), '') AS [Identify_Name01]
        //   , ISNULL((SELECT [Identify_Name]   FROM [Identify_List2] AS IL2 WHERE IL2.[Receive_Type] = SR.[Receive_Type] AND IL2.[Year_Id] = SR.[Year_Id] AND IL2.[Term_Id] = SR.[Term_Id] AND IL2.[Dep_Id]  = SR.[Dep_Id] AND IL2.[Identify_Id] = SR.[Identify_Id02]), '') AS [Identify_Name02]
        //   , ISNULL((SELECT [Identify_Name]   FROM [Identify_List3] AS IL3 WHERE IL3.[Receive_Type] = SR.[Receive_Type] AND IL3.[Year_Id] = SR.[Year_Id] AND IL3.[Term_Id] = SR.[Term_Id] AND IL3.[Dep_Id]  = SR.[Dep_Id] AND IL3.[Identify_Id] = SR.[Identify_Id03]), '') AS [Identify_Name03]
        //   , ISNULL((SELECT [Identify_Name]   FROM [Identify_List4] AS IL4 WHERE IL4.[Receive_Type] = SR.[Receive_Type] AND IL4.[Year_Id] = SR.[Year_Id] AND IL4.[Term_Id] = SR.[Term_Id] AND IL4.[Dep_Id]  = SR.[Dep_Id] AND IL4.[Identify_Id] = SR.[Identify_Id04]), '') AS [Identify_Name04]
        //   , ISNULL((SELECT [Identify_Name]   FROM [Identify_List5] AS IL5 WHERE IL5.[Receive_Type] = SR.[Receive_Type] AND IL5.[Year_Id] = SR.[Year_Id] AND IL5.[Term_Id] = SR.[Term_Id] AND IL5.[Dep_Id]  = SR.[Dep_Id] AND IL5.[Identify_Id] = SR.[Identify_Id05]), '') AS [Identify_Name05]
        //   , ISNULL((SELECT [Identify_Name]   FROM [Identify_List6] AS IL6 WHERE IL6.[Receive_Type] = SR.[Receive_Type] AND IL6.[Year_Id] = SR.[Year_Id] AND IL6.[Term_Id] = SR.[Term_Id] AND IL6.[Dep_Id]  = SR.[Dep_Id] AND IL6.[Identify_Id] = SR.[Identify_Id06]), '') AS [Identify_Name06]
        //   , ISNULL((SELECT [Channel_Name]    FROM [Channel_Set]    AS CS  WHERE CS.[Channel_Id] = SR.[Receive_Way]), '') AS [Receive_Way_Name]
        //FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS SR
        //LEFT JOIN [" + SchoolRidEntity.TABLE_NAME + @"] AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]";
        #endregion

        protected const string VIEWSQL = @"SELECT SR.*
     , ISNULL((SELECT [Stu_Name] FROM [" + StudentMasterEntity.TABLE_NAME + @"] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
     , ISNULL(RI.[Bill_Valid_Date], '') AS [Bill_Valid_Date], ISNULL(RI.[Bill_Close_Date], '') AS [Bill_Close_Date], ISNULL(RI.[Invoice_Close_Date], '') AS [Invoice_Close_Date]
     , ISNULL(RI.[Pay_Date], '') AS Pay_Due_Date1, ISNULL(RI.[Pay_Due_Date2], '') AS Pay_Due_Date2, ISNULL(RI.[Pay_Due_Date3], '') AS Pay_Due_Date3
     , ISNULL((SELECT [Year_Name]       FROM [Year_List]      AS YL  WHERE YL.[Year_Id]       = SR.[Year_Id]), '') AS [Year_Name]
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
  LEFT JOIN [" + SchoolRidEntity.TABLE_NAME + @"] AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]";
        #endregion
        #endregion

        #region Field Name Const Class
        /// <summary>
        /// 學生繳費資料 + 繳費期限 x 3 + 開關列印日期 + 院別、系所、班別、減免類別、住宿項目、就貸項目、身分註記1 ~ 身分註記6 名稱 View 欄位名稱定義抽象類別
        /// </summary>
        public abstract new class Field : StudentReceiveEntity.Field
        {
            #region 繳費期限 x 3 + 開關列印日期 + 列印收據關閉日
            /// <summary>
            /// 開放列印日期 (西元年 yyyyMMdd)
            /// </summary>
            public const string BillOpenDate = "Bill_Valid_Date";

            /// <summary>
            /// 關閉列印日期 (西元年 yyyyMMdd)
            /// </summary>
            public const string BillCloseDate = "Bill_Close_Date";

            #region [MDY:2018xxxx] 增加 列印收據關閉日 欄位
            /// <summary>
            /// 列印收據關閉日 (格式：yyyyMMdd)
            /// </summary>
            public const string InvoiceCloseDate = "Invoice_Close_Date";
            #endregion

            #region [MDY:20160131] 因為 StudentReceive 增加繳款期限 (PayDueDate)，所以要改名子
            /// <summary>
            /// 繳費期限1 (民國年 yyyMMdd) (SchoolRid.PayDate.PayDate)
            /// </summary>
            public const string PayDueDate1 = "Pay_Due_Date1";
            #endregion

            /// <summary>
            /// 繳費期限2 (民國年 yyyMMdd) (土銀用來存放信用卡繳費期限)
            /// </summary>
            public const string PayDueDate2 = "Pay_Due_Date2";

            /// <summary>
            /// 繳費期限3 (民國年 yyyMMdd) (土銀用來存放財金繳費期限)
            /// </summary>
            public const string PayDueDate3 = "Pay_Due_Date3";
            #endregion

            #region 代碼名稱
            /// <summary>
            /// 學生姓名
            /// </summary>
            public const string StuName = "Stu_Name";

            #region [MDY:202203XX] 2022擴充案 增加學年名稱 欄位
            /// <summary>
            /// 學年名稱 欄位名稱常數定義
            /// </summary>
            public const string YearName = "Year_Name";
            #endregion

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
        public StudentReceiveView()
            : base()
        {
        }
        #endregion

        #region Property
        #region 繳費期限 x 3 + 開關列印日期 + 列印收據關閉日
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

        #region [MDY:2018xxxx] 增加 列印收據關閉日 欄位
        private string _InvoiceCloseDate = String.Empty;
        /// <summary>
        /// 列印收據關閉日 (格式：yyyyMMdd)
        /// </summary>
        [FieldSpec(Field.InvoiceCloseDate, false, FieldTypeEnum.Char, 8, false)]
        public string InvoiceCloseDate
        {
            get
            {
                return _InvoiceCloseDate;
            }
            set
            {
                _InvoiceCloseDate = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }
        #endregion


        #region [MDY:20160131] 因為 StudentReceive 增加繳款期限 (PayDueDate)，所以要改名子
        /// <summary>
        /// 繳費期限 (民國年 yyyMMdd) (SchoolRid.PayDate.PayDate)
        /// </summary>
        [FieldSpec(Field.PayDueDate1, false, FieldTypeEnum.Char, 7, false)]
        public string PayDueDate1
        {
            get;
            set;
        }
        #endregion

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

        #region [MDY:202203XX] 2022擴充案 增加學年名稱
        /// <summary>
        /// 學年名稱
        /// </summary>
        [FieldSpec(Field.YearName, false, FieldTypeEnum.Char, 8, false)]
        public string YearName
        {
            get;
            set;
        }
        #endregion

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
