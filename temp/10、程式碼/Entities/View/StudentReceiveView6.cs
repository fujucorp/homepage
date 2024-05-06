using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 使用支付寶交易且已清算的學生繳費資料 (學生繳費資料 + 支付寶交易資料流水號) View 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class StudentReceiveView6 : StudentReceiveEntity
    {
        #region [Old]
//        protected const string VIEWSQL = @"SELECT * FROM (
//SELECT *
//     , ISNULL((SELECT TOP 1 [sn] FROM [" + InboundTxnDtlEntity.TABLE_NAME + @"] AS B
//         WHERE B.[Receive_Type] = A.[Receive_Type] AND B.[Year_Id] = A.[Year_Id] AND B.[Term_Id] = A.[Term_Id]
//           AND B.[Dep_Id] = A.[Dep_Id] AND B.[Receive_Id] = A.[Receive_Id] AND B.[Stu_Id] = A.[Stu_Id] AND B.[Seq] = A.[Old_Seq]
//           AND B.[Inbound_File] IS NOT NULL AND B.[Inbound_File] <> '' AND B.[Inbound_Data] IS NOT NULL AND B.[Inbound_Data] <> ''
//           AND B.[txn_time] < GETDATE()
//         ORDER BY B.[txn_time] DESC), 0) AS [TXN_SN]
//  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS A
// WHERE A.[Cancel_No] IS NOT NULL AND A.[Cancel_No] != ''
//) AS V
// WHERE [TXN_SN] > 0 ";
        #endregion

        protected const string VIEWSQL = @"SELECT *
     , ISNULL((SELECT TOP 1 [sn] FROM [" + InboundTxnDtlEntity.TABLE_NAME + @"] AS B
         WHERE B.[receive_type] = A.[Receive_Type] AND B.[year_id] = A.[Year_Id] AND B.[term_id] = A.[Term_Id]
           AND B.[dep_id] = A.[Dep_Id] AND B.[receive_id] = A.[Receive_Id] AND B.[stu_id] = A.[Stu_Id] AND B.[seq] = A.[Old_Seq]
           AND B.[inbound_file] IS NOT NULL AND B.[inbound_file] <> '' AND B.[inbound_data] IS NOT NULL AND B.[inbound_data] <> ''
           AND B.[txn_time] < GETDATE()
         ORDER BY B.[txn_time] DESC), 0) AS [TXN_SN]
  FROM [" + StudentReceiveEntity.TABLE_NAME + @"] AS A
 WHERE A.[Cancel_No] IS NOT NULL AND A.[Cancel_No] != ''
   AND EXISTS (SELECT 1 FROM [" + InboundTxnDtlEntity.TABLE_NAME + @"] AS B
         WHERE B.[receive_type] = A.[Receive_Type] AND B.[year_id] = A.[Year_Id] AND B.[term_id] = A.[Term_Id]
           AND B.[dep_id] = A.[Dep_Id] AND B.[receive_id] = A.[Receive_Id] AND B.[stu_id] = A.[Stu_Id] AND B.[seq] = A.[Old_Seq]
           AND B.[inbound_file] IS NOT NULL AND B.[inbound_file] <> '' AND B.[inbound_data] IS NOT NULL AND B.[inbound_data] <> ''
           AND B.[txn_time] < GETDATE()) ";

        #region Field Name Const Class
        /// <summary>
        /// 使用支付寶交易且已清算的學生繳費資料 (學生繳費資料 + 支付寶交易資料流水號) View 欄位名稱定義抽象類別
        /// </summary>
        public abstract new class Field : StudentReceiveEntity.Field
        {
            #region 支付寶交易資料流水號
            /// <summary>
            /// 最後一筆已清算的支付寶交易資料流水號 (InboundTxnDtlEntity.Sn，不可能小於 1)
            /// </summary>
            public const string TxnSN = "TXN_SN";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 使用支付寶交易且已清算的學生繳費資料 (學生繳費資料 + 支付寶交易資料流水號) View 的資料承載類別
        /// </summary>
        public StudentReceiveView6()
            : base()
        {
        }
        #endregion

        #region Property
        #region 支付寶交易資料流水號
        /// <summary>
        /// 最後一筆已清算的支付寶交易資料流水號 (InboundTxnDtlEntity.Sn，不可能小於 1)
        /// </summary>
        [FieldSpec(Field.TxnSN, false, FieldTypeEnum.Integer, false)]
        public Int32 TxnSN
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
