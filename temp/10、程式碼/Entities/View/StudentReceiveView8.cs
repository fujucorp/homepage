using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 使用財金信用卡繳費的學生繳費資料 (學生繳費資料 + 財金信用卡交易資料流水號) View 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class StudentReceiveView8 : StudentReceiveEntity
    {
        #region VIEWSQL
        protected const string VIEWSQL = @"SELECT *
     , ISNULL((SELECT TOP 1 [TXN_ID] FROM [" + CCardTxnDtlEntity.TABLE_NAME + @"] AS B
         WHERE B.[RECEIVE_TYPE] = A.[Receive_Type] AND B.[YEAR_ID] = A.[Year_Id] AND B.[TERM_ID] = A.[Term_Id]
           AND B.[DEP_ID] = A.[Dep_Id] AND B.[RECEIVE_ID] = A.[Receive_Id] AND B.[STUDEND_NO] = A.[Stu_Id] AND B.[OLD_SEQ] = A.[Old_Seq]
           AND B.[RID] = A.[Cancel_No] AND B.[STATUS] IN ('1', '2') AND B.[CREATE_DATE] < GETDATE()
           AND B.[AP_NO] = (CASE A.[NCCardFlag] WHEN 'Y' THEN '4' ELSE '1'END)
         ORDER BY B.[STATUS] DESC, B.[CREATE_DATE] DESC), 0) AS [TXN_ID]
  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS A
 WHERE A.[Cancel_No] IS NOT NULL AND A.[Cancel_No] != ''
   AND EXISTS (SELECT 1 FROM [" + CCardTxnDtlEntity.TABLE_NAME + @"] AS B
                WHERE B.[RECEIVE_TYPE] = A.[Receive_Type] AND B.[YEAR_ID] = A.[Year_Id] AND B.[TERM_ID] = A.[Term_Id]
                  AND B.[DEP_ID] = A.[Dep_Id] AND B.[RECEIVE_ID] = A.[Receive_Id] AND B.[STUDEND_NO] = A.[Stu_Id] AND B.[OLD_SEQ] = A.[Old_Seq]
                  AND B.[RID] = A.[Cancel_No] AND B.[STATUS] IN ('1', '2') AND B.[CREATE_DATE] < GETDATE()
                  AND B.[AP_NO] = (CASE A.[NCCardFlag] WHEN 'Y' THEN '4' ELSE '1'END))";
        #endregion

        #region Field Name Const Class
        /// <summary>
        /// 使用財金信用卡交易的學生繳費資料 (學生繳費資料 + 財金信用卡交易資料流水號) View 欄位名稱定義抽象類別
        /// </summary>
        public abstract new class Field : StudentReceiveEntity.Field
        {
            #region 財金信用卡交易資料流水號
            /// <summary>
            /// 最後一筆財金信用卡交易資料流水號 (CCardTxnDtl.TXN_ID)
            /// </summary>
            public const string TxnID = "TXN_ID";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 使用財金信用卡交易的學生繳費資料 (學生繳費資料 + 財金信用卡交易資料流水號) View 的資料承載類別
        /// </summary>
        public StudentReceiveView8()
            : base()
        {
        }
        #endregion

        #region Property
        #region 財金信用卡交易資料流水號
        /// <summary>
        /// 最後一筆財金信用卡交易資料流水號 (CCardTxnDtl.TXN_ID)
        /// </summary>
        [FieldSpec(Field.TxnID, false, FieldTypeEnum.VarChar, 20, false)]
        public string TxnID
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
