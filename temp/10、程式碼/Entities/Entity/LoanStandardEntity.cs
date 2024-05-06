/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:33:55
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
	/// Loan_Standard 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class LoanStandardEntity : Entity
	{
		public const string TABLE_NAME = "Loan_Standard";

		#region Field Name Const Class
		/// <summary>
		/// LoanStandardEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// Receive_Type 欄位名稱常數定義
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// Year_Id 欄位名稱常數定義
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
			/// Term_Id 欄位名稱常數定義
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
			/// Dep_Id 欄位名稱常數定義
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
			/// Receive_Id 欄位名稱常數定義
			/// </summary>
			public const string ReceiveId = "Receive_Id";

			/// <summary>
			/// Loan_Id 欄位名稱常數定義
			/// </summary>
			public const string LoanId = "Loan_Id";
			#endregion

			#region Data
			/// <summary>
			/// Loan_01 欄位名稱常數定義
			/// </summary>
			public const string Loan01 = "Loan_01";

			/// <summary>
			/// Loan_02 欄位名稱常數定義
			/// </summary>
			public const string Loan02 = "Loan_02";

			/// <summary>
			/// Loan_03 欄位名稱常數定義
			/// </summary>
			public const string Loan03 = "Loan_03";

			/// <summary>
			/// Loan_04 欄位名稱常數定義
			/// </summary>
			public const string Loan04 = "Loan_04";

			/// <summary>
			/// Loan_05 欄位名稱常數定義
			/// </summary>
			public const string Loan05 = "Loan_05";

			/// <summary>
			/// Loan_06 欄位名稱常數定義
			/// </summary>
			public const string Loan06 = "Loan_06";

			/// <summary>
			/// Loan_07 欄位名稱常數定義
			/// </summary>
			public const string Loan07 = "Loan_07";

			/// <summary>
			/// Loan_08 欄位名稱常數定義
			/// </summary>
			public const string Loan08 = "Loan_08";

			/// <summary>
			/// Loan_09 欄位名稱常數定義
			/// </summary>
			public const string Loan09 = "Loan_09";

			/// <summary>
			/// Loan_10 欄位名稱常數定義
			/// </summary>
			public const string Loan10 = "Loan_10";

			/// <summary>
			/// Loan_11 欄位名稱常數定義
			/// </summary>
			public const string Loan11 = "Loan_11";

			/// <summary>
			/// Loan_12 欄位名稱常數定義
			/// </summary>
			public const string Loan12 = "Loan_12";

			/// <summary>
			/// Loan_13 欄位名稱常數定義
			/// </summary>
			public const string Loan13 = "Loan_13";

			/// <summary>
			/// Loan_14 欄位名稱常數定義
			/// </summary>
			public const string Loan14 = "Loan_14";

			/// <summary>
			/// Loan_15 欄位名稱常數定義
			/// </summary>
			public const string Loan15 = "Loan_15";

			/// <summary>
			/// Loan_16 欄位名稱常數定義
			/// </summary>
			public const string Loan16 = "Loan_16";

			/// <summary>
			/// Loan_17 欄位名稱常數定義
			/// </summary>
			public const string Loan17 = "Loan_17";

			/// <summary>
			/// Loan_18 欄位名稱常數定義
			/// </summary>
			public const string Loan18 = "Loan_18";

			/// <summary>
			/// Loan_19 欄位名稱常數定義
			/// </summary>
			public const string Loan19 = "Loan_19";

			/// <summary>
			/// Loan_20 欄位名稱常數定義
			/// </summary>
			public const string Loan20 = "Loan_20";

			/// <summary>
			/// Loan_21 欄位名稱常數定義
			/// </summary>
			public const string Loan21 = "Loan_21";

			/// <summary>
			/// Loan_22 欄位名稱常數定義
			/// </summary>
			public const string Loan22 = "Loan_22";

			/// <summary>
			/// Loan_23 欄位名稱常數定義
			/// </summary>
			public const string Loan23 = "Loan_23";

			/// <summary>
			/// Loan_24 欄位名稱常數定義
			/// </summary>
			public const string Loan24 = "Loan_24";

			/// <summary>
			/// Loan_25 欄位名稱常數定義
			/// </summary>
			public const string Loan25 = "Loan_25";

			/// <summary>
			/// Loan_26 欄位名稱常數定義
			/// </summary>
			public const string Loan26 = "Loan_26";

			/// <summary>
			/// Loan_27 欄位名稱常數定義
			/// </summary>
			public const string Loan27 = "Loan_27";

			/// <summary>
			/// Loan_28 欄位名稱常數定義
			/// </summary>
			public const string Loan28 = "Loan_28";

			/// <summary>
			/// Loan_29 欄位名稱常數定義
			/// </summary>
			public const string Loan29 = "Loan_29";

			/// <summary>
			/// Loan_30 欄位名稱常數定義
			/// </summary>
			public const string Loan30 = "Loan_30";

			/// <summary>
			/// Loan_31 欄位名稱常數定義
			/// </summary>
			public const string Loan31 = "Loan_31";

			/// <summary>
			/// Loan_32 欄位名稱常數定義
			/// </summary>
			public const string Loan32 = "Loan_32";

			/// <summary>
			/// Loan_33 欄位名稱常數定義
			/// </summary>
			public const string Loan33 = "Loan_33";

			/// <summary>
			/// Loan_34 欄位名稱常數定義
			/// </summary>
			public const string Loan34 = "Loan_34";

			/// <summary>
			/// Loan_35 欄位名稱常數定義
			/// </summary>
			public const string Loan35 = "Loan_35";

			/// <summary>
			/// Loan_36 欄位名稱常數定義
			/// </summary>
			public const string Loan36 = "Loan_36";

			/// <summary>
			/// Loan_37 欄位名稱常數定義
			/// </summary>
			public const string Loan37 = "Loan_37";

			/// <summary>
			/// Loan_38 欄位名稱常數定義
			/// </summary>
			public const string Loan38 = "Loan_38";

			/// <summary>
			/// Loan_39 欄位名稱常數定義
			/// </summary>
			public const string Loan39 = "Loan_39";

			/// <summary>
			/// Loan_40 欄位名稱常數定義
			/// </summary>
			public const string Loan40 = "Loan_40";

			/// <summary>
			/// Loan_Amount 欄位名稱常數定義
			/// </summary>
			public const string LoanAmount = "Loan_Amount";

			/// <summary>
			/// Loan_Item 欄位名稱常數定義
			/// </summary>
			public const string LoanItem = "Loan_Item";

			/// <summary>
			/// Pay_Flag 欄位名稱常數定義
			/// </summary>
			public const string PayFlag = "Pay_Flag";

			/// <summary>
			/// TakeOff_Reduce 欄位名稱常數定義
			/// </summary>
			public const string TakeoffReduce = "TakeOff_Reduce";

			/// <summary>
			/// status 欄位名稱常數定義
			/// </summary>
			public const string Status = "status";

			/// <summary>
			/// crt_date 欄位名稱常數定義
			/// </summary>
			public const string CrtDate = "crt_date";

			/// <summary>
			/// crt_user 欄位名稱常數定義
			/// </summary>
			public const string CrtUser = "crt_user";

			/// <summary>
			/// mdy_date 欄位名稱常數定義
			/// </summary>
			public const string MdyDate = "mdy_date";

			/// <summary>
			/// mdy_user 欄位名稱常數定義
			/// </summary>
			public const string MdyUser = "mdy_user";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// LoanStandardEntity 類別建構式
		/// </summary>
		public LoanStandardEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _ReceiveType = null;
		/// <summary>
		/// Receive_Type 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReceiveType, true, FieldTypeEnum.VarChar, 6, false)]
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
		/// Year_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.YearId, true, FieldTypeEnum.Char, 3, false)]
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
		/// Term_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.TermId, true, FieldTypeEnum.Char, 1, false)]
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
		/// Dep_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.DepId, true, FieldTypeEnum.Char, 1, false)]
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
		/// Receive_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReceiveId, true, FieldTypeEnum.VarChar, 2, false)]
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

		private string _LoanId = null;
		/// <summary>
		/// Loan_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.LoanId, true, FieldTypeEnum.VarChar, 20, false)]
		public string LoanId
		{
			get
			{
				return _LoanId;
			}
			set
			{
				_LoanId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// Loan_01 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan01, false, FieldTypeEnum.Char, 1, true)]
		public string Loan01
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_02 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan02, false, FieldTypeEnum.Char, 1, true)]
		public string Loan02
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_03 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan03, false, FieldTypeEnum.Char, 1, true)]
		public string Loan03
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_04 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan04, false, FieldTypeEnum.Char, 1, true)]
		public string Loan04
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_05 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan05, false, FieldTypeEnum.Char, 1, true)]
		public string Loan05
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_06 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan06, false, FieldTypeEnum.Char, 1, true)]
		public string Loan06
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_07 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan07, false, FieldTypeEnum.Char, 1, true)]
		public string Loan07
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_08 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan08, false, FieldTypeEnum.Char, 1, true)]
		public string Loan08
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_09 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan09, false, FieldTypeEnum.Char, 1, true)]
		public string Loan09
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan10, false, FieldTypeEnum.Char, 1, true)]
		public string Loan10
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan11, false, FieldTypeEnum.Char, 1, true)]
		public string Loan11
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan12, false, FieldTypeEnum.Char, 1, true)]
		public string Loan12
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan13, false, FieldTypeEnum.Char, 1, true)]
		public string Loan13
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan14, false, FieldTypeEnum.Char, 1, true)]
		public string Loan14
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan15, false, FieldTypeEnum.Char, 1, true)]
		public string Loan15
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan16, false, FieldTypeEnum.Char, 1, true)]
		public string Loan16
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_17 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan17, false, FieldTypeEnum.Char, 1, true)]
		public string Loan17
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_18 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan18, false, FieldTypeEnum.Char, 1, true)]
		public string Loan18
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_19 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan19, false, FieldTypeEnum.Char, 1, true)]
		public string Loan19
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_20 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan20, false, FieldTypeEnum.Char, 1, true)]
		public string Loan20
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_21 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan21, false, FieldTypeEnum.Char, 1, true)]
		public string Loan21
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_22 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan22, false, FieldTypeEnum.Char, 1, true)]
		public string Loan22
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_23 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan23, false, FieldTypeEnum.Char, 1, true)]
		public string Loan23
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_24 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan24, false, FieldTypeEnum.Char, 1, true)]
		public string Loan24
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_25 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan25, false, FieldTypeEnum.Char, 1, true)]
		public string Loan25
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_26 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan26, false, FieldTypeEnum.Char, 1, true)]
		public string Loan26
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_27 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan27, false, FieldTypeEnum.Char, 1, true)]
		public string Loan27
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_28 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan28, false, FieldTypeEnum.Char, 1, true)]
		public string Loan28
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_29 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan29, false, FieldTypeEnum.Char, 1, true)]
		public string Loan29
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_30 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan30, false, FieldTypeEnum.Char, 1, true)]
		public string Loan30
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_31 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan31, false, FieldTypeEnum.Char, 1, true)]
		public string Loan31
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_32 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan32, false, FieldTypeEnum.Char, 1, true)]
		public string Loan32
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_33 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan33, false, FieldTypeEnum.Char, 1, true)]
		public string Loan33
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_34 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan34, false, FieldTypeEnum.Char, 1, true)]
		public string Loan34
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_35 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan35, false, FieldTypeEnum.Char, 1, true)]
		public string Loan35
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_36 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan36, false, FieldTypeEnum.Char, 1, true)]
		public string Loan36
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_37 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan37, false, FieldTypeEnum.Char, 1, true)]
		public string Loan37
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_38 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan38, false, FieldTypeEnum.Char, 1, true)]
		public string Loan38
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_39 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan39, false, FieldTypeEnum.Char, 1, true)]
		public string Loan39
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_40 欄位屬性
		/// </summary>
		[FieldSpec(Field.Loan40, false, FieldTypeEnum.Char, 1, true)]
		public string Loan40
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_Amount 欄位屬性
		/// </summary>
		[FieldSpec(Field.LoanAmount, false, FieldTypeEnum.Decimal, true)]
		public decimal? LoanAmount
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_Item 欄位屬性
		/// </summary>
		[FieldSpec(Field.LoanItem, false, FieldTypeEnum.Char, 2, true)]
		public string LoanItem
		{
			get;
			set;
		}

		/// <summary>
		/// Pay_Flag 欄位屬性
		/// </summary>
		[FieldSpec(Field.PayFlag, false, FieldTypeEnum.Char, 1, true)]
		public string PayFlag
		{
			get;
			set;
		}

		/// <summary>
		/// TakeOff_Reduce 欄位屬性
		/// </summary>
		[FieldSpec(Field.TakeoffReduce, false, FieldTypeEnum.Char, 1, false)]
		public string TakeoffReduce
		{
			get;
			set;
		}

		/// <summary>
		/// status 欄位屬性
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
		public string Status
		{
			get;
			set;
		}

		/// <summary>
		/// crt_date 欄位屬性
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get;
			set;
		}

		/// <summary>
		/// crt_user 欄位屬性
		/// </summary>
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
		public string CrtUser
		{
			get;
			set;
		}

		/// <summary>
		/// mdy_date 欄位屬性
		/// </summary>
		[FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? MdyDate
		{
			get;
			set;
		}

		/// <summary>
		/// mdy_user 欄位屬性
		/// </summary>
		[FieldSpec(Field.MdyUser, false, FieldTypeEnum.VarChar, 20, true)]
		public string MdyUser
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
