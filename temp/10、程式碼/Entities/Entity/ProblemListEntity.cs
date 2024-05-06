/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/04/14 16:13:10
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
	/// 銷帳問題檔資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ProblemListEntity : Entity
	{
		public const string TABLE_NAME = "Problem_List";

		#region Field Name Const Class
		/// <summary>
		/// ProblemListEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 問題檔編號 (Identity)
			/// </summary>
			public const string Id = "id";
			#endregion

			#region Data
			/// <summary>
			/// 商家代號
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 銷帳編號
			/// </summary>
			public const string CancelNo = "Cancel_No";

			/// <summary>
			/// 問題註記 (1:更新繳費資料失敗; 2:金額不符; 3:檢碼不符; 4:無此銷帳編號; 5:重複繳費; 6:尚未全繳)
			/// </summary>
			public const string ProblemFlag = "Problem_Flag";

			/// <summary>
			/// 銀行代碼 (土銀使用6碼)
			/// </summary>
			public const string BankId = "Bank_Id";

			/// <summary>
			/// 代收日期
			/// </summary>
			public const string ReceiveDate = "Receive_Date";

			/// <summary>
			/// 代收時間 (HHmmss)
			/// </summary>
			public const string ReceiveTime = "Receive_Time";

			/// <summary>
			/// 入帳日期
			/// </summary>
			public const string AccountDate = "Account_Date";

			/// <summary>
			/// 手續費金額 (土銀沒使用)
			/// </summary>
			public const string ProcessFee = "Process_Fee";

			/// <summary>
			/// 代收金額
			/// </summary>
			public const string PayAmount = "Pay_Amount";

			/// <summary>
			/// 問題說明
			/// </summary>
			public const string ProblemRemark = "Problem_Remark";

			/// <summary>
			/// 代收管道代碼
			/// </summary>
			public const string ReceiveWay = "Receive_Way";

			/// <summary>
			/// EB 的銷帳編號 (土銀沒使用)
			/// </summary>
			public const string CanceleEBNno = "Cancel_EBNo";

			/// <summary>
			/// Up_Flag (土銀沒使用)
			/// </summary>
			public const string UpFlag = "Up_Flag";

			/// <summary>
			/// 應繳金額
			/// </summary>
			public const string ReceiveAmount = "Receive_Amount";

			/// <summary>
			/// 銷帳金額 (土銀沒使用)
			/// </summary>
			public const string CancelReceive = "Cancel_Receive";

			/// <summary>
			/// Fee_Receivable (土銀沒使用)
			/// </summary>
			public const string FeeReceivable = "Fee_Receivable";

			/// <summary>
			/// Fee_Payable (土銀沒使用)
			/// </summary>
			public const string FeePayable = "Fee_Payable";

			/// <summary>
			/// P_flag (土銀沒使用)
			/// </summary>
			public const string PFlag = "P_flag";

			/// <summary>
			/// 建立日期
			/// </summary>
			public const string CreateDate = "create_date";

			/// <summary>
			/// 建立者
			/// </summary>
			public const string CreateMan = "create_man";

			/// <summary>
			/// 最後更新日期
			/// </summary>
			public const string UpdateDate = "update_date";

			/// <summary>
			/// 最後更新者
			/// </summary>
			public const string UpdateMan = "update_man";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// ProblemListEntity 類別建構式
		/// </summary>
		public ProblemListEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		/// <summary>
		/// 問題檔編號 (Identity)
		/// </summary>
		[FieldSpec(Field.Id, true, FieldTypeEnum.Identity, false)]
		public int Id
		{
			get;
			set;
		}
		#endregion

		#region Data
		/// <summary>
        /// 商家代號
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 6, false)]
		public string ReceiveType
		{
			get;
			set;
		}

		/// <summary>
		/// 銷帳編號
		/// </summary>
		[FieldSpec(Field.CancelNo, false, FieldTypeEnum.VarChar, 16, false)]
		public string CancelNo
		{
			get;
			set;
		}

		/// <summary>
		/// 問題註記 (1:更新繳費資料失敗; 2:金額不符; 3:檢碼不符; 4:無此銷帳編號; 5:重複繳費; 6:尚未全繳)
		/// </summary>
		[FieldSpec(Field.ProblemFlag, false, FieldTypeEnum.Char, 1, true)]
		public string ProblemFlag
		{
			get;
			set;
		}

		/// <summary>
		/// 銀行代碼 (土銀使用6碼)
		/// </summary>
		[FieldSpec(Field.BankId, false, FieldTypeEnum.VarChar, 7, true)]
		public string BankId
		{
			get;
			set;
		}

		/// <summary>
		/// 代收日期
		/// </summary>
		[FieldSpec(Field.ReceiveDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? ReceiveDate
		{
			get;
			set;
		}

		/// <summary>
		/// 代收時間
		/// </summary>
		[FieldSpec(Field.ReceiveTime, false, FieldTypeEnum.Char, 6, true)]
		public string ReceiveTime
		{
			get;
			set;
		}

		/// <summary>
		/// 入帳日期
		/// </summary>
		[FieldSpec(Field.AccountDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? AccountDate
		{
			get;
			set;
		}

		/// <summary>
		/// 手續費金額 (土銀沒使用)
		/// </summary>
		[FieldSpec(Field.ProcessFee, false, FieldTypeEnum.Decimal, true)]
		public decimal? ProcessFee
		{
			get;
			set;
		}

		/// <summary>
		/// 代收金額 (土銀沒使用)
		/// </summary>
		[FieldSpec(Field.PayAmount, false, FieldTypeEnum.Decimal, true)]
		public decimal? PayAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 問題說明
		/// </summary>
		[FieldSpec(Field.ProblemRemark, false, FieldTypeEnum.VarChar, 256, true)]
		public string ProblemRemark
		{
			get;
			set;
		}

		/// <summary>
		/// 代收管道代碼
		/// </summary>
		[FieldSpec(Field.ReceiveWay, false, FieldTypeEnum.VarChar, 2, true)]
		public string ReceiveWay
		{
			get;
			set;
		}

		/// <summary>
		/// EB 的銷帳編號 (土銀沒使用)
		/// </summary>
		[FieldSpec(Field.CanceleEBNno, false, FieldTypeEnum.VarChar, 16, true)]
		public string CanceleEBNno
		{
			get;
			set;
		}

		/// <summary>
		/// Up_Flag (土銀沒使用)
		/// </summary>
		[FieldSpec(Field.UpFlag, false, FieldTypeEnum.Char, 1, true)]
		public string UpFlag
		{
			get;
			set;
		}

		/// <summary>
		/// 應繳金額
		/// </summary>
		[FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReceiveAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 銷帳金額 (土銀沒使用)
		/// </summary>
		[FieldSpec(Field.CancelReceive, false, FieldTypeEnum.Decimal, true)]
		public decimal? CancelReceive
		{
			get;
			set;
		}

		/// <summary>
		/// Fee_Receivable (土銀沒使用)
		/// </summary>
		[FieldSpec(Field.FeeReceivable, false, FieldTypeEnum.Decimal, true)]
		public decimal? FeeReceivable
		{
			get;
			set;
		}

		/// <summary>
		/// Fee_Payable (土銀沒使用)
		/// </summary>
		[FieldSpec(Field.FeePayable, false, FieldTypeEnum.Decimal, true)]
		public decimal? FeePayable
		{
			get;
			set;
		}

		/// <summary>
		/// P_flag (土銀沒使用)
		/// </summary>
		[FieldSpec(Field.PFlag, false, FieldTypeEnum.Char, 1, true)]
		public string PFlag
		{
			get;
			set;
		}

		/// <summary>
		/// 建立日期
		/// </summary>
		[FieldSpec(Field.CreateDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? CreateDate
		{
			get;
			set;
		}

		/// <summary>
		/// 建立者
		/// </summary>
		[FieldSpec(Field.CreateMan, false, FieldTypeEnum.VarChar, 50, true)]
		public string CreateMan
		{
			get;
			set;
		}

		/// <summary>
		/// 最後更新日期
		/// </summary>
		[FieldSpec(Field.UpdateDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? UpdateDate
		{
			get;
			set;
		}

		/// <summary>
		/// 最後更新者
		/// </summary>
		[FieldSpec(Field.UpdateMan, false, FieldTypeEnum.VarChar, 50, true)]
		public string UpdateMan
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
