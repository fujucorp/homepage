/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:34:22
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
	/// Credit2_Standard 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class Credit2StandardEntity : Entity
	{
		public const string TABLE_NAME = "Credit2_Standard";

		#region Field Name Const Class
		/// <summary>
		/// Credit2StandardEntity 欄位名稱定義抽象類別
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
			/// Dep_Id 欄位名稱常數定義
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
			/// Receive_Id 欄位名稱常數定義
			/// </summary>
			public const string ReceiveId = "Receive_Id";
			#endregion

			#region Data
			/// <summary>
			/// Credit2_01 欄位名稱常數定義
			/// </summary>
			public const string Credit201 = "Credit2_01";

			/// <summary>
			/// Credit2_02 欄位名稱常數定義
			/// </summary>
			public const string Credit202 = "Credit2_02";

			/// <summary>
			/// Credit2_03 欄位名稱常數定義
			/// </summary>
			public const string Credit203 = "Credit2_03";

			/// <summary>
			/// Credit2_04 欄位名稱常數定義
			/// </summary>
			public const string Credit204 = "Credit2_04";

			/// <summary>
			/// Credit2_05 欄位名稱常數定義
			/// </summary>
			public const string Credit205 = "Credit2_05";

			/// <summary>
			/// Credit2_06 欄位名稱常數定義
			/// </summary>
			public const string Credit206 = "Credit2_06";

			/// <summary>
			/// Credit2_07 欄位名稱常數定義
			/// </summary>
			public const string Credit207 = "Credit2_07";

			/// <summary>
			/// Credit2_08 欄位名稱常數定義
			/// </summary>
			public const string Credit208 = "Credit2_08";

			/// <summary>
			/// Credit2_09 欄位名稱常數定義
			/// </summary>
			public const string Credit209 = "Credit2_09";

			/// <summary>
			/// Credit2_10 欄位名稱常數定義
			/// </summary>
			public const string Credit210 = "Credit2_10";

			/// <summary>
			/// Credit2_11 欄位名稱常數定義
			/// </summary>
			public const string Credit211 = "Credit2_11";

			/// <summary>
			/// Credit2_12 欄位名稱常數定義
			/// </summary>
			public const string Credit212 = "Credit2_12";

			/// <summary>
			/// Credit2_13 欄位名稱常數定義
			/// </summary>
			public const string Credit213 = "Credit2_13";

			/// <summary>
			/// Credit2_14 欄位名稱常數定義
			/// </summary>
			public const string Credit214 = "Credit2_14";

			/// <summary>
			/// Credit2_15 欄位名稱常數定義
			/// </summary>
			public const string Credit215 = "Credit2_15";

			/// <summary>
			/// Credit2_16 欄位名稱常數定義
			/// </summary>
			public const string Credit216 = "Credit2_16";

			/// <summary>
			/// Credit2_17 欄位名稱常數定義
			/// </summary>
			public const string Credit217 = "Credit2_17";

			/// <summary>
			/// Credit2_18 欄位名稱常數定義
			/// </summary>
			public const string Credit218 = "Credit2_18";

			/// <summary>
			/// Credit2_19 欄位名稱常數定義
			/// </summary>
			public const string Credit219 = "Credit2_19";

			/// <summary>
			/// Credit2_20 欄位名稱常數定義
			/// </summary>
			public const string Credit220 = "Credit2_20";

			/// <summary>
			/// Credit2_21 欄位名稱常數定義
			/// </summary>
			public const string Credit221 = "Credit2_21";

			/// <summary>
			/// Credit2_22 欄位名稱常數定義
			/// </summary>
			public const string Credit222 = "Credit2_22";

			/// <summary>
			/// Credit2_23 欄位名稱常數定義
			/// </summary>
			public const string Credit223 = "Credit2_23";

			/// <summary>
			/// Credit2_24 欄位名稱常數定義
			/// </summary>
			public const string Credit224 = "Credit2_24";

			/// <summary>
			/// Credit2_25 欄位名稱常數定義
			/// </summary>
			public const string Credit225 = "Credit2_25";

			/// <summary>
			/// Credit2_26 欄位名稱常數定義
			/// </summary>
			public const string Credit226 = "Credit2_26";

			/// <summary>
			/// Credit2_27 欄位名稱常數定義
			/// </summary>
			public const string Credit227 = "Credit2_27";

			/// <summary>
			/// Credit2_28 欄位名稱常數定義
			/// </summary>
			public const string Credit228 = "Credit2_28";

			/// <summary>
			/// Credit2_29 欄位名稱常數定義
			/// </summary>
			public const string Credit229 = "Credit2_29";

			/// <summary>
			/// Credit2_30 欄位名稱常數定義
			/// </summary>
			public const string Credit230 = "Credit2_30";

			/// <summary>
			/// Credit2_31 欄位名稱常數定義
			/// </summary>
			public const string Credit231 = "Credit2_31";

			/// <summary>
			/// Credit2_32 欄位名稱常數定義
			/// </summary>
			public const string Credit232 = "Credit2_32";

			/// <summary>
			/// Credit2_33 欄位名稱常數定義
			/// </summary>
			public const string Credit233 = "Credit2_33";

			/// <summary>
			/// Credit2_34 欄位名稱常數定義
			/// </summary>
			public const string Credit234 = "Credit2_34";

			/// <summary>
			/// Credit2_35 欄位名稱常數定義
			/// </summary>
			public const string Credit235 = "Credit2_35";

			/// <summary>
			/// Credit2_36 欄位名稱常數定義
			/// </summary>
			public const string Credit236 = "Credit2_36";

			/// <summary>
			/// Credit2_37 欄位名稱常數定義
			/// </summary>
			public const string Credit237 = "Credit2_37";

			/// <summary>
			/// Credit2_38 欄位名稱常數定義
			/// </summary>
			public const string Credit238 = "Credit2_38";

			/// <summary>
			/// Credit2_39 欄位名稱常數定義
			/// </summary>
			public const string Credit239 = "Credit2_39";

			/// <summary>
			/// Credit2_40 欄位名稱常數定義
			/// </summary>
			public const string Credit240 = "Credit2_40";

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
		/// Credit2StandardEntity 類別建構式
		/// </summary>
		public Credit2StandardEntity()
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

		private string _DepId = null;
		/// <summary>
		/// Dep_Id 欄位屬性
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
		#endregion

		#region Data
		/// <summary>
		/// Credit2_01 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit201, false, FieldTypeEnum.Char, 1, true)]
		public string Credit201
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_02 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit202, false, FieldTypeEnum.Char, 1, true)]
		public string Credit202
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_03 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit203, false, FieldTypeEnum.Char, 1, true)]
		public string Credit203
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_04 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit204, false, FieldTypeEnum.Char, 1, true)]
		public string Credit204
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_05 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit205, false, FieldTypeEnum.Char, 1, true)]
		public string Credit205
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_06 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit206, false, FieldTypeEnum.Char, 1, true)]
		public string Credit206
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_07 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit207, false, FieldTypeEnum.Char, 1, true)]
		public string Credit207
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_08 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit208, false, FieldTypeEnum.Char, 1, true)]
		public string Credit208
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_09 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit209, false, FieldTypeEnum.Char, 1, true)]
		public string Credit209
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit210, false, FieldTypeEnum.Char, 1, true)]
		public string Credit210
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit211, false, FieldTypeEnum.Char, 1, true)]
		public string Credit211
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit212, false, FieldTypeEnum.Char, 1, true)]
		public string Credit212
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit213, false, FieldTypeEnum.Char, 1, true)]
		public string Credit213
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit214, false, FieldTypeEnum.Char, 1, true)]
		public string Credit214
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit215, false, FieldTypeEnum.Char, 1, true)]
		public string Credit215
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit216, false, FieldTypeEnum.Char, 1, true)]
		public string Credit216
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_17 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit217, false, FieldTypeEnum.Char, 1, true)]
		public string Credit217
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_18 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit218, false, FieldTypeEnum.Char, 1, true)]
		public string Credit218
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_19 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit219, false, FieldTypeEnum.Char, 1, true)]
		public string Credit219
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_20 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit220, false, FieldTypeEnum.Char, 1, true)]
		public string Credit220
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_21 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit221, false, FieldTypeEnum.Char, 1, true)]
		public string Credit221
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_22 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit222, false, FieldTypeEnum.Char, 1, true)]
		public string Credit222
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_23 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit223, false, FieldTypeEnum.Char, 1, true)]
		public string Credit223
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_24 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit224, false, FieldTypeEnum.Char, 1, true)]
		public string Credit224
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_25 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit225, false, FieldTypeEnum.Char, 1, true)]
		public string Credit225
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_26 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit226, false, FieldTypeEnum.Char, 1, true)]
		public string Credit226
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_27 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit227, false, FieldTypeEnum.Char, 1, true)]
		public string Credit227
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_28 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit228, false, FieldTypeEnum.Char, 1, true)]
		public string Credit228
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_29 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit229, false, FieldTypeEnum.Char, 1, true)]
		public string Credit229
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_30 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit230, false, FieldTypeEnum.Char, 1, true)]
		public string Credit230
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_31 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit231, false, FieldTypeEnum.Char, 1, true)]
		public string Credit231
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_32 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit232, false, FieldTypeEnum.Char, 1, true)]
		public string Credit232
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_33 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit233, false, FieldTypeEnum.Char, 1, true)]
		public string Credit233
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_34 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit234, false, FieldTypeEnum.Char, 1, true)]
		public string Credit234
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_35 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit235, false, FieldTypeEnum.Char, 1, true)]
		public string Credit235
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_36 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit236, false, FieldTypeEnum.Char, 1, true)]
		public string Credit236
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_37 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit237, false, FieldTypeEnum.Char, 1, true)]
		public string Credit237
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_38 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit238, false, FieldTypeEnum.Char, 1, true)]
		public string Credit238
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_39 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit239, false, FieldTypeEnum.Char, 1, true)]
		public string Credit239
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_40 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit240, false, FieldTypeEnum.Char, 1, true)]
		public string Credit240
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
