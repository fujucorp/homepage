/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/06/16 15:42:34
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
	/// 合計項目設定資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ReceiveSumEntity : Entity
	{
		public const string TABLE_NAME = "Receive_Sum";

		#region Field Name Const Class
		/// <summary>
		/// 合計項目設定 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 代收類別代碼 (商家代號) 欄位名稱常數定義
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 學年代號 欄位名稱常數定義
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
			/// 學期代號 欄位名稱常數定義
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
			/// 部別代號 欄位名稱常數定義
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
			/// 代收費用別代號 欄位名稱常數定義
			/// </summary>
			public const string ReceiveId = "Receive_Id";

			/// <summary>
			/// 合計項目代號 欄位名稱常數定義
			/// </summary>
			public const string SumId = "Sum_Id";
			#endregion

			#region Data
			/// <summary>
			/// 合計項目名稱 欄位名稱常數定義
			/// </summary>
			public const string SumName = "Sum_Name";

			/// <summary>
			/// 收入科目01 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive01 = "Receive_01";

			/// <summary>
			/// 收入科目02 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive02 = "Receive_02";

			/// <summary>
			/// 收入科目03 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive03 = "Receive_03";

			/// <summary>
			/// 收入科目04 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive04 = "Receive_04";

			/// <summary>
			/// 收入科目05 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive05 = "Receive_05";

			/// <summary>
			/// 收入科目06 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive06 = "Receive_06";

			/// <summary>
			/// 收入科目07 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive07 = "Receive_07";

			/// <summary>
			/// 收入科目08 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive08 = "Receive_08";

			/// <summary>
			/// 收入科目09 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive09 = "Receive_09";

			/// <summary>
			/// 收入科目10 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive10 = "Receive_10";

			/// <summary>
			/// 收入科目11 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive11 = "Receive_11";

			/// <summary>
			/// 收入科目12 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive12 = "Receive_12";

			/// <summary>
			/// 收入科目13 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive13 = "Receive_13";

			/// <summary>
			/// 收入科目14 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive14 = "Receive_14";

			/// <summary>
			/// 收入科目15 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive15 = "Receive_15";

			/// <summary>
			/// 收入科目16 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive16 = "Receive_16";

			/// <summary>
			/// 收入科目17 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive17 = "Receive_17";

			/// <summary>
			/// 收入科目18 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive18 = "Receive_18";

			/// <summary>
			/// 收入科目19 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive19 = "Receive_19";

			/// <summary>
			/// 收入科目20 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive20 = "Receive_20";

			/// <summary>
			/// 收入科目21 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive21 = "Receive_21";

			/// <summary>
			/// 收入科目22 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive22 = "Receive_22";

			/// <summary>
			/// 收入科目23 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive23 = "Receive_23";

			/// <summary>
			/// 收入科目24 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive24 = "Receive_24";

			/// <summary>
			/// 收入科目25 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive25 = "Receive_25";

			/// <summary>
			/// 收入科目26 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive26 = "Receive_26";

			/// <summary>
			/// 收入科目27 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive27 = "Receive_27";

			/// <summary>
			/// 收入科目28 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive28 = "Receive_28";

			/// <summary>
			/// 收入科目29 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive29 = "Receive_29";

			/// <summary>
			/// 收入科目30 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive30 = "Receive_30";

			/// <summary>
			/// 收入科目31 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive31 = "Receive_31";

			/// <summary>
			/// 收入科目32 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive32 = "Receive_32";

			/// <summary>
			/// 收入科目33 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive33 = "Receive_33";

			/// <summary>
			/// 收入科目34 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive34 = "Receive_34";

			/// <summary>
			/// 收入科目35 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive35 = "Receive_35";

			/// <summary>
			/// 收入科目36 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive36 = "Receive_36";

			/// <summary>
			/// 收入科目37 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive37 = "Receive_37";

			/// <summary>
			/// 收入科目38 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive38 = "Receive_38";

			/// <summary>
			/// 收入科目39 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive39 = "Receive_39";

			/// <summary>
			/// 收入科目40 是否要計算期標 (Y=是 / N=否) 欄位名稱常數定義
			/// </summary>
			public const string Receive40 = "Receive_40";

			#region [MDY:20220808] 2022擴充案 合計項目英文名稱 新增欄位
			/// <summary>
			/// 合計項目英文名稱
			/// </summary>
			public const string SumEName = "Sum_EName";
			#endregion
			#endregion

			#region 狀態相關欄位
			/// <summary>
			/// 狀態
			/// </summary>
			public const string Status = "status";

			/// <summary>
			/// 建立日期
			/// </summary>
			public const string CrtDate = "crt_date";

			/// <summary>
			/// 建立者
			/// </summary>
			public const string CrtUser = "crt_user";

			/// <summary>
			/// 最後修改日期
			/// </summary>
			public const string MdyDate = "mdy_date";

			/// <summary>
			/// 最後修改者
			/// </summary>
			public const string MdyUser = "mdy_user";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// ReceiveSumEntity 類別建構式
		/// </summary>
		public ReceiveSumEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _ReceiveType = null;
		/// <summary>
		/// 代收類別代碼 (商家代號)
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
		/// 學年代號
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
		/// 學期代號
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
		/// 部別代號
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
		/// 代收費用別代號
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

		private string _SumId = null;
		/// <summary>
		/// 合計項目代號
		/// </summary>
		[FieldSpec(Field.SumId, true, FieldTypeEnum.VarChar, 20, false)]
		public string SumId
		{
			get
			{
				return _SumId;
			}
			set
			{
				_SumId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// 合計項目名稱
		/// </summary>
		[FieldSpec(Field.SumName, false, FieldTypeEnum.NVarChar, 40, true)]
		public string SumName
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目01 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive01, false, FieldTypeEnum.Char, 1, true)]
		public string Receive01
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目02 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive02, false, FieldTypeEnum.Char, 1, true)]
		public string Receive02
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目03 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive03, false, FieldTypeEnum.Char, 1, true)]
		public string Receive03
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目04 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive04, false, FieldTypeEnum.Char, 1, true)]
		public string Receive04
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目05 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive05, false, FieldTypeEnum.Char, 1, true)]
		public string Receive05
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目06 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive06, false, FieldTypeEnum.Char, 1, true)]
		public string Receive06
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目07 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive07, false, FieldTypeEnum.Char, 1, true)]
		public string Receive07
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目08 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive08, false, FieldTypeEnum.Char, 1, true)]
		public string Receive08
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目09 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive09, false, FieldTypeEnum.Char, 1, true)]
		public string Receive09
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目10 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive10, false, FieldTypeEnum.Char, 1, true)]
		public string Receive10
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目11 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive11, false, FieldTypeEnum.Char, 1, true)]
		public string Receive11
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目12 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive12, false, FieldTypeEnum.Char, 1, true)]
		public string Receive12
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目13 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive13, false, FieldTypeEnum.Char, 1, true)]
		public string Receive13
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目14 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive14, false, FieldTypeEnum.Char, 1, true)]
		public string Receive14
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目15 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive15, false, FieldTypeEnum.Char, 1, true)]
		public string Receive15
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目16 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive16, false, FieldTypeEnum.Char, 1, true)]
		public string Receive16
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目17 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive17, false, FieldTypeEnum.Char, 1, true)]
		public string Receive17
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目18 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive18, false, FieldTypeEnum.Char, 1, true)]
		public string Receive18
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目19 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive19, false, FieldTypeEnum.Char, 1, true)]
		public string Receive19
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目20 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive20, false, FieldTypeEnum.Char, 1, true)]
		public string Receive20
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目21 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive21, false, FieldTypeEnum.Char, 1, true)]
		public string Receive21
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目22 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive22, false, FieldTypeEnum.Char, 1, true)]
		public string Receive22
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目23 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive23, false, FieldTypeEnum.Char, 1, true)]
		public string Receive23
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目24 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive24, false, FieldTypeEnum.Char, 1, true)]
		public string Receive24
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目25 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive25, false, FieldTypeEnum.Char, 1, true)]
		public string Receive25
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目26 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive26, false, FieldTypeEnum.Char, 1, true)]
		public string Receive26
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目27 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive27, false, FieldTypeEnum.Char, 1, true)]
		public string Receive27
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目28 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive28, false, FieldTypeEnum.Char, 1, true)]
		public string Receive28
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目29 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive29, false, FieldTypeEnum.Char, 1, true)]
		public string Receive29
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目30 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive30, false, FieldTypeEnum.Char, 1, true)]
		public string Receive30
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目31 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive31, false, FieldTypeEnum.Char, 1, true)]
		public string Receive31
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目32 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive32, false, FieldTypeEnum.Char, 1, true)]
		public string Receive32
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目33 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive33, false, FieldTypeEnum.Char, 1, true)]
		public string Receive33
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目34 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive34, false, FieldTypeEnum.Char, 1, true)]
		public string Receive34
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目35 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive35, false, FieldTypeEnum.Char, 1, true)]
		public string Receive35
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目36 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive36, false, FieldTypeEnum.Char, 1, true)]
		public string Receive36
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目37 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive37, false, FieldTypeEnum.Char, 1, true)]
		public string Receive37
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目38 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive38, false, FieldTypeEnum.Char, 1, true)]
		public string Receive38
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目39 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive39, false, FieldTypeEnum.Char, 1, true)]
		public string Receive39
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目40 是否要計算期標 (Y=是 / N=否)
		/// </summary>
		[FieldSpec(Field.Receive40, false, FieldTypeEnum.Char, 1, true)]
		public string Receive40
		{
			get;
			set;
		}

		#region [MDY:20220808] 2022擴充案 合計項目英文名稱 新增欄位並調整為 NVarChar(140)
		/// <summary>
		/// 合計項目英文名稱
		/// </summary>
		[FieldSpec(Field.SumEName, false, FieldTypeEnum.NVarChar, 140, true)]
		public string SumEName
		{
			get;
			set;
		}
		#endregion
		#endregion

		#region 狀態相關欄位
		/// <summary>
		/// 狀態
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
		public string Status
		{
			get;
			set;
		}

		/// <summary>
		/// 建立日期
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get;
			set;
		}

		/// <summary>
		/// 建立者
		/// </summary>
		[FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
		public string CrtUser
		{
			get;
			set;
		}

		/// <summary>
		/// 最後修改日期
		/// </summary>
		[FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? MdyDate
		{
			get;
			set;
		}

		/// <summary>
		/// 最後修改者
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
        #region [NEW:20151229] 取得要計算合計項目的收入科目編號陣列
        /// <summary>
        /// 取得要計算合計項目的收入科目編號陣列
        /// </summary>
        /// <returns></returns>
        public int[] GetSumReceiveItemNos()
        {
            string[] itemValues = new string[] {
                this.Receive01, this.Receive02, this.Receive03, this.Receive04, this.Receive05, this.Receive06, this.Receive07, this.Receive08, this.Receive09, this.Receive10,
                this.Receive11, this.Receive12, this.Receive13, this.Receive14, this.Receive15, this.Receive16, this.Receive17, this.Receive18, this.Receive19, this.Receive20,
                this.Receive21, this.Receive22, this.Receive23, this.Receive24, this.Receive25, this.Receive26, this.Receive27, this.Receive28, this.Receive29, this.Receive30,
                this.Receive31, this.Receive32, this.Receive33, this.Receive34, this.Receive35, this.Receive36, this.Receive37, this.Receive38, this.Receive39, this.Receive40
            };
            List<int> itemNos = new List<int>(itemValues.Length);
            int itemNo = 0;
            foreach (string itemValue in itemValues)
            {
                itemNo++;
                if (itemValue != null && itemValue.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                {
                    itemNos.Add(itemNo);
                }
            }
            return itemNos.ToArray();
        }
        #endregion

		#region [MDY:202203XX] 2022擴充案 改寫
		public string[] GetAllReceives()
		{
			return new string[40] 
			{
				this.Receive01, this.Receive02, this.Receive03, this.Receive04, this.Receive05,
				this.Receive06, this.Receive07, this.Receive08, this.Receive09, this.Receive10,
				this.Receive11, this.Receive12, this.Receive13, this.Receive14, this.Receive15,
				this.Receive16, this.Receive17, this.Receive18, this.Receive19, this.Receive20,
				this.Receive21, this.Receive22, this.Receive23, this.Receive24, this.Receive25,
				this.Receive26, this.Receive27, this.Receive28, this.Receive29, this.Receive30,
				this.Receive31, this.Receive32, this.Receive33, this.Receive34, this.Receive35,
				this.Receive36, this.Receive37, this.Receive38, this.Receive39, this.Receive40,
			};
		}

		public string GetReceiveByNo(int no)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					return this.Receive01;
				case 02:
					return this.Receive02;
				case 03:
					return this.Receive03;
				case 04:
					return this.Receive04;
				case 05:
					return this.Receive05;
				case 06:
					return this.Receive06;
				case 07:
					return this.Receive07;
				case 08:
					return this.Receive08;
				case 09:
					return this.Receive09;
				case 10:
					return this.Receive10;
				#endregion

				#region 11 ~ 20
				case 11:
					return this.Receive11;
				case 12:
					return this.Receive12;
				case 13:
					return this.Receive13;
				case 14:
					return this.Receive14;
				case 15:
					return this.Receive15;
				case 16:
					return this.Receive16;
				case 17:
					return this.Receive17;
				case 18:
					return this.Receive18;
				case 19:
					return this.Receive19;
				case 20:
					return this.Receive20;
				#endregion

				#region 21 ~ 30
				case 21:
					return this.Receive21;
				case 22:
					return this.Receive22;
				case 23:
					return this.Receive23;
				case 24:
					return this.Receive24;
				case 25:
					return this.Receive25;
				case 26:
					return this.Receive26;
				case 27:
					return this.Receive27;
				case 28:
					return this.Receive28;
				case 29:
					return this.Receive29;
				case 30:
					return this.Receive30;
				#endregion

				#region 31 ~ 40
				case 31:
					return this.Receive31;
				case 32:
					return this.Receive32;
				case 33:
					return this.Receive33;
				case 34:
					return this.Receive34;
				case 35:
					return this.Receive35;
				case 36:
					return this.Receive36;
				case 37:
					return this.Receive37;
				case 38:
					return this.Receive38;
				case 39:
					return this.Receive39;
				case 40:
					return this.Receive40;
				#endregion

				default:
					return null;
			}
		}

		public bool SetReceiveByNo(int no, string value)
        {
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					this.Receive01 = value;
					return true;
				case 02:
					this.Receive02 = value;
					return true;
				case 03:
					this.Receive03 = value;
					return true;
				case 04:
					this.Receive04 = value;
					return true;
				case 05:
					this.Receive05 = value;
					return true;
				case 06:
					this.Receive06 = value;
					return true;
				case 07:
					this.Receive07 = value;
					return true;
				case 08:
					this.Receive08 = value;
					return true;
				case 09:
					this.Receive09 = value;
					return true;
				case 10:
					this.Receive10 = value;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					this.Receive11 = value;
					return true;
				case 12:
					this.Receive12 = value;
					return true;
				case 13:
					this.Receive13 = value;
					return true;
				case 14:
					this.Receive14 = value;
					return true;
				case 15:
					this.Receive15 = value;
					return true;
				case 16:
					this.Receive16 = value;
					return true;
				case 17:
					this.Receive17 = value;
					return true;
				case 18:
					this.Receive18 = value;
					return true;
				case 19:
					this.Receive19 = value;
					return true;
				case 20:
					this.Receive20 = value;
					return true;
				#endregion

				#region 21 ~ 30
				case 21:
					this.Receive21 = value;
					return true;
				case 22:
					this.Receive22 = value;
					return true;
				case 23:
					this.Receive23 = value;
					return true;
				case 24:
					this.Receive24 = value;
					return true;
				case 25:
					this.Receive25 = value;
					return true;
				case 26:
					this.Receive26 = value;
					return true;
				case 27:
					this.Receive27 = value;
					return true;
				case 28:
					this.Receive28 = value;
					return true;
				case 29:
					this.Receive29 = value;
					return true;
				case 30:
					this.Receive30 = value;
					return true;
				#endregion

				#region 31 ~ 40
				case 31:
					this.Receive31 = value;
					return true;
				case 32:
					this.Receive32 = value;
					return true;
				case 33:
					this.Receive33 = value;
					return true;
				case 34:
					this.Receive34 = value;
					return true;
				case 35:
					this.Receive35 = value;
					return true;
				case 36:
					this.Receive36 = value;
					return true;
				case 37:
					this.Receive37 = value;
					return true;
				case 38:
					this.Receive38 = value;
					return true;
				case 39:
					this.Receive39 = value;
					return true;
				case 40:
					this.Receive40 = value;
					return true;
				#endregion

				default:
					return false;
			}
		}
		#endregion
		#endregion

	}
}
