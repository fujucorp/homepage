/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:35:08
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
	/// Identify_Standard4 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class IdentifyStandard4Entity : Entity
	{
		public const string TABLE_NAME = "Identify_Standard4";

		#region Field Name Const Class
		/// <summary>
		/// IdentifyStandard4Entity 欄位名稱定義抽象類別
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
			/// Identify_Id 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId = "Identify_Id";
			#endregion

			#region Data
			/// <summary>
			/// Id_Way 欄位名稱常數定義
			/// </summary>
			public const string IdWay = "Id_Way";

			/// <summary>
			/// Id_Amount01 欄位名稱常數定義
			/// </summary>
			public const string IdAmount01 = "Id_Amount01";

			/// <summary>
			/// Id_Amount02 欄位名稱常數定義
			/// </summary>
			public const string IdAmount02 = "Id_Amount02";

			/// <summary>
			/// Id_Amount03 欄位名稱常數定義
			/// </summary>
			public const string IdAmount03 = "Id_Amount03";

			/// <summary>
			/// Id_Amount04 欄位名稱常數定義
			/// </summary>
			public const string IdAmount04 = "Id_Amount04";

			/// <summary>
			/// Id_Amount05 欄位名稱常數定義
			/// </summary>
			public const string IdAmount05 = "Id_Amount05";

			/// <summary>
			/// Id_Amount06 欄位名稱常數定義
			/// </summary>
			public const string IdAmount06 = "Id_Amount06";

			/// <summary>
			/// Id_Amount07 欄位名稱常數定義
			/// </summary>
			public const string IdAmount07 = "Id_Amount07";

			/// <summary>
			/// Id_Amount08 欄位名稱常數定義
			/// </summary>
			public const string IdAmount08 = "Id_Amount08";

			/// <summary>
			/// Id_Amount09 欄位名稱常數定義
			/// </summary>
			public const string IdAmount09 = "Id_Amount09";

			/// <summary>
			/// Id_Amount10 欄位名稱常數定義
			/// </summary>
			public const string IdAmount10 = "Id_Amount10";

			/// <summary>
			/// Id_Amount11 欄位名稱常數定義
			/// </summary>
			public const string IdAmount11 = "Id_Amount11";

			/// <summary>
			/// Id_Amount12 欄位名稱常數定義
			/// </summary>
			public const string IdAmount12 = "Id_Amount12";

			/// <summary>
			/// Id_Amount13 欄位名稱常數定義
			/// </summary>
			public const string IdAmount13 = "Id_Amount13";

			/// <summary>
			/// Id_Amount14 欄位名稱常數定義
			/// </summary>
			public const string IdAmount14 = "Id_Amount14";

			/// <summary>
			/// Id_Amount15 欄位名稱常數定義
			/// </summary>
			public const string IdAmount15 = "Id_Amount15";

			/// <summary>
			/// Id_Amount16 欄位名稱常數定義
			/// </summary>
			public const string IdAmount16 = "Id_Amount16";

			/// <summary>
			/// Id_Amount17 欄位名稱常數定義
			/// </summary>
			public const string IdAmount17 = "Id_Amount17";

			/// <summary>
			/// Id_Amount18 欄位名稱常數定義
			/// </summary>
			public const string IdAmount18 = "Id_Amount18";

			/// <summary>
			/// Id_Amount19 欄位名稱常數定義
			/// </summary>
			public const string IdAmount19 = "Id_Amount19";

			/// <summary>
			/// Id_Amount20 欄位名稱常數定義
			/// </summary>
			public const string IdAmount20 = "Id_Amount20";

			/// <summary>
			/// Id_Amount21 欄位名稱常數定義
			/// </summary>
			public const string IdAmount21 = "Id_Amount21";

			/// <summary>
			/// Id_Amount22 欄位名稱常數定義
			/// </summary>
			public const string IdAmount22 = "Id_Amount22";

			/// <summary>
			/// Id_Amount23 欄位名稱常數定義
			/// </summary>
			public const string IdAmount23 = "Id_Amount23";

			/// <summary>
			/// Id_Amount24 欄位名稱常數定義
			/// </summary>
			public const string IdAmount24 = "Id_Amount24";

			/// <summary>
			/// Id_Amount25 欄位名稱常數定義
			/// </summary>
			public const string IdAmount25 = "Id_Amount25";

			/// <summary>
			/// Id_Amount26 欄位名稱常數定義
			/// </summary>
			public const string IdAmount26 = "Id_Amount26";

			/// <summary>
			/// Id_Amount27 欄位名稱常數定義
			/// </summary>
			public const string IdAmount27 = "Id_Amount27";

			/// <summary>
			/// Id_Amount28 欄位名稱常數定義
			/// </summary>
			public const string IdAmount28 = "Id_Amount28";

			/// <summary>
			/// Id_Amount29 欄位名稱常數定義
			/// </summary>
			public const string IdAmount29 = "Id_Amount29";

			/// <summary>
			/// Id_Amount30 欄位名稱常數定義
			/// </summary>
			public const string IdAmount30 = "Id_Amount30";

			/// <summary>
			/// Id_Amount31 欄位名稱常數定義
			/// </summary>
			public const string IdAmount31 = "Id_Amount31";

			/// <summary>
			/// Id_Amount32 欄位名稱常數定義
			/// </summary>
			public const string IdAmount32 = "Id_Amount32";

			/// <summary>
			/// Id_Amount33 欄位名稱常數定義
			/// </summary>
			public const string IdAmount33 = "Id_Amount33";

			/// <summary>
			/// Id_Amount34 欄位名稱常數定義
			/// </summary>
			public const string IdAmount34 = "Id_Amount34";

			/// <summary>
			/// Id_Amount35 欄位名稱常數定義
			/// </summary>
			public const string IdAmount35 = "Id_Amount35";

			/// <summary>
			/// Id_Amount36 欄位名稱常數定義
			/// </summary>
			public const string IdAmount36 = "Id_Amount36";

			/// <summary>
			/// Id_Amount37 欄位名稱常數定義
			/// </summary>
			public const string IdAmount37 = "Id_Amount37";

			/// <summary>
			/// Id_Amount38 欄位名稱常數定義
			/// </summary>
			public const string IdAmount38 = "Id_Amount38";

			/// <summary>
			/// Id_Amount39 欄位名稱常數定義
			/// </summary>
			public const string IdAmount39 = "Id_Amount39";

			/// <summary>
			/// Id_Amount40 欄位名稱常數定義
			/// </summary>
			public const string IdAmount40 = "Id_Amount40";

			/// <summary>
			/// Id_Num01 欄位名稱常數定義
			/// </summary>
			public const string IdNum01 = "Id_Num01";

			/// <summary>
			/// Id_Dno01 欄位名稱常數定義
			/// </summary>
			public const string IdDno01 = "Id_Dno01";

			/// <summary>
			/// Id_Num02 欄位名稱常數定義
			/// </summary>
			public const string IdNum02 = "Id_Num02";

			/// <summary>
			/// Id_Dno02 欄位名稱常數定義
			/// </summary>
			public const string IdDno02 = "Id_Dno02";

			/// <summary>
			/// Id_Num03 欄位名稱常數定義
			/// </summary>
			public const string IdNum03 = "Id_Num03";

			/// <summary>
			/// Id_Dno03 欄位名稱常數定義
			/// </summary>
			public const string IdDno03 = "Id_Dno03";

			/// <summary>
			/// Id_Num04 欄位名稱常數定義
			/// </summary>
			public const string IdNum04 = "Id_Num04";

			/// <summary>
			/// Id_Dno04 欄位名稱常數定義
			/// </summary>
			public const string IdDno04 = "Id_Dno04";

			/// <summary>
			/// Id_Num05 欄位名稱常數定義
			/// </summary>
			public const string IdNum05 = "Id_Num05";

			/// <summary>
			/// Id_Dno05 欄位名稱常數定義
			/// </summary>
			public const string IdDno05 = "Id_Dno05";

			/// <summary>
			/// Id_Num06 欄位名稱常數定義
			/// </summary>
			public const string IdNum06 = "Id_Num06";

			/// <summary>
			/// Id_Dno06 欄位名稱常數定義
			/// </summary>
			public const string IdDno06 = "Id_Dno06";

			/// <summary>
			/// Id_Num07 欄位名稱常數定義
			/// </summary>
			public const string IdNum07 = "Id_Num07";

			/// <summary>
			/// Id_Dno07 欄位名稱常數定義
			/// </summary>
			public const string IdDno07 = "Id_Dno07";

			/// <summary>
			/// Id_Num08 欄位名稱常數定義
			/// </summary>
			public const string IdNum08 = "Id_Num08";

			/// <summary>
			/// Id_Dno08 欄位名稱常數定義
			/// </summary>
			public const string IdDno08 = "Id_Dno08";

			/// <summary>
			/// Id_Num09 欄位名稱常數定義
			/// </summary>
			public const string IdNum09 = "Id_Num09";

			/// <summary>
			/// Id_Dno09 欄位名稱常數定義
			/// </summary>
			public const string IdDno09 = "Id_Dno09";

			/// <summary>
			/// Id_Num10 欄位名稱常數定義
			/// </summary>
			public const string IdNum10 = "Id_Num10";

			/// <summary>
			/// Id_Dno10 欄位名稱常數定義
			/// </summary>
			public const string IdDno10 = "Id_Dno10";

			/// <summary>
			/// Id_Num11 欄位名稱常數定義
			/// </summary>
			public const string IdNum11 = "Id_Num11";

			/// <summary>
			/// Id_Dno11 欄位名稱常數定義
			/// </summary>
			public const string IdDno11 = "Id_Dno11";

			/// <summary>
			/// Id_Num12 欄位名稱常數定義
			/// </summary>
			public const string IdNum12 = "Id_Num12";

			/// <summary>
			/// Id_Dno12 欄位名稱常數定義
			/// </summary>
			public const string IdDno12 = "Id_Dno12";

			/// <summary>
			/// Id_Num13 欄位名稱常數定義
			/// </summary>
			public const string IdNum13 = "Id_Num13";

			/// <summary>
			/// Id_Dno13 欄位名稱常數定義
			/// </summary>
			public const string IdDno13 = "Id_Dno13";

			/// <summary>
			/// Id_Num14 欄位名稱常數定義
			/// </summary>
			public const string IdNum14 = "Id_Num14";

			/// <summary>
			/// Id_Dno14 欄位名稱常數定義
			/// </summary>
			public const string IdDno14 = "Id_Dno14";

			/// <summary>
			/// Id_Num15 欄位名稱常數定義
			/// </summary>
			public const string IdNum15 = "Id_Num15";

			/// <summary>
			/// Id_Dno15 欄位名稱常數定義
			/// </summary>
			public const string IdDno15 = "Id_Dno15";

			/// <summary>
			/// Id_Num16 欄位名稱常數定義
			/// </summary>
			public const string IdNum16 = "Id_Num16";

			/// <summary>
			/// Id_Dno16 欄位名稱常數定義
			/// </summary>
			public const string IdDno16 = "Id_Dno16";

			/// <summary>
			/// Id_Num17 欄位名稱常數定義
			/// </summary>
			public const string IdNum17 = "Id_Num17";

			/// <summary>
			/// Id_Dno17 欄位名稱常數定義
			/// </summary>
			public const string IdDno17 = "Id_Dno17";

			/// <summary>
			/// Id_Num18 欄位名稱常數定義
			/// </summary>
			public const string IdNum18 = "Id_Num18";

			/// <summary>
			/// Id_Dno18 欄位名稱常數定義
			/// </summary>
			public const string IdDno18 = "Id_Dno18";

			/// <summary>
			/// Id_Num19 欄位名稱常數定義
			/// </summary>
			public const string IdNum19 = "Id_Num19";

			/// <summary>
			/// Id_Dno19 欄位名稱常數定義
			/// </summary>
			public const string IdDno19 = "Id_Dno19";

			/// <summary>
			/// Id_Num20 欄位名稱常數定義
			/// </summary>
			public const string IdNum20 = "Id_Num20";

			/// <summary>
			/// Id_Dno20 欄位名稱常數定義
			/// </summary>
			public const string IdDno20 = "Id_Dno20";

			/// <summary>
			/// Id_Num21 欄位名稱常數定義
			/// </summary>
			public const string IdNum21 = "Id_Num21";

			/// <summary>
			/// Id_Dno21 欄位名稱常數定義
			/// </summary>
			public const string IdDno21 = "Id_Dno21";

			/// <summary>
			/// Id_Num22 欄位名稱常數定義
			/// </summary>
			public const string IdNum22 = "Id_Num22";

			/// <summary>
			/// Id_Dno22 欄位名稱常數定義
			/// </summary>
			public const string IdDno22 = "Id_Dno22";

			/// <summary>
			/// Id_Num23 欄位名稱常數定義
			/// </summary>
			public const string IdNum23 = "Id_Num23";

			/// <summary>
			/// Id_Dno23 欄位名稱常數定義
			/// </summary>
			public const string IdDno23 = "Id_Dno23";

			/// <summary>
			/// Id_Num24 欄位名稱常數定義
			/// </summary>
			public const string IdNum24 = "Id_Num24";

			/// <summary>
			/// Id_Dno24 欄位名稱常數定義
			/// </summary>
			public const string IdDno24 = "Id_Dno24";

			/// <summary>
			/// Id_Num25 欄位名稱常數定義
			/// </summary>
			public const string IdNum25 = "Id_Num25";

			/// <summary>
			/// Id_Dno25 欄位名稱常數定義
			/// </summary>
			public const string IdDno25 = "Id_Dno25";

			/// <summary>
			/// Id_Num26 欄位名稱常數定義
			/// </summary>
			public const string IdNum26 = "Id_Num26";

			/// <summary>
			/// Id_Dno26 欄位名稱常數定義
			/// </summary>
			public const string IdDno26 = "Id_Dno26";

			/// <summary>
			/// Id_Num27 欄位名稱常數定義
			/// </summary>
			public const string IdNum27 = "Id_Num27";

			/// <summary>
			/// Id_Dno27 欄位名稱常數定義
			/// </summary>
			public const string IdDno27 = "Id_Dno27";

			/// <summary>
			/// Id_Num28 欄位名稱常數定義
			/// </summary>
			public const string IdNum28 = "Id_Num28";

			/// <summary>
			/// Id_Dno28 欄位名稱常數定義
			/// </summary>
			public const string IdDno28 = "Id_Dno28";

			/// <summary>
			/// Id_Num29 欄位名稱常數定義
			/// </summary>
			public const string IdNum29 = "Id_Num29";

			/// <summary>
			/// Id_Dno29 欄位名稱常數定義
			/// </summary>
			public const string IdDno29 = "Id_Dno29";

			/// <summary>
			/// Id_Num30 欄位名稱常數定義
			/// </summary>
			public const string IdNum30 = "Id_Num30";

			/// <summary>
			/// Id_Dno30 欄位名稱常數定義
			/// </summary>
			public const string IdDno30 = "Id_Dno30";

			/// <summary>
			/// Id_Num31 欄位名稱常數定義
			/// </summary>
			public const string IdNum31 = "Id_Num31";

			/// <summary>
			/// Id_Dno31 欄位名稱常數定義
			/// </summary>
			public const string IdDno31 = "Id_Dno31";

			/// <summary>
			/// Id_Num32 欄位名稱常數定義
			/// </summary>
			public const string IdNum32 = "Id_Num32";

			/// <summary>
			/// Id_Dno32 欄位名稱常數定義
			/// </summary>
			public const string IdDno32 = "Id_Dno32";

			/// <summary>
			/// Id_Num33 欄位名稱常數定義
			/// </summary>
			public const string IdNum33 = "Id_Num33";

			/// <summary>
			/// Id_Dno33 欄位名稱常數定義
			/// </summary>
			public const string IdDno33 = "Id_Dno33";

			/// <summary>
			/// Id_Num34 欄位名稱常數定義
			/// </summary>
			public const string IdNum34 = "Id_Num34";

			/// <summary>
			/// Id_Dno34 欄位名稱常數定義
			/// </summary>
			public const string IdDno34 = "Id_Dno34";

			/// <summary>
			/// Id_Num35 欄位名稱常數定義
			/// </summary>
			public const string IdNum35 = "Id_Num35";

			/// <summary>
			/// Id_Dno35 欄位名稱常數定義
			/// </summary>
			public const string IdDno35 = "Id_Dno35";

			/// <summary>
			/// Id_Num36 欄位名稱常數定義
			/// </summary>
			public const string IdNum36 = "Id_Num36";

			/// <summary>
			/// Id_Dno36 欄位名稱常數定義
			/// </summary>
			public const string IdDno36 = "Id_Dno36";

			/// <summary>
			/// Id_Num37 欄位名稱常數定義
			/// </summary>
			public const string IdNum37 = "Id_Num37";

			/// <summary>
			/// Id_Dno37 欄位名稱常數定義
			/// </summary>
			public const string IdDno37 = "Id_Dno37";

			/// <summary>
			/// Id_Num38 欄位名稱常數定義
			/// </summary>
			public const string IdNum38 = "Id_Num38";

			/// <summary>
			/// Id_Dno38 欄位名稱常數定義
			/// </summary>
			public const string IdDno38 = "Id_Dno38";

			/// <summary>
			/// Id_Num39 欄位名稱常數定義
			/// </summary>
			public const string IdNum39 = "Id_Num39";

			/// <summary>
			/// Id_Dno39 欄位名稱常數定義
			/// </summary>
			public const string IdDno39 = "Id_Dno39";

			/// <summary>
			/// Id_Num40 欄位名稱常數定義
			/// </summary>
			public const string IdNum40 = "Id_Num40";

			/// <summary>
			/// Id_Dno40 欄位名稱常數定義
			/// </summary>
			public const string IdDno40 = "Id_Dno40";

			/// <summary>
			/// Id_Item 欄位名稱常數定義
			/// </summary>
			public const string IdItem = "Id_Item";

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
		/// IdentifyStandard4Entity 類別建構式
		/// </summary>
		public IdentifyStandard4Entity()
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

		#region [MDY:20220808] 2022擴充案 身分註記代碼型別調整為 NVarChar(140)
		private string _IdentifyId = null;
		/// <summary>
		/// 身分註記代碼
		/// </summary>
		[FieldSpec(Field.IdentifyId, true, FieldTypeEnum.NVarChar, 140, false)]
		public string IdentifyId
		{
			get
			{
				return _IdentifyId;
			}
			set
			{
				_IdentifyId = value == null ? null : value.Trim();
			}
		}
		#endregion
		#endregion

		#region Data
		/// <summary>
		/// Id_Way 欄位屬性
		/// 1:依百分比
		/// 2:依金額
		/// </summary>
		[FieldSpec(Field.IdWay, false, FieldTypeEnum.Char, 1, false)]
		public string IdWay
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount01 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount01, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount01
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount02 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount02, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount02
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount03 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount03, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount03
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount04 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount04, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount04
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount05 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount05, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount05
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount06 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount06, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount06
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount07 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount07, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount07
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount08 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount08, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount08
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount09 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount09, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount09
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount10 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount10, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount10
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount11 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount11, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount11
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount12 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount12, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount12
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount13 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount13, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount13
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount14 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount14, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount14
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount15 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount15, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount15
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount16 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount16, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount16
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount17 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount17, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount17
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount18 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount18, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount18
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount19 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount19, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount19
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount20 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount20, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount20
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount21 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount21, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount21
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount22 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount22, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount22
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount23 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount23, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount23
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount24 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount24, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount24
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount25 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount25, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount25
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount26 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount26, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount26
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount27 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount27, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount27
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount28 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount28, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount28
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount29 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount29, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount29
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount30 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount30, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount30
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount31 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount31, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount31
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount32 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount32, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount32
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount33 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount33, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount33
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount34 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount34, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount34
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount35 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount35, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount35
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount36 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount36, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount36
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount37 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount37, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount37
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount38 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount38, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount38
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount39 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount39, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount39
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Amount40 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdAmount40, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdAmount40
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num01 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum01, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum01
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno01 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno01, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno01
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num02 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum02, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum02
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno02 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno02, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno02
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num03 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum03, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum03
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno03 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno03, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno03
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num04 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum04, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum04
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno04 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno04, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno04
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num05 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum05, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum05
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno05 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno05, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno05
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num06 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum06, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum06
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno06 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno06, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno06
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num07 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum07, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum07
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno07 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno07, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno07
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num08 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum08, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum08
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno08 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno08, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno08
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num09 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum09, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum09
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno09 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno09, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno09
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num10 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum10, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum10
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno10 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno10, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno10
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num11 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum11, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum11
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno11 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno11, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno11
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num12 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum12, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum12
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno12 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno12, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno12
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num13 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum13, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum13
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno13 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno13, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno13
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num14 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum14, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum14
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno14 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno14, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno14
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num15 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum15, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum15
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno15 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno15, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno15
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num16 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum16, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum16
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno16 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno16, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno16
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num17 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum17, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum17
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno17 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno17, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno17
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num18 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum18, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum18
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno18 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno18, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno18
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num19 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum19, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum19
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno19 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno19, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno19
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num20 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum20, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum20
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno20 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno20, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno20
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num21 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum21, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum21
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno21 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno21, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno21
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num22 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum22, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum22
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno22 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno22, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno22
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num23 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum23, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum23
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno23 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno23, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno23
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num24 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum24, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum24
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno24 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno24, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno24
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num25 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum25, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum25
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno25 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno25, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno25
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num26 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum26, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum26
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno26 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno26, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno26
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num27 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum27, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum27
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno27 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno27, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno27
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num28 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum28, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum28
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno28 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno28, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno28
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num29 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum29, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum29
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno29 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno29, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno29
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num30 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum30, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum30
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno30 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno30, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno30
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num31 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum31, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum31
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno31 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno31, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno31
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num32 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum32, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum32
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno32 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno32, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno32
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num33 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum33, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum33
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno33 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno33, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno33
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num34 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum34, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum34
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno34 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno34, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno34
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num35 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum35, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum35
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno35 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno35, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno35
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num36 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum36, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum36
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno36 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno36, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno36
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num37 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum37, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum37
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno37 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno37, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno37
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num38 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum38, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum38
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno38 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno38, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno38
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num39 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum39, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum39
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno39 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno39, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno39
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Num40 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdNum40, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdNum40
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Dno40 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdDno40, false, FieldTypeEnum.Decimal, true)]
		public decimal? IdDno40
		{
			get;
			set;
		}

		/// <summary>
		/// Id_Item 欄位屬性
		/// </summary>
		[FieldSpec(Field.IdItem, false, FieldTypeEnum.Char, 2, true)]
		public string IdItem
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
        public decimal GetDenominator(Int32 index)
        {
            decimal Denominator = 1;
            decimal defaultValue = 0;

            switch (index)
            {
                case 1:
                    Denominator = (this.IdDno01 == null ? defaultValue : this.IdDno01.Value);
                    break;
                case 2:
                    Denominator = (this.IdDno02 == null ? defaultValue : this.IdDno02.Value);
                    break;
                case 3:
                    Denominator = (this.IdDno03 == null ? defaultValue : this.IdDno03.Value);
                    break;
                case 4:
                    Denominator = (this.IdDno04 == null ? defaultValue : this.IdDno04.Value);
                    break;
                case 5:
                    Denominator = (this.IdDno05 == null ? defaultValue : this.IdDno05.Value);
                    break;
                case 6:
                    Denominator = (this.IdDno06 == null ? defaultValue : this.IdDno06.Value);
                    break;
                case 7:
                    Denominator = (this.IdDno07 == null ? defaultValue : this.IdDno07.Value);
                    break;
                case 8:
                    Denominator = (this.IdDno08 == null ? defaultValue : this.IdDno08.Value);
                    break;
                case 9:
                    Denominator = (this.IdDno09 == null ? defaultValue : this.IdDno09.Value);
                    break;
                case 10:
                    Denominator = (this.IdDno10 == null ? defaultValue : this.IdDno10.Value);
                    break;
                case 11:
                    Denominator = (this.IdDno11 == null ? defaultValue : this.IdDno11.Value);
                    break;
                case 12:
                    Denominator = (this.IdDno12 == null ? defaultValue : this.IdDno12.Value);
                    break;
                case 13:
                    Denominator = (this.IdDno13 == null ? defaultValue : this.IdDno13.Value);
                    break;
                case 14:
                    Denominator = (this.IdDno14 == null ? defaultValue : this.IdDno14.Value);
                    break;
                case 15:
                    Denominator = (this.IdDno15 == null ? defaultValue : this.IdDno15.Value);
                    break;
                case 16:
                    Denominator = (this.IdDno16 == null ? defaultValue : this.IdDno16.Value);
                    break;
                case 17:
                    Denominator = (this.IdDno17 == null ? defaultValue : this.IdDno17.Value);
                    break;
                case 18:
                    Denominator = (this.IdDno18 == null ? defaultValue : this.IdDno18.Value);
                    break;
                case 19:
                    Denominator = (this.IdDno19 == null ? defaultValue : this.IdDno19.Value);
                    break;
                case 20:
                    Denominator = (this.IdDno20 == null ? defaultValue : this.IdDno20.Value);
                    break;
                case 21:
                    Denominator = (this.IdDno21 == null ? defaultValue : this.IdDno21.Value);
                    break;
                case 22:
                    Denominator = (this.IdDno22 == null ? defaultValue : this.IdDno22.Value);
                    break;
                case 23:
                    Denominator = (this.IdDno23 == null ? defaultValue : this.IdDno23.Value);
                    break;
                case 24:
                    Denominator = (this.IdDno24 == null ? defaultValue : this.IdDno24.Value);
                    break;
                case 25:
                    Denominator = (this.IdDno25 == null ? defaultValue : this.IdDno25.Value);
                    break;
                case 26:
                    Denominator = (this.IdDno26 == null ? defaultValue : this.IdDno26.Value);
                    break;
                case 27:
                    Denominator = (this.IdDno27 == null ? defaultValue : this.IdDno27.Value);
                    break;
                case 28:
                    Denominator = (this.IdDno28 == null ? defaultValue : this.IdDno28.Value);
                    break;
                case 29:
                    Denominator = (this.IdDno29 == null ? defaultValue : this.IdDno29.Value);
                    break;
                case 30:
                    Denominator = (this.IdDno30 == null ? defaultValue : this.IdDno30.Value);
                    break;
                case 31:
                    Denominator = (this.IdDno31 == null ? defaultValue : this.IdDno31.Value);
                    break;
                case 32:
                    Denominator = (this.IdDno32 == null ? defaultValue : this.IdDno32.Value);
                    break;
                case 33:
                    Denominator = (this.IdDno33 == null ? defaultValue : this.IdDno33.Value);
                    break;
                case 34:
                    Denominator = (this.IdDno34 == null ? defaultValue : this.IdDno34.Value);
                    break;
                case 35:
                    Denominator = (this.IdDno35 == null ? defaultValue : this.IdDno35.Value);
                    break;
                case 36:
                    Denominator = (this.IdDno36 == null ? defaultValue : this.IdDno36.Value);
                    break;
                case 37:
                    Denominator = (this.IdDno37 == null ? defaultValue : this.IdDno37.Value);
                    break;
                case 38:
                    Denominator = (this.IdDno38 == null ? defaultValue : this.IdDno38.Value);
                    break;
                case 39:
                    Denominator = (this.IdDno39 == null ? defaultValue : this.IdDno39.Value);
                    break;
                case 40:
                    Denominator = (this.IdDno40 == null ? defaultValue : this.IdDno40.Value);
                    break;
            }

            return Denominator;
        }

        public decimal GetMolecular(Int32 index)
        {
            decimal molecular = 0;
            decimal defaultValue = 0;

            switch (index)
            {
                case 1:
                    molecular = (this.IdNum01 == null ? defaultValue : this.IdNum01.Value);
                    break;
                case 2:
                    molecular = (this.IdNum02 == null ? defaultValue : this.IdNum02.Value);
                    break;
                case 3:
                    molecular = (this.IdNum03 == null ? defaultValue : this.IdNum03.Value);
                    break;
                case 4:
                    molecular = (this.IdNum04 == null ? defaultValue : this.IdNum04.Value);
                    break;
                case 5:
                    molecular = (this.IdNum05 == null ? defaultValue : this.IdNum05.Value);
                    break;
                case 6:
                    molecular = (this.IdNum06 == null ? defaultValue : this.IdNum06.Value);
                    break;
                case 7:
                    molecular = (this.IdNum07 == null ? defaultValue : this.IdNum07.Value);
                    break;
                case 8:
                    molecular = (this.IdNum08 == null ? defaultValue : this.IdNum08.Value);
                    break;
                case 9:
                    molecular = (this.IdNum09 == null ? defaultValue : this.IdNum09.Value);
                    break;
                case 10:
                    molecular = (this.IdNum10 == null ? defaultValue : this.IdNum10.Value);
                    break;
                case 11:
                    molecular = (this.IdNum11 == null ? defaultValue : this.IdNum11.Value);
                    break;
                case 12:
                    molecular = (this.IdNum12 == null ? defaultValue : this.IdNum12.Value);
                    break;
                case 13:
                    molecular = (this.IdNum13 == null ? defaultValue : this.IdNum13.Value);
                    break;
                case 14:
                    molecular = (this.IdNum14 == null ? defaultValue : this.IdNum14.Value);
                    break;
                case 15:
                    molecular = (this.IdNum15 == null ? defaultValue : this.IdNum15.Value);
                    break;
                case 16:
                    molecular = (this.IdNum16 == null ? defaultValue : this.IdNum16.Value);
                    break;
                case 17:
                    molecular = (this.IdNum17 == null ? defaultValue : this.IdNum17.Value);
                    break;
                case 18:
                    molecular = (this.IdNum18 == null ? defaultValue : this.IdNum18.Value);
                    break;
                case 19:
                    molecular = (this.IdNum19 == null ? defaultValue : this.IdNum19.Value);
                    break;
                case 20:
                    molecular = (this.IdNum20 == null ? defaultValue : this.IdNum20.Value);
                    break;
                case 21:
                    molecular = (this.IdNum21 == null ? defaultValue : this.IdNum21.Value);
                    break;
                case 22:
                    molecular = (this.IdNum22 == null ? defaultValue : this.IdNum22.Value);
                    break;
                case 23:
                    molecular = (this.IdNum23 == null ? defaultValue : this.IdNum23.Value);
                    break;
                case 24:
                    molecular = (this.IdNum24 == null ? defaultValue : this.IdNum24.Value);
                    break;
                case 25:
                    molecular = (this.IdNum25 == null ? defaultValue : this.IdNum25.Value);
                    break;
                case 26:
                    molecular = (this.IdNum26 == null ? defaultValue : this.IdNum26.Value);
                    break;
                case 27:
                    molecular = (this.IdNum27 == null ? defaultValue : this.IdNum27.Value);
                    break;
                case 28:
                    molecular = (this.IdNum28 == null ? defaultValue : this.IdNum28.Value);
                    break;
                case 29:
                    molecular = (this.IdNum29 == null ? defaultValue : this.IdNum29.Value);
                    break;
                case 30:
                    molecular = (this.IdNum30 == null ? defaultValue : this.IdNum30.Value);
                    break;
                case 31:
                    molecular = (this.IdNum31 == null ? defaultValue : this.IdNum31.Value);
                    break;
                case 32:
                    molecular = (this.IdNum32 == null ? defaultValue : this.IdNum32.Value);
                    break;
                case 33:
                    molecular = (this.IdNum33 == null ? defaultValue : this.IdNum33.Value);
                    break;
                case 34:
                    molecular = (this.IdNum34 == null ? defaultValue : this.IdNum34.Value);
                    break;
                case 35:
                    molecular = (this.IdNum35 == null ? defaultValue : this.IdNum35.Value);
                    break;
                case 36:
                    molecular = (this.IdNum36 == null ? defaultValue : this.IdNum36.Value);
                    break;
                case 37:
                    molecular = (this.IdNum37 == null ? defaultValue : this.IdNum37.Value);
                    break;
                case 38:
                    molecular = (this.IdNum38 == null ? defaultValue : this.IdNum38.Value);
                    break;
                case 39:
                    molecular = (this.IdNum39 == null ? defaultValue : this.IdNum39.Value);
                    break;
                case 40:
                    molecular = (this.IdNum40 == null ? defaultValue : this.IdNum40.Value);
                    break;
            }

            return molecular;
        }

        //public decimal GetLimit(Int32 index)
        //{
        //    decimal limit = 0;
        //    decimal defaultValue = 0;

        //    //switch (index)
        //    //{
        //    //    case 1:
        //    //        limit = (this.Limit01 == null ? defaultValue : this.Limit01.Value);
        //    //        break;
        //    //    case 2:
        //    //        limit = (this.Limit02 == null ? defaultValue : this.Limit02.Value);
        //    //        break;
        //    //    case 3:
        //    //        limit = (this.Limit03 == null ? defaultValue : this.Limit03.Value);
        //    //        break;
        //    //    case 4:
        //    //        limit = (this.Limit04 == null ? defaultValue : this.Limit04.Value);
        //    //        break;
        //    //    case 5:
        //    //        limit = (this.Limit05 == null ? defaultValue : this.Limit05.Value);
        //    //        break;
        //    //    case 6:
        //    //        limit = (this.Limit06 == null ? defaultValue : this.Limit06.Value);
        //    //        break;
        //    //    case 7:
        //    //        limit = (this.Limit07 == null ? defaultValue : this.Limit07.Value);
        //    //        break;
        //    //    case 8:
        //    //        limit = (this.Limit08 == null ? defaultValue : this.Limit08.Value);
        //    //        break;
        //    //    case 9:
        //    //        limit = (this.Limit09 == null ? defaultValue : this.Limit09.Value);
        //    //        break;
        //    //    case 10:
        //    //        limit = (this.Limit10 == null ? defaultValue : this.Limit10.Value);
        //    //        break;
        //    //    case 11:
        //    //        limit = (this.Limit11 == null ? defaultValue : this.Limit11.Value);
        //    //        break;
        //    //    case 12:
        //    //        limit = (this.Limit12 == null ? defaultValue : this.Limit12.Value);
        //    //        break;
        //    //    case 13:
        //    //        limit = (this.Limit13 == null ? defaultValue : this.Limit13.Value);
        //    //        break;
        //    //    case 14:
        //    //        limit = (this.Limit14 == null ? defaultValue : this.Limit14.Value);
        //    //        break;
        //    //    case 15:
        //    //        limit = (this.Limit15 == null ? defaultValue : this.Limit15.Value);
        //    //        break;
        //    //    case 16:
        //    //        limit = (this.Limit16 == null ? defaultValue : this.Limit16.Value);
        //    //        break;
        //    //    case 17:
        //    //        limit = (this.Limit17 == null ? defaultValue : this.Limit17.Value);
        //    //        break;
        //    //    case 18:
        //    //        limit = (this.Limit18 == null ? defaultValue : this.Limit18.Value);
        //    //        break;
        //    //    case 19:
        //    //        limit = (this.Limit19 == null ? defaultValue : this.Limit19.Value);
        //    //        break;
        //    //    case 20:
        //    //        limit = (this.Limit20 == null ? defaultValue : this.Limit20.Value);
        //    //        break;
        //    //    case 21:
        //    //        limit = (this.Limit21 == null ? defaultValue : this.Limit21.Value);
        //    //        break;
        //    //    case 22:
        //    //        limit = (this.Limit22 == null ? defaultValue : this.Limit22.Value);
        //    //        break;
        //    //    case 23:
        //    //        limit = (this.Limit23 == null ? defaultValue : this.Limit23.Value);
        //    //        break;
        //    //    case 24:
        //    //        limit = (this.Limit24 == null ? defaultValue : this.Limit24.Value);
        //    //        break;
        //    //    case 25:
        //    //        limit = (this.Limit25 == null ? defaultValue : this.Limit25.Value);
        //    //        break;
        //    //    case 26:
        //    //        limit = (this.Limit26 == null ? defaultValue : this.Limit26.Value);
        //    //        break;
        //    //    case 27:
        //    //        limit = (this.Limit27 == null ? defaultValue : this.Limit27.Value);
        //    //        break;
        //    //    case 28:
        //    //        limit = (this.Limit28 == null ? defaultValue : this.Limit28.Value);
        //    //        break;
        //    //    case 29:
        //    //        limit = (this.Limit29 == null ? defaultValue : this.Limit29.Value);
        //    //        break;
        //    //    case 30:
        //    //        limit = (this.Limit30 == null ? defaultValue : this.Limit30.Value);
        //    //        break;
        //    //    case 31:
        //    //        limit = (this.Limit31 == null ? defaultValue : this.Limit31.Value);
        //    //        break;
        //    //    case 32:
        //    //        limit = (this.Limit32 == null ? defaultValue : this.Limit32.Value);
        //    //        break;
        //    //    case 33:
        //    //        limit = (this.Limit33 == null ? defaultValue : this.Limit33.Value);
        //    //        break;
        //    //    case 34:
        //    //        limit = (this.Limit34 == null ? defaultValue : this.Limit34.Value);
        //    //        break;
        //    //    case 35:
        //    //        limit = (this.Limit35 == null ? defaultValue : this.Limit35.Value);
        //    //        break;
        //    //    case 36:
        //    //        limit = (this.Limit36 == null ? defaultValue : this.Limit36.Value);
        //    //        break;
        //    //    case 37:
        //    //        limit = (this.Limit37 == null ? defaultValue : this.Limit37.Value);
        //    //        break;
        //    //    case 38:
        //    //        limit = (this.Limit38 == null ? defaultValue : this.Limit38.Value);
        //    //        break;
        //    //    case 39:
        //    //        limit = (this.Limit39 == null ? defaultValue : this.Limit39.Value);
        //    //        break;
        //    //    case 40:
        //    //        limit = (this.Limit40 == null ? defaultValue : this.Limit40.Value);
        //    //        break;
        //    //}
        //    return limit;
        //}

        public decimal GetReduceAmount(Int32 index)
        {
            decimal reduce_amount = 0;
            decimal defaultValue = 0;

            switch (index)
            {
                case 1:
                    reduce_amount = (this.IdAmount01 == null ? defaultValue : this.IdAmount01.Value);
                    break;
                case 2:
                    reduce_amount = (this.IdAmount02 == null ? defaultValue : this.IdAmount02.Value);
                    break;
                case 3:
                    reduce_amount = (this.IdAmount03 == null ? defaultValue : this.IdAmount03.Value);
                    break;
                case 4:
                    reduce_amount = (this.IdAmount04 == null ? defaultValue : this.IdAmount04.Value);
                    break;
                case 5:
                    reduce_amount = (this.IdAmount05 == null ? defaultValue : this.IdAmount05.Value);
                    break;
                case 6:
                    reduce_amount = (this.IdAmount06 == null ? defaultValue : this.IdAmount06.Value);
                    break;
                case 7:
                    reduce_amount = (this.IdAmount07 == null ? defaultValue : this.IdAmount07.Value);
                    break;
                case 8:
                    reduce_amount = (this.IdAmount08 == null ? defaultValue : this.IdAmount08.Value);
                    break;
                case 9:
                    reduce_amount = (this.IdAmount09 == null ? defaultValue : this.IdAmount09.Value);
                    break;
                case 10:
                    reduce_amount = (this.IdAmount10 == null ? defaultValue : this.IdAmount10.Value);
                    break;
                case 11:
                    reduce_amount = (this.IdAmount11 == null ? defaultValue : this.IdAmount11.Value);
                    break;
                case 12:
                    reduce_amount = (this.IdAmount12 == null ? defaultValue : this.IdAmount12.Value);
                    break;
                case 13:
                    reduce_amount = (this.IdAmount13 == null ? defaultValue : this.IdAmount13.Value);
                    break;
                case 14:
                    reduce_amount = (this.IdAmount14 == null ? defaultValue : this.IdAmount14.Value);
                    break;
                case 15:
                    reduce_amount = (this.IdAmount15 == null ? defaultValue : this.IdAmount15.Value);
                    break;
                case 16:
                    reduce_amount = (this.IdAmount16 == null ? defaultValue : this.IdAmount16.Value);
                    break;
                case 17:
                    reduce_amount = (this.IdAmount17 == null ? defaultValue : this.IdAmount17.Value);
                    break;
                case 18:
                    reduce_amount = (this.IdAmount18 == null ? defaultValue : this.IdAmount18.Value);
                    break;
                case 19:
                    reduce_amount = (this.IdAmount19 == null ? defaultValue : this.IdAmount19.Value);
                    break;
                case 20:
                    reduce_amount = (this.IdAmount20 == null ? defaultValue : this.IdAmount20.Value);
                    break;
                case 21:
                    reduce_amount = (this.IdAmount21 == null ? defaultValue : this.IdAmount21.Value);
                    break;
                case 22:
                    reduce_amount = (this.IdAmount22 == null ? defaultValue : this.IdAmount22.Value);
                    break;
                case 23:
                    reduce_amount = (this.IdAmount23 == null ? defaultValue : this.IdAmount23.Value);
                    break;
                case 24:
                    reduce_amount = (this.IdAmount24 == null ? defaultValue : this.IdAmount24.Value);
                    break;
                case 25:
                    reduce_amount = (this.IdAmount25 == null ? defaultValue : this.IdAmount25.Value);
                    break;
                case 26:
                    reduce_amount = (this.IdAmount26 == null ? defaultValue : this.IdAmount26.Value);
                    break;
                case 27:
                    reduce_amount = (this.IdAmount27 == null ? defaultValue : this.IdAmount27.Value);
                    break;
                case 28:
                    reduce_amount = (this.IdAmount28 == null ? defaultValue : this.IdAmount28.Value);
                    break;
                case 29:
                    reduce_amount = (this.IdAmount29 == null ? defaultValue : this.IdAmount29.Value);
                    break;
                case 30:
                    reduce_amount = (this.IdAmount30 == null ? defaultValue : this.IdAmount30.Value);
                    break;
                case 31:
                    reduce_amount = (this.IdAmount31 == null ? defaultValue : this.IdAmount31.Value);
                    break;
                case 32:
                    reduce_amount = (this.IdAmount32 == null ? defaultValue : this.IdAmount32.Value);
                    break;
                case 33:
                    reduce_amount = (this.IdAmount33 == null ? defaultValue : this.IdAmount33.Value);
                    break;
                case 34:
                    reduce_amount = (this.IdAmount34 == null ? defaultValue : this.IdAmount34.Value);
                    break;
                case 35:
                    reduce_amount = (this.IdAmount35 == null ? defaultValue : this.IdAmount35.Value);
                    break;
                case 36:
                    reduce_amount = (this.IdAmount36 == null ? defaultValue : this.IdAmount36.Value);
                    break;
                case 37:
                    reduce_amount = (this.IdAmount37 == null ? defaultValue : this.IdAmount37.Value);
                    break;
                case 38:
                    reduce_amount = (this.IdAmount38 == null ? defaultValue : this.IdAmount38.Value);
                    break;
                case 39:
                    reduce_amount = (this.IdAmount39 == null ? defaultValue : this.IdAmount39.Value);
                    break;
                case 40:
                    reduce_amount = (this.IdAmount40 == null ? defaultValue : this.IdAmount40.Value);
                    break;
            }

            return reduce_amount;
        }
        #endregion
	}
}
