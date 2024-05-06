/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 15:39:00
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
	/// file_pool 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class FilePoolEntity : Entity
	{
		public const string TABLE_NAME = "file_pool";

		#region Field Name Const Class
		/// <summary>
		/// FilePoolEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 序號 (Identity)
			/// </summary>
			public const string Sn = "sn";
			#endregion

			#region Data
			/// <summary>
			/// 說明
			/// </summary>
			public const string Explain = "explain";

			/// <summary>
			/// 檔名
			/// </summary>
			public const string FileName = "file_name";

			/// <summary>
			/// 副檔名
			/// </summary>
			public const string ExtName = "ext_name";

			/// <summary>
            /// 型態 (1=連結; 2=檔案)
			/// </summary>
			public const string FileQual = "file_qual";

			/// <summary>
			/// 連結
			/// </summary>
			public const string Url = "url";

			/// <summary>
			/// 檔案image
			/// </summary>
			public const string File = "file";

			/// <summary>
			/// 狀態
			/// </summary>
			public const string Status = "status";

			/// <summary>
			/// 建立人員
			/// </summary>
			public const string CrtUser = "crt_user";

			/// <summary>
			/// 建立日期
			/// </summary>
			public const string CrtDate = "crt_date";

			/// <summary>
			/// 修改人員
			/// </summary>
			public const string MdyUser = "mdy_user";

			/// <summary>
			/// 修改日期
			/// </summary>
			public const string MdyDate = "mdy_date";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// FilePoolEntity 類別建構式
		/// </summary>
		public FilePoolEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		/// <summary>
		/// 序號 (Identity)
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
		/// 說明
		/// </summary>
		[FieldSpec(Field.Explain, false, FieldTypeEnum.NVarChar, 1000, false)]
		public string Explain
		{
			get;
			set;
		}

		/// <summary>
		/// 檔名
		/// </summary>
		[FieldSpec(Field.FileName, false, FieldTypeEnum.NVarChar, 100, false)]
		public string FileName
		{
			get;
			set;
		}

		/// <summary>
		/// 副檔名
		/// </summary>
		[FieldSpec(Field.ExtName, false, FieldTypeEnum.VarChar, 10, false)]
		public string ExtName
		{
			get;
			set;
		}

		/// <summary>
        /// 型態 (1=連結; 2=檔案)
		/// </summary>
		[FieldSpec(Field.FileQual, false, FieldTypeEnum.Char, 1, false)]
		public string FileQual
		{
			get;
			set;
		}

		/// <summary>
		/// 連結
		/// </summary>
		[FieldSpec(Field.Url, false, FieldTypeEnum.NVarChar, 1000, false)]
		public string Url
		{
			get;
			set;
		}

		/// <summary>
		/// 檔案image
		/// </summary>
		[FieldSpec(Field.File, false, FieldTypeEnum.Binary, true)]
		public byte[] File
		{
			get;
			set;
		}

		/// <summary>
		/// 狀態
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
		public string Status
		{
			get;
			set;
		}

		/// <summary>
		/// 建立人員
		/// </summary>
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
		public string CrtUser
		{
			get;
			set;
		}

		/// <summary>
		/// 建立日期
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get;
			set;
		}

		/// <summary>
		/// 修改人員
		/// </summary>
		[FieldSpec(Field.MdyUser, false, FieldTypeEnum.VarChar, 20, true)]
		public string MdyUser
		{
			get;
			set;
		}

		/// <summary>
		/// 修改日期
		/// </summary>
		[FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? MdyDate
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
