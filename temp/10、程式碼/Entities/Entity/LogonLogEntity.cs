/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/12/22 13:59:41
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
	/// 登入日誌資料
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class LogonLogEntity : Entity
	{
        #region Const
        /// <summary>
        /// 登入成功 : 00000
        /// </summary>
        public static readonly string STATUS_LOGON_OK = "00000";
        /// <summary>
        /// 一般登出 : 99999
        /// </summary>
        public static readonly string STATUS_LOGOUT_OK = "99999";

        /// <summary>
        /// 強迫登出 : 99998
        /// </summary>
        public static readonly string STATUS_LOGOUT_FORCED = "99998";

        /// <summary>
        /// 透過頁面登入 : 1
        /// </summary>
        public static readonly string LOGON_WAY_PAGE = "1";

        /// <summary>
        /// 透過SSO登入 :　2
        /// </summary>
        public static readonly string LOGON_WAY_SSO = "2";
        #endregion

		public const string TABLE_NAME = "Logon_Log";

		#region Field Name Const Class
		/// <summary>
		/// LogonLogEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 登入序號 (GUID)
			/// </summary>
			public const string SN = "sn";
			#endregion

			#region Data
			/// <summary>
			/// 登入單位代碼 (UserQual為 1:分行代碼(7碼) / 2:學校代碼 / 3:空字串 / 4:銀行主碼(3碼))
			/// </summary>
			public const string LogonUnit = "logon_unit";

			/// <summary>
			/// 登入帳號 (UserQual為 1:使用者帳號(行員工號) / 2:使用者帳號 / 3:學生學號 / 4:使用者帳號)
			/// </summary>
			public const string LogonId = "logon_id";

			#region [MDY:20220530] Checkmarx 調整
			#region [MDY:20210401] 原碼修正
			/// <summary>
			/// 登入密碼 (UserQual為 1、2、4:密碼 / 3:身分證號 + "" + 生日)
			/// </summary>
			public const string LogonPXX = "logon_pwd";
			#endregion
			#endregion

			/// <summary>
			/// 登入身分別 (1=銀行使用者; 2=學校使用者; 3=學生使用者; 4=同業使用者;) (請參考 UserQualCodeTexts)
			/// </summary>
			public const string LogonQual = "logon_qual";

			/// <summary>
			/// 登入方式 (1=頁面登入; 2=SSO登入)
			/// </summary>
			public const string LogonWay = "logon_way";

			/// <summary>
			/// 登入 IP
			/// </summary>
			public const string ClientIP = "client_ip";

			/// <summary>
			/// 使用語系名稱
			/// </summary>
			public const string CultureName = "culture_name";

			/// <summary>
			/// 登入時間
			/// </summary>
			public const string LogonTime = "logon_time";

			/// <summary>
			/// 登入結果狀態代碼 (00000=登入成功; 99999=已登出; 其他=錯誤代碼)
			/// </summary>
			public const string StatusCode = "status_code";

			/// <summary>
			/// 登入結果狀態訊息
			/// </summary>
			public const string StatusDesc = "status_desc";

			/// <summary>
			/// 登出時間
			/// </summary>
			public const string LogoutTime = "logout_time";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// LogonLogEntity 類別建構式
		/// </summary>
		public LogonLogEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _SN = null;
		/// <summary>
		/// 登入序號 (GUID)
		/// </summary>
		[FieldSpec(Field.SN, true, FieldTypeEnum.VarChar, 36, false)]
		public string SN
		{
			get
			{
				return _SN;
			}
			set
			{
				_SN = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		private string _LogonUnit = null;
		/// <summary>
		/// 登入單位代碼 (UserQual為 1:分行代碼(7碼) / 2:學校代碼 / 3:學校代碼 / 4:銀行主碼(3碼))
		/// </summary>
		[FieldSpec(Field.LogonUnit, false, FieldTypeEnum.VarChar, 12, false)]
		public string LogonUnit
		{
			get
			{
				return _LogonUnit;
			}
			set
			{
				_LogonUnit = value == null ? null : value.Trim();
			}
		}

		private string _LogonId = null;
		/// <summary>
		/// 登入帳號 (UserQual為 1:使用者帳號(行員工號) / 2:使用者帳號 / 3:學生學號 / 4:使用者帳號)
		/// </summary>
		[FieldSpec(Field.LogonId, false, FieldTypeEnum.VarChar, 20, false)]
		public string LogonId
		{
			get
			{
				return _LogonId;
			}
			set
			{
				_LogonId = value == null ? null : value.Trim();
			}
		}

		#region [MDY:20220530] Checkmarx 調整
		#region [MDY:20210401] 原碼修正
		private string _LogonPXX = null;
		/// <summary>
		/// 登入密碼
		/// </summary>
		[FieldSpec(Field.LogonPXX, false, FieldTypeEnum.VarChar, 64, false)]
		public string LogonPXX
		{
			get
			{
				return _LogonPXX;
			}
			set
			{
				_LogonPXX = value == null ? null : value.Trim();
			}
		}
		#endregion
		#endregion

		private string _LogonQual = null;
		/// <summary>
		/// 登入身分別 (1=銀行使用者; 2=學校使用者; 3=學生使用者; 4=同業使用者;) (請參考 UserQualCodeTexts)
		/// </summary>
		[FieldSpec(Field.LogonQual, false, FieldTypeEnum.Char, 1, false)]
		public string LogonQual
		{
			get
			{
				return _LogonQual;
			}
			set
			{
				_LogonQual = value == null ? null : value.Trim();
			}
		}

		private string _LogonWay = null;
		/// <summary>
		/// 登入方式 (1=頁面登入; 2=SSO登入)
		/// </summary>
		[FieldSpec(Field.LogonWay, false, FieldTypeEnum.Char, 1, false)]
		public string LogonWay
		{
			get
			{
				return _LogonWay;
			}
			set
			{
				_LogonWay = value == null ? null : value.Trim();
			}
		}

		private string _ClientIP = String.Empty;
		/// <summary>
		/// 登入 IP
		/// </summary>
		[FieldSpec(Field.ClientIP, false, FieldTypeEnum.VarChar, 50, false)]
		public string ClientIP
		{
			get
			{
				return _ClientIP;
			}
			set
			{
				_ClientIP = value == null ? String.Empty : value.Trim();
			}
		}

		private string _CultureName = String.Empty;
		/// <summary>
		/// 使用語系名稱
		/// </summary>
		[FieldSpec(Field.CultureName, false, FieldTypeEnum.VarChar, 10, false)]
		public string CultureName
		{
			get
			{
				return _CultureName;
			}
			set
			{
				_CultureName = value == null ? String.Empty : value.Trim();
			}
		}

		private DateTime? _LogonTime = null;
		/// <summary>
		/// 登入時間
		/// </summary>
		[FieldSpec(Field.LogonTime, false, FieldTypeEnum.DateTime, false)]
		public DateTime? LogonTime
		{
			get
			{
				return _LogonTime;
			}
			set
			{
				_LogonTime = value;
			}
		}

		private string _StatusCode = null;
		/// <summary>
		/// 登入結果狀態代碼 (00000=登入成功; 99999=已登出; 其他=錯誤代碼)
		/// </summary>
		[FieldSpec(Field.StatusCode, false, FieldTypeEnum.VarChar, 5, false)]
		public string StatusCode
		{
			get
			{
				return _StatusCode;
			}
			set
			{
				_StatusCode = value == null ? null : value.Trim();
			}
		}

		private string _StatusDesc = "";
		/// <summary>
		/// 登入結果狀態訊息
		/// </summary>
		[FieldSpec(Field.StatusDesc, false, FieldTypeEnum.NVarChar, 100, false)]
		public string StatusDesc
		{
			get
			{
				return _StatusDesc;
			}
			set
			{
				_StatusDesc = value == null ? "" : value.Trim();
			}
		}

		private DateTime? _LogoutTime = null;
		/// <summary>
		/// 登出時間
		/// </summary>
		[FieldSpec(Field.LogoutTime, false, FieldTypeEnum.DateTime, true)]
		public DateTime? LogoutTime
		{
			get
			{
				return _LogoutTime;
			}
			set
			{
				_LogoutTime = value;
			}
		}
		#endregion
		#endregion
	}
}
