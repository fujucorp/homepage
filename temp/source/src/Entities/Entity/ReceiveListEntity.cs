﻿/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/10/21 01:16:03
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
	/// <summary>
	/// 代收費用別資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ReceiveListEntity : Entity
	{
		public const string TABLE_NAME = "Receive_List";

		#region Field Name Const Class
		/// <summary>
		/// 代收費用別資料 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 代收類別代碼 欄位名稱常數定義
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 學年代碼 欄位名稱常數定義
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
			/// 學期代碼 欄位名稱常數定義
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
			/// 部別代碼 欄位名稱常數定義
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
			/// 代收費用別代碼 欄位名稱常數定義
			/// </summary>
			public const string ReceiveId = "Receive_Id";
			#endregion

			#region Data
			/// <summary>
			/// 代收費用別名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveName = "Receive_Name";

			#region [MDY:20220808] 2022擴充案 代收費用別英文名稱 新增欄位
			/// <summary>
			/// 代收費用別英文名稱
			/// </summary>
			public const string ReceiveEName = "Receive_EName";
			#endregion
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
		/// 建構代收費用別資料承載類別
		/// </summary>
		public ReceiveListEntity()
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

		private string _TermId = null;
		/// <summary>
		/// 學期代碼
		/// </summary>
		[FieldSpec(Field.TermId, true, FieldTypeEnum.Char, 1, false)]
		public string TermId
		{
			get
			{
				return _TermId;
			}
			set
			{
				_TermId = value == null ? null : value.Trim();
			}
		}

		private string _DepId = null;
		/// <summary>
		/// 部別代碼
		/// </summary>
		[FieldSpec(Field.DepId, true, FieldTypeEnum.Char, 1, false)]
		public string DepId
		{
			get
			{
				return _DepId;
			}
			set
			{
				_DepId = value == null ? null : value.Trim();
			}
		}

		private string _ReceiveId = null;
		/// <summary>
		/// 代收費用別代碼
		/// </summary>
		[FieldSpec(Field.ReceiveId, true, FieldTypeEnum.VarChar, 2, false)]
		public string ReceiveId
		{
			get
			{
				return _ReceiveId;
			}
			set
			{
				_ReceiveId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		#region [MDY:20220808] 2022擴充案 代收費用別名稱型別調整為 NVarChar(140)
		/// <summary>
		/// 代收費用別名稱
		/// </summary>
		[FieldSpec(Field.ReceiveName, false, FieldTypeEnum.NVarChar, 140, true)]
		public string ReceiveName
		{
			get;
			set;
		}
		#endregion

		#region [MDY:20220808] 2022擴充案 代收費用別英文名稱 新增欄位並調整為 NVarChar(140)
		/// <summary>
		/// 代收費用別英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveEName, false, FieldTypeEnum.NVarChar, 140, true)]
		public string ReceiveEName
		{
			get;
			set;
		}
		#endregion
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