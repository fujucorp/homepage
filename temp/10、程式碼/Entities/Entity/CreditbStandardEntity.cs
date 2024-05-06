/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/02/05 11:51:07
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
	/// 學分基準收費標準
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class CreditbStandardEntity : Entity
	{
		public const string TABLE_NAME = "Creditb_Standard";

		#region Field Name Const Class
		/// <summary>
		/// CreditbStandardEntity 欄位名稱定義抽象類別
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
			/// Receive_Id 欄位名稱常數定義
			/// </summary>
			public const string ReceiveId = "Receive_Id";

			/// <summary>
			/// Dep_id 欄位名稱常數定義
			/// </summary>
			public const string DepId = "Dep_id";

			/// <summary>
			/// Creditb_Id 欄位名稱常數定義
			/// </summary>
			public const string CreditbId = "Creditb_Id";
			#endregion

			#region Data
			/// <summary>
			/// Creditb_Name 欄位名稱常數定義
			/// </summary>
			public const string CreditbName = "Creditb_Name";

			/// <summary>
			/// Creditb_Cprice 欄位名稱常數定義
			/// </summary>
			public const string CreditbCprice = "Creditb_Cprice";

			/// <summary>
			/// Credit_Item 欄位名稱常數定義
			/// </summary>
			public const string CreditItem = "Credit_Item";

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
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// CreditbStandardEntity 類別建構式
		/// </summary>
		public CreditbStandardEntity()
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

		private string _ReceiveId = null;
		/// <summary>
		/// Receive_Id 欄位屬性
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

		private string _DepId = null;
		/// <summary>
		/// Dep_id 欄位屬性
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

		private string _CreditbId = null;
		/// <summary>
		/// Creditb_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditbId, true, FieldTypeEnum.VarChar, 20, false)]
		public string CreditbId
		{
			get
			{
				return _CreditbId;
			}
			set
			{
				_CreditbId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// Creditb_Name 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditbName, false, FieldTypeEnum.VarChar, 20, true)]
		public string CreditbName
		{
			get;
			set;
		}

		/// <summary>
		/// Creditb_Cprice 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditbCprice, false, FieldTypeEnum.Decimal, false)]
		public decimal CreditbCprice
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Item 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditItem, false, FieldTypeEnum.Char, 2, false)]
		public string CreditItem
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
		#endregion
		#endregion
	}
}
