/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/08/15 14:46:02
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
	/// 連動製單服務帳號資料
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class SchoolServiceAccountEntity : Entity
	{
		public const string TABLE_NAME = "School_Service_Account";

		#region Field Name Const Class
		/// <summary>
		/// 連動製單服務帳號資料 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 系統代碼
			/// </summary>
			public const string SysId = "Sys_Id";
			#endregion

			#region Data
			/// <summary>
			/// 系統名稱
			/// </summary>
			public const string SysName = "Sys_Name";

			#region [MDY:20220530] Checkmarx 調整
			#region [MDY:20210401] 原碼修正
			/// <summary>
			/// 系統驗證碼
			/// </summary>
			public const string SysPXX = "Sys_Pwd";
			#endregion
			#endregion

			/// <summary>
			/// 授權呼叫端IP清單 (多筆以逗號區隔)
			/// </summary>
			public const string ClientIp = "Client_Ip";

			/// <summary>
			/// 授權商家代號清單 (多筆以逗號分隔)
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 申請的學校代號
			/// </summary>
			public const string SchIdenty = "Sch_Identy";

			/// <summary>
			/// 學校接收端 Url
			/// </summary>
			public const string SchReceiveUrl = "Sch_Receive_Url";
			#endregion

			#region 狀態
			/// <summary>
			/// 資料狀態
			/// </summary>
			public const string Status = "status";

			/// <summary>
			/// 資料建立日期時間
			/// </summary>
			public const string CrtDate = "crt_date";

			/// <summary>
			/// 資料建立者
			/// </summary>
			public const string CrtUser = "crt_user";

			/// <summary>
			/// 資料最後修改日期時間
			/// </summary>
			public const string MdyDate = "mdy_date";

			/// <summary>
			/// 資料最後修改者
			/// </summary>
			public const string MdyUser = "mdy_user";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// SchoolServiceAccountEntity 類別建構式
		/// </summary>
		public SchoolServiceAccountEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _SysId = null;
		/// <summary>
		/// 系統代碼
		/// </summary>
		[FieldSpec(Field.SysId, true, FieldTypeEnum.VarChar, 32, false)]
		public string SysId
		{
			get
			{
				return _SysId;
			}
			set
			{
				_SysId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		private string _SysName = null;
		/// <summary>
		/// 系統名稱
		/// </summary>
		[FieldSpec(Field.SysName, false, FieldTypeEnum.NVarChar, 32, false)]
		public string SysName
		{
			get
			{
				return _SysName;
			}
			set
			{
				_SysName = value == null ? null : value.Trim();
			}
		}

		#region [MDY:20220530] Checkmarx 調整
		#region [MDY:20210401] 原碼修正
		private string _SysPXX = null;
		/// <summary>
		/// 系統驗證碼
		/// </summary>
		[FieldSpec(Field.SysPXX, false, FieldTypeEnum.VarChar, 32, false)]
		public string SysPXX
		{
			get
			{
				return _SysPXX;
			}
			set
			{
				_SysPXX = value == null ? null : value.Trim();
			}
		}
		#endregion
		#endregion

		private string[] _MyClientIPs = null;
		private string _ClientIp = null;
		/// <summary>
		/// 授權呼叫端IP清單 (多筆以逗號區隔)
		/// </summary>
		[FieldSpec(Field.ClientIp, false, FieldTypeEnum.VarChar, 120, false)]
		public string ClientIp
		{
			get
			{
				return _ClientIp;
			}
			set
			{
				_ClientIp = value == null ? null : value.Trim();
				_MyClientIPs = null;
			}
		}

		private string[] _MyReceiveTypes = null;
		private string _ReceiveType = null;
		/// <summary>
		/// 授權商家代號清單 (多筆以逗號分隔)
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 100, false)]
		public string ReceiveType
		{
			get
			{
				return _ReceiveType;
			}
			set
			{
				_ReceiveType = value == null ? null : value.Trim();
				_MyReceiveTypes = null;
			}
		}

		private string _SchIdenty = null;
		/// <summary>
		/// 申請的學校代號
		/// </summary>
		[FieldSpec(Field.SchIdenty, false, FieldTypeEnum.VarChar, 10, false)]
		public string SchIdenty
		{
			get
			{
				return _SchIdenty;
			}
			set
			{
				_SchIdenty = value == null ? null : value.Trim();
			}
		}

		private string _SchReceiveUrl = null;
		/// <summary>
		/// 學校接收端 Url
		/// </summary>
		[FieldSpec(Field.SchReceiveUrl, false, FieldTypeEnum.VarChar, 100, false)]
		public string SchReceiveUrl
		{
			get
			{
				return _SchReceiveUrl;
			}
			set
			{
				_SchReceiveUrl = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region 狀態
		/// <summary>
		/// 資料狀態
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
		public string Status
		{
			get;
			set;
		}

		/// <summary>
		/// 資料建立日期時間
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get;
			set;
		}

		/// <summary>
		/// 資料建立者
		/// </summary>
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
		public string CrtUser
		{
			get;
			set;
		}

		/// <summary>
		/// 資料最後修改日期時間
		/// </summary>
		[FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? MdyDate
		{
			get;
			set;
		}

		/// <summary>
		/// 資料最後修改者
		/// </summary>
		[FieldSpec(Field.MdyUser, false, FieldTypeEnum.VarChar, 20, true)]
		public string MdyUser
		{
			get;
			set;
		}
		#endregion
		#endregion

        #region Method
        /// <summary>
        /// 取得授權呼叫端IP陣列
        /// </summary>
        /// <returns>傳回授權商家代號陣列</returns>
        public string[] GetMyClientIPs()
        {
            if (_MyClientIPs == null)
            {
                if (String.IsNullOrEmpty(_ClientIp))
                {
                    _MyClientIPs = new string[0];
                }
                else
                {
                    _MyClientIPs = _ClientIp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int idx = 0; idx < _MyClientIPs.Length; idx++)
                    {
                        _MyClientIPs[idx] = _MyClientIPs[idx].Trim();
                    }
                }
            }
            return _MyClientIPs;
        }

        /// <summary>
        /// 取得授權商家代號陣列
        /// </summary>
        /// <returns>傳回授權商家代號陣列</returns>
        public string[] GetMyReceiveTypes()
        {
            if (_MyReceiveTypes == null)
            {
                if (String.IsNullOrEmpty(_ReceiveType))
                {
                    _MyReceiveTypes = new string[0];
                }
                else
                {
                    _MyReceiveTypes = _ReceiveType.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int idx = 0; idx < _MyReceiveTypes.Length; idx++)
                    {
                        _MyReceiveTypes[idx] = _MyReceiveTypes[idx].Trim();
                    }
                }
            }
            return _MyReceiveTypes;
        }

        /// <summary>
        /// 取得指定呼叫端IP是否為授權的呼叫端IP
        /// </summary>
        /// <param name="receiveType"></param>
        /// <returns></returns>
        public bool IsMyClientIP(string clientIP)
        {
            if (!String.IsNullOrWhiteSpace(clientIP))
            {
                clientIP = clientIP.Trim();
                string[] myClientIPs = this.GetMyClientIPs();
                if (myClientIPs != null && myClientIPs.Length > 0)
                {
                    return (Array.FindIndex<string>(myClientIPs, x => x == clientIP) > -1);
                }
            }
            return false;
        }

        /// <summary>
        /// 取得指定商家代號是否為授權的商家代號
        /// </summary>
        /// <param name="receiveType"></param>
        /// <returns></returns>
        public bool IsMyReceiveType(string receiveType)
        {
            if (!String.IsNullOrWhiteSpace(receiveType))
            {
                receiveType = receiveType.Trim();
                string[] myReceiveTypes = this.GetMyReceiveTypes();
                if (myReceiveTypes != null && myReceiveTypes.Length > 0)
                {
                    return (Array.FindIndex<string>(myReceiveTypes, x => x == receiveType) > -1);
                }
            }
            return false;
        }
        #endregion
	}
}
