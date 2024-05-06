/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/10 02:13:36
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
	/// e政府 (eGov) 交易資料 承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ChtTxnDtlEntity : Entity
	{
		public const string TABLE_NAME = "ChtTxnDtl";

		#region Field Name Const Class
		/// <summary>
		/// ChtTxnDtlEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 交易序號 (PKey) (由 eGov 發給 SessionTransactionKey)
			/// </summary>
			public const string TxnId = "txn_id";
			#endregion

			#region Data
			/// <summary>
			/// 銷帳編號
			/// </summary>
			public const string Rid = "rid";

			/// <summary>
			/// 持卡人身份證字號
			/// </summary>
			public const string PayId = "pay_id";

			/// <summary>
			/// 學生學號
			/// </summary>
			public const string StudentId = "student_id";

			/// <summary>
			/// 學生身份證字號
			/// </summary>
			public const string StudentNo = "student_no";

			/// <summary>
			/// 應繳金額
			/// </summary>
			public const string Amount = "amount";

			/// <summary>
			/// 狀態 (1=交易處理中; 2=交易成功; 3=交易失敗)
			/// </summary>
			public const string Status = "status";

			/// <summary>
			/// 交易結果 (eGov 回覆 TransactionResult)
			/// </summary>
			public const string TxnResult = "txn_result";

			/// <summary>
			/// 保留
			/// </summary>
			public const string ResultInfo = "result_info";

			/// <summary>
			/// 發卡銀行代碼
			/// </summary>
			public const string TxnBankId = "TXN_BANKID";

			/// <summary>
			/// 交易授權碼 (eGov 回覆 ApproveNo)
			/// </summary>
			public const string TxnAuthCode = "txn_authcode";

			/// <summary>
			/// 交易清算日
			/// </summary>
			public const string TxnSettleDate = "txn_settle_date";

			/// <summary>
			/// 交易授權日 (eGov 回覆 AuthDate)
			/// </summary>
			public const string TxnAuthDate = "txn_auth_date";

			/// <summary>
			/// 主機交易時間 (eGov 回覆 HostTime)
			/// </summary>
			public const string TxnHostDate = "txn_host_date";

			/// <summary>
			/// 建立日期時間
			/// </summary>
			public const string CreateDate = "create_date";

			/// <summary>
			/// 最後更新日期時間
			/// </summary>
			public const string UpdateDate = "update_date";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// ChtTxnDtlEntity 類別建構式
		/// </summary>
		public ChtTxnDtlEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _TxnId = null;
		/// <summary>
		/// 交易序號 (PKey) (由 eGov 發給 SessionTransactionKey)
		/// </summary>
		[FieldSpec(Field.TxnId, true, FieldTypeEnum.VarChar, 20, false)]
		public string TxnId
		{
			get
			{
				return _TxnId;
			}
			set
			{
				_TxnId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// 銷帳編號
		/// </summary>
		[FieldSpec(Field.Rid, false, FieldTypeEnum.VarChar, 16, true)]
		public string Rid
		{
			get;
			set;
		}

		/// <summary>
		/// 持卡人身份證字號
		/// </summary>
		[FieldSpec(Field.PayId, false, FieldTypeEnum.VarChar, 12, true)]
		public string PayId
		{
			get;
			set;
		}

		/// <summary>
		/// 學生學號
		/// </summary>
		[FieldSpec(Field.StudentId, false, FieldTypeEnum.VarChar, 12, true)]
		public string StudentId
		{
			get;
			set;
		}

		/// <summary>
		/// 學生身份證字號
		/// </summary>
		[FieldSpec(Field.StudentNo, false, FieldTypeEnum.VarChar, 20, true)]
		public string StudentNo
		{
			get;
			set;
		}

		/// <summary>
		/// 應繳金額
		/// </summary>
		[FieldSpec(Field.Amount, false, FieldTypeEnum.Decimal, true)]
		public decimal? Amount
		{
			get;
			set;
		}

		/// <summary>
		/// 狀態 (1=交易處理中; 2=交易成功; 3=交易失敗)
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.Integer, true)]
		public int? Status
		{
			get;
			set;
		}

		/// <summary>
		/// 交易結果 (eGov 回覆 TransactionResult)
		/// </summary>
		[FieldSpec(Field.TxnResult, false, FieldTypeEnum.VarChar, 200, true)]
		public string TxnResult
		{
			get;
			set;
		}

		/// <summary>
		/// 保留
		/// </summary>
		[FieldSpec(Field.ResultInfo, false, FieldTypeEnum.VarChar, 200, true)]
		public string ResultInfo
		{
			get;
			set;
		}

		/// <summary>
		/// 發卡銀行代碼
		/// </summary>
		[FieldSpec(Field.TxnBankId, false, FieldTypeEnum.VarChar, 7, true)]
		public string TxnBankId
		{
			get;
			set;
		}

		/// <summary>
		/// 交易授權碼 (eGov 回覆 ApproveNo)
		/// </summary>
		[FieldSpec(Field.TxnAuthCode, false, FieldTypeEnum.VarChar, 6, true)]
		public string TxnAuthCode
		{
			get;
			set;
		}

		/// <summary>
		/// 交易清算日
		/// </summary>
		[FieldSpec(Field.TxnSettleDate, false, FieldTypeEnum.VarChar, 14, true)]
		public string TxnSettleDate
		{
			get;
			set;
		}

		/// <summary>
		/// 交易授權日 (eGov 回覆 AuthDate)
		/// </summary>
		[FieldSpec(Field.TxnAuthDate, false, FieldTypeEnum.VarChar, 14, true)]
		public string TxnAuthDate
		{
			get;
			set;
		}

		/// <summary>
		/// 主機交易時間 (eGov 回覆 HostTime)
		/// </summary>
		[FieldSpec(Field.TxnHostDate, false, FieldTypeEnum.VarChar, 14, true)]
		public string TxnHostDate
		{
			get;
			set;
		}

		/// <summary>
		/// 建立日期時間
		/// </summary>
		[FieldSpec(Field.CreateDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CreateDate
		{
			get;
			set;
		}

		/// <summary>
		/// 最後更新日期時間
		/// </summary>
		[FieldSpec(Field.UpdateDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? UpdateDate
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
