/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:32:15
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
	/// Student_Return 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class StudentReturnEntity : Entity
	{
		public const string TABLE_NAME = "Student_Return";

		#region Field Name Const Class
		/// <summary>
		/// StudentReturnEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
            /// <summary>
            /// Data_No 欄位名稱常數定義 (Identity)
            /// </summary>
            public const string DataNo = "Data_No";

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
			/// SR_No 欄位名稱常數定義
			/// </summary>
			public const string SrNo = "SR_No";

			/// <summary>
			/// Cancel_No 欄位名稱常數定義
			/// </summary>
			public const string CancelNo = "Cancel_No";

			/// <summary>
			/// Return_Date 欄位名稱常數定義
			/// </summary>
			public const string ReturnDate = "Return_Date";

			/// <summary>
			/// Return_Way 欄位名稱常數定義
			/// </summary>
			public const string ReturnWay = "Return_Way";

			/// <summary>
			/// Return_Remark 欄位名稱常數定義
			/// </summary>
			public const string ReturnRemark = "Return_Remark";

			/// <summary>
			/// Return_01 欄位名稱常數定義
			/// </summary>
			public const string Return01 = "Return_01";

			/// <summary>
			/// Return_02 欄位名稱常數定義
			/// </summary>
			public const string Return02 = "Return_02";

			/// <summary>
			/// Return_03 欄位名稱常數定義
			/// </summary>
			public const string Return03 = "Return_03";

			/// <summary>
			/// Return_04 欄位名稱常數定義
			/// </summary>
			public const string Return04 = "Return_04";

			/// <summary>
			/// Return_05 欄位名稱常數定義
			/// </summary>
			public const string Return05 = "Return_05";

			/// <summary>
			/// Return_06 欄位名稱常數定義
			/// </summary>
			public const string Return06 = "Return_06";

			/// <summary>
			/// Return_07 欄位名稱常數定義
			/// </summary>
			public const string Return07 = "Return_07";

			/// <summary>
			/// Return_08 欄位名稱常數定義
			/// </summary>
			public const string Return08 = "Return_08";

			/// <summary>
			/// Return_09 欄位名稱常數定義
			/// </summary>
			public const string Return09 = "Return_09";

			/// <summary>
			/// Return_10 欄位名稱常數定義
			/// </summary>
			public const string Return10 = "Return_10";

			/// <summary>
			/// Return_11 欄位名稱常數定義
			/// </summary>
			public const string Return11 = "Return_11";

			/// <summary>
			/// Return_12 欄位名稱常數定義
			/// </summary>
			public const string Return12 = "Return_12";

			/// <summary>
			/// Return_13 欄位名稱常數定義
			/// </summary>
			public const string Return13 = "Return_13";

			/// <summary>
			/// Return_14 欄位名稱常數定義
			/// </summary>
			public const string Return14 = "Return_14";

			/// <summary>
			/// Return_15 欄位名稱常數定義
			/// </summary>
			public const string Return15 = "Return_15";

			/// <summary>
			/// Return_16 欄位名稱常數定義
			/// </summary>
			public const string Return16 = "Return_16";

			/// <summary>
			/// Return_17 欄位名稱常數定義
			/// </summary>
			public const string Return17 = "Return_17";

			/// <summary>
			/// Return_18 欄位名稱常數定義
			/// </summary>
			public const string Return18 = "Return_18";

			/// <summary>
			/// Return_19 欄位名稱常數定義
			/// </summary>
			public const string Return19 = "Return_19";

			/// <summary>
			/// Return_20 欄位名稱常數定義
			/// </summary>
			public const string Return20 = "Return_20";

			/// <summary>
			/// Return_21 欄位名稱常數定義
			/// </summary>
			public const string Return21 = "Return_21";

			/// <summary>
			/// Return_22 欄位名稱常數定義
			/// </summary>
			public const string Return22 = "Return_22";

			/// <summary>
			/// Return_23 欄位名稱常數定義
			/// </summary>
			public const string Return23 = "Return_23";

			/// <summary>
			/// Return_24 欄位名稱常數定義
			/// </summary>
			public const string Return24 = "Return_24";

			/// <summary>
			/// Return_25 欄位名稱常數定義
			/// </summary>
			public const string Return25 = "Return_25";

			/// <summary>
			/// Return_26 欄位名稱常數定義
			/// </summary>
			public const string Return26 = "Return_26";

			/// <summary>
			/// Return_27 欄位名稱常數定義
			/// </summary>
			public const string Return27 = "Return_27";

			/// <summary>
			/// Return_28 欄位名稱常數定義
			/// </summary>
			public const string Return28 = "Return_28";

			/// <summary>
			/// Return_29 欄位名稱常數定義
			/// </summary>
			public const string Return29 = "Return_29";

			/// <summary>
			/// Return_30 欄位名稱常數定義
			/// </summary>
			public const string Return30 = "Return_30";

			/// <summary>
			/// Return_31 欄位名稱常數定義
			/// </summary>
			public const string Return31 = "Return_31";

			/// <summary>
			/// Return_32 欄位名稱常數定義
			/// </summary>
			public const string Return32 = "Return_32";

			/// <summary>
			/// Return_33 欄位名稱常數定義
			/// </summary>
			public const string Return33 = "Return_33";

			/// <summary>
			/// Return_34 欄位名稱常數定義
			/// </summary>
			public const string Return34 = "Return_34";

			/// <summary>
			/// Return_35 欄位名稱常數定義
			/// </summary>
			public const string Return35 = "Return_35";

			/// <summary>
			/// Return_36 欄位名稱常數定義
			/// </summary>
			public const string Return36 = "Return_36";

			/// <summary>
			/// Return_37 欄位名稱常數定義
			/// </summary>
			public const string Return37 = "Return_37";

			/// <summary>
			/// Return_38 欄位名稱常數定義
			/// </summary>
			public const string Return38 = "Return_38";

			/// <summary>
			/// Return_39 欄位名稱常數定義
			/// </summary>
			public const string Return39 = "Return_39";

			/// <summary>
			/// Return_40 欄位名稱常數定義
			/// </summary>
			public const string Return40 = "Return_40";

			/// <summary>
			/// Return_Amount 欄位名稱常數定義
			/// </summary>
			public const string ReturnAmount = "Return_Amount";

			/// <summary>
			/// Remit_Data 欄位名稱常數定義
			/// </summary>
			public const string RemitData = "Remit_Data";

			/// <summary>
			/// Check_Way 欄位名稱常數定義
			/// </summary>
			public const string CheckWay = "Check_Way";

			/// <summary>
			/// Return_Id 欄位名稱常數定義
			/// </summary>
			public const string ReturnId = "Return_Id";

			/// <summary>
			/// Return_Credit 欄位名稱常數定義
			/// </summary>
			public const string ReturnCredit = "Return_Credit";

			/// <summary>
			/// Real_Credit 欄位名稱常數定義
			/// </summary>
			public const string RealCredit = "Real_Credit";

			/// <summary>
			/// Re_Seq 欄位名稱常數定義
			/// </summary>
			public const string ReSeq = "Re_Seq";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// StudentReturnEntity 類別建構式
		/// </summary>
		public StudentReturnEntity()
			: base()
		{
		}
		#endregion

		#region Property
        /// <summary>
        /// Data_No 欄位屬性 (Identity)
        /// </summary>
        [FieldSpec(Field.DataNo, false, FieldTypeEnum.Identity, false)]
        public Int64 DataNo
        {
            get;
            set;
        }

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
		/// SR_No 欄位屬性
		/// </summary>
		[FieldSpec(Field.SrNo, false, FieldTypeEnum.VarChar, 3, false)]
		public string SrNo
		{
			get;
			set;
		}

		/// <summary>
		/// Cancel_No 欄位屬性
		/// </summary>
		[FieldSpec(Field.CancelNo, false, FieldTypeEnum.VarChar, 16, true)]
		public string CancelNo
		{
			get;
			set;
		}

		/// <summary>
		/// Return_Date 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReturnDate, false, FieldTypeEnum.Char, 7, false)]
		public string ReturnDate
		{
			get;
			set;
		}

		/// <summary>
		/// Return_Way 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReturnWay, false, FieldTypeEnum.Char, 1, true)]
		public string ReturnWay
		{
			get;
			set;
		}

		/// <summary>
		/// Return_Remark 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReturnRemark, false, FieldTypeEnum.Char, 1, true)]
		public string ReturnRemark
		{
			get;
			set;
		}

		/// <summary>
		/// Return_01 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return01, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return01
		{
			get;
			set;
		}

		/// <summary>
		/// Return_02 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return02, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return02
		{
			get;
			set;
		}

		/// <summary>
		/// Return_03 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return03, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return03
		{
			get;
			set;
		}

		/// <summary>
		/// Return_04 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return04, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return04
		{
			get;
			set;
		}

		/// <summary>
		/// Return_05 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return05, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return05
		{
			get;
			set;
		}

		/// <summary>
		/// Return_06 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return06, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return06
		{
			get;
			set;
		}

		/// <summary>
		/// Return_07 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return07, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return07
		{
			get;
			set;
		}

		/// <summary>
		/// Return_08 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return08, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return08
		{
			get;
			set;
		}

		/// <summary>
		/// Return_09 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return09, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return09
		{
			get;
			set;
		}

		/// <summary>
		/// Return_10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return10, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return10
		{
			get;
			set;
		}

		/// <summary>
		/// Return_11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return11, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return11
		{
			get;
			set;
		}

		/// <summary>
		/// Return_12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return12, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return12
		{
			get;
			set;
		}

		/// <summary>
		/// Return_13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return13, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return13
		{
			get;
			set;
		}

		/// <summary>
		/// Return_14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return14, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return14
		{
			get;
			set;
		}

		/// <summary>
		/// Return_15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return15, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return15
		{
			get;
			set;
		}

		/// <summary>
		/// Return_16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return16, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return16
		{
			get;
			set;
		}

		/// <summary>
		/// Return_17 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return17, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return17
		{
			get;
			set;
		}

		/// <summary>
		/// Return_18 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return18, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return18
		{
			get;
			set;
		}

		/// <summary>
		/// Return_19 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return19, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return19
		{
			get;
			set;
		}

		/// <summary>
		/// Return_20 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return20, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return20
		{
			get;
			set;
		}

		/// <summary>
		/// Return_21 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return21, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return21
		{
			get;
			set;
		}

		/// <summary>
		/// Return_22 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return22, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return22
		{
			get;
			set;
		}

		/// <summary>
		/// Return_23 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return23, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return23
		{
			get;
			set;
		}

		/// <summary>
		/// Return_24 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return24, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return24
		{
			get;
			set;
		}

		/// <summary>
		/// Return_25 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return25, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return25
		{
			get;
			set;
		}

		/// <summary>
		/// Return_26 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return26, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return26
		{
			get;
			set;
		}

		/// <summary>
		/// Return_27 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return27, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return27
		{
			get;
			set;
		}

		/// <summary>
		/// Return_28 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return28, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return28
		{
			get;
			set;
		}

		/// <summary>
		/// Return_29 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return29, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return29
		{
			get;
			set;
		}

		/// <summary>
		/// Return_30 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return30, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return30
		{
			get;
			set;
		}

		/// <summary>
		/// Return_31 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return31, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return31
		{
			get;
			set;
		}

		/// <summary>
		/// Return_32 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return32, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return32
		{
			get;
			set;
		}

		/// <summary>
		/// Return_33 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return33, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return33
		{
			get;
			set;
		}

		/// <summary>
		/// Return_34 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return34, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return34
		{
			get;
			set;
		}

		/// <summary>
		/// Return_35 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return35, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return35
		{
			get;
			set;
		}

		/// <summary>
		/// Return_36 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return36, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return36
		{
			get;
			set;
		}

		/// <summary>
		/// Return_37 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return37, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return37
		{
			get;
			set;
		}

		/// <summary>
		/// Return_38 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return38, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return38
		{
			get;
			set;
		}

		/// <summary>
		/// Return_39 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return39, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return39
		{
			get;
			set;
		}

		/// <summary>
		/// Return_40 欄位屬性
		/// </summary>
		[FieldSpec(Field.Return40, false, FieldTypeEnum.Decimal, true)]
		public decimal? Return40
		{
			get;
			set;
		}

		/// <summary>
		/// Return_Amount 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReturnAmount, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReturnAmount
		{
			get;
			set;
		}

		/// <summary>
		/// Remit_Data 欄位屬性
		/// </summary>
		[FieldSpec(Field.RemitData, false, FieldTypeEnum.VarChar, 21, true)]
		public string RemitData
		{
			get;
			set;
		}

		/// <summary>
		/// Check_Way 欄位屬性
		/// </summary>
		[FieldSpec(Field.CheckWay, false, FieldTypeEnum.Char, 1, true)]
		public string CheckWay
		{
			get;
			set;
		}

		/// <summary>
		/// Return_Id 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReturnId, false, FieldTypeEnum.VarChar, 8, true)]
		public string ReturnId
		{
			get;
			set;
		}

		/// <summary>
		/// Return_Credit 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReturnCredit, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReturnCredit
		{
			get;
			set;
		}

		/// <summary>
		/// Real_Credit 欄位屬性
		/// </summary>
		[FieldSpec(Field.RealCredit, false, FieldTypeEnum.Decimal, true)]
		public decimal? RealCredit
		{
			get;
			set;
		}

		/// <summary>
		/// Re_Seq 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReSeq, false, FieldTypeEnum.VarChar, 10, true)]
		public string ReSeq
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
