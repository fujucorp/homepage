/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:31:34
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
	/// Users_Right 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class UsersRightEntity : Entity
	{
		public const string TABLE_NAME = "Users_Right";

		#region Field Name Const Class
		/// <summary>
		/// UsersRightEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// U_ID 欄位名稱常數定義
			/// </summary>
			public const string UId = "U_ID";

			/// <summary>
			/// U_RT 欄位名稱常數定義
			/// </summary>
			public const string URt = "U_RT";

			/// <summary>
			/// U_Grp 欄位名稱常數定義
			/// </summary>
			public const string UGrp = "U_Grp";

			/// <summary>
			/// U_Bank 欄位名稱常數定義
			/// </summary>
			public const string UBank = "U_Bank";

			/// <summary>
			/// Func_Id 欄位名稱常數定義
			/// </summary>
			public const string FuncId = "Func_Id";
			#endregion

			#region Data
			/// <summary>
			/// Right_Code 欄位名稱常數定義
			/// </summary>
			public const string RightCode = "Right_Code";

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
		/// UsersRightEntity 類別建構式
		/// </summary>
		public UsersRightEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _UId = null;
		/// <summary>
		/// U_ID 欄位屬性
		/// </summary>
		[FieldSpec(Field.UId, true, FieldTypeEnum.VarChar, 20, false)]
		public string UId
		{
			get
			{
				return _UId;
			}
			set
			{
				_UId = value == null ? null : value.Trim();
			}
		}

		private string _URt = null;
		/// <summary>
		/// U_RT 欄位屬性
		/// </summary>
		[FieldSpec(Field.URt, true, FieldTypeEnum.VarChar, 100, false)]
		public string URt
		{
			get
			{
				return _URt;
			}
			set
			{
				_URt = value == null ? null : value.Trim();
			}
		}

		private string _UGrp = null;
		/// <summary>
		/// U_Grp 欄位屬性
		/// </summary>
		[FieldSpec(Field.UGrp, true, FieldTypeEnum.VarChar, 8, false)]
		public string UGrp
		{
			get
			{
				return _UGrp;
			}
			set
			{
				_UGrp = value == null ? null : value.Trim();
			}
		}

		private string _UBank = null;
		/// <summary>
		/// U_Bank 欄位屬性
		/// </summary>
		[FieldSpec(Field.UBank, true, FieldTypeEnum.VarChar, 10, false)]
		public string UBank
		{
			get
			{
				return _UBank;
			}
			set
			{
				_UBank = value == null ? null : value.Trim();
			}
		}

		private string _FuncId = null;
		/// <summary>
		/// Func_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.FuncId, true, FieldTypeEnum.VarChar, 32, false)]
		public string FuncId
		{
			get
			{
				return _FuncId;
			}
			set
			{
				_FuncId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// Right_Code 欄位屬性
		/// </summary>
		[FieldSpec(Field.RightCode, false, FieldTypeEnum.VarChar, 10, false)]
		public string RightCode
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
