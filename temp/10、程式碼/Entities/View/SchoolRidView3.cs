using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 代收費用設定資料 + 代收費用別名稱 + 模板內容 View (History 用) 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class SchoolRidView3 : SchoolRidEntity
    {
        protected const string VIEWSQL = @"SELECT SR.*
     , ISNULL((SELECT [Receive_Name] FROM [" + ReceiveListEntity.TABLE_NAME + @"] AS RL WHERE RL.[Receive_Type] = SR.[Receive_Type] AND RL.[Year_Id] = SR.[Year_Id] AND RL.[Term_Id] = SR.[Term_Id] AND RL.[Dep_Id] = SR.[Dep_Id] AND RL.[Receive_Id] = SR.[Receive_Id]), '') AS [Receive_Name]
     , (SELECT [BillForm_Image] FROM [" + BillFormEntity.TABLE_NAME + @"] AS B WHERE B.[BillForm_id] = (CASE WHEN ISNUMERIC(SR.[BillForm_id]) = 1 THEN CAST(SR.[BillForm_id] AS numeric) ELSE 0 END)) AS [Bill_Templet]
     , (SELECT [BillForm_Image] FROM [" + BillFormEntity.TABLE_NAME + @"] AS B WHERE B.[BillForm_id] = (CASE WHEN ISNUMERIC(SR.[invoiceform_id]) = 1 THEN CAST(SR.[invoiceform_id] AS numeric) ELSE 0 END)) AS [Invoice_Templet]
  FROM [" + SchoolRidEntity.TABLE_NAME + @"] AS SR";

        #region Field Name Const Class
        /// <summary>
        /// 代收費用設定資料 + 代收費用別名稱 + 模板內容 View 欄位名稱定義抽象類別
        /// </summary>
        public abstract new class Field : SchoolRidEntity.Field
        {
            #region 額外 Data
            /// <summary>
            /// 代收費用別名稱 欄位名稱常數定義
            /// </summary>
            public const string ReceiveName = "Receive_Name";

            /// <summary>
            /// 繳費單模板內容 欄位名稱常數定義
            /// </summary>
            public const string BillTemplet = "Bill_Templet";

            /// <summary>
            /// 收據模板內容 欄位名稱常數定義
            /// </summary>
            public const string InvoiceTemplet = "Invoice_Templet";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構代收費用設定資料 + 代收費用別名稱 + 模板內容 View 的資料承載類別
        /// </summary>
        public SchoolRidView3()
            : base()
        {
        }
        #endregion

        #region Property
        #region 額外 Data
        private string _ReceiveName = null;
        /// <summary>
        /// 代收費用別名稱
        /// </summary>
        [FieldSpec(Field.ReceiveName, false, FieldTypeEnum.NVarChar, 40, true)]
        public string ReceiveName
        {
            get
            {
                return _ReceiveName;
            }
            set
            {
                _ReceiveName = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 繳費單模板內容
        /// </summary>
        [FieldSpec(Field.BillTemplet, false, FieldTypeEnum.Binary, true)]
        public string BillTemplet
        {
            get;
            set;
        }

        /// <summary>
        /// 收據模板內容
        /// </summary>
        [FieldSpec(Field.InvoiceTemplet, false, FieldTypeEnum.Binary, true)]
        public string InvoiceTemplet
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Readonly Property
        /// <summary>
        /// 繳費單格式名稱
        /// </summary>
        [XmlIgnore]
        public string PayStyleName
        {
            get
            {
                switch (this.Paystyle)
                {
                    case "0":
                        return "二聯式";
                    case "1":
                        return "三聯式預設";
                    case "2":
                        return "三聯式可調";
                    case "9":
                        return "專屬";
                }
                return String.Empty;
            }
        }
        #endregion
    }
}
