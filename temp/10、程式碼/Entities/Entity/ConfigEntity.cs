/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/04/11 19:01:45
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
	/// 設定資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ConfigEntity : Entity
	{
		public const string TABLE_NAME = "Config";

		#region Field Name Const Class
		/// <summary>
		/// ConfigEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
            /// 設定代碼 參考 ConfigKeyCodeTexts)
			/// </summary>
			public const string ConfigKey = "config_key";
			#endregion

			#region Data
			/// <summary>
			/// 設定值
			/// </summary>
			public const string ConfigValue = "config_value";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// ConfigEntity 類別建構式
		/// </summary>
		public ConfigEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _ConfigKey = null;
		/// <summary>
        /// 設定代碼 (參考 ConfigKeyCodeTexts)
		/// </summary>
		[FieldSpec(Field.ConfigKey, true, FieldTypeEnum.VarChar, 20, false)]
		public string ConfigKey
		{
			get
			{
				return _ConfigKey;
			}
			set
			{
				_ConfigKey = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// 設定值
		/// </summary>
		[FieldSpec(Field.ConfigValue, false, FieldTypeEnum.NVarCharMax, false)]
		public string ConfigValue
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
