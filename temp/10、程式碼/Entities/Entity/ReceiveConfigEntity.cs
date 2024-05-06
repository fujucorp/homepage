/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/11/24 14:30:10
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
	/// 代收類別(學校)設定資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ReceiveConfigEntity : Entity
	{
		public const string TABLE_NAME = "Receive_Config";

		#region Field Name Const Class
		/// <summary>
		/// 代收類別(學校)設定資料 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 代收類別代碼 欄位名稱常數定義
			/// </summary>
			public const string ReceiveType = "Receive_Type";
			#endregion

			#region Data
			/// <summary>
			/// 預設(目前)學年代碼 欄位名稱常數定義
			/// </summary>
			public const string DefaultYearId = "Year_Now";

			/// <summary>
            /// 預設(目前)學期代碼 欄位名稱常數定義 (土銀沒使用)
			/// </summary>
			public const string DefaultTermId = "Term_Now";

			/// <summary>
            /// 學生登入時是否檢查生日的旗標 (Y / N) 欄位名稱常數定義 (土銀用來紀錄登入依據，請參考 LoginKeyTypeCodeTexts)
			/// </summary>
			public const string CheckBirthday = "Birthday_Chk";

			/// <summary>
			/// 裁決章抬頭11 欄位名稱常數定義
			/// </summary>
			public const string Giree11 = "Giree_11";

			/// <summary>
			/// 裁決章抬頭12 欄位名稱常數定義
			/// </summary>
			public const string Giree12 = "Giree_12";

			/// <summary>
			/// 裁決章抬頭21 欄位名稱常數定義
			/// </summary>
			public const string Giree21 = "Giree_21";

			/// <summary>
			/// 裁決章抬頭22 欄位名稱常數定義
			/// </summary>
			public const string Giree22 = "Giree_22";

			/// <summary>
			/// 裁決章抬頭31 欄位名稱常數定義
			/// </summary>
			public const string Giree31 = "Giree_31";

			/// <summary>
			/// 裁決章抬頭32 欄位名稱常數定義
			/// </summary>
			public const string Giree32 = "Giree_32";

			/// <summary>
			/// 裁決章抬頭41 欄位名稱常數定義
			/// </summary>
			public const string Giree41 = "Giree_41";

			/// <summary>
			/// 裁決章抬頭42 欄位名稱常數定義
			/// </summary>
			public const string Giree42 = "Giree_42";

			/// <summary>
			/// 裁決章抬頭51 欄位名稱常數定義
			/// </summary>
			public const string Giree51 = "Giree_51";

			/// <summary>
			/// 裁決章抬頭52 欄位名稱常數定義
			/// </summary>
			public const string Giree52 = "Giree_52";

			/// <summary>
			/// 裁決章抬頭61 欄位名稱常數定義
			/// </summary>
			public const string Giree61 = "Giree_61";

			/// <summary>
			/// 裁決章抬頭62 欄位名稱常數定義
			/// </summary>
			public const string Giree62 = "Giree_62";

			/// <summary>
			/// 最高年級 (1~12) 欄位名稱常數定義
			/// </summary>
			public const string MaxGrade = "Stu_Grade";
			#endregion

			#region 狀態相關欄位
			/// <summary>
			/// 資料狀態 (0=正常 / D=停用) (請參考 DataStatusCodeTexts) 欄位名稱常數定義
			/// </summary>
			public const string Status = "status";

			/// <summary>
			/// 資料建立日期 (含時間) 欄位名稱常數定義
			/// </summary>
			public const string CrtDate = "crt_date";

			/// <summary>
			/// 資料建立者。暫時儲存使用者帳號 (UserId) 欄位名稱常數定義
			/// </summary>
			public const string CrtUser = "crt_user";

			/// <summary>
			/// 資料最後修改日期 (含時間) 欄位名稱常數定義
			/// </summary>
			public const string MdyDate = "mdy_date";

			/// <summary>
			/// 資料最後修改者。暫時儲存使用者帳號 (UserId) 欄位名稱常數定義
			/// </summary>
			public const string MdyUser = "mdy_user";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// 建構代收類別(學校)設定資料承載類別
		/// </summary>
		public ReceiveConfigEntity()
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
		/// 預設(目前)學年代碼
		/// </summary>
		[FieldSpec(Field.DefaultYearId, false, FieldTypeEnum.Char, 3, true)]
		public string DefaultYearId
		{
			get;
			set;
		}

		/// <summary>
		/// 預設(目前)學期代碼 (土銀沒使用)
		/// </summary>
		[FieldSpec(Field.DefaultTermId, false, FieldTypeEnum.Char, 1, true)]
		public string DefaultTermId
		{
			get;
			set;
		}

		/// <summary>
        /// 學生登入時是否檢查生日的旗標 (Y / N) (土銀用來紀錄登入依據，請參考 LoginKeyTypeCodeTexts)
		/// </summary>
		[FieldSpec(Field.CheckBirthday, false, FieldTypeEnum.Char, 1, false)]
		public string CheckBirthday
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭11
		/// </summary>
		[FieldSpec(Field.Giree11, false, FieldTypeEnum.Char, 20, true)]
		public string Giree11
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭12
		/// </summary>
		[FieldSpec(Field.Giree12, false, FieldTypeEnum.Char, 20, true)]
		public string Giree12
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭21
		/// </summary>
		[FieldSpec(Field.Giree21, false, FieldTypeEnum.Char, 20, true)]
		public string Giree21
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭22
		/// </summary>
		[FieldSpec(Field.Giree22, false, FieldTypeEnum.Char, 20, true)]
		public string Giree22
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭31
		/// </summary>
		[FieldSpec(Field.Giree31, false, FieldTypeEnum.Char, 20, true)]
		public string Giree31
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭32
		/// </summary>
		[FieldSpec(Field.Giree32, false, FieldTypeEnum.Char, 20, true)]
		public string Giree32
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭41
		/// </summary>
		[FieldSpec(Field.Giree41, false, FieldTypeEnum.Char, 20, true)]
		public string Giree41
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭42
		/// </summary>
		[FieldSpec(Field.Giree42, false, FieldTypeEnum.Char, 20, true)]
		public string Giree42
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭51
		/// </summary>
		[FieldSpec(Field.Giree51, false, FieldTypeEnum.Char, 20, true)]
		public string Giree51
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭52
		/// </summary>
		[FieldSpec(Field.Giree52, false, FieldTypeEnum.Char, 20, true)]
		public string Giree52
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭61
		/// </summary>
		[FieldSpec(Field.Giree61, false, FieldTypeEnum.Char, 20, true)]
		public string Giree61
		{
			get;
			set;
		}

		/// <summary>
		/// 裁決章抬頭62
		/// </summary>
		[FieldSpec(Field.Giree62, false, FieldTypeEnum.Char, 20, true)]
		public string Giree62
		{
			get;
			set;
		}

		/// <summary>
		/// 最高年級 (1~12)
		/// </summary>
		[FieldSpec(Field.MaxGrade, false, FieldTypeEnum.VarChar, 2, true)]
		public string MaxGrade
		{
			get;
			set;
		}
		#endregion

		#region 狀態相關欄位
		/// <summary>
		/// 資料狀態 (0=正常 / D=停用) (請參考 DataStatusCodeTexts)
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
		public string Status
		{
			get;
			set;
		}

		/// <summary>
		/// 資料建立日期 (含時間)
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get;
			set;
		}

		/// <summary>
		/// 資料建立者。暫時儲存使用者帳號 (UserId)
		/// </summary>
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
		public string CrtUser
		{
			get;
			set;
		}

		/// <summary>
		/// 資料最後修改日期 (含時間)
		/// </summary>
		[FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? MdyDate
		{
			get;
			set;
		}

		/// <summary>
		/// 資料最後修改者。暫時儲存使用者帳號 (UserId)
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
