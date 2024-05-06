/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/10/17 11:24:14
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
	/// 匯出資料檔View
	/// </summary>
	[Serializable]
	[EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
	public partial class ExportFileView : Entity
	{
		protected const string VIEWSQL = @"SELECT [SN], [Receive_Type], [Kind], [File_Name], [Ext_Name], [Explain], [status], [crt_user], [crt_date]
  FROM [Export_File]
 WHERE [crt_date] >= DATEADD(""D"", -90, GETDATE())";

		#region Field Name Const Class
		/// <summary>
		/// ExportFileView 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 流水號 (PKey)
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
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// ExportFileView 類別建構式
		/// </summary>
		public ExportFileView()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private int _SN = 0;
		/// <summary>
		/// 流水號 (PKey)
		/// </summary>
		[FieldSpec(Field.SN, false, FieldTypeEnum.Integer, true)]
		public int SN
		{
			get
			{
				return _SN;
			}
			set
			{
				_SN = value;
			}
		}
		#endregion

		#region Data
		private string _ReceiveType = null;
		/// <summary>
		/// 所屬商家代號
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarCharMax, true)]
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
		[FieldSpec(Field.Kind, false, FieldTypeEnum.VarCharMax, true)]
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
		[FieldSpec(Field.FileName, false, FieldTypeEnum.VarCharMax, true)]
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
		[FieldSpec(Field.ExtName, false, FieldTypeEnum.VarCharMax, true)]
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
		[FieldSpec(Field.Explain, false, FieldTypeEnum.VarCharMax, true)]
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
		#endregion

		#region 狀態相關欄位
		private string _Status = null;
		/// <summary>
		/// 資料狀態 (0=正常; 1=待處理; 2=處理中; 3=失敗，參考 ExportFileStatusCodeTexts)
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarCharMax, true)]
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
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarCharMax, true)]
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
		#endregion

		#region Readonly Property
        /// <summary>
        /// 取得資料狀態文字
        /// </summary>
        [XmlIgnore]
        public string StatusText
        {
            get
            {
                return ExportFileStatusCodeTexts.GetText(this.Status);
            }
        }
		#endregion
		#endregion
	}
}
