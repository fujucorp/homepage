/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:36:24
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
	/// MappingRt_Xlsmdb 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class MappingrtXlsmdbEntity : Entity
	{
		public const string TABLE_NAME = "MappingRt_Xlsmdb";

		#region Field Name Const Class
		/// <summary>
		/// MappingrtXlsmdbEntity 欄位名稱定義抽象類別
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
			/// Stu_Id 欄位名稱常數定義
			/// </summary>
			public const string StuId = "Stu_Id";

			/// <summary>
			/// RT_Name 欄位名稱常數定義
			/// </summary>
			public const string RtName = "RT_Name";

			/// <summary>
			/// Rt_Credit 欄位名稱常數定義
			/// </summary>
			public const string RtCredit = "Rt_Credit";

			/// <summary>
			/// Re_Credit 欄位名稱常數定義
			/// </summary>
			public const string ReCredit = "Re_Credit";

			/// <summary>
			/// Rt_Amount 欄位名稱常數定義
			/// </summary>
			public const string RtAmount = "Rt_Amount";

			/// <summary>
			/// Rt_Bank_Id 欄位名稱常數定義
			/// </summary>
			public const string RtBankId = "Rt_Bank_Id";

			/// <summary>
			/// Rt_Account 欄位名稱常數定義
			/// </summary>
			public const string RtAccount = "Rt_Account";

			/// <summary>
			/// Rt_01 欄位名稱常數定義
			/// </summary>
			public const string Rt01 = "Rt_01";

			/// <summary>
			/// Rt_02 欄位名稱常數定義
			/// </summary>
			public const string Rt02 = "Rt_02";

			/// <summary>
			/// Rt_03 欄位名稱常數定義
			/// </summary>
			public const string Rt03 = "Rt_03";

			/// <summary>
			/// Rt_04 欄位名稱常數定義
			/// </summary>
			public const string Rt04 = "Rt_04";

			/// <summary>
			/// Rt_05 欄位名稱常數定義
			/// </summary>
			public const string Rt05 = "Rt_05";

			/// <summary>
			/// Rt_06 欄位名稱常數定義
			/// </summary>
			public const string Rt06 = "Rt_06";

			/// <summary>
			/// Rt_07 欄位名稱常數定義
			/// </summary>
			public const string Rt07 = "Rt_07";

			/// <summary>
			/// Rt_08 欄位名稱常數定義
			/// </summary>
			public const string Rt08 = "Rt_08";

			/// <summary>
			/// Rt_09 欄位名稱常數定義
			/// </summary>
			public const string Rt09 = "Rt_09";

			/// <summary>
			/// Rt_10 欄位名稱常數定義
			/// </summary>
			public const string Rt10 = "Rt_10";

			/// <summary>
			/// Rt_11 欄位名稱常數定義
			/// </summary>
			public const string Rt11 = "Rt_11";

			/// <summary>
			/// Rt_12 欄位名稱常數定義
			/// </summary>
			public const string Rt12 = "Rt_12";

			/// <summary>
			/// Rt_13 欄位名稱常數定義
			/// </summary>
			public const string Rt13 = "Rt_13";

			/// <summary>
			/// Rt_14 欄位名稱常數定義
			/// </summary>
			public const string Rt14 = "Rt_14";

			/// <summary>
			/// Rt_15 欄位名稱常數定義
			/// </summary>
			public const string Rt15 = "Rt_15";

			/// <summary>
			/// Rt_16 欄位名稱常數定義
			/// </summary>
			public const string Rt16 = "Rt_16";

			/// <summary>
			/// Rt_17 欄位名稱常數定義
			/// </summary>
			public const string Rt17 = "Rt_17";

			/// <summary>
			/// Rt_18 欄位名稱常數定義
			/// </summary>
			public const string Rt18 = "Rt_18";

			/// <summary>
			/// Rt_19 欄位名稱常數定義
			/// </summary>
			public const string Rt19 = "Rt_19";

			/// <summary>
			/// Rt_20 欄位名稱常數定義
			/// </summary>
			public const string Rt20 = "Rt_20";

			/// <summary>
			/// Rt_21 欄位名稱常數定義
			/// </summary>
			public const string Rt21 = "Rt_21";

			/// <summary>
			/// Rt_22 欄位名稱常數定義
			/// </summary>
			public const string Rt22 = "Rt_22";

			/// <summary>
			/// Rt_23 欄位名稱常數定義
			/// </summary>
			public const string Rt23 = "Rt_23";

			/// <summary>
			/// Rt_24 欄位名稱常數定義
			/// </summary>
			public const string Rt24 = "Rt_24";

			/// <summary>
			/// Rt_25 欄位名稱常數定義
			/// </summary>
			public const string Rt25 = "Rt_25";

			/// <summary>
			/// Rt_26 欄位名稱常數定義
			/// </summary>
			public const string Rt26 = "Rt_26";

			/// <summary>
			/// Rt_27 欄位名稱常數定義
			/// </summary>
			public const string Rt27 = "Rt_27";

			/// <summary>
			/// Rt_28 欄位名稱常數定義
			/// </summary>
			public const string Rt28 = "Rt_28";

			/// <summary>
			/// Rt_29 欄位名稱常數定義
			/// </summary>
			public const string Rt29 = "Rt_29";

			/// <summary>
			/// Rt_30 欄位名稱常數定義
			/// </summary>
			public const string Rt30 = "Rt_30";

			/// <summary>
			/// Rt_31 欄位名稱常數定義
			/// </summary>
			public const string Rt31 = "Rt_31";

			/// <summary>
			/// Rt_32 欄位名稱常數定義
			/// </summary>
			public const string Rt32 = "Rt_32";

			/// <summary>
			/// Rt_33 欄位名稱常數定義
			/// </summary>
			public const string Rt33 = "Rt_33";

			/// <summary>
			/// Rt_34 欄位名稱常數定義
			/// </summary>
			public const string Rt34 = "Rt_34";

			/// <summary>
			/// Rt_35 欄位名稱常數定義
			/// </summary>
			public const string Rt35 = "Rt_35";

			/// <summary>
			/// Rt_36 欄位名稱常數定義
			/// </summary>
			public const string Rt36 = "Rt_36";

			/// <summary>
			/// Rt_37 欄位名稱常數定義
			/// </summary>
			public const string Rt37 = "Rt_37";

			/// <summary>
			/// Rt_38 欄位名稱常數定義
			/// </summary>
			public const string Rt38 = "Rt_38";

			/// <summary>
			/// Rt_39 欄位名稱常數定義
			/// </summary>
			public const string Rt39 = "Rt_39";

			/// <summary>
			/// Rt_40 欄位名稱常數定義
			/// </summary>
			public const string Rt40 = "Rt_40";

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
		/// MappingrtXlsmdbEntity 類別建構式
		/// </summary>
		public MappingrtXlsmdbEntity()
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
		/// Stu_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.StuId, false, FieldTypeEnum.VarChar, 20, true)]
		public string StuId
		{
			get;
			set;
		}

		/// <summary>
		/// RT_Name 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtName, false, FieldTypeEnum.VarChar, 20, true)]
		public string RtName
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Credit 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtCredit, false, FieldTypeEnum.VarChar, 20, true)]
		public string RtCredit
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Credit 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReCredit, false, FieldTypeEnum.VarChar, 20, true)]
		public string ReCredit
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Amount 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtAmount, false, FieldTypeEnum.VarChar, 20, true)]
		public string RtAmount
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Bank_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtBankId, false, FieldTypeEnum.VarChar, 20, true)]
		public string RtBankId
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_Account 欄位屬性
		/// </summary>
		[FieldSpec(Field.RtAccount, false, FieldTypeEnum.VarChar, 20, true)]
		public string RtAccount
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_01 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt01, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt01
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_02 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt02, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt02
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_03 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt03, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt03
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_04 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt04, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt04
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_05 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt05, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt05
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_06 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt06, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt06
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_07 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt07, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt07
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_08 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt08, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt08
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_09 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt09, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt09
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt10, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt10
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt11, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt11
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt12, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt12
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt13, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt13
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt14, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt14
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt15, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt15
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt16, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt16
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_17 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt17, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt17
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_18 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt18, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt18
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_19 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt19, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt19
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_20 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt20, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt20
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_21 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt21, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt21
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_22 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt22, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt22
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_23 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt23, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt23
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_24 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt24, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt24
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_25 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt25, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt25
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_26 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt26, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt26
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_27 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt27, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt27
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_28 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt28, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt28
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_29 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt29, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt29
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_30 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt30, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt30
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_31 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt31, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt31
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_32 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt32, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt32
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_33 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt33, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt33
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_34 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt34, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt34
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_35 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt35, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt35
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_36 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt36, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt36
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_37 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt37, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt37
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_38 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt38, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt38
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_39 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt39, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt39
		{
			get;
			set;
		}

		/// <summary>
		/// Rt_40 欄位屬性
		/// </summary>
		[FieldSpec(Field.Rt40, false, FieldTypeEnum.VarChar, 20, true)]
		public string Rt40
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

            if (!String.IsNullOrWhiteSpace(this.StuId))
            {
                mapFields.Add(new XlsMapField(Field.StuId, this.StuId, new CodeChecker(1, 20)));
            }

            if (!String.IsNullOrWhiteSpace(this.RtName))
            {
                mapFields.Add(new XlsMapField(Field.RtName, this.RtName, new CodeChecker(1, 20)));
            }

            if (!String.IsNullOrWhiteSpace(this.RtCredit))
            {
                mapFields.Add(new XlsMapField(Field.RtCredit, this.RtCredit, new DecimalChecker(0M, 999.99M)));
            }

            if (!String.IsNullOrWhiteSpace(this.ReCredit))
            {
                mapFields.Add(new XlsMapField(Field.ReCredit, this.ReCredit, new DecimalChecker(0M, 999.99M)));
            }

            if (!String.IsNullOrWhiteSpace(this.RtAmount))
            {
                mapFields.Add(new XlsMapField(Field.RtAmount, this.RtAmount, new DecimalChecker(0M, 999999999M)));
            }

            if (!String.IsNullOrWhiteSpace(this.RtBankId))
            {
                mapFields.Add(new XlsMapField(Field.RtBankId, this.RtBankId, null));
            }

            if (!String.IsNullOrWhiteSpace(this.RtAccount))
            {
                mapFields.Add(new XlsMapField(Field.RtAccount, this.RtAccount, new NumberChecker(0, 21, null)));
            }

            #region 代收科目退費金額對照欄位
            {
                string[] fields = new string[] {
                    Field.Rt01, Field.Rt02, Field.Rt03, Field.Rt04, Field.Rt05,
                    Field.Rt06, Field.Rt07, Field.Rt08, Field.Rt09, Field.Rt10,
                    Field.Rt11, Field.Rt12, Field.Rt13, Field.Rt14, Field.Rt15,
                    Field.Rt16, Field.Rt17, Field.Rt18, Field.Rt19, Field.Rt20,
                    Field.Rt21, Field.Rt22, Field.Rt23, Field.Rt24, Field.Rt25,
                    Field.Rt26, Field.Rt27, Field.Rt28, Field.Rt29, Field.Rt30,
                    Field.Rt31, Field.Rt32, Field.Rt33, Field.Rt34, Field.Rt35,
                    Field.Rt36, Field.Rt37, Field.Rt38, Field.Rt39, Field.Rt40
                };
                string[] values = new string[] {
                    this.Rt01, this.Rt02, this.Rt03, this.Rt04, this.Rt05,
                    this.Rt06, this.Rt07, this.Rt08, this.Rt09, this.Rt10,
                    this.Rt11, this.Rt12, this.Rt13, this.Rt14, this.Rt15,
                    this.Rt16, this.Rt17, this.Rt18, this.Rt19, this.Rt20,
                    this.Rt21, this.Rt22, this.Rt23, this.Rt24, this.Rt25,
                    this.Rt26, this.Rt27, this.Rt28, this.Rt29, this.Rt30,
                    this.Rt31, this.Rt32, this.Rt33, this.Rt34, this.Rt35,
                    this.Rt36, this.Rt37, this.Rt38, this.Rt39, this.Rt40
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
