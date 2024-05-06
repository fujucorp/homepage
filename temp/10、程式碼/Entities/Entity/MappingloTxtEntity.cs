/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:35:33
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
	/// MappingLO_Txt 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class MappingloTxtEntity : Entity
	{
		public const string TABLE_NAME = "MappingLO_Txt";

        #region Const
        /// <summary>
        /// 虛擬帳號欄位起始位置固定值
        /// </summary>
        public const int CancelNoS_FIX_VALUE = 0;

        /// <summary>
        /// 虛擬帳號欄位字元數固定值
        /// </summary>
        public const int CancelNoL_FIX_VALUE = 16;
        #endregion

		#region Field Name Const Class
		/// <summary>
		/// MappingloTxtEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
            /// 對照檔代碼 欄位名稱常數定義
			/// </summary>
			public const string MappingId = "Mapping_Id";

			/// <summary>
            /// 所屬商家代號 欄位名稱常數定義
			/// </summary>
			public const string ReceiveType = "Receive_Type";
			#endregion

			#region Data
            /// <summary>
            /// 對照檔名稱 欄位名稱常數定義
            /// </summary>
            public const string MappingName = "Mapping_Name";

			/// <summary>
            /// 學年代碼 欄位名稱常數定義 (土銀不使用)
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
            /// 學期代碼 欄位名稱常數定義 (土銀不使用)
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
            /// 部別代碼 欄位名稱常數定義 (土銀不使用)
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
            /// 費用別代碼 欄位名稱常數定義 (土銀不使用)
			/// </summary>
			public const string ReceiveId = "Receive_Id";
            #endregion

            #region 上傳資料相關欄位
            /// <summary>
            /// 虛擬帳號欄位起始位置 (此為後來額外加入的欄位，為了避免要異動資料庫，此欄位並不存在資料表)
            /// </summary>
            public const string CancelNoS = "Cancel_No_S";

            /// <summary>
            /// 虛擬帳號欄位字元數 (此為後來額外加入的欄位，為了避免要異動資料庫，此欄位並不存在資料表)
            /// </summary>
            public const string CancelNoL = "Cancel_No_L";

            /// <summary>
            /// 學號欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string SIdS = "S_ID_S";

			/// <summary>
            /// 學號欄位字元數 欄位名稱常數定義
			/// </summary>
			public const string SIdL = "S_ID_L";

			/// <summary>
            /// 就貸代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string LoNameS = "Lo_Name_S";

			/// <summary>
            /// 就貸代碼欄位字元數 欄位名稱常數定義
			/// </summary>
			public const string LoNameL = "Lo_Name_L";

			/// <summary>
            /// 就貸名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string LoCountS = "Lo_Count_S";

			/// <summary>
            /// 就貸名稱欄位字元數 欄位名稱常數定義
			/// </summary>
			public const string LoCountL = "Lo_Count_L";

			/// <summary>
            /// 可貸金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string LoAmountS = "Lo_amount_S";

			/// <summary>
            /// 可貸金額欄位字元數 欄位名稱常數定義
			/// </summary>
			public const string LoAmountL = "Lo_amount_L";

            #region 就貸金額項目 1 ~ 40
            /// <summary>
            /// 就貸金額項目 (就貸收入科目01)欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Lo1S = "Lo_1_S";

			/// <summary>
            /// 就貸金額項目 (就貸收入科目01)欄位字元數 欄位名稱常數定義
			/// </summary>
			public const string Lo1L = "Lo_1_L";

			/// <summary>
			/// Lo_2_S 欄位名稱常數定義
			/// </summary>
			public const string Lo2S = "Lo_2_S";

			/// <summary>
			/// Lo_2_L 欄位名稱常數定義
			/// </summary>
			public const string Lo2L = "Lo_2_L";

			/// <summary>
			/// Lo_3_S 欄位名稱常數定義
			/// </summary>
			public const string Lo3S = "Lo_3_S";

			/// <summary>
			/// Lo_3_L 欄位名稱常數定義
			/// </summary>
			public const string Lo3L = "Lo_3_L";

			/// <summary>
			/// Lo_4_S 欄位名稱常數定義
			/// </summary>
			public const string Lo4S = "Lo_4_S";

			/// <summary>
			/// Lo_4_L 欄位名稱常數定義
			/// </summary>
			public const string Lo4L = "Lo_4_L";

			/// <summary>
			/// Lo_5_S 欄位名稱常數定義
			/// </summary>
			public const string Lo5S = "Lo_5_S";

			/// <summary>
			/// Lo_5_L 欄位名稱常數定義
			/// </summary>
			public const string Lo5L = "Lo_5_L";

			/// <summary>
			/// Lo_6_S 欄位名稱常數定義
			/// </summary>
			public const string Lo6S = "Lo_6_S";

			/// <summary>
			/// Lo_6_L 欄位名稱常數定義
			/// </summary>
			public const string Lo6L = "Lo_6_L";

			/// <summary>
			/// Lo_7_S 欄位名稱常數定義
			/// </summary>
			public const string Lo7S = "Lo_7_S";

			/// <summary>
			/// Lo_7_L 欄位名稱常數定義
			/// </summary>
			public const string Lo7L = "Lo_7_L";

			/// <summary>
			/// Lo_8_S 欄位名稱常數定義
			/// </summary>
			public const string Lo8S = "Lo_8_S";

			/// <summary>
			/// Lo_8_L 欄位名稱常數定義
			/// </summary>
			public const string Lo8L = "Lo_8_L";

			/// <summary>
			/// Lo_9_S 欄位名稱常數定義
			/// </summary>
			public const string Lo9S = "Lo_9_S";

			/// <summary>
			/// Lo_9_L 欄位名稱常數定義
			/// </summary>
			public const string Lo9L = "Lo_9_L";

			/// <summary>
			/// Lo_10_S 欄位名稱常數定義
			/// </summary>
			public const string Lo10S = "Lo_10_S";

			/// <summary>
			/// Lo_10_L 欄位名稱常數定義
			/// </summary>
			public const string Lo10L = "Lo_10_L";

			/// <summary>
			/// Lo_11_S 欄位名稱常數定義
			/// </summary>
			public const string Lo11S = "Lo_11_S";

			/// <summary>
			/// Lo_11_L 欄位名稱常數定義
			/// </summary>
			public const string Lo11L = "Lo_11_L";

			/// <summary>
			/// Lo_12_S 欄位名稱常數定義
			/// </summary>
			public const string Lo12S = "Lo_12_S";

			/// <summary>
			/// Lo_12_L 欄位名稱常數定義
			/// </summary>
			public const string Lo12L = "Lo_12_L";

			/// <summary>
			/// Lo_13_S 欄位名稱常數定義
			/// </summary>
			public const string Lo13S = "Lo_13_S";

			/// <summary>
			/// Lo_13_L 欄位名稱常數定義
			/// </summary>
			public const string Lo13L = "Lo_13_L";

			/// <summary>
			/// Lo_14_S 欄位名稱常數定義
			/// </summary>
			public const string Lo14S = "Lo_14_S";

			/// <summary>
			/// Lo_14_L 欄位名稱常數定義
			/// </summary>
			public const string Lo14L = "Lo_14_L";

			/// <summary>
			/// Lo_15_S 欄位名稱常數定義
			/// </summary>
			public const string Lo15S = "Lo_15_S";

			/// <summary>
			/// Lo_15_L 欄位名稱常數定義
			/// </summary>
			public const string Lo15L = "Lo_15_L";

			/// <summary>
			/// Lo_16_S 欄位名稱常數定義
			/// </summary>
			public const string Lo16S = "Lo_16_S";

			/// <summary>
			/// Lo_16_L 欄位名稱常數定義
			/// </summary>
			public const string Lo16L = "Lo_16_L";

			/// <summary>
			/// Lo_17_S 欄位名稱常數定義
			/// </summary>
			public const string Lo17S = "Lo_17_S";

			/// <summary>
			/// Lo_17_L 欄位名稱常數定義
			/// </summary>
			public const string Lo17L = "Lo_17_L";

			/// <summary>
			/// Lo_18_S 欄位名稱常數定義
			/// </summary>
			public const string Lo18S = "Lo_18_S";

			/// <summary>
			/// Lo_18_L 欄位名稱常數定義
			/// </summary>
			public const string Lo18L = "Lo_18_L";

			/// <summary>
			/// Lo_19_S 欄位名稱常數定義
			/// </summary>
			public const string Lo19S = "Lo_19_S";

			/// <summary>
			/// Lo_19_L 欄位名稱常數定義
			/// </summary>
			public const string Lo19L = "Lo_19_L";

			/// <summary>
			/// Lo_20_S 欄位名稱常數定義
			/// </summary>
			public const string Lo20S = "Lo_20_S";

			/// <summary>
			/// Lo_20_L 欄位名稱常數定義
			/// </summary>
			public const string Lo20L = "Lo_20_L";

			/// <summary>
			/// Lo_21_S 欄位名稱常數定義
			/// </summary>
			public const string Lo21S = "Lo_21_S";

			/// <summary>
			/// Lo_21_L 欄位名稱常數定義
			/// </summary>
			public const string Lo21L = "Lo_21_L";

			/// <summary>
			/// Lo_22_S 欄位名稱常數定義
			/// </summary>
			public const string Lo22S = "Lo_22_S";

			/// <summary>
			/// Lo_22_L 欄位名稱常數定義
			/// </summary>
			public const string Lo22L = "Lo_22_L";

			/// <summary>
			/// Lo_23_S 欄位名稱常數定義
			/// </summary>
			public const string Lo23S = "Lo_23_S";

			/// <summary>
			/// Lo_23_L 欄位名稱常數定義
			/// </summary>
			public const string Lo23L = "Lo_23_L";

			/// <summary>
			/// Lo_24_S 欄位名稱常數定義
			/// </summary>
			public const string Lo24S = "Lo_24_S";

			/// <summary>
			/// Lo_24_L 欄位名稱常數定義
			/// </summary>
			public const string Lo24L = "Lo_24_L";

			/// <summary>
			/// Lo_25_S 欄位名稱常數定義
			/// </summary>
			public const string Lo25S = "Lo_25_S";

			/// <summary>
			/// Lo_25_L 欄位名稱常數定義
			/// </summary>
			public const string Lo25L = "Lo_25_L";

			/// <summary>
			/// Lo_26_S 欄位名稱常數定義
			/// </summary>
			public const string Lo26S = "Lo_26_S";

			/// <summary>
			/// Lo_26_L 欄位名稱常數定義
			/// </summary>
			public const string Lo26L = "Lo_26_L";

			/// <summary>
			/// Lo_27_S 欄位名稱常數定義
			/// </summary>
			public const string Lo27S = "Lo_27_S";

			/// <summary>
			/// Lo_27_L 欄位名稱常數定義
			/// </summary>
			public const string Lo27L = "Lo_27_L";

			/// <summary>
			/// Lo_28_S 欄位名稱常數定義
			/// </summary>
			public const string Lo28S = "Lo_28_S";

			/// <summary>
			/// Lo_28_L 欄位名稱常數定義
			/// </summary>
			public const string Lo28L = "Lo_28_L";

			/// <summary>
			/// Lo_29_S 欄位名稱常數定義
			/// </summary>
			public const string Lo29S = "Lo_29_S";

			/// <summary>
			/// Lo_29_L 欄位名稱常數定義
			/// </summary>
			public const string Lo29L = "Lo_29_L";

			/// <summary>
			/// Lo_30_S 欄位名稱常數定義
			/// </summary>
			public const string Lo30S = "Lo_30_S";

			/// <summary>
			/// Lo_30_L 欄位名稱常數定義
			/// </summary>
			public const string Lo30L = "Lo_30_L";

			/// <summary>
			/// Lo_31_S 欄位名稱常數定義
			/// </summary>
			public const string Lo31S = "Lo_31_S";

			/// <summary>
			/// Lo_31_L 欄位名稱常數定義
			/// </summary>
			public const string Lo31L = "Lo_31_L";

			/// <summary>
			/// Lo_32_S 欄位名稱常數定義
			/// </summary>
			public const string Lo32S = "Lo_32_S";

			/// <summary>
			/// Lo_32_L 欄位名稱常數定義
			/// </summary>
			public const string Lo32L = "Lo_32_L";

			/// <summary>
			/// Lo_33_S 欄位名稱常數定義
			/// </summary>
			public const string Lo33S = "Lo_33_S";

			/// <summary>
			/// Lo_33_L 欄位名稱常數定義
			/// </summary>
			public const string Lo33L = "Lo_33_L";

			/// <summary>
			/// Lo_34_S 欄位名稱常數定義
			/// </summary>
			public const string Lo34S = "Lo_34_S";

			/// <summary>
			/// Lo_34_L 欄位名稱常數定義
			/// </summary>
			public const string Lo34L = "Lo_34_L";

			/// <summary>
			/// Lo_35_S 欄位名稱常數定義
			/// </summary>
			public const string Lo35S = "Lo_35_S";

			/// <summary>
			/// Lo_35_L 欄位名稱常數定義
			/// </summary>
			public const string Lo35L = "Lo_35_L";

			/// <summary>
			/// Lo_36_S 欄位名稱常數定義
			/// </summary>
			public const string Lo36S = "Lo_36_S";

			/// <summary>
			/// Lo_36_L 欄位名稱常數定義
			/// </summary>
			public const string Lo36L = "Lo_36_L";

			/// <summary>
			/// Lo_37_S 欄位名稱常數定義
			/// </summary>
			public const string Lo37S = "Lo_37_S";

			/// <summary>
			/// Lo_37_L 欄位名稱常數定義
			/// </summary>
			public const string Lo37L = "Lo_37_L";

			/// <summary>
			/// Lo_38_S 欄位名稱常數定義
			/// </summary>
			public const string Lo38S = "Lo_38_S";

			/// <summary>
			/// Lo_38_L 欄位名稱常數定義
			/// </summary>
			public const string Lo38L = "Lo_38_L";

			/// <summary>
			/// Lo_39_S 欄位名稱常數定義
			/// </summary>
			public const string Lo39S = "Lo_39_S";

			/// <summary>
			/// Lo_39_L 欄位名稱常數定義
			/// </summary>
			public const string Lo39L = "Lo_39_L";

			/// <summary>
			/// Lo_40_S 欄位名稱常數定義
			/// </summary>
			public const string Lo40S = "Lo_40_S";

			/// <summary>
			/// Lo_40_L 欄位名稱常數定義
			/// </summary>
			public const string Lo40L = "Lo_40_L";
            #endregion

            /// <summary>
			/// Amount_S 欄位名稱常數定義
			/// </summary>
			public const string AmountS = "Amount_S";

			/// <summary>
			/// Amount_L 欄位名稱常數定義
			/// </summary>
			public const string AmountL = "Amount_L";
            #endregion

            #region 狀態相關欄位
            /// <summary>
            /// 資料狀態 (0=正常 / D=停用) (請參考 DataStatusCodeTexts) 欄位名稱常數定義
			/// </summary>
			public const string Status = "status";

			/// <summary>
            /// 資料建立日期 (含時間) 欄位名稱常數定義
			/// </summary>
			public const string CrtDate = "crt_date";

			/// <summary>
            /// 資料建立者。暫時儲存使用者帳號 (UserId) 欄位名稱常數定義
			/// </summary>
			public const string CrtUser = "crt_user";

			/// <summary>
            /// 資料最後修改日期 (含時間) 欄位名稱常數定義
			/// </summary>
			public const string MdyDate = "mdy_date";

			/// <summary>
            /// 資料最後修改者。暫時儲存使用者帳號 (UserId) 欄位名稱常數定義
			/// </summary>
			public const string MdyUser = "mdy_user";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// MappingloTxtEntity 類別建構式
		/// </summary>
		public MappingloTxtEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _MappingId = null;
		/// <summary>
        /// 對照檔代碼
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
        /// 所屬商家代號
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
        /// 對照檔名稱
        /// </summary>
        [FieldSpec(Field.MappingName, false, FieldTypeEnum.VarChar, 50, true)]
        public string MappingName
        {
            get;
            set;
        }

		/// <summary>
        /// 學年代碼 (土銀不使用)
		/// </summary>
		[FieldSpec(Field.YearId, false, FieldTypeEnum.Char, 3, true)]
		public string YearId
		{
			get;
			set;
		}

		/// <summary>
        /// 學期代碼 (土銀不使用)
		/// </summary>
		[FieldSpec(Field.TermId, false, FieldTypeEnum.Char, 1, true)]
		public string TermId
		{
			get;
			set;
		}

		/// <summary>
        /// 部別代碼 (土銀不使用)
		/// </summary>
		[FieldSpec(Field.DepId, false, FieldTypeEnum.Char, 1, true)]
		public string DepId
		{
			get;
			set;
		}

		/// <summary>
        /// 費用別代碼 (土銀不使用)
		/// </summary>
		[FieldSpec(Field.ReceiveId, false, FieldTypeEnum.Char, 1, true)]
		public string ReceiveId
		{
			get;
			set;
		}
        #endregion

        #region 上傳資料相關欄位
        /// <summary>
        /// 虛擬帳號欄位起始位置 (此為後來額外加入的欄位，為了避免要異動資料庫，此欄位並不存在資料表。唯獨，固定傳回 0)
        /// </summary>
        [XmlIgnore]
        public int? CancelNoS
        {
            get
            {
                return CancelNoS_FIX_VALUE;
            }
            //set;
        }

        /// <summary>
        /// 虛擬帳號欄位字元數 (此為後來額外加入的欄位，為了避免要異動資料庫，此欄位並不存在資料表。唯獨，固定傳回 16)
        /// </summary>
        [XmlIgnore]
        public int? CancelNoL
        {
            get
            {
                return CancelNoL_FIX_VALUE;
            }
            //set;
        }

        /// <summary>
        /// 學號欄位起始位置
		/// </summary>
		[FieldSpec(Field.SIdS, false, FieldTypeEnum.Integer, true)]
		public int? SIdS
		{
			get;
			set;
		}

		/// <summary>
        /// 學號欄位字元數
		/// </summary>
		[FieldSpec(Field.SIdL, false, FieldTypeEnum.Integer, true)]
		public int? SIdL
		{
			get;
			set;
		}

		/// <summary>
        /// 就貸代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.LoNameS, false, FieldTypeEnum.Integer, true)]
		public int? LoNameS
		{
			get;
			set;
		}

		/// <summary>
        /// 就貸代碼欄位字元數
		/// </summary>
		[FieldSpec(Field.LoNameL, false, FieldTypeEnum.Integer, true)]
		public int? LoNameL
		{
			get;
			set;
		}

		/// <summary>
        /// 就貸名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.LoCountS, false, FieldTypeEnum.Integer, true)]
		public int? LoCountS
		{
			get;
			set;
		}

		/// <summary>
        /// 就貸名稱欄位字元數
		/// </summary>
		[FieldSpec(Field.LoCountL, false, FieldTypeEnum.Integer, true)]
		public int? LoCountL
		{
			get;
			set;
		}

		/// <summary>
        /// 可貸金額欄位起始位置
		/// </summary>
		[FieldSpec(Field.LoAmountS, false, FieldTypeEnum.Integer, true)]
		public int? LoAmountS
		{
			get;
			set;
		}

		/// <summary>
        /// 可貸金額欄位字元數
		/// </summary>
		[FieldSpec(Field.LoAmountL, false, FieldTypeEnum.Integer, true)]
		public int? LoAmountL
		{
			get;
			set;
		}

        #region 就貸金額項目 1 ~ 40
        /// <summary>
        /// 就貸金額項目 (收入科目01就貸金額)欄位起始位置
		/// </summary>
		[FieldSpec(Field.Lo1S, false, FieldTypeEnum.Integer, true)]
		public int? Lo1S
		{
			get;
			set;
		}

		/// <summary>
        /// 就貸金額項目 (就貸收入科目01)欄位字元數
		/// </summary>
		[FieldSpec(Field.Lo1L, false, FieldTypeEnum.Integer, true)]
		public int? Lo1L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_2_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo2S, false, FieldTypeEnum.Integer, true)]
		public int? Lo2S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_2_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo2L, false, FieldTypeEnum.Integer, true)]
		public int? Lo2L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_3_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo3S, false, FieldTypeEnum.Integer, true)]
		public int? Lo3S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_3_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo3L, false, FieldTypeEnum.Integer, true)]
		public int? Lo3L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_4_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo4S, false, FieldTypeEnum.Integer, true)]
		public int? Lo4S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_4_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo4L, false, FieldTypeEnum.Integer, true)]
		public int? Lo4L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_5_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo5S, false, FieldTypeEnum.Integer, true)]
		public int? Lo5S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_5_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo5L, false, FieldTypeEnum.Integer, true)]
		public int? Lo5L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_6_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo6S, false, FieldTypeEnum.Integer, true)]
		public int? Lo6S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_6_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo6L, false, FieldTypeEnum.Integer, true)]
		public int? Lo6L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_7_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo7S, false, FieldTypeEnum.Integer, true)]
		public int? Lo7S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_7_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo7L, false, FieldTypeEnum.Integer, true)]
		public int? Lo7L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_8_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo8S, false, FieldTypeEnum.Integer, true)]
		public int? Lo8S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_8_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo8L, false, FieldTypeEnum.Integer, true)]
		public int? Lo8L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_9_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo9S, false, FieldTypeEnum.Integer, true)]
		public int? Lo9S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_9_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo9L, false, FieldTypeEnum.Integer, true)]
		public int? Lo9L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_10_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo10S, false, FieldTypeEnum.Integer, true)]
		public int? Lo10S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_10_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo10L, false, FieldTypeEnum.Integer, true)]
		public int? Lo10L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_11_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo11S, false, FieldTypeEnum.Integer, true)]
		public int? Lo11S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_11_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo11L, false, FieldTypeEnum.Integer, true)]
		public int? Lo11L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_12_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo12S, false, FieldTypeEnum.Integer, true)]
		public int? Lo12S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_12_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo12L, false, FieldTypeEnum.Integer, true)]
		public int? Lo12L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_13_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo13S, false, FieldTypeEnum.Integer, true)]
		public int? Lo13S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_13_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo13L, false, FieldTypeEnum.Integer, true)]
		public int? Lo13L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_14_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo14S, false, FieldTypeEnum.Integer, true)]
		public int? Lo14S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_14_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo14L, false, FieldTypeEnum.Integer, true)]
		public int? Lo14L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_15_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo15S, false, FieldTypeEnum.Integer, true)]
		public int? Lo15S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_15_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo15L, false, FieldTypeEnum.Integer, true)]
		public int? Lo15L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_16_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo16S, false, FieldTypeEnum.Integer, true)]
		public int? Lo16S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_16_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo16L, false, FieldTypeEnum.Integer, true)]
		public int? Lo16L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_17_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo17S, false, FieldTypeEnum.Integer, true)]
		public int? Lo17S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_17_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo17L, false, FieldTypeEnum.Integer, true)]
		public int? Lo17L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_18_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo18S, false, FieldTypeEnum.Integer, true)]
		public int? Lo18S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_18_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo18L, false, FieldTypeEnum.Integer, true)]
		public int? Lo18L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_19_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo19S, false, FieldTypeEnum.Integer, true)]
		public int? Lo19S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_19_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo19L, false, FieldTypeEnum.Integer, true)]
		public int? Lo19L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_20_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo20S, false, FieldTypeEnum.Integer, true)]
		public int? Lo20S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_20_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo20L, false, FieldTypeEnum.Integer, true)]
		public int? Lo20L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_21_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo21S, false, FieldTypeEnum.Integer, true)]
		public int? Lo21S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_21_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo21L, false, FieldTypeEnum.Integer, true)]
		public int? Lo21L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_22_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo22S, false, FieldTypeEnum.Integer, true)]
		public int? Lo22S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_22_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo22L, false, FieldTypeEnum.Integer, true)]
		public int? Lo22L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_23_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo23S, false, FieldTypeEnum.Integer, true)]
		public int? Lo23S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_23_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo23L, false, FieldTypeEnum.Integer, true)]
		public int? Lo23L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_24_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo24S, false, FieldTypeEnum.Integer, true)]
		public int? Lo24S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_24_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo24L, false, FieldTypeEnum.Integer, true)]
		public int? Lo24L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_25_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo25S, false, FieldTypeEnum.Integer, true)]
		public int? Lo25S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_25_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo25L, false, FieldTypeEnum.Integer, true)]
		public int? Lo25L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_26_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo26S, false, FieldTypeEnum.Integer, true)]
		public int? Lo26S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_26_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo26L, false, FieldTypeEnum.Integer, true)]
		public int? Lo26L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_27_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo27S, false, FieldTypeEnum.Integer, true)]
		public int? Lo27S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_27_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo27L, false, FieldTypeEnum.Integer, true)]
		public int? Lo27L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_28_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo28S, false, FieldTypeEnum.Integer, true)]
		public int? Lo28S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_28_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo28L, false, FieldTypeEnum.Integer, true)]
		public int? Lo28L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_29_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo29S, false, FieldTypeEnum.Integer, true)]
		public int? Lo29S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_29_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo29L, false, FieldTypeEnum.Integer, true)]
		public int? Lo29L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_30_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo30S, false, FieldTypeEnum.Integer, true)]
		public int? Lo30S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_30_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo30L, false, FieldTypeEnum.Integer, true)]
		public int? Lo30L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_31_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo31S, false, FieldTypeEnum.Integer, true)]
		public int? Lo31S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_31_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo31L, false, FieldTypeEnum.Integer, true)]
		public int? Lo31L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_32_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo32S, false, FieldTypeEnum.Integer, true)]
		public int? Lo32S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_32_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo32L, false, FieldTypeEnum.Integer, true)]
		public int? Lo32L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_33_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo33S, false, FieldTypeEnum.Integer, true)]
		public int? Lo33S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_33_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo33L, false, FieldTypeEnum.Integer, true)]
		public int? Lo33L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_34_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo34S, false, FieldTypeEnum.Integer, true)]
		public int? Lo34S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_34_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo34L, false, FieldTypeEnum.Integer, true)]
		public int? Lo34L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_35_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo35S, false, FieldTypeEnum.Integer, true)]
		public int? Lo35S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_35_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo35L, false, FieldTypeEnum.Integer, true)]
		public int? Lo35L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_36_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo36S, false, FieldTypeEnum.Integer, true)]
		public int? Lo36S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_36_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo36L, false, FieldTypeEnum.Integer, true)]
		public int? Lo36L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_37_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo37S, false, FieldTypeEnum.Integer, true)]
		public int? Lo37S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_37_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo37L, false, FieldTypeEnum.Integer, true)]
		public int? Lo37L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_38_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo38S, false, FieldTypeEnum.Integer, true)]
		public int? Lo38S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_38_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo38L, false, FieldTypeEnum.Integer, true)]
		public int? Lo38L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_39_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo39S, false, FieldTypeEnum.Integer, true)]
		public int? Lo39S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_39_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo39L, false, FieldTypeEnum.Integer, true)]
		public int? Lo39L
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_40_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo40S, false, FieldTypeEnum.Integer, true)]
		public int? Lo40S
		{
			get;
			set;
		}

		/// <summary>
		/// Lo_40_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Lo40L, false, FieldTypeEnum.Integer, true)]
		public int? Lo40L
		{
			get;
			set;
		}
        #endregion

        /// <summary>
		/// Amount_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.AmountS, false, FieldTypeEnum.Integer, true)]
		public int? AmountS
		{
			get;
			set;
		}

		/// <summary>
		/// Amount_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.AmountL, false, FieldTypeEnum.Integer, true)]
		public int? AmountL
		{
			get;
			set;
		}
        #endregion

        #region 狀態相關欄位
        /// <summary>
        /// 資料狀態 (0=正常 / D=停用) (請參考 DataStatusCodeTexts)
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
		public string Status
		{
			get;
			set;
		}

		/// <summary>
        /// 資料建立日期 (含時間)
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get;
			set;
		}

		/// <summary>
        /// 資料建立者。暫時儲存使用者帳號 (UserId)
		/// </summary>
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
		public string CrtUser
		{
			get;
			set;
		}

		/// <summary>
        /// 資料最後修改日期 (含時間)
		/// </summary>
		[FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? MdyDate
		{
			get;
			set;
		}

		/// <summary>
        /// 資料最後修改者。暫時儲存使用者帳號 (UserId)
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

            if (this.CancelNoS != null && this.CancelNoL != null)
            {
                mapFields.Add(new TxtMapField(MappingloXlsmdbEntity.Field.CancelNo, this.CancelNoS.Value, this.CancelNoL.Value, new NumberChecker(12, 16, "12、14或16碼的虛擬帳號數字")));
            }
            if (this.SIdS != null && this.SIdL != null)
            {
                mapFields.Add(new TxtMapField(MappingloXlsmdbEntity.Field.SId, this.SIdS.Value, this.SIdL.Value, new CodeChecker(1, 20)));
            }
            if (this.LoCountS != null && this.LoCountL != null)
            {
                mapFields.Add(new TxtMapField(MappingloXlsmdbEntity.Field.LoCount, this.LoCountS.Value, this.LoCountL.Value, new WordChecker(1, 20)));
            }
            if (this.LoNameS != null && this.LoNameL != null)
            {
                mapFields.Add(new TxtMapField(MappingloXlsmdbEntity.Field.LoName, this.LoNameS.Value, this.LoNameL.Value, new CodeChecker(1, 20)));
            }
            if (this.LoAmountS != null && this.LoAmountL != null)
            {
                mapFields.Add(new TxtMapField(MappingloXlsmdbEntity.Field.LoAmount, this.LoAmountS.Value, this.LoAmountL.Value, new DecimalChecker(0, 999999999M, false)));
            }

            #region 就貸金額對照欄位 (Lo1 ~ Lo40)
            {
                string[] fields = new string[] {
                    MappingloXlsmdbEntity.Field.Lo1, MappingloXlsmdbEntity.Field.Lo2, MappingloXlsmdbEntity.Field.Lo3, MappingloXlsmdbEntity.Field.Lo4, MappingloXlsmdbEntity.Field.Lo5,
                    MappingloXlsmdbEntity.Field.Lo6, MappingloXlsmdbEntity.Field.Lo7, MappingloXlsmdbEntity.Field.Lo8, MappingloXlsmdbEntity.Field.Lo9, MappingloXlsmdbEntity.Field.Lo10,
                    MappingloXlsmdbEntity.Field.Lo11, MappingloXlsmdbEntity.Field.Lo12, MappingloXlsmdbEntity.Field.Lo13, MappingloXlsmdbEntity.Field.Lo14, MappingloXlsmdbEntity.Field.Lo15,
                    MappingloXlsmdbEntity.Field.Lo16, MappingloXlsmdbEntity.Field.Lo17, MappingloXlsmdbEntity.Field.Lo18, MappingloXlsmdbEntity.Field.Lo19, MappingloXlsmdbEntity.Field.Lo20,
                    MappingloXlsmdbEntity.Field.Lo21, MappingloXlsmdbEntity.Field.Lo22, MappingloXlsmdbEntity.Field.Lo23, MappingloXlsmdbEntity.Field.Lo24, MappingloXlsmdbEntity.Field.Lo25,
                    MappingloXlsmdbEntity.Field.Lo26, MappingloXlsmdbEntity.Field.Lo27, MappingloXlsmdbEntity.Field.Lo28, MappingloXlsmdbEntity.Field.Lo29, MappingloXlsmdbEntity.Field.Lo30,
                    MappingloXlsmdbEntity.Field.Lo31, MappingloXlsmdbEntity.Field.Lo32, MappingloXlsmdbEntity.Field.Lo33, MappingloXlsmdbEntity.Field.Lo34, MappingloXlsmdbEntity.Field.Lo35,
                    MappingloXlsmdbEntity.Field.Lo36, MappingloXlsmdbEntity.Field.Lo37, MappingloXlsmdbEntity.Field.Lo38, MappingloXlsmdbEntity.Field.Lo39, MappingloXlsmdbEntity.Field.Lo40
                };
                int?[] starts = new int?[] {
                    this.Lo1S, this.Lo2S, this.Lo3S, this.Lo4S, this.Lo5S,
                    this.Lo6S, this.Lo7S, this.Lo8S, this.Lo9S, this.Lo10S,
                    this.Lo11S, this.Lo12S, this.Lo13S, this.Lo14S, this.Lo15S,
                    this.Lo16S, this.Lo17S, this.Lo18S, this.Lo19S, this.Lo20S,
                    this.Lo21S, this.Lo22S, this.Lo23S, this.Lo24S, this.Lo25S,
                    this.Lo26S, this.Lo27S, this.Lo28S, this.Lo29S, this.Lo30S,
                    this.Lo31S, this.Lo32S, this.Lo33S, this.Lo34S, this.Lo35S,
                    this.Lo36S, this.Lo37S, this.Lo38S, this.Lo39S, this.Lo40S
                };
                int?[] lengths = new int?[] {
                    this.Lo1L, this.Lo2L, this.Lo3L, this.Lo4L, this.Lo5L,
                    this.Lo6L, this.Lo7L, this.Lo8L, this.Lo9L, this.Lo10L,
                    this.Lo11L, this.Lo12L, this.Lo13L, this.Lo14L, this.Lo15L,
                    this.Lo16L, this.Lo17L, this.Lo18L, this.Lo19L, this.Lo20L,
                    this.Lo21L, this.Lo22L, this.Lo23L, this.Lo24L, this.Lo25L,
                    this.Lo26L, this.Lo27L, this.Lo28L, this.Lo29L, this.Lo30L,
                    this.Lo31L, this.Lo32L, this.Lo33L, this.Lo34L, this.Lo35L,
                    this.Lo36L, this.Lo37L, this.Lo38L, this.Lo39L, this.Lo40L
                };
                for (int idx = 0; idx < starts.Length; idx++)
                {
                    if (starts[idx] != null && lengths[idx] != null)
                    {
                        mapFields.Add(new TxtMapField(fields[idx], starts[idx].Value, lengths[idx].Value, new DecimalChecker(0M, 999999999M, false)));
                    }
                }
            }
            #endregion

            return mapFields.ToArray();
        }
        #endregion

        #region Overrider Fuju.Entity's Method
        /// <summary>
        /// 取得指定的欄位名稱 (Field) 或屬性名稱 (Property) 的值
        /// </summary>
        /// <param name="nameOfFieldOrProperty">指定的欄位名稱 (Field) 或屬性名稱 (Property)</param>
        /// <param name="result">傳回執行結果</param>
        /// <returns>成功傳回值，否則傳回 null</returns>
        public override object GetValue(string nameOfFieldOrProperty, out Fuju.Result result)
        {
            if (nameOfFieldOrProperty == Field.CancelNoS || nameOfFieldOrProperty == "CancelNoS")
            {
                result = new Fuju.Result(true);
                return this.CancelNoS;
            }
            else if (nameOfFieldOrProperty == Field.CancelNoL || nameOfFieldOrProperty == "CancelNoL")
            {
                result = new Fuju.Result(true);
                return this.CancelNoL;
            }
            else
            {
                return base.GetValue(nameOfFieldOrProperty, out result);
            }
        }
        #endregion
	}
}
