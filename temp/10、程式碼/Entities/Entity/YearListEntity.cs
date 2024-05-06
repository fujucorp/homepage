/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/04/09 13:31:41
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
	/// 學年資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class YearListEntity : Entity
	{
		public const string TABLE_NAME = "Year_List";

		#region Field Name Const Class
		/// <summary>
		/// YearListEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 學年代碼
			/// </summary>
			public const string YearId = "Year_Id";
			#endregion

			#region Data
			/// <summary>
			/// 學年名稱
			/// </summary>
			public const string YearName = "Year_Name";

			#region [MDY:202203XX] 2022擴充案 學年英文名稱、資料啟用 欄位
			/// <summary>
			/// 學年英文名稱
			/// </summary>
			public const string YearEName = "Year_EName";

			/// <summary>
			/// 資料啟用 (Y:啟用 N:停用，預設 Y，一律轉大寫，且非 N 值一律轉為 Y)
			/// </summary>
			public const string Enabled = "Enabled";
			#endregion
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// YearListEntity 類別建構式
		/// </summary>
		public YearListEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _YearId = null;
		/// <summary>
		/// 學年代碼
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
		#endregion

		#region Data
		/// <summary>
		/// 學年名稱
		/// </summary>
		[FieldSpec(Field.YearName, false, FieldTypeEnum.NVarChar, 20, false)]
		public string YearName
		{
			get;
			set;
		}

		#region [MDY:202203XX] 2022擴充案 學年英文名稱、資料啟用 欄位
		/// <summary>
		/// 學年英文名稱
		/// </summary>
		[FieldSpec(Field.YearEName, false, FieldTypeEnum.NVarChar, 40, true)]
		public string YearEName
		{
			get;
			set;
		}

		private string _Enabled = "Y";
		/// <summary>
		/// 資料啟用 (Y:啟用 N:停用，預設 Y，一律轉大寫，且非 N 值一律轉為 Y)
		/// </summary>
		[FieldSpec(Field.Enabled, false, FieldTypeEnum.Char, 1, true)]
		public string Enabled
		{
			get
			{
				return _Enabled;
			}
			set
			{
				if (String.IsNullOrWhiteSpace(value))
				{
					_Enabled = "Y";
				}
				else
				{
					_Enabled = "N".Equals(value.Trim(), StringComparison.InvariantCultureIgnoreCase) ? "N" : "Y";
				}
			}
		}
		#endregion
		#endregion
		#endregion
	}
}
