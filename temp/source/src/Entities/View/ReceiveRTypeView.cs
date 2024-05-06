using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
	/// <summary>
	/// 商家代號資料 View (選項用)
	/// </summary>
	[Serializable]
	[EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
	public partial class ReceiveRTypeView : Entity
	{
		protected const string VIEWSQL = @"SELECT [Receive_Type], [Sch_Name], [c_flag], [Eng_Enabled], [Sch_EName]
  FROM [School_Rtype]
 WHERE [status] = '" + DataStatusCodeTexts.NORMAL + "' ";

		#region Field Name Const Class
		/// <summary>
		/// SchoolRidView 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 代收類別代碼
			/// </summary>
			public const string ReceiveType = "Receive_Type";
			#endregion

			#region Data
			/// <summary>
			/// 代收類別名稱 (學校全名)
			/// </summary>
			public const string SchName = "Sch_Name";

			/// <summary>
			/// 是否需分行控管才可新增代收費用別  (1=是; 0=否)
			/// </summary>
			public const string CFlag = "c_flag";

			/// <summary>
			/// 英文資料啟用 (Y:啟用 N:停用，預設 N，一律轉大寫，且非 Y 值一律轉為 N)
			/// </summary>
			public const String EngEnabled = "Eng_Enabled";

			/// <summary>
			/// 學校英文全名
			/// </summary>
			public const string SchEName = "Sch_EName";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// SchoolRidView 類別建構式
		/// </summary>
		public ReceiveRTypeView()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _ReceiveType = null;
		/// <summary>
		/// 代收類別代碼
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
		#endregion

		#region Data
		/// <summary>
		/// 代收類別名稱 (學校全名)
		/// </summary>
		[FieldSpec(Field.SchName, false, FieldTypeEnum.VarChar, 54, false)]
		public string SchName
		{
			get;
			set;
		}

		/// <summary>
		/// 是否需分行控管才可新增代收費用別  (1=是; 0=否)
		/// </summary>
		[FieldSpec(Field.CFlag, false, FieldTypeEnum.Integer, true)]
		public int? CFlag
		{
			get;
			set;
		}

		private string _EngEnabled = null;
		/// <summary>
		/// 英文資料啟用 (Y:啟用 N:停用，預設 N，一律轉大寫，且非 Y 值一律轉為 N)
		/// </summary>
		[FieldSpec(Field.EngEnabled, false, FieldTypeEnum.Char, 1, true)]
		public string EngEnabled
		{
			get
			{
				return _EngEnabled;
			}
			set
			{
				if (String.IsNullOrWhiteSpace(value))
				{
					_EngEnabled = "N";
				}
				else
				{
					_EngEnabled = "Y".Equals(value.Trim(), StringComparison.InvariantCultureIgnoreCase) ? "Y" : "N";
				}
			}
		}

		/// <summary>
		/// 學校英文全名
		/// </summary>
		[FieldSpec(Field.SchEName, false, FieldTypeEnum.NVarChar, 140, true)]
		public string SchEName
		{
			get;
			set;
		}
		#endregion
		#endregion

		#region method
		/// <summary>
		/// 取得是否英文資料啟用
		/// </summary>
		/// <returns></returns>
		public bool IsEngEnabled()
		{
			return this.EngEnabled == "Y";
		}
		#endregion
	}
}
