/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/01/26 10:42:22
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
	/// 檔案庫
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class BankpmEntity : Entity
	{
		public const string TABLE_NAME = "bankPM";

		#region Field Name Const Class
		/// <summary>
		/// BankpmEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
            /// 序號 欄位名稱常數定義 (Identity)
			/// </summary>
			public const string Cancel = "cancel";
			#endregion

			#region Data
			/// <summary>
            /// 檔名 欄位名稱常數定義
			/// </summary>
			public const string Filename = "filename";

			/// <summary>
            /// 文字檔的檔案內容 欄位名稱常數定義
			/// </summary>
			public const string Filedetail = "filedetail";

			/// <summary>
            /// 狀態 欄位名稱常數定義 (請參考 BankPMStatusCodeTexts)
			/// </summary>
			public const string Status = "status";

			/// <summary>
            /// 建立日期 欄位名稱常數定義 (格式：yyyy/M/d)
			/// </summary>
			public const string Cdate = "cdate";

			/// <summary>
            /// 上傳日期 欄位名稱常數定義 (格式：yyyy/M/d)
			/// </summary>
			public const string Udate = "udate";

			/// <summary>
            /// 檔案內容 (byte[])
			/// </summary>
			public const string Tempfile = "Tempfile";

			/// <summary>
            /// 業務別碼 欄位名稱常數定義
			/// </summary>
			public const string ReceiveType = "receive_type";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// BankpmEntity 類別建構式
		/// </summary>
		public BankpmEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		/// <summary>
        /// 序號 (Identity)
		/// </summary>
		[FieldSpec(Field.Cancel, true, FieldTypeEnum.Identity, false)]
		public int Cancel
		{
			get;
			set;
		}
		#endregion

		#region Data
		/// <summary>
        /// 檔名
		/// </summary>
		[FieldSpec(Field.Filename, false, FieldTypeEnum.VarChar, 50, true)]
		public string Filename
		{
			get;
			set;
		}

		/// <summary>
		/// 文字檔的檔案內容
		/// </summary>
		[FieldSpec(Field.Filedetail, false, FieldTypeEnum.NVarCharMax, true)]
		public string Filedetail
		{
			get;
			set;
		}

		/// <summary>
        /// 狀態 (請參考 BankPMStatusCodeTexts)
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 50, true)]
		public string Status
		{
			get;
			set;
		}

		/// <summary>
        /// 建立日期 (格式：yyyy/M/d)
		/// </summary>
		[FieldSpec(Field.Cdate, false, FieldTypeEnum.Char, 10, true)]
		public string Cdate
		{
			get;
			set;
		}

		/// <summary>
        /// 上傳日期 (格式：yyyy/M/d)
		/// </summary>
		[FieldSpec(Field.Udate, false, FieldTypeEnum.Char, 10, true)]
		public string Udate
		{
			get;
			set;
		}

		/// <summary>
        /// 檔案內容 (byte[])
		/// </summary>
		[FieldSpec(Field.Tempfile, false, FieldTypeEnum.Binary, true)]
		public byte[] Tempfile
		{
			get;
			set;
		}

		/// <summary>
        /// 業務別碼
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.Char, 10, true)]
		public string ReceiveType
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
