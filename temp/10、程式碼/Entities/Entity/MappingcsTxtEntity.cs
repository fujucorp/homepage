/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/01/18 23:50:26
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
	public partial class MappingcsTxtEntity : Entity
	{
		public const string TABLE_NAME = "MappingCS_Txt";

		#region Field Name Const Class
		/// <summary>
		/// MappingcsTxtEntity 欄位名稱定義抽象類別
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
			/// Course_Id_S 欄位名稱常數定義
			/// </summary>
			public const string CourseIdS = "Course_Id_S";

			/// <summary>
			/// Course_Id_L 欄位名稱常數定義
			/// </summary>
			public const string CourseIdL = "Course_Id_L";

			/// <summary>
			/// Course_Name_S 欄位名稱常數定義
			/// </summary>
			public const string CourseNameS = "Course_Name_S";

			/// <summary>
			/// Course_Name_L 欄位名稱常數定義
			/// </summary>
			public const string CourseNameL = "Course_Name_L";

			/// <summary>
			/// Course_Credit_S 欄位名稱常數定義
			/// </summary>
			public const string CourseCreditS = "Course_Credit_S";

			/// <summary>
			/// Course_Credit_L 欄位名稱常數定義
			/// </summary>
			public const string CourseCreditL = "Course_Credit_L";

			/// <summary>
			/// Credit_No_S 欄位名稱常數定義
			/// </summary>
			public const string CreditNoS = "Credit_No_S";

			/// <summary>
			/// Credit_No_L 欄位名稱常數定義
			/// </summary>
			public const string CreditNoL = "Credit_No_L";

			/// <summary>
			/// Course_Cprice_S 欄位名稱常數定義
			/// </summary>
			public const string CourseCpriceS = "Course_Cprice_S";

			/// <summary>
			/// Course_Cprice_L 欄位名稱常數定義
			/// </summary>
			public const string CourseCpriceL = "Course_Cprice_L";

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
		/// MappingcsTxtEntity 類別建構式
		/// </summary>
		public MappingcsTxtEntity()
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
		/// Course_Id_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseIdS, false, FieldTypeEnum.Integer, true)]
		public int? CourseIdS
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseIdL, false, FieldTypeEnum.Integer, true)]
		public int? CourseIdL
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Name_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseNameS, false, FieldTypeEnum.Integer, true)]
		public int? CourseNameS
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Name_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseNameL, false, FieldTypeEnum.Integer, true)]
		public int? CourseNameL
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Credit_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseCreditS, false, FieldTypeEnum.Integer, true)]
		public int? CourseCreditS
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Credit_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseCreditL, false, FieldTypeEnum.Integer, true)]
		public int? CourseCreditL
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_No_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditNoS, false, FieldTypeEnum.Integer, true)]
		public int? CreditNoS
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_No_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditNoL, false, FieldTypeEnum.Integer, true)]
		public int? CreditNoL
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Cprice_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseCpriceS, false, FieldTypeEnum.Integer, true)]
		public int? CourseCpriceS
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Cprice_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseCpriceL, false, FieldTypeEnum.Integer, true)]
		public int? CourseCpriceL
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
        internal TxtMapField[] GetMapFields()
        {
            List<TxtMapField> mapFields = new List<TxtMapField>();

            #region 課程收費標準對照欄位 (StudentMasterEntity)
            {
                if (this.CourseIdS  != null && this.CourseIdL  != null)
                {
                    mapFields.Add(new TxtMapField(MappingcsXlsmdbEntity.Field.CourseId, this.CourseIdS.Value, this.CourseIdL.Value, new CodeChecker(1, 20)));
                }
                if (this.CourseNameS != null && this.CourseNameL  != null)
                {
					#region [MDY:20160430] 因為是中文所有改用 WordChecker
					#region [Old]
                    //mapFields.Add(new TxtMapField(MappingcsXlsmdbEntity.Field.CourseName, this.CourseNameS.Value, this.CourseNameL.Value, new CodeChecker(1, 20)));
					#endregion
					
					mapFields.Add(new TxtMapField(MappingcsXlsmdbEntity.Field.CourseName, this.CourseNameS.Value, this.CourseNameL.Value, new WordChecker(1, 20)));
					#endregion
                }
                if (this.CourseCreditS != null && this.CourseCreditL != null)
                {
                    mapFields.Add(new TxtMapField(MappingcsXlsmdbEntity.Field.CourseCredit, this.CourseCreditS.Value, this.CourseCreditL.Value, new NumberChecker()));
                }
                if (this.CreditNoS != null && this.CreditNoL != null)
                {
                    mapFields.Add(new TxtMapField(MappingcsXlsmdbEntity.Field.CreditNo, this.CreditNoS.Value, this.CreditNoL.Value, new CodeChecker(1, 8)));
                }
                if (this.CourseCpriceS != null && this.CourseCpriceL != null)
                {
                    mapFields.Add(new TxtMapField(MappingcsXlsmdbEntity.Field.CourseCprice, this.CourseCpriceS.Value, this.CourseCpriceL.Value, new NumberChecker()));
                }
            }
            #endregion

            return mapFields.ToArray();
        }
        #endregion
	}
}
