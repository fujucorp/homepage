using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
	/// <summary>
	/// 即查繳服務帳號資料
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class BankServiceAccountEntity : Entity
	{
		public const string TABLE_NAME = "Bank_Service_Account";

		#region Field Name Const Class
		/// <summary>
		/// 即查繳服務帳號資料 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 機關代碼
			/// </summary>
			public const string OrgId = "Org_Id";
			#endregion

			#region Data
			/// <summary>
			/// 機關名稱
			/// </summary>
			public const string OrgName = "Org_Name";

			#region [MDY:20220530] Checkmarx 調整
			#region [MDY:20210401] 原碼修正
			/// <summary>
			/// 機關驗證碼
			/// </summary>
			public const string OrgPXX = "Org_Pwd";
			#endregion
			#endregion

			/// <summary>
			/// 授權呼叫端IP清單 (多筆以逗號區隔)
			/// </summary>
			public const string ClientIp = "Client_Ip";

			/// <summary>
			/// 使用的管道代碼
			/// </summary>
			public const string ReceiveWay = "Receive_Way";
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
		/// 建構 BankServiceAccountEntity 類別
		/// </summary>
		public BankServiceAccountEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _OrgId = null;
		/// <summary>
		/// 機關代碼
		/// </summary>
		[FieldSpec(Field.OrgId, true, FieldTypeEnum.VarChar, 32, false)]
		public string OrgId
		{
			get
			{
				return _OrgId;
			}
			set
			{
				_OrgId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		private string _OrgName = null;
		/// <summary>
		/// 機關名稱
		/// </summary>
		[FieldSpec(Field.OrgName, false, FieldTypeEnum.NVarChar, 32, false)]
		public string SysName
		{
			get
			{
				return _OrgName;
			}
			set
			{
				_OrgName = value == null ? null : value.Trim();
			}
		}

		#region [MDY:20220530] Checkmarx 調整
		#region [MDY:20210401] 原碼修正
		private string _OrgPXX = null;
		/// <summary>
		/// 使用密碼
		/// </summary>
		[FieldSpec(Field.OrgPXX, false, FieldTypeEnum.VarChar, 32, false)]
		public string OrgPXX
		{
			get
			{
				return _OrgPXX;
			}
			set
			{
				_OrgPXX = value == null ? null : value.Trim();
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

		private string _ReceiveWay = null;
		/// <summary>
		/// 使用的管道代碼
		/// </summary>
		[FieldSpec(Field.ReceiveWay, false, FieldTypeEnum.VarChar, 4, false)]
		public string ReceiveWay
		{
			get
			{
				return _ReceiveWay;
			}
			set
			{
				_ReceiveWay = value == null ? null : value.Trim();
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
        #endregion
	}
}
