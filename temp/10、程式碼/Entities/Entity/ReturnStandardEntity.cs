/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:33:27
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
	/// Return_Standard 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ReturnStandardEntity : Entity
	{
		public const string TABLE_NAME = "Return_Standard";

		#region Field Name Const Class
		/// <summary>
		/// ReturnStandardEntity 欄位名稱定義抽象類別
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
			/// Return_Id 欄位名稱常數定義
			/// </summary>
			public const string ReturnId = "Return_Id";
			#endregion

			#region Data
			/// <summary>
			/// Re_Num01 欄位名稱常數定義
			/// </summary>
			public const string ReNum01 = "Re_Num01";

			/// <summary>
			/// Re_Dno01 欄位名稱常數定義
			/// </summary>
			public const string ReDno01 = "Re_Dno01";

			/// <summary>
			/// Re_Num02 欄位名稱常數定義
			/// </summary>
			public const string ReNum02 = "Re_Num02";

			/// <summary>
			/// Re_Dno02 欄位名稱常數定義
			/// </summary>
			public const string ReDno02 = "Re_Dno02";

			/// <summary>
			/// Re_Num03 欄位名稱常數定義
			/// </summary>
			public const string ReNum03 = "Re_Num03";

			/// <summary>
			/// Re_Dno03 欄位名稱常數定義
			/// </summary>
			public const string ReDno03 = "Re_Dno03";

			/// <summary>
			/// Re_Num04 欄位名稱常數定義
			/// </summary>
			public const string ReNum04 = "Re_Num04";

			/// <summary>
			/// Re_Dno04 欄位名稱常數定義
			/// </summary>
			public const string ReDno04 = "Re_Dno04";

			/// <summary>
			/// Re_Num05 欄位名稱常數定義
			/// </summary>
			public const string ReNum05 = "Re_Num05";

			/// <summary>
			/// Re_Dno05 欄位名稱常數定義
			/// </summary>
			public const string ReDno05 = "Re_Dno05";

			/// <summary>
			/// Re_Num06 欄位名稱常數定義
			/// </summary>
			public const string ReNum06 = "Re_Num06";

			/// <summary>
			/// Re_Dno06 欄位名稱常數定義
			/// </summary>
			public const string ReDno06 = "Re_Dno06";

			/// <summary>
			/// Re_Num07 欄位名稱常數定義
			/// </summary>
			public const string ReNum07 = "Re_Num07";

			/// <summary>
			/// Re_Dno07 欄位名稱常數定義
			/// </summary>
			public const string ReDno07 = "Re_Dno07";

			/// <summary>
			/// Re_Num08 欄位名稱常數定義
			/// </summary>
			public const string ReNum08 = "Re_Num08";

			/// <summary>
			/// Re_Dno08 欄位名稱常數定義
			/// </summary>
			public const string ReDno08 = "Re_Dno08";

			/// <summary>
			/// Re_Num09 欄位名稱常數定義
			/// </summary>
			public const string ReNum09 = "Re_Num09";

			/// <summary>
			/// Re_Dno09 欄位名稱常數定義
			/// </summary>
			public const string ReDno09 = "Re_Dno09";

			/// <summary>
			/// Re_Num10 欄位名稱常數定義
			/// </summary>
			public const string ReNum10 = "Re_Num10";

			/// <summary>
			/// Re_Dno10 欄位名稱常數定義
			/// </summary>
			public const string ReDno10 = "Re_Dno10";

			/// <summary>
			/// Re_Num11 欄位名稱常數定義
			/// </summary>
			public const string ReNum11 = "Re_Num11";

			/// <summary>
			/// Re_Dno11 欄位名稱常數定義
			/// </summary>
			public const string ReDno11 = "Re_Dno11";

			/// <summary>
			/// Re_Num12 欄位名稱常數定義
			/// </summary>
			public const string ReNum12 = "Re_Num12";

			/// <summary>
			/// Re_Dno12 欄位名稱常數定義
			/// </summary>
			public const string ReDno12 = "Re_Dno12";

			/// <summary>
			/// Re_Num13 欄位名稱常數定義
			/// </summary>
			public const string ReNum13 = "Re_Num13";

			/// <summary>
			/// Re_Dno13 欄位名稱常數定義
			/// </summary>
			public const string ReDno13 = "Re_Dno13";

			/// <summary>
			/// Re_Num14 欄位名稱常數定義
			/// </summary>
			public const string ReNum14 = "Re_Num14";

			/// <summary>
			/// Re_Dno14 欄位名稱常數定義
			/// </summary>
			public const string ReDno14 = "Re_Dno14";

			/// <summary>
			/// Re_Num15 欄位名稱常數定義
			/// </summary>
			public const string ReNum15 = "Re_Num15";

			/// <summary>
			/// Re_Dno15 欄位名稱常數定義
			/// </summary>
			public const string ReDno15 = "Re_Dno15";

			/// <summary>
			/// Re_Num16 欄位名稱常數定義
			/// </summary>
			public const string ReNum16 = "Re_Num16";

			/// <summary>
			/// Re_Dno16 欄位名稱常數定義
			/// </summary>
			public const string ReDno16 = "Re_Dno16";

			/// <summary>
			/// Re_Num17 欄位名稱常數定義
			/// </summary>
			public const string ReNum17 = "Re_Num17";

			/// <summary>
			/// Re_Dno17 欄位名稱常數定義
			/// </summary>
			public const string ReDno17 = "Re_Dno17";

			/// <summary>
			/// Re_Num18 欄位名稱常數定義
			/// </summary>
			public const string ReNum18 = "Re_Num18";

			/// <summary>
			/// Re_Dno18 欄位名稱常數定義
			/// </summary>
			public const string ReDno18 = "Re_Dno18";

			/// <summary>
			/// Re_Num19 欄位名稱常數定義
			/// </summary>
			public const string ReNum19 = "Re_Num19";

			/// <summary>
			/// Re_Dno19 欄位名稱常數定義
			/// </summary>
			public const string ReDno19 = "Re_Dno19";

			/// <summary>
			/// Re_Num20 欄位名稱常數定義
			/// </summary>
			public const string ReNum20 = "Re_Num20";

			/// <summary>
			/// Re_Dno20 欄位名稱常數定義
			/// </summary>
			public const string ReDno20 = "Re_Dno20";

			/// <summary>
			/// Re_Num21 欄位名稱常數定義
			/// </summary>
			public const string ReNum21 = "Re_Num21";

			/// <summary>
			/// Re_Dno21 欄位名稱常數定義
			/// </summary>
			public const string ReDno21 = "Re_Dno21";

			/// <summary>
			/// Re_Num22 欄位名稱常數定義
			/// </summary>
			public const string ReNum22 = "Re_Num22";

			/// <summary>
			/// Re_Dno22 欄位名稱常數定義
			/// </summary>
			public const string ReDno22 = "Re_Dno22";

			/// <summary>
			/// Re_Num23 欄位名稱常數定義
			/// </summary>
			public const string ReNum23 = "Re_Num23";

			/// <summary>
			/// Re_Dno23 欄位名稱常數定義
			/// </summary>
			public const string ReDno23 = "Re_Dno23";

			/// <summary>
			/// Re_Num24 欄位名稱常數定義
			/// </summary>
			public const string ReNum24 = "Re_Num24";

			/// <summary>
			/// Re_Dno24 欄位名稱常數定義
			/// </summary>
			public const string ReDno24 = "Re_Dno24";

			/// <summary>
			/// Re_Num25 欄位名稱常數定義
			/// </summary>
			public const string ReNum25 = "Re_Num25";

			/// <summary>
			/// Re_Dno25 欄位名稱常數定義
			/// </summary>
			public const string ReDno25 = "Re_Dno25";

			/// <summary>
			/// Re_Num26 欄位名稱常數定義
			/// </summary>
			public const string ReNum26 = "Re_Num26";

			/// <summary>
			/// Re_Dno26 欄位名稱常數定義
			/// </summary>
			public const string ReDno26 = "Re_Dno26";

			/// <summary>
			/// Re_Num27 欄位名稱常數定義
			/// </summary>
			public const string ReNum27 = "Re_Num27";

			/// <summary>
			/// Re_Dno27 欄位名稱常數定義
			/// </summary>
			public const string ReDno27 = "Re_Dno27";

			/// <summary>
			/// Re_Num28 欄位名稱常數定義
			/// </summary>
			public const string ReNum28 = "Re_Num28";

			/// <summary>
			/// Re_Dno28 欄位名稱常數定義
			/// </summary>
			public const string ReDno28 = "Re_Dno28";

			/// <summary>
			/// Re_Num29 欄位名稱常數定義
			/// </summary>
			public const string ReNum29 = "Re_Num29";

			/// <summary>
			/// Re_Dno29 欄位名稱常數定義
			/// </summary>
			public const string ReDno29 = "Re_Dno29";

			/// <summary>
			/// Re_Num30 欄位名稱常數定義
			/// </summary>
			public const string ReNum30 = "Re_Num30";

			/// <summary>
			/// Re_Dno30 欄位名稱常數定義
			/// </summary>
			public const string ReDno30 = "Re_Dno30";

			/// <summary>
			/// Re_Num31 欄位名稱常數定義
			/// </summary>
			public const string ReNum31 = "Re_Num31";

			/// <summary>
			/// Re_Dno31 欄位名稱常數定義
			/// </summary>
			public const string ReDno31 = "Re_Dno31";

			/// <summary>
			/// Re_Num32 欄位名稱常數定義
			/// </summary>
			public const string ReNum32 = "Re_Num32";

			/// <summary>
			/// Re_Dno32 欄位名稱常數定義
			/// </summary>
			public const string ReDno32 = "Re_Dno32";

			/// <summary>
			/// Re_Num33 欄位名稱常數定義
			/// </summary>
			public const string ReNum33 = "Re_Num33";

			/// <summary>
			/// Re_Dno33 欄位名稱常數定義
			/// </summary>
			public const string ReDno33 = "Re_Dno33";

			/// <summary>
			/// Re_Num34 欄位名稱常數定義
			/// </summary>
			public const string ReNum34 = "Re_Num34";

			/// <summary>
			/// Re_Dno34 欄位名稱常數定義
			/// </summary>
			public const string ReDno34 = "Re_Dno34";

			/// <summary>
			/// Re_Num35 欄位名稱常數定義
			/// </summary>
			public const string ReNum35 = "Re_Num35";

			/// <summary>
			/// Re_Dno35 欄位名稱常數定義
			/// </summary>
			public const string ReDno35 = "Re_Dno35";

			/// <summary>
			/// Re_Num36 欄位名稱常數定義
			/// </summary>
			public const string ReNum36 = "Re_Num36";

			/// <summary>
			/// Re_Dno36 欄位名稱常數定義
			/// </summary>
			public const string ReDno36 = "Re_Dno36";

			/// <summary>
			/// Re_Num37 欄位名稱常數定義
			/// </summary>
			public const string ReNum37 = "Re_Num37";

			/// <summary>
			/// Re_Dno_37 欄位名稱常數定義
			/// </summary>
			public const string ReDno37 = "Re_Dno_37";

			/// <summary>
			/// Re_Num38 欄位名稱常數定義
			/// </summary>
			public const string ReNum38 = "Re_Num38";

			/// <summary>
			/// Re_Dno38 欄位名稱常數定義
			/// </summary>
			public const string ReDno38 = "Re_Dno38";

			/// <summary>
			/// Re_Num39 欄位名稱常數定義
			/// </summary>
			public const string ReNum39 = "Re_Num39";

			/// <summary>
			/// Re_Dno39 欄位名稱常數定義
			/// </summary>
			public const string ReDno39 = "Re_Dno39";

			/// <summary>
			/// Re_Num40 欄位名稱常數定義
			/// </summary>
			public const string ReNum40 = "Re_Num40";

			/// <summary>
			/// Re_Dno40 欄位名稱常數定義
			/// </summary>
			public const string ReDno40 = "Re_Dno40";

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
		/// ReturnStandardEntity 類別建構式
		/// </summary>
		public ReturnStandardEntity()
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

		private string _ReturnId = null;
		/// <summary>
		/// Return_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReturnId, true, FieldTypeEnum.VarChar, 20, false)]
		public string ReturnId
		{
			get
			{
				return _ReturnId;
			}
			set
			{
				_ReturnId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// Re_Num01 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum01, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum01
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno01 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno01, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno01
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num02 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum02, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum02
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno02 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno02, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno02
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num03 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum03, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum03
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno03 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno03, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno03
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num04 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum04, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum04
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno04 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno04, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno04
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num05 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum05, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum05
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno05 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno05, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno05
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num06 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum06, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum06
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno06 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno06, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno06
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num07 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum07, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum07
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno07 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno07, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno07
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num08 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum08, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum08
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno08 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno08, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno08
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num09 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum09, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum09
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno09 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno09, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno09
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num10 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum10, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum10
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno10 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno10, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno10
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num11 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum11, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum11
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno11 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno11, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno11
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num12 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum12, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum12
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno12 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno12, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno12
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num13 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum13, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum13
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno13 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno13, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno13
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num14 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum14, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum14
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno14 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno14, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno14
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num15 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum15, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum15
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno15 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno15, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno15
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num16 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum16, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum16
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno16 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno16, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno16
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num17 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum17, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum17
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno17 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno17, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno17
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num18 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum18, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum18
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno18 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno18, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno18
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num19 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum19, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum19
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno19 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno19, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno19
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num20 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum20, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum20
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno20 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno20, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno20
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num21 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum21, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum21
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno21 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno21, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno21
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num22 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum22, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum22
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno22 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno22, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno22
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num23 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum23, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum23
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno23 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno23, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno23
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num24 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum24, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum24
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno24 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno24, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno24
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num25 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum25, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum25
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno25 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno25, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno25
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num26 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum26, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum26
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno26 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno26, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno26
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num27 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum27, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum27
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno27 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno27, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno27
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num28 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum28, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum28
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno28 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno28, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno28
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num29 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum29, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum29
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno29 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno29, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno29
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num30 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum30, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum30
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno30 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno30, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno30
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num31 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum31, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum31
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno31 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno31, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno31
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num32 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum32, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum32
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno32 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno32, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno32
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num33 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum33, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum33
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno33 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno33, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno33
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num34 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum34, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum34
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno34 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno34, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno34
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num35 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum35, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum35
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno35 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno35, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno35
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num36 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum36, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum36
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno36 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno36, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno36
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num37 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum37, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum37
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno_37 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno37, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno37
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num38 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum38, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum38
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno38 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno38, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno38
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num39 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum39, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum39
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno39 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno39, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno39
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Num40 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReNum40, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReNum40
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Dno40 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReDno40, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReDno40
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
