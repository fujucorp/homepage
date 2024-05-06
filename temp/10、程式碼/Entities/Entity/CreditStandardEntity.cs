/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/02/05 11:50:30
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
	/// 學分費收費標準
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class CreditStandardEntity : Entity
	{
		public const string TABLE_NAME = "Credit_Standard";

		#region Field Name Const Class
		/// <summary>
		/// CreditStandardEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
            /// 代收類別 欄位名稱常數定義
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
            /// 部別代碼 欄位名稱常數定義
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
            /// 費用別代碼 欄位名稱常數定義
			/// </summary>
			public const string ReceiveId = "Receive_Id";

			/// <summary>
            /// 院所代碼 欄位名稱常數定義
			/// </summary>
			public const string CollegeId = "College_Id";
			#endregion

			#region Data
			/// <summary>
            /// 學分費單價 欄位名稱常數定義
			/// </summary>
			public const string CreditPrice = "Credit_Price";

			/// <summary>
            /// 學分費單位 欄位名稱常數定義 (保留)
			/// </summary>
			public const string CreditUnit = "Credit_Unit";

			/// <summary>
            /// 所屬收入科目 欄位名稱常數定義 (第幾個 school_rid.Receive_itemxx)
			/// </summary>
			public const string CreditItem = "Credit_Item";

			/// <summary>
            /// 狀態 欄位名稱常數定義
			/// </summary>
			public const string Status = "status";

			/// <summary>
            /// 建立日期 欄位名稱常數定義
			/// </summary>
			public const string CrtDate = "crt_date";

			/// <summary>
            /// 建立人員 欄位名稱常數定義
			/// </summary>
			public const string CrtUser = "crt_user";

			/// <summary>
            /// 修改日期 欄位名稱常數定義
			/// </summary>
			public const string MdyDate = "mdy_date";

			/// <summary>
            /// 修改人員 欄位名稱常數定義
			/// </summary>
			public const string MdyUser = "mdy_user";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// CreditStandardEntity 類別建構式
		/// </summary>
		public CreditStandardEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _ReceiveType = null;
		/// <summary>
        /// 代收類別 欄位屬性
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
        /// 學年代碼 欄位屬性
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
        /// 學期代碼 欄位屬性
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
        /// 部別代碼 欄位屬性
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
        /// 費用別代碼 欄位屬性
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

		private string _CollegeId = null;
		/// <summary>
        /// 院所代碼 欄位屬性
		/// </summary>
		[FieldSpec(Field.CollegeId, true, FieldTypeEnum.Char, 8, false)]
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
		#endregion

		#region Data
		/// <summary>
        /// 學分費單價 欄位屬性 (第幾個 school_rid.Receive_itemxx)
		/// </summary>
		[FieldSpec(Field.CreditPrice, false, FieldTypeEnum.Decimal, true)]
		public decimal? CreditPrice
		{
			get;
			set;
		}

		/// <summary>
        /// 學分費單位 欄位屬性 (保留)
		/// </summary>
		[FieldSpec(Field.CreditUnit, false, FieldTypeEnum.Decimal, true)]
		public decimal? CreditUnit
		{
			get;
			set;
		}

		/// <summary>
        /// 所屬收入科目 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditItem, false, FieldTypeEnum.Char, 2, true)]
		public string CreditItem
		{
			get;
			set;
		}

		/// <summary>
        /// 狀態 欄位屬性
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
		public string Status
		{
			get;
			set;
		}

		/// <summary>
        /// 建立日期 欄位屬性
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get;
			set;
		}

		/// <summary>
        /// 建立人員 欄位屬性
		/// </summary>
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
		public string CrtUser
		{
			get;
			set;
		}

		/// <summary>
        /// 修改日期 欄位屬性
		/// </summary>
		[FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? MdyDate
		{
			get;
			set;
		}

		/// <summary>
        /// 修改人員 欄位屬性
		/// </summary>
		[FieldSpec(Field.MdyUser, false, FieldTypeEnum.VarChar, 20, true)]
		public string MdyUser
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
