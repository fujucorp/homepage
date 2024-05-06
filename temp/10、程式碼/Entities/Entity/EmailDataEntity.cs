/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/04/14 17:21:22
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
	/// 繳費通知資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class EmailDataEntity : Entity
	{
		public const string TABLE_NAME = "Email_Data";

		#region Field Name Const Class
		/// <summary>
		/// EmailDataEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 銷帳編號
			/// </summary>
			public const string CancelNo = "Cancel_No";
			#endregion

			#region Data
			/// <summary>
			/// 學生信箱
			/// </summary>
			public const string StuEmail = "Stu_Email";

			/// <summary>
			/// 學生名稱
			/// </summary>
			public const string StuName = "Stu_Name";

			/// <summary>
			/// 繳款期限 (民國年日期 7 碼)
			/// </summary>
			public const string DueDate = "Due_Date";

			/// <summary>
			/// 應繳金額
			/// </summary>
			public const string Amount = "Amount";

			/// <summary>
			/// 待寄信日期 (西元年日期 8 碼)
			/// </summary>
			public const string EmailDate = "Email_Date";

			/// <summary>
			/// 實際發送信件日期
			/// </summary>
			public const string SendDate = "Send_Date";

			/// <summary>
			/// 代收類別代碼
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 學年代碼
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
			/// 學期代碼
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
			/// 部別代碼
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
			/// 收費用別代碼
			/// </summary>
			public const string ReceiveId = "Receive_Id";

			/// <summary>
			/// 學號
			/// </summary>
			public const string StuId = "Stu_Id";

			/// <summary>
			/// 身份證字號
			/// </summary>
			public const string Personid = "PersonId";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// EmailDataEntity 類別建構式
		/// </summary>
		public EmailDataEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _CancelNo = null;
		/// <summary>
		/// 銷帳編號
		/// </summary>
		[FieldSpec(Field.CancelNo, true, FieldTypeEnum.VarChar, 16, false)]
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
		#endregion

		#region Data
		/// <summary>
		/// 學生信箱
		/// </summary>
		[FieldSpec(Field.StuEmail, false, FieldTypeEnum.VarChar, 50, false)]
		public string StuEmail
		{
			get;
			set;
		}

		/// <summary>
		/// 學生名稱
		/// </summary>
		[FieldSpec(Field.StuName, false, FieldTypeEnum.NVarChar, 60, true)]
		public string StuName
		{
			get;
			set;
		}

		/// <summary>
		/// 繳款期限 (民國年日期 7 碼)
		/// </summary>
		[FieldSpec(Field.DueDate, false, FieldTypeEnum.VarChar, 8, false)]
		public string DueDate
		{
			get;
			set;
		}

		/// <summary>
		/// 應繳金額
		/// </summary>
		[FieldSpec(Field.Amount, false, FieldTypeEnum.Decimal, false)]
		public decimal Amount
		{
			get;
			set;
		}

		/// <summary>
		/// 待寄信日期 (西元年日期 8 碼)
		/// </summary>
		[FieldSpec(Field.EmailDate, false, FieldTypeEnum.VarChar, 8, false)]
		public string EmailDate
		{
			get;
			set;
		}

		/// <summary>
		/// 實際發送信件日期
		/// </summary>
		[FieldSpec(Field.SendDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? SendDate
		{
			get;
			set;
		}

		/// <summary>
		/// 代收類別代碼
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 6, false)]
		public string ReceiveType
		{
			get;
			set;
		}

		/// <summary>
		/// 學年代碼
		/// </summary>
		[FieldSpec(Field.YearId, false, FieldTypeEnum.Char, 3, false)]
		public string YearId
		{
			get;
			set;
		}

		/// <summary>
		/// 學期代碼
		/// </summary>
		[FieldSpec(Field.TermId, false, FieldTypeEnum.Char, 1, false)]
		public string TermId
		{
			get;
			set;
		}

		/// <summary>
		/// 部別代碼
		/// </summary>
		[FieldSpec(Field.DepId, false, FieldTypeEnum.Char, 1, false)]
		public string DepId
		{
			get;
			set;
		}

		/// <summary>
		/// 收費用別代碼
		/// </summary>
		[FieldSpec(Field.ReceiveId, false, FieldTypeEnum.VarChar, 2, false)]
		public string ReceiveId
		{
			get;
			set;
		}

		/// <summary>
		/// 學號
		/// </summary>
		[FieldSpec(Field.StuId, false, FieldTypeEnum.VarChar, 20, false)]
		public string StuId
		{
			get;
			set;
		}

		/// <summary>
		/// 身份證字號
		/// </summary>
		[FieldSpec(Field.Personid, false, FieldTypeEnum.VarChar, 12, false)]
		public string Personid
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
