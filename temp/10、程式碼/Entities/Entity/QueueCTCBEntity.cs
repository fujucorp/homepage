/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/10/25 13:25:23
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
	/// 上傳中國信託繳費單資料
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class QueueCTCBEntity : Entity
	{
		public const string TABLE_NAME = "Queue_CTCB";

		#region Field Name Const Class
		/// <summary>
		/// QueueCTCBEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 流水號 (PKey) (Identity)
			/// </summary>
			public const string SN = "SN";
			#endregion

			#region Data
			/// <summary>
			/// 商家代號
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 學生學號
			/// </summary>
			public const string StuId = "Stu_Id";

			/// <summary>
			/// 學生姓名
			/// </summary>
			public const string StuName = "Stu_Name";

			/// <summary>
			/// 虛擬帳號
			/// </summary>
			public const string CancelNo = "Cancel_No";

			/// <summary>
			/// 應繳金額
			/// </summary>
			public const string ReceiveAmount = "Receive_Amount";

			/// <summary>
			/// 繳費期限
			/// </summary>
			public const string PayDueDate = "Pay_Due_Date";

			/// <summary>
			/// 上傳日期
			/// </summary>
			public const string SendDate = "Send_Date";

			/// <summary>
			/// 上傳戳記
			/// </summary>
			public const string SendStamp = "Send_Stamp";

			/// <summary>
			/// 資料建立日期
			/// </summary>
			public const string CrtDate = "crt_date";

			/// <summary>
			/// 資料建立者
			/// </summary>
			public const string CrtUser = "crt_user";

            /// <summary>
            /// 銷帳旗標 (預設值為 null / Y：已銷 (因為中心不會有這類資料，所以視為已銷)
            /// </summary>
            public const string CancelFlag = "Cancel_Flag";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// QueueCTCBEntity 類別建構式
		/// </summary>
		public QueueCTCBEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private Int64 _SN = 0;
		/// <summary>
		/// 流水號 (PKey) (Identity)
		/// </summary>
		[FieldSpec(Field.SN, true, FieldTypeEnum.Identity, false)]
		public Int64 SN
		{
			get
			{
				return _SN;
			}
			set
			{
				_SN = value < 0 ? 0 : value;
			}
		}
		#endregion

		#region Data
		private string _ReceiveType = null;
		/// <summary>
		/// 商家代號
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

		private string _StuId = null;
		/// <summary>
		/// 學生學號
		/// </summary>
		[FieldSpec(Field.StuId, false, FieldTypeEnum.VarChar, 20, false)]
		public string StuId
		{
			get
			{
				return _StuId;
			}
			set
			{
				_StuId = value == null ? null : value.Trim();
			}
		}

		private string _StuName = null;
		/// <summary>
		/// 學生姓名
		/// </summary>
		[FieldSpec(Field.StuName, false, FieldTypeEnum.NVarChar, 60, false)]
		public string StuName
		{
			get
			{
				return _StuName;
			}
			set
			{
				_StuName = value == null ? null : value.Trim();
			}
		}

		private string _CancelNo = null;
		/// <summary>
		/// 虛擬帳號
		/// </summary>
		[FieldSpec(Field.CancelNo, false, FieldTypeEnum.VarChar, 16, false)]
		public string CancelNo
		{
			get
			{
				return _CancelNo;
			}
			set
			{
				_CancelNo = value == null ? null : value.Trim();
			}
		}

		private decimal _ReceiveAmount = 0;
		/// <summary>
		/// 應繳金額
		/// </summary>
		[FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, false)]
		public decimal ReceiveAmount
		{
			get
			{
				return _ReceiveAmount;
			}
			set
			{
				_ReceiveAmount = value;
			}
		}

		private string _PayDueDate = null;
		/// <summary>
		/// 繳費期限
		/// </summary>
		[FieldSpec(Field.PayDueDate, false, FieldTypeEnum.Char, 7, false)]
		public string PayDueDate
		{
			get
			{
				return _PayDueDate;
			}
			set
			{
				_PayDueDate = value == null ? null : value.Trim();
			}
		}

		private DateTime? _SendDate = null;
		/// <summary>
		/// 上傳日期
		/// </summary>
		[FieldSpec(Field.SendDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? SendDate
		{
			get
			{
				return _SendDate;
			}
			set
			{
				_SendDate = value;
			}
		}

		private string _SendStamp = null;
		/// <summary>
		/// 上傳戳記
		/// </summary>
		[FieldSpec(Field.SendStamp, false, FieldTypeEnum.VarChar, 36, true)]
		public string SendStamp
		{
			get
			{
				return _SendStamp;
			}
			set
			{
				_SendStamp = value == null ? null : value.Trim();
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

		private string _CrtUser = null;
		/// <summary>
		/// 資料建立者
		/// </summary>
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
		public string CrtUser
		{
			get
			{
				return _CrtUser;
			}
			set
			{
				_CrtUser = value == null ? null : value.Trim();
			}
		}

        private string _CancelFlag = null;
        /// <summary>
        /// 銷帳旗標 (預設值為 null / Y：已銷 (因為中心不會有這類資料，所以視為已銷)
        /// </summary>
        [FieldSpec(Field.CancelFlag, false, FieldTypeEnum.Char, 1, true)]
        public string CancelFlag
        {
            get
            {
                return _CancelFlag;
            }
            set
            {
                _CancelFlag = value == null ? null : value.Trim();
            }
        }
		#endregion
		#endregion
	}
}
