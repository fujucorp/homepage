/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/12/19 18:16:27
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
	/// 模板資料
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class BillFormEntity : Entity
	{
		public const string TABLE_NAME = "BillForm";

		#region Field Name Const Class
		/// <summary>
		/// 模板資料 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 模板編號
			/// </summary>
			public const string BillFormId = "BillForm_id";
			#endregion

			#region Data
			/// <summary>
			/// 模板名稱
			/// </summary>
			public const string BillFormName = "BillForm_Name";

			/// <summary>
			/// 模板檔案內容 byte
			/// </summary>
			public const string BillFormImage = "BillForm_Image";

			/// <summary>
			/// 代收類別代號 (特例：M101為多聯繳費單，此資料的其他欄位沒用) (土銀是商家代號，模板版本類別為3才有值)
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 模板類別 (0=繳費單; 1=收據) (請參考 BillFormTypeCodeTexts)
			/// </summary>
			public const string BillFormType = "billform_Type";

			/// <summary>
			/// 學制、類別的代碼 (保留) (土銀不使用)
			/// </summary>
			public const string BizType = "biz_type";

			/// <summary>
			/// 適用單位類別 (1=學校; 2=企業) (請參考 BillFormUserCodeTexts) (土銀只有學校，固定設為 1=學校)
			/// </summary>
			public const string BillFormUser = "billForm_User";

			/// <summary>
			/// 模板版本類別 (1=系統公版; 2=學制、類別公版; 3=專屬) (請參考 BillFormEditionCodeTexts) (簡化成 2=公版; 3=專屬)
			/// </summary>
			public const string BillFormEdition = "billForm_edition";
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
		/// BillFormEntity 類別建構式
		/// </summary>
		public BillFormEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private decimal _BillFormId = 0;
		/// <summary>
		/// 模板編號
		/// </summary>
		[FieldSpec(Field.BillFormId, true, FieldTypeEnum.Decimal, false)]
		public decimal BillFormId
		{
			get
			{
				return _BillFormId;
			}
			set
			{
				_BillFormId = value;
			}
		}
		#endregion

		#region Data
		private string _BillFormName = null;
		/// <summary>
		/// 模板名稱
		/// </summary>
		[FieldSpec(Field.BillFormName, false, FieldTypeEnum.NVarChar, 90, false)]
		public string BillFormName
		{
			get
			{
				return _BillFormName;
			}
			set
			{
				_BillFormName = value == null ? null : value.Trim();
			}
		}

		/// <summary>
		/// 模板檔案內容 byte
		/// </summary>
		[FieldSpec(Field.BillFormImage, false, FieldTypeEnum.Binary, true)]
		public byte[] BillFormImage
		{
			get;
			set;
		}

		private string _ReceiveType = null;
		/// <summary>
		/// 代收類別代號 (特例：M101為多聯繳費單，此資料的其他欄位沒用) (土銀是商家代號，模板版本類別為3才有值)
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 6, true)]
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

		private string _BillFormType = null;
		/// <summary>
		/// 模板類別 (0=繳費單; 1=收據) (請參考 BillFormTypeCodeTexts)
		/// </summary>
		[FieldSpec(Field.BillFormType, false, FieldTypeEnum.Char, 1, true)]
		public string BillFormType
		{
			get
			{
				return _BillFormType;
			}
			set
			{
				_BillFormType = value == null ? null : value.Trim();
			}
		}

		private string _BizType = null;
		/// <summary>
        /// 學制、類別的代碼 (保留) (土銀不使用)
		/// </summary>
		[FieldSpec(Field.BizType, false, FieldTypeEnum.VarChar, 3, false)]
		public string BizType
		{
			get
			{
				return _BizType;
			}
			set
			{
				_BizType = value == null ? null : value.Trim();
			}
		}

		private string _BillFormUser = null;
		/// <summary>
        /// 適用單位類別 (1=學校; 2=企業) (請參考 BillFormUserCodeTexts) (土銀只有學校，固定設為 1=學校)
		/// </summary>
		[FieldSpec(Field.BillFormUser, false, FieldTypeEnum.Char, 1, false)]
		public string BillFormUser
		{
			get
			{
				return _BillFormUser;
			}
			set
			{
				_BillFormUser = value == null ? null : value.Trim();
			}
		}

		private string _BillFormEdition = null;
		/// <summary>
		/// 模板版本類別 (1=系統公版; 2=學制、類別公版; 3=專屬) (請參考 BillFormEditionCodeTexts) (簡化成 2=公版; 3=專屬)
		/// </summary>
		[FieldSpec(Field.BillFormEdition, false, FieldTypeEnum.Char, 1, false)]
		public string BillFormEdition
		{
			get
			{
				return _BillFormEdition;
			}
			set
			{
				_BillFormEdition = value == null ? null : value.Trim();
			}
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
