/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/11/15 17:22:33
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
	/// 功能選單資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class FuncMenuEntity : Entity
	{
		public const string TABLE_NAME = "FuncMenu";

		#region Field Name Const Class
		/// <summary>
		/// 功能選單資料 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 功能選單代碼 欄位名稱常數定義
			/// </summary>
			public const string FuncId = "Func_Id";
			#endregion

			#region Data
			/// <summary>
			/// 功能選單名稱 欄位名稱常數定義
			/// </summary>
			public const string FuncName = "Func_Name";

			/// <summary>
			/// 父層功能選單代碼 欄位名稱常數定義
			/// </summary>
			public const string ParentId = "Parent_Id";

			/// <summary>
			/// 功能選單網址 欄位名稱常數定義
			/// </summary>
			public const string Url = "Url";

			/// <summary>
			/// 功能選單顯示排序 欄位名稱常數定義
			/// </summary>
			public const string SortNo = "Sort_No";
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
		/// 建構功能選單資料承載類別
		/// </summary>
		public FuncMenuEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _FuncId = null;
		/// <summary>
		/// 功能選單代碼
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
		/// 功能選單名稱
		/// </summary>
		[FieldSpec(Field.FuncName, false, FieldTypeEnum.NVarChar, 50, false)]
		public string FuncName
		{
			get;
			set;
		}

        private string _ParentId = null;
		/// <summary>
		/// 父層功能選單代碼
		/// </summary>
		[FieldSpec(Field.ParentId, false, FieldTypeEnum.VarChar, 32, false)]
		public string ParentId
		{
            get
            {
                return _ParentId;
            }
            set
            {
                _ParentId = value == null ? null : value.Trim();
            }
		}

        private string _Url = null;
		/// <summary>
		/// 功能選單網址
		/// </summary>
		[FieldSpec(Field.Url, false, FieldTypeEnum.NVarChar, 100, false)]
		public string Url
		{
            get
            {
                return _Url;
            }
            set
            {
                _Url = value == null ? null : value.Trim();
            }
		}

		/// <summary>
		/// 功能選單顯示排序
		/// </summary>
		[FieldSpec(Field.SortNo, false, FieldTypeEnum.Integer, false)]
		public int SortNo
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
