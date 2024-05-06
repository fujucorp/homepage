/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:33:40
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
	/// Reduce_Standard 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class ReduceStandardEntity : Entity
	{
		public const string TABLE_NAME = "Reduce_Standard";

		#region Field Name Const Class
		/// <summary>
		/// ReduceStandardEntity 欄位名稱定義抽象類別
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
			/// Reduce_Id 欄位名稱常數定義
			/// </summary>
			public const string ReduceId = "Reduce_Id";
			#endregion

			#region Data
			/// <summary>
			/// Reduce_Way 欄位名稱常數定義
            /// 1:依百分比
            /// 2:依金額
            /// 3:依順序
			/// </summary>
			public const string ReduceWay = "Reduce_Way";

			/// <summary>
			/// Reduce_Total 欄位名稱常數定義
			/// </summary>
			public const string ReduceTotal = "Reduce_Total";

			/// <summary>
			/// Num_01 欄位名稱常數定義
			/// </summary>
			public const string Num01 = "Num_01";

			/// <summary>
			/// Dno_01 欄位名稱常數定義
			/// </summary>
			public const string Dno01 = "Dno_01";

			/// <summary>
			/// Num_02 欄位名稱常數定義
			/// </summary>
			public const string Num02 = "Num_02";

			/// <summary>
			/// Dno_02 欄位名稱常數定義
			/// </summary>
			public const string Dno02 = "Dno_02";

			/// <summary>
			/// Num_03 欄位名稱常數定義
			/// </summary>
			public const string Num03 = "Num_03";

			/// <summary>
			/// Dno_03 欄位名稱常數定義
			/// </summary>
			public const string Dno03 = "Dno_03";

			/// <summary>
			/// Num_04 欄位名稱常數定義
			/// </summary>
			public const string Num04 = "Num_04";

			/// <summary>
			/// Dno_04 欄位名稱常數定義
			/// </summary>
			public const string Dno04 = "Dno_04";

			/// <summary>
			/// Num_05 欄位名稱常數定義
			/// </summary>
			public const string Num05 = "Num_05";

			/// <summary>
			/// Dno_05 欄位名稱常數定義
			/// </summary>
			public const string Dno05 = "Dno_05";

			/// <summary>
			/// Num_06 欄位名稱常數定義
			/// </summary>
			public const string Num06 = "Num_06";

			/// <summary>
			/// Dno_06 欄位名稱常數定義
			/// </summary>
			public const string Dno06 = "Dno_06";

			/// <summary>
			/// Num_07 欄位名稱常數定義
			/// </summary>
			public const string Num07 = "Num_07";

			/// <summary>
			/// Dno_07 欄位名稱常數定義
			/// </summary>
			public const string Dno07 = "Dno_07";

			/// <summary>
			/// Num_08 欄位名稱常數定義
			/// </summary>
			public const string Num08 = "Num_08";

			/// <summary>
			/// Dno_08 欄位名稱常數定義
			/// </summary>
			public const string Dno08 = "Dno_08";

			/// <summary>
			/// Num_09 欄位名稱常數定義
			/// </summary>
			public const string Num09 = "Num_09";

			/// <summary>
			/// Dno_09 欄位名稱常數定義
			/// </summary>
			public const string Dno09 = "Dno_09";

			/// <summary>
			/// Num_10 欄位名稱常數定義
			/// </summary>
			public const string Num10 = "Num_10";

			/// <summary>
			/// Dno_10 欄位名稱常數定義
			/// </summary>
			public const string Dno10 = "Dno_10";

			/// <summary>
			/// Num_11 欄位名稱常數定義
			/// </summary>
			public const string Num11 = "Num_11";

			/// <summary>
			/// Dno_11 欄位名稱常數定義
			/// </summary>
			public const string Dno11 = "Dno_11";

			/// <summary>
			/// Num_12 欄位名稱常數定義
			/// </summary>
			public const string Num12 = "Num_12";

			/// <summary>
			/// Dno_12 欄位名稱常數定義
			/// </summary>
			public const string Dno12 = "Dno_12";

			/// <summary>
			/// Num_13 欄位名稱常數定義
			/// </summary>
			public const string Num13 = "Num_13";

			/// <summary>
			/// Dno_13 欄位名稱常數定義
			/// </summary>
			public const string Dno13 = "Dno_13";

			/// <summary>
			/// Num_14 欄位名稱常數定義
			/// </summary>
			public const string Num14 = "Num_14";

			/// <summary>
			/// Dno_14 欄位名稱常數定義
			/// </summary>
			public const string Dno14 = "Dno_14";

			/// <summary>
			/// Num_15 欄位名稱常數定義
			/// </summary>
			public const string Num15 = "Num_15";

			/// <summary>
			/// Dno_15 欄位名稱常數定義
			/// </summary>
			public const string Dno15 = "Dno_15";

			/// <summary>
			/// Num_16 欄位名稱常數定義
			/// </summary>
			public const string Num16 = "Num_16";

			/// <summary>
			/// Dno_16 欄位名稱常數定義
			/// </summary>
			public const string Dno16 = "Dno_16";

			/// <summary>
			/// Num_17 欄位名稱常數定義
			/// </summary>
			public const string Num17 = "Num_17";

			/// <summary>
			/// Dno_17 欄位名稱常數定義
			/// </summary>
			public const string Dno17 = "Dno_17";

			/// <summary>
			/// Num_18 欄位名稱常數定義
			/// </summary>
			public const string Num18 = "Num_18";

			/// <summary>
			/// Dno_18 欄位名稱常數定義
			/// </summary>
			public const string Dno18 = "Dno_18";

			/// <summary>
			/// Num_19 欄位名稱常數定義
			/// </summary>
			public const string Num19 = "Num_19";

			/// <summary>
			/// Dno_19 欄位名稱常數定義
			/// </summary>
			public const string Dno19 = "Dno_19";

			/// <summary>
			/// Num_20 欄位名稱常數定義
			/// </summary>
			public const string Num20 = "Num_20";

			/// <summary>
			/// Dno_20 欄位名稱常數定義
			/// </summary>
			public const string Dno20 = "Dno_20";

			/// <summary>
			/// Num_21 欄位名稱常數定義
			/// </summary>
			public const string Num21 = "Num_21";

			/// <summary>
			/// Dno_21 欄位名稱常數定義
			/// </summary>
			public const string Dno21 = "Dno_21";

			/// <summary>
			/// Num_22 欄位名稱常數定義
			/// </summary>
			public const string Num22 = "Num_22";

			/// <summary>
			/// Dno_22 欄位名稱常數定義
			/// </summary>
			public const string Dno22 = "Dno_22";

			/// <summary>
			/// Num_23 欄位名稱常數定義
			/// </summary>
			public const string Num23 = "Num_23";

			/// <summary>
			/// Dno_23 欄位名稱常數定義
			/// </summary>
			public const string Dno23 = "Dno_23";

			/// <summary>
			/// Num_24 欄位名稱常數定義
			/// </summary>
			public const string Num24 = "Num_24";

			/// <summary>
			/// Dno_24 欄位名稱常數定義
			/// </summary>
			public const string Dno24 = "Dno_24";

			/// <summary>
			/// Num_25 欄位名稱常數定義
			/// </summary>
			public const string Num25 = "Num_25";

			/// <summary>
			/// Dno_25 欄位名稱常數定義
			/// </summary>
			public const string Dno25 = "Dno_25";

			/// <summary>
			/// Num_26 欄位名稱常數定義
			/// </summary>
			public const string Num26 = "Num_26";

			/// <summary>
			/// Dno_26 欄位名稱常數定義
			/// </summary>
			public const string Dno26 = "Dno_26";

			/// <summary>
			/// Num_27 欄位名稱常數定義
			/// </summary>
			public const string Num27 = "Num_27";

			/// <summary>
			/// Dno_27 欄位名稱常數定義
			/// </summary>
			public const string Dno27 = "Dno_27";

			/// <summary>
			/// Num_28 欄位名稱常數定義
			/// </summary>
			public const string Num28 = "Num_28";

			/// <summary>
			/// Dno_28 欄位名稱常數定義
			/// </summary>
			public const string Dno28 = "Dno_28";

			/// <summary>
			/// Num_29 欄位名稱常數定義
			/// </summary>
			public const string Num29 = "Num_29";

			/// <summary>
			/// Dno_29 欄位名稱常數定義
			/// </summary>
			public const string Dno29 = "Dno_29";

			/// <summary>
			/// Num_30 欄位名稱常數定義
			/// </summary>
			public const string Num30 = "Num_30";

			/// <summary>
			/// Dno_30 欄位名稱常數定義
			/// </summary>
			public const string Dno30 = "Dno_30";

			/// <summary>
			/// Num_31 欄位名稱常數定義
			/// </summary>
			public const string Num31 = "Num_31";

			/// <summary>
			/// Dno_31 欄位名稱常數定義
			/// </summary>
			public const string Dno31 = "Dno_31";

			/// <summary>
			/// Num_32 欄位名稱常數定義
			/// </summary>
			public const string Num32 = "Num_32";

			/// <summary>
			/// Dno_32 欄位名稱常數定義
			/// </summary>
			public const string Dno32 = "Dno_32";

			/// <summary>
			/// Num_33 欄位名稱常數定義
			/// </summary>
			public const string Num33 = "Num_33";

			/// <summary>
			/// Dno_33 欄位名稱常數定義
			/// </summary>
			public const string Dno33 = "Dno_33";

			/// <summary>
			/// Num_34 欄位名稱常數定義
			/// </summary>
			public const string Num34 = "Num_34";

			/// <summary>
			/// Dno_34 欄位名稱常數定義
			/// </summary>
			public const string Dno34 = "Dno_34";

			/// <summary>
			/// Num_35 欄位名稱常數定義
			/// </summary>
			public const string Num35 = "Num_35";

			/// <summary>
			/// Dno_35 欄位名稱常數定義
			/// </summary>
			public const string Dno35 = "Dno_35";

			/// <summary>
			/// Num_36 欄位名稱常數定義
			/// </summary>
			public const string Num36 = "Num_36";

			/// <summary>
			/// Dno_36 欄位名稱常數定義
			/// </summary>
			public const string Dno36 = "Dno_36";

			/// <summary>
			/// Num_37 欄位名稱常數定義
			/// </summary>
			public const string Num37 = "Num_37";

			/// <summary>
			/// Dno_37 欄位名稱常數定義
			/// </summary>
			public const string Dno37 = "Dno_37";

			/// <summary>
			/// Num_38 欄位名稱常數定義
			/// </summary>
			public const string Num38 = "Num_38";

			/// <summary>
			/// Dno_38 欄位名稱常數定義
			/// </summary>
			public const string Dno38 = "Dno_38";

			/// <summary>
			/// Num_39 欄位名稱常數定義
			/// </summary>
			public const string Num39 = "Num_39";

			/// <summary>
			/// Dno_39 欄位名稱常數定義
			/// </summary>
			public const string Dno39 = "Dno_39";

			/// <summary>
			/// Num_40 欄位名稱常數定義
			/// </summary>
			public const string Num40 = "Num_40";

			/// <summary>
			/// Dno_40 欄位名稱常數定義
			/// </summary>
			public const string Dno40 = "Dno_40";

			/// <summary>
			/// Reduce_Amount01 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount01 = "Reduce_Amount01";

			/// <summary>
			/// Reduce_Amount02 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount02 = "Reduce_Amount02";

			/// <summary>
			/// Reduce_Amount03 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount03 = "Reduce_Amount03";

			/// <summary>
			/// Reduce_Amount04 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount04 = "Reduce_Amount04";

			/// <summary>
			/// Reduce_Amount05 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount05 = "Reduce_Amount05";

			/// <summary>
			/// Reduce_Amount06 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount06 = "Reduce_Amount06";

			/// <summary>
			/// Reduce_Amount07 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount07 = "Reduce_Amount07";

			/// <summary>
			/// Reduce_Amount08 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount08 = "Reduce_Amount08";

			/// <summary>
			/// Reduce_Amount09 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount09 = "Reduce_Amount09";

			/// <summary>
			/// Reduce_Amount10 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount10 = "Reduce_Amount10";

			/// <summary>
			/// Reduce_Amount11 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount11 = "Reduce_Amount11";

			/// <summary>
			/// Reduce_Amount12 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount12 = "Reduce_Amount12";

			/// <summary>
			/// Reduce_Amount13 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount13 = "Reduce_Amount13";

			/// <summary>
			/// Reduce_Amount14 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount14 = "Reduce_Amount14";

			/// <summary>
			/// Reduce_Amount15 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount15 = "Reduce_Amount15";

			/// <summary>
			/// Reduce_Amount16 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount16 = "Reduce_Amount16";

			/// <summary>
			/// Reduce_Amount17 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount17 = "Reduce_Amount17";

			/// <summary>
			/// Reduce_Amount18 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount18 = "Reduce_Amount18";

			/// <summary>
			/// Reduce_Amount19 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount19 = "Reduce_Amount19";

			/// <summary>
			/// Reduce_Amount20 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount20 = "Reduce_Amount20";

			/// <summary>
			/// Reduce_Amount21 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount21 = "Reduce_Amount21";

			/// <summary>
			/// Reduce_Amount22 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount22 = "Reduce_Amount22";

			/// <summary>
			/// Reduce_Amount23 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount23 = "Reduce_Amount23";

			/// <summary>
			/// Reduce_Amount24 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount24 = "Reduce_Amount24";

			/// <summary>
			/// Reduce_Amount25 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount25 = "Reduce_Amount25";

			/// <summary>
			/// Reduce_Amount26 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount26 = "Reduce_Amount26";

			/// <summary>
			/// Reduce_Amount27 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount27 = "Reduce_Amount27";

			/// <summary>
			/// Reduce_Amount28 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount28 = "Reduce_Amount28";

			/// <summary>
			/// Reduce_Amount29 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount29 = "Reduce_Amount29";

			/// <summary>
			/// Reduce_Amount30 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount30 = "Reduce_Amount30";

			/// <summary>
			/// Reduce_Amount31 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount31 = "Reduce_Amount31";

			/// <summary>
			/// Reduce_Amount32 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount32 = "Reduce_Amount32";

			/// <summary>
			/// Reduce_Amount33 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount33 = "Reduce_Amount33";

			/// <summary>
			/// Reduce_Amount34 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount34 = "Reduce_Amount34";

			/// <summary>
			/// Reduce_Amount35 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount35 = "Reduce_Amount35";

			/// <summary>
			/// Reduce_Amount36 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount36 = "Reduce_Amount36";

			/// <summary>
			/// Reduce_Amount37 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount37 = "Reduce_Amount37";

			/// <summary>
			/// Reduce_Amount38 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount38 = "Reduce_Amount38";

			/// <summary>
			/// Reduce_Amount39 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount39 = "Reduce_Amount39";

			/// <summary>
			/// Reduce_Amount40 欄位名稱常數定義
			/// </summary>
			public const string ReduceAmount40 = "Reduce_Amount40";

			/// <summary>
			/// Reduce_order1 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder1 = "Reduce_order1";

			/// <summary>
			/// Reduce_order2 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder2 = "Reduce_order2";

			/// <summary>
			/// Reduce_order3 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder3 = "Reduce_order3";

			/// <summary>
			/// Reduce_order4 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder4 = "Reduce_order4";

			/// <summary>
			/// Reduce_order5 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder5 = "Reduce_order5";

			/// <summary>
			/// Reduce_order6 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder6 = "Reduce_order6";

			/// <summary>
			/// Reduce_order7 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder7 = "Reduce_order7";

			/// <summary>
			/// Reduce_order8 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder8 = "Reduce_order8";

			/// <summary>
			/// Reduce_order9 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder9 = "Reduce_order9";

			/// <summary>
			/// Reduce_order10 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder10 = "Reduce_order10";

			/// <summary>
			/// Reduce_order11 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder11 = "Reduce_order11";

			/// <summary>
			/// Reduce_order12 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder12 = "Reduce_order12";

			/// <summary>
			/// Reduce_order13 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder13 = "Reduce_order13";

			/// <summary>
			/// Reduce_order14 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder14 = "Reduce_order14";

			/// <summary>
			/// Reduce_order15 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder15 = "Reduce_order15";

			/// <summary>
			/// Reduce_order16 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder16 = "Reduce_order16";

			/// <summary>
			/// Reduce_order17 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder17 = "Reduce_order17";

			/// <summary>
			/// Reduce_order18 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder18 = "Reduce_order18";

			/// <summary>
			/// Reduce_order19 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder19 = "Reduce_order19";

			/// <summary>
			/// Reduce_order20 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder20 = "Reduce_order20";

			/// <summary>
			/// Reduce_order21 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder21 = "Reduce_order21";

			/// <summary>
			/// Reduce_order22 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder22 = "Reduce_order22";

			/// <summary>
			/// Reduce_order23 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder23 = "Reduce_order23";

			/// <summary>
			/// Reduce_order24 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder24 = "Reduce_order24";

			/// <summary>
			/// Reduce_order25 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder25 = "Reduce_order25";

			/// <summary>
			/// Reduce_order26 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder26 = "Reduce_order26";

			/// <summary>
			/// Reduce_order27 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder27 = "Reduce_order27";

			/// <summary>
			/// Reduce_order28 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder28 = "Reduce_order28";

			/// <summary>
			/// Reduce_order29 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder29 = "Reduce_order29";

			/// <summary>
			/// Reduce_order30 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder30 = "Reduce_order30";

			/// <summary>
			/// Reduce_order31 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder31 = "Reduce_order31";

			/// <summary>
			/// Reduce_order32 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder32 = "Reduce_order32";

			/// <summary>
			/// Reduce_order33 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder33 = "Reduce_order33";

			/// <summary>
			/// Reduce_order34 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder34 = "Reduce_order34";

			/// <summary>
			/// Reduce_order35 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder35 = "Reduce_order35";

			/// <summary>
			/// Reduce_order36 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder36 = "Reduce_order36";

			/// <summary>
			/// Reduce_order37 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder37 = "Reduce_order37";

			/// <summary>
			/// Reduce_order38 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder38 = "Reduce_order38";

			/// <summary>
			/// Reduce_order39 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder39 = "Reduce_order39";

			/// <summary>
			/// Reduce_order40 欄位名稱常數定義
			/// </summary>
			public const string ReduceOrder40 = "Reduce_order40";

			/// <summary>
			/// Reduce_Item 欄位名稱常數定義
			/// </summary>
			public const string ReduceItem = "Reduce_Item";

			/// <summary>
			/// limit_01 欄位名稱常數定義
			/// </summary>
			public const string Limit01 = "limit_01";

			/// <summary>
			/// limit_02 欄位名稱常數定義
			/// </summary>
			public const string Limit02 = "limit_02";

			/// <summary>
			/// limit_03 欄位名稱常數定義
			/// </summary>
			public const string Limit03 = "limit_03";

			/// <summary>
			/// limit_04 欄位名稱常數定義
			/// </summary>
			public const string Limit04 = "limit_04";

			/// <summary>
			/// limit_05 欄位名稱常數定義
			/// </summary>
			public const string Limit05 = "limit_05";

			/// <summary>
			/// limit_06 欄位名稱常數定義
			/// </summary>
			public const string Limit06 = "limit_06";

			/// <summary>
			/// limit_07 欄位名稱常數定義
			/// </summary>
			public const string Limit07 = "limit_07";

			/// <summary>
			/// limit_08 欄位名稱常數定義
			/// </summary>
			public const string Limit08 = "limit_08";

			/// <summary>
			/// limit_09 欄位名稱常數定義
			/// </summary>
			public const string Limit09 = "limit_09";

			/// <summary>
			/// limit_10 欄位名稱常數定義
			/// </summary>
			public const string Limit10 = "limit_10";

			/// <summary>
			/// limit_11 欄位名稱常數定義
			/// </summary>
			public const string Limit11 = "limit_11";

			/// <summary>
			/// limit_12 欄位名稱常數定義
			/// </summary>
			public const string Limit12 = "limit_12";

			/// <summary>
			/// limit_13 欄位名稱常數定義
			/// </summary>
			public const string Limit13 = "limit_13";

			/// <summary>
			/// limit_14 欄位名稱常數定義
			/// </summary>
			public const string Limit14 = "limit_14";

			/// <summary>
			/// limit_15 欄位名稱常數定義
			/// </summary>
			public const string Limit15 = "limit_15";

			/// <summary>
			/// limit_16 欄位名稱常數定義
			/// </summary>
			public const string Limit16 = "limit_16";

			/// <summary>
			/// limit_17 欄位名稱常數定義
			/// </summary>
			public const string Limit17 = "limit_17";

			/// <summary>
			/// limit_18 欄位名稱常數定義
			/// </summary>
			public const string Limit18 = "limit_18";

			/// <summary>
			/// limit_19 欄位名稱常數定義
			/// </summary>
			public const string Limit19 = "limit_19";

			/// <summary>
			/// limit_20 欄位名稱常數定義
			/// </summary>
			public const string Limit20 = "limit_20";

			/// <summary>
			/// limit_21 欄位名稱常數定義
			/// </summary>
			public const string Limit21 = "limit_21";

			/// <summary>
			/// limit_22 欄位名稱常數定義
			/// </summary>
			public const string Limit22 = "limit_22";

			/// <summary>
			/// limit_23 欄位名稱常數定義
			/// </summary>
			public const string Limit23 = "limit_23";

			/// <summary>
			/// limit_24 欄位名稱常數定義
			/// </summary>
			public const string Limit24 = "limit_24";

			/// <summary>
			/// limit_25 欄位名稱常數定義
			/// </summary>
			public const string Limit25 = "limit_25";

			/// <summary>
			/// limit_26 欄位名稱常數定義
			/// </summary>
			public const string Limit26 = "limit_26";

			/// <summary>
			/// limit_27 欄位名稱常數定義
			/// </summary>
			public const string Limit27 = "limit_27";

			/// <summary>
			/// limit_28 欄位名稱常數定義
			/// </summary>
			public const string Limit28 = "limit_28";

			/// <summary>
			/// limit_29 欄位名稱常數定義
			/// </summary>
			public const string Limit29 = "limit_29";

			/// <summary>
			/// limit_30 欄位名稱常數定義
			/// </summary>
			public const string Limit30 = "limit_30";

			/// <summary>
			/// limit_31 欄位名稱常數定義
			/// </summary>
			public const string Limit31 = "limit_31";

			/// <summary>
			/// limit_32 欄位名稱常數定義
			/// </summary>
			public const string Limit32 = "limit_32";

			/// <summary>
			/// limit_33 欄位名稱常數定義
			/// </summary>
			public const string Limit33 = "limit_33";

			/// <summary>
			/// limit_34 欄位名稱常數定義
			/// </summary>
			public const string Limit34 = "limit_34";

			/// <summary>
			/// limit_35 欄位名稱常數定義
			/// </summary>
			public const string Limit35 = "limit_35";

			/// <summary>
			/// limit_36 欄位名稱常數定義
			/// </summary>
			public const string Limit36 = "limit_36";

			/// <summary>
			/// limit_37 欄位名稱常數定義
			/// </summary>
			public const string Limit37 = "limit_37";

			/// <summary>
			/// limit_38 欄位名稱常數定義
			/// </summary>
			public const string Limit38 = "limit_38";

			/// <summary>
			/// limit_39 欄位名稱常數定義
			/// </summary>
			public const string Limit39 = "limit_39";

			/// <summary>
			/// limit_40 欄位名稱常數定義
			/// </summary>
			public const string Limit40 = "limit_40";

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
		/// ReduceStandardEntity 類別建構式
		/// </summary>
		public ReduceStandardEntity()
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

		private string _ReduceId = null;
		/// <summary>
		/// Reduce_Id 欄位屬性
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
		/// Reduce_Way 欄位屬性
        /// 1:依百分比
        /// 2:依金額
        /// 3:依順序
		/// </summary>
		[FieldSpec(Field.ReduceWay, false, FieldTypeEnum.Char, 1, true)]
		public string ReduceWay
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Total 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceTotal, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceTotal
		{
			get;
			set;
		}

		/// <summary>
		/// Num_01 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num01, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num01
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_01 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno01, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno01
		{
			get;
			set;
		}

		/// <summary>
		/// Num_02 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num02, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num02
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_02 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno02, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno02
		{
			get;
			set;
		}

		/// <summary>
		/// Num_03 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num03, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num03
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_03 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno03, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno03
		{
			get;
			set;
		}

		/// <summary>
		/// Num_04 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num04, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num04
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_04 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno04, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno04
		{
			get;
			set;
		}

		/// <summary>
		/// Num_05 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num05, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num05
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_05 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno05, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno05
		{
			get;
			set;
		}

		/// <summary>
		/// Num_06 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num06, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num06
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_06 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno06, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno06
		{
			get;
			set;
		}

		/// <summary>
		/// Num_07 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num07, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num07
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_07 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno07, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno07
		{
			get;
			set;
		}

		/// <summary>
		/// Num_08 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num08, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num08
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_08 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno08, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno08
		{
			get;
			set;
		}

		/// <summary>
		/// Num_09 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num09, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num09
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_09 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno09, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno09
		{
			get;
			set;
		}

		/// <summary>
		/// Num_10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num10, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num10
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno10, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno10
		{
			get;
			set;
		}

		/// <summary>
		/// Num_11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num11, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num11
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno11, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno11
		{
			get;
			set;
		}

		/// <summary>
		/// Num_12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num12, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num12
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno12, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno12
		{
			get;
			set;
		}

		/// <summary>
		/// Num_13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num13, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num13
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno13, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno13
		{
			get;
			set;
		}

		/// <summary>
		/// Num_14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num14, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num14
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno14, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno14
		{
			get;
			set;
		}

		/// <summary>
		/// Num_15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num15, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num15
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno15, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno15
		{
			get;
			set;
		}

		/// <summary>
		/// Num_16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num16, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num16
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno16, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno16
		{
			get;
			set;
		}

		/// <summary>
		/// Num_17 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num17, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num17
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_17 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno17, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno17
		{
			get;
			set;
		}

		/// <summary>
		/// Num_18 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num18, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num18
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_18 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno18, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno18
		{
			get;
			set;
		}

		/// <summary>
		/// Num_19 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num19, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num19
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_19 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno19, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno19
		{
			get;
			set;
		}

		/// <summary>
		/// Num_20 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num20, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num20
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_20 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno20, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno20
		{
			get;
			set;
		}

		/// <summary>
		/// Num_21 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num21, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num21
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_21 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno21, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno21
		{
			get;
			set;
		}

		/// <summary>
		/// Num_22 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num22, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num22
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_22 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno22, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno22
		{
			get;
			set;
		}

		/// <summary>
		/// Num_23 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num23, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num23
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_23 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno23, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno23
		{
			get;
			set;
		}

		/// <summary>
		/// Num_24 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num24, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num24
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_24 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno24, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno24
		{
			get;
			set;
		}

		/// <summary>
		/// Num_25 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num25, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num25
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_25 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno25, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno25
		{
			get;
			set;
		}

		/// <summary>
		/// Num_26 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num26, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num26
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_26 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno26, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno26
		{
			get;
			set;
		}

		/// <summary>
		/// Num_27 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num27, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num27
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_27 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno27, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno27
		{
			get;
			set;
		}

		/// <summary>
		/// Num_28 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num28, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num28
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_28 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno28, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno28
		{
			get;
			set;
		}

		/// <summary>
		/// Num_29 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num29, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num29
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_29 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno29, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno29
		{
			get;
			set;
		}

		/// <summary>
		/// Num_30 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num30, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num30
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_30 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno30, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno30
		{
			get;
			set;
		}

		/// <summary>
		/// Num_31 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num31, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num31
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_31 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno31, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno31
		{
			get;
			set;
		}

		/// <summary>
		/// Num_32 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num32, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num32
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_32 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno32, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno32
		{
			get;
			set;
		}

		/// <summary>
		/// Num_33 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num33, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num33
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_33 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno33, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno33
		{
			get;
			set;
		}

		/// <summary>
		/// Num_34 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num34, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num34
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_34 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno34, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno34
		{
			get;
			set;
		}

		/// <summary>
		/// Num_35 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num35, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num35
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_35 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno35, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno35
		{
			get;
			set;
		}

		/// <summary>
		/// Num_36 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num36, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num36
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_36 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno36, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno36
		{
			get;
			set;
		}

		/// <summary>
		/// Num_37 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num37, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num37
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_37 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno37, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno37
		{
			get;
			set;
		}

		/// <summary>
		/// Num_38 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num38, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num38
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_38 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno38, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno38
		{
			get;
			set;
		}

		/// <summary>
		/// Num_39 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num39, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num39
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_39 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno39, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno39
		{
			get;
			set;
		}

		/// <summary>
		/// Num_40 欄位屬性
		/// </summary>
		[FieldSpec(Field.Num40, false, FieldTypeEnum.Decimal, true)]
		public decimal? Num40
		{
			get;
			set;
		}

		/// <summary>
		/// Dno_40 欄位屬性
		/// </summary>
		[FieldSpec(Field.Dno40, false, FieldTypeEnum.Decimal, true)]
		public decimal? Dno40
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount01 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount01, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount01
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount02 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount02, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount02
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount03 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount03, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount03
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount04 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount04, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount04
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount05 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount05, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount05
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount06 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount06, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount06
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount07 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount07, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount07
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount08 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount08, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount08
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount09 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount09, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount09
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount10 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount10, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount10
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount11 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount11, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount11
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount12 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount12, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount12
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount13 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount13, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount13
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount14 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount14, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount14
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount15 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount15, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount15
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount16 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount16, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount16
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount17 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount17, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount17
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount18 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount18, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount18
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount19 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount19, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount19
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount20 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount20, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount20
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount21 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount21, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount21
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount22 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount22, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount22
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount23 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount23, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount23
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount24 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount24, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount24
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount25 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount25, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount25
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount26 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount26, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount26
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount27 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount27, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount27
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount28 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount28, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount28
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount29 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount29, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount29
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount30 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount30, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount30
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount31 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount31, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount31
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount32 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount32, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount32
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount33 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount33, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount33
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount34 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount34, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount34
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount35 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount35, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount35
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount36 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount36, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount36
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount37 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount37, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount37
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount38 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount38, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount38
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount39 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount39, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount39
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Amount40 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceAmount40, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReduceAmount40
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order1 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder1, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder1
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order2 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder2, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder2
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order3 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder3, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder3
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order4 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder4, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder4
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order5 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder5, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder5
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order6 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder6, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder6
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order7 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder7, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder7
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order8 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder8, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder8
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order9 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder9, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder9
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order10 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder10, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder10
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order11 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder11, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder11
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order12 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder12, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder12
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order13 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder13, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder13
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order14 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder14, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder14
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order15 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder15, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder15
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order16 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder16, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder16
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order17 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder17, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder17
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order18 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder18, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder18
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order19 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder19, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder19
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order20 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder20, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder20
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order21 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder21, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder21
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order22 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder22, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder22
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order23 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder23, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder23
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order24 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder24, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder24
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order25 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder25, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder25
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order26 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder26, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder26
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order27 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder27, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder27
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order28 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder28, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder28
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order29 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder29, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder29
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order30 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder30, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder30
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order31 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder31, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder31
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order32 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder32, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder32
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order33 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder33, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder33
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order34 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder34, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder34
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order35 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder35, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder35
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order36 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder36, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder36
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order37 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder37, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder37
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order38 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder38, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder38
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order39 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder39, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder39
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_order40 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceOrder40, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceOrder40
		{
			get;
			set;
		}

		/// <summary>
		/// Reduce_Item 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReduceItem, false, FieldTypeEnum.Char, 2, true)]
		public string ReduceItem
		{
			get;
			set;
		}

		/// <summary>
		/// limit_01 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit01, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit01
		{
			get;
			set;
		}

		/// <summary>
		/// limit_02 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit02, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit02
		{
			get;
			set;
		}

		/// <summary>
		/// limit_03 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit03, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit03
		{
			get;
			set;
		}

		/// <summary>
		/// limit_04 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit04, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit04
		{
			get;
			set;
		}

		/// <summary>
		/// limit_05 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit05, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit05
		{
			get;
			set;
		}

		/// <summary>
		/// limit_06 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit06, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit06
		{
			get;
			set;
		}

		/// <summary>
		/// limit_07 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit07, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit07
		{
			get;
			set;
		}

		/// <summary>
		/// limit_08 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit08, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit08
		{
			get;
			set;
		}

		/// <summary>
		/// limit_09 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit09, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit09
		{
			get;
			set;
		}

		/// <summary>
		/// limit_10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit10, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit10
		{
			get;
			set;
		}

		/// <summary>
		/// limit_11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit11, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit11
		{
			get;
			set;
		}

		/// <summary>
		/// limit_12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit12, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit12
		{
			get;
			set;
		}

		/// <summary>
		/// limit_13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit13, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit13
		{
			get;
			set;
		}

		/// <summary>
		/// limit_14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit14, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit14
		{
			get;
			set;
		}

		/// <summary>
		/// limit_15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit15, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit15
		{
			get;
			set;
		}

		/// <summary>
		/// limit_16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit16, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit16
		{
			get;
			set;
		}

		/// <summary>
		/// limit_17 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit17, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit17
		{
			get;
			set;
		}

		/// <summary>
		/// limit_18 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit18, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit18
		{
			get;
			set;
		}

		/// <summary>
		/// limit_19 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit19, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit19
		{
			get;
			set;
		}

		/// <summary>
		/// limit_20 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit20, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit20
		{
			get;
			set;
		}

		/// <summary>
		/// limit_21 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit21, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit21
		{
			get;
			set;
		}

		/// <summary>
		/// limit_22 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit22, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit22
		{
			get;
			set;
		}

		/// <summary>
		/// limit_23 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit23, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit23
		{
			get;
			set;
		}

		/// <summary>
		/// limit_24 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit24, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit24
		{
			get;
			set;
		}

		/// <summary>
		/// limit_25 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit25, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit25
		{
			get;
			set;
		}

		/// <summary>
		/// limit_26 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit26, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit26
		{
			get;
			set;
		}

		/// <summary>
		/// limit_27 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit27, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit27
		{
			get;
			set;
		}

		/// <summary>
		/// limit_28 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit28, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit28
		{
			get;
			set;
		}

		/// <summary>
		/// limit_29 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit29, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit29
		{
			get;
			set;
		}

		/// <summary>
		/// limit_30 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit30, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit30
		{
			get;
			set;
		}

		/// <summary>
		/// limit_31 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit31, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit31
		{
			get;
			set;
		}

		/// <summary>
		/// limit_32 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit32, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit32
		{
			get;
			set;
		}

		/// <summary>
		/// limit_33 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit33, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit33
		{
			get;
			set;
		}

		/// <summary>
		/// limit_34 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit34, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit34
		{
			get;
			set;
		}

		/// <summary>
		/// limit_35 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit35, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit35
		{
			get;
			set;
		}

		/// <summary>
		/// limit_36 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit36, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit36
		{
			get;
			set;
		}

		/// <summary>
		/// limit_37 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit37, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit37
		{
			get;
			set;
		}

		/// <summary>
		/// limit_38 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit38, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit38
		{
			get;
			set;
		}

		/// <summary>
		/// limit_39 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit39, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit39
		{
			get;
			set;
		}

		/// <summary>
		/// limit_40 欄位屬性
		/// </summary>
		[FieldSpec(Field.Limit40, false, FieldTypeEnum.Decimal, true)]
		public decimal? Limit40
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
        public decimal GetDenominator(Int32 index)
        {
            decimal Denominator=1;
            decimal defaultValue = 0;

            switch(index)
            {
                case 1:
                    Denominator = (this.Dno01 == null ? defaultValue : this.Dno01.Value);
                    break;
                case 2:
                    Denominator = (this.Dno02 == null ? defaultValue : this.Dno02.Value);
                    break;
                case 3:
                    Denominator = (this.Dno03 == null ? defaultValue : this.Dno03.Value);
                    break;
                case 4:
                    Denominator = (this.Dno04 == null ? defaultValue : this.Dno04.Value);
                    break;
                case 5:
                    Denominator = (this.Dno05 == null ? defaultValue : this.Dno05.Value);
                    break;
                case 6:
                    Denominator = (this.Dno06 == null ? defaultValue : this.Dno06.Value);
                    break;
                case 7:
                    Denominator = (this.Dno07 == null ? defaultValue : this.Dno07.Value);
                    break;
                case 8:
                    Denominator = (this.Dno08 == null ? defaultValue : this.Dno08.Value);
                    break;
                case 9:
                    Denominator = (this.Dno09 == null ? defaultValue : this.Dno09.Value);
                    break;
                case 10:
                    Denominator = (this.Dno10 == null ? defaultValue : this.Dno10.Value);
                    break;
                case 11:
                    Denominator = (this.Dno11 == null ? defaultValue : this.Dno11.Value);
                    break;
                case 12:
                    Denominator = (this.Dno12 == null ? defaultValue : this.Dno12.Value);
                    break;
                case 13:
                    Denominator = (this.Dno13 == null ? defaultValue : this.Dno13.Value);
                    break;
                case 14:
                    Denominator = (this.Dno14 == null ? defaultValue : this.Dno14.Value);
                    break;
                case 15:
                    Denominator = (this.Dno15 == null ? defaultValue : this.Dno15.Value);
                    break;
                case 16:
                    Denominator = (this.Dno16 == null ? defaultValue : this.Dno16.Value);
                    break;
                case 17:
                    Denominator = (this.Dno17 == null ? defaultValue : this.Dno17.Value);
                    break;
                case 18:
                    Denominator = (this.Dno18 == null ? defaultValue : this.Dno18.Value);
                    break;
                case 19:
                    Denominator = (this.Dno19 == null ? defaultValue : this.Dno19.Value);
                    break;
                case 20:
                    Denominator = (this.Dno20 == null ? defaultValue : this.Dno20.Value);
                    break;
                case 21:
                    Denominator = (this.Dno21 == null ? defaultValue : this.Dno21.Value);
                    break;
                case 22:
                    Denominator = (this.Dno22 == null ? defaultValue : this.Dno22.Value);
                    break;
                case 23:
                    Denominator = (this.Dno23 == null ? defaultValue : this.Dno23.Value);
                    break;
                case 24:
                    Denominator = (this.Dno24 == null ? defaultValue : this.Dno24.Value);
                    break;
                case 25:
                    Denominator = (this.Dno25 == null ? defaultValue : this.Dno25.Value);
                    break;
                case 26:
                    Denominator = (this.Dno26 == null ? defaultValue : this.Dno26.Value);
                    break;
                case 27:
                    Denominator = (this.Dno27 == null ? defaultValue : this.Dno27.Value);
                    break;
                case 28:
                    Denominator = (this.Dno28 == null ? defaultValue : this.Dno28.Value);
                    break;
                case 29:
                    Denominator = (this.Dno29 == null ? defaultValue : this.Dno29.Value);
                    break;
                case 30:
                    Denominator = (this.Dno30 == null ? defaultValue : this.Dno30.Value);
                    break;
                case 31:
                    Denominator = (this.Dno31 == null ? defaultValue : this.Dno31.Value);
                    break;
                case 32:
                    Denominator = (this.Dno32 == null ? defaultValue : this.Dno32.Value);
                    break;
                case 33:
                    Denominator = (this.Dno33 == null ? defaultValue : this.Dno33.Value);
                    break;
                case 34:
                    Denominator = (this.Dno34 == null ? defaultValue : this.Dno34.Value);
                    break;
                case 35:
                    Denominator = (this.Dno35 == null ? defaultValue : this.Dno35.Value);
                    break;
                case 36:
                    Denominator = (this.Dno36 == null ? defaultValue : this.Dno36.Value);
                    break;
                case 37:
                    Denominator = (this.Dno37 == null ? defaultValue : this.Dno37.Value);
                    break;
                case 38:
                    Denominator = (this.Dno38 == null ? defaultValue : this.Dno38.Value);
                    break;
                case 39:
                    Denominator = (this.Dno39 == null ? defaultValue : this.Dno39.Value);
                    break;
                case 40:
                    Denominator = (this.Dno40 == null ? defaultValue : this.Dno40.Value);
                    break;
            }

            return Denominator;
        }

        public decimal GetMolecular(Int32 index)
        {
            decimal molecular = 0;
            decimal defaultValue = 0;

            switch (index)
            {
                case 1:
                    molecular = (this.Num01 == null ? defaultValue : this.Num01.Value);
                    break;
                case 2:
                    molecular = (this.Num02 == null ? defaultValue : this.Num02.Value);
                    break;
                case 3:
                    molecular = (this.Num03 == null ? defaultValue : this.Num03.Value);
                    break;
                case 4:
                    molecular = (this.Num04 == null ? defaultValue : this.Num04.Value);
                    break;
                case 5:
                    molecular = (this.Num05 == null ? defaultValue : this.Num05.Value);
                    break;
                case 6:
                    molecular = (this.Num06 == null ? defaultValue : this.Num06.Value);
                    break;
                case 7:
                    molecular = (this.Num07 == null ? defaultValue : this.Num07.Value);
                    break;
                case 8:
                    molecular = (this.Num08 == null ? defaultValue : this.Num08.Value);
                    break;
                case 9:
                    molecular = (this.Num09 == null ? defaultValue : this.Num09.Value);
                    break;
                case 10:
                    molecular = (this.Num10 == null ? defaultValue : this.Num10.Value);
                    break;
                case 11:
                    molecular = (this.Num11 == null ? defaultValue : this.Num11.Value);
                    break;
                case 12:
                    molecular = (this.Num12 == null ? defaultValue : this.Num12.Value);
                    break;
                case 13:
                    molecular = (this.Num13 == null ? defaultValue : this.Num13.Value);
                    break;
                case 14:
                    molecular = (this.Num14 == null ? defaultValue : this.Num14.Value);
                    break;
                case 15:
                    molecular = (this.Num15 == null ? defaultValue : this.Num15.Value);
                    break;
                case 16:
                    molecular = (this.Num16 == null ? defaultValue : this.Num16.Value);
                    break;
                case 17:
                    molecular = (this.Num17 == null ? defaultValue : this.Num17.Value);
                    break;
                case 18:
                    molecular = (this.Num18 == null ? defaultValue : this.Num18.Value);
                    break;
                case 19:
                    molecular = (this.Num19 == null ? defaultValue : this.Num19.Value);
                    break;
                case 20:
                    molecular = (this.Num20 == null ? defaultValue : this.Num20.Value);
                    break;
                case 21:
                    molecular = (this.Num21 == null ? defaultValue : this.Num21.Value);
                    break;
                case 22:
                    molecular = (this.Num22 == null ? defaultValue : this.Num22.Value);
                    break;
                case 23:
                    molecular = (this.Num23 == null ? defaultValue : this.Num23.Value);
                    break;
                case 24:
                    molecular = (this.Num24 == null ? defaultValue : this.Num24.Value);
                    break;
                case 25:
                    molecular = (this.Num25 == null ? defaultValue : this.Num25.Value);
                    break;
                case 26:
                    molecular = (this.Num26 == null ? defaultValue : this.Num26.Value);
                    break;
                case 27:
                    molecular = (this.Num27 == null ? defaultValue : this.Num27.Value);
                    break;
                case 28:
                    molecular = (this.Num28 == null ? defaultValue : this.Num28.Value);
                    break;
                case 29:
                    molecular = (this.Num29 == null ? defaultValue : this.Num29.Value);
                    break;
                case 30:
                    molecular = (this.Num30 == null ? defaultValue : this.Num30.Value);
                    break;
                case 31:
                    molecular = (this.Num31 == null ? defaultValue : this.Num31.Value);
                    break;
                case 32:
                    molecular = (this.Num32 == null ? defaultValue : this.Num32.Value);
                    break;
                case 33:
                    molecular = (this.Num33 == null ? defaultValue : this.Num33.Value);
                    break;
                case 34:
                    molecular = (this.Num34 == null ? defaultValue : this.Num34.Value);
                    break;
                case 35:
                    molecular = (this.Num35 == null ? defaultValue : this.Num35.Value);
                    break;
                case 36:
                    molecular = (this.Num36 == null ? defaultValue : this.Num36.Value);
                    break;
                case 37:
                    molecular = (this.Num37 == null ? defaultValue : this.Num37.Value);
                    break;
                case 38:
                    molecular = (this.Num38 == null ? defaultValue : this.Num38.Value);
                    break;
                case 39:
                    molecular = (this.Num39 == null ? defaultValue : this.Num39.Value);
                    break;
                case 40:
                    molecular = (this.Num40 == null ? defaultValue : this.Num40.Value);
                    break;
            }

            return molecular;
        }

        public decimal GetLimit(Int32 index)
        {
            decimal limit = 0;
            decimal defaultValue = 0;

            switch (index)
            {
                case 1:
                    limit = (this.Limit01 == null ? defaultValue : this.Limit01.Value);
                    break;
                case 2:
                    limit = (this.Limit02 == null ? defaultValue : this.Limit02.Value);
                    break;
                case 3:
                    limit = (this.Limit03 == null ? defaultValue : this.Limit03.Value);
                    break;
                case 4:
                    limit = (this.Limit04 == null ? defaultValue : this.Limit04.Value);
                    break;
                case 5:
                    limit = (this.Limit05 == null ? defaultValue : this.Limit05.Value);
                    break;
                case 6:
                    limit = (this.Limit06 == null ? defaultValue : this.Limit06.Value);
                    break;
                case 7:
                    limit = (this.Limit07 == null ? defaultValue : this.Limit07.Value);
                    break;
                case 8:
                    limit = (this.Limit08 == null ? defaultValue : this.Limit08.Value);
                    break;
                case 9:
                    limit = (this.Limit09 == null ? defaultValue : this.Limit09.Value);
                    break;
                case 10:
                    limit = (this.Limit10 == null ? defaultValue : this.Limit10.Value);
                    break;
                case 11:
                    limit = (this.Limit11 == null ? defaultValue : this.Limit11.Value);
                    break;
                case 12:
                    limit = (this.Limit12 == null ? defaultValue : this.Limit12.Value);
                    break;
                case 13:
                    limit = (this.Limit13 == null ? defaultValue : this.Limit13.Value);
                    break;
                case 14:
                    limit = (this.Limit14 == null ? defaultValue : this.Limit14.Value);
                    break;
                case 15:
                    limit = (this.Limit15 == null ? defaultValue : this.Limit15.Value);
                    break;
                case 16:
                    limit = (this.Limit16 == null ? defaultValue : this.Limit16.Value);
                    break;
                case 17:
                    limit = (this.Limit17 == null ? defaultValue : this.Limit17.Value);
                    break;
                case 18:
                    limit = (this.Limit18 == null ? defaultValue : this.Limit18.Value);
                    break;
                case 19:
                    limit = (this.Limit19 == null ? defaultValue : this.Limit19.Value);
                    break;
                case 20:
                    limit = (this.Limit20 == null ? defaultValue : this.Limit20.Value);
                    break;
                case 21:
                    limit = (this.Limit21 == null ? defaultValue : this.Limit21.Value);
                    break;
                case 22:
                    limit = (this.Limit22 == null ? defaultValue : this.Limit22.Value);
                    break;
                case 23:
                    limit = (this.Limit23 == null ? defaultValue : this.Limit23.Value);
                    break;
                case 24:
                    limit = (this.Limit24 == null ? defaultValue : this.Limit24.Value);
                    break;
                case 25:
                    limit = (this.Limit25 == null ? defaultValue : this.Limit25.Value);
                    break;
                case 26:
                    limit = (this.Limit26 == null ? defaultValue : this.Limit26.Value);
                    break;
                case 27:
                    limit = (this.Limit27 == null ? defaultValue : this.Limit27.Value);
                    break;
                case 28:
                    limit = (this.Limit28 == null ? defaultValue : this.Limit28.Value);
                    break;
                case 29:
                    limit = (this.Limit29 == null ? defaultValue : this.Limit29.Value);
                    break;
                case 30:
                    limit = (this.Limit30 == null ? defaultValue : this.Limit30.Value);
                    break;
                case 31:
                    limit = (this.Limit31 == null ? defaultValue : this.Limit31.Value);
                    break;
                case 32:
                    limit = (this.Limit32 == null ? defaultValue : this.Limit32.Value);
                    break;
                case 33:
                    limit = (this.Limit33 == null ? defaultValue : this.Limit33.Value);
                    break;
                case 34:
                    limit = (this.Limit34 == null ? defaultValue : this.Limit34.Value);
                    break;
                case 35:
                    limit = (this.Limit35 == null ? defaultValue : this.Limit35.Value);
                    break;
                case 36:
                    limit = (this.Limit36 == null ? defaultValue : this.Limit36.Value);
                    break;
                case 37:
                    limit = (this.Limit37 == null ? defaultValue : this.Limit37.Value);
                    break;
                case 38:
                    limit = (this.Limit38 == null ? defaultValue : this.Limit38.Value);
                    break;
                case 39:
                    limit = (this.Limit39 == null ? defaultValue : this.Limit39.Value);
                    break;
                case 40:
                    limit = (this.Limit40 == null ? defaultValue : this.Limit40.Value);
                    break;
            }
            return limit;
        }

        public decimal GetReduceAmount(Int32 index)
        {
            decimal reduce_amount = 0;
            decimal defaultValue = 0;

            switch (index)
            {
                case 1:
                    reduce_amount = (this.ReduceAmount01 == null ? defaultValue : this.ReduceAmount01.Value);
                    break;
                case 2:
                    reduce_amount = (this.ReduceAmount02 == null ? defaultValue : this.ReduceAmount02.Value);
                    break;
                case 3:
                    reduce_amount = (this.ReduceAmount03 == null ? defaultValue : this.ReduceAmount03.Value);
                    break;
                case 4:
                    reduce_amount = (this.ReduceAmount04 == null ? defaultValue : this.ReduceAmount04.Value);
                    break;
                case 5:
                    reduce_amount = (this.ReduceAmount05 == null ? defaultValue : this.ReduceAmount05.Value);
                    break;
                case 6:
                    reduce_amount = (this.ReduceAmount06 == null ? defaultValue : this.ReduceAmount06.Value);
                    break;
                case 7:
                    reduce_amount = (this.ReduceAmount07 == null ? defaultValue : this.ReduceAmount07.Value);
                    break;
                case 8:
                    reduce_amount = (this.ReduceAmount08 == null ? defaultValue : this.ReduceAmount08.Value);
                    break;
                case 9:
                    reduce_amount = (this.ReduceAmount09 == null ? defaultValue : this.ReduceAmount09.Value);
                    break;
                case 10:
                    reduce_amount = (this.ReduceAmount10 == null ? defaultValue : this.ReduceAmount10.Value);
                    break;
                case 11:
                    reduce_amount = (this.ReduceAmount11 == null ? defaultValue : this.ReduceAmount11.Value);
                    break;
                case 12:
                    reduce_amount = (this.ReduceAmount12 == null ? defaultValue : this.ReduceAmount12.Value);
                    break;
                case 13:
                    reduce_amount = (this.ReduceAmount13 == null ? defaultValue : this.ReduceAmount13.Value);
                    break;
                case 14:
                    reduce_amount = (this.ReduceAmount14 == null ? defaultValue : this.ReduceAmount14.Value);
                    break;
                case 15:
                    reduce_amount = (this.ReduceAmount15 == null ? defaultValue : this.ReduceAmount15.Value);
                    break;
                case 16:
                    reduce_amount = (this.ReduceAmount16 == null ? defaultValue : this.ReduceAmount16.Value);
                    break;
                case 17:
                    reduce_amount = (this.ReduceAmount17 == null ? defaultValue : this.ReduceAmount17.Value);
                    break;
                case 18:
                    reduce_amount = (this.ReduceAmount18 == null ? defaultValue : this.ReduceAmount18.Value);
                    break;
                case 19:
                    reduce_amount = (this.ReduceAmount19 == null ? defaultValue : this.ReduceAmount19.Value);
                    break;
                case 20:
                    reduce_amount = (this.ReduceAmount20 == null ? defaultValue : this.ReduceAmount20.Value);
                    break;
                case 21:
                    reduce_amount = (this.ReduceAmount21 == null ? defaultValue : this.ReduceAmount21.Value);
                    break;
                case 22:
                    reduce_amount = (this.ReduceAmount22 == null ? defaultValue : this.ReduceAmount22.Value);
                    break;
                case 23:
                    reduce_amount = (this.ReduceAmount23 == null ? defaultValue : this.ReduceAmount23.Value);
                    break;
                case 24:
                    reduce_amount = (this.ReduceAmount24 == null ? defaultValue : this.ReduceAmount24.Value);
                    break;
                case 25:
                    reduce_amount = (this.ReduceAmount25 == null ? defaultValue : this.ReduceAmount25.Value);
                    break;
                case 26:
                    reduce_amount = (this.ReduceAmount26 == null ? defaultValue : this.ReduceAmount26.Value);
                    break;
                case 27:
                    reduce_amount = (this.ReduceAmount27 == null ? defaultValue : this.ReduceAmount27.Value);
                    break;
                case 28:
                    reduce_amount = (this.ReduceAmount28 == null ? defaultValue : this.ReduceAmount28.Value);
                    break;
                case 29:
                    reduce_amount = (this.ReduceAmount29 == null ? defaultValue : this.ReduceAmount29.Value);
                    break;
                case 30:
                    reduce_amount = (this.ReduceAmount30 == null ? defaultValue : this.ReduceAmount30.Value);
                    break;
                case 31:
                    reduce_amount = (this.ReduceAmount31 == null ? defaultValue : this.ReduceAmount31.Value);
                    break;
                case 32:
                    reduce_amount = (this.ReduceAmount32 == null ? defaultValue : this.ReduceAmount32.Value);
                    break;
                case 33:
                    reduce_amount = (this.ReduceAmount33 == null ? defaultValue : this.ReduceAmount33.Value);
                    break;
                case 34:
                    reduce_amount = (this.ReduceAmount34 == null ? defaultValue : this.ReduceAmount34.Value);
                    break;
                case 35:
                    reduce_amount = (this.ReduceAmount35 == null ? defaultValue : this.ReduceAmount35.Value);
                    break;
                case 36:
                    reduce_amount = (this.ReduceAmount36 == null ? defaultValue : this.ReduceAmount36.Value);
                    break;
                case 37:
                    reduce_amount = (this.ReduceAmount37 == null ? defaultValue : this.ReduceAmount37.Value);
                    break;
                case 38:
                    reduce_amount = (this.ReduceAmount38 == null ? defaultValue : this.ReduceAmount38.Value);
                    break;
                case 39:
                    reduce_amount = (this.ReduceAmount39 == null ? defaultValue : this.ReduceAmount39.Value);
                    break;
                case 40:
                    reduce_amount = (this.ReduceAmount40 == null ? defaultValue : this.ReduceAmount40.Value);
                    break;
            }

            return reduce_amount;
        }

        public Int32 GetReduceItemByOrder(Int32 orderIndex)
        {
            Int32 index = 0;

            if(this.ReduceOrder1.Trim()!="" && Convert.ToInt32(this.ReduceOrder1.Trim())==orderIndex)
            {
                index = 1;
            }
            if (this.ReduceOrder2.Trim() != "" && Convert.ToInt32(this.ReduceOrder2.Trim()) == orderIndex)
            {
                index = 2;
            }
            if (this.ReduceOrder3.Trim() != "" && Convert.ToInt32(this.ReduceOrder3.Trim()) == orderIndex)
            {
                index = 3;
            }
            if (this.ReduceOrder4.Trim() != "" && Convert.ToInt32(this.ReduceOrder4.Trim()) == orderIndex)
            {
                index = 4;
            }
            if (this.ReduceOrder5.Trim() != "" && Convert.ToInt32(this.ReduceOrder5.Trim()) == orderIndex)
            {
                index = 5;
            }
            if (this.ReduceOrder6.Trim() != "" && Convert.ToInt32(this.ReduceOrder6.Trim()) == orderIndex)
            {
                index = 6;
            }
            if (this.ReduceOrder7.Trim() != "" && Convert.ToInt32(this.ReduceOrder7.Trim()) == orderIndex)
            {
                index = 7;
            }
            if (this.ReduceOrder8.Trim() != "" && Convert.ToInt32(this.ReduceOrder8.Trim()) == orderIndex)
            {
                index = 8;
            }
            if (this.ReduceOrder9.Trim() != "" && Convert.ToInt32(this.ReduceOrder9.Trim()) == orderIndex)
            {
                index = 9;
            }
            if (this.ReduceOrder10.Trim() != "" && Convert.ToInt32(this.ReduceOrder10.Trim()) == orderIndex)
            {
                index = 10;
            }
            if (this.ReduceOrder11.Trim() != "" && Convert.ToInt32(this.ReduceOrder11.Trim()) == orderIndex)
            {
                index = 11;
            }
            if (this.ReduceOrder12.Trim() != "" && Convert.ToInt32(this.ReduceOrder12.Trim()) == orderIndex)
            {
                index = 12;
            }
            if (this.ReduceOrder13.Trim() != "" && Convert.ToInt32(this.ReduceOrder13.Trim()) == orderIndex)
            {
                index = 13;
            }
            if (this.ReduceOrder14.Trim() != "" && Convert.ToInt32(this.ReduceOrder14.Trim()) == orderIndex)
            {
                index = 14;
            }
            if (this.ReduceOrder15.Trim() != "" && Convert.ToInt32(this.ReduceOrder15.Trim()) == orderIndex)
            {
                index = 15;
            }
            if (this.ReduceOrder16.Trim() != "" && Convert.ToInt32(this.ReduceOrder16.Trim()) == orderIndex)
            {
                index = 16;
            }
            if (this.ReduceOrder17.Trim() != "" && Convert.ToInt32(this.ReduceOrder17.Trim()) == orderIndex)
            {
                index = 17;
            }
            if (this.ReduceOrder18.Trim() != "" && Convert.ToInt32(this.ReduceOrder18.Trim()) == orderIndex)
            {
                index = 18;
            }
            if (this.ReduceOrder19.Trim() != "" && Convert.ToInt32(this.ReduceOrder19.Trim()) == orderIndex)
            {
                index = 19;
            }
            if (this.ReduceOrder20.Trim() != "" && Convert.ToInt32(this.ReduceOrder20.Trim()) == orderIndex)
            {
                index = 20;
            }
            if (this.ReduceOrder21.Trim() != "" && Convert.ToInt32(this.ReduceOrder21.Trim()) == orderIndex)
            {
                index = 21;
            }
            if (this.ReduceOrder22.Trim() != "" && Convert.ToInt32(this.ReduceOrder22.Trim()) == orderIndex)
            {
                index = 22;
            }
            if (this.ReduceOrder23.Trim() != "" && Convert.ToInt32(this.ReduceOrder23.Trim()) == orderIndex)
            {
                index = 23;
            }
            if (this.ReduceOrder24.Trim() != "" && Convert.ToInt32(this.ReduceOrder24.Trim()) == orderIndex)
            {
                index = 24;
            }
            if (this.ReduceOrder25.Trim() != "" && Convert.ToInt32(this.ReduceOrder25.Trim()) == orderIndex)
            {
                index = 25;
            }
            if (this.ReduceOrder26.Trim() != "" && Convert.ToInt32(this.ReduceOrder26.Trim()) == orderIndex)
            {
                index = 26;
            }
            if (this.ReduceOrder27.Trim() != "" && Convert.ToInt32(this.ReduceOrder27.Trim()) == orderIndex)
            {
                index = 27;
            }
            if (this.ReduceOrder28.Trim() != "" && Convert.ToInt32(this.ReduceOrder28.Trim()) == orderIndex)
            {
                index = 28;
            }
            if (this.ReduceOrder29.Trim() != "" && Convert.ToInt32(this.ReduceOrder29.Trim()) == orderIndex)
            {
                index = 29;
            }
            if (this.ReduceOrder30.Trim() != "" && Convert.ToInt32(this.ReduceOrder30.Trim()) == orderIndex)
            {
                index = 30;
            }
            if (this.ReduceOrder31.Trim() != "" && Convert.ToInt32(this.ReduceOrder31.Trim()) == orderIndex)
            {
                index = 31;
            }
            if (this.ReduceOrder32.Trim() != "" && Convert.ToInt32(this.ReduceOrder32.Trim()) == orderIndex)
            {
                index = 32;
            }
            if (this.ReduceOrder33.Trim() != "" && Convert.ToInt32(this.ReduceOrder33.Trim()) == orderIndex)
            {
                index = 33;
            }
            if (this.ReduceOrder34.Trim() != "" && Convert.ToInt32(this.ReduceOrder34.Trim()) == orderIndex)
            {
                index = 34;
            }
            if (this.ReduceOrder35.Trim() != "" && Convert.ToInt32(this.ReduceOrder35.Trim()) == orderIndex)
            {
                index = 35;
            }
            if (this.ReduceOrder36.Trim() != "" && Convert.ToInt32(this.ReduceOrder36.Trim()) == orderIndex)
            {
                index = 36;
            }
            if (this.ReduceOrder37.Trim() != "" && Convert.ToInt32(this.ReduceOrder37.Trim()) == orderIndex)
            {
                index = 37;
            }
            if (this.ReduceOrder38.Trim() != "" && Convert.ToInt32(this.ReduceOrder38.Trim()) == orderIndex)
            {
                index = 38;
            }
            if (this.ReduceOrder39.Trim() != "" && Convert.ToInt32(this.ReduceOrder39.Trim()) == orderIndex)
            {
                index = 39;
            }
            if (this.ReduceOrder40.Trim() != "" && Convert.ToInt32(this.ReduceOrder40.Trim()) == orderIndex)
            {
                index = 40;
            }

            return index;
        }
        #endregion
    }
}
