/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:32:58
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
	/// Student_Course 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class StudentCourseEntity : Entity
	{
		public const string TABLE_NAME = "Student_Course";

		#region Field Name Const Class
		/// <summary>
		/// StudentCourseEntity 欄位名稱定義抽象類別
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
			/// Stu_Id 欄位名稱常數定義
			/// </summary>
			public const string StuId = "Stu_Id";

            /// <summary>
            /// 舊資料序號 (非舊學雜費轉置的資料，固定為 0) 欄位名稱常數定義
            /// </summary>
            public const string OldSeq = "Old_Seq";
			#endregion

			#region Data
			/// <summary>
			/// Credit_Id1 欄位名稱常數定義
			/// </summary>
			public const string CreditId1 = "Credit_Id1";

			/// <summary>
			/// Course_Id1 欄位名稱常數定義
			/// </summary>
			public const string CourseId1 = "Course_Id1";

			/// <summary>
			/// Credit1 欄位名稱常數定義
			/// </summary>
			public const string Credit1 = "Credit1";

			/// <summary>
			/// Credit_Id2 欄位名稱常數定義
			/// </summary>
			public const string CreditId2 = "Credit_Id2";

			/// <summary>
			/// Course_Id2 欄位名稱常數定義
			/// </summary>
			public const string CourseId2 = "Course_Id2";

			/// <summary>
			/// Credit2 欄位名稱常數定義
			/// </summary>
			public const string Credit2 = "Credit2";

			/// <summary>
			/// Credit_Id3 欄位名稱常數定義
			/// </summary>
			public const string CreditId3 = "Credit_Id3";

			/// <summary>
			/// Course_Id3 欄位名稱常數定義
			/// </summary>
			public const string CourseId3 = "Course_Id3";

			/// <summary>
			/// Credit3 欄位名稱常數定義
			/// </summary>
			public const string Credit3 = "Credit3";

			/// <summary>
			/// Credit_Id4 欄位名稱常數定義
			/// </summary>
			public const string CreditId4 = "Credit_Id4";

			/// <summary>
			/// Course_Id4 欄位名稱常數定義
			/// </summary>
			public const string CourseId4 = "Course_Id4";

			/// <summary>
			/// Credit4 欄位名稱常數定義
			/// </summary>
			public const string Credit4 = "Credit4";

			/// <summary>
			/// Credit_Id5 欄位名稱常數定義
			/// </summary>
			public const string CreditId5 = "Credit_Id5";

			/// <summary>
			/// Course_Id5 欄位名稱常數定義
			/// </summary>
			public const string CourseId5 = "Course_Id5";

			/// <summary>
			/// Credit5 欄位名稱常數定義
			/// </summary>
			public const string Credit5 = "Credit5";

			/// <summary>
			/// Credit_Id6 欄位名稱常數定義
			/// </summary>
			public const string CreditId6 = "Credit_Id6";

			/// <summary>
			/// Course_Id6 欄位名稱常數定義
			/// </summary>
			public const string CourseId6 = "Course_Id6";

			/// <summary>
			/// Credit6 欄位名稱常數定義
			/// </summary>
			public const string Credit6 = "Credit6";

			/// <summary>
			/// Credit_Id7 欄位名稱常數定義
			/// </summary>
			public const string CreditId7 = "Credit_Id7";

			/// <summary>
			/// Course_Id7 欄位名稱常數定義
			/// </summary>
			public const string CourseId7 = "Course_Id7";

			/// <summary>
			/// Credit7 欄位名稱常數定義
			/// </summary>
			public const string Credit7 = "Credit7";

			/// <summary>
			/// Credit_Id8 欄位名稱常數定義
			/// </summary>
			public const string CreditId8 = "Credit_Id8";

			/// <summary>
			/// Course_Id8 欄位名稱常數定義
			/// </summary>
			public const string CourseId8 = "Course_Id8";

			/// <summary>
			/// Credit8 欄位名稱常數定義
			/// </summary>
			public const string Credit8 = "Credit8";

			/// <summary>
			/// Credit_Id9 欄位名稱常數定義
			/// </summary>
			public const string CreditId9 = "Credit_Id9";

			/// <summary>
			/// Course_Id9 欄位名稱常數定義
			/// </summary>
			public const string CourseId9 = "Course_Id9";

			/// <summary>
			/// Credit9 欄位名稱常數定義
			/// </summary>
			public const string Credit9 = "Credit9";

			/// <summary>
			/// Credit_Id10 欄位名稱常數定義
			/// </summary>
			public const string CreditId10 = "Credit_Id10";

			/// <summary>
			/// Course_Id10 欄位名稱常數定義
			/// </summary>
			public const string CourseId10 = "Course_Id10";

			/// <summary>
			/// Credit10 欄位名稱常數定義
			/// </summary>
			public const string Credit10 = "Credit10";

			/// <summary>
			/// Credit_Id11 欄位名稱常數定義
			/// </summary>
			public const string CreditId11 = "Credit_Id11";

			/// <summary>
			/// Course_Id11 欄位名稱常數定義
			/// </summary>
			public const string CourseId11 = "Course_Id11";

			/// <summary>
			/// Credit11 欄位名稱常數定義
			/// </summary>
			public const string Credit11 = "Credit11";

			/// <summary>
			/// Credit_Id12 欄位名稱常數定義
			/// </summary>
			public const string CreditId12 = "Credit_Id12";

			/// <summary>
			/// Course_Id12 欄位名稱常數定義
			/// </summary>
			public const string CourseId12 = "Course_Id12";

			/// <summary>
			/// Credit12 欄位名稱常數定義
			/// </summary>
			public const string Credit12 = "Credit12";

			/// <summary>
			/// Credit_Id13 欄位名稱常數定義
			/// </summary>
			public const string CreditId13 = "Credit_Id13";

			/// <summary>
			/// Course_Id13 欄位名稱常數定義
			/// </summary>
			public const string CourseId13 = "Course_Id13";

			/// <summary>
			/// Credit13 欄位名稱常數定義
			/// </summary>
			public const string Credit13 = "Credit13";

			/// <summary>
			/// Credit_Id14 欄位名稱常數定義
			/// </summary>
			public const string CreditId14 = "Credit_Id14";

			/// <summary>
			/// Course_Id14 欄位名稱常數定義
			/// </summary>
			public const string CourseId14 = "Course_Id14";

			/// <summary>
			/// Credit14 欄位名稱常數定義
			/// </summary>
			public const string Credit14 = "Credit14";

			/// <summary>
			/// Credit_Id15 欄位名稱常數定義
			/// </summary>
			public const string CreditId15 = "Credit_Id15";

			/// <summary>
			/// Course_Id15 欄位名稱常數定義
			/// </summary>
			public const string CourseId15 = "Course_Id15";

			/// <summary>
			/// Credit15 欄位名稱常數定義
			/// </summary>
			public const string Credit15 = "Credit15";

			/// <summary>
			/// Credit_Id16 欄位名稱常數定義
			/// </summary>
			public const string CreditId16 = "Credit_Id16";

			/// <summary>
			/// Course_Id16 欄位名稱常數定義
			/// </summary>
			public const string CourseId16 = "Course_Id16";

			/// <summary>
			/// Credit16 欄位名稱常數定義
			/// </summary>
			public const string Credit16 = "Credit16";

			/// <summary>
			/// Credit_Id17 欄位名稱常數定義
			/// </summary>
			public const string CreditId17 = "Credit_Id17";

			/// <summary>
			/// Course_Id17 欄位名稱常數定義
			/// </summary>
			public const string CourseId17 = "Course_Id17";

			/// <summary>
			/// Credit17 欄位名稱常數定義
			/// </summary>
			public const string Credit17 = "Credit17";

			/// <summary>
			/// Credit_Id18 欄位名稱常數定義
			/// </summary>
			public const string CreditId18 = "Credit_Id18";

			/// <summary>
			/// Course_Id18 欄位名稱常數定義
			/// </summary>
			public const string CourseId18 = "Course_Id18";

			/// <summary>
			/// Credit18 欄位名稱常數定義
			/// </summary>
			public const string Credit18 = "Credit18";

			/// <summary>
			/// Credit_Id19 欄位名稱常數定義
			/// </summary>
			public const string CreditId19 = "Credit_Id19";

			/// <summary>
			/// Course_Id19 欄位名稱常數定義
			/// </summary>
			public const string CourseId19 = "Course_Id19";

			/// <summary>
			/// Credit19 欄位名稱常數定義
			/// </summary>
			public const string Credit19 = "Credit19";

			/// <summary>
			/// Credit_Id20 欄位名稱常數定義
			/// </summary>
			public const string CreditId20 = "Credit_Id20";

			/// <summary>
			/// Course_Id20 欄位名稱常數定義
			/// </summary>
			public const string CourseId20 = "Course_Id20";

			/// <summary>
			/// Credit20 欄位名稱常數定義
			/// </summary>
			public const string Credit20 = "Credit20";

			/// <summary>
			/// Credit_Id21 欄位名稱常數定義
			/// </summary>
			public const string CreditId21 = "Credit_Id21";

			/// <summary>
			/// Course_Id21 欄位名稱常數定義
			/// </summary>
			public const string CourseId21 = "Course_Id21";

			/// <summary>
			/// Credit21 欄位名稱常數定義
			/// </summary>
			public const string Credit21 = "Credit21";

			/// <summary>
			/// Credit_Id22 欄位名稱常數定義
			/// </summary>
			public const string CreditId22 = "Credit_Id22";

			/// <summary>
			/// Course_Id22 欄位名稱常數定義
			/// </summary>
			public const string CourseId22 = "Course_Id22";

			/// <summary>
			/// Credit22 欄位名稱常數定義
			/// </summary>
			public const string Credit22 = "Credit22";

			/// <summary>
			/// Credit_Id23 欄位名稱常數定義
			/// </summary>
			public const string CreditId23 = "Credit_Id23";

			/// <summary>
			/// Course_Id23 欄位名稱常數定義
			/// </summary>
			public const string CourseId23 = "Course_Id23";

			/// <summary>
			/// Credit23 欄位名稱常數定義
			/// </summary>
			public const string Credit23 = "Credit23";

			/// <summary>
			/// Credit_Id24 欄位名稱常數定義
			/// </summary>
			public const string CreditId24 = "Credit_Id24";

			/// <summary>
			/// Course_Id24 欄位名稱常數定義
			/// </summary>
			public const string CourseId24 = "Course_Id24";

			/// <summary>
			/// Credit24 欄位名稱常數定義
			/// </summary>
			public const string Credit24 = "Credit24";

			/// <summary>
			/// Credit_Id25 欄位名稱常數定義
			/// </summary>
			public const string CreditId25 = "Credit_Id25";

			/// <summary>
			/// Course_Id25 欄位名稱常數定義
			/// </summary>
			public const string CourseId25 = "Course_Id25";

			/// <summary>
			/// Credit25 欄位名稱常數定義
			/// </summary>
			public const string Credit25 = "Credit25";

			/// <summary>
			/// Credit_Id26 欄位名稱常數定義
			/// </summary>
			public const string CreditId26 = "Credit_Id26";

			/// <summary>
			/// Course_Id26 欄位名稱常數定義
			/// </summary>
			public const string CourseId26 = "Course_Id26";

			/// <summary>
			/// Credit26 欄位名稱常數定義
			/// </summary>
			public const string Credit26 = "Credit26";

			/// <summary>
			/// Credit_Id27 欄位名稱常數定義
			/// </summary>
			public const string CreditId27 = "Credit_Id27";

			/// <summary>
			/// Course_Id27 欄位名稱常數定義
			/// </summary>
			public const string CourseId27 = "Course_Id27";

			/// <summary>
			/// Credit27 欄位名稱常數定義
			/// </summary>
			public const string Credit27 = "Credit27";

			/// <summary>
			/// Credit_Id28 欄位名稱常數定義
			/// </summary>
			public const string CreditId28 = "Credit_Id28";

			/// <summary>
			/// Course_Id28 欄位名稱常數定義
			/// </summary>
			public const string CourseId28 = "Course_Id28";

			/// <summary>
			/// Credit28 欄位名稱常數定義
			/// </summary>
			public const string Credit28 = "Credit28";

			/// <summary>
			/// Credit_Id29 欄位名稱常數定義
			/// </summary>
			public const string CreditId29 = "Credit_Id29";

			/// <summary>
			/// Course_Id29 欄位名稱常數定義
			/// </summary>
			public const string CourseId29 = "Course_Id29";

			/// <summary>
			/// Credit29 欄位名稱常數定義
			/// </summary>
			public const string Credit29 = "Credit29";

			/// <summary>
			/// Credit_Id30 欄位名稱常數定義
			/// </summary>
			public const string CreditId30 = "Credit_Id30";

			/// <summary>
			/// Course_Id30 欄位名稱常數定義
			/// </summary>
			public const string CourseId30 = "Course_Id30";

			/// <summary>
			/// Credit30 欄位名稱常數定義
			/// </summary>
			public const string Credit30 = "Credit30";

			/// <summary>
			/// Credit_Id31 欄位名稱常數定義
			/// </summary>
			public const string CreditId31 = "Credit_Id31";

			/// <summary>
			/// Course_Id31 欄位名稱常數定義
			/// </summary>
			public const string CourseId31 = "Course_Id31";

			/// <summary>
			/// Credit31 欄位名稱常數定義
			/// </summary>
			public const string Credit31 = "Credit31";

			/// <summary>
			/// Credit_Id32 欄位名稱常數定義
			/// </summary>
			public const string CreditId32 = "Credit_Id32";

			/// <summary>
			/// Course_Id32 欄位名稱常數定義
			/// </summary>
			public const string CourseId32 = "Course_Id32";

			/// <summary>
			/// Credit32 欄位名稱常數定義
			/// </summary>
			public const string Credit32 = "Credit32";

			/// <summary>
			/// Credit_Id33 欄位名稱常數定義
			/// </summary>
			public const string CreditId33 = "Credit_Id33";

			/// <summary>
			/// Course_Id33 欄位名稱常數定義
			/// </summary>
			public const string CourseId33 = "Course_Id33";

			/// <summary>
			/// Credit33 欄位名稱常數定義
			/// </summary>
			public const string Credit33 = "Credit33";

			/// <summary>
			/// Credit_Id34 欄位名稱常數定義
			/// </summary>
			public const string CreditId34 = "Credit_Id34";

			/// <summary>
			/// Course_Id34 欄位名稱常數定義
			/// </summary>
			public const string CourseId34 = "Course_Id34";

			/// <summary>
			/// Credit34 欄位名稱常數定義
			/// </summary>
			public const string Credit34 = "Credit34";

			/// <summary>
			/// Credit_Id35 欄位名稱常數定義
			/// </summary>
			public const string CreditId35 = "Credit_Id35";

			/// <summary>
			/// Course_Id35 欄位名稱常數定義
			/// </summary>
			public const string CourseId35 = "Course_Id35";

			/// <summary>
			/// Credit35 欄位名稱常數定義
			/// </summary>
			public const string Credit35 = "Credit35";

			/// <summary>
			/// Credit_Id36 欄位名稱常數定義
			/// </summary>
			public const string CreditId36 = "Credit_Id36";

			/// <summary>
			/// Course_Id36 欄位名稱常數定義
			/// </summary>
			public const string CourseId36 = "Course_Id36";

			/// <summary>
			/// Credit36 欄位名稱常數定義
			/// </summary>
			public const string Credit36 = "Credit36";

			/// <summary>
			/// Credit_Id37 欄位名稱常數定義
			/// </summary>
			public const string CreditId37 = "Credit_Id37";

			/// <summary>
			/// Course_Id37 欄位名稱常數定義
			/// </summary>
			public const string CourseId37 = "Course_Id37";

			/// <summary>
			/// Credit37 欄位名稱常數定義
			/// </summary>
			public const string Credit37 = "Credit37";

			/// <summary>
			/// Credit_Id38 欄位名稱常數定義
			/// </summary>
			public const string CreditId38 = "Credit_Id38";

			/// <summary>
			/// Course_Id38 欄位名稱常數定義
			/// </summary>
			public const string CourseId38 = "Course_Id38";

			/// <summary>
			/// Credit38 欄位名稱常數定義
			/// </summary>
			public const string Credit38 = "Credit38";

			/// <summary>
			/// Credit_Id39 欄位名稱常數定義
			/// </summary>
			public const string CreditId39 = "Credit_Id39";

			/// <summary>
			/// Course_Id39 欄位名稱常數定義
			/// </summary>
			public const string CourseId39 = "Course_Id39";

			/// <summary>
			/// Credit39 欄位名稱常數定義
			/// </summary>
			public const string Credit39 = "Credit39";

			/// <summary>
			/// Credit_Id40 欄位名稱常數定義
			/// </summary>
			public const string CreditId40 = "Credit_Id40";

			/// <summary>
			/// Course_Id40 欄位名稱常數定義
			/// </summary>
			public const string CourseId40 = "Course_Id40";

			/// <summary>
			/// Credit40 欄位名稱常數定義
			/// </summary>
			public const string Credit40 = "Credit40";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// StudentCourseEntity 類別建構式
		/// </summary>
		public StudentCourseEntity()
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
		[FieldSpec(Field.YearId, true, FieldTypeEnum.Char, 4, false)]
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

		private string _StuId = null;
		/// <summary>
		/// Stu_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.StuId, true, FieldTypeEnum.VarChar, 20, false)]
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

        private int _OldSeq = 0;
        /// <summary>
        /// 舊資料序號 (非舊學雜費轉置的資料，固定為 0)
        /// </summary>
        [FieldSpec(Field.OldSeq, true, FieldTypeEnum.Integer, false)]
        public int OldSeq
        {
            get
            {
                return _OldSeq;
            }
            set
            {
                _OldSeq = value < 0 ? 0 : value;
            }
        }
		#endregion

		#region Data
		/// <summary>
		/// Credit_Id1 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId1, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId1
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id1 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId1, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId1
		{
			get;
			set;
		}

		/// <summary>
		/// Credit1 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit1, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit1
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id2 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId2, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId2
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id2 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId2, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId2
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit2, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit2
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id3 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId3, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId3
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id3 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId3, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId3
		{
			get;
			set;
		}

		/// <summary>
		/// Credit3 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit3, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit3
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id4 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId4, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId4
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id4 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId4, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId4
		{
			get;
			set;
		}

		/// <summary>
		/// Credit4 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit4, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit4
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id5 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId5, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId5
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id5 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId5, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId5
		{
			get;
			set;
		}

		/// <summary>
		/// Credit5 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit5, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit5
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id6 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId6, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId6
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id6 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId6, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId6
		{
			get;
			set;
		}

		/// <summary>
		/// Credit6 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit6, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit6
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id7 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId7, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId7
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id7 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId7, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId7
		{
			get;
			set;
		}

		/// <summary>
		/// Credit7 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit7, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit7
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id8 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId8, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId8
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id8 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId8, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId8
		{
			get;
			set;
		}

		/// <summary>
		/// Credit8 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit8, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit8
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id9 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId9, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId9
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id9 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId9, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId9
		{
			get;
			set;
		}

		/// <summary>
		/// Credit9 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit9, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit9
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id10 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId10, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId10
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id10 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId10, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId10
		{
			get;
			set;
		}

		/// <summary>
		/// Credit10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit10, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit10
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id11 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId11, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId11
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id11 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId11, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId11
		{
			get;
			set;
		}

		/// <summary>
		/// Credit11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit11, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit11
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id12 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId12, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId12
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id12 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId12, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId12
		{
			get;
			set;
		}

		/// <summary>
		/// Credit12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit12, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit12
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id13 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId13, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId13
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id13 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId13, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId13
		{
			get;
			set;
		}

		/// <summary>
		/// Credit13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit13, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit13
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id14 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId14, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId14
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id14 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId14, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId14
		{
			get;
			set;
		}

		/// <summary>
		/// Credit14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit14, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit14
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id15 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId15, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId15
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id15 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId15, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId15
		{
			get;
			set;
		}

		/// <summary>
		/// Credit15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit15, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit15
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id16 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId16, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId16
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id16 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId16, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId16
		{
			get;
			set;
		}

		/// <summary>
		/// Credit16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit16, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit16
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id17 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId17, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId17
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id17 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId17, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId17
		{
			get;
			set;
		}

		/// <summary>
		/// Credit17 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit17, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit17
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id18 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId18, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId18
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id18 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId18, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId18
		{
			get;
			set;
		}

		/// <summary>
		/// Credit18 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit18, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit18
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id19 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId19, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId19
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id19 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId19, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId19
		{
			get;
			set;
		}

		/// <summary>
		/// Credit19 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit19, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit19
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id20 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId20, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId20
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id20 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId20, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId20
		{
			get;
			set;
		}

		/// <summary>
		/// Credit20 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit20, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit20
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id21 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId21, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId21
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id21 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId21, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId21
		{
			get;
			set;
		}

		/// <summary>
		/// Credit21 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit21, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit21
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id22 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId22, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId22
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id22 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId22, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId22
		{
			get;
			set;
		}

		/// <summary>
		/// Credit22 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit22, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit22
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id23 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId23, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId23
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id23 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId23, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId23
		{
			get;
			set;
		}

		/// <summary>
		/// Credit23 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit23, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit23
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id24 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId24, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId24
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id24 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId24, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId24
		{
			get;
			set;
		}

		/// <summary>
		/// Credit24 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit24, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit24
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id25 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId25, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId25
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id25 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId25, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId25
		{
			get;
			set;
		}

		/// <summary>
		/// Credit25 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit25, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit25
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id26 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId26, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId26
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id26 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId26, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId26
		{
			get;
			set;
		}

		/// <summary>
		/// Credit26 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit26, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit26
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id27 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId27, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId27
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id27 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId27, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId27
		{
			get;
			set;
		}

		/// <summary>
		/// Credit27 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit27, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit27
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id28 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId28, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId28
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id28 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId28, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId28
		{
			get;
			set;
		}

		/// <summary>
		/// Credit28 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit28, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit28
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id29 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId29, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId29
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id29 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId29, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId29
		{
			get;
			set;
		}

		/// <summary>
		/// Credit29 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit29, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit29
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id30 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId30, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId30
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id30 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId30, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId30
		{
			get;
			set;
		}

		/// <summary>
		/// Credit30 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit30, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit30
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id31 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId31, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId31
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id31 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId31, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId31
		{
			get;
			set;
		}

		/// <summary>
		/// Credit31 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit31, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit31
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id32 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId32, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId32
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id32 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId32, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId32
		{
			get;
			set;
		}

		/// <summary>
		/// Credit32 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit32, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit32
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id33 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId33, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId33
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id33 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId33, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId33
		{
			get;
			set;
		}

		/// <summary>
		/// Credit33 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit33, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit33
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id34 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId34, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId34
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id34 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId34, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId34
		{
			get;
			set;
		}

		/// <summary>
		/// Credit34 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit34, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit34
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id35 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId35, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId35
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id35 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId35, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId35
		{
			get;
			set;
		}

		/// <summary>
		/// Credit35 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit35, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit35
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id36 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId36, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId36
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id36 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId36, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId36
		{
			get;
			set;
		}

		/// <summary>
		/// Credit36 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit36, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit36
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id37 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId37, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId37
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id37 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId37, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId37
		{
			get;
			set;
		}

		/// <summary>
		/// Credit37 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit37, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit37
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id38 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId38, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId38
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id38 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId38, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId38
		{
			get;
			set;
		}

		/// <summary>
		/// Credit38 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit38, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit38
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id39 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId39, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId39
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id39 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId39, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId39
		{
			get;
			set;
		}

		/// <summary>
		/// Credit39 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit39, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit39
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id40 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId40, false, FieldTypeEnum.VarChar, 8, true)]
		public string CreditId40
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id40 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId40, false, FieldTypeEnum.VarChar, 8, true)]
		public string CourseId40
		{
			get;
			set;
		}

		/// <summary>
		/// Credit40 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit40, false, FieldTypeEnum.Decimal, true)]
		public decimal? Credit40
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
