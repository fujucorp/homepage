/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/02/05 11:51:38
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
	/// 課程收費標準
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ClassStandardEntity : Entity
	{
		public const string TABLE_NAME = "Class_Standard";

		#region Field Name Const Class
		/// <summary>
		/// ClassStandardEntity 欄位名稱定義抽象類別
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
			/// Course_Id 欄位名稱常數定義
			/// </summary>
			public const string CourseId = "Course_Id";
			#endregion

			#region Data
			/// <summary>
			/// Course_Name 欄位名稱常數定義
			/// </summary>
			public const string CourseName = "Course_Name";

			/// <summary>
			/// Course_Credit 欄位名稱常數定義
			/// </summary>
			public const string CourseCredit = "Course_Credit";

			/// <summary>
			/// Credit_No 欄位名稱常數定義
			/// </summary>
			public const string CreditNo = "Credit_No";

			/// <summary>
			/// Course_Cprice 欄位名稱常數定義
			/// </summary>
			public const string CourseCprice = "Course_Cprice";

			#region [Old] 
            ///// <summary>
            ///// status 欄位名稱常數定義
            ///// </summary>
            //public const string Status = "status";

            ///// <summary>
            ///// crt_date 欄位名稱常數定義
            ///// </summary>
            //public const string CrtDate = "crt_date";

            ///// <summary>
            ///// crt_user 欄位名稱常數定義
            ///// </summary>
            //public const string CrtUser = "crt_user";

            ///// <summary>
            ///// mdy_date 欄位名稱常數定義
            ///// </summary>
            //public const string MdyDate = "mdy_date";

            ///// <summary>
            ///// mdy_user 欄位名稱常數定義
            ///// </summary>
            //public const string MdyUser = "mdy_user";
			#endregion
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// ClassStandardEntity 類別建構式
		/// </summary>
		public ClassStandardEntity()
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

		private string _CourseId = null;
		/// <summary>
		/// Course_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId, true, FieldTypeEnum.VarChar, 20, false)]
		public string CourseId
		{
			get
			{
				return _CourseId;
			}
			set
			{
				_CourseId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// Course_Name 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseName, false, FieldTypeEnum.VarChar, 20, true)]
		public string CourseName
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Credit 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseCredit, false, FieldTypeEnum.Decimal, true)]
		public decimal? CourseCredit
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_No 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditNo, false, FieldTypeEnum.Char, 8, true)]
		public string CreditNo
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Cprice 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseCprice, false, FieldTypeEnum.Decimal, true)]
		public decimal? CourseCprice
		{
			get;
			set;
		}

		#region [Old]
        ///// <summary>
        ///// status 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
        //public string Status
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// crt_date 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
        //public DateTime CrtDate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// crt_user 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
        //public string CrtUser
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// mdy_date 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
        //public DateTime? MdyDate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// mdy_user 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.MdyUser, false, FieldTypeEnum.VarChar, 20, true)]
        //public string MdyUser
        //{
        //    get;
        //    set;
        //}
        #endregion
		#endregion
		#endregion
	}
}
