/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/08/17 11:37:10
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
	/// 連動製單服務日誌資料
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class SchoolServiceLogEntity : Entity
	{
		public const string TABLE_NAME = "School_Service_Log";

		#region Field Name Const Class
		/// <summary>
		/// 連動製單服務日誌資料 欄位名稱定義抽象類別
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
			/// 系統代碼
			/// </summary>
			public const string SysId = "Sys_Id";

			#region [MDY:20220530] Checkmarx 調整
			#region [MDY:20210401] 原碼修正
			/// <summary>
			/// 系統驗證碼
			/// </summary>
			public const string SysPXX = "Sys_Pwd";
			#endregion
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
		}
		#endregion

		#region Constructor
		/// <summary>
		/// SchoolServiceLogEntity 類別建構式
		/// </summary>
		public SchoolServiceLogEntity()
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
		private string _SysId = null;
		/// <summary>
		/// 系統代碼
		/// </summary>
		[FieldSpec(Field.SysId, false, FieldTypeEnum.VarChar, 32, false)]
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
				_ClientIp = value == null ? null : value.Trim();
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
				_MethodName = value == null ? null : value.Trim();
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
				_MethodArgs = value == null ? null : value.Trim();
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
                #region [MDY:20160424] 修正資料庫不允許 null 的問題
                if (value == null)
                {
                    _ErrorMsg = String.Empty;
                }
                else
                {
                    _ErrorMsg = value.Trim();
                    //寧願 cute 字也不要紀錄失敗
                    if (_ErrorMsg.Length > 2000)
                    {
                        _ErrorMsg = _ErrorMsg.Substring(0, 2000);
                    }
                }
                #endregion
			}
		}

		private string _ReturnMsg = null;
		/// <summary>
		/// 回傳訊息
		/// </summary>
		[FieldSpec(Field.ReturnMsg, false, FieldTypeEnum.NVarChar, 200, false)]
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
