/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/12/06 15:50:03
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
	/// 代收費用設定資料 View
	/// </summary>
	[Serializable]
	[EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
	public partial class SchoolRidView : Entity
    {
        #region [Old]
//        protected const string VIEWSQL = @"SELECT SR.Receive_Type, SR.Receive_Status, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_ID
//     , RL.Receive_Name, SR.Credit_Item
//     , (CASE WHEN (SR.PayStyle = '0' OR SR.PayStyle = '1' OR SR.PayStyle = '9') THEN SR.PayStyle 
//             WHEN SR.PayStyle IS NULL THEN '1' 
//             ELSE '2' END) PayStyle
//  FROM School_Rid SR, Receive_List RL 
// WHERE SR.Receive_Type = RL.Receive_Type AND SR.Year_Id = RL.Year_Id AND SR.Term_Id = RL.Term_Id 
//   AND SR.Dep_Id = RL.Dep_Id AND SR.Receive_Id = RL.Receive_Id";
        #endregion

        protected const string VIEWSQL = @"SELECT SR.Receive_Type, SR.Receive_Status, SR.Year_Id, SR.Term_Id, SR.Dep_Id, SR.Receive_ID
     , RL.Receive_Name, SR.Credit_Item, SR.BillForm_id, SR.invoiceform_id
     , ISNULL((SELECT BillForm_Name FROM BillForm WHERE BillForm.BillForm_id = STR(SR.BillForm_id)), '') AS BillForm_Name
     , ISNULL((SELECT BillForm_Name FROM BillForm WHERE BillForm.BillForm_id = STR(SR.invoiceform_id)), '') AS InvoiceForm_Name
  FROM School_Rid SR, Receive_List RL 
 WHERE SR.Receive_Type = RL.Receive_Type AND SR.Year_Id = RL.Year_Id AND SR.Term_Id = RL.Term_Id 
   AND SR.Dep_Id = RL.Dep_Id AND SR.Receive_Id = RL.Receive_Id";

        #region Field Name Const Class
        /// <summary>
		/// SchoolRidView 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey

			#endregion

			#region Data
			/// <summary>
			/// 代收類別代碼
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 代收費用型態 (1=已有繳費者資料之收費; 2=需登錄繳費者資料之收費 (本校生); 3=為需登錄繳費者資料之收費 (不分，本校或外界人士)
			/// </summary>
			public const string ReceiveStatus = "Receive_Status";

			/// <summary>
			/// 學年代碼
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
			/// 學期代碼
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
			/// 部別代碼
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
			/// 代收費用別代碼
			/// </summary>
			public const string ReceiveId = "Receive_ID";

			/// <summary>
			/// 代收費用別名稱
			/// </summary>
			public const string ReceiveName = "Receive_Name";

			/// <summary>
			/// 課程或學分基準所屬收入科目
			/// </summary>
			public const string CreditItem = "Credit_Item";

            #region [Old]
            ///// <summary>
            ///// 繳費單格式 (1=三聯式預設; 2=三聯式可調; 0=二聯式; 9=專屬)
            ///// </summary>
            //public const string PayStyle = "PayStyle";
            #endregion

            /// <summary>
            /// 繳費單模板代碼
            /// </summary>
            public const string BillFormId = "BillForm_id";

            /// <summary>
            /// 繳費單模板名稱
            /// </summary>
            public const string BillFormName = "BillForm_Name";

            /// <summary>
            /// 收據模板代碼
            /// </summary>
            public const string InvoiceFormId = "invoiceform_id";

            /// <summary>
            /// 收據模板名稱
            /// </summary>
            public const string InvoiceFormName = "InvoiceForm_Name";
            #endregion
        }
		#endregion

		#region Constructor
		/// <summary>
		/// SchoolRidView 類別建構式
		/// </summary>
		public SchoolRidView()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey

		#endregion

		#region Data
		/// <summary>
		/// 代收類別代碼
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarCharMax, true)]
		public string ReceiveType
		{
			get;
			set;
		}

		/// <summary>
		/// 代收費用型態 (1=已有繳費者資料之收費; 2=需登錄繳費者資料之收費 (本校生); 3=為需登錄繳費者資料之收費 (不分，本校或外界人士)
		/// </summary>
		[FieldSpec(Field.ReceiveStatus, false, FieldTypeEnum.VarCharMax, true)]
		public string ReceiveStatus
		{
			get;
			set;
		}

		/// <summary>
		/// 學年代碼
		/// </summary>
		[FieldSpec(Field.YearId, false, FieldTypeEnum.VarCharMax, true)]
		public string YearId
		{
			get;
			set;
		}

		/// <summary>
		/// 學期代碼
		/// </summary>
		[FieldSpec(Field.TermId, false, FieldTypeEnum.VarCharMax, true)]
		public string TermId
		{
			get;
			set;
		}

		/// <summary>
		/// 部別代碼
		/// </summary>
		[FieldSpec(Field.DepId, false, FieldTypeEnum.VarCharMax, true)]
		public string DepId
		{
			get;
			set;
		}

		/// <summary>
		/// 代收費用別代碼
		/// </summary>
		[FieldSpec(Field.ReceiveId, false, FieldTypeEnum.VarCharMax, true)]
		public string ReceiveId
		{
			get;
			set;
		}

		/// <summary>
		/// 代收費用別名稱
		/// </summary>
		[FieldSpec(Field.ReceiveName, false, FieldTypeEnum.VarCharMax, true)]
		public string ReceiveName
		{
			get;
			set;
		}

		/// <summary>
		/// 課程或學分基準所屬收入科目
		/// </summary>
		[FieldSpec(Field.CreditItem, false, FieldTypeEnum.VarCharMax, true)]
		public string CreditItem
		{
			get;
			set;
		}

        #region [Old]
        ///// <summary>
        ///// 繳費單格式 (1=三聯式預設; 2=三聯式可調; 0=二聯式; 9=專屬)
        ///// </summary>
        //[FieldSpec(Field.PayStyle, false, FieldTypeEnum.VarCharMax, true)]
        //public string PayStyle
        //{
        //    get;
        //    set;
        //}
        #endregion

        /// <summary>
        /// 繳費單模板代碼
        /// </summary>
        [FieldSpec(Field.BillFormId, false, FieldTypeEnum.VarChar, 32, true)]
        public string BillFormId
        {
            get;
            set;
        }

        /// <summary>
        /// 繳費單模板名稱
        /// </summary>
        [FieldSpec(Field.BillFormName, false, FieldTypeEnum.NVarChar, 90, true)]
        public string BillFormName
        {
            get;
            set;
        }

        /// <summary>
        /// 收據模板代碼
        /// </summary>
        [FieldSpec(Field.InvoiceFormId, false, FieldTypeEnum.VarChar, 32, true)]
        public string InvoiceFormId
        {
            get;
            set;
        }

        /// <summary>
        /// 收據模板名稱
        /// </summary>
        [FieldSpec(Field.InvoiceFormName, false, FieldTypeEnum.NVarChar, 90, true)]
        public string InvoiceFormName
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
