/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/10/17 11:07:50
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
	/// 匯出資料檔
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ExportFileEntity : Entity
	{
		public const string TABLE_NAME = "Export_File";

		#region Field Name Const Class
		/// <summary>
		/// ExportFileEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 流水號 (PKey) (Identity)
			/// </summary>
			public const string SN = "SN";
			#endregion

			#region Data
			/// <summary>
			/// 所屬商家代號
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 匯出種類 (01=產生學生繳費資料媒體檔(跨學年); 02=產生銷帳資料檔(跨學年))(參考 ExportFileKindCodeTexts) (請注意這裡的 Kind 與 ExportConfigEntity.Kind 不同)
			/// </summary>
			public const string Kind = "Kind";

			/// <summary>
			/// 匯出檔名 (不含副檔名)
			/// </summary>
			public const string FileName = "File_Name";

			/// <summary>
			/// 匯出副檔名
			/// </summary>
			public const string ExtName = "Ext_Name";

			/// <summary>
			/// 匯出檔說明
			/// </summary>
			public const string Explain = "Explain";

			/// <summary>
			/// 對應的批次處理佇列序號 (0 表示批次處理產生)
			/// </summary>
			public const string JobNo = "Job_No";

			/// <summary>
			/// 匯出檔內容
			/// </summary>
			public const string FileContent = "File_Content";
			#endregion

			#region 狀態相關欄位
			/// <summary>
			/// 資料狀態 (0=正常; 1=待處理; 2=處理中; 3=失敗，參考 ExportFileStatusCodeTexts)
			/// </summary>
			public const string Status = "status";

			/// <summary>
			/// 資料建立者
			/// </summary>
			public const string CrtUser = "crt_user";

			/// <summary>
			/// 資料建立日期
			/// </summary>
			public const string CrtDate = "crt_date";

			/// <summary>
			/// 資料最後修改者
			/// </summary>
			public const string MdyUser = "mdy_user";

			/// <summary>
			/// 資料最後修改日期
			/// </summary>
			public const string MdyDate = "mdy_date";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// ExportFileEntity 類別建構式
		/// </summary>
		public ExportFileEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private int _SN = 0;
		/// <summary>
		/// 流水號 (Pkey) (Identity)
		/// </summary>
		[FieldSpec(Field.SN, true, FieldTypeEnum.Identity, false)]
		public int SN
		{
			get
			{
				return _SN;
			}
			set
			{
				_SN = value < 0 ? 0 : value;
			}
		}
		#endregion

		#region Data
		private string _ReceiveType = null;
		/// <summary>
		/// 所屬商家代號
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 6, false)]
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

		private string _Kind = null;
		/// <summary>
		/// 匯出種類 (01=產生學生繳費資料媒體檔(跨學年); 02=產生銷帳資料檔(跨學年))(參考 ExportFileKindCodeTexts) (請注意這裡的 Kind 與 ExportConfigEntity.Kind 不同)
		/// </summary>
		[FieldSpec(Field.Kind, false, FieldTypeEnum.VarChar, 2, false)]
		public string Kind
		{
			get
			{
				return _Kind;
			}
			set
			{
				_Kind = value == null ? null : value.Trim();
			}
		}

		private string _FileName = String.Empty;
		/// <summary>
		/// 匯出檔名 (不含副檔名)
		/// </summary>
		[FieldSpec(Field.FileName, false, FieldTypeEnum.NVarChar, 50, false)]
		public string FileName
		{
			get
			{
				return _FileName;
			}
			set
			{
				_FileName = value == null ? String.Empty : value.Trim();
			}
		}

		private string _ExtName = String.Empty;
		/// <summary>
		/// 匯出副檔名
		/// </summary>
		[FieldSpec(Field.ExtName, false, FieldTypeEnum.VarChar, 10, false)]
		public string ExtName
		{
			get
			{
				return _ExtName;
			}
			set
			{
				_ExtName = value == null ? String.Empty : value.Trim();
			}
		}

		private string _Explain = String.Empty;
		/// <summary>
		/// 匯出檔說明
		/// </summary>
		[FieldSpec(Field.Explain, false, FieldTypeEnum.NVarChar, 100, false)]
		public string Explain
		{
			get
			{
				return _Explain;
			}
			set
			{
				_Explain = value == null ? String.Empty : value.Trim();
			}
		}

		private int _JobNo = 0;
		/// <summary>
		/// 對應的批次處理佇列序號 (0 表示非批次處理產生)
		/// </summary>
		[FieldSpec(Field.JobNo, false, FieldTypeEnum.Integer, false)]
		public int JobNo
		{
			get
			{
				return _JobNo;
			}
			set
			{
				_JobNo = value < 0 ? 0 :value;
			}
		}

		/// <summary>
		/// 匯出檔內容
		/// </summary>
		[FieldSpec(Field.FileContent, false, FieldTypeEnum.Binary, true)]
		public byte[] FileContent
		{
			get;
			set;
		}
		#endregion

		#region 狀態相關欄位
		private string _Status = null;
		/// <summary>
		/// 資料狀態 (0=正常; 1=待處理; 2=處理中; 3=失敗，參考 ExportFileStatusCodeTexts)
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
		public string Status
		{
			get
			{
				return _Status;
			}
			set
			{
				_Status = value == null ? null : value.Trim();
			}
		}

		private string _CrtUser = null;
		/// <summary>
		/// 資料建立者
		/// </summary>
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
		public string CrtUser
		{
			get
			{
				return _CrtUser;
			}
			set
			{
				_CrtUser = value == null ? null : value.Trim();
			}
		}

		/// <summary>
		/// 資料建立日期
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get;
			set;
		}

		private string _MdyUser = null;
		/// <summary>
		/// 資料最後修改者
		/// </summary>
		[FieldSpec(Field.MdyUser, false, FieldTypeEnum.VarChar, 20, true)]
		public string MdyUser
		{
			get
			{
				return _MdyUser;
			}
			set
			{
				_MdyUser = value == null ? null : value.Trim();
			}
		}

		/// <summary>
		/// 資料最後修改日期
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
