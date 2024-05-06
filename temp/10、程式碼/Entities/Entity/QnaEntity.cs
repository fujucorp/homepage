/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/27 15:35:43
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
	/// QNA 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class QnaEntity : Entity
	{
		public const string TABLE_NAME = "QNA";

		#region Field Name Const Class
		/// <summary>
		/// QnaEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// sn 欄位名稱常數定義 (Identity)
			/// </summary>
			public const string Sn = "sn";
			#endregion

			#region Data
			/// <summary>
			/// type 欄位名稱常數定義
			/// </summary>
			public const string Type = "type";

			/// <summary>
			/// sort 欄位名稱常數定義
			/// </summary>
			public const string Sort = "sort";

			/// <summary>
			/// Q 欄位名稱常數定義
			/// </summary>
			public const string Q = "Q";

			/// <summary>
			/// A 欄位名稱常數定義
			/// </summary>
			public const string A = "A";

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
		/// QnaEntity 類別建構式
		/// </summary>
		public QnaEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		/// <summary>
		/// sn 欄位屬性 (Identity)
		/// </summary>
		[FieldSpec(Field.Sn, true, FieldTypeEnum.Identity, false)]
		public int Sn
		{
			get;
			set;
		}
		#endregion

		#region Data
		/// <summary>
		/// type 欄位屬性
		/// </summary>
		[FieldSpec(Field.Type, false, FieldTypeEnum.VarChar, 5, false)]
		public string Type
		{
			get;
			set;
		}

		/// <summary>
		/// sort 欄位屬性
		/// </summary>
		[FieldSpec(Field.Sort, false, FieldTypeEnum.Integer, false)]
		public int Sort
		{
			get;
			set;
		}

		/// <summary>
		/// Q 欄位屬性
		/// </summary>
		[FieldSpec(Field.Q, false, FieldTypeEnum.NVarChar, 2000, false)]
		public string Q
		{
			get;
			set;
		}

		/// <summary>
		/// A 欄位屬性
		/// </summary>
		[FieldSpec(Field.A, false, FieldTypeEnum.NVarCharMax, false)]
		public string A
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
