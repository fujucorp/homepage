/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/11/15 17:26:46
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
	/// 群組資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class GroupListEntity : Entity
	{
		public const string TABLE_NAME = "Group_List";

		#region Field Name Const Class
		/// <summary>
		/// 群組資料 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 群組代碼 (PK) 欄位名稱常數定義
			/// </summary>
			public const string GroupId = "Group_Id";
			#endregion

			#region Data
			/// <summary>
			/// 群組名稱
			/// </summary>
			public const string GroupName = "Group_Name";

			/// <summary>
			/// 群組角色 (1=行員 / 2=學校，請參考 RoleCodeTexts)
			/// </summary>
			public const string Role = "Role";

			/// <summary>
			/// 權限角色。(3=主管(管理者) / 2=經辦(使用者)，請參考 RoleTypeCodeTexts)
			/// </summary>
			public const string RoleType = "Role_type";

			/// <summary>
			/// 特定單位代碼 (role為1時，空白或特定分行代碼 (6碼)，否則學校代號)
			/// </summary>
			public const string Branchs = "Branchs";
			#endregion

			#region 狀態相關欄位
			/// <summary>
			/// 資料狀態 (0=正常 / D=停用) (請參考 DataStatusCodeTexts) 欄位名稱常數定義
			/// </summary>
			public const string Status = "status";

			/// <summary>
			/// 資料建立日期 (含時間) 欄位名稱常數定義
			/// </summary>
			public const string CrtDate = "Create_Date";

			/// <summary>
			/// 資料建立者。暫時儲存使用者帳號 (UserId) 欄位名稱常數定義
			/// </summary>
			public const string CrtUser = "Create_User";

			/// <summary>
			/// 資料最後修改日期 (含時間) 欄位名稱常數定義
			/// </summary>
			public const string MdyDate = "Modify_Date";

			/// <summary>
			/// 資料最後修改者。暫時儲存使用者帳號 (UserId) 欄位名稱常數定義
			/// </summary>
			public const string MdyUser = "Modify_user";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// 建構群組資料承載類別
		/// </summary>
		public GroupListEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _GroupId = null;
		/// <summary>
		/// 群組代碼 (PK)
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
		#endregion

		#region Data
		/// <summary>
		/// 群組名稱
		/// </summary>
		[FieldSpec(Field.GroupName, false, FieldTypeEnum.VarChar, 20, false)]
		public string GroupName
		{
			get;
			set;
		}

		/// <summary>
		/// 群組角色 (1=行員 / 2=學校，請參考 RoleCodeTexts)
		/// </summary>
		[FieldSpec(Field.Role, false, FieldTypeEnum.Char, 1, false)]
		public string Role
		{
			get;
			set;
		}

		/// <summary>
		/// 權限角色。(3=主管(管理者) / 2=經辦(使用者)，請參考 RoleTypeCodeTexts)
		/// </summary>
		[FieldSpec(Field.RoleType, false, FieldTypeEnum.Char, 1, false)]
		public string RoleType
		{
			get;
			set;
		}

		/// <summary>
		/// 特定單位代碼 (role為1時，空白或特定分行代碼 (6碼)，否則學校代號)
		/// </summary>
		[FieldSpec(Field.Branchs, false, FieldTypeEnum.VarChar, 20, false)]
		public string Branchs
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

		#region Readonly Property
		/// <summary>
		/// 群組角色文字
		/// </summary>
		[XmlIgnore]
		public string RoleText
		{
			get
			{
				return RoleCodeTexts.GetText(this.Role);
			}
		}

		/// <summary>
		/// 權限角色文字
		/// </summary>
		[XmlIgnore]
		public string RoleTypeText
		{
			get
			{
				return RoleTypeCodeTexts.GetText(this.RoleType);
			}
		}
		#endregion
	}
}
