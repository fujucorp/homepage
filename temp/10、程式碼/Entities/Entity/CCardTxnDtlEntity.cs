/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/09 13:29:59
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
	/// 財金 (EZPOS) 交易資料 承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class CCardTxnDtlEntity : Entity
	{
		public const string TABLE_NAME = "CCardTxnDtl";

		#region Field Name Const Class
		/// <summary>
		/// CCardTxnDtlEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 交易序號 (PKey)
			/// </summary>
			public const string TxnId = "TXN_ID";
			#endregion

			#region Data
			/// <summary>
			/// 銷帳編號
			/// </summary>
			public const string Rid = "RID";

			/// <summary>
			/// 持卡人身份證字號
			/// </summary>
			public const string PayId = "PAY_ID";

			/// <summary>
			/// 學生身份證字號
			/// </summary>
			public const string StudentId = "STUDEND_ID";

			/// <summary>
			/// 學生學號
			/// </summary>
			public const string StudentNo = "STUDEND_NO";

			/// <summary>
			/// 應繳金額
			/// </summary>
			public const string Amount = "AMOUNT";

			/// <summary>
			/// 交易通道代碼 (1=國內信用卡; 4=國際信用卡)
			/// </summary>
			public const string ApNo = "AP_NO";

			/// <summary>
			/// 授權交易序號 (EZPOS 回覆 xid)
			/// </summary>
			public const string Xid = "XID";

			/// <summary>
			/// 狀態 (1=交易處理中; 2=交易成功; 3=交易失敗)
			/// </summary>
			public const string Status = "STATUS";

			/// <summary>
			/// 發卡銀行代碼
			/// </summary>
			public const string TxnBankId = "TXN_BANKID";

			/// <summary>
			/// 交易結果，成功則紀錄持卡人信用卡末四碼 (EZPOS 回覆 lastPan4)，交易失敗則紀錄授權狀態與錯誤代碼 (EZPOS 回覆 status:errcode)
			/// </summary>
			public const string TxnResult = "TXN_RESULT";

			/// <summary>
			/// 交易授權碼 (EZPOS 回覆 authCode)
			/// </summary>
			public const string TxnAuthCode = "TXN_AUTHCODE";

			/// <summary>
			/// 保留
			/// </summary>
			public const string TxnAuthDate = "TXN_AUTHDATE";

			/// <summary>
			/// 保留
			/// </summary>
			public const string TxnSettleDate = "TXN_SETTLEDATE";

			/// <summary>
			/// 保留
			/// </summary>
			public const string TxnHostDate = "TXN_HOSTDATE";

			/// <summary>
			/// 交易授權失敗原因 (EZPOS 回覆 errDesc)
			/// </summary>
			public const string TxnRemark = "TXN_REMARK";

			/// <summary>
			/// 建立日期時間
			/// </summary>
			public const string CreateDate = "CREATE_DATE";

			/// <summary>
			/// 最後更新日期時間
			/// </summary>
			public const string UpdateDate = "UPDATE_DATE";

			/// <summary>
			/// 交易授權狀態 (EZPOS 回覆 status)
			/// </summary>
			public const string TxnStatus = "TXN_STATUS";

			/// <summary>
			/// 紀錄對應 StudentReceive 資料的其他 KEY（Year_Id,TermId,Receive_Id,Old_Seq）
			/// </summary>
			public const string UpdateMan = "Update_Man";

            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 紀錄 StudentReceive 的 PKey
            /// <summary>
            /// 商家代號
            /// </summary>
            public const string ReceiveType = "RECEIVE_TYPE";

            /// <summary>
            /// 學年代碼
            /// </summary>
            public const string YearId = "YEAR_ID";

            /// <summary>
            /// 學期代碼
            /// </summary>
            public const string TermId = "TERM_ID";

            /// <summary>
            /// 部別代碼 (土銀不使用此部別欄位，固定設為空字串)
            /// </summary>
            public const string DepId = "DEP_ID";

            /// <summary>
            /// 代收費用別代碼
            /// </summary>
            public const string ReceiveId = "RECEIVE_ID";

            /// <summary>
            /// 同資料序號
            /// </summary>
            public const string OldSeq = "OLD_SEQ";
            #endregion

			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// CCardTxnDtlEntity 類別建構式
		/// </summary>
		public CCardTxnDtlEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _TxnId = null;
		/// <summary>
		/// 交易序號 (PKey)
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
		[FieldSpec(Field.Rid, false, FieldTypeEnum.VarChar, 16, false)]
		public string Rid
		{
			get;
			set;
		}

		/// <summary>
		/// 持卡人身份證字號
		/// </summary>
		[FieldSpec(Field.PayId, false, FieldTypeEnum.VarChar, 10, false)]
		public string PayId
		{
			get;
			set;
		}

		/// <summary>
		/// 學生身份證字號
		/// </summary>
		[FieldSpec(Field.StudentId, false, FieldTypeEnum.VarChar, 50, true)]
		public string StudentId
		{
			get;
			set;
		}

		/// <summary>
		/// 學生學號
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
		/// 交易通道代碼 (1=國內信用卡; 4=國際信用卡)
		/// </summary>
		[FieldSpec(Field.ApNo, false, FieldTypeEnum.Integer, true)]
		public int? ApNo
		{
			get;
			set;
		}

		/// <summary>
		/// 授權交易序號 (EZPOS 回覆 xid)
		/// </summary>
		[FieldSpec(Field.Xid, false, FieldTypeEnum.VarChar, 50, true)]
		public string Xid
		{
			get;
			set;
		}

		/// <summary>
		/// 狀態 (1=交易處理中; 2=交易成功; 3=交易失敗)
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 10, true)]
		public string Status
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
		/// 交易結果，成功則紀錄持卡人信用卡末四碼 (EZPOS 回覆 lastPan4)，交易失敗則紀錄授權狀態與錯誤代碼 (EZPOS 回覆 status:errcode)
		/// </summary>
		[FieldSpec(Field.TxnResult, false, FieldTypeEnum.VarChar, 200, true)]
		public string TxnResult
		{
			get;
			set;
		}

		/// <summary>
		/// 交易授權碼 (EZPOS 回覆 authCode)
		/// </summary>
		[FieldSpec(Field.TxnAuthCode, false, FieldTypeEnum.VarChar, 6, true)]
		public string TxnAuthCode
		{
			get;
			set;
		}

		/// <summary>
		/// 授權處理回應時間
		/// </summary>
		[FieldSpec(Field.TxnAuthDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? TxnAuthDate
		{
			get;
			set;
		}

		/// <summary>
		/// 保留
		/// </summary>
		[FieldSpec(Field.TxnSettleDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? TxnSettleDate
		{
			get;
			set;
		}

		/// <summary>
		/// 保留
		/// </summary>
		[FieldSpec(Field.TxnHostDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? TxnHostDate
		{
			get;
			set;
		}

		/// <summary>
		/// 交易授權失敗原因 (EZPOS 回覆 errDesc)
		/// </summary>
		[FieldSpec(Field.TxnRemark, false, FieldTypeEnum.VarChar, 500, true)]
		public string TxnRemark
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

		/// <summary>
		/// 交易授權狀態 (EZPOS 回覆 status)
		/// </summary>
		[FieldSpec(Field.TxnStatus, false, FieldTypeEnum.NVarChar, 10, true)]
		public string TxnStatus
		{
			get;
			set;
		}

		/// <summary>
		/// 紀錄對應 StudentReceive 資料的其他 KEY（Year_Id,TermId,Receive_Id,Old_Seq）
		/// </summary>
		[FieldSpec(Field.UpdateMan, false, FieldTypeEnum.VarChar, 20, true)]
		public string UpdateMan
		{
			get;
			set;
		}

        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 紀錄 StudentReceive 的 PKey
        private string _ReceiveType = null;
        /// <summary>
        /// 商家代號
        /// </summary>
        [FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 6, true)]
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

        private string _YearId = null;
        /// <summary>
        /// 學年代碼
        /// </summary>
        [FieldSpec(Field.YearId, false, FieldTypeEnum.Char, 3, true)]
        public string YearId
        {
            get
            {
                return _YearId;
            }
            set
            {
                _YearId = value == null ? null : value.Trim();
            }
        }

        private string _TermId = null;
        /// <summary>
        /// 學期代碼
        /// </summary>
        [FieldSpec(Field.TermId, false, FieldTypeEnum.Char, 1, true)]
        public string TermId
        {
            get
            {
                return _TermId;
            }
            set
            {
                _TermId = value == null ? null : value.Trim();
            }
        }

        private string _DepId = null;
        /// <summary>
        /// 部別代碼 (土銀不使用此部別欄位，固定設為空字串)
        /// </summary>
        [FieldSpec(Field.DepId, false, FieldTypeEnum.Char, 1, true)]
        public string DepId
        {
            get
            {
                return _DepId;
            }
            set
            {
                _DepId = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveId = null;
        /// <summary>
        /// 代收費用別代碼
        /// </summary>
        [FieldSpec(Field.ReceiveId, false, FieldTypeEnum.VarChar, 2, true)]
        public string ReceiveId
        {
            get
            {
                return _ReceiveId;
            }
            set
            {
                _ReceiveId = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 同5KEY下同學號的資料序號 (不指定則預設為 0，指定時只允許1~999，超過則為轉置的舊資料序號)
        /// </summary>
        [FieldSpec(Field.OldSeq, false, FieldTypeEnum.Integer, true)]
        public int? OldSeq
        {
            get;
            set;
        }
        #endregion

		#endregion
		#endregion
	}
}
