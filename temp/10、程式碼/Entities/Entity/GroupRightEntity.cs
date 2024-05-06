/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/11/16 19:39:42
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
	/// 群組權限資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class GroupRightEntity : Entity
	{
		public const string TABLE_NAME = "Group_Right";

		#region Field Name Const Class
		/// <summary>
		/// 群組權限資料 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 群組代碼 欄位名稱常數定義
			/// </summary>
			public const string GroupId = "Group_Id";

			/// <summary>
			/// 功能代碼 欄位名稱常數定義
			/// </summary>
			public const string FuncId = "Func_Id";
			#endregion

			#region Data
			/// <summary>
            /// 權限代碼 欄位名稱常數定義 (土銀區分新增、修改、刪除、查詢、列印，依序由左至右5碼，Y 表示有授權) (不再使用 FuncRightCodeTexts)
			/// </summary>
			public const string RightCode = "Right_Code";
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
		/// 建構群組權限資料承載類別
		/// </summary>
		public GroupRightEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _GroupId = null;
		/// <summary>
		/// 群組代碼
		/// </summary>
		[FieldSpec(Field.GroupId, true, FieldTypeEnum.VarChar, 8, false)]
		public string GroupId
		{
			get
			{
				return _GroupId;
			}
			set
			{
				_GroupId = value == null ? null : value.Trim();
			}
		}

		private string _FuncId = null;
		/// <summary>
		/// 功能代碼
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
		private string _RightCode = String.Empty;
		/// <summary>
        /// 權限代碼 (土銀區分新增、修改、刪除、查詢、列印，依序由左至右5碼，Y 表示有授權) (不再使用 FuncRightCodeTexts)
		/// </summary>
		[FieldSpec(Field.RightCode, false, FieldTypeEnum.VarChar, 10, false)]
		public string RightCode
		{
			get
			{
				return _RightCode;
			}
			set
			{
                _RightCode = value == null ? String.Empty : value.Trim();
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

        #region Static Method
        /// <summary>
        /// 授權所有權限的代碼
        /// </summary>
        public static readonly string All_RightCode = "YYYYY";
        /// <summary>
        /// 無任何權限的代碼
        /// </summary>
        public static readonly string None_RightCode = "NNNNN";

        /// <summary>
        /// 格式化授權代碼，補其 5 碼，並轉成大寫的 Y 或 N
        /// </summary>
        /// <param name="rightCode"></param>
        /// <returns></returns>
        public static string FormatRightCode(string rightCode)
        {
            if (rightCode == null || rightCode.Length > 5)
            {
                rightCode = "NNNNN";
            }
            StringBuilder rCode = new StringBuilder();
            foreach (char c in rightCode)
            {
                if (c == 'Y' || c == 'y')
                {
                    rCode.Append("Y");
                }
                else
                {
                    rCode.Append("N");
                }
            }
            return rCode.ToString();
        }
        #endregion
    }
}
