/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/01/18 23:50:35
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
	/// 上傳課程收費標準對照表
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class MappingcsXlsmdbEntity : Entity
	{
		public const string TABLE_NAME = "MappingCS_Xlsmdb";

		#region Field Name Const Class
		/// <summary>
		/// MappingcsXlsmdbEntity 欄位名稱定義抽象類別
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
			/// Course_Id 欄位名稱常數定義
			/// </summary>
			public const string CourseId = "Course_Id";

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
		/// MappingcsXlsmdbEntity 類別建構式
		/// </summary>
		public MappingcsXlsmdbEntity()
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
		/// Course_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId, false, FieldTypeEnum.VarChar, 20, true)]
		public string CourseId
		{
			get;
			set;
		}

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
		[FieldSpec(Field.CourseCredit, false, FieldTypeEnum.VarChar, 20, true)]
		public string CourseCredit
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_No 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditNo, false, FieldTypeEnum.VarChar, 20, true)]
		public string CreditNo
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Cprice 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseCprice, false, FieldTypeEnum.VarChar, 20, true)]
		public string CourseCprice
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
        public XlsMapField[] GetMapFields()
        {
            List<XlsMapField> mapFields = new List<XlsMapField>();

            #region 課程收費標準對照欄位 (StudentMasterEntity)
            {
                if (!String.IsNullOrWhiteSpace(this.CourseId))
                {
                    mapFields.Add(new XlsMapField(Field.CourseId, this.CourseId, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.CourseName))
                {
                    //20160306 jj
                    //mapFields.Add(new XlsMapField(Field.CourseName, this.CourseName, new CodeChecker(1, 20)));
                    mapFields.Add(new XlsMapField(Field.CourseName, this.CourseName, new WordChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.CourseCredit))
                {
                    mapFields.Add(new XlsMapField(Field.CourseCredit, this.CourseCredit, new NumberChecker()));
                }
                if (!String.IsNullOrWhiteSpace(this.CreditNo))
                {
                    mapFields.Add(new XlsMapField(Field.CreditNo, this.CreditNo, new CodeChecker(1, 8)));
                }
                if (!String.IsNullOrWhiteSpace(this.CourseCprice))
                {
                    mapFields.Add(new XlsMapField(Field.CourseCprice, this.CourseCprice, new NumberChecker()));
                }
            }
            #endregion

            return mapFields.ToArray();
        }
        #endregion
	}
}
