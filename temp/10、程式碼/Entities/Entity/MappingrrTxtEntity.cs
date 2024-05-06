/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:36:04
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
	/// MappingRR_Txt 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class MappingrrTxtEntity : Entity
	{
		public const string TABLE_NAME = "MappingRR_Txt";

		#region Field Name Const Class
		/// <summary>
		/// MappingrrTxtEntity 欄位名稱定義抽象類別
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
			/// S_ID_S 欄位名稱常數定義
			/// </summary>
			public const string SIdS = "S_ID_S";

			/// <summary>
			/// S_ID_L 欄位名稱常數定義
			/// </summary>
			public const string SIdL = "S_ID_L";

			/// <summary>
			/// RR_Name_S 欄位名稱常數定義
			/// </summary>
			public const string RrNameS = "RR_Name_S";

			/// <summary>
			/// RR_Name_L 欄位名稱常數定義
			/// </summary>
			public const string RrNameL = "RR_Name_L";

			/// <summary>
			/// RR_Count_S 欄位名稱常數定義
			/// </summary>
			public const string RrCountS = "RR_Count_S";

			/// <summary>
			/// RR_Count_L 欄位名稱常數定義
			/// </summary>
			public const string RrCountL = "RR_Count_L";

			/// <summary>
			/// RR_1_S 欄位名稱常數定義
			/// </summary>
			public const string Rr1S = "RR_1_S";

			/// <summary>
			/// RR_1_L 欄位名稱常數定義
			/// </summary>
			public const string Rr1L = "RR_1_L";

			/// <summary>
			/// RR_2_S 欄位名稱常數定義
			/// </summary>
			public const string Rr2S = "RR_2_S";

			/// <summary>
			/// RR_2_L 欄位名稱常數定義
			/// </summary>
			public const string Rr2L = "RR_2_L";

			/// <summary>
			/// RR_3_S 欄位名稱常數定義
			/// </summary>
			public const string Rr3S = "RR_3_S";

			/// <summary>
			/// RR_3_L 欄位名稱常數定義
			/// </summary>
			public const string Rr3L = "RR_3_L";

			/// <summary>
			/// RR_4_S 欄位名稱常數定義
			/// </summary>
			public const string Rr4S = "RR_4_S";

			/// <summary>
			/// RR_4_L 欄位名稱常數定義
			/// </summary>
			public const string Rr4L = "RR_4_L";

			/// <summary>
			/// RR_5_S 欄位名稱常數定義
			/// </summary>
			public const string Rr5S = "RR_5_S";

			/// <summary>
			/// RR_5_L 欄位名稱常數定義
			/// </summary>
			public const string Rr5L = "RR_5_L";

			/// <summary>
			/// RR_6_S 欄位名稱常數定義
			/// </summary>
			public const string Rr6S = "RR_6_S";

			/// <summary>
			/// RR_6_L 欄位名稱常數定義
			/// </summary>
			public const string Rr6L = "RR_6_L";

			/// <summary>
			/// RR_7_S 欄位名稱常數定義
			/// </summary>
			public const string Rr7S = "RR_7_S";

			/// <summary>
			/// RR_7_L 欄位名稱常數定義
			/// </summary>
			public const string Rr7L = "RR_7_L";

			/// <summary>
			/// RR_8_S 欄位名稱常數定義
			/// </summary>
			public const string Rr8S = "RR_8_S";

			/// <summary>
			/// RR_8_L 欄位名稱常數定義
			/// </summary>
			public const string Rr8L = "RR_8_L";

			/// <summary>
			/// RR_9_S 欄位名稱常數定義
			/// </summary>
			public const string Rr9S = "RR_9_S";

			/// <summary>
			/// RR_9_L 欄位名稱常數定義
			/// </summary>
			public const string Rr9L = "RR_9_L";

			/// <summary>
			/// RR_10_S 欄位名稱常數定義
			/// </summary>
			public const string Rr10S = "RR_10_S";

			/// <summary>
			/// RR_10_L 欄位名稱常數定義
			/// </summary>
			public const string Rr10L = "RR_10_L";

			/// <summary>
			/// RR_11_S 欄位名稱常數定義
			/// </summary>
			public const string Rr11S = "RR_11_S";

			/// <summary>
			/// RR_11_L 欄位名稱常數定義
			/// </summary>
			public const string Rr11L = "RR_11_L";

			/// <summary>
			/// RR_12_S 欄位名稱常數定義
			/// </summary>
			public const string Rr12S = "RR_12_S";

			/// <summary>
			/// RR_12_L 欄位名稱常數定義
			/// </summary>
			public const string Rr12L = "RR_12_L";

			/// <summary>
			/// RR_13_S 欄位名稱常數定義
			/// </summary>
			public const string Rr13S = "RR_13_S";

			/// <summary>
			/// RR_13_L 欄位名稱常數定義
			/// </summary>
			public const string Rr13L = "RR_13_L";

			/// <summary>
			/// RR_14_S 欄位名稱常數定義
			/// </summary>
			public const string Rr14S = "RR_14_S";

			/// <summary>
			/// RR_14_L 欄位名稱常數定義
			/// </summary>
			public const string Rr14L = "RR_14_L";

			/// <summary>
			/// RR_15_S 欄位名稱常數定義
			/// </summary>
			public const string Rr15S = "RR_15_S";

			/// <summary>
			/// RR_15_L 欄位名稱常數定義
			/// </summary>
			public const string Rr15L = "RR_15_L";

			/// <summary>
			/// RR_16_S 欄位名稱常數定義
			/// </summary>
			public const string Rr16S = "RR_16_S";

			/// <summary>
			/// RR_16_L 欄位名稱常數定義
			/// </summary>
			public const string Rr16L = "RR_16_L";

			/// <summary>
			/// RR_17_S 欄位名稱常數定義
			/// </summary>
			public const string Rr17S = "RR_17_S";

			/// <summary>
			/// RR_17_L 欄位名稱常數定義
			/// </summary>
			public const string Rr17L = "RR_17_L";

			/// <summary>
			/// RR_18_S 欄位名稱常數定義
			/// </summary>
			public const string Rr18S = "RR_18_S";

			/// <summary>
			/// RR_18_L 欄位名稱常數定義
			/// </summary>
			public const string Rr18L = "RR_18_L";

			/// <summary>
			/// RR_19_S 欄位名稱常數定義
			/// </summary>
			public const string Rr19S = "RR_19_S";

			/// <summary>
			/// RR_19_L 欄位名稱常數定義
			/// </summary>
			public const string Rr19L = "RR_19_L";

			/// <summary>
			/// RR_20_S 欄位名稱常數定義
			/// </summary>
			public const string Rr20S = "RR_20_S";

			/// <summary>
			/// RR_20_L 欄位名稱常數定義
			/// </summary>
			public const string Rr20L = "RR_20_L";

			/// <summary>
			/// RR_21_S 欄位名稱常數定義
			/// </summary>
			public const string Rr21S = "RR_21_S";

			/// <summary>
			/// RR_21_L 欄位名稱常數定義
			/// </summary>
			public const string Rr21L = "RR_21_L";

			/// <summary>
			/// RR_22_S 欄位名稱常數定義
			/// </summary>
			public const string Rr22S = "RR_22_S";

			/// <summary>
			/// RR_22_L 欄位名稱常數定義
			/// </summary>
			public const string Rr22L = "RR_22_L";

			/// <summary>
			/// RR_23_S 欄位名稱常數定義
			/// </summary>
			public const string Rr23S = "RR_23_S";

			/// <summary>
			/// RR_23_L 欄位名稱常數定義
			/// </summary>
			public const string Rr23L = "RR_23_L";

			/// <summary>
			/// RR_24_S 欄位名稱常數定義
			/// </summary>
			public const string Rr24S = "RR_24_S";

			/// <summary>
			/// RR_24_L 欄位名稱常數定義
			/// </summary>
			public const string Rr24L = "RR_24_L";

			/// <summary>
			/// RR_25_S 欄位名稱常數定義
			/// </summary>
			public const string Rr25S = "RR_25_S";

			/// <summary>
			/// RR_25_L 欄位名稱常數定義
			/// </summary>
			public const string Rr25L = "RR_25_L";

			/// <summary>
			/// RR_26_S 欄位名稱常數定義
			/// </summary>
			public const string Rr26S = "RR_26_S";

			/// <summary>
			/// RR_26_L 欄位名稱常數定義
			/// </summary>
			public const string Rr26L = "RR_26_L";

			/// <summary>
			/// RR_27_S 欄位名稱常數定義
			/// </summary>
			public const string Rr27S = "RR_27_S";

			/// <summary>
			/// RR_27_L 欄位名稱常數定義
			/// </summary>
			public const string Rr27L = "RR_27_L";

			/// <summary>
			/// RR_28_S 欄位名稱常數定義
			/// </summary>
			public const string Rr28S = "RR_28_S";

			/// <summary>
			/// RR_28_L 欄位名稱常數定義
			/// </summary>
			public const string Rr28L = "RR_28_L";

			/// <summary>
			/// RR_29_S 欄位名稱常數定義
			/// </summary>
			public const string Rr29S = "RR_29_S";

			/// <summary>
			/// RR_29_L 欄位名稱常數定義
			/// </summary>
			public const string Rr29L = "RR_29_L";

			/// <summary>
			/// RR_30_S 欄位名稱常數定義
			/// </summary>
			public const string Rr30S = "RR_30_S";

			/// <summary>
			/// RR_30_L 欄位名稱常數定義
			/// </summary>
			public const string Rr30L = "RR_30_L";

			/// <summary>
			/// RR_31_S 欄位名稱常數定義
			/// </summary>
			public const string Rr31S = "RR_31_S";

			/// <summary>
			/// RR_31_L 欄位名稱常數定義
			/// </summary>
			public const string Rr31L = "RR_31_L";

			/// <summary>
			/// RR_32_S 欄位名稱常數定義
			/// </summary>
			public const string Rr32S = "RR_32_S";

			/// <summary>
			/// RR_32_L 欄位名稱常數定義
			/// </summary>
			public const string Rr32L = "RR_32_L";

			/// <summary>
			/// RR_33_S 欄位名稱常數定義
			/// </summary>
			public const string Rr33S = "RR_33_S";

			/// <summary>
			/// RR_33_L 欄位名稱常數定義
			/// </summary>
			public const string Rr33L = "RR_33_L";

			/// <summary>
			/// RR_34_S 欄位名稱常數定義
			/// </summary>
			public const string Rr34S = "RR_34_S";

			/// <summary>
			/// RR_34_L 欄位名稱常數定義
			/// </summary>
			public const string Rr34L = "RR_34_L";

			/// <summary>
			/// RR_35_S 欄位名稱常數定義
			/// </summary>
			public const string Rr35S = "RR_35_S";

			/// <summary>
			/// RR_35_L 欄位名稱常數定義
			/// </summary>
			public const string Rr35L = "RR_35_L";

			/// <summary>
			/// RR_36_S 欄位名稱常數定義
			/// </summary>
			public const string Rr36S = "RR_36_S";

			/// <summary>
			/// RR_36_L 欄位名稱常數定義
			/// </summary>
			public const string Rr36L = "RR_36_L";

			/// <summary>
			/// RR_37_S 欄位名稱常數定義
			/// </summary>
			public const string Rr37S = "RR_37_S";

			/// <summary>
			/// RR_37_L 欄位名稱常數定義
			/// </summary>
			public const string Rr37L = "RR_37_L";

			/// <summary>
			/// RR_38_S 欄位名稱常數定義
			/// </summary>
			public const string Rr38S = "RR_38_S";

			/// <summary>
			/// RR_38_L 欄位名稱常數定義
			/// </summary>
			public const string Rr38L = "RR_38_L";

			/// <summary>
			/// RR_39_S 欄位名稱常數定義
			/// </summary>
			public const string Rr39S = "RR_39_S";

			/// <summary>
			/// RR_39_L 欄位名稱常數定義
			/// </summary>
			public const string Rr39L = "RR_39_L";

			/// <summary>
			/// RR_40_S 欄位名稱常數定義
			/// </summary>
			public const string Rr40S = "RR_40_S";

			/// <summary>
			/// RR_40_L 欄位名稱常數定義
			/// </summary>
			public const string Rr40L = "RR_40_L";

			/// <summary>
			/// Amount_S 欄位名稱常數定義
			/// </summary>
			public const string AmountS = "Amount_S";

			/// <summary>
			/// Amount_L 欄位名稱常數定義
			/// </summary>
			public const string AmountL = "Amount_L";

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
		/// MappingrrTxtEntity 類別建構式
		/// </summary>
		public MappingrrTxtEntity()
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
		/// S_ID_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.SIdS, false, FieldTypeEnum.Integer, true)]
		public int? SIdS
		{
			get;
			set;
		}

		/// <summary>
		/// S_ID_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.SIdL, false, FieldTypeEnum.Integer, true)]
		public int? SIdL
		{
			get;
			set;
		}

		/// <summary>
		/// RR_Name_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.RrNameS, false, FieldTypeEnum.Integer, true)]
		public int? RrNameS
		{
			get;
			set;
		}

		/// <summary>
		/// RR_Name_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.RrNameL, false, FieldTypeEnum.Integer, true)]
		public int? RrNameL
		{
			get;
			set;
		}

		/// <summary>
		/// RR_Count_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.RrCountS, false, FieldTypeEnum.Integer, true)]
		public int? RrCountS
		{
			get;
			set;
		}

		/// <summary>
		/// RR_Count_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.RrCountL, false, FieldTypeEnum.Integer, true)]
		public int? RrCountL
		{
			get;
			set;
		}

		/// <summary>
		/// RR_1_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr1S, false, FieldTypeEnum.Integer, true)]
		public int? Rr1S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_1_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr1L, false, FieldTypeEnum.Integer, true)]
		public int? Rr1L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_2_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr2S, false, FieldTypeEnum.Integer, true)]
		public int? Rr2S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_2_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr2L, false, FieldTypeEnum.Integer, true)]
		public int? Rr2L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_3_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr3S, false, FieldTypeEnum.Integer, true)]
		public int? Rr3S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_3_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr3L, false, FieldTypeEnum.Integer, true)]
		public int? Rr3L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_4_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr4S, false, FieldTypeEnum.Integer, true)]
		public int? Rr4S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_4_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr4L, false, FieldTypeEnum.Integer, true)]
		public int? Rr4L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_5_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr5S, false, FieldTypeEnum.Integer, true)]
		public int? Rr5S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_5_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr5L, false, FieldTypeEnum.Integer, true)]
		public int? Rr5L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_6_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr6S, false, FieldTypeEnum.Integer, true)]
		public int? Rr6S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_6_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr6L, false, FieldTypeEnum.Integer, true)]
		public int? Rr6L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_7_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr7S, false, FieldTypeEnum.Integer, true)]
		public int? Rr7S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_7_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr7L, false, FieldTypeEnum.Integer, true)]
		public int? Rr7L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_8_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr8S, false, FieldTypeEnum.Integer, true)]
		public int? Rr8S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_8_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr8L, false, FieldTypeEnum.Integer, true)]
		public int? Rr8L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_9_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr9S, false, FieldTypeEnum.Integer, true)]
		public int? Rr9S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_9_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr9L, false, FieldTypeEnum.Integer, true)]
		public int? Rr9L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_10_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr10S, false, FieldTypeEnum.Integer, true)]
		public int? Rr10S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_10_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr10L, false, FieldTypeEnum.Integer, true)]
		public int? Rr10L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_11_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr11S, false, FieldTypeEnum.Integer, true)]
		public int? Rr11S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_11_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr11L, false, FieldTypeEnum.Integer, true)]
		public int? Rr11L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_12_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr12S, false, FieldTypeEnum.Integer, true)]
		public int? Rr12S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_12_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr12L, false, FieldTypeEnum.Integer, true)]
		public int? Rr12L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_13_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr13S, false, FieldTypeEnum.Integer, true)]
		public int? Rr13S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_13_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr13L, false, FieldTypeEnum.Integer, true)]
		public int? Rr13L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_14_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr14S, false, FieldTypeEnum.Integer, true)]
		public int? Rr14S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_14_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr14L, false, FieldTypeEnum.Integer, true)]
		public int? Rr14L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_15_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr15S, false, FieldTypeEnum.Integer, true)]
		public int? Rr15S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_15_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr15L, false, FieldTypeEnum.Integer, true)]
		public int? Rr15L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_16_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr16S, false, FieldTypeEnum.Integer, true)]
		public int? Rr16S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_16_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr16L, false, FieldTypeEnum.Integer, true)]
		public int? Rr16L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_17_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr17S, false, FieldTypeEnum.Integer, true)]
		public int? Rr17S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_17_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr17L, false, FieldTypeEnum.Integer, true)]
		public int? Rr17L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_18_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr18S, false, FieldTypeEnum.Integer, true)]
		public int? Rr18S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_18_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr18L, false, FieldTypeEnum.Integer, true)]
		public int? Rr18L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_19_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr19S, false, FieldTypeEnum.Integer, true)]
		public int? Rr19S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_19_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr19L, false, FieldTypeEnum.Integer, true)]
		public int? Rr19L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_20_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr20S, false, FieldTypeEnum.Integer, true)]
		public int? Rr20S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_20_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr20L, false, FieldTypeEnum.Integer, true)]
		public int? Rr20L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_21_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr21S, false, FieldTypeEnum.Integer, true)]
		public int? Rr21S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_21_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr21L, false, FieldTypeEnum.Integer, true)]
		public int? Rr21L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_22_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr22S, false, FieldTypeEnum.Integer, true)]
		public int? Rr22S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_22_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr22L, false, FieldTypeEnum.Integer, true)]
		public int? Rr22L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_23_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr23S, false, FieldTypeEnum.Integer, true)]
		public int? Rr23S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_23_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr23L, false, FieldTypeEnum.Integer, true)]
		public int? Rr23L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_24_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr24S, false, FieldTypeEnum.Integer, true)]
		public int? Rr24S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_24_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr24L, false, FieldTypeEnum.Integer, true)]
		public int? Rr24L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_25_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr25S, false, FieldTypeEnum.Integer, true)]
		public int? Rr25S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_25_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr25L, false, FieldTypeEnum.Integer, true)]
		public int? Rr25L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_26_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr26S, false, FieldTypeEnum.Integer, true)]
		public int? Rr26S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_26_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr26L, false, FieldTypeEnum.Integer, true)]
		public int? Rr26L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_27_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr27S, false, FieldTypeEnum.Integer, true)]
		public int? Rr27S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_27_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr27L, false, FieldTypeEnum.Integer, true)]
		public int? Rr27L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_28_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr28S, false, FieldTypeEnum.Integer, true)]
		public int? Rr28S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_28_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr28L, false, FieldTypeEnum.Integer, true)]
		public int? Rr28L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_29_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr29S, false, FieldTypeEnum.Integer, true)]
		public int? Rr29S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_29_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr29L, false, FieldTypeEnum.Integer, true)]
		public int? Rr29L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_30_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr30S, false, FieldTypeEnum.Integer, true)]
		public int? Rr30S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_30_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr30L, false, FieldTypeEnum.Integer, true)]
		public int? Rr30L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_31_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr31S, false, FieldTypeEnum.Integer, true)]
		public int? Rr31S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_31_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr31L, false, FieldTypeEnum.Integer, true)]
		public int? Rr31L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_32_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr32S, false, FieldTypeEnum.Integer, true)]
		public int? Rr32S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_32_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr32L, false, FieldTypeEnum.Integer, true)]
		public int? Rr32L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_33_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr33S, false, FieldTypeEnum.Integer, true)]
		public int? Rr33S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_33_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr33L, false, FieldTypeEnum.Integer, true)]
		public int? Rr33L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_34_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr34S, false, FieldTypeEnum.Integer, true)]
		public int? Rr34S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_34_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr34L, false, FieldTypeEnum.Integer, true)]
		public int? Rr34L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_35_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr35S, false, FieldTypeEnum.Integer, true)]
		public int? Rr35S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_35_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr35L, false, FieldTypeEnum.Integer, true)]
		public int? Rr35L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_36_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr36S, false, FieldTypeEnum.Integer, true)]
		public int? Rr36S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_36_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr36L, false, FieldTypeEnum.Integer, true)]
		public int? Rr36L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_37_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr37S, false, FieldTypeEnum.Integer, true)]
		public int? Rr37S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_37_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr37L, false, FieldTypeEnum.Integer, true)]
		public int? Rr37L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_38_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr38S, false, FieldTypeEnum.Integer, true)]
		public int? Rr38S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_38_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr38L, false, FieldTypeEnum.Integer, true)]
		public int? Rr38L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_39_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr39S, false, FieldTypeEnum.Integer, true)]
		public int? Rr39S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_39_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr39L, false, FieldTypeEnum.Integer, true)]
		public int? Rr39L
		{
			get;
			set;
		}

		/// <summary>
		/// RR_40_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr40S, false, FieldTypeEnum.Integer, true)]
		public int? Rr40S
		{
			get;
			set;
		}

		/// <summary>
		/// RR_40_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr40L, false, FieldTypeEnum.Integer, true)]
		public int? Rr40L
		{
			get;
			set;
		}

		/// <summary>
		/// Amount_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.AmountS, false, FieldTypeEnum.Integer, true)]
		public int? AmountS
		{
			get;
			set;
		}

		/// <summary>
		/// Amount_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.AmountL, false, FieldTypeEnum.Integer, true)]
		public int? AmountL
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
        /// 取得有設定的 TxtMapField 設定陣列
        /// </summary>
        /// <returns>傳回 TxtMapField 設定陣列</returns>
        public TxtMapField[] GetMapFields()
        {
            List<TxtMapField> mapFields = new List<TxtMapField>();

            if (this.SIdS != null && this.SIdL != null)
            {
                mapFields.Add(new TxtMapField(MappingrrXlsmdbEntity.Field.SId, this.SIdS.Value, this.SIdL.Value, new CodeChecker(1, 20)));
            }

            if (this.RrNameS != null && this.RrNameL != null)
            {
                mapFields.Add(new TxtMapField(MappingrrXlsmdbEntity.Field.RrName, this.RrNameS.Value, this.RrNameL.Value, new CodeChecker(1, 20)));
            }

            if (this.RrCountS != null && this.RrCountL != null)
            {
                mapFields.Add(new TxtMapField(MappingrrXlsmdbEntity.Field.RrCount, this.RrCountS.Value, this.RrCountL.Value, new WordChecker(1, 20)));
            }

            if (this.AmountS != null && this.AmountL != null)
            {
                mapFields.Add(new TxtMapField(MappingrrXlsmdbEntity.Field.Amount, this.AmountS.Value, this.AmountL.Value, new DecimalChecker(0M, 999999999M)));
            }

            #region 代收科目減免金額對照欄位
            {
                string[] fileds = new string[] {
                    MappingrrXlsmdbEntity.Field.Rr1, MappingrrXlsmdbEntity.Field.Rr2,
                    MappingrrXlsmdbEntity.Field.Rr3, MappingrrXlsmdbEntity.Field.Rr4,
                    MappingrrXlsmdbEntity.Field.Rr5, MappingrrXlsmdbEntity.Field.Rr6,
                    MappingrrXlsmdbEntity.Field.Rr7, MappingrrXlsmdbEntity.Field.Rr8,
                    MappingrrXlsmdbEntity.Field.Rr9, MappingrrXlsmdbEntity.Field.Rr10,
                    MappingrrXlsmdbEntity.Field.Rr11, MappingrrXlsmdbEntity.Field.Rr12,
                    MappingrrXlsmdbEntity.Field.Rr13, MappingrrXlsmdbEntity.Field.Rr14,
                    MappingrrXlsmdbEntity.Field.Rr15, MappingrrXlsmdbEntity.Field.Rr16,
                    MappingrrXlsmdbEntity.Field.Rr17, MappingrrXlsmdbEntity.Field.Rr18,
                    MappingrrXlsmdbEntity.Field.Rr19, MappingrrXlsmdbEntity.Field.Rr20,
                    MappingrrXlsmdbEntity.Field.Rr21, MappingrrXlsmdbEntity.Field.Rr22,
                    MappingrrXlsmdbEntity.Field.Rr23, MappingrrXlsmdbEntity.Field.Rr24,
                    MappingrrXlsmdbEntity.Field.Rr25, MappingrrXlsmdbEntity.Field.Rr26,
                    MappingrrXlsmdbEntity.Field.Rr27, MappingrrXlsmdbEntity.Field.Rr28,
                    MappingrrXlsmdbEntity.Field.Rr29, MappingrrXlsmdbEntity.Field.Rr30,
                    MappingrrXlsmdbEntity.Field.Rr31, MappingrrXlsmdbEntity.Field.Rr32,
                    MappingrrXlsmdbEntity.Field.Rr33, MappingrrXlsmdbEntity.Field.Rr34,
                    MappingrrXlsmdbEntity.Field.Rr35, MappingrrXlsmdbEntity.Field.Rr36,
                    MappingrrXlsmdbEntity.Field.Rr37, MappingrrXlsmdbEntity.Field.Rr38,
                    MappingrrXlsmdbEntity.Field.Rr39, MappingrrXlsmdbEntity.Field.Rr40
                };
                int?[] starts = new int?[] {
                    this.Rr1S, this.Rr2S, this.Rr3S, this.Rr4S, this.Rr5S,
                    this.Rr6S, this.Rr7S, this.Rr8S, this.Rr9S, this.Rr10S,
                    this.Rr11S, this.Rr12S, this.Rr13S, this.Rr14S, this.Rr15S,
                    this.Rr16S, this.Rr17S, this.Rr18S, this.Rr19S, this.Rr20S,
                    this.Rr21S, this.Rr22S, this.Rr23S, this.Rr24S, this.Rr25S,
                    this.Rr26S, this.Rr27S, this.Rr28S, this.Rr29S, this.Rr30S,
                    this.Rr31S, this.Rr32S, this.Rr33S, this.Rr34S, this.Rr35S,
                    this.Rr36S, this.Rr37S, this.Rr38S, this.Rr39S, this.Rr40S
                };
                int?[] lengths = new int?[] {
                    this.Rr1L, this.Rr2L, this.Rr3L, this.Rr4L, this.Rr5L,
                    this.Rr6L, this.Rr7L, this.Rr8L, this.Rr9L, this.Rr10L,
                    this.Rr11L, this.Rr12L, this.Rr13L, this.Rr14L, this.Rr15L,
                    this.Rr16L, this.Rr17L, this.Rr18L, this.Rr19L, this.Rr20L,
                    this.Rr21L, this.Rr22L, this.Rr23L, this.Rr24L, this.Rr25L,
                    this.Rr26L, this.Rr27L, this.Rr28L, this.Rr29L, this.Rr30L,
                    this.Rr31L, this.Rr32L, this.Rr33L, this.Rr34L, this.Rr35L,
                    this.Rr36L, this.Rr37L, this.Rr38L, this.Rr39L, this.Rr40L
                };
                for (int idx = 0; idx < starts.Length; idx++)
                {
                    if (starts[idx] != null && lengths[idx] != null)
                    {
                        mapFields.Add(new TxtMapField(fileds[idx], starts[idx].Value, lengths[idx].Value, new DecimalChecker(0M, 999999999M)));
                    }
                }
            }
            #endregion

            return mapFields.ToArray();
        }
        #endregion
	}
}
