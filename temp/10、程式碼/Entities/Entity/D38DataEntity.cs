/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/06/11 08:57:02
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
	/// D38異動資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class D38DataEntity : Entity
	{
		public const string TABLE_NAME = "D38Data";

		#region Field Name Const Class
		/// <summary>
		/// D38DataEntity 欄位名稱定義抽象類別
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
			/// 異動註記
			/// </summary>
			public const string UpdFlag = "Upd_Flag";

			/// <summary>
			/// 學校代號
			/// </summary>
			public const string SchIdenty = "Sch_Identy";

			/// <summary>
			/// 商家代號
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 銷帳編號
			/// </summary>
			public const string CancelNo = "Cancel_No";

			/// <summary>
			/// 金額
			/// </summary>
			public const string ReceiveAmount = "Receive_Amount";

			/// <summary>
			/// 學號
			/// </summary>
			public const string StuId = "Stu_Id";

			/// <summary>
			/// 姓名
			/// </summary>
			public const string StuName = "Stu_Name";

			/// <summary>
			/// 異動資料
			/// </summary>
			public const string UpdData = "Upd_Data";

			/// <summary>
			/// 異動日期
			/// </summary>
			public const string UpdDate = "Upd_Date";

			/// <summary>
			/// 批次處理序號
			/// </summary>
			public const string JobNo = "Job_No";

			/// <summary>
			/// 狀態 (電文回覆的 replycode 值或 StatusCode)
			/// </summary>
			public const string Status = "status";

			/// <summary>
			/// 發動時間
			/// </summary>
			public const string CrtDate = "crt_date";

			/// <summary>
			/// 發動人員
			/// </summary>
			public const string CrtUser = "crt_user";

            #region [MDY:20160705] 增加此欄位紀錄執行結果 (電文回覆的 replycode 值或 StatusCode)
            /// <summary>
            /// 執行結果 (電文回覆的 replycode (因為沒有 replycode 的文字說明欄位) 或 StatusCode)
            /// </summary>
            public const string Result = "Result";
            #endregion
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// D38DataEntity 類別建構式
		/// </summary>
		public D38DataEntity()
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
		private string _UpdFlag = null;
		/// <summary>
		/// 異動註記
		/// </summary>
		[FieldSpec(Field.UpdFlag, false, FieldTypeEnum.Char, 1, false)]
		public string UpdFlag
		{
			get
			{
				return _UpdFlag;
			}
			set
			{
				_UpdFlag = value == null ? null : value.Trim();
			}
		}

		private string _SchIdenty = null;
		/// <summary>
		/// 學校代號
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

		private string _CancelNo = null;
		/// <summary>
		/// 銷帳編號
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

		/// <summary>
		/// 金額
		/// </summary>
		[FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, false)]
		public decimal ReceiveAmount
		{
			get;
			set;
		}

		private string _StuId = null;
		/// <summary>
		/// 學號
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
		/// 姓名
		/// </summary>
		[FieldSpec(Field.StuName, false, FieldTypeEnum.NVarChar, 60, true)]
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

		private string _UpdData = null;
		/// <summary>
		/// 異動資料
		/// </summary>
		[FieldSpec(Field.UpdData, false, FieldTypeEnum.NVarChar, 1000, true)]
		public string UpdData
		{
			get
			{
				return _UpdData;
			}
			set
			{
				_UpdData = value == null ? null : value.Trim();
			}
		}

		/// <summary>
		/// 異動日期
		/// </summary>
		[FieldSpec(Field.UpdDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? UpdDate
		{
			get;
			set;
		}

		/// <summary>
		/// 異動日期
		/// </summary>
		[FieldSpec(Field.JobNo, false, FieldTypeEnum.Integer, false)]
		public int JobNo
		{
			get;
			set;
		}

		private string _Status = null;
		/// <summary>
		/// 狀態 (電文回覆的 replycode 值或 StatusCode)
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
		public string Status
		{
			get
			{
				return _Status;
			}
			set
			{
				_Status = value == null ? null : value.Trim();
			}
		}

		private DateTime _CrtDate = DateTime.Now;
		/// <summary>
		/// 發動時間
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get
			{
				return _CrtDate;
			}
			set
			{
				_CrtDate = value;
			}
		}

		/// <summary>
		/// 發動人員
		/// </summary>
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
		public string CrtUser
		{
			get;
			set;
		}

        #region [MDY:20160705] 增加此欄位紀錄執行結果 (電文回覆的 replycode 值或 StatusDesc)
        private string _Result = null;
        /// <summary>
        /// 執行結果 (電文回覆的 replycode (因為沒有 replycode 的文字說明欄位) 或 StatusCode)
        /// </summary>
        [FieldSpec(Field.Result, false, FieldTypeEnum.NVarChar, 2000, false)]
        public string Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value == null ? null : value.Trim();
            }
        }
        #endregion
		#endregion
		#endregion

        #region Readonly Property
        /// <summary>
        /// 異動註記文字 (I:新增 U:異動 D:刪除)
        /// </summary>
        [XmlIgnore]
        public string UpdFlagText
        {
            get
            {
                switch (_UpdFlag)
                {
                    case "I":
                        return "新增";
                    case "U":
                        return "異動";
                    case "D":
                        return "刪除";
                    default:
                        return String.Empty;
                }
            }
        }
        #endregion
	}
}
