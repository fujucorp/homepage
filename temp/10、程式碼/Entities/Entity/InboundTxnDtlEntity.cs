/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2016/12/11 12:14:58
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
	/// 支付寶交易資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class InboundTxnDtlEntity : Entity
	{
		public const string TABLE_NAME = "InboundTxnDtl";

		#region Field Name Const Class
		/// <summary>
		/// 支付寶交易資料 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 流水號 (Identity)
			/// </summary>
			public const string Sn = "sn";
			#endregion

			#region Data
			/// <summary>
			/// 交易日期時間
			/// </summary>
			public const string TxnTime = "txn_time";

			/// <summary>
			/// 學生繳費單的商家代號
			/// </summary>
			public const string ReceiveType = "receive_type";

			/// <summary>
			/// 學生繳費單的學年代碼
			/// </summary>
			public const string YearId = "year_id";

			/// <summary>
			/// 學生繳費單的學期代碼
			/// </summary>
			public const string TermId = "term_id";

			/// <summary>
			/// 學生繳費單的部別代碼 (土銀固定為空白)
			/// </summary>
			public const string DepId = "dep_id";

			/// <summary>
			/// 學生繳費單的代收費用別代碼
			/// </summary>
			public const string ReceiveId = "receive_id";

			/// <summary>
			/// 學生繳費單的學號
			/// </summary>
			public const string StuId = "stu_id";

			/// <summary>
			/// 學生繳費單的資料序號
			/// </summary>
			public const string Seq = "seq";

			/// <summary>
			/// 學生繳費單的銷帳編號
			/// </summary>
			public const string CancelNo = "cancel_no";

			/// <summary>
			/// 學生繳費單的應繳金額
			/// </summary>
			public const string ReceiveAmount = "receive_amount";

			/// <summary>
			/// 手續費
			/// </summary>
			public const string Fee = "fee";

			/// <summary>
			/// 交易金額 (應繳總額)
			/// </summary>
			public const string Amount = "amount";

			/// <summary>
			/// 訂單編號
			/// </summary>
			public const string OrderNumber = "ordernumber";

			/// <summary>
			/// 交易狀態 (0：交易請求; 1：交易回應)
			/// </summary>
			public const string Status = "status";

			/// <summary>
			/// 回傳訊息
			/// </summary>
			public const string TxnMsg = "txn_msg";

			/// <summary>
			/// 資料最後修改者
			/// </summary>
			public const string MdyUser = "mdy_user";

			/// <summary>
			/// 資料最後修改日期時間
			/// </summary>
			public const string MdyTime = "mdy_time";

			/// <summary>
			/// 支付寶交易回覆檔檔名
			/// </summary>
			public const string InboundFile = "inbound_file";

			/// <summary>
			/// 支付寶交易回覆檔資料
			/// </summary>
			public const string InboundData = "inbound_data";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// 支付寶交易資料承載類別 建構式
		/// </summary>
		public InboundTxnDtlEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		/// <summary>
		/// 流水號 (Identity)
		/// </summary>
		[FieldSpec(Field.Sn, true, FieldTypeEnum.Identity, false)]
		public Int64 Sn
		{
			get;
			set;
		}
		#endregion

		#region Data
		/// <summary>
		/// 交易日期時間
		/// </summary>
		[FieldSpec(Field.TxnTime, false, FieldTypeEnum.DateTime, false)]
		public DateTime TxnTime
		{
			get;
			set;
		}

		/// <summary>
		/// 學生繳費單的商家代號
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 6, false)]
		public string ReceiveType
		{
			get;
			set;
		}

		/// <summary>
		/// 學生繳費單的學年代碼
		/// </summary>
		[FieldSpec(Field.YearId, false, FieldTypeEnum.VarChar, 3, false)]
		public string YearId
		{
			get;
			set;
		}

		/// <summary>
		/// 學生繳費單的學期代碼
		/// </summary>
		[FieldSpec(Field.TermId, false, FieldTypeEnum.Char, 1, false)]
		public string TermId
		{
			get;
			set;
		}

		/// <summary>
		/// 學生繳費單的部別代碼 (土銀固定為空白)
		/// </summary>
		[FieldSpec(Field.DepId, false, FieldTypeEnum.Char, 1, false)]
		public string DepId
		{
			get;
			set;
		}

		/// <summary>
		/// 學生繳費單的代收費用別代碼
		/// </summary>
		[FieldSpec(Field.ReceiveId, false, FieldTypeEnum.VarChar, 2, false)]
		public string ReceiveId
		{
			get;
			set;
		}

		/// <summary>
		/// 學生繳費單的學號
		/// </summary>
		[FieldSpec(Field.StuId, false, FieldTypeEnum.VarChar, 20, false)]
		public string StuId
		{
			get;
			set;
		}

		/// <summary>
		/// 學生繳費單的資料序號
		/// </summary>
		[FieldSpec(Field.Seq, false, FieldTypeEnum.Integer, false)]
		public int Seq
		{
			get;
			set;
		}

		/// <summary>
		/// 學生繳費單的銷帳編號
		/// </summary>
		[FieldSpec(Field.CancelNo, false, FieldTypeEnum.VarChar, 20, false)]
		public string CancelNo
		{
			get;
			set;
		}

		/// <summary>
		/// 學生繳費單的應繳金額
		/// </summary>
		[FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, false)]
		public decimal ReceiveAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 手續費
		/// </summary>
		[FieldSpec(Field.Fee, false, FieldTypeEnum.Decimal, false)]
		public decimal Fee
		{
			get;
			set;
		}

		/// <summary>
		/// 交易金額 (應繳總額)
		/// </summary>
		[FieldSpec(Field.Amount, false, FieldTypeEnum.Decimal, false)]
		public decimal Amount
		{
			get;
			set;
		}

		/// <summary>
		/// 訂單編號
		/// </summary>
		[FieldSpec(Field.OrderNumber, false, FieldTypeEnum.VarChar, 20, false)]
		public string OrderNumber
		{
			get;
			set;
		}

		/// <summary>
		/// 交易狀態 (0：交易請求; 1：交易回應)
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.Char, 1, false)]
		public string Status
		{
			get;
			set;
		}

		/// <summary>
		/// 回傳訊息
		/// </summary>
		[FieldSpec(Field.TxnMsg, false, FieldTypeEnum.NVarChar, 1024, false)]
		public string TxnMsg
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

		/// <summary>
		/// 資料最後修改日期時間
		/// </summary>
		[FieldSpec(Field.MdyTime, false, FieldTypeEnum.DateTime, true)]
		public DateTime? MdyTime
		{
			get;
			set;
		}

		private string _InboundFile = null;
		/// <summary>
		/// 支付寶交易回覆檔檔名
		/// </summary>
		[FieldSpec(Field.InboundFile, false, FieldTypeEnum.NVarChar, 32, true)]
		public string InboundFile
		{
			get
			{
				return _InboundFile;
			}
			set
			{
				_InboundFile = value == null ? null : value.Trim();
			}
		}

		/// <summary>
		/// 支付寶交易回覆檔資料
		/// </summary>
		[FieldSpec(Field.InboundData, false, FieldTypeEnum.NVarChar, 500, true)]
		public string InboundData
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
