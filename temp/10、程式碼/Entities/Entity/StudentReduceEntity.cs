/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2016/11/06 10:05:28
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
	/// Student_Reduce 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class StudentReduceEntity : Entity
	{
		public const string TABLE_NAME = "Student_Reduce";

		#region Field Name Const Class
		/// <summary>
		/// StudentReduceEntity 欄位名稱定義抽象類別
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

			/// <summary>
			/// Reduce_ID 欄位名稱常數定義
			/// </summary>
			public const string ReduceId = "Reduce_Id";
			#endregion

			#region Data
			/// <summary>
			/// Reduce_01 欄位名稱常數定義
			/// </summary>
			public const string Reduce01 = "Reduce_01";

			/// <summary>
			/// Reduce_02 欄位名稱常數定義
			/// </summary>
			public const string Reduce02 = "Reduce_02";

			/// <summary>
			/// Reduce_03 欄位名稱常數定義
			/// </summary>
			public const string Reduce03 = "Reduce_03";

			/// <summary>
			/// Reduce_04 欄位名稱常數定義
			/// </summary>
			public const string Reduce04 = "Reduce_04";

			/// <summary>
			/// Reduce_05 欄位名稱常數定義
			/// </summary>
			public const string Reduce05 = "Reduce_05";

			/// <summary>
			/// Reduce_06 欄位名稱常數定義
			/// </summary>
			public const string Reduce06 = "Reduce_06";

			/// <summary>
			/// Reduce_07 欄位名稱常數定義
			/// </summary>
			public const string Reduce07 = "Reduce_07";

			/// <summary>
			/// Reduce_08 欄位名稱常數定義
			/// </summary>
			public const string Reduce08 = "Reduce_08";

			/// <summary>
			/// Reduce_09 欄位名稱常數定義
			/// </summary>
			public const string Reduce09 = "Reduce_09";

			/// <summary>
			/// Reduce_10 欄位名稱常數定義
			/// </summary>
			public const string Reduce10 = "Reduce_10";

			/// <summary>
			/// Reduce_11 欄位名稱常數定義
			/// </summary>
			public const string Reduce11 = "Reduce_11";

			/// <summary>
			/// Reduce_12 欄位名稱常數定義
			/// </summary>
			public const string Reduce12 = "Reduce_12";

			/// <summary>
			/// Reduce_13 欄位名稱常數定義
			/// </summary>
			public const string Reduce13 = "Reduce_13";

			/// <summary>
			/// Reduce_14 欄位名稱常數定義
			/// </summary>
			public const string Reduce14 = "Reduce_14";

			/// <summary>
			/// Reduce_15 欄位名稱常數定義
			/// </summary>
			public const string Reduce15 = "Reduce_15";

			/// <summary>
			/// Reduce_16 欄位名稱常數定義
			/// </summary>
			public const string Reduce16 = "Reduce_16";

            public const string Reduce17 = "Reduce_17";

            public const string Reduce18 = "Reduce_18";

            public const string Reduce19 = "Reduce_19";

            public const string Reduce20 = "Reduce_20";

            public const string Reduce21 = "Reduce_21";

            public const string Reduce22 = "Reduce_22";

            public const string Reduce23 = "Reduce_23";

            public const string Reduce24 = "Reduce_24";

            public const string Reduce25 = "Reduce_25";

            public const string Reduce26 = "Reduce_26";

            public const string Reduce27 = "Reduce_27";

            public const string Reduce28 = "Reduce_28";

            public const string Reduce29 = "Reduce_29";

            public const string Reduce30 = "Reduce_30";

            public const string Reduce31 = "Reduce_31";

            public const string Reduce32 = "Reduce_32";

            public const string Reduce33 = "Reduce_33";

            public const string Reduce34 = "Reduce_34";

            public const string Reduce35 = "Reduce_35";

            public const string Reduce36 = "Reduce_36";

            public const string Reduce37 = "Reduce_37";

            public const string Reduce38 = "Reduce_38";

            public const string Reduce39 = "Reduce_39";

            public const string Reduce40 = "Reduce_40";

			/// <summary>
			/// Reduce_Amount 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount = "Reduce_Amount";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// StudentReduceEntity 類別建構式
		/// </summary>
		public StudentReduceEntity()
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
		[FieldSpec(Field.ReceiveId, true, FieldTypeEnum.Char, 1, false)]
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
		/// 舊資料序號 (非舊學雜費轉置的資料，固定為 0) (為了對應至 Student_Receive)
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

		private string _ReduceId = null;
		/// <summary>
		/// Reduce_ID 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceId, true, FieldTypeEnum.VarChar, 20, false)]
		public string ReduceId
		{
			get
			{
				return _ReduceId;
			}
			set
			{
				_ReduceId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// Reduce_01 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce01, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce01
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_02 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce02, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce02
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_03 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce03, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce03
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_04 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce04, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce04
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_05 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce05, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce05
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_06 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce06, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce06
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_07 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce07, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce07
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_08 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce08, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce08
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_09 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce09, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce09
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce10, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce10
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce11, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce11
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce12, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce12
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce13, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce13
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce14, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce14
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce15, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce15
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Reduce16, false, FieldTypeEnum.Decimal, true)]
		public decimal? Reduce16
		{
			get;
			set;
		}


        [FieldSpec(Field.Reduce17, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce17
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce18, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce18
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce19, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce19
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce20, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce20
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce21, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce21
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce22, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce22
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce23, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce23
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce24, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce24
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce25, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce25
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce26, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce26
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce27, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce27
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce28, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce28
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce29, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce29
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce30, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce30
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce31, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce31
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce32, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce32
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce33, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce33
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce34, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce34
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce35, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce35
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce36, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce36
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce37, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce37
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce38, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce38
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce39, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce39
        {
            get;
            set;
        }

        [FieldSpec(Field.Reduce40, false, FieldTypeEnum.Decimal, true)]
        public decimal? Reduce40
        {
            get;
            set;
        }

		/// <summary>
		/// Reduce_Amount 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount
		{
			get;
			set;
		}
		#endregion
		#endregion

        #region Method
        /// <summary>
        /// 取得指定項目索引的減免明細金額
        /// </summary>
        /// <param name="idx">項目索引 (0 ~ 39)</param>
        /// <returns></returns>
        public decimal? GetReduceAmount(int idx)
        {
            decimal?[] values = new decimal?[] {
                this.Reduce01, this.Reduce02, this.Reduce03, this.Reduce04, this.Reduce05,
                this.Reduce06, this.Reduce07, this.Reduce08, this.Reduce09, this.Reduce10,
                this.Reduce11, this.Reduce12, this.Reduce13, this.Reduce14, this.Reduce15,
                this.Reduce16, this.Reduce17, this.Reduce18, this.Reduce19, this.Reduce20,
                this.Reduce21, this.Reduce22, this.Reduce23, this.Reduce24, this.Reduce25,
                this.Reduce26, this.Reduce27, this.Reduce28, this.Reduce29, this.Reduce30,
                this.Reduce31, this.Reduce32, this.Reduce33, this.Reduce34, this.Reduce35,
                this.Reduce36, this.Reduce37, this.Reduce38, this.Reduce39, this.Reduce40
            };

            if (idx > -1 && idx < values.Length)
            {
                return values[idx];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 設定指定項目編號的減免明細金額 (1 ~ 40)
        /// </summary>
        /// <param name="no">項目編號 (1 ~ 40)</param>
        /// <param name="amount">項目減免金額</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool SetReduceItemAmount(int no, decimal amount)
        {
            string itemName = String.Format("Reduce_{0:00}", no);
            return this.SetReduceItemAmount(itemName, amount);
        }

        /// <summary>
        /// 設定指定項目名稱的減免明細金額 (Reduce_01 ~ Reduce_40)
        /// </summary>
        /// <param name="itemName">項目名稱 (Reduce_01 ~ Reduce_40，不區分大小寫)</param>
        /// <param name="amount">項目減免金額</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool SetReduceItemAmount(string itemName, decimal? amount)
        {
            if (!String.IsNullOrWhiteSpace(itemName) && itemName.StartsWith("Reduce_", StringComparison.CurrentCultureIgnoreCase))
            {
                itemName = itemName.ToLower().Replace("reduce_", "Reduce_");
                switch (itemName)
                {
                    #region 01 ~ 10
                    case Field.Reduce01:
                        this.Reduce01 = amount;
                        return true;
                    case Field.Reduce02:
                        this.Reduce02 = amount;
                        return true;
                    case Field.Reduce03:
                        this.Reduce03 = amount;
                        return true;
                    case Field.Reduce04:
                        this.Reduce04 = amount;
                        return true;
                    case Field.Reduce05:
                        this.Reduce05 = amount;
                        return true;
                    case Field.Reduce06:
                        this.Reduce06 = amount;
                        return true;
                    case Field.Reduce07:
                        this.Reduce07 = amount;
                        return true;
                    case Field.Reduce08:
                        this.Reduce08 = amount;
                        return true;
                    case Field.Reduce09:
                        this.Reduce09 = amount;
                        return true;
                    case Field.Reduce10:
                        this.Reduce10 = amount;
                        return true;
                    #endregion

                    #region 11 ~ 20
                    case Field.Reduce11:
                        this.Reduce11 = amount;
                        return true;
                    case Field.Reduce12:
                        this.Reduce12 = amount;
                        return true;
                    case Field.Reduce13:
                        this.Reduce13 = amount;
                        return true;
                    case Field.Reduce14:
                        this.Reduce14 = amount;
                        return true;
                    case Field.Reduce15:
                        this.Reduce15 = amount;
                        return true;
                    case Field.Reduce16:
                        this.Reduce16 = amount;
                        return true;
                    case Field.Reduce17:
                        this.Reduce17 = amount;
                        return true;
                    case Field.Reduce18:
                        this.Reduce18 = amount;
                        return true;
                    case Field.Reduce19:
                        this.Reduce19 = amount;
                        return true;
                    case Field.Reduce20:
                        this.Reduce20 = amount;
                        return true;
                    #endregion

                    #region 21 ~ 30
                    case Field.Reduce21:
                        this.Reduce21 = amount;
                        return true;
                    case Field.Reduce22:
                        this.Reduce22 = amount;
                        return true;
                    case Field.Reduce23:
                        this.Reduce23 = amount;
                        return true;
                    case Field.Reduce24:
                        this.Reduce24 = amount;
                        return true;
                    case Field.Reduce25:
                        this.Reduce25 = amount;
                        return true;
                    case Field.Reduce26:
                        this.Reduce26 = amount;
                        return true;
                    case Field.Reduce27:
                        this.Reduce27 = amount;
                        return true;
                    case Field.Reduce28:
                        this.Reduce28 = amount;
                        return true;
                    case Field.Reduce29:
                        this.Reduce29 = amount;
                        return true;
                    case Field.Reduce30:
                        this.Reduce30 = amount;
                        return true;
                    #endregion

                    #region 31 ~ 40
                    case Field.Reduce31:
                        this.Reduce31 = amount;
                        return true;
                    case Field.Reduce32:
                        this.Reduce32 = amount;
                        return true;
                    case Field.Reduce33:
                        this.Reduce33 = amount;
                        return true;
                    case Field.Reduce34:
                        this.Reduce34 = amount;
                        return true;
                    case Field.Reduce35:
                        this.Reduce35 = amount;
                        return true;
                    case Field.Reduce36:
                        this.Reduce36 = amount;
                        return true;
                    case Field.Reduce37:
                        this.Reduce37 = amount;
                        return true;
                    case Field.Reduce38:
                        this.Reduce38 = amount;
                        return true;
                    case Field.Reduce39:
                        this.Reduce39 = amount;
                        return true;
                    case Field.Reduce40:
                        this.Reduce40 = amount;
                        return true;
                    #endregion
                }
            }
            return false;
        }

        /// <summary>
        /// 取得指定項目編號的減免明細金額 (1 ~ 40)
        /// </summary>
        /// <param name="no">項目編號 (1 ~ 40)</param>
        /// <returns>傳回項目減免金額或 null</returns>
        public decimal? GetReduceItemAmount(int no)
        {
            string itemName = String.Format("Reduce_{0:00}", no);
            return this.GetReduceItemAmount(itemName);
        }

        /// <summary>
        /// 取得指定項目名稱的減免明細金額 (Recuce_01 ~ Reduce_40)
        /// </summary>
        /// <param name="itemName">項目名稱 (Reduce_01 ~ Reduce_40，不區分大小寫)</param>
        /// <returns>傳回項目減免金額或 null</returns>
        public decimal? GetReduceItemAmount(string itemName)
        {
            if (itemName != null && itemName.Length == 9 && itemName.StartsWith("Reduce_", StringComparison.CurrentCultureIgnoreCase))
            {
                itemName = String.Concat("Reduce_", itemName.Substring(7));
                switch (itemName)
                {
                    #region 01 ~ 10
                    case Field.Reduce01:
                        return this.Reduce01;
                    case Field.Reduce02:
                        return this.Reduce02;
                    case Field.Reduce03:
                        return this.Reduce03;
                    case Field.Reduce04:
                        return this.Reduce04;
                    case Field.Reduce05:
                        return this.Reduce05;
                    case Field.Reduce06:
                        return this.Reduce06;
                    case Field.Reduce07:
                        return this.Reduce07;
                    case Field.Reduce08:
                        return this.Reduce08;
                    case Field.Reduce09:
                        return this.Reduce09;
                    case Field.Reduce10:
                        return this.Reduce10;
                    #endregion

                    #region 11 ~ 20
                    case Field.Reduce11:
                        return this.Reduce11;
                    case Field.Reduce12:
                        return this.Reduce12;
                    case Field.Reduce13:
                        return this.Reduce13;
                    case Field.Reduce14:
                        return this.Reduce14;
                    case Field.Reduce15:
                        return this.Reduce15;
                    case Field.Reduce16:
                        return this.Reduce16;
                    case Field.Reduce17:
                        return this.Reduce17;
                    case Field.Reduce18:
                        return this.Reduce18;
                    case Field.Reduce19:
                        return this.Reduce19;
                    case Field.Reduce20:
                        return this.Reduce20;
                    #endregion

                    #region 21 ~ 30
                    case Field.Reduce21:
                        return this.Reduce21;
                    case Field.Reduce22:
                        return this.Reduce22;
                    case Field.Reduce23:
                        return this.Reduce23;
                    case Field.Reduce24:
                        return this.Reduce24;
                    case Field.Reduce25:
                        return this.Reduce25;
                    case Field.Reduce26:
                        return this.Reduce26;
                    case Field.Reduce27:
                        return this.Reduce27;
                    case Field.Reduce28:
                        return this.Reduce28;
                    case Field.Reduce29:
                        return this.Reduce29;
                    case Field.Reduce30:
                        return this.Reduce30;
                    #endregion

                    #region 31 ~ 40
                    case Field.Reduce31:
                        return this.Reduce31;
                    case Field.Reduce32:
                        return this.Reduce32;
                    case Field.Reduce33:
                        return this.Reduce33;
                    case Field.Reduce34:
                        return this.Reduce34;
                    case Field.Reduce35:
                        return this.Reduce35;
                    case Field.Reduce36:
                        return this.Reduce36;
                    case Field.Reduce37:
                        return this.Reduce37;
                    case Field.Reduce38:
                        return this.Reduce38;
                    case Field.Reduce39:
                        return this.Reduce39;
                    case Field.Reduce40:
                        return this.Reduce40;
                    #endregion
                }
            }
            return null;
        }

        /// <summary>
        /// 取得指定項目名稱的減免明細金額 (Reduce_01 ~ Reduce_40) 並嘗試轉成 Int32 型別
        /// </summary>
        /// <param name="itemName">項目名稱 (Reduce_01 ~ Reduce_40，不區分大小寫)</param>
        /// <returns>傳回Int32 型別的項目減免金額或 0</returns>
        public int TryGetReduceItemAmountByInt32(string itemName)
        {
            decimal? amount = this.GetReduceItemAmount(itemName);
            if (amount == null || amount.Value > Int32.MaxValue)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(amount.Value);
            }
        }
        #endregion
    }
}
