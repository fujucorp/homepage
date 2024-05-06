/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:36:16
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
	/// MappingRt_Txt 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class MappingrtTxtEntity : Entity
	{
		public const string TABLE_NAME = "MappingRt_Txt";

		#region Field Name Const Class
		/// <summary>
		/// MappingrtTxtEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// Mapping_Id 欄位名稱常數定義
			/// </summary>
			public const string MappingId = "Mapping_Id";

			/// <summary>
			/// Receive_Type 欄位名稱常數定義
			/// </summary>
			public const string ReceiveType = "Receive_Type";
			#endregion

			#region Data
			/// <summary>
			/// Year_Id 欄位名稱常數定義
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
			/// Term_Id 欄位名稱常數定義
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
			/// Mapping_Name 欄位名稱常數定義
			/// </summary>
			public const string MappingName = "Mapping_Name";

			/// <summary>
			/// Dep_Id 欄位名稱常數定義
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
			/// Receive_Id 欄位名稱常數定義
			/// </summary>
			public const string ReceiveId = "Receive_Id";

			/// <summary>
			/// Stu_Id_S 欄位名稱常數定義
			/// </summary>
			public const string StuIdS = "Stu_Id_S";

			/// <summary>
			/// Stu_Id_L 欄位名稱常數定義
			/// </summary>
			public const string StuIdL = "Stu_Id_L";

			/// <summary>
			/// RT_Name_S 欄位名稱常數定義
			/// </summary>
			public const string RtNameS = "RT_Name_S";

			/// <summary>
			/// RT_Name_L 欄位名稱常數定義
			/// </summary>
			public const string RtNameL = "RT_Name_L";

			/// <summary>
			/// Rt_Credit_S 欄位名稱常數定義
			/// </summary>
			public const string RtCreditS = "Rt_Credit_S";

			/// <summary>
			/// Rt_Credit_L 欄位名稱常數定義
			/// </summary>
			public const string RtCreditL = "Rt_Credit_L";

			/// <summary>
			/// Re_Credit_S 欄位名稱常數定義
			/// </summary>
			public const string ReCreditS = "Re_Credit_S";

			/// <summary>
			/// Re_Credit_L 欄位名稱常數定義
			/// </summary>
			public const string ReCreditL = "Re_Credit_L";

			/// <summary>
			/// Rt_Amount_S 欄位名稱常數定義
			/// </summary>
			public const string RtAmountS = "Rt_Amount_S";

			/// <summary>
			/// Rt_Amount_L 欄位名稱常數定義
			/// </summary>
			public const string RtAmountL = "Rt_Amount_L";

			/// <summary>
			/// Rt_Bank_ID_S 欄位名稱常數定義
			/// </summary>
			public const string RtBankIdS = "Rt_Bank_ID_S";

			/// <summary>
			/// Rt_Bank_ID_L 欄位名稱常數定義
			/// </summary>
			public const string RtBankIdL = "Rt_Bank_ID_L";

			/// <summary>
			/// Rt_Account_S 欄位名稱常數定義
			/// </summary>
			public const string RtAccountS = "Rt_Account_S";

			/// <summary>
			/// Rt_Account_L 欄位名稱常數定義
			/// </summary>
			public const string RtAccountL = "Rt_Account_L";

			/// <summary>
			/// Rt_01_S 欄位名稱常數定義
			/// </summary>
			public const string Rt01S = "Rt_01_S";

			/// <summary>
			/// Rt_01_L 欄位名稱常數定義
			/// </summary>
			public const string Rt01L = "Rt_01_L";

			/// <summary>
			/// Rt_02_S 欄位名稱常數定義
			/// </summary>
			public const string Rt02S = "Rt_02_S";

			/// <summary>
			/// Rt_02_L 欄位名稱常數定義
			/// </summary>
			public const string Rt02L = "Rt_02_L";

			/// <summary>
			/// Rt_03_S 欄位名稱常數定義
			/// </summary>
			public const string Rt03S = "Rt_03_S";

			/// <summary>
			/// Rt_03_L 欄位名稱常數定義
			/// </summary>
			public const string Rt03L = "Rt_03_L";

			/// <summary>
			/// Rt_04_S 欄位名稱常數定義
			/// </summary>
			public const string Rt04S = "Rt_04_S";

			/// <summary>
			/// Rt_04_L 欄位名稱常數定義
			/// </summary>
			public const string Rt04L = "Rt_04_L";

			/// <summary>
			/// Rt_05_S 欄位名稱常數定義
			/// </summary>
			public const string Rt05S = "Rt_05_S";

			/// <summary>
			/// Rt_05_L 欄位名稱常數定義
			/// </summary>
			public const string Rt05L = "Rt_05_L";

			/// <summary>
			/// Rt_06_S 欄位名稱常數定義
			/// </summary>
			public const string Rt06S = "Rt_06_S";

			/// <summary>
			/// Rt_06_L 欄位名稱常數定義
			/// </summary>
			public const string Rt06L = "Rt_06_L";

			/// <summary>
			/// Rt_07_S 欄位名稱常數定義
			/// </summary>
			public const string Rt07S = "Rt_07_S";

			/// <summary>
			/// Rt_07_L 欄位名稱常數定義
			/// </summary>
			public const string Rt07L = "Rt_07_L";

			/// <summary>
			/// Rt_08_S 欄位名稱常數定義
			/// </summary>
			public const string Rt08S = "Rt_08_S";

			/// <summary>
			/// Rt_08_L 欄位名稱常數定義
			/// </summary>
			public const string Rt08L = "Rt_08_L";

			/// <summary>
			/// Rt_09_S 欄位名稱常數定義
			/// </summary>
			public const string Rt09S = "Rt_09_S";

			/// <summary>
			/// Rt_09_L 欄位名稱常數定義
			/// </summary>
			public const string Rt09L = "Rt_09_L";

			/// <summary>
			/// Rt_10_S 欄位名稱常數定義
			/// </summary>
			public const string Rt10S = "Rt_10_S";

			/// <summary>
			/// Rt_10_L 欄位名稱常數定義
			/// </summary>
			public const string Rt10L = "Rt_10_L";

			/// <summary>
			/// Rt_11_S 欄位名稱常數定義
			/// </summary>
			public const string Rt11S = "Rt_11_S";

			/// <summary>
			/// Rt_11_L 欄位名稱常數定義
			/// </summary>
			public const string Rt11L = "Rt_11_L";

			/// <summary>
			/// Rt_12_S 欄位名稱常數定義
			/// </summary>
			public const string Rt12S = "Rt_12_S";

			/// <summary>
			/// Rt_12_L 欄位名稱常數定義
			/// </summary>
			public const string Rt12L = "Rt_12_L";

			/// <summary>
			/// Rt_13_S 欄位名稱常數定義
			/// </summary>
			public const string Rt13S = "Rt_13_S";

			/// <summary>
			/// Rt_13_L 欄位名稱常數定義
			/// </summary>
			public const string Rt13L = "Rt_13_L";

			/// <summary>
			/// Rt_14_S 欄位名稱常數定義
			/// </summary>
			public const string Rt14S = "Rt_14_S";

			/// <summary>
			/// Rt_14_L 欄位名稱常數定義
			/// </summary>
			public const string Rt14L = "Rt_14_L";

			/// <summary>
			/// Rt_15_S 欄位名稱常數定義
			/// </summary>
			public const string Rt15S = "Rt_15_S";

			/// <summary>
			/// Rt_15_L 欄位名稱常數定義
			/// </summary>
			public const string Rt15L = "Rt_15_L";

			/// <summary>
			/// Rt_16_S 欄位名稱常數定義
			/// </summary>
			public const string Rt16S = "Rt_16_S";

			/// <summary>
			/// Rt_16_L 欄位名稱常數定義
			/// </summary>
			public const string Rt16L = "Rt_16_L";

			/// <summary>
			/// Rt_17_S 欄位名稱常數定義
			/// </summary>
			public const string Rt17S = "Rt_17_S";

			/// <summary>
			/// Rt_17_L 欄位名稱常數定義
			/// </summary>
			public const string Rt17L = "Rt_17_L";

			/// <summary>
			/// Rt_18_S 欄位名稱常數定義
			/// </summary>
			public const string Rt18S = "Rt_18_S";

			/// <summary>
			/// Rt_18_L 欄位名稱常數定義
			/// </summary>
			public const string Rt18L = "Rt_18_L";

			/// <summary>
			/// Rt_19_S 欄位名稱常數定義
			/// </summary>
			public const string Rt19S = "Rt_19_S";

			/// <summary>
			/// Rt_19_L 欄位名稱常數定義
			/// </summary>
			public const string Rt19L = "Rt_19_L";

			/// <summary>
			/// Rt_20_S 欄位名稱常數定義
			/// </summary>
			public const string Rt20S = "Rt_20_S";

			/// <summary>
			/// Rt_20_L 欄位名稱常數定義
			/// </summary>
			public const string Rt20L = "Rt_20_L";

			/// <summary>
			/// Rt_21_S 欄位名稱常數定義
			/// </summary>
			public const string Rt21S = "Rt_21_S";

			/// <summary>
			/// Rt_21_L 欄位名稱常數定義
			/// </summary>
			public const string Rt21L = "Rt_21_L";

			/// <summary>
			/// Rt_22_S 欄位名稱常數定義
			/// </summary>
			public const string Rt22S = "Rt_22_S";

			/// <summary>
			/// Rt_22_L 欄位名稱常數定義
			/// </summary>
			public const string Rt22L = "Rt_22_L";

			/// <summary>
			/// Rt_23_S 欄位名稱常數定義
			/// </summary>
			public const string Rt23S = "Rt_23_S";

			/// <summary>
			/// Rt_23_L 欄位名稱常數定義
			/// </summary>
			public const string Rt23L = "Rt_23_L";

			/// <summary>
			/// Rt_24_S 欄位名稱常數定義
			/// </summary>
			public const string Rt24S = "Rt_24_S";

			/// <summary>
			/// Rt_24_L 欄位名稱常數定義
			/// </summary>
			public const string Rt24L = "Rt_24_L";

			/// <summary>
			/// Rt_25_S 欄位名稱常數定義
			/// </summary>
			public const string Rt25S = "Rt_25_S";

			/// <summary>
			/// Rt_25_L 欄位名稱常數定義
			/// </summary>
			public const string Rt25L = "Rt_25_L";

			/// <summary>
			/// Rt_26_S 欄位名稱常數定義
			/// </summary>
			public const string Rt26S = "Rt_26_S";

			/// <summary>
			/// Rt_26_L 欄位名稱常數定義
			/// </summary>
			public const string Rt26L = "Rt_26_L";

			/// <summary>
			/// Rt_27_S 欄位名稱常數定義
			/// </summary>
			public const string Rt27S = "Rt_27_S";

			/// <summary>
			/// Rt_27_L 欄位名稱常數定義
			/// </summary>
			public const string Rt27L = "Rt_27_L";

			/// <summary>
			/// Rt_28_S 欄位名稱常數定義
			/// </summary>
			public const string Rt28S = "Rt_28_S";

			/// <summary>
			/// Rt_28_L 欄位名稱常數定義
			/// </summary>
			public const string Rt28L = "Rt_28_L";

			/// <summary>
			/// Rt_29_S 欄位名稱常數定義
			/// </summary>
			public const string Rt29S = "Rt_29_S";

			/// <summary>
			/// Rt_29_L 欄位名稱常數定義
			/// </summary>
			public const string Rt29L = "Rt_29_L";

			/// <summary>
			/// Rt_30_S 欄位名稱常數定義
			/// </summary>
			public const string Rt30S = "Rt_30_S";

			/// <summary>
			/// Rt_30_L 欄位名稱常數定義
			/// </summary>
			public const string Rt30L = "Rt_30_L";

			/// <summary>
			/// Rt_31_S 欄位名稱常數定義
			/// </summary>
			public const string Rt31S = "Rt_31_S";

			/// <summary>
			/// Rt_31_L 欄位名稱常數定義
			/// </summary>
			public const string Rt31L = "Rt_31_L";

			/// <summary>
			/// Rt_32_S 欄位名稱常數定義
			/// </summary>
			public const string Rt32S = "Rt_32_S";

			/// <summary>
			/// Rt_32_L 欄位名稱常數定義
			/// </summary>
			public const string Rt32L = "Rt_32_L";

			/// <summary>
			/// Rt_33_S 欄位名稱常數定義
			/// </summary>
			public const string Rt33S = "Rt_33_S";

			/// <summary>
			/// Rt_33_L 欄位名稱常數定義
			/// </summary>
			public const string Rt33L = "Rt_33_L";

			/// <summary>
			/// Rt_34_S 欄位名稱常數定義
			/// </summary>
			public const string Rt34S = "Rt_34_S";

			/// <summary>
			/// Rt_34_L 欄位名稱常數定義
			/// </summary>
			public const string Rt34L = "Rt_34_L";

			/// <summary>
			/// Rt_35_S 欄位名稱常數定義
			/// </summary>
			public const string Rt35S = "Rt_35_S";

			/// <summary>
			/// Rt_35_L 欄位名稱常數定義
			/// </summary>
			public const string Rt35L = "Rt_35_L";

			/// <summary>
			/// Rt_36_S 欄位名稱常數定義
			/// </summary>
			public const string Rt36S = "Rt_36_S";

			/// <summary>
			/// Rt_36_L 欄位名稱常數定義
			/// </summary>
			public const string Rt36L = "Rt_36_L";

			/// <summary>
			/// Rt_37_S 欄位名稱常數定義
			/// </summary>
			public const string Rt37S = "Rt_37_S";

			/// <summary>
			/// Rt_37_L 欄位名稱常數定義
			/// </summary>
			public const string Rt37L = "Rt_37_L";

			/// <summary>
			/// Rt_38_S 欄位名稱常數定義
			/// </summary>
			public const string Rt38S = "Rt_38_S";

			/// <summary>
			/// Rt_38_L 欄位名稱常數定義
			/// </summary>
			public const string Rt38L = "Rt_38_L";

			/// <summary>
			/// Rt_39_S 欄位名稱常數定義
			/// </summary>
			public const string Rt39S = "Rt_39_S";

			/// <summary>
			/// Rt_39_L 欄位名稱常數定義
			/// </summary>
			public const string Rt39L = "Rt_39_L";

			/// <summary>
			/// Rt_40_S 欄位名稱常數定義
			/// </summary>
			public const string Rt40S = "Rt_40_S";

			/// <summary>
			/// Rt_40_L 欄位名稱常數定義
			/// </summary>
			public const string Rt40L = "Rt_40_L";

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
		/// MappingrtTxtEntity 類別建構式
		/// </summary>
		public MappingrtTxtEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _MappingId = null;
		/// <summary>
		/// Mapping_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.MappingId, true, FieldTypeEnum.Char, 2, false)]
		public string MappingId
		{
			get
			{
				return _MappingId;
			}
			set
			{
				_MappingId = value == null ? null : value.Trim();
			}
		}

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
		#endregion

		#region Data
		/// <summary>
		/// Year_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.YearId, false, FieldTypeEnum.Char, 3, true)]
		public string YearId
		{
			get;
			set;
		}

		/// <summary>
		/// Term_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.TermId, false, FieldTypeEnum.Char, 1, true)]
		public string TermId
		{
			get;
			set;
		}

		/// <summary>
		/// Mapping_Name 欄位屬性
		/// </summary>
		[FieldSpec(Field.MappingName, false, FieldTypeEnum.VarChar, 50, true)]
		public string MappingName
		{
			get;
			set;
		}

		/// <summary>
		/// Dep_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.DepId, false, FieldTypeEnum.Char, 1, true)]
		public string DepId
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReceiveId, false, FieldTypeEnum.Char, 1, true)]
		public string ReceiveId
		{
			get;
			set;
		}

		/// <summary>
		/// Stu_Id_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.StuIdS, false, FieldTypeEnum.Integer, true)]
		public int? StuIdS
		{
			get;
			set;
		}

		/// <summary>
		/// Stu_Id_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.StuIdL, false, FieldTypeEnum.Integer, true)]
		public int? StuIdL
		{
			get;
			set;
		}

		/// <summary>
		/// RT_Name_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtNameS, false, FieldTypeEnum.Integer, true)]
		public int? RtNameS
		{
			get;
			set;
		}

		/// <summary>
		/// RT_Name_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtNameL, false, FieldTypeEnum.Integer, true)]
		public int? RtNameL
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Credit_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtCreditS, false, FieldTypeEnum.Integer, true)]
		public int? RtCreditS
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Credit_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtCreditL, false, FieldTypeEnum.Integer, true)]
		public int? RtCreditL
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Credit_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReCreditS, false, FieldTypeEnum.Integer, true)]
		public int? ReCreditS
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Credit_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReCreditL, false, FieldTypeEnum.Integer, true)]
		public int? ReCreditL
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Amount_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtAmountS, false, FieldTypeEnum.Integer, true)]
		public int? RtAmountS
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Amount_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtAmountL, false, FieldTypeEnum.Integer, true)]
		public int? RtAmountL
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Bank_ID_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtBankIdS, false, FieldTypeEnum.Integer, true)]
		public int? RtBankIdS
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Bank_ID_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtBankIdL, false, FieldTypeEnum.Integer, true)]
		public int? RtBankIdL
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Account_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtAccountS, false, FieldTypeEnum.Integer, true)]
		public int? RtAccountS
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Account_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtAccountL, false, FieldTypeEnum.Integer, true)]
		public int? RtAccountL
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_01_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt01S, false, FieldTypeEnum.Integer, true)]
		public int? Rt01S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_01_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt01L, false, FieldTypeEnum.Integer, true)]
		public int? Rt01L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_02_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt02S, false, FieldTypeEnum.Integer, true)]
		public int? Rt02S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_02_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt02L, false, FieldTypeEnum.Integer, true)]
		public int? Rt02L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_03_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt03S, false, FieldTypeEnum.Integer, true)]
		public int? Rt03S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_03_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt03L, false, FieldTypeEnum.Integer, true)]
		public int? Rt03L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_04_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt04S, false, FieldTypeEnum.Integer, true)]
		public int? Rt04S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_04_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt04L, false, FieldTypeEnum.Integer, true)]
		public int? Rt04L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_05_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt05S, false, FieldTypeEnum.Integer, true)]
		public int? Rt05S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_05_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt05L, false, FieldTypeEnum.Integer, true)]
		public int? Rt05L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_06_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt06S, false, FieldTypeEnum.Integer, true)]
		public int? Rt06S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_06_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt06L, false, FieldTypeEnum.Integer, true)]
		public int? Rt06L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_07_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt07S, false, FieldTypeEnum.Integer, true)]
		public int? Rt07S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_07_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt07L, false, FieldTypeEnum.Integer, true)]
		public int? Rt07L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_08_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt08S, false, FieldTypeEnum.Integer, true)]
		public int? Rt08S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_08_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt08L, false, FieldTypeEnum.Integer, true)]
		public int? Rt08L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_09_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt09S, false, FieldTypeEnum.Integer, true)]
		public int? Rt09S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_09_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt09L, false, FieldTypeEnum.Integer, true)]
		public int? Rt09L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_10_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt10S, false, FieldTypeEnum.Integer, true)]
		public int? Rt10S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_10_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt10L, false, FieldTypeEnum.Integer, true)]
		public int? Rt10L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_11_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt11S, false, FieldTypeEnum.Integer, true)]
		public int? Rt11S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_11_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt11L, false, FieldTypeEnum.Integer, true)]
		public int? Rt11L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_12_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt12S, false, FieldTypeEnum.Integer, true)]
		public int? Rt12S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_12_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt12L, false, FieldTypeEnum.Integer, true)]
		public int? Rt12L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_13_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt13S, false, FieldTypeEnum.Integer, true)]
		public int? Rt13S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_13_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt13L, false, FieldTypeEnum.Integer, true)]
		public int? Rt13L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_14_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt14S, false, FieldTypeEnum.Integer, true)]
		public int? Rt14S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_14_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt14L, false, FieldTypeEnum.Integer, true)]
		public int? Rt14L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_15_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt15S, false, FieldTypeEnum.Integer, true)]
		public int? Rt15S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_15_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt15L, false, FieldTypeEnum.Integer, true)]
		public int? Rt15L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_16_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt16S, false, FieldTypeEnum.Integer, true)]
		public int? Rt16S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_16_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt16L, false, FieldTypeEnum.Integer, true)]
		public int? Rt16L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_17_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt17S, false, FieldTypeEnum.Integer, true)]
		public int? Rt17S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_17_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt17L, false, FieldTypeEnum.Integer, true)]
		public int? Rt17L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_18_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt18S, false, FieldTypeEnum.Integer, true)]
		public int? Rt18S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_18_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt18L, false, FieldTypeEnum.Integer, true)]
		public int? Rt18L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_19_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt19S, false, FieldTypeEnum.Integer, true)]
		public int? Rt19S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_19_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt19L, false, FieldTypeEnum.Integer, true)]
		public int? Rt19L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_20_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt20S, false, FieldTypeEnum.Integer, true)]
		public int? Rt20S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_20_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt20L, false, FieldTypeEnum.Integer, true)]
		public int? Rt20L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_21_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt21S, false, FieldTypeEnum.Integer, true)]
		public int? Rt21S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_21_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt21L, false, FieldTypeEnum.Integer, true)]
		public int? Rt21L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_22_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt22S, false, FieldTypeEnum.Integer, true)]
		public int? Rt22S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_22_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt22L, false, FieldTypeEnum.Integer, true)]
		public int? Rt22L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_23_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt23S, false, FieldTypeEnum.Integer, true)]
		public int? Rt23S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_23_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt23L, false, FieldTypeEnum.Integer, true)]
		public int? Rt23L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_24_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt24S, false, FieldTypeEnum.Integer, true)]
		public int? Rt24S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_24_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt24L, false, FieldTypeEnum.Integer, true)]
		public int? Rt24L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_25_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt25S, false, FieldTypeEnum.Integer, true)]
		public int? Rt25S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_25_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt25L, false, FieldTypeEnum.Integer, true)]
		public int? Rt25L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_26_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt26S, false, FieldTypeEnum.Integer, true)]
		public int? Rt26S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_26_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt26L, false, FieldTypeEnum.Integer, true)]
		public int? Rt26L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_27_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt27S, false, FieldTypeEnum.Integer, true)]
		public int? Rt27S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_27_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt27L, false, FieldTypeEnum.Integer, true)]
		public int? Rt27L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_28_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt28S, false, FieldTypeEnum.Integer, true)]
		public int? Rt28S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_28_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt28L, false, FieldTypeEnum.Integer, true)]
		public int? Rt28L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_29_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt29S, false, FieldTypeEnum.Integer, true)]
		public int? Rt29S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_29_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt29L, false, FieldTypeEnum.Integer, true)]
		public int? Rt29L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_30_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt30S, false, FieldTypeEnum.Integer, true)]
		public int? Rt30S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_30_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt30L, false, FieldTypeEnum.Integer, true)]
		public int? Rt30L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_31_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt31S, false, FieldTypeEnum.Integer, true)]
		public int? Rt31S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_31_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt31L, false, FieldTypeEnum.Integer, true)]
		public int? Rt31L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_32_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt32S, false, FieldTypeEnum.Integer, true)]
		public int? Rt32S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_32_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt32L, false, FieldTypeEnum.Integer, true)]
		public int? Rt32L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_33_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt33S, false, FieldTypeEnum.Integer, true)]
		public int? Rt33S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_33_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt33L, false, FieldTypeEnum.Integer, true)]
		public int? Rt33L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_34_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt34S, false, FieldTypeEnum.Integer, true)]
		public int? Rt34S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_34_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt34L, false, FieldTypeEnum.Integer, true)]
		public int? Rt34L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_35_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt35S, false, FieldTypeEnum.Integer, true)]
		public int? Rt35S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_35_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt35L, false, FieldTypeEnum.Integer, true)]
		public int? Rt35L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_36_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt36S, false, FieldTypeEnum.Integer, true)]
		public int? Rt36S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_36_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt36L, false, FieldTypeEnum.Integer, true)]
		public int? Rt36L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_37_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt37S, false, FieldTypeEnum.Integer, true)]
		public int? Rt37S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_37_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt37L, false, FieldTypeEnum.Integer, true)]
		public int? Rt37L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_38_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt38S, false, FieldTypeEnum.Integer, true)]
		public int? Rt38S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_38_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt38L, false, FieldTypeEnum.Integer, true)]
		public int? Rt38L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_39_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt39S, false, FieldTypeEnum.Integer, true)]
		public int? Rt39S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_39_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt39L, false, FieldTypeEnum.Integer, true)]
		public int? Rt39L
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_40_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt40S, false, FieldTypeEnum.Integer, true)]
		public int? Rt40S
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_40_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt40L, false, FieldTypeEnum.Integer, true)]
		public int? Rt40L
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

        #region Method
        /// <summary>
        /// 取得有設定的 XlsMapField 設定陣列
        /// </summary>
        /// <returns>傳回 XlsMapField 設定陣列</returns>
        public TxtMapField[] GetMapFields()
        {
            List<TxtMapField> mapFields = new List<TxtMapField>();

            if (this.StuIdS != null && this.StuIdL != null)
            {
                mapFields.Add(new TxtMapField(MappingrtXlsmdbEntity.Field.StuId, this.StuIdS.Value, this.StuIdL.Value, new CodeChecker(1, 20)));
            }

            if (this.RtNameS != null && this.RtNameL != null)
            {
                mapFields.Add(new TxtMapField(MappingrtXlsmdbEntity.Field.RtName, this.RtNameS.Value, this.RtNameL.Value, new CodeChecker(1, 20)));
            }

            if (this.RtCreditS != null && this.RtCreditL != null)
            {
                mapFields.Add(new TxtMapField(MappingrtXlsmdbEntity.Field.RtCredit, this.RtCreditS.Value, this.RtCreditL.Value, new DecimalChecker(0M, 999.99M)));
            }

            if (this.ReCreditS != null && this.ReCreditL != null)
            {
                mapFields.Add(new TxtMapField(MappingrtXlsmdbEntity.Field.ReCredit, this.ReCreditS.Value, this.ReCreditL.Value, new DecimalChecker(0M, 999.99M)));
            }

            if (this.RtAmountS != null && this.RtAmountL != null)
            {
                mapFields.Add(new TxtMapField(MappingrtXlsmdbEntity.Field.RtAmount, this.RtAmountS.Value, this.RtAmountL.Value, new DecimalChecker(0M, 999999999M)));
            }

            if (this.RtBankIdS != null && this.RtBankIdL != null)
            {
                mapFields.Add(new TxtMapField(MappingrtXlsmdbEntity.Field.RtBankId, this.RtBankIdS.Value, this.RtBankIdL.Value, null));
            }

            if (this.RtAccountS != null && this.RtAccountL != null)
            {
                mapFields.Add(new TxtMapField(MappingrtXlsmdbEntity.Field.RtAccount, this.RtAccountS.Value, this.RtAccountL.Value, new NumberChecker(0, 21, null)));
            }

            #region 代收科目退費金額對照欄位
            {
                string[] fields = new string[] {
                    MappingrtXlsmdbEntity.Field.Rt01, MappingrtXlsmdbEntity.Field.Rt02, MappingrtXlsmdbEntity.Field.Rt03, MappingrtXlsmdbEntity.Field.Rt04, MappingrtXlsmdbEntity.Field.Rt05,
                    MappingrtXlsmdbEntity.Field.Rt06, MappingrtXlsmdbEntity.Field.Rt07, MappingrtXlsmdbEntity.Field.Rt08, MappingrtXlsmdbEntity.Field.Rt09, MappingrtXlsmdbEntity.Field.Rt10,
                    MappingrtXlsmdbEntity.Field.Rt11, MappingrtXlsmdbEntity.Field.Rt12, MappingrtXlsmdbEntity.Field.Rt13, MappingrtXlsmdbEntity.Field.Rt14, MappingrtXlsmdbEntity.Field.Rt15,
                    MappingrtXlsmdbEntity.Field.Rt16, MappingrtXlsmdbEntity.Field.Rt17, MappingrtXlsmdbEntity.Field.Rt18, MappingrtXlsmdbEntity.Field.Rt19, MappingrtXlsmdbEntity.Field.Rt20,
                    MappingrtXlsmdbEntity.Field.Rt21, MappingrtXlsmdbEntity.Field.Rt22, MappingrtXlsmdbEntity.Field.Rt23, MappingrtXlsmdbEntity.Field.Rt24, MappingrtXlsmdbEntity.Field.Rt25,
                    MappingrtXlsmdbEntity.Field.Rt26, MappingrtXlsmdbEntity.Field.Rt27, MappingrtXlsmdbEntity.Field.Rt28, MappingrtXlsmdbEntity.Field.Rt29, MappingrtXlsmdbEntity.Field.Rt30,
                    MappingrtXlsmdbEntity.Field.Rt31, MappingrtXlsmdbEntity.Field.Rt32, MappingrtXlsmdbEntity.Field.Rt33, MappingrtXlsmdbEntity.Field.Rt34, MappingrtXlsmdbEntity.Field.Rt35,
                    MappingrtXlsmdbEntity.Field.Rt36, MappingrtXlsmdbEntity.Field.Rt37, MappingrtXlsmdbEntity.Field.Rt38, MappingrtXlsmdbEntity.Field.Rt39, MappingrtXlsmdbEntity.Field.Rt40
                };
                int?[] starts = new int?[] {
                    this.Rt01S, this.Rt02S, this.Rt03S, this.Rt04S, this.Rt05S,
                    this.Rt06S, this.Rt07S, this.Rt08S, this.Rt09S, this.Rt10S,
                    this.Rt11S, this.Rt12S, this.Rt13S, this.Rt14S, this.Rt15S,
                    this.Rt16S, this.Rt17S, this.Rt18S, this.Rt19S, this.Rt20S,
                    this.Rt21S, this.Rt22S, this.Rt23S, this.Rt24S, this.Rt25S,
                    this.Rt26S, this.Rt27S, this.Rt28S, this.Rt29S, this.Rt30S,
                    this.Rt31S, this.Rt32S, this.Rt33S, this.Rt34S, this.Rt35S,
                    this.Rt36S, this.Rt37S, this.Rt38S, this.Rt39S, this.Rt40S
                };
                int?[] lengths = new int?[] {
                    this.Rt01L, this.Rt02L, this.Rt03L, this.Rt04L, this.Rt05L,
                    this.Rt06L, this.Rt07L, this.Rt08L, this.Rt09L, this.Rt10L,
                    this.Rt11L, this.Rt12L, this.Rt13L, this.Rt14L, this.Rt15L,
                    this.Rt16L, this.Rt17L, this.Rt18L, this.Rt19L, this.Rt20L,
                    this.Rt21L, this.Rt22L, this.Rt23L, this.Rt24L, this.Rt25L,
                    this.Rt26L, this.Rt27L, this.Rt28L, this.Rt29L, this.Rt30L,
                    this.Rt31L, this.Rt32L, this.Rt33L, this.Rt34L, this.Rt35L,
                    this.Rt36L, this.Rt37L, this.Rt38L, this.Rt39L, this.Rt40L
                };
                for (int idx = 0; idx < starts.Length; idx++)
                {
                    if (starts[idx] != null && lengths[idx] != null)
                    {
                        mapFields.Add(new TxtMapField(fields[idx], starts[idx].Value, lengths[idx].Value, new DecimalChecker(0M, 999999999M)));
                    }
                }
            }
            #endregion

            return mapFields.ToArray();
        }
        #endregion
	}
}
