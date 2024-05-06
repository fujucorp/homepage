/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:36:09
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
	/// MappingRR_Xlsmdb 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class MappingrrXlsmdbEntity : Entity
	{
		public const string TABLE_NAME = "MappingRR_Xlsmdb";

		#region Field Name Const Class
		/// <summary>
		/// MappingrrXlsmdbEntity 欄位名稱定義抽象類別
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
			/// S_Id 欄位名稱常數定義
			/// </summary>
			public const string SId = "S_Id";

			/// <summary>
			/// RR_Name 欄位名稱常數定義
			/// </summary>
			public const string RrName = "RR_Name";

			/// <summary>
			/// RR_Count 欄位名稱常數定義
			/// </summary>
			public const string RrCount = "RR_Count";

			/// <summary>
			/// RR_1 欄位名稱常數定義
			/// </summary>
			public const string Rr1 = "RR_1";

			/// <summary>
			/// RR_2 欄位名稱常數定義
			/// </summary>
			public const string Rr2 = "RR_2";

			/// <summary>
			/// RR_3 欄位名稱常數定義
			/// </summary>
			public const string Rr3 = "RR_3";

			/// <summary>
			/// RR_4 欄位名稱常數定義
			/// </summary>
			public const string Rr4 = "RR_4";

			/// <summary>
			/// RR_5 欄位名稱常數定義
			/// </summary>
			public const string Rr5 = "RR_5";

			/// <summary>
			/// RR_6 欄位名稱常數定義
			/// </summary>
			public const string Rr6 = "RR_6";

			/// <summary>
			/// RR_7 欄位名稱常數定義
			/// </summary>
			public const string Rr7 = "RR_7";

			/// <summary>
			/// RR_8 欄位名稱常數定義
			/// </summary>
			public const string Rr8 = "RR_8";

			/// <summary>
			/// RR_9 欄位名稱常數定義
			/// </summary>
			public const string Rr9 = "RR_9";

			/// <summary>
			/// RR_10 欄位名稱常數定義
			/// </summary>
			public const string Rr10 = "RR_10";

			/// <summary>
			/// RR_11 欄位名稱常數定義
			/// </summary>
			public const string Rr11 = "RR_11";

			/// <summary>
			/// RR_12 欄位名稱常數定義
			/// </summary>
			public const string Rr12 = "RR_12";

			/// <summary>
			/// RR_13 欄位名稱常數定義
			/// </summary>
			public const string Rr13 = "RR_13";

			/// <summary>
			/// RR_14 欄位名稱常數定義
			/// </summary>
			public const string Rr14 = "RR_14";

			/// <summary>
			/// RR_15 欄位名稱常數定義
			/// </summary>
			public const string Rr15 = "RR_15";

			/// <summary>
			/// RR_16 欄位名稱常數定義
			/// </summary>
			public const string Rr16 = "RR_16";

			/// <summary>
			/// RR_17 欄位名稱常數定義
			/// </summary>
			public const string Rr17 = "RR_17";

			/// <summary>
			/// RR_18 欄位名稱常數定義
			/// </summary>
			public const string Rr18 = "RR_18";

			/// <summary>
			/// RR_19 欄位名稱常數定義
			/// </summary>
			public const string Rr19 = "RR_19";

			/// <summary>
			/// RR_20 欄位名稱常數定義
			/// </summary>
			public const string Rr20 = "RR_20";

			/// <summary>
			/// RR_21 欄位名稱常數定義
			/// </summary>
			public const string Rr21 = "RR_21";

			/// <summary>
			/// RR_22 欄位名稱常數定義
			/// </summary>
			public const string Rr22 = "RR_22";

			/// <summary>
			/// RR_23 欄位名稱常數定義
			/// </summary>
			public const string Rr23 = "RR_23";

			/// <summary>
			/// RR_24 欄位名稱常數定義
			/// </summary>
			public const string Rr24 = "RR_24";

			/// <summary>
			/// RR_25 欄位名稱常數定義
			/// </summary>
			public const string Rr25 = "RR_25";

			/// <summary>
			/// RR_26 欄位名稱常數定義
			/// </summary>
			public const string Rr26 = "RR_26";

			/// <summary>
			/// RR_27 欄位名稱常數定義
			/// </summary>
			public const string Rr27 = "RR_27";

			/// <summary>
			/// RR_28 欄位名稱常數定義
			/// </summary>
			public const string Rr28 = "RR_28";

			/// <summary>
			/// RR_29 欄位名稱常數定義
			/// </summary>
			public const string Rr29 = "RR_29";

			/// <summary>
			/// RR_30 欄位名稱常數定義
			/// </summary>
			public const string Rr30 = "RR_30";

			/// <summary>
			/// RR_31 欄位名稱常數定義
			/// </summary>
			public const string Rr31 = "RR_31";

			/// <summary>
			/// RR_32 欄位名稱常數定義
			/// </summary>
			public const string Rr32 = "RR_32";

			/// <summary>
			/// RR_33 欄位名稱常數定義
			/// </summary>
			public const string Rr33 = "RR_33";

			/// <summary>
			/// RR_34 欄位名稱常數定義
			/// </summary>
			public const string Rr34 = "RR_34";

			/// <summary>
			/// RR_35 欄位名稱常數定義
			/// </summary>
			public const string Rr35 = "RR_35";

			/// <summary>
			/// RR_36 欄位名稱常數定義
			/// </summary>
			public const string Rr36 = "RR_36";

			/// <summary>
			/// RR_37 欄位名稱常數定義
			/// </summary>
			public const string Rr37 = "RR_37";

			/// <summary>
			/// RR_38 欄位名稱常數定義
			/// </summary>
			public const string Rr38 = "RR_38";

			/// <summary>
			/// RR_39 欄位名稱常數定義
			/// </summary>
			public const string Rr39 = "RR_39";

			/// <summary>
			/// RR_40 欄位名稱常數定義
			/// </summary>
			public const string Rr40 = "RR_40";

			/// <summary>
			/// Amount 欄位名稱常數定義
			/// </summary>
			public const string Amount = "Amount";

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
		/// MappingrrXlsmdbEntity 類別建構式
		/// </summary>
		public MappingrrXlsmdbEntity()
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
		/// S_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.SId, false, FieldTypeEnum.VarChar, 20, true)]
		public string SId
		{
			get;
			set;
		}

		/// <summary>
		/// RR_Name 欄位屬性
		/// </summary>
		[FieldSpec(Field.RrName, false, FieldTypeEnum.VarChar, 20, true)]
		public string RrName
		{
			get;
			set;
		}

		/// <summary>
		/// RR_Count 欄位屬性
		/// </summary>
		[FieldSpec(Field.RrCount, false, FieldTypeEnum.VarChar, 20, true)]
		public string RrCount
		{
			get;
			set;
		}

		/// <summary>
		/// RR_1 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr1, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr1
		{
			get;
			set;
		}

		/// <summary>
		/// RR_2 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr2, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr2
		{
			get;
			set;
		}

		/// <summary>
		/// RR_3 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr3, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr3
		{
			get;
			set;
		}

		/// <summary>
		/// RR_4 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr4, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr4
		{
			get;
			set;
		}

		/// <summary>
		/// RR_5 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr5, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr5
		{
			get;
			set;
		}

		/// <summary>
		/// RR_6 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr6, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr6
		{
			get;
			set;
		}

		/// <summary>
		/// RR_7 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr7, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr7
		{
			get;
			set;
		}

		/// <summary>
		/// RR_8 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr8, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr8
		{
			get;
			set;
		}

		/// <summary>
		/// RR_9 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr9, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr9
		{
			get;
			set;
		}

		/// <summary>
		/// RR_10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr10, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr10
		{
			get;
			set;
		}

		/// <summary>
		/// RR_11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr11, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr11
		{
			get;
			set;
		}

		/// <summary>
		/// RR_12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr12, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr12
		{
			get;
			set;
		}

		/// <summary>
		/// RR_13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr13, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr13
		{
			get;
			set;
		}

		/// <summary>
		/// RR_14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr14, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr14
		{
			get;
			set;
		}

		/// <summary>
		/// RR_15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr15, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr15
		{
			get;
			set;
		}

		/// <summary>
		/// RR_16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr16, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr16
		{
			get;
			set;
		}

		/// <summary>
		/// RR_17 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr17, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr17
		{
			get;
			set;
		}

		/// <summary>
		/// RR_18 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr18, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr18
		{
			get;
			set;
		}

		/// <summary>
		/// RR_19 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr19, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr19
		{
			get;
			set;
		}

		/// <summary>
		/// RR_20 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr20, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr20
		{
			get;
			set;
		}

		/// <summary>
		/// RR_21 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr21, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr21
		{
			get;
			set;
		}

		/// <summary>
		/// RR_22 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr22, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr22
		{
			get;
			set;
		}

		/// <summary>
		/// RR_23 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr23, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr23
		{
			get;
			set;
		}

		/// <summary>
		/// RR_24 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr24, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr24
		{
			get;
			set;
		}

		/// <summary>
		/// RR_25 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr25, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr25
		{
			get;
			set;
		}

		/// <summary>
		/// RR_26 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr26, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr26
		{
			get;
			set;
		}

		/// <summary>
		/// RR_27 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr27, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr27
		{
			get;
			set;
		}

		/// <summary>
		/// RR_28 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr28, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr28
		{
			get;
			set;
		}

		/// <summary>
		/// RR_29 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr29, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr29
		{
			get;
			set;
		}

		/// <summary>
		/// RR_30 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr30, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr30
		{
			get;
			set;
		}

		/// <summary>
		/// RR_31 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr31, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr31
		{
			get;
			set;
		}

		/// <summary>
		/// RR_32 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr32, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr32
		{
			get;
			set;
		}

		/// <summary>
		/// RR_33 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr33, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr33
		{
			get;
			set;
		}

		/// <summary>
		/// RR_34 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr34, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr34
		{
			get;
			set;
		}

		/// <summary>
		/// RR_35 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr35, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr35
		{
			get;
			set;
		}

		/// <summary>
		/// RR_36 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr36, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr36
		{
			get;
			set;
		}

		/// <summary>
		/// RR_37 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr37, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr37
		{
			get;
			set;
		}

		/// <summary>
		/// RR_38 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr38, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr38
		{
			get;
			set;
		}

		/// <summary>
		/// RR_39 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr39, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr39
		{
			get;
			set;
		}

		/// <summary>
		/// RR_40 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rr40, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rr40
		{
			get;
			set;
		}

		/// <summary>
		/// Amount 欄位屬性
		/// </summary>
		[FieldSpec(Field.Amount, false, FieldTypeEnum.VarChar, 20, true)]
		public string Amount
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

            if (!String.IsNullOrWhiteSpace(this.SId))
            {
                mapFields.Add(new XlsMapField(Field.SId, this.SId, new CodeChecker(1, 20)));
            }

            if (!String.IsNullOrWhiteSpace(this.RrName))
            {
                mapFields.Add(new XlsMapField(Field.RrName, this.RrName, new CodeChecker(1, 20)));
            }

            if (!String.IsNullOrWhiteSpace(this.RrCount))
            {
                mapFields.Add(new XlsMapField(Field.RrCount, this.RrCount, new WordChecker(1, 20)));
            }

            if (!String.IsNullOrWhiteSpace(this.Amount))
            {
                mapFields.Add(new XlsMapField(Field.Amount, this.Amount, new DecimalChecker(0M, 999999999M)));
            }

            #region 代收科目減免金額對照欄位
            {
                string[] fields = new string[] {
                    Field.Rr1, Field.Rr2, Field.Rr3, Field.Rr4, Field.Rr5,
                    Field.Rr6, Field.Rr7, Field.Rr8, Field.Rr9, Field.Rr10,
                    Field.Rr11, Field.Rr12, Field.Rr13, Field.Rr14, Field.Rr15,
                    Field.Rr16, Field.Rr17, Field.Rr18, Field.Rr19, Field.Rr20,
                    Field.Rr21, Field.Rr22, Field.Rr23, Field.Rr24, Field.Rr25,
                    Field.Rr26, Field.Rr27, Field.Rr28, Field.Rr29, Field.Rr30,
                    Field.Rr31, Field.Rr32, Field.Rr33, Field.Rr34, Field.Rr35,
                    Field.Rr36, Field.Rr37, Field.Rr38, Field.Rr39, Field.Rr40
                };
                string[] values = new string[] {
                    this.Rr1, this.Rr2, this.Rr3, this.Rr4, this.Rr5,
                    this.Rr6, this.Rr7, this.Rr8, this.Rr9, this.Rr10,
                    this.Rr11, this.Rr12, this.Rr13, this.Rr14, this.Rr15,
                    this.Rr16, this.Rr17, this.Rr18, this.Rr19, this.Rr20,
                    this.Rr21, this.Rr22, this.Rr23, this.Rr24, this.Rr25,
                    this.Rr26, this.Rr27, this.Rr28, this.Rr29, this.Rr30,
                    this.Rr31, this.Rr32, this.Rr33, this.Rr34, this.Rr35,
                    this.Rr36, this.Rr37, this.Rr38, this.Rr39, this.Rr40
                };
                for (int idx = 0; idx < values.Length; idx++)
                {
                    if (!String.IsNullOrWhiteSpace(values[idx]))
                    {
                        mapFields.Add(new XlsMapField(fields[idx], values[idx], new DecimalChecker(0M, 999999999M)));
                    }
                }
            }
            #endregion

            return mapFields.ToArray();
        }
        #endregion
	}
}
