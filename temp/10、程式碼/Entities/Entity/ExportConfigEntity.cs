/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/10/14 17:27:53
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
	/// 匯出資料設定檔
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ExportConfigEntity : Entity
	{
		public const string TABLE_NAME = "Export_Config";

		#region Field Name Const Class
		/// <summary>
		/// ExportConfigEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 代碼 (PKey)
			/// </summary>
			public const string Id = "ID";
			#endregion

			#region Data
			/// <summary>
			/// 所屬商家代號
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 設定種類 (01=學生繳費單資料; 02=銷帳資料) (參考 ExportConfigKindCodeTexts) (請注意這裡的 Kind 與 ExportFileEntity.Kind 不同)
			/// </summary>
			public const string Kind = "Kind";

			/// <summary>
			/// 設定編號 (01~99)
			/// </summary>
			public const string No = "No";

			/// <summary>
			/// 設定說明
			/// </summary>
			public const string Name = "Name";

			/// <summary>
			/// 匯出欄位清單
			/// </summary>
			public const string FieldData = "Field_Data";
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
		/// ExportConfigEntity 類別建構式
		/// </summary>
		public ExportConfigEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _Id = null;
		/// <summary>
		/// 代碼 (PKey)
		/// </summary>
		[FieldSpec(Field.Id, true, FieldTypeEnum.VarChar, 10, false)]
		public string Id
		{
			get
			{
				return _Id;
			}
			set
			{
				_Id = value == null ? null : value.Trim();
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
		/// 設定種類 (01=學生繳費單資料; 02=銷帳資料) (參考 ExportConfigKindCodeTexts) (請注意這裡的 Kind 與 ExportFileEntity.Kind 不同)
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

		private string _No = null;
		/// <summary>
		/// 設定編號 (01~99)
		/// </summary>
		[FieldSpec(Field.No, false, FieldTypeEnum.VarChar, 2, false)]
		public string No
		{
			get
			{
				return _No;
			}
			set
			{
				_No = value == null ? null : value.Trim();
			}
		}

		private string _Name = null;
		/// <summary>
		/// 設定說明
		/// </summary>
		[FieldSpec(Field.Name, false, FieldTypeEnum.NVarChar, 50, false)]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value == null ? null : value.Trim();
			}
		}

		private string _FieldData = null;
		/// <summary>
		/// 匯出欄位清單
		/// </summary>
		[FieldSpec(Field.FieldData, false, FieldTypeEnum.NVarChar, 2000, false)]
		public string FieldData
		{
			get
			{
				return _FieldData;
			}
			set
			{
				_FieldData = value == null ? String.Empty : value.Trim();
			}
		}
		#endregion

		#region 狀態相關欄位
		/// <summary>
		/// 資料狀態 (0=正常 / D=停用) (請參考 DataStatusCodeTexts)
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.NChar, 10, false)]
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

        #region Static Method
        /// <summary>
        /// 產生代碼
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="kind"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        public static string GenID(string receiveType, string kind, string no)
        {
            if (Fuju.Common.IsNumber(receiveType, 4, 6) && ExportConfigKindCodeTexts.IsDefine(kind) && Fuju.Common.IsNumber(no, 2))
            {
                return String.Format("{0}{1}{2}", receiveType, kind, no);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 產生代碼
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="kind"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        public static string GenID(string receiveType, string kind, int no)
        {
            if (Fuju.Common.IsNumber(receiveType, 4, 6) && ExportConfigKindCodeTexts.IsDefine(kind) && no >= 1 && no <= 99)
            {
                return String.Format("{0}{1}{2:00}", receiveType, kind, no);
            }
            else
            {
                return null;
            }
        }
        #endregion
	}
}
