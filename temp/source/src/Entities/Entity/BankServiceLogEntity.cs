using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
	/// <summary>
	/// 即查繳服務日誌資料
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class BankServiceLogEntity: Entity
	{
		public const string TABLE_NAME = "Bank_Service_Log";

		#region Field Name Const Class
		/// <summary>
		/// 即查繳服務日誌資料 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 日誌流水號 (Identity)
			/// </summary>
			public const string SN = "SN";
			#endregion

			#region Data
			/// <summary>
			/// 機關代號
			/// </summary>
			public const string OrgId = "Org_Id";

			#region [MDY:20220530] Checkmarx 調整
			#region [MDY:20210401] 原碼修正
			/// <summary>
			/// 機關驗證碼
			/// </summary>
			public const string OrgPXX = "Org_Pwd";
			#endregion

			/// <summary>
			/// 呼叫端 IP
			/// </summary>
			public const string ClientIp = "Client_Ip";

			/// <summary>
			/// 呼叫方法名稱
			/// </summary>
			public const string MethodName = "Method_Name";

			/// <summary>
			/// 呼叫方法參數
			/// </summary>
			public const string MethodArgs = "Method_Args";

			/// <summary>
			/// 呼叫端 SeqNo
			/// </summary>
			public const string CallSeqNo = "Call_SeqNo";

			/// <summary>
			/// 呼叫端 虛擬帳號
			/// </summary>
			public const string CallRid = "Call_Rid";

			/// <summary>
			/// 錯誤訊息
			/// </summary>
			public const string ErrorMsg = "Error_Msg";

			/// <summary>
			/// 回傳訊息
			/// </summary>
			public const string ReturnMsg = "Return_Msg";

			/// <summary>
			/// 資料建立日期
			/// </summary>
			public const string CrtDate = "crt_date";
			#endregion
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// BankServiceLogEntity 類別建構式
		/// </summary>
		public BankServiceLogEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		/// <summary>
		/// 日誌流水號 (Identity)
		/// </summary>
		[FieldSpec(Field.SN, true, FieldTypeEnum.Identity, false)]
		public Int64 SN
		{
			get;
			set;
		}
		#endregion

		#region Data
		private string _OrgId = null;
		/// <summary>
		/// 機關代號
		/// </summary>
		[FieldSpec(Field.OrgId, false, FieldTypeEnum.VarChar, 32, false)]
		public string OrgId
		{
			get
			{
				return _OrgId;
			}
			set
			{
				_OrgId = value == null ? String.Empty : value.Trim();
				//寧願 cute 字也不要紀錄失敗
				if (_OrgId != null && _OrgId.Length > 32)
				{
					_OrgId = _OrgId.Substring(0, 32);
				}
			}
		}

		#region [MDY:20220530] Checkmarx 調整
		#region [MDY:20210401] 原碼修正
		private string _OrgPXX = null;
		/// <summary>
		/// 機關驗證碼
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
				_OrgPXX = value == null ? String.Empty : value.Trim();
				//寧願 cute 字也不要紀錄失敗
				if (_OrgPXX != null && _OrgPXX.Length > 32)
				{
					_OrgPXX = _OrgPXX.Substring(0, 32);
				}
			}
		}
		#endregion
		#endregion

		private string _ClientIp = null;
		/// <summary>
		/// 呼叫端 IP
		/// </summary>
		[FieldSpec(Field.ClientIp, false, FieldTypeEnum.VarChar, 40, false)]
		public string ClientIp
		{
			get
			{
				return _ClientIp;
			}
			set
			{
				_ClientIp = value == null ? String.Empty : value.Trim();
				//寧願 cute 字也不要紀錄失敗
				if (_ClientIp != null && _ClientIp.Length > 32)
				{
					_ClientIp = _ClientIp.Substring(0, 32);
				}
			}
		}

		private string _MethodName = null;
		/// <summary>
		/// 呼叫方法名稱
		/// </summary>
		[FieldSpec(Field.MethodName, false, FieldTypeEnum.VarChar, 20, false)]
		public string MethodName
		{
			get
			{
				return _MethodName;
			}
			set
			{
				_MethodName = value == null ? String.Empty : value.Trim();
				//寧願 cute 字也不要紀錄失敗
				if (_MethodName != null && _MethodName.Length > 20)
				{
					_MethodName = _MethodName.Substring(0, 20);
				}
			}
		}

		private string _MethodArgs = null;
		/// <summary>
		/// 呼叫方法參數
		/// </summary>
		[FieldSpec(Field.MethodArgs, false, FieldTypeEnum.NVarCharMax, false)]
		public string MethodArgs
		{
			get
			{
				return _MethodArgs;
			}
			set
			{
				_MethodArgs = value == null ? String.Empty : value.Trim();
			}
		}

		private string _CallSeqNo = null;
		/// <summary>
		/// 呼叫端 SeqNo
		/// </summary>
		[FieldSpec(Field.CallSeqNo, false, FieldTypeEnum.VarChar, 32, false)]
		public string CallSeqNo
		{
			get
			{
				return _CallSeqNo;
			}
			set
			{
				_CallSeqNo = value == null ? String.Empty : value.Trim();
				//寧願 cute 字也不要紀錄失敗
				if (_CallSeqNo != null && _CallSeqNo.Length > 2000)
				{
					_CallSeqNo = _CallSeqNo.Substring(0, 2000);
				}
			}
		}

		private string _CallRid = null;
		/// <summary>
		/// 呼叫端 虛擬帳號
		/// </summary>
		[FieldSpec(Field.CallRid, false, FieldTypeEnum.VarChar, 20, false)]
		public string CallRid
		{
			get
			{
				return _CallRid;
			}
			set
			{
				_CallRid = value == null ? String.Empty : value.Trim();
				//寧願 cute 字也不要紀錄失敗
				if (_CallRid != null && _CallRid.Length > 2000)
				{
					_CallRid = _CallRid.Substring(0, 2000);
				}
			}
		}

		private string _ErrorMsg = null;
		/// <summary>
		/// 錯誤訊息
		/// </summary>
		[FieldSpec(Field.ErrorMsg, false, FieldTypeEnum.NVarChar, 2000, false)]
		public string ErrorMsg
		{
			get
			{
				return _ErrorMsg;
			}
			set
			{
				_ErrorMsg = value == null ? String.Empty : value.Trim();
				//寧願 cute 字也不要紀錄失敗
				if (_ErrorMsg != null && _ErrorMsg.Length > 2000)
				{
					_ErrorMsg = _ErrorMsg.Substring(0, 2000);
				}
			}
		}

		private string _ReturnMsg = null;
		/// <summary>
		/// 回傳訊息
		/// </summary>
		[FieldSpec(Field.ReturnMsg, false, FieldTypeEnum.NVarChar, 2000, false)]
		public string ReturnMsg
		{
			get
			{
				return _ReturnMsg;
			}
			set
			{
				_ReturnMsg = value == null ? null : value.Trim();
				//寧願 cute 字也不要紀錄失敗
				if (_ReturnMsg != null && _ReturnMsg.Length > 2000)
				{
					_ReturnMsg = _ReturnMsg.Substring(0, 2000);
				}
			}
		}

		/// <summary>
		/// 資料建立日期
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
